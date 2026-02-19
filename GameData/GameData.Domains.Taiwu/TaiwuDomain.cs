using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Config;
using Config.Common;
using Config.ConfigCells;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Common.SingleValueCollection;
using GameData.DLC;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Adventure;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.Relation;
using GameData.Domains.Character.SortFilter;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Information;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Item.Filters;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Merchant;
using GameData.Domains.Organization;
using GameData.Domains.Organization.Display;
using GameData.Domains.Organization.TaiwuVillageStoragesRecord;
using GameData.Domains.Taiwu.Debate;
using GameData.Domains.Taiwu.DebateAI;
using GameData.Domains.Taiwu.Display;
using GameData.Domains.Taiwu.LifeSkillCombat;
using GameData.Domains.Taiwu.LifeSkillCombat.Operation;
using GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Taiwu;

[GameDataDomain(5)]
public class TaiwuDomain : BaseGameDataDomain
{
	private struct LifeSkillStateModify
	{
		public int Index;

		public int ReadPageIndex;
	}

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private int _taiwuCharId;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private int _taiwuGenerationsCount;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private int _cricketLuckPoint;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private List<int> _previousTaiwuIds;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private bool _needToEscape;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<ItemDisplayData> _receivedItems;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<CharacterDisplayData> _receivedCharacters;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private bool _villagerLearnCombatSkillsFromSect;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private bool _villagerLearnLifeSkillsFromSect;

	private List<int> _prevMonthResourceCollection;

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<ItemKey, int> _warehouseItems;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private int _warehouseMaxLoad;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private int _warehouseCurrLoad;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private int _troughCurrLoad;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private int _troughMaxLoad;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private int _buildingSpaceLimit;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private int _buildingSpaceCurr;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private int _buildingSpaceExtraAdd;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private bool _prosperousConstruction;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private ResourceInts _totalResources;

	private static readonly ItemSourceType[] TaiwuVillageStorages = new ItemSourceType[5]
	{
		ItemSourceType.Warehouse,
		ItemSourceType.Trough,
		ItemSourceType.StockStorageWarehouse,
		ItemSourceType.StockStorageGoodsShelf,
		ItemSourceType.Treasury
	};

	public const sbyte TeachTaiwuMaxLifeSkill = 3;

	public const sbyte TeachTaiwuMaxCombatSkill = 3;

	public const sbyte ThinkPageVisibleRange = 3;

	public const int SkillAttainmentPanelPlanCount = 5;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private readonly Dictionary<short, TaiwuCombatSkill> _combatSkills;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private readonly Dictionary<short, TaiwuLifeSkill> _lifeSkills;

	[DomainData(DomainDataType.ElementList, true, false, false, false, ArrayElementsCount = 9)]
	private readonly CombatSkillPlan[] _combatSkillPlans;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private int _currCombatSkillPlanId;

	[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 16)]
	private readonly sbyte[] _currLifeSkillAttainmentPanelPlanIndex;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
	private readonly Dictionary<short, SkillBreakPlateObsolete> _skillBreakPlateObsoleteDict;

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<short, SkillBreakBonusCollection> _skillBreakBonusDict;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private readonly Dictionary<int, GameData.Utilities.ShortList> _teachTaiwuLifeSkillDict;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private readonly Dictionary<int, GameData.Utilities.ShortList> _teachTaiwuCombatSkillDict;

	[DomainData(DomainDataType.SingleValue, true, false, true, false, ArrayElementsCount = 630)]
	private short[] _combatSkillAttainmentPanelPlans;

	[DomainData(DomainDataType.SingleValue, true, false, true, false, ArrayElementsCount = 14)]
	private sbyte[] _currCombatSkillAttainmentPanelPlanIds;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private bool _canBreakOut;

	private GameData.Domains.Character.Character _taiwuChar;

	private CivilianSettlement _taiwuVillage;

	private bool _canTaiwuBeSneakyHarmfulActionTarget;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private int _moveTimeCostPercent;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private List<IntPair> _overweightSanctionPercent;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private List<IntPair> _clothingDurability;

	[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 6)]
	private sbyte[] _weaponInnerRatios;

	[DomainData(DomainDataType.SingleValue, false, true, true, true, ArrayElementsCount = 7)]
	private sbyte[] _weaponCurrInnerRatios;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
	private readonly Dictionary<int, short> _appointments;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private short _babyBonusMainAttributes;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private short _babyBonusLifeSkillQualifications;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private short _babyBonusCombatSkillQualifications;

	public bool JieqingPunishmentAssassinAlreadyAdd;

	public bool JieqingHuntTaiwu;

	private HashSet<int> _debateVisitedPawns = new HashSet<int>();

	private HashSet<int> _debateNpcSpectators = new HashSet<int>();

	private Dictionary<int, int> _debateSpectatorCooldownMap = new Dictionary<int, int>();

	private int _debateCardUsedCount;

	private (int taiwu, int npc) _debateBeatPawnCount;

	private (int taiwu, int npc) _debateRevealedPawnCount;

	private int _pawnId;

	private int _nodeEffectId;

	private int _activatedStrategyId;

	private int _debateNpcId = -1;

	private sbyte _debateLifeSkillType = -1;

	private DebateAi _debateAiTaiwu;

	private DebateAi _debateAiNpc;

	private GameData.Domains.Character.Character _debateNpc;

	private bool _forceAiBribery = false;

	private Dictionary<int, Action<DebateStrategyActionParams>> _debateStrategyActions;

	private readonly List<Action> _oneTimeStrategyActionList = new List<Action>();

	private const int EquipmentPlansCount = 5;

	[DomainData(DomainDataType.ElementList, true, false, false, false, ArrayElementsCount = 5)]
	private readonly EquipmentPlan[] _equipmentsPlans;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private int _currEquipmentPlanId;

	public const sbyte TaiwuGroupBaseMaxCount = 10;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private CharacterSet _groupCharIds;

	[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 3)]
	private readonly int[] _combatGroupCharIds;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private int _taiwuGroupMaxCount;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private Injuries _taiwuGroupWorstInjuries;

	[DomainData(DomainDataType.SingleValue, false, true, true, false)]
	private List<int> _taiwuSpecialGroup;

	[DomainData(DomainDataType.SingleValue, false, true, true, false)]
	private List<int> _taiwuGearMateGroup;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
	private Dictionary<short, short> _legacyPointDict;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private int _legacyPoint;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private List<short> _availableLegacyList;

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private sbyte _legacyPassingState;

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private List<int> _successorCandidates;

	[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 15)]
	private readonly SByteList[] _stateNewCharacterLegacyGrowingGrades;

	private bool _isTaiwuDying;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _isTaiwuDieOfCombatWithXiangshu;

	private readonly List<short> _selectedLegacies = new List<short>();

	private static List<(short, short)>[][] _categorizedNormalLegacyConfigs;

	private Match _lifeSkillCombat;

	private ItemKey _lockedItemKey = ItemKey.Invalid;

	private readonly Dictionary<OperationPrepareForceAdversary.ForceAdversaryOperation, bool> _nextLifeSkillCombatForceOperationForbid = new Dictionary<OperationPrepareForceAdversary.ForceAdversaryOperation, bool>();

	public const int MaxReferenceSkillCount = 3;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private byte _referenceSkillSlotUnlockStates;

	private static bool _autoAllocateNeiliToMax;

	private int _currentLoopingEventCostedConcentration = 0;

	private static readonly sbyte[] UseDrawConditionCanObtainNeiliIdList = new sbyte[6] { 0, 3, 6, 9, 12, 15 };

	private static readonly sbyte[] UseDrawConditionNeiliAllocationLessThanIdList = new sbyte[6] { 2, 5, 8, 11, 14, 17 };

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private readonly Dictionary<short, TaiwuCombatSkill> _notLearnCombatSkillReadingProgress;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private readonly Dictionary<short, TaiwuLifeSkill> _notLearnLifeSkillReadingProgress;

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<ItemKey, ReadingBookStrategies> _readingBooks;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private ItemKey _curReadingBook;

	[DomainData(DomainDataType.SingleValue, true, false, true, false, ArrayElementsCount = 3)]
	private ItemKey[] _referenceBooks;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private byte _referenceBookSlotUnlockStates;

	[Obsolete("Use _readingEventBookIdList Instead")]
	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private bool _readingEventTriggered;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private sbyte _readInCombatCount;

	private int _currentReadingEventCostedIntelligence;

	private readonly List<short> _unlockedDebateStrategyList = new List<short>();

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _healingOuterInjuryRestriction;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _healingInnerInjuryRestriction;

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private byte _neiliAllocationTypeRestriction;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private List<short> _visitedSettlements;

	private const short WarehouseBaseMaxLoad = 5000;

	private const short ProsperousConstructionSpace = 20;

	public const sbyte ResourceRecoverSpeedMultiplierWithWorker = 3;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private short _taiwuVillageSettlementId;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
	private readonly Dictionary<int, VillagerWorkData> _villagerWork;

	[DomainData(DomainDataType.SingleValueCollection, false, false, true, false)]
	private readonly HashSetAsDictionary<Location> _villagerWorkLocations;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private int _materialResourceMaxCount;

	[DomainData(DomainDataType.SingleValue, false, true, true, true, ArrayElementsCount = 8)]
	private int[] _resourceChange;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private short _workLocationMaxCount;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private short _totalVillagerCount;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private short _totalAdultVillagerCount;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private short _availableVillagerCount;

	private readonly List<int> _moveToWorkLocationCharList = new List<int>();

	public readonly Dictionary<sbyte, List<Location>> ResourceRecoverSpeedUpDict = new Dictionary<sbyte, List<Location>>();

	private bool _needCommitTaiwuSettlementTreasury;

	private readonly bool[] _preAdvanceVillagerRoleFarmerFixedActionSuccessArray = new bool[1];

	private readonly bool[] _preAdvanceVillagerRoleCraftsmanFixedActionSuccessArray = new bool[4];

	private readonly bool[] _preAdvanceVillagerRoleDoctorFixedActionSuccessArray = new bool[3];

	private readonly bool[] _periAdvanceVillagerRoleFarmerFixedActionSuccessArray = new bool[1];

	private readonly bool[] _periAdvanceVillagerRoleCraftsmanFixedActionSuccessArray = new bool[4];

	private readonly bool[] _periAdvanceVillagerRoleDoctorFixedActionSuccessArray = new bool[3];

	private readonly float _gainItemActionRate = 0.5f;

	private readonly Dictionary<short, HashSet<sbyte>> _villagerRoleNeedMaterials = new Dictionary<short, HashSet<sbyte>>
	{
		{ -1, null },
		{
			0,
			new HashSet<sbyte> { 0 }
		},
		{
			1,
			new HashSet<sbyte> { 1, 3, 2, 4 }
		},
		{
			2,
			new HashSet<sbyte> { 5 }
		},
		{
			3,
			new HashSet<sbyte> { 3, 4, 0 }
		},
		{
			4,
			new HashSet<sbyte> { 3, 4 }
		},
		{
			5,
			new HashSet<sbyte> { 1, 3, 2, 4 }
		},
		{ 6, null }
	};

	public static readonly Dictionary<short, HashSet<sbyte>> VillagerRoleNeedLifeSkillBooks = new Dictionary<short, HashSet<sbyte>>
	{
		{ -1, null },
		{
			0,
			new HashSet<sbyte> { 14, 13 }
		},
		{
			1,
			new HashSet<sbyte> { 7, 6, 11, 14 }
		},
		{
			2,
			new HashSet<sbyte> { 8, 9, 12 }
		},
		{
			3,
			new HashSet<sbyte> { 5, 15 }
		},
		{
			4,
			new HashSet<sbyte> { 0, 1, 2, 3, 4, 5 }
		},
		{
			5,
			new HashSet<sbyte> { 1, 3, 2, 4 }
		},
		{ 6, null }
	};

	private readonly List<int>[] _villagerListClassArray = new List<int>[9];

	private readonly Dictionary<int, sbyte> _villagerClassDict = new Dictionary<int, sbyte>(9);

	private readonly Dictionary<int, List<GameData.Domains.Character.Ai.PersonalNeed>> _timeMeetTreasuryNeedDict = new Dictionary<int, List<GameData.Domains.Character.Ai.PersonalNeed>>(128);

	private readonly Dictionary<ItemKey, int> _villagerItems = new Dictionary<ItemKey, int>(512);

	private readonly HashSet<sbyte> _villagerGainedItemTypeSet = new HashSet<sbyte>();

	private readonly Dictionary<short, List<(ItemKey itemKey, int score)>> _villagerSelectableItemKeys = new Dictionary<short, List<(ItemKey, int)>>(512);

	private readonly List<(ItemKey weapon, int score)> _availableWeapons = new List<(ItemKey, int)>(128);

	private readonly List<(short itemTemplateId, short count)> _suitableWeapons = new List<(short, short)>(128);

	private readonly HashSet<short> _fixedBestWeapons = new HashSet<short>(128);

	private readonly Dictionary<ItemKey, Dictionary<int, sbyte>> _treasuryNeededItemDict = new Dictionary<ItemKey, Dictionary<int, sbyte>>(128);

	private readonly List<(GameData.Domains.Item.Medicine item, int amount)>[] _categorizedMedicines = new List<(GameData.Domains.Item.Medicine, int)>[7]
	{
		new List<(GameData.Domains.Item.Medicine, int)>(64),
		new List<(GameData.Domains.Item.Medicine, int)>(64),
		new List<(GameData.Domains.Item.Medicine, int)>(64),
		new List<(GameData.Domains.Item.Medicine, int)>(64),
		new List<(GameData.Domains.Item.Medicine, int)>(64),
		new List<(GameData.Domains.Item.Medicine, int)>(64),
		new List<(GameData.Domains.Item.Medicine, int)>(64)
	};

	private readonly List<(GameData.Domains.Item.Misc item, int amount)> _itemsForNeili = new List<(GameData.Domains.Item.Misc, int)>(64);

	private readonly List<(GameData.Domains.Item.Food item, int amount)>[] _foodsForMainAttributes = new List<(GameData.Domains.Item.Food, int)>[6]
	{
		new List<(GameData.Domains.Item.Food, int)>(64),
		new List<(GameData.Domains.Item.Food, int)>(64),
		new List<(GameData.Domains.Item.Food, int)>(64),
		new List<(GameData.Domains.Item.Food, int)>(64),
		new List<(GameData.Domains.Item.Food, int)>(64),
		new List<(GameData.Domains.Item.Food, int)>(64)
	};

	private readonly List<(GameData.Domains.Item.TeaWine item, int amount)> _teaWinesForHappiness = new List<(GameData.Domains.Item.TeaWine, int)>(64);

	private readonly ISet<int> _villagerFeatureHolderSet = new HashSet<int>();

	private static short[] _gradeVillagerRoles;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[77][];

	private SpinLock _spinLockWarehouseMaxLoad = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockWarehouseCurrLoad = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockBuildingSpaceLimit = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockBuildingSpaceCurr = new SpinLock(enableThreadOwnerTracking: false);

	private SingleValueCollectionModificationCollection<short> _modificationsCombatSkills = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsLifeSkills = SingleValueCollectionModificationCollection<short>.Create();

	private static readonly DataInfluence[][] CacheInfluencesCombatSkillPlans = new DataInfluence[9][];

	private readonly byte[] _dataStatesCombatSkillPlans = new byte[3];

	private static readonly DataInfluence[][] CacheInfluencesCurrLifeSkillAttainmentPanelPlanIndex = new DataInfluence[16][];

	private readonly byte[] _dataStatesCurrLifeSkillAttainmentPanelPlanIndex = new byte[4];

	private SingleValueCollectionModificationCollection<short> _modificationsSkillBreakPlateObsoleteDict = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<int> _modificationsTeachTaiwuLifeSkillDict = SingleValueCollectionModificationCollection<int>.Create();

	private SingleValueCollectionModificationCollection<int> _modificationsTeachTaiwuCombatSkillDict = SingleValueCollectionModificationCollection<int>.Create();

	private SpinLock _spinLockMoveTimeCostPercent = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockWeaponCurrInnerRatios = new SpinLock(enableThreadOwnerTracking: false);

	private SingleValueCollectionModificationCollection<int> _modificationsAppointments = SingleValueCollectionModificationCollection<int>.Create();

	private static readonly DataInfluence[][] CacheInfluencesEquipmentsPlans = new DataInfluence[5][];

	private readonly byte[] _dataStatesEquipmentsPlans = new byte[2];

	private static readonly DataInfluence[][] CacheInfluencesCombatGroupCharIds = new DataInfluence[3][];

	private readonly byte[] _dataStatesCombatGroupCharIds = new byte[1];

	private SpinLock _spinLockTaiwuGroupMaxCount = new SpinLock(enableThreadOwnerTracking: false);

	private SingleValueCollectionModificationCollection<short> _modificationsLegacyPointDict = SingleValueCollectionModificationCollection<short>.Create();

	private static readonly DataInfluence[][] CacheInfluencesStateNewCharacterLegacyGrowingGrades = new DataInfluence[15][];

	private readonly byte[] _dataStatesStateNewCharacterLegacyGrowingGrades = new byte[4];

	private SingleValueCollectionModificationCollection<short> _modificationsNotLearnCombatSkillReadingProgress = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsNotLearnLifeSkillReadingProgress = SingleValueCollectionModificationCollection<short>.Create();

	private SpinLock _spinLockReferenceBookSlotUnlockStates = new SpinLock(enableThreadOwnerTracking: false);

	private SingleValueCollectionModificationCollection<int> _modificationsVillagerWork = SingleValueCollectionModificationCollection<int>.Create();

	private SingleValueCollectionModificationCollection<Location> _modificationsVillagerWorkLocations = SingleValueCollectionModificationCollection<Location>.Create();

	private SpinLock _spinLockMaterialResourceMaxCount = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockResourceChange = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockWorkLocationMaxCount = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockTotalVillagerCount = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockTotalAdultVillagerCount = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockAvailableVillagerCount = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockVillagerLearnLifeSkillsFromSect = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockVillagerLearnCombatSkillsFromSect = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockOverweightSanctionPercent = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockReferenceSkillSlotUnlockStates = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockTaiwuGroupWorstInjuries = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockTotalResources = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockTaiwuSpecialGroup = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockTaiwuGearMateGroup = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockCanBreakOut = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockTroughMaxLoad = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockTroughCurrLoad = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockClothingDurability = new SpinLock(enableThreadOwnerTracking: false);

	private Queue<uint> _pendingLoadingOperationIds;

	public Dictionary<ItemKey, int> WarehouseItems => _warehouseItems;

	public CivilianSettlement TaiwuVillage
	{
		get
		{
			if (_taiwuVillage == null)
			{
				_taiwuVillage = DomainManager.Organization.GetElement_CivilianSettlements(_taiwuVillageSettlementId);
			}
			return _taiwuVillage;
		}
	}

	public DebateGame Debate { get; private set; }

	public int CurrentReadingEventCostedIntelligence
	{
		get
		{
			return _currentReadingEventCostedIntelligence;
		}
		set
		{
			_currentReadingEventCostedIntelligence = value;
		}
	}

	private void OnInitializedDomainData()
	{
		for (int i = 0; i < 5; i++)
		{
			_equipmentsPlans[i] = new EquipmentPlan();
		}
		for (int j = 0; j < _combatGroupCharIds.Length; j++)
		{
			_combatGroupCharIds[j] = -1;
		}
	}

	private void InitializeOnInitializeGameDataModule()
	{
		InitializeNormalLegacies();
		InitializeGradeRoles();
	}

	private unsafe void InitializeOnEnterNewWorld()
	{
		_taiwuCharId = -1;
		_taiwuGenerationsCount = 1;
		_legacyPoint = 0;
		_cricketLuckPoint = 100;
		_legacyPassingState = 0;
		_taiwuVillageSettlementId = -1;
		_taiwuChar = null;
		_prosperousConstruction = false;
		_currEquipmentPlanId = 0;
		_curReadingBook = ItemKey.Invalid;
		for (int i = 0; i < _referenceBooks.Length; i++)
		{
			_referenceBooks[i] = ItemKey.Invalid;
		}
		_referenceBookSlotUnlockStates = 1;
		_referenceSkillSlotUnlockStates = 1;
		for (int j = 0; j < 9; j++)
		{
			_combatSkillPlans[j] = new CombatSkillPlan();
		}
		_currCombatSkillPlanId = 0;
		fixed (short* combatSkillAttainmentPanelPlans = _combatSkillAttainmentPanelPlans)
		{
			CollectionUtils.SetMemoryToMinusOne((byte*)combatSkillAttainmentPanelPlans, _combatSkillAttainmentPanelPlans.Length * 2);
		}
		Events.RegisterHandler_AdvanceMonthBegin(OnAdvanceMonthBegin);
		RegisterUpdateMusicBonus();
	}

	private void OnLoadedArchiveData()
	{
		_taiwuChar = DomainManager.Character.GetElement_Objects(_taiwuCharId);
		InitializeVillagerWorkLocations();
		Events.RegisterHandler_AdvanceMonthBegin(OnAdvanceMonthBegin);
		RegisterUpdateMusicBonus();
		ReCalcVillagerFeatureHolderSet();
	}

	public override void FixAbnormalDomainArchiveData(DataContext context)
	{
		FixAbnormalTaiwuData(context);
		FixAbnormalGroupData(context);
		FixNotLearnedSkills(context);
		FixHiddenEquipSkills(context);
		FixInvalidSkills(context);
		FixBreakGridBonus(context);
		FixWarehouse(context);
		FixInventory(context);
		FixFiveLoongDlcClothing(context);
		FixItemsPoisonEffects(context);
		FixVillageWorkData(context);
		FixUnlockedWorkingVillagersData(context);
		FixVillageIdentityInfo(context);
		FixAbnormalEquipmentPlan(context);
		FixSkillBookReadingProgress(context);
		FixSkillBookReadingState(context);
		FixAvailableReadingStrategies(context);
		FixTaiwuReadingStrategyExpireTime(context);
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 70))
		{
			RecordAllOwnedClothing(context);
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 74, 40))
		{
			FixTaiwuObsoleteBuildingCore(context);
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 74, 38))
		{
			FixTaiwuStockStorageGoodsShelf(context);
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 76, 22))
		{
			FixObsoletePoisonImmunities(context);
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 74))
		{
			DomainManager.Organization.ForceUpdateTaiwuVillager(context);
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 79, 12))
		{
			UpdateConsummateLevelBrokenFeature(context);
		}
	}

	public void RecordAllOwnedClothing(DataContext context)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Inventory inventory = taiwu.GetInventory();
		foreach (ItemKey key in inventory.Items.Keys)
		{
			if (key.ItemType == 3)
			{
				DomainManager.Extra.RecordOwnedClothing(context, key.TemplateId);
			}
		}
		ItemKey[] equipment = taiwu.GetEquipment();
		if (equipment[4].IsValid())
		{
			DomainManager.Extra.RecordOwnedClothing(context, equipment[4].TemplateId);
		}
		foreach (ItemKey key2 in _warehouseItems.Keys)
		{
			if (key2.ItemType == 3)
			{
				DomainManager.Extra.RecordOwnedClothing(context, key2.TemplateId);
			}
		}
	}

	private void RegisterUpdateMusicBonus()
	{
		DataUid uid = new DataUid(19, 125, ulong.MaxValue);
		DataUid uid2 = new DataUid(19, 126, ulong.MaxValue);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(uid, "UpdateMusicBonus", UpdateMusicBonus);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(uid2, "UpdateMusicBonus", UpdateMusicBonus);
	}

	private void UpdateMusicBonus(DataContext context, DataUid uid)
	{
		short sectXuannvPlayerMusicId = DomainManager.Extra.GetSectXuannvPlayerMusicId();
		bool sectXuannvPlayerIsEnabled = DomainManager.Extra.GetSectXuannvPlayerIsEnabled();
		if (sectXuannvPlayerMusicId < 0 || !sectXuannvPlayerIsEnabled)
		{
			HashSet<int> collection = _groupCharIds.GetCollection();
			foreach (int item in collection)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.RemoveFeatureGroup(context, 416);
			}
			{
				foreach (int item2 in _taiwuSpecialGroup)
				{
					GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2);
					element_Objects2.RemoveFeatureGroup(context, 416);
				}
				return;
			}
		}
		short temporaryFeature = Music.Instance[sectXuannvPlayerMusicId].TemporaryFeature;
		HashSet<int> collection2 = _groupCharIds.GetCollection();
		foreach (int item3 in collection2)
		{
			GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(item3);
			element_Objects3.AddFeature(context, temporaryFeature, removeMutexFeature: true);
		}
		foreach (int item4 in _taiwuSpecialGroup)
		{
			GameData.Domains.Character.Character element_Objects4 = DomainManager.Character.GetElement_Objects(item4);
			element_Objects4.AddFeature(context, temporaryFeature, removeMutexFeature: true);
		}
	}

	private void OnAdvanceMonthBegin(DataContext context)
	{
		_prevMonthResourceCollection = new List<int>();
		for (sbyte b = 0; b < 6; b++)
		{
			int resource = _taiwuChar.GetResource(b);
			_prevMonthResourceCollection.Add(resource);
		}
		_canTaiwuBeSneakyHarmfulActionTarget = !DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(28);
	}

	private void FixVillageIdentityInfo(DataContext context)
	{
		HashSet<int> members = DomainManager.Organization.GetSettlement(_taiwuVillageSettlementId).GetMembers().GetMembers(0);
		List<int> list = new List<int>();
		foreach (int item in members)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			OrganizationInfo organizationInfo = element_Objects.GetOrganizationInfo();
			if (organizationInfo.Grade == 0 && !organizationInfo.Principal)
			{
				organizationInfo.Principal = true;
				element_Objects.SetOrganizationInfo(organizationInfo, context);
				Logger.Warn($"Fixing character {element_Objects}'s wrong principal value when join taiwu by stone house");
			}
			if (organizationInfo.OrgTemplateId == 20)
			{
				list.Add(item);
			}
		}
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			int objectId = list[i];
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(objectId);
			OrganizationInfo organizationInfo2 = element_Objects2.GetOrganizationInfo();
			organizationInfo2.OrgTemplateId = 16;
			organizationInfo2.Grade = 0;
			organizationInfo2.SettlementId = _taiwuVillageSettlementId;
			element_Objects2.SetOrganizationInfo(organizationInfo2, context);
			Events.RaiseXiangshuInfectionFeatureChanged(context, element_Objects2, 218);
			Logger.Warn($"Fixing character {element_Objects2}'s wrong settlement id as taiwu villiager");
		}
	}

	private void FixVillageWorkData(DataContext context)
	{
		Location taiwuVillageLocation = GetTaiwuVillageLocation();
		foreach (KeyValuePair<int, VillagerWorkData> item in _villagerWork)
		{
			VillagerWorkData value = item.Value;
			if (VillagerWorkType.IsWorkOnMap(value.WorkType))
			{
				Location location = new Location(value.AreaId, value.BlockId);
				DomainManager.Extra.AddLocationMark(context, location);
			}
			if (item.Value.WorkType < 10 && item.Value.AreaId == taiwuVillageLocation.AreaId && item.Value.BlockId == taiwuVillageLocation.BlockId)
			{
				BuildingBlockKey elementId = new BuildingBlockKey(item.Value.AreaId, item.Value.BlockId, item.Value.BuildingBlockIndex);
				BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId);
				if (element_BuildingBlocks.TemplateId == 0)
				{
					Logger.Warn($"Fixing character {item.Key}'s wrong  villager work data");
					RemoveVillagerWork(context, item.Key);
				}
			}
		}
		ExtraDomain extra = DomainManager.Extra;
		CharacterDomain character = DomainManager.Character;
		VillagerRoleRecords villagerRoleRecordsData = extra.GetVillagerRoleRecordsData();
		HashSet<int> hashSet = new HashSet<int>(extra.GetVillagerRoleCharacters());
		foreach (int charId in hashSet)
		{
			VillagerRoleBase villagerRoleBase = null;
			if (!villagerRoleRecordsData.History.Any((VillagerRoleRecordElement r) => r.CharacterId == charId) && character.TryGetElement_Objects(charId, out var element) && (villagerRoleBase = extra.GetVillagerRole(charId)) != null)
			{
				VillagerRoleRecordElement villagerRoleRecordElement = new VillagerRoleRecordElement
				{
					CharacterId = charId,
					CharacterTemplateId = element.GetTemplateId(),
					RoleTemplateId = villagerRoleBase.RoleTemplateId,
					Personalities = element.GetPersonalities(),
					LifeSkillAttainments = element.GetLifeSkillAttainments(),
					Avatar = element.GenerateAvatarRelatedData(),
					Date = DomainManager.World.GetCurrDate()
				};
				CharacterDomain.GetNameRelatedData(element, ref villagerRoleRecordElement.Name);
				villagerRoleRecordsData.History.Add(villagerRoleRecordElement);
			}
		}
		extra.SetVillagerRoleRecordsData(context, villagerRoleRecordsData);
	}

	private void FixUnlockedWorkingVillagersData(DataContext context)
	{
		List<int> unlockedWorkingVillagers = DomainManager.Extra.GetUnlockedWorkingVillagers();
		for (int num = unlockedWorkingVillagers.Count - 1; num >= 0; num--)
		{
			if (!_villagerWork.ContainsKey(unlockedWorkingVillagers[num]))
			{
				Logger.Warn($"Fixing character {unlockedWorkingVillagers[num]}'s wrong unlocked working villagers data");
				unlockedWorkingVillagers.RemoveAt(num);
			}
		}
		DomainManager.Extra.SetUnlockedWorkingVillagers(unlockedWorkingVillagers, context);
	}

	private void FixAbnormalTaiwuData(DataContext context)
	{
		ClearExceedCombatSkills(context);
		RemoveTaiwuInvalidFeatures(context);
		TryRemoveReferenceSkillAtLockedSlot(context);
		RemoveTaiwuFromPrison(context);
	}

	private void RemoveTaiwuFromPrison(DataContext context)
	{
		sbyte prisonerSect = DomainManager.Organization.GetPrisonerSect(_taiwuCharId);
		if (prisonerSect >= 0)
		{
			Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(prisonerSect);
			if (settlementByOrgTemplateId is Sect { Prison: var prison } sect)
			{
				prison.OfflineRemovePrisoner(_taiwuCharId);
				DomainManager.Organization.UnregisterSectPrisoner(_taiwuCharId);
				DomainManager.Extra.SetSettlementPrison(context, settlementByOrgTemplateId.GetId(), prison);
				AdaptableLog.Warning($"Fixing taiwu in prison: {_taiwuChar} removed from {sect}.");
			}
		}
	}

	private void FixAbnormalEquipmentPlan(DataContext context)
	{
		if (!DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 68))
		{
			return;
		}
		ItemKey other = default(ItemKey);
		bool flag = false;
		for (int i = 0; i < _equipmentsPlans.Length; i++)
		{
			EquipmentPlan equipmentPlan = _equipmentsPlans[i];
			for (int j = 0; j < equipmentPlan.Slots.Length; j++)
			{
				if (equipmentPlan.Slots[j].Equals(other))
				{
					equipmentPlan.Slots[j] = ItemKey.Invalid;
					flag = true;
				}
			}
			if (flag)
			{
				SetElement_EquipmentsPlans(i, equipmentPlan, context);
				Logger.Warn($"Fixing abnormal equipment plan {i}.");
			}
		}
	}

	private void FixAbnormalGroupData(DataContext context)
	{
		OrganizationInfo organizationInfo = _taiwuChar.GetOrganizationInfo();
		OrganizationInfo organizationInfo2 = new OrganizationInfo(organizationInfo.OrgTemplateId, 0, principal: true, organizationInfo.SettlementId);
		OrganizationItem organizationItem = Config.Organization.Instance[organizationInfo2.OrgTemplateId];
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(organizationInfo2);
		foreach (int item3 in _groupCharIds.GetCollection())
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item3);
			if (element_Objects.IsActiveExternalRelationState(60))
			{
				element_Objects.DeactivateExternalRelationState(context, 60);
			}
			if (DomainManager.Character.TryGetElement_CrossAreaMoveInfos(item3, out var _))
			{
				DomainManager.Character.RemoveCrossAreaTravelInfo(context, item3);
				Logger.Warn($"Removing unexpectedly traveling taiwu group member {element_Objects}");
			}
			OrganizationInfo organizationInfo3 = element_Objects.GetOrganizationInfo();
			if (organizationInfo3.OrgTemplateId != organizationInfo.OrgTemplateId)
			{
				(string surname, string givenName) realName = CharacterDomain.GetRealName(element_Objects);
				string item = realName.surname;
				string item2 = realName.givenName;
				OrganizationMemberItem orgMemberConfig2 = OrganizationDomain.GetOrgMemberConfig(organizationInfo3);
				OrganizationItem organizationItem2 = Config.Organization.Instance[organizationInfo3.OrgTemplateId];
				if (DomainManager.Organization.TryGetSettlementCharacter(item3, out var settlementChar) && settlementChar.GetSettlementId() != organizationInfo2.SettlementId)
				{
					DomainManager.Organization.ChangeOrganization(context, element_Objects, organizationInfo2);
				}
				Logger.Warn($"Fixing abnormal taiwu group char {item}{item2}({item3}) organization: {organizationItem2.Name}{orgMemberConfig2.GradeName} => {organizationItem.Name}{orgMemberConfig.GradeName}");
			}
		}
	}

	private void RemoveTaiwuInvalidFeatures(DataContext context)
	{
		List<short> featureIds = _taiwuChar.GetFeatureIds();
		for (sbyte b = 0; b < 14; b++)
		{
			CombatSkillTypeItem combatSkillTypeItem = Config.CombatSkillType.Instance[b];
			if (featureIds.Remove(combatSkillTypeItem.LegendaryBookFeature))
			{
				Logger.Warn("Removing taiwu's invalid feature " + CharacterFeature.Instance[combatSkillTypeItem.LegendaryBookFeature].Name);
			}
		}
		_taiwuChar.SetFeatureIds(featureIds, context);
	}

	private void FixNotLearnedSkills(DataContext context)
	{
		HashSet<short> hashSet = _notLearnLifeSkillReadingProgress.Keys.ToHashSet();
		HashSet<short> other = _lifeSkills.Keys.ToHashSet();
		hashSet.IntersectWith(other);
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = _taiwuChar.GetLearnedLifeSkills();
		if (hashSet.Count > 0)
		{
			foreach (short item2 in hashSet)
			{
				Logger.Warn("Fixing life skill exists in both not learned and learned skills: " + LifeSkill.Instance[item2].Name);
				TaiwuLifeSkill element_NotLearnLifeSkillReadingProgress = GetElement_NotLearnLifeSkillReadingProgress(item2);
				TaiwuLifeSkill element_LifeSkills = GetElement_LifeSkills(item2);
				for (byte b = 0; b < 5; b++)
				{
					OfflineAddReadingProgress(element_LifeSkills, b, element_NotLearnLifeSkillReadingProgress.GetBookPageReadingProgress(b));
				}
				RemoveElement_NotLearnLifeSkillReadingProgress(item2, context);
				SetElement_LifeSkills(item2, element_LifeSkills, context);
			}
		}
		for (int i = 0; i < learnedLifeSkills.Count; i++)
		{
			GameData.Domains.Character.LifeSkillItem skill = learnedLifeSkills[i];
			if (!_lifeSkills.ContainsKey(skill.SkillTemplateId))
			{
				RegisterLifeSkill(context, skill);
				Logger.Warn("Fixing unregistered life skill " + LifeSkill.Instance[skill.SkillTemplateId].Name);
			}
		}
		bool flag = false;
		foreach (KeyValuePair<short, TaiwuLifeSkill> lifeSkill in _lifeSkills)
		{
			lifeSkill.Deconstruct(out var key, out var value);
			short num = key;
			TaiwuLifeSkill taiwuLifeSkill = value;
			int num2 = _taiwuChar.FindLearnedLifeSkillIndex(num);
			if (num2 >= 0)
			{
				continue;
			}
			Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[num];
			LifeSkillTypeItem lifeSkillTypeItem = Config.LifeSkillType.Instance[lifeSkillItem.Type];
			GameData.Domains.Character.LifeSkillItem item = new GameData.Domains.Character.LifeSkillItem(num);
			for (byte b2 = 0; b2 < 5; b2++)
			{
				if (taiwuLifeSkill.GetBookPageReadingProgress(b2) >= 100)
				{
					item.SetPageRead(b2);
				}
			}
			learnedLifeSkills.Add(item);
			flag = true;
			Logger.Warn($"Fixing lifeSkill {lifeSkillItem.Name} ({lifeSkillTypeItem.Name}) exists only in taiwu data, not in char data");
		}
		if (flag)
		{
			_taiwuChar.SetLearnedLifeSkills(learnedLifeSkills, context);
		}
		HashSet<short> hashSet2 = _notLearnCombatSkillReadingProgress.Keys.ToHashSet();
		HashSet<short> other2 = _combatSkills.Keys.ToHashSet();
		hashSet2.IntersectWith(other2);
		if (hashSet2.Count <= 0)
		{
			return;
		}
		foreach (short item3 in hashSet2)
		{
			Logger.Warn("Fixing combat skill exists in both not learned and learned skills: " + Config.CombatSkill.Instance[item3].Name);
			TaiwuCombatSkill element_NotLearnCombatSkillReadingProgress = GetElement_NotLearnCombatSkillReadingProgress(item3);
			TaiwuCombatSkill element_CombatSkills = GetElement_CombatSkills(item3);
			for (byte b3 = 0; b3 < 15; b3++)
			{
				OfflineAddReadingProgress(element_CombatSkills, b3, element_NotLearnCombatSkillReadingProgress.GetBookPageReadingProgress(b3));
			}
			RemoveElement_NotLearnCombatSkillReadingProgress(item3, context);
			SetElement_CombatSkills(item3, element_CombatSkills, context);
		}
	}

	private void FixHiddenEquipSkills(DataContext context)
	{
		bool flag = false;
		short[] equippedCombatSkills = _taiwuChar.GetEquippedCombatSkills();
		for (sbyte b = GameData.Domains.Character.CombatSkillHelper.SlotEndIndexes[^1]; b < equippedCombatSkills.Length; b++)
		{
			if (equippedCombatSkills[b] >= 0)
			{
				equippedCombatSkills[b] = -1;
				flag = true;
			}
		}
		if (flag)
		{
			_taiwuChar.SetEquippedCombatSkills(equippedCombatSkills, context);
			AdaptableLog.Warning("Hidden equip skills removed");
		}
	}

	private void FixInvalidSkills(DataContext context)
	{
		short num = 880;
		List<short> learnedCombatSkills = _taiwuChar.GetLearnedCombatSkills();
		if (learnedCombatSkills.Contains(num))
		{
			learnedCombatSkills.Remove(num);
			_taiwuChar.SetLearnedCombatSkills(learnedCombatSkills, context);
			CombatSkillKey objectId = new CombatSkillKey(_taiwuCharId, num);
			if (DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out var _))
			{
				DomainManager.CombatSkill.RemoveCombatSkill(_taiwuCharId, num);
			}
			AdaptableLog.Warning($"Fixing invalid skill {num} {_taiwuChar}");
		}
	}

	private void FixBreakGridBonus(DataContext context)
	{
		Dictionary<short, int> dictionary = new Dictionary<short, int>();
		foreach (KeyValuePair<short, SkillBreakBonusCollection> item in _skillBreakBonusDict)
		{
			SkillBreakPlateObsolete skillBreakPlateObsolete = _skillBreakPlateObsoleteDict[item.Key];
			sbyte activeOutlinePageType = CombatSkillStateHelper.GetActiveOutlinePageType(skillBreakPlateObsolete.SelectedPages);
			dictionary.Clear();
			List<BreakGrid> bonusBreakGrids = CombatSkillDomain.GetBonusBreakGrids(item.Key, activeOutlinePageType);
			if (bonusBreakGrids == null)
			{
				continue;
			}
			foreach (BreakGrid item2 in bonusBreakGrids)
			{
				if (dictionary.ContainsKey(item2.BonusType))
				{
					dictionary[item2.BonusType] += item2.GridCount;
				}
				else
				{
					dictionary[item2.BonusType] = item2.GridCount;
				}
			}
			SkillBreakBonusCollection value = item.Value;
			value.Clear();
			for (byte b = 0; b < skillBreakPlateObsolete.Height; b++)
			{
				for (byte b2 = 0; b2 < skillBreakPlateObsolete.Width; b2++)
				{
					SkillBreakPlateGridObsolete skillBreakPlateGridObsolete = skillBreakPlateObsolete.Grids[b][b2];
					if (skillBreakPlateGridObsolete.State == ESkillBreakGridState.Selected && skillBreakPlateGridObsolete.BonusType >= 0 && dictionary.TryGetValue(skillBreakPlateGridObsolete.BonusType, out var value2) && value2 > 0)
					{
						dictionary[skillBreakPlateGridObsolete.BonusType] = value2 - 1;
						value.AddBonusType(skillBreakPlateGridObsolete.BonusType);
					}
				}
			}
			SetElement_SkillBreakBonusDict(item.Key, value, context);
		}
	}

	private bool FixWarehouseModifiedItem(DataContext context, ItemKey key, out ItemKey baseKey)
	{
		baseKey = DomainManager.Item.GetBaseItem(key).GetItemKey();
		if (!key.Equals(baseKey))
		{
			int warehouseItemCount = DomainManager.Taiwu.GetWarehouseItemCount(key);
			DomainManager.Taiwu.WarehouseRemove(context, key, warehouseItemCount);
			DomainManager.Taiwu.WarehouseAdd(context, baseKey, warehouseItemCount);
			return true;
		}
		return false;
	}

	private void FixWarehouseNontransferableItems(DataContext context, ItemKey key)
	{
		ItemBase baseItem = DomainManager.Item.GetBaseItem(key);
		if (!baseItem.GetTransferable())
		{
			int warehouseItemCount = DomainManager.Taiwu.GetWarehouseItemCount(key);
			DomainManager.Taiwu.WarehouseRemove(context, key, warehouseItemCount);
			_taiwuChar.AddInventoryItem(context, key, warehouseItemCount);
		}
	}

	private bool FixWarehouseExtraGoodsItems(DataContext context, ItemKey key, out ItemKey result)
	{
		result = ItemKey.Invalid;
		if (ModificationStateHelper.IsActive(key.ModificationState, 8))
		{
			DomainManager.Taiwu.WarehouseRemove(context, key, 1, deleteItem: true);
			result = DomainManager.Item.CreateItem(context, key.ItemType, key.TemplateId);
			DomainManager.Taiwu.WarehouseAdd(context, result, 1);
			return true;
		}
		return false;
	}

	private bool FixWarehouseSectAccessoryItems(DataContext context, ItemKey key, out ItemKey result)
	{
		result = ItemKey.Invalid;
		ItemBase resultItem;
		bool flag = DomainManager.Item.RemoveRefinedEffectsAndReturnMaterial(context, key, null, out resultItem);
		if (flag)
		{
			result = resultItem.GetItemKey();
			DomainManager.Taiwu.WarehouseRemove(context, key, 1);
			DomainManager.Taiwu.WarehouseAdd(context, result, 1);
		}
		return flag;
	}

	private void FixItemsPoisonEffects(DataContext context)
	{
		Version currWorldGameVersion = DomainManager.World.GetCurrWorldGameVersion();
		if ((object)currWorldGameVersion != null && currWorldGameVersion.Build > 64)
		{
			return;
		}
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		list.Clear();
		list.AddRange(_taiwuChar.GetInventory().Items.Keys);
		foreach (ItemKey item in list)
		{
			Fix(item, ItemSourceType.Inventory);
		}
		list.Clear();
		list.AddRange(_warehouseItems.Keys);
		foreach (ItemKey item2 in list)
		{
			Fix(item2, ItemSourceType.Warehouse);
		}
		list.Clear();
		list.AddRange(DomainManager.Extra.TreasuryItems.Keys);
		foreach (ItemKey item3 in list)
		{
			Fix(item3, ItemSourceType.OldTreasury);
		}
		list.Clear();
		list.AddRange(DomainManager.Extra.TroughItems.Keys);
		foreach (ItemKey item4 in list)
		{
			Fix(item4, ItemSourceType.Trough);
		}
		list.Clear();
		list.AddRange(_taiwuChar.GetEquipment());
		for (int i = 0; i < list.Count; i++)
		{
			ItemKey itemKey = list[i];
			Fix(itemKey, ItemSourceType.Equipment, i);
		}
		for (int j = 0; j < _equipmentsPlans.Length; j++)
		{
			if (j != _currEquipmentPlanId)
			{
				EquipmentPlan equipmentPlan = _equipmentsPlans[j];
				list.Clear();
				list.AddRange(equipmentPlan.Slots);
				for (int k = 0; k < list.Count; k++)
				{
					ItemKey itemKey2 = list[k];
					Fix(itemKey2, ItemSourceType.EquipmentPlan, k, j);
				}
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
		void Fix(ItemKey itemKey3, ItemSourceType sourceType, int index = -1, int planIndex = -1)
		{
			if (itemKey3.IsValid() && ModificationStateHelper.IsActive(itemKey3.ModificationState, 1))
			{
				bool flag = false;
				if (DomainManager.Item.HasOldPoisonEffects(itemKey3))
				{
					if (!DomainManager.Item.GetOldPoisonEffects(itemKey3).HasPoison)
					{
						flag = true;
					}
				}
				else if (!DomainManager.Item.PoisonEffects.ContainsKey(itemKey3.Id))
				{
					flag = true;
				}
				if (flag)
				{
					ItemKey itemKey4 = itemKey3;
					if (DomainManager.Item.ItemExists(itemKey3))
					{
						DomainManager.Item.RemoveOldPoisonEffect(context, itemKey3);
					}
					switch (sourceType)
					{
					case ItemSourceType.Equipment:
						if (index > -1)
						{
							ItemKey[] equipment = _taiwuChar.GetEquipment();
							equipment[index] = itemKey4;
							_taiwuChar.SetEquipment(equipment, context);
						}
						return;
					case ItemSourceType.EquipmentPlan:
						if (index > -1 && planIndex > -1)
						{
							if (_taiwuChar.GetInventory().Items.ContainsKey(itemKey3))
							{
								RemoveItem(context, itemKey3, 1, ItemSourceType.Inventory, deleteItem: false);
							}
							if (itemKey4.Equals(itemKey3))
							{
								itemKey4 = DomainManager.Item.CreateItem(context, itemKey3.ItemType, itemKey3.TemplateId);
								AddItem(context, itemKey4, 1, ItemSourceType.Inventory);
							}
							SetEquipmentPlan(context, itemKey4, planIndex, index);
							return;
						}
						break;
					}
					RemoveItem(context, itemKey3, 1, sourceType, deleteItem: false);
					AddItem(context, itemKey4, 1, sourceType);
				}
			}
		}
	}

	private void FixWarehouse(DataContext context)
	{
		bool versionNeedRepairSectAccessory = DomainManager.Item.VersionNeedRepairSectAccessory;
		HashSet<ItemKey> hashSet = ObjectPool<HashSet<ItemKey>>.Instance.Get();
		hashSet.Clear();
		List<ItemKey> warehouseAllItemKey = DomainManager.Taiwu.GetWarehouseAllItemKey();
		for (int i = 0; i < warehouseAllItemKey.Count; i++)
		{
			ItemKey itemKey = warehouseAllItemKey[i];
			if (DomainManager.Item.ItemExists(itemKey))
			{
				if (FixWarehouseModifiedItem(context, itemKey, out var baseKey))
				{
					itemKey = baseKey;
				}
				if (FixWarehouseExtraGoodsItems(context, itemKey, out baseKey))
				{
					itemKey = baseKey;
				}
				if (versionNeedRepairSectAccessory && FixWarehouseSectAccessoryItems(context, itemKey, out baseKey))
				{
					itemKey = baseKey;
				}
				FixWarehouseNontransferableItems(context, itemKey);
				if (!ItemTemplateHelper.IsStackable(itemKey.ItemType, itemKey.TemplateId))
				{
					int warehouseItemCount = DomainManager.Taiwu.GetWarehouseItemCount(itemKey);
					if (warehouseItemCount > 1)
					{
						hashSet.Add(itemKey);
					}
				}
			}
			else
			{
				RemoveElement_WarehouseItems(itemKey, context);
				Logger.Warn($"Removing non-existing warehouse item {itemKey}.");
			}
		}
		if (hashSet.Count > 0)
		{
			foreach (ItemKey item in hashSet)
			{
				_warehouseItems[item] = 1;
			}
			hashSet.Clear();
		}
		ObjectPool<HashSet<ItemKey>>.Instance.Return(hashSet);
	}

	private void FixInventory(DataContext context)
	{
		List<ItemKey> list = _taiwuChar.GetInventory().Items.Keys.ToList();
		for (int i = 0; i < list.Count; i++)
		{
			ItemKey itemKey = list[i];
			if (DomainManager.Item.ItemExists(itemKey))
			{
				int amount = _taiwuChar.GetInventory().Items[itemKey];
				FixInventoryAnimalArmor(context, itemKey, amount);
			}
		}
	}

	private void FixInventoryAnimalArmor(DataContext context, ItemKey key, int amount)
	{
		ItemBase baseItem = DomainManager.Item.GetBaseItem(key);
		if (baseItem.GetItemSubType() == 104)
		{
			_taiwuChar.RemoveInventoryItem(context, key, amount, deleteItem: true);
		}
	}

	private void FixFiveLoongDlcClothing(DataContext context)
	{
		if (!DlcManager.IsDlcInstalled(2764950uL))
		{
			return;
		}
		bool[] array = new bool[5];
		ItemKey[] equipment = _taiwuChar.GetEquipment();
		for (int num = equipment.Length - 1; num >= 0; num--)
		{
			ItemKey itemKey = equipment[num];
			if (itemKey.ItemType == 3)
			{
				short templateId = itemKey.TemplateId;
				if (templateId >= 75 && templateId <= 79)
				{
					int num2 = itemKey.TemplateId - 75;
					if (!array[num2])
					{
						array[num2] = true;
					}
				}
			}
		}
		List<ItemKey> list = _taiwuChar.GetInventory().Items.Keys.ToList();
		for (int num3 = list.Count - 1; num3 >= 0; num3--)
		{
			ItemKey itemKey2 = list[num3];
			if (itemKey2.ItemType == 3)
			{
				short templateId = itemKey2.TemplateId;
				if (templateId >= 75 && templateId <= 79)
				{
					int num4 = itemKey2.TemplateId - 75;
					if (!array[num4])
					{
						array[num4] = true;
					}
					else
					{
						_taiwuChar.RemoveInventoryItem(context, itemKey2, 1, deleteItem: true);
						Logger.Warn($"Fixing taiwu repeated fiveLoongCloth, templateId: {itemKey2.TemplateId}");
					}
				}
			}
		}
	}

	private void FixSkillBookReadingProgress(DataContext context)
	{
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = _taiwuChar.GetLearnedLifeSkills();
		List<short> learnedCombatSkills = _taiwuChar.GetLearnedCombatSkills();
		foreach (GameData.Domains.Character.LifeSkillItem item in learnedLifeSkills)
		{
			if (TryGetTaiwuLifeSkill(item.SkillTemplateId, out var lifeSkill))
			{
				FixLifeSkillBookReadingProgress(item, lifeSkill);
			}
		}
		foreach (short item2 in learnedCombatSkills)
		{
			if (TryGetElement_CombatSkills(item2, out var value))
			{
				FixCombatSkillBookReadingProgress(item2, value);
			}
		}
		void FixCombatSkillBookReadingProgress(short skillTemplateId, TaiwuCombatSkill taiwuCombatSkill)
		{
			if (DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(_taiwuCharId, skillTemplateId), out var element))
			{
				bool flag = false;
				for (byte b = 0; b < 15; b++)
				{
					sbyte bookPageReadingProgress = taiwuCombatSkill.GetBookPageReadingProgress(b);
					if (bookPageReadingProgress < 100 && CombatSkillStateHelper.IsPageRead(element.GetReadingState(), b))
					{
						taiwuCombatSkill.SetBookPageReadingProgress(b, 100);
						flag = true;
					}
				}
				if (flag)
				{
					SetTaiwuCombatSkill(context, skillTemplateId, taiwuCombatSkill);
					Logger.Warn("Fixing taiwu Wrong SkillBookReadingProgress, SkillName: " + Config.CombatSkill.Instance[skillTemplateId].Name);
				}
			}
		}
		void FixLifeSkillBookReadingProgress(GameData.Domains.Character.LifeSkillItem lifeSkillItem, TaiwuLifeSkill taiwuLifeSkill)
		{
			short skillTemplateId = lifeSkillItem.SkillTemplateId;
			bool flag = false;
			for (byte b = 0; b < 5; b++)
			{
				sbyte bookPageReadingProgress = taiwuLifeSkill.GetBookPageReadingProgress(b);
				if (bookPageReadingProgress < 100 && lifeSkillItem.IsPageRead(b))
				{
					taiwuLifeSkill.SetBookPageReadingProgress(b, 100);
					flag = true;
				}
			}
			if (flag)
			{
				DomainManager.Taiwu.SetTaiwuLifeSkill(context, skillTemplateId, taiwuLifeSkill);
				Logger.Warn("Fixing taiwu Wrong SkillBookReadingProgress, SkillName: " + LifeSkill.Instance[skillTemplateId].Name);
			}
		}
	}

	private void FixSkillBookReadingState(DataContext context)
	{
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = _taiwuChar.GetLearnedLifeSkills();
		List<LifeSkillStateModify> list = new List<LifeSkillStateModify>();
		for (int i = 0; i < learnedLifeSkills.Count; i++)
		{
			GameData.Domains.Character.LifeSkillItem lifeSkillItem = learnedLifeSkills[i];
			if (TryGetTaiwuLifeSkill(lifeSkillItem.SkillTemplateId, out var lifeSkill))
			{
				FixLifeSkillBookReadingState(lifeSkillItem, lifeSkill, i, list);
			}
		}
		bool flag = list.Count > 0;
		foreach (LifeSkillStateModify item in list)
		{
			GameData.Domains.Character.LifeSkillItem value = learnedLifeSkills[item.Index];
			value.ReadingState = (byte)(value.ReadingState | (1 << item.ReadPageIndex));
			learnedLifeSkills[item.Index] = value;
		}
		if (flag)
		{
			_taiwuChar.SetLearnedLifeSkills(learnedLifeSkills, context);
		}
		static void FixLifeSkillBookReadingState(GameData.Domains.Character.LifeSkillItem lifeSkillItem2, TaiwuLifeSkill taiwuLifeSkill, int index, List<LifeSkillStateModify> lifeSkillStateModifies)
		{
			for (byte b = 0; b < 5; b++)
			{
				sbyte bookPageReadingProgress = taiwuLifeSkill.GetBookPageReadingProgress(b);
				if (bookPageReadingProgress == 100 && !lifeSkillItem2.IsPageRead(b))
				{
					lifeSkillStateModifies.Add(new LifeSkillStateModify
					{
						Index = index,
						ReadPageIndex = b
					});
				}
			}
		}
	}

	private void FixAvailableReadingStrategies(DataContext context)
	{
		ItemKey curReadingBook = GetCurReadingBook();
		if (curReadingBook.IsValid())
		{
			List<int> readingEventBookIdList = DomainManager.Extra.GetReadingEventBookIdList();
			if (readingEventBookIdList.Contains(curReadingBook.Id) && !DomainManager.Extra.TryGetElement_AvailableReadingStrategyMap(curReadingBook.Id, out var _))
			{
				GenerateAvailableReadingStrategies(context);
			}
		}
	}

	private void FixTaiwuReadingStrategyExpireTime(DataContext context)
	{
		foreach (KeyValuePair<ItemKey, ReadingBookStrategies> allReadingBook in DomainManager.Taiwu.GetAllReadingBooks())
		{
			DomainManager.Extra.TryFixStrategiesExpireTime(context, allReadingBook.Key, allReadingBook.Value);
		}
	}

	private void FixTaiwuObsoleteBuildingCore(DataContext context)
	{
		List<short> list = new List<short> { 236, 235 };
		List<ItemSourceType> list2 = new List<ItemSourceType>
		{
			ItemSourceType.Inventory,
			ItemSourceType.Warehouse,
			ItemSourceType.Treasury,
			ItemSourceType.Trough,
			ItemSourceType.StockStorageWarehouse,
			ItemSourceType.StockStorageGoodsShelf
		};
		List<(ItemKey, ItemSourceType)> list3 = new List<(ItemKey, ItemSourceType)>();
		foreach (ItemSourceType item in list2)
		{
			IReadOnlyDictionary<ItemKey, int> items = GetItems(item);
			foreach (ItemKey key in items.Keys)
			{
				if (key.ItemType == 12 && list.Contains(key.TemplateId))
				{
					list3.Add((key, item));
				}
			}
		}
		foreach (var (itemKey, itemSourceType) in list3)
		{
			Logger.Warn($"Remove Taiwu Obsolete Item : {itemKey}");
			RemoveItem(context, itemKey, 1, itemSourceType, deleteItem: true);
		}
	}

	private void FixTaiwuStockStorageGoodsShelf(DataContext context)
	{
		TaiwuVillageStorage stockStorage = DomainManager.Extra.GetStockStorage();
		Inventory inventory = stockStorage.Inventories[1];
		Inventory inventory2 = new Inventory();
		ItemKey key;
		int value;
		foreach (KeyValuePair<ItemKey, int> item in inventory.Items)
		{
			item.Deconstruct(out key, out value);
			ItemKey itemKey = key;
			int amount = value;
			ItemDisplayData itemData = new ItemDisplayData
			{
				Key = itemKey
			};
			if (!GameData.Domains.Taiwu.VillagerRole.SharedMethods.CheckCanStoreItem(ItemSourceType.StockStorageGoodsShelf, itemData))
			{
				inventory2.OfflineAdd(itemKey, amount);
			}
		}
		foreach (KeyValuePair<ItemKey, int> item2 in inventory2.Items)
		{
			item2.Deconstruct(out key, out value);
			ItemKey itemKey2 = key;
			int amount2 = value;
			inventory.OfflineRemove(itemKey2);
			DomainManager.Taiwu.AddItem(context, itemKey2, amount2, ItemSourceType.Warehouse);
		}
		stockStorage.NeedCommit = true;
		DomainManager.Extra.CommitTaiwuVillageStorages(context);
	}

	private void FixObsoletePoisonImmunities(DataContext context)
	{
		if (DomainManager.Extra.RemovePoisonImmunities(context, _taiwuCharId))
		{
			sbyte worldCreationGroupLevel = GetWorldCreationGroupLevel(0);
			short num = SwordTomb.Instance[(sbyte)0].Legacies[worldCreationGroupLevel];
			ApplyLegacyAffect(context, _taiwuChar, num);
			Logger.Warn($"Removing {_taiwuChar}'s poison immunities and applied {num}'s effect.");
		}
	}

	public void FixTaiwuSettlementTreasuryLoveTokenItem(DataContext context)
	{
		SettlementTreasury taiwuTreasury = DomainManager.Taiwu.GetTaiwuTreasury();
		List<ItemKey> list = taiwuTreasury.Inventory.Items.Keys.ToList();
		foreach (ItemKey item in list)
		{
			if (ModificationStateHelper.IsActive(item.ModificationState, 4))
			{
				ItemBase baseItem = DomainManager.Item.GetBaseItem(item);
				ItemKey itemKey = baseItem.GetItemKey();
				if (!itemKey.Equals(item))
				{
					int amount = taiwuTreasury.Inventory.Items[item];
					taiwuTreasury.Inventory.OfflineRemove(item);
					taiwuTreasury.Inventory.OfflineAdd(itemKey, amount);
				}
			}
		}
		CommitTaiwuSettlementTreasury(context);
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public (int, int, int) PracticeCombatSkillWithExp(DataContext context, short skillTemplateId)
	{
		return (-1, -1, -1);
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public int GetPracticeCostExp(short skillTemplateId)
	{
		return 0;
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public int CalcPracticeResult(DataContext context, short skillTemplateId, short attainment = -1)
	{
		return 0;
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public sbyte GetLearnCombatSkillPracticeLevel(short combatSkillTemplateId)
	{
		return 0;
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public bool CanBreakOut(short templateId)
	{
		return true;
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public SkillBreakBonusCollection AutoBreakOut(DataContext context, short skillTemplateId, ushort selectedPages)
	{
		return null;
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public int GetRemainingBonusCount(DataContext context, short skillTemplateId)
	{
		return 0;
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public int GetBreakGridCostExp(CombatSkillItem skillConfig)
	{
		return 0;
	}

	[SingleValueDependency(9, new ushort[] { 2 })]
	[SingleValueCollectionDependency(9, new ushort[] { 1 })]
	[SingleValueCollectionDependency(9, new ushort[] { 22 })]
	private int CalcTaiwuGroupMaxCount()
	{
		int num = 10;
		return num + DomainManager.Building.CalcExtraTaiwuGroupMaxCountByStrategyRoom();
	}

	[SingleValueDependency(9, new ushort[] { 2 })]
	[SingleValueCollectionDependency(9, new ushort[] { 1 })]
	[SingleValueDependency(19, new ushort[] { 284 })]
	private int CalcWarehouseMaxLoad()
	{
		int num = 5000;
		BuildingScaleItem buildingScaleItem = BuildingScale.Instance[(short)112];
		List<Location> taiwuBuildingAreas = DomainManager.Building.GetTaiwuBuildingAreas();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location elementId = taiwuBuildingAreas[i];
			BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(elementId);
			for (short num2 = 0; num2 < element_BuildingAreas.Width * element_BuildingAreas.Width; num2++)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, num2);
				BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
				if (element_BuildingBlocks.TemplateId == 48 && element_BuildingBlocks.CanUse() && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value))
				{
					num += buildingScaleItem.LevelEffect.GetOrLast(value.CalcUnlockedLevelCount() - 1) * 100;
				}
			}
		}
		return num;
	}

	[SingleValueCollectionDependency(19, new ushort[] { 18 })]
	private int CalcTroughCurrLoad()
	{
		return DomainManager.Extra.CalcTroughCurrLoad();
	}

	[SingleValueDependency(9, new ushort[] { 2 })]
	[SingleValueCollectionDependency(9, new ushort[] { 1 })]
	private int CalcTroughMaxLoad()
	{
		int num = 0;
		BuildingScaleItem buildingScaleItem = BuildingScale.Instance[(short)110];
		List<Location> taiwuBuildingAreas = DomainManager.Building.GetTaiwuBuildingAreas();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location elementId = taiwuBuildingAreas[i];
			BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(elementId);
			for (short num2 = 0; num2 < element_BuildingAreas.Width * element_BuildingAreas.Width; num2++)
			{
				BuildingBlockKey elementId2 = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, num2);
				BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId2);
				if (element_BuildingBlocks.TemplateId == 49)
				{
					num += buildingScaleItem.LevelEffect[0] * 100;
				}
			}
		}
		return num;
	}

	[SingleValueDependency(9, new ushort[] { 2 })]
	[SingleValueCollectionDependency(9, new ushort[] { 1 })]
	[SingleValueDependency(5, new ushort[] { 12 })]
	[SingleValueDependency(19, new ushort[] { 284 })]
	private int CalcBuildingSpaceLimit()
	{
		int num = GetTaiwuVillageBaseSpace();
		if (_prosperousConstruction)
		{
			num += 20;
		}
		num += GetBuildingSpaceExtraAdd();
		Location taiwuVillageLocation = GetTaiwuVillageLocation();
		return num + DomainManager.Building.GetBuildingBlockEffect(taiwuVillageLocation, EBuildingScaleEffect.BuildingSpaceBonus);
	}

	private int GetTaiwuVillageBaseSpace()
	{
		int num = 0;
		BuildingScaleItem buildingScaleItem = BuildingScale.Instance[(short)107];
		List<Location> taiwuBuildingAreas = DomainManager.Building.GetTaiwuBuildingAreas();
		Location taiwuVillageLocation = GetTaiwuVillageLocation();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location elementId = taiwuBuildingAreas[i];
			if (elementId.AreaId != taiwuVillageLocation.AreaId || elementId.BlockId != taiwuVillageLocation.BlockId)
			{
				continue;
			}
			BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(elementId);
			for (short num2 = 0; num2 < element_BuildingAreas.Width * element_BuildingAreas.Width; num2++)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, num2);
				BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
				if (element_BuildingBlocks.TemplateId == 44 && element_BuildingBlocks.CanUse() && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value))
				{
					num += buildingScaleItem.LevelEffect.GetOrLast(value.CalcUnlockedLevelCount() - 1);
				}
			}
		}
		return num;
	}

	[SingleValueDependency(9, new ushort[] { 2 })]
	[SingleValueCollectionDependency(9, new ushort[] { 1 })]
	private int CalcBuildingSpaceCurr()
	{
		int num = 0;
		List<Location> taiwuBuildingAreas = DomainManager.Building.GetTaiwuBuildingAreas();
		Location taiwuVillageLocation = GetTaiwuVillageLocation();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location elementId = taiwuBuildingAreas[i];
			if (elementId.AreaId != taiwuVillageLocation.AreaId || elementId.BlockId != taiwuVillageLocation.BlockId)
			{
				continue;
			}
			BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(elementId);
			for (short num2 = 0; num2 < element_BuildingAreas.Width * element_BuildingAreas.Width; num2++)
			{
				BuildingBlockKey elementId2 = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, num2);
				BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(elementId2);
				if (element_BuildingBlocks.TemplateId >= 0 && (BuildingBlock.Instance[element_BuildingBlocks.TemplateId].Type == EBuildingBlockType.Building || BuildingBlock.Instance[element_BuildingBlocks.TemplateId].Type == EBuildingBlockType.MainBuilding))
				{
					num += BuildingBlock.Instance[element_BuildingBlocks.TemplateId].Width;
				}
			}
		}
		return num;
	}

	[SingleValueCollectionDependency(5, new ushort[] { 7 })]
	[SingleValueCollectionDependency(19, new ushort[] { 195 })]
	[SingleValueDependency(19, new ushort[] { 230, 231, 232, 234 })]
	private int CalcWarehouseCurrLoad()
	{
		int num = 0;
		foreach (KeyValuePair<ItemKey, int> warehouseItem in _warehouseItems)
		{
			warehouseItem.Deconstruct(out var key, out var value);
			ItemKey itemKey = key;
			int num2 = value;
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			num += baseItem.GetWeight() * num2;
		}
		if (_taiwuVillageSettlementId >= 0)
		{
			num += GetTaiwuTreasury().Inventory.GetTotalWeight();
		}
		num += DomainManager.Extra.GetStockStorage().GetTotalWeight();
		num += DomainManager.Extra.GetCraftStorage().GetTotalWeight();
		num += DomainManager.Extra.GetMedicineStorage().GetTotalWeight();
		return num + DomainManager.Extra.GetFoodStorage().GetTotalWeight();
	}

	[SingleValueDependency(5, new ushort[] { 67 })]
	[ObjectCollectionDependency(6, 4, new ushort[] { 4 }, Scope = InfluenceScope.CharWhoEquippedTheItem, Condition = InfluenceCondition.ItemIsEquipped)]
	private int CalcMoveTimeCostPercent()
	{
		int num = 0;
		bool flag = false;
		foreach (int item in _groupCharIds.GetCollection())
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			num = Math.Max(num, element_Objects.CalcMoveTimePercent());
			flag = flag || element_Objects.IsOverweight;
		}
		if (!flag)
		{
			num = DomainManager.Taiwu.GetTaiwu().CalcMoveTimePercent();
		}
		return num;
	}

	[SingleValueDependency(5, new ushort[] { 34 })]
	[ObjectCollectionDependency(6, 3, new ushort[] { 4 }, Scope = InfluenceScope.CharWhoEquippedTheItem, Condition = InfluenceCondition.ItemIsEquipped)]
	private void CalcClothingDurability(List<IntPair> value)
	{
		foreach (int item in _groupCharIds.GetCollection())
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			ItemKey itemKey = element_Objects.GetEquipment()[4];
			value.Add((!itemKey.IsValid()) ? new IntPair(item, 0) : new IntPair(item, DomainManager.Item.GetBaseItem(itemKey).GetCurrDurability()));
		}
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 104, 105 }, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
	[SingleValueDependency(5, new ushort[] { 34 })]
	[SingleValueCollectionDependency(19, new ushort[] { 247 }, Scope = InfluenceScope.TaiwuChar)]
	private void CalcOverweightSanctionPercent(List<IntPair> value)
	{
		foreach (int item in _groupCharIds.GetCollection())
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.IsOverweight)
			{
				value.Add(new IntPair(item, element_Objects.CalcOverweightSanctionPercent()));
			}
		}
		foreach (int item2 in _taiwuSpecialGroup)
		{
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2);
			if (element_Objects2.IsOverweight)
			{
				value.Add(new IntPair(item2, element_Objects2.CalcOverweightSanctionPercent()));
			}
		}
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 92, 57 }, Condition = InfluenceCondition.CharIsTaiwu)]
	[SingleValueDependency(5, new ushort[] { 26 })]
	[SingleValueDependency(19, new ushort[] { 78 })]
	private void CalcWeaponCurrInnerRatios(sbyte[] value)
	{
		ItemKey[] equipment = _taiwuChar.GetEquipment();
		sbyte[] weaponInnerRatios = GetWeaponInnerRatios();
		for (int i = 0; i < 7; i++)
		{
			if (1 == 0)
			{
			}
			short num = ((i < 3) ? equipment[i].TemplateId : (i switch
			{
				3 => 0, 
				4 => 1, 
				5 => 2, 
				_ => 884, 
			}));
			if (1 == 0)
			{
			}
			short num2 = num;
			if (num2 >= 0)
			{
				sbyte expectRatio = ((i < 6) ? weaponInnerRatios[i] : DomainManager.Extra.GetVoiceWeaponInnerRatio());
				value[i] = _taiwuChar.CalcWeaponInnerRatio(num2, expectRatio);
			}
		}
	}

	[SingleValueDependency(1, new ushort[] { 0 })]
	private int CalcMaterialResourceMaxCount()
	{
		return 999999999;
	}

	[SingleValueCollectionDependency(5, new ushort[] { 56 })]
	[SingleValueDependency(19, new ushort[] { 106 })]
	[SingleValueCollectionDependency(9, new ushort[] { 1 })]
	[SingleValueCollectionDependency(9, new ushort[] { 22 })]
	[SingleValueCollectionDependency(9, new ushort[] { 3 })]
	[SingleValueDependency(19, new ushort[] { 180 })]
	[SingleValueDependency(19, new ushort[] { 22 })]
	[SingleValueDependency(19, new ushort[] { 160 })]
	[SingleValueDependency(19, new ushort[] { 284 })]
	[SingleValueDependency(9, new ushort[] { 2 })]
	[SingleValueDependency(1, new ushort[] { 18 })]
	[SingleValueDependency(1, new ushort[] { 7 })]
	[ObjectCollectionDependency(4, 0, new ushort[] { 34 }, Condition = InfluenceCondition.CharIsTaiwu)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 97, 98, 56 }, Condition = InfluenceCondition.CharIsTaiwuWorker)]
	private void CalcResourceChange(int[] value)
	{
		if (!DomainManager.World.GetWorldFunctionsStatus(10))
		{
			for (sbyte b = 0; b < 8; b++)
			{
				value[b] = 0;
			}
			return;
		}
		CalcResourceChangeBeforeExpand(value);
		for (sbyte b2 = 0; b2 < value.Length; b2++)
		{
			value[b2] += DomainManager.Extra.CalcResourceChangeByJiaoPool(b2);
		}
		value[7] += DomainManager.Taiwu.GetVillagerRoleHeadTotalAuthorityCost();
		value[7] += DomainManager.Organization.CalcApprovingRateEffectAuthorityGain();
	}

	public void CalcResourceChangeBeforeExpand(int[] value)
	{
		CalcResourceBlockIncomeEffectValues(ref value);
		foreach (KeyValuePair<int, VillagerWorkData> item in _villagerWork)
		{
			VillagerWorkData value2 = item.Value;
			if (value2.WorkType == 10 && CanWork(value2.CharacterId))
			{
				int collectResourceIncome = value2.GetCollectResourceIncome();
				value[value2.ResourceType] += collectResourceIncome;
			}
		}
		List<Location> taiwuBuildingAreas = DomainManager.Building.GetTaiwuBuildingAreas();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location elementId = taiwuBuildingAreas[i];
			BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(elementId);
			for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, num);
				BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
				if (element_BuildingBlocks.RootBlockIndex < 0 && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value3))
				{
					BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
					if (buildingBlockItem.IsCollectResourceBuilding)
					{
						sbyte collectBuildingResourceType = DomainManager.Building.GetCollectBuildingResourceType(buildingBlockKey);
						sbyte b = (sbyte)((collectBuildingResourceType < 6) ? collectBuildingResourceType : 5);
						value[b] += DomainManager.Building.CalcResourceOutputCount(buildingBlockKey, b);
					}
					if (element_BuildingBlocks.NeedMaintenanceCost())
					{
						int[] finalMaintenanceCost = GameData.Domains.Building.SharedMethods.GetFinalMaintenanceCost(buildingBlockItem);
						for (int j = 0; j < finalMaintenanceCost.Length; j++)
						{
							value[j] -= finalMaintenanceCost[j];
						}
					}
					if (element_BuildingBlocks.TemplateId == 45 && element_BuildingBlocks.CanUse())
					{
						value[7] += DomainManager.Building.CalculateGainAuthorityByShrinePerMonth(buildingBlockKey);
					}
					if (element_BuildingBlocks.Durability == 0 && value3.CalcUnlockedLevelCount() == 1 && buildingBlockItem.DestoryType == 1)
					{
						value[7] -= buildingBlockItem.MaxDurability;
					}
				}
			}
		}
		if (DomainManager.World.GetCurrMonthInYear() == GlobalConfig.Instance.CricketActiveStartMonth - 1)
		{
			value[7] += BuildingDomain.GetCricketAuthorityGain();
		}
	}

	public void CalcResourceBlockIncomeEffectValues(ref int[] value)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		ResourceInts resourceChange = DomainManager.Building.CalcResourceBlockIncomeEffects(taiwuVillageLocation).resourcesChange;
		DomainManager.Building.FinalizeResourceBlockIncomeEffectValues(ref resourceChange, isTaiwu: true);
		for (int i = 0; i < value.Length; i++)
		{
			value[i] += resourceChange.Get(i);
		}
	}

	[SingleValueCollectionDependency(9, new ushort[] { 1 })]
	private short CalcWorkLocationMaxCount()
	{
		DomainManager.Building.TryGetElement_BuildingBlocks(default(BuildingBlockKey), out var _);
		int markLocationBaseMaxCount = GlobalConfig.Instance.MarkLocationBaseMaxCount;
		return (short)markLocationBaseMaxCount;
	}

	[ObjectCollectionDependency(3, 1, new ushort[] { 10 }, Condition = InfluenceCondition.CivilianSettlementIsTaiwuVillage)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(5, new ushort[] { 55 })]
	private short CalcTotalVillagerCount()
	{
		short num = 0;
		if (_taiwuVillageSettlementId >= 0)
		{
			List<int> list = ObjectPool<List<int>>.Instance.Get();
			DomainManager.Organization.GetElement_CivilianSettlements(_taiwuVillageSettlementId).GetMembers().GetAllMembers(list);
			num += (short)list.Count;
			if (list.Contains(_taiwuCharId))
			{
				num--;
			}
			ObjectPool<List<int>>.Instance.Return(list);
		}
		return num;
	}

	[ObjectCollectionDependency(3, 1, new ushort[] { 10 }, Condition = InfluenceCondition.CivilianSettlementIsTaiwuVillage)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 76 }, Condition = InfluenceCondition.CharIsTaiwuVillager)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(5, new ushort[] { 55 })]
	private short CalcTotalAdultVillagerCount()
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		DomainManager.Organization.GetElement_CivilianSettlements(_taiwuVillageSettlementId).GetMembers().GetAllMembers(list);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (DomainManager.Character.GetElement_Objects(list[num]).GetPhysiologicalAge() < 16)
			{
				list.RemoveAt(num);
			}
		}
		short num2 = (short)list.Count;
		if (list.Contains(_taiwuCharId))
		{
			num2--;
		}
		ObjectPool<List<int>>.Instance.Return(list);
		return num2;
	}

	[ObjectCollectionDependency(3, 1, new ushort[] { 10 }, Condition = InfluenceCondition.CivilianSettlementIsTaiwuVillage)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 76, 69 }, Condition = InfluenceCondition.CharIsTaiwuVillager)]
	[SingleValueDependency(5, new ushort[] { 34 })]
	[SingleValueDependency(5, new ushort[] { 55 })]
	[SingleValueCollectionDependency(5, new ushort[] { 56 })]
	private short CalcAvailableVillagerCount()
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		DomainManager.Organization.GetElement_CivilianSettlements(_taiwuVillageSettlementId).GetMembers().GetAllMembers(list);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (!IsVillagerAvailableForWork(list[num], actuallyNotOccupiedOnly: true))
			{
				list.RemoveAt(num);
			}
		}
		short result = (short)list.Count;
		ObjectPool<List<int>>.Instance.Return(list);
		return result;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 98 }, Condition = InfluenceCondition.CharIsTaiwu)]
	private unsafe byte CalcReferenceBookSlotUnlockStates()
	{
		LifeSkillShorts lifeSkillAttainments = _taiwuChar.GetLifeSkillAttainments();
		byte b = 1;
		for (int i = 0; i < 16; i++)
		{
			if (lifeSkillAttainments.Items[i] >= GlobalConfig.Instance.ReferenceBookSlotUnlockParams[2])
			{
				b = 3;
				break;
			}
			if (lifeSkillAttainments.Items[i] >= GlobalConfig.Instance.ReferenceBookSlotUnlockParams[1])
			{
				b = 2;
			}
		}
		if (1 == 0)
		{
		}
		byte result = b switch
		{
			3 => 7, 
			2 => 3, 
			_ => 1, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	[SingleValueDependency(9, new ushort[] { 2 })]
	[SingleValueDependency(1, new ushort[] { 27 })]
	[SingleValueCollectionDependency(9, new ushort[] { 1 })]
	[SingleValueCollectionDependency(19, new ushort[] { 245 })]
	private bool CalcVillagerLearnLifeSkillsFromSect()
	{
		int currDate = DomainManager.World.GetCurrDate();
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(16);
		if (professionData.SkillsData is TeaTasterSkillsData teaTasterSkillsData && teaTasterSkillsData.VillagersLastLearnSkillDate + 3 >= currDate)
		{
			return false;
		}
		Location taiwuVillageLocation = GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			if (element_BuildingBlocks.TemplateId == 281 && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value))
			{
				return value.CalcUnlockedLevelCount() > 0;
			}
		}
		return false;
	}

	[SingleValueDependency(9, new ushort[] { 2 })]
	[SingleValueDependency(1, new ushort[] { 27 })]
	[SingleValueCollectionDependency(9, new ushort[] { 1 })]
	[SingleValueCollectionDependency(19, new ushort[] { 245 })]
	private bool CalcVillagerLearnCombatSkillsFromSect()
	{
		int currDate = DomainManager.World.GetCurrDate();
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(7);
		if (professionData.SkillsData is WineTasterSkillsData wineTasterSkillsData && wineTasterSkillsData.VillagersLastLearnSkillDate + 3 >= currDate)
		{
			return false;
		}
		Location taiwuVillageLocation = GetTaiwuVillageLocation();
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(taiwuVillageLocation);
		for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, num);
			BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
			if (element_BuildingBlocks.TemplateId == 282 && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value))
			{
				return value.CalcUnlockedLevelCount() > 0;
			}
		}
		return false;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 100 }, Condition = InfluenceCondition.CharIsTaiwu)]
	private unsafe byte CalcReferenceSkillSlotUnlockStates()
	{
		CombatSkillShorts combatSkillAttainments = _taiwuChar.GetCombatSkillAttainments();
		byte b = 1;
		for (int i = 0; i < 14; i++)
		{
			if (combatSkillAttainments.Items[i] >= GlobalConfig.Instance.ReferenceSkillSlotUnlockParams[2])
			{
				b = 3;
				break;
			}
			if (combatSkillAttainments.Items[i] >= GlobalConfig.Instance.ReferenceSkillSlotUnlockParams[1])
			{
				b = 2;
			}
		}
		if (1 == 0)
		{
		}
		byte result = b switch
		{
			3 => 7, 
			2 => 3, 
			_ => 1, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 26 }, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
	private Injuries CalcTaiwuGroupWorstInjuries()
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		Injuries result = default(Injuries);
		result.Initialize();
		foreach (int item in collection)
		{
			Injuries injuries = DomainManager.Character.GetElement_Objects(item).GetInjuries();
			for (sbyte b = 0; b < 7; b++)
			{
				var (b2, b3) = injuries.Get(b);
				if (result.Get(b, isInnerInjury: true) < b3)
				{
					result.Set(b, isInnerInjury: true, b3);
				}
				if (result.Get(b, isInnerInjury: false) < b2)
				{
					result.Set(b, isInnerInjury: false, b2);
				}
			}
		}
		return result;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 34 }, Condition = InfluenceCondition.CharIsTaiwu)]
	[SingleValueDependency(19, new ushort[] { 230, 231, 232, 234 })]
	private ResourceInts CalcTotalResources()
	{
		ResourceInts result = default(ResourceInts);
		result.Initialize();
		result.Add(ref _taiwuChar.GetResources());
		if (!DomainManager.Extra.TryGetElement_SettlementTreasuries(_taiwuVillageSettlementId, out var value))
		{
			return result;
		}
		result.Add(ref value.Resources);
		TaiwuVillageStorage stockStorage = DomainManager.Extra.GetStockStorage();
		TaiwuVillageStorage craftStorage = DomainManager.Extra.GetCraftStorage();
		TaiwuVillageStorage medicineStorage = DomainManager.Extra.GetMedicineStorage();
		TaiwuVillageStorage foodStorage = DomainManager.Extra.GetFoodStorage();
		result.Add(ref stockStorage.Resources);
		result.Add(ref craftStorage.Resources);
		result.Add(ref medicineStorage.Resources);
		result.Add(ref foodStorage.Resources);
		return result;
	}

	[SingleValueCollectionDependency(19, new ushort[] { 247 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(8, new ushort[] { 32 })]
	private List<int> CalcTaiwuSpecialGroup(List<int> value)
	{
		int taiwuCharId = GetTaiwuCharId();
		IReadOnlySet<int> specialGroup = DomainManager.Extra.GetSpecialGroup(taiwuCharId);
		value.Clear();
		value.AddRange(specialGroup);
		foreach (int taiwuSpecialGroupCharId in DomainManager.Combat.GetTaiwuSpecialGroupCharIds())
		{
			if (!value.Contains(taiwuSpecialGroupCharId))
			{
				value.Add(taiwuSpecialGroupCharId);
			}
		}
		return value;
	}

	[SingleValueCollectionDependency(5, new ushort[] { 71 })]
	private List<int> CalcTaiwuGearMateGroup(List<int> value)
	{
		List<int> taiwuSpecialGroup = DomainManager.Taiwu.GetTaiwuSpecialGroup();
		value.Clear();
		foreach (int item in taiwuSpecialGroup)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetTemplateId() == 836)
			{
				value.Add(item);
			}
		}
		return value;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 19 }, Condition = InfluenceCondition.CharIsTaiwu)]
	private bool CalcCanBreakOut()
	{
		return _taiwuChar.GetHealth() > 0;
	}

	[DomainMethod]
	public void WarehouseAdd(DataContext context, ItemKey itemKey, int amount)
	{
		Tester.Assert(itemKey.IsValid());
		if (amount <= 0)
		{
			Logger.Warn($"WarehouseAdd amount <= 0, itemKey: {itemKey}, amount: {amount}");
			return;
		}
		DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Warehouse, _taiwuVillageSettlementId);
		if (_warehouseItems.TryGetValue(itemKey, out var value))
		{
			Tester.Assert(ItemTemplateHelper.IsPureStackable(itemKey));
			SetElement_WarehouseItems(itemKey, value + amount, context);
		}
		else
		{
			AddElement_WarehouseItems(itemKey, amount, context);
		}
		Events.RaiseTaiwuItemModified(context, itemKey);
	}

	[DomainMethod]
	public void WarehouseAddList(DataContext context, List<ItemKey> keyList)
	{
		Tester.Assert(keyList != null);
		Tester.Assert(keyList.Count > 0);
		foreach (ItemKey key in keyList)
		{
			WarehouseAdd(context, key, 1);
		}
	}

	[DomainMethod]
	public void WarehouseRemove(DataContext context, ItemKey itemKey, int amount, bool deleteItem = false)
	{
		Tester.Assert(itemKey.IsValid());
		Tester.Assert(amount > 0);
		DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Warehouse, _taiwuVillageSettlementId);
		int num = _warehouseItems[itemKey] - amount;
		if (num > 0)
		{
			SetElement_WarehouseItems(itemKey, num, context);
			return;
		}
		if (num == 0)
		{
			RemoveElement_WarehouseItems(itemKey, context);
			if (itemKey.ItemType == 0)
			{
				DomainManager.Extra.ClearLegendaryBookWeaponSlot(context, itemKey);
			}
			if (ItemType.IsEquipmentItemType(itemKey.ItemType))
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
				baseEquipment.SetModificationState(baseEquipment.GetModificationState(), context);
			}
			if (deleteItem)
			{
				DomainManager.Item.RemoveItem(context, itemKey);
			}
			return;
		}
		throw new Exception($"Item amount cannot be negative after removing: {itemKey}, {amount}");
	}

	[DomainMethod]
	public void WarehouseRemoveList(DataContext context, List<ItemKey> keyList, bool deleteItem = false)
	{
		Tester.Assert(keyList != null);
		Tester.Assert(keyList.Count > 0);
		foreach (ItemKey key in keyList)
		{
			WarehouseRemove(context, key, 1, deleteItem);
		}
	}

	public ItemKey CreateWarehouseItem(DataContext context, sbyte itemType, short templateId, int amount)
	{
		ItemKey itemKey = ItemKey.Invalid;
		if (amount <= 1 || ItemTemplateHelper.IsStackable(itemType, templateId))
		{
			itemKey = DomainManager.Item.CreateItem(context, itemType, templateId);
			WarehouseAdd(context, itemKey, amount);
		}
		else
		{
			for (int i = 0; i < amount; i++)
			{
				itemKey = DomainManager.Item.CreateItem(context, itemType, templateId);
				AddElement_WarehouseItems(itemKey, 1, context);
				DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Warehouse, _taiwuVillageSettlementId);
			}
		}
		return itemKey;
	}

	[DomainMethod]
	public void PutItemIntoWarehouse(DataContext context, ItemKey itemKey, int amount)
	{
		if (itemKey.ItemType == 0)
		{
			DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: true);
		}
		_taiwuChar.RemoveInventoryItem(context, itemKey, amount, deleteItem: false);
		WarehouseAdd(context, itemKey, amount);
		if (itemKey.ItemType == 0)
		{
			DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: false);
		}
	}

	[DomainMethod]
	public void PutItemListIntoWarehouse(DataContext context, List<ItemKey> keyList)
	{
		DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: true);
		_taiwuChar.RemoveInventoryItemList(context, keyList, deleteItem: false);
		WarehouseAddList(context, keyList);
		DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: false);
	}

	[DomainMethod]
	public void TakeOutItemFromWarehouse(DataContext context, ItemKey itemKey, int amount)
	{
		if (itemKey.ItemType == 0)
		{
			DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: true);
		}
		WarehouseRemove(context, itemKey, amount);
		_taiwuChar.AddInventoryItem(context, itemKey, amount);
		if (itemKey.ItemType == 0)
		{
			DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: false);
		}
	}

	[DomainMethod]
	public void TakeOutItemListFromWarehouse(DataContext context, List<ItemKey> keyList)
	{
		DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: true);
		WarehouseRemoveList(context, keyList);
		_taiwuChar.AddInventoryItemList(context, keyList);
		DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: false);
	}

	[DomainMethod]
	public bool CanTransferItemToWarehouse(DataContext context)
	{
		Location location = _taiwuChar.GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		Location location2 = DomainManager.Map.GetBlock(location).GetRootBlock().GetLocation();
		if (!DomainManager.Building.TryGetElement_BuildingAreas(location2, out var value))
		{
			return false;
		}
		return BuildingDomain.HasBuilt(location2, value, 48) || BuildingDomain.HasBuilt(location2, value, 257) || BuildingDomain.HasBuilt(location2, value, 258) || BuildingDomain.HasBuilt(location2, value, 44);
	}

	public (OpenShopEventArguments.EMerchantSourceType merchantSourceType, sbyte merchantType) GetAreaMerchantInfo(short areaId)
	{
		sbyte b = -1;
		OpenShopEventArguments.EMerchantSourceType item = OpenShopEventArguments.EMerchantSourceType.None;
		MapAreaData areaByAreaId = DomainManager.Map.GetAreaByAreaId(areaId);
		short blockId = areaByAreaId.SettlementInfos.First().BlockId;
		Location location = new Location(areaId, blockId);
		if (!DomainManager.Building.TryGetElement_BuildingAreas(location, out var value))
		{
			return (merchantSourceType: item, merchantType: b);
		}
		for (short num = 274; num <= 280; num++)
		{
			if (BuildingDomain.HasBuilt(location, value, num))
			{
				BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[num];
				b = buildingBlockItem.MerchantId;
				MerchantTypeItem merchantTypeItem = Config.MerchantType.Instance[b];
				item = ((merchantTypeItem.HeadArea == areaByAreaId.GetTemplateId()) ? OpenShopEventArguments.EMerchantSourceType.MerchantHeadBuilding : OpenShopEventArguments.EMerchantSourceType.MerchantBranchBuilding);
				break;
			}
		}
		if (BuildingDomain.HasBuilt(location, value, 318))
		{
			item = OpenShopEventArguments.EMerchantSourceType.SpecialBuilding;
			BuildingBlockItem buildingBlockItem2 = BuildingBlock.Instance[(short)318];
			b = buildingBlockItem2.MerchantId;
		}
		return (merchantSourceType: item, merchantType: b);
	}

	[DomainMethod]
	public void EnterMerchant()
	{
		(OpenShopEventArguments.EMerchantSourceType, sbyte) areaMerchantInfo = GetAreaMerchantInfo(_taiwuChar.GetLocation().AreaId);
		DomainManager.Merchant.StartBuildingShopAction(areaMerchantInfo.Item1, areaMerchantInfo.Item2);
	}

	[DomainMethod]
	public int GetSelectMapBlockHasMerchantId(DataContext context, Location location)
	{
		if (!location.IsValid())
		{
			return -1;
		}
		Location location2 = DomainManager.Map.GetBlock(location).GetRootBlock().GetLocation();
		if (!DomainManager.Building.TryGetElement_BuildingAreas(location2, out var value))
		{
			return -1;
		}
		for (short num = 274; num <= 280; num++)
		{
			if (BuildingDomain.HasBuilt(location2, value, num))
			{
				return num;
			}
		}
		if (BuildingDomain.HasBuilt(location2, value, 318))
		{
			return 318;
		}
		return -1;
	}

	[DomainMethod]
	[Obsolete("Use CalcResourceOutputCount() replace", false)]
	public int CalcBuildingResourceOutput(BuildingBlockKey blockKey)
	{
		BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
		if (!buildingBlockItem.IsCollectResourceBuilding)
		{
			return -1;
		}
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		Location elementId = DomainManager.Building.GetTaiwuBuildingAreas()[0];
		BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(elementId);
		element_BuildingAreas.GetNeighborBlocks(blockKey.BuildingBlockIndex, buildingBlockItem.Width, list);
		DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out var value);
		int num = 0;
		sbyte collectBuildingResourceType = DomainManager.Building.GetCollectBuildingResourceType(blockKey);
		sbyte requireLifeSkillType = buildingBlockItem.RequireLifeSkillType;
		for (int i = 0; i < list.Count; i++)
		{
			BuildingBlockKey elementId2 = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, list[i]);
			BuildingBlockData element_BuildingBlocks2 = DomainManager.Building.GetElement_BuildingBlocks(elementId2);
			BuildingBlockItem buildingBlockItem2 = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
			if (!BuildingBlockData.IsResource(buildingBlockItem2.Type) || buildingBlockItem.DependBuildings[0] != element_BuildingBlocks2.TemplateId || !value.GetCollection().Exists((int charId) => charId >= 0 && CanWork(charId)))
			{
				continue;
			}
			for (int num2 = 0; num2 < value.GetCount(); num2++)
			{
				int num3 = value.GetCollection()[num2];
				if (num3 >= 0 && CanWork(num3))
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num3);
					num += DomainManager.Building.BaseWorkContribution;
					num += element_Objects.GetLifeSkillAttainment(requireLifeSkillType);
				}
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		return num;
	}

	[DomainMethod]
	public List<ItemDisplayData> GetCanOperateItemDisplayDataInVillage(DataContext context, short itemSubType)
	{
		bool flag = CanTransferItemToWarehouse(context);
		List<ItemDisplayData> warehouseItemsBySubType = GetWarehouseItemsBySubType(context, itemSubType);
		if (flag)
		{
			List<ItemDisplayData> inventoryItems = DomainManager.Character.GetInventoryItems(_taiwuCharId, itemSubType);
			for (int i = 0; i < inventoryItems.Count; i++)
			{
				ItemDisplayData targetItemDisplayData = GetTargetItemDisplayData(warehouseItemsBySubType, inventoryItems[i].Key);
				if (targetItemDisplayData != null && ItemTemplateHelper.IsStackable(inventoryItems[i].Key.ItemType, inventoryItems[i].Key.TemplateId))
				{
					targetItemDisplayData.Amount += inventoryItems[i].Amount;
				}
				else
				{
					warehouseItemsBySubType.Add(inventoryItems[i]);
				}
			}
		}
		return warehouseItemsBySubType;
	}

	[DomainMethod]
	public List<ItemDisplayData> GetCannotOperateItemDisplayDataInInventory(DataContext context, short itemSubType)
	{
		bool flag = CanTransferItemToWarehouse(context);
		List<ItemDisplayData> list = new List<ItemDisplayData>();
		if (!flag)
		{
			List<ItemDisplayData> inventoryItems = DomainManager.Character.GetInventoryItems(_taiwuCharId, itemSubType);
			for (int i = 0; i < inventoryItems.Count; i++)
			{
				ItemDisplayData targetItemDisplayData = GetTargetItemDisplayData(list, inventoryItems[i].Key);
				if (targetItemDisplayData != null && ItemTemplateHelper.IsStackable(inventoryItems[i].Key.ItemType, inventoryItems[i].Key.TemplateId))
				{
					targetItemDisplayData.Amount += inventoryItems[i].Amount;
				}
				else
				{
					list.Add(inventoryItems[i]);
				}
			}
		}
		return list;
	}

	private ItemDisplayData GetTargetItemDisplayData(List<ItemDisplayData> itemDisplayDataList, ItemKey itemKey)
	{
		ItemDisplayData result = null;
		for (int i = 0; i < itemDisplayDataList.Count; i++)
		{
			if (itemKey.ItemType == itemDisplayDataList[i].Key.ItemType && itemKey.TemplateId == itemDisplayDataList[i].Key.TemplateId)
			{
				result = itemDisplayDataList[i];
			}
		}
		return result;
	}

	public int GetWarehouseItemCount(ItemKey itemKey)
	{
		if (_warehouseItems.TryGetValue(itemKey, out var value))
		{
			return value;
		}
		return 0;
	}

	public void FindWarehouseItems(Predicate<ItemBase> predicate, List<(ItemKey itemKey, int amount)> items)
	{
		foreach (KeyValuePair<ItemKey, int> warehouseItem in _warehouseItems)
		{
			warehouseItem.Deconstruct(out var key, out var value);
			ItemKey itemKey = key;
			int item = value;
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			if (predicate(baseItem))
			{
				items.Add((itemKey, item));
			}
		}
	}

	public void FindWarehouseItems(List<Predicate<ItemBase>> predicates, List<(ItemKey itemKey, int amount)> items)
	{
		foreach (KeyValuePair<ItemKey, int> warehouseItem in _warehouseItems)
		{
			warehouseItem.Deconstruct(out var key, out var value);
			ItemKey itemKey = key;
			int item = value;
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			if (ItemMatchers.MatchAll(baseItem, predicates))
			{
				items.Add((itemKey, item));
			}
		}
	}

	public List<ItemKey> GetWarehouseAllItemKey()
	{
		return _warehouseItems.Keys.ToList();
	}

	public void InitializeOwnedItems()
	{
		ItemSourceType[] taiwuVillageStorages = TaiwuVillageStorages;
		foreach (ItemSourceType itemSourceType in taiwuVillageStorages)
		{
			ItemOwnerType ownerType = ItemSourceToOwner(itemSourceType);
			IReadOnlyDictionary<ItemKey, int> items = GetItems(itemSourceType);
			foreach (var (itemKey2, _) in items)
			{
				DomainManager.Item.SetOwner(itemKey2, ownerType, _taiwuVillageSettlementId);
			}
		}
	}

	public void LoseOverloadWarehouseItems(DataContext context)
	{
		List<MapBlockData> neighborList = ObjectPool<List<MapBlockData>>.Instance.Get();
		MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		int currDate = DomainManager.World.GetCurrDate();
		TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
		bool addSecretInformation = false;
		ItemKey secretInfoParamItemKey = ItemKey.Invalid;
		Location taiwuVillageLocation = GetTaiwuVillageLocation();
		int reduceHappiness = 0;
		int num = DomainManager.Extra.CalcTroughCurrLoad();
		int troughMaxLoad = GetTroughMaxLoad();
		if (num > troughMaxLoad)
		{
			LoseOverloadItems(ItemSourceType.Trough, DomainManager.Extra.TroughItems, num - troughMaxLoad);
		}
		int num2 = GetWarehouseCurrLoad() - GetWarehouseMaxLoad();
		if (num2 <= 0)
		{
			Clean();
			return;
		}
		ItemSourceType[] taiwuVillageStorages = TaiwuVillageStorages;
		foreach (ItemSourceType itemSourceType in taiwuVillageStorages)
		{
			IReadOnlyDictionary<ItemKey, int> items = GetItems(itemSourceType);
			LoseOverloadItems(itemSourceType, items, num2);
			num2 = GetWarehouseCurrLoad() - GetWarehouseMaxLoad();
			if (num2 <= 0)
			{
				break;
			}
		}
		DomainManager.Extra.SetTaiwuVillageStoragesRecordCollection(context, taiwuVillageStoragesRecordCollection);
		if (addSecretInformation)
		{
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddLoseOverloadingItem(_taiwuCharId, (ulong)secretInfoParamItemKey, taiwuVillageLocation);
			DomainManager.Information.AddSecretInformationMetaDataWithNecessity(context, dataOffset, withInitialDistribute: true, necessarily: false, delegate(ICollection<int> charIds)
			{
				foreach (int charId in charIds)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
					Location location = element_Objects.GetLocation();
					GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(24, location);
					element_Objects.AddPersonalNeed(context, personalNeed);
				}
			});
		}
		if (reduceHappiness > 0)
		{
			_taiwuChar.ChangeHappiness(context, -reduceHappiness);
		}
		instantNotificationCollection.AddHappinessDecreased(_taiwuCharId);
		Clean();
		void Clean()
		{
			ObjectPool<List<MapBlockData>>.Instance.Return(neighborList);
		}
		void LoseOverloadItems(ItemSourceType sourceType, IReadOnlyDictionary<ItemKey, int> oriItems, int toLoseWeight)
		{
			List<(ItemBase, int)> obj = context.AdvanceMonthRelatedData.ItemsWithAmount.Occupy();
			CharacterDomain.GetLostItemsDueToOverload(context, toLoseWeight, oriItems, obj);
			DomainManager.Map.GetNeighborBlocks(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, neighborList, 3);
			int j = 0;
			for (int count = obj.Count; j < count; j++)
			{
				(ItemBase, int) tuple = obj[j];
				ItemBase item = tuple.Item1;
				int item2 = tuple.Item2;
				ItemKey itemKey = item.GetItemKey();
				monthlyNotifications.AddLoseItemCausedByWarehouseFull(itemKey.ItemType, itemKey.TemplateId);
				taiwuVillageStoragesRecordCollection.AddLoseOverloadWarehouseItems(currDate, GetTaiwuVillageStorageType(sourceType), itemKey.ItemType, itemKey.TemplateId);
				RemoveItem(context, itemKey, item2, sourceType, deleteItem: false);
				MapBlockData block = neighborList[context.Random.Next(neighborList.Count)];
				DomainManager.Map.AddBlockItem(context, block, itemKey, item2);
				reduceHappiness += item.GetHappinessChange() * item2;
				if (!addSecretInformation && ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId) >= 6)
				{
					addSecretInformation = true;
					if (!secretInfoParamItemKey.IsValid() || ItemTemplateHelper.GetBaseValue(secretInfoParamItemKey.ItemType, secretInfoParamItemKey.TemplateId) < ItemTemplateHelper.GetBaseValue(itemKey.ItemType, itemKey.TemplateId))
					{
						secretInfoParamItemKey = itemKey;
					}
				}
			}
			context.AdvanceMonthRelatedData.ItemsWithAmount.Release(ref obj);
		}
	}

	public static TaiwuVillageStorageType GetTaiwuVillageStorageType(ItemSourceType itemSourceType)
	{
		if (1 == 0)
		{
		}
		TaiwuVillageStorageType result = itemSourceType switch
		{
			ItemSourceType.Invalid => TaiwuVillageStorageType.Count, 
			ItemSourceType.Equipment => TaiwuVillageStorageType.Count, 
			ItemSourceType.Inventory => TaiwuVillageStorageType.Count, 
			ItemSourceType.Warehouse => TaiwuVillageStorageType.Warehouse, 
			ItemSourceType.Treasury => TaiwuVillageStorageType.Treasury, 
			ItemSourceType.Trough => TaiwuVillageStorageType.Count, 
			ItemSourceType.StockStorageGoodsShelf => TaiwuVillageStorageType.Stock, 
			ItemSourceType.EquipmentPlan => TaiwuVillageStorageType.Count, 
			ItemSourceType.JiaoPool => TaiwuVillageStorageType.Count, 
			ItemSourceType.SettlementTreasury => TaiwuVillageStorageType.Count, 
			ItemSourceType.OldTreasury => TaiwuVillageStorageType.Count, 
			ItemSourceType.Resources => TaiwuVillageStorageType.Count, 
			_ => TaiwuVillageStorageType.Count, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static ItemSourceType ItemOwnerToSource(ItemOwnerType ownerType)
	{
		if (1 == 0)
		{
		}
		ItemSourceType result = ownerType switch
		{
			ItemOwnerType.CharacterInventory => ItemSourceType.Inventory, 
			ItemOwnerType.CharacterEquipment => ItemSourceType.Equipment, 
			ItemOwnerType.Warehouse => ItemSourceType.Warehouse, 
			ItemOwnerType.Treasury => ItemSourceType.Treasury, 
			ItemOwnerType.StockStorageWarehouse => ItemSourceType.StockStorageWarehouse, 
			ItemOwnerType.StockStorageGoodsShelf => ItemSourceType.StockStorageGoodsShelf, 
			ItemOwnerType.CraftStorageWarehouse => ItemSourceType.CraftStorageWarehouse, 
			ItemOwnerType.CraftStorageMaterial => ItemSourceType.CraftStorageMaterial, 
			ItemOwnerType.CraftStorageToFix => ItemSourceType.CraftStorageToFix, 
			ItemOwnerType.CraftStorageToDisassemble => ItemSourceType.CraftStorageToDisassemble, 
			ItemOwnerType.MedicineStorageWarehouse => ItemSourceType.MedicineStorageWarehouse, 
			ItemOwnerType.MedicineStorageMaterial => ItemSourceType.MedicineStorageMaterial, 
			ItemOwnerType.MedicineStorageToDetox => ItemSourceType.MedicineStorageToDetox, 
			ItemOwnerType.MedicineStorageToAddPoison => ItemSourceType.MedicineStorageToAddPoison, 
			ItemOwnerType.FoodStorageWarehouse => ItemSourceType.FoodStorageWarehouse, 
			ItemOwnerType.FoodStorageMaterial => ItemSourceType.FoodStorageMaterial, 
			ItemOwnerType.Trough => ItemSourceType.Trough, 
			_ => throw new ArgumentOutOfRangeException("ownerType", ownerType, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static ItemOwnerType ItemSourceToOwner(ItemSourceType sourceType)
	{
		if (1 == 0)
		{
		}
		ItemOwnerType result = sourceType switch
		{
			ItemSourceType.Inventory => ItemOwnerType.CharacterInventory, 
			ItemSourceType.Equipment => ItemOwnerType.CharacterEquipment, 
			ItemSourceType.Warehouse => ItemOwnerType.Warehouse, 
			ItemSourceType.Treasury => ItemOwnerType.Treasury, 
			ItemSourceType.SettlementTreasury => ItemOwnerType.Treasury, 
			ItemSourceType.StockStorageWarehouse => ItemOwnerType.StockStorageWarehouse, 
			ItemSourceType.StockStorageGoodsShelf => ItemOwnerType.StockStorageGoodsShelf, 
			ItemSourceType.CraftStorageWarehouse => ItemOwnerType.CraftStorageWarehouse, 
			ItemSourceType.CraftStorageMaterial => ItemOwnerType.CraftStorageMaterial, 
			ItemSourceType.CraftStorageToFix => ItemOwnerType.CraftStorageToFix, 
			ItemSourceType.CraftStorageToDisassemble => ItemOwnerType.CraftStorageToDisassemble, 
			ItemSourceType.MedicineStorageWarehouse => ItemOwnerType.MedicineStorageWarehouse, 
			ItemSourceType.MedicineStorageMaterial => ItemOwnerType.MedicineStorageMaterial, 
			ItemSourceType.MedicineStorageToDetox => ItemOwnerType.MedicineStorageToDetox, 
			ItemSourceType.MedicineStorageToAddPoison => ItemOwnerType.MedicineStorageToAddPoison, 
			ItemSourceType.FoodStorageWarehouse => ItemOwnerType.FoodStorageWarehouse, 
			ItemSourceType.FoodStorageMaterial => ItemOwnerType.FoodStorageMaterial, 
			ItemSourceType.Trough => ItemOwnerType.Trough, 
			_ => throw new ArgumentOutOfRangeException("sourceType", sourceType, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public void LoseOverloadResources(DataContext context)
	{
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		int materialResourceMaxCount = DomainManager.Taiwu.GetMaterialResourceMaxCount();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = _taiwuChar.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
		ResourceInts totalResources = GetTotalResources();
		ResourceInts resources = _taiwuChar.GetResources();
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		TaiwuVillageStorage stockStorage = DomainManager.Extra.GetStockStorage();
		TaiwuVillageStorage craftStorage = DomainManager.Extra.GetCraftStorage();
		TaiwuVillageStorage medicineStorage = DomainManager.Extra.GetMedicineStorage();
		TaiwuVillageStorage foodStorage = DomainManager.Extra.GetFoodStorage();
		bool flag = false;
		for (sbyte b = 0; b < 6; b++)
		{
			int num = totalResources[b];
			if (num > materialResourceMaxCount)
			{
				int num2 = num - materialResourceMaxCount;
				int toLoseAmount = num2;
				if (LoseResource(b, ref resources, ref toLoseAmount))
				{
					_taiwuChar.SetResources(ref resources, context);
				}
				if (LoseResource(b, ref taiwuTreasury.Resources, ref toLoseAmount, TaiwuVillageStorageType.Treasury))
				{
					flag = true;
				}
				if (LoseResource(b, ref stockStorage.Resources, ref toLoseAmount, TaiwuVillageStorageType.Stock))
				{
					stockStorage.NeedCommit = true;
				}
				if (LoseResource(b, ref craftStorage.Resources, ref toLoseAmount, TaiwuVillageStorageType.Craft))
				{
					craftStorage.NeedCommit = true;
				}
				if (LoseResource(b, ref medicineStorage.Resources, ref toLoseAmount, TaiwuVillageStorageType.Medicine))
				{
					medicineStorage.NeedCommit = true;
				}
				if (LoseResource(b, ref foodStorage.Resources, ref toLoseAmount, TaiwuVillageStorageType.Food))
				{
					foodStorage.NeedCommit = true;
				}
				if (flag || stockStorage.NeedCommit || craftStorage.NeedCommit || medicineStorage.NeedCommit || foodStorage.NeedCommit)
				{
					DomainManager.Extra.SetTaiwuVillageStoragesRecordCollection(context, taiwuVillageStoragesRecordCollection);
				}
				if (DomainManager.Taiwu._prevMonthResourceCollection[b] != materialResourceMaxCount)
				{
					monthlyNotificationCollection.AddLoseResourceCausedByInventoryFull(_taiwuCharId, b);
					lifeRecordCollection.AddLoseOverloadingResource(_taiwuCharId, currDate, location, b, num2);
				}
			}
		}
		if (flag)
		{
			SetTaiwuTreasury(context, taiwuTreasury);
		}
		bool LoseResource(sbyte resourceType, ref ResourceInts reference2, ref int reference, TaiwuVillageStorageType storageType = TaiwuVillageStorageType.Count)
		{
			if (reference <= 0)
			{
				return false;
			}
			int num3 = Math.Min(reference2[resourceType], reference);
			if (num3 > 0)
			{
				reference2[resourceType] -= num3;
				reference -= num3;
				if (storageType != TaiwuVillageStorageType.Count)
				{
					taiwuVillageStoragesRecordCollection.AddLoseOverloadResources(currDate, storageType, resourceType, num3);
				}
			}
			return true;
		}
	}

	public void AddBuildingSpaceExtra(DataContext context, int add)
	{
		_buildingSpaceExtraAdd += add;
		SetBuildingSpaceExtraAdd(_buildingSpaceExtraAdd, context);
	}

	[DomainMethod]
	[Obsolete("使用TransferItem代替")]
	public bool TransferAllItems(DataContext context, bool isToWarehouse, List<ItemKey> keyList)
	{
		if (!CanTransferItemToWarehouse(context))
		{
			return false;
		}
		DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: true);
		if (isToWarehouse)
		{
			foreach (ItemKey key in keyList)
			{
				if (_taiwuChar.GetInventory().Items.ContainsKey(key) && !_curReadingBook.Equals(key) && !_referenceBooks.Contains(key))
				{
					ItemBase baseItem = DomainManager.Item.GetBaseItem(key);
					if (baseItem.GetTransferable())
					{
						_taiwuChar.RemoveInventoryItem(context, key, 1, deleteItem: false);
						WarehouseAdd(context, key, 1);
					}
				}
			}
		}
		else
		{
			foreach (ItemKey key2 in keyList)
			{
				if (_warehouseItems.ContainsKey(key2))
				{
					WarehouseRemove(context, key2, 1);
					_taiwuChar.AddInventoryItem(context, key2, 1);
				}
			}
		}
		DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: false);
		return true;
	}

	[DomainMethod]
	public void WarehouseDiscardItem(DataContext context, ItemKey itemKey, int count = 1)
	{
		WarehouseRemove(context, itemKey, count);
	}

	[DomainMethod]
	public void WarehouseDiscardItemList(DataContext context, List<ItemKey> keyList)
	{
		Tester.Assert(keyList != null);
		Tester.Assert(keyList.Count > 0);
		foreach (ItemKey key in keyList)
		{
			WarehouseDiscardItem(context, key);
		}
	}

	[DomainMethod]
	public bool TransferItem(DataContext context, sbyte from, sbyte to, ItemKey itemKey, int amount, bool offLine = false)
	{
		if (!CanTransferItemToWarehouse(context))
		{
			return false;
		}
		if (itemKey.ItemType == 0)
		{
			DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: true);
		}
		if (to == 4 && !GameData.Domains.Building.SharedMethods.CheckItemCanFeedChicken(itemKey))
		{
			return false;
		}
		RemoveItem(context, itemKey, amount, from, deleteItem: false, offLine);
		AddItem(context, itemKey, amount, to, offLine);
		if (itemKey.ItemType == 0)
		{
			DomainManager.Extra.SetKeepLegendaryWeaponSlotOnItemRemovedFlag(flag: false);
		}
		return true;
	}

	[DomainMethod]
	public bool TransferItemList(DataContext context, sbyte from, sbyte to, List<ItemKey> keyList)
	{
		if (!CanTransferItemToWarehouse(context))
		{
			return false;
		}
		foreach (ItemKey key in keyList)
		{
			TransferItem(context, from, to, key, 1, offLine: true);
		}
		CommitOfflineOperation(context, (ItemSourceType)from);
		CommitOfflineOperation(context, (ItemSourceType)to);
		return true;
	}

	[DomainMethod]
	public bool TransferResource(DataContext context, ItemSourceType srcType, ItemSourceType destType, sbyte resourceType, int amount)
	{
		RemoveResource(context, srcType, resourceType, amount);
		AddResource(context, destType, resourceType, amount);
		return true;
	}

	public void AddResource(DataContext context, ItemSourceType sourceType, sbyte resourceType, int amount)
	{
		if (amount <= 0)
		{
			Logger.Warn($"AddResource amount <= 0, sourceType: {sourceType}, resourceType: {resourceType}, amount: {amount}");
			return;
		}
		bool flag;
		switch (sourceType)
		{
		case ItemSourceType.Treasury:
		{
			SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
			taiwuTreasury.Resources.Add(resourceType, amount);
			SetTaiwuTreasury(context, taiwuTreasury);
			return;
		}
		case ItemSourceType.Inventory:
		case ItemSourceType.Resources:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			ResourceInts resources = GetTaiwu().GetResources();
			resources.Add(resourceType, amount);
			GetTaiwu().SetResources(ref resources, context);
			return;
		}
		if (1 == 0)
		{
		}
		TaiwuVillageStorage taiwuVillageStorage = sourceType switch
		{
			ItemSourceType.StockStorageWarehouse => DomainManager.Extra.GetStockStorage(), 
			ItemSourceType.CraftStorageWarehouse => DomainManager.Extra.GetCraftStorage(), 
			ItemSourceType.MedicineStorageWarehouse => DomainManager.Extra.GetMedicineStorage(), 
			ItemSourceType.FoodStorageWarehouse => DomainManager.Extra.GetFoodStorage(), 
			_ => throw new ArgumentOutOfRangeException("sourceType", sourceType, null), 
		};
		if (1 == 0)
		{
		}
		TaiwuVillageStorage taiwuVillageStorage2 = taiwuVillageStorage;
		taiwuVillageStorage2.Resources.Add(resourceType, amount);
		taiwuVillageStorage2.NeedCommit = true;
	}

	public void AddResource(DataContext context, ItemSourceType sourceType, ref ResourceInts resources)
	{
		for (sbyte b = 0; b < 6; b++)
		{
			AddResource(context, sourceType, b, resources.Get(b));
		}
	}

	public void RemoveResource(DataContext context, ItemSourceType sourceType, sbyte resourceType, int amount)
	{
		if (amount <= 0)
		{
			Logger.Warn($"RemoveResource amount <= 0, sourceType: {sourceType}, resourceType: {resourceType}, amount: {amount}");
			return;
		}
		bool flag;
		switch (sourceType)
		{
		case ItemSourceType.Treasury:
		{
			SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
			taiwuTreasury.Resources.Subtract(resourceType, amount);
			SetTaiwuTreasury(context, taiwuTreasury);
			return;
		}
		case ItemSourceType.Inventory:
		case ItemSourceType.Resources:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			ResourceInts resources = GetTaiwu().GetResources();
			resources.Subtract(resourceType, amount);
			GetTaiwu().SetResources(ref resources, context);
			return;
		}
		if (1 == 0)
		{
		}
		TaiwuVillageStorage taiwuVillageStorage = sourceType switch
		{
			ItemSourceType.StockStorageWarehouse => DomainManager.Extra.GetStockStorage(), 
			ItemSourceType.CraftStorageWarehouse => DomainManager.Extra.GetCraftStorage(), 
			ItemSourceType.MedicineStorageWarehouse => DomainManager.Extra.GetMedicineStorage(), 
			ItemSourceType.FoodStorageWarehouse => DomainManager.Extra.GetFoodStorage(), 
			_ => throw new ArgumentOutOfRangeException("sourceType", sourceType, null), 
		};
		if (1 == 0)
		{
		}
		TaiwuVillageStorage taiwuVillageStorage2 = taiwuVillageStorage;
		taiwuVillageStorage2.Resources.Subtract(resourceType, amount);
		taiwuVillageStorage2.NeedCommit = true;
	}

	public void RemoveResource(DataContext context, ItemSourceType sourceType, ref ResourceInts resources)
	{
		for (sbyte b = 0; b < 6; b++)
		{
			RemoveResource(context, sourceType, b, resources.Get(b));
		}
	}

	[DomainMethod]
	public bool FindTaiwuBuilding(short buildingTemplateId, bool checkUsable = true)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData buildingAreaData = DomainManager.Building.GetBuildingAreaData(taiwuVillageLocation);
		BuildingBlockData buildingBlockData = BuildingDomain.FindBuilding(taiwuVillageLocation, buildingAreaData, buildingTemplateId, checkUsable);
		return buildingBlockData != null;
	}

	public BuildingBlockData GetTaiwuBuildingBlockData(short buildingTemplateId)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingAreaData buildingAreaData = DomainManager.Building.GetBuildingAreaData(taiwuVillageLocation);
		return BuildingDomain.FindBuilding(taiwuVillageLocation, buildingAreaData, buildingTemplateId);
	}

	public (int attainmentEffect, bool upgradeMakeItem, bool hasBuilding) GetTaiwuCraftBuildingEffect(sbyte lifeSkillType)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		short buildingBlockTemplateId = -1;
		BuildingBlock.Instance.Iterate(delegate(BuildingBlockItem b)
		{
			if (b.CanMakeItem && b.RequireLifeSkillType == lifeSkillType)
			{
				buildingBlockTemplateId = b.TemplateId;
				return false;
			}
			return true;
		});
		BuildingBlockData taiwuBuildingBlockData = DomainManager.Taiwu.GetTaiwuBuildingBlockData(buildingBlockTemplateId);
		if (taiwuBuildingBlockData != null)
		{
			BuildingBlockKey buildingBlockKey = new BuildingBlockKey(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, taiwuBuildingBlockData.BlockIndex);
			(int, bool) buildingEffectForMake = DomainManager.Building.GetBuildingEffectForMake(buildingBlockKey, lifeSkillType);
			return (attainmentEffect: buildingEffectForMake.Item1, upgradeMakeItem: buildingEffectForMake.Item2, hasBuilding: true);
		}
		return (attainmentEffect: 0, upgradeMakeItem: false, hasBuilding: false);
	}

	public void ReturnBuildingCoreItem(DataContext context, BuildingBlockItem configData)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (DomainManager.Taiwu.CanTransferItemToWarehouse(context))
		{
			taiwu.CreateInventoryItem(context, 12, configData.BuildingCoreItem, 1);
			return;
		}
		ItemKey itemKey = DomainManager.Item.CreateItem(context, 12, configData.BuildingCoreItem);
		DomainManager.Taiwu.WarehouseAdd(context, itemKey, 1);
	}

	[DomainMethod]
	public int GetItemCount(sbyte itemType, short itemTemplateId)
	{
		int num = 0;
		ItemKey[] equipment = _taiwuChar.GetEquipment();
		ItemKey[] array = equipment;
		for (int i = 0; i < array.Length; i++)
		{
			ItemKey itemKey = array[i];
			if (itemKey.ItemType == itemType && itemKey.TemplateId == itemTemplateId)
			{
				num++;
			}
		}
		num += _taiwuChar.GetInventory().GetInventoryItemCount(itemType, itemTemplateId);
		ItemSourceType[] taiwuVillageStorages = TaiwuVillageStorages;
		foreach (ItemSourceType itemSourceType in taiwuVillageStorages)
		{
			IReadOnlyDictionary<ItemKey, int> items = GetItems(itemSourceType);
			foreach (KeyValuePair<ItemKey, int> item in items)
			{
				if (item.Key.ItemType == itemType && item.Key.TemplateId == itemTemplateId)
				{
					num += item.Value;
				}
			}
		}
		return num;
	}

	[DomainMethod]
	[Obsolete]
	public Dictionary<sbyte, VillagerRoleActionSetting> GetAllVillagerRoleActionSetting(DataContext context)
	{
		return DomainManager.Extra.GetVillagerRoleActionSettingDict(context);
	}

	[DomainMethod]
	[Obsolete]
	public VillagerRoleActionSetting GetVillagerRoleActionSetting(DataContext context, TaiwuVillageStorageType storageType)
	{
		Dictionary<sbyte, VillagerRoleActionSetting> allVillagerRoleActionSetting = GetAllVillagerRoleActionSetting(context);
		return allVillagerRoleActionSetting.GetValueOrDefault((sbyte)storageType);
	}

	[DomainMethod]
	[Obsolete]
	public void SetVillagerRoleActionSetting(DataContext context, TaiwuVillageStorageType storageType, VillagerRoleActionSetting setting)
	{
		DomainManager.Extra.SetVillagerRoleActionSetting(context, storageType, setting);
	}

	[DomainMethod]
	public static int GetStrategyRoomLevel()
	{
		int result = 0;
		List<Location> taiwuBuildingAreas = DomainManager.Building.GetTaiwuBuildingAreas();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location elementId = taiwuBuildingAreas[i];
			BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(elementId);
			for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, num);
				BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
				if (element_BuildingBlocks.TemplateId == 98 && element_BuildingBlocks.CanUse() && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value))
				{
					return value.CalcUnlockedLevelCount();
				}
			}
		}
		return result;
	}

	[DomainMethod]
	public (int villageProvide, int spaceExtraAdd, int prosperousConstruction, int ResourceBlockEffect) GetTaiwuVillageSpaceLimitInfo()
	{
		int taiwuVillageBaseSpace = GetTaiwuVillageBaseSpace();
		Location taiwuVillageLocation = GetTaiwuVillageLocation();
		int buildingBlockEffect = DomainManager.Building.GetBuildingBlockEffect(taiwuVillageLocation, EBuildingScaleEffect.BuildingSpaceBonus);
		return (villageProvide: taiwuVillageBaseSpace, spaceExtraAdd: _buildingSpaceExtraAdd, prosperousConstruction: _prosperousConstruction ? 20 : 0, ResourceBlockEffect: buildingBlockEffect);
	}

	[DomainMethod]
	public unsafe void SelectCombatSkillAttainmentPanelPlan(DataContext context, sbyte combatSkillType, sbyte planId)
	{
		SaveCombatSkillAttainmentPanelPlan(context, combatSkillType, _currCombatSkillAttainmentPanelPlanIds[combatSkillType]);
		_currCombatSkillAttainmentPanelPlanIds[combatSkillType] = planId;
		SetCurrCombatSkillAttainmentPanelPlanIds(_currCombatSkillAttainmentPanelPlanIds, context);
		short[] combatSkillAttainmentPanels = _taiwuChar.GetCombatSkillAttainmentPanels();
		int num = 126;
		fixed (short* combatSkillAttainmentPanelPlans = _combatSkillAttainmentPanelPlans)
		{
			short* pCombatSkillTemplateIds = combatSkillAttainmentPanelPlans + num * planId + combatSkillType * 9;
			CombatSkillAttainmentPanelsHelper.SetPanel(combatSkillAttainmentPanels, combatSkillType, pCombatSkillTemplateIds);
		}
		for (sbyte b = 0; b < 9; b++)
		{
			short skillTemplateId = CombatSkillAttainmentPanelsHelper.Get(combatSkillAttainmentPanels, combatSkillType, b);
			CombatSkillKey objectId = new CombatSkillKey(_taiwuCharId, skillTemplateId);
			if (!DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out var element) || element.GetRevoked() || !CombatSkillStateHelper.IsBrokenOut(element.GetActivationState()))
			{
				CombatSkillAttainmentPanelsHelper.Set(combatSkillAttainmentPanels, combatSkillType, b, -1);
			}
		}
		QuickFillCombatSkillAttainmentPanel(context, combatSkillType);
	}

	private unsafe void SaveCombatSkillAttainmentPanelPlan(DataContext context, sbyte combatSkillType, sbyte planId)
	{
		short[] combatSkillAttainmentPanels = _taiwuChar.GetCombatSkillAttainmentPanels();
		int num = 126;
		fixed (short* combatSkillAttainmentPanelPlans = _combatSkillAttainmentPanelPlans)
		{
			short* pCombatSkillTemplateIds = combatSkillAttainmentPanelPlans + num * planId + combatSkillType * 9;
			CombatSkillAttainmentPanelsHelper.GetPanel(combatSkillAttainmentPanels, combatSkillType, pCombatSkillTemplateIds);
		}
		SetCombatSkillAttainmentPanelPlans(_combatSkillAttainmentPanelPlans, context);
	}

	private bool QuickFillCombatSkillAttainmentPanel(DataContext context, sbyte combatSkillType)
	{
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_taiwuCharId);
		List<KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill>> list = charCombatSkills.Where((KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> pair) => Config.CombatSkill.Instance[pair.Key].Type == combatSkillType && !pair.Value.GetRevoked() && CombatSkillStateHelper.IsBrokenOut(pair.Value.GetActivationState())).ToList();
		short[] combatSkillAttainmentPanels = _taiwuChar.GetCombatSkillAttainmentPanels();
		bool result = false;
		for (sbyte b = 0; b < 9; b++)
		{
			if (CombatSkillAttainmentPanelsHelper.Get(combatSkillAttainmentPanels, combatSkillType, b) == -1)
			{
				foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item in list)
				{
					short key = item.Key;
					if (Config.CombatSkill.Instance[key].Grade != b)
					{
						continue;
					}
					CombatSkillAttainmentPanelsHelper.Set(combatSkillAttainmentPanels, combatSkillType, b, key);
					result = true;
					break;
				}
			}
		}
		_taiwuChar.SetCombatSkillAttainmentPanels(combatSkillAttainmentPanels, context);
		return result;
	}

	[DomainMethod]
	public byte[] GetGenericGridAllocation()
	{
		CombatSkillPlan combatSkillPlan = _combatSkillPlans[_currCombatSkillPlanId];
		return combatSkillPlan.GenericGridAllocation;
	}

	public void SetGenericGridAllocation(DataContext context, byte[] genericGridAllocation)
	{
		CombatSkillPlan combatSkillPlan = _combatSkillPlans[_currCombatSkillPlanId];
		byte[] genericGridAllocation2 = combatSkillPlan.GenericGridAllocation;
		for (int i = 0; i < genericGridAllocation2.Length; i++)
		{
			genericGridAllocation2[i] = genericGridAllocation[i];
		}
		SetElement_CombatSkillPlans(_currCombatSkillPlanId, combatSkillPlan, context);
	}

	[DomainMethod]
	public bool MasteredSkillWillChangePlan(short skillTemplateId)
	{
		CombatSkillEquipment combatSkillEquipment = _taiwuChar.GetCombatSkillEquipment();
		CombatSkillPlan combatSkillPlan = _combatSkillPlans[_currCombatSkillPlanId];
		if (!combatSkillEquipment.IsCombatSkillEquipped(skillTemplateId))
		{
			return false;
		}
		Span<sbyte> slotCounts = stackalloc sbyte[5];
		sbyte combatSkillSlotCounts = _taiwuChar.GetCombatSkillSlotCounts(slotCounts);
		Span<sbyte> span = stackalloc sbyte[4];
		for (sbyte b = 1; b < 5; b++)
		{
			span[b - 1] = CharacterDomain.GetCombatSkillsCostedGrid(_taiwuCharId, combatSkillEquipment, b);
		}
		Span<sbyte> specificGrids = stackalloc sbyte[4];
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_taiwuCharId, skillTemplateId));
		element_CombatSkills.GetSpecificGridCount(specificGrids);
		for (sbyte b2 = 1; b2 < 5; b2++)
		{
			sbyte b3 = slotCounts[b2];
			sbyte b4 = span[b2 - 1];
			sbyte b5 = specificGrids[b2 - 1];
			if (b3 - b5 < b4)
			{
				return true;
			}
		}
		byte[] genericGridAllocation = combatSkillPlan.GenericGridAllocation;
		int num = genericGridAllocation.Sum();
		sbyte genericGridCount = element_CombatSkills.GetGenericGridCount();
		return combatSkillSlotCounts - genericGridCount < num;
	}

	[Obsolete]
	public unsafe void ClearExceedCombatSkills(DataContext context)
	{
		if (!DomainManager.Character.IsCharacterAlive(_taiwuCharId))
		{
			return;
		}
		CombatSkillPlan combatSkillPlan = _combatSkillPlans[_currCombatSkillPlanId];
		sbyte* pointer = stackalloc sbyte[5];
		Span<sbyte> slotCounts = new Span<sbyte>(pointer, 5);
		int num = _taiwuChar.GetCombatSkillSlotCounts(slotCounts);
		if (_taiwuChar.ClearExceededCombatSkills(context, 0, slotCounts[0]))
		{
			num = _taiwuChar.GetCombatSkillSlotCounts(slotCounts);
		}
		for (int i = 0; i < combatSkillPlan.GenericGridAllocation.Length; i++)
		{
			byte b = combatSkillPlan.GenericGridAllocation[i];
			if (num >= b)
			{
				num -= b;
				continue;
			}
			b = (byte)num;
			combatSkillPlan.GenericGridAllocation[i] = b;
			num = 0;
		}
		for (sbyte b2 = 1; b2 < 5; b2++)
		{
			_taiwuChar.ClearExceededCombatSkills(context, b2);
		}
		SetElement_CombatSkillPlans(_currCombatSkillPlanId, combatSkillPlan, context);
	}

	[DomainMethod]
	public void AllocateGenericGrid(DataContext context, sbyte equipType)
	{
		if (equipType < 1)
		{
			throw new ArgumentException($"Invalid generic grid type:{equipType}");
		}
		Span<sbyte> slotCounts = stackalloc sbyte[5];
		sbyte combatSkillSlotCounts = _taiwuChar.GetCombatSkillSlotCounts(slotCounts);
		CombatSkillPlan combatSkillPlan = _combatSkillPlans[_currCombatSkillPlanId];
		int num = equipType - 1;
		int genericAllocationNextCost = GameData.Domains.Character.CombatSkillHelper.GetGenericAllocationNextCost(equipType, combatSkillPlan.GenericGridAllocation[num]);
		int num2 = _taiwuChar.ApplyGenericCombatSkillSlotAllocations(slotCounts, combatSkillSlotCounts);
		if (genericAllocationNextCost > num2)
		{
			throw new InvalidOperationException("No more available generic grid");
		}
		combatSkillPlan.GenericGridAllocation[num]++;
		SetElement_CombatSkillPlans(_currCombatSkillPlanId, combatSkillPlan, context);
	}

	[DomainMethod]
	public void DeallocateGenericGrid(DataContext context, sbyte equipType)
	{
		if (equipType < 1)
		{
			throw new Exception($"Invalid generic grid type:{equipType}");
		}
		CombatSkillPlan combatSkillPlan = _combatSkillPlans[_currCombatSkillPlanId];
		if (combatSkillPlan.GenericGridAllocation[equipType - 1] == 0)
		{
			throw new Exception($"No generic grid allocated in type:{equipType}");
		}
		combatSkillPlan.GenericGridAllocation[equipType - 1]--;
		SetElement_CombatSkillPlans(_currCombatSkillPlanId, combatSkillPlan, context);
	}

	[DomainMethod]
	public void UpdateCombatSkillPlan(DataContext context, int index)
	{
		CombatSkillPlan combatSkillPlan = _combatSkillPlans[_currCombatSkillPlanId];
		if (_currCombatSkillPlanId != index)
		{
			combatSkillPlan.Record(_taiwuChar);
			SetElement_CombatSkillPlans(_currCombatSkillPlanId, combatSkillPlan, context);
			SetCurrCombatSkillPlanId(index, context);
			combatSkillPlan = _combatSkillPlans[_currCombatSkillPlanId];
		}
		DomainManager.Extra.UpdateTaiwuMasteredCombatSkillsByPlan(context);
		DomainManager.Character.ApplyCombatSkillPlan(context, _taiwuChar, combatSkillPlan);
		SetElement_CombatSkillPlans(_currCombatSkillPlanId, combatSkillPlan, context);
	}

	[DomainMethod]
	public void CopyCombatSkillPlan(DataContext context)
	{
		byte unlockedCombatSkillPlanCount = DomainManager.Extra.GetUnlockedCombatSkillPlanCount();
		if (unlockedCombatSkillPlanCount != 9)
		{
			DomainManager.Extra.SetUnlockedCombatSkillPlanCount((byte)(unlockedCombatSkillPlanCount + 1), context);
			CombatSkillPlan combatSkillPlan = _combatSkillPlans[_currCombatSkillPlanId];
			combatSkillPlan.Record(_taiwuChar);
			SetElement_CombatSkillPlans(_currCombatSkillPlanId, combatSkillPlan, context);
			CombatSkillPlan combatSkillPlan2 = _combatSkillPlans[unlockedCombatSkillPlanCount];
			combatSkillPlan2.Assign(combatSkillPlan);
			SetElement_CombatSkillPlans(unlockedCombatSkillPlanCount, combatSkillPlan2, context);
			DomainManager.Extra.CopyTaiwuCombatSkillsPlan(context, _currCombatSkillPlanId, unlockedCombatSkillPlanCount);
			UpdateCombatSkillPlan(context, unlockedCombatSkillPlanCount);
		}
	}

	[DomainMethod]
	public void AppendCombatSkillPlan(DataContext context)
	{
		byte unlockedCombatSkillPlanCount = DomainManager.Extra.GetUnlockedCombatSkillPlanCount();
		if (unlockedCombatSkillPlanCount != 9)
		{
			DomainManager.Extra.SetUnlockedCombatSkillPlanCount((byte)(unlockedCombatSkillPlanCount + 1), context);
			CombatSkillPlan combatSkillPlan = _combatSkillPlans[unlockedCombatSkillPlanCount];
			combatSkillPlan.Reset();
			SetElement_CombatSkillPlans(unlockedCombatSkillPlanCount, combatSkillPlan, context);
			UpdateCombatSkillPlan(context, unlockedCombatSkillPlanCount);
		}
	}

	[DomainMethod]
	public void ClearCombatSkillPlan(DataContext context)
	{
		DomainManager.Character.UnequipAllCombatSkills(context, _taiwuCharId);
	}

	[DomainMethod]
	public void DeleteCombatSkillPlan(DataContext context)
	{
		byte unlockedCombatSkillPlanCount = DomainManager.Extra.GetUnlockedCombatSkillPlanCount();
		if (unlockedCombatSkillPlanCount != 1)
		{
			DomainManager.Extra.SetUnlockedCombatSkillPlanCount((byte)(unlockedCombatSkillPlanCount - 1), context);
			CombatSkillPlan combatSkillPlan = _combatSkillPlans[_currCombatSkillPlanId];
			combatSkillPlan.Reset();
			for (int i = _currCombatSkillPlanId; i < unlockedCombatSkillPlanCount - 1; i++)
			{
				SetElement_CombatSkillPlans(i, _combatSkillPlans[i + 1], context);
			}
			SetElement_CombatSkillPlans(unlockedCombatSkillPlanCount - 1, combatSkillPlan, context);
			DomainManager.Extra.DeleteTaiwuCombatSkillsPlan(context, _currCombatSkillPlanId);
			if (_currCombatSkillPlanId != unlockedCombatSkillPlanCount - 1)
			{
				UpdateCombatSkillPlan(context, _currCombatSkillPlanId);
				return;
			}
			ClearCombatSkillPlan(context);
			UpdateCombatSkillPlan(context, _currCombatSkillPlanId - 1);
		}
	}

	public void RegisterCombatSkill(DataContext context, GameData.Domains.CombatSkill.CombatSkill skill)
	{
		short skillTemplateId = skill.GetId().SkillTemplateId;
		ushort readingState = skill.GetReadingState();
		if (_notLearnCombatSkillReadingProgress.TryGetValue(skillTemplateId, out var value))
		{
			for (byte b = 0; b < 15; b++)
			{
				if (CombatSkillStateHelper.IsPageRead(readingState, b))
				{
					value.SetBookPageReadingProgress(b, 100);
				}
			}
			RemoveElement_NotLearnCombatSkillReadingProgress(skillTemplateId, context);
			AddElement_CombatSkills(skillTemplateId, value, context);
		}
		else if (!_combatSkills.ContainsKey(skillTemplateId))
		{
			TaiwuCombatSkill value2 = new TaiwuCombatSkill(readingState);
			AddElement_CombatSkills(skillTemplateId, value2, context);
		}
		else
		{
			AdaptableLog.Warning("Combat skill " + Config.CombatSkill.Instance[skillTemplateId].Name + " is already learned.", appendWarningMessage: true);
		}
	}

	public void UnregisterCombatSkill(DataContext context, short skillTemplateId)
	{
		RemoveElement_CombatSkills(skillTemplateId, context);
	}

	public void RegisterLifeSkill(DataContext context, GameData.Domains.Character.LifeSkillItem skill)
	{
		short skillTemplateId = skill.SkillTemplateId;
		byte readingState = skill.ReadingState;
		if (_notLearnLifeSkillReadingProgress.TryGetValue(skillTemplateId, out var value))
		{
			for (byte b = 0; b < 5; b++)
			{
				if (CombatSkillStateHelper.IsPageRead(readingState, b))
				{
					value.SetBookPageReadingProgress(b, 100);
				}
			}
			RemoveElement_NotLearnLifeSkillReadingProgress(skillTemplateId, context);
			AddElement_LifeSkills(skillTemplateId, value, context);
		}
		else if (!_lifeSkills.ContainsKey(skillTemplateId))
		{
			TaiwuLifeSkill value2 = new TaiwuLifeSkill(readingState);
			AddElement_LifeSkills(skillTemplateId, value2, context);
		}
		else
		{
			AdaptableLog.Warning("Life skill " + LifeSkill.Instance[skillTemplateId].Name + " is already learned.", appendWarningMessage: true);
		}
	}

	public void UnregisterLifeSkill(DataContext context, short skillTemplateId)
	{
		RemoveElement_LifeSkills(skillTemplateId, context);
	}

	public void RegisterProtagonistLifeSkillsAndCombatSkills(DataContext context, List<GameData.Domains.Character.LifeSkillItem> lifeSkills, List<GameData.Domains.CombatSkill.CombatSkill> combatSkills, short readLifeSkillTemplateId, short readCombatSkillTemplateId, byte combatSkillBookPageTypes)
	{
		int i = 0;
		for (int count = lifeSkills.Count; i < count; i++)
		{
			RegisterLifeSkill(context, lifeSkills[i]);
		}
		int j = 0;
		for (int count2 = combatSkills.Count; j < count2; j++)
		{
			RegisterCombatSkill(context, combatSkills[j]);
		}
		if (readLifeSkillTemplateId >= 0)
		{
			TaiwuLifeSkill element_LifeSkills = GetElement_LifeSkills(readLifeSkillTemplateId);
			for (byte b = 0; b < 5; b++)
			{
				element_LifeSkills.SetBookPageReadingProgress(b, 50);
			}
			SetElement_LifeSkills(readLifeSkillTemplateId, element_LifeSkills, context);
		}
		if (readCombatSkillTemplateId >= 0)
		{
			TaiwuCombatSkill element_CombatSkills = GetElement_CombatSkills(readCombatSkillTemplateId);
			sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(combatSkillBookPageTypes);
			byte outlinePageInternalIndex = CombatSkillStateHelper.GetOutlinePageInternalIndex(outlinePageType);
			element_CombatSkills.SetBookPageReadingProgress(outlinePageInternalIndex, 50);
			for (byte b2 = 1; b2 < 6; b2++)
			{
				sbyte normalPageType = SkillBookStateHelper.GetNormalPageType(combatSkillBookPageTypes, b2);
				outlinePageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(normalPageType, b2);
				element_CombatSkills.SetBookPageReadingProgress(outlinePageInternalIndex, 50);
			}
			SetElement_CombatSkills(readCombatSkillTemplateId, element_CombatSkills, context);
		}
	}

	public ItemKey GetCombatSkillBookAndRead(DataContext context, short combatSkillTemplateId, sbyte progress)
	{
		GameData.Domains.Character.Character taiwu = GetTaiwu();
		short bookId = Config.CombatSkill.Instance[combatSkillTemplateId].BookId;
		sbyte behaviorType = taiwu.GetBehaviorType();
		ItemKey itemKey = DomainManager.Item.CreateSkillBook(context, bookId, -1, -1, behaviorType, 50);
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(itemKey.Id);
		if (!taiwu.GetLearnedCombatSkills().Contains(combatSkillTemplateId))
		{
			DomainManager.Character.LearnCombatSkill(context, _taiwuCharId, combatSkillTemplateId, 0);
		}
		TaiwuCombatSkill taiwuCombatSkill = GetTaiwuCombatSkill(combatSkillTemplateId);
		byte outlinePageInternalIndex = CombatSkillStateHelper.GetOutlinePageInternalIndex(behaviorType);
		sbyte bookPageReadingProgress = taiwuCombatSkill.GetBookPageReadingProgress(outlinePageInternalIndex);
		taiwuCombatSkill.SetBookPageReadingProgress(outlinePageInternalIndex, Math.Max(progress, bookPageReadingProgress));
		byte pageTypes = element_SkillBooks.GetPageTypes();
		for (byte b = 1; b < 6; b++)
		{
			sbyte normalPageType = SkillBookStateHelper.GetNormalPageType(pageTypes, b);
			outlinePageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(normalPageType, b);
			bookPageReadingProgress = taiwuCombatSkill.GetBookPageReadingProgress(outlinePageInternalIndex);
			taiwuCombatSkill.SetBookPageReadingProgress(outlinePageInternalIndex, Math.Max(progress, bookPageReadingProgress));
		}
		SetTaiwuCombatSkill(context, combatSkillTemplateId, taiwuCombatSkill);
		taiwu.AddInventoryItem(context, itemKey, 1);
		return itemKey;
	}

	public ItemKey GetLifeSkillBookAndRead(DataContext context, short lifeSkillTemplateId, sbyte progress)
	{
		GameData.Domains.Character.Character taiwu = GetTaiwu();
		short skillBookId = LifeSkill.Instance[lifeSkillTemplateId].SkillBookId;
		ItemKey itemKey = DomainManager.Item.CreateSkillBook(context, skillBookId, -1, -1, -1, 50);
		if (taiwu.FindLearnedLifeSkillIndex(lifeSkillTemplateId) < 0)
		{
			DomainManager.Character.LearnLifeSkill(context, _taiwuCharId, lifeSkillTemplateId, 0);
		}
		TaiwuLifeSkill taiwuLifeSkill = GetTaiwuLifeSkill(lifeSkillTemplateId);
		for (byte b = 0; b < 5; b++)
		{
			sbyte bookPageReadingProgress = taiwuLifeSkill.GetBookPageReadingProgress(b);
			taiwuLifeSkill.SetBookPageReadingProgress(b, Math.Max(bookPageReadingProgress, progress));
		}
		SetTaiwuLifeSkill(context, lifeSkillTemplateId, taiwuLifeSkill);
		taiwu.AddInventoryItem(context, itemKey, 1);
		return itemKey;
	}

	public bool TryGetTaiwuLifeSkill(short skillTemplateId, out TaiwuLifeSkill lifeSkill)
	{
		return _lifeSkills.TryGetValue(skillTemplateId, out lifeSkill);
	}

	public bool TryGetNotLearnLifeSkillReadingProgress(short skillTemplateId, out TaiwuLifeSkill notLearnLifeSkill)
	{
		return _notLearnLifeSkillReadingProgress.TryGetValue(skillTemplateId, out notLearnLifeSkill);
	}

	public void TaiwuLearnLifeSkill(DataContext context, short skillTemplateId, byte readingState = 0)
	{
		DomainManager.Character.LearnLifeSkill(context, _taiwuCharId, skillTemplateId, readingState);
		if (_notLearnLifeSkillReadingProgress.ContainsKey(skillTemplateId))
		{
			SetElement_LifeSkills(skillTemplateId, GetElement_NotLearnLifeSkillReadingProgress(skillTemplateId), context);
			RemoveElement_NotLearnLifeSkillReadingProgress(skillTemplateId, context);
		}
	}

	public bool TryGetTaiwuCombatSkill(short skillTemplateId, out TaiwuCombatSkill combatSkill)
	{
		return _combatSkills.TryGetValue(skillTemplateId, out combatSkill);
	}

	public bool TryGetNotLearnCombatSkillReadingProgress(short skillTemplateId, out TaiwuCombatSkill notLearnCombatSkill)
	{
		return _notLearnCombatSkillReadingProgress.TryGetValue(skillTemplateId, out notLearnCombatSkill);
	}

	public void TaiwuLearnCombatSkill(DataContext context, short skillTemplateId, ushort readingState = 0)
	{
		DomainManager.Character.LearnCombatSkill(context, _taiwuCharId, skillTemplateId, readingState);
		if (_notLearnCombatSkillReadingProgress.ContainsKey(skillTemplateId))
		{
			SetElement_CombatSkills(skillTemplateId, GetElement_NotLearnCombatSkillReadingProgress(skillTemplateId), context);
			RemoveElement_NotLearnCombatSkillReadingProgress(skillTemplateId, context);
		}
	}

	[DomainMethod]
	public bool SetActivePage(DataContext context, short skillId, byte pageId, sbyte direction)
	{
		return DomainManager.CombatSkill.SetActivePage(context, _taiwuCharId, skillId, pageId, direction);
	}

	[DomainMethod]
	public SkillBreakPlate GetBreakPlateData(short skillTemplateId)
	{
		SkillBreakPlate value;
		return DomainManager.Extra.TryGetElement_SkillBreakPlates(skillTemplateId, out value) ? value : null;
	}

	[DomainMethod]
	public SkillBreakPlate EnterSkillBreakPlate(DataContext context, short skillId, ushort selectedPages)
	{
		if (!DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: _taiwuCharId, skillId: skillId), out var element))
		{
			throw new Exception($"Skill {skillId} not learned");
		}
		if (!element.CanBreakout())
		{
			throw new Exception($"Skill {skillId} cannot break out");
		}
		if (CombatSkillStateHelper.GetActiveOutlinePageType(selectedPages) == -1 || CombatSkillStateHelper.GetReadNormalPagesCount(selectedPages) != 5)
		{
			throw new Exception($"Skill {skillId} selected break out page {selectedPages} not correct");
		}
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		if (!DomainManager.Extra.TryGetElement_SkillBreakPlates(skillId, out var value))
		{
			SkillBreakPlateItem config = combatSkillItem.SkillBreakPlate;
			if (DomainManager.TutorialChapter.InGuiding && skillId == 706)
			{
				config = Config.SkillBreakPlate.Instance[(sbyte)9];
			}
			(int succeedCount, int failCount) clearedSkillPlateStepInfo = DomainManager.Extra.GetClearedSkillPlateStepInfo(skillId);
			int item = clearedSkillPlateStepInfo.succeedCount;
			int item2 = clearedSkillPlateStepInfo.failCount;
			value = new SkillBreakPlate(context.Random, config, selectedPages, item, item2);
			DomainManager.Extra.RemoveClearedSkillPlateStepInfo(context, skillId);
		}
		value.StepBase = _taiwuChar.GetSkillBreakoutAvailableStepsCount(skillId);
		value.BaseSuccessRate = CalcTaiwuBreakBaseSuccessRate(combatSkillItem);
		DomainManager.Extra.SetOrAddSkillBreakPlate(context, skillId, value);
		return value;
	}

	[DomainMethod]
	public void ClearBreakPlate(DataContext context, short skillId, bool fromGmCmd = false)
	{
		int succeedCount = 0;
		int failCount = 0;
		if (_skillBreakPlateObsoleteDict.TryGetValue(skillId, out var value))
		{
			RemoveElement_SkillBreakPlateObsoleteDict(skillId, context);
			RemoveElement_SkillBreakBonusDict(skillId, context);
			(succeedCount, failCount) = value.GetSucceedAndFailCount(exceptBonusAndPrev: true);
		}
		if (DomainManager.Extra.TryGetElement_SkillBreakPlates(skillId, out var value2))
		{
			DomainManager.Extra.RemoveSkillBreakPlate(context, skillId);
			succeedCount = value2.SuccessCount;
			failCount = value2.FailedCount;
		}
		DomainManager.Extra.AddClearedSkillPlateStepInfo(context, skillId, succeedCount, failCount);
		if (_taiwuChar.GetLoopingNeigong() == skillId)
		{
			_taiwuChar.SetLoopingNeigong(-1, context);
		}
		if (DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: _taiwuCharId, skillId: skillId), out var element))
		{
			element.SetActivationState(0, context);
		}
		CharacterDomain.TryRemoveCombatSkillFromAttainmentPanel(context, _taiwuChar, skillId);
		if (TryGetElement_CombatSkills(skillId, out var value3))
		{
			if (!fromGmCmd)
			{
				value3.LastClearBreakPlateTime = DomainManager.World.GetCurrDate();
			}
			value3.FullPowerCastTimes = 0;
			SetElement_CombatSkills(skillId, value3, context);
		}
		if (_taiwuChar.IsCombatSkillEquipped(skillId))
		{
			DomainManager.SpecialEffect.Remove(context, _taiwuCharId, skillId, 2);
		}
	}

	[DomainMethod]
	public SkillBreakPlate SelectSkillBreakGrid(DataContext context, short skillId, SkillBreakPlateIndex index)
	{
		SkillBreakPlate element_SkillBreakPlates = DomainManager.Extra.GetElement_SkillBreakPlates(skillId);
		int breakBaseCostExp = GetBreakBaseCostExp(skillId);
		int num = element_SkillBreakPlates.CalcCostExp(breakBaseCostExp, index);
		if (_taiwuChar.GetExp() < num)
		{
			return null;
		}
		if (!element_SkillBreakPlates.SelectBreak(context.Random, index, out var selectInGoneMad))
		{
			return null;
		}
		_taiwuChar.ChangeExp(context, -num);
		CombatSkillItem config = Config.CombatSkill.Instance[skillId];
		if (element_SkillBreakPlates.Success)
		{
			FinishBreak(context, skillId, element_SkillBreakPlates);
		}
		SkillBreakGridTypeItem skillBreakGridTypeItem = SkillBreakGridType.Instance[element_SkillBreakPlates[index].TemplateId];
		if (skillBreakGridTypeItem.HealthCost != 0)
		{
			_taiwuChar.ChangeHealth(context, -skillBreakGridTypeItem.HealthCost);
		}
		bool flag = DomainManager.SpecialEffect.ModifyData(_taiwuCharId, skillId, 267, dataValue: false);
		if (selectInGoneMad || flag)
		{
			Injuries injuries = _taiwuChar.GetInjuries();
			short disorderOfQi = 0;
			int num2 = GameData.Domains.Character.CombatSkillHelper.CalcForceBreakoutInjuriesAndDisorderOfQi(context.Random, config, ref injuries, ref disorderOfQi);
			_taiwuChar.SetInjuries(injuries, context);
			_taiwuChar.ChangeDisorderOfQiRandomRecovery(context, disorderOfQi);
			int num3 = -(num2 * GlobalConfig.Instance.ReduceHealthPerFatalDamageMark[1]);
			num3 += DisorderLevelOfQi.GetDisorderLevelOfQiConfig(_taiwuChar.GetDisorderOfQi()).BreakCostHealth;
			_taiwuChar.ChangeHealth(context, num3);
		}
		DomainManager.Extra.SetOrAddSkillBreakPlate(context, skillId, element_SkillBreakPlates);
		return element_SkillBreakPlates;
	}

	[DomainMethod]
	public int GetBreakBaseCostExp(short skillId)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		sbyte index = combatSkillItem.Grade;
		if (DomainManager.TutorialChapter.InGuiding)
		{
			index = 0;
		}
		return Config.SkillBreakPlate.Instance[index].CostExp;
	}

	[DomainMethod]
	public List<SkillBreakPlateBonus> GetAvailableRelationBonuses(short skillId)
	{
		HashSet<RelationKey> hashSet = new HashSet<RelationKey>();
		HashSet<RelationKey> hashSet2 = new HashSet<RelationKey>();
		HashSet<RelationKey> hashSet3 = new HashSet<RelationKey>();
		foreach (SkillBreakPlate conflictRelationSkillBreakPlate in DomainManager.Extra.GetConflictRelationSkillBreakPlates(skillId))
		{
			foreach (SkillBreakPlateBonus item4 in conflictRelationSkillBreakPlate.GetBonusesWithoutCheck())
			{
				if (item4.RelationType == 16384)
				{
					hashSet.Add(item4.RelationKey);
				}
				if (item4.RelationType == 32768)
				{
					hashSet2.Add(item4.RelationKey);
				}
				if (item4.Type == ESkillBreakPlateBonusType.Friend)
				{
					hashSet3.Add(item4.RelationKey);
				}
			}
		}
		List<SkillBreakPlateBonus> list = new List<SkillBreakPlateBonus>();
		RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(_taiwuCharId);
		foreach (int item5 in relatedCharacters.Adored.GetCollection())
		{
			if (DomainManager.Character.IsCharacterAlive(item5))
			{
				SkillBreakPlateBonus item = SkillBreakPlateBonusHelper.CreateRelation(item5, _taiwuCharId, 16384);
				if (!item.ShouldBeRemoved() && !hashSet.Contains(item.RelationKey))
				{
					list.Add(item);
				}
			}
		}
		foreach (int item6 in relatedCharacters.Enemies.GetCollection())
		{
			if (DomainManager.Character.IsCharacterAlive(item6))
			{
				SkillBreakPlateBonus item2 = SkillBreakPlateBonusHelper.CreateRelation(item6, _taiwuCharId, 32768);
				if (!item2.ShouldBeRemoved() && !hashSet2.Contains(item2.RelationKey))
				{
					list.Add(item2);
				}
			}
		}
		HashSet<int> hashSet4 = ObjectPool<HashSet<int>>.Instance.Get();
		hashSet4.Clear();
		relatedCharacters.GetAllTwoWayRelatedCharIds(hashSet4);
		foreach (int item7 in hashSet4)
		{
			if (DomainManager.Character.IsCharacterAlive(item7))
			{
				SkillBreakPlateBonus item3 = SkillBreakPlateBonusHelper.CreateFriend(item7, _taiwuCharId, skillId);
				if (!item3.ShouldBeRemoved() && !hashSet3.Contains(item3.RelationKey))
				{
					list.Add(item3);
				}
			}
		}
		ObjectPool<HashSet<int>>.Instance.Return(hashSet4);
		return list;
	}

	[DomainMethod]
	public SkillBreakPlate ClearBonus(DataContext context, short skillId, SkillBreakPlateIndex index)
	{
		if (!DomainManager.Extra.TryGetElement_SkillBreakPlates(skillId, out var value))
		{
			return null;
		}
		if (!value.ClearBonus(index))
		{
			return null;
		}
		DomainManager.Extra.SetOrAddSkillBreakPlate(context, skillId, value);
		DomainManager.Extra.UpdateSkillActivationState(context, skillId, value);
		return value;
	}

	[DomainMethod]
	public SkillBreakPlate SetBonusItem(DataContext context, short skillId, SkillBreakPlateIndex index, ItemKey itemKey, sbyte itemSourceType)
	{
		if (!DomainManager.Extra.TryGetElement_SkillBreakPlates(skillId, out var value))
		{
			return null;
		}
		if (!value.SetBonus(index, SkillBreakPlateBonusHelper.CreateItem(itemKey)))
		{
			return null;
		}
		RemoveItem(context, itemKey, 1, itemSourceType, deleteItem: true);
		DomainManager.Extra.SetOrAddSkillBreakPlate(context, skillId, value);
		DomainManager.Extra.UpdateSkillActivationState(context, skillId, value);
		return value;
	}

	[DomainMethod]
	public SkillBreakPlate SetBonusRelation(DataContext context, short skillId, SkillBreakPlateIndex index, int charId, ushort relationType)
	{
		if (charId == _taiwuCharId)
		{
			return null;
		}
		if (!DomainManager.Extra.TryGetElement_SkillBreakPlates(skillId, out var value))
		{
			return null;
		}
		if (!value.SetBonus(index, SkillBreakPlateBonusHelper.CreateRelation(charId, _taiwuCharId, relationType)))
		{
			return null;
		}
		DomainManager.Extra.SetOrAddSkillBreakPlate(context, skillId, value);
		DomainManager.Extra.UpdateSkillActivationState(context, skillId, value);
		return value;
	}

	[DomainMethod]
	public SkillBreakPlate SetBonusFriend(DataContext context, short skillId, SkillBreakPlateIndex index, int charId)
	{
		if (charId == _taiwuCharId)
		{
			return null;
		}
		if (!DomainManager.Extra.TryGetElement_SkillBreakPlates(skillId, out var value))
		{
			return null;
		}
		if (!value.SetBonus(index, SkillBreakPlateBonusHelper.CreateFriend(charId, _taiwuCharId, skillId)))
		{
			return null;
		}
		DomainManager.Extra.SetOrAddSkillBreakPlate(context, skillId, value);
		DomainManager.Extra.UpdateSkillActivationState(context, skillId, value);
		return value;
	}

	[DomainMethod]
	public SkillBreakPlate SetBonusExp(DataContext context, short skillId, SkillBreakPlateIndex index, int expLevel)
	{
		if (expLevel < 0 || expLevel >= SkillBreakPlateConstants.ExpLevelValues.Count)
		{
			return null;
		}
		int num = SkillBreakPlateConstants.ExpLevelValues[expLevel];
		if (_taiwuChar.GetExp() < num)
		{
			return null;
		}
		if (!DomainManager.Extra.TryGetElement_SkillBreakPlates(skillId, out var value))
		{
			return null;
		}
		if (!value.SetBonus(index, SkillBreakPlateBonusHelper.CreateExp(expLevel)))
		{
			return null;
		}
		_taiwuChar.ChangeExp(context, -num);
		DomainManager.Extra.SetOrAddSkillBreakPlate(context, skillId, value);
		DomainManager.Extra.UpdateSkillActivationState(context, skillId, value);
		return value;
	}

	public bool GetAutoBreakPlate(DataContext context, short skillId, out SkillBreakPlate plate, out SkillBreakPlateObsolete obsoletePlate)
	{
		bool flag = DomainManager.Extra.TryGetElement_SkillBreakPlates(skillId, out plate);
		bool flag2 = TryGetElement_SkillBreakPlateObsoleteDict(skillId, out obsoletePlate);
		if (flag && flag2)
		{
			RemoveElement_SkillBreakPlateObsoleteDict(skillId, context);
		}
		if (flag)
		{
			obsoletePlate = null;
		}
		return flag || flag2;
	}

	public bool UpdateBreakPlateSelectedPages(DataContext context, short skillId, ushort activationState)
	{
		bool result = false;
		if (_skillBreakPlateObsoleteDict.TryGetValue(skillId, out var value))
		{
			value.SelectedPages = activationState;
			SetElement_SkillBreakPlateObsoleteDict(skillId, value, context);
			result = true;
		}
		if (DomainManager.Extra.TryGetElement_SkillBreakPlates(skillId, out var value2) && value2.UpdateSelectedPages(activationState))
		{
			DomainManager.Extra.SetOrAddSkillBreakPlate(context, skillId, value2);
			result = true;
		}
		return result;
	}

	public bool TryGetSkillBreakPlate(short skillId, out SkillBreakPlate breakPlate)
	{
		return DomainManager.Extra.TryGetElement_SkillBreakPlates(skillId, out breakPlate);
	}

	public void OverwriteBreakPlateWithConflictCombatSkill(DataContext context, ConflictCombatSkill conflictCombatSkill)
	{
		short templateId = conflictCombatSkill.TemplateId;
		if (conflictCombatSkill.BreakPlate != null)
		{
			DomainManager.Extra.SetOrAddSkillBreakPlate(context, templateId, conflictCombatSkill.BreakPlate);
			return;
		}
		SetElement_SkillBreakPlateObsoleteDict(templateId, conflictCombatSkill.BreakPlateObsolete, context);
		SkillBreakBonusCollection bonusCollection = conflictCombatSkill.BonusCollection ?? new SkillBreakBonusCollection();
		conflictCombatSkill.BreakPlateObsolete.CalcSkillBreakBonusCollection(bonusCollection);
		SetElement_SkillBreakBonusDict(templateId, conflictCombatSkill.BonusCollection, context);
	}

	public SkillBreakBonusCollection GetBreakGridBonusCollection(short skillTemplateId)
	{
		_skillBreakBonusDict.TryGetValue(skillTemplateId, out var value);
		return value;
	}

	private byte CalcTaiwuBreakBaseSuccessRate(CombatSkillItem skillConfig)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		int num = 30 + (9 - skillConfig.Grade) * 5;
		if (skillConfig.SectId != 0)
		{
			short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(skillConfig.SectId);
			Sect element_Sects = DomainManager.Organization.GetElement_Sects(settlementIdByOrgTemplateId);
			short num2 = element_Sects.CalcApprovingRate();
			if (num2 >= 500)
			{
				num += 10;
			}
		}
		num += Math.Min(_taiwuChar.GetMaxMainAttributes()[5] / 10, 30);
		int index = ((!_taiwuChar.IsLoseConsummateBonusByFeature()) ? _taiwuChar.GetConsummateLevel() : 0);
		num += ConsummateLevel.Instance[index].AddBreakSuccessRate;
		Location location = _taiwuChar.GetLocation();
		int buildingBlockEffect = DomainManager.Building.GetBuildingBlockEffect(location, EBuildingScaleEffect.BreakOutSuccessRate, skillConfig.Type);
		if (buildingBlockEffect != 0)
		{
			num *= CValuePercentBonus.op_Implicit(buildingBlockEffect);
		}
		if (location.IsValid())
		{
			AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
			if (adventuresInArea.AdventureSites.TryGetValue(location.BlockId, out var value) && value.SiteState >= 2 && value.TemplateId == 43)
			{
				num += 10;
			}
		}
		byte breakoutDifficulty = DomainManager.Extra.GetBreakoutDifficulty();
		short num3 = WorldCreation.Instance[(byte)9].InfluenceFactors[breakoutDifficulty];
		num += num3;
		return (byte)Math.Clamp(num, 0, 100);
	}

	private unsafe void FinishBreak(DataContext context, short skillTemplateId, SkillBreakPlate plate)
	{
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills((charId: _taiwuCharId, skillId: skillTemplateId));
		element_CombatSkills.SetActivationState(plate.SelectedPages, context);
		int num = plate.StepCostedGoneMad;
		if (num < 0)
		{
			num = 0;
		}
		element_CombatSkills.SetForcedBreakoutStepsCount((sbyte)num, context);
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
		short[] combatSkillAttainmentPanels = _taiwuChar.GetCombatSkillAttainmentPanels();
		short* ptr = stackalloc short[9];
		CombatSkillAttainmentPanelsHelper.GetPanel(combatSkillAttainmentPanels, combatSkillItem.Type, ptr);
		if (ptr[combatSkillItem.Grade] < 0)
		{
			CombatSkillAttainmentPanelsHelper.Set(combatSkillAttainmentPanels, combatSkillItem.Type, combatSkillItem.Grade, skillTemplateId);
			_taiwuChar.SetCombatSkillAttainmentPanels(combatSkillAttainmentPanels, context);
		}
		if (_taiwuChar.IsCombatSkillEquipped(skillTemplateId))
		{
			DomainManager.SpecialEffect.UpdateEquippedSkillEffect(context, _taiwuChar);
		}
		AddLegacyPoint(context, 24);
	}

	public void AddFullPowerCastTimes(DataContext context, short skillTemplateId)
	{
		if (TryGetElement_CombatSkills(skillTemplateId, out var value) && value.FullPowerCastTimes < TaiwuCombatSkill.FullPowerAddBreakSuccessRate.Length)
		{
			value.FullPowerCastTimes++;
			SetElement_CombatSkills(skillTemplateId, value, context);
		}
	}

	public void ChangeCurrBreakPlate(DataContext context, short skillId, SkillBreakPlate plate, SkillBreakPlateObsolete plateObsolete, int lastClearTime, int lastForceBreakoutStepsCount)
	{
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_taiwuCharId, skillId));
		TaiwuCombatSkill element_CombatSkills2 = GetElement_CombatSkills(skillId);
		ChangeCurrBreakPlateBasic(context, skillId, plate, plateObsolete);
		ChangeCurrBreakPlateBonus(context, skillId, plateObsolete);
		if ((plate == null || !plate.Success) && !(plateObsolete?.Finished ?? false))
		{
			if (_taiwuChar.GetLoopingNeigong() == skillId)
			{
				_taiwuChar.SetLoopingNeigong(-1, context);
			}
			CharacterDomain.TryRemoveCombatSkillFromAttainmentPanel(context, _taiwuChar, skillId);
		}
		element_CombatSkills2.LastClearBreakPlateTime = lastClearTime;
		SetElement_CombatSkills(skillId, element_CombatSkills2, context);
		element_CombatSkills.SetForcedBreakoutStepsCount((sbyte)lastForceBreakoutStepsCount, context);
		ushort activationState = element_CombatSkills.GetActivationState();
		ushort activationState2 = (ushort)((plate != null && plate.Success) ? plate.SelectedPages : ((plateObsolete != null && plateObsolete.Finished) ? plateObsolete.SelectedPages : 0));
		element_CombatSkills.SetActivationState(activationState2, context);
		if (element_CombatSkills.Template.EquipType == 0)
		{
			_taiwuChar.UpdateAllocatedGenericGrids(context);
		}
		if (_taiwuChar.IsCombatSkillEquipped(skillId) && _taiwuChar.GetCombatSkillCanAffect(skillId))
		{
			if (CombatSkillStateHelper.IsBrokenOut(activationState))
			{
				DomainManager.SpecialEffect.Remove(context, _taiwuCharId, skillId, 2);
			}
			if (CombatSkillStateHelper.IsBrokenOut(activationState2))
			{
				DomainManager.SpecialEffect.Add(context, _taiwuCharId, skillId, 2, -1);
			}
		}
	}

	private void ChangeCurrBreakPlateBasic(DataContext context, short skillId, SkillBreakPlate plate, SkillBreakPlateObsolete plateObsolete)
	{
		SkillBreakPlateObsolete value;
		bool flag = TryGetElement_SkillBreakPlateObsoleteDict(skillId, out value);
		if (TryGetSkillBreakPlate(skillId, out var _) && plate == null)
		{
			DomainManager.Extra.RemoveSkillBreakPlate(context, skillId);
		}
		if (flag && plateObsolete == null)
		{
			RemoveElement_SkillBreakPlateObsoleteDict(skillId, context);
		}
		if (plate != null)
		{
			DomainManager.Extra.SetOrAddSkillBreakPlate(context, skillId, plate);
		}
		if (plateObsolete != null)
		{
			if (flag)
			{
				SetElement_SkillBreakPlateObsoleteDict(skillId, plateObsolete, context);
			}
			else
			{
				AddElement_SkillBreakPlateObsoleteDict(skillId, plateObsolete, context);
			}
		}
	}

	private void ChangeCurrBreakPlateBonus(DataContext context, short skillId, SkillBreakPlateObsolete plateObsolete)
	{
		SkillBreakBonusCollection value;
		bool flag = TryGetElement_SkillBreakBonusDict(skillId, out value);
		if (flag && plateObsolete == null)
		{
			RemoveElement_SkillBreakBonusDict(skillId, context);
		}
		if (plateObsolete != null && plateObsolete.Finished)
		{
			if (value == null)
			{
				value = new SkillBreakBonusCollection();
			}
			plateObsolete.CalcSkillBreakBonusCollection(value);
			if (flag)
			{
				SetElement_SkillBreakBonusDict(skillId, value, context);
			}
			else
			{
				AddElement_SkillBreakBonusDict(skillId, value, context);
			}
		}
	}

	public void AddTeachTaiwuLifeSkill(DataContext context, int charId, short skillId)
	{
		if (!_teachTaiwuLifeSkillDict.ContainsKey(charId))
		{
			AddElement_TeachTaiwuLifeSkillDict(charId, GameData.Utilities.ShortList.Create(), context);
		}
		GameData.Utilities.ShortList value = _teachTaiwuLifeSkillDict[charId];
		if (value.Items.Count >= 3)
		{
			throw new Exception($"Npc {charId} already teached {value.Items.Count} life skills to taiwu, cannnot teach more");
		}
		value.Items.Add(skillId);
		SetElement_TeachTaiwuLifeSkillDict(charId, value, context);
	}

	public void ClearTeachTaiwuLifeSkillList(DataContext context, int charId)
	{
		if (_teachTaiwuLifeSkillDict.ContainsKey(charId))
		{
			RemoveElement_TeachTaiwuLifeSkillDict(charId, context);
		}
	}

	public void AddTeachTaiwuCombatSkill(DataContext context, int charId, short skillId)
	{
		if (!_teachTaiwuCombatSkillDict.ContainsKey(charId))
		{
			AddElement_TeachTaiwuCombatSkillDict(charId, GameData.Utilities.ShortList.Create(), context);
		}
		GameData.Utilities.ShortList value = _teachTaiwuCombatSkillDict[charId];
		if (value.Items.Count >= 3)
		{
			throw new Exception($"Npc {charId} already teached {value.Items.Count} combat skills to taiwu, cannnot teach more");
		}
		value.Items.Add(skillId);
		SetElement_TeachTaiwuCombatSkillDict(charId, value, context);
	}

	public void ClearTeachTaiwuCombatSkillList(DataContext context, int charId)
	{
		if (_teachTaiwuCombatSkillDict.ContainsKey(charId))
		{
			RemoveElement_TeachTaiwuCombatSkillDict(charId, context);
		}
	}

	[DomainMethod]
	public void EscapeToAdjacentBlock(DataContext context)
	{
		Location location = _taiwuChar.GetLocation();
		short randomAdjacentBlockId = DomainManager.Map.GetRandomAdjacentBlockId(context.Random, location.AreaId, location.BlockId);
		DomainManager.Map.Move(context, randomAdjacentBlockId, notCostTime: true);
		SetNeedToEscape(value: false, context);
	}

	[DomainMethod]
	public void TaiwuAddFeature(DataContext context, short featureId)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		taiwu.AddFeature(context, featureId, removeMutexFeature: true);
	}

	public void SetTaiwu(DataContext context, GameData.Domains.Character.Character newTaiwuChar)
	{
		if (_taiwuChar != null)
		{
			_previousTaiwuIds.Add(_taiwuChar.GetId());
			SetPreviousTaiwuIds(_previousTaiwuIds, context);
		}
		int id = newTaiwuChar.GetId();
		for (int i = 0; i < _combatGroupCharIds.Length; i++)
		{
			if (_combatGroupCharIds[i] == id)
			{
				SetElement_CombatGroupCharIds(i, -1, context);
			}
		}
		SetTaiwuCharId(id, context);
		_taiwuChar = newTaiwuChar;
		DomainManager.Extra.UpdateTaiwuSkillDataListener(context);
		DomainManager.Extra.UpdateTaiwuLegendaryBookEffects(context);
		DomainManager.Taiwu.TryRemoveReferenceSkillAtLockedSlot(context);
	}

	public sbyte GetTaiwuVillageStateSect()
	{
		sbyte taiwuVillageStateTemplateId = DomainManager.World.GetTaiwuVillageStateTemplateId();
		return MapState.Instance[taiwuVillageStateTemplateId].SectID;
	}

	public GameData.Domains.Character.Character GetTaiwu()
	{
		return _taiwuChar;
	}

	public bool CanTaiwuBeSneakyHarmfulActionTarget()
	{
		return _canTaiwuBeSneakyHarmfulActionTarget;
	}

	public void ResetEquipmentPlans(DataContext context)
	{
		for (int i = 0; i < _equipmentsPlans.Length; i++)
		{
			for (int j = 0; j < _equipmentsPlans[i].Slots.Length; j++)
			{
				_equipmentsPlans[i].Slots[j] = ItemKey.Invalid;
			}
			SetElement_EquipmentsPlans(i, _equipmentsPlans[i], context);
		}
		SetCurrEquipmentPlanId(0, context);
	}

	public void SetWeaponInnerRatio(DataContext context, int index, sbyte innerRatio)
	{
		if (index < 6)
		{
			_weaponInnerRatios[index] = innerRatio;
			SetWeaponInnerRatios(_weaponInnerRatios, context);
		}
		else
		{
			DomainManager.Extra.SetVoiceWeaponInnerRatio(innerRatio, context);
		}
	}

	public void UpdateTaiwuWeaponInnerRatios(DataContext context, sbyte srcSlot, sbyte destSlot, ItemKey srcItemKey)
	{
		sbyte[] array = EquipmentSlot.EquipmentType2Slots[0];
		if (array.Exist(destSlot))
		{
			int num = array.IndexOf(srcSlot);
			int num2 = array.IndexOf(destSlot);
			if (num >= 0)
			{
				ref sbyte reference = ref _weaponInnerRatios[num];
				ref sbyte reference2 = ref _weaponInnerRatios[num2];
				sbyte b = _weaponInnerRatios[num2];
				sbyte b2 = _weaponInnerRatios[num];
				reference = b;
				reference2 = b2;
			}
			else
			{
				_weaponInnerRatios[num2] = Config.Weapon.Instance[srcItemKey.TemplateId].DefaultInnerRatio;
			}
			SetWeaponInnerRatios(_weaponInnerRatios, context);
		}
	}

	public void AddAppointment(DataContext context, int charId, short settlementId)
	{
		if (_appointments.ContainsKey(charId))
		{
			SetElement_Appointments(charId, settlementId, context);
		}
		else
		{
			AddElement_Appointments(charId, settlementId, context);
		}
	}

	public void RemoveAppointment(DataContext context, int charId)
	{
		if (_appointments.ContainsKey(charId))
		{
			RemoveElement_Appointments(charId, context);
		}
	}

	public void PrenatalEducateForMainAttribute(DataContext context, short bonus)
	{
		_babyBonusMainAttributes += bonus;
		SetBabyBonusMainAttributes(_babyBonusMainAttributes, context);
	}

	public void PrenatalEducateForLifeSkill(DataContext context, short bonus)
	{
		_babyBonusLifeSkillQualifications += bonus;
		SetBabyBonusLifeSkillQualifications(_babyBonusLifeSkillQualifications, context);
	}

	public void PrenatalEducateForCombatSkill(DataContext context, short bonus)
	{
		_babyBonusCombatSkillQualifications += bonus;
		SetBabyBonusCombatSkillQualifications(_babyBonusCombatSkillQualifications, context);
	}

	public unsafe void ApplyPrenatalEducationBonus(DataContext context, GameData.Domains.Character.Character child)
	{
		if (_babyBonusMainAttributes != 0)
		{
			MainAttributes delta = default(MainAttributes);
			for (sbyte b = 0; b < 6; b++)
			{
				delta.Items[b] = _babyBonusMainAttributes;
			}
			child.ChangeBaseMainAttributes(context, delta);
		}
		if (_babyBonusLifeSkillQualifications != 0)
		{
			LifeSkillShorts delta2 = default(LifeSkillShorts);
			for (sbyte b2 = 0; b2 < 16; b2++)
			{
				delta2.Items[b2] = _babyBonusLifeSkillQualifications;
			}
			child.ChangeBaseLifeSkillQualifications(context, ref delta2);
		}
		if (_babyBonusCombatSkillQualifications != 0)
		{
			CombatSkillShorts delta3 = default(CombatSkillShorts);
			for (sbyte b3 = 0; b3 < 14; b3++)
			{
				delta3.Items[b3] = _babyBonusCombatSkillQualifications;
			}
			child.ChangeBaseCombatSkillQualifications(context, ref delta3);
		}
	}

	public void ClearPrenatalEducationBonus(DataContext context)
	{
		SetBabyBonusMainAttributes(0, context);
		SetBabyBonusLifeSkillQualifications(0, context);
		SetBabyBonusCombatSkillQualifications(0, context);
	}

	public void AddItem(DataContext context, ItemKey itemKey, int amount, sbyte itemSourceType, bool offLine = false)
	{
		AddItem(context, itemKey, amount, (ItemSourceType)itemSourceType, offLine);
	}

	public void AddItem(DataContext context, ItemKey itemKey, int amount, ItemSourceType itemSourceType, bool offLine = false)
	{
		switch (itemSourceType)
		{
		case ItemSourceType.Equipment:
		case ItemSourceType.Inventory:
			_taiwuChar.AddInventoryItem(context, itemKey, amount, offLine);
			break;
		case ItemSourceType.Warehouse:
			DomainManager.Taiwu.WarehouseAdd(context, itemKey, amount);
			break;
		case ItemSourceType.Treasury:
			StoreItemInTreasury(context, itemKey, amount, offLine);
			break;
		case ItemSourceType.Trough:
			DomainManager.Extra.TroughAdd(context, itemKey, amount);
			break;
		case ItemSourceType.StockStorageWarehouse:
			DomainManager.Extra.AddStockStorageItem(context, 0, itemKey, amount);
			break;
		case ItemSourceType.StockStorageGoodsShelf:
			DomainManager.Extra.AddStockStorageItem(context, 1, itemKey, amount);
			break;
		case ItemSourceType.OldTreasury:
			DomainManager.Extra.TreasuryAdd(context, itemKey, amount);
			break;
		default:
			throw new ArgumentOutOfRangeException("itemSourceType", itemSourceType, null);
		}
	}

	public void AddItemList(DataContext context, List<ItemKey> keyList, ItemSourceType itemSourceType)
	{
		Tester.Assert(keyList != null);
		Tester.Assert(keyList.Count > 0);
		foreach (ItemKey key in keyList)
		{
			AddItem(context, key, 1, itemSourceType, offLine: true);
		}
		CommitOfflineOperation(context, itemSourceType);
	}

	public void RemoveItem(DataContext context, ItemKey itemKey, int amount, sbyte itemSourceType, bool deleteItem, bool offLine = false)
	{
		RemoveItem(context, itemKey, amount, (ItemSourceType)itemSourceType, deleteItem, offLine);
	}

	public void RemoveItem(DataContext context, ItemKey itemKey, int amount, ItemSourceType itemSourceType, bool deleteItem, bool offLine = false)
	{
		switch (itemSourceType)
		{
		case ItemSourceType.Equipment:
		case ItemSourceType.Inventory:
			_taiwuChar.RemoveInventoryItem(context, itemKey, amount, deleteItem, offLine);
			break;
		case ItemSourceType.Warehouse:
			DomainManager.Taiwu.WarehouseRemove(context, itemKey, amount, deleteItem);
			break;
		case ItemSourceType.Treasury:
			DomainManager.Taiwu.TakeItemFromTreasury(context, itemKey, amount, deleteItem, offLine);
			break;
		case ItemSourceType.Trough:
			DomainManager.Extra.TroughRemove(context, itemKey, amount, deleteItem);
			break;
		case ItemSourceType.StockStorageWarehouse:
			DomainManager.Extra.RemoveStockStorageItem(context, 0, itemKey, amount, deleteItem);
			break;
		case ItemSourceType.StockStorageGoodsShelf:
			DomainManager.Extra.RemoveStockStorageItem(context, 1, itemKey, amount, deleteItem);
			break;
		case ItemSourceType.OldTreasury:
			DomainManager.Extra.TreasuryRemove(context, itemKey, amount, deleteItem);
			break;
		default:
			throw new ArgumentOutOfRangeException("itemSourceType", itemSourceType, null);
		}
	}

	public void RemoveItemList(DataContext context, List<ItemKey> keyList, ItemSourceType itemSourceType, bool deleteItem)
	{
		Tester.Assert(keyList != null);
		Tester.Assert(keyList.Count > 0);
		foreach (ItemKey key in keyList)
		{
			RemoveItem(context, key, 1, itemSourceType, deleteItem, offLine: true);
		}
		CommitOfflineOperation(context, itemSourceType);
	}

	public void CommitOfflineOperation(DataContext context, ItemSourceType itemSourceType)
	{
		switch (itemSourceType)
		{
		case ItemSourceType.Equipment:
		case ItemSourceType.Inventory:
			_taiwuChar.SetInventory(_taiwuChar.GetInventory(), context);
			break;
		case ItemSourceType.Treasury:
		{
			SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
			SetTaiwuTreasury(context, taiwuTreasury);
			break;
		}
		case ItemSourceType.StockStorageWarehouse:
		case ItemSourceType.StockStorageGoodsShelf:
			break;
		case ItemSourceType.Warehouse:
		case ItemSourceType.Trough:
			break;
		}
	}

	public ItemSourceType GetNotPureStackableItemSource(ItemKey itemKey)
	{
		if (!ItemTemplateHelper.IsPureStackable(itemKey))
		{
			return ItemSourceType.Invalid;
		}
		foreach (KeyValuePair<ItemKey, int> item in _taiwuChar.GetInventory().Items)
		{
			if (itemKey.Equals(item.Key))
			{
				return ItemSourceType.Inventory;
			}
		}
		foreach (KeyValuePair<ItemKey, int> warehouseItem in _warehouseItems)
		{
			if (itemKey.Equals(warehouseItem.Key))
			{
				return ItemSourceType.Warehouse;
			}
		}
		foreach (KeyValuePair<ItemKey, int> troughItem in DomainManager.Extra.TroughItems)
		{
			if (itemKey.Equals(troughItem.Key))
			{
				return ItemSourceType.Trough;
			}
		}
		return ItemSourceType.Invalid;
	}

	public void RemoveTaiwuItemFromTaiwuStorage(DataContext context, ItemKey itemKey, int count, bool deleteItem)
	{
		if (itemKey.IsValid())
		{
			if (DomainManager.Taiwu.GetItems(ItemSourceType.Inventory).TryGetValue(itemKey, out var value))
			{
				DomainManager.Taiwu.RemoveItem(context, itemKey, count, 1, deleteItem && value == count);
			}
			else if (DomainManager.Taiwu.GetItems(ItemSourceType.Warehouse).TryGetValue(itemKey, out value))
			{
				DomainManager.Taiwu.RemoveItem(context, itemKey, count, 2, deleteItem && value == count);
			}
			else if (DomainManager.Taiwu.GetItems(ItemSourceType.Treasury).TryGetValue(itemKey, out value))
			{
				DomainManager.Taiwu.RemoveItem(context, itemKey, count, 3, deleteItem && value == count);
			}
		}
	}

	public int GetTaiwuItemCountFromSources(sbyte itemType, short itemTemplateId, IList<ItemSourceType> itemSourceTypes)
	{
		if (itemSourceTypes == null)
		{
			return 0;
		}
		int num = 0;
		foreach (ItemSourceType itemSourceType in itemSourceTypes)
		{
			IReadOnlyDictionary<ItemKey, int> items = DomainManager.Taiwu.GetItems(itemSourceType);
			foreach (KeyValuePair<ItemKey, int> item in items)
			{
				if (item.Key.ItemType == itemType && item.Key.TemplateId == itemTemplateId)
				{
					num += item.Value;
				}
			}
		}
		return num;
	}

	public bool RemoveTaiwuItemFromSources(DataContext context, sbyte itemType, short itemTemplateId, int count, IList<ItemSourceType> itemSourceTypes, bool deleteItem)
	{
		if (itemSourceTypes == null)
		{
			return false;
		}
		int num = 0;
		List<(ItemKey, int, ItemSourceType)> list = new List<(ItemKey, int, ItemSourceType)>();
		foreach (ItemSourceType itemSourceType2 in itemSourceTypes)
		{
			IReadOnlyDictionary<ItemKey, int> items = DomainManager.Taiwu.GetItems(itemSourceType2);
			foreach (KeyValuePair<ItemKey, int> item in items)
			{
				if (item.Key.ItemType == itemType && item.Key.TemplateId == itemTemplateId)
				{
					num += item.Value;
					list.Add((item.Key, item.Value, itemSourceType2));
				}
			}
		}
		if (num < count)
		{
			return false;
		}
		foreach (var (itemKey, val, itemSourceType) in list)
		{
			if (count <= 0)
			{
				break;
			}
			int num2 = Math.Min(count, val);
			DomainManager.Taiwu.RemoveItem(context, itemKey, num2, itemSourceType, deleteItem);
			count -= num2;
		}
		return true;
	}

	public bool IsTaiwuAvoidMapBlockEnemies()
	{
		return false;
	}

	public bool IsTaiwuAvoidTravelingEnemies()
	{
		return DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(28);
	}

	public bool IsCharacterRelationFriendlyTaiwu(int charId)
	{
		return DomainManager.Character.IsCharacterRelationFriendly(_taiwuCharId, charId);
	}

	[DomainMethod]
	public (int, int) GetRelationCharacterCountOnBlock(Location location)
	{
		(int, int) result = (0, 0);
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (block.CharacterSet == null)
		{
			return result;
		}
		foreach (int item in block.CharacterSet)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.GetAgeGroup() == 0)
			{
				continue;
			}
			RelatedCharacter relation;
			bool flag = DomainManager.Character.TryGetRelation(_taiwuCharId, element_Objects.GetId(), out relation);
			RelatedCharacter relation2;
			bool flag2 = DomainManager.Character.TryGetRelation(element_Objects.GetId(), _taiwuCharId, out relation2);
			if (flag || flag2)
			{
				sbyte targetRelationCategory = AiHelper.ActionTargetRelationCategory.GetTargetRelationCategory(relation.RelationType);
				sbyte targetRelationCategory2 = AiHelper.ActionTargetRelationCategory.GetTargetRelationCategory(relation2.RelationType);
				if (targetRelationCategory == 1 || targetRelationCategory2 == 1)
				{
					result.Item1++;
				}
				if (targetRelationCategory == 2 || targetRelationCategory2 == 2)
				{
					result.Item2++;
				}
			}
		}
		return result;
	}

	[DomainMethod]
	public int GetConsumedCountOnBlock(Location location)
	{
		int num = 0;
		if (!location.IsValid())
		{
			return num;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (block.CharacterSet == null)
		{
			return num;
		}
		foreach (int item in block.CharacterSet)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.GetLegendaryBookOwnerState() == 3)
			{
				num++;
			}
		}
		return num;
	}

	[DomainMethod]
	public int GetPastLifeRelationCharacterCountOnBlock(Location location)
	{
		int num = 0;
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (block.CharacterSet == null)
		{
			return num;
		}
		foreach (int item in block.CharacterSet)
		{
			(ushort, ushort) dreamBackRelationTypeWithTaiwu = DomainManager.Extra.GetDreamBackRelationTypeWithTaiwu(item);
			if (DomainManager.Extra.TryGetDreamBackLifeRecordByRelatedCharId(item, out var _))
			{
				num++;
			}
			else if (dreamBackRelationTypeWithTaiwu.Item1 != 0 && dreamBackRelationTypeWithTaiwu.Item2 != 0)
			{
				num++;
			}
		}
		return num;
	}

	[DomainMethod]
	public List<ItemDisplayData> ChoosyGetMaterial(DataContext context, sbyte resourceType, int count)
	{
		_taiwuChar.ChangeResource(context, resourceType, -count * GlobalConfig.Instance.ChoosyResourceBaseCost);
		List<ItemKey> list = new List<ItemKey>();
		List<short> materialIdList = new List<short>();
		List<ChoosyItem> list2 = new List<ChoosyItem>();
		LifeSkillShorts lifeSkillAttainments = DomainManager.Taiwu.GetTaiwu().GetLifeSkillAttainments();
		if (resourceType == 5)
		{
			list2.AddRange(Choosy.Instance.Where(delegate(ChoosyItem c)
			{
				sbyte lifeSkillType2 = c.LifeSkillType;
				return (uint)(lifeSkillType2 - 8) <= 1u;
			}));
		}
		else
		{
			ResourceTypeItem resourceTypeItem = Config.ResourceType.Instance[resourceType];
			sbyte lifeSkillType = resourceTypeItem.LifeSkillType;
			list2.AddRange(Choosy.Instance.Where((ChoosyItem c) => c.LifeSkillType == lifeSkillType));
		}
		for (int num = 0; num < count; num++)
		{
			int num2 = 0;
			ChoosyItem random = list2.GetRandom(context.Random);
			short num3 = lifeSkillAttainments[random.LifeSkillType];
			string name = Config.LifeSkillType.Instance[random.LifeSkillType].Name;
			DomainManager.Extra.TryGetChoosyRemainUpgradeRate(random.TemplateId, out var value);
			DomainManager.Extra.TryGetChoosyRemainUpgradeCount(random.TemplateId, out var value2);
			int num4 = Math.Min(random.MaxUpgradeRate, random.BaseUpgradeRate + value);
			int num5 = Math.Min(random.MaxUpgradeCount, random.BaseUpgradeCount + value2) / 10000;
			int num6 = 0;
			int index = Math.Min(random.GradeList.Count - 1, num5 + 1);
			short num7 = random.GradeList[index];
			bool flag = false;
			while (num6 < num5)
			{
				num6++;
				int num8 = context.Random.Next(10000);
				if (num8 > 10000 - num4)
				{
					num2++;
					if (num2 >= random.GradeList.Count - 1)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				value = 0;
				value2 = 0;
			}
			else
			{
				value += random.UpgradeRateAttainmentBonus * num3 / random.AttainmentRate;
				value = Math.Min(value, random.MaxUpgradeRate - random.BaseUpgradeRate);
				value2 += random.UpgradeCountAttainmentBonus * num3 / random.AttainmentRate;
				value2 = Math.Min(value2, random.MaxUpgradeCount - random.BaseUpgradeCount);
			}
			if (num2 == random.GradeList.Count - 2)
			{
				value /= 2;
			}
			DomainManager.Extra.SetChoosyRemainUpgradeRate(random.TemplateId, value, context);
			DomainManager.Extra.SetChoosyRemainUpgradeCount(random.TemplateId, value2, context);
			short grade = random.GradeList[num2];
			materialIdList.Clear();
			Config.Material.Instance.Iterate(delegate(MaterialItem materialItem2)
			{
				short templateId2 = materialItem2.TemplateId;
				if (templateId2 >= 280 && templateId2 <= 341)
				{
					return true;
				}
				if (materialItem2.ResourceType == resourceType && materialItem2.Grade == grade && materialItem2.DropRate > 0)
				{
					materialIdList.Add(materialItem2.TemplateId);
				}
				return true;
			});
			if (materialIdList.Count > 0)
			{
				short random2 = materialIdList.GetRandom(context.Random);
				ItemKey itemKey = DomainManager.Item.CreateItem(context, 5, random2);
				_taiwuChar.AddInventoryItem(context, itemKey, 1);
				list.Add(itemKey);
			}
		}
		list = (from key in list
			orderby ItemTemplateHelper.GetItemSubType(key.ItemType, key.TemplateId), ItemTemplateHelper.GetGrade(key.ItemType, key.TemplateId)
			select key).ToList();
		List<ItemDisplayData> itemDisplayDataListOptional = DomainManager.Item.GetItemDisplayDataListOptional(list, _taiwuCharId, 1, merge: true);
		foreach (ItemDisplayData item in itemDisplayDataListOptional)
		{
			sbyte resourceType2 = ItemTemplateHelper.GetResourceType(item.Key.ItemType, item.Key.TemplateId);
			if ((uint)(resourceType2 - 1) <= 3u)
			{
				MaterialItem materialItem = Config.Material.Instance[item.Key.TemplateId];
				int templateId = ((materialItem.RefiningEffect < 0) ? 21 : 22);
				int baseDelta = ProfessionFormulaImpl.Calculate(templateId, item.Value);
				DomainManager.Extra.ChangeProfessionSeniority(context, 2, baseDelta);
			}
			else if (resourceType2 == 5)
			{
				int baseDelta2 = ProfessionFormulaImpl.Calculate(89, item.Value);
				DomainManager.Extra.ChangeProfessionSeniority(context, 13, baseDelta2);
			}
		}
		return itemDisplayDataListOptional;
	}

	public void AddChoosyRemainUpgradeData(DataContext context)
	{
		LifeSkillShorts lifeSkillAttainments = DomainManager.Taiwu.GetTaiwu().GetLifeSkillAttainments();
		for (sbyte b = 0; b < Choosy.Instance.Count; b++)
		{
			ChoosyItem choosyItem = Choosy.Instance[b];
			DomainManager.Extra.TryGetChoosyRemainUpgradeCount(b, out var value);
			DomainManager.Extra.TryGetChoosyRemainUpgradeRate(b, out var value2);
			short num = lifeSkillAttainments[choosyItem.LifeSkillType];
			int num2 = choosyItem.UpgradeCountAttainmentBonus * num / choosyItem.AttainmentRate;
			int num3 = choosyItem.UpgradeRateAttainmentBonus * num / choosyItem.AttainmentRate;
			int num4 = ((num2 > 0) ? context.Random.Next(num2) : 0);
			int num5 = ((num3 > 0) ? context.Random.Next(num3) : 0);
			int value3 = Math.Min(choosyItem.MaxUpgradeCount, value + num4);
			int value4 = Math.Min(choosyItem.MaxUpgradeRate, value2 + num5);
			DomainManager.Extra.SetChoosyRemainUpgradeCount(b, value3, context);
			DomainManager.Extra.SetChoosyRemainUpgradeRate(b, value4, context);
		}
	}

	[DomainMethod]
	public void DeleteTaiwuFeature(DataContext context, short featureId)
	{
		CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[featureId];
		if (!characterFeatureItem.CanDeleteManually)
		{
			throw new Exception($"Can't delete feature: {featureId} manually");
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		taiwu.RemoveFeature(context, featureId);
	}

	public bool IsTaiwuContainPunishmentFeature(short orgTemplateId)
	{
		List<short> featureIds = DomainManager.Taiwu.GetTaiwu().GetFeatureIds();
		List<short> taiwuPunishementFeature = Config.Organization.Instance[orgTemplateId].TaiwuPunishementFeature;
		if (taiwuPunishementFeature == null || taiwuPunishementFeature.Count == 0)
		{
			return false;
		}
		foreach (short item in taiwuPunishementFeature)
		{
			if (featureIds.Contains(item))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsTaiwuAbleToGetTaught(GameData.Domains.Character.Character teacher)
	{
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(2);
		sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(settlementByOrgTemplateId.GetLocation().AreaId);
		return teacher.GetBelongMapState() != stateIdByAreaId || !DomainManager.Taiwu.IsTaiwuContainPunishmentFeature(2);
	}

	public void UpdateConsummateLevelBrokenFeature(DataContext context)
	{
		_taiwuChar.RemoveFeatureGroup(context, 756);
		if (DomainManager.World.GetWorldFunctionsStatus(26) && _taiwuChar.GetConsummateLevel() != GlobalConfig.Instance.MaxConsummateLevel)
		{
			_taiwuChar.AddFeature(context, (short)((DomainManager.World.GetMainStoryLineProgress() > 10) ? 756 : 757));
		}
	}

	public void UpdateTaiwuPunishment(DataContext context)
	{
		if (!IsTaiwuContainPunishmentFeature(13))
		{
		}
	}

	public void AddJieqingPunishmentMonthlyEvent(DataContext context)
	{
		if (JieqingPunishmentAssassinAlreadyAdd)
		{
			return;
		}
		List<short> featureIds = _taiwuChar.GetFeatureIds();
		for (int i = 0; i < featureIds.Count; i++)
		{
			short num = featureIds[i];
			if (num >= 595 && num <= 599)
			{
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddJieQingPunishmentAssassin(_taiwuChar.GetId(), _taiwuChar.GetLocation(), -1);
				break;
			}
		}
	}

	public (int charId, BasePrioritizedAction action) TryGetHuntTaiwuPrioritizedAction()
	{
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(13);
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		settlementByOrgTemplateId.GetMembers().GetAllMembers(list);
		for (int i = 0; i < list.Count; i++)
		{
			int num = list[i];
			if (DomainManager.Character.TryGetCharacterPrioritizedAction(num, out var action) && action.ActionType == 22)
			{
				ObjectPool<List<int>>.Instance.Return(list);
				return (charId: num, action: action);
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
		return (charId: -1, action: null);
	}

	public void TaiwuAddFeatureTryInterruptHuntTaiwuAction(DataContext context, short featureId)
	{
		if ((featureId >= 595 && featureId <= 599) || 1 == 0)
		{
			(int, BasePrioritizedAction) tuple = TryGetHuntTaiwuPrioritizedAction();
			if (tuple.Item1 != -1 && DomainManager.Character.TryGetElement_Objects(tuple.Item1, out var element) && !PrioritizedActionTypeHelper.CheckHuntTaiwuCondition(featureId, element.GetConsummateLevel(), _taiwuChar.GetConsummateLevel()))
			{
				tuple.Item2.OnInterrupt(context, element);
				DomainManager.Character.RemoveCharacterPrioritizedAction(context, element.GetId());
				element.ResetPrioritizedActionCooldown(context, 22, isFailToCreate: false);
			}
		}
	}

	public void TaiwuRemoveFeatureTryInterruptHuntTaiwuAction(DataContext context, short featureId, List<short> featureIds)
	{
		if ((featureId < 595 || featureId > 599) ? true : false)
		{
			return;
		}
		(int, BasePrioritizedAction) tuple = TryGetHuntTaiwuPrioritizedAction();
		if (tuple.Item1 == -1 || !DomainManager.Character.TryGetElement_Objects(tuple.Item1, out var element))
		{
			return;
		}
		bool flag = true;
		for (int i = 0; i < featureIds.Count; i++)
		{
			short num = featureIds[i];
			if (num >= 595 && num <= 599)
			{
				flag = false;
			}
		}
		if (flag)
		{
			tuple.Item2.OnInterrupt(context, element);
			DomainManager.Character.RemoveCharacterPrioritizedAction(context, element.GetId());
			element.ResetPrioritizedActionCooldown(context, 22, isFailToCreate: false);
		}
	}

	[DomainMethod]
	public int GetLastCricketPlan()
	{
		return DomainManager.Extra.GetLastCricketPlan();
	}

	[DomainMethod]
	public void SetLastCricketPlan(DataContext context, int value)
	{
		DomainManager.Extra.SetLastCricketPlan(context, value);
	}

	[DomainMethod]
	public List<ItemKey> RequestValidCricketPlan(int index, CricketCombatConfig config)
	{
		List<CricketCombatPlan> cricketPlans = DomainManager.Extra.GetCricketPlans();
		CricketCombatPlan orDefault = cricketPlans.GetOrDefault(index);
		if (orDefault != null)
		{
			List<ItemKey> crickets = orDefault.Crickets;
			if (crickets != null && crickets.Count > 0)
			{
				Inventory inventory = _taiwuChar.GetInventory();
				List<ItemKey> list = new List<ItemKey>();
				for (int i = 0; i < orDefault.Crickets.Count; i++)
				{
					ItemKey itemKey = orDefault.Crickets[i];
					if (inventory.Items.ContainsKey(itemKey) && DomainManager.Item.TryGetElement_Crickets(itemKey.Id, out var element) && element.Match(config))
					{
						list.SetOrAdd(i, itemKey, ItemKey.Invalid);
					}
				}
				return list;
			}
		}
		return null;
	}

	[DomainMethod]
	public void SetCricketPlan(DataContext context, int index, ItemKey cricket, int cricketIndex)
	{
		List<CricketCombatPlan> cricketPlans = DomainManager.Extra.GetCricketPlans();
		CricketCombatPlan cricketCombatPlan = cricketPlans.GetOrDefault(index);
		if (cricketCombatPlan == null || !(cricketCombatPlan.Crickets?.GetOrDefault(cricketIndex, ItemKey.Invalid) == cricket))
		{
			if (cricketCombatPlan == null || cricketCombatPlan.Crickets == null)
			{
				cricketCombatPlan = cricketPlans.SetOrAdd(index, new CricketCombatPlan
				{
					Crickets = new List<ItemKey>()
				});
			}
			cricketCombatPlan.Crickets.SetOrAdd(cricketIndex, cricket, ItemKey.Invalid);
			DomainManager.Extra.SetCricketPlans(context, cricketPlans);
		}
	}

	[DomainMethod]
	public void ClearCricketPlan(DataContext context, int index)
	{
		List<CricketCombatPlan> cricketPlans = DomainManager.Extra.GetCricketPlans();
		CricketCombatPlan orDefault = cricketPlans.GetOrDefault(index);
		if (orDefault != null)
		{
			List<ItemKey> crickets = orDefault.Crickets;
			if (crickets != null && crickets.Count > 0)
			{
				orDefault.Crickets.Clear();
				DomainManager.Extra.SetCricketPlans(context, cricketPlans);
			}
		}
	}

	public void ReplaceCricketPlan(DataContext context, ItemKey srcCricket, ItemKey dstCricket)
	{
		List<CricketCombatPlan> cricketPlans = DomainManager.Extra.GetCricketPlans();
		if (cricketPlans.ReplaceCricket(srcCricket.Id, dstCricket))
		{
			DomainManager.Extra.SetCricketPlans(context, cricketPlans);
		}
	}

	public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
	{
		crossArchiveGameData.PreviousTaiwuCharIds = _previousTaiwuIds;
		if (crossArchiveGameData.WarehouseItems == null)
		{
			crossArchiveGameData.WarehouseItems = new Dictionary<ItemKey, int>();
		}
		foreach (var (itemKey2, amount) in _warehouseItems)
		{
			crossArchiveGameData.PackWarehouseItem(itemKey2, amount);
		}
		crossArchiveGameData.TaiwuCombatSkills = _combatSkills;
		crossArchiveGameData.TaiwuLifeSkills = _lifeSkills;
		crossArchiveGameData.NotLearnedCombatSkills = _notLearnCombatSkillReadingProgress;
		crossArchiveGameData.NotLearnedLifeSkills = _notLearnLifeSkillReadingProgress;
		crossArchiveGameData.SkillBreakPlateObsoleteDict = _skillBreakPlateObsoleteDict;
		crossArchiveGameData.SkillBreakBonusDict = _skillBreakBonusDict;
		crossArchiveGameData.CurrCombatSkillPlanId = _currCombatSkillPlanId;
		crossArchiveGameData.CombatSkillPlans = _combatSkillPlans;
		crossArchiveGameData.CurrEquipmentPlanId = _currEquipmentPlanId;
		crossArchiveGameData.WeaponInnerRatios = _weaponInnerRatios;
		crossArchiveGameData.EquipmentsPlans = _equipmentsPlans;
		EquipmentPlan[] equipmentsPlans = _equipmentsPlans;
		foreach (EquipmentPlan equipmentPlan in equipmentsPlans)
		{
			ItemKey[] slots = equipmentPlan.Slots;
			for (int j = 0; j < slots.Length; j++)
			{
				ItemKey itemKey3 = slots[j];
				if (itemKey3.IsValid())
				{
					DomainManager.Item.PackCrossArchiveItem(crossArchiveGameData, itemKey3);
				}
			}
		}
		crossArchiveGameData.CurrCombatSkillAttainmentPanelPlanIds = _currCombatSkillAttainmentPanelPlanIds;
		crossArchiveGameData.CombatSkillAttainmentPanelPlans = _combatSkillAttainmentPanelPlans;
		crossArchiveGameData.CurrLifeSkillAttainmentPanelPlanIndex = _currLifeSkillAttainmentPanelPlanIndex;
		crossArchiveGameData.BuildingSpaceExtraAdd = _buildingSpaceExtraAdd;
	}

	public override void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
	}

	public void UnpackCrossArchiveGameData_Building(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		_taiwuChar.ChangeResources(context, ref crossArchiveGameData.TaiwuResources);
		if (crossArchiveGameData.WarehouseItems != null)
		{
			foreach (KeyValuePair<ItemKey, int> warehouseItem in crossArchiveGameData.WarehouseItems)
			{
				ItemKey itemKey = DomainManager.Item.UnpackCrossArchiveItem(context, crossArchiveGameData, warehouseItem.Key);
				if (itemKey.IsValid() && DomainManager.Item.ItemExists(itemKey))
				{
					WarehouseAdd(context, itemKey, warehouseItem.Value);
					continue;
				}
				Logger.Warn($"Invalid treasury item found during dreamback: {itemKey}.");
			}
		}
		crossArchiveGameData.WarehouseItems = null;
		AddBuildingSpaceExtra(context, crossArchiveGameData.BuildingSpaceExtraAdd);
	}

	public void UnpackCrossArchiveGameData_Items(DataContext context, CrossArchiveGameData crossArchiveGameData, bool overwriteWeapons)
	{
		_curReadingBook = ItemKey.Invalid;
		SetCurReadingBook(_curReadingBook, context);
		for (int i = 0; i < _referenceBooks.Length; i++)
		{
			_referenceBooks[i] = ItemKey.Invalid;
		}
		SetReferenceBooks(_referenceBooks, context);
		ClearReadingBooks(context);
		crossArchiveGameData.ReadingBooks = null;
		if (overwriteWeapons)
		{
			SetCurrEquipmentPlanId(crossArchiveGameData.CurrEquipmentPlanId, context);
			for (int j = 0; j < 5; j++)
			{
				EquipmentPlan equipmentPlan = crossArchiveGameData.EquipmentsPlans[j];
				for (int k = 0; k < equipmentPlan.Slots.Length; k++)
				{
					ItemKey srcItemKey = equipmentPlan.Slots[k];
					if (srcItemKey.IsValid())
					{
						equipmentPlan.Slots[k] = DomainManager.Item.UnpackCrossArchiveItem(context, crossArchiveGameData, srcItemKey);
					}
				}
				SetElement_EquipmentsPlans(j, crossArchiveGameData.EquipmentsPlans[j], context);
			}
			SetWeaponInnerRatios(crossArchiveGameData.WeaponInnerRatios, context);
		}
		crossArchiveGameData.WeaponInnerRatios = null;
		crossArchiveGameData.EquipmentsPlans = null;
	}

	public void UnpackCrossArchiveGameData_LifeSkills(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		Dictionary<short, TaiwuLifeSkill> taiwuLifeSkills = crossArchiveGameData.TaiwuLifeSkills;
		short key;
		TaiwuLifeSkill value;
		if (taiwuLifeSkills != null)
		{
			foreach (KeyValuePair<short, TaiwuLifeSkill> item in taiwuLifeSkills)
			{
				item.Deconstruct(out key, out value);
				short num = key;
				TaiwuLifeSkill taiwuLifeSkill = value;
				if (!CombineDreamBackTaiwuLifeSkill(num, taiwuLifeSkill))
				{
					AddElement_LifeSkills(num, taiwuLifeSkill, context);
				}
			}
		}
		Dictionary<short, TaiwuLifeSkill> notLearnedLifeSkills = crossArchiveGameData.NotLearnedLifeSkills;
		if (notLearnedLifeSkills != null)
		{
			foreach (KeyValuePair<short, TaiwuLifeSkill> item2 in notLearnedLifeSkills)
			{
				item2.Deconstruct(out key, out value);
				short num2 = key;
				TaiwuLifeSkill taiwuLifeSkill2 = value;
				if (!CombineDreamBackTaiwuLifeSkill(num2, taiwuLifeSkill2))
				{
					AddElement_NotLearnLifeSkillReadingProgress(num2, taiwuLifeSkill2, context);
				}
			}
		}
		for (int i = 0; i < _currLifeSkillAttainmentPanelPlanIndex.Length; i++)
		{
			SetElement_CurrLifeSkillAttainmentPanelPlanIndex(i, crossArchiveGameData.CurrLifeSkillAttainmentPanelPlanIndex[i], context);
		}
		crossArchiveGameData.TaiwuLifeSkills = null;
		crossArchiveGameData.NotLearnedLifeSkills = null;
		crossArchiveGameData.CurrLifeSkillAttainmentPanelPlanIndex = null;
		bool CombineDreamBackTaiwuLifeSkill(short skillId, TaiwuLifeSkill dreamBackLifeSkill)
		{
			if (_lifeSkills.TryGetValue(skillId, out var value2))
			{
				CombineTaiwuLifeSkillData(value2, dreamBackLifeSkill);
				SetElement_LifeSkills(skillId, value2, context);
				return true;
			}
			if (_notLearnLifeSkillReadingProgress.TryGetValue(skillId, out var value3))
			{
				CombineTaiwuLifeSkillData(value3, dreamBackLifeSkill);
				RemoveElement_NotLearnCombatSkillReadingProgress(skillId, context);
				AddElement_LifeSkills(skillId, value3, context);
				return true;
			}
			return false;
		}
		static void CombineTaiwuLifeSkillData(TaiwuLifeSkill taiwuLifeSkill3, TaiwuLifeSkill dreamBackLifeSkill)
		{
			sbyte[] allBookPageReadingProgress = taiwuLifeSkill3.GetAllBookPageReadingProgress();
			sbyte[] allBookPageReadingProgress2 = dreamBackLifeSkill.GetAllBookPageReadingProgress();
			for (byte b = 0; b < allBookPageReadingProgress.Length; b++)
			{
				sbyte progress = Math.Max(allBookPageReadingProgress[b], allBookPageReadingProgress2[b]);
				taiwuLifeSkill3.SetBookPageReadingProgress(b, progress);
			}
		}
	}

	public void UnpackCrossArchiveGameData_CombatSkills(DataContext context, CrossArchiveGameData crossArchiveGameData, bool overwriteEquipments)
	{
		int currDate = DomainManager.World.GetCurrDate();
		int num = currDate - DomainManager.Extra.GetFinalDateBeforeDreamBack();
		Dictionary<short, TaiwuCombatSkill> taiwuCombatSkills = crossArchiveGameData.TaiwuCombatSkills;
		short key;
		TaiwuCombatSkill value;
		if (taiwuCombatSkills != null)
		{
			foreach (KeyValuePair<short, TaiwuCombatSkill> item in taiwuCombatSkills)
			{
				item.Deconstruct(out key, out value);
				short num2 = key;
				TaiwuCombatSkill taiwuCombatSkill = value;
				if (taiwuCombatSkill.LastClearBreakPlateTime >= 0)
				{
					taiwuCombatSkill.LastClearBreakPlateTime += num;
				}
				if (!CombineDreamBackTaiwuCombatSkill(num2, taiwuCombatSkill))
				{
					AddElement_CombatSkills(num2, taiwuCombatSkill, context);
				}
			}
		}
		Dictionary<short, TaiwuCombatSkill> notLearnedCombatSkills = crossArchiveGameData.NotLearnedCombatSkills;
		if (notLearnedCombatSkills != null)
		{
			foreach (KeyValuePair<short, TaiwuCombatSkill> item2 in notLearnedCombatSkills)
			{
				item2.Deconstruct(out key, out value);
				short num3 = key;
				TaiwuCombatSkill taiwuCombatSkill2 = value;
				if (!CombineDreamBackTaiwuCombatSkill(num3, taiwuCombatSkill2))
				{
					AddElement_NotLearnCombatSkillReadingProgress(num3, taiwuCombatSkill2, context);
				}
			}
		}
		Dictionary<short, SkillBreakPlate> dictionary = new Dictionary<short, SkillBreakPlate>();
		Dictionary<short, SkillBreakPlateObsolete> dictionary2 = new Dictionary<short, SkillBreakPlateObsolete>();
		Dictionary<short, SkillBreakBonusCollection> dictionary3 = new Dictionary<short, SkillBreakBonusCollection>();
		List<short> list = new List<short>();
		crossArchiveGameData.SkillBreakPlateObsoleteDict.RemoveDuplicateKeys(crossArchiveGameData.SkillBreakPlateDict, list);
		list.Clear();
		if (crossArchiveGameData.SkillBreakPlateDict != null)
		{
			list.AddRange(crossArchiveGameData.SkillBreakPlateDict.Keys);
		}
		if (crossArchiveGameData.SkillBreakPlateObsoleteDict != null)
		{
			list.AddRange(crossArchiveGameData.SkillBreakPlateObsoleteDict.Keys);
		}
		foreach (short item3 in list)
		{
			SkillBreakPlate skillBreakPlate = crossArchiveGameData.SkillBreakPlateDict?.GetValueOrDefault(item3);
			SkillBreakPlateObsolete skillBreakPlateObsolete = crossArchiveGameData.SkillBreakPlateObsoleteDict?.GetValueOrDefault(item3);
			if (skillBreakPlate != null && skillBreakPlateObsolete != null)
			{
				skillBreakPlateObsolete = null;
			}
			if (!GetAutoBreakPlate(context, item3, out var plate, out var obsoletePlate) || (plate != null && !plate.Success) || (obsoletePlate != null && !obsoletePlate.Finished))
			{
				if (skillBreakPlate != null)
				{
					DomainManager.Extra.SetOrAddSkillBreakPlate(context, item3, skillBreakPlate);
				}
				if (skillBreakPlateObsolete != null)
				{
					if (obsoletePlate == null)
					{
						AddElement_SkillBreakPlateObsoleteDict(item3, skillBreakPlateObsolete, context);
					}
					else
					{
						SetElement_SkillBreakPlateObsoleteDict(item3, skillBreakPlateObsolete, context);
					}
				}
				if (skillBreakPlateObsolete != null && skillBreakPlateObsolete.Finished)
				{
					SkillBreakBonusCollection skillBreakBonusCollection = crossArchiveGameData.SkillBreakBonusDict[item3];
					skillBreakPlateObsolete.CalcSkillBreakBonusCollection(skillBreakBonusCollection);
					AddElement_SkillBreakBonusDict(item3, skillBreakBonusCollection, context);
				}
			}
			else
			{
				if (skillBreakPlate != null)
				{
					dictionary.Add(item3, skillBreakPlate);
				}
				if (skillBreakPlateObsolete != null)
				{
					SkillBreakBonusCollection value2 = crossArchiveGameData.SkillBreakBonusDict[item3];
					dictionary2.Add(item3, skillBreakPlateObsolete);
					dictionary3.Add(item3, value2);
				}
			}
		}
		crossArchiveGameData.SkillBreakPlateDict = dictionary;
		crossArchiveGameData.SkillBreakPlateObsoleteDict = dictionary2;
		crossArchiveGameData.SkillBreakBonusDict = dictionary3;
		if (overwriteEquipments)
		{
			short[] equippedCombatSkills = _taiwuChar.GetEquippedCombatSkills();
			short[] equippedCombatSkills2 = crossArchiveGameData.TaiwuChar.GetEquippedCombatSkills();
			Array.Copy(equippedCombatSkills2, equippedCombatSkills, equippedCombatSkills.Length);
			_taiwuChar.SetEquippedCombatSkills(equippedCombatSkills, context);
			if (crossArchiveGameData.ExternalEquippedCombatSkills != null)
			{
				DomainManager.Extra.SetCharacterEquippedCombatSkills(context, _taiwuCharId, crossArchiveGameData.ExternalEquippedCombatSkills);
			}
			SetCurrCombatSkillPlanId(crossArchiveGameData.CurrCombatSkillPlanId, context);
			for (int i = 0; i < _combatSkillPlans.Length; i++)
			{
				SetElement_CombatSkillPlans(i, crossArchiveGameData.CombatSkillPlans[i], context);
			}
			short[] combatSkillAttainmentPanels = crossArchiveGameData.TaiwuChar.GetCombatSkillAttainmentPanels();
			short[] combatSkillAttainmentPanels2 = _taiwuChar.GetCombatSkillAttainmentPanels();
			combatSkillAttainmentPanels.CopyTo(combatSkillAttainmentPanels2, 0);
			_taiwuChar.SetCombatSkillAttainmentPanels(combatSkillAttainmentPanels2, context);
			SetCombatSkillAttainmentPanelPlans(crossArchiveGameData.CombatSkillAttainmentPanelPlans, context);
			SetCurrCombatSkillAttainmentPanelPlanIds(crossArchiveGameData.CurrCombatSkillAttainmentPanelPlanIds, context);
		}
		_taiwuChar.SetCurrNeili(_taiwuChar.GetMaxNeili(), context);
		crossArchiveGameData.CombatSkillPlans = null;
		crossArchiveGameData.CombatSkillAttainmentPanelPlans = null;
		crossArchiveGameData.CurrCombatSkillAttainmentPanelPlanIds = null;
		bool CombineDreamBackTaiwuCombatSkill(short skillId, TaiwuCombatSkill dreamBackCombatSkill)
		{
			if (_combatSkills.TryGetValue(skillId, out var value3))
			{
				CombineTaiwuCombatSkillData(value3, dreamBackCombatSkill);
				SetElement_CombatSkills(skillId, value3, context);
				return true;
			}
			if (_notLearnCombatSkillReadingProgress.TryGetValue(skillId, out var value4))
			{
				CombineTaiwuCombatSkillData(value4, dreamBackCombatSkill);
				RemoveElement_NotLearnCombatSkillReadingProgress(skillId, context);
				AddElement_CombatSkills(skillId, value4, context);
				return true;
			}
			return false;
		}
		static void CombineTaiwuCombatSkillData(TaiwuCombatSkill taiwuCombatSkill3, TaiwuCombatSkill dreamBackCombatSkill)
		{
			taiwuCombatSkill3.FullPowerCastTimes = Math.Max(taiwuCombatSkill3.FullPowerCastTimes, dreamBackCombatSkill.FullPowerCastTimes);
			taiwuCombatSkill3.LastClearBreakPlateTime = Math.Max(taiwuCombatSkill3.LastClearBreakPlateTime, dreamBackCombatSkill.LastClearBreakPlateTime);
			sbyte[] allBookPageReadingProgress = taiwuCombatSkill3.GetAllBookPageReadingProgress();
			sbyte[] allBookPageReadingProgress2 = dreamBackCombatSkill.GetAllBookPageReadingProgress();
			for (byte b = 0; b < allBookPageReadingProgress.Length; b++)
			{
				sbyte progress = Math.Max(allBookPageReadingProgress[b], allBookPageReadingProgress2[b]);
				taiwuCombatSkill3.SetBookPageReadingProgress(b, progress);
			}
		}
	}

	[DomainMethod]
	public int GetIsTaiwuFirstByLuck(DataContext context, int npcId)
	{
		int num = 0;
		int num2 = 0;
		foreach (int item in GetGroupCharIds().GetCollection())
		{
			num += DomainManager.Character.GetElement_Objects(item).GetPersonality(5);
		}
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(npcId);
		int leaderId = element_Objects.GetLeaderId();
		if (leaderId == _taiwuCharId)
		{
			num2 = num;
		}
		else if (leaderId >= 0)
		{
			foreach (int item2 in DomainManager.Character.GetGroup(leaderId).GetCollection())
			{
				num2 += DomainManager.Character.GetElement_Objects(item2).GetPersonality(5);
			}
		}
		else
		{
			num2 += element_Objects.GetPersonality(5);
		}
		int percentProb = 50 + num - num2;
		return context.Random.CheckPercentProb(percentProb) ? 1 : (-1);
	}

	[DomainMethod]
	public DebateGame DebateGameInitialize(DataContext context, sbyte type, bool isTaiwuFirst, int npcId, List<int> spectatorIds)
	{
		GameData.Domains.Character.Character taiwu = GetTaiwu();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(npcId);
		List<short> unlockedDebateStrategyList = taiwu.GetUnlockedDebateStrategyList();
		List<short> unlockedDebateStrategyList2 = element_Objects.GetUnlockedDebateStrategyList();
		List<sbyte> list = DebateGameGetTaiwuSelectedCardTypes();
		List<sbyte> list2 = DomainManager.Extra.DebateGameGetPlayerSelectedStrategyCardTypes(npcId);
		int attainment = taiwu.GetLifeSkillAttainment(type) * context.Random.Next(DebateConstants.AttainmentToBasesPercent[0], DebateConstants.AttainmentToBasesPercent[1]) / 100;
		int attainment2 = element_Objects.GetLifeSkillAttainment(type) * context.Random.Next(DebateConstants.AttainmentToBasesPercent[0], DebateConstants.AttainmentToBasesPercent[1]) / 100;
		for (int num = unlockedDebateStrategyList.Count - 1; num >= 0; num--)
		{
			if (!list.Contains(DebateStrategy.Instance[unlockedDebateStrategyList[num]].LifeSkillType))
			{
				unlockedDebateStrategyList.RemoveAt(num);
			}
		}
		for (int num2 = unlockedDebateStrategyList2.Count - 1; num2 >= 0; num2--)
		{
			if (!list2.Contains(DebateStrategy.Instance[unlockedDebateStrategyList2[num2]].LifeSkillType))
			{
				unlockedDebateStrategyList2.RemoveAt(num2);
			}
		}
		DebatePlayer playerLeft = new DebatePlayer(taiwu.GetId(), DebateConstants.MaxPressure, attainment, unlockedDebateStrategyList);
		DebatePlayer playerRight = new DebatePlayer(element_Objects.GetId(), DebateConstants.MaxPressure, attainment2, unlockedDebateStrategyList2);
		Dictionary<IntPair, DebateNode> dictionary = new Dictionary<IntPair, DebateNode>();
		for (int i = 0; i < DebateConstants.DebateLineCount; i++)
		{
			for (int j = 0; j < DebateConstants.DebateLineNodeCount; j++)
			{
				dictionary[new IntPair(j, i)] = new DebateNode(j, i);
			}
		}
		_pawnId = 0;
		_nodeEffectId = 0;
		_activatedStrategyId = 0;
		_debateRevealedPawnCount = (taiwu: 0, npc: 0);
		if (spectatorIds != null)
		{
			foreach (int spectatorId in spectatorIds)
			{
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(spectatorId);
				if (element_Objects2.GetCreatingType() == 1)
				{
					if (element_Objects.GetCreatingType() == 1)
					{
						DomainManager.Character.TryCreateRelation(context, spectatorId, _debateNpcId);
					}
					DomainManager.Character.TryCreateRelation(context, spectatorId, _taiwuCharId);
				}
				_debateSpectatorCooldownMap[spectatorId] = 0;
			}
		}
		Debate = new DebateGame(type, isTaiwuFirst, playerLeft, playerRight, spectatorIds ?? new List<int>(), dictionary, isTaiwuAi: false);
		DebateGameInitializeAi(context, element_Objects);
		DebateGameInitializeStrategyActions();
		return Debate;
	}

	[DomainMethod]
	public List<int> DebateGamePickSpectators(DataContext context, sbyte lifeSkillType, int npcId, bool isTaiwu, List<int> spectatorIds)
	{
		_debateLifeSkillType = lifeSkillType;
		_debateNpcId = npcId;
		_debateNpc = DomainManager.Character.GetElement_Objects(_debateNpcId);
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		HashSet<int> hashSet = new HashSet<int>();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(npcId);
		if (element_Objects.GetCreatingType() != 1)
		{
			if (!isTaiwu)
			{
				CharacterItem characterItem = Config.Character.Instance[element_Objects.GetTemplateId()];
				if (characterItem.MinionGroupId >= 0)
				{
					MinionGroupItem minionGroupItem = MinionGroup.Instance[characterItem.MinionGroupId];
					foreach (short minion in minionGroupItem.Minions)
					{
						int item = DomainManager.Character.CreateNonIntelligentCharacter(context, minion);
						list.Add(item);
						_debateNpcSpectators.Add(item);
					}
				}
				return list;
			}
		}
		else
		{
			int leaderId = element_Objects.GetLeaderId();
			if (leaderId >= 0 && leaderId != _taiwuCharId)
			{
				foreach (int item2 in DomainManager.Character.GetGroup(leaderId).GetCollection())
				{
					hashSet.Add(item2);
				}
			}
		}
		Location location = GetTaiwu().GetLocation();
		if (location.IsValid())
		{
			List<MapBlockData> list3 = new List<MapBlockData>();
			DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, list3, DebateConstants.SpectatorPickRange, includeCenter: true);
			foreach (MapBlockData item3 in list3)
			{
				if (item3.CharacterSet == null || item3.CharacterSet.Count == 0)
				{
					continue;
				}
				foreach (int item4 in item3.CharacterSet)
				{
					list2.Add(item4);
				}
			}
		}
		else if (!isTaiwu)
		{
			foreach (int item5 in hashSet)
			{
				list2.Add(item5);
			}
		}
		if (isTaiwu)
		{
			foreach (int item6 in GetGroupCharIds().GetCollection())
			{
				list2.Add(item6);
			}
		}
		for (int num = list2.Count - 1; num >= 0; num--)
		{
			int num2 = list2[num];
			if (num2 == _debateNpcId || num2 == _taiwuCharId || !DomainManager.Character.TryGetElement_Objects(num2, out var element) || element.GetAgeGroup() == 0 || element.GetFeatureIds().Contains(218) || element.GetLegendaryBookOwnerState() == 3 || (isTaiwu && hashSet.Contains(num2)))
			{
				list2.RemoveAt(num);
			}
		}
		if (spectatorIds != null)
		{
			foreach (int spectatorId in spectatorIds)
			{
				list2.Remove(spectatorId);
			}
		}
		if (isTaiwu)
		{
			list2.Sort(DebateGameCompareSpectatorByAttainment);
		}
		else
		{
			switch (_debateNpc.GetBehaviorType())
			{
			case 0:
			case 1:
				list2.Sort(DebateGameCompareSpectatorByNpcImpression);
				break;
			case 3:
			case 4:
				list2.Sort(DebateGameCompareSpectatorByTaiwuImpression);
				break;
			default:
				list2.Sort(DebateGameCompareSpectatorByMixedImpression);
				break;
			}
		}
		int num3 = (isTaiwu ? list2.Count : Math.Min(3, list2.Count));
		for (int i = 0; i < num3; i++)
		{
			list.Add(list2[0]);
			list2.RemoveAt(0);
		}
		if (!isTaiwu)
		{
			foreach (int item7 in list)
			{
				_debateNpcSpectators.Add(item7);
			}
		}
		return list;
	}

	[DomainMethod]
	public DebateGame DebateGameNextState(DataContext context)
	{
		Debate.DebateOperations.Clear();
		switch (Debate.State)
		{
		case 0:
			if (Debate.IsTaiwuFirst)
			{
				DebateGameTryApplyComment(context, isTaiwu: true, 0);
				DebateGameTryApplyComment(context, isTaiwu: true, 8);
			}
			else
			{
				DebateGameTryApplyComment(context, isTaiwu: false, 0);
				DebateGameTryApplyComment(context, isTaiwu: false, 8);
			}
			break;
		case 1:
			if (Debate.IsTaiwuFirst)
			{
				DebateGameTryApplyComment(context, isTaiwu: false, 0);
				DebateGameTryApplyComment(context, isTaiwu: false, 8);
			}
			else
			{
				DebateGameTryApplyComment(context, isTaiwu: true, 0);
				DebateGameTryApplyComment(context, isTaiwu: true, 8);
			}
			break;
		}
		Debate.State = (sbyte)((Debate.State + 1) % 3);
		_debateCardUsedCount = 0;
		_debateBeatPawnCount = (taiwu: 0, npc: 0);
		if (Debate.State == 0)
		{
			Debate.IsTaiwuAiProcessedInRound = false;
			Debate.Round++;
			DebateGameUpdateNodeEffects(context);
			if (Debate.Round > 0)
			{
				DebateGameUpdatePlayerStrategyCard(context, isTaiwu: true);
				DebateGameUpdatePlayerStrategyCard(context, isTaiwu: false);
			}
			if (Debate.Round > 1)
			{
				DebateGameRecoverResources(context, isTaiwu: true);
				DebateGameRecoverResources(context, isTaiwu: false);
				DebateGameRemoveDeadPawns();
			}
			DebateGameUpdateRoundStartStrategyEffect(context);
			if (Debate.Round > DebateConstants.PressureAutoIncreaseRound)
			{
				DebateGameChangePlayerPressure((left: DebateConstants.PressureAutoIncreaseValue, right: DebateConstants.PressureAutoIncreaseValue));
			}
			if (Debate.Round >= DebateConstants.MaxRound && !Debate.IsGameOver)
			{
				DebateGameTryProcessEnd();
			}
		}
		if (!Debate.IsGameOver)
		{
			DebateGameProcessState(context);
		}
		return Debate;
	}

	[DomainMethod]
	public DebateGame DebateGameMakeMove(DataContext context, IntPair coordinate, bool isTaiwu, sbyte grade, bool countAsMakeMove = true)
	{
		Tester.Assert(Debate.DebateGrid[coordinate].PawnId < 0);
		if (isTaiwu && !Debate.IsTaiwuAi && countAsMakeMove)
		{
			Debate.DebateOperations.Clear();
		}
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(isTaiwu);
		int pawnGradeToBase = Debate.GetPawnGradeToBase(playerByPlayerIsTaiwu.MaxBases, grade);
		DebateGameChangePlayerBases(playerByPlayerIsTaiwu, -pawnGradeToBase, isTaiwu: false, -1);
		bool flag = DebateGameCheckPressure(context, isTaiwu, 7);
		DebateGameAddPawn(context, coordinate, isTaiwu, grade, 0, flag);
		if (countAsMakeMove)
		{
			DebateGameTryApplyNoCountAsMakeMove(coordinate, isTaiwu, flag);
		}
		if (flag)
		{
			DebateGameApplyPressureEffect(isTaiwu, 7, playerByPlayerIsTaiwu.Pressure, -1);
		}
		return Debate;
	}

	[DomainMethod]
	public DebateGame DebateGameSetTaiwuAi(DataContext context, bool isAi)
	{
		Debate.DebateOperations.Clear();
		Debate.IsTaiwuAi = isAi;
		if (isAi && Debate.GetIsTaiwuTurn())
		{
			DebateGameTaiwuTurn(context);
		}
		return Debate;
	}

	[DomainMethod]
	public void DebateGameSetTaiwuSelectedCardTypes(DataContext context, List<sbyte> types)
	{
		int num = 0;
		if (types != null)
		{
			foreach (sbyte type in types)
			{
				num |= 1 << (int)type;
			}
		}
		DomainManager.Extra.SetTaiwuSelectedDebateCardType(num, context);
	}

	[DomainMethod]
	public List<sbyte> DebateGameGetTaiwuSelectedCardTypes()
	{
		int taiwuSelectedDebateCardType = DomainManager.Extra.GetTaiwuSelectedDebateCardType();
		List<sbyte> list = new List<sbyte>();
		if (taiwuSelectedDebateCardType <= 0)
		{
			return list;
		}
		for (sbyte b = 0; b < 16; b++)
		{
			if ((taiwuSelectedDebateCardType & (1 << (int)b)) != 0)
			{
				list.Add(b);
			}
		}
		return list;
	}

	[DomainMethod]
	public DebateGame DebateGameRemoveCards(bool isTaiwu, List<int> removingCards)
	{
		if (isTaiwu && !Debate.IsTaiwuAi)
		{
			Debate.DebateOperations.Clear();
		}
		removingCards.Sort();
		for (int num = removingCards.Count - 1; num >= 0; num--)
		{
			int index = removingCards[num];
			DebateGameRemovePlayerCanUseCard(isTaiwu, Debate.GetStrategyCardLocation(isTaiwu, 7), index);
		}
		if (removingCards.Count > 0)
		{
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 4, new int[1] { removingCards.Count }));
		}
		return Debate;
	}

	[DomainMethod]
	public DebateGame DebateGameResetCards(bool isTaiwu)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(isTaiwu);
		int pressure = playerByPlayerIsTaiwu.Pressure;
		bool flag = DebateGameSetPlayerPressure(playerByPlayerIsTaiwu, playerByPlayerIsTaiwu.Pressure + GlobalConfig.Instance.DebateResetCardsPressureDelta);
		playerByPlayerIsTaiwu.OwnedCards.AddRange(playerByPlayerIsTaiwu.UsedCards);
		playerByPlayerIsTaiwu.UsedCards.Clear();
		Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 68, new int[1] { playerByPlayerIsTaiwu.Pressure - pressure }));
		if (flag)
		{
			DebateGameTryProcessEnd();
		}
		return Debate;
	}

	[DomainMethod]
	public bool DebateGameTryForceWin(DataContext context)
	{
		int num = Math.Max(1, _taiwuChar.GetLifeSkillAttainment(Debate.LifeSkillType) * (100 + Debate.PlayerLeft.GamePoint * DebateConstants.SurrenderAttainmentFactor) / 100);
		int num2 = Math.Max(1, _debateNpc.GetLifeSkillAttainment(Debate.LifeSkillType) * (100 + Debate.PlayerRight.GamePoint * DebateConstants.SurrenderAttainmentFactor) / 100);
		int percentProb = num * GlobalConfig.Instance.DebateSurrenderFactor / num2 + Math.Max(1, Debate.PlayerRight.Pressure) - DebateConstants.SurrenderBehaviorFactor[_debateNpc.GetBehaviorType()];
		return context.Random.CheckPercentProb(percentProb);
	}

	[DomainMethod]
	public DebateResult DebateGameOver(DataContext context, bool isTaiwuWin, bool isSurrender)
	{
		DebateResult debateResult = new DebateResult();
		sbyte consummateLevel = _taiwuChar.GetConsummateLevel();
		sbyte consummateLevel2 = _debateNpc.GetConsummateLevel();
		short lifeSkillAttainment = _taiwuChar.GetLifeSkillAttainment(_debateLifeSkillType);
		short lifeSkillAttainment2 = _debateNpc.GetLifeSkillAttainment(_debateLifeSkillType);
		bool flag = _debateNpc.GetCreatingType() == 1;
		sbyte happiness = _taiwuChar.GetHappiness();
		sbyte happiness2 = _debateNpc.GetHappiness();
		debateResult.IsTaiwuWin = isTaiwuWin;
		if (isTaiwuWin)
		{
			DebateGameApplyDebateEvaluation(context, 0, isWin: true, debateResult);
			int num = lifeSkillAttainment2 * 100 / lifeSkillAttainment;
			if (num <= DebateConstants.BullyPercent)
			{
				DebateGameApplyDebateEvaluation(context, 2, isWin: true, debateResult);
			}
			else if (num >= DebateConstants.OverComePercent)
			{
				DebateGameApplyDebateEvaluation(context, 3, isWin: true, debateResult);
			}
		}
		else
		{
			DebateGameApplyDebateEvaluation(context, 1, isWin: false, debateResult);
			if (isSurrender)
			{
				DebateGameApplyDebateEvaluation(context, 4, isWin: false, debateResult);
			}
		}
		foreach (int spectator in Debate.Spectators)
		{
			debateResult.Favorability[spectator] = new IntPair(DebateGameGetSpectatorFavor(spectator, _taiwuCharId), 0);
		}
		debateResult.AddHappiness(_taiwuCharId, debateResult.HappinessDelta);
		debateResult.AddHappiness(_debateNpcId, -debateResult.HappinessDelta);
		foreach (int spectator2 in Debate.Spectators)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(spectator2);
			if (element_Objects.GetCreatingType() == 1)
			{
				if (_debateNpcSpectators.Contains(spectator2))
				{
					element_Objects.ChangeHappiness(context, -debateResult.HappinessDelta);
				}
				else
				{
					element_Objects.ChangeHappiness(context, debateResult.HappinessDelta);
				}
			}
		}
		foreach (GameData.Domains.Taiwu.Debate.DebateComment comment in Debate.Comments)
		{
			if (comment.PlayerId == _taiwuCharId || flag)
			{
				DebateGameApplyComment(context, comment, debateResult);
			}
		}
		if (debateResult.Happiness.ContainsKey(_taiwuCharId))
		{
			_taiwuChar.ChangeHappiness(context, debateResult.GetHappiness(_taiwuCharId));
		}
		if (debateResult.Happiness.ContainsKey(_debateNpcId) && _debateNpc.GetCreatingType() == 1)
		{
			_debateNpc.ChangeHappiness(context, debateResult.GetHappiness(_debateNpcId));
		}
		debateResult.IsTaiwuWin = isTaiwuWin;
		debateResult.Happiness[_taiwuCharId] = new IntPair(happiness, _taiwuChar.GetHappiness());
		debateResult.Happiness[_debateNpcId] = new IntPair(happiness2, _debateNpc.GetHappiness());
		foreach (int spectator3 in Debate.Spectators)
		{
			debateResult.Favorability[spectator3] = new IntPair(debateResult.Favorability[spectator3].First, DebateGameGetSpectatorFavor(spectator3, _taiwuCharId));
		}
		int num2 = Math.Clamp(consummateLevel2, 0, GlobalConfig.Instance.CombatGetExpBase.Length - 1);
		int num3 = GlobalConfig.Instance.CombatGetExpBase[num2];
		num3 += debateResult.ExpA;
		num3 = num3 * debateResult.ExpB / 100;
		num3 = num3 * (100 + debateResult.ExpCMax + debateResult.ExpCMin) / 100;
		debateResult.Exp = new IntPair(_taiwuChar.GetExp(), num3);
		_taiwuChar.ChangeExp(context, num3);
		Events.RaiseEvaluationAddExp(context, num3);
		int num4 = Math.Clamp(consummateLevel2, 0, GlobalConfig.Instance.CombatGetAuthorityBase.Length - 1);
		int num5 = GlobalConfig.Instance.CombatGetAuthorityBase[num4];
		num5 += debateResult.AuthorityA;
		num5 = num5 * debateResult.AuthorityB / 100;
		num5 = num5 * (100 + debateResult.AuthorityCMax + debateResult.AuthorityCMin) / 100;
		debateResult.Authority = new IntPair(_taiwuChar.GetResource(7), num5);
		_taiwuChar.ChangeResource(context, 7, num5);
		sbyte readInLifeSkillCombatCount = DomainManager.Extra.GetReadInLifeSkillCombatCount();
		ItemKey curReadingBook = GetCurReadingBook();
		SkillBookItem skillBookItem = Config.SkillBook.Instance[curReadingBook.TemplateId];
		int num6 = 40 + (consummateLevel2 - consummateLevel) * 10;
		num6 += debateResult.ReadRate;
		if (readInLifeSkillCombatCount > 0 && curReadingBook.IsValid() && GetTotalReadingProgress(curReadingBook.Id) < 100 && skillBookItem.LifeSkillTemplateId >= 0 && context.Random.CheckPercentProb(num6))
		{
			debateResult.ShowReadingEvent = true;
			DomainManager.Extra.SetReadInLifeSkillCombatCount((sbyte)(readInLifeSkillCombatCount - 1), context);
			debateResult.ShowReadingEvent2 = UpdateReadingProgressInCombat(context);
			if (debateResult.ShowReadingEvent2)
			{
				DomainManager.Extra.AddReadingEventBookId(context, curReadingBook.Id);
			}
		}
		sbyte loopInLifeSkillCombatCount = DomainManager.Extra.GetLoopInLifeSkillCombatCount();
		short loopingNeigong = _taiwuChar.GetLoopingNeigong();
		CombatSkillKey objectId = new CombatSkillKey(_taiwuChar.GetId(), loopingNeigong);
		int num7 = 40 + (consummateLevel2 - consummateLevel) * 10;
		num7 += debateResult.LoopRate;
		if (loopInLifeSkillCombatCount > 0 && loopingNeigong >= 0 && DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out var element) && element.GetObtainedNeili() >= element.GetTotalObtainableNeili() && context.Random.CheckPercentProb(num7))
		{
			DomainManager.Extra.SetLoopInLifeSkillCombatCount((sbyte)(loopInLifeSkillCombatCount - 1), context);
			ApplyNeigongLoopingImprovementOnce(context);
			debateResult.ShowLoopingEvent = true;
			debateResult.ShowLoopingEvent2 = TryAddLoopingEvent(context, num7);
			InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
			instantNotificationCollection.AddQiArtInLifeSkillCombatNoChance(loopingNeigong);
		}
		if (isTaiwuWin)
		{
			switch (_debateLifeSkillType)
			{
			case 13:
			{
				ProfessionFormulaItem formulaCfg3 = ProfessionFormula.Instance[81];
				int baseDelta3 = formulaCfg3.Calculate(lifeSkillAttainment2);
				DomainManager.Extra.ChangeProfessionSeniority(context, 12, baseDelta3);
				break;
			}
			case 12:
			{
				ProfessionFormulaItem formulaCfg2 = ProfessionFormula.Instance[94];
				int baseDelta2 = formulaCfg2.Calculate(lifeSkillAttainment2);
				DomainManager.Extra.ChangeProfessionSeniority(context, 14, baseDelta2);
				break;
			}
			default:
			{
				ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[30];
				int baseDelta = formulaCfg.Calculate(lifeSkillAttainment2);
				DomainManager.Extra.ChangeProfessionSeniority(context, 4, baseDelta);
				break;
			}
			}
		}
		foreach (int key in debateResult.Happiness.Keys)
		{
			debateResult.CharacterDisplayDataMap[key] = DomainManager.Character.GetCharacterDisplayData(key);
		}
		foreach (int key2 in debateResult.Favorability.Keys)
		{
			debateResult.CharacterDisplayDataMap[key2] = DomainManager.Character.GetCharacterDisplayData(key2);
		}
		_debateStrategyActions = null;
		_debateAiTaiwu = null;
		_debateAiNpc = null;
		Debate = null;
		_debateSpectatorCooldownMap.Clear();
		_debateNpcSpectators.Clear();
		DomainManager.TaiwuEvent.RecordCombatResult(isTaiwuWin);
		return debateResult;
	}

	private void DebateGameProcessState(DataContext context)
	{
		DebateGameUpdateCanMakeMoveNode();
		DebateGameUpdatePlayerMakeMoveCount();
		switch (Debate.State)
		{
		case 0:
			if (Debate.IsTaiwuFirst)
			{
				DebateGameTaiwuTurn(context);
			}
			else
			{
				DebateGameNpcTurn(context);
			}
			break;
		case 1:
			if (Debate.IsTaiwuFirst)
			{
				DebateGameNpcTurn(context);
			}
			else
			{
				DebateGameTaiwuTurn(context);
			}
			break;
		case 2:
			DebateGamePawnTurn(context);
			break;
		}
	}

	private void DebateGameTaiwuTurn(DataContext context)
	{
		if (Debate.IsTaiwuAi && !Debate.IsTaiwuAiProcessedInRound)
		{
			DebateGameStartAiOperation(context, isTaiwu: true);
		}
	}

	private void DebateGameNpcTurn(DataContext context)
	{
		DebateGameStartAiOperation(context, isTaiwu: false);
	}

	private void DebateGamePawnTurn(DataContext context)
	{
		_debateVisitedPawns.Clear();
		for (int i = 0; i < DebateConstants.DebateLineCount; i++)
		{
			DebateGameProcessLine(context, i);
		}
	}

	private void DebateGameRecoverResources(DataContext context, bool isTaiwu)
	{
		DebatePlayer debatePlayer = (isTaiwu ? Debate.PlayerLeft : Debate.PlayerRight);
		bool flag = DebateGameCheckPressure(context, isTaiwu, 5);
		bool flag2 = DebateGameCheckPressure(context, isTaiwu, 4);
		int num = DebateConstants.StrategyPointRecover;
		int num2 = DebateConstants.BasesRecoverPercent;
		if (flag2)
		{
			DebateGameApplyPressureEffect(isTaiwu, 4, debatePlayer.Pressure, -1);
			num = num * DebateConstants.PressureStrategyRecoverPercent / 100;
		}
		if (flag)
		{
			DebateGameApplyPressureEffect(isTaiwu, 5, debatePlayer.Pressure, -1);
			num2 = num2 * DebateConstants.PressureBasesRecoverPercent / 100;
		}
		Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 0, new int[4] { num, 0, num2, 0 }));
		DebateGameChangePlayerStrategyPoint(debatePlayer, num);
		DebateGameChangePlayerBases(debatePlayer, debatePlayer.MaxBases * num2 / 100, isTaiwu: false, -1);
	}

	private void DebateGameRemoveDeadPawns()
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		foreach (var (item, pawn2) in Debate.Pawns)
		{
			if (!pawn2.IsAlive)
			{
				list.Add(item);
			}
		}
		foreach (int item2 in list)
		{
			DebateGameRemovePawn(item2);
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	private void DebateGameUpdateNodeEffects(DataContext context)
	{
		foreach (DebateNode value in Debate.DebateGrid.Values)
		{
			if (value.EffectState.TemplateId >= 0)
			{
				value.EffectState.Duration--;
				if (value.EffectState.Duration <= 0)
				{
					DebateGameRemoveNodeEffect(value);
				}
			}
		}
		foreach (int spectator in Debate.Spectators)
		{
			if (_debateSpectatorCooldownMap[spectator] > 0)
			{
				_debateSpectatorCooldownMap[spectator]--;
			}
			else
			{
				if (!context.Random.CheckPercentProb(DebateConstants.AddNodeEffectProb))
				{
					continue;
				}
				sbyte behaviorType = DomainManager.Character.GetElement_Objects(spectator).GetBehaviorType();
				foreach (DebateNodeEffectItem item in (IEnumerable<DebateNodeEffectItem>)DebateNodeEffect.Instance)
				{
					if (item.BehaviorType == behaviorType && DebateGameTryAddNodeEffect(context, item.TemplateId, spectator))
					{
						break;
					}
				}
			}
		}
		foreach (DebateNode value2 in Debate.DebateGrid.Values)
		{
			if (value2.EffectState.TemplateId >= 0 && value2.PawnId >= 0)
			{
				DebateGameTryApplyNodeEffect(context, value2.Coordinate, EDebateNodeEffectRemoveType.Instant);
			}
		}
	}

	private void DebateGameUpdateRoundStartStrategyEffect(DataContext context)
	{
		foreach (var (pawnId, pawn2) in Debate.Pawns)
		{
			if (pawn2.IsAlive)
			{
				DebateGameApplyPawnStrategyEffectByTriggerType(context, pawnId, EDebateStrategyTriggerType.RoundStart);
			}
		}
	}

	private void DebateGameUpdatePlayerStrategyCard(DataContext context, bool isTaiwu)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(isTaiwu);
		Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, -1, null));
		int num = Math.Min(playerByPlayerIsTaiwu.OwnedCards.Count, Math.Min(GlobalConfig.Instance.DebateMaxShuffleCard, GlobalConfig.Instance.DebateMaxCanUseCards - playerByPlayerIsTaiwu.CanUseCards.Count));
		if (num > 0)
		{
			CollectionUtils.Shuffle(context.Random, playerByPlayerIsTaiwu.OwnedCards);
			for (int i = 0; i < num; i++)
			{
				DebateGameAddPlayerCanUseCard(isTaiwu, Debate.GetStrategyCardLocation(isTaiwu, 6), 0);
			}
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 3, new int[1] { num }));
		}
	}

	private void DebateGameUpdateCanMakeMoveNode()
	{
		for (int i = 0; i < DebateConstants.DebateLineCount; i++)
		{
			for (int j = 0; j < DebateConstants.DebateLineNodeCount; j++)
			{
				IntPair intPair = new IntPair(j, i);
				Debate.DebateGrid[intPair].TaiwuCanMakeMove = Debate.GetNodeCanMakeMove(intPair, isTaiwu: true);
				Debate.DebateGrid[intPair].NpcCanMakeMove = Debate.GetNodeCanMakeMove(intPair, isTaiwu: false);
			}
		}
	}

	private void DebateGameUpdatePlayerMakeMoveCount()
	{
		Debate.PlayerLeft.MakeMoveCount = 0;
		Debate.PlayerRight.MakeMoveCount = 0;
	}

	private void DebateGameTryProcessEnd()
	{
		if (Debate.Round >= DebateConstants.MaxRound)
		{
			Debate.IsTaiwuWin = ((Debate.PlayerLeft.GamePoint == Debate.PlayerRight.GamePoint) ? Debate.IsTaiwuFirst : (Debate.PlayerLeft.GamePoint > Debate.PlayerRight.GamePoint));
			Debate.IsGameOver = true;
		}
		else if (Debate.PlayerLeft.GamePoint <= 0)
		{
			Debate.IsTaiwuWin = Debate.PlayerRight.GamePoint <= 0 && Debate.IsTaiwuFirst;
			Debate.IsGameOver = true;
		}
		else if (Debate.PlayerRight.GamePoint <= 0)
		{
			Debate.IsTaiwuWin = true;
			Debate.IsGameOver = true;
		}
	}

	private void DebateGameExecutePawnConflict(DataContext context, int taiwuPawnId, int npcPawnId, bool isTaiwuPawnTurn)
	{
		int pawnBases = Debate.GetPawnBases(taiwuPawnId, npcPawnId);
		int pawnBases2 = Debate.GetPawnBases(npcPawnId, taiwuPawnId);
		bool flag = pawnBases > pawnBases2 || (pawnBases == pawnBases2 && isTaiwuPawnTurn);
		int result = ((!flag) ? 1 : 0);
		bool flag2 = (flag ? Debate.Pawns[npcPawnId].IsZero : Debate.Pawns[taiwuPawnId].IsZero);
		bool flag3 = (flag ? (pawnBases2 == 0) : (pawnBases == 0));
		DebateGameRevealPawnBasesAndStrategy(context, taiwuPawnId);
		DebateGameRevealPawnBasesAndStrategy(context, npcPawnId);
		if (Debate.TryGetPawnStrategyEffectId(taiwuPawnId, 6, isCastedByTaiwu: true, out var value))
		{
			DebateGameAddStrategyRecord(isTaiwu: true, 27, new List<short> { Debate.ActivatedStrategies[value].TemplateId }, null, null);
		}
		if (Debate.TryGetPawnStrategyEffectId(npcPawnId, 6, isCastedByTaiwu: false, out value))
		{
			DebateGameAddStrategyRecord(isTaiwu: false, 27, new List<short> { Debate.ActivatedStrategies[value].TemplateId }, null, null);
		}
		IntPair bases = new IntPair(pawnBases, pawnBases2);
		Debate.DebateOperations.Add(new DebateOperation(0, taiwuPawnId, npcPawnId, result, bases, Debate.PlayerLeft, Debate.PlayerRight));
		DebateGameProcessPawnConflictResult(context, npcPawnId, pawnBases, !flag);
		DebateGameProcessPawnConflictResult(context, taiwuPawnId, pawnBases2, flag);
		DebateGameChangePlayerPressure((!flag) ? ((flag2 && flag3) ? (left: 0, right: DebateConstants.PressureDeltaInConflict) : (left: DebateConstants.PressureDeltaInConflict, right: 0)) : ((flag2 && flag3) ? (left: DebateConstants.PressureDeltaInConflict, right: 0) : (left: 0, right: DebateConstants.PressureDeltaInConflict)));
	}

	private void DebateGameRevealPawnBasesAndStrategy(DataContext context, int pawnId)
	{
		Pawn pawn = Debate.Pawns[pawnId];
		DebateGameSetPawnRevealed(context, pawnId);
		int[] strategies = pawn.Strategies;
		foreach (int num in strategies)
		{
			if (num >= 0)
			{
				Debate.ActivatedStrategies[num].IsRevealed = true;
			}
		}
	}

	private void DebateGameProcessPawnConflictResult(DataContext context, int pawnId, int otherPawnBases, bool isWin)
	{
		_oneTimeStrategyActionList.Clear();
		Pawn pawn = Debate.Pawns[pawnId];
		DebateGameApplyPawnStrategyEffectByTriggerType(context, pawnId, (!isWin) ? EDebateStrategyTriggerType.ConflictLose : EDebateStrategyTriggerType.ConflictWin);
		if (!isWin)
		{
			DebateGameSetPawnDead(context, pawnId, 0);
		}
		if (pawn.IsAlive && !pawn.IsImmuneDebuff && !pawn.IsImmuneRemove)
		{
			DebateGameSetPawnBases(pawnId, pawn.Bases - otherPawnBases);
		}
		foreach (Action oneTimeStrategyAction in _oneTimeStrategyActionList)
		{
			oneTimeStrategyAction?.Invoke();
		}
	}

	private void DebateGameProcessLine(DataContext context, int y)
	{
		bool flag = y % 2 == 0 && Debate.IsTaiwuFirst;
		bool flag2 = flag;
		bool[] array = new bool[2];
		while (!array[0] || !array[1])
		{
			int num = ((!flag2) ? 1 : 0);
			int num2 = ((!flag2) ? 1 : (-1));
			int num3 = -1;
			int num4 = (flag2 ? (DebateConstants.DebateLineNodeCount - 1) : 0);
			if (array[num])
			{
				flag2 = !flag2;
				continue;
			}
			for (int i = num4; Debate.GetCoordinateValid(i); i += num2)
			{
				num3 = Debate.DebateGrid[new IntPair(i, y)].PawnId;
				if (num3 >= 0 && Debate.Pawns[num3].IsOwnedByTaiwu == flag2 && !_debateVisitedPawns.Contains(num3))
				{
					break;
				}
				num3 = -1;
			}
			if (num3 < 0)
			{
				array[num] = true;
				flag2 = !flag2;
				continue;
			}
			Pawn pawn = Debate.Pawns[num3];
			IntPair pawnTargetPosition = Debate.GetPawnTargetPosition(num3);
			if (!Debate.GetCoordinateValid(pawnTargetPosition))
			{
				continue;
			}
			DebateGameTryApplyPawnMoveMore(num3);
			DebateGameApplyPawnStrategyEffectByTriggerType(context, num3, EDebateStrategyTriggerType.PawnActing);
			if (Debate.GetPawnIsHalt(num3))
			{
				if (Debate.TryGetPawnStrategyEffectId(num3, 34, !pawn.IsOwnedByTaiwu, out var value))
				{
					DebateGameAddStrategyRecord(pawn.IsOwnedByTaiwu, 61, new List<short> { Debate.ActivatedStrategies[value].TemplateId }, null, null);
				}
				flag2 = !flag2;
				continue;
			}
			int pawnId = Debate.DebateGrid[pawnTargetPosition].PawnId;
			if (pawnId < 0)
			{
				DebateGamePawnMoveForward(context, num3);
				if (_debateVisitedPawns.Contains(num3))
				{
					DebateGameTryApplyPawnMoveMore(num3);
				}
				if (_debateVisitedPawns.Contains(num3))
				{
					flag2 = !flag2;
				}
				continue;
			}
			if (Debate.Pawns[num3].IsOwnedByTaiwu == Debate.Pawns[pawnId].IsOwnedByTaiwu)
			{
				if (_debateVisitedPawns.Contains(num3))
				{
					flag2 = !flag2;
				}
				continue;
			}
			int num5 = (flag2 ? num3 : pawnId);
			int num6 = (flag2 ? pawnId : num3);
			DebateGameApplyPawnStrategyEffectByTriggerType(context, num3, EDebateStrategyTriggerType.ConflictStart);
			DebateGameApplyPawnStrategyEffectByTriggerType(context, pawnId, EDebateStrategyTriggerType.ConflictStart);
			if (pawn.IsSwitchLocation || Debate.Pawns[pawnId].IsSwitchLocation)
			{
				IntPair coordinate = Debate.Pawns[num5].Coordinate;
				IntPair coordinate2 = Debate.Pawns[num6].Coordinate;
				DebateGameSetPawnCoordinate(context, num5, coordinate2);
				DebateGameSetPawnCoordinate(context, num6, coordinate);
			}
			else
			{
				DebateGameExecutePawnConflict(context, num5, num6, flag2);
				if (Debate.Pawns[num3].IsAlive && Debate.GetNodeCanTeleportPawn(Debate.GetPawnTargetPosition(num3)))
				{
					DebateGamePawnMoveForward(context, num3);
				}
			}
			if (_debateVisitedPawns.Contains(num3))
			{
				DebateGameTryApplyPawnMoveMore(num3);
			}
			if (_debateVisitedPawns.Contains(num3))
			{
				flag2 = !flag2;
			}
		}
	}

	private void DebateGamePawnMoveForward(DataContext context, int pawnId)
	{
		IntPair pawnTargetPosition = Debate.GetPawnTargetPosition(pawnId);
		if (Debate.DebateGrid[pawnTargetPosition].PawnId < 0)
		{
			DebateGameSetPawnCoordinate(context, pawnId, pawnTargetPosition);
		}
		DebateGameApplyPawnStrategyEffectByTriggerType(context, pawnId, EDebateStrategyTriggerType.PawnForward);
	}

	private void DebateGameApplyPawnDamage(DataContext context, int pawnId)
	{
		Pawn pawn = Debate.Pawns[pawnId];
		if (pawn.Damaged)
		{
			return;
		}
		DebateGameApplyPawnStrategyEffectByTriggerType(context, pawnId, EDebateStrategyTriggerType.PawnDamage);
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(!pawn.IsOwnedByTaiwu);
		DebatePlayer playerByPlayerIsTaiwu2 = Debate.GetPlayerByPlayerIsTaiwu(pawn.IsOwnedByTaiwu);
		if (pawn.NodeDamage > 0)
		{
			DebateGameChangePlayerGamePoint(playerByPlayerIsTaiwu, -pawn.NodeDamage, needPlay: true, !pawn.IsOwnedByTaiwu, 4);
		}
		DebateGameChangePlayerGamePoint(playerByPlayerIsTaiwu, -DebateConstants.PawnDamageToGamePoint, needPlay: false, !pawn.IsOwnedByTaiwu, -1);
		Debate.DebateOperations.Add(new DebateOperation(4, !pawn.IsOwnedByTaiwu, pawnId, pawn.Coordinate, Debate.PlayerLeft, Debate.PlayerRight));
		Debate.DebateOperations.Add(new DebateOperation(20, !pawn.IsOwnedByTaiwu, 2, new int[4]
		{
			0,
			0,
			DebateConstants.PawnDamageToGamePoint,
			0
		}));
		foreach (PawnDamageInfo damage in pawn.DamageList)
		{
			if (!damage.IsToSelf && !damage.IsStrategyDamage)
			{
				DebateGameChangePlayerGamePoint(playerByPlayerIsTaiwu, -damage.Damage, needPlay: true, damage.IsTaiwuCasted, -1);
			}
		}
		int num = DebateConstants.PawnDamageToGamePoint + pawn.DamageToOpponent;
		DebateGameChangePlayerGamePoint(playerByPlayerIsTaiwu2, num * pawn.ChangeSelfGamePointByDamagePercent / 100, needPlay: true, pawn.ChangeSelfGamePointByDamagePercentIsCastedByTaiwu, -1);
		foreach (PawnDamageInfo damage2 in pawn.DamageList)
		{
			if (!damage2.IsToSelf && damage2.IsStrategyDamage)
			{
				DebateGameChangePlayerGamePoint(playerByPlayerIsTaiwu, -damage2.Damage, needPlay: true, damage2.IsTaiwuCasted, -1);
			}
		}
		foreach (PawnDamageInfo damage3 in pawn.DamageList)
		{
			if (damage3.IsToSelf)
			{
				DebateGameChangePlayerGamePoint(playerByPlayerIsTaiwu2, -damage3.Damage, needPlay: true, damage3.IsTaiwuCasted, -1);
			}
		}
		_debateAiTaiwu.UpdateLineWeightByDamage(pawn.IsOwnedByTaiwu, pawn.Coordinate.Second);
		_debateAiNpc.UpdateLineWeightByDamage(!pawn.IsOwnedByTaiwu, pawn.Coordinate.Second);
		DebateGameSetPawnDead(context, pawnId, 1);
		pawn.Damaged = true;
	}

	private int DebateGameGetSpectatorFavor(int spectatorId, int playerId)
	{
		RelatedCharacter relation;
		return DomainManager.Character.TryGetRelation(spectatorId, playerId, out relation) ? relation.Favorability : 0;
	}

	private void DebateGameApplyDebateEvaluation(DataContext context, short templateId, bool isWin, DebateResult result)
	{
		DebateEvaluationItem debateEvaluationItem = DebateEvaluation.Instance[templateId];
		result.ExpA += debateEvaluationItem.ExpA;
		result.ExpB += debateEvaluationItem.ExpB;
		result.AuthorityA += debateEvaluationItem.AuthorityA;
		result.AuthorityB += debateEvaluationItem.AuthorityB;
		result.FavorIncreaseB += debateEvaluationItem.FavorIncreaseB;
		result.FavorDecreaseB += debateEvaluationItem.FavorDecreaseB;
		result.ReadRate += debateEvaluationItem.ReadRate;
		result.LoopRate += debateEvaluationItem.LoopRate;
		result.HappinessDelta += debateEvaluationItem.HappinessDelta;
		result.Evaluations.Add(templateId);
		if (debateEvaluationItem.ExpC > result.ExpCMax)
		{
			result.ExpCMax = debateEvaluationItem.ExpC;
		}
		else if (debateEvaluationItem.ExpC < result.ExpCMin)
		{
			result.ExpCMin = debateEvaluationItem.ExpC;
		}
		if (debateEvaluationItem.AuthorityC > result.AuthorityCMax)
		{
			result.AuthorityCMax = debateEvaluationItem.AuthorityC;
		}
		else if (debateEvaluationItem.AuthorityC < result.AuthorityCMin)
		{
			result.AuthorityCMin = debateEvaluationItem.AuthorityC;
		}
		if (debateEvaluationItem.FavorIncreaseC > result.FavorIncreaseCMax)
		{
			result.FavorIncreaseCMax = debateEvaluationItem.FavorIncreaseC;
		}
		else if (debateEvaluationItem.FavorIncreaseC < result.FavorIncreaseCMin)
		{
			result.FavorIncreaseCMin = debateEvaluationItem.FavorIncreaseC;
		}
		if (debateEvaluationItem.FavorDecreaseC > result.FavorDecreaseCMax)
		{
			result.FavorDecreaseCMax = debateEvaluationItem.FavorDecreaseC;
		}
		else if (debateEvaluationItem.FavorDecreaseC < result.FavorDecreaseCMin)
		{
			result.FavorDecreaseCMin = debateEvaluationItem.FavorDecreaseC;
		}
		if (debateEvaluationItem.FameAction >= 0)
		{
			_taiwuChar.RecordFameAction(context, debateEvaluationItem.FameAction, -1, 1);
		}
		foreach (LegacyPointReference item in debateEvaluationItem.AddLegacyPoint)
		{
			int num = (isWin ? item.WinPercent : item.FailPercent);
			if (num > 0)
			{
				DomainManager.Taiwu.AddLegacyPoint(context, item.TemplateId, num);
			}
		}
	}

	private void DebateGameApplyComment(DataContext context, GameData.Domains.Taiwu.Debate.DebateComment comment, DebateResult result)
	{
		bool flag = comment.PlayerId == _taiwuCharId;
		GameData.Domains.Character.Character character = (flag ? _taiwuChar : _debateNpc);
		DebateCommentItem debateCommentItem = Config.DebateComment.Instance[comment.TemplateId];
		int favor = debateCommentItem.Favor;
		Dictionary<short, int> dictionary = (flag ? result.TaiwuComments : result.NpcComments);
		if (dictionary.TryGetValue(comment.TemplateId, out var value))
		{
			if (value >= DebateConstants.CommentStackLimit)
			{
				return;
			}
			dictionary[comment.TemplateId]++;
		}
		else
		{
			dictionary[comment.TemplateId] = 1;
		}
		result.AddHappiness(comment.PlayerId, debateCommentItem.Happiness[character.GetBehaviorType()]);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(comment.SpectatorId);
		if (element_Objects.GetCreatingType() != 1)
		{
			return;
		}
		favor += result.FavorA;
		if (favor != 0)
		{
			if (favor > 0)
			{
				favor = favor * result.FavorIncreaseB / 100;
				favor = favor * (100 + result.FavorIncreaseCMax + result.FavorIncreaseCMin) / 100;
			}
			else
			{
				favor = favor * result.FavorDecreaseB / 100;
				favor = favor * (100 + result.FavorDecreaseCMax + result.FavorDecreaseCMin) / 100;
			}
			DomainManager.Character.ChangeFavorabilityOptional(context, element_Objects, character, favor, -1);
		}
	}

	private void DebateGameTryApplyComment(DataContext context, bool isTaiwu, short templateId)
	{
		int id = Debate.GetPlayerByPlayerIsTaiwu(isTaiwu).Id;
		DebateCommentItem debateCommentItem = Config.DebateComment.Instance[templateId];
		if (1 == 0)
		{
		}
		bool flag = templateId switch
		{
			0 => DebateGameComment0(isTaiwu, debateCommentItem.CheckValue), 
			2 => DebateGameComment1(isTaiwu, debateCommentItem.CheckValue), 
			4 => DebateGameComment2(isTaiwu, debateCommentItem.CheckValue), 
			6 => DebateGameComment3(isTaiwu, debateCommentItem.CheckValue), 
			8 => DebateGameComment4(isTaiwu, debateCommentItem.CheckValue), 
			_ => throw new ArgumentOutOfRangeException("templateId", templateId, null), 
		};
		if (1 == 0)
		{
		}
		if (!flag)
		{
			return;
		}
		foreach (int spectator in Debate.Spectators)
		{
			sbyte behaviorType = DomainManager.Character.GetElement_Objects(spectator).GetBehaviorType();
			int num = DebateGameGetSpectatorFavor(spectator, id);
			int num2 = ((!isTaiwu) ? (_debateNpcSpectators.Contains(spectator) ? DebateConstants.SameSideCommentProb : DebateConstants.OtherSideCommentProb) : (_debateNpcSpectators.Contains(spectator) ? DebateConstants.OtherSideCommentProb : DebateConstants.SameSideCommentProb));
			bool flag2 = context.Random.CheckPercentProb(num2 + num / DebateConstants.CommentDivider);
			if (behaviorType != debateCommentItem.BehaviorType)
			{
				debateCommentItem = Config.DebateComment.Instance[debateCommentItem.Negation];
				if (behaviorType != debateCommentItem.BehaviorType)
				{
					continue;
				}
			}
			if (context.Random.CheckPercentProb(DebateConstants.CommentProb) && debateCommentItem.IsPositive == flag2)
			{
				Debate.Comments.Add(new GameData.Domains.Taiwu.Debate.DebateComment(spectator, id, debateCommentItem.TemplateId));
				Debate.DebateOperations.Add(new DebateOperation(5, isTaiwu, new IntPair(spectator, id), debateCommentItem.TemplateId, Debate.PlayerLeft, Debate.PlayerRight));
				Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 5, new int[3] { spectator, id, debateCommentItem.TemplateId }));
			}
		}
	}

	private bool DebateGameComment0(bool isTaiwu, int value)
	{
		return _debateCardUsedCount == value;
	}

	private bool DebateGameComment1(bool isTaiwu, int value)
	{
		bool flag = _debateRevealedPawnCount.taiwu == value;
		bool flag2 = _debateRevealedPawnCount.npc == value;
		bool result = (isTaiwu ? flag : flag2);
		if (isTaiwu && flag)
		{
			_debateRevealedPawnCount.taiwu = 0;
		}
		if (!isTaiwu && flag2)
		{
			_debateRevealedPawnCount.npc = 0;
		}
		return result;
	}

	private bool DebateGameComment2(bool isTaiwu, int value)
	{
		return Debate.GetPawnCount(isTaiwu) >= value;
	}

	private bool DebateGameComment3(bool isTaiwu, int value)
	{
		return isTaiwu ? (_debateBeatPawnCount.taiwu >= value) : (_debateBeatPawnCount.npc >= value);
	}

	private bool DebateGameComment4(bool isTaiwu, int value)
	{
		return _debateCardUsedCount != 0 && (isTaiwu ? (Debate.PlayerLeft.CanUseCards.Count == value) : (Debate.PlayerRight.CanUseCards.Count == value));
	}

	private int DebateGameCompareSpectatorByNpcImpression(int a, int b)
	{
		int num = DebateGameGetSpectatorFavor(a, _debateNpcId);
		int num2 = DebateGameGetSpectatorFavor(b, _debateNpcId);
		return (num == num2) ? (-DomainManager.Character.GetElement_Objects(a).GetLifeSkillAttainment(_debateLifeSkillType).CompareTo(DomainManager.Character.GetElement_Objects(b).GetLifeSkillAttainment(_debateLifeSkillType))) : (-num.CompareTo(num2));
	}

	private int DebateGameCompareSpectatorByTaiwuImpression(int a, int b)
	{
		int num = DebateGameGetSpectatorFavor(a, _taiwuCharId);
		int num2 = DebateGameGetSpectatorFavor(b, _taiwuCharId);
		return (num == num2) ? (-DomainManager.Character.GetElement_Objects(a).GetLifeSkillAttainment(_debateLifeSkillType).CompareTo(DomainManager.Character.GetElement_Objects(b).GetLifeSkillAttainment(_debateLifeSkillType))) : num.CompareTo(num2);
	}

	private int DebateGameCompareSpectatorByMixedImpression(int a, int b)
	{
		int val = DebateGameGetSpectatorFavor(a, _debateNpcId);
		int num = DebateGameGetSpectatorFavor(a, _taiwuCharId);
		int num2 = Math.Max(-num, val);
		int val2 = DebateGameGetSpectatorFavor(b, _debateNpcId);
		int num3 = DebateGameGetSpectatorFavor(b, _taiwuCharId);
		int num4 = Math.Max(-num3, val2);
		return (num2 == num4) ? (-DomainManager.Character.GetElement_Objects(a).GetLifeSkillAttainment(_debateLifeSkillType).CompareTo(DomainManager.Character.GetElement_Objects(b).GetLifeSkillAttainment(_debateLifeSkillType))) : (-num2.CompareTo(num4));
	}

	private int DebateGameCompareSpectatorByAttainment(int a, int b)
	{
		return -DomainManager.Character.GetElement_Objects(a).GetLifeSkillAttainment(_debateLifeSkillType).CompareTo(DomainManager.Character.GetElement_Objects(b).GetLifeSkillAttainment(_debateLifeSkillType));
	}

	private bool DebateGameCheckPressure(DataContext context, bool isTaiwu, sbyte type)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(isTaiwu);
		sbyte pressureType = Debate.GetPressureType(playerByPlayerIsTaiwu.Pressure, playerByPlayerIsTaiwu.MaxPressure);
		if (1 == 0)
		{
		}
		int num = type switch
		{
			4 => DebateConstants.ReduceStrategyRecoverProb[pressureType], 
			5 => DebateConstants.ReduceBasesRecoverProb[pressureType], 
			6 => DebateConstants.UseStrategyFailedProb[pressureType], 
			7 => DebateConstants.MakeMoveFailedProb[pressureType], 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		int percentProb = num;
		return context.Random.CheckPercentProb(percentProb);
	}

	private void DebateGameApplyPressureEffect(bool isTaiwu, sbyte type, int pressure, short templateId = -1)
	{
		switch (type)
		{
		case 5:
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 13, new int[3] { 0, pressure, 0 }));
			break;
		case 4:
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 14, new int[3] { 0, pressure, 0 }));
			break;
		case 7:
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 15, new int[3] { 0, pressure, 0 }));
			break;
		case 6:
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 16, new int[3] { 0, pressure, templateId }));
			break;
		}
	}

	private void DebateGameTryApplyNodeEffect(DataContext context, IntPair coordinate, EDebateNodeEffectRemoveType type)
	{
		if (!Debate.GetCoordinateValid(coordinate))
		{
			return;
		}
		DebateNode debateNode = Debate.DebateGrid[coordinate];
		if (debateNode.PawnId < 0)
		{
			return;
		}
		Pawn pawn = Debate.Pawns[debateNode.PawnId];
		pawn.ResetNodeValue();
		DebateNodeEffectState effectState = debateNode.EffectState;
		if (effectState.TemplateId < 0)
		{
			return;
		}
		DebateNodeEffectItem debateNodeEffectItem = DebateNodeEffect.Instance[effectState.TemplateId];
		int value = 0;
		if (1 == 0)
		{
		}
		List<IntPair> list = type switch
		{
			EDebateNodeEffectRemoveType.Instant => debateNodeEffectItem.InstantEffectList, 
			EDebateNodeEffectRemoveType.Trigger => debateNodeEffectItem.TriggerEffectList, 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
		if (1 == 0)
		{
		}
		List<IntPair> list2 = list;
		if (list2.Count == 0)
		{
			return;
		}
		DebateOperation item = new DebateOperation(19, effectState, debateNode.Coordinate, pawn.IsOwnedByTaiwu);
		Debate.DebateOperations.Add(item);
		if (debateNodeEffectItem.RemoveType.Contains(type))
		{
			DebateGameRemoveNodeEffect(debateNode);
		}
		foreach (IntPair item2 in list2)
		{
			if (_debateStrategyActions.TryGetValue(item2.First, out var value2))
			{
				value = item2.Second;
				DebateStrategyActionParams obj = new DebateStrategyActionParams
				{
					Context = context,
					PawnId = debateNode.PawnId,
					Value = item2.Second,
					NodeEffect = effectState,
					Coordinate = debateNode.Coordinate,
					IsCastedByTaiwu = pawn.IsOwnedByTaiwu
				};
				value2(obj);
			}
		}
		if (debateNodeEffectItem.DebateRecord >= 0)
		{
			int[] recordParams = DebateGameGetDebateNodeRecordParameters(debateNodeEffectItem.DebateRecord, effectState, value);
			Debate.DebateOperations.Add(new DebateOperation(20, pawn.IsOwnedByTaiwu, debateNodeEffectItem.DebateRecord, recordParams));
		}
	}

	private void DebateGameTryApplyPawnMoveMore(int pawnId)
	{
		Pawn pawn = Debate.Pawns[pawnId];
		if (pawn.IsAlive)
		{
			if (Debate.GetNodeIsContainingEffect(pawn.Coordinate, 44))
			{
				int[] recordParams = DebateGameGetDebateNodeRecordParameters(7, Debate.DebateGrid[pawn.Coordinate].EffectState, 1);
				_debateVisitedPawns.Remove(pawnId);
				DebateGameRemoveNodeEffect(Debate.DebateGrid[pawn.Coordinate]);
				Debate.DebateOperations.Add(new DebateOperation(20, pawn.IsOwnedByTaiwu, 7, recordParams));
			}
			else
			{
				_debateVisitedPawns.Add(pawnId);
			}
		}
	}

	private void DebateGameTryApplyNoCountAsMakeMove(IntPair coordinate, bool isTaiwu, bool makeMoveFailed)
	{
		if (Debate.GetNodeIsContainingEffect(coordinate, 41))
		{
			DebateNode debateNode = Debate.DebateGrid[coordinate];
			DebateNodeEffectState effectState = debateNode.EffectState;
			int[] recordParams = DebateGameGetDebateNodeRecordParameters(9, effectState, 1);
			Debate.DebateOperations.Add(new DebateOperation(19, effectState, debateNode.Coordinate, isTaiwu));
			DebateGameRemoveNodeEffect(Debate.DebateGrid[coordinate]);
			if (!makeMoveFailed)
			{
				Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 9, recordParams));
			}
		}
		else
		{
			DebateGameChangePlayerMakeMoveCount(Debate.GetPlayerByPlayerIsTaiwu(isTaiwu), 1);
			if (!makeMoveFailed)
			{
				Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 1, new int[2] { 1, 0 }));
			}
		}
	}

	private int[] DebateGameGetDebateNodeRecordParameters(short templateId, DebateNodeEffectState effect, int value)
	{
		DebateRecordItem debateRecordItem = DebateRecord.Instance[templateId];
		int[] array = new int[debateRecordItem.Parameters.Length];
		for (int i = 0; i < debateRecordItem.Parameters.Length; i++)
		{
			int[] array2 = array;
			int num = i;
			EDebateRecordParamType eDebateRecordParamType = debateRecordItem.Parameters[i];
			if (1 == 0)
			{
			}
			int num2 = eDebateRecordParamType switch
			{
				EDebateRecordParamType.IntValue => value, 
				EDebateRecordParamType.NodeEffect => effect.Id, 
				EDebateRecordParamType.Spectator => effect.CasterId, 
				EDebateRecordParamType.Character => effect.IsHelpTaiwu ? _debateNpcId : _taiwuCharId, 
				_ => 0, 
			};
			if (1 == 0)
			{
			}
			array2[num] = num2;
		}
		return array;
	}

	private bool DebateGameGetSpectatorHelpTaiwu(DataContext context, int spectatorId, bool isTaiwu)
	{
		bool flag = !_debateNpcSpectators.Contains(spectatorId);
		int num = DebateGameGetSpectatorFavor(spectatorId, Debate.GetPlayerByPlayerIsTaiwu(isTaiwu).Id);
		int percentProb = DebateConstants.SpectatorHelpSameSideBase + num / DebateConstants.SpectatorHelpSameSideDivider;
		return context.Random.CheckPercentProb(percentProb) == flag;
	}

	private bool DebateGameTryAddNodeEffect(DataContext context, short templateId, int spectatorId)
	{
		bool isTaiwu = !_debateNpcSpectators.Contains(spectatorId);
		bool flag = DebateGameGetSpectatorHelpTaiwu(context, spectatorId, isTaiwu);
		bool decide = context.Random.CheckPercentProb(DebateConstants.SpectatorHelpSameSideBase);
		if (1 == 0)
		{
		}
		List<DebateNode> list = templateId switch
		{
			0 => DebateGameGetAvailableNodeJust(flag), 
			1 => DebateGameGetAvailableNodeKind(flag), 
			2 => DebateGameGetAvailableNodeEven(flag), 
			3 => DebateGameGetAvailableNodeRebel(decide), 
			4 => DebateGameGetAvailableNodeEgoistic(flag), 
			_ => null, 
		};
		if (1 == 0)
		{
		}
		List<DebateNode> list2 = list;
		if (list2 == null)
		{
			return false;
		}
		CollectionUtils.Shuffle(context.Random, list2);
		DebateGameAddNodeEffect(list2[0], templateId, spectatorId, flag);
		return true;
	}

	private List<DebateNode> DebateGameGetAvailableNodeJust(bool isTaiwuSide)
	{
		List<DebateNode> list = null;
		int startCoordinate = Debate.GetStartCoordinate(!isTaiwuSide);
		foreach (DebateNode value in Debate.DebateGrid.Values)
		{
			if (value.Coordinate.First != startCoordinate && value.PawnId >= 0 && Debate.Pawns[value.PawnId].IsOwnedByTaiwu != isTaiwuSide && !Debate.Pawns[value.PawnId].IsRevealed && value.EffectState.TemplateId < 0)
			{
				if (list == null)
				{
					list = new List<DebateNode>();
				}
				list.Add(value);
			}
		}
		if (list != null)
		{
			return list;
		}
		foreach (DebateNode value2 in Debate.DebateGrid.Values)
		{
			if (value2.Coordinate.First != startCoordinate && value2.IsVantage != isTaiwuSide && value2.EffectState.TemplateId < 0)
			{
				if (list == null)
				{
					list = new List<DebateNode>();
				}
				list.Add(value2);
			}
		}
		return list;
	}

	private List<DebateNode> DebateGameGetAvailableNodeKind(bool isTaiwuSide)
	{
		List<DebateNode> list = null;
		int startCoordinate = Debate.GetStartCoordinate(isTaiwuSide);
		foreach (DebateNode value in Debate.DebateGrid.Values)
		{
			if (value.Coordinate.First != startCoordinate && value.IsVantage == isTaiwuSide && value.PawnId < 0 && value.EffectState.TemplateId < 0)
			{
				if (list == null)
				{
					list = new List<DebateNode>();
				}
				list.Add(value);
			}
		}
		return list;
	}

	private List<DebateNode> DebateGameGetAvailableNodeEven(bool isTaiwuSide)
	{
		List<DebateNode> list = null;
		int startCoordinate = Debate.GetStartCoordinate(isTaiwuSide);
		foreach (DebateNode value in Debate.DebateGrid.Values)
		{
			if (value.Coordinate.First != startCoordinate && value.IsVantage == isTaiwuSide && value.EffectState.TemplateId < 0 && value.PawnId < 0)
			{
				if (list == null)
				{
					list = new List<DebateNode>();
				}
				list.Add(value);
			}
		}
		if (list == null)
		{
			foreach (DebateNode value2 in Debate.DebateGrid.Values)
			{
				if (value2.Coordinate.First != startCoordinate && value2.IsVantage == isTaiwuSide && value2.EffectState.TemplateId < 0)
				{
					if (list == null)
					{
						list = new List<DebateNode>();
					}
					list.Add(value2);
				}
			}
		}
		return list;
	}

	private List<DebateNode> DebateGameGetAvailableNodeRebel(bool decide)
	{
		if (!decide)
		{
			return null;
		}
		List<DebateNode> list = null;
		int startCoordinate = Debate.GetStartCoordinate(isTaiwu: true);
		int startCoordinate2 = Debate.GetStartCoordinate(isTaiwu: false);
		foreach (DebateNode value in Debate.DebateGrid.Values)
		{
			if (value.Coordinate.First != startCoordinate && value.Coordinate.First != startCoordinate2 && value.PawnId < 0 && value.EffectState.TemplateId < 0)
			{
				if (list == null)
				{
					list = new List<DebateNode>();
				}
				list.Add(value);
			}
		}
		return list;
	}

	private List<DebateNode> DebateGameGetAvailableNodeEgoistic(bool isTaiwuSide)
	{
		List<DebateNode> list = null;
		int startCoordinate = Debate.GetStartCoordinate(!isTaiwuSide);
		foreach (DebateNode value in Debate.DebateGrid.Values)
		{
			if (value.Coordinate.First == startCoordinate && value.EffectState.TemplateId < 0)
			{
				if (list == null)
				{
					list = new List<DebateNode>();
				}
				list.Add(value);
			}
		}
		return list;
	}

	private void DebateGameInitializeAi(DataContext context, GameData.Domains.Character.Character npc)
	{
		_debateAiTaiwu = new DebateAi(isTaiwu: true, _taiwuChar.GetBehaviorType());
		_debateAiNpc = new DebateAi(isTaiwu: false, npc.GetBehaviorType());
		_debateAiTaiwu.Initialize(context);
		_debateAiNpc.Initialize(context);
	}

	private void DebateGameStartAiOperation(DataContext context, bool isTaiwu)
	{
		if (isTaiwu)
		{
			Debate.IsTaiwuAiProcessedInRound = true;
		}
		DebateAi debateAi = (isTaiwu ? _debateAiTaiwu : _debateAiNpc);
		debateAi.Start(context);
	}

	[DomainMethod]
	public void GmCmd_SetForceAiBribery(bool value)
	{
		_forceAiBribery = value;
	}

	[DomainMethod]
	public OperationForceAdversary GetAiBriberyDataOnPrepareLifeSkillCombat(DataContext context, int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		int percentProb = 0;
		switch (element_Objects.GetBehaviorType())
		{
		case 2:
			percentProb = 10;
			break;
		case 3:
			percentProb = 13;
			break;
		case 4:
			percentProb = 16;
			break;
		}
		if (DomainManager.Extra.GetAiActionRateAdjust(charId, 11, -1) != 0)
		{
			percentProb = 0;
		}
		if (_forceAiBribery)
		{
			percentProb = 100;
		}
		if (context.Random.CheckPercentProb(percentProb))
		{
			bool flag = false;
			switch (element_Objects.GetBehaviorType())
			{
			case 1:
				flag = context.Random.CheckPercentProb(70);
				break;
			case 2:
				flag = context.Random.CheckPercentProb(50);
				break;
			case 3:
				flag = context.Random.CheckPercentProb(80);
				break;
			case 4:
				flag = context.Random.CheckPercentProb(40);
				break;
			}
			if (flag)
			{
				SecretInformationDisplayPackage secretInformationDisplayPackage = DomainManager.Taiwu.PickLifeSkillCombatCharacterUseSecretInformation(context, charId);
				if (secretInformationDisplayPackage != null && secretInformationDisplayPackage.SecretInformationDisplayDataList.Count > 0)
				{
					return new OperationForceAdversary(0, 0, secretInformationDisplayPackage);
				}
			}
			ItemDisplayData itemDisplayData = DomainManager.Taiwu.PickLifeSkillCombatCharacterUseItem(context, charId);
			if (itemDisplayData != null)
			{
				return new OperationForceAdversary(0, 0, itemDisplayData);
			}
		}
		return null;
	}

	private bool DebateGameSetPlayerPressure(DebatePlayer player, int value)
	{
		(int pressure, int gamePoint) gamePointAndPressureDelta = Debate.GetGamePointAndPressureDelta(player, value);
		int item = gamePointAndPressureDelta.pressure;
		int item2 = gamePointAndPressureDelta.gamePoint;
		player.Pressure = item;
		player.HighestPressure = Math.Max(player.Pressure, player.HighestPressure);
		if (item2 < 0)
		{
			bool isTaiwu = player.Id == Debate.PlayerLeft.Id;
			player.GamePoint = Math.Max(0, player.GamePoint + item2);
			Debate.DebateOperations.Add(new DebateOperation(9, isTaiwu, Debate.PlayerLeft, Debate.PlayerRight, -1));
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, 12, new int[4]
			{
				0,
				item,
				Math.Abs(item2),
				0
			}));
		}
		return player.GamePoint == 0;
	}

	private void DebateGameChangePlayerMakeMoveCount(DebatePlayer player, int delta)
	{
		player.MakeMoveCount += delta;
	}

	private void DebateGameChangePlayerPressure((int left, int right) delta)
	{
		bool flag = false;
		bool flag2 = false;
		if (delta.left != 0)
		{
			flag = DebateGameSetPlayerPressure(Debate.PlayerLeft, Debate.PlayerLeft.Pressure + delta.left);
		}
		if (delta.right != 0)
		{
			flag2 = DebateGameSetPlayerPressure(Debate.PlayerRight, Debate.PlayerRight.Pressure + delta.right);
		}
		if (flag || flag2)
		{
			DebateGameTryProcessEnd();
		}
	}

	private void DebateGameChangePlayerGamePoint(DebatePlayer player, int delta, bool needPlay = true, bool isTaiwuCasted = false, short nodeEffectTemplateId = -1)
	{
		if (delta == 0)
		{
			return;
		}
		int num = Math.Min(DebateConstants.MaxGamePoint, player.GamePoint + delta);
		if (player.GamePoint != num)
		{
			player.GamePoint = num;
			if (needPlay)
			{
				Debate.DebateOperations.Add(new DebateOperation(11, isTaiwuCasted, Debate.PlayerLeft, Debate.PlayerRight, nodeEffectTemplateId));
			}
			DebateGameTryProcessEnd();
		}
	}

	private void DebateGameAddPawn(DataContext context, IntPair coordinate, bool isOwnedByTaiwu, sbyte grade, int value = 0, bool isFailed = false)
	{
		if (isFailed)
		{
			Debate.DebateOperations.Add(new DebateOperation(2, isOwnedByTaiwu, -1, coordinate, Debate.PlayerLeft, Debate.PlayerRight, 0, isFailed: true));
			return;
		}
		int pawnInitialBases = Debate.GetPawnInitialBases(isOwnedByTaiwu, grade, value);
		Pawn pawn = new Pawn(_pawnId, coordinate, isOwnedByTaiwu, pawnInitialBases);
		Debate.DebateGrid[coordinate].PawnId = pawn.Id;
		Debate.DebateOperations.Add(new DebateOperation(2, isOwnedByTaiwu, pawn.Id, coordinate, Debate.PlayerLeft, Debate.PlayerRight, pawn.Bases, isFailed: false));
		Debate.Pawns[_pawnId] = pawn;
		_pawnId++;
		DebateGameTryApplyComment(context, pawn.IsOwnedByTaiwu, 4);
		DebateGameTryApplyNodeEffect(context, coordinate, EDebateNodeEffectRemoveType.Instant);
	}

	private void DebateGameRemovePawn(int id)
	{
		Pawn pawn = Debate.Pawns[id];
		if (Debate.DebateGrid[pawn.Coordinate].PawnId == id)
		{
			Debate.DebateGrid[pawn.Coordinate].PawnId = -1;
		}
		int[] strategies = pawn.Strategies;
		foreach (int num in strategies)
		{
			if (num >= 0)
			{
				DebateGameRemoveStrategy(num, pawn.IsOwnedByTaiwu);
			}
		}
		Debate.Pawns.Remove(id);
	}

	private void DebateGameSetPawnBases(int id, int value)
	{
		Debate.Pawns[id].Bases = Math.Max(0, value);
		Debate.DebateOperations.Add(new DebateOperation(10, id, value, Debate.PlayerLeft, Debate.PlayerRight));
	}

	private void DebateGameSetPawnDead(DataContext context, int id, int cause, bool isTaiwu = false)
	{
		Pawn pawn = Debate.Pawns[id];
		if (pawn.IsHalfImmuneRemove && Debate.TryGetEmptyNode(pawn.IsOwnedByTaiwu, out var coordinates))
		{
			CollectionUtils.Shuffle(context.Random, coordinates);
			DebateGameSetPawnCoordinate(context, id, coordinates[0], isImmuneRemove: true);
			DebateGameSetPawnBases(id, pawn.Bases / 2);
			return;
		}
		if (pawn.IsImmuneRemove)
		{
			IntPair startCoordinate = Debate.GetStartCoordinate(pawn.IsOwnedByTaiwu, pawn.Coordinate.Second);
			if (Debate.DebateGrid[startCoordinate].PawnId < 0 || Debate.DebateGrid[startCoordinate].PawnId == pawn.Id)
			{
				DebateGameSetPawnCoordinate(context, id, startCoordinate, isImmuneRemove: true);
				return;
			}
		}
		if (Debate.DebateGrid[Debate.Pawns[id].Coordinate].PawnId == id)
		{
			Debate.DebateGrid[Debate.Pawns[id].Coordinate].PawnId = -1;
		}
		Debate.Pawns[id].IsAlive = false;
		Debate.DebateOperations.Add(new DebateOperation(3, id, cause, Debate.PlayerLeft, Debate.PlayerRight, isTaiwu));
		if (cause != 3)
		{
			DebateGameApplyPawnStrategyEffectByTriggerType(context, id, EDebateStrategyTriggerType.PawnDead);
		}
		if (1 == 0)
		{
		}
		bool flag = cause switch
		{
			0 => true, 
			2 => pawn.IsOwnedByTaiwu != isTaiwu, 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		if (flag)
		{
			if (pawn.IsOwnedByTaiwu)
			{
				_debateBeatPawnCount.npc++;
			}
			else
			{
				_debateBeatPawnCount.taiwu++;
			}
			DebateGameTryApplyComment(context, !pawn.IsOwnedByTaiwu, 6);
		}
	}

	private void DebateGameSetPawnRevealed(DataContext context, int id)
	{
		Pawn pawn = Debate.Pawns[id];
		if (pawn.IsRevealed)
		{
			return;
		}
		pawn.IsRevealed = true;
		if (pawn.Bases <= 0)
		{
			if (pawn.IsOwnedByTaiwu)
			{
				_debateRevealedPawnCount.taiwu++;
			}
			else
			{
				_debateRevealedPawnCount.npc++;
			}
		}
		DebateGameTryApplyComment(context, pawn.IsOwnedByTaiwu, 2);
	}

	private void DebateGameSetPawnCoordinate(DataContext context, int id, IntPair coordinate, bool isImmuneRemove = false)
	{
		Pawn pawn = Debate.Pawns[id];
		if (Debate.GetCoordinateValid(pawn.Coordinate) && Debate.DebateGrid[pawn.Coordinate].PawnId == id)
		{
			Debate.DebateGrid[pawn.Coordinate].PawnId = -1;
		}
		pawn.Coordinate = coordinate;
		Debate.DebateGrid[coordinate].PawnId = id;
		Debate.DebateOperations.Add(new DebateOperation(1, id, coordinate, Debate.PlayerLeft, Debate.PlayerRight, isImmuneRemove));
		DebateGameTryApplyNodeEffect(context, coordinate, EDebateNodeEffectRemoveType.Trigger);
		if (Debate.GetPawnCanDamage(pawn.IsOwnedByTaiwu, pawn.Coordinate.First))
		{
			DebateGameApplyPawnDamage(context, id);
		}
	}

	private void DebateGameAddNodeEffect(DebateNode node, short templateId, int casterId, bool isHelpTaiwu)
	{
		DebateNodeEffectItem debateNodeEffectItem = DebateNodeEffect.Instance[templateId];
		node.EffectState = new DebateNodeEffectState(_nodeEffectId, templateId, casterId, debateNodeEffectItem.Duration, isHelpTaiwu);
		_debateSpectatorCooldownMap[casterId] = debateNodeEffectItem.Cooldown;
		Debate.NodeEffects[_nodeEffectId] = node.EffectState;
		_nodeEffectId++;
		Debate.DebateOperations.Add(new DebateOperation(15, node.EffectState, node.Coordinate));
	}

	private void DebateGameRemoveNodeEffect(DebateNode node)
	{
		DebateOperation item = new DebateOperation(16, node.EffectState, node.Coordinate);
		Debate.DebateOperations.Add(item);
		node.EffectState = DebateNodeEffectState.Invalid;
	}

	private void DebateGameChangePlayerBases(DebatePlayer player, int delta, bool isTaiwu = false, short nodeEffectTemplateId = -1)
	{
		player.Bases = Math.Clamp(player.Bases + delta, 0, player.MaxBases);
		DebateOperation item = new DebateOperation(21, isTaiwu, Debate.PlayerLeft, Debate.PlayerRight, nodeEffectTemplateId);
		Debate.DebateOperations.Add(item);
	}

	private void DebateGameChangePlayerStrategyPoint(DebatePlayer player, int delta)
	{
		player.StrategyPoint = Math.Clamp(player.StrategyPoint + delta, 0, DebateConstants.MaxStrategyPoint);
	}

	private void DebateGameAddStrategy(int pawnId, short templateId, bool isCastedByTaiwu)
	{
		Tester.Assert(Debate.GetPawnStrategyCount(pawnId) < DebateConstants.PawnStrategyLimit);
		ActivatedStrategy activatedStrategy = new ActivatedStrategy(_activatedStrategyId, pawnId, templateId, isCastedByTaiwu);
		Pawn pawn = Debate.Pawns[pawnId];
		if (Debate.GetNodeIsContainingEffect(pawn.Coordinate, 32))
		{
			activatedStrategy.IsRevealed = true;
		}
		for (int i = 0; i < pawn.Strategies.Length; i++)
		{
			if (pawn.Strategies[i] < 0)
			{
				pawn.Strategies[i] = activatedStrategy.Id;
				Debate.DebateOperations.Add(new DebateOperation(6, activatedStrategy.PawnId, i, Debate.PlayerLeft, Debate.PlayerRight));
				break;
			}
		}
		Debate.ActivatedStrategies.Add(activatedStrategy.Id, activatedStrategy);
		_activatedStrategyId++;
	}

	private void DebateGameRemoveStrategy(int id, bool isCastedByTaiwu)
	{
		ActivatedStrategy activatedStrategy = Debate.ActivatedStrategies[id];
		Pawn pawn = Debate.Pawns[activatedStrategy.PawnId];
		for (int i = 0; i < pawn.Strategies.Length; i++)
		{
			if (pawn.Strategies[i] == activatedStrategy.Id)
			{
				pawn.Strategies[i] = -1;
				Debate.DebateOperations.Add(new DebateOperation(13, activatedStrategy.PawnId, i, Debate.PlayerLeft, Debate.PlayerRight, isCastedByTaiwu));
			}
		}
		Debate.ActivatedStrategies.Remove(id);
	}

	private void DebateGameChangePawnStrategy(int pawnId, int strategyId, int index, bool changeOwner)
	{
		if (strategyId >= 0)
		{
			ActivatedStrategy activatedStrategy = Debate.ActivatedStrategies[strategyId];
			if (changeOwner)
			{
				activatedStrategy.IsCastedByTaiwu = !activatedStrategy.IsCastedByTaiwu;
			}
			activatedStrategy.PawnId = pawnId;
		}
		Debate.Pawns[pawnId].Strategies[index] = strategyId;
	}

	private void DebateGameAddPlayerCanUseCard(bool isTaiwu, int source, int index)
	{
		List<short> strategyCardCollection = Debate.GetStrategyCardCollection(isTaiwu, source);
		short num = strategyCardCollection[index];
		strategyCardCollection.RemoveAt(index);
		int num2 = (isTaiwu ? 2 : 5);
		List<short> strategyCardCollection2 = Debate.GetStrategyCardCollection(isTaiwu, num2);
		strategyCardCollection2.Add(num);
		Debate.DebateOperations.Add(new DebateOperation(8, source, num2, strategyCardCollection2.Count - 1, num, Debate.PlayerLeft, Debate.PlayerRight));
	}

	private void DebateGameRemovePlayerCanUseCard(bool isTaiwu, int destination, int index, int offset = 0, bool playAnim = true)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(isTaiwu);
		List<short> strategyCardCollection = Debate.GetStrategyCardCollection(isTaiwu, destination);
		int value = (isTaiwu ? 2 : 5);
		short num = playerByPlayerIsTaiwu.CanUseCards[index + offset];
		playerByPlayerIsTaiwu.CanUseCards.RemoveAt(index + offset);
		strategyCardCollection.Add(num);
		if (playAnim)
		{
			Debate.DebateOperations.Add(new DebateOperation(8, value, destination, index, num, Debate.PlayerLeft, Debate.PlayerRight));
		}
	}

	[DomainMethod]
	public void GmCmd_GetDebateStrategyCard(int id)
	{
		if (Debate != null && id >= 0 && id < DebateStrategy.Instance.Count && Debate.GetIsTaiwuTurn())
		{
			short num = (short)id;
			Debate.DebateOperations.Clear();
			Debate.PlayerLeft.CanUseCards.Add(num);
			Debate.DebateOperations.Add(new DebateOperation(8, -1, 2, Debate.PlayerLeft.CanUseCards.Count - 1, num, Debate.PlayerLeft, Debate.PlayerRight));
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ChangeLifeSkillCombatData, Debate);
		}
	}

	[DomainMethod]
	public void GmCmd_ChangeStrategyPoint(bool isTaiwu, int delta)
	{
		if (Debate != null)
		{
			Debate.DebateOperations.Clear();
			DebateGameChangePlayerStrategyPoint(Debate.GetPlayerByPlayerIsTaiwu(isTaiwu), delta);
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ChangeLifeSkillCombatData, Debate);
		}
	}

	[DomainMethod]
	public void GmCmd_ChangeBases(bool isTaiwu, int delta)
	{
		if (Debate != null)
		{
			Debate.DebateOperations.Clear();
			DebateGameChangePlayerBases(Debate.GetPlayerByPlayerIsTaiwu(isTaiwu), delta, isTaiwu: false, -1);
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ChangeLifeSkillCombatData, Debate);
		}
	}

	[DomainMethod]
	public void GmCmd_ChangePressure(bool isTaiwu, int delta)
	{
		if (Debate != null)
		{
			Debate.DebateOperations.Clear();
			DebateGameChangePlayerPressure(isTaiwu ? (left: delta, right: 0) : (left: 0, right: delta));
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ChangeLifeSkillCombatData, Debate);
		}
	}

	[DomainMethod]
	public void GmCmd_ChangeGamePoint(bool isTaiwu, int delta)
	{
		if (Debate != null)
		{
			Debate.DebateOperations.Clear();
			DebateGameChangePlayerGamePoint(Debate.GetPlayerByPlayerIsTaiwu(isTaiwu), delta, needPlay: true, isTaiwuCasted: false, -1);
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ChangeLifeSkillCombatData, Debate);
		}
	}

	[DomainMethod]
	public void GmCmd_EmptyAiOwnedCard()
	{
		if (Debate != null)
		{
			Debate.PlayerRight.OwnedCards.Clear();
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ChangeLifeSkillCombatData, Debate);
		}
	}

	[DomainMethod]
	public void GmCmd_AddAiOwnedCard(int id)
	{
		if (Debate != null)
		{
			Debate.PlayerRight.OwnedCards.Add((short)id);
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ChangeLifeSkillCombatData, Debate);
		}
	}

	[DomainMethod]
	public void GmCmd_AddNodeEffect(DataContext context, short templateId, int spectatorId)
	{
		Debate.DebateOperations.Clear();
		if (Debate != null && spectatorId >= 0 && DebateGameTryAddNodeEffect(context, templateId, spectatorId))
		{
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ChangeLifeSkillCombatData, Debate);
		}
	}

	private void DebateGameInitializeStrategyActions()
	{
		_debateStrategyActions = new Dictionary<int, Action<DebateStrategyActionParams>>
		{
			{ 4, DebateGameApplyDamageMore },
			{ 5, DebateGameApplyForwardMore },
			{ 7, DebateGameApplyDrawCardInOwned },
			{ 12, DebateGameApplyChangeOpponentBases },
			{ 13, DebateGameApplyChangeOpponentStrategyPoint },
			{ 16, DebateGameApplyHalfImmuneRemove },
			{ 17, DebateGameApplyImmuneDebuff },
			{ 18, DebateGameApplyImmuneRemove },
			{ 19, DebateGameApplyChangeBases },
			{ 9, DebateGameApplyChangeSelfBases },
			{ 10, DebateGameApplyChangeSelfStrategyPoint },
			{ 20, DebateGameApplyRevealBases },
			{ 15, DebateGameApplyChangeGamePoint },
			{ 21, DebateGameApplyRemovePawn },
			{ 22, DebateGameApplyPawnLink },
			{ 11, DebateGameApplyApplyToSelfWhenDamage },
			{ 25, DebateGameApplyHalfMakeMove },
			{ 27, DebateGameApplyRecycleBases },
			{ 14, DebateGameApplyDamageMoreToBoth },
			{ 29, DebateGameApplyPawnBackward },
			{ 31, DebateGameApplyDrawCardInUsed },
			{ 32, DebateGameApplyRevealStrategy },
			{ 33, DebateGameApplyPawnAvoidConflict },
			{ 34, DebateGameApplyPawnHalt },
			{ 37, DebateGameApplySplitPawn },
			{ 38, DebateGameApplyRemoveStrategy },
			{ 40, DebateGameApplyExchangeCard },
			{ 1, DebateGameApplyMakeMove },
			{ 2, DebateGameApplySwitchCoordinate },
			{ 3, DebateGameApplyTeleportCoordinate },
			{ 30, DebateGameApplyRecycleStrategy },
			{ 36, DebateGameApplyMergePawn },
			{ 39, DebateGameApplyExchangeStrategy },
			{ 42, DebateGameApplyTeleportToBottom },
			{ 43, DebateGameApplyDamageMoreByNode }
		};
	}

	[DomainMethod]
	public DebateGame DebateGameCastStrategy(DataContext context, int index, bool isCastedByTaiwu, List<StrategyTarget> strategyTargets)
	{
		if (isCastedByTaiwu && !Debate.IsTaiwuAi)
		{
			Debate.DebateOperations.Clear();
		}
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(isCastedByTaiwu);
		short num = playerByPlayerIsTaiwu.CanUseCards[index];
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[num];
		int num2 = 0;
		bool flag = DebateGameCheckPressure(context, isCastedByTaiwu, 6);
		Debate.DebateOperations.Add(new DebateOperation(14, isCastedByTaiwu, num, Debate.PlayerLeft, Debate.PlayerRight, strategyTargets, flag));
		int strategyCardLocation = Debate.GetStrategyCardLocation(isCastedByTaiwu, 7);
		DebateGameRemovePlayerCanUseCard(isCastedByTaiwu, strategyCardLocation, index, 0, !flag);
		if (!flag)
		{
			if (!isCastedByTaiwu)
			{
				Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu: false, 17, new int[1] { num }));
			}
			num2 = ((debateStrategyItem.TriggerType != EDebateStrategyTriggerType.Instant) ? (num2 + DebateGameAddPawnStrategy(isCastedByTaiwu, num, strategyTargets)) : (num2 + DebateGameApplyInstantStrategyEffect(context, index, isCastedByTaiwu, num, strategyTargets)));
		}
		DebateGameChangePlayerStrategyPoint(playerByPlayerIsTaiwu, -(debateStrategyItem.UsedCost + num2));
		DebateGameUpdateCanMakeMoveNode();
		if (flag)
		{
			DebateGameApplyPressureEffect(isCastedByTaiwu, 6, playerByPlayerIsTaiwu.Pressure, num);
		}
		_debateCardUsedCount++;
		return Debate;
	}

	private int[] DebateGameGetStrategyRecordParameters(short templateId, List<short> strategyTemplateIds, List<int> pawnIds, List<int> values)
	{
		DebateRecordItem debateRecordItem = DebateRecord.Instance[templateId];
		int[] array = new int[debateRecordItem.Parameters.Length];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < debateRecordItem.Parameters.Length; i++)
		{
			int[] array2 = array;
			int num4 = i;
			EDebateRecordParamType eDebateRecordParamType = debateRecordItem.Parameters[i];
			if (1 == 0)
			{
			}
			int num5 = eDebateRecordParamType switch
			{
				EDebateRecordParamType.IntValue => Math.Abs(values[num3++]), 
				EDebateRecordParamType.Strategy => strategyTemplateIds[num2++], 
				EDebateRecordParamType.Pawn => pawnIds[num++], 
				EDebateRecordParamType.PawnCount => pawnIds.Count, 
				_ => 0, 
			};
			if (1 == 0)
			{
			}
			array2[num4] = num5;
		}
		return array;
	}

	private void DebateGameAddStrategyRecord(bool isTaiwu, short recordTemplateId, List<short> strategyTemplateIds, List<int> pawnIds, List<int> values)
	{
		Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu, recordTemplateId, DebateGameGetStrategyRecordParameters(recordTemplateId, strategyTemplateIds, pawnIds, values)));
	}

	private bool DebateGameCheckDebateRecord(bool isCastedByTaiwu, short templateId, out bool isPawnCount)
	{
		isPawnCount = false;
		if (!isCastedByTaiwu || templateId < 0)
		{
			return false;
		}
		DebateRecordItem debateRecordItem = DebateRecord.Instance[templateId];
		EDebateRecordParamType[] parameters = debateRecordItem.Parameters;
		foreach (EDebateRecordParamType eDebateRecordParamType in parameters)
		{
			if (eDebateRecordParamType == EDebateRecordParamType.PawnCount)
			{
				isPawnCount = true;
				break;
			}
		}
		return true;
	}

	private void DebateGameDecodeStrategyEffectTargets(List<StrategyTarget> strategyTargets, out List<int> pawnIds, out List<int> cards, out List<IntPair> coordinates, out List<sbyte> pawnGrades)
	{
		pawnIds = null;
		cards = null;
		coordinates = null;
		pawnGrades = null;
		if (strategyTargets == null)
		{
			return;
		}
		foreach (StrategyTarget strategyTarget in strategyTargets)
		{
			switch (strategyTarget.Type)
			{
			case EDebateStrategyTargetObjectType.Pawn:
				if (pawnIds == null)
				{
					pawnIds = new List<int>();
				}
				foreach (ulong item2 in strategyTarget.List)
				{
					pawnIds.Add((int)item2);
				}
				break;
			case EDebateStrategyTargetObjectType.Node:
				if (coordinates == null)
				{
					coordinates = new List<IntPair>();
				}
				foreach (ulong item3 in strategyTarget.List)
				{
					coordinates.Add((IntPair)item3);
				}
				break;
			case EDebateStrategyTargetObjectType.PawnGrade:
				if (pawnGrades == null)
				{
					pawnGrades = new List<sbyte>();
				}
				foreach (ulong item4 in strategyTarget.List)
				{
					pawnGrades.Add((sbyte)item4);
				}
				break;
			case EDebateStrategyTargetObjectType.StrategyCard:
				if (cards == null)
				{
					cards = new List<int>();
				}
				foreach (ulong item5 in strategyTarget.List)
				{
					int item = (int)item5;
					cards.Add(item);
				}
				break;
			}
		}
	}

	private void DebateGameApplyNoTargetInstantStrategyEffect(DataContext context, bool isCastedByTaiwu, short templateId)
	{
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[templateId];
		foreach (IntPair effect in debateStrategyItem.EffectList)
		{
			if (!_debateStrategyActions.TryGetValue(effect.First, out var value))
			{
				throw new Exception("Instant Debate Strategy has no effect: " + debateStrategyItem.Name);
			}
			DebateStrategyActionParams obj = new DebateStrategyActionParams
			{
				Context = context,
				IsCastedByTaiwu = isCastedByTaiwu,
				StrategyTemplateId = templateId,
				Value = effect.Second
			};
			value(obj);
		}
	}

	private void DebateGameApplySingleTargetInstantStrategyEffect(DataContext context, int index, bool isCastedByTaiwu, short templateId, List<StrategyTarget> strategyTargets)
	{
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[templateId];
		DebateGameDecodeStrategyEffectTargets(strategyTargets, out var pawnIds, out var cards, out var _, out var _);
		Tester.Assert(pawnIds != null || cards != null);
		Tester.Assert(pawnIds == null || cards == null);
		if (pawnIds != null && DebateGameCheckDebateRecord(isCastedByTaiwu, debateStrategyItem.DebateRecord, out var isPawnCount))
		{
			if (isPawnCount)
			{
				List<int> list = null;
				List<int> list2 = null;
				foreach (int item in pawnIds)
				{
					if (Debate.Pawns[item].IsOwnedByTaiwu)
					{
						if (list == null)
						{
							list = new List<int>();
						}
						list.Add(item);
					}
					else
					{
						if (list2 == null)
						{
							list2 = new List<int>();
						}
						list2.Add(item);
					}
				}
				if (list != null)
				{
					DebateGameAddStrategyRecord(isTaiwu: true, debateStrategyItem.DebateRecord, new List<short> { debateStrategyItem.TemplateId }, list, new List<int> { debateStrategyItem.EffectList[0].Second });
				}
				if (list2 != null)
				{
					DebateGameAddStrategyRecord(isTaiwu: true, debateStrategyItem.DebateRecord, new List<short> { debateStrategyItem.TemplateId }, list2, new List<int> { debateStrategyItem.EffectList[0].Second });
				}
			}
			else if (debateStrategyItem.DebateRecord == 18)
			{
				Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu: true, 18, new int[1] { templateId }));
			}
			else
			{
				foreach (int item2 in pawnIds)
				{
					DebateGameAddStrategyRecord(isTaiwu: true, debateStrategyItem.DebateRecord, new List<short> { debateStrategyItem.TemplateId }, new List<int> { item2 }, new List<int> { debateStrategyItem.EffectList[0].Second });
				}
			}
		}
		foreach (IntPair effect in debateStrategyItem.EffectList)
		{
			if (!_debateStrategyActions.TryGetValue(effect.First, out var value))
			{
				throw new Exception("Instant Debate Strategy has no effect: " + debateStrategyItem.Name);
			}
			DebateStrategyActionParams debateStrategyActionParams = new DebateStrategyActionParams
			{
				Context = context,
				IsCastedByTaiwu = isCastedByTaiwu,
				UsingCard = index,
				StrategyTemplateId = templateId,
				Value = effect.Second
			};
			if (pawnIds != null)
			{
				foreach (int item3 in pawnIds)
				{
					debateStrategyActionParams.PawnId = item3;
					value(debateStrategyActionParams);
				}
			}
			else
			{
				if (cards == null)
				{
					continue;
				}
				cards.Sort();
				cards.Reverse();
				foreach (int item4 in cards)
				{
					debateStrategyActionParams.Card = item4;
					value(debateStrategyActionParams);
				}
			}
		}
	}

	private void DebateGameApplyMultipleTargetInstantStrategyEffect(DataContext context, bool isCastedByTaiwu, short templateId, List<StrategyTarget> strategyTargets)
	{
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[templateId];
		Tester.Assert(debateStrategyItem.EffectList.Count == 1);
		IntPair intPair = debateStrategyItem.EffectList[0];
		if (!_debateStrategyActions.TryGetValue(intPair.First, out var value))
		{
			throw new Exception("Instant Debate Strategy has no effect: " + debateStrategyItem.Name);
		}
		DebateGameDecodeStrategyEffectTargets(strategyTargets, out var pawnIds, out var _, out var coordinates, out var pawnGrades);
		DebateStrategyActionParams debateStrategyActionParams = new DebateStrategyActionParams
		{
			Context = context,
			IsCastedByTaiwu = isCastedByTaiwu,
			StrategyTemplateId = templateId,
			Value = intPair.Second
		};
		switch (intPair.First)
		{
		case 1:
			debateStrategyActionParams.Grade = pawnGrades[0];
			debateStrategyActionParams.Coordinate = coordinates[0];
			value(debateStrategyActionParams);
			pawnIds = new List<int> { Debate.DebateGrid[coordinates[0]].PawnId };
			break;
		case 2:
		case 36:
		case 39:
			debateStrategyActionParams.PawnId = pawnIds[0];
			debateStrategyActionParams.PawnId2 = pawnIds[1];
			value(debateStrategyActionParams);
			break;
		case 3:
			debateStrategyActionParams.PawnId = pawnIds[0];
			debateStrategyActionParams.Coordinate = coordinates[0];
			value(debateStrategyActionParams);
			break;
		}
		if (isCastedByTaiwu && debateStrategyItem.DebateRecord >= 0)
		{
			DebateGameAddStrategyRecord(isTaiwu: true, debateStrategyItem.DebateRecord, new List<short> { debateStrategyItem.TemplateId }, pawnIds, new List<int> { debateStrategyItem.EffectList[0].Second });
		}
	}

	public int DebateGameApplyInstantStrategyEffect(DataContext context, int index, bool isCastedByTaiwu, short templateId, List<StrategyTarget> strategyTargets)
	{
		int num = 0;
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[templateId];
		if (debateStrategyItem.EffectList == null || debateStrategyItem.EffectList.Count == 0)
		{
			return num;
		}
		if (debateStrategyItem.TargetList == null || debateStrategyItem.TargetList.Count == 0)
		{
			DebateGameApplyNoTargetInstantStrategyEffect(context, isCastedByTaiwu, templateId);
		}
		else
		{
			if (debateStrategyItem.TargetList.Count == 1)
			{
				DebateGameApplySingleTargetInstantStrategyEffect(context, index, isCastedByTaiwu, templateId, strategyTargets);
			}
			else
			{
				DebateGameApplyMultipleTargetInstantStrategyEffect(context, isCastedByTaiwu, templateId, strategyTargets);
			}
			foreach (StrategyTarget strategyTarget in strategyTargets)
			{
				if (strategyTarget.Type != EDebateStrategyTargetObjectType.Pawn)
				{
					continue;
				}
				foreach (ulong item in strategyTarget.List)
				{
					int num2 = (int)item;
					List<DebateGame.EffectItem> list = new List<DebateGame.EffectItem>();
					if (Debate.TryGetPawnStrategyEffectValue(num2, 28, out var value, list))
					{
						num += value;
						DebateGameAddStrategyRecord(isCastedByTaiwu, 54, new List<short>
						{
							list[0].StrategyTemplateId,
							templateId
						}, new List<int> { num2 }, new List<int> { value });
					}
				}
			}
		}
		return num;
	}

	private int DebateGameAddPawnStrategy(bool isCastedByTaiwu, short templateId, List<StrategyTarget> strategyTargets)
	{
		int num = 0;
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[templateId];
		foreach (StrategyTarget strategyTarget in strategyTargets)
		{
			Tester.Assert(strategyTarget.Type == EDebateStrategyTargetObjectType.Pawn);
			Tester.Assert(strategyTarget.List.Count != 0);
			foreach (ulong item in strategyTarget.List)
			{
				int num2 = (int)item;
				List<DebateGame.EffectItem> list = new List<DebateGame.EffectItem>();
				if (Debate.TryGetPawnStrategyEffectValue(num2, 28, out var value, list))
				{
					num += value;
					DebateGameAddStrategyRecord(isCastedByTaiwu, 54, new List<short>
					{
						list[0].StrategyTemplateId,
						templateId
					}, new List<int> { num2 }, new List<int> { value });
				}
			}
		}
		foreach (StrategyTarget strategyTarget2 in strategyTargets)
		{
			foreach (ulong item2 in strategyTarget2.List)
			{
				int num3 = (int)item2;
				if (isCastedByTaiwu && debateStrategyItem.TriggerType == EDebateStrategyTriggerType.Invalid && debateStrategyItem.DebateRecord >= 0)
				{
					DebateGameAddStrategyRecord(isTaiwu: true, debateStrategyItem.DebateRecord, new List<short> { debateStrategyItem.TemplateId }, new List<int> { num3 }, new List<int> { debateStrategyItem.EffectList[0].Second });
				}
				DebateGameAddStrategy(num3, templateId, isCastedByTaiwu);
			}
		}
		if (isCastedByTaiwu && debateStrategyItem.TriggerType != EDebateStrategyTriggerType.Invalid)
		{
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu: true, 18, new int[1] { templateId }));
		}
		return num;
	}

	private void DebateGameApplyPawnStrategyEffectByTriggerType(DataContext context, int pawnId, EDebateStrategyTriggerType type)
	{
		Pawn pawn = Debate.Pawns[pawnId];
		pawn.ResetStrategyValue();
		int[] strategies = pawn.Strategies;
		foreach (int num in strategies)
		{
			if (num >= 0 && Debate.ActivatedStrategies.TryGetValue(num, out var value) && value.GetTriggerType() == type)
			{
				DebateGameApplyTriggeredStrategyEffect(context, value);
			}
		}
	}

	private void DebateGameApplyTriggeredStrategyEffect(DataContext context, ActivatedStrategy strategy)
	{
		DebateStrategyItem config = strategy.GetConfig();
		int index = -1;
		Pawn pawn = Debate.Pawns[strategy.PawnId];
		int[] strategies = pawn.Strategies;
		for (int i = 0; i < strategies.Length; i++)
		{
			if (strategies[i] == index)
			{
				index = i;
				break;
			}
		}
		Debate.DebateOperations.Add(new DebateOperation(7, strategy.PawnId, strategy.Id, Debate.PlayerLeft, Debate.PlayerRight));
		if (config.EffectList != null && config.EffectList.Count != 0)
		{
			foreach (IntPair effect in config.EffectList)
			{
				if (_debateStrategyActions.TryGetValue(effect.First, out var value))
				{
					DebateStrategyActionParams obj = new DebateStrategyActionParams
					{
						Context = context,
						IsCastedByTaiwu = strategy.IsCastedByTaiwu,
						StrategyTemplateId = strategy.TemplateId,
						PawnId = strategy.PawnId,
						Value = effect.Second
					};
					value(obj);
				}
			}
			if (config.DebateRecord >= 0)
			{
				DebateGameAddStrategyRecord(pawn.IsOwnedByTaiwu, config.DebateRecord, new List<short> { config.TemplateId }, new List<int> { strategy.PawnId }, new List<int> { config.EffectList[0].Second });
			}
		}
		if (config.IsOneTime)
		{
			_oneTimeStrategyActionList.Add(Action);
		}
		void Action()
		{
			Debate.DebateOperations.Add(new DebateOperation(12, strategy.PawnId, index, Debate.PlayerLeft, Debate.PlayerRight));
			DebateGameRemoveStrategy(strategy.Id, pawn.IsOwnedByTaiwu);
		}
	}

	private void DebateGameApplyDamageMore(DebateStrategyActionParams param)
	{
		Debate.Pawns[param.PawnId].DamageList.Add(new PawnDamageInfo(param.Value, param.IsCastedByTaiwu, isToSelf: false));
	}

	private void DebateGameApplyForwardMore(DebateStrategyActionParams param)
	{
		IntPair pawnTargetPosition = Debate.GetPawnTargetPosition(param.PawnId);
		if (Debate.GetNodeCanTeleportPawn(pawnTargetPosition))
		{
			DebateGameSetPawnCoordinate(param.Context, param.PawnId, pawnTargetPosition);
		}
	}

	private void DebateGameApplyDrawCardInOwned(DebateStrategyActionParams param)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < param.Value; i++)
		{
			bool isTaiwu = !param.IsCastedByTaiwu;
			List<int> collection;
			int num3 = Debate.TryGetStrategyCardIndexCollection(ref isTaiwu, 6, out collection);
			if (num3 == -1)
			{
				continue;
			}
			DebateGameAddPlayerCanUseCard(param.IsCastedByTaiwu, num3, collection[(collection.Count != 1) ? param.Context.Random.Next(collection.Count) : 0]);
			if (param.IsCastedByTaiwu)
			{
				if (isTaiwu)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		if (num > 0)
		{
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu: true, DebateStrategy.Instance[param.StrategyTemplateId].DebateRecord, new int[3] { param.StrategyTemplateId, 0, num }));
		}
		if (num2 > 0)
		{
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu: true, DebateStrategy.Instance[param.StrategyTemplateId].DebateRecord, new int[3] { param.StrategyTemplateId, 1, num2 }));
		}
	}

	private void DebateGameApplyChangeOpponentBases(DebateStrategyActionParams param)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(!param.IsCastedByTaiwu);
		DebateGameChangePlayerBases(playerByPlayerIsTaiwu, playerByPlayerIsTaiwu.MaxBases * param.Value / 100, isTaiwu: false, -1);
	}

	private void DebateGameApplyChangeOpponentStrategyPoint(DebateStrategyActionParams param)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(!param.IsCastedByTaiwu);
		DebateGameChangePlayerStrategyPoint(playerByPlayerIsTaiwu, param.Value);
	}

	private void DebateGameApplyHalfImmuneRemove(DebateStrategyActionParams param)
	{
		Debate.Pawns[param.PawnId].IsHalfImmuneRemove = true;
	}

	private void DebateGameApplyImmuneDebuff(DebateStrategyActionParams param)
	{
		Debate.Pawns[param.PawnId].IsImmuneDebuff = true;
	}

	private void DebateGameApplyImmuneRemove(DebateStrategyActionParams param)
	{
		Debate.Pawns[param.PawnId].IsImmuneRemove = true;
	}

	private void DebateGameApplyChangeBases(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		DebateGameSetPawnBases(param.PawnId, pawn.Bases * (100 + param.Value) / 100);
	}

	private void DebateGameApplyChangeSelfBases(DebateStrategyActionParams param)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(param.IsCastedByTaiwu);
		DebateGameChangePlayerBases(playerByPlayerIsTaiwu, playerByPlayerIsTaiwu.MaxBases * param.Value / 100, param.IsCastedByTaiwu, (short)param.NodeEffect.TemplateId);
	}

	private void DebateGameApplyChangeSelfStrategyPoint(DebateStrategyActionParams param)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(param.IsCastedByTaiwu);
		DebateGameChangePlayerStrategyPoint(playerByPlayerIsTaiwu, param.Value);
	}

	private void DebateGameApplyRevealBases(DebateStrategyActionParams param)
	{
		DebateGameSetPawnRevealed(param.Context, param.PawnId);
	}

	private void DebateGameApplyChangeGamePoint(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(pawn.IsOwnedByTaiwu);
		DebateGameChangePlayerGamePoint(playerByPlayerIsTaiwu, param.Value, needPlay: true, pawn.IsOwnedByTaiwu, (short)(param.NodeEffect?.TemplateId ?? (-1)));
		DebateGameAddStrategyRecord(pawn.IsOwnedByTaiwu, 45, new List<short> { param.StrategyTemplateId }, null, new List<int> { param.Value });
	}

	private void DebateGameApplyRemovePawn(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		if (pawn.IsAlive)
		{
			DebateGameSetPawnDead(param.Context, param.PawnId, 2, param.IsCastedByTaiwu);
		}
	}

	private void DebateGameApplyPawnLink(DebateStrategyActionParams param)
	{
		foreach (KeyValuePair<int, Pawn> pawn2 in Debate.Pawns)
		{
			pawn2.Deconstruct(out var key, out var value);
			int num = key;
			Pawn pawn = value;
			if (num != param.PawnId && pawn.IsAlive && Debate.TryGetPawnStrategyEffectValue(num, 22, out key))
			{
				DebateGameSetPawnDead(param.Context, num, 3, param.IsCastedByTaiwu);
			}
		}
	}

	private void DebateGameApplyApplyToSelfWhenDamage(DebateStrategyActionParams param)
	{
		Debate.Pawns[param.PawnId].ChangeSelfGamePointByDamagePercent = 100 * param.Value;
		Debate.Pawns[param.PawnId].ChangeSelfGamePointByDamagePercentIsCastedByTaiwu = param.IsCastedByTaiwu;
	}

	private void DebateGameApplyHalfMakeMove(DebateStrategyActionParams param)
	{
		IntPair pawnBehindPosition = Debate.GetPawnBehindPosition(param.PawnId);
		int value = Debate.GetPawnBases(param.PawnId) / 2;
		if (Debate.GetNodeCanTeleportPawn(pawnBehindPosition))
		{
			DebateGameAddPawn(param.Context, pawnBehindPosition, param.IsCastedByTaiwu, 0, value);
		}
	}

	private void DebateGameApplyRecycleBases(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		int pawnBases = Debate.GetPawnBases(param.PawnId);
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(pawn.IsOwnedByTaiwu);
		if (pawn.IsAlive)
		{
			DebateGameSetPawnDead(param.Context, param.PawnId, 2, param.IsCastedByTaiwu);
			DebateGameChangePlayerBases(playerByPlayerIsTaiwu, pawnBases, isTaiwu: false, -1);
			DebateGameAddStrategyRecord(pawn.IsOwnedByTaiwu, 53, new List<short> { param.StrategyTemplateId }, null, new List<int> { pawnBases });
		}
	}

	private void DebateGameApplyDamageMoreToBoth(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		pawn.DamageList.Add(new PawnDamageInfo(param.Value, param.IsCastedByTaiwu, isToSelf: true, isStrategyDamage: true));
		pawn.DamageList.Add(new PawnDamageInfo(param.Value, param.IsCastedByTaiwu, isToSelf: false, isStrategyDamage: true));
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[param.StrategyTemplateId];
		DebateGameAddStrategyRecord(!pawn.IsOwnedByTaiwu, debateStrategyItem.DebateRecord, new List<short> { debateStrategyItem.TemplateId }, new List<int> { pawn.Id }, new List<int> { debateStrategyItem.EffectList[0].Second });
	}

	private void DebateGameApplyPawnBackward(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		if (!pawn.IsAlive)
		{
			return;
		}
		for (int num = param.Value; num > 0; num--)
		{
			IntPair pawnBehindPosition = Debate.GetPawnBehindPosition(param.PawnId, num);
			if (Debate.GetNodeCanTeleportPawn(pawnBehindPosition))
			{
				DebateGameSetPawnCoordinate(param.Context, param.PawnId, pawnBehindPosition);
				break;
			}
		}
	}

	private void DebateGameApplyDrawCardInUsed(DebateStrategyActionParams param)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < param.Value; i++)
		{
			bool isTaiwu = param.Context.Random.NextBool();
			List<int> collection;
			int num3 = Debate.TryGetStrategyCardIndexCollection(ref isTaiwu, 7, out collection);
			if (num3 == -1)
			{
				continue;
			}
			DebateGameAddPlayerCanUseCard(param.IsCastedByTaiwu, num3, collection[(collection.Count != 1) ? param.Context.Random.Next(collection.Count) : 0]);
			if (param.IsCastedByTaiwu)
			{
				if (isTaiwu)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		if (num > 0)
		{
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu: true, DebateStrategy.Instance[param.StrategyTemplateId].DebateRecord, new int[3] { param.StrategyTemplateId, 0, num }));
		}
		if (num2 > 0)
		{
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu: true, DebateStrategy.Instance[param.StrategyTemplateId].DebateRecord, new int[3] { param.StrategyTemplateId, 1, num2 }));
		}
	}

	private void DebateGameApplyRevealStrategy(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		int[] strategies = pawn.Strategies;
		foreach (int num in strategies)
		{
			if (num >= 0)
			{
				Debate.ActivatedStrategies[num].IsRevealed = true;
			}
		}
	}

	private void DebateGameApplyPawnAvoidConflict(DebateStrategyActionParams param)
	{
		Debate.Pawns[param.PawnId].IsSwitchLocation = true;
	}

	private void DebateGameApplyPawnHalt(DebateStrategyActionParams param)
	{
		Debate.Pawns[param.PawnId].IsHalt = true;
	}

	private void DebateGameApplySplitPawn(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		int pawnBases = Debate.GetPawnBases(param.PawnId);
		if (!pawn.IsAlive)
		{
			return;
		}
		List<IntPair> list = new List<IntPair>();
		if (!Debate.TryGetEmptyNode(pawn.IsOwnedByTaiwu, list) && Debate.DebateGrid[pawn.Coordinate].IsVantage != pawn.IsOwnedByTaiwu)
		{
			return;
		}
		DebateGameSetPawnDead(param.Context, param.PawnId, 2, param.IsCastedByTaiwu);
		Debate.TryGetEmptyNode(pawn.IsOwnedByTaiwu, list);
		int num = Math.Min(list.Count, param.Value);
		pawnBases /= num;
		for (int i = 0; i < num; i++)
		{
			if (!Debate.TryGetEmptyNode(pawn.IsOwnedByTaiwu, list))
			{
				break;
			}
			CollectionUtils.Shuffle(param.Context.Random, list);
			DebateGameAddPawn(param.Context, list[0], pawn.IsOwnedByTaiwu, 0, pawnBases);
		}
	}

	private void DebateGameApplyRemoveStrategy(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		int[] strategies = pawn.Strategies;
		foreach (int num in strategies)
		{
			if (num >= 0 && !Debate.ActivatedStrategies[num].GetIsInertia())
			{
				DebateGameRemoveStrategy(num, param.IsCastedByTaiwu);
			}
		}
	}

	private void DebateGameApplyExchangeCard(DebateStrategyActionParams param)
	{
		List<short> canUseCards = Debate.PlayerLeft.CanUseCards;
		List<short> canUseCards2 = Debate.PlayerRight.CanUseCards;
		Debate.PlayerLeft.CanUseCards = canUseCards2;
		Debate.PlayerRight.CanUseCards = canUseCards;
		for (int i = 0; i < canUseCards.Count; i++)
		{
			Debate.DebateOperations.Add(new DebateOperation(8, 2, 5, i, canUseCards[i], Debate.PlayerLeft, Debate.PlayerRight));
		}
		for (int j = 0; j < canUseCards2.Count; j++)
		{
			Debate.DebateOperations.Add(new DebateOperation(8, 5, 2, j, canUseCards2[j], Debate.PlayerLeft, Debate.PlayerRight));
		}
		if (param.IsCastedByTaiwu)
		{
			Debate.DebateOperations.Add(new DebateOperation(20, isTaiwu: true, DebateStrategy.Instance[param.StrategyTemplateId].DebateRecord, new int[1] { param.StrategyTemplateId }));
		}
	}

	private void DebateGameApplyMakeMove(DebateStrategyActionParams param)
	{
		DebateGameMakeMove(param.Context, param.Coordinate, param.IsCastedByTaiwu, param.Grade, countAsMakeMove: false);
	}

	private void DebateGameApplySwitchCoordinate(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		Pawn pawn2 = Debate.Pawns[param.PawnId2];
		IntPair coordinate = pawn.Coordinate;
		IntPair coordinate2 = pawn2.Coordinate;
		DebateGameSetPawnCoordinate(param.Context, param.PawnId, coordinate2);
		DebateGameSetPawnCoordinate(param.Context, param.PawnId2, coordinate);
	}

	private void DebateGameApplyTeleportCoordinate(DebateStrategyActionParams param)
	{
		DebateGameSetPawnCoordinate(param.Context, param.PawnId, param.Coordinate);
	}

	private void DebateGameApplyRecycleStrategy(DebateStrategyActionParams param)
	{
		DebatePlayer playerByPlayerIsTaiwu = Debate.GetPlayerByPlayerIsTaiwu(param.IsCastedByTaiwu);
		int num = ((param.Card > param.UsingCard) ? (-1) : 0);
		short num2 = playerByPlayerIsTaiwu.CanUseCards[param.Card + num];
		sbyte usedCost = DebateStrategy.Instance[num2].UsedCost;
		DebateGameChangePlayerStrategyPoint(playerByPlayerIsTaiwu, usedCost);
		DebateGameRemovePlayerCanUseCard(param.IsCastedByTaiwu, Debate.GetStrategyCardLocation(param.IsCastedByTaiwu, 7), param.Card, num);
		if (param.IsCastedByTaiwu)
		{
			DebateGameAddStrategyRecord(isTaiwu: true, DebateStrategy.Instance[param.StrategyTemplateId].DebateRecord, new List<short> { param.StrategyTemplateId, num2 }, null, new List<int> { usedCost });
		}
	}

	private void DebateGameApplyMergePawn(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		Pawn pawn2 = Debate.Pawns[param.PawnId2];
		if (pawn2.IsAlive)
		{
			DebateGameSetPawnBases(param.PawnId, pawn.Bases + Debate.GetPawnBases(param.PawnId2));
			DebateGameSetPawnDead(param.Context, param.PawnId2, 2, param.IsCastedByTaiwu);
		}
	}

	private void DebateGameApplyExchangeStrategy(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		Pawn pawn2 = Debate.Pawns[param.PawnId2];
		bool changeOwner = pawn.IsOwnedByTaiwu != pawn2.IsOwnedByTaiwu;
		for (int i = 0; i < DebateConstants.PawnStrategyLimit; i++)
		{
			int num = pawn.Strategies[i];
			int num2 = pawn2.Strategies[i];
			if ((num < 0 || !Debate.ActivatedStrategies[num].GetIsInertia()) && (num2 < 0 || !Debate.ActivatedStrategies[num2].GetIsInertia()))
			{
				if (num >= 0)
				{
					Debate.DebateOperations.Add(new DebateOperation(13, pawn.Id, i, Debate.PlayerLeft, Debate.PlayerRight, param.IsCastedByTaiwu));
				}
				if (num2 >= 0)
				{
					Debate.DebateOperations.Add(new DebateOperation(13, pawn2.Id, i, Debate.PlayerLeft, Debate.PlayerRight, param.IsCastedByTaiwu));
				}
			}
		}
		for (int j = 0; j < DebateConstants.PawnStrategyLimit; j++)
		{
			int num3 = pawn.Strategies[j];
			int num4 = pawn2.Strategies[j];
			if ((num3 < 0 || !Debate.ActivatedStrategies[num3].GetIsInertia()) && (num4 < 0 || !Debate.ActivatedStrategies[num4].GetIsInertia()))
			{
				if (num4 >= 0)
				{
					Debate.DebateOperations.Add(new DebateOperation(6, pawn.Id, j, Debate.PlayerLeft, Debate.PlayerRight));
				}
				if (num3 >= 0)
				{
					Debate.DebateOperations.Add(new DebateOperation(6, pawn2.Id, j, Debate.PlayerLeft, Debate.PlayerRight));
				}
			}
		}
		for (int k = 0; k < DebateConstants.PawnStrategyLimit; k++)
		{
			int num5 = pawn.Strategies[k];
			int num6 = pawn2.Strategies[k];
			if ((num5 < 0 || !Debate.ActivatedStrategies[num5].GetIsInertia()) && (num6 < 0 || !Debate.ActivatedStrategies[num6].GetIsInertia()))
			{
				DebateGameChangePawnStrategy(param.PawnId, num6, k, changeOwner);
				DebateGameChangePawnStrategy(param.PawnId2, num5, k, changeOwner);
			}
		}
	}

	private void DebateGameApplyTeleportToBottom(DebateStrategyActionParams param)
	{
		Pawn pawn = Debate.Pawns[param.PawnId];
		if (!pawn.IsAlive || !Debate.GetCoordinateValid(pawn.Coordinate))
		{
			return;
		}
		int casterId = param.NodeEffect.CasterId;
		bool flag = DebateGameGetSpectatorHelpTaiwu(param.Context, casterId, pawn.IsOwnedByTaiwu);
		int num = ((!flag) ? 1 : (-1));
		int first = Debate.GetStartCoordinate(!flag) + num;
		int num2 = -1;
		for (int i = 0; i < DebateConstants.DebateLineCount; i++)
		{
			if (Debate.DebateGrid[new IntPair(first, i)].PawnId < 0 && (num2 < 0 || !param.Context.Random.NextBool()))
			{
				num2 = i;
			}
		}
		IntPair coordinate = new IntPair(first, num2);
		if (Debate.GetCoordinateValid(coordinate))
		{
			DebateGameSetPawnCoordinate(param.Context, pawn.Id, coordinate);
		}
	}

	private void DebateGameApplyDamageMoreByNode(DebateStrategyActionParams param)
	{
		Debate.Pawns[param.PawnId].NodeDamage = param.Value;
	}

	[DomainMethod]
	public List<ItemDisplayData> GetAllWarehouseItems(DataContext context)
	{
		return CharacterDomain.GetItemDisplayData(GetTaiwuCharId(), _warehouseItems, ItemSourceType.Warehouse);
	}

	[DomainMethod]
	public List<ItemDisplayData> GetAllWarehouseItemsExcludeValueZero(DataContext context)
	{
		Dictionary<ItemKey, int> dictionary = ObjectPool<Dictionary<ItemKey, int>>.Instance.Get();
		dictionary.Clear();
		foreach (KeyValuePair<ItemKey, int> warehouseItem in _warehouseItems)
		{
			if (DomainManager.Item.GetBaseItem(warehouseItem.Key).GetValue() > 0)
			{
				dictionary.Add(warehouseItem.Key, warehouseItem.Value);
			}
		}
		List<ItemDisplayData> itemDisplayData = CharacterDomain.GetItemDisplayData(GetTaiwuCharId(), dictionary, ItemSourceType.Warehouse);
		ObjectPool<Dictionary<ItemKey, int>>.Instance.Return(dictionary);
		return itemDisplayData;
	}

	[DomainMethod]
	public List<ItemDisplayData> GetTaiwuAllItems(DataContext context)
	{
		List<ItemDisplayData> allWarehouseItemsExcludeValueZero = GetAllWarehouseItemsExcludeValueZero(context);
		allWarehouseItemsExcludeValueZero.AddRange(DomainManager.Character.GetAllInventoryItemsExcludeValueZero(_taiwuCharId));
		return allWarehouseItemsExcludeValueZero;
	}

	[DomainMethod]
	public List<ItemDisplayData> GetWarehouseItemsBySubType(DataContext context, short itemSubType)
	{
		Dictionary<ItemKey, int> items = _warehouseItems.Where((KeyValuePair<ItemKey, int> pair) => ItemTemplateHelper.GetItemSubType(pair.Key.ItemType, pair.Key.TemplateId) == itemSubType).ToDictionary((KeyValuePair<ItemKey, int> pair) => pair.Key, (KeyValuePair<ItemKey, int> pair) => pair.Value);
		return CharacterDomain.GetItemDisplayData(GetTaiwuCharId(), items, ItemSourceType.Warehouse);
	}

	[DomainMethod]
	public List<ItemDisplayData> GetAllTroughItems(DataContext context)
	{
		return CharacterDomain.GetItemDisplayData(GetTaiwuCharId(), DomainManager.Extra.TroughItems, ItemSourceType.Trough);
	}

	[DomainMethod]
	public List<ItemDisplayData> GetAllTreasuryItems(DataContext context)
	{
		Dictionary<ItemKey, int> items = GetTaiwuTreasury().Inventory.Items;
		return CharacterDomain.GetItemDisplayData(_taiwuCharId, items, ItemSourceType.Treasury);
	}

	private List<ItemDisplayData> GetResourcesItems(ResourceInts resources, ItemSourceType itemSourceType)
	{
		List<ItemDisplayData> list = new List<ItemDisplayData>();
		for (int i = 0; i < 8; i++)
		{
			int num = resources[i];
			if (num > 0)
			{
				short templateId = Convert.ToInt16(321 + i);
				ItemKey key = new ItemKey(12, 0, templateId, 0);
				ItemDisplayData item = new ItemDisplayData
				{
					Key = key,
					Amount = num,
					ItemSourceType = (sbyte)itemSourceType
				};
				list.Add(item);
			}
		}
		return list;
	}

	[DomainMethod]
	public (ItemSourceType itemSourceType, List<ItemDisplayData> list) GetAllItems(ItemSourceType itemSourceType, bool includeResources = false)
	{
		if (itemSourceType == ItemSourceType.Resources)
		{
			List<ItemDisplayData> resourcesItems = GetResourcesItems(GetTaiwu().GetResources(), ItemSourceType.Resources);
			return (itemSourceType: itemSourceType, list: resourcesItems);
		}
		List<ItemDisplayData> itemDisplayData = CharacterDomain.GetItemDisplayData(GetTaiwuCharId(), GetItems(itemSourceType), itemSourceType);
		if (includeResources)
		{
			ResourceInts taiwuVillageResources = GetTaiwuVillageResources(itemSourceType);
			List<ItemDisplayData> resourcesItems2 = GetResourcesItems(taiwuVillageResources, itemSourceType);
			itemDisplayData.AddRange(resourcesItems2);
		}
		return (itemSourceType: itemSourceType, list: itemDisplayData);
	}

	[DomainMethod]
	public (ItemSourceType itemSourceType, ResourceInts resource) GetAllResources(ItemSourceType itemSourceType)
	{
		ResourceInts obj;
		if (itemSourceType != ItemSourceType.Resources)
		{
			obj = GetTaiwuVillageResources(itemSourceType);
		}
		else
		{
			GameData.Domains.Character.Character taiwu = GetTaiwu();
			obj = ((taiwu != null) ? taiwu.GetResources() : default(ResourceInts));
		}
		ResourceInts item = obj;
		return (itemSourceType: itemSourceType, resource: item);
	}

	public IReadOnlyDictionary<ItemKey, int> GetItems(ItemSourceType itemSourceType)
	{
		return itemSourceType switch
		{
			ItemSourceType.Equipment => (from k in _taiwuChar.GetEquipment()
				select (k) into k
				where k.IsValid()
				select k).ToDictionary((ItemKey k) => k, (ItemKey _) => 1), 
			ItemSourceType.Inventory => _taiwuChar.GetInventory().Items, 
			ItemSourceType.Warehouse => _warehouseItems, 
			ItemSourceType.Trough => DomainManager.Extra.TroughItems, 
			ItemSourceType.StockStorageWarehouse => DomainManager.Extra.GetStockStorage().Inventories[0].Items, 
			ItemSourceType.StockStorageGoodsShelf => DomainManager.Extra.GetStockStorage().Inventories[1].Items, 
			ItemSourceType.Treasury => GetTaiwuTreasury().Inventory.Items, 
			_ => throw new ArgumentOutOfRangeException("itemSourceType", itemSourceType, null), 
		};
	}

	private ResourceInts GetTaiwuVillageResources(ItemSourceType itemSourceType)
	{
		if (1 == 0)
		{
		}
		ResourceInts result = itemSourceType switch
		{
			ItemSourceType.StockStorageWarehouse => DomainManager.Extra.GetStockStorage().Resources, 
			ItemSourceType.Treasury => GetTaiwuTreasury()?.Resources ?? default(ResourceInts), 
			_ => default(ResourceInts), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	[Obsolete]
	public void ReceiveItems(DataContext context, params (ItemKey itemKey, int amount)[] items)
	{
		if (items.Length != 0)
		{
			for (int i = 0; i < items.Length; i++)
			{
				(ItemKey, int) tuple = items[i];
				ItemDisplayData itemDisplayData = DomainManager.Item.GetItemDisplayData(tuple.Item1, _taiwuCharId);
				itemDisplayData.Amount = tuple.Item2;
				_receivedItems.Add(itemDisplayData);
			}
			SetReceivedItems(_receivedItems, context);
		}
	}

	[Obsolete]
	public void ReceiveItems(DataContext context, IEnumerable<(ItemKey itemKey, int amount)> items)
	{
		foreach (var item in items)
		{
			ItemDisplayData itemDisplayData = DomainManager.Item.GetItemDisplayData(item.itemKey, _taiwuCharId);
			itemDisplayData.Amount = item.amount;
			_receivedItems.Add(itemDisplayData);
		}
		SetReceivedItems(_receivedItems, context);
	}

	[Obsolete]
	public void ReceiveCharacters(DataContext context, params int[] charIds)
	{
		for (int i = 0; i < charIds.Length; i++)
		{
			CharacterDisplayData characterDisplayData = DomainManager.Character.GetCharacterDisplayData(charIds[i]);
			_receivedCharacters.Add(characterDisplayData);
		}
		SetReceivedCharacters(_receivedCharacters, context);
	}

	[Obsolete]
	public void ReceiveCharacters(DataContext context, IEnumerable<int> charIds)
	{
		foreach (int charId in charIds)
		{
			CharacterDisplayData characterDisplayData = DomainManager.Character.GetCharacterDisplayData(charId);
			_receivedCharacters.Add(characterDisplayData);
		}
		SetReceivedCharacters(_receivedCharacters, context);
	}

	[DomainMethod]
	public void SwitchEquipmentPlan(DataContext context, int planId)
	{
		EquipmentPlan equipmentPlan = _equipmentsPlans[_currEquipmentPlanId];
		equipmentPlan.Record(_taiwuChar);
		SetElement_EquipmentsPlans(_currEquipmentPlanId, equipmentPlan, context);
		EquipmentPlan equipmentPlan2 = _equipmentsPlans[planId];
		equipmentPlan2.Apply(context, _taiwuChar, skipInvalid: false);
		equipmentPlan2.Record(_taiwuChar);
		SetElement_EquipmentsPlans(planId, equipmentPlan2, context);
		SetCurrEquipmentPlanId(planId, context);
	}

	public bool CheckItemIsInEquipmentPlan(ItemKey itemKey)
	{
		int planIndex;
		int slotIndex;
		return FindItemInEquipmentPlan(itemKey, out planIndex, out slotIndex);
	}

	public bool FindItemInEquipmentPlan(ItemKey itemKey, out int planIndex, out int slotIndex)
	{
		planIndex = -1;
		slotIndex = -1;
		if (!ItemType.IsEquipmentItemType(itemKey.ItemType))
		{
			return false;
		}
		for (int i = 0; i < _equipmentsPlans.Length; i++)
		{
			if (i == _currEquipmentPlanId)
			{
				continue;
			}
			EquipmentPlan equipmentPlan = _equipmentsPlans[i];
			for (int j = 0; j < equipmentPlan.Slots.Length; j++)
			{
				ItemKey itemKey2 = equipmentPlan.Slots[j];
				if (itemKey2.Equals(itemKey))
				{
					planIndex = i;
					slotIndex = j;
					return true;
				}
			}
		}
		return false;
	}

	public void SetEquipmentPlan(DataContext context, ItemKey itemKey, int planIndex, int slotIndex)
	{
		if (_equipmentsPlans.CheckIndex(planIndex))
		{
			EquipmentPlan element_EquipmentsPlans = GetElement_EquipmentsPlans(planIndex);
			if (element_EquipmentsPlans.Slots.CheckIndex(slotIndex))
			{
				element_EquipmentsPlans.Slots[slotIndex] = itemKey;
				SetElement_EquipmentsPlans(planIndex, element_EquipmentsPlans, context);
			}
		}
	}

	public void ReCalcAllEquipment(DataContext context)
	{
		ItemKey[] equipment = _taiwuChar.GetEquipment();
		ItemKey[] array = equipment;
		for (int i = 0; i < array.Length; i++)
		{
			ItemKey itemKey = array[i];
			if (itemKey.IsValid())
			{
				UpdateEquipment(itemKey);
			}
		}
		_taiwuChar.SetEquipment(equipment, context);
		Inventory inventory = _taiwuChar.GetInventory();
		if (UpdateEquipmentsInDictionary(inventory.Items))
		{
			_taiwuChar.SetInventory(inventory, context);
		}
		if (UpdateEquipmentsInDictionary(_warehouseItems))
		{
			KeyValuePair<ItemKey, int> keyValuePair = _warehouseItems.First();
			SetElement_WarehouseItems(keyValuePair.Key, keyValuePair.Value, context);
		}
		TaiwuVillageStorage stockStorage = DomainManager.Extra.GetStockStorage();
		Inventory[] inventories = stockStorage.Inventories;
		foreach (Inventory inventory2 in inventories)
		{
			if (UpdateEquipmentsInDictionary(inventory2.Items))
			{
				stockStorage.NeedCommit = true;
			}
		}
		TaiwuVillageStorage craftStorage = DomainManager.Extra.GetCraftStorage();
		Inventory[] inventories2 = craftStorage.Inventories;
		foreach (Inventory inventory3 in inventories2)
		{
			if (UpdateEquipmentsInDictionary(inventory3.Items))
			{
				craftStorage.NeedCommit = true;
			}
		}
		bool UpdateEquipment(ItemKey itemKey2)
		{
			if (ItemTemplateHelper.IsRefinable(itemKey2.ItemType, itemKey2.TemplateId) && ModificationStateHelper.IsActive(itemKey2.ModificationState, 2))
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey2);
				baseEquipment.SetModificationState(baseEquipment.GetModificationState(), context);
				return true;
			}
			return false;
		}
		bool UpdateEquipmentsInDictionary(Dictionary<ItemKey, int> items)
		{
			bool result = false;
			foreach (var (itemKey3, num2) in items)
			{
				if (UpdateEquipment(itemKey3))
				{
					result = true;
				}
			}
			return result;
		}
	}

	[DomainMethod]
	public void TaiwuFollowNpc(DataContext context, int npcId)
	{
		List<int> followingNpcList = DomainManager.Extra.GetFollowingNpcList();
		if (OfflineFollowNpc(followingNpcList, npcId))
		{
			DomainManager.Extra.SaveFollowingNpcList(context, followingNpcList);
		}
	}

	private bool OfflineFollowNpc(List<int> followingNpcList, int npcId)
	{
		if (DomainManager.Taiwu.GetIsFollowingNpcListMax())
		{
			return false;
		}
		if (followingNpcList.Contains(npcId))
		{
			return false;
		}
		followingNpcList.Add(npcId);
		return true;
	}

	[DomainMethod]
	public void TaiwuUnfollowNpc(DataContext context, int characterId)
	{
		List<int> followingNpcList = DomainManager.Extra.GetFollowingNpcList();
		if (OfflineUnfollowNpc(followingNpcList, characterId))
		{
			DomainManager.Extra.RemoveFollowingNpcNickName(context, characterId);
			DomainManager.Extra.SaveFollowingNpcList(context, followingNpcList);
		}
	}

	private bool OfflineUnfollowNpc(List<int> followingNpcList, int npcId)
	{
		if (!followingNpcList.Contains(npcId))
		{
			return false;
		}
		followingNpcList.Remove(npcId);
		return true;
	}

	[DomainMethod]
	public void GmCmd_FollowRandomNpc(DataContext context, int count)
	{
		List<GameData.Domains.Character.Character> list = new List<GameData.Domains.Character.Character>();
		DomainManager.Character.FindIntelligentCharacters((GameData.Domains.Character.Character _) => true, list);
		List<int> followingNpcList = DomainManager.Extra.GetFollowingNpcList();
		for (int num = 0; num < count; num++)
		{
			int index = context.Random.Next(0, list.Count);
			GameData.Domains.Character.Character character = list[index];
			OfflineFollowNpc(followingNpcList, character.GetId());
		}
		DomainManager.Extra.SaveFollowingNpcList(context, followingNpcList);
	}

	public bool IsCharacterFollowedByTaiwu(int npcId)
	{
		List<int> followingNpcList = DomainManager.Extra.GetFollowingNpcList();
		return followingNpcList.Contains(npcId);
	}

	[DomainMethod]
	public int GetFollowingNpcListMaxCount()
	{
		return 10 + DomainManager.Map.GetStationUnlockedRegularAreaCount();
	}

	[DomainMethod]
	public bool GetIsFollowingNpcListMax()
	{
		return DomainManager.Extra.GetFollowingNpcList().Count >= DomainManager.Taiwu.GetFollowingNpcListMaxCount();
	}

	public void GenerateAllFollowingMonthNotifications()
	{
		List<int> followingNpcList = DomainManager.Extra.GetFollowingNpcList();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		foreach (int item in followingNpcList)
		{
			GenerateSingleFollowingMonthNotification(item, monthlyNotificationCollection);
		}
	}

	private void GenerateSingleFollowingMonthNotification(int characterId, MonthlyNotificationCollection monthlyNotificationCollection)
	{
		if (DomainManager.Character.TryGetElement_Objects(characterId, out var element))
		{
			TryGenerateHealthNotification(element, monthlyNotificationCollection);
			bool flag = TryGenerateDisorderOfQiNotification(element, monthlyNotificationCollection);
			if (!flag)
			{
				flag = TryGenerateInjuryNotification(element, monthlyNotificationCollection);
			}
			if (!flag)
			{
				TryGeneratePoisonNotification(element, monthlyNotificationCollection);
			}
			TryGenerateKidnappedOrInAdventure(element, monthlyNotificationCollection);
		}
	}

	private void TryGenerateKidnappedOrInAdventure(GameData.Domains.Character.Character character, MonthlyNotificationCollection monthlyNotificationCollection)
	{
		if (character.GetKidnapperId() != -1)
		{
			monthlyNotificationCollection.AddTrappedNotice(character.GetId());
		}
		else if (character.IsActiveExternalRelationState(4) && character.GetKidnappingEnemyNestAdventure() != -1)
		{
			monthlyNotificationCollection.AddTrappedNotice(character.GetId());
		}
	}

	private bool TryGeneratePoisonNotification(GameData.Domains.Character.Character character, MonthlyNotificationCollection monthlyNotificationCollection)
	{
		PoisonInts poisoned = character.GetPoisoned();
		for (int i = 0; i < 6; i++)
		{
			if (poisoned[i] > 0)
			{
				monthlyNotificationCollection.AddInjuredNotice(character.GetId());
				return true;
			}
		}
		return false;
	}

	private bool TryGenerateInjuryNotification(GameData.Domains.Character.Character character, MonthlyNotificationCollection monthlyNotificationCollection)
	{
		Injuries injuries = character.GetInjuries();
		for (sbyte b = 0; b < 7; b++)
		{
			var (b2, b3) = injuries.Get(b);
			if (b2 >= 2 && b3 >= 2)
			{
				monthlyNotificationCollection.AddInjuredNotice(character.GetId());
				return true;
			}
		}
		return false;
	}

	private bool TryGenerateDisorderOfQiNotification(GameData.Domains.Character.Character character, MonthlyNotificationCollection monthlyNotificationCollection)
	{
		short disorderOfQi = character.GetDisorderOfQi();
		if (disorderOfQi > 400)
		{
			monthlyNotificationCollection.AddInjuredNotice(character.GetId());
			return true;
		}
		return false;
	}

	private void TryGenerateHealthNotification(GameData.Domains.Character.Character character, MonthlyNotificationCollection monthlyNotificationCollection)
	{
		short health = character.GetHealth();
		short leftMaxHealth = character.GetLeftMaxHealth();
		if (leftMaxHealth > health * 5)
		{
			monthlyNotificationCollection.AddDyingNotice(character.GetId());
		}
	}

	[DomainMethod]
	public int SetFollowingNpcNickName(DataContext context, int characterId, string nickName)
	{
		return DomainManager.Extra.SetFollowingNpcNickName(context, characterId, nickName);
	}

	[DomainMethod]
	public string GetFollowingNpcNickName(int characterId)
	{
		if (DomainManager.Extra.TryGetElement_FollowingNickNameMap(characterId, out var value))
		{
			return DomainManager.World.GetElement_CustomTexts(value);
		}
		return null;
	}

	[DomainMethod]
	public int GetFollowingNpcNickNameId(int characterId)
	{
		if (DomainManager.Extra.TryGetElement_FollowingNickNameMap(characterId, out var value))
		{
			return value;
		}
		return -1;
	}

	public void UpdateFollowingNotifications(DataContext context)
	{
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		foreach (int followingNpc in DomainManager.Extra.GetFollowingNpcList())
		{
			if (!DomainManager.Character.TryGetElement_Objects(followingNpc, out var _))
			{
				continue;
			}
			foreach (sbyte fugitiveBountySect in DomainManager.Organization.GetFugitiveBountySects(followingNpc))
			{
				monthlyNotificationCollection.AddWantedNotice(followingNpc, DomainManager.Organization.GetSettlementIdByOrgTemplateId(fugitiveBountySect));
			}
		}
	}

	[DomainMethod]
	public void GmCmd_AddResource(DataContext context, sbyte type, int count)
	{
		if (count < 0)
		{
			count = Math.Max(count, -_taiwuChar.GetResource(type));
		}
		_taiwuChar.ChangeResource(context, type, count);
	}

	[DomainMethod]
	public void GmCmd_AddLegacyPoint(DataContext context, short template, int percent)
	{
		AddLegacyPoint(context, template, percent);
	}

	[DomainMethod]
	public void GmCmd_FillLegacyPoint(DataContext context, short templateId)
	{
		LegacyPointItem configData = LegacyPoint.Instance[templateId];
		int legacyMaxPoint = GetLegacyMaxPoint(configData);
		DirectlyAddLegacyPoint(context, templateId, legacyMaxPoint);
	}

	[DomainMethod]
	public void GmCmd_AddExp(DataContext context, int count)
	{
		_taiwuChar.ChangeExp(context, count);
	}

	[DomainMethod]
	public void GmCmd_SetTaiwuCombatSkillActiveState(DataContext context, short skillTemplateId, ushort selectedPages, bool bonusOn = false)
	{
		if (_skillBreakBonusDict.ContainsKey(skillTemplateId))
		{
			RemoveElement_SkillBreakBonusDict(skillTemplateId, context);
		}
		if (_taiwuChar.IsCombatSkillEquipped(skillTemplateId))
		{
			DomainManager.SpecialEffect.Remove(context, _taiwuCharId, skillTemplateId, 2);
		}
		if (CombatSkillStateHelper.IsBrokenOut(selectedPages))
		{
			ClearBreakPlate(context, skillTemplateId, fromGmCmd: true);
			SkillBreakPlate skillBreakPlate = EnterSkillBreakPlate(context, skillTemplateId, selectedPages);
			if (bonusOn)
			{
				for (int i = 0; i < skillBreakPlate.Width; i++)
				{
					for (int j = 0; j < skillBreakPlate.Height; j++)
					{
						SkillBreakPlateGrid skillBreakPlateGrid = skillBreakPlate[i, j];
						if (skillBreakPlateGrid.AddMaxPower > 0 || skillBreakPlateGrid.TemplateId == 2)
						{
							skillBreakPlateGrid.State = ESkillBreakGridState.Selected;
						}
					}
				}
			}
			skillBreakPlate.SelectBreakWithoutCheck((x: ((int)skillBreakPlate.Width - ((!SkillBreakPlate.IsProtrusion(skillBreakPlate.Height / 2)) ? 1 : 0)) / 2, y: skillBreakPlate.Height / 2));
			FinishBreak(context, skillTemplateId, skillBreakPlate);
			DomainManager.Extra.SetOrAddSkillBreakPlate(context, skillTemplateId, skillBreakPlate);
		}
		else
		{
			if (_skillBreakPlateObsoleteDict.ContainsKey(skillTemplateId))
			{
				RemoveElement_SkillBreakPlateObsoleteDict(skillTemplateId, context);
			}
			DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_taiwuCharId, skillTemplateId)).SetActivationState(selectedPages, context);
			CharacterDomain.TryRemoveCombatSkillFromAttainmentPanel(context, _taiwuChar, skillTemplateId);
		}
	}

	[DomainMethod]
	public void GmCmd_MarkAllCarrierFullTamePoint(DataContext context)
	{
		List<int> carrierIds = new List<int>();
		TryAddCarrierId(_taiwuChar.GetEquipment()[11]);
		Inventory inventory = _taiwuChar.GetInventory();
		foreach (ItemKey key in inventory.Items.Keys)
		{
			TryAddCarrierId(key);
		}
		foreach (int item in carrierIds)
		{
			int carrierMaxTamePoint = DomainManager.Extra.GetCarrierMaxTamePoint(item);
			DomainManager.Extra.SetCarrierTamePoint(context, item, carrierMaxTamePoint);
		}
		void TryAddCarrierId(ItemKey itemKey)
		{
			if (itemKey.IsValid() && itemKey.ItemType == 4 && DomainManager.Extra.IsCarrierTamable(itemKey.TemplateId) && !DomainManager.Extra.IsCarrierFullTamePoint(itemKey))
			{
				carrierIds.Add(itemKey.Id);
			}
		}
	}

	[DomainMethod]
	public void GmCmd_LifeSkillCombatAiSetAlwaysUseForceAdversary(DataContext context, sbyte playerId, bool active)
	{
		if (_lifeSkillCombat != null)
		{
			Match.Ai aiState = _lifeSkillCombat.GetAiState(playerId);
			aiState.AlwaysUseForceAdversary = active;
		}
	}

	[DomainMethod]
	public void JoinGroup(DataContext context, int charId, bool showNotification = true)
	{
		DomainManager.Extra.ApplyGroupCharEquipmentPlan(context, charId);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		if (element_Objects.GetLeaderId() >= 0)
		{
			DomainManager.Character.LeaveGroup(context, element_Objects);
		}
		if (element_Objects.IsCreatedWithFixedTemplate() && charId != _taiwuCharId)
		{
			element_Objects.SetLeaderId(_taiwuCharId, context);
			DomainManager.Extra.AddSpecialGroupCharacter(context, _taiwuCharId, charId);
			return;
		}
		if (element_Objects.GetLeaderId() == charId)
		{
			foreach (int item in DomainManager.Character.GetGroup(charId).GetCollection())
			{
				if (item != charId)
				{
					JoinGroup(context, item);
				}
			}
		}
		CharacterCombatSkillConfiguration characterCombatSkillConfiguration = DomainManager.Extra.TryGetCharacterCombatSkillConfiguration(charId);
		if (characterCombatSkillConfiguration != null)
		{
			if (characterCombatSkillConfiguration.IsCombatSkillLocked)
			{
				DomainManager.Character.ApplyCombatSkillPlan(context, element_Objects, characterCombatSkillConfiguration.CurrentEquipPlan);
				DomainManager.Extra.SetCharacterMasteredCombatSkills(context, charId, characterCombatSkillConfiguration.CurrentMasterPlan);
			}
			if (characterCombatSkillConfiguration.IsNeiliAllocationLocked)
			{
				DomainManager.Character.ApplyNeiliAllocation(context, element_Objects, characterCombatSkillConfiguration.NeiliAllocation);
			}
			if (characterCombatSkillConfiguration.IsCombatSkillAttainmentLocked)
			{
				DomainManager.Character.ApplyCombatSkillAttainmentPanels(context, element_Objects, characterCombatSkillConfiguration.CombatSkillAttainmentPanels);
			}
		}
		_groupCharIds.Add(charId);
		SetGroupCharIds(_groupCharIds, context);
		element_Objects.SetLeaderId(_taiwuCharId, context);
		Events.RaiseCharacterLocationChanged(context, charId, element_Objects.GetLocation(), Location.Invalid);
		element_Objects.SetLocation(_taiwuChar.GetLocation(), context);
		OrganizationInfo organizationInfo = element_Objects.GetOrganizationInfo();
		OrganizationInfo organizationInfo2 = _taiwuChar.GetOrganizationInfo();
		if (organizationInfo.OrgTemplateId != organizationInfo2.OrgTemplateId || organizationInfo.SettlementId != organizationInfo2.SettlementId)
		{
			organizationInfo2.Grade = 0;
			organizationInfo2.Principal = true;
			DomainManager.Organization.ChangeOrganization(context, element_Objects, organizationInfo2);
		}
		else if (organizationInfo.OrgTemplateId == 16)
		{
			DomainManager.Taiwu.TryRemoveTaiwuVillageResident(context, charId);
			DomainManager.Building.TryRemoveFeastCustomer(context, charId);
		}
		Tester.Assert(!element_Objects.IsActiveExternalRelationState(32));
		Tester.Assert(!element_Objects.IsActiveExternalRelationState(8));
		if (element_Objects.IsActiveExternalRelationState(60))
		{
			element_Objects.DeactivateExternalRelationState(context, 60);
		}
		foreach (int item2 in _groupCharIds.GetCollection())
		{
			if (item2 != charId)
			{
				DomainManager.Character.TryCreateRelation(context, charId, item2);
			}
		}
		short sectXuannvPlayerMusicId = DomainManager.Extra.GetSectXuannvPlayerMusicId();
		bool sectXuannvPlayerIsEnabled = DomainManager.Extra.GetSectXuannvPlayerIsEnabled();
		if (sectXuannvPlayerMusicId >= 0 && sectXuannvPlayerIsEnabled)
		{
			short temporaryFeature = Music.Instance[sectXuannvPlayerMusicId].TemporaryFeature;
			element_Objects.AddFeature(context, temporaryFeature, removeMutexFeature: true);
		}
		if (showNotification)
		{
			InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
			instantNotificationCollection.AddJoinGroup(charId);
		}
		DomainManager.TaiwuEvent.RecordTeammateStateChanged(isLosing: false, charId);
	}

	[DomainMethod]
	public void LeaveGroup(DataContext context, int charId, bool bringWards = true, bool showNotification = true, bool moveToRandomAdjacentBlock = true)
	{
		for (int i = 0; i < _combatGroupCharIds.Length; i++)
		{
			if (_combatGroupCharIds[i] == charId)
			{
				SetElement_CombatGroupCharIds(i, -1, context);
			}
		}
		GameData.Domains.Character.Character element;
		bool flag = DomainManager.Character.TryGetElement_Objects(charId, out element);
		if (flag && element.IsCreatedWithFixedTemplate() && charId != _taiwuCharId)
		{
			element.SetLeaderId(-1, context);
			element.RemoveFeatureGroup(context, 416);
			DomainManager.Extra.TryRemoveSpecialGroupCharacter(context, _taiwuCharId, charId);
			return;
		}
		if (!_groupCharIds.Remove(charId).Item2)
		{
			throw new Exception($"Character {charId} is not in the group");
		}
		SetGroupCharIds(_groupCharIds, context);
		DomainManager.Extra.RemoveManualChangeEquipGroupChar(context, charId);
		DomainManager.Extra.SaveGroupCharEquipmentPlan(context, charId);
		if (!flag)
		{
			return;
		}
		CharacterCombatSkillConfiguration characterCombatSkillConfiguration = DomainManager.Extra.TryGetCharacterCombatSkillConfiguration(charId);
		if (characterCombatSkillConfiguration != null)
		{
			if (characterCombatSkillConfiguration.IsCombatSkillLocked)
			{
				characterCombatSkillConfiguration.OfflineRecordCombatSkillPlan(element, characterCombatSkillConfiguration.CurrentPlanId);
			}
			if (characterCombatSkillConfiguration.IsNeiliAllocationLocked)
			{
				characterCombatSkillConfiguration.OfflineRecordNeiliAllocation(element);
			}
			if (characterCombatSkillConfiguration.IsCombatSkillAttainmentLocked)
			{
				characterCombatSkillConfiguration.OfflineRecordCombatSkillAttainmentPanels(element);
			}
			DomainManager.Extra.SetCharacterCombatSkillConfiguration(context, charId, characterCombatSkillConfiguration);
		}
		element.SetLeaderId(-1, context);
		Location location = _taiwuChar.GetLocation();
		if (!location.IsValid())
		{
			location = _taiwuChar.GetValidLocation();
		}
		if (moveToRandomAdjacentBlock)
		{
			short randomAdjacentBlockId = DomainManager.Map.GetRandomAdjacentBlockId(context.Random, location.AreaId, location.BlockId);
			location.BlockId = randomAdjacentBlockId;
		}
		Events.RaiseCharacterLocationChanged(context, charId, element.GetLocation(), location);
		element.SetLocation(location, context);
		element.RemoveFeatureGroup(context, 416);
		if (element.GetOrganizationInfo().OrgTemplateId == 16)
		{
			DomainManager.Taiwu.AddTaiwuVillageResident(context, charId);
		}
		if (bringWards)
		{
			HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
			DomainManager.Character.GetWardsInGroup(element, _groupCharIds, hashSet);
			DomainManager.Character.ChangeGroup(context, hashSet, element);
		}
		if (showNotification)
		{
			InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
			instantNotificationCollection.AddLeaveGroup(charId);
		}
		DomainManager.TaiwuEvent.RecordTeammateStateChanged(isLosing: true, charId);
	}

	[DomainMethod]
	public List<CharacterDisplayData> GetInventoryOverloadedGroupCharNames()
	{
		List<CharacterDisplayData> list = new List<CharacterDisplayData>();
		foreach (int item in _groupCharIds.GetCollection())
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			int currInventoryLoad = element_Objects.GetCurrInventoryLoad();
			int maxInventoryLoad = element_Objects.GetMaxInventoryLoad();
			if (currInventoryLoad > maxInventoryLoad)
			{
				CharacterDisplayData characterDisplayData = DomainManager.Character.GetCharacterDisplayData(item);
				list.Add(characterDisplayData);
			}
		}
		foreach (int item2 in _taiwuSpecialGroup)
		{
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2);
			int currInventoryLoad2 = element_Objects2.GetCurrInventoryLoad();
			int maxInventoryLoad2 = element_Objects2.GetMaxInventoryLoad();
			if (currInventoryLoad2 > maxInventoryLoad2)
			{
				CharacterDisplayData characterDisplayData2 = DomainManager.Character.GetCharacterDisplayData(item2);
				list.Add(characterDisplayData2);
			}
		}
		return list;
	}

	[DomainMethod]
	public List<NameRelatedData> GetSeverelyInjuredGroupCharNames(bool includeTaiwu = false)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		List<NameRelatedData> list = new List<NameRelatedData>();
		if (includeTaiwu)
		{
			bool flag = false;
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			for (sbyte b = 0; b < 7; b++)
			{
				var (b2, b3) = taiwu.GetInjuries().Get(b);
				if (b2 >= 2 || b3 >= 2)
				{
					NameRelatedData data = new NameRelatedData();
					CharacterDomain.GetNameRelatedData(taiwu, ref data);
					list.Add(data);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(new NameRelatedData
				{
					CharTemplateId = -1
				});
			}
		}
		foreach (int item in _groupCharIds.GetCollection())
		{
			if (item == taiwuCharId)
			{
				continue;
			}
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			for (sbyte b4 = 0; b4 < 7; b4++)
			{
				var (b5, b6) = element_Objects.GetInjuries().Get(b4);
				if (b5 >= 2 || b6 >= 2)
				{
					NameRelatedData data2 = new NameRelatedData();
					CharacterDomain.GetNameRelatedData(element_Objects, ref data2);
					list.Add(data2);
					break;
				}
			}
		}
		return list;
	}

	[DomainMethod]
	public List<NameRelatedData> GetDyingGroupCharNames(bool includeTaiwu = false)
	{
		List<NameRelatedData> list = new List<NameRelatedData>();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		foreach (int item in _groupCharIds.GetCollection())
		{
			if (item != taiwuCharId || includeTaiwu)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				EHealthType healthType = element_Objects.GetHealthType();
				if ((uint)healthType <= 1u)
				{
					NameRelatedData data = new NameRelatedData();
					CharacterDomain.GetNameRelatedData(element_Objects, ref data);
					list.Add(data);
				}
			}
		}
		return list;
	}

	public bool IsInGroup(int charId)
	{
		return _groupCharIds.Contains(charId) || DomainManager.Extra.GetSpecialGroup(_taiwuCharId).Contains(charId);
	}

	public int GetCharCombatGroupIndex(int charId)
	{
		return _combatGroupCharIds.IndexOf(charId);
	}

	public void UpdateTaiwuGroupFavorabilities(DataContext context)
	{
		if (_groupCharIds.GetCount() <= 1)
		{
			return;
		}
		HashSet<int> collection = _groupCharIds.GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			(short, short) tuple = AiHelper.UpdateStatusConstants.SameGroupFavorabilityChange[element_Objects.GetBehaviorType()];
			foreach (int item2 in collection)
			{
				if (item != item2)
				{
					GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2);
					int baseDelta = context.Random.Next((int)tuple.Item1, tuple.Item2 + 1);
					DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, element_Objects, element_Objects2, baseDelta);
				}
			}
		}
	}

	public void UpdateChildrenEducation(DataContext context)
	{
		HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(_taiwuCharId, 2);
		sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		foreach (int item in relatedCharIds)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && IsInGroup(item) && element.GetBirthMonth() == currMonthInYear)
			{
				switch (element.GetCurrAge())
				{
				case 1:
					element.RemoveFeatureGroup(context, 171);
					monthlyEventCollection.AddChildZhuazhou(item);
					break;
				case 3:
				case 6:
				case 9:
				case 12:
					monthlyEventCollection.AddTeachChild(item);
					break;
				case 15:
					monthlyEventCollection.AddReachAdulthood(item);
					break;
				}
			}
		}
	}

	[DomainMethod]
	public int GetGroupBabyCount()
	{
		int num = 0;
		foreach (int item in _groupCharIds.GetCollection())
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.GetAgeGroup() == 0)
			{
				num++;
			}
		}
		return num;
	}

	[DomainMethod]
	public List<CharacterDisplayData> GetGroupNeiliConflictingCharDataList()
	{
		List<CharacterDisplayData> list = new List<CharacterDisplayData>();
		List<int> list2 = GetGroupCharIds().GetCollection().ToList();
		int taiwuCharId = GetTaiwuCharId();
		list2.Remove(taiwuCharId);
		list2.Insert(0, taiwuCharId);
		foreach (int item in list2)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			NeiliTypeItem neiliTypeItem = NeiliType.Instance[element_Objects.GetNeiliType()];
			if (neiliTypeItem.ShowConflictingWorldState)
			{
				CharacterDisplayData characterDisplayData = DomainManager.Character.GetCharacterDisplayData(item);
				list.Add(characterDisplayData);
			}
		}
		return list;
	}

	public void SetLegacyPassingState(DataContext context, sbyte legacyPassingState)
	{
		SetLegacyPassingState(legacyPassingState, context);
		Events.RaiseEventOnLegacyPassingStateChange(context, legacyPassingState);
	}

	[DomainMethod]
	public void CompletePassingLegacy(DataContext context)
	{
		_taiwuChar.ChangeResource(context, 7, _legacyPoint / 4);
		SetIsTaiwuDieOfCombatWithXiangshu(value: false, context);
	}

	[DomainMethod]
	public void SelectLegacy(DataContext context, short templateId)
	{
		LegacyItem legacyItem = Legacy.Instance[templateId];
		_selectedLegacies.Add(templateId);
		if (legacyItem.PermanentlyUnique)
		{
			DomainManager.Extra.AddSelectedUniqueLegacies(context, templateId);
		}
	}

	[DomainMethod]
	public int GetLegacyMaxPointByType(short legacyType)
	{
		int num = 0;
		foreach (LegacyPointItem item in (IEnumerable<LegacyPointItem>)LegacyPoint.Instance)
		{
			if (item.Type == legacyType)
			{
				num += GetLegacyMaxPoint(item);
			}
		}
		return num;
	}

	[DomainMethod]
	public List<IntList> GetLegacyMaxPointAndTimesListByType(short legacyType)
	{
		List<IntList> list = new List<IntList>();
		foreach (LegacyPointItem item2 in (IEnumerable<LegacyPointItem>)LegacyPoint.Instance)
		{
			if (item2.Type == legacyType)
			{
				IntList item = IntList.Create();
				item.Items.Add(item2.TemplateId);
				item.Items.Add(GetLegacyMaxPoint(item2));
				if (DomainManager.Extra.TryGetElement_LegacyPointTimesDict(item2.TemplateId, out var value))
				{
					item.Items.Add(value);
				}
				else
				{
					item.Items.Add(0);
				}
				list.Add(item);
			}
		}
		return list;
	}

	[DomainMethod]
	public List<short> GetRandomLegaciesInGroup(DataContext context, sbyte groupId, int count)
	{
		List<short> list = new List<short>();
		while (list.Count < count)
		{
			short randomAvailableLegacy = GetRandomAvailableLegacy(context.Random, groupId);
			if (!list.Contains(randomAvailableLegacy))
			{
				list.Add(randomAvailableLegacy);
			}
		}
		return list;
	}

	public void SetIsTaiwuDying(bool isTaiwuDying)
	{
		_isTaiwuDying = isTaiwuDying;
	}

	public void AddLegacyPoint(DataContext context, short templateId, int percent = 100)
	{
		LegacyPointItem legacyPointItem = LegacyPoint.Instance[templateId];
		int num = legacyPointItem.BasePoint * percent / 100;
		int legacySettingsPercent = GetLegacySettingsPercent(legacyPointItem);
		num = num * legacySettingsPercent / 100;
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		int buildingBlockEffect = DomainManager.Building.GetBuildingBlockEffect(taiwuVillageSettlementId, EBuildingScaleEffect.LegacyPointBonusFactor);
		num = num * (100 + buildingBlockEffect) / 100;
		DirectlyAddLegacyPoint(context, templateId, num);
	}

	private void DirectlyAddLegacyPoint(DataContext context, short templateId, int addValue)
	{
		LegacyPointItem legacyPointItem = LegacyPoint.Instance[templateId];
		LegacyPointTypeItem legacyPointTypeItem = LegacyPointType.Instance[legacyPointItem.Type];
		int legacyMaxPoint = GetLegacyMaxPoint(legacyPointItem);
		if (!_legacyPointDict.TryGetValue(templateId, out var value))
		{
			AddElement_LegacyPointDict(templateId, 0, context);
			value = 0;
		}
		addValue = Math.Min(addValue, legacyMaxPoint - value);
		if (addValue > 0)
		{
			int num = value + addValue;
			SetElement_LegacyPointDict(templateId, (short)num, context);
			SetLegacyPoint(_legacyPoint + addValue, context);
			DomainManager.Extra.IncreaseLegacyPointTimes(context, templateId);
			if (value < legacyMaxPoint / 2 && num >= legacyMaxPoint / 2)
			{
				short randomAvailableLegacy = GetRandomAvailableLegacy(context.Random, legacyPointTypeItem.Group);
				AddAvailableLegacy(context, randomAvailableLegacy);
			}
			if (value < legacyMaxPoint && num == legacyMaxPoint)
			{
				short randomAvailableLegacy2 = GetRandomAvailableLegacy(context.Random, legacyPointTypeItem.Group);
				AddAvailableLegacy(context, randomAvailableLegacy2);
			}
		}
	}

	public int GetLegacyPointCountForLegacyType(sbyte legacyPointType)
	{
		int num = 0;
		foreach (KeyValuePair<short, short> item in _legacyPointDict)
		{
			LegacyPointItem legacyPointItem = LegacyPoint.Instance[item.Key];
			if (legacyPointItem.Type == legacyPointType)
			{
				num += item.Value;
			}
		}
		return num;
	}

	public static int GetLegacyMaxPoint(LegacyPointItem configData)
	{
		int legacySettingsPercent = GetLegacySettingsPercent(configData);
		return configData.MaxPoint * legacySettingsPercent / 100;
	}

	public static int GetLegacySettingsPercent(LegacyPointItem configData)
	{
		int num = 100;
		byte[] bonusTypes = configData.BonusTypes;
		foreach (byte b in bonusTypes)
		{
			int worldCreationSetting = DomainManager.World.GetWorldCreationSetting(b);
			if (worldCreationSetting >= 0)
			{
				short[] legacyPointBonus = WorldCreation.Instance[b].LegacyPointBonus;
				if (legacyPointBonus.CheckIndex(worldCreationSetting))
				{
					num += legacyPointBonus[worldCreationSetting];
					continue;
				}
				PredefinedLog.Show(19, $"index {worldCreationSetting} is invalid for creatingType {b}");
			}
		}
		return num;
	}

	public bool AddAvailableLegacy(DataContext context, short templateId)
	{
		LegacyItem legacyItem = Legacy.Instance[templateId];
		if (legacyItem.IsUnique && _availableLegacyList.Contains(templateId))
		{
			return false;
		}
		if (legacyItem.PermanentlyUnique && DomainManager.Extra.IsUniqueLegacySelected(templateId))
		{
			return false;
		}
		_availableLegacyList.Add(templateId);
		DomainManager.World.GetInstantNotifications().AddLegacy(templateId);
		SetAvailableLegacyList(_availableLegacyList, context);
		return true;
	}

	public short GetRandomAvailableLegacy(IRandomSource random, sbyte groupId = -1)
	{
		if (groupId < 0)
		{
			groupId = (sbyte)random.Next(WorldCreationGroup.Instance.Count - 1);
		}
		sbyte worldCreationGroupLevel = GetWorldCreationGroupLevel(groupId);
		return RandomUtils.GetRandomResult(_categorizedNormalLegacyConfigs[groupId][worldCreationGroupLevel], random);
	}

	public static sbyte GetWorldCreationGroupLevel(sbyte groupId)
	{
		int worldCreationGroupLegacyBonusSum = GetWorldCreationGroupLegacyBonusSum(groupId);
		for (sbyte b = (sbyte)(GlobalConfig.Instance.LegacyGroupLevelThresholds.Length - 1); b >= 0; b--)
		{
			if (worldCreationGroupLegacyBonusSum >= GlobalConfig.Instance.LegacyGroupLevelThresholds[b])
			{
				return b;
			}
		}
		throw new Exception($"Invalid legacy bonus sum {worldCreationGroupLegacyBonusSum} for group {groupId}.");
	}

	private static int GetWorldCreationGroupLegacyBonusSum(sbyte groupId)
	{
		int num = 0;
		WorldCreationGroupItem worldCreationGroupItem = WorldCreationGroup.Instance[groupId];
		byte[] worldCreations = worldCreationGroupItem.WorldCreations;
		foreach (byte b in worldCreations)
		{
			WorldCreationItem worldCreationItem = WorldCreation.Instance[b];
			int worldCreationSetting = DomainManager.World.GetWorldCreationSetting(b);
			if (worldCreationItem.LegacyPointBonus.CheckIndex(worldCreationSetting))
			{
				num += worldCreationItem.LegacyPointBonus[worldCreationSetting];
			}
		}
		return num;
	}

	private static void InitializeNormalLegacies()
	{
		int num = GlobalConfig.Instance.LegacyGroupLevelThresholds.Length;
		int num2 = WorldCreationGroup.Instance.Count - 1;
		_categorizedNormalLegacyConfigs = new List<(short, short)>[num2][];
		for (int i = 0; i < num2; i++)
		{
			_categorizedNormalLegacyConfigs[i] = new List<(short, short)>[num];
			for (int j = 0; j < num; j++)
			{
				_categorizedNormalLegacyConfigs[i][j] = new List<(short, short)>();
			}
		}
		foreach (LegacyItem item in (IEnumerable<LegacyItem>)Legacy.Instance)
		{
			if (item.Weight != 0)
			{
				_categorizedNormalLegacyConfigs[item.WorldCreationGroup][item.Level].Add((item.TemplateId, item.Weight));
			}
		}
	}

	public void PushStateNewCharacterLegacyGrowingGrades(DataContext context, sbyte stateId, IEnumerable<sbyte> grades)
	{
		SByteList value = _stateNewCharacterLegacyGrowingGrades[stateId];
		if (value.Items == null)
		{
			value = SByteList.Create();
		}
		value.Items.AddRange(grades);
		SetElement_StateNewCharacterLegacyGrowingGrades(stateId, value, context);
	}

	public sbyte PopStateNewCharacterLegacyGrowingGrade(DataContext context, sbyte stateId, sbyte minGrade = -1)
	{
		if (stateId < 0)
		{
			return minGrade;
		}
		SByteList value = _stateNewCharacterLegacyGrowingGrades[stateId];
		if (value.Items == null || value.Items.Count == 0)
		{
			return minGrade;
		}
		sbyte b = value.Items[0];
		if (b < minGrade)
		{
			return minGrade;
		}
		value.Items.RemoveAt(0);
		SetElement_StateNewCharacterLegacyGrowingGrades(stateId, value, context);
		return b;
	}

	public sbyte OfflinePopStateNewCharacterLegacyGrowingGrade(sbyte stateId)
	{
		if (stateId < 0)
		{
			return -1;
		}
		SByteList sByteList = _stateNewCharacterLegacyGrowingGrades[stateId];
		if (sByteList.Items == null || sByteList.Items.Count == 0)
		{
			return -1;
		}
		sbyte result = sByteList.Items[0];
		sByteList.Items.RemoveAt(0);
		return result;
	}

	[DomainMethod]
	public List<int> FindSuccessorCandidates(DataContext context)
	{
		Tester.Assert(_successorCandidates.Count == 0);
		switch (_legacyPassingState)
		{
		case 1:
			foreach (int item in _groupCharIds.GetCollection())
			{
				if (_taiwuCharId != item)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					if (AgeGroup.GetAgeGroup(element_Objects.GetCurrAge()) != 0)
					{
						_successorCandidates.Add(item);
					}
				}
			}
			break;
		case 2:
		{
			Location location = _taiwuChar.GetLocation();
			if (!location.IsValid())
			{
				location = _taiwuChar.GetValidLocation();
			}
			List<Predicate<GameData.Domains.Character.Character>> list = ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Get();
			List<GameData.Domains.Character.Character> list2 = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			list.Clear();
			list2.Clear();
			GameData.Domains.Character.Filters.CharacterFilterRules.ToPredicates(13, list, -1);
			MapCharacterFilter.Find(list, list2, location.AreaId);
			Logger.Info((list.Count > 0) ? "found male" : "created male");
			GameData.Domains.Character.Character character = ((list2.Count > 0) ? list2.GetRandom(context.Random) : DomainManager.Character.CreateTemporaryIntelligentCharacter(context, 13, location));
			list.Clear();
			list2.Clear();
			GameData.Domains.Character.Filters.CharacterFilterRules.ToPredicates(14, list, -1);
			MapCharacterFilter.Find(list, list2, location.AreaId);
			Logger.Info((list.Count > 0) ? "found female" : "created female");
			GameData.Domains.Character.Character character2 = ((list2.Count > 0) ? list2.GetRandom(context.Random) : DomainManager.Character.CreateTemporaryIntelligentCharacter(context, 14, location));
			ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Return(list);
			ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list2);
			_successorCandidates.Add(character.GetId());
			_successorCandidates.Add(character2.GetId());
			break;
		}
		default:
			throw new Exception($"Trying to find successors in a invalid state {_legacyPassingState}.");
		}
		SetSuccessorCandidates(_successorCandidates, context);
		return _successorCandidates;
	}

	[DomainMethod]
	public void ConfirmChosenSuccessor(DataContext context, int newTaiwuId)
	{
		bool randomSuccessor = _legacyPassingState == 2;
		ConvertAndClearSuccessorsCandidates(context, newTaiwuId);
		SetLegacyPassingState(context, 4);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(newTaiwuId);
		(short, short) tuple = (element_Objects.GetHealth(), element_Objects.GetLeftMaxHealth());
		DomainManager.Character.RevertAllTemporaryModifications(context, _taiwuChar);
		_taiwuChar.RemoveFeatureGroup(context, 756);
		TransferTaiwuData(context, element_Objects, _taiwuChar, randomSuccessor);
		SetTaiwu(context, element_Objects);
		UpdateConsummateLevelBrokenFeature(context);
		ApplyAndResetLegacy(context, element_Objects, isClearLegaciesBuilding: false);
		short leftMaxHealth = element_Objects.GetLeftMaxHealth();
		short health = (short)Math.Clamp(element_Objects.GetLeftMaxHealth() * tuple.Item1 / Math.Max(1, (int)tuple.Item2), 0, leftMaxHealth);
		element_Objects.SetHealth(health, context);
		Location location = element_Objects.GetLocation();
		DomainManager.Map.SetBlockAndViewRangeVisible(context, location.AreaId, location.BlockId);
		_isTaiwuDying = false;
		DomainManager.TaiwuEvent.TriggerListener("PassingLegacyComplete", value: true);
	}

	public void ConvertAndClearSuccessorsCandidates(DataContext context, int newTaiwuId)
	{
		foreach (int successorCandidate in _successorCandidates)
		{
			if (DomainManager.Character.IsTemporaryIntelligentCharacter(successorCandidate))
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(successorCandidate);
				if (successorCandidate == newTaiwuId)
				{
					DomainManager.Character.ConvertTemporaryIntelligentCharacter(context, element_Objects);
				}
				else
				{
					DomainManager.Character.RemoveTemporaryIntelligentCharacter(context, element_Objects);
				}
			}
		}
		_successorCandidates.Clear();
	}

	public void CheckNotTaiwu(int charId)
	{
		Tester.Assert(charId != _taiwuCharId || _legacyPassingState != 0);
	}

	public void TransferTaiwuData(DataContext context, GameData.Domains.Character.Character newTaiwuChar, GameData.Domains.Character.Character oldTaiwuChar, bool randomSuccessor = false)
	{
		int newTaiwuCharId = newTaiwuChar.GetId();
		int id = oldTaiwuChar.GetId();
		DomainManager.Character.PrepareTaiwuInheritor(context, newTaiwuChar, oldTaiwuChar);
		SetTaiwuGenerationsCount(_taiwuGenerationsCount + 1, context);
		DomainManager.Taiwu.TaiwuUnfollowNpc(context, newTaiwuCharId);
		List<short> learnedCombatSkills = newTaiwuChar.GetLearnedCombatSkills();
		List<GameData.Domains.CombatSkill.CombatSkill> list = ObjectPool<List<GameData.Domains.CombatSkill.CombatSkill>>.Instance.Get();
		list.Clear();
		foreach (short learnedCombatSkill in oldTaiwuChar.GetLearnedCombatSkills())
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(id, learnedCombatSkill));
			GameData.Domains.CombatSkill.CombatSkill combatSkill = GameData.Serializer.Serializer.CreateCopy(element_CombatSkills);
			combatSkill.OfflineSetCharId(newTaiwuCharId);
			list.Add(combatSkill);
			learnedCombatSkills.Add(learnedCombatSkill);
		}
		DomainManager.CombatSkill.RegisterCombatSkills(newTaiwuCharId, list);
		DomainManager.Character.AutoActivateReadCombatSkillNormalPages(context, list, newTaiwuCharId);
		ObjectPool<List<GameData.Domains.CombatSkill.CombatSkill>>.Instance.Return(list);
		newTaiwuChar.SetLearnedCombatSkills(learnedCombatSkills, context);
		newTaiwuChar.CopyCombatSkillEquipmentFrom(context, oldTaiwuChar);
		DomainManager.SpecialEffect.AddAllBrokenSkillEffects(context, newTaiwuChar);
		newTaiwuChar.SetLoopingNeigong(oldTaiwuChar.GetLoopingNeigong(), context);
		newTaiwuChar.SetCombatSkillAttainmentPanels(oldTaiwuChar.GetCombatSkillAttainmentPanels().ToArray(), context);
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = newTaiwuChar.GetLearnedLifeSkills();
		learnedLifeSkills.Clear();
		learnedLifeSkills.AddRange(oldTaiwuChar.GetLearnedLifeSkills());
		newTaiwuChar.SetLearnedLifeSkills(learnedLifeSkills, context);
		GameData.Utilities.ShortList characterMasteredCombatSkills = DomainManager.Extra.GetCharacterMasteredCombatSkills(id);
		DomainManager.Extra.ClearCharacterMasteredCombatSkills(context, id);
		DomainManager.Extra.SetCharacterMasteredCombatSkills(context, newTaiwuCharId, characterMasteredCombatSkills);
		DomainManager.Extra.TransferTaiwuProficiency(context, id, newTaiwuCharId);
		ItemKey[] array = new ItemKey[12];
		for (int i = 0; i < 12; i++)
		{
			array[i] = ItemKey.Invalid;
		}
		ItemKey[] array2 = oldTaiwuChar.GetEquipment().ToArray();
		ItemKey itemKey = array2[4];
		if (itemKey.IsValid() && !(Config.Clothing.Instance[itemKey.TemplateId]?.KeepOnPassing ?? false))
		{
			array[4] = itemKey;
		}
		oldTaiwuChar.ChangeEquipment(context, array);
		List<sbyte> charOwnedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(id);
		if (charOwnedBookTypes != null)
		{
			for (int num = charOwnedBookTypes.Count - 1; num >= 0; num--)
			{
				sbyte combatSkillType = charOwnedBookTypes[num];
				ItemKey legendaryBookItem = DomainManager.LegendaryBook.GetLegendaryBookItem(combatSkillType);
				oldTaiwuChar.RemoveInventoryItem(context, legendaryBookItem, 1, deleteItem: false);
				_taiwuCharId = newTaiwuCharId;
				newTaiwuChar.AddInventoryItem(context, legendaryBookItem, 1);
				_taiwuCharId = id;
			}
		}
		Dictionary<ItemKey, int> items = oldTaiwuChar.GetInventory().Items;
		foreach (KeyValuePair<ItemKey, int> item in items)
		{
			newTaiwuChar.AddInventoryItem(context, item.Key, item.Value);
		}
		oldTaiwuChar.SetInventory(new Inventory(), context);
		ResetEquipmentPlans(context);
		ResourceInts delta = oldTaiwuChar.GetResources();
		newTaiwuChar.ChangeResources(context, ref delta);
		delta.Initialize();
		oldTaiwuChar.SetResources(ref delta, context);
		newTaiwuChar.ChangeExp(context, oldTaiwuChar.GetExp());
		oldTaiwuChar.SetExp(0, context);
		DomainManager.Information.TransferInformation(id, newTaiwuCharId, context);
		if (randomSuccessor)
		{
			HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
			hashSet.Clear();
			hashSet.UnionWith(_groupCharIds.GetCollection());
			_groupCharIds.Remove(id);
			foreach (int item2 in _groupCharIds.GetCollection())
			{
				if (DomainManager.Character.GetElement_Objects(item2).GetAgeGroup() != 0)
				{
					hashSet.Add(item2);
				}
			}
			foreach (int item3 in hashSet)
			{
				if (item3 != id && _groupCharIds.Contains(item3))
				{
					LeaveGroup(context, item3, bringWards: true, showNotification: false);
				}
			}
			_groupCharIds.Add(id);
			ObjectPool<HashSet<int>>.Instance.Get();
			foreach (int item4 in _groupCharIds.GetCollection())
			{
				if (item4 != id)
				{
					LeaveGroup(context, item4, bringWards: true, showNotification: false);
				}
			}
			JoinGroup(context, newTaiwuCharId, showNotification: false);
			oldTaiwuChar.SetLeaderId(newTaiwuCharId, context);
			newTaiwuChar.SetLeaderId(newTaiwuCharId, context);
		}
		else
		{
			foreach (int item5 in _groupCharIds.GetCollection())
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item5);
				element_Objects.SetLeaderId(newTaiwuCharId, context);
			}
			DomainManager.Character.TransferKidnappedCharactersToTaiwuSuccessor(context, newTaiwuChar, oldTaiwuChar);
		}
		DomainManager.Extra.TransferSpecialGroup(context, id, newTaiwuCharId, clearDestGroupFirst: true);
		if (randomSuccessor)
		{
			newTaiwuChar.SetHappiness(50, context);
			List<FameActionRecord> fameActionRecords = newTaiwuChar.GetFameActionRecords();
			fameActionRecords.Clear();
			newTaiwuChar.SetFameActionRecords(fameActionRecords, context);
		}
		sbyte fame = oldTaiwuChar.GetFame();
		sbyte fameType = FameType.GetFameType(fame);
		if (fameType >= 5)
		{
			newTaiwuChar.RecordFameAction(context, 54, -1, 1);
		}
		else if (fameType <= 1)
		{
			newTaiwuChar.RecordFameAction(context, 55, -1, 1);
		}
		DomainManager.Organization.ResetSectExploreStatuses(context);
		DomainManager.TaiwuEvent.ClearTaiwuBindingMonthlyActions(context);
		DomainManager.World.GetMonthlyEventCollection().Clear();
		DomainManager.World.TransferXiangshuAvatarRelations(context, id, newTaiwuCharId);
		DomainManager.Extra.TransferRanshanThreeCorpsesRelations(context, id, newTaiwuCharId);
		DomainManager.Character.TransferFixedCharacterFavorability(context, id, newTaiwuCharId);
		DomainManager.Organization.ResetSectExploreStatuses(context);
		DomainManager.Extra.RemoveAllTaiwuExtraBonuses(context);
		DomainManager.Building.SetShrineBuyTimes(0, context);
		DomainManager.Extra.ResetInteractOfLoveData(context);
		ProfessionSkillHandle.OnTaiwuDeath(context);
		ClearPrenatalEducationBonus(context);
		DomainManager.Extra.TaiwuWantedResetAllSectInteracted(context);
		DomainManager.Extra.ClearAllTaiwuInteractionCooldowns(context);
		DomainManager.Extra.ClearFriendOrFamilyInteractionCooldowns(context);
		DomainManager.Extra.TransferTaiwuArtisanOrder(context, newTaiwuCharId, id);
		DomainManager.Extra.ClearAllGearMateNeili(context);
		newTaiwuChar.SetCurrNeili(oldTaiwuChar.GetCurrNeili(), context);
		newTaiwuChar.SetExtraNeili(oldTaiwuChar.GetExtraNeili(), context);
		newTaiwuChar.SetBaseNeiliAllocation(oldTaiwuChar.GetBaseNeiliAllocation(), context);
		newTaiwuChar.SetExtraNeiliAllocation(oldTaiwuChar.GetExtraNeiliAllocation(), context);
		DomainManager.Extra.TransferExtraNeiliAllocationProgress(context, oldTaiwuChar, newTaiwuChar);
		newTaiwuChar.SetConsummateLevel(oldTaiwuChar.GetConsummateLevel(), context);
		byte poisonImmunities = DomainManager.Extra.GetPoisonImmunities(id);
		if (poisonImmunities != 0)
		{
			ref PoisonInts poisoned = ref newTaiwuChar.GetPoisoned();
			for (sbyte b = 0; b < 6; b++)
			{
				if (BitOperation.GetBit(poisonImmunities, (int)b))
				{
					poisoned[b] = 0;
				}
			}
			newTaiwuChar.SetPoisoned(ref poisoned, context);
			DomainManager.Extra.AddPoisonImmunities(context, newTaiwuCharId, poisonImmunities);
			DomainManager.Extra.RemovePoisonImmunities(context, id);
		}
		List<short> featureIds = oldTaiwuChar.GetFeatureIds();
		for (short num2 = 0; num2 < CharacterFeature.Instance.Count; num2++)
		{
			CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[num2];
			if (characterFeatureItem.InheritableTransferTaiwu && featureIds.Contains(num2))
			{
				newTaiwuChar.AddFeature(context, num2);
				oldTaiwuChar.RemoveFeature(context, num2);
			}
		}
		OrganizationInfo organizationInfo = oldTaiwuChar.GetOrganizationInfo();
		if (_isTaiwuDying)
		{
			LeaveGroup(context, id, bringWards: false, showNotification: false);
			short deathType = (short)((oldTaiwuChar.GetLeftMaxHealth() <= 0) ? 1 : 2);
			DomainManager.Character.MakeCharacterDead(context, oldTaiwuChar, deathType);
		}
		else
		{
			LeaveGroup(context, id, bringWards: false);
			Events.RaiseXiangshuInfectionFeatureChanged(context, oldTaiwuChar, 218);
		}
		DomainManager.Organization.ChangeGrade(context, newTaiwuChar, organizationInfo.Grade, organizationInfo.Principal);
		newTaiwuChar.RemoveFeatureGroup(context, 734);
		_villagerFeatureHolderSet.Remove(newTaiwuCharId);
		VillagerRoleRecords villagerRoleRecordsData = DomainManager.Extra.GetVillagerRoleRecordsData();
		if (villagerRoleRecordsData.History.RemoveAll((VillagerRoleRecordElement rc) => rc.CharacterId == newTaiwuCharId) > 0)
		{
			DomainManager.Extra.SetVillagerRoleRecordsData(context, villagerRoleRecordsData);
		}
	}

	public void ApplyAndResetLegacy(DataContext context, GameData.Domains.Character.Character taiwuChar, bool isClearLegaciesBuilding = true)
	{
		if (isClearLegaciesBuilding)
		{
			ClearLegaciesBuildingTemplateIdList(context);
		}
		foreach (short selectedLegacy in _selectedLegacies)
		{
			ApplyLegacyAffect(context, taiwuChar, selectedLegacy);
		}
		ResetLegacy(context);
	}

	private void ResetLegacy(DataContext context)
	{
		_selectedLegacies.Clear();
		ClearLegacyPointDict(context);
		_availableLegacyList.Clear();
		SetAvailableLegacyList(_availableLegacyList, context);
		SetLegacyPoint(0, context);
	}

	private void ClearLegaciesBuildingTemplateIdList(DataContext context)
	{
		List<short> legaciesBuildingTemplateIdList = DomainManager.Extra.GetLegaciesBuildingTemplateIdList();
		legaciesBuildingTemplateIdList.Clear();
		DomainManager.Extra.SetLegaciesBuildingTemplateIdList(legaciesBuildingTemplateIdList, context);
	}

	private void ApplyLegacyAffect(DataContext context, GameData.Domains.Character.Character newTaiwuChar, short legacyTemplateId)
	{
		LegacyItem legacyItem = Legacy.Instance[legacyTemplateId];
		if (legacyItem.AddFeature >= 0)
		{
			newTaiwuChar.AddFeature(context, legacyItem.AddFeature, removeMutexFeature: true);
		}
		if (legacyItem.ModifiedProperty >= 0)
		{
			DomainManager.Extra.AddTaiwuPropertyPermanentBonus(context, (ECharacterPropertyReferencedType)legacyItem.ModifiedProperty, (EDataModifyType)legacyItem.PropertyBonusType, legacyItem.PropertyDelta);
		}
		if (legacyItem.TargetBehaviorType >= 0)
		{
			(short, short) tuple = GameData.Domains.Character.BehaviorType.Ranges[legacyItem.TargetBehaviorType];
			int num = context.Random.Next((int)tuple.Item1, tuple.Item2 + 1);
			newTaiwuChar.SetBaseMorality((short)num, context);
		}
		if (legacyItem.AddBuildingBlock >= 0)
		{
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[legacyItem.AddBuildingBlock];
			if (BuildingBlockData.IsUsefulResource(buildingBlockItem.Type))
			{
				List<short> legaciesBuildingTemplateIdList = DomainManager.Extra.GetLegaciesBuildingTemplateIdList();
				legaciesBuildingTemplateIdList.Add(legacyItem.AddBuildingBlock);
				DomainManager.Extra.SetLegaciesBuildingTemplateIdList(legaciesBuildingTemplateIdList, context);
			}
			else
			{
				Location taiwuVillageLocation = GetTaiwuVillageLocation();
				DomainManager.Building.PlaceBuildingAtBlock(context, taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, legacyItem.AddBuildingBlock, forcePlace: false, isRandom: true);
			}
		}
		if (legacyItem.AddBuildingCoreItem >= 0)
		{
			ItemKey itemKey = DomainManager.Item.CreateItem(context, 12, legacyItem.AddBuildingCoreItem);
			DomainManager.Taiwu.WarehouseAdd(context, itemKey, 1);
		}
		if (legacyItem.SpiritualDebtDelta != 0)
		{
			List<short> list = ObjectPool<List<short>>.Instance.Get();
			list.Clear();
			sbyte stateId = (sbyte)(legacyItem.AffectingState - 1);
			DomainManager.Map.GetAllAreaInState(stateId, list);
			int delta = 10 * legacyItem.SpiritualDebtDelta;
			foreach (short item in list)
			{
				DomainManager.Extra.ChangeAreaSpiritualDebt(context, item, delta);
			}
		}
		sbyte[] stateNewbornChildrenGrowingGrade = legacyItem.StateNewbornChildrenGrowingGrade;
		if (stateNewbornChildrenGrowingGrade != null && stateNewbornChildrenGrowingGrade.Length != 0)
		{
			sbyte stateId2 = (sbyte)(legacyItem.AffectingState - 1);
			PushStateNewCharacterLegacyGrowingGrades(context, stateId2, legacyItem.StateNewbornChildrenGrowingGrade);
		}
		if (legacyItem.AffectingOrganization >= 0)
		{
			sbyte[] supportingSectCharacterGrades = legacyItem.SupportingSectCharacterGrades;
			foreach (sbyte grade in supportingSectCharacterGrades)
			{
				DomainManager.Organization.SelectRandomSectCharacterToApproveTaiwu(context, legacyItem.AffectingOrganization, grade);
			}
		}
		if (legacyItem.TargetNeiliProportionOfFiveElements.SumCheck() == 100)
		{
			newTaiwuChar.SetBaseNeiliProportionOfFiveElements(legacyItem.TargetNeiliProportionOfFiveElements, context);
		}
		if (legacyItem.PoisonImmunityCount > 0)
		{
			Span<sbyte> pArray = stackalloc sbyte[6];
			for (sbyte b = 0; b < 6; b++)
			{
				pArray[b] = b;
			}
			CollectionUtils.Shuffle(context.Random, pArray, 6);
			ref PoisonInts poisoned = ref newTaiwuChar.GetPoisoned();
			byte b2 = 0;
			for (int j = 0; j < legacyItem.PoisonImmunityCount; j++)
			{
				sbyte b3 = pArray[j];
				b2 = BitOperation.SetBit(b2, (int)b3, true);
				poisoned[j] = 0;
			}
			newTaiwuChar.SetPoisoned(ref poisoned, context);
			DomainManager.Extra.AddPoisonImmunities(context, newTaiwuChar.GetId(), b2);
		}
		if (legacyItem.TargetQualificationGrowthTypeLifeSkill >= 0)
		{
			newTaiwuChar.SetLifeSkillQualificationGrowthType(legacyItem.TargetQualificationGrowthTypeLifeSkill, context);
		}
		if (legacyItem.TargetQualificationGrowthTypeCombatSkill >= 0)
		{
			newTaiwuChar.SetCombatSkillQualificationGrowthType(legacyItem.TargetQualificationGrowthTypeCombatSkill, context);
		}
	}

	public void SetNextLifeSkillCombatForceOperationForbid(bool isForceSilentForbid, bool isForceGiveupForbid)
	{
		_nextLifeSkillCombatForceOperationForbid[OperationPrepareForceAdversary.ForceAdversaryOperation.Silent] = isForceSilentForbid;
		_nextLifeSkillCombatForceOperationForbid[OperationPrepareForceAdversary.ForceAdversaryOperation.GiveUp] = isForceGiveupForbid;
	}

	[DomainMethod]
	public unsafe byte[] LifeSkillCombatStart(DataContext context, sbyte lifeSkillType, int charA, int charB, sbyte firstTurnPlayerId)
	{
		Tester.Assert(_lifeSkillCombat == null);
		_lifeSkillCombat = new Match(context, lifeSkillType, charA, charB, firstTurnPlayerId);
		if (_nextLifeSkillCombatForceOperationForbid.TryGetValue(OperationPrepareForceAdversary.ForceAdversaryOperation.Silent, out var value) && value)
		{
			_lifeSkillCombat.GetPlayer(0).SetForceSilentRemainingCount(0);
			_lifeSkillCombat.GetPlayer(1).SetForceSilentRemainingCount(0);
		}
		if (_nextLifeSkillCombatForceOperationForbid.TryGetValue(OperationPrepareForceAdversary.ForceAdversaryOperation.GiveUp, out value) && value)
		{
			_lifeSkillCombat.GetPlayer(0).SetForceGiveUpRemainingCount(0);
			_lifeSkillCombat.GetPlayer(1).SetForceGiveUpRemainingCount(0);
		}
		_nextLifeSkillCombatForceOperationForbid.Clear();
		StatusSnapshotDiff statusSnapshotDiff = new StatusSnapshotDiff(_lifeSkillCombat.Dump());
		byte[] array = new byte[statusSnapshotDiff.GetSerializedSize()];
		fixed (byte* pData = array)
		{
			statusSnapshotDiff.Serialize(pData);
		}
		return array;
	}

	[DomainMethod]
	public CombatResultDisplayData LifeSkillCombatTerminate(DataContext context)
	{
		Tester.Assert(_lifeSkillCombat != null);
		_lifeSkillCombat.CheckResult(out var winnerPlayerId);
		int playerCharacterId = _lifeSkillCombat.GetPlayerCharacterId(1);
		bool flag = winnerPlayerId == 0;
		CombatResultDisplayData result = ApplyLifeSkillCombatResult(context, playerCharacterId, flag);
		if (flag)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(playerCharacterId);
			short lifeSkillAttainment = element_Objects.GetLifeSkillAttainment(_lifeSkillCombat.LifeSkillType);
			short lifeSkillAttainment2 = _taiwuChar.GetLifeSkillAttainment(_lifeSkillCombat.LifeSkillType);
			if (lifeSkillAttainment >= lifeSkillAttainment2)
			{
				DomainManager.Taiwu.AddLegacyPoint(context, 18);
			}
			switch (_lifeSkillCombat.LifeSkillType)
			{
			case 13:
			{
				ProfessionFormulaItem formulaCfg3 = ProfessionFormula.Instance[81];
				int baseDelta3 = formulaCfg3.Calculate(lifeSkillAttainment);
				DomainManager.Extra.ChangeProfessionSeniority(context, 12, baseDelta3);
				break;
			}
			case 12:
			{
				ProfessionFormulaItem formulaCfg2 = ProfessionFormula.Instance[94];
				int baseDelta2 = formulaCfg2.Calculate(lifeSkillAttainment);
				DomainManager.Extra.ChangeProfessionSeniority(context, 14, baseDelta2);
				break;
			}
			default:
			{
				ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[30];
				int baseDelta = formulaCfg.Calculate(lifeSkillAttainment);
				DomainManager.Extra.ChangeProfessionSeniority(context, 4, baseDelta);
				break;
			}
			}
		}
		_lifeSkillCombat.GenerateRecordFile();
		InformationDomain information = DomainManager.Information;
		foreach (KeyValuePair<sbyte, sbyte> item in _lifeSkillCombat.PlayerForcedSecretInformation)
		{
			int characterId = _lifeSkillCombat.GetPlayer(item.Key).CharacterId;
			int characterId2 = _lifeSkillCombat.GetPlayer(Player.PredefinedId.GetTheOtherSide(item.Key)).CharacterId;
			int dataOffset = information.GetSecretInformationCollection().AddForcingSilence(characterId, characterId2);
			information.AddSecretInformationMetaDataWithNecessity(context, dataOffset, withInitialDistribute: true, necessarily: true, null);
		}
		_lifeSkillCombat = null;
		return result;
	}

	[DomainMethod]
	public unsafe byte[] LifeSkillCombatCurrentPlayerGetUsableOperationList(DataContext context, byte[] inputData)
	{
		List<sbyte> list = new List<sbyte>();
		List<sbyte> list2 = new List<sbyte>();
		List<int> list3 = new List<int>();
		fixed (byte* ptr = inputData)
		{
			byte* ptr2 = ptr;
			ushort num = *(ushort*)ptr2;
			ptr2 += 2;
			for (int i = 0; i < num; i++)
			{
				list.Add((sbyte)(*ptr2));
				ptr2++;
			}
			num = *(ushort*)ptr2;
			ptr2 += 2;
			for (int j = 0; j < num; j++)
			{
				list2.Add((sbyte)(*ptr2));
				ptr2++;
			}
			num = *(ushort*)ptr2;
			ptr2 += 2;
			for (int k = 0; k < num; k++)
			{
				list3.Add(*(int*)ptr2);
				ptr2 += 4;
			}
		}
		bool maybeHasAdditionalOperation;
		OperationList operationList = _lifeSkillCombat.CalcUsableOperationList(list, list2, list3, out maybeHasAdditionalOperation);
		byte[] array = new byte[operationList.GetSerializedSize() + 1];
		fixed (byte* ptr3 = array)
		{
			byte* ptr4 = ptr3;
			ptr4 += operationList.Serialize(ptr4);
			*ptr4 = (maybeHasAdditionalOperation ? ((byte)1) : ((byte)0));
			ptr4++;
		}
		return array;
	}

	[DomainMethod]
	public unsafe byte[] LifeSkillCombatCurrentPlayerGetSecondPhaseUsableOperationList(DataContext context, byte[] inputData)
	{
		OperationUseEffectCard operationUseEffectCard = new OperationUseEffectCard();
		List<sbyte> list = new List<sbyte>();
		List<sbyte> list2 = new List<sbyte>();
		List<int> list3 = new List<int>();
		fixed (byte* ptr = inputData)
		{
			byte* ptr2 = ptr;
			ptr2 += operationUseEffectCard.Deserialize(ptr2);
			ushort num = *(ushort*)ptr2;
			ptr2 += 2;
			for (int i = 0; i < num; i++)
			{
				list.Add((sbyte)(*ptr2));
				ptr2++;
			}
			num = *(ushort*)ptr2;
			ptr2 += 2;
			for (int j = 0; j < num; j++)
			{
				list2.Add((sbyte)(*ptr2));
				ptr2++;
			}
			num = *(ushort*)ptr2;
			ptr2 += 2;
			for (int k = 0; k < num; k++)
			{
				list3.Add(*(int*)ptr2);
				ptr2 += 4;
			}
		}
		OperationList operationList = _lifeSkillCombat.CalcUsableSecondPhaseOperationList(operationUseEffectCard, list, list2, list3);
		byte[] array = new byte[operationList.GetSerializedSize() + 1];
		fixed (byte* ptr3 = array)
		{
			byte* ptr4 = ptr3;
			ptr4 += operationList.Serialize(ptr4);
		}
		return array;
	}

	[DomainMethod]
	public unsafe byte[] LifeSkillCombatCurrentPlayerCalcUsableFirstPhaseEffectCardInfo(DataContext context, sbyte effectCardTemplateId)
	{
		_lifeSkillCombat.CalcUsableFirstPhaseEffectCardInfo(effectCardTemplateId, out var gridIndices, out var bookIndices);
		byte[] array = new byte[2 + 4 * gridIndices.Count + 2 + bookIndices.Count];
		fixed (byte* ptr = array)
		{
			byte* ptr2 = ptr;
			*(ushort*)ptr2 = (ushort)gridIndices.Count;
			ptr2 += 2;
			foreach (int item in gridIndices)
			{
				*(int*)ptr2 = item;
				ptr2 += 4;
			}
			*(ushort*)ptr2 = (ushort)bookIndices.Count;
			ptr2 += 2;
			foreach (sbyte item2 in bookIndices)
			{
				*ptr2 = (byte)item2;
				ptr2++;
			}
		}
		return array;
	}

	[DomainMethod]
	public unsafe byte[] LifeSkillCombatCurrentPlayerAiPickOperation(DataContext context, byte[] operationListData)
	{
		if (_lifeSkillCombat == null)
		{
			return null;
		}
		OperationList operationList = new OperationList();
		fixed (byte* pData = operationListData)
		{
			operationList.Deserialize(pData);
		}
		OperationBase operation = _lifeSkillCombat.CalcAiOperation(context, operationList);
		byte[] array = new byte[OperationBase.GetSerializeSizeWithPolymorphism(operation)];
		fixed (byte* pData2 = array)
		{
			OperationBase.SerializeWithPolymorphism(operation, pData2);
		}
		return array;
	}

	[DomainMethod]
	public unsafe byte[] LifeSkillCombatCurrentPlayerCommitOperation(DataContext context, byte[] data)
	{
		OperationBase operation = null;
		fixed (byte* pData = data)
		{
			OperationBase.DeserializeWithPolymorphism(ref operation, pData);
		}
		if (_lifeSkillCombat == null)
		{
			throw new Exception($"commit operation {operation} when lifeSkillCombat is inactive");
		}
		StatusSnapshot previous = _lifeSkillCombat.Dump();
		List<StatusSnapshotDiff.BookStateExtraDiff> list = new List<StatusSnapshotDiff.BookStateExtraDiff>();
		List<StatusSnapshotDiff.GridTrapStateExtraDiff> list2 = new List<StatusSnapshotDiff.GridTrapStateExtraDiff>();
		_lifeSkillCombat.CommitOperation(context, operation, list, list2);
		StatusSnapshotDiff statusSnapshotDiff = new StatusSnapshotDiff(in previous, _lifeSkillCombat.Dump());
		statusSnapshotDiff.BookStateExtraDiffList.AddRange(list);
		statusSnapshotDiff.GridTrapStateExtraDiffList.AddRange(list2);
		_lifeSkillCombat.RecordDiff(statusSnapshotDiff);
		byte[] array = new byte[statusSnapshotDiff.GetSerializedSize()];
		fixed (byte* pData2 = array)
		{
			statusSnapshotDiff.Serialize(pData2);
		}
		return array;
	}

	[DomainMethod]
	public unsafe byte[] LifeSkillCombatCurrentPlayerSimulateCommitOperation(DataContext context, byte[] data)
	{
		StatusSnapshot previous = _lifeSkillCombat.Dump();
		OperationBase operation = null;
		fixed (byte* pData = data)
		{
			OperationBase.DeserializeWithPolymorphism(ref operation, pData);
		}
		List<StatusSnapshotDiff.BookStateExtraDiff> list = new List<StatusSnapshotDiff.BookStateExtraDiff>();
		List<StatusSnapshotDiff.GridTrapStateExtraDiff> list2 = new List<StatusSnapshotDiff.GridTrapStateExtraDiff>();
		_lifeSkillCombat.SimulateOperationCommit(context, operation, list, list2);
		StatusSnapshotDiff statusSnapshotDiff = new StatusSnapshotDiff(in previous, _lifeSkillCombat.Dump());
		statusSnapshotDiff.BookStateExtraDiffList.AddRange(list);
		statusSnapshotDiff.GridTrapStateExtraDiffList.AddRange(list2);
		byte[] array = new byte[statusSnapshotDiff.GetSerializedSize()];
		fixed (byte* pData2 = array)
		{
			statusSnapshotDiff.Serialize(pData2);
		}
		return array;
	}

	[DomainMethod]
	public unsafe byte[] LifeSkillCombatCurrentPlayerSimulateCancelOperation(DataContext context)
	{
		StatusSnapshot previous = _lifeSkillCombat.Dump();
		_lifeSkillCombat.SimulateOperationCancel(context);
		StatusSnapshotDiff statusSnapshotDiff = new StatusSnapshotDiff(in previous, _lifeSkillCombat.Dump());
		byte[] array = new byte[statusSnapshotDiff.GetSerializedSize()];
		fixed (byte* pData = array)
		{
			statusSnapshotDiff.Serialize(pData);
		}
		return array;
	}

	[DomainMethod]
	public unsafe byte[] LifeSkillCombatCurrentPlayerCommitOperationPreview(DataContext context, byte[] data)
	{
		OperationBase operation = null;
		fixed (byte* pData = data)
		{
			OperationBase.DeserializeWithPolymorphism(ref operation, pData);
		}
		_lifeSkillCombat.RecordLine("-- Incoming preview --");
		StatusSnapshot previous = _lifeSkillCombat.Dump();
		List<int> list = new List<int>();
		List<StatusSnapshotDiff.BookStateExtraDiff> list2 = new List<StatusSnapshotDiff.BookStateExtraDiff>();
		List<StatusSnapshotDiff.GridTrapStateExtraDiff> list3 = new List<StatusSnapshotDiff.GridTrapStateExtraDiff>();
		if (operation.PlayerId == _lifeSkillCombat.CurrentPlayerId)
		{
			_lifeSkillCombat.CommitOperationProcess(context, operation, list2, list3, withProcessActiveOperationCells: false);
			_lifeSkillCombat.SwitchCurrentPlayerProcess(context, list2, list3);
		}
		StatusSnapshotDiff statusSnapshotDiff = new StatusSnapshotDiff(in previous, _lifeSkillCombat.Dump());
		statusSnapshotDiff.BookStateExtraDiffList.AddRange(list2);
		statusSnapshotDiff.GridTrapStateExtraDiffList.AddRange(list3);
		_lifeSkillCombat.Restore(previous);
		List<StatusSnapshotDiff.BookStateExtraDiff> list4 = new List<StatusSnapshotDiff.BookStateExtraDiff>();
		List<StatusSnapshotDiff.GridTrapStateExtraDiff> list5 = new List<StatusSnapshotDiff.GridTrapStateExtraDiff>();
		if (operation.PlayerId == _lifeSkillCombat.CurrentPlayerId)
		{
			_lifeSkillCombat.CommitOperationProcess(context, new OperationSilent(operation.PlayerId, _lifeSkillCombat.PlayerSwitchCount), list4, list5, withProcessActiveOperationCells: false);
			_lifeSkillCombat.SwitchCurrentPlayerProcess(context, list4, list5);
		}
		StatusSnapshotDiff statusSnapshotDiff2 = new StatusSnapshotDiff(in previous, _lifeSkillCombat.Dump());
		statusSnapshotDiff2.BookStateExtraDiffList.AddRange(list4);
		statusSnapshotDiff2.GridTrapStateExtraDiffList.AddRange(list5);
		_lifeSkillCombat.Restore(previous);
		Dictionary<int, Grid> dictionary = new Dictionary<int, Grid>();
		Dictionary<int, Grid> dictionary2 = new Dictionary<int, Grid>();
		foreach (Grid item in statusSnapshotDiff.GridStatusDiffCurrent)
		{
			dictionary.Add(item.Index, item);
		}
		foreach (Grid item2 in statusSnapshotDiff2.GridStatusDiffCurrent)
		{
			dictionary2.Add(item2.Index, item2);
		}
		if (operation is OperationGridBase operationGridBase)
		{
			dictionary.Remove(operationGridBase.GridIndex);
		}
		foreach (KeyValuePair<int, Grid> item3 in dictionary)
		{
			if (dictionary2.TryGetValue(item3.Key, out var value))
			{
				Grid value2 = item3.Value;
				OperationPointBase operationPointBase = value2.LastHistoryOperation as OperationPointBase;
				OperationPointBase operationPointBase2 = value.LastHistoryOperation as OperationPointBase;
				int num = operationPointBase?.Point ?? (-1);
				int num2 = operationPointBase2?.Point ?? (-1);
				sbyte b = operationPointBase?.PlayerId ?? (-1);
				sbyte b2 = operationPointBase2?.PlayerId ?? (-1);
				if (num != num2 || b != b2)
				{
					list.Add(item3.Key);
				}
			}
			else
			{
				list.Add(item3.Key);
			}
		}
		statusSnapshotDiff.GridTrapStateExtraDiffList.Clear();
		_lifeSkillCombat.RecordLine("-- Ending preview --");
		byte[] array = new byte[statusSnapshotDiff.GetSerializedSize() + 2 + list.Count * 4];
		fixed (byte* ptr = array)
		{
			byte* ptr2 = ptr;
			ptr2 += statusSnapshotDiff.Serialize(ptr2);
			*(ushort*)ptr2 = (ushort)list.Count;
			ptr2 += 2;
			for (int i = 0; i < list.Count; i++)
			{
				*(int*)ptr2 = list[i];
				ptr2 += 4;
			}
		}
		return array;
	}

	[DomainMethod]
	public unsafe byte[] LifeSkillCombatCurrentPlayerAcceptForceSilent(DataContext context)
	{
		StatusSnapshot previous = _lifeSkillCombat.Dump();
		OperationBase operation = new OperationSilent(_lifeSkillCombat.CurrentPlayerId, _lifeSkillCombat.PlayerSwitchCount);
		List<StatusSnapshotDiff.BookStateExtraDiff> list = new List<StatusSnapshotDiff.BookStateExtraDiff>();
		List<StatusSnapshotDiff.GridTrapStateExtraDiff> list2 = new List<StatusSnapshotDiff.GridTrapStateExtraDiff>();
		_lifeSkillCombat.CommitOperation(context, operation, list, list2);
		StatusSnapshotDiff statusSnapshotDiff = new StatusSnapshotDiff(in previous, _lifeSkillCombat.Dump());
		statusSnapshotDiff.BookStateExtraDiffList.AddRange(list);
		statusSnapshotDiff.GridTrapStateExtraDiffList.AddRange(list2);
		_lifeSkillCombat.RecordDiff(statusSnapshotDiff);
		byte[] array = new byte[statusSnapshotDiff.GetSerializedSize()];
		fixed (byte* pData = array)
		{
			statusSnapshotDiff.Serialize(pData);
		}
		return array;
	}

	[DomainMethod]
	public unsafe List<sbyte> LifeSkillCombatCurrentPlayerGetNotUsableEffectCardTemplateIds(DataContext context, byte[] inputData)
	{
		List<sbyte> list = new List<sbyte>();
		List<sbyte> list2 = new List<sbyte>();
		List<int> list3 = new List<int>();
		fixed (byte* ptr = inputData)
		{
			byte* ptr2 = ptr;
			ushort num = *(ushort*)ptr2;
			ptr2 += 2;
			for (int i = 0; i < num; i++)
			{
				list.Add((sbyte)(*ptr2));
				ptr2++;
			}
			num = *(ushort*)ptr2;
			ptr2 += 2;
			for (int j = 0; j < num; j++)
			{
				list2.Add((sbyte)(*ptr2));
				ptr2++;
			}
			num = *(ushort*)ptr2;
			ptr2 += 2;
			for (int k = 0; k < num; k++)
			{
				list3.Add(*(int*)ptr2);
				ptr2 += 4;
			}
		}
		return _lifeSkillCombat.GetNotUsableEffectCardTemplateIds(context, list2, list3, list);
	}

	[DomainMethod]
	public CombatResultDisplayData ApplyLifeSkillCombatResult(DataContext context, int adversaryCharacterId, bool isTaiwuWin)
	{
		CombatResultDisplayData combatResultDisplayData = new CombatResultDisplayData();
		combatResultDisplayData.EvaluationList = new List<sbyte>();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (isTaiwuWin)
		{
			int num = Math.Clamp(DomainManager.Character.GetElement_Objects(adversaryCharacterId).GetConsummateLevel(), 0, GlobalConfig.Instance.CombatGetAuthorityBase.Length - 1);
			int num2 = GlobalConfig.Instance.CombatGetAuthorityBase[num];
			num2 *= GlobalConfig.Instance.LifeSkillBattleGainRatio;
			num2 /= 100;
			combatResultDisplayData.Resource[7] = num2;
			taiwu.ChangeResource(context, 7, num2);
		}
		if (isTaiwuWin)
		{
			int num3 = 0;
			int num4 = Math.Clamp(DomainManager.Character.GetElement_Objects(adversaryCharacterId).GetConsummateLevel(), 0, GlobalConfig.Instance.CombatGetExpBase.Length - 1);
			num3 += GlobalConfig.Instance.CombatGetExpBase[num4];
			num3 *= GlobalConfig.Instance.LifeSkillBattleGainRatio;
			num3 = (combatResultDisplayData.Exp = num3 / 100);
			taiwu.ChangeExp(context, num3);
			Events.RaiseEvaluationAddExp(context, num3);
		}
		sbyte readInLifeSkillCombatCount = DomainManager.Extra.GetReadInLifeSkillCombatCount();
		ItemKey curReadingBook = DomainManager.Taiwu.GetCurReadingBook();
		SkillBookItem skillBookItem = Config.SkillBook.Instance[curReadingBook.TemplateId];
		if (readInLifeSkillCombatCount > 0 && curReadingBook.IsValid() && DomainManager.Taiwu.GetTotalReadingProgress(curReadingBook.Id) < 100 && skillBookItem.LifeSkillTemplateId >= 0)
		{
			int percentProb = 0;
			if (isTaiwuWin && DomainManager.Character.TryGetElement_Objects(adversaryCharacterId, out var element))
			{
				percentProb = 40 + (element.GetConsummateLevel() - taiwu.GetConsummateLevel()) * 10;
			}
			if (context.Random.CheckPercentProb(percentProb))
			{
				DomainManager.Extra.SetReadInLifeSkillCombatCount((sbyte)(readInLifeSkillCombatCount - 1), context);
				combatResultDisplayData.ShowReadingEvent = DomainManager.Taiwu.UpdateReadingProgressInCombat(context);
				combatResultDisplayData.EvaluationList.Add(33);
				if (combatResultDisplayData.ShowReadingEvent)
				{
					DomainManager.Extra.AddReadingEventBookId(context, curReadingBook.Id);
				}
			}
		}
		CalcQiQrtInLifeCombat(context, adversaryCharacterId, isTaiwuWin, combatResultDisplayData);
		return combatResultDisplayData;
	}

	[Obsolete("新版较艺的相关逻辑直接写在了DebateGameOver()")]
	private void CalcQiQrtInLifeCombat(DataContext context, int adversaryCharacterId, bool isTaiwuWin, CombatResultDisplayData result)
	{
		result.ShowLoopingEvent = false;
		sbyte loopInLifeSkillCombatCount = DomainManager.Extra.GetLoopInLifeSkillCombatCount();
		if (loopInLifeSkillCombatCount <= 0)
		{
			return;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		if (loopingNeigong < 0)
		{
			return;
		}
		CombatSkillKey objectId = new CombatSkillKey(taiwu.GetId(), loopingNeigong);
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(objectId);
		short totalObtainableNeili = element_CombatSkills.GetTotalObtainableNeili();
		short obtainedNeili = element_CombatSkills.GetObtainedNeili();
		if (obtainedNeili < totalObtainableNeili)
		{
			int num = 0;
			if (isTaiwuWin && DomainManager.Character.TryGetElement_Objects(adversaryCharacterId, out var element))
			{
				num = 40 + (element.GetConsummateLevel() - taiwu.GetConsummateLevel()) * 10;
			}
			if (context.Random.CheckPercentProb(num))
			{
				DomainManager.Extra.SetLoopInLifeSkillCombatCount((sbyte)(loopInLifeSkillCombatCount - 1), context);
				DomainManager.Taiwu.ApplyNeigongLoopingImprovementOnce(context);
				result.EvaluationList.Add(43);
				InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
				instantNotificationCollection.AddQiArtInLifeSkillCombatNoChance(loopingNeigong);
				result.ShowLoopingEvent = DomainManager.Taiwu.TryAddLoopingEvent(context, num);
			}
		}
	}

	[DomainMethod]
	public SecretInformationDisplayPackage PickLifeSkillCombatCharacterUseSecretInformation(DataContext context, int characterId)
	{
		SecretInformationDisplayPackage secretInformationDisplayPackageFromCharacter = DomainManager.Information.GetSecretInformationDisplayPackageFromCharacter(characterId);
		Func<SecretInformationDisplayData, int> leveler;
		switch (DomainManager.Character.GetElement_Objects(characterId).GetBehaviorType())
		{
		case 1:
			secretInformationDisplayPackageFromCharacter.SecretInformationDisplayDataList.RemoveAll(delegate(SecretInformationDisplayData d)
			{
				SecretInformationItem secretInformationItem = SecretInformation.Instance[d.SecretInformationTemplateId];
				return secretInformationItem.ValueType != ESecretInformationValueType.Positive && secretInformationItem.ValueType != ESecretInformationValueType.Normal;
			});
			leveler = delegate(SecretInformationDisplayData d)
			{
				if ((d.FilterMask & 4) != 0)
				{
					return 0;
				}
				return ((d.FilterMask & 2) != 0) ? 1 : 3;
			};
			break;
		case 2:
			leveler = (SecretInformationDisplayData d) => ((d.FilterMask & 4) == 0) ? 3 : 0;
			break;
		case 3:
			secretInformationDisplayPackageFromCharacter.SecretInformationDisplayDataList.RemoveAll(delegate(SecretInformationDisplayData d)
			{
				SecretInformationItem secretInformationItem = SecretInformation.Instance[d.SecretInformationTemplateId];
				return secretInformationItem.ValueType != ESecretInformationValueType.Negative && secretInformationItem.ValueType != ESecretInformationValueType.Normal;
			});
			leveler = delegate(SecretInformationDisplayData d)
			{
				if ((d.FilterMask & 4) != 0)
				{
					return 0;
				}
				return ((d.FilterMask & 1) != 0) ? 1 : 3;
			};
			break;
		case 4:
			secretInformationDisplayPackageFromCharacter.SecretInformationDisplayDataList.RemoveAll(delegate(SecretInformationDisplayData d)
			{
				SecretInformationItem secretInformationItem = SecretInformation.Instance[d.SecretInformationTemplateId];
				return secretInformationItem.ValueType != ESecretInformationValueType.Negative && secretInformationItem.ValueType != ESecretInformationValueType.Positive;
			});
			leveler = delegate(SecretInformationDisplayData d)
			{
				if ((d.FilterMask & 4) != 0)
				{
					return 0;
				}
				return ((d.FilterMask & 1) != 0) ? 1 : 3;
			};
			break;
		default:
			leveler = (SecretInformationDisplayData _) => 3;
			break;
		}
		secretInformationDisplayPackageFromCharacter.SecretInformationDisplayDataList.RemoveAll((SecretInformationDisplayData d) => leveler(d) >= 3);
		secretInformationDisplayPackageFromCharacter.SecretInformationDisplayDataList.Sort((SecretInformationDisplayData a, SecretInformationDisplayData b) => leveler(a).CompareTo(leveler(b)));
		while (secretInformationDisplayPackageFromCharacter.SecretInformationDisplayDataList.Count > 0)
		{
			secretInformationDisplayPackageFromCharacter.SecretInformationDisplayDataList.RemoveAt(secretInformationDisplayPackageFromCharacter.SecretInformationDisplayDataList.Count - 1);
		}
		return secretInformationDisplayPackageFromCharacter;
	}

	[DomainMethod]
	public ItemDisplayData PickLifeSkillCombatCharacterUseItem(DataContext context, int characterId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(characterId);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		OrganizationItem organizationItem = Config.Organization.Instance[element_Objects.GetOrganizationInfo().OrgTemplateId];
		if (organizationItem.IsSect)
		{
			int num = organizationItem.LifeSkillCombatBriberyItemTypeWeight.Sum((LifeSkillCombatBriberyItemTypeWeight t) => t.Weight);
			int num2 = organizationItem.LifeSkillCombatBriberyItemSubTypeWeight.Sum((LifeSkillCombatBriberyItemSubTypeWeight t) => t.Weight);
			if (num + num2 == 0)
			{
				return null;
			}
		}
		sbyte behaviorType = element_Objects.GetBehaviorType();
		sbyte b = behaviorType;
		if ((uint)(b - 2) <= 2u)
		{
			List<ItemDisplayData> allInventoryItems = DomainManager.Character.GetAllInventoryItems(characterId);
			sbyte consummateLevel = taiwu.GetConsummateLevel();
			sbyte consummateLevel2 = element_Objects.GetConsummateLevel();
			sbyte grade = element_Objects.GetOrganizationInfo().Grade;
			int minGrade;
			int maxGrade;
			if (consummateLevel >= consummateLevel2)
			{
				minGrade = Math.Clamp(grade - 2, 0, 8);
				maxGrade = Math.Clamp(grade, 0, 8);
			}
			else
			{
				minGrade = Math.Clamp(consummateLevel / 2 - 2, 0, 8);
				maxGrade = Math.Clamp(consummateLevel / 2, 0, 8);
			}
			allInventoryItems.RemoveAll(delegate(ItemDisplayData itemData)
			{
				List<ItemKey> allItemKeysFromPool = itemData.GetAllItemKeysFromPool();
				foreach (ItemKey item in allItemKeysFromPool)
				{
					if (item.Equals(_lockedItemKey))
					{
						return true;
					}
					sbyte grade2 = ItemTemplateHelper.GetGrade(item.ItemType, item.TemplateId);
					if (grade2 < minGrade || grade2 > maxGrade)
					{
						return true;
					}
				}
				ItemDisplayData.ReturnItemKeyListToPool(allItemKeysFromPool);
				return false;
			});
			ItemDisplayData itemDisplayData = null;
			if (organizationItem.IsSect)
			{
				Dictionary<int, (short, int, List<ItemDisplayData>)> dictionary = organizationItem.LifeSkillCombatBriberyItemTypeWeight.Where((LifeSkillCombatBriberyItemTypeWeight p) => p.Weight > 0).ToDictionary((Func<LifeSkillCombatBriberyItemTypeWeight, int>)((LifeSkillCombatBriberyItemTypeWeight p) => p.ItemType), (Func<LifeSkillCombatBriberyItemTypeWeight, (short, int, List<ItemDisplayData>)>)((LifeSkillCombatBriberyItemTypeWeight p) => (Weight: p.Weight, 0, new List<ItemDisplayData>())));
				Dictionary<int, (short, int, List<ItemDisplayData>)> dictionary2 = organizationItem.LifeSkillCombatBriberyItemSubTypeWeight.Where((LifeSkillCombatBriberyItemSubTypeWeight p) => p.Weight > 0).ToDictionary((Func<LifeSkillCombatBriberyItemSubTypeWeight, int>)((LifeSkillCombatBriberyItemSubTypeWeight p) => p.ItemSubType), (Func<LifeSkillCombatBriberyItemSubTypeWeight, (short, int, List<ItemDisplayData>)>)((LifeSkillCombatBriberyItemSubTypeWeight p) => (Weight: p.Weight, 0, new List<ItemDisplayData>())));
				foreach (ItemDisplayData item2 in allInventoryItems)
				{
					sbyte itemType = item2.RealKey.ItemType;
					if (dictionary.TryGetValue(itemType, out var value))
					{
						value.Item3.Add(item2);
						continue;
					}
					short itemSubType = ItemTemplateHelper.GetItemSubType(itemType, item2.RealKey.TemplateId);
					if (dictionary2.TryGetValue(itemSubType, out var value2))
					{
						value2.Item3.Add(item2);
					}
				}
				HandleBriberyWeightDict(dictionary);
				HandleBriberyWeightDict(dictionary2);
				int num3 = dictionary.Sum<KeyValuePair<int, (short, int, List<ItemDisplayData>)>>((KeyValuePair<int, (short Weight, int, List<ItemDisplayData>)> p) => p.Value.Weight);
				int num4 = dictionary2.Sum<KeyValuePair<int, (short, int, List<ItemDisplayData>)>>((KeyValuePair<int, (short Weight, int, List<ItemDisplayData>)> p) => p.Value.Weight);
				int num5 = num3 + num4;
				if (num5 > 0)
				{
					int random = context.Random.Next(num5);
					if (random < num3)
					{
						random = context.Random.Next(num3);
						itemDisplayData = dictionary.Values.Where<(short, int, List<ItemDisplayData>)>(((short Weight, int, List<ItemDisplayData>) p) => p.Item2 > random).MinBy<(short, int, List<ItemDisplayData>), int>(((short Weight, int, List<ItemDisplayData>) p) => p.Item2).Item3.GetRandom(context.Random);
					}
					else
					{
						random = context.Random.Next(num4);
						itemDisplayData = dictionary2.Values.Where<(short, int, List<ItemDisplayData>)>(((short Weight, int, List<ItemDisplayData>) p) => p.Item2 > random).MinBy<(short, int, List<ItemDisplayData>), int>(((short Weight, int, List<ItemDisplayData>) p) => p.Item2).Item3.GetRandom(context.Random);
					}
				}
			}
			else
			{
				itemDisplayData = allInventoryItems.GetRandom(context.Random);
			}
			_lifeSkillCombat?.RecordLine($"{"PickLifeSkillCombatCharacterUseItem"} picked {itemDisplayData}");
			return itemDisplayData;
		}
		return null;
	}

	private void HandleBriberyWeightDict(Dictionary<int, (short weight, int curWeight, List<ItemDisplayData> list)> dict)
	{
		foreach (int key in dict.Keys)
		{
			List<ItemDisplayData> item = dict[key].list;
			if (item == null || item.Count <= 0)
			{
				dict.Remove(key);
			}
		}
		int num = 0;
		foreach (int key2 in dict.Keys)
		{
			(short, int, List<ItemDisplayData>) value = dict[key2];
			num = (value.Item2 = num + value.Item1);
			dict[key2] = value;
		}
	}

	public void SetLifeSkillCombatLockItem(DataContext dataContext, ItemKey itemKey)
	{
		_lockedItemKey = itemKey;
	}

	public void OnCurrentPlayerUseForceSilent(sbyte type, bool result)
	{
		Player player = _lifeSkillCombat.GetPlayer(_lifeSkillCombat.CurrentPlayerId);
		player.SetForceSilentRemainingCount(player.ForceSilentRemainingCount - 1);
		_lifeSkillCombat.PlayerForcedSecretInformation[player.Id] = type;
		if (result)
		{
			_lifeSkillCombat.AcceptSilentPlayerId = Player.PredefinedId.GetTheOtherSide(player.Id);
		}
	}

	public sbyte GetLifeSkillCombatCharacterBehaviorType()
	{
		return DomainManager.Character.GetElement_Objects(_lifeSkillCombat.GetPlayerCharacterId(1)).GetBehaviorType();
	}

	[DomainMethod]
	public void SetReferenceCombatSkillAt(DataContext context, int index, short combatSkillTemplateId)
	{
		if (!IsReferenceSkillSlotUnlocked(index))
		{
			throw new InvalidOperationException("Reference skill slot is locked.");
		}
		List<short> list = DomainManager.Extra.GetReferenceSkillList() ?? new List<short>();
		while (list.Count < 3)
		{
			list.Add(-1);
		}
		List<short> list2 = list;
		list2[index] = combatSkillTemplateId;
		DomainManager.Extra.SetReferenceSkillList(list2, context);
		short loopingNeigong = _taiwuChar.GetLoopingNeigong();
		TryRemoveLoopingEvent(context, loopingNeigong);
	}

	[DomainMethod]
	public void SetTaiwuLoopingNeigong(DataContext context, short combatSkillTemplateId)
	{
		short loopingNeigong = _taiwuChar.GetLoopingNeigong();
		if (combatSkillTemplateId != loopingNeigong)
		{
			TryRemoveLoopingEvent(context, loopingNeigong);
			_taiwuChar.SetLoopingNeigong(combatSkillTemplateId, context);
		}
	}

	private void TryRemoveLoopingEvent(DataContext context, short loopingNeigong)
	{
		List<short> loopingEventSkillIdList = DomainManager.Extra.GetLoopingEventSkillIdList();
		if (loopingEventSkillIdList.Contains(loopingNeigong))
		{
			loopingEventSkillIdList.Remove(loopingNeigong);
			DomainManager.Extra.SetLoopingEventSkillIdList(loopingEventSkillIdList, context);
		}
	}

	public bool IsReferenceSkillSlotUnlocked(int slotIndex)
	{
		byte referenceSkillSlotUnlockStates = GetReferenceSkillSlotUnlockStates();
		return (referenceSkillSlotUnlockStates & (1 << slotIndex)) != 0;
	}

	public void ResetLoopInCombatCounts(DataContext context)
	{
		DomainManager.Extra.SetLoopInCombatCount(1, context);
		DomainManager.Extra.SetLoopInLifeSkillCombatCount(1, context);
	}

	public void TryRemoveReferenceSkillAtLockedSlot(DataContext context)
	{
		List<short> referenceSkillList = DomainManager.Extra.GetReferenceSkillList();
		for (int i = 0; i < referenceSkillList.Count; i++)
		{
			if (referenceSkillList[i] != -1 && !IsReferenceSkillSlotUnlocked(i))
			{
				referenceSkillList[i] = -1;
			}
		}
		DomainManager.Extra.SetReferenceSkillList(referenceSkillList, context);
	}

	[DomainMethod]
	public void SetAutoAllocateNeiliToMax(bool value)
	{
		_autoAllocateNeiliToMax = value;
	}

	public void UpdateTaiwuNeiliAllocation(DataContext context, bool isInCombat = false)
	{
		if (_autoAllocateNeiliToMax)
		{
			NeiliAllocation taiwuMaxNeiliAllocation = DomainManager.Extra.GetTaiwuMaxNeiliAllocation();
			UpdateTaiwuNeiliAllocation(context, taiwuMaxNeiliAllocation, isInCombat);
		}
	}

	public void UpdateTaiwuNeiliAllocation(DataContext context, NeiliAllocation targetAllocation, bool isInCombat = false)
	{
		if (_autoAllocateNeiliToMax)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			NeiliAllocation current = taiwu.GetBaseNeiliAllocation();
			int currNeili = taiwu.GetCurrNeili();
			int maxNeili = taiwu.GetMaxNeili();
			short maxTotalNeiliAllocationConsideringFeature = CombatHelper.GetMaxTotalNeiliAllocationConsideringFeature(taiwu.GetConsummateLevel(), taiwu.GetFeatureIds());
			if (isInCombat)
			{
				short disorderOfQi = taiwu.GetDisorderOfQi();
				sbyte disorderLevelOfQi = DisorderLevelOfQi.GetDisorderLevelOfQi(disorderOfQi);
				sbyte neiliCostInCombat = QiDisorderEffect.Instance[disorderLevelOfQi].NeiliCostInCombat;
				CombatHelper.TryAllocateToTargetAllocation(targetAllocation, ref current, maxNeili, ref currNeili, maxTotalNeiliAllocationConsideringFeature, neiliCostInCombat);
			}
			else
			{
				CombatHelper.TryAllocateToTargetAllocation(targetAllocation, ref current, maxNeili, ref currNeili, maxTotalNeiliAllocationConsideringFeature);
			}
			taiwu.SetBaseNeiliAllocation(current, context);
			taiwu.SpecifyCurrNeili(context, currNeili);
		}
	}

	[DomainMethod]
	public unsafe void ActiveNeigongLoopingOnce(DataContext context)
	{
		int activeNeigongLoopingTimeCost = GlobalConfig.Instance.ActiveNeigongLoopingTimeCost;
		MainAttributes currMainAttributes = _taiwuChar.GetCurrMainAttributes();
		short activeNeigongLoopingAttributeCost = GlobalConfig.Instance.ActiveNeigongLoopingAttributeCost;
		short num = currMainAttributes.Items[2];
		if (DomainManager.World.GetLeftDaysInCurrMonth() < activeNeigongLoopingTimeCost)
		{
			Logger.Warn("You don't have enough days in this month");
			return;
		}
		if (activeNeigongLoopingAttributeCost > num)
		{
			Logger.Warn($"You don't have enough intelligence: {activeNeigongLoopingAttributeCost} needed, current int is {num}");
			return;
		}
		DomainManager.World.AdvanceDaysInMonth(context, activeNeigongLoopingTimeCost);
		if (activeNeigongLoopingAttributeCost > 0)
		{
			currMainAttributes[2] -= activeNeigongLoopingAttributeCost;
			_taiwuChar.SetCurrMainAttributes(currMainAttributes, context);
		}
		short maxActiveNeigongLoopingProgress = GlobalConfig.Instance.MaxActiveNeigongLoopingProgress;
		short activeLoopingProgress = DomainManager.Extra.GetActiveLoopingProgress();
		int num2 = activeLoopingProgress + 1;
		if (num2 <= maxActiveNeigongLoopingProgress)
		{
			DomainManager.Extra.SetActiveLoopingProgress((short)num2, context);
			bool flag = num2 % 10 == 0;
			int efficiency = GlobalConfig.Instance.ActiveLoopProgressAffectedEfficiency[activeLoopingProgress / 10];
			if (flag)
			{
				ApplyNeigongLoopingImprovementOnce(context, efficiency);
			}
		}
	}

	public void ApplyNeigongLoopingImprovementOnce(DataContext context, int efficiency = 100)
	{
		if (_taiwuChar.GenerateNeigongLoopingImprovementForTaiwu(context, out var neili, out var qiDisorder, out var extraNeiliAllocationProgress))
		{
			neili = (short)(neili * efficiency / 100);
			qiDisorder = (short)(qiDisorder * efficiency / 100);
			for (int i = 0; i < extraNeiliAllocationProgress.Length; i++)
			{
				extraNeiliAllocationProgress[i] = extraNeiliAllocationProgress[i] * efficiency / 100;
			}
			DomainManager.CombatSkill.ApplyNeigongLoopingEffect(context, _taiwuChar, _taiwuChar.GetLoopingNeigong(), neili, extraNeiliAllocationProgress);
			if (qiDisorder != 0)
			{
				_taiwuChar.ChangeDisorderOfQiRandomRecovery(context, qiDisorder);
			}
			DomainManager.World.UpdateBaihuaLifeLinkNeiliType(context);
		}
	}

	[DomainMethod]
	public void GmCmd_TaiwuActiveLoopingApply(DataContext context)
	{
		ApplyNeigongLoopingImprovementOnce(context);
	}

	public void ClearActiveNeigongLoopingProgressOnMonthChange(DataContext context)
	{
		short activeLoopingProgress = DomainManager.Extra.GetActiveLoopingProgress();
		int num = activeLoopingProgress % 10;
		DomainManager.Extra.SetActiveLoopingProgress((short)num, context);
	}

	[DomainMethod]
	public ProfessionTipDisplayData GetProfessionTipDisplayData(int professionId)
	{
		var (workingSkillType, attainmentBonus) = GetWorkingSkillTypeAndBonus();
		return new ProfessionTipDisplayData
		{
			ProfessionId = professionId,
			WorkingSkillType = workingSkillType,
			AttainmentBonus = attainmentBonus,
			ProfessionUpgrade = DomainManager.Extra.GetProfessionUpgrade(),
			ProfessionUpgradeBonus = ExtraDomain.GetWorldProfessionUpgradePercent(),
			IsWearingBonusClothing = IsWearingBonusClothing()
		};
		(sbyte, int) GetWorkingSkillTypeAndBonus()
		{
			ProfessionItem professionItem = Config.Profession.Instance[professionId];
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int num = 100;
			sbyte item = -1;
			if (professionItem.BonusLifeSkills != null)
			{
				short num2 = 0;
				foreach (sbyte bonusLifeSkill in professionItem.BonusLifeSkills)
				{
					short lifeSkillAttainment = taiwu.GetLifeSkillAttainment(bonusLifeSkill);
					if (lifeSkillAttainment > num2)
					{
						num2 = lifeSkillAttainment;
						item = bonusLifeSkill;
					}
				}
				num += num2 / 3;
			}
			if (professionItem.BonusCombatSkills != null)
			{
				short num3 = 0;
				foreach (sbyte bonusCombatSkill in professionItem.BonusCombatSkills)
				{
					short combatSkillAttainment = taiwu.GetCombatSkillAttainment(bonusCombatSkill);
					if (combatSkillAttainment > num3)
					{
						num3 = combatSkillAttainment;
						item = bonusCombatSkill;
					}
				}
				num += num3 / 3;
			}
			return (item, num);
		}
		bool IsWearingBonusClothing()
		{
			ItemKey itemKey = _taiwuChar.GetEquipment()[4];
			if (!itemKey.IsValid() || DomainManager.Item.GetBaseItem(itemKey).IsDurabilityRunningOut())
			{
				return false;
			}
			ProfessionItem professionItem = Config.Profession.Instance[professionId];
			return professionItem.BonusClothing == itemKey.TemplateId;
		}
	}

	[DomainMethod]
	public void SetQiArtStrategy(DataContext context, int index, sbyte templateId)
	{
		if (index >= GlobalConfig.Instance.MaxQiArtStrategyCount)
		{
			throw new ArgumentOutOfRangeException("index", index, "index should be less than MaxQiArtStrategyCount");
		}
		if (templateId < 0)
		{
			throw new ArgumentOutOfRangeException("templateId", templateId, "templateId should be greater than or equal to 0");
		}
		QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[templateId];
		if (!TryCost(context, qiArtStrategyItem))
		{
			throw new ArgumentException("Concentration or Neili not enough", "templateId");
		}
		short loopingNeigong = _taiwuChar.GetLoopingNeigong();
		if (loopingNeigong < 0)
		{
			throw new Exception("should have looping neigong");
		}
		SByteList orInitQiArtStrategies = DomainManager.Extra.GetOrInitQiArtStrategies(context, loopingNeigong);
		while (index >= orInitQiArtStrategies.Items.Count)
		{
			orInitQiArtStrategies.Items.Add(-1);
		}
		IntList orInitQiArtStrategyExpireTimeList = DomainManager.Extra.GetOrInitQiArtStrategyExpireTimeList(context, loopingNeigong);
		while (index >= orInitQiArtStrategyExpireTimeList.Items.Count)
		{
			orInitQiArtStrategyExpireTimeList.Items.Add(-1);
		}
		PreProcessInstantQiArtStrategy(context, qiArtStrategyItem, orInitQiArtStrategies);
		orInitQiArtStrategies.Items[index] = templateId;
		orInitQiArtStrategyExpireTimeList.Items[index] = qiArtStrategyItem.Duration + DomainManager.World.GetCurrDate();
		RemoveOtherEffectInSameGroup(orInitQiArtStrategies.Items, orInitQiArtStrategyExpireTimeList.Items, index);
		RemoveOtherEffectByConfig(orInitQiArtStrategies.Items, orInitQiArtStrategyExpireTimeList.Items, index);
		ProcessInstantQiArtStrategy(context, qiArtStrategyItem);
		DomainManager.Extra.SetQiArtStrategies(context, loopingNeigong, orInitQiArtStrategies);
		DomainManager.Extra.SetQiArtStrategyExpireTimeList(context, loopingNeigong, orInitQiArtStrategyExpireTimeList);
		short concentrationCost = qiArtStrategyItem.ConcentrationCost;
		_currentLoopingEventCostedConcentration += concentrationCost;
	}

	[DomainMethod]
	public SByteList GetLoopingNeigongQiArtStrategies(DataContext context)
	{
		short loopingNeigong = _taiwuChar.GetLoopingNeigong();
		if (loopingNeigong < 0)
		{
			throw new Exception("should have looping neigong");
		}
		return DomainManager.Extra.GetOrInitQiArtStrategies(context, loopingNeigong);
	}

	[DomainMethod]
	public List<QiArtStrategyDisplayData> GetLoopingNeigongQiArtStrategyDisplayDatas(DataContext context)
	{
		short loopingNeigong = _taiwuChar.GetLoopingNeigong();
		if (loopingNeigong < 0)
		{
			throw new Exception("should have looping neigong");
		}
		SByteList orInitQiArtStrategies = DomainManager.Extra.GetOrInitQiArtStrategies(context, loopingNeigong);
		IntList orInitQiArtStrategyExpireTimeList = DomainManager.Extra.GetOrInitQiArtStrategyExpireTimeList(context, loopingNeigong);
		List<QiArtStrategyDisplayData> list = new List<QiArtStrategyDisplayData>();
		for (int i = 0; i < orInitQiArtStrategyExpireTimeList.Items.Count; i++)
		{
			list.Add(new QiArtStrategyDisplayData
			{
				TemplateId = orInitQiArtStrategies.Items[i],
				ExpireTime = orInitQiArtStrategyExpireTimeList.Items[i]
			});
		}
		return list;
	}

	[DomainMethod]
	public SByteList GetLoopingNeigongAvailableQiArtStrategies(DataContext context)
	{
		short loopingNeigong = _taiwuChar.GetLoopingNeigong();
		if (loopingNeigong < 0)
		{
			throw new Exception("should have looping neigong");
		}
		return DomainManager.Extra.GetElement_AvailableQiArtStrategyMap(loopingNeigong);
	}

	private void RemoveOtherEffectInSameGroup(List<sbyte> qiArtStrategyList, List<int> expireList, int index)
	{
		QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[qiArtStrategyList[index]];
		short activeGroup = qiArtStrategyItem.ActiveGroup;
		if (activeGroup == -1)
		{
			return;
		}
		for (int i = 0; i < qiArtStrategyList.Count; i++)
		{
			if (i == index)
			{
				continue;
			}
			sbyte b = qiArtStrategyList[i];
			if (b != -1)
			{
				QiArtStrategyItem qiArtStrategyItem2 = QiArtStrategy.Instance[b];
				if (qiArtStrategyItem2.ActiveGroup == activeGroup)
				{
					qiArtStrategyList[i] = -1;
					expireList[i] = -1;
				}
			}
		}
	}

	private void RemoveOtherEffectByConfig(List<sbyte> qiArtStrategyList, List<int> expireList, int index)
	{
		QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[qiArtStrategyList[index]];
		if (!qiArtStrategyItem.ClearOtherEffect)
		{
			return;
		}
		for (int i = 0; i < qiArtStrategyList.Count; i++)
		{
			if (i != index)
			{
				qiArtStrategyList[i] = -1;
				expireList[i] = -1;
			}
		}
	}

	private bool TryCost(DataContext context, QiArtStrategyItem config)
	{
		short concentrationCost = config.ConcentrationCost;
		if (concentrationCost > 0 && _taiwuChar.GetCurrMainAttributes()[2] < concentrationCost)
		{
			return false;
		}
		short neiliCost = config.NeiliCost;
		if (neiliCost > 0)
		{
			int currNeili = _taiwuChar.GetCurrNeili();
			if (currNeili < neiliCost)
			{
				return false;
			}
		}
		if (concentrationCost > 0)
		{
			MainAttributes currMainAttributes = _taiwuChar.GetCurrMainAttributes();
			currMainAttributes[2] -= concentrationCost;
			_taiwuChar.SetCurrMainAttributes(currMainAttributes, context);
		}
		if (neiliCost > 0)
		{
			int currNeili2 = _taiwuChar.GetCurrNeili();
			_taiwuChar.SetCurrNeili(currNeili2 - neiliCost, context);
		}
		return true;
	}

	private void PreProcessInstantQiArtStrategy(DataContext context, QiArtStrategyItem config, SByteList qiArtStrategyList)
	{
		TryGainConcentrationInstantly(context, config, qiArtStrategyList);
	}

	private void ProcessInstantQiArtStrategy(DataContext context, QiArtStrategyItem config)
	{
		short neili;
		short qiDisorder;
		int[] extraNeiliAllocationProgress;
		bool haveLoopingNeigong = _taiwuChar.GenerateNeigongLoopingImprovementForTaiwu(context, out neili, out qiDisorder, out extraNeiliAllocationProgress);
		TryGainNeiliInstantly(context, config, haveLoopingNeigong, neili);
		TryGainFiveElementsInstantly(context, config);
		TryGainExtraNeiliAllocationProgressInstantly(context, config, extraNeiliAllocationProgress);
	}

	private void TryGainExtraNeiliAllocationProgressInstantly(DataContext context, QiArtStrategyItem config, int[] extraNeiliAllocationProgress)
	{
		if (config.MinGainNeiliAllocation > 0 && config.MaxGainNeiliAllocation > 0 && extraNeiliAllocationProgress != null)
		{
			int randomBetween = GetRandomBetween(context.Random, config.MinGainNeiliAllocation, config.MaxGainNeiliAllocation);
			for (int i = 0; i < extraNeiliAllocationProgress.Length; i++)
			{
				extraNeiliAllocationProgress[i] = extraNeiliAllocationProgress[i] * randomBetween / 100;
			}
			DomainManager.CombatSkill.ApplyLoopingExtraNeiliAllocationProgressModify(context, _taiwuChar, extraNeiliAllocationProgress);
		}
	}

	private void TryGainFiveElementsInstantly(DataContext context, QiArtStrategyItem config)
	{
		if (config.MinGainFiveElements > 0 && config.MaxGainFiveElements > 0)
		{
			int randomBetween = GetRandomBetween(context.Random, config.MinGainFiveElements, config.MaxGainFiveElements);
			(sbyte destinationType, sbyte transferType, sbyte amount) loopingTransferNeiliProportionOfFiveElementsDataForTaiwu = DomainManager.CombatSkill.GetLoopingTransferNeiliProportionOfFiveElementsDataForTaiwu(context, _taiwuChar);
			sbyte item = loopingTransferNeiliProportionOfFiveElementsDataForTaiwu.destinationType;
			sbyte item2 = loopingTransferNeiliProportionOfFiveElementsDataForTaiwu.transferType;
			sbyte item3 = loopingTransferNeiliProportionOfFiveElementsDataForTaiwu.amount;
			item3 = (sbyte)(item3 * randomBetween / 100);
			if (item3 > 0 && item != -1 && item2 != -1)
			{
				_taiwuChar.TransferNeiliProportionOfFiveElements(context, item, item2, item3);
			}
		}
	}

	private void TryGainNeiliInstantly(DataContext context, QiArtStrategyItem config, bool haveLoopingNeigong, short originDeltaNeili)
	{
		if (config.MinGainNeili > 0 && config.MaxGainNeili > 0)
		{
			int randomBetween = GetRandomBetween(context.Random, config.MinGainNeili, config.MaxGainNeili);
			if (haveLoopingNeigong)
			{
				short obtainedNeili = (short)(originDeltaNeili * randomBetween / 100);
				DomainManager.CombatSkill.ApplyLoopingNeiliModifyForTaiwu(context, _taiwuChar, obtainedNeili);
			}
		}
	}

	private void TryGainConcentrationInstantly(DataContext context, QiArtStrategyItem config, SByteList qiArtStrategyList)
	{
		if (config.TemplateId != 39)
		{
			return;
		}
		int num = 0;
		foreach (sbyte item in qiArtStrategyList.Items)
		{
			if (item >= 0)
			{
				QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[item];
				num += qiArtStrategyItem.ConcentrationCost / 2;
			}
		}
		_taiwuChar.ChangeCurrMainAttribute(context, 2, num);
	}

	public short GetAnchoredFiveElements()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		if (DomainManager.Extra.TryGetElement_QiArtStrategyMap(loopingNeigong, out var value))
		{
			foreach (sbyte item in value.Items)
			{
				if (item != -1)
				{
					QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[item];
					if (qiArtStrategyItem.AnchorFiveElements != -1)
					{
						return qiArtStrategyItem.AnchorFiveElements;
					}
				}
			}
		}
		return -1;
	}

	public (sbyte destinationType, sbyte transferType) GetOverrideFiveElementTransferInfo()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		if (DomainManager.Extra.TryGetElement_QiArtStrategyMap(loopingNeigong, out var value))
		{
			foreach (sbyte item in value.Items)
			{
				if (item != -1)
				{
					QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[item];
					if (qiArtStrategyItem.TransferToFiveElements > -1 && qiArtStrategyItem.FiveElementsTransferType > -1)
					{
						return (destinationType: qiArtStrategyItem.TransferToFiveElements, transferType: qiArtStrategyItem.FiveElementsTransferType);
					}
				}
			}
		}
		return (destinationType: -1, transferType: -1);
	}

	public int GetQiArtStrategyDeltaNeiliBonus(IRandomSource random)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		if (DomainManager.Extra.TryGetElement_QiArtStrategyMap(loopingNeigong, out var value))
		{
			if (value.Items.Count == 0)
			{
				return 0;
			}
			int num = 0;
			foreach (sbyte item in value.Items)
			{
				if (item != -1)
				{
					QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[item];
					num += GetRandomBetween(random, qiArtStrategyItem.MinExtraNeili, qiArtStrategyItem.MaxExtraNeili);
				}
			}
			return num;
		}
		return 0;
	}

	public (int min, int max) GetQiArtStrategyDeltaNeiliBonusRange()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		if (DomainManager.Extra.TryGetElement_QiArtStrategyMap(loopingNeigong, out var value))
		{
			if (value.Items.Count == 0)
			{
				return (min: 0, max: 0);
			}
			int num = 0;
			int num2 = 0;
			foreach (sbyte item in value.Items)
			{
				if (item != -1)
				{
					QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[item];
					num += qiArtStrategyItem.MinExtraNeili;
					num2 += qiArtStrategyItem.MaxExtraNeili;
				}
			}
			return (min: num, max: num2);
		}
		return (min: 0, max: 0);
	}

	public int GetQiArtStrategyFiveElementTransferAmountBonus(IRandomSource random)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		if (DomainManager.Extra.TryGetElement_QiArtStrategyMap(loopingNeigong, out var value))
		{
			if (value.Items.Count == 0)
			{
				return 0;
			}
			int num = 0;
			foreach (sbyte item in value.Items)
			{
				if (item != -1)
				{
					QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[item];
					num += GetRandomBetween(random, qiArtStrategyItem.MinExtraFiveElements, qiArtStrategyItem.MaxExtraFiveElements);
				}
			}
			return num;
		}
		return 0;
	}

	public int GetQiArtStrategyExtraNeiliAllocationBonus(IRandomSource random, short loopingNeigongTemplateId)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (DomainManager.Extra.TryGetElement_QiArtStrategyMap(loopingNeigongTemplateId, out var value))
		{
			if (value.Items.Count == 0)
			{
				return 0;
			}
			int num = 0;
			foreach (sbyte item in value.Items)
			{
				if (item != -1)
				{
					QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[item];
					num += GetRandomBetween(random, qiArtStrategyItem.MinExtraNeiliAllocation, qiArtStrategyItem.MaxExtraNeiliAllocation);
				}
			}
			return num;
		}
		return 0;
	}

	public (int min, int max) GetQiArtStrategyExtraNeiliAllocationBonusRange()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		if (DomainManager.Extra.TryGetElement_QiArtStrategyMap(loopingNeigong, out var value))
		{
			if (value.Items.Count == 0)
			{
				return (min: 0, max: 0);
			}
			int num = 0;
			int num2 = 0;
			foreach (sbyte item in value.Items)
			{
				if (item != -1)
				{
					QiArtStrategyItem qiArtStrategyItem = QiArtStrategy.Instance[item];
					num += qiArtStrategyItem.MinExtraNeiliAllocation;
					num2 += qiArtStrategyItem.MaxExtraNeiliAllocation;
				}
			}
			return (min: num, max: num2);
		}
		return (min: 0, max: 0);
	}

	public void ClearExpiredQiArtStrategies(DataContext context)
	{
		foreach (KeyValuePair<short, SByteList> item in DomainManager.Extra.GetQiArtStrategyMapEnumerable())
		{
			item.Deconstruct(out var key, out var value);
			short num = key;
			SByteList strategies = value;
			IntList element_QiArtStrategyExpireTimeMap = DomainManager.Extra.GetElement_QiArtStrategyExpireTimeMap(num);
			int currDate = DomainManager.World.GetCurrDate();
			for (int i = 0; i < element_QiArtStrategyExpireTimeMap.Items.Count; i++)
			{
				sbyte b = strategies.Items[i];
				if (b != -1 && currDate >= element_QiArtStrategyExpireTimeMap.Items[i])
				{
					strategies.Items[i] = -1;
					element_QiArtStrategyExpireTimeMap.Items[i] = -1;
				}
			}
			DomainManager.Extra.SetQiArtStrategyExpireTimeList(context, num, element_QiArtStrategyExpireTimeMap);
			DomainManager.Extra.SetQiArtStrategies(context, num, strategies);
		}
	}

	public bool TryAddLoopingEvent(DataContext context, int basePercentProb)
	{
		int num = basePercentProb;
		List<short> referenceSkillList = DomainManager.Extra.GetReferenceSkillList();
		if (referenceSkillList != null && referenceSkillList.Count > 0)
		{
			foreach (short item in referenceSkillList)
			{
				if (item != -1)
				{
					CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[item];
					num += combatSkillItem.QiArtStrategyGenerateProbability;
				}
			}
		}
		if (!context.Random.CheckPercentProb(num))
		{
			return false;
		}
		return AddLoopingEvent(context);
	}

	public bool AddLoopingEvent(DataContext context)
	{
		_currentLoopingEventCostedConcentration = 0;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		if (loopingNeigong < 0)
		{
			return false;
		}
		List<short> loopingEventSkillIdList = DomainManager.Extra.GetLoopingEventSkillIdList();
		if (loopingEventSkillIdList.Contains(loopingNeigong))
		{
			return false;
		}
		loopingEventSkillIdList.Add(loopingNeigong);
		DomainManager.Extra.SetLoopingEventSkillIdList(loopingEventSkillIdList, context);
		GenerateAvailableQiArtStrategies(context, loopingNeigong);
		return true;
	}

	private void GenerateAvailableQiArtStrategies(DataContext context, short loopingNeigongTemplateId)
	{
		QiArtStrategy instance = QiArtStrategy.Instance;
		List<List<sbyte>> list = new List<List<sbyte>>();
		for (int i = 0; i < 2; i++)
		{
			list.Add(new List<sbyte>());
		}
		sbyte b = DrawOneByNeigong(loopingNeigongTemplateId, context.Random);
		if (b != -1)
		{
			list[instance[b].ExtractGroup - 1].Add(b);
		}
		List<short> referenceSkillList = DomainManager.Extra.GetReferenceSkillList();
		if (referenceSkillList != null && referenceSkillList.Count > 0)
		{
			foreach (short item2 in referenceSkillList)
			{
				if (item2 == -1)
				{
					continue;
				}
				sbyte b2 = DrawOneByNeigong(item2, context.Random);
				if (b2 != -1)
				{
					List<sbyte> list2 = list[instance[b2].ExtractGroup - 1];
					if (list2.Count < 3)
					{
						list2.Add(b2);
					}
				}
			}
		}
		List<sbyte> list3 = new List<sbyte>();
		List<short> list4 = new List<short>();
		for (int j = 0; j < 2; j++)
		{
			list3.Clear();
			list4.Clear();
			foreach (QiArtStrategyItem item3 in (IEnumerable<QiArtStrategyItem>)instance)
			{
				sbyte templateId = item3.TemplateId;
				if (item3.ExtractGroup - 1 == j && CanDrawStrategy(templateId, -1))
				{
					list3.Add(templateId);
					list4.Add(item3.ExtractWeight);
				}
			}
			while (list[j].Count < 3)
			{
				int randomIndex = RandomUtils.GetRandomIndex(list4, context.Random);
				sbyte item = list3[randomIndex];
				list[j].Add(item);
			}
		}
		List<sbyte> list5 = new List<sbyte>();
		foreach (List<sbyte> item4 in list)
		{
			list5.AddRange(item4);
		}
		if (DomainManager.Extra.IsProfessionalSkillUnlocked(3, 1))
		{
			int seniority = DomainManager.Extra.GetProfessionData(3).Seniority;
			int num = ProfessionData.SeniorityToExtraReadingLoopingStrategyCount(seniority);
			for (int k = 0; k < num; k++)
			{
				int num2 = context.Random.Next(QiArtStrategy.Instance.Count);
				list5.Add((sbyte)num2);
			}
		}
		DomainManager.Extra.SetAvailableQiArtStrategiesForNeigong(context, loopingNeigongTemplateId, new SByteList(list5));
	}

	private sbyte DrawOneByNeigong(short sourceNeigongTemplateId, IRandomSource randomSource)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[sourceNeigongTemplateId];
		List<sbyte> possibleQiArtStrategyList = combatSkillItem.PossibleQiArtStrategyList;
		List<sbyte> list = new List<sbyte>();
		List<short> list2 = new List<short>();
		foreach (sbyte item in possibleQiArtStrategyList)
		{
			if (CanDrawStrategy(item, sourceNeigongTemplateId))
			{
				list.Add(item);
				list2.Add(QiArtStrategy.Instance[item].ExtractWeight);
			}
		}
		if (list2.Count == 0)
		{
			return -1;
		}
		int randomIndex = RandomUtils.GetRandomIndex(list2, randomSource);
		return list[randomIndex];
	}

	private bool CanDrawStrategy(sbyte qiArtStrategyTemplateId, short sourceNeigongTemplateId)
	{
		QiArtStrategyItem config = QiArtStrategy.Instance[qiArtStrategyTemplateId];
		if (UseDrawConditionCanObtainNeiliIdList.Contains(qiArtStrategyTemplateId))
		{
			return DrawConditionCanObtainNeili();
		}
		if (UseDrawConditionNeiliAllocationLessThanIdList.Contains(qiArtStrategyTemplateId))
		{
			return DrawConditionCanObtainNeiliAllocation();
		}
		if (qiArtStrategyTemplateId >= 18 && qiArtStrategyTemplateId <= 23)
		{
			return DrawConditionIsReferenceSkill(sourceNeigongTemplateId);
		}
		if (qiArtStrategyTemplateId >= 24 && qiArtStrategyTemplateId <= 38)
		{
			return DrawConditionIsDifferenceFiveElementTransfer(config);
		}
		return true;
	}

	private bool DrawConditionIsDifferenceFiveElementTransfer(QiArtStrategyItem config)
	{
		sbyte transferToFiveElements = config.TransferToFiveElements;
		sbyte fiveElementsTransferType = config.FiveElementsTransferType;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[loopingNeigong];
		sbyte destTypeWhileLooping = combatSkillItem.DestTypeWhileLooping;
		sbyte transferTypeWhileLooping = combatSkillItem.TransferTypeWhileLooping;
		return destTypeWhileLooping != transferToFiveElements || fiveElementsTransferType != transferTypeWhileLooping;
	}

	private bool DrawConditionIsReferenceSkill(short sourceNeigongTemplateId)
	{
		List<short> referenceSkillList = DomainManager.Extra.GetReferenceSkillList();
		if (referenceSkillList == null)
		{
			return false;
		}
		if (referenceSkillList.Count == 0)
		{
			return false;
		}
		return referenceSkillList.Contains(sourceNeigongTemplateId);
	}

	private bool DrawConditionCanObtainNeiliAllocation()
	{
		int neiliAllocationMaxProgress = CombatSkillDomain.GetNeiliAllocationMaxProgress();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (!DomainManager.Extra.TryGetExtraNeiliAllocationProgress(taiwu.GetId(), out var result))
		{
			return true;
		}
		for (int i = 0; i < 4; i++)
		{
			if (result.Items[i] < neiliAllocationMaxProgress)
			{
				return true;
			}
		}
		return false;
	}

	private bool DrawConditionCanObtainNeili()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		CombatSkillKey objectId = new CombatSkillKey(taiwu.GetId(), loopingNeigong);
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(objectId);
		short totalObtainableNeili = element_CombatSkills.GetTotalObtainableNeili();
		short obtainedNeili = element_CombatSkills.GetObtainedNeili();
		return obtainedNeili < totalObtainableNeili;
	}

	[DomainMethod]
	public void ClearCurrentLoopingNeigongEvent(DataContext context)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		if (_currentLoopingEventCostedConcentration > 0)
		{
			sbyte grade = Config.CombatSkill.Instance[loopingNeigong].Grade;
			int baseDelta = ProfessionFormulaImpl.Calculate(28, _currentLoopingEventCostedConcentration, grade);
			DomainManager.Extra.ChangeProfessionSeniority(context, 3, baseDelta);
		}
		_currentLoopingEventCostedConcentration = 0;
		List<short> loopingEventSkillIdList = DomainManager.Extra.GetLoopingEventSkillIdList();
		if (loopingEventSkillIdList.Contains(loopingNeigong))
		{
			loopingEventSkillIdList.Remove(loopingNeigong);
			DomainManager.Extra.SetLoopingEventSkillIdList(loopingEventSkillIdList, context);
		}
	}

	private static int GetRandomBetween(IRandomSource random, int a, int b)
	{
		if (a.Equals(b))
		{
			return a;
		}
		return random.Next(a, b);
	}

	[DomainMethod]
	public void SetReferenceBook(DataContext context, sbyte index, ItemKey bookItemKey)
	{
		if (bookItemKey.IsValid())
		{
			if (!IsReferenceBookSlotUnlocked(index))
			{
				throw new Exception($"Reference book slot {index} is locked");
			}
			if (_curReadingBook.Id == bookItemKey.Id)
			{
				throw new Exception($"Book with Id {bookItemKey.Id} is already being read.");
			}
			for (int i = 0; i < _referenceBooks.Length; i++)
			{
				if (i != index && bookItemKey.Id == _referenceBooks[i].Id)
				{
					throw new Exception($"Book with templateId {bookItemKey.Id} is already being referenced at slot {i}");
				}
			}
		}
		_referenceBooks[index] = bookItemKey;
		SetReferenceBooks(_referenceBooks, context);
	}

	[DomainMethod]
	public void SetReadingBook(DataContext context, ItemKey bookItemKey)
	{
		if (bookItemKey.IsValid())
		{
			for (int i = 0; i < _referenceBooks.Length; i++)
			{
				if (bookItemKey.Id == _referenceBooks[i].Id)
				{
					throw new Exception($"Book with templateId {bookItemKey.TemplateId} is already being referenced at slot {i}");
				}
			}
			if (!_readingBooks.ContainsKey(bookItemKey))
			{
				ReadingBookStrategies value = default(ReadingBookStrategies);
				value.Initialize();
				AddElement_ReadingBooks(bookItemKey, ref value, context);
			}
		}
		else
		{
			bool flag = false;
			for (int j = 0; j < _referenceBooks.Length; j++)
			{
				if (_referenceBooks[j].IsValid())
				{
					_referenceBooks[j] = ItemKey.Invalid;
					flag = true;
				}
			}
			if (flag)
			{
				SetReferenceBooks(_referenceBooks, context);
			}
		}
		SetCurReadingBook(bookItemKey, context);
	}

	[DomainMethod]
	public ReadingBookStrategies GetCurReadingStrategies()
	{
		if (!_readingBooks.TryGetValue(_curReadingBook, out var value))
		{
			value.Initialize();
		}
		return value;
	}

	[DomainMethod]
	public unsafe void SetReadingStrategy(DataContext context, byte pageIndex, int strategyIndex, sbyte strategyId)
	{
		if (!_curReadingBook.IsValid())
		{
			throw new Exception($"CurrentReadingBook is invalid {_curReadingBook}");
		}
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
		short itemSubType = ItemTemplateHelper.GetItemSubType(_curReadingBook.ItemType, _curReadingBook.TemplateId);
		int num = ((itemSubType == 1001) ? 6 : 5);
		if (pageIndex >= num)
		{
			throw new Exception($"the given page index {pageIndex} is illegal.");
		}
		if (strategyIndex < 0 || strategyIndex >= 3)
		{
			throw new Exception($"The given strategy slot index {strategyIndex} is illegal.");
		}
		ReadingStrategyItem readingStrategyItem = ReadingStrategy.Instance[strategyId];
		if (!_readingBooks.TryGetValue(_curReadingBook, out var value))
		{
			value.Initialize();
			AddElement_ReadingBooks(_curReadingBook, ref value, context);
		}
		MainAttributes currMainAttributes = _taiwuChar.GetCurrMainAttributes();
		int sutraReadingRoomBuffValue = DomainManager.Building.GetSutraReadingRoomBuffValue();
		int num2 = readingStrategyItem.IntelligenceCost * sutraReadingRoomBuffValue / 100 + value.GetPageIntCostChange(pageIndex);
		short num3 = currMainAttributes.Items[5];
		if (num2 > num3)
		{
			throw new Exception($"You don't have enough intelligence: {num2} needed, current int is {num3}");
		}
		if (num2 > 0)
		{
			_taiwuChar.ChangeCurrMainAttribute(context, 5, -num2);
		}
		if (readingStrategyItem.DurabilityCost > 0)
		{
			element_SkillBooks.SetCurrDurability((short)(element_SkillBooks.GetCurrDurability() - readingStrategyItem.DurabilityCost), context);
		}
		if (element_SkillBooks.IsCombatSkillBook())
		{
			ApplyImmediateReadingStrategyEffectForCombatSkill(context, element_SkillBooks, pageIndex, ref value, strategyId);
		}
		else
		{
			ApplyImmediateReadingStrategyEffectForLifeSkill(context, element_SkillBooks, pageIndex, ref value, strategyId);
		}
		if (readingStrategyItem.ClearPageStrategies)
		{
			value.ClearPageStrategies(pageIndex);
			DomainManager.Extra.ClearReadingStrategiesExpireTimeForPage(context, _curReadingBook, pageIndex);
		}
		sbyte efficiencyBonus = (sbyte)((readingStrategyItem.MaxCurrPageEfficiencyChange > readingStrategyItem.MinCurrPageEfficiencyChange) ? context.Random.Next((int)readingStrategyItem.MinCurrPageEfficiencyChange, readingStrategyItem.MaxCurrPageEfficiencyChange + 1) : readingStrategyItem.MaxCurrPageEfficiencyChange);
		value.SetPageStrategy(pageIndex, strategyIndex, strategyId, efficiencyBonus);
		SetElement_ReadingBooks(_curReadingBook, ref value, context);
		DomainManager.Extra.SetReadingStrategiesExpireTime(context, _curReadingBook, pageIndex, strategyIndex, strategyId);
		if (element_SkillBooks.GetCurrDurability() <= 0)
		{
			_taiwuChar.RemoveInventoryItem(context, _curReadingBook, 1, deleteItem: true);
			SetReadingBook(context, ItemKey.Invalid);
		}
		_currentReadingEventCostedIntelligence += num2;
	}

	private sbyte GetStrategyProgressAddValue(IRandomSource random, ReadingStrategyItem strategyCfg)
	{
		return (sbyte)((strategyCfg.MaxProgressAddValue > strategyCfg.MinProgressAddValue) ? random.Next((int)strategyCfg.MinProgressAddValue, strategyCfg.MaxProgressAddValue + 1) : strategyCfg.MaxProgressAddValue);
	}

	private void ApplyImmediateReadingStrategyEffectForCombatSkill(DataContext context, GameData.Domains.Item.SkillBook book, byte pageIndex, ref ReadingBookStrategies strategies, sbyte strategyId)
	{
		short combatSkillTemplateId = book.GetCombatSkillTemplateId();
		TaiwuCombatSkill taiwuCombatSkill = GetTaiwuCombatSkill(combatSkillTemplateId);
		byte pageTypes = book.GetPageTypes();
		sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
		byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(outlinePageType, SkillBookStateHelper.GetNormalPageType(pageTypes, pageIndex), pageIndex);
		ReadingStrategyItem readingStrategyItem = ReadingStrategy.Instance[strategyId];
		ushort pageIncompleteState = book.GetPageIncompleteState();
		if (readingStrategyItem.MaxProgressAddValue != 0)
		{
			sbyte strategyProgressAddValue = GetStrategyProgressAddValue(context.Random, readingStrategyItem);
			bool flag = OfflineAddReadingProgress(taiwuCombatSkill, pageInternalIndex, strategyProgressAddValue);
			SetTaiwuCombatSkill(context, combatSkillTemplateId, taiwuCombatSkill);
			if (flag)
			{
				SetCombatSkillPageComplete(context, book, pageInternalIndex);
			}
		}
		ApplyNextPageProgressAddValueEffectForCombatSkill(context, book, pageIndex, readingStrategyItem, taiwuCombatSkill, pageInternalIndex);
		if (strategyId == 4)
		{
			ApplyDoubleCurrentPageStrategyAddValues(context, book, taiwuCombatSkill, ref strategies, pageIndex, pageInternalIndex);
		}
		if (strategyId == 5)
		{
			ApplyDoubleCurrentPageStrategyEfficiencyChange(context, book.GetItemKey(), ref strategies, pageIndex);
		}
		if (strategyId == 14)
		{
			ApplyIntGainAccordingToStrategies(context, strategies, pageIndex);
		}
	}

	private void ApplyNextPageProgressAddValueEffectForCombatSkill(DataContext context, GameData.Domains.Item.SkillBook book, byte pageIndex, ReadingStrategyItem strategyCfg, TaiwuCombatSkill taiwuCombatSkill, byte pageInternalIndex)
	{
		byte pageTypes = book.GetPageTypes();
		ushort pageIncompleteState = book.GetPageIncompleteState();
		short combatSkillTemplateId = book.GetCombatSkillTemplateId();
		if (strategyCfg.NextPageProgressAddValue == 0 || taiwuCombatSkill.GetBookPageReadingProgress(pageInternalIndex) != 100)
		{
			return;
		}
		for (byte b = (byte)(pageIndex + 1); b < 6; b++)
		{
			sbyte normalPageType = SkillBookStateHelper.GetNormalPageType(pageTypes, b);
			byte normalPageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(normalPageType, b);
			int pageIncompleteState2 = DomainManager.Item.GetPageIncompleteState(pageIncompleteState, b, _referenceBooks, _curReadingBook);
			if (taiwuCombatSkill.GetBookPageReadingProgress(normalPageInternalIndex) < 100 && pageIncompleteState2 != 2)
			{
				bool flag = OfflineAddReadingProgress(taiwuCombatSkill, normalPageInternalIndex, strategyCfg.NextPageProgressAddValue);
				SetTaiwuCombatSkill(context, combatSkillTemplateId, taiwuCombatSkill);
				if (flag)
				{
					SetCombatSkillPageComplete(context, book, normalPageInternalIndex);
				}
				break;
			}
		}
	}

	private void ApplyImmediateReadingStrategyEffectForLifeSkill(DataContext context, GameData.Domains.Item.SkillBook book, byte pageIndex, ref ReadingBookStrategies strategies, sbyte strategyId)
	{
		short lifeSkillTemplateId = book.GetLifeSkillTemplateId();
		TaiwuLifeSkill taiwuLifeSkill = GetTaiwuLifeSkill(lifeSkillTemplateId);
		ReadingStrategyItem readingStrategyItem = ReadingStrategy.Instance[strategyId];
		ushort pageIncompleteState = book.GetPageIncompleteState();
		if (readingStrategyItem.MaxProgressAddValue != 0)
		{
			sbyte delta = (sbyte)((readingStrategyItem.MaxProgressAddValue > readingStrategyItem.MinProgressAddValue) ? context.Random.Next((int)readingStrategyItem.MinProgressAddValue, readingStrategyItem.MaxProgressAddValue + 1) : readingStrategyItem.MaxProgressAddValue);
			bool flag = OfflineAddReadingProgress(taiwuLifeSkill, pageIndex, delta);
			SetTaiwuLifeSkill(context, lifeSkillTemplateId, taiwuLifeSkill);
			if (flag)
			{
				SetLifeSkillPageComplete(context, book, pageIndex);
			}
		}
		ApplyNextPageProgressAddValueEffectForLifeSkill(context, book, pageIndex, readingStrategyItem, taiwuLifeSkill);
		if (strategyId == 4)
		{
			ApplyDoubleCurrentPageStrategyAddValues(context, book, taiwuLifeSkill, ref strategies, pageIndex);
		}
		if (strategyId == 5)
		{
			ApplyDoubleCurrentPageStrategyEfficiencyChange(context, book.GetItemKey(), ref strategies, pageIndex);
		}
		if (strategyId == 14)
		{
			ApplyIntGainAccordingToStrategies(context, strategies, pageIndex);
		}
	}

	private void ApplyNextPageProgressAddValueEffectForLifeSkill(DataContext context, GameData.Domains.Item.SkillBook book, byte startPageIndex, ReadingStrategyItem strategyCfg, TaiwuLifeSkill taiwuLifeSkill)
	{
		if (strategyCfg.NextPageProgressAddValue == 0 || taiwuLifeSkill.GetBookPageReadingProgress(startPageIndex) != 100)
		{
			return;
		}
		ushort pageIncompleteState = book.GetPageIncompleteState();
		short lifeSkillTemplateId = book.GetLifeSkillTemplateId();
		for (byte b = (byte)(startPageIndex + 1); b < 5; b++)
		{
			int pageIncompleteState2 = DomainManager.Item.GetPageIncompleteState(pageIncompleteState, b, _referenceBooks, _curReadingBook);
			if (taiwuLifeSkill.GetBookPageReadingProgress(b) < 100 && pageIncompleteState2 != 2)
			{
				bool flag = OfflineAddReadingProgress(taiwuLifeSkill, b, strategyCfg.NextPageProgressAddValue);
				SetTaiwuLifeSkill(context, lifeSkillTemplateId, taiwuLifeSkill);
				if (flag)
				{
					SetLifeSkillPageComplete(context, book, b);
				}
				break;
			}
		}
	}

	private void ApplyDoubleCurrentPageStrategyAddValues(DataContext context, GameData.Domains.Item.SkillBook book, TaiwuCombatSkill taiwuCombatSkill, ref ReadingBookStrategies strategies, byte pageIndex, byte pageInternalIndex)
	{
		bool flag = false;
		for (int i = 0; i < 3; i++)
		{
			if (flag)
			{
				break;
			}
			sbyte pageStrategy = strategies.GetPageStrategy(pageIndex, i);
			if (pageStrategy >= 0)
			{
				ReadingStrategyItem readingStrategyItem = ReadingStrategy.Instance[pageStrategy];
				if (readingStrategyItem.MaxProgressAddValue != 0)
				{
					sbyte strategyProgressAddValue = GetStrategyProgressAddValue(context.Random, readingStrategyItem);
					flag = OfflineAddReadingProgress(taiwuCombatSkill, pageInternalIndex, strategyProgressAddValue);
				}
				if (readingStrategyItem.NextPageProgressAddValue != 0)
				{
					ApplyNextPageProgressAddValueEffectForCombatSkill(context, book, pageIndex, readingStrategyItem, taiwuCombatSkill, pageInternalIndex);
				}
			}
		}
		SetTaiwuCombatSkill(context, book.GetCombatSkillTemplateId(), taiwuCombatSkill);
		if (flag)
		{
			SetCombatSkillPageComplete(context, book, pageInternalIndex);
		}
	}

	private void ApplyDoubleCurrentPageStrategyAddValues(DataContext context, GameData.Domains.Item.SkillBook book, TaiwuLifeSkill taiwuLifeSkill, ref ReadingBookStrategies strategies, byte pageIndex)
	{
		bool flag = false;
		for (int i = 0; i < 3; i++)
		{
			if (flag)
			{
				break;
			}
			sbyte pageStrategy = strategies.GetPageStrategy(pageIndex, i);
			if (pageStrategy >= 0)
			{
				ReadingStrategyItem readingStrategyItem = ReadingStrategy.Instance[pageStrategy];
				if (readingStrategyItem.MaxProgressAddValue != 0)
				{
					sbyte strategyProgressAddValue = GetStrategyProgressAddValue(context.Random, readingStrategyItem);
					flag = OfflineAddReadingProgress(taiwuLifeSkill, pageIndex, strategyProgressAddValue);
				}
				if (readingStrategyItem.NextPageProgressAddValue != 0)
				{
					ApplyNextPageProgressAddValueEffectForLifeSkill(context, book, pageIndex, readingStrategyItem, taiwuLifeSkill);
				}
			}
		}
		SetTaiwuLifeSkill(context, book.GetLifeSkillTemplateId(), taiwuLifeSkill);
		if (flag)
		{
			SetLifeSkillPageComplete(context, book, pageIndex);
		}
	}

	private unsafe void ApplyDoubleCurrentPageStrategyEfficiencyChange(DataContext context, ItemKey bookItemKey, ref ReadingBookStrategies strategies, byte pageIndex)
	{
		for (int i = 0; i < 3; i++)
		{
			int value = strategies.Bonus[pageIndex * 3 + i] * 2;
			strategies.Bonus[pageIndex * 3 + i] = (sbyte)Math.Clamp(value, 0, 100);
		}
		SetElement_ReadingBooks(bookItemKey, ref strategies, context);
	}

	private void ApplyIntGainAccordingToStrategies(DataContext context, ReadingBookStrategies strategies, byte pageIndex)
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			sbyte pageStrategy = strategies.GetPageStrategy(pageIndex, i);
			if (pageStrategy >= 0)
			{
				ReadingStrategyItem readingStrategyItem = ReadingStrategy.Instance[pageStrategy];
				if (readingStrategyItem.IntelligenceCost > 0)
				{
					num += readingStrategyItem.IntelligenceCost / 2;
				}
			}
		}
		_taiwuChar.ChangeCurrMainAttribute(context, 5, num);
	}

	[DomainMethod]
	public void ClearPageStrategy(DataContext context, byte pageIndex)
	{
		if (!_curReadingBook.IsValid())
		{
			throw new Exception($"CurrentReadingBook is invalid {_curReadingBook}");
		}
		short itemSubType = ItemTemplateHelper.GetItemSubType(_curReadingBook.ItemType, _curReadingBook.TemplateId);
		int num = ((itemSubType == 1001) ? 6 : 5);
		if (pageIndex < 0 || pageIndex >= num)
		{
			throw new Exception($"the given page index {pageIndex} is illegal.");
		}
		ReadingBookStrategies value = _readingBooks[_curReadingBook];
		value.ClearPageStrategies(pageIndex);
		SetElement_ReadingBooks(_curReadingBook, ref value, context);
		DomainManager.Extra.ClearReadingStrategiesExpireTimeForPage(context, _curReadingBook, pageIndex);
	}

	[DomainMethod]
	[Obsolete]
	public List<byte> GetRandomSelectableStrategies(DataContext context, byte pageIndex)
	{
		return null;
	}

	public bool IsCombatSkillBookPageRead(GameData.Domains.Item.SkillBook book, byte curReadingPage)
	{
		short combatSkillTemplateId = book.GetCombatSkillTemplateId();
		byte pageTypes = book.GetPageTypes();
		sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
		byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(outlinePageType, SkillBookStateHelper.GetNormalPageType(pageTypes, curReadingPage), curReadingPage);
		if (!_taiwuChar.GetLearnedCombatSkills().Contains(combatSkillTemplateId))
		{
			return false;
		}
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_taiwuCharId, combatSkillTemplateId));
		ushort readingState = element_CombatSkills.GetReadingState();
		return CombatSkillStateHelper.IsPageRead(readingState, pageInternalIndex);
	}

	public bool IsLifeSkillBookPageRead(GameData.Domains.Item.SkillBook book, byte curReadingPage)
	{
		short lifeSkillTemplateId = book.GetLifeSkillTemplateId();
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = _taiwuChar.GetLearnedLifeSkills();
		int num = _taiwuChar.FindLearnedLifeSkillIndex(lifeSkillTemplateId);
		if (num < 0)
		{
			return false;
		}
		return learnedLifeSkills[num].IsPageRead(curReadingPage);
	}

	public sbyte GetBaseReadingSpeed(byte curReadingPage)
	{
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
		int pageIncompleteState = DomainManager.Item.GetPageIncompleteState(element_SkillBooks.GetPageIncompleteState(), curReadingPage, _referenceBooks, _curReadingBook);
		return SkillBookPageIncompleteState.BaseReadingSpeed[pageIncompleteState];
	}

	public int GetReadingSpeedBonus(byte curReadingPage, bool isInBattle = false, int tempPageReadStatus = 0, short skillAttainment = 0)
	{
		if (!_curReadingBook.IsValid())
		{
			return 0;
		}
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
		if (!_readingBooks.TryGetValue(_curReadingBook, out var value))
		{
			value.Initialize();
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		if (SkillGroup.FromItemSubType(element_SkillBooks.GetItemSubType()) == 0)
		{
			short lifeSkillTemplateId = element_SkillBooks.GetLifeSkillTemplateId();
			sbyte lifeSkillType = element_SkillBooks.GetLifeSkillType();
			List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = _taiwuChar.GetLearnedLifeSkills();
			int num6 = _taiwuChar.FindLearnedLifeSkillIndex(lifeSkillTemplateId);
			if (num6 >= 0)
			{
				GameData.Domains.Character.LifeSkillItem lifeSkillItem = learnedLifeSkills[num6];
				for (byte b = 0; b < curReadingPage; b++)
				{
					if (!lifeSkillItem.IsPageRead(b) && (tempPageReadStatus & (1 << (int)b)) == 0)
					{
						num++;
					}
				}
			}
			else
			{
				for (byte b2 = 0; b2 < curReadingPage; b2++)
				{
					if ((tempPageReadStatus & (1 << (int)b2)) == 0)
					{
						num++;
					}
				}
			}
			if (skillAttainment <= 0)
			{
				skillAttainment = _taiwuChar.GetLifeSkillAttainment(lifeSkillType);
			}
			foreach (short featureId in _taiwuChar.GetFeatureIds())
			{
				CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[featureId];
				num2 += characterFeatureItem.LifeSkillBookReadEfficiency;
			}
			num4 = value.GetPageReadingEfficiencyBonus(curReadingPage);
			Location location = _taiwuChar.GetLocation();
			num5 += DomainManager.Building.GetBuildingBlockEffect(location, EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor, lifeSkillType);
		}
		else
		{
			short combatSkillTemplateId = element_SkillBooks.GetCombatSkillTemplateId();
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkillTemplateId];
			byte pageTypes = element_SkillBooks.GetPageTypes();
			sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
			sbyte normalPageType = SkillBookStateHelper.GetNormalPageType(pageTypes, curReadingPage);
			byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(outlinePageType, normalPageType, curReadingPage);
			if (_taiwuChar.GetLearnedCombatSkills().Contains(combatSkillTemplateId))
			{
				GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_taiwuCharId, combatSkillTemplateId));
				ushort readingState = element_CombatSkills.GetReadingState();
				for (byte b3 = 0; b3 < curReadingPage; b3++)
				{
					byte pageInternalIndex2 = CombatSkillStateHelper.GetPageInternalIndex(outlinePageType, SkillBookStateHelper.GetNormalPageType(pageTypes, b3), b3);
					if (!CombatSkillStateHelper.IsPageRead(readingState, pageInternalIndex2) && (tempPageReadStatus & (1 << (int)b3)) == 0)
					{
						num++;
					}
				}
			}
			else
			{
				for (byte b4 = 0; b4 < curReadingPage; b4++)
				{
					if ((tempPageReadStatus & (1 << (int)b4)) == 0)
					{
						num++;
					}
				}
			}
			skillAttainment = _taiwuChar.GetCombatSkillAttainment(element_SkillBooks.GetCombatSkillType());
			short readingAttainmentRequirement = SkillGradeData.Instance[element_SkillBooks.GetGrade()].ReadingAttainmentRequirement;
			skillAttainment = GetAttainmentWithSectApprovalBonus(combatSkillItem.SectId, skillAttainment, readingAttainmentRequirement);
			foreach (short featureId2 in _taiwuChar.GetFeatureIds())
			{
				CharacterFeatureItem characterFeatureItem2 = CharacterFeature.Instance[featureId2];
				num2 += characterFeatureItem2.CombatSkillBookReadEfficiency;
			}
			num4 = value.GetPageReadingEfficiencyBonus(curReadingPage);
			sbyte sectId = Config.CombatSkill.Instance[element_SkillBooks.GetCombatSkillTemplateId()].SectId;
			num3 = CalcReadingSpeedSectApprovalFactor(sectId, (curReadingPage == 0) ? SkillBookStateHelper.GetOutlinePageType(pageTypes) : normalPageType, (sbyte)curReadingPage, isInBattle);
			Location location2 = _taiwuChar.GetLocation();
			num5 += DomainManager.Building.GetBuildingBlockEffect(location2, EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor, combatSkillItem.Type);
		}
		int num7 = CalcReadingSpeedAttainmentFactor(skillAttainment, element_SkillBooks.GetGrade());
		int num8 = CalcReferenceBooksBonusSpeedPercent();
		byte readingDifficulty = DomainManager.Extra.GetReadingDifficulty();
		short num9 = WorldCreation.Instance[(byte)8].InfluenceFactors[readingDifficulty];
		int num10 = 0;
		if (DomainManager.Extra.IsProfessionalSkillUnlocked(4, 1))
		{
			int seniority = DomainManager.Extra.GetProfessionData(4).Seniority;
			int num11 = 3000000;
			num10 = 30 + 30 * seniority / num11;
		}
		int num12 = 100 + num2 + num5 + num8 + num4 + num10;
		num12 = num12 * (100 + num7) / 100;
		num12 = num12 * (100 + num3) / 100;
		num12 = num12 * num9 / 100;
		for (int i = 0; i < num; i++)
		{
			num12 /= 2;
		}
		return Math.Max(num12, 0);
	}

	public static int CalcReadingSpeedAttainmentFactor(short attainment, sbyte grade)
	{
		return Math.Clamp(100 * attainment / SkillGradeData.Instance[grade].ReadingAttainmentRequirement, 10, 1000);
	}

	private int CalcReferenceBooksBonusSpeedPercent()
	{
		int num = 0;
		ItemKey[] referenceBooks = _referenceBooks;
		for (int i = 0; i < referenceBooks.Length; i++)
		{
			ItemKey refBookKey = referenceBooks[i];
			if (refBookKey.IsValid())
			{
				num += GetRefBonusSpeed(refBookKey);
			}
		}
		return Math.Max(0, num);
	}

	[DomainMethod]
	public int GetRefBonusSpeed(ItemKey refBookKey)
	{
		if (!_curReadingBook.IsValid())
		{
			return 0;
		}
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
		sbyte b = ((element_SkillBooks.GetItemSubType() == 1000) ? element_SkillBooks.GetLifeSkillType() : element_SkillBooks.GetCombatSkillType());
		GameData.Domains.Item.SkillBook element_SkillBooks2 = DomainManager.Item.GetElement_SkillBooks(refBookKey.Id);
		sbyte grade = element_SkillBooks2.GetGrade();
		int num = 15;
		if (CheckRefBonusSpeedIsSpecial(refBookKey))
		{
			num = 30;
		}
		sbyte b2 = ((element_SkillBooks2.GetItemSubType() == 1000) ? element_SkillBooks2.GetLifeSkillType() : element_SkillBooks2.GetCombatSkillType());
		if (element_SkillBooks2.GetItemSubType() == element_SkillBooks.GetItemSubType() && b2 == b)
		{
			num = 30;
		}
		return GlobalConfig.Instance.BaseRefBonusSpeed + (grade + 1) * num;
	}

	private bool CheckRefBonusSpeedIsSpecial(ItemKey refBookKey)
	{
		if (!_curReadingBook.IsValid())
		{
			return false;
		}
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
		return element_SkillBooks.GetReferenceBooksWithBonus()?.Contains(refBookKey.TemplateId) ?? false;
	}

	[DomainMethod]
	public void CheckNotInInventoryBooks(DataContext context)
	{
		Inventory inventory = _taiwuChar.GetInventory();
		for (sbyte b = 0; b < _referenceBooks.Length; b++)
		{
			if (_referenceBooks[b].IsValid() && !inventory.Items.ContainsKey(_referenceBooks[b]))
			{
				SetReferenceBook(context, b, ItemKey.Invalid);
			}
			else if (!IsReferenceBookSlotUnlocked(b))
			{
				SetReferenceBook(context, b, ItemKey.Invalid);
			}
		}
		if (_curReadingBook.IsValid() && !inventory.Items.ContainsKey(_curReadingBook))
		{
			SetReadingBook(context, ItemKey.Invalid);
		}
	}

	public bool IsReferenceBookSlotUnlocked(int index)
	{
		byte referenceBookSlotUnlockStates = GetReferenceBookSlotUnlockStates();
		return ((referenceBookSlotUnlockStates >> index) & 1) == 1;
	}

	public bool UpdateReadingProgressOnce(DataContext context, bool isInCombat, bool addInstantNotification = true)
	{
		CheckNotInInventoryBooks(context);
		if (!_curReadingBook.IsValid())
		{
			return false;
		}
		ReadingBookStrategies curReadingStrategies = GetCurReadingStrategies();
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
		ItemKey itemKey = element_SkillBooks.GetItemKey();
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		if (isInCombat)
		{
			Events.RegisterHandler_CombatEnd(OnCombatEndTryAttachPoisonByReadInCombat);
		}
		else
		{
			ApplyAttachedPoisonForReading(context);
		}
		if (SkillGroup.FromItemSubType(element_SkillBooks.GetItemSubType()) == 0)
		{
			int value = UpdateLifeSkillBookReadingProgress(context, element_SkillBooks, curReadingStrategies, isInCombat);
			sbyte readInLifeSkillCombatCount = DomainManager.Extra.GetReadInLifeSkillCombatCount();
			if (addInstantNotification)
			{
				if (readInLifeSkillCombatCount <= 0)
				{
					instantNotificationCollection.AddReadInLifeSkillCombatNoChance(itemKey.ItemType, itemKey.TemplateId, value);
				}
				else
				{
					instantNotificationCollection.AddReadInLifeSkillCombat(itemKey.ItemType, itemKey.TemplateId, value, readInLifeSkillCombatCount);
				}
			}
		}
		else
		{
			int value2 = UpdateCombatSkillBookReadingProgress(context, element_SkillBooks, curReadingStrategies, isInCombat);
			sbyte readInCombatCount = DomainManager.Taiwu.GetReadInCombatCount();
			if (addInstantNotification)
			{
				if (readInCombatCount <= 0)
				{
					instantNotificationCollection.AddReadInCombatNoChance(itemKey.ItemType, itemKey.TemplateId, value2);
				}
				else
				{
					instantNotificationCollection.AddReadInCombat(itemKey.ItemType, itemKey.TemplateId, value2, readInCombatCount);
				}
			}
		}
		int num = 20;
		if (isInCombat)
		{
			Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
			if (location.IsValid())
			{
				AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
				if (adventuresInArea.AdventureSites.TryGetValue(location.BlockId, out var value3) && value3.SiteState >= 2 && value3.TemplateId == 38)
				{
					num += 20;
				}
			}
		}
		for (int i = 0; i < _referenceBooks.Length; i++)
		{
			if (_referenceBooks[i].IsValid())
			{
				GameData.Domains.Item.SkillBook element_SkillBooks2 = DomainManager.Item.GetElement_SkillBooks(_referenceBooks[i].Id);
				if (SkillGroup.FromItemSubType(element_SkillBooks2.GetItemSubType()) == 0)
				{
					num += LifeSkill.Instance[element_SkillBooks2.GetLifeSkillTemplateId()].ReadingEventBonusRate;
				}
			}
		}
		if (_curReadingBook.IsValid() && (num >= 100 || context.Random.CheckProb(num, 100)))
		{
			return true;
		}
		return false;
	}

	public bool UpdateReadingProgressInCombat(DataContext context)
	{
		return UpdateReadingProgressOnce(context, isInCombat: true);
	}

	private void OnCombatEndTryAttachPoisonByReadInCombat(DataContext context)
	{
		ApplyAttachedPoisonForReading(context);
		Events.UnRegisterHandler_CombatEnd(OnCombatEndTryAttachPoisonByReadInCombat);
	}

	private void ApplyAttachedPoisonForReading(DataContext context)
	{
		if (_curReadingBook.IsValid())
		{
			GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
			_taiwuChar.TryApplyAttachedPoison(context, element_SkillBooks.GetItemKey());
		}
	}

	public void UpdateReadingProgressOnMonthChange(DataContext context)
	{
		SetReadInCombatCount(1, context);
		DomainManager.Extra.SetReadInLifeSkillCombatCount(1, context);
		short currReadingEventBonusRate = GetCurrReadingEventBonusRate();
		MakeReadingProgressActuallyWork(context);
		List<int> readingEventBookIdList = DomainManager.Extra.GetReadingEventBookIdList();
		readingEventBookIdList.Clear();
		DomainManager.Extra.SetReadingEventBookIdList(readingEventBookIdList, context);
		if (_curReadingBook.IsValid() && (currReadingEventBonusRate >= 100 || context.Random.CheckProb(currReadingEventBonusRate, 100)))
		{
			DomainManager.World.GetMonthlyNotificationCollection().AddReadingEvent((ulong)_curReadingBook);
			DomainManager.Extra.AddReadingEventBookId(context, _curReadingBook.Id);
		}
	}

	public void MakeReadingProgressActuallyWork(DataContext context, int activeReadProgressAffectedEfficiency = 100)
	{
		CheckNotInInventoryBooks(context);
		if (!_curReadingBook.IsValid() || !DomainManager.SpecialEffect.ModifyData(_taiwuCharId, -1, 260, dataValue: true))
		{
			return;
		}
		ReadingBookStrategies curReadingStrategies = GetCurReadingStrategies();
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
		_taiwuChar.TryApplyAttachedPoison(context, element_SkillBooks.GetItemKey());
		if (SkillGroup.FromItemSubType(element_SkillBooks.GetItemSubType()) == 0)
		{
			UpdateLifeSkillBookReadingProgress(context, element_SkillBooks, curReadingStrategies, isInBattle: false, activeReadProgressAffectedEfficiency);
		}
		else
		{
			UpdateCombatSkillBookReadingProgress(context, element_SkillBooks, curReadingStrategies, isInBattle: false, activeReadProgressAffectedEfficiency);
		}
		short currReadingEventBonusRate = GetCurrReadingEventBonusRate();
		for (int i = 0; i < _referenceBooks.Length; i++)
		{
			if (_referenceBooks[i].IsValid())
			{
				GameData.Domains.Item.SkillBook element_SkillBooks2 = DomainManager.Item.GetElement_SkillBooks(_referenceBooks[i].Id);
				_taiwuChar.TryApplyAttachedPoison(context, element_SkillBooks2.GetItemKey());
				element_SkillBooks2.SetCurrDurability((short)(element_SkillBooks2.GetCurrDurability() - 1), context);
				if (element_SkillBooks2.GetCurrDurability() <= 0)
				{
					_taiwuChar.RemoveInventoryItem(context, _referenceBooks[i], 1, deleteItem: true);
					_referenceBooks[i] = ItemKey.Invalid;
				}
			}
		}
		SetReferenceBooks(_referenceBooks, context);
		element_SkillBooks.SetCurrDurability((short)(element_SkillBooks.GetCurrDurability() - 1), context);
		if (element_SkillBooks.GetCurrDurability() <= 0)
		{
			_taiwuChar.RemoveInventoryItem(context, _curReadingBook, 1, deleteItem: true);
			SetReadingBook(context, ItemKey.Invalid);
		}
	}

	public void ClearReadingStrategy(DataContext context, ItemKey book, byte pageIndex, int strategyIndex)
	{
		if (_readingBooks.TryGetValue(book, out var value))
		{
			value.SetPageStrategy(pageIndex, strategyIndex, -1, 0);
			SetElement_ReadingBooks(book, ref value, context);
		}
	}

	private TaiwuLifeSkill GetTaiwuLifeSkill(short skillTemplateId)
	{
		if (TryGetElement_LifeSkills(skillTemplateId, out var value))
		{
			return value;
		}
		if (TryGetElement_NotLearnLifeSkillReadingProgress(skillTemplateId, out value))
		{
			return value;
		}
		return new TaiwuLifeSkill();
	}

	private void SetTaiwuLifeSkill(DataContext context, short skillTemplateId, TaiwuLifeSkill taiwuLifeSkill)
	{
		if (_lifeSkills.ContainsKey(skillTemplateId))
		{
			SetElement_LifeSkills(skillTemplateId, taiwuLifeSkill, context);
		}
		else if (_notLearnLifeSkillReadingProgress.ContainsKey(skillTemplateId))
		{
			SetElement_NotLearnLifeSkillReadingProgress(skillTemplateId, taiwuLifeSkill, context);
		}
		else
		{
			AddElement_NotLearnLifeSkillReadingProgress(skillTemplateId, taiwuLifeSkill, context);
		}
	}

	private TaiwuCombatSkill GetTaiwuCombatSkill(short skillTemplateId)
	{
		if (TryGetElement_CombatSkills(skillTemplateId, out var value))
		{
			return value;
		}
		if (TryGetElement_NotLearnCombatSkillReadingProgress(skillTemplateId, out value))
		{
			return value;
		}
		return new TaiwuCombatSkill();
	}

	private void SetTaiwuCombatSkill(DataContext context, short skillTemplateId, TaiwuCombatSkill taiwuCombatSkill)
	{
		if (_combatSkills.ContainsKey(skillTemplateId))
		{
			SetElement_CombatSkills(skillTemplateId, taiwuCombatSkill, context);
		}
		else if (_notLearnCombatSkillReadingProgress.ContainsKey(skillTemplateId))
		{
			SetElement_NotLearnCombatSkillReadingProgress(skillTemplateId, taiwuCombatSkill, context);
		}
		else
		{
			AddElement_NotLearnCombatSkillReadingProgress(skillTemplateId, taiwuCombatSkill, context);
		}
	}

	private int UpdateLifeSkillBookReadingProgress(DataContext context, GameData.Domains.Item.SkillBook book, ReadingBookStrategies strategies, bool isInBattle = false, int activeReadProgressAffectedEfficiency = 100)
	{
		short lifeSkillTemplateId = book.GetLifeSkillTemplateId();
		TaiwuLifeSkill taiwuLifeSkill = GetTaiwuLifeSkill(lifeSkillTemplateId);
		byte readingPage = GetCurrentReadingPage(book, strategies, taiwuLifeSkill);
		int num = 0;
		int remainingSpeedPercent = 100;
		SkillBookItem skillBookItem = Config.SkillBook.Instance[lifeSkillTemplateId];
		sbyte lifeSkillType = skillBookItem.LifeSkillType;
		sbyte grade = skillBookItem.Grade;
		while (remainingSpeedPercent > 0 && readingPage < 5)
		{
			if (IsLifeSkillBookPageRead(book, readingPage) || strategies.GetSkipPage(readingPage))
			{
				readingPage++;
				continue;
			}
			int num2 = (isInBattle ? DomainManager.Taiwu.CalcReadingBonusRateFactorInCombat(DomainManager.Taiwu.GetCurReadingBook()) : 100);
			int num3 = GetBaseReadingSpeed(readingPage) * GetReadingSpeedBonus(readingPage, isInBattle, 0, 0) / 100;
			num3 = num3 * activeReadProgressAffectedEfficiency / 100;
			num3 = num3 * num2 / 100;
			if (num3 == 0)
			{
				break;
			}
			(bool, int) tuple = ReadSkillBookPage(context, book, readingPage, num3, isCombatSkill: false, ref remainingSpeedPercent, ref readingPage);
			bool item = tuple.Item1;
			int item2 = tuple.Item2;
			num += item2;
			if (item)
			{
				switch (lifeSkillType)
				{
				case 13:
				{
					int baseDelta2 = ProfessionFormulaImpl.Calculate(42, grade);
					DomainManager.Extra.ChangeProfessionSeniority(context, 6, baseDelta2);
					break;
				}
				case 12:
				{
					int baseDelta = ProfessionFormulaImpl.Calculate(36, grade);
					DomainManager.Extra.ChangeProfessionSeniority(context, 5, baseDelta);
					break;
				}
				}
			}
			if (item)
			{
				continue;
			}
			break;
		}
		if (readingPage == 5)
		{
			int readPageCountByRereading = GetReadPageCountByRereading(isInBattle, remainingSpeedPercent, null, 0, 0);
			_taiwuChar.ChangeExp(context, GetExpByRereading(grade, readPageCountByRereading));
			switch (lifeSkillType)
			{
			case 13:
			{
				int baseDelta4 = ProfessionFormulaImpl.Calculate(43, grade) * readPageCountByRereading;
				DomainManager.Extra.ChangeProfessionSeniority(context, 6, baseDelta4);
				break;
			}
			case 12:
			{
				int baseDelta3 = ProfessionFormulaImpl.Calculate(37, grade) * readPageCountByRereading;
				DomainManager.Extra.ChangeProfessionSeniority(context, 5, baseDelta3);
				break;
			}
			}
		}
		return num;
	}

	private int UpdateCombatSkillBookReadingProgress(DataContext context, GameData.Domains.Item.SkillBook book, ReadingBookStrategies strategies, bool isInBattle = false, int activeReadProgressAffectedEfficiency = 100)
	{
		short combatSkillTemplateId = book.GetCombatSkillTemplateId();
		byte pageTypes = book.GetPageTypes();
		TaiwuCombatSkill taiwuCombatSkill = GetTaiwuCombatSkill(combatSkillTemplateId);
		byte readingPage = GetCurrentReadingPage(book, strategies, taiwuCombatSkill);
		int num = 0;
		int remainingSpeedPercent = 100;
		while (remainingSpeedPercent > 0 && readingPage < 6)
		{
			if (IsCombatSkillBookPageRead(book, readingPage) || strategies.GetSkipPage(readingPage))
			{
				readingPage++;
				continue;
			}
			int num2 = (isInBattle ? DomainManager.Taiwu.CalcReadingBonusRateFactorInCombat(DomainManager.Taiwu.GetCurReadingBook()) : 100);
			int num3 = GetBaseReadingSpeed(readingPage) * GetReadingSpeedBonus(readingPage, isInBattle, 0, 0) / 100;
			num3 = num3 * activeReadProgressAffectedEfficiency / 100;
			num3 = num3 * num2 / 100;
			if (num3 == 0)
			{
				break;
			}
			byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(SkillBookStateHelper.GetOutlinePageType(pageTypes), SkillBookStateHelper.GetNormalPageType(pageTypes, readingPage), readingPage);
			(bool, int) tuple = ReadSkillBookPage(context, book, pageInternalIndex, num3, isCombatSkill: true, ref remainingSpeedPercent, ref readingPage);
			bool item = tuple.Item1;
			int item2 = tuple.Item2;
			num += item2;
			if (item)
			{
				continue;
			}
			break;
		}
		if (readingPage == 6)
		{
			_taiwuChar.ChangeExp(context, GetExpByRereading(isInBattle, remainingSpeedPercent));
		}
		return num;
	}

	[DomainMethod]
	public int GetExpByRereading(bool isInBattle, int remainingSpeedPercent)
	{
		return _curReadingBook.IsValid() ? GetExpByRereading(DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id).GetGrade(), GetReadPageCountByRereading(isInBattle, remainingSpeedPercent, null, 0, 0)) : 0;
	}

	public int GetExpByRereading(sbyte grade, int pageCount)
	{
		return SkillGradeData.Instance[grade].ReadingExpGainPerPage * GlobalConfig.Instance.ReadingFinishedBookExpGainPercent * (grade + 1) / 100 * pageCount;
	}

	private int GetReadPageCountByRereading(bool isInBattle, int remainingSpeedPercent, int[] progress = null, int tempPageReadStatus = 0, short skillAttainment = 0)
	{
		if (!_curReadingBook.IsValid())
		{
			return 0;
		}
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
		byte pageCount = element_SkillBooks.GetPageCount();
		ReadingBookStrategies curReadingStrategies = GetCurReadingStrategies();
		int num = 0;
		for (byte b = 0; b < pageCount; b++)
		{
			if (!curReadingStrategies.GetSkipPage(b))
			{
				int num2 = (isInBattle ? DomainManager.Taiwu.CalcReadingBonusRateFactorInCombat(element_SkillBooks.GetItemKey()) : 100);
				int num3 = SkillBookPageIncompleteState.BaseReadingSpeed[0] * GetReadingSpeedBonus(b, isInBattle, tempPageReadStatus, skillAttainment) * num2 / 10000;
				int num4 = num3 * remainingSpeedPercent / 100;
				if (progress != null)
				{
					progress[b] += Math.Min(num4, 100);
				}
				if (num4 < 100)
				{
					break;
				}
				num++;
				remainingSpeedPercent -= 10000 / num3;
			}
		}
		return num;
	}

	[DomainMethod]
	public int[] GetReadingResult()
	{
		int[] array = new int[6];
		if (!_curReadingBook.IsValid() || GetCurrReadingBanByWug())
		{
			return array;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 100;
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
		byte pageCount = element_SkillBooks.GetPageCount();
		ReadingBookStrategies curReadingStrategies = GetCurReadingStrategies();
		bool flag = element_SkillBooks.IsCombatSkillBook();
		byte pageTypes = element_SkillBooks.GetPageTypes();
		short skillAttainment = 0;
		byte b;
		TaiwuSkill taiwuSkill;
		if (flag)
		{
			short combatSkillTemplateId = element_SkillBooks.GetCombatSkillTemplateId();
			TaiwuCombatSkill taiwuCombatSkill = GetTaiwuCombatSkill(combatSkillTemplateId);
			b = GetCurrentReadingPage(element_SkillBooks, curReadingStrategies, taiwuCombatSkill);
			taiwuSkill = taiwuCombatSkill;
		}
		else
		{
			short lifeSkillTemplateId = element_SkillBooks.GetLifeSkillTemplateId();
			TaiwuLifeSkill taiwuLifeSkill = GetTaiwuLifeSkill(lifeSkillTemplateId);
			b = GetCurrentReadingPage(element_SkillBooks, curReadingStrategies, taiwuLifeSkill);
			taiwuSkill = taiwuLifeSkill;
		}
		while (b < pageCount)
		{
			sbyte b2 = (flag ? taiwuSkill.GetBookPageReadingProgress(CombatSkillStateHelper.GetPageInternalIndex(SkillBookStateHelper.GetOutlinePageType(pageTypes), SkillBookStateHelper.GetNormalPageType(pageTypes, b), b)) : taiwuSkill.GetBookPageReadingProgress(b));
			if (b2 == 100)
			{
				num2 |= 1 << (int)b;
				num++;
			}
			else if (!curReadingStrategies.GetSkipPage(b))
			{
				if (!flag)
				{
					skillAttainment = _taiwuChar.GetPredictLifeSkillAttainment(element_SkillBooks.GetLifeSkillType(), element_SkillBooks.GetLifeSkillTemplateId(), num);
				}
				int num4 = GetBaseReadingSpeed(b) * GetReadingSpeedBonus(b, isInBattle: false, num2, skillAttainment) / 100;
				int num5 = num4 * num3 / 100;
				int num6 = Math.Min(100, num5 + b2) - b2;
				if (num4 == 0)
				{
					break;
				}
				num3 -= num6 * 100 / num4;
				array[b] = (sbyte)num6;
				if (num5 + b2 < 100)
				{
					break;
				}
				num2 |= 1 << (int)b;
				num++;
			}
			b++;
		}
		if (b == pageCount)
		{
			skillAttainment = (short)((!flag) ? _taiwuChar.GetPredictLifeSkillAttainment(element_SkillBooks.GetLifeSkillType(), element_SkillBooks.GetLifeSkillTemplateId(), num) : 0);
			GetReadPageCountByRereading(isInBattle: false, num3, array, num2, skillAttainment);
		}
		return array;
	}

	public void ReadSkillBookPageAndSetComplete(DataContext context, GameData.Domains.Item.SkillBook book, byte readingPage)
	{
		if (book.IsCombatSkillBook())
		{
			short combatSkillTemplateId = book.GetCombatSkillTemplateId();
			TaiwuCombatSkill taiwuCombatSkill = GetTaiwuCombatSkill(combatSkillTemplateId);
			byte pageTypes = book.GetPageTypes();
			sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
			sbyte normalPageType = SkillBookStateHelper.GetNormalPageType(pageTypes, readingPage);
			byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(outlinePageType, normalPageType, readingPage);
			taiwuCombatSkill.SetBookPageReadingProgress(pageInternalIndex, 100);
			SetTaiwuCombatSkill(context, combatSkillTemplateId, taiwuCombatSkill);
			SetCombatSkillPageComplete(context, book, pageInternalIndex);
		}
		else
		{
			short lifeSkillTemplateId = book.GetLifeSkillTemplateId();
			TaiwuLifeSkill taiwuLifeSkill = GetTaiwuLifeSkill(lifeSkillTemplateId);
			taiwuLifeSkill.SetBookPageReadingProgress(readingPage, 100);
			SetTaiwuLifeSkill(context, lifeSkillTemplateId, taiwuLifeSkill);
			SetLifeSkillPageComplete(context, book, readingPage);
		}
	}

	private void SetLifeSkillPageComplete(DataContext context, GameData.Domains.Item.SkillBook book, byte readingPage)
	{
		if (!_readingBooks.TryGetValue(book.GetItemKey(), out var value) || !value.PageContainsStrategy(readingPage, 12))
		{
			short readingExpGainPerPage = SkillGradeData.Instance[book.GetGrade()].ReadingExpGainPerPage;
			_taiwuChar.ChangeExp(context, readingExpGainPerPage);
		}
		short lifeSkillTemplateId = book.GetLifeSkillTemplateId();
		if (_notLearnLifeSkillReadingProgress.ContainsKey(lifeSkillTemplateId))
		{
			TaiwuLearnLifeSkill(context, lifeSkillTemplateId, (byte)(1 << (int)readingPage));
			return;
		}
		int num = _taiwuChar.FindLearnedLifeSkillIndex(lifeSkillTemplateId);
		GameData.Domains.Character.LifeSkillItem learnedLifeSkill = _taiwuChar.GetLearnedLifeSkills()[num];
		_taiwuChar.CheckLearnedBookHasUnlockedDebateStrategy(learnedLifeSkill, out var strategyTemplateId, out var _);
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[strategyTemplateId];
		Dictionary<sbyte, int> hasUnlockedDebateStrategyLearnedBookCountDict = _taiwuChar.GetHasUnlockedDebateStrategyLearnedBookCountDict(debateStrategyItem.Level);
		_taiwuChar.ReadLifeSkillPage(context, num, readingPage);
		GameData.Domains.Character.LifeSkillItem learnedLifeSkill2 = _taiwuChar.GetLearnedLifeSkills()[num];
		if (SkillBookStateHelper.GetPageIncompleteState(book.GetPageIncompleteState(), readingPage) == 0)
		{
			DomainManager.Taiwu.AddLegacyPoint(context, 15);
		}
		else
		{
			DomainManager.Taiwu.AddLegacyPoint(context, 16);
		}
		if (learnedLifeSkill2.IsAllPagesRead())
		{
			DomainManager.Taiwu.AddLegacyPoint(context, 17);
			short unlockedDebateStrategy = _taiwuChar.GetUnlockedDebateStrategy(learnedLifeSkill2, hasUnlockedDebateStrategyLearnedBookCountDict);
			if (unlockedDebateStrategy >= 0 && !_unlockedDebateStrategyList.Contains(unlockedDebateStrategy))
			{
				_unlockedDebateStrategyList.Add(unlockedDebateStrategy);
				GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.UnlockLifeSkillCombatStrategy);
			}
		}
	}

	private void SetCombatSkillPageComplete(DataContext context, GameData.Domains.Item.SkillBook book, byte internalIndex)
	{
		byte pageId = CombatSkillStateHelper.GetPageId(internalIndex);
		if (!_readingBooks.TryGetValue(book.GetItemKey(), out var value) || !value.PageContainsStrategy(pageId, 12))
		{
			short readingExpGainPerPage = SkillGradeData.Instance[book.GetGrade()].ReadingExpGainPerPage;
			_taiwuChar.ChangeExp(context, readingExpGainPerPage);
		}
		short combatSkillTemplateId = book.GetCombatSkillTemplateId();
		if (_notLearnCombatSkillReadingProgress.ContainsKey(combatSkillTemplateId))
		{
			ushort readingState = CombatSkillStateHelper.SetPageRead(0, internalIndex);
			TaiwuLearnCombatSkill(context, combatSkillTemplateId, readingState);
		}
		else
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_taiwuCharId, combatSkillTemplateId));
			element_CombatSkills.SetReadingState(CombatSkillStateHelper.SetPageRead(element_CombatSkills.GetReadingState(), internalIndex), context);
			if (SkillBookStateHelper.GetPageIncompleteState(book.GetPageIncompleteState(), pageId) == 0)
			{
				DomainManager.Taiwu.AddLegacyPoint(context, 21);
			}
			else
			{
				DomainManager.Taiwu.AddLegacyPoint(context, 22);
			}
		}
		DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, _taiwuCharId, combatSkillTemplateId, internalIndex);
	}

	private bool CheckIsCombatSkillBookFinished(GameData.Domains.Item.SkillBook book, GameData.Domains.CombatSkill.CombatSkill combatSkill)
	{
		ushort readingState = combatSkill.GetReadingState();
		byte pageTypes = book.GetPageTypes();
		sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
		for (byte b = 0; b < 6; b++)
		{
			sbyte normalPageType = SkillBookStateHelper.GetNormalPageType(pageTypes, b);
			byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(outlinePageType, normalPageType, b);
			if (!CombatSkillStateHelper.IsPageRead(readingState, pageInternalIndex))
			{
				return false;
			}
		}
		return true;
	}

	private bool OfflineAddReadingProgress(TaiwuSkill skill, byte readingPage, int delta)
	{
		sbyte bookPageReadingProgress = skill.GetBookPageReadingProgress(readingPage);
		bookPageReadingProgress = (sbyte)Math.Min(100, bookPageReadingProgress + delta);
		skill.SetBookPageReadingProgress(readingPage, bookPageReadingProgress);
		return bookPageReadingProgress == 100;
	}

	public byte GetCurrentReadingPage(GameData.Domains.Item.SkillBook book, ReadingBookStrategies strategies, TaiwuCombatSkill combatSkill)
	{
		sbyte[] allBookPageReadingProgress = combatSkill.GetAllBookPageReadingProgress();
		byte pageTypes = book.GetPageTypes();
		sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
		byte outlinePageInternalIndex = CombatSkillStateHelper.GetOutlinePageInternalIndex(outlinePageType);
		if (allBookPageReadingProgress[outlinePageInternalIndex] < 100 && !strategies.GetSkipPage(0))
		{
			return 0;
		}
		for (byte b = 1; b < 6; b++)
		{
			outlinePageType = SkillBookStateHelper.GetNormalPageType(pageTypes, b);
			outlinePageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(outlinePageType, b);
			if (allBookPageReadingProgress[outlinePageInternalIndex] < 100 && !strategies.GetSkipPage(b))
			{
				return b;
			}
		}
		return 6;
	}

	public byte GetCurrentReadingPage(GameData.Domains.Item.SkillBook book, ReadingBookStrategies strategies, TaiwuLifeSkill lifeSkill)
	{
		sbyte[] allBookPageReadingProgress = lifeSkill.GetAllBookPageReadingProgress();
		if (allBookPageReadingProgress[0] < 100 && !strategies.GetSkipPage(0))
		{
			return 0;
		}
		for (byte b = 1; b < allBookPageReadingProgress.Length; b++)
		{
			if (allBookPageReadingProgress[b] < 100 && !strategies.GetSkipPage(b))
			{
				return b;
			}
		}
		return 5;
	}

	[DomainMethod]
	public sbyte GetTotalReadingProgress(int bookItemId)
	{
		if (!DomainManager.Item.TryGetElement_SkillBooks(bookItemId, out var element))
		{
			return 0;
		}
		sbyte[] array;
		if (SkillGroup.FromItemSubType(element.GetItemSubType()) == 0)
		{
			short lifeSkillTemplateId = element.GetLifeSkillTemplateId();
			array = (DomainManager.Taiwu.TryGetElement_LifeSkills(lifeSkillTemplateId, out var value) ? value.GetAllBookPageReadingProgress() : ((!DomainManager.Taiwu.TryGetElement_NotLearnLifeSkillReadingProgress(lifeSkillTemplateId, out var value2)) ? new sbyte[5] : value2.GetAllBookPageReadingProgress()));
		}
		else
		{
			byte pageTypes = element.GetPageTypes();
			sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
			short lifeSkillTemplateId = element.GetCombatSkillTemplateId();
			sbyte[] array2 = null;
			TaiwuCombatSkill value4;
			if (DomainManager.Taiwu.TryGetElement_CombatSkills(lifeSkillTemplateId, out var value3))
			{
				array2 = value3.GetAllBookPageReadingProgress();
			}
			else if (DomainManager.Taiwu.TryGetElement_NotLearnCombatSkillReadingProgress(lifeSkillTemplateId, out value4))
			{
				array2 = value4.GetAllBookPageReadingProgress();
			}
			array = new sbyte[6];
			if (array2 != null)
			{
				for (byte b = 0; b < 6; b++)
				{
					sbyte normalPageType = SkillBookStateHelper.GetNormalPageType(pageTypes, b);
					array[b] = array2[CombatSkillStateHelper.GetPageInternalIndex(outlinePageType, normalPageType, b)];
				}
			}
		}
		return (sbyte)(array.Sum() / array.Length);
	}

	[DomainMethod]
	public List<sbyte> GetTotalReadingProgressList(List<int> bookItemIdList)
	{
		if (bookItemIdList == null || bookItemIdList.Count == 0)
		{
			return new List<sbyte>();
		}
		List<sbyte> list = new List<sbyte>();
		foreach (int bookItemId in bookItemIdList)
		{
			list.Add(GetTotalReadingProgress(bookItemId));
		}
		return list;
	}

	[DomainMethod]
	public bool GetCurrReadingBanByWug()
	{
		return !DomainManager.SpecialEffect.ModifyData(_taiwuCharId, -1, 260, dataValue: true);
	}

	[DomainMethod]
	public short GetCurrReadingEventBonusRate()
	{
		if (GetCurrReadingBanByWug())
		{
			return 0;
		}
		if (!_curReadingBook.IsValid())
		{
			return 0;
		}
		int num;
		if (DomainManager.TutorialChapter.InGuiding && DomainManager.TutorialChapter.GetTutorialChapter() == 6 && _curReadingBook.TemplateId == 870)
		{
			num = ((GetTotalReadingProgress(_curReadingBook.Id) < 100) ? 100 : 0);
			return (short)num;
		}
		num = 20;
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (location.IsValid())
		{
			AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
			if (adventuresInArea.AdventureSites.TryGetValue(location.BlockId, out var value) && value.SiteState >= 2 && value.TemplateId == 38)
			{
				num += 20;
			}
		}
		for (int i = 0; i < _referenceBooks.Length; i++)
		{
			if (_referenceBooks[i].IsValid())
			{
				GameData.Domains.Item.SkillBook element_SkillBooks2 = DomainManager.Item.GetElement_SkillBooks(_referenceBooks[i].Id);
				if (SkillGroup.FromItemSubType(element_SkillBooks2.GetItemSubType()) == 0)
				{
					num += LifeSkill.Instance[element_SkillBooks2.GetLifeSkillTemplateId()].ReadingEventBonusRate;
				}
			}
		}
		return (short)MathF.Min(100f, num);
	}

	[DomainMethod]
	public short GetCurrReadingEfficiency(DataContext context)
	{
		if (GetCurrReadingBanByWug())
		{
			return 0;
		}
		if (!_curReadingBook.IsValid())
		{
			return 0;
		}
		if (!_readingBooks.ContainsKey(_curReadingBook))
		{
			ReadingBookStrategies value = default(ReadingBookStrategies);
			value.Initialize();
			AddElement_ReadingBooks(_curReadingBook, ref value, context);
		}
		ReadingBookStrategies strategies = _readingBooks[_curReadingBook];
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(_curReadingBook.Id);
		short num = -1;
		byte b = 0;
		if (element_SkillBooks.IsCombatSkillBook())
		{
			num = element_SkillBooks.GetCombatSkillTemplateId();
			TaiwuCombatSkill taiwuCombatSkill = GetTaiwuCombatSkill(num);
			b = GetCurrentReadingPage(element_SkillBooks, strategies, taiwuCombatSkill);
			if (b >= 6)
			{
				b = 0;
			}
		}
		else
		{
			num = element_SkillBooks.GetLifeSkillTemplateId();
			TaiwuLifeSkill taiwuLifeSkill = GetTaiwuLifeSkill(num);
			b = GetCurrentReadingPage(element_SkillBooks, strategies, taiwuLifeSkill);
			if (b >= 5)
			{
				b = 0;
			}
		}
		int num2 = GetBaseReadingSpeed(b) * GetReadingSpeedBonus(b, isInBattle: false, 0, 0) / 100;
		return (short)num2;
	}

	[DomainMethod]
	public unsafe void ActiveReadOnce(DataContext context)
	{
		int activeReadingTimeCost = GlobalConfig.Instance.ActiveReadingTimeCost;
		MainAttributes currMainAttributes = _taiwuChar.GetCurrMainAttributes();
		short activeReadingAttributeCost = GlobalConfig.Instance.ActiveReadingAttributeCost;
		short num = currMainAttributes.Items[5];
		if (DomainManager.World.GetLeftDaysInCurrMonth() < activeReadingTimeCost)
		{
			Logger.Warn("You don't have enough days in this month");
			return;
		}
		if (activeReadingAttributeCost > num)
		{
			Logger.Warn($"You don't have enough intelligence: {activeReadingAttributeCost} needed, current int is {num}");
			return;
		}
		DomainManager.World.AdvanceDaysInMonth(context, activeReadingTimeCost);
		if (activeReadingAttributeCost > 0)
		{
			currMainAttributes[5] -= activeReadingAttributeCost;
			_taiwuChar.SetCurrMainAttributes(currMainAttributes, context);
		}
		short maxActiveReadingProgress = GlobalConfig.Instance.MaxActiveReadingProgress;
		short activeReadingProgress = DomainManager.Extra.GetActiveReadingProgress();
		int num2 = activeReadingProgress + 1;
		if (num2 <= maxActiveReadingProgress)
		{
			DomainManager.Extra.SetActiveReadingProgress((short)num2, context);
			int activeReadProgressAffectedEfficiency = GlobalConfig.Instance.ActiveReadProgressAffectedEfficiency[activeReadingProgress / 10];
			if (num2 % 10 == 0)
			{
				MakeReadingProgressActuallyWork(context, activeReadProgressAffectedEfficiency);
			}
		}
	}

	public void ClearActiveReadingProgressOnMonthChange(DataContext context)
	{
		short activeReadingProgress = DomainManager.Extra.GetActiveReadingProgress();
		int num = activeReadingProgress % 10;
		DomainManager.Extra.SetActiveReadingProgress((short)num, context);
	}

	public void GenerateAvailableReadingStrategies(DataContext context)
	{
		if (!_curReadingBook.IsValid())
		{
			return;
		}
		SByteList strategyIds = SByteList.Create();
		if (DomainManager.TutorialChapter.InGuiding && DomainManager.TutorialChapter.GetTutorialChapter() == 6 && _curReadingBook.TemplateId == 870)
		{
			for (int i = 0; i < 9; i++)
			{
				strategyIds.Items.Add(18);
			}
			DomainManager.Extra.SetAvailableReadingStrategies(context, _curReadingBook.Id, strategyIds);
			return;
		}
		List<List<sbyte>> list = new List<List<sbyte>>();
		for (int j = 0; j < 3; j++)
		{
			list.Add(new List<sbyte>());
		}
		ItemKey[] referenceBooks = _referenceBooks;
		if (referenceBooks != null && referenceBooks.Length != 0)
		{
			ItemKey[] array = referenceBooks;
			for (int k = 0; k < array.Length; k++)
			{
				ItemKey itemKey = array[k];
				if (!itemKey.IsValid())
				{
					continue;
				}
				short lifeSkillTemplateId = Config.SkillBook.Instance[itemKey.TemplateId].LifeSkillTemplateId;
				if (lifeSkillTemplateId >= 0)
				{
					sbyte b = DrawOneStrategyByBook(context.Random, lifeSkillTemplateId);
					if (b != -1)
					{
						list[ReadingStrategy.Instance[b].ExtractGroup - 1].Add(b);
					}
				}
			}
		}
		List<sbyte> list2 = new List<sbyte>();
		List<short> list3 = new List<short>();
		for (int l = 0; l < 3; l++)
		{
			list2.Clear();
			list3.Clear();
			foreach (ReadingStrategyItem item2 in (IEnumerable<ReadingStrategyItem>)ReadingStrategy.Instance)
			{
				sbyte templateId = item2.TemplateId;
				if (item2.ExtractGroup - 1 == l)
				{
					list2.Add(templateId);
					list3.Add(item2.ExtractWeight);
				}
			}
			while (list[l].Count < 3)
			{
				int randomIndex = RandomUtils.GetRandomIndex(list3, context.Random);
				sbyte item = list2[randomIndex];
				list[l].Add(item);
			}
		}
		foreach (List<sbyte> item3 in list)
		{
			strategyIds.Items.AddRange(item3);
		}
		if (DomainManager.Extra.IsProfessionalSkillUnlocked(4, 1))
		{
			int seniority = DomainManager.Extra.GetProfessionData(4).Seniority;
			int num = ProfessionData.SeniorityToExtraReadingLoopingStrategyCount(seniority);
			for (int m = 0; m < num; m++)
			{
				int num2 = context.Random.Next(ReadingStrategy.Instance.Count - 1);
				strategyIds.Items.Add((sbyte)num2);
			}
		}
		DomainManager.Extra.SetAvailableReadingStrategies(context, _curReadingBook.Id, strategyIds);
	}

	private sbyte DrawOneStrategyByBook(IRandomSource randomSource, short lifeSkillTid)
	{
		Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[lifeSkillTid];
		List<byte> providedReadingStrategies = lifeSkillItem.ProvidedReadingStrategies;
		if (providedReadingStrategies == null || providedReadingStrategies.Count == 0)
		{
			return -1;
		}
		List<short> list = new List<short>();
		foreach (byte item in providedReadingStrategies)
		{
			list.Add(ReadingStrategy.Instance[item].ExtractWeight);
		}
		int randomIndex = RandomUtils.GetRandomIndex(list, randomSource);
		return (sbyte)providedReadingStrategies[randomIndex];
	}

	[DomainMethod]
	public List<sbyte> GetCurrentBookAvailableReadingStrategies(DataContext context)
	{
		return DomainManager.Extra.GetElement_AvailableReadingStrategyMap(_curReadingBook.Id).Items;
	}

	public IEnumerable<KeyValuePair<ItemKey, ReadingBookStrategies>> GetAllReadingBooks()
	{
		return _readingBooks;
	}

	public (bool, int) ReadSkillBookPage(DataContext context, GameData.Domains.Item.SkillBook book, byte combatSkillBookInternalIndex, int speed, bool isCombatSkill, ref int remainingSpeedPercent, ref byte readingPage)
	{
		short skillTemplateId = (isCombatSkill ? book.GetCombatSkillTemplateId() : book.GetLifeSkillTemplateId());
		TaiwuCombatSkill taiwuCombatSkill = GetTaiwuCombatSkill(skillTemplateId);
		TaiwuLifeSkill taiwuLifeSkill = GetTaiwuLifeSkill(skillTemplateId);
		TaiwuSkill taiwuSkill2;
		if (!isCombatSkill)
		{
			TaiwuSkill taiwuSkill = taiwuLifeSkill;
			taiwuSkill2 = taiwuSkill;
		}
		else
		{
			TaiwuSkill taiwuSkill = taiwuCombatSkill;
			taiwuSkill2 = taiwuSkill;
		}
		TaiwuSkill taiwuSkill3 = taiwuSkill2;
		sbyte bookPageReadingProgress = taiwuSkill3.GetBookPageReadingProgress(combatSkillBookInternalIndex);
		int delta = speed * remainingSpeedPercent / 100;
		bool flag = OfflineAddReadingProgress(taiwuSkill3, combatSkillBookInternalIndex, delta);
		int num = taiwuSkill3.GetBookPageReadingProgress(combatSkillBookInternalIndex) - bookPageReadingProgress;
		if (isCombatSkill)
		{
			SetTaiwuCombatSkill(context, skillTemplateId, taiwuCombatSkill);
			if (flag)
			{
				SetCombatSkillPageComplete(context, book, combatSkillBookInternalIndex);
			}
		}
		else
		{
			SetTaiwuLifeSkill(context, skillTemplateId, taiwuLifeSkill);
			if (flag)
			{
				SetLifeSkillPageComplete(context, book, combatSkillBookInternalIndex);
			}
		}
		remainingSpeedPercent -= num * 100 / speed;
		readingPage++;
		return (flag, num);
	}

	[DomainMethod]
	public List<short> GetNewUnlockedDebateStrategyList()
	{
		if (_unlockedDebateStrategyList.Count <= 0)
		{
			return null;
		}
		List<short> result = new List<short>(_unlockedDebateStrategyList);
		_unlockedDebateStrategyList.Clear();
		return result;
	}

	[DomainMethod]
	public void GmCmd_ShowUnlockedDebateStrategy(short start, short end)
	{
		if (start < 0)
		{
			return;
		}
		if (end > start)
		{
			for (short num = start; num <= end; num++)
			{
				if (num < DebateStrategy.Instance.Count && !_unlockedDebateStrategyList.Contains(num))
				{
					_unlockedDebateStrategyList.Add(num);
				}
			}
		}
		else if (!_unlockedDebateStrategyList.Contains(start))
		{
			_unlockedDebateStrategyList.Add(start);
		}
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.UnlockLifeSkillCombatStrategy);
	}

	public void SetNeiliAllocationTypeRestriction(DataContext context, byte allocationType, bool isRestricted)
	{
		_neiliAllocationTypeRestriction = (byte)(isRestricted ? (_neiliAllocationTypeRestriction | (1 << (int)allocationType)) : (_neiliAllocationTypeRestriction & ~(1 << (int)allocationType)));
		SetNeiliAllocationTypeRestriction(_neiliAllocationTypeRestriction, context);
	}

	public bool IsNeiliAllocationTypeRestricted(byte allocationType)
	{
		return ((_neiliAllocationTypeRestriction >> (int)allocationType) & 1) == 1;
	}

	public void ClearAllTemporaryRestrictions(DataContext context)
	{
		SetNeiliAllocationTypeRestriction(0, context);
		SetHealingInnerInjuryRestriction(value: false, context);
		SetHealingOuterInjuryRestriction(value: false, context);
	}

	public unsafe short GetAttainmentWithSectApprovalBonus(sbyte orgTemplateId, short currAttainment, short requiredAttainment)
	{
		if (orgTemplateId == 0)
		{
			return currAttainment;
		}
		SectApprovingEffectItem sectApprovingEffectItem = SectApprovingEffect.Instance[orgTemplateId - 1];
		LifeSkillShorts lifeSkillAttainments = _taiwuChar.GetLifeSkillAttainments();
		short num = currAttainment;
		foreach (sbyte requirementSubstitution in sectApprovingEffectItem.RequirementSubstitutions)
		{
			if (num < lifeSkillAttainments.Items[requirementSubstitution])
			{
				num = lifeSkillAttainments.Items[requirementSubstitution];
			}
		}
		return num;
	}

	public short GetQualificationWithSectApprovalBonus(sbyte orgTemplateId, short currQualification, short requiredQualification)
	{
		sbyte bonusLifeSkillType;
		return SharedMethods.GetQualificationWithSectApprovalBonus(orgTemplateId, currQualification, _taiwuChar.GetLifeSkillQualifications(), out bonusLifeSkillType);
	}

	public unsafe short GetAttainmentWithSectApprovalBonus(sbyte orgTemplateId, short currAttainment)
	{
		if (orgTemplateId == 0)
		{
			return currAttainment;
		}
		SectApprovingEffectItem sectApprovingEffectItem = SectApprovingEffect.Instance[orgTemplateId - 1];
		LifeSkillShorts lifeSkillAttainments = _taiwuChar.GetLifeSkillAttainments();
		short num = currAttainment;
		foreach (sbyte requirementSubstitution in sectApprovingEffectItem.RequirementSubstitutions)
		{
			if (num < lifeSkillAttainments.Items[requirementSubstitution])
			{
				num = lifeSkillAttainments.Items[requirementSubstitution];
			}
		}
		return num;
	}

	public int CalcReadingSpeedSectApprovalFactor(sbyte orgTemplateId, sbyte combatSkillDirection, sbyte pageId, bool isInBattle)
	{
		if (orgTemplateId == 0)
		{
			return 0;
		}
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(orgTemplateId);
		short num = DomainManager.Organization.GetElement_Sects(settlementIdByOrgTemplateId).CalcApprovingRate();
		SectApprovingEffectItem sectApprovingEffectItem = SectApprovingEffect.Instance[orgTemplateId - 1];
		short num2 = sectApprovingEffectItem.BehaviorTypeBonuses[_taiwuChar.GetBehaviorType()];
		int num3 = ((pageId >= 1) ? sectApprovingEffectItem.CombatSkillDirectionBonuses[combatSkillDirection] : 100);
		int num4 = ((pageId < 1 || isInBattle) ? 100 : ((_taiwuChar.GetGender() == 1) ? sectApprovingEffectItem.PageBonusesOfMale[pageId - 1] : sectApprovingEffectItem.PageBonusesOfFemale[pageId - 1]));
		int num5 = num2 * num3 * num4 / 10000;
		if (num >= 400)
		{
			num5 += 25;
		}
		return num5 - 100;
	}

	public int CalcReadingBonusRateFactorInCombat(ItemKey itemKey)
	{
		SkillBookItem skillBookItem = Config.SkillBook.Instance[itemKey.TemplateId];
		if (skillBookItem == null || skillBookItem.ItemSubType != 1001)
		{
			return 100;
		}
		sbyte sectId = Config.CombatSkill.Instance[skillBookItem.CombatSkillTemplateId].SectId;
		TaiwuCombatSkill taiwuCombatSkill = GetTaiwuCombatSkill(skillBookItem.CombatSkillTemplateId);
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(itemKey.Id);
		ReadingBookStrategies curReadingStrategies = GetCurReadingStrategies();
		byte currentReadingPage = DomainManager.Taiwu.GetCurrentReadingPage(element_SkillBooks, curReadingStrategies, taiwuCombatSkill);
		sbyte combatSkillDirection = ((currentReadingPage == 0) ? SkillBookStateHelper.GetOutlinePageType(element_SkillBooks.GetPageTypes()) : SkillBookStateHelper.GetNormalPageType(element_SkillBooks.GetPageTypes(), currentReadingPage));
		return CalcReadingBonusRateFactorInCombat(sectId, combatSkillDirection, currentReadingPage);
	}

	public int CalcReadingBonusRateFactorInCombat(sbyte orgTemplateId, sbyte combatSkillDirection, byte pageId)
	{
		if (orgTemplateId == 0)
		{
			return 100;
		}
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(orgTemplateId);
		short num = DomainManager.Organization.GetElement_Sects(settlementIdByOrgTemplateId).CalcApprovingRate();
		SectApprovingEffectItem sectApprovingEffectItem = SectApprovingEffect.Instance[orgTemplateId - 1];
		return (pageId < 1) ? 100 : ((_taiwuChar.GetGender() == 1) ? sectApprovingEffectItem.ActualCombatBonusOfMale : sectApprovingEffectItem.ActualCombatBonusOfFemale);
	}

	public short GetSpiritualDebtFinalCost(short targetSettlementId, short baseCost)
	{
		if (targetSettlementId < 0)
		{
			return baseCost;
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(targetSettlementId);
		if (settlement is Sect sect)
		{
			short num = sect.CalcApprovingRate();
			if (num >= 800)
			{
				return (short)(baseCost / 2);
			}
		}
		return baseCost;
	}

	[DomainMethod]
	public List<SettlementDisplayData> GetAllVisitedSettlements()
	{
		List<SettlementDisplayData> list = new List<SettlementDisplayData>();
		foreach (short visitedSettlement in _visitedSettlements)
		{
			list.Add(DomainManager.Organization.GetDisplayData(visitedSettlement));
		}
		return list;
	}

	public void TryAddVisitedSettlement(short settlementId, DataContext context)
	{
		if (!_visitedSettlements.Contains(settlementId))
		{
			_visitedSettlements.Add(settlementId);
			SetVisitedSettlements(_visitedSettlements, context);
			Location location = DomainManager.Organization.GetSettlement(settlementId).GetLocation();
			EMapBlockType type = DomainManager.Map.GetBlock(location).GetConfig().Type;
			bool flag = (uint)type <= 1u;
			int index = (flag ? 74 : 75);
			int baseDelta = ProfessionFormula.Instance[index].Calculate();
			DomainManager.Extra.ChangeProfessionSeniority(context, 11, baseDelta);
		}
	}

	public SettlementTreasury GetTaiwuTreasury()
	{
		SettlementTreasury value;
		return DomainManager.Extra.TryGetElement_SettlementTreasuries(_taiwuVillageSettlementId, out value) ? value : new SettlementTreasury();
	}

	public void SetTaiwuTreasury(DataContext context, SettlementTreasury treasury)
	{
		DomainManager.Extra.SetSettlementTreasuries(context, _taiwuVillageSettlementId, treasury);
	}

	public void StoreItemInTreasury(DataContext context, ItemKey itemKey, int amount, bool offLine = false)
	{
		Tester.Assert(itemKey.IsValid());
		Tester.Assert(amount > 0);
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		taiwuTreasury.Inventory.OfflineAdd(itemKey, amount);
		DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Treasury, _taiwuVillageSettlementId);
		if (!offLine)
		{
			SetTaiwuTreasury(context, taiwuTreasury);
		}
		Events.RaiseTaiwuItemModified(context, itemKey);
	}

	public void TakeItemFromTreasury(DataContext context, ItemKey itemKey, int amount, bool deleteItem, bool offLine = false)
	{
		Tester.Assert(itemKey.IsValid());
		Tester.Assert(amount > 0);
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		taiwuTreasury.Inventory.OfflineRemove(itemKey, amount);
		DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Treasury, _taiwuVillageSettlementId);
		if (!offLine)
		{
			SetTaiwuTreasury(context, taiwuTreasury);
		}
		if (deleteItem)
		{
			DomainManager.Item.RemoveItem(context, itemKey);
		}
	}

	public void StoreResourceInTreasury(DataContext context, sbyte resourceType, int amount)
	{
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		taiwuTreasury.Resources.Add(resourceType, amount);
		SetTaiwuTreasury(context, taiwuTreasury);
	}

	public void TakeResourceFromTreasury(DataContext context, sbyte resourceType, int amount)
	{
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		taiwuTreasury.Resources.Subtract(resourceType, amount);
		SetTaiwuTreasury(context, taiwuTreasury);
	}

	public int GetTreasuryItemCount(ItemKey itemKey)
	{
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		taiwuTreasury.Inventory.Items.TryGetValue(itemKey, out var value);
		return value;
	}

	public void VillagerStoreItemInTreasury(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey, int amount, bool addLifeSkillRecord = true)
	{
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		int num = taiwuTreasury.CalcAdjustedWorth(baseItem.GetItemSubType(), baseItem.GetValue()) * amount;
		baseItem.SetOwner(ItemOwnerType.Treasury, _taiwuVillageSettlementId);
		taiwuTreasury.Inventory.OfflineAdd(itemKey, amount);
		taiwuTreasury.OfflineChangeContribution(character, num);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = character.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		if (num > 0 && addLifeSkillRecord)
		{
			lifeRecordCollection.AddTaiwuVillagerStorageItem(id, currDate, itemKey.ItemType, itemKey.TemplateId);
		}
		TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
		taiwuVillageStoragesRecordCollection.AddStorageItem(currDate, TaiwuVillageStorageType.Treasury, id, itemKey.ItemType, itemKey.TemplateId);
		SetTaiwuTreasury(context, taiwuTreasury);
		DomainManager.Extra.SetTaiwuVillageStoragesRecordCollection(context, taiwuVillageStoragesRecordCollection);
		Events.RaiseTaiwuItemModified(context, itemKey);
	}

	public void VillagerTakeItemFromTreasury(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey, int amount)
	{
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		int num = taiwuTreasury.CalcAdjustedWorth(baseItem.GetItemSubType(), baseItem.GetValue()) * amount;
		baseItem.RemoveOwner(ItemOwnerType.Treasury, _taiwuVillageSettlementId);
		taiwuTreasury.Inventory.OfflineRemove(itemKey, amount);
		taiwuTreasury.OfflineChangeContribution(character, -num);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = character.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		if (num > 0)
		{
			lifeRecordCollection.AddTaiwuVillagerTakeItem(id, currDate, itemKey.ItemType, itemKey.TemplateId);
		}
		TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
		taiwuVillageStoragesRecordCollection.AddTakeItem(currDate, TaiwuVillageStorageType.Treasury, id, itemKey.ItemType, itemKey.TemplateId);
		SetTaiwuTreasury(context, taiwuTreasury);
		DomainManager.Extra.SetTaiwuVillageStoragesRecordCollection(context, taiwuVillageStoragesRecordCollection);
	}

	public void VillagerStoreResourceInTreasury(DataContext context, GameData.Domains.Character.Character character, sbyte resourceType, int amount)
	{
		int num = DomainManager.Organization.CalcResourceContribution(16, resourceType, amount);
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		taiwuTreasury.Resources.Add(resourceType, amount);
		taiwuTreasury.OfflineChangeContribution(character, num);
		SetTaiwuTreasury(context, taiwuTreasury);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = character.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		if (num > 0)
		{
			lifeRecordCollection.AddTaiwuVillagerStorageResources(id, currDate, amount, resourceType);
		}
		TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
		taiwuVillageStoragesRecordCollection.AddStorageResources(currDate, TaiwuVillageStorageType.Treasury, id, amount, resourceType);
		DomainManager.Extra.SetTaiwuVillageStoragesRecordCollection(context, taiwuVillageStoragesRecordCollection);
	}

	public void VillagerTakeResourceFromTreasury(DataContext context, GameData.Domains.Character.Character character, sbyte resourceType, int amount)
	{
		int num = DomainManager.Organization.CalcResourceContribution(16, resourceType, amount);
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		taiwuTreasury.Resources.Subtract(resourceType, amount);
		taiwuTreasury.OfflineChangeContribution(character, -num);
		SetTaiwuTreasury(context, taiwuTreasury);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = character.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		if (num > 0)
		{
			lifeRecordCollection.AddTaiwuVillagerTakeResources(id, currDate, amount, resourceType);
		}
		TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
		taiwuVillageStoragesRecordCollection.AddTakeResources(currDate, TaiwuVillageStorageType.Treasury, id, amount, resourceType);
		DomainManager.Extra.SetTaiwuVillageStoragesRecordCollection(context, taiwuVillageStoragesRecordCollection);
	}

	public int CalcItemContribution(ItemKey itemKey, int amount)
	{
		short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
		int value = DomainManager.Item.GetValue(itemKey);
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		return taiwuTreasury.CalcAdjustedWorth(itemSubType, value) * amount * GlobalConfig.Instance.ItemContributionPercent / 100;
	}

	public void UpdateTaiwuTreasury(DataContext context)
	{
		int currDate = DomainManager.World.GetCurrDate();
		TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
		taiwuVillageStoragesRecordCollection.Clear();
		for (sbyte b = 0; b < 5; b++)
		{
			taiwuVillageStoragesRecordCollection.AddClearRecord(currDate, (TaiwuVillageStorageType)b);
		}
		taiwuVillageStoragesRecordCollection.AddClearRecord(currDate, TaiwuVillageStorageType.Warehouse);
		DomainManager.Extra.SetTaiwuVillageStoragesRecordCollection(context, taiwuVillageStoragesRecordCollection);
	}

	public void ChangeVillagerWorkBuildingBlockIndex(int charId, short buildingBlockIndex, DataContext context)
	{
		_villagerWork[charId].BuildingBlockIndex = buildingBlockIndex;
		SetElement_VillagerWork(charId, _villagerWork[charId], context);
	}

	private void InitializeVillagerWorkLocations()
	{
		foreach (VillagerWorkData value in _villagerWork.Values)
		{
			if (VillagerWorkType.IsWorkOnMap(value.WorkType))
			{
				_villagerWorkLocations.Add(new Location(value.AreaId, value.BlockId), default(VoidValue));
			}
		}
	}

	[DomainMethod]
	public bool SetVillagerCollectResourceWork(DataContext context, int charId, short areaId, short blockId, sbyte resourceType)
	{
		StopVillagerWorkOptional(context, areaId, blockId, -1, removeUnlockedState: false);
		VillagerWorkData villagerWorkData = new VillagerWorkData(charId, 10, areaId, blockId);
		villagerWorkData.ResourceType = resourceType;
		SetVillagerWork(context, charId, villagerWorkData);
		return true;
	}

	[DomainMethod]
	public sbyte GetVillagerCollectStorageType(DataContext context, int charId)
	{
		_villagerWork.TryGetValue(charId, out var value);
		if (value.GetVillagerRole() is VillagerRoleFarmer villagerRoleFarmer)
		{
			return villagerRoleFarmer.FarmerStorageTypes[value.ResourceType];
		}
		return -1;
	}

	[DomainMethod]
	public bool SetVillagerCollectStorageType(DataContext context, int charId, sbyte villagerRoleStorageType)
	{
		_villagerWork.TryGetValue(charId, out var value);
		if (value.GetVillagerRole() is VillagerRoleFarmer villagerRoleFarmer)
		{
			villagerRoleFarmer.FarmerStorageTypes[value.ResourceType] = villagerRoleStorageType;
			DomainManager.Extra.SetVillagerRole(context, charId);
		}
		return true;
	}

	[DomainMethod]
	public bool SetVillagerCollectTributeWork(DataContext context, int charId, short areaId, short blockId)
	{
		StopVillagerWorkOptional(context, areaId, blockId, 11, removeUnlockedState: false);
		VillagerWorkData workData = new VillagerWorkData(charId, 11, areaId, blockId);
		SetVillagerWork(context, charId, workData);
		return true;
	}

	[DomainMethod]
	public bool SetVillagerKeepGraveWork(DataContext context, int charId, short areaId, short blockId, int graveId)
	{
		StopVillagerWorkOptional(context, areaId, blockId, 12, removeUnlockedState: false);
		VillagerWorkData villagerWorkData = new VillagerWorkData(charId, 12, areaId, blockId);
		villagerWorkData.GraveId = graveId;
		SetVillagerWork(context, charId, villagerWorkData);
		return true;
	}

	[DomainMethod]
	public bool SetVillagerIdleWork(DataContext context, int charId, short areaId, short blockId)
	{
		StopVillagerWorkOptional(context, areaId, blockId, 13, removeUnlockedState: false);
		VillagerWorkData workData = new VillagerWorkData(charId, 13, areaId, blockId);
		SetVillagerWork(context, charId, workData);
		return true;
	}

	[DomainMethod]
	public bool SetVillagerMigrateWork(DataContext context, int charId, short areaId, short blockId, sbyte resourceType)
	{
		StopVillagerWorkOptional(context, areaId, blockId, 14, removeUnlockedState: false);
		VillagerWorkData villagerWorkData = new VillagerWorkData(charId, 14, areaId, blockId);
		villagerWorkData.ResourceType = resourceType;
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(charId);
		if (villagerRole == null)
		{
			Logger.Warn($"character {charId} is not assigned as any villager role.");
			return false;
		}
		SetVillagerWork(context, charId, villagerWorkData);
		return true;
	}

	[DomainMethod]
	public bool SetVillagerDevelopWork(DataContext context, int charId, short areaId, short blockId, sbyte resourceType, short index)
	{
		StopVillagerWorkOptional(context, areaId, blockId, -1, removeUnlockedState: false);
		return true;
	}

	[DomainMethod]
	[Obsolete("使用StopVillagerWorkOptional代替")]
	public bool StopVillagerWork(DataContext context, short areaId, short blockId, sbyte workType)
	{
		return StopVillagerWorkOptional(context, areaId, blockId, workType, removeUnlockedState: true);
	}

	[DomainMethod]
	public bool StopVillagerWorkOptional(DataContext context, short areaId, short blockId, sbyte workType, bool removeUnlockedState)
	{
		foreach (KeyValuePair<int, VillagerWorkData> item in _villagerWork)
		{
			if ((item.Value.WorkType == workType || workType == -1) && item.Value.AreaId == areaId && item.Value.BlockId == blockId)
			{
				RemoveVillagerWork(context, item.Key, removeUnlockedState);
				return true;
			}
		}
		return false;
	}

	[DomainMethod]
	[Obsolete("使用StopVillagerWorkOptional代替")]
	public bool StopVillagerCollectResourceWork(DataContext context, short areaId, short blockId)
	{
		foreach (KeyValuePair<int, VillagerWorkData> item in _villagerWork)
		{
			if (item.Value.WorkType == 10 && item.Value.AreaId == areaId && item.Value.BlockId == blockId)
			{
				RemoveVillagerWork(context, item.Key, removeUnlockedState: false);
				return true;
			}
		}
		return false;
	}

	public VillagerWorkData GetVillagerMapWorkData(short areaId, short blockId, sbyte workType)
	{
		Tester.Assert(VillagerWorkType.IsWorkOnMap(workType));
		if (!_villagerWorkLocations.ContainsKey(new Location(areaId, blockId)))
		{
			return null;
		}
		foreach (VillagerWorkData value in _villagerWork.Values)
		{
			if (value.WorkType == workType && value.AreaId == areaId && value.BlockId == blockId)
			{
				return value;
			}
		}
		return null;
	}

	[DomainMethod]
	public List<VillagerWorkData> GetCollectResourceWorkDataList(List<Location> locationList)
	{
		List<VillagerWorkData> list = new List<VillagerWorkData>();
		foreach (VillagerWorkData value in _villagerWork.Values)
		{
			if (value.WorkType == 10 && locationList.Contains(new Location(value.AreaId, value.BlockId)))
			{
				list.Add(value);
			}
		}
		return list;
	}

	public void GetKeptGraves(HashSet<int> keptGraveSet)
	{
		keptGraveSet.Clear();
		foreach (VillagerWorkData value in _villagerWork.Values)
		{
			if (value.WorkType == 12)
			{
				keptGraveSet.Add(value.GraveId);
			}
		}
	}

	[DomainMethod]
	public void ExpelVillager(DataContext context, int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		if (element_Objects.GetAgeGroup() == 0)
		{
			throw new ArgumentException($"You cannot expel a baby {charId}.");
		}
		if (IsInGroup(charId))
		{
			LeaveGroup(context, charId);
		}
		DomainManager.Organization.JoinNearbyVillageTownAsBeggar(context, element_Objects, -1);
		Location location = element_Objects.GetLocation();
		if (location.IsValid())
		{
			MapBlockData block = DomainManager.Map.GetBlock(location);
			DomainManager.Map.SetBlockData(context, block);
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		lifeRecordCollection.AddExpelVillager(_taiwuCharId, currDate, charId, _taiwuChar.GetLocation());
		sbyte behaviorType = element_Objects.GetBehaviorType();
		if ((behaviorType == 0 || (uint)(behaviorType - 3) <= 1u) ? true : false)
		{
			GameData.Domains.Character.Character.ApplyAddRelation_Enemy(context, element_Objects, _taiwuChar, selfIsTaiwuPeople: true, 0);
		}
		EventHelper.ChangeFavorabilityOptional(element_Objects, _taiwuChar, GlobalConfig.Instance.FavorabilityChangeOnExpel[behaviorType], 4);
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		instantNotificationCollection.AddLeaveTaiwuVillage(charId);
	}

	public Location GetTaiwuVillageLocation()
	{
		return DomainManager.Building.GetTaiwuBuildingAreas()[0];
	}

	[DomainMethod]
	public MapBlockData GetTaiwuVillagerMapBlockData()
	{
		Location taiwuVillageLocation = GetTaiwuVillageLocation();
		return DomainManager.Map.GetBlock(taiwuVillageLocation);
	}

	public bool VillagerHasWork(int charId)
	{
		return _villagerWork.ContainsKey(charId);
	}

	public void MakeVillagerWorkSettlementsVisited(DataContext context)
	{
		MapDomain map = DomainManager.Map;
		CharacterDomain character = DomainManager.Character;
		foreach (KeyValuePair<int, VillagerWorkData> item in _villagerWork)
		{
			if (!character.TryGetElement_Objects(item.Key, out var element))
			{
				continue;
			}
			Location location = element.GetLocation();
			if (location.AreaId != item.Value.AreaId || location.BlockId != item.Value.BlockId)
			{
				continue;
			}
			MapBlockData rootBlock = map.GetBlock(location).GetRootBlock();
			MapAreaData element_Areas = map.GetElement_Areas(location.AreaId);
			SettlementInfo[] settlementInfos = element_Areas.SettlementInfos;
			for (int i = 0; i < settlementInfos.Length; i++)
			{
				SettlementInfo settlementInfo = settlementInfos[i];
				if (settlementInfo.BlockId == rootBlock.BlockId)
				{
					TryAddVisitedSettlement(settlementInfo.SettlementId, context);
				}
			}
		}
	}

	public void SetVillagerWork(DataContext context, int charId, VillagerWorkData workData, bool allowChild = false)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		bool flag = DomainManager.TutorialChapter.InGuiding && DomainManager.TutorialChapter.GetTutorialChapter() == 2;
		if (!allowChild && element_Objects.GetPhysiologicalAge() < 16 && !flag)
		{
			throw new Exception($"Cannot send a child to work: {element_Objects}");
		}
		if (_villagerWork.TryGetValue(charId, out var value))
		{
			if (value.GetVillagerRole() != null && workData.GetVillagerRole() == null)
			{
				value.GetVillagerRole().ArrangementTemplateId = -1;
				value.GetVillagerRole().WorkData = null;
				DomainManager.Extra.SetVillagerRole(context, charId);
			}
			RemoveVillagerWork(context, charId);
		}
		AddElement_VillagerWork(charId, workData, context);
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(charId);
		if (villagerRole != null)
		{
			villagerRole.WorkData = workData;
		}
		switch (workData.WorkType)
		{
		case 0:
			DomainManager.Building.SetBuildingOperator(context, new BuildingBlockKey(workData.AreaId, workData.BlockId, workData.BuildingBlockIndex), workData.WorkerIndex, charId);
			break;
		case 1:
			DomainManager.Building.SetShopBuildingManager(context, new BuildingBlockKey(workData.AreaId, workData.BlockId, workData.BuildingBlockIndex), workData.WorkerIndex, charId);
			break;
		case 2:
			if (workData.GetVillagerRole() == null || workData.GetVillagerRole().ArrangementTemplateId < 0)
			{
				throw new Exception($"Need a role to work a job: {element_Objects}");
			}
			break;
		default:
			AddElement_VillagerWorkLocations(new Location(workData.AreaId, workData.BlockId), default(VoidValue), context);
			break;
		}
		if (VillagerWorkType.IsWorkOnMap(workData.WorkType))
		{
			DomainManager.Extra.AddLocationMark(context, new Location(workData.AreaId, workData.BlockId));
		}
		element_Objects.ActiveExternalRelationState(context, 1);
		if (VillagerWorkType.IsWorkOnMap(workData.WorkType) && DomainManager.Extra.IsExtraTaskInProgress(23))
		{
			DomainManager.Extra.FinishTriggeredExtraTask(context, 14, 23);
		}
	}

	public void RemoveVillagerWork(DataContext context, int charId, bool removeUnlockedState = true)
	{
		if (!_villagerWork.TryGetValue(charId, out var value))
		{
			return;
		}
		if (value.GetVillagerRole() != null)
		{
			value.GetVillagerRole().WorkData = null;
			if (value.GetVillagerRole().ArrangementTemplateId >= 0)
			{
				value.GetVillagerRole().ArrangementTemplateId = -1;
				DomainManager.Extra.SetVillagerRole(context, charId);
			}
		}
		switch (value.WorkType)
		{
		case 0:
			DomainManager.Building.SetBuildingOperator(context, new BuildingBlockKey(value.AreaId, value.BlockId, value.BuildingBlockIndex), value.WorkerIndex, -1);
			break;
		case 1:
			DomainManager.Building.SetShopBuildingManager(context, new BuildingBlockKey(value.AreaId, value.BlockId, value.BuildingBlockIndex), value.WorkerIndex, -1);
			break;
		case 14:
		{
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(charId);
			if (villagerRole is VillagerRoleFarmer villagerRoleFarmer)
			{
				villagerRoleFarmer.ResetFailureAccumulation();
				DomainManager.Extra.SetVillagerRole(context, charId);
			}
			RemoveElement_VillagerWorkLocations(new Location(value.AreaId, value.BlockId), context);
			break;
		}
		default:
			RemoveElement_VillagerWorkLocations(new Location(value.AreaId, value.BlockId), context);
			break;
		case 2:
			break;
		}
		if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			element.DeactivateExternalRelationState(context, 1);
		}
		RemoveElement_VillagerWork(charId, context);
		if (removeUnlockedState)
		{
			DomainManager.Building.TryRemoveUnlockedWorkingVillager(context, charId);
		}
	}

	public Dictionary<int, VillagerWorkData> GetVillagerWorkDict()
	{
		return _villagerWork;
	}

	public short GetWorkingVillagerCount()
	{
		return (short)_villagerWork.Count;
	}

	public byte GetVillagerWorkStatus(GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		byte result = 0;
		if (_villagerWork.ContainsKey(id))
		{
			result = 2;
		}
		if (character.GetPhysiologicalAge() < 16)
		{
			result = 3;
		}
		if (IsInGroup(id))
		{
			result = 1;
		}
		if (character.IsCompletelyInfected())
		{
			result = 4;
		}
		if (character.GetKidnapperId() >= 0)
		{
			result = 6;
		}
		else if (character.IsActiveExternalRelationState(4) && character.GetKidnappingEnemyNestAdventure() != -1)
		{
			result = 6;
		}
		if (DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Get<Location>("MainStoryLine_SpiritualWanderPlace_TaiwuVillagersCenter", out Location _))
		{
			result = 5;
		}
		return result;
	}

	[DomainMethod]
	public List<VillagerStatusDisplayData> GetVillagerStatusDisplayDataList(List<int> charIds)
	{
		if (charIds == null)
		{
			return null;
		}
		List<VillagerStatusDisplayData> list = new List<VillagerStatusDisplayData>(charIds.Count);
		foreach (int charId in charIds)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			byte livingStatus = DomainManager.Building.GetLivingStatus(charId);
			byte b = 0;
			if (_villagerWork.ContainsKey(charId))
			{
				b = 2;
			}
			if (element_Objects.GetPhysiologicalAge() < 16)
			{
				b = 3;
			}
			if (IsInGroup(charId))
			{
				b = 1;
			}
			if (element_Objects.IsCompletelyInfected())
			{
				b = 4;
				livingStatus = 3;
			}
			if (element_Objects.GetKidnapperId() >= 0)
			{
				b = 6;
			}
			else if (element_Objects.IsActiveExternalRelationState(4) && element_Objects.GetKidnappingEnemyNestAdventure() != -1)
			{
				b = 6;
			}
			if (DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Get<Location>("MainStoryLine_SpiritualWanderPlace_TaiwuVillagersCenter", out Location _))
			{
				b = 5;
			}
			OrganizationInfo organizationInfo = element_Objects.GetOrganizationInfo();
			AvatarData avatar = element_Objects.GetAvatar();
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(charId);
			VillagerRoleArrangementDisplayDataWrapper villagerRoleArrangementDisplayDataWrapper = null;
			if (villagerRole != null)
			{
				villagerRoleArrangementDisplayDataWrapper = new VillagerRoleArrangementDisplayDataWrapper();
				villagerRoleArrangementDisplayDataWrapper.ArrangementTemplateId = villagerRole.ArrangementTemplateId;
				if (villagerRole.WorkData != null)
				{
					villagerRoleArrangementDisplayDataWrapper.AreaId = villagerRole.WorkData.Location.AreaId;
				}
				villagerRoleArrangementDisplayDataWrapper.ArrangementDataId = CalcVillagerRoleDefaultArrangementId(villagerRole.RoleTemplateId);
				villagerRoleArrangementDisplayDataWrapper.ArrangementData = villagerRole.GetArrangementDisplayData();
			}
			short trappedAreaId = -1;
			if (b == 6)
			{
				if (element_Objects.GetKidnapperId() >= 0)
				{
					if (DomainManager.Character.TryGetElement_Objects(element_Objects.GetKidnapperId(), out var element))
					{
						trappedAreaId = element.GetLocation().AreaId;
					}
				}
				else
				{
					trappedAreaId = element_Objects.GetLocation().AreaId;
				}
			}
			short favorabilityToTaiwu = (DomainManager.Character.IsInteractedWithTaiwu(element_Objects.GetId()) ? DomainManager.Character.GetFavorability(element_Objects.GetId(), DomainManager.Taiwu.GetTaiwuCharId()) : short.MinValue);
			list.Add(new VillagerStatusDisplayData
			{
				CharacterId = charId,
				Name = new NameRelatedData
				{
					CharTemplateId = element_Objects.GetTemplateId(),
					Gender = element_Objects.GetGender(),
					MonkType = element_Objects.GetMonkType(),
					FullName = element_Objects.GetFullName(),
					OrgTemplateId = organizationInfo.OrgTemplateId,
					OrgGrade = organizationInfo.Grade,
					MonasticTitle = element_Objects.GetMonasticTitle(),
					NickNameId = DomainManager.Taiwu.GetFollowingNpcNickNameId(element_Objects.GetId()),
					CustomDisplayNameId = DomainManager.Extra.GetCharacterCustomDisplayName(charId)
				},
				CurrAge = element_Objects.GetCurrAge(),
				Health = element_Objects.GetHealth(),
				MaxLeftHealth = element_Objects.GetLeftMaxHealth(),
				Gender = element_Objects.GetGender(),
				BehaviorType = element_Objects.GetBehaviorType(),
				Happiness = element_Objects.GetHappiness(),
				FavorabilityToTaiwu = favorabilityToTaiwu,
				Fame = element_Objects.GetFame(),
				WorkStatus = b,
				LivingStatus = livingStatus,
				PhysiologicalAge = element_Objects.GetPhysiologicalAge(),
				ClothDisplayId = element_Objects.GetClothingDisplayId(),
				FaceVisible = (!avatar.ShowVeil && !avatar.ShowMask(element_Objects.GetClothingDisplayId())),
				CreatingType = element_Objects.GetCreatingType(),
				BirthDate = element_Objects.GetBirthDate(),
				DefeatMarkCount = (sbyte)CombatDomain.GetDefeatMarksCountOutOfCombat(element_Objects),
				Charm = element_Objects.GetAttraction(),
				PreexistenceCharCount = (short)element_Objects.GetPreexistenceCharIds().Count,
				AttackMedal = element_Objects.GetFeatureMedalValue(0),
				DefenceMedal = element_Objects.GetFeatureMedalValue(1),
				WisdomMedal = element_Objects.GetFeatureMedalValue(2),
				MaxMainAttributes = element_Objects.GetMaxMainAttributes(),
				Penetrations = element_Objects.GetPenetrations(),
				PenetrationResists = element_Objects.GetPenetrationResists(),
				HitValues = element_Objects.GetHitValues(),
				AvoidValues = element_Objects.GetAvoidValues(),
				DisorderOfQi = element_Objects.GetDisorderOfQi(),
				LifeSkillQualifications = element_Objects.GetLifeSkillQualifications(),
				LifeSkillAttainment = element_Objects.GetLifeSkillAttainments(),
				LifeSkillGrowthType = element_Objects.GetLifeSkillQualificationGrowthType(),
				CombatSkillQualifications = element_Objects.GetCombatSkillQualifications(),
				CombatSkillGrowthType = element_Objects.GetCombatSkillQualificationGrowthType(),
				Personalities = element_Objects.GetPersonalities(),
				Resources = element_Objects.GetResources(),
				CurrInventoryLoad = element_Objects.GetCurrInventoryLoad(),
				MaxInventoryLoad = element_Objects.GetMaxInventoryLoad(),
				KidnapCount = (sbyte)DomainManager.Character.GetKidnappedCharacterCount(charId),
				OrgInfo = organizationInfo,
				RoleTemplateId = DomainManager.Extra.GetVillagerRoleTemplateId(charId),
				ArrangementDisplayData = villagerRoleArrangementDisplayDataWrapper,
				TrappedAreaId = trappedAreaId
			});
		}
		return list;
	}

	[DomainMethod]
	public List<VillagerStatusDisplayData> GetAllVillagersStatus()
	{
		List<int> list = new List<int>();
		DomainManager.Organization.GetElement_CivilianSettlements(_taiwuVillageSettlementId).GetMembers().GetAllMembers(list);
		list.Remove(_taiwuCharId);
		return GetVillagerStatusDisplayDataList(list);
	}

	[DomainMethod]
	public List<int> GetAllVillagersAvailableForWork(bool actuallyNotOccupiedOnly = false)
	{
		List<int> list = new List<int>();
		if (DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Get<Location>("MainStoryLine_SpiritualWanderPlace_TaiwuVillagersCenter", out Location _))
		{
			return list;
		}
		DomainManager.Organization.GetElement_CivilianSettlements(_taiwuVillageSettlementId).GetMembers().GetAllMembers(list);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (!IsVillagerAvailableForWork(list[num], actuallyNotOccupiedOnly))
			{
				list.RemoveAt(num);
			}
		}
		return list;
	}

	[DomainMethod]
	public List<int> GetAllChildAvailableForWork(bool actuallyNotOccupiedOnly = false)
	{
		List<int> list = new List<int>();
		DomainManager.Organization.GetElement_CivilianSettlements(_taiwuVillageSettlementId).GetMembers().GetAllMembers(list);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (!IsVillagerChildAvailableForWork(list[num], actuallyNotOccupiedOnly))
			{
				list.RemoveAt(num);
			}
		}
		return list;
	}

	[DomainMethod]
	public List<int> GetVillagersAvailableForVillagerRole(bool removeTaiwuGroup = true)
	{
		List<int> list = new List<int>();
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		DomainManager.Organization.GetElement_CivilianSettlements(_taiwuVillageSettlementId).GetMembers().GetAllMembers(list);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(list[num]);
			if (element_Objects.GetPhysiologicalAge() < 16 || element_Objects.GetKidnapperId() >= 0 || list[num] == taiwuCharId || (removeTaiwuGroup && collection.Contains(list[num])))
			{
				list.RemoveAt(num);
			}
		}
		return list;
	}

	[DomainMethod]
	public List<sbyte> GetVillagerRoleExecuteFixedActionFailReasons(DataContext context, short villagerRoleTemplateId, sbyte fixedActionType)
	{
		Tester.Assert(villagerRoleTemplateId >= 0 && villagerRoleTemplateId <= 2);
		List<sbyte> list = new List<sbyte>();
		List<sbyte> failReasons = ObjectPool<List<sbyte>>.Instance.Get();
		IReadOnlySet<int> villagerRoleSet = DomainManager.Taiwu.GetVillagerRoleSet(villagerRoleTemplateId);
		bool flag = false;
		foreach (int item in villagerRoleSet)
		{
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(item);
			failReasons.Clear();
			if (villagerRole.GetExecuteFixedActionFailReasons(context, fixedActionType, ref failReasons))
			{
				flag = true;
			}
			for (int i = 0; i < failReasons.Count; i++)
			{
				if (!list.Contains(failReasons[i]))
				{
					list.Add(failReasons[i]);
				}
			}
		}
		ObjectPool<List<sbyte>>.Instance.Return(failReasons);
		if (flag)
		{
			list.Clear();
		}
		return list;
	}

	[DomainMethod]
	public List<int> GetVillagersForWork(bool includeUnlockedWorkingVillagers = false, bool farmerFirst = false)
	{
		if (DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Get<Location>("MainStoryLine_SpiritualWanderPlace_TaiwuVillagersCenter", out Location _))
		{
			return new List<int>();
		}
		List<int> allVillagersAvailableForWork = GetAllVillagersAvailableForWork(actuallyNotOccupiedOnly: true);
		if (includeUnlockedWorkingVillagers)
		{
			List<int> unlockedWorkingVillagers = DomainManager.Extra.GetUnlockedWorkingVillagers();
			foreach (int item in unlockedWorkingVillagers)
			{
				if (IsVillagerAvailableForWork(item))
				{
					allVillagersAvailableForWork.Add(item);
				}
			}
		}
		if (!farmerFirst)
		{
			return allVillagersAvailableForWork;
		}
		HashSet<int> farmerSet = DomainManager.Organization.GetSettlement(_taiwuVillageSettlementId).GetMembers().GetMembers(1);
		return allVillagersAvailableForWork.OrderBy((int charId) => !farmerSet.Contains(charId)).ToList();
	}

	public bool IsVillagerAvailableForWork(int charId, bool actuallyNotOccupiedOnly = false)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		if (!CharacterMatcher.DefValue.VillagerAvailableForWork.Match(element_Objects))
		{
			return false;
		}
		return !actuallyNotOccupiedOnly || !_villagerWork.ContainsKey(charId);
	}

	public bool IsVillagerChildAvailableForWork(int charId, bool actuallyNotOccupiedOnly = false)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		if (!CharacterMatcher.DefValue.ChildVillagerAvailableForWork.Match(element_Objects))
		{
			return false;
		}
		return !actuallyNotOccupiedOnly || !_villagerWork.ContainsKey(charId);
	}

	public bool CanWork(int charId)
	{
		if (_villagerWork.ContainsKey(charId))
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			return CharacterMatcher.DefValue.CanWork.Match(element_Objects);
		}
		throw new Exception($"Villager with character id {charId} is not working");
	}

	public void MoveVillagersToWorkLocation(DataContext context)
	{
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		_moveToWorkLocationCharList.Clear();
		foreach (KeyValuePair<int, VillagerWorkData> item in _villagerWork)
		{
			VillagerWorkData value = item.Value;
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(value.CharacterId);
			Location location = element_Objects.GetLocation();
			Location location2 = new Location(value.AreaId, value.BlockId);
			if (!location2.IsValid() || value.WorkType == 2)
			{
				continue;
			}
			sbyte workType = value.WorkType;
			if ((uint)workType <= 1u)
			{
				if (!DomainManager.Map.IsLocationInSettlementInfluenceRange(location, _taiwuVillageSettlementId))
				{
					List<short> list = new List<short>();
					DomainManager.Map.GetSettlementBlocksAndAffiliatedBlocks(location2.AreaId, location2.BlockId, list);
					int index = context.Random.Next(list.Count);
					Location targetLocation = new Location(location2.AreaId, list[index]);
					DomainManager.Character.LeaveGroup(context, element_Objects);
					DomainManager.Character.GroupMove(context, element_Objects, targetLocation);
					_moveToWorkLocationCharList.Add(value.CharacterId);
				}
				else
				{
					Location targetLocation2 = location;
					DomainManager.Character.LeaveGroup(context, element_Objects);
					DomainManager.Character.GroupMove(context, element_Objects, targetLocation2);
				}
			}
			else if (location.AreaId != value.AreaId || location.BlockId != value.BlockId)
			{
				Location targetLocation3 = location2;
				DomainManager.Character.LeaveGroup(context, element_Objects);
				DomainManager.Character.GroupMove(context, element_Objects, targetLocation3);
				_moveToWorkLocationCharList.Add(value.CharacterId);
			}
		}
	}

	public void CalcVillagerWorkOnMap(DataContext context)
	{
		foreach (KeyValuePair<int, VillagerWorkData> item in _villagerWork)
		{
			VillagerWorkData value = item.Value;
			if (CanWork(value.CharacterId))
			{
				switch (value.WorkType)
				{
				case 10:
					VillagerWorkCollectResource(context, value);
					break;
				case 11:
					DomainManager.Adventure.CollectTribute(context, value.CharacterId, value.AreaId, value.BlockId);
					break;
				case 14:
					VillagerWorkMigrateResource(context, value);
					break;
				}
			}
		}
	}

	public void ClearAdvanceMonthData()
	{
		_moveToWorkLocationCharList.Clear();
		ResourceRecoverSpeedUpDict.Clear();
	}

	public void AddTaiwuVillageResident(DataContext context, int charId)
	{
		DomainManager.Building.AddTaiwuResident(context, charId, autoHousing: true);
	}

	public void TryRemoveTaiwuVillageResident(DataContext context, int charId)
	{
		RemoveVillagerWork(context, charId);
		DomainManager.Building.RemoveTaiwuResident(context, charId);
	}

	[DomainMethod]
	public int[] CalcResourceChangeByVillageWork(DataContext context)
	{
		int[] array = new int[8];
		if (!DomainManager.World.GetWorldFunctionsStatus(10))
		{
			for (sbyte b = 0; b < 8; b++)
			{
				array[b] = 0;
			}
			return array;
		}
		foreach (KeyValuePair<int, VillagerWorkData> item in _villagerWork)
		{
			VillagerWorkData value = item.Value;
			if (value.WorkType == 10 && CanWork(value.CharacterId))
			{
				array[value.ResourceType] += value.GetCollectResourceIncome();
			}
		}
		return array;
	}

	[DomainMethod]
	public int[] CalcResourceChangeByBuildingEarn(DataContext context)
	{
		int[] value = new int[8];
		if (!DomainManager.World.GetWorldFunctionsStatus(10))
		{
			for (sbyte b = 0; b < 8; b++)
			{
				value[b] = 0;
			}
			return value;
		}
		CalcResourceBlockIncomeEffectValues(ref value);
		List<Location> taiwuBuildingAreas = DomainManager.Building.GetTaiwuBuildingAreas();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location elementId = taiwuBuildingAreas[i];
			BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(elementId);
			for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, num);
				BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
				if (element_BuildingBlocks.RootBlockIndex < 0)
				{
					BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
					if (buildingBlockItem.IsCollectResourceBuilding)
					{
						sbyte collectBuildingResourceType = DomainManager.Building.GetCollectBuildingResourceType(buildingBlockKey);
						sbyte b2 = (sbyte)((collectBuildingResourceType < 6) ? collectBuildingResourceType : 5);
						value[b2] += DomainManager.Building.CalcResourceOutputCount(buildingBlockKey, b2);
					}
					if (element_BuildingBlocks.TemplateId == 45 && element_BuildingBlocks.CanUse())
					{
						value[7] += DomainManager.Building.CalculateGainAuthorityByShrinePerMonth(buildingBlockKey);
					}
				}
			}
		}
		return value;
	}

	[DomainMethod]
	public int[] CalcResourceChangeByBuildingMaintain(DataContext context)
	{
		int[] array = new int[8];
		if (!DomainManager.World.GetWorldFunctionsStatus(10))
		{
			for (sbyte b = 0; b < 8; b++)
			{
				array[b] = 0;
			}
			return array;
		}
		List<Location> taiwuBuildingAreas = DomainManager.Building.GetTaiwuBuildingAreas();
		for (int i = 0; i < taiwuBuildingAreas.Count; i++)
		{
			Location elementId = taiwuBuildingAreas[i];
			BuildingAreaData element_BuildingAreas = DomainManager.Building.GetElement_BuildingAreas(elementId);
			for (short num = 0; num < element_BuildingAreas.Width * element_BuildingAreas.Width; num++)
			{
				BuildingBlockKey buildingBlockKey = new BuildingBlockKey(elementId.AreaId, elementId.BlockId, num);
				BuildingBlockData element_BuildingBlocks = DomainManager.Building.GetElement_BuildingBlocks(buildingBlockKey);
				if (element_BuildingBlocks.RootBlockIndex < 0 && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)buildingBlockKey, out var value))
				{
					BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[element_BuildingBlocks.TemplateId];
					sbyte b2 = value.CalcUnlockedLevelCount();
					if (element_BuildingBlocks.NeedMaintenanceCost())
					{
						int[] finalMaintenanceCost = GameData.Domains.Building.SharedMethods.GetFinalMaintenanceCost(buildingBlockItem);
						for (int j = 0; j < finalMaintenanceCost.Length; j++)
						{
							array[j] -= finalMaintenanceCost[j];
						}
					}
					if (element_BuildingBlocks.Durability == 0 && buildingBlockItem.DestoryType == 1 && b2 == 1)
					{
						array[7] -= buildingBlockItem.MaxDurability;
					}
				}
			}
		}
		return array;
	}

	[Obsolete]
	[DomainMethod]
	public int[] CalcResourceChangeByAutoExpand(DataContext context)
	{
		return new int[8];
	}

	[Obsolete]
	[DomainMethod]
	public int CalcAutoExpandNotSatisfyIndex(DataContext context)
	{
		return 0;
	}

	public void CheckAboutToDieVillagersAndTaiwuPeople(DataContext context)
	{
		HashSet<int> obj = context.AdvanceMonthRelatedData.BlockCharSet.Occupy();
		DomainManager.Character.GetTaiwuPeople(obj);
		OrgMemberCollection members = DomainManager.Organization.GetElement_CivilianSettlements(_taiwuVillageSettlementId).GetMembers();
		for (sbyte b = 0; b < 9; b++)
		{
			obj.UnionWith(members.GetMembers(b));
		}
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		foreach (int item in obj)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetLeftMaxHealth() == 6)
			{
				monthlyNotificationCollection.AddAboutToDie(item);
			}
		}
		context.AdvanceMonthRelatedData.BlockCharSet.Release(ref obj);
	}

	public unsafe (int[] qualifications, int[] attianments) GetVillagerAverageLifeSkillInfo()
	{
		List<int> list = new List<int>();
		Settlement settlement = DomainManager.Organization.GetSettlement(_taiwuVillageSettlementId);
		settlement.GetMembers().GetAllMembers(list);
		list.Remove(_taiwuCharId);
		int[] array = new int[16];
		int[] array2 = new int[16];
		foreach (int item in list)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				LifeSkillShorts lifeSkillQualifications = element.GetLifeSkillQualifications();
				LifeSkillShorts lifeSkillAttainments = element.GetLifeSkillAttainments();
				for (int i = 0; i < 16; i++)
				{
					array[i] += lifeSkillQualifications.Items[i];
					array2[i] += lifeSkillAttainments.Items[i];
				}
			}
		}
		for (int j = 0; j < 16; j++)
		{
			array[j] = Convert.ToInt32((float)array[j] / (float)list.Count);
			array2[j] = Convert.ToInt32((float)array2[j] / (float)list.Count);
		}
		return (qualifications: array, attianments: array2);
	}

	public unsafe (int[] qualifications, int[] attianments) GetVillagerAverageCombatSkillInfo()
	{
		List<int> list = new List<int>();
		Settlement settlement = DomainManager.Organization.GetSettlement(_taiwuVillageSettlementId);
		settlement.GetMembers().GetAllMembers(list);
		list.Remove(_taiwuCharId);
		int[] array = new int[14];
		int[] array2 = new int[14];
		foreach (int item in list)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				CombatSkillShorts combatSkillQualifications = element.GetCombatSkillQualifications();
				CombatSkillShorts combatSkillAttainments = element.GetCombatSkillAttainments();
				for (int i = 0; i < 14; i++)
				{
					array[i] += combatSkillQualifications.Items[i];
					array2[i] += combatSkillAttainments.Items[i];
				}
			}
		}
		for (int j = 0; j < 14; j++)
		{
			array[j] = Convert.ToInt32((float)array[j] / (float)list.Count);
			array2[j] = Convert.ToInt32((float)array2[j] / (float)list.Count);
		}
		return (qualifications: array, attianments: array2);
	}

	internal void CommitTaiwuSettlementTreasury(DataContext context)
	{
		if (_needCommitTaiwuSettlementTreasury)
		{
			_needCommitTaiwuSettlementTreasury = false;
			SetTaiwuTreasury(context, GetTaiwuTreasury());
		}
	}

	public void SetNeedCommitSettlementTreasury(bool needCommit)
	{
		_needCommitTaiwuSettlementTreasury = needCommit;
		GetTaiwuTreasury().DetectAndFixInvalidData();
	}

	public void UpdateVillagerRoleFixedActionSuccessArray(DataContext context, bool isPreAdvance)
	{
		List<sbyte> failReasons = ObjectPool<List<sbyte>>.Instance.Get();
		for (short num = 0; num < 3; num++)
		{
			int villagerRoleFixedActionTypeCount = GetVillagerRoleFixedActionTypeCount(num);
			bool[] villagerRoleFixedActionSuccessArray = GetVillagerRoleFixedActionSuccessArray(num, isPreAdvance);
			IReadOnlySet<int> villagerRoleSet = DomainManager.Taiwu.GetVillagerRoleSet(num);
			for (sbyte b = 0; b < villagerRoleFixedActionTypeCount; b++)
			{
				bool flag = false;
				foreach (int item in villagerRoleSet)
				{
					VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(item);
					failReasons.Clear();
					if (villagerRole.GetExecuteFixedActionFailReasons(context, b, ref failReasons))
					{
						flag = true;
					}
				}
				villagerRoleFixedActionSuccessArray[b] = flag;
			}
		}
		ObjectPool<List<sbyte>>.Instance.Return(failReasons);
	}

	private int GetVillagerRoleFixedActionTypeCount(short villagerRoleTemplateId)
	{
		Tester.Assert(villagerRoleTemplateId >= 0 && villagerRoleTemplateId <= 2);
		if (1 == 0)
		{
		}
		sbyte result = villagerRoleTemplateId switch
		{
			0 => 1, 
			1 => 4, 
			2 => 3, 
			_ => throw new ArgumentOutOfRangeException("villagerRoleTemplateId", villagerRoleTemplateId, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private bool[] GetVillagerRoleFixedActionSuccessArray(short villagerRoleTemplateId, bool isPreAdvance)
	{
		Tester.Assert(villagerRoleTemplateId >= 0 && villagerRoleTemplateId <= 2);
		if (1 == 0)
		{
		}
		bool[] result = villagerRoleTemplateId switch
		{
			0 => isPreAdvance ? _preAdvanceVillagerRoleFarmerFixedActionSuccessArray : _periAdvanceVillagerRoleFarmerFixedActionSuccessArray, 
			1 => isPreAdvance ? _preAdvanceVillagerRoleCraftsmanFixedActionSuccessArray : _periAdvanceVillagerRoleCraftsmanFixedActionSuccessArray, 
			2 => isPreAdvance ? _preAdvanceVillagerRoleDoctorFixedActionSuccessArray : _periAdvanceVillagerRoleDoctorFixedActionSuccessArray, 
			_ => throw new ArgumentOutOfRangeException("villagerRoleTemplateId", villagerRoleTemplateId, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public bool IsVillagerRoleFixedActionAddFailLifeRecord(short villagerRoleTemplateId, sbyte fixedActionType)
	{
		bool[] villagerRoleFixedActionSuccessArray = GetVillagerRoleFixedActionSuccessArray(villagerRoleTemplateId, isPreAdvance: true);
		bool[] villagerRoleFixedActionSuccessArray2 = GetVillagerRoleFixedActionSuccessArray(villagerRoleTemplateId, isPreAdvance: false);
		if (villagerRoleFixedActionSuccessArray[fixedActionType] && !villagerRoleFixedActionSuccessArray2[fixedActionType])
		{
			return true;
		}
		return false;
	}

	internal static short CalcVillagerRoleDefaultArrangementId(short roleId)
	{
		return VillagerRoleArrangement.Instance.FirstOrDefault((VillagerRoleArrangementItem item) => !item.InvisibleInGui && item.VillagerRole == roleId)?.TemplateId ?? (-1);
	}

	public void UpdateVillagerTreasuryNeed(DataContext context)
	{
		CalcVillagerClasses();
		List<int> taiwuVillagerMemberList = GetTaiwuVillagerMemberList();
		Settlement settlement = DomainManager.Organization.GetSettlement(_taiwuVillageSettlementId);
		RemoveAllInvalidVillagerNeed(taiwuVillagerMemberList, settlement);
		foreach (int item in taiwuVillagerMemberList)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (IsMeetWorkStatus(element_Objects, settlement))
			{
				element_Objects.VillagerReturnNotReadableBooks(context);
				element_Objects.ReturnVillagerRoleClothing(context);
			}
		}
		List<int>[] villagerListClassArray = _villagerListClassArray;
		foreach (List<int> list in villagerListClassArray)
		{
			if (list == null || list.Count <= 0)
			{
				continue;
			}
			_timeMeetTreasuryNeedDict.Clear();
			foreach (int item2 in list)
			{
				UpdateVillagerTreasuryNeed(item2);
			}
			if (_timeMeetTreasuryNeedDict.Count <= 0)
			{
				continue;
			}
			List<int> list2 = _timeMeetTreasuryNeedDict.Keys.ToList();
			CollectionUtils.Shuffle(context.Random, list2);
			foreach (int item3 in list2)
			{
				List<GameData.Domains.Character.Ai.PersonalNeed> list3 = _timeMeetTreasuryNeedDict[item3];
				foreach (GameData.Domains.Character.Ai.PersonalNeed item4 in list3)
				{
					HandleVillagerTreasuryNeed(context, item3, settlement, item4.ItemType, item4.ItemTemplateId);
				}
			}
		}
		RemoveAllInvalidVillagerNeed(taiwuVillagerMemberList, settlement);
		DomainManager.Extra.ClearVillagerLastInfluencePowerGrades(context);
		for (int j = 0; j < _villagerListClassArray.Length; j++)
		{
			List<int> list4 = _villagerListClassArray[j];
			if (list4 == null || list4.Count <= 0)
			{
				continue;
			}
			foreach (int item5 in list4)
			{
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item5);
				if (element_Objects2.GetAgeGroup() != 0 && IsMeetWorkStatus(element_Objects2, settlement))
				{
					HandleVillagerPersonalNeed(context, element_Objects2, settlement);
					HandleVillagerNoNeed(context, element_Objects2, settlement);
				}
			}
			sbyte value = Convert.ToSByte(8 - j);
			foreach (int item6 in list4)
			{
				DomainManager.Extra.SetVillagerLastInfluencePowerGrade(context, item6, value);
			}
		}
		_treasuryNeededItemDict.Clear();
		foreach (int item7 in taiwuVillagerMemberList)
		{
			GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(item7);
			element_Objects3.SetPersonalNeeds(element_Objects3.GetPersonalNeeds(), context);
			if (!DomainManager.Extra.TryGetVillagerTreasuryNeed(item7, out var villagerTreasuryNeed))
			{
				continue;
			}
			DomainManager.Extra.SetVillagerTreasuryNeed(context, item7, villagerTreasuryNeed);
			foreach (GameData.Domains.Character.Ai.PersonalNeed personalNeed in villagerTreasuryNeed.PersonalNeeds)
			{
				AddNeededItem(personalNeed, item7);
			}
		}
		foreach (int item8 in taiwuVillagerMemberList)
		{
			GameData.Domains.Character.Character element_Objects4 = DomainManager.Character.GetElement_Objects(item8);
			if (IsMeetWorkStatus(element_Objects4, settlement))
			{
				element_Objects4.ReturnVillagerRoleClothing(context);
			}
		}
	}

	private void RemoveAllInvalidVillagerNeed(List<int> memberList, Settlement settlement)
	{
		foreach (int member in memberList)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(member);
			RemoveInvalidVillagerTreasuryNeed(element_Objects, GetTaiwuTreasury());
			RemoveInvalidVillagerPersonalNeed(element_Objects);
		}
	}

	private void UpdateVillagerTreasuryNeed(int charId)
	{
		if (!DomainManager.Extra.TryGetVillagerTreasuryNeed(charId, out var villagerTreasuryNeed))
		{
			return;
		}
		List<GameData.Domains.Character.Ai.PersonalNeed> personalNeeds = villagerTreasuryNeed.PersonalNeeds;
		if (personalNeeds == null || personalNeeds.Count <= 0 || !_villagerClassDict.TryGetValue(charId, out var value))
		{
			return;
		}
		for (int i = 0; i < villagerTreasuryNeed.PersonalNeeds.Count; i++)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed = villagerTreasuryNeed.PersonalNeeds[i];
			if (personalNeed.RemainingMonths > 0)
			{
				personalNeed.RemainingMonths--;
				if (DomainManager.Extra.TryGetVillagerLastInfluencePowerGrade(charId, out var value2))
				{
					sbyte grade = ItemTemplateHelper.GetGrade(personalNeed.ItemType, personalNeed.ItemTemplateId);
					sbyte b = CalcVillagerTreasuryNeedWaitTime(value2, grade);
					sbyte b2 = CalcVillagerTreasuryNeedWaitTime(value, grade);
					personalNeed.RemainingMonths += Convert.ToSByte(b2 - b);
				}
				villagerTreasuryNeed.PersonalNeeds[i] = personalNeed;
			}
			if (personalNeed.RemainingMonths <= 0)
			{
				if (!_timeMeetTreasuryNeedDict.TryGetValue(charId, out var value3))
				{
					value3 = new List<GameData.Domains.Character.Ai.PersonalNeed>();
					_timeMeetTreasuryNeedDict[charId] = value3;
				}
				value3.Add(personalNeed);
			}
		}
	}

	private static sbyte CalcVillagerTreasuryNeedWaitTime(sbyte charClass, sbyte itemGrade)
	{
		return (sbyte)Math.Max(0, itemGrade - charClass);
	}

	private void HandleVillagerTreasuryNeed(DataContext context, int charId, Settlement settlement, sbyte itemType, short templateId)
	{
		GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		GetTaiwuTreasury().Inventory.GetInventoryItemKeyList(itemType, templateId, list);
		if (list.Count == 0)
		{
			ObjectPool<List<ItemKey>>.Instance.Return(list);
			return;
		}
		list.RemoveAll((ItemKey k) => !IsMeetInventoryLoad(character, k) || !ShouldTakeEquipment(character, k, checkEquipmentLoad: true));
		if (list.Count == 0)
		{
			ObjectPool<List<ItemKey>>.Instance.Return(list);
			return;
		}
		ItemKey random = list.GetRandom(context.Random);
		VillagerTakeItemFromTreasury(context, character, random, 1);
		character.AddInventoryItem(context, random, 1);
		HandleVillagerGainTreasuryItem(context, random, character);
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	private void RemoveInvalidVillagerTreasuryNeed(GameData.Domains.Character.Character selfChar, SettlementTreasury settlementTreasury)
	{
		if (!DomainManager.Extra.TryGetVillagerTreasuryNeed(selfChar.GetId(), out var villagerTreasuryNeed))
		{
			return;
		}
		List<GameData.Domains.Character.Ai.PersonalNeed> personalNeeds = villagerTreasuryNeed.PersonalNeeds;
		if (personalNeeds == null || personalNeeds.Count <= 0)
		{
			return;
		}
		villagerTreasuryNeed.PersonalNeeds?.RemoveAll(delegate(GameData.Domains.Character.Ai.PersonalNeed personalNeed)
		{
			List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
			settlementTreasury.Inventory.GetInventoryItemKeyList(personalNeed.ItemType, personalNeed.ItemTemplateId, list);
			if (list.Count == 0)
			{
				ObjectPool<List<ItemKey>>.Instance.Return(list);
				return true;
			}
			list.RemoveAll((ItemKey k) => !ShouldTakeEquipment(selfChar, k, checkEquipmentLoad: false));
			if (list.Count == 0)
			{
				ObjectPool<List<ItemKey>>.Instance.Return(list);
				return true;
			}
			ObjectPool<List<ItemKey>>.Instance.Return(list);
			if (selfChar.GetInventory().GetInventoryItemKey(personalNeed.ItemType, personalNeed.ItemTemplateId).IsValid())
			{
				return true;
			}
			return selfChar.GetEquipment().Exist((ItemKey k) => k.ItemType == personalNeed.ItemType && k.TemplateId == personalNeed.ItemTemplateId) ? true : false;
		});
	}

	private void HandleVillagerPersonalNeed(DataContext context, GameData.Domains.Character.Character character, Settlement settlement)
	{
		_villagerGainedItemTypeSet.Clear();
		_villagerItems.Clear();
		ItemKey key;
		int value;
		foreach (KeyValuePair<ItemKey, int> item in character.GetInventory().Items)
		{
			item.Deconstruct(out key, out value);
			ItemKey key2 = key;
			int value2 = value;
			_villagerItems[key2] = value2;
		}
		foreach (KeyValuePair<ItemKey, int> item2 in GetTaiwuTreasury().Inventory.Items)
		{
			item2.Deconstruct(out key, out value);
			ItemKey key3 = key;
			int num = value;
			_villagerItems.TryGetValue(key3, out var value3);
			_villagerItems[key3] = value3 + num;
		}
		CategorizedRegenItems(_villagerItems);
		List<GameData.Domains.Character.Ai.PersonalNeed> personalNeeds = character.GetPersonalNeeds();
		for (int i = 0; i < personalNeeds.Count; i++)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed = personalNeeds[i];
			switch (personalNeed.TemplateId)
			{
			case 2:
			case 4:
			case 5:
				HandleVillagerPersonalNeedForHealingAndDetox(context, character, settlement, personalNeed);
				break;
			case 1:
				HandleVillagerPersonalNeedForHeath(context, character, settlement, personalNeed);
				break;
			case 3:
				HandleVillagerPersonalNeedForNeili(context, character, settlement, personalNeed);
				break;
			case 7:
				HandleVillagerPersonalNeedForWug(context, character, settlement, personalNeed);
				break;
			case 6:
				HandleVillagerPersonalNeedForMainAttributes(context, character, settlement, personalNeed);
				break;
			case 0:
				HandleVillagerPersonalNeedForHappiness(context, character, settlement, personalNeed);
				break;
			case 8:
				HandleVillagerPersonalNeedForGainResource(context, character, settlement, personalNeed, i);
				break;
			case 10:
				HandleVillagerPersonalNeedForGainItem(context, character, settlement, personalNeed);
				break;
			case 12:
			{
				bool flag = HandleVillagerPersonalNeedForAddPoisonToItem(context, character, settlement, personalNeed);
				sbyte duration = Config.PersonalNeed.Instance[personalNeed.TemplateId].Duration;
				if (flag && personalNeed.RemainingMonths < duration)
				{
					personalNeed.RemainingMonths = duration;
					personalNeeds[i] = personalNeed;
				}
				break;
			}
			}
		}
	}

	private void HandleVillagerNoNeed(DataContext context, GameData.Domains.Character.Character character, Settlement settlement)
	{
		short villagerRoleTemplateId = DomainManager.Extra.GetVillagerRoleTemplateId(character.GetId());
		VillagerRoleItem villagerRoleItem = ((villagerRoleTemplateId >= 0) ? Config.VillagerRole.Instance[villagerRoleTemplateId] : null);
		sbyte charClass = _villagerClassDict[character.GetId()];
		HashSet<sbyte> lifeSkillTypeSet = VillagerRoleNeedLifeSkillBooks[villagerRoleTemplateId];
		bool flag = !_villagerGainedItemTypeSet.Any(ItemType.IsEquipmentItemType) && !DomainManager.Extra.GetManualChangeEquipGroupCharIds().Contains(character.GetId());
		bool flag2 = !_villagerGainedItemTypeSet.Contains(10) && !character.HasReadableBook() && villagerRoleItem != null;
		bool flag3 = !_villagerGainedItemTypeSet.Contains(6) && lifeSkillTypeSet != null && lifeSkillTypeSet.Count > 0;
		if (flag3 && character.GetInventory().Items.Keys.Any((ItemKey k) => k.ItemType == 6 && Config.CraftTool.Instance[k.TemplateId].RequiredLifeSkillTypes.Any((sbyte t) => lifeSkillTypeSet.Contains(t))))
		{
			flag3 = false;
		}
		bool flag4 = !_villagerGainedItemTypeSet.Contains(12) && context.Random.NextFloat() >= _gainItemActionRate;
		bool flag5 = !_villagerGainedItemTypeSet.Contains(5) && context.Random.NextFloat() >= _gainItemActionRate;
		if (!(flag || flag2 || flag3 || flag4 || flag5))
		{
			return;
		}
		HashSet<sbyte> hashSet = _villagerRoleNeedMaterials[villagerRoleTemplateId];
		ItemKey[] equipment = character.GetEquipment();
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		foreach (KeyValuePair<ItemKey, int> item2 in taiwuTreasury.Inventory.Items)
		{
			item2.Deconstruct(out var key, out var _);
			ItemKey itemKey = key;
			bool flag6 = false;
			if (!_villagerSelectableItemKeys.TryGetValue(itemKey.ItemType, out List<(ItemKey, int)> value2))
			{
				value2 = ObjectPool<List<(ItemKey, int)>>.Instance.Get();
				_villagerSelectableItemKeys[itemKey.ItemType] = value2;
			}
			if (ItemType.IsEquipmentItemType(itemKey.ItemType))
			{
				if (!flag)
				{
					continue;
				}
				short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
				for (sbyte b = 0; b < equipment.Length; b++)
				{
					ItemKey selfItemKey = equipment[b];
					if (itemKey.ItemType == EquipmentSlotHelper.GetSlotItemType(b) && (itemKey.ItemType != 3 || (!Config.VillagerRole.Instance.Any((VillagerRoleItem r) => r.Clothing == itemKey.TemplateId) && character.GetAgeGroup() == 2)))
					{
						if (selfItemKey.IsValid())
						{
							if ((itemKey.ItemType != 1 || itemSubType == ItemTemplateHelper.GetItemSubType(selfItemKey.ItemType, selfItemKey.TemplateId)) && CompareVillagerSelectableEquipments(selfItemKey, itemKey, character))
							{
								flag6 = true;
							}
						}
						else
						{
							flag6 = true;
						}
					}
				}
			}
			else if (itemKey.ItemType == 12)
			{
				if (!flag4)
				{
					continue;
				}
				flag6 = true;
			}
			else if (itemKey.ItemType == 5)
			{
				if (!flag5)
				{
					continue;
				}
				sbyte resourceType = ItemTemplateHelper.GetResourceType(itemKey.ItemType, itemKey.TemplateId);
				if (hashSet == null || hashSet.Contains(resourceType))
				{
					flag6 = true;
				}
			}
			else if (itemKey.ItemType == 10)
			{
				if (!flag2 || !character.BookIsReadable(itemKey))
				{
					continue;
				}
				SkillBookItem skillBookItem = Config.SkillBook.Instance[itemKey.TemplateId];
				if (skillBookItem.ItemSubType == 1001)
				{
					if (!villagerRoleItem.LearnableCombatSkillTypes.Contains(skillBookItem.CombatSkillType))
					{
						continue;
					}
					CombatSkillKey objectId = new CombatSkillKey(character.GetId(), skillBookItem.CombatSkillTemplateId);
					if (DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out var element))
					{
						if (!CombatSkillStateHelper.IsBrokenOut(element.GetActivationState()) && !element.CanBreakout())
						{
							flag6 = true;
						}
					}
					else
					{
						flag6 = true;
					}
				}
				else
				{
					if (!villagerRoleItem.LearnableLifeSkillTypes.Contains(skillBookItem.LifeSkillType))
					{
						continue;
					}
					flag6 = true;
				}
			}
			else if (itemKey.ItemType == 6)
			{
				if (!flag3)
				{
					continue;
				}
				if (lifeSkillTypeSet != null)
				{
					CraftToolItem craftToolItem = Config.CraftTool.Instance[itemKey.TemplateId];
					if (craftToolItem.RequiredLifeSkillTypes.Any((sbyte t) => lifeSkillTypeSet.Contains(t)))
					{
						flag6 = true;
					}
				}
			}
			if (flag6)
			{
				value2.Add((itemKey, 0));
			}
		}
		short key2;
		List<(ItemKey, int)> value3;
		foreach (KeyValuePair<short, List<(ItemKey, int)>> villagerSelectableItemKey in _villagerSelectableItemKeys)
		{
			villagerSelectableItemKey.Deconstruct(out key2, out value3);
			List<(ItemKey, int)> items = value3;
			SortVillagerSelectableItemsByGrade(items);
		}
		if (flag)
		{
			for (sbyte b2 = 0; b2 < equipment.Length; b2++)
			{
				sbyte slotItemType = EquipmentSlotHelper.GetSlotItemType(b2);
				if (_villagerSelectableItemKeys.TryGetValue(slotItemType, out List<(ItemKey, int)> value4) && value4.Count > 0)
				{
					SortVillagerSelectableEquipments(value4, character);
					TakeHighestScoreVillagerSelectableItem(context, character, settlement, charClass, slotItemType, b2);
				}
			}
		}
		if (flag2 && _villagerSelectableItemKeys.TryGetValue(10, out List<(ItemKey, int)> value5))
		{
			SortVillagerSelectableBooks(value5, character);
			TakeHighestScoreVillagerSelectableItem(context, character, settlement, charClass, 10, -1);
		}
		if (flag3)
		{
			TakeHighestScoreVillagerSelectableItem(context, character, settlement, charClass, 6, -1);
		}
		if (flag4)
		{
			TakeHighestScoreVillagerSelectableItem(context, character, settlement, charClass, 12, -1);
		}
		if (flag5)
		{
			TakeHighestScoreVillagerSelectableItem(context, character, settlement, charClass, 5, -1);
		}
		foreach (KeyValuePair<short, List<(ItemKey, int)>> villagerSelectableItemKey2 in _villagerSelectableItemKeys)
		{
			villagerSelectableItemKey2.Deconstruct(out key2, out value3);
			List<(ItemKey, int)> list = value3;
			List<ItemKey> list2 = ObjectPool<List<ItemKey>>.Instance.Get();
			foreach (var (item, num) in list)
			{
				list2.Add(item);
			}
			UpdateVillagerTreasuryNeed(context, character, settlement, list2, listIsFromTreasuryOnly: true);
			ObjectPool<List<(ItemKey, int)>>.Instance.Return(list);
			ObjectPool<List<ItemKey>>.Instance.Return(list2);
		}
		_villagerSelectableItemKeys.Clear();
	}

	private void TakeHighestScoreVillagerSelectableItem(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, sbyte charClass, sbyte itemType, sbyte slot = -1)
	{
		if (!_villagerSelectableItemKeys.TryGetValue(itemType, out List<(ItemKey, int)> value) || value == null || value.Count <= 0)
		{
			return;
		}
		ItemKey equippedItem = ((slot >= 0) ? character.GetEquipment()[slot] : ItemKey.Invalid);
		short idealClothingTemplateId = character.GetIdealClothingTemplateId();
		VillagerWorkData villagerWorkData = DomainManager.Extra.GetVillagerRole(character.GetId())?.WorkData;
		BuildingBlockData value2 = null;
		if (itemType == 10 && villagerWorkData?.WorkType == 1)
		{
			DomainManager.Building.TryGetElement_BuildingBlocks(new BuildingBlockKey(villagerWorkData.AreaId, villagerWorkData.BlockId, villagerWorkData.BuildingBlockIndex), out value2);
		}
		bool flag = false;
		bool flag2 = false;
		for (int num = value.Count - 1; num >= 0; num--)
		{
			(ItemKey, int) tuple = value[num];
			sbyte grade = ItemTemplateHelper.GetGrade(tuple.Item1.ItemType, tuple.Item1.TemplateId);
			bool flag3 = itemType == 3 && idealClothingTemplateId == tuple.Item1.TemplateId;
			if (itemType == 10)
			{
				if (DomainManager.Extra.GetVillagerRoleTemplateId(character.GetId()) == 5)
				{
					if (flag2 = value2?.ConfigData.RequireCombatSkillType >= 0 && Config.SkillBook.Instance[tuple.Item1.TemplateId].CombatSkillType == value2?.ConfigData.RequireCombatSkillType)
					{
						flag = true;
					}
				}
				else if (flag2 = value2?.ConfigData.RequireLifeSkillType >= 0 && Config.SkillBook.Instance[tuple.Item1.TemplateId].LifeSkillType == value2?.ConfigData.RequireLifeSkillType)
				{
					flag = true;
				}
			}
			if ((flag3 || charClass >= grade) && IsMeetInventoryLoad(character, tuple.Item1) && (!ItemType.IsEquipmentItemType(tuple.Item1.ItemType) || (EquipmentSlotHelper.IsItemMeetSlot(slot, tuple.Item1) && ShouldTakeEquipment(character, equippedItem, tuple.Item1, checkEquipmentLoad: true))) && (!(itemType == 10 && flag) || flag2))
			{
				VillagerTakeItemFromTreasury(context, character, tuple.Item1, 1);
				character.AddInventoryItem(context, tuple.Item1, 1);
				HandleVillagerGainTreasuryItem(context, tuple.Item1, character);
				value.RemoveAt(num);
				break;
			}
		}
	}

	private void RemoveInvalidVillagerPersonalNeed(GameData.Domains.Character.Character selfChar)
	{
		List<GameData.Domains.Character.Ai.PersonalNeed> personalNeeds = selfChar.GetPersonalNeeds();
		for (int num = personalNeeds.Count - 1; num >= 0; num--)
		{
			GameData.Domains.Character.Ai.PersonalNeed need = personalNeeds[num];
			if (!need.CheckValid(selfChar))
			{
				CollectionUtils.SwapAndRemove(personalNeeds, num);
			}
		}
	}

	private void HandleVillagerGainTreasuryItem(DataContext context, ItemKey itemKey, GameData.Domains.Character.Character character)
	{
		sbyte itemType = itemKey.ItemType;
		sbyte b = itemType;
		if ((uint)b <= 4u)
		{
			HandleVillagerGainTreasuryEquipment(context, itemKey, character);
		}
	}

	private void HandleVillagerGainTreasuryEquipment(DataContext context, ItemKey itemKey, GameData.Domains.Character.Character character)
	{
		Settlement settlement = DomainManager.Organization.GetSettlement(_taiwuVillageSettlementId);
		ItemKey[] equipment = character.GetEquipment();
		short idealClothingTemplateId = character.GetIdealClothingTemplateId();
		IReadOnlyDictionary<ItemKey, int> readOnlyDictionary = (DomainManager.Taiwu.IsInGroup(character.GetId()) ? DomainManager.Extra.GetTaiwuGiftItems(character.GetId()) : Inventory.Empty);
		for (sbyte b = 0; b < equipment.Length; b++)
		{
			if (EquipmentSlotHelper.IsItemMeetSlot(b, itemKey))
			{
				ItemKey oldItemKey = equipment[b];
				if (!oldItemKey.IsValid() && IsMeetEquipmentLoad(character, oldItemKey, itemKey))
				{
					character.ChangeEquipment(context, -1, b, itemKey);
					return;
				}
			}
		}
		for (sbyte b2 = 0; b2 < equipment.Length; b2++)
		{
			if (EquipmentSlotHelper.IsItemMeetSlot(b2, itemKey))
			{
				ItemKey itemKey2 = equipment[b2];
				if (IsMeetEquipmentLoad(character, itemKey2, itemKey) && itemKey2.IsValid() && CompareEquipmentScore(itemKey2, itemKey, character))
				{
					character.ChangeEquipment(context, b2, -1, itemKey2);
					character.ChangeEquipment(context, -1, b2, itemKey);
					bool flag = itemKey.ItemType == 3 && itemKey.TemplateId == idealClothingTemplateId && itemKey2.TemplateId != idealClothingTemplateId;
					if (readOnlyDictionary.ContainsKey(itemKey))
					{
						flag = true;
					}
					if (!flag)
					{
						character.RemoveInventoryItem(context, itemKey2, 1, deleteItem: false);
						DomainManager.Taiwu.VillagerStoreItemInTreasury(context, character, itemKey2, 1);
					}
					break;
				}
			}
		}
	}

	private ItemKey RemovePersonalNeedSelectedItem<T>(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, List<(T item, int amount)> items, List<ItemKey> selections) where T : ItemBase
	{
		ItemKey itemKey = InternalRemovePersonalNeedSelectedItem(context, character, settlement, selections);
		if (itemKey.IsValid())
		{
			int num = items.FindIndex(((T item, int amount) i) => i.item.GetItemKey() == itemKey);
			if (num >= 0)
			{
				var (item, num2) = items[num];
				if (num2 > 1)
				{
					items[num] = (item, num2 - 1);
				}
				else
				{
					items.RemoveAt(num);
				}
			}
		}
		return itemKey;
	}

	private ItemKey InternalRemovePersonalNeedSelectedItem(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, List<ItemKey> itemKeyList)
	{
		sbyte b = _villagerClassDict[character.GetId()];
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		for (int num = itemKeyList.Count - 1; num >= 0; num--)
		{
			ItemKey itemKey = itemKeyList[num];
			sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
			int inventoryItemCount = taiwuTreasury.Inventory.GetInventoryItemCount(itemKey);
			if (inventoryItemCount > 0 && b >= grade)
			{
				VillagerTakeItemFromTreasury(context, character, itemKey, 1);
				itemKeyList.RemoveAt(num);
				return itemKey;
			}
			int inventoryItemCount2 = character.GetInventory().GetInventoryItemCount(itemKey);
			if (inventoryItemCount2 > 0)
			{
				character.RemoveInventoryItem(context, itemKey, 1, deleteItem: true);
				itemKeyList.RemoveAt(num);
				return itemKey;
			}
		}
		return ItemKey.Invalid;
	}

	private void UpdateVillagerTreasuryNeed(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, List<ItemKey> itemKeyList, bool listIsFromTreasuryOnly)
	{
		sbyte charClass = _villagerClassDict[character.GetId()];
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		ItemKey targetItemKey = ItemKey.Invalid;
		for (int num = itemKeyList.Count - 1; num >= 0; num--)
		{
			ItemKey itemKey = itemKeyList[num];
			if (!listIsFromTreasuryOnly)
			{
				int inventoryItemCount = taiwuTreasury.Inventory.GetInventoryItemCount(itemKey);
				if (inventoryItemCount <= 0)
				{
					continue;
				}
			}
			if (!CanNeedTreasuryItem(character, settlement, itemKey, charClass))
			{
				continue;
			}
			targetItemKey = itemKey;
			break;
		}
		if (!targetItemKey.IsValid())
		{
			return;
		}
		if (!DomainManager.Extra.TryGetVillagerTreasuryNeed(character.GetId(), out var villagerTreasuryNeed))
		{
			villagerTreasuryNeed = new VillagerTreasuryNeed();
			villagerTreasuryNeed.PersonalNeeds = new List<GameData.Domains.Character.Ai.PersonalNeed>();
		}
		int num2 = villagerTreasuryNeed.PersonalNeeds.FindIndex((GameData.Domains.Character.Ai.PersonalNeed n) => n.ItemType == targetItemKey.ItemType);
		GameData.Domains.Character.Ai.PersonalNeed personalNeed;
		if (num2 >= 0)
		{
			personalNeed = villagerTreasuryNeed.PersonalNeeds[num2];
			ItemKey selfItemKey = new ItemKey(personalNeed.ItemType, 0, personalNeed.ItemTemplateId, -1);
			targetItemKey.Id = -1;
			if (!CompareVillagerSelectableItems(selfItemKey, targetItemKey, character))
			{
				return;
			}
			sbyte grade = ItemTemplateHelper.GetGrade(targetItemKey.ItemType, targetItemKey.TemplateId);
			sbyte remainingMonths = CalcVillagerTreasuryNeedWaitTime(charClass, grade);
			personalNeed.RemainingMonths = remainingMonths;
			personalNeed.ItemTemplateId = targetItemKey.TemplateId;
			villagerTreasuryNeed.PersonalNeeds[num2] = personalNeed;
		}
		else
		{
			sbyte grade2 = ItemTemplateHelper.GetGrade(targetItemKey.ItemType, targetItemKey.TemplateId);
			sbyte remainingMonths2 = CalcVillagerTreasuryNeedWaitTime(charClass, grade2);
			personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(10, targetItemKey.ItemType, targetItemKey.TemplateId);
			personalNeed.RemainingMonths = remainingMonths2;
			villagerTreasuryNeed.PersonalNeeds.Add(personalNeed);
			DomainManager.Extra.SetVillagerTreasuryNeed(context, character.GetId(), villagerTreasuryNeed);
		}
		AddNeededItem(personalNeed, character.GetId());
	}

	private bool CanNeedTreasuryItem(GameData.Domains.Character.Character character, Settlement settlement, ItemKey itemKey, int charClass)
	{
		SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(character.GetId());
		short curInfluencePower = settlementCharacter.GetInfluencePower();
		sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
		if (grade <= charClass)
		{
			return false;
		}
		ItemKey invalid = ItemKey.Invalid;
		invalid.ItemType = itemKey.ItemType;
		invalid.TemplateId = itemKey.TemplateId;
		if (!_treasuryNeededItemDict.TryGetValue(invalid, out var value))
		{
			return true;
		}
		int inventoryItemCount = GetTaiwuTreasury().Inventory.GetInventoryItemCount(invalid.ItemType, invalid.TemplateId);
		if (inventoryItemCount > value.Count)
		{
			return true;
		}
		return value.Keys.All(delegate(int charId)
		{
			if (DomainManager.Organization.TryGetSettlementCharacter(charId, out settlementCharacter))
			{
				short influencePower = settlementCharacter.GetInfluencePower();
				return curInfluencePower > influencePower;
			}
			return true;
		});
	}

	public bool IsMeetWorkStatus(GameData.Domains.Character.Character character, Settlement settlement)
	{
		byte villagerWorkStatus = GetVillagerWorkStatus(character);
		bool flag = villagerWorkStatus != 2;
		sbyte grade = character.GetOrganizationInfo().Grade;
		bool flag2 = grade == 0;
		if (flag && flag2)
		{
			if (settlement.GetLocation().AreaId != character.GetLocation().AreaId)
			{
				return false;
			}
			if (!DomainManager.Map.IsLocationInSettlementInfluenceRange(character.GetLocation(), _taiwuVillageSettlementId))
			{
				return false;
			}
		}
		return true;
	}

	private bool IsMeetInventoryLoad(GameData.Domains.Character.Character character, ItemKey itemKey)
	{
		int weight = DomainManager.Item.GetBaseItem(itemKey).GetWeight();
		if (weight == 0)
		{
			return true;
		}
		int currInventoryLoad = character.GetCurrInventoryLoad();
		int maxInventoryLoad = character.GetMaxInventoryLoad();
		return currInventoryLoad + weight <= maxInventoryLoad;
	}

	private bool IsMeetEquipmentLoad(GameData.Domains.Character.Character character, ItemKey oldItemKey, ItemKey newItemKey)
	{
		int num = (oldItemKey.IsValid() ? DomainManager.Item.GetBaseItem(newItemKey).GetWeight() : 0);
		if (num == 0)
		{
			return true;
		}
		int num2 = (oldItemKey.IsValid() ? DomainManager.Item.GetBaseItem(oldItemKey).GetWeight() : 0);
		int currEquipmentLoad = character.GetCurrEquipmentLoad();
		int maxEquipmentLoad = character.GetMaxEquipmentLoad();
		return currEquipmentLoad - num2 + num <= maxEquipmentLoad;
	}

	private bool ShouldTakeEquipment(GameData.Domains.Character.Character character, ItemKey equippedItem, ItemKey itemKey, bool checkEquipmentLoad)
	{
		if (checkEquipmentLoad && !IsMeetEquipmentLoad(character, equippedItem, itemKey))
		{
			return false;
		}
		if (itemKey.ItemType == 0)
		{
			short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			if (character.GetEquipment().Exist((ItemKey e) => e.IsValid() && itemSubType == ItemTemplateHelper.GetItemSubType(e.ItemType, e.TemplateId)))
			{
				return false;
			}
		}
		if (equippedItem.IsValid())
		{
			return CompareEquipmentScore(equippedItem, itemKey, character);
		}
		return true;
	}

	private bool ShouldTakeEquipment(GameData.Domains.Character.Character character, ItemKey targetItemKey, bool checkEquipmentLoad)
	{
		if (!ItemType.IsEquipmentItemType(targetItemKey.ItemType))
		{
			return true;
		}
		ItemKey[] equipment = character.GetEquipment();
		for (sbyte b = 0; b < equipment.Length; b++)
		{
			if (EquipmentSlotHelper.IsItemMeetSlot(b, targetItemKey))
			{
				ItemKey equippedItem = equipment[b];
				if (ShouldTakeEquipment(character, equippedItem, targetItemKey, checkEquipmentLoad))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool CompareEquipmentScore(ItemKey selfKey, ItemKey targetKey, GameData.Domains.Character.Character character)
	{
		if (!ItemType.IsEquipmentItemType(selfKey.ItemType))
		{
			return false;
		}
		if (selfKey.ItemType == 0)
		{
			_availableWeapons.Clear();
			_suitableWeapons.Clear();
			_availableWeapons.Add((selfKey, 0));
			_availableWeapons.Add((targetKey, 0));
			Equipping.GetWeaponScores(character, _availableWeapons, _suitableWeapons, _fixedBestWeapons);
			return _availableWeapons[1].score > _availableWeapons[0].score;
		}
		if (selfKey.ItemType == 3)
		{
			short idealClothingTemplateId = character.GetIdealClothingTemplateId();
			if (selfKey.TemplateId == idealClothingTemplateId)
			{
				return false;
			}
			if (targetKey.TemplateId == idealClothingTemplateId)
			{
				return true;
			}
			bool flag = Config.VillagerRole.Instance.Any((VillagerRoleItem r) => r.Clothing == selfKey.TemplateId);
			bool flag2 = Config.VillagerRole.Instance.Any((VillagerRoleItem r) => r.Clothing == targetKey.TemplateId);
			if (flag != flag2)
			{
				return !flag2;
			}
			(int, bool) tuple = Equipping.CalcEquipmentScore(selfKey, -1);
			return Equipping.CalcEquipmentScore(targetKey, -1).score > tuple.Item1;
		}
		(int, bool) tuple2 = Equipping.CalcEquipmentScore(selfKey, -1);
		return Equipping.CalcEquipmentScore(targetKey, -1).score > tuple2.Item1;
	}

	private unsafe void CategorizedRegenItems(IReadOnlyDictionary<ItemKey, int> inventoryItems)
	{
		List<(GameData.Domains.Item.Medicine, int)>[] categorizedMedicines = _categorizedMedicines;
		foreach (List<(GameData.Domains.Item.Medicine, int)> list in categorizedMedicines)
		{
			list.Clear();
		}
		List<(GameData.Domains.Item.Food, int)>[] foodsForMainAttributes = _foodsForMainAttributes;
		foreach (List<(GameData.Domains.Item.Food, int)> list2 in foodsForMainAttributes)
		{
			list2.Clear();
		}
		_teaWinesForHappiness.Clear();
		_itemsForNeili.Clear();
		foreach (var (itemKey2, item) in inventoryItems)
		{
			switch (itemKey2.ItemType)
			{
			case 7:
			{
				FoodItem foodItem = Config.Food.Instance[itemKey2.TemplateId];
				for (sbyte b = 0; b < 6; b++)
				{
					if (foodItem.MainAttributesRegen.Items[b] > 0)
					{
						GameData.Domains.Item.Food element_Foods = DomainManager.Item.GetElement_Foods(itemKey2.Id);
						_foodsForMainAttributes[b].Add((element_Foods, item));
						break;
					}
				}
				break;
			}
			case 8:
			{
				MedicineItem medicineItem = Config.Medicine.Instance[itemKey2.TemplateId];
				if (medicineItem.EffectType != EMedicineEffectType.Invalid)
				{
					GameData.Domains.Item.Medicine element_Medicines = DomainManager.Item.GetElement_Medicines(itemKey2.Id);
					_categorizedMedicines[(int)medicineItem.EffectType].Add((element_Medicines, item));
				}
				break;
			}
			case 9:
				if (DomainManager.Item.GetBaseItem(itemKey2).GetHappinessChange() > 0)
				{
					GameData.Domains.Item.TeaWine element_TeaWines = DomainManager.Item.GetElement_TeaWines(itemKey2.Id);
					_teaWinesForHappiness.Add((element_TeaWines, item));
				}
				break;
			case 12:
			{
				MiscItem miscItem = Config.Misc.Instance[itemKey2.TemplateId];
				GameData.Domains.Item.Misc element_Misc = DomainManager.Item.GetElement_Misc(itemKey2.Id);
				if (miscItem.Neili > 0)
				{
					_itemsForNeili.Add((element_Misc, item));
				}
				break;
			}
			}
		}
	}

	private void AddNeededItem(GameData.Domains.Character.Ai.PersonalNeed personalNeed, int charId)
	{
		ItemKey invalid = ItemKey.Invalid;
		invalid.ItemType = personalNeed.ItemType;
		invalid.TemplateId = personalNeed.ItemTemplateId;
		if (!_treasuryNeededItemDict.TryGetValue(invalid, out var value))
		{
			value = new Dictionary<int, sbyte>();
			_treasuryNeededItemDict[invalid] = value;
		}
		value[charId] = personalNeed.RemainingMonths;
	}

	private unsafe void HandleVillagerPersonalNeedForHealingAndDetox(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		sbyte currMaxEatingSlotsCount = character.GetCurrMaxEatingSlotsCount();
		int num = character.GetEatingItems().GetAvailableEatingSlotsCount(currMaxEatingSlotsCount);
		PoisonInts poisonResists = character.GetPoisonResists();
		MainAttributes mainAttributes = character.GetCurrMainAttributes();
		Injuries injuries = character.GetInjuries();
		PoisonInts poisoned = character.GetPoisoned();
		short disorderOfQi = character.GetDisorderOfQi();
		List<(GameData.Domains.Item.Medicine, int)>[] categorizedMedicines = _categorizedMedicines;
		List<(GameData.Domains.Item.Medicine, int)> list = categorizedMedicines[0];
		List<(GameData.Domains.Item.Medicine, int)> list2 = categorizedMedicines[1];
		list.Sort(EatingItemComparer.MedicineInjury);
		list2.Sort(EatingItemComparer.MedicineInjury);
		List<ItemKey> list3 = ObjectPool<List<ItemKey>>.Instance.Get();
		for (sbyte b = 0; b < 7; b++)
		{
			var (b2, b3) = injuries.Get(b);
			if (b2 > 2 && list.Count > 0)
			{
				int num2 = character.SelectTopicalMedicineIndex(list, b2, ref mainAttributes, list3);
				if (num2 >= 0)
				{
					ItemKey itemKey = RemovePersonalNeedSelectedItem(context, character, settlement, list, list3);
					if (itemKey.IsValid())
					{
						DomainManager.Item.RemoveItem(context, itemKey);
						character.ApplyTopicalMedicine(context, itemKey);
						if (personalNeed.CheckValid(character))
						{
							UpdateVillagerTreasuryNeed(context, character, settlement, list3, listIsFromTreasuryOnly: false);
						}
					}
					else
					{
						UpdateVillagerTreasuryNeed(context, character, settlement, list3, listIsFromTreasuryOnly: false);
					}
				}
			}
			if (b3 > 2 && list2.Count > 0)
			{
				int num3 = character.SelectTopicalMedicineIndex(list2, b3, ref mainAttributes, list3);
				if (num3 >= 0)
				{
					ItemKey itemKey2 = RemovePersonalNeedSelectedItem(context, character, settlement, list2, list3);
					if (itemKey2.IsValid())
					{
						DomainManager.Item.RemoveItem(context, itemKey2);
						character.ApplyTopicalMedicine(context, itemKey2);
						if (personalNeed.CheckValid(character))
						{
							UpdateVillagerTreasuryNeed(context, character, settlement, list3, listIsFromTreasuryOnly: false);
						}
					}
				}
				else
				{
					UpdateVillagerTreasuryNeed(context, character, settlement, list3, listIsFromTreasuryOnly: false);
				}
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list3);
		categorizedMedicines[3].Sort(EatingItemComparer.MedicineQiDisorder);
		int* ptr = stackalloc int[3];
		while (num > 1)
		{
			*ptr = GameData.Domains.Character.Character.GetHealthChangeDueToInjuries(ref injuries) << 8;
			ptr[1] = (GameData.Domains.Character.Character.GetHealthChangeDueToPoisons(ref poisoned, ref poisonResists) << 8) + 1;
			ptr[2] = (character.GetHealthChangeDueToDisorderOfQi(disorderOfQi) << 8) + 2;
			CollectionUtils.Sort(ptr, 3);
			if (*ptr >> 8 >= 0 || !HandleVillagerPersonalNeedForHealingAndDetoxHelper(context, ptr, character, settlement, personalNeed))
			{
				break;
			}
			num--;
		}
	}

	private unsafe bool HandleVillagerPersonalNeedForHealingAndDetoxHelper(DataContext context, int* healthChanges, GameData.Domains.Character.Character character, Settlement settlement, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		PoisonInts poisonResists = character.GetPoisonResists();
		Injuries injuries = character.GetInjuries();
		PoisonInts poisoned = character.GetPoisoned();
		short disorderOfQi = character.GetDisorderOfQi();
		List<(GameData.Domains.Item.Medicine, int)>[] categorizedMedicines = _categorizedMedicines;
		int* ptr = stackalloc int[6];
		for (int i = 0; i < 3; i++)
		{
			int num = healthChanges[i] & 0xFF;
			int num2 = healthChanges[i] >> 8;
			if (num2 >= 0)
			{
				break;
			}
			switch (num)
			{
			case 0:
			{
				List<(GameData.Domains.Item.Medicine, int)> list3 = categorizedMedicines[0];
				List<(GameData.Domains.Item.Medicine, int)> list4 = categorizedMedicines[1];
				sbyte b = sbyte.MaxValue;
				sbyte b2 = sbyte.MaxValue;
				sbyte b3 = 0;
				sbyte b4 = 0;
				for (sbyte b5 = 0; b5 < 7; b5++)
				{
					(sbyte outer, sbyte inner) tuple = injuries.Get(b5);
					sbyte item = tuple.outer;
					sbyte item2 = tuple.inner;
					b3 += item;
					b4 += item2;
					if (item > 0 && item < b2)
					{
						b2 = item;
					}
					if (item2 > 0 && item2 < b)
					{
						b = item2;
					}
				}
				int num4 = -1;
				if (list3.Count > 0 && b2 > b && b2 <= 6)
				{
					num4 = character.SelectMedicineIndexForInjury(list3, b2, b3, list);
					if (num4 >= 0)
					{
						ItemKey itemKey2 = RemovePersonalNeedSelectedItem(context, character, settlement, list3, list);
						if (itemKey2.IsValid())
						{
							character.AddEatingItem(context, itemKey2);
							if (personalNeed.CheckValid(character))
							{
								UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
							}
							return true;
						}
						UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
					}
				}
				if (list4.Count <= 0 || b > 6)
				{
					break;
				}
				num4 = character.SelectMedicineIndexForInjury(list4, b, b4, list);
				if (num4 < 0)
				{
					break;
				}
				ItemKey itemKey3 = RemovePersonalNeedSelectedItem(context, character, settlement, list4, list);
				if (itemKey3.IsValid())
				{
					character.AddEatingItem(context, itemKey3);
					if (personalNeed.CheckValid(character))
					{
						UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
					}
					return true;
				}
				UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
				break;
			}
			case 1:
			{
				List<(GameData.Domains.Item.Medicine, int)> list5 = _categorizedMedicines[4];
				if (list5.Count <= 0)
				{
					break;
				}
				list5.Sort(character.CompareDetoxPoisonMedicines);
				for (sbyte b6 = 0; b6 < 6; b6++)
				{
					int num5 = ((poisonResists.Items[b6] < 1000) ? PoisonsAndLevels.CalcPoisonedLevel(poisoned.Items[b6]) : 0);
					ptr[b6] = (num5 << 8) + b6;
				}
				CollectionUtils.Sort(ptr, 6);
				for (sbyte b7 = 5; b7 >= 0; b7--)
				{
					sbyte b8 = (sbyte)(ptr[b7] & 0xFF);
					sbyte b9 = (sbyte)(ptr[b7] >> 8);
					if (b9 <= 0)
					{
						break;
					}
					int num6 = character.SelectMedicineIndexForDetoxPoison(list5, b8, b9, poisoned.Items[b8], list);
					if (num6 >= 0)
					{
						ItemKey itemKey4 = RemovePersonalNeedSelectedItem(context, character, settlement, list5, list);
						if (itemKey4.IsValid())
						{
							character.AddEatingItem(context, itemKey4);
							if (personalNeed.CheckValid(character))
							{
								UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
							}
							return true;
						}
						UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
					}
				}
				break;
			}
			case 2:
			{
				List<(GameData.Domains.Item.Medicine, int)> list2 = _categorizedMedicines[3];
				int num3 = character.SelectMedicineIndexForQiDisorder(list2, disorderOfQi, list);
				if (num3 < 0)
				{
					break;
				}
				ItemKey itemKey = RemovePersonalNeedSelectedItem(context, character, settlement, list2, list);
				if (itemKey.IsValid())
				{
					character.AddEatingItem(context, itemKey);
					if (personalNeed.CheckValid(character))
					{
						UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
					}
					return true;
				}
				UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
				break;
			}
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
		return false;
	}

	private void HandleVillagerPersonalNeedForHeath(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		short health = character.GetHealth();
		short leftMaxHealth = character.GetLeftMaxHealth();
		sbyte currMaxEatingSlotsCount = character.GetCurrMaxEatingSlotsCount();
		sbyte availableEatingSlot = character.GetEatingItems().GetAvailableEatingSlot(currMaxEatingSlotsCount);
		List<(GameData.Domains.Item.Medicine, int)> list2 = _categorizedMedicines[2];
		if (availableEatingSlot >= 0 && list2.Count > 0)
		{
			list2.Sort(EatingItemComparer.MedicineEffect);
			while (availableEatingSlot >= 0 && health < leftMaxHealth)
			{
				int num = character.SelectMedicineIndexForHealth(list2, health, leftMaxHealth, list);
				if (num < 0)
				{
					break;
				}
				ItemKey itemKey = RemovePersonalNeedSelectedItem(context, character, settlement, list2, list);
				if (!itemKey.IsValid())
				{
					UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
					break;
				}
				character.AddEatingItem(context, itemKey);
				if (personalNeed.CheckValid(character))
				{
					UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
				}
				health = character.GetHealth();
				leftMaxHealth = character.GetLeftMaxHealth();
				currMaxEatingSlotsCount = character.GetCurrMaxEatingSlotsCount();
				availableEatingSlot = character.GetEatingItems().GetAvailableEatingSlot(currMaxEatingSlotsCount);
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	private unsafe void HandleVillagerPersonalNeedForWug(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		EatingItems eatingItems = character.GetEatingItems();
		sbyte currMaxEatingSlotsCount = character.GetCurrMaxEatingSlotsCount();
		sbyte availableEatingSlot = eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
		List<(GameData.Domains.Item.Medicine, int)> list2 = _categorizedMedicines[5];
		List<(GameData.Domains.Item.Medicine, int)> list3 = _categorizedMedicines[6];
		if (availableEatingSlot >= 0 && (list2.Count > 0 || list3.Count > 0))
		{
			list2.Sort(EatingItemComparer.MedicineGrade);
			list3.Sort(EatingItemComparer.MedicineGrade);
			for (sbyte b = 0; b < 9; b++)
			{
				ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[b];
				if (EatingItems.IsWug(itemKey) && !itemKey.IsValid())
				{
					MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
					while (availableEatingSlot >= 0 && EatingItems.IsWug(eatingItems.Get(b)) && !eatingItems.Get(b).IsValid())
					{
						int num = character.SelectMedicineIndexForWug(list2, medicineItem.WugType, eatingItems.Durations[b], list);
						if (num >= 0)
						{
							ItemKey itemKey2 = RemovePersonalNeedSelectedItem(context, character, settlement, list2, list);
							if (itemKey.IsValid())
							{
								character.AddEatingItem(context, itemKey2);
								if (personalNeed.CheckValid(character))
								{
									UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
								}
								currMaxEatingSlotsCount = character.GetCurrMaxEatingSlotsCount();
								eatingItems = character.GetEatingItems();
								availableEatingSlot = eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
								continue;
							}
							UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
						}
						num = GameData.Domains.Character.Character.SelectPoisonIndexForWug(list3, medicineItem.WugType, eatingItems.Durations[b], list);
						if (num >= 0)
						{
							ItemKey itemKey3 = RemovePersonalNeedSelectedItem(context, character, settlement, list3, list);
							if (itemKey3.IsValid())
							{
								character.AddEatingItem(context, itemKey3);
								if (personalNeed.CheckValid(character))
								{
									UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
								}
								currMaxEatingSlotsCount = character.GetCurrMaxEatingSlotsCount();
								eatingItems = character.GetEatingItems();
								availableEatingSlot = eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
								continue;
							}
							UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
							break;
						}
						break;
					}
				}
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	private void HandleVillagerPersonalNeedForNeili(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		int currNeili = character.GetCurrNeili();
		int maxNeili = character.GetMaxNeili();
		List<(GameData.Domains.Item.Misc, int)> itemsForNeili = _itemsForNeili;
		if (itemsForNeili.Count > 0)
		{
			itemsForNeili.Sort(EatingItemComparer.MiscNeili);
			while (currNeili < maxNeili)
			{
				int num = character.SelectItemIndexForNeili(itemsForNeili, currNeili, maxNeili, list);
				if (num < 0)
				{
					break;
				}
				ItemKey itemKey = RemovePersonalNeedSelectedItem(context, character, settlement, itemsForNeili, list);
				if (!itemKey.IsValid())
				{
					UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
					break;
				}
				character.AddEatingItem(context, itemKey);
				if (personalNeed.CheckValid(character))
				{
					UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
				}
				currNeili = character.GetCurrNeili();
				maxNeili = character.GetMaxNeili();
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	private unsafe void HandleVillagerPersonalNeedForMainAttributes(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		MainAttributes currMainAttributes = character.GetCurrMainAttributes();
		MainAttributes maxMainAttributes = character.GetMaxMainAttributes();
		sbyte currMaxEatingSlotsCount = character.GetCurrMaxEatingSlotsCount();
		sbyte availableEatingSlot = character.GetEatingItems().GetAvailableEatingSlot(currMaxEatingSlotsCount);
		for (sbyte b = 0; b < 6; b++)
		{
			short num = maxMainAttributes.Items[b];
			if (currMainAttributes.Items[b] * 2 < num)
			{
				List<(GameData.Domains.Item.Food, int)> list2 = _foodsForMainAttributes[b];
				if (availableEatingSlot >= 0 && list2.Count > 0)
				{
					sbyte behaviorType = character.GetBehaviorType();
					sbyte b2 = AiHelper.UpdateStatusConstants.EatForbiddenFoodChance[behaviorType];
					bool flag = character.IsForbiddenToEatMeat();
					short aiActionRateAdjust = DomainManager.Extra.GetAiActionRateAdjust(character.GetId(), 6, -1);
					bool allowMeat = !flag || context.Random.CheckPercentProb(b2 + aiActionRateAdjust);
					list2.Sort(EatingItemComparer.FoodMainAttributes[b]);
					while (availableEatingSlot >= 0 && currMainAttributes.Items[b] < num)
					{
						int num2 = character.SelectFoodIndexForMainAttributes(list2, b, currMainAttributes.Items[b], num, allowMeat, list);
						if (num2 < 0)
						{
							break;
						}
						ItemKey itemKey = RemovePersonalNeedSelectedItem(context, character, settlement, list2, list);
						if (!itemKey.IsValid())
						{
							UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
							break;
						}
						character.AddEatingItem(context, itemKey);
						if (personalNeed.CheckValid(character))
						{
							UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
						}
						currMainAttributes = character.GetCurrMainAttributes();
						maxMainAttributes = character.GetMaxMainAttributes();
						currMaxEatingSlotsCount = character.GetCurrMaxEatingSlotsCount();
						availableEatingSlot = character.GetEatingItems().GetAvailableEatingSlot(currMaxEatingSlotsCount);
					}
				}
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	private void HandleVillagerPersonalNeedForHappiness(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		sbyte item = HappinessType.Ranges[3].min;
		sbyte b = character.GetHappiness();
		sbyte currMaxEatingSlotsCount = character.GetCurrMaxEatingSlotsCount();
		sbyte availableEatingSlot = character.GetEatingItems().GetAvailableEatingSlot(currMaxEatingSlotsCount);
		List<(GameData.Domains.Item.TeaWine, int)> teaWinesForHappiness = _teaWinesForHappiness;
		if (availableEatingSlot >= 0 && teaWinesForHappiness.Count > 0)
		{
			sbyte behaviorType = character.GetBehaviorType();
			short aiActionRateAdjust = DomainManager.Extra.GetAiActionRateAdjust(character.GetId(), 6, -1);
			sbyte b2 = AiHelper.UpdateStatusConstants.EatForbiddenFoodChance[behaviorType];
			bool flag = character.IsForbiddenToDrinkingWines();
			bool allowWines = !flag || context.Random.CheckPercentProb(b2 + aiActionRateAdjust);
			teaWinesForHappiness.Sort(EatingItemComparer.TeaWineHappiness);
			while (availableEatingSlot >= 0 && b < item)
			{
				int num = character.SelectTeaWineForHappiness(teaWinesForHappiness, b, item, allowWines, list);
				if (num < 0)
				{
					break;
				}
				ItemKey itemKey = RemovePersonalNeedSelectedItem(context, character, settlement, teaWinesForHappiness, list);
				if (!itemKey.IsValid())
				{
					UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
					break;
				}
				character.AddEatingItem(context, itemKey);
				TeaWineItem teaWineItem = Config.TeaWine.Instance[itemKey.TemplateId];
				b = (sbyte)Math.Clamp(b + teaWineItem.EatHappinessChange, -119, 119);
				if (flag && teaWineItem.ItemSubType == 901)
				{
					sbyte b3 = AiHelper.UpdateStatusConstants.EatForbiddenFoodHappinessChange[behaviorType];
					b = (sbyte)Math.Clamp(b + b3, -119, 119);
				}
				character.SetHappiness(b, context);
				if (personalNeed.CheckValid(character))
				{
					UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: false);
				}
				currMaxEatingSlotsCount = character.GetCurrMaxEatingSlotsCount();
				availableEatingSlot = character.GetEatingItems().GetAvailableEatingSlot(currMaxEatingSlotsCount);
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	private void HandleVillagerPersonalNeedForGainItem(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!IsMeetWorkStatus(character, settlement))
		{
			return;
		}
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		bool flag = personalNeed.ItemType == 3 && Config.VillagerRole.Instance.Any((VillagerRoleItem r) => r.Clothing == personalNeed.ItemTemplateId);
		int minGradeOffset = ((!flag) ? (-2) : 0);
		ItemKey itemInSameGroup = taiwuTreasury.Inventory.GetItemInSameGroup(personalNeed.ItemType, personalNeed.ItemTemplateId, minGradeOffset);
		if (!itemInSameGroup.IsValid())
		{
			return;
		}
		sbyte grade = ItemTemplateHelper.GetGrade(itemInSameGroup.ItemType, itemInSameGroup.TemplateId);
		if (!flag)
		{
			short groupId = ItemTemplateHelper.GetGroupId(personalNeed.ItemType, personalNeed.ItemTemplateId);
			List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
			foreach (var (item, _) in taiwuTreasury.Inventory.Items)
			{
				if (item.ItemType != personalNeed.ItemType)
				{
					continue;
				}
				short groupId2 = ItemTemplateHelper.GetGroupId(item.ItemType, item.TemplateId);
				if (groupId != groupId2)
				{
					continue;
				}
				sbyte grade2 = ItemTemplateHelper.GetGrade(item.ItemType, item.TemplateId);
				if (grade2 < grade)
				{
					continue;
				}
				if (itemInSameGroup.ItemType == 11)
				{
					ItemBase baseItem = DomainManager.Item.GetBaseItem(itemInSameGroup);
					if (baseItem.GetCurrDurability() <= 0)
					{
						continue;
					}
				}
				list.Add(item);
			}
			if (personalNeed.ItemType == 10)
			{
				sbyte bookType = -1;
				short villagerRoleTemplateId = DomainManager.Extra.GetVillagerRoleTemplateId(character.GetId());
				if (villagerRoleTemplateId < 0)
				{
					SortVillagerSelectableItemsByGrade(list);
				}
				else
				{
					VillagerWorkData villagerWorkData = DomainManager.Extra.GetVillagerRole(character.GetId())?.WorkData;
					HashSet<sbyte> lifeSkillTypeSet = VillagerRoleNeedLifeSkillBooks[villagerRoleTemplateId];
					if (villagerRoleTemplateId == 5)
					{
						if (villagerWorkData?.WorkType == 1 && DomainManager.Building.TryGetElement_BuildingBlocks(new BuildingBlockKey(villagerWorkData.AreaId, villagerWorkData.BlockId, villagerWorkData.BuildingBlockIndex), out var value))
						{
							bookType = value.ConfigData.RequireCombatSkillType;
						}
						list.Sort((ItemKey a, ItemKey b2) => CompareCombatSkillBook(a, b2, bookType));
					}
					else if (lifeSkillTypeSet != null && lifeSkillTypeSet.Count > 0)
					{
						if (villagerWorkData?.WorkType == 1 && DomainManager.Building.TryGetElement_BuildingBlocks(new BuildingBlockKey(villagerWorkData.AreaId, villagerWorkData.BlockId, villagerWorkData.BuildingBlockIndex), out var value2))
						{
							bookType = value2.ConfigData.RequireLifeSkillType;
						}
						list.Sort((ItemKey a, ItemKey b2) => CompareLifeSkillBook(a, b2, lifeSkillTypeSet, bookType));
					}
				}
			}
			else
			{
				SortVillagerSelectableItemsByGrade(list);
			}
			UpdateVillagerTreasuryNeed(context, character, settlement, list, listIsFromTreasuryOnly: true);
			ObjectPool<List<ItemKey>>.Instance.Return(list);
		}
		if (itemInSameGroup.ItemType != 3)
		{
			sbyte b = _villagerClassDict[character.GetId()];
			if (b < grade)
			{
				return;
			}
		}
		if (itemInSameGroup.ItemType == 11)
		{
			ItemBase baseItem2 = DomainManager.Item.GetBaseItem(itemInSameGroup);
			if (baseItem2.GetCurrDurability() <= 0)
			{
				return;
			}
		}
		if (IsMeetInventoryLoad(character, itemInSameGroup))
		{
			VillagerTakeItemFromTreasury(context, character, itemInSameGroup, 1);
			character.AddInventoryItem(context, itemInSameGroup, 1);
			HandleVillagerGainTreasuryItem(context, itemInSameGroup, character);
			_villagerGainedItemTypeSet.Add(itemInSameGroup.ItemType);
		}
	}

	private void HandleVillagerPersonalNeedForGainResource(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, GameData.Domains.Character.Ai.PersonalNeed personalNeed, int needIndex)
	{
		if (IsMeetWorkStatus(character, settlement))
		{
			SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
			int num = (taiwuTreasury.Resources.CheckIsMeet(personalNeed.ResourceType, personalNeed.Amount) ? personalNeed.Amount : taiwuTreasury.Resources.Get(personalNeed.ResourceType));
			if (num > 0)
			{
				VillagerTakeResourceFromTreasury(context, character, personalNeed.ResourceType, num);
				character.ChangeResource(context, personalNeed.ResourceType, num);
				personalNeed.Amount -= num;
				List<GameData.Domains.Character.Ai.PersonalNeed> personalNeeds = character.GetPersonalNeeds();
				personalNeeds[needIndex] = personalNeed;
			}
		}
	}

	private bool HandleVillagerPersonalNeedForAddPoisonToItem(DataContext context, GameData.Domains.Character.Character character, Settlement settlement, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!IsMeetWorkStatus(character, settlement))
		{
			return false;
		}
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		short lifeSkillAttainment = character.GetLifeSkillAttainment(9);
		sbyte poisonType = personalNeed.PoisonType;
		ItemKey itemKey = ItemKey.Invalid;
		foreach (var (itemKey3, _) in taiwuTreasury.Inventory.Items)
		{
			if (itemKey3.ItemType == 8)
			{
				MedicineItem medicineItem = Config.Medicine.Instance[itemKey3.TemplateId];
				short num2 = GlobalConfig.Instance.PoisonAttainments[medicineItem.Grade];
				if (lifeSkillAttainment >= num2 && medicineItem.ApplyPoisonType == poisonType)
				{
					itemKey = itemKey3;
					break;
				}
			}
		}
		if (!itemKey.IsValid())
		{
			return false;
		}
		GameData.Domains.Character.Ai.PersonalNeed personalNeed2 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(10, itemKey.ItemType, itemKey.TemplateId);
		HandleVillagerPersonalNeedForGainItem(context, character, settlement, personalNeed2);
		return true;
	}

	private static void SortVillagerSelectableItemsByGrade(List<(ItemKey itemKey, int score)> items)
	{
		items.Sort(delegate((ItemKey itemKey, int score) a, (ItemKey itemKey, int score) b)
		{
			sbyte grade = ItemTemplateHelper.GetGrade(a.itemKey.ItemType, a.itemKey.TemplateId);
			sbyte grade2 = ItemTemplateHelper.GetGrade(b.itemKey.ItemType, b.itemKey.TemplateId);
			return grade.CompareTo(grade2);
		});
	}

	private static void SortVillagerSelectableItemsByGrade(List<ItemKey> items)
	{
		items.Sort(delegate(ItemKey a, ItemKey b)
		{
			sbyte grade = ItemTemplateHelper.GetGrade(a.ItemType, a.TemplateId);
			sbyte grade2 = ItemTemplateHelper.GetGrade(b.ItemType, b.TemplateId);
			return grade.CompareTo(grade2);
		});
	}

	private void SortVillagerSelectableBooks(List<(ItemKey itemKey, int score)> items, GameData.Domains.Character.Character character)
	{
		short villagerRoleTemplateId = DomainManager.Extra.GetVillagerRoleTemplateId(character.GetId());
		HashSet<sbyte> lifeSkillTypeSet = VillagerRoleNeedLifeSkillBooks[villagerRoleTemplateId];
		Equipping.SortBooksByScore(items, character);
		sbyte bookType = -1;
		VillagerWorkData villagerWorkData = DomainManager.Extra.GetVillagerRole(character.GetId())?.WorkData;
		if (villagerRoleTemplateId == 5)
		{
			if (villagerWorkData?.WorkType == 1 && DomainManager.Building.TryGetElement_BuildingBlocks(new BuildingBlockKey(villagerWorkData.AreaId, villagerWorkData.BlockId, villagerWorkData.BuildingBlockIndex), out var value))
			{
				bookType = value.ConfigData.RequireCombatSkillType;
			}
			items.Sort(((ItemKey itemKey, int score) a, (ItemKey itemKey, int score) b) => CompareCombatSkillBook(a.itemKey, b.itemKey, bookType));
		}
		else if (lifeSkillTypeSet != null && lifeSkillTypeSet.Count > 0)
		{
			if (villagerWorkData?.WorkType == 1 && DomainManager.Building.TryGetElement_BuildingBlocks(new BuildingBlockKey(villagerWorkData.AreaId, villagerWorkData.BlockId, villagerWorkData.BuildingBlockIndex), out var value2))
			{
				bookType = value2.ConfigData.RequireLifeSkillType;
			}
			items.Sort(((ItemKey itemKey, int score) a, (ItemKey itemKey, int score) b) => CompareLifeSkillBook(a.itemKey, b.itemKey, lifeSkillTypeSet, bookType));
		}
	}

	private static int CompareCombatSkillBook(ItemKey a, ItemKey b, sbyte bookType)
	{
		SkillBookItem skillBookItem = Config.SkillBook.Instance[a.TemplateId];
		SkillBookItem skillBookItem2 = Config.SkillBook.Instance[b.TemplateId];
		bool flag = skillBookItem.ItemSubType == 1001 && skillBookItem2.CombatSkillType != bookType;
		bool value = skillBookItem2.ItemSubType == 1001 && skillBookItem.CombatSkillType != bookType;
		return flag.CompareTo(value);
	}

	private static int CompareLifeSkillBook(ItemKey a, ItemKey b, HashSet<sbyte> lifeSkillTypeSet, sbyte bookType)
	{
		SkillBookItem skillBookItem = Config.SkillBook.Instance[a.TemplateId];
		SkillBookItem skillBookItem2 = Config.SkillBook.Instance[b.TemplateId];
		bool flag = lifeSkillTypeSet.Contains(skillBookItem.LifeSkillType) && skillBookItem2.LifeSkillType != bookType;
		bool value = lifeSkillTypeSet.Contains(skillBookItem2.LifeSkillType) && skillBookItem.LifeSkillType != bookType;
		return flag.CompareTo(value);
	}

	private void SortVillagerSelectableEquipments(List<(ItemKey itemKey, int score)> items, GameData.Domains.Character.Character character)
	{
		if (items == null || items.Count <= 0)
		{
			return;
		}
		short idealClothingTemplateId = character.GetIdealClothingTemplateId();
		items.Sort(delegate((ItemKey itemKey, int score) a, (ItemKey itemKey, int score) b)
		{
			switch (items.First().itemKey.ItemType)
			{
			case 0:
				_availableWeapons.Clear();
				_suitableWeapons.Clear();
				_availableWeapons.Add((a.itemKey, 0));
				_availableWeapons.Add((b.itemKey, 0));
				Equipping.GetWeaponScores(character, _availableWeapons, _suitableWeapons, _fixedBestWeapons);
				if (_availableWeapons[0].weapon.Equals(a.itemKey))
				{
					a.score = _availableWeapons[0].score;
					b.score = _availableWeapons[1].score;
				}
				else
				{
					a.score = _availableWeapons[1].score;
					b.score = _availableWeapons[0].score;
				}
				return a.score.CompareTo(b.score);
			case 3:
			{
				if (a.itemKey.TemplateId == idealClothingTemplateId)
				{
					return 1;
				}
				if (b.itemKey.TemplateId == idealClothingTemplateId)
				{
					return -1;
				}
				bool flag = Config.VillagerRole.Instance.Any((VillagerRoleItem r) => r.Clothing == a.itemKey.TemplateId);
				bool flag2 = Config.VillagerRole.Instance.Any((VillagerRoleItem r) => r.Clothing == b.itemKey.TemplateId);
				if (flag != flag2)
				{
					return flag2.CompareTo(flag);
				}
				(int, bool) tuple5 = Equipping.CalcEquipmentScore(a.itemKey, -1);
				(int, bool) tuple6 = Equipping.CalcEquipmentScore(b.itemKey, -1);
				(a.score, _) = tuple5;
				(b.score, _) = tuple6;
				return a.score.CompareTo(b.score);
			}
			default:
			{
				(int, bool) tuple = Equipping.CalcEquipmentScore(a.itemKey, -1);
				(int, bool) tuple2 = Equipping.CalcEquipmentScore(b.itemKey, -1);
				(a.score, _) = tuple;
				(b.score, _) = tuple2;
				return a.score.CompareTo(b.score);
			}
			}
		});
	}

	private bool CompareVillagerSelectableItems(ItemKey selfItemKey, ItemKey targetItemKey, GameData.Domains.Character.Character character)
	{
		List<(ItemKey, int)> list = ObjectPool<List<(ItemKey, int)>>.Instance.Get();
		list.Add((targetItemKey, 0));
		list.Add((selfItemKey, 0));
		if (ItemType.IsEquipmentItemType(selfItemKey.ItemType))
		{
			SortVillagerSelectableEquipments(list, character);
		}
		else if (selfItemKey.ItemType == 10)
		{
			SortVillagerSelectableBooks(list, character);
		}
		else
		{
			SortVillagerSelectableItemsByGrade(list);
		}
		int num = list.FindIndex(((ItemKey itemKey, int score) t) => t.itemKey.Equals(selfItemKey));
		int num2 = list.FindIndex(((ItemKey itemKey, int score) t) => t.itemKey.Equals(targetItemKey));
		bool result = num2 > num;
		ObjectPool<List<(ItemKey, int)>>.Instance.Return(list);
		return result;
	}

	private bool CompareVillagerSelectableBooks(ItemKey selfItemKey, ItemKey targetItemKey, GameData.Domains.Character.Character character)
	{
		List<(ItemKey, int)> list = ObjectPool<List<(ItemKey, int)>>.Instance.Get();
		list.Add((targetItemKey, 0));
		list.Add((selfItemKey, 0));
		SortVillagerSelectableBooks(list, character);
		int num = list.FindIndex(((ItemKey itemKey, int score) t) => t.itemKey.Equals(selfItemKey));
		int num2 = list.FindIndex(((ItemKey itemKey, int score) t) => t.itemKey.Equals(targetItemKey));
		bool result = num2 > num;
		ObjectPool<List<(ItemKey, int)>>.Instance.Return(list);
		return result;
	}

	private bool CompareVillagerSelectableEquipments(ItemKey selfItemKey, ItemKey targetItemKey, GameData.Domains.Character.Character character)
	{
		List<(ItemKey, int)> list = ObjectPool<List<(ItemKey, int)>>.Instance.Get();
		list.Add((targetItemKey, 0));
		list.Add((selfItemKey, 0));
		SortVillagerSelectableEquipments(list, character);
		int num = list.FindIndex(((ItemKey itemKey, int score) t) => t.itemKey.Equals(selfItemKey));
		int num2 = list.FindIndex(((ItemKey itemKey, int score) t) => t.itemKey.Equals(targetItemKey));
		bool result = num2 > num;
		ObjectPool<List<(ItemKey, int)>>.Instance.Return(list);
		return result;
	}

	private List<int> GetTaiwuVillagerMemberList()
	{
		List<int> list = new List<int>();
		Settlement settlement = DomainManager.Organization.GetSettlement(_taiwuVillageSettlementId);
		settlement.GetMembers().GetAllMembers(list);
		list.Remove(_taiwuCharId);
		list.Sort(delegate(int a, int b)
		{
			SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(a);
			SettlementCharacter settlementCharacter2 = DomainManager.Organization.GetSettlementCharacter(b);
			int num = settlementCharacter2.GetInfluencePower().CompareTo(settlementCharacter.GetInfluencePower());
			if (num == 0)
			{
				sbyte ageGroup = DomainManager.Character.GetElement_Objects(a).GetAgeGroup();
				return DomainManager.Character.GetElement_Objects(b).GetAgeGroup().CompareTo(ageGroup);
			}
			return num;
		});
		return list;
	}

	[DomainMethod]
	public List<int>[] GetVillagerListClassArray()
	{
		return _villagerListClassArray;
	}

	[DomainMethod]
	public Dictionary<int, sbyte> GetVillagerClassesDict()
	{
		return _villagerClassDict;
	}

	public void InitVillagerTreasuryTempData()
	{
		_villagerClassDict.Clear();
		List<int>[] villagerListClassArray = _villagerListClassArray;
		for (int i = 0; i < villagerListClassArray.Length; i++)
		{
			villagerListClassArray[i]?.Clear();
		}
		foreach (var (num2, b2) in DomainManager.Extra.VillagerLastInfluencePowerGrade)
		{
			if (DomainManager.Character.TryGetElement_Objects(num2, out var element) && element.GetOrganizationInfo().SettlementId == _taiwuVillageSettlementId)
			{
				_villagerClassDict[num2] = b2;
				int num3 = 8 - b2;
				List<int> list = _villagerListClassArray[num3];
				if (list == null)
				{
					list = new List<int>();
				}
				list.Add(num2);
				_villagerListClassArray[num3] = list;
			}
		}
		RefreshTreasuryNeededItem();
	}

	private void CalcVillagerClasses()
	{
		List<int> taiwuVillagerMemberList = GetTaiwuVillagerMemberList();
		int num = GlobalConfig.Instance.VillagerInfluencePowerRankingRatio.Length;
		Span<short> span = stackalloc short[num];
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			if (i == num - 1)
			{
				span[i] = (short)(taiwuVillagerMemberList.Count - num2);
				break;
			}
			float num3 = GlobalConfig.Instance.VillagerInfluencePowerRankingRatio[i];
			short num4 = (short)Math.Max(1.0, Math.Round(num3 * (float)taiwuVillagerMemberList.Count / 100f));
			span[i] = num4;
			num2 += num4;
		}
		int num5 = 0;
		int num6 = 0;
		_villagerClassDict.Clear();
		List<int>[] villagerListClassArray = _villagerListClassArray;
		for (int j = 0; j < villagerListClassArray.Length; j++)
		{
			villagerListClassArray[j]?.Clear();
		}
		foreach (int item in taiwuVillagerMemberList)
		{
			short num7 = span[num5];
			List<int>[] villagerListClassArray2 = _villagerListClassArray;
			int num8 = num5;
			if (villagerListClassArray2[num8] == null)
			{
				villagerListClassArray2[num8] = new List<int>(num7);
			}
			_villagerListClassArray[num5].Add(item);
			sbyte value = Convert.ToSByte(8 - num5);
			_villagerClassDict[item] = value;
			num6++;
			if (num6 >= num7)
			{
				num5++;
				num6 = 0;
			}
		}
	}

	private Dictionary<ItemKey, Dictionary<int, sbyte>> GetTreasuryNeededItemDict()
	{
		return _treasuryNeededItemDict;
	}

	[DomainMethod]
	public Dictionary<int, sbyte> GetTreasuryItemNeededCharDict(ItemKey itemKey)
	{
		return GetTreasuryNeededItemDict().GetValueOrDefault(itemKey);
	}

	[DomainMethod]
	public List<ItemKey> GetTreasuryNeededItemList()
	{
		return GetTreasuryNeededItemDict().Keys.ToList();
	}

	private void RefreshTreasuryNeededItem()
	{
		_treasuryNeededItemDict.Clear();
		List<int> list = new List<int>();
		Settlement settlement = DomainManager.Organization.GetSettlement(_taiwuVillageSettlementId);
		settlement.GetMembers().GetAllMembers(list);
		list.Remove(_taiwuCharId);
		foreach (int item in list)
		{
			if (!DomainManager.Extra.TryGetVillagerTreasuryNeed(item, out var villagerTreasuryNeed))
			{
				continue;
			}
			foreach (GameData.Domains.Character.Ai.PersonalNeed personalNeed in villagerTreasuryNeed.PersonalNeeds)
			{
				AddNeededItem(personalNeed, item);
			}
		}
	}

	[DomainMethod]
	public VillagerTreasuryNeed GetVillagerTreasuryNeed(int charId)
	{
		VillagerTreasuryNeed villagerTreasuryNeed;
		return DomainManager.Extra.TryGetVillagerTreasuryNeed(charId, out villagerTreasuryNeed) ? villagerTreasuryNeed : null;
	}

	public sbyte GetVillagerNeedWaitTime(int charId, CharacterSortFilter characterSortFilter)
	{
		if (characterSortFilter == null)
		{
			return 0;
		}
		if (characterSortFilter.Settings.FilterType != 4)
		{
			return 0;
		}
		Dictionary<int, sbyte> dictionary = _treasuryNeededItemDict[characterSortFilter.Settings.VillagerNeededItem];
		sbyte valueOrDefault = dictionary.GetValueOrDefault<int, sbyte>(charId, 0);
		return Math.Max(1, valueOrDefault);
	}

	private void InitializeGradeRoles()
	{
		_gradeVillagerRoles = new short[9];
		Array.Fill(_gradeVillagerRoles, (short)(-1));
		foreach (VillagerRoleItem item in (IEnumerable<VillagerRoleItem>)Config.VillagerRole.Instance)
		{
			OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[item.OrganizationMember];
			_gradeVillagerRoles[organizationMemberItem.Grade] = item.TemplateId;
		}
	}

	public short GetGradeVillagerRole(sbyte grade)
	{
		return _gradeVillagerRoles[grade];
	}

	[DomainMethod]
	public bool SetVillagerRole(DataContext context, int charId, short roleTemplateId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		VillagerRoleItem roleCfg = Config.VillagerRole.Instance.GetItem(roleTemplateId);
		GameData.Domains.Character.Character element;
		int villagerCount = ((roleCfg != null) ? _villagerFeatureHolderSet.Count((int cId) => DomainManager.Character.TryGetElement_Objects(cId, out element) && element.GetFeatureIds().Contains(roleCfg.FeatureId)) : 0);
		foreach (short item in CalcVillagerRoleFeatureTemplateIds())
		{
			element_Objects.RemoveFeature(context, item);
		}
		_villagerFeatureHolderSet.Remove(charId);
		ExtraDomain extra = DomainManager.Extra;
		VillagerRoleRecords villagerRoleRecordsData = extra.GetVillagerRoleRecordsData();
		short currentRoleId = DomainManager.Extra.GetVillagerRoleTemplateId(charId);
		bool flag = currentRoleId >= 0 && !villagerRoleRecordsData.History.Any((VillagerRoleRecordElement element) => element.CharacterId == charId && element.RoleTemplateId == currentRoleId);
		if (flag)
		{
			VillagerRoleRecordElement villagerRoleRecordElement = new VillagerRoleRecordElement();
			FillVillagerRoleRecordElement(element_Objects, villagerRoleRecordElement);
			villagerRoleRecordsData.History.Add(villagerRoleRecordElement);
		}
		if (roleCfg == null)
		{
			DomainManager.Character.ChangeFavorabilityOptional(context, element_Objects, DomainManager.Taiwu.GetTaiwu(), -VillagerRoleFormula.DefValue.BaseFavorCostLostRole.Calculate(element_Objects.GetOrganizationInfo().Grade), 3);
			DomainManager.Organization.ChangeGrade(context, element_Objects, 0, destPrincipal: true);
			if (flag)
			{
				extra.SetVillagerRoleRecordsData(context, villagerRoleRecordsData);
			}
			return true;
		}
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[roleCfg.OrganizationMember];
		VillagerRoleFormulaItem formula = ((organizationMemberItem.Grade > element_Objects.GetOrganizationInfo().Grade) ? VillagerRoleFormula.DefValue.BaseFavorAddGainRole : VillagerRoleFormula.DefValue.BaseFavorCostLostRole);
		int num = formula.Calculate(element_Objects.GetOrganizationInfo().Grade);
		DomainManager.Organization.ChangeGrade(context, element_Objects, organizationMemberItem.Grade, destPrincipal: true);
		DomainManager.Character.ChangeFavorabilityOptional(context, element_Objects, DomainManager.Taiwu.GetTaiwu(), formula.Calculate(organizationMemberItem.Grade) - num, 3);
		int num2 = GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalcSetVillagerRoleAuthorityCost(villagerCount, roleTemplateId);
		GetTaiwu().ChangeResource(context, 7, -num2);
		element_Objects.AddFeature(context, roleCfg.FeatureId, removeMutexFeature: true);
		_villagerFeatureHolderSet.Add(charId);
		villagerRoleRecordsData.History.RemoveAll((VillagerRoleRecordElement element) => element.CharacterId == charId && element.RoleTemplateId == roleTemplateId);
		VillagerRoleRecordElement villagerRoleRecordElement2 = new VillagerRoleRecordElement();
		FillVillagerRoleRecordElement(element_Objects, villagerRoleRecordElement2);
		villagerRoleRecordsData.History.Add(villagerRoleRecordElement2);
		extra.SetVillagerRoleRecordsData(context, villagerRoleRecordsData);
		return true;
	}

	public void OnTaiwuVillagerGradeChanged(DataContext context, GameData.Domains.Character.Character character, sbyte targetGrade)
	{
		Tester.Assert(character.GetOrganizationInfo().OrgTemplateId == 16);
		int id = character.GetId();
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(id);
		if (villagerRole != null)
		{
			DomainManager.Extra.UnregisterVillagerRole(context, id);
			VillagerWorkData value;
			if (villagerRole.ArrangementTemplateId >= 0)
			{
				RemoveVillagerWork(context, id);
			}
			else if (_villagerWork.TryGetValue(id, out value) && VillagerWorkType.IsVillagerRoleSpecificType(value.WorkType))
			{
				RemoveVillagerWork(context, id);
				SetVillagerIdleWork(context, id, value.AreaId, value.BlockId);
			}
		}
		short gradeVillagerRole = GetGradeVillagerRole(targetGrade);
		if (gradeVillagerRole >= 0)
		{
			DomainManager.Extra.RegisterVillagerRole(context, id, gradeVillagerRole);
		}
		DomainManager.Merchant.RemoveMerchantData(context, id);
		if (gradeVillagerRole == 3)
		{
			SetMerchantType(context, id, 7);
		}
	}

	public IReadOnlySet<int> GetVillagerRoleSet(short roleTemplateId)
	{
		VillagerRoleItem villagerRoleItem = Config.VillagerRole.Instance[roleTemplateId];
		OrgMemberCollection members = DomainManager.Organization.GetSettlement(_taiwuVillageSettlementId).GetMembers();
		sbyte grade = OrganizationMember.Instance[villagerRoleItem.OrganizationMember].Grade;
		return members.GetMembers(grade);
	}

	public void UpdateVillagerFixedActions(DataContext context)
	{
		OrgMemberCollection members = TaiwuVillage.GetMembers();
		foreach (VillagerRoleItem item in (IEnumerable<VillagerRoleItem>)Config.VillagerRole.Instance)
		{
			OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[item.OrganizationMember];
			HashSet<int> members2 = members.GetMembers(organizationMemberItem.Grade);
			foreach (int item2 in members2)
			{
				VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(item2);
				if (villagerRole.Character.IsInteractableAsIntelligentCharacter() && villagerRole.Character.GetLocation().IsValid() && !villagerRole.Character.IsActiveAdvanceMonthStatus(4) && villagerRole.Character.GetLeaderId() != _taiwuCharId)
				{
					DomainManager.Taiwu.UpdateVillagerRoleFixedActionSuccessArray(context, isPreAdvance: false);
					villagerRole.ExecuteFixedAction(context);
				}
			}
		}
		TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
		DomainManager.Extra.SetTaiwuVillageStoragesRecordCollection(context, taiwuVillageStoragesRecordCollection);
	}

	public void UpdateSwordTombKeepersOnXiangshuAvatarMoved(DataContext context, GameData.Domains.Character.Character character, Location location)
	{
		if (character.GetXiangshuType() != 1 || !location.IsValid())
		{
			return;
		}
		IReadOnlySet<int> villagerRoleSet = GetVillagerRoleSet(5);
		if (villagerRoleSet.Count == 0)
		{
			return;
		}
		List<GameData.Domains.Character.Character> list = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		list.Clear();
		MapBlockData rootBlock = DomainManager.Map.GetBlock(location).GetRootBlock();
		foreach (int item in villagerRoleSet)
		{
			VillagerRoleSwordTombKeeper villagerRoleSwordTombKeeper = (VillagerRoleSwordTombKeeper)DomainManager.Extra.GetVillagerRole(item);
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (villagerRoleSwordTombKeeper.ArrangementTemplateId != 3 || !context.Random.CheckPercentProb(villagerRoleSwordTombKeeper.InjuredByXiangshuAvatarChance) || element_Objects.ReachFallenInCombat(CombatType.Die))
			{
				continue;
			}
			Location location2 = element_Objects.GetLocation();
			if (location2.IsValid())
			{
				MapBlockData rootBlock2 = DomainManager.Map.GetBlock(location2).GetRootBlock();
				if (rootBlock == rootBlock2)
				{
					list.Add(element_Objects);
				}
			}
		}
		if (list.Count != 0)
		{
			GameData.Domains.Character.Character random = list.GetRandom(context.Random);
			int injuryByXiangshuAvatarAmount = VillagerRoleSwordTombKeeper.InjuryByXiangshuAvatarAmount;
			injuryByXiangshuAvatarAmount /= list.Count;
			random.TakeRandomDamage(context, injuryByXiangshuAvatarAmount);
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			if (injuryByXiangshuAvatarAmount > 0)
			{
				lifeRecordCollection.AddGuardingSwordTomb(random.GetId(), currDate, character.GetTemplateId());
			}
			else
			{
				lifeRecordCollection.AddGuardingSwordTombSucceed(random.GetId(), currDate, character.GetTemplateId());
			}
			ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list);
		}
	}

	public void UpdateVillagerRoleNewClothing(DataContext context)
	{
		SettlementTreasury taiwuTreasury = GetTaiwuTreasury();
		bool flag = false;
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		foreach (VillagerRoleItem roleCfg in (IEnumerable<VillagerRoleItem>)Config.VillagerRole.Instance)
		{
			IReadOnlySet<int> villagerRoleSet = GetVillagerRoleSet(roleCfg.TemplateId);
			if (villagerRoleSet.Count > 0 && !DomainManager.Extra.HasOwnedClothing(roleCfg.Clothing) && !villagerRoleSet.Any((int charId) => DomainManager.Character.GetElement_Objects(charId).GetEquipment()[4].TemplateId == roleCfg.Clothing))
			{
				OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[roleCfg.OrganizationMember];
				monthlyEventCollection.AddTaiWuVillagerClothing(16, organizationMemberItem.Grade, orgPrincipal: true, 1, 3, roleCfg.Clothing);
				flag = true;
				ItemKey itemKey = DomainManager.Item.CreateClothing(context, roleCfg.Clothing, -1);
				taiwuTreasury.Inventory.OfflineAdd(itemKey, 1);
				DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Treasury, _taiwuVillageSettlementId);
				Events.RaiseTaiwuItemModified(context, itemKey);
			}
		}
		if (flag)
		{
			SetTaiwuTreasury(context, taiwuTreasury);
		}
	}

	public static int CalcVillagerRoleMaxCount(int progress)
	{
		int num = 0;
		while (true)
		{
			num++;
			progress -= num * 100;
			int num2 = progress;
			int num3 = num2;
			if (num3 >= 0)
			{
				if (num3 == 0)
				{
					break;
				}
				bool flag = true;
				continue;
			}
			return num - 1;
		}
		return num;
	}

	public static int CalcUsedCapacityBySeatCount(int seatCount)
	{
		return 50 * seatCount * (seatCount + 1);
	}

	[DomainMethod]
	public VillagerRoleManageDisplayData GetVillagerRoleDisplayData(short roleTemplateId)
	{
		VillagerRoleItem villagerRoleItem = Config.VillagerRole.Instance[roleTemplateId];
		OrgMemberCollection members = TaiwuVillage.GetMembers();
		sbyte grade = OrganizationMember.Instance[villagerRoleItem.OrganizationMember].Grade;
		return new VillagerRoleManageDisplayData
		{
			RoleTemplateId = roleTemplateId,
			CharacterIds = new List<int>(members.GetMembers(grade))
		};
	}

	[DomainMethod]
	public List<VillagerRoleManageDisplayData> GetAllVillagerRoleDisplayData()
	{
		List<VillagerRoleManageDisplayData> list = new List<VillagerRoleManageDisplayData>();
		foreach (VillagerRoleItem item in (IEnumerable<VillagerRoleItem>)Config.VillagerRole.Instance)
		{
			short templateId = item.TemplateId;
			VillagerRoleManageDisplayData villagerRoleDisplayData = GetVillagerRoleDisplayData(templateId);
			list.Add(villagerRoleDisplayData);
		}
		return list;
	}

	[DomainMethod]
	public VillagerRoleCharacterDisplayData GetVillagerRoleCharacterDisplayData(int characterId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(characterId, out var element))
		{
			return null;
		}
		sbyte readBookMaxGrade = 0;
		bool matchVillagerRole = false;
		sbyte leftPotentialCount = GlobalConfig.Instance.TaiwuVillagerMaxPotential;
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(characterId);
		if (_villagerWork.TryGetValue(characterId, out var value) && value.WorkType == 1)
		{
			BuildingBlockData buildingBlockData = DomainManager.Building.GetBuildingBlockData(new BuildingBlockKey(value.AreaId, value.BlockId, value.BuildingBlockIndex));
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingBlockData.TemplateId];
			readBookMaxGrade = ((buildingBlockItem.RequireLifeSkillType >= 0) ? element.GetLearnedLifeSkillMaxGradeByType(buildingBlockItem.RequireLifeSkillType) : element.GetLearnedCombatSkillMaxGradeByType(buildingBlockItem.RequireCombatSkillType));
			if (villagerRole != null)
			{
				matchVillagerRole = buildingBlockItem.VillagerRoleTemplateIds.Contains(villagerRole.RoleTemplateId);
			}
			leftPotentialCount = GetTaiwuVillagerLeftPotentialCount(characterId);
		}
		if (!DomainManager.Taiwu.TryGetElement_VillagerWork(characterId, out var value2))
		{
			value2 = new VillagerWorkData();
		}
		VillagerRoleCharacterDisplayData villagerRoleCharacterDisplayData = new VillagerRoleCharacterDisplayData
		{
			Avatar = element.GenerateAvatarRelatedData(),
			CombatSkillAttainments = element.GetCombatSkillAttainments(),
			LifeSkillAttainments = element.GetLifeSkillAttainments(),
			CombatSkillQualifications = element.GetCombatSkillQualifications(),
			LifeSkillQualifications = element.GetLifeSkillQualifications(),
			Id = characterId,
			Personalities = element.GetPersonalities(),
			ArrangementDisplayData = new VillagerRoleArrangementDisplayDataWrapper(),
			VillagerWorkData = value2,
			AliveState = ((!DomainManager.Character.IsCharacterAlive(characterId)) ? ((sbyte)1) : ((sbyte)0)),
			ReadBookMaxGrade = readBookMaxGrade,
			MatchVillagerRole = matchVillagerRole,
			LeftPotentialCount = leftPotentialCount,
			Age = element.GetPhysiologicalAge()
		};
		CharacterDomain.GetNameRelatedData(element, ref villagerRoleCharacterDisplayData.Name);
		villagerRoleCharacterDisplayData.RoleTemplateId = DomainManager.Extra.GetVillagerRoleTemplateId(characterId);
		if (villagerRole != null)
		{
			villagerRoleCharacterDisplayData.ArrangementDisplayData.ArrangementTemplateId = villagerRole.ArrangementTemplateId;
			if (villagerRole.WorkData != null)
			{
				villagerRoleCharacterDisplayData.ArrangementDisplayData.AreaId = villagerRole.WorkData.Location.AreaId;
				if (villagerRole.WorkData.WorkerIndex == 0)
				{
					villagerRoleCharacterDisplayData.Flags |= 4;
				}
			}
			villagerRoleCharacterDisplayData.ArrangementDisplayData.ArrangementDataId = CalcVillagerRoleDefaultArrangementId(villagerRole.RoleTemplateId);
			villagerRoleCharacterDisplayData.ArrangementDisplayData.ArrangementData = villagerRole.GetArrangementDisplayData();
			if (villagerRole is VillagerRoleMerchant villagerRoleMerchant)
			{
				villagerRoleCharacterDisplayData.ItemTemplateKey = villagerRoleMerchant.ItemTemplateKey;
			}
		}
		villagerRoleCharacterDisplayData.CollectResourceAmount = ((value2 != null && value2.WorkType == 10) ? value2.GetCollectResourceIncome() : 0);
		return villagerRoleCharacterDisplayData;
	}

	public sbyte GetTaiwuVillagerLeftPotentialCount(int charId)
	{
		sbyte taiwuVillagerMaxPotential = GlobalConfig.Instance.TaiwuVillagerMaxPotential;
		DomainManager.Extra.TryGetElement_TaiwuVillagerPotentialData(charId, out var value);
		taiwuVillagerMaxPotential -= value;
		return (sbyte)MathF.Max(0f, taiwuVillagerMaxPotential);
	}

	[DomainMethod]
	public VillagerRoleCharacterSlimDisplayData GetVillagerRoleCharacterSlimDisplayData(int characterId)
	{
		VillagerRoleCharacterSlimDisplayData villagerRoleCharacterSlimDisplayData = new VillagerRoleCharacterSlimDisplayData
		{
			Id = characterId,
			RoleTemplateId = DomainManager.Extra.GetVillagerRoleTemplateId(characterId),
			ArrangementTemplateId = -1
		};
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(characterId);
		if (villagerRole != null)
		{
			villagerRoleCharacterSlimDisplayData.ArrangementTemplateId = (short)villagerRole.ArrangementTemplateId;
		}
		return villagerRoleCharacterSlimDisplayData;
	}

	[DomainMethod]
	public (int successRateBonus, int chickenSuccess, int MigrateSpeedBonusFactor) GetVillagerFarmerMigrateResourceSuccessRateBonus(int characterId)
	{
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(characterId);
		if (villagerRole is VillagerRoleFarmer villagerRoleFarmer && _villagerWork.TryGetValue(characterId, out var value) && value.WorkType == 14)
		{
			int migrateResourceSuccessRateBonus = villagerRoleFarmer.MigrateResourceSuccessRateBonus;
			int item = (villagerRoleFarmer.HasChickenUpgradeEffect ? villagerRoleFarmer.UpgradeBuildingCoreRate : (-1));
			short settlementId = DomainManager.Character.GetElement_Objects(characterId).GetOrganizationInfo().SettlementId;
			int buildingBlockEffect = DomainManager.Building.GetBuildingBlockEffect(settlementId, EBuildingScaleEffect.MigrateSpeedBonusFactor);
			return (successRateBonus: migrateResourceSuccessRateBonus, chickenSuccess: item, MigrateSpeedBonusFactor: buildingBlockEffect);
		}
		return (successRateBonus: -1, chickenSuccess: -1, MigrateSpeedBonusFactor: -1);
	}

	internal IEnumerable<short> CalcVillagerRoleFeatureTemplateIds()
	{
		foreach (VillagerRoleItem role in (IEnumerable<VillagerRoleItem>)Config.VillagerRole.Instance)
		{
			if (role.FeatureId >= 0)
			{
				yield return role.FeatureId;
			}
		}
	}

	internal void ReCalcVillagerFeatureHolderSet()
	{
		HashSet<short> featureSet = new HashSet<short>(CalcVillagerRoleFeatureTemplateIds());
		List<GameData.Domains.Character.Character> list = new List<GameData.Domains.Character.Character>();
		_villagerFeatureHolderSet.Clear();
		DomainManager.Character.FindIntelligentCharacters((GameData.Domains.Character.Character ch) => featureSet.Overlaps(ch.GetFeatureIds()), list);
		_villagerFeatureHolderSet.UnionWith(list.Select((GameData.Domains.Character.Character ch) => ch.GetId()));
	}

	internal void FillVillagerRoleRecordElement(GameData.Domains.Character.Character character, VillagerRoleRecordElement record)
	{
		record.CharacterId = character.GetId();
		record.CharacterTemplateId = character.GetTemplateId();
		record.Avatar = character.GenerateAvatarRelatedData();
		record.CombatSkillAttainments = character.GetCombatSkillAttainments();
		record.LifeSkillAttainments = character.GetLifeSkillAttainments();
		record.Personalities = character.GetPersonalities();
		record.RoleTemplateId = DomainManager.Extra.GetVillagerRoleTemplateId(character.GetId());
		record.Date = DomainManager.World.GetCurrDate();
		CharacterDomain.GetNameRelatedData(character, ref record.Name);
	}

	[DomainMethod]
	public List<VillagerRoleCharacterDisplayData> GetVillagerRoleCharacterDisplayDataOnPanel(DataContext ctx)
	{
		CharacterDomain character = DomainManager.Character;
		ExtraDomain extra = DomainManager.Extra;
		VillagerRoleRecords villagerRoleRecordsData = extra.GetVillagerRoleRecordsData();
		List<VillagerRoleCharacterDisplayData> list = new List<VillagerRoleCharacterDisplayData>();
		bool flag = false;
		HashSet<int> hashSet = new HashSet<int>(extra.GetVillagerRoleCharacters());
		HashSet<int> hashSet2 = new HashSet<int>(_villagerFeatureHolderSet);
		hashSet2.ExceptWith(hashSet);
		foreach (VillagerRoleRecordElement item in villagerRoleRecordsData.History)
		{
			int characterId = item.CharacterId;
			if (hashSet.Contains(characterId) && extra.GetVillagerRoleTemplateId(characterId) == item.RoleTemplateId)
			{
				continue;
			}
			if (character.TryGetElement_Objects(characterId, out var element))
			{
				short roleTemplateId = item.RoleTemplateId;
				int date = item.Date;
				FillVillagerRoleRecordElement(element, item);
				item.RoleTemplateId = roleTemplateId;
				item.Date = date;
				flag = true;
			}
			else
			{
				hashSet2.Remove(characterId);
			}
			VillagerRoleCharacterDisplayData villagerRoleCharacterDisplayData = new VillagerRoleCharacterDisplayData
			{
				Avatar = item.Avatar,
				CombatSkillAttainments = item.CombatSkillAttainments,
				LifeSkillAttainments = item.LifeSkillAttainments,
				Id = characterId,
				Personalities = item.Personalities,
				Name = item.Name,
				ArrangementDisplayData = new VillagerRoleArrangementDisplayDataWrapper(),
				VillagerWorkData = new VillagerWorkData(),
				AliveState = ((!DomainManager.Character.IsCharacterAlive(characterId)) ? ((sbyte)1) : ((sbyte)0))
			};
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(characterId);
			villagerRoleCharacterDisplayData.RoleTemplateId = item.RoleTemplateId;
			if (villagerRole != null)
			{
				villagerRoleCharacterDisplayData.ArrangementDisplayData.ArrangementTemplateId = villagerRole.ArrangementTemplateId;
				if (villagerRole.WorkData != null)
				{
					villagerRoleCharacterDisplayData.ArrangementDisplayData.AreaId = villagerRole.WorkData.Location.AreaId;
				}
				villagerRoleCharacterDisplayData.ArrangementDisplayData.ArrangementDataId = CalcVillagerRoleDefaultArrangementId(villagerRole.RoleTemplateId);
				villagerRoleCharacterDisplayData.ArrangementDisplayData.ArrangementData = villagerRole.GetArrangementDisplayData();
			}
			if (hashSet2.Contains(characterId))
			{
				villagerRoleCharacterDisplayData.Flags |= 2;
			}
			list.Add(villagerRoleCharacterDisplayData);
		}
		foreach (int item2 in hashSet)
		{
			VillagerRoleCharacterDisplayData villagerRoleCharacterDisplayData2 = GetVillagerRoleCharacterDisplayData(item2);
			if (villagerRoleCharacterDisplayData2 != null)
			{
				villagerRoleCharacterDisplayData2.Flags |= 1;
				list.Add(villagerRoleCharacterDisplayData2);
			}
		}
		if (flag)
		{
			extra.SetVillagerRoleRecordsData(ctx, villagerRoleRecordsData);
		}
		return list;
	}

	[DomainMethod]
	public List<VillagerRoleCharacterDisplayData> GetVillagerRoleCharacterDisplayDataList(List<int> characterIdList)
	{
		if (characterIdList == null)
		{
			return null;
		}
		List<VillagerRoleCharacterDisplayData> list = new List<VillagerRoleCharacterDisplayData>();
		foreach (int characterId in characterIdList)
		{
			list.Add(GetVillagerRoleCharacterDisplayData(characterId));
		}
		return list;
	}

	[DomainMethod]
	public bool BatchSetVillagerRole(DataContext context, List<int> characterIdList, short roleTemplateId)
	{
		IReadOnlySet<int> villagerRoleSet = GetVillagerRoleSet(roleTemplateId);
		HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
		hashSet.Clear();
		hashSet.UnionWith(villagerRoleSet);
		foreach (int item in hashSet)
		{
			if (characterIdList == null || !characterIdList.Contains(item))
			{
				SetVillagerRole(context, item, -1);
			}
		}
		if (characterIdList == null)
		{
			return true;
		}
		foreach (int characterId in characterIdList)
		{
			if (!hashSet.Contains(characterId))
			{
				SetVillagerRole(context, characterId, roleTemplateId);
			}
		}
		return true;
	}

	[DomainMethod]
	public bool DispatchVillagerArrangement(DataContext context, int characterId, short arrangementTemplateId, Location location)
	{
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(characterId);
		if (villagerRole == null)
		{
			Logger.Warn($"character {characterId} is not assigned as any villager role.");
			return false;
		}
		RemoveVillagerWork(context, characterId);
		if (arrangementTemplateId < 0)
		{
			return true;
		}
		villagerRole.OfflineSetArrangement(arrangementTemplateId, location);
		DomainManager.Extra.SetVillagerRole(context, characterId);
		SetVillagerWork(context, characterId, villagerRole.WorkData);
		return true;
	}

	[DomainMethod]
	public bool RecallVillager(DataContext context, int characterId)
	{
		RemoveVillagerWork(context, characterId);
		return true;
	}

	[DomainMethod]
	public bool AssignTargetItem(DataContext context, int characterId, TemplateKey targetItem)
	{
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(characterId);
		if (!(villagerRole is VillagerRoleMerchant villagerRoleMerchant))
		{
			Logger.Warn($"character {characterId} is not assigned as a merchant.");
			return false;
		}
		villagerRoleMerchant.ItemTemplateKey = targetItem;
		villagerRoleMerchant.BoughtInAmount = 0;
		DomainManager.Extra.SetVillagerRole(context, characterId);
		return true;
	}

	[DomainMethod]
	public void SetMerchantType(DataContext context, int characterId, sbyte merchantType, bool immediate = false)
	{
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(characterId);
		if (!(villagerRole is VillagerRoleMerchant villagerRoleMerchant))
		{
			Logger.Warn($"character {characterId} is not assigned as a merchant.");
			return;
		}
		villagerRoleMerchant.DesignatedMerchantType = merchantType;
		if (!immediate)
		{
			if (merchantType == 7)
			{
				villagerRoleMerchant.SelfDecideMerchantType = (sbyte)context.Random.Next(7);
			}
			if (villagerRoleMerchant.CurrentMerchantType == 7)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(characterId);
				villagerRoleMerchant.CurrentMerchantType = (sbyte)context.Random.Next(7);
				element_Objects.ChangeMerchantType(context, element_Objects.GetOrganizationInfo());
			}
		}
		else
		{
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(characterId);
			villagerRoleMerchant.CurrentMerchantType = ((merchantType == 7) ? villagerRoleMerchant.SelfDecideMerchantType : merchantType);
			element_Objects2.ChangeMerchantType(context, element_Objects2.GetOrganizationInfo());
		}
		DomainManager.Extra.SetVillagerRole(context, characterId);
	}

	[DomainMethod]
	public sbyte GetMerchantType(int characterId)
	{
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(characterId);
		return (sbyte)((!(villagerRole is VillagerRoleMerchant villagerRoleMerchant)) ? 7 : villagerRoleMerchant.DesignatedMerchantType);
	}

	[DomainMethod]
	public bool AssignArrangementIncreaseOrDecrease(DataContext context, int characterId, bool isIncrease)
	{
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(characterId);
		VillagerRoleBase villagerRoleBase = villagerRole;
		VillagerRoleBase villagerRoleBase2 = villagerRoleBase;
		if (!(villagerRoleBase2 is VillagerRoleLiterati villagerRoleLiterati))
		{
			if (!(villagerRoleBase2 is VillagerRoleSwordTombKeeper villagerRoleSwordTombKeeper))
			{
				Logger.Warn($"character {characterId} is neither assigned as a literati nor a sword tomb keeper.");
				return false;
			}
			villagerRoleSwordTombKeeper.PositiveAction = isIncrease;
		}
		else
		{
			villagerRoleLiterati.PositiveAction = isIncrease;
		}
		DomainManager.Extra.SetVillagerRole(context, characterId);
		return true;
	}

	[DomainMethod]
	public VillagerRoleTipsDisplayData GetVillagerRoleTipsDisplayData(DataContext context, short roleTemplateId)
	{
		VillagerRoleTipsDisplayData villagerRoleTipsDisplayData = new VillagerRoleTipsDisplayData
		{
			RoleTemplateId = roleTemplateId,
			RelatedBuildingClassList = new List<int>(),
			DetailList = new List<IntPair>()
		};
		GetVillagerRoleCapacityRelatedBuildingClass(roleTemplateId, ref villagerRoleTipsDisplayData.RelatedBuildingClassList);
		DomainManager.Building.GetVillagerRoleCapacitiesDetail(roleTemplateId, ref villagerRoleTipsDisplayData.DetailList);
		return villagerRoleTipsDisplayData;
	}

	private void GetVillagerRoleCapacityRelatedBuildingClass(short roleTemplateId, ref List<int> relatedBuildingClassList)
	{
		foreach (BuildingScaleItem item in (IEnumerable<BuildingScaleItem>)BuildingScale.Instance)
		{
			foreach (BuildingBlockItem item2 in (IEnumerable<BuildingBlockItem>)BuildingBlock.Instance)
			{
				if (item2.VillagerRoleTemplateIds != null && item2.VillagerRoleTemplateIds.Exist(roleTemplateId) && item2.ExpandInfos != null && item2.ExpandInfos.Contains(item.TemplateId) && !relatedBuildingClassList.Contains((int)item2.Class))
				{
					relatedBuildingClassList.Add((int)item2.Class);
				}
			}
		}
	}

	[DomainMethod]
	public int SetVillagerRoleNickName(DataContext context, short roleTemplateId, string nickName)
	{
		return DomainManager.Extra.SetVillagerRoleNickName(context, roleTemplateId, nickName);
	}

	[DomainMethod]
	public string GetVillagerRoleNpcNickName(short roleTemplateId)
	{
		if (DomainManager.Extra.TryGetElement_VillagerRoleNickNameMap(roleTemplateId, out var value))
		{
			return DomainManager.World.GetElement_CustomTexts(value);
		}
		return null;
	}

	[DomainMethod]
	public List<DispatchSwordTombDisplayData> GetAllSwordTombDisplayDataForDispatch()
	{
		List<DispatchSwordTombDisplayData> list = new List<DispatchSwordTombDisplayData>();
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(EventArgBox.TaiwuVillageAreaId);
		sbyte[] xiangshuAvatarTasksInOrder = DomainManager.World.GetXiangshuAvatarTasksInOrder();
		for (int i = 0; i < xiangshuAvatarTasksInOrder.Length - 1; i++)
		{
			sbyte b = xiangshuAvatarTasksInOrder[i];
			DispatchSwordTombDisplayData dispatchSwordTombDisplayData = new DispatchSwordTombDisplayData
			{
				Id = b,
				KeeperCount = 0
			};
			WorldStateData worldStateData = DomainManager.World.GetWorldStateData();
			sbyte escapeState = 0;
			if (worldStateData.IsXiangshuAvatarAwakening(b))
			{
				escapeState = 1;
			}
			if (worldStateData.IsXiangshuAvatarAttacking(b))
			{
				escapeState = 2;
			}
			dispatchSwordTombDisplayData.EscapeState = escapeState;
			dispatchSwordTombDisplayData.Location = DomainManager.Map.GetElement_SwordTombLocations(i);
			dispatchSwordTombDisplayData.BlockData = DomainManager.Map.GetBlock(dispatchSwordTombDisplayData.Location);
			if (dispatchSwordTombDisplayData.BlockData.RootBlockId >= 0)
			{
				dispatchSwordTombDisplayData.RootBlockData = DomainManager.Map.GetBlockData(dispatchSwordTombDisplayData.Location.AreaId, dispatchSwordTombDisplayData.BlockData.RootBlockId);
			}
			VillagerRoleManageDisplayData villagerRoleDisplayData = GetVillagerRoleDisplayData(5);
			foreach (int characterId in villagerRoleDisplayData.CharacterIds)
			{
				if (DomainManager.Extra.GetVillagerRole(characterId) is VillagerRoleSwordTombKeeper { ArrangementTemplateId: 3 } villagerRoleSwordTombKeeper && villagerRoleSwordTombKeeper.XiangshuAvatarId == b)
				{
					dispatchSwordTombDisplayData.KeeperCount++;
				}
			}
			bool flag = false;
			foreach (KeyValuePair<short, AdventureSiteData> adventureSite in adventuresInArea.AdventureSites)
			{
				sbyte xiangshuAvatarIdBySwordTomb = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureSite.Value.TemplateId);
				if (xiangshuAvatarIdBySwordTomb == b)
				{
					dispatchSwordTombDisplayData.RemainingMonths = adventureSite.Value.RemainingMonths;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				dispatchSwordTombDisplayData.EscapeState = 3;
			}
			list.Add(dispatchSwordTombDisplayData);
		}
		return list;
	}

	internal void UpdateVillagerRoleRecords(DataContext ctx)
	{
		ExtraDomain extra = DomainManager.Extra;
		VillagerRoleRecords villagerRoleRecordsData = extra.GetVillagerRoleRecordsData();
		int currDate = DomainManager.World.GetCurrDate();
		if (villagerRoleRecordsData.History.RemoveAll((VillagerRoleRecordElement element) => currDate - element.Date > 36) > 0)
		{
			extra.SetVillagerRoleRecordsData(ctx, villagerRoleRecordsData);
		}
	}

	[DomainMethod]
	public int GetVillagerRoleHeadTotalAuthorityCost()
	{
		int num = 0;
		IReadOnlySet<int> villagerRoleSet = DomainManager.Taiwu.GetVillagerRoleSet(6);
		foreach (int item in villagerRoleSet)
		{
			if (DomainManager.Extra.GetVillagerRole(item) is VillagerRoleHead villagerRoleHead)
			{
				num -= villagerRoleHead.GetAuthorityCost(removeExceeded: false, out var _);
			}
		}
		return num;
	}

	private void VillagerWorkCollectResource(DataContext context, VillagerWorkData workData)
	{
		if (workData.ResourceType >= 0)
		{
			if (workData.GetVillagerRole() is VillagerRoleFarmer villagerRoleFarmer)
			{
				Location key = new Location(workData.AreaId, workData.BlockId);
				MapBlockData block = DomainManager.Map.GetBlock(key);
				villagerRoleFarmer.CollectResource(context, block, workData.ResourceType);
			}
			else
			{
				Logger.AppendWarning($"Non-farmer character {workData.CharacterId} trying to collect resource for taiwu village.");
			}
		}
	}

	private void VillagerWorkMigrateResource(DataContext context, VillagerWorkData workData)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		Location location = workData.Location;
		short settlementId = DomainManager.Character.GetElement_Objects(workData.CharacterId).GetOrganizationInfo().SettlementId;
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (workData.ResourceType < 0 || !(workData.GetVillagerRole() is VillagerRoleFarmer villagerRoleFarmer))
		{
			return;
		}
		villagerRoleFarmer.RefreshFailureAccumulation(workData.ResourceType);
		int migrateResourceBaseSuccessRate = villagerRoleFarmer.MigrateResourceBaseSuccessRate;
		int buildingBlockEffect = DomainManager.Building.GetBuildingBlockEffect(settlementId, EBuildingScaleEffect.MigrateSpeedBonusFactor);
		migrateResourceBaseSuccessRate *= CValuePercentBonus.op_Implicit(buildingBlockEffect);
		migrateResourceBaseSuccessRate += villagerRoleFarmer.MigrateResourceSuccessRateBonus;
		if (context.Random.CheckPercentProb(migrateResourceBaseSuccessRate))
		{
			ResourceTypeItem resourceTypeItem = Config.ResourceType.Instance[workData.ResourceType];
			List<(short, short)> obj = context.AdvanceMonthRelatedData.WeightTable.Occupy();
			short[] array = resourceTypeItem.PossibleBuildingCoreItem;
			if (villagerRoleFarmer.HasChickenUpgradeEffect && context.Random.CheckPercentProb(villagerRoleFarmer.UpgradeBuildingCoreRate))
			{
				short[] possibleUpgradedBuildingCoreItem = resourceTypeItem.PossibleUpgradedBuildingCoreItem;
				if (possibleUpgradedBuildingCoreItem != null && possibleUpgradedBuildingCoreItem.Length > 0)
				{
					array = resourceTypeItem.PossibleUpgradedBuildingCoreItem;
				}
			}
			short[] array2 = array;
			foreach (short item in array2)
			{
				obj.Add((item, 0));
			}
			foreach (BuildingBlockItem item2 in (IEnumerable<BuildingBlockItem>)BuildingBlock.Instance)
			{
				if (item2.BuildingCoreItem < 0)
				{
					continue;
				}
				int num = resourceTypeItem.PossibleBuildingCoreItem.IndexOf(item2.BuildingCoreItem);
				if (num < 0)
				{
					continue;
				}
				sbyte[] collectResourcePercent = item2.CollectResourcePercent;
				if (collectResourcePercent != null && collectResourcePercent.Length > 0)
				{
					short num2 = (short)(obj[num].Item2 + item2.CollectResourcePercent[workData.ResourceType]);
					if (workData.ResourceType == 5)
					{
						num2 += item2.CollectResourcePercent[workData.ResourceType + 1];
					}
					obj[num] = (item2.BuildingCoreItem, num2);
				}
			}
			short randomResult = RandomUtils.GetRandomResult(obj, context.Random);
			ItemKey itemKey = DomainManager.Item.CreateMisc(context, randomResult);
			DomainManager.Taiwu.WarehouseAdd(context, itemKey, 1);
			context.AdvanceMonthRelatedData.WeightTable.Release(ref obj);
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
			lifeRecordCollection.AddVillagerMigrateResources(workData.CharacterId, currDate, location, itemKey.ItemType, itemKey.TemplateId);
			monthlyNotificationCollection.AddResourceMigration(workData.CharacterId, location, itemKey.ItemType, itemKey.TemplateId);
			taiwuVillageStoragesRecordCollection.AddMigrateResources(currDate, TaiwuVillageStorageType.Warehouse, workData.CharacterId, itemKey.ItemType, itemKey.TemplateId);
			RemoveVillagerWork(context, workData.CharacterId);
			if (block.RootBlockId >= 0 || block.GroupBlockList != null)
			{
				return;
			}
			block.Destroyed = true;
			block.DestroyItemsDirect(context.AdvanceMonthRelatedData.WorldItemsToBeRemoved);
			block.CurrResources.Initialize();
			DomainManager.Map.SetBlockData(context, block);
			DomainManager.Extra.AddMapBlockRecoveryLock(context, location, 120);
		}
		else
		{
			villagerRoleFarmer.AccumulateMigrateFailure();
		}
		DomainManager.Extra.SetVillagerRole(context, villagerRoleFarmer.Character.GetId());
	}

	public TaiwuDomain()
		: base(77)
	{
		_taiwuCharId = 0;
		_taiwuGenerationsCount = 0;
		_cricketLuckPoint = 0;
		_previousTaiwuIds = new List<int>();
		_needToEscape = false;
		_receivedItems = new List<ItemDisplayData>();
		_receivedCharacters = new List<CharacterDisplayData>();
		_warehouseItems = new Dictionary<ItemKey, int>(0);
		_warehouseMaxLoad = 0;
		_warehouseCurrLoad = 0;
		_buildingSpaceLimit = 0;
		_buildingSpaceCurr = 0;
		_buildingSpaceExtraAdd = 0;
		_prosperousConstruction = false;
		_combatSkills = new Dictionary<short, TaiwuCombatSkill>(0);
		_lifeSkills = new Dictionary<short, TaiwuLifeSkill>(0);
		_combatSkillPlans = new CombatSkillPlan[9];
		_currCombatSkillPlanId = 0;
		_currLifeSkillAttainmentPanelPlanIndex = new sbyte[16];
		_skillBreakPlateObsoleteDict = new Dictionary<short, SkillBreakPlateObsolete>(0);
		_skillBreakBonusDict = new Dictionary<short, SkillBreakBonusCollection>(0);
		_teachTaiwuLifeSkillDict = new Dictionary<int, GameData.Utilities.ShortList>(0);
		_teachTaiwuCombatSkillDict = new Dictionary<int, GameData.Utilities.ShortList>(0);
		_combatSkillAttainmentPanelPlans = new short[630];
		_currCombatSkillAttainmentPanelPlanIds = new sbyte[14];
		_moveTimeCostPercent = 0;
		_weaponInnerRatios = new sbyte[6];
		_weaponCurrInnerRatios = new sbyte[7];
		_appointments = new Dictionary<int, short>(0);
		_babyBonusMainAttributes = 0;
		_babyBonusLifeSkillQualifications = 0;
		_babyBonusCombatSkillQualifications = 0;
		_equipmentsPlans = new EquipmentPlan[5];
		_currEquipmentPlanId = 0;
		_groupCharIds = default(CharacterSet);
		_combatGroupCharIds = new int[3];
		_taiwuGroupMaxCount = 0;
		_legacyPointDict = new Dictionary<short, short>(0);
		_legacyPoint = 0;
		_availableLegacyList = new List<short>();
		_legacyPassingState = 0;
		_successorCandidates = new List<int>();
		_stateNewCharacterLegacyGrowingGrades = new SByteList[15];
		_notLearnCombatSkillReadingProgress = new Dictionary<short, TaiwuCombatSkill>(0);
		_notLearnLifeSkillReadingProgress = new Dictionary<short, TaiwuLifeSkill>(0);
		_readingBooks = new Dictionary<ItemKey, ReadingBookStrategies>(0);
		_curReadingBook = default(ItemKey);
		_referenceBooks = new ItemKey[3];
		_referenceBookSlotUnlockStates = 0;
		_readingEventTriggered = false;
		_readInCombatCount = 0;
		_healingOuterInjuryRestriction = false;
		_healingInnerInjuryRestriction = false;
		_neiliAllocationTypeRestriction = 0;
		_visitedSettlements = new List<short>();
		_taiwuVillageSettlementId = 0;
		_villagerWork = new Dictionary<int, VillagerWorkData>(0);
		_villagerWorkLocations = new HashSetAsDictionary<Location>();
		_materialResourceMaxCount = 0;
		_resourceChange = new int[8];
		_workLocationMaxCount = 0;
		_totalVillagerCount = 0;
		_totalAdultVillagerCount = 0;
		_availableVillagerCount = 0;
		_isTaiwuDieOfCombatWithXiangshu = false;
		_villagerLearnLifeSkillsFromSect = false;
		_villagerLearnCombatSkillsFromSect = false;
		_overweightSanctionPercent = new List<IntPair>();
		_referenceSkillSlotUnlockStates = 0;
		_taiwuGroupWorstInjuries = default(Injuries);
		_totalResources = default(ResourceInts);
		_taiwuSpecialGroup = new List<int>();
		_taiwuGearMateGroup = new List<int>();
		_canBreakOut = false;
		_troughMaxLoad = 0;
		_troughCurrLoad = 0;
		_clothingDurability = new List<IntPair>();
		OnInitializedDomainData();
	}

	public int GetTaiwuCharId()
	{
		return _taiwuCharId;
	}

	private unsafe void SetTaiwuCharId(int value, DataContext context)
	{
		_taiwuCharId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 0, 4);
		*(int*)ptr = _taiwuCharId;
		ptr += 4;
	}

	public int GetTaiwuGenerationsCount()
	{
		return _taiwuGenerationsCount;
	}

	public unsafe void SetTaiwuGenerationsCount(int value, DataContext context)
	{
		_taiwuGenerationsCount = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 1, 4);
		*(int*)ptr = _taiwuGenerationsCount;
		ptr += 4;
	}

	public int GetCricketLuckPoint()
	{
		return _cricketLuckPoint;
	}

	public unsafe void SetCricketLuckPoint(int value, DataContext context)
	{
		_cricketLuckPoint = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 2, 4);
		*(int*)ptr = _cricketLuckPoint;
		ptr += 4;
	}

	public List<int> GetPreviousTaiwuIds()
	{
		return _previousTaiwuIds;
	}

	public unsafe void SetPreviousTaiwuIds(List<int> value, DataContext context)
	{
		_previousTaiwuIds = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		int count = _previousTaiwuIds.Count;
		int num = 4 * count;
		int valueSize = 2 + num;
		byte* ptr = OperationAdder.DynamicSingleValue_Set(5, 3, valueSize);
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			((int*)ptr)[i] = _previousTaiwuIds[i];
		}
		ptr += num;
	}

	public bool GetNeedToEscape()
	{
		return _needToEscape;
	}

	public unsafe void SetNeedToEscape(bool value, DataContext context)
	{
		_needToEscape = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 4, 1);
		*ptr = (_needToEscape ? ((byte)1) : ((byte)0));
		ptr++;
	}

	public List<ItemDisplayData> GetReceivedItems()
	{
		return _receivedItems;
	}

	public void SetReceivedItems(List<ItemDisplayData> value, DataContext context)
	{
		_receivedItems = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
	}

	public List<CharacterDisplayData> GetReceivedCharacters()
	{
		return _receivedCharacters;
	}

	public void SetReceivedCharacters(List<CharacterDisplayData> value, DataContext context)
	{
		_receivedCharacters = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
	}

	private int GetElement_WarehouseItems(ItemKey elementId)
	{
		return _warehouseItems[elementId];
	}

	private bool TryGetElement_WarehouseItems(ItemKey elementId, out int value)
	{
		return _warehouseItems.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_WarehouseItems(ItemKey elementId, int value, DataContext context)
	{
		_warehouseItems.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(5, 7, elementId, 4);
		*(int*)ptr = value;
		ptr += 4;
	}

	private unsafe void SetElement_WarehouseItems(ItemKey elementId, int value, DataContext context)
	{
		_warehouseItems[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(5, 7, elementId, 4);
		*(int*)ptr = value;
		ptr += 4;
	}

	private void RemoveElement_WarehouseItems(ItemKey elementId, DataContext context)
	{
		_warehouseItems.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(5, 7, elementId);
	}

	private void ClearWarehouseItems(DataContext context)
	{
		_warehouseItems.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(5, 7);
	}

	public int GetWarehouseMaxLoad()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 8))
		{
			return _warehouseMaxLoad;
		}
		int warehouseMaxLoad = CalcWarehouseMaxLoad();
		bool lockTaken = false;
		try
		{
			_spinLockWarehouseMaxLoad.Enter(ref lockTaken);
			_warehouseMaxLoad = warehouseMaxLoad;
			BaseGameDataDomain.SetCached(DataStates, 8);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockWarehouseMaxLoad.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _warehouseMaxLoad;
	}

	public int GetWarehouseCurrLoad()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 9))
		{
			return _warehouseCurrLoad;
		}
		int warehouseCurrLoad = CalcWarehouseCurrLoad();
		bool lockTaken = false;
		try
		{
			_spinLockWarehouseCurrLoad.Enter(ref lockTaken);
			_warehouseCurrLoad = warehouseCurrLoad;
			BaseGameDataDomain.SetCached(DataStates, 9);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockWarehouseCurrLoad.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _warehouseCurrLoad;
	}

	public int GetBuildingSpaceLimit()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 10))
		{
			return _buildingSpaceLimit;
		}
		int buildingSpaceLimit = CalcBuildingSpaceLimit();
		bool lockTaken = false;
		try
		{
			_spinLockBuildingSpaceLimit.Enter(ref lockTaken);
			_buildingSpaceLimit = buildingSpaceLimit;
			BaseGameDataDomain.SetCached(DataStates, 10);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockBuildingSpaceLimit.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _buildingSpaceLimit;
	}

	public int GetBuildingSpaceCurr()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 11))
		{
			return _buildingSpaceCurr;
		}
		int buildingSpaceCurr = CalcBuildingSpaceCurr();
		bool lockTaken = false;
		try
		{
			_spinLockBuildingSpaceCurr.Enter(ref lockTaken);
			_buildingSpaceCurr = buildingSpaceCurr;
			BaseGameDataDomain.SetCached(DataStates, 11);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockBuildingSpaceCurr.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _buildingSpaceCurr;
	}

	public int GetBuildingSpaceExtraAdd()
	{
		return _buildingSpaceExtraAdd;
	}

	public unsafe void SetBuildingSpaceExtraAdd(int value, DataContext context)
	{
		_buildingSpaceExtraAdd = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 12, 4);
		*(int*)ptr = _buildingSpaceExtraAdd;
		ptr += 4;
	}

	public bool GetProsperousConstruction()
	{
		return _prosperousConstruction;
	}

	public unsafe void SetProsperousConstruction(bool value, DataContext context)
	{
		_prosperousConstruction = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 13, 1);
		*ptr = (_prosperousConstruction ? ((byte)1) : ((byte)0));
		ptr++;
	}

	public TaiwuCombatSkill GetElement_CombatSkills(short elementId)
	{
		return _combatSkills[elementId];
	}

	public bool TryGetElement_CombatSkills(short elementId, out TaiwuCombatSkill value)
	{
		return _combatSkills.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_CombatSkills(short elementId, TaiwuCombatSkill value, DataContext context)
	{
		_combatSkills.Add(elementId, value);
		_modificationsCombatSkills.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(5, 14, elementId, 20);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_CombatSkills(short elementId, TaiwuCombatSkill value, DataContext context)
	{
		_combatSkills[elementId] = value;
		_modificationsCombatSkills.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(5, 14, elementId, 20);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_CombatSkills(short elementId, DataContext context)
	{
		_combatSkills.Remove(elementId);
		_modificationsCombatSkills.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(5, 14, elementId);
	}

	private void ClearCombatSkills(DataContext context)
	{
		_combatSkills.Clear();
		_modificationsCombatSkills.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(5, 14);
	}

	public TaiwuLifeSkill GetElement_LifeSkills(short elementId)
	{
		return _lifeSkills[elementId];
	}

	public bool TryGetElement_LifeSkills(short elementId, out TaiwuLifeSkill value)
	{
		return _lifeSkills.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_LifeSkills(short elementId, TaiwuLifeSkill value, DataContext context)
	{
		_lifeSkills.Add(elementId, value);
		_modificationsLifeSkills.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(5, 15, elementId, 5);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_LifeSkills(short elementId, TaiwuLifeSkill value, DataContext context)
	{
		_lifeSkills[elementId] = value;
		_modificationsLifeSkills.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(5, 15, elementId, 5);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_LifeSkills(short elementId, DataContext context)
	{
		_lifeSkills.Remove(elementId);
		_modificationsLifeSkills.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(5, 15, elementId);
	}

	private void ClearLifeSkills(DataContext context)
	{
		_lifeSkills.Clear();
		_modificationsLifeSkills.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(5, 15);
	}

	private CombatSkillPlan GetElement_CombatSkillPlans(int index)
	{
		return _combatSkillPlans[index];
	}

	private unsafe void SetElement_CombatSkillPlans(int index, CombatSkillPlan value, DataContext context)
	{
		_combatSkillPlans[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesCombatSkillPlans, CacheInfluencesCombatSkillPlans, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicElementList_Set(5, 16, index, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicElementList_Set(5, 16, index, 0);
		}
	}

	public int GetCurrCombatSkillPlanId()
	{
		return _currCombatSkillPlanId;
	}

	private unsafe void SetCurrCombatSkillPlanId(int value, DataContext context)
	{
		_currCombatSkillPlanId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 17, 4);
		*(int*)ptr = _currCombatSkillPlanId;
		ptr += 4;
	}

	public sbyte GetElement_CurrLifeSkillAttainmentPanelPlanIndex(int index)
	{
		return _currLifeSkillAttainmentPanelPlanIndex[index];
	}

	public unsafe void SetElement_CurrLifeSkillAttainmentPanelPlanIndex(int index, sbyte value, DataContext context)
	{
		_currLifeSkillAttainmentPanelPlanIndex[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesCurrLifeSkillAttainmentPanelPlanIndex, CacheInfluencesCurrLifeSkillAttainmentPanelPlanIndex, context);
		byte* ptr = OperationAdder.FixedElementList_Set(5, 18, index, 1);
		*ptr = (byte)value;
		ptr++;
	}

	public SkillBreakPlateObsolete GetElement_SkillBreakPlateObsoleteDict(short elementId)
	{
		return _skillBreakPlateObsoleteDict[elementId];
	}

	public bool TryGetElement_SkillBreakPlateObsoleteDict(short elementId, out SkillBreakPlateObsolete value)
	{
		return _skillBreakPlateObsoleteDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_SkillBreakPlateObsoleteDict(short elementId, SkillBreakPlateObsolete value, DataContext context)
	{
		_skillBreakPlateObsoleteDict.Add(elementId, value);
		_modificationsSkillBreakPlateObsoleteDict.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(5, 19, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(5, 19, elementId, 0);
		}
	}

	private unsafe void SetElement_SkillBreakPlateObsoleteDict(short elementId, SkillBreakPlateObsolete value, DataContext context)
	{
		_skillBreakPlateObsoleteDict[elementId] = value;
		_modificationsSkillBreakPlateObsoleteDict.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(5, 19, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(5, 19, elementId, 0);
		}
	}

	private void RemoveElement_SkillBreakPlateObsoleteDict(short elementId, DataContext context)
	{
		_skillBreakPlateObsoleteDict.Remove(elementId);
		_modificationsSkillBreakPlateObsoleteDict.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(5, 19, elementId);
	}

	private void ClearSkillBreakPlateObsoleteDict(DataContext context)
	{
		_skillBreakPlateObsoleteDict.Clear();
		_modificationsSkillBreakPlateObsoleteDict.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(5, 19);
	}

	private SkillBreakBonusCollection GetElement_SkillBreakBonusDict(short elementId)
	{
		return _skillBreakBonusDict[elementId];
	}

	private bool TryGetElement_SkillBreakBonusDict(short elementId, out SkillBreakBonusCollection value)
	{
		return _skillBreakBonusDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_SkillBreakBonusDict(short elementId, SkillBreakBonusCollection value, DataContext context)
	{
		_skillBreakBonusDict.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(5, 20, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(5, 20, elementId, 0);
		}
	}

	private unsafe void SetElement_SkillBreakBonusDict(short elementId, SkillBreakBonusCollection value, DataContext context)
	{
		_skillBreakBonusDict[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(5, 20, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(5, 20, elementId, 0);
		}
	}

	private void RemoveElement_SkillBreakBonusDict(short elementId, DataContext context)
	{
		_skillBreakBonusDict.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(5, 20, elementId);
	}

	private void ClearSkillBreakBonusDict(DataContext context)
	{
		_skillBreakBonusDict.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(5, 20);
	}

	public GameData.Utilities.ShortList GetElement_TeachTaiwuLifeSkillDict(int elementId)
	{
		return _teachTaiwuLifeSkillDict[elementId];
	}

	public bool TryGetElement_TeachTaiwuLifeSkillDict(int elementId, out GameData.Utilities.ShortList value)
	{
		return _teachTaiwuLifeSkillDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_TeachTaiwuLifeSkillDict(int elementId, GameData.Utilities.ShortList value, DataContext context)
	{
		_teachTaiwuLifeSkillDict.Add(elementId, value);
		_modificationsTeachTaiwuLifeSkillDict.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(5, 21, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_TeachTaiwuLifeSkillDict(int elementId, GameData.Utilities.ShortList value, DataContext context)
	{
		_teachTaiwuLifeSkillDict[elementId] = value;
		_modificationsTeachTaiwuLifeSkillDict.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(5, 21, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_TeachTaiwuLifeSkillDict(int elementId, DataContext context)
	{
		_teachTaiwuLifeSkillDict.Remove(elementId);
		_modificationsTeachTaiwuLifeSkillDict.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(5, 21, elementId);
	}

	private void ClearTeachTaiwuLifeSkillDict(DataContext context)
	{
		_teachTaiwuLifeSkillDict.Clear();
		_modificationsTeachTaiwuLifeSkillDict.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(5, 21);
	}

	public GameData.Utilities.ShortList GetElement_TeachTaiwuCombatSkillDict(int elementId)
	{
		return _teachTaiwuCombatSkillDict[elementId];
	}

	public bool TryGetElement_TeachTaiwuCombatSkillDict(int elementId, out GameData.Utilities.ShortList value)
	{
		return _teachTaiwuCombatSkillDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_TeachTaiwuCombatSkillDict(int elementId, GameData.Utilities.ShortList value, DataContext context)
	{
		_teachTaiwuCombatSkillDict.Add(elementId, value);
		_modificationsTeachTaiwuCombatSkillDict.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(5, 22, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_TeachTaiwuCombatSkillDict(int elementId, GameData.Utilities.ShortList value, DataContext context)
	{
		_teachTaiwuCombatSkillDict[elementId] = value;
		_modificationsTeachTaiwuCombatSkillDict.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(5, 22, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_TeachTaiwuCombatSkillDict(int elementId, DataContext context)
	{
		_teachTaiwuCombatSkillDict.Remove(elementId);
		_modificationsTeachTaiwuCombatSkillDict.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(5, 22, elementId);
	}

	private void ClearTeachTaiwuCombatSkillDict(DataContext context)
	{
		_teachTaiwuCombatSkillDict.Clear();
		_modificationsTeachTaiwuCombatSkillDict.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(5, 22);
	}

	public short[] GetCombatSkillAttainmentPanelPlans()
	{
		return _combatSkillAttainmentPanelPlans;
	}

	private unsafe void SetCombatSkillAttainmentPanelPlans(short[] value, DataContext context)
	{
		_combatSkillAttainmentPanelPlans = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 23, 1260);
		for (int i = 0; i < 630; i++)
		{
			((short*)ptr)[i] = _combatSkillAttainmentPanelPlans[i];
		}
		ptr += 1260;
	}

	public sbyte[] GetCurrCombatSkillAttainmentPanelPlanIds()
	{
		return _currCombatSkillAttainmentPanelPlanIds;
	}

	private unsafe void SetCurrCombatSkillAttainmentPanelPlanIds(sbyte[] value, DataContext context)
	{
		_currCombatSkillAttainmentPanelPlanIds = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 24, 14);
		for (int i = 0; i < 14; i++)
		{
			ptr[i] = (byte)_currCombatSkillAttainmentPanelPlanIds[i];
		}
		ptr += 14;
	}

	public int GetMoveTimeCostPercent()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 25))
		{
			return _moveTimeCostPercent;
		}
		int moveTimeCostPercent = CalcMoveTimeCostPercent();
		bool lockTaken = false;
		try
		{
			_spinLockMoveTimeCostPercent.Enter(ref lockTaken);
			_moveTimeCostPercent = moveTimeCostPercent;
			BaseGameDataDomain.SetCached(DataStates, 25);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockMoveTimeCostPercent.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _moveTimeCostPercent;
	}

	public sbyte[] GetWeaponInnerRatios()
	{
		return _weaponInnerRatios;
	}

	public unsafe void SetWeaponInnerRatios(sbyte[] value, DataContext context)
	{
		_weaponInnerRatios = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 26, 6);
		for (int i = 0; i < 6; i++)
		{
			ptr[i] = (byte)_weaponInnerRatios[i];
		}
		ptr += 6;
	}

	public sbyte[] GetWeaponCurrInnerRatios()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 27))
		{
			return _weaponCurrInnerRatios;
		}
		sbyte[] array = new sbyte[7];
		CalcWeaponCurrInnerRatios(array);
		bool lockTaken = false;
		try
		{
			_spinLockWeaponCurrInnerRatios.Enter(ref lockTaken);
			for (int i = 0; i < 7; i++)
			{
				_weaponCurrInnerRatios[i] = array[i];
			}
			BaseGameDataDomain.SetCached(DataStates, 27);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockWeaponCurrInnerRatios.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _weaponCurrInnerRatios;
	}

	public short GetElement_Appointments(int elementId)
	{
		return _appointments[elementId];
	}

	public bool TryGetElement_Appointments(int elementId, out short value)
	{
		return _appointments.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_Appointments(int elementId, short value, DataContext context)
	{
		_appointments.Add(elementId, value);
		_modificationsAppointments.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(5, 28, elementId, 2);
		*(short*)ptr = value;
		ptr += 2;
	}

	private unsafe void SetElement_Appointments(int elementId, short value, DataContext context)
	{
		_appointments[elementId] = value;
		_modificationsAppointments.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(5, 28, elementId, 2);
		*(short*)ptr = value;
		ptr += 2;
	}

	private void RemoveElement_Appointments(int elementId, DataContext context)
	{
		_appointments.Remove(elementId);
		_modificationsAppointments.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(5, 28, elementId);
	}

	private void ClearAppointments(DataContext context)
	{
		_appointments.Clear();
		_modificationsAppointments.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(5, 28);
	}

	private short GetBabyBonusMainAttributes()
	{
		return _babyBonusMainAttributes;
	}

	private unsafe void SetBabyBonusMainAttributes(short value, DataContext context)
	{
		_babyBonusMainAttributes = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(29, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 29, 2);
		*(short*)ptr = _babyBonusMainAttributes;
		ptr += 2;
	}

	private short GetBabyBonusLifeSkillQualifications()
	{
		return _babyBonusLifeSkillQualifications;
	}

	private unsafe void SetBabyBonusLifeSkillQualifications(short value, DataContext context)
	{
		_babyBonusLifeSkillQualifications = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(30, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 30, 2);
		*(short*)ptr = _babyBonusLifeSkillQualifications;
		ptr += 2;
	}

	private short GetBabyBonusCombatSkillQualifications()
	{
		return _babyBonusCombatSkillQualifications;
	}

	private unsafe void SetBabyBonusCombatSkillQualifications(short value, DataContext context)
	{
		_babyBonusCombatSkillQualifications = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(31, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 31, 2);
		*(short*)ptr = _babyBonusCombatSkillQualifications;
		ptr += 2;
	}

	private EquipmentPlan GetElement_EquipmentsPlans(int index)
	{
		return _equipmentsPlans[index];
	}

	private unsafe void SetElement_EquipmentsPlans(int index, EquipmentPlan value, DataContext context)
	{
		_equipmentsPlans[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesEquipmentsPlans, CacheInfluencesEquipmentsPlans, context);
		byte* ptr = OperationAdder.FixedElementList_Set(5, 32, index, 99);
		ptr += value.Serialize(ptr);
	}

	public int GetCurrEquipmentPlanId()
	{
		return _currEquipmentPlanId;
	}

	private unsafe void SetCurrEquipmentPlanId(int value, DataContext context)
	{
		_currEquipmentPlanId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(33, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 33, 4);
		*(int*)ptr = _currEquipmentPlanId;
		ptr += 4;
	}

	public CharacterSet GetGroupCharIds()
	{
		return _groupCharIds;
	}

	private unsafe void SetGroupCharIds(CharacterSet value, DataContext context)
	{
		_groupCharIds = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(34, DataStates, CacheInfluences, context);
		int serializedSize = _groupCharIds.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValue_Set(5, 34, serializedSize);
		ptr += _groupCharIds.Serialize(ptr);
	}

	public int GetElement_CombatGroupCharIds(int index)
	{
		return _combatGroupCharIds[index];
	}

	public unsafe void SetElement_CombatGroupCharIds(int index, int value, DataContext context)
	{
		_combatGroupCharIds[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesCombatGroupCharIds, CacheInfluencesCombatGroupCharIds, context);
		byte* ptr = OperationAdder.FixedElementList_Set(5, 35, index, 4);
		*(int*)ptr = value;
		ptr += 4;
	}

	public int GetTaiwuGroupMaxCount()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 36))
		{
			return _taiwuGroupMaxCount;
		}
		int taiwuGroupMaxCount = CalcTaiwuGroupMaxCount();
		bool lockTaken = false;
		try
		{
			_spinLockTaiwuGroupMaxCount.Enter(ref lockTaken);
			_taiwuGroupMaxCount = taiwuGroupMaxCount;
			BaseGameDataDomain.SetCached(DataStates, 36);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockTaiwuGroupMaxCount.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _taiwuGroupMaxCount;
	}

	public short GetElement_LegacyPointDict(short elementId)
	{
		return _legacyPointDict[elementId];
	}

	public bool TryGetElement_LegacyPointDict(short elementId, out short value)
	{
		return _legacyPointDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_LegacyPointDict(short elementId, short value, DataContext context)
	{
		_legacyPointDict.Add(elementId, value);
		_modificationsLegacyPointDict.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(5, 37, elementId, 2);
		*(short*)ptr = value;
		ptr += 2;
	}

	private unsafe void SetElement_LegacyPointDict(short elementId, short value, DataContext context)
	{
		_legacyPointDict[elementId] = value;
		_modificationsLegacyPointDict.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(5, 37, elementId, 2);
		*(short*)ptr = value;
		ptr += 2;
	}

	private void RemoveElement_LegacyPointDict(short elementId, DataContext context)
	{
		_legacyPointDict.Remove(elementId);
		_modificationsLegacyPointDict.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(5, 37, elementId);
	}

	private void ClearLegacyPointDict(DataContext context)
	{
		_legacyPointDict.Clear();
		_modificationsLegacyPointDict.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(5, 37);
	}

	public int GetLegacyPoint()
	{
		return _legacyPoint;
	}

	public unsafe void SetLegacyPoint(int value, DataContext context)
	{
		_legacyPoint = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(38, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 38, 4);
		*(int*)ptr = _legacyPoint;
		ptr += 4;
	}

	public List<short> GetAvailableLegacyList()
	{
		return _availableLegacyList;
	}

	public unsafe void SetAvailableLegacyList(List<short> value, DataContext context)
	{
		_availableLegacyList = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(39, DataStates, CacheInfluences, context);
		int count = _availableLegacyList.Count;
		int num = 2 * count;
		int valueSize = 2 + num;
		byte* ptr = OperationAdder.DynamicSingleValue_Set(5, 39, valueSize);
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			((short*)ptr)[i] = _availableLegacyList[i];
		}
		ptr += num;
	}

	public sbyte GetLegacyPassingState()
	{
		return _legacyPassingState;
	}

	private void SetLegacyPassingState(sbyte value, DataContext context)
	{
		_legacyPassingState = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(40, DataStates, CacheInfluences, context);
	}

	public List<int> GetSuccessorCandidates()
	{
		return _successorCandidates;
	}

	private void SetSuccessorCandidates(List<int> value, DataContext context)
	{
		_successorCandidates = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(41, DataStates, CacheInfluences, context);
	}

	public SByteList GetElement_StateNewCharacterLegacyGrowingGrades(int index)
	{
		return _stateNewCharacterLegacyGrowingGrades[index];
	}

	public unsafe void SetElement_StateNewCharacterLegacyGrowingGrades(int index, SByteList value, DataContext context)
	{
		_stateNewCharacterLegacyGrowingGrades[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesStateNewCharacterLegacyGrowingGrades, CacheInfluencesStateNewCharacterLegacyGrowingGrades, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicElementList_Set(5, 42, index, serializedSize);
		ptr += value.Serialize(ptr);
	}

	public TaiwuCombatSkill GetElement_NotLearnCombatSkillReadingProgress(short elementId)
	{
		return _notLearnCombatSkillReadingProgress[elementId];
	}

	public bool TryGetElement_NotLearnCombatSkillReadingProgress(short elementId, out TaiwuCombatSkill value)
	{
		return _notLearnCombatSkillReadingProgress.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_NotLearnCombatSkillReadingProgress(short elementId, TaiwuCombatSkill value, DataContext context)
	{
		_notLearnCombatSkillReadingProgress.Add(elementId, value);
		_modificationsNotLearnCombatSkillReadingProgress.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(5, 43, elementId, 20);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_NotLearnCombatSkillReadingProgress(short elementId, TaiwuCombatSkill value, DataContext context)
	{
		_notLearnCombatSkillReadingProgress[elementId] = value;
		_modificationsNotLearnCombatSkillReadingProgress.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(5, 43, elementId, 20);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_NotLearnCombatSkillReadingProgress(short elementId, DataContext context)
	{
		_notLearnCombatSkillReadingProgress.Remove(elementId);
		_modificationsNotLearnCombatSkillReadingProgress.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(5, 43, elementId);
	}

	private void ClearNotLearnCombatSkillReadingProgress(DataContext context)
	{
		_notLearnCombatSkillReadingProgress.Clear();
		_modificationsNotLearnCombatSkillReadingProgress.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(5, 43);
	}

	public TaiwuLifeSkill GetElement_NotLearnLifeSkillReadingProgress(short elementId)
	{
		return _notLearnLifeSkillReadingProgress[elementId];
	}

	public bool TryGetElement_NotLearnLifeSkillReadingProgress(short elementId, out TaiwuLifeSkill value)
	{
		return _notLearnLifeSkillReadingProgress.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_NotLearnLifeSkillReadingProgress(short elementId, TaiwuLifeSkill value, DataContext context)
	{
		_notLearnLifeSkillReadingProgress.Add(elementId, value);
		_modificationsNotLearnLifeSkillReadingProgress.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(5, 44, elementId, 5);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_NotLearnLifeSkillReadingProgress(short elementId, TaiwuLifeSkill value, DataContext context)
	{
		_notLearnLifeSkillReadingProgress[elementId] = value;
		_modificationsNotLearnLifeSkillReadingProgress.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(5, 44, elementId, 5);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_NotLearnLifeSkillReadingProgress(short elementId, DataContext context)
	{
		_notLearnLifeSkillReadingProgress.Remove(elementId);
		_modificationsNotLearnLifeSkillReadingProgress.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(5, 44, elementId);
	}

	private void ClearNotLearnLifeSkillReadingProgress(DataContext context)
	{
		_notLearnLifeSkillReadingProgress.Clear();
		_modificationsNotLearnLifeSkillReadingProgress.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(5, 44);
	}

	private ReadingBookStrategies GetElement_ReadingBooks(ItemKey elementId)
	{
		return _readingBooks[elementId];
	}

	private bool TryGetElement_ReadingBooks(ItemKey elementId, out ReadingBookStrategies value)
	{
		return _readingBooks.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_ReadingBooks(ItemKey elementId, ref ReadingBookStrategies value, DataContext context)
	{
		_readingBooks.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(5, 45, elementId, 36);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_ReadingBooks(ItemKey elementId, ref ReadingBookStrategies value, DataContext context)
	{
		_readingBooks[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(5, 45, elementId, 36);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_ReadingBooks(ItemKey elementId, DataContext context)
	{
		_readingBooks.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(5, 45, elementId);
	}

	private void ClearReadingBooks(DataContext context)
	{
		_readingBooks.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(5, 45);
	}

	public ItemKey GetCurReadingBook()
	{
		return _curReadingBook;
	}

	private unsafe void SetCurReadingBook(ItemKey value, DataContext context)
	{
		_curReadingBook = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(46, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 46, 8);
		ptr += _curReadingBook.Serialize(ptr);
	}

	public ItemKey[] GetReferenceBooks()
	{
		return _referenceBooks;
	}

	private unsafe void SetReferenceBooks(ItemKey[] value, DataContext context)
	{
		_referenceBooks = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(47, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 47, 24);
		for (int i = 0; i < 3; i++)
		{
			ptr += _referenceBooks[i].Serialize(ptr);
		}
	}

	public byte GetReferenceBookSlotUnlockStates()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 48))
		{
			return _referenceBookSlotUnlockStates;
		}
		byte referenceBookSlotUnlockStates = CalcReferenceBookSlotUnlockStates();
		bool lockTaken = false;
		try
		{
			_spinLockReferenceBookSlotUnlockStates.Enter(ref lockTaken);
			_referenceBookSlotUnlockStates = referenceBookSlotUnlockStates;
			BaseGameDataDomain.SetCached(DataStates, 48);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockReferenceBookSlotUnlockStates.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _referenceBookSlotUnlockStates;
	}

	[Obsolete("DomainData _readingEventTriggered is no longer in use.")]
	public bool GetReadingEventTriggered()
	{
		return _readingEventTriggered;
	}

	[Obsolete("DomainData _readingEventTriggered is no longer in use.")]
	public unsafe void SetReadingEventTriggered(bool value, DataContext context)
	{
		_readingEventTriggered = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(49, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 49, 1);
		*ptr = (_readingEventTriggered ? ((byte)1) : ((byte)0));
		ptr++;
	}

	public sbyte GetReadInCombatCount()
	{
		return _readInCombatCount;
	}

	public unsafe void SetReadInCombatCount(sbyte value, DataContext context)
	{
		_readInCombatCount = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(50, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 50, 1);
		*ptr = (byte)_readInCombatCount;
		ptr++;
	}

	public bool GetHealingOuterInjuryRestriction()
	{
		return _healingOuterInjuryRestriction;
	}

	public void SetHealingOuterInjuryRestriction(bool value, DataContext context)
	{
		_healingOuterInjuryRestriction = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(51, DataStates, CacheInfluences, context);
	}

	public bool GetHealingInnerInjuryRestriction()
	{
		return _healingInnerInjuryRestriction;
	}

	public void SetHealingInnerInjuryRestriction(bool value, DataContext context)
	{
		_healingInnerInjuryRestriction = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(52, DataStates, CacheInfluences, context);
	}

	public byte GetNeiliAllocationTypeRestriction()
	{
		return _neiliAllocationTypeRestriction;
	}

	private void SetNeiliAllocationTypeRestriction(byte value, DataContext context)
	{
		_neiliAllocationTypeRestriction = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(53, DataStates, CacheInfluences, context);
	}

	public List<short> GetVisitedSettlements()
	{
		return _visitedSettlements;
	}

	public unsafe void SetVisitedSettlements(List<short> value, DataContext context)
	{
		_visitedSettlements = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(54, DataStates, CacheInfluences, context);
		int count = _visitedSettlements.Count;
		int num = 2 * count;
		int valueSize = 2 + num;
		byte* ptr = OperationAdder.DynamicSingleValue_Set(5, 54, valueSize);
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			((short*)ptr)[i] = _visitedSettlements[i];
		}
		ptr += num;
	}

	public short GetTaiwuVillageSettlementId()
	{
		return _taiwuVillageSettlementId;
	}

	public unsafe void SetTaiwuVillageSettlementId(short value, DataContext context)
	{
		_taiwuVillageSettlementId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(55, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 55, 2);
		*(short*)ptr = _taiwuVillageSettlementId;
		ptr += 2;
	}

	public VillagerWorkData GetElement_VillagerWork(int elementId)
	{
		return _villagerWork[elementId];
	}

	public bool TryGetElement_VillagerWork(int elementId, out VillagerWorkData value)
	{
		return _villagerWork.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_VillagerWork(int elementId, VillagerWorkData value, DataContext context)
	{
		_villagerWork.Add(elementId, value);
		_modificationsVillagerWork.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(56, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Add(5, 56, elementId, 20);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_VillagerWork(int elementId, VillagerWorkData value, DataContext context)
	{
		_villagerWork[elementId] = value;
		_modificationsVillagerWork.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(56, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValueCollection_Set(5, 56, elementId, 20);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_VillagerWork(int elementId, DataContext context)
	{
		_villagerWork.Remove(elementId);
		_modificationsVillagerWork.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(56, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Remove(5, 56, elementId);
	}

	private void ClearVillagerWork(DataContext context)
	{
		_villagerWork.Clear();
		_modificationsVillagerWork.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(56, DataStates, CacheInfluences, context);
		OperationAdder.FixedSingleValueCollection_Clear(5, 56);
	}

	public VoidValue GetElement_VillagerWorkLocations(Location elementId)
	{
		return _villagerWorkLocations[elementId];
	}

	public bool TryGetElement_VillagerWorkLocations(Location elementId, out VoidValue value)
	{
		return _villagerWorkLocations.TryGetValue(elementId, out value);
	}

	private void AddElement_VillagerWorkLocations(Location elementId, VoidValue value, DataContext context)
	{
		_villagerWorkLocations.Add(elementId, value);
		_modificationsVillagerWorkLocations.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(57, DataStates, CacheInfluences, context);
	}

	private void SetElement_VillagerWorkLocations(Location elementId, VoidValue value, DataContext context)
	{
		_villagerWorkLocations[elementId] = value;
		_modificationsVillagerWorkLocations.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(57, DataStates, CacheInfluences, context);
	}

	private void RemoveElement_VillagerWorkLocations(Location elementId, DataContext context)
	{
		_villagerWorkLocations.Remove(elementId);
		_modificationsVillagerWorkLocations.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(57, DataStates, CacheInfluences, context);
	}

	private void ClearVillagerWorkLocations(DataContext context)
	{
		_villagerWorkLocations.Clear();
		_modificationsVillagerWorkLocations.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(57, DataStates, CacheInfluences, context);
	}

	public int GetMaterialResourceMaxCount()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 58))
		{
			return _materialResourceMaxCount;
		}
		int materialResourceMaxCount = CalcMaterialResourceMaxCount();
		bool lockTaken = false;
		try
		{
			_spinLockMaterialResourceMaxCount.Enter(ref lockTaken);
			_materialResourceMaxCount = materialResourceMaxCount;
			BaseGameDataDomain.SetCached(DataStates, 58);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockMaterialResourceMaxCount.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _materialResourceMaxCount;
	}

	public int[] GetResourceChange()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 59))
		{
			return _resourceChange;
		}
		int[] array = new int[8];
		CalcResourceChange(array);
		bool lockTaken = false;
		try
		{
			_spinLockResourceChange.Enter(ref lockTaken);
			for (int i = 0; i < 8; i++)
			{
				_resourceChange[i] = array[i];
			}
			BaseGameDataDomain.SetCached(DataStates, 59);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockResourceChange.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _resourceChange;
	}

	public short GetWorkLocationMaxCount()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 60))
		{
			return _workLocationMaxCount;
		}
		short workLocationMaxCount = CalcWorkLocationMaxCount();
		bool lockTaken = false;
		try
		{
			_spinLockWorkLocationMaxCount.Enter(ref lockTaken);
			_workLocationMaxCount = workLocationMaxCount;
			BaseGameDataDomain.SetCached(DataStates, 60);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockWorkLocationMaxCount.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _workLocationMaxCount;
	}

	public short GetTotalVillagerCount()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 61))
		{
			return _totalVillagerCount;
		}
		short totalVillagerCount = CalcTotalVillagerCount();
		bool lockTaken = false;
		try
		{
			_spinLockTotalVillagerCount.Enter(ref lockTaken);
			_totalVillagerCount = totalVillagerCount;
			BaseGameDataDomain.SetCached(DataStates, 61);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockTotalVillagerCount.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _totalVillagerCount;
	}

	public short GetTotalAdultVillagerCount()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 62))
		{
			return _totalAdultVillagerCount;
		}
		short totalAdultVillagerCount = CalcTotalAdultVillagerCount();
		bool lockTaken = false;
		try
		{
			_spinLockTotalAdultVillagerCount.Enter(ref lockTaken);
			_totalAdultVillagerCount = totalAdultVillagerCount;
			BaseGameDataDomain.SetCached(DataStates, 62);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockTotalAdultVillagerCount.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _totalAdultVillagerCount;
	}

	public short GetAvailableVillagerCount()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 63))
		{
			return _availableVillagerCount;
		}
		short availableVillagerCount = CalcAvailableVillagerCount();
		bool lockTaken = false;
		try
		{
			_spinLockAvailableVillagerCount.Enter(ref lockTaken);
			_availableVillagerCount = availableVillagerCount;
			BaseGameDataDomain.SetCached(DataStates, 63);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockAvailableVillagerCount.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _availableVillagerCount;
	}

	public bool GetIsTaiwuDieOfCombatWithXiangshu()
	{
		return _isTaiwuDieOfCombatWithXiangshu;
	}

	public void SetIsTaiwuDieOfCombatWithXiangshu(bool value, DataContext context)
	{
		_isTaiwuDieOfCombatWithXiangshu = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(64, DataStates, CacheInfluences, context);
	}

	public bool GetVillagerLearnLifeSkillsFromSect()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 65))
		{
			return _villagerLearnLifeSkillsFromSect;
		}
		bool villagerLearnLifeSkillsFromSect = CalcVillagerLearnLifeSkillsFromSect();
		bool lockTaken = false;
		try
		{
			_spinLockVillagerLearnLifeSkillsFromSect.Enter(ref lockTaken);
			_villagerLearnLifeSkillsFromSect = villagerLearnLifeSkillsFromSect;
			BaseGameDataDomain.SetCached(DataStates, 65);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockVillagerLearnLifeSkillsFromSect.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _villagerLearnLifeSkillsFromSect;
	}

	public bool GetVillagerLearnCombatSkillsFromSect()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 66))
		{
			return _villagerLearnCombatSkillsFromSect;
		}
		bool villagerLearnCombatSkillsFromSect = CalcVillagerLearnCombatSkillsFromSect();
		bool lockTaken = false;
		try
		{
			_spinLockVillagerLearnCombatSkillsFromSect.Enter(ref lockTaken);
			_villagerLearnCombatSkillsFromSect = villagerLearnCombatSkillsFromSect;
			BaseGameDataDomain.SetCached(DataStates, 66);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockVillagerLearnCombatSkillsFromSect.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _villagerLearnCombatSkillsFromSect;
	}

	public List<IntPair> GetOverweightSanctionPercent()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 67))
		{
			return _overweightSanctionPercent;
		}
		List<IntPair> list = new List<IntPair>();
		CalcOverweightSanctionPercent(list);
		bool lockTaken = false;
		try
		{
			_spinLockOverweightSanctionPercent.Enter(ref lockTaken);
			_overweightSanctionPercent.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 67);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockOverweightSanctionPercent.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _overweightSanctionPercent;
	}

	public byte GetReferenceSkillSlotUnlockStates()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 68))
		{
			return _referenceSkillSlotUnlockStates;
		}
		byte referenceSkillSlotUnlockStates = CalcReferenceSkillSlotUnlockStates();
		bool lockTaken = false;
		try
		{
			_spinLockReferenceSkillSlotUnlockStates.Enter(ref lockTaken);
			_referenceSkillSlotUnlockStates = referenceSkillSlotUnlockStates;
			BaseGameDataDomain.SetCached(DataStates, 68);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockReferenceSkillSlotUnlockStates.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _referenceSkillSlotUnlockStates;
	}

	public Injuries GetTaiwuGroupWorstInjuries()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 69))
		{
			return _taiwuGroupWorstInjuries;
		}
		Injuries taiwuGroupWorstInjuries = CalcTaiwuGroupWorstInjuries();
		bool lockTaken = false;
		try
		{
			_spinLockTaiwuGroupWorstInjuries.Enter(ref lockTaken);
			_taiwuGroupWorstInjuries = taiwuGroupWorstInjuries;
			BaseGameDataDomain.SetCached(DataStates, 69);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockTaiwuGroupWorstInjuries.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _taiwuGroupWorstInjuries;
	}

	public ref ResourceInts GetTotalResources()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 70))
		{
			return ref _totalResources;
		}
		ResourceInts totalResources = CalcTotalResources();
		bool lockTaken = false;
		try
		{
			_spinLockTotalResources.Enter(ref lockTaken);
			_totalResources = totalResources;
			BaseGameDataDomain.SetCached(DataStates, 70);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockTotalResources.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return ref _totalResources;
	}

	public List<int> GetTaiwuSpecialGroup()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 71))
		{
			return _taiwuSpecialGroup;
		}
		List<int> list = new List<int>();
		CalcTaiwuSpecialGroup(list);
		bool lockTaken = false;
		try
		{
			_spinLockTaiwuSpecialGroup.Enter(ref lockTaken);
			_taiwuSpecialGroup.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 71);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockTaiwuSpecialGroup.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _taiwuSpecialGroup;
	}

	public List<int> GetTaiwuGearMateGroup()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 72))
		{
			return _taiwuGearMateGroup;
		}
		List<int> list = new List<int>();
		CalcTaiwuGearMateGroup(list);
		bool lockTaken = false;
		try
		{
			_spinLockTaiwuGearMateGroup.Enter(ref lockTaken);
			_taiwuGearMateGroup.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 72);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockTaiwuGearMateGroup.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _taiwuGearMateGroup;
	}

	public bool GetCanBreakOut()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 73))
		{
			return _canBreakOut;
		}
		bool canBreakOut = CalcCanBreakOut();
		bool lockTaken = false;
		try
		{
			_spinLockCanBreakOut.Enter(ref lockTaken);
			_canBreakOut = canBreakOut;
			BaseGameDataDomain.SetCached(DataStates, 73);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockCanBreakOut.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _canBreakOut;
	}

	public int GetTroughMaxLoad()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 74))
		{
			return _troughMaxLoad;
		}
		int troughMaxLoad = CalcTroughMaxLoad();
		bool lockTaken = false;
		try
		{
			_spinLockTroughMaxLoad.Enter(ref lockTaken);
			_troughMaxLoad = troughMaxLoad;
			BaseGameDataDomain.SetCached(DataStates, 74);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockTroughMaxLoad.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _troughMaxLoad;
	}

	public int GetTroughCurrLoad()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 75))
		{
			return _troughCurrLoad;
		}
		int troughCurrLoad = CalcTroughCurrLoad();
		bool lockTaken = false;
		try
		{
			_spinLockTroughCurrLoad.Enter(ref lockTaken);
			_troughCurrLoad = troughCurrLoad;
			BaseGameDataDomain.SetCached(DataStates, 75);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockTroughCurrLoad.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _troughCurrLoad;
	}

	public List<IntPair> GetClothingDurability()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 76))
		{
			return _clothingDurability;
		}
		List<IntPair> list = new List<IntPair>();
		CalcClothingDurability(list);
		bool lockTaken = false;
		try
		{
			_spinLockClothingDurability.Enter(ref lockTaken);
			_clothingDurability.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 76);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockClothingDurability.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _clothingDurability;
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		byte* ptr = OperationAdder.FixedSingleValue_Set(5, 0, 4);
		*(int*)ptr = _taiwuCharId;
		ptr += 4;
		byte* ptr2 = OperationAdder.FixedSingleValue_Set(5, 1, 4);
		*(int*)ptr2 = _taiwuGenerationsCount;
		ptr2 += 4;
		byte* ptr3 = OperationAdder.FixedSingleValue_Set(5, 2, 4);
		*(int*)ptr3 = _cricketLuckPoint;
		ptr3 += 4;
		int count = _previousTaiwuIds.Count;
		int num = 4 * count;
		int valueSize = 2 + num;
		byte* ptr4 = OperationAdder.DynamicSingleValue_Set(5, 3, valueSize);
		*(ushort*)ptr4 = (ushort)count;
		ptr4 += 2;
		for (int i = 0; i < count; i++)
		{
			((int*)ptr4)[i] = _previousTaiwuIds[i];
		}
		ptr4 += num;
		byte* ptr5 = OperationAdder.FixedSingleValue_Set(5, 4, 1);
		*ptr5 = (_needToEscape ? ((byte)1) : ((byte)0));
		ptr5++;
		foreach (KeyValuePair<ItemKey, int> warehouseItem in _warehouseItems)
		{
			ItemKey key = warehouseItem.Key;
			int value = warehouseItem.Value;
			byte* ptr6 = OperationAdder.FixedSingleValueCollection_Add(5, 7, key, 4);
			*(int*)ptr6 = value;
			ptr6 += 4;
		}
		byte* ptr7 = OperationAdder.FixedSingleValue_Set(5, 12, 4);
		*(int*)ptr7 = _buildingSpaceExtraAdd;
		ptr7 += 4;
		byte* ptr8 = OperationAdder.FixedSingleValue_Set(5, 13, 1);
		*ptr8 = (_prosperousConstruction ? ((byte)1) : ((byte)0));
		ptr8++;
		foreach (KeyValuePair<short, TaiwuCombatSkill> combatSkill in _combatSkills)
		{
			short key2 = combatSkill.Key;
			TaiwuCombatSkill value2 = combatSkill.Value;
			byte* ptr9 = OperationAdder.FixedSingleValueCollection_Add(5, 14, key2, 20);
			ptr9 += value2.Serialize(ptr9);
		}
		foreach (KeyValuePair<short, TaiwuLifeSkill> lifeSkill in _lifeSkills)
		{
			short key3 = lifeSkill.Key;
			TaiwuLifeSkill value3 = lifeSkill.Value;
			byte* ptr10 = OperationAdder.FixedSingleValueCollection_Add(5, 15, key3, 5);
			ptr10 += value3.Serialize(ptr10);
		}
		int num2 = 0;
		for (int j = 0; j < 9; j++)
		{
			CombatSkillPlan combatSkillPlan = _combatSkillPlans[j];
			num2 = ((combatSkillPlan == null) ? (num2 + 4) : (num2 + (4 + combatSkillPlan.GetSerializedSize())));
		}
		byte* ptr11 = OperationAdder.DynamicElementList_InsertRange(5, 16, 0, 9, num2);
		for (int k = 0; k < 9; k++)
		{
			CombatSkillPlan combatSkillPlan2 = _combatSkillPlans[k];
			if (combatSkillPlan2 != null)
			{
				byte* ptr12 = ptr11;
				ptr11 += 4;
				int num3 = combatSkillPlan2.Serialize(ptr11);
				ptr11 += num3;
				*(int*)ptr12 = num3;
			}
			else
			{
				*(int*)ptr11 = 0;
				ptr11 += 4;
			}
		}
		byte* ptr13 = OperationAdder.FixedSingleValue_Set(5, 17, 4);
		*(int*)ptr13 = _currCombatSkillPlanId;
		ptr13 += 4;
		byte* ptr14 = OperationAdder.FixedElementList_InsertRange(5, 18, 0, 16, 16);
		for (int l = 0; l < 16; l++)
		{
			ptr14[l] = (byte)_currLifeSkillAttainmentPanelPlanIndex[l];
		}
		ptr14 += 16;
		foreach (KeyValuePair<short, SkillBreakPlateObsolete> item in _skillBreakPlateObsoleteDict)
		{
			short key4 = item.Key;
			SkillBreakPlateObsolete value4 = item.Value;
			if (value4 != null)
			{
				int serializedSize = value4.GetSerializedSize();
				byte* ptr15 = OperationAdder.DynamicSingleValueCollection_Add(5, 19, key4, serializedSize);
				ptr15 += value4.Serialize(ptr15);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(5, 19, key4, 0);
			}
		}
		foreach (KeyValuePair<short, SkillBreakBonusCollection> item2 in _skillBreakBonusDict)
		{
			short key5 = item2.Key;
			SkillBreakBonusCollection value5 = item2.Value;
			if (value5 != null)
			{
				int serializedSize2 = value5.GetSerializedSize();
				byte* ptr16 = OperationAdder.DynamicSingleValueCollection_Add(5, 20, key5, serializedSize2);
				ptr16 += value5.Serialize(ptr16);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(5, 20, key5, 0);
			}
		}
		foreach (KeyValuePair<int, GameData.Utilities.ShortList> item3 in _teachTaiwuLifeSkillDict)
		{
			int key6 = item3.Key;
			GameData.Utilities.ShortList value6 = item3.Value;
			int serializedSize3 = value6.GetSerializedSize();
			byte* ptr17 = OperationAdder.DynamicSingleValueCollection_Add(5, 21, key6, serializedSize3);
			ptr17 += value6.Serialize(ptr17);
		}
		foreach (KeyValuePair<int, GameData.Utilities.ShortList> item4 in _teachTaiwuCombatSkillDict)
		{
			int key7 = item4.Key;
			GameData.Utilities.ShortList value7 = item4.Value;
			int serializedSize4 = value7.GetSerializedSize();
			byte* ptr18 = OperationAdder.DynamicSingleValueCollection_Add(5, 22, key7, serializedSize4);
			ptr18 += value7.Serialize(ptr18);
		}
		byte* ptr19 = OperationAdder.FixedSingleValue_Set(5, 23, 1260);
		for (int m = 0; m < 630; m++)
		{
			((short*)ptr19)[m] = _combatSkillAttainmentPanelPlans[m];
		}
		ptr19 += 1260;
		byte* ptr20 = OperationAdder.FixedSingleValue_Set(5, 24, 14);
		for (int n = 0; n < 14; n++)
		{
			ptr20[n] = (byte)_currCombatSkillAttainmentPanelPlanIds[n];
		}
		ptr20 += 14;
		byte* ptr21 = OperationAdder.FixedSingleValue_Set(5, 26, 6);
		for (int num4 = 0; num4 < 6; num4++)
		{
			ptr21[num4] = (byte)_weaponInnerRatios[num4];
		}
		ptr21 += 6;
		foreach (KeyValuePair<int, short> appointment in _appointments)
		{
			int key8 = appointment.Key;
			short value8 = appointment.Value;
			byte* ptr22 = OperationAdder.FixedSingleValueCollection_Add(5, 28, key8, 2);
			*(short*)ptr22 = value8;
			ptr22 += 2;
		}
		byte* ptr23 = OperationAdder.FixedSingleValue_Set(5, 29, 2);
		*(short*)ptr23 = _babyBonusMainAttributes;
		ptr23 += 2;
		byte* ptr24 = OperationAdder.FixedSingleValue_Set(5, 30, 2);
		*(short*)ptr24 = _babyBonusLifeSkillQualifications;
		ptr24 += 2;
		byte* ptr25 = OperationAdder.FixedSingleValue_Set(5, 31, 2);
		*(short*)ptr25 = _babyBonusCombatSkillQualifications;
		ptr25 += 2;
		byte* ptr26 = OperationAdder.FixedElementList_InsertRange(5, 32, 0, 5, 495);
		for (int num5 = 0; num5 < 5; num5++)
		{
			ptr26 += _equipmentsPlans[num5].Serialize(ptr26);
		}
		byte* ptr27 = OperationAdder.FixedSingleValue_Set(5, 33, 4);
		*(int*)ptr27 = _currEquipmentPlanId;
		ptr27 += 4;
		int serializedSize5 = _groupCharIds.GetSerializedSize();
		byte* ptr28 = OperationAdder.DynamicSingleValue_Set(5, 34, serializedSize5);
		ptr28 += _groupCharIds.Serialize(ptr28);
		byte* ptr29 = OperationAdder.FixedElementList_InsertRange(5, 35, 0, 3, 12);
		for (int num6 = 0; num6 < 3; num6++)
		{
			((int*)ptr29)[num6] = _combatGroupCharIds[num6];
		}
		ptr29 += 12;
		foreach (KeyValuePair<short, short> item5 in _legacyPointDict)
		{
			short key9 = item5.Key;
			short value9 = item5.Value;
			byte* ptr30 = OperationAdder.FixedSingleValueCollection_Add(5, 37, key9, 2);
			*(short*)ptr30 = value9;
			ptr30 += 2;
		}
		byte* ptr31 = OperationAdder.FixedSingleValue_Set(5, 38, 4);
		*(int*)ptr31 = _legacyPoint;
		ptr31 += 4;
		int count2 = _availableLegacyList.Count;
		int num7 = 2 * count2;
		int valueSize2 = 2 + num7;
		byte* ptr32 = OperationAdder.DynamicSingleValue_Set(5, 39, valueSize2);
		*(ushort*)ptr32 = (ushort)count2;
		ptr32 += 2;
		for (int num8 = 0; num8 < count2; num8++)
		{
			((short*)ptr32)[num8] = _availableLegacyList[num8];
		}
		ptr32 += num7;
		int num9 = 0;
		for (int num10 = 0; num10 < 15; num10++)
		{
			num9 += 4 + _stateNewCharacterLegacyGrowingGrades[num10].GetSerializedSize();
		}
		byte* ptr33 = OperationAdder.DynamicElementList_InsertRange(5, 42, 0, 15, num9);
		for (int num11 = 0; num11 < 15; num11++)
		{
			byte* ptr34 = ptr33;
			ptr33 += 4;
			int num12 = _stateNewCharacterLegacyGrowingGrades[num11].Serialize(ptr33);
			ptr33 += num12;
			*(int*)ptr34 = num12;
		}
		foreach (KeyValuePair<short, TaiwuCombatSkill> item6 in _notLearnCombatSkillReadingProgress)
		{
			short key10 = item6.Key;
			TaiwuCombatSkill value10 = item6.Value;
			byte* ptr35 = OperationAdder.FixedSingleValueCollection_Add(5, 43, key10, 20);
			ptr35 += value10.Serialize(ptr35);
		}
		foreach (KeyValuePair<short, TaiwuLifeSkill> item7 in _notLearnLifeSkillReadingProgress)
		{
			short key11 = item7.Key;
			TaiwuLifeSkill value11 = item7.Value;
			byte* ptr36 = OperationAdder.FixedSingleValueCollection_Add(5, 44, key11, 5);
			ptr36 += value11.Serialize(ptr36);
		}
		foreach (KeyValuePair<ItemKey, ReadingBookStrategies> readingBook in _readingBooks)
		{
			ItemKey key12 = readingBook.Key;
			ReadingBookStrategies value12 = readingBook.Value;
			byte* ptr37 = OperationAdder.FixedSingleValueCollection_Add(5, 45, key12, 36);
			ptr37 += value12.Serialize(ptr37);
		}
		byte* ptr38 = OperationAdder.FixedSingleValue_Set(5, 46, 8);
		ptr38 += _curReadingBook.Serialize(ptr38);
		byte* ptr39 = OperationAdder.FixedSingleValue_Set(5, 47, 24);
		for (int num13 = 0; num13 < 3; num13++)
		{
			ptr39 += _referenceBooks[num13].Serialize(ptr39);
		}
		byte* ptr40 = OperationAdder.FixedSingleValue_Set(5, 49, 1);
		*ptr40 = (_readingEventTriggered ? ((byte)1) : ((byte)0));
		ptr40++;
		byte* ptr41 = OperationAdder.FixedSingleValue_Set(5, 50, 1);
		*ptr41 = (byte)_readInCombatCount;
		ptr41++;
		int count3 = _visitedSettlements.Count;
		int num14 = 2 * count3;
		int valueSize3 = 2 + num14;
		byte* ptr42 = OperationAdder.DynamicSingleValue_Set(5, 54, valueSize3);
		*(ushort*)ptr42 = (ushort)count3;
		ptr42 += 2;
		for (int num15 = 0; num15 < count3; num15++)
		{
			((short*)ptr42)[num15] = _visitedSettlements[num15];
		}
		ptr42 += num14;
		byte* ptr43 = OperationAdder.FixedSingleValue_Set(5, 55, 2);
		*(short*)ptr43 = _taiwuVillageSettlementId;
		ptr43 += 2;
		foreach (KeyValuePair<int, VillagerWorkData> item8 in _villagerWork)
		{
			int key13 = item8.Key;
			VillagerWorkData value13 = item8.Value;
			byte* ptr44 = OperationAdder.FixedSingleValueCollection_Add(5, 56, key13, 20);
			ptr44 += value13.Serialize(ptr44);
		}
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 0));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 1));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 2));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(5, 3));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 4));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(5, 7));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 12));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 13));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(5, 14));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(5, 15));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(5, 16));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 17));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(5, 18));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(5, 19));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(5, 20));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(5, 21));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(5, 22));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 23));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 24));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 26));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(5, 28));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 29));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 30));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 31));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(5, 32));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 33));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(5, 34));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(5, 35));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(5, 37));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 38));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(5, 39));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(5, 42));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(5, 43));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(5, 44));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(5, 45));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 46));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 47));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 49));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 50));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(5, 54));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(5, 55));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(5, 56));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 0);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuCharId, dataPool);
		case 1:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuGenerationsCount, dataPool);
		case 2:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			return GameData.Serializer.Serializer.Serialize(_cricketLuckPoint, dataPool);
		case 3:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
			}
			return GameData.Serializer.Serializer.Serialize(_previousTaiwuIds, dataPool);
		case 4:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
			}
			return GameData.Serializer.Serializer.Serialize(_needToEscape, dataPool);
		case 5:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
			}
			return GameData.Serializer.Serializer.Serialize(_receivedItems, dataPool);
		case 6:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
			}
			return GameData.Serializer.Serializer.Serialize(_receivedCharacters, dataPool);
		case 7:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 8:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
			}
			return GameData.Serializer.Serializer.Serialize(GetWarehouseMaxLoad(), dataPool);
		case 9:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
			}
			return GameData.Serializer.Serializer.Serialize(GetWarehouseCurrLoad(), dataPool);
		case 10:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 10);
			}
			return GameData.Serializer.Serializer.Serialize(GetBuildingSpaceLimit(), dataPool);
		case 11:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
			}
			return GameData.Serializer.Serializer.Serialize(GetBuildingSpaceCurr(), dataPool);
		case 12:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 12);
			}
			return GameData.Serializer.Serializer.Serialize(_buildingSpaceExtraAdd, dataPool);
		case 13:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 13);
			}
			return GameData.Serializer.Serializer.Serialize(_prosperousConstruction, dataPool);
		case 14:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 14);
				_modificationsCombatSkills.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, TaiwuCombatSkill>)_combatSkills, dataPool);
		case 15:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
				_modificationsLifeSkills.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, TaiwuLifeSkill>)_lifeSkills, dataPool);
		case 16:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 17:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 17);
			}
			return GameData.Serializer.Serializer.Serialize(_currCombatSkillPlanId, dataPool);
		case 18:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesCurrLifeSkillAttainmentPanelPlanIndex, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_currLifeSkillAttainmentPanelPlanIndex[(uint)subId0], dataPool);
		case 19:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 19);
				_modificationsSkillBreakPlateObsoleteDict.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, SkillBreakPlateObsolete>)_skillBreakPlateObsoleteDict, dataPool);
		case 20:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 21:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
				_modificationsTeachTaiwuLifeSkillDict.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<int, GameData.Utilities.ShortList>)_teachTaiwuLifeSkillDict, dataPool);
		case 22:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
				_modificationsTeachTaiwuCombatSkillDict.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<int, GameData.Utilities.ShortList>)_teachTaiwuCombatSkillDict, dataPool);
		case 23:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 23);
			}
			return GameData.Serializer.Serializer.Serialize(_combatSkillAttainmentPanelPlans, dataPool);
		case 24:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 24);
			}
			return GameData.Serializer.Serializer.Serialize(_currCombatSkillAttainmentPanelPlanIds, dataPool);
		case 25:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 25);
			}
			return GameData.Serializer.Serializer.Serialize(GetMoveTimeCostPercent(), dataPool);
		case 26:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 26);
			}
			return GameData.Serializer.Serializer.Serialize(_weaponInnerRatios, dataPool);
		case 27:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 27);
			}
			return GameData.Serializer.Serializer.Serialize(GetWeaponCurrInnerRatios(), dataPool);
		case 28:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 28);
				_modificationsAppointments.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<int, short>)_appointments, dataPool);
		case 29:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 30:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 31:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 32:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 33:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 33);
			}
			return GameData.Serializer.Serializer.Serialize(_currEquipmentPlanId, dataPool);
		case 34:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 34);
			}
			return GameData.Serializer.Serializer.Serialize(_groupCharIds, dataPool);
		case 35:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesCombatGroupCharIds, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_combatGroupCharIds[(uint)subId0], dataPool);
		case 36:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 36);
			}
			return GameData.Serializer.Serializer.Serialize(GetTaiwuGroupMaxCount(), dataPool);
		case 37:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 37);
				_modificationsLegacyPointDict.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, short>)_legacyPointDict, dataPool);
		case 38:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 38);
			}
			return GameData.Serializer.Serializer.Serialize(_legacyPoint, dataPool);
		case 39:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 39);
			}
			return GameData.Serializer.Serializer.Serialize(_availableLegacyList, dataPool);
		case 40:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 40);
			}
			return GameData.Serializer.Serializer.Serialize(_legacyPassingState, dataPool);
		case 41:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 41);
			}
			return GameData.Serializer.Serializer.Serialize(_successorCandidates, dataPool);
		case 42:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesStateNewCharacterLegacyGrowingGrades, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_stateNewCharacterLegacyGrowingGrades[(uint)subId0], dataPool);
		case 43:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 43);
				_modificationsNotLearnCombatSkillReadingProgress.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, TaiwuCombatSkill>)_notLearnCombatSkillReadingProgress, dataPool);
		case 44:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 44);
				_modificationsNotLearnLifeSkillReadingProgress.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, TaiwuLifeSkill>)_notLearnLifeSkillReadingProgress, dataPool);
		case 45:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 46:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 46);
			}
			return GameData.Serializer.Serializer.Serialize(_curReadingBook, dataPool);
		case 47:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 47);
			}
			return GameData.Serializer.Serializer.Serialize(_referenceBooks, dataPool);
		case 48:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 48);
			}
			return GameData.Serializer.Serializer.Serialize(GetReferenceBookSlotUnlockStates(), dataPool);
		case 49:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 49);
			}
			return GameData.Serializer.Serializer.Serialize(_readingEventTriggered, dataPool);
		case 50:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 50);
			}
			return GameData.Serializer.Serializer.Serialize(_readInCombatCount, dataPool);
		case 51:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 51);
			}
			return GameData.Serializer.Serializer.Serialize(_healingOuterInjuryRestriction, dataPool);
		case 52:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 52);
			}
			return GameData.Serializer.Serializer.Serialize(_healingInnerInjuryRestriction, dataPool);
		case 53:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 53);
			}
			return GameData.Serializer.Serializer.Serialize(_neiliAllocationTypeRestriction, dataPool);
		case 54:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 54);
			}
			return GameData.Serializer.Serializer.Serialize(_visitedSettlements, dataPool);
		case 55:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 55);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuVillageSettlementId, dataPool);
		case 56:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 56);
				_modificationsVillagerWork.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<int, VillagerWorkData>)_villagerWork, dataPool);
		case 57:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 57);
				_modificationsVillagerWorkLocations.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<Location, VoidValue>)_villagerWorkLocations, dataPool);
		case 58:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 58);
			}
			return GameData.Serializer.Serializer.Serialize(GetMaterialResourceMaxCount(), dataPool);
		case 59:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 59);
			}
			return GameData.Serializer.Serializer.Serialize(GetResourceChange(), dataPool);
		case 60:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 60);
			}
			return GameData.Serializer.Serializer.Serialize(GetWorkLocationMaxCount(), dataPool);
		case 61:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 61);
			}
			return GameData.Serializer.Serializer.Serialize(GetTotalVillagerCount(), dataPool);
		case 62:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 62);
			}
			return GameData.Serializer.Serializer.Serialize(GetTotalAdultVillagerCount(), dataPool);
		case 63:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 63);
			}
			return GameData.Serializer.Serializer.Serialize(GetAvailableVillagerCount(), dataPool);
		case 64:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 64);
			}
			return GameData.Serializer.Serializer.Serialize(_isTaiwuDieOfCombatWithXiangshu, dataPool);
		case 65:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 65);
			}
			return GameData.Serializer.Serializer.Serialize(GetVillagerLearnLifeSkillsFromSect(), dataPool);
		case 66:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 66);
			}
			return GameData.Serializer.Serializer.Serialize(GetVillagerLearnCombatSkillsFromSect(), dataPool);
		case 67:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 67);
			}
			return GameData.Serializer.Serializer.Serialize(GetOverweightSanctionPercent(), dataPool);
		case 68:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 68);
			}
			return GameData.Serializer.Serializer.Serialize(GetReferenceSkillSlotUnlockStates(), dataPool);
		case 69:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 69);
			}
			return GameData.Serializer.Serializer.Serialize(GetTaiwuGroupWorstInjuries(), dataPool);
		case 70:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 70);
			}
			return GameData.Serializer.Serializer.Serialize(GetTotalResources(), dataPool);
		case 71:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 71);
			}
			return GameData.Serializer.Serializer.Serialize(GetTaiwuSpecialGroup(), dataPool);
		case 72:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 72);
			}
			return GameData.Serializer.Serializer.Serialize(GetTaiwuGearMateGroup(), dataPool);
		case 73:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 73);
			}
			return GameData.Serializer.Serializer.Serialize(GetCanBreakOut(), dataPool);
		case 74:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 74);
			}
			return GameData.Serializer.Serializer.Serialize(GetTroughMaxLoad(), dataPool);
		case 75:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 75);
			}
			return GameData.Serializer.Serializer.Serialize(GetTroughCurrLoad(), dataPool);
		case 76:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 76);
			}
			return GameData.Serializer.Serializer.Serialize(GetClothingDurability(), dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 1:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _taiwuGenerationsCount);
			SetTaiwuGenerationsCount(_taiwuGenerationsCount, context);
			break;
		case 2:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _cricketLuckPoint);
			SetCricketLuckPoint(_cricketLuckPoint, context);
			break;
		case 3:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _previousTaiwuIds);
			SetPreviousTaiwuIds(_previousTaiwuIds, context);
			break;
		case 4:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _needToEscape);
			SetNeedToEscape(_needToEscape, context);
			break;
		case 5:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _receivedItems);
			SetReceivedItems(_receivedItems, context);
			break;
		case 6:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _receivedCharacters);
			SetReceivedCharacters(_receivedCharacters, context);
			break;
		case 7:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 8:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 9:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 10:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 11:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 12:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _buildingSpaceExtraAdd);
			SetBuildingSpaceExtraAdd(_buildingSpaceExtraAdd, context);
			break;
		case 13:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _prosperousConstruction);
			SetProsperousConstruction(_prosperousConstruction, context);
			break;
		case 14:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 15:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 16:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 17:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 18:
		{
			sbyte item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			_currLifeSkillAttainmentPanelPlanIndex[(uint)subId0] = item3;
			SetElement_CurrLifeSkillAttainmentPanelPlanIndex((int)subId0, item3, context);
			break;
		}
		case 19:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 20:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 21:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 22:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 23:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 24:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 25:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 26:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _weaponInnerRatios);
			SetWeaponInnerRatios(_weaponInnerRatios, context);
			break;
		case 27:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 28:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 29:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 30:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 31:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 32:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 33:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 34:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 35:
		{
			int item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			_combatGroupCharIds[(uint)subId0] = item2;
			SetElement_CombatGroupCharIds((int)subId0, item2, context);
			break;
		}
		case 36:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 37:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 38:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _legacyPoint);
			SetLegacyPoint(_legacyPoint, context);
			break;
		case 39:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _availableLegacyList);
			SetAvailableLegacyList(_availableLegacyList, context);
			break;
		case 40:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 41:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 42:
		{
			SByteList item = default(SByteList);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			_stateNewCharacterLegacyGrowingGrades[(uint)subId0] = item;
			SetElement_StateNewCharacterLegacyGrowingGrades((int)subId0, item, context);
			break;
		}
		case 43:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 44:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 45:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 46:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 47:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 48:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 49:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _readingEventTriggered);
			SetReadingEventTriggered(_readingEventTriggered, context);
			break;
		case 50:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _readInCombatCount);
			SetReadInCombatCount(_readInCombatCount, context);
			break;
		case 51:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _healingOuterInjuryRestriction);
			SetHealingOuterInjuryRestriction(_healingOuterInjuryRestriction, context);
			break;
		case 52:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _healingInnerInjuryRestriction);
			SetHealingInnerInjuryRestriction(_healingInnerInjuryRestriction, context);
			break;
		case 53:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 54:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _visitedSettlements);
			SetVisitedSettlements(_visitedSettlements, context);
			break;
		case 55:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _taiwuVillageSettlementId);
			SetTaiwuVillageSettlementId(_taiwuVillageSettlementId, context);
			break;
		case 56:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 57:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 58:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 59:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 60:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 61:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 62:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 63:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 64:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _isTaiwuDieOfCombatWithXiangshu);
			SetIsTaiwuDieOfCombatWithXiangshu(_isTaiwuDieOfCombatWithXiangshu, context);
			break;
		case 65:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 66:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 67:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 68:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 69:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 70:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 71:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 72:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 73:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 74:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 75:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 76:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
	{
		int argsOffset = operation.ArgsOffset;
		switch (operation.MethodId)
		{
		case 0:
			if (operation.ArgsCount == 0)
			{
				List<SettlementDisplayData> allVisitedSettlements = GetAllVisitedSettlements();
				return GameData.Serializer.Serializer.Serialize(allVisitedSettlements, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 1:
		{
			int argsCount91 = operation.ArgsCount;
			int num91 = argsCount91;
			if (num91 == 4)
			{
				int item239 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item239);
				short item240 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item240);
				short item241 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item241);
				sbyte item242 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item242);
				bool item243 = SetVillagerCollectResourceWork(context, item239, item240, item241, item242);
				return GameData.Serializer.Serializer.Serialize(item243, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
		{
			int argsCount22 = operation.ArgsCount;
			int num22 = argsCount22;
			if (num22 == 3)
			{
				int item55 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item55);
				short item56 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item56);
				short item57 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item57);
				bool item58 = SetVillagerCollectTributeWork(context, item55, item56, item57);
				return GameData.Serializer.Serializer.Serialize(item58, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
		{
			int argsCount70 = operation.ArgsCount;
			int num70 = argsCount70;
			if (num70 == 4)
			{
				int item188 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item188);
				short item189 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item189);
				short item190 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item190);
				int item191 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item191);
				bool item192 = SetVillagerKeepGraveWork(context, item188, item189, item190, item191);
				return GameData.Serializer.Serializer.Serialize(item192, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 4:
		{
			int argsCount115 = operation.ArgsCount;
			int num115 = argsCount115;
			if (num115 == 3)
			{
				int item293 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item293);
				short item294 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item294);
				short item295 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item295);
				bool item296 = SetVillagerIdleWork(context, item293, item294, item295);
				return GameData.Serializer.Serializer.Serialize(item296, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
		{
			int argsCount59 = operation.ArgsCount;
			int num59 = argsCount59;
			if (num59 == 3)
			{
				short item160 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item160);
				short item161 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item161);
				sbyte item162 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item162);
				bool item163 = StopVillagerWork(context, item160, item161, item162);
				return GameData.Serializer.Serializer.Serialize(item163, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 6:
		{
			int argsCount119 = operation.ArgsCount;
			int num119 = argsCount119;
			if (num119 == 2)
			{
				short item307 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item307);
				short item308 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item308);
				bool item309 = StopVillagerCollectResourceWork(context, item307, item308);
				return GameData.Serializer.Serializer.Serialize(item309, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 7:
		{
			int argsCount88 = operation.ArgsCount;
			int num88 = argsCount88;
			if (num88 == 1)
			{
				List<Location> item232 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item232);
				List<VillagerWorkData> collectResourceWorkDataList = GetCollectResourceWorkDataList(item232);
				return GameData.Serializer.Serializer.Serialize(collectResourceWorkDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 8:
		{
			int argsCount23 = operation.ArgsCount;
			int num23 = argsCount23;
			if (num23 == 1)
			{
				int item59 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item59);
				ExpelVillager(context, item59);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 9:
		{
			int argsCount127 = operation.ArgsCount;
			int num127 = argsCount127;
			if (num127 == 1)
			{
				List<int> item323 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item323);
				List<VillagerStatusDisplayData> villagerStatusDisplayDataList = GetVillagerStatusDisplayDataList(item323);
				return GameData.Serializer.Serializer.Serialize(villagerStatusDisplayDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
			if (operation.ArgsCount == 0)
			{
				List<VillagerStatusDisplayData> allVillagersStatus = GetAllVillagersStatus();
				return GameData.Serializer.Serializer.Serialize(allVillagersStatus, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 11:
			switch (operation.ArgsCount)
			{
			case 0:
			{
				List<int> allVillagersAvailableForWork2 = GetAllVillagersAvailableForWork();
				return GameData.Serializer.Serializer.Serialize(allVillagersAvailableForWork2, returnDataPool);
			}
			case 1:
			{
				bool item275 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item275);
				List<int> allVillagersAvailableForWork = GetAllVillagersAvailableForWork(item275);
				return GameData.Serializer.Serializer.Serialize(allVillagersAvailableForWork, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 12:
			if (operation.ArgsCount == 0)
			{
				int[] item206 = CalcResourceChangeByVillageWork(context);
				return GameData.Serializer.Serializer.Serialize(item206, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 13:
			if (operation.ArgsCount == 0)
			{
				int[] item146 = CalcResourceChangeByBuildingEarn(context);
				return GameData.Serializer.Serializer.Serialize(item146, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 14:
			if (operation.ArgsCount == 0)
			{
				int[] item31 = CalcResourceChangeByBuildingMaintain(context);
				return GameData.Serializer.Serializer.Serialize(item31, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 15:
			if (operation.ArgsCount == 0)
			{
				List<ItemDisplayData> allWarehouseItems = GetAllWarehouseItems(context);
				return GameData.Serializer.Serializer.Serialize(allWarehouseItems, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 16:
		{
			int argsCount108 = operation.ArgsCount;
			int num108 = argsCount108;
			if (num108 == 1)
			{
				short item283 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item283);
				List<ItemDisplayData> warehouseItemsBySubType = GetWarehouseItemsBySubType(context, item283);
				return GameData.Serializer.Serializer.Serialize(warehouseItemsBySubType, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 17:
		{
			int argsCount100 = operation.ArgsCount;
			int num100 = argsCount100;
			if (num100 == 1)
			{
				int item267 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item267);
				SwitchEquipmentPlan(context, item267);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 18:
		{
			int argsCount81 = operation.ArgsCount;
			int num81 = argsCount81;
			if (num81 == 2)
			{
				sbyte item219 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item219);
				int item220 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item220);
				GmCmd_AddResource(context, item219, item220);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 19:
		{
			int argsCount66 = operation.ArgsCount;
			int num66 = argsCount66;
			if (num66 == 2)
			{
				short item182 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item182);
				int item183 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item183);
				GmCmd_AddLegacyPoint(context, item182, item183);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 20:
		{
			int argsCount43 = operation.ArgsCount;
			int num43 = argsCount43;
			if (num43 == 1)
			{
				int item120 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item120);
				GmCmd_AddExp(context, item120);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 21:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				short item112 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item112);
				ushort item113 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item113);
				GmCmd_SetTaiwuCombatSkillActiveState(context, item112, item113);
				return -1;
			}
			case 3:
			{
				short item109 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item109);
				ushort item110 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item110);
				bool item111 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item111);
				GmCmd_SetTaiwuCombatSkillActiveState(context, item109, item110, item111);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 22:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				int item97 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item97);
				JoinGroup(context, item97);
				return -1;
			}
			case 2:
			{
				int item95 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item95);
				bool item96 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item96);
				JoinGroup(context, item95, item96);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 23:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				int item90 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item90);
				LeaveGroup(context, item90);
				return -1;
			}
			case 2:
			{
				int item88 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item88);
				bool item89 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item89);
				LeaveGroup(context, item88, item89);
				return -1;
			}
			case 3:
			{
				int item85 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item85);
				bool item86 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item86);
				bool item87 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item87);
				LeaveGroup(context, item85, item86, item87);
				return -1;
			}
			case 4:
			{
				int item81 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item81);
				bool item82 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item82);
				bool item83 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item83);
				bool item84 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item84);
				LeaveGroup(context, item81, item82, item83, item84);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 24:
			if (operation.ArgsCount == 0)
			{
				CompletePassingLegacy(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 25:
		{
			int argsCount135 = operation.ArgsCount;
			int num135 = argsCount135;
			if (num135 == 1)
			{
				short item356 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item356);
				SelectLegacy(context, item356);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 26:
			if (operation.ArgsCount == 0)
			{
				List<int> item316 = FindSuccessorCandidates(context);
				return GameData.Serializer.Serializer.Serialize(item316, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 27:
		{
			int argsCount112 = operation.ArgsCount;
			int num112 = argsCount112;
			if (num112 == 1)
			{
				int item290 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item290);
				ConfirmChosenSuccessor(context, item290);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 28:
		{
			int argsCount105 = operation.ArgsCount;
			int num105 = argsCount105;
			if (num105 == 2)
			{
				sbyte item276 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item276);
				ItemKey item277 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item277);
				SetReferenceBook(context, item276, item277);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 29:
		{
			int argsCount97 = operation.ArgsCount;
			int num97 = argsCount97;
			if (num97 == 1)
			{
				ItemKey item258 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item258);
				SetReadingBook(context, item258);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 30:
			if (operation.ArgsCount == 0)
			{
				ReadingBookStrategies curReadingStrategies = GetCurReadingStrategies();
				return GameData.Serializer.Serializer.Serialize(curReadingStrategies, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 31:
		{
			int argsCount75 = operation.ArgsCount;
			int num75 = argsCount75;
			if (num75 == 3)
			{
				byte item203 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item203);
				int item204 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item204);
				sbyte item205 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item205);
				SetReadingStrategy(context, item203, item204, item205);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 32:
		{
			int argsCount60 = operation.ArgsCount;
			int num60 = argsCount60;
			if (num60 == 1)
			{
				byte item164 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item164);
				ClearPageStrategy(context, item164);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 33:
		{
			int argsCount51 = operation.ArgsCount;
			int num51 = argsCount51;
			if (num51 == 1)
			{
				byte item141 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item141);
				List<byte> randomSelectableStrategies = GetRandomSelectableStrategies(context, item141);
				return GameData.Serializer.Serializer.Serialize(randomSelectableStrategies, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 34:
			if (operation.ArgsCount == 0)
			{
				CheckNotInInventoryBooks(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 35:
		{
			int argsCount30 = operation.ArgsCount;
			int num30 = argsCount30;
			if (num30 == 1)
			{
				int item77 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item77);
				sbyte totalReadingProgress = GetTotalReadingProgress(item77);
				return GameData.Serializer.Serializer.Serialize(totalReadingProgress, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 36:
			if (operation.ArgsCount == 0)
			{
				short currReadingEventBonusRate = GetCurrReadingEventBonusRate();
				return GameData.Serializer.Serializer.Serialize(currReadingEventBonusRate, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 37:
			if (operation.ArgsCount == 0)
			{
				short currReadingEfficiency = GetCurrReadingEfficiency(context);
				return GameData.Serializer.Serializer.Serialize(currReadingEfficiency, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 38:
		{
			int argsCount131 = operation.ArgsCount;
			int num131 = argsCount131;
			if (num131 == 2)
			{
				ItemKey item347 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item347);
				int item348 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item348);
				WarehouseAdd(context, item347, item348);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 39:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				ItemKey item344 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item344);
				int item345 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item345);
				WarehouseRemove(context, item344, item345);
				return -1;
			}
			case 3:
			{
				ItemKey item341 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item341);
				int item342 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item342);
				bool item343 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item343);
				WarehouseRemove(context, item341, item342, item343);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 40:
		{
			int argsCount124 = operation.ArgsCount;
			int num124 = argsCount124;
			if (num124 == 2)
			{
				ItemKey item319 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item319);
				int item320 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item320);
				PutItemIntoWarehouse(context, item319, item320);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 41:
		{
			int argsCount118 = operation.ArgsCount;
			int num118 = argsCount118;
			if (num118 == 2)
			{
				ItemKey item305 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item305);
				int item306 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item306);
				TakeOutItemFromWarehouse(context, item305, item306);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 42:
			if (operation.ArgsCount == 0)
			{
				bool item284 = CanTransferItemToWarehouse(context);
				return GameData.Serializer.Serializer.Serialize(item284, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 43:
		{
			int argsCount106 = operation.ArgsCount;
			int num106 = argsCount106;
			if (num106 == 1)
			{
				BuildingBlockKey item279 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item279);
				int item280 = CalcBuildingResourceOutput(item279);
				return GameData.Serializer.Serializer.Serialize(item280, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 44:
		{
			int argsCount102 = operation.ArgsCount;
			int num102 = argsCount102;
			if (num102 == 2)
			{
				bool item269 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item269);
				List<ItemKey> item270 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item270);
				bool item271 = TransferAllItems(context, item269, item270);
				return GameData.Serializer.Serializer.Serialize(item271, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 45:
		{
			int argsCount95 = operation.ArgsCount;
			int num95 = argsCount95;
			if (num95 == 2)
			{
				sbyte item252 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item252);
				sbyte item253 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item253);
				SelectCombatSkillAttainmentPanelPlan(context, item252, item253);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 46:
			if (operation.ArgsCount == 0)
			{
				byte[] genericGridAllocation = GetGenericGridAllocation();
				return GameData.Serializer.Serializer.Serialize(genericGridAllocation, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 47:
		{
			int argsCount78 = operation.ArgsCount;
			int num78 = argsCount78;
			if (num78 == 1)
			{
				sbyte item215 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item215);
				AllocateGenericGrid(context, item215);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 48:
		{
			int argsCount72 = operation.ArgsCount;
			int num72 = argsCount72;
			if (num72 == 1)
			{
				sbyte item195 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item195);
				DeallocateGenericGrid(context, item195);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 49:
		{
			int argsCount63 = operation.ArgsCount;
			int num63 = argsCount63;
			if (num63 == 1)
			{
				int item179 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item179);
				UpdateCombatSkillPlan(context, item179);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 50:
		{
			int argsCount56 = operation.ArgsCount;
			int num56 = argsCount56;
			if (num56 == 1)
			{
				short item152 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item152);
				SkillBreakPlate breakPlateData = GetBreakPlateData(item152);
				return GameData.Serializer.Serializer.Serialize(breakPlateData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 51:
		{
			int argsCount49 = operation.ArgsCount;
			int num49 = argsCount49;
			if (num49 == 2)
			{
				short item134 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item134);
				ushort item135 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item135);
				SkillBreakPlate item136 = EnterSkillBreakPlate(context, item134, item135);
				return GameData.Serializer.Serializer.Serialize(item136, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 52:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				short item126 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item126);
				ClearBreakPlate(context, item126);
				return -1;
			}
			case 2:
			{
				short item124 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item124);
				bool item125 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item125);
				ClearBreakPlate(context, item124, item125);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 53:
		{
			int argsCount40 = operation.ArgsCount;
			int num40 = argsCount40;
			if (num40 == 2)
			{
				short item106 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item106);
				SkillBreakPlateIndex item107 = default(SkillBreakPlateIndex);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item107);
				SkillBreakPlate item108 = SelectSkillBreakGrid(context, item106, item107);
				return GameData.Serializer.Serializer.Serialize(item108, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 54:
			if (operation.ArgsCount == 0)
			{
				EscapeToAdjacentBlock(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 55:
		{
			int argsCount27 = operation.ArgsCount;
			int num27 = argsCount27;
			if (num27 == 1)
			{
				short item67 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item67);
				List<ItemDisplayData> canOperateItemDisplayDataInVillage = GetCanOperateItemDisplayDataInVillage(context, item67);
				return GameData.Serializer.Serializer.Serialize(canOperateItemDisplayDataInVillage, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 56:
		{
			int argsCount17 = operation.ArgsCount;
			int num17 = argsCount17;
			if (num17 == 1)
			{
				List<ItemKey> item47 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item47);
				PutItemListIntoWarehouse(context, item47);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 57:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 1)
			{
				List<ItemKey> item39 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item39);
				WarehouseAddList(context, item39);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 58:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 1)
			{
				List<ItemKey> item13 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item13);
				TakeOutItemListFromWarehouse(context, item13);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 59:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				List<ItemKey> item9 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				WarehouseRemoveList(context, item9);
				return -1;
			}
			case 2:
			{
				List<ItemKey> item7 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				bool item8 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				WarehouseRemoveList(context, item7, item8);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 60:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				ItemKey item3 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				WarehouseDiscardItem(context, item3);
				return -1;
			}
			case 2:
			{
				ItemKey item = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				int item2 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				WarehouseDiscardItem(context, item, item2);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 61:
		{
			int argsCount134 = operation.ArgsCount;
			int num134 = argsCount134;
			if (num134 == 1)
			{
				List<ItemKey> item355 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item355);
				WarehouseDiscardItemList(context, item355);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 62:
			if (operation.ArgsCount == 0)
			{
				List<ItemDisplayData> taiwuAllItems = GetTaiwuAllItems(context);
				return GameData.Serializer.Serializer.Serialize(taiwuAllItems, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 63:
			switch (operation.ArgsCount)
			{
			case 4:
			{
				sbyte item336 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item336);
				sbyte item337 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item337);
				ItemKey item338 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item338);
				int item339 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item339);
				bool item340 = TransferItem(context, item336, item337, item338, item339);
				return GameData.Serializer.Serializer.Serialize(item340, returnDataPool);
			}
			case 5:
			{
				sbyte item330 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item330);
				sbyte item331 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item331);
				ItemKey item332 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item332);
				int item333 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item333);
				bool item334 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item334);
				bool item335 = TransferItem(context, item330, item331, item332, item333, item334);
				return GameData.Serializer.Serializer.Serialize(item335, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 64:
			if (operation.ArgsCount == 0)
			{
				List<ItemDisplayData> allTroughItems = GetAllTroughItems(context);
				return GameData.Serializer.Serializer.Serialize(allTroughItems, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 65:
		{
			int argsCount121 = operation.ArgsCount;
			int num121 = argsCount121;
			if (num121 == 3)
			{
				sbyte item311 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item311);
				sbyte item312 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item312);
				List<ItemKey> item313 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item313);
				bool item314 = TransferItemList(context, item311, item312, item313);
				return GameData.Serializer.Serializer.Serialize(item314, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 66:
			if (operation.ArgsCount == 0)
			{
				List<ItemDisplayData> allTreasuryItems = GetAllTreasuryItems(context);
				return GameData.Serializer.Serializer.Serialize(allTreasuryItems, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 67:
		{
			int argsCount111 = operation.ArgsCount;
			int num111 = argsCount111;
			if (num111 == 1)
			{
				List<int> item289 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item289);
				List<sbyte> totalReadingProgressList = GetTotalReadingProgressList(item289);
				return GameData.Serializer.Serializer.Serialize(totalReadingProgressList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 68:
			if (operation.ArgsCount == 0)
			{
				int[] item282 = CalcResourceChangeByAutoExpand(context);
				return GameData.Serializer.Serializer.Serialize(item282, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 69:
			if (operation.ArgsCount == 0)
			{
				int item278 = CalcAutoExpandNotSatisfyIndex(context);
				return GameData.Serializer.Serializer.Serialize(item278, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 70:
		{
			int argsCount103 = operation.ArgsCount;
			int num103 = argsCount103;
			if (num103 == 1)
			{
				Location item273 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item273);
				(int, int) relationCharacterCountOnBlock = GetRelationCharacterCountOnBlock(item273);
				return GameData.Serializer.Serializer.Serialize(relationCharacterCountOnBlock, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 71:
		{
			int argsCount99 = operation.ArgsCount;
			int num99 = argsCount99;
			if (num99 == 2)
			{
				int item264 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item264);
				bool item265 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item265);
				CombatResultDisplayData item266 = ApplyLifeSkillCombatResult(context, item264, item265);
				return GameData.Serializer.Serializer.Serialize(item266, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 72:
		{
			int argsCount92 = operation.ArgsCount;
			int num92 = argsCount92;
			if (num92 == 1)
			{
				int item244 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item244);
				ItemDisplayData item245 = PickLifeSkillCombatCharacterUseItem(context, item244);
				return GameData.Serializer.Serializer.Serialize(item245, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 73:
		{
			int argsCount86 = operation.ArgsCount;
			int num86 = argsCount86;
			if (num86 == 1)
			{
				int item229 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item229);
				SecretInformationDisplayPackage item230 = PickLifeSkillCombatCharacterUseSecretInformation(context, item229);
				return GameData.Serializer.Serializer.Serialize(item230, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 74:
		{
			int argsCount83 = operation.ArgsCount;
			int num83 = argsCount83;
			if (num83 == 1)
			{
				Location item224 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item224);
				int consumedCountOnBlock = GetConsumedCountOnBlock(item224);
				return GameData.Serializer.Serializer.Serialize(consumedCountOnBlock, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 75:
		{
			int argsCount77 = operation.ArgsCount;
			int num77 = argsCount77;
			if (num77 == 1)
			{
				ItemKey item214 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item214);
				int refBonusSpeed = GetRefBonusSpeed(item214);
				return GameData.Serializer.Serializer.Serialize(refBonusSpeed, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 76:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				short item210 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item210);
				bool item211 = FindTaiwuBuilding(item210);
				return GameData.Serializer.Serializer.Serialize(item211, returnDataPool);
			}
			case 2:
			{
				short item207 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item207);
				bool item208 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item208);
				bool item209 = FindTaiwuBuilding(item207, item208);
				return GameData.Serializer.Serializer.Serialize(item209, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 77:
		{
			int argsCount74 = operation.ArgsCount;
			int num74 = argsCount74;
			if (num74 == 2)
			{
				sbyte item200 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item200);
				int item201 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item201);
				List<ItemDisplayData> item202 = ChoosyGetMaterial(context, item200, item201);
				return GameData.Serializer.Serializer.Serialize(item202, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 78:
		{
			int argsCount67 = operation.ArgsCount;
			int num67 = argsCount67;
			if (num67 == 1)
			{
				short item184 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item184);
				List<ItemDisplayData> cannotOperateItemDisplayDataInInventory = GetCannotOperateItemDisplayDataInInventory(context, item184);
				return GameData.Serializer.Serializer.Serialize(cannotOperateItemDisplayDataInInventory, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 79:
			if (operation.ArgsCount == 0)
			{
				List<CharacterDisplayData> inventoryOverloadedGroupCharNames = GetInventoryOverloadedGroupCharNames();
				return GameData.Serializer.Serializer.Serialize(inventoryOverloadedGroupCharNames, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 80:
		{
			int argsCount58 = operation.ArgsCount;
			int num58 = argsCount58;
			if (num58 == 1)
			{
				bool item158 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item158);
				SetAutoAllocateNeiliToMax(item158);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 81:
		{
			int argsCount54 = operation.ArgsCount;
			int num54 = argsCount54;
			if (num54 == 1)
			{
				byte[] item144 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item144);
				List<sbyte> item145 = LifeSkillCombatCurrentPlayerGetNotUsableEffectCardTemplateIds(context, item144);
				return GameData.Serializer.Serializer.Serialize(item145, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 82:
		{
			int argsCount46 = operation.ArgsCount;
			int num46 = argsCount46;
			if (num46 == 4)
			{
				sbyte item127 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item127);
				int item128 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item128);
				int item129 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item129);
				sbyte item130 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item130);
				byte[] item131 = LifeSkillCombatStart(context, item127, item128, item129, item130);
				return GameData.Serializer.Serializer.Serialize(item131, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 83:
			if (operation.ArgsCount == 0)
			{
				CombatResultDisplayData item117 = LifeSkillCombatTerminate(context);
				return GameData.Serializer.Serializer.Serialize(item117, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 84:
		{
			int argsCount37 = operation.ArgsCount;
			int num37 = argsCount37;
			if (num37 == 1)
			{
				byte[] item101 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item101);
				byte[] item102 = LifeSkillCombatCurrentPlayerCommitOperation(context, item101);
				return GameData.Serializer.Serializer.Serialize(item102, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 85:
		{
			int argsCount34 = operation.ArgsCount;
			int num34 = argsCount34;
			if (num34 == 1)
			{
				byte[] item93 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item93);
				byte[] item94 = LifeSkillCombatCurrentPlayerGetUsableOperationList(context, item93);
				return GameData.Serializer.Serializer.Serialize(item94, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 86:
			if (operation.ArgsCount == 0)
			{
				byte[] item78 = LifeSkillCombatCurrentPlayerSimulateCancelOperation(context);
				return GameData.Serializer.Serializer.Serialize(item78, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 87:
		{
			int argsCount26 = operation.ArgsCount;
			int num26 = argsCount26;
			if (num26 == 1)
			{
				byte[] item65 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item65);
				byte[] item66 = LifeSkillCombatCurrentPlayerGetSecondPhaseUsableOperationList(context, item65);
				return GameData.Serializer.Serializer.Serialize(item66, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 88:
		{
			int argsCount19 = operation.ArgsCount;
			int num19 = argsCount19;
			if (num19 == 1)
			{
				sbyte item49 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item49);
				byte[] item50 = LifeSkillCombatCurrentPlayerCalcUsableFirstPhaseEffectCardInfo(context, item49);
				return GameData.Serializer.Serializer.Serialize(item50, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 89:
		{
			int argsCount15 = operation.ArgsCount;
			int num15 = argsCount15;
			if (num15 == 1)
			{
				byte[] item41 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item41);
				byte[] item42 = LifeSkillCombatCurrentPlayerSimulateCommitOperation(context, item41);
				return GameData.Serializer.Serializer.Serialize(item42, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 90:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 1)
			{
				byte[] item32 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item32);
				byte[] item33 = LifeSkillCombatCurrentPlayerAiPickOperation(context, item32);
				return GameData.Serializer.Serializer.Serialize(item33, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 91:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 1)
			{
				byte[] item22 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				byte[] item23 = LifeSkillCombatCurrentPlayerCommitOperationPreview(context, item22);
				return GameData.Serializer.Serializer.Serialize(item23, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 92:
			if (operation.ArgsCount == 0)
			{
				byte[] item4 = LifeSkillCombatCurrentPlayerAcceptForceSilent(context);
				return GameData.Serializer.Serializer.Serialize(item4, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 93:
		{
			int argsCount138 = operation.ArgsCount;
			int num138 = argsCount138;
			if (num138 == 1)
			{
				short item367 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item367);
				bool item368 = MasteredSkillWillChangePlan(item367);
				return GameData.Serializer.Serializer.Serialize(item368, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 94:
			switch (operation.ArgsCount)
			{
			case 0:
			{
				List<int> villagersForWork3 = GetVillagersForWork();
				return GameData.Serializer.Serializer.Serialize(villagersForWork3, returnDataPool);
			}
			case 1:
			{
				bool item361 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item361);
				List<int> villagersForWork2 = GetVillagersForWork(item361);
				return GameData.Serializer.Serializer.Serialize(villagersForWork2, returnDataPool);
			}
			case 2:
			{
				bool item359 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item359);
				bool item360 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item360);
				List<int> villagersForWork = GetVillagersForWork(item359, item360);
				return GameData.Serializer.Serializer.Serialize(villagersForWork, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 95:
			switch (operation.ArgsCount)
			{
			case 0:
			{
				List<NameRelatedData> severelyInjuredGroupCharNames2 = GetSeverelyInjuredGroupCharNames();
				return GameData.Serializer.Serializer.Serialize(severelyInjuredGroupCharNames2, returnDataPool);
			}
			case 1:
			{
				bool item358 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item358);
				List<NameRelatedData> severelyInjuredGroupCharNames = GetSeverelyInjuredGroupCharNames(item358);
				return GameData.Serializer.Serializer.Serialize(severelyInjuredGroupCharNames, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 96:
		{
			int argsCount132 = operation.ArgsCount;
			int num132 = argsCount132;
			if (num132 == 2)
			{
				sbyte item349 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item349);
				short item350 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item350);
				int itemCount = GetItemCount(item349, item350);
				return GameData.Serializer.Serializer.Serialize(itemCount, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 97:
			if (operation.ArgsCount == 0)
			{
				MapBlockData taiwuVillagerMapBlockData = GetTaiwuVillagerMapBlockData();
				return GameData.Serializer.Serializer.Serialize(taiwuVillagerMapBlockData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 98:
		{
			int argsCount128 = operation.ArgsCount;
			int num128 = argsCount128;
			if (num128 == 4)
			{
				short item324 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item324);
				short item325 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item325);
				sbyte item326 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item326);
				bool item327 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item327);
				bool item328 = StopVillagerWorkOptional(context, item324, item325, item326, item327);
				return GameData.Serializer.Serializer.Serialize(item328, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 99:
		{
			int argsCount126 = operation.ArgsCount;
			int num126 = argsCount126;
			if (num126 == 1)
			{
				short item322 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item322);
				int legacyMaxPointByType = GetLegacyMaxPointByType(item322);
				return GameData.Serializer.Serializer.Serialize(legacyMaxPointByType, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 100:
			if (operation.ArgsCount == 0)
			{
				bool currReadingBanByWug = GetCurrReadingBanByWug();
				return GameData.Serializer.Serializer.Serialize(currReadingBanByWug, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 101:
		{
			int argsCount120 = operation.ArgsCount;
			int num120 = argsCount120;
			if (num120 == 1)
			{
				Location item310 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item310);
				int pastLifeRelationCharacterCountOnBlock = GetPastLifeRelationCharacterCountOnBlock(item310);
				return GameData.Serializer.Serializer.Serialize(pastLifeRelationCharacterCountOnBlock, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 102:
			if (operation.ArgsCount == 0)
			{
				GmCmd_MarkAllCarrierFullTamePoint(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 103:
		{
			int argsCount114 = operation.ArgsCount;
			int num114 = argsCount114;
			if (num114 == 1)
			{
				Location item292 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item292);
				int selectMapBlockHasMerchantId = GetSelectMapBlockHasMerchantId(context, item292);
				return GameData.Serializer.Serializer.Serialize(selectMapBlockHasMerchantId, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 104:
		{
			int argsCount109 = operation.ArgsCount;
			int num109 = argsCount109;
			if (num109 == 2)
			{
				sbyte item285 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item285);
				bool item286 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item286);
				GmCmd_LifeSkillCombatAiSetAlwaysUseForceAdversary(context, item285, item286);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 105:
			if (operation.ArgsCount == 0)
			{
				ActiveReadOnce(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 106:
			if (operation.ArgsCount == 0)
			{
				ActiveNeigongLoopingOnce(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 107:
			if (operation.ArgsCount == 0)
			{
				AppendCombatSkillPlan(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 108:
			if (operation.ArgsCount == 0)
			{
				CopyCombatSkillPlan(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 109:
			if (operation.ArgsCount == 0)
			{
				ClearCombatSkillPlan(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 110:
			if (operation.ArgsCount == 0)
			{
				DeleteCombatSkillPlan(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 111:
		{
			int argsCount94 = operation.ArgsCount;
			int num94 = argsCount94;
			if (num94 == 1)
			{
				short item251 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item251);
				List<IntList> legacyMaxPointAndTimesListByType = GetLegacyMaxPointAndTimesListByType(item251);
				return GameData.Serializer.Serializer.Serialize(legacyMaxPointAndTimesListByType, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 112:
		{
			int argsCount89 = operation.ArgsCount;
			int num89 = argsCount89;
			if (num89 == 2)
			{
				int item233 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item233);
				sbyte item234 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item234);
				SetQiArtStrategy(context, item233, item234);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 113:
			if (operation.ArgsCount == 0)
			{
				List<sbyte> currentBookAvailableReadingStrategies = GetCurrentBookAvailableReadingStrategies(context);
				return GameData.Serializer.Serializer.Serialize(currentBookAvailableReadingStrategies, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 114:
			if (operation.ArgsCount == 0)
			{
				SByteList loopingNeigongQiArtStrategies = GetLoopingNeigongQiArtStrategies(context);
				return GameData.Serializer.Serializer.Serialize(loopingNeigongQiArtStrategies, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 115:
		{
			int argsCount80 = operation.ArgsCount;
			int num80 = argsCount80;
			if (num80 == 2)
			{
				int item217 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item217);
				short item218 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item218);
				SetReferenceCombatSkillAt(context, item217, item218);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 116:
			if (operation.ArgsCount == 0)
			{
				List<QiArtStrategyDisplayData> loopingNeigongQiArtStrategyDisplayDatas = GetLoopingNeigongQiArtStrategyDisplayDatas(context);
				return GameData.Serializer.Serializer.Serialize(loopingNeigongQiArtStrategyDisplayDatas, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 117:
			if (operation.ArgsCount == 0)
			{
				SByteList loopingNeigongAvailableQiArtStrategies = GetLoopingNeigongAvailableQiArtStrategies(context);
				return GameData.Serializer.Serializer.Serialize(loopingNeigongAvailableQiArtStrategies, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 118:
			if (operation.ArgsCount == 0)
			{
				ClearCurrentLoopingNeigongEvent(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 119:
		{
			int argsCount69 = operation.ArgsCount;
			int num69 = argsCount69;
			if (num69 == 1)
			{
				short item187 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item187);
				SetTaiwuLoopingNeigong(context, item187);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 120:
		{
			int argsCount64 = operation.ArgsCount;
			int num64 = argsCount64;
			if (num64 == 1)
			{
				short item180 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item180);
				DeleteTaiwuFeature(context, item180);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 121:
			if (operation.ArgsCount == 0)
			{
				GmCmd_TaiwuActiveLoopingApply(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 122:
			if (operation.ArgsCount == 0)
			{
				bool isFollowingNpcListMax = GetIsFollowingNpcListMax();
				return GameData.Serializer.Serializer.Serialize(isFollowingNpcListMax, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 123:
			if (operation.ArgsCount == 0)
			{
				int followingNpcListMaxCount = GetFollowingNpcListMaxCount();
				return GameData.Serializer.Serializer.Serialize(followingNpcListMaxCount, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 124:
		{
			int argsCount52 = operation.ArgsCount;
			int num52 = argsCount52;
			if (num52 == 1)
			{
				int item142 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item142);
				GmCmd_FollowRandomNpc(context, item142);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 125:
		{
			int argsCount48 = operation.ArgsCount;
			int num48 = argsCount48;
			if (num48 == 1)
			{
				int item133 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item133);
				TaiwuFollowNpc(context, item133);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 126:
		{
			int argsCount44 = operation.ArgsCount;
			int num44 = argsCount44;
			if (num44 == 1)
			{
				int item121 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item121);
				TaiwuUnfollowNpc(context, item121);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 127:
		{
			int argsCount41 = operation.ArgsCount;
			int num41 = argsCount41;
			if (num41 == 2)
			{
				int item114 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item114);
				string item115 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item115);
				int item116 = SetFollowingNpcNickName(context, item114, item115);
				return GameData.Serializer.Serializer.Serialize(item116, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 128:
		{
			int argsCount38 = operation.ArgsCount;
			int num38 = argsCount38;
			if (num38 == 1)
			{
				int item103 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item103);
				string followingNpcNickName = GetFollowingNpcNickName(item103);
				return GameData.Serializer.Serializer.Serialize(followingNpcNickName, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 129:
		{
			int argsCount35 = operation.ArgsCount;
			int num35 = argsCount35;
			if (num35 == 1)
			{
				int item99 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item99);
				int followingNpcNickNameId = GetFollowingNpcNickNameId(item99);
				return GameData.Serializer.Serializer.Serialize(followingNpcNickNameId, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 130:
		{
			int argsCount32 = operation.ArgsCount;
			int num32 = argsCount32;
			if (num32 == 1)
			{
				List<int> item91 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item91);
				List<VillagerRoleCharacterDisplayData> villagerRoleCharacterDisplayDataList = GetVillagerRoleCharacterDisplayDataList(item91);
				return GameData.Serializer.Serializer.Serialize(villagerRoleCharacterDisplayDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 131:
		{
			int argsCount31 = operation.ArgsCount;
			int num31 = argsCount31;
			if (num31 == 1)
			{
				int item80 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item80);
				VillagerRoleCharacterDisplayData villagerRoleCharacterDisplayData = GetVillagerRoleCharacterDisplayData(item80);
				return GameData.Serializer.Serializer.Serialize(villagerRoleCharacterDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 132:
		{
			int argsCount28 = operation.ArgsCount;
			int num28 = argsCount28;
			if (num28 == 2)
			{
				List<int> item68 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item68);
				short item69 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item69);
				bool item70 = BatchSetVillagerRole(context, item68, item69);
				return GameData.Serializer.Serializer.Serialize(item70, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 133:
		{
			int argsCount25 = operation.ArgsCount;
			int num25 = argsCount25;
			if (num25 == 3)
			{
				int item61 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item61);
				short item62 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item62);
				Location item63 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item63);
				bool item64 = DispatchVillagerArrangement(context, item61, item62, item63);
				return GameData.Serializer.Serializer.Serialize(item64, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 134:
		{
			int argsCount20 = operation.ArgsCount;
			int num20 = argsCount20;
			if (num20 == 1)
			{
				int item51 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item51);
				bool item52 = RecallVillager(context, item51);
				return GameData.Serializer.Serializer.Serialize(item52, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 135:
		{
			int argsCount16 = operation.ArgsCount;
			int num16 = argsCount16;
			if (num16 == 2)
			{
				int item44 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item44);
				TemplateKey item45 = default(TemplateKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item45);
				bool item46 = AssignTargetItem(context, item44, item45);
				return GameData.Serializer.Serializer.Serialize(item46, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 136:
		{
			int argsCount14 = operation.ArgsCount;
			int num14 = argsCount14;
			if (num14 == 1)
			{
				short item40 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item40);
				VillagerRoleManageDisplayData villagerRoleDisplayData = GetVillagerRoleDisplayData(item40);
				return GameData.Serializer.Serializer.Serialize(villagerRoleDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 137:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 2)
			{
				int item36 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item36);
				bool item37 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item37);
				bool item38 = AssignArrangementIncreaseOrDecrease(context, item36, item37);
				return GameData.Serializer.Serializer.Serialize(item38, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 138:
			if (operation.ArgsCount == 0)
			{
				List<VillagerRoleManageDisplayData> allVillagerRoleDisplayData = GetAllVillagerRoleDisplayData();
				return GameData.Serializer.Serializer.Serialize(allVillagerRoleDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 139:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				ItemSourceType item27 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item27);
				(ItemSourceType, List<ItemDisplayData>) allItems2 = GetAllItems(item27);
				return GameData.Serializer.Serializer.Serialize(allItems2, returnDataPool);
			}
			case 2:
			{
				ItemSourceType item25 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item25);
				bool item26 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				(ItemSourceType, List<ItemDisplayData>) allItems = GetAllItems(item25, item26);
				return GameData.Serializer.Serializer.Serialize(allItems, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 140:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 4)
			{
				ItemSourceType item14 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item14);
				ItemSourceType item15 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item15);
				sbyte item16 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item16);
				int item17 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				bool item18 = TransferResource(context, item14, item15, item16, item17);
				return GameData.Serializer.Serializer.Serialize(item18, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 141:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 1)
			{
				short item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				VillagerRoleTipsDisplayData villagerRoleTipsDisplayData = GetVillagerRoleTipsDisplayData(context, item6);
				return GameData.Serializer.Serializer.Serialize(villagerRoleTipsDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 142:
		{
			int argsCount139 = operation.ArgsCount;
			int num139 = argsCount139;
			if (num139 == 2)
			{
				int item369 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item369);
				short item370 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item370);
				bool item371 = SetVillagerRole(context, item369, item370);
				return GameData.Serializer.Serializer.Serialize(item371, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 143:
		{
			int argsCount137 = operation.ArgsCount;
			int num137 = argsCount137;
			if (num137 == 4)
			{
				int item362 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item362);
				short item363 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item363);
				short item364 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item364);
				sbyte item365 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item365);
				bool item366 = SetVillagerMigrateWork(context, item362, item363, item364, item365);
				return GameData.Serializer.Serializer.Serialize(item366, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 144:
		{
			int argsCount136 = operation.ArgsCount;
			int num136 = argsCount136;
			if (num136 == 1)
			{
				short item357 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item357);
				string villagerRoleNpcNickName = GetVillagerRoleNpcNickName(item357);
				return GameData.Serializer.Serializer.Serialize(villagerRoleNpcNickName, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 145:
		{
			int argsCount133 = operation.ArgsCount;
			int num133 = argsCount133;
			if (num133 == 2)
			{
				short item352 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item352);
				string item353 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item353);
				int item354 = SetVillagerRoleNickName(context, item352, item353);
				return GameData.Serializer.Serializer.Serialize(item354, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 146:
			switch (operation.ArgsCount)
			{
			case 0:
			{
				List<int> villagersAvailableForVillagerRole2 = GetVillagersAvailableForVillagerRole();
				return GameData.Serializer.Serializer.Serialize(villagersAvailableForVillagerRole2, returnDataPool);
			}
			case 1:
			{
				bool item351 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item351);
				List<int> villagersAvailableForVillagerRole = GetVillagersAvailableForVillagerRole(item351);
				return GameData.Serializer.Serializer.Serialize(villagersAvailableForVillagerRole, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 147:
		{
			int argsCount130 = operation.ArgsCount;
			int num130 = argsCount130;
			if (num130 == 1)
			{
				ItemSourceType item346 = ItemSourceType.Equipment;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item346);
				(ItemSourceType, ResourceInts) allResources = GetAllResources(item346);
				return GameData.Serializer.Serializer.Serialize(allResources, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 148:
			if (operation.ArgsCount == 0)
			{
				List<DispatchSwordTombDisplayData> allSwordTombDisplayDataForDispatch = GetAllSwordTombDisplayDataForDispatch();
				return GameData.Serializer.Serializer.Serialize(allSwordTombDisplayDataForDispatch, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 149:
		{
			int argsCount129 = operation.ArgsCount;
			int num129 = argsCount129;
			if (num129 == 1)
			{
				int item329 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item329);
				VillagerRoleCharacterSlimDisplayData villagerRoleCharacterSlimDisplayData = GetVillagerRoleCharacterSlimDisplayData(item329);
				return GameData.Serializer.Serializer.Serialize(villagerRoleCharacterSlimDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 150:
			if (operation.ArgsCount == 0)
			{
				List<ItemDisplayData> allWarehouseItemsExcludeValueZero = GetAllWarehouseItemsExcludeValueZero(context);
				return GameData.Serializer.Serializer.Serialize(allWarehouseItemsExcludeValueZero, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 151:
		{
			int argsCount125 = operation.ArgsCount;
			int num125 = argsCount125;
			if (num125 == 1)
			{
				short item321 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item321);
				GmCmd_FillLegacyPoint(context, item321);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 152:
		{
			int argsCount123 = operation.ArgsCount;
			int num123 = argsCount123;
			if (num123 == 2)
			{
				TaiwuVillageStorageType item317 = TaiwuVillageStorageType.Treasury;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item317);
				VillagerRoleActionSetting item318 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item318);
				SetVillagerRoleActionSetting(context, item317, item318);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 153:
		{
			int argsCount122 = operation.ArgsCount;
			int num122 = argsCount122;
			if (num122 == 1)
			{
				TaiwuVillageStorageType item315 = TaiwuVillageStorageType.Treasury;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item315);
				VillagerRoleActionSetting villagerRoleActionSetting = GetVillagerRoleActionSetting(context, item315);
				return GameData.Serializer.Serializer.Serialize(villagerRoleActionSetting, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 154:
			if (operation.ArgsCount == 0)
			{
				Dictionary<sbyte, VillagerRoleActionSetting> allVillagerRoleActionSetting = GetAllVillagerRoleActionSetting(context);
				return GameData.Serializer.Serializer.SerializeDefault(allVillagerRoleActionSetting, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 155:
		{
			int argsCount117 = operation.ArgsCount;
			int num117 = argsCount117;
			if (num117 == 2)
			{
				short item303 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item303);
				sbyte item304 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item304);
				List<sbyte> villagerRoleExecuteFixedActionFailReasons = GetVillagerRoleExecuteFixedActionFailReasons(context, item303, item304);
				return GameData.Serializer.Serializer.Serialize(villagerRoleExecuteFixedActionFailReasons, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 156:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				int item301 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item301);
				sbyte item302 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item302);
				SetMerchantType(context, item301, item302);
				return -1;
			}
			case 3:
			{
				int item298 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item298);
				sbyte item299 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item299);
				bool item300 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item300);
				SetMerchantType(context, item298, item299, item300);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 157:
		{
			int argsCount116 = operation.ArgsCount;
			int num116 = argsCount116;
			if (num116 == 1)
			{
				int item297 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item297);
				sbyte merchantType = GetMerchantType(item297);
				return GameData.Serializer.Serializer.Serialize(merchantType, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 158:
		{
			int argsCount113 = operation.ArgsCount;
			int num113 = argsCount113;
			if (num113 == 1)
			{
				int item291 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item291);
				ProfessionTipDisplayData professionTipDisplayData = GetProfessionTipDisplayData(item291);
				return GameData.Serializer.Serializer.Serialize(professionTipDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 159:
		{
			int argsCount110 = operation.ArgsCount;
			int num110 = argsCount110;
			if (num110 == 2)
			{
				bool item287 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item287);
				int item288 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item288);
				int expByRereading = GetExpByRereading(item287, item288);
				return GameData.Serializer.Serializer.Serialize(expByRereading, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 160:
			if (operation.ArgsCount == 0)
			{
				EnterMerchant();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 161:
			if (operation.ArgsCount == 0)
			{
				int[] readingResult = GetReadingResult();
				return GameData.Serializer.Serializer.Serialize(readingResult, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 162:
		{
			int argsCount107 = operation.ArgsCount;
			int num107 = argsCount107;
			if (num107 == 1)
			{
				int item281 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item281);
				VillagerTreasuryNeed villagerTreasuryNeed = GetVillagerTreasuryNeed(item281);
				return GameData.Serializer.Serializer.Serialize(villagerTreasuryNeed, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 163:
			if (operation.ArgsCount == 0)
			{
				Dictionary<int, sbyte> villagerClassesDict = GetVillagerClassesDict();
				return GameData.Serializer.Serializer.SerializeDefault(villagerClassesDict, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 164:
			if (operation.ArgsCount == 0)
			{
				List<int>[] villagerListClassArray = GetVillagerListClassArray();
				return GameData.Serializer.Serializer.SerializeDefault(villagerListClassArray, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 165:
		{
			int argsCount104 = operation.ArgsCount;
			int num104 = argsCount104;
			if (num104 == 1)
			{
				ItemKey item274 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item274);
				Dictionary<int, sbyte> treasuryItemNeededCharDict = GetTreasuryItemNeededCharDict(item274);
				return GameData.Serializer.Serializer.SerializeDefault(treasuryItemNeededCharDict, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 166:
			if (operation.ArgsCount == 0)
			{
				List<ItemKey> treasuryNeededItemList = GetTreasuryNeededItemList();
				return GameData.Serializer.Serializer.Serialize(treasuryNeededItemList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 167:
			switch (operation.ArgsCount)
			{
			case 0:
			{
				List<NameRelatedData> dyingGroupCharNames2 = GetDyingGroupCharNames();
				return GameData.Serializer.Serializer.Serialize(dyingGroupCharNames2, returnDataPool);
			}
			case 1:
			{
				bool item272 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item272);
				List<NameRelatedData> dyingGroupCharNames = GetDyingGroupCharNames(item272);
				return GameData.Serializer.Serializer.Serialize(dyingGroupCharNames, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 168:
		{
			int argsCount101 = operation.ArgsCount;
			int num101 = argsCount101;
			if (num101 == 1)
			{
				short item268 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item268);
				int breakBaseCostExp = GetBreakBaseCostExp(item268);
				return GameData.Serializer.Serializer.Serialize(breakBaseCostExp, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 169:
		{
			int argsCount98 = operation.ArgsCount;
			int num98 = argsCount98;
			if (num98 == 4)
			{
				short item259 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item259);
				SkillBreakPlateIndex item260 = default(SkillBreakPlateIndex);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item260);
				int item261 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item261);
				ushort item262 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item262);
				SkillBreakPlate item263 = SetBonusRelation(context, item259, item260, item261, item262);
				return GameData.Serializer.Serializer.Serialize(item263, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 170:
		{
			int argsCount96 = operation.ArgsCount;
			int num96 = argsCount96;
			if (num96 == 3)
			{
				short item254 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item254);
				SkillBreakPlateIndex item255 = default(SkillBreakPlateIndex);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item255);
				int item256 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item256);
				SkillBreakPlate item257 = SetBonusExp(context, item254, item255, item256);
				return GameData.Serializer.Serializer.Serialize(item257, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 171:
		{
			int argsCount93 = operation.ArgsCount;
			int num93 = argsCount93;
			if (num93 == 4)
			{
				short item246 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item246);
				SkillBreakPlateIndex item247 = default(SkillBreakPlateIndex);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item247);
				ItemKey item248 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item248);
				sbyte item249 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item249);
				SkillBreakPlate item250 = SetBonusItem(context, item246, item247, item248, item249);
				return GameData.Serializer.Serializer.Serialize(item250, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 172:
		{
			int argsCount90 = operation.ArgsCount;
			int num90 = argsCount90;
			if (num90 == 3)
			{
				short item235 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item235);
				byte item236 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item236);
				sbyte item237 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item237);
				bool item238 = SetActivePage(context, item235, item236, item237);
				return GameData.Serializer.Serializer.Serialize(item238, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 173:
		{
			int argsCount87 = operation.ArgsCount;
			int num87 = argsCount87;
			if (num87 == 1)
			{
				short item231 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item231);
				List<SkillBreakPlateBonus> availableRelationBonuses = GetAvailableRelationBonuses(item231);
				return GameData.Serializer.Serializer.Serialize(availableRelationBonuses, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 174:
		{
			int argsCount85 = operation.ArgsCount;
			int num85 = argsCount85;
			if (num85 == 1)
			{
				int item228 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item228);
				sbyte villagerCollectStorageType = GetVillagerCollectStorageType(context, item228);
				return GameData.Serializer.Serializer.Serialize(villagerCollectStorageType, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 175:
		{
			int argsCount84 = operation.ArgsCount;
			int num84 = argsCount84;
			if (num84 == 2)
			{
				int item225 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item225);
				sbyte item226 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item226);
				bool item227 = SetVillagerCollectStorageType(context, item225, item226);
				return GameData.Serializer.Serializer.Serialize(item227, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 176:
		{
			int argsCount82 = operation.ArgsCount;
			int num82 = argsCount82;
			if (num82 == 2)
			{
				short item221 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item221);
				SkillBreakPlateIndex item222 = default(SkillBreakPlateIndex);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item222);
				SkillBreakPlate item223 = ClearBonus(context, item221, item222);
				return GameData.Serializer.Serializer.Serialize(item223, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 177:
		{
			int argsCount79 = operation.ArgsCount;
			int num79 = argsCount79;
			if (num79 == 1)
			{
				short item216 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item216);
				TaiwuAddFeature(context, item216);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 178:
		{
			int argsCount76 = operation.ArgsCount;
			int num76 = argsCount76;
			if (num76 == 2)
			{
				sbyte item212 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item212);
				int item213 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item213);
				List<short> randomLegaciesInGroup = GetRandomLegaciesInGroup(context, item212, item213);
				return GameData.Serializer.Serializer.Serialize(randomLegaciesInGroup, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 179:
			if (operation.ArgsCount == 0)
			{
				int groupBabyCount = GetGroupBabyCount();
				return GameData.Serializer.Serializer.Serialize(groupBabyCount, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 180:
			if (operation.ArgsCount == 0)
			{
				int strategyRoomLevel = GetStrategyRoomLevel();
				return GameData.Serializer.Serializer.Serialize(strategyRoomLevel, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 181:
		{
			int argsCount73 = operation.ArgsCount;
			int num73 = argsCount73;
			if (num73 == 3)
			{
				short item196 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item196);
				SkillBreakPlateIndex item197 = default(SkillBreakPlateIndex);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item197);
				int item198 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item198);
				SkillBreakPlate item199 = SetBonusFriend(context, item196, item197, item198);
				return GameData.Serializer.Serializer.Serialize(item199, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 182:
		{
			int argsCount71 = operation.ArgsCount;
			int num71 = argsCount71;
			if (num71 == 2)
			{
				short item193 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item193);
				short item194 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item194);
				GmCmd_ShowUnlockedDebateStrategy(item193, item194);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 183:
		{
			int argsCount68 = operation.ArgsCount;
			int num68 = argsCount68;
			if (num68 == 2)
			{
				bool item185 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item185);
				int item186 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item186);
				GmCmd_ChangeGamePoint(item185, item186);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 184:
		{
			int argsCount65 = operation.ArgsCount;
			int num65 = argsCount65;
			if (num65 == 1)
			{
				bool item181 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item181);
				GmCmd_SetForceAiBribery(item181);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 185:
		{
			int argsCount62 = operation.ArgsCount;
			int num62 = argsCount62;
			if (num62 == 2)
			{
				bool item176 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item176);
				bool item177 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item177);
				DebateResult item178 = DebateGameOver(context, item176, item177);
				return GameData.Serializer.Serializer.Serialize(item178, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 186:
		{
			int argsCount61 = operation.ArgsCount;
			int num61 = argsCount61;
			if (num61 == 1)
			{
				bool item174 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item174);
				DebateGame item175 = DebateGameSetTaiwuAi(context, item174);
				return GameData.Serializer.Serializer.Serialize(item175, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 187:
			switch (operation.ArgsCount)
			{
			case 3:
			{
				IntPair item170 = default(IntPair);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item170);
				bool item171 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item171);
				sbyte item172 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item172);
				DebateGame item173 = DebateGameMakeMove(context, item170, item171, item172);
				return GameData.Serializer.Serializer.Serialize(item173, returnDataPool);
			}
			case 4:
			{
				IntPair item165 = default(IntPair);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item165);
				bool item166 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item166);
				sbyte item167 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item167);
				bool item168 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item168);
				DebateGame item169 = DebateGameMakeMove(context, item165, item166, item167, item168);
				return GameData.Serializer.Serializer.Serialize(item169, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 188:
			if (operation.ArgsCount == 0)
			{
				DebateGame item159 = DebateGameNextState(context);
				return GameData.Serializer.Serializer.Serialize(item159, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 189:
		{
			int argsCount57 = operation.ArgsCount;
			int num57 = argsCount57;
			if (num57 == 4)
			{
				sbyte item153 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item153);
				int item154 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item154);
				bool item155 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item155);
				List<int> item156 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item156);
				List<int> item157 = DebateGamePickSpectators(context, item153, item154, item155, item156);
				return GameData.Serializer.Serializer.Serialize(item157, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 190:
		{
			int argsCount55 = operation.ArgsCount;
			int num55 = argsCount55;
			if (num55 == 4)
			{
				sbyte item147 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item147);
				bool item148 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item148);
				int item149 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item149);
				List<int> item150 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item150);
				DebateGame item151 = DebateGameInitialize(context, item147, item148, item149, item150);
				return GameData.Serializer.Serializer.Serialize(item151, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 191:
		{
			int argsCount53 = operation.ArgsCount;
			int num53 = argsCount53;
			if (num53 == 1)
			{
				int item143 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item143);
				OperationForceAdversary aiBriberyDataOnPrepareLifeSkillCombat = GetAiBriberyDataOnPrepareLifeSkillCombat(context, item143);
				return GameData.Serializer.Serializer.Serialize(aiBriberyDataOnPrepareLifeSkillCombat, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 192:
		{
			int argsCount50 = operation.ArgsCount;
			int num50 = argsCount50;
			if (num50 == 3)
			{
				int item137 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item137);
				bool item138 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item138);
				List<StrategyTarget> item139 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item139);
				DebateGame item140 = DebateGameCastStrategy(context, item137, item138, item139);
				return GameData.Serializer.Serializer.Serialize(item140, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 193:
		{
			int argsCount47 = operation.ArgsCount;
			int num47 = argsCount47;
			if (num47 == 1)
			{
				int item132 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item132);
				GmCmd_GetDebateStrategyCard(item132);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 194:
		{
			int argsCount45 = operation.ArgsCount;
			int num45 = argsCount45;
			if (num45 == 2)
			{
				bool item122 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item122);
				int item123 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item123);
				GmCmd_ChangeStrategyPoint(item122, item123);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 195:
		{
			int argsCount42 = operation.ArgsCount;
			int num42 = argsCount42;
			if (num42 == 2)
			{
				bool item118 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item118);
				int item119 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item119);
				GmCmd_ChangeBases(item118, item119);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 196:
			if (operation.ArgsCount == 0)
			{
				List<short> newUnlockedDebateStrategyList = GetNewUnlockedDebateStrategyList();
				return GameData.Serializer.Serializer.Serialize(newUnlockedDebateStrategyList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 197:
		{
			int argsCount39 = operation.ArgsCount;
			int num39 = argsCount39;
			if (num39 == 2)
			{
				bool item104 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item104);
				int item105 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item105);
				GmCmd_ChangePressure(item104, item105);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 198:
		{
			int argsCount36 = operation.ArgsCount;
			int num36 = argsCount36;
			if (num36 == 1)
			{
				List<sbyte> item100 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item100);
				DebateGameSetTaiwuSelectedCardTypes(context, item100);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 199:
			if (operation.ArgsCount == 0)
			{
				List<sbyte> item98 = DebateGameGetTaiwuSelectedCardTypes();
				return GameData.Serializer.Serializer.Serialize(item98, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 200:
		{
			int argsCount33 = operation.ArgsCount;
			int num33 = argsCount33;
			if (num33 == 1)
			{
				int item92 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item92);
				GmCmd_AddAiOwnedCard(item92);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 201:
			if (operation.ArgsCount == 0)
			{
				GmCmd_EmptyAiOwnedCard();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 202:
			if (operation.ArgsCount == 0)
			{
				bool item79 = DebateGameTryForceWin(context);
				return GameData.Serializer.Serializer.Serialize(item79, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 203:
		{
			int argsCount29 = operation.ArgsCount;
			int num29 = argsCount29;
			if (num29 == 5)
			{
				int item71 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item71);
				short item72 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item72);
				short item73 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item73);
				sbyte item74 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item74);
				short item75 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item75);
				bool item76 = SetVillagerDevelopWork(context, item71, item72, item73, item74, item75);
				return GameData.Serializer.Serializer.Serialize(item76, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 204:
			if (operation.ArgsCount == 0)
			{
				List<VillagerRoleCharacterDisplayData> villagerRoleCharacterDisplayDataOnPanel = GetVillagerRoleCharacterDisplayDataOnPanel(context);
				return GameData.Serializer.Serializer.Serialize(villagerRoleCharacterDisplayDataOnPanel, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 205:
		{
			int argsCount24 = operation.ArgsCount;
			int num24 = argsCount24;
			if (num24 == 1)
			{
				int item60 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item60);
				int isTaiwuFirstByLuck = GetIsTaiwuFirstByLuck(context, item60);
				return GameData.Serializer.Serializer.Serialize(isTaiwuFirstByLuck, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 206:
		{
			int argsCount21 = operation.ArgsCount;
			int num21 = argsCount21;
			if (num21 == 2)
			{
				short item53 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item53);
				int item54 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item54);
				GmCmd_AddNodeEffect(context, item53, item54);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 207:
		{
			int argsCount18 = operation.ArgsCount;
			int num18 = argsCount18;
			if (num18 == 1)
			{
				int item48 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item48);
				(int, int, int) villagerFarmerMigrateResourceSuccessRateBonus = GetVillagerFarmerMigrateResourceSuccessRateBonus(item48);
				return GameData.Serializer.Serializer.Serialize(villagerFarmerMigrateResourceSuccessRateBonus, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 208:
			if (operation.ArgsCount == 0)
			{
				int villagerRoleHeadTotalAuthorityCost = GetVillagerRoleHeadTotalAuthorityCost();
				return GameData.Serializer.Serializer.Serialize(villagerRoleHeadTotalAuthorityCost, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 209:
			switch (operation.ArgsCount)
			{
			case 0:
			{
				List<int> allChildAvailableForWork2 = GetAllChildAvailableForWork();
				return GameData.Serializer.Serializer.Serialize(allChildAvailableForWork2, returnDataPool);
			}
			case 1:
			{
				bool item43 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item43);
				List<int> allChildAvailableForWork = GetAllChildAvailableForWork(item43);
				return GameData.Serializer.Serializer.Serialize(allChildAvailableForWork, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 210:
			if (operation.ArgsCount == 0)
			{
				(int, int, int, int) taiwuVillageSpaceLimitInfo = GetTaiwuVillageSpaceLimitInfo();
				return GameData.Serializer.Serializer.Serialize(taiwuVillageSpaceLimitInfo, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 211:
			if (operation.ArgsCount == 0)
			{
				List<CharacterDisplayData> groupNeiliConflictingCharDataList = GetGroupNeiliConflictingCharDataList();
				return GameData.Serializer.Serializer.Serialize(groupNeiliConflictingCharDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 212:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 1)
			{
				bool item34 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item34);
				DebateGame item35 = DebateGameResetCards(item34);
				return GameData.Serializer.Serializer.Serialize(item35, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 213:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 2)
			{
				bool item28 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item28);
				List<int> item29 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item29);
				DebateGame item30 = DebateGameRemoveCards(item28, item29);
				return GameData.Serializer.Serializer.Serialize(item30, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 214:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 1)
			{
				int item24 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item24);
				SetLastCricketPlan(context, item24);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 215:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 2)
			{
				int item19 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item19);
				CricketCombatConfig item20 = default(CricketCombatConfig);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				List<ItemKey> item21 = RequestValidCricketPlan(item19, item20);
				return GameData.Serializer.Serializer.Serialize(item21, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 216:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 3)
			{
				int item10 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				ItemKey item11 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				int item12 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				SetCricketPlan(context, item10, item11, item12);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 217:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 1)
			{
				int item5 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				ClearCricketPlan(context, item5);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 218:
			if (operation.ArgsCount == 0)
			{
				int lastCricketPlan = GetLastCricketPlan();
				return GameData.Serializer.Serializer.Serialize(lastCricketPlan, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
	{
		switch (dataId)
		{
		case 0:
			break;
		case 1:
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
			break;
		case 9:
			break;
		case 10:
			break;
		case 11:
			break;
		case 12:
			break;
		case 13:
			break;
		case 14:
			_modificationsCombatSkills.ChangeRecording(monitoring);
			break;
		case 15:
			_modificationsLifeSkills.ChangeRecording(monitoring);
			break;
		case 16:
			break;
		case 17:
			break;
		case 18:
			break;
		case 19:
			_modificationsSkillBreakPlateObsoleteDict.ChangeRecording(monitoring);
			break;
		case 20:
			break;
		case 21:
			_modificationsTeachTaiwuLifeSkillDict.ChangeRecording(monitoring);
			break;
		case 22:
			_modificationsTeachTaiwuCombatSkillDict.ChangeRecording(monitoring);
			break;
		case 23:
			break;
		case 24:
			break;
		case 25:
			break;
		case 26:
			break;
		case 27:
			break;
		case 28:
			_modificationsAppointments.ChangeRecording(monitoring);
			break;
		case 29:
			break;
		case 30:
			break;
		case 31:
			break;
		case 32:
			break;
		case 33:
			break;
		case 34:
			break;
		case 35:
			break;
		case 36:
			break;
		case 37:
			_modificationsLegacyPointDict.ChangeRecording(monitoring);
			break;
		case 38:
			break;
		case 39:
			break;
		case 40:
			break;
		case 41:
			break;
		case 42:
			break;
		case 43:
			_modificationsNotLearnCombatSkillReadingProgress.ChangeRecording(monitoring);
			break;
		case 44:
			_modificationsNotLearnLifeSkillReadingProgress.ChangeRecording(monitoring);
			break;
		case 45:
			break;
		case 46:
			break;
		case 47:
			break;
		case 48:
			break;
		case 49:
			break;
		case 50:
			break;
		case 51:
			break;
		case 52:
			break;
		case 53:
			break;
		case 54:
			break;
		case 55:
			break;
		case 56:
			_modificationsVillagerWork.ChangeRecording(monitoring);
			break;
		case 57:
			_modificationsVillagerWorkLocations.ChangeRecording(monitoring);
			break;
		case 58:
			break;
		case 59:
			break;
		case 60:
			break;
		case 61:
			break;
		case 62:
			break;
		case 63:
			break;
		case 64:
			break;
		case 65:
			break;
		case 66:
			break;
		case 67:
			break;
		case 68:
			break;
		case 69:
			break;
		case 70:
			break;
		case 71:
			break;
		case 72:
			break;
		case 73:
			break;
		case 74:
			break;
		case 75:
			break;
		case 76:
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		switch (dataId)
		{
		case 0:
			if (!BaseGameDataDomain.IsModified(DataStates, 0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 0);
			return GameData.Serializer.Serializer.Serialize(_taiwuCharId, dataPool);
		case 1:
			if (!BaseGameDataDomain.IsModified(DataStates, 1))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 1);
			return GameData.Serializer.Serializer.Serialize(_taiwuGenerationsCount, dataPool);
		case 2:
			if (!BaseGameDataDomain.IsModified(DataStates, 2))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 2);
			return GameData.Serializer.Serializer.Serialize(_cricketLuckPoint, dataPool);
		case 3:
			if (!BaseGameDataDomain.IsModified(DataStates, 3))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 3);
			return GameData.Serializer.Serializer.Serialize(_previousTaiwuIds, dataPool);
		case 4:
			if (!BaseGameDataDomain.IsModified(DataStates, 4))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 4);
			return GameData.Serializer.Serializer.Serialize(_needToEscape, dataPool);
		case 5:
			if (!BaseGameDataDomain.IsModified(DataStates, 5))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 5);
			return GameData.Serializer.Serializer.Serialize(_receivedItems, dataPool);
		case 6:
			if (!BaseGameDataDomain.IsModified(DataStates, 6))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 6);
			return GameData.Serializer.Serializer.Serialize(_receivedCharacters, dataPool);
		case 7:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 8:
			if (!BaseGameDataDomain.IsModified(DataStates, 8))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 8);
			return GameData.Serializer.Serializer.Serialize(GetWarehouseMaxLoad(), dataPool);
		case 9:
			if (!BaseGameDataDomain.IsModified(DataStates, 9))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 9);
			return GameData.Serializer.Serializer.Serialize(GetWarehouseCurrLoad(), dataPool);
		case 10:
			if (!BaseGameDataDomain.IsModified(DataStates, 10))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 10);
			return GameData.Serializer.Serializer.Serialize(GetBuildingSpaceLimit(), dataPool);
		case 11:
			if (!BaseGameDataDomain.IsModified(DataStates, 11))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 11);
			return GameData.Serializer.Serializer.Serialize(GetBuildingSpaceCurr(), dataPool);
		case 12:
			if (!BaseGameDataDomain.IsModified(DataStates, 12))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 12);
			return GameData.Serializer.Serializer.Serialize(_buildingSpaceExtraAdd, dataPool);
		case 13:
			if (!BaseGameDataDomain.IsModified(DataStates, 13))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 13);
			return GameData.Serializer.Serializer.Serialize(_prosperousConstruction, dataPool);
		case 14:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 14))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 14);
			int result5 = GameData.Serializer.Serializer.SerializeModifications(_combatSkills, dataPool, _modificationsCombatSkills);
			_modificationsCombatSkills.Reset();
			return result5;
		}
		case 15:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 15))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 15);
			int result4 = GameData.Serializer.Serializer.SerializeModifications(_lifeSkills, dataPool, _modificationsLifeSkills);
			_modificationsLifeSkills.Reset();
			return result4;
		}
		case 16:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 17:
			if (!BaseGameDataDomain.IsModified(DataStates, 17))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 17);
			return GameData.Serializer.Serializer.Serialize(_currCombatSkillPlanId, dataPool);
		case 18:
			if (!BaseGameDataDomain.IsModified(_dataStatesCurrLifeSkillAttainmentPanelPlanIndex, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesCurrLifeSkillAttainmentPanelPlanIndex, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_currLifeSkillAttainmentPanelPlanIndex[(uint)subId0], dataPool);
		case 19:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 19))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 19);
			int result11 = GameData.Serializer.Serializer.SerializeModifications(_skillBreakPlateObsoleteDict, dataPool, _modificationsSkillBreakPlateObsoleteDict);
			_modificationsSkillBreakPlateObsoleteDict.Reset();
			return result11;
		}
		case 20:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 21:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 21))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 21);
			int result10 = GameData.Serializer.Serializer.SerializeModifications(_teachTaiwuLifeSkillDict, dataPool, _modificationsTeachTaiwuLifeSkillDict);
			_modificationsTeachTaiwuLifeSkillDict.Reset();
			return result10;
		}
		case 22:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 22))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 22);
			int result6 = GameData.Serializer.Serializer.SerializeModifications(_teachTaiwuCombatSkillDict, dataPool, _modificationsTeachTaiwuCombatSkillDict);
			_modificationsTeachTaiwuCombatSkillDict.Reset();
			return result6;
		}
		case 23:
			if (!BaseGameDataDomain.IsModified(DataStates, 23))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 23);
			return GameData.Serializer.Serializer.Serialize(_combatSkillAttainmentPanelPlans, dataPool);
		case 24:
			if (!BaseGameDataDomain.IsModified(DataStates, 24))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 24);
			return GameData.Serializer.Serializer.Serialize(_currCombatSkillAttainmentPanelPlanIds, dataPool);
		case 25:
			if (!BaseGameDataDomain.IsModified(DataStates, 25))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 25);
			return GameData.Serializer.Serializer.Serialize(GetMoveTimeCostPercent(), dataPool);
		case 26:
			if (!BaseGameDataDomain.IsModified(DataStates, 26))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 26);
			return GameData.Serializer.Serializer.Serialize(_weaponInnerRatios, dataPool);
		case 27:
			if (!BaseGameDataDomain.IsModified(DataStates, 27))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 27);
			return GameData.Serializer.Serializer.Serialize(GetWeaponCurrInnerRatios(), dataPool);
		case 28:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 28))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 28);
			int result = GameData.Serializer.Serializer.SerializeModifications(_appointments, dataPool, _modificationsAppointments);
			_modificationsAppointments.Reset();
			return result;
		}
		case 29:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 30:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 31:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 32:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 33:
			if (!BaseGameDataDomain.IsModified(DataStates, 33))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 33);
			return GameData.Serializer.Serializer.Serialize(_currEquipmentPlanId, dataPool);
		case 34:
			if (!BaseGameDataDomain.IsModified(DataStates, 34))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 34);
			return GameData.Serializer.Serializer.Serialize(_groupCharIds, dataPool);
		case 35:
			if (!BaseGameDataDomain.IsModified(_dataStatesCombatGroupCharIds, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesCombatGroupCharIds, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_combatGroupCharIds[(uint)subId0], dataPool);
		case 36:
			if (!BaseGameDataDomain.IsModified(DataStates, 36))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 36);
			return GameData.Serializer.Serializer.Serialize(GetTaiwuGroupMaxCount(), dataPool);
		case 37:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 37))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 37);
			int result8 = GameData.Serializer.Serializer.SerializeModifications(_legacyPointDict, dataPool, _modificationsLegacyPointDict);
			_modificationsLegacyPointDict.Reset();
			return result8;
		}
		case 38:
			if (!BaseGameDataDomain.IsModified(DataStates, 38))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 38);
			return GameData.Serializer.Serializer.Serialize(_legacyPoint, dataPool);
		case 39:
			if (!BaseGameDataDomain.IsModified(DataStates, 39))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 39);
			return GameData.Serializer.Serializer.Serialize(_availableLegacyList, dataPool);
		case 40:
			if (!BaseGameDataDomain.IsModified(DataStates, 40))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 40);
			return GameData.Serializer.Serializer.Serialize(_legacyPassingState, dataPool);
		case 41:
			if (!BaseGameDataDomain.IsModified(DataStates, 41))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 41);
			return GameData.Serializer.Serializer.Serialize(_successorCandidates, dataPool);
		case 42:
			if (!BaseGameDataDomain.IsModified(_dataStatesStateNewCharacterLegacyGrowingGrades, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesStateNewCharacterLegacyGrowingGrades, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_stateNewCharacterLegacyGrowingGrades[(uint)subId0], dataPool);
		case 43:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 43))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 43);
			int result3 = GameData.Serializer.Serializer.SerializeModifications(_notLearnCombatSkillReadingProgress, dataPool, _modificationsNotLearnCombatSkillReadingProgress);
			_modificationsNotLearnCombatSkillReadingProgress.Reset();
			return result3;
		}
		case 44:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 44))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 44);
			int result2 = GameData.Serializer.Serializer.SerializeModifications(_notLearnLifeSkillReadingProgress, dataPool, _modificationsNotLearnLifeSkillReadingProgress);
			_modificationsNotLearnLifeSkillReadingProgress.Reset();
			return result2;
		}
		case 45:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 46:
			if (!BaseGameDataDomain.IsModified(DataStates, 46))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 46);
			return GameData.Serializer.Serializer.Serialize(_curReadingBook, dataPool);
		case 47:
			if (!BaseGameDataDomain.IsModified(DataStates, 47))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 47);
			return GameData.Serializer.Serializer.Serialize(_referenceBooks, dataPool);
		case 48:
			if (!BaseGameDataDomain.IsModified(DataStates, 48))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 48);
			return GameData.Serializer.Serializer.Serialize(GetReferenceBookSlotUnlockStates(), dataPool);
		case 49:
			if (!BaseGameDataDomain.IsModified(DataStates, 49))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 49);
			return GameData.Serializer.Serializer.Serialize(_readingEventTriggered, dataPool);
		case 50:
			if (!BaseGameDataDomain.IsModified(DataStates, 50))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 50);
			return GameData.Serializer.Serializer.Serialize(_readInCombatCount, dataPool);
		case 51:
			if (!BaseGameDataDomain.IsModified(DataStates, 51))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 51);
			return GameData.Serializer.Serializer.Serialize(_healingOuterInjuryRestriction, dataPool);
		case 52:
			if (!BaseGameDataDomain.IsModified(DataStates, 52))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 52);
			return GameData.Serializer.Serializer.Serialize(_healingInnerInjuryRestriction, dataPool);
		case 53:
			if (!BaseGameDataDomain.IsModified(DataStates, 53))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 53);
			return GameData.Serializer.Serializer.Serialize(_neiliAllocationTypeRestriction, dataPool);
		case 54:
			if (!BaseGameDataDomain.IsModified(DataStates, 54))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 54);
			return GameData.Serializer.Serializer.Serialize(_visitedSettlements, dataPool);
		case 55:
			if (!BaseGameDataDomain.IsModified(DataStates, 55))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 55);
			return GameData.Serializer.Serializer.Serialize(_taiwuVillageSettlementId, dataPool);
		case 56:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 56))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 56);
			int result9 = GameData.Serializer.Serializer.SerializeModifications(_villagerWork, dataPool, _modificationsVillagerWork);
			_modificationsVillagerWork.Reset();
			return result9;
		}
		case 57:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 57))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 57);
			int result7 = GameData.Serializer.Serializer.SerializeModifications(_villagerWorkLocations, dataPool, _modificationsVillagerWorkLocations);
			_modificationsVillagerWorkLocations.Reset();
			return result7;
		}
		case 58:
			if (!BaseGameDataDomain.IsModified(DataStates, 58))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 58);
			return GameData.Serializer.Serializer.Serialize(GetMaterialResourceMaxCount(), dataPool);
		case 59:
			if (!BaseGameDataDomain.IsModified(DataStates, 59))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 59);
			return GameData.Serializer.Serializer.Serialize(GetResourceChange(), dataPool);
		case 60:
			if (!BaseGameDataDomain.IsModified(DataStates, 60))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 60);
			return GameData.Serializer.Serializer.Serialize(GetWorkLocationMaxCount(), dataPool);
		case 61:
			if (!BaseGameDataDomain.IsModified(DataStates, 61))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 61);
			return GameData.Serializer.Serializer.Serialize(GetTotalVillagerCount(), dataPool);
		case 62:
			if (!BaseGameDataDomain.IsModified(DataStates, 62))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 62);
			return GameData.Serializer.Serializer.Serialize(GetTotalAdultVillagerCount(), dataPool);
		case 63:
			if (!BaseGameDataDomain.IsModified(DataStates, 63))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 63);
			return GameData.Serializer.Serializer.Serialize(GetAvailableVillagerCount(), dataPool);
		case 64:
			if (!BaseGameDataDomain.IsModified(DataStates, 64))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 64);
			return GameData.Serializer.Serializer.Serialize(_isTaiwuDieOfCombatWithXiangshu, dataPool);
		case 65:
			if (!BaseGameDataDomain.IsModified(DataStates, 65))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 65);
			return GameData.Serializer.Serializer.Serialize(GetVillagerLearnLifeSkillsFromSect(), dataPool);
		case 66:
			if (!BaseGameDataDomain.IsModified(DataStates, 66))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 66);
			return GameData.Serializer.Serializer.Serialize(GetVillagerLearnCombatSkillsFromSect(), dataPool);
		case 67:
			if (!BaseGameDataDomain.IsModified(DataStates, 67))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 67);
			return GameData.Serializer.Serializer.Serialize(GetOverweightSanctionPercent(), dataPool);
		case 68:
			if (!BaseGameDataDomain.IsModified(DataStates, 68))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 68);
			return GameData.Serializer.Serializer.Serialize(GetReferenceSkillSlotUnlockStates(), dataPool);
		case 69:
			if (!BaseGameDataDomain.IsModified(DataStates, 69))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 69);
			return GameData.Serializer.Serializer.Serialize(GetTaiwuGroupWorstInjuries(), dataPool);
		case 70:
			if (!BaseGameDataDomain.IsModified(DataStates, 70))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 70);
			return GameData.Serializer.Serializer.Serialize(GetTotalResources(), dataPool);
		case 71:
			if (!BaseGameDataDomain.IsModified(DataStates, 71))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 71);
			return GameData.Serializer.Serializer.Serialize(GetTaiwuSpecialGroup(), dataPool);
		case 72:
			if (!BaseGameDataDomain.IsModified(DataStates, 72))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 72);
			return GameData.Serializer.Serializer.Serialize(GetTaiwuGearMateGroup(), dataPool);
		case 73:
			if (!BaseGameDataDomain.IsModified(DataStates, 73))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 73);
			return GameData.Serializer.Serializer.Serialize(GetCanBreakOut(), dataPool);
		case 74:
			if (!BaseGameDataDomain.IsModified(DataStates, 74))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 74);
			return GameData.Serializer.Serializer.Serialize(GetTroughMaxLoad(), dataPool);
		case 75:
			if (!BaseGameDataDomain.IsModified(DataStates, 75))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 75);
			return GameData.Serializer.Serializer.Serialize(GetTroughCurrLoad(), dataPool);
		case 76:
			if (!BaseGameDataDomain.IsModified(DataStates, 76))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 76);
			return GameData.Serializer.Serializer.Serialize(GetClothingDurability(), dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			if (BaseGameDataDomain.IsModified(DataStates, 0))
			{
				BaseGameDataDomain.ResetModified(DataStates, 0);
			}
			break;
		case 1:
			if (BaseGameDataDomain.IsModified(DataStates, 1))
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			break;
		case 2:
			if (BaseGameDataDomain.IsModified(DataStates, 2))
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			break;
		case 3:
			if (BaseGameDataDomain.IsModified(DataStates, 3))
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
			}
			break;
		case 4:
			if (BaseGameDataDomain.IsModified(DataStates, 4))
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
			}
			break;
		case 5:
			if (BaseGameDataDomain.IsModified(DataStates, 5))
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
			}
			break;
		case 6:
			if (BaseGameDataDomain.IsModified(DataStates, 6))
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
			}
			break;
		case 7:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 8:
			if (BaseGameDataDomain.IsModified(DataStates, 8))
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
			}
			break;
		case 9:
			if (BaseGameDataDomain.IsModified(DataStates, 9))
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
			}
			break;
		case 10:
			if (BaseGameDataDomain.IsModified(DataStates, 10))
			{
				BaseGameDataDomain.ResetModified(DataStates, 10);
			}
			break;
		case 11:
			if (BaseGameDataDomain.IsModified(DataStates, 11))
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
			}
			break;
		case 12:
			if (BaseGameDataDomain.IsModified(DataStates, 12))
			{
				BaseGameDataDomain.ResetModified(DataStates, 12);
			}
			break;
		case 13:
			if (BaseGameDataDomain.IsModified(DataStates, 13))
			{
				BaseGameDataDomain.ResetModified(DataStates, 13);
			}
			break;
		case 14:
			if (BaseGameDataDomain.IsModified(DataStates, 14))
			{
				BaseGameDataDomain.ResetModified(DataStates, 14);
				_modificationsCombatSkills.Reset();
			}
			break;
		case 15:
			if (BaseGameDataDomain.IsModified(DataStates, 15))
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
				_modificationsLifeSkills.Reset();
			}
			break;
		case 16:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 17:
			if (BaseGameDataDomain.IsModified(DataStates, 17))
			{
				BaseGameDataDomain.ResetModified(DataStates, 17);
			}
			break;
		case 18:
			if (BaseGameDataDomain.IsModified(_dataStatesCurrLifeSkillAttainmentPanelPlanIndex, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesCurrLifeSkillAttainmentPanelPlanIndex, (int)subId0);
			}
			break;
		case 19:
			if (BaseGameDataDomain.IsModified(DataStates, 19))
			{
				BaseGameDataDomain.ResetModified(DataStates, 19);
				_modificationsSkillBreakPlateObsoleteDict.Reset();
			}
			break;
		case 20:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 21:
			if (BaseGameDataDomain.IsModified(DataStates, 21))
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
				_modificationsTeachTaiwuLifeSkillDict.Reset();
			}
			break;
		case 22:
			if (BaseGameDataDomain.IsModified(DataStates, 22))
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
				_modificationsTeachTaiwuCombatSkillDict.Reset();
			}
			break;
		case 23:
			if (BaseGameDataDomain.IsModified(DataStates, 23))
			{
				BaseGameDataDomain.ResetModified(DataStates, 23);
			}
			break;
		case 24:
			if (BaseGameDataDomain.IsModified(DataStates, 24))
			{
				BaseGameDataDomain.ResetModified(DataStates, 24);
			}
			break;
		case 25:
			if (BaseGameDataDomain.IsModified(DataStates, 25))
			{
				BaseGameDataDomain.ResetModified(DataStates, 25);
			}
			break;
		case 26:
			if (BaseGameDataDomain.IsModified(DataStates, 26))
			{
				BaseGameDataDomain.ResetModified(DataStates, 26);
			}
			break;
		case 27:
			if (BaseGameDataDomain.IsModified(DataStates, 27))
			{
				BaseGameDataDomain.ResetModified(DataStates, 27);
			}
			break;
		case 28:
			if (BaseGameDataDomain.IsModified(DataStates, 28))
			{
				BaseGameDataDomain.ResetModified(DataStates, 28);
				_modificationsAppointments.Reset();
			}
			break;
		case 29:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 30:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 31:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 32:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 33:
			if (BaseGameDataDomain.IsModified(DataStates, 33))
			{
				BaseGameDataDomain.ResetModified(DataStates, 33);
			}
			break;
		case 34:
			if (BaseGameDataDomain.IsModified(DataStates, 34))
			{
				BaseGameDataDomain.ResetModified(DataStates, 34);
			}
			break;
		case 35:
			if (BaseGameDataDomain.IsModified(_dataStatesCombatGroupCharIds, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesCombatGroupCharIds, (int)subId0);
			}
			break;
		case 36:
			if (BaseGameDataDomain.IsModified(DataStates, 36))
			{
				BaseGameDataDomain.ResetModified(DataStates, 36);
			}
			break;
		case 37:
			if (BaseGameDataDomain.IsModified(DataStates, 37))
			{
				BaseGameDataDomain.ResetModified(DataStates, 37);
				_modificationsLegacyPointDict.Reset();
			}
			break;
		case 38:
			if (BaseGameDataDomain.IsModified(DataStates, 38))
			{
				BaseGameDataDomain.ResetModified(DataStates, 38);
			}
			break;
		case 39:
			if (BaseGameDataDomain.IsModified(DataStates, 39))
			{
				BaseGameDataDomain.ResetModified(DataStates, 39);
			}
			break;
		case 40:
			if (BaseGameDataDomain.IsModified(DataStates, 40))
			{
				BaseGameDataDomain.ResetModified(DataStates, 40);
			}
			break;
		case 41:
			if (BaseGameDataDomain.IsModified(DataStates, 41))
			{
				BaseGameDataDomain.ResetModified(DataStates, 41);
			}
			break;
		case 42:
			if (BaseGameDataDomain.IsModified(_dataStatesStateNewCharacterLegacyGrowingGrades, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesStateNewCharacterLegacyGrowingGrades, (int)subId0);
			}
			break;
		case 43:
			if (BaseGameDataDomain.IsModified(DataStates, 43))
			{
				BaseGameDataDomain.ResetModified(DataStates, 43);
				_modificationsNotLearnCombatSkillReadingProgress.Reset();
			}
			break;
		case 44:
			if (BaseGameDataDomain.IsModified(DataStates, 44))
			{
				BaseGameDataDomain.ResetModified(DataStates, 44);
				_modificationsNotLearnLifeSkillReadingProgress.Reset();
			}
			break;
		case 45:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 46:
			if (BaseGameDataDomain.IsModified(DataStates, 46))
			{
				BaseGameDataDomain.ResetModified(DataStates, 46);
			}
			break;
		case 47:
			if (BaseGameDataDomain.IsModified(DataStates, 47))
			{
				BaseGameDataDomain.ResetModified(DataStates, 47);
			}
			break;
		case 48:
			if (BaseGameDataDomain.IsModified(DataStates, 48))
			{
				BaseGameDataDomain.ResetModified(DataStates, 48);
			}
			break;
		case 49:
			if (BaseGameDataDomain.IsModified(DataStates, 49))
			{
				BaseGameDataDomain.ResetModified(DataStates, 49);
			}
			break;
		case 50:
			if (BaseGameDataDomain.IsModified(DataStates, 50))
			{
				BaseGameDataDomain.ResetModified(DataStates, 50);
			}
			break;
		case 51:
			if (BaseGameDataDomain.IsModified(DataStates, 51))
			{
				BaseGameDataDomain.ResetModified(DataStates, 51);
			}
			break;
		case 52:
			if (BaseGameDataDomain.IsModified(DataStates, 52))
			{
				BaseGameDataDomain.ResetModified(DataStates, 52);
			}
			break;
		case 53:
			if (BaseGameDataDomain.IsModified(DataStates, 53))
			{
				BaseGameDataDomain.ResetModified(DataStates, 53);
			}
			break;
		case 54:
			if (BaseGameDataDomain.IsModified(DataStates, 54))
			{
				BaseGameDataDomain.ResetModified(DataStates, 54);
			}
			break;
		case 55:
			if (BaseGameDataDomain.IsModified(DataStates, 55))
			{
				BaseGameDataDomain.ResetModified(DataStates, 55);
			}
			break;
		case 56:
			if (BaseGameDataDomain.IsModified(DataStates, 56))
			{
				BaseGameDataDomain.ResetModified(DataStates, 56);
				_modificationsVillagerWork.Reset();
			}
			break;
		case 57:
			if (BaseGameDataDomain.IsModified(DataStates, 57))
			{
				BaseGameDataDomain.ResetModified(DataStates, 57);
				_modificationsVillagerWorkLocations.Reset();
			}
			break;
		case 58:
			if (BaseGameDataDomain.IsModified(DataStates, 58))
			{
				BaseGameDataDomain.ResetModified(DataStates, 58);
			}
			break;
		case 59:
			if (BaseGameDataDomain.IsModified(DataStates, 59))
			{
				BaseGameDataDomain.ResetModified(DataStates, 59);
			}
			break;
		case 60:
			if (BaseGameDataDomain.IsModified(DataStates, 60))
			{
				BaseGameDataDomain.ResetModified(DataStates, 60);
			}
			break;
		case 61:
			if (BaseGameDataDomain.IsModified(DataStates, 61))
			{
				BaseGameDataDomain.ResetModified(DataStates, 61);
			}
			break;
		case 62:
			if (BaseGameDataDomain.IsModified(DataStates, 62))
			{
				BaseGameDataDomain.ResetModified(DataStates, 62);
			}
			break;
		case 63:
			if (BaseGameDataDomain.IsModified(DataStates, 63))
			{
				BaseGameDataDomain.ResetModified(DataStates, 63);
			}
			break;
		case 64:
			if (BaseGameDataDomain.IsModified(DataStates, 64))
			{
				BaseGameDataDomain.ResetModified(DataStates, 64);
			}
			break;
		case 65:
			if (BaseGameDataDomain.IsModified(DataStates, 65))
			{
				BaseGameDataDomain.ResetModified(DataStates, 65);
			}
			break;
		case 66:
			if (BaseGameDataDomain.IsModified(DataStates, 66))
			{
				BaseGameDataDomain.ResetModified(DataStates, 66);
			}
			break;
		case 67:
			if (BaseGameDataDomain.IsModified(DataStates, 67))
			{
				BaseGameDataDomain.ResetModified(DataStates, 67);
			}
			break;
		case 68:
			if (BaseGameDataDomain.IsModified(DataStates, 68))
			{
				BaseGameDataDomain.ResetModified(DataStates, 68);
			}
			break;
		case 69:
			if (BaseGameDataDomain.IsModified(DataStates, 69))
			{
				BaseGameDataDomain.ResetModified(DataStates, 69);
			}
			break;
		case 70:
			if (BaseGameDataDomain.IsModified(DataStates, 70))
			{
				BaseGameDataDomain.ResetModified(DataStates, 70);
			}
			break;
		case 71:
			if (BaseGameDataDomain.IsModified(DataStates, 71))
			{
				BaseGameDataDomain.ResetModified(DataStates, 71);
			}
			break;
		case 72:
			if (BaseGameDataDomain.IsModified(DataStates, 72))
			{
				BaseGameDataDomain.ResetModified(DataStates, 72);
			}
			break;
		case 73:
			if (BaseGameDataDomain.IsModified(DataStates, 73))
			{
				BaseGameDataDomain.ResetModified(DataStates, 73);
			}
			break;
		case 74:
			if (BaseGameDataDomain.IsModified(DataStates, 74))
			{
				BaseGameDataDomain.ResetModified(DataStates, 74);
			}
			break;
		case 75:
			if (BaseGameDataDomain.IsModified(DataStates, 75))
			{
				BaseGameDataDomain.ResetModified(DataStates, 75);
			}
			break;
		case 76:
			if (BaseGameDataDomain.IsModified(DataStates, 76))
			{
				BaseGameDataDomain.ResetModified(DataStates, 76);
			}
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		return dataId switch
		{
			0 => BaseGameDataDomain.IsModified(DataStates, 0), 
			1 => BaseGameDataDomain.IsModified(DataStates, 1), 
			2 => BaseGameDataDomain.IsModified(DataStates, 2), 
			3 => BaseGameDataDomain.IsModified(DataStates, 3), 
			4 => BaseGameDataDomain.IsModified(DataStates, 4), 
			5 => BaseGameDataDomain.IsModified(DataStates, 5), 
			6 => BaseGameDataDomain.IsModified(DataStates, 6), 
			7 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			8 => BaseGameDataDomain.IsModified(DataStates, 8), 
			9 => BaseGameDataDomain.IsModified(DataStates, 9), 
			10 => BaseGameDataDomain.IsModified(DataStates, 10), 
			11 => BaseGameDataDomain.IsModified(DataStates, 11), 
			12 => BaseGameDataDomain.IsModified(DataStates, 12), 
			13 => BaseGameDataDomain.IsModified(DataStates, 13), 
			14 => BaseGameDataDomain.IsModified(DataStates, 14), 
			15 => BaseGameDataDomain.IsModified(DataStates, 15), 
			16 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			17 => BaseGameDataDomain.IsModified(DataStates, 17), 
			18 => BaseGameDataDomain.IsModified(_dataStatesCurrLifeSkillAttainmentPanelPlanIndex, (int)subId0), 
			19 => BaseGameDataDomain.IsModified(DataStates, 19), 
			20 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			21 => BaseGameDataDomain.IsModified(DataStates, 21), 
			22 => BaseGameDataDomain.IsModified(DataStates, 22), 
			23 => BaseGameDataDomain.IsModified(DataStates, 23), 
			24 => BaseGameDataDomain.IsModified(DataStates, 24), 
			25 => BaseGameDataDomain.IsModified(DataStates, 25), 
			26 => BaseGameDataDomain.IsModified(DataStates, 26), 
			27 => BaseGameDataDomain.IsModified(DataStates, 27), 
			28 => BaseGameDataDomain.IsModified(DataStates, 28), 
			29 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			30 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			31 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			32 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			33 => BaseGameDataDomain.IsModified(DataStates, 33), 
			34 => BaseGameDataDomain.IsModified(DataStates, 34), 
			35 => BaseGameDataDomain.IsModified(_dataStatesCombatGroupCharIds, (int)subId0), 
			36 => BaseGameDataDomain.IsModified(DataStates, 36), 
			37 => BaseGameDataDomain.IsModified(DataStates, 37), 
			38 => BaseGameDataDomain.IsModified(DataStates, 38), 
			39 => BaseGameDataDomain.IsModified(DataStates, 39), 
			40 => BaseGameDataDomain.IsModified(DataStates, 40), 
			41 => BaseGameDataDomain.IsModified(DataStates, 41), 
			42 => BaseGameDataDomain.IsModified(_dataStatesStateNewCharacterLegacyGrowingGrades, (int)subId0), 
			43 => BaseGameDataDomain.IsModified(DataStates, 43), 
			44 => BaseGameDataDomain.IsModified(DataStates, 44), 
			45 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			46 => BaseGameDataDomain.IsModified(DataStates, 46), 
			47 => BaseGameDataDomain.IsModified(DataStates, 47), 
			48 => BaseGameDataDomain.IsModified(DataStates, 48), 
			49 => BaseGameDataDomain.IsModified(DataStates, 49), 
			50 => BaseGameDataDomain.IsModified(DataStates, 50), 
			51 => BaseGameDataDomain.IsModified(DataStates, 51), 
			52 => BaseGameDataDomain.IsModified(DataStates, 52), 
			53 => BaseGameDataDomain.IsModified(DataStates, 53), 
			54 => BaseGameDataDomain.IsModified(DataStates, 54), 
			55 => BaseGameDataDomain.IsModified(DataStates, 55), 
			56 => BaseGameDataDomain.IsModified(DataStates, 56), 
			57 => BaseGameDataDomain.IsModified(DataStates, 57), 
			58 => BaseGameDataDomain.IsModified(DataStates, 58), 
			59 => BaseGameDataDomain.IsModified(DataStates, 59), 
			60 => BaseGameDataDomain.IsModified(DataStates, 60), 
			61 => BaseGameDataDomain.IsModified(DataStates, 61), 
			62 => BaseGameDataDomain.IsModified(DataStates, 62), 
			63 => BaseGameDataDomain.IsModified(DataStates, 63), 
			64 => BaseGameDataDomain.IsModified(DataStates, 64), 
			65 => BaseGameDataDomain.IsModified(DataStates, 65), 
			66 => BaseGameDataDomain.IsModified(DataStates, 66), 
			67 => BaseGameDataDomain.IsModified(DataStates, 67), 
			68 => BaseGameDataDomain.IsModified(DataStates, 68), 
			69 => BaseGameDataDomain.IsModified(DataStates, 69), 
			70 => BaseGameDataDomain.IsModified(DataStates, 70), 
			71 => BaseGameDataDomain.IsModified(DataStates, 71), 
			72 => BaseGameDataDomain.IsModified(DataStates, 72), 
			73 => BaseGameDataDomain.IsModified(DataStates, 73), 
			74 => BaseGameDataDomain.IsModified(DataStates, 74), 
			75 => BaseGameDataDomain.IsModified(DataStates, 75), 
			76 => BaseGameDataDomain.IsModified(DataStates, 76), 
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
		case 8:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(8, DataStates, CacheInfluences, context);
			break;
		case 9:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(9, DataStates, CacheInfluences, context);
			break;
		case 10:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(10, DataStates, CacheInfluences, context);
			break;
		case 11:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(11, DataStates, CacheInfluences, context);
			break;
		case 25:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(25, DataStates, CacheInfluences, context);
			break;
		case 27:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(27, DataStates, CacheInfluences, context);
			break;
		case 36:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(36, DataStates, CacheInfluences, context);
			break;
		case 48:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(48, DataStates, CacheInfluences, context);
			break;
		case 58:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(58, DataStates, CacheInfluences, context);
			break;
		case 59:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(59, DataStates, CacheInfluences, context);
			break;
		case 60:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(60, DataStates, CacheInfluences, context);
			break;
		case 61:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(61, DataStates, CacheInfluences, context);
			break;
		case 62:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(62, DataStates, CacheInfluences, context);
			break;
		case 63:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(63, DataStates, CacheInfluences, context);
			break;
		case 65:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(65, DataStates, CacheInfluences, context);
			break;
		case 66:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(66, DataStates, CacheInfluences, context);
			break;
		case 67:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(67, DataStates, CacheInfluences, context);
			break;
		case 68:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(68, DataStates, CacheInfluences, context);
			break;
		case 69:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(69, DataStates, CacheInfluences, context);
			break;
		case 70:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(70, DataStates, CacheInfluences, context);
			break;
		case 71:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(71, DataStates, CacheInfluences, context);
			break;
		case 72:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(72, DataStates, CacheInfluences, context);
			break;
		case 73:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(73, DataStates, CacheInfluences, context);
			break;
		case 74:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(74, DataStates, CacheInfluences, context);
			break;
		case 75:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(75, DataStates, CacheInfluences, context);
			break;
		case 76:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(76, DataStates, CacheInfluences, context);
			break;
		default:
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
		case 26:
		case 28:
		case 29:
		case 30:
		case 31:
		case 32:
		case 33:
		case 34:
		case 35:
		case 37:
		case 38:
		case 39:
		case 40:
		case 41:
		case 42:
		case 43:
		case 44:
		case 45:
		case 46:
		case 47:
		case 49:
		case 50:
		case 51:
		case 52:
		case 53:
		case 54:
		case 55:
		case 56:
		case 57:
		case 64:
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		uint num;
		switch (operation.DataId)
		{
		case 0:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _taiwuCharId);
			goto IL_056a;
		case 1:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _taiwuGenerationsCount);
			goto IL_056a;
		case 2:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _cricketLuckPoint);
			goto IL_056a;
		case 3:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_List(operation, pResult, _previousTaiwuIds);
			goto IL_056a;
		case 4:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _needToEscape);
			goto IL_056a;
		case 7:
			ResponseProcessor.ProcessSingleValueCollection_BasicType_Fixed_Value(operation, pResult, _warehouseItems);
			goto IL_056a;
		case 12:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _buildingSpaceExtraAdd);
			goto IL_056a;
		case 13:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _prosperousConstruction);
			goto IL_056a;
		case 14:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Ref(operation, pResult, _combatSkills, 20);
			goto IL_056a;
		case 15:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Ref(operation, pResult, _lifeSkills, 5);
			goto IL_056a;
		case 16:
			ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref(operation, pResult, _combatSkillPlans, 9);
			goto IL_056a;
		case 17:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _currCombatSkillPlanId);
			goto IL_056a;
		case 18:
			ResponseProcessor.ProcessElementList_BasicType_Fixed_Value(operation, pResult, _currLifeSkillAttainmentPanelPlanIndex, 16, 1);
			goto IL_056a;
		case 19:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _skillBreakPlateObsoleteDict);
			goto IL_056a;
		case 20:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _skillBreakBonusDict);
			goto IL_056a;
		case 21:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<int, GameData.Utilities.ShortList>(operation, pResult, (IDictionary<int, GameData.Utilities.ShortList>)_teachTaiwuLifeSkillDict);
			goto IL_056a;
		case 22:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<int, GameData.Utilities.ShortList>(operation, pResult, (IDictionary<int, GameData.Utilities.ShortList>)_teachTaiwuCombatSkillDict);
			goto IL_056a;
		case 23:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array(operation, pResult, _combatSkillAttainmentPanelPlans, 630);
			goto IL_056a;
		case 24:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array(operation, pResult, _currCombatSkillAttainmentPanelPlanIds, 14);
			goto IL_056a;
		case 26:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array(operation, pResult, _weaponInnerRatios, 6);
			goto IL_056a;
		case 28:
			ResponseProcessor.ProcessSingleValueCollection_BasicType_Fixed_Value(operation, pResult, _appointments);
			goto IL_056a;
		case 29:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _babyBonusMainAttributes);
			goto IL_056a;
		case 30:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _babyBonusLifeSkillQualifications);
			goto IL_056a;
		case 31:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _babyBonusCombatSkillQualifications);
			goto IL_056a;
		case 32:
			ResponseProcessor.ProcessElementList_CustomType_Fixed_Ref(operation, pResult, _equipmentsPlans, 5, 99);
			goto IL_056a;
		case 33:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _currEquipmentPlanId);
			goto IL_056a;
		case 34:
			ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Value_Single<CharacterSet>(operation, pResult, ref _groupCharIds);
			goto IL_056a;
		case 35:
			ResponseProcessor.ProcessElementList_BasicType_Fixed_Value(operation, pResult, _combatGroupCharIds, 3, 4);
			goto IL_056a;
		case 37:
			ResponseProcessor.ProcessSingleValueCollection_BasicType_Fixed_Value(operation, pResult, _legacyPointDict);
			goto IL_056a;
		case 38:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _legacyPoint);
			goto IL_056a;
		case 39:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_List(operation, pResult, _availableLegacyList);
			goto IL_056a;
		case 42:
			ResponseProcessor.ProcessElementList_CustomType_Dynamic_Value<SByteList>(operation, pResult, _stateNewCharacterLegacyGrowingGrades, 15);
			goto IL_056a;
		case 43:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Ref(operation, pResult, _notLearnCombatSkillReadingProgress, 20);
			goto IL_056a;
		case 44:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Ref(operation, pResult, _notLearnLifeSkillReadingProgress, 5);
			goto IL_056a;
		case 45:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Value<ItemKey, ReadingBookStrategies>(operation, pResult, (IDictionary<ItemKey, ReadingBookStrategies>)_readingBooks, 36);
			goto IL_056a;
		case 46:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<ItemKey>(operation, pResult, ref _curReadingBook);
			goto IL_056a;
		case 47:
			ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Array<ItemKey>(operation, pResult, _referenceBooks, 3, 8);
			goto IL_056a;
		case 49:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _readingEventTriggered);
			goto IL_056a;
		case 50:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _readInCombatCount);
			goto IL_056a;
		case 54:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_List(operation, pResult, _visitedSettlements);
			goto IL_056a;
		case 55:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _taiwuVillageSettlementId);
			goto IL_056a;
		case 56:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Ref(operation, pResult, _villagerWork, 20);
			goto IL_056a;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		case 5:
		case 6:
		case 8:
		case 9:
		case 10:
		case 11:
		case 25:
		case 27:
		case 36:
		case 40:
		case 41:
		case 48:
		case 51:
		case 52:
		case 53:
		case 57:
		case 58:
		case 59:
		case 60:
		case 61:
		case 62:
		case 63:
		case 64:
		case 65:
		case 66:
		case 67:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
		case 73:
		case 74:
		case 75:
		case 76:
			{
				throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
			}
			IL_056a:
			if (_pendingLoadingOperationIds == null)
			{
				break;
			}
			num = _pendingLoadingOperationIds.Peek();
			if (num == operation.Id)
			{
				_pendingLoadingOperationIds.Dequeue();
				if (_pendingLoadingOperationIds.Count <= 0)
				{
					_pendingLoadingOperationIds = null;
					InitializeInternalDataOfCollections();
					OnLoadedArchiveData();
					DomainManager.Global.CompleteLoading(5);
				}
			}
			break;
		}
	}

	private void InitializeInternalDataOfCollections()
	{
	}

	private void RunTestings(DataContext context)
	{
		Test_QuickFillCombatSkillAttainmentPanel(context);
		Test_SaveCombatSkillAttainmentPanelPlan(context);
	}

	private void Test_FindSectLeaderLocation()
	{
		Location location = _taiwuChar.GetLocation();
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(location.AreaId);
		for (int i = 0; i < 10; i++)
		{
			Logger.Info("-----------");
			Stopwatch stopwatch = Stopwatch.StartNew();
			GameData.Domains.Character.Character character = null;
			Span<MapBlockData> span = areaBlocks;
			for (int j = 0; j < span.Length; j++)
			{
				MapBlockData mapBlockData = span[j];
				if (character == null)
				{
					character = EventHelper.FindLeaderOfTargetOrganizationAtLocation(mapBlockData.GetLocation(), 3, includeAllRelatedLocations: true);
				}
			}
			stopwatch.Stop();
			Logger.Info($"Found {character} in {stopwatch.ElapsedTicks} ticks with {"FindLeaderOfTargetOrganizationAtLocation"}");
			Stopwatch stopwatch2 = Stopwatch.StartNew();
			GameData.Domains.Character.Character character2 = null;
			Span<MapBlockData> span2 = areaBlocks;
			for (int k = 0; k < span2.Length; k++)
			{
				MapBlockData mapBlockData2 = span2[k];
				if (character2 == null)
				{
					character2 = EventHelper.TryGetSectLeaderAtLocation(mapBlockData2.GetLocation(), 3, includeAllBlocksInGroup: true);
				}
			}
			stopwatch2.Stop();
			Logger.Info($"Found {character2} in {stopwatch2.ElapsedTicks} ticks with {"TryGetSectLeaderAtLocation"}");
			Logger.Info("-----------");
		}
	}

	private void Test_QuickFillCombatSkillAttainmentPanel(DataContext context)
	{
		short[] combatSkillAttainmentPanels = _taiwuChar.GetCombatSkillAttainmentPanels();
		for (sbyte b = 0; b < 14; b += 2)
		{
			QuickFillCombatSkillAttainmentPanel(context, b);
		}
		sbyte skillType;
		for (skillType = 0; skillType < 14; skillType += 2)
		{
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_taiwuCharId);
			List<KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill>> list = charCombatSkills.Where((KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> pair) => Config.CombatSkill.Instance[pair.Key].Type == skillType && !pair.Value.GetRevoked() && CombatSkillStateHelper.IsBrokenOut(pair.Value.GetActivationState())).ToList();
			sbyte grade;
			for (grade = 0; grade < 9; grade++)
			{
				short num = CombatSkillAttainmentPanelsHelper.Get(combatSkillAttainmentPanels, skillType, grade);
				if (num < 0)
				{
					Tester.Assert(!list.Exists((KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> pair) => Config.CombatSkill.Instance[pair.Key].Grade == grade));
				}
				else
				{
					Tester.Assert(Config.CombatSkill.Instance[num].Grade == grade);
					Tester.Assert(Config.CombatSkill.Instance[num].Type == skillType);
					Tester.Assert(_skillBreakPlateObsoleteDict[num].Finished);
				}
			}
		}
	}

	private void Test_SaveCombatSkillAttainmentPanelPlan(DataContext context)
	{
		short[] combatSkillAttainmentPanels = _taiwuChar.GetCombatSkillAttainmentPanels();
		for (sbyte b = 0; b < 14; b += 2)
		{
			SaveCombatSkillAttainmentPanelPlan(context, b, (sbyte)(b % 4 + 1));
		}
		sbyte skillType;
		for (skillType = 0; skillType < 14; skillType++)
		{
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_taiwuCharId);
			List<KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill>> list = charCombatSkills.Where((KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> pair) => Config.CombatSkill.Instance[pair.Key].Type == skillType && !pair.Value.GetRevoked() && CombatSkillStateHelper.IsBrokenOut(pair.Value.GetActivationState())).ToList();
			for (sbyte b2 = 0; b2 < 5; b2++)
			{
				SelectCombatSkillAttainmentPanelPlan(context, skillType, b2);
				Logger.Info("CombatSkillAttainmentPanelPlan ========================================");
				Logger.Info($"skill type {skillType}\t planId {b2}");
				sbyte grade;
				for (grade = 0; grade < 9; grade++)
				{
					short num = CombatSkillAttainmentPanelsHelper.Get(combatSkillAttainmentPanels, skillType, grade);
					if (num < 0)
					{
						Tester.Assert(!list.Exists((KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> pair) => Config.CombatSkill.Instance[pair.Key].Grade == grade));
						Logger.Warn($"Can't find viable CombatSkill with type {skillType} and grade {grade}");
					}
					else
					{
						Logger.Info($"Can't find viable CombatSkill with type {skillType} and grade {grade}");
						Tester.Assert(Config.CombatSkill.Instance[num].Grade == grade);
						Tester.Assert(Config.CombatSkill.Instance[num].Type == skillType);
						Tester.Assert(_skillBreakPlateObsoleteDict[num].Finished);
					}
				}
			}
		}
	}
}
