using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Combat.Ai.Memory;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.GameDataBridge;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai;

public class AiController
{
	private readonly CombatCharacter _combatCharacter;

	public readonly AiEnvironment Environment;

	public readonly AiMemory Memory;

	public bool AllowDefense;

	public static readonly int[] AddHazardPerMark = new int[4] { 150, 75, 50, 150 };

	public static readonly int[] SpecialMarkAddHazardNeedCount = new int[4] { 0, 2, 4, 0 };

	private int _maxHazardValue;

	private int _addHazardPerMark;

	private int _specialMarkAddHazardNeedCount;

	private DefeatMarkCollection _lastMarks;

	private DataUid _defeatMarkUid;

	private string _defeatMarkDataHandlerKey;

	private const sbyte SkillMaxZeroScoreCount = 1;

	private const sbyte WeaponMaxZeroScoreCount = 2;

	private readonly Dictionary<short, int> _skillScoreDict = new Dictionary<short, int>();

	private readonly Dictionary<int, int> _weaponScoreDict = new Dictionary<int, int>();

	private readonly AiTree _aiTree;

	public bool IsCombatDifficultyLevel1 => _combatCharacter.IsAlly || DomainManager.World.GetCombatDifficulty() >= 1;

	public bool IsCombatDifficultyLevel2 => _combatCharacter.IsAlly || DomainManager.World.GetCombatDifficulty() >= 2;

	public AiController(CombatCharacter combatCharacter)
	{
		_combatCharacter = combatCharacter;
		Environment = new AiEnvironment(combatCharacter);
		Memory = new AiMemory(combatCharacter);
		AllowDefense = true;
		_aiTree = new AiTree(combatCharacter, new AiDataFile(ProperAiData()));
	}

	private AiDataItem ProperAiData()
	{
		if (_combatCharacter.IsTaiwu)
		{
			return Config.AiData.Instance[0];
		}
		CombatConfigItem combatConfig = DomainManager.Combat.CombatConfig;
		if (combatConfig.EnemyAi >= 0 && !_combatCharacter.IsAlly)
		{
			return Config.AiData.Instance[combatConfig.EnemyAi];
		}
		CharacterItem template = _combatCharacter.GetCharacter().Template;
		return Config.AiData.Instance[template.CombatAi];
	}

	public void Init()
	{
		InitHazard();
		Environment.RegisterCallbacks();
		Memory.RegisterCallbacks();
		if (!IsCombatDifficultyLevel2)
		{
			return;
		}
		List<short> learnedCombatSkills = _combatCharacter.GetCharacter().GetLearnedCombatSkills();
		int[] characterList = DomainManager.Combat.GetCharacterList(!_combatCharacter.IsAlly);
		foreach (int num in characterList)
		{
			if (num < 0)
			{
				continue;
			}
			List<short> attackSkillList = DomainManager.Combat.GetElement_CombatCharacterDict(num).GetAttackSkillList();
			foreach (short item in attackSkillList)
			{
				if (learnedCombatSkills.Contains(item))
				{
					Memory.EnemyRecordDict[num].SkillRecord.Add(item, (400, 0));
				}
			}
		}
	}

	public void UnInit()
	{
		UnInitHazard();
		Environment.UnregisterCallbacks();
		Memory.UnregisterCallbacks();
	}

	public void ClearMemories()
	{
		Memory.ClearMemories();
		_aiTree.ClearMemories();
	}

