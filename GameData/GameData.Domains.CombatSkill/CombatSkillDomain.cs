using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Dependencies;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.CombatSkill;

[GameDataDomain(7)]
public class CombatSkillDomain : BaseGameDataDomain
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[DomainData(DomainDataType.ObjectCollection, true, false, true, true, CollectionCapacity = 8192)]
	private readonly CombatSkillCollection _combatSkills;

	private static readonly Dictionary<short, CombatSkill> EmptyCharCombatSkills = new Dictionary<short, CombatSkill>();

	public static short[][] EquipAddPropertyDict;

	private static List<CombatSkillItem>[][] _learnableCombatSkillsCache;

	public const byte MaxCostTrickTypeCount = 3;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[1][];

	private static readonly DataInfluence[][] CacheInfluencesCombatSkills = new DataInfluence[29][];

	private readonly ObjectCollectionDataStates _dataStatesCombatSkills = new ObjectCollectionDataStates(29, 8192);

	public readonly ObjectCollectionHelperData HelperDataCombatSkills;

	private Queue<uint> _pendingLoadingOperationIds;

	private void OnInitializedDomainData()
	{
	}

	private void InitializeOnInitializeGameDataModule()
	{
		EquipAddPropertyDict = new short[Config.CombatSkill.Instance.Count][];
		for (short num = 0; num < Config.CombatSkill.Instance.Count; num++)
		{
			List<PropertyAndValue> propertyAddList = Config.CombatSkill.Instance[num].PropertyAddList;
			if (propertyAddList != null && propertyAddList.Count > 0)
			{
				short[] array = new short[112];
				Array.Clear(array, 0, array.Length);
				foreach (PropertyAndValue item in propertyAddList)
				{
					array[item.PropertyId] = item.Value;
				}
				EquipAddPropertyDict[num] = array;
			}
		}
		InitializeLearnableCombatSkillTemplateIds();
	}

	private void InitializeOnEnterNewWorld()
	{
	}

	private void OnLoadedArchiveData()
	{
	}

	public override void FixAbnormalDomainArchiveData(DataContext context)
	{
		FixInvalidActivateState(context);
	}

	private void FixInvalidActivateState(DataContext context)
	{
		if (!DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 76, 19) || !DomainManager.World.IsCurrWorldAfterVersion(0, 0, 76))
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<CombatSkillKey, CombatSkill> combatSkill2 in _combatSkills)
		{
			combatSkill2.Deconstruct(out var key, out var value);
			CombatSkillKey combatSkillKey = key;
			CombatSkill combatSkill = value;
			ushort activationState = combatSkill.GetActivationState();
			if (CombatSkillStateHelper.IsBrokenOut(activationState) && CombatSkillStateHelper.GetNormalPagesActivationCount(activationState) < 5)
			{
				combatSkill.SetActivationState(0, context);
				num++;
			}
		}
		if (num > 0)
		{
			Logger.Warn($"Un-breaking {num} combat skills.");
		}
	}

	private static void InitializeLearnableCombatSkillTemplateIds()
	{
		_learnableCombatSkillsCache = new List<CombatSkillItem>[16][];
		for (int i = 0; i < _learnableCombatSkillsCache.Length; i++)
		{
			_learnableCombatSkillsCache[i] = new List<CombatSkillItem>[14];
			for (int j = 0; j < 14; j++)
			{
				_learnableCombatSkillsCache[i][j] = new List<CombatSkillItem>();
			}
		}
		foreach (CombatSkillItem item in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
		{
			if (item.BookId >= 0)
			{
				_learnableCombatSkillsCache[item.SectId][item.Type].Add(item);
			}
		}
	}

	public static sbyte GetCombatSkillGradeGroup(sbyte grade, sbyte orgTemplateId, sbyte combatSkillType)
	{
		int count = GetLearnableCombatSkills(orgTemplateId, combatSkillType).Count;
		if (count > 6)
		{
			return Grade.GetGroup(grade);
		}
		int num = count / 3;
		int num2 = count % 3;
		int num3 = num + ((num2 > 0) ? 1 : 0);
		int num4 = num3 + num + ((num2 > 1) ? 1 : 0);
		return (sbyte)((grade >= num4) ? 2 : ((grade >= num3) ? 1 : 0));
	}

	public static IReadOnlyList<IReadOnlyList<CombatSkillItem>> GetLearnableCombatSkills(sbyte orgTemplateId)
	{
		return _learnableCombatSkillsCache[orgTemplateId];
	}

	public static IReadOnlyList<CombatSkillItem> GetLearnableCombatSkills(sbyte orgTemplateId, sbyte combatSkillType)
	{
		return _learnableCombatSkillsCache[orgTemplateId][combatSkillType];
	}

	[DomainMethod]
	public bool SetActivePage(DataContext context, int charId, short skillId, byte pageId, sbyte direction)
	{
		if ((pageId <= 0 || pageId >= 6) ? true : false)
		{
			return false;
		}
		if ((uint)direction > 1u)
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return false;
		}
		if (!TryGetElement_CombatSkills((charId: charId, skillId: skillId), out var element2))
		{
			return false;
		}
		bool flag = direction == 0;
		byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(-1, 0, pageId);
		ushort readingState = element2.GetReadingState();
		if (flag && !CombatSkillStateHelper.IsPageRead(readingState, pageInternalIndex))
		{
			return false;
		}
		bool flag2 = direction == 1;
		byte pageInternalIndex2 = CombatSkillStateHelper.GetPageInternalIndex(-1, 1, pageId);
		if (flag2 && !CombatSkillStateHelper.IsPageRead(readingState, pageInternalIndex2))
		{
			return false;
		}
		ushort activationState = element2.GetActivationState();
		sbyte combatSkillDirection = CombatSkillStateHelper.GetCombatSkillDirection(activationState);
		bool flag3 = element.IsTaiwu();
		activationState = CombatSkillStateHelper.SetPageInactive(activationState, pageInternalIndex);
		activationState = CombatSkillStateHelper.SetPageInactive(activationState, pageInternalIndex2);
		if (flag)
		{
			activationState = CombatSkillStateHelper.SetPageActive(activationState, pageInternalIndex);
		}
		if (flag2)
		{
			activationState = CombatSkillStateHelper.SetPageActive(activationState, pageInternalIndex2);
		}
		if (element2.GetActivationState() == activationState)
		{
			return false;
		}
		bool flag4 = CombatSkillStateHelper.IsBrokenOut(activationState);
		if (flag3 && flag4 && !DomainManager.Taiwu.UpdateBreakPlateSelectedPages(context, skillId, activationState))
		{
			return false;
		}
		element2.SetActivationState(activationState, context);
		sbyte combatSkillDirection2 = CombatSkillStateHelper.GetCombatSkillDirection(activationState);
		if (flag3 && combatSkillDirection == 0 && combatSkillDirection2 == 1 && Config.CombatSkill.Instance[skillId].SectId == 4)
		{
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.SectMainStoryWudangStart, skillId);
		}
		if (!element.IsCombatSkillEquipped(skillId))
		{
			return true;
		}
		DomainManager.SpecialEffect.Remove(context, charId, skillId, 2);
		DomainManager.SpecialEffect.Add(context, charId, skillId, 2, -1);
		return true;
	}

	[DomainMethod]
	public bool DeActivePage(DataContext context, int charId, short skillId, byte pageId, sbyte direction)
	{
		if ((pageId <= 0 || pageId >= 6) ? true : false)
		{
			return false;
		}
		if ((uint)direction > 1u)
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return false;
		}
		if (!TryGetElement_CombatSkills((charId: charId, skillId: skillId), out var element2))
		{
			return false;
		}
		bool flag = direction == 0;
		byte normalPageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(0, pageId);
		ushort readingState = element2.GetReadingState();
		if (flag && !CombatSkillStateHelper.IsPageRead(readingState, normalPageInternalIndex))
		{
			return false;
		}
		bool flag2 = direction == 1;
		byte normalPageInternalIndex2 = CombatSkillStateHelper.GetNormalPageInternalIndex(1, pageId);
		if (flag2 && !CombatSkillStateHelper.IsPageRead(readingState, normalPageInternalIndex2))
		{
			return false;
		}
		ushort activationState = element2.GetActivationState();
		if (CombatSkillStateHelper.IsBrokenOut(activationState))
		{
			return false;
		}
		activationState = CombatSkillStateHelper.SetPageInactive(activationState, normalPageInternalIndex);
		activationState = CombatSkillStateHelper.SetPageInactive(activationState, normalPageInternalIndex2);
		if (element2.GetActivationState() == activationState)
		{
			return false;
		}
		element2.SetActivationState(activationState, context);
		if (!element.IsCombatSkillEquipped(skillId))
		{
			return true;
		}
		DomainManager.SpecialEffect.Remove(context, charId, skillId, 2);
		DomainManager.SpecialEffect.Add(context, charId, skillId, 2, -1);
		return true;
	}

	public bool TryActivateCombatSkillBookPageWhenSetReadingState(DataContext context, int charId, short combatSkillTemplateId, byte pageInternalIndex)
	{
		if (pageInternalIndex < 5)
		{
			return false;
		}
		if (pageInternalIndex >= 15)
		{
			return false;
		}
		CombatSkillKey objectId = new CombatSkillKey(charId, combatSkillTemplateId);
		if (!TryGetElement_CombatSkills(objectId, out var element))
		{
			return false;
		}
		byte pageId = CombatSkillStateHelper.GetPageId(pageInternalIndex);
		ushort readingState = element.GetReadingState();
		if (!CombatSkillStateHelper.IsPageRead(readingState, pageInternalIndex))
		{
			return false;
		}
		ushort activationState = element.GetActivationState();
		if (CombatSkillStateHelper.IsPageActive(activationState, pageInternalIndex))
		{
			return false;
		}
		byte normalPageOppositeInternalIndex = CombatSkillStateHelper.GetNormalPageOppositeInternalIndex(pageInternalIndex);
		if (CombatSkillStateHelper.IsPageActive(activationState, normalPageOppositeInternalIndex))
		{
			return false;
		}
		sbyte direction = ((pageInternalIndex >= 10) ? ((sbyte)1) : ((sbyte)0));
		SetActivePage(context, charId, combatSkillTemplateId, pageId, direction);
		return true;
	}

	public Dictionary<short, CombatSkill> GetCharCombatSkills(int charId)
	{
		Dictionary<short, CombatSkill> value;
		return _combatSkills.Collection.TryGetValue(charId, out value) ? value : EmptyCharCombatSkills;
	}

	public static (short neili, short qiDisorder, int[] extraNeiliAllocationProgress) CalcNeigongLoopingEffect(IRandomSource random, GameData.Domains.Character.Character character, CombatSkillItem skillCfg, bool includeReference = true)
	{
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		(short, int, ItemKey) value;
		bool flag = DomainManager.Building.IsCharacterParticipantFeast(character.GetId(), out value);
		sbyte neiliType = character.GetNeiliType();
		byte fiveElements = NeiliType.Instance[neiliType].FiveElements;
		(int benefit, short qiDisorder) tuple = CalcNeigongLoopingEffect_GetBenefitAndQiDisorder(fiveElements, character.GetId(), skillCfg);
		int num = tuple.benefit;
		short num2 = tuple.qiDisorder;
		ConsummateLevelItem consummateLevelItem = ConsummateLevel.Instance[(!character.IsLoseConsummateBonusByFeature()) ? character.GetConsummateLevel() : 0];
		CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillCfg.TemplateId));
		int num3 = skillCfg.ObtainedNeiliPerLoop + element_CombatSkills.GetBreakoutGridCombatSkillPropertyBonus(7);
		bool flag2 = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
		if (flag2 && includeReference)
		{
			short loopingNeigong = character.GetLoopingNeigong();
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[loopingNeigong];
			num += GetTaiwuReferenceSkillNeiliBonus(skillConfig);
			num += DomainManager.Taiwu.GetQiArtStrategyDeltaNeiliBonus(random);
			num += CalcTaiwuProfessionNeigongLoopingEffectBonus();
		}
		num += consummateLevelItem.LoopingNeiliBonus;
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		if (!flag2 && organizationInfo.SettlementId >= 0 && OrganizationDomain.IsSect(organizationInfo.OrgTemplateId))
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(organizationInfo.SettlementId);
			num = num * settlement.GetMemberSelfImproveSpeedFactor() / 100;
		}
		if (flag)
		{
			FeastItem feastItem = Feast.Instance[value.Item1];
			num += num * feastItem.Loop / 100 * value.Item2 / 100;
		}
		int num4 = num3 * num / 100;
		int num5 = num4 * 3 / 4;
		int num6 = num4 * 5 / 4;
		num4 = random.Next(num5, num6 + 1);
		if (num2 > 0)
		{
			num2 = (short)random.Next(num2 + 1);
		}
		else if (num2 < 0)
		{
			num2 = (short)(-random.Next(-num2 + 1));
		}
		if (flag)
		{
			FeastItem feastItem2 = Feast.Instance[value.Item1];
			num2 += (short)(num2 * feastItem2.Loop / 100 * value.Item2 / 100);
		}
		sbyte[] extraNeiliAllocationProgress = skillCfg.ExtraNeiliAllocationProgress;
		int[] array = RandomUtils.DistributeNIntoKBuckets(random, extraNeiliAllocationProgress[4], 4);
		int[] array2 = new int[4];
		int num7 = 0;
		if (flag2)
		{
			num7 += GetTaiwuReferenceSkillNeiliAllocationBonus(skillCfg);
			num7 += DomainManager.Taiwu.GetQiArtStrategyExtraNeiliAllocationBonus(random, skillCfg.TemplateId);
			num7 += CalcTaiwuProfessionNeigongLoopingEffectBonus();
		}
		num7 += consummateLevelItem.LoopingNeiliAllocationBonus;
		if (flag)
		{
			FeastItem feastItem3 = Feast.Instance[value.Item1];
			num7 += feastItem3.Loop / 100 * value.Item2;
		}
		for (int i = 0; i < 4; i++)
		{
			array2[i] = 100 * (extraNeiliAllocationProgress[i] + array[i]);
			ref int reference = ref array2[i];
			reference *= CValuePercentBonus.op_Implicit(num7);
		}
		return (neili: (short)num4, qiDisorder: num2, extraNeiliAllocationProgress: array2);
	}

	private static int CalcTaiwuProfessionNeigongLoopingEffectBonus()
	{
		if (!DomainManager.Extra.IsProfessionalSkillUnlocked(3, 1))
		{
			return 0;
		}
		int seniority = DomainManager.Extra.GetProfessionData(3).Seniority;
		int num = 3000000;
		return 50 + 50 * seniority / num;
	}

	[DomainMethod]
	public (int minNeili, int maxNeili) CalcTaiwuExtraDeltaNeiliPerLoop(DataContext context)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		if (loopingNeigong < 0)
		{
			return (minNeili: 0, maxNeili: 0);
		}
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[loopingNeigong];
		sbyte neiliType = taiwu.GetNeiliType();
		byte fiveElements = NeiliType.Instance[neiliType].FiveElements;
		(int benefit, short qiDisorder) tuple = CalcNeigongLoopingEffect_GetBenefitAndQiDisorder(fiveElements, taiwu.GetId(), combatSkillItem);
		int item = tuple.benefit;
		short item2 = tuple.qiDisorder;
		ConsummateLevelItem consummateLevelItem = ConsummateLevel.Instance[(!taiwu.IsLoseConsummateBonusByFeature()) ? taiwu.GetConsummateLevel() : 0];
		CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(taiwu.GetId(), combatSkillItem.TemplateId));
		int num = combatSkillItem.ObtainedNeiliPerLoop + element_CombatSkills.GetBreakoutGridCombatSkillPropertyBonus(7);
		int num2 = item;
		int num3 = item;
		int taiwuReferenceSkillNeiliBonus = GetTaiwuReferenceSkillNeiliBonus(combatSkillItem);
		num3 += taiwuReferenceSkillNeiliBonus;
		num2 += taiwuReferenceSkillNeiliBonus;
		(int, int) qiArtStrategyDeltaNeiliBonusRange = DomainManager.Taiwu.GetQiArtStrategyDeltaNeiliBonusRange();
		num3 += qiArtStrategyDeltaNeiliBonusRange.Item1;
		num2 += qiArtStrategyDeltaNeiliBonusRange.Item2;
		int num4 = CalcTaiwuProfessionNeigongLoopingEffectBonus();
		num3 += num4;
		num2 += num4;
		num3 += consummateLevelItem.LoopingNeiliBonus;
		num2 += consummateLevelItem.LoopingNeiliBonus;
		int num5 = num * num3 / 100;
		int num6 = num * num2 / 100;
		int num7 = num5 * 3 / 4;
		int num8 = num6 * 5 / 4;
		byte loopingDifficulty = DomainManager.Extra.GetLoopingDifficulty();
		short num9 = WorldCreation.Instance[(byte)10].InfluenceFactors[loopingDifficulty];
		num7 = num7 * num9 / 100;
		num8 = num8 * num9 / 100;
		return (minNeili: num7 - combatSkillItem.ObtainedNeiliPerLoop, maxNeili: num8 - combatSkillItem.ObtainedNeiliPerLoop);
	}

	[DomainMethod]
	public IntList CalcTaiwuExtraDeltaNeiliAllocationPerLoop(DataContext context)
	{
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		int[] array = new int[4];
		int[] array2 = new int[4];
		IntList result = IntList.Create();
		for (int i = 0; i < 8; i++)
		{
			result.Items.Add(0);
		}
		if (loopingNeigong < 0)
		{
			return result;
		}
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[loopingNeigong];
		sbyte[] extraNeiliAllocationProgress = combatSkillItem.ExtraNeiliAllocationProgress;
		for (int j = 0; j < 4; j++)
		{
			array[j] = 100 * extraNeiliAllocationProgress[j];
			array2[j] = 100 * (extraNeiliAllocationProgress[j] + extraNeiliAllocationProgress[4]);
		}
		int num = 0;
		int num2 = 0;
		short loopingNeigong2 = taiwu.GetLoopingNeigong();
		int taiwuReferenceSkillNeiliAllocationBonus = GetTaiwuReferenceSkillNeiliAllocationBonus(combatSkillItem);
		num += taiwuReferenceSkillNeiliAllocationBonus;
		num2 += taiwuReferenceSkillNeiliAllocationBonus;
		(int, int) qiArtStrategyExtraNeiliAllocationBonusRange = DomainManager.Taiwu.GetQiArtStrategyExtraNeiliAllocationBonusRange();
		num += qiArtStrategyExtraNeiliAllocationBonusRange.Item1;
		num2 += qiArtStrategyExtraNeiliAllocationBonusRange.Item2;
		int num3 = CalcTaiwuProfessionNeigongLoopingEffectBonus();
		num += num3;
		num2 += num3;
		ConsummateLevelItem consummateLevelItem = ConsummateLevel.Instance[(!taiwu.IsLoseConsummateBonusByFeature()) ? taiwu.GetConsummateLevel() : 0];
		num += consummateLevelItem.LoopingNeiliAllocationBonus;
		num2 += consummateLevelItem.LoopingNeiliAllocationBonus;
		for (int k = 0; k < 4; k++)
		{
			ref int reference = ref array[k];
			reference *= CValuePercentBonus.op_Implicit(num);
			ref int reference2 = ref array2[k];
			reference2 *= CValuePercentBonus.op_Implicit(num2);
			array[k] -= extraNeiliAllocationProgress[k] * 100;
			array2[k] -= extraNeiliAllocationProgress[k] * 100;
		}
		for (int l = 0; l < 4; l++)
		{
			result.Items[l] = array[l];
		}
		for (int m = 0; m < 4; m++)
		{
			result.Items[m + 4] = array2[m];
		}
		return result;
	}

	private static int GetTaiwuReferenceSkillNeiliBonus(CombatSkillItem skillConfig)
	{
		List<short> referenceSkillList = DomainManager.Extra.GetReferenceSkillList();
		int num = 0;
		if (referenceSkillList != null && referenceSkillList.Count > 0)
		{
			for (int i = 0; i < referenceSkillList.Count; i++)
			{
				short num2 = referenceSkillList[i];
				if (skillConfig.LoopBonusSkillList.Contains(num2))
				{
					num += 10;
				}
				if (num2 != -1)
				{
					CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[num2];
					if (combatSkillItem.SectId == skillConfig.SectId)
					{
						num += 20;
					}
				}
			}
		}
		return num;
	}

	private static int GetTaiwuReferenceSkillNeiliAllocationBonus(CombatSkillItem skillConfig)
	{
		int num = 0;
		if (skillConfig == null)
		{
			return num;
		}
		List<short> referenceSkillList = DomainManager.Extra.GetReferenceSkillList();
		if (referenceSkillList != null && referenceSkillList.Count > 0)
		{
			for (int i = 0; i < referenceSkillList.Count; i++)
			{
				short num2 = referenceSkillList[i];
				if (skillConfig.LoopBonusSkillList.Contains(num2))
				{
					num += 10;
				}
				if (num2 != -1)
				{
					CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[num2];
					if (combatSkillItem.SectId == skillConfig.SectId)
					{
						num += 20;
					}
				}
			}
		}
		return num;
	}

	public void ApplyNeigongLoopingEffect(DataContext context, GameData.Domains.Character.Character character, short combatSkillTemplateId, short obtainedNeili, int[] extraNeiliAllocationProgress)
	{
		int id = character.GetId();
		CombatSkillKey key = new CombatSkillKey(id, combatSkillTemplateId);
		CombatSkill combatSkill = _combatSkills[key];
		ApplyLoopingNeiliModify(context, character, obtainedNeili, combatSkill);
		ApplyLoopingTransferNeiliProportionOfFiveElements(context, character, combatSkillTemplateId, combatSkill);
		ApplyLoopingExtraNeiliAllocationProgressModify(context, character, extraNeiliAllocationProgress);
	}

	public void ApplyLoopingNeiliModify(DataContext context, GameData.Domains.Character.Character character, short obtainedNeili, CombatSkill skill)
	{
		short obtainedNeili2 = skill.GetObtainedNeili();
		skill.ObtainNeili(context, obtainedNeili);
		int num = skill.GetObtainedNeili() - obtainedNeili2;
		if (num > 0)
		{
			int num2 = character.GetCurrNeili() + num;
			int maxNeili = character.GetMaxNeili();
			if (num2 > maxNeili)
			{
				num2 = maxNeili;
			}
			character.SetCurrNeili(num2, context);
		}
	}

	public void ApplyLoopingNeiliModifyForTaiwu(DataContext context, GameData.Domains.Character.Character taiwuChar, short obtainedNeili)
	{
		int id = taiwuChar.GetId();
		short loopingNeigong = taiwuChar.GetLoopingNeigong();
		CombatSkillKey key = new CombatSkillKey(id, loopingNeigong);
		CombatSkill skill = _combatSkills[key];
		ApplyLoopingNeiliModify(context, taiwuChar, obtainedNeili, skill);
	}

	public void ApplyLoopingExtraNeiliAllocationProgressModify(DataContext context, GameData.Domains.Character.Character character, int[] extraNeiliAllocationProgress)
	{
		int id = character.GetId();
		IntList orInitExtraNeiliAllocationProgress = DomainManager.Extra.GetOrInitExtraNeiliAllocationProgress(context, id);
		NeiliAllocation delta = default(NeiliAllocation);
		for (int i = 0; i < 4; i++)
		{
			int num = orInitExtraNeiliAllocationProgress.Items[i];
			int neiliAllocationMaxProgress = GetNeiliAllocationMaxProgress();
			int num2 = extraNeiliAllocationProgress[i];
			if (num2 > 0 && num >= neiliAllocationMaxProgress)
			{
				num2 = 0;
			}
			orInitExtraNeiliAllocationProgress.Items[i] = Math.Max(0, num + num2);
			int newProgress = orInitExtraNeiliAllocationProgress.Items[i];
			int num3 = CalculateDeltaNeiliAllocation(num, newProgress);
			delta[i] = (short)num3;
		}
		DomainManager.Extra.SetExtraNeiliAllocationProgress(context, id, orInitExtraNeiliAllocationProgress);
		character.ChangeExtraNeiliAllocation(context, delta);
	}

	private static void ApplyLoopingTransferNeiliProportionOfFiveElements(DataContext context, GameData.Domains.Character.Character character, short combatSkillTemplateId, CombatSkill loopingSkill)
	{
		var (destType, b, b2) = GetLoopingTransferNeiliProportionOfFiveElementsData(context, character, combatSkillTemplateId, loopingSkill);
		if (b >= 0 && b2 > 0)
		{
			character.TransferNeiliProportionOfFiveElements(context, destType, b, b2);
		}
	}

	private static (sbyte destinationType, sbyte transferType, sbyte amount) GetLoopingTransferNeiliProportionOfFiveElementsData(DataContext context, GameData.Domains.Character.Character character, short combatSkillTemplateId, CombatSkill loopingSkill)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkillTemplateId];
		sbyte b = loopingSkill.GetFiveElementsChange();
		sbyte item = combatSkillItem.DestTypeWhileLooping;
		sbyte item2 = combatSkillItem.TransferTypeWhileLooping;
		if (character.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
		{
			var (b2, b3) = DomainManager.Taiwu.GetOverrideFiveElementTransferInfo();
			if (b2 > -1)
			{
				item = b2;
				item2 = b3;
			}
			short anchoredFiveElements = DomainManager.Taiwu.GetAnchoredFiveElements();
			sbyte neiliType = character.GetNeiliType();
			NeiliTypeItem neiliTypeItem = NeiliType.Instance[neiliType];
			bool flag = neiliTypeItem.ColorType == 2;
			if (anchoredFiveElements > -1 && !flag && neiliTypeItem.FiveElements == anchoredFiveElements)
			{
				item = -1;
				item2 = -1;
			}
			int num = 0;
			num = DomainManager.Taiwu.GetQiArtStrategyFiveElementTransferAmountBonus(context.Random);
			b = (sbyte)(b * (100 + num) / 100);
		}
		return (destinationType: item, transferType: item2, amount: b);
	}

	public (sbyte destinationType, sbyte transferType, sbyte amount) GetLoopingTransferNeiliProportionOfFiveElementsDataForTaiwu(DataContext context, GameData.Domains.Character.Character taiwuChar)
	{
		int id = taiwuChar.GetId();
		short loopingNeigong = taiwuChar.GetLoopingNeigong();
		CombatSkillKey key = new CombatSkillKey(id, loopingNeigong);
		CombatSkill loopingSkill = _combatSkills[key];
		return GetLoopingTransferNeiliProportionOfFiveElementsData(context, taiwuChar, loopingNeigong, loopingSkill);
	}

	private static int CalculateDeltaNeiliAllocation(int currentProgress, int newProgress)
	{
		if (newProgress > currentProgress)
		{
			return GetNeiliAllocationProgressMilestoneCount(currentProgress, newProgress);
		}
		return -GetNeiliAllocationProgressMilestoneCount(newProgress, currentProgress);
	}

	[Obsolete]
	private static List<int> GenerateNeiliAllocationProgressMinestones(int currentProgress, int newProgress)
	{
		int num = GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * 100;
		int num2 = GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatioGrowth * 100;
		int num3 = 0;
		int num4 = num;
		List<int> list = new List<int>();
		while (num3 <= newProgress)
		{
			if (num3 > currentProgress)
			{
				list.Add(num3);
			}
			num3 += num4;
			num4 += num2;
		}
		return list;
	}

	private static int GetNeiliAllocationProgressMilestoneCount(int currentProgress, int newProgress)
	{
		int num = GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * 100;
		int num2 = GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatioGrowth * 100;
		int num3 = 0;
		int num4 = num;
		int num5 = 0;
		while (num3 <= newProgress)
		{
			if (num3 > currentProgress)
			{
				num5++;
			}
			num3 += num4;
			num4 += num2;
		}
		return num5;
	}

	private static int GetNeiliAllocationProgressMilestoneByIndex(int currentProgress, int newProgress, int index)
	{
		int num = GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * 100;
		int num2 = GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatioGrowth * 100;
		int num3 = 0;
		int num4 = num;
		int num5 = -1;
		while (num3 <= newProgress)
		{
			if (num3 > currentProgress)
			{
				num5++;
				if (num5 == index)
				{
					return num3;
				}
			}
			num3 += num4;
			num4 += num2;
		}
		throw new ArgumentOutOfRangeException("index", "下标超出了可用进度节点的范围");
	}

	public static int GetExtraNeiliAllocationByProgress(int currentProgress)
	{
		int num = GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * 100;
		int num2 = GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatioGrowth * 100;
		int num3 = 0;
		int num4 = num;
		int num5 = 0;
		while (num3 <= currentProgress)
		{
			if (num3 > 0)
			{
				num5++;
			}
			num3 += num4;
			num4 += num2;
		}
		return num5;
	}

	public static int GetNeiliAllocationMaxProgress()
	{
		short maxExtraNeiliAllocation = GlobalConfig.Instance.MaxExtraNeiliAllocation;
		return GetExtraNeiliAllocationProgressByExtraNeiliAllocation(maxExtraNeiliAllocation);
	}

	public static int GetExtraNeiliAllocationProgressByExtraNeiliAllocation(int extraNeiliAllocation)
	{
		int num = GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * 100;
		int num2 = GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatioGrowth * 100;
		return num * extraNeiliAllocation + num2 * (extraNeiliAllocation * (extraNeiliAllocation - 1)) / 2;
	}

	public unsafe void SetCharacterExtraNeiliAllocationAndProgress(DataContext context, GameData.Domains.Character.Character character, IntList extraNeiliAllocationProgress, bool canOverMax = false)
	{
		int neiliAllocationMaxProgress = GetNeiliAllocationMaxProgress();
		NeiliAllocation extraNeiliAllocation = default(NeiliAllocation);
		extraNeiliAllocation.Initialize();
		for (int i = 0; i < 4; i++)
		{
			int currentProgress = (canOverMax ? extraNeiliAllocationProgress.Items[i] : Math.Clamp(extraNeiliAllocationProgress.Items[i], 0, neiliAllocationMaxProgress));
			extraNeiliAllocation.Items[i] = (short)GetExtraNeiliAllocationByProgress(currentProgress);
		}
		DomainManager.Extra.SetExtraNeiliAllocationProgress(context, character.GetId(), extraNeiliAllocationProgress);
		character.SetExtraNeiliAllocation(extraNeiliAllocation, context);
	}

	public unsafe short AddExtraNeiliAllocationProgressToGainExtraNeiliAllocation(DataContext context, GameData.Domains.Character.Character character, byte neiliAllocationType, int deltaNeiliAllocation, bool allowOverMax = false)
	{
		int newProgress = GetNeiliAllocationMaxProgress();
		if (allowOverMax)
		{
			newProgress = int.MaxValue;
		}
		IntList orInitExtraNeiliAllocationProgress = DomainManager.Extra.GetOrInitExtraNeiliAllocationProgress(context, character.GetId());
		int neiliAllocationProgressMilestoneCount = GetNeiliAllocationProgressMilestoneCount(orInitExtraNeiliAllocationProgress.Items[neiliAllocationType], newProgress);
		int num = Math.Min(neiliAllocationProgressMilestoneCount - 1, deltaNeiliAllocation - 1);
		int neiliAllocationProgressMilestoneByIndex = GetNeiliAllocationProgressMilestoneByIndex(orInitExtraNeiliAllocationProgress.Items[neiliAllocationType], newProgress, num);
		NeiliAllocation extraNeiliAllocation = character.GetExtraNeiliAllocation();
		orInitExtraNeiliAllocationProgress.Items[neiliAllocationType] = neiliAllocationProgressMilestoneByIndex;
		short num2 = (short)(num + 1);
		ref short reference = ref extraNeiliAllocation.Items[(int)neiliAllocationType];
		reference += num2;
		character.SetExtraNeiliAllocation(extraNeiliAllocation, context);
		DomainManager.Extra.SetExtraNeiliAllocationProgress(context, character.GetId(), orInitExtraNeiliAllocationProgress);
		return num2;
	}

	public sbyte GetSkillDirection(int charId, short skillId)
	{
		return GetElement_CombatSkills(new CombatSkillKey(charId, skillId)).GetDirection();
	}

	public sbyte GetSkillType(int charId, short skillId)
	{
		sbyte type = Config.CombatSkill.Instance[skillId].Type;
		return (sbyte)DomainManager.SpecialEffect.ModifyData(charId, skillId, (ushort)221, (int)type, -1, -1, -1);
	}

	private static (int benefit, short qiDisorder) CalcNeigongLoopingEffect_GetBenefitAndQiDisorder(byte neiliElementType, int charId, CombatSkillItem skillConfig)
	{
		if (neiliElementType == 5)
		{
			return (benefit: 100, qiDisorder: 0);
		}
		if (FiveElementEquals(charId, skillConfig, FiveElementsType.Producing[neiliElementType]))
		{
			return (benefit: 200, qiDisorder: (short)(skillConfig.Grade * -125));
		}
		if (FiveElementEquals(charId, skillConfig, FiveElementsType.Countering[neiliElementType]))
		{
			return (benefit: 50, qiDisorder: (short)(skillConfig.Grade * 125));
		}
		return (benefit: 100, qiDisorder: 0);
	}

	public CombatSkill CreateCombatSkill(int charId, short skillTemplateId, ushort readingState = 0)
	{
		CombatSkill combatSkill = new CombatSkill(charId, skillTemplateId, readingState);
		AddElement_CombatSkills(combatSkill.GetId(), combatSkill);
		return combatSkill;
	}

	public void RegisterCombatSkills(int charId, List<CombatSkill> combatSkills)
	{
		int i = 0;
		for (int count = combatSkills.Count; i < count; i++)
		{
			CombatSkill combatSkill = combatSkills[i];
			combatSkill.OfflineSetCharId(charId);
			AddElement_CombatSkills(combatSkill.GetId(), combatSkill);
		}
	}

	public void RemoveCombatSkill(int charId, short skillTemplateId)
	{
		RemoveElement_CombatSkills(new CombatSkillKey(charId, skillTemplateId));
	}

	public void RemoveAllCombatSkills(int charId)
	{
		Dictionary<short, CombatSkill> charCombatSkills = GetCharCombatSkills(charId);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list.AddRange(charCombatSkills.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			RemoveElement_CombatSkills(new CombatSkillKey(charId, list[i]));
		}
		_combatSkills.RemoveCharStub(charId);
		ObjectPool<List<short>>.Instance.Return(list);
	}

	public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		Dictionary<short, CombatSkill> charCombatSkills = GetCharCombatSkills(taiwuCharId);
		crossArchiveGameData.CombatSkills = new List<CombatSkill>();
		crossArchiveGameData.CombatSkills.AddRange(charCombatSkills.Values);
	}

	public void UnpackCrossArchiveGameData_CombatSkills(DataContext context, CrossArchiveGameData crossArchiveGameData, bool overwriteEquipments)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		Dictionary<short, CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(taiwuCharId);
		List<short> learnedCombatSkills = taiwu.GetLearnedCombatSkills();
		List<CombatSkill> combatSkills = crossArchiveGameData.CombatSkills;
		List<CombatSkill> list = null;
		foreach (CombatSkill item in combatSkills)
		{
			short skillTemplateId = item.GetId().SkillTemplateId;
			if (charCombatSkills.TryGetValue(skillTemplateId, out var value))
			{
				ushort readingState = (ushort)(value.GetReadingState() | item.GetReadingState());
				short obtainedNeili = Math.Max(value.GetObtainedNeili(), item.GetObtainedNeili());
				bool revoked = value.GetRevoked();
				bool revoked2 = item.GetRevoked();
				value.SetReadingState(readingState, context);
				value.SetObtainedNeili(obtainedNeili, context);
				value.SetRevoked(revoked && revoked2, context);
				ushort activationState = value.GetActivationState();
				ushort activationState2 = item.GetActivationState();
				if (activationState != 0 && activationState2 != 0)
				{
					if (list == null)
					{
						list = new List<CombatSkill>();
					}
					list.Add(item);
				}
				else if (activationState2 != 0)
				{
					value.SetActivationState(activationState2, context);
					value.SetBreakoutStepsCount(item.GetBreakoutStepsCount(), context);
					value.SetForcedBreakoutStepsCount(item.GetForcedBreakoutStepsCount(), context);
				}
				if (overwriteEquipments)
				{
					value.SetInnerRatio(item.GetInnerRatio(), context);
				}
			}
			else
			{
				item.OfflineSetCharId(taiwuCharId);
				learnedCombatSkills.Add(skillTemplateId);
				AddElement_CombatSkills(item.GetId(), item);
			}
		}
		taiwu.SetLearnedCombatSkills(learnedCombatSkills, context);
		crossArchiveGameData.CombatSkills = list;
	}

	[DomainMethod]
	public CombatSkillDisplayData GetCombatSkillDisplayDataOnce(int charId, short skillTemplateId)
	{
		GameData.Domains.Character.Character element = null;
		if (charId >= 0 && !DomainManager.Character.TryGetElement_Objects(charId, out element))
		{
			charId = -1;
		}
		if (charId < 0)
		{
			charId = DomainManager.Taiwu.GetTaiwuCharId();
			element = DomainManager.Taiwu.GetTaiwu();
		}
		return CalcCombatSkillDisplayData(skillTemplateId, charId, element);
	}

	[DomainMethod]
	public List<CombatSkillDisplayData> GetCombatSkillDisplayData(int charId, List<short> skillTemplateIdList)
	{
		return CalcCombatSkillDisplayDataList(charId, skillTemplateIdList);
	}

	[DomainMethod]
	public List<CombatSkillDisplayData> GetCharacterEquipCombatSkillDisplayData(int charId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return null;
		}
		return CalcCombatSkillDisplayDataList(charId, element.GetCombatSkillEquipment());
	}

	[DomainMethod]
	public List<CombatSkillEffectDescriptionDisplayData> GetEffectDescriptionData(int charId, List<short> skillIds)
	{
		List<CombatSkillEffectDescriptionDisplayData> list = new List<CombatSkillEffectDescriptionDisplayData>();
		foreach (short skillId in skillIds)
		{
			CombatSkillKey combatSkillKey = new CombatSkillKey(charId, skillId);
			if (TryGetElement_CombatSkills(combatSkillKey, out var element))
			{
				list.Add(GetEffectDisplayData(element));
				continue;
			}
			list.Add(CombatSkillEffectDescriptionDisplayData.Invalid);
			PredefinedLog.Show(13, $"GetEffectDescriptionData no exist key {combatSkillKey}");
		}
		return list;
	}

	[DomainMethod]
	public sbyte GetCombatSkillBreakStepCount(int charId, short skillTemplateId)
	{
		return DomainManager.Character.GetElement_Objects(charId).GetSkillBreakoutAvailableStepsCount(skillTemplateId);
	}

	[DomainMethod]
	public int GetCombatSkillBreakoutStepsMaxPower(int charId, short skillTemplateId)
	{
		GameData.Domains.Character.Character element;
		return DomainManager.Character.TryGetElement_Objects(charId, out element) ? element.GetSkillBreakoutStepsMaxPower(skillTemplateId) : 0;
	}

	private CombatSkillDisplayData CalcCombatSkillDisplayData(short skillTemplateId, int charId, GameData.Domains.Character.Character character)
	{
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		if (character == null)
		{
			PredefinedLog.Show(12, $"CalcCombatSkillDisplayData {skillTemplateId} {charId}");
			return null;
		}
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
		CombatCharacter element = null;
		bool flag = DomainManager.Combat.IsInCombat() && DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out element);
		CombatSkillKey key = new CombatSkillKey(charId, skillTemplateId);
		bool flag2 = _combatSkills.ContainsKey(key);
		CombatSkill combatSkill = (flag2 ? _combatSkills[key] : new CombatSkill(charId, skillTemplateId, 0));
		bool flag3 = charId == DomainManager.Taiwu.GetTaiwuCharId();
		CombatSkillDisplayData combatSkillDisplayData = new CombatSkillDisplayData();
		combatSkillDisplayData.CharId = charId;
		combatSkillDisplayData.TemplateId = skillTemplateId;
		combatSkillDisplayData.ReadingState = combatSkill.GetReadingState();
		combatSkillDisplayData.ActivationState = combatSkill.GetActivationState();
		combatSkillDisplayData.CanAffect = character.GetCombatSkillCanAffect(skillTemplateId);
		combatSkillDisplayData.Conflicting = DomainManager.Extra.GetConflictCombatSkill(skillTemplateId) != null;
		combatSkillDisplayData.GridCount = character.GetCombatSkillGridCost(skillTemplateId);
		combatSkillDisplayData.Power = (short)(flag2 ? combatSkill.GetPower() : 100);
		combatSkillDisplayData.MaxPower = (flag2 ? combatSkill.GetMaxPower() : GlobalConfig.Instance.CombatSkillMaxBasePower);
		combatSkillDisplayData.RequirementsPower = (short)(flag2 ? combatSkill.GetRequirementsPower() : (-1));
		combatSkillDisplayData.Requirements = combatSkill.GetRequirementsAndActualValues(character, flag2);
		combatSkillDisplayData.BreakAddProperty = combatSkill.GetBreakAddPropertyList(CValuePercent.op_Implicit((int)combatSkillDisplayData.Power));
		combatSkillDisplayData.NeiliAllocationAddProperty = combatSkill.GetNeiliAllocationPropertyList(combatSkillDisplayData.Power);
		combatSkillDisplayData.BreakPlateIndex = (sbyte)(flag3 ? DomainManager.Extra.GetCombatSkillCurrBreakPlateIndex(skillTemplateId) : 0);
		combatSkillDisplayData.EffectType = (sbyte)(flag2 ? combatSkill.GetDirection() : (-1));
		combatSkillDisplayData.Mastered = DomainManager.Extra.GetCharacterMasteredCombatSkills(charId).Items?.Contains(skillTemplateId) ?? false;
		combatSkillDisplayData.Revoked = combatSkill.GetRevoked();
		combatSkillDisplayData.JumpThreshold = (short)(flag3 ? DomainManager.Extra.GetJumpThreshold(skillTemplateId) : (-1));
		combatSkillDisplayData.BaseInnerRatio = (flag2 ? combatSkill.GetBaseInnerRatio() : combatSkillItem.BaseInnerRatio);
		combatSkillDisplayData.InnerRatioChangeRange = (flag2 ? combatSkill.GetInnerRatioChangeRange() : combatSkillItem.InnerRatioChangeRange);
		combatSkillDisplayData.CurrInnerRatio = (flag2 ? combatSkill.GetCurrInnerRatio() : combatSkill.GetInnerRatio());
		combatSkillDisplayData.ExpectInnerRatio = combatSkill.GetInnerRatio();
		combatSkillDisplayData.NewUnderstandingNeedExp = (flag2 ? GetNewUnderstandingNeedExp(charId, skillTemplateId) : (-1));
		combatSkillDisplayData.BreakSuccess = GetBreakSuccess(charId, skillTemplateId);
		combatSkillDisplayData.EffectDescription = (flag2 ? GetEffectDisplayData(combatSkill) : CombatSkillEffectDescriptionDisplayData.Invalid);
		combatSkillDisplayData.DamageStepBonus = (flag2 ? combatSkill.CalcStepBonusDisplayData() : default(CombatSkillDamageStepBonusDisplayData));
		if (flag2)
		{
			IEnumerable<SkillBreakPlateBonus> breakBonuses = combatSkill.GetBreakBonuses();
			combatSkillDisplayData.BreakBonusGrades = new List<sbyte>();
			foreach (SkillBreakPlateBonus item in breakBonuses)
			{
				combatSkillDisplayData.BreakBonusGrades.Add(item.Grade);
			}
		}
		if (!flag2)
		{
			for (int i = 0; i < combatSkillDisplayData.Requirements.Count; i++)
			{
				(int, int, int) value = combatSkillDisplayData.Requirements[i];
				value.Item3 = -1;
				combatSkillDisplayData.Requirements[i] = value;
			}
		}
		switch (combatSkillItem.EquipType)
		{
		case 0:
			CalcNeigongSkillDisplayData(combatSkillDisplayData, flag2, combatSkill, combatSkillItem);
			break;
		case 1:
			CalcAttackSkillDisplayData(skillTemplateId, charId, combatSkillDisplayData, flag2, combatSkillItem, combatSkill);
			break;
		case 2:
			CalcAgileSkillDisplayData(combatSkillDisplayData, combatSkill, flag2, combatSkillItem);
			break;
		case 3:
			CalcDefenseSkillDisplayData(combatSkillDisplayData, flag2, combatSkill, combatSkillItem);
			break;
		}
		if (GameData.Domains.Character.CombatSkillHelper.IsProactiveSkill(combatSkillItem.EquipType))
		{
			CalcProactiveSkillDisplayData(combatSkillDisplayData, combatSkill, flag2, combatSkillItem, flag, element);
		}
		if (flag)
		{
			CalcInCombatSkillDisplayData(combatSkillDisplayData);
		}
		CalcSkillDisplayDataFiveElement(combatSkillDisplayData, charId, character);
		return combatSkillDisplayData;
	}

	private List<CombatSkillDisplayData> CalcCombatSkillDisplayDataList(int charId, IEnumerable<short> skillTemplateIdList)
	{
		List<CombatSkillDisplayData> list = new List<CombatSkillDisplayData>();
		if (skillTemplateIdList == null)
		{
			return list;
		}
		GameData.Domains.Character.Character element = null;
		if (charId >= 0 && !DomainManager.Character.TryGetElement_Objects(charId, out element))
		{
			charId = -1;
		}
		if (charId < 0)
		{
			charId = DomainManager.Taiwu.GetTaiwuCharId();
			element = DomainManager.Taiwu.GetTaiwu();
		}
		foreach (short skillTemplateId in skillTemplateIdList)
		{
			if (skillTemplateId >= 0)
			{
				list.Add(CalcCombatSkillDisplayData(skillTemplateId, charId, element));
			}
		}
		return list;
	}

	private static void CalcNeigongSkillDisplayData(CombatSkillDisplayData data, bool skillExist, CombatSkill skill, CombatSkillItem configData, bool preview = false)
	{
		if (skillExist)
		{
			data.MaxObtainableNeili = skill.GetTotalObtainableNeili();
			data.ObtainedNeili = skill.GetObtainedNeili();
			data.SpecificGrids = skill.GetSpecificGridCount(preview);
			data.GenericGrid = skill.GetGenericGridCount(preview);
		}
		else
		{
			data.MaxObtainableNeili = configData.TotalObtainableNeili;
			data.ObtainedNeili = 0;
			data.SpecificGrids = configData.SpecificGrids;
			data.GenericGrid = configData.GenericGrid;
		}
	}

	private unsafe void CalcAttackSkillDisplayData(short skillTemplateId, int charId, CombatSkillDisplayData data, bool skillExist, CombatSkillItem configData, CombatSkill skill)
	{
		if (skillExist)
		{
			data.AddAttackDistanceForward = GetCombatSkillAddAttackDistance(charId, skillTemplateId, forward: true);
			data.AddAttackDistanceBackward = GetCombatSkillAddAttackDistance(charId, skillTemplateId, forward: false);
			data.Poisons = skill.GetPoisons();
			HitOrAvoidInts hitValue = skill.GetHitValue();
			OuterAndInnerInts penetrations = skill.GetPenetrations();
			TaiwuCombatSkill value;
			sbyte fullPowerCastTimes = (sbyte)((charId == DomainManager.Taiwu.GetTaiwuCharId() && DomainManager.Taiwu.TryGetElement_CombatSkills(skillTemplateId, out value)) ? value.FullPowerCastTimes : 0);
			data.HitValueStrength = hitValue.Items[0];
			data.HitValueTechnique = hitValue.Items[1];
			data.HitValueSpeed = hitValue.Items[2];
			data.HitValueMind = hitValue.Items[3];
			data.PenetrateValueInner = penetrations.Inner;
			data.PenetrateValueOuter = penetrations.Outer;
			data.HitDistribution = skill.GetHitDistribution();
			data.BodyPartWeights = new List<int>(skill.GetBodyPartWeights());
			data.FullPowerCastTimes = fullPowerCastTimes;
			return;
		}
		data.AddAttackDistanceForward = configData.DistanceAdditionWhenCast;
		data.AddAttackDistanceBackward = configData.DistanceAdditionWhenCast;
		data.Poisons = configData.Poisons;
		sbyte[] perHitDamageRateDistribution = configData.PerHitDamageRateDistribution;
		int totalHit = configData.TotalHit;
		data.HitValueStrength = (data.HitValueTechnique = (data.HitValueSpeed = (data.HitValueMind = 0)));
		if (CombatSkillTemplateHelper.IsMindHitSkill(skillTemplateId))
		{
			data.HitValueMind = totalHit;
		}
		else
		{
			data.HitValueStrength = totalHit * perHitDamageRateDistribution[0] / 100;
			data.HitValueStrength = totalHit * perHitDamageRateDistribution[1] / 100;
			data.HitValueStrength = totalHit * perHitDamageRateDistribution[2] / 100;
		}
		short penetrate = configData.Penetrate;
		data.PenetrateValueInner = penetrate * data.CurrInnerRatio / 100;
		data.PenetrateValueOuter = penetrate - data.PenetrateValueInner;
		ref int items = ref data.HitDistribution.Items[0];
		items = perHitDamageRateDistribution[0];
		data.HitDistribution.Items[1] = perHitDamageRateDistribution[1];
		data.HitDistribution.Items[2] = perHitDamageRateDistribution[2];
		data.HitDistribution.Items[3] = perHitDamageRateDistribution[3];
		data.BodyPartWeights = new List<int>(((IEnumerable<sbyte>)configData.InjuryPartAtkRateDistribution).Select((Func<sbyte, int>)((sbyte x) => x)));
		data.FullPowerCastTimes = 0;
	}

	private unsafe void CalcAgileSkillDisplayData(CombatSkillDisplayData data, CombatSkill skill, bool skillExist, CombatSkillItem configData)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		data.JumpSpeed = CalcJumpSpeed(data.CharId, data.TemplateId);
		data.AddMoveSpeed = CalcCastAddMoveSpeed(skill, CValuePercent.op_Implicit((int)data.Power));
		data.AddPercentMoveSpeed = CalcCastAddPercentMoveSpeed(skill, CValuePercent.op_Implicit((int)data.Power));
		if (skillExist)
		{
			HitOrAvoidInts hitOrAvoidInts = CalcAddHitValueOnCast(skill, CValuePercent.op_Implicit((int)data.Power));
			data.AddHitStrength = hitOrAvoidInts.Items[0];
			data.AddHitTechnique = hitOrAvoidInts.Items[1];
			data.AddHitSpeed = hitOrAvoidInts.Items[2];
			data.AddHitMind = hitOrAvoidInts.Items[3];
		}
		else
		{
			data.AddAvoidStrength = configData.AddHitOnCast[0];
			data.AddAvoidTechnique = configData.AddHitOnCast[1];
			data.AddAvoidSpeed = configData.AddHitOnCast[2];
			data.AddAvoidMind = configData.AddHitOnCast[3];
		}
	}

	private unsafe static void CalcDefenseSkillDisplayData(CombatSkillDisplayData data, bool skillExist, CombatSkill skill, CombatSkillItem configData)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		if (skillExist)
		{
			HitOrAvoidInts hitOrAvoidInts = CalcAddAvoidValueOnCast(skill, CValuePercent.op_Implicit((int)data.Power));
			OuterAndInnerInts outerAndInnerInts = CalcAddPenetrateResist(skill, CValuePercent.op_Implicit((int)data.Power));
			OuterAndInnerInts outerAndInnerInts2 = CalcBouncePower(skill, CValuePercent.op_Implicit((int)data.Power));
			data.EffectDuration = CalcContinuousFrames(skill);
			data.AddOuterDef = outerAndInnerInts.Outer;
			data.AddInnerDef = outerAndInnerInts.Inner;
			data.AddAvoidStrength = hitOrAvoidInts.Items[0];
			data.AddAvoidTechnique = hitOrAvoidInts.Items[1];
			data.AddAvoidSpeed = hitOrAvoidInts.Items[2];
			data.AddAvoidMind = hitOrAvoidInts.Items[3];
			data.FightbackPower = CalcFightBackPower(skill, CValuePercent.op_Implicit((int)data.Power));
			data.BouncePowerOuter = outerAndInnerInts2.Outer;
			data.BouncePowerInner = outerAndInnerInts2.Inner;
			data.BounceDistance = skill.GetBounceDistance();
		}
		else
		{
			data.EffectDuration = configData.ContinuousFrames;
			data.AddOuterDef = configData.AddOuterPenetrateResistOnCast;
			data.AddInnerDef = configData.AddInnerPenetrateResistOnCast;
			data.AddAvoidStrength = configData.AddAvoidOnCast[0];
			data.AddAvoidTechnique = configData.AddAvoidOnCast[1];
			data.AddAvoidSpeed = configData.AddAvoidOnCast[2];
			data.AddAvoidMind = configData.AddAvoidOnCast[3];
			data.FightbackPower = configData.FightBackDamage;
			data.BouncePowerOuter = configData.BounceRateOfOuterInjury;
			data.BouncePowerInner = configData.BounceRateOfInnerInjury;
			data.BounceDistance = configData.BounceDistance;
		}
	}

	private unsafe void CalcProactiveSkillDisplayData(CombatSkillDisplayData data, CombatSkill skill, bool skillExist, CombatSkillItem configData, bool inCombat, CombatCharacter combatChar)
	{
		data.CostMobility = (skillExist ? skill.GetCostMobilityPercent() : configData.MobilityCost);
		data.CostNeiliAllocation = ((sbyte, sbyte))(skillExist ? (((int, int))skill.GetCostNeiliAllocation()) : (-1, 0));
		data.CostTricks = new List<NeedTrick>();
		GetCombatSkillCostTrick(skill, data.CostTricks);
		if (inCombat)
		{
			int num = MoveSpecialConstants.MaxMobility * data.CostMobility / 100;
			NeiliAllocation neiliAllocation = combatChar.GetNeiliAllocation();
			TrickCollection tricks = combatChar.GetTricks();
			if (skillExist)
			{
				DomainManager.Combat.GetSkillCostBreathStance(combatChar.GetId(), skill).Deconstruct(out var outer, out var inner);
				int num2 = outer;
				int num3 = inner;
				sbyte costStance = (sbyte)num2;
				sbyte costBreath = (sbyte)num3;
				data.CostStance = costStance;
				data.CostBreath = costBreath;
				data.CostBreathFontType = Convert(combatChar.GetBreathValue() >= 30000 * data.CostBreath / 100);
				data.CostStanceFontType = Convert(combatChar.GetStanceValue() >= 4000 * data.CostStance / 100);
			}
			else
			{
				PredefinedLog.Show(13, $"Skill not exist in combat {combatChar} {configData.Name}");
				data.CostBreath = (data.CostStance = 50);
				data.CostBreathFontType = (data.CostStanceFontType = Convert(enough: false));
			}
			data.CostMobilityFontType = Convert(combatChar.GetMobilityValue() >= num);
			data.CostNeiliAllocationFontType = Convert(neiliAllocation.Items[data.CostNeiliAllocation.Item1] >= data.CostNeiliAllocation.Item2);
			data.CostWeaponDurabilityFontType = Convert(combatChar.GetUsingWeaponIndex() >= 3 || DomainManager.Item.GetElement_Weapons(DomainManager.Combat.GetUsingWeaponKey(combatChar).Id).GetCurrDurability() >= configData.WeaponDurableCost);
			data.CostWugFontType = Convert(combatChar.GetWugCount() >= configData.WugCost);
			data.CostTricksFontType = new List<sbyte>();
			for (int i = 0; i < data.CostTricks.Count; i++)
			{
				NeedTrick needTrick = data.CostTricks[i];
				int num4 = 0;
				foreach (sbyte value in tricks.Tricks.Values)
				{
					if (combatChar.TrickEquals(value, needTrick.TrickType))
					{
						num4++;
					}
				}
				data.CostTricksFontType.Add(Convert(num4 >= needTrick.NeedCount));
			}
		}
		else
		{
			data.CostBreath = (skillExist ? skill.GetCostBreathPercent() : ((sbyte)(configData.BreathStanceTotalCost * data.CurrInnerRatio / 100)));
			data.CostStance = (skillExist ? skill.GetCostStancePercent() : ((sbyte)(configData.BreathStanceTotalCost - data.CostBreath)));
			data.CostBreathFontType = (data.CostStanceFontType = (data.CostMobilityFontType = (data.CostNeiliAllocationFontType = (data.CostWeaponDurabilityFontType = (data.CostWugFontType = 0)))));
			data.CostTricksFontType = new List<sbyte>();
			for (int j = 0; j < data.CostTricks.Count; j++)
			{
				data.CostTricksFontType.Add(0);
			}
		}
		static sbyte Convert(bool enough)
		{
			return (sbyte)(enough ? 1 : 2);
		}
	}

	private void CalcInCombatSkillDisplayData(CombatSkillDisplayData data)
	{
		if (DomainManager.Combat.TryGetCombatSkillData(data.CharId, data.TemplateId, out var combatSkillData))
		{
			data.EffectData = combatSkillData.GetEffectData();
		}
	}

	private void CalcSkillDisplayDataFiveElement(CombatSkillDisplayData data, int charId, GameData.Domains.Character.Character character)
	{
		bool flag = charId == DomainManager.Taiwu.GetTaiwuCharId();
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[data.TemplateId];
		data.FiveElementDestTypeWhileLooping = combatSkillItem.DestTypeWhileLooping;
		data.FiveElementTransferTypeWhileLooping = combatSkillItem.TransferTypeWhileLooping;
		if (!flag)
		{
			return;
		}
		short loopingNeigong = character.GetLoopingNeigong();
		if (loopingNeigong < 0 || !DomainManager.Extra.TryGetElement_QiArtStrategyMap(loopingNeigong, out var value) || value.Items == null || value.Items.Count == 0)
		{
			return;
		}
		sbyte neiliType = character.GetNeiliType();
		foreach (sbyte item in value.Items)
		{
			if (item != -1)
			{
				QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[item];
				if (qiArtStrategyItem.TransferToFiveElements > -1 && qiArtStrategyItem.FiveElementsTransferType > -1)
				{
					data.FiveElementDestTypeWhileLooping = qiArtStrategyItem.TransferToFiveElements;
					data.FiveElementTransferTypeWhileLooping = qiArtStrategyItem.FiveElementsTransferType;
				}
				short anchorFiveElements = qiArtStrategyItem.AnchorFiveElements;
				NeiliTypeItem neiliTypeItem = NeiliType.Instance[neiliType];
				bool flag2 = neiliTypeItem.ColorType == 2;
				if (anchorFiveElements != -1 && !flag2 && neiliTypeItem.FiveElements == anchorFiveElements)
				{
					data.FiveElementDestTypeWhileLooping = qiArtStrategyItem.TransferToFiveElements;
					data.FiveElementTransferTypeWhileLooping = qiArtStrategyItem.FiveElementsTransferType;
					break;
				}
			}
		}
	}

	public CombatSkillEffectDescriptionDisplayData GetEffectDisplayData(CombatSkillKey skillKey)
	{
		CombatSkill element;
		return TryGetElement_CombatSkills(skillKey, out element) ? GetEffectDisplayData(element) : CombatSkillEffectDescriptionDisplayData.Invalid;
	}

	public CombatSkillEffectDescriptionDisplayData GetEffectDisplayData(int charId, short skillTemplateId)
	{
		return GetEffectDisplayData(new CombatSkillKey(charId, skillTemplateId));
	}

	public CombatSkillEffectDescriptionDisplayData GetEffectDisplayData(CombatSkill skill)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skill.GetId().SkillTemplateId];
		sbyte direction = skill.GetDirection();
		CombatSkillEffectDescriptionDisplayData result = default(CombatSkillEffectDescriptionDisplayData);
		bool flag = ((direction < 0 || direction >= 2) ? true : false);
		result.EffectId = (flag ? (-1) : ((direction == 0) ? combatSkillItem.DirectEffectID : combatSkillItem.ReverseEffectID));
		result.AffectRequirePower = (skill.AnyAffectRequirePower() ? new List<int>(skill.GetAffectRequirePower()) : null);
		return result;
	}

	public short GetCombatSkillAddAttackDistance(int charId, short skillId, bool forward)
	{
		CombatSkillKey key = new CombatSkillKey(charId, skillId);
		ushort fieldId = (ushort)(forward ? 145 : 146);
		int modifyValue = DomainManager.SpecialEffect.GetModifyValue(charId, skillId, fieldId, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		modifyValue -= DomainManager.SpecialEffect.GetModifyValue(charId, -1, fieldId, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		if (skillId >= 0)
		{
			modifyValue += (_combatSkills.TryGetValue(key, out var value) ? value.GetDistanceAdditionWhenCast(forward) : Config.CombatSkill.Instance[skillId].DistanceAdditionWhenCast);
		}
		return (short)modifyValue;
	}

	public void GetCombatSkillCostTrick(CombatSkill skill, List<NeedTrick> costTricks, bool applySpecialEffect = true)
	{
		CombatSkillKey id = skill.GetId();
		costTricks.Clear();
		costTricks.AddRange(Config.CombatSkill.Instance[id.SkillTemplateId].TrickCost);
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < costTricks.Count; i++)
		{
			bool flag = list.Count == costTricks.Count - 1;
			short propertyId = (short)(53 + costTricks[i].TrickType);
			int index = i;
			NeedTrick value = costTricks[i];
			value.NeedCount = (byte)Math.Max(flag ? 1 : 0, costTricks[i].NeedCount + skill.GetBreakoutGridCombatSkillPropertyBonus(propertyId));
			costTricks[index] = value;
			if (costTricks[i].NeedCount == 0)
			{
				list.Add(i);
			}
		}
		for (int num = list.Count - 1; num >= 0; num--)
		{
			costTricks.RemoveAt(list[num]);
		}
		ObjectPool<List<int>>.Instance.Return(list);
		if (applySpecialEffect)
		{
			DomainManager.SpecialEffect.ModifyData(id.CharId, id.SkillTemplateId, 208, costTricks);
		}
	}

	public sbyte GetCombatSkillGridCost(int charId, short skillTemplateId)
	{
		if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return element.GetCombatSkillGridCost(skillTemplateId);
		}
		return Config.CombatSkill.Instance[skillTemplateId].GridCost;
	}

	public static int CalcJumpSpeed(int charId, short skillTemplateId)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		GameData.Domains.Character.Character element;
		int moveSpeed = (DomainManager.Character.TryGetElement_Objects(charId, out element) ? element.GetMoveSpeed() : 100);
		int num = CFormula.CalcJumpSpeed(moveSpeed);
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(charId, skillTemplateId, 152, (EDataModifyType)1, -1, -1, -1, (EDataSumType)0));
		CombatSkillKey objectId = new CombatSkillKey(charId, skillTemplateId);
		if (DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out var element2))
		{
			val += CValuePercentBonus.op_Implicit(element2.GetBreakoutGridCombatSkillPropertyBonus(69));
		}
		return num * val;
	}

	public static short CalcCastAddMoveSpeed(CombatSkill skill, CValuePercent power)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skill.GetId().SkillTemplateId];
		CValuePercent val = CValuePercent.op_Implicit((int)GlobalConfig.Instance.AgileSkillBaseAddSpeed);
		CValuePercentBonus val2 = CValuePercentBonus.op_Implicit(skill.GetBreakoutGridCombatSkillPropertyBonus(11));
		return (short)((int)combatSkillItem.AddMoveSpeedOnCast * val * val2 * power);
	}

	public static short CalcCastAddPercentMoveSpeed(CombatSkill skill, CValuePercent power)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skill.GetId().SkillTemplateId];
		return (short)((int)combatSkillItem.AddPercentMoveSpeedOnCast * power);
	}

	public static HitOrAvoidInts CalcAddHitValueOnCast(CombatSkill skill, CValuePercent power)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skill.GetId().SkillTemplateId];
		CValuePercent val = CValuePercent.op_Implicit((int)GlobalConfig.Instance.AgileSkillBaseAddHit);
		HitOrAvoidInts result = default(HitOrAvoidInts);
		int num = 0;
		foreach (SkillBreakPlateBonus breakBonuse in skill.GetBreakBonuses())
		{
			num += breakBonuse.CalcAddHitOnCast();
		}
		for (int i = 0; i < 4; i++)
		{
			CValuePercentBonus val2 = CValuePercentBonus.op_Implicit(skill.GetBreakoutGridCombatSkillPropertyBonus((short)(12 + i)));
			val2 += CValuePercentBonus.op_Implicit(num);
			result[i] = (int)combatSkillItem.AddHitOnCast[i] * val * val2 * power;
		}
		return result;
	}

	public static OuterAndInnerInts CalcAddPenetrateResist(CombatSkill skill, CValuePercent power)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		OuterAndInnerInts result = default(OuterAndInnerInts);
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skill.GetId().SkillTemplateId];
		CValuePercent val = CValuePercent.op_Implicit((int)GlobalConfig.Instance.DefendSkillBaseAddPenetrateResist);
		CValuePercentBonus val2 = CValuePercentBonus.op_Implicit(skill.GetBreakoutGridCombatSkillPropertyBonus(18));
		CValuePercentBonus val3 = CValuePercentBonus.op_Implicit(skill.GetBreakoutGridCombatSkillPropertyBonus(19));
		foreach (SkillBreakPlateBonus breakBonuse in skill.GetBreakBonuses())
		{
			val2 += CValuePercentBonus.op_Implicit(breakBonuse.CalcAddPenetrateResist());
			val3 += CValuePercentBonus.op_Implicit(breakBonuse.CalcAddPenetrateResist());
		}
		result.Outer = (int)combatSkillItem.AddOuterPenetrateResistOnCast * val * val2 * power;
		result.Inner = (int)combatSkillItem.AddInnerPenetrateResistOnCast * val * val3 * power;
		return result;
	}

	public static short CalcContinuousFrames(CombatSkill skill)
	{
		return (short)DomainManager.SpecialEffect.ModifyValue(skill.GetId().CharId, skill.GetId().SkillTemplateId, 176, skill.GetContinuousFrames());
	}

	public static HitOrAvoidInts CalcAddAvoidValueOnCast(CombatSkill skill, CValuePercent power)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skill.GetId().SkillTemplateId];
		CValuePercent val = CValuePercent.op_Implicit((int)GlobalConfig.Instance.DefendSkillBaseAddAvoid);
		HitOrAvoidInts result = default(HitOrAvoidInts);
		int num = 0;
		foreach (SkillBreakPlateBonus breakBonuse in skill.GetBreakBonuses())
		{
			num += breakBonuse.CalcAddAvoidValueOnCast();
		}
		for (int i = 0; i < 4; i++)
		{
			CValuePercentBonus val2 = CValuePercentBonus.op_Implicit(skill.GetBreakoutGridCombatSkillPropertyBonus((short)(20 + i)));
			val2 += CValuePercentBonus.op_Implicit(num);
			result[i] = (int)combatSkillItem.AddAvoidOnCast[i] * val * val2 * power;
		}
		return result;
	}

	public static int CalcFightBackPower(CombatSkill skill, CValuePercent power)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skill.GetId().SkillTemplateId];
		CValuePercent val = CValuePercent.op_Implicit((int)GlobalConfig.Instance.DefendSkillBaseFightBackPower);
		CValuePercentBonus val2 = CValuePercentBonus.op_Implicit(skill.GetBreakoutGridCombatSkillPropertyBonus(24));
		foreach (SkillBreakPlateBonus breakBonuse in skill.GetBreakBonuses())
		{
			val2 += CValuePercentBonus.op_Implicit(breakBonuse.CalcFightBackPower());
		}
		return (int)combatSkillItem.FightBackDamage * val2 * val * power;
	}

	public static OuterAndInnerInts CalcBouncePower(CombatSkill skill, CValuePercent power)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		OuterAndInnerInts result = default(OuterAndInnerInts);
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skill.GetId().SkillTemplateId];
		CValuePercent val = CValuePercent.op_Implicit((int)GlobalConfig.Instance.DefendSkillBaseBouncePower);
		CValuePercentBonus val2 = CValuePercentBonus.op_Implicit(skill.GetBreakoutGridCombatSkillPropertyBonus(25));
		CValuePercentBonus val3 = CValuePercentBonus.op_Implicit(skill.GetBreakoutGridCombatSkillPropertyBonus(26));
		foreach (SkillBreakPlateBonus breakBonuse in skill.GetBreakBonuses())
		{
			val2 += CValuePercentBonus.op_Implicit(breakBonuse.CalcBouncePower());
			val3 += CValuePercentBonus.op_Implicit(breakBonuse.CalcBouncePower());
		}
		result.Outer = (int)combatSkillItem.BounceRateOfOuterInjury * val2 * val * power;
		result.Inner = (int)combatSkillItem.BounceRateOfInnerInjury * val3 * val * power;
		return result;
	}

	public int GetNewUnderstandingNeedExp(int charId, short skillTemplateId)
	{
		if (charId != DomainManager.Taiwu.GetTaiwuCharId())
		{
			return 0;
		}
		sbyte combatSkillBreakStepCount = DomainManager.CombatSkill.GetCombatSkillBreakStepCount(charId, skillTemplateId);
		int num = Math.Max(50 - combatSkillBreakStepCount, 0);
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
		short costExp = Config.SkillBreakPlate.Instance[combatSkillItem.Grade].CostExp;
		return num * costExp;
	}

	public bool GetBreakSuccess(int charId, short skillTemplateId)
	{
		if (charId != DomainManager.Taiwu.GetTaiwuCharId())
		{
			return false;
		}
		if (!TryGetElement_CombatSkills((charId: charId, skillId: skillTemplateId), out var element))
		{
			return false;
		}
		return CombatSkillStateHelper.IsBrokenOut(element.GetActivationState());
	}

	[DomainMethod]
	public CombatSkillDisplayData GetCombatSkillPreviewDisplayDataOnce(int charId, short skillTemplateId)
	{
		GameData.Domains.Character.Character element = null;
		if (charId >= 0 && !DomainManager.Character.TryGetElement_Objects(charId, out element))
		{
			charId = -1;
		}
		if (charId < 0)
		{
			charId = DomainManager.Taiwu.GetTaiwuCharId();
			element = DomainManager.Taiwu.GetTaiwu();
		}
		CombatSkillDisplayData combatSkillDisplayData = CalcCombatSkillDisplayData(skillTemplateId, charId, element);
		bool flag = DomainManager.Extra.IsCombatSkillMasteredByCharacter(charId, skillTemplateId);
		combatSkillDisplayData.GridCount += (sbyte)(flag ? 1 : (-1));
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
		if (combatSkillItem.EquipType == 0)
		{
			CombatSkillKey key = new CombatSkillKey(charId, skillTemplateId);
			bool flag2 = _combatSkills.ContainsKey(key);
			CombatSkill skill = (flag2 ? _combatSkills[key] : new CombatSkill(charId, skillTemplateId, 0));
			CalcNeigongSkillDisplayData(combatSkillDisplayData, flag2, skill, combatSkillItem, preview: true);
		}
		combatSkillDisplayData.Mastered = !flag;
		combatSkillDisplayData.PreviewMastered = true;
		return combatSkillDisplayData;
	}

	[DomainMethod]
	public List<SkillBreakPlateBonus> GetCombatSkillBreakBonuses(int charId, short skillTemplateId)
	{
		List<SkillBreakPlateBonus> result = new List<SkillBreakPlateBonus>();
		if (!DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(charId, skillTemplateId), out var element))
		{
			return result;
		}
		return element.GetBreakBonuses().ToList();
	}

	public static List<BreakGrid> GetBonusBreakGrids(short skillTemplateId, sbyte behaviorType)
	{
		SkillBreakGridListItem skillBreakGridListItem = SkillBreakGridList.Instance[skillTemplateId];
		if (skillBreakGridListItem == null)
		{
			return null;
		}
		if (1 == 0)
		{
		}
		List<BreakGrid> result = behaviorType switch
		{
			0 => skillBreakGridListItem.BreakGridListJust, 
			1 => skillBreakGridListItem.BreakGridListKind, 
			2 => skillBreakGridListItem.BreakGridListEven, 
			3 => skillBreakGridListItem.BreakGridListRebel, 
			4 => skillBreakGridListItem.BreakGridListEgoistic, 
			_ => null, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static IEnumerable<int> FiveElementIndexes(int charId, CombatSkillItem config)
	{
		yield return config.FiveElements;
		BoolArray8 alsoAs = DomainManager.SpecialEffect.ModifyData(charId, config.TemplateId, 240, BoolArray8.op_Implicit((byte)0), config.FiveElements);
		for (int i = 0; i <= 5; i++)
		{
			if (i != config.FiveElements && ((BoolArray8)(ref alsoAs))[i])
			{
				yield return i;
			}
		}
	}

	public static bool FiveElementEquals(int charId, CombatSkillItem config, sbyte fiveElement)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if ((fiveElement < 0 || fiveElement > 5) ? true : false)
		{
			return false;
		}
		int result;
		if (config.FiveElements != fiveElement)
		{
			BoolArray8 val = DomainManager.SpecialEffect.ModifyData(charId, config.TemplateId, 240, BoolArray8.op_Implicit((byte)0), config.FiveElements);
			result = (((BoolArray8)(ref val))[(int)fiveElement] ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public static bool FiveElementEquals(int charId, short skillId, sbyte fiveElement)
	{
		return skillId >= 0 && FiveElementEquals(charId, Config.CombatSkill.Instance[skillId], fiveElement);
	}

	public static bool FiveElementEquals(int charId, CombatSkillItem config, IEnumerable<sbyte> fiveElements)
	{
		return fiveElements.Any((sbyte fiveElement) => FiveElementEquals(charId, config, fiveElement));
	}

	public static int FiveElementIndexesSum(int charId, CombatSkillItem config, sbyte[] properties)
	{
		int num = 0;
		foreach (int item in FiveElementIndexes(charId, config))
		{
			num += properties[item];
		}
		return num;
	}

	public static (int min, int max) FiveElementIndexesTotal(int charId, CombatSkillItem config, sbyte[] properties)
	{
		int num = 0;
		int num2 = 0;
		foreach (int item in FiveElementIndexes(charId, config))
		{
			int num3 = Math.Min(num, properties[item]);
			num2 = Math.Max(num2, properties[item]);
			num = num3;
		}
		return (min: num, max: num2);
	}

	public static bool FiveElementMatch(int charId, CombatSkillItem config, List<sbyte> fiveElementsLimit)
	{
		return fiveElementsLimit == null || fiveElementsLimit.Count == 0 || FiveElementEquals(charId, config, fiveElementsLimit);
	}

	public static bool FiveElementContains(int charId, CombatSkillItem config, List<byte> fiveElements)
	{
		return fiveElements != null && fiveElements.Count > 0 && FiveElementEquals(charId, config, fiveElements.Select((byte x) => (sbyte)x));
	}

	public CombatSkillDomain()
		: base(1)
	{
		_combatSkills = new CombatSkillCollection(8192);
		HelperDataCombatSkills = new ObjectCollectionHelperData(7, 0, CacheInfluencesCombatSkills, _dataStatesCombatSkills, isArchive: true);
		OnInitializedDomainData();
	}

	public CombatSkill GetElement_CombatSkills(CombatSkillKey objectId)
	{
		return _combatSkills[objectId];
	}

	public bool TryGetElement_CombatSkills(CombatSkillKey objectId, out CombatSkill element)
	{
		return _combatSkills.TryGetValue(objectId, out element);
	}

	private unsafe void AddElement_CombatSkills(CombatSkillKey objectId, CombatSkill instance)
	{
		instance.CollectionHelperData = HelperDataCombatSkills;
		instance.DataStatesOffset = _dataStatesCombatSkills.Create();
		_combatSkills.Add(objectId, instance);
		byte* pData = OperationAdder.FixedObjectCollection_Add(7, 0, objectId, 27);
		instance.Serialize(pData);
	}

	private void RemoveElement_CombatSkills(CombatSkillKey objectId)
	{
		if (_combatSkills.TryGetValue(objectId, out var value))
		{
			_dataStatesCombatSkills.Remove(value.DataStatesOffset);
			_combatSkills.Remove(objectId);
			OperationAdder.FixedObjectCollection_Remove(7, 0, objectId);
		}
	}

	private void ClearCombatSkills()
	{
		_dataStatesCombatSkills.Clear();
		_combatSkills.Clear();
		OperationAdder.FixedObjectCollection_Clear(7, 0);
	}

	public int GetElementField_CombatSkills(CombatSkillKey objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_combatSkills.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_CombatSkills", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesCombatSkills.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetPracticeLevel(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetReadingState(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetActivationState(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetForcedBreakoutStepsCount(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetBreakoutStepsCount(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetInnerRatio(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetObtainedNeili(), dataPool);
		case 8:
			return GameData.Serializer.Serializer.Serialize(value.GetRevoked(), dataPool);
		case 9:
			return GameData.Serializer.Serializer.Serialize(value.GetSpecialEffectId(), dataPool);
		case 10:
			return GameData.Serializer.Serializer.Serialize(value.GetPower(), dataPool);
		case 11:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxPower(), dataPool);
		case 12:
			return GameData.Serializer.Serializer.Serialize(value.GetRequirementPercent(), dataPool);
		case 13:
			return GameData.Serializer.Serializer.Serialize(value.GetDirection(), dataPool);
		case 14:
			return GameData.Serializer.Serializer.Serialize(value.GetBaseScore(), dataPool);
		case 15:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrInnerRatio(), dataPool);
		case 16:
			return GameData.Serializer.Serializer.Serialize(value.GetHitValue(), dataPool);
		case 17:
			return GameData.Serializer.Serializer.Serialize(value.GetPenetrations(), dataPool);
		case 18:
			return GameData.Serializer.Serializer.Serialize(value.GetCostBreathAndStancePercent(), dataPool);
		case 19:
			return GameData.Serializer.Serializer.Serialize(value.GetCostBreathPercent(), dataPool);
		case 20:
			return GameData.Serializer.Serializer.Serialize(value.GetCostStancePercent(), dataPool);
		case 21:
			return GameData.Serializer.Serializer.Serialize(value.GetCostMobilityPercent(), dataPool);
		case 22:
			return GameData.Serializer.Serializer.Serialize(value.GetAddHitValueOnCast(), dataPool);
		case 23:
			return GameData.Serializer.Serializer.Serialize(value.GetAddPenetrateResist(), dataPool);
		case 24:
			return GameData.Serializer.Serializer.Serialize(value.GetAddAvoidValueOnCast(), dataPool);
		case 25:
			return GameData.Serializer.Serializer.Serialize(value.GetFightBackPower(), dataPool);
		case 26:
			return GameData.Serializer.Serializer.Serialize(value.GetBouncePower(), dataPool);
		case 27:
			return GameData.Serializer.Serializer.Serialize(value.GetRequirementsPower(), dataPool);
		case 28:
			return GameData.Serializer.Serializer.Serialize(value.GetPlateAddMaxPower(), dataPool);
		default:
			if (fieldId >= 29)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_CombatSkills(CombatSkillKey objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_combatSkills.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
		{
			sbyte item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetPracticeLevel(item3, context);
			return;
		}
		case 2:
		{
			ushort item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetReadingState(item2, context);
			return;
		}
		case 3:
		{
			ushort item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetActivationState(item, context);
			return;
		}
		case 4:
		{
			sbyte item9 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item9);
			value.SetForcedBreakoutStepsCount(item9, context);
			return;
		}
		case 5:
		{
			sbyte item8 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item8);
			value.SetBreakoutStepsCount(item8, context);
			return;
		}
		case 6:
		{
			sbyte item7 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item7);
			value.SetInnerRatio(item7, context);
			return;
		}
		case 7:
		{
			short item6 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item6);
			value.SetObtainedNeili(item6, context);
			return;
		}
		case 8:
		{
			bool item5 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetRevoked(item5, context);
			return;
		}
		case 9:
		{
			long item4 = 0L;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetSpecialEffectId(item4, context);
			return;
		}
		}
		if (fieldId >= 29)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 29)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_CombatSkills(CombatSkillKey objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_combatSkills.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 29)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesCombatSkills.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesCombatSkills.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetPracticeLevel(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetReadingState(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetActivationState(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetForcedBreakoutStepsCount(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetBreakoutStepsCount(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetInnerRatio(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetObtainedNeili(), dataPool), 
			8 => GameData.Serializer.Serializer.Serialize(value.GetRevoked(), dataPool), 
			9 => GameData.Serializer.Serializer.Serialize(value.GetSpecialEffectId(), dataPool), 
			10 => GameData.Serializer.Serializer.Serialize(value.GetPower(), dataPool), 
			11 => GameData.Serializer.Serializer.Serialize(value.GetMaxPower(), dataPool), 
			12 => GameData.Serializer.Serializer.Serialize(value.GetRequirementPercent(), dataPool), 
			13 => GameData.Serializer.Serializer.Serialize(value.GetDirection(), dataPool), 
			14 => GameData.Serializer.Serializer.Serialize(value.GetBaseScore(), dataPool), 
			15 => GameData.Serializer.Serializer.Serialize(value.GetCurrInnerRatio(), dataPool), 
			16 => GameData.Serializer.Serializer.Serialize(value.GetHitValue(), dataPool), 
			17 => GameData.Serializer.Serializer.Serialize(value.GetPenetrations(), dataPool), 
			18 => GameData.Serializer.Serializer.Serialize(value.GetCostBreathAndStancePercent(), dataPool), 
			19 => GameData.Serializer.Serializer.Serialize(value.GetCostBreathPercent(), dataPool), 
			20 => GameData.Serializer.Serializer.Serialize(value.GetCostStancePercent(), dataPool), 
			21 => GameData.Serializer.Serializer.Serialize(value.GetCostMobilityPercent(), dataPool), 
			22 => GameData.Serializer.Serializer.Serialize(value.GetAddHitValueOnCast(), dataPool), 
			23 => GameData.Serializer.Serializer.Serialize(value.GetAddPenetrateResist(), dataPool), 
			24 => GameData.Serializer.Serializer.Serialize(value.GetAddAvoidValueOnCast(), dataPool), 
			25 => GameData.Serializer.Serializer.Serialize(value.GetFightBackPower(), dataPool), 
			26 => GameData.Serializer.Serializer.Serialize(value.GetBouncePower(), dataPool), 
			27 => GameData.Serializer.Serializer.Serialize(value.GetRequirementsPower(), dataPool), 
			28 => GameData.Serializer.Serializer.Serialize(value.GetPlateAddMaxPower(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_CombatSkills(CombatSkillKey objectId, ushort fieldId)
	{
		if (_combatSkills.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 29)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesCombatSkills.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesCombatSkills.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_CombatSkills(CombatSkillKey objectId, ushort fieldId)
	{
		if (!_combatSkills.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 29)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesCombatSkills.IsModified(value.DataStatesOffset, fieldId);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		foreach (KeyValuePair<CombatSkillKey, CombatSkill> combatSkill in _combatSkills)
		{
			CombatSkillKey key = combatSkill.Key;
			CombatSkill value = combatSkill.Value;
			byte* pData = OperationAdder.FixedObjectCollection_Add(7, 0, key, 27);
			value.Serialize(pData);
		}
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(7, 0));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		if (dataId == 0)
		{
			return GetElementField_CombatSkills((CombatSkillKey)subId0, (ushort)subId1, dataPool, resetModified);
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (dataId == 0)
		{
			SetElementField_CombatSkills((CombatSkillKey)subId0, (ushort)subId1, valueOffset, dataPool, context);
			return;
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
	{
		int argsOffset = operation.ArgsOffset;
		switch (operation.MethodId)
		{
		case 0:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 2)
			{
				int item20 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				List<short> item21 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item21);
				List<CombatSkillDisplayData> combatSkillDisplayData = GetCombatSkillDisplayData(item20, item21);
				return GameData.Serializer.Serializer.Serialize(combatSkillDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 2)
			{
				int item8 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				short item9 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				sbyte combatSkillBreakStepCount = GetCombatSkillBreakStepCount(item8, item9);
				return GameData.Serializer.Serializer.Serialize(combatSkillBreakStepCount, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 1)
			{
				int item22 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				List<CombatSkillDisplayData> characterEquipCombatSkillDisplayData = GetCharacterEquipCombatSkillDisplayData(item22);
				return GameData.Serializer.Serializer.Serialize(characterEquipCombatSkillDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 2)
			{
				int item26 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				short item27 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item27);
				CombatSkillDisplayData combatSkillDisplayDataOnce = GetCombatSkillDisplayDataOnce(item26, item27);
				return GameData.Serializer.Serializer.Serialize(combatSkillDisplayDataOnce, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 4:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 2)
			{
				int item15 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				List<short> item16 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item16);
				List<CombatSkillEffectDescriptionDisplayData> effectDescriptionData = GetEffectDescriptionData(item15, item16);
				return GameData.Serializer.Serializer.Serialize(effectDescriptionData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
			if (operation.ArgsCount == 0)
			{
				(int, int) item25 = CalcTaiwuExtraDeltaNeiliPerLoop(context);
				return GameData.Serializer.Serializer.Serialize(item25, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 6:
			if (operation.ArgsCount == 0)
			{
				IntList item17 = CalcTaiwuExtraDeltaNeiliAllocationPerLoop(context);
				return GameData.Serializer.Serializer.Serialize(item17, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 7:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 2)
			{
				int item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				short item7 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				CombatSkillDisplayData combatSkillPreviewDisplayDataOnce = GetCombatSkillPreviewDisplayDataOnce(item6, item7);
				return GameData.Serializer.Serializer.Serialize(combatSkillPreviewDisplayDataOnce, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 8:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 2)
			{
				int item23 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				short item24 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item24);
				int combatSkillBreakoutStepsMaxPower = GetCombatSkillBreakoutStepsMaxPower(item23, item24);
				return GameData.Serializer.Serializer.Serialize(combatSkillBreakoutStepsMaxPower, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 9:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 2)
			{
				int item18 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item18);
				short item19 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item19);
				List<SkillBreakPlateBonus> combatSkillBreakBonuses = GetCombatSkillBreakBonuses(item18, item19);
				return GameData.Serializer.Serializer.Serialize(combatSkillBreakBonuses, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 4)
			{
				int item10 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				short item11 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				byte item12 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				sbyte item13 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item13);
				bool item14 = SetActivePage(context, item10, item11, item12, item13);
				return GameData.Serializer.Serializer.Serialize(item14, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 11:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 4)
			{
				int item = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				short item2 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				byte item3 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				sbyte item4 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				bool item5 = DeActivePage(context, item, item2, item3, item4);
				return GameData.Serializer.Serializer.Serialize(item5, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
	{
		if (dataId == 0)
		{
			return;
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		if (dataId == 0)
		{
			return CheckModified_CombatSkills((CombatSkillKey)subId0, (ushort)subId1, dataPool);
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		if (dataId == 0)
		{
			ResetModifiedWrapper_CombatSkills((CombatSkillKey)subId0, (ushort)subId1);
			return;
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		if (dataId == 0)
		{
			return IsModifiedWrapper_CombatSkills((CombatSkillKey)subId0, (ushort)subId1);
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		if (influence.TargetIndicator.DataId == 0)
		{
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _combatSkills, list))
				{
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						BaseGameDataObject baseGameDataObject = list[i];
						List<DataUid> targetUids = influence.TargetUids;
						int count2 = targetUids.Count;
						for (int j = 0; j < count2; j++)
						{
							baseGameDataObject.InvalidateSelfAndInfluencedCache((ushort)targetUids[j].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCombatSkills, _dataStatesCombatSkills, influence, context);
				}
				list.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCombatSkills, _dataStatesCombatSkills, influence, context);
			}
			return;
		}
		throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		if (operation.DataId == 0)
		{
			ResponseProcessor.ProcessFixedObjectCollection(operation, pResult, _combatSkills);
			if (_pendingLoadingOperationIds == null)
			{
				return;
			}
			uint num = _pendingLoadingOperationIds.Peek();
			if (num == operation.Id)
			{
				_pendingLoadingOperationIds.Dequeue();
				if (_pendingLoadingOperationIds.Count <= 0)
				{
					_pendingLoadingOperationIds = null;
					InitializeInternalDataOfCollections();
					OnLoadedArchiveData();
					DomainManager.Global.CompleteLoading(7);
				}
			}
			return;
		}
		throw new Exception($"Unsupported dataId {operation.DataId}");
	}

	private void InitializeInternalDataOfCollections()
	{
		foreach (KeyValuePair<CombatSkillKey, CombatSkill> combatSkill in _combatSkills)
		{
			CombatSkill value = combatSkill.Value;
			value.CollectionHelperData = HelperDataCombatSkills;
			value.DataStatesOffset = _dataStatesCombatSkills.Create();
		}
	}
}
