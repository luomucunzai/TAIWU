using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Config;
using Config.EventConfig;
using GameData.ArchiveData;
using GameData.Common;
using GameData.DLC;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Adventure;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Relation;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.EventLog;
using GameData.Domains.TaiwuEvent.EventManager;
using GameData.Domains.TaiwuEvent.EventOption;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.World;
using GameData.Domains.World.Notification;
using GameData.Domains.World.SectMainStory;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.TaiwuEvent;

[GameDataDomain(12)]
public class TaiwuEventDomain : BaseGameDataDomain
{
	private enum EEventPackageLoadMethod
	{
		LoadFile,
		LoadFrom,
		LoadBuffer
	}

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private static EventManagerBase[] _managerArray;

	private LocalObjectPool<EventArgBox> _argBoxPool;

	private List<TaiwuEvent> _triggeredEventList;

	private TaiwuEvent _showingEvent;

	private (string EventGuid, string OptionKey, string nextGuid) _interactCheckContinueData;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private TaiwuEventDisplayData _displayingEventData;

	public DataContext MainThreadDataContext;

	public string SeriesEventTexture;

	private bool _stopAutoNextEvent;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<ItemKey> _tempCreateItemList;

	private string _waitConfirmSelectOption;

	private TaiwuEventItem _waitConfirmEventConfig;

	private string _waitConfirmOptionKey;

	[Obsolete("use arg in global argument box instead")]
	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private int _awayForeverLoverCharId;

	private readonly List<string> _eventEnteredList = new List<string>();

	public List<(string listeningAction, TaiwuEvent listenerEvent)> ListeningEventActionList = new List<(string, TaiwuEvent)>();

	[Obsolete]
	private TaiwuEvent _listenerEvent;

	private readonly Queue<List<EventLogResultData>> _eventLogQueue = new Queue<List<EventLogResultData>>();

	private readonly Dictionary<int, CharacterDisplayData> _characterCache = new Dictionary<int, CharacterDisplayData>();

	private readonly Dictionary<int, int> _characterReferences = new Dictionary<int, int>();

	private readonly Dictionary<int, SecretInformationDisplayData> _secretInformationCache = new Dictionary<int, SecretInformationDisplayData>();

	private readonly Dictionary<int, int> _secretInformationReferences = new Dictionary<int, int>();

	private readonly Dictionary<int, ItemDisplayData> _itemCache = new Dictionary<int, ItemDisplayData>();

	private readonly Dictionary<int, int> _itemReferences = new Dictionary<int, int>();

	private readonly Dictionary<int, CombatSkillDisplayData> _combatSkillCache = new Dictionary<int, CombatSkillDisplayData>();

	private readonly Dictionary<int, int> _combatSkillReferences = new Dictionary<int, int>();

	private readonly Dictionary<int, (ItemKey, bool)> _itemKeys = new Dictionary<int, (ItemKey, bool)>();

	private readonly Dictionary<int, EventLogCharacterData> _npcStatus = new Dictionary<int, EventLogCharacterData>();

	private readonly EventLogCharacterData _taiwuStatus = new EventLogCharacterData(isTaiwu: true);

	private List<EventLogResultData> _resultCache = new List<EventLogResultData>();

	private string _rawResponseData = "";

	private (int, int) _interactingCharacters = (-1, -1);

	private int _adventureId = -1;

	private bool _isCheckValid = false;

	private bool _isSequential = false;

	private bool _shouldCheckStatusImmediately = true;

	private readonly HashSet<IntPair> _executedOncePerMonthOptions = new HashSet<IntPair>();

	private static List<EventPackage> _packagesList;

	private static List<(TaiwuEventOption TaiwuEventOption, short templateId, TaiwuEventItem TaiwuEventItem)> _characterInteractionEventOptionList = new List<(TaiwuEventOption, short, TaiwuEventItem)>();

	private static EEventPackageLoadMethod _loadMethod;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private EventArgBox _globalArgBox;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private MonthlyEventActionsManager _monthlyEventActionManager;

	private static EventScriptRuntime _scriptRuntime;

	private readonly HashSet<string> _selectedTemporaryOptions = new HashSet<string>();

	[Obsolete]
	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private ushort _eventCount;

	[Obsolete("使用DisplayEventType.StartDoctorHeal方式实现")]
	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private int _healDoctorCharId = -1;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private string _cgName;

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private EventNotifyData _notifyData;

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private bool _hasListeningEvent;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private EventSelectInformationData _selectInformationData;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private EventCricketBettingData _cricketBettingData;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private EventSelectCombatSkillData _selectCombatSkillData;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private EventSelectLifeSkillData _selectLifeSkillData;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _taiwuLocationChangeFlag;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private bool _secretVillageOnFire;

	[DomainData(DomainDataType.SingleValue, true, false, true, true)]
	private bool _taiwuVillageShowShrine;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _hideAllTeammates;

