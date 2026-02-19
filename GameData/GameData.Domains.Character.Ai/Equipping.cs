using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using Config.ConfigCells.Character;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Building;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.ParallelModifications;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.Ai;

public class Equipping
{
	public ref struct EquipCombatSkillContext
	{
		public bool IsTaiwu;

		public bool EquipBestSkillsForWeapon;

		public ItemKey[] Equipments;

		public sbyte SlotCostTemplateAdjust;

		public Personalities Personalities;

		public sbyte NeiliType;

		public sbyte OrgTemplateId;

		public sbyte IdealSectId;

		public List<sbyte> OwnedLegendaryBookTypes;

		public Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> CharacterCombatSkills;

		public unsafe sbyte* SlotTotalCounts;

		public CombatSkillEquipment EquippedSkills;
	}

	private class GradeComparer : IComparer<(CombatSkillItem skillCfg, int index)>
	{
		public int Compare((CombatSkillItem skillCfg, int index) x, (CombatSkillItem skillCfg, int index) y)
		{
			return x.skillCfg.Grade - y.skillCfg.Grade;
		}
	}

	public struct BreakoutCombatSkillContext
	{
		public IRandomSource Random;

		public Character Character;

		public CombatSkillShorts Qualifications;

		public bool IsCreatedWithFixedTemplate;

		public sbyte BehaviorType;

		public int CharExp;

		public int ExpPerMonth;

		public Injuries Injuries;

		public short DisorderOfQi;

		public BreakoutCombatSkillContext(IRandomSource random, Character character)
		{
			Random = random;
			Character = character;
			Qualifications = character.GetCombatSkillQualifications();
			IsCreatedWithFixedTemplate = character.GetCreatingType() != 1;
			CharExp = character.GetExp();
			ExpPerMonth = character.GetExpPerMonth();
			Injuries = character.GetInjuries();
			DisorderOfQi = character.GetDisorderOfQi();
			BehaviorType = character.GetBehaviorType();
		}
	}

	private struct BreakPlateBonusContext
	{
		public readonly IRandomSource Random;

		public readonly Character Character;

		public readonly Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> CharCombatSkills;

		public HashSet<int> UsedRelatedCharIds;

		public BreakPlateBonusContext(IRandomSource random, Character character)
		{
			Random = random;
			Character = character;
			CharCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(character.GetId());
			UsedRelatedCharIds = null;
		}
	}

	private readonly List<GameData.Domains.CombatSkill.CombatSkill>[] _availableCombatSkills;

	private List<short> _masteredSkills;

	private readonly CombatSkillEquipment _equippedCombatSkills;

	private short[] _combatSkillAttainmentPanels;

	private readonly LocalObjectPool<SectCandidateSkills> _sectCandidateSkillsPool;

	private readonly List<SectCandidateSkills> _sectCandidateSkillInfos;

	private readonly List<SectCandidateSkills> _sortedSectCandidateSkillInfos;

	private static readonly IComparer<(CombatSkillItem skillCfg, int index)> Comparer = new GradeComparer();

	private List<CombatSkillInitialBreakoutData> _brokenOutCombatSkills;

	private readonly List<GameData.Domains.CombatSkill.CombatSkill>[] _categorizedCombatSkillsByGrade;

	private readonly List<(CombatSkillItem skillCfg, int index)> _brokenOutNeigongList;

	private readonly HashSet<int> _usedRelatedCharIds;

	private readonly List<ShortPair> _skillBreakBonusWeights = new List<ShortPair>();

	private const sbyte EnsuredSuccessStepCount = 20;

	[Obsolete]
	private const sbyte RequiredStepMinCount = 10;

	[Obsolete]
	private const sbyte RequiredStepBaseCount = 15;

	[Obsolete]
	private const sbyte BreakoutBaseSuccessRate = 75;

	[Obsolete]
	private const short BreakoutFailureDefeatMarkCount = 4;

	[Obsolete]
	private const short BreakoutFailureDisorderOfQi = 2000;

	private HashSet<int> _askingForHelpSkills = new HashSet<int>();

	private readonly List<(GameData.Domains.CombatSkill.CombatSkill combatSkill, int score)> _canUpdateCombatSkills = new List<(GameData.Domains.CombatSkill.CombatSkill, int)>();

	private readonly List<PersonalNeed> _newPersonalNeeds = new List<PersonalNeed>();

	private List<ItemKey> _consumedItems = new List<ItemKey>();

	private List<(GameData.Domains.CombatSkill.CombatSkill combatSkill, ushort activationState)> _newlyActivatedCombatSkills = new List<(GameData.Domains.CombatSkill.CombatSkill, ushort)>();

	private List<GameData.Domains.CombatSkill.CombatSkill> _failedToBreakoutCombatSkills = new List<GameData.Domains.CombatSkill.CombatSkill>();

	private List<(short skillTemplateId, int startIndex, SerializableList<SkillBreakPlateBonus> bonuses)> _modifiedBreakPlateBonuses = new List<(short, int, SerializableList<SkillBreakPlateBonus>)>();

	private ItemKey[] _equippedItems;

	private readonly List<(ItemKey weapon, int score)> _availableWeapons;

	private readonly List<(short itemTemplateId, short count)> _suitableWeapons;

	private readonly HashSet<short> _fixedBestWeapons;

	private readonly List<GameData.Domains.Item.Armor> _availableHelms;

	private readonly List<GameData.Domains.Item.Armor> _availableTorsos;

	private readonly List<GameData.Domains.Item.Armor> _availableBracers;

	private readonly List<GameData.Domains.Item.Armor> _availableBoots;

	private readonly List<GameData.Domains.Item.Accessory> _availableAccessories;

	private readonly List<GameData.Domains.Item.Clothing> _availableClothing;

	private readonly List<GameData.Domains.Item.Carrier> _availableCarriers;

	private readonly List<(CombatSkillItem skillCfg, bool canObtainNeili)> _candidateCombatSkillsForLooping;

	private static readonly IComparer<sbyte> ReverseComparer = new ReverseComparerSbyte();

	private readonly List<(GameData.Domains.Item.SkillBook book, int learnedSkillIndex, byte readingPage)> _availableReadingBooks;

	private readonly List<short> _hasPersonalNeedToReadBooks;

	private readonly List<sbyte> _hasPersonalNeedToLearnCombatSkillTypes;

	private readonly List<sbyte> _hasPersonalNeedToLearnLifeSkillTypes;

	public Equipping()
	{
		_brokenOutCombatSkills = new List<CombatSkillInitialBreakoutData>(32);
		_brokenOutNeigongList = new List<(CombatSkillItem, int)>();
		_combatSkillAttainmentPanels = new short[126];
		_sectCandidateSkillsPool = new LocalObjectPool<SectCandidateSkills>(15, 30);
		_sectCandidateSkillInfos = new List<SectCandidateSkills>();
		_sortedSectCandidateSkillInfos = new List<SectCandidateSkills>();
		_availableCombatSkills = new List<GameData.Domains.CombatSkill.CombatSkill>[5];
		for (int i = 0; i < 5; i++)
		{
			_availableCombatSkills[i] = new List<GameData.Domains.CombatSkill.CombatSkill>();
		}
		_equippedCombatSkills = new CombatSkillEquipment();
		_equippedCombatSkills.Set(new CombatSkillPlan());
		_categorizedCombatSkillsByGrade = new List<GameData.Domains.CombatSkill.CombatSkill>[9];
		for (sbyte b = 0; b <= 8; b++)
		{
			_categorizedCombatSkillsByGrade[b] = new List<GameData.Domains.CombatSkill.CombatSkill>();
		}
		_masteredSkills = new List<short>();
		_candidateCombatSkillsForLooping = new List<(CombatSkillItem, bool)>();
		_equippedItems = new ItemKey[12];
		_availableWeapons = new List<(ItemKey, int)>();
		_suitableWeapons = new List<(short, short)>();
		_fixedBestWeapons = new HashSet<short>();
		_availableHelms = new List<GameData.Domains.Item.Armor>();
		_availableTorsos = new List<GameData.Domains.Item.Armor>();
		_availableBracers = new List<GameData.Domains.Item.Armor>();
		_availableBoots = new List<GameData.Domains.Item.Armor>();
		_availableAccessories = new List<GameData.Domains.Item.Accessory>();
		_availableClothing = new List<GameData.Domains.Item.Clothing>();
		_availableCarriers = new List<GameData.Domains.Item.Carrier>();
		_availableReadingBooks = new List<(GameData.Domains.Item.SkillBook, int, byte)>();
		_hasPersonalNeedToReadBooks = new List<short>();
		_hasPersonalNeedToLearnCombatSkillTypes = new List<sbyte>();
		_hasPersonalNeedToLearnLifeSkillTypes = new List<sbyte>();
		_usedRelatedCharIds = new HashSet<int>();
	}

	public void SetInitialCombatSkillBreakouts(DataContext context, Character character)
	{
		if (character.GetAgeGroup() != 0)
		{
			var (list, list2, neiliProportion, extraNeiliAllocationProgress) = ParallelSetInitialCombatSkillBreakouts(context, character, recordModification: false);
			if (list != null && list.Count > 0)
			{
				ComplementSetInitialCombatSkillBreakouts(context, list, character, neiliProportion, extraNeiliAllocationProgress);
			}
			if (list2 != null && list2.Count > 0)
			{
				ApplyBreakPlateBonuses(context, character.GetId(), list2);
			}
		}
	}

	public void SetInitialCombatSkillAttainmentPanels(DataContext context, Character character)
	{
		if (character.GetAgeGroup() != 0)
		{
			short[] array = ParallelSetInitialCombatSkillAttainmentPanels(context, character, recordModification: false);
			if (array != null)
			{
				ComplementSetInitialCombatSkillAttainmentPanels(context, character, array);
			}
		}
	}

	public void SelectEquipments(DataContext context, Character character, bool isOutOfTaiwuGroup, bool removeUnequippedEquipment = false)
	{
		if (character.GetAgeGroup() != 0)
		{
			SelectEquipmentsModification mod = ParallelSelectEquipments(context, character, isOutOfTaiwuGroup, removeUnequippedEquipment, recordModification: false);
			ComplementSelectEquipments(context, mod);
		}
	}

	public void SelectEquipmentsByCombatConfig(DataContext context, Character character, short combatTemplateId, bool isOutOfTaiwuGroup, bool removeUnequippedEquipment = false)
	{
		if (character.GetAgeGroup() != 0)
		{
			SelectEquipmentsModification mod = ParallelSelectEquipmentsByCombatConfig(context, character, combatTemplateId, isOutOfTaiwuGroup, removeUnequippedEquipment, recordModification: false);
			ComplementSelectEquipments(context, mod);
		}
	}

