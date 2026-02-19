using System;
using System.Collections.Generic;
using GameData.DLC;
using GameData.DLC.FiveLoong;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation.RelationTree;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.SpecialEffect;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.World;
using GameData.Domains.World.SectMainStory;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Global;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
public class CrossArchiveGameData : ISerializableGameData
{
	private static class CrossArchiveTaiwuSerializer
	{
		public static int GetSerializedSize(GameData.Domains.Character.Character target)
		{
			return (4 + target?.GetSerializedSize()).GetValueOrDefault();
		}

		public unsafe static int Serialize(byte* pData, GameData.Domains.Character.Character target)
		{
			byte* ptr = pData;
			if (target != null)
			{
				byte* ptr2 = ptr;
				ptr += 4;
				int num = target.Serialize(ptr);
				ptr += num;
				*(int*)ptr2 = num;
			}
			else
			{
				*(int*)ptr = 0;
				ptr += 4;
			}
			return (int)(ptr - pData);
		}

		public unsafe static int Deserialize(byte* pData, ref GameData.Domains.Character.Character target)
		{
			byte* ptr = pData;
			if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 69))
			{
				ushort num = *(ushort*)ptr;
				ptr += 2;
				if (num > 0)
				{
					if (target == null)
					{
						target = new GameData.Domains.Character.Character();
					}
					ptr += target.Deserialize_Legacy(ptr);
				}
				else
				{
					target = null;
				}
			}
			else
			{
				uint num2 = *(uint*)ptr;
				ptr += 4;
				if (num2 != 0)
				{
					if (target == null)
					{
						target = new GameData.Domains.Character.Character();
					}
					ptr += target.Deserialize(ptr);
				}
				else
				{
					target = null;
				}
			}
			return (int)(ptr - pData);
		}
	}

	private static class FieldIds
	{
		public const ushort TaiwuChar = 0;

		public const ushort CombatSkills = 1;

		public const ushort TaiwuEffects = 2;

		public const ushort UnpackedItems = 3;

		public const ushort TaiwuVillageLocation = 4;

		public const ushort TaiwuVillageAreaData = 5;

		public const ushort TaiwuVillageBlocks = 6;

		public const ushort Chicken = 7;

		public const ushort WarehouseItems = 8;

		public const ushort TaiwuCombatSkills = 9;

		public const ushort TaiwuLifeSkills = 10;

		public const ushort NotLearnedCombatSkills = 11;

		public const ushort NotLearnedLifeSkills = 12;

		public const ushort CombatSkillPlans = 13;

		public const ushort CurrCombatSkillPlanId = 14;

		public const ushort CurrLifeSkillAttainmentPanelPlanIndex = 15;

		public const ushort SkillBreakPlateObsoleteDict = 16;

		public const ushort SkillBreakBonusDict = 17;

		public const ushort CombatSkillAttainmentPanelPlans = 18;

		public const ushort CurrCombatSkillAttainmentPanelPlanIds = 19;

		public const ushort EquipmentsPlans = 20;

		public const ushort CurrEquipmentPlanId = 21;

		public const ushort WeaponInnerRatios = 22;

		public const ushort VoiceWeaponInnerRatio = 23;

		public const ushort ReadingBooks = 24;

		public const ushort ObsoleteProfessions = 25;

		public const ushort CurrProfessionId = 26;

		public const ushort TreasuryItems = 27;

		public const ushort TroughItems = 28;

		public const ushort ReadingEventBookIdList = 29;

		public const ushort ReadingEventReferenceBooks = 30;

		public const ushort ClearedSkillPlateStepInfo = 31;

		public const ushort TaiwuMaxNeiliAllocation = 32;

		public const ushort CurrMasteredCombatSkillPlan = 33;

		public const ushort MasteredCombatSkillPlans = 34;

		public const ushort TaiwuExp = 35;

		public const ushort TaiwuResources = 36;

		public const ushort LegendaryBookBreakPlateCounts = 37;

		public const ushort CombatSkillBreakPlateObsoleteList = 38;

		public const ushort CombatSkillBreakPlateLastClearTimeList = 39;

		public const ushort CombatSkillBreakPlateLastForceBreakoutStepsCount = 40;

		public const ushort CombatSkillCurrBreakPlateIndex = 41;

		public const ushort LegendaryBookWeaponSlot = 42;

		public const ushort LegendaryBookWeaponEffectId = 43;

		public const ushort LegendaryBookSkillSlot = 44;

		public const ushort LegendaryBookSkillEffectId = 45;

		public const ushort LegendaryBookBonusCountYin = 46;

		public const ushort LegendaryBookBonusCountYang = 47;

		public const ushort HandledOneShotEvents = 48;

		public const ushort LegaciesBuildingTemplateIds = 49;

		public const ushort CollectionCrickets = 50;

		public const ushort CollectionCricketRegen = 51;

		public const ushort CollectionCricketJars = 52;

		public const ushort BuildingSpaceExtraAdd = 53;

		public const ushort NormalInformation = 54;

		public const ushort JiaoPools = 55;

		public const ushort SectEmeiSkillBreakBonus = 56;

		public const ushort SectEmeiBreakBonusTemplateIds = 57;

		public const ushort MaxTaiwuVillageLevel = 58;

		public const ushort IsJiaoPoolOpen = 59;

		public const ushort CricketCollectionDatas = 60;

		public const ushort UnlockedCombatSkillPlanCount = 61;

		public const ushort AvailableReadingStrategyMap = 62;

		public const ushort ExtraNeiliAllocationProgress = 63;

		public const ushort ExtraNeiliAllocation = 64;

		public const ushort SectEmeiBonusData = 65;

		public const ushort SectFulongOrgMemberChickens = 66;

		public const ushort CraftStorageObsolete = 67;

		public const ushort MedicineStorageObsolete = 68;

		public const ushort FoodStorageObsolete = 69;

		public const ushort StockStorageObsolete = 70;

		public const ushort TaiwuSettlementTreasuryObsolete = 71;

		public const ushort Professions = 72;

		public const ushort ExternalEquippedCombatSkills = 73;

		public const ushort SectZhujianGearMate = 74;

		public const ushort CombatSkillBreakPlateList = 75;

		public const ushort SkillBreakPlateDict = 76;

		public const ushort TaiwuCombatSkillProficiencies = 77;

		public const ushort XiangshuIdInKungfuPracticeRoom = 78;

		public const ushort CraftStorage = 79;

		public const ushort MedicineStorage = 80;

		public const ushort FoodStorage = 81;

		public const ushort StockStorage = 82;

		public const ushort TaiwuSettlementTreasury = 83;

		public const ushort TaiwuVillageBlocksEx = 84;

		public const ushort Count = 85;

		public static readonly string[] FieldId2FieldName = new string[85]
		{
			"TaiwuChar", "CombatSkills", "TaiwuEffects", "UnpackedItems", "TaiwuVillageLocation", "TaiwuVillageAreaData", "TaiwuVillageBlocks", "Chicken", "WarehouseItems", "TaiwuCombatSkills",
			"TaiwuLifeSkills", "NotLearnedCombatSkills", "NotLearnedLifeSkills", "CombatSkillPlans", "CurrCombatSkillPlanId", "CurrLifeSkillAttainmentPanelPlanIndex", "SkillBreakPlateObsoleteDict", "SkillBreakBonusDict", "CombatSkillAttainmentPanelPlans", "CurrCombatSkillAttainmentPanelPlanIds",
			"EquipmentsPlans", "CurrEquipmentPlanId", "WeaponInnerRatios", "VoiceWeaponInnerRatio", "ReadingBooks", "ObsoleteProfessions", "CurrProfessionId", "TreasuryItems", "TroughItems", "ReadingEventBookIdList",
			"ReadingEventReferenceBooks", "ClearedSkillPlateStepInfo", "TaiwuMaxNeiliAllocation", "CurrMasteredCombatSkillPlan", "MasteredCombatSkillPlans", "TaiwuExp", "TaiwuResources", "LegendaryBookBreakPlateCounts", "CombatSkillBreakPlateObsoleteList", "CombatSkillBreakPlateLastClearTimeList",
			"CombatSkillBreakPlateLastForceBreakoutStepsCount", "CombatSkillCurrBreakPlateIndex", "LegendaryBookWeaponSlot", "LegendaryBookWeaponEffectId", "LegendaryBookSkillSlot", "LegendaryBookSkillEffectId", "LegendaryBookBonusCountYin", "LegendaryBookBonusCountYang", "HandledOneShotEvents", "LegaciesBuildingTemplateIds",
			"CollectionCrickets", "CollectionCricketRegen", "CollectionCricketJars", "BuildingSpaceExtraAdd", "NormalInformation", "JiaoPools", "SectEmeiSkillBreakBonus", "SectEmeiBreakBonusTemplateIds", "MaxTaiwuVillageLevel", "IsJiaoPoolOpen",
			"CricketCollectionDatas", "UnlockedCombatSkillPlanCount", "AvailableReadingStrategyMap", "ExtraNeiliAllocationProgress", "ExtraNeiliAllocation", "SectEmeiBonusData", "SectFulongOrgMemberChickens", "CraftStorageObsolete", "MedicineStorageObsolete", "FoodStorageObsolete",
			"StockStorageObsolete", "TaiwuSettlementTreasuryObsolete", "Professions", "ExternalEquippedCombatSkills", "SectZhujianGearMate", "CombatSkillBreakPlateList", "SkillBreakPlateDict", "TaiwuCombatSkillProficiencies", "XiangshuIdInKungfuPracticeRoom", "CraftStorage",
			"MedicineStorage", "FoodStorage", "StockStorage", "TaiwuSettlementTreasury", "TaiwuVillageBlocksEx"
		};
	}

	[SerializableGameDataField(SerializationHandler = "CrossArchiveTaiwuSerializer")]
	public GameData.Domains.Character.Character TaiwuChar;

	public AbridgedCharacter DreamBackTaiwuAbridged;

	public int NextObjectId;

	public Dictionary<int, AbridgedCharacter> AbridgedCharacters;

	public ReadonlyLifeRecords LifeRecords;

	public Genealogy Genealogy;

	public Dictionary<int, DeadCharacter> PreexistenceDeadCharacters;

	[SerializableGameDataField]
	public ResourceInts TaiwuResources;

	[SerializableGameDataField]
	public int TaiwuExp;

	[SerializableGameDataField]
	public CombatSkillPlan ExternalEquippedCombatSkills;

	public int FuyuFaith;

	[SerializableGameDataField]
	public NormalInformationCollection NormalInformation;

	[SerializableGameDataField]
	public List<GameData.Domains.CombatSkill.CombatSkill> CombatSkills;

	[SerializableGameDataField]
	public List<SpecialEffectWrapper> TaiwuEffects;

	public ItemGroupPackage ItemGroupPackage;

	[SerializableGameDataField]
	public Dictionary<int, ItemKey> UnpackedItems;

	[SerializableGameDataField]
	public Location TaiwuVillageLocation;

	[SerializableGameDataField]
	public BuildingAreaData TaiwuVillageAreaData;

	[SerializableGameDataField]
	public List<BuildingBlockData> TaiwuVillageBlocks;

	[SerializableGameDataField]
	public List<BuildingBlockDataEx> TaiwuVillageBlocksEx;

	[SerializableGameDataField]
	public Dictionary<int, Chicken> Chicken;

	[SerializableGameDataField]
	public List<sbyte> XiangshuIdInKungfuPracticeRoom;

	public CombatSkillShorts SamsaraPlatformAddCombatSkillQualifications;

	public LifeSkillShorts SamsaraPlatformAddLifeSkillQualifications;

	public MainAttributes SamsaraPlatformAddMainAttributes;

	[SerializableGameDataField]
	[Obsolete]
	public ItemKey[] CollectionCrickets;

	[SerializableGameDataField]
	[Obsolete]
	public ItemKey[] CollectionCricketJars;

	[SerializableGameDataField]
	[Obsolete]
	public int[] CollectionCricketRegen;

	[SerializableGameDataField]
	public List<CricketCollectionData> CricketCollectionDatas;

	[SerializableGameDataField]
	public Dictionary<ItemKey, int> WarehouseItems;

	[SerializableGameDataField]
	public Dictionary<short, TaiwuCombatSkill> TaiwuCombatSkills;

	[SerializableGameDataField]
	public Dictionary<short, TaiwuLifeSkill> TaiwuLifeSkills;

	[SerializableGameDataField]
	public Dictionary<short, TaiwuCombatSkill> NotLearnedCombatSkills;

	[SerializableGameDataField]
	public Dictionary<short, TaiwuLifeSkill> NotLearnedLifeSkills;

	[SerializableGameDataField]
	public CombatSkillPlan[] CombatSkillPlans;

	[SerializableGameDataField]
	public int CurrCombatSkillPlanId;

	[SerializableGameDataField]
	public sbyte[] CurrLifeSkillAttainmentPanelPlanIndex;

	[SerializableGameDataField]
	public Dictionary<short, SkillBreakPlate> SkillBreakPlateDict;

	[SerializableGameDataField]
	public Dictionary<short, SkillBreakPlateObsolete> SkillBreakPlateObsoleteDict;

	[SerializableGameDataField]
	public Dictionary<short, SkillBreakBonusCollection> SkillBreakBonusDict;

	[SerializableGameDataField]
	public short[] CombatSkillAttainmentPanelPlans;

	[SerializableGameDataField]
	public sbyte[] CurrCombatSkillAttainmentPanelPlanIds;

	[SerializableGameDataField]
	public EquipmentPlan[] EquipmentsPlans;

	[SerializableGameDataField]
	public int CurrEquipmentPlanId;

	[SerializableGameDataField]
	public sbyte[] WeaponInnerRatios;

	[SerializableGameDataField]
	public sbyte VoiceWeaponInnerRatio;

	[SerializableGameDataField]
	public Dictionary<ItemKey, ReadingBookStrategies> ReadingBooks;

	public Dictionary<int, NotificationSortingGroup> MonthlyNotificationSortingGroups;

	public List<int> PreviousTaiwuCharIds;

	[SerializableGameDataField]
	public int BuildingSpaceExtraAdd;

	[SerializableGameDataField]
	public IntList ExtraNeiliAllocationProgress;

	[SerializableGameDataField]
	public NeiliAllocation ExtraNeiliAllocation;

	public Dictionary<int, string> CustomTexts;

	public int NextCustomTextId;

	public int FinalDateBeforeDreamBack;

	public WorldCreationInfo WorldCreationInfo;

	public uint WorldId;

	[Obsolete]
	[SerializableGameDataField]
	public int CurrProfessionId;

	[Obsolete]
	[SerializableGameDataField]
	public Dictionary<int, ObsoleteProfessionData> ObsoleteProfessions;

	[SerializableGameDataField]
	public Dictionary<int, ProfessionData> Professions;

	[SerializableGameDataField]
	public List<int> HandledOneShotEvents;

	[SerializableGameDataField]
	public Dictionary<ItemKey, int> TreasuryItems;

	[SerializableGameDataField]
	public Dictionary<ItemKey, int> TroughItems;

	[SerializableGameDataField]
	public List<short> LegaciesBuildingTemplateIds;

	[SerializableGameDataField]
	public List<int> ReadingEventBookIdList;

	[SerializableGameDataField]
	public Dictionary<int, ShortList> ReadingEventReferenceBooks;

	[SerializableGameDataField]
	public Dictionary<int, SByteList> AvailableReadingStrategyMap;

	[SerializableGameDataField]
	public Dictionary<short, IntPair> ClearedSkillPlateStepInfo;

	[SerializableGameDataField]
	public NeiliAllocation TaiwuMaxNeiliAllocation;

	[SerializableGameDataField]
	public ShortList CurrMasteredCombatSkillPlan;

	[SerializableGameDataField]
	public ShortList[] MasteredCombatSkillPlans;

	[SerializableGameDataField]
	public byte UnlockedCombatSkillPlanCount;

	[SerializableGameDataField]
	public List<JiaoPool> JiaoPools;

	[SerializableGameDataField]
	public bool IsJiaoPoolOpen;

	[SerializableGameDataField]
	public int MaxTaiwuVillageLevel;

	[SerializableGameDataField]
	public Dictionary<short, int> TaiwuCombatSkillProficiencies;

	[SerializableGameDataField]
	public Dictionary<short, SkillBreakBonusCollection> SectEmeiSkillBreakBonus;

	[SerializableGameDataField]
	public Dictionary<short, ShortList> SectEmeiBreakBonusTemplateIds;

	[SerializableGameDataField]
	public Dictionary<short, SectEmeiBreakBonusData> SectEmeiBonusData;

	[SerializableGameDataField]
	public Dictionary<short, IntList> SectFulongOrgMemberChickens;

	[SerializableGameDataField]
	public GearMateDreamBackData SectZhujianGearMate;

	[Obsolete]
	[SerializableGameDataField]
	private TaiwuVillageStorage _stockStorageObsolete;

	[Obsolete]
	[SerializableGameDataField]
	private TaiwuVillageStorage _craftStorageObsolete;

	[Obsolete]
	[SerializableGameDataField]
	private TaiwuVillageStorage _medicineStorageObsolete;

	[Obsolete]
	[SerializableGameDataField]
	private TaiwuVillageStorage _foodStorageObsolete;

	[Obsolete]
	[SerializableGameDataField]
	private SettlementTreasury _taiwuSettlementTreasuryObsolete;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	private TaiwuVillageStorage _stockStorage;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	[Obsolete]
	private TaiwuVillageStorage _craftStorage;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	[Obsolete]
	private TaiwuVillageStorage _medicineStorage;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	[Obsolete]
	private TaiwuVillageStorage _foodStorage;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	private SettlementTreasury _taiwuSettlementTreasury;

	public EventArgBox GlobalEventArgBox;

	public EventArgBox DlcEventArgBox;

	public Dictionary<ulong, DlcEntryWrapper> DlcEntries;

	public EventArgBox[] SectMainStoryEventArgBoxes;

	public Dictionary<short, CharacterPropertyBonus> TaiwuPropertyPermanentBonuses;

	public byte TaiwuPoisonImmunities;

	public Dictionary<IntPair, int> CharacterTemporaryFeatures;

	public List<short> SelectedUniqueLegacies;

	public HashSetAsDictionary<short> ProficiencyEnoughSkills;

	public HashSetAsDictionary<short> OwnedClothingSet;

	public Dictionary<int, short> ClothingDisplayModifications;

	public bool EnemyUnyieldingFallen;

	public bool EnemyDisableAi;

	public short LastTargetDistance;

	public int LastCricketPlanIndex;

	public List<CricketCombatPlan> CricketCombatPlans;

	[SerializableGameDataField]
	public Dictionary<sbyte, sbyte> LegendaryBookBreakPlateCounts;

	[SerializableGameDataField]
	public Dictionary<short, SkillBreakPlateList> CombatSkillBreakPlateList;

	[SerializableGameDataField]
	public Dictionary<short, SkillBreakPlateObsoleteList> CombatSkillBreakPlateObsoleteList;

	[SerializableGameDataField]
	public Dictionary<short, IntList> CombatSkillBreakPlateLastClearTimeList;

	[SerializableGameDataField]
	public Dictionary<short, IntList> CombatSkillBreakPlateLastForceBreakoutStepsCount;

	[SerializableGameDataField]
	public Dictionary<short, sbyte> CombatSkillCurrBreakPlateIndex;

	[SerializableGameDataField]
	public Dictionary<sbyte, ItemKey> LegendaryBookWeaponSlot;

	[SerializableGameDataField]
	public Dictionary<sbyte, long> LegendaryBookWeaponEffectId;

	[SerializableGameDataField]
	public Dictionary<sbyte, ShortList> LegendaryBookSkillSlot;

	[SerializableGameDataField]
	public Dictionary<sbyte, LongList> LegendaryBookSkillEffectId;

	[SerializableGameDataField]
	public SByteList LegendaryBookBonusCountYin;

	[SerializableGameDataField]
	public SByteList LegendaryBookBonusCountYang;

	public List<int> InteractedCharacterList;

	public List<int> FollowingNpcList;

	public TaiwuVillageStorage StockStorage
	{
		get
		{
			return _stockStorage ?? _stockStorageObsolete;
		}
		set
		{
			_stockStorage = value;
		}
	}

	public TaiwuVillageStorage CraftStorage
	{
		get
		{
			return _craftStorage ?? _craftStorageObsolete;
		}
		set
		{
			_craftStorage = value;
		}
	}

	public TaiwuVillageStorage MedicineStorage
	{
		get
		{
			return _medicineStorage ?? _medicineStorageObsolete;
		}
		set
		{
			_medicineStorage = value;
		}
	}

	public TaiwuVillageStorage FoodStorage
	{
		get
		{
			return _foodStorage ?? _foodStorageObsolete;
		}
		set
		{
			_foodStorage = value;
		}
	}

	public SettlementTreasury TaiwuSettlementTreasury
	{
		get
		{
			return _taiwuSettlementTreasury ?? _taiwuSettlementTreasuryObsolete;
		}
		set
		{
			_taiwuSettlementTreasury = value;
		}
	}

	public void PackWarehouseItem(ItemKey itemKey, int amount)
	{
		if (ItemTemplateHelper.IsInheritable(itemKey.ItemType, itemKey.TemplateId))
		{
			WarehouseItems.Add(itemKey, amount);
			DomainManager.Item.PackCrossArchiveItem(this, itemKey);
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 83;
		num += CrossArchiveTaiwuSerializer.GetSerializedSize(TaiwuChar);
		num = ((CombatSkills == null) ? (num + 2) : (num + (2 + 27 * CombatSkills.Count)));
		if (TaiwuEffects != null)
		{
			num += 2;
			int count = TaiwuEffects.Count;
			for (int i = 0; i < count; i++)
			{
				SpecialEffectWrapper specialEffectWrapper = TaiwuEffects[i];
				num = ((specialEffectWrapper == null) ? (num + 2) : (num + (2 + specialEffectWrapper.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ItemKey>(UnpackedItems);
		num = ((TaiwuVillageBlocks == null) ? (num + 2) : (num + (2 + 16 * TaiwuVillageBlocks.Count)));
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, Chicken>(Chicken);
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(WarehouseItems);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuCombatSkill>(TaiwuCombatSkills);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuLifeSkill>(TaiwuLifeSkills);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuCombatSkill>(NotLearnedCombatSkills);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuLifeSkill>(NotLearnedLifeSkills);
		if (CombatSkillPlans != null)
		{
			num += 2;
			int num2 = CombatSkillPlans.Length;
			for (int j = 0; j < num2; j++)
			{
				CombatSkillPlan combatSkillPlan = CombatSkillPlans[j];
				num = ((combatSkillPlan == null) ? (num + 2) : (num + (2 + combatSkillPlan.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num = ((CurrLifeSkillAttainmentPanelPlanIndex == null) ? (num + 2) : (num + (2 + CurrLifeSkillAttainmentPanelPlanIndex.Length)));
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakPlateObsolete>(SkillBreakPlateObsoleteDict);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakBonusCollection>(SkillBreakBonusDict);
		num = ((CombatSkillAttainmentPanelPlans == null) ? (num + 2) : (num + (2 + 2 * CombatSkillAttainmentPanelPlans.Length)));
		num = ((CurrCombatSkillAttainmentPanelPlanIds == null) ? (num + 2) : (num + (2 + CurrCombatSkillAttainmentPanelPlanIds.Length)));
		num = ((EquipmentsPlans == null) ? (num + 2) : (num + (2 + 99 * EquipmentsPlans.Length)));
		num = ((WeaponInnerRatios == null) ? (num + 2) : (num + (2 + WeaponInnerRatios.Length)));
		num += DictionaryOfCustomTypePair.GetSerializedSize<ItemKey, ReadingBookStrategies>(ReadingBooks);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ObsoleteProfessionData>(ObsoleteProfessions);
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(TreasuryItems);
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(TroughItems);
		num = ((ReadingEventBookIdList == null) ? (num + 2) : (num + (2 + 4 * ReadingEventBookIdList.Count)));
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ShortList>(ReadingEventReferenceBooks);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, IntPair>(ClearedSkillPlateStepInfo);
		num += CurrMasteredCombatSkillPlan.GetSerializedSize();
		if (MasteredCombatSkillPlans != null)
		{
			num += 2;
			int num3 = MasteredCombatSkillPlans.Length;
			for (int k = 0; k < num3; k++)
			{
				num += MasteredCombatSkillPlans[k].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		num += DictionaryOfBasicTypePair.GetSerializedSize<sbyte, sbyte>((IReadOnlyDictionary<sbyte, sbyte>)LegendaryBookBreakPlateCounts);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakPlateObsoleteList>(CombatSkillBreakPlateObsoleteList);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, IntList>(CombatSkillBreakPlateLastClearTimeList);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, IntList>(CombatSkillBreakPlateLastForceBreakoutStepsCount);
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, sbyte>((IReadOnlyDictionary<short, sbyte>)CombatSkillCurrBreakPlateIndex);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<sbyte, ItemKey>(LegendaryBookWeaponSlot);
		num += DictionaryOfBasicTypePair.GetSerializedSize<sbyte, long>((IReadOnlyDictionary<sbyte, long>)LegendaryBookWeaponEffectId);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<sbyte, ShortList>(LegendaryBookSkillSlot);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<sbyte, LongList>(LegendaryBookSkillEffectId);
		num += LegendaryBookBonusCountYin.GetSerializedSize();
		num += LegendaryBookBonusCountYang.GetSerializedSize();
		num = ((HandledOneShotEvents == null) ? (num + 2) : (num + (2 + 4 * HandledOneShotEvents.Count)));
		num = ((LegaciesBuildingTemplateIds == null) ? (num + 2) : (num + (2 + 2 * LegaciesBuildingTemplateIds.Count)));
		num = ((CollectionCrickets == null) ? (num + 2) : (num + (2 + 8 * CollectionCrickets.Length)));
		num = ((CollectionCricketRegen == null) ? (num + 2) : (num + (2 + 4 * CollectionCricketRegen.Length)));
		num = ((CollectionCricketJars == null) ? (num + 2) : (num + (2 + 8 * CollectionCricketJars.Length)));
		num = ((NormalInformation == null) ? (num + 2) : (num + (2 + NormalInformation.GetSerializedSize())));
		if (JiaoPools != null)
		{
			num += 2;
			int count2 = JiaoPools.Count;
			for (int l = 0; l < count2; l++)
			{
				JiaoPool jiaoPool = JiaoPools[l];
				num = ((jiaoPool == null) ? (num + 2) : (num + (2 + jiaoPool.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakBonusCollection>(SectEmeiSkillBreakBonus);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, ShortList>(SectEmeiBreakBonusTemplateIds);
		if (CricketCollectionDatas != null)
		{
			num += 2;
			int count3 = CricketCollectionDatas.Count;
			for (int m = 0; m < count3; m++)
			{
				CricketCollectionData cricketCollectionData = CricketCollectionDatas[m];
				num = ((cricketCollectionData == null) ? (num + 2) : (num + (2 + cricketCollectionData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, SByteList>(AvailableReadingStrategyMap);
		num += ExtraNeiliAllocationProgress.GetSerializedSize();
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SectEmeiBreakBonusData>(SectEmeiBonusData);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, IntList>(SectFulongOrgMemberChickens);
		num = ((_craftStorageObsolete == null) ? (num + 2) : (num + (2 + _craftStorageObsolete.GetSerializedSize())));
		num = ((_medicineStorageObsolete == null) ? (num + 2) : (num + (2 + _medicineStorageObsolete.GetSerializedSize())));
		num = ((_foodStorageObsolete == null) ? (num + 2) : (num + (2 + _foodStorageObsolete.GetSerializedSize())));
		num = ((_stockStorageObsolete == null) ? (num + 2) : (num + (2 + _stockStorageObsolete.GetSerializedSize())));
		num = ((_taiwuSettlementTreasuryObsolete == null) ? (num + 2) : (num + (2 + _taiwuSettlementTreasuryObsolete.GetSerializedSize())));
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ProfessionData>(Professions);
		num = ((ExternalEquippedCombatSkills == null) ? (num + 2) : (num + (2 + ExternalEquippedCombatSkills.GetSerializedSize())));
		num = ((SectZhujianGearMate == null) ? (num + 2) : (num + (2 + SectZhujianGearMate.GetSerializedSize())));
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakPlateList>(CombatSkillBreakPlateList);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakPlate>(SkillBreakPlateDict);
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, int>((IReadOnlyDictionary<short, int>)TaiwuCombatSkillProficiencies);
		num = ((XiangshuIdInKungfuPracticeRoom == null) ? (num + 2) : (num + (2 + XiangshuIdInKungfuPracticeRoom.Count)));
		num = ((_craftStorage == null) ? (num + 4) : (num + (4 + _craftStorage.GetSerializedSize())));
		num = ((_medicineStorage == null) ? (num + 4) : (num + (4 + _medicineStorage.GetSerializedSize())));
		num = ((_foodStorage == null) ? (num + 4) : (num + (4 + _foodStorage.GetSerializedSize())));
		num = ((_stockStorage == null) ? (num + 4) : (num + (4 + _stockStorage.GetSerializedSize())));
		num = ((_taiwuSettlementTreasury == null) ? (num + 4) : (num + (4 + _taiwuSettlementTreasury.GetSerializedSize())));
		if (TaiwuVillageBlocksEx != null)
		{
			num += 2;
			int count4 = TaiwuVillageBlocksEx.Count;
			for (int n = 0; n < count4; n++)
			{
				BuildingBlockDataEx buildingBlockDataEx = TaiwuVillageBlocksEx[n];
				num = ((buildingBlockDataEx == null) ? (num + 2) : (num + (2 + buildingBlockDataEx.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 85;
		ptr += 2;
		ptr += CrossArchiveTaiwuSerializer.Serialize(ptr, TaiwuChar);
		if (CombatSkills != null)
		{
			int count = CombatSkills.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += CombatSkills[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (TaiwuEffects != null)
		{
			int count2 = TaiwuEffects.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				SpecialEffectWrapper specialEffectWrapper = TaiwuEffects[j];
				if (specialEffectWrapper != null)
				{
					byte* ptr2 = ptr;
					ptr += 2;
					int num = specialEffectWrapper.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)ptr2 = (ushort)num;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, ItemKey>(ptr, ref UnpackedItems);
		ptr += TaiwuVillageLocation.Serialize(ptr);
		ptr += TaiwuVillageAreaData.Serialize(ptr);
		if (TaiwuVillageBlocks != null)
		{
			int count3 = TaiwuVillageBlocks.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				ptr += TaiwuVillageBlocks[k].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, Chicken>(ptr, ref Chicken);
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(ptr, ref WarehouseItems);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuCombatSkill>(ptr, ref TaiwuCombatSkills);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuLifeSkill>(ptr, ref TaiwuLifeSkills);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuCombatSkill>(ptr, ref NotLearnedCombatSkills);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuLifeSkill>(ptr, ref NotLearnedLifeSkills);
		if (CombatSkillPlans != null)
		{
			int num2 = CombatSkillPlans.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int l = 0; l < num2; l++)
			{
				CombatSkillPlan combatSkillPlan = CombatSkillPlans[l];
				if (combatSkillPlan != null)
				{
					byte* ptr3 = ptr;
					ptr += 2;
					int num3 = combatSkillPlan.Serialize(ptr);
					ptr += num3;
					Tester.Assert(num3 <= 65535);
					*(ushort*)ptr3 = (ushort)num3;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = CurrCombatSkillPlanId;
		ptr += 4;
		if (CurrLifeSkillAttainmentPanelPlanIndex != null)
		{
			int num4 = CurrLifeSkillAttainmentPanelPlanIndex.Length;
			Tester.Assert(num4 <= 65535);
			*(ushort*)ptr = (ushort)num4;
			ptr += 2;
			for (int m = 0; m < num4; m++)
			{
				ptr[m] = (byte)CurrLifeSkillAttainmentPanelPlanIndex[m];
			}
			ptr += num4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakPlateObsolete>(ptr, ref SkillBreakPlateObsoleteDict);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakBonusCollection>(ptr, ref SkillBreakBonusDict);
		if (CombatSkillAttainmentPanelPlans != null)
		{
			int num5 = CombatSkillAttainmentPanelPlans.Length;
			Tester.Assert(num5 <= 65535);
			*(ushort*)ptr = (ushort)num5;
			ptr += 2;
			for (int n = 0; n < num5; n++)
			{
				((short*)ptr)[n] = CombatSkillAttainmentPanelPlans[n];
			}
			ptr += 2 * num5;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CurrCombatSkillAttainmentPanelPlanIds != null)
		{
			int num6 = CurrCombatSkillAttainmentPanelPlanIds.Length;
			Tester.Assert(num6 <= 65535);
			*(ushort*)ptr = (ushort)num6;
			ptr += 2;
			for (int num7 = 0; num7 < num6; num7++)
			{
				ptr[num7] = (byte)CurrCombatSkillAttainmentPanelPlanIds[num7];
			}
			ptr += num6;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (EquipmentsPlans != null)
		{
			int num8 = EquipmentsPlans.Length;
			Tester.Assert(num8 <= 65535);
			*(ushort*)ptr = (ushort)num8;
			ptr += 2;
			for (int num9 = 0; num9 < num8; num9++)
			{
				ptr += EquipmentsPlans[num9].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = CurrEquipmentPlanId;
		ptr += 4;
		if (WeaponInnerRatios != null)
		{
			int num10 = WeaponInnerRatios.Length;
			Tester.Assert(num10 <= 65535);
			*(ushort*)ptr = (ushort)num10;
			ptr += 2;
			for (int num11 = 0; num11 < num10; num11++)
			{
				ptr[num11] = (byte)WeaponInnerRatios[num11];
			}
			ptr += num10;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)VoiceWeaponInnerRatio;
		ptr++;
		ptr += DictionaryOfCustomTypePair.Serialize<ItemKey, ReadingBookStrategies>(ptr, ref ReadingBooks);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, ObsoleteProfessionData>(ptr, ref ObsoleteProfessions);
		*(int*)ptr = CurrProfessionId;
		ptr += 4;
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(ptr, ref TreasuryItems);
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(ptr, ref TroughItems);
		if (ReadingEventBookIdList != null)
		{
			int count4 = ReadingEventBookIdList.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int num12 = 0; num12 < count4; num12++)
			{
				((int*)ptr)[num12] = ReadingEventBookIdList[num12];
			}
			ptr += 4 * count4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, ShortList>(ptr, ref ReadingEventReferenceBooks);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, IntPair>(ptr, ref ClearedSkillPlateStepInfo);
		ptr += TaiwuMaxNeiliAllocation.Serialize(ptr);
		int num13 = CurrMasteredCombatSkillPlan.Serialize(ptr);
		ptr += num13;
		Tester.Assert(num13 <= 65535);
		if (MasteredCombatSkillPlans != null)
		{
			int num14 = MasteredCombatSkillPlans.Length;
			Tester.Assert(num14 <= 65535);
			*(ushort*)ptr = (ushort)num14;
			ptr += 2;
			for (int num15 = 0; num15 < num14; num15++)
			{
				int num16 = MasteredCombatSkillPlans[num15].Serialize(ptr);
				ptr += num16;
				Tester.Assert(num16 <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = TaiwuExp;
		ptr += 4;
		ptr += TaiwuResources.Serialize(ptr);
		ptr += DictionaryOfBasicTypePair.Serialize<sbyte, sbyte>(ptr, ref LegendaryBookBreakPlateCounts);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakPlateObsoleteList>(ptr, ref CombatSkillBreakPlateObsoleteList);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, IntList>(ptr, ref CombatSkillBreakPlateLastClearTimeList);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, IntList>(ptr, ref CombatSkillBreakPlateLastForceBreakoutStepsCount);
		ptr += DictionaryOfBasicTypePair.Serialize<short, sbyte>(ptr, ref CombatSkillCurrBreakPlateIndex);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<sbyte, ItemKey>(ptr, ref LegendaryBookWeaponSlot);
		ptr += DictionaryOfBasicTypePair.Serialize<sbyte, long>(ptr, ref LegendaryBookWeaponEffectId);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<sbyte, ShortList>(ptr, ref LegendaryBookSkillSlot);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<sbyte, LongList>(ptr, ref LegendaryBookSkillEffectId);
		int num17 = LegendaryBookBonusCountYin.Serialize(ptr);
		ptr += num17;
		Tester.Assert(num17 <= 65535);
		int num18 = LegendaryBookBonusCountYang.Serialize(ptr);
		ptr += num18;
		Tester.Assert(num18 <= 65535);
		if (HandledOneShotEvents != null)
		{
			int count5 = HandledOneShotEvents.Count;
			Tester.Assert(count5 <= 65535);
			*(ushort*)ptr = (ushort)count5;
			ptr += 2;
			for (int num19 = 0; num19 < count5; num19++)
			{
				((int*)ptr)[num19] = HandledOneShotEvents[num19];
			}
			ptr += 4 * count5;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (LegaciesBuildingTemplateIds != null)
		{
			int count6 = LegaciesBuildingTemplateIds.Count;
			Tester.Assert(count6 <= 65535);
			*(ushort*)ptr = (ushort)count6;
			ptr += 2;
			for (int num20 = 0; num20 < count6; num20++)
			{
				((short*)ptr)[num20] = LegaciesBuildingTemplateIds[num20];
			}
			ptr += 2 * count6;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CollectionCrickets != null)
		{
			int num21 = CollectionCrickets.Length;
			Tester.Assert(num21 <= 65535);
			*(ushort*)ptr = (ushort)num21;
			ptr += 2;
			for (int num22 = 0; num22 < num21; num22++)
			{
				ptr += CollectionCrickets[num22].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CollectionCricketRegen != null)
		{
			int num23 = CollectionCricketRegen.Length;
			Tester.Assert(num23 <= 65535);
			*(ushort*)ptr = (ushort)num23;
			ptr += 2;
			for (int num24 = 0; num24 < num23; num24++)
			{
				((int*)ptr)[num24] = CollectionCricketRegen[num24];
			}
			ptr += 4 * num23;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CollectionCricketJars != null)
		{
			int num25 = CollectionCricketJars.Length;
			Tester.Assert(num25 <= 65535);
			*(ushort*)ptr = (ushort)num25;
			ptr += 2;
			for (int num26 = 0; num26 < num25; num26++)
			{
				ptr += CollectionCricketJars[num26].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = BuildingSpaceExtraAdd;
		ptr += 4;
		if (NormalInformation != null)
		{
			byte* ptr4 = ptr;
			ptr += 2;
			int num27 = NormalInformation.Serialize(ptr);
			ptr += num27;
			Tester.Assert(num27 <= 65535);
			*(ushort*)ptr4 = (ushort)num27;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (JiaoPools != null)
		{
			int count7 = JiaoPools.Count;
			Tester.Assert(count7 <= 65535);
			*(ushort*)ptr = (ushort)count7;
			ptr += 2;
			for (int num28 = 0; num28 < count7; num28++)
			{
				JiaoPool jiaoPool = JiaoPools[num28];
				if (jiaoPool != null)
				{
					byte* ptr5 = ptr;
					ptr += 2;
					int num29 = jiaoPool.Serialize(ptr);
					ptr += num29;
					Tester.Assert(num29 <= 65535);
					*(ushort*)ptr5 = (ushort)num29;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakBonusCollection>(ptr, ref SectEmeiSkillBreakBonus);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, ShortList>(ptr, ref SectEmeiBreakBonusTemplateIds);
		*(int*)ptr = MaxTaiwuVillageLevel;
		ptr += 4;
		*ptr = (IsJiaoPoolOpen ? ((byte)1) : ((byte)0));
		ptr++;
		if (CricketCollectionDatas != null)
		{
			int count8 = CricketCollectionDatas.Count;
			Tester.Assert(count8 <= 65535);
			*(ushort*)ptr = (ushort)count8;
			ptr += 2;
			for (int num30 = 0; num30 < count8; num30++)
			{
				CricketCollectionData cricketCollectionData = CricketCollectionDatas[num30];
				if (cricketCollectionData != null)
				{
					byte* ptr6 = ptr;
					ptr += 2;
					int num31 = cricketCollectionData.Serialize(ptr);
					ptr += num31;
					Tester.Assert(num31 <= 65535);
					*(ushort*)ptr6 = (ushort)num31;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = UnlockedCombatSkillPlanCount;
		ptr++;
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, SByteList>(ptr, ref AvailableReadingStrategyMap);
		int num32 = ExtraNeiliAllocationProgress.Serialize(ptr);
		ptr += num32;
		Tester.Assert(num32 <= 65535);
		ptr += ExtraNeiliAllocation.Serialize(ptr);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, SectEmeiBreakBonusData>(ptr, ref SectEmeiBonusData);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, IntList>(ptr, ref SectFulongOrgMemberChickens);
		if (_craftStorageObsolete != null)
		{
			byte* ptr7 = ptr;
			ptr += 2;
			int num33 = _craftStorageObsolete.Serialize(ptr);
			ptr += num33;
			Tester.Assert(num33 <= 65535);
			*(ushort*)ptr7 = (ushort)num33;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_medicineStorageObsolete != null)
		{
			byte* ptr8 = ptr;
			ptr += 2;
			int num34 = _medicineStorageObsolete.Serialize(ptr);
			ptr += num34;
			Tester.Assert(num34 <= 65535);
			*(ushort*)ptr8 = (ushort)num34;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_foodStorageObsolete != null)
		{
			byte* ptr9 = ptr;
			ptr += 2;
			int num35 = _foodStorageObsolete.Serialize(ptr);
			ptr += num35;
			Tester.Assert(num35 <= 65535);
			*(ushort*)ptr9 = (ushort)num35;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_stockStorageObsolete != null)
		{
			byte* ptr10 = ptr;
			ptr += 2;
			int num36 = _stockStorageObsolete.Serialize(ptr);
			ptr += num36;
			Tester.Assert(num36 <= 65535);
			*(ushort*)ptr10 = (ushort)num36;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_taiwuSettlementTreasuryObsolete != null)
		{
			byte* ptr11 = ptr;
			ptr += 2;
			int num37 = _taiwuSettlementTreasuryObsolete.Serialize(ptr);
			ptr += num37;
			Tester.Assert(num37 <= 65535);
			*(ushort*)ptr11 = (ushort)num37;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, ProfessionData>(ptr, ref Professions);
		if (ExternalEquippedCombatSkills != null)
		{
			byte* ptr12 = ptr;
			ptr += 2;
			int num38 = ExternalEquippedCombatSkills.Serialize(ptr);
			ptr += num38;
			Tester.Assert(num38 <= 65535);
			*(ushort*)ptr12 = (ushort)num38;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SectZhujianGearMate != null)
		{
			byte* ptr13 = ptr;
			ptr += 2;
			int num39 = SectZhujianGearMate.Serialize(ptr);
			ptr += num39;
			Tester.Assert(num39 <= 65535);
			*(ushort*)ptr13 = (ushort)num39;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakPlateList>(ptr, ref CombatSkillBreakPlateList);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakPlate>(ptr, ref SkillBreakPlateDict);
		ptr += DictionaryOfBasicTypePair.Serialize<short, int>(ptr, ref TaiwuCombatSkillProficiencies);
		if (XiangshuIdInKungfuPracticeRoom != null)
		{
			int count9 = XiangshuIdInKungfuPracticeRoom.Count;
			Tester.Assert(count9 <= 65535);
			*(ushort*)ptr = (ushort)count9;
			ptr += 2;
			for (int num40 = 0; num40 < count9; num40++)
			{
				ptr[num40] = (byte)XiangshuIdInKungfuPracticeRoom[num40];
			}
			ptr += count9;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_craftStorage != null)
		{
			byte* ptr14 = ptr;
			ptr += 4;
			int num41 = _craftStorage.Serialize(ptr);
			ptr += num41;
			Tester.Assert(num41 <= int.MaxValue);
			*(int*)ptr14 = num41;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (_medicineStorage != null)
		{
			byte* ptr15 = ptr;
			ptr += 4;
			int num42 = _medicineStorage.Serialize(ptr);
			ptr += num42;
			Tester.Assert(num42 <= int.MaxValue);
			*(int*)ptr15 = num42;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (_foodStorage != null)
		{
			byte* ptr16 = ptr;
			ptr += 4;
			int num43 = _foodStorage.Serialize(ptr);
			ptr += num43;
			Tester.Assert(num43 <= int.MaxValue);
			*(int*)ptr16 = num43;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (_stockStorage != null)
		{
			byte* ptr17 = ptr;
			ptr += 4;
			int num44 = _stockStorage.Serialize(ptr);
			ptr += num44;
			Tester.Assert(num44 <= int.MaxValue);
			*(int*)ptr17 = num44;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (_taiwuSettlementTreasury != null)
		{
			byte* ptr18 = ptr;
			ptr += 4;
			int num45 = _taiwuSettlementTreasury.Serialize(ptr);
			ptr += num45;
			Tester.Assert(num45 <= int.MaxValue);
			*(int*)ptr18 = num45;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (TaiwuVillageBlocksEx != null)
		{
			int count10 = TaiwuVillageBlocksEx.Count;
			Tester.Assert(count10 <= 65535);
			*(ushort*)ptr = (ushort)count10;
			ptr += 2;
			for (int num46 = 0; num46 < count10; num46++)
			{
				BuildingBlockDataEx buildingBlockDataEx = TaiwuVillageBlocksEx[num46];
				if (buildingBlockDataEx != null)
				{
					byte* ptr19 = ptr;
					ptr += 2;
					int num47 = buildingBlockDataEx.Serialize(ptr);
					ptr += num47;
					Tester.Assert(num47 <= 65535);
					*(ushort*)ptr19 = (ushort)num47;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num48 = (int)(ptr - pData);
		return (num48 <= 4) ? num48 : ((num48 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ptr += CrossArchiveTaiwuSerializer.Deserialize(ptr, ref TaiwuChar);
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (CombatSkills == null)
				{
					CombatSkills = new List<GameData.Domains.CombatSkill.CombatSkill>(num2);
				}
				else
				{
					CombatSkills.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					GameData.Domains.CombatSkill.CombatSkill combatSkill = new GameData.Domains.CombatSkill.CombatSkill();
					ptr += combatSkill.Deserialize(ptr);
					CombatSkills.Add(combatSkill);
				}
			}
			else
			{
				CombatSkills?.Clear();
			}
		}
		if (num > 2)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (TaiwuEffects == null)
				{
					TaiwuEffects = new List<SpecialEffectWrapper>(num3);
				}
				else
				{
					TaiwuEffects.Clear();
				}
				for (int j = 0; j < num3; j++)
				{
					ushort num4 = *(ushort*)ptr;
					ptr += 2;
					if (num4 > 0)
					{
						SpecialEffectWrapper specialEffectWrapper = new SpecialEffectWrapper();
						ptr += specialEffectWrapper.Deserialize(ptr);
						TaiwuEffects.Add(specialEffectWrapper);
					}
					else
					{
						TaiwuEffects.Add(null);
					}
				}
			}
			else
			{
				TaiwuEffects?.Clear();
			}
		}
		if (num > 3)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ItemKey>(ptr, ref UnpackedItems);
		}
		if (num > 4)
		{
			ptr += TaiwuVillageLocation.Deserialize(ptr);
		}
		if (num > 5)
		{
			if (TaiwuVillageAreaData == null)
			{
				TaiwuVillageAreaData = new BuildingAreaData();
			}
			ptr += TaiwuVillageAreaData.Deserialize(ptr);
		}
		if (num > 6)
		{
			ushort num5 = *(ushort*)ptr;
			ptr += 2;
			if (num5 > 0)
			{
				if (TaiwuVillageBlocks == null)
				{
					TaiwuVillageBlocks = new List<BuildingBlockData>(num5);
				}
				else
				{
					TaiwuVillageBlocks.Clear();
				}
				for (int k = 0; k < num5; k++)
				{
					BuildingBlockData buildingBlockData = new BuildingBlockData();
					ptr += buildingBlockData.Deserialize(ptr);
					TaiwuVillageBlocks.Add(buildingBlockData);
				}
			}
			else
			{
				TaiwuVillageBlocks?.Clear();
			}
		}
		if (num > 7)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, Chicken>(ptr, ref Chicken);
		}
		if (num > 8)
		{
			ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(ptr, ref WarehouseItems);
		}
		if (num > 9)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuCombatSkill>(ptr, ref TaiwuCombatSkills);
		}
		if (num > 10)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuLifeSkill>(ptr, ref TaiwuLifeSkills);
		}
		if (num > 11)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuCombatSkill>(ptr, ref NotLearnedCombatSkills);
		}
		if (num > 12)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuLifeSkill>(ptr, ref NotLearnedLifeSkills);
		}
		if (num > 13)
		{
			ushort num6 = *(ushort*)ptr;
			ptr += 2;
			if (num6 > 0)
			{
				if (CombatSkillPlans == null || CombatSkillPlans.Length != num6)
				{
					CombatSkillPlans = new CombatSkillPlan[num6];
				}
				for (int l = 0; l < num6; l++)
				{
					ushort num7 = *(ushort*)ptr;
					ptr += 2;
					if (num7 > 0)
					{
						CombatSkillPlan combatSkillPlan = CombatSkillPlans[l] ?? new CombatSkillPlan();
						ptr += combatSkillPlan.Deserialize(ptr);
						CombatSkillPlans[l] = combatSkillPlan;
					}
					else
					{
						CombatSkillPlans[l] = null;
					}
				}
			}
			else
			{
				CombatSkillPlans = null;
			}
		}
		if (num > 14)
		{
			CurrCombatSkillPlanId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 15)
		{
			ushort num8 = *(ushort*)ptr;
			ptr += 2;
			if (num8 > 0)
			{
				if (CurrLifeSkillAttainmentPanelPlanIndex == null || CurrLifeSkillAttainmentPanelPlanIndex.Length != num8)
				{
					CurrLifeSkillAttainmentPanelPlanIndex = new sbyte[num8];
				}
				for (int m = 0; m < num8; m++)
				{
					CurrLifeSkillAttainmentPanelPlanIndex[m] = (sbyte)ptr[m];
				}
				ptr += (int)num8;
			}
			else
			{
				CurrLifeSkillAttainmentPanelPlanIndex = null;
			}
		}
		if (num > 16)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakPlateObsolete>(ptr, ref SkillBreakPlateObsoleteDict);
		}
		if (num > 17)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakBonusCollection>(ptr, ref SkillBreakBonusDict);
		}
		if (num > 18)
		{
			ushort num9 = *(ushort*)ptr;
			ptr += 2;
			if (num9 > 0)
			{
				if (CombatSkillAttainmentPanelPlans == null || CombatSkillAttainmentPanelPlans.Length != num9)
				{
					CombatSkillAttainmentPanelPlans = new short[num9];
				}
				for (int n = 0; n < num9; n++)
				{
					CombatSkillAttainmentPanelPlans[n] = ((short*)ptr)[n];
				}
				ptr += 2 * num9;
			}
			else
			{
				CombatSkillAttainmentPanelPlans = null;
			}
		}
		if (num > 19)
		{
			ushort num10 = *(ushort*)ptr;
			ptr += 2;
			if (num10 > 0)
			{
				if (CurrCombatSkillAttainmentPanelPlanIds == null || CurrCombatSkillAttainmentPanelPlanIds.Length != num10)
				{
					CurrCombatSkillAttainmentPanelPlanIds = new sbyte[num10];
				}
				for (int num11 = 0; num11 < num10; num11++)
				{
					CurrCombatSkillAttainmentPanelPlanIds[num11] = (sbyte)ptr[num11];
				}
				ptr += (int)num10;
			}
			else
			{
				CurrCombatSkillAttainmentPanelPlanIds = null;
			}
		}
		if (num > 20)
		{
			ushort num12 = *(ushort*)ptr;
			ptr += 2;
			if (num12 > 0)
			{
				if (EquipmentsPlans == null || EquipmentsPlans.Length != num12)
				{
					EquipmentsPlans = new EquipmentPlan[num12];
				}
				for (int num13 = 0; num13 < num12; num13++)
				{
					EquipmentPlan equipmentPlan = EquipmentsPlans[num13] ?? new EquipmentPlan();
					ptr += equipmentPlan.Deserialize(ptr);
					EquipmentsPlans[num13] = equipmentPlan;
				}
			}
			else
			{
				EquipmentsPlans = null;
			}
		}
		if (num > 21)
		{
			CurrEquipmentPlanId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 22)
		{
			ushort num14 = *(ushort*)ptr;
			ptr += 2;
			if (num14 > 0)
			{
				if (WeaponInnerRatios == null || WeaponInnerRatios.Length != num14)
				{
					WeaponInnerRatios = new sbyte[num14];
				}
				for (int num15 = 0; num15 < num14; num15++)
				{
					WeaponInnerRatios[num15] = (sbyte)ptr[num15];
				}
				ptr += (int)num14;
			}
			else
			{
				WeaponInnerRatios = null;
			}
		}
		if (num > 23)
		{
			VoiceWeaponInnerRatio = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 24)
		{
			ptr += DictionaryOfCustomTypePair.Deserialize<ItemKey, ReadingBookStrategies>(ptr, ref ReadingBooks);
		}
		if (num > 25)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ObsoleteProfessionData>(ptr, ref ObsoleteProfessions);
		}
		if (num > 26)
		{
			CurrProfessionId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 27)
		{
			ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(ptr, ref TreasuryItems);
		}
		if (num > 28)
		{
			ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(ptr, ref TroughItems);
		}
		if (num > 29)
		{
			ushort num16 = *(ushort*)ptr;
			ptr += 2;
			if (num16 > 0)
			{
				if (ReadingEventBookIdList == null)
				{
					ReadingEventBookIdList = new List<int>(num16);
				}
				else
				{
					ReadingEventBookIdList.Clear();
				}
				for (int num17 = 0; num17 < num16; num17++)
				{
					ReadingEventBookIdList.Add(((int*)ptr)[num17]);
				}
				ptr += 4 * num16;
			}
			else
			{
				ReadingEventBookIdList?.Clear();
			}
		}
		if (num > 30)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ShortList>(ptr, ref ReadingEventReferenceBooks);
		}
		if (num > 31)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, IntPair>(ptr, ref ClearedSkillPlateStepInfo);
		}
		if (num > 32)
		{
			ptr += TaiwuMaxNeiliAllocation.Deserialize(ptr);
		}
		if (num > 33)
		{
			ptr += CurrMasteredCombatSkillPlan.Deserialize(ptr);
		}
		if (num > 34)
		{
			ushort num18 = *(ushort*)ptr;
			ptr += 2;
			if (num18 > 0)
			{
				if (MasteredCombatSkillPlans == null || MasteredCombatSkillPlans.Length != num18)
				{
					MasteredCombatSkillPlans = new ShortList[num18];
				}
				for (int num19 = 0; num19 < num18; num19++)
				{
					ShortList shortList = default(ShortList);
					ptr += shortList.Deserialize(ptr);
					MasteredCombatSkillPlans[num19] = shortList;
				}
			}
			else
			{
				MasteredCombatSkillPlans = null;
			}
		}
		if (num > 35)
		{
			TaiwuExp = *(int*)ptr;
			ptr += 4;
		}
		if (num > 36)
		{
			ptr += TaiwuResources.Deserialize(ptr);
		}
		if (num > 37)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<sbyte, sbyte>(ptr, ref LegendaryBookBreakPlateCounts);
		}
		if (num > 38)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakPlateObsoleteList>(ptr, ref CombatSkillBreakPlateObsoleteList);
		}
		if (num > 39)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, IntList>(ptr, ref CombatSkillBreakPlateLastClearTimeList);
		}
		if (num > 40)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, IntList>(ptr, ref CombatSkillBreakPlateLastForceBreakoutStepsCount);
		}
		if (num > 41)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<short, sbyte>(ptr, ref CombatSkillCurrBreakPlateIndex);
		}
		if (num > 42)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<sbyte, ItemKey>(ptr, ref LegendaryBookWeaponSlot);
		}
		if (num > 43)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<sbyte, long>(ptr, ref LegendaryBookWeaponEffectId);
		}
		if (num > 44)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<sbyte, ShortList>(ptr, ref LegendaryBookSkillSlot);
		}
		if (num > 45)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<sbyte, LongList>(ptr, ref LegendaryBookSkillEffectId);
		}
		if (num > 46)
		{
			ptr += LegendaryBookBonusCountYin.Deserialize(ptr);
		}
		if (num > 47)
		{
			ptr += LegendaryBookBonusCountYang.Deserialize(ptr);
		}
		if (num > 48)
		{
			ushort num20 = *(ushort*)ptr;
			ptr += 2;
			if (num20 > 0)
			{
				if (HandledOneShotEvents == null)
				{
					HandledOneShotEvents = new List<int>(num20);
				}
				else
				{
					HandledOneShotEvents.Clear();
				}
				for (int num21 = 0; num21 < num20; num21++)
				{
					HandledOneShotEvents.Add(((int*)ptr)[num21]);
				}
				ptr += 4 * num20;
			}
			else
			{
				HandledOneShotEvents?.Clear();
			}
		}
		if (num > 49)
		{
			ushort num22 = *(ushort*)ptr;
			ptr += 2;
			if (num22 > 0)
			{
				if (LegaciesBuildingTemplateIds == null)
				{
					LegaciesBuildingTemplateIds = new List<short>(num22);
				}
				else
				{
					LegaciesBuildingTemplateIds.Clear();
				}
				for (int num23 = 0; num23 < num22; num23++)
				{
					LegaciesBuildingTemplateIds.Add(((short*)ptr)[num23]);
				}
				ptr += 2 * num22;
			}
			else
			{
				LegaciesBuildingTemplateIds?.Clear();
			}
		}
		if (num > 50)
		{
			ushort num24 = *(ushort*)ptr;
			ptr += 2;
			if (num24 > 0)
			{
				if (CollectionCrickets == null || CollectionCrickets.Length != num24)
				{
					CollectionCrickets = new ItemKey[num24];
				}
				for (int num25 = 0; num25 < num24; num25++)
				{
					ItemKey itemKey = default(ItemKey);
					ptr += itemKey.Deserialize(ptr);
					CollectionCrickets[num25] = itemKey;
				}
			}
			else
			{
				CollectionCrickets = null;
			}
		}
		if (num > 51)
		{
			ushort num26 = *(ushort*)ptr;
			ptr += 2;
			if (num26 > 0)
			{
				if (CollectionCricketRegen == null || CollectionCricketRegen.Length != num26)
				{
					CollectionCricketRegen = new int[num26];
				}
				for (int num27 = 0; num27 < num26; num27++)
				{
					CollectionCricketRegen[num27] = ((int*)ptr)[num27];
				}
				ptr += 4 * num26;
			}
			else
			{
				CollectionCricketRegen = null;
			}
		}
		if (num > 52)
		{
			ushort num28 = *(ushort*)ptr;
			ptr += 2;
			if (num28 > 0)
			{
				if (CollectionCricketJars == null || CollectionCricketJars.Length != num28)
				{
					CollectionCricketJars = new ItemKey[num28];
				}
				for (int num29 = 0; num29 < num28; num29++)
				{
					ItemKey itemKey2 = default(ItemKey);
					ptr += itemKey2.Deserialize(ptr);
					CollectionCricketJars[num29] = itemKey2;
				}
			}
			else
			{
				CollectionCricketJars = null;
			}
		}
		if (num > 53)
		{
			BuildingSpaceExtraAdd = *(int*)ptr;
			ptr += 4;
		}
		if (num > 54)
		{
			ushort num30 = *(ushort*)ptr;
			ptr += 2;
			if (num30 > 0)
			{
				if (NormalInformation == null)
				{
					NormalInformation = new NormalInformationCollection();
				}
				ptr += NormalInformation.Deserialize(ptr);
			}
			else
			{
				NormalInformation = null;
			}
		}
		if (num > 55)
		{
			ushort num31 = *(ushort*)ptr;
			ptr += 2;
			if (num31 > 0)
			{
				if (JiaoPools == null)
				{
					JiaoPools = new List<JiaoPool>(num31);
				}
				else
				{
					JiaoPools.Clear();
				}
				for (int num32 = 0; num32 < num31; num32++)
				{
					ushort num33 = *(ushort*)ptr;
					ptr += 2;
					if (num33 > 0)
					{
						JiaoPool jiaoPool = new JiaoPool();
						ptr += jiaoPool.Deserialize(ptr);
						JiaoPools.Add(jiaoPool);
					}
					else
					{
						JiaoPools.Add(null);
					}
				}
			}
			else
			{
				JiaoPools?.Clear();
			}
		}
		if (num > 56)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakBonusCollection>(ptr, ref SectEmeiSkillBreakBonus);
		}
		if (num > 57)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, ShortList>(ptr, ref SectEmeiBreakBonusTemplateIds);
		}
		if (num > 58)
		{
			MaxTaiwuVillageLevel = *(int*)ptr;
			ptr += 4;
		}
		if (num > 59)
		{
			IsJiaoPoolOpen = *ptr != 0;
			ptr++;
		}
		if (num > 60)
		{
			ushort num34 = *(ushort*)ptr;
			ptr += 2;
			if (num34 > 0)
			{
				if (CricketCollectionDatas == null)
				{
					CricketCollectionDatas = new List<CricketCollectionData>(num34);
				}
				else
				{
					CricketCollectionDatas.Clear();
				}
				for (int num35 = 0; num35 < num34; num35++)
				{
					ushort num36 = *(ushort*)ptr;
					ptr += 2;
					if (num36 > 0)
					{
						CricketCollectionData cricketCollectionData = new CricketCollectionData();
						ptr += cricketCollectionData.Deserialize(ptr);
						CricketCollectionDatas.Add(cricketCollectionData);
					}
					else
					{
						CricketCollectionDatas.Add(null);
					}
				}
			}
			else
			{
				CricketCollectionDatas?.Clear();
			}
		}
		if (num > 61)
		{
			UnlockedCombatSkillPlanCount = *ptr;
			ptr++;
		}
		if (num > 62)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, SByteList>(ptr, ref AvailableReadingStrategyMap);
		}
		if (num > 63)
		{
			ptr += ExtraNeiliAllocationProgress.Deserialize(ptr);
		}
		if (num > 64)
		{
			ptr += ExtraNeiliAllocation.Deserialize(ptr);
		}
		if (num > 65)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SectEmeiBreakBonusData>(ptr, ref SectEmeiBonusData);
		}
		if (num > 66)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, IntList>(ptr, ref SectFulongOrgMemberChickens);
		}
		if (num > 67)
		{
			ushort num37 = *(ushort*)ptr;
			ptr += 2;
			if (num37 > 0)
			{
				if (_craftStorageObsolete == null)
				{
					_craftStorageObsolete = new TaiwuVillageStorage();
				}
				ptr += _craftStorageObsolete.Deserialize(ptr);
			}
			else
			{
				_craftStorageObsolete = null;
			}
		}
		if (num > 68)
		{
			ushort num38 = *(ushort*)ptr;
			ptr += 2;
			if (num38 > 0)
			{
				if (_medicineStorageObsolete == null)
				{
					_medicineStorageObsolete = new TaiwuVillageStorage();
				}
				ptr += _medicineStorageObsolete.Deserialize(ptr);
			}
			else
			{
				_medicineStorageObsolete = null;
			}
		}
		if (num > 69)
		{
			ushort num39 = *(ushort*)ptr;
			ptr += 2;
			if (num39 > 0)
			{
				if (_foodStorageObsolete == null)
				{
					_foodStorageObsolete = new TaiwuVillageStorage();
				}
				ptr += _foodStorageObsolete.Deserialize(ptr);
			}
			else
			{
				_foodStorageObsolete = null;
			}
		}
		if (num > 70)
		{
			ushort num40 = *(ushort*)ptr;
			ptr += 2;
			if (num40 > 0)
			{
				if (_stockStorageObsolete == null)
				{
					_stockStorageObsolete = new TaiwuVillageStorage();
				}
				ptr += _stockStorageObsolete.Deserialize(ptr);
			}
			else
			{
				_stockStorageObsolete = null;
			}
		}
		if (num > 71)
		{
			ushort num41 = *(ushort*)ptr;
			ptr += 2;
			if (num41 > 0)
			{
				if (_taiwuSettlementTreasuryObsolete == null)
				{
					_taiwuSettlementTreasuryObsolete = new SettlementTreasury();
				}
				ptr += _taiwuSettlementTreasuryObsolete.Deserialize(ptr);
			}
			else
			{
				_taiwuSettlementTreasuryObsolete = null;
			}
		}
		if (num > 72)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ProfessionData>(ptr, ref Professions);
		}
		if (num > 73)
		{
			ushort num42 = *(ushort*)ptr;
			ptr += 2;
			if (num42 > 0)
			{
				if (ExternalEquippedCombatSkills == null)
				{
					ExternalEquippedCombatSkills = new CombatSkillPlan();
				}
				ptr += ExternalEquippedCombatSkills.Deserialize(ptr);
			}
			else
			{
				ExternalEquippedCombatSkills = null;
			}
		}
		if (num > 74)
		{
			ushort num43 = *(ushort*)ptr;
			ptr += 2;
			if (num43 > 0)
			{
				if (SectZhujianGearMate == null)
				{
					SectZhujianGearMate = new GearMateDreamBackData();
				}
				ptr += SectZhujianGearMate.Deserialize(ptr);
			}
			else
			{
				SectZhujianGearMate = null;
			}
		}
		if (num > 75)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakPlateList>(ptr, ref CombatSkillBreakPlateList);
		}
		if (num > 76)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakPlate>(ptr, ref SkillBreakPlateDict);
		}
		if (num > 77)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<short, int>(ptr, ref TaiwuCombatSkillProficiencies);
		}
		if (num > 78)
		{
			ushort num44 = *(ushort*)ptr;
			ptr += 2;
			if (num44 > 0)
			{
				if (XiangshuIdInKungfuPracticeRoom == null)
				{
					XiangshuIdInKungfuPracticeRoom = new List<sbyte>(num44);
				}
				else
				{
					XiangshuIdInKungfuPracticeRoom.Clear();
				}
				for (int num45 = 0; num45 < num44; num45++)
				{
					XiangshuIdInKungfuPracticeRoom.Add((sbyte)ptr[num45]);
				}
				ptr += (int)num44;
			}
			else
			{
				XiangshuIdInKungfuPracticeRoom?.Clear();
			}
		}
		if (num > 79)
		{
			int num46 = *(int*)ptr;
			ptr += 4;
			if (num46 > 0)
			{
				if (_craftStorage == null)
				{
					_craftStorage = new TaiwuVillageStorage();
				}
				ptr += _craftStorage.Deserialize(ptr);
			}
			else
			{
				_craftStorage = null;
			}
		}
		if (num > 80)
		{
			int num47 = *(int*)ptr;
			ptr += 4;
			if (num47 > 0)
			{
				if (_medicineStorage == null)
				{
					_medicineStorage = new TaiwuVillageStorage();
				}
				ptr += _medicineStorage.Deserialize(ptr);
			}
			else
			{
				_medicineStorage = null;
			}
		}
		if (num > 81)
		{
			int num48 = *(int*)ptr;
			ptr += 4;
			if (num48 > 0)
			{
				if (_foodStorage == null)
				{
					_foodStorage = new TaiwuVillageStorage();
				}
				ptr += _foodStorage.Deserialize(ptr);
			}
			else
			{
				_foodStorage = null;
			}
		}
		if (num > 82)
		{
			int num49 = *(int*)ptr;
			ptr += 4;
			if (num49 > 0)
			{
				if (_stockStorage == null)
				{
					_stockStorage = new TaiwuVillageStorage();
				}
				ptr += _stockStorage.Deserialize(ptr);
			}
			else
			{
				_stockStorage = null;
			}
		}
		if (num > 83)
		{
			int num50 = *(int*)ptr;
			ptr += 4;
			if (num50 > 0)
			{
				if (_taiwuSettlementTreasury == null)
				{
					_taiwuSettlementTreasury = new SettlementTreasury();
				}
				ptr += _taiwuSettlementTreasury.Deserialize(ptr);
			}
			else
			{
				_taiwuSettlementTreasury = null;
			}
		}
		if (num > 84)
		{
			ushort num51 = *(ushort*)ptr;
			ptr += 2;
			if (num51 > 0)
			{
				if (TaiwuVillageBlocksEx == null)
				{
					TaiwuVillageBlocksEx = new List<BuildingBlockDataEx>(num51);
				}
				else
				{
					TaiwuVillageBlocksEx.Clear();
				}
				for (int num52 = 0; num52 < num51; num52++)
				{
					ushort num53 = *(ushort*)ptr;
					ptr += 2;
					if (num53 > 0)
					{
						BuildingBlockDataEx buildingBlockDataEx = new BuildingBlockDataEx();
						ptr += buildingBlockDataEx.Deserialize(ptr);
						TaiwuVillageBlocksEx.Add(buildingBlockDataEx);
					}
					else
					{
						TaiwuVillageBlocksEx.Add(null);
					}
				}
			}
			else
			{
				TaiwuVillageBlocksEx?.Clear();
			}
		}
		int num54 = (int)(ptr - pData);
		return (num54 <= 4) ? num54 : ((num54 + 3) / 4 * 4);
	}
}