	[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 3)]
	private int[] _allCombatGroupChars;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private string _leftRoleAlternativeName;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private string _rightRoleAlternativeName;

	[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 2)]
	private sbyte[] _rightRoleXiangshuDisplayData;

	[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 3)]
	private ItemDisplayData[] _itemListOfLeft;

	[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 3)]
	private ItemDisplayData[] _itemListOfRight;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _showItemWithCricketBattleGuess;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<sbyte> _coverCricketJarGradeListForRight;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _marriageLook1CharIdList;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _marriageLook2CharIdList;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _jieqingMaskCharIdList;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[29][];

	private Queue<uint> _pendingLoadingOperationIds;

	public bool ShowInteractCheckAnimation { get; set; }

	public EventInteractCheckData InteractCheckData { get; set; }

	private TaiwuEvent ShowingEvent
	{
		get
		{
			return _showingEvent;
		}
		set
		{
			_showingEvent = value;
			Events.RaiseEventWindowFocusStateChanged(MainThreadDataContext, !_showingEvent.IsEmpty);
		}
	}

	public bool IsShowingEvent
	{
		get
		{
			TaiwuEvent showingEvent = _showingEvent;
			return showingEvent != null && !showingEvent.IsEmpty;
		}
	}

	public sbyte LegacyReason { get; private set; }

	[Obsolete]
	public string ListeningAction { get; private set; }

	[Obsolete]
	public static bool IsQuickStartGame { get; private set; }

	public EventScriptRuntime ScriptRuntime => _scriptRuntime;

	public override void OnUpdate(DataContext context)
	{
		_scriptRuntime?.Update();
	}

	private void OnInitializedDomainData()
	{
		MainThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
		_argBoxPool = new LocalObjectPool<EventArgBox>(5, 65535);
		SetHasListeningEvent(value: false, MainThreadDataContext);
		SetHealDoctorCharId(-1, MainThreadDataContext);
		SetAwayForeverLoverCharId(-1, MainThreadDataContext);
		SetRightRoleXiangshuDisplayData(new sbyte[2] { 9, 0 }, MainThreadDataContext);
	}

	private void InitializeOnInitializeGameDataModule()
	{
		_scriptRuntime = new EventScriptRuntime(MainThreadDataContext, enableDebugging: true);
		_managerArray = new EventManagerBase[8];
		_managerArray[0] = new MainStoryEventManager();
		_managerArray[5] = new AdventureEventManager();
		_managerArray[4] = new NpcEventManager();
		_managerArray[7] = new TutorialEventManager();
		_managerArray[1] = new GlobalCommonEventManager();
		_managerArray[6] = new ModEventManager();
		InitConchShipEvents();
	}

	private void InitializeOnEnterNewWorld()
	{
		InitRuntimeEnvironment();
		DataUid uid = new DataUid(0, 1, ulong.MaxValue);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(uid, "InitializeMonthlyActions", InitializeMonthlyActions);
	}

	private void OnLoadedArchiveData()
	{
		InitRuntimeEnvironment();
		_monthlyEventActionManager.OnArchiveDataLoaded();
	}

	public override void FixAbnormalDomainArchiveData(DataContext dataContext)
	{
		if (!_monthlyEventActionManager.IsInitialized)
		{
			_monthlyEventActionManager.Init();
			SetMonthlyEventActionManager(_monthlyEventActionManager, dataContext);
		}
		_monthlyEventActionManager.HandleInvalidActions();
		BugFixForSpiritLandDisappear();
		FixFirstMartialArtTournament(dataContext);
		TaskCheckDaoShiAskForWildFood();
		FixShixiangExceptionTaskState(dataContext);
		FixShixiangArgKeysWrongState(dataContext);
		FixJingangInformation(dataContext);
		FixShixiangDrumEasterEggArgs(dataContext);
		ShaolinSectMainStoryInteractionSetData(dataContext);
	}

	private void InitRuntimeEnvironment()
	{
		_triggeredEventList = new List<TaiwuEvent>();
		ShowingEvent = TaiwuEvent.Empty;
		_notifyData = EventNotifyData.Empty;
		SetTempCreateItemList(new List<ItemKey>(), MainThreadDataContext);
		for (int i = 0; i < _managerArray.Length; i++)
		{
			_managerArray[i]?.ClearExtendOptions();
		}
		string path = Path.Combine(GameData.ArchiveData.Common.ArchiveBaseDir, "EventScriptRuntimeSettings.json");
		ScriptRuntime.LoadSettings(path);
		Events.RegisterHandler_AdvanceMonthBegin(OnAdvanceMonthBegin);
	}

	private void InitializeMonthlyActions(DataContext context, DataUid dataUid)
	{
		if (DomainManager.Global.GetLoadedAllArchiveData())
		{
			_monthlyEventActionManager.Init();
			SetMonthlyEventActionManager(_monthlyEventActionManager, context);
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(dataUid, "InitializeMonthlyActions");
		}
	}

	private bool CanNextEvent()
	{
		if (_stopAutoNextEvent)
		{
			return false;
		}
		if (DomainManager.Taiwu.GetLegacyPassingState() != 0)
		{
			return false;
		}
		TaiwuEvent taiwuEvent = null;
		if (_triggeredEventList.Count > 0)
		{
			taiwuEvent = _triggeredEventList[0];
		}
		if (taiwuEvent == null)
		{
			return false;
		}
		if (!NotAffectedInProgress(taiwuEvent.EventConfig.TriggerType) && DomainManager.World.GetAdvancingMonthState() == 20)
		{
			return false;
		}
		if (DomainManager.Combat.IsInCombat() && taiwuEvent.EventConfig.TriggerType != EventTrigger.CombatOpening && taiwuEvent.EventConfig.TriggerType != EventTrigger.SoulWitheringBellTransfer)
		{
			return false;
		}
		return true;
	}

	private void ResetArgBoxEventSelectData(EventArgBox argBox)
	{
		argBox.Set("SelectItemInfo", (ISerializableGameData)null);
		argBox.Set("SelectCharacterData", (ISerializableGameData)null);
		argBox.Set("InputRequestData", (ISerializableGameData)null);
		argBox.Set("SelectReadingBookCount", (ISerializableGameData)null);
		argBox.Set("SelectNeigongLoopingCount", (ISerializableGameData)null);
		argBox.Set("SelectFameData", (ISerializableGameData)null);
		argBox.Set("SelectFuyuFaithCount", (ISerializableGameData)null);
	}

	private void UpdateEventDisplayData()
	{
		TaiwuEvent showingEvent = ShowingEvent;
		if (showingEvent != null && !showingEvent.IsEmpty)
		{
			_eventEnteredList.Clear();
			TaiwuEvent showingEvent2;
			do
			{
				showingEvent2 = ShowingEvent;
				ShowingEvent.ArgBox = ShowingEvent.ArgBox;
				if (!ShowingEvent.TryExecuteScript(_scriptRuntime) && !_eventEnteredList.Contains(ShowingEvent.EventGuid))
				{
					_eventEnteredList.Add(ShowingEvent.EventGuid);
					ShowingEvent.EventConfig.OnEventEnter();
				}
			}
			while (showingEvent2 != ShowingEvent && !ShowingEvent.IsEmpty);
		}
		try
		{
			showingEvent = ShowingEvent;
			if (showingEvent != null && !showingEvent.IsEmpty)
			{
				if (!string.IsNullOrEmpty(ShowingEvent.EventConfig.TargetRoleKey))
				{
					GameData.Domains.Character.Character character = ShowingEvent.ArgBox.GetCharacter(ShowingEvent.EventConfig.TargetRoleKey);
					if (character != null && character.GetId() != EventArgBox.TaiwuCharacterId)
					{
						if (character.GetCreatingType() == 1)
						{
							DomainManager.Character.TryCreateRelation(MainThreadDataContext, EventArgBox.TaiwuCharacterId, character.GetId());
						}
						DomainManager.Extra.AddInteractedCharacter(MainThreadDataContext, character.GetId());
					}
				}
				TaiwuEventDisplayData value = ShowingEvent.ToDisplayData();
				SetDisplayingEventData(value, MainThreadDataContext);
				return;
			}
		}
		catch (Exception ex)
		{
			AdaptableLog.Info(ShowingEvent.EventGuid);
			AdaptableLog.Warning(ex.ToString());
			ShowingEvent = TaiwuEvent.Empty;
			if (_triggeredEventList.Count > 0)
			{
				NextEvent();
			}
		}
		SetDisplayingEventData(null, MainThreadDataContext);
		if (!IsShowingEvent && _triggeredEventList.Count <= 0)
		{
			Events.RaiseEventHandleComplete(MainThreadDataContext);
		}
	}

	private void TriggerHandled()
	{
		List<TaiwuEvent> triggeredEventList = _triggeredEventList;
		bool flag = triggeredEventList != null && triggeredEventList.Count > 0 && _triggeredEventList.First().EventConfig.IgnoreShowingEvent;
		if (!flag)
		{
			TaiwuEvent showingEvent = ShowingEvent;
			if (showingEvent != null && !showingEvent.IsEmpty)
			{
				return;
			}
		}
		if (_triggeredEventList == null || _triggeredEventList.Count <= 0)
		{
			Events.RaiseEventHandleComplete(MainThreadDataContext);
			return;
		}
		TaiwuEvent taiwuEvent = _triggeredEventList[0];
		if (!NotAffectedInProgress(taiwuEvent.EventConfig.TriggerType) && DomainManager.World.GetAdvancingMonthState() != 0)
		{
			return;
		}
		if (!flag)
		{
			TaiwuEvent showingEvent = ShowingEvent;
			if (showingEvent == null || !showingEvent.IsEmpty)
			{
				return;
			}
		}
		if (CanNextEvent())
		{
			NextEvent();
		}
	}

	private void NextEvent()
	{
		ShowingEvent = TaiwuEvent.Empty;
		if (_triggeredEventList.Count <= 0)
		{
			SetDisplayingEventData(null, MainThreadDataContext);
			return;
		}
		int i = 0;
		for (int count = _triggeredEventList.Count; i < count; i++)
		{
			if (_triggeredEventList[i].EventConfig.CheckCondition())
			{
				ShowingEvent = _triggeredEventList[i];
				_triggeredEventList.RemoveAt(i);
				break;
			}
		}
		TaiwuEvent showingEvent = ShowingEvent;
		if (showingEvent != null && showingEvent.IsEmpty)
		{
			_triggeredEventList.ForEach(delegate(TaiwuEvent e)
			{
				AdaptableLog.Warning("event " + e.EventGuid + " has triggered but failed to execute,removed trigger");
				e.ArgBox = null;
			});
			_triggeredEventList.Clear();
		}
		UpdateEventDisplayData();
	}

	private bool NotAffectedInProgress(short triggerType)
	{
		return triggerType == EventTrigger.NeedToPassLegacy || triggerType == EventTrigger.LifeSkillCombatForceSilent || triggerType == EventTrigger.CombatOpening;
	}

	private void HandleOptionConsume(TaiwuEventOption option, string mainRoleKey, string targetRoleKey)
	{
		if (option != null && option.OptionConsumeInfos != null)
		{
			GameData.Domains.Character.Character character = option.ArgBox.GetCharacter("RoleTaiwu");
			GameData.Domains.Character.Character character2 = null;
			if (!string.IsNullOrEmpty(targetRoleKey))
			{
				character2 = option.ArgBox.GetCharacter(targetRoleKey);
			}
			for (int i = 0; i < option.OptionConsumeInfos.Count; i++)
			{
				OptionConsumeInfo info = OptionConsumeHelper.ModifyOptionConsumeInfo(option.OptionConsumeInfos[i], option.ArgBox);
				info.DoConsume(character.GetId(), character2?.GetId() ?? (-1));
			}
		}
	}

	private void HandlerOptionEffect(TaiwuEventOption option)
	{
		if (option == null)
		{
			return;
		}
		sbyte b = EventOptionBehavior.ToBehaviorType[option.Behavior];
		if (b != -1)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			sbyte behaviorType = taiwu.GetBehaviorType();
			if (behaviorType == b)
			{
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.ChangeRoleHappiness(taiwu, 5);
			}
			else if (GameData.Domains.Character.BehaviorType.IsContradictory(behaviorType, b))
			{
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.ChangeRoleHappiness(taiwu, -5);
			}
			short behaviorChangeDeltaByEventSelect = GameData.Domains.Character.BehaviorType.GetBehaviorChangeDeltaByEventSelect(b, taiwu.GetBaseMorality());
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.ChangeRoleBaseBehaviorValue(taiwu, behaviorChangeDeltaByEventSelect);
		}
	}

	private bool IsCharacterRelatedToEvent(TaiwuEvent eventItem, int charId)
	{
		if (eventItem.IsEmpty)
		{
			return false;
		}
		if (!string.IsNullOrEmpty(eventItem.EventConfig.MainRoleKey))
		{
			int arg = -1;
			if (eventItem.ArgBox.Get(eventItem.EventConfig.MainRoleKey, ref arg) && arg == charId)
			{
				return true;
			}
		}
		if (!string.IsNullOrEmpty(eventItem.EventConfig.TargetRoleKey))
		{
			int arg2 = -1;
			if (eventItem.ArgBox.Get(eventItem.EventConfig.TargetRoleKey, ref arg2) && arg2 == charId)
			{
				return true;
			}
		}
		return false;
	}

	private void OnAdvanceMonthBegin(DataContext context)
	{
		List<ItemKey> tempCreateItemList = GetTempCreateItemList();
		if (tempCreateItemList != null && tempCreateItemList.Count > 0)
		{
			DomainManager.Item.RemoveItems(context, tempCreateItemList);
		}
		tempCreateItemList.Clear();
		SetTempCreateItemList(tempCreateItemList, MainThreadDataContext);
		_executedOncePerMonthOptions.Clear();
	}

	private bool IsEventStay(string eventGuid, string optionKey)
	{
		if (_selectInformationData != null && _selectInformationData.AvailableData && !_selectInformationData.SelectComplete)
		{
			if (_selectInformationData.IsForShopping)
			{
				return false;
			}
			_selectInformationData.SelectForEventGuid = eventGuid;
			_selectInformationData.SelectForOptionKey = optionKey;
			return true;
		}
		if (_cricketBettingData.IsValid && !_cricketBettingData.IsComplete)
		{
			_cricketBettingData.SelectForEventGuid = eventGuid;
			_cricketBettingData.SelectForOptionKey = optionKey;
			return true;
		}
		return false;
	}

	public EventArgBox GetEventArgBox()
	{
		return _argBoxPool.Get();
	}

	public void ReturnArgBox(EventArgBox argBox)
	{
		if (argBox != null)
		{
			argBox.Clear();
			_argBoxPool.Return(argBox);
		}
	}

	public bool IsTriggeredEvent(string guid)
	{
		if (ShowingEvent != null && ShowingEvent.EventGuid == guid)
		{
			return true;
		}
		if (_triggeredEventList != null)
		{
			foreach (TaiwuEvent triggeredEvent in _triggeredEventList)
			{
				if (triggeredEvent.EventGuid == guid)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetEventTriggeredCount(string guid)
	{
		int num = 0;
		if (ShowingEvent != null && ShowingEvent.EventGuid == guid)
		{
			num++;
		}
		if (_triggeredEventList != null)
		{
			foreach (TaiwuEvent triggeredEvent in _triggeredEventList)
			{
				if (triggeredEvent.EventGuid == guid)
				{
					num++;
				}
			}
		}
		return num;
	}

	public void AddTriggeredEvent(TaiwuEvent eventItem)
	{
		bool flag = false;
		if (_triggeredEventList.Count > 0)
		{
			for (int i = 0; i < _triggeredEventList.Count; i++)
			{
				if (_triggeredEventList[i].EventConfig.EventSortingOrder < eventItem.EventConfig.EventSortingOrder)
				{
					if (!_triggeredEventList.Contains(eventItem))
					{
						_triggeredEventList.Insert(i, eventItem);
					}
					flag = true;
					break;
				}
			}
		}
		if (!flag && !_triggeredEventList.Contains(eventItem))
		{
			_triggeredEventList.Add(eventItem);
		}
		AdaptableLog.Info($"new Event triggered : {eventItem.EventGuid}, _triggeredEventList.Count = {_triggeredEventList.Count}");
	}

	public void ToEvent(string eventGuid)
	{
		if (ShowingEvent.IsEmpty)
		{
			throw new Exception("Failed to new event because no event showing!");
		}
		if (eventGuid == ShowingEvent.EventGuid)
		{
			throw new Exception(eventGuid + " try to use ToEvent to self,this is not allowed!");
		}
		ShowingEvent.EventConfig?.OnEventExit();
		AdaptableLog.Info(ShowingEvent.EventGuid + " to event => " + eventGuid);
		if (string.IsNullOrEmpty(eventGuid))
		{
			ShowingEvent = TaiwuEvent.Empty;
			if (CanNextEvent() && !GetHasListeningEvent())
			{
				NextEvent();
				return;
			}
			Events.RaiseEventHandleComplete(MainThreadDataContext);
			if (DomainManager.World.GetAdvancingMonthState() != 0 && !GetHasListeningEvent())
			{
				DomainManager.World.SetOnHandingMonthlyEventBlock(value: false, DataContextManager.GetCurrentThreadDataContext());
			}
			return;
		}
		TaiwuEvent taiwuEvent = GetEvent(eventGuid);
		if (taiwuEvent == null)
		{
			return;
		}
		taiwuEvent.ArgBox = ShowingEvent.ArgBox;
		ShowingEvent.ArgBox = null;
		if (taiwuEvent.EventConfig.CheckCondition())
		{
			ShowingEvent = taiwuEvent;
			return;
		}
		taiwuEvent.ArgBox = null;
		if (CanNextEvent())
		{
			NextEvent();
		}
		else
		{
			ShowingEvent = TaiwuEvent.Empty;
		}
	}

	public void SetStopAutoNextEvent(bool flag)
	{
		_stopAutoNextEvent = flag;
	}

	public TaiwuEvent GetEvent(string guid)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			TaiwuEvent taiwuEvent = _managerArray[i]?.GetEvent(guid);
			if (taiwuEvent != null)
			{
				return taiwuEvent;
			}
		}
		return null;
	}

	public void TravelingEventCheckComplete()
	{
		TriggerHandled();
	}

	[DomainMethod]
	public void SetItemSelectResult(string key, ItemKey itemKey, bool callComplete)
	{
		if (ShowingEvent != null && !string.IsNullOrEmpty(key) && ShowingEvent.ArgBox.Get("SelectItemInfo", out EventSelectItemData arg))
		{
			ShowingEvent.ArgBox.Set(key, (ISerializableGameData)(object)itemKey);
			if (callComplete)
			{
				arg.OnSelectFinish?.Invoke();
			}
		}
	}

	[DomainMethod]
	public void SetItemSelectCount(string key, int count)
	{
		if (ShowingEvent != null && !string.IsNullOrEmpty(key) && ShowingEvent.ArgBox.Get("SelectItemInfo", out EventSelectItemData _))
		{
			ShowingEvent.ArgBox.Set(key + "Count", count);
		}
	}

	[DomainMethod]
	public void SetCharacterSelectResult(string key, int charId, bool callComplete)
	{
		if (ShowingEvent != null && !string.IsNullOrEmpty(key) && ShowingEvent.ArgBox.Get("SelectCharacterData", out EventSelectCharacterData arg))
		{
			ShowingEvent.ArgBox.Set(key, charId);
			if (callComplete)
			{
				arg.OnSelectComplete?.Invoke();
			}
		}
	}

	[DomainMethod]
	public void SetCharacterMultSelectResult(string key, List<int> charIds, bool callComplete)
	{
		if (ShowingEvent != null && !string.IsNullOrEmpty(key) && ShowingEvent.ArgBox.Get("SelectCharacterData", out EventSelectCharacterData arg))
		{
			IntList intList = IntList.Create();
			intList.Items = charIds;
			ShowingEvent.ArgBox.Set(key, (ISerializableGameData)(object)intList);
			if (callComplete)
			{
				arg.OnSelectComplete?.Invoke();
			}
		}
	}

	[DomainMethod]
	public void SetCharacterSetSelectResult(string actionName, string key, CharacterSet characterSet)
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		for (int i = 0; i < ListeningEventActionList.Count; i++)
		{
			if (ListeningEventActionList[i].listeningAction.Equals(actionName))
			{
				TaiwuEvent item = ListeningEventActionList[i].listenerEvent;
				if (item != null && !item.IsEmpty)
				{
					ListeningEventActionList[i].listenerEvent.ArgBox.Set(key, (ISerializableGameData)(object)characterSet);
				}
			}
		}
	}

	[DomainMethod]
	public void SetSecretInformationSelectResult(string key, int informationMetaDataId)
	{
		if (_selectInformationData != null)
		{
			if (SecretInformationDisplayData.IsSecretInformationValid(informationMetaDataId))
			{
				ShowingEvent.ArgBox.Set(key, informationMetaDataId);
				_selectInformationData.SelectComplete = true;
				EventSelect(_selectInformationData.SelectForEventGuid, _selectInformationData.SelectForOptionKey);
			}
			else
			{
				UpdateEventDisplayData();
			}
			SetSelectInformationData(null, MainThreadDataContext);
		}
	}

	[DomainMethod]
	public void SetNormalInformationSelectResult(string key, NormalInformation normalInformation)
	{
		if (_selectInformationData != null)
		{
			if (normalInformation.IsValid())
			{
				ShowingEvent.ArgBox.Set(key, (ISerializableGameData)(object)normalInformation);
				_selectInformationData.SelectComplete = true;
				EventSelect(_selectInformationData.SelectForEventGuid, _selectInformationData.SelectForOptionKey);
			}
			else
			{
				UpdateEventDisplayData();
			}
			SetSelectInformationData(null, MainThreadDataContext);
		}
	}

	[DomainMethod]
	public void SetCombatSkillSelectResult(short combatSkillId)
	{
		TaiwuEvent showingEvent = ShowingEvent;
		if (showingEvent != null && !showingEvent.IsEmpty && _selectCombatSkillData != null)
		{
			if (combatSkillId >= 0)
			{
				ShowingEvent.ArgBox.Set(_selectCombatSkillData.ResultSaveKey, combatSkillId);
			}
			SetSelectCombatSkillData(null, MainThreadDataContext);
		}
	}

	[DomainMethod]
	public void SetLifeSkillSelectResult(short lifeSkillId)
	{
		TaiwuEvent showingEvent = ShowingEvent;
		if (showingEvent != null && !showingEvent.IsEmpty && _selectLifeSkillData != null)
		{
			if (lifeSkillId >= 0)
			{
				ShowingEvent.ArgBox.Set(_selectLifeSkillData.ResultSaveKey, lifeSkillId);
			}
			SetSelectLifeSkillData(null, MainThreadDataContext);
		}
	}

	[DomainMethod]
	public void SetCricketBettingResult(bool ok, Wager wager, int index)
	{
		if (_cricketBettingData.IsValid)
		{
			_cricketBettingData.IsValid = false;
			_cricketBettingData.IsComplete = ok;
			_cricketBettingData.IsConfirmed = ok;
			_cricketBettingData.Wager = wager;
			_cricketBettingData.Index = index;
			SetCricketBettingData(_cricketBettingData, MainThreadDataContext);
			if (ok)
			{
				CricketWagerData cricketWagerData = _cricketBettingData.BetRewards[index];
				DomainManager.Item.SetWager(wager, cricketWagerData.Wager);
				EventSelect(_cricketBettingData.SelectForEventGuid, _cricketBettingData.SelectForOptionKey);
			}
			else
			{
				UpdateEventDisplayData();
			}
		}
	}

	[DomainMethod]
	public void SetSelectCount(int count)
	{
		if (ShowingEvent != null)
		{
			ShowingEvent.ArgBox.Set("SelectCountResult", count);
		}
	}

	[DomainMethod]
	public void StartHandleEventDuringAdvance()
	{
		NextEvent();
	}

	[DomainMethod]
	public List<TaiwuEventSummaryDisplayData> GetTriggeredEventSummaryDisplayData()
	{
		List<TaiwuEventSummaryDisplayData> list = new List<TaiwuEventSummaryDisplayData>();
		foreach (TaiwuEvent triggeredEvent in _triggeredEventList)
		{
			TaiwuEventSummaryDisplayData taiwuEventSummaryDisplayData = triggeredEvent.ToSummaryDisplayData();
			if (taiwuEventSummaryDisplayData != null)
			{
				list.Add(taiwuEventSummaryDisplayData);
			}
		}
		return list;
	}

	[DomainMethod]
	public void SetEventInProcessing(string eventGuid)
	{
		foreach (TaiwuEvent triggeredEvent in _triggeredEventList)
		{
			if (triggeredEvent.EventGuid == eventGuid)
			{
				ShowingEvent = triggeredEvent;
				_triggeredEventList.Remove(triggeredEvent);
				UpdateEventDisplayData();
				break;
			}
		}
	}

	public void ProcessEventWithDefaultOption(string guid, EventArgBox eventArgBox, sbyte behaviorType)
	{
		HashSet<string> hashSet = ObjectPool<HashSet<string>>.Instance.Get();
		hashSet.Clear();
		while (!string.IsNullOrEmpty(guid))
		{
			if (!hashSet.Add(guid))
			{
				throw new Exception("Loop detected when executing event " + guid);
			}
			TaiwuEvent taiwuEvent = GetEvent(guid);
			if (taiwuEvent.ArgBox != null)
			{
				ReturnArgBox(taiwuEvent.ArgBox);
			}
			taiwuEvent.ArgBox = eventArgBox;
			_triggeredEventList.Remove(taiwuEvent);
			if (!taiwuEvent.EventConfig.CheckCondition())
			{
				break;
			}
			taiwuEvent.EventConfig.OnEventEnter();
			if (string.IsNullOrEmpty(taiwuEvent.EventConfig.EscOptionKey))
			{
				if (taiwuEvent.EventConfig.EventOptions == null || taiwuEvent.EventConfig.EventOptions.Length == 0)
				{
					Logger.AppendWarning("Monthly event " + taiwuEvent.EventGuid + " has no option detected when trying to process with default option.");
					guid = string.Empty;
				}
				else
				{
					bool flag = false;
					TaiwuEventOption[] eventOptions = taiwuEvent.EventConfig.EventOptions;
					foreach (TaiwuEventOption taiwuEventOption in eventOptions)
					{
						if (EventOptionBehavior.ToBehaviorType[taiwuEventOption.Behavior] == behaviorType)
						{
							guid = taiwuEventOption.Select(_scriptRuntime);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Logger.AppendWarning("Monthly event " + taiwuEvent.EventGuid + " has neither esc option nor behavior option to handle as default.");
						guid = string.Empty;
					}
				}
			}
			else
			{
				guid = taiwuEvent.EventConfig[taiwuEvent.EventConfig.EscOptionKey].Select(_scriptRuntime);
			}
			taiwuEvent.EventConfig.OnEventExit();
			taiwuEvent.ArgBox = null;
		}
		ObjectPool<HashSet<string>>.Instance.Return(hashSet);
	}

	[DomainMethod]
	public void EventSelect(string eventGuid, string optionKey, bool isContinue = false)
	{
		TaiwuEvent showingEvent = ShowingEvent;
		if ((showingEvent != null && showingEvent.IsEmpty) || eventGuid != ShowingEvent.EventGuid)
		{
			return;
		}
		TaiwuEventItem eventConfig = ShowingEvent.EventConfig;
		EventArgBox argBox = ShowingEvent.ArgBox;
		TaiwuEventOption taiwuEventOption = eventConfig[optionKey];
		string text;
		if (isContinue)
		{
			ShowInteractCheckAnimation = false;
			text = _interactCheckContinueData.nextGuid;
			_interactCheckContinueData = (EventGuid: string.Empty, OptionKey: string.Empty, nextGuid: string.Empty);
		}
		else
		{
			text = taiwuEventOption.Select(_scriptRuntime);
		}
		if (ShowInteractCheckAnimation)
		{
			_interactCheckContinueData = (EventGuid: eventGuid, OptionKey: optionKey, nextGuid: text);
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.InteractCheckAnimation, InteractCheckData);
			return;
		}
		int arg = -1;
		GenerateResponseLog(optionKey, argBox.Get("ConchShip_PresetKey_EventLogMainCharacter", ref arg) ? arg : (-1));
		if (IsEventStay(eventGuid, optionKey))
		{
			return;
		}
		string arg2 = string.Empty;
		if (eventConfig.ArgBox.Get("ConchShip_PresetKey_OptionWaitConfirm", ref arg2))
		{
			_waitConfirmOptionKey = arg2;
			_waitConfirmEventConfig = eventConfig;
			_waitConfirmSelectOption = optionKey;
			eventConfig.ArgBox.Remove<string>("ConchShip_PresetKey_OptionWaitConfirm");
		}
		else
		{
			string arg3 = string.Empty;
			if (eventConfig.ArgBox.Get("ConchShip_PresetKey_ConfirmWaitOptionSignal", ref arg3))
			{
				if (arg3 == _waitConfirmOptionKey)
				{
					_waitConfirmEventConfig.ArgBox = eventConfig.ArgBox;
					HandleOptionConsume(_waitConfirmEventConfig[_waitConfirmSelectOption], _waitConfirmEventConfig.MainRoleKey, _waitConfirmEventConfig.TargetRoleKey);
					HandlerOptionEffect(_waitConfirmEventConfig[_waitConfirmSelectOption]);
					_waitConfirmOptionKey = string.Empty;
					_waitConfirmEventConfig = null;
					_waitConfirmSelectOption = string.Empty;
				}
				eventConfig.ArgBox.Remove<string>("ConchShip_PresetKey_ConfirmWaitOptionSignal");
			}
			HandleOptionConsume(eventConfig[optionKey], eventConfig.MainRoleKey, eventConfig.TargetRoleKey);
			HandlerOptionEffect(eventConfig[optionKey]);
		}
		eventConfig.OnEventExit();
		ResetArgBoxEventSelectData(argBox);
		if (string.IsNullOrEmpty(text))
		{
			if (GetHasListeningEvent())
			{
				TaiwuEvent showingEvent2 = _showingEvent;
				List<(string listeningAction, TaiwuEvent listenerEvent)> listeningEventActionList = ListeningEventActionList;
				if (showingEvent2 != listeningEventActionList[listeningEventActionList.Count - 1].listenerEvent)
				{
					ShowingEvent.ArgBox = null;
				}
			}
			ShowingEvent = TaiwuEvent.Empty;
			if (GetHasListeningEvent())
			{
				SetDisplayingEventData(null, MainThreadDataContext);
				Events.RaiseEventHandleComplete(MainThreadDataContext);
				return;
			}
			if (CanNextEvent())
			{
				NextEvent();
				return;
			}
			SetDisplayingEventData(null, MainThreadDataContext);
			Events.RaiseEventHandleComplete(MainThreadDataContext);
			if (DomainManager.World.GetAdvancingMonthState() != 0)
			{
				DomainManager.World.SetOnHandingMonthlyEventBlock(value: false, DataContextManager.GetCurrentThreadDataContext());
			}
			return;
		}
		AdaptableLog.Info("select option to next Event: " + text);
		ShowingEvent.ArgBox = null;
		TaiwuEvent taiwuEvent = GetEvent(text);
		if (taiwuEvent != null)
		{
			taiwuEvent.ArgBox = argBox;
			if (taiwuEvent.EventConfig.CheckCondition())
			{
				ShowingEvent = taiwuEvent;
				UpdateEventDisplayData();
			}
			else if (CanNextEvent())
			{
				NextEvent();
			}
			else
			{
				ShowingEvent = TaiwuEvent.Empty;
				SetDisplayingEventData(null, MainThreadDataContext);
				Events.RaiseEventHandleComplete(MainThreadDataContext);
			}
		}
		else
		{
			AdaptableLog.TagError("TaiwuEvent", "can not find event " + text);
		}
	}

	[DomainMethod]
	public void EventSelectContinue()
	{
		EventSelect(_interactCheckContinueData.EventGuid, _interactCheckContinueData.OptionKey, isContinue: true);
	}

	[DomainMethod]
	public List<int> GetImplementedFunctionIds(DataContext context)
	{
		return new List<int>(_scriptRuntime.ImplementedFunctionIds);
	}

	[DomainMethod]
	[Obsolete("use UpdateEventDisplayData instead")]
	public List<TaiwuEventDisplayData> GetEventDisplayData()
	{
		return null;
	}

	[DomainMethod]
	public void SetShowingEventShortListArg(string key, GameData.Utilities.ShortList value)
	{
		if (ShowingEvent != null)
		{
			ShowingEvent.ArgBox.Set(key, (ISerializableGameData)(object)value);
		}
	}

	private void BugFixForSpiritLandDisappear()
	{
		if (DomainManager.World.GetMainStoryLineProgress() != 26 || GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GlobalArgBoxContainsKey<int>("ImmortalXuMoveForSpiriteLand"))
		{
			return;
		}
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(EventArgBox.TaiwuVillageAreaId);
		foreach (KeyValuePair<short, AdventureSiteData> adventureSite in adventuresInArea.AdventureSites)
		{
			if (adventureSite.Value.TemplateId == 107 && adventureSite.Value.SiteState == 1)
			{
				return;
			}
		}
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.SaveGlobalArg("ImmortalXuMoveForSpiriteLand", 3);
	}

	private void FixFirstMartialArtTournament(DataContext context)
	{
		short mainStoryLineProgress = DomainManager.World.GetMainStoryLineProgress();
		if (mainStoryLineProgress == 22 && _globalArgBox.GetBool("WaitForFirstWulinConference"))
		{
			List<short> previousMartialArtTournamentHosts = DomainManager.Organization.GetPreviousMartialArtTournamentHosts();
			if (previousMartialArtTournamentHosts.Count > 0)
			{
				Logger.Warn($"Clearing previous martial art tournament hosts: count = {previousMartialArtTournamentHosts.Count}");
				previousMartialArtTournamentHosts.Clear();
				DomainManager.Organization.SetPreviousMartialArtTournamentHosts(previousMartialArtTournamentHosts, context);
			}
			MonthlyActionKey key = MonthlyEventActionsManager.PredefinedKeys["MartialArtTournamentDefault"];
			if (GetMonthlyAction(key) is MartialArtTournamentMonthlyAction { State: 0, LastFinishDate: >=0 } martialArtTournamentMonthlyAction)
			{
				martialArtTournamentMonthlyAction.LastFinishDate = int.MinValue;
				Logger.Warn($"Fixing last martial art tournament date: {martialArtTournamentMonthlyAction.LastFinishDate}");
			}
		}
	}

	private void FixShixiangExceptionTaskState(DataContext context)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(6);
		if ((sectMainStoryEventArgBox.Contains<int>("ConchShip_PresetKey_SectMainStoryShixiangProsperousEndDate") || DomainManager.World.GetSectMainStoryTaskStatus(6) != 0) && DomainManager.Extra.IsExtraTaskInProgress(233))
		{
			DomainManager.Extra.FinishTriggeredExtraTask(context, 34, 233);
			Logger.Warn("Fix Shixiang Exception TaskState");
		}
	}

	private void FixShixiangArgKeysWrongState(DataContext context)
	{
		if (!DomainManager.World.ShixiangSettlementAffiliatedBlocksHasEnemy(context, 613))
		{
			EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(6);
			if (sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_ShixiangToFightEnemy"))
			{
				sectMainStoryEventArgBox.Remove<bool>("ConchShip_PresetKey_ShixiangToFightEnemy");
				DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 6);
				Logger.Warn("Fix Shixiang ArgKeys WrongState,key name:ConchShip_PresetKey_ShixiangToFightEnemy");
			}
		}
	}

	private void FixJingangInformation(DataContext context)
	{
		if (DomainManager.World.GetSectMainStoryTaskStatus(11) != 0)
		{
			DomainManager.Information.ReleaseAllJingangInformation(context);
		}
	}

	private void FixShixiangDrumEasterEggArgs(DataContext context)
	{
		EventArgBox globalArgBox = GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetGlobalArgBox();
		bool arg = false;
		globalArgBox.Get("GivenShixiangDrumEasterEggItems", ref arg);
		bool arg2 = false;
		globalArgBox.Get("EnteredShixiangDrumEasterEggThisMonth", ref arg2);
		globalArgBox.Remove<bool>("GivenShixiangDrumEasterEggItems");
		if (arg)
		{
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.SaveArgToSectMainStory(6, "GivenShixiangDrumEasterEggItems", value: true);
		}
		globalArgBox.Remove<bool>("EnteredShixiangDrumEasterEggThisMonth");
		if (arg2)
		{
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.SaveArgToSectMainStory(6, "EnteredShixiangDrumEasterEggThisMonth", value: true);
		}
	}

	private void ShaolinSectMainStoryInteractionSetData(DataContext context)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(1);
		if (!sectMainStoryEventArgBox.Contains<bool>("ConchShip_PresetKey_ShaolinInteractionChangeFirstLoad"))
		{
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 1, "ConchShip_PresetKey_ShaolinInteractionChangeFirstLoad", value: true);
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(context, 1, "ConchShip_PresetKey_ShaolinMeditationInteractionFinished", sectMainStoryEventArgBox.GetBool("ConchShip_PresetKey_ShaolinMeditationInteractionActivated"));
		}
	}

	public void SetNewAdventure(short adventureId, EventArgBox argBox)
	{
		if (_managerArray[5] is AdventureEventManager adventureEventManager)
		{
			adventureEventManager.SetNewAdventure(adventureId, argBox);
		}
		TriggerHandled();
	}

	public void TriggerAdventureGlobalEvent(AdventureMapPoint node)
	{
		if (_managerArray[5] is AdventureEventManager adventureEventManager)
		{
			adventureEventManager.TriggerGlobalEvent(node);
		}
		TriggerHandled();
	}

	public void TriggerAdventureExtraEvent(string guid, AdventureMapPoint node)
	{
		if (_managerArray[5] is AdventureEventManager adventureEventManager)
		{
			adventureEventManager.TriggerExtraEvent(guid, node);
		}
		TriggerHandled();
	}

	public void SetNewAdventureByConfigData(AdventureItem configData, EventArgBox argBox)
	{
		if (_managerArray[5] is AdventureEventManager adventureEventManager)
		{
			adventureEventManager.SetNewAdventureByConfigData(configData, argBox);
		}
	}

	public void AppendMarriageLook1CharId(int charId)
	{
		if (_marriageLook1CharIdList == null)
		{
			_marriageLook1CharIdList = new List<int>();
		}
		if (!_marriageLook1CharIdList.Contains(charId))
		{
			_marriageLook1CharIdList.Add(charId);
			SetMarriageLook1CharIdList(_marriageLook1CharIdList, MainThreadDataContext);
			if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
			{
				element.SetAvatar(element.GetAvatar(), MainThreadDataContext);
			}
		}
	}

	public void RemoveMarriageLook1CharId(int charId)
	{
		if (_marriageLook1CharIdList != null && _marriageLook1CharIdList.Contains(charId))
		{
			_marriageLook1CharIdList.Remove(charId);
			SetMarriageLook1CharIdList(_marriageLook1CharIdList, MainThreadDataContext);
			if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
			{
				element.SetAvatar(element.GetAvatar(), MainThreadDataContext);
			}
		}
	}

	public void AppendMarriageLook2CharId(int charId)
	{
		if (_marriageLook2CharIdList == null)
		{
			_marriageLook2CharIdList = new List<int>();
		}
		if (!_marriageLook2CharIdList.Contains(charId))
		{
			_marriageLook2CharIdList.Add(charId);
			SetMarriageLook2CharIdList(_marriageLook2CharIdList, MainThreadDataContext);
			if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
			{
				element.SetAvatar(element.GetAvatar(), MainThreadDataContext);
			}
		}
	}

	public void RemoveMarriageLook2CharId(int charId)
	{
		if (_marriageLook2CharIdList != null && _marriageLook2CharIdList.Contains(charId))
		{
			_marriageLook2CharIdList.Remove(charId);
			SetMarriageLook2CharIdList(_marriageLook2CharIdList, MainThreadDataContext);
			if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
			{
				element.SetAvatar(element.GetAvatar(), MainThreadDataContext);
			}
		}
	}

	public void ClearAllMarriageLook()
	{
		_marriageLook1CharIdList?.Clear();
		_marriageLook2CharIdList?.Clear();
		SetMarriageLook1CharIdList(_marriageLook1CharIdList, MainThreadDataContext);
		SetMarriageLook2CharIdList(_marriageLook2CharIdList, MainThreadDataContext);
	}

	public void ClearAllTriggeredEvent()
	{
		if (ListeningEventActionList.Count > 0)
		{
			foreach (var listeningEventAction in ListeningEventActionList)
			{
				ReturnArgBox(listeningEventAction.listenerEvent.ArgBox);
			}
			ListeningEventActionList.Clear();
			SetHasListeningEvent(value: false, MainThreadDataContext);
		}
		foreach (TaiwuEvent triggeredEvent in _triggeredEventList)
		{
			ReturnArgBox(triggeredEvent.ArgBox);
		}
		_triggeredEventList.Clear();
	}

	public void OnPassingLegacyStateChange(sbyte newState)
	{
		if (newState == 0 && ShowingEvent.IsEmpty)
		{
			SetLegacyReason(-1);
			NextEvent();
		}
	}

	public void OnTemporaryIntelligentCharacterRemoved(int charId)
	{
		if (IsCharacterRelatedToEvent(ShowingEvent, charId))
		{
			throw new Exception($"{charId} remove error:{ShowingEvent.EventGuid} need this character");
		}
		foreach (TaiwuEvent triggeredEvent in _triggeredEventList)
		{
			if (IsCharacterRelatedToEvent(triggeredEvent, charId))
			{
				throw new Exception($"{charId} remove error:{ShowingEvent.EventGuid} need this character");
			}
		}
	}

	public void SetTaiwuLocationDirtyFlag()
	{
		_taiwuLocationChangeFlag = !_taiwuLocationChangeFlag;
		SetTaiwuLocationChangeFlag(_taiwuLocationChangeFlag, MainThreadDataContext);
	}

	public void OnCharacterDie(int charId)
	{
		if (_triggeredEventList.Count <= 0)
		{
			return;
		}
		for (int num = _triggeredEventList.Count - 1; num >= 0; num--)
		{
			TaiwuEvent eventItem = _triggeredEventList[num];
			if (IsCharacterRelatedToEvent(eventItem, charId))
			{
				_triggeredEventList.RemoveAt(num);
			}
		}
	}

	public void GMCharacterDie(int charId)
	{
		TaiwuEvent showingEvent = ShowingEvent;
		if (showingEvent != null && !showingEvent.IsEmpty && IsCharacterRelatedToEvent(ShowingEvent, charId))
		{
			ShowingEvent = TaiwuEvent.Empty;
			UpdateEventDisplayData();
		}
	}

	public void SetLegacyReason(sbyte reason)
	{
		LegacyReason = reason;
	}

	public void SetListenerWithActionName(string listenerEventGuid, EventArgBox eventBox, string actionName)
	{
		eventBox?.Remove<bool>(actionName);
		TaiwuEvent item = TaiwuEvent.Empty;
		if (!string.IsNullOrEmpty(listenerEventGuid))
		{
			TaiwuEvent taiwuEvent = GetEvent(listenerEventGuid);
			if (taiwuEvent != null)
			{
				taiwuEvent.ArgBox = eventBox;
				item = taiwuEvent;
			}
			else
			{
				item = TaiwuEvent.Empty;
			}
		}
		ListeningEventActionList.Add((actionName, item));
		SetHasListeningEvent(ListeningEventActionList.Count > 0, MainThreadDataContext);
	}

	public void RemoveEventInListenWithActionName(string eventGuid, string actionName)
	{
		for (int num = ListeningEventActionList.Count - 1; num >= 0; num--)
		{
			if (ListeningEventActionList[num].listeningAction.Equals(actionName) && ListeningEventActionList[num].listenerEvent.EventGuid.Equals(eventGuid))
			{
				ListeningEventActionList.RemoveAt(num);
			}
		}
		SetHasListeningEvent(ListeningEventActionList.Count > 0, MainThreadDataContext);
	}

	[DomainMethod]
	public void TriggerListener(string key, bool value)
	{
		(string, TaiwuEvent) tuple = (string.Empty, TaiwuEvent.Empty);
		for (int num = ListeningEventActionList.Count - 1; num >= 0; num--)
		{
			if (ListeningEventActionList[num].listeningAction.Equals(key))
			{
				tuple = ListeningEventActionList[num];
				ListeningEventActionList.RemoveAt(num);
				break;
			}
		}
		if (string.IsNullOrEmpty(tuple.Item1) || tuple.Item2.IsEmpty)
		{
			SetHasListeningEvent(ListeningEventActionList.Count > 0, MainThreadDataContext);
			Events.RaiseEventHandleComplete(MainThreadDataContext);
			return;
		}
		if (!string.IsNullOrEmpty(key))
		{
			tuple.Item2.ArgBox.Set(key, value);
		}
		SetHasListeningEvent(ListeningEventActionList.Count > 0, MainThreadDataContext);
		if (tuple.Item2.EventConfig.CheckCondition())
		{
			ShowingEvent = tuple.Item2;
			UpdateEventDisplayData();
		}
		else
		{
			Events.RaiseEventHandleComplete(MainThreadDataContext);
		}
	}

	[DomainMethod]
	public void SetListenerEventActionISerializableArg(string actionName, string key, ISerializableGameData value)
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		for (int i = 0; i < ListeningEventActionList.Count; i++)
		{
			if (ListeningEventActionList[i].listeningAction.Equals(actionName))
			{
				TaiwuEvent item = ListeningEventActionList[i].listenerEvent;
				if (item != null && !item.IsEmpty)
				{
					ListeningEventActionList[i].listenerEvent.ArgBox.Set(key, value);
				}
			}
		}
	}

	[DomainMethod]
	public void SetListenerEventActionIntListArg(string actionName, string key, IntList value)
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		for (int i = 0; i < ListeningEventActionList.Count; i++)
		{
			if (ListeningEventActionList[i].listeningAction.Equals(actionName))
			{
				TaiwuEvent item = ListeningEventActionList[i].listenerEvent;
				if (item != null && !item.IsEmpty)
				{
					ListeningEventActionList[i].listenerEvent.ArgBox.Set(key, (ISerializableGameData)(object)value);
				}
			}
		}
	}

	[DomainMethod]
	public void SetListenerEventActionShortListArg(string actionName, string key, GameData.Utilities.ShortList value)
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		for (int i = 0; i < ListeningEventActionList.Count; i++)
		{
			if (ListeningEventActionList[i].listeningAction.Equals(actionName))
			{
				TaiwuEvent item = ListeningEventActionList[i].listenerEvent;
				if (item != null && !item.IsEmpty)
				{
					ListeningEventActionList[i].listenerEvent.ArgBox.Set(key, (ISerializableGameData)(object)value);
				}
			}
		}
	}

	[DomainMethod]
	public void SetListenerEventActionItemKeyArg(string actionName, string key, ItemKey value)
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		for (int i = 0; i < ListeningEventActionList.Count; i++)
		{
			if (ListeningEventActionList[i].listeningAction.Equals(actionName))
			{
				TaiwuEvent item = ListeningEventActionList[i].listenerEvent;
				if (item != null && !item.IsEmpty)
				{
					ListeningEventActionList[i].listenerEvent.ArgBox.Set(key, (ISerializableGameData)(object)value);
				}
			}
		}
	}

	[DomainMethod]
	public void SetListenerEventActionIntArg(string actionName, string key, int value)
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		for (int i = 0; i < ListeningEventActionList.Count; i++)
		{
			if (ListeningEventActionList[i].listeningAction.Equals(actionName))
			{
				TaiwuEvent item = ListeningEventActionList[i].listenerEvent;
				if (item != null && !item.IsEmpty)
				{
					ListeningEventActionList[i].listenerEvent.ArgBox.Set(key, value);
				}
			}
		}
	}

	[DomainMethod]
	public void SetListenerEventActionBoolArg(string actionName, string key, bool value)
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		for (int i = 0; i < ListeningEventActionList.Count; i++)
		{
			if (ListeningEventActionList[i].listeningAction.Equals(actionName))
			{
				TaiwuEvent item = ListeningEventActionList[i].listenerEvent;
				if (item != null && !item.IsEmpty)
				{
					ListeningEventActionList[i].listenerEvent.ArgBox.Set(key, value);
				}
			}
		}
	}

	[DomainMethod]
	public void SetListenerEventActionStringArg(string actionName, string key, string value)
	{
		if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
		{
			return;
		}
		for (int i = 0; i < ListeningEventActionList.Count; i++)
		{
			if (ListeningEventActionList[i].listeningAction.Equals(actionName))
			{
				TaiwuEvent item = ListeningEventActionList[i].listenerEvent;
				if (item != null && !item.IsEmpty)
				{
					ListeningEventActionList[i].listenerEvent.ArgBox.Set(key, value);
				}
			}
		}
	}

	[Obsolete]
	public void SetListener(string listenerEventGuid, EventArgBox eventBox)
	{
		if (string.IsNullOrEmpty(listenerEventGuid))
		{
			_listenerEvent = TaiwuEvent.Empty;
		}
		else
		{
			TaiwuEvent taiwuEvent = GetEvent(listenerEventGuid);
			if (taiwuEvent != null)
			{
				taiwuEvent.ArgBox = eventBox;
				_listenerEvent = taiwuEvent;
			}
			else
			{
				_listenerEvent = TaiwuEvent.Empty;
			}
		}
		TaiwuEvent listenerEvent = _listenerEvent;
		SetHasListeningEvent(listenerEvent != null && !listenerEvent.IsEmpty, MainThreadDataContext);
	}

	[Obsolete]
	public void SetWaitActionName(string actionName)
	{
		ListeningAction = actionName;
	}

	[Obsolete]
	public void SetListenerISerializableArg(string key, ISerializableGameData value)
	{
		if (_listenerEvent != null && !_listenerEvent.IsEmpty && !string.IsNullOrEmpty(key))
		{
			_listenerEvent.ArgBox.Set(key, value);
		}
	}

	[Obsolete]
	public void SetListenerIntArg(string key, int value)
	{
		if (_listenerEvent != null && !_listenerEvent.IsEmpty && !string.IsNullOrEmpty(key))
		{
			_listenerEvent.ArgBox.Set(key, value);
		}
	}

	[Obsolete]
	public void SetListenerFloatArg(string key, float value)
	{
		if (_listenerEvent != null && !_listenerEvent.IsEmpty && !string.IsNullOrEmpty(key))
		{
			_listenerEvent.ArgBox.Set(key, value);
		}
	}

	[Obsolete]
	public void SetListenerBoolArg(string key, bool value)
	{
		if (_listenerEvent != null && !_listenerEvent.IsEmpty && !string.IsNullOrEmpty(key))
		{
			_listenerEvent.ArgBox.Set(key, value);
		}
	}

	[Obsolete]
	public void SetListenerStringArg(string key, string value)
	{
		if (_listenerEvent != null && !_listenerEvent.IsEmpty && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
		{
			_listenerEvent.ArgBox.Set(key, value);
		}
	}

	[Obsolete]
	public void SetListenerItemKeyArg(string key, ItemKey value)
	{
		TaiwuEvent listenerEvent = _listenerEvent;
		if (listenerEvent != null && !listenerEvent.IsEmpty && !string.IsNullOrEmpty(key))
		{
			_listenerEvent.ArgBox.Set(key, (ISerializableGameData)(object)value);
		}
	}

	[Obsolete]
	public void SetListenerIntListArg(string key, IntList value)
	{
		TaiwuEvent listenerEvent = _listenerEvent;
		if (listenerEvent != null && !listenerEvent.IsEmpty && !string.IsNullOrEmpty(key))
		{
			_listenerEvent.ArgBox.Set(key, (ISerializableGameData)(object)value);
		}
	}

	[DomainMethod]
	public EventLogData GetEventLogData()
	{
		EventLogData eventLogData = new EventLogData
		{
			CharacterList = _characterCache.Values.ToList(),
			SecretInformationList = _secretInformationCache.Values.ToList(),
			ItemList = _itemCache.Values.ToList(),
			CombatSkillList = _combatSkillCache.Values.ToList()
		};
		foreach (List<EventLogResultData> item in _eventLogQueue)
		{
			eventLogData.ResultList.AddRange(item);
		}
		return eventLogData;
	}

	[DomainMethod]
	public void StartNewDialog(DataContext context, IntPair charIds, string dialog, string rawResponseData, EventActorData leftActor, EventActorData rightActor, string leftName, string rightName, short merchantTemplateId)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		sbyte type = 0;
		if (IsResultCacheValid())
		{
			if (_eventLogQueue.Count > 99)
			{
				EventLogDequeue();
			}
			short curAdventureId = DomainManager.Adventure.GetCurAdventureId();
			if (_isCheckValid || _isSequential || (curAdventureId >= 0 && _adventureId >= 0))
			{
				CompareCharacterStatusRecord(taiwuCharId);
				if (_interactingCharacters.Item1 >= 0 && _interactingCharacters.Item1 != taiwuCharId)
				{
					if (_interactingCharacters.Item1 == charIds.First)
					{
						CompareCharacterStatusRecord(_interactingCharacters.Item1);
					}
					else if (_npcStatus.ContainsKey(_interactingCharacters.Item1))
					{
						_npcStatus.Remove(_interactingCharacters.Item1);
					}
				}
				if (_interactingCharacters.Item2 >= 0 && _interactingCharacters.Item2 != taiwuCharId && _interactingCharacters.Item1 != _interactingCharacters.Item2)
				{
					if (_interactingCharacters.Item2 == charIds.Second)
					{
						CompareCharacterStatusRecord(_interactingCharacters.Item2);
					}
					else if (_npcStatus.ContainsKey(_interactingCharacters.Item2))
					{
						_npcStatus.Remove(_interactingCharacters.Item2);
					}
				}
			}
			else
			{
				_npcStatus.Clear();
			}
			_adventureId = curAdventureId;
			EventLogEnqueue();
		}
		else
		{
			_resultCache = new List<EventLogResultData>();
		}
		if (merchantTemplateId >= 0)
		{
			type = 1;
			rightActor = new EventActorData(merchantTemplateId);
		}
		_resultCache.Add(new EventLogResultData
		{
			Type = type,
			ValueList = new List<int> { 2, charIds.First, charIds.Second },
			Text = dialog,
			LeftActorData = leftActor,
			RightActorData = rightActor,
			LeftName = leftName,
			RightName = rightName
		});
		_rawResponseData = rawResponseData;
		_interactingCharacters = (charIds.First, charIds.Second);
		_isSequential = false;
		_isCheckValid = true;
		UpdateCharacterStatusRecord(taiwuCharId);
		if (charIds.First >= 0 && charIds.First != taiwuCharId)
		{
			UpdateCharacterStatusRecord(charIds.First);
		}
		if (charIds.Second >= 0 && charIds.Second != taiwuCharId)
		{
			UpdateCharacterStatusRecord(charIds.Second);
		}
	}

	public void CheckTaiwuStatusImmediately()
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (_shouldCheckStatusImmediately)
		{
			CompareCharacterStatusRecord(taiwuCharId);
		}
		_shouldCheckStatusImmediately = true;
		UpdateCharacterStatusRecord(taiwuCharId);
	}

	public void BlockEventLogStatusCheck()
	{
		_isCheckValid = false;
	}

	public void BlockEventLogImmediateStatusCheck()
	{
		_shouldCheckStatusImmediately = false;
	}

	public void SetIsSequential(bool value)
	{
		_isSequential = value;
	}

	public void RecordCharacterEnterCombat()
	{
		_taiwuStatus.Combat = (8, -1);
		SetIsSequential(value: true);
		Events.RegisterHandler_CombatSettlement(CombatSettlementEventCallback);
	}

	public void RecordCharacterEnterLifeCombat()
	{
		_taiwuStatus.Combat = (9, -1);
		SetIsSequential(value: true);
	}

	public void RecordCharacterEnterCricketCombat()
	{
		_taiwuStatus.Combat = (10, -1);
		SetIsSequential(value: true);
	}

	public void RecordCombatResult(bool isTaiwuWin)
	{
		_taiwuStatus.Combat = (_taiwuStatus.Combat.Item1, isTaiwuWin ? 1 : 0);
	}

	public void UpdateEventLogCharacterDisplayData(int charId)
	{
		if (_characterCache.ContainsKey(charId))
		{
			_characterCache[charId] = DomainManager.Character.GetCharacterDisplayData(charId);
		}
	}

	private void CombatSettlementEventCallback(DataContext context, sbyte combatStatus)
	{
		switch (combatStatus)
		{
		case 2:
		case 4:
			RecordCombatResult(isTaiwuWin: false);
			break;
		case 3:
		case 5:
			RecordCombatResult(isTaiwuWin: true);
			break;
		}
		Events.UnRegisterHandler_CombatSettlement(CombatSettlementEventCallback);
	}

	public void RecordCharacterRelationChanged(bool isRemove, int id1, int id2, ushort type)
	{
		_taiwuStatus.Relation.Add((isRemove, id1, id2, type));
	}

	public void RecordTeammateStateChanged(bool isLosing, int id)
	{
		_taiwuStatus.Teammate = (isLosing, id);
	}

	public void RecordFavorabilityToTaiwuChanged(int id, short value)
	{
		if (_npcStatus.TryGetValue(id, out var value2))
		{
			value2.FavorabilityToTaiwu = value;
		}
	}

	private void GenerateResponseLog(string optionKey, int charId)
	{
		if (string.IsNullOrEmpty(optionKey) || string.IsNullOrEmpty(_rawResponseData))
		{
			return;
		}
		if (charId < 0)
		{
			charId = DomainManager.Taiwu.GetTaiwuCharId();
		}
		string[] array = _rawResponseData.Split("<$new response dialog>");
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string[] array3 = text.Split("<$optionKey>");
				if (array3[0] == optionKey)
				{
					_resultCache.Insert(1, new EventLogResultData
					{
						Type = 2,
						ValueList = new List<int>
						{
							1,
							charId,
							Convert.ToInt32(array3[2])
						},
						Text = array3[1]
					});
					break;
				}
			}
		}
	}

	private void EventLogDequeue()
	{
		List<EventLogResultData> list = _eventLogQueue.Dequeue();
		foreach (EventLogResultData item in list)
		{
			for (int i = 1; i < 1 + item.ValueList[0]; i++)
			{
				int num = item.ValueList[i];
				if (num >= 0)
				{
					_characterReferences[num]--;
					if (_characterReferences[num] <= 0)
					{
						_characterReferences.Remove(num);
						_characterCache.Remove(num);
					}
				}
			}
			switch (item.Type)
			{
			case 29:
			{
				List<int> valueList = item.ValueList;
				int key2 = valueList[valueList.Count - 1];
				_secretInformationReferences[key2]--;
				if (_secretInformationReferences[key2] <= 0)
				{
					_secretInformationReferences.Remove(key2);
					_secretInformationCache.Remove(key2);
				}
				break;
			}
			case 11:
			{
				int key3 = item.ValueList[2];
				_itemReferences[key3]--;
				if (_itemReferences[key3] <= 0)
				{
					_itemReferences.Remove(key3);
					_itemCache.Remove(key3);
				}
				break;
			}
			case 21:
			{
				int key = item.ValueList[2];
				_combatSkillReferences[key]--;
				if (_combatSkillReferences[key] <= 0)
				{
					_combatSkillReferences.Remove(key);
					_combatSkillCache.Remove(key);
				}
				break;
			}
			}
		}
	}

	private void EventLogEnqueue()
	{
		if (_resultCache.Count == 0)
		{
			return;
		}
		foreach (EventLogResultData item in _resultCache)
		{
			switch (item.Type)
			{
			case 29:
			{
				List<int> valueList = item.ValueList;
				int num = valueList[valueList.Count - 1];
				HashSet<int> hashSet = new HashSet<int>();
				SecretInformationDisplayData secretInformationDisplayData = DomainManager.Information.GetSecretInformationDisplayData(num, hashSet);
				item.ValueList.InsertRange(2, hashSet);
				item.ValueList[0] = item.ValueList.Count - 2;
				if (!_secretInformationReferences.ContainsKey(num))
				{
					_secretInformationReferences[num] = 0;
					_secretInformationCache[num] = secretInformationDisplayData;
				}
				_secretInformationReferences[num]++;
				break;
			}
			case 11:
			{
				int key2 = item.ValueList[2];
				if (!_itemReferences.ContainsKey(key2))
				{
					ItemKey itemKey = _itemKeys[key2].Item1;
					bool flag = false;
					if (!DomainManager.Item.ItemExists(itemKey))
					{
						itemKey = DomainManager.Item.CreateItem(DomainManager.TaiwuEvent.MainThreadDataContext, itemKey.ItemType, itemKey.TemplateId);
						flag = true;
					}
					_itemReferences[key2] = 0;
					_itemCache[key2] = DomainManager.Item.GetItemDisplayData(itemKey, item.ValueList[1]);
					if (_itemKeys[key2].Item2 || flag)
					{
						DomainManager.Item.RemoveItem(MainThreadDataContext, itemKey);
					}
					_itemKeys.Remove(key2);
				}
				_itemReferences[key2]++;
				break;
			}
			case 21:
			{
				int key = item.ValueList[2];
				if (!_combatSkillReferences.ContainsKey(key))
				{
					_combatSkillReferences[key] = 0;
					_combatSkillCache[key] = DomainManager.CombatSkill.GetCombatSkillDisplayDataOnce(item.ValueList[1], (short)item.ValueList[2]);
				}
				_combatSkillReferences[key]++;
				break;
			}
			}
			for (int i = 1; i < 1 + item.ValueList[0]; i++)
			{
				int num2 = item.ValueList[i];
				if (num2 >= 0)
				{
					if (!_characterReferences.ContainsKey(num2))
					{
						_characterReferences[num2] = 0;
						_characterCache[num2] = DomainManager.Character.GetCharacterDisplayData(num2);
					}
					_characterReferences[num2]++;
				}
			}
		}
		_resultCache.Add(new EventLogResultData
		{
			Type = 30,
			ValueList = new List<int> { 0 }
		});
		_eventLogQueue.Enqueue(_resultCache);
		_resultCache = new List<EventLogResultData>();
	}

	private bool IsResultCacheValid()
	{
		foreach (EventLogResultData item in _resultCache)
		{
			if (item.Type == 2)
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateCharacterStatusRecord(int id)
	{
		DomainManager.Character.TryGetElement_Objects(id, out var element);
		EventLogCharacterData eventLogCharacterData;
		if (DomainManager.Taiwu.GetTaiwuCharId() == id)
		{
			eventLogCharacterData = _taiwuStatus;
			eventLogCharacterData.SecretInformation.Clear();
			if (DomainManager.Information.TryGetElement_CharacterSecretInformation(id, out var value))
			{
				foreach (SecretInformationCharacterData value3 in value.Collection.Values)
				{
					eventLogCharacterData.SecretInformation.Add(value3.SecretInformationMetaDataId);
				}
			}
			eventLogCharacterData.NormalInformation.Clear();
			if (DomainManager.Information.TryGetElement_Information(id, out var value2))
			{
				foreach (NormalInformation item in value2.GetList())
				{
					eventLogCharacterData.NormalInformation.Add(item);
				}
			}
			eventLogCharacterData.Combat = (-1, -1);
			eventLogCharacterData.Relation.Clear();
			eventLogCharacterData.SpiritualDebt = DomainManager.Extra.GetAreaSpiritualDebt(GetTaiwuLocation().AreaId);
			eventLogCharacterData.Teammate = (false, -1);
			eventLogCharacterData.Profession.Clear();
			foreach (ProfessionItem item2 in (IEnumerable<ProfessionItem>)Profession.Instance)
			{
				eventLogCharacterData.Profession[item2.TemplateId] = DomainManager.Extra.GetProfessionData(item2.TemplateId).Seniority;
			}
		}
		else
		{
			if (!_npcStatus.ContainsKey(id))
			{
				_npcStatus.Add(id, new EventLogCharacterData(isTaiwu: false));
			}
			eventLogCharacterData = _npcStatus[id];
			eventLogCharacterData.FavorabilityToTaiwu = 0;
			eventLogCharacterData.ApprovedTaiwu = ((DomainManager.Organization.TryGetSettlementCharacter(id, out var settlementChar) && DomainManager.Organization.GetSettlementByOrgTemplateId(settlementChar.GetOrgTemplateId()) != null) ? DomainManager.Organization.GetSettlementByOrgTemplateId(settlementChar.GetOrgTemplateId()).CalcApprovingRate() : 0);
		}
		eventLogCharacterData.Happiness = element.GetHappiness();
		eventLogCharacterData.Fame = element.GetFame();
		eventLogCharacterData.Infection = element.GetXiangshuInfection();
		eventLogCharacterData.InfectionStatus = XiangshuInfectionTypeHelper.GetInfectionFeatureIdThatShouldBe(eventLogCharacterData.Infection);
		eventLogCharacterData.Item.Clear();
		foreach (KeyValuePair<ItemKey, int> item3 in element.GetInventory().Items)
		{
			eventLogCharacterData.Item[item3.Key] = item3.Value;
		}
		for (sbyte b = 0; b < 8; b++)
		{
			eventLogCharacterData.Resource[b] = element.GetResource(b);
		}
		eventLogCharacterData.Exp = element.GetExp();
		eventLogCharacterData.Health = element.GetHealth();
		eventLogCharacterData.MainAttribute = element.GetCurrMainAttributes();
		eventLogCharacterData.Injury = element.GetInjuries();
		eventLogCharacterData.Poison = element.GetPoisoned();
		eventLogCharacterData.DisorderOfQi = element.GetDisorderOfQi();
		eventLogCharacterData.CombatSkills.Clear();
		eventLogCharacterData.CombatSkills.AddRange(element.GetLearnedCombatSkills());
		eventLogCharacterData.LifeSkills.Clear();
		eventLogCharacterData.LifeSkills.AddRange(element.GetLearnedLifeSkills());
		eventLogCharacterData.Feature.Clear();
		eventLogCharacterData.Feature.AddRange(element.GetFeatureIds());
	}

	private void CompareCharacterStatusRecord(int id)
	{
		DomainManager.Character.TryGetElement_Objects(id, out var element);
		if (DomainManager.Taiwu.GetTaiwuCharId() == id)
		{
			EventLogCheckCombatResult(_taiwuStatus, element);
			EventLogCheckHappiness(_taiwuStatus, element);
			EventLogCheckFame(_taiwuStatus, element);
			EventLogCheckInfection(_taiwuStatus, element);
			EventLogCheckItem(_taiwuStatus, element);
			EventLogCheckResource(_taiwuStatus, element);
			EventLogCheckExp(_taiwuStatus, element);
			EventLogCheckHealth(_taiwuStatus, element);
			EventLogCheckMainAttribute(_taiwuStatus, element);
			EventLogCheckInjury(_taiwuStatus, element);
			EventLogCheckPoison(_taiwuStatus, element);
			EventLogCheckDisorderOfQi(_taiwuStatus, element);
			EventLogCheckCombatSkills(_taiwuStatus, element);
			EventLogCheckLifeSkills(_taiwuStatus, element);
			EventLogCheckRelation(_taiwuStatus, element);
			EventLogCheckFeature(_taiwuStatus, element);
			EventLogCheckSecretInformation(_taiwuStatus, element);
			EventLogCheckNormalInformation(_taiwuStatus, element);
			EventLogCheckSpiritualDebt(_taiwuStatus, element);
			EventLogCheckTeammate(_taiwuStatus, element);
			EventLogCheckProfession(_taiwuStatus, element);
		}
		else
		{
			_npcStatus.TryGetValue(id, out var value);
			EventLogCheckHappiness(value, element);
			EventLogCheckFame(value, element);
			EventLogCheckInfection(value, element);
			EventLogCheckExp(value, element);
			EventLogCheckHealth(value, element);
			EventLogCheckMainAttribute(value, element);
			EventLogCheckInjury(value, element);
			EventLogCheckPoison(value, element);
			EventLogCheckDisorderOfQi(value, element);
			EventLogCheckCombatSkills(value, element);
			EventLogCheckLifeSkills(value, element);
			EventLogCheckFeature(value, element);
			EventLogCheckFavorabilityToTaiwu(value, element);
			EventLogCheckApprovedTaiwu(value, element);
		}
	}

	private void AddDifferenceToResultCache<T>(sbyte type, int charId, bool isLosing, ICollection<T> origList, ICollection<T> newList, bool elementIsCharacter = false, int? extraAdding = null) where T : struct
	{
		foreach (T @new in newList)
		{
			if (origList.Contains(@new))
			{
				continue;
			}
			List<EventLogResultData> resultCache = _resultCache;
			EventLogResultData eventLogResultData = new EventLogResultData();
			eventLogResultData.Type = type;
			eventLogResultData.IsLosing = isLosing;
			EventLogResultData eventLogResultData2 = eventLogResultData;
			List<int> list = new List<int>();
			list.Add((!elementIsCharacter) ? 1 : 2);
			list.Add(charId);
			List<int> list2 = list;
			if (1 == 0)
			{
			}
			int item2;
			if (!(@new is sbyte b))
			{
				if (!(@new is byte b2))
				{
					if (!(@new is short num))
					{
						if (!(@new is ushort num2))
						{
							if (!(@new is int num3))
							{
								if (!(@new is uint num4))
								{
									if (!(@new is long num5))
									{
										if (!(@new is ulong num6))
										{
											if (!(@new is GameData.Domains.Character.LifeSkillItem lifeSkillItem))
											{
												if (!(@new is NormalInformation item))
												{
													throw new Exception("Fatal error. Event Log Result Value List doesn't support this type yet.");
												}
												item2 = AddNormalInformationToValueList(item, out extraAdding);
											}
											else
											{
												item2 = lifeSkillItem.SkillTemplateId;
											}
										}
										else
										{
											item2 = (int)num6;
										}
									}
									else
									{
										item2 = (int)num5;
									}
								}
								else
								{
									item2 = (int)num4;
								}
							}
							else
							{
								item2 = num3;
							}
						}
						else
						{
							item2 = num2;
						}
					}
					else
					{
						item2 = num;
					}
				}
				else
				{
					item2 = b2;
				}
			}
			else
			{
				item2 = b;
			}
			if (1 == 0)
			{
			}
			list2.Add(item2);
			eventLogResultData2.ValueList = list;
			resultCache.Add(eventLogResultData);
			if (extraAdding.HasValue)
			{
				List<EventLogResultData> resultCache2 = _resultCache;
				resultCache2[resultCache2.Count - 1].ValueList.Add(extraAdding.Value);
			}
		}
	}

	private void AddDifferenceToResultCache(sbyte type, int charId, int origValue, int newValue, int? extraAdding = null)
	{
		if (origValue != newValue)
		{
			_resultCache.Add(new EventLogResultData
			{
				Type = type,
				ValueList = new List<int> { 1, charId, origValue, newValue }
			});
			if (extraAdding.HasValue)
			{
				List<EventLogResultData> resultCache = _resultCache;
				resultCache[resultCache.Count - 1].ValueList.Add(extraAdding.Value);
			}
		}
	}

	private void AddDifferenceToResultCache(int charId, bool isLosing, Dictionary<ItemKey, int> origInventory, Dictionary<ItemKey, int> newInventory)
	{
		foreach (KeyValuePair<ItemKey, int> item2 in newInventory)
		{
			item2.Deconstruct(out var key, out var value);
			ItemKey itemKey = key;
			int num = value;
			int value2;
			int num2 = (origInventory.TryGetValue(itemKey, out value2) ? (num - value2) : num);
			if (num2 > 0)
			{
				ItemKey itemKey2 = itemKey;
				bool item = false;
				if (itemKey2.Id == -1 || !DomainManager.Item.ItemExists(itemKey2))
				{
					itemKey2 = DomainManager.Item.CreateItem(DomainManager.TaiwuEvent.MainThreadDataContext, itemKey.ItemType, itemKey.TemplateId);
					item = true;
				}
				_itemKeys[itemKey2.Id] = (itemKey2, item);
				_resultCache.Add(new EventLogResultData
				{
					Type = 11,
					IsLosing = isLosing,
					ValueList = new List<int> { 1, charId, itemKey2.Id, num2 }
				});
			}
		}
	}

	private int AddNormalInformationToValueList(NormalInformation item, out int? level)
	{
		level = item.Level;
		return item.TemplateId;
	}

	private Location GetTaiwuLocation()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		return location.IsValid() ? location : taiwu.GetValidLocation();
	}

	private void EventLogCheckHappiness(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		AddDifferenceToResultCache(3, character.GetId(), status.Happiness, character.GetHappiness());
	}

	private void EventLogCheckFame(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		AddDifferenceToResultCache(4, character.GetId(), status.Fame, character.GetFame());
	}

	private void EventLogCheckInfection(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		byte xiangshuInfection = character.GetXiangshuInfection();
		AddDifferenceToResultCache(6, character.GetId(), status.Infection, xiangshuInfection);
		AddDifferenceToResultCache(7, character.GetId(), status.InfectionStatus, XiangshuInfectionTypeHelper.GetInfectionFeatureIdThatShouldBe(xiangshuInfection));
	}

	private void EventLogCheckItem(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		Dictionary<ItemKey, int> items = character.GetInventory().Items;
		int id = character.GetId();
		AddDifferenceToResultCache(id, isLosing: false, status.Item, items);
		AddDifferenceToResultCache(id, isLosing: true, items, status.Item);
	}

	private void EventLogCheckResource(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		for (sbyte b = 0; b < 8; b++)
		{
			AddDifferenceToResultCache(12, character.GetId(), status.Resource[b], character.GetResource(b), b);
		}
	}

	private void EventLogCheckExp(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		AddDifferenceToResultCache(27, character.GetId(), status.Exp, character.GetExp());
	}

	private void EventLogCheckHealth(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		AddDifferenceToResultCache(15, character.GetId(), status.Health, character.GetHealth());
	}

	private unsafe void EventLogCheckMainAttribute(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		MainAttributes mainAttributes = character.GetCurrMainAttributes().Subtract(status.MainAttribute);
		if (mainAttributes.GetSum() == 0)
		{
			return;
		}
		for (sbyte b = 0; b < 6; b++)
		{
			if (mainAttributes.Items[b] != 0)
			{
				_resultCache.Add(new EventLogResultData
				{
					Type = 16,
					ValueList = new List<int>
					{
						1,
						character.GetId(),
						status.MainAttribute.Items[b],
						mainAttributes.Items[b],
						b
					}
				});
			}
		}
	}

	private void EventLogCheckInjury(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		Injuries injuries = character.GetInjuries().Subtract(status.Injury);
		if (injuries.GetSum() == 0)
		{
			return;
		}
		for (sbyte b = 0; b < 7; b++)
		{
			sbyte b2 = injuries.Get(b, isInnerInjury: true);
			if (b2 != 0)
			{
				_resultCache.Add(new EventLogResultData
				{
					Type = 17,
					ValueList = new List<int>
					{
						1,
						character.GetId(),
						status.Injury.Get(b, isInnerInjury: true),
						b2,
						b
					}
				});
			}
			sbyte b3 = injuries.Get(b, isInnerInjury: false);
			if (b3 != 0)
			{
				_resultCache.Add(new EventLogResultData
				{
					Type = 18,
					ValueList = new List<int>
					{
						1,
						character.GetId(),
						status.Injury.Get(b, isInnerInjury: false),
						b3,
						b
					}
				});
			}
		}
	}

	private unsafe void EventLogCheckPoison(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		PoisonInts poisonInts = character.GetPoisoned().Subtract(ref status.Poison);
		if (poisonInts.Sum() == 0)
		{
			return;
		}
		for (sbyte b = 0; b < 6; b++)
		{
			if (poisonInts.Items[b] != 0)
			{
				_resultCache.Add(new EventLogResultData
				{
					Type = 19,
					ValueList = new List<int>
					{
						1,
						character.GetId(),
						status.Poison.Items[b],
						poisonInts.Items[b],
						b
					}
				});
			}
		}
	}

	private void EventLogCheckDisorderOfQi(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		AddDifferenceToResultCache(20, character.GetId(), status.DisorderOfQi, character.GetDisorderOfQi());
	}

	private void EventLogCheckCombatSkills(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		AddDifferenceToResultCache(21, character.GetId(), isLosing: false, status.CombatSkills, character.GetLearnedCombatSkills());
	}

	private void EventLogCheckLifeSkills(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		AddDifferenceToResultCache(22, character.GetId(), isLosing: false, status.LifeSkills, character.GetLearnedLifeSkills());
	}

	private void EventLogCheckFeature(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		List<short> featureIds = character.GetFeatureIds();
		AddDifferenceToResultCache(25, character.GetId(), isLosing: false, status.Feature, featureIds);
		AddDifferenceToResultCache(25, character.GetId(), isLosing: true, featureIds, status.Feature);
	}

	private void EventLogCheckSecretInformation(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		if (DomainManager.Information.TryGetElement_CharacterSecretInformation(character.GetId(), out var value))
		{
			AddDifferenceToResultCache(29, character.GetId(), isLosing: false, status.SecretInformation, value.Collection.Values.Select((SecretInformationCharacterData info) => info.SecretInformationMetaDataId).ToList());
		}
	}

	private void EventLogCheckNormalInformation(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		if (DomainManager.Information.TryGetElement_Information(character.GetId(), out var value))
		{
			AddDifferenceToResultCache(28, character.GetId(), isLosing: false, status.NormalInformation, value.GetList().ToList());
		}
	}

	private void EventLogCheckCombatResult(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		if (status.Combat.Item1 != -1 && status.Combat.Item2 >= 0)
		{
			_resultCache.Add(new EventLogResultData
			{
				Type = status.Combat.Item1,
				ValueList = new List<int>
				{
					0,
					status.Combat.Item2
				}
			});
		}
	}

	private void EventLogCheckRelation(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		foreach (var item2 in status.Relation)
		{
			int item = 0;
			if (RelationType.IsOneWayRelation(item2.Item4))
			{
				if (DomainManager.Character.HasRelation(item2.Item2, item2.Item3, item2.Item4) && !DomainManager.Character.HasRelation(item2.Item3, item2.Item2, item2.Item4))
				{
					item = 1;
				}
				else if (!DomainManager.Character.HasRelation(item2.Item2, item2.Item3, item2.Item4) && DomainManager.Character.HasRelation(item2.Item3, item2.Item2, item2.Item4))
				{
					item = 2;
				}
			}
			_resultCache.Add(new EventLogResultData
			{
				Type = 24,
				IsLosing = item2.Item1,
				ValueList = new List<int>
				{
					2,
					item2.Item2,
					item2.Item3,
					RelationType.GetTypeId(item2.Item4),
					item
				}
			});
		}
	}

	private void EventLogCheckSpiritualDebt(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		short areaId = GetTaiwuLocation().AreaId;
		AddDifferenceToResultCache(13, character.GetId(), status.SpiritualDebt, DomainManager.Extra.GetAreaSpiritualDebt(areaId), areaId);
	}

	private void EventLogCheckTeammate(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		if (status.Teammate.Item2 >= 0)
		{
			_resultCache.Add(new EventLogResultData
			{
				Type = 14,
				IsLosing = status.Teammate.Item1,
				ValueList = new List<int>
				{
					2,
					character.GetId(),
					status.Teammate.Item2
				}
			});
		}
	}

	private void EventLogCheckProfession(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		foreach (ProfessionItem item in (IEnumerable<ProfessionItem>)Profession.Instance)
		{
			int templateId = item.TemplateId;
			if (status.Profession.ContainsKey(templateId))
			{
				AddDifferenceToResultCache(26, character.GetId(), status.Profession[templateId], DomainManager.Extra.GetProfessionData(templateId).Seniority, templateId);
			}
		}
	}

	private void EventLogCheckFavorabilityToTaiwu(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		if (status.FavorabilityToTaiwu != 0)
		{
			_resultCache.Add(new EventLogResultData
			{
				Type = 5,
				ValueList = new List<int>
				{
					1,
					character.GetId(),
					status.FavorabilityToTaiwu
				}
			});
		}
	}

	private void EventLogCheckApprovedTaiwu(EventLogCharacterData status, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		if (DomainManager.Organization.TryGetSettlementCharacter(id, out var settlementChar))
		{
			Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(settlementChar.GetOrgTemplateId());
			if (settlementByOrgTemplateId != null)
			{
				AddDifferenceToResultCache(23, id, status.ApprovedTaiwu, settlementByOrgTemplateId.CalcApprovingRate(), settlementChar.GetOrgTemplateId());
			}
		}
	}

	[DomainMethod]
	public void GmCmd_SaveMonthlyActionManager(DataContext context)
	{
		SetMonthlyEventActionManager(_monthlyEventActionManager, context);
	}

	[DomainMethod]
	public void GmCmd_TaiwuCrossArchive()
	{
		DomainManager.TaiwuEvent.OnEvent_TaiwuCrossArchive();
	}

	[DomainMethod]
	public void GmCmd_TaiwuWantedSectPunished(DataContext context, sbyte orgTemplateId, sbyte severity)
	{
		EventArgBox eventArgBox = new EventArgBox();
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId);
		if (settlementByOrgTemplateId != null && settlementByOrgTemplateId is Sect sect)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			sect.AddBounty(context, taiwu, severity, 0);
			eventArgBox.Set("SettlementId", settlementByOrgTemplateId.GetId());
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.TaiwuWantedSectPunished(eventArgBox, confess: false);
		}
	}

	[DomainMethod]
	public List<short> GetValidInteractionEventOptions(int targetCharId)
	{
		List<short> list = new List<short>();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(targetCharId);
		foreach (InteractionEventOptionItem item in (IEnumerable<InteractionEventOptionItem>)InteractionEventOption.Instance)
		{
			if (CheckInteractionEventOption(item, element_Objects))
			{
				list.Add(item.TemplateId);
			}
		}
		return list;
	}

	public bool CheckInteractionEventOption(InteractionEventOptionItem optionCfg, GameData.Domains.Character.Character targetChar)
	{
		int id = targetChar.GetId();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (optionCfg.ProfessionSkill >= 0)
		{
			ProfessionSkillItem professionSkillItem = ProfessionSkill.Instance[optionCfg.ProfessionSkill];
			int skillIndex = ProfessionSkillHandle.GetSkillIndex(professionSkillItem);
			if (!DomainManager.Extra.CanExecuteProfessionSkill(professionSkillItem.Profession, skillIndex))
			{
				return false;
			}
		}
		if (DomainManager.Extra.GetActionPointCurrMonth() < optionCfg.ActionPointCost)
		{
			return false;
		}
		short settlementId = targetChar.GetOrganizationInfo().SettlementId;
		if (settlementId >= 0)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			Location location = settlement.GetLocation();
			if (DomainManager.Extra.GetAreaSpiritualDebt(location.AreaId) < optionCfg.SpiritualDebtCost)
			{
				return false;
			}
		}
		ResourceInts needResources = optionCfg.ResourceCost;
		if (taiwu.GetResources().CheckIsMeet(ref needResources))
		{
			return false;
		}
		MainAttributes needMainAttributes = optionCfg.MainAttributeCost;
		if (taiwu.GetCurrMainAttributes().CheckIsMeet(ref needMainAttributes))
		{
			return false;
		}
		return true;
	}

	public void SetInteractionEventOptionCooldown(int targetCharId, short optionTemplateId)
	{
		_executedOncePerMonthOptions.Add(new IntPair(targetCharId, optionTemplateId));
	}

	public bool IsInteractionEventOptionOffCooldown(int targetCharId, short optionTemplateId)
	{
		return !_executedOncePerMonthOptions.Contains(new IntPair(targetCharId, optionTemplateId));
	}

	[DomainMethod]
	public void InitConchShipEvents()
	{
		string path = Path.Combine("..", "Event/EventLib/GlobalScriptCompiled");
		_scriptRuntime.LoadGlobalScripts(path);
		_packagesList = new List<EventPackage>();
		EventPackagePathInfo pathInfo = new EventPackagePathInfo("../Event");
		string path2 = Path.Combine("..", "use_load_from.txt");
		string path3 = Path.Combine("..", "use_load_file.txt");
		for (int i = 0; i < _managerArray.Length; i++)
		{
			_managerArray[i]?.Reset();
		}
		_loadMethod = (File.Exists(path2) ? EEventPackageLoadMethod.LoadFrom : ((!File.Exists(path3)) ? EEventPackageLoadMethod.LoadBuffer : EEventPackageLoadMethod.LoadFile));
		string[] files = Directory.GetFiles(pathInfo.DllDirPath, "*.dll", SearchOption.AllDirectories);
		foreach (string path4 in files)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path4);
			LoadEventPackageFromAssembly(fileNameWithoutExtension, pathInfo, "ConchShip");
		}
		AdaptableLog.Info("ConchShip events init complete,can load mod events now");
		DlcManager.LoadAllEventPackages();
		DomainManager.Mod.LoadAllEventPackages();
		InitCharacterInteractionEventOptionConfigList();
	}

	[DomainMethod]
	public void LoadEventsFromPath(string eventDataDirectory)
	{
		if (!Directory.Exists(eventDataDirectory))
		{
			throw new Exception("Directory " + eventDataDirectory + " does not exist");
		}
		EventPackagePathInfo pathInfo = new EventPackagePathInfo(eventDataDirectory);
		string[] files = Directory.GetFiles(eventDataDirectory, "*.dll", SearchOption.AllDirectories);
		string fullName = new DirectoryInfo(eventDataDirectory).Parent.FullName;
		int num = 0;
		foreach (string path in files)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			LoadEventPackageFromAssembly(fileNameWithoutExtension, pathInfo, fullName);
			num++;
		}
		AdaptableLog.Info($"load events from {eventDataDirectory} complete,{num} packages loaded!");
	}

	public void LoadEventPackageFromAssembly(string packageName, EventPackagePathInfo pathInfo, string modIdString, string dllFilePath = null)
	{
		if (string.IsNullOrEmpty(dllFilePath))
		{
			dllFilePath = Path.Combine(pathInfo.DllDirPath, packageName + ".dll");
		}
		Assembly assembly = LoadEventPackageAssembly(dllFilePath);
		EventPackage eventPackage = CreateEventPackageObject(assembly);
		if (eventPackage == null)
		{
			Logger.AppendWarning("Failed to load event package at " + dllFilePath + ".");
			return;
		}
		if (_packagesList.Contains(eventPackage))
		{
			for (int i = 0; i < _managerArray.Length; i++)
			{
				_managerArray[i]?.UnloadPackage(eventPackage);
			}
		}
		eventPackage.SetModIdString(modIdString);
		string text = Path.Combine(pathInfo.ScriptDirPath, packageName + ".twes");
		try
		{
			_scriptRuntime.LoadPackageScripts(eventPackage, text);
		}
		catch (Exception value)
		{
			Logger.AppendWarning($"Failed to load event package at {text}.\n{value}");
			return;
		}
		string text2 = "CN";
		string languageFilePath = Path.Combine(pathInfo.LanguageDirPath, packageName + "_Language_" + text2 + ".txt");
		eventPackage.InitLanguage(languageFilePath);
		for (int j = 0; j < _managerArray.Length; j++)
		{
			_managerArray[j]?.HandleEventPackage(eventPackage);
		}
		_packagesList.Add(eventPackage);
	}

	private Assembly LoadEventPackageAssembly(string path)
	{
		EEventPackageLoadMethod loadMethod = _loadMethod;
		if (1 == 0)
		{
		}
		Assembly result = loadMethod switch
		{
			EEventPackageLoadMethod.LoadFile => Assembly.LoadFile(path), 
			EEventPackageLoadMethod.LoadFrom => Assembly.LoadFrom(path), 
			EEventPackageLoadMethod.LoadBuffer => LoadBinaryWithPdb(), 
			_ => throw new Exception($"Unable to load assembly with undefined method {_loadMethod}."), 
		};
		if (1 == 0)
		{
		}
		return result;
		Assembly LoadBinaryWithPdb()
		{
			DirectoryInfo parent = Directory.GetParent(path);
			string path2 = Path.Combine(parent.FullName, Path.GetFileNameWithoutExtension(path) + ".pdb");
			if (File.Exists(path2))
			{
				return Assembly.Load(File.ReadAllBytes(path), File.ReadAllBytes(path2));
			}
			return Assembly.Load(File.ReadAllBytes(path));
		}
	}

	private EventPackage CreateEventPackageObject(Assembly assembly)
	{
		Type[] exportedTypes = assembly.GetExportedTypes();
		Type typeFromHandle = typeof(EventPackage);
		Type[] array = exportedTypes;
		foreach (Type type in array)
		{
			if (type.IsSubclassOf(typeFromHandle))
			{
				try
				{
					return Activator.CreateInstance(type) as EventPackage;
				}
				catch (Exception value)
				{
					Logger.AppendWarning($"Failed to load event package {value}.");
					return null;
				}
			}
		}
		return null;
	}

	public List<TaiwuEventItem> GetAllEventConfigs()
	{
		List<TaiwuEventItem> allEventConfigList = new List<TaiwuEventItem>();
		_packagesList.ForEach(delegate(EventPackage e)
		{
			allEventConfigList.AddRange(e.GetAllEvents());
		});
		return allEventConfigList;
	}

	private void InitCharacterInteractionEventOptionConfigList()
	{
		_characterInteractionEventOptionList.Clear();
		List<short> allKeys = InteractionEventOption.Instance.GetAllKeys();
		foreach (EventPackage packages in _packagesList)
		{
			List<TaiwuEventItem> allEvents = packages.GetAllEvents();
			if (allKeys.Count == 0)
			{
				break;
			}
			foreach (TaiwuEventItem item in allEvents)
			{
				if (allKeys.Count == 0)
				{
					break;
				}
				TaiwuEventOption[] eventOptions = item.EventOptions;
				foreach (TaiwuEventOption taiwuEventOption in eventOptions)
				{
					if (allKeys.Count == 0)
					{
						break;
					}
					int num = allKeys.FindIndex(delegate(short key)
					{
						InteractionEventOptionItem interactionEventOptionItem = InteractionEventOption.Instance[key];
						return interactionEventOptionItem.OptionGuid == taiwuEventOption.OptionGuid;
					});
					if (num >= 0)
					{
						_characterInteractionEventOptionList.Add((taiwuEventOption, allKeys[num], item));
						allKeys.RemoveAt(num);
					}
				}
			}
		}
	}

	public (Dictionary<short, bool> dict, int NoInteractionReason) GetVisibleCharacterInteractionEventOptions(int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		string characterClickedSpecialNextEvent = GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetCharacterClickedSpecialNextEvent(element_Objects, new EventArgBox(), createRelation: false);
		if (!string.IsNullOrEmpty(characterClickedSpecialNextEvent))
		{
			return (dict: null, NoInteractionReason: 2);
		}
		if (!DomainManager.Character.TryGetRelation(charId, DomainManager.Taiwu.GetTaiwuCharId(), out var _))
		{
			return (dict: null, NoInteractionReason: 1);
		}
		Dictionary<short, bool> dictionary = new Dictionary<short, bool>();
		EventArgBox eventArgBox = _argBoxPool.Get();
		eventArgBox.Set("RoleTaiwu", EventArgBox.TaiwuCharacterId);
		eventArgBox.Set("CharacterId", charId);
		foreach (var characterInteractionEventOption in _characterInteractionEventOptionList)
		{
			TaiwuEventOption item = characterInteractionEventOption.TaiwuEventOption;
			short item2 = characterInteractionEventOption.templateId;
			TaiwuEventItem item3 = characterInteractionEventOption.TaiwuEventItem;
			InteractionEventOptionItem interactionEventOptionItem = InteractionEventOption.Instance[item2];
			if (interactionEventOptionItem.InteractionType != EInteractionEventOptionInteractionType.Invalid)
			{
				EventArgBox argBox = item3.ArgBox;
				EventArgBox argBox2 = item.ArgBox;
				item3.ArgBox = eventArgBox;
				item.ArgBox = eventArgBox;
				if (item.IsVisible)
				{
					dictionary.Add(item2, item.IsAvailable);
				}
				item3.ArgBox = argBox;
				item.ArgBox = argBox2;
			}
		}
		_argBoxPool.Return(eventArgBox);
		return (dict: dictionary, NoInteractionReason: 0);
	}

	public EventArgBox GetGlobalEventArgumentBox()
	{
		return GetGlobalArgBox();
	}

	public void ClearGlobalEventArgumentBox()
	{
		_globalArgBox.Clear();
		SetGlobalArgBox(_globalArgBox, MainThreadDataContext);
	}

	public void SaveGlobalEventArgumentBox()
	{
		SetGlobalArgBox(_globalArgBox, MainThreadDataContext);
	}

	public void SaveArgToGlobalArgBox<T>(string key, T value)
	{
		_globalArgBox.GenericSet(key, value);
		SetGlobalArgBox(_globalArgBox, MainThreadDataContext);
	}

	public void ActivateNextSwordTomb()
	{
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.SetNextSwordTombCountDownDate();
	}

	public void XiangshuMinionSurroundTaiwuVillage()
	{
		if (!GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GlobalArgBoxContainsKey<bool>("TrySurroundTaiwuVillage"))
		{
			return;
		}
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.MakeAreaGraduallyBrokenInCondition(EventArgBox.TaiwuVillageAreaId, delegate(MapBlockData blockData)
		{
			if (blockData.CharacterSet == null)
			{
				return false;
			}
			if (blockData.CharacterSet.Count > 0)
			{
				int num5 = Math.Min(3, blockData.CharacterSet.Count);
				List<int> list2 = blockData.CharacterSet.ToList();
				CollectionUtils.Shuffle(MainThreadDataContext.Random, list2);
				for (int i = 0; i < num5; i++)
				{
					if (DomainManager.Character.TryGetElement_Objects(list2[i], out var element2))
					{
						DomainManager.Character.MakeCharacterDead(MainThreadDataContext, element2, 10);
					}
				}
			}
			HashSet<int> characterSet = blockData.CharacterSet;
			return characterSet != null && characterSet.Count > 0;
		}, new Dictionary<short, byte>());
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		List<int> list = new List<int>();
		for (short num = 0; num < 45; num++)
		{
			list.Clear();
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(num);
			Span<MapBlockData> span = areaBlocks;
			for (int num2 = 0; num2 < span.Length; num2++)
			{
				MapBlockData mapBlockData = span[num2];
				if (mapBlockData.CharacterSet != null)
				{
					list.AddRange(mapBlockData.CharacterSet);
				}
			}
			if (list.Count > 0)
			{
				int num3 = (int)Math.Max(1f, (float)list.Count * 0.2f);
				CollectionUtils.Shuffle(MainThreadDataContext.Random, list);
				for (int num4 = 0; num4 < num3; num4++)
				{
					if (DomainManager.Character.TryGetElement_Objects(list[num4], out var element) && element.GetCreatingType() != 0)
					{
						DomainManager.Character.MakeCharacterDead(MainThreadDataContext, element, 10);
					}
				}
				Location location = new Location(num, -1);
				monthlyNotificationCollection.AddXiangshuKilling(location, num3);
			}
		}
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.EnsureTaiwuVillagerLocationForSpiritualWanderPlace();
	}

	[DomainMethod]
	public void SetIsQuickStartGame(bool flag)
	{
		EventArgBox globalArgBox = DomainManager.TaiwuEvent.GetGlobalArgBox();
		globalArgBox.Set("CS_PK_IsQuickStartGame", flag);
		DomainManager.TaiwuEvent.SaveGlobalEventArgumentBox();
	}

	public void CollectUnreleasedCalledCharacters(HashSet<int> calledCharacters)
	{
		_monthlyEventActionManager.CollectUnreleasedCalledCharacters(calledCharacters);
	}

	[DomainMethod]
	public (int, int) GetMonthlyActionStateAndTime(MonthlyActionKey key)
	{
		MonthlyActionBase monthlyAction = _monthlyEventActionManager.GetMonthlyAction(key);
		if (monthlyAction == null)
		{
			return (0, 0);
		}
		return (monthlyAction.State, monthlyAction.Month);
	}

	public void ResetAllConfigMonthlyActions()
	{
		_monthlyEventActionManager.Init();
	}

	public MonthlyActionBase GetMonthlyAction(MonthlyActionKey key)
	{
		return _monthlyEventActionManager.GetMonthlyAction(key);
	}

	public MonthlyActionKey AddTempDynamicAction<T>(DataContext context, T action) where T : MonthlyActionBase, IDynamicAction
	{
		MonthlyActionKey result = _monthlyEventActionManager.AddTempDynamicAction(action);
		action.TriggerAction();
		SetMonthlyEventActionManager(_monthlyEventActionManager, context);
		return result;
	}

	public MonthlyActionKey AddWrappedConfigAction(DataContext context, short templateId, short assignedAreaId = -1)
	{
		MonthlyActionKey result = _monthlyEventActionManager.AddWrappedConfigAction(templateId, assignedAreaId);
		SetMonthlyEventActionManager(_monthlyEventActionManager, context);
		return result;
	}

	public void RemoveTempDynamicAction(DataContext context, MonthlyActionKey key)
	{
		_monthlyEventActionManager.RemoveTempDynamicAction(key);
		SetMonthlyEventActionManager(_monthlyEventActionManager, context);
	}

	public void ClearTaiwuBindingMonthlyActions(DataContext context)
	{
		_monthlyEventActionManager.ClearTaiwuBindingMonthlyActions();
		SetMonthlyEventActionManager(_monthlyEventActionManager, context);
	}

	public void HandleMonthlyAction(DataContext context, MonthlyActionKey key)
	{
		MonthlyActionBase monthlyAction = _monthlyEventActionManager.GetMonthlyAction(key);
		monthlyAction.MonthlyHandler();
		SetMonthlyEventActionManager(_monthlyEventActionManager, context);
	}

	public void HandleMonthlyActions()
	{
		_monthlyEventActionManager.HandleMonthlyActions();
		SetMonthlyEventActionManager(_monthlyEventActionManager, MainThreadDataContext);
		if (GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GlobalArgBoxContainsKey<int>("ConchShip_PresetKey_DateOfNextSwordTombActivate"))
		{
			int intFromGlobalArgBox = GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetIntFromGlobalArgBox("ConchShip_PresetKey_DateOfNextSwordTombActivate");
			if (DomainManager.World.GetCurrDate() >= intFromGlobalArgBox)
			{
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.RemoveFromGlobalArgBox<int>("ConchShip_PresetKey_DateOfNextSwordTombActivate");
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.StartNextSwordTombCountDown();
			}
		}
	}

	public bool WasTemporaryOptionSelected(string guid)
	{
		return _selectedTemporaryOptions.Contains(guid);
	}

	public void SetTemporaryOptionSelected(string guid, bool selected)
	{
		if (selected)
		{
			_selectedTemporaryOptions.Add(guid);
		}
		else
		{
			_selectedTemporaryOptions.Remove(guid);
		}
	}

	[DomainMethod]
	public void EventScriptExecuteNext()
	{
		_scriptRuntime.MovingNext = true;
	}

	[DomainMethod]
	public void SetEventScriptExecutionPause(bool isPaused)
	{
		_scriptRuntime.IsPaused = isPaused;
	}

	[DomainMethod]
	public void OnCharacterClicked(DataContext context, int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		short templateId = element_Objects.GetTemplateId();
		if (!GameData.Domains.Character.Character.IsXiangshuMinion(templateId))
		{
			if (XiangshuAvatarIds.JuniorXiangshuTemplateIds.Contains(templateId))
			{
				sbyte xiangshuAvatarIdByCharacterTemplateId = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(templateId);
				OnEvent_PurpleBambooAvatarClicked(charId, xiangshuAvatarIdByCharacterTemplateId);
			}
			else if (element_Objects.GetCreatingType() == 0)
			{
				OnEvent_FixedCharacterClicked(charId, templateId);
			}
			else if (element_Objects.GetCreatingType() != 3 || DomainManager.Extra.IsSpecialGroupMember(element_Objects))
			{
				OnEvent_CharacterClicked(charId);
			}
			else if (element_Objects.GetCreatingType() == 3 && SectMainStoryRelatedConstants.SectMainStoryCharacterTemplateIds.Contains(templateId))
			{
				OnEvent_FixedEnemyClicked(charId, templateId);
			}
		}
	}

	[DomainMethod]
	public void OnCharacterTemplateClicked(DataContext context, short characterTemplateId)
	{
		OnEvent_CharacterTemplateClicked(characterTemplateId);
	}

	[DomainMethod]
	public void OnLetTeammateLeaveGroup(DataContext context, int charId)
	{
		OnEvent_LetTeammateLeaveGroup(charId);
	}

	[DomainMethod]
	public void OnInteractCaravan(int caravanId)
	{
		OnEvent_CaravanClicked(caravanId);
	}

	[DomainMethod]
	public void OnInteractKidnappedCharacter(int charId)
	{
		OnEvent_KidnappedCharacterClicked(charId);
	}

	[DomainMethod]
	public void OnSectBuildingClicked(short buildingTemplateId)
	{
		OnEvent_SectBuildingClicked(buildingTemplateId);
	}

	[DomainMethod]
	public void OnRecordEnterGame(short mainStoryLineProgress)
	{
		if (!DomainManager.TutorialChapter.InGuiding)
		{
			OnEvent_RecordEnterGame(mainStoryLineProgress);
		}
	}

	[DomainMethod]
	public void OnNewGameMonth()
	{
		OnEvent_NewGameMonth();
		DomainManager.Building.TriggerBuildingCompleteEvents(MainThreadDataContext);
	}

	[DomainMethod]
	public void OnCombatWithXiangshuMinionComplete(short templateId)
	{
		OnEvent_CombatWithXiangshuMinionComplete(templateId);
	}

	[DomainMethod]
	public void OnBlackMaskAnimationComplete(bool maskVisible)
	{
		OnEvent_BlackMaskAnimationComplete(maskVisible);
	}

	[DomainMethod]
	public void OnMakingSystemOpened(BuildingBlockKey blockKey, short templateId)
	{
		OnEvent_MakingSystemOpened(blockKey, templateId);
	}

	[DomainMethod]
	public void OnCollectedMakingSystemItem(BuildingBlockKey blockKey, short templateId, bool showingGetItem)
	{
		OnEvent_CollectedMakingSystemItem(blockKey, templateId, showingGetItem);
	}

	[DomainMethod]
	public void OnSectSpecialBuildingClicked(short templateId)
	{
		OnEvent_OnSectSpecialBuildingClicked(templateId);
	}

	[DomainMethod]
	public void AnimalAvatarClicked(int animalId)
	{
		OnEvent_AnimalAvatarClicked(animalId);
	}

	[DomainMethod]
	public void MainStoryFinishCatchCricket(bool result)
	{
		OnEvent_MainStoryFinishCatchCricket(result);
	}

	[DomainMethod]
	public void NpcTombClicked(int tombId)
	{
		OnEvent_NpcTombClicked(tombId);
	}

	[DomainMethod]
	public void OnLifeSkillCombatForceSilent(int charId, sbyte concessionCount, sbyte inducementCount)
	{
		OnEvent_LifeSkillCombatForceSilent(charId, concessionCount, inducementCount);
	}

	[DomainMethod]
	public void TryMoveWhenMoveDisable()
	{
		OnEvent_TryMoveWhenMoveDisabled();
	}

	[DomainMethod]
	public void TryMoveToInvalidLocationInTutorial()
	{
		OnEvent_TryMoveToInvalidLocationInTutorial();
	}

	[DomainMethod]
	public void CloseUI(string uiName, bool presetBool = false, int presetInt = -1)
	{
		DomainManager.TaiwuEvent.OnEvent_CloseUI(uiName, presetBool, presetInt);
	}

	[DomainMethod]
	public void TaiwuCollectWudangHeavenlyTreeSeed(sbyte resourceType)
	{
		DomainManager.TaiwuEvent.OnEvent_TaiwuCollectWudangHeavenlyTreeSeed(resourceType);
	}

	[DomainMethod]
	public void TaiwuVillagerExpelled(int charId)
	{
		DomainManager.TaiwuEvent.OnEvent_TaiwuVillagerExpelled(charId);
	}

	[DomainMethod]
	public void TaiwuCrossArchiveFindMemory(sbyte type)
	{
		DomainManager.TaiwuEvent.OnEvent_TaiwuCrossArchiveFindMemory(type);
	}

	[DomainMethod]
	public void UserLoadDreamBackArchive()
	{
		DomainManager.TaiwuEvent.OnEvent_UserLoadDreamBackArchive();
		OnEvent_RecordEnterGame(DomainManager.World.GetMainStoryLineProgress());
	}

	[DomainMethod]
	public void OperateInventoryItem(int charId, sbyte operationType, ItemDisplayData itemData)
	{
		DomainManager.TaiwuEvent.OnEvent_OperateInventoryItem(charId, operationType, itemData);
	}

	[DomainMethod]
	public void SettlementTreasuryBuildingClicked(short templateId, byte currStatus, sbyte currPage)
	{
		DomainManager.TaiwuEvent.OnEvent_OnSettlementTreasuryBuildingClicked(templateId, currStatus, currPage);
	}

	[DomainMethod]
	public void TriggerShixiangDrumEasterEgg()
	{
		DomainManager.TaiwuEvent.OnEvent_OnShixiangDrumClickedManyTimes();
	}

	[DomainMethod]
	public void OnClickedPrisonBtn(short buildingTemplateId)
	{
		DomainManager.TaiwuEvent.OnEvent_OnClickedPrisonBtn(buildingTemplateId);
	}

	[DomainMethod]
	public void OnClickedSendPrisonBtn()
	{
		DomainManager.TaiwuEvent.OnEvent_OnClickedSendPrisonBtn();
	}

	[DomainMethod]
	public void InteractPrisoner(int characterId, int interactPrisonerType)
	{
		DomainManager.TaiwuEvent.OnEvent_InteractPrisoner(characterId, interactPrisonerType);
	}

	[DomainMethod]
	public void OnClickMapPickupEvent(Location location)
	{
		DomainManager.TaiwuEvent.OnEvent_TriggerMapPickupEvent(location, arg1: true);
	}

	[DomainMethod]
	public void OnClickMapPickupNormalEvent(Location location)
	{
		if (!DomainManager.Map.TempDisableTriggerNormalPickupByTaiwuEscape)
		{
			DomainManager.TaiwuEvent.OnEvent_TriggerMapPickupEvent(location, arg1: false);
		}
	}

	[DomainMethod]
	public void OnClickDeportButton(int type, bool isGood)
	{
		DomainManager.TaiwuEvent.OnEvent_TaiwuDeportVitals(type, isGood);
	}

	[DomainMethod]
	public void OnSwitchToGuardedPage(byte currStatus, sbyte currPage)
	{
		OnEvent_SwitchToGuardedPage(currStatus, currPage);
	}

	public void AddJieqingMaskCharId(int charId)
	{
		if (_jieqingMaskCharIdList == null)
		{
			_jieqingMaskCharIdList = new List<int>();
		}
		if (!_jieqingMaskCharIdList.Contains(charId))
		{
			_jieqingMaskCharIdList.Add(charId);
			SetJieqingMaskCharIdList(_jieqingMaskCharIdList, MainThreadDataContext);
			if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
			{
				element.SetAvatar(element.GetAvatar(), MainThreadDataContext);
			}
		}
	}

	public void RemoveJieqingMaskCharId(int charId)
	{
		if (_jieqingMaskCharIdList != null && _jieqingMaskCharIdList.Contains(charId))
		{
			_jieqingMaskCharIdList.Remove(charId);
			SetJieqingMaskCharIdList(_jieqingMaskCharIdList, MainThreadDataContext);
			if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
			{
				element.SetAvatar(element.GetAvatar(), MainThreadDataContext);
			}
		}
	}

	[DomainMethod]
	public void GmCmd_AddJieqingMaskCharId(int charId)
	{
		AddJieqingMaskCharId(charId);
	}

	[DomainMethod]
	public void GmCmd_RemoveJieqingMaskCharId(int charId)
	{
		RemoveJieqingMaskCharId(charId);
	}

	public TaiwuEventDomain()
		: base(29)
	{
		_globalArgBox = new EventArgBox();
		_monthlyEventActionManager = new MonthlyEventActionsManager();
		_awayForeverLoverCharId = 0;
		_eventCount = 0;
		_healDoctorCharId = 0;
		_cgName = string.Empty;
		_notifyData = new EventNotifyData();
		_hasListeningEvent = false;
		_selectInformationData = new EventSelectInformationData();
		_taiwuLocationChangeFlag = false;
		_secretVillageOnFire = false;
		_taiwuVillageShowShrine = false;
		_hideAllTeammates = false;
		_leftRoleAlternativeName = string.Empty;
		_rightRoleAlternativeName = string.Empty;
		_rightRoleXiangshuDisplayData = new sbyte[2];
		_selectCombatSkillData = new EventSelectCombatSkillData();
		_selectLifeSkillData = new EventSelectLifeSkillData();
		_itemListOfLeft = new ItemDisplayData[3];
		_itemListOfRight = new ItemDisplayData[3];
		_showItemWithCricketBattleGuess = false;
		_displayingEventData = new TaiwuEventDisplayData();
		_tempCreateItemList = new List<ItemKey>();
		_coverCricketJarGradeListForRight = new List<sbyte>();
		_marriageLook1CharIdList = new List<int>();
		_marriageLook2CharIdList = new List<int>();
		_allCombatGroupChars = new int[3];
		_cricketBettingData = new EventCricketBettingData();
		_jieqingMaskCharIdList = new List<int>();
		OnInitializedDomainData();
	}

	private EventArgBox GetGlobalArgBox()
	{
		return _globalArgBox;
	}

	private unsafe void SetGlobalArgBox(EventArgBox value, DataContext context)
	{
		_globalArgBox = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		int serializedSize = _globalArgBox.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValue_Set(12, 0, serializedSize);
		ptr += _globalArgBox.Serialize(ptr);
	}

	private MonthlyEventActionsManager GetMonthlyEventActionManager()
	{
		return _monthlyEventActionManager;
	}

	private unsafe void SetMonthlyEventActionManager(MonthlyEventActionsManager value, DataContext context)
	{
		_monthlyEventActionManager = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		int serializedSize = _monthlyEventActionManager.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValue_Set(12, 1, serializedSize);
		ptr += _monthlyEventActionManager.Serialize(ptr);
	}

	[Obsolete("DomainData _awayForeverLoverCharId is no longer in use.")]
	public int GetAwayForeverLoverCharId()
	{
		return _awayForeverLoverCharId;
	}

	[Obsolete("DomainData _awayForeverLoverCharId is no longer in use.")]
	public unsafe void SetAwayForeverLoverCharId(int value, DataContext context)
	{
		_awayForeverLoverCharId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(12, 2, 4);
		*(int*)ptr = _awayForeverLoverCharId;
		ptr += 4;
	}

	[Obsolete("DomainData _eventCount is no longer in use.")]
	public ushort GetEventCount()
	{
		return _eventCount;
	}

	[Obsolete("DomainData _eventCount is no longer in use.")]
	private void SetEventCount(ushort value, DataContext context)
	{
		_eventCount = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
	}

	[Obsolete("DomainData _healDoctorCharId is no longer in use.")]
	public int GetHealDoctorCharId()
	{
		return _healDoctorCharId;
	}

	[Obsolete("DomainData _healDoctorCharId is no longer in use.")]
	public void SetHealDoctorCharId(int value, DataContext context)
	{
		_healDoctorCharId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
	}

	public string GetCgName()
	{
		return _cgName;
	}

	public void SetCgName(string value, DataContext context)
	{
		_cgName = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
	}

	public EventNotifyData GetNotifyData()
	{
		return _notifyData;
	}

	private void SetNotifyData(EventNotifyData value, DataContext context)
	{
		_notifyData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
	}

	public bool GetHasListeningEvent()
	{
		return _hasListeningEvent;
	}

	private void SetHasListeningEvent(bool value, DataContext context)
	{
		_hasListeningEvent = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
	}

	public EventSelectInformationData GetSelectInformationData()
	{
		return _selectInformationData;
	}

	public void SetSelectInformationData(EventSelectInformationData value, DataContext context)
	{
		_selectInformationData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
	}

	public bool GetTaiwuLocationChangeFlag()
	{
		return _taiwuLocationChangeFlag;
	}

	public void SetTaiwuLocationChangeFlag(bool value, DataContext context)
	{
		_taiwuLocationChangeFlag = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
	}

	public bool GetSecretVillageOnFire()
	{
		return _secretVillageOnFire;
	}

	public unsafe void SetSecretVillageOnFire(bool value, DataContext context)
	{
		_secretVillageOnFire = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(12, 10, 1);
		*ptr = (_secretVillageOnFire ? ((byte)1) : ((byte)0));
		ptr++;
	}

	public bool GetTaiwuVillageShowShrine()
	{
		return _taiwuVillageShowShrine;
	}

	public unsafe void SetTaiwuVillageShowShrine(bool value, DataContext context)
	{
		_taiwuVillageShowShrine = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(12, 11, 1);
		*ptr = (_taiwuVillageShowShrine ? ((byte)1) : ((byte)0));
		ptr++;
	}

	public bool GetHideAllTeammates()
	{
		return _hideAllTeammates;
	}

	public void SetHideAllTeammates(bool value, DataContext context)
	{
		_hideAllTeammates = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
	}

	public string GetLeftRoleAlternativeName()
	{
		return _leftRoleAlternativeName;
	}

	public void SetLeftRoleAlternativeName(string value, DataContext context)
	{
		_leftRoleAlternativeName = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
	}

	public string GetRightRoleAlternativeName()
	{
		return _rightRoleAlternativeName;
	}

	public void SetRightRoleAlternativeName(string value, DataContext context)
	{
		_rightRoleAlternativeName = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
	}

	public sbyte[] GetRightRoleXiangshuDisplayData()
	{
		return _rightRoleXiangshuDisplayData;
	}

	public void SetRightRoleXiangshuDisplayData(sbyte[] value, DataContext context)
	{
		_rightRoleXiangshuDisplayData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
	}

	public EventSelectCombatSkillData GetSelectCombatSkillData()
	{
		return _selectCombatSkillData;
	}

	public void SetSelectCombatSkillData(EventSelectCombatSkillData value, DataContext context)
	{
		_selectCombatSkillData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
	}

	public EventSelectLifeSkillData GetSelectLifeSkillData()
	{
		return _selectLifeSkillData;
	}

	public void SetSelectLifeSkillData(EventSelectLifeSkillData value, DataContext context)
	{
		_selectLifeSkillData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, DataStates, CacheInfluences, context);
	}

	public ItemDisplayData[] GetItemListOfLeft()
	{
		return _itemListOfLeft;
	}

	public void SetItemListOfLeft(ItemDisplayData[] value, DataContext context)
	{
		_itemListOfLeft = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
	}

	public ItemDisplayData[] GetItemListOfRight()
	{
		return _itemListOfRight;
	}

	public void SetItemListOfRight(ItemDisplayData[] value, DataContext context)
	{
		_itemListOfRight = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
	}

	public bool GetShowItemWithCricketBattleGuess()
	{
		return _showItemWithCricketBattleGuess;
	}

	public void SetShowItemWithCricketBattleGuess(bool value, DataContext context)
	{
		_showItemWithCricketBattleGuess = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
	}

	public TaiwuEventDisplayData GetDisplayingEventData()
	{
		return _displayingEventData;
	}

	public void SetDisplayingEventData(TaiwuEventDisplayData value, DataContext context)
	{
		_displayingEventData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
	}

	public List<ItemKey> GetTempCreateItemList()
	{
		return _tempCreateItemList;
	}

	public void SetTempCreateItemList(List<ItemKey> value, DataContext context)
	{
		_tempCreateItemList = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
	}

	public List<sbyte> GetCoverCricketJarGradeListForRight()
	{
		return _coverCricketJarGradeListForRight;
	}

	public void SetCoverCricketJarGradeListForRight(List<sbyte> value, DataContext context)
	{
		_coverCricketJarGradeListForRight = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, DataStates, CacheInfluences, context);
	}

	public List<int> GetMarriageLook1CharIdList()
	{
		return _marriageLook1CharIdList;
	}

	public void SetMarriageLook1CharIdList(List<int> value, DataContext context)
	{
		_marriageLook1CharIdList = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, DataStates, CacheInfluences, context);
	}

	public List<int> GetMarriageLook2CharIdList()
	{
		return _marriageLook2CharIdList;
	}

	public void SetMarriageLook2CharIdList(List<int> value, DataContext context)
	{
		_marriageLook2CharIdList = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, DataStates, CacheInfluences, context);
	}

	public int[] GetAllCombatGroupChars()
	{
		return _allCombatGroupChars;
	}

	public void SetAllCombatGroupChars(int[] value, DataContext context)
	{
		_allCombatGroupChars = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, DataStates, CacheInfluences, context);
	}

	public EventCricketBettingData GetCricketBettingData()
	{
		return _cricketBettingData;
	}

	public void SetCricketBettingData(EventCricketBettingData value, DataContext context)
	{
		_cricketBettingData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, DataStates, CacheInfluences, context);
	}

	public List<int> GetJieqingMaskCharIdList()
	{
		return _jieqingMaskCharIdList;
	}

	public void SetJieqingMaskCharIdList(List<int> value, DataContext context)
	{
		_jieqingMaskCharIdList = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, DataStates, CacheInfluences, context);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		int serializedSize = _globalArgBox.GetSerializedSize();
		byte* ptr = OperationAdder.DynamicSingleValue_Set(12, 0, serializedSize);
		ptr += _globalArgBox.Serialize(ptr);
		int serializedSize2 = _monthlyEventActionManager.GetSerializedSize();
		byte* ptr2 = OperationAdder.DynamicSingleValue_Set(12, 1, serializedSize2);
		ptr2 += _monthlyEventActionManager.Serialize(ptr2);
		byte* ptr3 = OperationAdder.FixedSingleValue_Set(12, 2, 4);
		*(int*)ptr3 = _awayForeverLoverCharId;
		ptr3 += 4;
		byte* ptr4 = OperationAdder.FixedSingleValue_Set(12, 10, 1);
		*ptr4 = (_secretVillageOnFire ? ((byte)1) : ((byte)0));
		ptr4++;
		byte* ptr5 = OperationAdder.FixedSingleValue_Set(12, 11, 1);
		*ptr5 = (_taiwuVillageShowShrine ? ((byte)1) : ((byte)0));
		ptr5++;
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(12, 0));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(12, 1));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(12, 2));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(12, 10));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(12, 11));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 1:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 2:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			return GameData.Serializer.Serializer.Serialize(_awayForeverLoverCharId, dataPool);
		case 3:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
			}
			return GameData.Serializer.Serializer.Serialize(_eventCount, dataPool);
		case 4:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
			}
			return GameData.Serializer.Serializer.Serialize(_healDoctorCharId, dataPool);
		case 5:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
			}
			return GameData.Serializer.Serializer.Serialize(_cgName, dataPool);
		case 6:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
			}
			return GameData.Serializer.Serializer.Serialize(_notifyData, dataPool);
		case 7:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
			}
			return GameData.Serializer.Serializer.Serialize(_hasListeningEvent, dataPool);
		case 8:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
			}
			return GameData.Serializer.Serializer.Serialize(_selectInformationData, dataPool);
		case 9:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuLocationChangeFlag, dataPool);
		case 10:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 10);
			}
			return GameData.Serializer.Serializer.Serialize(_secretVillageOnFire, dataPool);
		case 11:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuVillageShowShrine, dataPool);
		case 12:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 12);
			}
			return GameData.Serializer.Serializer.Serialize(_hideAllTeammates, dataPool);
		case 13:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 13);
			}
			return GameData.Serializer.Serializer.Serialize(_leftRoleAlternativeName, dataPool);
		case 14:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 14);
			}
			return GameData.Serializer.Serializer.Serialize(_rightRoleAlternativeName, dataPool);
		case 15:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
			}
			return GameData.Serializer.Serializer.Serialize(_rightRoleXiangshuDisplayData, dataPool);
		case 16:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 16);
			}
			return GameData.Serializer.Serializer.Serialize(_selectCombatSkillData, dataPool);
		case 17:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 17);
			}
			return GameData.Serializer.Serializer.Serialize(_selectLifeSkillData, dataPool);
		case 18:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 18);
			}
			return GameData.Serializer.Serializer.Serialize(_itemListOfLeft, dataPool);
		case 19:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 19);
			}
			return GameData.Serializer.Serializer.Serialize(_itemListOfRight, dataPool);
		case 20:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 20);
			}
			return GameData.Serializer.Serializer.Serialize(_showItemWithCricketBattleGuess, dataPool);
		case 21:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
			}
			return GameData.Serializer.Serializer.Serialize(_displayingEventData, dataPool);
		case 22:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
			}
			return GameData.Serializer.Serializer.Serialize(_tempCreateItemList, dataPool);
		case 23:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 23);
			}
			return GameData.Serializer.Serializer.Serialize(_coverCricketJarGradeListForRight, dataPool);
		case 24:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 24);
			}
			return GameData.Serializer.Serializer.Serialize(_marriageLook1CharIdList, dataPool);
		case 25:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 25);
			}
			return GameData.Serializer.Serializer.Serialize(_marriageLook2CharIdList, dataPool);
		case 26:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 26);
			}
			return GameData.Serializer.Serializer.Serialize(_allCombatGroupChars, dataPool);
		case 27:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 27);
			}
			return GameData.Serializer.Serializer.Serialize(_cricketBettingData, dataPool);
		case 28:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 28);
			}
			return GameData.Serializer.Serializer.Serialize(_jieqingMaskCharIdList, dataPool);
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
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _awayForeverLoverCharId);
			SetAwayForeverLoverCharId(_awayForeverLoverCharId, context);
			break;
		case 3:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 4:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _healDoctorCharId);
			SetHealDoctorCharId(_healDoctorCharId, context);
			break;
		case 5:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _cgName);
			SetCgName(_cgName, context);
			break;
		case 6:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 7:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 8:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _selectInformationData);
			SetSelectInformationData(_selectInformationData, context);
			break;
		case 9:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _taiwuLocationChangeFlag);
			SetTaiwuLocationChangeFlag(_taiwuLocationChangeFlag, context);
			break;
		case 10:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _secretVillageOnFire);
			SetSecretVillageOnFire(_secretVillageOnFire, context);
			break;
		case 11:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _taiwuVillageShowShrine);
			SetTaiwuVillageShowShrine(_taiwuVillageShowShrine, context);
			break;
		case 12:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _hideAllTeammates);
			SetHideAllTeammates(_hideAllTeammates, context);
			break;
		case 13:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _leftRoleAlternativeName);
			SetLeftRoleAlternativeName(_leftRoleAlternativeName, context);
			break;
		case 14:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _rightRoleAlternativeName);
			SetRightRoleAlternativeName(_rightRoleAlternativeName, context);
			break;
		case 15:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _rightRoleXiangshuDisplayData);
			SetRightRoleXiangshuDisplayData(_rightRoleXiangshuDisplayData, context);
			break;
		case 16:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _selectCombatSkillData);
			SetSelectCombatSkillData(_selectCombatSkillData, context);
			break;
		case 17:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _selectLifeSkillData);
			SetSelectLifeSkillData(_selectLifeSkillData, context);
			break;
		case 18:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _itemListOfLeft);
			SetItemListOfLeft(_itemListOfLeft, context);
			break;
		case 19:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _itemListOfRight);
			SetItemListOfRight(_itemListOfRight, context);
			break;
		case 20:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _showItemWithCricketBattleGuess);
			SetShowItemWithCricketBattleGuess(_showItemWithCricketBattleGuess, context);
			break;
		case 21:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _displayingEventData);
			SetDisplayingEventData(_displayingEventData, context);
			break;
		case 22:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _tempCreateItemList);
			SetTempCreateItemList(_tempCreateItemList, context);
			break;
		case 23:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _coverCricketJarGradeListForRight);
			SetCoverCricketJarGradeListForRight(_coverCricketJarGradeListForRight, context);
			break;
		case 24:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _marriageLook1CharIdList);
			SetMarriageLook1CharIdList(_marriageLook1CharIdList, context);
			break;
		case 25:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _marriageLook2CharIdList);
			SetMarriageLook2CharIdList(_marriageLook2CharIdList, context);
			break;
		case 26:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _allCombatGroupChars);
			SetAllCombatGroupChars(_allCombatGroupChars, context);
			break;
		case 27:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _cricketBettingData);
			SetCricketBettingData(_cricketBettingData, context);
			break;
		case 28:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _jieqingMaskCharIdList);
			SetJieqingMaskCharIdList(_jieqingMaskCharIdList, context);
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
			int argsCount55 = operation.ArgsCount;
			int num55 = argsCount55;
			if (num55 == 1)
			{
				MonthlyActionKey item113 = default(MonthlyActionKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item113);
				(int, int) monthlyActionStateAndTime = GetMonthlyActionStateAndTime(item113);
				return GameData.Serializer.Serializer.Serialize(monthlyActionStateAndTime, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
			if (operation.ArgsCount == 0)
			{
				InitConchShipEvents();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 2:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 2)
			{
				string item7 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				bool item8 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				TriggerListener(item7, item8);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
		{
			int argsCount23 = operation.ArgsCount;
			int num23 = argsCount23;
			if (num23 == 3)
			{
				string item41 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item41);
				ItemKey item42 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item42);
				bool item43 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item43);
				SetItemSelectResult(item41, item42, item43);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 4:
		{
			int argsCount38 = operation.ArgsCount;
			int num38 = argsCount38;
			if (num38 == 3)
			{
				string item70 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item70);
				int item71 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item71);
				bool item72 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item72);
				SetCharacterSelectResult(item70, item71, item72);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
		{
			int argsCount17 = operation.ArgsCount;
			int num17 = argsCount17;
			if (num17 == 2)
			{
				string item29 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item29);
				int item30 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item30);
				SetSecretInformationSelectResult(item29, item30);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 6:
		{
			int argsCount44 = operation.ArgsCount;
			int num44 = argsCount44;
			if (num44 == 2)
			{
				string item79 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item79);
				NormalInformation item80 = default(NormalInformation);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item80);
				SetNormalInformationSelectResult(item79, item80);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 7:
			if (operation.ArgsCount == 0)
			{
				StartHandleEventDuringAdvance();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 8:
			if (operation.ArgsCount == 0)
			{
				List<TaiwuEventSummaryDisplayData> triggeredEventSummaryDisplayData = GetTriggeredEventSummaryDisplayData();
				return GameData.Serializer.Serializer.Serialize(triggeredEventSummaryDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 9:
		{
			int argsCount52 = operation.ArgsCount;
			int num52 = argsCount52;
			if (num52 == 1)
			{
				string item107 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item107);
				SetEventInProcessing(item107);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				string item84 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item84);
				string item85 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item85);
				EventSelect(item84, item85);
				return -1;
			}
			case 3:
			{
				string item81 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item81);
				string item82 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item82);
				bool item83 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item83);
				EventSelect(item81, item82, item83);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 11:
			if (operation.ArgsCount == 0)
			{
				List<TaiwuEventDisplayData> eventDisplayData = GetEventDisplayData();
				return GameData.Serializer.Serializer.Serialize(eventDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 12:
			if (operation.ArgsCount == 0)
			{
				GmCmd_SaveMonthlyActionManager(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 13:
		{
			int argsCount16 = operation.ArgsCount;
			int num16 = argsCount16;
			if (num16 == 1)
			{
				int item28 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item28);
				OnCharacterClicked(context, item28);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 14:
		{
			int argsCount57 = operation.ArgsCount;
			int num57 = argsCount57;
			if (num57 == 1)
			{
				int item117 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item117);
				OnLetTeammateLeaveGroup(context, item117);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 15:
		{
			int argsCount49 = operation.ArgsCount;
			int num49 = argsCount49;
			if (num49 == 1)
			{
				int item102 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item102);
				OnInteractCaravan(item102);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 16:
		{
			int argsCount40 = operation.ArgsCount;
			int num40 = argsCount40;
			if (num40 == 1)
			{
				int item74 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item74);
				OnInteractKidnappedCharacter(item74);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 17:
		{
			int argsCount34 = operation.ArgsCount;
			int num34 = argsCount34;
			if (num34 == 1)
			{
				short item64 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item64);
				OnSectBuildingClicked(item64);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 18:
		{
			int argsCount27 = operation.ArgsCount;
			int num27 = argsCount27;
			if (num27 == 1)
			{
				short item48 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item48);
				OnRecordEnterGame(item48);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 19:
			if (operation.ArgsCount == 0)
			{
				OnNewGameMonth();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 20:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 1)
			{
				short item22 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				OnCombatWithXiangshuMinionComplete(item22);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 21:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 1)
			{
				bool item5 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				OnBlackMaskAnimationComplete(item5);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 22:
		{
			int argsCount53 = operation.ArgsCount;
			int num53 = argsCount53;
			if (num53 == 2)
			{
				BuildingBlockKey item108 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item108);
				short item109 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item109);
				OnMakingSystemOpened(item108, item109);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 23:
		{
			int argsCount47 = operation.ArgsCount;
			int num47 = argsCount47;
			if (num47 == 3)
			{
				BuildingBlockKey item96 = default(BuildingBlockKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item96);
				short item97 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item97);
				bool item98 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item98);
				OnCollectedMakingSystemItem(item96, item97, item98);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 24:
		{
			int argsCount41 = operation.ArgsCount;
			int num41 = argsCount41;
			if (num41 == 1)
			{
				short item75 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item75);
				OnSectSpecialBuildingClicked(item75);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 25:
		{
			int argsCount36 = operation.ArgsCount;
			int num36 = argsCount36;
			if (num36 == 1)
			{
				int item68 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item68);
				AnimalAvatarClicked(item68);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 26:
		{
			int argsCount31 = operation.ArgsCount;
			int num31 = argsCount31;
			if (num31 == 1)
			{
				bool item52 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item52);
				MainStoryFinishCatchCricket(item52);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 27:
		{
			int argsCount28 = operation.ArgsCount;
			int num28 = argsCount28;
			if (num28 == 1)
			{
				string item49 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item49);
				LoadEventsFromPath(item49);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 28:
		{
			int argsCount24 = operation.ArgsCount;
			int num24 = argsCount24;
			if (num24 == 1)
			{
				int item44 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item44);
				NpcTombClicked(item44);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 29:
		{
			int argsCount20 = operation.ArgsCount;
			int num20 = argsCount20;
			if (num20 == 1)
			{
				short item36 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item36);
				SetLifeSkillSelectResult(item36);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 30:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 1)
			{
				short item24 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item24);
				SetCombatSkillSelectResult(item24);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 31:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 3)
			{
				int item14 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				sbyte item15 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				sbyte item16 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item16);
				OnLifeSkillCombatForceSilent(item14, item15, item16);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 32:
			if (operation.ArgsCount == 0)
			{
				TryMoveWhenMoveDisable();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 33:
			if (operation.ArgsCount == 0)
			{
				TryMoveToInvalidLocationInTutorial();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 34:
		{
			int argsCount50 = operation.ArgsCount;
			int num50 = argsCount50;
			if (num50 == 3)
			{
				string item103 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item103);
				string item104 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item104);
				CharacterSet item105 = default(CharacterSet);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item105);
				SetCharacterSetSelectResult(item103, item104, item105);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 35:
		{
			int argsCount46 = operation.ArgsCount;
			int num46 = argsCount46;
			if (num46 == 1)
			{
				short item95 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item95);
				OnCharacterTemplateClicked(context, item95);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 36:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				string item91 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item91);
				CloseUI(item91);
				return -1;
			}
			case 2:
			{
				string item89 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item89);
				bool item90 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item90);
				CloseUI(item89, item90);
				return -1;
			}
			case 3:
			{
				string item86 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item86);
				bool item87 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item87);
				int item88 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item88);
				CloseUI(item86, item87, item88);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 37:
		{
			int argsCount43 = operation.ArgsCount;
			int num43 = argsCount43;
			if (num43 == 1)
			{
				bool item78 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item78);
				SetIsQuickStartGame(item78);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 38:
		{
			int argsCount39 = operation.ArgsCount;
			int num39 = argsCount39;
			if (num39 == 1)
			{
				sbyte item73 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item73);
				TaiwuCollectWudangHeavenlyTreeSeed(item73);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 39:
			if (operation.ArgsCount == 0)
			{
				EventLogData eventLogData = GetEventLogData();
				return GameData.Serializer.Serializer.Serialize(eventLogData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 40:
		{
			int argsCount32 = operation.ArgsCount;
			int num32 = argsCount32;
			if (num32 == 8)
			{
				IntPair item53 = default(IntPair);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item53);
				string item54 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item54);
				string item55 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item55);
				EventActorData item56 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item56);
				EventActorData item57 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item57);
				string item58 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item58);
				string item59 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item59);
				short item60 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item60);
				StartNewDialog(context, item53, item54, item55, item56, item57, item58, item59, item60);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 41:
		{
			int argsCount30 = operation.ArgsCount;
			int num30 = argsCount30;
			if (num30 == 1)
			{
				int item51 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item51);
				TaiwuVillagerExpelled(item51);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 42:
			if (operation.ArgsCount == 0)
			{
				GmCmd_TaiwuCrossArchive();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 43:
		{
			int argsCount25 = operation.ArgsCount;
			int num25 = argsCount25;
			if (num25 == 1)
			{
				sbyte item45 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item45);
				TaiwuCrossArchiveFindMemory(item45);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 44:
			if (operation.ArgsCount == 0)
			{
				UserLoadDreamBackArchive();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 45:
		{
			int argsCount19 = operation.ArgsCount;
			int num19 = argsCount19;
			if (num19 == 3)
			{
				int item33 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item33);
				sbyte item34 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item34);
				ItemDisplayData item35 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item35);
				OperateInventoryItem(item33, item34, item35);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 46:
		{
			int argsCount14 = operation.ArgsCount;
			int num14 = argsCount14;
			if (num14 == 2)
			{
				string item25 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item25);
				int item26 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				SetItemSelectCount(item25, item26);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 47:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 3)
			{
				short item19 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item19);
				byte item20 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				sbyte item21 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item21);
				SettlementTreasuryBuildingClicked(item19, item20, item21);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 48:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 3)
			{
				string item9 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				string item10 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				ISerializableGameData item11 = null;
				argsOffset += GameData.Serializer.Serializer.DeserializeDefault(argDataPool, argsOffset, ref item11);
				SetListenerEventActionISerializableArg(item9, item10, item11);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 49:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 3)
			{
				string item2 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				string item3 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				int item4 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				SetListenerEventActionIntArg(item2, item3, item4);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 50:
		{
			int argsCount56 = operation.ArgsCount;
			int num56 = argsCount56;
			if (num56 == 3)
			{
				string item114 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item114);
				string item115 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item115);
				bool item116 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item116);
				SetListenerEventActionBoolArg(item114, item115, item116);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 51:
		{
			int argsCount54 = operation.ArgsCount;
			int num54 = argsCount54;
			if (num54 == 3)
			{
				string item110 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item110);
				string item111 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item111);
				string item112 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item112);
				SetListenerEventActionStringArg(item110, item111, item112);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 52:
		{
			int argsCount51 = operation.ArgsCount;
			int num51 = argsCount51;
			if (num51 == 1)
			{
				int item106 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item106);
				List<short> validInteractionEventOptions = GetValidInteractionEventOptions(item106);
				return GameData.Serializer.Serializer.Serialize(validInteractionEventOptions, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 53:
		{
			int argsCount48 = operation.ArgsCount;
			int num48 = argsCount48;
			if (num48 == 3)
			{
				string item99 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item99);
				string item100 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item100);
				IntList item101 = default(IntList);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item101);
				SetListenerEventActionIntListArg(item99, item100, item101);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 54:
		{
			int argsCount45 = operation.ArgsCount;
			int num45 = argsCount45;
			if (num45 == 3)
			{
				string item92 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item92);
				string item93 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item93);
				ItemKey item94 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item94);
				SetListenerEventActionItemKeyArg(item92, item93, item94);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 55:
			if (operation.ArgsCount == 0)
			{
				TriggerShixiangDrumEasterEgg();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 56:
		{
			int argsCount42 = operation.ArgsCount;
			int num42 = argsCount42;
			if (num42 == 2)
			{
				int item76 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item76);
				int item77 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item77);
				InteractPrisoner(item76, item77);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 57:
			if (operation.ArgsCount == 0)
			{
				OnClickedSendPrisonBtn();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 58:
		{
			int argsCount37 = operation.ArgsCount;
			int num37 = argsCount37;
			if (num37 == 1)
			{
				short item69 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item69);
				OnClickedPrisonBtn(item69);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 59:
		{
			int argsCount35 = operation.ArgsCount;
			int num35 = argsCount35;
			if (num35 == 3)
			{
				string item65 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item65);
				List<int> item66 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item66);
				bool item67 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item67);
				SetCharacterMultSelectResult(item65, item66, item67);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 60:
		{
			int argsCount33 = operation.ArgsCount;
			int num33 = argsCount33;
			if (num33 == 3)
			{
				bool item61 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item61);
				Wager item62 = default(Wager);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item62);
				int item63 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item63);
				SetCricketBettingResult(item61, item62, item63);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 61:
			if (operation.ArgsCount == 0)
			{
				List<int> implementedFunctionIds = GetImplementedFunctionIds(context);
				return GameData.Serializer.Serializer.Serialize(implementedFunctionIds, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 62:
		{
			int argsCount29 = operation.ArgsCount;
			int num29 = argsCount29;
			if (num29 == 1)
			{
				bool item50 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item50);
				SetEventScriptExecutionPause(item50);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 63:
			if (operation.ArgsCount == 0)
			{
				EventScriptExecuteNext();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 64:
		{
			int argsCount26 = operation.ArgsCount;
			int num26 = argsCount26;
			if (num26 == 2)
			{
				sbyte item46 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item46);
				sbyte item47 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item47);
				GmCmd_TaiwuWantedSectPunished(context, item46, item47);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 65:
			if (operation.ArgsCount == 0)
			{
				EventSelectContinue();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 66:
		{
			int argsCount22 = operation.ArgsCount;
			int num22 = argsCount22;
			if (num22 == 1)
			{
				int item40 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item40);
				SetSelectCount(item40);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 67:
		{
			int argsCount21 = operation.ArgsCount;
			int num21 = argsCount21;
			if (num21 == 3)
			{
				string item37 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item37);
				string item38 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item38);
				GameData.Utilities.ShortList item39 = default(GameData.Utilities.ShortList);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item39);
				SetListenerEventActionShortListArg(item37, item38, item39);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 68:
		{
			int argsCount18 = operation.ArgsCount;
			int num18 = argsCount18;
			if (num18 == 2)
			{
				string item31 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item31);
				GameData.Utilities.ShortList item32 = default(GameData.Utilities.ShortList);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item32);
				SetShowingEventShortListArg(item31, item32);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 69:
		{
			int argsCount15 = operation.ArgsCount;
			int num15 = argsCount15;
			if (num15 == 1)
			{
				Location item27 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item27);
				OnClickMapPickupEvent(item27);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 70:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 1)
			{
				Location item23 = default(Location);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				OnClickMapPickupNormalEvent(item23);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 71:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 2)
			{
				int item17 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				bool item18 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item18);
				OnClickDeportButton(item17, item18);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 72:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 2)
			{
				byte item12 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				sbyte item13 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item13);
				OnSwitchToGuardedPage(item12, item13);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 73:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 1)
			{
				int item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				GmCmd_AddJieqingMaskCharId(item6);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 74:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 1)
			{
				int item = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				GmCmd_RemoveJieqingMaskCharId(item);
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
			return;
		case 1:
			return;
		case 2:
			return;
		case 3:
			return;
		case 4:
			return;
		case 5:
			return;
		case 6:
			return;
		case 7:
			return;
		case 8:
			return;
		case 9:
			return;
		case 10:
			return;
		case 11:
			return;
		case 12:
			return;
		case 13:
			return;
		case 14:
			return;
		case 15:
			return;
		case 16:
			return;
		case 17:
			return;
		case 18:
			return;
		case 19:
			return;
		case 20:
			return;
		case 21:
			return;
		case 22:
			return;
		case 23:
			return;
		case 24:
			return;
		case 25:
			return;
		case 26:
			return;
		case 27:
			return;
		case 28:
			return;
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 2:
			if (!BaseGameDataDomain.IsModified(DataStates, 2))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 2);
			return GameData.Serializer.Serializer.Serialize(_awayForeverLoverCharId, dataPool);
		case 3:
			if (!BaseGameDataDomain.IsModified(DataStates, 3))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 3);
			return GameData.Serializer.Serializer.Serialize(_eventCount, dataPool);
		case 4:
			if (!BaseGameDataDomain.IsModified(DataStates, 4))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 4);
			return GameData.Serializer.Serializer.Serialize(_healDoctorCharId, dataPool);
		case 5:
			if (!BaseGameDataDomain.IsModified(DataStates, 5))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 5);
			return GameData.Serializer.Serializer.Serialize(_cgName, dataPool);
		case 6:
			if (!BaseGameDataDomain.IsModified(DataStates, 6))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 6);
			return GameData.Serializer.Serializer.Serialize(_notifyData, dataPool);
		case 7:
			if (!BaseGameDataDomain.IsModified(DataStates, 7))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 7);
			return GameData.Serializer.Serializer.Serialize(_hasListeningEvent, dataPool);
		case 8:
			if (!BaseGameDataDomain.IsModified(DataStates, 8))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 8);
			return GameData.Serializer.Serializer.Serialize(_selectInformationData, dataPool);
		case 9:
			if (!BaseGameDataDomain.IsModified(DataStates, 9))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 9);
			return GameData.Serializer.Serializer.Serialize(_taiwuLocationChangeFlag, dataPool);
		case 10:
			if (!BaseGameDataDomain.IsModified(DataStates, 10))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 10);
			return GameData.Serializer.Serializer.Serialize(_secretVillageOnFire, dataPool);
		case 11:
			if (!BaseGameDataDomain.IsModified(DataStates, 11))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 11);
			return GameData.Serializer.Serializer.Serialize(_taiwuVillageShowShrine, dataPool);
		case 12:
			if (!BaseGameDataDomain.IsModified(DataStates, 12))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 12);
			return GameData.Serializer.Serializer.Serialize(_hideAllTeammates, dataPool);
		case 13:
			if (!BaseGameDataDomain.IsModified(DataStates, 13))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 13);
			return GameData.Serializer.Serializer.Serialize(_leftRoleAlternativeName, dataPool);
		case 14:
			if (!BaseGameDataDomain.IsModified(DataStates, 14))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 14);
			return GameData.Serializer.Serializer.Serialize(_rightRoleAlternativeName, dataPool);
		case 15:
			if (!BaseGameDataDomain.IsModified(DataStates, 15))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 15);
			return GameData.Serializer.Serializer.Serialize(_rightRoleXiangshuDisplayData, dataPool);
		case 16:
			if (!BaseGameDataDomain.IsModified(DataStates, 16))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 16);
			return GameData.Serializer.Serializer.Serialize(_selectCombatSkillData, dataPool);
		case 17:
			if (!BaseGameDataDomain.IsModified(DataStates, 17))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 17);
			return GameData.Serializer.Serializer.Serialize(_selectLifeSkillData, dataPool);
		case 18:
			if (!BaseGameDataDomain.IsModified(DataStates, 18))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 18);
			return GameData.Serializer.Serializer.Serialize(_itemListOfLeft, dataPool);
		case 19:
			if (!BaseGameDataDomain.IsModified(DataStates, 19))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 19);
			return GameData.Serializer.Serializer.Serialize(_itemListOfRight, dataPool);
		case 20:
			if (!BaseGameDataDomain.IsModified(DataStates, 20))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 20);
			return GameData.Serializer.Serializer.Serialize(_showItemWithCricketBattleGuess, dataPool);
		case 21:
			if (!BaseGameDataDomain.IsModified(DataStates, 21))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 21);
			return GameData.Serializer.Serializer.Serialize(_displayingEventData, dataPool);
		case 22:
			if (!BaseGameDataDomain.IsModified(DataStates, 22))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 22);
			return GameData.Serializer.Serializer.Serialize(_tempCreateItemList, dataPool);
		case 23:
			if (!BaseGameDataDomain.IsModified(DataStates, 23))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 23);
			return GameData.Serializer.Serializer.Serialize(_coverCricketJarGradeListForRight, dataPool);
		case 24:
			if (!BaseGameDataDomain.IsModified(DataStates, 24))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 24);
			return GameData.Serializer.Serializer.Serialize(_marriageLook1CharIdList, dataPool);
		case 25:
			if (!BaseGameDataDomain.IsModified(DataStates, 25))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 25);
			return GameData.Serializer.Serializer.Serialize(_marriageLook2CharIdList, dataPool);
		case 26:
			if (!BaseGameDataDomain.IsModified(DataStates, 26))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 26);
			return GameData.Serializer.Serializer.Serialize(_allCombatGroupChars, dataPool);
		case 27:
			if (!BaseGameDataDomain.IsModified(DataStates, 27))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 27);
			return GameData.Serializer.Serializer.Serialize(_cricketBettingData, dataPool);
		case 28:
			if (!BaseGameDataDomain.IsModified(DataStates, 28))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 28);
			return GameData.Serializer.Serializer.Serialize(_jieqingMaskCharIdList, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
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
			if (BaseGameDataDomain.IsModified(DataStates, 7))
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
			}
			break;
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
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		return dataId switch
		{
			0 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			1 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
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
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
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
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		uint num;
		switch (operation.DataId)
		{
		case 0:
			ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Ref_Single(operation, pResult, _globalArgBox);
			goto IL_0194;
		case 1:
			ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Ref_Single(operation, pResult, _monthlyEventActionManager);
			goto IL_0194;
		case 2:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _awayForeverLoverCharId);
			goto IL_0194;
		case 10:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _secretVillageOnFire);
			goto IL_0194;
		case 11:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _taiwuVillageShowShrine);
			goto IL_0194;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
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
			{
				throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
			}
			IL_0194:
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
					DomainManager.Global.CompleteLoading(12);
				}
			}
			break;
		}
	}

	private void InitializeInternalDataOfCollections()
	{
	}

	private void TaskCheckDaoShiAskForWildFood()
	{
		if (DomainManager.World.GetMainStoryLineProgress() == 3)
		{
			GameData.Domains.TaiwuEvent.EventHelper.EventHelper.SaveGlobalArg("DaoshiAskForWildFood", value: true);
		}
	}

	private bool CanTriggerEventType(EEventType eventType)
	{
		return DomainManager.TutorialChapter.InGuiding == (eventType == EEventType.TutorialEvent);
	}

	public void OnEvent_TaiwuBlockChanged(Location arg0, Location arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuBlockChanged(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_CharacterClicked(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_CharacterClicked(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_AdventureReachStartNode(short arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_AdventureReachStartNode(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_AdventureReachTransferNode(short arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_AdventureReachTransferNode(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_AdventureReachEndNode(short arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_AdventureReachEndNode(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_AdventureEnterNode(AdventureMapPoint arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_AdventureEnterNode(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_EnterTutorialChapter(short arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_EnterTutorialChapter(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_LetTeammateLeaveGroup(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_LetTeammateLeaveGroup(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_NeedToPassLegacy(bool arg0, string arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_NeedToPassLegacy(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_CaravanClicked(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_CaravanClicked(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_KidnappedCharacterClicked(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_KidnappedCharacterClicked(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TeammateMonthAdvance(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TeammateMonthAdvance(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_SameBlockWithTaiwuWhenMonthAdvance(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_SameBlockWithTaiwuWhenMonthAdvance(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_SameBlockWithRandomEnemyOnNewMonth()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_SameBlockWithRandomEnemyOnNewMonth();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_SameBlockWithTaiwuOnNewMonthSpecial(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_SameBlockWithTaiwuOnNewMonthSpecial(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_SectBuildingClicked(short arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_SectBuildingClicked(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_SecretInformationBroadcast(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_SecretInformationBroadcast(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_RecordEnterGame(short arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_RecordEnterGame(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_NewGameMonth()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_NewGameMonth();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_CombatWithXiangshuMinionComplete(short arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_CombatWithXiangshuMinionComplete(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_BlackMaskAnimationComplete(bool arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_BlackMaskAnimationComplete(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_ConstructComplete(BuildingBlockKey arg0, short arg1, sbyte arg2)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_ConstructComplete(arg0, arg1, arg2);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_CombatOpening(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_CombatOpening(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_MakingSystemOpened(BuildingBlockKey arg0, short arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_MakingSystemOpened(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_CollectedMakingSystemItem(BuildingBlockKey arg0, short arg1, bool arg2)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_CollectedMakingSystemItem(arg0, arg1, arg2);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuVillageDestroyed()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuVillageDestroyed();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_OnSectSpecialBuildingClicked(short arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_OnSectSpecialBuildingClicked(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_AnimalAvatarClicked(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_AnimalAvatarClicked(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_MainStoryFinishCatchCricket(bool arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_MainStoryFinishCatchCricket(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_PurpleBambooAvatarClicked(int arg0, sbyte arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_PurpleBambooAvatarClicked(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_UserLoadDreamBackArchive()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_UserLoadDreamBackArchive();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_NpcTombClicked(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_NpcTombClicked(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_LifeSkillCombatForceSilent(int arg0, sbyte arg1, sbyte arg2)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_LifeSkillCombatForceSilent(arg0, arg1, arg2);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TryMoveWhenMoveDisabled()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TryMoveWhenMoveDisabled();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TryMoveToInvalidLocationInTutorial()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TryMoveToInvalidLocationInTutorial();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_ProfessionExperienceChange(int arg0, int arg1, int arg2)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_ProfessionExperienceChange(arg0, arg1, arg2);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_ProfessionSkillClicked(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_ProfessionSkillClicked(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuGotTianjieFulu(int arg0, ItemKey arg1, int arg2)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuGotTianjieFulu(arg0, arg1, arg2);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuSaveCountChange(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuSaveCountChange(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_CharacterTemplateClicked(short arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_CharacterTemplateClicked(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_CloseUI(string arg0, bool arg1, int arg2)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_CloseUI(arg0, arg1, arg2);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuFindMaterial(int arg0, TreasureFindResult arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuFindMaterial(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuFindExtraTreasure(TreasureFindResult arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuFindExtraTreasure(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuCollectWudangHeavenlyTreeSeed(sbyte arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuCollectWudangHeavenlyTreeSeed(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuVillagerExpelled(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuVillagerExpelled(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuCrossArchive()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuCrossArchive();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuCrossArchiveFindMemory(sbyte arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuCrossArchiveFindMemory(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_DlcLoongPutJiaoEggs(int arg0, ItemKey arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_DlcLoongPutJiaoEggs(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_DlcLoongInteractJiao(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_DlcLoongInteractJiao(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_DlcLoongPetJiao(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_DlcLoongPetJiao(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_JingangSectMainStoryReborn()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_JingangSectMainStoryReborn();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_JingangSectMainStoryMonkSoul()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_JingangSectMainStoryMonkSoul();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_OperateInventoryItem(int arg0, sbyte arg1, ItemDisplayData arg2)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_OperateInventoryItem(arg0, arg1, arg2);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_OnSettlementTreasuryBuildingClicked(short arg0, byte arg1, sbyte arg2)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_OnSettlementTreasuryBuildingClicked(arg0, arg1, arg2);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_OnShixiangDrumClickedManyTimes()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_OnShixiangDrumClickedManyTimes();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_OnClickedPrisonBtn(short arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_OnClickedPrisonBtn(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_OnClickedSendPrisonBtn()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_OnClickedSendPrisonBtn();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_InteractPrisoner(int arg0, int arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_InteractPrisoner(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_ClickChicken(int arg0, short arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_ClickChicken(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_SoulWitheringBellTransfer()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_SoulWitheringBellTransfer();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_CatchThief(sbyte arg0, bool arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_CatchThief(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_ConfirmEnterSwordTomb()
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_ConfirmEnterSwordTomb();
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuBeHuntedArrivedSect(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuBeHuntedArrivedSect(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuBeHuntedHunterDie(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuBeHuntedHunterDie(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_StartSectShaolinDemonSlayer(int arg0)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_StartSectShaolinDemonSlayer(arg0);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TriggerMapPickupEvent(Location arg0, bool arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TriggerMapPickupEvent(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_FixedCharacterClicked(int arg0, short arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_FixedCharacterClicked(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_FixedEnemyClicked(int arg0, short arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_FixedEnemyClicked(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_AdventureRemoved(short arg0, Location arg1, bool arg2)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_AdventureRemoved(arg0, arg1, arg2);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_TaiwuDeportVitals(int arg0, bool arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_TaiwuDeportVitals(arg0, arg1);
			}
		}
		TriggerHandled();
	}

	public void OnEvent_SwitchToGuardedPage(byte arg0, sbyte arg1)
	{
		for (int i = 0; i < _managerArray.Length; i++)
		{
			if (CanTriggerEventType((EEventType)i))
			{
				_managerArray[i]?.OnEventTrigger_SwitchToGuardedPage(arg0, arg1);
			}
		}
		TriggerHandled();
	}
}