	public unsafe SelectEquipmentsModification ParallelSelectEquipments(DataContext context, Character character, bool isOutOfTaiwuGroup, bool removeUnequippedEquipment = false, bool recordModification = true)
	{
		SelectEquipmentsModification selectEquipmentsModification = new SelectEquipmentsModification(character, removeUnequippedEquipment);
		int id = character.GetId();
		sbyte* skillSlotTotalCounts = stackalloc sbyte[5];
		CharacterCombatSkillConfiguration characterCombatSkillConfiguration = DomainManager.Extra.TryGetCharacterCombatSkillConfiguration(id);
		bool flag = isOutOfTaiwuGroup || !(characterCombatSkillConfiguration?.IsCombatSkillLocked ?? false);
		bool flag2 = isOutOfTaiwuGroup || !(characterCombatSkillConfiguration?.IsNeiliAllocationLocked ?? false);
		bool flag3 = !character.IsCreatedWithFixedTemplate() && (isOutOfTaiwuGroup || !DomainManager.Extra.GetManualChangeEquipGroupCharIds().Contains(id));
		ChooseLoopingNeigong(character, selectEquipmentsModification);
		if (flag)
		{
			EquipCombatSkills(character, skillSlotTotalCounts, -1, selectEquipmentsModification);
		}
		if (flag2)
		{
			AllocateNeili(character, skillSlotTotalCounts, selectEquipmentsModification);
		}
		if (flag3)
		{
			EquipItems(character, selectEquipmentsModification);
		}
		if (recordModification && (selectEquipmentsModification.EquippedSkillsChanged || selectEquipmentsModification.NeiliAllocationChanged || selectEquipmentsModification.LoopingNeigongChanged || selectEquipmentsModification.EquippedItems != null || selectEquipmentsModification.MasteredSkillsChanged))
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.SelectEquipments);
			parallelModificationsRecorder.RecordParameterClass(selectEquipmentsModification);
		}
		return selectEquipmentsModification;
	}

	private unsafe SelectEquipmentsModification ParallelSelectEquipmentsByCombatConfig(DataContext context, Character character, short combatConfigTemplateId, bool isOutOfTaiwuGroup, bool removeUnequippedEquipment = false, bool recordModification = true)
	{
		SelectEquipmentsModification selectEquipmentsModification = new SelectEquipmentsModification(character, removeUnequippedEquipment);
		sbyte* skillSlotTotalCounts = stackalloc sbyte[5];
		int id = character.GetId();
		CharacterCombatSkillConfiguration characterCombatSkillConfiguration = DomainManager.Extra.TryGetCharacterCombatSkillConfiguration(id);
		bool flag = isOutOfTaiwuGroup || !(characterCombatSkillConfiguration?.IsCombatSkillLocked ?? false);
		bool flag2 = isOutOfTaiwuGroup || !(characterCombatSkillConfiguration?.IsNeiliAllocationLocked ?? false);
		bool flag3 = !character.IsCreatedWithFixedTemplate() && (isOutOfTaiwuGroup || !DomainManager.Extra.GetManualChangeEquipGroupCharIds().Contains(id));
		ChooseLoopingNeigong(character, selectEquipmentsModification);
		if (flag)
		{
			EquipCombatSkills(character, skillSlotTotalCounts, combatConfigTemplateId, selectEquipmentsModification);
		}
		if (flag2)
		{
			AllocateNeili(character, skillSlotTotalCounts, selectEquipmentsModification);
		}
		if (flag3)
		{
			EquipItems(character, selectEquipmentsModification);
		}
		if (recordModification && (selectEquipmentsModification.EquippedSkillsChanged || selectEquipmentsModification.NeiliAllocationChanged || selectEquipmentsModification.LoopingNeigongChanged || selectEquipmentsModification.EquippedItems != null || selectEquipmentsModification.MasteredSkillsChanged))
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.SelectEquipments);
			parallelModificationsRecorder.RecordParameterClass(selectEquipmentsModification);
		}
		return selectEquipmentsModification;
	}

	public static void ComplementSelectEquipments(DataContext context, SelectEquipmentsModification mod)
	{
		Character character = mod.Character;
		int id = character.GetId();
		if (mod.EquippedSkillsChanged)
		{
			character.ApplyCombatSkillEquipmentModification(context, mod.CombatSkillEquipment);
		}
		if (mod.NeiliAllocationChanged)
		{
			character.SpecifyBaseNeiliAllocation(context, mod.NeiliAllocation);
		}
		if (mod.LoopingNeigongChanged)
		{
			character.SetLoopingNeigong(mod.LoopingNeigong, context);
		}
		if (mod.EquippedItems != null)
		{
			character.ChangeEquipment(context, mod.EquippedItems);
		}
		if (mod.RemoveUnequippedEquipment)
		{
			character.RemoveUnequippedEquipment(context);
		}
		if (mod.PersonalNeedChanged)
		{
			character.SetPersonalNeeds(character.GetPersonalNeeds(), context);
		}
		if (mod.MasteredSkillsChanged)
		{
			DomainManager.Extra.SetCharacterMasteredCombatSkills(context, id, mod.MasteredCombatSkills);
		}
	}

	public unsafe void EquipCombatSkills(DataContext context, Character character, short combatConfigTemplateId)
	{
		int id = character.GetId();
		SelectEquipmentsModification selectEquipmentsModification = new SelectEquipmentsModification(character, removeUnequippedEquipment: false);
		sbyte* skillSlotTotalCounts = stackalloc sbyte[5];
		EquipCombatSkills(character, skillSlotTotalCounts, combatConfigTemplateId, selectEquipmentsModification);
		if (selectEquipmentsModification.MasteredSkillsChanged)
		{
			AdaptableLog.Info($"{character} changed mastered skills.");
			DomainManager.Extra.SetCharacterMasteredCombatSkills(context, id, selectEquipmentsModification.MasteredCombatSkills);
		}
		if (!selectEquipmentsModification.EquippedSkillsChanged)
		{
			return;
		}
		if (selectEquipmentsModification.GenericSkillSlotAllocation != null)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			if (id == taiwuCharId)
			{
				AdaptableLog.Info($"{character} changed generic skill slot allocation.");
				DomainManager.Taiwu.SetGenericGridAllocation(context, selectEquipmentsModification.GenericSkillSlotAllocation);
			}
			else if (character.GetLeaderId() == taiwuCharId)
			{
				CharacterCombatSkillConfiguration characterCombatSkillConfiguration = DomainManager.Extra.TryGetCharacterCombatSkillConfiguration(id);
				if (characterCombatSkillConfiguration != null)
				{
					AdaptableLog.Info($"{character} changed generic skill slot allocation.");
					byte[] genericGridAllocation = characterCombatSkillConfiguration.CurrentEquipPlan.GenericGridAllocation;
					for (int i = 0; i < genericGridAllocation.Length; i++)
					{
						genericGridAllocation[i] = selectEquipmentsModification.GenericSkillSlotAllocation[i];
					}
				}
			}
		}
		AdaptableLog.Info($"{character} changed equipped skills.");
		character.ApplyCombatSkillEquipmentModification(context, selectEquipmentsModification.CombatSkillEquipment);
	}

	private unsafe void EquipCombatSkills(Character character, sbyte* skillSlotTotalCounts, short combatConfigTemplateId, SelectEquipmentsModification mod)
	{
		int id = character.GetId();
		for (int i = 0; i < 5; i++)
		{
			_availableCombatSkills[i].Clear();
		}
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(id);
		CombatConfigItem item = CombatConfig.Instance.GetItem(combatConfigTemplateId);
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item2 in charCombatSkills)
		{
			item2.Deconstruct(out var key, out var value);
			short num = key;
			GameData.Domains.CombatSkill.CombatSkill combatSkill = value;
			sbyte equipType = Config.CombatSkill.Instance[num].EquipType;
			if (!combatSkill.GetRevoked() && (item == null || MatchCombatSkillByCombatConfig(num, item)))
			{
				_availableCombatSkills[equipType].Add(combatSkill);
			}
		}
		_equippedCombatSkills.OfflineClear();
		CombatSkillEquipment combatSkillEquipment = character.GetCombatSkillEquipment();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		bool isTaiwu = id == taiwuCharId;
		EquipCombatSkillContext context = new EquipCombatSkillContext
		{
			IsTaiwu = isTaiwu,
			EquipBestSkillsForWeapon = (character.GetLeaderId() == taiwuCharId),
			Equipments = character.GetEquipment(),
			Personalities = character.GetPersonalities(),
			NeiliType = character.GetNeiliType(),
			OrgTemplateId = character.GetOrganizationInfo().OrgTemplateId,
			IdealSectId = character.GetIdealSect(),
			OwnedLegendaryBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(id),
			EquippedSkills = _equippedCombatSkills,
			CharacterCombatSkills = charCombatSkills,
			SlotTotalCounts = skillSlotTotalCounts
		};
		*skillSlotTotalCounts = character.GetCombatSkillSlotCountNeigong();
		SelectCombatSkills(ref context, 0);
		Span<sbyte> slotCounts = new Span<sbyte>(skillSlotTotalCounts, 5);
		sbyte combatSkillSlotCounts = character.GetCombatSkillSlotCounts(slotCounts, context.EquippedSkills.Neigong);
		Span<byte> result = stackalloc byte[4];
		Span<byte> currSlotCounts = stackalloc byte[4];
		for (sbyte b = 1; b < 5; b++)
		{
			currSlotCounts[b - 1] = (byte)character.GetCombatSkillBasicSlotCount(b, context.EquippedSkills.Neigong);
		}
		AllocateGenericSkillSlots(ref result, ref currSlotCounts, combatSkillSlotCounts);
		for (sbyte b2 = 0; b2 < 4; b2++)
		{
			int num2 = b2 + 1;
			int num3 = skillSlotTotalCounts[num2] + result[b2];
			skillSlotTotalCounts[num2] = (sbyte)num3;
		}
		if (context.IsTaiwu)
		{
			mod.GenericSkillSlotAllocation = new byte[4];
			for (int j = 0; j < 4; j++)
			{
				mod.GenericSkillSlotAllocation[j] = result[j];
			}
		}
		SelectCombatSkills(ref context, 1);
		SelectCombatSkills(ref context, 2);
		SelectCombatSkills(ref context, 3);
		SelectCombatSkills(ref context, 4);
		if (!combatSkillEquipment.EqualsTo(context.EquippedSkills))
		{
			combatSkillEquipment.CopyFrom(context.EquippedSkills);
			mod.CombatSkillEquipment = combatSkillEquipment;
			mod.EquippedSkillsChanged = true;
		}
	}

	private void AllocateGenericSkillSlots(ref Span<byte> result, ref Span<byte> currSlotCounts, int genericSlotsCount)
	{
		result.Fill(0);
		do
		{
			int num = int.MaxValue;
			int num2 = -1;
			for (sbyte b = 1; b < 5; b++)
			{
				int index = b - 1;
				if (result[index] + currSlotCounts[index] < CombatSkillHelper.MaxSlotCounts[b])
				{
					int genericAllocationNextCost = CombatSkillHelper.GetGenericAllocationNextCost(b, result[index]);
					if (genericAllocationNextCost <= num && genericAllocationNextCost <= genericSlotsCount)
					{
						num = genericAllocationNextCost;
						num2 = b;
					}
				}
			}
			if (num2 < 0)
			{
				break;
			}
			result[num2 - 1]++;
			genericSlotsCount -= num;
		}
		while (genericSlotsCount > 0);
	}

	private unsafe void SelectCombatSkills(ref EquipCombatSkillContext context, sbyte equipType)
	{
		sbyte b = context.SlotTotalCounts[equipType];
		if (b <= 0)
		{
			return;
		}
		List<GameData.Domains.CombatSkill.CombatSkill> list = _availableCombatSkills[equipType];
		int count = list.Count;
		int* ptr = stackalloc int[count];
		for (int i = 0; i < count; i++)
		{
			GameData.Domains.CombatSkill.CombatSkill combatSkill = list[i];
			short skillTemplateId = combatSkill.GetId().SkillTemplateId;
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
			if (combatSkillItem.ScoreBonusType == -2)
			{
				ptr[i] = 2147418112 + skillTemplateId;
				continue;
			}
			short num = CalcCombatSkillScore(combatSkill, equipType, ref context.Personalities, context.NeiliType, context.OrgTemplateId, context.IdealSectId, context.OwnedLegendaryBookTypes);
			if (context.EquipBestSkillsForWeapon)
			{
				num += CalcCombatSkillScoreForCurrWeapons(combatSkillItem, context.Equipments);
			}
			ptr[i] = (num << 16) + skillTemplateId;
		}
		CollectionUtils.Sort(ptr, count);
		int num2 = 0;
		context.EquippedSkills.OfflineEnsureCapacity(equipType, b);
		ref ArraySegmentList<short> reference = ref context.EquippedSkills[equipType];
		reference.Clear();
		if (context.IsTaiwu)
		{
			Character taiwu = DomainManager.Taiwu.GetTaiwu();
			for (int j = 0; j < count; j++)
			{
				if (num2 >= b)
				{
					break;
				}
				int num3 = count - j - 1;
				int num4 = ptr[num3];
				short num5 = (short)num4;
				sbyte combatSkillGridCost = taiwu.GetCombatSkillGridCost(num5);
				if (num2 + combatSkillGridCost <= b)
				{
					reference.Add(num5);
					num2 += combatSkillGridCost;
				}
			}
			return;
		}
		for (int k = 0; k < count; k++)
		{
			if (num2 >= b)
			{
				break;
			}
			int num6 = count - k - 1;
			int num7 = ptr[num6];
			short num8 = (short)num7;
			var (b2, flag) = CalcSlotCostInfo(context, num8);
			if (num2 + b2 <= b)
			{
				reference.Add(num8);
				num2 += b2;
			}
		}
	}

	private static (sbyte slotCost, bool isMastered) CalcSlotCostInfo(EquipCombatSkillContext context, short skillTemplateId)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
		sbyte gridCost = combatSkillItem.GridCost;
		return (slotCost: Math.Max(gridCost, 1), isMastered: false);
	}

	private static short CalcCombatSkillScoreForCurrWeapons(CombatSkillItem skillCfg, ItemKey[] equipments)
	{
		int num = 0;
		for (int i = 0; i <= 2; i++)
		{
			ItemKey itemKey = equipments[i];
			if (itemKey.ItemType == 0)
			{
				GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(itemKey.Id);
				WeaponItem weaponItem = Config.Weapon.Instance[itemKey.TemplateId];
				if (weaponItem.GroupId == skillCfg.MostFittingWeaponID)
				{
					num += 50 * ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
				}
				if (element_Weapons.TricksMatchCombatSkill(skillCfg))
				{
					num += 100;
				}
			}
		}
		return (short)num;
	}

	public unsafe static short CalcCombatSkillScore(GameData.Domains.CombatSkill.CombatSkill skill, sbyte equipType, ref Personalities personalities, sbyte neiliType, sbyte orgTemplateId, sbyte idealSectTemplateId, List<sbyte> ownedLegendaryBookTypes)
	{
		short skillTemplateId = skill.GetId().SkillTemplateId;
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
		if (combatSkillItem.ScoreBonusType == -2)
		{
			return short.MaxValue;
		}
		short num = 0;
		if (combatSkillItem.SectId == orgTemplateId)
		{
			num += (short)(150 + personalities.Items[0]);
		}
		if (combatSkillItem.SectId == idealSectTemplateId)
		{
			num += (short)(75 + personalities.Items[2]);
		}
		if (!CheckCounterWithNeiliType(combatSkillItem.FiveElements, neiliType))
		{
			num += (short)(150 + personalities.Items[0]);
		}
		num += (short)(50 * combatSkillItem.Grade);
		num += skill.GetPower();
		if (CombatSkillStateHelper.IsBrokenOut(skill.GetActivationState()))
		{
			num += 100;
		}
		num += (short)(50 * (combatSkillItem.GridCost - 1));
		if (ownedLegendaryBookTypes != null && equipType == 1 && ownedLegendaryBookTypes.Contains(combatSkillItem.Type))
		{
			num += 300;
		}
		return num;
	}

	public static bool CheckCounterWithNeiliType(sbyte fiveElementsType, sbyte neiliTypeId)
	{
		NeiliTypeItem neiliTypeItem = NeiliType.Instance[neiliTypeId];
		return neiliTypeItem.InjuryOnUseType == fiveElementsType || neiliTypeItem.MaxPowerChange[fiveElementsType] < 0;
	}

	public static bool CheckCounterWithTargetFiveElementsType(short fiveElementsType, sbyte targetFiveElementsType)
	{
		return targetFiveElementsType == 5 || (FiveElementsType.Countered[targetFiveElementsType] != fiveElementsType && FiveElementsType.Countering[targetFiveElementsType] != fiveElementsType);
	}

	public short[] ParallelSetInitialCombatSkillAttainmentPanels(DataContext context, Character character, bool recordModification = true)
	{
		short[] array = _combatSkillAttainmentPanels ?? new short[126];
		CombatSkillAttainmentPanelsHelper.Initialize(array);
		int id = character.GetId();
		sbyte orgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
		sbyte idealSect = character.GetIdealSect();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(id);
		for (sbyte b = 0; b < 14; b++)
		{
			SetCombatSkillAttainmentPanel(charCombatSkills, orgTemplateId, idealSect, array, b);
		}
		short[] combatSkillAttainmentPanels = character.GetCombatSkillAttainmentPanels();
		if (CombatSkillAttainmentPanelsHelper.EqualAll(combatSkillAttainmentPanels, array))
		{
			_combatSkillAttainmentPanels = array;
			return null;
		}
		if (recordModification)
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.SetInitialCombatSkillAttainmentPanels);
			parallelModificationsRecorder.RecordParameterClass(character);
			parallelModificationsRecorder.RecordParameterClass(array);
		}
		_combatSkillAttainmentPanels = null;
		return array;
	}

	public static void ComplementSetInitialCombatSkillAttainmentPanels(DataContext context, Character character, short[] panels)
	{
		short[] combatSkillAttainmentPanels = character.GetCombatSkillAttainmentPanels();
		CombatSkillAttainmentPanelsHelper.CopyAll(panels, combatSkillAttainmentPanels);
		character.SetCombatSkillAttainmentPanels(combatSkillAttainmentPanels, context);
	}

	private unsafe void SetCombatSkillAttainmentPanel(Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills, sbyte selfOrgTemplateId, sbyte lovingOrgTemplateId, short[] panels, sbyte combatSkillType)
	{
		int i = 0;
		for (int count = _sectCandidateSkillInfos.Count; i < count; i++)
		{
			_sectCandidateSkillsPool.Return(_sectCandidateSkillInfos[i]);
		}
		_sectCandidateSkillInfos.Clear();
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkill in charCombatSkills)
		{
			charCombatSkill.Deconstruct(out var key, out var value);
			short index = key;
			GameData.Domains.CombatSkill.CombatSkill combatSkill = value;
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[index];
			if (combatSkillItem.Type == combatSkillType)
			{
				ushort activationState = combatSkill.GetActivationState();
				if (CombatSkillStateHelper.IsBrokenOut(activationState) && !combatSkill.GetRevoked())
				{
					SetCombatSkillAttainmentPanel_AddSectCandidateSkill(combatSkillItem);
				}
			}
		}
		SetCombatSkillAttainmentPanel_SortCandidateSects(selfOrgTemplateId, lovingOrgTemplateId);
		byte* intPtr = stackalloc byte[18];
		// IL initblk instruction
		Unsafe.InitBlock(intPtr, 255, 18);
		short* ptr = (short*)intPtr;
		int j = 0;
		for (int count2 = _sortedSectCandidateSkillInfos.Count; j < count2; j++)
		{
			short[] skillTemplateIds = _sortedSectCandidateSkillInfos[j].SkillTemplateIds;
			for (int k = 0; k < 9; k++)
			{
				if (ptr[k] < 0 && skillTemplateIds[k] >= 0)
				{
					ptr[k] = skillTemplateIds[k];
				}
			}
		}
		CombatSkillAttainmentPanelsHelper.SetPanel(panels, combatSkillType, ptr);
	}

	private void SetCombatSkillAttainmentPanel_AddSectCandidateSkill(CombatSkillItem config)
	{
		sbyte sectId = config.SectId;
		sbyte grade = config.Grade;
		int num = -1;
		int i = 0;
		for (int count = _sectCandidateSkillInfos.Count; i < count; i++)
		{
			if (_sectCandidateSkillInfos[i].OrgTemplateId == sectId)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			SectCandidateSkills sectCandidateSkills = _sectCandidateSkillInfos[num];
			sectCandidateSkills.Add(config.TemplateId, grade);
			return;
		}
		SectCandidateSkills sectCandidateSkills2 = _sectCandidateSkillsPool.Get();
		sectCandidateSkills2.Initialize(sectId);
		sectCandidateSkills2.Add(config.TemplateId, grade);
		_sectCandidateSkillInfos.Add(sectCandidateSkills2);
	}

	private void SetCombatSkillAttainmentPanel_SortCandidateSects(sbyte selfOrgTemplateId, sbyte lovingOrgTemplateId)
	{
		int num = int.MinValue;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int i = 0;
		for (int count = _sectCandidateSkillInfos.Count; i < count; i++)
		{
			SectCandidateSkills sectCandidateSkills = _sectCandidateSkillInfos[i];
			int num5 = (sectCandidateSkills.CombatSkillsCount << 8) + sectCandidateSkills.MaxGrade;
			if (num5 > num)
			{
				num = num5;
				num2 = i;
			}
			sbyte orgTemplateId = sectCandidateSkills.OrgTemplateId;
			if (orgTemplateId == selfOrgTemplateId)
			{
				num3 = i;
			}
			else if (orgTemplateId == lovingOrgTemplateId)
			{
				num4 = i;
			}
		}
		_sortedSectCandidateSkillInfos.Clear();
		if (num2 >= 0)
		{
			SectCandidateSkills sectCandidateSkills2 = _sectCandidateSkillInfos[num2];
			if (sectCandidateSkills2.CombatSkillsCount >= 3)
			{
				_sortedSectCandidateSkillInfos.Add(sectCandidateSkills2);
			}
			else
			{
				num2 = -1;
			}
		}
		if (num3 >= 0 && num3 != num2)
		{
			_sortedSectCandidateSkillInfos.Add(_sectCandidateSkillInfos[num3]);
		}
		if (num4 >= 0 && num4 != num2 && num4 != num3)
		{
			_sortedSectCandidateSkillInfos.Add(_sectCandidateSkillInfos[num4]);
		}
		int j = 0;
		for (int count2 = _sectCandidateSkillInfos.Count; j < count2; j++)
		{
			if (j != num2 && j != num3 && j != num4)
			{
				_sortedSectCandidateSkillInfos.Add(_sectCandidateSkillInfos[j]);
			}
		}
	}

	public (List<CombatSkillInitialBreakoutData> brokenOutSkills, List<(short skillTemplateId, int startIndex, SerializableList<SkillBreakPlateBonus> bonuses)> breakPlateBonuses, NeiliProportionOfFiveElements neiliProportion, int[] extraNeiliAllocationProgress) ParallelSetInitialCombatSkillBreakouts(DataContext context, Character character, bool recordModification = true)
	{
		_brokenOutCombatSkills.Clear();
		_brokenOutNeigongList.Clear();
		_modifiedBreakPlateBonuses.Clear();
		PerformInitialCombatSkillBreakouts(context, character);
		if (_brokenOutCombatSkills.Count <= 0)
		{
			return (brokenOutSkills: null, breakPlateBonuses: null, neiliProportion: default(NeiliProportionOfFiveElements), extraNeiliAllocationProgress: new int[4]);
		}
		(NeiliProportionOfFiveElements neiliProportionOfFiveElements, int[] extraNeiliAllocationProgress) tuple = PerformInitialNeigongLooping(context, character);
		NeiliProportionOfFiveElements item = tuple.neiliProportionOfFiveElements;
		int[] item2 = tuple.extraNeiliAllocationProgress;
		List<CombatSkillInitialBreakoutData> brokenOutCombatSkills = _brokenOutCombatSkills;
		List<(short, int, SerializableList<SkillBreakPlateBonus>)> modifiedBreakPlateBonuses = _modifiedBreakPlateBonuses;
		if (recordModification)
		{
			_brokenOutCombatSkills = new List<CombatSkillInitialBreakoutData>(32);
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.SetInitialCombatSkillBreakouts);
			parallelModificationsRecorder.RecordParameterClass(brokenOutCombatSkills);
			parallelModificationsRecorder.RecordParameterClass(character);
			parallelModificationsRecorder.RecordParameterUnmanaged(item);
			parallelModificationsRecorder.RecordParameterClass(item2);
			if (_modifiedBreakPlateBonuses.Count > 0)
			{
				_modifiedBreakPlateBonuses = new List<(short, int, SerializableList<SkillBreakPlateBonus>)>();
				parallelModificationsRecorder.RecordType(ParallelModificationType.UpdateBreakPlateBonuses);
				parallelModificationsRecorder.RecordParameterClass(new UpdateBreakPlateBonusesModification(character)
				{
					ModifiedBonuses = modifiedBreakPlateBonuses
				});
			}
		}
		return (brokenOutSkills: brokenOutCombatSkills, breakPlateBonuses: modifiedBreakPlateBonuses, neiliProportion: item, extraNeiliAllocationProgress: item2);
	}

	public static void ComplementSetInitialCombatSkillBreakouts(DataContext context, List<CombatSkillInitialBreakoutData> brokenOutSkills, Character character, NeiliProportionOfFiveElements neiliProportion, int[] extraNeiliAllocationProgress)
	{
		int i = 0;
		for (int count = brokenOutSkills.Count; i < count; i++)
		{
			CombatSkillInitialBreakoutData combatSkillInitialBreakoutData = brokenOutSkills[i];
			GameData.Domains.CombatSkill.CombatSkill combatSkill = combatSkillInitialBreakoutData.CombatSkill;
			combatSkill.SetActivationState(combatSkillInitialBreakoutData.ActivationState, context);
			combatSkill.SetForcedBreakoutStepsCount(combatSkillInitialBreakoutData.ForceBreakoutStepsCount, context);
			combatSkill.SetBreakoutStepsCount(combatSkillInitialBreakoutData.BreakoutStepsCount, context);
			if (combatSkillInitialBreakoutData.ObtainedNeili != 0)
			{
				combatSkill.SetObtainedNeili(combatSkillInitialBreakoutData.ObtainedNeili, context);
			}
		}
		if (character.Template.PresetNeiliProportionOfFiveElements.Sum() <= 0)
		{
			character.SetBaseNeiliProportionOfFiveElements(neiliProportion, context);
		}
		IntList extraNeiliAllocationProgress2 = IntList.Create();
		for (int j = 0; j < 4; j++)
		{
			extraNeiliAllocationProgress2.Items.Add(extraNeiliAllocationProgress[j]);
		}
		DomainManager.CombatSkill.SetCharacterExtraNeiliAllocationAndProgress(context, character, extraNeiliAllocationProgress2, canOverMax: true);
	}

	private void PerformInitialCombatSkillBreakouts(DataContext context, Character character)
	{
		int id = character.GetId();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(id);
		BreakoutCombatSkillContext context2 = new BreakoutCombatSkillContext(context.Random, character);
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item2 in charCombatSkills)
		{
			item2.Deconstruct(out var key, out var value);
			short index = key;
			GameData.Domains.CombatSkill.CombatSkill combatSkill = value;
			ushort activationState = combatSkill.GetActivationState();
			if (CombatSkillStateHelper.IsBrokenOut(activationState) || !combatSkill.CanBreakout())
			{
				continue;
			}
			var (activationState2, b, b2) = CalcCombatSkillBreakoutResult(ref context2, combatSkill);
			if (CombatSkillStateHelper.IsBrokenOut(activationState2))
			{
				int count = _brokenOutCombatSkills.Count;
				_brokenOutCombatSkills.Add(new CombatSkillInitialBreakoutData(combatSkill, activationState2, (sbyte)(b + b2), b2));
				CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[index];
				if (combatSkillItem.EquipType == 0)
				{
					_brokenOutNeigongList.Add((combatSkillItem, count));
				}
			}
		}
		BreakPlateBonusContext context3 = new BreakPlateBonusContext(context.Random, character);
		List<GameData.Domains.CombatSkill.CombatSkill>[] categorizedCombatSkillsByGrade = _categorizedCombatSkillsByGrade;
		foreach (List<GameData.Domains.CombatSkill.CombatSkill> list in categorizedCombatSkillsByGrade)
		{
			list.Clear();
		}
		foreach (CombatSkillInitialBreakoutData brokenOutCombatSkill in _brokenOutCombatSkills)
		{
			_categorizedCombatSkillsByGrade[brokenOutCombatSkill.CombatSkill.Template.Grade].Add(brokenOutCombatSkill.CombatSkill);
		}
		List<GameData.Domains.CombatSkill.CombatSkill>[] categorizedCombatSkillsByGrade2 = _categorizedCombatSkillsByGrade;
		foreach (List<GameData.Domains.CombatSkill.CombatSkill> list2 in categorizedCombatSkillsByGrade2)
		{
			CollectionUtils.Shuffle(context.Random, list2);
			int num = list2.Count * 80 / 100;
			for (int k = 0; k < num; k++)
			{
				int index2 = context.Random.Next(list2.Count);
				GameData.Domains.CombatSkill.CombatSkill combatSkill2 = list2[index2];
				SerializableList<SkillBreakPlateBonus> item = CreateInitialBreakPlateBonuses(ref context3, combatSkill2);
				CollectionUtils.SwapAndRemove(list2, index2);
				if (item.Items.Count > 0)
				{
					_modifiedBreakPlateBonuses.Add((combatSkill2.GetId().SkillTemplateId, 0, item));
				}
			}
		}
	}

	private SerializableList<SkillBreakPlateBonus> CreateInitialBreakPlateBonuses(ref BreakPlateBonusContext context, GameData.Domains.CombatSkill.CombatSkill combatSkill)
	{
		Character character = context.Character;
		int id = character.GetId();
		OrganizationItem skillBreakBonusOrganization = GetSkillBreakBonusOrganization(character);
		sbyte grade = character.GetOrganizationInfo().Grade;
		int num = CalcMaxSkillBreakBonusCount(character, combatSkill);
		SerializableList<SkillBreakPlateBonus> result = SerializableList<SkillBreakPlateBonus>.Create();
		CombatSkillItem template = combatSkill.Template;
		_skillBreakBonusWeights.Clear();
		_skillBreakBonusWeights.AddRange(skillBreakBonusOrganization.SkillBreakBonusWeights);
		if (_skillBreakBonusWeights.Count == 0)
		{
			foreach (SkillBreakBonusEffectItem item in (IEnumerable<SkillBreakBonusEffectItem>)SkillBreakBonusEffect.Instance)
			{
				_skillBreakBonusWeights.Add(new ShortPair(item.TemplateId, 1));
			}
		}
		while (result.Items.Count < num)
		{
			int randomSkillBreakBonusIndex = GetRandomSkillBreakBonusIndex(context.Random, _skillBreakBonusWeights);
			sbyte b = (sbyte)_skillBreakBonusWeights[randomSkillBreakBonusIndex].First;
			if (b < 0)
			{
				break;
			}
			if (!template.MatchBreakPlateBonusEffect(SkillBreakBonusEffect.Instance[b]))
			{
				CollectionUtils.SwapAndRemove(_skillBreakBonusWeights, randomSkillBreakBonusIndex);
				continue;
			}
			switch (b)
			{
			case 37:
			{
				int level = GradeToExpLevel(grade);
				result.Items.Add(SkillBreakPlateBonusHelper.CreateExp(level));
				continue;
			}
			case 33:
			case 34:
			{
				ushort relationType = (ushort)((b == 33) ? 16384 : 32768);
				int num2 = SelectRelatedCharForSkillBreakBonus(ref context, id, relationType);
				if (num2 >= 0)
				{
					result.Items.Add(SkillBreakPlateBonusHelper.CreateRelation(id, num2, relationType));
				}
				continue;
			}
			}
			TemplateKey randomItemGroupIdByEffect = ItemDomain.GetRandomItemGroupIdByEffect(context.Random, b);
			if (randomItemGroupIdByEffect.ItemType >= 0)
			{
				short templateIdInGroup = ItemTemplateHelper.GetTemplateIdInGroup(randomItemGroupIdByEffect.ItemType, randomItemGroupIdByEffect.TemplateId, grade);
				if (templateIdInGroup >= 0)
				{
					result.Items.Add(SkillBreakPlateBonusHelper.CreateItem(new ItemKey(randomItemGroupIdByEffect.ItemType, 0, templateIdInGroup, -1)));
				}
			}
		}
		return result;
	}

	private (NeiliProportionOfFiveElements neiliProportionOfFiveElements, int[] extraNeiliAllocationProgress) PerformInitialNeigongLooping(DataContext context, Character character)
	{
		IRandomSource random = context.Random;
		NeiliProportionOfFiveElements baseNeiliProportionOfFiveElements = character.GetBaseNeiliProportionOfFiveElements();
		int[] array = new int[4];
		CharacterItem template = character.Template;
		sbyte[] extraNeiliAllocationProgress = template.ExtraNeiliAllocationProgress;
		for (int i = 0; i < 4; i++)
		{
			int extraNeiliAllocationProgressByExtraNeiliAllocation = CombatSkillDomain.GetExtraNeiliAllocationProgressByExtraNeiliAllocation(extraNeiliAllocationProgress[i]);
			array[i] = extraNeiliAllocationProgressByExtraNeiliAllocation;
		}
		int neiliAllocationMaxProgress = CombatSkillDomain.GetNeiliAllocationMaxProgress();
		if (_brokenOutNeigongList.Count <= 0)
		{
			return (neiliProportionOfFiveElements: baseNeiliProportionOfFiveElements, extraNeiliAllocationProgress: array);
		}
		int totalLoopsCount = GenerateInitialNeigongLoopsCount(character);
		if (totalLoopsCount <= 0)
		{
			return (neiliProportionOfFiveElements: baseNeiliProportionOfFiveElements, extraNeiliAllocationProgress: array);
		}
		_brokenOutNeigongList.Sort(Comparer);
		bool flag = CreatingType.IsNonEvolutionaryType(character.GetCreatingType());
		int j = 0;
		for (int count = _brokenOutNeigongList.Count; j < count; j++)
		{
			if (totalLoopsCount <= 0)
			{
				break;
			}
			(CombatSkillItem skillCfg, int index) tuple = _brokenOutNeigongList[j];
			CombatSkillItem item = tuple.skillCfg;
			int item2 = tuple.index;
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), item.TemplateId));
			sbyte fiveElementsChange = element_CombatSkills.GetFiveElementsChange();
			(short ObtainedNeili, int loopCount, int[] extraNeiliAllocationProgress) tuple2 = GenerateInitialNeili(random, character, item, ref totalLoopsCount);
			short item3 = tuple2.ObtainedNeili;
			int item4 = tuple2.loopCount;
			int[] item5 = tuple2.extraNeiliAllocationProgress;
			CombatSkillInitialBreakoutData value = _brokenOutCombatSkills[item2];
			value.ObtainedNeili = item3;
			for (int k = 0; k < 4; k++)
			{
				if (!flag && array[k] < neiliAllocationMaxProgress)
				{
					array[k] = item5[k];
				}
			}
			_brokenOutCombatSkills[item2] = value;
			if (fiveElementsChange > 0 && item.TransferTypeWhileLooping >= 0)
			{
				baseNeiliProportionOfFiveElements.Transfer(item.DestTypeWhileLooping, item.TransferTypeWhileLooping, fiveElementsChange * item4);
			}
		}
		if (totalLoopsCount > 0)
		{
			CombatSkillItem combatSkillItem = SelectCombatSkillForAdjustingNeiliType(character, _brokenOutNeigongList);
			if (combatSkillItem != null)
			{
				GameData.Domains.CombatSkill.CombatSkill element_CombatSkills2 = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), combatSkillItem.TemplateId));
				sbyte fiveElementsChange2 = element_CombatSkills2.GetFiveElementsChange();
				if (fiveElementsChange2 > 0 && combatSkillItem.TransferTypeWhileLooping >= 0)
				{
					baseNeiliProportionOfFiveElements.Transfer(combatSkillItem.DestTypeWhileLooping, combatSkillItem.TransferTypeWhileLooping, fiveElementsChange2 * totalLoopsCount);
				}
			}
			List<(CombatSkillItem skillCfg, int index)> brokenOutNeigongList = _brokenOutNeigongList;
			if (brokenOutNeigongList != null && brokenOutNeigongList.Count > 0)
			{
				CombatSkillItem item6 = _brokenOutNeigongList[0].skillCfg;
				for (int l = 0; l < totalLoopsCount; l++)
				{
					int[] item7 = GenerateInitialNeili(random, character, item6, ref totalLoopsCount).extraNeiliAllocationProgress;
					for (int m = 0; m < 4; m++)
					{
						if (!flag && array[m] < neiliAllocationMaxProgress)
						{
							array[m] += item7[m];
						}
					}
				}
			}
		}
		return (neiliProportionOfFiveElements: baseNeiliProportionOfFiveElements, extraNeiliAllocationProgress: array);
	}

	private static (ushort activationState, sbyte availableStepsCount, sbyte forcedStepsCount) CalcCombatSkillBreakoutResult(ref BreakoutCombatSkillContext context, GameData.Domains.CombatSkill.CombatSkill skill)
	{
		short skillTemplateId = skill.GetId().SkillTemplateId;
		ushort readingState = skill.GetReadingState();
		IRandomSource random = context.Random;
		ushort activationState = CombatSkillStateHelper.GenerateRandomActivatedNormalPages(random, readingState, 0);
		sbyte skillBreakoutAvailableStepsCount = context.Character.GetSkillBreakoutAvailableStepsCount(skillTemplateId);
		int num = 20 - skillBreakoutAvailableStepsCount;
		if (num < 0)
		{
			num = 0;
		}
		int percentProb = (context.IsCreatedWithFixedTemplate ? 100 : CombatSkillHelper.CalcBreakoutSuccessRate(skillTemplateId, ref context.Qualifications));
		if (random.CheckPercentProb(percentProb))
		{
			activationState = CombatSkillStateHelper.GenerateRandomActivatedOutlinePage(random, readingState, activationState, context.BehaviorType);
			return (activationState: activationState, availableStepsCount: skillBreakoutAvailableStepsCount, forcedStepsCount: (sbyte)num);
		}
		return (activationState: 0, availableStepsCount: skillBreakoutAvailableStepsCount, forcedStepsCount: (sbyte)num);
	}

	private unsafe static int GenerateInitialNeigongLoopsCount(Character character)
	{
		int num = (character.GetActualAge() - 10) * 12;
		int num2 = 60 + character.GetCombatSkillQualifications().Items[0];
		return num * num2 / 100;
	}

	private static (short ObtainedNeili, int loopCount, int[] extraNeiliAllocationProgress) GenerateInitialNeili(IRandomSource random, Character character, CombatSkillItem skillCfg, ref int totalLoopsCount)
	{
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillCfg.TemplateId));
		short totalObtainableNeili = element_CombatSkills.GetTotalObtainableNeili();
		int num = 0;
		int num2 = 0;
		int[] array = new int[4];
		while ((num < totalObtainableNeili) & (totalLoopsCount > 0))
		{
			(short neili, short qiDisorder, int[] extraNeiliAllocationProgress) tuple = CombatSkillDomain.CalcNeigongLoopingEffect(random, character, skillCfg);
			short item = tuple.neili;
			int[] item2 = tuple.extraNeiliAllocationProgress;
			num += item;
			for (int i = 0; i < 4; i++)
			{
				array[i] += item2[i];
			}
			num2++;
			totalLoopsCount--;
		}
		if (num > totalObtainableNeili)
		{
			num = totalObtainableNeili;
		}
		return (ObtainedNeili: (short)num, loopCount: num2, extraNeiliAllocationProgress: array);
	}

	public void ParallelPracticeAndBreakoutCombatSkills(DataContext context, Character character)
	{
		_canUpdateCombatSkills.Clear();
		_brokenOutCombatSkills.Clear();
		_failedToBreakoutCombatSkills.Clear();
		_newPersonalNeeds.Clear();
		_askingForHelpSkills.Clear();
		_newlyActivatedCombatSkills.Clear();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(character.GetId());
		Personalities personalities = character.GetPersonalities();
		sbyte neiliType = character.GetNeiliType();
		sbyte orgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
		sbyte idealSect = character.GetIdealSect();
		foreach (PersonalNeed personalNeed in character.GetPersonalNeeds())
		{
			if (personalNeed.TemplateId == 18)
			{
				_askingForHelpSkills.Add(personalNeed.CombatSkillTemplateId);
			}
		}
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item3 in charCombatSkills)
		{
			item3.Deconstruct(out var key, out var value);
			short num = key;
			GameData.Domains.CombatSkill.CombatSkill combatSkill = value;
			ushort activationState = combatSkill.GetActivationState();
			if (CombatSkillStateHelper.IsBrokenOut(activationState))
			{
				ushort readingState = combatSkill.GetReadingState();
				ushort num2 = CombatSkillStateHelper.GenerateRandomActivatedNormalPages(context.Random, readingState, activationState);
				if (num2 != activationState)
				{
					_newlyActivatedCombatSkills.Add((combatSkill, num2));
				}
			}
			else if (combatSkill.CanBreakout() && !_askingForHelpSkills.Contains(num))
			{
				int item = CalcCombatSkillPracticeOrBreakoutScore(num, combatSkill, orgTemplateId, idealSect, ref personalities, neiliType);
				_canUpdateCombatSkills.Add((combatSkill, item));
			}
		}
		if (_newlyActivatedCombatSkills.Count >= 0)
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.ActivateCombatSkillPages);
			parallelModificationsRecorder.RecordParameterClass(_newlyActivatedCombatSkills);
			_newlyActivatedCombatSkills = new List<(GameData.Domains.CombatSkill.CombatSkill, ushort)>();
		}
		if (_canUpdateCombatSkills.Count == 0)
		{
			return;
		}
		_canUpdateCombatSkills.Sort(CompareScore);
		BreakoutCombatSkillContext context2 = new BreakoutCombatSkillContext(context.Random, character);
		for (int num3 = _canUpdateCombatSkills.Count - 1; num3 >= 0; num3--)
		{
			GameData.Domains.CombatSkill.CombatSkill item2 = _canUpdateCombatSkills[num3].combatSkill;
			if (OfflineBreakoutCombatSkill(ref context2, item2))
			{
				break;
			}
		}
		PracticeAndBreakoutModification practiceAndBreakoutModification = new PracticeAndBreakoutModification(character);
		if (_brokenOutCombatSkills.Count != 0)
		{
			practiceAndBreakoutModification.BrokenOutCombatSkills = _brokenOutCombatSkills;
			_brokenOutCombatSkills = new List<CombatSkillInitialBreakoutData>(32);
		}
		if (_failedToBreakoutCombatSkills.Count != 0)
		{
			practiceAndBreakoutModification.FailedToBreakoutCombatSkills = _failedToBreakoutCombatSkills;
			_failedToBreakoutCombatSkills = new List<GameData.Domains.CombatSkill.CombatSkill>(8);
		}
		if (_newPersonalNeeds.Count != 0)
		{
			foreach (PersonalNeed newPersonalNeed in _newPersonalNeeds)
			{
				character.OfflineAddPersonalNeed(newPersonalNeed);
			}
			practiceAndBreakoutModification.PersonalNeedsChanged = true;
		}
		practiceAndBreakoutModification.NewExp = context2.CharExp;
		practiceAndBreakoutModification.NewInjuries = context2.Injuries;
		practiceAndBreakoutModification.NewDisorderOfQi = context2.DisorderOfQi;
		ParallelModificationsRecorder parallelModificationsRecorder2 = context.ParallelModificationsRecorder;
		parallelModificationsRecorder2.RecordType(ParallelModificationType.PracticeAndBreakoutCombatSkills);
		parallelModificationsRecorder2.RecordParameterClass(practiceAndBreakoutModification);
	}

	public static void ComplementActivateCombatSkillPages(DataContext context, List<(GameData.Domains.CombatSkill.CombatSkill skill, ushort activationStates)> newlyActivatedCombatSkills)
	{
		foreach (var newlyActivatedCombatSkill in newlyActivatedCombatSkills)
		{
			newlyActivatedCombatSkill.skill.SetActivationState(newlyActivatedCombatSkill.activationStates, context);
		}
	}

	public static void ComplementPracticeAndBreakoutCombatSkill(DataContext context, PracticeAndBreakoutModification mod)
	{
		Character character = mod.Character;
		character.SetExp(Math.Max(mod.NewExp, 0), context);
		character.SetInjuries(mod.NewInjuries, context);
		character.SetDisorderOfQi(Math.Clamp(mod.NewDisorderOfQi, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue), context);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = character.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = character.GetLocation();
		if (mod.BrokenOutCombatSkills != null)
		{
			bool flag = false;
			foreach (CombatSkillInitialBreakoutData brokenOutCombatSkill in mod.BrokenOutCombatSkills)
			{
				GameData.Domains.CombatSkill.CombatSkill combatSkill = brokenOutCombatSkill.CombatSkill;
				combatSkill.SetActivationState(brokenOutCombatSkill.ActivationState, context);
				combatSkill.SetForcedBreakoutStepsCount(brokenOutCombatSkill.ForceBreakoutStepsCount, context);
				combatSkill.SetBreakoutStepsCount(brokenOutCombatSkill.BreakoutStepsCount, context);
				if (brokenOutCombatSkill.ObtainedNeili != 0)
				{
					combatSkill.SetObtainedNeili(brokenOutCombatSkill.ObtainedNeili, context);
				}
				CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkill.GetId().SkillTemplateId];
				if (character.IsCombatSkillEquipped(combatSkillItem.TemplateId))
				{
					flag = true;
				}
				short bookId = combatSkillItem.BookId;
				if (bookId >= 0)
				{
					character.ChangeHappiness(context, ItemTemplateHelper.GetBaseHappinessChange(10, bookId));
					continue;
				}
				AdaptableLog.Warning($"Character {id} is breaking out combat skill {combatSkillItem.Name}");
			}
			if (flag)
			{
				DomainManager.SpecialEffect.UpdateEquippedSkillEffect(context, character);
			}
		}
		if (mod.FailedToBreakoutCombatSkills != null)
		{
			foreach (GameData.Domains.CombatSkill.CombatSkill failedToBreakoutCombatSkill in mod.FailedToBreakoutCombatSkills)
			{
				short skillTemplateId = failedToBreakoutCombatSkill.GetId().SkillTemplateId;
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int dataOffset = secretInformationCollection.AddBreakoutFail(id, skillTemplateId);
				DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			}
		}
		if (mod.PersonalNeedsChanged)
		{
			character.SetPersonalNeeds(character.GetPersonalNeeds(), context);
		}
	}

	private int CompareScore((GameData.Domains.CombatSkill.CombatSkill combatSkill, int score) x, (GameData.Domains.CombatSkill.CombatSkill combatSkill, int score) y)
	{
		return x.score - y.score;
	}

	private bool OfflineBreakoutCombatSkill(ref BreakoutCombatSkillContext context, GameData.Domains.CombatSkill.CombatSkill combatSkill)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkill.GetId().SkillTemplateId];
		int num = Config.SkillBreakPlate.Instance[combatSkillItem.Grade].CostExp * 10;
		if (context.ExpPerMonth + context.CharExp >= num)
		{
			context.ExpPerMonth -= num;
			if (context.ExpPerMonth < 0)
			{
				context.CharExp += context.ExpPerMonth;
				context.ExpPerMonth = 0;
			}
			var (num2, b, b2) = CalcCombatSkillBreakoutResult(ref context, combatSkill);
			if (!context.IsCreatedWithFixedTemplate && DomainManager.SpecialEffect.ModifyData(context.Character.GetId(), -1, 267, dataValue: false))
			{
				int num3 = CombatSkillHelper.CalcBreakoutSuccessRate(combatSkill.GetId().SkillTemplateId, ref context.Qualifications);
				int num4 = (130 - num3) / 10;
				for (int i = 0; i < num4; i++)
				{
					CombatSkillHelper.CalcForceBreakoutInjuriesAndDisorderOfQi(context.Random, combatSkillItem, ref context.Injuries, ref context.DisorderOfQi);
				}
			}
			if (num2 == 0 && b < 10)
			{
				PersonalNeed item = PersonalNeed.CreatePersonalNeed(18, combatSkillItem.TemplateId);
				_newPersonalNeeds.Add(item);
				return false;
			}
			if (CombatSkillStateHelper.IsBrokenOut(num2))
			{
				sbyte breakoutStepsCount = (sbyte)(b + b2);
				_brokenOutCombatSkills.Add(new CombatSkillInitialBreakoutData(combatSkill, num2, breakoutStepsCount, b2));
				return false;
			}
			PersonalNeed item2 = PersonalNeed.CreatePersonalNeed(18, combatSkillItem.TemplateId);
			_newPersonalNeeds.Add(item2);
			_failedToBreakoutCombatSkills.Add(combatSkill);
			CombatSkillHelper.CalcForceBreakoutInjuriesAndDisorderOfQi(context.Random, combatSkillItem, ref context.Injuries, ref context.DisorderOfQi);
			return true;
		}
		PersonalNeed item3 = PersonalNeed.CreatePersonalNeed(16, num);
		_newPersonalNeeds.Add(item3);
		return true;
	}

	public void ParallelUpdateBreakPlateBonuses(DataContext context, Character character)
	{
		_canUpdateCombatSkills.Clear();
		_newPersonalNeeds.Clear();
		_modifiedBreakPlateBonuses.Clear();
		_consumedItems.Clear();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(character.GetId());
		Personalities personalities = character.GetPersonalities();
		sbyte neiliType = character.GetNeiliType();
		sbyte orgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
		sbyte idealSect = character.GetIdealSect();
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item2 in charCombatSkills)
		{
			item2.Deconstruct(out var key, out var value);
			short skillTemplateId = key;
			GameData.Domains.CombatSkill.CombatSkill combatSkill = value;
			ushort activationState = combatSkill.GetActivationState();
			if (!combatSkill.GetRevoked() && CombatSkillStateHelper.IsBrokenOut(activationState))
			{
				int item = CalcCombatSkillPracticeOrBreakoutScore(skillTemplateId, combatSkill, orgTemplateId, idealSect, ref personalities, neiliType);
				_canUpdateCombatSkills.Add((combatSkill, item));
			}
		}
		int num = 0;
		if (_canUpdateCombatSkills.Count > 0)
		{
			BreakPlateBonusContext context2 = new BreakPlateBonusContext(context.Random, character);
			_canUpdateCombatSkills.Sort(CompareScore);
			for (int num2 = _canUpdateCombatSkills.Count - 1; num2 >= 0; num2--)
			{
				num += OfflineUpdateBonuses(ref context2, _canUpdateCombatSkills[num2].combatSkill);
			}
		}
		if (_modifiedBreakPlateBonuses.Count <= 0 && _newPersonalNeeds.Count <= 0)
		{
			return;
		}
		UpdateBreakPlateBonusesModification updateBreakPlateBonusesModification = new UpdateBreakPlateBonusesModification(character);
		ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
		parallelModificationsRecorder.RecordType(ParallelModificationType.UpdateBreakPlateBonuses);
		parallelModificationsRecorder.RecordParameterClass(updateBreakPlateBonusesModification);
		if (_modifiedBreakPlateBonuses.Count > 0)
		{
			updateBreakPlateBonusesModification.ModifiedBonuses = _modifiedBreakPlateBonuses;
			_modifiedBreakPlateBonuses = new List<(short, int, SerializableList<SkillBreakPlateBonus>)>();
		}
		updateBreakPlateBonusesModification.ExpCost = num;
		if (_consumedItems.Count > 0)
		{
			updateBreakPlateBonusesModification.ToDeleteItems = _consumedItems;
			_consumedItems = new List<ItemKey>();
		}
		if (_newPersonalNeeds.Count <= 0)
		{
			return;
		}
		foreach (PersonalNeed newPersonalNeed in _newPersonalNeeds)
		{
			character.OfflineAddPersonalNeed(newPersonalNeed);
		}
		updateBreakPlateBonusesModification.PersonalNeedsUpdated = true;
	}

	public static void ComplementUpdateBreakPlateBonuses(DataContext context, UpdateBreakPlateBonusesModification mod)
	{
		Character character = mod.Character;
		int id = character.GetId();
		if (mod.ModifiedBonuses != null)
		{
			ApplyBreakPlateBonuses(context, id, mod.ModifiedBonuses);
			AddBreakPlateBonusLifeRecords(id, mod.ModifiedBonuses);
		}
		if (mod.PersonalNeedsUpdated)
		{
			character.SetPersonalNeeds(character.GetPersonalNeeds(), context);
		}
		List<ItemKey> toDeleteItems = mod.ToDeleteItems;
		if (toDeleteItems != null && toDeleteItems.Count > 0)
		{
			foreach (ItemKey toDeleteItem in mod.ToDeleteItems)
			{
				Events.RaiseItemRemovedFromInventory(context, character, toDeleteItem, 1);
				DomainManager.Item.RemoveItem(context, toDeleteItem);
			}
			character.SetInventory(character.GetInventory(), context);
		}
		if (mod.ExpCost > 0)
		{
			character.ChangeExp(context, -mod.ExpCost);
		}
	}

	private static void ApplyBreakPlateBonuses(DataContext context, int charId, List<(short skillTemplateId, int startIndex, SerializableList<SkillBreakPlateBonus> bonuses)> modifiedBonuses)
	{
		foreach (var modifiedBonuse in modifiedBonuses)
		{
			DomainManager.Extra.SetCharacterSkillBreakBonuses(context, charId, modifiedBonuse.skillTemplateId, modifiedBonuse.bonuses);
		}
	}

	private static void AddBreakPlateBonusLifeRecords(int charId, List<(short skillTemplateId, int startIndex, SerializableList<SkillBreakPlateBonus> bonuses)> modifiedBonuses)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		foreach (var modifiedBonuse in modifiedBonuses)
		{
			for (int i = modifiedBonuse.startIndex; i < modifiedBonuse.bonuses.Items.Count; i++)
			{
				SkillBreakPlateBonus skillBreakPlateBonus = modifiedBonuse.bonuses.Items[i];
				switch (skillBreakPlateBonus.Type)
				{
				case ESkillBreakPlateBonusType.Item:
					lifeRecordCollection.AddCombatSkillKeyPointComprehensionByItems(charId, currDate, modifiedBonuse.skillTemplateId, skillBreakPlateBonus.ItemType, skillBreakPlateBonus.ItemTemplateId);
					break;
				case ESkillBreakPlateBonusType.Relation:
					if (skillBreakPlateBonus.RelationType == 16384)
					{
						lifeRecordCollection.AddCombatSkillKeyPointComprehensionByLoveRelationship(charId, currDate, modifiedBonuse.skillTemplateId, skillBreakPlateBonus.RelationRelatedCharId);
					}
					else
					{
						lifeRecordCollection.AddCombatSkillKeyPointComprehensionByHatredRelationship(charId, currDate, modifiedBonuse.skillTemplateId, skillBreakPlateBonus.RelationRelatedCharId);
					}
					break;
				case ESkillBreakPlateBonusType.Exp:
					lifeRecordCollection.AddCombatSkillKeyPointComprehensionByExp(charId, currDate, modifiedBonuse.skillTemplateId);
					break;
				}
			}
		}
	}

	private int OfflineUpdateBonuses(ref BreakPlateBonusContext context, GameData.Domains.CombatSkill.CombatSkill combatSkill)
	{
		Character character = context.Character;
		short skillTemplateId = combatSkill.GetId().SkillTemplateId;
		OrganizationItem skillBreakBonusOrganization = GetSkillBreakBonusOrganization(character);
		int num = CalcMaxSkillBreakBonusCount(character, combatSkill);
		SerializableList<SkillBreakPlateBonus> characterSkillBreakBonuses = DomainManager.Extra.GetCharacterSkillBreakBonuses(character.GetId(), skillTemplateId);
		int num2 = characterSkillBreakBonuses.Items?.Count ?? 0;
		if (num2 >= num)
		{
			return 0;
		}
		bool flag = false;
		int num3 = 0;
		for (int i = num2; i < num; i++)
		{
			SkillBreakPlateBonus item = SelectSkillBreakBonus(ref context, combatSkill.Template, skillBreakBonusOrganization);
			if (item.Type == ESkillBreakPlateBonusType.None)
			{
				int randomSkillBreakBonusIndex = GetRandomSkillBreakBonusIndex(context.Random, skillBreakBonusOrganization.SkillBreakBonusWeights);
				if (randomSkillBreakBonusIndex >= 0)
				{
					sbyte bonusEffectId = (sbyte)skillBreakBonusOrganization.SkillBreakBonusWeights[randomSkillBreakBonusIndex].First;
					PersonalNeed item2 = CreateSkillBreakPlateBonusNeed(context.Random, character, bonusEffectId);
					_newPersonalNeeds.Add(item2);
					break;
				}
				continue;
			}
			if (item.Type == ESkillBreakPlateBonusType.Exp)
			{
				num3 += SkillBreakPlateConstants.ExpLevelValues[item.ExpLevel];
			}
			ref List<SkillBreakPlateBonus> items = ref characterSkillBreakBonuses.Items;
			if (items == null)
			{
				items = new List<SkillBreakPlateBonus>();
			}
			characterSkillBreakBonuses.Items.Add(item);
			flag = true;
		}
		if (flag)
		{
			_modifiedBreakPlateBonuses.Add((skillTemplateId, num2, characterSkillBreakBonuses));
		}
		return num3;
	}

	private int CalcMaxSkillBreakBonusCount(Character character, GameData.Domains.CombatSkill.CombatSkill combatSkill)
	{
		int bonusCount = combatSkill.Template.SkillBreakPlate.BonusCount;
		int skillBreakoutStepsPercentage = character.GetSkillBreakoutStepsPercentage(combatSkill.GetId().SkillTemplateId);
		return Math.Min(bonusCount * skillBreakoutStepsPercentage / 100, bonusCount);
	}

	private OrganizationItem GetSkillBreakBonusOrganization(Character character)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		sbyte orgTemplateId = organizationInfo.OrgTemplateId;
		OrganizationItem organizationItem = Config.Organization.Instance[orgTemplateId];
		if (organizationItem.IsSect)
		{
			return organizationItem;
		}
		orgTemplateId = character.GetIdealSect();
		if (orgTemplateId >= 0)
		{
			return Config.Organization.Instance[orgTemplateId];
		}
		if (organizationInfo.SettlementId >= 0)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(organizationInfo.SettlementId);
			Location location = settlement.GetLocation();
			if (location.IsValid())
			{
				return organizationItem;
			}
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
			orgTemplateId = MapState.Instance[stateTemplateIdByAreaId].SectID;
			return Config.Organization.Instance[orgTemplateId];
		}
		return organizationItem;
	}

	private SkillBreakPlateBonus SelectSkillBreakBonus(ref BreakPlateBonusContext context, CombatSkillItem skillCfg, OrganizationItem organizationCfg)
	{
		ShortPair[] skillBreakBonusWeights = organizationCfg.SkillBreakBonusWeights;
		for (int i = 0; i < skillBreakBonusWeights.Length; i++)
		{
			ShortPair shortPair = skillBreakBonusWeights[i];
			sbyte b = (sbyte)shortPair.First;
			SkillBreakBonusEffectItem effect = SkillBreakBonusEffect.Instance[b];
			if (skillCfg.MatchBreakPlateBonusEffect(effect))
			{
				SkillBreakPlateBonus result = CreateSkillBreakBonus(ref context, b);
				if (result.Type != ESkillBreakPlateBonusType.None)
				{
					return result;
				}
			}
		}
		return SkillBreakPlateBonus.Invalid;
	}

	private static int GetRandomSkillBreakBonusIndex(IRandomSource random, IReadOnlyList<ShortPair> bonusWeights)
	{
		if (bonusWeights.Count == 0)
		{
			return -1;
		}
		int num = 0;
		foreach (ShortPair bonusWeight in bonusWeights)
		{
			num += bonusWeight.Second;
		}
		int num2 = random.Next(0, num);
		for (int i = 0; i < bonusWeights.Count; i++)
		{
			num2 -= bonusWeights[i].Second;
			if (num2 < 0)
			{
				return i;
			}
		}
		throw new ArgumentException("Unable to get random from weight table.", "bonusWeights");
	}

	private PersonalNeed CreateSkillBreakPlateBonusNeed(IRandomSource random, Character character, sbyte bonusEffectId)
	{
		sbyte interactionGrade = character.GetInteractionGrade();
		sbyte b = ItemDomain.GenerateRandomItemGrade(random, interactionGrade);
		switch (bonusEffectId)
		{
		case 37:
			return PersonalNeed.CreatePersonalNeed(16, SkillBreakPlateConstants.ExpLevelValues[GradeToExpLevel(b)]);
		case 33:
			return PersonalNeed.CreatePersonalNeed((sbyte)25, (ushort)16384);
		case 34:
			return PersonalNeed.CreatePersonalNeed((sbyte)25, (ushort)32768);
		default:
		{
			TemplateKey randomItemGroupIdByEffect = ItemDomain.GetRandomItemGroupIdByEffect(random, bonusEffectId);
			if (randomItemGroupIdByEffect.ItemType >= 0)
			{
				short templateIdInGroup = ItemTemplateHelper.GetTemplateIdInGroup(randomItemGroupIdByEffect.ItemType, randomItemGroupIdByEffect.TemplateId, b);
				return PersonalNeed.CreatePersonalNeed(10, randomItemGroupIdByEffect.ItemType, templateIdInGroup);
			}
			throw new ArgumentException($"Unable to create personal need with bonus effect {bonusEffectId}");
		}
		}
	}

	private int GradeToExpLevel(sbyte grade)
	{
		return Math.Clamp(SkillBreakPlateConstants.ExpLevelValues.Count - 1 - 8 + grade, 0, SkillBreakPlateConstants.ExpLevelValues.Count - 1);
	}

	private SkillBreakPlateBonus CreateSkillBreakBonus(ref BreakPlateBonusContext context, sbyte bonusEffectId)
	{
		Character character = context.Character;
		switch (bonusEffectId)
		{
		case 37:
		{
			sbyte interactionGrade = character.GetInteractionGrade();
			return SkillBreakPlateBonusHelper.CreateExp(GradeToExpLevel(interactionGrade));
		}
		case 33:
		case 34:
		{
			int id = character.GetId();
			ushort relationType = (ushort)((bonusEffectId == 33) ? 16384 : 32768);
			int num2 = SelectRelatedCharForSkillBreakBonus(ref context, id, relationType);
			if (num2 < 0)
			{
				break;
			}
			return SkillBreakPlateBonusHelper.CreateRelation(id, num2, relationType);
		}
		default:
		{
			Inventory inventory = character.GetInventory();
			ItemKey itemKey = ItemKey.Invalid;
			int num = -1;
			foreach (KeyValuePair<ItemKey, int> item in inventory.Items)
			{
				item.Deconstruct(out var key, out var _);
				ItemKey itemKey2 = key;
				sbyte breakBonusEffect = ItemTemplateHelper.GetBreakBonusEffect(itemKey2.ItemType, itemKey2.TemplateId);
				if (breakBonusEffect == bonusEffectId)
				{
					sbyte grade = ItemTemplateHelper.GetGrade(itemKey2.ItemType, itemKey2.TemplateId);
					if (grade > num)
					{
						num = grade;
						itemKey = itemKey2;
					}
				}
			}
			if (itemKey.IsValid())
			{
				inventory.OfflineRemove(itemKey, 1);
				_consumedItems.Add(itemKey);
				return SkillBreakPlateBonusHelper.CreateItem(itemKey);
			}
			break;
		}
		}
		return SkillBreakPlateBonus.Invalid;
	}

	private int SelectRelatedCharForSkillBreakBonus(ref BreakPlateBonusContext context, int charId, ushort relationType)
	{
		HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(charId, relationType);
		if (relatedCharIds.Count == 0)
		{
			return -1;
		}
		short num = 0;
		int num2 = -1;
		if (context.UsedRelatedCharIds == null)
		{
			InitializeUsedRelatedCharactersForBreakBonus(ref context);
		}
		foreach (int item in relatedCharIds)
		{
			if (!DomainManager.Character.IsCharacterAlive(item) || !DomainManager.Character.HasRelation(item, charId, relationType) || _usedRelatedCharIds.Contains(item))
			{
				continue;
			}
			short favorability = DomainManager.Character.GetFavorability(charId, item);
			if (num2 < 0)
			{
				num2 = item;
				num = favorability;
			}
			else if (relationType == 32768)
			{
				if (favorability < num)
				{
					num = favorability;
					num2 = relationType;
				}
			}
			else if (favorability > num)
			{
				num = favorability;
				num2 = relationType;
			}
		}
		if (num2 >= 0)
		{
			context.UsedRelatedCharIds.Add(num2);
		}
		return num2;
	}

	private void InitializeUsedRelatedCharactersForBreakBonus(ref BreakPlateBonusContext context)
	{
		int id = context.Character.GetId();
		_usedRelatedCharIds.Clear();
		foreach (var (skillTemplateId, combatSkill2) in context.CharCombatSkills)
		{
			if (!CombatSkillStateHelper.IsBrokenOut(combatSkill2.GetActivationState()))
			{
				continue;
			}
			SerializableList<SkillBreakPlateBonus> characterSkillBreakBonuses = DomainManager.Extra.GetCharacterSkillBreakBonuses(id, skillTemplateId);
			List<SkillBreakPlateBonus> items = characterSkillBreakBonuses.Items;
			if (items == null || items.Count <= 0)
			{
				continue;
			}
			for (int i = 0; i < characterSkillBreakBonuses.Items.Count; i++)
			{
				SkillBreakPlateBonus skillBreakPlateBonus = characterSkillBreakBonuses.Items[i];
				if (skillBreakPlateBonus.Type == ESkillBreakPlateBonusType.Relation)
				{
					_usedRelatedCharIds.Add(skillBreakPlateBonus.RelationRelatedCharId);
				}
			}
		}
		context.UsedRelatedCharIds = _usedRelatedCharIds;
	}

	private unsafe static int CalcCombatSkillPracticeOrBreakoutScore(short skillTemplateId, GameData.Domains.CombatSkill.CombatSkill skill, short selfOrgTemplateId, short targetOrgTemplateId, ref Personalities personalities, sbyte neiliType)
	{
		int num = 0;
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
		if (combatSkillItem.SectId == selfOrgTemplateId)
		{
			num += 150 + personalities.Items[0];
		}
		if (combatSkillItem.SectId == targetOrgTemplateId)
		{
			num += 75 + personalities.Items[2];
		}
		if (!CheckCounterWithNeiliType(combatSkillItem.FiveElements, neiliType))
		{
			num += 150 + personalities.Items[0];
		}
		return num + 50 * (8 - combatSkillItem.Grade);
	}

	public void EquipItems(DataContext context, Character character)
	{
		SelectEquipmentsModification selectEquipmentsModification = new SelectEquipmentsModification(character, removeUnequippedEquipment: false);
		EquipItems(character, selectEquipmentsModification);
		if (selectEquipmentsModification.EquippedItems != null)
		{
			AdaptableLog.Info($"{character} changed equipped items.");
			character.ChangeEquipment(context, selectEquipmentsModification.EquippedItems);
		}
	}

	private void EquipItems(Character character, SelectEquipmentsModification mod)
	{
		ClassifyAvailableItems(character);
		SelectWeapons(character, mod);
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(character.GetOrganizationInfo());
		PresetEquipmentItemWithProb[] equipment = orgMemberConfig.Equipment;
		_equippedItems[3] = SelectArmor(mod, _availableHelms, equipment[0]);
		_equippedItems[5] = SelectArmor(mod, _availableTorsos, equipment[1]);
		_equippedItems[6] = SelectArmor(mod, _availableBracers, equipment[2]);
		_equippedItems[7] = SelectArmor(mod, _availableBoots, equipment[3]);
		_equippedItems[8] = SelectAccessory(mod, _availableAccessories, equipment[4]);
		_equippedItems[9] = SelectAccessory(mod, _availableAccessories, equipment[5]);
		_equippedItems[10] = SelectAccessory(mod, _availableAccessories, equipment[6]);
		short idealClothingTemplateId = character.GetIdealClothingTemplateId();
		_equippedItems[4] = SelectClothing(character, mod, idealClothingTemplateId);
		_equippedItems[11] = SelectCarrier(mod, _availableCarriers, equipment[7]);
		if (!CollectionUtils.Equals(character.GetEquipment(), _equippedItems, 12))
		{
			mod.EquippedItems = _equippedItems;
			_equippedItems = new ItemKey[12];
		}
	}

	public ItemKey SelectClothing(DataContext context, Character character)
	{
		_availableClothing.Clear();
		ItemKey[] equipment = character.GetEquipment();
		sbyte gender = character.GetGender();
		if (equipment[4].IsValid())
		{
			EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(equipment[4]);
			AddAvailableItem(baseEquipment, baseEquipment.GetEquipmentType(), gender);
		}
		Inventory inventory = character.GetInventory();
		foreach (KeyValuePair<ItemKey, int> item in inventory.Items)
		{
			item.Deconstruct(out var key, out var _);
			ItemKey itemKey = key;
			EquipmentBase equipmentBase = DomainManager.Item.TryGetBaseEquipment(itemKey);
			if (equipmentBase != null && equipmentBase.GetEquipmentType() == 2)
			{
				AddAvailableItem(equipmentBase, equipmentBase.GetEquipmentType(), gender);
			}
		}
		SelectEquipmentsModification selectEquipmentsModification = new SelectEquipmentsModification(character, removeUnequippedEquipment: false);
		short idealClothingTemplateId = character.GetIdealClothingTemplateId();
		ItemKey result = SelectClothing(character, selectEquipmentsModification, idealClothingTemplateId);
		if (selectEquipmentsModification.PersonalNeedChanged)
		{
			character.SetPersonalNeeds(character.GetPersonalNeeds(), context);
		}
		return result;
	}

	private void ClassifyAvailableItems(Character character)
	{
		_availableWeapons.Clear();
		_availableHelms.Clear();
		_availableTorsos.Clear();
		_availableBracers.Clear();
		_availableBoots.Clear();
		_availableAccessories.Clear();
		_availableClothing.Clear();
		_availableCarriers.Clear();
		ItemKey[] equipment = character.GetEquipment();
		sbyte gender = character.GetGender();
		for (int i = 0; i < 12; i++)
		{
			ItemKey itemKey = equipment[i];
			if (itemKey.IsValid())
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
				AddAvailableItem(baseEquipment, baseEquipment.GetEquipmentType(), gender);
			}
		}
		Inventory inventory = character.GetInventory();
		foreach (KeyValuePair<ItemKey, int> item in inventory.Items)
		{
			item.Deconstruct(out var key, out var _);
			ItemKey itemKey2 = key;
			EquipmentBase equipmentBase = DomainManager.Item.TryGetBaseEquipment(itemKey2);
			if (equipmentBase != null)
			{
				AddAvailableItem(equipmentBase, equipmentBase.GetEquipmentType(), gender);
			}
		}
	}

	private void AddAvailableItem(EquipmentBase item, sbyte equipmentType, sbyte gender)
	{
		if (item.GetMaxDurability() > 0 && item.GetCurrDurability() <= 0)
		{
			return;
		}
		switch (equipmentType)
		{
		case 0:
			_availableWeapons.Add((item.GetItemKey(), 0));
			break;
		case 1:
			_availableHelms.Add((GameData.Domains.Item.Armor)item);
			break;
		case 2:
		{
			GameData.Domains.Item.Clothing clothing = (GameData.Domains.Item.Clothing)item;
			if (clothing.GetAgeGroup() == 2)
			{
				_availableClothing.Add(clothing);
			}
			break;
		}
		case 3:
			_availableTorsos.Add((GameData.Domains.Item.Armor)item);
			break;
		case 4:
			_availableBracers.Add((GameData.Domains.Item.Armor)item);
			break;
		case 5:
			_availableBoots.Add((GameData.Domains.Item.Armor)item);
			break;
		case 6:
			_availableAccessories.Add((GameData.Domains.Item.Accessory)item);
			break;
		case 7:
			_availableCarriers.Add((GameData.Domains.Item.Carrier)item);
			break;
		}
	}

	public unsafe static bool GetWeaponScores(Character character, List<(ItemKey weapon, int score)> availableWeapons, List<(short itemTemplateId, short count)> suitableWeapons, HashSet<short> fixedBestWeapons)
	{
		suitableWeapons.Clear();
		fixedBestWeapons.Clear();
		byte* intPtr = stackalloc byte[8];
		// IL initblk instruction
		Unsafe.InitBlock(intPtr, 0, 8);
		short* pRequiredHitRates = (short*)intPtr;
		byte* intPtr2 = stackalloc byte[22];
		// IL initblk instruction
		Unsafe.InitBlock(intPtr2, 0, 22);
		byte* pRequiredTricks = intPtr2;
		CombatSkillEquipment combatSkillEquipment = character.GetCombatSkillEquipment();
		CalcAttackSkillsRequirement(combatSkillEquipment, pRequiredHitRates, pRequiredTricks, suitableWeapons, fixedBestWeapons);
		byte* pClonedRequiredTricks = stackalloc byte[22];
		bool hasMatchTricks = false;
		int i = 0;
		for (int count = availableWeapons.Count; i < count; i++)
		{
			ItemKey item = availableWeapons[i].weapon;
			int item2 = CalcWeaponScore(item, pRequiredTricks, pClonedRequiredTricks, pRequiredHitRates, ref hasMatchTricks, suitableWeapons, fixedBestWeapons);
			availableWeapons[i] = (item, item2);
		}
		return hasMatchTricks;
	}

	private unsafe static int CalcWeaponScore(ItemKey itemKey, byte* pRequiredTricks, byte* pClonedRequiredTricks, short* pRequiredHitRates, ref bool hasMatchTricks, List<(short itemTemplateId, short count)> suitableWeapons, HashSet<short> fixedBestWeapons)
	{
		WeaponItem weaponItem = Config.Weapon.Instance[itemKey.TemplateId];
		int num = weaponItem.Grade * 200;
		if (!itemKey.IsValid())
		{
			return num;
		}
		GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetBaseItem(itemKey) as GameData.Domains.Item.Weapon;
		if (ModificationStateHelper.IsActive(1, weapon.GetModificationState()))
		{
			PoisonsAndLevels attachedPoisons = DomainManager.Item.GetAttachedPoisons(weapon.GetItemKey());
			for (sbyte b = 0; b < 6; b++)
			{
				short num2 = attachedPoisons.Values[b];
				sbyte b2 = attachedPoisons.Levels[b];
				num += num2 * b2 * 2;
			}
		}
		Buffer.MemoryCopy(pRequiredTricks, pClonedRequiredTricks, 22L, 22L);
		int num3 = CalcMatchedTricksCount(pClonedRequiredTricks, weapon);
		short maxDurability = weapon.GetMaxDurability();
		if (maxDurability > 0)
		{
			num = num * weapon.GetCurrDurability() / maxDurability;
		}
		if (num3 > 0)
		{
			num += num3 * 10;
			hasMatchTricks = true;
			short itemTemplateId = (short)(itemKey.TemplateId - weaponItem.Grade);
			num += GetSuitableWeaponCount(itemTemplateId, suitableWeapons) * 300;
			if (fixedBestWeapons.Contains(itemKey.TemplateId))
			{
				num += 900;
			}
			HitOrAvoidShorts hitFactors = weapon.GetHitFactors();
			for (int i = 0; i < 4; i++)
			{
				num += pRequiredHitRates[i] * hitFactors.Items[i] / 150;
			}
			num += 65536;
		}
		return num;
	}

	private bool MatchCombatSkillByCombatConfig(short combatSkillTemplateId, CombatConfigItem combatConfig)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkillTemplateId];
		if (combatSkillItem.EquipType == 4 || combatSkillItem.EquipType == 0)
		{
			return true;
		}
		if (combatConfig.Sect >= 0 && combatConfig.Sect != combatSkillItem.SectId)
		{
			return false;
		}
		if (combatConfig.CombatSkillType != null && combatConfig.CombatSkillType.Count > 0 && !combatConfig.CombatSkillType.Contains(combatSkillItem.Type))
		{
			return false;
		}
		return true;
	}

	private void SelectWeapons(Character character, SelectEquipmentsModification mod)
	{
		if (!GetWeaponScores(character, _availableWeapons, _suitableWeapons, _fixedBestWeapons))
		{
			foreach (short fixedBestWeapon in _fixedBestWeapons)
			{
				PersonalNeed personalNeed = PersonalNeed.CreatePersonalNeed(10, 0, fixedBestWeapon);
				character.OfflineAddPersonalNeed(personalNeed);
				mod.PersonalNeedChanged = true;
			}
			foreach (var suitableWeapon in _suitableWeapons)
			{
				short item = suitableWeapon.itemTemplateId;
				PersonalNeed personalNeed2 = PersonalNeed.CreatePersonalNeed(10, 0, item);
				character.OfflineAddPersonalNeed(personalNeed2);
				mod.PersonalNeedChanged = true;
			}
		}
		_equippedItems[0] = SelectBestWeapon(removeSameType: true);
		_equippedItems[1] = SelectBestWeapon(removeSameType: true);
		_equippedItems[2] = SelectBestWeapon(removeSameType: false);
	}

	private void SelectFixedWeapons(Character character, SelectEquipmentsModification mod)
	{
		CharacterItem characterItem = Config.Character.Instance[character.GetTemplateId()];
		for (sbyte b = 0; b <= 2; b++)
		{
			PresetEquipmentItem presetWeapon = characterItem.PresetEquipment[b];
			if (presetWeapon.TemplateId < 0)
			{
				_equippedItems[b] = ItemKey.Invalid;
			}
			else
			{
				int index = _availableWeapons.FindIndex(((ItemKey weapon, int score) pair) => pair.weapon.TemplateId == presetWeapon.TemplateId);
				ItemKey item = _availableWeapons[index].weapon;
				_availableWeapons.RemoveAt(index);
				_equippedItems[b] = item;
			}
		}
	}

	private unsafe static void CalcAttackSkillsRequirement(CombatSkillEquipment skillEquipment, short* pRequiredHitRates, byte* pRequiredTricks, List<(short itemTemplateId, short count)> suitableWeapons, HashSet<short> fixedBestWeapons)
	{
		ArraySegmentList<short>.Enumerator enumerator = skillEquipment.Attack.GetEnumerator();
		while (enumerator.MoveNext())
		{
			short current = enumerator.Current;
			if (current >= 0)
			{
				CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[current];
				if (combatSkillItem.MostFittingWeaponID >= 0)
				{
					RecordSuitableWeapon(combatSkillItem.MostFittingWeaponID, suitableWeapons);
				}
				if (combatSkillItem.FixedBestWeaponID >= 0)
				{
					fixedBestWeapons.Add(combatSkillItem.FixedBestWeaponID);
				}
				if (!CombatSkillTemplateHelper.IsMindHitSkill(current))
				{
					pRequiredHitRates[2] += combatSkillItem.PerHitDamageRateDistribution[0];
					pRequiredHitRates[1] += combatSkillItem.PerHitDamageRateDistribution[1];
					*pRequiredHitRates += combatSkillItem.PerHitDamageRateDistribution[2];
				}
				else
				{
					pRequiredHitRates[3] += combatSkillItem.PerHitDamageRateDistribution[3];
				}
				List<NeedTrick> trickCost = combatSkillItem.TrickCost;
				int i = 0;
				for (int count = trickCost.Count; i < count; i++)
				{
					NeedTrick needTrick = trickCost[i];
					byte* num = pRequiredTricks + needTrick.TrickType;
					*num += needTrick.NeedCount;
				}
			}
		}
	}

	private unsafe static bool CalcWeaponScores(short* pRequiredHitRates, byte* pRequiredTricks, List<(GameData.Domains.Item.Weapon weapon, int score)> availableWeapons, List<(short itemTemplateId, short count)> suitableWeapons)
	{
		byte* ptr = stackalloc byte[22];
		bool result = false;
		int i = 0;
		for (int count = availableWeapons.Count; i < count; i++)
		{
			GameData.Domains.Item.Weapon item = availableWeapons[i].weapon;
			short templateId = item.GetTemplateId();
			WeaponItem weaponItem = Config.Weapon.Instance[templateId];
			Buffer.MemoryCopy(pRequiredTricks, ptr, 22L, 22L);
			int num = CalcMatchedTricksCount(ptr, item);
			int num2 = (num * 50 + weaponItem.Grade * 200) * item.GetCurrDurability() / item.GetMaxDurability();
			if (num > 0)
			{
				result = true;
				short itemTemplateId = (short)(templateId - weaponItem.Grade);
				num2 += GetSuitableWeaponCount(itemTemplateId, suitableWeapons) * 300;
				HitOrAvoidShorts hitFactors = item.GetHitFactors();
				for (int j = 0; j < 4; j++)
				{
					num2 += pRequiredHitRates[j] * hitFactors.Items[j] / 150;
				}
				num2 += 65536;
			}
			availableWeapons[i] = (item, num2);
		}
		return result;
	}

	private ItemKey SelectBestWeapon(bool removeSameType)
	{
		int count = _availableWeapons.Count;
		if (count <= 0)
		{
			return ItemKey.Invalid;
		}
		int num = int.MinValue;
		int index = 0;
		for (int i = 0; i < count; i++)
		{
			int item = _availableWeapons[i].score;
			if (item > num)
			{
				num = item;
				index = i;
			}
		}
		ItemKey item2 = _availableWeapons[index].weapon;
		if (removeSameType)
		{
			CollectionUtils.SwapAndRemove(_availableWeapons, index);
			WeaponItem weaponItem = Config.Weapon.Instance[item2.TemplateId];
			short itemSubType = weaponItem.ItemSubType;
			int j = 0;
			for (int num2 = _availableWeapons.Count; j < num2; j++)
			{
				ItemKey item3 = _availableWeapons[j].weapon;
				WeaponItem weaponItem2 = Config.Weapon.Instance[item3.TemplateId];
				short itemSubType2 = weaponItem2.ItemSubType;
				if (itemSubType2 == itemSubType && itemSubType2 != 16)
				{
					CollectionUtils.SwapAndRemove(_availableWeapons, j);
					num2--;
					j--;
				}
			}
		}
		return item2;
	}

	private static void RecordSuitableWeapon(short itemTemplateId, List<(short itemTemplateId, short count)> suitableWeapons)
	{
		int num = -1;
		int i = 0;
		for (int count = suitableWeapons.Count; i < count; i++)
		{
			if (suitableWeapons[i].itemTemplateId == itemTemplateId)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			suitableWeapons[num] = (itemTemplateId, (short)(suitableWeapons[num].count + 1));
		}
		else
		{
			suitableWeapons.Add((itemTemplateId, 1));
		}
	}

	private static int GetSuitableWeaponCount(short itemTemplateId, List<(short itemTemplateId, short count)> suitableWeapons)
	{
		int num = -1;
		int i = 0;
		for (int count = suitableWeapons.Count; i < count; i++)
		{
			if (suitableWeapons[i].itemTemplateId == itemTemplateId)
			{
				num = i;
				break;
			}
		}
		return (num >= 0) ? suitableWeapons[num].count : 0;
	}

	private unsafe static int CalcMatchedTricksCount(byte* pClonedRequiredTricks, GameData.Domains.Item.Weapon weapon)
	{
		int num = 0;
		List<sbyte> tricks = weapon.GetTricks();
		int i = 0;
		for (int count = tricks.Count; i < count; i++)
		{
			sbyte b = tricks[i];
			byte b2 = pClonedRequiredTricks[b];
			if (b2 > 0)
			{
				pClonedRequiredTricks[b] = (byte)(b2 - 1);
				num++;
			}
		}
		return num;
	}

	private ItemKey SelectClothing(Character character, SelectEquipmentsModification mod, short orgClothingTemplateId)
	{
		if (character.GetAgeGroup() != 2)
		{
			return character.GetEquipment()[4];
		}
		if (orgClothingTemplateId >= 0)
		{
			ItemKey result = SelectOrgClothing(character, orgClothingTemplateId);
			if (result.IsValid())
			{
				return result;
			}
			PersonalNeed personalNeed = PersonalNeed.CreatePersonalNeed(10, 3, orgClothingTemplateId);
			character.OfflineAddPersonalNeed(personalNeed);
			mod.PersonalNeedChanged = true;
		}
		int num = int.MinValue;
		int num2 = -1;
		int i = 0;
		for (int count = _availableClothing.Count; i < count; i++)
		{
			GameData.Domains.Item.Clothing clothing = _availableClothing[i];
			int item = CalcEquipmentScore(clothing.GetItemKey(), -1).score;
			if (item > num)
			{
				num = item;
				num2 = i;
			}
		}
		return (num2 >= 0) ? _availableClothing[num2].GetItemKey() : ItemKey.Invalid;
	}

	private ItemKey SelectOrgClothing(Character character, short orgClothingTemplateId)
	{
		ItemKey result = character.GetEquipment()[4];
		if (result.IsValid() && result.TemplateId == orgClothingTemplateId)
		{
			return result;
		}
		int i = 0;
		for (int count = _availableClothing.Count; i < count; i++)
		{
			GameData.Domains.Item.Clothing clothing = _availableClothing[i];
			if (clothing.GetTemplateId() == orgClothingTemplateId)
			{
				return clothing.GetItemKey();
			}
		}
		return ItemKey.Invalid;
	}

	private static ItemKey SelectEquipment<T>(SelectEquipmentsModification mod, List<T> availableEquipments, PresetEquipmentItemWithProb orgEquipment) where T : ItemBase
	{
		bool flag = false;
		int count = availableEquipments.Count;
		int num = int.MinValue;
		int index = 0;
		sbyte grade = mod.Character.GetOrganizationInfo().Grade;
		for (int i = 0; i < count; i++)
		{
			T val = availableEquipments[i];
			(int score, bool meetReq) tuple = CalcEquipmentScore(val.GetItemKey(), orgEquipment.TemplateId);
			int item = tuple.score;
			bool item2 = tuple.meetReq;
			flag = flag || item2;
			if (item > num)
			{
				num = item;
				index = i;
			}
		}
		if (!flag && orgEquipment.TemplateId >= 0)
		{
			short templateIdInGroup = ItemTemplateHelper.GetTemplateIdInGroup(orgEquipment.Type, orgEquipment.TemplateId, grade);
			PersonalNeed personalNeed = PersonalNeed.CreatePersonalNeed(10, orgEquipment.Type, templateIdInGroup);
			mod.Character.OfflineAddPersonalNeed(personalNeed);
			mod.PersonalNeedChanged = true;
		}
		if (count <= 0)
		{
			return ItemKey.Invalid;
		}
		ItemKey itemKey = availableEquipments[index].GetItemKey();
		CollectionUtils.SwapAndRemove(availableEquipments, index);
		return itemKey;
	}

	private static ItemKey SelectArmor(SelectEquipmentsModification mod, List<GameData.Domains.Item.Armor> availableArmors, PresetEquipmentItemWithProb orgEquipment)
	{
		return SelectEquipment(mod, availableArmors, orgEquipment);
	}

	private static ItemKey SelectAccessory(SelectEquipmentsModification mod, List<GameData.Domains.Item.Accessory> availableAccessories, PresetEquipmentItemWithProb orgEquipment)
	{
		return SelectEquipment(mod, availableAccessories, orgEquipment);
	}

	private static ItemKey SelectCarrier(SelectEquipmentsModification mod, List<GameData.Domains.Item.Carrier> availableCarriers, PresetEquipmentItemWithProb orgEquipment)
	{
		return SelectEquipment(mod, availableCarriers, orgEquipment);
	}

	public static (int score, bool meetReq) CalcEquipmentScore(ItemKey itemKey, int expectedItemGroupTemplateId)
	{
		sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
		int num = grade * 200;
		bool flag = expectedItemGroupTemplateId >= 0 && itemKey.TemplateId >= expectedItemGroupTemplateId && itemKey.TemplateId <= expectedItemGroupTemplateId + 8;
		if (flag)
		{
			num += 300;
		}
		if (itemKey.IsValid())
		{
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			short currDurability = baseItem.GetCurrDurability();
			short maxDurability = baseItem.GetMaxDurability();
			num = ((maxDurability <= 0) ? num : ((currDurability == 0) ? (-1) : (num * currDurability / maxDurability)));
		}
		return (score: num, meetReq: flag);
	}

	[Obsolete]
	private static bool MeetOrgRequirement(short itemTemplateId, PresetEquipmentItemWithProb orgEquipment)
	{
		return orgEquipment.TemplateId >= 0 && itemTemplateId >= orgEquipment.TemplateId && itemTemplateId <= orgEquipment.TemplateId + 8;
	}

	private void ChooseLoopingNeigong(Character character, SelectEquipmentsModification mod)
	{
		_candidateCombatSkillsForLooping.Clear();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(character.GetId());
		foreach (var (index, combatSkill2) in charCombatSkills)
		{
			if (CharacterDomain.IsLoopable(combatSkill2))
			{
				CombatSkillItem item = Config.CombatSkill.Instance[index];
				bool item2 = combatSkill2.GetObtainedNeili() < combatSkill2.GetTotalObtainableNeili();
				_candidateCombatSkillsForLooping.Add((item, item2));
			}
		}
		short num2 = SelectCombatSkillForLooping(character, _candidateCombatSkillsForLooping);
		if (num2 != character.GetLoopingNeigong())
		{
			mod.LoopingNeigongChanged = true;
			mod.LoopingNeigong = num2;
		}
	}

	private static short SelectCombatSkillForLooping(Character character, List<(CombatSkillItem skillCfg, bool canObtainNeili)> candidates)
	{
		sbyte orgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
		sbyte fiveElementsType = Config.Organization.Instance[orgTemplateId].FiveElementsType;
		sbyte idealSect = character.GetIdealSect();
		sbyte b = (sbyte)((idealSect >= 0) ? Config.Organization.Instance[idealSect].FiveElementsType : (-1));
		short num = -1;
		int num2 = -1;
		int i = 0;
		for (int count = candidates.Count; i < count; i++)
		{
			(CombatSkillItem skillCfg, bool canObtainNeili) tuple = candidates[i];
			CombatSkillItem item = tuple.skillCfg;
			bool item2 = tuple.canObtainNeili;
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), item.TemplateId));
			sbyte fiveElementsType2 = (sbyte)((element_CombatSkills.GetFiveElementsChange() > 0 && item.TransferTypeWhileLooping >= 0) ? NeiliProportionOfFiveElements.GetTransferSource(item.TransferTypeWhileLooping, item.DestTypeWhileLooping) : (-1));
			int num3 = 0;
			num3 += 50 * item.Grade;
			if (item2)
			{
				num3 += 800;
			}
			if (fiveElementsType >= 0)
			{
				if (!CheckCounterWithTargetFiveElementsType(item.FiveElements, fiveElementsType))
				{
					num3 += 200;
				}
				if (item.DestTypeWhileLooping == fiveElementsType)
				{
					num3 += 100;
				}
				if (CheckCounterWithTargetFiveElementsType(fiveElementsType2, fiveElementsType))
				{
					num3 += 100;
				}
			}
			if (b >= 0)
			{
				if (!CheckCounterWithTargetFiveElementsType(item.FiveElements, b))
				{
					num3 += 200;
				}
				if (item.DestTypeWhileLooping == b)
				{
					num3 += 100;
				}
				if (CheckCounterWithTargetFiveElementsType(fiveElementsType2, b))
				{
					num3 += 100;
				}
			}
			if (CanObtainExtraNeiliAllocationProgressFromSkill(character, item))
			{
				num3 += 200;
			}
			if (num3 > num2)
			{
				num = item.TemplateId;
				num2 = num3;
			}
		}
		return (short)((num2 >= 0) ? num : (-1));
	}

	private CombatSkillItem SelectCombatSkillForAdjustingNeiliType(Character character, List<(CombatSkillItem skillCfg, int index)> brokenOutNeigongList)
	{
		_candidateCombatSkillsForLooping.Clear();
		int i = 0;
		for (int count = brokenOutNeigongList.Count; i < count; i++)
		{
			CombatSkillItem item = brokenOutNeigongList[i].skillCfg;
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), item.TemplateId));
			if (element_CombatSkills.GetFiveElementsChange() > 0 && item.TransferTypeWhileLooping >= 0)
			{
				_candidateCombatSkillsForLooping.Add((item, false));
			}
		}
		short num = SelectCombatSkillForLooping(character, _candidateCombatSkillsForLooping);
		return (num < 0) ? null : Config.CombatSkill.Instance[num];
	}

	private static bool CanObtainExtraNeiliAllocationProgressFromSkill(Character character, CombatSkillItem skillCfg)
	{
		sbyte[] extraNeiliAllocationProgress = skillCfg.ExtraNeiliAllocationProgress;
		if (!DomainManager.Extra.TryGetExtraNeiliAllocationProgress(character.GetId(), out var result))
		{
			for (int i = 0; i < 4; i++)
			{
				if (extraNeiliAllocationProgress[i] > 0)
				{
					return true;
				}
			}
		}
		for (int j = 0; j < 4; j++)
		{
			sbyte b = extraNeiliAllocationProgress[j];
			if (b > 0)
			{
				int num = Math.Min(100 * GlobalConfig.Instance.ExtraNeiliAllocationFromProgressRatio * GlobalConfig.Instance.MaxExtraNeiliAllocation, b * 100 + result.Items[j]);
				if (num > result.Items[j])
				{
					return true;
				}
			}
		}
		return false;
	}

	public unsafe void AllocateNeili(DataContext context, Character character)
	{
		sbyte* skillSlotTotalCounts = stackalloc sbyte[5];
		SelectEquipmentsModification selectEquipmentsModification = new SelectEquipmentsModification(character, removeUnequippedEquipment: false);
		AllocateNeili(character, skillSlotTotalCounts, selectEquipmentsModification);
		if (selectEquipmentsModification.NeiliAllocationChanged)
		{
			character.SpecifyBaseNeiliAllocation(context, selectEquipmentsModification.NeiliAllocation);
		}
	}

	private unsafe static void AllocateNeili(Character character, sbyte* skillSlotTotalCounts, SelectEquipmentsModification mod)
	{
		sbyte neiliType = character.GetNeiliType();
		sbyte[] ideaAllocationProportion = NeiliType.Instance[neiliType].IdeaAllocationProportion;
		CharacterItem characterItem = Config.Character.Instance[character.GetTemplateId()];
		if (character.GetCreatingType() == 0 && characterItem.IdeaAllocationProportion.Sum() > 0)
		{
			ideaAllocationProportion = characterItem.IdeaAllocationProportion;
		}
		sbyte* pProportions = stackalloc sbyte[4]
		{
			ideaAllocationProportion[0],
			ideaAllocationProportion[1],
			ideaAllocationProportion[2],
			ideaAllocationProportion[3]
		};
		NeiliAllocation neiliAllocation = FindBestNeiliAllocation(character, pProportions);
		NeiliAllocation baseNeiliAllocation = character.GetBaseNeiliAllocation();
		if (!NeiliAllocation.Equals(baseNeiliAllocation, neiliAllocation))
		{
			mod.NeiliAllocationChanged = true;
			mod.NeiliAllocation = neiliAllocation;
		}
	}

	private unsafe static void CalcNeiliAllocationProportions(sbyte* skillSlotTotalCounts, sbyte* pProportions)
	{
		for (int i = 0; i < 4; i++)
		{
			pProportions[i] = (sbyte)Math.Max(skillSlotTotalCounts[i + 1] + 6, 6);
		}
	}

	private unsafe static NeiliAllocation FindBestNeiliAllocation(Character character, sbyte* pProportions)
	{
		sbyte* ptr = stackalloc sbyte[4] { 0, 1, 2, 3 };
		Span<sbyte> keys = new Span<sbyte>(pProportions, 4);
		Span<sbyte> items = new Span<sbyte>(ptr, 4);
		keys.Sort(items, ReverseComparer);
		short proportionSum = (short)(*pProportions + pProportions[1] + pProportions[2] + pProportions[3]);
		short maxTotalNeiliAllocationConsideringFeature = CombatHelper.GetMaxTotalNeiliAllocationConsideringFeature(character.GetConsummateLevel(), character.GetFeatureIds());
		NeiliAllocation neiliAllocation = CalcNeiliAllocation(pProportions, proportionSum, maxTotalNeiliAllocationConsideringFeature);
		int pureCurrNeili = character.GetPureCurrNeili();
		if (CombatHelper.CalcRequiredNeili(neiliAllocation) <= pureCurrNeili)
		{
			return RestorePositions(neiliAllocation, ptr);
		}
		neiliAllocation = BinarySearch(pProportions, proportionSum, maxTotalNeiliAllocationConsideringFeature, pureCurrNeili);
		neiliAllocation = RestorePositions(neiliAllocation, ptr);
		Tester.Assert(CombatHelper.CalcRequiredNeili(neiliAllocation) <= pureCurrNeili);
		short total = neiliAllocation.GetTotal();
		NeiliAllocation allocation = CalcNeiliAllocation(pProportions, proportionSum, total + 1);
		Tester.Assert(CombatHelper.CalcRequiredNeili(allocation) > pureCurrNeili);
		return neiliAllocation;
	}

	private unsafe static NeiliAllocation BinarySearch(sbyte* pProportions, int proportionSum, int maxTotalAllocation, int availableNeili)
	{
		int num = 0;
		int num2 = maxTotalAllocation;
		while (num <= num2)
		{
			int num3 = num + (num2 - num) / 2;
			NeiliAllocation neiliAllocation = CalcNeiliAllocation(pProportions, proportionSum, num3);
			int num4 = CombatHelper.CalcRequiredNeili(neiliAllocation) - availableNeili;
			if (num4 == 0)
			{
				return neiliAllocation;
			}
			if (num4 < 0)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3 - 1;
			}
		}
		int num5 = num - 1;
		if (num5 > 0)
		{
			return CalcNeiliAllocation(pProportions, proportionSum, num5);
		}
		NeiliAllocation result = default(NeiliAllocation);
		result.Initialize();
		return result;
	}

	private unsafe static NeiliAllocation CalcNeiliAllocation(sbyte* pProportions, int proportionSum, int totalAllocation)
	{
		int num = proportionSum;
		int num2 = totalAllocation;
		NeiliAllocation result = default(NeiliAllocation);
		for (int i = 0; i < 4; i++)
		{
			sbyte b = pProportions[i];
			int num3 = num2 * b / num;
			if (num3 > 100)
			{
				num3 = 100;
			}
			result.Items[i] = (short)num3;
			num2 -= num3;
			num -= b;
		}
		return result;
	}

	private unsafe static NeiliAllocation RestorePositions(NeiliAllocation allocations, sbyte* pAllocationTypes)
	{
		NeiliAllocation result = default(NeiliAllocation);
		for (int i = 0; i < 4; i++)
		{
			sbyte b = pAllocationTypes[i];
			result.Items[b] = allocations.Items[i];
		}
		return result;
	}

	public (GameData.Domains.Item.SkillBook book, int learnedSkillIndex, byte readingPage) GetCurrReadingBook(Character character)
	{
		_availableReadingBooks.Clear();
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		character.GetReadableBookList(list);
		foreach (ItemKey item4 in list)
		{
			GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(item4.Id);
			int item;
			byte item2;
			if (element_SkillBooks.IsCombatSkillBook())
			{
				(item, item2) = character.GetCombatSkillBookCurrReadingInfo(element_SkillBooks);
			}
			else
			{
				(item, item2) = character.GetLifeSkillBookCurrReadingInfo(element_SkillBooks);
			}
			_availableReadingBooks.Add((element_SkillBooks, item, item2));
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
		if (_availableReadingBooks.Count == 0)
		{
			return (book: null, learnedSkillIndex: -1, readingPage: 0);
		}
		_hasPersonalNeedToReadBooks.Clear();
		_hasPersonalNeedToLearnCombatSkillTypes.Clear();
		_hasPersonalNeedToLearnLifeSkillTypes.Clear();
		foreach (PersonalNeed personalNeed in character.GetPersonalNeeds())
		{
			if (personalNeed.TemplateId == 14)
			{
				_hasPersonalNeedToLearnCombatSkillTypes.Add(personalNeed.CombatSkillType);
			}
			else if (personalNeed.TemplateId == 15)
			{
				_hasPersonalNeedToLearnLifeSkillTypes.Add(personalNeed.LifeSkillType);
			}
		}
		int index = -1;
		int num = int.MinValue;
		int num2 = -1;
		int num3 = int.MaxValue;
		int num4 = int.MinValue;
		for (int i = 0; i < _availableReadingBooks.Count; i++)
		{
			GameData.Domains.Item.SkillBook item3 = _availableReadingBooks[i].book;
			sbyte grade = item3.GetGrade();
			short readingAttainmentRequirement = SkillGradeData.Instance[grade].ReadingAttainmentRequirement;
			SkillBookItem bookConfig = Config.SkillBook.Instance[item3.GetItemKey().TemplateId];
			short attainment;
			int num5 = CalcSkillBookScore(item3, bookConfig, character, _hasPersonalNeedToLearnCombatSkillTypes, _hasPersonalNeedToLearnLifeSkillTypes, out attainment);
			if (num5 > num)
			{
				num = num5;
				index = i;
			}
			int num6 = attainment - readingAttainmentRequirement;
			if (num6 >= 0 && num6 < num3)
			{
				num3 = num6;
				num2 = i;
				num4 = num5;
			}
		}
		if (num2 >= 0)
		{
			num4 += 50;
		}
		return (num4 > num) ? _availableReadingBooks[num2] : _availableReadingBooks[index];
	}

	private unsafe static int CalcSkillBookScore(GameData.Domains.Item.SkillBook book, SkillBookItem bookConfig, Character character, List<sbyte> needToLearnCombatSkillTypes, List<sbyte> needToLearnLifeSkillTypes, out short attainment)
	{
		short villagerRoleTemplateId = DomainManager.Extra.GetVillagerRoleTemplateId(character.GetId());
		CombatSkillShorts combatSkillAttainments = character.GetCombatSkillAttainments();
		LifeSkillShorts lifeSkillAttainments = character.GetLifeSkillAttainments();
		CombatSkillShorts qualifications = character.GetCombatSkillQualifications();
		LifeSkillShorts qualifications2 = character.GetLifeSkillQualifications();
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		sbyte idealSect = character.GetIdealSect();
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig((idealSect >= 0) ? idealSect : organizationInfo.OrgTemplateId, organizationInfo.Grade);
		OrganizationMemberItem orgMemberConfig2 = OrganizationDomain.GetOrgMemberConfig(organizationInfo.OrgTemplateId, organizationInfo.Grade);
		VillagerWorkData villagerWorkData = DomainManager.Extra.GetVillagerRole(character.GetId())?.WorkData;
		BuildingBlockData value;
		int num = ((villagerWorkData == null || !DomainManager.Building.TryGetElement_BuildingBlocks(new BuildingBlockKey(villagerWorkData.AreaId, villagerWorkData.BlockId, villagerWorkData.BuildingBlockIndex), out value)) ? (-1) : ((bookConfig.CombatSkillType >= 0) ? value.ConfigData.RequireCombatSkillType : value.ConfigData.RequireLifeSkillType));
		int num2;
		if (bookConfig.CombatSkillType >= 0)
		{
			sbyte combatSkillType = bookConfig.CombatSkillType;
			attainment = combatSkillAttainments.Items[combatSkillType];
			num2 = CalcCombatSkillBookScore(book, combatSkillType, orgMemberConfig2, orgMemberConfig, ref qualifications, villagerRoleTemplateId, needToLearnCombatSkillTypes);
			if (num == bookConfig.CombatSkillType)
			{
				num2 += 1000;
			}
		}
		else
		{
			sbyte lifeSkillType = bookConfig.LifeSkillType;
			attainment = lifeSkillAttainments.Items[lifeSkillType];
			num2 = CalcLifeSkillBookScore(book, lifeSkillType, orgMemberConfig2, orgMemberConfig, ref qualifications2, villagerRoleTemplateId, needToLearnLifeSkillTypes);
			if (num == bookConfig.LifeSkillType)
			{
				num2 += 1000;
			}
		}
		return num2;
	}

	private unsafe static int CalcCombatSkillBookScore(GameData.Domains.Item.SkillBook book, sbyte combatSkillType, OrganizationMemberItem selfOrgMemberCfg, OrganizationMemberItem lovingOrgMemberCfg, ref CombatSkillShorts qualifications, short roleTemplateId, List<sbyte> needToLearnSkillTypes)
	{
		int num = 0;
		num += selfOrgMemberCfg.CombatSkillsAdjust[combatSkillType] * 10;
		num += lovingOrgMemberCfg.CombatSkillsAdjust[combatSkillType] * 5;
		num += qualifications.Items[combatSkillType];
		if (book != null)
		{
			num += SkillBookStateHelper.GetTotalIncompleteStateValue(book.GetPageIncompleteState(), book.GetPageCount());
		}
		if (needToLearnSkillTypes != null && needToLearnSkillTypes.Contains(combatSkillType))
		{
			num += 100;
		}
		if (roleTemplateId == 5)
		{
			num += 100;
		}
		return num;
	}

	private unsafe static int CalcLifeSkillBookScore(GameData.Domains.Item.SkillBook book, sbyte lifeSkillType, OrganizationMemberItem selfOrgMemberCfg, OrganizationMemberItem lovingOrgMemberCfg, ref LifeSkillShorts qualifications, short roleTemplateId, List<sbyte> needToLearnSkillTypes)
	{
		int num = 0;
		num += selfOrgMemberCfg.LifeSkillsAdjust[lifeSkillType] * 10;
		num += lovingOrgMemberCfg.LifeSkillsAdjust[lifeSkillType] * 5;
		num += qualifications.Items[lifeSkillType];
		if (book != null)
		{
			num += SkillBookStateHelper.GetTotalIncompleteStateValue(book.GetPageIncompleteState(), book.GetPageCount());
		}
		if (needToLearnSkillTypes != null && needToLearnSkillTypes.Contains(lifeSkillType))
		{
			num += 100;
		}
		HashSet<sbyte> hashSet = TaiwuDomain.VillagerRoleNeedLifeSkillBooks[roleTemplateId];
		if (hashSet != null && hashSet.Contains(lifeSkillType))
		{
			num += 100;
		}
		return num;
	}

	public static void SortBooksByScore(List<(ItemKey itemKey, int score)> items, Character character)
	{
		List<sbyte> needToLearnCombatSkillTypes = ObjectPool<List<sbyte>>.Instance.Get();
		List<sbyte> needToLearnLifeSkillTypes = ObjectPool<List<sbyte>>.Instance.Get();
		foreach (PersonalNeed personalNeed in character.GetPersonalNeeds())
		{
			if (personalNeed.TemplateId == 14)
			{
				needToLearnCombatSkillTypes.Add(personalNeed.CombatSkillType);
			}
			else if (personalNeed.TemplateId == 15)
			{
				needToLearnLifeSkillTypes.Add(personalNeed.LifeSkillType);
			}
		}
		items.Sort(delegate((ItemKey itemKey, int score) a, (ItemKey itemKey, int score) b)
		{
			GameData.Domains.Item.SkillBook book = (a.itemKey.IsValid() ? DomainManager.Item.GetElement_SkillBooks(a.itemKey.Id) : null);
			SkillBookItem bookConfig = Config.SkillBook.Instance[a.itemKey.TemplateId];
			a.score = CalcSkillBookScore(book, bookConfig, character, needToLearnCombatSkillTypes, needToLearnLifeSkillTypes, out var attainment);
			GameData.Domains.Item.SkillBook book2 = (b.itemKey.IsValid() ? DomainManager.Item.GetElement_SkillBooks(b.itemKey.Id) : null);
			SkillBookItem bookConfig2 = Config.SkillBook.Instance[b.itemKey.TemplateId];
			b.score = CalcSkillBookScore(book2, bookConfig2, character, needToLearnCombatSkillTypes, needToLearnLifeSkillTypes, out attainment);
			return a.score.CompareTo(b.score);
		});
		ObjectPool<List<sbyte>>.Instance.Return(needToLearnCombatSkillTypes);
		ObjectPool<List<sbyte>>.Instance.Return(needToLearnLifeSkillTypes);
	}
}