	private void InitHazard()
	{
		_maxHazardValue = Math.Min(750 + 750 * _combatCharacter.GetPersonalityValue(4) / 100, 1500);
		_addHazardPerMark = AddHazardPerMark[DomainManager.Combat.CombatConfig.CombatType];
		_specialMarkAddHazardNeedCount = SpecialMarkAddHazardNeedCount[DomainManager.Combat.CombatConfig.CombatType];
		_lastMarks = new DefeatMarkCollection();
		_lastMarks.Clear();
		_defeatMarkUid = new DataUid(8, 10, (ulong)_combatCharacter.GetId(), 50u);
		_defeatMarkDataHandlerKey = $"CombatAiHazard{_combatCharacter.GetId()}";
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_defeatMarkUid, _defeatMarkDataHandlerKey, OnDefeatMarkChanged);
		OnDefeatMarkChanged(_combatCharacter.GetDataContext(), _defeatMarkUid);
	}

	private void UnInitHazard()
	{
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_defeatMarkUid, _defeatMarkDataHandlerKey);
	}

	private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
	{
		DefeatMarkCollection defeatMarkCollection = _combatCharacter.GetDefeatMarkCollection();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (sbyte b = 0; b < 7; b++)
		{
			byte b2 = defeatMarkCollection.OuterInjuryMarkList[b];
			byte b3 = defeatMarkCollection.InnerInjuryMarkList[b];
			ByteList byteList = defeatMarkCollection.FlawMarkList[b];
			ByteList byteList2 = defeatMarkCollection.AcupointMarkList[b];
			if (b2 > _lastMarks.OuterInjuryMarkList[b])
			{
				num5 += _addHazardPerMark * (b2 - _lastMarks.OuterInjuryMarkList[b]);
			}
			if (b3 > _lastMarks.InnerInjuryMarkList[b])
			{
				num5 += _addHazardPerMark * (b3 - _lastMarks.InnerInjuryMarkList[b]);
			}
			num += byteList.Count;
			if (byteList.Count > _lastMarks.FlawMarkList[b].Count)
			{
				num2 += byteList.Count - _lastMarks.FlawMarkList[b].Count;
			}
			num3 += byteList2.Count;
			if (byteList2.Count > _lastMarks.AcupointMarkList[b].Count)
			{
				num4 += byteList2.Count - _lastMarks.AcupointMarkList[b].Count;
			}
			int val = Math.Max(byteList2.Count - _specialMarkAddHazardNeedCount, 0);
			num5 += _addHazardPerMark * Math.Min(byteList2.Count - _lastMarks.AcupointMarkList[b].Count, val);
			_lastMarks.OuterInjuryMarkList[b] = b2;
			_lastMarks.InnerInjuryMarkList[b] = b3;
			_lastMarks.FlawMarkList[b].Clear();
			_lastMarks.FlawMarkList[b].AddRange(byteList);
			_lastMarks.AcupointMarkList[b].Clear();
			_lastMarks.AcupointMarkList[b].AddRange(byteList2);
		}
		if (num2 > 0)
		{
			int val2 = Math.Max(num - _specialMarkAddHazardNeedCount, 0);
			num5 += _addHazardPerMark * Math.Min(num2, val2);
		}
		if (num4 > 0)
		{
			int val3 = Math.Max(num3 - _specialMarkAddHazardNeedCount, 0);
			num5 += _addHazardPerMark * Math.Min(num4, val3);
		}
		for (sbyte b4 = 0; b4 < 6; b4++)
		{
			byte b5 = defeatMarkCollection.PoisonMarkList[b4];
			if (b5 > _lastMarks.PoisonMarkList[b4])
			{
				num5 += _addHazardPerMark * (b5 - _lastMarks.PoisonMarkList[b4]);
			}
			_lastMarks.PoisonMarkList[b4] = b5;
		}
		if (defeatMarkCollection.MindMarkList.Count > _lastMarks.MindMarkList.Count)
		{
			int val4 = Math.Max(defeatMarkCollection.MindMarkList.Count - _specialMarkAddHazardNeedCount, 0);
			num5 += _addHazardPerMark * Math.Min(defeatMarkCollection.MindMarkList.Count - _lastMarks.MindMarkList.Count, val4);
		}
		_lastMarks.MindMarkList.Clear();
		_lastMarks.MindMarkList.AddRange(defeatMarkCollection.MindMarkList);
		if (defeatMarkCollection.DieMarkList.Count > _lastMarks.DieMarkList.Count)
		{
			num5 += _addHazardPerMark * (defeatMarkCollection.DieMarkList.Count - _lastMarks.DieMarkList.Count);
		}
		_lastMarks.DieMarkList.Clear();
		_lastMarks.DieMarkList.AddRange(defeatMarkCollection.DieMarkList);
		if (defeatMarkCollection.FatalDamageMarkCount > _lastMarks.FatalDamageMarkCount)
		{
			num5 += _addHazardPerMark * (defeatMarkCollection.FatalDamageMarkCount - _lastMarks.FatalDamageMarkCount);
		}
		_lastMarks.FatalDamageMarkCount = defeatMarkCollection.FatalDamageMarkCount;
		ChangeHazardValue(context, num5);
	}

	public void ChangeHazardValue(DataContext context, int hazardValue)
	{
		int hazardValue2 = Math.Clamp(_combatCharacter.GetHazardValue() + hazardValue, 0, _maxHazardValue);
		_combatCharacter.SetHazardValue(hazardValue2, context);
	}

	public CValuePercent GetHazardPercent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return CValuePercent.Parse(_combatCharacter.GetHazardValue(), _maxHazardValue);
	}

	public bool IsHazard()
	{
		DefeatMarkCollection defeatMarkCollection = _combatCharacter.GetDefeatMarkCollection();
		return _combatCharacter.GetHazardValue() >= _maxHazardValue || defeatMarkCollection.FatalDamageMarkCount >= 3 || defeatMarkCollection.DieMarkList.Count >= 3;
	}

	public bool CanFlee()
	{
		return DomainManager.Combat.CanFlee(_combatCharacter.IsAlly);
	}

	public short GetBestCombatSkill(IRandomSource random, sbyte equipType, bool requireCanUse = false, int costMaxFrame = -1, int costMaxBreath = -1, int costMaxStance = -1, short exceptSkill = -1)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		short result = -1;
		list.Clear();
		list.AddRange(_combatCharacter.GetCombatSkillList(equipType));
		list.RemoveAll((short id) => id < 0);
		list.Remove(exceptSkill);
		if (list.Count > 0)
		{
			_skillScoreDict.Clear();
			foreach (short item in list)
			{
				if (1 == 0)
				{
				}
				int num = equipType switch
				{
					1 => CalcAttackSkillScore(random, item, requireCanUse, costMaxFrame, costMaxBreath, costMaxStance), 
					3 => CalcDefenseSkillScore(item, requireCanUse, costMaxFrame, costMaxBreath, costMaxStance), 
					_ => 0, 
				};
				if (1 == 0)
				{
				}
				int value = num;
				_skillScoreDict.Add(item, value);
			}
			int hazardValue = _combatCharacter.GetHazardValue();
			if (!IsHazard() && hazardValue < _maxHazardValue / 2 && list.Count > 1)
			{
				int num2 = (list.Count - 1) * hazardValue / (_maxHazardValue / 2) + 1;
				CollectionUtils.Shuffle(DomainManager.Combat.Context.Random, list);
				list.Sort((short skillL, short skillR) => _skillScoreDict[skillR] - _skillScoreDict[skillL]);
				while (list.Count > num2)
				{
					list.RemoveAt(0);
				}
			}
			int num3 = 0;
			for (int num4 = 0; num4 < list.Count; num4++)
			{
				num3 = Math.Max(num3, _skillScoreDict[list[num4]]);
			}
			foreach (short key in _skillScoreDict.Keys)
			{
				if (_skillScoreDict[key] != num3)
				{
					list.Remove(key);
				}
			}
			if (list.Count > 0)
			{
				result = list.GetRandom(random);
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		return result;
	}

	public int GetBestWeaponIndex(IRandomSource random, short skillId = -1, bool needInRange = false, int exceptIndex = -1)
	{
		bool flag = false;
		_weaponScoreDict.Clear();
		for (int i = 0; i < 7; i++)
		{
			if (i != exceptIndex && _combatCharacter.GetWeapons()[i].IsValid() && (i >= 3 || _combatCharacter.GetWeaponData(i).GetDurability() > 0))
			{
				if (i >= 3 && (flag || !Config.Character.Instance[_combatCharacter.GetCharacter().GetTemplateId()].AllowUseFreeWeapon))
				{
					break;
				}
				if (i < 3)
				{
					flag = true;
				}
				int num = CalcWeaponScore(i, skillId, needInRange);
				if (num >= 0)
				{
					_weaponScoreDict.Add(i, num);
				}
			}
		}
		if (_weaponScoreDict.Count == 0)
		{
			return -1;
		}
		int num2 = 0;
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		foreach (int value in _weaponScoreDict.Values)
		{
			num2 = Math.Max(num2, value);
		}
		foreach (int key in _weaponScoreDict.Keys)
		{
			if (_weaponScoreDict[key] == num2)
			{
				list.Add(key);
			}
		}
		int random2 = list.GetRandom(random);
		ObjectPool<List<int>>.Instance.Return(list);
		return random2;
	}

	public unsafe int CalcAttackSkillScore(IRandomSource random, short skillId, bool requireCanUse = false, int costMaxFrame = -1, int costMaxBreath = -1, int costMaxStance = -1)
	{
		int id = _combatCharacter.GetId();
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(id, skillId));
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		GameData.Domains.Character.Character character = _combatCharacter.GetCharacter();
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		Personalities personalities = character.GetPersonalities();
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!_combatCharacter.IsAlly);
		if (requireCanUse && !DomainManager.Combat.GetCombatSkillData(id, skillId).GetCanUse())
		{
			return -1;
		}
		if (costMaxFrame >= 0 && DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(id, skillId)).GetPrepareTotalProgress() / DomainManager.Combat.GetSkillPrepareSpeed(_combatCharacter) > costMaxFrame)
		{
			return -1;
		}
		if (skillId >= 0 && DomainManager.Combat.CombatSkillDataExist(new CombatSkillKey(id, skillId)) && DomainManager.Combat.GetCombatSkillData(id, skillId).GetLeftCdFrame() != 0)
		{
			return -1;
		}
		if (costMaxBreath >= 0 || costMaxStance >= 0)
		{
			OuterAndInnerInts skillCostBreathStance = DomainManager.Combat.GetSkillCostBreathStance(id, element_CombatSkills);
			if (skillCostBreathStance.Outer > costMaxStance || skillCostBreathStance.Inner > costMaxBreath)
			{
				return -1;
			}
		}
		if (Memory.SelfRecord.SkillRecord.ContainsKey(skillId) && Memory.SelfRecord.SkillRecord[skillId].zeroScoreCount > 1)
		{
			return 0;
		}
		int num = Equipping.CalcCombatSkillScore(element_CombatSkills, combatSkillItem.EquipType, ref personalities, character.GetNeiliType(), organizationInfo.OrgTemplateId, character.GetIdealSect(), DomainManager.LegendaryBook.GetCharOwnedBookTypes(character.GetId()));
		sbyte innerRatio = element_CombatSkills.GetInnerRatio();
		OuterAndInnerInts penetrationResists = combatCharacter.GetCharacter().GetPenetrationResists();
		int num2 = penetrationResists.Outer - penetrationResists.Inner;
		if ((num2 > 0) ? (innerRatio >= 50) : ((num2 < 0) ? (innerRatio <= 50) : (innerRatio == 50)))
		{
			num += 150 * Math.Max(innerRatio, 100 - innerRatio) / 100;
		}
		HitOrAvoidInts hitValues = character.GetHitValues();
		HitOrAvoidInts hitValue = element_CombatSkills.GetHitValue();
		HitOrAvoidInts hitDistribution = element_CombatSkills.GetHitDistribution();
		for (sbyte b = 0; b < 4; b++)
		{
			if (hitDistribution.Items[b] > 0 && hitValues.Items[b] * hitValue.Items[b] / 100 >= Memory.EnemyRecordDict[combatCharacter.GetId()].MaxAvoid.Items[b])
			{
				num += hitDistribution.Items[b] * 2;
			}
		}
		if (combatSkillItem.HasAtkAcupointEffect)
		{
			num += 100;
		}
		if (organizationInfo.OrgTemplateId >= 0 && Config.Organization.Instance[organizationInfo.OrgTemplateId].AllowPoisoning)
		{
			PoisonsAndLevels poisons = combatSkillItem.Poisons;
			int num3 = 0;
			for (int i = 0; i < 6; i++)
			{
				num3 = Math.Max(num3, poisons.Values[i] * poisons.Levels[i]);
			}
			num += num3;
		}
		int bestWeaponIndex = GetBestWeaponIndex(random, skillId);
		int num4 = ((DomainManager.CombatSkill.GetSkillType(_combatCharacter.GetId(), skillId) != 5) ? bestWeaponIndex : 3);
		if (num4 >= 0)
		{
			GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(_combatCharacter.GetWeapons()[num4].Id);
			int indexByDistance = RecordCollection.GetIndexByDistance((short)(element_Weapons.GetMinDistance() - combatSkillItem.DistanceAdditionWhenCast));
			int indexByDistance2 = RecordCollection.GetIndexByDistance((short)(element_Weapons.GetMaxDistance() - combatSkillItem.DistanceAdditionWhenCast));
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < Memory.SelfRecord.MaxDamages.Length; j++)
			{
				OuterAndInnerInts outerAndInnerInts = Memory.SelfRecord.MaxDamages[j];
				num5 += outerAndInnerInts.Outer + outerAndInnerInts.Inner;
				if (indexByDistance <= j && j <= indexByDistance2)
				{
					num6 += outerAndInnerInts.Outer + outerAndInnerInts.Inner + Memory.SelfRecord.MaxMindDamages[j];
				}
			}
			if (num6 > 0 && num5 > 0)
			{
				sbyte personalityValue = _combatCharacter.GetPersonalityValue(3);
				int num7 = 200 - 200 * personalityValue / 100 * num6 * 100 / num5;
				num = Math.Max(num - num7, 0);
			}
		}
		num = ((!Memory.SelfRecord.SkillRecord.ContainsKey(skillId)) ? (num + Memory.SelfRecord.GetSkillRecordMaxScore()) : (num + Memory.SelfRecord.SkillRecord[skillId].score));
		if (bestWeaponIndex < 0)
		{
			num = -1;
		}
		else if (bestWeaponIndex >= 3)
		{
			num = num * 66 / 100;
		}
		return num;
	}

	public unsafe int CalcDefenseSkillScore(short skillId, bool requireCanUse = false, int costMaxFrame = -1, int costMaxBreath = -1, int costMaxStance = -1)
	{
		int id = _combatCharacter.GetId();
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(id, skillId));
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!_combatCharacter.IsAlly);
		if (requireCanUse && !DomainManager.Combat.GetCombatSkillData(id, skillId).GetCanUse())
		{
			return -1;
		}
		if (costMaxFrame >= 0 && DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(id, skillId)).GetPrepareTotalProgress() / DomainManager.Combat.GetSkillPrepareSpeed(_combatCharacter) > costMaxFrame)
		{
			return -1;
		}
		if (costMaxBreath >= 0 || costMaxStance >= 0)
		{
			OuterAndInnerInts skillCostBreathStance = DomainManager.Combat.GetSkillCostBreathStance(id, element_CombatSkills);
			if (skillCostBreathStance.Outer > costMaxStance || skillCostBreathStance.Inner > costMaxBreath)
			{
				return -1;
			}
		}
		sbyte b;
		HitOrAvoidInts hitValue = default(HitOrAvoidInts);
		if (combatCharacter.GetPreparingSkillId() >= 0)
		{
			CombatSkillKey objectId = new CombatSkillKey(combatCharacter.GetId(), combatCharacter.GetPreparingSkillId());
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills2 = DomainManager.CombatSkill.GetElement_CombatSkills(objectId);
			b = element_CombatSkills2.GetCurrInnerRatio();
			hitValue = element_CombatSkills2.GetHitValue();
		}
		else
		{
			CombatWeaponData weaponData = combatCharacter.GetWeaponData();
			HitOrAvoidShorts hitFactors = weaponData.Item.GetHitFactors();
			b = weaponData.GetInnerRatio();
			for (int i = 0; i < 4; i++)
			{
				hitValue.Items[i] = hitFactors.Items[i];
			}
		}
		int num = 0;
		num += combatSkillItem.AddOuterPenetrateResistOnCast * (100 - b) / 300;
		num += combatSkillItem.AddInnerPenetrateResistOnCast * b / 300;
		if ((element_CombatSkills.GetBouncePower().Inner > 0 || element_CombatSkills.GetBouncePower().Outer > 0) && DomainManager.Combat.GetCurrentDistance() < combatSkillItem.BounceDistance)
		{
			num += combatSkillItem.BounceRateOfOuterInjury * (100 - b) / 150;
			num += combatSkillItem.BounceRateOfInnerInjury * b / 150;
		}
		if (element_CombatSkills.GetFightBackPower() > 0 && DomainManager.Combat.InAttackRange(_combatCharacter))
		{
			num += element_CombatSkills.GetFightBackPower() / 2;
		}
		HitOrAvoidInts addAvoidValueOnCast = element_CombatSkills.GetAddAvoidValueOnCast();
		for (int j = 0; j < 4; j++)
		{
			num += addAvoidValueOnCast.Items[j] * hitValue.Items[j] / 200;
		}
		if (num > 0)
		{
			GameData.Domains.Character.Character character = _combatCharacter.GetCharacter();
			Personalities personalities = character.GetPersonalities();
			num += Equipping.CalcCombatSkillScore(element_CombatSkills, combatSkillItem.EquipType, ref personalities, _combatCharacter.GetNeiliType(), character.GetOrganizationInfo().OrgTemplateId, character.GetIdealSect(), DomainManager.LegendaryBook.GetCharOwnedBookTypes(_combatCharacter.GetId()));
		}
		return num;
	}

	private unsafe int CalcWeaponScore(int weaponIndex, short skillId = -1, bool needInRange = false)
	{
		CombatWeaponData weaponData = _combatCharacter.GetWeaponData(weaponIndex);
		if (skillId >= 0 && !DomainManager.Combat.WeaponHasNeedTrick(_combatCharacter, skillId, weaponData))
		{
			return -1;
		}
		ItemKey itemKey = _combatCharacter.GetWeapons()[weaponIndex];
		GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(itemKey.Id);
		WeaponItem weaponItem = Config.Weapon.Instance[itemKey.TemplateId];
		int num = weaponItem.InnerRatioAdjustRange * _combatCharacter.GetCharacter().GetInnerRatio() / 100;
		bool flag = itemKey.TemplateId == 0 || itemKey.TemplateId == 1 || itemKey.TemplateId == 2;
		if ((!flag && weaponData.GetDurability() <= 0) || (needInRange && !InWeaponAttackRange(itemKey)))
		{
			return -1;
		}
		if (Memory.SelfRecord.WeaponRecord.ContainsKey(itemKey.Id) && Memory.SelfRecord.WeaponRecord[itemKey.Id].zeroScoreCount > 2)
		{
			return 0;
		}
		HitOrAvoidShorts hitFactors = element_Weapons.GetHitFactors(_combatCharacter.GetId());
		int num2 = Config.Weapon.Instance[itemKey.TemplateId].Grade * 10 * DomainManager.Character.GetItemPower(_combatCharacter.GetId(), itemKey) / 100;
		if (!flag)
		{
			num2 += 50;
		}
		if (Memory.SelfRecord.WeaponRecord.ContainsKey(itemKey.Id))
		{
			num2 += Memory.SelfRecord.WeaponRecord[itemKey.Id].score;
		}
		if (skillId >= 0)
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_combatCharacter.GetId(), skillId));
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
			sbyte[] weaponTricks = weaponData.GetWeaponTricks();
			int num3 = 0;
			for (int i = 0; i < combatSkillItem.TrickCost.Count; i++)
			{
				NeedTrick needTrick = combatSkillItem.TrickCost[i];
				int num4 = weaponTricks.CountAll((sbyte type) => type == needTrick.TrickType);
				if (Math.Min(needTrick.NeedCount, (byte)2) > num4)
				{
					num3 = 0;
					break;
				}
				num3 += num4 * 100;
			}
			num2 += num3;
			HitOrAvoidInts hitDistribution = element_CombatSkills.GetHitDistribution();
			for (sbyte b = 0; b < 4; b++)
			{
				int num5 = hitDistribution.Items[b];
				short num6 = hitFactors.Items[b];
				if (num5 > 0 && num6 != 0)
				{
					num2 += num6 * num5 * 2 / 100;
				}
			}
			num2 = Math.Max(num2, 0);
			sbyte currInnerRatio = element_CombatSkills.GetCurrInnerRatio();
			int num7 = weaponItem.DefaultInnerRatio - num;
			int num8 = weaponItem.DefaultInnerRatio + num;
			int num9 = ((currInnerRatio > num8) ? (currInnerRatio - num8) : ((currInnerRatio < num7) ? (num7 - currInnerRatio) : 0));
			num2 += 50 - Math.Abs(num9 / 2);
			if (combatSkillItem.MostFittingWeaponID >= 0)
			{
				int num10 = itemKey.TemplateId - combatSkillItem.MostFittingWeaponID;
				if (0 <= num10 && num10 <= 8)
				{
					num2 += 200;
				}
			}
		}
		else
		{
			int num11 = 200;
			HitOrAvoidInts hitValues = _combatCharacter.GetCharacter().GetHitValues();
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			Dictionary<int, int> hitValueDict = ObjectPool<Dictionary<int, int>>.Instance.Get();
			list.Clear();
			hitValueDict.Clear();
			for (sbyte b2 = 0; b2 < 4; b2++)
			{
				list.Add(b2);
				hitValueDict.Add(b2, hitValues.Items[b2]);
			}
			list.Sort((sbyte typeL, sbyte typeR) => hitValueDict[typeR] - hitValueDict[typeL]);
			for (int num12 = 0; num12 < list.Count; num12++)
			{
				if (hitFactors.Items[list[num12]] > 0)
				{
					num2 += num11;
				}
				num11 -= 50;
			}
			ObjectPool<List<sbyte>>.Instance.Return(list);
			ObjectPool<Dictionary<int, int>>.Instance.Return(hitValueDict);
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!_combatCharacter.IsAlly);
			OuterAndInnerInts maxPenetrateResist = Memory.EnemyRecordDict[combatCharacter.GetId()].MaxPenetrateResist;
			int num13 = Math.Clamp(weaponItem.DefaultInnerRatio + num, 0, 100);
			int num14 = Math.Clamp(weaponItem.DefaultInnerRatio - num, 0, 100);
			if (maxPenetrateResist.Outer > maxPenetrateResist.Inner)
			{
				num2 += (100 - num14) * 3;
			}
			if (maxPenetrateResist.Inner > maxPenetrateResist.Outer)
			{
				num2 += num13 * 3;
			}
			else if (num14 <= 50 && num13 >= 50)
			{
				num2 += 300;
			}
			int indexByDistance = RecordCollection.GetIndexByDistance(element_Weapons.GetMinDistance());
			int indexByDistance2 = RecordCollection.GetIndexByDistance(element_Weapons.GetMaxDistance());
			int num15 = 200;
			int num16 = 0;
			int num17 = 0;
			for (int num18 = 0; num18 < Memory.SelfRecord.MaxDamages.Length; num18++)
			{
				OuterAndInnerInts outerAndInnerInts = Memory.SelfRecord.MaxDamages[num18];
				num16 += outerAndInnerInts.Outer + outerAndInnerInts.Inner;
				if (indexByDistance <= num18 && num18 <= indexByDistance2)
				{
					num17 += outerAndInnerInts.Outer + outerAndInnerInts.Inner + Memory.SelfRecord.MaxMindDamages[num18];
				}
			}
			if (num17 > 0 && num16 > 0)
			{
				sbyte personalityValue = _combatCharacter.GetPersonalityValue(3);
				int num19 = num15 - num15 * personalityValue / 100 * num17 * 100 / num16;
				num2 = Math.Max(num2 - num19, 0);
			}
		}
		return num2;
	}

	public bool InWeaponAttackRange(ItemKey weaponKey)
	{
		(int, int) weaponAttackRange = DomainManager.Item.GetWeaponAttackRange(_combatCharacter.GetId(), weaponKey);
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		return weaponAttackRange.Item1 <= currentDistance && currentDistance <= weaponAttackRange.Item2;
	}

	public short GetTargetDistance()
	{
		return _combatCharacter.AiTargetDistance;
	}

	public void Update(DataContext context)
	{
		if (!AutoStopJumpInReach())
		{
			_aiTree.Update();
			UpdateTargetDistance(context);
		}
	}

	public void UpdateOnlyMove(DataContext context)
	{
		UpdateTargetDistance(context);
		AutoStopJumpInReach();
	}

	public bool AutoStopJumpInReach()
	{
		if (!_combatCharacter.KeepMoving || !_combatCharacter.MoveData.PreparingJumpMove() || (_combatCharacter.IsAlly && (!DomainManager.Combat.AiOptions.AutoMove || _combatCharacter.PlayerControllingMove)))
		{
			return false;
		}
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		short targetDistance = _combatCharacter.GetTargetDistance();
		if (targetDistance < 0 || (_combatCharacter.MoveForward ? (currentDistance <= targetDistance) : (currentDistance >= targetDistance)) || !MoveCanApproachTargetDistance(targetDistance))
		{
			DomainManager.Combat.SetMoveState(0, _combatCharacter.IsAlly);
		}
		else if (_combatCharacter.MoveData.CanPartlyJump)
		{
			short jumpPreparedDistance = _combatCharacter.GetJumpPreparedDistance();
			if (jumpPreparedDistance > 0 && (_combatCharacter.MoveForward ? (currentDistance - jumpPreparedDistance <= targetDistance) : (currentDistance + jumpPreparedDistance >= targetDistance)))
			{
				DomainManager.Combat.SetMoveState(0, _combatCharacter.IsAlly);
			}
		}
		return true;
	}

	public void UpdateTargetDistance(DataContext context)
	{
		if (!_combatCharacter.PlayerControllingMove)
		{
			if (!_combatCharacter.IsAlly || DomainManager.Combat.IsAiMoving)
			{
				_combatCharacter.SetTargetDistance(GetTargetDistance(), context);
			}
			else
			{
				_combatCharacter.SetTargetDistance(_combatCharacter.PlayerTargetDistance, context);
			}
			short targetDistance = _combatCharacter.GetTargetDistance();
			short currentDistance = DomainManager.Combat.GetCurrentDistance();
			byte b = (byte)((targetDistance >= 0 && targetDistance != currentDistance && MoveCanApproachTargetDistance(targetDistance)) ? ((targetDistance < currentDistance) ? 1 : 2) : 0);
			byte b2 = (byte)(_combatCharacter.KeepMoving ? (_combatCharacter.MoveForward ? 1 : 2) : 0);
			if (b != b2)
			{
				DomainManager.Combat.SetMoveState(b, _combatCharacter.IsAlly);
			}
		}
	}

	private bool MoveCanApproachTargetDistance(short targetDistance)
	{
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		bool flag = targetDistance < currentDistance;
		if (!_combatCharacter.MoveData.IsJumpMove(flag))
		{
			return true;
		}
		if (_combatCharacter.IsAlly)
		{
			return Math.Abs(currentDistance - targetDistance) >= DomainManager.Extra.GetJumpThreshold(_combatCharacter.MoveData.JumpMoveSkillId);
		}
		int num = (_combatCharacter.MoveData.CanPartlyJump ? 10 : (flag ? _combatCharacter.MoveData.MaxJumpForwardDist : _combatCharacter.MoveData.MaxJumpBackwardDist));
		return Math.Abs(currentDistance - num * (flag ? 1 : (-1)) - targetDistance) < Math.Abs(currentDistance - targetDistance);
	}
}
