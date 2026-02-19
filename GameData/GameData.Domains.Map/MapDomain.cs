using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Config;
using Config.ConfigCells;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Common.Algorithm;
using GameData.Common.SingleValueCollection;
using GameData.DLC.FiveLoong;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.ParallelModifications;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.Item.Filters;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map.TeammateBubble;
using GameData.Domains.Merchant;
using GameData.Domains.Organization;
using GameData.Domains.Organization.ParallelModifications;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.World;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Domains.World.TravelingEvent;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Map;

[GameDataDomain(2)]
public class MapDomain : BaseGameDataDomain
{
	public delegate void ApplyPickupDelegate(DataContext context, MapPickup pickup);

	public delegate bool MapBlockDataFilter(MapBlockData blockData);

	public const sbyte RegularStateCount = 15;

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[DomainData(DomainDataType.ElementList, true, false, true, false, ArrayElementsCount = 139)]
	private readonly MapAreaData[] _areas;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks0;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks1;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks2;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks3;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks4;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks5;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks6;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks7;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks8;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks9;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks10;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks11;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks12;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks13;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks14;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks15;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks16;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks17;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks18;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks19;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks20;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks21;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks22;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks23;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks24;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks25;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks26;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks27;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks28;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks29;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks30;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks31;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks32;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks33;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks34;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks35;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks36;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks37;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks38;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks39;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks40;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks41;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks42;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks43;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _areaBlocks44;

	private AreaBlockCollection[] _regularAreaBlocksArray;

	private Action<short, MapBlockData, DataContext>[] _regularAreaBlocksAddFuncs;

	private Action<short, MapBlockData, DataContext>[] _regularAreaBlocksSetFuncs;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _brokenAreaBlocks;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _bornAreaBlocks;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _guideAreaBlocks;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _secretVillageAreaBlocks;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private AreaBlockCollection _brokenPerformAreaBlocks;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private Dictionary<TravelRouteKey, TravelRoute> _travelRouteDict;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private Dictionary<TravelRouteKey, TravelRoute> _bornStateTravelRouteDict;

	[Obsolete]
	[DomainData(DomainDataType.ElementList, true, false, true, false, ArrayElementsCount = 139)]
	private readonly AnimalPlaceData[] _animalPlaceData;

	[DomainData(DomainDataType.ElementList, true, false, true, false, ArrayElementsCount = 139)]
	private readonly CricketPlaceData[] _cricketPlaceData;

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<short, GameData.Utilities.ShortList> _regularAreaNearList;

	[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 8)]
	private readonly Location[] _swordTombLocations;

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private List<HunterAnimalKey> _hunterAnimalsCache;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private List<LoongLocationData> _loongLocations;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private List<Location> _fleeBeasts;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private List<Location> _fleeLoongs;

	private readonly List<MapBlockDisplayData> _blockDisplayDataCache = new List<MapBlockDisplayData>();

	public Location LastGetBlockDataPosition_Debug;

	private static AStarMap _aStarMap = new AStarMap();

	private const sbyte InitMaliceBlockPercent = 5;

	private Stopwatch _swCreatingNormalAreas;

	private Stopwatch _swCreatingSettlements;

	private Stopwatch _swInitializingAreaTravelRoutes;

	private static readonly Dictionary<MapPickup.EMapPickupType, ApplyPickupDelegate> ApplyPickups = new Dictionary<MapPickup.EMapPickupType, ApplyPickupDelegate>
	{
		{
			MapPickup.EMapPickupType.Resource,
			AddResourceByPickup
		},
		{
			MapPickup.EMapPickupType.Item,
			AddItemByPickup
		},
		{
			MapPickup.EMapPickupType.LoopEffect,
			LoopOnceByPickup
		},
		{
			MapPickup.EMapPickupType.ReadEffect,
			ReadOnceByPickup
		},
		{
			MapPickup.EMapPickupType.ExpBonus,
			AddExpByPickup
		},
		{
			MapPickup.EMapPickupType.DebtBonus,
			AddDebtByPickup
		}
	};

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private List<MapElementPickupDisplayData> _visibleMapPickups;

	private HashSet<Location> _wudangHeavenlyTrees = new HashSet<Location>();

	private HashSet<int> _fulongLightedBlocks = new HashSet<int>();

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private int _moveBanned;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _crossArchiveLockMoveTime;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _isTaiwuInFulongFlameArea;

	public Queue<Location> TaiwuMoveRecord = new Queue<Location>(3);

	private bool _lockMoveTime = false;

	private bool _teleportMove = false;

	private (int charId, int count) _lastTeammateBubble;

	private Location _lastTaiwuLocation;

	private bool _canTriggerFulongFlameTeammateBubble;

	private readonly List<(int charId, int index, int subtype)> _teammates = new List<(int, int, int)>();

	private readonly List<(int charId, int index, int subtype)> _teammateHighestPriorityText = new List<(int, int, int)>();

	private readonly Dictionary<int, int> _availableBubbleCache = new Dictionary<int, int>();

	private readonly Dictionary<ETeammateBubbleBubbleElementType, HashSet<short>> _elementTypeBubbleCache = new Dictionary<ETeammateBubbleBubbleElementType, HashSet<short>>();

	private readonly Dictionary<short, sbyte> _combatMatchIdToCombatSkillType = new Dictionary<short, sbyte>
	{
		{ 2, 13 },
		{ 0, 8 },
		{ 1, 7 },
		{ 13, 10 },
		{ 17, 12 },
		{ 14, 3 },
		{ 18, 4 },
		{ 11, 6 },
		{ 16, 5 },
		{ 15, 11 },
		{ 12, 9 }
	};

	private int _teammateTypes;

	private const int AreaFindPathMultiplier = 1000;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private CrossAreaMoveInfo _travelInfo;

	private readonly DijkstraMap _dijkstraMap = new DijkstraMap();

	private int _carrierReduceTravelCostDaysPercent;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _onHandlingTravelingEventBlock;

	private readonly List<(short, short)> _travelingEventWeights = new List<(short, short)>();

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private List<Location> _alterSettlementLocations;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[68][];

	private static readonly DataInfluence[][] CacheInfluencesAreas = new DataInfluence[139][];

	private readonly byte[] _dataStatesAreas = new byte[35];

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks0 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks1 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks2 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks3 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks4 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks5 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks6 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks7 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks8 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks9 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks10 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks11 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks12 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks13 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks14 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks15 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks16 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks17 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks18 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks19 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks20 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks21 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks22 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks23 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks24 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks25 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks26 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks27 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks28 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks29 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks30 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks31 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks32 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks33 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks34 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks35 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks36 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks37 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks38 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks39 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks40 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks41 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks42 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks43 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsAreaBlocks44 = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsBrokenAreaBlocks = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsBornAreaBlocks = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsGuideAreaBlocks = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsSecretVillageAreaBlocks = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<short> _modificationsBrokenPerformAreaBlocks = SingleValueCollectionModificationCollection<short>.Create();

	private SingleValueCollectionModificationCollection<TravelRouteKey> _modificationsTravelRouteDict = SingleValueCollectionModificationCollection<TravelRouteKey>.Create();

	private SingleValueCollectionModificationCollection<TravelRouteKey> _modificationsBornStateTravelRouteDict = SingleValueCollectionModificationCollection<TravelRouteKey>.Create();

	private static readonly DataInfluence[][] CacheInfluencesAnimalPlaceData = new DataInfluence[139][];

	private readonly byte[] _dataStatesAnimalPlaceData = new byte[35];

	private static readonly DataInfluence[][] CacheInfluencesCricketPlaceData = new DataInfluence[139][];

	private readonly byte[] _dataStatesCricketPlaceData = new byte[35];

	private static readonly DataInfluence[][] CacheInfluencesSwordTombLocations = new DataInfluence[8][];

	private readonly byte[] _dataStatesSwordTombLocations = new byte[2];

	private SpinLock _spinLockFleeBeasts = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockFleeLoongs = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockLoongLocations = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockAlterSettlementLocations = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockVisibleMapPickups = new SpinLock(enableThreadOwnerTracking: false);

	private Queue<uint> _pendingLoadingOperationIds;

	public bool TempDisableTriggerNormalPickupByTaiwuEscape { get; set; }

	public Location TaiwuLastLocation { get; set; }

	public bool IsTraveling => _travelInfo.Traveling;

	private void OnInitializedDomainData()
	{
		for (int i = 0; i < 139; i++)
		{
			_areas[i] = new MapAreaData();
		}
		InitRegularBlockArrayAndFuncs();
		TaiwuLastLocation = Location.Invalid;
	}

	private void InitializeOnInitializeGameDataModule()
	{
	}

	private void InitializeOnEnterNewWorld()
	{
		Events.RegisterHandler_CharacterLocationChanged(OnCharacterLocationChanged);
		InitializeTeammateBubble();
	}

	private void OnLoadedArchiveData()
	{
		for (short num = 0; num < 45; num++)
		{
			GetRegularAreaBlocks(num).ConvertToRegularCollection();
		}
		_brokenAreaBlocks.ConvertToRegularCollection();
		_bornAreaBlocks.ConvertToRegularCollection();
		_guideAreaBlocks.ConvertToRegularCollection();
		_secretVillageAreaBlocks.ConvertToRegularCollection();
		_brokenPerformAreaBlocks.ConvertToRegularCollection();
		for (short num2 = 0; num2 < 139; num2++)
		{
			Span<MapBlockData> areaBlocks = GetAreaBlocks(num2);
			for (short num3 = 0; num3 < areaBlocks.Length; num3++)
			{
				MapBlockData mapBlockData = areaBlocks[num3];
				if (mapBlockData.RootBlockId >= 0)
				{
					MapBlockData mapBlockData2 = areaBlocks[mapBlockData.RootBlockId];
					MapBlockData mapBlockData3 = mapBlockData2;
					if (mapBlockData3.GroupBlockList == null)
					{
						mapBlockData3.GroupBlockList = new List<MapBlockData>();
					}
					if (!mapBlockData2.GroupBlockList.Contains(mapBlockData))
					{
						mapBlockData2.GroupBlockList.Add(mapBlockData);
					}
				}
			}
		}
		Events.RegisterHandler_CharacterLocationChanged(OnCharacterLocationChanged);
		InitializeTravelMap();
		InitializeTeammateBubble();
	}

	public override void FixAbnormalDomainArchiveData(DataContext context)
	{
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		for (short num = 0; num < 45; num++)
		{
			MapAreaData mapAreaData = _areas[num];
			for (int i = 0; i < mapAreaData.SettlementInfos.Length; i++)
			{
				short blockId = mapAreaData.SettlementInfos[i].BlockId;
				if (blockId < 0)
				{
					continue;
				}
				list.Clear();
				GetNeighborBlocks(num, blockId, list, GetBlockRange(GetBlock(num, blockId)));
				foreach (MapBlockData item in list)
				{
					if (item.BelongBlockId < 0)
					{
						item.BelongBlockId = blockId;
						SetBlockData(context, item);
					}
				}
			}
		}
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 69, 61))
		{
			FixUnexpectedItemsOnMapBlocks(context);
		}
		if (DomainManager.Extra.GetStationInited() == 0)
		{
			for (short num2 = 0; num2 < 135; num2++)
			{
				if (!_areas[num2].StationUnlocked)
				{
					UnlockStation(context, num2, costAuthority: false);
				}
			}
			DomainManager.Extra.SetStationInited(1, context);
		}
		for (short num3 = 0; num3 < _animalPlaceData.Length; num3++)
		{
			AnimalPlaceData animalPlaceData = _animalPlaceData[num3];
			if (animalPlaceData != null)
			{
				foreach (KeyValuePair<short, short> blockAnimalCharacterTemplateId in animalPlaceData.BlockAnimalCharacterTemplateIds)
				{
					DomainManager.Extra.CreateAnimalByCharacterTemplateId(context, blockAnimalCharacterTemplateId.Value, new Location(num3, blockAnimalCharacterTemplateId.Key));
				}
				SetElement_AnimalPlaceData(num3, null, context);
			}
		}
		for (short num4 = 0; num4 < _cricketPlaceData.Length; num4++)
		{
			if (_cricketPlaceData[num4] != null)
			{
				_cricketPlaceData[num4].FixInvalidData(num4);
				SetElement_CricketPlaceData(num4, _cricketPlaceData[num4], context);
			}
		}
		for (short num5 = 0; num5 < 45; num5++)
		{
			Span<MapBlockData> areaBlocks = GetAreaBlocks(num5);
			for (int j = 0; j < areaBlocks.Length; j++)
			{
				MapBlockData mapBlockData = areaBlocks[j];
				MapBlockItem config = mapBlockData.GetConfig();
				if (config.Size > 1 && mapBlockData.Destroyed)
				{
					mapBlockData.Destroyed = false;
					SetBlockData(context, mapBlockData);
					Logger.Warn("Fixing destroyed multi-block block " + config.Name + ".");
				}
			}
		}
		if (!DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 79, 24))
		{
			return;
		}
		AdaptableLog.Info("Fixing TemplateEnemyList during.");
		for (short num6 = 0; num6 < 139; num6++)
		{
			Span<MapBlockData> areaBlocks2 = GetAreaBlocks(num6);
			for (int k = 0; k < areaBlocks2.Length; k++)
			{
				MapBlockData mapBlockData2 = areaBlocks2[k];
				List<MapTemplateEnemyInfo> templateEnemyList = mapBlockData2.TemplateEnemyList;
				if ((templateEnemyList != null && templateEnemyList.Count != 0) || 1 == 0)
				{
					int count = mapBlockData2.TemplateEnemyList.Count;
					while (count-- > 0)
					{
						MapTemplateEnemyInfo value = mapBlockData2.TemplateEnemyList[count];
						value.Duration = -1;
						mapBlockData2.TemplateEnemyList[count] = value;
					}
					SetBlockData(context, mapBlockData2);
				}
			}
		}
	}

	private void FixUnexpectedItemsOnMapBlocks(DataContext context)
	{
		HashSet<ItemKey> hashSet = new HashSet<ItemKey>();
		for (short num = 0; num < 139; num++)
		{
			Span<MapBlockData> areaBlocks = GetAreaBlocks(num);
			int i = 0;
			for (int length = areaBlocks.Length; i < length; i++)
			{
				MapBlockData mapBlockData = areaBlocks[i];
				SortedList<ItemKeyAndDate, int> items = mapBlockData.Items;
				if (items == null)
				{
					continue;
				}
				bool flag = false;
				for (int num2 = items.Keys.Count - 1; num2 >= 0; num2--)
				{
					ItemKeyAndDate itemKeyAndDate = items.Keys[num2];
					ItemKey itemKey = itemKeyAndDate.ItemKey;
					if (!ItemTemplateHelper.IsPureStackable(itemKey) && !hashSet.Add(itemKey))
					{
						Logger.Warn($"Removing duplicate item {itemKey} on map block at {mapBlockData.GetLocation()}.");
						mapBlockData.RemoveItem(itemKeyAndDate);
						flag = true;
					}
					else if (!ItemDomain.CanItemBeLost(itemKey))
					{
						Logger.Warn($"Removing unexpected item {itemKey} on map block at {mapBlockData.GetLocation()}.");
						mapBlockData.RemoveItem(itemKeyAndDate);
						DomainManager.Item.RemoveItem(context, itemKey);
						flag = true;
						if (ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 1202)
						{
							DomainManager.LegendaryBook.UnregisterLegendaryBookItem(itemKey);
						}
					}
					else if (!DomainManager.Item.ItemExists(itemKey))
					{
						Logger.Warn($"Removing non-existing item {itemKey} on map block at {mapBlockData.GetLocation()}");
						mapBlockData.RemoveItem(itemKeyAndDate);
						flag = true;
					}
					else if (ItemType.IsEquipmentItemType(itemKey.ItemType))
					{
						int equippedCharId = DomainManager.Item.GetBaseEquipment(itemKey).GetEquippedCharId();
						if (equippedCharId >= 0 && DomainManager.Character.TryGetElement_Objects(equippedCharId, out var element) && element.GetEquipment().Exist(itemKey))
						{
							Logger.Warn($"Removing equipment {itemKey} on map block at {mapBlockData.GetLocation()} because it is equipped by {element}");
							mapBlockData.RemoveItem(itemKeyAndDate);
							flag = true;
						}
					}
				}
				if (flag)
				{
					SetBlockData(context, mapBlockData);
				}
			}
		}
	}

	public AreaBlockCollection GetRegularAreaBlocks(short areaId)
	{
		if (areaId == 138)
		{
			return _brokenPerformAreaBlocks;
		}
		return _regularAreaBlocksArray[areaId];
	}

	public void AddRegularBlockData(DataContext context, Location blockKey, MapBlockData data)
	{
		_regularAreaBlocksAddFuncs[blockKey.AreaId](blockKey.BlockId, data, context);
	}

	public void SetRegularBlockData(DataContext context, MapBlockData block)
	{
		_regularAreaBlocksSetFuncs[block.AreaId](block.BlockId, block, context);
	}

	private void InitRegularBlockArrayAndFuncs()
	{
		_regularAreaBlocksArray = new AreaBlockCollection[45]
		{
			_areaBlocks0, _areaBlocks1, _areaBlocks2, _areaBlocks3, _areaBlocks4, _areaBlocks5, _areaBlocks6, _areaBlocks7, _areaBlocks8, _areaBlocks9,
			_areaBlocks10, _areaBlocks11, _areaBlocks12, _areaBlocks13, _areaBlocks14, _areaBlocks15, _areaBlocks16, _areaBlocks17, _areaBlocks18, _areaBlocks19,
			_areaBlocks20, _areaBlocks21, _areaBlocks22, _areaBlocks23, _areaBlocks24, _areaBlocks25, _areaBlocks26, _areaBlocks27, _areaBlocks28, _areaBlocks29,
			_areaBlocks30, _areaBlocks31, _areaBlocks32, _areaBlocks33, _areaBlocks34, _areaBlocks35, _areaBlocks36, _areaBlocks37, _areaBlocks38, _areaBlocks39,
			_areaBlocks40, _areaBlocks41, _areaBlocks42, _areaBlocks43, _areaBlocks44
		};
		_regularAreaBlocksAddFuncs = new Action<short, MapBlockData, DataContext>[45]
		{
			AddElement_AreaBlocks0, AddElement_AreaBlocks1, AddElement_AreaBlocks2, AddElement_AreaBlocks3, AddElement_AreaBlocks4, AddElement_AreaBlocks5, AddElement_AreaBlocks6, AddElement_AreaBlocks7, AddElement_AreaBlocks8, AddElement_AreaBlocks9,
			AddElement_AreaBlocks10, AddElement_AreaBlocks11, AddElement_AreaBlocks12, AddElement_AreaBlocks13, AddElement_AreaBlocks14, AddElement_AreaBlocks15, AddElement_AreaBlocks16, AddElement_AreaBlocks17, AddElement_AreaBlocks18, AddElement_AreaBlocks19,
			AddElement_AreaBlocks20, AddElement_AreaBlocks21, AddElement_AreaBlocks22, AddElement_AreaBlocks23, AddElement_AreaBlocks24, AddElement_AreaBlocks25, AddElement_AreaBlocks26, AddElement_AreaBlocks27, AddElement_AreaBlocks28, AddElement_AreaBlocks29,
			AddElement_AreaBlocks30, AddElement_AreaBlocks31, AddElement_AreaBlocks32, AddElement_AreaBlocks33, AddElement_AreaBlocks34, AddElement_AreaBlocks35, AddElement_AreaBlocks36, AddElement_AreaBlocks37, AddElement_AreaBlocks38, AddElement_AreaBlocks39,
			AddElement_AreaBlocks40, AddElement_AreaBlocks41, AddElement_AreaBlocks42, AddElement_AreaBlocks43, AddElement_AreaBlocks44
		};
		_regularAreaBlocksSetFuncs = new Action<short, MapBlockData, DataContext>[45]
		{
			SetElement_AreaBlocks0, SetElement_AreaBlocks1, SetElement_AreaBlocks2, SetElement_AreaBlocks3, SetElement_AreaBlocks4, SetElement_AreaBlocks5, SetElement_AreaBlocks6, SetElement_AreaBlocks7, SetElement_AreaBlocks8, SetElement_AreaBlocks9,
			SetElement_AreaBlocks10, SetElement_AreaBlocks11, SetElement_AreaBlocks12, SetElement_AreaBlocks13, SetElement_AreaBlocks14, SetElement_AreaBlocks15, SetElement_AreaBlocks16, SetElement_AreaBlocks17, SetElement_AreaBlocks18, SetElement_AreaBlocks19,
			SetElement_AreaBlocks20, SetElement_AreaBlocks21, SetElement_AreaBlocks22, SetElement_AreaBlocks23, SetElement_AreaBlocks24, SetElement_AreaBlocks25, SetElement_AreaBlocks26, SetElement_AreaBlocks27, SetElement_AreaBlocks28, SetElement_AreaBlocks29,
			SetElement_AreaBlocks30, SetElement_AreaBlocks31, SetElement_AreaBlocks32, SetElement_AreaBlocks33, SetElement_AreaBlocks34, SetElement_AreaBlocks35, SetElement_AreaBlocks36, SetElement_AreaBlocks37, SetElement_AreaBlocks38, SetElement_AreaBlocks39,
			SetElement_AreaBlocks40, SetElement_AreaBlocks41, SetElement_AreaBlocks42, SetElement_AreaBlocks43, SetElement_AreaBlocks44
		};
	}

	public void GetPassableBlocksInArea(short areaId, List<MapBlockData> result)
	{
		result.Clear();
		Span<MapBlockData> areaBlocks = GetAreaBlocks(areaId);
		Span<MapBlockData> span = areaBlocks;
		for (int i = 0; i < span.Length; i++)
		{
			MapBlockData mapBlockData = span[i];
			if (mapBlockData.IsPassable())
			{
				result.Add(mapBlockData);
			}
		}
	}

	public Span<MapBlockData> GetAreaBlocks(short areaId)
	{
		short num = areaId;
		short num2 = num;
		if (num2 >= 45)
		{
			if (num2 >= 135)
			{
				return num2 switch
				{
					135 => new Span<MapBlockData>(_bornAreaBlocks.GetArray(), 0, _bornAreaBlocks.Count), 
					136 => new Span<MapBlockData>(_guideAreaBlocks.GetArray(), 0, _guideAreaBlocks.Count), 
					137 => new Span<MapBlockData>(_secretVillageAreaBlocks.GetArray(), 0, _secretVillageAreaBlocks.Count), 
					138 => new Span<MapBlockData>(_brokenPerformAreaBlocks.GetArray(), 0, _brokenPerformAreaBlocks.Count), 
					_ => throw new Exception($"Invalid area id: {areaId}"), 
				};
			}
			MapBlockData[] array = _brokenAreaBlocks.GetArray();
			if (array.Length == 0)
			{
				return new Span<MapBlockData>(array, 0, 0);
			}
			int num3 = 25;
			int start = (short)(num3 * (areaId - 45));
			return new Span<MapBlockData>(array, start, num3);
		}
		AreaBlockCollection regularAreaBlocks = GetRegularAreaBlocks(areaId);
		return new Span<MapBlockData>(regularAreaBlocks.GetArray(), 0, regularAreaBlocks.Count);
	}

	public void SetBlockData(DataContext context, MapBlockData block)
	{
		short areaId = block.AreaId;
		short num = areaId;
		if (num >= 45)
		{
			if (num >= 135)
			{
				switch (num)
				{
				case 135:
					SetElement_BornAreaBlocks(block.BlockId, block, context);
					break;
				case 136:
					SetElement_GuideAreaBlocks(block.BlockId, block, context);
					break;
				case 137:
					SetElement_SecretVillageAreaBlocks(block.BlockId, block, context);
					break;
				case 138:
					SetElement_BrokenPerformAreaBlocks(block.BlockId, block, context);
					break;
				default:
					throw new Exception($"Invalid area id: {block.AreaId}");
				}
			}
			else
			{
				short elementId = (short)(25 * (block.AreaId - 45) + block.BlockId);
				SetElement_BrokenAreaBlocks(elementId, block, context);
			}
		}
		else
		{
			SetRegularBlockData(context, block);
		}
	}

	public void OnCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
	{
		if (srcLocation.IsValid())
		{
			MapBlockData block = GetBlock(srcLocation);
			if (block.RemoveCharacter(charId))
			{
				SetBlockData(context, block);
			}
		}
		if (destLocation.IsValid())
		{
			MapBlockData block2 = GetBlock(destLocation);
			block2.AddCharacter(charId);
			SetBlockData(context, block2);
			DomainManager.Taiwu.CheckNotTaiwu(charId);
		}
	}

	public void OnInfectedCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
	{
		if (srcLocation.IsValid())
		{
			MapBlockData block = GetBlock(srcLocation);
			if (block.RemoveInfectedCharacter(charId))
			{
				SetBlockData(context, block);
			}
		}
		if (destLocation.IsValid())
		{
			MapBlockData block2 = GetBlock(destLocation);
			block2.AddInfectedCharacter(charId);
			SetBlockData(context, block2);
			DomainManager.Taiwu.CheckNotTaiwu(charId);
		}
	}

	public void OnFixedCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
	{
		if (srcLocation.IsValid())
		{
			MapBlockData block = GetBlock(srcLocation);
			if (block.RemoveFixedCharacter(charId))
			{
				SetBlockData(context, block);
			}
		}
		if (destLocation.IsValid())
		{
			MapBlockData block2 = GetBlock(destLocation);
			block2.AddFixedCharacter(charId);
			SetBlockData(context, block2);
			SetBlockAndViewRangeVisible(context, destLocation.AreaId, destLocation.BlockId);
		}
	}

	public void OnEnemyCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
	{
		if (srcLocation.IsValid())
		{
			MapBlockData block = GetBlock(srcLocation);
			if (block.RemoveEnemyCharacter(charId))
			{
				SetBlockData(context, block);
			}
		}
		if (destLocation.IsValid())
		{
			MapBlockData block2 = GetBlock(destLocation);
			block2.AddEnemyCharacter(charId);
			SetBlockData(context, block2);
		}
	}

	public void OnGraveLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
	{
		if (srcLocation.IsValid())
		{
			MapBlockData block = GetBlock(srcLocation);
			if (block.RemoveGrave(charId))
			{
				SetBlockData(context, block);
			}
		}
		if (destLocation.IsValid())
		{
			MapBlockData block2 = GetBlock(destLocation);
			block2.AddGrave(charId);
			SetBlockData(context, block2);
		}
	}

	public void OnTemplateEnemyLocationChanged(DataContext context, MapTemplateEnemyInfo templateEnemyInfo, Location srcLocation, Location destLocation)
	{
		if (srcLocation.IsValid())
		{
			MapBlockData block = GetBlock(srcLocation);
			if (block.RemoveTemplateEnemy(templateEnemyInfo))
			{
				SetBlockData(context, block);
			}
		}
		if (destLocation.IsValid())
		{
			MapBlockData block2 = GetBlock(destLocation);
			templateEnemyInfo.BlockId = destLocation.BlockId;
			block2.AddTemplateEnemy(new MapTemplateEnemyInfo(templateEnemyInfo.TemplateId, destLocation.BlockId, templateEnemyInfo.SourceAdventureBlockId, templateEnemyInfo.Duration));
			SetBlockData(context, block2);
		}
	}

	public int GetStationUnlockedRegularAreaCount()
	{
		int num = 0;
		for (short num2 = 0; num2 < 45; num2++)
		{
			if (_areas[num2].StationUnlocked)
			{
				num++;
			}
		}
		return num;
	}

	[SingleValueCollectionDependency(19, new ushort[] { 163 })]
	private void CalcLoongLocations(List<LoongLocationData> value)
	{
		value.Clear();
		foreach (LoongInfo value2 in DomainManager.Extra.FiveLoongDict.Values)
		{
			if (!value2.IsDisappear)
			{
				value.Add(new LoongLocationData(value2));
			}
		}
	}

	[SingleValueCollectionDependency(19, new ushort[] { 194 })]
	private void CalcFleeBeasts(List<Location> value)
	{
		value.Clear();
		foreach (Location allFleeBeastLocation in DomainManager.Extra.GetAllFleeBeastLocations())
		{
			if (!value.Contains(allFleeBeastLocation))
			{
				value.Add(allFleeBeastLocation);
			}
		}
	}

	[SingleValueCollectionDependency(19, new ushort[] { 194 })]
	private void CalcFleeLoongs(List<Location> value)
	{
		value.Clear();
		foreach (Location allFleeJiaoLoongLocation in DomainManager.Extra.GetAllFleeJiaoLoongLocations())
		{
			if (!value.Contains(allFleeJiaoLoongLocation))
			{
				value.Add(allFleeJiaoLoongLocation);
			}
		}
	}

	[SingleValueCollectionDependency(19, new ushort[] { 195 })]
	private void CalcAlterSettlementLocations(List<Location> value)
	{
		value.Clear();
		foreach (short allAlteredSettlementId in DomainManager.Extra.GetAllAlteredSettlementIds())
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(allAlteredSettlementId);
			value.Add(settlement.GetLocation());
		}
	}

	[SingleValueCollectionDependency(19, new ushort[] { 281 })]
	[SingleValueCollectionDependency(1, new ushort[] { 1 })]
	[SingleValueCollectionDependency(1, new ushort[] { 27 })]
	[ObjectCollectionDependency(4, 0, new ushort[] { 47 }, Condition = InfluenceCondition.CharIsTaiwu)]
	[SingleValueCollectionDependency(5, new ushort[] { 46 })]
	private void CalcVisibleMapPickups(List<MapElementPickupDisplayData> value)
	{
		value.Clear();
		foreach (MapPickupCollection value2 in DomainManager.Extra.PickupDict.Values)
		{
			if (value2 != null && value2.Count > 0)
			{
				List<MapPickup> list = value2.Where(MapPickupHelper.IsVisible).ToList();
				list.Sort(MapPickupHelper.CompareVisiblePickups);
				value.AddRange(list.Select(GetPickupDisplayData));
			}
		}
	}

	public void AddHunterAnimal(DataContext context, short areaId, short blockId, short animalId)
	{
		_hunterAnimalsCache.Add(new HunterAnimalKey(areaId, blockId, animalId));
		SetHunterAnimalsCache(_hunterAnimalsCache, context);
	}

	public void MoveHunterAnimal(DataContext context, Location before, Location after, short animalId)
	{
		HunterAnimalKey item = new HunterAnimalKey(before.AreaId, before.BlockId, animalId);
		if (_hunterAnimalsCache.Contains(item))
		{
			_hunterAnimalsCache.Remove(item);
			HunterAnimalKey item2 = new HunterAnimalKey(after.AreaId, after.BlockId, animalId);
			_hunterAnimalsCache.Add(item2);
			SetHunterAnimalsCache(_hunterAnimalsCache, context);
		}
	}

	public void RemoveHunterAnimal(DataContext context, Location location, short animalId)
	{
		HunterAnimalKey item = new HunterAnimalKey(location.AreaId, location.BlockId, animalId);
		if (_hunterAnimalsCache.Contains(item))
		{
			_hunterAnimalsCache.Remove(item);
			SetHunterAnimalsCache(_hunterAnimalsCache, context);
		}
	}

	public void ClearHunterAnim(DataContext context)
	{
		if (_hunterAnimalsCache.Count != 0)
		{
			_hunterAnimalsCache.Clear();
			SetHunterAnimalsCache(_hunterAnimalsCache, context);
		}
	}

	private static void AddAnimalProfessionSeniority(DataContext context, Location destLocation)
	{
		if (!DomainManager.Extra.TryGetElement_TaiwuProfessions(1, out var _) || !destLocation.IsValid() || !DomainManager.Extra.TryGetAnimalsByLocation(destLocation, out var animals))
		{
			return;
		}
		ProfessionFormulaItem professionFormulaItem = ProfessionFormula.Instance[8];
		foreach (GameData.Domains.Character.Animal item in animals)
		{
			if (DomainManager.Extra.TryTriggerAddSeniorityPoint(context, professionFormulaItem.TemplateId, item.Id))
			{
				sbyte consummateLevel = Config.Character.Instance[item.CharacterTemplateId].ConsummateLevel;
				int baseDelta = professionFormulaItem.Calculate(consummateLevel);
				DomainManager.Extra.ChangeProfessionSeniority(context, 1, baseDelta);
			}
		}
	}

	[DomainMethod]
	public AreaDisplayData[] GetAllAreaDisplayData()
	{
		AreaDisplayData[] array = new AreaDisplayData[135];
		for (short num = 0; num < 135; num++)
		{
			array[num] = GetAreaDisplayData(num);
		}
		return array;
	}

	public AreaDisplayData GetAreaDisplayData(short areaId)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		List<GameData.Domains.Character.Character> list = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		list.Clear();
		MapCharacterFilter.FindInfected(CharacterMatchers.MatchCompletelyInfected, list, areaId);
		int count = list.Count;
		list.Clear();
		MapCharacterFilter.Find(CharacterMatchers.MatchNotTaiwuOwnedLegendaryBook, list, areaId, includeInfected: true);
		int count2 = list.Count;
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list);
		bool flag = DomainManager.Map.IsContainsPurpleBamboo(areaId);
		AreaAdventureData element_AdventureAreas = DomainManager.Adventure.GetElement_AdventureAreas(areaId);
		return new AreaDisplayData
		{
			IsBroken = (areaId >= 45),
			AnyPurpleBamboo = flag,
			_loongStatusInternal = BoolArray8.op_Implicit(DomainManager.Extra.GetAreaLoongStatus(areaId)),
			AnyFleeBeast = DomainManager.Map.GetFleeBeasts().Any((Location x) => x.AreaId == areaId),
			AnyFleeLoongson = DomainManager.Map.GetFleeLoongs().Any((Location x) => x.AreaId == areaId),
			InfectedCount = count,
			LegendaryCount = count2,
			BrokenLevel = DomainManager.Map.QueryAreaBrokenLevel(areaId),
			PurpleBambooTemplateIds = (flag ? new List<short>(DomainManager.Map.IterAreaPurpleBambooTemplateIds(areaId)) : null),
			AdventureTemplates = new List<short>(from adv in element_AdventureAreas.AdventureSites.Values
				where adv.SiteState >= 1
				select adv.TemplateId),
			PastLifeRelationCount = DomainManager.Extra.GetAreaPastLifeRelationCount(areaId),
			HasSectZhujianSpecialMerchant = (DomainManager.Taiwu.GetAreaMerchantInfo(areaId).merchantSourceType == OpenShopEventArguments.EMerchantSourceType.SpecialBuilding)
		};
	}

	[DomainMethod]
	public List<MapBlockDisplayData> GetBlockDisplayDataInArea(short areaId)
	{
		Span<MapBlockData> areaBlocks = GetAreaBlocks(areaId);
		_blockDisplayDataCache.Clear();
		_blockDisplayDataCache.EnsureCapacity(areaBlocks.Length);
		List<GameData.Domains.Character.Character> list = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			MapBlockDisplayData mapBlockDisplayData = GetMapBlockDisplayData(mapBlockData.GetLocation(), -1, list);
			_blockDisplayDataCache.Add(mapBlockDisplayData);
		}
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list);
		return _blockDisplayDataCache;
	}

	private MapBlockDisplayData GetMapBlockDisplayData(Location location, int professionId, List<GameData.Domains.Character.Character> characters)
	{
		MapBlockDisplayData result = new MapBlockDisplayData
		{
			TreasureExpect = DomainManager.Extra.FindTreasureExpect(location),
			ProfessionId = professionId,
			Count0 = 0,
			Count1 = 0,
			Count2 = 0
		};
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		MapBlockData block = DomainManager.Map.GetBlock(location.AreaId, location.BlockId);
		switch (professionId)
		{
		case 0:
			result.Count0 = (block.Destroyed ? 1 : 0);
			break;
		case 1:
			result.Count0 = DomainManager.Extra.QueryAnimalCount(location);
			break;
		case 5:
			QueryCharacters(expectInfected: false);
			foreach (GameData.Domains.Character.Character character in characters)
			{
				List<short> featureIds = character.GetFeatureIds();
				if (featureIds.Contains(217))
				{
					result.Count0++;
				}
				else if (featureIds.Contains(218))
				{
					result.Count1++;
				}
			}
			break;
		case 10:
			foreach (int item in IterCharIds())
			{
				HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(item, 32768);
				if (relatedCharIds.Contains(taiwuCharId))
				{
					result.Count0++;
				}
			}
			break;
		case 12:
			QueryCharacters();
			foreach (GameData.Domains.Character.Character character2 in characters)
			{
				switch (character2.GetBehaviorType())
				{
				case 3:
					result.Count0++;
					break;
				case 4:
					result.Count1++;
					break;
				}
			}
			break;
		case 13:
			QueryCharacters();
			foreach (GameData.Domains.Character.Character character3 in characters)
			{
				if (character3.GetInjuries().HasAnyInjury())
				{
					result.Count0++;
				}
				PoisonInts poisoned = character3.GetPoisoned();
				if (poisoned.IsNonZero())
				{
					result.Count1++;
				}
				if (character3.GetDisorderOfQi() > 0)
				{
					result.Count2++;
				}
			}
			break;
		case 17:
			foreach (int item2 in IterCharIds(expectInfected: false))
			{
				if (ProfessionSkillHandle.DukeSkill_CheckCharacterHasTitle(item2))
				{
					result.Count0++;
				}
			}
			break;
		}
		return result;
		IEnumerable<int> IterCharIds(bool expectInfected = true)
		{
			if (block.CharacterSet != null)
			{
				foreach (int item3 in block.CharacterSet)
				{
					yield return item3;
				}
				if (!(block.InfectedCharacterSet == null || expectInfected))
				{
					foreach (int item4 in block.InfectedCharacterSet)
					{
						yield return item4;
					}
				}
			}
		}
		void QueryCharacters(bool expectInfected = true)
		{
			characters.Clear();
			foreach (int item5 in IterCharIds(expectInfected))
			{
				if (DomainManager.Character.TryGetElement_Objects(item5, out var element))
				{
					characters.Add(element);
				}
			}
		}
	}

	[DomainMethod]
	public int[] GetAllAreaCompletelyInfectedCharCount()
	{
		List<GameData.Domains.Character.Character> list = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		int[] array = new int[135];
		for (short num = 0; num < 135; num++)
		{
			list.Clear();
			MapCharacterFilter.FindInfected(CharacterMatchers.MatchCompletelyInfected, list, num);
			array[num] = list.Count;
		}
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list);
		return array;
	}

	[DomainMethod]
	public int[] GetAllStateCompletelyInfectedCharCount()
	{
		List<GameData.Domains.Character.Character> list = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		int[] array = new int[15];
		for (sbyte b = 0; b < 15; b++)
		{
			list.Clear();
			MapCharacterFilter.FindStateInfected(CharacterMatchers.MatchCompletelyInfected, list, b);
			array[b] = list.Count;
		}
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list);
		return array;
	}

	[DomainMethod]
	public Dictionary<TravelRouteKey, TravelRoute> GetTravelRoutesInState(sbyte stateId)
	{
		if (stateId < 0)
		{
			return null;
		}
		Dictionary<TravelRouteKey, TravelRoute> dictionary = new Dictionary<TravelRouteKey, TravelRoute>();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		Dictionary<TravelRouteKey, TravelRoute> dictionary2 = (DomainManager.World.GetWorldFunctionsStatus(4) ? _travelRouteDict : _bornStateTravelRouteDict);
		GetAllAreaInState(stateId, list);
		for (int i = 0; i < list.Count - 1; i++)
		{
			for (int j = i + 1; j < list.Count; j++)
			{
				TravelRouteKey key = new TravelRouteKey(list[i], list[j]);
				dictionary.Add(key, dictionary2[key]);
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		return dictionary;
	}

	public Location CrossAreaTravelInfoToLocation(CrossAreaMoveInfo crossAreaMoveInfos)
	{
		if (crossAreaMoveInfos.CostedDays == 0)
		{
			return new Location(crossAreaMoveInfos.FromAreaId, crossAreaMoveInfos.FromBlockId);
		}
		int num = 0;
		for (int i = 0; i < crossAreaMoveInfos.Route.CostList.Count; i++)
		{
			short num2 = crossAreaMoveInfos.Route.CostList[i];
			num += num2;
			if (num >= crossAreaMoveInfos.CostedDays)
			{
				short num3 = crossAreaMoveInfos.Route.AreaList[i];
				MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(num3);
				return new Location(num3, element_Areas.StationBlockId);
			}
		}
		return Location.Invalid;
	}

	public void GetAllAreaInState(sbyte stateId, List<short> areaList)
	{
		SharedMethods.GetAreaListInState(stateId, areaList);
	}

	public void GetAllRegularAreaInState(sbyte stateId, List<short> areaList)
	{
		SharedMethods.GetRegularAreaListInState(stateId, areaList);
	}

	public void GetAllBrokenAreaInState(sbyte stateId, List<short> areaList)
	{
		SharedMethods.GetBrokenAreaListInState(stateId, areaList);
	}

	public byte GetAreaSize(short areaId)
	{
		if (areaId < 45 || areaId >= 135)
		{
			MapAreaItem config = _areas[areaId].GetConfig();
			short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			if (taiwuVillageSettlementId >= 0 && DomainManager.Organization.GetSettlement(taiwuVillageSettlementId).GetLocation().AreaId == areaId)
			{
				return (byte)GlobalConfig.Instance.TaiwuVillageForceAreaSize;
			}
			return config.Size;
		}
		return 5;
	}

	public bool IsAreaBroken(short areaId)
	{
		int num = 45;
		int num2 = 135;
		return areaId >= num && areaId < num2;
	}

	public short GetMainSettlementMainBlockId(short areaId)
	{
		MapAreaData mapAreaData = _areas[areaId];
		SettlementInfo settlementInfo = mapAreaData.SettlementInfos[0];
		return settlementInfo.BlockId;
	}

	public short GetNearestSettlementIdByLocation(Location location)
	{
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(location.AreaId);
		byte areaSize = DomainManager.Map.GetAreaSize(location.AreaId);
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(location.BlockId, areaSize);
		int num = int.MaxValue;
		short result = -1;
		for (int i = 0; i < element_Areas.SettlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = element_Areas.SettlementInfos[i];
			int manhattanDistance = byteCoordinate.GetManhattanDistance(ByteCoordinate.IndexToCoordinate(settlementInfo.BlockId, areaSize));
			if (manhattanDistance < num)
			{
				num = manhattanDistance;
				result = settlementInfo.SettlementId;
			}
		}
		return result;
	}

	public ByteCoordinate GetWorldPos(short areaId)
	{
		sbyte[] worldMapPos = _areas[areaId].GetConfig().WorldMapPos;
		return new ByteCoordinate((byte)worldMapPos[0], (byte)worldMapPos[1]);
	}

	public sbyte GetStateIdByStateTemplateId(short stateTemplateId)
	{
		return (sbyte)(stateTemplateId - 1);
	}

	public sbyte GetStateIdByAreaId(short areaId)
	{
		return GetStateIdByStateTemplateId(_areas[areaId].GetConfig().StateID);
	}

	public sbyte GetStateTemplateIdByAreaId(short areaId)
	{
		return _areas[areaId].GetConfig().StateID;
	}

	public (string stateName, string areaName) GetStateAndAreaNameByAreaId(short areaId)
	{
		MapAreaItem config = _areas[areaId].GetConfig();
		string name = config.Name;
		string name2 = MapState.Instance[config.StateID].Name;
		return (stateName: name2, areaName: name);
	}

	public short GetAreaIdByAreaTemplateId(short areaTemplateId)
	{
		for (short num = 0; num < _areas.Length; num++)
		{
			if (_areas[num].GetTemplateId() == areaTemplateId)
			{
				return num;
			}
		}
		return -1;
	}

	public void GetEdgeBlockList(short areaId, List<short> blockIdList, bool excludeTravelBlock = false, bool strictSelect = true)
	{
		HashSet<short> hashSet = ObjectPool<HashSet<short>>.Instance.Get();
		hashSet.Clear();
		hashSet.Add(126);
		GetEdgeBlockList(areaId, blockIdList, null, excludeTravelBlock, strictSelect);
		ObjectPool<HashSet<short>>.Instance.Return(hashSet);
	}

	public void GetEdgeBlockList(short areaId, List<short> blockIdList, ISet<short> edgeTemplates, bool excludeTravelBlock = false, bool strictSelect = true, bool containsBigBlock = false)
	{
		byte areaSize = GetAreaSize(areaId);
		Span<MapBlockData> areaBlocks = GetAreaBlocks(areaId);
		int length = areaBlocks.Length;
		List<short> list = null;
		byte b = byte.MaxValue;
		byte b2 = byte.MaxValue;
		byte b3 = 0;
		byte b4 = 0;
		if (excludeTravelBlock)
		{
			list = ObjectPool<List<short>>.Instance.Get();
			list.Clear();
		}
		if (strictSelect)
		{
			for (byte b5 = 0; b5 < areaSize; b5++)
			{
				for (byte b6 = 0; b6 < areaSize; b6++)
				{
					MapBlockData mapBlockData = areaBlocks[ByteCoordinate.CoordinateToIndex(new ByteCoordinate(b5, b6), areaSize)];
					if (mapBlockData.IsPassable() && !containsBigBlock && mapBlockData.RootBlockId < 0 && mapBlockData.GroupBlockList == null)
					{
						b = Math.Min(b5, b);
						b2 = Math.Min(b6, b2);
						b3 = Math.Max(b5, b3);
						b4 = Math.Max(b6, b4);
					}
				}
			}
		}
		blockIdList.Clear();
		for (short num = 0; num < length; num++)
		{
			MapBlockData mapBlockData2 = areaBlocks[num];
			if (mapBlockData2.IsPassable() && (containsBigBlock || mapBlockData2.RootBlockId < 0) && (mapBlockData2.GroupBlockList == null || mapBlockData2.GroupBlockList.Count <= 0))
			{
				ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(num, areaSize);
				bool flag = byteCoordinate.X == 0 || byteCoordinate.X == areaSize - 1 || byteCoordinate.Y == 0 || byteCoordinate.Y == areaSize - 1;
				if (!flag)
				{
					if (strictSelect)
					{
						bool flag2 = byteCoordinate.X < areaSize / 2;
						bool flag3 = !flag2;
						bool flag4 = byteCoordinate.Y < areaSize / 2;
						bool flag5 = !flag4;
						int num2 = (flag2 ? byteCoordinate.X : (areaSize - byteCoordinate.X));
						int num3 = (flag4 ? byteCoordinate.Y : (areaSize - byteCoordinate.Y));
						if (num2 < num3)
						{
							flag4 = (flag5 = false);
						}
						else if (num2 > num3)
						{
							flag2 = (flag3 = false);
						}
						flag = (!flag2 || byteCoordinate.X == b) && (!flag3 || byteCoordinate.X == b3) && (!flag4 || byteCoordinate.Y == b2) && (!flag5 || byteCoordinate.Y == b4);
					}
					else
					{
						flag = edgeTemplates.Contains(areaBlocks[(short)(num - 1)].TemplateId) || edgeTemplates.Contains(areaBlocks[(short)(num + 1)].TemplateId) || edgeTemplates.Contains(areaBlocks[(short)(num - areaSize)].TemplateId) || edgeTemplates.Contains(areaBlocks[(short)(num + areaSize)].TemplateId);
					}
				}
				if (flag && (!excludeTravelBlock || !list.Contains(num)))
				{
					blockIdList.Add(num);
				}
			}
		}
		if (excludeTravelBlock)
		{
			ObjectPool<List<short>>.Instance.Return(list);
		}
	}

	public short GetRandomSettlementId(sbyte stateId, IRandomSource random, bool containsMainCityAndSect = false)
	{
		int num = 3;
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		short result = -1;
		list.Clear();
		for (int i = 0; i < num; i++)
		{
			short num2 = (short)(stateId * num + i);
			SettlementInfo[] settlementInfos = _areas[num2].SettlementInfos;
			for (int j = 0; j < settlementInfos.Length; j++)
			{
				SettlementInfo settlementInfo = settlementInfos[j];
				if (settlementInfo.SettlementId >= 0 && (containsMainCityAndSect || GetBlock(num2, settlementInfo.BlockId).BlockType == EMapBlockType.Town))
				{
					list.Add(settlementInfo.SettlementId);
				}
			}
		}
		if (list.Count > 0)
		{
			result = list[random.Next(0, list.Count)];
		}
		return result;
	}

	public short GetRandomStateSettlementId(IRandomSource random, sbyte stateId, bool containsMainCity = false, bool containsSect = false)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		GetStateSettlementIds(stateId, list, containsMainCity, containsSect);
		if (list.Count == 0)
		{
			return -1;
		}
		short random2 = list.GetRandom(random);
		ObjectPool<List<short>>.Instance.Return(list);
		return random2;
	}

	public void GetStateSettlementIds(sbyte stateId, List<short> settlementIds, bool containsMainCity = false, bool containsSect = false)
	{
		settlementIds.Clear();
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			short num2 = (short)(stateId * num + i);
			SettlementInfo[] settlementInfos = _areas[num2].SettlementInfos;
			for (int j = 0; j < settlementInfos.Length; j++)
			{
				SettlementInfo settlementInfo = settlementInfos[j];
				if (settlementInfo.SettlementId < 0)
				{
					continue;
				}
				MapBlockData block = GetBlock(num2, settlementInfo.BlockId);
				switch (block.BlockType)
				{
				case EMapBlockType.Town:
					settlementIds.Add(settlementInfo.SettlementId);
					break;
				case EMapBlockType.City:
					if (containsMainCity)
					{
						settlementIds.Add(settlementInfo.SettlementId);
					}
					break;
				case EMapBlockType.Sect:
					if (containsSect)
					{
						settlementIds.Add(settlementInfo.SettlementId);
					}
					break;
				}
			}
		}
	}

	public void GetAreaSettlementIds(short areaId, List<short> settlementIds, bool containsMainCity = false, bool containsSect = false)
	{
		settlementIds.Clear();
		SettlementInfo[] settlementInfos = _areas[areaId].SettlementInfos;
		for (int i = 0; i < settlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = settlementInfos[i];
			if (settlementInfo.SettlementId < 0)
			{
				continue;
			}
			MapBlockData block = GetBlock(areaId, settlementInfo.BlockId);
			switch (block.BlockType)
			{
			case EMapBlockType.Town:
				settlementIds.Add(settlementInfo.SettlementId);
				break;
			case EMapBlockType.City:
				if (containsMainCity)
				{
					settlementIds.Add(settlementInfo.SettlementId);
				}
				break;
			case EMapBlockType.Sect:
				if (containsSect)
				{
					settlementIds.Add(settlementInfo.SettlementId);
				}
				break;
			}
		}
	}

	public CrossAreaMoveInfo CalcAreaTravelRoute(GameData.Domains.Character.Character character, short fromAreaId, short fromBlockId, short toAreaId)
	{
		if (fromAreaId == toAreaId)
		{
			throw new ArgumentException("fromAreaId cannot equals toAreaId");
		}
		bool flag = fromAreaId > toAreaId;
		TravelRouteKey key = new TravelRouteKey(flag ? toAreaId : fromAreaId, flag ? fromAreaId : toAreaId);
		ItemKey itemKey = character.GetEquipment()[11];
		int num = 0;
		Dictionary<TravelRouteKey, TravelRoute> travelRouteDict = _travelRouteDict;
		TravelRoute travelRoute = new TravelRoute(travelRouteDict[key]);
		if (flag)
		{
			travelRoute.PosList.Reverse();
			travelRoute.AreaList.Reverse();
			travelRoute.CostList.Reverse();
		}
		travelRoute.AreaList.RemoveAt(0);
		if (itemKey.IsValid())
		{
			GameData.Domains.Item.Carrier element_Carriers = DomainManager.Item.GetElement_Carriers(itemKey.Id);
			if (element_Carriers.GetCurrDurability() > 0)
			{
				num = element_Carriers.GetTravelTimeReduction();
			}
		}
		for (int i = 0; i < travelRoute.CostList.Count; i++)
		{
			int num2 = travelRoute.CostList[i];
			num2 -= num2 * num / 100;
			num2 = Math.Max(num2, 1);
			travelRoute.CostList[i] = (short)num2;
		}
		return new CrossAreaMoveInfo
		{
			FromAreaId = fromAreaId,
			FromBlockId = fromBlockId,
			ToAreaId = toAreaId,
			Route = travelRoute
		};
	}

	public bool AllowCrossAreaTravel(short fromAreaId, short toAreaId)
	{
		if (fromAreaId >= 135 || toAreaId >= 135)
		{
			return false;
		}
		bool flag = fromAreaId > toAreaId;
		TravelRouteKey travelRouteKey = new TravelRouteKey(flag ? toAreaId : fromAreaId, flag ? fromAreaId : toAreaId);
		if (!_travelRouteDict.ContainsKey(travelRouteKey))
		{
			Logger.Warn($"Invalid travel route {travelRouteKey}: path not exists.");
			return false;
		}
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		return (fromAreaId != taiwuVillageLocation.AreaId && toAreaId != taiwuVillageLocation.AreaId) || (DomainManager.Map.GetElement_Areas(taiwuVillageLocation.AreaId).StationUnlocked && DomainManager.World.GetWorldFunctionsStatus(4));
	}

	public int GetTotalTimeCost(GameData.Domains.Character.Character character, short fromAreaId, short toAreaId)
	{
		if (fromAreaId == toAreaId)
		{
			return 0;
		}
		if (!AllowCrossAreaTravel(fromAreaId, toAreaId))
		{
			return int.MaxValue;
		}
		bool flag = fromAreaId > toAreaId;
		TravelRouteKey key = new TravelRouteKey(flag ? toAreaId : fromAreaId, flag ? fromAreaId : toAreaId);
		ItemKey itemKey = character.GetEquipment()[11];
		int num = 0;
		if (itemKey.IsValid())
		{
			GameData.Domains.Item.Carrier element_Carriers = DomainManager.Item.GetElement_Carriers(itemKey.Id);
			if (element_Carriers.GetCurrDurability() > 0)
			{
				num = element_Carriers.GetTravelTimeReduction();
			}
		}
		TravelRoute travelRoute = _travelRouteDict[key];
		int num2 = 0;
		for (int i = 0; i < travelRoute.CostList.Count; i++)
		{
			num2 += Math.Max(travelRoute.CostList[i] * (100 - num) / 100, 1);
		}
		return num2;
	}

	public static void ParallelUpdateBrokenBlockOnMonthChange(DataContext context, int areaIdInt)
	{
		ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaIdInt);
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			if (mapBlockData.CountDown())
			{
				parallelModificationsRecorder.RecordType(ParallelModificationType.UpdateBrokenArea);
				parallelModificationsRecorder.RecordParameterClass(mapBlockData);
			}
		}
	}

	public static void ParallelUpdateOnMonthChange(DataContext context, int areaIdInt)
	{
		short num = (short)areaIdInt;
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(num);
		ParallelMapAreaModification parallelMapAreaModification = new ParallelMapAreaModification();
		List<sbyte> recoverResourceType = Month.Instance[DomainManager.World.GetCurrMonthInYear()].RecoverResourceType;
		int maxRecoverPercent = 14 - DomainManager.World.GetWorldResourceAmountType() * 3;
		Dictionary<sbyte, List<Location>> resourceRecoverSpeedUpDict = DomainManager.Taiwu.ResourceRecoverSpeedUpDict;
		List<(short, short)> obj = context.AdvanceMonthRelatedData.WeightTable.Occupy();
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(num);
		for (short num2 = 0; num2 < areaBlocks.Length; num2++)
		{
			MapBlockData mapBlockData = areaBlocks[num2];
			Location location = new Location(num, num2);
			if (mapBlockData.IsPassable())
			{
				if (!mapBlockData.Destroyed || !DomainManager.Extra.IsMapBlockRecoveryLocked(location))
				{
					if (mapBlockData.Destroyed)
					{
						OfflineRecoverBlock(mapBlockData);
					}
					OfflineRecoverResource(context.Random, mapBlockData, location, maxRecoverPercent, recoverResourceType, resourceRecoverSpeedUpDict);
				}
				if (mapBlockData.Items != null)
				{
					mapBlockData.DestroyItems(parallelMapAreaModification.DestroyedUniqueItems);
				}
				short maxMalice = mapBlockData.GetMaxMalice();
				bool flag = maxMalice > 0 && mapBlockData.Malice * 100 / maxMalice >= 25 && mapBlockData.RootBlockId < 0 && mapBlockData.GroupBlockList == null;
				if (mapBlockData.GetConfig().SubType == EMapBlockSubType.DLCLoong)
				{
					flag = false;
				}
				if (flag)
				{
					obj.Add((num2, (short)(mapBlockData.Malice * 100 / maxMalice)));
				}
				mapBlockData.CountDown();
			}
		}
		if (num < 45)
		{
			TriggerDisasters(context.Random, num, obj, parallelMapAreaModification);
		}
		context.AdvanceMonthRelatedData.WeightTable.Release(ref obj);
		for (int i = 0; i < element_Areas.SettlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = element_Areas.SettlementInfos[i];
			if (settlementInfo.SettlementId >= 0 && !parallelMapAreaModification.SettlementDict.ContainsKey(settlementInfo.SettlementId))
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
				if (settlement.GetSafety() >= settlement.GetMaxSafety() / 2)
				{
					parallelMapAreaModification.SettlementDict.Add(settlementInfo.SettlementId, new ParallelSettlementModification(settlement.GetCulture(), settlement.GetSafety(), Math.Min(settlement.GetPopulation() + 1, settlement.GetMaxPopulation())));
				}
			}
		}
		ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
		parallelMapAreaModification.AreaId = num;
		parallelModificationsRecorder.RecordType(ParallelModificationType.UpdateMapArea);
		parallelModificationsRecorder.RecordParameterClass(parallelMapAreaModification);
	}

	private unsafe static void OfflineRecoverBlock(MapBlockData block)
	{
		int num = 0;
		int num2 = 0;
		for (sbyte b = 0; b < 6; b++)
		{
			num += block.CurrResources.Items[b];
			num2 += block.MaxResources.Items[b];
		}
		if (num >= num2 / 2)
		{
			block.Destroyed = false;
		}
	}

	private unsafe static void OfflineRecoverResource(IRandomSource random, MapBlockData block, Location location, int maxRecoverPercent, List<sbyte> recoverResourceType, Dictionary<sbyte, List<Location>> resourceSpeedUpDict)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		int buildingBlockEffect = DomainManager.Building.GetBuildingBlockEffect(location, EBuildingScaleEffect.MapResourceRegenBonus);
		for (int i = 0; i < recoverResourceType.Count; i++)
		{
			sbyte b = recoverResourceType[i];
			int num = Math.Max(block.MaxResources.Items[b] * maxRecoverPercent / 100, 1);
			int num2 = random.Next(num + 1);
			num2 *= CValuePercentBonus.op_Implicit(buildingBlockEffect);
			num2 = num2 * GameData.Domains.World.SharedMethods.GetGainResourcePercent(0) / 100;
			if (resourceSpeedUpDict.TryGetValue(b, out var value) && value.Contains(location))
			{
				num2 *= 3;
			}
			block.CurrResources.Items[b] = (short)Math.Min(block.CurrResources.Items[b] + num2, block.MaxResources.Items[b]);
		}
	}

	private static void TriggerDisasters(IRandomSource random, short areaId, List<(short, short)> potentialDisasterBlocks, ParallelMapAreaModification mod)
	{
		potentialDisasterBlocks.Sort(((short, short) a, (short, short) tuple) => tuple.Item2.CompareTo(a.Item2));
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		for (int num = 0; num < potentialDisasterBlocks.Count; num++)
		{
			short item = potentialDisasterBlocks[num].Item1;
			MapBlockData mapBlockData = areaBlocks[item];
			short maxMalice = mapBlockData.GetMaxMalice();
			int num2 = mapBlockData.Malice * 100 / maxMalice;
			List<MapBlockData> groupBlockList = mapBlockData.GroupBlockList;
			if ((groupBlockList != null && groupBlockList.Count > 0) || mapBlockData.RootBlockId >= 0)
			{
				continue;
			}
			for (int num3 = 0; num3 < GlobalConfig.Instance.DisasterTriggerCurrBlockThresholds.Length; num3++)
			{
				sbyte b = GlobalConfig.Instance.DisasterTriggerCurrBlockThresholds[num3];
				if (num2 >= b)
				{
					sbyte b2 = GlobalConfig.Instance.DisasterTriggerRanges[num3];
					if (b2 > 0)
					{
						DomainManager.Map.GetRealNeighborBlocks(areaId, item, list, b2);
					}
					else
					{
						list.Clear();
					}
					int num4 = GetMalicePercentageSum(list) + num2;
					short num5 = GlobalConfig.Instance.DisasterTriggerNeighborSumThresholds[num3];
					if (num4 >= num5)
					{
						DomainManager.Map.GetRealNeighborBlocks(areaId, item, list, GlobalConfig.Instance.DisasterTriggerRanges[^1]);
						TriggerDisaster(random, areaId, item, list, mod);
						break;
					}
				}
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}

	private static int GetMalicePercentageSum(List<MapBlockData> blocks)
	{
		int num = 0;
		foreach (MapBlockData block in blocks)
		{
			short maxMalice = block.GetMaxMalice();
			if (maxMalice > 0)
			{
				num += block.Malice * 100 / maxMalice;
			}
		}
		return num;
	}

	private unsafe static void TriggerDisaster(IRandomSource random, short areaId, short blockId, List<MapBlockData> neighborBlocks, ParallelMapAreaModification mod)
	{
		Location key = new Location(areaId, blockId);
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(areaId);
		MapBlockData block = DomainManager.Map.GetBlock(key);
		mod.DisasterBlocks.Add(blockId);
		short num = DomainManager.Adventure.GenerateDisasterAdventureId(random, block);
		if (num >= 0)
		{
			mod.DisasterAdventureId.Add(blockId, num);
		}
		block.Destroyed = true;
		block.Malice = 0;
		foreach (MapBlockData neighborBlock in neighborBlocks)
		{
			neighborBlock.Malice = 0;
		}
		for (sbyte b = 0; b < 6; b++)
		{
			block.CurrResources.Items[b] = 0;
		}
		block.DestroyItemsDirect(mod.DestroyedUniqueItems);
		for (int i = 0; i < element_Areas.SettlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = element_Areas.SettlementInfos[i];
			if (settlementInfo.BlockId == blockId && settlementInfo.SettlementId >= 0)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
				mod.SettlementDict.Add(settlementInfo.SettlementId, new ParallelSettlementModification(0, 0, settlement.GetPopulation() * random.Next(70, 91) / 100));
				break;
			}
		}
		if (block.CharacterSet != null)
		{
			foreach (int item4 in block.CharacterSet)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item4);
				int num2 = random.Next(5);
				if (element_Objects.GetAgeGroup() != 2 || DomainManager.Taiwu.GetGroupCharIds().Contains(item4) || element_Objects.GetFeatureIds().Contains(200))
				{
					num2--;
				}
				switch (num2)
				{
				case 1:
				{
					AddOrIncreaseInjuryParams item2 = new AddOrIncreaseInjuryParams(BodyPartType.GetRandomBodyPartType(random), isInnerInjury: false, (sbyte)random.Next(1, 3));
					mod.CharInjuries.Add((element_Objects, item2));
					break;
				}
				case 2:
				{
					for (int k = 0; k < 2; k++)
					{
						AddOrIncreaseInjuryParams item3 = new AddOrIncreaseInjuryParams(BodyPartType.GetRandomBodyPartType(random), isInnerInjury: false, (sbyte)random.Next(3, 5));
						mod.CharInjuries.Add((element_Objects, item3));
					}
					break;
				}
				case 3:
				{
					for (int j = 0; j < 2; j++)
					{
						AddOrIncreaseInjuryParams item = new AddOrIncreaseInjuryParams(BodyPartType.GetRandomBodyPartType(random), isInnerInjury: false, (sbyte)random.Next(5, 7));
						mod.CharInjuries.Add((element_Objects, item));
					}
					break;
				}
				case 4:
					mod.DeadCharList.Add(item4);
					break;
				}
			}
		}
		if (block.GraveSet != null)
		{
			mod.DamageGraveList.AddRange(block.GraveSet);
		}
	}

	public void ComplementUpdateMapArea(DataContext context, ParallelMapAreaModification mod)
	{
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Span<MapBlockData> areaBlocks = GetAreaBlocks(mod.AreaId);
		int i = 0;
		for (int length = areaBlocks.Length; i < length; i++)
		{
			SetBlockData(context, areaBlocks[i]);
		}
		foreach (KeyValuePair<short, ParallelSettlementModification> item in mod.SettlementDict)
		{
			Sect element2;
			if (DomainManager.Organization.TryGetElement_CivilianSettlements(item.Key, out var element))
			{
				element.SetCulture(item.Value.Culture, context);
				element.SetSafety(item.Value.Safety, context);
				element.SetPopulation(item.Value.Population, context);
			}
			else if (DomainManager.Organization.TryGetElement_Sects(item.Key, out element2))
			{
				element2.SetCulture(item.Value.Culture, context);
				element2.SetSafety(item.Value.Safety, context);
				element2.SetPopulation(item.Value.Population, context);
			}
		}
		if (mod.DisasterBlocks.Count > 0)
		{
			Location location = new Location(mod.AreaId, -1);
			monthlyNotificationCollection.AddNaturalDisasterOccurred(location, mod.DeadCharList.Count, mod.DisasterBlocks.Count);
			List<MapBlockData> list = new List<MapBlockData>();
			foreach (short disasterBlock in mod.DisasterBlocks)
			{
				DomainManager.Map.GetRealNeighborBlocks(mod.AreaId, disasterBlock, list, 2);
				int num = context.Random.Next(GlobalConfig.Instance.GenerateXiangshuMinionAfterDisasterRangeMax) + GlobalConfig.Instance.GenerateXiangshuMinionAfterDisasterBase;
				while (num-- > 0)
				{
					if (DomainManager.Map.GetBlock(mod.AreaId, disasterBlock).BlockType != EMapBlockType.Developed || context.Random.CheckPercentProb(GlobalConfig.Instance.GenerateXiangshuMinionAfterDisasterInDevelopedBlockProbabilityPercentage))
					{
						DomainManager.Adventure.CreateTemporaryEnemiesOnValidBlocks(context, Location.Invalid, (short)Math.Clamp(298 + DomainManager.World.GetXiangshuLevel() - context.Random.Next(GlobalConfig.Instance.GenerateXiangshuMinionAfterDisasterGradeMinusMax), 298, 306), 1, list);
					}
				}
			}
		}
		GameData.Domains.Character.Character character = null;
		int j = 0;
		for (int count = mod.CharInjuries.Count; j < count; j++)
		{
			var (character2, addOrIncreaseInjuryParams) = mod.CharInjuries[j];
			character2.GetInjuries().Change(addOrIncreaseInjuryParams.BodyPartType, addOrIncreaseInjuryParams.IsInnerInjury, addOrIncreaseInjuryParams.InjuryValue);
			if (character2 != character)
			{
				character?.SetInjuries(character.GetInjuries(), context);
				character = character2;
				Location location2 = character2.GetLocation();
				if (!location2.IsValid())
				{
					location2 = character2.GetLocation();
				}
				lifeRecordCollection.AddNaturalDisasterButSurvive(character2.GetId(), currDate, location2);
			}
		}
		character?.SetInjuries(character.GetInjuries(), context);
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		for (int k = 0; k < mod.DeadCharList.Count; k++)
		{
			int num2 = mod.DeadCharList[k];
			if (num2 == taiwuCharId)
			{
				DomainManager.World.SetTaiwuDying();
			}
			if (DomainManager.Character.TryGetElement_Objects(num2, out var element3))
			{
				DomainManager.Character.MakeCharacterDead(context, element3, 6);
			}
		}
		for (int l = 0; l < mod.DamageGraveList.Count; l++)
		{
			int objectId = mod.DamageGraveList[l];
			Grave element_Graves = DomainManager.Character.GetElement_Graves(objectId);
			if (element_Graves.GetLevel() > 1)
			{
				element_Graves.SetLevel((sbyte)(element_Graves.GetLevel() - 1), context);
			}
			else
			{
				DomainManager.Character.RemoveGrave(context, element_Graves);
			}
		}
		foreach (KeyValuePair<short, short> item2 in mod.DisasterAdventureId)
		{
			Logger.Info<string, short, short>("Disaster generate material adventure {0} at ({1}, {2})", Config.Adventure.Instance[item2.Value].Name, mod.AreaId, item2.Key);
			DomainManager.Adventure.TryCreateAdventureSite(context, mod.AreaId, item2.Key, item2.Value, MonthlyActionKey.Invalid);
			monthlyNotificationCollection.AddDisasterAndPreciousMaterial(new Location(mod.AreaId, item2.Key));
		}
		DomainManager.Item.RemoveItems(context, mod.DestroyedUniqueItems);
	}

	[Obsolete]
	public void CheckAnimalAttackTaiwuOnAdvanceMonth(IRandomSource randomSource)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid() || !_animalPlaceData.CheckIndex(location.AreaId))
		{
			return;
		}
		AnimalAreaData element_AnimalAreaData = DomainManager.Extra.GetElement_AnimalAreaData(location.AreaId);
		if (element_AnimalAreaData.BlockAnimalCharacterTemplateIdList == null || !element_AnimalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(location.BlockId, out var value) || value.Count <= 0)
		{
			return;
		}
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.AddRange(value);
		for (short num = (short)(list.Count - 1); num >= 0; num--)
		{
			if (!Config.Character.Instance[list[num]].RandomAnimalAttack)
			{
				list.RemoveAt(num);
			}
		}
		if (list.Count > 0)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddRandomAnimalAttack(location, list.Max((short a, short b) => Config.Character.Instance[a].ConsummateLevel.CompareTo(Config.Character.Instance[b].ConsummateLevel)));
		}
		else
		{
			ObjectPool<List<short>>.Instance.Return(list);
		}
	}

	[Obsolete]
	public int CalcAnimalCountInArea(short areaId)
	{
		return CalcAnimalCountInArea(DomainManager.Extra.GetElement_AnimalAreaData(areaId));
	}

	[Obsolete]
	public int CalcAnimalCountInArea(AnimalAreaData data)
	{
		if (data == null)
		{
			return 0;
		}
		int num = 0;
		foreach (KeyValuePair<short, List<short>> blockAnimalCharacterTemplateId in data.BlockAnimalCharacterTemplateIdList)
		{
			num += blockAnimalCharacterTemplateId.Value.Count;
		}
		return num;
	}

	[Obsolete]
	public void UpdateAnimalAreaData(DataContext context)
	{
		ObjectPool<List<short>> instance = ObjectPool<List<short>>.Instance;
		List<short> list = instance.Get();
		for (short num = 0; num < _animalPlaceData.Length; num++)
		{
			if (!IsAreaBroken(num))
			{
				AnimalAreaData animalAreaData = DomainManager.Extra.GetElement_AnimalAreaData(num) ?? new AnimalAreaData();
				if (CalcAnimalCountInArea(animalAreaData) >= 12)
				{
					list.Clear();
					foreach (KeyValuePair<short, List<short>> blockAnimalCharacterTemplateId in animalAreaData.BlockAnimalCharacterTemplateIdList)
					{
						list.Add(blockAnimalCharacterTemplateId.Key);
					}
					CollectionUtils.Shuffle(context.Random, list);
					foreach (short item in list)
					{
						if (!animalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(item, out var value))
						{
							continue;
						}
						if (value == null || value.Count <= 0)
						{
							animalAreaData.BlockAnimalCharacterTemplateIdList.Remove(item);
							continue;
						}
						Location location = new Location(num, item);
						if (_fleeBeasts.Contains(location))
						{
							continue;
						}
						bool flag = false;
						using (List<short>.Enumerator enumerator3 = value.GetEnumerator())
						{
							if (enumerator3.MoveNext())
							{
								short current2 = enumerator3.Current;
								if (!Config.Character.Instance[num].RandomAnimalAttack)
								{
									flag = true;
								}
							}
						}
						if (flag)
						{
							continue;
						}
						RemoveBlockSingleAnimal(location, value.GetRandom(context.Random), context);
						break;
					}
				}
				else
				{
					list.Clear();
					Span<MapBlockData> areaBlocks = GetAreaBlocks(num);
					for (int i = 0; i < areaBlocks.Length; i++)
					{
						MapBlockData mapBlockData = areaBlocks[i];
						if (mapBlockData.GetConfig().Type == EMapBlockType.Wild)
						{
							list.Add(mapBlockData.BlockId);
						}
					}
					CollectionUtils.Shuffle(context.Random, list);
					short random = list.GetRandom(context.Random);
					if (!animalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(random, out var value2))
					{
						animalAreaData.BlockAnimalCharacterTemplateIdList.Add(random, value2 = new List<short>());
					}
					value2.Add(SharedConstValue.AnimalCharIdGroups.GetRandom(context.Random)[0]);
					DomainManager.Extra.SetAnimalAreaData(context, num, animalAreaData);
				}
			}
		}
		instance.Return(list);
	}

	[Obsolete]
	public void AnimalsGetInRange(Location location, int range, Dictionary<Location, List<short>> result)
	{
		if (!location.IsValid())
		{
			return;
		}
		AnimalAreaData animalAreaData = DomainManager.Extra.GetElement_AnimalAreaData(location.AreaId) ?? new AnimalAreaData();
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		list.Clear();
		GetRealNeighborBlocks(location.AreaId, location.BlockId, list, range, includeCenter: true);
		foreach (MapBlockData item in list)
		{
			Location key = new Location(item.AreaId, item.BlockId);
			if (animalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(item.BlockId, out var value) && value.Count > 0)
			{
				if (!result.TryGetValue(key, out var value2) || value2 == null)
				{
					value2 = (result[key] = new List<short>());
				}
				value2.AddRange(value);
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}

	[Obsolete]
	public void AnimalsWillAttackGetInRange(Location location, int range, Dictionary<Location, List<short>> result)
	{
		AnimalsGetInRange(location, range, result);
		Location[] array = result.Keys.ToArray();
		foreach (Location key in array)
		{
			List<short> list = result[key];
			list.RemoveAll(delegate(short id)
			{
				CharacterItem item = Config.Character.Instance.GetItem(id);
				return item != null && !item.RandomAnimalAttack;
			});
			if (list.Count <= 0)
			{
				result.Remove(key);
			}
		}
	}

	[Obsolete]
	public bool AnimalMoveToLocation(DataContext context, Location animalLocation, short animalId, Location animalTarget)
	{
		if (!animalLocation.IsValid() || !animalTarget.IsValid())
		{
			return false;
		}
		AnimalAreaData element_AnimalAreaData = DomainManager.Extra.GetElement_AnimalAreaData(animalLocation.AreaId);
		if (element_AnimalAreaData == null || !element_AnimalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(animalLocation.BlockId, out var value))
		{
			return false;
		}
		int num = value.LastIndexOf(animalId);
		if (num < 0)
		{
			return false;
		}
		AnimalAreaData animalAreaData = DomainManager.Extra.GetElement_AnimalAreaData(animalTarget.AreaId) ?? new AnimalAreaData();
		if (!animalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(animalTarget.BlockId, out var value2))
		{
			animalAreaData.BlockAnimalCharacterTemplateIdList.Add(animalTarget.BlockId, value2 = new List<short>());
		}
		value2.Add(value[num]);
		value.RemoveAt(num);
		DomainManager.Extra.SetAnimalAreaData(context, animalTarget.AreaId, element_AnimalAreaData);
		MoveHunterAnimal(context, animalLocation, animalTarget, animalId);
		ItemKey fleeCarrierByLocation = DomainManager.Extra.GetFleeCarrierByLocation(animalId, animalLocation);
		if (fleeCarrierByLocation.IsValid())
		{
			DomainManager.Extra.SetFleeCarrierLocation(context, fleeCarrierByLocation, animalTarget);
		}
		return true;
	}

	[Obsolete]
	public unsafe void AnimalCollectInBlockByHunterSkill(DataContext context, Location location, int range)
	{
		short areaId = location.AreaId;
		short blockId = location.BlockId;
		AnimalAreaData animalAreaData = DomainManager.Extra.GetElement_AnimalAreaData(areaId) ?? new AnimalAreaData();
		if (!animalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(blockId, out var value))
		{
			animalAreaData.BlockAnimalCharacterTemplateIdList.Add(blockId, value = new List<short>());
		}
		byte areaSize = GetAreaSize(areaId);
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(blockId, areaSize);
		short* ptr = stackalloc short[4];
		short* ptr2 = stackalloc short[4];
		*ptr = 1;
		ptr[1] = -1;
		ptr[2] = 0;
		ptr[3] = 0;
		*ptr2 = 0;
		ptr2[1] = 0;
		ptr2[2] = 1;
		ptr2[3] = -1;
		for (int i = 1; i <= range; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				int num = (byteCoordinate.X + ptr[j]) * i;
				int num2 = (byteCoordinate.Y + ptr2[j]) * i;
				if (num >= 0 && num2 >= 0 && num < areaSize && num2 < areaSize)
				{
					short key = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)num, (byte)num2), areaSize);
					if (!animalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(key, out var value2))
					{
						animalAreaData.BlockAnimalCharacterTemplateIdList.Add(key, value2 = new List<short>());
					}
					value.AddRange(value2);
					value2.Clear();
				}
			}
		}
		DomainManager.Extra.SetAnimalAreaData(context, areaId, animalAreaData);
	}

	[Obsolete]
	public bool AnimalGenerateInAreaByHunterSkill(DataContext context, short areaId, short animalCharId)
	{
		if (IsAreaBroken(areaId))
		{
			return false;
		}
		AnimalAreaData animalAreaData = DomainManager.Extra.GetElement_AnimalAreaData(areaId) ?? new AnimalAreaData();
		List<MapBlockData> list = new List<MapBlockData>();
		Span<MapBlockData> areaBlocks = GetAreaBlocks(areaId);
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData item = areaBlocks[i];
			list.Add(item);
		}
		list.RemoveAll((MapBlockData block) => block.BlockId == DomainManager.Taiwu.GetTaiwu().GetLocation().BlockId);
		list.RemoveAll((MapBlockData block) => !block.Visible);
		list.RemoveAll((MapBlockData block) => block.IsCityTown());
		list.RemoveAll((MapBlockData block) => !block.IsPassable());
		if (list.Count <= 0)
		{
			return false;
		}
		short blockId = list.GetRandom(context.Random).BlockId;
		if (!animalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(blockId, out var value))
		{
			animalAreaData.BlockAnimalCharacterTemplateIdList.Add(blockId, value = new List<short>());
		}
		value.Add(animalCharId);
		DomainManager.Extra.SetAnimalAreaData(context, areaId, animalAreaData);
		AddHunterAnimal(context, areaId, blockId, animalCharId);
		return true;
	}

	[Obsolete]
	public void AnimalRandomGenerateInArea(DataContext context, short areaId)
	{
		IRandomSource random = context.Random;
		AnimalAreaData animalAreaData = DomainManager.Extra.GetElement_AnimalAreaData(areaId) ?? new AnimalAreaData();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		Span<MapBlockData> areaBlocks = GetAreaBlocks(areaId);
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			if (mapBlockData.GetConfig().Type == EMapBlockType.Wild)
			{
				float num = 10f * (float)mapBlockData.CurrResources.GetSum() / (float)mapBlockData.MaxResources.GetSum();
				num -= (float)animalAreaData.BlockAnimalCharacterTemplateIdList.Sum((KeyValuePair<short, List<short>> b) => b.Value.Count);
				num = Math.Clamp(num, 0f, 100f);
				if (random.NextFloat() < num)
				{
					list.Add(mapBlockData.BlockId);
				}
			}
		}
		if (list.Count > 0)
		{
			short random2 = list.GetRandom(context.Random);
			if (!animalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(random2, out var value))
			{
				animalAreaData.BlockAnimalCharacterTemplateIdList.Add(random2, value = new List<short>());
			}
			value.Add(SharedConstValue.AnimalCharIdGroups.GetRandom(random)[0]);
			DomainManager.Extra.SetAnimalAreaData(context, areaId, animalAreaData);
		}
		ObjectPool<List<short>>.Instance.Return(list);
		DomainManager.Extra.SetAnimalAreaData(context, areaId, animalAreaData);
	}

	[Obsolete]
	public void AnimalRandomLostInArea(DataContext context, short areaId)
	{
		IRandomSource random = context.Random;
		AnimalAreaData animalAreaData = DomainManager.Extra.GetElement_AnimalAreaData(areaId) ?? new AnimalAreaData();
		Span<MapBlockData> areaBlocks = GetAreaBlocks(areaId);
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			if (animalAreaData.BlockAnimalCharacterTemplateIdList.ContainsKey(mapBlockData.BlockId))
			{
				float num = 10f * (float)mapBlockData.CurrResources.GetSum() / (float)mapBlockData.MaxResources.GetSum();
				num -= (float)animalAreaData.BlockAnimalCharacterTemplateIdList.Sum((KeyValuePair<short, List<short>> b) => b.Value.Count);
				num = Math.Clamp(100f - num, 0f, 100f);
				if (random.NextFloat() < num && animalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(mapBlockData.BlockId, out var value) && value.Count > 0)
				{
					value.RemoveAt(context.Random.Next(value.Count));
				}
			}
		}
		DomainManager.Extra.SetAnimalAreaData(context, areaId, animalAreaData);
	}

	public bool LocationHasCricket(DataContext context, Location location)
	{
		CricketPlaceData cricketPlaceData = _cricketPlaceData[location.AreaId];
		if (cricketPlaceData != null)
		{
			int num = Array.IndexOf(cricketPlaceData.CricketBlocks, location.BlockId);
			if (num >= 0)
			{
				return !cricketPlaceData.CricketTriggered[num];
			}
		}
		if (DomainManager.Extra.TryGetElement_CricketPlaceExtraData(location.AreaId, out var value) && value != null && value.ExtraMapUnits != null && value.ExtraMapUnits.TryGetValue(location.BlockId, out var _))
		{
			return true;
		}
		return false;
	}

	public void UpdateCricketPlaceData(DataContext context)
	{
		sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
		if (currMonthInYear == GlobalConfig.Instance.CricketActiveStartMonth)
		{
			for (short num = 0; num < 45; num++)
			{
				InitializeCricketPlaceData(context, num);
			}
			InitializeCricketPlaceData(context, 137);
			InitializeCricketPlaceData(context, 138);
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddCricketsAppeared();
		}
		else if (currMonthInYear == GlobalConfig.Instance.CricketActiveEndMonth)
		{
			for (short num2 = 0; num2 < 45; num2++)
			{
				SetElement_CricketPlaceData(num2, null, context);
			}
			SetElement_CricketPlaceData(137, null, context);
			SetElement_CricketPlaceData(138, null, context);
		}
		DomainManager.Extra.UpdateExtraCricketMapUnit(context);
	}

	private void InitializeCricketPlaceData(DataContext context, short areaId)
	{
		CricketPlaceData cricketPlaceData = new CricketPlaceData();
		cricketPlaceData.Init(areaId, context.Random);
		SetElement_CricketPlaceData(areaId, cricketPlaceData, context);
	}

	[Obsolete]
	private void UpdateCricketPlaceDataPerArea(DataContext context, short areaId)
	{
		sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
		if (currMonthInYear == GlobalConfig.Instance.CricketActiveStartMonth)
		{
			CricketPlaceData cricketPlaceData = new CricketPlaceData();
			cricketPlaceData.Init(areaId, context.Random);
			SetElement_CricketPlaceData(areaId, cricketPlaceData, context);
		}
		else if (currMonthInYear == GlobalConfig.Instance.CricketActiveEndMonth)
		{
			SetElement_CricketPlaceData(areaId, null, context);
		}
	}

	public void SetCricketPlaceData(DataContext context, short areaId, CricketPlaceData cricketData)
	{
		SetElement_CricketPlaceData(areaId, cricketData, context);
	}

	[DomainMethod]
	public bool TryTriggerCricketCatch(DataContext context)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (DomainManager.Extra.TryGetElement_CricketPlaceExtraData(location.AreaId, out var value) && value != null && value.ExtraMapUnits != null && value.ExtraMapUnits.TryGetValue(location.BlockId, out var _))
		{
			if (!TryCostSweepNet())
			{
				return false;
			}
			DomainManager.Extra.RemoveExtraCricketMapUnit(context, location);
			CricketLuckPointPostProcess();
			return true;
		}
		CricketPlaceData element_CricketPlaceData = GetElement_CricketPlaceData(location.AreaId);
		if (element_CricketPlaceData == null)
		{
			return false;
		}
		int num = Array.IndexOf(element_CricketPlaceData.CricketBlocks, location.BlockId);
		if (num < 0 || element_CricketPlaceData.CricketTriggered[num])
		{
			return false;
		}
		if (!TryCostSweepNet())
		{
			return false;
		}
		int num2 = num / 3;
		if (element_CricketPlaceData.RealCircketIdx[num2] == num % 3)
		{
			for (int i = 0; i < 3; i++)
			{
				element_CricketPlaceData.CricketTriggered[3 * num2 + i] = true;
			}
			SetElement_CricketPlaceData(location.AreaId, element_CricketPlaceData, context);
			return true;
		}
		CricketLuckPointPostProcess();
		element_CricketPlaceData.CricketTriggered[num] = true;
		element_CricketPlaceData.ChangePlace(location.AreaId, num);
		SetElement_CricketPlaceData(location.AreaId, element_CricketPlaceData, context);
		return false;
		void CricketLuckPointPostProcess()
		{
			DomainManager.Taiwu.SetCricketLuckPoint(DomainManager.Taiwu.GetCricketLuckPoint() + context.Random.Next(6, 13), context);
		}
		bool TryCostSweepNet()
		{
			List<(ItemKey, int)> list = new List<(ItemKey, int)>();
			CharacterItemFilterWrappers.FindByTemplateId(taiwu, 12, 9, list, searchInventory: true, searchEquipment: false);
			if (list.Count == 0)
			{
				return false;
			}
			ItemKey item = list[0].Item1;
			taiwu.RemoveInventoryItem(context, item, 1, deleteItem: false);
			return true;
		}
	}

	public bool IsCricketInLocation(Location location)
	{
		if (!location.IsValid())
		{
			return false;
		}
		CricketPlaceData cricketPlaceData = _cricketPlaceData[location.AreaId];
		for (int i = 0; i < (cricketPlaceData?.CricketBlocks?.Length).GetValueOrDefault(); i++)
		{
			if (cricketPlaceData.CricketBlocks[i] == location.BlockId && !cricketPlaceData.CricketTriggered[i])
			{
				return true;
			}
		}
		if (DomainManager.Extra.TryGetElement_CricketPlaceExtraData(location.AreaId, out var value))
		{
			Dictionary<short, short> extraMapUnits = value.ExtraMapUnits;
			if (extraMapUnits != null && extraMapUnits.Count > 0)
			{
				return value.ExtraMapUnits.ContainsKey(location.BlockId);
			}
		}
		return false;
	}

	[Obsolete("Use DomainManager.Extra.ChangeAreaSpiritualDebt instead")]
	public void ChangeAreaSpiritualDebt(DataContext context, short areaId, int delta)
	{
		MapAreaData mapAreaData = _areas[areaId];
		int[] spiritualDebtLimit = GlobalConfig.Instance.SpiritualDebtLimit;
		mapAreaData.SpiritualDebt = (short)Math.Clamp(mapAreaData.SpiritualDebt + delta, spiritualDebtLimit[0], spiritualDebtLimit[1]);
		if (delta > 0)
		{
			DomainManager.World.GetInstantNotifications().AddGraceIncreased(new Location(areaId, -1));
		}
		SetElement_Areas(areaId, mapAreaData, context);
	}

	[Obsolete("Use DomainManager.Extra.SetAreaSpiritualDebt instead")]
	public void SetAreaSpiritualDebt(DataContext context, short areaId, short value)
	{
		MapAreaData mapAreaData = _areas[areaId];
		short spiritualDebt = mapAreaData.SpiritualDebt;
		int[] spiritualDebtLimit = GlobalConfig.Instance.SpiritualDebtLimit;
		mapAreaData.SpiritualDebt = (short)Math.Clamp(value, spiritualDebtLimit[0], spiritualDebtLimit[1]);
		if (mapAreaData.SpiritualDebt > spiritualDebt)
		{
			DomainManager.World.GetInstantNotifications().AddGraceIncreased(new Location(areaId, -1));
		}
		SetElement_Areas(areaId, mapAreaData, context);
	}

	[Obsolete("Use SetAreaSpritualDebt instead")]
	public void ChangeSpiritualDebt(DataContext context, short areaId, short spiritualDebt)
	{
		DomainManager.Extra.SetAreaSpiritualDebt(context, areaId, spiritualDebt);
	}

	[Obsolete("Use ChangeAreaSpiritualDebt instead")]
	public void SetSpiritualDebtByChange(DataContext context, short areaId, short changeValue)
	{
		DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, changeValue);
	}

	public void GetNearAreaList(short areaId, List<short> areaList)
	{
		if (areaId < 45)
		{
			areaList.Clear();
			areaList.Add(areaId);
			areaList.AddRange(_regularAreaNearList[areaId].Items);
		}
	}

	public void ChangeSettlementSafetyInArea(DataContext context, short areaId, int delta)
	{
		MapAreaData mapAreaData = _areas[areaId];
		SettlementInfo[] settlementInfos = mapAreaData.SettlementInfos;
		for (int i = 0; i < settlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = settlementInfos[i];
			if (settlementInfo.SettlementId >= 0)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
				settlement.ChangeSafety(context, delta);
			}
		}
	}

	public void ChangeSettlementCultureInArea(DataContext context, short areaId, int delta)
	{
		MapAreaData mapAreaData = _areas[areaId];
		SettlementInfo[] settlementInfos = mapAreaData.SettlementInfos;
		for (int i = 0; i < settlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = settlementInfos[i];
			if (settlementInfo.SettlementId >= 0)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
				settlement.ChangeCulture(context, delta);
			}
		}
	}

	[DomainMethod]
	public MapAreaData GetAreaByAreaId(short areaId)
	{
		return GetElement_Areas(areaId);
	}

	public short GetSpiritualDebtLowestAreaIdByAreaId(short currAreaId)
	{
		sbyte stateIdByAreaId = GetStateIdByAreaId(currAreaId);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		GetAllAreaInState(stateIdByAreaId, list);
		int num = int.MaxValue;
		short result = currAreaId;
		foreach (short item in list)
		{
			if (!IsAreaBroken(item))
			{
				int areaSpiritualDebt = DomainManager.Extra.GetAreaSpiritualDebt(item);
				if (areaSpiritualDebt < num)
				{
					num = areaSpiritualDebt;
					result = item;
				}
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		return result;
	}

	[DomainMethod]
	public MapBlockData GetBlockData(short areaId, short blockId)
	{
		LastGetBlockDataPosition_Debug = new Location(areaId, blockId);
		return GetBlock(areaId, blockId);
	}

	[DomainMethod]
	public FullBlockName GetBlockFullName(Location location)
	{
		if (location.AreaId < 0)
		{
			return new FullBlockName
			{
				areaTemplateId = -1,
				stateTemplateId = -1,
				BelongBlockData = null,
				BlockData = null
			};
		}
		FullBlockName result = new FullBlockName
		{
			areaTemplateId = GetElement_Areas(location.AreaId).GetTemplateId(),
			stateTemplateId = GetStateTemplateIdByAreaId(location.AreaId)
		};
		MapBlockData block = GetBlock(location);
		result.BlockData = MapBlockData.SimpleClone(block);
		if (block.BelongBlockId >= 0)
		{
			MapBlockData block2 = GetBlock(block.AreaId, block.BelongBlockId);
			result.BelongBlockData = MapBlockData.SimpleClone(block2);
		}
		return result;
	}

	[DomainMethod]
	public List<CollectResourceResult> CollectAllResourcesFree(DataContext context)
	{
		List<CollectResourceResult> list = new List<CollectResourceResult>();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		MapDomain map = DomainManager.Map;
		foreach (Location item in ProfessionSkillHandle.GetSavageSkill_1_EffectRange(taiwu.GetLocation()))
		{
			MapBlockData block = map.GetBlock(item);
			if (126 != block.TemplateId)
			{
				for (sbyte b = 0; b < 6; b++)
				{
					short currentResource;
					short maxResource;
					CollectResourceResult result = CalcCollectResourceResult(context.Random, block, b, out currentResource, out maxResource);
					ApplyCollectResourceResult(context, taiwu, block, currentResource, maxResource, costResource: false, ref result);
					list.Add(result);
				}
			}
		}
		return list;
	}

	internal unsafe CollectResourceResult CalcCollectResourceResult(IRandomSource random, MapBlockData blockData, sbyte resourceType, out short currentResource, out short maxResource)
	{
		CollectResourceResult result = default(CollectResourceResult);
		currentResource = blockData.CurrResources.Items[resourceType];
		maxResource = Math.Max(blockData.MaxResources.Items[resourceType], (short)1);
		result.ResourceType = resourceType;
		result.ResourceCount = (short)GetCollectResourceAmount(random, blockData, resourceType);
		result.ResourceCount = (short)(result.ResourceCount * GameData.Domains.World.SharedMethods.GetGainResourcePercent(11) / 100);
		return result;
	}

	internal unsafe void ApplyCollectResourceResult(DataContext ctx, GameData.Domains.Character.Character character, MapBlockData blockData, short currentResource, short maxResource, bool costResource, ref CollectResourceResult result)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		IRandomSource random = ctx.Random;
		int id = character.GetId();
		sbyte resourceType = result.ResourceType;
		character.ChangeResource(ctx, resourceType, result.ResourceCount);
		bool flag = DomainManager.TutorialChapter.IsInTutorialChapter(3) && resourceType == 1 && EventHelper.GetBoolFromGlobalArgBox("WaitCollectWoodOuter3") && EventArgBox.TaiwuAreaId == 136;
		short itemTemplateId = blockData.GetCollectItemTemplateId(random, resourceType);
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(DomainManager.Building.GetBuildingBlockEffect(blockData.GetLocation(), EBuildingScaleEffect.CollectResourceGetItemBonus));
		int percentProb = blockData.GetCollectItemChance(resourceType) * val;
		if (flag)
		{
			ItemKey itemKey = DomainManager.Item.CreateItem(ctx, 5, 2);
			EventHelper.RemoveFromGlobalArgBox<bool>("WaitCollectWoodOuter3");
			character.AddInventoryItem(ctx, itemKey, 1);
			EventHelper.SaveGlobalArg("TutorialWoodOuter3Got", itemKey);
			result.ItemDisplayData = DomainManager.Item.GetItemDisplayData(itemKey, id);
			result.ItemDisplayData.Amount = 1;
		}
		else if (itemTemplateId >= 0 && random.CheckPercentProb(percentProb))
		{
			int num = 1;
			if (id == DomainManager.Taiwu.GetTaiwuCharId())
			{
				MapBlockItem mapBlockItem = MapBlock.Instance[blockData.TemplateId];
				List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
				GetNeighborBlocks(blockData.AreaId, blockData.BlockId, list);
				for (int i = 0; i < list.Count; i++)
				{
					if (mapBlockItem.ResourceCollectionType == MapBlock.Instance[list[i].TemplateId].ResourceCollectionType)
					{
						num += 2;
					}
				}
				ObjectPool<List<MapBlockData>>.Instance.Return(list);
			}
			UpgradeCollectMaterial(random, blockData.GetResourceCollectionConfig(), resourceType, maxResource, currentResource, num, ref itemTemplateId);
			ItemKey itemKey2 = DomainManager.Item.CreateItem(ctx, 5, itemTemplateId);
			character.AddInventoryItem(ctx, itemKey2, 1);
			result.ItemDisplayData = DomainManager.Item.GetItemDisplayData(itemKey2, id);
			result.ItemDisplayData.Amount = 1;
		}
		else
		{
			result.ItemDisplayData = null;
		}
		if (!DomainManager.TutorialChapter.GetInGuiding() && id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[0];
			int num2 = formulaCfg.Calculate(currentResource);
			int num3 = 0;
			if (result.ItemDisplayData != null)
			{
				ProfessionFormulaItem formulaCfg2 = ProfessionFormula.Instance[1];
				num3 = formulaCfg2.Calculate(result.ItemDisplayData.Value);
			}
			DomainManager.Extra.ChangeProfessionSeniority(ctx, 0, num2 + num3);
		}
		if (costResource)
		{
			ResourceTypeItem resourceTypeItem = Config.ResourceType.Instance[resourceType];
			blockData.CurrResources.Items[resourceType] = (short)Math.Max(currentResource - resourceTypeItem.ResourceReducePerCollection, 0);
			SetBlockData(ctx, blockData);
			AddBlockMalice(ctx, blockData.AreaId, blockData.BlockId, 10);
		}
		VillagerWorkData villagerMapWorkData = DomainManager.Taiwu.GetVillagerMapWorkData(blockData.AreaId, blockData.BlockId, 10);
		if (villagerMapWorkData != null)
		{
			DomainManager.Taiwu.SetVillagerWork(ctx, villagerMapWorkData.CharacterId, villagerMapWorkData);
		}
	}

	public void UpgradeCollectMaterial(IRandomSource random, ResourceCollectionItem collectionConfig, sbyte resourceType, short maxResource, short currentResource, int neighborOddsMultiplier, ref short itemTemplateId)
	{
		sbyte b = collectionConfig.MaxAddGrade[resourceType];
		int num = collectionConfig.GradeUpOdds[resourceType] + Math.Max(maxResource - 100, 0) / 10 * neighborOddsMultiplier;
		if (!random.CheckPercentProb(num))
		{
			return;
		}
		int num2 = num * currentResource / maxResource;
		if (num2 >= 100)
		{
			itemTemplateId += b;
			return;
		}
		for (int i = 0; i < b; i++)
		{
			if (random.CheckPercentProb(num2))
			{
				itemTemplateId++;
			}
		}
	}

	[DomainMethod]
	public CollectResourceResult CollectResource(DataContext context, int charId, sbyte resourceType, bool costTime = true, bool costResource = true)
	{
		if (costTime && DomainManager.World.GetLeftDaysInCurrMonth() == 0)
		{
			throw new Exception("No enough time for resource collection");
		}
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		MapBlockData block = GetBlock(element_Objects.GetLocation());
		short currentResource;
		short maxResource;
		CollectResourceResult result = CalcCollectResourceResult(context.Random, block, resourceType, out currentResource, out maxResource);
		ApplyCollectResourceResult(context, element_Objects, block, currentResource, maxResource, costResource, ref result);
		return result;
	}

	[DomainMethod]
	public List<MapBlockData> GetMapBlockDataList(List<Location> locationList)
	{
		return GetMapBlockDataListOptional(locationList);
	}

	[DomainMethod]
	public List<MapBlockData> GetMapBlockDataListOptional(List<Location> locationList, bool includeRoot = false, bool includeBelong = false)
	{
		List<MapBlockData> list = new List<MapBlockData>();
		if (locationList != null)
		{
			for (int i = 0; i < locationList.Count; i++)
			{
				MapBlockData block = GetBlock(locationList[i]);
				list.Add(block);
				if (includeRoot && block.RootBlockId > -1)
				{
					MapBlockData block2 = GetBlock(block.AreaId, block.RootBlockId);
					list.Add(block2);
				}
				if (includeBelong && block.BelongBlockId > -1)
				{
					MapBlockData block3 = GetBlock(block.AreaId, block.BelongBlockId);
					list.Add(block3);
				}
			}
		}
		return list;
	}

	[DomainMethod]
	public bool IsContainsPurpleBamboo(short areaId)
	{
		using (IEnumerator<short> enumerator = IterAreaPurpleBambooTemplateIds(areaId).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				short current = enumerator.Current;
				return true;
			}
		}
		return false;
	}

	public IEnumerable<short> IterAreaPurpleBambooTemplateIds(short areaId)
	{
		byte size = GetAreaSize(areaId);
		for (short i = 0; i < size * size; i++)
		{
			MapBlockData blockData = GetBlock(areaId, i);
			if (blockData.FixedCharacterSet != null)
			{
				foreach (int charId in blockData.FixedCharacterSet)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					if (character.GetXiangshuType() == 3)
					{
						yield return character.GetTemplateId();
					}
				}
			}
		}
	}

	[DomainMethod]
	public List<short> GetBelongBlockTemplateIdList(List<Location> locationList)
	{
		List<short> list = new List<short>();
		if (locationList != null)
		{
			for (int i = 0; i < locationList.Count; i++)
			{
				Location key = locationList[i];
				short belongBlockId = GetBlock(key).BelongBlockId;
				list.Add((short)((belongBlockId >= 0) ? GetBlock(key.AreaId, belongBlockId).TemplateId : (-1)));
			}
		}
		return list;
	}

	[DomainMethod]
	public LocationNameRelatedData GetLocationNameRelatedData(Location location)
	{
		if (location.AreaId < 0)
		{
			return new LocationNameRelatedData(-1);
		}
		MapAreaData mapAreaData = _areas[location.AreaId];
		LocationNameRelatedData result = new LocationNameRelatedData(mapAreaData.GetTemplateId());
		if (location.BlockId < 0)
		{
			return result;
		}
		MapBlockData rootBlock = GetBlock(location).GetRootBlock();
		if (rootBlock.IsCityTown())
		{
			result.SettlementMapBlockTemplateId = rootBlock.TemplateId;
			int settlementIndex = mapAreaData.GetSettlementIndex(rootBlock.BlockId);
			SettlementInfo settlementInfo = mapAreaData.SettlementInfos[settlementIndex];
			result.SettlementRandomNameId = settlementInfo.RandomNameId;
		}
		else
		{
			var (num, direction) = mapAreaData.GetReferenceSettlementAndDirection(location.BlockId);
			if (num >= 0)
			{
				SettlementInfo settlementInfo2 = mapAreaData.SettlementInfos[num];
				MapBlockData block = GetBlock(location.AreaId, settlementInfo2.BlockId);
				result.SettlementMapBlockTemplateId = block.TemplateId;
				result.SettlementRandomNameId = settlementInfo2.RandomNameId;
			}
			result.Direction = direction;
		}
		return result;
	}

	[DomainMethod]
	public List<LocationNameRelatedData> GetLocationNameRelatedDataList(List<Location> locations)
	{
		int count = locations.Count;
		List<LocationNameRelatedData> list = new List<LocationNameRelatedData>(count);
		for (int i = 0; i < count; i++)
		{
			list.Add(GetLocationNameRelatedData(locations[i]));
		}
		return list;
	}

	[DomainMethod]
	public void ChangeBlockTemplate(DataContext context, Location location, short blockTemplateId, bool isTurnVisible)
	{
		MapBlockData block = GetBlock(location);
		ChangeBlockTemplate(context, block, blockTemplateId);
		if (isTurnVisible)
		{
			SetBlockAndViewRangeVisible(context, location.AreaId, location.BlockId);
		}
	}

	public bool TryGetBlock(Location location, out MapBlockData blockData)
	{
		blockData = null;
		short areaId = location.AreaId;
		if ((areaId < 0 || areaId >= 139) ? true : false)
		{
			return false;
		}
		short areaId2 = location.AreaId;
		if (1 == 0)
		{
		}
		AreaBlockCollection areaBlockCollection = ((areaId2 < 45) ? GetRegularAreaBlocks(areaId2) : ((areaId2 < 135) ? _brokenAreaBlocks : (areaId2 switch
		{
			135 => _bornAreaBlocks, 
			136 => _guideAreaBlocks, 
			137 => _secretVillageAreaBlocks, 
			138 => _brokenPerformAreaBlocks, 
			_ => null, 
		})));
		if (1 == 0)
		{
		}
		AreaBlockCollection areaBlockCollection2 = areaBlockCollection;
		short num = location.BlockId;
		if (IsAreaBroken(areaId2))
		{
			num += (short)(25 * (areaId2 - 45));
		}
		return areaBlockCollection2?.TryGetValue(num, out blockData) ?? false;
	}

	public MapBlockData GetBlock(short areaId, short blockId)
	{
		if (TryGetBlock(new Location(areaId, blockId), out var blockData))
		{
			return blockData;
		}
		throw new Exception($"Failed to get block at {areaId} {blockId}");
	}

	public MapBlockData GetBlock(Location key)
	{
		return GetBlock(key.AreaId, key.BlockId);
	}

	public bool SplitMultiBlock(DataContext context, MapBlockData blockData)
	{
		MapBlockItem config = blockData.GetConfig();
		if (config.SplitOrMergeBlockId < 0)
		{
			return false;
		}
		if (config.Size < 2)
		{
			return false;
		}
		return ChangeBlockTemplate(context, blockData, config.SplitOrMergeBlockId);
	}

	public bool MergeMultiBlock(DataContext context, MapBlockData blockData)
	{
		MapBlockItem config = blockData.GetConfig();
		if (config.SplitOrMergeBlockId < 0)
		{
			return false;
		}
		if (config.Size > 1)
		{
			return false;
		}
		return ChangeBlockTemplateByMerge(context, blockData, config.SplitOrMergeBlockId);
	}

	public bool ChangeBlockTemplate(DataContext context, MapBlockData blockData, short newTemplateId)
	{
		if (MapBlock.Instance[newTemplateId].Size > 1)
		{
			return ChangeBlockTemplateByMerge(context, blockData, newTemplateId);
		}
		blockData = blockData.GetRootBlock();
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		list.Clear();
		if (blockData.GroupBlockList == null)
		{
			list.Add(blockData);
		}
		else
		{
			list.Add(blockData);
			list.AddRange(blockData.GroupBlockList);
			blockData.GroupBlockList = null;
		}
		foreach (MapBlockData item in list)
		{
			item.RootBlockId = -1;
			item.ChangeTemplateId(newTemplateId);
			item.InitResources(context.Random);
			item.Destroyed = false;
			SetBlockData(context, item);
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		return true;
	}

	public bool ChangeBlockTemplateByMerge(DataContext context, MapBlockData blockData, short newTemplateId)
	{
		byte areaSize = GetAreaSize(blockData.AreaId);
		Span<MapBlockData> areaBlocks = GetAreaBlocks(blockData.AreaId);
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(blockData.BlockId, areaSize);
		MapBlockItem mapBlockItem = MapBlock.Instance[newTemplateId];
		for (byte b = 0; b < mapBlockItem.Size; b++)
		{
			for (byte b2 = 0; b2 < mapBlockItem.Size; b2++)
			{
				ByteCoordinate byteCoordinate2 = byteCoordinate + new ByteCoordinate(b, b2);
				short index = ByteCoordinate.CoordinateToIndex(byteCoordinate2, areaSize);
				MapBlockData mapBlockData = areaBlocks[index];
				Tester.Assert(mapBlockData.IsPassable(), "childBlock.IsPassable()");
				if (mapBlockData == blockData)
				{
					mapBlockData.ChangeTemplateId(newTemplateId);
				}
				else
				{
					mapBlockData.ChangeTemplateId(-1);
					mapBlockData.SetToSizeBlock(blockData);
				}
				mapBlockData.InitResources(context.Random);
				mapBlockData.Destroyed = false;
				SetBlockData(context, mapBlockData);
			}
		}
		blockData.SetVisible(blockData.Visible, context);
		return true;
	}

	private void GetInSightBlocks(List<MapBlockData> inSightBlocks)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (location.IsValid())
		{
			MapBlockData block = GetBlock(location);
			GetNeighborBlocks(location.AreaId, location.BlockId, inSightBlocks, block.GetConfig().ViewRange);
			inSightBlocks.Add(block.GetRootBlock());
		}
	}

	public void GetNeighborBlocks(short areaId, short blockId, List<MapBlockData> neighborBlocks, int maxSteps = 1)
	{
		byte areaSize = GetAreaSize(areaId);
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		MapBlockData rootBlock = areaBlocks[blockId].GetRootBlock();
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(rootBlock.BlockId, areaSize);
		byte size = rootBlock.GetConfig().Size;
		neighborBlocks.Clear();
		for (byte b = (byte)Math.Max(byteCoordinate.X - maxSteps, 0); b < Math.Min(byteCoordinate.X + size + maxSteps, areaSize); b++)
		{
			for (byte b2 = (byte)Math.Max(byteCoordinate.Y - maxSteps, 0); b2 < Math.Min(byteCoordinate.Y + size + maxSteps, areaSize); b2++)
			{
				MapBlockData rootBlock2 = areaBlocks[ByteCoordinate.CoordinateToIndex(new ByteCoordinate(b, b2), areaSize)].GetRootBlock();
				if (rootBlock2.BlockId != rootBlock.BlockId && rootBlock.GetManhattanDistanceToPos(b, b2) <= maxSteps && rootBlock2.IsPassable() && !neighborBlocks.Contains(rootBlock2))
				{
					neighborBlocks.Add(rootBlock2);
				}
			}
		}
	}

	public void GetLocationByDistance(Location centerLocation, int minStep, int maxStep, ref List<MapBlockData> mapBlockList)
	{
		ByteCoordinate blockPos = DomainManager.Map.GetBlock(centerLocation).GetBlockPos();
		DomainManager.Map.GetRealNeighborBlocks(centerLocation.AreaId, centerLocation.BlockId, mapBlockList, maxStep);
		if (minStep <= 0)
		{
			return;
		}
		for (int num = mapBlockList.Count - 1; num >= 0; num--)
		{
			if (mapBlockList[num].GetManhattanDistanceToPos(blockPos.X, blockPos.Y) < minStep)
			{
				CollectionUtils.SwapAndRemove(mapBlockList, num);
			}
		}
	}

	public void GetTaiwuVillageDistanceLocations(List<Location> locations, int distance)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		byte areaSize = GetAreaSize(taiwuVillageLocation.AreaId);
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(taiwuVillageLocation.AreaId);
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(taiwuVillageLocation.BlockId, areaSize);
		for (byte b = 0; b < areaSize; b++)
		{
			for (byte b2 = 0; b2 < areaSize; b2++)
			{
				if (MathF.Abs(b - byteCoordinate.X) > (float)distance && MathF.Abs(b2 - byteCoordinate.Y) > (float)distance)
				{
					short index = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(b, b2), areaSize);
					MapBlockData rootBlock = areaBlocks[index].GetRootBlock();
					locations.Add(new Location(rootBlock.AreaId, rootBlock.BlockId));
				}
			}
		}
	}

	public void GetAreaNotSettlementLocations(List<Location> locations, short areaId)
	{
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(areaId);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < element_Areas.SettlementInfos.Length; i++)
		{
			short blockId = element_Areas.SettlementInfos[i].BlockId;
			if (blockId >= 0)
			{
				list.Add(blockId);
			}
		}
		for (int j = 0; j < areaBlocks.Length; j++)
		{
			if (!list.Contains(areaBlocks[j].BlockId))
			{
				MapBlockData mapBlockData = areaBlocks[j];
				locations.Add(new Location(mapBlockData.AreaId, mapBlockData.BlockId));
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
	}

	public void GetRealNeighborBlocks(short areaId, short blockId, List<MapBlockData> neighborBlocks, int maxSteps = 1, bool includeCenter = false)
	{
		byte areaSize = GetAreaSize(areaId);
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		MapBlockData mapBlockData = areaBlocks[blockId];
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(mapBlockData.BlockId, areaSize);
		neighborBlocks.Clear();
		for (byte b = (byte)Math.Max(byteCoordinate.X - maxSteps, 0); b < Math.Min(byteCoordinate.X + maxSteps + 1, areaSize); b++)
		{
			for (byte b2 = (byte)Math.Max(byteCoordinate.Y - maxSteps, 0); b2 < Math.Min(byteCoordinate.Y + maxSteps + 1, areaSize); b2++)
			{
				ByteCoordinate byteCoordinate2 = new ByteCoordinate(b, b2);
				MapBlockData mapBlockData2 = areaBlocks[ByteCoordinate.CoordinateToIndex(byteCoordinate2, areaSize)];
				if (mapBlockData2.IsPassable() && byteCoordinate.GetManhattanDistance(byteCoordinate2) <= maxSteps && (mapBlockData2.BlockId != mapBlockData.BlockId || includeCenter) && !neighborBlocks.Contains(mapBlockData2))
				{
					neighborBlocks.Add(mapBlockData2);
				}
			}
		}
	}

	public Location GetRandomGraveBlock(IRandomSource random, Location location)
	{
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(location.AreaId);
		MapBlockData rootBlock = areaBlocks[location.BlockId].GetRootBlock();
		if (!rootBlock.IsCityTown() && rootBlock.BlockType != EMapBlockType.Station)
		{
			return location;
		}
		byte areaSize = GetAreaSize(location.AreaId);
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(rootBlock.BlockId, areaSize);
		byte size = rootBlock.GetConfig().Size;
		int num = ((size >= 2) ? 3 : 2);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		for (byte b = (byte)Math.Max(byteCoordinate.X - num, 0); b < Math.Min(byteCoordinate.X + size + num, areaSize); b++)
		{
			for (byte b2 = (byte)Math.Max(byteCoordinate.Y - num, 0); b2 < Math.Min(byteCoordinate.Y + size + num, areaSize); b2++)
			{
				MapBlockData mapBlockData = areaBlocks[ByteCoordinate.CoordinateToIndex(new ByteCoordinate(b, b2), areaSize)];
				short blockId = mapBlockData.BlockId;
				if (blockId != rootBlock.BlockId && mapBlockData.IsPassable() && !mapBlockData.IsCityTown() && mapBlockData.BlockType != EMapBlockType.Station && rootBlock.GetManhattanDistanceToPos(b, b2) <= num)
				{
					list.Add(blockId);
				}
			}
		}
		short blockId2 = list[random.Next(list.Count)];
		ObjectPool<List<short>>.Instance.Return(list);
		return new Location(location.AreaId, blockId2);
	}

	public unsafe short GetRandomAdjacentBlockId(IRandomSource random, short areaId, short blockId)
	{
		byte areaSize = GetAreaSize(areaId);
		int num = blockId % areaSize;
		int num2 = blockId / areaSize;
		sbyte* ptr = stackalloc sbyte[8]
		{
			(sbyte)(num - 1),
			(sbyte)num2,
			(sbyte)(num + 1),
			(sbyte)num2,
			(sbyte)num,
			(sbyte)(num2 - 1),
			(sbyte)num,
			(sbyte)(num2 + 1)
		};
		Span<MapBlockData> areaBlocks = GetAreaBlocks(areaId);
		for (int num3 = 4; num3 > 0; num3--)
		{
			int num4 = random.Next(num3);
			sbyte b = ptr[num4 * 2];
			sbyte b2 = ptr[num4 * 2 + 1];
			if (b >= 0 && b < areaSize && b2 >= 0 && b2 < areaSize)
			{
				int num5 = b + b2 * areaSize;
				MapBlockData mapBlockData = areaBlocks[num5];
				if (mapBlockData.IsPassable())
				{
					return (short)num5;
				}
			}
			short* ptr2 = (short*)ptr;
			short num6 = ptr2[num3 - 1];
			ptr2[num4] = num6;
		}
		throw new Exception($"Failed to get passable adjacent block: ({areaId}, {blockId})");
	}

	public short GetRandomEdgeBlock(IRandomSource random, short areaId, sbyte edgeType)
	{
		Span<MapBlockData> areaBlocks = GetAreaBlocks(areaId);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		byte areaSize = GetAreaSize(areaId);
		list.Clear();
		for (byte b = 0; b < areaSize; b++)
		{
			for (byte b2 = 0; b2 < areaSize; b2++)
			{
				int num;
				switch (edgeType)
				{
				default:
					num = (byte)(areaSize - 1 - b);
					break;
				case 0:
					num = b;
					break;
				case 2:
				case 3:
					num = b2;
					break;
				}
				byte x = (byte)num;
				int num2;
				switch (edgeType)
				{
				default:
					num2 = (byte)(areaSize - 1 - b);
					break;
				case 3:
					num2 = b;
					break;
				case 0:
				case 1:
					num2 = b2;
					break;
				}
				byte y = (byte)num2;
				ByteCoordinate byteCoordinate = new ByteCoordinate(x, y);
				short num3 = ByteCoordinate.CoordinateToIndex(byteCoordinate, areaSize);
				if (areaBlocks[num3].IsPassable())
				{
					list.Add(num3);
				}
			}
			if (list.Count > 0)
			{
				break;
			}
		}
		short result = (short)((list.Count > 0) ? list[random.Next(list.Count)] : 0);
		ObjectPool<List<short>>.Instance.Return(list);
		return result;
	}

	public void EnsureBlockVisible(DataContext context, Location location)
	{
		if (location.IsValid())
		{
			MapBlockData block = GetBlock(location);
			if (!block.Visible)
			{
				block.SetVisible(visible: true, context);
			}
		}
	}

	public void HideAllBlocks(DataContext context)
	{
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		GetInSightBlocks(list);
		for (short num = 0; num < 139; num++)
		{
			HideAllBlocks(context, num, list);
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		DomainManager.Merchant.RefreshCaravanInTaiwuState(context);
	}

	public void HideAllBlocks(DataContext context, short areaId, List<MapBlockData> exceptBlocks)
	{
		Span<MapBlockData> areaBlocks = GetAreaBlocks(areaId);
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			if (mapBlockData.IsPassable())
			{
				bool flag = exceptBlocks.Contains(mapBlockData) || exceptBlocks.Contains(mapBlockData.GetRootBlock());
				if (flag != mapBlockData.Visible)
				{
					mapBlockData.SetVisible(flag, context);
				}
			}
		}
	}

	public void AddBlockMalice(DataContext context, short areaId, short blockId, int addValue)
	{
		MapBlockData block = GetBlock(areaId, blockId);
		short maxMalice = block.GetMaxMalice();
		if (maxMalice > 0)
		{
			block.Malice = (short)Math.Clamp(block.Malice + addValue, 0, block.GetMaxMalice());
			SetBlockData(context, block);
		}
	}

	public void AddBlockItem(DataContext context, MapBlockData block, ItemKey itemKey, int amount)
	{
		SortedList<ItemKeyAndDate, int> items = block.Items;
		if (items != null && items.Count > 500)
		{
			DomainManager.Item.RemoveItem(context, itemKey);
		}
		else
		{
			OfflineAddBlockItem(block, itemKey, amount);
		}
		SetBlockData(context, block);
	}

	public void AddBlockItems(DataContext context, MapBlockData block, List<(ItemKey itemKey, int amount)> items)
	{
		if (block.Items?.Count + items.Count > 500)
		{
			DomainManager.Item.RemoveItems(context, items);
		}
		else
		{
			OfflineAddBlockItems(block, items);
		}
		SetBlockData(context, block);
	}

	public void RemoveBlockItem(DataContext context, MapBlockData block, ItemKeyAndDate itemKeyAndDate)
	{
		OfflineRemoveBlockItem(block, itemKeyAndDate);
		SetBlockData(context, block);
	}

	private void OfflineAddBlockItem(MapBlockData block, ItemKey itemKey, int amount)
	{
		int hashCode = block.GetLocation().GetHashCode();
		DomainManager.Item.SetOwner(itemKey, ItemOwnerType.MapBlock, hashCode);
		block.AddItem(itemKey, amount);
	}

	private void OfflineAddBlockItems(MapBlockData block, List<(ItemKey itemKey, int amount)> items)
	{
		int hashCode = block.GetLocation().GetHashCode();
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			var (itemKey, num) = items[i];
			DomainManager.Item.SetOwner(itemKey, ItemOwnerType.MapBlock, hashCode);
		}
		block.AddItems(items);
	}

	private void OfflineRemoveBlockItem(MapBlockData block, ItemKeyAndDate itemKeyAndDate)
	{
		DomainManager.Item.RemoveOwner(itemKeyAndDate.ItemKey, ItemOwnerType.MapBlock, block.GetLocation().GetHashCode());
		block.RemoveItem(itemKeyAndDate);
	}

	private void OfflineRemoveBlockItemByCount(MapBlockData block, ItemKeyAndDate itemKeyAndDate, int count)
	{
		DomainManager.Item.RemoveOwner(itemKeyAndDate.ItemKey, ItemOwnerType.MapBlock, block.GetLocation().GetHashCode());
		block.RemoveItemByCount(itemKeyAndDate, count);
	}

	public void InitializeOwnedItems()
	{
		for (short num = 0; num < 139; num++)
		{
			Span<MapBlockData> areaBlocks = GetAreaBlocks(num);
			int i = 0;
			for (int length = areaBlocks.Length; i < length; i++)
			{
				MapBlockData mapBlockData = areaBlocks[i];
				SortedList<ItemKeyAndDate, int> items = mapBlockData.Items;
				if (items != null)
				{
					Location location = new Location(num, mapBlockData.BlockId);
					IList<ItemKeyAndDate> keys = items.Keys;
					int j = 0;
					for (int count = items.Count; j < count; j++)
					{
						ItemKey itemKey = keys[j].ItemKey;
						DomainManager.Item.SetOwner(itemKey, ItemOwnerType.MapBlock, location.GetHashCode());
					}
				}
			}
		}
	}

	public List<(Location, short)> CalcBlockTravelRoute(IRandomSource random, Location start, Location end, bool isMerchant = true)
	{
		List<(Location, short)> list = new List<(Location, short)>();
		List<(short, short)> list2 = new List<(short, short)>();
		Location item = start;
		sbyte b = -1;
		AStarMap aStarMap = new AStarMap();
		List<ByteCoordinate> list3 = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		List<ByteCoordinate> list4 = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		List<short> list5 = ObjectPool<List<short>>.Instance.Get();
		Dictionary<TravelRouteKey, TravelRoute> dictionary = ((isMerchant || start.AreaId >= 135 || DomainManager.World.GetWorldFunctionsStatus(4)) ? _travelRouteDict : _bornStateTravelRouteDict);
		list2.Add((start.AreaId, 0));
		if (start.AreaId != end.AreaId)
		{
			TravelRouteKey key = new TravelRouteKey(start.AreaId, end.AreaId);
			bool flag = key.FromAreaId > key.ToAreaId;
			if (flag)
			{
				key.Reverse();
			}
			TravelRoute travelRoute = dictionary[key];
			if (!flag)
			{
				for (int i = 0; i < travelRoute.CostList.Count; i++)
				{
					list2.Add((travelRoute.AreaList[i + 1], travelRoute.CostList[i]));
				}
			}
			else
			{
				for (int num = travelRoute.CostList.Count - 1; num >= 0; num--)
				{
					list2.Add((travelRoute.AreaList[num], travelRoute.CostList[num]));
				}
			}
		}
		for (int j = 0; j < list2.Count; j++)
		{
			(short, short) tuple = list2[j];
			short areaId = tuple.Item1;
			byte areaSize = GetAreaSize(areaId);
			if (b >= 0)
			{
				short randomEdgeBlock = GetRandomEdgeBlock(random, areaId, b);
				item.AreaId = areaId;
				item.BlockId = randomEdgeBlock;
				list.Add((item, tuple.Item2));
			}
			aStarMap.InitMap(areaSize, areaSize, (ByteCoordinate coord) => GetBlock(areaId, ByteCoordinate.CoordinateToIndex(coord, areaSize)).MoveCost);
			list4.Clear();
			if (isMerchant && areaId < 45)
			{
				list5.Clear();
				SettlementInfo[] settlementInfos = _areas[areaId].SettlementInfos;
				for (int num2 = 0; num2 < settlementInfos.Length; num2++)
				{
					SettlementInfo settlementInfo = settlementInfos[num2];
					if (settlementInfo.BlockId >= 0)
					{
						MapBlockData block = GetBlock(areaId, settlementInfo.BlockId);
						int num3 = block.GroupBlockList?.Count ?? 0;
						if (num3 > 1)
						{
							list5.Add(block.GroupBlockList.GetRandom(random).BlockId);
						}
						else
						{
							list5.Add(settlementInfo.BlockId);
						}
					}
				}
				if (list5.Count > 1)
				{
					MapBlockData curBlockData = GetBlock(areaId, item.BlockId);
					list5.Sort(delegate(short aBlockId, short bBlockId)
					{
						MapBlockData block3 = GetBlock(areaId, aBlockId);
						int num5 = ((block3.GetRootBlock().BlockId == end.BlockId) ? int.MaxValue : curBlockData.GetBlockPos().GetManhattanDistance(block3.GetBlockPos()));
						MapBlockData block4 = GetBlock(areaId, bBlockId);
						int value = ((block4.GetRootBlock().BlockId == end.BlockId) ? int.MaxValue : curBlockData.GetBlockPos().GetManhattanDistance(block4.GetBlockPos()));
						return num5.CompareTo(value);
					});
				}
				foreach (short item2 in list5)
				{
					CalcPathInArea(aStarMap, list, list3, areaId, areaSize, item.BlockId, item2);
					item.BlockId = item2;
					list4.AddRange(list3);
				}
			}
			if (j == list2.Count - 1)
			{
				if (isMerchant && list.Exists(((Location, short) r) => r.Item1 == end))
				{
					MapBlockData block2 = GetBlock(areaId, end.BlockId);
					List<MapBlockData> list6 = ObjectPool<List<MapBlockData>>.Instance.Get();
					int num4 = block2.GroupBlockList?.Count ?? 0;
					if (num4 > 1)
					{
						list6.AddRange(block2.GroupBlockList.Where((MapBlockData mapBlockData) => mapBlockData.GetLocation() != end));
						if (list6.Count > 0)
						{
							end.BlockId = list6.GetRandom(random).BlockId;
						}
					}
					ObjectPool<List<MapBlockData>>.Instance.Return(list6);
				}
				CalcPathInArea(aStarMap, list, list3, areaId, areaSize, item.BlockId, end.BlockId, list4);
			}
			else
			{
				sbyte[] worldMapPos = DomainManager.Map.GetElement_Areas(tuple.Item1).GetConfig().WorldMapPos;
				sbyte[] worldMapPos2 = DomainManager.Map.GetElement_Areas(list2[j + 1].Item1).GetConfig().WorldMapPos;
				b = MapAreaEdge.GetEnterEdge(worldMapPos, worldMapPos2);
				short randomEdgeBlock2 = GetRandomEdgeBlock(random, areaId, MapAreaEdge.GetOppositeEdge(b));
				CalcPathInArea(aStarMap, list, list3, areaId, areaSize, item.BlockId, randomEdgeBlock2, list4);
			}
		}
		ObjectPool<List<ByteCoordinate>>.Instance.Return(list3);
		ObjectPool<List<ByteCoordinate>>.Instance.Return(list4);
		ObjectPool<List<short>>.Instance.Return(list5);
		return list;
	}

	[DomainMethod]
	public List<Location> GetPathInAreaWithoutCost(Location start, Location end)
	{
		List<Location> list = new List<Location>();
		GetPathInAreaWithoutCost(start, end, list);
		return list;
	}

	public static void GetPathInAreaWithoutCost(Location start, Location end, List<Location> locations)
	{
		locations.Clear();
		if (start.Equals(end))
		{
			return;
		}
		byte areaSize = DomainManager.Map.GetAreaSize(start.AreaId);
		_aStarMap.InitMap(areaSize, areaSize, delegate(ByteCoordinate coord)
		{
			MapBlockData block = DomainManager.Map.GetBlock(start.AreaId, ByteCoordinate.CoordinateToIndex(coord, areaSize));
			return (sbyte)(block.IsPassable() ? 1 : sbyte.MaxValue);
		});
		List<ByteCoordinate> path = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		_aStarMap.FindWay(ByteCoordinate.IndexToCoordinate(start.BlockId, areaSize), ByteCoordinate.IndexToCoordinate(end.BlockId, areaSize), ref path);
		foreach (ByteCoordinate item2 in path)
		{
			Location item = new Location(start.AreaId, ByteCoordinate.CoordinateToIndex(item2, areaSize));
			locations.Add(item);
		}
		ObjectPool<List<ByteCoordinate>>.Instance.Return(path);
	}

	public bool ContainsCharacter(Location location, int charId)
	{
		MapBlockData block = GetBlock(location.AreaId, location.BlockId);
		return block.CharacterSet?.Contains(charId) ?? false;
	}

	private void CalcPathInArea(AStarMap aStarMap, List<(Location, short)> route, List<ByteCoordinate> pathInArea, short areaId, byte areaSize, short startBlock, short endBlock, List<ByteCoordinate> avoidPosList = null)
	{
		pathInArea.Clear();
		aStarMap.FindWay(ByteCoordinate.IndexToCoordinate(startBlock, areaSize), ByteCoordinate.IndexToCoordinate(endBlock, areaSize), ref pathInArea, avoidPosList);
		for (int i = 1; i < pathInArea.Count; i++)
		{
			short blockId = ByteCoordinate.CoordinateToIndex(pathInArea[i], areaSize);
			route.Add((new Location(areaId, blockId), GetBlock(areaId, blockId).MoveCost));
		}
	}

	private short GetMerchantSettlementDestBlockId(short blockId, byte areaSize, byte blockSize, sbyte inEdge, sbyte outEdge)
	{
		if (blockSize < 2 || inEdge < 0 || outEdge < 0 || outEdge == MapAreaEdge.GetOppositeEdge(inEdge))
		{
			return blockId;
		}
		return inEdge switch
		{
			0 => (short)((outEdge == 2) ? (blockId + areaSize * (blockSize - 1)) : blockId), 
			1 => (short)((outEdge == 2) ? (blockId + areaSize * (blockSize - 1) + blockSize - 1) : (blockId + blockSize - 1)), 
			2 => (short)((outEdge == 0) ? (blockId + areaSize * (blockSize - 1)) : (blockId + areaSize * (blockSize - 1) + blockSize - 1)), 
			3 => (short)((outEdge == 2) ? blockId : (blockId + blockSize - 1)), 
			_ => blockId, 
		};
	}

	public unsafe int GetCollectResourceAmount(IRandomSource random, MapBlockData blockData, sbyte resourceType)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		ResourceTypeItem resourceTypeItem = Config.ResourceType.Instance[resourceType];
		short num = blockData.CurrResources.Items[resourceType];
		int collectMultiplier = resourceTypeItem.CollectMultiplier;
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(DomainManager.Building.GetBuildingBlockEffect(blockData.GetLocation(), EBuildingScaleEffect.CollectResourceIncomeBonus));
		return num * (((num >= 100) ? 60 : 40) + random.Next(-20, 21)) / 100 * collectMultiplier * val;
	}

	public List<Location> GetBlockLocationGroup(Location location, bool includeSelf = true)
	{
		List<Location> retList = new List<Location>();
		retList.Add(location);
		if (!location.IsValid())
		{
			return retList;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		MapBlockData mapBlockData = null;
		short rootBlockId = block.RootBlockId;
		if (-1 != rootBlockId)
		{
			mapBlockData = DomainManager.Map.GetBlockData(location.AreaId, rootBlockId);
			if (mapBlockData == null)
			{
				return retList;
			}
		}
		else
		{
			List<MapBlockData> groupBlockList = block.GroupBlockList;
			if (groupBlockList != null && groupBlockList.Count > 0)
			{
				mapBlockData = block;
			}
		}
		if (mapBlockData != null)
		{
			Location item = new Location(mapBlockData.AreaId, mapBlockData.BlockId);
			if (!retList.Contains(item))
			{
				retList.Add(item);
			}
			List<MapBlockData> groupBlockList2 = mapBlockData.GroupBlockList;
			if (groupBlockList2 != null && groupBlockList2.Count > 0)
			{
				groupBlockList2.ForEach(delegate(MapBlockData e)
				{
					Location item2 = new Location(e.AreaId, e.BlockId);
					if (!retList.Contains(item2))
					{
						retList.Add(item2);
					}
				});
			}
		}
		return retList;
	}

	public void DestroyMapBlockItemsDirect(DataContext context, MapBlockData blockData)
	{
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		blockData.DestroyItemsDirect(list);
		SetBlockData(context, blockData);
		DomainManager.Item.RemoveItems(context, list);
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	public void ClearBlockRandomEnemies(DataContext context, MapBlockData blockData)
	{
		if (blockData.TemplateEnemyList != null)
		{
			Location location = blockData.GetLocation();
			for (int num = blockData.TemplateEnemyList.Count - 1; num >= 0; num--)
			{
				MapTemplateEnemyInfo templateEnemyInfo = blockData.TemplateEnemyList[num];
				Events.RaiseTemplateEnemyLocationChanged(context, templateEnemyInfo, location, Location.Invalid);
			}
		}
	}

	[Obsolete]
	public bool RemoveBlockAnimal(Location location, DataContext context)
	{
		return false;
	}

	[Obsolete]
	public bool RemoveBlockSingleAnimal(Location location, short animalId, DataContext context)
	{
		AnimalAreaData element_AnimalAreaData = DomainManager.Extra.GetElement_AnimalAreaData(location.AreaId);
		if (element_AnimalAreaData.BlockAnimalCharacterTemplateIdList != null && element_AnimalAreaData.BlockAnimalCharacterTemplateIdList.TryGetValue(location.BlockId, out var value))
		{
			int num = value.IndexOf(animalId);
			if (num >= 0)
			{
				value.RemoveAt(num);
				DomainManager.Extra.SetAnimalAreaData(context, location.AreaId, element_AnimalAreaData);
				RemoveHunterAnimal(context, location, animalId);
				return true;
			}
		}
		return false;
	}

	[Obsolete]
	public void AddBlockSingleAnimal(Location location, short templateId, DataContext context)
	{
		AnimalAreaData element_AnimalAreaData = DomainManager.Extra.GetElement_AnimalAreaData(location.AreaId);
		if (element_AnimalAreaData.BlockAnimalCharacterTemplateIdList.ContainsKey(location.BlockId))
		{
			element_AnimalAreaData.BlockAnimalCharacterTemplateIdList[location.BlockId].Add(templateId);
		}
		else
		{
			element_AnimalAreaData.BlockAnimalCharacterTemplateIdList.Add(location.BlockId, new List<short> { templateId });
		}
		DomainManager.Extra.SetAnimalAreaData(context, location.AreaId, element_AnimalAreaData);
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use ChangeBlockTemplate instead.")]
	public void ChangeBlockTemplateUnsafe(DataContext context, Location location, short blockTemplateId, bool isTurnVisible, bool isCheckCanChange)
	{
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use ChangeBlockTemplate instead.")]
	public void HeavenlyTreeChangeBlockTemplateUnsafe(DataContext context, Location location, short blockTemplateId)
	{
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use SplitMultiBlock instead.")]
	public void ChangeBigSizeBlockToSmallSizeBlockTemplateUnSafe(DataContext context, Location location)
	{
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use ChangeBlockTemplate instead.")]
	public void ChangeBlockTemplateUnSafe(DataContext context, Location location, short blockTemplateId)
	{
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use FiveLoongDlcEntry.MapBlockToLoong instead.")]
	public void ChangeBlockToLoongBlockTemplateUnSafe(DataContext context, Location location, MapBlockData centerBlock, short blockTemplateId)
	{
	}

	public void CreateFixedTutorialArea(DataContext context)
	{
		Dictionary<int, List<short>> dictionary = new Dictionary<int, List<short>>();
		Dictionary<int, List<short>> dictionary2 = new Dictionary<int, List<short>>();
		List<short> allKeys = MapBlock.Instance.GetAllKeys();
		for (int i = 0; i < allKeys.Count; i++)
		{
			MapBlockItem mapBlockItem = MapBlock.Instance[allKeys[i]];
			int type = (int)mapBlockItem.Type;
			int subType = (int)mapBlockItem.SubType;
			if (!dictionary.ContainsKey(type))
			{
				dictionary.Add(type, new List<short>());
			}
			dictionary[type].Add(mapBlockItem.TemplateId);
			if (!dictionary2.ContainsKey(subType))
			{
				dictionary2.Add(subType, new List<short>());
			}
			dictionary2[subType].Add(mapBlockItem.TemplateId);
		}
		DomainManager.Organization.BeginCreatingSettlements(context.Random);
		_swCreatingNormalAreas = new Stopwatch();
		_swCreatingSettlements = new Stopwatch();
		_swInitializingAreaTravelRoutes = new Stopwatch();
		CreateEmptyStateAreas(context);
		DomainManager.Organization.CreateEmptySects(context);
		MapAreaData mapAreaData = _areas[135];
		mapAreaData.Init(0, 135);
		_bornAreaBlocks.Init(0);
		SetElement_Areas(135, mapAreaData, context);
		MapAreaData mapAreaData2 = _areas[136];
		MapAreaItem mapAreaItem = MapArea.Instance[(short)136];
		mapAreaData2.Init(136, 136);
		_guideAreaBlocks.Init(mapAreaItem.Size * mapAreaItem.Size);
		CreateNormalArea(context, mapAreaData2, 136, dictionary, dictionary2);
		mapAreaData2.Discovered = true;
		SetElement_Areas(136, mapAreaData2, context);
		MapAreaData mapAreaData3 = _areas[137];
		mapAreaData3.Init(137, 137);
		_secretVillageAreaBlocks.Init(0);
		SetElement_Areas(137, mapAreaData3, context);
		short num = -1;
		sbyte stateIdByStateTemplateId = GetStateIdByStateTemplateId(DomainManager.World.GetTaiwuVillageStateTemplateId());
		for (int j = 0; j < 6; j++)
		{
			num = (short)(45 + stateIdByStateTemplateId * 6 + j);
			if (context.Random.NextBool())
			{
				break;
			}
		}
		Tester.Assert(num >= 0);
		MapAreaData mapAreaData4 = _areas[138];
		MapAreaItem config = GetElement_Areas(num).GetConfig();
		mapAreaData4.Init(config.TemplateId, 138);
		_brokenPerformAreaBlocks.Init(0);
		SetElement_Areas(138, mapAreaData4, context);
		DomainManager.Organization.EndCreatingSettlements(context);
		Logger.Info($"CreateNormalAreas: {_swCreatingNormalAreas.Elapsed.TotalMilliseconds:N1}");
		Logger.Info($"CreateSettlements: {_swCreatingSettlements.Elapsed.TotalMilliseconds:N1}");
		Logger.Info($"InitializeAreaTravelRoutes: {_swInitializingAreaTravelRoutes.Elapsed.TotalMilliseconds:N1}");
	}

	private void CreateEmptyStateAreas(DataContext context)
	{
		Dictionary<sbyte, List<short>> dictionary = new Dictionary<sbyte, List<short>>();
		List<short> allKeys = MapArea.Instance.GetAllKeys();
		for (int i = 31; i < allKeys.Count; i++)
		{
			short num = allKeys[i];
			sbyte stateID = MapArea.Instance[num].StateID;
			if (stateID >= 0)
			{
				if (!dictionary.ContainsKey(stateID))
				{
					dictionary.Add(stateID, new List<short>());
				}
				dictionary[stateID].Add(num);
			}
		}
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		for (int j = 0; j < 15; j++)
		{
			MapStateItem mapStateItem = MapState.Instance[j + 1];
			List<short> list2 = dictionary[mapStateItem.TemplateId];
			short num2 = list2[context.Random.Next(list2.Count)];
			for (int k = 0; k < 3; k++)
			{
				short num3 = (short)(j * 3 + k);
				short templateId = k switch
				{
					0 => mapStateItem.MainAreaID, 
					1 => mapStateItem.SectAreaID, 
					_ => num2, 
				};
				MapAreaData mapAreaData = _areas[num3];
				mapAreaData.Init(templateId, num3);
				MapAreaItem config = mapAreaData.GetConfig();
				if (config.StateID == DomainManager.World.GetTaiwuVillageStateTemplateId() && k == 2)
				{
					AreaBlockCollection regularAreaBlocks = GetRegularAreaBlocks(num3);
					regularAreaBlocks.Init(1);
					AddRegularBlockData(context, new Location(num3, 0), new MapBlockData(num3, 0, 0));
					short num4 = DomainManager.Organization.CreateSettlement(context, new Location(num3, 0), 16);
					Settlement settlement = DomainManager.Organization.GetSettlement(num4);
					short randomNameId = (short)((settlement is CivilianSettlement civilianSettlement) ? civilianSettlement.GetRandomNameId() : (-1));
					mapAreaData.SettlementInfos[1] = new SettlementInfo(num4, 0, settlement.GetOrgTemplateId(), randomNameId);
					DomainManager.Taiwu.SetTaiwuVillageSettlementId(num4, context);
					DomainManager.Building.CreateBuildingArea(context, num3, 0, 0);
					DomainManager.Building.AddTaiwuBuildingArea(context, new Location(num3, 0));
				}
				else
				{
					GetRegularAreaBlocks(num3).Init(0);
				}
			}
			list2.Remove(num2);
			list.AddRange(list2);
		}
		_brokenAreaBlocks.Init(0);
		for (int l = 0; l < list.Count; l++)
		{
			short num5 = (short)(45 + l);
			MapAreaData mapAreaData2 = _areas[num5];
			short templateId2 = list[l];
			mapAreaData2.Init(templateId2, num5);
		}
		ObjectPool<List<short>>.Instance.Return(list);
	}

	public void CreateAllAreas(DataContext context)
	{
		Dictionary<int, List<short>> dictionary = new Dictionary<int, List<short>>();
		Dictionary<int, List<short>> dictionary2 = new Dictionary<int, List<short>>();
		List<short> allKeys = MapBlock.Instance.GetAllKeys();
		for (int i = 0; i < allKeys.Count; i++)
		{
			MapBlockItem mapBlockItem = MapBlock.Instance[allKeys[i]];
			int type = (int)mapBlockItem.Type;
			int subType = (int)mapBlockItem.SubType;
			if (!dictionary.ContainsKey(type))
			{
				dictionary.Add(type, new List<short>());
			}
			dictionary[type].Add(mapBlockItem.TemplateId);
			if (!dictionary2.ContainsKey(subType))
			{
				dictionary2.Add(subType, new List<short>());
			}
			dictionary2[subType].Add(mapBlockItem.TemplateId);
		}
		DomainManager.Organization.BeginCreatingSettlements(context.Random);
		_swCreatingNormalAreas = new Stopwatch();
		_swCreatingSettlements = new Stopwatch();
		_swInitializingAreaTravelRoutes = new Stopwatch();
		CreateStateAreas(context, dictionary, dictionary2);
		_swInitializingAreaTravelRoutes.Start();
		InitAreaTravelRoute(context);
		_swInitializingAreaTravelRoutes.Stop();
		for (short num = 0; num < 135; num++)
		{
			SetElement_Areas(num, _areas[num], context);
		}
		sbyte taiwuVillageStateTemplateId = DomainManager.World.GetTaiwuVillageStateTemplateId();
		sbyte[] neighborStates = MapState.Instance[taiwuVillageStateTemplateId].NeighborStates;
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list.Add(GetStateIdByStateTemplateId(taiwuVillageStateTemplateId));
		for (int j = 0; j < neighborStates.Length; j++)
		{
			list.Add(GetStateIdByStateTemplateId(neighborStates[j]));
		}
		while (list.Count > GlobalConfig.Instance.MapInitUnlockStationStateCount)
		{
			list.RemoveAt(context.Random.Next(1, list.Count));
		}
		for (int k = 0; k < list.Count; k++)
		{
			GetAllAreaInState(list[k], list2);
			foreach (short item in list2)
			{
				if (!_areas[item].StationUnlocked)
				{
					UnlockStation(context, item, costAuthority: false);
				}
			}
		}
		DomainManager.Extra.SetStationInited(1, context);
		ObjectPool<List<sbyte>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		DomainManager.Adventure.GenerateBrokenAreaInitialEnemies(context);
		stopwatch.Stop();
		MapAreaData mapAreaData = _areas[135];
		MapAreaItem mapAreaItem = MapArea.Instance[(short)0];
		mapAreaData.Init(0, 135);
		_bornAreaBlocks.Init(mapAreaItem.Size * mapAreaItem.Size);
		CreateNormalArea(context, mapAreaData, 135, dictionary, dictionary2);
		mapAreaData.Discovered = true;
		SetElement_Areas(135, mapAreaData, context);
		DomainManager.Taiwu.TryAddVisitedSettlement(mapAreaData.SettlementInfos[0].SettlementId, context);
		MapAreaData mapAreaData2 = _areas[136];
		MapAreaItem mapAreaItem2 = MapArea.Instance[(short)136];
		mapAreaData2.Init(136, 136);
		_guideAreaBlocks.Init(mapAreaItem2.Size * mapAreaItem2.Size);
		CreateNormalArea(context, mapAreaData2, 136, dictionary, dictionary2);
		mapAreaData2.Discovered = true;
		SetElement_Areas(136, mapAreaData2, context);
		MapAreaData mapAreaData3 = _areas[137];
		MapAreaItem mapAreaItem3 = MapArea.Instance[(short)137];
		mapAreaData3.Init(mapAreaItem3.TemplateId, 137);
		_secretVillageAreaBlocks.Init(mapAreaItem3.Size * mapAreaItem3.Size);
		CreateNormalArea(context, mapAreaData3, 137, dictionary, dictionary2);
		mapAreaData3.Discovered = true;
		mapAreaData3.StationUnlocked = true;
		SetElement_Areas(137, mapAreaData3, context);
		short num2 = -1;
		sbyte stateIdByStateTemplateId = GetStateIdByStateTemplateId(DomainManager.World.GetTaiwuVillageStateTemplateId());
		for (int l = 0; l < 6; l++)
		{
			num2 = (short)(45 + stateIdByStateTemplateId * 6 + l);
			if (context.Random.NextBool())
			{
				break;
			}
		}
		Tester.Assert(num2 >= 0);
		MapAreaData mapAreaData4 = _areas[138];
		MapAreaItem config = GetElement_Areas(num2).GetConfig();
		mapAreaData4.Init(config.TemplateId, 138);
		_brokenPerformAreaBlocks.Init(config.Size * config.Size);
		CreateNormalArea(context, mapAreaData4, 138, dictionary, dictionary2);
		mapAreaData4.Discovered = true;
		mapAreaData4.StationUnlocked = true;
		SetElement_Areas(138, mapAreaData4, context);
		SetBlockAndViewRangeVisible(context, 138, mapAreaData4.StationBlockId);
		DomainManager.Organization.EndCreatingSettlements(context);
		InitializeTravelMap();
		Logger.Info($"CreateNormalAreas: {_swCreatingNormalAreas.Elapsed.TotalMilliseconds:N1}");
		Logger.Info($"CreateSettlements: {_swCreatingSettlements.Elapsed.TotalMilliseconds:N1}");
		Logger.Info($"InitializeAreaTravelRoutes: {_swInitializingAreaTravelRoutes.Elapsed.TotalMilliseconds:N1}");
		Logger.Info($"InitializeBrokenAreaEnemies: {stopwatch.Elapsed.TotalMilliseconds:N1}");
	}

	private void CreateStateAreas(DataContext context, Dictionary<int, List<short>> blockTypeDict, Dictionary<int, List<short>> blockSubTypeDict)
	{
		Dictionary<sbyte, List<short>> dictionary = new Dictionary<sbyte, List<short>>();
		List<short> allKeys = MapArea.Instance.GetAllKeys();
		for (int i = 31; i < allKeys.Count; i++)
		{
			short num = allKeys[i];
			sbyte stateID = MapArea.Instance[num].StateID;
			if (stateID >= 0)
			{
				if (!dictionary.ContainsKey(stateID))
				{
					dictionary.Add(stateID, new List<short>());
				}
				dictionary[stateID].Add(num);
			}
		}
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		for (int j = 0; j < 15; j++)
		{
			MapStateItem mapStateItem = MapState.Instance[j + 1];
			List<short> list3 = dictionary[mapStateItem.TemplateId];
			short num2 = list3[context.Random.Next(list3.Count)];
			for (int k = 0; k < 3; k++)
			{
				short num3 = (short)(j * 3 + k);
				short num4 = k switch
				{
					0 => mapStateItem.MainAreaID, 
					1 => mapStateItem.SectAreaID, 
					_ => num2, 
				};
				MapAreaData mapAreaData = _areas[num3];
				MapAreaItem mapAreaItem = MapArea.Instance[num4];
				mapAreaData.Init(num4, num3);
				byte b = mapAreaItem.Size;
				if (mapAreaItem.StateID == DomainManager.World.GetTaiwuVillageStateTemplateId() && k == 2)
				{
					b = (byte)GlobalConfig.Instance.TaiwuVillageForceAreaSize;
				}
				GetRegularAreaBlocks(num3).Init(b * b);
				CreateNormalArea(context, mapAreaData, num3, blockTypeDict, blockSubTypeDict, k);
			}
			list3.Remove(num2);
			list.AddRange(list3);
		}
		list2.Clear();
		list2.Add(118);
		list2.Add(119);
		list2.Add(120);
		list2.Add(121);
		list2.Add(122);
		list2.Add(123);
		_brokenAreaBlocks.Init(2250);
		for (int l = 0; l < list.Count; l++)
		{
			short num5 = (short)(45 + l);
			MapAreaData mapAreaData2 = _areas[num5];
			short templateId = list[l];
			mapAreaData2.Init(templateId, num5);
			CreateBrokenArea(context, _areas[num5], num5, list2);
		}
		for (sbyte b2 = 0; b2 < 45; b2++)
		{
			int m = 0;
			for (int num6 = context.Random.Next(3, 6); m < num6; m++)
			{
				DomainManager.Extra.AnimalRandomGenerateInArea(context, b2);
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
	}

	private unsafe void CreateNormalArea(DataContext context, MapAreaData mapAreaData, short areaId, Dictionary<int, List<short>> blockTypeDict, Dictionary<int, List<short>> blockSubTypeDict, int indexInState = -1)
	{
		_swCreatingNormalAreas.Start();
		short* ptr = stackalloc short[4];
		short* ptr2 = stackalloc short[4];
		*ptr = 1;
		ptr[1] = -1;
		ptr[2] = 0;
		ptr[3] = 0;
		*ptr2 = 0;
		ptr2[1] = 0;
		ptr2[2] = 1;
		ptr2[3] = -1;
		MapAreaItem config = mapAreaData.GetConfig();
		bool flag = config.StateID == DomainManager.World.GetTaiwuVillageStateTemplateId() && indexInState == 2;
		byte areaSize = config.Size;
		if (flag)
		{
			areaSize = (byte)GlobalConfig.Instance.TaiwuVillageForceAreaSize;
		}
		MapBlockData[] areaBlocks = ((areaId < 45) ? GetRegularAreaBlocks(areaId) : ((areaId == 135) ? _bornAreaBlocks : ((areaId == 136) ? _guideAreaBlocks : ((areaId == 138) ? _brokenPerformAreaBlocks : _secretVillageAreaBlocks)))).GetArray();
		int num = config.OrganizationId.Length + (flag ? 1 : 0);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		if (config.CustomBlockConfig == null)
		{
			int num2 = areaSize * 3 / 4;
			if (flag)
			{
				num2 = Math.Max(num2, SharedConstValue.TaiwuEnsuredSurroundingBlockOffsets.Select(((int, int) p) => Math.Abs(p.Item1 + p.Item2)).Max());
			}
			byte[,] areaShape = GetAreaShape(context.Random, areaSize, (byte)num2, ensureEdge: true);
			byte[,] array = new byte[areaSize, areaSize];
			for (byte b = 0; b < areaSize; b++)
			{
				for (byte b2 = 0; b2 < areaSize; b2++)
				{
					if (areaShape[b, b2] == 0)
					{
						short num3 = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(b, b2), areaSize);
						areaBlocks[num3] = new MapBlockData(areaId, num3, 126);
						array[b, b2] = 1;
					}
				}
			}
			PlaceStaticBlocks(areaId, config, areaSize, areaBlocks, array, areaShape, flag, list, context.Random);
			mapAreaData.Discovered = flag;
			PlaceOtherBlocks(areaId, config, areaSize, areaBlocks, array, list, num, context.Random);
			FixSeriesBlocks(config, areaSize, areaBlocks, blockTypeDict, context.Random);
			FixEncircleBlocks(config, areaSize, areaBlocks, blockTypeDict, context.Random);
		}
		else
		{
			short[][] array2 = CustomMapBlockConfig.Data[config.CustomBlockConfig];
			areaSize = (byte)array2.GetLength(0);
			for (byte b3 = 0; b3 < array2.Length; b3++)
			{
				short[] array3 = array2[b3];
				for (byte b4 = 0; b4 < array3.Length; b4++)
				{
					short num4 = array3[b4];
					MapBlockItem mapBlockItem = MapBlock.Instance[num4];
					if (mapBlockItem == null)
					{
						continue;
					}
					ByteCoordinate byteCoordinate = new ByteCoordinate(b3, b4);
					short num5 = ByteCoordinate.CoordinateToIndex(byteCoordinate, areaSize);
					MapBlockData mapBlockData = areaBlocks[num5];
					if (mapBlockData == null)
					{
						mapBlockData = new MapBlockData(areaId, num5, num4);
						mapBlockData.Visible = true;
						areaBlocks[num5] = mapBlockData;
						if (mapBlockData.IsCityTown())
						{
							list.Add(num5);
						}
					}
					else if (-1 != mapBlockData.RootBlockId)
					{
						continue;
					}
					if (mapBlockItem.Size <= 1)
					{
						continue;
					}
					for (byte b5 = 0; b5 < mapBlockItem.Size; b5++)
					{
						for (byte b6 = 0; b6 < mapBlockItem.Size; b6++)
						{
							if (b5 + b6 != 0 && byteCoordinate.X + b5 < areaSize && byteCoordinate.Y + b6 < areaSize)
							{
								ByteCoordinate byteCoordinate2 = new ByteCoordinate((byte)(byteCoordinate.X + b5), (byte)(byteCoordinate.Y + b6));
								short num6 = ByteCoordinate.CoordinateToIndex(byteCoordinate2, areaSize);
								areaBlocks[num6] = new MapBlockData(areaId, num6, -1);
								areaBlocks[num6].SetToSizeBlock(mapBlockData);
								areaBlocks[num6].Visible = true;
							}
						}
					}
				}
			}
			List<MapBlockData> list2 = ObjectPool<List<MapBlockData>>.Instance.Get();
			for (int num7 = 0; num7 < list.Count; num7++)
			{
				bool flag2 = false;
				if (!list.CheckIndex(num7))
				{
					flag2 = true;
					AdaptableLog.Warning($"staticBlockIdList invalid index: {num7}");
				}
				if (!areaBlocks.CheckIndex(list[num7]))
				{
					flag2 = true;
					AdaptableLog.Warning($"areaBlocks invalid index: {list[num7]}");
				}
				if (flag2)
				{
					PrintAreaDebug(areaBlocks, areaSize);
					StringBuilder stringBuilder = new StringBuilder();
					int num8 = 0;
					for (int count = list.Count; num8 < count; num8++)
					{
						if (num8 != 0)
						{
							stringBuilder.Append(", ");
						}
						StringBuilder stringBuilder2 = stringBuilder;
						StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
						handler.AppendFormatted(list[num8]);
						stringBuilder2.Append(ref handler);
					}
					AdaptableLog.Warning($"{"staticBlockIdList"}: {stringBuilder}");
					continue;
				}
				MapBlockData mapBlockData2 = areaBlocks[list[num7]];
				MapBlockItem config2 = mapBlockData2.GetConfig();
				if (config2.Range <= 0)
				{
					continue;
				}
				ByteCoordinate byteCoordinate3 = ByteCoordinate.IndexToCoordinate(mapBlockData2.BlockId, areaSize);
				list2.Clear();
				for (byte b7 = (byte)Math.Max(byteCoordinate3.X - config2.Range, 0); b7 < Math.Min(byteCoordinate3.X + config2.Size + config2.Range, areaSize); b7++)
				{
					for (byte b8 = (byte)Math.Max(byteCoordinate3.Y - config2.Range, 0); b8 < Math.Min(byteCoordinate3.Y + config2.Size + config2.Range, areaSize); b8++)
					{
						MapBlockData rootBlock = areaBlocks[ByteCoordinate.CoordinateToIndex(new ByteCoordinate(b7, b8), areaSize)].GetRootBlock();
						if (rootBlock.BlockId != mapBlockData2.BlockId && mapBlockData2.GetManhattanDistanceToPos(b7, b8) <= config2.Range && rootBlock.IsPassable() && !list2.Contains(rootBlock))
						{
							list2.Add(rootBlock);
						}
					}
				}
				foreach (MapBlockData item2 in list2)
				{
					if (item2.IsPassable() && item2.GetConfig().Size == 1 && item2.BelongBlockId == -1)
					{
						item2.BelongBlockId = mapBlockData2.BlockId;
					}
				}
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(list2);
		}
		List<MapBlockData> list3 = ObjectPool<List<MapBlockData>>.Instance.Get();
		list3.Clear();
		for (short num9 = 0; num9 < areaBlocks.Length; num9++)
		{
			MapBlockData mapBlockData3 = areaBlocks[num9];
			if (mapBlockData3.IsPassable())
			{
				mapBlockData3.InitResources(context.Random);
				if (mapBlockData3.GetConfig().MaxMalice > 0)
				{
					list3.Add(mapBlockData3);
				}
			}
		}
		int num10 = list3.Count * 5 / 100;
		for (int num11 = 0; num11 < num10; num11++)
		{
			int index = context.Random.Next(list3.Count);
			MapBlockData mapBlockData4 = list3[index];
			short maxMalice = mapBlockData4.GetMaxMalice();
			int num12 = RedzenHelper.SkewDistribute(context.Random, 20f, 8f, 1.8f, 0, 80);
			mapBlockData4.Malice = (short)(maxMalice * num12 / 100);
			list3.RemoveAt(index);
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list3);
		if (areaId < 45)
		{
			for (short num13 = 0; num13 < areaBlocks.Length; num13++)
			{
				AddRegularBlockData(context, new Location(areaId, num13), areaBlocks[num13]);
			}
			if (mapAreaData.StationBlockId >= 0)
			{
				areaBlocks[mapAreaData.StationBlockId].SetVisible(visible: true, context);
			}
		}
		else if (areaId == 135)
		{
			for (short num14 = 0; num14 < areaBlocks.Length; num14++)
			{
				AddElement_BornAreaBlocks(num14, areaBlocks[num14], context);
			}
		}
		else if (areaId == 136)
		{
			for (short num15 = 0; num15 < areaBlocks.Length; num15++)
			{
				AddElement_GuideAreaBlocks(num15, areaBlocks[num15], context);
			}
		}
		else if (areaId == 137)
		{
			for (short num16 = 0; num16 < areaBlocks.Length; num16++)
			{
				AddElement_SecretVillageAreaBlocks(num16, areaBlocks[num16], context);
			}
		}
		else if (areaId == 138)
		{
			for (short num17 = 0; num17 < areaBlocks.Length; num17++)
			{
				AddElement_BrokenPerformAreaBlocks(num17, areaBlocks[num17], context);
			}
		}
		List<MapBlockData> list4 = ObjectPool<List<MapBlockData>>.Instance.Get();
		bool flag3 = areaId == 138;
		if (flag3)
		{
			num = 1;
		}
		short taiwuVillageBlockId = -1;
		for (int num18 = num - 1; num18 >= 0; num18--)
		{
			short num19 = list[num18];
			Location location = new Location(areaId, num19);
			MapBlockData mapBlockData5 = areaBlocks[num19];
			bool flag4 = flag && num18 == num - 1;
			sbyte b9 = (sbyte)(flag4 ? 16 : (flag3 ? 38 : config.OrganizationId[num18]));
			_swCreatingSettlements.Start();
			short num20 = DomainManager.Organization.CreateSettlement(context, location, b9);
			_swCreatingSettlements.Stop();
			Settlement settlement = DomainManager.Organization.GetSettlement(num20);
			short randomNameId = (short)((settlement is CivilianSettlement civilianSettlement) ? civilianSettlement.GetRandomNameId() : (-1));
			mapAreaData.SettlementInfos[num18] = new SettlementInfo(num20, num19, settlement.GetOrgTemplateId(), randomNameId);
			if (flag4)
			{
				taiwuVillageBlockId = mapBlockData5.BlockId;
				DomainManager.Taiwu.SetTaiwuVillageSettlementId(num20, context);
			}
			if (areaId != 137)
			{
				GetNeighborBlocks(areaId, num19, list4, (areaId != 135) ? 1 : mapBlockData5.GetConfig().ViewRange);
				mapBlockData5.SetVisible(visible: true, context);
				if (mapBlockData5.GroupBlockList != null)
				{
					for (int num21 = 0; num21 < mapBlockData5.GroupBlockList.Count; num21++)
					{
						mapBlockData5.SetVisible(visible: true, context);
					}
				}
				for (int num22 = 0; num22 < list4.Count; num22++)
				{
					MapBlockData mapBlockData6 = list4[num22];
					mapBlockData6.SetVisible(visible: true, context);
					if (mapBlockData6.GroupBlockList != null)
					{
						for (int num23 = 0; num23 < mapBlockData6.GroupBlockList.Count; num23++)
						{
							mapBlockData6.GroupBlockList[num23].SetVisible(visible: true, context);
						}
					}
				}
			}
			if (areaBlocks[num19].IsCityTown())
			{
				DomainManager.Building.CreateBuildingArea(context, areaId, num19, areaBlocks[num19].TemplateId);
				if (b9 == 16)
				{
					DomainManager.Building.AddTaiwuBuildingArea(context, new Location(areaId, num19));
				}
			}
		}
		List<Location> locations;
		ByteCoordinate origin;
		sbyte offsetX;
		sbyte offsetY;
		if (taiwuVillageBlockId >= 0)
		{
			locations = new List<Location>();
			(sbyte, sbyte, sbyte)* ptr3 = stackalloc(sbyte, sbyte, sbyte)[8];
			Unsafe.Write(ptr3, ((sbyte)1, (sbyte)0, (sbyte)0));
			Unsafe.Write(ptr3 + 1, ((sbyte)0, (sbyte)1, (sbyte)0));
			Unsafe.Write(ptr3 + 2, ((sbyte)(-1), (sbyte)0, (sbyte)1));
			Unsafe.Write(ptr3 + 3, ((sbyte)0, (sbyte)(-1), (sbyte)1));
			Unsafe.Write(ptr3 + 4, ((sbyte)1, (sbyte)1, (sbyte)(-1)));
			Unsafe.Write(ptr3 + 5, ((sbyte)(-1), (sbyte)(-1), (sbyte)0));
			Unsafe.Write(ptr3 + 6, ((sbyte)(-1), (sbyte)1, (sbyte)(-1)));
			Unsafe.Write(ptr3 + 7, ((sbyte)1, (sbyte)(-1), (sbyte)(-1)));
			CollectionUtils.Shuffle<(sbyte, sbyte, sbyte)>(context.Random, ptr3, 8);
			origin = ByteCoordinate.IndexToCoordinate(taiwuVillageBlockId, areaSize);
			for (int num24 = 0; num24 < 8; num24++)
			{
				(sbyte, sbyte, sbyte) tuple = ptr3[num24];
				offsetX = tuple.Item1;
				offsetY = tuple.Item2;
				sbyte item = tuple.Item3;
				int num25 = ((num24 == 0) ? 3 : context.Random.Next(6, 8)) + item;
				ByteCoordinate byteCoordinate4 = GiveCoordinate(num25);
				while (!IsBigBlockCanPlace(areaBlocks[ByteCoordinate.CoordinateToIndex(byteCoordinate4, areaSize)], 2))
				{
					ByteCoordinate byteCoordinate5 = byteCoordinate4;
					for (int num26 = 0; num26 < 4; num26++)
					{
						MapBlockData mapBlockData7 = areaBlocks[ByteCoordinate.CoordinateToIndex(byteCoordinate4, areaSize)];
						if (IsBigBlockCanPlace(mapBlockData7, 2))
						{
							break;
						}
						if (mapBlockData7.RootBlockId >= 0 && IsBigBlockCanPlace(areaBlocks[mapBlockData7.RootBlockId], areaSize))
						{
							byteCoordinate4 = ByteCoordinate.IndexToCoordinate(mapBlockData7.RootBlockId, areaSize);
							break;
						}
						int num27 = byteCoordinate5.X + offsetX;
						int num28 = byteCoordinate5.Y + offsetY;
						if (num27 < 0 || num27 >= areaSize)
						{
							num28 += ptr2[num26];
						}
						if (num28 < 0 || num28 >= areaSize)
						{
							num27 += ptr[num26];
						}
						if (num27 >= 0 && num27 < areaSize)
						{
							byteCoordinate4.X = (byte)num27;
						}
						if (num28 >= 0 && num28 < areaSize)
						{
							byteCoordinate4.Y = (byte)num28;
						}
					}
					if (!IsBigBlockCanPlace(areaBlocks[ByteCoordinate.CoordinateToIndex(byteCoordinate4, areaSize)], 2))
					{
						num25++;
						ByteCoordinate byteCoordinate6 = GiveCoordinate(num25);
						if (byteCoordinate6.X == 0 || byteCoordinate6.Y == 0 || byteCoordinate6.X == areaSize - 1 || byteCoordinate6.Y == areaSize - 1)
						{
							PrintBigBlockCanPlaceMap();
							throw new Exception("can not place sword tomb, need re-run: CreateNormalArea");
						}
						byteCoordinate4 = byteCoordinate6;
					}
				}
				locations.Add(new Location(areaId, ByteCoordinate.CoordinateToIndex(byteCoordinate4, areaSize)));
			}
			int num29 = 0;
			foreach (Location item3 in locations.OrderBy((Location loc) => origin.GetManhattanDistance(ByteCoordinate.IndexToCoordinate(loc.BlockId, areaSize))))
			{
				SetElement_SwordTombLocations(num29, item3, context);
				num29++;
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list4);
		ObjectPool<List<short>>.Instance.Return(list);
		_swCreatingNormalAreas.Stop();
		ByteCoordinate GiveCoordinate(int distance)
		{
			return new ByteCoordinate((byte)Math.Clamp(origin.X + offsetX * distance, 0, areaSize - 1), (byte)Math.Clamp(origin.Y + offsetY * distance, 0, areaSize - 1));
		}
		bool IsBigBlockCanPlace(MapBlockData block, byte size)
		{
			if (!block.CanChangeBlockType() || locations.Contains(new Location(areaId, block.BlockId)))
			{
				return false;
			}
			ByteCoordinate byteCoordinate7 = ByteCoordinate.IndexToCoordinate(block.BlockId, areaSize);
			if (size <= 1)
			{
				return true;
			}
			for (byte b10 = 0; b10 < size; b10++)
			{
				for (byte b11 = 0; b11 < size; b11++)
				{
					if (b10 + b11 != 0 && byteCoordinate7.X + b10 < areaSize && byteCoordinate7.Y + b11 < areaSize)
					{
						ByteCoordinate byteCoordinate8 = new ByteCoordinate((byte)(byteCoordinate7.X + b10), (byte)(byteCoordinate7.Y + b11));
						short num30 = ByteCoordinate.CoordinateToIndex(byteCoordinate8, areaSize);
						MapBlockData mapBlockData8 = areaBlocks[num30];
						if (!mapBlockData8.CanChangeBlockType() || mapAreaData.StationBlockId == num30 || (mapBlockData8.GroupBlockList != null && mapBlockData8.GroupBlockList.Count > 0) || locations.Contains(new Location(areaId, num30)))
						{
							return false;
						}
					}
				}
			}
			return true;
		}
		void PrintBigBlockCanPlaceMap()
		{
			StringBuilder stringBuilder3 = new StringBuilder();
			int num30 = 0;
			while (true)
			{
				if (num30 % areaSize == 0 && stringBuilder3.Length > 0)
				{
					AdaptableLog.Info(stringBuilder3.ToString());
					stringBuilder3.Clear();
				}
				if (num30 == areaBlocks.Length)
				{
					break;
				}
				bool flag5 = IsBigBlockCanPlace(areaBlocks[num30], 2);
				if (num30 == taiwuVillageBlockId)
				{
					stringBuilder3.Append("c ");
				}
				else
				{
					StringBuilder stringBuilder4 = stringBuilder3;
					StringBuilder.AppendInterpolatedStringHandler handler2 = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder4);
					handler2.AppendFormatted(flag5 ? '0' : 'x');
					handler2.AppendLiteral(" ");
					stringBuilder4.Append(ref handler2);
				}
				num30++;
			}
		}
	}

	public static void PrintAreaDebug(Span<MapBlockData> blockCollection, byte areaSize)
	{
		StringBuilder stringBuilder = new StringBuilder();
		AdaptableLog.Info("The blockCollection is:");
		for (int i = 0; i < areaSize; i++)
		{
			stringBuilder.Clear();
			for (int j = 0; j < areaSize; j++)
			{
				short index = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)j, (byte)i), areaSize);
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
				handler.AppendLiteral(" ");
				handler.AppendFormatted(blockCollection[index].TemplateId, "D3");
				stringBuilder2.Append(ref handler);
			}
			AdaptableLog.Info(stringBuilder.ToString());
		}
		AdaptableLog.Info("The BelongBlockId is:");
		for (int k = 0; k < areaSize; k++)
		{
			stringBuilder.Clear();
			for (int l = 0; l < areaSize; l++)
			{
				short index2 = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)l, (byte)k), areaSize);
				int num = ((blockCollection[index2] != null) ? blockCollection[index2].BelongBlockId : (-1));
				stringBuilder.Append((num >= 0) ? $" {num:D3}" : " XXX");
			}
			AdaptableLog.Info(stringBuilder.ToString());
		}
		AdaptableLog.Info("The TemplateId is:");
		for (int m = 0; m < areaSize; m++)
		{
			stringBuilder.Clear();
			for (int n = 0; n < areaSize; n++)
			{
				short index3 = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)n, (byte)m), areaSize);
				int num2 = ((blockCollection[index3] != null) ? blockCollection[index3].TemplateId : (-1));
				stringBuilder.Append((num2 >= 0) ? $" {num2:D3}" : " XXX");
			}
			AdaptableLog.Info(stringBuilder.ToString());
		}
	}

	private void CreateBrokenArea(DataContext context, MapAreaData mapAreaData, short areaId, List<short> ruinBlockRandomPool)
	{
		byte b = 5;
		short num = (short)(b * b * (areaId - 45));
		byte[,] areaShape = GetAreaShape(context.Random, b, b, ensureEdge: true);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		for (byte b2 = 0; b2 < b; b2++)
		{
			for (byte b3 = 0; b3 < b; b3++)
			{
				short num2 = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(b2, b3), b);
				bool flag = areaShape[b2, b3] != 0;
				short templateId = (short)(flag ? ruinBlockRandomPool[context.Random.Next(ruinBlockRandomPool.Count)] : 126);
				MapBlockData mapBlockData = new MapBlockData(areaId, num2, templateId);
				if (flag)
				{
					list.Add(num2);
				}
				mapBlockData.InitResources(context.Random);
				AddElement_BrokenAreaBlocks((short)(num + num2), mapBlockData, context);
			}
		}
		mapAreaData.Discovered = false;
		mapAreaData.StationBlockId = list[context.Random.Next(list.Count)];
		MapBlockData mapBlockData2 = _brokenAreaBlocks[(short)(num + mapAreaData.StationBlockId)];
		mapBlockData2.ChangeTemplateId(38, checkCanChange: false);
		mapBlockData2.SetVisible(visible: true, context);
		SetElement_BrokenAreaBlocks((short)(num + mapAreaData.StationBlockId), mapBlockData2, context);
		ObjectPool<List<short>>.Instance.Return(list);
	}

	private byte[,] GetAreaShape(IRandomSource random, byte mapSize, byte keepRange, bool ensureEdge = false)
	{
		if (mapSize < keepRange)
		{
			AdaptableLog.TagWarning("MapCreate", "error,sizeMax < sizeMin");
			return null;
		}
		byte[,] map = new byte[mapSize, mapSize];
		byte b = (byte)(mapSize / 2 - 1);
		float num = (float)(int)keepRange / 2f;
		ByteCoordinate byteCoordinate = new ByteCoordinate(b, b);
		List<ByteCoordinate> list = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		list.Clear();
		for (byte b2 = 0; b2 < mapSize; b2++)
		{
			for (byte b3 = 0; b3 < mapSize; b3++)
			{
				ByteCoordinate byteCoordinate2 = new ByteCoordinate(b2, b3);
				if (ByteCoordinate.Distance(byteCoordinate, byteCoordinate2) > (double)num)
				{
					byte b4 = 50;
					if (ensureEdge && (b2 == 0 || b2 == mapSize - 1 || b3 == 0 || b3 == mapSize - 1))
					{
						b4 += 25;
					}
					map[b2, b3] = (byte)((random.Next(100) < b4) ? 1u : 0u);
					list.Add(byteCoordinate2);
				}
				else
				{
					map[b2, b3] = 1;
				}
			}
		}
		for (int i = 0; i < 1; i++)
		{
			for (int j = 0; j < list.Count; j++)
			{
				ByteCoordinate byteCoordinate3 = list[j];
				int num2 = CountWalls(byteCoordinate3.X, byteCoordinate3.Y);
				if (num2 > 4)
				{
					map[byteCoordinate3.X, byteCoordinate3.Y] = 0;
				}
				else if (num2 < 4)
				{
					map[byteCoordinate3.X, byteCoordinate3.Y] = 1;
				}
			}
		}
		ObjectPool<List<ByteCoordinate>>.Instance.Return(list);
		List<ByteCoordinate> island = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		List<ByteCoordinate> list2 = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		island.Clear();
		SpreadIsland(byteCoordinate, island, mapSize, CanPass, new byte[mapSize, mapSize]);
		for (byte b5 = 0; b5 < mapSize; b5++)
		{
			for (byte b6 = 0; b6 < mapSize; b6++)
			{
				if (!island.Contains(new ByteCoordinate(b5, b6)))
				{
					map[b5, b6] = 0;
				}
			}
		}
		island.Clear();
		list2.Clear();
		for (byte b7 = 0; b7 < mapSize - 1; b7++)
		{
			if (map[0, b7] == 0)
			{
				list2.Add(new ByteCoordinate(0, b7));
			}
			if (map[(byte)(b7 + 1), (byte)(mapSize - 1)] == 0)
			{
				list2.Add(new ByteCoordinate((byte)(b7 + 1), (byte)(mapSize - 1)));
			}
			if (map[b7, (byte)(mapSize - 1)] == 0)
			{
				list2.Add(new ByteCoordinate(b7, (byte)(mapSize - 1)));
			}
			if (map[(byte)(mapSize - 1), (byte)(b7 + 1)] == 0)
			{
				list2.Add(new ByteCoordinate((byte)(mapSize - 1), (byte)(b7 + 1)));
			}
		}
		while (list2.Count > 0)
		{
			SpreadIsland(list2[0], island, mapSize, IsWall, new byte[mapSize, mapSize]);
			list2.RemoveAll((ByteCoordinate pos) => island.Contains(pos));
		}
		for (byte b8 = 0; b8 < mapSize; b8++)
		{
			for (byte b9 = 0; b9 < mapSize; b9++)
			{
				if (!island.Contains(new ByteCoordinate(b8, b9)))
				{
					map[b8, b9] = 1;
				}
			}
		}
		for (byte b10 = 0; b10 < mapSize; b10++)
		{
			for (byte b11 = 0; b11 < mapSize; b11++)
			{
				if (map[b10, b11] == 1 && (b10 == 0 || b10 == mapSize - 1 || b11 == 0 || b11 == mapSize - 1 || map[b10 - 1, b11] == 0 || map[b10 + 1, b11] == 0 || map[b10, b11 - 1] == 0 || map[b10, b11 + 1] == 0))
				{
					map[b10, b11] = 2;
				}
			}
		}
		ObjectPool<List<ByteCoordinate>>.Instance.Return(island);
		ObjectPool<List<ByteCoordinate>>.Instance.Return(list2);
		return map;
		bool CanPass(ByteCoordinate pos)
		{
			return map[pos.X, pos.Y] == 1;
		}
		int CountWalls(int x, int y)
		{
			int num3 = 0;
			for (int k = x - 1; k <= x + 1; k++)
			{
				for (int l = y - 1; l <= y + 1; l++)
				{
					if (k != x || l != y)
					{
						num3 = ((k < 0 || k >= mapSize || l < 0 || l >= mapSize) ? (num3 + 1) : (num3 + ((map[k, l] == 0) ? 1 : 0)));
					}
				}
			}
			return num3;
		}
		bool IsWall(ByteCoordinate pos)
		{
			return map[pos.X, pos.Y] == 0;
		}
	}

	private void PlaceStaticBlocks(short areaId, MapAreaItem areaConfigData, byte mapSize, MapBlockData[] areaBlocks, byte[,] availableBlockMap, byte[,] edgeClipMap, bool createTaiwuVillage, List<short> staticBlockIds, IRandomSource random)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		if (areaId == 138)
		{
			list.Add(36);
		}
		else
		{
			list.AddRange(areaConfigData.SettlementBlockCore);
		}
		int num = list.Count;
		if (createTaiwuVillage)
		{
			list.Add(0);
		}
		if (areaConfigData.SceneryBlockCore != null)
		{
			list.AddRange(areaConfigData.SceneryBlockCore);
		}
		if (areaConfigData.BigBaseBlockCore != null)
		{
			for (int i = 0; i < areaConfigData.BigBaseBlockCore.Count; i++)
			{
				short[] array = areaConfigData.BigBaseBlockCore[i];
				int num2 = ((array[1] == 1) ? 1 : random.Next(1, (int)array[1]));
				for (int j = 0; j < num2; j++)
				{
					list.Add(array[0]);
				}
			}
		}
		staticBlockIds.Clear();
		if (list.Contains(areaConfigData.CenterBlock))
		{
			ByteCoordinate byteCoordinate = new ByteCoordinate((byte)((mapSize - 1) / 2), (byte)((mapSize - 1) / 2));
			PlaceStaticBlock(areaId, areaConfigData, mapSize, areaBlocks, areaConfigData.CenterBlock, byteCoordinate, availableBlockMap, random);
			staticBlockIds.Add(ByteCoordinate.CoordinateToIndex(byteCoordinate, mapSize));
			list.Remove(areaConfigData.CenterBlock);
			num--;
		}
		if (list.Count > 0)
		{
			List<ByteCoordinate> list2 = ObjectPool<List<ByteCoordinate>>.Instance.Get();
			ByteCoordinate center = new ByteCoordinate((byte)((mapSize - 1) / 2), (byte)((mapSize - 1) / 2));
			int maxBlockRange = 0;
			MapBlock.Instance.Iterate(delegate(MapBlockItem b)
			{
				maxBlockRange = Math.Max(maxBlockRange, b.Range * 2);
				return true;
			});
			for (int num3 = 0; num3 < list.Count; num3++)
			{
				MapBlockItem blockConfig = MapBlock.Instance[list[num3]];
				GetStaticBlockPosRandomPool(blockConfig, mapSize, availableBlockMap, edgeClipMap, list2);
				list2.RemoveAll((ByteCoordinate coord) => coord.GetManhattanDistance(center) <= maxBlockRange);
				if (num3 < num && list2.Count == 0)
				{
					GetStaticBlockPosRandomPool(blockConfig, mapSize, availableBlockMap, edgeClipMap, list2, calcRange: false);
				}
				list2.RemoveAll((ByteCoordinate coord) => coord.GetManhattanDistance(center) <= maxBlockRange);
				list2.RemoveAll(delegate(ByteCoordinate coord)
				{
					short num4 = ByteCoordinate.CoordinateToIndex(coord, mapSize);
					byte blockRange = GetBlockRange(areaId, blockConfig.TemplateId);
					int num5 = Math.Max(0, coord.X - blockRange);
					int num6 = Math.Max(0, coord.Y - blockRange);
					int num7 = Math.Min(mapSize - 1, coord.X + blockConfig.Size + blockRange);
					int num8 = Math.Min(mapSize - 1, coord.Y + blockConfig.Size + blockRange);
					for (byte b = (byte)num5; b < num7; b++)
					{
						for (byte b2 = (byte)num6; b2 < num8; b2++)
						{
							short num9 = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(b, b2), mapSize);
							MapBlockData mapBlockData = areaBlocks[num9];
							if (mapBlockData != null && (mapBlockData.BelongBlockId >= 0 || mapBlockData.IsCityTown()))
							{
								return true;
							}
						}
					}
					return areaBlocks[num4] != null && (areaBlocks[num4].BelongBlockId >= 0 || areaBlocks[num4].IsCityTown());
				});
				if (num3 < num && list2.Count == 0)
				{
					throw new Exception($"Area {areaId} is too small to place the {num3} static block {blockConfig.TemplateId}");
				}
				bool flag = createTaiwuVillage && num3 == areaConfigData.SettlementBlockCore.Length;
				if (list2.Count > 0)
				{
					ByteCoordinate byteCoordinate2 = ((flag || (areaId == 138 && blockConfig.TemplateId == 36)) ? center : list2.GetRandom(random));
					PlaceStaticBlock(areaId, areaConfigData, mapSize, areaBlocks, list[num3], byteCoordinate2, availableBlockMap, random, num3 == (createTaiwuVillage ? areaConfigData.SettlementBlockCore.Length : areaConfigData.StationLocate), flag);
					staticBlockIds.Add(ByteCoordinate.CoordinateToIndex(byteCoordinate2, mapSize));
				}
			}
			ObjectPool<List<ByteCoordinate>>.Instance.Return(list2);
		}
		ObjectPool<List<short>>.Instance.Return(list);
	}

	private void GetStaticBlockPosRandomPool(MapBlockItem blockConfig, byte mapSize, byte[,] availableBlockMap, byte[,] edgeClipMap, List<ByteCoordinate> posList, bool calcRange = true)
	{
		posList.Clear();
		for (byte b = 0; b < mapSize; b++)
		{
			for (byte b2 = 0; b2 < mapSize; b2++)
			{
				bool flag = true;
				int num = (calcRange ? (b - blockConfig.Range) : b);
				int num2 = (calcRange ? (b2 - blockConfig.Range) : b2);
				int num3 = (calcRange ? (b + blockConfig.Size + blockConfig.Range) : (b + blockConfig.Size));
				int num4 = (calcRange ? (b2 + blockConfig.Size + blockConfig.Range) : (b2 + blockConfig.Size));
				for (int i = num; i < num3; i++)
				{
					for (int j = num2; j < num4; j++)
					{
						if (i <= 0 || i >= mapSize - 1 || j <= 0 || j >= mapSize - 1 || availableBlockMap[i, j] == 1 || edgeClipMap[i, j] != 1)
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
				if (flag)
				{
					posList.Add(new ByteCoordinate(b, b2));
				}
			}
		}
	}

	private byte GetBlockRange(short areaId, short blockTemplateId)
	{
		MapBlockItem mapBlockItem = MapBlock.Instance[blockTemplateId];
		if (areaId == 138 && mapBlockItem.TemplateId == 36)
		{
			return 2;
		}
		return mapBlockItem.Range;
	}

	private byte GetBlockRange(MapBlockData blockData)
	{
		return GetBlockRange(blockData.AreaId, blockData.TemplateId);
	}

	private void PlaceStaticBlock(short areaId, MapAreaItem areaConfigData, byte areaSize, MapBlockData[] areaBlocks, short blockTemplateId, ByteCoordinate byteCoordinate, byte[,] availableBlockMap, IRandomSource random, bool hasStation = false, bool createTaiwuVillage = false)
	{
		short num = ByteCoordinate.CoordinateToIndex(byteCoordinate, areaSize);
		MapBlockData mapBlockData = new MapBlockData(areaId, num, blockTemplateId);
		MapBlockItem mapBlockItem = MapBlock.Instance[blockTemplateId];
		List<ByteCoordinate> staticBlockLocationList = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		areaBlocks[num] = mapBlockData;
		staticBlockLocationList.Clear();
		staticBlockLocationList.Add(byteCoordinate);
		availableBlockMap[byteCoordinate.X, byteCoordinate.Y] = 1;
		if (mapBlockItem.Size > 1)
		{
			for (byte b = 0; b < mapBlockItem.Size; b++)
			{
				for (byte b2 = 0; b2 < mapBlockItem.Size; b2++)
				{
					if (b + b2 != 0 && byteCoordinate.X + b < areaSize && byteCoordinate.Y + b2 < areaSize)
					{
						ByteCoordinate byteCoordinate2 = new ByteCoordinate((byte)(byteCoordinate.X + b), (byte)(byteCoordinate.Y + b2));
						short num2 = ByteCoordinate.CoordinateToIndex(byteCoordinate2, areaSize);
						availableBlockMap[byteCoordinate2.X, byteCoordinate2.Y] = 1;
						areaBlocks[num2] = new MapBlockData(areaId, num2, -1);
						areaBlocks[num2].SetToSizeBlock(mapBlockData);
						staticBlockLocationList.Add(byteCoordinate2);
					}
				}
			}
		}
		byte blockRange = GetBlockRange(mapBlockData);
		if (blockRange > 0)
		{
			int num3 = Math.Max(0, byteCoordinate.X - blockRange);
			int num4 = Math.Max(0, byteCoordinate.Y - blockRange);
			int num5 = Math.Min(areaSize - 1, byteCoordinate.X + mapBlockItem.Size + blockRange);
			int num6 = Math.Min(areaSize - 1, byteCoordinate.Y + mapBlockItem.Size + blockRange);
			List<short> list = ObjectPool<List<short>>.Instance.Get();
			List<short[]> result = ObjectPool<List<short[]>>.Instance.Get();
			list.Clear();
			for (byte b3 = (byte)num3; b3 < num5; b3++)
			{
				for (byte b4 = (byte)num4; b4 < num6; b4++)
				{
					ByteCoordinate byteCoordinate3 = new ByteCoordinate(b3, b4);
					if (byteCoordinate3.GetMinManhattanDistance(staticBlockLocationList) <= blockRange && availableBlockMap[b3, b4] != 1)
					{
						availableBlockMap[b3, b4] = 1;
						list.Add(ByteCoordinate.CoordinateToIndex(byteCoordinate3, areaSize));
					}
				}
			}
			result.Clear();
			RandomUtils.GenerateRandomWeightCellList(random, areaConfigData.DevelopedBlockCore, list.Count, ref result);
			for (int i = 0; i < list.Count; i++)
			{
				short num7 = list[i];
				areaBlocks[num7] = new MapBlockData(areaId, num7, result[i][0]);
				areaBlocks[num7].BelongBlockId = num;
			}
			if (hasStation)
			{
				if (list.Count <= 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					AdaptableLog.Info("The availableBlockMap is:");
					for (int j = 0; j < areaSize; j++)
					{
						stringBuilder.Clear();
						for (int k = 0; k < areaSize; k++)
						{
							if (k != 0)
							{
								stringBuilder.Append(" ");
							}
							StringBuilder stringBuilder2 = stringBuilder;
							StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
							handler.AppendFormatted(availableBlockMap[k, j]);
							stringBuilder2.Append(ref handler);
						}
						AdaptableLog.Info(stringBuilder.ToString());
					}
					AdaptableLog.Info("The BelongBlockId is:");
					for (int l = 0; l < areaSize; l++)
					{
						stringBuilder.Clear();
						for (int m = 0; m < areaSize; m++)
						{
							short num8 = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)m, (byte)l), areaSize);
							int num9 = ((areaBlocks[num8] != null) ? areaBlocks[num8].BelongBlockId : (-1));
							if (m != 0)
							{
								stringBuilder.Append(" ");
							}
							stringBuilder.Append((num9 >= 0) ? $"{num9:D3}" : "XXX");
						}
						AdaptableLog.Info(stringBuilder.ToString());
					}
					AdaptableLog.Info("The TemplateId is:");
					for (int n = 0; n < areaSize; n++)
					{
						stringBuilder.Clear();
						for (int num10 = 0; num10 < areaSize; num10++)
						{
							short num11 = ByteCoordinate.CoordinateToIndex(new ByteCoordinate((byte)num10, (byte)n), areaSize);
							int num12 = ((areaBlocks[num11] != null) ? areaBlocks[num11].TemplateId : (-1));
							if (num10 != 0)
							{
								stringBuilder.Append(" ");
							}
							stringBuilder.Append((num12 >= 0) ? $"{num12:D3}" : "XXX");
						}
						AdaptableLog.Info(stringBuilder.ToString());
					}
					throw new Exception($"rangeBlockList.Count must be > 0 when creating station | (templateId: {mapBlockItem.TemplateId}, areaSize: {areaSize}, coord: {byteCoordinate.X}, {byteCoordinate.Y})");
				}
				List<short> list2 = ObjectPool<List<short>>.Instance.Get();
				list2.Clear();
				list2.AddRange(list.Where(delegate(short bId)
				{
					ByteCoordinate byteCoordinate4 = ByteCoordinate.IndexToCoordinate(bId, areaSize);
					if (staticBlockLocationList.Contains(byteCoordinate4))
					{
						return false;
					}
					foreach (ByteCoordinate item in staticBlockLocationList)
					{
						if (item.GetManhattanDistance(byteCoordinate4) != 2)
						{
							return false;
						}
					}
					return true;
				}));
				MapAreaData mapAreaData = _areas[areaId];
				MapBlockData mapBlockData2 = null;
				mapBlockData2 = ((list2.Count <= 0) ? areaBlocks[list.OrderByDescending(delegate(short bId)
				{
					ByteCoordinate coord = ByteCoordinate.IndexToCoordinate(bId, areaSize);
					return (!staticBlockLocationList.Contains(coord)) ? staticBlockLocationList.Max((ByteCoordinate groupCoord) => groupCoord.GetManhattanDistance(coord)) : 0;
				}).First()] : areaBlocks[list2.GetRandom(random)]);
				if (areaId == 138 || createTaiwuVillage)
				{
					mapBlockData2.ChangeTemplateId(38, checkCanChange: false);
				}
				else
				{
					mapBlockData2.ChangeTemplateId(37, checkCanChange: false);
				}
				mapAreaData.StationBlockId = mapBlockData2.BlockId;
				ObjectPool<List<short>>.Instance.Return(list2);
			}
			ObjectPool<List<short>>.Instance.Return(list);
			ObjectPool<List<short[]>>.Instance.Return(result);
		}
		ObjectPool<List<ByteCoordinate>>.Instance.Return(staticBlockLocationList);
	}

	private void PlaceOtherBlocks(short areaId, MapAreaItem areaConfigData, byte areaSize, MapBlockData[] areaBlocks, byte[,] availableBlockMap, List<short> staticBlockIds, int settlementCount, IRandomSource random)
	{
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		List<short[]> result = ObjectPool<List<short[]>>.Instance.Get();
		List<short> list3 = ObjectPool<List<short>>.Instance.Get();
		List<short[]> result2 = ObjectPool<List<short[]>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < settlementCount; i++)
		{
			list.Add(areaBlocks[staticBlockIds[i]]);
		}
		list2.Clear();
		list3.Clear();
		for (byte b = 0; b < areaSize; b++)
		{
			for (byte b2 = 0; b2 < areaSize; b2++)
			{
				if (availableBlockMap[b, b2] == 0)
				{
					short num = ByteCoordinate.CoordinateToIndex(new ByteCoordinate(b, b2), areaSize);
					availableBlockMap[b, b2] = 1;
					if (InNormalBlockRange(num, list, areaSize))
					{
						list2.Add(num);
					}
					else
					{
						list3.Add(num);
					}
				}
			}
		}
		result.Clear();
		RandomUtils.GenerateRandomWeightCellList(random, areaConfigData.NormalBlockCore, list2.Count, ref result);
		for (int j = 0; j < list2.Count; j++)
		{
			short num2 = list2[j];
			areaBlocks[num2] = new MapBlockData(areaId, num2, result[j][0]);
		}
		result2.Clear();
		RandomUtils.GenerateRandomWeightCellList(random, areaConfigData.WildBlockCore, list3.Count, ref result2);
		for (int k = 0; k < list3.Count; k++)
		{
			short num3 = list3[k];
			areaBlocks[num3] = new MapBlockData(areaId, num3, result2[k][0]);
		}
		int l = 0;
		for (int num4 = areaBlocks.Length; l < num4; l++)
		{
			MapBlockData mapBlockData = areaBlocks[l];
			if (mapBlockData.TemplateId != 124)
			{
				continue;
			}
			int num5 = 1;
			ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(mapBlockData.BlockId, areaSize);
			int num6 = 1;
			bool flag = false;
			for (byte b3 = (byte)Math.Max(byteCoordinate.X - num5, 0); b3 < Math.Min(byteCoordinate.X + num6 + num5, areaSize); b3++)
			{
				for (byte b4 = (byte)Math.Max(byteCoordinate.Y - num5, 0); b4 < Math.Min(byteCoordinate.Y + num6 + num5, areaSize); b4++)
				{
					MapBlockData mapBlockData2 = areaBlocks[ByteCoordinate.CoordinateToIndex(new ByteCoordinate(b3, b4), areaSize)];
					if (mapBlockData2.BlockId != mapBlockData.BlockId && (mapBlockData2.TemplateId == 124 || mapBlockData2.TemplateId == 126))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				areaBlocks[l] = new MapBlockData(mapBlockData.AreaId, mapBlockData.BlockId, MapBlock.Instance.First((MapBlockItem mapBlockItem) => mapBlockItem.Type == EMapBlockType.Normal && mapBlockItem.Size == 1 && mapBlockItem.TemplateId != 124).TemplateId);
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
		ObjectPool<List<short[]>>.Instance.Return(result);
		ObjectPool<List<short>>.Instance.Return(list3);
		ObjectPool<List<short[]>>.Instance.Return(result2);
	}

	private bool InNormalBlockRange(short blockId, List<MapBlockData> staticBlocks, byte mapSize)
	{
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(blockId, mapSize);
		for (int i = 0; i < staticBlocks.Count; i++)
		{
			MapBlockData mapBlockData = staticBlocks[i];
			byte range = mapBlockData.GetConfig().Range;
			ByteCoordinate byteCoordinate2 = ByteCoordinate.IndexToCoordinate(mapBlockData.BlockId, mapSize);
			int manhattanDistance = MathUtils.GetManhattanDistance(byteCoordinate2.X - range - GlobalConfig.Instance.MapNormalBlockRange, byteCoordinate2.Y - range - GlobalConfig.Instance.MapNormalBlockRange, byteCoordinate.X, byteCoordinate.Y, mapBlockData.GetConfig().Size + (range + GlobalConfig.Instance.MapNormalBlockRange) * 2);
			if (manhattanDistance <= 0)
			{
				return true;
			}
		}
		return false;
	}

	private ByteCoordinate[] GetSniffRectList(ByteCoordinate byteCoordinate, int range, byte mapSize)
	{
		List<ByteCoordinate> list = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		list.Clear();
		byte b = (byte)Math.Max(byteCoordinate.X - range, 0);
		byte b2 = (byte)Math.Max(byteCoordinate.Y - range, 0);
		byte b3 = (byte)Math.Min(byteCoordinate.X + range, mapSize - 1);
		byte b4 = (byte)Math.Min(byteCoordinate.Y + range, mapSize - 1);
		for (byte b5 = b; b5 < b3; b5++)
		{
			list.Add(new ByteCoordinate(b5, b2));
			list.Add(new ByteCoordinate(b5, b4));
		}
		for (byte b6 = (byte)(b2 + 1); b6 < b4; b6++)
		{
			list.Add(new ByteCoordinate(b, b6));
			list.Add(new ByteCoordinate(b3, b6));
		}
		ByteCoordinate[] result = list.ToArray();
		ObjectPool<List<ByteCoordinate>>.Instance.Return(list);
		return result;
	}

	private void FixSeriesBlocks(MapAreaItem areaConfigData, byte mapSize, MapBlockData[] areaBlocks, Dictionary<int, List<short>> blockTypeDict, IRandomSource random)
	{
		if (areaConfigData.SeriesBlockCore == null)
		{
			return;
		}
		foreach (short[] item in areaConfigData.SeriesBlockCore)
		{
			int blockSubType = item[0];
			List<MapBlockData> list = areaBlocks.FindAll((MapBlockData block) => block.CanChangeBlockType() && block.BlockSubType == (EMapBlockSubType)blockSubType);
			int num = item[1];
			int infectCount = random.Next((int)item[2], (int)item[3]);
			for (int num2 = 0; num2 < num; num2++)
			{
				MapBlockData random2 = list.GetRandom(random);
				if (random2 == null)
				{
					break;
				}
				BlockInfect(random2, infectCount);
				list.Remove(random2);
				infectCount = random.Next((int)item[2], (int)item[3]) - 1;
			}
		}
		void BlockInfect(MapBlockData block, int num4)
		{
			int blockSubType2 = (int)block.BlockSubType;
			List<ByteCoordinate> list2 = ObjectPool<List<ByteCoordinate>>.Instance.Get();
			list2.Clear();
			int num3 = 1;
			while (list2.Count < num4)
			{
				list2.AddRange(GetSniffRectList(ByteCoordinate.IndexToCoordinate(block.BlockId, mapSize), num3++, mapSize));
				list2.RemoveAll(delegate(ByteCoordinate location)
				{
					short num7 = ByteCoordinate.CoordinateToIndex(location, mapSize);
					return !areaBlocks.CheckIndex(num7) || !areaBlocks[num7].CanChangeBlockType();
				});
			}
			for (int num5 = 0; num5 < num4; num5++)
			{
				int num6 = ByteCoordinate.CoordinateToIndex(list2[num5], mapSize);
				if (blockTypeDict.TryGetValue(blockSubType2, out var value))
				{
					areaBlocks[num6].ChangeTemplateId(value.GetRandom(random));
				}
			}
			ObjectPool<List<ByteCoordinate>>.Instance.Return(list2);
		}
	}

	private void FixEncircleBlocks(MapAreaItem areaConfigData, byte mapSize, MapBlockData[] areaBlocks, Dictionary<int, List<short>> blockTypeDict, IRandomSource random)
	{
		if (areaConfigData.EncircleBlockCore == null)
		{
			return;
		}
		List<ByteCoordinate> list = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		List<ByteCoordinate> list2 = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		List<MapBlockData> list3 = ObjectPool<List<MapBlockData>>.Instance.Get();
		for (int i = 0; i < areaConfigData.EncircleBlockCore.Count; i++)
		{
			short[] array = areaConfigData.EncircleBlockCore[i];
			if (!blockTypeDict.TryGetValue(array[0], out var targetTypeIdList))
			{
				continue;
			}
			targetTypeIdList.RemoveAll((short templateId) => MapBlock.Instance[templateId].Size != 1);
			if (targetTypeIdList.Count == 0)
			{
				continue;
			}
			short centerTemplateId = array[1];
			byte[,] areaShape = GetAreaShape(random, (byte)(array[4] * 2 + 1), (byte)array[3], ensureEdge: true);
			ByteCoordinate value = ((centerTemplateId == -1) ? new ByteCoordinate((byte)random.Next(mapSize / 4, mapSize * 3 / 4), (byte)random.Next(mapSize / 4, mapSize * 3 / 4)) : ByteCoordinate.IndexToCoordinate(areaBlocks.FindAll((MapBlockData block) => block.TemplateId == centerTemplateId).GetRandom(random).BlockId, mapSize));
			list.Clear();
			list2.Clear();
			list3.Clear();
			for (byte b = 0; b < areaShape.GetLength(0); b++)
			{
				for (byte b2 = 0; b2 < areaShape.GetLength(1); b2++)
				{
					byte b3 = areaShape[b, b2];
					ByteCoordinate item = new ByteCoordinate(b, b2);
					switch (b3)
					{
					case 1:
						list.Add(item);
						break;
					case 2:
						list2.Add(item);
						break;
					}
				}
			}
			if (list2.Count > 0)
			{
				int[] array2 = new int[2];
				int[] array3 = new int[2];
				for (int num = 0; num < list2.Count; num++)
				{
					ByteCoordinate byteCoordinate = list2[num];
					array2[0] = byteCoordinate.X - value.X;
					array2[1] = byteCoordinate.Y - value.Y;
					array3[0] = value.X + array2[0];
					array3[1] = value.Y + array2[1];
					if (array3[0] >= 0 && array3[0] < mapSize && array3[1] >= 0 && array3[1] < mapSize)
					{
						ByteCoordinate byteCoordinate2 = new ByteCoordinate((byte)array3[0], (byte)array3[1]);
						short num2 = ByteCoordinate.CoordinateToIndex(byteCoordinate2, mapSize);
						MapBlockData mapBlockData = null;
						if (areaBlocks.CheckIndex(num2))
						{
							mapBlockData = areaBlocks[num2];
						}
						if (mapBlockData != null && mapBlockData.CanChangeBlockType())
						{
							list3.Add(mapBlockData);
						}
					}
				}
			}
			else
			{
				Logger.Warn($"{areaConfigData.Name}:encircleType {array[0]} has no edgePosList");
			}
			if (list3.Count > 0)
			{
				for (int num3 = 0; num3 < array[2]; num3++)
				{
					if (list3.Count <= 0)
					{
						break;
					}
					list3.RemoveAt(random.Next(list3.Count));
				}
				list3.ForEach(delegate(MapBlockData block)
				{
					block.ChangeTemplateId(targetTypeIdList.GetRandom(random));
				});
			}
			else
			{
				Logger.Warn($"Error:areaId {areaConfigData.TemplateId} failed to set encircle data of type {array[0]} circle center is {value}");
			}
		}
		List<List<ByteCoordinate>> list4 = ObjectPool<List<List<ByteCoordinate>>>.Instance.Get();
		List<ByteCoordinate> list5 = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		list4.Clear();
		list5.Clear();
		for (short num4 = 0; num4 < areaBlocks.Length; num4++)
		{
			if (areaBlocks[num4].IsPassable())
			{
				list5.Add(ByteCoordinate.IndexToCoordinate(num4, mapSize));
			}
		}
		while (list5.Count > 0)
		{
			List<ByteCoordinate> island = ObjectPool<List<ByteCoordinate>>.Instance.Get();
			island.Clear();
			list4.Add(island);
			SpreadIsland(list5[0], island, mapSize, CanPass, new byte[mapSize, mapSize]);
			list5.RemoveAll((ByteCoordinate pos) => island.Contains(pos));
		}
		if (list4.Count > 1)
		{
			List<short> list6 = ObjectPool<List<short>>.Instance.Get();
			list6.Clear();
			for (int num5 = 0; num5 < areaConfigData.WildBlockCore.Count; num5++)
			{
				short num6 = areaConfigData.WildBlockCore[num5][0];
				if (num6 != 124)
				{
					list6.Add(num6);
				}
			}
			list4.Sort((List<ByteCoordinate> left, List<ByteCoordinate> right) => (left.Count != right.Count) ? (right.Count - left.Count) : (left.GetHashCode() - right.GetHashCode()));
			List<ByteCoordinate> list7 = list4[0];
			for (int num7 = 1; num7 < list4.Count; num7++)
			{
				List<ByteCoordinate> list8 = list4[num7];
				ByteCoordinate byteCoordinate3 = list7[random.Next(list7.Count)];
				MapBlockData mapBlockData2 = null;
				int num8 = int.MaxValue;
				foreach (ByteCoordinate item2 in list8)
				{
					int manhattanDistance = item2.GetManhattanDistance(byteCoordinate3);
					if (manhattanDistance < num8)
					{
						mapBlockData2 = areaBlocks[ByteCoordinate.CoordinateToIndex(item2, mapSize)];
						num8 = manhattanDistance;
					}
				}
				ByteCoordinate byteCoordinate4 = ByteCoordinate.IndexToCoordinate(mapBlockData2.BlockId, mapSize);
				MapBlockData mapBlockData3 = null;
				num8 = int.MaxValue;
				foreach (ByteCoordinate item3 in list7)
				{
					int manhattanDistance2 = item3.GetManhattanDistance(byteCoordinate4);
					if (manhattanDistance2 < num8)
					{
						mapBlockData3 = areaBlocks[ByteCoordinate.CoordinateToIndex(item3, mapSize)];
						num8 = manhattanDistance2;
					}
				}
				ByteCoordinate byteCoordinate5 = ByteCoordinate.IndexToCoordinate(mapBlockData3.BlockId, mapSize);
				int num9 = Math.Sign(byteCoordinate5.X - byteCoordinate4.X);
				int num10 = Math.Sign(byteCoordinate5.Y - byteCoordinate4.Y);
				while (byteCoordinate4 != byteCoordinate5)
				{
					if (byteCoordinate4.X != byteCoordinate5.X && (byteCoordinate4.Y == byteCoordinate5.Y || random.CheckPercentProb(50)))
					{
						byteCoordinate4.X = (byte)(byteCoordinate4.X + num9);
					}
					else
					{
						byteCoordinate4.Y = (byte)(byteCoordinate4.Y + num10);
					}
					short num11 = ByteCoordinate.CoordinateToIndex(byteCoordinate4, mapSize);
					if (!areaBlocks[num11].IsPassable())
					{
						areaBlocks[num11].ChangeTemplateId(list6.GetRandom(random), checkCanChange: false);
					}
				}
				if (mapBlockData3.TemplateId == 124)
				{
					mapBlockData3.ChangeTemplateId(list6.GetRandom(random), checkCanChange: false);
				}
			}
			ObjectPool<List<short>>.Instance.Return(list6);
		}
		ObjectPool<List<ByteCoordinate>>.Instance.Return(list);
		ObjectPool<List<ByteCoordinate>>.Instance.Return(list2);
		ObjectPool<List<MapBlockData>>.Instance.Return(list3);
		list4.ForEach(delegate(List<ByteCoordinate> item2)
		{
			ObjectPool<List<ByteCoordinate>>.Instance.Return(item2);
		});
		ObjectPool<List<List<ByteCoordinate>>>.Instance.Return(list4);
		ObjectPool<List<ByteCoordinate>>.Instance.Return(list5);
		bool CanPass(ByteCoordinate pos)
		{
			return areaBlocks[ByteCoordinate.CoordinateToIndex(pos, mapSize)].IsPassable();
		}
	}

	private void SpreadIsland(ByteCoordinate pos, List<ByteCoordinate> island, byte mapSize, Predicate<ByteCoordinate> canPass, byte[,] reachMap)
	{
		if (reachMap[pos.X, pos.Y] == 1)
		{
			return;
		}
		reachMap[pos.X, pos.Y] = 1;
		if (canPass(pos))
		{
			island.Add(pos);
			if (pos.X > 0)
			{
				SpreadIsland(new ByteCoordinate((byte)(pos.X - 1), pos.Y), island, mapSize, canPass, reachMap);
			}
			if (pos.X < mapSize - 1)
			{
				SpreadIsland(new ByteCoordinate((byte)(pos.X + 1), pos.Y), island, mapSize, canPass, reachMap);
			}
			if (pos.Y > 0)
			{
				SpreadIsland(new ByteCoordinate(pos.X, (byte)(pos.Y - 1)), island, mapSize, canPass, reachMap);
			}
			if (pos.Y < mapSize - 1)
			{
				SpreadIsland(new ByteCoordinate(pos.X, (byte)(pos.Y + 1)), island, mapSize, canPass, reachMap);
			}
		}
	}

	private void InitAreaTravelRoute(DataContext context)
	{
		ClearTravelRouteDict(context);
		for (short num = 0; num < 135; num++)
		{
			MapAreaData mapAreaData = _areas[num];
			MapAreaItem config = mapAreaData.GetConfig();
			AreaTravelRoute[] neighborAreas = config.NeighborAreas;
			AreaTravelRoute[] array = neighborAreas;
			for (int i = 0; i < array.Length; i++)
			{
				AreaTravelRoute areaTravelRoute = array[i];
				short areaIdByAreaTemplateId = GetAreaIdByAreaTemplateId(areaTravelRoute.DestAreaId);
				TravelRouteKey elementId = new TravelRouteKey(num, areaIdByAreaTemplateId);
				TravelRoute travelRoute = new TravelRoute();
				mapAreaData.NeighborAreas.Add(areaIdByAreaTemplateId);
				_areas[areaIdByAreaTemplateId].NeighborAreas.Add(num);
				travelRoute.PosList.AddRange(areaTravelRoute.MapPosList);
				travelRoute.AreaList.Add(num);
				travelRoute.AreaList.Add(areaIdByAreaTemplateId);
				travelRoute.CostList.Add(areaTravelRoute.CostDays);
				if (num > areaIdByAreaTemplateId)
				{
					elementId.Reverse();
					travelRoute.PosList.Reverse();
					travelRoute.AreaList.Reverse();
				}
				AddElement_TravelRouteDict(elementId, travelRoute, context);
			}
		}
		AStarMapForTravel aStarMapForTravel = new AStarMapForTravel();
		List<short> path = ObjectPool<List<short>>.Instance.Get();
		aStarMapForTravel.InitMap(GetTravelCost);
		for (short num2 = 0; num2 < 134; num2++)
		{
			for (short num3 = (short)(num2 + 1); num3 < 135; num3++)
			{
				TravelRouteKey travelRouteKey = new TravelRouteKey(num2, num3);
				if (!_travelRouteDict.ContainsKey(travelRouteKey))
				{
					TravelRoute travelRoute2 = new TravelRoute();
					path.Clear();
					aStarMapForTravel.FindWay(num2, num3, ref path);
					travelRoute2.AreaList.AddRange(path);
					for (int j = 0; j < path.Count - 1; j++)
					{
						short num4 = path[j];
						short num5 = path[j + 1];
						bool flag = num4 > num5;
						TravelRouteKey key = new TravelRouteKey(num4, num5);
						if (flag)
						{
							key.Reverse();
						}
						TravelRoute travelRoute3 = _travelRouteDict[key];
						if (!flag)
						{
							travelRoute2.PosList.AddRange(travelRoute3.PosList);
							for (int k = 0; k < travelRoute3.CostList.Count; k++)
							{
								travelRoute2.CostList.Add(travelRoute3.CostList[k]);
							}
						}
						else
						{
							for (int num6 = travelRoute3.PosList.Count - 1; num6 >= 0; num6--)
							{
								travelRoute2.PosList.Add(travelRoute3.PosList[num6]);
							}
							for (int num7 = travelRoute3.CostList.Count - 1; num7 >= 0; num7--)
							{
								travelRoute2.CostList.Add(travelRoute3.CostList[num7]);
							}
						}
						if (j < path.Count - 2)
						{
							sbyte[] worldMapPos = _areas[num5].GetConfig().WorldMapPos;
							travelRoute2.PosList.Add(new ByteCoordinate((byte)worldMapPos[0], (byte)worldMapPos[1]));
						}
					}
					AddElement_TravelRouteDict(travelRouteKey, travelRoute2, context);
				}
			}
		}
		List<short> areaIdInBornState = ObjectPool<List<short>>.Instance.Get();
		short num8 = (short)((DomainManager.World.GetTaiwuVillageStateTemplateId() - 1) * 3);
		short num9 = (short)(45 + (DomainManager.World.GetTaiwuVillageStateTemplateId() - 1) * 6);
		areaIdInBornState.Clear();
		for (int l = 0; l < 3; l++)
		{
			areaIdInBornState.Add((short)(num8 + l));
		}
		for (int m = 0; m < 6; m++)
		{
			areaIdInBornState.Add((short)(num9 + m));
		}
		aStarMapForTravel.InitMap(GetTravelCostInState);
		for (int n = 0; n < areaIdInBornState.Count - 1; n++)
		{
			short num10 = areaIdInBornState[n];
			for (int num11 = n + 1; num11 < areaIdInBornState.Count; num11++)
			{
				short num12 = areaIdInBornState[num11];
				TravelRouteKey travelRouteKey2 = new TravelRouteKey(num10, num12);
				if (_bornStateTravelRouteDict.ContainsKey(travelRouteKey2))
				{
					continue;
				}
				TravelRoute travelRoute4 = new TravelRoute();
				path.Clear();
				aStarMapForTravel.FindWay(num10, num12, ref path);
				travelRoute4.AreaList.AddRange(path);
				for (int num13 = 0; num13 < path.Count - 1; num13++)
				{
					short num14 = path[num13];
					short num15 = path[num13 + 1];
					bool flag2 = num14 > num15;
					TravelRouteKey key2 = new TravelRouteKey(num14, num15);
					if (flag2)
					{
						key2.Reverse();
					}
					TravelRoute travelRoute5 = _travelRouteDict[key2];
					if (!flag2)
					{
						travelRoute4.PosList.AddRange(travelRoute5.PosList);
						for (int num16 = 0; num16 < travelRoute5.CostList.Count; num16++)
						{
							travelRoute4.CostList.Add(travelRoute5.CostList[num16]);
						}
					}
					else
					{
						for (int num17 = travelRoute5.PosList.Count - 1; num17 >= 0; num17--)
						{
							travelRoute4.PosList.Add(travelRoute5.PosList[num17]);
						}
						for (int num18 = travelRoute5.CostList.Count - 1; num18 >= 0; num18--)
						{
							travelRoute4.CostList.Add(travelRoute5.CostList[num18]);
						}
					}
					if (num13 < path.Count - 2)
					{
						sbyte[] worldMapPos2 = _areas[num15].GetConfig().WorldMapPos;
						travelRoute4.PosList.Add(new ByteCoordinate((byte)worldMapPos2[0], (byte)worldMapPos2[1]));
					}
				}
				AddElement_BornStateTravelRouteDict(travelRouteKey2, travelRoute4, context);
			}
		}
		for (short num19 = 0; num19 < 134; num19++)
		{
			TravelRouteKey elementId2 = new TravelRouteKey(num19, 135);
			TravelRoute travelRoute6 = new TravelRoute();
			travelRoute6.AreaList.Add(num19);
			travelRoute6.AreaList.Add(135);
			travelRoute6.CostList.Add(10);
			AddElement_TravelRouteDict(elementId2, travelRoute6, context);
		}
		ObjectPool<List<short>>.Instance.Return(areaIdInBornState);
		ObjectPool<List<short>>.Instance.Return(path);
		List<short> list = new List<short>();
		for (short num20 = 0; num20 < 45; num20++)
		{
			list.Add(num20);
		}
		short areaId;
		for (areaId = 0; areaId < 45; areaId++)
		{
			GameData.Utilities.ShortList value = GameData.Utilities.ShortList.Create();
			value.Items.AddRange(list);
			value.Items.Remove(areaId);
			value.Items.Sort(delegate(short area1, short area2)
			{
				TravelRouteKey key3 = new TravelRouteKey(areaId, area1);
				TravelRouteKey key4 = new TravelRouteKey(areaId, area2);
				if (areaId > area1)
				{
					key3.Reverse();
				}
				if (areaId > area2)
				{
					key4.Reverse();
				}
				return _travelRouteDict[key3].GetTotalTimeCost().CompareTo(_travelRouteDict[key4].GetTotalTimeCost());
			});
			AddElement_RegularAreaNearList(areaId, value, context);
		}
		short GetTravelCost(short fromArea, short toArea)
		{
			bool flag3 = fromArea > toArea;
			TravelRouteKey key3 = new TravelRouteKey(fromArea, toArea);
			if (flag3)
			{
				key3.Reverse();
			}
			return _travelRouteDict[key3].GetTotalTimeCost();
		}
		short GetTravelCostInState(short fromArea, short toArea)
		{
			if (!areaIdInBornState.Contains(fromArea) || !areaIdInBornState.Contains(toArea))
			{
				return 10000;
			}
			bool flag3 = fromArea > toArea;
			TravelRouteKey key3 = new TravelRouteKey(fromArea, toArea);
			if (flag3)
			{
				key3.Reverse();
			}
			return _travelRouteDict[key3].GetTotalTimeCost();
		}
	}

	public override void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		DomainManager.Map.SetCrossArchiveLockMoveTime(value: true, context);
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.TaiwuCrossArchiveSpecialEffect);
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		List<MapBlockData> mapBlockList = ObjectPool<List<MapBlockData>>.Instance.Get();
		DomainManager.Map.GetLocationByDistance(taiwuVillageLocation, 15, 15, ref mapBlockList);
		if (mapBlockList.Count > 0)
		{
			MapBlockData random = mapBlockList.GetRandom(context.Random);
			DomainManager.Map.SetTeleportMove(teleport: true);
			DomainManager.Map.Move(context, random.BlockId, notCostTime: true);
			DomainManager.Map.SetTeleportMove(teleport: false);
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(mapBlockList);
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		GetInSightBlocks(list);
		HideAllBlocks(context, DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId, list);
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		MapBlockData block = DomainManager.Map.GetBlock(taiwuVillageLocation);
		block.SetVisible(visible: true, context);
	}

	[DomainMethod]
	public void RetrieveDreamBackLocation(DataContext context, Location location)
	{
		if (DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().GetBool("ConchShip_PresetKey_FuyuHiltGuiding") || DomainManager.Taiwu.GetTaiwu().GetLocation() != location)
		{
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.SetEventLockInputState, arg1: false);
			return;
		}
		foreach (EDreamBackLocationType item in RemoveDreamBackLocation(context, location))
		{
			TaiwuEventDomain taiwuEvent = DomainManager.TaiwuEvent;
			if (1 == 0)
			{
			}
			sbyte arg = item switch
			{
				EDreamBackLocationType.Inventory => 1, 
				EDreamBackLocationType.LifeSkill => 3, 
				EDreamBackLocationType.CombatSkill => 2, 
				_ => -1, 
			};
			if (1 == 0)
			{
			}
			taiwuEvent.OnEvent_TaiwuCrossArchiveFindMemory(arg);
		}
	}

	public void CreateDreamBackLocation(DataContext context, Location location, EDreamBackLocationType locationType)
	{
		List<DreamBackLocationData> dreamBackLocationData = DomainManager.Extra.GetDreamBackLocationData();
		DreamBackLocationData item = DreamBackLocationData.Create(location, locationType);
		dreamBackLocationData.Add(item);
		DomainManager.Extra.SetDreamBackLocationData(dreamBackLocationData, context);
	}

	public IEnumerable<EDreamBackLocationType> RemoveDreamBackLocation(DataContext context, Location location)
	{
		List<DreamBackLocationData> dreamBackLocationData = DomainManager.Extra.GetDreamBackLocationData();
		bool anyRemoved = false;
		for (int i = dreamBackLocationData.Count - 1; i >= 0; i--)
		{
			DreamBackLocationData locationData = dreamBackLocationData[i];
			if (!(locationData.Location != location))
			{
				CollectionUtils.SwapAndRemove(dreamBackLocationData, i);
				anyRemoved = true;
				yield return locationData.Type;
			}
		}
		if (anyRemoved)
		{
			DomainManager.Extra.SetDreamBackLocationData(dreamBackLocationData, context);
		}
	}

	[DomainMethod]
	public void GmCmd_SetLockTime(bool isLock)
	{
		_lockMoveTime = isLock;
	}

	[DomainMethod]
	public void GmCmd_SetTeleportMove(bool teleportOn)
	{
		_teleportMove = teleportOn;
	}

	[DomainMethod]
	public void GmCmd_ShowAllMapBlock(DataContext context)
	{
		for (short num = 0; num < 139; num++)
		{
			Span<MapBlockData> areaBlocks = GetAreaBlocks(num);
			for (int i = 0; i < areaBlocks.Length; i++)
			{
				MapBlockData mapBlockData = areaBlocks[i];
				if (mapBlockData.TemplateId != 126)
				{
					mapBlockData.SetVisible(visible: true, context);
				}
			}
			_areas[num].Discovered = true;
			SetElement_Areas(num, _areas[num], context);
		}
		DomainManager.Merchant.RefreshCaravanInTaiwuState(context);
	}

	[DomainMethod]
	public void GmCmd_HideAllMapBlock(DataContext context)
	{
		DomainManager.Map.HideAllBlocks(context);
		DomainManager.Merchant.RefreshCaravanInTaiwuState(context);
	}

	[DomainMethod]
	public void GmCmd_UnlockAllStation(DataContext context)
	{
		for (short num = 0; num < 135; num++)
		{
			if (!_areas[num].StationUnlocked)
			{
				MapBlockData block = GetBlock(num, _areas[num].StationBlockId);
				UnlockStation(context, num, costAuthority: false);
				if (!block.Visible)
				{
					block.SetVisible(visible: true, context);
				}
			}
		}
	}

	[DomainMethod]
	public void GmCmd_ChangeSpiritualDebt(DataContext context, short areaId, int spiritualDebt)
	{
		DomainManager.Extra.SetAreaSpiritualDebt(context, areaId, spiritualDebt);
	}

	[DomainMethod]
	public void GmCmd_ChangeAllSpiritualDebt(DataContext context, int spiritualDebt)
	{
		for (short num = 0; num < DomainManager.Map._areas.Length; num++)
		{
			DomainManager.Extra.SetAreaSpiritualDebt(context, num, spiritualDebt);
		}
	}

	[DomainMethod]
	public void GmCmd_SetMapBlockData(DataContext context, MapBlockData mapBlockData)
	{
		SetBlockData(context, mapBlockData);
	}

	[DomainMethod]
	public void GmCmd_CreateFixedCharacterAtCurrentBlock(DataContext context, short templateId)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (Config.Character.Instance[templateId].CreatingType == 0 && DomainManager.Character.TryGetFixedCharacterByTemplateId(templateId, out var character))
		{
			EventHelper.MoveFixedCharacter(character, taiwu.GetLocation());
			return;
		}
		character = DomainManager.Character.CreateFixedCharacter(context, templateId);
		if (character != null && taiwu != null)
		{
			Location location = taiwu.GetLocation();
			DomainManager.Character.CompleteCreatingCharacter(character.GetId());
			EventHelper.MoveFixedCharacter(character, location);
		}
	}

	[DomainMethod]
	public void GmCmd_AddAnimal(DataContext context, short templateId)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		DomainManager.Extra.CreateAnimalByCharacterTemplateId(context, templateId, location);
	}

	[DomainMethod]
	public void GmCmd_AddRandomEnemyOnMap(DataContext context, short templateId)
	{
		byte creatingType = Config.Character.Instance[templateId].CreatingType;
		bool condition = (uint)(creatingType - 2) <= 1u;
		Tester.Assert(condition);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		MapBlockData block = DomainManager.Map.GetBlock(taiwu.GetLocation());
		block.AddTemplateEnemy(new MapTemplateEnemyInfo(templateId, block.BlockId, -1));
		DomainManager.Map.SetBlockData(context, block);
	}

	[DomainMethod]
	public void GmCmd_TurnMapBlockIntoAshes(DataContext context)
	{
		short areaId = DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId;
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			if (!mapBlockData.IsCityTown() && (mapBlockData.IsNonDeveloped() || mapBlockData.GetConfig().Type == EMapBlockType.Developed))
			{
				DomainManager.Map.ChangeBlockTemplate(context, mapBlockData, (short)(context.Random.NextBool() ? 118 : 124));
			}
		}
	}

	[DomainMethod]
	public void GMCmd_ThrowBackend()
	{
		throw new Exception("a backend test exception");
	}

	[DomainMethod]
	public bool GmCmd_TriggerTravelingEvent(DataContext context, short templateId)
	{
		TravelingEventItem travelingEventItem = TravelingEvent.Instance[templateId];
		if (string.IsNullOrEmpty(travelingEventItem.Event))
		{
			return false;
		}
		TravelingEventCollection travelingEventCollection = DomainManager.Extra.GetTravelingEventCollection();
		short areaId = DomainManager.Taiwu.GetTaiwu().GetValidLocation().AreaId;
		int num = AddTravelingEvent(context, templateId, areaId);
		if (num < 0)
		{
			return false;
		}
		GameData.Domains.TaiwuEvent.TaiwuEvent taiwuEvent = DomainManager.TaiwuEvent.GetEvent(travelingEventItem.Event);
		if (taiwuEvent != null)
		{
			GameData.Domains.TaiwuEvent.TaiwuEvent taiwuEvent2 = taiwuEvent;
			if (taiwuEvent2.ArgBox == null)
			{
				EventArgBox eventArgBox = (taiwuEvent2.ArgBox = DomainManager.TaiwuEvent.GetEventArgBox());
			}
			travelingEventCollection.FillEventArgBox(num, taiwuEvent.ArgBox);
			if (!taiwuEvent.EventConfig.CheckCondition())
			{
				int recordSize = travelingEventCollection.GetRecordSize(num);
				travelingEventCollection.Remove(num, recordSize);
				Logger.Warn($"Traveling event {templateId} - {travelingEventItem.Name} is triggering {taiwuEvent.EventGuid} when OnCheckEventCondition return false.");
				return false;
			}
			DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
			DomainManager.TaiwuEvent.TravelingEventCheckComplete();
			DomainManager.Map.SetOnHandlingTravelingEventBlock(value: true, context);
		}
		else
		{
			Logger.Warn($"Monthly Event {templateId} - {travelingEventItem.Name} ({travelingEventItem.Event}) not found.");
		}
		return false;
	}

	[DomainMethod]
	public int GmCmd_GetTreasuryValueByTaiwuLocation()
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return 0;
		}
		return DomainManager.Organization.GetSettlementByLocation(location)?.Treasuries.CurrentTotalValue ?? 0;
	}

	public void SetMapPickupUsed(DataContext context, MapPickup pickup)
	{
		if (pickup == null)
		{
			throw new ArgumentNullException("pickup");
		}
		if (!DomainManager.Extra.TryGetElement_PickupDict(pickup.Location, out var value))
		{
			throw new ArgumentException("SetPickupUsed: Trying to set a pickup not in pickup collection");
		}
		if (value != null)
		{
			SetMapPickupUsedInternal(context, value, pickup);
		}
	}

	private void SetMapPickupUsedInternal(DataContext context, MapPickupCollection pickupCollection, MapPickup pickup)
	{
		pickup.SetAsUsed();
		DomainManager.Extra.SetMapPickupCollection(context, pickup.Location, pickupCollection);
	}

	public IEnumerable<MapPickup> GetMapPickups(Location location)
	{
		if (!DomainManager.Extra.TryGetElement_PickupDict(location, out var value))
		{
			return Enumerable.Empty<MapPickup>();
		}
		sbyte xiangshuProgress = DomainManager.World.GetXiangshuProgress();
		sbyte xiangshuLevel = GameData.Domains.World.SharedMethods.GetXiangshuLevel(xiangshuProgress);
		return value.IterVisiblePickups(xiangshuLevel);
	}

	public MapPickup FindFirstVisibleMapPickupOnLocation(Location location)
	{
		List<MapPickup> list = GetMapPickups(location).ToList();
		list.Sort(MapPickupHelper.CompareVisiblePickups);
		if (list.Count == 0)
		{
			return null;
		}
		return list[0];
	}

	public void TriggerNormalMapPickup(DataContext context, MapPickup pickup)
	{
		if (pickup == null || !DomainManager.Extra.TryGetElement_PickupDict(pickup.Location, out var value) || value == null)
		{
			return;
		}
		if (pickup.IsEventType)
		{
			throw new ArgumentException("TriggerNormalMapPickup: Trying to trigger a pickup that is an event");
		}
		SetMapPickupUsedInternal(context, value, pickup);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int num = 0;
		ItemKey[] equipment = taiwu.GetEquipment();
		foreach (ItemKey itemKey in equipment)
		{
			if (DomainManager.Item.TryGetBaseItem(itemKey) is IExploreBonusRateItem exploreBonusRateItem)
			{
				num += exploreBonusRateItem.GetExploreBonusRate();
			}
		}
		bool flag = true;
		bool flag2 = context.Random.CheckPercentProb(num);
		if (flag2 && pickup.Type == MapPickup.EMapPickupType.Item)
		{
			IItemConfig config = ItemConfigHelper.GetConfig(pickup.ItemType, pickup.ItemTemplateId);
			if (config.ItemSubType != 505)
			{
				flag = false;
				IItemConfig itemConfig = config.Upgrade();
				if (itemConfig == null)
				{
					flag2 = false;
				}
				else
				{
					pickup = new MapPickup(pickup)
					{
						ItemTemplateId = itemConfig.TemplateId
					};
				}
			}
		}
		if (ApplyPickups.TryGetValue(pickup.Type, out var value2))
		{
			value2(context, pickup);
			if (flag2 && flag)
			{
				value2(context, pickup);
			}
		}
		MapElementPickupDisplayData pickupDisplayData = GetPickupDisplayData(pickup);
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.PlayMapPickupEffect, pickupDisplayData);
		if (flag2 && pickup.Template.ExtraBonusReplaceInstantNotification >= 0)
		{
			AddNotification(pickup, pickup.Template.ExtraBonusReplaceInstantNotification);
			return;
		}
		if (pickup.Template.InstantNotification >= 0)
		{
			AddNotification(pickup, pickup.Template.InstantNotification);
		}
		if (flag2 && pickup.Template.ExtraBonusAddInstantNotification >= 0)
		{
			AddNotification(pickup, pickup.Template.ExtraBonusAddInstantNotification);
		}
	}

	private void AddNotification(MapPickup pickup, short notificationId)
	{
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		switch (notificationId)
		{
		case 179:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Resource);
			instantNotificationCollection.AddMapPickupsResource(pickup.Location, pickup.ResourceType, pickup.ResourceCount);
			break;
		case 244:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Resource);
			instantNotificationCollection.AddMapPickupsResourceUpdate(pickup.ResourceType, pickup.ResourceCount);
			break;
		case 180:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsFoodIngredients(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 181:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsMaterials(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 182:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsHerbal0(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 183:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsHerbal1(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 188:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsFruit(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 189:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsChickenDishes(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 190:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsMeatDishes(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 191:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsVegetarianDishes(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 192:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsSeafoodDishes(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 193:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsWine(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 194:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsTea(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 195:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsTool(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 196:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsAccessory(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 197:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsPoisonCream(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 198:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsHarrier(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 199:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsToken(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 200:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsNeedleBox(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 201:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsThorn(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 202:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsHiddenWeapon(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 203:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsFlute(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 204:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsGloves(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 205:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsFurGloves(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 206:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsPestle(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 207:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsSword(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 208:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsBlade(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 209:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsPolearm(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 210:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupQin(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 211:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsWhisk(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 212:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsWhip(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 213:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsCrest(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 214:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsShoes(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 215:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsArmor(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 216:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsArmGuard(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 217:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsCarDrop(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 184:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsPoisonCorrected(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 185:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsInjuryMedicineCorrected(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 186:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsAntidoteCorrected(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 187:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsGainMedicineCorrected(pickup.Location, pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 247:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsItemUpdate(pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 254:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.Item);
			instantNotificationCollection.AddMapPickupsMedicineUpdate(pickup.ItemType, pickup.ItemTemplateId);
			break;
		case 218:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.ExpBonus);
			instantNotificationCollection.AddMapPickupsExp(pickup.Location, pickup.ExpCount);
			break;
		case 245:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.ExpBonus);
			instantNotificationCollection.AddMapPickupsExpUpdate(pickup.ExpCount);
			break;
		case 219:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.ReadEffect);
			instantNotificationCollection.AddMapPickupsReading();
			break;
		case 248:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.ReadEffect);
			instantNotificationCollection.AddMapPickupsReadingUpdate();
			break;
		case 220:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.LoopEffect);
			instantNotificationCollection.AddMapPickupsQiArt();
			break;
		case 249:
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.LoopEffect);
			instantNotificationCollection.AddMapPickupsQiArtUpdate();
			break;
		case 221:
		{
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.DebtBonus);
			sbyte stateTemplateIdByAreaId2 = GetStateTemplateIdByAreaId(pickup.Location.AreaId);
			instantNotificationCollection.AddMapPickupsMorale(stateTemplateIdByAreaId2);
			break;
		}
		case 246:
		{
			Tester.Assert(pickup.Type == MapPickup.EMapPickupType.DebtBonus);
			sbyte stateTemplateIdByAreaId = GetStateTemplateIdByAreaId(pickup.Location.AreaId);
			instantNotificationCollection.AddMapPickupsMoraleUpdate(stateTemplateIdByAreaId);
			break;
		}
		case 222:
		case 223:
		case 224:
		case 225:
		case 226:
		case 227:
		case 228:
		case 229:
		case 230:
		case 231:
		case 232:
		case 233:
		case 234:
		case 235:
		case 236:
		case 237:
		case 238:
		case 239:
		case 240:
		case 241:
		case 242:
		case 243:
		case 250:
		case 251:
		case 252:
		case 253:
			break;
		}
	}

	private static void AddResourceByPickup(DataContext context, MapPickup pickup)
	{
		DomainManager.Taiwu.AddResource(context, ItemSourceType.Inventory, pickup.ResourceType, pickup.ResourceCount);
	}

	private static void AddItemByPickup(DataContext context, MapPickup pickup)
	{
		ItemKey itemKey = DomainManager.Item.CreateItem(context, pickup.ItemType, pickup.ItemTemplateId);
		AddItem(context, itemKey, 1);
	}

	private static void LoopOnceByPickup(DataContext context, MapPickup pickup)
	{
		DomainManager.Taiwu.ApplyNeigongLoopingImprovementOnce(context);
		DomainManager.Taiwu.TryAddLoopingEvent(context, GlobalConfig.Instance.BaseLoopingEventProbability);
	}

	private static void ReadOnceByPickup(DataContext context, MapPickup pickup)
	{
		if (DomainManager.Taiwu.UpdateReadingProgressOnce(context, isInCombat: false, addInstantNotification: false))
		{
			ItemKey curReadingBook = DomainManager.Taiwu.GetCurReadingBook();
			DomainManager.Extra.AddReadingEventBookId(context, curReadingBook.Id);
		}
	}

	private static void AddExpByPickup(DataContext context, MapPickup pickup)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		taiwu.ChangeExp(context, pickup.ExpCount);
	}

	private static void AddDebtByPickup(DataContext context, MapPickup pickup)
	{
		sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(pickup.Location.AreaId);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		DomainManager.Map.GetAllAreaInState(stateIdByAreaId, list);
		foreach (short item in list)
		{
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, item, pickup.DebtCount, getProfessionSeniority: true, addInstantNotification: false);
		}
	}

	public void SetPickupAtFirst(DataContext context, MapPickup pickup)
	{
		if (pickup == null)
		{
			throw new ArgumentNullException("pickup");
		}
		if (!DomainManager.Extra.TryGetElement_PickupDict(pickup.Location, out var value))
		{
			throw new ArgumentException("IgnorePickup: Trying Ignore a pickup not in pickup collection");
		}
		value.SetPickupAtFirst(pickup);
		DomainManager.Extra.SetMapPickupCollection(context, pickup.Location, value);
	}

	public void IgnoreOneMapPickup(DataContext context, MapPickup pickup)
	{
		if (pickup == null)
		{
			throw new ArgumentNullException("pickup");
		}
		if (!DomainManager.Extra.TryGetElement_PickupDict(pickup.Location, out var value))
		{
			throw new ArgumentException("IgnorePickup: Trying Ignore a pickup not in pickup collection");
		}
		value.IgnorePickup(pickup);
		DomainManager.Extra.SetMapPickupCollection(context, pickup.Location, value);
	}

	public static (bool, PresetItemWithCount) CheckMapPickupConfigItemRewards(MapPickupsItem pickupConfig, int index)
	{
		List<PresetItemWithCount> eventSecondItemRewards = pickupConfig.EventSecondItemRewards;
		if (index < 0 || eventSecondItemRewards == null || index > eventSecondItemRewards.Count - 1)
		{
			return (false, null);
		}
		PresetItemWithCount presetItemWithCount = eventSecondItemRewards[index];
		if (!presetItemWithCount.IsValid)
		{
			return (false, null);
		}
		return (true, presetItemWithCount);
	}

	public static (bool, ResourceInfo?) CheckMapPickupConfigResourceRewards(MapPickupsItem pickupConfig, int index)
	{
		List<ResourceInfo> eventSecondResourceRewards = pickupConfig.EventSecondResourceRewards;
		if (index < 0 || eventSecondResourceRewards == null || index > eventSecondResourceRewards.Count - 1)
		{
			return (false, null);
		}
		ResourceInfo value = eventSecondResourceRewards[index];
		if (value.ResourceType < 0)
		{
			return (false, null);
		}
		return (true, value);
	}

	public static (bool, PropertyAndValue?) CheckMapPickupConfigQualificationRewards(MapPickupsItem pickupConfig, int index)
	{
		List<PropertyAndValue> eventSecondPropertyRewards = pickupConfig.EventSecondPropertyRewards;
		if (index < 0 || eventSecondPropertyRewards == null || index > eventSecondPropertyRewards.Count - 1)
		{
			return (false, null);
		}
		PropertyAndValue value = eventSecondPropertyRewards[index];
		short propertyId = value.PropertyId;
		bool flag = propertyId >= 34 && propertyId <= 49;
		bool flag2 = propertyId >= 66 && propertyId <= 79;
		if (!flag && !flag2)
		{
			return (false, null);
		}
		return (true, value);
	}

	public static (bool, PropertyAndValue?) CheckMapPickupConfigCricketLuckPointRewards(MapPickupsItem pickupConfig, int index)
	{
		List<PropertyAndValue> eventSecondPropertyRewards = pickupConfig.EventSecondPropertyRewards;
		if (index < 0 || eventSecondPropertyRewards == null || index > eventSecondPropertyRewards.Count - 1)
		{
			return (false, null);
		}
		PropertyAndValue value = eventSecondPropertyRewards[index];
		short propertyId = value.PropertyId;
		if (propertyId != 111)
		{
			return (false, null);
		}
		return (true, value);
	}

	public static (bool, int?) CheckMapPickupConfigDebtRewards(MapPickupsItem pickupConfig, int index)
	{
		List<int> eventSecondDebtRewards = pickupConfig.EventSecondDebtRewards;
		if (index < 0 || eventSecondDebtRewards == null || index > eventSecondDebtRewards.Count - 1)
		{
			return (false, null);
		}
		int num = eventSecondDebtRewards[index];
		if (num < 0)
		{
			return (false, null);
		}
		return (true, num);
	}

	public static (bool, int?) CheckMapPickupConfigExpRewards(MapPickupsItem pickupConfig, int index)
	{
		List<int> eventSecondExpRewards = pickupConfig.EventSecondExpRewards;
		if (index < 0 || eventSecondExpRewards == null || index > eventSecondExpRewards.Count - 1)
		{
			return (false, null);
		}
		int value = eventSecondExpRewards[index];
		return (true, value);
	}

	public bool GiveMapPickupEventUserSelectedItemReward(DataContext context, ItemKey itemKey, int count)
	{
		AddItem(context, itemKey, count);
		return true;
	}

	public bool GiveMapPickupEventNormalReward(DataContext context, MapPickup pickup, int index)
	{
		if (index < 0)
		{
			return false;
		}
		var (flag, itemReward) = CheckMapPickupConfigItemRewards(pickup.Template, index);
		if (flag)
		{
			return AddItemByEventPickup(context, itemReward);
		}
		var (flag2, resourceInfo) = CheckMapPickupConfigResourceRewards(pickup.Template, index);
		if (flag2)
		{
			return AddResourceByEventPickup(context, resourceInfo.Value);
		}
		var (flag3, propertyAndValue) = CheckMapPickupConfigQualificationRewards(pickup.Template, index);
		if (flag3)
		{
			return AddQualificationByEventPickup(context, propertyAndValue.Value);
		}
		var (flag4, propertyAndValue2) = CheckMapPickupConfigCricketLuckPointRewards(pickup.Template, index);
		if (flag4)
		{
			return AddCricketLuckPointByEventPickup(context, propertyAndValue2.Value);
		}
		var (flag5, num) = CheckMapPickupConfigDebtRewards(pickup.Template, index);
		if (flag5)
		{
			return AddDebtByEventPickup(context, pickup, num.Value);
		}
		var (flag6, num2) = CheckMapPickupConfigExpRewards(pickup.Template, index);
		if (flag6)
		{
			return AddExpByEventPickup(context, num2.Value);
		}
		return false;
	}

	private static bool AddDebtByEventPickup(DataContext context, MapPickup pickup, int debtReward)
	{
		short areaId = pickup.Location.AreaId;
		DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, debtReward);
		return true;
	}

	private static bool AddExpByEventPickup(DataContext context, int expReward)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		taiwu.ChangeExp(context, expReward);
		return true;
	}

	private static bool AddQualificationByEventPickup(DataContext context, PropertyAndValue qualificationReward)
	{
		short value = qualificationReward.Value;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		ECharacterPropertyReferencedType eCharacterPropertyReferencedType = ECharacterPropertyReferencedType.QualificationMusic;
		ECharacterPropertyReferencedType eCharacterPropertyReferencedType2 = ECharacterPropertyReferencedType.QualificationEclectic;
		ECharacterPropertyReferencedType eCharacterPropertyReferencedType3 = ECharacterPropertyReferencedType.QualificationNeigong;
		ECharacterPropertyReferencedType eCharacterPropertyReferencedType4 = ECharacterPropertyReferencedType.QualificationCombatMusic;
		short propertyId = qualificationReward.PropertyId;
		if (propertyId >= (short)eCharacterPropertyReferencedType && propertyId <= (short)eCharacterPropertyReferencedType2)
		{
			LifeSkillShorts baseLifeSkillQualifications = taiwu.GetBaseLifeSkillQualifications();
			int index = propertyId - (short)eCharacterPropertyReferencedType;
			baseLifeSkillQualifications.Set(index, (short)(baseLifeSkillQualifications.Get(index) + value));
			taiwu.SetBaseLifeSkillQualifications(ref baseLifeSkillQualifications, context);
			return true;
		}
		if (propertyId >= (short)eCharacterPropertyReferencedType3 && propertyId <= (short)eCharacterPropertyReferencedType4)
		{
			CombatSkillShorts baseCombatSkillQualifications = taiwu.GetBaseCombatSkillQualifications();
			int index2 = propertyId - (short)eCharacterPropertyReferencedType3;
			baseCombatSkillQualifications[index2] += value;
			taiwu.SetBaseCombatSkillQualifications(ref baseCombatSkillQualifications, context);
			return true;
		}
		return false;
	}

	private static bool AddCricketLuckPointByEventPickup(DataContext context, PropertyAndValue cricketLuckPointReward)
	{
		short value = cricketLuckPointReward.Value;
		DomainManager.Taiwu.SetCricketLuckPoint(DomainManager.Taiwu.GetCricketLuckPoint() + value, context);
		return true;
	}

	private static bool AddResourceByEventPickup(DataContext context, ResourceInfo resourceReward)
	{
		sbyte resourceType = resourceReward.ResourceType;
		int resourceCount = resourceReward.ResourceCount;
		DomainManager.Taiwu.AddResource(context, ItemSourceType.Inventory, resourceType, resourceCount);
		return true;
	}

	private static bool AddItemByEventPickup(DataContext context, PresetItemWithCount itemReward)
	{
		sbyte itemType = itemReward.ItemType;
		short templateId = itemReward.TemplateId;
		int count = itemReward.Count;
		ItemKey itemKey = DomainManager.Item.CreateItem(context, itemType, templateId);
		if (!itemKey.IsValid())
		{
			return false;
		}
		AddItem(context, itemKey, count);
		return true;
	}

	private static void AddItem(DataContext context, ItemKey itemKey, int count)
	{
		DomainManager.Taiwu.AddItem(context, itemKey, count, ItemSourceType.Inventory);
	}

	public MapElementPickupDisplayData GetPickupDisplayData(MapPickup pickup)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return new MapElementPickupDisplayData
		{
			Pickup = pickup,
			BanReason = BoolArray32.op_Implicit(pickup.CalcMapPickupBanReason()),
			CanAutoBeatXiangshuMinion = pickup.CalcCanAutoBeatXiangshuMinion()
		};
	}

	[DomainMethod]
	public void TeleportByTraveler(DataContext context, short destBlockId)
	{
		SetTeleportMove(teleport: true);
		Move(context, destBlockId);
		SetTeleportMove(teleport: false);
	}

	[DomainMethod]
	public bool BuildTravelerPalace(DataContext context, Location location)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
		TravelerSkillsData skillsData = professionData.GetSkillsData<TravelerSkillsData>();
		if (skillsData.PalaceCount >= 3)
		{
			return false;
		}
		skillsData.OfflineBuildPalace(location);
		DomainManager.Extra.SetProfessionData(context, professionData);
		return true;
	}

	[DomainMethod]
	public bool ChangeTravelerPalaceName(DataContext context, int index, string newName)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
		TravelerSkillsData skillsData = professionData.GetSkillsData<TravelerSkillsData>();
		TravelerPalaceData travelerPalaceData = skillsData.TryGetPalaceData(index);
		if (travelerPalaceData == null)
		{
			return false;
		}
		travelerPalaceData.CustomName = newName;
		DomainManager.Extra.SetProfessionData(context, professionData);
		return true;
	}

	[DomainMethod]
	public bool DestroyTravelerPalace(DataContext context, int index)
	{
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
		TravelerSkillsData skillsData = professionData.GetSkillsData<TravelerSkillsData>();
		bool flag = skillsData.OfflineDestroyPalace(index);
		if (flag)
		{
			DomainManager.Extra.SetProfessionData(context, professionData);
		}
		return flag;
	}

	[DomainMethod]
	public bool TeleportOnTravelerPalace(DataContext context, int index)
	{
		if (IsTraveling)
		{
			return false;
		}
		if (DomainManager.Extra.GetTotalActionPointsRemaining() < 10)
		{
			return false;
		}
		DomainManager.Extra.ConsumeActionPoint(context, 10);
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
		TravelerSkillsData skillsData = professionData.GetSkillsData<TravelerSkillsData>();
		TravelerPalaceData travelerPalaceData = skillsData.TryGetPalaceData(index);
		if (travelerPalaceData == null)
		{
			return false;
		}
		if (DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId != travelerPalaceData.Location.AreaId)
		{
			QuickTravel(context, travelerPalaceData.Location.AreaId);
		}
		TeleportByTraveler(context, travelerPalaceData.Location.BlockId);
		foreach (int item in DomainManager.Taiwu.GetGroupCharIds().GetCollection())
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			MakeRandomTravelerPalaceDisaster(context, element_Objects);
		}
		return true;
	}

	private static void MakeRandomTravelerPalaceDisaster(DataContext context, GameData.Domains.Character.Character groupChar)
	{
		switch (context.Random.Next(4))
		{
		case 0:
		{
			for (int i = 0; i < ProfessionRelatedConstants.TravelerPalaceRandomInjuryCount(context.Random); i++)
			{
				sbyte bodyPartType = (sbyte)context.Random.Next(7);
				bool isInnerInjury = context.Random.NextBool();
				groupChar.ChangeInjury(context, bodyPartType, isInnerInjury, 1);
			}
			break;
		}
		case 1:
		{
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			for (sbyte b = 0; b < 6; b++)
			{
				list.Add(b);
			}
			foreach (sbyte item in RandomUtils.GetRandomUnrepeated(context.Random, 3, list))
			{
				groupChar.ChangePoisoned(context, item, 3, ProfessionRelatedConstants.TravelerRandomPoisonValue(context.Random));
			}
			ObjectPool<List<sbyte>>.Instance.Return(list);
			break;
		}
		case 2:
		{
			int delta = ProfessionRelatedConstants.TravelerRandomQiDisorderValue(context.Random);
			groupChar.ChangeDisorderOfQiRandomRecovery(context, delta);
			break;
		}
		case 3:
		{
			int num = ProfessionRelatedConstants.TravelerRandomHealthValue(context.Random);
			groupChar.ChangeHealth(context, -num);
			break;
		}
		}
	}

	[DomainMethod]
	public Location QueryFixedCharacterLocation(short templateId)
	{
		GameData.Domains.Character.Character character;
		return DomainManager.Character.TryGetFixedCharacterByTemplateId(templateId, out character) ? character.GetLocation() : Location.Invalid;
	}

	[DomainMethod]
	public Location QueryFixedCharacterLocationInArea(short templateId, short areaId)
	{
		Location location = QueryFixedCharacterLocation(templateId);
		return (location.AreaId == areaId) ? location : Location.Invalid;
	}

	[DomainMethod]
	public Location QueryTemplateBlockLocation(int templateId)
	{
		for (short num = 0; num < 45; num++)
		{
			Location result = QueryTemplateBlockLocationInArea(templateId, num);
			if (result.IsValid())
			{
				return result;
			}
		}
		return Location.Invalid;
	}

	[DomainMethod]
	public Location QueryTemplateBlockLocationInArea(int templateId, short areaId)
	{
		if ((areaId < 0 || areaId >= 45) ? true : false)
		{
			return Location.Invalid;
		}
		AreaBlockCollection areaBlockCollection = _regularAreaBlocksArray[areaId];
		MapBlockData[] array = areaBlockCollection.GetArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].TemplateId == templateId)
			{
				return new Location(areaId, array[i].BlockId);
			}
		}
		return Location.Invalid;
	}

	public void QueryRootBlocks(List<MapBlockData> rootBlocks, Location location)
	{
		rootBlocks.Clear();
		if (!location.IsValid())
		{
			return;
		}
		Span<MapBlockData> areaBlocks = GetAreaBlocks(location.AreaId);
		Span<MapBlockData> span = areaBlocks;
		for (int i = 0; i < span.Length; i++)
		{
			MapBlockData mapBlockData = span[i];
			if (mapBlockData.BlockId == location.BlockId || mapBlockData.RootBlockId == location.BlockId)
			{
				rootBlocks.Add(mapBlockData);
			}
		}
	}

	public void QueryRegularBelongBlocks(List<MapBlockData> belongBlocks, Location location, bool includeSect, params MapBlockDataFilter[] extraFilters)
	{
		belongBlocks.Clear();
		if (!location.IsValid() || location.AreaId >= _regularAreaBlocksArray.Length)
		{
			return;
		}
		AreaBlockCollection areaBlockCollection = _regularAreaBlocksArray[location.AreaId];
		MapBlockData[] array = areaBlockCollection.GetArray();
		foreach (MapBlockData mapBlockData in array)
		{
			if (IsFiltersPass(mapBlockData, extraFilters) && mapBlockData.IsPassable())
			{
				if (includeSect && (mapBlockData.BlockId == location.BlockId || mapBlockData.RootBlockId == location.BlockId))
				{
					belongBlocks.Add(mapBlockData);
				}
				else if (mapBlockData.BelongBlockId == location.BlockId)
				{
					belongBlocks.Add(mapBlockData);
				}
			}
		}
	}

	public int QueryAreaBrokenLevel(short areaId)
	{
		if ((areaId < 45 || areaId >= 135) ? true : false)
		{
			return -1;
		}
		return DomainManager.Adventure.GetElement_BrokenAreaEnemies(areaId - 45).Level;
	}

	private static bool IsFiltersPass(MapBlockData blockData, IEnumerable<MapBlockDataFilter> filters)
	{
		if (filters == null)
		{
			return true;
		}
		foreach (MapBlockDataFilter filter in filters)
		{
			if (!filter(blockData))
			{
				return false;
			}
		}
		return true;
	}

	public static bool QueryFilterAnyCharacter(MapBlockData blockData)
	{
		return blockData.CharacterSet != null && blockData.CharacterSet.Count > 0;
	}

	public MapBlockData GetRandomMapBlockDataByFilters(IRandomSource random, sbyte stateTemplateId, sbyte areaFilterType, List<short> mapBlockSubTypes, bool includeBlockWithAdventure)
	{
		if (mapBlockSubTypes != null && mapBlockSubTypes.Count > 0)
		{
			EMapBlockSubType eMapBlockSubType = (EMapBlockSubType)mapBlockSubTypes[random.Next(0, mapBlockSubTypes.Count)];
			if (eMapBlockSubType == EMapBlockSubType.TaiwuCun)
			{
				return GetBlock(DomainManager.Taiwu.GetTaiwuVillageLocation());
			}
			if (eMapBlockSubType > EMapBlockSubType.TaiwuCun && eMapBlockSubType < EMapBlockSubType.Farmland)
			{
				for (short num = 0; num < 45; num++)
				{
					AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(num);
					short mainSettlementMainBlockId = DomainManager.Map.GetMainSettlementMainBlockId(num);
					MapBlockData block = DomainManager.Map.GetBlock(num, mainSettlementMainBlockId);
					if (block.BlockSubType == eMapBlockSubType)
					{
						List<short> list = new List<short>();
						GetSettlementBlocks(num, mainSettlementMainBlockId, list);
						for (int num2 = list.Count - 1; num2 >= 0; num2--)
						{
							short num3 = list[num2];
							if (!includeBlockWithAdventure && adventuresInArea.AdventureSites.ContainsKey(num3))
							{
								list.Remove(num3);
							}
						}
						return GetBlock(num, list[random.Next(list.Count)]);
					}
				}
			}
		}
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		list2.Clear();
		if (stateTemplateId == 0)
		{
			list2.Add(135);
		}
		else
		{
			if (stateTemplateId == -1)
			{
				stateTemplateId = (sbyte)random.Next(1, 16);
			}
			int index = 3;
			DomainManager.Map.GetAllAreaInState((sbyte)(stateTemplateId - 1), list2);
			list2.RemoveRange(index, 6);
		}
		MapStateItem stateConfig = MapState.Instance[stateTemplateId];
		if (1 == 0)
		{
		}
		short num4 = areaFilterType switch
		{
			-1 => list2[random.Next(0, list2.Count)], 
			0 => DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId, 
			1 => stateConfig.MainAreaID, 
			2 => stateConfig.SectAreaID, 
			3 => list2.First((short area) => area != stateConfig.MainAreaID && area != stateConfig.SectAreaID), 
			_ => throw new Exception($"Unrecognized area filter type {areaFilterType}"), 
		};
		if (1 == 0)
		{
		}
		short areaId = num4;
		ObjectPool<List<short>>.Instance.Return(list2);
		return GetRandomMapBlockDataInAreaByFilters(random, areaId, mapBlockSubTypes, includeBlockWithAdventure);
	}

	public void GetMapBlocksInAreaByFilters(short areaId, Predicate<MapBlockData> predicate, List<MapBlockData> result)
	{
		result.Clear();
		Span<MapBlockData> areaBlocks = GetAreaBlocks(areaId);
		Span<MapBlockData> span = areaBlocks;
		for (int i = 0; i < span.Length; i++)
		{
			MapBlockData mapBlockData = span[i];
			if (mapBlockData.IsPassable() && !DomainManager.Extra.IsLocationInDreamBack(mapBlockData.GetLocation()) && predicate(mapBlockData))
			{
				result.Add(mapBlockData);
			}
		}
	}

	public MapBlockData GetRandomMapBlockDataInAreaByFilters(IRandomSource random, short areaId, IReadOnlyCollection<short> mapBlockSubTypes, bool includeBlocksWithAdventure)
	{
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		Dictionary<short, AdventureSiteData> adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(areaId).AdventureSites;
		Predicate<MapBlockData> predicate = ((mapBlockSubTypes != null && mapBlockSubTypes.Count > 0) ? new Predicate<MapBlockData>(IsBlockInList) : new Predicate<MapBlockData>(IsBlockNormalOrWild));
		GetMapBlocksInAreaByFilters(areaId, predicate, list);
		MapBlockData result = ((list.Count > 0) ? list[random.Next(list.Count)] : null);
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		return result;
		bool IsBlockInList(MapBlockData mapBlockData)
		{
			if (!mapBlockSubTypes.Contains((short)mapBlockData.BlockType))
			{
				return false;
			}
			if (!includeBlocksWithAdventure)
			{
				return !adventuresInArea.ContainsKey(mapBlockData.BlockId);
			}
			return true;
		}
		bool IsBlockNormalOrWild(MapBlockData mapBlockData)
		{
			if (mapBlockData.BlockSubType == EMapBlockSubType.SwordTomb || mapBlockData.BlockSubType == EMapBlockSubType.DLCLoong || (mapBlockData.BlockType != EMapBlockType.Normal && mapBlockData.BlockType != EMapBlockType.Wild))
			{
				return false;
			}
			if (!includeBlocksWithAdventure)
			{
				return !adventuresInArea.ContainsKey(mapBlockData.BlockId);
			}
			return true;
		}
	}

	public MapBlockData SelectBlockInCurrentOrNeighborState(IRandomSource random, Location centerLocation, Predicate<MapBlockData> condition, bool taiwuVillageInfluenceRangeIsLast = false)
	{
		MapBlockData mapBlockData = SelectBlockInArea(random, centerLocation.AreaId, condition, taiwuVillageInfluenceRangeIsLast);
		if (mapBlockData != null)
		{
			return mapBlockData;
		}
		sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(centerLocation.AreaId);
		mapBlockData = SelectBlockInState(random, stateIdByAreaId, condition);
		if (mapBlockData != null)
		{
			return mapBlockData;
		}
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(centerLocation.AreaId);
		sbyte[] neighborStates = MapState.Instance[stateTemplateIdByAreaId].NeighborStates;
		sbyte[] array = neighborStates;
		foreach (sbyte stateTemplateId in array)
		{
			sbyte stateIdByStateTemplateId = DomainManager.Map.GetStateIdByStateTemplateId(stateTemplateId);
			mapBlockData = SelectBlockInState(random, stateIdByStateTemplateId, condition);
			if (mapBlockData != null)
			{
				return mapBlockData;
			}
		}
		if (taiwuVillageInfluenceRangeIsLast)
		{
			mapBlockData = SelectBlockInTaiwuVillageInfluenceRange(random, condition);
			if (mapBlockData != null)
			{
				return mapBlockData;
			}
		}
		return null;
	}

	private MapBlockData SelectBlockInState(IRandomSource random, sbyte stateId, Predicate<MapBlockData> condition)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		DomainManager.Map.GetAllRegularAreaInState(stateId, list);
		CollectionUtils.Shuffle(random, list);
		MapBlockData mapBlockData = null;
		foreach (short item in list)
		{
			mapBlockData = SelectBlockInArea(random, item, condition);
			if (mapBlockData != null)
			{
				break;
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		return mapBlockData;
	}

	public MapBlockData SelectBlockInArea(IRandomSource random, short areaId, Predicate<MapBlockData> condition, bool exceptTaiwuVillageInfluenceRange = false)
	{
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		DomainManager.Map.GetMapBlocksInAreaByFilters(areaId, condition, list);
		if (exceptTaiwuVillageInfluenceRange)
		{
			short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			list.RemoveAll((MapBlockData b) => DomainManager.Map.IsLocationInSettlementInfluenceRange(b.GetLocation(), settlementId));
		}
		MapBlockData randomOrDefault = list.GetRandomOrDefault(random, null);
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		return randomOrDefault;
	}

	public MapBlockData SelectBlockInTaiwuVillageInfluenceRange(IRandomSource random, Predicate<MapBlockData> condition)
	{
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		DomainManager.Map.GetMapBlocksInAreaByFilters(taiwuVillageLocation.AreaId, condition, list);
		short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		list.RemoveAll((MapBlockData b) => !DomainManager.Map.IsLocationInSettlementInfluenceRange(b.GetLocation(), settlementId));
		MapBlockData randomOrDefault = list.GetRandomOrDefault(random, null);
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		return randomOrDefault;
	}

	public MapBlockData GetBelongSettlementBlock(Location location)
	{
		MapBlockData block = GetBlock(location);
		if (block.IsCityTown())
		{
			return block.GetRootBlock();
		}
		if (block.BelongBlockId >= 0)
		{
			MapBlockData block2 = GetBlock(new Location(location.AreaId, block.BelongBlockId));
			if (block2.IsCityTown())
			{
				return block2;
			}
		}
		return null;
	}

	public bool IsLocationInSettlementInfluenceRange(Location location, short settlementId)
	{
		Location location2 = DomainManager.Organization.GetSettlement(settlementId).GetLocation();
		if (location.AreaId != location2.AreaId)
		{
			return false;
		}
		MapBlockData block = GetBlock(location);
		return block.BlockId == location2.BlockId || block.RootBlockId == location2.BlockId || block.BelongBlockId == location2.BlockId;
	}

	public bool IsLocationInOrganizationInfluenceRange(Location location, sbyte orgTemplateId)
	{
		MapBlockData belongSettlementBlock = GetBelongSettlementBlock(location);
		if (belongSettlementBlock == null)
		{
			return false;
		}
		Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(belongSettlementBlock.GetLocation());
		return settlementByLocation.GetOrgTemplateId() == orgTemplateId;
	}

	public bool IsLocationOnSettlementBlock(Location location, short settlementId)
	{
		MapBlockData mapBlockData = GetBlock(location)?.GetRootBlock();
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		if (mapBlockData == null || settlement == null)
		{
			return false;
		}
		Location location2 = settlement.GetLocation();
		return mapBlockData.AreaId == location2.AreaId && mapBlockData.BlockId == location2.BlockId;
	}

	public bool CheckLocationsHasSameRoot(Location locationA, Location locationB)
	{
		if (!locationA.IsValid() || !locationB.IsValid())
		{
			return false;
		}
		if (locationA == locationB)
		{
			return true;
		}
		MapBlockData block = GetBlock(locationA);
		MapBlockData block2 = GetBlock(locationB);
		return block.GetRootBlock() == block2.GetRootBlock();
	}

	public void GetSettlementBlocks(short areaId, short blockId, List<short> blockIds)
	{
		blockIds.Add(blockId);
		List<MapBlockData> groupBlockList = GetRegularAreaBlocks(areaId)[blockId].GroupBlockList;
		if (groupBlockList != null)
		{
			int i = 0;
			for (int count = groupBlockList.Count; i < count; i++)
			{
				blockIds.Add(groupBlockList[i].BlockId);
			}
		}
	}

	public void GetSettlementBlocksWithoutAdventure(short areaId, short blockId, List<short> blockIds)
	{
		List<MapBlockData> groupBlockList = GetRegularAreaBlocks(areaId)[blockId].GroupBlockList;
		if (groupBlockList == null)
		{
			return;
		}
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(areaId);
		int i = 0;
		for (int count = groupBlockList.Count; i < count; i++)
		{
			if (!Enumerable.Contains(adventuresInArea.AdventureSites.Keys, groupBlockList[i].BlockId))
			{
				blockIds.Add(groupBlockList[i].BlockId);
			}
		}
		if (!Enumerable.Contains(adventuresInArea.AdventureSites.Keys, blockId))
		{
			blockIds.Add(blockId);
		}
	}

	public void GetSettlementBlocksAndAffiliatedBlocks(short areaId, short blockId, List<short> blockIds)
	{
		blockIds.Add(blockId);
		AreaBlockCollection regularAreaBlocks = GetRegularAreaBlocks(areaId);
		int i = 0;
		for (int count = regularAreaBlocks.Count; i < count; i++)
		{
			MapBlockData mapBlockData = regularAreaBlocks[(short)i];
			if (mapBlockData.RootBlockId == blockId || (mapBlockData.BelongBlockId == blockId && mapBlockData.IsPassable()))
			{
				blockIds.Add(mapBlockData.BlockId);
			}
		}
	}

	public void GetSettlementAffiliatedBlocks(short areaId, short blockId, List<MapBlockData> blocks)
	{
		blocks.Clear();
		AreaBlockCollection regularAreaBlocks = GetRegularAreaBlocks(areaId);
		int i = 0;
		for (int count = regularAreaBlocks.Count; i < count; i++)
		{
			MapBlockData mapBlockData = regularAreaBlocks[(short)i];
			if (mapBlockData.BelongBlockId == blockId && mapBlockData.IsPassable())
			{
				blocks.Add(mapBlockData);
			}
		}
	}

	[DomainMethod]
	public Dictionary<short, short> GetAllSettlementInfluenceRangeBlocks(short areaId)
	{
		Span<MapBlockData> areaBlocks = GetAreaBlocks(areaId);
		Dictionary<short, short> dictionary = new Dictionary<short, short>();
		Span<MapBlockData> span = areaBlocks;
		for (int i = 0; i < span.Length; i++)
		{
			MapBlockData mapBlockData = span[i];
			MapBlockData belongSettlementBlock = GetBelongSettlementBlock(mapBlockData.GetLocation());
			if (belongSettlementBlock != null)
			{
				dictionary.Add(mapBlockData.BlockId, belongSettlementBlock.BlockId);
			}
		}
		return dictionary;
	}

	[Obsolete("Use IsLocationInSettlementInfluenceRange")]
	[DomainMethod]
	public bool IsLocationInBuildingEffectRange(Location location, Location settlementLocation)
	{
		if (location.AreaId != settlementLocation.AreaId)
		{
			return false;
		}
		MapBlockData block = GetBlock(location);
		return block.BlockId == settlementLocation.BlockId || block.RootBlockId == settlementLocation.BlockId || block.BelongBlockId == settlementLocation.BlockId;
	}

	[Obsolete("Use IsLocationOnSettlementBlock instead.")]
	public bool IsLocationInSettlementRange(Location location, sbyte organizationTemplateId)
	{
		return IsLocationOnSettlementBlock(location, DomainManager.Organization.GetSettlementIdByOrgTemplateId(organizationTemplateId));
	}

	[Obsolete("Use IsLocationOnSettlementBlock instead.")]
	public bool BelongSettlementBlock(short areaId, short blockId, Location location)
	{
		if (areaId != location.AreaId)
		{
			return false;
		}
		if (blockId == location.BlockId)
		{
			return true;
		}
		List<MapBlockData> groupBlockList = GetRegularAreaBlocks(areaId)[blockId].GroupBlockList;
		if (groupBlockList == null)
		{
			return false;
		}
		int i = 0;
		for (int count = groupBlockList.Count; i < count; i++)
		{
			if (groupBlockList[i].BlockId == location.BlockId)
			{
				return true;
			}
		}
		return false;
	}

	[Obsolete("Use IsLocationInSettlementInfluenceRange instead.")]
	public bool BelongSettlementBlocksAndAffiliatedBlocks(short areaId, short blockId, Location location)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		GetSettlementBlocksAndAffiliatedBlocks(areaId, blockId, list);
		if (areaId == location.AreaId && list.Contains(location.BlockId))
		{
			ObjectPool<List<short>>.Instance.Return(list);
			return true;
		}
		ObjectPool<List<short>>.Instance.Return(list);
		return false;
	}

	[Obsolete("Use IsLocationInSettlementInfluenceRange")]
	public bool IsLocationAtSettlementRange(Location currLocation, short settlementId)
	{
		return IsLocationInSettlementInfluenceRange(currLocation, settlementId);
	}

	public void InitializeSpecialBlocksData()
	{
		UpdateWudangHeavenlyTreeLocations();
		UpdateFulongFlameLocations();
	}

	public void UpdateWudangHeavenlyTreeLocations()
	{
		List<SectStoryHeavenlyTreeExtendable> allHeavenlyTrees = DomainManager.Extra.GetAllHeavenlyTrees();
		_wudangHeavenlyTrees.Clear();
		foreach (SectStoryHeavenlyTreeExtendable item in allHeavenlyTrees)
		{
			_wudangHeavenlyTrees.Add(item.Location);
		}
	}

	public void UpdateFulongFlameLocations()
	{
		List<FulongInFlameArea> allFulongInFlameAreas = DomainManager.Extra.GetAllFulongInFlameAreas();
		_fulongLightedBlocks.Clear();
		foreach (FulongInFlameArea item in allFulongInFlameAreas)
		{
			foreach (short key in item.LightedBlocks.Keys)
			{
				_fulongLightedBlocks.Add(key);
			}
		}
	}

	public bool IsBlockOccupiedByCriticalAdventure(short areaId, short blockId)
	{
		AdventureSiteData value;
		return DomainManager.Adventure.GetElement_AdventureAreas(areaId).AdventureSites.TryGetValue(blockId, out value) && !Config.AdventureType.Instance[Config.Adventure.Instance[value.TemplateId].Type].IsTrivial;
	}

	public bool IsBlockSpecial(MapBlockData mapBlockData, bool strictCheck = true)
	{
		if (FiveLoongDlcEntry.IsBlockLoongBlock(mapBlockData))
		{
			return true;
		}
		if (IsLocationInFulongFlameArea(mapBlockData.GetLocation()))
		{
			return true;
		}
		if (strictCheck && _wudangHeavenlyTrees.Contains(mapBlockData.GetLocation()))
		{
			return true;
		}
		return false;
	}

	public bool IsLocationInFulongFlameArea(Location location)
	{
		List<FulongInFlameArea> allFulongInFlameAreas = DomainManager.Extra.GetAllFulongInFlameAreas();
		foreach (FulongInFlameArea item in allFulongInFlameAreas)
		{
			if (item.IsLocationInActiveFlame(location))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsBlockAvailable(MapBlockData mapBlockData, bool strictCheck)
	{
		return mapBlockData.IsNonDeveloped() && !mapBlockData.Destroyed && !IsBlockSpecial(mapBlockData, strictCheck) && mapBlockData.GetConfig().TemplateId != 126 && mapBlockData.IsPassable() && !IsBlockOccupiedByCriticalAdventure(mapBlockData.AreaId, mapBlockData.BlockId) && (!strictCheck || mapBlockData.GetConfig().TemplateId != 124);
	}

	public void RemoveTrivialObjectsOnBlocks(DataContext context, List<MapBlockData> mapBlocks)
	{
		foreach (MapBlockData mapBlock in mapBlocks)
		{
			Location location = mapBlock.GetLocation();
			if (DomainManager.Extra.TryGetAnimalIdsByLocation(location, out var animals))
			{
				for (int num = animals.Count - 1; num >= 0; num--)
				{
					int id = animals[num];
					DomainManager.Extra.ApplyAnimalDeadByAccident(context, id);
				}
			}
			if (DomainManager.Adventure.GetElement_AdventureAreas(location.AreaId).AdventureSites.TryGetValue(location.BlockId, out var _))
			{
				DomainManager.Adventure.RemoveAdventureSite(context, location.AreaId, location.BlockId, isTimeout: false, isComplete: false);
			}
			List<MapTemplateEnemyInfo> templateEnemyList = mapBlock.TemplateEnemyList;
			if (templateEnemyList != null && templateEnemyList.Count > 0)
			{
				for (int num2 = mapBlock.TemplateEnemyList.Count - 1; num2 >= 0; num2--)
				{
					MapTemplateEnemyInfo templateEnemyInfo = mapBlock.TemplateEnemyList[num2];
					Events.RaiseTemplateEnemyLocationChanged(context, templateEnemyInfo, location, Location.Invalid);
				}
			}
			if (LocationHasCricket(context, location))
			{
				int num3 = Array.IndexOf(_cricketPlaceData[location.AreaId].CricketBlocks, location.BlockId);
				_cricketPlaceData[location.AreaId].CricketTriggered[num3] = true;
				SetElement_CricketPlaceData(location.AreaId, _cricketPlaceData[location.AreaId], context);
			}
		}
	}

	public bool TryGetAvailableBlockIdInRange(short areaId, int range, List<MapBlockData> neighborBlocks)
	{
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		HashSet<short> visited = new HashSet<short>();
		return TryGetAvailableBlockIdInRange(areaId, range, isStrict: true, areaBlocks, visited, neighborBlocks) || TryGetAvailableBlockIdInRange(areaId, range, isStrict: false, areaBlocks, visited, neighborBlocks);
	}

	private bool TryGetAvailableBlockIdInRange(short areaId, int range, bool isStrict, Span<MapBlockData> mapBlocks, HashSet<short> visited, List<MapBlockData> neighborBlocks)
	{
		int num = range + ((!isStrict) ? 1 : 2);
		byte areaSize = GetAreaSize(areaId);
		visited.Clear();
		Span<MapBlockData> span = mapBlocks;
		for (int i = 0; i < span.Length; i++)
		{
			MapBlockData mapBlockData = span[i];
			bool flag = true;
			ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(mapBlockData.BlockId, areaSize);
			neighborBlocks.Clear();
			for (int j = byteCoordinate.X - num; j <= byteCoordinate.X + num; j++)
			{
				if (!flag)
				{
					break;
				}
				for (int k = byteCoordinate.Y - num; k <= byteCoordinate.Y + num; k++)
				{
					if (j < 0 || j >= areaSize || k < 0 || k >= areaSize)
					{
						flag = false;
						break;
					}
					ByteCoordinate byteCoordinate2 = new ByteCoordinate((byte)j, (byte)k);
					int manhattanDistance = byteCoordinate.GetManhattanDistance(byteCoordinate2);
					if (manhattanDistance > num)
					{
						continue;
					}
					MapBlockData mapBlockData2 = mapBlocks[ByteCoordinate.CoordinateToIndex(byteCoordinate2, areaSize)];
					if (!neighborBlocks.Contains(mapBlockData2))
					{
						if (visited.Contains(mapBlockData2.BlockId))
						{
							flag = false;
							break;
						}
						if (!IsBlockAvailable(mapBlockData2, isStrict))
						{
							visited.Add(mapBlockData2.BlockId);
							flag = false;
							break;
						}
						if (manhattanDistance <= range)
						{
							neighborBlocks.Add(mapBlockData2);
						}
					}
				}
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	public void IncreaseMoveBanned(DataContext context)
	{
		SetMoveBanned(_moveBanned + 1, context);
	}

	public void DecreaseMoveBanned(DataContext context)
	{
		SetMoveBanned(_moveBanned - 1, context);
		if (_moveBanned < 0)
		{
			AdaptableLog.Warning($"Move banned count less than zero, current value is {_moveBanned}");
		}
	}

	public void Move(DataContext context, short destBlockId, bool notCostTime)
	{
		Logger.Info($"Move to {destBlockId}");
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location key = (TaiwuLastLocation = taiwu.GetLocation());
		Location location2 = new Location(key.AreaId, destBlockId);
		MapAreaData mapAreaData = _areas[key.AreaId];
		MapBlockData block = GetBlock(key);
		MapBlockData block2 = GetBlock(location2);
		byte areaSize = GetAreaSize(key.AreaId);
		MapBlockItem config = block2.GetConfig();
		int val = config.MoveCost * DomainManager.Taiwu.GetMoveTimeCostPercent() / 10;
		val = Math.Max(1, val);
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(key.BlockId, areaSize);
		ByteCoordinate byteCoordinate2 = ByteCoordinate.IndexToCoordinate(destBlockId, areaSize);
		bool flag = Math.Abs(byteCoordinate2.X - byteCoordinate.X) + Math.Abs(byteCoordinate2.Y - byteCoordinate.Y) == 1;
		if (byteCoordinate == byteCoordinate2)
		{
			return;
		}
		if (DomainManager.TutorialChapter.InGuiding)
		{
			Location nextForceLocation = DomainManager.TutorialChapter.GetNextForceLocation();
			if (nextForceLocation != Location.Invalid && nextForceLocation != location2)
			{
				return;
			}
		}
		if (!block2.IsPassable())
		{
			PredefinedLog.Show(11, $"Block is not passable: {destBlockId}");
			return;
		}
		if (!flag && !_teleportMove)
		{
			PredefinedLog.Show(11, $"Can only move to a neighbor block: src={key.BlockId}, dst={destBlockId}");
			return;
		}
		if (!DomainManager.Extra.IsActionPointEnough(val) && !notCostTime)
		{
			PredefinedLog.Show(11, $"Cannot advance days across month: {val}, left days: {DomainManager.Extra.GetTotalActionPointsRemaining()}");
			return;
		}
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			element_Objects.SetLocation(location2, context);
		}
		DomainManager.Extra.GearMateFollowTaiwu(context);
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
		TravelerSkillsData skillsData = professionData.GetSkillsData<TravelerSkillsData>();
		int num = skillsData.RecordMovementConsumedActionPoints(val);
		if (num > 0)
		{
			DomainManager.Extra.ChangeProfessionSeniority(context, 11, num);
		}
		else
		{
			DomainManager.Extra.SetProfessionData(context, professionData);
		}
		SetBlockAndViewRangeVisibleByMove(context, block2);
		AddAnimalProfessionSeniority(context, location2);
		if (block2.IsCityTown())
		{
			short settlementId = mapAreaData.SettlementInfos[mapAreaData.GetSettlementIndex(block2.GetRootBlock().BlockId)].SettlementId;
			DomainManager.Taiwu.TryAddVisitedSettlement(settlementId, context);
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			Location location3 = settlement.GetLocation();
			MapBlockData block3 = DomainManager.Map.GetBlock(location3);
			short templateId = block3.GetConfig().TemplateId;
			if (!DomainManager.Extra.CheckIsUnlockedSectXuannvMusicByMapBlockId(templateId))
			{
				DomainManager.Extra.UnlockSectXuannvMusicByMapBlockId(context, templateId);
			}
		}
		if (config.TemplateId == 124)
		{
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			IEnumerable<int> enumerable = collection;
			if (taiwu.IsActiveExternalRelationState(2))
			{
				enumerable = enumerable.Union(from c in DomainManager.Character.GetKidnappedCharacters(taiwu.GetId()).GetCollection()
					select c.CharId);
			}
			foreach (int item2 in enumerable)
			{
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2);
				Injuries injuries = element_Objects2.GetInjuries();
				list.Clear();
				list.AddRange(BodyPart.Instance.GetAllKeys());
				list.RemoveAll((sbyte part) => injuries.Get(part, isInnerInjury: false) >= 6);
				if (list.Count > 0)
				{
					injuries.Change(list[context.Random.Next(list.Count)], isInnerInjury: false, 1);
					element_Objects2.SetInjuries(injuries, context);
				}
				ReduceCharCarrierDurability(context, item2);
			}
			ObjectPool<List<sbyte>>.Instance.Return(list);
		}
		else if (config.SubType == EMapBlockSubType.Ruin || block2.Destroyed)
		{
			foreach (int item3 in collection)
			{
				ReduceCharCarrierDurability(context, item3);
			}
		}
		UpdateIsTaiwuInFulongFlameArea(context);
		if (!notCostTime)
		{
			DomainManager.World.ConsumeActionPoint(context, val);
		}
		Events.RaiseTaiwuMove(context, block, block2, val);
	}

	[DomainMethod]
	public void Move(DataContext context, short destBlockId)
	{
		Move(context, destBlockId, _lockMoveTime || _teleportMove || _crossArchiveLockMoveTime);
	}

	[DomainMethod]
	public void MoveFinish(DataContext context, Location previous, Location current)
	{
		if (!DomainManager.Map.GetCrossArchiveLockMoveTime())
		{
			MapBlockData blockData = GetBlockData(current.AreaId, current.BlockId);
			if (blockData != null)
			{
				MapBlockItem config = blockData.GetConfig();
				if (config != null)
				{
					if (config.TemplateId == 124)
					{
						DomainManager.World.GetInstantNotifications().AddWalkThroughAbyss();
					}
					else if (config.SubType == EMapBlockSubType.Ruin && IsAnyGroupCharEquippingCarrier())
					{
						DomainManager.World.GetInstantNotifications().AddWalkThroughErosionBlock();
					}
				}
				if (blockData.Destroyed && IsAnyGroupCharEquippingCarrier())
				{
					DomainManager.World.GetInstantNotifications().AddWalkThroughDestroyBlock();
				}
			}
		}
		DomainManager.Extra.UpdateFollowMovementCharacters(context);
		RefreshTaiwuMoveRecord(previous);
		DomainManager.TaiwuEvent.OnEvent_TaiwuBlockChanged(previous, current);
	}

	private void RefreshTaiwuMoveRecord(Location previous)
	{
		TaiwuMoveRecord.Enqueue(previous);
		while (TaiwuMoveRecord.Count > 3)
		{
			TaiwuMoveRecord.Dequeue();
		}
	}

	[DomainMethod]
	public bool IsContinuousMovingBreak()
	{
		return DomainManager.TaiwuEvent.IsShowingEvent || DomainManager.TaiwuEvent.GetHasListeningEvent();
	}

	[DomainMethod]
	public MapHealSimulateResult SimulateHealCost(int typeInt, int doctorId, int patientId, bool needPay = false, bool isExpensiveHeal = false)
	{
		if (GameData.Domains.Character.Character.AllHealActions.Contains((EHealActionType)typeInt))
		{
			return SimulateHealCost((EHealActionType)typeInt, doctorId, patientId, needPay, isExpensiveHeal);
		}
		Logger.Warn($"SimulateHealCost by invalid type {typeInt}");
		return default(MapHealSimulateResult);
	}

	[DomainMethod]
	public bool HealOnMap(DataContext context, int typeInt, int doctorId, int patientId, bool needPay = false, int payerId = -1, bool isExpensiveHeal = false)
	{
		if (GameData.Domains.Character.Character.AllHealActions.Contains((EHealActionType)typeInt))
		{
			return HealOnMap(context, (EHealActionType)typeInt, doctorId, patientId, needPay, payerId, isExpensiveHeal);
		}
		Logger.Warn($"HealOnMap by invalid type {typeInt}");
		return false;
	}

	private void ReduceCharCarrierDurability(DataContext context, int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		ItemKey itemKey = element_Objects.GetEquipment()[11];
		if (itemKey.IsValid())
		{
			int num = -context.Random.Next(GlobalConfig.CarrierDurationReduceOnRuinBlock[0], GlobalConfig.CarrierDurationReduceOnRuinBlock[1]);
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			short currDurability = baseItem.GetCurrDurability();
			short currDurability2 = (short)Math.Clamp(num + currDurability, 0, baseItem.GetMaxDurability());
			baseItem.SetCurrDurability(currDurability2, context);
		}
	}

	private bool IsAnyGroupCharEquippingCarrier()
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			ItemKey itemKey = element_Objects.GetEquipment()[11];
			if (itemKey.IsValid())
			{
				return true;
			}
		}
		return false;
	}

	private MapHealSimulateResult SimulateHealCost(EHealActionType type, int doctorId, int patientId, bool needPay = false, bool isExpensiveHeal = false)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(doctorId);
		GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(patientId);
		int maxRequireAttainment;
		int healEffect = element_Objects.CalcHealEffect(type, element_Objects2, out maxRequireAttainment, isExpensiveHeal);
		int costHerb = element_Objects2.CalcHealCostHerb(type, isExpensiveHeal);
		int costMoney = (needPay ? element_Objects2.CalcHealCostMoney(type, element_Objects.GetBehaviorType(), isExpensiveHeal) : 0);
		int costSpiritualDebt = (isExpensiveHeal ? element_Objects2.CalcHealCostSpiritualDebt(type) : 0);
		return new MapHealSimulateResult(type, costHerb, costMoney, healEffect, costSpiritualDebt, maxRequireAttainment);
	}

	private bool HealOnMap(DataContext context, EHealActionType type, int doctorId, int patientId, bool needPay = false, int payerId = -1, bool isExpensiveHeal = false)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(doctorId);
		GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(patientId);
		if (DomainManager.Character.GetUsableCombatResources(doctorId).Get(type) <= 0)
		{
			Logger.Warn("Heal count not enough");
			return false;
		}
		int num = element_Objects2.CalcHealCostHerb(type, isExpensiveHeal);
		if (element_Objects.GetResource(5) < num)
		{
			Logger.Warn($"Herb not enough. need {num}, has {element_Objects.GetResource(5)}");
			return false;
		}
		int num2 = (needPay ? element_Objects2.CalcHealCostMoney(type, element_Objects.GetBehaviorType(), isExpensiveHeal) : 0);
		GameData.Domains.Character.Character character = (needPay ? DomainManager.Character.GetElement_Objects(payerId) : null);
		if (num2 > 0 && character != null && character.GetResource(6) < num2)
		{
			Logger.Warn($"Money not enough. need {num2}, has {character.GetResource(6)}");
			return false;
		}
		if (isExpensiveHeal)
		{
			GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(doctorId);
			Settlement settlement = DomainManager.Organization.GetSettlement(element_Objects3.GetOrganizationInfo().SettlementId);
			if (settlement == null)
			{
				Logger.Warn($"DoctorId:{doctorId}, Settlement  is null");
				return false;
			}
			int num3 = element_Objects2.CalcHealCostSpiritualDebt(type);
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, settlement.GetLocation().AreaId, -num3);
		}
		if (DomainManager.World.GetLeftDaysInCurrMonth() == 0)
		{
			Logger.Warn("Time not enough to heal");
			return false;
		}
		DomainManager.Character.UseCombatResources(context, doctorId, type, 1);
		if (num > 0)
		{
			element_Objects.ChangeResource(context, 5, -num);
		}
		if (num2 > 0 && character != null)
		{
			character.ChangeResource(context, 6, -num2);
			element_Objects.ChangeResource(context, 6, num2);
		}
		DomainManager.World.AdvanceDaysInMonth(context, 1);
		element_Objects.DoHealAction(context, type, element_Objects2, doctorId == DomainManager.Taiwu.GetTaiwuCharId(), isExpensiveHeal);
		return true;
	}

	public void UpdateIsTaiwuInFulongFlameArea(DataContext context)
	{
		if (!IsLocationInFulongFlameArea(DomainManager.Taiwu.GetTaiwu().GetLocation()))
		{
			if (_isTaiwuInFulongFlameArea)
			{
				_isTaiwuInFulongFlameArea = false;
				SetIsTaiwuInFulongFlameArea(_isTaiwuInFulongFlameArea, context);
			}
		}
		else if (!_isTaiwuInFulongFlameArea)
		{
			_canTriggerFulongFlameTeammateBubble = true;
			_isTaiwuInFulongFlameArea = true;
			SetIsTaiwuInFulongFlameArea(_isTaiwuInFulongFlameArea, context);
		}
	}

	public void SetTeleportMove(bool teleport)
	{
		_teleportMove = teleport;
	}

	public int GetTaiwuViewRange(MapBlockData blockData)
	{
		int num = blockData.GetConfig().ViewRange;
		if (DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(36))
		{
			num += DomainManager.Extra.GetProfessionData(11).GetSeniorityVisionRangeBonus();
		}
		return num;
	}

	public void SetBlockAndViewRangeVisibleByMove(DataContext context, MapBlockData blockData)
	{
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		list.Clear();
		Span<MapBlockData> areaBlocks = GetAreaBlocks(blockData.AreaId);
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			if (!mapBlockData.Visible)
			{
				list.Add(mapBlockData);
			}
		}
		int taiwuViewRange = GetTaiwuViewRange(blockData);
		SetBlockAndNeighborVisible(context, blockData, taiwuViewRange);
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(11);
		TravelerSkillsData skillsData = professionData.GetSkillsData<TravelerSkillsData>();
		bool flag = false;
		foreach (MapBlockData item in list)
		{
			if (item.Visible)
			{
				int num = skillsData.RecordExploredMapBlock(item.GetConfig().MoveCost * 10);
				if (num > 0)
				{
					DomainManager.Extra.ChangeProfessionSeniority(context, 11, num);
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			DomainManager.Extra.SetProfessionData(context, professionData);
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}

	public void SetBlockAndViewRangeVisible(DataContext context, short areaId, short blockId)
	{
		MapBlockData block = GetBlock(areaId, blockId);
		SetBlockAndNeighborVisible(context, block, block.GetConfig().ViewRange);
	}

	public void SetBlockAndNeighborVisible(DataContext context, MapBlockData block, int range)
	{
		if (!block.Visible)
		{
			block.SetVisible(visible: true, context);
		}
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		GetNeighborBlocks(block.AreaId, block.BlockId, list, range);
		for (int i = 0; i < list.Count; i++)
		{
			MapBlockData mapBlockData = list[i];
			if (!mapBlockData.Visible)
			{
				mapBlockData.SetVisible(visible: true, context);
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}

	[DomainMethod]
	public TeammateBubbleCollection GetTeammateBubbleCollection(DataContext context, bool isTraveling)
	{
		TeammateBubbleCollection collection;
		return (DomainManager.Adventure.GetCurAdventureId() < 0 && TryUpdateTeammateTypes() && TryGetTeammateBubbleCollection(context, isTraveling, out collection)) ? collection : null;
	}

	public bool IsTeammateBubbleAbleToDisplayByTeammateTypes(short templateId)
	{
		return (_availableBubbleCache[templateId] & _teammateTypes) != 0;
	}

	public (int charId, int index, int subtype) GetSelectedBubbleBestMatchTeammate(short templateId)
	{
		(int, int, int) result = (-1, -1, -1);
		sbyte personalityType = Config.TeammateBubble.Instance[templateId].PersonalityType;
		int num = -1;
		int num2 = -1;
		_teammateHighestPriorityText.Clear();
		foreach (var teammate in _teammates)
		{
			if (!IsTeammateAbleToDisplayBubble(teammate.charId))
			{
				continue;
			}
			for (int i = 0; i < 12; i++)
			{
				int num3 = 1 << i;
				if ((num3 & _availableBubbleCache[templateId]) != 0 && (num3 & teammate.subtype) != 0)
				{
					_teammateHighestPriorityText.Add((teammate.charId, teammate.index, i));
				}
			}
		}
		foreach (var item in _teammateHighestPriorityText)
		{
			int priority = TeammateBubbleSubType.GetPriority(item.subtype);
			sbyte personality = DomainManager.Character.GetElement_Objects(item.charId).GetPersonality(personalityType);
			if (!GameData.Domains.Character.Character.IsCharacterIdValid(result.Item1) || num < priority || (num == priority && num2 < personality))
			{
				result = item;
				num2 = personality;
				num = priority;
			}
		}
		return result;
	}

	public bool TryUpdateTeammateTypes()
	{
		if (GetTeammateCount() == 0)
		{
			return false;
		}
		if (_lastTaiwuLocation == DomainManager.Taiwu.GetTaiwu().GetValidLocation())
		{
			return false;
		}
		_teammateTypes = 0;
		foreach (var teammate in _teammates)
		{
			if (IsTeammateAbleToDisplayBubble(teammate.charId))
			{
				_teammateTypes |= teammate.subtype;
			}
		}
		return true;
	}

	public bool TryGetTeammateBubbleCollection(DataContext context, bool isTraveling, out TeammateBubbleCollection collection)
	{
		Location validLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(validLocation);
		MapBlockData block = DomainManager.Map.GetBlock(validLocation);
		AdventureSiteData adventureSite = DomainManager.Adventure.GetAdventureSite(validLocation.AreaId, validLocation.BlockId);
		short num = (short)((adventureSite != null && adventureSite.SiteState == 1) ? adventureSite.TemplateId : (-1));
		collection = new TeammateBubbleCollection();
		if (isTraveling)
		{
			return TryGetTravelingBubble(validLocation.AreaId, collection);
		}
		for (ETeammateBubbleBubbleElementType eTeammateBubbleBubbleElementType = ETeammateBubbleBubbleElementType.Lost; eTeammateBubbleBubbleElementType < ETeammateBubbleBubbleElementType.Count; eTeammateBubbleBubbleElementType++)
		{
			switch (eTeammateBubbleBubbleElementType)
			{
			case ETeammateBubbleBubbleElementType.Lost:
				if (TryGetDreamBackBubble(validLocation, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Story:
				if (TryGetStoryBubble(num, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.StoryMapblockEffect:
				if (TryGetStoryMapBlockEffectBubble(validLocation, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Queerbook:
				if (TryGetLegendaryBookBubble(num, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Chicken:
				if (TryGetChickenBubble(settlementByLocation, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.SectCombatMatch:
				if (TryGetSectCombatMatchBubble(num, block.TemplateId, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Treasure:
				if (TryGetMaterialResourceBubble(num, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.RelatedCharacter:
				if (TryGetRelatedCharacterBubble(block, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Infected:
				if (TryGetInfectedCharacterBubble(block, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.LegendaryBookInsane:
				if (TryGetLegendaryBookInsaneCharacterBubble(block, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Grave:
				if (TryGetNonEnemyGraveBubble(block, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.SectLeader:
				if (TryGetSectLeaderBubble(block, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.DestroyedArea:
				if (TryGetBrokenAreaBubble(validLocation.AreaId, block.TemplateId, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Animal:
				if (TryGetAnimalCharacterBubble(validLocation, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Caravan:
				if (TryGetCaravanCharacterBubble(collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.SwordGrave:
				if (TryGetSwordTombBubble(num, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.SettlementAdventure:
				if (TryGetSettlementAdventureBubble(num, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.TaiwuVillage:
				if (TryGetTaiwuVillageBubble(block.TemplateId, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Organization:
				if (TryGetOrganizationBubble(block.TemplateId, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.City:
				if (TryGetCityBubble(block.TemplateId, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Village:
				if (TryGetVillageBubble(block.TemplateId, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.EnemyNest:
				if (TryGetEnemyNestBubble(num, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Worker:
				if (TryGetWorkerBubble(validLocation, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.Cricket:
				if (TryGetCricketBubble(context, validLocation, collection))
				{
					return true;
				}
				break;
			case ETeammateBubbleBubbleElementType.SummerCombatMatch:
				if (TryGetCombatMatchBubble(num, collection))
				{
					return true;
				}
				break;
			}
		}
		return false;
	}

	private void InitializeTeammateBubble()
	{
		_lastTaiwuLocation = Location.Invalid;
		_availableBubbleCache.Clear();
		_elementTypeBubbleCache.Clear();
		foreach (TeammateBubbleItem item in (IEnumerable<TeammateBubbleItem>)Config.TeammateBubble.Instance)
		{
			if (!_elementTypeBubbleCache.ContainsKey(item.BubbleElementType))
			{
				_elementTypeBubbleCache.Add(item.BubbleElementType, new HashSet<short>());
			}
			_elementTypeBubbleCache[item.BubbleElementType].Add(item.TemplateId);
			_availableBubbleCache[item.TemplateId] = 0;
			if (!string.IsNullOrEmpty(item.SpecialDesc0))
			{
				_availableBubbleCache[item.TemplateId] |= 1;
			}
			if (!string.IsNullOrEmpty(item.SpecialDesc1))
			{
				_availableBubbleCache[item.TemplateId] |= 2;
			}
			if (!string.IsNullOrEmpty(item.SpecialDesc2))
			{
				_availableBubbleCache[item.TemplateId] |= 4;
			}
			if (!string.IsNullOrEmpty(item.SpecialDesc3))
			{
				_availableBubbleCache[item.TemplateId] |= 8;
			}
			if (!string.IsNullOrEmpty(item.SpecialDesc4))
			{
				_availableBubbleCache[item.TemplateId] |= 16;
			}
			if (!string.IsNullOrEmpty(item.FamilyDesc))
			{
				_availableBubbleCache[item.TemplateId] |= 32;
			}
			if (!string.IsNullOrEmpty(item.FriendDesc))
			{
				_availableBubbleCache[item.TemplateId] |= 64;
			}
			if (!string.IsNullOrEmpty(item.BehaviorDesc[0]))
			{
				_availableBubbleCache[item.TemplateId] |= 128;
			}
			if (!string.IsNullOrEmpty(item.BehaviorDesc[1]))
			{
				_availableBubbleCache[item.TemplateId] |= 256;
			}
			if (!string.IsNullOrEmpty(item.BehaviorDesc[2]))
			{
				_availableBubbleCache[item.TemplateId] |= 512;
			}
			if (!string.IsNullOrEmpty(item.BehaviorDesc[3]))
			{
				_availableBubbleCache[item.TemplateId] |= 1024;
			}
			if (!string.IsNullOrEmpty(item.BehaviorDesc[4]))
			{
				_availableBubbleCache[item.TemplateId] |= 2048;
			}
		}
		UpdateTeammateBubbleData();
		for (int i = 0; i < 3; i++)
		{
			GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(new DataUid(5, 35, (ulong)i), "UpdateTeammateBubbleData", UpdateTeammateBubbleData);
		}
	}

	private void UpdateTeammateBubbleData(DataContext context, DataUid uid)
	{
		UpdateTeammateBubbleData();
	}

	public void UpdateTeammateBubbleData()
	{
		_teammates.Clear();
		CharacterSet groupCharIds = DomainManager.Taiwu.GetGroupCharIds();
		for (int i = 0; i < 3; i++)
		{
			int element_CombatGroupCharIds = DomainManager.Taiwu.GetElement_CombatGroupCharIds(i);
			if (element_CombatGroupCharIds >= 0 && groupCharIds.Contains(element_CombatGroupCharIds))
			{
				_teammates.Add((element_CombatGroupCharIds, i, GetTeammateType(element_CombatGroupCharIds)));
			}
		}
	}

	private int GetTeammateHighestPriority(int charId)
	{
		int num = 0;
		int num2 = 0;
		foreach (var teammate in _teammates)
		{
			if (teammate.charId == charId)
			{
				num2 = teammate.subtype;
				break;
			}
		}
		for (int i = 0; i < 12; i++)
		{
			if ((num2 & (1 << i)) != 0)
			{
				num = Math.Max(num, TeammateBubbleSubType.GetPriority(i));
			}
		}
		return num;
	}

	private int GetTeammateType(int charId)
	{
		int num = 0;
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		if (element_Objects.GetFeatureIds().Contains(554))
		{
			num |= 1;
		}
		short templateId = element_Objects.GetTemplateId();
		if (templateId == 464)
		{
			num |= 2;
		}
		if (templateId == 463)
		{
			num |= 4;
		}
		if (templateId == 466)
		{
			num |= 8;
		}
		if (templateId == 465)
		{
			num |= 0x10;
		}
		if (!DomainManager.Character.TryGetRelation(charId, DomainManager.Taiwu.GetTaiwuCharId(), out var relation))
		{
			return 0;
		}
		ushort relationType = relation.RelationType;
		if (RelationType.IsFamilyRelation(relationType))
		{
			num |= 0x20;
		}
		if (RelationType.IsFriendRelation(relationType))
		{
			num |= 0x40;
		}
		switch (element_Objects.GetBehaviorType())
		{
		case 0:
			num |= 0x80;
			break;
		case 1:
			num |= 0x100;
			break;
		case 2:
			num |= 0x200;
			break;
		case 3:
			num |= 0x400;
			break;
		case 4:
			num |= 0x800;
			break;
		}
		return num;
	}

	private int GetTeammateCount()
	{
		return _teammates.Count;
	}

	private bool IsTeammateAbleToDisplayBubble(int charId)
	{
		return GetTeammateCount() == 1 || _lastTeammateBubble.charId != charId || _lastTeammateBubble.count < 2;
	}

	private void SetLastTeammateBubble(int charId)
	{
		_lastTeammateBubble = ((_lastTeammateBubble.charId == charId) ? (charId: charId, count: _lastTeammateBubble.count + 1) : (charId: charId, count: 1));
		_lastTaiwuLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
	}

	private void ApplyTeammateBubble(short templateId, TeammateBubbleCollection collection)
	{
		(int, int, int) selectedBubbleBestMatchTeammate = GetSelectedBubbleBestMatchTeammate(templateId);
		collection.AddNoneParameterBubble(Config.TeammateBubble.Instance[templateId], selectedBubbleBestMatchTeammate.Item2, selectedBubbleBestMatchTeammate.Item3);
		SetLastTeammateBubble(selectedBubbleBestMatchTeammate.Item1);
	}

	private void ApplyAdventureTeammateBubble(short templateId, TeammateBubbleCollection collection, short adventureId)
	{
		(int, int, int) selectedBubbleBestMatchTeammate = GetSelectedBubbleBestMatchTeammate(templateId);
		collection.AddAdventureParameterBubble(Config.TeammateBubble.Instance[templateId], selectedBubbleBestMatchTeammate.Item2, selectedBubbleBestMatchTeammate.Item3, adventureId);
		SetLastTeammateBubble(selectedBubbleBestMatchTeammate.Item1);
	}

	private void ApplyCombatMatchTeammateBubble(short templateId, TeammateBubbleCollection collection, sbyte type)
	{
		(int, int, int) selectedBubbleBestMatchTeammate = GetSelectedBubbleBestMatchTeammate(templateId);
		collection.AddSingleCombatSkillTypeParameterBubble(Config.TeammateBubble.Instance[templateId], selectedBubbleBestMatchTeammate.Item2, selectedBubbleBestMatchTeammate.Item3, type);
		SetLastTeammateBubble(selectedBubbleBestMatchTeammate.Item1);
	}

	private void ApplyIdentityTeammateBubble(short templateId, TeammateBubbleCollection collection, int type)
	{
		(int, int, int) selectedBubbleBestMatchTeammate = GetSelectedBubbleBestMatchTeammate(templateId);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(selectedBubbleBestMatchTeammate.Item1);
		OrganizationInfo organizationInfo = element_Objects.GetOrganizationInfo();
		sbyte gender = element_Objects.GetGender();
		collection.AddCharacterIdentityBubble(Config.TeammateBubble.Instance[templateId], selectedBubbleBestMatchTeammate.Item2, selectedBubbleBestMatchTeammate.Item3, type, organizationInfo, gender);
		SetLastTeammateBubble(selectedBubbleBestMatchTeammate.Item1);
	}

	private bool TryGetBrokenAreaBubble(short areaId, short mapBlockTemplateId, TeammateBubbleCollection collection)
	{
		if (MapAreaData.IsBrokenArea(areaId) && mapBlockTemplateId == 38 && IsTeammateBubbleAbleToDisplayByTeammateTypes(187))
		{
			ApplyTeammateBubble(187, collection);
			return true;
		}
		return false;
	}

	private bool TryGetTravelingBubble(short areaId, TeammateBubbleCollection collection)
	{
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(areaId);
		if (_lastTaiwuLocation != Location.Invalid && DomainManager.Map.GetStateTemplateIdByAreaId(_lastTaiwuLocation.AreaId) == stateTemplateIdByAreaId)
		{
			return false;
		}
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Traveling])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (teammateBubbleItem.MapStateTemplateId == stateTemplateIdByAreaId)
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetDreamBackBubble(Location location, TeammateBubbleCollection collection)
	{
		List<DreamBackLocationData> dreamBackLocationData = DomainManager.Extra.GetDreamBackLocationData();
		if (dreamBackLocationData == null || dreamBackLocationData.Count == 0)
		{
			return false;
		}
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Lost])
		{
			if (!IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				continue;
			}
			foreach (DreamBackLocationData item2 in dreamBackLocationData)
			{
				if (item2.Location == location)
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetStoryBubble(short adventureTemplateId, TeammateBubbleCollection collection)
	{
		if (adventureTemplateId < 0)
		{
			return false;
		}
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Story])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (teammateBubbleItem.AdventureTemplateIdList != null && teammateBubbleItem.AdventureTemplateIdList.Contains(adventureTemplateId))
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetStoryMapBlockEffectBubble(Location location, TeammateBubbleCollection collection)
	{
		if (_canTriggerFulongFlameTeammateBubble)
		{
			ApplyTeammateBubble(188, collection);
			_canTriggerFulongFlameTeammateBubble = false;
			return true;
		}
		return false;
	}

	private bool TryGetLegendaryBookBubble(short adventureId, TeammateBubbleCollection collection)
	{
		if (adventureId < 0)
		{
			return false;
		}
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Queerbook])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (teammateBubbleItem.AdventureTemplateIdList != null && teammateBubbleItem.AdventureTemplateIdList.Contains(adventureId))
				{
					ApplyAdventureTeammateBubble(item, collection, adventureId);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetChickenBubble(Settlement settlement, TeammateBubbleCollection collection)
	{
		if (settlement == null)
		{
			return false;
		}
		if (DomainManager.Building.TryGetFirstChickenInSettlement(settlement, out var _))
		{
			foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Chicken])
			{
				if (!IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
				{
					continue;
				}
				ApplyTeammateBubble(item, collection);
				return true;
			}
		}
		return false;
	}

	private bool TryGetSectCombatMatchBubble(short adventureId, short mapBlockTemplateId, TeammateBubbleCollection collection)
	{
		if (adventureId != 26)
		{
			return false;
		}
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.SectCombatMatch])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (mapBlockTemplateId == teammateBubbleItem.MapBlockTemplateId)
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetMaterialResourceBubble(short adventureId, TeammateBubbleCollection collection)
	{
		if (adventureId < 0)
		{
			return false;
		}
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Treasure])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (teammateBubbleItem.AdventureTemplateIdList != null && teammateBubbleItem.AdventureTemplateIdList.Contains(adventureId))
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetSwordTombBubble(short adventureTemplateId, TeammateBubbleCollection collection)
	{
		if (adventureTemplateId < 0)
		{
			return false;
		}
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.SwordGrave])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (teammateBubbleItem.AdventureTemplateIdList != null && teammateBubbleItem.AdventureTemplateIdList.Contains(adventureTemplateId))
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetSettlementAdventureBubble(short adventureTemplateId, TeammateBubbleCollection collection)
	{
		if (adventureTemplateId < 0)
		{
			return false;
		}
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.SettlementAdventure])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (teammateBubbleItem.AdventureTemplateIdList != null && teammateBubbleItem.AdventureTemplateIdList.Contains(adventureTemplateId))
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetCombatMatchBubble(short adventureTemplateId, TeammateBubbleCollection collection)
	{
		if (adventureTemplateId < 0)
		{
			return false;
		}
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.SummerCombatMatch])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (teammateBubbleItem == null)
				{
					return false;
				}
				if (teammateBubbleItem.AdventureTemplateIdList != null && teammateBubbleItem.AdventureTemplateIdList.Contains(adventureTemplateId))
				{
					ApplyCombatMatchTeammateBubble(item, collection, _combatMatchIdToCombatSkillType[adventureTemplateId]);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetTaiwuVillageBubble(short mapBlockTemplateId, TeammateBubbleCollection collection)
	{
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.TaiwuVillage])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (mapBlockTemplateId == teammateBubbleItem.MapBlockTemplateId)
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetOrganizationBubble(short mapBlockTemplateId, TeammateBubbleCollection collection)
	{
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Organization])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (mapBlockTemplateId == teammateBubbleItem.MapBlockTemplateId)
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetCityBubble(short mapBlockTemplateId, TeammateBubbleCollection collection)
	{
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.City])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (mapBlockTemplateId == teammateBubbleItem.MapBlockTemplateId)
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetVillageBubble(short mapBlockTemplateId, TeammateBubbleCollection collection)
	{
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Village])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (mapBlockTemplateId == teammateBubbleItem.MapBlockTemplateId)
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetEnemyNestBubble(short adventureId, TeammateBubbleCollection collection)
	{
		if (adventureId < 0)
		{
			return false;
		}
		foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.EnemyNest])
		{
			if (IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
			{
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item];
				if (teammateBubbleItem.AdventureTemplateIdList != null && teammateBubbleItem.AdventureTemplateIdList.Contains(adventureId))
				{
					ApplyTeammateBubble(item, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetWorkerBubble(Location location, TeammateBubbleCollection collection)
	{
		if (DomainManager.Taiwu.TryGetElement_VillagerWorkLocations(location, out var _))
		{
			foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Worker])
			{
				if (!IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
				{
					continue;
				}
				ApplyTeammateBubble(item, collection);
				return true;
			}
		}
		return false;
	}

	private bool TryGetCricketBubble(DataContext context, Location location, TeammateBubbleCollection collection)
	{
		if (DomainManager.Map.LocationHasCricket(context, location))
		{
			foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Cricket])
			{
				if (!IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
				{
					continue;
				}
				ApplyTeammateBubble(item, collection);
				return true;
			}
		}
		return false;
	}

	private bool TryGetAnimalCharacterBubble(Location location, TeammateBubbleCollection collection)
	{
		if (DomainManager.Extra.TryGetAnimalIdsByLocation(location, out var animals) && animals.Count > 0 && DomainManager.Extra.TryGetAnimal(animals[0], out var animal))
		{
			short templateId = (short)((animal.ItemKey == ItemKey.Invalid) ? 155 : 156);
			if (!IsTeammateBubbleAbleToDisplayByTeammateTypes(templateId))
			{
				return false;
			}
			ApplyTeammateBubble(templateId, collection);
			return true;
		}
		return false;
	}

	private bool TryGetCaravanCharacterBubble(TeammateBubbleCollection collection)
	{
		if (DomainManager.Merchant.TryGetFirstTaiwuLocationCaravanId(out var _))
		{
			foreach (short item in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Caravan])
			{
				if (!IsTeammateBubbleAbleToDisplayByTeammateTypes(item))
				{
					continue;
				}
				ApplyTeammateBubble(item, collection);
				return true;
			}
		}
		return false;
	}

	private bool TryGetRelatedCharacterBubble(MapBlockData block, TeammateBubbleCollection collection)
	{
		if (block.CharacterSet == null || block.CharacterSet.Count == 0)
		{
			return false;
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		foreach (int item in block.CharacterSet)
		{
			if (!DomainManager.Character.TryGetRelation(item, taiwuCharId, out var relation))
			{
				continue;
			}
			ushort relationType = relation.RelationType;
			if (RelationType.IsFamilyRelation(relationType))
			{
				if (IsTeammateBubbleAbleToDisplayByTeammateTypes(158))
				{
					ApplyTeammateBubble(158, collection);
					return true;
				}
			}
			else if (RelationType.IsFriendRelation(relationType))
			{
				if (IsTeammateBubbleAbleToDisplayByTeammateTypes(159))
				{
					ApplyTeammateBubble(159, collection);
					return true;
				}
			}
			else if ((0x8000 & relationType) != 0 && IsTeammateBubbleAbleToDisplayByTeammateTypes(160))
			{
				ApplyTeammateBubble(160, collection);
				return true;
			}
		}
		return false;
	}

	private bool TryGetInfectedCharacterBubble(MapBlockData block, TeammateBubbleCollection collection)
	{
		if (block.InfectedCharacterSet == null || block.InfectedCharacterSet.Count == 0)
		{
			return false;
		}
		foreach (int item in block.InfectedCharacterSet)
		{
			if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				continue;
			}
			List<short> featureIds = element.GetFeatureIds();
			foreach (short item2 in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.Infected])
			{
				if (!IsTeammateBubbleAbleToDisplayByTeammateTypes(item2))
				{
					continue;
				}
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item2];
				if (teammateBubbleItem.CharacterFeatureTemplateIdList == null)
				{
					continue;
				}
				foreach (short characterFeatureTemplateId in teammateBubbleItem.CharacterFeatureTemplateIdList)
				{
					if (featureIds.Contains(characterFeatureTemplateId))
					{
						ApplyTeammateBubble(item2, collection);
						return true;
					}
				}
			}
		}
		return false;
	}

	private bool TryGetLegendaryBookInsaneCharacterBubble(MapBlockData block, TeammateBubbleCollection collection)
	{
		if (block.InfectedCharacterSet == null || block.InfectedCharacterSet.Count == 0)
		{
			return false;
		}
		foreach (int item in block.InfectedCharacterSet)
		{
			if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				continue;
			}
			List<sbyte> charOwnedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(item);
			if (charOwnedBookTypes == null)
			{
				continue;
			}
			List<short> featureIds = element.GetFeatureIds();
			if (featureIds.Contains(204))
			{
				if (IsTeammateBubbleAbleToDisplayByTeammateTypes(165))
				{
					ApplyTeammateBubble(165, collection);
					return true;
				}
			}
			else if (featureIds.Contains(205) && IsTeammateBubbleAbleToDisplayByTeammateTypes(166))
			{
				ApplyTeammateBubble(166, collection);
				return true;
			}
		}
		return false;
	}

	private bool TryGetNonEnemyGraveBubble(MapBlockData block, TeammateBubbleCollection collection)
	{
		if (block.GraveSet == null || block.GraveSet.Count == 0)
		{
			return false;
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		foreach (int item in block.GraveSet)
		{
			if (DomainManager.Character.TryGetRelation(item, taiwuCharId, out var relation))
			{
				ushort relationType = relation.RelationType;
				if ((0x8000 & relationType) == 0 && IsTeammateBubbleAbleToDisplayByTeammateTypes(167))
				{
					ApplyTeammateBubble(167, collection);
					return true;
				}
			}
		}
		return false;
	}

	private bool TryGetSectLeaderBubble(MapBlockData block, TeammateBubbleCollection collection)
	{
		if (block.CharacterSet == null || block.CharacterSet.Count == 0)
		{
			return false;
		}
		foreach (int item in block.CharacterSet)
		{
			if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				continue;
			}
			List<short> featureIds = element.GetFeatureIds();
			foreach (short item2 in _elementTypeBubbleCache[ETeammateBubbleBubbleElementType.SectLeader])
			{
				if (!IsTeammateBubbleAbleToDisplayByTeammateTypes(item2))
				{
					continue;
				}
				TeammateBubbleItem teammateBubbleItem = Config.TeammateBubble.Instance[item2];
				if (teammateBubbleItem.CharacterFeatureTemplateIdList == null)
				{
					continue;
				}
				foreach (short characterFeatureTemplateId in teammateBubbleItem.CharacterFeatureTemplateIdList)
				{
					if (featureIds.Contains(characterFeatureTemplateId))
					{
						ApplyIdentityTeammateBubble(item2, collection, item);
						return true;
					}
				}
			}
		}
		return false;
	}

	public static sbyte GetSectOrgTemplateIdByStateTemplateId(sbyte mapStateTemplateId)
	{
		return MapState.Instance[mapStateTemplateId].SectID;
	}

	public static short GetCharacterTemplateId(sbyte mapStateTemplateId, sbyte gender)
	{
		return MapState.Instance[mapStateTemplateId].TemplateCharacterIds[gender];
	}

	private void InitializeTravelMap()
	{
		_dijkstraMap.Initialize(GetAllAreaIds(), GetAreaNeighbors);
	}

	[DomainMethod]
	public void UnlockStation(DataContext context, short areaId, bool costAuthority = true)
	{
		MapAreaData mapAreaData = _areas[areaId];
		if (areaId >= 135)
		{
			throw new Exception($"Invalid area id {areaId}");
		}
		if (mapAreaData.StationUnlocked)
		{
			throw new Exception($"Station of area {areaId} already unlocked");
		}
		if (costAuthority)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int num = 0;
			for (short num2 = 0; num2 < 135; num2++)
			{
				if (_areas[num2].StationUnlocked)
				{
					num++;
				}
			}
			int num3 = Math.Max(GlobalConfig.Instance.MapAreaOpenPrestige * (num - 9 * GlobalConfig.Instance.MapInitUnlockStationStateCount + 1), 0);
			if (taiwu.GetResource(7) < num3)
			{
				throw new Exception($"Authority not enough to unlock station at area {areaId}, need {num3}, have {taiwu.GetResource(7)}");
			}
			taiwu.ChangeResource(context, 7, -num3);
			DomainManager.Taiwu.AddLegacyPoint(context, 37);
			AddUnlockStationSeniority(context, num3);
		}
		mapAreaData.Discovered = true;
		mapAreaData.StationUnlocked = true;
		SetElement_Areas(areaId, mapAreaData, context);
		foreach (short neighborArea in mapAreaData.NeighborAreas)
		{
			MapAreaData mapAreaData2 = _areas[neighborArea];
			if (!mapAreaData2.Discovered)
			{
				mapAreaData2.Discovered = true;
				SetElement_Areas(neighborArea, mapAreaData2, context);
			}
		}
	}

	[DomainMethod]
	public void QuickTravel(DataContext context, short destAreaId)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short pureTravelBlockId = GetPureTravelBlockId(context, destAreaId, taiwu.GetLocation().AreaId);
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			element_Objects.SetLocation(new Location(destAreaId, pureTravelBlockId), context);
		}
		DomainManager.Extra.GearMateFollowTaiwu(context);
	}

	[DomainMethod]
	public CrossAreaMoveInfo GetTravelCost(short fromAreaId, short fromBlockId, short toAreaId)
	{
		sbyte stateTemplateIdByAreaId = GetStateTemplateIdByAreaId(fromAreaId);
		CrossAreaMoveInfo crossAreaMoveInfo = new CrossAreaMoveInfo
		{
			FromAreaId = fromAreaId,
			FromBlockId = fromBlockId,
			ToAreaId = toAreaId
		};
		if (fromAreaId == toAreaId)
		{
			return crossAreaMoveInfo;
		}
		_carrierReduceTravelCostDaysPercent = CalcCarrierReduceTravelCostDaysPercent();
		if (toAreaId == 137 || fromAreaId == 137 || toAreaId == 138 || fromAreaId == 138)
		{
			TravelRoute travelRoute = new TravelRoute();
			travelRoute.AreaList.Add(fromAreaId);
			travelRoute.AreaList.Add(toAreaId);
			sbyte[] worldMapPos = _areas[fromAreaId].GetConfig().WorldMapPos;
			travelRoute.PosList.Add(new ByteCoordinate((byte)worldMapPos[0], (byte)worldMapPos[1]));
			travelRoute.PosList.Add(new ByteCoordinate(0, 0));
			travelRoute.CostList.Add(0);
			travelRoute.CostList.Add(0);
			crossAreaMoveInfo.Route = travelRoute;
		}
		else
		{
			IReadOnlyList<IReadonlyDijkstraNode<short>> readOnlyList = ((DijkstraAlgorithm<short>)_dijkstraMap).FindShortestPath(fromAreaId, toAreaId);
			List<ByteCoordinate> list = ObjectPool<List<ByteCoordinate>>.Instance.Get();
			TravelRoute travelRoute2 = (crossAreaMoveInfo.Route = new TravelRoute());
			travelRoute2.AreaList.EnsureCapacity(readOnlyList.Count);
			travelRoute2.CostList.EnsureCapacity(readOnlyList.Count);
			int num = 0;
			foreach (IReadonlyDijkstraNode<short> item in readOnlyList)
			{
				if (item.Pos == fromAreaId)
				{
					continue;
				}
				travelRoute2.AreaList.Add(item.Pos);
				travelRoute2.CostList.Add((short)(item.Cost / 1000 - num));
				num = item.Cost / 1000;
				MapAreaItem config = _areas[item.Last].GetConfig();
				MapAreaItem config2 = _areas[item.Pos].GetConfig();
				list.Clear();
				if (config2.TemplateId > config.TemplateId)
				{
					AreaTravelRoute[] neighborAreas = config.NeighborAreas;
					for (int i = 0; i < neighborAreas.Length; i++)
					{
						AreaTravelRoute areaTravelRoute = neighborAreas[i];
						if (areaTravelRoute.DestAreaId == config2.TemplateId)
						{
							list.AddRange(areaTravelRoute.MapPosList);
							break;
						}
					}
				}
				else
				{
					AreaTravelRoute[] neighborAreas2 = config2.NeighborAreas;
					for (int j = 0; j < neighborAreas2.Length; j++)
					{
						AreaTravelRoute areaTravelRoute2 = neighborAreas2[j];
						if (areaTravelRoute2.DestAreaId == config.TemplateId)
						{
							list.AddRange(areaTravelRoute2.MapPosList);
							list.Reverse();
							break;
						}
					}
				}
				travelRoute2.PosList.AddRange(list);
				if (item.Pos != toAreaId)
				{
					travelRoute2.PosList.Add(new ByteCoordinate((byte)config2.WorldMapPos[0], (byte)config2.WorldMapPos[1]));
				}
			}
			ObjectPool<List<ByteCoordinate>>.Instance.Return(list);
		}
		crossAreaMoveInfo.MoneyCost = crossAreaMoveInfo.Route.GetTotalTimeCost() * MapState.Instance[stateTemplateIdByAreaId].TravalMoney * 5;
		int num2 = 0;
		for (short num3 = 0; num3 < 135; num3++)
		{
			if (_areas[num3].StationUnlocked)
			{
				num2++;
			}
		}
		crossAreaMoveInfo.AuthorityCost = 0;
		for (int k = 0; k < crossAreaMoveInfo.Route.AreaList.Count; k++)
		{
			short num4 = crossAreaMoveInfo.Route.AreaList[k];
			if (!_areas[num4].StationUnlocked)
			{
				crossAreaMoveInfo.AuthorityCost += Math.Max(GlobalConfig.Instance.MapAreaOpenPrestige * (num2 - 9 * GlobalConfig.Instance.MapInitUnlockStationStateCount + 1), 0);
				num2++;
			}
		}
		if (fromAreaId == 135 || fromAreaId == 138 || fromAreaId == 137)
		{
			crossAreaMoveInfo.MoneyCost = 0;
			crossAreaMoveInfo.AuthorityCost = 0;
		}
		return crossAreaMoveInfo;
	}

	[DomainMethod]
	public TravelPreviewDisplayData GetTravelPreview(short toAreaId)
	{
		TravelPreviewDisplayData travelPreviewDisplayData = new TravelPreviewDisplayData
		{
			ToAreaId = -1
		};
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid() || toAreaId == location.AreaId || toAreaId < 0)
		{
			return travelPreviewDisplayData;
		}
		travelPreviewDisplayData.ToAreaId = toAreaId;
		CrossAreaMoveInfo travelCost = GetTravelCost(location.AreaId, location.BlockId, toAreaId);
		travelPreviewDisplayData.AuthorityCost = travelCost.AuthorityCost;
		travelPreviewDisplayData.MoneyCost = travelCost.MoneyCost;
		travelPreviewDisplayData.DaysCost = ((IEnumerable<short>)travelCost.Route.CostList).Select((Func<short, int>)((short x) => x)).Sum();
		travelPreviewDisplayData.CurrentAuthority = taiwu.GetResource(7);
		if (travelPreviewDisplayData.AuthorityCost > 0)
		{
			travelPreviewDisplayData.NeedUnlockStations = new List<short>();
			foreach (short area in travelCost.Route.AreaList)
			{
				if (!_areas[area].StationUnlocked)
				{
					travelPreviewDisplayData.NeedUnlockStations.Add(area);
				}
			}
		}
		return travelPreviewDisplayData;
	}

	[DomainMethod]
	public bool UnlockTravelPath(DataContext context, short toAreaId)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		CrossAreaMoveInfo travelCost = GetTravelCost(location.AreaId, location.BlockId, toAreaId);
		return travelCost.AuthorityCost > 0 && UnlockTravelPathInternal(context, travelCost);
	}

	[DomainMethod]
	public void StartTravel(DataContext context, short toAreaId)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		CrossAreaMoveInfo travelCost = GetTravelCost(location.AreaId, location.BlockId, toAreaId);
		travelCost.AutoCheckFreeTravel();
		if (!UnlockTravelPathInternal(context, travelCost))
		{
			throw new Exception($"Authority not enough to travel to area {travelCost.ToAreaId}, need {travelCost.AuthorityCost}, have {taiwu.GetResource(7)}");
		}
		if (travelCost.MoneyCost > 0)
		{
			taiwu.ChangeResource(context, 6, -travelCost.MoneyCost);
		}
		StartTravelWithoutCost(context, travelCost);
	}

	[DomainMethod]
	public void DirectTravelToTaiwuVillage(DataContext context)
	{
		StopTravel(context);
		short areaId = DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		CrossAreaMoveInfo travelCost = GetTravelCost(location.AreaId, location.BlockId, areaId);
		travelCost.AutoCheckFreeTravel();
		if (travelCost.MoneyCost > 0)
		{
			taiwu.ChangeResource(context, 6, -travelCost.MoneyCost);
		}
		DirectTravel(context, travelCost);
	}

	[DomainMethod]
	public bool ContinueTravel(DataContext context)
	{
		if (_travelInfo.Traveling && _travelInfo.CurrentAreaId == _travelInfo.ToAreaId)
		{
			FinishTravel(context, _travelInfo.ToAreaId);
			return false;
		}
		EnsureCostDays(context);
		int nextCostDays = _travelInfo.NextCostDays;
		if (nextCostDays == -1)
		{
			return false;
		}
		int totalActionPointsRemaining = DomainManager.Extra.GetTotalActionPointsRemaining();
		int num = nextCostDays * 10;
		int num2 = Math.Min(num, totalActionPointsRemaining);
		DomainManager.Extra.ConsumeActionPoint(context, num2);
		RecordTravelCostedDays(context, _travelInfo.CostedDays + num2 / 10);
		if (num2 < num)
		{
			DomainManager.World.AdvanceMonth(context);
			return false;
		}
		int routeIndex = _travelInfo.RouteIndex;
		short num3 = ((routeIndex == 0) ? _travelInfo.FromAreaId : _travelInfo.Route.AreaList[routeIndex - 1]);
		short num4 = _travelInfo.Route.AreaList[routeIndex];
		if (num3 < 135)
		{
			List<(Location, short)> list = CalcBlockTravelRoute(context.Random, new Location(num3, _areas[num3].StationBlockId), new Location(num4, _areas[num4].StationBlockId), isMerchant: false);
			for (int i = 0; i < list.Count; i++)
			{
				Location item = list[i].Item1;
				SetBlockAndViewRangeVisible(context, item.AreaId, item.BlockId);
			}
		}
		else if (num4 != 137 && num4 != 138)
		{
			SetBlockAndViewRangeVisible(context, num4, _areas[num4].StationBlockId);
		}
		return true;
	}

	[DomainMethod]
	public short ContinueTravelWithDetectTravelingEvent(DataContext context)
	{
		if (_travelInfo.Traveling && ContinueTravel(context))
		{
			return DetectTravelingEvent(context);
		}
		return -1;
	}

	[DomainMethod]
	public void RecordTravelCostedDays(DataContext context, int costedDays)
	{
		_travelInfo.CostedDays = costedDays;
		SetTravelInfo(_travelInfo, context);
	}

	[DomainMethod]
	public void StopTravel(DataContext context)
	{
		if (_travelInfo.Traveling)
		{
			FinishTravel(context, _travelInfo.CurrentAreaId);
		}
	}

	[DomainMethod]
	public void TaiwuBeKidnapped(DataContext context, Location targetLocation, int hunterCharId)
	{
		short areaId = targetLocation.AreaId;
		Tester.Assert(areaId >= 0 && areaId < 135);
		DomainManager.Map.StopTravel(context);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (taiwu.GetLocation().AreaId != targetLocation.AreaId)
		{
			KidnappedTravelData value = new KidnappedTravelData
			{
				Target = targetLocation,
				HunterCharId = hunterCharId
			};
			DomainManager.Extra.SetKidnappedTravelData(value, context);
			DirectTravel(context, targetLocation.AreaId);
		}
		else
		{
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OperateBlackMaskView, arg1: true, 1f, arg3: true);
			SetTeleportMove(teleport: true);
			Move(context, targetLocation.BlockId, notCostTime: true);
			SetTeleportMove(teleport: false);
			DomainManager.TaiwuEvent.OnEvent_TaiwuBeHuntedArrivedSect(hunterCharId);
		}
	}

	public void DirectTravel(DataContext context, short toAreaId)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		CrossAreaMoveInfo travelCost = GetTravelCost(location.AreaId, location.BlockId, toAreaId);
		DirectTravel(context, travelCost);
	}

	public void DirectTravel(DataContext context, CrossAreaMoveInfo moveInfo)
	{
		if (!DomainManager.Extra.GetIsDirectTraveling())
		{
			DomainManager.Extra.SetIsDirectTraveling(value: true, context);
		}
		StartTravelWithoutCost(context, moveInfo);
	}

	public void StartTravelWithoutCost(DataContext context, CrossAreaMoveInfo moveInfo)
	{
		if (_travelInfo.Traveling)
		{
			throw new Exception($"Last travel to area {_travelInfo.ToAreaId} is not finished");
		}
		SetTravelInfo(moveInfo, context);
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		Location invalid = Location.Invalid;
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			element_Objects.SetLocation(invalid, context);
		}
		DomainManager.Extra.GearMateFollowTaiwu(context);
	}

	public Location GetTravelCurrLocation()
	{
		if (_travelInfo.ToAreaId < 0)
		{
			throw new Exception("Taiwu is not travelling");
		}
		short currentAreaId = _travelInfo.CurrentAreaId;
		Location result = new Location(currentAreaId, _areas[currentAreaId].StationBlockId);
		if (!result.IsValid())
		{
			result.AreaId = _travelInfo.FromAreaId;
			result.BlockId = _travelInfo.FromBlockId;
		}
		return result;
	}

	internal bool UnlockTravelPathInternal(DataContext context, CrossAreaMoveInfo moveInfo)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (moveInfo.AuthorityCost <= 0)
		{
			return true;
		}
		if (taiwu.GetResource(7) < moveInfo.AuthorityCost)
		{
			return false;
		}
		taiwu.ChangeResource(context, 7, -moveInfo.AuthorityCost);
		AddUnlockStationSeniority(context, moveInfo.AuthorityCost);
		for (int i = 0; i < moveInfo.Route.AreaList.Count; i++)
		{
			short num = moveInfo.Route.AreaList[i];
			if (!_areas[num].StationUnlocked)
			{
				UnlockStation(context, num, costAuthority: false);
				DomainManager.Taiwu.AddLegacyPoint(context, 37);
			}
			foreach (short neighborArea in _areas[num].NeighborAreas)
			{
				MapAreaData mapAreaData = _areas[neighborArea];
				if (!mapAreaData.Discovered)
				{
					mapAreaData.Discovered = true;
					SetElement_Areas(neighborArea, mapAreaData, context);
				}
			}
		}
		return true;
	}

	private void EnsureCostDays(DataContext context)
	{
		if (!_travelInfo.Traveling)
		{
			return;
		}
		_carrierReduceTravelCostDaysPercent = CalcCarrierReduceTravelCostDaysPercent();
		int areaCostDays = GetAreaCostDays(_travelInfo.CurrentAreaId, _travelInfo.NextAreaId);
		if (areaCostDays < 0)
		{
			return;
		}
		short num = _travelInfo.Route.CostList[_travelInfo.RouteIndex + 1];
		if (num != areaCostDays)
		{
			int num2 = Math.Min(num - areaCostDays, _travelInfo.NextCostDays - 1);
			if (num2 != 0)
			{
				_travelInfo.Route.CostList[_travelInfo.RouteIndex + 1] -= (short)num2;
				SetTravelInfo(_travelInfo, context);
			}
		}
	}

	private IEnumerable<short> GetAllAreaIds()
	{
		for (short i = 0; i < _areas.Length; i++)
		{
			yield return i;
		}
	}

	private IEnumerable<(short area, int cost)> GetAreaNeighbors(short areaId)
	{
		MapAreaItem config = _areas[areaId].GetConfig();
		AreaTravelRoute[] neighborAreas = config.NeighborAreas;
		for (int i = 0; i < neighborAreas.Length; i++)
		{
			AreaTravelRoute neighborArea = neighborAreas[i];
			yield return (area: GetAreaIdByAreaTemplateId(neighborArea.DestAreaId), cost: GetAreaPathDays(neighborArea.CostDays));
		}
		foreach (MapAreaItem extraConfig in (IEnumerable<MapAreaItem>)MapArea.Instance)
		{
			AreaTravelRoute[] neighborAreas2 = extraConfig.NeighborAreas;
			for (int j = 0; j < neighborAreas2.Length; j++)
			{
				AreaTravelRoute neighborArea2 = neighborAreas2[j];
				if (neighborArea2.DestAreaId == config.TemplateId)
				{
					yield return (area: GetAreaIdByAreaTemplateId(extraConfig.TemplateId), cost: GetAreaPathDays(neighborArea2.CostDays));
				}
			}
		}
	}

	private int GetAreaCostDays(short areaA, short areaB)
	{
		if (areaA == areaB)
		{
			return -1;
		}
		MapAreaItem config = _areas[areaA].GetConfig();
		MapAreaItem config2 = _areas[areaB].GetConfig();
		AreaTravelRoute[] neighborAreas = config.NeighborAreas;
		for (int i = 0; i < neighborAreas.Length; i++)
		{
			AreaTravelRoute areaTravelRoute = neighborAreas[i];
			if (areaTravelRoute.DestAreaId == config2.TemplateId)
			{
				return GetAreaCostDays(areaTravelRoute.CostDays);
			}
		}
		AreaTravelRoute[] neighborAreas2 = config2.NeighborAreas;
		for (int j = 0; j < neighborAreas2.Length; j++)
		{
			AreaTravelRoute areaTravelRoute2 = neighborAreas2[j];
			if (areaTravelRoute2.DestAreaId == config.TemplateId)
			{
				return GetAreaCostDays(areaTravelRoute2.CostDays);
			}
		}
		return -1;
	}

	private int GetAreaPathDays(short configCostDays)
	{
		return Math.Max(GetAreaCostDays(configCostDays), 1) * 1000 + 1;
	}

	private int GetAreaCostDays(short configCostDays)
	{
		return configCostDays - configCostDays * _carrierReduceTravelCostDaysPercent / 100;
	}

	private int CalcCarrierReduceTravelCostDaysPercent()
	{
		KidnappedTravelData kidnappedTravelData = DomainManager.Extra.GetKidnappedTravelData();
		if (kidnappedTravelData.Valid)
		{
			return Config.Carrier.Instance[(short)26].BaseTravelTimeReduction;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		ItemKey itemKey = taiwu.GetEquipment()[11];
		if (!itemKey.IsValid())
		{
			return 0;
		}
		GameData.Domains.Item.Carrier element_Carriers = DomainManager.Item.GetElement_Carriers(itemKey.Id);
		return (element_Carriers.GetCurrDurability() > 0) ? element_Carriers.GetTravelTimeReduction() : 0;
	}

	private void FinishTravel(DataContext context, short destAreaId)
	{
		short blockId = ((destAreaId == _travelInfo.FromAreaId) ? _travelInfo.FromBlockId : GetPureTravelBlockId(context, destAreaId, _travelInfo.FromAreaId));
		Location location = new Location(destAreaId, blockId);
		if (DomainManager.Extra.GetIsDirectTraveling())
		{
			DomainManager.Extra.SetIsDirectTraveling(value: false, context);
		}
		KidnappedTravelData kidnappedTravelData = DomainManager.Extra.GetKidnappedTravelData();
		if (kidnappedTravelData.Valid && destAreaId == kidnappedTravelData.Target.AreaId)
		{
			location.BlockId = kidnappedTravelData.Target.BlockId;
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.HidePartWorldMap);
			DomainManager.TaiwuEvent.OnEvent_TaiwuBeHuntedArrivedSect(kidnappedTravelData.HunterCharId);
			DomainManager.Extra.SetKidnappedTravelData(KidnappedTravelData.Invalid, context);
		}
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			element_Objects.SetLocation(location, context);
		}
		DomainManager.Extra.GearMateFollowTaiwu(context);
		_travelInfo.ToAreaId = -1;
		SetTravelInfo(_travelInfo, context);
		MapAreaData element_Areas = GetElement_Areas(destAreaId);
		if (!DomainManager.Map.IsAreaBroken(destAreaId))
		{
			sbyte stateID = element_Areas.GetConfig().StateID;
			if (!DomainManager.Extra.CheckIsUnlockedSectXuannvMusicByMapStateId(stateID))
			{
				DomainManager.Extra.UnlockSectXuannvMusicByMapStateId(context, stateID);
			}
		}
		DomainManager.Merchant.RefreshCaravanInTaiwuState(context);
		DomainManager.Extra.ClearTravelingEvents(context);
		DomainManager.Extra.ClearGainsInTravel(context);
	}

	private short GetPureTravelBlockId(DataContext context, short destAreaId, short fromAreaId)
	{
		short stationBlockId = _areas[destAreaId].StationBlockId;
		short num = destAreaId;
		bool flag = (uint)(num - 137) <= 1u;
		bool flag2 = !flag;
		bool flag3 = flag2;
		if (flag3)
		{
			bool flag4 = ((fromAreaId == 135 || fromAreaId == 138) ? true : false);
			flag3 = !flag4;
		}
		if (flag3)
		{
			return stationBlockId;
		}
		List<short> list = new List<short>();
		GetEdgeBlockList(destAreaId, list, excludeTravelBlock: true);
		list.RemoveAll((short blockId) => GetBlockData(destAreaId, blockId).TemplateId == 124);
		list.RemoveAll(delegate(short blockId)
		{
			MapBlockData blockData = GetBlockData(destAreaId, blockId);
			return (blockData.TemplateEnemyList != null && blockData.TemplateEnemyList.Count > 0) || DomainManager.Extra.IsLocationContainsAnimal(new Location(destAreaId, blockId));
		});
		stationBlockId = list.GetRandom(context.Random);
		MapBlockData block = GetBlock(destAreaId, stationBlockId);
		SetBlockAndViewRangeVisibleByMove(context, block);
		return stationBlockId;
	}

	private static void AddUnlockStationSeniority(DataContext context, int needAuthority)
	{
		ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[73];
		int baseDelta = formulaCfg.Calculate(needAuthority);
		DomainManager.Extra.ChangeProfessionSeniority(context, 11, baseDelta);
	}

	public unsafe short DetectTravelingEvent(DataContext context)
	{
		List<(short, short)> travelingEventWeights = _travelingEventWeights;
		travelingEventWeights.Clear();
		int routeIndex = _travelInfo.RouteIndex;
		short num = _travelInfo.CurrentAreaId;
		short fromAreaId = _travelInfo.LastAreaId;
		short toAreaId = _travelInfo.NextAreaId;
		if (_travelInfo.ToAreaId < 0)
		{
			num = (fromAreaId = (toAreaId = DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId));
		}
		sbyte stateTemplateIdByAreaId = GetStateTemplateIdByAreaId(num);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Personalities personalities = taiwu.GetPersonalities();
		sbyte fame = taiwu.GetFame();
		List<short> costList = _travelInfo.Route.CostList;
		int num2 = DomainManager.World.GetLeftDaysInCurrMonth() - ((costList.CheckIndex(routeIndex) ? costList[routeIndex] : costList[0]) + 1);
		foreach (TravelingEventItem item in (IEnumerable<TravelingEventItem>)TravelingEvent.Instance)
		{
			if (item.OccurRate > 0 && num2 >= item.NeedTime && fame >= item.FameLimit[0] && fame <= item.FameLimit[1] && CheckValidAreaToTrigger(item, num, fromAreaId, toAreaId) && (item.StateTemplateId < 0 || item.StateTemplateId == stateTemplateIdByAreaId) && (!item.IsUnique || !DomainManager.Extra.HasTravelingEventBeenTriggered(item.TemplateId)) && CheckTravelingEventSpecialCondition(context.Random, item.TemplateId))
			{
				int num3 = item.OccurRate;
				if (item.NeedPersonality >= 0)
				{
					sbyte b = personalities.Items[item.NeedPersonality];
					num3 = num3 * b / 50;
				}
				if (item.FameMultiplier != 0)
				{
					num3 = num3 * item.FameMultiplier * fame / 100;
				}
				travelingEventWeights.Add((item.TemplateId, (short)Math.Clamp(num3, 0, 32767)));
			}
		}
		CollectionUtils.Shuffle(context.Random, travelingEventWeights);
		travelingEventWeights.Sort(CompareTravelingEventsDec);
		foreach (var (num4, percentProb) in travelingEventWeights)
		{
			if (!context.Random.CheckPercentProb(percentProb))
			{
				continue;
			}
			int num5 = AddTravelingEvent(context, num4, num);
			if (num5 < 0)
			{
				continue;
			}
			DomainManager.Extra.AddTriggeredTravelingEvent(context, num4);
			TravelingEventItem travelingEventItem = TravelingEvent.Instance[num4];
			if (string.IsNullOrEmpty(travelingEventItem.Event))
			{
				return num4;
			}
			TravelingEventCollection travelingEventCollection = DomainManager.Extra.GetTravelingEventCollection();
			GameData.Domains.TaiwuEvent.TaiwuEvent taiwuEvent = DomainManager.TaiwuEvent.GetEvent(travelingEventItem.Event);
			if (taiwuEvent != null)
			{
				GameData.Domains.TaiwuEvent.TaiwuEvent taiwuEvent2 = taiwuEvent;
				if (taiwuEvent2.ArgBox == null)
				{
					EventArgBox eventArgBox = (taiwuEvent2.ArgBox = DomainManager.TaiwuEvent.GetEventArgBox());
				}
				travelingEventCollection.FillEventArgBox(num5, taiwuEvent.ArgBox);
				if (taiwuEvent.EventConfig.CheckCondition())
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
					DomainManager.TaiwuEvent.TravelingEventCheckComplete();
					DomainManager.Map.SetOnHandlingTravelingEventBlock(value: true, context);
					ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[76];
					int baseDelta = formulaCfg.Calculate();
					DomainManager.Extra.ChangeProfessionSeniority(context, 11, baseDelta);
					return num4;
				}
				int recordSize = travelingEventCollection.GetRecordSize(num5);
				travelingEventCollection.Remove(num5, recordSize);
				Logger.Warn($"Traveling event {num4} - {travelingEventItem.Name} is triggering {taiwuEvent.EventGuid} when OnCheckEventCondition return false.");
				return -1;
			}
			Logger.Warn($"Monthly Event {num4} - {travelingEventItem.Name} ({travelingEventItem.Event}) not found.");
			return -1;
		}
		return -1;
		static int CompareTravelingEventsDec((short templateId, short weight) a, (short templateId, short weight) tuple2)
		{
			sbyte occurOrder = TravelingEvent.Instance[a.templateId].OccurOrder;
			sbyte occurOrder2 = TravelingEvent.Instance[tuple2.templateId].OccurOrder;
			return occurOrder2.CompareTo(occurOrder);
		}
	}

	private bool CheckValidAreaToTrigger(TravelingEventItem config, short currAreaId, short fromAreaId, short toAreaId)
	{
		return config.TriggerType switch
		{
			ETravelingEventTriggerType.Any => CheckTriggerAreaType(config.TriggerAreaType, fromAreaId) || CheckTriggerAreaType(config.TriggerAreaType, currAreaId) || CheckTriggerAreaType(config.TriggerAreaType, toAreaId), 
			ETravelingEventTriggerType.OnArea => CheckTriggerAreaType(config.TriggerAreaType, currAreaId), 
			ETravelingEventTriggerType.ToArea => CheckTriggerAreaType(config.TriggerAreaType, toAreaId), 
			ETravelingEventTriggerType.FromArea => CheckTriggerAreaType(config.TriggerAreaType, fromAreaId), 
			_ => false, 
		};
	}

	private bool CheckTriggerAreaType(ETravelingEventTriggerAreaType areaType, short areaId)
	{
		switch (areaType)
		{
		case ETravelingEventTriggerAreaType.Any:
			return true;
		case ETravelingEventTriggerAreaType.NormalArea:
			return areaId < 45;
		case ETravelingEventTriggerAreaType.BrokenArea:
			return areaId >= 45 && areaId < 135;
		case ETravelingEventTriggerAreaType.SectArea:
		{
			MapAreaItem config2 = _areas[areaId].GetConfig();
			return MapState.Instance[config2.StateID].SectAreaID == config2.TemplateId;
		}
		case ETravelingEventTriggerAreaType.MainCityArea:
		{
			MapAreaItem config = _areas[areaId].GetConfig();
			return MapState.Instance[config.StateID].MainAreaID == config.TemplateId;
		}
		default:
			return false;
		}
	}

	private int AddTravelingEvent(DataContext context, short templateId, short areaId)
	{
		TravelingEventCollection travelingEventCollection = DomainManager.Extra.GetTravelingEventCollection();
		TravelingEventItem travelingEventItem = TravelingEvent.Instance[templateId];
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = new Location(areaId, -1);
		switch (travelingEventItem.Type)
		{
		case ETravelingEventType.AreaMaterial:
			var (itemType, num6) = GenerateTravelingEventItemParameter(context, travelingEventItem);
			if (string.IsNullOrEmpty(travelingEventItem.Event))
			{
				ItemKey itemKey3 = DomainManager.Item.CreateItem(context, itemType, num6);
				DomainManager.Extra.AddGainsInTravel(context, itemKey3);
				taiwu.AddInventoryItem(context, itemKey3, 1);
			}
			return travelingEventCollection.AddType_AreaMaterial(templateId, taiwuCharId, itemType, num6);
		case ETravelingEventType.AreaResource:
			var (b2, num11) = GenerateTravelingEventResourceParameter(context, travelingEventItem);
			if (b2 < 0 || num11 <= 0)
			{
				return -1;
			}
			if (string.IsNullOrEmpty(travelingEventItem.Event))
			{
				taiwu.ChangeResource(context, b2, num11);
			}
			return travelingEventCollection.AddType_AreaResource(templateId, taiwuCharId, num11, b2);
		case ETravelingEventType.AreaFood:
			var (itemType2, num9) = GenerateTravelingEventItemParameter(context, travelingEventItem);
			if (string.IsNullOrEmpty(travelingEventItem.Event))
			{
				ItemKey itemKey4 = DomainManager.Item.CreateItem(context, itemType2, num9);
				DomainManager.Extra.AddGainsInTravel(context, itemKey4);
				taiwu.AddInventoryItem(context, itemKey4, 1);
			}
			return travelingEventCollection.AddType_AreaFood(templateId, taiwuCharId, itemType2, num9);
		case ETravelingEventType.Heal:
		{
			int num3 = context.Random.Next(travelingEventItem.ValueRange[0], travelingEventItem.ValueRange[1]);
			switch (templateId)
			{
			case 45:
			{
				sbyte randomInjuredBodyPartToHeal2 = taiwu.GetRandomInjuredBodyPartToHeal(context.Random, isInnerInjury: false, 6);
				taiwu.ChangeInjury(context, randomInjuredBodyPartToHeal2, isInnerInjury: false, (sbyte)(-num3));
				return travelingEventCollection.AddHealOuterInjury(taiwuCharId, num3);
			}
			case 46:
			{
				sbyte randomInjuredBodyPartToHeal = taiwu.GetRandomInjuredBodyPartToHeal(context.Random, isInnerInjury: true, 6);
				if (randomInjuredBodyPartToHeal < 0)
				{
					return -1;
				}
				taiwu.ChangeInjury(context, randomInjuredBodyPartToHeal, isInnerInjury: true, (sbyte)(-num3));
				return travelingEventCollection.AddHealInnerInjury(taiwuCharId, num3);
			}
			case 47:
			{
				sbyte randomPoisonTypeToDetox = taiwu.GetRandomPoisonTypeToDetox(context.Random, 3);
				if (randomPoisonTypeToDetox < 0)
				{
					return -1;
				}
				taiwu.ChangePoisoned(context, randomPoisonTypeToDetox, 3, -num3);
				return travelingEventCollection.AddHealPoison(taiwuCharId, randomPoisonTypeToDetox);
			}
			case 48:
				taiwu.ChangeDisorderOfQi(context, (short)(-num3));
				return travelingEventCollection.AddHealDisorderOfQi(taiwuCharId, num3);
			case 49:
				taiwu.ChangeHealth(context, num3);
				return travelingEventCollection.AddHealLifeSpan(taiwuCharId, num3);
			default:
				return -1;
			}
		}
		case ETravelingEventType.CharacterGiftItem:
		{
			int num2 = ((travelingEventItem.FameMultiplier != 0) ? GenerateTravelingEventAreaCharacterParameter(context, travelingEventItem, areaId) : GenerateTravelingEventFriendOrFamilyCharacterParameter(context, travelingEventItem, areaId));
			if (num2 < 0)
			{
				return -1;
			}
			ItemKey itemKey2 = GenerateTravelingEventCharacterItemParameter(context, travelingEventItem, num2);
			if (!itemKey2.IsValid())
			{
				return -1;
			}
			if (string.IsNullOrEmpty(travelingEventItem.Event))
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
				DomainManager.Character.TransferInventoryItem(context, element_Objects, taiwu, itemKey2, 1);
				DomainManager.Extra.AddGainsInTravel(context, itemKey2);
			}
			return travelingEventCollection.AddType_CharacterGiftItem(templateId, taiwuCharId, location, num2, itemKey2.ItemType, itemKey2.TemplateId);
		}
		case ETravelingEventType.CharacterGiftResource:
		{
			int num7 = ((travelingEventItem.FameMultiplier != 0) ? GenerateTravelingEventAreaCharacterParameter(context, travelingEventItem, areaId) : GenerateTravelingEventFriendOrFamilyCharacterParameter(context, travelingEventItem, areaId));
			if (num7 < 0)
			{
				return -1;
			}
			var (b, num8) = GenerateTravelingEventCharacterResourceParameter(context, travelingEventItem, num7);
			if (b < 0 || num8 <= 0)
			{
				return -1;
			}
			if (string.IsNullOrEmpty(travelingEventItem.Event))
			{
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(num7);
				DomainManager.Character.TransferResource(context, element_Objects2, taiwu, b, num8);
			}
			return travelingEventCollection.AddType_CharacterGiftResource(templateId, taiwuCharId, location, num7, num8, b);
		}
		case ETravelingEventType.AttributeRegen:
			if (string.IsNullOrEmpty(travelingEventItem.Event))
			{
				sbyte mainAttributeType = (sbyte)(travelingEventItem.CharacterProperty - 0);
				taiwu.ChangeCurrMainAttribute(context, mainAttributeType, 10);
			}
			return travelingEventCollection.AddType_AttributeRegen(templateId, taiwuCharId, location, 10);
		case ETravelingEventType.AreaInteraction:
		{
			int num10 = GenerateTravelingEventAreaCharacterParameter(context, travelingEventItem, location.AreaId);
			if (num10 < 0)
			{
				return -1;
			}
			return travelingEventCollection.AddType_AreaInteraction(templateId, taiwuCharId, location, num10);
		}
		case ETravelingEventType.SpiritualDebt:
		{
			short num5 = GenerateTravelingEventSettlementParameter(context, travelingEventItem, location.AreaId);
			if (num5 < 0)
			{
				return -1;
			}
			return travelingEventCollection.AddType_SpiritualDebt(templateId, taiwuCharId, num5);
		}
		case ETravelingEventType.SectVisit:
			return travelingEventCollection.AddType_SectVisit(templateId, taiwuCharId, location);
		case ETravelingEventType.Combat:
		{
			short num = GenerateTravelingEventEnemyCharTemplateParameter(context, travelingEventItem);
			if (num < 0)
			{
				return -1;
			}
			return travelingEventCollection.AddType_Combat(templateId, taiwuCharId, num);
		}
		case ETravelingEventType.SectCombat:
		{
			short num12 = GenerateTravelingEventEnemyCharTemplateParameter(context, travelingEventItem);
			if (num12 < 0)
			{
				return -1;
			}
			return travelingEventCollection.AddType_SectCombat(templateId, taiwuCharId, num12);
		}
		case ETravelingEventType.CharacterRecommendVillager:
		{
			int num4 = ((travelingEventItem.FameMultiplier != 0) ? GenerateTravelingEventAreaCharacterParameter(context, travelingEventItem, areaId) : GenerateTravelingEventFriendOrFamilyCharacterParameter(context, travelingEventItem, areaId));
			if (num4 < 0)
			{
				return -1;
			}
			int recommendedCharacterId = GetRecommendedCharacterId(context, num4);
			if (recommendedCharacterId < 0)
			{
				return -1;
			}
			return travelingEventCollection.AddType_CharacterRecommendVillager(templateId, taiwuCharId, location, num4, recommendedCharacterId);
		}
		case ETravelingEventType.AttributeCost:
			return travelingEventCollection.AddType_AttributeCost(templateId, taiwuCharId, 10);
		case ETravelingEventType.CarrierDurability:
		{
			ItemKey itemKey = taiwu.GetEquipment()[11];
			EquipmentBase equipmentBase = DomainManager.Item.TryGetBaseEquipment(itemKey);
			if ((equipmentBase == null || equipmentBase.GetCurrDurability() == 0) && DomainManager.World.GetLeftDaysInCurrMonth() < travelingEventItem.NeedTime + 3)
			{
				return -1;
			}
			return travelingEventCollection.AddRoadBlock(taiwuCharId);
		}
		default:
			return -1;
		}
	}

	private bool CheckTravelingEventSpecialCondition(IRandomSource random, short templateId)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		switch (templateId)
		{
		case 45:
			return taiwu.GetRandomInjuredBodyPartToHeal(random, isInnerInjury: false, 6) >= 0;
		case 46:
			return taiwu.GetRandomInjuredBodyPartToHeal(random, isInnerInjury: true, 6) >= 0;
		case 47:
			return taiwu.GetRandomPoisonTypeToDetox(random, 3) >= 0;
		case 48:
			return taiwu.GetDisorderOfQi() > 0;
		case 49:
			return taiwu.GetHealth() < taiwu.GetLeftMaxHealth();
		default:
		{
			TravelingEventItem item = TravelingEvent.Instance.GetItem(templateId);
			if (item != null && item.Type == ETravelingEventType.Combat && DomainManager.Taiwu.IsTaiwuAvoidTravelingEnemies())
			{
				return false;
			}
			return true;
		}
		}
	}

	private (sbyte itemType, short itemTemplateId) GenerateTravelingEventItemParameter(DataContext context, TravelingEventItem config)
	{
		Tester.Assert(config.ItemRange.Count > 0 && config.ItemGradeWeight != null && config.ItemGradeWeight.Length != 0);
		List<TemplateKey> obj = context.AdvanceMonthRelatedData.ItemTemplateKeys.Occupy();
		int randomIndex = RandomUtils.GetRandomIndex(config.ItemGradeWeight, context.Random);
		PresetItemTemplateIdGroup random = config.ItemRange.GetRandom(context.Random);
		for (int i = 0; i < random.GroupLength; i++)
		{
			short templateId = (short)(random.StartId + i);
			if (ItemTemplateHelper.GetGrade(random.ItemType, templateId) == randomIndex)
			{
				obj.Add(new TemplateKey(random.ItemType, templateId));
			}
		}
		if (obj.Count == 0)
		{
			throw new Exception($"No valid item of type {random.ItemType} and grade {randomIndex} for traveling event {config.TemplateId} {config.Name}");
		}
		TemplateKey random2 = obj.GetRandom(context.Random);
		context.AdvanceMonthRelatedData.ItemTemplateKeys.Release(ref obj);
		return (itemType: random2.ItemType, itemTemplateId: random2.TemplateId);
	}

	private ItemKey GenerateTravelingEventCharacterItemParameter(DataContext context, TravelingEventItem config, int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		Inventory inventory = element_Objects.GetInventory();
		List<ItemKey> obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		List<(ItemBase, int)>[] obj2 = context.AdvanceMonthRelatedData.ClassifiedItems.Occupy();
		foreach (var (itemKey2, item) in inventory.Items)
		{
			if (itemKey2.ItemType == config.FilterItemType && !ItemTemplateHelper.IsTransferable(itemKey2.ItemType, itemKey2.TemplateId))
			{
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey2);
				obj2[baseItem.GetGrade() / 3].Add((baseItem, item));
			}
		}
		List<(ItemBase, int)>[] array = obj2;
		foreach (List<(ItemBase, int)> list in array)
		{
			if (list.Count > 0)
			{
				obj.Add(list.GetRandom(context.Random).Item1.GetItemKey());
			}
		}
		ItemKey randomOrDefault = obj.GetRandomOrDefault(context.Random, ItemKey.Invalid);
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		context.AdvanceMonthRelatedData.ClassifiedItems.Release(ref obj2);
		return randomOrDefault;
	}

	private short GenerateTravelingEventSettlementParameter(DataContext context, TravelingEventItem config, short areaId)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		SettlementInfo[] settlementInfos = _areas[areaId].SettlementInfos;
		for (int i = 0; i < settlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = settlementInfos[i];
			if (settlementInfo.SettlementId >= 0 && GetBlock(areaId, settlementInfo.BlockId).BlockType != EMapBlockType.Sect)
			{
				list.Add(settlementInfo.SettlementId);
			}
		}
		int num = ((list.Count > 0) ? list.GetRandom(context.Random) : (-1));
		ObjectPool<List<short>>.Instance.Return(list);
		return (short)num;
	}

	private (sbyte resourceType, int amount) GenerateTravelingEventResourceParameter(DataContext context, TravelingEventItem config)
	{
		Tester.Assert(config.ResourceWeights != null && config.ResourceWeights.Length != 0);
		sbyte b = (sbyte)RandomUtils.GetRandomIndex(config.ResourceWeights, context.Random);
		int num = context.Random.Next(50, 151) * GlobalConfig.ResourcesWorth[0];
		int item = num / GlobalConfig.ResourcesWorth[b];
		return (resourceType: b, amount: item);
	}

	private (sbyte resourceType, int amount) GenerateTravelingEventCharacterResourceParameter(DataContext context, TravelingEventItem config, int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			if (element_Objects.GetResource(b) >= 100)
			{
				list.Add(b);
			}
		}
		sbyte b2 = (sbyte)((list.Count > 0) ? list.GetRandom(context.Random) : (-1));
		ObjectPool<List<sbyte>>.Instance.Return(list);
		if (b2 < 0)
		{
			return (resourceType: -1, amount: 0);
		}
		int item = element_Objects.GetResource(b2) / context.Random.Next(5, 11);
		return (resourceType: b2, amount: item);
	}

	private short GenerateTravelingEventEnemyCharTemplateParameter(DataContext context, TravelingEventItem config)
	{
		sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
		sbyte expectedGrade = Math.Clamp(xiangshuLevel, 0, 8);
		return CharacterDomain.GetRandomEnemyCharTemplateId(context.Random, config.OrgTemplateId, expectedGrade);
	}

	private int GenerateTravelingEventAreaCharacterParameter(DataContext context, TravelingEventItem config, short areaId)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		List<int> obj = context.AdvanceMonthRelatedData.CharIdList.Occupy();
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		int i = 0;
		for (int length = areaBlocks.Length; i < length; i++)
		{
			HashSet<int> characterSet = areaBlocks[i].CharacterSet;
			if (characterSet == null)
			{
				continue;
			}
			foreach (int item in characterSet)
			{
				if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.IsInteractableAsIntelligentCharacter() && !DomainManager.Character.HasRelation(item, taiwuCharId, 32768))
				{
					obj.Add(item);
				}
			}
		}
		int randomOrDefault = obj.GetRandomOrDefault(context.Random, -1);
		context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
		return randomOrDefault;
	}

	private int GenerateTravelingEventFriendOrFamilyCharacterParameter(DataContext context, TravelingEventItem config, short areaId)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		List<int> obj = context.AdvanceMonthRelatedData.CharIdList.Occupy();
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		int i = 0;
		for (int length = areaBlocks.Length; i < length; i++)
		{
			HashSet<int> characterSet = areaBlocks[i].CharacterSet;
			if (characterSet == null)
			{
				continue;
			}
			foreach (int item in characterSet)
			{
				if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.IsInteractableAsIntelligentCharacter() && DomainManager.Character.TryGetRelation(item, taiwuCharId, out var relation) && AiHelper.ActionTargetRelationCategory.GetTargetRelationCategory(relation.RelationType) == 1)
				{
					obj.Add(item);
				}
			}
		}
		int randomOrDefault = obj.GetRandomOrDefault(context.Random, -1);
		context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
		return randomOrDefault;
	}

	private int GetRecommendedCharacterId(DataContext context, int recommenderId)
	{
		HashSet<int> obj = context.AdvanceMonthRelatedData.RelatedCharIds.Occupy();
		DomainManager.Character.GetAllTwoWayRelatedCharIds(recommenderId, obj);
		OrganizationInfo organizationInfo = DomainManager.Taiwu.GetTaiwu().GetOrganizationInfo();
		sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
		sbyte b = GlobalConfig.Instance.XiangshuInfectionGradeUpperLimits[xiangshuLevel];
		int result = -1;
		short num = -30000;
		foreach (int item in obj)
		{
			if (!DomainManager.Character.TryGetElement_Objects(item, out var element) || !element.IsInteractableAsIntelligentCharacter())
			{
				continue;
			}
			OrganizationInfo organizationInfo2 = element.GetOrganizationInfo();
			if (organizationInfo2.Grade <= b && organizationInfo2.OrgTemplateId != organizationInfo.OrgTemplateId && !element.IsTreasuryGuard())
			{
				RelatedCharacter relation = DomainManager.Character.GetRelation(recommenderId, item);
				if (relation.Favorability > num)
				{
					result = item;
					num = relation.Favorability;
				}
			}
		}
		context.AdvanceMonthRelatedData.RelatedCharIds.Release(ref obj);
		return result;
	}

	public MapDomain()
		: base(68)
	{
		_areas = new MapAreaData[139];
		_areaBlocks0 = new AreaBlockCollection();
		_areaBlocks1 = new AreaBlockCollection();
		_areaBlocks2 = new AreaBlockCollection();
		_areaBlocks3 = new AreaBlockCollection();
		_areaBlocks4 = new AreaBlockCollection();
		_areaBlocks5 = new AreaBlockCollection();
		_areaBlocks6 = new AreaBlockCollection();
		_areaBlocks7 = new AreaBlockCollection();
		_areaBlocks8 = new AreaBlockCollection();
		_areaBlocks9 = new AreaBlockCollection();
		_areaBlocks10 = new AreaBlockCollection();
		_areaBlocks11 = new AreaBlockCollection();
		_areaBlocks12 = new AreaBlockCollection();
		_areaBlocks13 = new AreaBlockCollection();
		_areaBlocks14 = new AreaBlockCollection();
		_areaBlocks15 = new AreaBlockCollection();
		_areaBlocks16 = new AreaBlockCollection();
		_areaBlocks17 = new AreaBlockCollection();
		_areaBlocks18 = new AreaBlockCollection();
		_areaBlocks19 = new AreaBlockCollection();
		_areaBlocks20 = new AreaBlockCollection();
		_areaBlocks21 = new AreaBlockCollection();
		_areaBlocks22 = new AreaBlockCollection();
		_areaBlocks23 = new AreaBlockCollection();
		_areaBlocks24 = new AreaBlockCollection();
		_areaBlocks25 = new AreaBlockCollection();
		_areaBlocks26 = new AreaBlockCollection();
		_areaBlocks27 = new AreaBlockCollection();
		_areaBlocks28 = new AreaBlockCollection();
		_areaBlocks29 = new AreaBlockCollection();
		_areaBlocks30 = new AreaBlockCollection();
		_areaBlocks31 = new AreaBlockCollection();
		_areaBlocks32 = new AreaBlockCollection();
		_areaBlocks33 = new AreaBlockCollection();
		_areaBlocks34 = new AreaBlockCollection();
		_areaBlocks35 = new AreaBlockCollection();
		_areaBlocks36 = new AreaBlockCollection();
		_areaBlocks37 = new AreaBlockCollection();
		_areaBlocks38 = new AreaBlockCollection();
		_areaBlocks39 = new AreaBlockCollection();
		_areaBlocks40 = new AreaBlockCollection();
		_areaBlocks41 = new AreaBlockCollection();
		_areaBlocks42 = new AreaBlockCollection();
		_areaBlocks43 = new AreaBlockCollection();
		_areaBlocks44 = new AreaBlockCollection();
		_brokenAreaBlocks = new AreaBlockCollection();
		_bornAreaBlocks = new AreaBlockCollection();
		_guideAreaBlocks = new AreaBlockCollection();
		_secretVillageAreaBlocks = new AreaBlockCollection();
		_brokenPerformAreaBlocks = new AreaBlockCollection();
		_travelRouteDict = new Dictionary<TravelRouteKey, TravelRoute>(0);
		_bornStateTravelRouteDict = new Dictionary<TravelRouteKey, TravelRoute>(0);
		_animalPlaceData = new AnimalPlaceData[139];
		_cricketPlaceData = new CricketPlaceData[139];
		_regularAreaNearList = new Dictionary<short, GameData.Utilities.ShortList>(0);
		_swordTombLocations = new Location[8];
		_travelInfo = new CrossAreaMoveInfo();
		_onHandlingTravelingEventBlock = false;
		_hunterAnimalsCache = new List<HunterAnimalKey>();
		_moveBanned = 0;
		_crossArchiveLockMoveTime = false;
		_fleeBeasts = new List<Location>();
		_fleeLoongs = new List<Location>();
		_loongLocations = new List<LoongLocationData>();
		_alterSettlementLocations = new List<Location>();
		_isTaiwuInFulongFlameArea = false;
		_visibleMapPickups = new List<MapElementPickupDisplayData>();
		OnInitializedDomainData();
	}

	public MapAreaData GetElement_Areas(int index)
	{
		return _areas[index];
	}

	private unsafe void SetElement_Areas(int index, MapAreaData value, DataContext context)
	{
		_areas[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesAreas, CacheInfluencesAreas, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicElementList_Set(2, 0, index, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicElementList_Set(2, 0, index, 0);
		}
	}

	public MapBlockData GetElement_AreaBlocks0(short elementId)
	{
		return _areaBlocks0[elementId];
	}

	public bool TryGetElement_AreaBlocks0(short elementId, out MapBlockData value)
	{
		return _areaBlocks0.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks0(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks0.Add(elementId, value);
		_modificationsAreaBlocks0.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 1, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 1, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks0(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks0[elementId] = value;
		_modificationsAreaBlocks0.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 1, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 1, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks0(short elementId, DataContext context)
	{
		_areaBlocks0.Remove(elementId);
		_modificationsAreaBlocks0.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 1, elementId);
	}

	private void ClearAreaBlocks0(DataContext context)
	{
		_areaBlocks0.Clear();
		_modificationsAreaBlocks0.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 1);
	}

	public MapBlockData GetElement_AreaBlocks1(short elementId)
	{
		return _areaBlocks1[elementId];
	}

	public bool TryGetElement_AreaBlocks1(short elementId, out MapBlockData value)
	{
		return _areaBlocks1.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks1(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks1.Add(elementId, value);
		_modificationsAreaBlocks1.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 2, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 2, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks1(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks1[elementId] = value;
		_modificationsAreaBlocks1.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 2, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 2, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks1(short elementId, DataContext context)
	{
		_areaBlocks1.Remove(elementId);
		_modificationsAreaBlocks1.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 2, elementId);
	}

	private void ClearAreaBlocks1(DataContext context)
	{
		_areaBlocks1.Clear();
		_modificationsAreaBlocks1.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 2);
	}

	public MapBlockData GetElement_AreaBlocks2(short elementId)
	{
		return _areaBlocks2[elementId];
	}

	public bool TryGetElement_AreaBlocks2(short elementId, out MapBlockData value)
	{
		return _areaBlocks2.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks2(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks2.Add(elementId, value);
		_modificationsAreaBlocks2.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 3, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 3, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks2(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks2[elementId] = value;
		_modificationsAreaBlocks2.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 3, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 3, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks2(short elementId, DataContext context)
	{
		_areaBlocks2.Remove(elementId);
		_modificationsAreaBlocks2.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 3, elementId);
	}

	private void ClearAreaBlocks2(DataContext context)
	{
		_areaBlocks2.Clear();
		_modificationsAreaBlocks2.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 3);
	}

	public MapBlockData GetElement_AreaBlocks3(short elementId)
	{
		return _areaBlocks3[elementId];
	}

	public bool TryGetElement_AreaBlocks3(short elementId, out MapBlockData value)
	{
		return _areaBlocks3.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks3(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks3.Add(elementId, value);
		_modificationsAreaBlocks3.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 4, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 4, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks3(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks3[elementId] = value;
		_modificationsAreaBlocks3.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 4, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 4, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks3(short elementId, DataContext context)
	{
		_areaBlocks3.Remove(elementId);
		_modificationsAreaBlocks3.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 4, elementId);
	}

	private void ClearAreaBlocks3(DataContext context)
	{
		_areaBlocks3.Clear();
		_modificationsAreaBlocks3.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 4);
	}

	public MapBlockData GetElement_AreaBlocks4(short elementId)
	{
		return _areaBlocks4[elementId];
	}

	public bool TryGetElement_AreaBlocks4(short elementId, out MapBlockData value)
	{
		return _areaBlocks4.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks4(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks4.Add(elementId, value);
		_modificationsAreaBlocks4.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 5, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 5, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks4(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks4[elementId] = value;
		_modificationsAreaBlocks4.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 5, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 5, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks4(short elementId, DataContext context)
	{
		_areaBlocks4.Remove(elementId);
		_modificationsAreaBlocks4.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 5, elementId);
	}

	private void ClearAreaBlocks4(DataContext context)
	{
		_areaBlocks4.Clear();
		_modificationsAreaBlocks4.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 5);
	}

	public MapBlockData GetElement_AreaBlocks5(short elementId)
	{
		return _areaBlocks5[elementId];
	}

	public bool TryGetElement_AreaBlocks5(short elementId, out MapBlockData value)
	{
		return _areaBlocks5.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks5(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks5.Add(elementId, value);
		_modificationsAreaBlocks5.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 6, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 6, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks5(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks5[elementId] = value;
		_modificationsAreaBlocks5.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 6, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 6, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks5(short elementId, DataContext context)
	{
		_areaBlocks5.Remove(elementId);
		_modificationsAreaBlocks5.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 6, elementId);
	}

	private void ClearAreaBlocks5(DataContext context)
	{
		_areaBlocks5.Clear();
		_modificationsAreaBlocks5.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 6);
	}

	public MapBlockData GetElement_AreaBlocks6(short elementId)
	{
		return _areaBlocks6[elementId];
	}

	public bool TryGetElement_AreaBlocks6(short elementId, out MapBlockData value)
	{
		return _areaBlocks6.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks6(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks6.Add(elementId, value);
		_modificationsAreaBlocks6.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 7, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 7, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks6(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks6[elementId] = value;
		_modificationsAreaBlocks6.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 7, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 7, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks6(short elementId, DataContext context)
	{
		_areaBlocks6.Remove(elementId);
		_modificationsAreaBlocks6.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 7, elementId);
	}

	private void ClearAreaBlocks6(DataContext context)
	{
		_areaBlocks6.Clear();
		_modificationsAreaBlocks6.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 7);
	}

	public MapBlockData GetElement_AreaBlocks7(short elementId)
	{
		return _areaBlocks7[elementId];
	}

	public bool TryGetElement_AreaBlocks7(short elementId, out MapBlockData value)
	{
		return _areaBlocks7.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks7(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks7.Add(elementId, value);
		_modificationsAreaBlocks7.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 8, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 8, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks7(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks7[elementId] = value;
		_modificationsAreaBlocks7.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 8, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 8, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks7(short elementId, DataContext context)
	{
		_areaBlocks7.Remove(elementId);
		_modificationsAreaBlocks7.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 8, elementId);
	}

	private void ClearAreaBlocks7(DataContext context)
	{
		_areaBlocks7.Clear();
		_modificationsAreaBlocks7.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 8);
	}

	public MapBlockData GetElement_AreaBlocks8(short elementId)
	{
		return _areaBlocks8[elementId];
	}

	public bool TryGetElement_AreaBlocks8(short elementId, out MapBlockData value)
	{
		return _areaBlocks8.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks8(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks8.Add(elementId, value);
		_modificationsAreaBlocks8.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 9, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 9, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks8(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks8[elementId] = value;
		_modificationsAreaBlocks8.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 9, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 9, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks8(short elementId, DataContext context)
	{
		_areaBlocks8.Remove(elementId);
		_modificationsAreaBlocks8.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 9, elementId);
	}

	private void ClearAreaBlocks8(DataContext context)
	{
		_areaBlocks8.Clear();
		_modificationsAreaBlocks8.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 9);
	}

	public MapBlockData GetElement_AreaBlocks9(short elementId)
	{
		return _areaBlocks9[elementId];
	}

	public bool TryGetElement_AreaBlocks9(short elementId, out MapBlockData value)
	{
		return _areaBlocks9.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks9(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks9.Add(elementId, value);
		_modificationsAreaBlocks9.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 10, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 10, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks9(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks9[elementId] = value;
		_modificationsAreaBlocks9.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 10, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 10, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks9(short elementId, DataContext context)
	{
		_areaBlocks9.Remove(elementId);
		_modificationsAreaBlocks9.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 10, elementId);
	}

	private void ClearAreaBlocks9(DataContext context)
	{
		_areaBlocks9.Clear();
		_modificationsAreaBlocks9.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 10);
	}

	public MapBlockData GetElement_AreaBlocks10(short elementId)
	{
		return _areaBlocks10[elementId];
	}

	public bool TryGetElement_AreaBlocks10(short elementId, out MapBlockData value)
	{
		return _areaBlocks10.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks10(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks10.Add(elementId, value);
		_modificationsAreaBlocks10.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 11, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 11, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks10(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks10[elementId] = value;
		_modificationsAreaBlocks10.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 11, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 11, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks10(short elementId, DataContext context)
	{
		_areaBlocks10.Remove(elementId);
		_modificationsAreaBlocks10.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 11, elementId);
	}

	private void ClearAreaBlocks10(DataContext context)
	{
		_areaBlocks10.Clear();
		_modificationsAreaBlocks10.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 11);
	}

	public MapBlockData GetElement_AreaBlocks11(short elementId)
	{
		return _areaBlocks11[elementId];
	}

	public bool TryGetElement_AreaBlocks11(short elementId, out MapBlockData value)
	{
		return _areaBlocks11.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks11(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks11.Add(elementId, value);
		_modificationsAreaBlocks11.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 12, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 12, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks11(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks11[elementId] = value;
		_modificationsAreaBlocks11.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 12, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 12, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks11(short elementId, DataContext context)
	{
		_areaBlocks11.Remove(elementId);
		_modificationsAreaBlocks11.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 12, elementId);
	}

	private void ClearAreaBlocks11(DataContext context)
	{
		_areaBlocks11.Clear();
		_modificationsAreaBlocks11.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 12);
	}

	public MapBlockData GetElement_AreaBlocks12(short elementId)
	{
		return _areaBlocks12[elementId];
	}

	public bool TryGetElement_AreaBlocks12(short elementId, out MapBlockData value)
	{
		return _areaBlocks12.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks12(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks12.Add(elementId, value);
		_modificationsAreaBlocks12.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 13, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 13, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks12(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks12[elementId] = value;
		_modificationsAreaBlocks12.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 13, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 13, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks12(short elementId, DataContext context)
	{
		_areaBlocks12.Remove(elementId);
		_modificationsAreaBlocks12.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 13, elementId);
	}

	private void ClearAreaBlocks12(DataContext context)
	{
		_areaBlocks12.Clear();
		_modificationsAreaBlocks12.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 13);
	}

	public MapBlockData GetElement_AreaBlocks13(short elementId)
	{
		return _areaBlocks13[elementId];
	}

	public bool TryGetElement_AreaBlocks13(short elementId, out MapBlockData value)
	{
		return _areaBlocks13.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks13(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks13.Add(elementId, value);
		_modificationsAreaBlocks13.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 14, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 14, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks13(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks13[elementId] = value;
		_modificationsAreaBlocks13.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 14, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 14, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks13(short elementId, DataContext context)
	{
		_areaBlocks13.Remove(elementId);
		_modificationsAreaBlocks13.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 14, elementId);
	}

	private void ClearAreaBlocks13(DataContext context)
	{
		_areaBlocks13.Clear();
		_modificationsAreaBlocks13.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 14);
	}

	public MapBlockData GetElement_AreaBlocks14(short elementId)
	{
		return _areaBlocks14[elementId];
	}

	public bool TryGetElement_AreaBlocks14(short elementId, out MapBlockData value)
	{
		return _areaBlocks14.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks14(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks14.Add(elementId, value);
		_modificationsAreaBlocks14.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 15, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 15, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks14(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks14[elementId] = value;
		_modificationsAreaBlocks14.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 15, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 15, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks14(short elementId, DataContext context)
	{
		_areaBlocks14.Remove(elementId);
		_modificationsAreaBlocks14.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 15, elementId);
	}

	private void ClearAreaBlocks14(DataContext context)
	{
		_areaBlocks14.Clear();
		_modificationsAreaBlocks14.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 15);
	}

	public MapBlockData GetElement_AreaBlocks15(short elementId)
	{
		return _areaBlocks15[elementId];
	}

	public bool TryGetElement_AreaBlocks15(short elementId, out MapBlockData value)
	{
		return _areaBlocks15.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks15(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks15.Add(elementId, value);
		_modificationsAreaBlocks15.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 16, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 16, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks15(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks15[elementId] = value;
		_modificationsAreaBlocks15.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 16, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 16, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks15(short elementId, DataContext context)
	{
		_areaBlocks15.Remove(elementId);
		_modificationsAreaBlocks15.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 16, elementId);
	}

	private void ClearAreaBlocks15(DataContext context)
	{
		_areaBlocks15.Clear();
		_modificationsAreaBlocks15.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 16);
	}

	public MapBlockData GetElement_AreaBlocks16(short elementId)
	{
		return _areaBlocks16[elementId];
	}

	public bool TryGetElement_AreaBlocks16(short elementId, out MapBlockData value)
	{
		return _areaBlocks16.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks16(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks16.Add(elementId, value);
		_modificationsAreaBlocks16.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 17, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 17, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks16(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks16[elementId] = value;
		_modificationsAreaBlocks16.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 17, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 17, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks16(short elementId, DataContext context)
	{
		_areaBlocks16.Remove(elementId);
		_modificationsAreaBlocks16.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 17, elementId);
	}

	private void ClearAreaBlocks16(DataContext context)
	{
		_areaBlocks16.Clear();
		_modificationsAreaBlocks16.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 17);
	}

	public MapBlockData GetElement_AreaBlocks17(short elementId)
	{
		return _areaBlocks17[elementId];
	}

	public bool TryGetElement_AreaBlocks17(short elementId, out MapBlockData value)
	{
		return _areaBlocks17.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks17(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks17.Add(elementId, value);
		_modificationsAreaBlocks17.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 18, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 18, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks17(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks17[elementId] = value;
		_modificationsAreaBlocks17.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 18, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 18, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks17(short elementId, DataContext context)
	{
		_areaBlocks17.Remove(elementId);
		_modificationsAreaBlocks17.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 18, elementId);
	}

	private void ClearAreaBlocks17(DataContext context)
	{
		_areaBlocks17.Clear();
		_modificationsAreaBlocks17.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 18);
	}

	public MapBlockData GetElement_AreaBlocks18(short elementId)
	{
		return _areaBlocks18[elementId];
	}

	public bool TryGetElement_AreaBlocks18(short elementId, out MapBlockData value)
	{
		return _areaBlocks18.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks18(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks18.Add(elementId, value);
		_modificationsAreaBlocks18.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 19, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 19, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks18(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks18[elementId] = value;
		_modificationsAreaBlocks18.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 19, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 19, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks18(short elementId, DataContext context)
	{
		_areaBlocks18.Remove(elementId);
		_modificationsAreaBlocks18.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 19, elementId);
	}

	private void ClearAreaBlocks18(DataContext context)
	{
		_areaBlocks18.Clear();
		_modificationsAreaBlocks18.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 19);
	}

	public MapBlockData GetElement_AreaBlocks19(short elementId)
	{
		return _areaBlocks19[elementId];
	}

	public bool TryGetElement_AreaBlocks19(short elementId, out MapBlockData value)
	{
		return _areaBlocks19.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks19(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks19.Add(elementId, value);
		_modificationsAreaBlocks19.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 20, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 20, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks19(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks19[elementId] = value;
		_modificationsAreaBlocks19.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 20, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 20, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks19(short elementId, DataContext context)
	{
		_areaBlocks19.Remove(elementId);
		_modificationsAreaBlocks19.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 20, elementId);
	}

	private void ClearAreaBlocks19(DataContext context)
	{
		_areaBlocks19.Clear();
		_modificationsAreaBlocks19.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 20);
	}

	public MapBlockData GetElement_AreaBlocks20(short elementId)
	{
		return _areaBlocks20[elementId];
	}

	public bool TryGetElement_AreaBlocks20(short elementId, out MapBlockData value)
	{
		return _areaBlocks20.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks20(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks20.Add(elementId, value);
		_modificationsAreaBlocks20.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 21, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 21, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks20(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks20[elementId] = value;
		_modificationsAreaBlocks20.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 21, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 21, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks20(short elementId, DataContext context)
	{
		_areaBlocks20.Remove(elementId);
		_modificationsAreaBlocks20.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 21, elementId);
	}

	private void ClearAreaBlocks20(DataContext context)
	{
		_areaBlocks20.Clear();
		_modificationsAreaBlocks20.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 21);
	}

	public MapBlockData GetElement_AreaBlocks21(short elementId)
	{
		return _areaBlocks21[elementId];
	}

	public bool TryGetElement_AreaBlocks21(short elementId, out MapBlockData value)
	{
		return _areaBlocks21.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks21(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks21.Add(elementId, value);
		_modificationsAreaBlocks21.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 22, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 22, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks21(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks21[elementId] = value;
		_modificationsAreaBlocks21.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 22, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 22, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks21(short elementId, DataContext context)
	{
		_areaBlocks21.Remove(elementId);
		_modificationsAreaBlocks21.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 22, elementId);
	}

	private void ClearAreaBlocks21(DataContext context)
	{
		_areaBlocks21.Clear();
		_modificationsAreaBlocks21.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 22);
	}

	public MapBlockData GetElement_AreaBlocks22(short elementId)
	{
		return _areaBlocks22[elementId];
	}

	public bool TryGetElement_AreaBlocks22(short elementId, out MapBlockData value)
	{
		return _areaBlocks22.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks22(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks22.Add(elementId, value);
		_modificationsAreaBlocks22.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 23, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 23, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks22(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks22[elementId] = value;
		_modificationsAreaBlocks22.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 23, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 23, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks22(short elementId, DataContext context)
	{
		_areaBlocks22.Remove(elementId);
		_modificationsAreaBlocks22.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 23, elementId);
	}

	private void ClearAreaBlocks22(DataContext context)
	{
		_areaBlocks22.Clear();
		_modificationsAreaBlocks22.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 23);
	}

	public MapBlockData GetElement_AreaBlocks23(short elementId)
	{
		return _areaBlocks23[elementId];
	}

	public bool TryGetElement_AreaBlocks23(short elementId, out MapBlockData value)
	{
		return _areaBlocks23.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks23(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks23.Add(elementId, value);
		_modificationsAreaBlocks23.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 24, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 24, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks23(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks23[elementId] = value;
		_modificationsAreaBlocks23.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 24, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 24, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks23(short elementId, DataContext context)
	{
		_areaBlocks23.Remove(elementId);
		_modificationsAreaBlocks23.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 24, elementId);
	}

	private void ClearAreaBlocks23(DataContext context)
	{
		_areaBlocks23.Clear();
		_modificationsAreaBlocks23.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 24);
	}

	public MapBlockData GetElement_AreaBlocks24(short elementId)
	{
		return _areaBlocks24[elementId];
	}

	public bool TryGetElement_AreaBlocks24(short elementId, out MapBlockData value)
	{
		return _areaBlocks24.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks24(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks24.Add(elementId, value);
		_modificationsAreaBlocks24.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 25, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 25, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks24(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks24[elementId] = value;
		_modificationsAreaBlocks24.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 25, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 25, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks24(short elementId, DataContext context)
	{
		_areaBlocks24.Remove(elementId);
		_modificationsAreaBlocks24.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 25, elementId);
	}

	private void ClearAreaBlocks24(DataContext context)
	{
		_areaBlocks24.Clear();
		_modificationsAreaBlocks24.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 25);
	}

	public MapBlockData GetElement_AreaBlocks25(short elementId)
	{
		return _areaBlocks25[elementId];
	}

	public bool TryGetElement_AreaBlocks25(short elementId, out MapBlockData value)
	{
		return _areaBlocks25.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks25(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks25.Add(elementId, value);
		_modificationsAreaBlocks25.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 26, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 26, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks25(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks25[elementId] = value;
		_modificationsAreaBlocks25.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 26, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 26, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks25(short elementId, DataContext context)
	{
		_areaBlocks25.Remove(elementId);
		_modificationsAreaBlocks25.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 26, elementId);
	}

	private void ClearAreaBlocks25(DataContext context)
	{
		_areaBlocks25.Clear();
		_modificationsAreaBlocks25.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 26);
	}

	public MapBlockData GetElement_AreaBlocks26(short elementId)
	{
		return _areaBlocks26[elementId];
	}

	public bool TryGetElement_AreaBlocks26(short elementId, out MapBlockData value)
	{
		return _areaBlocks26.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks26(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks26.Add(elementId, value);
		_modificationsAreaBlocks26.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 27, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 27, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks26(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks26[elementId] = value;
		_modificationsAreaBlocks26.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 27, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 27, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks26(short elementId, DataContext context)
	{
		_areaBlocks26.Remove(elementId);
		_modificationsAreaBlocks26.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 27, elementId);
	}

	private void ClearAreaBlocks26(DataContext context)
	{
		_areaBlocks26.Clear();
		_modificationsAreaBlocks26.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 27);
	}

	public MapBlockData GetElement_AreaBlocks27(short elementId)
	{
		return _areaBlocks27[elementId];
	}

	public bool TryGetElement_AreaBlocks27(short elementId, out MapBlockData value)
	{
		return _areaBlocks27.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks27(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks27.Add(elementId, value);
		_modificationsAreaBlocks27.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 28, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 28, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks27(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks27[elementId] = value;
		_modificationsAreaBlocks27.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 28, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 28, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks27(short elementId, DataContext context)
	{
		_areaBlocks27.Remove(elementId);
		_modificationsAreaBlocks27.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 28, elementId);
	}

	private void ClearAreaBlocks27(DataContext context)
	{
		_areaBlocks27.Clear();
		_modificationsAreaBlocks27.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 28);
	}

	public MapBlockData GetElement_AreaBlocks28(short elementId)
	{
		return _areaBlocks28[elementId];
	}

	public bool TryGetElement_AreaBlocks28(short elementId, out MapBlockData value)
	{
		return _areaBlocks28.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks28(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks28.Add(elementId, value);
		_modificationsAreaBlocks28.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(29, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 29, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 29, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks28(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks28[elementId] = value;
		_modificationsAreaBlocks28.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(29, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 29, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 29, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks28(short elementId, DataContext context)
	{
		_areaBlocks28.Remove(elementId);
		_modificationsAreaBlocks28.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(29, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 29, elementId);
	}

	private void ClearAreaBlocks28(DataContext context)
	{
		_areaBlocks28.Clear();
		_modificationsAreaBlocks28.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(29, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 29);
	}

	public MapBlockData GetElement_AreaBlocks29(short elementId)
	{
		return _areaBlocks29[elementId];
	}

	public bool TryGetElement_AreaBlocks29(short elementId, out MapBlockData value)
	{
		return _areaBlocks29.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks29(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks29.Add(elementId, value);
		_modificationsAreaBlocks29.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(30, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 30, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 30, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks29(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks29[elementId] = value;
		_modificationsAreaBlocks29.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(30, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 30, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 30, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks29(short elementId, DataContext context)
	{
		_areaBlocks29.Remove(elementId);
		_modificationsAreaBlocks29.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(30, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 30, elementId);
	}

	private void ClearAreaBlocks29(DataContext context)
	{
		_areaBlocks29.Clear();
		_modificationsAreaBlocks29.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(30, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 30);
	}

	public MapBlockData GetElement_AreaBlocks30(short elementId)
	{
		return _areaBlocks30[elementId];
	}

	public bool TryGetElement_AreaBlocks30(short elementId, out MapBlockData value)
	{
		return _areaBlocks30.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks30(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks30.Add(elementId, value);
		_modificationsAreaBlocks30.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(31, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 31, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 31, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks30(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks30[elementId] = value;
		_modificationsAreaBlocks30.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(31, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 31, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 31, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks30(short elementId, DataContext context)
	{
		_areaBlocks30.Remove(elementId);
		_modificationsAreaBlocks30.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(31, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 31, elementId);
	}

	private void ClearAreaBlocks30(DataContext context)
	{
		_areaBlocks30.Clear();
		_modificationsAreaBlocks30.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(31, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 31);
	}

	public MapBlockData GetElement_AreaBlocks31(short elementId)
	{
		return _areaBlocks31[elementId];
	}

	public bool TryGetElement_AreaBlocks31(short elementId, out MapBlockData value)
	{
		return _areaBlocks31.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks31(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks31.Add(elementId, value);
		_modificationsAreaBlocks31.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(32, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 32, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 32, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks31(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks31[elementId] = value;
		_modificationsAreaBlocks31.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(32, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 32, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 32, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks31(short elementId, DataContext context)
	{
		_areaBlocks31.Remove(elementId);
		_modificationsAreaBlocks31.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(32, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 32, elementId);
	}

	private void ClearAreaBlocks31(DataContext context)
	{
		_areaBlocks31.Clear();
		_modificationsAreaBlocks31.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(32, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 32);
	}

	public MapBlockData GetElement_AreaBlocks32(short elementId)
	{
		return _areaBlocks32[elementId];
	}

	public bool TryGetElement_AreaBlocks32(short elementId, out MapBlockData value)
	{
		return _areaBlocks32.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks32(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks32.Add(elementId, value);
		_modificationsAreaBlocks32.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(33, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 33, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 33, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks32(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks32[elementId] = value;
		_modificationsAreaBlocks32.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(33, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 33, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 33, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks32(short elementId, DataContext context)
	{
		_areaBlocks32.Remove(elementId);
		_modificationsAreaBlocks32.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(33, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 33, elementId);
	}

	private void ClearAreaBlocks32(DataContext context)
	{
		_areaBlocks32.Clear();
		_modificationsAreaBlocks32.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(33, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 33);
	}

	public MapBlockData GetElement_AreaBlocks33(short elementId)
	{
		return _areaBlocks33[elementId];
	}

	public bool TryGetElement_AreaBlocks33(short elementId, out MapBlockData value)
	{
		return _areaBlocks33.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks33(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks33.Add(elementId, value);
		_modificationsAreaBlocks33.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(34, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 34, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 34, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks33(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks33[elementId] = value;
		_modificationsAreaBlocks33.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(34, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 34, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 34, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks33(short elementId, DataContext context)
	{
		_areaBlocks33.Remove(elementId);
		_modificationsAreaBlocks33.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(34, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 34, elementId);
	}

	private void ClearAreaBlocks33(DataContext context)
	{
		_areaBlocks33.Clear();
		_modificationsAreaBlocks33.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(34, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 34);
	}

	public MapBlockData GetElement_AreaBlocks34(short elementId)
	{
		return _areaBlocks34[elementId];
	}

	public bool TryGetElement_AreaBlocks34(short elementId, out MapBlockData value)
	{
		return _areaBlocks34.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks34(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks34.Add(elementId, value);
		_modificationsAreaBlocks34.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(35, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 35, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 35, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks34(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks34[elementId] = value;
		_modificationsAreaBlocks34.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(35, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 35, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 35, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks34(short elementId, DataContext context)
	{
		_areaBlocks34.Remove(elementId);
		_modificationsAreaBlocks34.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(35, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 35, elementId);
	}

	private void ClearAreaBlocks34(DataContext context)
	{
		_areaBlocks34.Clear();
		_modificationsAreaBlocks34.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(35, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 35);
	}

	public MapBlockData GetElement_AreaBlocks35(short elementId)
	{
		return _areaBlocks35[elementId];
	}

	public bool TryGetElement_AreaBlocks35(short elementId, out MapBlockData value)
	{
		return _areaBlocks35.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks35(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks35.Add(elementId, value);
		_modificationsAreaBlocks35.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(36, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 36, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 36, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks35(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks35[elementId] = value;
		_modificationsAreaBlocks35.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(36, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 36, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 36, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks35(short elementId, DataContext context)
	{
		_areaBlocks35.Remove(elementId);
		_modificationsAreaBlocks35.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(36, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 36, elementId);
	}

	private void ClearAreaBlocks35(DataContext context)
	{
		_areaBlocks35.Clear();
		_modificationsAreaBlocks35.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(36, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 36);
	}

	public MapBlockData GetElement_AreaBlocks36(short elementId)
	{
		return _areaBlocks36[elementId];
	}

	public bool TryGetElement_AreaBlocks36(short elementId, out MapBlockData value)
	{
		return _areaBlocks36.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks36(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks36.Add(elementId, value);
		_modificationsAreaBlocks36.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 37, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 37, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks36(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks36[elementId] = value;
		_modificationsAreaBlocks36.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 37, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 37, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks36(short elementId, DataContext context)
	{
		_areaBlocks36.Remove(elementId);
		_modificationsAreaBlocks36.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 37, elementId);
	}

	private void ClearAreaBlocks36(DataContext context)
	{
		_areaBlocks36.Clear();
		_modificationsAreaBlocks36.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(37, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 37);
	}

	public MapBlockData GetElement_AreaBlocks37(short elementId)
	{
		return _areaBlocks37[elementId];
	}

	public bool TryGetElement_AreaBlocks37(short elementId, out MapBlockData value)
	{
		return _areaBlocks37.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks37(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks37.Add(elementId, value);
		_modificationsAreaBlocks37.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(38, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 38, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 38, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks37(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks37[elementId] = value;
		_modificationsAreaBlocks37.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(38, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 38, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 38, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks37(short elementId, DataContext context)
	{
		_areaBlocks37.Remove(elementId);
		_modificationsAreaBlocks37.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(38, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 38, elementId);
	}

	private void ClearAreaBlocks37(DataContext context)
	{
		_areaBlocks37.Clear();
		_modificationsAreaBlocks37.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(38, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 38);
	}

	public MapBlockData GetElement_AreaBlocks38(short elementId)
	{
		return _areaBlocks38[elementId];
	}

	public bool TryGetElement_AreaBlocks38(short elementId, out MapBlockData value)
	{
		return _areaBlocks38.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks38(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks38.Add(elementId, value);
		_modificationsAreaBlocks38.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(39, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 39, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 39, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks38(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks38[elementId] = value;
		_modificationsAreaBlocks38.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(39, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 39, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 39, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks38(short elementId, DataContext context)
	{
		_areaBlocks38.Remove(elementId);
		_modificationsAreaBlocks38.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(39, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 39, elementId);
	}

	private void ClearAreaBlocks38(DataContext context)
	{
		_areaBlocks38.Clear();
		_modificationsAreaBlocks38.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(39, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 39);
	}

	public MapBlockData GetElement_AreaBlocks39(short elementId)
	{
		return _areaBlocks39[elementId];
	}

	public bool TryGetElement_AreaBlocks39(short elementId, out MapBlockData value)
	{
		return _areaBlocks39.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks39(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks39.Add(elementId, value);
		_modificationsAreaBlocks39.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(40, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 40, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 40, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks39(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks39[elementId] = value;
		_modificationsAreaBlocks39.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(40, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 40, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 40, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks39(short elementId, DataContext context)
	{
		_areaBlocks39.Remove(elementId);
		_modificationsAreaBlocks39.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(40, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 40, elementId);
	}

	private void ClearAreaBlocks39(DataContext context)
	{
		_areaBlocks39.Clear();
		_modificationsAreaBlocks39.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(40, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 40);
	}

	public MapBlockData GetElement_AreaBlocks40(short elementId)
	{
		return _areaBlocks40[elementId];
	}

	public bool TryGetElement_AreaBlocks40(short elementId, out MapBlockData value)
	{
		return _areaBlocks40.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks40(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks40.Add(elementId, value);
		_modificationsAreaBlocks40.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(41, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 41, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 41, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks40(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks40[elementId] = value;
		_modificationsAreaBlocks40.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(41, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 41, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 41, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks40(short elementId, DataContext context)
	{
		_areaBlocks40.Remove(elementId);
		_modificationsAreaBlocks40.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(41, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 41, elementId);
	}

	private void ClearAreaBlocks40(DataContext context)
	{
		_areaBlocks40.Clear();
		_modificationsAreaBlocks40.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(41, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 41);
	}

	public MapBlockData GetElement_AreaBlocks41(short elementId)
	{
		return _areaBlocks41[elementId];
	}

	public bool TryGetElement_AreaBlocks41(short elementId, out MapBlockData value)
	{
		return _areaBlocks41.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks41(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks41.Add(elementId, value);
		_modificationsAreaBlocks41.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(42, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 42, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 42, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks41(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks41[elementId] = value;
		_modificationsAreaBlocks41.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(42, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 42, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 42, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks41(short elementId, DataContext context)
	{
		_areaBlocks41.Remove(elementId);
		_modificationsAreaBlocks41.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(42, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 42, elementId);
	}

	private void ClearAreaBlocks41(DataContext context)
	{
		_areaBlocks41.Clear();
		_modificationsAreaBlocks41.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(42, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 42);
	}

	public MapBlockData GetElement_AreaBlocks42(short elementId)
	{
		return _areaBlocks42[elementId];
	}

	public bool TryGetElement_AreaBlocks42(short elementId, out MapBlockData value)
	{
		return _areaBlocks42.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks42(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks42.Add(elementId, value);
		_modificationsAreaBlocks42.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 43, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 43, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks42(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks42[elementId] = value;
		_modificationsAreaBlocks42.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 43, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 43, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks42(short elementId, DataContext context)
	{
		_areaBlocks42.Remove(elementId);
		_modificationsAreaBlocks42.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 43, elementId);
	}

	private void ClearAreaBlocks42(DataContext context)
	{
		_areaBlocks42.Clear();
		_modificationsAreaBlocks42.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(43, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 43);
	}

	public MapBlockData GetElement_AreaBlocks43(short elementId)
	{
		return _areaBlocks43[elementId];
	}

	public bool TryGetElement_AreaBlocks43(short elementId, out MapBlockData value)
	{
		return _areaBlocks43.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks43(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks43.Add(elementId, value);
		_modificationsAreaBlocks43.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 44, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 44, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks43(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks43[elementId] = value;
		_modificationsAreaBlocks43.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 44, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 44, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks43(short elementId, DataContext context)
	{
		_areaBlocks43.Remove(elementId);
		_modificationsAreaBlocks43.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 44, elementId);
	}

	private void ClearAreaBlocks43(DataContext context)
	{
		_areaBlocks43.Clear();
		_modificationsAreaBlocks43.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(44, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 44);
	}

	public MapBlockData GetElement_AreaBlocks44(short elementId)
	{
		return _areaBlocks44[elementId];
	}

	public bool TryGetElement_AreaBlocks44(short elementId, out MapBlockData value)
	{
		return _areaBlocks44.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_AreaBlocks44(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks44.Add(elementId, value);
		_modificationsAreaBlocks44.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 45, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 45, elementId, 0);
		}
	}

	private unsafe void SetElement_AreaBlocks44(short elementId, MapBlockData value, DataContext context)
	{
		_areaBlocks44[elementId] = value;
		_modificationsAreaBlocks44.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 45, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 45, elementId, 0);
		}
	}

	private void RemoveElement_AreaBlocks44(short elementId, DataContext context)
	{
		_areaBlocks44.Remove(elementId);
		_modificationsAreaBlocks44.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 45, elementId);
	}

	private void ClearAreaBlocks44(DataContext context)
	{
		_areaBlocks44.Clear();
		_modificationsAreaBlocks44.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(45, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 45);
	}

	public MapBlockData GetElement_BrokenAreaBlocks(short elementId)
	{
		return _brokenAreaBlocks[elementId];
	}

	public bool TryGetElement_BrokenAreaBlocks(short elementId, out MapBlockData value)
	{
		return _brokenAreaBlocks.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_BrokenAreaBlocks(short elementId, MapBlockData value, DataContext context)
	{
		_brokenAreaBlocks.Add(elementId, value);
		_modificationsBrokenAreaBlocks.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(46, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 46, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 46, elementId, 0);
		}
	}

	private unsafe void SetElement_BrokenAreaBlocks(short elementId, MapBlockData value, DataContext context)
	{
		_brokenAreaBlocks[elementId] = value;
		_modificationsBrokenAreaBlocks.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(46, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 46, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 46, elementId, 0);
		}
	}

	private void RemoveElement_BrokenAreaBlocks(short elementId, DataContext context)
	{
		_brokenAreaBlocks.Remove(elementId);
		_modificationsBrokenAreaBlocks.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(46, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 46, elementId);
	}

	private void ClearBrokenAreaBlocks(DataContext context)
	{
		_brokenAreaBlocks.Clear();
		_modificationsBrokenAreaBlocks.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(46, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 46);
	}

	public MapBlockData GetElement_BornAreaBlocks(short elementId)
	{
		return _bornAreaBlocks[elementId];
	}

	public bool TryGetElement_BornAreaBlocks(short elementId, out MapBlockData value)
	{
		return _bornAreaBlocks.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_BornAreaBlocks(short elementId, MapBlockData value, DataContext context)
	{
		_bornAreaBlocks.Add(elementId, value);
		_modificationsBornAreaBlocks.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(47, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 47, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 47, elementId, 0);
		}
	}

	private unsafe void SetElement_BornAreaBlocks(short elementId, MapBlockData value, DataContext context)
	{
		_bornAreaBlocks[elementId] = value;
		_modificationsBornAreaBlocks.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(47, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 47, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 47, elementId, 0);
		}
	}

	private void RemoveElement_BornAreaBlocks(short elementId, DataContext context)
	{
		_bornAreaBlocks.Remove(elementId);
		_modificationsBornAreaBlocks.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(47, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 47, elementId);
	}

	private void ClearBornAreaBlocks(DataContext context)
	{
		_bornAreaBlocks.Clear();
		_modificationsBornAreaBlocks.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(47, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 47);
	}

	public MapBlockData GetElement_GuideAreaBlocks(short elementId)
	{
		return _guideAreaBlocks[elementId];
	}

	public bool TryGetElement_GuideAreaBlocks(short elementId, out MapBlockData value)
	{
		return _guideAreaBlocks.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_GuideAreaBlocks(short elementId, MapBlockData value, DataContext context)
	{
		_guideAreaBlocks.Add(elementId, value);
		_modificationsGuideAreaBlocks.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(48, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 48, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 48, elementId, 0);
		}
	}

	private unsafe void SetElement_GuideAreaBlocks(short elementId, MapBlockData value, DataContext context)
	{
		_guideAreaBlocks[elementId] = value;
		_modificationsGuideAreaBlocks.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(48, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 48, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 48, elementId, 0);
		}
	}

	private void RemoveElement_GuideAreaBlocks(short elementId, DataContext context)
	{
		_guideAreaBlocks.Remove(elementId);
		_modificationsGuideAreaBlocks.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(48, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 48, elementId);
	}

	private void ClearGuideAreaBlocks(DataContext context)
	{
		_guideAreaBlocks.Clear();
		_modificationsGuideAreaBlocks.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(48, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 48);
	}

	public MapBlockData GetElement_SecretVillageAreaBlocks(short elementId)
	{
		return _secretVillageAreaBlocks[elementId];
	}

	public bool TryGetElement_SecretVillageAreaBlocks(short elementId, out MapBlockData value)
	{
		return _secretVillageAreaBlocks.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_SecretVillageAreaBlocks(short elementId, MapBlockData value, DataContext context)
	{
		_secretVillageAreaBlocks.Add(elementId, value);
		_modificationsSecretVillageAreaBlocks.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(49, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 49, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 49, elementId, 0);
		}
	}

	private unsafe void SetElement_SecretVillageAreaBlocks(short elementId, MapBlockData value, DataContext context)
	{
		_secretVillageAreaBlocks[elementId] = value;
		_modificationsSecretVillageAreaBlocks.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(49, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 49, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 49, elementId, 0);
		}
	}

	private void RemoveElement_SecretVillageAreaBlocks(short elementId, DataContext context)
	{
		_secretVillageAreaBlocks.Remove(elementId);
		_modificationsSecretVillageAreaBlocks.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(49, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 49, elementId);
	}

	private void ClearSecretVillageAreaBlocks(DataContext context)
	{
		_secretVillageAreaBlocks.Clear();
		_modificationsSecretVillageAreaBlocks.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(49, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 49);
	}

	public MapBlockData GetElement_BrokenPerformAreaBlocks(short elementId)
	{
		return _brokenPerformAreaBlocks[elementId];
	}

	public bool TryGetElement_BrokenPerformAreaBlocks(short elementId, out MapBlockData value)
	{
		return _brokenPerformAreaBlocks.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_BrokenPerformAreaBlocks(short elementId, MapBlockData value, DataContext context)
	{
		_brokenPerformAreaBlocks.Add(elementId, value);
		_modificationsBrokenPerformAreaBlocks.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(50, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 50, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 50, elementId, 0);
		}
	}

	private unsafe void SetElement_BrokenPerformAreaBlocks(short elementId, MapBlockData value, DataContext context)
	{
		_brokenPerformAreaBlocks[elementId] = value;
		_modificationsBrokenPerformAreaBlocks.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(50, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 50, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 50, elementId, 0);
		}
	}

	private void RemoveElement_BrokenPerformAreaBlocks(short elementId, DataContext context)
	{
		_brokenPerformAreaBlocks.Remove(elementId);
		_modificationsBrokenPerformAreaBlocks.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(50, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 50, elementId);
	}

	private void ClearBrokenPerformAreaBlocks(DataContext context)
	{
		_brokenPerformAreaBlocks.Clear();
		_modificationsBrokenPerformAreaBlocks.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(50, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 50);
	}

	public TravelRoute GetElement_TravelRouteDict(TravelRouteKey elementId)
	{
		return _travelRouteDict[elementId];
	}

	public bool TryGetElement_TravelRouteDict(TravelRouteKey elementId, out TravelRoute value)
	{
		return _travelRouteDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_TravelRouteDict(TravelRouteKey elementId, TravelRoute value, DataContext context)
	{
		_travelRouteDict.Add(elementId, value);
		_modificationsTravelRouteDict.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(51, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 51, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 51, elementId, 0);
		}
	}

	private unsafe void SetElement_TravelRouteDict(TravelRouteKey elementId, TravelRoute value, DataContext context)
	{
		_travelRouteDict[elementId] = value;
		_modificationsTravelRouteDict.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(51, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 51, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 51, elementId, 0);
		}
	}

	private void RemoveElement_TravelRouteDict(TravelRouteKey elementId, DataContext context)
	{
		_travelRouteDict.Remove(elementId);
		_modificationsTravelRouteDict.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(51, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 51, elementId);
	}

	private void ClearTravelRouteDict(DataContext context)
	{
		_travelRouteDict.Clear();
		_modificationsTravelRouteDict.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(51, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 51);
	}

	public TravelRoute GetElement_BornStateTravelRouteDict(TravelRouteKey elementId)
	{
		return _bornStateTravelRouteDict[elementId];
	}

	public bool TryGetElement_BornStateTravelRouteDict(TravelRouteKey elementId, out TravelRoute value)
	{
		return _bornStateTravelRouteDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_BornStateTravelRouteDict(TravelRouteKey elementId, TravelRoute value, DataContext context)
	{
		_bornStateTravelRouteDict.Add(elementId, value);
		_modificationsBornStateTravelRouteDict.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(52, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 52, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(2, 52, elementId, 0);
		}
	}

	private unsafe void SetElement_BornStateTravelRouteDict(TravelRouteKey elementId, TravelRoute value, DataContext context)
	{
		_bornStateTravelRouteDict[elementId] = value;
		_modificationsBornStateTravelRouteDict.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(52, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 52, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(2, 52, elementId, 0);
		}
	}

	private void RemoveElement_BornStateTravelRouteDict(TravelRouteKey elementId, DataContext context)
	{
		_bornStateTravelRouteDict.Remove(elementId);
		_modificationsBornStateTravelRouteDict.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(52, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 52, elementId);
	}

	private void ClearBornStateTravelRouteDict(DataContext context)
	{
		_bornStateTravelRouteDict.Clear();
		_modificationsBornStateTravelRouteDict.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(52, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 52);
	}

	[Obsolete("DomainData _animalPlaceData is no longer in use.")]
	public AnimalPlaceData GetElement_AnimalPlaceData(int index)
	{
		return _animalPlaceData[index];
	}

	[Obsolete("DomainData _animalPlaceData is no longer in use.")]
	private unsafe void SetElement_AnimalPlaceData(int index, AnimalPlaceData value, DataContext context)
	{
		_animalPlaceData[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesAnimalPlaceData, CacheInfluencesAnimalPlaceData, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicElementList_Set(2, 53, index, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicElementList_Set(2, 53, index, 0);
		}
	}

	public CricketPlaceData GetElement_CricketPlaceData(int index)
	{
		return _cricketPlaceData[index];
	}

	private unsafe void SetElement_CricketPlaceData(int index, CricketPlaceData value, DataContext context)
	{
		_cricketPlaceData[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesCricketPlaceData, CacheInfluencesCricketPlaceData, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicElementList_Set(2, 54, index, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicElementList_Set(2, 54, index, 0);
		}
	}

	private GameData.Utilities.ShortList GetElement_RegularAreaNearList(short elementId)
	{
		return _regularAreaNearList[elementId];
	}

	private bool TryGetElement_RegularAreaNearList(short elementId, out GameData.Utilities.ShortList value)
	{
		return _regularAreaNearList.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_RegularAreaNearList(short elementId, GameData.Utilities.ShortList value, DataContext context)
	{
		_regularAreaNearList.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(55, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(2, 55, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private unsafe void SetElement_RegularAreaNearList(short elementId, GameData.Utilities.ShortList value, DataContext context)
	{
		_regularAreaNearList[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(55, DataStates, CacheInfluences, context);
		int serializedSize = value.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(2, 55, elementId, serializedSize);
		ptr += value.Serialize(ptr);
	}

	private void RemoveElement_RegularAreaNearList(short elementId, DataContext context)
	{
		_regularAreaNearList.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(55, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(2, 55, elementId);
	}

	private void ClearRegularAreaNearList(DataContext context)
	{
		_regularAreaNearList.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(55, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(2, 55);
	}

	public Location GetElement_SwordTombLocations(int index)
	{
		return _swordTombLocations[index];
	}

	public unsafe void SetElement_SwordTombLocations(int index, Location value, DataContext context)
	{
		_swordTombLocations[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesSwordTombLocations, CacheInfluencesSwordTombLocations, context);
		byte* ptr = OperationAdder.FixedElementList_Set(2, 56, index, 4);
		ptr += value.Serialize(ptr);
	}

	public CrossAreaMoveInfo GetTravelInfo()
	{
		return _travelInfo;
	}

	private unsafe void SetTravelInfo(CrossAreaMoveInfo value, DataContext context)
	{
		_travelInfo = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(57, DataStates, CacheInfluences, context);
		int serializedSize = _travelInfo.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValue_Set(2, 57, serializedSize);
		ptr += _travelInfo.Serialize(ptr);
	}

	public bool GetOnHandlingTravelingEventBlock()
	{
		return _onHandlingTravelingEventBlock;
	}

	public void SetOnHandlingTravelingEventBlock(bool value, DataContext context)
	{
		_onHandlingTravelingEventBlock = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(58, DataStates, CacheInfluences, context);
	}

	public List<HunterAnimalKey> GetHunterAnimalsCache()
	{
		return _hunterAnimalsCache;
	}

	private void SetHunterAnimalsCache(List<HunterAnimalKey> value, DataContext context)
	{
		_hunterAnimalsCache = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(59, DataStates, CacheInfluences, context);
	}

	public int GetMoveBanned()
	{
		return _moveBanned;
	}

	private void SetMoveBanned(int value, DataContext context)
	{
		_moveBanned = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(60, DataStates, CacheInfluences, context);
	}

	public bool GetCrossArchiveLockMoveTime()
	{
		return _crossArchiveLockMoveTime;
	}

	public void SetCrossArchiveLockMoveTime(bool value, DataContext context)
	{
		_crossArchiveLockMoveTime = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(61, DataStates, CacheInfluences, context);
	}

	public List<Location> GetFleeBeasts()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 62))
		{
			return _fleeBeasts;
		}
		List<Location> list = new List<Location>();
		CalcFleeBeasts(list);
		bool lockTaken = false;
		try
		{
			_spinLockFleeBeasts.Enter(ref lockTaken);
			_fleeBeasts.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 62);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockFleeBeasts.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _fleeBeasts;
	}

	public List<Location> GetFleeLoongs()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 63))
		{
			return _fleeLoongs;
		}
		List<Location> list = new List<Location>();
		CalcFleeLoongs(list);
		bool lockTaken = false;
		try
		{
			_spinLockFleeLoongs.Enter(ref lockTaken);
			_fleeLoongs.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 63);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockFleeLoongs.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _fleeLoongs;
	}

	public List<LoongLocationData> GetLoongLocations()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 64))
		{
			return _loongLocations;
		}
		List<LoongLocationData> list = new List<LoongLocationData>();
		CalcLoongLocations(list);
		bool lockTaken = false;
		try
		{
			_spinLockLoongLocations.Enter(ref lockTaken);
			_loongLocations.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 64);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockLoongLocations.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _loongLocations;
	}

	public List<Location> GetAlterSettlementLocations()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 65))
		{
			return _alterSettlementLocations;
		}
		List<Location> list = new List<Location>();
		CalcAlterSettlementLocations(list);
		bool lockTaken = false;
		try
		{
			_spinLockAlterSettlementLocations.Enter(ref lockTaken);
			_alterSettlementLocations.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 65);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockAlterSettlementLocations.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _alterSettlementLocations;
	}

	public bool GetIsTaiwuInFulongFlameArea()
	{
		return _isTaiwuInFulongFlameArea;
	}

	public void SetIsTaiwuInFulongFlameArea(bool value, DataContext context)
	{
		_isTaiwuInFulongFlameArea = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(66, DataStates, CacheInfluences, context);
	}

	public List<MapElementPickupDisplayData> GetVisibleMapPickups()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 67))
		{
			return _visibleMapPickups;
		}
		List<MapElementPickupDisplayData> list = new List<MapElementPickupDisplayData>();
		CalcVisibleMapPickups(list);
		bool lockTaken = false;
		try
		{
			_spinLockVisibleMapPickups.Enter(ref lockTaken);
			_visibleMapPickups.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 67);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockVisibleMapPickups.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _visibleMapPickups;
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		int num = 0;
		for (int i = 0; i < 139; i++)
		{
			MapAreaData mapAreaData = _areas[i];
			num = ((mapAreaData == null) ? (num + 4) : (num + (4 + mapAreaData.GetSerializedSize())));
		}
		byte* ptr = OperationAdder.DynamicElementList_InsertRange(2, 0, 0, 139, num);
		for (int j = 0; j < 139; j++)
		{
			MapAreaData mapAreaData2 = _areas[j];
			if (mapAreaData2 != null)
			{
				byte* ptr2 = ptr;
				ptr += 4;
				int num2 = mapAreaData2.Serialize(ptr);
				ptr += num2;
				*(int*)ptr2 = num2;
			}
			else
			{
				*(int*)ptr = 0;
				ptr += 4;
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item in _areaBlocks0)
		{
			short key = item.Key;
			MapBlockData value = item.Value;
			if (value != null)
			{
				int serializedSize = value.GetSerializedSize();
				byte* ptr3 = OperationAdder.DynamicSingleValueCollection_Add(2, 1, key, serializedSize);
				ptr3 += value.Serialize(ptr3);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 1, key, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item2 in _areaBlocks1)
		{
			short key2 = item2.Key;
			MapBlockData value2 = item2.Value;
			if (value2 != null)
			{
				int serializedSize2 = value2.GetSerializedSize();
				byte* ptr4 = OperationAdder.DynamicSingleValueCollection_Add(2, 2, key2, serializedSize2);
				ptr4 += value2.Serialize(ptr4);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 2, key2, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item3 in _areaBlocks2)
		{
			short key3 = item3.Key;
			MapBlockData value3 = item3.Value;
			if (value3 != null)
			{
				int serializedSize3 = value3.GetSerializedSize();
				byte* ptr5 = OperationAdder.DynamicSingleValueCollection_Add(2, 3, key3, serializedSize3);
				ptr5 += value3.Serialize(ptr5);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 3, key3, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item4 in _areaBlocks3)
		{
			short key4 = item4.Key;
			MapBlockData value4 = item4.Value;
			if (value4 != null)
			{
				int serializedSize4 = value4.GetSerializedSize();
				byte* ptr6 = OperationAdder.DynamicSingleValueCollection_Add(2, 4, key4, serializedSize4);
				ptr6 += value4.Serialize(ptr6);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 4, key4, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item5 in _areaBlocks4)
		{
			short key5 = item5.Key;
			MapBlockData value5 = item5.Value;
			if (value5 != null)
			{
				int serializedSize5 = value5.GetSerializedSize();
				byte* ptr7 = OperationAdder.DynamicSingleValueCollection_Add(2, 5, key5, serializedSize5);
				ptr7 += value5.Serialize(ptr7);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 5, key5, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item6 in _areaBlocks5)
		{
			short key6 = item6.Key;
			MapBlockData value6 = item6.Value;
			if (value6 != null)
			{
				int serializedSize6 = value6.GetSerializedSize();
				byte* ptr8 = OperationAdder.DynamicSingleValueCollection_Add(2, 6, key6, serializedSize6);
				ptr8 += value6.Serialize(ptr8);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 6, key6, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item7 in _areaBlocks6)
		{
			short key7 = item7.Key;
			MapBlockData value7 = item7.Value;
			if (value7 != null)
			{
				int serializedSize7 = value7.GetSerializedSize();
				byte* ptr9 = OperationAdder.DynamicSingleValueCollection_Add(2, 7, key7, serializedSize7);
				ptr9 += value7.Serialize(ptr9);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 7, key7, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item8 in _areaBlocks7)
		{
			short key8 = item8.Key;
			MapBlockData value8 = item8.Value;
			if (value8 != null)
			{
				int serializedSize8 = value8.GetSerializedSize();
				byte* ptr10 = OperationAdder.DynamicSingleValueCollection_Add(2, 8, key8, serializedSize8);
				ptr10 += value8.Serialize(ptr10);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 8, key8, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item9 in _areaBlocks8)
		{
			short key9 = item9.Key;
			MapBlockData value9 = item9.Value;
			if (value9 != null)
			{
				int serializedSize9 = value9.GetSerializedSize();
				byte* ptr11 = OperationAdder.DynamicSingleValueCollection_Add(2, 9, key9, serializedSize9);
				ptr11 += value9.Serialize(ptr11);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 9, key9, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item10 in _areaBlocks9)
		{
			short key10 = item10.Key;
			MapBlockData value10 = item10.Value;
			if (value10 != null)
			{
				int serializedSize10 = value10.GetSerializedSize();
				byte* ptr12 = OperationAdder.DynamicSingleValueCollection_Add(2, 10, key10, serializedSize10);
				ptr12 += value10.Serialize(ptr12);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 10, key10, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item11 in _areaBlocks10)
		{
			short key11 = item11.Key;
			MapBlockData value11 = item11.Value;
			if (value11 != null)
			{
				int serializedSize11 = value11.GetSerializedSize();
				byte* ptr13 = OperationAdder.DynamicSingleValueCollection_Add(2, 11, key11, serializedSize11);
				ptr13 += value11.Serialize(ptr13);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 11, key11, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item12 in _areaBlocks11)
		{
			short key12 = item12.Key;
			MapBlockData value12 = item12.Value;
			if (value12 != null)
			{
				int serializedSize12 = value12.GetSerializedSize();
				byte* ptr14 = OperationAdder.DynamicSingleValueCollection_Add(2, 12, key12, serializedSize12);
				ptr14 += value12.Serialize(ptr14);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 12, key12, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item13 in _areaBlocks12)
		{
			short key13 = item13.Key;
			MapBlockData value13 = item13.Value;
			if (value13 != null)
			{
				int serializedSize13 = value13.GetSerializedSize();
				byte* ptr15 = OperationAdder.DynamicSingleValueCollection_Add(2, 13, key13, serializedSize13);
				ptr15 += value13.Serialize(ptr15);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 13, key13, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item14 in _areaBlocks13)
		{
			short key14 = item14.Key;
			MapBlockData value14 = item14.Value;
			if (value14 != null)
			{
				int serializedSize14 = value14.GetSerializedSize();
				byte* ptr16 = OperationAdder.DynamicSingleValueCollection_Add(2, 14, key14, serializedSize14);
				ptr16 += value14.Serialize(ptr16);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 14, key14, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item15 in _areaBlocks14)
		{
			short key15 = item15.Key;
			MapBlockData value15 = item15.Value;
			if (value15 != null)
			{
				int serializedSize15 = value15.GetSerializedSize();
				byte* ptr17 = OperationAdder.DynamicSingleValueCollection_Add(2, 15, key15, serializedSize15);
				ptr17 += value15.Serialize(ptr17);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 15, key15, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item16 in _areaBlocks15)
		{
			short key16 = item16.Key;
			MapBlockData value16 = item16.Value;
			if (value16 != null)
			{
				int serializedSize16 = value16.GetSerializedSize();
				byte* ptr18 = OperationAdder.DynamicSingleValueCollection_Add(2, 16, key16, serializedSize16);
				ptr18 += value16.Serialize(ptr18);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 16, key16, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item17 in _areaBlocks16)
		{
			short key17 = item17.Key;
			MapBlockData value17 = item17.Value;
			if (value17 != null)
			{
				int serializedSize17 = value17.GetSerializedSize();
				byte* ptr19 = OperationAdder.DynamicSingleValueCollection_Add(2, 17, key17, serializedSize17);
				ptr19 += value17.Serialize(ptr19);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 17, key17, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item18 in _areaBlocks17)
		{
			short key18 = item18.Key;
			MapBlockData value18 = item18.Value;
			if (value18 != null)
			{
				int serializedSize18 = value18.GetSerializedSize();
				byte* ptr20 = OperationAdder.DynamicSingleValueCollection_Add(2, 18, key18, serializedSize18);
				ptr20 += value18.Serialize(ptr20);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 18, key18, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item19 in _areaBlocks18)
		{
			short key19 = item19.Key;
			MapBlockData value19 = item19.Value;
			if (value19 != null)
			{
				int serializedSize19 = value19.GetSerializedSize();
				byte* ptr21 = OperationAdder.DynamicSingleValueCollection_Add(2, 19, key19, serializedSize19);
				ptr21 += value19.Serialize(ptr21);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 19, key19, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item20 in _areaBlocks19)
		{
			short key20 = item20.Key;
			MapBlockData value20 = item20.Value;
			if (value20 != null)
			{
				int serializedSize20 = value20.GetSerializedSize();
				byte* ptr22 = OperationAdder.DynamicSingleValueCollection_Add(2, 20, key20, serializedSize20);
				ptr22 += value20.Serialize(ptr22);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 20, key20, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item21 in _areaBlocks20)
		{
			short key21 = item21.Key;
			MapBlockData value21 = item21.Value;
			if (value21 != null)
			{
				int serializedSize21 = value21.GetSerializedSize();
				byte* ptr23 = OperationAdder.DynamicSingleValueCollection_Add(2, 21, key21, serializedSize21);
				ptr23 += value21.Serialize(ptr23);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 21, key21, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item22 in _areaBlocks21)
		{
			short key22 = item22.Key;
			MapBlockData value22 = item22.Value;
			if (value22 != null)
			{
				int serializedSize22 = value22.GetSerializedSize();
				byte* ptr24 = OperationAdder.DynamicSingleValueCollection_Add(2, 22, key22, serializedSize22);
				ptr24 += value22.Serialize(ptr24);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 22, key22, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item23 in _areaBlocks22)
		{
			short key23 = item23.Key;
			MapBlockData value23 = item23.Value;
			if (value23 != null)
			{
				int serializedSize23 = value23.GetSerializedSize();
				byte* ptr25 = OperationAdder.DynamicSingleValueCollection_Add(2, 23, key23, serializedSize23);
				ptr25 += value23.Serialize(ptr25);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 23, key23, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item24 in _areaBlocks23)
		{
			short key24 = item24.Key;
			MapBlockData value24 = item24.Value;
			if (value24 != null)
			{
				int serializedSize24 = value24.GetSerializedSize();
				byte* ptr26 = OperationAdder.DynamicSingleValueCollection_Add(2, 24, key24, serializedSize24);
				ptr26 += value24.Serialize(ptr26);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 24, key24, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item25 in _areaBlocks24)
		{
			short key25 = item25.Key;
			MapBlockData value25 = item25.Value;
			if (value25 != null)
			{
				int serializedSize25 = value25.GetSerializedSize();
				byte* ptr27 = OperationAdder.DynamicSingleValueCollection_Add(2, 25, key25, serializedSize25);
				ptr27 += value25.Serialize(ptr27);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 25, key25, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item26 in _areaBlocks25)
		{
			short key26 = item26.Key;
			MapBlockData value26 = item26.Value;
			if (value26 != null)
			{
				int serializedSize26 = value26.GetSerializedSize();
				byte* ptr28 = OperationAdder.DynamicSingleValueCollection_Add(2, 26, key26, serializedSize26);
				ptr28 += value26.Serialize(ptr28);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 26, key26, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item27 in _areaBlocks26)
		{
			short key27 = item27.Key;
			MapBlockData value27 = item27.Value;
			if (value27 != null)
			{
				int serializedSize27 = value27.GetSerializedSize();
				byte* ptr29 = OperationAdder.DynamicSingleValueCollection_Add(2, 27, key27, serializedSize27);
				ptr29 += value27.Serialize(ptr29);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 27, key27, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item28 in _areaBlocks27)
		{
			short key28 = item28.Key;
			MapBlockData value28 = item28.Value;
			if (value28 != null)
			{
				int serializedSize28 = value28.GetSerializedSize();
				byte* ptr30 = OperationAdder.DynamicSingleValueCollection_Add(2, 28, key28, serializedSize28);
				ptr30 += value28.Serialize(ptr30);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 28, key28, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item29 in _areaBlocks28)
		{
			short key29 = item29.Key;
			MapBlockData value29 = item29.Value;
			if (value29 != null)
			{
				int serializedSize29 = value29.GetSerializedSize();
				byte* ptr31 = OperationAdder.DynamicSingleValueCollection_Add(2, 29, key29, serializedSize29);
				ptr31 += value29.Serialize(ptr31);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 29, key29, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item30 in _areaBlocks29)
		{
			short key30 = item30.Key;
			MapBlockData value30 = item30.Value;
			if (value30 != null)
			{
				int serializedSize30 = value30.GetSerializedSize();
				byte* ptr32 = OperationAdder.DynamicSingleValueCollection_Add(2, 30, key30, serializedSize30);
				ptr32 += value30.Serialize(ptr32);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 30, key30, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item31 in _areaBlocks30)
		{
			short key31 = item31.Key;
			MapBlockData value31 = item31.Value;
			if (value31 != null)
			{
				int serializedSize31 = value31.GetSerializedSize();
				byte* ptr33 = OperationAdder.DynamicSingleValueCollection_Add(2, 31, key31, serializedSize31);
				ptr33 += value31.Serialize(ptr33);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 31, key31, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item32 in _areaBlocks31)
		{
			short key32 = item32.Key;
			MapBlockData value32 = item32.Value;
			if (value32 != null)
			{
				int serializedSize32 = value32.GetSerializedSize();
				byte* ptr34 = OperationAdder.DynamicSingleValueCollection_Add(2, 32, key32, serializedSize32);
				ptr34 += value32.Serialize(ptr34);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 32, key32, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item33 in _areaBlocks32)
		{
			short key33 = item33.Key;
			MapBlockData value33 = item33.Value;
			if (value33 != null)
			{
				int serializedSize33 = value33.GetSerializedSize();
				byte* ptr35 = OperationAdder.DynamicSingleValueCollection_Add(2, 33, key33, serializedSize33);
				ptr35 += value33.Serialize(ptr35);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 33, key33, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item34 in _areaBlocks33)
		{
			short key34 = item34.Key;
			MapBlockData value34 = item34.Value;
			if (value34 != null)
			{
				int serializedSize34 = value34.GetSerializedSize();
				byte* ptr36 = OperationAdder.DynamicSingleValueCollection_Add(2, 34, key34, serializedSize34);
				ptr36 += value34.Serialize(ptr36);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 34, key34, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item35 in _areaBlocks34)
		{
			short key35 = item35.Key;
			MapBlockData value35 = item35.Value;
			if (value35 != null)
			{
				int serializedSize35 = value35.GetSerializedSize();
				byte* ptr37 = OperationAdder.DynamicSingleValueCollection_Add(2, 35, key35, serializedSize35);
				ptr37 += value35.Serialize(ptr37);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 35, key35, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item36 in _areaBlocks35)
		{
			short key36 = item36.Key;
			MapBlockData value36 = item36.Value;
			if (value36 != null)
			{
				int serializedSize36 = value36.GetSerializedSize();
				byte* ptr38 = OperationAdder.DynamicSingleValueCollection_Add(2, 36, key36, serializedSize36);
				ptr38 += value36.Serialize(ptr38);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 36, key36, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item37 in _areaBlocks36)
		{
			short key37 = item37.Key;
			MapBlockData value37 = item37.Value;
			if (value37 != null)
			{
				int serializedSize37 = value37.GetSerializedSize();
				byte* ptr39 = OperationAdder.DynamicSingleValueCollection_Add(2, 37, key37, serializedSize37);
				ptr39 += value37.Serialize(ptr39);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 37, key37, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item38 in _areaBlocks37)
		{
			short key38 = item38.Key;
			MapBlockData value38 = item38.Value;
			if (value38 != null)
			{
				int serializedSize38 = value38.GetSerializedSize();
				byte* ptr40 = OperationAdder.DynamicSingleValueCollection_Add(2, 38, key38, serializedSize38);
				ptr40 += value38.Serialize(ptr40);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 38, key38, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item39 in _areaBlocks38)
		{
			short key39 = item39.Key;
			MapBlockData value39 = item39.Value;
			if (value39 != null)
			{
				int serializedSize39 = value39.GetSerializedSize();
				byte* ptr41 = OperationAdder.DynamicSingleValueCollection_Add(2, 39, key39, serializedSize39);
				ptr41 += value39.Serialize(ptr41);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 39, key39, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item40 in _areaBlocks39)
		{
			short key40 = item40.Key;
			MapBlockData value40 = item40.Value;
			if (value40 != null)
			{
				int serializedSize40 = value40.GetSerializedSize();
				byte* ptr42 = OperationAdder.DynamicSingleValueCollection_Add(2, 40, key40, serializedSize40);
				ptr42 += value40.Serialize(ptr42);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 40, key40, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item41 in _areaBlocks40)
		{
			short key41 = item41.Key;
			MapBlockData value41 = item41.Value;
			if (value41 != null)
			{
				int serializedSize41 = value41.GetSerializedSize();
				byte* ptr43 = OperationAdder.DynamicSingleValueCollection_Add(2, 41, key41, serializedSize41);
				ptr43 += value41.Serialize(ptr43);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 41, key41, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item42 in _areaBlocks41)
		{
			short key42 = item42.Key;
			MapBlockData value42 = item42.Value;
			if (value42 != null)
			{
				int serializedSize42 = value42.GetSerializedSize();
				byte* ptr44 = OperationAdder.DynamicSingleValueCollection_Add(2, 42, key42, serializedSize42);
				ptr44 += value42.Serialize(ptr44);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 42, key42, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item43 in _areaBlocks42)
		{
			short key43 = item43.Key;
			MapBlockData value43 = item43.Value;
			if (value43 != null)
			{
				int serializedSize43 = value43.GetSerializedSize();
				byte* ptr45 = OperationAdder.DynamicSingleValueCollection_Add(2, 43, key43, serializedSize43);
				ptr45 += value43.Serialize(ptr45);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 43, key43, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item44 in _areaBlocks43)
		{
			short key44 = item44.Key;
			MapBlockData value44 = item44.Value;
			if (value44 != null)
			{
				int serializedSize44 = value44.GetSerializedSize();
				byte* ptr46 = OperationAdder.DynamicSingleValueCollection_Add(2, 44, key44, serializedSize44);
				ptr46 += value44.Serialize(ptr46);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 44, key44, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> item45 in _areaBlocks44)
		{
			short key45 = item45.Key;
			MapBlockData value45 = item45.Value;
			if (value45 != null)
			{
				int serializedSize45 = value45.GetSerializedSize();
				byte* ptr47 = OperationAdder.DynamicSingleValueCollection_Add(2, 45, key45, serializedSize45);
				ptr47 += value45.Serialize(ptr47);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 45, key45, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> brokenAreaBlock in _brokenAreaBlocks)
		{
			short key46 = brokenAreaBlock.Key;
			MapBlockData value46 = brokenAreaBlock.Value;
			if (value46 != null)
			{
				int serializedSize46 = value46.GetSerializedSize();
				byte* ptr48 = OperationAdder.DynamicSingleValueCollection_Add(2, 46, key46, serializedSize46);
				ptr48 += value46.Serialize(ptr48);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 46, key46, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> bornAreaBlock in _bornAreaBlocks)
		{
			short key47 = bornAreaBlock.Key;
			MapBlockData value47 = bornAreaBlock.Value;
			if (value47 != null)
			{
				int serializedSize47 = value47.GetSerializedSize();
				byte* ptr49 = OperationAdder.DynamicSingleValueCollection_Add(2, 47, key47, serializedSize47);
				ptr49 += value47.Serialize(ptr49);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 47, key47, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> guideAreaBlock in _guideAreaBlocks)
		{
			short key48 = guideAreaBlock.Key;
			MapBlockData value48 = guideAreaBlock.Value;
			if (value48 != null)
			{
				int serializedSize48 = value48.GetSerializedSize();
				byte* ptr50 = OperationAdder.DynamicSingleValueCollection_Add(2, 48, key48, serializedSize48);
				ptr50 += value48.Serialize(ptr50);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 48, key48, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> secretVillageAreaBlock in _secretVillageAreaBlocks)
		{
			short key49 = secretVillageAreaBlock.Key;
			MapBlockData value49 = secretVillageAreaBlock.Value;
			if (value49 != null)
			{
				int serializedSize49 = value49.GetSerializedSize();
				byte* ptr51 = OperationAdder.DynamicSingleValueCollection_Add(2, 49, key49, serializedSize49);
				ptr51 += value49.Serialize(ptr51);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 49, key49, 0);
			}
		}
		foreach (KeyValuePair<short, MapBlockData> brokenPerformAreaBlock in _brokenPerformAreaBlocks)
		{
			short key50 = brokenPerformAreaBlock.Key;
			MapBlockData value50 = brokenPerformAreaBlock.Value;
			if (value50 != null)
			{
				int serializedSize50 = value50.GetSerializedSize();
				byte* ptr52 = OperationAdder.DynamicSingleValueCollection_Add(2, 50, key50, serializedSize50);
				ptr52 += value50.Serialize(ptr52);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 50, key50, 0);
			}
		}
		foreach (KeyValuePair<TravelRouteKey, TravelRoute> item46 in _travelRouteDict)
		{
			TravelRouteKey key51 = item46.Key;
			TravelRoute value51 = item46.Value;
			if (value51 != null)
			{
				int serializedSize51 = value51.GetSerializedSize();
				byte* ptr53 = OperationAdder.DynamicSingleValueCollection_Add(2, 51, key51, serializedSize51);
				ptr53 += value51.Serialize(ptr53);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 51, key51, 0);
			}
		}
		foreach (KeyValuePair<TravelRouteKey, TravelRoute> item47 in _bornStateTravelRouteDict)
		{
			TravelRouteKey key52 = item47.Key;
			TravelRoute value52 = item47.Value;
			if (value52 != null)
			{
				int serializedSize52 = value52.GetSerializedSize();
				byte* ptr54 = OperationAdder.DynamicSingleValueCollection_Add(2, 52, key52, serializedSize52);
				ptr54 += value52.Serialize(ptr54);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(2, 52, key52, 0);
			}
		}
		int num3 = 0;
		for (int k = 0; k < 139; k++)
		{
			AnimalPlaceData animalPlaceData = _animalPlaceData[k];
			num3 = ((animalPlaceData == null) ? (num3 + 4) : (num3 + (4 + animalPlaceData.GetSerializedSize())));
		}
		byte* ptr55 = OperationAdder.DynamicElementList_InsertRange(2, 53, 0, 139, num3);
		for (int l = 0; l < 139; l++)
		{
			AnimalPlaceData animalPlaceData2 = _animalPlaceData[l];
			if (animalPlaceData2 != null)
			{
				byte* ptr56 = ptr55;
				ptr55 += 4;
				int num4 = animalPlaceData2.Serialize(ptr55);
				ptr55 += num4;
				*(int*)ptr56 = num4;
			}
			else
			{
				*(int*)ptr55 = 0;
				ptr55 += 4;
			}
		}
		int num5 = 0;
		for (int m = 0; m < 139; m++)
		{
			CricketPlaceData cricketPlaceData = _cricketPlaceData[m];
			num5 = ((cricketPlaceData == null) ? (num5 + 4) : (num5 + (4 + cricketPlaceData.GetSerializedSize())));
		}
		byte* ptr57 = OperationAdder.DynamicElementList_InsertRange(2, 54, 0, 139, num5);
		for (int n = 0; n < 139; n++)
		{
			CricketPlaceData cricketPlaceData2 = _cricketPlaceData[n];
			if (cricketPlaceData2 != null)
			{
				byte* ptr58 = ptr57;
				ptr57 += 4;
				int num6 = cricketPlaceData2.Serialize(ptr57);
				ptr57 += num6;
				*(int*)ptr58 = num6;
			}
			else
			{
				*(int*)ptr57 = 0;
				ptr57 += 4;
			}
		}
		foreach (KeyValuePair<short, GameData.Utilities.ShortList> regularAreaNear in _regularAreaNearList)
		{
			short key53 = regularAreaNear.Key;
			GameData.Utilities.ShortList value53 = regularAreaNear.Value;
			int serializedSize53 = value53.GetSerializedSize();
			byte* ptr59 = OperationAdder.DynamicSingleValueCollection_Add(2, 55, key53, serializedSize53);
			ptr59 += value53.Serialize(ptr59);
		}
		byte* ptr60 = OperationAdder.FixedElementList_InsertRange(2, 56, 0, 8, 32);
		for (int num7 = 0; num7 < 8; num7++)
		{
			ptr60 += _swordTombLocations[num7].Serialize(ptr60);
		}
		int serializedSize54 = _travelInfo.GetSerializedSize();
		byte* ptr61 = OperationAdder.DynamicSingleValue_Set(2, 57, serializedSize54);
		ptr61 += _travelInfo.Serialize(ptr61);
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(2, 0));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 1));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 2));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 3));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 4));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 5));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 6));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 7));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 8));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 9));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 10));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 11));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 12));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 13));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 14));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 15));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 16));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 17));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 18));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 19));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 20));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 21));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 22));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 23));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 24));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 25));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 26));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 27));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 28));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 29));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 30));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 31));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 32));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 33));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 34));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 35));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 36));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 37));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 38));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 39));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 40));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 41));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 42));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 43));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 44));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 45));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 46));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 47));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 48));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 49));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 50));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 51));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 52));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(2, 53));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(2, 54));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(2, 55));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(2, 56));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(2, 57));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesAreas, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_areas[(uint)subId0], dataPool);
		case 1:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
				_modificationsAreaBlocks0.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks0, dataPool);
		case 2:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
				_modificationsAreaBlocks1.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks1, dataPool);
		case 3:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
				_modificationsAreaBlocks2.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks2, dataPool);
		case 4:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
				_modificationsAreaBlocks3.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks3, dataPool);
		case 5:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
				_modificationsAreaBlocks4.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks4, dataPool);
		case 6:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
				_modificationsAreaBlocks5.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks5, dataPool);
		case 7:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
				_modificationsAreaBlocks6.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks6, dataPool);
		case 8:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
				_modificationsAreaBlocks7.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks7, dataPool);
		case 9:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
				_modificationsAreaBlocks8.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks8, dataPool);
		case 10:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 10);
				_modificationsAreaBlocks9.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks9, dataPool);
		case 11:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
				_modificationsAreaBlocks10.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks10, dataPool);
		case 12:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 12);
				_modificationsAreaBlocks11.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks11, dataPool);
		case 13:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 13);
				_modificationsAreaBlocks12.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks12, dataPool);
		case 14:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 14);
				_modificationsAreaBlocks13.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks13, dataPool);
		case 15:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
				_modificationsAreaBlocks14.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks14, dataPool);
		case 16:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 16);
				_modificationsAreaBlocks15.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks15, dataPool);
		case 17:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 17);
				_modificationsAreaBlocks16.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks16, dataPool);
		case 18:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 18);
				_modificationsAreaBlocks17.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks17, dataPool);
		case 19:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 19);
				_modificationsAreaBlocks18.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks18, dataPool);
		case 20:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 20);
				_modificationsAreaBlocks19.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks19, dataPool);
		case 21:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
				_modificationsAreaBlocks20.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks20, dataPool);
		case 22:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
				_modificationsAreaBlocks21.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks21, dataPool);
		case 23:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 23);
				_modificationsAreaBlocks22.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks22, dataPool);
		case 24:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 24);
				_modificationsAreaBlocks23.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks23, dataPool);
		case 25:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 25);
				_modificationsAreaBlocks24.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks24, dataPool);
		case 26:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 26);
				_modificationsAreaBlocks25.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks25, dataPool);
		case 27:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 27);
				_modificationsAreaBlocks26.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks26, dataPool);
		case 28:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 28);
				_modificationsAreaBlocks27.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks27, dataPool);
		case 29:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 29);
				_modificationsAreaBlocks28.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks28, dataPool);
		case 30:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 30);
				_modificationsAreaBlocks29.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks29, dataPool);
		case 31:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 31);
				_modificationsAreaBlocks30.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks30, dataPool);
		case 32:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 32);
				_modificationsAreaBlocks31.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks31, dataPool);
		case 33:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 33);
				_modificationsAreaBlocks32.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks32, dataPool);
		case 34:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 34);
				_modificationsAreaBlocks33.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks33, dataPool);
		case 35:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 35);
				_modificationsAreaBlocks34.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks34, dataPool);
		case 36:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 36);
				_modificationsAreaBlocks35.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks35, dataPool);
		case 37:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 37);
				_modificationsAreaBlocks36.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks36, dataPool);
		case 38:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 38);
				_modificationsAreaBlocks37.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks37, dataPool);
		case 39:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 39);
				_modificationsAreaBlocks38.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks38, dataPool);
		case 40:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 40);
				_modificationsAreaBlocks39.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks39, dataPool);
		case 41:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 41);
				_modificationsAreaBlocks40.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks40, dataPool);
		case 42:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 42);
				_modificationsAreaBlocks41.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks41, dataPool);
		case 43:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 43);
				_modificationsAreaBlocks42.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks42, dataPool);
		case 44:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 44);
				_modificationsAreaBlocks43.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks43, dataPool);
		case 45:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 45);
				_modificationsAreaBlocks44.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_areaBlocks44, dataPool);
		case 46:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 46);
				_modificationsBrokenAreaBlocks.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_brokenAreaBlocks, dataPool);
		case 47:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 47);
				_modificationsBornAreaBlocks.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_bornAreaBlocks, dataPool);
		case 48:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 48);
				_modificationsGuideAreaBlocks.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_guideAreaBlocks, dataPool);
		case 49:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 49);
				_modificationsSecretVillageAreaBlocks.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_secretVillageAreaBlocks, dataPool);
		case 50:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 50);
				_modificationsBrokenPerformAreaBlocks.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<short, MapBlockData>)_brokenPerformAreaBlocks, dataPool);
		case 51:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 51);
				_modificationsTravelRouteDict.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<TravelRouteKey, TravelRoute>)_travelRouteDict, dataPool);
		case 52:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 52);
				_modificationsBornStateTravelRouteDict.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<TravelRouteKey, TravelRoute>)_bornStateTravelRouteDict, dataPool);
		case 53:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesAnimalPlaceData, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_animalPlaceData[(uint)subId0], dataPool);
		case 54:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesCricketPlaceData, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_cricketPlaceData[(uint)subId0], dataPool);
		case 55:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 56:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesSwordTombLocations, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_swordTombLocations[(uint)subId0], dataPool);
		case 57:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 57);
			}
			return GameData.Serializer.Serializer.Serialize(_travelInfo, dataPool);
		case 58:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 58);
			}
			return GameData.Serializer.Serializer.Serialize(_onHandlingTravelingEventBlock, dataPool);
		case 59:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 59);
			}
			return GameData.Serializer.Serializer.Serialize(_hunterAnimalsCache, dataPool);
		case 60:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 60);
			}
			return GameData.Serializer.Serializer.Serialize(_moveBanned, dataPool);
		case 61:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 61);
			}
			return GameData.Serializer.Serializer.Serialize(_crossArchiveLockMoveTime, dataPool);
		case 62:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 62);
			}
			return GameData.Serializer.Serializer.Serialize(GetFleeBeasts(), dataPool);
		case 63:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 63);
			}
			return GameData.Serializer.Serializer.Serialize(GetFleeLoongs(), dataPool);
		case 64:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 64);
			}
			return GameData.Serializer.Serializer.Serialize(GetLoongLocations(), dataPool);
		case 65:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 65);
			}
			return GameData.Serializer.Serializer.Serialize(GetAlterSettlementLocations(), dataPool);
		case 66:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 66);
			}
			return GameData.Serializer.Serializer.Serialize(_isTaiwuInFulongFlameArea, dataPool);
		case 67:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 67);
			}
			return GameData.Serializer.Serializer.Serialize(GetVisibleMapPickups(), dataPool);
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
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 2:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 3:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 4:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 5:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 6:
			throw new Exception($"Not allow to set value of dataId {dataId}");
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
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 13:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 14:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 15:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 16:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 17:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 18:
			throw new Exception($"Not allow to set value of dataId {dataId}");
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
			throw new Exception($"Not allow to set value of dataId {dataId}");
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
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 36:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 37:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 38:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 39:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 40:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 41:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 42:
			throw new Exception($"Not allow to set value of dataId {dataId}");
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
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 50:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 51:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 52:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 53:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 54:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 55:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 56:
		{
			Location item = default(Location);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			_swordTombLocations[(uint)subId0] = item;
			SetElement_SwordTombLocations((int)subId0, item, context);
			break;
		}
		case 57:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 58:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _onHandlingTravelingEventBlock);
			SetOnHandlingTravelingEventBlock(_onHandlingTravelingEventBlock, context);
			break;
		case 59:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 60:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 61:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _crossArchiveLockMoveTime);
			SetCrossArchiveLockMoveTime(_crossArchiveLockMoveTime, context);
			break;
		case 62:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 63:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 64:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 65:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 66:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _isTaiwuInFulongFlameArea);
			SetIsTaiwuInFulongFlameArea(_isTaiwuInFulongFlameArea, context);
			break;
		case 67:
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
		{
			int argsCount17 = operation.ArgsCount;
			int num17 = argsCount17;
			if (num17 == 1)
			{
				bool item35 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item35);
				GmCmd_SetLockTime(item35);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount39 = operation.ArgsCount;
			int num39 = argsCount39;
			if (num39 == 1)
			{
				bool item126 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item126);
				GmCmd_SetTeleportMove(item126);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
			if (operation.ArgsCount == 0)
			{
				GmCmd_ShowAllMapBlock(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 3:
			if (operation.ArgsCount == 0)
			{
				GmCmd_UnlockAllStation(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 4:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 2)
			{
				short item8 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				int item9 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				GmCmd_ChangeSpiritualDebt(context, item8, item9);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
		{
			int argsCount29 = operation.ArgsCount;
			int num29 = argsCount29;
			if (num29 == 1)
			{
				MapBlockData item56 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item56);
				GmCmd_SetMapBlockData(context, item56);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 6:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 1)
			{
				short item14 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				GmCmd_CreateFixedCharacterAtCurrentBlock(context, item14);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 7:
		{
			int argsCount38 = operation.ArgsCount;
			int num38 = argsCount38;
			if (num38 == 1)
			{
				short item125 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item125);
				Move(context, item125);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 8:
		{
			int argsCount23 = operation.ArgsCount;
			int num23 = argsCount23;
			if (num23 == 2)
			{
				Location item45 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item45);
				Location item46 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item46);
				MoveFinish(context, item45, item46);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 9:
			if (operation.ArgsCount == 0)
			{
				bool item29 = IsContinuousMovingBreak();
				return GameData.Serializer.Serializer.Serialize(item29, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 10:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				short item22 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				UnlockStation(context, item22);
				return -1;
			}
			case 2:
			{
				short item20 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				bool item21 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item21);
				UnlockStation(context, item20, item21);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 11:
		{
			int argsCount43 = operation.ArgsCount;
			int num43 = argsCount43;
			if (num43 == 3)
			{
				short item130 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item130);
				short item131 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item131);
				short item132 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item132);
				CrossAreaMoveInfo travelCost = GetTravelCost(item130, item131, item132);
				return GameData.Serializer.Serializer.Serialize(travelCost, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 12:
		{
			int argsCount34 = operation.ArgsCount;
			int num34 = argsCount34;
			if (num34 == 1)
			{
				short item114 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item114);
				StartTravel(context, item114);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 13:
			if (operation.ArgsCount == 0)
			{
				bool item53 = ContinueTravel(context);
				return GameData.Serializer.Serializer.Serialize(item53, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 14:
			if (operation.ArgsCount == 0)
			{
				StopTravel(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 15:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 1)
			{
				int item28 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item28);
				RecordTravelCostedDays(context, item28);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 16:
			if (operation.ArgsCount == 0)
			{
				int[] allAreaCompletelyInfectedCharCount = GetAllAreaCompletelyInfectedCharCount();
				return GameData.Serializer.Serializer.Serialize(allAreaCompletelyInfectedCharCount, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 17:
		{
			int argsCount41 = operation.ArgsCount;
			int num41 = argsCount41;
			if (num41 == 1)
			{
				sbyte item128 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item128);
				Dictionary<TravelRouteKey, TravelRoute> travelRoutesInState = GetTravelRoutesInState(item128);
				return GameData.Serializer.Serializer.Serialize(travelRoutesInState, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 18:
			if (operation.ArgsCount == 0)
			{
				bool item115 = TryTriggerCricketCatch(context);
				return GameData.Serializer.Serializer.Serialize(item115, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 19:
		{
			int argsCount30 = operation.ArgsCount;
			int num30 = argsCount30;
			if (num30 == 2)
			{
				short item107 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item107);
				short item108 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item108);
				MapBlockData blockData = GetBlockData(item107, item108);
				return GameData.Serializer.Serializer.Serialize(blockData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 20:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				int item66 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item66);
				sbyte item67 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item67);
				CollectResourceResult item68 = CollectResource(context, item66, item67);
				return GameData.Serializer.Serializer.Serialize(item68, returnDataPool);
			}
			case 3:
			{
				int item62 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item62);
				sbyte item63 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item63);
				bool item64 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item64);
				CollectResourceResult item65 = CollectResource(context, item62, item63, item64);
				return GameData.Serializer.Serializer.Serialize(item65, returnDataPool);
			}
			case 4:
			{
				int item57 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item57);
				sbyte item58 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item58);
				bool item59 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item59);
				bool item60 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item60);
				CollectResourceResult item61 = CollectResource(context, item57, item58, item59, item60);
				return GameData.Serializer.Serializer.Serialize(item61, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 21:
		{
			int argsCount26 = operation.ArgsCount;
			int num26 = argsCount26;
			if (num26 == 1)
			{
				List<Location> item50 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item50);
				List<MapBlockData> mapBlockDataList = GetMapBlockDataList(item50);
				return GameData.Serializer.Serializer.Serialize(mapBlockDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 22:
		{
			int argsCount20 = operation.ArgsCount;
			int num20 = argsCount20;
			if (num20 == 1)
			{
				List<Location> item39 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item39);
				List<short> belongBlockTemplateIdList = GetBelongBlockTemplateIdList(item39);
				return GameData.Serializer.Serializer.Serialize(belongBlockTemplateIdList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 23:
		{
			int argsCount16 = operation.ArgsCount;
			int num16 = argsCount16;
			if (num16 == 1)
			{
				Location item34 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item34);
				LocationNameRelatedData locationNameRelatedData = GetLocationNameRelatedData(item34);
				return GameData.Serializer.Serializer.Serialize(locationNameRelatedData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 24:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 1)
			{
				List<Location> item23 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				List<LocationNameRelatedData> locationNameRelatedDataList = GetLocationNameRelatedDataList(item23);
				return GameData.Serializer.Serializer.Serialize(locationNameRelatedDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 25:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 3)
			{
				Location item10 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				short item11 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				bool item12 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				ChangeBlockTemplate(context, item10, item11, item12);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 26:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 1)
			{
				short item2 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				bool item3 = IsContainsPurpleBamboo(item2);
				return GameData.Serializer.Serializer.Serialize(item3, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 27:
			if (operation.ArgsCount == 0)
			{
				int[] allStateCompletelyInfectedCharCount = GetAllStateCompletelyInfectedCharCount();
				return GameData.Serializer.Serializer.Serialize(allStateCompletelyInfectedCharCount, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 28:
		{
			int argsCount36 = operation.ArgsCount;
			int num36 = argsCount36;
			if (num36 == 1)
			{
				Location item123 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item123);
				FullBlockName blockFullName = GetBlockFullName(item123);
				return GameData.Serializer.Serializer.Serialize(blockFullName, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 29:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				List<Location> item122 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item122);
				List<MapBlockData> mapBlockDataListOptional3 = GetMapBlockDataListOptional(item122);
				return GameData.Serializer.Serializer.Serialize(mapBlockDataListOptional3, returnDataPool);
			}
			case 2:
			{
				List<Location> item120 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item120);
				bool item121 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item121);
				List<MapBlockData> mapBlockDataListOptional2 = GetMapBlockDataListOptional(item120, item121);
				return GameData.Serializer.Serializer.Serialize(mapBlockDataListOptional2, returnDataPool);
			}
			case 3:
			{
				List<Location> item117 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item117);
				bool item118 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item118);
				bool item119 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item119);
				List<MapBlockData> mapBlockDataListOptional = GetMapBlockDataListOptional(item117, item118, item119);
				return GameData.Serializer.Serializer.Serialize(mapBlockDataListOptional, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 30:
		{
			int argsCount32 = operation.ArgsCount;
			int num32 = argsCount32;
			if (num32 == 2)
			{
				Location item110 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item110);
				Location item111 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item111);
				bool item112 = IsLocationInBuildingEffectRange(item110, item111);
				return GameData.Serializer.Serializer.Serialize(item112, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 31:
			if (operation.ArgsCount == 0)
			{
				short item106 = ContinueTravelWithDetectTravelingEvent(context);
				return GameData.Serializer.Serializer.Serialize(item106, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 32:
			if (operation.ArgsCount == 0)
			{
				List<CollectResourceResult> item54 = CollectAllResourcesFree(context);
				return GameData.Serializer.Serializer.Serialize(item54, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 33:
		{
			int argsCount25 = operation.ArgsCount;
			int num25 = argsCount25;
			if (num25 == 1)
			{
				short item49 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item49);
				QuickTravel(context, item49);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 34:
		{
			int argsCount21 = operation.ArgsCount;
			int num21 = argsCount21;
			if (num21 == 1)
			{
				short item40 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item40);
				Location item41 = QueryFixedCharacterLocation(item40);
				return GameData.Serializer.Serializer.Serialize(item41, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 35:
			if (operation.ArgsCount == 0)
			{
				AreaDisplayData[] allAreaDisplayData = GetAllAreaDisplayData();
				return GameData.Serializer.Serializer.Serialize(allAreaDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 36:
		{
			int argsCount14 = operation.ArgsCount;
			int num14 = argsCount14;
			if (num14 == 1)
			{
				int item30 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item30);
				Location item31 = QueryTemplateBlockLocation(item30);
				return GameData.Serializer.Serializer.Serialize(item31, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 37:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 1)
			{
				short item27 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item27);
				List<MapBlockDisplayData> blockDisplayDataInArea = GetBlockDisplayDataInArea(item27);
				return GameData.Serializer.Serializer.Serialize(blockDisplayDataInArea, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 38:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 1)
			{
				short item15 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				bool item16 = UnlockTravelPath(context, item15);
				return GameData.Serializer.Serializer.Serialize(item16, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 39:
			if (operation.ArgsCount == 0)
			{
				GmCmd_HideAllMapBlock(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 40:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 2)
			{
				Location item4 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				Location item5 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				List<Location> pathInAreaWithoutCost = GetPathInAreaWithoutCost(item4, item5);
				return GameData.Serializer.Serializer.Serialize(pathInAreaWithoutCost, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 41:
		{
			int argsCount42 = operation.ArgsCount;
			int num42 = argsCount42;
			if (num42 == 1)
			{
				short item129 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item129);
				TravelPreviewDisplayData travelPreview = GetTravelPreview(item129);
				return GameData.Serializer.Serializer.Serialize(travelPreview, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 42:
		{
			int argsCount40 = operation.ArgsCount;
			int num40 = argsCount40;
			if (num40 == 1)
			{
				Location item127 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item127);
				RetrieveDreamBackLocation(context, item127);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 43:
		{
			int argsCount37 = operation.ArgsCount;
			int num37 = argsCount37;
			if (num37 == 1)
			{
				short item124 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item124);
				MapAreaData areaByAreaId = GetAreaByAreaId(item124);
				return GameData.Serializer.Serializer.Serialize(areaByAreaId, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 44:
		{
			int argsCount35 = operation.ArgsCount;
			int num35 = argsCount35;
			if (num35 == 1)
			{
				short item116 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item116);
				GmCmd_AddAnimal(context, item116);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 45:
		{
			int argsCount33 = operation.ArgsCount;
			int num33 = argsCount33;
			if (num33 == 1)
			{
				bool item113 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item113);
				TeammateBubbleCollection teammateBubbleCollection = GetTeammateBubbleCollection(context, item113);
				return GameData.Serializer.Serializer.Serialize(teammateBubbleCollection, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 46:
		{
			int argsCount31 = operation.ArgsCount;
			int num31 = argsCount31;
			if (num31 == 1)
			{
				short item109 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item109);
				GmCmd_AddRandomEnemyOnMap(context, item109);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 47:
			if (operation.ArgsCount == 0)
			{
				GMCmd_ThrowBackend();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 48:
			switch (operation.ArgsCount)
			{
			case 3:
			{
				int item102 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item102);
				int item103 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item103);
				int item104 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item104);
				MapHealSimulateResult item105 = SimulateHealCost(item102, item103, item104);
				return GameData.Serializer.Serializer.Serialize(item105, returnDataPool);
			}
			case 4:
			{
				int item97 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item97);
				int item98 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item98);
				int item99 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item99);
				bool item100 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item100);
				MapHealSimulateResult item101 = SimulateHealCost(item97, item98, item99, item100);
				return GameData.Serializer.Serializer.Serialize(item101, returnDataPool);
			}
			case 5:
			{
				int item91 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item91);
				int item92 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item92);
				int item93 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item93);
				bool item94 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item94);
				bool item95 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item95);
				MapHealSimulateResult item96 = SimulateHealCost(item91, item92, item93, item94, item95);
				return GameData.Serializer.Serializer.Serialize(item96, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 49:
			switch (operation.ArgsCount)
			{
			case 3:
			{
				int item87 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item87);
				int item88 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item88);
				int item89 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item89);
				bool item90 = HealOnMap(context, item87, item88, item89);
				return GameData.Serializer.Serializer.Serialize(item90, returnDataPool);
			}
			case 4:
			{
				int item82 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item82);
				int item83 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item83);
				int item84 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item84);
				bool item85 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item85);
				bool item86 = HealOnMap(context, item82, item83, item84, item85);
				return GameData.Serializer.Serializer.Serialize(item86, returnDataPool);
			}
			case 5:
			{
				int item76 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item76);
				int item77 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item77);
				int item78 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item78);
				bool item79 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item79);
				int item80 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item80);
				bool item81 = HealOnMap(context, item76, item77, item78, item79, item80);
				return GameData.Serializer.Serializer.Serialize(item81, returnDataPool);
			}
			case 6:
			{
				int item69 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item69);
				int item70 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item70);
				int item71 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item71);
				bool item72 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item72);
				int item73 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item73);
				bool item74 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item74);
				bool item75 = HealOnMap(context, item69, item70, item71, item72, item73, item74);
				return GameData.Serializer.Serializer.Serialize(item75, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 50:
		{
			int argsCount28 = operation.ArgsCount;
			int num28 = argsCount28;
			if (num28 == 1)
			{
				short item55 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item55);
				TeleportByTraveler(context, item55);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 51:
		{
			int argsCount27 = operation.ArgsCount;
			int num27 = argsCount27;
			if (num27 == 1)
			{
				Location item51 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item51);
				bool item52 = BuildTravelerPalace(context, item51);
				return GameData.Serializer.Serializer.Serialize(item52, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 52:
		{
			int argsCount24 = operation.ArgsCount;
			int num24 = argsCount24;
			if (num24 == 1)
			{
				int item47 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item47);
				bool item48 = TeleportOnTravelerPalace(context, item47);
				return GameData.Serializer.Serializer.Serialize(item48, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 53:
		{
			int argsCount22 = operation.ArgsCount;
			int num22 = argsCount22;
			if (num22 == 2)
			{
				int item42 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item42);
				string item43 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item43);
				bool item44 = ChangeTravelerPalaceName(context, item42, item43);
				return GameData.Serializer.Serializer.Serialize(item44, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 54:
		{
			int argsCount19 = operation.ArgsCount;
			int num19 = argsCount19;
			if (num19 == 1)
			{
				int item37 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item37);
				bool item38 = DestroyTravelerPalace(context, item37);
				return GameData.Serializer.Serializer.Serialize(item38, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 55:
		{
			int argsCount18 = operation.ArgsCount;
			int num18 = argsCount18;
			if (num18 == 1)
			{
				int item36 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item36);
				GmCmd_ChangeAllSpiritualDebt(context, item36);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 56:
		{
			int argsCount15 = operation.ArgsCount;
			int num15 = argsCount15;
			if (num15 == 2)
			{
				Location item32 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item32);
				int item33 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item33);
				TaiwuBeKidnapped(context, item32, item33);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 57:
			if (operation.ArgsCount == 0)
			{
				DirectTravelToTaiwuVillage(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 58:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 2)
			{
				int item24 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item24);
				short item25 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item25);
				Location item26 = QueryTemplateBlockLocationInArea(item24, item25);
				return GameData.Serializer.Serializer.Serialize(item26, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 59:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 2)
			{
				short item17 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				short item18 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item18);
				Location item19 = QueryFixedCharacterLocationInArea(item17, item18);
				return GameData.Serializer.Serializer.Serialize(item19, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 60:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 1)
			{
				short item13 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item13);
				Dictionary<short, short> allSettlementInfluenceRangeBlocks = GetAllSettlementInfluenceRangeBlocks(item13);
				return GameData.Serializer.Serializer.SerializeDefault(allSettlementInfluenceRangeBlocks, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 61:
			if (operation.ArgsCount == 0)
			{
				GmCmd_TurnMapBlockIntoAshes(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 62:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 1)
			{
				short item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				bool item7 = GmCmd_TriggerTravelingEvent(context, item6);
				return GameData.Serializer.Serializer.Serialize(item7, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 63:
			if (operation.ArgsCount == 0)
			{
				int item = GmCmd_GetTreasuryValueByTaiwuLocation();
				return GameData.Serializer.Serializer.Serialize(item, returnDataPool);
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
			_modificationsAreaBlocks0.ChangeRecording(monitoring);
			break;
		case 2:
			_modificationsAreaBlocks1.ChangeRecording(monitoring);
			break;
		case 3:
			_modificationsAreaBlocks2.ChangeRecording(monitoring);
			break;
		case 4:
			_modificationsAreaBlocks3.ChangeRecording(monitoring);
			break;
		case 5:
			_modificationsAreaBlocks4.ChangeRecording(monitoring);
			break;
		case 6:
			_modificationsAreaBlocks5.ChangeRecording(monitoring);
			break;
		case 7:
			_modificationsAreaBlocks6.ChangeRecording(monitoring);
			break;
		case 8:
			_modificationsAreaBlocks7.ChangeRecording(monitoring);
			break;
		case 9:
			_modificationsAreaBlocks8.ChangeRecording(monitoring);
			break;
		case 10:
			_modificationsAreaBlocks9.ChangeRecording(monitoring);
			break;
		case 11:
			_modificationsAreaBlocks10.ChangeRecording(monitoring);
			break;
		case 12:
			_modificationsAreaBlocks11.ChangeRecording(monitoring);
			break;
		case 13:
			_modificationsAreaBlocks12.ChangeRecording(monitoring);
			break;
		case 14:
			_modificationsAreaBlocks13.ChangeRecording(monitoring);
			break;
		case 15:
			_modificationsAreaBlocks14.ChangeRecording(monitoring);
			break;
		case 16:
			_modificationsAreaBlocks15.ChangeRecording(monitoring);
			break;
		case 17:
			_modificationsAreaBlocks16.ChangeRecording(monitoring);
			break;
		case 18:
			_modificationsAreaBlocks17.ChangeRecording(monitoring);
			break;
		case 19:
			_modificationsAreaBlocks18.ChangeRecording(monitoring);
			break;
		case 20:
			_modificationsAreaBlocks19.ChangeRecording(monitoring);
			break;
		case 21:
			_modificationsAreaBlocks20.ChangeRecording(monitoring);
			break;
		case 22:
			_modificationsAreaBlocks21.ChangeRecording(monitoring);
			break;
		case 23:
			_modificationsAreaBlocks22.ChangeRecording(monitoring);
			break;
		case 24:
			_modificationsAreaBlocks23.ChangeRecording(monitoring);
			break;
		case 25:
			_modificationsAreaBlocks24.ChangeRecording(monitoring);
			break;
		case 26:
			_modificationsAreaBlocks25.ChangeRecording(monitoring);
			break;
		case 27:
			_modificationsAreaBlocks26.ChangeRecording(monitoring);
			break;
		case 28:
			_modificationsAreaBlocks27.ChangeRecording(monitoring);
			break;
		case 29:
			_modificationsAreaBlocks28.ChangeRecording(monitoring);
			break;
		case 30:
			_modificationsAreaBlocks29.ChangeRecording(monitoring);
			break;
		case 31:
			_modificationsAreaBlocks30.ChangeRecording(monitoring);
			break;
		case 32:
			_modificationsAreaBlocks31.ChangeRecording(monitoring);
			break;
		case 33:
			_modificationsAreaBlocks32.ChangeRecording(monitoring);
			break;
		case 34:
			_modificationsAreaBlocks33.ChangeRecording(monitoring);
			break;
		case 35:
			_modificationsAreaBlocks34.ChangeRecording(monitoring);
			break;
		case 36:
			_modificationsAreaBlocks35.ChangeRecording(monitoring);
			break;
		case 37:
			_modificationsAreaBlocks36.ChangeRecording(monitoring);
			break;
		case 38:
			_modificationsAreaBlocks37.ChangeRecording(monitoring);
			break;
		case 39:
			_modificationsAreaBlocks38.ChangeRecording(monitoring);
			break;
		case 40:
			_modificationsAreaBlocks39.ChangeRecording(monitoring);
			break;
		case 41:
			_modificationsAreaBlocks40.ChangeRecording(monitoring);
			break;
		case 42:
			_modificationsAreaBlocks41.ChangeRecording(monitoring);
			break;
		case 43:
			_modificationsAreaBlocks42.ChangeRecording(monitoring);
			break;
		case 44:
			_modificationsAreaBlocks43.ChangeRecording(monitoring);
			break;
		case 45:
			_modificationsAreaBlocks44.ChangeRecording(monitoring);
			break;
		case 46:
			_modificationsBrokenAreaBlocks.ChangeRecording(monitoring);
			break;
		case 47:
			_modificationsBornAreaBlocks.ChangeRecording(monitoring);
			break;
		case 48:
			_modificationsGuideAreaBlocks.ChangeRecording(monitoring);
			break;
		case 49:
			_modificationsSecretVillageAreaBlocks.ChangeRecording(monitoring);
			break;
		case 50:
			_modificationsBrokenPerformAreaBlocks.ChangeRecording(monitoring);
			break;
		case 51:
			_modificationsTravelRouteDict.ChangeRecording(monitoring);
			break;
		case 52:
			_modificationsBornStateTravelRouteDict.ChangeRecording(monitoring);
			break;
		case 53:
			break;
		case 54:
			break;
		case 55:
			break;
		case 56:
			break;
		case 57:
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
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		switch (dataId)
		{
		case 0:
			if (!BaseGameDataDomain.IsModified(_dataStatesAreas, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesAreas, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_areas[(uint)subId0], dataPool);
		case 1:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 1))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 1);
			int result25 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks0, dataPool, _modificationsAreaBlocks0);
			_modificationsAreaBlocks0.Reset();
			return result25;
		}
		case 2:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 2))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 2);
			int result51 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks1, dataPool, _modificationsAreaBlocks1);
			_modificationsAreaBlocks1.Reset();
			return result51;
		}
		case 3:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 3))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 3);
			int result13 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks2, dataPool, _modificationsAreaBlocks2);
			_modificationsAreaBlocks2.Reset();
			return result13;
		}
		case 4:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 4))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 4);
			int result33 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks3, dataPool, _modificationsAreaBlocks3);
			_modificationsAreaBlocks3.Reset();
			return result33;
		}
		case 5:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 5))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 5);
			int result11 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks4, dataPool, _modificationsAreaBlocks4);
			_modificationsAreaBlocks4.Reset();
			return result11;
		}
		case 6:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 6))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 6);
			int result37 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks5, dataPool, _modificationsAreaBlocks5);
			_modificationsAreaBlocks5.Reset();
			return result37;
		}
		case 7:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 7))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 7);
			int result20 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks6, dataPool, _modificationsAreaBlocks6);
			_modificationsAreaBlocks6.Reset();
			return result20;
		}
		case 8:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 8))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 8);
			int result4 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks7, dataPool, _modificationsAreaBlocks7);
			_modificationsAreaBlocks7.Reset();
			return result4;
		}
		case 9:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 9))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 9);
			int result43 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks8, dataPool, _modificationsAreaBlocks8);
			_modificationsAreaBlocks8.Reset();
			return result43;
		}
		case 10:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 10))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 10);
			int result26 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks9, dataPool, _modificationsAreaBlocks9);
			_modificationsAreaBlocks9.Reset();
			return result26;
		}
		case 11:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 11))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 11);
			int result17 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks10, dataPool, _modificationsAreaBlocks10);
			_modificationsAreaBlocks10.Reset();
			return result17;
		}
		case 12:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 12))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 12);
			int result7 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks11, dataPool, _modificationsAreaBlocks11);
			_modificationsAreaBlocks11.Reset();
			return result7;
		}
		case 13:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 13))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 13);
			int result50 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks12, dataPool, _modificationsAreaBlocks12);
			_modificationsAreaBlocks12.Reset();
			return result50;
		}
		case 14:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 14))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 14);
			int result39 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks13, dataPool, _modificationsAreaBlocks13);
			_modificationsAreaBlocks13.Reset();
			return result39;
		}
		case 15:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 15))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 15);
			int result30 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks14, dataPool, _modificationsAreaBlocks14);
			_modificationsAreaBlocks14.Reset();
			return result30;
		}
		case 16:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 16))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 16);
			int result22 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks15, dataPool, _modificationsAreaBlocks15);
			_modificationsAreaBlocks15.Reset();
			return result22;
		}
		case 17:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 17))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 17);
			int result16 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks16, dataPool, _modificationsAreaBlocks16);
			_modificationsAreaBlocks16.Reset();
			return result16;
		}
		case 18:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 18))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 18);
			int result8 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks17, dataPool, _modificationsAreaBlocks17);
			_modificationsAreaBlocks17.Reset();
			return result8;
		}
		case 19:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 19))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 19);
			int result2 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks18, dataPool, _modificationsAreaBlocks18);
			_modificationsAreaBlocks18.Reset();
			return result2;
		}
		case 20:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 20))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 20);
			int result47 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks19, dataPool, _modificationsAreaBlocks19);
			_modificationsAreaBlocks19.Reset();
			return result47;
		}
		case 21:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 21))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 21);
			int result42 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks20, dataPool, _modificationsAreaBlocks20);
			_modificationsAreaBlocks20.Reset();
			return result42;
		}
		case 22:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 22))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 22);
			int result34 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks21, dataPool, _modificationsAreaBlocks21);
			_modificationsAreaBlocks21.Reset();
			return result34;
		}
		case 23:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 23))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 23);
			int result28 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks22, dataPool, _modificationsAreaBlocks22);
			_modificationsAreaBlocks22.Reset();
			return result28;
		}
		case 24:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 24))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 24);
			int result23 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks23, dataPool, _modificationsAreaBlocks23);
			_modificationsAreaBlocks23.Reset();
			return result23;
		}
		case 25:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 25))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 25);
			int result19 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks24, dataPool, _modificationsAreaBlocks24);
			_modificationsAreaBlocks24.Reset();
			return result19;
		}
		case 26:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 26))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 26);
			int result14 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks25, dataPool, _modificationsAreaBlocks25);
			_modificationsAreaBlocks25.Reset();
			return result14;
		}
		case 27:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 27))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 27);
			int result10 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks26, dataPool, _modificationsAreaBlocks26);
			_modificationsAreaBlocks26.Reset();
			return result10;
		}
		case 28:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 28))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 28);
			int result5 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks27, dataPool, _modificationsAreaBlocks27);
			_modificationsAreaBlocks27.Reset();
			return result5;
		}
		case 29:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 29))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 29);
			int result = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks28, dataPool, _modificationsAreaBlocks28);
			_modificationsAreaBlocks28.Reset();
			return result;
		}
		case 30:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 30))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 30);
			int result48 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks29, dataPool, _modificationsAreaBlocks29);
			_modificationsAreaBlocks29.Reset();
			return result48;
		}
		case 31:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 31))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 31);
			int result45 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks30, dataPool, _modificationsAreaBlocks30);
			_modificationsAreaBlocks30.Reset();
			return result45;
		}
		case 32:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 32))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 32);
			int result40 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks31, dataPool, _modificationsAreaBlocks31);
			_modificationsAreaBlocks31.Reset();
			return result40;
		}
		case 33:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 33))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 33);
			int result36 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks32, dataPool, _modificationsAreaBlocks32);
			_modificationsAreaBlocks32.Reset();
			return result36;
		}
		case 34:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 34))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 34);
			int result31 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks33, dataPool, _modificationsAreaBlocks33);
			_modificationsAreaBlocks33.Reset();
			return result31;
		}
		case 35:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 35))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 35);
			int result27 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks34, dataPool, _modificationsAreaBlocks34);
			_modificationsAreaBlocks34.Reset();
			return result27;
		}
		case 36:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 36))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 36);
			int result24 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks35, dataPool, _modificationsAreaBlocks35);
			_modificationsAreaBlocks35.Reset();
			return result24;
		}
		case 37:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 37))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 37);
			int result21 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks36, dataPool, _modificationsAreaBlocks36);
			_modificationsAreaBlocks36.Reset();
			return result21;
		}
		case 38:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 38))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 38);
			int result18 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks37, dataPool, _modificationsAreaBlocks37);
			_modificationsAreaBlocks37.Reset();
			return result18;
		}
		case 39:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 39))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 39);
			int result15 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks38, dataPool, _modificationsAreaBlocks38);
			_modificationsAreaBlocks38.Reset();
			return result15;
		}
		case 40:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 40))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 40);
			int result12 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks39, dataPool, _modificationsAreaBlocks39);
			_modificationsAreaBlocks39.Reset();
			return result12;
		}
		case 41:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 41))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 41);
			int result9 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks40, dataPool, _modificationsAreaBlocks40);
			_modificationsAreaBlocks40.Reset();
			return result9;
		}
		case 42:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 42))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 42);
			int result6 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks41, dataPool, _modificationsAreaBlocks41);
			_modificationsAreaBlocks41.Reset();
			return result6;
		}
		case 43:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 43))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 43);
			int result3 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks42, dataPool, _modificationsAreaBlocks42);
			_modificationsAreaBlocks42.Reset();
			return result3;
		}
		case 44:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 44))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 44);
			int result52 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks43, dataPool, _modificationsAreaBlocks43);
			_modificationsAreaBlocks43.Reset();
			return result52;
		}
		case 45:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 45))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 45);
			int result49 = GameData.Serializer.Serializer.SerializeModifications(_areaBlocks44, dataPool, _modificationsAreaBlocks44);
			_modificationsAreaBlocks44.Reset();
			return result49;
		}
		case 46:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 46))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 46);
			int result46 = GameData.Serializer.Serializer.SerializeModifications(_brokenAreaBlocks, dataPool, _modificationsBrokenAreaBlocks);
			_modificationsBrokenAreaBlocks.Reset();
			return result46;
		}
		case 47:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 47))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 47);
			int result44 = GameData.Serializer.Serializer.SerializeModifications(_bornAreaBlocks, dataPool, _modificationsBornAreaBlocks);
			_modificationsBornAreaBlocks.Reset();
			return result44;
		}
		case 48:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 48))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 48);
			int result41 = GameData.Serializer.Serializer.SerializeModifications(_guideAreaBlocks, dataPool, _modificationsGuideAreaBlocks);
			_modificationsGuideAreaBlocks.Reset();
			return result41;
		}
		case 49:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 49))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 49);
			int result38 = GameData.Serializer.Serializer.SerializeModifications(_secretVillageAreaBlocks, dataPool, _modificationsSecretVillageAreaBlocks);
			_modificationsSecretVillageAreaBlocks.Reset();
			return result38;
		}
		case 50:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 50))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 50);
			int result35 = GameData.Serializer.Serializer.SerializeModifications(_brokenPerformAreaBlocks, dataPool, _modificationsBrokenPerformAreaBlocks);
			_modificationsBrokenPerformAreaBlocks.Reset();
			return result35;
		}
		case 51:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 51))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 51);
			int result32 = GameData.Serializer.Serializer.SerializeModifications(_travelRouteDict, dataPool, _modificationsTravelRouteDict);
			_modificationsTravelRouteDict.Reset();
			return result32;
		}
		case 52:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 52))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 52);
			int result29 = GameData.Serializer.Serializer.SerializeModifications(_bornStateTravelRouteDict, dataPool, _modificationsBornStateTravelRouteDict);
			_modificationsBornStateTravelRouteDict.Reset();
			return result29;
		}
		case 53:
			if (!BaseGameDataDomain.IsModified(_dataStatesAnimalPlaceData, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesAnimalPlaceData, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_animalPlaceData[(uint)subId0], dataPool);
		case 54:
			if (!BaseGameDataDomain.IsModified(_dataStatesCricketPlaceData, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesCricketPlaceData, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_cricketPlaceData[(uint)subId0], dataPool);
		case 55:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 56:
			if (!BaseGameDataDomain.IsModified(_dataStatesSwordTombLocations, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesSwordTombLocations, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_swordTombLocations[(uint)subId0], dataPool);
		case 57:
			if (!BaseGameDataDomain.IsModified(DataStates, 57))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 57);
			return GameData.Serializer.Serializer.Serialize(_travelInfo, dataPool);
		case 58:
			if (!BaseGameDataDomain.IsModified(DataStates, 58))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 58);
			return GameData.Serializer.Serializer.Serialize(_onHandlingTravelingEventBlock, dataPool);
		case 59:
			if (!BaseGameDataDomain.IsModified(DataStates, 59))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 59);
			return GameData.Serializer.Serializer.Serialize(_hunterAnimalsCache, dataPool);
		case 60:
			if (!BaseGameDataDomain.IsModified(DataStates, 60))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 60);
			return GameData.Serializer.Serializer.Serialize(_moveBanned, dataPool);
		case 61:
			if (!BaseGameDataDomain.IsModified(DataStates, 61))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 61);
			return GameData.Serializer.Serializer.Serialize(_crossArchiveLockMoveTime, dataPool);
		case 62:
			if (!BaseGameDataDomain.IsModified(DataStates, 62))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 62);
			return GameData.Serializer.Serializer.Serialize(GetFleeBeasts(), dataPool);
		case 63:
			if (!BaseGameDataDomain.IsModified(DataStates, 63))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 63);
			return GameData.Serializer.Serializer.Serialize(GetFleeLoongs(), dataPool);
		case 64:
			if (!BaseGameDataDomain.IsModified(DataStates, 64))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 64);
			return GameData.Serializer.Serializer.Serialize(GetLoongLocations(), dataPool);
		case 65:
			if (!BaseGameDataDomain.IsModified(DataStates, 65))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 65);
			return GameData.Serializer.Serializer.Serialize(GetAlterSettlementLocations(), dataPool);
		case 66:
			if (!BaseGameDataDomain.IsModified(DataStates, 66))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 66);
			return GameData.Serializer.Serializer.Serialize(_isTaiwuInFulongFlameArea, dataPool);
		case 67:
			if (!BaseGameDataDomain.IsModified(DataStates, 67))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 67);
			return GameData.Serializer.Serializer.Serialize(GetVisibleMapPickups(), dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			if (BaseGameDataDomain.IsModified(_dataStatesAreas, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesAreas, (int)subId0);
			}
			break;
		case 1:
			if (BaseGameDataDomain.IsModified(DataStates, 1))
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
				_modificationsAreaBlocks0.Reset();
			}
			break;
		case 2:
			if (BaseGameDataDomain.IsModified(DataStates, 2))
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
				_modificationsAreaBlocks1.Reset();
			}
			break;
		case 3:
			if (BaseGameDataDomain.IsModified(DataStates, 3))
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
				_modificationsAreaBlocks2.Reset();
			}
			break;
		case 4:
			if (BaseGameDataDomain.IsModified(DataStates, 4))
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
				_modificationsAreaBlocks3.Reset();
			}
			break;
		case 5:
			if (BaseGameDataDomain.IsModified(DataStates, 5))
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
				_modificationsAreaBlocks4.Reset();
			}
			break;
		case 6:
			if (BaseGameDataDomain.IsModified(DataStates, 6))
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
				_modificationsAreaBlocks5.Reset();
			}
			break;
		case 7:
			if (BaseGameDataDomain.IsModified(DataStates, 7))
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
				_modificationsAreaBlocks6.Reset();
			}
			break;
		case 8:
			if (BaseGameDataDomain.IsModified(DataStates, 8))
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
				_modificationsAreaBlocks7.Reset();
			}
			break;
		case 9:
			if (BaseGameDataDomain.IsModified(DataStates, 9))
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
				_modificationsAreaBlocks8.Reset();
			}
			break;
		case 10:
			if (BaseGameDataDomain.IsModified(DataStates, 10))
			{
				BaseGameDataDomain.ResetModified(DataStates, 10);
				_modificationsAreaBlocks9.Reset();
			}
			break;
		case 11:
			if (BaseGameDataDomain.IsModified(DataStates, 11))
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
				_modificationsAreaBlocks10.Reset();
			}
			break;
		case 12:
			if (BaseGameDataDomain.IsModified(DataStates, 12))
			{
				BaseGameDataDomain.ResetModified(DataStates, 12);
				_modificationsAreaBlocks11.Reset();
			}
			break;
		case 13:
			if (BaseGameDataDomain.IsModified(DataStates, 13))
			{
				BaseGameDataDomain.ResetModified(DataStates, 13);
				_modificationsAreaBlocks12.Reset();
			}
			break;
		case 14:
			if (BaseGameDataDomain.IsModified(DataStates, 14))
			{
				BaseGameDataDomain.ResetModified(DataStates, 14);
				_modificationsAreaBlocks13.Reset();
			}
			break;
		case 15:
			if (BaseGameDataDomain.IsModified(DataStates, 15))
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
				_modificationsAreaBlocks14.Reset();
			}
			break;
		case 16:
			if (BaseGameDataDomain.IsModified(DataStates, 16))
			{
				BaseGameDataDomain.ResetModified(DataStates, 16);
				_modificationsAreaBlocks15.Reset();
			}
			break;
		case 17:
			if (BaseGameDataDomain.IsModified(DataStates, 17))
			{
				BaseGameDataDomain.ResetModified(DataStates, 17);
				_modificationsAreaBlocks16.Reset();
			}
			break;
		case 18:
			if (BaseGameDataDomain.IsModified(DataStates, 18))
			{
				BaseGameDataDomain.ResetModified(DataStates, 18);
				_modificationsAreaBlocks17.Reset();
			}
			break;
		case 19:
			if (BaseGameDataDomain.IsModified(DataStates, 19))
			{
				BaseGameDataDomain.ResetModified(DataStates, 19);
				_modificationsAreaBlocks18.Reset();
			}
			break;
		case 20:
			if (BaseGameDataDomain.IsModified(DataStates, 20))
			{
				BaseGameDataDomain.ResetModified(DataStates, 20);
				_modificationsAreaBlocks19.Reset();
			}
			break;
		case 21:
			if (BaseGameDataDomain.IsModified(DataStates, 21))
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
				_modificationsAreaBlocks20.Reset();
			}
			break;
		case 22:
			if (BaseGameDataDomain.IsModified(DataStates, 22))
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
				_modificationsAreaBlocks21.Reset();
			}
			break;
		case 23:
			if (BaseGameDataDomain.IsModified(DataStates, 23))
			{
				BaseGameDataDomain.ResetModified(DataStates, 23);
				_modificationsAreaBlocks22.Reset();
			}
			break;
		case 24:
			if (BaseGameDataDomain.IsModified(DataStates, 24))
			{
				BaseGameDataDomain.ResetModified(DataStates, 24);
				_modificationsAreaBlocks23.Reset();
			}
			break;
		case 25:
			if (BaseGameDataDomain.IsModified(DataStates, 25))
			{
				BaseGameDataDomain.ResetModified(DataStates, 25);
				_modificationsAreaBlocks24.Reset();
			}
			break;
		case 26:
			if (BaseGameDataDomain.IsModified(DataStates, 26))
			{
				BaseGameDataDomain.ResetModified(DataStates, 26);
				_modificationsAreaBlocks25.Reset();
			}
			break;
		case 27:
			if (BaseGameDataDomain.IsModified(DataStates, 27))
			{
				BaseGameDataDomain.ResetModified(DataStates, 27);
				_modificationsAreaBlocks26.Reset();
			}
			break;
		case 28:
			if (BaseGameDataDomain.IsModified(DataStates, 28))
			{
				BaseGameDataDomain.ResetModified(DataStates, 28);
				_modificationsAreaBlocks27.Reset();
			}
			break;
		case 29:
			if (BaseGameDataDomain.IsModified(DataStates, 29))
			{
				BaseGameDataDomain.ResetModified(DataStates, 29);
				_modificationsAreaBlocks28.Reset();
			}
			break;
		case 30:
			if (BaseGameDataDomain.IsModified(DataStates, 30))
			{
				BaseGameDataDomain.ResetModified(DataStates, 30);
				_modificationsAreaBlocks29.Reset();
			}
			break;
		case 31:
			if (BaseGameDataDomain.IsModified(DataStates, 31))
			{
				BaseGameDataDomain.ResetModified(DataStates, 31);
				_modificationsAreaBlocks30.Reset();
			}
			break;
		case 32:
			if (BaseGameDataDomain.IsModified(DataStates, 32))
			{
				BaseGameDataDomain.ResetModified(DataStates, 32);
				_modificationsAreaBlocks31.Reset();
			}
			break;
		case 33:
			if (BaseGameDataDomain.IsModified(DataStates, 33))
			{
				BaseGameDataDomain.ResetModified(DataStates, 33);
				_modificationsAreaBlocks32.Reset();
			}
			break;
		case 34:
			if (BaseGameDataDomain.IsModified(DataStates, 34))
			{
				BaseGameDataDomain.ResetModified(DataStates, 34);
				_modificationsAreaBlocks33.Reset();
			}
			break;
		case 35:
			if (BaseGameDataDomain.IsModified(DataStates, 35))
			{
				BaseGameDataDomain.ResetModified(DataStates, 35);
				_modificationsAreaBlocks34.Reset();
			}
			break;
		case 36:
			if (BaseGameDataDomain.IsModified(DataStates, 36))
			{
				BaseGameDataDomain.ResetModified(DataStates, 36);
				_modificationsAreaBlocks35.Reset();
			}
			break;
		case 37:
			if (BaseGameDataDomain.IsModified(DataStates, 37))
			{
				BaseGameDataDomain.ResetModified(DataStates, 37);
				_modificationsAreaBlocks36.Reset();
			}
			break;
		case 38:
			if (BaseGameDataDomain.IsModified(DataStates, 38))
			{
				BaseGameDataDomain.ResetModified(DataStates, 38);
				_modificationsAreaBlocks37.Reset();
			}
			break;
		case 39:
			if (BaseGameDataDomain.IsModified(DataStates, 39))
			{
				BaseGameDataDomain.ResetModified(DataStates, 39);
				_modificationsAreaBlocks38.Reset();
			}
			break;
		case 40:
			if (BaseGameDataDomain.IsModified(DataStates, 40))
			{
				BaseGameDataDomain.ResetModified(DataStates, 40);
				_modificationsAreaBlocks39.Reset();
			}
			break;
		case 41:
			if (BaseGameDataDomain.IsModified(DataStates, 41))
			{
				BaseGameDataDomain.ResetModified(DataStates, 41);
				_modificationsAreaBlocks40.Reset();
			}
			break;
		case 42:
			if (BaseGameDataDomain.IsModified(DataStates, 42))
			{
				BaseGameDataDomain.ResetModified(DataStates, 42);
				_modificationsAreaBlocks41.Reset();
			}
			break;
		case 43:
			if (BaseGameDataDomain.IsModified(DataStates, 43))
			{
				BaseGameDataDomain.ResetModified(DataStates, 43);
				_modificationsAreaBlocks42.Reset();
			}
			break;
		case 44:
			if (BaseGameDataDomain.IsModified(DataStates, 44))
			{
				BaseGameDataDomain.ResetModified(DataStates, 44);
				_modificationsAreaBlocks43.Reset();
			}
			break;
		case 45:
			if (BaseGameDataDomain.IsModified(DataStates, 45))
			{
				BaseGameDataDomain.ResetModified(DataStates, 45);
				_modificationsAreaBlocks44.Reset();
			}
			break;
		case 46:
			if (BaseGameDataDomain.IsModified(DataStates, 46))
			{
				BaseGameDataDomain.ResetModified(DataStates, 46);
				_modificationsBrokenAreaBlocks.Reset();
			}
			break;
		case 47:
			if (BaseGameDataDomain.IsModified(DataStates, 47))
			{
				BaseGameDataDomain.ResetModified(DataStates, 47);
				_modificationsBornAreaBlocks.Reset();
			}
			break;
		case 48:
			if (BaseGameDataDomain.IsModified(DataStates, 48))
			{
				BaseGameDataDomain.ResetModified(DataStates, 48);
				_modificationsGuideAreaBlocks.Reset();
			}
			break;
		case 49:
			if (BaseGameDataDomain.IsModified(DataStates, 49))
			{
				BaseGameDataDomain.ResetModified(DataStates, 49);
				_modificationsSecretVillageAreaBlocks.Reset();
			}
			break;
		case 50:
			if (BaseGameDataDomain.IsModified(DataStates, 50))
			{
				BaseGameDataDomain.ResetModified(DataStates, 50);
				_modificationsBrokenPerformAreaBlocks.Reset();
			}
			break;
		case 51:
			if (BaseGameDataDomain.IsModified(DataStates, 51))
			{
				BaseGameDataDomain.ResetModified(DataStates, 51);
				_modificationsTravelRouteDict.Reset();
			}
			break;
		case 52:
			if (BaseGameDataDomain.IsModified(DataStates, 52))
			{
				BaseGameDataDomain.ResetModified(DataStates, 52);
				_modificationsBornStateTravelRouteDict.Reset();
			}
			break;
		case 53:
			if (BaseGameDataDomain.IsModified(_dataStatesAnimalPlaceData, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesAnimalPlaceData, (int)subId0);
			}
			break;
		case 54:
			if (BaseGameDataDomain.IsModified(_dataStatesCricketPlaceData, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesCricketPlaceData, (int)subId0);
			}
			break;
		case 55:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 56:
			if (BaseGameDataDomain.IsModified(_dataStatesSwordTombLocations, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesSwordTombLocations, (int)subId0);
			}
			break;
		case 57:
			if (BaseGameDataDomain.IsModified(DataStates, 57))
			{
				BaseGameDataDomain.ResetModified(DataStates, 57);
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
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		return dataId switch
		{
			0 => BaseGameDataDomain.IsModified(_dataStatesAreas, (int)subId0), 
			1 => BaseGameDataDomain.IsModified(DataStates, 1), 
			2 => BaseGameDataDomain.IsModified(DataStates, 2), 
			3 => BaseGameDataDomain.IsModified(DataStates, 3), 
			4 => BaseGameDataDomain.IsModified(DataStates, 4), 
			5 => BaseGameDataDomain.IsModified(DataStates, 5), 
			6 => BaseGameDataDomain.IsModified(DataStates, 6), 
			7 => BaseGameDataDomain.IsModified(DataStates, 7), 
			8 => BaseGameDataDomain.IsModified(DataStates, 8), 
			9 => BaseGameDataDomain.IsModified(DataStates, 9), 
			10 => BaseGameDataDomain.IsModified(DataStates, 10), 
			11 => BaseGameDataDomain.IsModified(DataStates, 11), 
			12 => BaseGameDataDomain.IsModified(DataStates, 12), 
			13 => BaseGameDataDomain.IsModified(DataStates, 13), 
			14 => BaseGameDataDomain.IsModified(DataStates, 14), 
			15 => BaseGameDataDomain.IsModified(DataStates, 15), 
			16 => BaseGameDataDomain.IsModified(DataStates, 16), 
			17 => BaseGameDataDomain.IsModified(DataStates, 17), 
			18 => BaseGameDataDomain.IsModified(DataStates, 18), 
			19 => BaseGameDataDomain.IsModified(DataStates, 19), 
			20 => BaseGameDataDomain.IsModified(DataStates, 20), 
			21 => BaseGameDataDomain.IsModified(DataStates, 21), 
			22 => BaseGameDataDomain.IsModified(DataStates, 22), 
			23 => BaseGameDataDomain.IsModified(DataStates, 23), 
			24 => BaseGameDataDomain.IsModified(DataStates, 24), 
			25 => BaseGameDataDomain.IsModified(DataStates, 25), 
			26 => BaseGameDataDomain.IsModified(DataStates, 26), 
			27 => BaseGameDataDomain.IsModified(DataStates, 27), 
			28 => BaseGameDataDomain.IsModified(DataStates, 28), 
			29 => BaseGameDataDomain.IsModified(DataStates, 29), 
			30 => BaseGameDataDomain.IsModified(DataStates, 30), 
			31 => BaseGameDataDomain.IsModified(DataStates, 31), 
			32 => BaseGameDataDomain.IsModified(DataStates, 32), 
			33 => BaseGameDataDomain.IsModified(DataStates, 33), 
			34 => BaseGameDataDomain.IsModified(DataStates, 34), 
			35 => BaseGameDataDomain.IsModified(DataStates, 35), 
			36 => BaseGameDataDomain.IsModified(DataStates, 36), 
			37 => BaseGameDataDomain.IsModified(DataStates, 37), 
			38 => BaseGameDataDomain.IsModified(DataStates, 38), 
			39 => BaseGameDataDomain.IsModified(DataStates, 39), 
			40 => BaseGameDataDomain.IsModified(DataStates, 40), 
			41 => BaseGameDataDomain.IsModified(DataStates, 41), 
			42 => BaseGameDataDomain.IsModified(DataStates, 42), 
			43 => BaseGameDataDomain.IsModified(DataStates, 43), 
			44 => BaseGameDataDomain.IsModified(DataStates, 44), 
			45 => BaseGameDataDomain.IsModified(DataStates, 45), 
			46 => BaseGameDataDomain.IsModified(DataStates, 46), 
			47 => BaseGameDataDomain.IsModified(DataStates, 47), 
			48 => BaseGameDataDomain.IsModified(DataStates, 48), 
			49 => BaseGameDataDomain.IsModified(DataStates, 49), 
			50 => BaseGameDataDomain.IsModified(DataStates, 50), 
			51 => BaseGameDataDomain.IsModified(DataStates, 51), 
			52 => BaseGameDataDomain.IsModified(DataStates, 52), 
			53 => BaseGameDataDomain.IsModified(_dataStatesAnimalPlaceData, (int)subId0), 
			54 => BaseGameDataDomain.IsModified(_dataStatesCricketPlaceData, (int)subId0), 
			55 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			56 => BaseGameDataDomain.IsModified(_dataStatesSwordTombLocations, (int)subId0), 
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
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
		case 62:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(62, DataStates, CacheInfluences, context);
			break;
		case 63:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(63, DataStates, CacheInfluences, context);
			break;
		case 64:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(64, DataStates, CacheInfluences, context);
			break;
		case 65:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(65, DataStates, CacheInfluences, context);
			break;
		case 67:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(67, DataStates, CacheInfluences, context);
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
		case 8:
		case 9:
		case 10:
		case 11:
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
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
		case 30:
		case 31:
		case 32:
		case 33:
		case 34:
		case 35:
		case 36:
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
		case 48:
		case 49:
		case 50:
		case 51:
		case 52:
		case 53:
		case 54:
		case 55:
		case 56:
		case 57:
		case 58:
		case 59:
		case 60:
		case 61:
		case 66:
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		uint num;
		switch (operation.DataId)
		{
		case 0:
			ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref(operation, pResult, _areas, 139);
			goto IL_05e4;
		case 1:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks0);
			goto IL_05e4;
		case 2:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks1);
			goto IL_05e4;
		case 3:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks2);
			goto IL_05e4;
		case 4:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks3);
			goto IL_05e4;
		case 5:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks4);
			goto IL_05e4;
		case 6:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks5);
			goto IL_05e4;
		case 7:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks6);
			goto IL_05e4;
		case 8:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks7);
			goto IL_05e4;
		case 9:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks8);
			goto IL_05e4;
		case 10:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks9);
			goto IL_05e4;
		case 11:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks10);
			goto IL_05e4;
		case 12:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks11);
			goto IL_05e4;
		case 13:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks12);
			goto IL_05e4;
		case 14:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks13);
			goto IL_05e4;
		case 15:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks14);
			goto IL_05e4;
		case 16:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks15);
			goto IL_05e4;
		case 17:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks16);
			goto IL_05e4;
		case 18:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks17);
			goto IL_05e4;
		case 19:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks18);
			goto IL_05e4;
		case 20:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks19);
			goto IL_05e4;
		case 21:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks20);
			goto IL_05e4;
		case 22:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks21);
			goto IL_05e4;
		case 23:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks22);
			goto IL_05e4;
		case 24:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks23);
			goto IL_05e4;
		case 25:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks24);
			goto IL_05e4;
		case 26:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks25);
			goto IL_05e4;
		case 27:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks26);
			goto IL_05e4;
		case 28:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks27);
			goto IL_05e4;
		case 29:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks28);
			goto IL_05e4;
		case 30:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks29);
			goto IL_05e4;
		case 31:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks30);
			goto IL_05e4;
		case 32:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks31);
			goto IL_05e4;
		case 33:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks32);
			goto IL_05e4;
		case 34:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks33);
			goto IL_05e4;
		case 35:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks34);
			goto IL_05e4;
		case 36:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks35);
			goto IL_05e4;
		case 37:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks36);
			goto IL_05e4;
		case 38:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks37);
			goto IL_05e4;
		case 39:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks38);
			goto IL_05e4;
		case 40:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks39);
			goto IL_05e4;
		case 41:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks40);
			goto IL_05e4;
		case 42:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks41);
			goto IL_05e4;
		case 43:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks42);
			goto IL_05e4;
		case 44:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks43);
			goto IL_05e4;
		case 45:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _areaBlocks44);
			goto IL_05e4;
		case 46:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _brokenAreaBlocks);
			goto IL_05e4;
		case 47:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _bornAreaBlocks);
			goto IL_05e4;
		case 48:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _guideAreaBlocks);
			goto IL_05e4;
		case 49:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _secretVillageAreaBlocks);
			goto IL_05e4;
		case 50:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _brokenPerformAreaBlocks);
			goto IL_05e4;
		case 51:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _travelRouteDict);
			goto IL_05e4;
		case 52:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _bornStateTravelRouteDict);
			goto IL_05e4;
		case 53:
			ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref(operation, pResult, _animalPlaceData, 139);
			goto IL_05e4;
		case 54:
			ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref(operation, pResult, _cricketPlaceData, 139);
			goto IL_05e4;
		case 55:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<short, GameData.Utilities.ShortList>(operation, pResult, (IDictionary<short, GameData.Utilities.ShortList>)_regularAreaNearList);
			goto IL_05e4;
		case 56:
			ResponseProcessor.ProcessElementList_CustomType_Fixed_Value<Location>(operation, pResult, _swordTombLocations, 8, 4);
			goto IL_05e4;
		case 57:
			ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Ref_Single(operation, pResult, _travelInfo);
			goto IL_05e4;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
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
			{
				throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
			}
			IL_05e4:
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
					DomainManager.Global.CompleteLoading(2);
				}
			}
			break;
		}
	}

	private void InitializeInternalDataOfCollections()
	{
	}
}
