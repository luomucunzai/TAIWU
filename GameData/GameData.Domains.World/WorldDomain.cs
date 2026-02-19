using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using Config;
using Config.ConfigCells;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Common.Binary;
using GameData.Common.SingleValueCollection;
using GameData.Common.WorkerThread;
using GameData.DLC;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.Relation;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Domains.Map;
using GameData.Domains.Mod;
using GameData.Domains.Organization;
using GameData.Domains.Organization.Display;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Domains.World.SectMainStory;
using GameData.Domains.World.Task;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.World;

[GameDataDomain(1)]
public class WorldDomain : BaseGameDataDomain
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private uint _worldId;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private sbyte _xiangshuProgress;

	[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 9)]
	private readonly XiangshuAvatarTaskStatus[] _xiangshuAvatarTaskStatuses;

	[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 9)]
	private sbyte[] _xiangshuAvatarTasksInOrder;

	[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 15)]
	private readonly sbyte[] _stateTaskStatuses;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private short _mainStoryLineProgress;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private bool _beatRanChenZi;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private ulong _worldFunctionsStatuses;

	[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
	private readonly Dictionary<int, string> _customTexts;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private int _nextCustomTextId;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private WorldStateData _worldStateData;

	private Version _currWorldGameVersion;

	[DomainData(DomainDataType.Binary, true, false, true, true, CollectionCapacity = 1024)]
	private readonly InstantNotificationCollection _instantNotifications;

	private int _instantNotificationsCommittedOffset;

	private readonly List<short> _instantNotificationTemplateIds = new List<short>();

	private readonly Dictionary<short, string> _instantNotificationTemplateId2Name = new Dictionary<short, string>();

	private Type _instantNotificationCollectionType;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _onHandingMonthlyEventBlock;

	private MonthlyEventCollection _monthlyEventCollection;

	private bool _isTaiwuDying;

	private bool _isTaiwuGettingCompletelyInfected;

	private bool _isTaiwuVillageDestroyed;

	public bool IsTaiwuHunterDie;

	private bool _isTaiwuDyingOfDystocia;

	private int _toRepayKindnessCharId = -1;

	private static readonly BinaryHeap<(int offset, int score)> SpecialEvents = new BinaryHeap<(int, int)>(((int offset, int score) a, (int offset, int score) b) => a.score.CompareTo(b.score));

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _sortedMonthlyNotificationSortingGroups;

	[Obsolete("Use ExtraDomain._previousMonthlyNotifications instead.")]
	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private MonthlyNotificationCollection _lastMonthlyNotifications;

	private MonthlyNotificationCollection _currMonthlyNotifications = new MonthlyNotificationCollection();

	private readonly List<short> _monthlyNotificationTemplateIds = new List<short>();

	private readonly Dictionary<short, string> _monthlyNotificationTemplateId2Name = new Dictionary<short, string>();

	private Type _monthlyNotificationCollectionType;

	private readonly List<GameData.Domains.Character.Character> _candidatesCharacters = new List<GameData.Domains.Character.Character>();

	private readonly List<TemplateKey> _candidateItems = new List<TemplateKey>();

	private readonly List<short> _candidateCombatSkills = new List<short>();

	private readonly List<short> _candidateSettlements = new List<short>();

	private readonly List<short> _candidateBuildings = new List<short>();

	private readonly List<short> _candidateAdventures = new List<short>();

	private readonly List<(short colorId, short partId)> _candidateCrickets = new List<(short, short)>();

	private readonly List<short> _candidateChickens = new List<short>();

	private readonly List<short> _canTestMonthlyEventTemplateIdList = new List<short>
	{
		87, 88, 89, 90, 66, 68, 70, 72, 73, 74,
		75, 76, 77, 78, 79, 80, 81, 82, 83, 84,
		85, 86, 114, 115, 116, 117, 118, 353, 354, 290,
		61
	};

	private readonly List<(short monthlyEventTemplateId, int selfCharId, int targetCharId)> _testMonthlyEventList = new List<(short, int, int)>();

	private float _probAdjustOfCreatingCharacter;

	private int _sectMainStoryCombatTimesShaolin;

	private bool _sectMainStoryDefeatingXuannv;

	private bool _sectMainStoryLifeLinkUpdated;

	private readonly HashSet<sbyte> _storyStatus = new HashSet<sbyte>();

	private readonly List<(GameData.Domains.Character.Character character, int health, int leftMaxHealth)> _tmpLifeGateChars = new List<(GameData.Domains.Character.Character, int, int)>();

	private readonly List<(GameData.Domains.Character.Character character, int distributableHealth)> _tmpDeathGateChars = new List<(GameData.Domains.Character.Character, int)>();

	private Dictionary<int, (int index, bool isLifeGate)> _baihuaLinkedCharacters;

	private sbyte _baihuaLifeLinkNeiliType;

	private Dictionary<short, List<Func<bool>>> _sectMainStoryTriggerConditions = new Dictionary<short, List<Func<bool>>>
	{
		{
			1,
			new List<Func<bool>> { ShaolinMainStoryTrigger0, ShaolinMainStoryTrigger1 }
		},
		{
			2,
			new List<Func<bool>> { EMeiMainStoryTrigger0, EMeiMainStoryTrigger1 }
		},
		{
			3,
			new List<Func<bool>> { BaihuaMainStoryTrigger0, BaihuaMainStoryTrigger1 }
		},
		{
			4,
			new List<Func<bool>> { WudangMainStoryTrigger0, WudangMainStoryTrigger1 }
		},
		{
			5,
			new List<Func<bool>> { YuanshanMainStoryTrigger0, YuanshanMainStoryTrigger1, YuanshanMainStoryTrigger2 }
		},
		{
			6,
			new List<Func<bool>> { ShixiangMainStoryTrigger0, ShixiangMainStoryTrigger1 }
		},
		{
			7,
			new List<Func<bool>> { RanshanMainStoryTrigger0, RanshanMainStoryTrigger1, RanshanMainStoryTrigger2 }
		},
		{
			8,
			new List<Func<bool>> { XuannvMainStoryTrigger0, XuannvMainStoryTrigger1 }
		},
		{
			9,
			new List<Func<bool>> { ZhujianMainStoryTrigger0, ZhujianMainStoryTrigger1 }
		},
		{
			10,
			new List<Func<bool>> { KongsangMainStoryTrigger0, KongsangMainStoryTrigger1 }
		},
		{
			11,
			new List<Func<bool>> { JingangMainStoryTrigger0, JingangMainStoryTrigger1 }
		},
		{
			12,
			new List<Func<bool>> { WuxianMainStoryTrigger0, WuxianMainStoryTrigger1 }
		},
		{
			13,
			new List<Func<bool>>()
		},
		{
			14,
			new List<Func<bool>> { FulongMainStoryTrigger0, FulongMainStoryTrigger1 }
		},
		{
			15,
			new List<Func<bool>> { XuehouMainStoryTrigger0 }
		}
	};

	public const sbyte ArchiveFilesBackupsCount = 3;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private byte _worldPopulationType;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private byte _characterLifespanType;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private byte _combatDifficulty;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private byte _hereticsAmountType;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private byte _bossInvasionSpeedType;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private byte _worldResourceAmountType;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private bool _allowRandomTaiwuHeir;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private bool _restrictOptionsBehaviorType;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private sbyte _taiwuVillageStateTemplateId;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private sbyte _taiwuVillageLandFormType;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _hideTaiwuOriginalSurname;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _allowExecute;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private sbyte _archiveFilesBackupInterval;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private sbyte _archiveFilesBackupCount;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private int _worldStandardPopulation;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private List<TaskData> _currTaskList;

	[DomainData(DomainDataType.SingleValue, false, true, true, true)]
	private List<TaskDisplayData> _sortedTaskList;

	private const int MonitorIntervalOfAdvancingMonth = 100;

	private const int MinAdvanceMonthTimeMilliseconds = 2000;

	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private int _currDate;

	[Obsolete("Use ExtraDomain._actionPointCurrMonth instead.")]
	[DomainData(DomainDataType.SingleValue, true, false, true, false)]
	private sbyte _daysInCurrMonth;

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private sbyte _advancingMonthState;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[35][];

	private SpinLock _spinLockXiangshuProgress = new SpinLock(enableThreadOwnerTracking: false);

	private static readonly DataInfluence[][] CacheInfluencesXiangshuAvatarTaskStatuses = new DataInfluence[9][];

	private readonly byte[] _dataStatesXiangshuAvatarTaskStatuses = new byte[3];

	private static readonly DataInfluence[][] CacheInfluencesStateTaskStatuses = new DataInfluence[15][];

	private readonly byte[] _dataStatesStateTaskStatuses = new byte[4];

	private SingleValueCollectionModificationCollection<int> _modificationsCustomTexts = SingleValueCollectionModificationCollection<int>.Create();

	private BinaryModificationCollection _modificationsInstantNotifications = BinaryModificationCollection.Create();

	private SpinLock _spinLockCurrTaskList = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockSortedTaskList = new SpinLock(enableThreadOwnerTracking: false);

	private SpinLock _spinLockWorldStateData = new SpinLock(enableThreadOwnerTracking: false);

	private Queue<uint> _pendingLoadingOperationIds;

	private void OnInitializedDomainData()
	{
		_monthlyEventCollection = new MonthlyEventCollection();
	}

	private void InitializeOnInitializeGameDataModule()
	{
	}

	private void InitializeOnEnterNewWorld()
	{
		DataContext currentThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
		_worldId = currentThreadDataContext.Random.NextUInt();
		_xiangshuProgress = 0;
		_mainStoryLineProgress = 0;
		_worldFunctionsStatuses = 0uL;
		_nextCustomTextId = 0;
		_currDate = 8;
		_advancingMonthState = 0;
		_instantNotificationsCommittedOffset = 0;
		for (sbyte b = 0; b < _xiangshuAvatarTasksInOrder.Length; b++)
		{
			_xiangshuAvatarTasksInOrder[b] = b;
		}
		CollectionUtils.Shuffle(currentThreadDataContext.Random, _xiangshuAvatarTasksInOrder);
		SetWorldFunctionsStatus(currentThreadDataContext, 19);
		SetWorldFunctionsStatus(currentThreadDataContext, 20);
		SetWorldFunctionsStatus(currentThreadDataContext, 17);
		Logger.Info($"EnterNewWorld: {_worldId}");
	}

	private void OnLoadedArchiveData()
	{
		_instantNotificationsCommittedOffset = _instantNotifications.Size;
		Logger.Info($"LoadWorld: {_worldId}");
	}

	public override void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
	{
		InitializeBaihuaLinkedCharacters(context);
	}

	public override void FixAbnormalDomainArchiveData(DataContext context)
	{
		if (DomainManager.World.GetMainStoryLineProgress() >= 7 && !IsCurrWorldBeforeVersion(0, 0, 63))
		{
			SetWorldFunctionsStatus(context, 5);
		}
		if (GetWorldFunctionsStatus(4))
		{
			SetWorldFunctionsStatus(context, 3);
		}
		if (DomainManager.Character.TryGetFixedCharacterByTemplateId(441, out var _))
		{
			SetWorldFunctionsStatus(context, 21);
		}
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 28 }, Condition = InfluenceCondition.CharIsTaiwu)]
	private sbyte CalcXiangshuProgress()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (taiwu == null)
		{
			return 0;
		}
		int consummateLevel = taiwu.GetConsummateLevel();
		return (sbyte)Math.Clamp(consummateLevel, 0, 18);
	}

	[SingleValueDependency(1, new ushort[] { 1, 5, 27 })]
	[ElementListDependency(1, 4, 15)]
	[SingleValueDependency(5, new ushort[] { 25, 0, 8, 9, 58, 70, 34, 74, 75 })]
	[SingleValueDependency(2, new ushort[] { 57 })]
	[ObjectCollectionDependency(4, 0, new ushort[] { 21, 26, 44, 65 }, Condition = InfluenceCondition.CharIsTaiwu)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 105, 104 }, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
	[ElementListDependency(10, 17, 45)]
	[SingleValueDependency(19, new ushort[] { 56 })]
	[ObjectCollectionDependency(4, 0, new ushort[] { 26 }, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
	[SingleValueDependency(19, new ushort[] { 157 })]
	[SingleValueDependency(19, new ushort[] { 265 })]
	[SingleValueCollectionDependency(19, new ushort[] { 163, 245, 223 })]
	[SingleValueDependency(2, new ushort[] { 66 })]
	[SingleValueCollectionDependency(19, new ushort[] { 247 }, Scope = InfluenceScope.TaiwuChar)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 17 }, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 95 }, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 19 }, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
	[SingleValueDependency(9, new ushort[] { 15 })]
	[ObjectCollectionDependency(4, 0, new ushort[] { 112 }, Condition = InfluenceCondition.CharIsInTaiwuGroup)]
	private WorldStateData CalcWorldStateData()
	{
		WorldStateData data = default(WorldStateData);
		data.DetectWarehouseOverload();
		data.DetectResourceOverload();
		data.DetectInventoryOverload();
		data.DetectInjuries();
		data.DetectPoisons();
		data.DetectDisorderOfQi();
		data.DetectTeammateInjuries();
		data.DetectXiangshuInvasionProgress();
		data.DetectXiangshuInfection();
		data.DetectMainStory();
		data.DetectXiangshuAvatars();
		data.DetectMartialArtTournament();
		data.DetectChangeWorldCreation();
		data.DetectLoongDebuff();
		data.DetectInFulongFlameArea();
		data.DetectTribulation();
		data.DetectSectMainStory();
		data.DetectTaiwuWanted();
		data.DetectTeammateDying();
		data.DetectHomelessVillager();
		data.DetectNeiliConflicting();
		return data;
	}

	[ElementListDependency(1, 2, 9)]
	[SingleValueDependency(1, new ushort[] { 5, 7 })]
	[SingleValueDependency(12, new ushort[] { 0 })]
	[SingleValueDependency(5, new ushort[] { 34 })]
	[SingleValueDependency(19, new ushort[] { 56 })]
	[SingleValueDependency(19, new ushort[] { 246 })]
	[ElementListDependency(10, 17, 139)]
	[SingleValueCollectionDependency(19, new ushort[] { 245 })]
	[SingleValueCollectionDependency(9, new ushort[] { 0 })]
	[ObjectCollectionDependency(4, 0, new ushort[] { 58, 56 }, Condition = InfluenceCondition.CharIsTaskRelated)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	private void CalcCurrTaskList(List<TaskData> value)
	{
		foreach (TaskChainItem item3 in (IEnumerable<TaskChainItem>)TaskChain.Instance)
		{
			if (!CheckCurrMainStoryLineProgressInRange(item3.MainStoryLineMin, item3.MainStoryLineMax) || item3.TaskList.Count <= 0 || (item3.StartConditions.Count > 0 && !item3.StartConditions.TrueForAll(TaskConditionChecker.CheckCondition)) || (item3.RemoveCondtions.Count > 0 && item3.RemoveCondtions.TrueForAll(TaskConditionChecker.CheckCondition)))
			{
				continue;
			}
			foreach (int task in item3.TaskList)
			{
				TaskInfoItem taskInfoItem = TaskInfo.Instance[task];
				if (!taskInfoItem.IsTriggeredTask && CheckCurrMainStoryLineProgressInRange(taskInfoItem.MainStoryLineMin, taskInfoItem.MainStoryLineMax) && (taskInfoItem.RunCondition.Count <= 0 || taskInfoItem.RunCondition.TrueForAll(TaskConditionChecker.CheckCondition)) && (taskInfoItem.FinishCondition.Count <= 0 || !taskInfoItem.FinishCondition.TrueForAll(TaskConditionChecker.CheckCondition)))
				{
					bool flag = taskInfoItem.BlockCondition.Count > 0 && taskInfoItem.BlockCondition.TrueForAll(TaskConditionChecker.CheckCondition);
					TaskData item = new TaskData
					{
						TaskChainId = item3.TemplateId,
						TaskInfoId = task,
						TaskStatus = (byte)(flag ? 1 : 0)
					};
					value.Add(item);
					if (item3.Type == ETaskChainType.Line)
					{
						break;
					}
				}
			}
		}
		List<TaskData> extraTriggeredTasks = DomainManager.Extra.GetExtraTriggeredTasks();
		foreach (TaskData item4 in extraTriggeredTasks)
		{
			TaskInfoItem taskInfoItem2 = TaskInfo.Instance[item4.TaskInfoId];
			if (CheckCurrMainStoryLineProgressInRange(taskInfoItem2.MainStoryLineMin, taskInfoItem2.MainStoryLineMax))
			{
				bool flag2 = taskInfoItem2.BlockCondition.Count > 0 && taskInfoItem2.BlockCondition.TrueForAll(TaskConditionChecker.CheckCondition);
				TaskData item2 = item4;
				item2.TaskStatus = (byte)(flag2 ? 1 : 0);
				value.Add(item2);
			}
		}
	}

	[SingleValueDependency(1, new ushort[] { 30 })]
	[SingleValueDependency(19, new ushort[] { 57 })]
	[ObjectCollectionDependency(7, 0, new ushort[] { 1 }, Condition = InfluenceCondition.CombatSkillIsLearnedByTaiwu)]
	private void CalcSortedTaskList(List<TaskDisplayData> value)
	{
		List<TaskData> currTaskList = GetCurrTaskList();
		List<int> sortingOrder = DomainManager.Extra.GetTaskSortingOrder();
		foreach (TaskData item2 in currTaskList)
		{
			TaskInfoItem taskInfoItem = TaskInfo.Instance[item2.TaskInfoId];
			if (taskInfoItem == null)
			{
				PredefinedLog.Show(10, item2.TaskInfoId);
				continue;
			}
			TaskDisplayData item = new TaskDisplayData
			{
				DisplayType = 0,
				TargetLocation = Location.Invalid,
				SkillIdList = GameData.Utilities.ShortList.Create(),
				CountDown = -1,
				SettlementNameData = new SettlementNameRelatedData
				{
					RandomNameId = -1,
					MapBlockTemplateId = -1
				},
				StringArray = null,
				InnerTaskData = item2
			};
			foreach (short item3 in taskInfoItem.CharacterTemplateId)
			{
				if (DomainManager.Character.TryGetFixedCharacterByTemplateId(item3, out var character) && character.GetLocation().IsValid())
				{
					item.TargetLocation = character.GetLocation();
				}
			}
			TaskChainItem taskChainItem = TaskChain.Instance[item2.TaskChainId];
			ETaskChainGroup eTaskChainGroup = taskChainItem.Group;
			if (1 == 0)
			{
			}
			EventArgBox eventArgBox = eTaskChainGroup switch
			{
				ETaskChainGroup.MainStory => DomainManager.TaiwuEvent.GetGlobalEventArgumentBox(), 
				ETaskChainGroup.SectMainStory => DomainManager.Extra.GetSectMainStoryEventArgBox(taskChainItem.Sect), 
				_ => null, 
			};
			if (1 == 0)
			{
			}
			EventArgBox eventArgBox2 = eventArgBox;
			int groupId;
			if (eventArgBox2 != null)
			{
				if (!string.IsNullOrEmpty(taskInfoItem.EventArgBoxKey))
				{
					if (taskInfoItem.EventArgBoxKey == "ConchShip_PresetKey_StudyForBodhidharmaChallenge")
					{
						item.DisplayType |= 64;
						item.CountDown = eventArgBox2.GetInt(taskInfoItem.EventArgBoxKey);
					}
					else if (taskInfoItem.EventArgBoxKey == "ConchShip_PresetKey_SanZongBiWuCountDown" || taskInfoItem.EventArgBoxKey == "ConchShip_PresetKey_FulongAdventureOneCountDown" || taskInfoItem.EventArgBoxKey == "ConchShip_PresetKey_FulongAdventureThreeCountDown")
					{
						item.DisplayType |= 8;
						item.CountDown = eventArgBox2.GetInt(taskInfoItem.EventArgBoxKey);
					}
					else
					{
						if (taskInfoItem.TemplateId == 309)
						{
							item.DisplayType |= 128;
							short arg = -1;
							eventArgBox2.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", ref arg);
							sbyte arg2 = -1;
							eventArgBox2.Get("ConchShip_PresetKey_BaihuaLeukoKillsFiveElementsType", ref arg2);
							Settlement settlement = DomainManager.Organization.GetSettlement(arg);
							List<SettlementNameRelatedData> settlementNameRelatedData = DomainManager.Organization.GetSettlementNameRelatedData(new List<short> { settlement.GetId() });
							item.TargetLocation = DomainManager.Organization.GetSettlement(arg).GetLocation();
							item.SettlementNameData = settlementNameRelatedData[0];
							item.StringArray = new string[2];
							item.StringArray[0] = arg2.ToString();
							item.StringArray[1] = MathF.Max(0f, 1 - DomainManager.World.BaihuaGroupMeetCount(isLeuko: true, out groupId)).ToString();
							value.Add(item);
							continue;
						}
						if (taskInfoItem.TemplateId == 311)
						{
							item.DisplayType |= 128;
							short arg3 = -1;
							eventArgBox2.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", ref arg3);
							sbyte arg4 = -1;
							eventArgBox2.Get("ConchShip_PresetKey_BaihuaMelanoKillsFiveElementsType", ref arg4);
							Settlement settlement2 = DomainManager.Organization.GetSettlement(arg3);
							List<SettlementNameRelatedData> settlementNameRelatedData2 = DomainManager.Organization.GetSettlementNameRelatedData(new List<short> { settlement2.GetId() });
							item.TargetLocation = DomainManager.Organization.GetSettlement(arg3).GetLocation();
							item.SettlementNameData = settlementNameRelatedData2[0];
							item.StringArray = new string[2];
							item.StringArray[0] = arg4.ToString();
							item.StringArray[1] = MathF.Max(0f, 1 - DomainManager.World.BaihuaGroupMeetCount(isLeuko: false, out groupId)).ToString();
							value.Add(item);
							continue;
						}
						if (!item.TargetLocation.IsValid())
						{
							if (eventArgBox2.Get<Location>(taskInfoItem.EventArgBoxKey, out item.TargetLocation))
							{
								MapBlockData block = DomainManager.Map.GetBlock(item.TargetLocation);
								if (block.IsCityTown())
								{
									Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(item.TargetLocation);
									List<SettlementNameRelatedData> settlementNameRelatedData3 = DomainManager.Organization.GetSettlementNameRelatedData(new List<short> { settlementByLocation.GetId() });
									item.SettlementNameData = settlementNameRelatedData3[0];
								}
							}
							else
							{
								short arg5 = -1;
								eventArgBox2.Get(taskInfoItem.EventArgBoxKey, ref arg5);
								item.TargetLocation = DomainManager.Organization.GetSettlement(arg5).GetLocation();
								List<SettlementNameRelatedData> settlementNameRelatedData4 = DomainManager.Organization.GetSettlementNameRelatedData(new List<short> { arg5 });
								item.SettlementNameData = settlementNameRelatedData4[0];
							}
						}
					}
				}
				string[] combatSkillIdsEventArgBoxKey = taskInfoItem.CombatSkillIdsEventArgBoxKey;
				if (combatSkillIdsEventArgBoxKey != null && combatSkillIdsEventArgBoxKey.Length > 0)
				{
					item.DisplayType |= 1;
					string[] combatSkillIdsEventArgBoxKey2 = taskInfoItem.CombatSkillIdsEventArgBoxKey;
					foreach (string key in combatSkillIdsEventArgBoxKey2)
					{
						short arg6 = -1;
						if (eventArgBox2.Get(key, ref arg6) && arg6 > -1 && DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(DomainManager.Taiwu.GetTaiwuCharId(), arg6), out var element) && !element.GetRevoked() && !CombatSkillStateHelper.IsBrokenOut(element.GetActivationState()))
						{
							item.SkillIdList.Items.Add(arg6);
						}
					}
				}
				combatSkillIdsEventArgBoxKey = taskInfoItem.SkillIdsEventArgBoxKey;
				if (combatSkillIdsEventArgBoxKey != null && combatSkillIdsEventArgBoxKey.Length > 0)
				{
					item.DisplayType |= 4;
					string[] skillIdsEventArgBoxKey = taskInfoItem.SkillIdsEventArgBoxKey;
					foreach (string key2 in skillIdsEventArgBoxKey)
					{
						short num = eventArgBox2.GetShort(key2);
						if (num > -1)
						{
							item.SkillIdList.Items.Add(num);
						}
					}
				}
				combatSkillIdsEventArgBoxKey = taskInfoItem.StringArrayEventArgBoxKey;
				if (combatSkillIdsEventArgBoxKey != null && combatSkillIdsEventArgBoxKey.Length > 0)
				{
					item.DisplayType |= 32;
					item.StringArray = new string[taskInfoItem.StringArrayEventArgBoxKey.Length];
					int k = 0;
					for (int num2 = taskInfoItem.StringArrayEventArgBoxKey.Length; k < num2; k++)
					{
						item.StringArray[k] = eventArgBox2.GetString(taskInfoItem.StringArrayEventArgBoxKey[k]);
					}
				}
			}
			if (item.TargetLocation.IsValid())
			{
				item.DisplayType |= 2;
			}
			if (taskInfoItem.FrontEndKey != null)
			{
				item.DisplayType |= 16;
			}
			groupId = taskInfoItem.TemplateId;
			if (groupId >= 268 && groupId <= 272)
			{
				short elementId = (short)(taskInfoItem.TemplateId - 268 + 686);
				if (DomainManager.Extra.TryGetElement_FiveLoongDict(elementId, out var value2))
				{
					item.DisplayType |= 2;
					item.TargetLocation = value2.LoongCurrentLocation;
				}
			}
			if (taskInfoItem.TemplateId == 342)
			{
				item.DisplayType |= 256;
				item.TargetLocations = new List<Location>();
				if (DomainManager.Building.ChickenMapInfo == null)
				{
					DomainManager.Building.ClickChickenMap(DataContextManager.GetCurrentThreadDataContext());
				}
				for (int l = 0; l < DomainManager.Building.ChickenMapInfo.Count; l++)
				{
					short settlementId = DomainManager.Building.ChickenMapInfo[l];
					Settlement settlement3 = DomainManager.Organization.GetSettlement(settlementId);
					item.TargetLocations.Add(settlement3.GetLocation());
				}
				List<SettlementNameRelatedData> settlementNameRelatedData5 = DomainManager.Organization.GetSettlementNameRelatedData(DomainManager.Building.ChickenMapInfo);
				item.SettlementNameDatas = settlementNameRelatedData5;
			}
			value.Add(item);
		}
		value.Sort(delegate(TaskDisplayData a, TaskDisplayData b)
		{
			if (a.InnerTaskData.IsBlocked != b.InnerTaskData.IsBlocked)
			{
				return a.InnerTaskData.IsBlocked.CompareTo(b.InnerTaskData.IsBlocked);
			}
			if (sortingOrder.IndexOf(a.InnerTaskData.TaskInfoId) != -1)
			{
				return -1;
			}
			if (sortingOrder.IndexOf(b.InnerTaskData.TaskInfoId) != -1)
			{
				return 1;
			}
			TaskInfoItem taskInfoItem2 = TaskInfo.Instance[a.InnerTaskData.TaskInfoId];
			TaskInfoItem taskInfoItem3 = TaskInfo.Instance[b.InnerTaskData.TaskInfoId];
			return (taskInfoItem2.TaskOrder == taskInfoItem3.TaskOrder) ? taskInfoItem2.TemplateId.CompareTo(taskInfoItem3.TemplateId) : taskInfoItem2.TaskOrder.CompareTo(taskInfoItem3.TaskOrder);
		});
	}

	public sbyte GetXiangshuLevel()
	{
		return SharedMethods.GetXiangshuLevel(GetXiangshuProgress());
	}

	public sbyte GetMaxGradeOfXiangshuInfection()
	{
		return SharedMethods.GetMaxGradeOfXiangshuInfection(GetXiangshuProgress());
	}

	public void SetSwordTombStatus(DataContext context, sbyte xiangshuAvatarId, sbyte swordTombStatus)
	{
		if (DomainManager.Combat.GetIsPuppetCombat())
		{
			return;
		}
		XiangshuAvatarTaskStatus value = _xiangshuAvatarTaskStatuses[xiangshuAvatarId];
		if (swordTombStatus <= value.SwordTombStatus)
		{
			return;
		}
		int delta = swordTombStatus - value.SwordTombStatus;
		value.SwordTombStatus = swordTombStatus;
		SetElement_XiangshuAvatarTaskStatuses(xiangshuAvatarId, value, context);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		taiwu.ChangeConsummateLevel(context, delta);
		DomainManager.Taiwu.UpdateConsummateLevelBrokenFeature(context);
		if (swordTombStatus == 2)
		{
			sbyte worldCreationGroupLevel = TaiwuDomain.GetWorldCreationGroupLevel(0);
			short num = SwordTomb.Instance[xiangshuAvatarId].Legacies[worldCreationGroupLevel];
			if (DomainManager.Taiwu.AddAvailableLegacy(context, num))
			{
				DomainManager.Combat.AddCombatResultLegacy(num);
			}
			DomainManager.Extra.BigEventGainSwordTombRemoved(context, xiangshuAvatarId);
		}
	}

	public void ChangeMainStoryLineProgress(DataContext context, short progress)
	{
		if (!MainStoryLineProgress.CheckTransition(_mainStoryLineProgress, progress))
		{
			throw new Exception($"Invalid transition: {_mainStoryLineProgress} -> {progress}");
		}
		Logger.Info($"Main storyline progress is being changed to {progress}.");
		if (progress == 27)
		{
			DomainManager.Organization.TryRemoveTaiwuGroupBountyAndPunishment(context);
		}
		if (progress == 8)
		{
			DomainManager.Building.SetAllResidenceAutoCheckIn(context);
		}
		if (progress == 3)
		{
			GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = EventHelper.GetOrCreateFixedCharacterByTemplateId(467);
			orCreateFixedCharacterByTemplateId.AddDarkAsh(context, null, 0);
			orCreateFixedCharacterByTemplateId.DirectlyChangeDarkAshDuration(context, DomainManager.World.GetCurrDate(), 0, 0, 6);
		}
		if (progress == 6)
		{
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(138);
			for (int i = 0; i < areaBlocks.Length; i++)
			{
				MapBlockData mapBlockData = areaBlocks[i];
				if (mapBlockData.TemplateId != 36)
				{
					continue;
				}
				Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(new Location(138, mapBlockData.BlockId));
				int num = context.Random.Next(GlobalConfig.Instance.BrokenPerformDarkAshInfectorRangeMax) + GlobalConfig.Instance.BrokenPerformDarkAshInfectorBase;
				foreach (int member in settlementByLocation.GetMembers())
				{
					if (DomainManager.Character.TryGetElement_Objects(member, out var element) && element.GetDarkAshProtector() == 0 && element.GetActualAge() >= 16 && num-- > 0)
					{
						element.AddDarkAsh(context);
					}
				}
				break;
			}
		}
		if (progress == 16 && !DomainManager.Extra.GetIsDreamBack())
		{
			DomainManager.Extra.AddFuyuFaith(context, GlobalConfig.FuyuFaithCountBySaveInfected[^1]);
			DomainManager.World.GetInstantNotificationCollection().AddGainFuyuFaith3(GlobalConfig.FuyuFaithCountBySaveInfected[^1]);
		}
		SetMainStoryLineProgress(progress, context);
	}

	public bool CheckCurrMainStoryLineProgressInRange(short min, short max)
	{
		return _mainStoryLineProgress >= min && _mainStoryLineProgress < max;
	}

	public bool GetWorldFunctionsStatus(byte worldFunctionType)
	{
		return WorldFunctionType.Get(_worldFunctionsStatuses, worldFunctionType);
	}

	public void SetWorldFunctionsStatus(DataContext context, byte worldFunctionType)
	{
		ulong num = WorldFunctionType.Set(_worldFunctionsStatuses, worldFunctionType);
		if (num != _worldFunctionsStatuses)
		{
			SetWorldFunctionsStatuses(num, context);
			Logger.Info($"Unlocking world function: {worldFunctionType}");
		}
	}

	public void ResetWorldFunctionsStatus(DataContext context, byte worldFunctionType)
	{
		ulong num = WorldFunctionType.Reset(_worldFunctionsStatuses, worldFunctionType);
		if (num != _worldFunctionsStatuses)
		{
			SetWorldFunctionsStatuses(num, context);
			Logger.Info($"Reseting world function status: {worldFunctionType}");
		}
	}

	[DomainMethod]
	public void CreateWorld(DataContext context, WorldCreationInfo info)
	{
		Stopwatch sw = GlobalDomain.StartTimer();
		SetWorldCreationInfo(context, info, inherit: false);
		context.SwitchRandomSource((ulong)_worldId);
		DomainManager.Extra.InitializeWorldVersionInfo(context);
		DomainManager.Map.CreateAllAreas(context);
		DomainManager.Character.CreatePregeneratedCityTownGuards(context);
		DomainManager.Character.CreatePregeneratedRandomEnemies(context);
		DomainManager.Extra.CreatePregeneratedFixedEnemies(context);
		Stopwatch sw2 = GlobalDomain.StartTimer();
		DomainManager.Extra.CreatePickups(context);
		GlobalDomain.StopTimer(sw2, "CreatePickups");
		GlobalDomain.StopTimer(sw, "CreateWorld");
	}

	[DomainMethod]
	public void SetWorldCreationInfo(DataContext context, WorldCreationInfo info, bool inherit)
	{
		SetWorldPopulationType(info.WorldPopulationType, context);
		SetCharacterLifespanType(info.CharacterLifespanType, context);
		SetCombatDifficulty(info.CombatDifficulty, context);
		SetHereticsAmountType(info.HereticsAmountType, context);
		byte bossInvasionSpeedType = GetBossInvasionSpeedType();
		SetBossInvasionSpeedType(info.BossInvasionSpeedType, context);
		SetWorldResourceAmountType(info.WorldResourceAmountType, context);
		SetAllowRandomTaiwuHeir(info.AllowRandomTaiwuHeir, context);
		SetRestrictOptionsBehaviorType(info.RestrictOptionsBehaviorType, context);
		SetTaiwuVillageStateTemplateId(info.TaiwuVillageStateTemplateId, context);
		SetTaiwuVillageLandFormType(info.TaiwuVillageLandFormType, context);
		DomainManager.Extra.SetReadingDifficulty(info.ReadingDifficulty, context);
		DomainManager.Extra.SetBreakoutDifficulty(info.BreakoutDifficulty, context);
		DomainManager.Extra.SetLoopingDifficulty(info.LoopingDifficulty, context);
		DomainManager.Extra.SetEnemyPracticeLevel(info.EnemyPracticeLevel, context);
		DomainManager.Extra.SetFavorabilityChange(info.FavorabilityChange, context);
		DomainManager.Extra.SetProfessionUpgrade(info.ProfessionUpgrade, context);
		DomainManager.Extra.SetLootYield(info.LootYield, context);
		if (!inherit)
		{
			DomainManager.Extra.SetCanResetWorldSettings(value: false, context);
		}
		if (GetMainStoryLineProgress() >= 16 && bossInvasionSpeedType != info.BossInvasionSpeedType)
		{
			Events.RaiseBossInvasionSpeedTypeChanged(context, bossInvasionSpeedType);
		}
	}

	[DomainMethod]
	public WorldCreationInfo GetWorldCreationInfo()
	{
		return new WorldCreationInfo
		{
			WorldPopulationType = DomainManager.World.GetWorldPopulationType(),
			CharacterLifespanType = DomainManager.World.GetCharacterLifespanType(),
			CombatDifficulty = DomainManager.World.GetCombatDifficulty(),
			ReadingDifficulty = DomainManager.Extra.GetReadingDifficulty(),
			BreakoutDifficulty = DomainManager.Extra.GetBreakoutDifficulty(),
			LoopingDifficulty = DomainManager.Extra.GetLoopingDifficulty(),
			EnemyPracticeLevel = DomainManager.Extra.GetEnemyPracticeLevel(),
			FavorabilityChange = DomainManager.Extra.GetFavorabilityChange(),
			ProfessionUpgrade = DomainManager.Extra.GetProfessionUpgrade(),
			LootYield = DomainManager.Extra.GetLootYield(),
			HereticsAmountType = DomainManager.World.GetHereticsAmountType(),
			BossInvasionSpeedType = DomainManager.World.GetBossInvasionSpeedType(),
			WorldResourceAmountType = DomainManager.World.GetWorldResourceAmountType(),
			AllowRandomTaiwuHeir = DomainManager.World.GetAllowRandomTaiwuHeir(),
			RestrictOptionsBehaviorType = DomainManager.World.GetRestrictOptionsBehaviorType(),
			TaiwuVillageStateTemplateId = DomainManager.World._taiwuVillageStateTemplateId,
			TaiwuVillageLandFormType = DomainManager.World._taiwuVillageLandFormType
		};
	}

	public static WorldInfo GetWorldInfo()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		(string surname, string givenName) realName = CharacterDomain.GetRealName(taiwu);
		string item = realName.surname;
		string item2 = realName.givenName;
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			location = taiwu.GetValidLocation();
		}
		var (mapStateName, mapAreaName) = DomainManager.Map.GetStateAndAreaNameByAreaId(location.AreaId);
		return new WorldInfo
		{
			CurrDate = DomainManager.World.GetCurrDate(),
			TaiwuGenerationsCount = DomainManager.Taiwu.GetTaiwuGenerationsCount(),
			SavingTimestamp = DateTime.UtcNow.Ticks,
			TaiwuSurname = item,
			TaiwuGivenName = item2,
			Gender = taiwu.GetGender(),
			AvatarRelatedData = taiwu.GenerateAvatarRelatedData(),
			AvatarExtraData = DomainManager.Extra.GetCharacterAvatarExtraData(DataContextManager.GetCurrentThreadDataContext(), taiwu.GetId()),
			MapStateName = mapStateName,
			MapAreaName = mapAreaName,
			CharacterLifespanType = DomainManager.World.GetCharacterLifespanType(),
			CombatDifficulty = DomainManager.World.GetCombatDifficulty(),
			BreakoutDifficulty = DomainManager.Extra.GetBreakoutDifficulty(),
			ReadingDifficulty = DomainManager.Extra.GetReadingDifficulty(),
			LoopingDifficulty = DomainManager.Extra.GetLoopingDifficulty(),
			EnemyPracticeLevel = DomainManager.Extra.GetEnemyPracticeLevel(),
			FavorabilityChange = DomainManager.Extra.GetFavorabilityChange(),
			ProfessionUpgrade = DomainManager.Extra.GetProfessionUpgrade(),
			LootYield = DomainManager.Extra.GetLootYield(),
			HereticsAmountType = DomainManager.World.GetHereticsAmountType(),
			BossInvasionSpeedType = DomainManager.World.GetBossInvasionSpeedType(),
			WorldResourceAmountType = DomainManager.World.GetWorldResourceAmountType(),
			WorldPopulationType = DomainManager.World.GetWorldPopulationType(),
			AllowRandomTaiwuHeir = DomainManager.World.GetAllowRandomTaiwuHeir(),
			RestrictOptionsBehaviorType = DomainManager.World.GetRestrictOptionsBehaviorType(),
			StateTaskStatuses = DomainManager.World._stateTaskStatuses.ToArray(),
			XiangshuAvatarTaskStatuses = DomainManager.World._xiangshuAvatarTaskStatuses.ToArray(),
			MainStoryLineProgress = DomainManager.World._mainStoryLineProgress,
			BeatRanChenZi = DomainManager.World._beatRanChenZi,
			ModIds = ModDomain.GetLoadedModIds(),
			DlcIds = DlcManager.GetAllInstalledDlcIds(),
			GameVersionInfo = DomainManager.Extra.GetWorldVersionInfo()
		};
	}

	public void UpdateCurrWorldGameVersion()
	{
		GameVersionInfo worldVersionInfo = DomainManager.Extra.GetWorldVersionInfo();
		_currWorldGameVersion = ((worldVersionInfo != null && !string.IsNullOrEmpty(worldVersionInfo.GameVersionLastSaving)) ? GameVersionInfo.ParseGameVersion(worldVersionInfo.GameVersionLastSaving) : new Version(0, 0, 61, 23));
	}

	public Version GetCurrWorldGameVersion()
	{
		return _currWorldGameVersion;
	}

	public bool IsCurrWorldBeforeVersion(int major, int minor = 0, int build = 0, int revision = 0)
	{
		if (_currWorldGameVersion == null)
		{
			return false;
		}
		if (_currWorldGameVersion.Major != major)
		{
			return _currWorldGameVersion.Major < major;
		}
		if (_currWorldGameVersion.Minor != minor)
		{
			return _currWorldGameVersion.Minor < minor;
		}
		if (_currWorldGameVersion.Build != build)
		{
			return _currWorldGameVersion.Build < build;
		}
		if (_currWorldGameVersion.Revision != revision)
		{
			return _currWorldGameVersion.Revision < revision;
		}
		return false;
	}

	public bool IsCurrWorldAfterVersion(int major, int minor = 0, int build = 0, int revision = 0)
	{
		if (_currWorldGameVersion == null)
		{
			return false;
		}
		if (_currWorldGameVersion.Major != major)
		{
			return _currWorldGameVersion.Major > major;
		}
		if (_currWorldGameVersion.Minor != minor)
		{
			return _currWorldGameVersion.Minor > minor;
		}
		if (_currWorldGameVersion.Build != build)
		{
			return _currWorldGameVersion.Build > build;
		}
		if (_currWorldGameVersion.Revision != revision)
		{
			return _currWorldGameVersion.Revision > revision;
		}
		return false;
	}

	public bool IsCurrWorldSavedWithVersion(int major, int minor, int build, int revision)
	{
		if (_currWorldGameVersion == null)
		{
			return false;
		}
		if (_currWorldGameVersion.Major != major)
		{
			return false;
		}
		if (_currWorldGameVersion.Minor != minor)
		{
			return false;
		}
		if (_currWorldGameVersion.Build != build)
		{
			return false;
		}
		if (_currWorldGameVersion.Revision != revision)
		{
			return false;
		}
		return true;
	}

	public int RegisterCustomText(DataContext context, string text)
	{
		if (text == null)
		{
			throw new Exception("Text can not be null");
		}
		int num = GenerateNextCustomTextId(context);
		AddElement_CustomTexts(num, text, context);
		return num;
	}

	public void UnregisterCustomText(DataContext context, int id)
	{
		RemoveElement_CustomTexts(id, context);
	}

	public IReadOnlyDictionary<int, string> GetCustomTexts()
	{
		return _customTexts;
	}

	[DomainMethod]
	public List<Location> GetJuniorXiangshuLocations()
	{
		List<Location> list = new List<Location>();
		XiangshuAvatarTaskStatus[] xiangshuAvatarTaskStatuses = _xiangshuAvatarTaskStatuses;
		for (int i = 0; i < xiangshuAvatarTaskStatuses.Length; i++)
		{
			XiangshuAvatarTaskStatus xiangshuAvatarTaskStatus = xiangshuAvatarTaskStatuses[i];
			if (xiangshuAvatarTaskStatus.JuniorXiangshuCharId < 0)
			{
				list.Add(Location.Invalid);
				continue;
			}
			Location location = DomainManager.Character.GetElement_Objects(xiangshuAvatarTaskStatus.JuniorXiangshuCharId).GetLocation();
			list.Add(location);
		}
		return list;
	}

	public void ChangeXiangshuAvatarFavorability(DataContext context, sbyte xiangshuAvatarId, int delta)
	{
		XiangshuAvatarTaskStatus xiangshuAvatarTaskStatus = _xiangshuAvatarTaskStatuses[xiangshuAvatarId];
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(xiangshuAvatarTaskStatus.JuniorXiangshuCharId);
		DomainManager.Character.DirectlyChangeFavorabilityOptional(context, element_Objects, taiwu, delta, 4);
	}

	public short GetXiangshuAvatarFavorability(sbyte xiangshuAvatarId)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		XiangshuAvatarTaskStatus xiangshuAvatarTaskStatus = _xiangshuAvatarTaskStatuses[xiangshuAvatarId];
		return DomainManager.Character.GetFavorability(xiangshuAvatarTaskStatus.JuniorXiangshuCharId, taiwuCharId);
	}

	public sbyte GetXiangshuAvatarFavorabilityType(sbyte xiangshuAvatarId)
	{
		return FavorabilityType.GetFavorabilityType(DomainManager.World.GetXiangshuAvatarFavorability(xiangshuAvatarId));
	}

	public void TransferXiangshuAvatarRelations(DataContext context, int oldTaiwuCharId, int newTaiwuCharId)
	{
		for (int i = 0; i < 9; i++)
		{
			XiangshuAvatarTaskStatus value = _xiangshuAvatarTaskStatuses[i];
			int juniorXiangshuCharId = value.JuniorXiangshuCharId;
			if (DomainManager.Character.TryGetElement_Objects(juniorXiangshuCharId, out var _))
			{
				RelatedCharacter relation = DomainManager.Character.GetRelation(oldTaiwuCharId, juniorXiangshuCharId);
				RelatedCharacter relation2 = DomainManager.Character.GetRelation(juniorXiangshuCharId, oldTaiwuCharId);
				DomainManager.Character.DirectlySetFavorabilities(context, newTaiwuCharId, juniorXiangshuCharId, relation.Favorability, relation2.Favorability);
				SetElement_XiangshuAvatarTaskStatuses(i, value, context);
			}
		}
	}

	public string GetWorldDateKey()
	{
		int value = GetCurrYear() + 1;
		int value2 = GetCurrMonthInYear() + 1;
		return $"{_worldId}_{value}_{value2}";
	}

	private int GenerateNextCustomTextId(DataContext context)
	{
		int nextCustomTextId = _nextCustomTextId;
		_nextCustomTextId++;
		if ((uint)_nextCustomTextId > 2147483647u)
		{
			_nextCustomTextId = 0;
		}
		SetNextCustomTextId(_nextCustomTextId, context);
		return nextCustomTextId;
	}

	public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
	{
		crossArchiveGameData.CustomTexts = _customTexts;
		crossArchiveGameData.NextCustomTextId = _nextCustomTextId;
		crossArchiveGameData.FinalDateBeforeDreamBack = _currDate;
		crossArchiveGameData.WorldCreationInfo = GetWorldCreationInfo();
		crossArchiveGameData.WorldId = _worldId;
	}

	public override void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		foreach (KeyValuePair<int, string> customText in crossArchiveGameData.CustomTexts)
		{
			if (_customTexts.ContainsKey(customText.Key))
			{
				SetElement_CustomTexts(customText.Key, customText.Value, context);
			}
			else
			{
				AddElement_CustomTexts(customText.Key, customText.Value, context);
			}
		}
		SetNextCustomTextId(Math.Max(crossArchiveGameData.NextCustomTextId, _nextCustomTextId), context);
		SetWorldCreationInfo(context, crossArchiveGameData.WorldCreationInfo, inherit: false);
		if (crossArchiveGameData.WorldId != _worldId || !DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().GetBool("IsDreamBackArchive"))
		{
			PredefinedLog.Show(16);
		}
	}

	[DomainMethod]
	public bool GmCmd_SectEmeiAddSkillBreakBonus(DataContext context, short combatSkillId, short bonusTypeTemplateId)
	{
		return DomainManager.Extra.AddEmeiSkillBreakBonusWithoutCost(context, combatSkillId, bonusTypeTemplateId);
	}

	[DomainMethod]
	public bool GmCmd_SectEmeiClearSkillBreakBonus(DataContext context, short combatSkillId)
	{
		return DomainManager.Extra.ClearEmeiSkillBreakBonus(context, combatSkillId);
	}

	public InstantNotificationCollection GetInstantNotificationCollection()
	{
		return _instantNotifications;
	}

	public void CommitInstantNotifications(DataContext context)
	{
		int num = _instantNotifications.Size - _instantNotificationsCommittedOffset;
		if (num > 0)
		{
			CommitInsert_InstantNotifications(context, _instantNotificationsCommittedOffset, num);
			CommitSetMetadata_InstantNotifications(context);
			_instantNotificationsCommittedOffset = _instantNotifications.Size;
		}
	}

	private void RemoveObsoletedInstantNotifications(DataContext context)
	{
		int num = RemoveObsoletedInstantNotificationsInternal(context);
		_instantNotificationsCommittedOffset -= num;
	}

	private unsafe int RemoveObsoletedInstantNotificationsInternal(DataContext context)
	{
		int num = GetCurrDate() - 12;
		int index = -1;
		int offset = -1;
		bool flag = false;
		fixed (byte* rawData = _instantNotifications.RawData)
		{
			while (_instantNotifications.Next(ref index, ref offset))
			{
				byte* ptr = rawData + offset;
				int num2 = *(int*)(ptr + 1);
				if (num2 >= num)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			if (index <= 0)
			{
				return 0;
			}
			_instantNotifications.Remove(0, offset);
			_instantNotifications.Count -= index;
			CommitRemove_InstantNotifications(context, 0, offset);
			CommitSetMetadata_InstantNotifications(context);
			return offset;
		}
		if (_instantNotifications.Count <= 0)
		{
			return 0;
		}
		int size = _instantNotifications.Size;
		_instantNotifications.Remove(0, size);
		_instantNotifications.Count = 0;
		CommitRemove_InstantNotifications(context, 0, size);
		CommitSetMetadata_InstantNotifications(context);
		return size;
	}

	public void PrepareTestInstantNotificationRelatedData()
	{
		if (_instantNotificationTemplateIds.Count <= 0)
		{
			InitializeTestInstantNotificationRelatedData();
		}
	}

	public void AddRandomInstantNotification(DataContext context, InstantNotificationCollection notifications)
	{
		int index = context.Random.Next(_instantNotificationTemplateIds.Count);
		short index2 = _instantNotificationTemplateIds[index];
		InstantNotificationItem instantNotificationItem = InstantNotification.Instance[index2];
		string text = _instantNotificationTemplateId2Name[instantNotificationItem.TemplateId];
		MethodInfo method = _instantNotificationCollectionType.GetMethod("Add" + text);
		Tester.Assert(method != null);
		List<object> list = new List<object>();
		GameData.Domains.Character.Character character = null;
		int i = 0;
		for (int num = instantNotificationItem.Parameters.Length; i < num; i++)
		{
			string text2 = instantNotificationItem.Parameters[i];
			if (string.IsNullOrEmpty(text2))
			{
				break;
			}
			sbyte paramType = ParameterType.Parse(text2);
			AddNotificationArguments(context.Random, list, paramType, ref character);
		}
		method.Invoke(notifications, list.ToArray());
	}

	private void InitializeTestInstantNotificationRelatedData()
	{
		_instantNotificationTemplateIds.Clear();
		_instantNotificationTemplateId2Name.Clear();
		Type type = Type.GetType("Config.InstantNotification+DefKey");
		Tester.Assert(type != null);
		FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			string name = fieldInfo.Name;
			short num = (short)fieldInfo.GetValue(null);
			_instantNotificationTemplateIds.Add(num);
			_instantNotificationTemplateId2Name.Add(num, name);
		}
		_instantNotificationCollectionType = Type.GetType("GameData.Domains.World.Notification.InstantNotificationCollection");
	}

	public void CheckMonthlyEvents(DataContext context)
	{
		AddTestMonthlyEvent(context);
		if (_isTaiwuVillageDestroyed)
		{
			_monthlyEventCollection.Clear();
			_monthlyEventCollection.AddTaiwuVillageBeDestoryed();
			ResetFlag();
			Logger.Info("CheckMonthlyEvents: TaiwuVillageBeDestroyed.");
			return;
		}
		if (_mainStoryLineProgress == 6 && EventHelper.GlobalArgBoxContainsKey<Location>("WangliuLocation"))
		{
			Location objectFromGlobalArgBox = EventHelper.GetObjectFromGlobalArgBox<Location>("WangliuLocation");
			List<Location> mapBlocksNearLocation = EventHelper.GetMapBlocksNearLocation(objectFromGlobalArgBox, 3);
			List<Location> innerLocations = EventHelper.GetMapBlocksNearLocation(objectFromGlobalArgBox, 2);
			mapBlocksNearLocation.RemoveAll((Location e) => innerLocations.Contains(e));
			bool flag = true;
			foreach (Location item in mapBlocksNearLocation)
			{
				MapBlockData block = DomainManager.Map.GetBlock(item);
				if (block.BlockSubType != EMapBlockSubType.Ruin)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				_monthlyEventCollection.Clear();
				_monthlyEventCollection.AddAreaTotallyDestoryed(DomainManager.Taiwu.GetTaiwuCharId());
				Logger.Info("CheckMonthlyEvents: AreaTotallyDestoryed.");
				return;
			}
		}
		if (_isTaiwuDying || _isTaiwuGettingCompletelyInfected)
		{
			_monthlyEventCollection.Clear();
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			if (_isTaiwuDying)
			{
				_monthlyEventCollection.AddTaiwuDeath(taiwu.GetId(), taiwu.GetLocation());
			}
			else
			{
				_monthlyEventCollection.AddTaiwuInfected(taiwu.GetId(), taiwu.GetLocation());
			}
			ResetFlag();
			Events.RaisePassingLegacyWhileAdvancingMonth(context);
			Logger.Info("CheckMonthlyEvents: LegacyPassing.");
			return;
		}
		KidnappedTravelData kidnappedTravelData = DomainManager.Extra.GetKidnappedTravelData();
		if (kidnappedTravelData.Valid)
		{
			_monthlyEventCollection.Clear();
			if (IsTaiwuHunterDie)
			{
				_monthlyEventCollection.AddTaiwuBeHuntedHunterDie(kidnappedTravelData.HunterCharId, DomainManager.Taiwu.GetTaiwuCharId());
				ResetFlag();
			}
			return;
		}
		DomainManager.Adventure.CheckRandomEnemyAttackTaiwuOnAdvanceMonth();
		DomainManager.Extra.CheckAnimalAttackTaiwuOnAdvanceMonth(context.Random);
		UpdateMonthlyEventToRepayKindness(context);
		CheckWorldStateMonthlyEvents();
		CheckTaskMonthlyEvents();
		RemoveAllInvalidMonthlyEvents(context);
		TrimSpecialMonthlyEvents(context);
		Logger.Info($"{"CheckMonthlyEvents"}: {_monthlyEventCollection.Count} events detected.");
		void ResetFlag()
		{
			_isTaiwuDying = false;
			_isTaiwuGettingCompletelyInfected = false;
			_isTaiwuDyingOfDystocia = false;
			_isTaiwuVillageDestroyed = false;
			IsTaiwuHunterDie = false;
		}
	}

	[DomainMethod]
	public void HandleMonthlyEvent(DataContext context, int offset)
	{
		short recordType = _monthlyEventCollection.GetRecordType(offset);
		MonthlyEventItem monthlyEventItem = Config.MonthlyEvent.Instance[recordType];
		int recordSize = _monthlyEventCollection.GetRecordSize(offset);
		switch (recordType)
		{
		case 1:
			EventHelper.TriggerLegacyPassingEvent(isTaiwuDying: true, string.Empty);
			DomainManager.TaiwuEvent.SetEventInProcessing(monthlyEventItem.Event);
			return;
		case 3:
			EventHelper.TriggerLegacyPassingEvent(isTaiwuDying: false, string.Empty);
			DomainManager.TaiwuEvent.SetEventInProcessing(monthlyEventItem.Event);
			return;
		}
		if (!string.IsNullOrEmpty(monthlyEventItem.Event))
		{
			GameData.Domains.TaiwuEvent.TaiwuEvent taiwuEvent = DomainManager.TaiwuEvent.GetEvent(monthlyEventItem.Event);
			if (taiwuEvent != null)
			{
				if (taiwuEvent.ArgBox == null)
				{
					taiwuEvent.ArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
				}
				_monthlyEventCollection.FillEventArgBox(offset, taiwuEvent.ArgBox);
				if (!taiwuEvent.EventConfig.CheckCondition())
				{
					throw new Exception($"monthly event {monthlyEventItem.Name} is triggering {taiwuEvent.EventGuid} when OnCheckEventCondition return false.");
				}
				DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
				DomainManager.TaiwuEvent.SetEventInProcessing(monthlyEventItem.Event);
			}
			else
			{
				Logger.Warn($"Monthly Event {monthlyEventItem.Name} ({monthlyEventItem.Event}) not found.");
			}
		}
		Logger.Info($"Removing monthly event {recordType} at {offset} of size {recordSize} from a collection of size {_monthlyEventCollection.Size} and count {_monthlyEventCollection.Count}");
		if (_monthlyEventCollection.Size == 0)
		{
			Logger.AppendWarning("MonthlyEventCollection is empty.");
			return;
		}
		_monthlyEventCollection.Remove(offset, recordSize);
		_monthlyEventCollection.Count--;
	}

	[DomainMethod]
	public MonthlyEventCollection GetMonthlyEventCollection()
	{
		return _monthlyEventCollection;
	}

	[DomainMethod]
	public void RemoveAllInvalidMonthlyEvents(DataContext context)
	{
		int index = -1;
		int offset = -1;
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		EventArgBox eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
		while (_monthlyEventCollection.Next(ref index, ref offset))
		{
			short recordType = _monthlyEventCollection.GetRecordType(offset);
			MonthlyEventItem monthlyEventItem = Config.MonthlyEvent.Instance[recordType];
			if (string.IsNullOrEmpty(monthlyEventItem.Event))
			{
				if (monthlyEventItem.TemplateId == 0)
				{
					DomainManager.Taiwu.CheckNotInInventoryBooks(context);
					if (!DomainManager.Taiwu.GetCurReadingBook().IsValid())
					{
						list.Add(offset);
					}
				}
				continue;
			}
			GameData.Domains.TaiwuEvent.TaiwuEvent taiwuEvent = DomainManager.TaiwuEvent.GetEvent(monthlyEventItem.Event);
			if (taiwuEvent != null)
			{
				_monthlyEventCollection.FillEventArgBox(offset, eventArgBox);
				taiwuEvent.ArgBox = eventArgBox;
				if (!taiwuEvent.EventConfig.CheckCondition())
				{
					list.Add(offset);
				}
				taiwuEvent.ArgBox = null;
			}
		}
		for (int num = list.Count - 1; num >= 0; num--)
		{
			int num2 = list[num];
			int recordSize = _monthlyEventCollection.GetRecordSize(num2);
			Logger.Info($"Removing monthly event {recordSize} at {num2} of size {recordSize} from a collection of size {_monthlyEventCollection.Size} and count {_monthlyEventCollection.Count}");
			_monthlyEventCollection.Remove(num2, recordSize);
			_monthlyEventCollection.Count--;
		}
		DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		ObjectPool<List<int>>.Instance.Return(list);
	}

	private void TrimSpecialMonthlyEvents(DataContext context)
	{
		int num = 15;
		int index = -1;
		int offset = -1;
		SpecialEvents.Clear();
		while (_monthlyEventCollection.Next(ref index, ref offset))
		{
			short recordType = _monthlyEventCollection.GetRecordType(offset);
			MonthlyEventItem monthlyEventItem = Config.MonthlyEvent.Instance[recordType];
			if (monthlyEventItem.Type == EMonthlyEventType.SpecialEvent)
			{
				SpecialEvents.Push((offset, monthlyEventItem.Score));
			}
		}
		List<int> obj = context.AdvanceMonthRelatedData.IntList.Occupy();
		while (SpecialEvents.Count > 0)
		{
			(int, int) tuple = SpecialEvents.Pop();
			if (num < tuple.Item2)
			{
				obj.Add(tuple.Item1);
			}
			else
			{
				num -= tuple.Item2;
			}
		}
		if (obj.Count == 0)
		{
			context.AdvanceMonthRelatedData.IntList.Release(ref obj);
			return;
		}
		obj.Sort();
		for (int num2 = obj.Count - 1; num2 >= 0; num2--)
		{
			int num3 = obj[num2];
			int recordSize = _monthlyEventCollection.GetRecordSize(num3);
			Logger.Info($"{"TrimSpecialMonthlyEvents"}: Removing monthly event {recordSize} at {num3} of size {recordSize} from a collection of size {_monthlyEventCollection.Size} and count {_monthlyEventCollection.Count}");
			_monthlyEventCollection.Remove(num3, recordSize);
			_monthlyEventCollection.Count--;
		}
		context.AdvanceMonthRelatedData.IntList.Release(ref obj);
	}

	public void ClearTrivialMonthlyEvents(DataContext context)
	{
		int index = -1;
		int offset = -1;
		EventArgBox eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
		sbyte behaviorType = DomainManager.Taiwu.GetTaiwu().GetBehaviorType();
		while (_monthlyEventCollection.Next(ref index, ref offset))
		{
			short recordType = _monthlyEventCollection.GetRecordType(offset);
			MonthlyEventItem monthlyEventItem = Config.MonthlyEvent.Instance[recordType];
			if (monthlyEventItem.Type != EMonthlyEventType.SpecialEvent)
			{
				if (monthlyEventItem.Type == EMonthlyEventType.LockedEvent)
				{
					Logger.AppendWarning("Monthly Event " + monthlyEventItem.Name + " is a locked event which cannot be cleared or handled with default option.");
				}
				else if (!string.IsNullOrEmpty(monthlyEventItem.Event))
				{
					eventArgBox.Clear();
					eventArgBox.Set("DefaultHandleFlag", arg: true);
					_monthlyEventCollection.FillEventArgBox(offset, eventArgBox);
					DomainManager.TaiwuEvent.ProcessEventWithDefaultOption(monthlyEventItem.Event, eventArgBox, behaviorType);
				}
			}
		}
		DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		_monthlyEventCollection.Clear();
		SetOnHandingMonthlyEventBlock(value: false, context);
	}

	[DomainMethod]
	public void ProcessAllMonthlyEventsWithDefaultOption(DataContext context)
	{
		int index = -1;
		int offset = -1;
		EventArgBox eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
		sbyte behaviorType = DomainManager.Taiwu.GetTaiwu().GetBehaviorType();
		while (_monthlyEventCollection.Next(ref index, ref offset))
		{
			short recordType = _monthlyEventCollection.GetRecordType(offset);
			MonthlyEventItem monthlyEventItem = Config.MonthlyEvent.Instance[recordType];
			if (monthlyEventItem.Type == EMonthlyEventType.SpecialEvent || monthlyEventItem.Type == EMonthlyEventType.LockedEvent)
			{
				Logger.AppendWarning(monthlyEventItem.Name + " is a special event that cannot be handled with default option.");
			}
			else if (!string.IsNullOrEmpty(monthlyEventItem.Event))
			{
				eventArgBox.Clear();
				eventArgBox.Set("DefaultHandleFlag", arg: true);
				_monthlyEventCollection.FillEventArgBox(offset, eventArgBox);
				DomainManager.TaiwuEvent.ProcessEventWithDefaultOption(monthlyEventItem.Event, eventArgBox, behaviorType);
			}
		}
		DomainManager.TaiwuEvent.ReturnArgBox(eventArgBox);
		_monthlyEventCollection.Clear();
		SetOnHandingMonthlyEventBlock(value: false, context);
	}

	public void EscapeDuringMonthlyEvent(DataContext context)
	{
		if (!DomainManager.Taiwu.GetNeedToEscape())
		{
			Tester.Assert(!_isTaiwuDying && !_isTaiwuGettingCompletelyInfected);
			Tester.Assert(_advancingMonthState == 20);
			DomainManager.Taiwu.SetNeedToEscape(value: true, context);
			ClearTrivialMonthlyEvents(context);
			_monthlyEventCollection.Clear();
		}
	}

	public void SetTaiwuDying(bool isTaiwuDyingOfDystocia = false)
	{
		if (!_isTaiwuDying)
		{
			_isTaiwuDying = true;
			_isTaiwuDyingOfDystocia = isTaiwuDyingOfDystocia;
		}
	}

	public void SetTaiwuGettingCompletelyInfected()
	{
		_isTaiwuGettingCompletelyInfected = true;
	}

	public void SetTaiwuVillageDestroyed()
	{
		_isTaiwuVillageDestroyed = true;
	}

	public void SetToRepayKindnessCharId(int charId)
	{
		_toRepayKindnessCharId = charId;
	}

	public void UpdateMonthlyEventToRepayKindness(DataContext context)
	{
		if (GameData.Domains.Character.Character.IsCharacterIdValid(_toRepayKindnessCharId) && DomainManager.Character.TryGetElement_Objects(_toRepayKindnessCharId, out var _))
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddToRepayKindness(DomainManager.Taiwu.GetTaiwu().GetValidLocation());
		}
	}

	public int GetToRepayKindnessCharId()
	{
		return _toRepayKindnessCharId;
	}

	public bool ClearMonthlyEventCollectionNotEndGame()
	{
		return _isTaiwuDying || _isTaiwuGettingCompletelyInfected || DomainManager.Extra.GetKidnappedTravelData().Valid;
	}

	private void CheckTaskMonthlyEvents()
	{
		MonthlyEventCollection monthlyEventCollection = GetMonthlyEventCollection();
		List<TaskData> currTaskList = GetCurrTaskList();
		foreach (TaskData item in currTaskList)
		{
			TaskInfoItem taskInfoItem = TaskInfo.Instance[item.TaskInfoId];
			AutoTriggerMonthlyEvent[] monthlyEvents = taskInfoItem.MonthlyEvents;
			if (monthlyEvents != null && monthlyEvents.Length > 0)
			{
				sbyte sect = TaskChain.Instance[item.TaskChainId].Sect;
				EventArgBox argBox = ((sect >= 0 && OrganizationDomain.IsSect(sect)) ? DomainManager.Extra.GetSectMainStoryEventArgBox(sect) : DomainManager.TaiwuEvent.GetGlobalEventArgumentBox());
				AutoTriggerMonthlyEvent[] monthlyEvents2 = taskInfoItem.MonthlyEvents;
				foreach (AutoTriggerMonthlyEvent monthlyEvent in monthlyEvents2)
				{
					monthlyEventCollection.AddTaskMonthlyEvent(argBox, taskInfoItem, monthlyEvent);
				}
			}
		}
	}

	private void CheckWorldStateMonthlyEvents()
	{
		MonthlyEventCollection monthlyEventCollection = GetMonthlyEventCollection();
		WorldStateData worldStateData = GetWorldStateData();
		foreach (WorldStateItem item in (IEnumerable<WorldStateItem>)WorldState.Instance)
		{
			short[] monthlyEvents = item.MonthlyEvents;
			if (monthlyEvents != null && monthlyEvents.Length > 0 && worldStateData.GetWorldState(item.TemplateId))
			{
				short[] monthlyEvents2 = item.MonthlyEvents;
				foreach (short index in monthlyEvents2)
				{
					monthlyEventCollection.AddWorldStateMonthlyEvent(item, Config.MonthlyEvent.Instance[index]);
				}
			}
		}
	}

	public MonthlyNotificationCollection GetMonthlyNotificationCollection()
	{
		return _currMonthlyNotifications;
	}

	private void CheckMonthlyNotifications()
	{
		MonthlyNotificationCollection monthlyNotificationCollection = GetMonthlyNotificationCollection();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (_mainStoryLineProgress != 12)
		{
			return;
		}
		int intFromGlobalArgBox = EventHelper.GetIntFromGlobalArgBox("Yiren");
		if (intFromGlobalArgBox >= 0 && DomainManager.Character.TryGetElement_Objects(intFromGlobalArgBox, out var element))
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			if (!taiwu.GetLocation().Equals(taiwuVillageLocation) && element.GetLocation().IsValid())
			{
				monthlyNotificationCollection.AddYirenAppearInTaiwuArea(taiwu.GetId());
			}
		}
	}

	private void TransferMonthlyNotifications(DataContext context)
	{
		DomainManager.Extra.UpdatePreviousMonthlyNotifications(context, ref _currMonthlyNotifications);
	}

	public void InitializeSortedMonthlyNotificationSortingGroups(Dictionary<int, NotificationSortingGroup> groups)
	{
		_sortedMonthlyNotificationSortingGroups = new List<int>();
		foreach (int key in groups.Keys)
		{
			_sortedMonthlyNotificationSortingGroups.Add(key);
		}
		_sortedMonthlyNotificationSortingGroups.Sort(CompareGroups);
	}

	public void SortMonthlyNotificationSortingGroups(DataContext context)
	{
		_sortedMonthlyNotificationSortingGroups.Sort(CompareGroups);
		SetSortedMonthlyNotificationSortingGroups(_sortedMonthlyNotificationSortingGroups, context);
	}

	private int CompareGroups(int groupId1, int groupId2)
	{
		NotificationSortingGroup element_MonthlyNotificationSortingGroups = DomainManager.Extra.GetElement_MonthlyNotificationSortingGroups(groupId1);
		NotificationSortingGroup element_MonthlyNotificationSortingGroups2 = DomainManager.Extra.GetElement_MonthlyNotificationSortingGroups(groupId2);
		if (element_MonthlyNotificationSortingGroups.IsOnTop && !element_MonthlyNotificationSortingGroups2.IsOnTop)
		{
			return -1;
		}
		if (!element_MonthlyNotificationSortingGroups.IsOnTop && element_MonthlyNotificationSortingGroups2.IsOnTop)
		{
			return 1;
		}
		return (element_MonthlyNotificationSortingGroups.Priority == element_MonthlyNotificationSortingGroups2.Priority) ? element_MonthlyNotificationSortingGroups.Id.CompareTo(element_MonthlyNotificationSortingGroups2.Id) : element_MonthlyNotificationSortingGroups.Priority.CompareTo(element_MonthlyNotificationSortingGroups2.Priority);
	}

	public void PrepareTestMonthlyNotificationRelatedData()
	{
		if (_monthlyNotificationTemplateIds.Count <= 0)
		{
			InitializeTestMonthlyNotificationRelatedData();
		}
		_candidatesCharacters.Clear();
		DomainManager.Character.FindIntelligentCharacters((GameData.Domains.Character.Character _) => true, _candidatesCharacters);
	}

	public void AddRandomMonthlyNotification(DataContext context, MonthlyNotificationCollection notifications)
	{
		int index = context.Random.Next(_monthlyNotificationTemplateIds.Count);
		short index2 = _monthlyNotificationTemplateIds[index];
		MonthlyNotificationItem monthlyNotificationItem = MonthlyNotification.Instance[index2];
		string text = _monthlyNotificationTemplateId2Name[monthlyNotificationItem.TemplateId];
		MethodInfo method = _monthlyNotificationCollectionType.GetMethod("Add" + text);
		Tester.Assert(method != null);
		List<object> list = new List<object>();
		GameData.Domains.Character.Character character = null;
		int i = 0;
		for (int num = monthlyNotificationItem.Parameters.Length; i < num; i++)
		{
			string text2 = monthlyNotificationItem.Parameters[i];
			if (string.IsNullOrEmpty(text2))
			{
				break;
			}
			sbyte paramType = ParameterType.Parse(text2);
			AddNotificationArguments(context.Random, list, paramType, ref character);
		}
		method.Invoke(notifications, list.ToArray());
	}

	private void InitializeTestMonthlyNotificationRelatedData()
	{
		_monthlyNotificationTemplateIds.Clear();
		_monthlyNotificationTemplateId2Name.Clear();
		Type type = Type.GetType("Config.MonthlyNotification+DefKey");
		Tester.Assert(type != null);
		FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			string name = fieldInfo.Name;
			short num = (short)fieldInfo.GetValue(null);
			_monthlyNotificationTemplateIds.Add(num);
			_monthlyNotificationTemplateId2Name.Add(num, name);
		}
		_monthlyNotificationCollectionType = Type.GetType("GameData.Domains.World.Notification.MonthlyNotificationCollection");
		_candidateItems.Clear();
		InitializeCandidateItems(_candidateItems);
		_candidateCombatSkills.Clear();
		foreach (CombatSkillItem item in (IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance)
		{
			_candidateCombatSkills.Add(item.TemplateId);
		}
		_candidateSettlements.Clear();
		InitializeCandidateSettlements(_candidateSettlements);
		_candidateBuildings.Clear();
		foreach (BuildingBlockItem item2 in (IEnumerable<BuildingBlockItem>)BuildingBlock.Instance)
		{
			_candidateBuildings.Add(item2.TemplateId);
		}
		_candidateAdventures.Clear();
		foreach (AdventureItem item3 in (IEnumerable<AdventureItem>)Config.Adventure.Instance)
		{
			_candidateAdventures.Add(item3.TemplateId);
		}
		_candidateCrickets.Clear();
		InitializeCandidateCrickets(_candidateCrickets);
		_candidateChickens.Clear();
		foreach (ChickenItem item4 in (IEnumerable<ChickenItem>)Chicken.Instance)
		{
			_candidateChickens.Add(item4.TemplateId);
		}
	}

	private static void InitializeCandidateItems(List<TemplateKey> items)
	{
		foreach (AccessoryItem item in (IEnumerable<AccessoryItem>)Config.Accessory.Instance)
		{
			items.Add(new TemplateKey(item.ItemType, item.TemplateId));
		}
		foreach (ArmorItem item2 in (IEnumerable<ArmorItem>)Config.Armor.Instance)
		{
			items.Add(new TemplateKey(item2.ItemType, item2.TemplateId));
		}
		foreach (CarrierItem item3 in (IEnumerable<CarrierItem>)Config.Carrier.Instance)
		{
			items.Add(new TemplateKey(item3.ItemType, item3.TemplateId));
		}
		foreach (ClothingItem item4 in (IEnumerable<ClothingItem>)Config.Clothing.Instance)
		{
			items.Add(new TemplateKey(item4.ItemType, item4.TemplateId));
		}
		foreach (CraftToolItem item5 in (IEnumerable<CraftToolItem>)Config.CraftTool.Instance)
		{
			items.Add(new TemplateKey(item5.ItemType, item5.TemplateId));
		}
		foreach (CricketItem item6 in (IEnumerable<CricketItem>)Config.Cricket.Instance)
		{
			items.Add(new TemplateKey(item6.ItemType, item6.TemplateId));
		}
		foreach (FoodItem item7 in (IEnumerable<FoodItem>)Config.Food.Instance)
		{
			items.Add(new TemplateKey(item7.ItemType, item7.TemplateId));
		}
		foreach (MaterialItem item8 in (IEnumerable<MaterialItem>)Config.Material.Instance)
		{
			items.Add(new TemplateKey(item8.ItemType, item8.TemplateId));
		}
		foreach (MedicineItem item9 in (IEnumerable<MedicineItem>)Config.Medicine.Instance)
		{
			items.Add(new TemplateKey(item9.ItemType, item9.TemplateId));
		}
		foreach (MiscItem item10 in (IEnumerable<MiscItem>)Config.Misc.Instance)
		{
			items.Add(new TemplateKey(item10.ItemType, item10.TemplateId));
		}
		foreach (SkillBookItem item11 in (IEnumerable<SkillBookItem>)Config.SkillBook.Instance)
		{
			items.Add(new TemplateKey(item11.ItemType, item11.TemplateId));
		}
		foreach (TeaWineItem item12 in (IEnumerable<TeaWineItem>)Config.TeaWine.Instance)
		{
			items.Add(new TemplateKey(item12.ItemType, item12.TemplateId));
		}
		foreach (WeaponItem item13 in (IEnumerable<WeaponItem>)Config.Weapon.Instance)
		{
			items.Add(new TemplateKey(item13.ItemType, item13.TemplateId));
		}
	}

	private static void InitializeCandidateSettlements(List<short> settlementIds)
	{
		Type type = Type.GetType("GameData.Domains.Organization.OrganizationDomain");
		Tester.Assert(type != null);
		FieldInfo field = type.GetField("_settlements", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		Tester.Assert(field != null);
		Dictionary<short, Settlement> dictionary = (Dictionary<short, Settlement>)field.GetValue(DomainManager.Organization);
		Tester.Assert(dictionary != null);
		foreach (var (item, _) in dictionary)
		{
			settlementIds.Add(item);
		}
	}

	private static void InitializeCandidateCrickets(List<(short colorId, short partId)> crickets)
	{
		List<short> list = new List<short>();
		List<short> list2 = new List<short>();
		foreach (CricketPartsItem item in (IEnumerable<CricketPartsItem>)CricketParts.Instance)
		{
			switch (item.Type)
			{
			case ECricketPartsType.Trash:
			case ECricketPartsType.King:
			case ECricketPartsType.RealColor:
				crickets.Add((item.TemplateId, 0));
				break;
			case ECricketPartsType.Parts:
				list.Add(item.TemplateId);
				break;
			default:
				list2.Add(item.TemplateId);
				break;
			}
		}
		foreach (short item2 in list)
		{
			foreach (short item3 in list2)
			{
				crickets.Add((item3, item2));
			}
		}
	}

	private void AddNotificationArguments(IRandomSource random, List<object> arguments, sbyte paramType, ref GameData.Domains.Character.Character character)
	{
		switch (paramType)
		{
		case 0:
		{
			GameData.Domains.Character.Character character2 = ((character != null) ? SelectRandomCharacter(random, character.GetId()) : (character = SelectRandomCharacter(random)));
			arguments.Add(character2.GetId());
			break;
		}
		case 1:
		{
			Location location;
			if (character == null)
			{
				short areaId = (short)random.Next(139);
				location = new Location(areaId, -1);
			}
			else
			{
				location = character.GetLocation();
				if (!location.IsValid())
				{
					location = character.GetValidLocation();
				}
			}
			arguments.Add(location);
			break;
		}
		case 2:
		{
			int index7 = random.Next(_candidateItems.Count);
			TemplateKey templateKey = _candidateItems[index7];
			arguments.Add(templateKey.ItemType);
			arguments.Add(templateKey.TemplateId);
			break;
		}
		case 3:
		{
			int index6 = random.Next(_candidateCombatSkills.Count);
			short num10 = _candidateCombatSkills[index6];
			arguments.Add(num10);
			break;
		}
		case 4:
		{
			sbyte b7 = (sbyte)random.Next(0, 8);
			arguments.Add(b7);
			break;
		}
		case 5:
		{
			int index5 = random.Next(_candidateSettlements.Count);
			short num9 = _candidateSettlements[index5];
			arguments.Add(num9);
			break;
		}
		case 6:
		{
			Tester.Assert(character != null);
			OrganizationInfo organizationInfo = character.GetOrganizationInfo();
			arguments.Add(organizationInfo.OrgTemplateId);
			arguments.Add(organizationInfo.Grade);
			arguments.Add(organizationInfo.Principal);
			arguments.Add(character.GetGender());
			break;
		}
		case 7:
		{
			int index4 = random.Next(_candidateBuildings.Count);
			short num8 = _candidateBuildings[index4];
			arguments.Add(num8);
			break;
		}
		case 8:
		{
			sbyte b6 = (sbyte)random.Next(0, 9);
			arguments.Add(b6);
			break;
		}
		case 9:
		{
			sbyte b5 = (sbyte)random.Next(0, 9);
			arguments.Add(b5);
			break;
		}
		case 10:
		{
			int index3 = random.Next(_candidateAdventures.Count);
			short num7 = _candidateAdventures[index3];
			arguments.Add(num7);
			break;
		}
		case 11:
		{
			sbyte b4 = (sbyte)random.Next(0, 5);
			arguments.Add(b4);
			break;
		}
		case 12:
		{
			short favorability = (short)random.Next(-30000, 30001);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			arguments.Add(favorabilityType);
			break;
		}
		case 13:
		{
			int index2 = random.Next(_candidateCrickets.Count);
			var (num5, num6) = _candidateCrickets[index2];
			arguments.Add(num5);
			arguments.Add(num6);
			break;
		}
		case 14:
		{
			short num4 = ItemSubType.GetRandom(random);
			if (!ItemSubType.IsHobbyType(num4))
			{
				num4 = -1;
			}
			arguments.Add(num4);
			break;
		}
		case 15:
		{
			int index = random.Next(_candidateChickens.Count);
			short num3 = _candidateChickens[index];
			arguments.Add(num3);
			break;
		}
		case 16:
		{
			short num2 = (short)random.Next(0, 112);
			arguments.Add(num2);
			break;
		}
		case 17:
		{
			sbyte b3 = (sbyte)random.Next(0, 7);
			arguments.Add(b3);
			break;
		}
		case 18:
		{
			sbyte b2 = (sbyte)random.Next(0, 2);
			arguments.Add(b2);
			break;
		}
		case 19:
		{
			sbyte b = (sbyte)random.Next(0, 6);
			arguments.Add(b);
			break;
		}
		case 22:
		{
			int num = random.Next(1000);
			arguments.Add(num);
			break;
		}
		default:
			throw new Exception($"Unsupported ParameterType: {paramType}");
		}
	}

	private GameData.Domains.Character.Character SelectRandomCharacter(IRandomSource random, int exceptedCharId = -1)
	{
		for (int i = 0; i < 100; i++)
		{
			int index = random.Next(_candidatesCharacters.Count);
			GameData.Domains.Character.Character character = _candidatesCharacters[index];
			if (exceptedCharId < 0 || character.GetId() != exceptedCharId)
			{
				return character;
			}
		}
		throw new Exception("Exceeded max retry count");
	}

	[DomainMethod]
	public bool GmCmd_AddMonthlyEvent(short startTemplateId, short endTemplateId, int selfCharId, int targetCharId)
	{
		bool result = false;
		if (endTemplateId == -1)
		{
			endTemplateId = startTemplateId;
		}
		for (short num = startTemplateId; num <= endTemplateId; num++)
		{
			if (_canTestMonthlyEventTemplateIdList.Contains(startTemplateId))
			{
				(short, int, int) item = (num, selfCharId, targetCharId);
				if (!_testMonthlyEventList.Contains(item))
				{
					_testMonthlyEventList.Add(item);
				}
				CharacterDomain.AddLockMovementCharSet(selfCharId);
				result = true;
			}
		}
		return result;
	}

	public void AddTestMonthlyEvent(DataContext context)
	{
		GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = taiwuChar.GetId();
		Location validLocation = taiwuChar.GetValidLocation();
		short num = -1;
		short num2 = -1;
		byte value = CombatSkillStateHelper.GeneratePageTypesFromReadingState(context.Random, 0);
		byte b = 0;
		PoisonItem poisonConfig;
		foreach (var testMonthlyEvent in _testMonthlyEventList)
		{
			short item = testMonthlyEvent.monthlyEventTemplateId;
			int item2 = testMonthlyEvent.selfCharId;
			int item3 = testMonthlyEvent.targetCharId;
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(item2);
			switch (item)
			{
			case 87:
				monthlyEventCollection.AddRequestPlayCombat(item2, validLocation, id);
				break;
			case 88:
				monthlyEventCollection.AddRequestNormalCombat(item2, validLocation, id);
				break;
			case 89:
				monthlyEventCollection.AddRequestLifeSkillBattle(item2, validLocation, id);
				break;
			case 90:
				monthlyEventCollection.AddRequestCricketBattle(item2, validLocation, id);
				break;
			case 66:
			{
				num = Config.Medicine.Instance.First((MedicineItem m) => m.EffectType == EMedicineEffectType.RecoverOuterInjury).TemplateId;
				ItemKey itemKey = AddItem(context, num, 8, id);
				character.ChangeInjury(context, 0, isInnerInjury: false, 1);
				monthlyEventCollection.AddRequestHealOuterInjuryByItem(item2, validLocation, id, (ulong)itemKey, 0);
				break;
			}
			case 68:
			{
				num = Config.Medicine.Instance.First((MedicineItem m) => m.EffectType == EMedicineEffectType.RecoverInnerInjury).TemplateId;
				ItemKey itemKey = AddItem(context, num, 8, id);
				character.ChangeInjury(context, 0, isInnerInjury: true, 1);
				monthlyEventCollection.AddRequestHealInnerInjuryByItem(item2, validLocation, id, (ulong)itemKey, 0);
				break;
			}
			case 70:
			{
				poisonConfig = Poison.Instance.FirstOrDefault((PoisonItem p) => !taiwuChar.HasPoisonImmunity(p.TemplateId));
				num = Config.Medicine.Instance.First((MedicineItem m) => m.EffectType == EMedicineEffectType.DetoxPoison && m.DetoxPoisonType == poisonConfig.TemplateId).TemplateId;
				ItemKey itemKey = AddItem(context, num, 8, id);
				character.ChangePoisoned(context, poisonConfig.TemplateId, 0, 100);
				monthlyEventCollection.AddRequestHealPoisonByItem(item2, validLocation, id, (ulong)itemKey, 0);
				break;
			}
			case 72:
			{
				num = Config.Medicine.Instance.First((MedicineItem m) => m.EffectType == EMedicineEffectType.RecoverHealth).TemplateId;
				ItemKey itemKey = AddItem(context, num, 8, id);
				character.ChangeHealth(context, -10);
				monthlyEventCollection.AddRequestHealth(item2, validLocation, id, (ulong)itemKey);
				break;
			}
			case 73:
			{
				num = Config.Medicine.Instance.First((MedicineItem m) => m.EffectType == EMedicineEffectType.ChangeDisorderOfQi).TemplateId;
				ItemKey itemKey = AddItem(context, num, 8, id);
				character.ChangeDisorderOfQi(context, 1000);
				monthlyEventCollection.AddRequestHealDisorderOfQi(item2, validLocation, id, (ulong)itemKey);
				break;
			}
			case 74:
			{
				num = Config.Misc.Instance.First((MiscItem m) => m.Neili > 0).TemplateId;
				ItemKey itemKey = AddItem(context, num, 12, id);
				character.ChangeCurrNeili(context, -Math.Min(character.GetMaxNeili(), 100));
				monthlyEventCollection.AddRequestNeili(item2, validLocation, id, (ulong)itemKey);
				break;
			}
			case 75:
			{
				num = Config.Medicine.Instance.First((MedicineItem m) => m.WugType == 0).TemplateId;
				ItemKey itemKey = AddItem(context, num, 8, id);
				ItemKey itemKey2 = AddWug(context, item2);
				monthlyEventCollection.AddRequestKillWug(item2, validLocation, id, (ulong)itemKey, (ulong)itemKey2);
				break;
			}
			case 76:
			{
				num = Config.Food.Instance.First().TemplateId;
				ItemKey itemKey = AddItem(context, num, 7, id);
				monthlyEventCollection.AddRequestFood(item2, validLocation, id, (ulong)itemKey, 1);
				break;
			}
			case 77:
			{
				num = Config.TeaWine.Instance[1].TemplateId;
				ItemKey itemKey = AddItem(context, num, 9, id);
				monthlyEventCollection.AddRequestTeaWine(item2, validLocation, id, (ulong)itemKey);
				break;
			}
			case 78:
				monthlyEventCollection.AddRequestResource(item2, validLocation, id, 1, 0);
				break;
			case 79:
			{
				num = 91;
				ItemKey itemKey = AddItem(context, num, 12, id);
				monthlyEventCollection.AddRequestItem(item2, validLocation, id, (ulong)itemKey, 1);
				break;
			}
			case 80:
			{
				num = Config.Weapon.Instance.First((WeaponItem w) => w.Grade == 2 && w.ResourceType == 1).TemplateId;
				ItemKey itemKey = AddItem(context, num, 0, item2);
				num2 = Config.CraftTool.Instance.First((CraftToolItem w) => w.Grade == 2 && w.RequiredLifeSkillTypes.Contains(7)).TemplateId;
				ItemKey itemKey3 = AddItem(context, num2, 6, id);
				monthlyEventCollection.AddRequestRepairItem(item2, validLocation, id, (ulong)itemKey, (ulong)itemKey3, 1, 1);
				break;
			}
			case 81:
			{
				num = Config.Weapon.Instance.First((WeaponItem w) => w.Grade == 2 && w.ResourceType == 2).TemplateId;
				ItemKey itemKey = AddItem(context, num, 0, item2, equip: true);
				num2 = Config.Medicine.Instance.First((MedicineItem m) => m.EffectType == EMedicineEffectType.ApplyPoison).TemplateId;
				ItemKey itemKey3 = AddItem(context, num2, 8, id);
				monthlyEventCollection.AddRequestAddPoisonToItem(item2, validLocation, id, (ulong)itemKey, (ulong)itemKey3);
				break;
			}
			case 82:
			{
				SkillBookItem skillBookItem = Config.SkillBook.Instance.FirstOrDefault((SkillBookItem s) => s.LifeSkillType > 0 && character.FindLearnedLifeSkillIndex(s.LifeSkillTemplateId) < 0);
				if (skillBookItem == null)
				{
					Logger.Warn($"未找到{character}未学过的技艺书籍");
				}
				else
				{
					ItemKey itemKey = AddItem(context, skillBookItem.TemplateId, 10, item2);
					monthlyEventCollection.AddRequestInstructionOnLifeSkill(item2, validLocation, id, itemKey.ItemType, itemKey.TemplateId, 1);
				}
				break;
			}
			case 83:
			{
				SkillBookItem skillBookItem = Config.SkillBook.Instance.FirstOrDefault((SkillBookItem s) => s.CombatSkillType >= 0 && !character.GetLearnedCombatSkills().Contains(s.CombatSkillTemplateId));
				if (skillBookItem == null)
				{
					Logger.Warn($"未找到{character}未学过的功法书籍");
				}
				else
				{
					ItemKey itemKey = AddItem(context, skillBookItem.TemplateId, 10, item2);
					monthlyEventCollection.AddRequestInstructionOnCombatSkill(item2, validLocation, id, itemKey.ItemType, itemKey.TemplateId, CombatSkillStateHelper.GetPageId(b) + 1, b, value);
				}
				break;
			}
			case 84:
			{
				SkillBookItem skillBookItem = Config.SkillBook.Instance.FirstOrDefault((SkillBookItem s) => s.LifeSkillType >= 0 && character.FindLearnedLifeSkillIndex(s.LifeSkillTemplateId) < 0);
				if (skillBookItem == null)
				{
					Logger.Warn($"未找到{character}未学过的技艺书籍");
				}
				else
				{
					ItemKey itemKey = AddItem(context, skillBookItem.TemplateId, 10, item2);
					monthlyEventCollection.AddRequestInstructionOnReadingLifeSkill(item2, validLocation, id, (ulong)itemKey, 1);
				}
				break;
			}
			case 85:
			{
				SkillBookItem skillBookItem = Config.SkillBook.Instance.FirstOrDefault((SkillBookItem s) => s.CombatSkillType >= 0 && !character.GetLearnedCombatSkills().Contains(s.CombatSkillTemplateId));
				if (skillBookItem == null)
				{
					Logger.Warn($"未找到{character}未学过的功法书籍");
				}
				else
				{
					ItemKey itemKey = AddItem(context, skillBookItem.TemplateId, 10, item2);
					monthlyEventCollection.AddRequestInstructionOnReadingCombatSkill(item2, validLocation, id, (ulong)itemKey, CombatSkillStateHelper.GetPageId(b) + 1, b);
				}
				break;
			}
			case 86:
			{
				SkillBookItem skillBookItem = Config.SkillBook.Instance.FirstOrDefault((SkillBookItem s) => s.CombatSkillType >= 0 && !character.GetLearnedCombatSkills().Contains(s.CombatSkillTemplateId));
				if (skillBookItem == null)
				{
					Logger.Warn($"未找到{character}未学过的功法书籍");
				}
				else
				{
					character.LearnNewCombatSkill(context, skillBookItem.CombatSkillTemplateId, 32767);
					monthlyEventCollection.AddRequestInstructionOnBreakout(item2, validLocation, id, skillBookItem.CombatSkillTemplateId);
				}
				break;
			}
			case 114:
				taiwuChar.ChangeInjury(context, 0, isInnerInjury: true, 1);
				monthlyEventCollection.AddAdviseHealInjury(item2, validLocation, id, 1);
				break;
			case 115:
				poisonConfig = Poison.Instance.FirstOrDefault((PoisonItem p) => !taiwuChar.HasPoisonImmunity(p.TemplateId));
				taiwuChar.ChangePoisoned(context, poisonConfig.TemplateId, 0, 100);
				monthlyEventCollection.AddAdviseHealPoison(item2, validLocation, id, 1);
				break;
			case 116:
			{
				num = Config.Weapon.Instance.First((WeaponItem w) => w.Grade == 2 && w.ResourceType == 1).TemplateId;
				ItemKey itemKey = AddItem(context, num, 0, id);
				num2 = Config.CraftTool.Instance.First((CraftToolItem w) => w.Grade == 2 && w.RequiredLifeSkillTypes.Contains(7)).TemplateId;
				ItemKey itemKey3 = AddItem(context, num2, 6, item2);
				monthlyEventCollection.AddAdviseRepairItem(item2, validLocation, id, (ulong)itemKey, (ulong)itemKey3, 1, 1);
				break;
			}
			case 117:
				monthlyEventCollection.AddAdviseBarb(item2, validLocation, id);
				break;
			case 118:
				monthlyEventCollection.AddAskForMoney(item2, validLocation, id);
				break;
			case 353:
				taiwuChar.ChangeDisorderOfQi(context, 1000);
				monthlyEventCollection.AddAdviseHealDisorderOfQi(item2, validLocation, id, 1);
				break;
			case 354:
				taiwuChar.ChangeHealth(context, -10);
				monthlyEventCollection.AddAdviseHealHealth(item2, validLocation, id, 1);
				break;
			case 290:
			{
				Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(item2);
				short num3 = -1;
				num3 = charCombatSkills.Keys.FirstOrDefault((short combatSkillTemplateId) => !taiwuChar.GetLearnedCombatSkills().Contains(combatSkillTemplateId));
				if (num3 < 0 || charCombatSkills.Keys.Count == 0)
				{
					Logger.Warn("未找到可以指点的功法书籍");
				}
				else
				{
					monthlyEventCollection.AddTeachCombatSkill(item2, validLocation, id, num3);
				}
				break;
			}
			case 61:
				monthlyEventCollection.AddAskProtectByRevengeAttack(item2, validLocation, item3, id);
				break;
			default:
			{
				MonthlyEventItem item4 = Config.MonthlyEvent.Instance.GetItem(item);
				if (item4 == null)
				{
					Logger.Warn($"未找到ID为{item}的过月事件");
				}
				else
				{
					Logger.Warn("未添加过月事件 " + item4.Name + " 的测试代码");
				}
				break;
			}
			}
		}
		_testMonthlyEventList.Clear();
	}

	private ItemKey AddItem(DataContext context, short itemTemplateId, sbyte itemType, int charId, bool equip = false)
	{
		ItemKey itemKey = DomainManager.Item.CreateItem(context, itemType, itemTemplateId);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		element_Objects.AddInventoryItem(context, itemKey, 1);
		if (ItemType.IsEquipmentItemType(itemType))
		{
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			baseItem.SetCurrDurability(Convert.ToInt16((float)baseItem.GetMaxDurability() * 0.5f), context);
			if (equip)
			{
				ItemKey[] equipment = element_Objects.GetEquipment();
				if (equipment[0].IsValid())
				{
					element_Objects.ChangeEquipment(context, 0, -1, equipment[0]);
				}
				element_Objects.ChangeEquipment(context, -1, 0, itemKey);
			}
		}
		return itemKey;
	}

	private ItemKey AddWug(DataContext context, int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		short templateId = 347;
		ItemKey itemKey = new ItemKey(8, 0, templateId, -1);
		element_Objects.AddWug(context, itemKey);
		return itemKey;
	}

	public float GetProbAdjustOfCreatingCharacter()
	{
		return _probAdjustOfCreatingCharacter;
	}

	public void RecordWorldStandardPopulation(DataContext context)
	{
		int worldPopulation = DomainManager.Character.GetWorldPopulation();
		SetWorldStandardPopulation(worldPopulation, context);
	}

	public void ChangeWorldPopulation(DataContext context, byte oriWorldPopulationType)
	{
		int num = _worldStandardPopulation * 100 / GetWorldPopulationFactor(oriWorldPopulationType);
		int worldPopulationFactor = GetWorldPopulationFactor();
		int value = num * worldPopulationFactor / 100;
		SetWorldStandardPopulation(value, context);
	}

	public void UpdatePopulationRelatedData()
	{
		int worldStandardPopulation = GetWorldStandardPopulation();
		int worldPopulation = DomainManager.Character.GetWorldPopulation();
		_probAdjustOfCreatingCharacter = 1f;
		Logger.Info($"Currrent Population / World Standard Population: {worldPopulation}/{worldStandardPopulation}, Probability Adjustment of Creating Character: {(int)Math.Round(_probAdjustOfCreatingCharacter * 100f)}%");
	}

	private void AdvanceMonth_SectClearData(DataContext context)
	{
		_sectMainStoryCombatTimesShaolin = 0;
		_sectMainStoryDefeatingXuannv = false;
		_sectMainStoryLifeLinkUpdated = false;
		Events.UnRegisterHandler_AdvanceMonthFinish(AdvanceMonth_SectClearData);
		_storyStatus.Clear();
	}

	public int GetSectMainStoryCombatTimesShaolin()
	{
		return _sectMainStoryCombatTimesShaolin;
	}

	public bool GetSectMainStoryDefeatingXuannv()
	{
		return _sectMainStoryDefeatingXuannv;
	}

	public void SetSectMainStoryDefeatingXuannv()
	{
		_sectMainStoryDefeatingXuannv = true;
	}

	public bool CheckSectMainStoryAvailable(sbyte orgTemplateId)
	{
		if (DomainManager.World.GetSectMainStoryTaskStatus(orgTemplateId) != 0)
		{
			return false;
		}
		OrganizationItem organizationItem = Config.Organization.Instance[orgTemplateId];
		int[] taskChains = organizationItem.TaskChains;
		if (taskChains == null || taskChains.Length <= 0)
		{
			return false;
		}
		for (int i = 0; i < organizationItem.TaskChains.Length; i++)
		{
			if (DomainManager.Extra.IsExtraTaskChainInProgress(organizationItem.TaskChains[i]))
			{
				return false;
			}
		}
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(orgTemplateId);
		sbyte largeSectIndex = OrganizationDomain.GetLargeSectIndex(orgTemplateId);
		int arg = int.MaxValue;
		if (sectMainStoryEventArgBox.Get(SectMainStoryEventArgKeys.GoodEndDateKeys[largeSectIndex], ref arg) || sectMainStoryEventArgBox.Get(SectMainStoryEventArgKeys.BadEndDateKeys[largeSectIndex], ref arg))
		{
			return false;
		}
		return true;
	}

	public void TriggerSectMainStoryEndingCountDown(DataContext context, sbyte orgTemplateId, bool isGoodEnding)
	{
		if (GetSectMainStoryTaskStatus(orgTemplateId) != 0)
		{
			Logger.AppendWarning("Sect main story for " + Config.Organization.Instance[orgTemplateId].Name + " is already finished.");
			return;
		}
		sbyte largeSectIndex = OrganizationDomain.GetLargeSectIndex(orgTemplateId);
		string key = (isGoodEnding ? SectMainStoryEventArgKeys.GoodEndDateKeys[largeSectIndex] : SectMainStoryEventArgKeys.BadEndDateKeys[largeSectIndex]);
		DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, orgTemplateId, key, _currDate);
	}

	public sbyte GetSectMainStoryTaskStatus(sbyte orgTemplateId)
	{
		return _stateTaskStatuses[orgTemplateId - 1];
	}

	public void SetSectMainStoryTaskStatus(DataContext context, sbyte orgTemplateId, sbyte status)
	{
		SetElement_StateTaskStatuses(orgTemplateId - 1, status, context);
	}

	public void StatSectMainStoryCombatTimes(short combatConfigTemplateId)
	{
		CombatConfigItem combatConfigItem = CombatConfig.Instance[combatConfigTemplateId];
		bool flag = DomainManager.Extra.IsExtraTaskChainInProgress(30);
		bool flag2 = flag;
		if (flag2)
		{
			sbyte combatType = combatConfigItem.CombatType;
			bool flag3 = (uint)(combatType - 1) <= 1u;
			flag2 = flag3;
		}
		if (flag2)
		{
			_sectMainStoryCombatTimesShaolin++;
		}
	}

	private void AdvanceMonth_SectMainStory(DataContext context)
	{
		AdvanceMonth_SectMainStory_Kongsang(context);
		AdvanceMonth_SectMainStory_Xuehou(context);
		AdvanceMonth_SectMainStory_Shaolin(context);
		AdvanceMonth_SectMainStory_Xuannv(context);
		AdvanceMonth_SectMainStory_Wudang(context);
		AdvanceMonth_SectMainStory_Shixiang(context);
		AdvanceMonth_SectMainStory_Emei(context);
		AdvanceMonth_SectMainStory_Jingang(context);
		AdvanceMonth_SectMainStory_Wuxian(context);
		AdvanceMonth_SectMainStory_Ranshan(context);
		AdvanceMonth_SectMainStory_Baihua(context);
		AdvanceMonth_SectMainStory_Zhujian(context);
		AdvanceMonth_SectMainStoryFulong(context);
		UpdateBaihuaLifeLinkNeiliType(context);
		UpdateAreaMerchantType(context);
		DomainManager.Extra.UpdateThreeVitalsInfection(context);
		Events.RegisterHandler_AdvanceMonthFinish(AdvanceMonth_SectClearData);
	}

	[DomainMethod]
	public int GetSectMainStoryActiveStatus(sbyte orgTemplateId)
	{
		bool flag = !GameData.ArchiveData.Common.IsInWorld();
		bool flag2 = flag;
		if (!flag2)
		{
			bool flag3 = ((orgTemplateId < 1 || orgTemplateId > 15) ? true : false);
			flag2 = flag3;
		}
		if (flag2)
		{
			return int.MinValue;
		}
		int[] taskChains = Config.Organization.Instance[orgTemplateId].TaskChains;
		if (taskChains == null || taskChains.Length == 0)
		{
			return 2;
		}
		if (!CheckSectMainStoryAvailable(orgTemplateId) || _storyStatus.Contains(orgTemplateId))
		{
			return 1;
		}
		EventArgBox sectMainStoryEventArgBox = EventHelper.GetSectMainStoryEventArgBox(orgTemplateId);
		return sectMainStoryEventArgBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus) ? (-1) : 0;
	}

	public bool SectMainStoryTriggeredThisMonth(sbyte orgTemplateId)
	{
		return _storyStatus.Contains(orgTemplateId);
	}

	[DomainMethod]
	public void SetSectMainStoryActiveStatus(sbyte orgTemplateId, bool pause)
	{
		if (pause)
		{
			EventHelper.SaveArgToSectMainStory(orgTemplateId, SectMainStoryEventArgKeys.TriggeringStatus, -1);
			if (orgTemplateId == 2 && EventHelper.GetSectMainStoryEventArgBox(orgTemplateId).Contains<short>("ConchShip_PresetKey_WhiteApeBlockId"))
			{
				EventHelper.ClearWhiteApeBlockId();
			}
		}
		else
		{
			EventHelper.RemoveArgFromSectMainStory<int>(orgTemplateId, SectMainStoryEventArgKeys.TriggeringStatus);
			if (orgTemplateId == 2 && DomainManager.Organization.GetSettlementByOrgTemplateId(2).CalcApprovingRate() >= 500)
			{
				EventHelper.SaveWhiteApeBlockId();
			}
		}
	}

	[DomainMethod]
	public void NotifySectStoryActivated(DataContext context, sbyte orgTemplateId)
	{
		_storyStatus.Add(orgTemplateId);
		EventHelper.RemoveArgFromSectMainStory<int>(orgTemplateId, SectMainStoryEventArgKeys.TriggeringStatus);
		SetSectMainStoryTaskStatus(context, orgTemplateId, GetSectMainStoryTaskStatus(orgTemplateId));
	}

	private void AdvanceMonth_SectMainStory_Kongsang(DataContext context)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(10);
		int arg = int.MaxValue;
		if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryKongsangProsperousEndDate", ref arg) && _currDate >= arg + 1)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddSectMainStoryKongsangProsperous();
			return;
		}
		arg = int.MaxValue;
		if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryKongsangFailingEndDate", ref arg) && _currDate >= arg + 1)
		{
			MonthlyEventCollection monthlyEventCollection2 = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection2.AddSectMainStoryKongsangFailing();
		}
		else
		{
			if (!DomainManager.Character.TryGetFixedCharacterByTemplateId(668, out var character))
			{
				return;
			}
			int extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(27);
			int arg2 = int.MaxValue;
			switch (extraTaskChainCurrentTask)
			{
			case 105:
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 10, "ConchShip_PresetKey_MissionUnacceptedEventTriggeredSameMonth", value: true);
				break;
			case 106:
			case 109:
			case 112:
				if (!character.GetLocation().IsValid() && sectMainStoryEventArgBox.Get("ConchShip_PresetKey_LiaoWumingQuestStartDate", ref arg2) && arg2 <= _currDate)
				{
					MonthlyEventCollection monthlyEventCollection4 = DomainManager.World.GetMonthlyEventCollection();
					monthlyEventCollection4.AddSectMainStoryKongsangTargetFound();
				}
				break;
			case 117:
				if (!character.GetLocation().IsValid() && sectMainStoryEventArgBox.Get("ConchShip_PresetKey_KongsangAdventureCountDown", ref arg2) && arg2 <= _currDate)
				{
					MonthlyEventCollection monthlyEventCollection3 = DomainManager.World.GetMonthlyEventCollection();
					monthlyEventCollection3.AddSectMainStoryKongsangAdventure();
				}
				break;
			}
		}
	}

	private void AdvanceMonth_SectMainStory_Xuehou(DataContext context)
	{
		int extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(28);
		if (extraTaskChainCurrentTask < 0)
		{
			extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(29);
		}
		EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(15);
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		if (IsJixiFree())
		{
			bool arg = false;
			if (argBox.Get("ConchShip_PresetKey_NeedTriggerPassLegacyMonthlyNotification", ref arg))
			{
				argBox.Set("ConchShip_PresetKey_NeedTriggerPassLegacyMonthlyNotification", arg: false);
				TryTriggerXuehouJixiGone();
			}
		}
		if (extraTaskChainCurrentTask < 0)
		{
			int arg2 = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_SectMainStoryXuehouProsperousEndDate", ref arg2) && _currDate >= arg2 + 1)
			{
				monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddSectMainStoryXuehouProsperous();
				return;
			}
			arg2 = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_SectMainStoryXuehouFailingEndDate", ref arg2) && _currDate >= arg2 + 1)
			{
				monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddSectMainStoryXuehouFailing();
			}
			return;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location taiwuLocation = taiwu.GetLocation();
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(15);
		Location location = settlementByOrgTemplateId.GetLocation();
		switch (extraTaskChainCurrentTask)
		{
		case 119:
			if (AreaHasAdultGraveOfTargetOrganization(location.AreaId, 15))
			{
				int arg5 = int.MaxValue;
				if (!argBox.Contains<bool>("ConchShip_PresetKey_XuehouGraveDiggingEventTriggered"))
				{
					monthlyEventCollection.AddSectMainStoryXuehouGraveDigging();
				}
				else if (!argBox.Contains<int>("ConchShip_PresetKey_XuehouGraveDiggingNormalTriggerTime") || (argBox.Get("ConchShip_PresetKey_XuehouGraveDiggingNormalTriggerTime", ref arg5) && arg5 + 6 <= _currDate))
				{
					monthlyEventCollection.AddSectMainStoryXuehouGraveDiggingNormal();
				}
			}
			else
			{
				monthlyEventCollection.AddSectMainStoryXuehouStrangeDeath();
			}
			break;
		case 120:
		{
			int arg8 = int.MaxValue;
			argBox.Get("ConchShip_PresetKey_FirstGotBellTime", ref arg8);
			if (taiwuLocation.AreaId == location.AreaId && arg8 + 3 <= _currDate)
			{
				monthlyEventCollection.AddSectMainStoryXuehouOldManAppears();
			}
			break;
		}
		case 121:
		{
			int arg9 = int.MaxValue;
			if (!argBox.GetBool("ConchShip_PresetKey_XuehouOldManGraveDisappearTriggered") && argBox.Get("ConchShip_PresetKey_DefeatXuehouOldManTime", ref arg9) && arg9 + 3 <= _currDate)
			{
				monthlyEventCollection.AddSectMainStoryXuehouOldManReturns();
				break;
			}
			int arg10 = int.MaxValue;
			if (argBox.GetBool("ConchShip_PresetKey_XuehouOldManGraveDisappearTriggered") || (argBox.Get("ConchShip_PresetKey_GiveBellToXuehouOldManTime", ref arg10) && arg10 + 3 <= _currDate))
			{
				GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId3 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 522);
				int id = orCreateFixedCharacterByTemplateId3.GetId();
				Location location3 = orCreateFixedCharacterByTemplateId3.GetLocation();
				DealXuehouOldManBell(orCreateFixedCharacterByTemplateId3);
				DomainManager.Extra.TriggerExtraTask(context, 28, 122);
				List<MapBlockData> obj3 = context.AdvanceMonthRelatedData.Blocks.Occupy();
				DomainManager.Map.GetSettlementAffiliatedBlocks(location.AreaId, location.BlockId, obj3);
				Location location4 = obj3.GetRandom(context.Random).GetLocation();
				context.AdvanceMonthRelatedData.Blocks.Release(ref obj3);
				Events.RaiseFixedCharacterLocationChanged(context, id, location3, location4);
				orCreateFixedCharacterByTemplateId3.SetLocation(location4, context);
			}
			break;
		}
		case 123:
		{
			List<Location> sectXuehouBloodLightLocations = DomainManager.Extra.GetSectXuehouBloodLightLocations();
			if (sectXuehouBloodLightLocations.Count > 0 && DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwuLocation, settlementByOrgTemplateId.GetId()))
			{
				GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId2 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 522);
				DealXuehouOldManBell(orCreateFixedCharacterByTemplateId2);
				Location location2 = orCreateFixedCharacterByTemplateId2.GetLocation();
				bool flag = taiwuLocation.Equals(location2);
				bool flag2 = sectXuehouBloodLightLocations.Contains(taiwuLocation);
				if (flag && flag2)
				{
					monthlyEventCollection.AddSectMainStoryXuehouOnBloodBlock();
				}
				else if (flag)
				{
					monthlyEventCollection.AddSectMainStoryXuehouOldManAttacks();
				}
			}
			break;
		}
		case 125:
			DealMonthlyEvent();
			break;
		case 126:
		{
			DealMonthlyEvent();
			bool flag3 = DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwuLocation, taiwuVillageSettlementId);
			short totalAdultVillagerCount = DomainManager.Taiwu.GetTotalAdultVillagerCount();
			if (flag3 && totalAdultVillagerCount >= 3)
			{
				switch (argBox.GetInt("ConchShip_PresetKey_JixiArrivedTaiwuMonthlyEventTriggeredCount"))
				{
				case 0:
					monthlyEventCollection.AddSectMainStoryXuehouHarmoniousTaiwu();
					break;
				case 1:
					monthlyEventCollection.AddSectMainStoryXuehouFeedJixi();
					break;
				case 2:
					monthlyEventCollection.AddSectMainStoryXuehouMythInVillage();
					break;
				}
			}
			break;
		}
		case 127:
		{
			int arg6 = -1;
			if (!argBox.Get("ConchShip_PresetKey_XuehouComingTime", ref arg6))
			{
				argBox.Set("ConchShip_PresetKey_XuehouComingTime", 3);
			}
			else
			{
				arg6++;
			}
			argBox.Set("ConchShip_PresetKey_XuehouComingTime", arg6);
			int arg7 = -1;
			argBox.Get("ConchShip_PresetKey_XuehouComingTriggeredCount", ref arg7);
			if (arg6 >= 3 && arg7 < GetJixiFavorabilityType() - 1)
			{
				monthlyEventCollection.AddSectMainStoryXuehouComing();
				argBox.Set("ConchShip_PresetKey_XuehouComingTime", 0);
				if (!argBox.Get("ConchShip_PresetKey_XuehouComingTriggeredCount", ref arg7))
				{
					argBox.Set("ConchShip_PresetKey_XuehouComingTriggeredCount", 1);
				}
				else
				{
					argBox.Set("ConchShip_PresetKey_XuehouComingTriggeredCount", arg7 + 1);
				}
			}
			if (TaiwuNotInVillageArea())
			{
				break;
			}
			if (JixiAdventureDisappear(3))
			{
				JixiGrowUp(context, 538, 539);
				DomainManager.Extra.TriggerExtraTask(context, 28, 134);
				DomainManager.Extra.FinishAllTaskInChain(context, 29);
			}
			else if (JixiAdventurePass(2, 0) || JixiAdventureDisappear(2))
			{
				if (JixiAdventureDisappear(2))
				{
					argBox.Set("ConchShip_PresetKey_JixiAdventureTwoStartDate", int.MaxValue);
					JixiGrowUp(context, 537, 538);
				}
				if (!argBox.Contains<int>("ConchShip_PresetKey_JixiAdventureThreeStartDate") && (JixiAdventurePass(2, 3) || JixiAdventureDisappear(2)))
				{
					List<MapBlockData> obj = context.AdvanceMonthRelatedData.Blocks.Occupy();
					DomainManager.Map.GetMapBlocksInAreaByFilters(taiwuVillageLocation.AreaId, IsInTaiwuVillageRange, obj);
					if (obj.Count > 0)
					{
						MapBlockData random = obj.GetRandom(context.Random);
						DomainManager.Adventure.TryCreateAdventureSite(context, random.AreaId, random.BlockId, 162, MonthlyActionKey.Invalid);
						argBox.Set("ConchShip_PresetKey_JixiAdventureThreeStartDate", _currDate);
					}
					context.AdvanceMonthRelatedData.Blocks.Release(ref obj);
				}
				else
				{
					if (!argBox.GetBool("ConchShip_PresetKey_JixiFeedChickenEventTriggered"))
					{
						monthlyEventCollection.AddSectMainStoryXuehouJixiFeedChicken();
					}
					if (!argBox.GetBool("ConchShip_PresetKey_JixiHarmvillagerEventTriggered"))
					{
						monthlyEventCollection.AddSectMainStoryXuehouJixiKills();
					}
					else
					{
						monthlyEventCollection.AddSectMainStoryXuehouVillageWork();
					}
				}
			}
			else
			{
				if (!JixiAdventurePass(1, 0) && !JixiAdventureDisappear(1))
				{
					break;
				}
				if (!argBox.Contains<int>("ConchShip_PresetKey_JixiAdventureTwoStartDate") && (JixiAdventurePass(1, 3) || JixiAdventureDisappear(1)))
				{
					List<MapBlockData> obj2 = context.AdvanceMonthRelatedData.Blocks.Occupy();
					DomainManager.Map.GetMapBlocksInAreaByFilters(taiwuVillageLocation.AreaId, IsInTaiwuVillageRange, obj2);
					if (obj2.Count > 0)
					{
						MapBlockData random2 = obj2.GetRandom(context.Random);
						DomainManager.Adventure.TryCreateAdventureSite(context, random2.AreaId, random2.BlockId, 161, MonthlyActionKey.Invalid);
						argBox.Set("ConchShip_PresetKey_JixiAdventureTwoStartDate", _currDate);
					}
					context.AdvanceMonthRelatedData.Blocks.Release(ref obj2);
				}
				else if (!argBox.GetBool("ConchShip_PresetKey_ProtectedJixiTriggered"))
				{
					monthlyEventCollection.AddSectMainStoryXuehouProtectJixi();
				}
				else
				{
					monthlyEventCollection.AddSectMainStoryXuehouJixiAskForFood();
				}
			}
			break;
		}
		case 134:
			if (!TaiwuNotInVillageArea())
			{
				int arg3 = int.MaxValue;
				int arg4 = int.MaxValue;
				if (!argBox.Contains<bool>("ConchShip_PresetKey_CombatWithUltimateZombieTriggered") && ((argBox.Get("ConchShip_PresetKey_JixiAdventureThreePassDate", ref arg3) && _currDate >= arg3 + 2) || (argBox.Get("ConchShip_PresetKey_JixiAdventureThreeStartDate", ref arg4) && _currDate >= arg4 + 9 + 2)))
				{
					monthlyEventCollection.AddSectMainStoryXuehouFinale();
				}
				arg4 = int.MaxValue;
				if (argBox.Get("ConchShip_PresetKey_JixiAdventureFourStartDate", ref arg4) && _currDate >= arg4 + 6)
				{
					DomainManager.Extra.FinishTriggeredExtraTask(context, 28, 134);
					DomainManager.World.SetSectMainStoryTaskStatus(context, 15, 1);
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 15, "ConchShip_PresetKey_SectMainStoryXuehouProsperousEndDate", _currDate + 1);
					JixiGrowUp(context, 539, 537);
					GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 537);
					Events.RaiseFixedCharacterLocationChanged(context, orCreateFixedCharacterByTemplateId.GetId(), orCreateFixedCharacterByTemplateId.GetLocation(), taiwuVillageLocation);
					orCreateFixedCharacterByTemplateId.SetLocation(taiwuVillageLocation, context);
					MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
					monthlyNotificationCollection.AddSectMainStoryXuehouJixiGoneFinal(taiwu.GetId());
				}
			}
			break;
		case 135:
			TryTriggerXuehouJixiGone();
			break;
		}
		DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 15);
		void DealMonthlyEvent()
		{
			int arg11 = int.MaxValue;
			if (!argBox.Contains<bool>("ConchShip_PresetKey_XuehouEmptyCaveTriggered") && argBox.Get("ConchShip_PresetKey_PassXuehouAdventure1Time", ref arg11) && _currDate >= arg11 + 1)
			{
				monthlyEventCollection.AddSectMainStoryXuehouEmptyGrave();
			}
			else if (!argBox.Contains<bool>("ConchShip_PresetKey_XuehouFindPeopleTriggered") && argBox.Get("ConchShip_PresetKey_PassXuehouAdventure1Time", ref arg11) && _currDate >= arg11 + 2)
			{
				monthlyEventCollection.AddSectMainStoryXuehouLookingForTaiwu(taiwu.GetId());
			}
		}
		void DealXuehouOldManBell(GameData.Domains.Character.Character oldMan)
		{
			bool arg11 = false;
			if (argBox.Contains<bool>("ConchShip_PresetKey_XuehouOldManHasBell") && argBox.Get("ConchShip_PresetKey_XuehouOldManHasBell", ref arg11))
			{
				ItemKey itemKey = DomainManager.Item.CreateItem(context, 12, 246);
				oldMan.AddInventoryItem(context, itemKey, 1);
			}
		}
		bool IsInTaiwuVillageRange(MapBlockData blockData)
		{
			if (blockData.GetConfig().TemplateId == 124)
			{
				return false;
			}
			if (DomainManager.Adventure.GetAdventuresInArea(blockData.AreaId).AdventureSites.ContainsKey(blockData.BlockId))
			{
				return false;
			}
			ByteCoordinate blockPos = DomainManager.Map.GetBlockData(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId).GetBlockPos();
			return blockData.GetManhattanDistanceToPos(blockPos.X, blockPos.Y) == 3;
		}
		bool TaiwuNotInVillageArea()
		{
			return !DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwuLocation, taiwuVillageSettlementId);
		}
		void TryTriggerXuehouJixiGone()
		{
			if (!argBox.Contains<bool>("ConchShip_PresetKey_PassLegacyMonthlyNotificationTriggered"))
			{
				int arg11 = -1;
				if (argBox.Get("ConchShip_PresetKey_AwakeJixiTaiwuId", ref arg11))
				{
					argBox.Set("ConchShip_PresetKey_PassLegacyMonthlyNotificationTriggered", arg: true);
					if (DomainManager.Character.IsCharacterAlive(arg11))
					{
						DomainManager.World.GetMonthlyNotificationCollection().AddSectMainStoryXuehouJixiGoneAgain();
					}
					else
					{
						DomainManager.World.GetMonthlyNotificationCollection().AddSectMainStoryXuehouJixiGone();
					}
				}
			}
		}
	}

	private void AdvanceMonth_SectMainStory_Shaolin(DataContext context)
	{
		MonthlyEventCollection monthlyEventCollection = GetMonthlyEventCollection();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = taiwu.GetId();
		Location location = taiwu.GetLocation();
		EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(1);
		int extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(30);
		int arg = -1;
		bool flag = argBox.Get("ConchShip_PresetKey_DamoDreamMeetTaiwuId", ref arg) && arg != id;
		int num = extraTaskChainCurrentTask;
		int num2 = num;
		if (num2 >= 0)
		{
			switch (num2)
			{
			case 220:
				if (location.IsValid() && DomainManager.Map.GetBlock(location).BlockSubType == EMapBlockSubType.ShaolinPai)
				{
					monthlyEventCollection.AddSectMainStoryShaolinTwoMonksTalk();
				}
				break;
			case 137:
				if (taiwu.GetInventory().GetInventoryItemCount(12, 247) > 0)
				{
					monthlyEventCollection.AddSectMainStoryShaolinDreamFirst();
				}
				break;
			case 221:
				if (flag)
				{
					monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
				}
				else
				{
					monthlyEventCollection.AddSectMainStoryShaolinLearning();
				}
				break;
			case 224:
			{
				if (flag)
				{
					monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
					break;
				}
				int arg3 = 0;
				bool flag2 = !argBox.Get("ConchShip_PresetKey_ShaolinMonthlyEventNotEnoughDate", ref arg3);
				int num4 = Math.Clamp(argBox.GetInt("ConchShip_PresetKey_ShaolinDamoFightTimes"), 0, 2);
				int num5 = ((num4 == 2) ? 6 : 3);
				sbyte arg4 = -1;
				if (!argBox.Get("ConchShip_PresetKey_ShaolinCombatSkillType", ref arg4) || arg4 < 0)
				{
					break;
				}
				IReadOnlyList<CombatSkillItem> learnableCombatSkills = CombatSkillDomain.GetLearnableCombatSkills(1, arg4);
				Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(taiwu.GetId());
				foreach (CombatSkillItem item in learnableCombatSkills)
				{
					sbyte combatSkillGradeGroup = CombatSkillDomain.GetCombatSkillGradeGroup(item.Grade, 1, arg4);
					if (combatSkillGradeGroup != num4 || (charCombatSkills.TryGetValue(item.TemplateId, out var value) && CombatSkillStateHelper.IsBrokenOut(value.GetActivationState())))
					{
						continue;
					}
					if (flag2)
					{
						monthlyEventCollection.AddSectMainStoryShaolinNotEnough();
					}
					else if (arg3 + num5 <= _currDate)
					{
						monthlyEventCollection.AddSectMainStoryShaolinNotEnoughCommon();
					}
					return;
				}
				if (num4 == 2)
				{
					AddEndChallengeMonthlyEvent();
				}
				else
				{
					AddChallengeMonthlyEvent();
				}
				break;
			}
			case 225:
				if (flag)
				{
					monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
				}
				else if (argBox.GetInt("ConchShip_PresetKey_ShaolinDamoFightTimes") >= 2)
				{
					AddEndChallengeMonthlyEvent();
				}
				else
				{
					AddChallengeMonthlyEvent();
				}
				break;
			case 227:
			{
				if (flag)
				{
					monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
					break;
				}
				int arg5 = 0;
				if (!argBox.Get("ConchShip_PresetKey_StudyForBodhidharmaChallenge", ref arg5) || arg5 <= _currDate)
				{
					if (argBox.GetInt("ConchShip_PresetKey_ShaolinDamoFightTimes") >= 2)
					{
						AddEndChallengeMonthlyEvent();
					}
					else
					{
						AddChallengeMonthlyEvent();
					}
				}
				break;
			}
			case 228:
				if (flag)
				{
					monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
				}
				else
				{
					monthlyEventCollection.AddSectMainStoryShaolinDreamOfReadingSutra();
				}
				break;
			case 229:
			{
				if (flag)
				{
					monthlyEventCollection.AddSectMainStoryShaolinDreamOfNewTaiwu();
					break;
				}
				short arg2 = -1;
				if (!argBox.Get("ConchShip_PresetKey_ShaolinReadingMaxGradeSutra", ref arg2) || arg2 < 0)
				{
					break;
				}
				int num3 = taiwu.FindLearnedLifeSkillIndex(arg2);
				if (num3 < 0)
				{
					break;
				}
				List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = taiwu.GetLearnedLifeSkills();
				if (learnedLifeSkills[num3].GetReadPagesCount() >= 3)
				{
					if (LifeSkill.Instance[arg2].Grade == 8)
					{
						monthlyEventCollection.AddSectMainStoryShaolinEnlightenment();
					}
					else
					{
						monthlyEventCollection.AddSectMainStoryShaolinLearning();
					}
				}
				break;
			}
			}
		}
		else
		{
			if (GetSectMainStoryTaskStatus(1) != 0)
			{
				return;
			}
			int arg6 = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_SectMainStoryShaolinProsperousEndDate", ref arg6))
			{
				if (_currDate >= arg6 + 1)
				{
					monthlyEventCollection.AddSectMainStoryShaolinProsperous();
				}
				return;
			}
			if (argBox.Get("ConchShip_PresetKey_SectMainStoryShaolinFailingEndDate", ref arg6))
			{
				if (_currDate >= arg6 + 1)
				{
					monthlyEventCollection.AddSectMainStoryShaolinFailing();
				}
				return;
			}
			Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(1);
			if (sect.GetTaiwuExploreStatus() != 0 && location.IsValid() && DomainManager.Map.GetBlock(location).BlockSubType == EMapBlockSubType.ShaolinPai && !argBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus))
			{
				monthlyEventCollection.AddSectMainStoryShaolinTowerFalling();
			}
		}
		void AddChallengeMonthlyEvent()
		{
			if (argBox.GetBool("ConchShip_PresetKey_ShaolinDamoTrialTriggered"))
			{
				monthlyEventCollection.AddSectMainStoryShaolinChallengeCommon();
			}
			else
			{
				monthlyEventCollection.AddSectMainStoryShaolinChallenge();
			}
		}
		void AddEndChallengeMonthlyEvent()
		{
			if (argBox.GetBool("ConchShip_PresetKey_ShaolinLearnedAny"))
			{
				if (argBox.GetBool("ConchShip_PresetKey_ShaolinDamoFightTriggered"))
				{
					monthlyEventCollection.AddSectMainStoryShaolinEndChallengeCommon();
				}
				else
				{
					monthlyEventCollection.AddSectMainStoryShaolinEndChallenge();
				}
			}
			else if (argBox.GetBool("ConchShip_PresetKey_ShaolinDamoFightTriggered"))
			{
				monthlyEventCollection.AddSectMainStoryShaolinNeverLearnChallengeCommon();
			}
			else
			{
				monthlyEventCollection.AddSectMainStoryShaolinNeverLearnChallenge();
			}
		}
	}

	private void AdvanceMonth_SectMainStory_Xuannv(DataContext context)
	{
		MonthlyEventCollection monthlyEventCollection = GetMonthlyEventCollection();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = taiwu.GetId();
		Location location = taiwu.GetLocation();
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(8);
		int extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(31);
		if (extraTaskChainCurrentTask < 0)
		{
			if (GetSectMainStoryTaskStatus(8) != 0)
			{
				return;
			}
			int arg = int.MaxValue;
			if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryXuannvProsperousEndDate", ref arg))
			{
				if (_currDate >= arg + 1)
				{
					monthlyEventCollection.AddSectMainStoryXuannvProsperous();
				}
				return;
			}
			if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryXuannvFailingEndDate", ref arg))
			{
				if (_currDate >= arg + 1)
				{
					monthlyEventCollection.AddSectMainStoryXuannvFailing();
				}
				return;
			}
			Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(8);
			if (sect.GetTaiwuExploreStatus() != 0 && location.IsValid() && DomainManager.Map.GetBlock(location).BlockSubType == EMapBlockSubType.XuannvPai && !sectMainStoryEventArgBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus))
			{
				monthlyEventCollection.AddSectMainStoryXuannvPrologue();
			}
			return;
		}
		if (sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_XuannvStoryTriggerFirstTrack"))
		{
			monthlyEventCollection.AddSectMainStoryXuannvFirstTrack();
		}
		switch (extraTaskChainCurrentTask)
		{
		case 139:
			if (sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_XuannStoryPartOneReceivingLetter"))
			{
				monthlyEventCollection.AddSectMainStoryXuannvLetter(id);
			}
			break;
		case 140:
			if (sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_XuannStoryPartOneReceivingLetter"))
			{
				monthlyEventCollection.AddSectMainStoryXuannvLetter(id);
			}
			if (sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_XuannStoryPartOne_WaitSecretGuestEvent"))
			{
				monthlyEventCollection.AddSectMainStoryXuannvDeadMessage(id);
			}
			if (taiwu.GetInjuries().HasAnyInjury() || taiwu.GetPoisoned().IsNonZero() || taiwu.GetDisorderOfQi() > 0)
			{
				monthlyEventCollection.AddSectMainStoryXuannvHealing(id);
			}
			break;
		case 146:
			monthlyEventCollection.AddSectMainStoryXuannvMirrorDream();
			break;
		case 148:
			monthlyEventCollection.AddSectMainStoryXuannvReincarnationDeath(id);
			break;
		case 248:
		{
			int arg2 = int.MaxValue;
			if (location.IsValid() && DomainManager.Map.GetBlock(location).BlockSubType == EMapBlockSubType.XuannvPai)
			{
				monthlyEventCollection.AddSectMainStoryXuannvMeetJuner(id);
			}
			else if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_Xuannv_PartThree_TakeLoverDate", ref arg2))
			{
				if (_currDate >= arg2 + 6)
				{
					monthlyEventCollection.AddSectMainStoryXuannvReincarnationDeath2(id);
				}
				else if (_currDate > arg2 && !sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_Xuannv_MonthlyEventTrigger_WithSister"))
				{
					monthlyEventCollection.AddSectMainStoryXuannvWithSister();
				}
			}
			break;
		}
		case 249:
			monthlyEventCollection.AddSectMainStoryXuannvStrangeMoan();
			break;
		}
	}

	private void AdvanceMonth_SectMainStory_Wudang(DataContext context)
	{
		MonthlyEventCollection monthlyEventCollection = GetMonthlyEventCollection();
		GameData.Domains.Character.Character character;
		bool flag = !DomainManager.Character.TryGetFixedCharacterByTemplateId(543, out character) || GetSectMainStoryTaskStatus(4) != 0;
		List<SectStoryHeavenlyTreeExtendable> allHeavenlyTrees = DomainManager.Extra.GetAllHeavenlyTrees();
		int num = 0;
		foreach (SectStoryHeavenlyTreeExtendable item in allHeavenlyTrees)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item.Id);
			short templateId = element_Objects.GetTemplateId();
			if (templateId == 677 && (ItemTemplateHelper.CheckIsHeavenlyNormalTreeSeeds(12, item.TemplateId) || flag))
			{
				monthlyEventCollection.AddSectMainStoryWudangHeavenlyTreeDestroyed2(item.Location);
			}
			else if (item.Location.IsValid() && templateId != 677 && ItemTemplateHelper.CheckIsHeavenlyNormalTreeSeeds(12, item.TemplateId))
			{
				num += XiangshuMinionsProtectWudangHeavenlyTree(context, item, normalTree: true);
			}
		}
		if (num > 0)
		{
			monthlyEventCollection.AddSectMainStoryWudangProtectHeavenlyTree2();
		}
		if (GetSectMainStoryTaskStatus(4) != 0)
		{
			return;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int taiwuCharId = taiwu.GetId();
		Location taiwuLocation = taiwu.GetLocation();
		EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(4);
		bool triggerTreeMonthlyNotification = false;
		if (DomainManager.Extra.IsExtraTaskChainInProgress(41))
		{
			if (DomainManager.Extra.IsExtraTaskInProgress(238))
			{
				List<SectStoryHeavenlyTreeExtendable> allHeavenlyTrees2 = DomainManager.Extra.GetAllHeavenlyTrees();
				int num2 = 0;
				foreach (SectStoryHeavenlyTreeExtendable item2 in allHeavenlyTrees2)
				{
					if (!ItemTemplateHelper.CheckIsHeavenlyNormalTreeSeeds(12, item2.TemplateId))
					{
						GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2.Id);
						short templateId2 = element_Objects2.GetTemplateId();
						if (item2.Location.IsValid() && templateId2 != 677)
						{
							num2 += XiangshuMinionsProtectWudangHeavenlyTree(context, item2, normalTree: false);
						}
					}
				}
				if (num2 > 0)
				{
					monthlyEventCollection.AddSectMainStoryWudangProtectHeavenlyTree();
				}
			}
			if (DomainManager.Extra.IsExtraTaskInProgress(236))
			{
				List<SectStoryHeavenlyTreeExtendable> allHeavenlyTrees3 = DomainManager.Extra.GetAllHeavenlyTrees();
				foreach (SectStoryHeavenlyTreeExtendable item3 in allHeavenlyTrees3)
				{
					if (item3.Location.IsValid() && !ItemTemplateHelper.CheckIsHeavenlyNormalTreeSeeds(12, item3.TemplateId) && !item3.MetInDream && item3.GrowPoint >= 900)
					{
						monthlyEventCollection.AddSectMainStoryWudangMeetingImmortal(taiwuCharId, taiwu.GetValidLocation(), item3.Id, item3.Location);
					}
				}
				if (DomainManager.Character.TryGetFixedCharacterByTemplateId(543, out var character2))
				{
					AddSectMainStoryWudangGiftsReceived(character2);
				}
				if (taiwuLocation.IsValid() && argBox.GetInt("ConchShip_PresetKey_GiveTaoistTreasureCount") < 3 && argBox.GetInt("ConchShip_PresetKey_GiveTaoistTreasureCount") > argBox.GetInt("ConchShip_PresetKey_WudangChatEventTriggeredCount"))
				{
					monthlyEventCollection.AddSectMainStoryWudangChat();
				}
			}
			TriggerTreeMonthlyNotification();
		}
		int extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(32);
		if (extraTaskChainCurrentTask < 0)
		{
			int arg = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_SectMainStoryWudangProsperousEndDate", ref arg) && _currDate >= arg + 1)
			{
				monthlyEventCollection.AddSectMainStoryWudangProsperous();
				return;
			}
			arg = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_SectMainStoryWudangFailingEndDate", ref arg) && _currDate >= arg + 1)
			{
				monthlyEventCollection.AddSectMainStoryWudangFailing();
			}
			return;
		}
		TriggerTreeMonthlyNotification();
		switch (extraTaskChainCurrentTask)
		{
		case 150:
		{
			int arg2 = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_CombatWithTaoistMonkDate", ref arg2) && _currDate >= arg2 + 3)
			{
				monthlyEventCollection.AddSectMainStoryWudangRequest(taiwu.GetId(), taiwu.GetLocation());
			}
			break;
		}
		case 241:
			if (taiwuLocation.IsValid() && argBox.GetInt("ConchShip_PresetKey_GiveTaoistTreasureCount") < 3 && argBox.GetInt("ConchShip_PresetKey_GiveTaoistTreasureCount") > argBox.GetInt("ConchShip_PresetKey_WudangChatEventTriggeredCount"))
			{
				monthlyEventCollection.AddSectMainStoryWudangChat();
			}
			break;
		case 242:
		{
			if (DomainManager.Character.TryGetFixedCharacterByTemplateId(543, out var character3) && !AddSectMainStoryWudangGiftsReceived(character3))
			{
				monthlyEventCollection.AddSectMainStoryWudangGiftsReceived2(taiwuCharId, taiwuLocation);
			}
			break;
		}
		}
		bool AddSectMainStoryWudangGiftsReceived(GameData.Domains.Character.Character character4)
		{
			short num3 = argBox.GetShort("ConchShip_PresetKey_LastEventSloppyTaoistMonkFavor");
			short favorability = DomainManager.Character.GetFavorability(character4.GetId(), taiwuCharId);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			if (favorabilityType > num3)
			{
				monthlyEventCollection.AddSectMainStoryWudangGiftsReceived(taiwuCharId, taiwuLocation);
				return true;
			}
			return false;
		}
		void TriggerTreeMonthlyNotification()
		{
			if (!triggerTreeMonthlyNotification)
			{
				List<SectStoryHeavenlyTreeExtendable> allHeavenlyTrees4 = DomainManager.Extra.GetAllHeavenlyTrees();
				if (allHeavenlyTrees4 != null)
				{
					foreach (SectStoryHeavenlyTreeExtendable item4 in allHeavenlyTrees4)
					{
						short heavenlyTreeTemplateIdByGrowValue = DomainManager.Extra.GetHeavenlyTreeTemplateIdByGrowValue(item4.GrowPoint);
						if (heavenlyTreeTemplateIdByGrowValue == 677)
						{
							GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(item4.Id);
							if (ItemTemplateHelper.CheckIsHeavenlyNormalTreeSeeds(12, item4.TemplateId))
							{
								DomainManager.World.GetMonthlyNotificationCollection().AddNormalTreesGrow(element_Objects3.GetLocation());
							}
							else
							{
								DomainManager.World.GetMonthlyNotificationCollection().AddSectMainStoryWudangTreesGrow(element_Objects3.GetLocation());
							}
						}
					}
				}
				triggerTreeMonthlyNotification = true;
			}
		}
	}

	private int XiangshuMinionsProtectWudangHeavenlyTree(DataContext context, SectStoryHeavenlyTreeExtendable tree, bool normalTree)
	{
		MonthlyEventCollection monthlyEventCollection = GetMonthlyEventCollection();
		Location location = tree.Location;
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(location.AreaId);
		List<MapTemplateEnemyInfo> list = new List<MapTemplateEnemyInfo>();
		List<GameData.Domains.Character.Character> list2 = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		List<Location> list3 = ObjectPool<List<Location>>.Instance.Get();
		List<MapTemplateEnemyInfo> list4 = new List<MapTemplateEnemyInfo>();
		bool flag = false;
		list.Clear();
		Span<MapBlockData> span = areaBlocks;
		for (int i = 0; i < span.Length; i++)
		{
			MapBlockData mapBlockData = span[i];
			if (mapBlockData.TemplateEnemyList == null)
			{
				continue;
			}
			Location location2 = mapBlockData.GetLocation();
			list4.Clear();
			foreach (MapTemplateEnemyInfo templateEnemy in mapBlockData.TemplateEnemyList)
			{
				if (templateEnemy.SourceAdventureBlockId == location.BlockId)
				{
					list4.Add(templateEnemy);
				}
			}
			if (list4.Count == 0)
			{
				continue;
			}
			list2.Clear();
			if (mapBlockData.CharacterSet != null)
			{
				foreach (int item in mapBlockData.CharacterSet)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					OrganizationInfo organizationInfo = element_Objects.GetOrganizationInfo();
					if (organizationInfo.OrgTemplateId == 16 && organizationInfo.Grade != 8 && DomainManager.Taiwu.TryGetElement_VillagerWork(item, out var value) && value.AreaId == location2.AreaId && value.BlockId == location2.BlockId)
					{
						list2.Add(element_Objects);
					}
				}
			}
			if (list2.Count > 0)
			{
				int num = 0;
				foreach (GameData.Domains.Character.Character item2 in list2)
				{
					if (list4.Count <= 0)
					{
						break;
					}
					int index = context.Random.Next(list4.Count);
					list4.RemoveAt(index);
					item2.ChangeHealth(context, -60);
					num++;
					if (item2.GetHealth() > 0)
					{
						continue;
					}
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					if (DomainManager.Extra.IsCharacterDying(item2.GetId()))
					{
						DomainManager.Character.MakeCharacterDead(context, item2, 15);
						lifeRecordCollection.AddSectMainStoryWudangVillagerKilled(item2.GetId(), DomainManager.World.GetCurrDate(), item2.GetLocation());
						MonthlyNotificationCollection monthlyNotificationCollection = GetMonthlyNotificationCollection();
						if (!normalTree)
						{
							monthlyNotificationCollection.AddSectMainStoryWudangVillagerCasualty(item2.GetId(), item2.GetLocation());
						}
						else
						{
							monthlyNotificationCollection.AddNormalVillagerCasualty(item2.GetId(), item2.GetLocation());
						}
					}
					else
					{
						DomainManager.Extra.AddDyingCharacters(context, item2.GetId(), 15);
						lifeRecordCollection.AddSectMainStoryWudangInjured(item2.GetId(), DomainManager.World.GetCurrDate(), item2.GetLocation());
					}
				}
				if (num > 0)
				{
					MonthlyNotificationCollection monthlyNotificationCollection2 = GetMonthlyNotificationCollection();
					if (!normalTree)
					{
						monthlyNotificationCollection2.AddSectMainStoryWudangVillagersInjured(num);
					}
					else
					{
						monthlyNotificationCollection2.AddNormalVillagersInjured(num);
					}
				}
			}
			list.AddRange(list4);
		}
		foreach (MapTemplateEnemyInfo item3 in list)
		{
			if (item3.SourceAdventureBlockId != location.BlockId || Config.Character.Instance[item3.TemplateId].OrganizationInfo.OrgTemplateId != 19)
			{
				continue;
			}
			Location location3 = new Location(location.AreaId, item3.BlockId);
			MapDomain.GetPathInAreaWithoutCost(location3, location, list3);
			Location location4 = ((list3.Count > 2) ? list3[1] : location);
			Events.RaiseTemplateEnemyLocationChanged(context, item3, location3, location4);
			if (location4 != location)
			{
				continue;
			}
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location location5 = taiwu.GetLocation();
			if (location5 == location4)
			{
				MapBlockData block = DomainManager.Map.GetBlock(location5);
				if (block.TemplateEnemyList == null || block.TemplateEnemyList.Count == 0)
				{
					continue;
				}
				bool flag2 = false;
				foreach (MapTemplateEnemyInfo templateEnemy2 in block.TemplateEnemyList)
				{
					CharacterItem characterItem = Config.Character.Instance[templateEnemy2.TemplateId];
					if (characterItem.OrganizationInfo.OrgTemplateId != 19 || templateEnemy2.SourceAdventureBlockId != location.BlockId)
					{
						continue;
					}
					if (!normalTree)
					{
						monthlyEventCollection.AddSectMainStoryWudangGuardHeavenlyTree(taiwu.GetId(), location5);
					}
					else
					{
						monthlyEventCollection.AddNormalGuardHeavenlyTree(taiwu.GetId(), location5);
					}
					flag2 = true;
					break;
				}
				if (!flag2)
				{
					continue;
				}
				break;
			}
			if (!normalTree)
			{
				monthlyEventCollection.AddSectMainStoryWudangHeavenlyTreeDestroyed(location);
			}
			else
			{
				monthlyEventCollection.AddNormalHeavenlyTreeDestroyed(location);
			}
			flag = true;
			break;
		}
		ObjectPool<List<Location>>.Instance.Return(list3);
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list2);
		if (!flag && !normalTree)
		{
			short num2 = (short)(298 + GetXiangshuLevel());
			if (num2 > 306)
			{
				num2 = 306;
			}
			List<MapBlockData> mapBlockList = context.AdvanceMonthRelatedData.Blocks.Occupy();
			DomainManager.Map.GetLocationByDistance(location, 3, 3, ref mapBlockList);
			MapBlockData random = mapBlockList.GetRandom(context.Random);
			context.AdvanceMonthRelatedData.Blocks.Release(ref mapBlockList);
			random.AddTemplateEnemy(new MapTemplateEnemyInfo(num2, random.BlockId, location.BlockId));
			return 1;
		}
		if (!flag && normalTree)
		{
			List<MapBlockData> list5 = ObjectPool<List<MapBlockData>>.Instance.Get();
			DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, list5, 3);
			MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, location.BlockId);
			list5.Add(blockData);
			bool flag3 = false;
			foreach (MapBlockData item4 in list5)
			{
				if (item4.TemplateEnemyList == null || item4.TemplateEnemyList.Count <= 0)
				{
					continue;
				}
				foreach (MapTemplateEnemyInfo templateEnemy3 in item4.TemplateEnemyList)
				{
					if (templateEnemy3.SourceAdventureBlockId == location.BlockId)
					{
						ObjectPool<List<MapBlockData>>.Instance.Return(list5);
						return 1;
					}
				}
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(list5);
		}
		return 0;
	}

	private void AdvanceMonth_SectMainStory_Shixiang(DataContext context)
	{
		MonthlyEventCollection monthlyEventCollection = GetMonthlyEventCollection();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location taiwuLocation = taiwu.GetLocation();
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(6);
		int extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(34);
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
		Location location = settlementByOrgTemplateId.GetLocation();
		if (GetSectMainStoryTaskStatus(6) == 0)
		{
			int arg = int.MaxValue;
			if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryShixiangProsperousEndDate", ref arg) && _currDate >= arg + 1)
			{
				monthlyEventCollection.AddSectMainStoryShixiangProsperous(taiwuLocation);
			}
			else if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryShixiangFailingEndDate", ref arg) && _currDate >= arg + 1)
			{
				monthlyEventCollection.AddSectMainStoryShixiangFailing(taiwuLocation);
			}
		}
		if ((uint)(extraTaskChainCurrentTask - 171) <= 5u)
		{
			AddInteractWithShixiangMemberEvent();
		}
		switch (extraTaskChainCurrentTask)
		{
		case 169:
		case 170:
		{
			int arg3 = 0;
			if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_ShixiangAdventureAppearDate", ref arg3) && _currDate < arg3)
			{
				break;
			}
			AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
			foreach (var (num3, adventureSiteData2) in adventuresInArea.AdventureSites)
			{
				if (adventureSiteData2.TemplateId == 176)
				{
					return;
				}
			}
			List<short> obj = context.AdvanceMonthRelatedData.BlockIds.Occupy();
			DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, obj);
			short random = obj.GetRandom(context.Random);
			context.AdvanceMonthRelatedData.BlockIds.Release(ref obj);
			ShixiangStoryAdventureTriggerAction action = new ShixiangStoryAdventureTriggerAction
			{
				Location = new Location(location.AreaId, random)
			};
			DomainManager.TaiwuEvent.AddTempDynamicAction(context, action);
			break;
		}
		case 171:
		{
			int arg4 = int.MaxValue;
			if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_ShixiangFirstLetterDate", ref arg4) && _currDate >= arg4 + 4)
			{
				monthlyEventCollection.AddSectMainStoryShixiangNotLetter(taiwuLocation);
			}
			else
			{
				monthlyEventCollection.AddSectMainStoryShixiangLetterFrom(taiwuLocation);
			}
			break;
		}
		case 175:
			monthlyEventCollection.AddSectMainStoryShixiangEnemyAttack();
			break;
		case 176:
			if (ShixiangSettlementAffiliatedBlocksHasEnemy(context, 608))
			{
				int arg2 = 0;
				if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_StartFightShixiangTraitorsDate", ref arg2) && _currDate >= arg2 + 36)
				{
					monthlyEventCollection.AddSectMainStoryShixiangEnemyAttack2();
				}
			}
			break;
		case 177:
		{
			sbyte b = sectMainStoryEventArgBox.GetSbyte("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount2");
			int num = b / 10;
			if (num > 0)
			{
				monthlyEventCollection.AddSectMainStoryShixiangGoodNews();
				b -= (sbyte)(num * 10);
				sectMainStoryEventArgBox.Set("ConchShip_PresetKey_ShixiangKillBarbarianMasterCount2", b);
			}
			sbyte b2 = sectMainStoryEventArgBox.GetSbyte("ConchShip_PresetKey_TaiwuKillBarbarianMasterCount2");
			num = b2 / 10;
			if (num > 0)
			{
				monthlyEventCollection.AddSectMainStoryShixiangGoodNews2();
				b2 -= (sbyte)(num * 10);
				sectMainStoryEventArgBox.Set("ConchShip_PresetKey_TaiwuKillBarbarianMasterCount2", b2);
			}
			EnemyClear();
			break;
		}
		case 234:
			EnemyClear();
			break;
		case 174:
			monthlyEventCollection.AddSectMainStoryShixiangEnemyAttack3();
			break;
		}
		void AddInteractWithShixiangMemberEvent()
		{
			if (taiwuLocation.IsValid())
			{
				IRandomSource random2 = context.Random;
				Span<sbyte> span = stackalloc sbyte[3] { 0, 1, 2 };
				MapBlockData block = DomainManager.Map.GetBlock(taiwuLocation);
				if (block.CharacterSet != null)
				{
					CharacterMatcherItem interactWithShixiangMemberEventTarget = CharacterMatcher.DefValue.InteractWithShixiangMemberEventTarget;
					foreach (int item2 in block.CharacterSet)
					{
						GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
						if (interactWithShixiangMemberEventTarget.Match(element_Objects) && !context.Random.CheckPercentProb(30))
						{
							CollectionUtils.Shuffle(random2, span, span.Length);
							Span<sbyte> span2 = span;
							for (int i = 0; i < span2.Length; i++)
							{
								sbyte b3 = span2[i];
								if (1 == 0)
								{
								}
								bool flag = b3 switch
								{
									0 => TryAddChallengeEvent(element_Objects), 
									1 => TryAddRequestBookEvent(element_Objects), 
									2 => TryAddLearnSkillEvent(element_Objects), 
									_ => false, 
								};
								if (1 == 0)
								{
								}
								if (flag)
								{
									break;
								}
							}
						}
					}
				}
			}
		}
		void EnemyClear()
		{
			if (!ShixiangSettlementAffiliatedBlocksHasEnemy(context, 613))
			{
				DomainManager.Extra.TriggerExtraTask(context, 34, 178);
				monthlyEventCollection.AddSectMainStoryShixiangLetterFrom2(taiwu.GetId());
				DomainManager.Extra.RemoveArgToSectMainStoryEventArgBox<bool>(context, 6, "ConchShip_PresetKey_ShixiangToFightEnemy");
			}
		}
		bool TryAddChallengeEvent(GameData.Domains.Character.Character shixiangMember)
		{
			monthlyEventCollection.AddSectMainStoryShixiangDuel(shixiangMember.GetId(), taiwuLocation, taiwu.GetId());
			return true;
		}
		bool TryAddLearnSkillEvent(GameData.Domains.Character.Character shixiangMember)
		{
			List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = taiwu.GetLearnedLifeSkills();
			List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills2 = shixiangMember.GetLearnedLifeSkills();
			Inventory inventory = shixiangMember.GetInventory();
			List<ItemKey> obj2 = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
			foreach (var (item, num5) in inventory.Items)
			{
				if (item.ItemType == 10)
				{
					SkillBookItem skillBookItem = Config.SkillBook.Instance[item.TemplateId];
					if (skillBookItem.ItemSubType == 1000)
					{
						sbyte lifeSkillType = skillBookItem.LifeSkillType;
						if ((uint)lifeSkillType <= 3u)
						{
							int num6 = shixiangMember.FindLearnedLifeSkillIndex(skillBookItem.LifeSkillTemplateId);
							if (num6 < 0 || !learnedLifeSkills2[num6].IsAllPagesRead())
							{
								int num7 = taiwu.FindLearnedLifeSkillIndex(skillBookItem.LifeSkillTemplateId);
								if (num7 >= 0 && learnedLifeSkills[num7].IsAllPagesRead())
								{
									obj2.Add(item);
								}
							}
						}
					}
				}
			}
			ItemKey randomOrDefault = obj2.GetRandomOrDefault(context.Random, ItemKey.Invalid);
			context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj2);
			if (!randomOrDefault.IsValid())
			{
				return false;
			}
			monthlyEventCollection.AddSectMainStoryShixiangRequestLifeSkill(shixiangMember.GetId(), taiwuLocation, (ulong)randomOrDefault, taiwu.GetId());
			return true;
		}
		bool TryAddRequestBookEvent(GameData.Domains.Character.Character shixiangMember)
		{
			Inventory inventory = taiwu.GetInventory();
			List<ItemKey> obj2 = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
			foreach (var (item, num5) in inventory.Items)
			{
				if (item.ItemType == 10)
				{
					SkillBookItem skillBookItem = Config.SkillBook.Instance[item.TemplateId];
					if (skillBookItem.ItemSubType == 1000)
					{
						sbyte lifeSkillType = skillBookItem.LifeSkillType;
						if ((uint)lifeSkillType <= 3u)
						{
							int num6 = shixiangMember.FindLearnedLifeSkillIndex(skillBookItem.LifeSkillTemplateId);
							if (num6 < 0)
							{
								obj2.Add(item);
							}
						}
					}
				}
			}
			ItemKey randomOrDefault = obj2.GetRandomOrDefault(context.Random, ItemKey.Invalid);
			context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj2);
			if (!randomOrDefault.IsValid())
			{
				return false;
			}
			monthlyEventCollection.AddSectMainStoryShixiangRequestBook(shixiangMember.GetId(), taiwuLocation, (ulong)randomOrDefault, taiwu.GetId());
			return true;
		}
	}

	private void AdvanceMonth_SectMainStory_Emei(DataContext context)
	{
		if (GetSectMainStoryTaskStatus(2) != 0)
		{
			return;
		}
		EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
		int extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(37);
		MonthlyEventCollection monthlyEventCollection = GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = GetMonthlyNotificationCollection();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int arg = int.MaxValue;
		if (argBox.Get("ConchShip_PresetKey_SectMainStoryEmeiProsperousEndDate", ref arg) && _currDate >= arg + 1)
		{
			monthlyEventCollection.AddSectMainStoryEmeiProsperous();
		}
		else if (argBox.Get("ConchShip_PresetKey_SectMainStoryEmeiFailingEndDate", ref arg) && _currDate >= arg + 1)
		{
			monthlyEventCollection.AddSectMainStoryEmeiFailing();
		}
		switch (extraTaskChainCurrentTask)
		{
		case 200:
		{
			int arg3 = int.MaxValue;
			Location location = DomainManager.Organization.GetSettlementByOrgTemplateId(2).GetLocation();
			short arg4 = -1;
			argBox.Get("ConchShip_PresetKey_WhiteApeBlockId", ref arg4);
			if ((argBox.Get("ConchShip_PresetKey_HomocideCase0Time", ref arg3) && arg3 + 1 <= _currDate) || (argBox.Get("ConchShip_PresetKey_HomocideCase1Time", ref arg3) && arg3 + 1 <= _currDate))
			{
				DomainManager.Extra.GenerateEmeiBlood(context, new Location(location.AreaId, arg4));
			}
			break;
		}
		case 258:
		{
			int arg6 = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_FirstClickWhiteGibbonDate", ref arg6) && !argBox.Contains<int>("ConchShip_PresetKey_SecondClickWhiteGibbonDate") && arg6 + 1 <= _currDate)
			{
				EmeiWhiteGibbonAppear();
				monthlyNotificationCollection.AddSectMainStoryWhiteGibbonReturns();
			}
			if (argBox.Get("ConchShip_PresetKey_SecondClickWhiteGibbonDate", ref arg6) && !argBox.Contains<int>("ConchShip_PresetKey_ThirdClickWhiteGibbonDate") && arg6 + 2 <= _currDate)
			{
				EmeiWhiteGibbonAppear(isQuestionMark: true);
				monthlyNotificationCollection.AddSectMainStoryWhiteGibbonReturns();
			}
			break;
		}
		case 203:
		{
			int arg5 = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_ThirdClickWhiteGibbonDate", ref arg5) && !argBox.Contains<int>("ConchShip_PresetKey_FourthClickWhiteGibbonDate") && arg5 + 3 <= _currDate)
			{
				EmeiWhiteGibbonAppear();
				monthlyNotificationCollection.AddSectMainStoryWhiteGibbonReturns();
			}
			break;
		}
		case 259:
		{
			int arg7 = int.MaxValue;
			if ((argBox.Get("ConchShip_PresetKey_FourthClickWhiteGibbonDate", ref arg7) && !argBox.Contains<int>("ConchShip_PresetKey_FifthClickWhiteGibbonDate") && arg7 + 1 <= _currDate) || (argBox.Get("ConchShip_PresetKey_FifthClickWhiteGibbonDate", ref arg7) && !argBox.Contains<int>("ConchShip_PresetKey_SixthClickWhiteGibbonDate") && arg7 + 2 <= _currDate))
			{
				ShiHoujiuAppear();
				monthlyNotificationCollection.AddSectMainStoryEmeiShiReturns();
			}
			break;
		}
		case 204:
		{
			int arg2 = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_EmeiAdventureTwoAppearDate", ref arg2) && _currDate >= arg2 + 6 && !argBox.Contains<bool>("ConchShip_PresetKey_EmeiEnterAdventureTwo"))
			{
				HandleMissEmeiAdventureTwo();
			}
			break;
		}
		}
		void CharacterAppear(GameData.Domains.Character.Character character)
		{
			short arg8 = -1;
			argBox.Get("ConchShip_PresetKey_WhiteApeBlockId", ref arg8);
			Location location2 = new Location(DomainManager.Organization.GetSettlementByOrgTemplateId(2).GetLocation().AreaId, arg8);
			Events.RaiseFixedCharacterLocationChanged(context, character.GetId(), character.GetLocation(), location2);
			character.SetLocation(location2, context);
		}
		void EmeiWhiteGibbonAppear(bool isQuestionMark = false)
		{
			GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, (short)(isQuestionMark ? 636 : 679));
			CharacterAppear(orCreateFixedCharacterByTemplateId);
			DomainManager.Extra.TriggerExtraTask(context, 37, 202);
		}
		void HandleMissEmeiAdventureTwo()
		{
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 2, "ConchShip_PresetKey_SectMainStoryEmeiFailingEndDate", _currDate);
			DomainManager.Extra.FinishAllTaskInChain(context, 37);
			ItemKey inventoryItemKey = taiwu.GetInventory().GetInventoryItemKey(12, 267);
			if (inventoryItemKey.IsValid())
			{
				taiwu.RemoveInventoryItem(context, inventoryItemKey, 1, deleteItem: true);
			}
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 2, "ConchShip_PresetKey_EmeiOptionWhoIsOrthodoxVisible", value: false);
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 2, "ConchShip_PresetKey_EmeiOptionReclusiveElderVisible", value: false);
			DomainManager.Extra.RemoveArgToSectMainStoryEventArgBox<int>(context, 2, "ConchShip_PresetKey_EmeiKillEachOtherStage");
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 2, "ConchShip_PresetKey_EmeiPassAdventureTwoTaiwuId", taiwuCharId);
		}
		void ShiHoujiuAppear()
		{
			GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 637);
			CharacterAppear(orCreateFixedCharacterByTemplateId);
			DomainManager.Extra.TriggerExtraTask(context, 37, 255);
		}
	}

	private void AdvanceMonth_SectMainStory_Jingang(DataContext context)
	{
		if (GetSectMainStoryTaskStatus(11) != 0)
		{
			return;
		}
		int extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(35);
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location validLocation = taiwu.GetValidLocation();
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(11);
		int arg = int.MaxValue;
		if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangMonkMurderedTriggeredDate", ref arg) && _currDate >= arg + 2)
		{
			monthlyNotificationCollection.AddSectMainStoryJingangWrongdoing();
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 11, "ConchShip_PresetKey_JingangMonkMurderedTriggeredDate", _currDate);
		}
		int num = extraTaskChainCurrentTask;
		int num2 = num;
		if (num2 >= 0)
		{
			switch (num2)
			{
			case 182:
				if (taiwu.GetFeatureIds().Contains(483))
				{
					monthlyNotificationCollection.AddSectMainStoryJingangHaunted(taiwu.GetId());
				}
				break;
			case 183:
				monthlyNotificationCollection.AddSectMainStoryJingangFollowedByGhost(taiwu.GetId());
				break;
			case 286:
				monthlyEventCollection.AddSectMainStoryJingangHearsay();
				break;
			case 187:
			{
				int num3 = JingangSpreadSecInfoStage();
				int num4 = SectMainStoryRelatedConstants.JingangTaiwuSpreadSecInfoCount[num3];
				int num5 = SectMainStoryRelatedConstants.JingangSecInfoSpreadSpeed[num3];
				int num6 = SectMainStoryRelatedConstants.JingangSecInfoOpenCount[num3];
				int arg5 = 0;
				sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangSecInfoMetaDataId", ref arg5);
				if (JingangKnowSecInfoCount() >= num4)
				{
					sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_JingangKnowSecInfoIdList", out IntList arg6);
					arg6.Items.Add(taiwu.GetId());
					CollectionUtils.Shuffle(context.Random, arg6.Items);
					GameData.Domains.Character.Character element = null;
					for (int i = 0; i < arg6.Items.Count && !DomainManager.Character.TryGetElement_Objects(arg6.Items[i], out element); i++)
					{
					}
					MapBlockData[] array = DomainManager.Map.GetAreaBlocks(element.GetValidLocation().AreaId).ToArray();
					CollectionUtils.Shuffle(context.Random, array);
					int num7 = 0;
					for (int j = 0; j < array.Length; j++)
					{
						HashSet<int> characterSet = array[j].CharacterSet;
						if (characterSet != null && characterSet.Count > 0)
						{
							foreach (int item in array[j].CharacterSet)
							{
								if (!arg6.Items.Contains(item))
								{
									JingangDistributeSecInfo(context, arg5, item);
									JingangAddKnowSecInfoCharId(context, item);
									DomainManager.World.GetInstantNotifications().AddKnowMonkSecret(item);
									num7++;
									if (num7 >= num5)
									{
										break;
									}
								}
							}
						}
						if (num7 >= num5)
						{
							break;
						}
					}
				}
				if (JingangKnowSecInfoCount() >= num6 - 1 || DomainManager.Information.IsSecretInformationInBroadcast(arg5))
				{
					monthlyEventCollection.AddSectMainStoryJingangVisitorsArrive();
					JingangBroadCastSecInfo(context);
				}
				break;
			}
			case 290:
				monthlyEventCollection.AddSectMainStoryJingangSutraSecrets();
				break;
			case 287:
				monthlyEventCollection.AddSectMainStoryJingangSutraDisappears(taiwu.GetId());
				break;
			case 190:
			case 192:
			{
				bool arg2 = false;
				sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangDefeatShmashanaAdhipati", ref arg2);
				if (arg2)
				{
					bool arg3 = false;
					sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangSoulTransformOpen", ref arg3);
					if (!arg3)
					{
						monthlyEventCollection.AddSectMainStoryJingangReincarnation(taiwu.GetId());
						monthlyNotificationCollection.AddSectMainStoryJingangRockFleshed();
						GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 747);
						Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
						Events.RaiseFixedCharacterLocationChanged(context, orCreateFixedCharacterByTemplateId.GetId(), orCreateFixedCharacterByTemplateId.GetLocation(), taiwuVillageLocation);
						orCreateFixedCharacterByTemplateId.SetLocation(taiwuVillageLocation, context);
						DomainManager.Character.DirectlySetFavorabilities(context, orCreateFixedCharacterByTemplateId.GetId(), taiwu.GetId(), 30000, 30000);
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 11, "ConchShip_PresetKey_JingangWesternBuddhistMonkPassLegacyTaiwuId", taiwu.GetId());
					}
				}
				else
				{
					bool arg4 = false;
					sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangMonkGhostVanishesTriggered", ref arg4);
					if (!arg4)
					{
						monthlyEventCollection.AddSectMainStoryJingangGhostVanishes();
					}
				}
				break;
			}
			case 188:
				if (sectMainStoryEventArgBox.Contains<bool>("ConchShip_PresetKey_JingangTriggerMonthlyEventVillagerSuffer"))
				{
					monthlyEventCollection.AddSectMainStoryJingangResidentsSufferingContinues();
				}
				break;
			}
			if (JingangIsInSpreadSutraTask())
			{
				if (JingangCanTriggerMonkSoulEnterDream(context))
				{
					monthlyEventCollection.AddSectMainStoryJingangRitualsInDream();
				}
				int arg7 = int.MaxValue;
				if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangAttackDate", ref arg7) && _currDate >= arg7 + SectMainStoryRelatedConstants.JingangEventFrequency1)
				{
					monthlyEventCollection.AddSectMainStoryJingangAttack();
					sectMainStoryEventArgBox.Set("ConchShip_PresetKey_JingangAttackDate", _currDate);
				}
				if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangFamousFakeMonkDate", ref arg7) && _currDate >= arg7 + SectMainStoryRelatedConstants.JingangEventFrequency1)
				{
					monthlyNotificationCollection.AddSectMainStoryJingangFamousFakeMonk();
					sectMainStoryEventArgBox.Set("ConchShip_PresetKey_JingangFamousFakeMonkDate", _currDate);
				}
				if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangPrayDate", ref arg7) && _currDate >= arg7 + SectMainStoryRelatedConstants.JingangEventFrequency1)
				{
					monthlyNotificationCollection.AddSectMainStoryJingangPray();
					sectMainStoryEventArgBox.Set("ConchShip_PresetKey_JingangPrayDate", _currDate);
				}
				bool arg8 = false;
				if (!sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangSelectHelpWestMonk", ref arg8) && sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangLettersFromJingangDate", ref arg7) && _currDate >= arg7 + SectMainStoryRelatedConstants.JingangEventFrequency2)
				{
					GameData.Domains.Character.Character character = settlementByOrgTemplateId?.GetAvailableHighMember(8, 0, needAdult: false);
					if (character != null)
					{
						monthlyEventCollection.AddSectMainStoryJingangLettersFromJingang(character.GetId());
						sectMainStoryEventArgBox.Set("ConchShip_PresetKey_JingangLettersFromJingangDate", _currDate);
					}
				}
				bool arg9 = false;
				if (!sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangSecInfoSpreadingSelectBetray", ref arg9) && sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangFameDistributionDate", ref arg7) && _currDate >= arg7 + SectMainStoryRelatedConstants.JingangEventFrequency1)
				{
					short spiritualDebtLowestAreaIdByAreaId = DomainManager.Map.GetSpiritualDebtLowestAreaIdByAreaId(validLocation.AreaId);
					DomainManager.Extra.ChangeAreaSpiritualDebt(context, spiritualDebtLowestAreaIdByAreaId, 200);
					taiwu.ChangeResource(context, 7, 4000);
					InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
					instantNotificationCollection.AddResourceIncreased(taiwu.GetId(), 7, 4000);
					monthlyNotificationCollection.AddSectMainStoryJingangFameDistribution(taiwu.GetId(), validLocation);
					sectMainStoryEventArgBox.Set("ConchShip_PresetKey_JingangFameDistributionDate", _currDate);
				}
				if (JingangCanTriggerPietyEvent())
				{
					monthlyEventCollection.AddSectMainStoryJingangPiety(taiwu.GetId());
				}
			}
			switch (extraTaskChainCurrentTask)
			{
			case 186:
			{
				int arg10 = 0;
				sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangPersuadeVillagerCount", ref arg10);
				if (arg10 >= 1 && !sectMainStoryEventArgBox.Contains<bool>("ConchShip_PresetKey_JingangMonthlyEventVillagerEscapeTriggered"))
				{
					monthlyNotificationCollection.AddSectMainStoryJingangVillagerFlee(taiwu.GetId());
				}
				break;
			}
			case 181:
			case 188:
				if (!sectMainStoryEventArgBox.Contains<bool>("ConchShip_PresetKey_JingangMonthlyEventVillagerEscapeTriggered") && !sectMainStoryEventArgBox.Contains<bool>("ConchShip_PresetKey_JingangTriggerMonthlyEventVillagerSuffer"))
				{
					monthlyNotificationCollection.AddSectMainStoryJingangVillagerFlee(taiwu.GetId());
				}
				break;
			}
			int num8 = extraTaskChainCurrentTask;
			int num9 = num8;
			if (num9 == 188 || num9 == 192)
			{
				int arg11 = int.MaxValue;
				if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryJingangProsperousEndDate", ref arg11) && _currDate >= arg11 + 1)
				{
					monthlyEventCollection.AddSectMainStoryJingangProsperous();
				}
				else if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryJingangFailingEndDate", ref arg11) && _currDate >= arg11 + 1)
				{
					monthlyEventCollection.AddSectMainStoryJingangFailing();
				}
			}
		}
		else if (JingangMonkWasRobbedCanTrigger() && !sectMainStoryEventArgBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus))
		{
			monthlyEventCollection.AddSectMainStoryJingangMonkMurdered();
		}
	}

	private void AdvanceMonth_SectMainStory_Wuxian(DataContext context)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		int extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(36);
		if (TryGetPrologueAddedWug(out var wugAdded) && wugAdded > -1)
		{
			if (IsWuxianTaiwuChanged() && IsWuxianPrologueWugAttackedOnce() && extraTaskChainCurrentTask == 276)
			{
				monthlyEventCollection.AddSectMainStoryWuxianMiaoWoman(location);
			}
			else
			{
				monthlyEventCollection.AddSectMainStoryWuxianPoisonousWug(taiwuCharId);
			}
		}
		else if (GetWuxianChapterOneWishCount() > GetWuxianChapterOneWishComeTrueCount() && GetWuxianChapterOneWishComeTrueCount() < 2)
		{
			monthlyEventCollection.AddSectMainStoryWuxianStrangeThings();
		}
		else if (IsAbleToTriggerWuxianChapterThreeMail())
		{
			monthlyEventCollection.AddSectMainStoryWuxianGiftsReceived(taiwuCharId, location);
		}
		else
		{
			EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
			int arg = -1;
			int currDate = GetCurrDate();
			if (extraTaskChainCurrentTask >= 0 && extraTaskChainCurrentTask != 284 && sectMainStoryEventArgBox.Get("ConchShip_PresetKey_Wuxian_Chapter4_HappyEndingEventDate", ref arg) && currDate >= arg)
			{
				monthlyEventCollection.AddSectMainStoryWuxianStrangeThings();
			}
			switch (extraTaskChainCurrentTask)
			{
			case 279:
				if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryWuxianFailingEndDate", ref arg) && currDate >= arg && !IsWuxianEndingEventTriggered())
				{
					monthlyEventCollection.AddSectMainStoryWuxianFailing0();
				}
				break;
			case 199:
			case 285:
				if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryWuxianFailingEndDate", ref arg) && currDate >= arg && !IsWuxianEndingEventTriggered())
				{
					monthlyEventCollection.AddSectMainStoryWuxianFailing1();
				}
				break;
			case 198:
				if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryWuxianProsperousEndDate", ref arg) && currDate >= arg && !IsWuxianEndingEventTriggered())
				{
					monthlyEventCollection.AddSectMainStoryWuxianProsperous();
				}
				break;
			}
		}
		UpdateWuxianParanoiaCharacters(context);
	}

	private void AdvanceMonth_SectMainStory_Ranshan(DataContext context)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		Location validLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(7);
		if (DomainManager.Extra.IsExtraTaskChainInProgress(49))
		{
			int currDate = GetCurrDate();
			int arg = currDate;
			sectMainStoryEventArgBox.Get("ConchShip_PresetKey_Ranshan_Chapter2_TeachStartDate", ref arg);
			sectMainStoryEventArgBox.Set("ConchShip_PresetKey_SanZongBiWuCountDown", 24 - currDate + arg);
			DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 7);
		}
		if (IsRanshanSectMainStoryAbleToTrigger())
		{
			if (!sectMainStoryEventArgBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus))
			{
				monthlyEventCollection.AddSectMainStoryRanshanDragonGate();
			}
		}
		else if (IsRanshanChapter1MonthlyEvent2AbleToTrigger())
		{
			monthlyEventCollection.AddSectMainStoryRanshanMessage(taiwuCharId);
		}
		else if (IsRanshanChapter1MonthlyEvent3AbleToTrigger())
		{
			monthlyEventCollection.AddSectMainStoryRanshanAfterQinglang(taiwuCharId, validLocation);
		}
		else if (IsRanshanChapter2HuajuAbleToTrigger())
		{
			monthlyEventCollection.AddSectMainStoryRanshanPaperCraneFromYufuFaction(taiwuCharId, validLocation);
		}
		else if (IsRanshanChapter2XuanzhiAbleToTrigger())
		{
			monthlyEventCollection.AddSectMainStoryRanshanPaperCraneFromShenjianFaction(taiwuCharId, validLocation);
		}
		else if (IsRanshanChapter2YingjiaoAbleToTrigger())
		{
			monthlyEventCollection.AddSectMainStoryRanshanPaperCraneFromYinyangFaction(taiwuCharId, validLocation);
		}
		else if (IsRanshanChapter2MonthlyEventAbleToTrigger())
		{
			monthlyEventCollection.AddSectMainStoryRanshanSanshiLeave(taiwuCharId);
		}
		else if (DomainManager.Extra.IsExtraTaskInProgress(323))
		{
			DomainManager.Extra.TriggerExtraTask(context, 40, 297);
			GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 625);
			Location location = DomainManager.Organization.GetSettlementByOrgTemplateId(7).GetLocation();
			Events.RaiseFixedCharacterLocationChanged(context, orCreateFixedCharacterByTemplateId.GetId(), orCreateFixedCharacterByTemplateId.GetLocation(), location);
			orCreateFixedCharacterByTemplateId.SetLocation(location, context);
		}
		else
		{
			switch (DomainManager.Extra.GetExtraTaskChainCurrentTask(40))
			{
			case 299:
				if (DomainManager.Extra.IsRanshanMenteeGoodStoryEnding())
				{
					monthlyEventCollection.AddSectMainStoryRanshanProsperous();
					ConvertRanshanFootman(context, isGoodEnd: true);
				}
				else
				{
					monthlyEventCollection.AddSectMainStoryRanshanFailing();
					ConvertRanshanFootman(context, isGoodEnd: false);
				}
				break;
			case 300:
				monthlyEventCollection.AddSectMainStoryRanshanFailing();
				ConvertRanshanFootman(context, isGoodEnd: false);
				break;
			}
		}
		UpdateRanshanThreeCorpsesAction(context);
	}

	private void AdvanceMonth_SectMainStory_Baihua(DataContext context)
	{
		if (DomainManager.World.GetMainStoryLineProgress() < 22)
		{
			return;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location taiwuLocation = taiwu.GetLocation();
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		short areaIdByAreaTemplateId = DomainManager.Map.GetAreaIdByAreaTemplateId(18);
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		bool flag = argBox.Contains<bool>("ConchShip_PresetKey_BaihuaEndenmicTriggered");
		if (taiwuLocation.AreaId == areaIdByAreaTemplateId && !flag && !argBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus))
		{
			monthlyEventCollection.AddSectMainStoryBaihuaEndenmic();
		}
		sbyte sectMainStoryTaskStatus = GetSectMainStoryTaskStatus(3);
		if (sectMainStoryTaskStatus == 1 && argBox.Contains<int>("ConchShip_PresetKey_BaihuaLMTransferAnimalDate"))
		{
			int arg = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_BaihuaLMTransferAnimalDate", ref arg) && arg + GlobalConfig.Instance.BaihuaLifeLinkRemoveCharacterCooldown <= _currDate)
			{
				GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 781);
				GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId2 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 786);
				GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId3 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 808);
				GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId4 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 809);
				Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(3);
				Location location = settlementByOrgTemplateId.GetLocation();
				Events.RaiseFixedCharacterLocationChanged(context, orCreateFixedCharacterByTemplateId.GetId(), orCreateFixedCharacterByTemplateId.GetLocation(), location);
				orCreateFixedCharacterByTemplateId.SetLocation(location, context);
				Events.RaiseFixedCharacterLocationChanged(context, orCreateFixedCharacterByTemplateId2.GetId(), orCreateFixedCharacterByTemplateId2.GetLocation(), location);
				orCreateFixedCharacterByTemplateId2.SetLocation(location, context);
				Events.RaiseFixedCharacterLocationChanged(context, orCreateFixedCharacterByTemplateId3.GetId(), orCreateFixedCharacterByTemplateId3.GetLocation(), Location.Invalid);
				orCreateFixedCharacterByTemplateId3.SetLocation(Location.Invalid, context);
				Events.RaiseFixedCharacterLocationChanged(context, orCreateFixedCharacterByTemplateId4.GetId(), orCreateFixedCharacterByTemplateId4.GetLocation(), Location.Invalid);
				orCreateFixedCharacterByTemplateId4.SetLocation(Location.Invalid, context);
				DomainManager.Extra.RemoveArgToSectMainStoryEventArgBox<int>(context, 3, "ConchShip_PresetKey_BaihuaLMTransferAnimalDate");
				InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
				instantNotificationCollection.AddSectStoryBaihuaToHuman();
			}
		}
		if (sectMainStoryTaskStatus != 0)
		{
			return;
		}
		int arg2 = int.MaxValue;
		if (argBox.Get("ConchShip_PresetKey_SectMainStoryBaihuaFailingEndDate", ref arg2) && _currDate >= arg2 + 1)
		{
			monthlyEventCollection.AddSectMainStoryBaihuaFailing();
			return;
		}
		if (argBox.Get("ConchShip_PresetKey_SectMainStoryBaihuaProsperousEndDate", ref arg2) && _currDate >= arg2 + 1)
		{
			monthlyEventCollection.AddSectMainStoryBaihuaProsperous();
			return;
		}
		int extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(48);
		bool flag2 = DomainManager.Extra.IsExtraTaskChainInProgress(50);
		bool flag3 = DomainManager.Extra.IsExtraTaskChainInProgress(51);
		bool tryTriggerBaihuaCombatTaskChain = false;
		bool tryTriggerBaihuaManicLow = false;
		bool tryTriggerBaihuaManicHigh = false;
		bool tryTriggerLeukoMelanoPlay = false;
		bool tryTriggerLMPlay = false;
		switch (extraTaskChainCurrentTask)
		{
		case 304:
			if (!argBox.Contains<bool>("ConchShip_PresetKey_BaihuaDreamAboutPastFirstTriggered"))
			{
				monthlyEventCollection.AddSectMainStoryBaihuaDreamAboutPastFirst(taiwu.GetId());
			}
			break;
		case 305:
			if (!argBox.Contains<bool>("ConchShip_PresetKey_BaihuaMelanpsycheArrivedEventTriggered"))
			{
				monthlyEventCollection.AddSectMainStoryBaihuaMelanoArrived(taiwu.GetId());
			}
			break;
		case 302:
		{
			int arg4 = 0;
			if (argBox.Get("ConchShip_PresetKey_BaihuaAdventureFourAppearDate", ref arg4) && _currDate >= arg4)
			{
				if (DomainManager.Adventure.QueryAdventureLocation(185).IsValid())
				{
					return;
				}
				DomainManager.TaiwuEvent.AddTempDynamicAction(context, new BaihuaStoryAdventureFourTriggerAction());
			}
			break;
		}
		case 325:
			TryTriggerBaihuaManicLow();
			TryTriggerBaihuaManicHigh(triggerTask: true);
			TryTriggedLeukoMelanoPlay();
			TryTriggedLMPlay();
			break;
		case 326:
		{
			TryTriggerBaihuaManicHigh(triggerTask: false);
			int arg5 = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_BaihuaManicHighDate", ref arg5) && _currDate >= arg5 + 3)
			{
				monthlyEventCollection.AddSectMainStoryBaihuaAnonymReturns();
			}
			break;
		}
		case 327:
			TryTriggerBaihuaManicHigh(triggerTask: false);
			break;
		case 328:
		{
			int arg3 = int.MaxValue;
			if (argBox.Get("ConchShip_PresetKey_BaihuaTriggerFinaleTaskDate", ref arg3) && _currDate >= arg3 + 1)
			{
				monthlyEventCollection.AddSectMainStoryBaihuaGifts(taiwu.GetLocation());
			}
			break;
		}
		}
		if (flag2)
		{
			if (DomainManager.Extra.IsExtraTaskInProgress(307))
			{
				TryTriggerBaihuaCombatTaskChain();
				int arg6 = 0;
				if (!argBox.Get("ConchShip_PresetKey_BaihuaDreamAboutPastLastDate", ref arg6) || (argBox.Get("ConchShip_PresetKey_BaihuaDreamAboutPastLastDate", ref arg6) && _currDate < arg6 + 3))
				{
					return;
				}
				if (!argBox.Contains<bool>("ConchShip_PresetKey_BaihuaDreamAboutPastLastTriggered"))
				{
					monthlyEventCollection.AddSectMainStoryBaihuaDreamAboutPastLast(taiwu.GetId());
				}
			}
			if (DomainManager.Extra.IsExtraTaskInProgress(308))
			{
				TryTriggerBaihuaCombatTaskChain();
				if (context.Random.CheckPercentProb(50))
				{
					bool arg7 = false;
					argBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementIdLock", ref arg7);
					short arg8 = -1;
					argBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", ref arg8);
					Settlement settlement = DomainManager.Organization.GetSettlement(arg8);
					if (!arg7)
					{
						List<short> list = ObjectPool<List<short>>.Instance.Get();
						DomainManager.Map.GetAreaSettlementIds(settlement.GetLocation().AreaId, list, containsMainCity: true, containsSect: true);
						short random = list.GetRandom(context.Random);
						ObjectPool<List<short>>.Instance.Return(list);
						if (random != arg8)
						{
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", random);
							settlement = DomainManager.Organization.GetSettlement(random);
							CallBaihuaMember(context, isLeuko: true);
						}
					}
					else
					{
						CallBaihuaMember(context, isLeuko: true);
					}
					monthlyNotificationCollection.AddSectMainStoryBaihuaLeukoKills(settlement.GetLocation());
				}
			}
			if (DomainManager.Extra.IsExtraTaskInProgress(310))
			{
				TryTriggerBaihuaCombatTaskChain();
				if (context.Random.CheckPercentProb(50))
				{
					bool arg9 = false;
					argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementIdLock", ref arg9);
					short arg10 = -1;
					argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", ref arg10);
					Settlement settlement2 = DomainManager.Organization.GetSettlement(arg10);
					if (!arg9)
					{
						List<short> list2 = ObjectPool<List<short>>.Instance.Get();
						DomainManager.Map.GetAreaSettlementIds(settlement2.GetLocation().AreaId, list2, containsMainCity: true, containsSect: true);
						short random2 = list2.GetRandom(context.Random);
						ObjectPool<List<short>>.Instance.Return(list2);
						if (random2 != arg10)
						{
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", random2);
							settlement2 = DomainManager.Organization.GetSettlement(random2);
							CallBaihuaMember(context, isLeuko: false);
						}
					}
					else
					{
						CallBaihuaMember(context, isLeuko: false);
					}
					monthlyNotificationCollection.AddSectMainStoryBaihuaMelanoKills(settlement2.GetLocation());
				}
			}
			int groupId;
			if (DomainManager.Extra.IsExtraTaskInProgress(309))
			{
				short arg11 = -1;
				argBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", ref arg11);
				if (DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwu.GetLocation(), arg11) && BaihuaGroupMeetCount(isLeuko: true, out groupId) >= 1 && context.Random.CheckPercentProb(GetAmbushProb(isisLeuko: true)))
				{
					monthlyEventCollection.AddSectMainStoryBaihuaAmbushLeuko();
				}
			}
			if (DomainManager.Extra.IsExtraTaskInProgress(311))
			{
				short arg12 = -1;
				argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", ref arg12);
				if (DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwu.GetLocation(), arg12) && BaihuaGroupMeetCount(isLeuko: false, out groupId) >= 1 && context.Random.CheckPercentProb(GetAmbushProb(isisLeuko: false)))
				{
					monthlyEventCollection.AddSectMainStoryBaihuaAmbushMelano();
				}
			}
		}
		if (flag3)
		{
			if (DomainManager.Extra.IsExtraTaskInProgress(313))
			{
				int arg13 = int.MaxValue;
				if (argBox.Get("ConchShip_PresetKey_BaihuaAnimalsBackDate", ref arg13) && _currDate < arg13 + 6)
				{
					TryTriggerPandemicStartTask();
				}
				TryTriggerBaihuaManicLow();
				TryTriggerBaihuaManicHigh(triggerTask: true);
				TryTriggedLeukoMelanoPlay();
				TryTriggedLMPlay();
				if (!argBox.Contains<bool>("ConchShip_PresetKey_BaihuaLeukoAssistedMelano"))
				{
					GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId5 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 808);
					if (FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(orCreateFixedCharacterByTemplateId5.GetId(), taiwu.GetId())) >= 5)
					{
						monthlyNotificationCollection.AddSectMainStoryBaihuaLeukoHelps();
						DomainManager.Extra.TriggerExtraTask(context, 51, 316);
						DomainManager.Extra.FinishTriggeredExtraTask(context, 51, 315);
					}
				}
				if (!argBox.Contains<bool>("ConchShip_PresetKey_BaihuaMelanoAssistedLeuko"))
				{
					GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId6 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 809);
					if (FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(orCreateFixedCharacterByTemplateId6.GetId(), taiwu.GetId())) >= 5)
					{
						monthlyNotificationCollection.AddSectMainStoryBaihuaMelanoHelps();
						DomainManager.Extra.TriggerExtraTask(context, 51, 324);
						DomainManager.Extra.FinishTriggeredExtraTask(context, 51, 318);
					}
				}
			}
			if (DomainManager.Extra.IsExtraTaskInProgress(316) && _currDate % 2 == 1 && DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwu.GetLocation(), taiwuVillageSettlementId))
			{
				monthlyEventCollection.AddSectMainStoryBaihuaLeukoAssistsMelano();
			}
			if (DomainManager.Extra.IsExtraTaskInProgress(324) && _currDate % 2 == 0 && DomainManager.Map.IsLocationInSettlementInfluenceRange(taiwu.GetLocation(), taiwuVillageSettlementId))
			{
				monthlyEventCollection.AddSectMainStoryBaihuaMelanoAssistsLeuko();
			}
		}
		UpdateBaihuaManicCharacters(context);
		int GetAmbushProb(bool isisLeuko)
		{
			int arg14 = int.MaxValue;
			if (isisLeuko)
			{
				argBox.Get("ConchShipEventArgBoxKey_PresetKey_BaihuaLeukoKillsOptionSelectDate", ref arg14);
			}
			else
			{
				argBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsOptionSelectDate", ref arg14);
			}
			return 40 + (_currDate - arg14) * 10;
		}
		void TryTriggedLMPlay()
		{
			if (!tryTriggerLMPlay)
			{
				tryTriggerLMPlay = true;
				if (taiwuLocation.AreaId == taiwuVillageLocation.AreaId && argBox.Contains<bool>("ConchShip_PresetKey_BaihuaLeukoAssistedMelano") && argBox.Contains<bool>("ConchShip_PresetKey_BaihuaMelanoAssistedLeuko"))
				{
					int arg14 = -1;
					argBox.Get("ConchShip_PresetKey_BaihuaLMPlayCount", ref arg14);
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaLMPlayCount", ++arg14);
					if (arg14 >= 2)
					{
						monthlyEventCollection.AddSectMainStoryBaihuaLeukoMelanoPlay();
					}
				}
			}
		}
		void TryTriggedLeukoMelanoPlay()
		{
			if (!tryTriggerLeukoMelanoPlay)
			{
				tryTriggerLeukoMelanoPlay = true;
				if (taiwuLocation.AreaId == taiwuVillageLocation.AreaId)
				{
					bool flag4 = false;
					bool flag5 = false;
					int arg14 = -1;
					argBox.Get("ConchShip_PresetKey_BaihuaLeukoPlayCount", ref arg14);
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaLeukoPlayCount", ++arg14);
					if (arg14 > 2)
					{
						flag4 = true;
					}
					arg14 = -1;
					argBox.Get("ConchShip_PresetKey_BaihuaMelanoPlayCount", ref arg14);
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaMelanoPlayCount", ++arg14);
					if (arg14 > 2)
					{
						flag5 = true;
					}
					if (flag4 && flag5)
					{
						if (context.Random.Next(0, 2) == 0)
						{
							monthlyEventCollection.AddSectMainStoryBaihuaLeukoPlay();
						}
						else
						{
							monthlyEventCollection.AddSectMainStoryBaihuaMelanoPlay();
						}
					}
					else if (flag4)
					{
						monthlyEventCollection.AddSectMainStoryBaihuaLeukoPlay();
					}
					else if (flag5)
					{
						monthlyEventCollection.AddSectMainStoryBaihuaMelanoPlay();
					}
				}
			}
		}
		void TryTriggerBaihuaCombatTaskChain()
		{
			if (!tryTriggerBaihuaCombatTaskChain)
			{
				tryTriggerBaihuaCombatTaskChain = true;
				if (argBox.Contains<bool>("ConchShip_PresetKey_BaihuaDreamAboutPastLastTriggered"))
				{
					if (_currDate % 2 == 0)
					{
						if (!argBox.Contains<bool>("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventTriggered"))
						{
							short num = BaihuaSelectSettlementIdNeighborTaiwuVillage(context);
							Settlement settlement3 = DomainManager.Organization.GetSettlement(num);
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", num);
							DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaLeukoKillsInteractOpen", value: true);
							monthlyEventCollection.AddSectMainStoryBaihuaLeukoKills(settlement3.GetLocation());
						}
					}
					else if (!argBox.Contains<bool>("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventTriggered"))
					{
						short num2 = BaihuaSelectSettlementIdNeighborTaiwuVillage(context);
						Settlement settlement4 = DomainManager.Organization.GetSettlement(num2);
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", num2);
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaMelanoKillsInteractOpen", value: true);
						monthlyEventCollection.AddSectMainStoryBaihuaMelanoKills(settlement4.GetLocation());
					}
				}
			}
		}
		void TryTriggerBaihuaManicHigh(bool triggerTask)
		{
			if (!tryTriggerBaihuaManicHigh)
			{
				tryTriggerBaihuaManicHigh = true;
				int arg14 = int.MaxValue;
				if (argBox.Get("ConchShip_PresetKey_BaihuaManicLowDate", ref arg14) && _currDate >= arg14 + 3)
				{
					short areaId = taiwuVillageLocation.AreaId;
					List<short> list3 = ObjectPool<List<short>>.Instance.Get();
					DomainManager.Map.GetAreaSettlementIds(areaId, list3, containsMainCity: true, containsSect: true);
					short random3 = list3.GetRandom(context.Random);
					CharacterSet groupCharIds = DomainManager.Taiwu.GetGroupCharIds();
					Settlement settlement3 = DomainManager.Organization.GetSettlement(random3);
					monthlyNotificationCollection.AddSectMainStoryBaihuaManicHigh(settlement3.GetLocation());
					List<int> list4 = ObjectPool<List<int>>.Instance.Get();
					settlement3.GetMembers().GetAllMembers(list4);
					CollectionUtils.Shuffle(context.Random, list4);
					for (int num = list4.Count - 1; num >= 0; num--)
					{
						int num2 = list4[num];
						if (DomainManager.Character.TryGetElement_Objects(num2, out var element) && (groupCharIds.Contains(num2) || element.GetAgeGroup() < 2))
						{
							list4.RemoveAt(num);
						}
					}
					int val = context.Random.Next(3, 7);
					for (int i = 0; i < Math.Min(val, list4.Count); i++)
					{
						int num3 = list4[i];
						if (DomainManager.Character.TryGetElement_Objects(num3, out var element2))
						{
							element2.AddFeature(context, 542);
							lifeRecordCollection.AddSectMainStoryBaihuaManiaHigh(num3, _currDate, element2.GetLocation());
							BaihuaAddCharIdToSpecialDebuffIntList(context, num3);
						}
					}
					if (!argBox.Contains<int>("ConchShip_PresetKey_BaihuaManicHighDate"))
					{
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaManicHighDate", _currDate);
					}
					ObjectPool<List<int>>.Instance.Return(list4);
					DomainManager.Extra.FinishAllTaskInChain(context, 51);
					if (triggerTask)
					{
						DomainManager.Extra.TriggerExtraTask(context, 48, 326);
					}
				}
			}
		}
		void TryTriggerBaihuaManicLow()
		{
			if (!tryTriggerBaihuaManicLow)
			{
				tryTriggerBaihuaManicLow = true;
				int arg14 = int.MaxValue;
				if (argBox.Get("ConchShip_PresetKey_BaihuaAnimalsBackDate", ref arg14) && _currDate >= arg14 + 6 && (!argBox.Contains<int>("ConchShip_PresetKey_BaihuaManicLowDate") || (argBox.Get("ConchShip_PresetKey_BaihuaManicLowDate", ref arg14) && _currDate <= arg14 + 6)))
				{
					List<short> list3 = BaihuaSelectSettlementIds(context, avoidGuangnan: false, avoidTaiwuVillageArea: true, 3);
					List<int> list4 = ObjectPool<List<int>>.Instance.Get();
					CharacterSet groupCharIds = DomainManager.Taiwu.GetGroupCharIds();
					for (int i = 0; i < list3.Count; i++)
					{
						short settlementId = list3[i];
						Settlement settlement3 = DomainManager.Organization.GetSettlement(settlementId);
						monthlyNotificationCollection.AddSectMainStoryBaihuaManicLow(settlement3.GetLocation());
						list4.Clear();
						settlement3.GetMembers().GetAllMembers(list4);
						CollectionUtils.Shuffle(context.Random, list4);
						for (int num = list4.Count - 1; num >= 0; num--)
						{
							int num2 = list4[num];
							if (DomainManager.Character.TryGetElement_Objects(num2, out var element) && (groupCharIds.Contains(num2) || element.GetAgeGroup() < 2))
							{
								list4.RemoveAt(num);
							}
						}
						int val = context.Random.Next(3, 7);
						for (int j = 0; j < Math.Min(val, list4.Count); j++)
						{
							int num3 = list4[j];
							if (DomainManager.Character.TryGetElement_Objects(num3, out var element2))
							{
								element2.AddFeature(context, 541);
								lifeRecordCollection.AddSectMainStoryBaihuaManiaLow(num3, _currDate, element2.GetLocation());
								BaihuaAddCharIdToSpecialDebuffIntList(context, num3);
							}
						}
					}
					ObjectPool<List<int>>.Instance.Return(list4);
					TryTriggerPandemicStartTask();
					if (!argBox.Contains<int>("ConchShip_PresetKey_BaihuaManicLowDate"))
					{
						DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaManicLowDate", _currDate);
					}
				}
			}
		}
		void TryTriggerPandemicStartTask()
		{
			if (argBox.Contains<bool>("ConchShip_PresetKey_BaihuaLeukoAssistedMelano") && argBox.Contains<bool>("ConchShip_PresetKey_BaihuaMelanoAssistedLeuko"))
			{
				DomainManager.Extra.TriggerExtraTask(context, 48, 325);
			}
		}
	}

	private void AdvanceMonth_SectMainStory_Zhujian(DataContext context)
	{
		if (GetSectMainStoryTaskStatus(9) != 0)
		{
			return;
		}
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(9);
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (CheckSectMainStoryAvailable(9) && DomainManager.Organization.GetSettlementByOrgTemplateId(9).CalcApprovingRate() >= 500 && !sectMainStoryEventArgBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus) && ZhujianMainStoryTrigger1())
		{
			monthlyEventCollection.AddSectMainStoryZhujianHeir(taiwu.GetId(), taiwu.GetLocation());
			return;
		}
		ExtraDomain extra = DomainManager.Extra;
		if (extra.IsExtraTaskInProgress(350))
		{
			Location location = taiwu.GetLocation();
			if (location.IsValid())
			{
				MapBlockData block = DomainManager.Map.GetBlock(location);
				if (block.GetConfig().TemplateId == 27)
				{
					monthlyEventCollection.AddSectMainStoryZhujianHazyRain(taiwu.GetId());
					return;
				}
			}
		}
		if (extra.IsExtraTaskInProgress(351) && InTriggerLocation(out var location2))
		{
			monthlyEventCollection.AddSectMainStoryZhujianTongshengSpeaks(taiwu.GetId(), location2);
			return;
		}
		if (extra.IsExtraTaskInProgress(358) && InTriggerLocation(out var location3))
		{
			monthlyEventCollection.AddSectMainStoryZhujianHuichuntang(taiwu.GetId(), location3);
			return;
		}
		int arg = int.MaxValue;
		if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryZhujianProsperousEndDate", ref arg) && _currDate >= arg + 1 && InTriggerLocation(out var _))
		{
			monthlyEventCollection.AddSectMainStoryZhujianProsperous();
			return;
		}
		arg = int.MaxValue;
		if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryZhujianFailingEndDate", ref arg) && _currDate >= arg + 1 && InTriggerLocation(out var _))
		{
			monthlyEventCollection.AddSectMainStoryZhujianFailing();
		}
		bool InTriggerLocation(out Location reference)
		{
			reference = Location.Invalid;
			if (DomainManager.Map.IsTraveling)
			{
				reference = taiwu.GetValidLocation();
				return true;
			}
			Location location6 = taiwu.GetLocation();
			if (location6.IsValid() && !MapAreaData.IsBrokenArea(location6.AreaId))
			{
				reference = location6;
				return true;
			}
			return false;
		}
	}

	private void AdvanceMonth_SectMainStoryFulong(DataContext context)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(14);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		short id = DomainManager.Organization.GetSettlementByOrgTemplateId(14).GetId();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		int arg = int.MaxValue;
		if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryFulongProsperousEndDate", ref arg) && _currDate >= arg + 1)
		{
			monthlyEventCollection.AddSectMainStoryFulongProsperous();
			return;
		}
		arg = int.MaxValue;
		if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_SectMainStoryFulongFailingEndDate", ref arg) && _currDate >= arg + 1)
		{
			monthlyEventCollection.AddSectMainStoryFulongFailing();
			return;
		}
		bool arg2 = false;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongDisasterStart", ref arg2);
		if ((!sectMainStoryEventArgBox.Contains<bool>("ConchShip_PresetKey_FulongDisasterStart") && !sectMainStoryEventArgBox.Contains<int>(SectMainStoryEventArgKeys.TriggeringStatus) && FulongDisasterStart()) || arg2)
		{
			int arg3 = 30;
			if (sectMainStoryEventArgBox.Contains<int>("ConchShip_PresetKey_FulongDisasterStartProb"))
			{
				sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongDisasterStartProb", ref arg3);
			}
			bool flag = location.AreaId == DomainManager.Map.GetAreaIdByAreaTemplateId(29);
			arg3 = ((!flag) ? (arg3 + 15) : (arg3 + 30));
			if (context.Random.CheckPercentProb(arg3))
			{
				arg3 -= 45;
				FulongTriggerDisaster(context);
				monthlyNotificationCollection.AddSectMainStoryFulongSacrifice();
				if (flag)
				{
					monthlyEventCollection.AddSectMainStoryFulongDiasterAppear();
				}
			}
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 14, "ConchShip_PresetKey_FulongDisasterStartProb", arg3);
		}
		int extraTaskChainCurrentTask = DomainManager.Extra.GetExtraTaskChainCurrentTask(52);
		int num = extraTaskChainCurrentTask;
		int num2 = num;
		if ((uint)(num2 - 333) <= 4u)
		{
			int arg4 = int.MaxValue;
			if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongMessengerAppearTime", ref arg4) && _currDate >= arg4 + SectMainStoryRelatedConstants.FulongZealotStartRobTime)
			{
				DomainManager.Extra.ApplyFulongOutLawAdvanceMonth(context, arg4);
			}
		}
		switch (extraTaskChainCurrentTask)
		{
		case 330:
		{
			int arg10 = 0;
			sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongAdventureOneCountDown", ref arg10);
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 14, "ConchShip_PresetKey_FulongAdventureOneCountDown", --arg10);
			break;
		}
		case 333:
		{
			Location validLocation = taiwu.GetValidLocation();
			if (DomainManager.Map.IsLocationInSettlementInfluenceRange(validLocation, id))
			{
				monthlyEventCollection.AddSectMainStoryFulongShadow();
			}
			break;
		}
		case 343:
		case 346:
		{
			DomainManager.Extra.ApplyFulongInFlameAreaAdvanceMonth(context);
			int arg11 = int.MaxValue;
			if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongFireStartTime", ref arg11) && _currDate >= arg11 + GlobalConfig.Instance.FulongFlameExtinguishTime)
			{
				monthlyNotificationCollection.AddSectMainStoryFulongFireVanishes();
				List<FulongInFlameArea> allFulongInFlameAreas = DomainManager.Extra.GetAllFulongInFlameAreas();
				int count = allFulongInFlameAreas.Count;
				for (int i = 0; i < count; i++)
				{
					DomainManager.Extra.ApplyFulongInFlameAreaFullyExtinguished(context, 0, triggerEvent: false);
				}
			}
			else
			{
				bool arg12 = false;
				sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongPutOutFire", ref arg12);
				if (arg12 && DomainManager.Extra.GetAllFulongInFlameAreas().Count > 0)
				{
					monthlyEventCollection.AddSectMainStoryFulongFireFighting(taiwu.GetId());
				}
			}
			break;
		}
		case 331:
		{
			int arg7 = int.MaxValue;
			sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongFireStartTime", ref arg7);
			if (_currDate > arg7 && (_currDate - arg7) % 3 == 0)
			{
				monthlyEventCollection.AddSectMainStoryFulongAftermath();
			}
			break;
		}
		case 344:
		{
			int arg8 = 3;
			sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongLazuliFindFlowerDialogLevel", ref arg8);
			if (arg8 < 6)
			{
				break;
			}
			int arg9 = int.MaxValue;
			if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongStayWithLazuliTaskTriggerDate", ref arg9) && _currDate >= arg9 + 3)
			{
				Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
				MapBlockData blockData = DomainManager.Map.GetBlockData(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId);
				MapBlockData blockData2 = DomainManager.Map.GetBlockData(taiwu.GetValidLocation().AreaId, taiwu.GetValidLocation().BlockId);
				ByteCoordinate blockPos = blockData2.GetBlockPos();
				if (blockData.GetManhattanDistanceToPos(blockPos.X, blockPos.Y) <= 3)
				{
					DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 14, "ConchShip_PresetKey_FulongTravelWithLazuliFinished", value: true);
				}
			}
			break;
		}
		case 336:
			break;
		case 338:
		{
			int arg6 = -1;
			sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongAdventureTwoTaiwuId", ref arg6);
			if (arg6 != taiwu.GetId())
			{
				monthlyEventCollection.AddSectMainStoryFulongLazuliLetter();
			}
			GameData.Utilities.ShortList value = sectMainStoryEventArgBox.Get<GameData.Utilities.ShortList>("ConchShip_PresetKey_FulongChickenFeatherDropList");
			List<short> items = value.Items;
			if (items == null || items.Count <= 0)
			{
				break;
			}
			foreach (short item in value.Items)
			{
				monthlyNotificationCollection.AddSectMainStoryFulongFeatherDrop(item);
			}
			value.Items.Clear();
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 14, "ConchShip_PresetKey_FulongChickenFeatherDropList", value);
			break;
		}
		case 340:
		{
			int arg5 = 0;
			sectMainStoryEventArgBox.Get("ConchShip_PresetKey_FulongAdventureThreeCountDown", ref arg5);
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 14, "ConchShip_PresetKey_FulongAdventureThreeCountDown", --arg5);
			break;
		}
		case 332:
		case 334:
		case 335:
		case 337:
		case 339:
		case 341:
		case 342:
		case 345:
			break;
		}
	}

	private void UpdateWuxianParanoiaCharacters(DataContext context)
	{
		List<GameData.Domains.Character.Character> list = new List<GameData.Domains.Character.Character>();
		List<int> list2 = new List<int>();
		MapCharacterFilter.ParallelFind(ShouldAttackRandomTarget, list, 0, 135);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		if (list.Count < 7 && GetSectMainStoryTaskStatus(12) == 2 && context.Random.CheckPercentProb(25))
		{
			Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(12);
			List<int> obj = context.AdvanceMonthRelatedData.CharIdList.Occupy();
			settlementByOrgTemplateId.GetMembers().GetAllMembers(obj);
			int num = int.MinValue;
			foreach (int item in obj)
			{
				if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetHappinessType() != 6 && !element.GetFeatureIds().Contains(486))
				{
					short lifeSkillAttainment = element.GetLifeSkillAttainment(9);
					if (lifeSkillAttainment > num)
					{
						num = lifeSkillAttainment;
						list2.Clear();
						list2.Add(item);
					}
					else if (lifeSkillAttainment == num)
					{
						list2.Add(item);
					}
				}
			}
			context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
			if (list2.Count > 0)
			{
				int random = list2.GetRandom(context.Random);
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(random);
				element_Objects.AddFeature(context, 486);
				lifeRecordCollection.AddWuxianParanoiaAdded(random, currDate, element_Objects.GetLocation());
				monthlyNotificationCollection.AddSectMainStoryWuxianParanoiaAppeared(random);
			}
		}
		foreach (GameData.Domains.Character.Character item2 in list)
		{
			int id = item2.GetId();
			Location location2 = item2.GetLocation();
			if (!DomainManager.Character.IsCharacterAlive(id))
			{
				continue;
			}
			if (item2.GetHappinessType() == 6)
			{
				item2.RemoveFeature(context, 486);
				lifeRecordCollection.AddWuxianParanoiaErased(id, currDate, location2);
			}
			else
			{
				if (!location2.IsValid() || !item2.IsInteractableAsIntelligentCharacter() || context.Random.NextBool())
				{
					continue;
				}
				if (location2 == location)
				{
					lifeRecordCollection.AddWuxianParanoiaAttack(id, currDate, taiwu.GetId(), location2);
					monthlyEventCollection.AddSectMainStoryWuxianAssault(id, location2);
					DomainManager.Character.HandleAttackAction(context, item2, taiwu);
					continue;
				}
				item2.GetPotentialHarmfulActionTargets(list2);
				if (list2.Count != 0)
				{
					int random2 = list2.GetRandom(context.Random);
					GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(random2);
					lifeRecordCollection.AddWuxianParanoiaAttack(id, currDate, random2, location2);
					DomainManager.Character.HandleAttackAction(context, item2, element_Objects2);
				}
			}
		}
		static bool ShouldAttackRandomTarget(GameData.Domains.Character.Character character)
		{
			return character.GetFeatureIds().Contains(486);
		}
	}

	private void UpdateBaihuaManicCharacters(DataContext context)
	{
		List<int> items = DomainManager.World.BaihuaGetSpecialDebuffIntList().Items;
		if (items == null || items.Count <= 0)
		{
			return;
		}
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		foreach (int item in items)
		{
			if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				continue;
			}
			Location location2 = element.GetLocation();
			if (!location2.IsValid() || !element.IsInteractableAsIntelligentCharacter() || DomainManager.Character.IsNotManicCharacter(element) || context.Random.NextBool())
			{
				continue;
			}
			if (location2 == location)
			{
				lifeRecordCollection.AddSectMainStoryBaihuaManiaAttack(item, currDate, taiwu.GetId(), location2);
				monthlyEventCollection.AddSectMainStoryBaihuaManicAttack(item, location2);
				DomainManager.Character.HandleAttackAction(context, element, taiwu);
				continue;
			}
			element.GetPotentialHarmfulActionTargets(list);
			if (list.Count != 0)
			{
				int random = list.GetRandom(context.Random);
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(random);
				lifeRecordCollection.AddSectMainStoryBaihuaManiaAttack(item, currDate, random, location2);
				DomainManager.Character.HandleAttackAction(context, element, element_Objects);
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	private bool AreaHasAdultGraveOfTargetOrganization(short areaId, sbyte orgTemplateId)
	{
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		Span<MapBlockData> span = areaBlocks;
		for (int i = 0; i < span.Length; i++)
		{
			MapBlockData mapBlockData = span[i];
			if (mapBlockData.GraveSet == null || mapBlockData.GraveSet.Count <= 0)
			{
				continue;
			}
			foreach (int item in mapBlockData.GraveSet)
			{
				DomainManager.Character.TryGetElement_Graves(item, out var element);
				DeadCharacter deadCharacter = DomainManager.Character.GetDeadCharacter(element.GetId());
				if (deadCharacter.OrganizationInfo.OrgTemplateId == orgTemplateId && deadCharacter.GetActualAge() >= 16)
				{
					return true;
				}
			}
		}
		return false;
	}

	internal void ShixiangQueryEnemyLocations(DataContext context, out short areaId, out List<short> blockIds)
	{
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
		Location location = settlementByOrgTemplateId.GetLocation();
		areaId = location.AreaId;
		List<MapBlockData> obj = context.AdvanceMonthRelatedData.Blocks.Occupy();
		DomainManager.Map.QueryRegularBelongBlocks(obj, location, true);
		blockIds = new List<short>();
		foreach (MapBlockData item in obj)
		{
			HashSet<int> enemyCharacterSet = item.EnemyCharacterSet;
			if (enemyCharacterSet == null || enemyCharacterSet.Count <= 0)
			{
				continue;
			}
			foreach (int item2 in item.EnemyCharacterSet)
			{
				if (DomainManager.Character.TryGetElement_Objects(item2, out var element))
				{
					short templateId = element.GetTemplateId();
					if ((templateId >= 608 && templateId <= 617) || 1 == 0)
					{
						blockIds.Add(item.BlockId);
						break;
					}
				}
			}
		}
		context.AdvanceMonthRelatedData.Blocks.Release(ref obj);
	}

	private void FindShixiangLeader()
	{
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
		GameData.Domains.Character.Character leader = settlementByOrgTemplateId.GetLeader();
		if (leader != null)
		{
		}
	}

	public void JixiGrowUp(DataContext context, short oldCharTemplateId, short newCharTemplateId)
	{
		GameData.Domains.Character.Character character = DomainManager.Character.ReplaceFixedCharacter(context, oldCharTemplateId, newCharTemplateId);
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(15);
		bool arg = false;
		if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JixiHasAntiqueJadeFox", ref arg) && arg)
		{
			ItemKey itemKey = DomainManager.Item.CreateItem(context, 12, 244);
			character.AddInventoryItem(context, itemKey, 1);
		}
		bool arg2 = false;
		if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JixiHasAntiqueJadeBat", ref arg2) && arg2)
		{
			ItemKey itemKey2 = DomainManager.Item.CreateItem(context, 12, 243);
			character.AddInventoryItem(context, itemKey2, 1);
		}
		bool arg3 = false;
		if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JixiHasAntiqueJadeButterfly", ref arg3) && arg3)
		{
			ItemKey itemKey3 = DomainManager.Item.CreateItem(context, 12, 245);
			character.AddInventoryItem(context, itemKey3, 1);
		}
		if (newCharTemplateId == 539)
		{
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 15, "ConchShip_PresetKey_SectStory_Xuehou_Jixi_KilledCount", 14);
		}
		if (newCharTemplateId == 538)
		{
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 15, "ConchShip_PresetKey_SectStory_Xuehou_Jixi_KilledCount", 7);
		}
		if (newCharTemplateId == 537)
		{
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 15, "ConchShip_PresetKey_SectStory_Xuehou_Jixi_KilledCount", 0);
		}
	}

	public bool JixiAdventurePass(sbyte index, int overTime)
	{
		string text = string.Empty;
		switch (index)
		{
		case 1:
			text = "ConchShip_PresetKey_JixiAdventureOnePassDate";
			break;
		case 2:
			text = "ConchShip_PresetKey_JixiAdventureTwoPassDate";
			break;
		case 3:
			text = "ConchShip_PresetKey_JixiAdventureThreePassDate";
			break;
		}
		Tester.Assert(text != string.Empty);
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(15);
		int arg = int.MaxValue;
		if (sectMainStoryEventArgBox.Get(text, ref arg))
		{
			int currDate = DomainManager.World.GetCurrDate();
			return currDate >= arg + overTime;
		}
		return false;
	}

	public bool JixiAdventureDisappear(sbyte index, int overTime = 9)
	{
		string text = string.Empty;
		switch (index)
		{
		case 1:
			text = "ConchShip_PresetKey_JixiAdventureOneStartDate";
			break;
		case 2:
			text = "ConchShip_PresetKey_JixiAdventureTwoStartDate";
			break;
		case 3:
			text = "ConchShip_PresetKey_JixiAdventureThreeStartDate";
			break;
		}
		Tester.Assert(text != string.Empty);
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(15);
		int arg = int.MaxValue;
		if (sectMainStoryEventArgBox.Get(text, ref arg))
		{
			int currDate = DomainManager.World.GetCurrDate();
			return currDate >= arg + overTime;
		}
		return false;
	}

	public sbyte GetJixiFavorabilityType()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		GameData.Domains.Character.Character character = TryGetJixi();
		if (character == null)
		{
			return sbyte.MinValue;
		}
		return FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(character.GetId(), taiwu.GetId()));
	}

	public GameData.Domains.Character.Character TryGetJixi()
	{
		if (DomainManager.Character.TryGetFixedCharacterByTemplateId(539, out var character))
		{
			return character;
		}
		if (DomainManager.Character.TryGetFixedCharacterByTemplateId(537, out var character2))
		{
			return character2;
		}
		if (DomainManager.Character.TryGetFixedCharacterByTemplateId(538, out var character3))
		{
			return character3;
		}
		return null;
	}

	public void DealSectMainStoryEnd(DataContext context, sbyte orgTemplateId, sbyte endState, int time)
	{
		Tester.Assert(endState != 0);
		SetSectMainStoryTaskStatus(context, orgTemplateId, endState);
		sbyte b = orgTemplateId;
		sbyte b2 = b;
		if (b2 == 15)
		{
			switch (endState)
			{
			case 1:
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, orgTemplateId, "ConchShip_PresetKey_SectMainStoryXuehouProsperousEndDate", time);
				break;
			case 2:
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, orgTemplateId, "ConchShip_PresetKey_SectMainStoryXuehouFailingEndDate", time);
				break;
			}
			DomainManager.Extra.FinishAllTaskInChain(context, 28);
			DomainManager.Extra.FinishAllTaskInChain(context, 29);
		}
	}

	public bool IsJixiFree()
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(15);
		return DomainManager.World.GetSectMainStoryTaskStatus(15) == 1 || sectMainStoryEventArgBox.Contains<int>("ConchShip_PresetKey_SectMainStoryXuehouProsperousEndDate");
	}

	public void ShaolinGetSutraBooks(DataContext context, sbyte beginGrade, sbyte endGrade, Action<ItemKey> onGeneratedBook)
	{
		short[] skillList = Config.LifeSkillType.Instance[(sbyte)13].SkillList;
		for (sbyte b = beginGrade; b <= endGrade; b++)
		{
			short index = skillList[b];
			short skillBookId = LifeSkill.Instance[index].SkillBookId;
			ItemKey obj = DomainManager.Item.CreateSkillBook(context, skillBookId, 5, -1, -1, 50);
			SkillBookItem skillBookItem = Config.SkillBook.Instance[skillBookId];
			if (skillBookItem.Grade == 8 && DomainManager.Item.TryGetElement_SkillBooks(obj.Id, out var element))
			{
				element.SetMaxDurability(15, context);
				element.SetCurrDurability(15, context);
			}
			onGeneratedBook(obj);
		}
		DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 1, "ConchShip_PresetKey_ShaolinReadingMaxGradeSutra", skillList[endGrade]);
		DomainManager.Extra.TriggerExtraTask(context, 30, 228);
	}

	[DomainMethod]
	public bool EmeiTransferBonusProgress(DataContext context, short bonusTemplateId, List<ItemKey> itemKeys)
	{
		if (!DomainManager.Extra.TryGetElement_SectEmeiBreakBonusData(bonusTemplateId, out var value))
		{
			return false;
		}
		int progress = SectMainStorySharedMethods.CalcEmeiBonusItemProgress(bonusTemplateId, itemKeys);
		DomainManager.Taiwu.RemoveItemList(context, itemKeys, ItemSourceType.Inventory, deleteItem: true);
		value.OfflineAddProgress(progress);
		DomainManager.Extra.SectEmeiSetBonus(context, bonusTemplateId, value);
		return true;
	}

	public void GetEmeiPotentialVictims(GameData.Domains.Character.Character selfChar, out List<int> charIds)
	{
		Location location = selfChar.GetLocation();
		List<MapBlockData> list = new List<MapBlockData>();
		int id = selfChar.GetId();
		sbyte grade = selfChar.GetOrganizationInfo().Grade;
		CharacterMatcherItem emeiPotentialVictims = CharacterMatcher.DefValue.EmeiPotentialVictims;
		charIds = new List<int>();
		DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, list, 3);
		foreach (MapBlockData item in list)
		{
			HashSet<int> characterSet = item.CharacterSet;
			if (characterSet == null || characterSet.Count <= 0)
			{
				continue;
			}
			foreach (int item2 in item.CharacterSet)
			{
				if (item2 == id || !DomainManager.Character.TryGetElement_Objects(item2, out var element) || !emeiPotentialVictims.Match(element) || element.GetOrganizationInfo().Grade > grade)
				{
					continue;
				}
				charIds.Add(item2);
				break;
			}
		}
	}

	public void AddEmeiExp(DataContext context, int expFromCombat)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
		if (sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_EmeiShiHoujiuFollowOpen") || sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_EmeiWhiteGibbonFollowOpen"))
		{
			bool emeiCostExpEnough = GetEmeiCostExpEnough();
			long sectEmeiExp = DomainManager.Extra.GetSectEmeiExp();
			sectEmeiExp += (int)Math.Ceiling((double)expFromCombat / 4.0);
			DomainManager.Extra.SetSectEmeiExp(sectEmeiExp, context);
			bool emeiCostExpEnough2 = GetEmeiCostExpEnough();
			if (!emeiCostExpEnough && emeiCostExpEnough2)
			{
				int emeiExpCharacterId = GetEmeiExpCharacterId();
				InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
				instantNotificationCollection.AddDuChuangYiGeReady(emeiExpCharacterId);
			}
		}
	}

	public bool GetEmeiCostExpEnough()
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
		if (sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_EmeiBreakBonusSaved"))
		{
			return true;
		}
		return true;
	}

	public int GetEmeiExpCharacterId()
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
		bool flag = sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_EmeiShiHoujiuFollowOpen");
		bool flag2 = sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_EmeiWhiteGibbonFollowOpen");
		if (!flag && !flag2)
		{
			return -1;
		}
		short templateId = (short)(flag ? 637 : 679);
		return DomainManager.Character.GetFixedCharacterIdByTemplateId(templateId);
	}

	[DomainMethod]
	public ItemKey RefiningWugKing(DataContext context)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		SectWuxianWugJugData sectWuxianWugJugPoisons = DomainManager.Extra.GetSectWuxianWugJugPoisons();
		sbyte b = SectMainStorySharedMethods.CalcWugKingType(list, sectWuxianWugJugPoisons);
		ItemKey itemKey = ItemKey.Invalid;
		if (b < 0)
		{
			List<short> list2 = ObjectPool<List<short>>.Instance.Get();
			foreach (WugKingItem item in (IEnumerable<WugKingItem>)WugKing.Instance)
			{
				list2.Add(item.RefiningWeight);
			}
			b = (sbyte)RandomUtils.GetRandomIndex(list2, context.Random);
			ObjectPool<List<short>>.Instance.Return(list2);
		}
		if (list.Sum() > 0)
		{
			for (sbyte b2 = 0; b2 < 6; b2++)
			{
				sectWuxianWugJugPoisons.ReducePoison(b2, list[b2]);
			}
			sectWuxianWugJugPoisons.UpdateRefiningDate();
			WugKingItem wugKingItem = WugKing.Instance[b];
			itemKey = DomainManager.Item.CreateMedicine(context, wugKingItem.WugMedicine);
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			taiwu.GetInventory().OfflineAdd(itemKey, 1);
			taiwu.SetInventory(taiwu.GetInventory(), context);
		}
		DomainManager.Extra.SetSectWuxianWugJugPoisons(sectWuxianWugJugPoisons, context);
		ObjectPool<List<int>>.Instance.Return(list);
		return itemKey;
	}

	[DomainMethod]
	public bool DropPoisonsToWugJug(DataContext context, List<ItemKey> poisonMaterials)
	{
		if (poisonMaterials == null || poisonMaterials.Count <= 0)
		{
			return false;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Inventory inventory = taiwu.GetInventory();
		foreach (ItemKey poisonMaterial in poisonMaterials)
		{
			if (!inventory.Items.ContainsKey(poisonMaterial))
			{
				return false;
			}
			if (!SectMainStorySharedMethods.CalcDropPoisonValue(poisonMaterial).IsNonZero())
			{
				return false;
			}
		}
		SectWuxianWugJugData sectWuxianWugJugPoisons = DomainManager.Extra.GetSectWuxianWugJugPoisons();
		PoisonInts poisonInts = SectMainStorySharedMethods.CalcDropPoisonValue(sectWuxianWugJugPoisons, poisonMaterials);
		taiwu.RemoveInventoryItemList(context, poisonMaterials, deleteItem: true);
		for (sbyte b = 0; b < 6; b++)
		{
			sectWuxianWugJugPoisons.AddPoison(b, poisonInts[b]);
		}
		DomainManager.Extra.SetSectWuxianWugJugPoisons(sectWuxianWugJugPoisons, context);
		return true;
	}

	public bool TryGetPrologueAddedWug(out int wugAdded)
	{
		wugAdded = -1;
		return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Prologue_AddedWug", ref wugAdded);
	}

	public int GetWuxianChapterOneWishComeTrueCount()
	{
		int arg = 0;
		return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Chapter1_WishComeTrueCount", ref arg) ? arg : 0;
	}

	public int GetWuxianChapterOneWishCount()
	{
		int arg = 0;
		return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Chapter1_WuxianChapter1WishCount", ref arg) ? arg : 0;
	}

	public int GetWuxianHappyEndingEventDate()
	{
		int arg = 0;
		return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Chapter4_HappyEndingEventDate", ref arg) ? arg : 0;
	}

	public static bool IsAbleToTriggerWuxianChapterThreeMail()
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
		bool arg = false;
		int arg2 = 0;
		return sectMainStoryEventArgBox.Get("ConchShip_PresetKey_Wuxian_Chapter3_AbleToStart", ref arg) && arg && (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_Wuxian_Chapter3_MailReceivedCount", ref arg2) ? arg2 : 0) < 3;
	}

	public bool IsWuxianFinalBossBeaten()
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
		bool arg = false;
		return sectMainStoryEventArgBox.Get("ConchShip_PresetKey_Wuxian_Chapter4_FinalBossBeaten", ref arg) && arg;
	}

	public bool IsWuxianTaiwuChanged()
	{
		int arg = -1;
		return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Prologue_TaiwuId", ref arg) && arg != DomainManager.Taiwu.GetTaiwuCharId();
	}

	public bool IsWuxianPrologueWugAttackedOnce()
	{
		bool arg = false;
		return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Prologue_WugAttacked", ref arg) && arg;
	}

	public void WuxianEndingProsperous(DataContext context)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
		sectMainStoryEventArgBox.Set("ConchShip_PresetKey_SectMainStoryWuxianProsperousEndDate", DomainManager.World.GetCurrDate() + 1);
		sectMainStoryEventArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_HappyEndingEventDate", DomainManager.World.GetCurrDate() + 3);
		sectMainStoryEventArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_FinalBossBeaten", arg: true);
		sectMainStoryEventArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_AdventureComplete", arg: false);
		DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 12);
	}

	public void WuxianEndingFailing0(DataContext context)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
		sectMainStoryEventArgBox.Set("ConchShip_PresetKey_SectMainStoryWuxianFailingEndDate", DomainManager.World.GetCurrDate() + 3);
		DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 12);
	}

	public void WuxianEndingFailing1(DataContext context, bool isComplete)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(12);
		sectMainStoryEventArgBox.Set("ConchShip_PresetKey_SectMainStoryWuxianFailingEndDate", DomainManager.World.GetCurrDate() + 1);
		sectMainStoryEventArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_FinalBossBeaten", arg: false);
		sectMainStoryEventArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_AdventureComplete", isComplete);
		if (isComplete)
		{
			sectMainStoryEventArgBox.Set("ConchShip_PresetKey_Wuxian_Chapter4_HappyEndingEventDate", DomainManager.World.GetCurrDate() + 3);
		}
		DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 12);
	}

	public bool IsWuxianEndingEventTriggered()
	{
		bool arg = false;
		return DomainManager.Extra.GetSectMainStoryEventArgBox(12).Get("ConchShip_PresetKey_Wuxian_Chapter4_EndingEventTriggered", ref arg) && arg;
	}

	public bool JingangWorldStateCheck()
	{
		return DomainManager.Building.IsTaiwuVillageHaveSpecifyBuilding(50, notBuild: true);
	}

	public bool JingangMonkWasRobbedCanTrigger()
	{
		if (!JingangWorldStateCheck())
		{
			return false;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location validLocation = taiwu.GetValidLocation();
		if (DomainManager.Map.IsAreaBroken(validLocation.AreaId))
		{
			return false;
		}
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(validLocation.AreaId);
		return stateTemplateIdByAreaId == 11;
	}

	public int JingangSpreadSecInfoStage()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (DomainManager.Information.CharacterHasSecretInformationByTemplateId(taiwu.GetId(), 115))
		{
			return 3;
		}
		if (DomainManager.Information.CharacterHasSecretInformationByTemplateId(taiwu.GetId(), 114))
		{
			return 2;
		}
		if (DomainManager.Information.CharacterHasSecretInformationByTemplateId(taiwu.GetId(), 113))
		{
			return 1;
		}
		if (DomainManager.Information.CharacterHasSecretInformationByTemplateId(taiwu.GetId(), 112))
		{
			return 0;
		}
		return -1;
	}

	[DomainMethod]
	public bool JingangMonkSoulBtnShow()
	{
		if (JingangSpreadSecInfoStage() == -1)
		{
			return false;
		}
		if (!JingangIsInSpreadSutraTask())
		{
			return false;
		}
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
		bool arg = false;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangMonkSoulBtnDisappear", ref arg);
		return !arg;
	}

	[DomainMethod]
	public bool JingangSoulTransformOpen()
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
		bool arg = false;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangSoulTransformOpen", ref arg);
		return arg;
	}

	public int JingangKnowSecInfoCount()
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
		sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_JingangKnowSecInfoIdList", out IntList arg);
		if (arg.Items == null)
		{
			return 0;
		}
		return arg.Items.Count;
	}

	public bool JingangCanTriggerMonkSoulEnterDream(DataContext context)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
		int arg = 0;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangSpreadSecInfoTotalCount", ref arg);
		int arg2 = 0;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangMonkSoulEnterDreamCount", ref arg2);
		if (arg / 5 > arg2 && arg2 < 5)
		{
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 11, "ConchShip_PresetKey_JingangMonkSoulEnterDreamCount", ++arg2);
			return true;
		}
		return false;
	}

	public void JingangClearKnowSecInfo(DataContext context)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
		sectMainStoryEventArgBox.Remove<IntList>("ConchShip_PresetKey_JingangKnowSecInfoIdList");
		DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 11);
	}

	public void JingangAddKnowSecInfoCharId(DataContext context, int charId)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
		sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_JingangKnowSecInfoIdList", out IntList arg);
		if (arg.Items == null)
		{
			arg = IntList.Create();
		}
		arg.Items.Add(charId);
		DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 11, "ConchShip_PresetKey_JingangKnowSecInfoIdList", arg);
		int arg2 = 0;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangSpreadSecInfoTotalCount", ref arg2);
		DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 11, "ConchShip_PresetKey_JingangSpreadSecInfoTotalCount", ++arg2);
	}

	public bool JingangCanTriggerPietyEvent()
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
		int arg = 0;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangPietyCount", ref arg);
		return arg < JingangCanTriggerPietyEventCount();
	}

	public int JingangCanTriggerPietyEventCount()
	{
		int num = 3;
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
		bool arg = false;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangGiveVillagerFood", ref arg);
		if (arg)
		{
			num++;
		}
		bool arg2 = false;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangGiveVillagerMoney", ref arg2);
		if (arg2)
		{
			num++;
		}
		bool arg3 = false;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangGiveVillagerPromise", ref arg3);
		if (arg3)
		{
			num++;
		}
		bool arg4 = false;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangGiveVillagerHelp", ref arg4);
		if (arg4)
		{
			num++;
		}
		return num;
	}

	public void JingangDistributeSecInfo(DataContext context, int metaDataId, int targetCharId)
	{
		GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 623);
		DomainManager.Information.DistributeSecretInformationToCharacter(context, metaDataId, targetCharId, orCreateFixedCharacterByTemplateId.GetId());
	}

	public bool JingangIsInSpreadSutraTask()
	{
		return DomainManager.Extra.IsExtraTaskInProgress(187) || DomainManager.Extra.IsExtraTaskInProgress(288) || DomainManager.Extra.IsExtraTaskInProgress(289) || DomainManager.Extra.IsExtraTaskInProgress(290);
	}

	public void JingangBroadCastSecInfo(DataContext context)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(11);
		int arg = 0;
		if (sectMainStoryEventArgBox.Get("ConchShip_PresetKey_JingangSecInfoMetaDataId", ref arg))
		{
			DomainManager.Information.MakeSecretInformationBroadcastEffect(context, arg, -1);
			if (DomainManager.Information.GetSecretInformationConfig(arg).TemplateId == 115)
			{
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 11, "ConchShip_PresetKey_JingangMonkSoulBtnDisappear", value: true);
			}
		}
	}

	public bool IsTaiwuAtRanshanSettlement(bool settlementBlockOnly = false)
	{
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(7);
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		return settlementBlockOnly ? DomainManager.Map.IsLocationOnSettlementBlock(location, settlementIdByOrgTemplateId) : DomainManager.Map.IsLocationInSettlementInfluenceRange(location, settlementIdByOrgTemplateId);
	}

	public bool IsRanshanSectMainStoryAbleToTrigger()
	{
		Location validLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		int num = 0;
		bool flag = false;
		if (validLocation.IsValid())
		{
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(validLocation.AreaId);
			flag = stateTemplateIdByAreaId == 7;
		}
		for (sbyte b = 0; b < 14; b++)
		{
			if (DomainManager.Item.HasTrackedSpecialItems(12, (short)(211 + b)))
			{
				num++;
			}
		}
		return num >= 3 && CheckSectMainStoryAvailable(7) && flag && !MapAreaData.IsBrokenArea(validLocation.AreaId) && DomainManager.Organization.GetSettlementByOrgTemplateId(7).CalcApprovingRate() >= 500;
	}

	public bool IsRanshanChapter1MonthlyEvent2AbleToTrigger()
	{
		if (!DomainManager.Extra.IsExtraTaskInProgress(293))
		{
			return false;
		}
		int ranshanChapter1MonthlyEventTriggeredCount = GetRanshanChapter1MonthlyEventTriggeredCount();
		if (ranshanChapter1MonthlyEventTriggeredCount >= 3)
		{
			return false;
		}
		Location validLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		MapAreaData areaByAreaId = DomainManager.Map.GetAreaByAreaId(validLocation.AreaId);
		if (areaByAreaId.GetConfig().StateID != 7)
		{
			return false;
		}
		return GetCurrDate() - GetRanshanChapter1MonthlyEventTriggeredDate() >= 3;
	}

	public bool IsRanshanChapter1MonthlyEvent3AbleToTrigger()
	{
		return DomainManager.Extra.IsExtraTaskInProgress(295) && IsTaiwuAtRanshanSettlement(settlementBlockOnly: true);
	}

	public bool IsRanshanChapter2HuajuAbleToTrigger()
	{
		return DomainManager.Extra.IsExtraTaskInProgress(296) && DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(660) == null;
	}

	public bool IsRanshanChapter2XuanzhiAbleToTrigger()
	{
		return DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(660) != null && DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(661) == null;
	}

	public bool IsRanshanChapter2YingjiaoAbleToTrigger()
	{
		return DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(661) != null && DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(662) == null;
	}

	public bool IsRanshanChapter2MonthlyEventAbleToTrigger()
	{
		int arg = 0;
		DomainManager.Extra.GetSectMainStoryEventArgBox(7).Get("ConchShip_PresetKey_Ranshan_Chapter2_TeachStartDate", ref arg);
		return DomainManager.Extra.IsExtraTaskChainInProgress(49) && (IsAllRanshanMenteeFinishedTeaching() || (DomainManager.Extra.GetSectMainStoryEventArgBox(7).Get("ConchShip_PresetKey_Ranshan_Chapter2_TeachStartDate", ref arg) && GetCurrDate() - arg >= 24));
	}

	public bool IsAllRanshanMenteeFinishedTeaching()
	{
		foreach (short ranshanThreeCorpsesCharacterTemplateId in SectMainStoryRelatedConstants.RanshanThreeCorpsesCharacterTemplateIdList)
		{
			if (DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(ranshanThreeCorpsesCharacterTemplateId).Progress != 3)
			{
				return false;
			}
		}
		return true;
	}

	public int GetRanshanChapter1MonthlyEventTriggeredCount()
	{
		int arg = 0;
		return DomainManager.Extra.GetSectMainStoryEventArgBox(7).Get("ConchShip_PresetKey_Ranshan_Chapter1_MonthlyEventTriggeredCount", ref arg) ? arg : 0;
	}

	public int GetRanshanChapter1MonthlyEventTriggeredDate()
	{
		int arg = 0;
		return DomainManager.Extra.GetSectMainStoryEventArgBox(7).Get("ConchShip_PresetKey_Ranshan_Chapter1_MonthlyEventTriggeredDate", ref arg) ? arg : 0;
	}

	public void UpdateRanshanThreeCorpsesAction(DataContext context)
	{
		if (GetSectMainStoryTaskStatus(7) != 1)
		{
			return;
		}
		foreach (short ranshanThreeCorpsesCharacterTemplateId in SectMainStoryRelatedConstants.RanshanThreeCorpsesCharacterTemplateIdList)
		{
			SectStoryThreeCorpsesCharacter ranshanThreeCorpsesCharacterByTemplateId = DomainManager.Extra.GetRanshanThreeCorpsesCharacterByTemplateId(ranshanThreeCorpsesCharacterTemplateId);
			if (ranshanThreeCorpsesCharacterByTemplateId == null || !ranshanThreeCorpsesCharacterByTemplateId.IsGoodEnd)
			{
				continue;
			}
			int currDate = GetCurrDate();
			if (ranshanThreeCorpsesCharacterByTemplateId.Target < 0)
			{
				continue;
			}
			if (currDate >= ranshanThreeCorpsesCharacterByTemplateId.NextDate)
			{
				if (DomainManager.LegendaryBook.GetOwner(ranshanThreeCorpsesCharacterByTemplateId.Target) != ranshanThreeCorpsesCharacterByTemplateId.TargetOwner)
				{
					DomainManager.Extra.ApplyRanshanThreeCorpsesLegendaryBookActionResult(context, ranshanThreeCorpsesCharacterTemplateId, 320, isSuccess: false);
				}
				else if (DomainManager.Extra.GetRanshanThreeCorpsesActionSucceed(context, ranshanThreeCorpsesCharacterTemplateId))
				{
					DomainManager.Extra.ApplyRanshanThreeCorpsesLegendaryBookActionResult(context, ranshanThreeCorpsesCharacterTemplateId, 314, isSuccess: true);
				}
				else
				{
					DomainManager.Extra.SetRanshanThreeCorpsesCharacterNextDate(context, ranshanThreeCorpsesCharacterTemplateId);
				}
			}
			if (currDate >= ranshanThreeCorpsesCharacterByTemplateId.EndDate && ranshanThreeCorpsesCharacterByTemplateId.EndDate >= 0)
			{
				DomainManager.Extra.ApplyRanshanThreeCorpsesLegendaryBookActionResult(context, ranshanThreeCorpsesCharacterTemplateId, 317, isSuccess: false);
			}
			DomainManager.Extra.SetRanshanThreeCorpsesCharacterLocation(context, ranshanThreeCorpsesCharacterTemplateId);
		}
	}

	public void ConvertRanshanFootman(DataContext context, bool isGoodEnd)
	{
		GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 625);
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short randomFavorability = FavorabilityType.GetRandomFavorability(context.Random, (sbyte)((!isGoodEnd) ? 1 : 6));
		DomainManager.Character.ConvertFixedCharacter(context, orCreateFixedCharacterByTemplateId, taiwu.GetLocation(), recreateAttributesAndQualifications: false);
		DomainManager.Character.DirectlySetFavorabilities(context, orCreateFixedCharacterByTemplateId.GetId(), taiwu.GetId(), randomFavorability, randomFavorability);
		if (isGoodEnd)
		{
			DomainManager.Organization.ChangeGrade(context, orCreateFixedCharacterByTemplateId, 5, destPrincipal: true);
		}
	}

	public IntList BaihuaGetSpecialDebuffIntList()
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaSpecialDebuffIntList", out IntList arg);
		return arg;
	}

	public void BaihuaAddCharIdToSpecialDebuffIntList(DataContext context, int charId)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaSpecialDebuffIntList", out IntList arg);
		ref List<int> items = ref arg.Items;
		if (items == null)
		{
			items = new List<int>();
		}
		arg.Items.Add(charId);
		DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaSpecialDebuffIntList", arg);
	}

	public IntList BaihuaGetCureSpecialDebuffIntList()
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaCureSpecialDebuffIntList", out IntList arg);
		return arg;
	}

	public void BaihuaAddCharIdToCureSpecialDebuffIntList(DataContext context, int charId)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaCureSpecialDebuffIntList", out IntList arg);
		ref List<int> items = ref arg.Items;
		if (items == null)
		{
			items = new List<int>();
		}
		arg.Items.Add(charId);
		DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaCureSpecialDebuffIntList", arg);
	}

	public void BaihuaRemoveCharIdToCureSpecialDebuffIntList(DataContext context, int charId)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaCureSpecialDebuffIntList", out IntList arg);
		if (arg.Items != null)
		{
			arg.Items.Remove(charId);
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaCureSpecialDebuffIntList", arg);
		}
	}

	public short BaihuaSelectSettlementId(DataContext context, bool avoidGuangnan, bool avoidTaiwuVillageArea)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		List<short> list3 = ObjectPool<List<short>>.Instance.Get();
		for (short num = 1; num <= 15; num++)
		{
			list2.Clear();
			if (!avoidGuangnan || num != 3)
			{
				sbyte stateIdByStateTemplateId = DomainManager.Map.GetStateIdByStateTemplateId(num);
				DomainManager.Map.GetAllRegularAreaInState(stateIdByStateTemplateId, list2);
				for (int i = 0; i < list2.Count; i++)
				{
					if (!DomainManager.Map.IsAreaBroken(list2[i]))
					{
						list.Add(list2[i]);
					}
				}
			}
		}
		if (avoidTaiwuVillageArea)
		{
			list.Remove(taiwuVillageLocation.AreaId);
		}
		short random = list.GetRandom(context.Random);
		DomainManager.Map.GetAreaSettlementIds(random, list3, containsMainCity: true, containsSect: true);
		short random2 = list3.GetRandom(context.Random);
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
		ObjectPool<List<short>>.Instance.Return(list3);
		return random2;
	}

	public List<short> BaihuaSelectSettlementIds(DataContext context, bool avoidGuangnan, bool avoidTaiwuVillageArea, int needCount)
	{
		int num = 0;
		List<short> list = new List<short>();
		while (list.Count < needCount && num < 10000)
		{
			short item = BaihuaSelectSettlementId(context, avoidGuangnan, avoidTaiwuVillageArea);
			if (!list.Contains(item))
			{
				list.Add(item);
			}
			num++;
		}
		return list;
	}

	public short BaihuaSelectSettlementIdNeighborTaiwuVillage(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(taiwuVillageLocation.AreaId);
		sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(taiwuVillageLocation.AreaId);
		MapStateItem mapStateItem = MapState.Instance[stateTemplateIdByAreaId];
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		List<short> list3 = ObjectPool<List<short>>.Instance.Get();
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		short arg = -1;
		sbyte b = -1;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", ref arg);
		short arg2 = -1;
		sbyte b2 = -1;
		sectMainStoryEventArgBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", ref arg2);
		if (arg != -1)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(arg);
			b = DomainManager.Map.GetStateIdByAreaId(settlement.GetLocation().AreaId);
		}
		if (arg2 != -1)
		{
			Settlement settlement2 = DomainManager.Organization.GetSettlement(arg2);
			b2 = DomainManager.Map.GetStateIdByAreaId(settlement2.GetLocation().AreaId);
		}
		for (int i = 0; i < mapStateItem.NeighborStates.Length; i++)
		{
			list2.Clear();
			if (mapStateItem.NeighborStates[i] == 3)
			{
				continue;
			}
			sbyte stateIdByStateTemplateId = DomainManager.Map.GetStateIdByStateTemplateId(mapStateItem.NeighborStates[i]);
			if (stateIdByStateTemplateId == b || stateIdByStateTemplateId == b2)
			{
				continue;
			}
			DomainManager.Map.GetAllRegularAreaInState(stateIdByStateTemplateId, list2);
			for (int j = 0; j < list2.Count; j++)
			{
				if (!DomainManager.Map.IsAreaBroken(list2[j]))
				{
					list.Add(list2[j]);
				}
			}
		}
		if (list.Count == 0)
		{
			short settlementId = ((arg > 0) ? arg : arg2);
			Settlement settlement3 = DomainManager.Organization.GetSettlement(settlementId);
			sbyte stateTemplateIdByAreaId2 = DomainManager.Map.GetStateTemplateIdByAreaId(settlement3.GetLocation().AreaId);
			MapStateItem mapStateItem2 = MapState.Instance[stateTemplateIdByAreaId2];
			for (int k = 0; k < mapStateItem2.NeighborStates.Length; k++)
			{
				list2.Clear();
				if (mapStateItem2.NeighborStates[k] == 3)
				{
					continue;
				}
				sbyte stateIdByStateTemplateId2 = DomainManager.Map.GetStateIdByStateTemplateId(mapStateItem2.NeighborStates[k]);
				if (stateIdByStateTemplateId2 == stateIdByAreaId)
				{
					continue;
				}
				DomainManager.Map.GetAllRegularAreaInState(stateIdByStateTemplateId2, list2);
				for (int l = 0; l < list2.Count; l++)
				{
					if (!DomainManager.Map.IsAreaBroken(list2[l]))
					{
						list.Add(list2[l]);
					}
				}
			}
		}
		short random = list.GetRandom(context.Random);
		DomainManager.Map.GetAreaSettlementIds(random, list3, containsMainCity: true, containsSect: true);
		short random2 = list3.GetRandom(context.Random);
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
		ObjectPool<List<short>>.Instance.Return(list3);
		return random2;
	}

	public void CallBaihuaMember(DataContext context, bool isLeuko)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		short arg = -1;
		IntList arg2;
		if (isLeuko)
		{
			sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaLeukoKillsCalledCharIds", out arg2);
			sectMainStoryEventArgBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsMonthEventSettlementId", ref arg);
		}
		else
		{
			sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaMelanoKillsCalledCharIds", out arg2);
			sectMainStoryEventArgBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsMonthEventSettlementId", ref arg);
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(arg);
		if (arg2.Items == null)
		{
			arg2 = IntList.Create();
		}
		if (!HaveAliveMember(arg2.Items))
		{
			Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(3);
			List<int> list = ObjectPool<List<int>>.Instance.Get();
			for (sbyte b = 0; b <= 5; b++)
			{
				list.AddRange(settlementByOrgTemplateId.GetMembers().GetMembers(b));
			}
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (DomainManager.Character.TryGetElement_Objects(list[num], out var element))
				{
					if (!element.IsInteractableAsIntelligentCharacter() || element.GetAgeGroup() != 2)
					{
						list.RemoveAt(num);
					}
				}
				else
				{
					list.RemoveAt(num);
				}
			}
			int val = context.Random.Next(1, 4);
			CollectionUtils.Shuffle(context.Random, list);
			for (int i = 0; i < Math.Min(val, list.Count); i++)
			{
				int num2 = list[i];
				arg2.Items.Add(num2);
				if (DomainManager.Character.TryGetElement_Objects(num2, out var element2))
				{
					DomainManager.Character.GroupMove(context, element2, settlement.GetLocation());
					element2.ActiveExternalRelationState(context, 4);
				}
			}
			ObjectPool<List<int>>.Instance.Return(list);
			if (isLeuko)
			{
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaLeukoKillsCalledCharIds", arg2);
			}
			else
			{
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaMelanoKillsCalledCharIds", arg2);
			}
			return;
		}
		for (int j = 0; j < arg2.Items.Count; j++)
		{
			if (DomainManager.Character.TryGetElement_Objects(arg2.Items[j], out var element3))
			{
				DomainManager.Character.GroupMove(context, element3, settlement.GetLocation());
				element3.ActiveExternalRelationState(context, 4);
			}
		}
		static bool HaveAliveMember(List<int> members)
		{
			for (int k = 0; k < members.Count; k++)
			{
				if (DomainManager.Character.TryGetElement_Objects(members[k], out var _))
				{
					return true;
				}
			}
			return false;
		}
	}

	public void BaihuaClearCalledCharacters(DataContext context, bool isLeuko)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		IntList arg;
		if (isLeuko)
		{
			sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaLeukoKillsCalledCharIds", out arg);
		}
		else
		{
			sectMainStoryEventArgBox.Get<IntList>("ConchShip_PresetKey_BaihuaMelanoKillsCalledCharIds", out arg);
		}
		if (arg.Items == null)
		{
			return;
		}
		for (int i = 0; i < arg.Items.Count; i++)
		{
			if (DomainManager.Character.TryGetElement_Objects(arg.Items[i], out var element))
			{
				element.DeactivateExternalRelationState(context, 4);
			}
		}
	}

	public int BaihuaGroupMeetCount(bool isLeuko, out int groupId)
	{
		groupId = -1;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		sbyte arg = -1;
		if (isLeuko)
		{
			sectMainStoryEventArgBox.Get("ConchShip_PresetKey_BaihuaLeukoKillsFiveElementsType", ref arg);
		}
		else
		{
			sectMainStoryEventArgBox.Get("ConchShip_PresetKey_BaihuaMelanoKillsFiveElementsType", ref arg);
		}
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		int num = 0;
		foreach (int item in collection)
		{
			if (item != taiwu.GetId() && DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetAgeGroup() >= 2)
			{
				NeiliTypeItem neiliTypeItem = NeiliType.Instance[element.GetNeiliType()];
				if (neiliTypeItem.FiveElements == arg)
				{
					groupId = element.GetId();
					num++;
				}
			}
		}
		return num;
	}

	public bool FulongDisasterStart()
	{
		if (DomainManager.Character.TryGetFixedCharacterByTemplateId(446, out var _) && !DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Contains<bool>("YuFuTellRanchenziStory"))
		{
			return true;
		}
		return false;
	}

	private unsafe void FulongTriggerDisaster(DataContext context)
	{
		int num = 6;
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(14);
		List<MapBlockData> mapBlockList = ObjectPool<List<MapBlockData>>.Instance.Get();
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		DomainManager.Map.GetLocationByDistance(settlementByOrgTemplateId.GetLocation(), num, num, ref mapBlockList);
		for (int i = 0; i < mapBlockList.Count; i++)
		{
			MapBlockData mapBlockData = mapBlockList[i];
			if (mapBlockData.GetConfig().SubType != EMapBlockSubType.DLCLoong && mapBlockData.GetConfig().Size <= 1 && mapBlockData.IsNonDeveloped())
			{
				list.Add(mapBlockData);
			}
		}
		if (list.Count < 1)
		{
			return;
		}
		MapBlockData random = list.GetRandom(context.Random);
		mapBlockList.Clear();
		DomainManager.Map.GetLocationByDistance(random.GetLocation(), 1, 1, ref mapBlockList);
		MapBlockData random2 = mapBlockList.GetRandom(context.Random);
		int resourceType = 0;
		int num2 = 0;
		for (sbyte b = 0; b < 6; b++)
		{
			if (random2.CurrResources.Items[b] > num2)
			{
				num2 = random2.CurrResources.Items[b];
				resourceType = b;
			}
		}
		List<short> disasterAdventureTypesByResourceType = DomainManager.Adventure.GetDisasterAdventureTypesByResourceType(resourceType);
		short random3 = disasterAdventureTypesByResourceType.GetRandom(context.Random);
		Location location = random2.GetLocation();
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(settlementByOrgTemplateId.GetLocation().AreaId);
		if (!Enumerable.Contains(adventuresInArea.AdventureSites.Keys, location.BlockId))
		{
			DomainManager.Adventure.RemoveAdventureSite(context, location.AreaId, location.BlockId, isTimeout: false, isComplete: false);
			DomainManager.Adventure.TryCreateAdventureSite(context, location.AreaId, location.BlockId, random3, MonthlyActionKey.Invalid);
		}
		mapBlockList.Add(random);
		for (int j = 0; j < mapBlockList.Count; j++)
		{
			MapBlockData mapBlockData2 = mapBlockList[j];
			if (mapBlockData2.GetConfig().Size <= 1)
			{
				mapBlockData2.Destroyed = true;
				mapBlockData2.DestroyItemsDirect(context.AdvanceMonthRelatedData.WorldItemsToBeRemoved);
				mapBlockData2.Malice = 0;
				for (sbyte b2 = 0; b2 < 6; b2++)
				{
					mapBlockData2.CurrResources.Items[b2] = 0;
				}
				DomainManager.Map.SetBlockData(context, mapBlockData2);
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(mapBlockList);
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}

	public bool ShixiangSettlementAffiliatedBlocksHasEnemy(DataContext context, short startTemplateId)
	{
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(6);
		Location location = settlementByOrgTemplateId.GetLocation();
		List<MapBlockData> obj = context.AdvanceMonthRelatedData.Blocks.Occupy();
		DomainManager.Map.GetSettlementAffiliatedBlocks(location.AreaId, location.BlockId, obj);
		bool result = false;
		foreach (MapBlockData item in obj)
		{
			if (item.EnemyCharacterSet == null)
			{
				continue;
			}
			foreach (int item2 in item.EnemyCharacterSet)
			{
				DomainManager.Character.TryGetElement_Objects(item2, out var element);
				short templateId = element.GetTemplateId();
				if (templateId < startTemplateId || templateId > startTemplateId + 4)
				{
					continue;
				}
				result = true;
				break;
			}
		}
		context.AdvanceMonthRelatedData.Blocks.Release(ref obj);
		return result;
	}

	private void InitializeBaihuaLinkedCharacters(DataContext context)
	{
		_baihuaLinkedCharacters = new Dictionary<int, (int, bool)>();
		SectBaihuaLifeLinkData sectBaihuaLifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
		if (!sectBaihuaLifeLinkData.IsInitialized())
		{
			_baihuaLifeLinkNeiliType = -1;
			sectBaihuaLifeLinkData.Initialize();
			DomainManager.Extra.SetSectBaihuaLifeLinkData(sectBaihuaLifeLinkData, context);
			return;
		}
		_baihuaLifeLinkNeiliType = CalcBaihuaLifeLinkNeiliType();
		for (int i = 0; i < sectBaihuaLifeLinkData.LifeGateCharIds.Length; i++)
		{
			int num = sectBaihuaLifeLinkData.LifeGateCharIds[i];
			if (num >= 0)
			{
				_baihuaLinkedCharacters.Add(num, (i, true));
			}
		}
		for (int j = 0; j < sectBaihuaLifeLinkData.DeathGateCharIds.Length; j++)
		{
			int num2 = sectBaihuaLifeLinkData.DeathGateCharIds[j];
			if (num2 >= 0)
			{
				_baihuaLinkedCharacters.Add(num2, (j, false));
			}
		}
	}

	[DomainMethod]
	public sbyte GetBaihuaLifeLinkNeiliType()
	{
		return _baihuaLifeLinkNeiliType;
	}

	[DomainMethod]
	public void SetLifeLinkCharacter(DataContext context, int charId, int index, bool isLifeGate)
	{
		Logger.Info($"Setting life link character {charId}: isLifeGate = {isLifeGate}, index = {index}");
		SectBaihuaLifeLinkData sectBaihuaLifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
		int[] array = (isLifeGate ? sectBaihuaLifeLinkData.LifeGateCharIds : sectBaihuaLifeLinkData.DeathGateCharIds);
		int num = array[index];
		if (num >= 0)
		{
			_baihuaLinkedCharacters.Remove(num);
			if (_baihuaLifeLinkNeiliType >= 0)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
				NeiliTypeItem neiliTypeItem = NeiliType.Instance[_baihuaLifeLinkNeiliType];
				short[] array2 = (isLifeGate ? neiliTypeItem.LifeGateFeatures : neiliTypeItem.DeathGateFeatures);
				if (array2 != null)
				{
					for (int i = 0; i < array2.Length; i++)
					{
						element_Objects.RemoveFeature(context, array2[i]);
					}
				}
			}
		}
		array[index] = charId;
		DomainManager.Extra.SetSectBaihuaLifeLinkData(sectBaihuaLifeLinkData, context);
		bool flag = UpdateBaihuaLifeLinkNeiliType(context);
		if (charId < 0)
		{
			return;
		}
		_baihuaLinkedCharacters.Add(charId, (index, isLifeGate));
		if (flag || _baihuaLifeLinkNeiliType < 0)
		{
			return;
		}
		GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(charId);
		NeiliTypeItem neiliTypeItem2 = NeiliType.Instance[_baihuaLifeLinkNeiliType];
		short[] array3 = (isLifeGate ? neiliTypeItem2.LifeGateFeatures : neiliTypeItem2.DeathGateFeatures);
		if (array3 != null)
		{
			for (int j = 0; j < array3.Length; j++)
			{
				element_Objects2.AddFeature(context, array3[j]);
			}
		}
	}

	public void TryRemoveLifeLinkCharacter(DataContext context, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		if (!_baihuaLinkedCharacters.TryGetValue(id, out (int, bool) value))
		{
			return;
		}
		SectBaihuaLifeLinkData sectBaihuaLifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
		int[] array = (value.Item2 ? sectBaihuaLifeLinkData.LifeGateCharIds : sectBaihuaLifeLinkData.DeathGateCharIds);
		array[value.Item1] = -1;
		_baihuaLinkedCharacters.Remove(id);
		if (_baihuaLifeLinkNeiliType >= 0 && DomainManager.Character.IsCharacterAlive(id))
		{
			NeiliTypeItem neiliTypeItem = NeiliType.Instance[_baihuaLifeLinkNeiliType];
			short[] lifeGateFeatures = neiliTypeItem.LifeGateFeatures;
			foreach (short featureId in lifeGateFeatures)
			{
				character.RemoveFeature(context, featureId);
			}
		}
		DomainManager.Extra.SetSectBaihuaLifeLinkData(sectBaihuaLifeLinkData, context);
		UpdateBaihuaLifeLinkNeiliType(context);
	}

	private void UpdateBaihuaFixedCharacterLocations(DataContext context, int charId)
	{
		int currDate = DomainManager.World.GetCurrDate();
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		int arg = int.MaxValue;
		if (!sectMainStoryEventArgBox.Get("ConchShip_PresetKey_BaihuaLMTransferAnimalDate", ref arg) || arg != currDate)
		{
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 3, "ConchShip_PresetKey_BaihuaLMTransferAnimalDate", currDate);
			GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 781);
			GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId2 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 786);
			GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId3 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 808);
			GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId4 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 809);
			if (orCreateFixedCharacterByTemplateId.GetLocation().IsValid())
			{
				Events.RaiseFixedCharacterLocationChanged(context, orCreateFixedCharacterByTemplateId3.GetId(), orCreateFixedCharacterByTemplateId3.GetLocation(), orCreateFixedCharacterByTemplateId.GetLocation());
				orCreateFixedCharacterByTemplateId3.SetLocation(orCreateFixedCharacterByTemplateId.GetLocation(), context);
				Events.RaiseFixedCharacterLocationChanged(context, orCreateFixedCharacterByTemplateId4.GetId(), orCreateFixedCharacterByTemplateId4.GetLocation(), orCreateFixedCharacterByTemplateId2.GetLocation());
				orCreateFixedCharacterByTemplateId4.SetLocation(orCreateFixedCharacterByTemplateId2.GetLocation(), context);
				Events.RaiseFixedCharacterLocationChanged(context, orCreateFixedCharacterByTemplateId.GetId(), orCreateFixedCharacterByTemplateId.GetLocation(), Location.Invalid);
				orCreateFixedCharacterByTemplateId.SetLocation(Location.Invalid, context);
				Events.RaiseFixedCharacterLocationChanged(context, orCreateFixedCharacterByTemplateId2.GetId(), orCreateFixedCharacterByTemplateId2.GetLocation(), Location.Invalid);
				orCreateFixedCharacterByTemplateId2.SetLocation(Location.Invalid, context);
				InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
				instantNotificationCollection.AddSectStoryBaihuaToAnimal(charId);
			}
		}
	}

	public bool UpdateBaihuaLifeLinkNeiliType(DataContext context)
	{
		SectBaihuaLifeLinkData sectBaihuaLifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
		if (!sectBaihuaLifeLinkData.IsInitialized())
		{
			return false;
		}
		sbyte b = CalcBaihuaLifeLinkNeiliType();
		if (b == _baihuaLifeLinkNeiliType)
		{
			return false;
		}
		if (_baihuaLifeLinkNeiliType >= 0)
		{
			NeiliTypeItem neiliTypeItem = NeiliType.Instance[_baihuaLifeLinkNeiliType];
			int[] lifeGateCharIds = sectBaihuaLifeLinkData.LifeGateCharIds;
			foreach (int num in lifeGateCharIds)
			{
				if (num >= 0)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
					short[] lifeGateFeatures = neiliTypeItem.LifeGateFeatures;
					foreach (short featureId in lifeGateFeatures)
					{
						element_Objects.RemoveFeature(context, featureId);
					}
				}
			}
			int[] deathGateCharIds = sectBaihuaLifeLinkData.DeathGateCharIds;
			foreach (int num2 in deathGateCharIds)
			{
				if (num2 >= 0)
				{
					GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(num2);
					short[] deathGateFeatures = neiliTypeItem.DeathGateFeatures;
					foreach (short featureId2 in deathGateFeatures)
					{
						element_Objects2.RemoveFeature(context, featureId2);
					}
				}
			}
		}
		if (b >= 0)
		{
			NeiliTypeItem neiliTypeItem2 = NeiliType.Instance[b];
			int[] lifeGateCharIds2 = sectBaihuaLifeLinkData.LifeGateCharIds;
			foreach (int num3 in lifeGateCharIds2)
			{
				if (num3 >= 0)
				{
					GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(num3);
					short[] lifeGateFeatures2 = neiliTypeItem2.LifeGateFeatures;
					foreach (short featureId3 in lifeGateFeatures2)
					{
						element_Objects3.AddFeature(context, featureId3);
					}
				}
			}
			int[] deathGateCharIds2 = sectBaihuaLifeLinkData.DeathGateCharIds;
			foreach (int num5 in deathGateCharIds2)
			{
				if (num5 >= 0)
				{
					GameData.Domains.Character.Character element_Objects4 = DomainManager.Character.GetElement_Objects(num5);
					short[] deathGateFeatures2 = neiliTypeItem2.DeathGateFeatures;
					foreach (short featureId4 in deathGateFeatures2)
					{
						element_Objects4.AddFeature(context, featureId4);
					}
				}
			}
		}
		_baihuaLifeLinkNeiliType = b;
		if (_advancingMonthState != 0 && !_sectMainStoryLifeLinkUpdated)
		{
			_sectMainStoryLifeLinkUpdated = true;
			GetMonthlyNotificationCollection().AddFiveElementsChange();
		}
		return true;
	}

	private sbyte CalcBaihuaLifeLinkNeiliType()
	{
		SectBaihuaLifeLinkData sectBaihuaLifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
		Span<NeiliProportionOfFiveElements> span = stackalloc NeiliProportionOfFiveElements[sectBaihuaLifeLinkData.LifeGateCharIds.Length + sectBaihuaLifeLinkData.DeathGateCharIds.Length];
		SpanList<NeiliProportionOfFiveElements> spanList = span;
		int num = 0;
		int[] lifeGateCharIds = sectBaihuaLifeLinkData.LifeGateCharIds;
		foreach (int objectId in lifeGateCharIds)
		{
			if (DomainManager.Character.TryGetElement_Objects(objectId, out var element))
			{
				NeiliProportionOfFiveElements neiliProportionOfFiveElements = element.GetNeiliProportionOfFiveElements();
				spanList.Add(neiliProportionOfFiveElements);
				num++;
			}
		}
		if (num == 0)
		{
			return -1;
		}
		int num2 = 0;
		int[] deathGateCharIds = sectBaihuaLifeLinkData.DeathGateCharIds;
		foreach (int objectId2 in deathGateCharIds)
		{
			if (DomainManager.Character.TryGetElement_Objects(objectId2, out var element2))
			{
				NeiliProportionOfFiveElements neiliProportionOfFiveElements2 = element2.GetNeiliProportionOfFiveElements();
				spanList.Add(neiliProportionOfFiveElements2);
				num2++;
			}
		}
		if (num2 == 0)
		{
			return -1;
		}
		NeiliProportionOfFiveElements total = NeiliProportionOfFiveElements.GetTotal(spanList);
		return total.GetNeiliType(DomainManager.World.GetCurrMonthInYear());
	}

	private void UpdateLifeDeathGateCharacters(DataContext context)
	{
		SectBaihuaLifeLinkData sectBaihuaLifeLinkData = DomainManager.Extra.GetSectBaihuaLifeLinkData();
		if (sectBaihuaLifeLinkData == null || !sectBaihuaLifeLinkData.IsInitialized())
		{
			return;
		}
		if (sectBaihuaLifeLinkData.Cooldown > 0)
		{
			sectBaihuaLifeLinkData.Cooldown--;
			DomainManager.Extra.SetSectBaihuaLifeLinkData(sectBaihuaLifeLinkData, context);
			if (sectBaihuaLifeLinkData.Cooldown >= 0)
			{
				return;
			}
		}
		_tmpLifeGateChars.Clear();
		int[] lifeGateCharIds = sectBaihuaLifeLinkData.LifeGateCharIds;
		foreach (int num in lifeGateCharIds)
		{
			if (num >= 0)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
				short health = element_Objects.GetHealth();
				short leftMaxHealth = element_Objects.GetLeftMaxHealth();
				if (health < leftMaxHealth)
				{
					_tmpLifeGateChars.Add((element_Objects, health, leftMaxHealth));
				}
			}
		}
		int num2 = 0;
		_tmpDeathGateChars.Clear();
		int[] deathGateCharIds = sectBaihuaLifeLinkData.DeathGateCharIds;
		foreach (int num3 in deathGateCharIds)
		{
			if (num3 >= 0)
			{
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(num3);
				short health2 = element_Objects2.GetHealth();
				if (health2 > 0)
				{
					num2 += health2;
					_tmpDeathGateChars.Add((element_Objects2, health2));
				}
			}
		}
		if (_tmpLifeGateChars.Count == 0 || _tmpDeathGateChars.Count == 0)
		{
			return;
		}
		IRandomSource random = context.Random;
		Span<int> span = stackalloc int[8];
		SpanList<int> spanList = span;
		span = stackalloc int[8];
		SpanList<int> spanList2 = span;
		while (_tmpLifeGateChars.Count > 0 && _tmpDeathGateChars.Count > 0)
		{
			int index = random.Next(_tmpLifeGateChars.Count);
			(GameData.Domains.Character.Character, int, int) tuple = _tmpLifeGateChars[index];
			int num4 = Math.Min(tuple.Item3 - tuple.Item2, num2);
			int num5 = 0;
			for (int num6 = _tmpDeathGateChars.Count - 1; num6 >= 0; num6--)
			{
				(GameData.Domains.Character.Character, int) value = _tmpDeathGateChars[num6];
				int num7 = num4 * value.Item2 / num2;
				if (num7 > 0)
				{
					value.Item2 -= num7;
					tuple.Item2 += num7;
					num5 += num7;
					if (value.Item2 <= 0)
					{
						CollectionUtils.SwapAndRemove(_tmpDeathGateChars, num6);
						value.Item1.SetHealth(0, context);
						int id = value.Item1.GetId();
						spanList2.Add(id);
						UpdateBaihuaFixedCharacterLocations(context, id);
						sectBaihuaLifeLinkData.Cooldown = GlobalConfig.Instance.BaihuaLifeLinkRemoveCharacterCooldown;
					}
					else
					{
						_tmpDeathGateChars[num6] = value;
					}
				}
			}
			if (tuple.Item2 < tuple.Item3)
			{
				num4 = tuple.Item3 - tuple.Item2;
				while (num4 > 0 && _tmpDeathGateChars.Count > 0)
				{
					int index2 = random.Next(_tmpDeathGateChars.Count);
					(GameData.Domains.Character.Character, int) value2 = _tmpDeathGateChars[index2];
					int num8 = Math.Min(num4, value2.Item2);
					tuple.Item2 += num8;
					value2.Item2 -= num8;
					num4 -= num8;
					num5 += num8;
					if (value2.Item2 <= 0)
					{
						CollectionUtils.SwapAndRemove(_tmpDeathGateChars, index2);
						value2.Item1.SetHealth(0, context);
						int id2 = value2.Item1.GetId();
						spanList2.Add(id2);
						UpdateBaihuaFixedCharacterLocations(context, id2);
						sectBaihuaLifeLinkData.Cooldown = GlobalConfig.Instance.BaihuaLifeLinkRemoveCharacterCooldown;
					}
					else
					{
						_tmpDeathGateChars[index2] = value2;
					}
				}
			}
			tuple.Item1.SetHealth((short)tuple.Item2, context);
			CollectionUtils.SwapAndRemove(_tmpLifeGateChars, index);
			spanList.Add(tuple.Item1.GetId());
			Tester.Assert(num5 <= num2);
			num2 -= num5;
		}
		foreach (var tmpDeathGateChar in _tmpDeathGateChars)
		{
			short health3 = tmpDeathGateChar.character.GetHealth();
			int item = tmpDeathGateChar.distributableHealth;
			if (health3 != item)
			{
				tmpDeathGateChar.character.SetHealth((short)item, context);
				spanList2.Add(tmpDeathGateChar.character.GetId());
			}
		}
		MonthlyNotificationCollection monthlyNotificationCollection = GetMonthlyNotificationCollection();
		for (int k = 0; k < spanList.Count; k++)
		{
			monthlyNotificationCollection.AddLifeLinkHealing(spanList[k]);
		}
		for (int l = 0; l < spanList2.Count; l++)
		{
			monthlyNotificationCollection.AddLifeLinkDamage(spanList2[l]);
		}
		DomainManager.Extra.SetSectBaihuaLifeLinkData(sectBaihuaLifeLinkData, context);
	}

	[DomainMethod]
	public bool ShaolinInterruptDemonSlayerTrial(DataContext context)
	{
		SectShaolinDemonSlayerData sectShaolinDemonSlayerData = DomainManager.Extra.GetSectShaolinDemonSlayerData();
		DomainManager.TaiwuEvent.CloseUI("ShaolinInterruptDemonSlayerTrial", presetBool: false, sectShaolinDemonSlayerData.TrialingLevel.TemplateId);
		bool flag = sectShaolinDemonSlayerData.ClearDemons();
		if (flag)
		{
			DomainManager.Extra.SetSectShaolinDemonSlayerData(context, sectShaolinDemonSlayerData);
		}
		return flag;
	}

	[DomainMethod]
	public bool ShaolinRegenerateRestricts(DataContext context)
	{
		SectShaolinDemonSlayerData sectShaolinDemonSlayerData = DomainManager.Extra.GetSectShaolinDemonSlayerData();
		bool flag = sectShaolinDemonSlayerData.ReGenerateRestricts(context.Random);
		if (flag)
		{
			DomainManager.Extra.SetSectShaolinDemonSlayerData(context, sectShaolinDemonSlayerData);
		}
		return flag;
	}

	[DomainMethod]
	public byte ShaolinQueryRestrictsAreSatisfied(int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		BoolArray8 val = BoolArray8.op_Implicit((byte)0);
		SectShaolinDemonSlayerData sectShaolinDemonSlayerData = DomainManager.Extra.GetSectShaolinDemonSlayerData();
		int num = 0;
		foreach (DemonSlayerTrialRestrictItem trialingRestrict in sectShaolinDemonSlayerData.GetTrialingRestricts(index))
		{
			((BoolArray8)(ref val))[num++] = trialingRestrict.Check();
		}
		return BoolArray8.op_Implicit(val);
	}

	[DomainMethod]
	public void ShaolinStartDemonSlayerTrial(DataContext context, int index)
	{
		DomainManager.TaiwuEvent.OnEvent_StartSectShaolinDemonSlayer(index);
	}

	[DomainMethod]
	public List<int> ShaolinGenerateTemporaryDemon(DataContext context)
	{
		List<int> list = new List<int>();
		SectShaolinDemonSlayerData sectShaolinDemonSlayerData = DomainManager.Extra.GetSectShaolinDemonSlayerData();
		for (int i = 0; i < 2; i++)
		{
			DemonSlayerTrialItem trialingDemon = sectShaolinDemonSlayerData.GetTrialingDemon(i);
			if (trialingDemon != null)
			{
				GameData.Domains.Character.Character character = CreateFixedDemon(context, trialingDemon.CharacterId);
				list.Add(character.GetId());
			}
		}
		return list;
	}

	[DomainMethod]
	public void ShaolinClearTemporaryDemon(DataContext context, List<int> demonCharIds)
	{
		SectShaolinDemonSlayerData sectShaolinDemonSlayerData = DomainManager.Extra.GetSectShaolinDemonSlayerData();
		for (int i = 0; i < 2; i++)
		{
			DemonSlayerTrialItem trialingDemon = sectShaolinDemonSlayerData.GetTrialingDemon(i);
			if (trialingDemon == null)
			{
				continue;
			}
			int orDefault = demonCharIds.GetOrDefault(i, -1);
			if (DomainManager.Character.TryGetElement_Objects(orDefault, out var element))
			{
				if (element.GetTemplateId() != trialingDemon.CharacterId)
				{
					AdaptableLog.Warning($"Failed to clear demon template by mismatch {element.GetTemplateId()}");
				}
				else
				{
					DomainManager.Character.RemoveNonIntelligentCharacter(context, element);
				}
			}
		}
	}

	public bool ShaolinGenerateDemonSlayerTrial(DataContext context)
	{
		SectShaolinDemonSlayerData sectShaolinDemonSlayerData = DomainManager.Extra.GetSectShaolinDemonSlayerData();
		bool flag = sectShaolinDemonSlayerData.GenerateDemons(context.Random);
		if (flag)
		{
			DomainManager.Extra.SetSectShaolinDemonSlayerData(context, sectShaolinDemonSlayerData);
		}
		return flag;
	}

	public GameData.Domains.Character.Character CreateFixedDemon(DataContext context, short demonTemplateId)
	{
		ulong seed = (ulong)(DomainManager.World.GetWorldId() + demonTemplateId);
		context.SwitchRandomSource(seed);
		GameData.Domains.Character.Character character = DomainManager.Character.CreateFixedEnemy(context, demonTemplateId);
		DomainManager.Character.CompleteCreatingCharacter(character.GetId());
		context.RestoreRandomSource();
		return character;
	}

	[DomainMethod]
	public int GetSectMainStoryTriggerConditions(short templateId)
	{
		int num = 0;
		List<Func<bool>> list = _sectMainStoryTriggerConditions[templateId];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i]())
			{
				num |= 1 << i;
			}
		}
		return num;
	}

	private static bool ShaolinMainStoryTrigger0()
	{
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(1);
		return DomainManager.Organization.GetElement_Sects(settlementByOrgTemplateId.GetId()).GetTaiwuExploreStatus() == 2;
	}

	private static bool ShaolinMainStoryTrigger1()
	{
		Location validLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(1);
		return validLocation.IsValid() && DomainManager.Map.IsLocationOnSettlementBlock(validLocation, settlementIdByOrgTemplateId);
	}

	private static bool EMeiMainStoryTrigger0()
	{
		return DomainManager.Organization.GetSettlementByOrgTemplateId(2).CalcApprovingRate() >= 500;
	}

	private static bool EMeiMainStoryTrigger1()
	{
		short arg = -1;
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
		if (!sectMainStoryEventArgBox.Get("ConchShip_PresetKey_WhiteApeBlockId", ref arg))
		{
			return false;
		}
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		Location location2 = DomainManager.Organization.GetSettlementByOrgTemplateId(2).GetLocation();
		if (location2.AreaId != location.AreaId)
		{
			return false;
		}
		MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, location.BlockId);
		ByteCoordinate blockPos = DomainManager.Map.GetBlockData(location2.AreaId, arg).GetBlockPos();
		return blockData.GetManhattanDistanceToPos(blockPos.X, blockPos.Y) <= 3;
	}

	private static bool BaihuaMainStoryTrigger0()
	{
		return DomainManager.World.GetMainStoryLineProgress() >= 22;
	}

	private static bool BaihuaMainStoryTrigger1()
	{
		return DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId == DomainManager.Map.GetAreaIdByAreaTemplateId(18);
	}

	private static bool WudangMainStoryTrigger0()
	{
		short id = DomainManager.Organization.GetSettlementByOrgTemplateId(4).GetId();
		return DomainManager.Organization.GetElement_Sects(id).GetTaiwuExploreStatus() == 2;
	}

	private static bool WudangMainStoryTrigger1()
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		Location location2 = DomainManager.Organization.GetSettlementByOrgTemplateId(4).GetLocation();
		return location.AreaId == location2.AreaId;
	}

	private static bool YuanshanMainStoryTrigger0()
	{
		return DomainManager.World.GetMainStoryLineProgress() >= 21;
	}

	private static bool YuanshanMainStoryTrigger1()
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		Location location2 = DomainManager.Organization.GetSettlementByOrgTemplateId(5).GetLocation();
		return location.AreaId == location2.AreaId;
	}

	private static bool YuanshanMainStoryTrigger2()
	{
		return DomainManager.Organization.GetSettlementByOrgTemplateId(5).CalcApprovingRate() >= 500;
	}

	private static bool ShixiangMainStoryTrigger0()
	{
		return DomainManager.Organization.GetSettlementByOrgTemplateId(6).CalcApprovingRate() >= 500;
	}

	private static bool ShixiangMainStoryTrigger1()
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(location.AreaId);
		sbyte stateIdByStateTemplateId = DomainManager.Map.GetStateIdByStateTemplateId(6);
		MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, location.BlockId);
		bool flag = stateIdByAreaId == stateIdByStateTemplateId;
		bool flag2 = flag;
		if (flag2)
		{
			EMapBlockType blockType = blockData.BlockType;
			bool flag3 = ((blockType == EMapBlockType.City || blockType == EMapBlockType.Town) ? true : false);
			flag2 = flag3;
		}
		return flag2;
	}

	private static bool RanshanMainStoryTrigger0()
	{
		return DomainManager.Organization.GetSettlementByOrgTemplateId(7).CalcApprovingRate() >= 500;
	}

	private static bool RanshanMainStoryTrigger1()
	{
		int num = 0;
		for (sbyte b = 0; b < 14; b++)
		{
			if (DomainManager.Item.HasTrackedSpecialItems(12, (short)(211 + b)))
			{
				num++;
			}
		}
		return num >= 3;
	}

	private static bool RanshanMainStoryTrigger2()
	{
		Location validLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		return validLocation.IsValid() && !MapAreaData.IsBrokenArea(validLocation.AreaId);
	}

	private static bool XuannvMainStoryTrigger0()
	{
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(8);
		return DomainManager.Organization.GetElement_Sects(settlementByOrgTemplateId.GetId()).GetTaiwuExploreStatus() == 2;
	}

	private static bool XuannvMainStoryTrigger1()
	{
		Location validLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(8);
		return validLocation.IsValid() && DomainManager.Map.IsLocationOnSettlementBlock(validLocation, settlementIdByOrgTemplateId);
	}

	private static bool ZhujianMainStoryTrigger0()
	{
		return DomainManager.Organization.GetSettlementByOrgTemplateId(9).CalcApprovingRate() >= 500;
	}

	private static bool ZhujianMainStoryTrigger1()
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (location.IsValid() && !MapAreaData.IsBrokenArea(location.AreaId))
		{
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
			return stateTemplateIdByAreaId == 9;
		}
		return false;
	}

	private static bool KongsangMainStoryTrigger0()
	{
		return DomainManager.Organization.GetSettlementByOrgTemplateId(10).CalcApprovingRate() >= 500;
	}

	private static bool KongsangMainStoryTrigger1()
	{
		Location validLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(10);
		if (!DomainManager.Map.IsLocationOnSettlementBlock(validLocation, settlementByOrgTemplateId.GetId()))
		{
			return false;
		}
		MapBlockData blockData = DomainManager.Map.GetBlockData(validLocation.AreaId, validLocation.BlockId);
		if (blockData.CharacterSet == null || blockData.CharacterSet.Count == 0)
		{
			return false;
		}
		GameData.Domains.Character.Character leader = settlementByOrgTemplateId.GetLeader();
		if (leader == null)
		{
			return false;
		}
		int id = leader.GetId();
		return blockData.CharacterSet.Contains(id);
	}

	private static bool JingangMainStoryTrigger0()
	{
		return DomainManager.Building.IsTaiwuVillageHaveSpecifyBuilding(50, notBuild: true);
	}

	private static bool JingangMainStoryTrigger1()
	{
		Location validLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		if (DomainManager.Map.IsAreaBroken(validLocation.AreaId))
		{
			return false;
		}
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(validLocation.AreaId);
		return stateTemplateIdByAreaId == 11;
	}

	private static bool WuxianMainStoryTrigger0()
	{
		short id = DomainManager.Organization.GetSettlementByOrgTemplateId(12).GetId();
		return DomainManager.Organization.GetElement_Sects(id).GetSpiritualDebtInteractionOccurred();
	}

	private static bool WuxianMainStoryTrigger1()
	{
		Location validLocation = DomainManager.Taiwu.GetTaiwu().GetValidLocation();
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(12);
		return validLocation.IsValid() && DomainManager.Map.IsLocationInSettlementInfluenceRange(validLocation, settlementIdByOrgTemplateId);
	}

	private static bool FulongMainStoryTrigger0()
	{
		GameData.Domains.Character.Character character;
		return DomainManager.Character.TryGetFixedCharacterByTemplateId(446, out character) && !DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Contains<bool>("YuFuTellRanchenziStory");
	}

	private static bool FulongMainStoryTrigger1()
	{
		return DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId == DomainManager.Map.GetAreaIdByAreaTemplateId(29);
	}

	private static bool XuehouMainStoryTrigger0()
	{
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(35);
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		return location.IsValid() && DomainManager.Map.IsLocationOnSettlementBlock(location, settlementIdByOrgTemplateId);
	}

	[DomainMethod]
	public int TryTriggerThiefCatch(DataContext context)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!TryGetThief(location, out var thiefData, out var thiefIndex))
		{
			return -1;
		}
		if (thiefData.ThiefTriggered[thiefIndex])
		{
			return -1;
		}
		List<SectStoryThiefData> sectZhujianThiefList = DomainManager.Extra.GetSectZhujianThiefList();
		thiefData.ThiefTriggered[thiefIndex] = true;
		bool flag = thiefIndex == thiefData.RealThiefIndex;
		if (flag || thiefData.AllIsTriggered())
		{
			sectZhujianThiefList.Remove(thiefData);
		}
		else
		{
			thiefData.UpdatePlace(context.Random);
		}
		DomainManager.Extra.SetSectZhujianThiefList(sectZhujianThiefList, context);
		return flag ? thiefData.CatchThiefTimes : (-1);
	}

	[DomainMethod]
	public void CatchThief(sbyte thiefLevel, bool timeOut)
	{
		DomainManager.TaiwuEvent.OnEvent_CatchThief(thiefLevel, timeOut);
	}

	public void CreateNewThief(DataContext context, short areaId)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(9);
		int catchTimes = sectMainStoryEventArgBox.GetInt("ConchShip_PresetKey_ZhujianCatchThiefTimes");
		SectStoryThiefData sectStoryThiefData = CreateThiefData(context.Random, catchTimes, areaId);
		foreach (short thiefBlockId in sectStoryThiefData.ThiefBlockIds)
		{
			MapBlockData block = DomainManager.Map.GetBlock(areaId, thiefBlockId);
			if (!block.Visible)
			{
				block.SetVisible(visible: true, context);
			}
		}
		List<SectStoryThiefData> sectZhujianThiefList = DomainManager.Extra.GetSectZhujianThiefList();
		sectZhujianThiefList.Add(sectStoryThiefData);
		DomainManager.Extra.SetSectZhujianThiefList(sectZhujianThiefList, context);
	}

	private SectStoryThiefData CreateThiefData(IRandomSource random, int catchTimes, short areaId)
	{
		SectStoryThiefData sectStoryThiefData = new SectStoryThiefData
		{
			CatchThiefTimes = catchTimes,
			AreaId = areaId,
			ThiefBlockIds = new List<short>(),
			ThiefTriggered = new List<bool>(),
			RealThiefIndex = random.Next(3)
		};
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		list.Clear();
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		for (int i = 0; i < areaBlocks.Length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			if (SectStoryThiefDataHelper.IsBlockAvailable(mapBlockData))
			{
				list.Add(mapBlockData);
			}
		}
		CollectionUtils.Shuffle(random, list);
		List<MapBlockData> list2 = ObjectPool<List<MapBlockData>>.Instance.Get();
		foreach (MapBlockData item in list)
		{
			DomainManager.Map.GetRealNeighborBlocks(item.AreaId, item.BlockId, list2);
			list2.RemoveAll(SectStoryThiefDataHelper.IsBlockUnAvailable);
			if (list2.Count < 2)
			{
				continue;
			}
			CollectionUtils.Shuffle(random, list2);
			sectStoryThiefData.ThiefBlockIds.Add(item.BlockId);
			sectStoryThiefData.ThiefBlockIds.Add(list2[0].BlockId);
			sectStoryThiefData.ThiefBlockIds.Add(list2[1].BlockId);
			for (int j = 0; j < 3; j++)
			{
				sectStoryThiefData.ThiefTriggered.Add(item: false);
			}
			break;
		}
		if (sectStoryThiefData.ThiefBlockIds.Count == 0)
		{
			PredefinedLog.Show(11, $"Create thief failed in {areaId}");
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		ObjectPool<List<MapBlockData>>.Instance.Return(list2);
		return sectStoryThiefData;
	}

	public bool TryGetThief(Location location, out SectStoryThiefData thiefData, out int thiefIndex)
	{
		List<SectStoryThiefData> sectZhujianThiefList = DomainManager.Extra.GetSectZhujianThiefList();
		foreach (SectStoryThiefData item in sectZhujianThiefList)
		{
			if (item.AreaId != location.AreaId)
			{
				continue;
			}
			for (int i = 0; i < item.ThiefBlockIds.Count; i++)
			{
				if (item.ThiefBlockIds[i] == location.BlockId)
				{
					thiefData = item;
					thiefIndex = i;
					return true;
				}
			}
		}
		thiefData = null;
		thiefIndex = -1;
		return false;
	}

	public void UpdateAreaMerchantType(DataContext context)
	{
		foreach (KeyValuePair<short, sbyte> item2 in DomainManager.Extra.SectZhujianAreaMerchantTypeDict)
		{
			item2.Deconstruct(out var key, out var value);
			short areaTemplateId = key;
			sbyte b = value;
			short areaIdByAreaTemplateId = DomainManager.Map.GetAreaIdByAreaTemplateId(areaTemplateId);
			MapAreaData areaByAreaId = DomainManager.Map.GetAreaByAreaId(areaIdByAreaTemplateId);
			List<SettlementInfo> list = areaByAreaId.SettlementInfos.ToList();
			SettlementInfo[] settlementInfos = areaByAreaId.SettlementInfos;
			for (int i = 0; i < settlementInfos.Length; i++)
			{
				SettlementInfo item = settlementInfos[i];
				Settlement settlement = DomainManager.Organization.GetSettlement(item.SettlementId);
				OrganizationItem organizationItem = Config.Organization.Instance[settlement.GetOrgTemplateId()];
				if (organizationItem.IsSect)
				{
					list.Remove(item);
				}
			}
			CollectionUtils.Shuffle(context.Random, list);
			Dictionary<int, sbyte> dictionary = new Dictionary<int, sbyte>();
			foreach (SettlementInfo item3 in list)
			{
				Settlement settlement2 = DomainManager.Organization.GetSettlement(item3.SettlementId);
				List<int> list2 = new List<int>();
				settlement2.GetMembers().GetAllMembers(list2);
				foreach (int item4 in list2)
				{
					if (DomainManager.Character.TryGetElement_Objects(item4, out var element) && element.IsInteractableAsIntelligentCharacter() && OrganizationDomain.CanInteractWithType(element, 4) && DomainManager.Extra.TryGetMerchantCharToType(item4, out var type))
					{
						dictionary[item4] = type;
					}
				}
			}
			bool flag = false;
			foreach (KeyValuePair<int, sbyte> item5 in dictionary)
			{
				item5.Deconstruct(out var key2, out value);
				int num = key2;
				sbyte b2 = value;
				if (b2 == b)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (dictionary.Count > 0)
				{
					int random = dictionary.Keys.ToList().GetRandom(context.Random);
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(random);
					element_Objects.AddOrSetMerchantType(b, context);
					DomainManager.Map.SetBlockData(context, DomainManager.Map.GetBlock(element_Objects.GetLocation()));
					continue;
				}
				SettlementInfo random2 = list.GetRandom(context.Random);
				Settlement settlement3 = DomainManager.Organization.GetSettlement(random2.SettlementId);
				Location location = settlement3.GetLocation();
				sbyte random3 = Gender.GetRandom(context.Random);
				short age = 16;
				GameData.Domains.Character.Character character = EventHelper.CreateIntelligentCharacter(location, random3, age, -1, random2.SettlementId, 4);
				character.AddOrSetMerchantType(b, context);
				DomainManager.Map.SetBlockData(context, DomainManager.Map.GetBlock(location));
			}
		}
	}

	[DomainMethod]
	public void SpecifyWorldPopulationType(DataContext context, byte worldPopulationType)
	{
		byte worldPopulationType2 = _worldPopulationType;
		SetWorldPopulationType(worldPopulationType, context);
		ChangeWorldPopulation(context, worldPopulationType2);
		DomainManager.Organization.ChangeSettlementStandardPopulations(context, worldPopulationType2);
	}

	public int GetWorldCreationSetting(byte worldCreationType)
	{
		if (1 == 0)
		{
		}
		int num = worldCreationType switch
		{
			1 => GetCombatDifficulty(), 
			11 => DomainManager.Extra.GetEnemyPracticeLevel(), 
			12 => DomainManager.Extra.GetFavorabilityChange(), 
			8 => DomainManager.Extra.GetReadingDifficulty(), 
			9 => DomainManager.Extra.GetBreakoutDifficulty(), 
			10 => DomainManager.Extra.GetLoopingDifficulty(), 
			2 => GetHereticsAmountType(), 
			3 => GetBossInvasionSpeedType(), 
			4 => GetWorldResourceAmountType(), 
			13 => DomainManager.Extra.GetProfessionUpgrade(), 
			14 => DomainManager.Extra.GetLootYield(), 
			_ => -1, 
		};
		if (1 == 0)
		{
		}
		int num2 = num;
		if (num2 < 0)
		{
			PredefinedLog.Show(19, $"GetSettingWorldCreationType by {worldCreationType}");
		}
		return num2;
	}

	public int GetWorldPopulationFactor()
	{
		return WorldCreation.Instance[(byte)5].InfluenceFactors[_worldPopulationType];
	}

	public static int GetWorldPopulationFactor(byte worldPopulationType)
	{
		return WorldCreation.Instance[(byte)5].InfluenceFactors[worldPopulationType];
	}

	public int GetCharacterLifeSpanFactor()
	{
		return WorldCreation.Instance[(byte)0].InfluenceFactors[_characterLifespanType];
	}

	public int GetHereticsAmountFactor()
	{
		return WorldCreation.Instance[(byte)2].InfluenceFactors[_hereticsAmountType];
	}

	public int GetBossInvasionSpeed()
	{
		return WorldCreation.Instance[(byte)3].InfluenceFactors[_bossInvasionSpeedType];
	}

	public int GetGainResourcePercent(byte type)
	{
		return WorldResource.Instance[type].InfluenceFactors[_worldResourceAmountType];
	}

	public (int Value, bool Reciprocal) GetFavorabilityChangePercent(short type, bool isFavorabilityGainFixed)
	{
		if (type < 0)
		{
			return (Value: 100, Reciprocal: false);
		}
		byte favorabilityChange = DomainManager.Extra.GetFavorabilityChange();
		if (isFavorabilityGainFixed)
		{
			type = 6;
		}
		WorldFavorabilityItem worldFavorabilityItem = WorldFavorability.Instance[type];
		return (Value: worldFavorabilityItem.InfluenceFactors[favorabilityChange], Reciprocal: worldFavorabilityItem.NegativeUsingReciprocal);
	}

	public short GetCurrYear()
	{
		return (short)(_currDate / 12);
	}

	public sbyte GetCurrMonthInYear()
	{
		return (sbyte)(_currDate % 12);
	}

	public bool CheckDateInterval(int beginDate, int expectedInterval)
	{
		int num = _currDate - beginDate;
		return num / expectedInterval > 0 && num % expectedInterval == 0;
	}

	public int GetLeftDaysInCurrMonth()
	{
		return DomainManager.Extra.GetTotalActionPointsRemaining() / 10;
	}

	[DomainMethod]
	public void AdvanceDaysInMonth(DataContext context, int days)
	{
		DomainManager.Extra.ConsumeActionPoint(context, days * 10);
	}

	public void ConsumeActionPoint(DataContext context, int actionPoints)
	{
		DomainManager.Extra.ConsumeActionPoint(context, actionPoints);
	}

	[DomainMethod]
	public void AdvanceMonth(DataContext context)
	{
		if (_advancingMonthState != 0)
		{
			Logger.Warn($"{"AdvanceMonth"}: Wrong AdvancingMonthState: {_advancingMonthState}.");
			return;
		}
		Logger.Info("AdvanceMonth: begin ------------------------------------------------------------");
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		var (dataMonitorManager, oriPendingNotifications) = GameData.GameDataBridge.GameDataBridge.StartSemiBlockingTask();
		dataMonitorManager.MonitorData(new DataUid(1, 29, ulong.MaxValue));
		dataMonitorManager.MonitorData(new DataUid(1, 27, ulong.MaxValue));
		_monthlyEventCollection.Clear();
		_currMonthlyNotifications.Clear();
		DomainManager.Merchant.UpdateCaravansMove(context);
		DomainManager.Information.ClearedTaiwuReceivedSecretInformationInMonth();
		CharacterDomain.ClearLockMovementCharSet();
		DomainManager.LegendaryBook.ClearActCrazyShockedCharacters();
		DomainManager.Adventure.RemoveAllTemporaryEnemies(context);
		DomainManager.Adventure.RemoveAllTemporaryIntelligentCharacters(context);
		DomainManager.Building.RemoveTemporaryPossessionCharacter(context);
		DomainManager.Map.ClearHunterAnim(context);
		DomainManager.Information.PrepareSecretInformationAdvanceMonth();
		DomainManager.Character.ResetAllAdvanceMonthStatus();
		AdvanceMonth_CheckPrerequisites(context);
		Events.RaiseAdvanceMonthBegin(context);
		Logger.Info($"New month begin: Year {GetCurrYear() + 1}, Month {GetCurrMonthInYear() + 1} ({_currDate})");
		if (_mainStoryLineProgress >= 3)
		{
			PreAdvanceMonth(context, dataMonitorManager);
			PeriAdvanceMonth(context, dataMonitorManager);
			PostAdvanceMonth(context, dataMonitorManager);
		}
		else
		{
			PreAdvanceMonth_BornArea(context, dataMonitorManager);
			PeriAdvanceMonth_BornArea(context, dataMonitorManager);
			PostAdvanceMonth_BornArea(context, dataMonitorManager);
		}
		GameData.GameDataBridge.GameDataBridge.StopSemiBlockingTask(dataMonitorManager, oriPendingNotifications);
		CharacterDomain.ClearLockMovementCharSet();
		CheckMonthlyEvents(context);
		CheckMonthlyNotifications();
		TransferMonthlyNotifications(context);
		stopwatch.Stop();
		if (stopwatch.ElapsedMilliseconds < 2000)
		{
			Thread.Sleep((int)(2000 - stopwatch.ElapsedMilliseconds));
		}
		SetAdvancingMonthState(20, context);
		Logger.Info("AdvanceMonth: end --------------------------------------------------------------");
	}

	[DomainMethod]
	public void AdvanceMonth_DisplayedMonthlyNotifications(DataContext context, bool saveWorld)
	{
		Logger.Info(saveWorld ? "Exit advancing month state and start saving world." : "Exit advancing month state without saving world.");
		if (_advancingMonthState != 20)
		{
			throw new Exception($"Wrong AdvancingMonthState: {_advancingMonthState}");
		}
		SetAdvancingMonthState(0, context);
		Events.RaiseAdvanceMonthFinish(context);
		Events.ClearPassingLegacyWhileAdvancingMonthHandlers(context);
		RemoveObsoletedInstantNotifications(context);
		if (saveWorld)
		{
			DomainManager.Global.SaveWorld(context);
		}
		UpdatePopulationRelatedData();
		ShowWorldStatistics();
	}

	private void AdvanceMonth_CheckPrerequisites(DataContext context)
	{
		CheckSanity();
		DomainManager.Global.CheckDriveSpace(context);
	}

	public static void CheckSanity()
	{
		DomainManager.Character.CheckCharacterCreationState();
		DomainManager.Character.CheckTemporaryIntelligentCharacters();
		DomainManager.Character.CheckCharacterTemporaryModificationState();
		DomainManager.Character.Test_SoftCheckGroups();
	}

	public void ShowWorldStatistics()
	{
		DomainManager.Character.ShowCharactersStats();
		DomainManager.Character.ShowNonIntelligentCharactersStats();
		EventArgBox.ShowStatus();
		DomainManager.Item.CheckUnownedItems();
		GlobalDomain.ShowMemoryUsage();
	}

	private void SetAndNotifyAdvancingMonthState(DataContext context, sbyte value, DataMonitorManager monitor)
	{
		SetAdvancingMonthState(value, context);
		monitor.CheckMonitoredData();
		GameData.GameDataBridge.GameDataBridge.TransferPendingNotifications();
	}

	private void AddSolarTermNotification(int month)
	{
		switch (month)
		{
		case 0:
			_currMonthlyNotifications.AddSolarTerm0();
			break;
		case 1:
			_currMonthlyNotifications.AddSolarTerm1();
			break;
		case 2:
			_currMonthlyNotifications.AddSolarTerm2();
			break;
		case 3:
			_currMonthlyNotifications.AddSolarTerm3();
			break;
		case 4:
			_currMonthlyNotifications.AddSolarTerm4();
			break;
		case 5:
			_currMonthlyNotifications.AddSolarTerm5();
			break;
		case 6:
			_currMonthlyNotifications.AddSolarTerm6();
			break;
		case 7:
			_currMonthlyNotifications.AddSolarTerm7();
			break;
		case 8:
			_currMonthlyNotifications.AddSolarTerm8();
			break;
		case 9:
			_currMonthlyNotifications.AddSolarTerm9();
			break;
		case 10:
			_currMonthlyNotifications.AddSolarTerm10();
			break;
		case 11:
			_currMonthlyNotifications.AddSolarTerm11();
			break;
		}
	}

	private void PeriAdvanceMonth(DataContext context, DataMonitorManager monitor)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		Location arg;
		bool flag = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Get<Location>("MainStoryLine_SpiritualWanderPlace_TaiwuVillagersCenter", out arg);
		SetAndNotifyAdvancingMonthState(context, 2, monitor);
		DomainManager.Character.AssassinationByJieqing(context);
		WorkerThreadManager.Run(PeriAdvanceMonth_CharacterMixedPoisonEffect, -1, 139, monitor, 100);
		WorkerThreadManager.Run(PeriAdvanceMonth_UpdateCharacterStatus, -2, 139, monitor, 100);
		UpdateLifeDeathGateCharacters(context);
		DomainManager.Character.UpdateIntelligentCharacterAliveStates(context);
		DomainManager.Character.RecoverGuards(context);
		DomainManager.Character.UpdateGroupFavorabilities(context);
		DomainManager.Character.UpdateKidnappedCharacters(context);
		DomainManager.Character.UpdatePregnancyUnlockDates(context);
		DomainManager.Taiwu.UpdateChildrenEducation(context);
		DomainManager.LegendaryBook.UpdateLegendaryBookOwnersStatuses(context);
		DomainManager.Extra.RecoverHunterCarrierAttackCount(context);
		DomainManager.Extra.TryRestoreCharacterAvatars(context);
		DomainManager.Taiwu.AddChoosyRemainUpgradeData(context);
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.UpdateCharacterStatus: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		DomainManager.Extra.MartialArtistSkill3Execute(context, updateData: false);
		SetAndNotifyAdvancingMonthState(context, 3, monitor);
		WorkerThreadManager.Run(DomainManager.Adventure.PreAdvanceMonth_UpdateRandomEnemies, 0, 139, monitor, 100);
		Thread.Sleep(20);
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.UpdateRandomEnemies: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		SetAndNotifyAdvancingMonthState(context, 4, monitor);
		WorkerThreadManager.Run(PeriAdvanceMonth_CharacterSelfImprovement, -1, 139, monitor, 100);
		DomainManager.Taiwu.UpdateReadingProgressOnMonthChange(context);
		DomainManager.Extra.DeleteExpiredReadingStrategiesOnMonthChange(context);
		DomainManager.Taiwu.ClearActiveReadingProgressOnMonthChange(context);
		DomainManager.Taiwu.ClearActiveNeigongLoopingProgressOnMonthChange(context);
		DomainManager.Taiwu.ResetLoopInCombatCounts(context);
		DomainManager.Taiwu.ClearExpiredQiArtStrategies(context);
		DomainManager.Taiwu.TryAddLoopingEvent(context, GlobalConfig.Instance.BaseLoopingEventProbability);
		DomainManager.Taiwu.GenerateAllFollowingMonthNotifications();
		DomainManager.Extra.UpdateSeniorityForCharacterProfessions(context);
		WorkerThreadManager.Run(PeriAdvanceMonth_CharacterSelfImprovement_PracticeAndBreakout, -1, 139, monitor, 100);
		WorkerThreadManager.Run(PeriAdvanceMonth_CharacterSelfImprovement_LearnNewSkills, -1, 139, monitor, 100);
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.CharacterSelfImprovement: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		SetAndNotifyAdvancingMonthState(context, 5, monitor);
		WorkerThreadManager.Run(PeriAdvanceMonth_CharacterActivePreparation_GetSupply, -1, 135, monitor, 100);
		WorkerThreadManager.Run(PeriAdvanceMonth_CharacterActivePreparation, -1, 139, monitor, 100);
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.CharacterActivePreparation: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		SetAndNotifyAdvancingMonthState(context, 6, monitor);
		DomainManager.Item.UpdateCrickets(context);
		WorkerThreadManager.Run(PeriAdvanceMonth_CharacterPassivePreparation, -1, 139, monitor, 100);
		DomainManager.Taiwu.UpdateVillagerTreasuryNeed(context);
		DomainManager.Taiwu.LoseOverloadResources(context);
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.CharacterPassivePreparation: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		SetAndNotifyAdvancingMonthState(context, 7, monitor);
		WorkerThreadManager.Run(PeriAdvanceMonth_CharacterRelationsUpdate, -1, 139, monitor, 100);
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.CharacterRelationsUpdate: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		DomainManager.Character.UpdateDistantMarriages(context);
		DomainManager.Character.UpdateAdoreRelationsInMarriage(context);
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.UpdateAdoreRelationsInMarriage: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		DomainManager.Organization.ExpandAllFactions(context);
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.ExpandAllFactions: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		SetAndNotifyAdvancingMonthState(context, 8, monitor);
		WorkerThreadManager.Run(PeriAdvanceMonth_CharacterPersonalNeedsProcessing, -1, 139, monitor, 100);
		Thread.Sleep(20);
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.CharacterPersonalNeedsProcessing: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		DomainManager.Character.UpdateInfectedCharacterActions(context);
		DomainManager.LegendaryBook.UpdateLegendaryBookOwnersActions(context);
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.UpdateInfectedCharacterActions: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		if (!flag)
		{
			DomainManager.Character.PrepareForPrioritizedAction(context);
			SetAndNotifyAdvancingMonthState(context, 9, monitor);
			WorkerThreadManager.Run(PeriAdvanceMonth_CharacterPrioritizedAction, 0, 139, monitor, 100);
		}
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.CharacterPrioritizedAction: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		if (!flag)
		{
			SetAndNotifyAdvancingMonthState(context, 10, monitor);
			WorkerThreadManager.Run(PeriAdvanceMonth_CharacterGeneralAction, -1, 139, monitor, 100);
		}
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.CharacterGeneralAction: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		if (!flag)
		{
			SetAndNotifyAdvancingMonthState(context, 11, monitor);
			WorkerThreadManager.Run(PeriAdvanceMonth_CharacterFixedAction, 0, 139, monitor, 100);
			DomainManager.Organization.UpdateFugitiveGroupsOnAdvanceMonth(context);
			DomainManager.Character.UpdateExceedingGroupChars(context);
			DomainManager.Taiwu.UpdateVillagerFixedActions(context);
		}
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.CharacterFixedAction: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		DomainManager.Taiwu.MoveVillagersToWorkLocation(context);
		DomainManager.Character.RecoverSkillBookLibraries(context);
		DomainManager.Extra.UpdateItemPriceFluctuations(context);
		stopwatch.Stop();
		Logger.Info($"PeriAdvanceMonth.UpdateMisc: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
	}

	private void PeriAdvanceMonth_BornArea(DataContext context, DataMonitorManager monitor)
	{
		SetAndNotifyAdvancingMonthState(context, 2, monitor);
		PeriAdvanceMonth_UpdateCharacterStatus_TaiwuGroup(context);
		PeriAdvanceMonth_UpdateCharacterStatus(context, 135);
		context.ParallelModificationsRecorder.ApplyAll(context);
		DomainManager.Taiwu.UpdateTaiwuGroupFavorabilities(context);
		DomainManager.Taiwu.UpdateChildrenEducation(context);
		Thread.Sleep(50);
		SetAndNotifyAdvancingMonthState(context, 4, monitor);
		PeriAdvanceMonth_CharacterSelfImprovement_TaiwuGroup(context);
		context.ParallelModificationsRecorder.ApplyAll(context);
		DomainManager.Taiwu.UpdateReadingProgressOnMonthChange(context);
		DomainManager.Extra.DeleteExpiredReadingStrategiesOnMonthChange(context);
		DomainManager.Taiwu.ClearActiveNeigongLoopingProgressOnMonthChange(context);
		DomainManager.Taiwu.ClearActiveReadingProgressOnMonthChange(context);
		DomainManager.Taiwu.ClearExpiredQiArtStrategies(context);
		DomainManager.Taiwu.TryAddLoopingEvent(context, GlobalConfig.Instance.BaseLoopingEventProbability);
		PeriAdvanceMonth_CharacterSelfImprovement_PracticeAndBreakout_TaiwuGroup(context);
		context.ParallelModificationsRecorder.ApplyAll(context);
		Thread.Sleep(50);
		SetAndNotifyAdvancingMonthState(context, 5, monitor);
		PeriAdvanceMonth_CharacterActivePreparation_TaiwuGroup(context);
		context.ParallelModificationsRecorder.ApplyAll(context);
		Thread.Sleep(50);
		SetAndNotifyAdvancingMonthState(context, 6, monitor);
		PeriAdvanceMonth_CharacterPassivePreparation_TaiwuGroup(context);
		PeriAdvanceMonth_CharacterPassivePreparation(context, 135);
		context.ParallelModificationsRecorder.ApplyAll(context);
		Thread.Sleep(50);
		DomainManager.Item.UpdateCrickets(context);
		DomainManager.Taiwu.LoseOverloadResources(context);
		SetAndNotifyAdvancingMonthState(context, 7, monitor);
		PeriAdvanceMonth_CharacterRelationsUpdate_TaiwuGroup(context);
		context.ParallelModificationsRecorder.ApplyAll(context);
		Thread.Sleep(50);
		SetAndNotifyAdvancingMonthState(context, 8, monitor);
		PeriAdvanceMonth_CharacterPersonalNeedsProcessing_TaiwuGroup(context);
		context.ParallelModificationsRecorder.ApplyAll(context);
		Thread.Sleep(50);
		SetAndNotifyAdvancingMonthState(context, 10, monitor);
		PeriAdvanceMonth_CharacterGeneralAction_TaiwuGroup(context);
		context.ParallelModificationsRecorder.ApplyAll(context);
		Thread.Sleep(50);
		SetAndNotifyAdvancingMonthState(context, 11, monitor);
		PeriAdvanceMonth_CharacterFixedAction_TaiwuGroup(context);
		context.ParallelModificationsRecorder.ApplyAll(context);
		Thread.Sleep(50);
	}

	private static void PeriAdvanceMonth_CharacterMixedPoisonEffect(DataContext context, int areaId)
	{
		if (areaId < 0)
		{
			PeriAdvanceMonth_CharacterMixedPoisonEffect_TaiwuGroup(context);
			return;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
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
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.PeriAdvanceMonth_MixedPoisonEffect(context);
				if (element_Objects.IsActiveExternalRelationState(2))
				{
					PeriAdvanceMonth_CharacterMixedPoisonEffect_KidnappedChars(context, item);
				}
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterMixedPoisonEffect_TaiwuGroup(DataContext context)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			element_Objects.PeriAdvanceMonth_MixedPoisonEffect(context);
			if (element_Objects.IsActiveExternalRelationState(2))
			{
				PeriAdvanceMonth_CharacterMixedPoisonEffect_KidnappedChars(context, item);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterMixedPoisonEffect_KidnappedChars(DataContext context, int kidnapperCharId)
	{
		List<KidnappedCharacter> collection = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
		int i = 0;
		for (int count = collection.Count; i < count; i++)
		{
			int charId = collection[i].CharId;
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			element_Objects.PeriAdvanceMonth_MixedPoisonEffect(context);
		}
	}

	private static void PeriAdvanceMonth_UpdateCharacterStatus(DataContext context, int areaId)
	{
		switch (areaId)
		{
		case -1:
			PeriAdvanceMonth_UpdateCharacterStatus_TaiwuGroup(context);
			return;
		case -2:
			PeriAdvanceMonth_UpdateCharacterStatus_HiddenCharacters(context);
			return;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
		int i = 0;
		for (int length = areaBlocks.Length; i < length; i++)
		{
			HashSet<int> characterSet = areaBlocks[i].CharacterSet;
			if (characterSet != null)
			{
				foreach (int item in characterSet)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					element_Objects.PeriAdvanceMonth_UpdateStatus(context);
					if (element_Objects.IsActiveExternalRelationState(2))
					{
						PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, item);
					}
				}
			}
			characterSet = areaBlocks[i].InfectedCharacterSet;
			if (characterSet == null)
			{
				continue;
			}
			foreach (int item2 in characterSet)
			{
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2);
				element_Objects2.PeriAdvanceMonth_UpdateStatus(context);
				if (element_Objects2.IsActiveExternalRelationState(2))
				{
					PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, item2);
				}
			}
		}
	}

	private static void PeriAdvanceMonth_UpdateCharacterStatus_HiddenCharacters(DataContext context)
	{
		List<int> list = new List<int>();
		DomainManager.Character.GetCrossAreaTravelingCharacterIds(list);
		foreach (int item in list)
		{
			if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
			{
				continue;
			}
			if (element.GetLeaderId() == item)
			{
				PeriAdvanceMonth_UpdateCharacterStatus_HiddenGroupChars(context, item);
			}
			if (!element.IsActiveExternalRelationState(60))
			{
				element.PeriAdvanceMonth_UpdateStatus(context);
				if (element.IsActiveExternalRelationState(2))
				{
					PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, item);
				}
			}
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		HashSet<int> hashSet = new HashSet<int>();
		DomainManager.TaiwuEvent.CollectUnreleasedCalledCharacters(hashSet);
		foreach (int item2 in hashSet)
		{
			if (!DomainManager.Character.TryGetElement_Objects(item2, out var element2) || !element2.IsActiveExternalRelationState(60) || element2.GetKidnapperId() >= 0 || element2.GetLeaderId() == taiwuCharId)
			{
				continue;
			}
			Location location = element2.GetLocation();
			if (location.IsValid())
			{
				MapBlockData block = DomainManager.Map.GetBlock(location);
				if ((block.CharacterSet != null && block.CharacterSet.Contains(item2)) || (block.InfectedCharacterSet != null && block.InfectedCharacterSet.Contains(item2)))
				{
					continue;
				}
			}
			element2.PeriAdvanceMonth_UpdateStatus(context);
			if (element2.IsActiveExternalRelationState(2))
			{
				PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, item2);
			}
		}
		for (sbyte b = 0; b < 15; b++)
		{
			sbyte largeSectTemplateId = OrganizationDomain.GetLargeSectTemplateId(b);
			Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(largeSectTemplateId);
			for (int num = sect.Prison.Prisoners.Count - 1; num >= 0; num--)
			{
				SettlementPrisoner settlementPrisoner = sect.Prison.Prisoners[num];
				if (DomainManager.Character.TryGetElement_Objects(settlementPrisoner.CharId, out var element3))
				{
					element3.PeriAdvanceMonth_UpdateStatus(context);
				}
			}
		}
	}

	private static void PeriAdvanceMonth_UpdateCharacterStatus_TaiwuGroup(DataContext context)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			element_Objects.PeriAdvanceMonth_UpdateStatus(context);
			if (element_Objects.IsActiveExternalRelationState(2))
			{
				PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, item);
			}
		}
	}

	private static void PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(DataContext context, int kidnapperCharId)
	{
		List<KidnappedCharacter> collection = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
		int i = 0;
		for (int count = collection.Count; i < count; i++)
		{
			int charId = collection[i].CharId;
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			element_Objects.PeriAdvanceMonth_UpdateStatus(context);
		}
	}

	private static void PeriAdvanceMonth_UpdateCharacterStatus_HiddenGroupChars(DataContext context, int charId)
	{
		Tester.Assert(!DomainManager.Character.GetElement_Objects(charId).GetLocation().IsValid());
		HashSet<int> collection = DomainManager.Character.GetGroup(charId).GetCollection();
		foreach (int item in collection)
		{
			if (item == charId)
			{
				continue;
			}
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (!element_Objects.IsActiveExternalRelationState(60))
			{
				element_Objects.PeriAdvanceMonth_UpdateStatus(context);
				if (element_Objects.IsActiveExternalRelationState(2))
				{
					PeriAdvanceMonth_UpdateCharacterStatus_KidnappedChars(context, item);
				}
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterSelfImprovement(DataContext context, int areaId)
	{
		if (areaId < 0)
		{
			PeriAdvanceMonth_CharacterSelfImprovement_TaiwuGroup(context);
			return;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
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
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.PeriAdvanceMonth_SelfImprovement(context);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterSelfImprovement_TaiwuGroup(DataContext context)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (item != DomainManager.Taiwu.GetTaiwuCharId())
			{
				element_Objects.PeriAdvanceMonth_SelfImprovement(context);
			}
			else
			{
				element_Objects.PeriAdvanceMonth_SelfImprovement_Taiwu(context);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterSelfImprovement_PracticeAndBreakout(DataContext context, int areaId)
	{
		if (areaId < 0)
		{
			PeriAdvanceMonth_CharacterSelfImprovement_PracticeAndBreakout_TaiwuGroup(context);
			return;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
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
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				context.Equipping.ParallelPracticeAndBreakoutCombatSkills(context, element_Objects);
				context.Equipping.ParallelUpdateBreakPlateBonuses(context, element_Objects);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterSelfImprovement_PracticeAndBreakout_TaiwuGroup(DataContext context)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (item != DomainManager.Taiwu.GetTaiwuCharId())
			{
				context.Equipping.ParallelPracticeAndBreakoutCombatSkills(context, element_Objects);
				context.Equipping.ParallelUpdateBreakPlateBonuses(context, element_Objects);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterSelfImprovement_LearnNewSkills(DataContext context, int areaId)
	{
		if (areaId < 0)
		{
			PeriAdvanceMonth_CharacterSelfImprovement_LearnNewSkills_TaiwuGroup(context);
			return;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
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
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.PeriAdvanceMonth_SelfImprovement_LearnNewSkills(context);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterSelfImprovement_LearnNewSkills_TaiwuGroup(DataContext context)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (item != DomainManager.Taiwu.GetTaiwuCharId())
			{
				element_Objects.PeriAdvanceMonth_SelfImprovement_LearnNewSkills(context);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterActivePreparation_GetSupply(DataContext context, int areaId)
	{
		if (areaId < 0)
		{
			PeriAdvanceMonth_CharacterActivePreparation_GetSupply_TaiwuGroup(context);
			return;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
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
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.PeriAdvanceMonth_ActivePreparation_GetSupply(context);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterActivePreparation_GetSupply_TaiwuGroup(DataContext context)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (item != DomainManager.Taiwu.GetTaiwuCharId())
			{
				element_Objects.PeriAdvanceMonth_ActivePreparation_GetSupply(context);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterActivePreparation(DataContext context, int areaId)
	{
		if (areaId < 0)
		{
			PeriAdvanceMonth_CharacterActivePreparation_TaiwuGroup(context);
			return;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
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
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.PeriAdvanceMonth_ActivePreparation(context);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterActivePreparation_TaiwuGroup(DataContext context)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (item != DomainManager.Taiwu.GetTaiwuCharId())
			{
				element_Objects.PeriAdvanceMonth_ActivePreparation(context);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterPassivePreparation(DataContext context, int areaId)
	{
		if (areaId < 0)
		{
			PeriAdvanceMonth_CharacterPassivePreparation_TaiwuGroup(context);
			return;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
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
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.PeriAdvanceMonth_PassivePreparation(context);
				if (element_Objects.IsActiveExternalRelationState(2))
				{
					PeriAdvanceMonth_CharacterPassivePreparation_KidnappedChars(context, item);
				}
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterPassivePreparation_TaiwuGroup(DataContext context)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			element_Objects.PeriAdvanceMonth_PassivePreparation(context);
			if (element_Objects.IsActiveExternalRelationState(2))
			{
				PeriAdvanceMonth_CharacterPassivePreparation_KidnappedChars(context, item);
			}
		}
		Dictionary<int, GearMate>.KeyCollection allGearMateId = DomainManager.Extra.GetAllGearMateId();
		foreach (int item2 in allGearMateId)
		{
			DomainManager.Character.GetElement_Objects(item2).PeriAdvanceMonth_GearMateLoseOverLoadedItems(context);
		}
	}

	private static void PeriAdvanceMonth_CharacterPassivePreparation_KidnappedChars(DataContext context, int kidnapperCharId)
	{
		List<KidnappedCharacter> collection = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
		int i = 0;
		for (int count = collection.Count; i < count; i++)
		{
			int charId = collection[i].CharId;
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			element_Objects.PeriAdvanceMonth_PassivePreparation(context);
		}
	}

	private static void PeriAdvanceMonth_CharacterRelationsUpdate(DataContext context, int areaId)
	{
		if (areaId < 0)
		{
			PeriAdvanceMonth_CharacterRelationsUpdate_TaiwuGroup(context);
			return;
		}
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
		if (location.AreaId == areaId)
		{
			hashSet.Clear();
			hashSet.UnionWith(DomainManager.Taiwu.GetGroupCharIds().GetCollection());
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
		int i = 0;
		for (int length = areaBlocks.Length; i < length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			HashSet<int> characterSet = mapBlockData.CharacterSet;
			if (characterSet == null)
			{
				continue;
			}
			HashSet<int> hashSet2 = characterSet;
			if (location.AreaId == mapBlockData.AreaId && location.BlockId == mapBlockData.BlockId)
			{
				hashSet2 = hashSet;
				hashSet2.UnionWith(characterSet);
			}
			foreach (int item in characterSet)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.PeriAdvanceMonth_RelationsUpdate(context, hashSet2);
				if (element_Objects.IsActiveExternalRelationState(2))
				{
					PeriAdvanceMonth_CharacterRelationsUpdate_KidnappedChars(context, item);
				}
			}
		}
		ObjectPool<HashSet<int>>.Instance.Return(hashSet);
	}

	private static void PeriAdvanceMonth_CharacterRelationsUpdate_TaiwuGroup(DataContext context)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
		hashSet.Clear();
		hashSet.UnionWith(collection);
		if (location.IsValid())
		{
			MapBlockData block = DomainManager.Map.GetBlock(location);
			if (block.CharacterSet != null)
			{
				hashSet.UnionWith(block.CharacterSet);
			}
		}
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (item != DomainManager.Taiwu.GetTaiwuCharId())
			{
				element_Objects.PeriAdvanceMonth_RelationsUpdate(context, hashSet);
			}
			if (element_Objects.IsActiveExternalRelationState(2))
			{
				PeriAdvanceMonth_CharacterRelationsUpdate_KidnappedChars(context, item);
			}
		}
		ObjectPool<HashSet<int>>.Instance.Return(hashSet);
	}

	private static void PeriAdvanceMonth_CharacterRelationsUpdate_KidnappedChars(DataContext context, int kidnapperCharId)
	{
		List<KidnappedCharacter> collection = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
		int i = 0;
		for (int count = collection.Count; i < count; i++)
		{
			int charId = collection[i].CharId;
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		}
	}

	private static void PeriAdvanceMonth_CharacterPrioritizedAction(DataContext context, int areaId)
	{
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
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
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.PeriAdvanceMonth_ExecutePrioritizedAction(context);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterPersonalNeedsProcessing(DataContext context, int areaId)
	{
		if (areaId < 0)
		{
			PeriAdvanceMonth_CharacterPersonalNeedsProcessing_TaiwuGroup(context);
			return;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
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
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.PeriAdvanceMonth_PersonalNeedsProcessing(context);
				if (element_Objects.IsActiveExternalRelationState(2))
				{
					PeriAdvanceMonth_CharacterPersonalNeedsProcessing_KidnappedChars(context, item);
				}
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterPersonalNeedsProcessing_TaiwuGroup(DataContext context)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			element_Objects.PeriAdvanceMonth_PersonalNeedsProcessing(context);
			if (element_Objects.IsActiveExternalRelationState(2))
			{
				PeriAdvanceMonth_CharacterPersonalNeedsProcessing_KidnappedChars(context, item);
			}
		}
	}

	private static void PeriAdvanceMonth_CharacterPersonalNeedsProcessing_KidnappedChars(DataContext context, int kidnapperCharId)
	{
		List<KidnappedCharacter> collection = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
		int i = 0;
		for (int count = collection.Count; i < count; i++)
		{
			int charId = collection[i].CharId;
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			element_Objects.PeriAdvanceMonth_PersonalNeedsProcessing(context);
		}
	}

	private static void PeriAdvanceMonth_CharacterGeneralAction(DataContext context, int areaId)
	{
		if (areaId < 0)
		{
			PeriAdvanceMonth_CharacterGeneralAction_TaiwuGroup(context);
			return;
		}
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		HashSet<int> obj = context.AdvanceMonthRelatedData.BlockCharSet.Occupy();
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
		int i = 0;
		for (int length = areaBlocks.Length; i < length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			HashSet<int> characterSet = mapBlockData.CharacterSet;
			if (characterSet == null)
			{
				continue;
			}
			obj.Clear();
			CharacterDomain.UnionWithInteractableCharacters(obj, characterSet);
			if (location.AreaId == mapBlockData.AreaId && location.BlockId == mapBlockData.BlockId)
			{
				CharacterDomain.UnionWithInteractableCharacters(obj, DomainManager.Taiwu.GetGroupCharIds().GetCollection());
			}
			foreach (int item in characterSet)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.PeriAdvanceMonth_ExecuteGeneralAction(context, obj, mapBlockData.GraveSet);
			}
		}
		context.AdvanceMonthRelatedData.BlockCharSet.Release(ref obj);
	}

	private static void PeriAdvanceMonth_CharacterGeneralAction_TaiwuGroup(DataContext context)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		HashSet<int> obj = context.AdvanceMonthRelatedData.BlockCharSet.Occupy();
		obj.UnionWith(collection);
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (block.CharacterSet != null)
		{
			CharacterDomain.UnionWithInteractableCharacters(obj, block.CharacterSet);
		}
		HashSet<int> graveSet = block.GraveSet;
		foreach (int item in collection)
		{
			if (item != DomainManager.Taiwu.GetTaiwuCharId())
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				element_Objects.PeriAdvanceMonth_ExecuteGeneralAction(context, obj, graveSet);
			}
		}
		context.AdvanceMonthRelatedData.BlockCharSet.Release(ref obj);
	}

	private static void PeriAdvanceMonth_CharacterFixedAction(DataContext context, int areaId)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		HashSet<int> obj = context.AdvanceMonthRelatedData.BlockCharSet.Occupy();
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
		int i = 0;
		for (int length = areaBlocks.Length; i < length; i++)
		{
			MapBlockData mapBlockData = areaBlocks[i];
			HashSet<int> characterSet = mapBlockData.CharacterSet;
			obj.Clear();
			if (characterSet != null)
			{
				CharacterDomain.UnionWithInteractableCharacters(obj, characterSet);
			}
			if (location.AreaId == mapBlockData.AreaId && location.BlockId == mapBlockData.BlockId)
			{
				HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
				CharacterDomain.UnionWithInteractableCharacters(obj, collection);
				foreach (int item in collection)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					if (!element_Objects.IsTaiwu())
					{
						element_Objects.PeriAdvanceMonth_ExecuteFixedActions(context, obj);
					}
				}
			}
			if (characterSet == null)
			{
				continue;
			}
			foreach (int item2 in characterSet)
			{
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2);
				element_Objects2.PeriAdvanceMonth_ExecuteFixedActions(context, obj);
			}
		}
		context.AdvanceMonthRelatedData.BlockCharSet.Release(ref obj);
	}

	private static void PeriAdvanceMonth_CharacterFixedAction_TaiwuGroup(DataContext context)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		HashSet<int> obj = context.AdvanceMonthRelatedData.BlockCharSet.Occupy();
		obj.UnionWith(collection);
		MapBlockData block = DomainManager.Map.GetBlock(location);
		if (block.CharacterSet != null)
		{
			obj.UnionWith(block.CharacterSet);
		}
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.GetId() != DomainManager.Taiwu.GetTaiwuCharId())
			{
				element_Objects.PeriAdvanceMonth_ExecuteFixedActions(context, obj);
			}
		}
		context.AdvanceMonthRelatedData.BlockCharSet.Release(ref obj);
	}

	private static void TestCreateChildren(DataContext context)
	{
		List<(GameData.Domains.Character.Character, GameData.Domains.Character.Character)> list = new List<(GameData.Domains.Character.Character, GameData.Domains.Character.Character)>();
		float num = 0.01f * DomainManager.World.GetProbAdjustOfCreatingCharacter();
		for (short num2 = 0; num2 < 135; num2++)
		{
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(num2);
			int i = 0;
			for (int length = areaBlocks.Length; i < length; i++)
			{
				HashSet<int> characterSet = areaBlocks[i].CharacterSet;
				if (characterSet == null)
				{
					continue;
				}
				foreach (int item2 in characterSet)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
					if (element_Objects.GetGender() == 0 && element_Objects.GetAgeGroup() == 2)
					{
						HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(item2, 1024);
						int aliveSpouse = DomainManager.Character.GetAliveSpouse(item2);
						if ((relatedCharIds.Count <= 0 || aliveSpouse >= 0) && !(context.Random.NextFloat() >= num))
						{
							GameData.Domains.Character.Character item = ((aliveSpouse >= 0) ? DomainManager.Character.GetElement_Objects(aliveSpouse) : null);
							list.Add((element_Objects, item));
						}
					}
				}
			}
		}
		foreach (var (mother, father) in list)
		{
			DomainManager.Character.TestAddPregnantState(context, mother, father);
			DomainManager.Character.ParallelCreateNewbornChildren(context, mother, isDystocia: false, isMotherDead: false);
		}
		context.ParallelModificationsRecorder.ApplyAll(context);
	}

	private static void TestGenerateLifeRecords(DataContext context)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		DomainManager.LifeRecord.InitializeTestRelatedData();
		for (short num = 0; num < 135; num++)
		{
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(num);
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
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					DomainManager.LifeRecord.AddRandomLifeRecord(context, lifeRecordCollection, element_Objects, areaBlocks[i]);
				}
			}
		}
	}

	private void TestGenerateMonthlyNotifications(DataContext context)
	{
		MonthlyNotificationCollection monthlyNotificationCollection = GetMonthlyNotificationCollection();
		PrepareTestMonthlyNotificationRelatedData();
		int num = context.Random.Next(6, 25);
		for (int i = 0; i < num; i++)
		{
			AddRandomMonthlyNotification(context, monthlyNotificationCollection);
		}
	}

	private void TestGenerateInstantNotifications(DataContext context)
	{
		InstantNotificationCollection instantNotificationCollection = GetInstantNotificationCollection();
		PrepareTestInstantNotificationRelatedData();
		int num = context.Random.Next(15, 31);
		for (int i = 0; i < num; i++)
		{
			AddRandomInstantNotification(context, instantNotificationCollection);
		}
	}

	private void PostAdvanceMonth(DataContext context, DataMonitorManager monitor)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		Location arg;
		bool flag = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox().Get<Location>("MainStoryLine_SpiritualWanderPlace_TaiwuVillagersCenter", out arg);
		Events.RaisePostAdvanceMonthBegin(context);
		stopwatch.Stop();
		Logger.Info($"DynamicEvents: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		SetAndNotifyAdvancingMonthState(context, 12, monitor);
		DomainManager.Information.ProcessAdvanceMonth(context);
		stopwatch.Stop();
		Logger.Info($"Information: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		SetAndNotifyAdvancingMonthState(context, 13, monitor);
		WorkerThreadManager.Run(PostAdvanceMonth_CharacterPersonalNeedsUpdate, -1, 139, monitor, 100);
		Thread.Sleep(20);
		stopwatch.Stop();
		Logger.Info($"PostAdvanceMonth.CharacterPersonalNeedsUpdate: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		DomainManager.Character.UpdateLuckEvents(context);
		stopwatch.Stop();
		Logger.Info($"PostAdvanceMonth.UpdateLuckEvents: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		DomainManager.Merchant.GenTradeCaravansOnAdvanceMonth(context);
		DomainManager.Merchant.CaravanMonthEvent(context);
		stopwatch.Stop();
		Logger.Info($"PostAdvanceMonth.GenTradeCarvansOnAdvanceMonth: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		DomainManager.Adventure.UpdateAdventuresInAllAreas(context);
		DomainManager.Adventure.TryCreateElopeWithLove(context);
		WorkerThreadManager.Run(MapDomain.ParallelUpdateOnMonthChange, 0, 45, monitor, 100);
		WorkerThreadManager.Run(MapDomain.ParallelUpdateBrokenBlockOnMonthChange, 45, 135, monitor, 100);
		DomainManager.Map.UpdateCricketPlaceData(context);
		DomainManager.Extra.UpdateMapBlockRecoveryUnlockDates(context);
		stopwatch.Stop();
		Logger.Info($"UpdateMapAndAdventures: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		if (_mainStoryLineProgress >= 25)
		{
			DomainManager.Character.GenerateSkeletons(context);
		}
		DomainManager.TaiwuEvent.XiangshuMinionSurroundTaiwuVillage();
		DomainManager.Character.UpdateInfectedCharacterMovements(context);
		DomainManager.Character.UpdateLegendaryBookInsaneCharacterMovements(context);
		DomainManager.Character.UpdateFixedCharacterMovements(context);
		DomainManager.Character.UpdateFixedCharacterEatingItems(context);
		if (!flag)
		{
			DomainManager.Character.UpdateIntelligentCharacterMovements(context);
		}
		DomainManager.Extra.UpdateTaiwuTeammateTaming(context);
		WorkerThreadManager.Run(DomainManager.Extra.PostAdvanceMonth_UpdateNpcTaming, 0, 139, monitor, 100);
		Thread.Sleep(20);
		DomainManager.Taiwu.CheckAboutToDieVillagersAndTaiwuPeople(context);
		stopwatch.Stop();
		Logger.Info($"UpdateCharacterMovements: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		if (DomainManager.World.GetWorldFunctionsStatus(10))
		{
			DomainManager.Building.UpdateBrokenBuildings(context);
			DomainManager.Building.ParallelUpdate(context);
			context.ParallelModificationsRecorder.ApplyAll(context);
			DomainManager.Taiwu.MakeVillagerWorkSettlementsVisited(context);
			DomainManager.Taiwu.UpdateVillagerRoleNewClothing(context);
			DomainManager.Extra.UpdateBuildingAreaEffectProgresses(context);
			DomainManager.Building.FeastAdvanceMonth_Complement(context);
			short availableVillagerCount = DomainManager.Taiwu.GetAvailableVillagerCount();
			if (availableVillagerCount > 0)
			{
				short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
				InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
				instantNotificationCollection.AddTaiwuVillageIdleCount(taiwuVillageSettlementId, availableVillagerCount);
			}
		}
		stopwatch.Stop();
		Logger.Info($"UpdateBuildings: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		DomainManager.Organization.UpdateOrganizationMembers(context);
		stopwatch.Stop();
		Logger.Info($"UpdateOrganizationMembers: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		DomainManager.Extra.UpdateAnimalAreaData(context);
		DomainManager.SpecialEffect.ApplyBrokenEffectChangedDuringAdvance(context);
		DomainManager.Organization.Test_CheckFactions();
		DomainManager.Extra.UpdateWorldCharacterTitles(context);
		DomainManager.Extra.UpdateCharacterTemporaryFeatures(context);
		DomainManager.Extra.UpdateAiActionCooldowns(context);
		DomainManager.Extra.UpdateTaiwuTaming(context);
		DomainManager.Extra.GearMateUpdateStatus(context);
		DomainManager.LegendaryBook.CreateLegendaryBooksAccordingToXiangshuProgress(context);
		DomainManager.TaiwuEvent.HandleMonthlyActions();
		ProfessionSkillHandle.OnPostAdvanceMonth(context);
		AdvanceMonth_SectMainStory(context);
		DomainManager.Taiwu.AddJieqingPunishmentMonthlyEvent(context);
		DomainManager.Taiwu.UpdateFollowingNotifications(context);
		DomainManager.Extra.TriggerTaiwuVillageVowMonthlyEvent(context);
		DomainManager.Extra.RemoveOverdueCityPunishmentSeverityCustomizeData(context);
		DomainManager.Extra.UpdateSpecialCustomizedSeverity(context);
		DomainManager.Extra.UpdateNpcArtisanOrderProgress(context);
		DomainManager.Extra.UpdateBuildingArtisanOrderProgress(context);
		DomainManager.Extra.UpdateResourceBlockBuildingCoreProducing(context);
		DomainManager.Extra.MapPickupsPostAdvanceMonth(context);
		DlcManager.OnPostAdvanceMonth(context);
		DlcManager.AddMonthlyEventHappyNewYear2026();
		stopwatch.Stop();
		Logger.Info($"PostAdvanceMonth.AdjustMisc: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		DomainManager.Character.PostAdvanceMonthCalcDarkAsh(context);
		stopwatch.Stop();
		Logger.Info($"PostAdvanceMonth.CalcDarkAsh: {stopwatch.Elapsed.TotalMilliseconds:N1}");
		stopwatch.Restart();
		PostAdvanceMonth_ClearRedundantData(context);
		PostAdvanceMonth_SetEventGlobalArgs(context);
		stopwatch.Stop();
		Logger.Info($"PostAdvanceMonth.Clean: {stopwatch.Elapsed.TotalMilliseconds:N1}");
	}

	private void PostAdvanceMonth_BornArea(DataContext context, DataMonitorManager monitor)
	{
		Events.RaisePostAdvanceMonthBegin(context);
		SetAndNotifyAdvancingMonthState(context, 13, monitor);
		PostAdvanceMonth_CharacterPersonalNeedsUpdate_TaiwuGroup(context);
		PostAdvanceMonth_CharacterPersonalNeedsUpdate(context, 135);
		context.ParallelModificationsRecorder.ApplyAll(context);
		MapDomain.ParallelUpdateOnMonthChange(context, 135);
		context.ParallelModificationsRecorder.ApplyAll(context);
		Thread.Sleep(50);
		DomainManager.Adventure.UpdateAdventuresInAllAreas(context);
		DomainManager.Information.ProcessAdvanceMonth(context);
		DomainManager.SpecialEffect.ApplyBrokenEffectChangedDuringAdvance(context);
		DomainManager.Extra.UpdateTaiwuTaming(context);
		DlcManager.AddMonthlyEventHappyNewYear2026();
		PostAdvanceMonth_ClearRedundantData(context);
		PostAdvanceMonth_SetEventGlobalArgs(context);
		if (DomainManager.TutorialChapter.InGuiding && DomainManager.TutorialChapter.GetTutorialChapter() == 2)
		{
			DomainManager.Building.TutorialUpdate(context);
		}
	}

	private static void PostAdvanceMonth_SetEventGlobalArgs(DataContext context)
	{
		EventHelper.SaveArgToSectMainStory(6, "EnteredShixiangDrumEasterEggThisMonth", value: false);
	}

	private static void PostAdvanceMonth_ClearRedundantData(DataContext context)
	{
		DomainManager.Taiwu.ClearAdvanceMonthData();
		if (DomainManager.World.GetCurrMonthInYear() == 0)
		{
			DomainManager.Character.TryRemoveRecentDeadCharacters(context);
			DomainManager.Character.TryRemoveDeadCharacters(context);
			short currYear = DomainManager.World.GetCurrYear();
			if (currYear % 10 == 0)
			{
				DomainManager.Character.RemoveObsoleteActualBloodParents(context);
			}
		}
		DomainManager.Merchant.RemoveObsoleteMerchantData(context);
		DomainManager.Merchant.SetVillagerRoleMerchantType(context);
		WorkerThreadManager.RunPostAction(RemoveWorldItemsToBeRemoved);
		RemoveWorldItemsToBeRemoved(context);
	}

	private static void RemoveWorldItemsToBeRemoved(DataContext context)
	{
		List<ItemKey> worldItemsToBeRemoved = context.AdvanceMonthRelatedData.WorldItemsToBeRemoved;
		DomainManager.Item.RemoveItems(context, worldItemsToBeRemoved);
		worldItemsToBeRemoved.Clear();
	}

	private static void PostAdvanceMonth_CharacterPersonalNeedsUpdate(DataContext context, int areaId)
	{
		if (areaId < 0)
		{
			PostAdvanceMonth_CharacterPersonalNeedsUpdate_TaiwuGroup(context);
			return;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks((short)areaId);
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
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				if (element_Objects.GetPersonalNeeds().Count > 0)
				{
					element_Objects.OfflineUpdatePersonalNeedsDuration();
					ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
					parallelModificationsRecorder.RecordType(ParallelModificationType.PostAdvanceMonthPersonalNeedsUpdate);
					parallelModificationsRecorder.RecordParameterClass(element_Objects);
				}
				if (element_Objects.IsActiveExternalRelationState(2))
				{
					PostAdvanceMonth_CharacterPersonalNeedsUpdate_KidnappedChars(context, item);
				}
			}
		}
	}

	private static void PostAdvanceMonth_CharacterPersonalNeedsUpdate_TaiwuGroup(DataContext context)
	{
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		foreach (int item in collection)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.GetPersonalNeeds().Count > 0)
			{
				element_Objects.OfflineUpdatePersonalNeedsDuration();
				ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
				parallelModificationsRecorder.RecordType(ParallelModificationType.PostAdvanceMonthPersonalNeedsUpdate);
				parallelModificationsRecorder.RecordParameterClass(element_Objects);
			}
			if (element_Objects.IsActiveExternalRelationState(2))
			{
				PostAdvanceMonth_CharacterPersonalNeedsUpdate_KidnappedChars(context, item);
			}
		}
	}

	private static void PostAdvanceMonth_CharacterPersonalNeedsUpdate_KidnappedChars(DataContext context, int kidnapperCharId)
	{
		List<KidnappedCharacter> collection = DomainManager.Character.GetKidnappedCharacters(kidnapperCharId).GetCollection();
		int i = 0;
		for (int count = collection.Count; i < count; i++)
		{
			int charId = collection[i].CharId;
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			element_Objects.OfflineUpdatePersonalNeedsDuration();
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.PostAdvanceMonthPersonalNeedsUpdate);
			parallelModificationsRecorder.RecordParameterClass(element_Objects);
		}
	}

	private void PreAdvanceMonth(DataContext context, DataMonitorManager monitor)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		DomainManager.Map.TaiwuMoveRecord.Clear();
		DomainManager.Taiwu.UpdateVillagerRoleRecords(context);
		DomainManager.Taiwu.UpdateVillagerRoleFixedActionSuccessArray(context, isPreAdvance: true);
		DomainManager.Taiwu.JieqingPunishmentAssassinAlreadyAdd = false;
		DomainManager.Taiwu.JieqingHuntTaiwu = DomainManager.Taiwu.TryGetHuntTaiwuPrioritizedAction().action != null;
		SetAndNotifyAdvancingMonthState(context, 1, monitor);
		SetCurrDate(_currDate + 1, context);
		DomainManager.Extra.UpdateActionPoint(context);
		sbyte currMonthInYear = GetCurrMonthInYear();
		AddSolarTermNotification(currMonthInYear);
		DomainManager.Taiwu.LoseOverloadWarehouseItems(context);
		DomainManager.Organization.UpdateSectPrisonersOnAdvanceMonth(context);
		DomainManager.Character.DecayGraves(context);
		if (DomainManager.World.GetWorldFunctionsStatus(10))
		{
			DomainManager.Taiwu.CalcVillagerWorkOnMap(context);
		}
		DomainManager.Organization.MakeNoneOrgCharactersBecomeBeggar(context);
		DomainManager.Extra.UpdateMaxApprovingRateTempBonus(context);
		DomainManager.Organization.UpdateApprovingRateEffectOnAdvanceMonth(context);
		if (DomainManager.World.GetWorldFunctionsStatus(10))
		{
			DomainManager.Building.UpdateTaiwuBuildingAutoOperation(context);
			DomainManager.Building.SerialUpdate(context);
			DomainManager.Building.UpdateResourceBlockEffectsOnAdvanceMonth(context);
			DomainManager.Extra.FeastAdvanceMonth(context);
		}
		DomainManager.Building.UpdateMakingProgressOnMonthChange(context);
		ProfessionSkillHandle.OnPreAdvanceMonth(context);
		stopwatch.Stop();
		Logger.Info($"PreAdvanceMonth: {stopwatch.Elapsed.TotalMilliseconds:N1}");
	}

	private void PreAdvanceMonth_BornArea(DataContext context, DataMonitorManager monitor)
	{
		SetAndNotifyAdvancingMonthState(context, 1, monitor);
		DomainManager.Map.TaiwuMoveRecord.Clear();
		SetCurrDate(_currDate + 1, context);
		DomainManager.Extra.UpdateActionPoint(context);
		sbyte currMonthInYear = GetCurrMonthInYear();
		AddSolarTermNotification(currMonthInYear);
		DomainManager.Taiwu.LoseOverloadWarehouseItems(context);
		DomainManager.Building.UpdateMakingProgressOnMonthChange(context);
		Thread.Sleep(50);
	}

	public WorldDomain()
		: base(35)
	{
		_worldId = 0u;
		_xiangshuProgress = 0;
		_xiangshuAvatarTaskStatuses = new XiangshuAvatarTaskStatus[9];
		_xiangshuAvatarTasksInOrder = new sbyte[9];
		_stateTaskStatuses = new sbyte[15];
		_mainStoryLineProgress = 0;
		_beatRanChenZi = false;
		_worldFunctionsStatuses = 0uL;
		_customTexts = new Dictionary<int, string>(0);
		_nextCustomTextId = 0;
		_instantNotifications = new InstantNotificationCollection(1024);
		_onHandingMonthlyEventBlock = false;
		_lastMonthlyNotifications = new MonthlyNotificationCollection(0);
		_worldPopulationType = 0;
		_characterLifespanType = 0;
		_combatDifficulty = 0;
		_hereticsAmountType = 0;
		_bossInvasionSpeedType = 0;
		_worldResourceAmountType = 0;
		_allowRandomTaiwuHeir = false;
		_restrictOptionsBehaviorType = false;
		_taiwuVillageStateTemplateId = 0;
		_taiwuVillageLandFormType = 0;
		_hideTaiwuOriginalSurname = false;
		_allowExecute = false;
		_archiveFilesBackupInterval = 0;
		_worldStandardPopulation = 0;
		_currDate = 0;
		_daysInCurrMonth = 0;
		_advancingMonthState = 0;
		_currTaskList = new List<TaskData>();
		_sortedTaskList = new List<TaskDisplayData>();
		_worldStateData = default(WorldStateData);
		_archiveFilesBackupCount = 0;
		_sortedMonthlyNotificationSortingGroups = new List<int>();
		OnInitializedDomainData();
	}

	public uint GetWorldId()
	{
		return _worldId;
	}

	private unsafe void SetWorldId(uint value, DataContext context)
	{
		_worldId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 0, 4);
		*(uint*)ptr = _worldId;
		ptr += 4;
	}

	public sbyte GetXiangshuProgress()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 1))
		{
			return _xiangshuProgress;
		}
		sbyte xiangshuProgress = CalcXiangshuProgress();
		bool lockTaken = false;
		try
		{
			_spinLockXiangshuProgress.Enter(ref lockTaken);
			_xiangshuProgress = xiangshuProgress;
			BaseGameDataDomain.SetCached(DataStates, 1);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockXiangshuProgress.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _xiangshuProgress;
	}

	public XiangshuAvatarTaskStatus GetElement_XiangshuAvatarTaskStatuses(int index)
	{
		return _xiangshuAvatarTaskStatuses[index];
	}

	public unsafe void SetElement_XiangshuAvatarTaskStatuses(int index, XiangshuAvatarTaskStatus value, DataContext context)
	{
		_xiangshuAvatarTaskStatuses[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesXiangshuAvatarTaskStatuses, CacheInfluencesXiangshuAvatarTaskStatuses, context);
		byte* ptr = OperationAdder.FixedElementList_Set(1, 2, index, 8);
		ptr += value.Serialize(ptr);
	}

	public sbyte[] GetXiangshuAvatarTasksInOrder()
	{
		return _xiangshuAvatarTasksInOrder;
	}

	public unsafe void SetXiangshuAvatarTasksInOrder(sbyte[] value, DataContext context)
	{
		_xiangshuAvatarTasksInOrder = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 3, 9);
		for (int i = 0; i < 9; i++)
		{
			ptr[i] = (byte)_xiangshuAvatarTasksInOrder[i];
		}
		ptr += 9;
	}

	public sbyte GetElement_StateTaskStatuses(int index)
	{
		return _stateTaskStatuses[index];
	}

	public unsafe void SetElement_StateTaskStatuses(int index, sbyte value, DataContext context)
	{
		_stateTaskStatuses[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesStateTaskStatuses, CacheInfluencesStateTaskStatuses, context);
		byte* ptr = OperationAdder.FixedElementList_Set(1, 4, index, 1);
		*ptr = (byte)value;
		ptr++;
	}

	public short GetMainStoryLineProgress()
	{
		return _mainStoryLineProgress;
	}

	public unsafe void SetMainStoryLineProgress(short value, DataContext context)
	{
		_mainStoryLineProgress = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 5, 2);
		*(short*)ptr = _mainStoryLineProgress;
		ptr += 2;
	}

	public bool GetBeatRanChenZi()
	{
		return _beatRanChenZi;
	}

	public unsafe void SetBeatRanChenZi(bool value, DataContext context)
	{
		_beatRanChenZi = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 6, 1);
		*ptr = (_beatRanChenZi ? ((byte)1) : ((byte)0));
		ptr++;
	}

	public ulong GetWorldFunctionsStatuses()
	{
		return _worldFunctionsStatuses;
	}

	public unsafe void SetWorldFunctionsStatuses(ulong value, DataContext context)
	{
		_worldFunctionsStatuses = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 7, 8);
		*(ulong*)ptr = _worldFunctionsStatuses;
		ptr += 8;
	}

	public string GetElement_CustomTexts(int elementId)
	{
		return _customTexts[elementId];
	}

	public bool TryGetElement_CustomTexts(int elementId, out string value)
	{
		return _customTexts.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_CustomTexts(int elementId, string value, DataContext context)
	{
		_customTexts.Add(elementId, value);
		_modificationsCustomTexts.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int length = value.Length;
			int elementSize = 2 * length;
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(1, 8, elementId, elementSize);
			fixed (char* ptr2 = value)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)ptr2[i];
				}
			}
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(1, 8, elementId, 0);
		}
	}

	private unsafe void SetElement_CustomTexts(int elementId, string value, DataContext context)
	{
		_customTexts[elementId] = value;
		_modificationsCustomTexts.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int length = value.Length;
			int elementSize = 2 * length;
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(1, 8, elementId, elementSize);
			fixed (char* ptr2 = value)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)ptr2[i];
				}
			}
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(1, 8, elementId, 0);
		}
	}

	private void RemoveElement_CustomTexts(int elementId, DataContext context)
	{
		_customTexts.Remove(elementId);
		_modificationsCustomTexts.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(1, 8, elementId);
	}

	private void ClearCustomTexts(DataContext context)
	{
		_customTexts.Clear();
		_modificationsCustomTexts.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(1, 8);
	}

	private int GetNextCustomTextId()
	{
		return _nextCustomTextId;
	}

	private unsafe void SetNextCustomTextId(int value, DataContext context)
	{
		_nextCustomTextId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 9, 4);
		*(int*)ptr = _nextCustomTextId;
		ptr += 4;
	}

	public InstantNotificationCollection GetInstantNotifications()
	{
		return _instantNotifications;
	}

	private unsafe void CommitInsert_InstantNotifications(DataContext context, int offset, int size)
	{
		_modificationsInstantNotifications.RecordInserting(offset, size);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
		byte* pDest = OperationAdder.Binary_Insert(1, 10, offset, size);
		_instantNotifications.CopyTo(offset, size, pDest);
	}

	private unsafe void CommitWrite_InstantNotifications(DataContext context, int offset, int size)
	{
		_modificationsInstantNotifications.RecordWriting(offset, size);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
		byte* pDest = OperationAdder.Binary_Write(1, 10, offset, size);
		_instantNotifications.CopyTo(offset, size, pDest);
	}

	private void CommitRemove_InstantNotifications(DataContext context, int offset, int size)
	{
		_modificationsInstantNotifications.RecordRemoving(offset, size);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
		OperationAdder.Binary_Remove(1, 10, offset, size);
	}

	private unsafe void CommitSetMetadata_InstantNotifications(DataContext context)
	{
		_modificationsInstantNotifications.RecordSettingMetadata();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
		ushort serializedFixedSizeOfMetadata = _instantNotifications.GetSerializedFixedSizeOfMetadata();
		byte* pData = OperationAdder.Binary_SetMetadata(1, 10, serializedFixedSizeOfMetadata);
		_instantNotifications.SerializeMetadata(pData);
	}

	public bool GetOnHandingMonthlyEventBlock()
	{
		return _onHandingMonthlyEventBlock;
	}

	public void SetOnHandingMonthlyEventBlock(bool value, DataContext context)
	{
		_onHandingMonthlyEventBlock = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, DataStates, CacheInfluences, context);
	}

	[Obsolete("DomainData _lastMonthlyNotifications is no longer in use.")]
	public MonthlyNotificationCollection GetLastMonthlyNotifications()
	{
		return _lastMonthlyNotifications;
	}

	[Obsolete("DomainData _lastMonthlyNotifications is no longer in use.")]
	private unsafe void SetLastMonthlyNotifications(MonthlyNotificationCollection value, DataContext context)
	{
		_lastMonthlyNotifications = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
		int serializedSize = _lastMonthlyNotifications.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValue_Set(1, 12, serializedSize);
		ptr += _lastMonthlyNotifications.Serialize(ptr);
	}

	public byte GetWorldPopulationType()
	{
		return _worldPopulationType;
	}

	private unsafe void SetWorldPopulationType(byte value, DataContext context)
	{
		_worldPopulationType = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 13, 1);
		*ptr = _worldPopulationType;
		ptr++;
	}

	public byte GetCharacterLifespanType()
	{
		return _characterLifespanType;
	}

	public unsafe void SetCharacterLifespanType(byte value, DataContext context)
	{
		_characterLifespanType = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 14, 1);
		*ptr = _characterLifespanType;
		ptr++;
	}

	public byte GetCombatDifficulty()
	{
		return _combatDifficulty;
	}

	public unsafe void SetCombatDifficulty(byte value, DataContext context)
	{
		_combatDifficulty = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 15, 1);
		*ptr = _combatDifficulty;
		ptr++;
	}

	public byte GetHereticsAmountType()
	{
		return _hereticsAmountType;
	}

	public unsafe void SetHereticsAmountType(byte value, DataContext context)
	{
		_hereticsAmountType = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 16, 1);
		*ptr = _hereticsAmountType;
		ptr++;
	}

	public byte GetBossInvasionSpeedType()
	{
		return _bossInvasionSpeedType;
	}

	public unsafe void SetBossInvasionSpeedType(byte value, DataContext context)
	{
		_bossInvasionSpeedType = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 17, 1);
		*ptr = _bossInvasionSpeedType;
		ptr++;
	}

	public byte GetWorldResourceAmountType()
	{
		return _worldResourceAmountType;
	}

	public unsafe void SetWorldResourceAmountType(byte value, DataContext context)
	{
		_worldResourceAmountType = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 18, 1);
		*ptr = _worldResourceAmountType;
		ptr++;
	}

	public bool GetAllowRandomTaiwuHeir()
	{
		return _allowRandomTaiwuHeir;
	}

	public unsafe void SetAllowRandomTaiwuHeir(bool value, DataContext context)
	{
		_allowRandomTaiwuHeir = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 19, 1);
		*ptr = (_allowRandomTaiwuHeir ? ((byte)1) : ((byte)0));
		ptr++;
	}

	public bool GetRestrictOptionsBehaviorType()
	{
		return _restrictOptionsBehaviorType;
	}

	public unsafe void SetRestrictOptionsBehaviorType(bool value, DataContext context)
	{
		_restrictOptionsBehaviorType = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 20, 1);
		*ptr = (_restrictOptionsBehaviorType ? ((byte)1) : ((byte)0));
		ptr++;
	}

	public sbyte GetTaiwuVillageStateTemplateId()
	{
		return _taiwuVillageStateTemplateId;
	}

	public unsafe void SetTaiwuVillageStateTemplateId(sbyte value, DataContext context)
	{
		_taiwuVillageStateTemplateId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 21, 1);
		*ptr = (byte)_taiwuVillageStateTemplateId;
		ptr++;
	}

	public sbyte GetTaiwuVillageLandFormType()
	{
		return _taiwuVillageLandFormType;
	}

	public unsafe void SetTaiwuVillageLandFormType(sbyte value, DataContext context)
	{
		_taiwuVillageLandFormType = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 22, 1);
		*ptr = (byte)_taiwuVillageLandFormType;
		ptr++;
	}

	public bool GetHideTaiwuOriginalSurname()
	{
		return _hideTaiwuOriginalSurname;
	}

	public void SetHideTaiwuOriginalSurname(bool value, DataContext context)
	{
		_hideTaiwuOriginalSurname = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, DataStates, CacheInfluences, context);
	}

	public bool GetAllowExecute()
	{
		return _allowExecute;
	}

	public void SetAllowExecute(bool value, DataContext context)
	{
		_allowExecute = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, DataStates, CacheInfluences, context);
	}

	public sbyte GetArchiveFilesBackupInterval()
	{
		return _archiveFilesBackupInterval;
	}

	public void SetArchiveFilesBackupInterval(sbyte value, DataContext context)
	{
		_archiveFilesBackupInterval = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, DataStates, CacheInfluences, context);
	}

	public int GetWorldStandardPopulation()
	{
		return _worldStandardPopulation;
	}

	private unsafe void SetWorldStandardPopulation(int value, DataContext context)
	{
		_worldStandardPopulation = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 26, 4);
		*(int*)ptr = _worldStandardPopulation;
		ptr += 4;
	}

	public int GetCurrDate()
	{
		return _currDate;
	}

	private unsafe void SetCurrDate(int value, DataContext context)
	{
		_currDate = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 27, 4);
		*(int*)ptr = _currDate;
		ptr += 4;
	}

	[Obsolete("DomainData _daysInCurrMonth is no longer in use.")]
	public sbyte GetDaysInCurrMonth()
	{
		return _daysInCurrMonth;
	}

	[Obsolete("DomainData _daysInCurrMonth is no longer in use.")]
	private unsafe void SetDaysInCurrMonth(sbyte value, DataContext context)
	{
		_daysInCurrMonth = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 28, 1);
		*ptr = (byte)_daysInCurrMonth;
		ptr++;
	}

	public sbyte GetAdvancingMonthState()
	{
		return _advancingMonthState;
	}

	private void SetAdvancingMonthState(sbyte value, DataContext context)
	{
		_advancingMonthState = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(29, DataStates, CacheInfluences, context);
	}

	public List<TaskData> GetCurrTaskList()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 30))
		{
			return _currTaskList;
		}
		List<TaskData> list = new List<TaskData>();
		CalcCurrTaskList(list);
		bool lockTaken = false;
		try
		{
			_spinLockCurrTaskList.Enter(ref lockTaken);
			_currTaskList.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 30);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockCurrTaskList.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _currTaskList;
	}

	public List<TaskDisplayData> GetSortedTaskList()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 31))
		{
			return _sortedTaskList;
		}
		List<TaskDisplayData> list = new List<TaskDisplayData>();
		CalcSortedTaskList(list);
		bool lockTaken = false;
		try
		{
			_spinLockSortedTaskList.Enter(ref lockTaken);
			_sortedTaskList.Assign(list);
			BaseGameDataDomain.SetCached(DataStates, 31);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockSortedTaskList.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _sortedTaskList;
	}

	public ref WorldStateData GetWorldStateData()
	{
		Thread.MemoryBarrier();
		if (BaseGameDataDomain.IsCached(DataStates, 32))
		{
			return ref _worldStateData;
		}
		WorldStateData worldStateData = CalcWorldStateData();
		bool lockTaken = false;
		try
		{
			_spinLockWorldStateData.Enter(ref lockTaken);
			_worldStateData = worldStateData;
			BaseGameDataDomain.SetCached(DataStates, 32);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLockWorldStateData.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return ref _worldStateData;
	}

	public sbyte GetArchiveFilesBackupCount()
	{
		return _archiveFilesBackupCount;
	}

	public void SetArchiveFilesBackupCount(sbyte value, DataContext context)
	{
		_archiveFilesBackupCount = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(33, DataStates, CacheInfluences, context);
	}

	public List<int> GetSortedMonthlyNotificationSortingGroups()
	{
		return _sortedMonthlyNotificationSortingGroups;
	}

	public void SetSortedMonthlyNotificationSortingGroups(List<int> value, DataContext context)
	{
		_sortedMonthlyNotificationSortingGroups = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(34, DataStates, CacheInfluences, context);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		byte* ptr = OperationAdder.FixedSingleValue_Set(1, 0, 4);
		*(uint*)ptr = _worldId;
		ptr += 4;
		byte* ptr2 = OperationAdder.FixedElementList_InsertRange(1, 2, 0, 9, 72);
		for (int i = 0; i < 9; i++)
		{
			ptr2 += _xiangshuAvatarTaskStatuses[i].Serialize(ptr2);
		}
		byte* ptr3 = OperationAdder.FixedSingleValue_Set(1, 3, 9);
		for (int j = 0; j < 9; j++)
		{
			ptr3[j] = (byte)_xiangshuAvatarTasksInOrder[j];
		}
		ptr3 += 9;
		byte* ptr4 = OperationAdder.FixedElementList_InsertRange(1, 4, 0, 15, 15);
		for (int k = 0; k < 15; k++)
		{
			ptr4[k] = (byte)_stateTaskStatuses[k];
		}
		ptr4 += 15;
		byte* ptr5 = OperationAdder.FixedSingleValue_Set(1, 5, 2);
		*(short*)ptr5 = _mainStoryLineProgress;
		ptr5 += 2;
		byte* ptr6 = OperationAdder.FixedSingleValue_Set(1, 6, 1);
		*ptr6 = (_beatRanChenZi ? ((byte)1) : ((byte)0));
		ptr6++;
		byte* ptr7 = OperationAdder.FixedSingleValue_Set(1, 7, 8);
		*(ulong*)ptr7 = _worldFunctionsStatuses;
		ptr7 += 8;
		foreach (KeyValuePair<int, string> customText in _customTexts)
		{
			int key = customText.Key;
			string value = customText.Value;
			if (value != null)
			{
				int length = value.Length;
				int elementSize = 2 * length;
				byte* ptr8 = OperationAdder.DynamicSingleValueCollection_Add(1, 8, key, elementSize);
				fixed (char* ptr9 = value)
				{
					for (int l = 0; l < length; l++)
					{
						((short*)ptr8)[l] = (short)ptr9[l];
					}
				}
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(1, 8, key, 0);
			}
		}
		byte* ptr10 = OperationAdder.FixedSingleValue_Set(1, 9, 4);
		*(int*)ptr10 = _nextCustomTextId;
		ptr10 += 4;
		int size = _instantNotifications.GetSize();
		byte* pDest = OperationAdder.Binary_Write(1, 10, 0, size);
		_instantNotifications.CopyTo(0, size, pDest);
		ushort serializedFixedSizeOfMetadata = _instantNotifications.GetSerializedFixedSizeOfMetadata();
		byte* pData = OperationAdder.Binary_SetMetadata(1, 10, serializedFixedSizeOfMetadata);
		_instantNotifications.SerializeMetadata(pData);
		int serializedSize = _lastMonthlyNotifications.GetSerializedSize();
		byte* ptr11 = OperationAdder.DynamicSingleValue_Set(1, 12, serializedSize);
		ptr11 += _lastMonthlyNotifications.Serialize(ptr11);
		byte* ptr12 = OperationAdder.FixedSingleValue_Set(1, 13, 1);
		*ptr12 = _worldPopulationType;
		ptr12++;
		byte* ptr13 = OperationAdder.FixedSingleValue_Set(1, 14, 1);
		*ptr13 = _characterLifespanType;
		ptr13++;
		byte* ptr14 = OperationAdder.FixedSingleValue_Set(1, 15, 1);
		*ptr14 = _combatDifficulty;
		ptr14++;
		byte* ptr15 = OperationAdder.FixedSingleValue_Set(1, 16, 1);
		*ptr15 = _hereticsAmountType;
		ptr15++;
		byte* ptr16 = OperationAdder.FixedSingleValue_Set(1, 17, 1);
		*ptr16 = _bossInvasionSpeedType;
		ptr16++;
		byte* ptr17 = OperationAdder.FixedSingleValue_Set(1, 18, 1);
		*ptr17 = _worldResourceAmountType;
		ptr17++;
		byte* ptr18 = OperationAdder.FixedSingleValue_Set(1, 19, 1);
		*ptr18 = (_allowRandomTaiwuHeir ? ((byte)1) : ((byte)0));
		ptr18++;
		byte* ptr19 = OperationAdder.FixedSingleValue_Set(1, 20, 1);
		*ptr19 = (_restrictOptionsBehaviorType ? ((byte)1) : ((byte)0));
		ptr19++;
		byte* ptr20 = OperationAdder.FixedSingleValue_Set(1, 21, 1);
		*ptr20 = (byte)_taiwuVillageStateTemplateId;
		ptr20++;
		byte* ptr21 = OperationAdder.FixedSingleValue_Set(1, 22, 1);
		*ptr21 = (byte)_taiwuVillageLandFormType;
		ptr21++;
		byte* ptr22 = OperationAdder.FixedSingleValue_Set(1, 26, 4);
		*(int*)ptr22 = _worldStandardPopulation;
		ptr22 += 4;
		byte* ptr23 = OperationAdder.FixedSingleValue_Set(1, 27, 4);
		*(int*)ptr23 = _currDate;
		ptr23 += 4;
		byte* ptr24 = OperationAdder.FixedSingleValue_Set(1, 28, 1);
		*ptr24 = (byte)_daysInCurrMonth;
		ptr24++;
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 0));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(1, 2));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 3));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(1, 4));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 5));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 6));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 7));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(1, 8));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 9));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.Binary_Get(1, 10));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(1, 12));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 13));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 14));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 15));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 16));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 17));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 18));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 19));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 20));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 21));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 22));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 26));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 27));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(1, 28));
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
			return GameData.Serializer.Serializer.Serialize(_worldId, dataPool);
		case 1:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			return GameData.Serializer.Serializer.Serialize(GetXiangshuProgress(), dataPool);
		case 2:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesXiangshuAvatarTaskStatuses, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_xiangshuAvatarTaskStatuses[(uint)subId0], dataPool);
		case 3:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
			}
			return GameData.Serializer.Serializer.Serialize(_xiangshuAvatarTasksInOrder, dataPool);
		case 4:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesStateTaskStatuses, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_stateTaskStatuses[(uint)subId0], dataPool);
		case 5:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
			}
			return GameData.Serializer.Serializer.Serialize(_mainStoryLineProgress, dataPool);
		case 6:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
			}
			return GameData.Serializer.Serializer.Serialize(_beatRanChenZi, dataPool);
		case 7:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
			}
			return GameData.Serializer.Serializer.Serialize(_worldFunctionsStatuses, dataPool);
		case 8:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
				_modificationsCustomTexts.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<int, string>)_customTexts, dataPool);
		case 9:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 10:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 10);
				_modificationsInstantNotifications.Reset(_instantNotifications.GetSize());
			}
			return GameData.Serializer.Serializer.SerializeModifications((IBinary)(object)_instantNotifications, dataPool);
		case 11:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
			}
			return GameData.Serializer.Serializer.Serialize(_onHandingMonthlyEventBlock, dataPool);
		case 12:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 12);
			}
			return GameData.Serializer.Serializer.Serialize(_lastMonthlyNotifications, dataPool);
		case 13:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 13);
			}
			return GameData.Serializer.Serializer.Serialize(_worldPopulationType, dataPool);
		case 14:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 14);
			}
			return GameData.Serializer.Serializer.Serialize(_characterLifespanType, dataPool);
		case 15:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
			}
			return GameData.Serializer.Serializer.Serialize(_combatDifficulty, dataPool);
		case 16:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 16);
			}
			return GameData.Serializer.Serializer.Serialize(_hereticsAmountType, dataPool);
		case 17:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 17);
			}
			return GameData.Serializer.Serializer.Serialize(_bossInvasionSpeedType, dataPool);
		case 18:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 18);
			}
			return GameData.Serializer.Serializer.Serialize(_worldResourceAmountType, dataPool);
		case 19:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 19);
			}
			return GameData.Serializer.Serializer.Serialize(_allowRandomTaiwuHeir, dataPool);
		case 20:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 20);
			}
			return GameData.Serializer.Serializer.Serialize(_restrictOptionsBehaviorType, dataPool);
		case 21:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuVillageStateTemplateId, dataPool);
		case 22:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuVillageLandFormType, dataPool);
		case 23:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 23);
			}
			return GameData.Serializer.Serializer.Serialize(_hideTaiwuOriginalSurname, dataPool);
		case 24:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 24);
			}
			return GameData.Serializer.Serializer.Serialize(_allowExecute, dataPool);
		case 25:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 25);
			}
			return GameData.Serializer.Serializer.Serialize(_archiveFilesBackupInterval, dataPool);
		case 26:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 26);
			}
			return GameData.Serializer.Serializer.Serialize(_worldStandardPopulation, dataPool);
		case 27:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 27);
			}
			return GameData.Serializer.Serializer.Serialize(_currDate, dataPool);
		case 28:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 28);
			}
			return GameData.Serializer.Serializer.Serialize(_daysInCurrMonth, dataPool);
		case 29:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 29);
			}
			return GameData.Serializer.Serializer.Serialize(_advancingMonthState, dataPool);
		case 30:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 30);
			}
			return GameData.Serializer.Serializer.Serialize(GetCurrTaskList(), dataPool);
		case 31:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 31);
			}
			return GameData.Serializer.Serializer.Serialize(GetSortedTaskList(), dataPool);
		case 32:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 32);
			}
			return GameData.Serializer.Serializer.Serialize(GetWorldStateData(), dataPool);
		case 33:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 33);
			}
			return GameData.Serializer.Serializer.Serialize(_archiveFilesBackupCount, dataPool);
		case 34:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 34);
			}
			return GameData.Serializer.Serializer.Serialize(_sortedMonthlyNotificationSortingGroups, dataPool);
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
		{
			XiangshuAvatarTaskStatus item2 = default(XiangshuAvatarTaskStatus);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			_xiangshuAvatarTaskStatuses[(uint)subId0] = item2;
			SetElement_XiangshuAvatarTaskStatuses((int)subId0, item2, context);
			break;
		}
		case 3:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _xiangshuAvatarTasksInOrder);
			SetXiangshuAvatarTasksInOrder(_xiangshuAvatarTasksInOrder, context);
			break;
		case 4:
		{
			sbyte item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			_stateTaskStatuses[(uint)subId0] = item;
			SetElement_StateTaskStatuses((int)subId0, item, context);
			break;
		}
		case 5:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _mainStoryLineProgress);
			SetMainStoryLineProgress(_mainStoryLineProgress, context);
			break;
		case 6:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _beatRanChenZi);
			SetBeatRanChenZi(_beatRanChenZi, context);
			break;
		case 7:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _worldFunctionsStatuses);
			SetWorldFunctionsStatuses(_worldFunctionsStatuses, context);
			break;
		case 8:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 9:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 10:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 11:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _onHandingMonthlyEventBlock);
			SetOnHandingMonthlyEventBlock(_onHandingMonthlyEventBlock, context);
			break;
		case 12:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 13:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 14:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _characterLifespanType);
			SetCharacterLifespanType(_characterLifespanType, context);
			break;
		case 15:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _combatDifficulty);
			SetCombatDifficulty(_combatDifficulty, context);
			break;
		case 16:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _hereticsAmountType);
			SetHereticsAmountType(_hereticsAmountType, context);
			break;
		case 17:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _bossInvasionSpeedType);
			SetBossInvasionSpeedType(_bossInvasionSpeedType, context);
			break;
		case 18:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _worldResourceAmountType);
			SetWorldResourceAmountType(_worldResourceAmountType, context);
			break;
		case 19:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _allowRandomTaiwuHeir);
			SetAllowRandomTaiwuHeir(_allowRandomTaiwuHeir, context);
			break;
		case 20:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _restrictOptionsBehaviorType);
			SetRestrictOptionsBehaviorType(_restrictOptionsBehaviorType, context);
			break;
		case 21:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _taiwuVillageStateTemplateId);
			SetTaiwuVillageStateTemplateId(_taiwuVillageStateTemplateId, context);
			break;
		case 22:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _taiwuVillageLandFormType);
			SetTaiwuVillageLandFormType(_taiwuVillageLandFormType, context);
			break;
		case 23:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _hideTaiwuOriginalSurname);
			SetHideTaiwuOriginalSurname(_hideTaiwuOriginalSurname, context);
			break;
		case 24:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _allowExecute);
			SetAllowExecute(_allowExecute, context);
			break;
		case 25:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _archiveFilesBackupInterval);
			SetArchiveFilesBackupInterval(_archiveFilesBackupInterval, context);
			break;
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
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _archiveFilesBackupCount);
			SetArchiveFilesBackupCount(_archiveFilesBackupCount, context);
			break;
		case 34:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _sortedMonthlyNotificationSortingGroups);
			SetSortedMonthlyNotificationSortingGroups(_sortedMonthlyNotificationSortingGroups, context);
			break;
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
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 1)
			{
				WorldCreationInfo item12 = default(WorldCreationInfo);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				CreateWorld(context, item12);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount17 = operation.ArgsCount;
			int num17 = argsCount17;
			if (num17 == 2)
			{
				WorldCreationInfo item36 = default(WorldCreationInfo);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item36);
				bool item37 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item37);
				SetWorldCreationInfo(context, item36, item37);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
			if (operation.ArgsCount == 0)
			{
				WorldCreationInfo worldCreationInfo = GetWorldCreationInfo();
				return GameData.Serializer.Serializer.Serialize(worldCreationInfo, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 3:
			if (operation.ArgsCount == 0)
			{
				List<Location> juniorXiangshuLocations = GetJuniorXiangshuLocations();
				return GameData.Serializer.Serializer.Serialize(juniorXiangshuLocations, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 4:
		{
			int argsCount19 = operation.ArgsCount;
			int num19 = argsCount19;
			if (num19 == 1)
			{
				int item41 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item41);
				HandleMonthlyEvent(context, item41);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
			if (operation.ArgsCount == 0)
			{
				MonthlyEventCollection monthlyEventCollection = GetMonthlyEventCollection();
				return GameData.Serializer.Serializer.Serialize(monthlyEventCollection, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 6:
			if (operation.ArgsCount == 0)
			{
				RemoveAllInvalidMonthlyEvents(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 7:
			if (operation.ArgsCount == 0)
			{
				ProcessAllMonthlyEventsWithDefaultOption(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 8:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 1)
			{
				byte item23 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				SpecifyWorldPopulationType(context, item23);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 9:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 1)
			{
				int item10 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				AdvanceDaysInMonth(context, item10);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
			if (operation.ArgsCount == 0)
			{
				AdvanceMonth(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 11:
		{
			int argsCount15 = operation.ArgsCount;
			int num15 = argsCount15;
			if (num15 == 1)
			{
				bool item32 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item32);
				AdvanceMonth_DisplayedMonthlyNotifications(context, item32);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 12:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 2)
			{
				short item25 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item25);
				short item26 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				bool item27 = GmCmd_SectEmeiAddSkillBreakBonus(context, item25, item26);
				return GameData.Serializer.Serializer.Serialize(item27, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 13:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 1)
			{
				short item17 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				bool item18 = GmCmd_SectEmeiClearSkillBreakBonus(context, item17);
				return GameData.Serializer.Serializer.Serialize(item18, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 14:
			if (operation.ArgsCount == 0)
			{
				ItemKey item2 = RefiningWugKing(context);
				return GameData.Serializer.Serializer.Serialize(item2, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 15:
		{
			int argsCount18 = operation.ArgsCount;
			int num18 = argsCount18;
			if (num18 == 1)
			{
				List<ItemKey> item39 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item39);
				bool item40 = DropPoisonsToWugJug(context, item39);
				return GameData.Serializer.Serializer.Serialize(item40, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 16:
			if (operation.ArgsCount == 0)
			{
				bool item34 = JingangMonkSoulBtnShow();
				return GameData.Serializer.Serializer.Serialize(item34, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 17:
			if (operation.ArgsCount == 0)
			{
				bool item31 = JingangSoulTransformOpen();
				return GameData.Serializer.Serializer.Serialize(item31, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 18:
			if (operation.ArgsCount == 0)
			{
				sbyte baihuaLifeLinkNeiliType = GetBaihuaLifeLinkNeiliType();
				return GameData.Serializer.Serializer.Serialize(baihuaLifeLinkNeiliType, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 19:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 3)
			{
				int item20 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				int item21 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item21);
				bool item22 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				SetLifeLinkCharacter(context, item20, item21, item22);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 20:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 2)
			{
				short item13 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item13);
				List<ItemKey> item14 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				bool item15 = EmeiTransferBonusProgress(context, item13, item14);
				return GameData.Serializer.Serializer.Serialize(item15, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 21:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 4)
			{
				short item5 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				short item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				int item7 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				int item8 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				bool item9 = GmCmd_AddMonthlyEvent(item5, item6, item7, item8);
				return GameData.Serializer.Serializer.Serialize(item9, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 22:
		{
			int argsCount20 = operation.ArgsCount;
			int num20 = argsCount20;
			if (num20 == 2)
			{
				sbyte item42 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item42);
				bool item43 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item43);
				CatchThief(item42, item43);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 23:
			if (operation.ArgsCount == 0)
			{
				int item38 = TryTriggerThiefCatch(context);
				return GameData.Serializer.Serializer.Serialize(item38, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 24:
		{
			int argsCount16 = operation.ArgsCount;
			int num16 = argsCount16;
			if (num16 == 1)
			{
				int item35 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item35);
				ShaolinStartDemonSlayerTrial(context, item35);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 25:
			if (operation.ArgsCount == 0)
			{
				bool item33 = ShaolinInterruptDemonSlayerTrial(context);
				return GameData.Serializer.Serializer.Serialize(item33, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 26:
			if (operation.ArgsCount == 0)
			{
				bool item30 = ShaolinRegenerateRestricts(context);
				return GameData.Serializer.Serializer.Serialize(item30, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 27:
		{
			int argsCount14 = operation.ArgsCount;
			int num14 = argsCount14;
			if (num14 == 1)
			{
				int item28 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item28);
				byte item29 = ShaolinQueryRestrictsAreSatisfied(item28);
				return GameData.Serializer.Serializer.Serialize(item29, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 28:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 1)
			{
				List<int> item24 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item24);
				ShaolinClearTemporaryDemon(context, item24);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 29:
			if (operation.ArgsCount == 0)
			{
				List<int> item19 = ShaolinGenerateTemporaryDemon(context);
				return GameData.Serializer.Serializer.Serialize(item19, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 30:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 1)
			{
				short item16 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item16);
				int sectMainStoryTriggerConditions = GetSectMainStoryTriggerConditions(item16);
				return GameData.Serializer.Serializer.Serialize(sectMainStoryTriggerConditions, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 31:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 1)
			{
				sbyte item11 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				int sectMainStoryActiveStatus = GetSectMainStoryActiveStatus(item11);
				return GameData.Serializer.Serializer.Serialize(sectMainStoryActiveStatus, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 32:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 2)
			{
				sbyte item3 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				bool item4 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				SetSectMainStoryActiveStatus(item3, item4);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 33:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 1)
			{
				sbyte item = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				NotifySectStoryActivated(context, item);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
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
			_modificationsCustomTexts.ChangeRecording(monitoring);
			break;
		case 9:
			break;
		case 10:
			_modificationsInstantNotifications.ChangeRecording(monitoring, _instantNotifications.GetSize());
			break;
		case 11:
			break;
		case 12:
			break;
		case 13:
			break;
		case 14:
			break;
		case 15:
			break;
		case 16:
			break;
		case 17:
			break;
		case 18:
			break;
		case 19:
			break;
		case 20:
			break;
		case 21:
			break;
		case 22:
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
			return GameData.Serializer.Serializer.Serialize(_worldId, dataPool);
		case 1:
			if (!BaseGameDataDomain.IsModified(DataStates, 1))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 1);
			return GameData.Serializer.Serializer.Serialize(GetXiangshuProgress(), dataPool);
		case 2:
			if (!BaseGameDataDomain.IsModified(_dataStatesXiangshuAvatarTaskStatuses, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesXiangshuAvatarTaskStatuses, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_xiangshuAvatarTaskStatuses[(uint)subId0], dataPool);
		case 3:
			if (!BaseGameDataDomain.IsModified(DataStates, 3))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 3);
			return GameData.Serializer.Serializer.Serialize(_xiangshuAvatarTasksInOrder, dataPool);
		case 4:
			if (!BaseGameDataDomain.IsModified(_dataStatesStateTaskStatuses, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesStateTaskStatuses, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_stateTaskStatuses[(uint)subId0], dataPool);
		case 5:
			if (!BaseGameDataDomain.IsModified(DataStates, 5))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 5);
			return GameData.Serializer.Serializer.Serialize(_mainStoryLineProgress, dataPool);
		case 6:
			if (!BaseGameDataDomain.IsModified(DataStates, 6))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 6);
			return GameData.Serializer.Serializer.Serialize(_beatRanChenZi, dataPool);
		case 7:
			if (!BaseGameDataDomain.IsModified(DataStates, 7))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 7);
			return GameData.Serializer.Serializer.Serialize(_worldFunctionsStatuses, dataPool);
		case 8:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 8))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 8);
			int result = GameData.Serializer.Serializer.SerializeModifications(_customTexts, dataPool, _modificationsCustomTexts);
			_modificationsCustomTexts.Reset();
			return result;
		}
		case 9:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 10:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 10))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 10);
			int result2 = GameData.Serializer.Serializer.SerializeModifications((IBinary)(object)_instantNotifications, dataPool, _modificationsInstantNotifications);
			_modificationsInstantNotifications.Reset(_instantNotifications.GetSize());
			return result2;
		}
		case 11:
			if (!BaseGameDataDomain.IsModified(DataStates, 11))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 11);
			return GameData.Serializer.Serializer.Serialize(_onHandingMonthlyEventBlock, dataPool);
		case 12:
			if (!BaseGameDataDomain.IsModified(DataStates, 12))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 12);
			return GameData.Serializer.Serializer.Serialize(_lastMonthlyNotifications, dataPool);
		case 13:
			if (!BaseGameDataDomain.IsModified(DataStates, 13))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 13);
			return GameData.Serializer.Serializer.Serialize(_worldPopulationType, dataPool);
		case 14:
			if (!BaseGameDataDomain.IsModified(DataStates, 14))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 14);
			return GameData.Serializer.Serializer.Serialize(_characterLifespanType, dataPool);
		case 15:
			if (!BaseGameDataDomain.IsModified(DataStates, 15))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 15);
			return GameData.Serializer.Serializer.Serialize(_combatDifficulty, dataPool);
		case 16:
			if (!BaseGameDataDomain.IsModified(DataStates, 16))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 16);
			return GameData.Serializer.Serializer.Serialize(_hereticsAmountType, dataPool);
		case 17:
			if (!BaseGameDataDomain.IsModified(DataStates, 17))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 17);
			return GameData.Serializer.Serializer.Serialize(_bossInvasionSpeedType, dataPool);
		case 18:
			if (!BaseGameDataDomain.IsModified(DataStates, 18))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 18);
			return GameData.Serializer.Serializer.Serialize(_worldResourceAmountType, dataPool);
		case 19:
			if (!BaseGameDataDomain.IsModified(DataStates, 19))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 19);
			return GameData.Serializer.Serializer.Serialize(_allowRandomTaiwuHeir, dataPool);
		case 20:
			if (!BaseGameDataDomain.IsModified(DataStates, 20))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 20);
			return GameData.Serializer.Serializer.Serialize(_restrictOptionsBehaviorType, dataPool);
		case 21:
			if (!BaseGameDataDomain.IsModified(DataStates, 21))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 21);
			return GameData.Serializer.Serializer.Serialize(_taiwuVillageStateTemplateId, dataPool);
		case 22:
			if (!BaseGameDataDomain.IsModified(DataStates, 22))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 22);
			return GameData.Serializer.Serializer.Serialize(_taiwuVillageLandFormType, dataPool);
		case 23:
			if (!BaseGameDataDomain.IsModified(DataStates, 23))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 23);
			return GameData.Serializer.Serializer.Serialize(_hideTaiwuOriginalSurname, dataPool);
		case 24:
			if (!BaseGameDataDomain.IsModified(DataStates, 24))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 24);
			return GameData.Serializer.Serializer.Serialize(_allowExecute, dataPool);
		case 25:
			if (!BaseGameDataDomain.IsModified(DataStates, 25))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 25);
			return GameData.Serializer.Serializer.Serialize(_archiveFilesBackupInterval, dataPool);
		case 26:
			if (!BaseGameDataDomain.IsModified(DataStates, 26))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 26);
			return GameData.Serializer.Serializer.Serialize(_worldStandardPopulation, dataPool);
		case 27:
			if (!BaseGameDataDomain.IsModified(DataStates, 27))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 27);
			return GameData.Serializer.Serializer.Serialize(_currDate, dataPool);
		case 28:
			if (!BaseGameDataDomain.IsModified(DataStates, 28))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 28);
			return GameData.Serializer.Serializer.Serialize(_daysInCurrMonth, dataPool);
		case 29:
			if (!BaseGameDataDomain.IsModified(DataStates, 29))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 29);
			return GameData.Serializer.Serializer.Serialize(_advancingMonthState, dataPool);
		case 30:
			if (!BaseGameDataDomain.IsModified(DataStates, 30))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 30);
			return GameData.Serializer.Serializer.Serialize(GetCurrTaskList(), dataPool);
		case 31:
			if (!BaseGameDataDomain.IsModified(DataStates, 31))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 31);
			return GameData.Serializer.Serializer.Serialize(GetSortedTaskList(), dataPool);
		case 32:
			if (!BaseGameDataDomain.IsModified(DataStates, 32))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 32);
			return GameData.Serializer.Serializer.Serialize(GetWorldStateData(), dataPool);
		case 33:
			if (!BaseGameDataDomain.IsModified(DataStates, 33))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 33);
			return GameData.Serializer.Serializer.Serialize(_archiveFilesBackupCount, dataPool);
		case 34:
			if (!BaseGameDataDomain.IsModified(DataStates, 34))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 34);
			return GameData.Serializer.Serializer.Serialize(_sortedMonthlyNotificationSortingGroups, dataPool);
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
			if (BaseGameDataDomain.IsModified(_dataStatesXiangshuAvatarTaskStatuses, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesXiangshuAvatarTaskStatuses, (int)subId0);
			}
			break;
		case 3:
			if (BaseGameDataDomain.IsModified(DataStates, 3))
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
			}
			break;
		case 4:
			if (BaseGameDataDomain.IsModified(_dataStatesStateTaskStatuses, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesStateTaskStatuses, (int)subId0);
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
			if (BaseGameDataDomain.IsModified(DataStates, 7))
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
			}
			break;
		case 8:
			if (BaseGameDataDomain.IsModified(DataStates, 8))
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
				_modificationsCustomTexts.Reset();
			}
			break;
		case 9:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
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
			}
			break;
		case 15:
			if (BaseGameDataDomain.IsModified(DataStates, 15))
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
			}
			break;
		case 16:
			if (BaseGameDataDomain.IsModified(DataStates, 16))
			{
				BaseGameDataDomain.ResetModified(DataStates, 16);
			}
			break;
		case 17:
			if (BaseGameDataDomain.IsModified(DataStates, 17))
			{
				BaseGameDataDomain.ResetModified(DataStates, 17);
			}
			break;
		case 18:
			if (BaseGameDataDomain.IsModified(DataStates, 18))
			{
				BaseGameDataDomain.ResetModified(DataStates, 18);
			}
			break;
		case 19:
			if (BaseGameDataDomain.IsModified(DataStates, 19))
			{
				BaseGameDataDomain.ResetModified(DataStates, 19);
			}
			break;
		case 20:
			if (BaseGameDataDomain.IsModified(DataStates, 20))
			{
				BaseGameDataDomain.ResetModified(DataStates, 20);
			}
			break;
		case 21:
			if (BaseGameDataDomain.IsModified(DataStates, 21))
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
			}
			break;
		case 22:
			if (BaseGameDataDomain.IsModified(DataStates, 22))
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
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
			}
			break;
		case 29:
			if (BaseGameDataDomain.IsModified(DataStates, 29))
			{
				BaseGameDataDomain.ResetModified(DataStates, 29);
			}
			break;
		case 30:
			if (BaseGameDataDomain.IsModified(DataStates, 30))
			{
				BaseGameDataDomain.ResetModified(DataStates, 30);
			}
			break;
		case 31:
			if (BaseGameDataDomain.IsModified(DataStates, 31))
			{
				BaseGameDataDomain.ResetModified(DataStates, 31);
			}
			break;
		case 32:
			if (BaseGameDataDomain.IsModified(DataStates, 32))
			{
				BaseGameDataDomain.ResetModified(DataStates, 32);
			}
			break;
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
			2 => BaseGameDataDomain.IsModified(_dataStatesXiangshuAvatarTaskStatuses, (int)subId0), 
			3 => BaseGameDataDomain.IsModified(DataStates, 3), 
			4 => BaseGameDataDomain.IsModified(_dataStatesStateTaskStatuses, (int)subId0), 
			5 => BaseGameDataDomain.IsModified(DataStates, 5), 
			6 => BaseGameDataDomain.IsModified(DataStates, 6), 
			7 => BaseGameDataDomain.IsModified(DataStates, 7), 
			8 => BaseGameDataDomain.IsModified(DataStates, 8), 
			9 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
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
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
		case 1:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(1, DataStates, CacheInfluences, context);
			break;
		case 30:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(30, DataStates, CacheInfluences, context);
			break;
		case 31:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(31, DataStates, CacheInfluences, context);
			break;
		case 32:
			BaseGameDataDomain.InvalidateSelfAndInfluencedCache(32, DataStates, CacheInfluences, context);
			break;
		default:
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		case 0:
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
		case 33:
		case 34:
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		uint num;
		switch (operation.DataId)
		{
		case 0:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _worldId);
			goto IL_02d3;
		case 2:
			ResponseProcessor.ProcessElementList_CustomType_Fixed_Value<XiangshuAvatarTaskStatus>(operation, pResult, _xiangshuAvatarTaskStatuses, 9, 8);
			goto IL_02d3;
		case 3:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array(operation, pResult, _xiangshuAvatarTasksInOrder, 9);
			goto IL_02d3;
		case 4:
			ResponseProcessor.ProcessElementList_BasicType_Fixed_Value(operation, pResult, _stateTaskStatuses, 15, 1);
			goto IL_02d3;
		case 5:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _mainStoryLineProgress);
			goto IL_02d3;
		case 6:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _beatRanChenZi);
			goto IL_02d3;
		case 7:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _worldFunctionsStatuses);
			goto IL_02d3;
		case 8:
			ResponseProcessor.ProcessSingleValueCollection_BasicType_String(operation, pResult, _customTexts);
			goto IL_02d3;
		case 9:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _nextCustomTextId);
			goto IL_02d3;
		case 10:
			ResponseProcessor.ProcessBinary(operation, pResult, (IBinary)(object)_instantNotifications);
			goto IL_02d3;
		case 12:
			ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Ref_Single(operation, pResult, _lastMonthlyNotifications);
			goto IL_02d3;
		case 13:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _worldPopulationType);
			goto IL_02d3;
		case 14:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _characterLifespanType);
			goto IL_02d3;
		case 15:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _combatDifficulty);
			goto IL_02d3;
		case 16:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _hereticsAmountType);
			goto IL_02d3;
		case 17:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _bossInvasionSpeedType);
			goto IL_02d3;
		case 18:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _worldResourceAmountType);
			goto IL_02d3;
		case 19:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _allowRandomTaiwuHeir);
			goto IL_02d3;
		case 20:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _restrictOptionsBehaviorType);
			goto IL_02d3;
		case 21:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _taiwuVillageStateTemplateId);
			goto IL_02d3;
		case 22:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _taiwuVillageLandFormType);
			goto IL_02d3;
		case 26:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _worldStandardPopulation);
			goto IL_02d3;
		case 27:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _currDate);
			goto IL_02d3;
		case 28:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _daysInCurrMonth);
			goto IL_02d3;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		case 1:
		case 11:
		case 23:
		case 24:
		case 25:
		case 29:
		case 30:
		case 31:
		case 32:
		case 33:
		case 34:
			{
				throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
			}
			IL_02d3:
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
					DomainManager.Global.CompleteLoading(1);
				}
			}
			break;
		}
	}

	private void InitializeInternalDataOfCollections()
	{
	}
}
