using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Config;
using Config.EventConfig;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.DLC;
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

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x02000084 RID: 132
	[GameDataDomain(12)]
	public class TaiwuEventDomain : BaseGameDataDomain
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06001741 RID: 5953 RVA: 0x001579A4 File Offset: 0x00155BA4
		// (set) Token: 0x06001742 RID: 5954 RVA: 0x001579AC File Offset: 0x00155BAC
		public bool ShowInteractCheckAnimation { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x001579B5 File Offset: 0x00155BB5
		// (set) Token: 0x06001744 RID: 5956 RVA: 0x001579BD File Offset: 0x00155BBD
		public EventInteractCheckData InteractCheckData { get; set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06001745 RID: 5957 RVA: 0x001579C6 File Offset: 0x00155BC6
		// (set) Token: 0x06001746 RID: 5958 RVA: 0x001579CE File Offset: 0x00155BCE
		private TaiwuEvent ShowingEvent
		{
			get
			{
				return this._showingEvent;
			}
			set
			{
				this._showingEvent = value;
				Events.RaiseEventWindowFocusStateChanged(this.MainThreadDataContext, !this._showingEvent.IsEmpty);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06001747 RID: 5959 RVA: 0x001579F4 File Offset: 0x00155BF4
		public bool IsShowingEvent
		{
			get
			{
				TaiwuEvent showingEvent = this._showingEvent;
				return showingEvent != null && !showingEvent.IsEmpty;
			}
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x00157A17 File Offset: 0x00155C17
		public override void OnUpdate(DataContext context)
		{
			EventScriptRuntime scriptRuntime = TaiwuEventDomain._scriptRuntime;
			if (scriptRuntime != null)
			{
				scriptRuntime.Update();
			}
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x00157A2C File Offset: 0x00155C2C
		private void OnInitializedDomainData()
		{
			this.MainThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
			this._argBoxPool = new LocalObjectPool<EventArgBox>(5, 65535);
			this.SetHasListeningEvent(false, this.MainThreadDataContext);
			this.SetHealDoctorCharId(-1, this.MainThreadDataContext);
			this.SetAwayForeverLoverCharId(-1, this.MainThreadDataContext);
			sbyte[] array = new sbyte[2];
			array[0] = 9;
			this.SetRightRoleXiangshuDisplayData(array, this.MainThreadDataContext);
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x00157A98 File Offset: 0x00155C98
		private void InitializeOnInitializeGameDataModule()
		{
			TaiwuEventDomain._scriptRuntime = new EventScriptRuntime(this.MainThreadDataContext, true);
			TaiwuEventDomain._managerArray = new EventManagerBase[8];
			TaiwuEventDomain._managerArray[0] = new MainStoryEventManager();
			TaiwuEventDomain._managerArray[5] = new AdventureEventManager();
			TaiwuEventDomain._managerArray[4] = new NpcEventManager();
			TaiwuEventDomain._managerArray[7] = new TutorialEventManager();
			TaiwuEventDomain._managerArray[1] = new GlobalCommonEventManager();
			TaiwuEventDomain._managerArray[6] = new ModEventManager();
			this.InitConchShipEvents();
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x00157B14 File Offset: 0x00155D14
		private void InitializeOnEnterNewWorld()
		{
			this.InitRuntimeEnvironment();
			DataUid dataUid = new DataUid(0, 1, ulong.MaxValue, uint.MaxValue);
			GameDataBridge.AddPostDataModificationHandler(dataUid, "InitializeMonthlyActions", new Action<DataContext, DataUid>(this.InitializeMonthlyActions));
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x00157B4D File Offset: 0x00155D4D
		private void OnLoadedArchiveData()
		{
			this.InitRuntimeEnvironment();
			this._monthlyEventActionManager.OnArchiveDataLoaded();
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x00157B64 File Offset: 0x00155D64
		public override void FixAbnormalDomainArchiveData(DataContext dataContext)
		{
			bool flag = !this._monthlyEventActionManager.IsInitialized;
			if (flag)
			{
				this._monthlyEventActionManager.Init();
				this.SetMonthlyEventActionManager(this._monthlyEventActionManager, dataContext);
			}
			this._monthlyEventActionManager.HandleInvalidActions();
			this.BugFixForSpiritLandDisappear();
			this.FixFirstMartialArtTournament(dataContext);
			this.TaskCheckDaoShiAskForWildFood();
			this.FixShixiangExceptionTaskState(dataContext);
			this.FixShixiangArgKeysWrongState(dataContext);
			this.FixJingangInformation(dataContext);
			this.FixShixiangDrumEasterEggArgs(dataContext);
			this.ShaolinSectMainStoryInteractionSetData(dataContext);
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x00157BEC File Offset: 0x00155DEC
		private void InitRuntimeEnvironment()
		{
			this._triggeredEventList = new List<TaiwuEvent>();
			this.ShowingEvent = TaiwuEvent.Empty;
			this._notifyData = EventNotifyData.Empty;
			this.SetTempCreateItemList(new List<ItemKey>(), this.MainThreadDataContext);
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
				if (eventManagerBase != null)
				{
					eventManagerBase.ClearExtendOptions();
				}
			}
			string runtimeSettingsPath = Path.Combine(Common.ArchiveBaseDir, "EventScriptRuntimeSettings.json");
			this.ScriptRuntime.LoadSettings(runtimeSettingsPath);
			Events.RegisterHandler_AdvanceMonthBegin(new Events.OnAdvanceMonthBegin(this.OnAdvanceMonthBegin));
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x00157C88 File Offset: 0x00155E88
		private void InitializeMonthlyActions(DataContext context, DataUid dataUid)
		{
			bool flag = !DomainManager.Global.GetLoadedAllArchiveData();
			if (!flag)
			{
				this._monthlyEventActionManager.Init();
				this.SetMonthlyEventActionManager(this._monthlyEventActionManager, context);
				GameDataBridge.RemovePostDataModificationHandler(dataUid, "InitializeMonthlyActions");
			}
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x00157CD0 File Offset: 0x00155ED0
		private bool CanNextEvent()
		{
			bool stopAutoNextEvent = this._stopAutoNextEvent;
			bool result;
			if (stopAutoNextEvent)
			{
				result = false;
			}
			else
			{
				bool flag = DomainManager.Taiwu.GetLegacyPassingState() != 0;
				if (flag)
				{
					result = false;
				}
				else
				{
					TaiwuEvent topEvent = null;
					bool flag2 = this._triggeredEventList.Count > 0;
					if (flag2)
					{
						topEvent = this._triggeredEventList[0];
					}
					bool flag3 = topEvent == null;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = !this.NotAffectedInProgress(topEvent.EventConfig.TriggerType);
						if (flag4)
						{
							bool flag5 = DomainManager.World.GetAdvancingMonthState() == 20;
							if (flag5)
							{
								return false;
							}
						}
						bool flag6 = DomainManager.Combat.IsInCombat() && topEvent.EventConfig.TriggerType != EventTrigger.CombatOpening && topEvent.EventConfig.TriggerType != EventTrigger.SoulWitheringBellTransfer;
						result = !flag6;
					}
				}
			}
			return result;
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x00157DB4 File Offset: 0x00155FB4
		private void ResetArgBoxEventSelectData(EventArgBox argBox)
		{
			argBox.Set("SelectItemInfo", null);
			argBox.Set("SelectCharacterData", null);
			argBox.Set("InputRequestData", null);
			argBox.Set("SelectReadingBookCount", null);
			argBox.Set("SelectNeigongLoopingCount", null);
			argBox.Set("SelectFameData", null);
			argBox.Set("SelectFuyuFaithCount", null);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x00157E20 File Offset: 0x00156020
		private void UpdateEventDisplayData()
		{
			TaiwuEvent showingEvent = this.ShowingEvent;
			bool flag = showingEvent != null && !showingEvent.IsEmpty;
			if (flag)
			{
				this._eventEnteredList.Clear();
				TaiwuEvent eventItem;
				do
				{
					eventItem = this.ShowingEvent;
					this.ShowingEvent.ArgBox = this.ShowingEvent.ArgBox;
					bool flag2 = !this.ShowingEvent.TryExecuteScript(TaiwuEventDomain._scriptRuntime) && !this._eventEnteredList.Contains(this.ShowingEvent.EventGuid);
					if (flag2)
					{
						this._eventEnteredList.Add(this.ShowingEvent.EventGuid);
						this.ShowingEvent.EventConfig.OnEventEnter();
					}
				}
				while (eventItem != this.ShowingEvent && !this.ShowingEvent.IsEmpty);
			}
			try
			{
				showingEvent = this.ShowingEvent;
				bool flag3 = showingEvent != null && !showingEvent.IsEmpty;
				if (flag3)
				{
					bool flag4 = !string.IsNullOrEmpty(this.ShowingEvent.EventConfig.TargetRoleKey);
					if (flag4)
					{
						GameData.Domains.Character.Character targetChar = this.ShowingEvent.ArgBox.GetCharacter(this.ShowingEvent.EventConfig.TargetRoleKey);
						bool flag5 = targetChar != null;
						if (flag5)
						{
							bool flag6 = targetChar.GetId() != EventArgBox.TaiwuCharacterId;
							if (flag6)
							{
								bool flag7 = targetChar.GetCreatingType() == 1;
								if (flag7)
								{
									DomainManager.Character.TryCreateRelation(this.MainThreadDataContext, EventArgBox.TaiwuCharacterId, targetChar.GetId());
								}
								DomainManager.Extra.AddInteractedCharacter(this.MainThreadDataContext, targetChar.GetId());
							}
						}
					}
					TaiwuEventDisplayData displayData = this.ShowingEvent.ToDisplayData();
					this.SetDisplayingEventData(displayData, this.MainThreadDataContext);
					return;
				}
			}
			catch (Exception e)
			{
				AdaptableLog.Info(this.ShowingEvent.EventGuid);
				AdaptableLog.Warning(e.ToString(), false);
				this.ShowingEvent = TaiwuEvent.Empty;
				bool flag8 = this._triggeredEventList.Count > 0;
				if (flag8)
				{
					this.NextEvent();
				}
			}
			this.SetDisplayingEventData(null, this.MainThreadDataContext);
			bool flag9 = !this.IsShowingEvent && this._triggeredEventList.Count <= 0;
			if (flag9)
			{
				Events.RaiseEventHandleComplete(this.MainThreadDataContext);
			}
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00158088 File Offset: 0x00156288
		private void TriggerHandled()
		{
			List<TaiwuEvent> triggeredEventList = this._triggeredEventList;
			bool ignoreShowingEvent = triggeredEventList != null && triggeredEventList.Count > 0 && this._triggeredEventList.First<TaiwuEvent>().EventConfig.IgnoreShowingEvent;
			bool flag;
			if (!ignoreShowingEvent)
			{
				TaiwuEvent showingEvent = this.ShowingEvent;
				flag = (showingEvent != null && !showingEvent.IsEmpty);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				bool flag3 = this._triggeredEventList == null || this._triggeredEventList.Count <= 0;
				if (flag3)
				{
					Events.RaiseEventHandleComplete(this.MainThreadDataContext);
				}
				else
				{
					TaiwuEvent eventItem = this._triggeredEventList[0];
					bool flag4 = !this.NotAffectedInProgress(eventItem.EventConfig.TriggerType);
					if (flag4)
					{
						sbyte advanceState = DomainManager.World.GetAdvancingMonthState();
						bool flag5 = advanceState != 0;
						if (flag5)
						{
							return;
						}
					}
					bool flag6;
					if (!ignoreShowingEvent)
					{
						TaiwuEvent showingEvent = this.ShowingEvent;
						if (showingEvent == null || !showingEvent.IsEmpty)
						{
							flag6 = false;
							goto IL_E9;
						}
					}
					flag6 = this.CanNextEvent();
					IL_E9:
					bool flag7 = flag6;
					if (flag7)
					{
						this.NextEvent();
					}
				}
			}
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x0015818C File Offset: 0x0015638C
		private void NextEvent()
		{
			this.ShowingEvent = TaiwuEvent.Empty;
			bool flag = this._triggeredEventList.Count <= 0;
			if (flag)
			{
				this.SetDisplayingEventData(null, this.MainThreadDataContext);
			}
			else
			{
				int i = 0;
				int max = this._triggeredEventList.Count;
				while (i < max)
				{
					bool flag2 = this._triggeredEventList[i].EventConfig.CheckCondition();
					if (flag2)
					{
						this.ShowingEvent = this._triggeredEventList[i];
						this._triggeredEventList.RemoveAt(i);
						break;
					}
					i++;
				}
				TaiwuEvent showingEvent = this.ShowingEvent;
				bool flag3 = showingEvent != null && showingEvent.IsEmpty;
				if (flag3)
				{
					this._triggeredEventList.ForEach(delegate(TaiwuEvent e)
					{
						AdaptableLog.Warning("event " + e.EventGuid + " has triggered but failed to execute,removed trigger", false);
						e.ArgBox = null;
					});
					this._triggeredEventList.Clear();
				}
				this.UpdateEventDisplayData();
			}
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x00158288 File Offset: 0x00156488
		private bool NotAffectedInProgress(short triggerType)
		{
			return triggerType == EventTrigger.NeedToPassLegacy || triggerType == EventTrigger.LifeSkillCombatForceSilent || triggerType == EventTrigger.CombatOpening;
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x001582B8 File Offset: 0x001564B8
		private void HandleOptionConsume(TaiwuEventOption option, string mainRoleKey, string targetRoleKey)
		{
			bool flag = option == null || option.OptionConsumeInfos == null;
			if (!flag)
			{
				GameData.Domains.Character.Character characterA = option.ArgBox.GetCharacter("RoleTaiwu");
				GameData.Domains.Character.Character characterB = null;
				bool flag2 = !string.IsNullOrEmpty(targetRoleKey);
				if (flag2)
				{
					characterB = option.ArgBox.GetCharacter(targetRoleKey);
				}
				for (int i = 0; i < option.OptionConsumeInfos.Count; i++)
				{
					OptionConsumeInfo consumeInfo = OptionConsumeHelper.ModifyOptionConsumeInfo(option.OptionConsumeInfos[i], option.ArgBox);
					consumeInfo.DoConsume(characterA.GetId(), (characterB != null) ? characterB.GetId() : -1);
				}
			}
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00158364 File Offset: 0x00156564
		private void HandlerOptionEffect(TaiwuEventOption option)
		{
			bool flag = option == null;
			if (!flag)
			{
				sbyte behaviorType = EventOptionBehavior.ToBehaviorType[(int)option.Behavior];
				bool flag2 = behaviorType == -1;
				if (!flag2)
				{
					GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
					sbyte taiwuBehaviorType = taiwuChar.GetBehaviorType();
					bool flag3 = taiwuBehaviorType == behaviorType;
					if (flag3)
					{
						EventHelper.ChangeRoleHappiness(taiwuChar, 5);
					}
					else
					{
						bool flag4 = GameData.Domains.Character.BehaviorType.IsContradictory(taiwuBehaviorType, behaviorType);
						if (flag4)
						{
							EventHelper.ChangeRoleHappiness(taiwuChar, -5);
						}
					}
					short delta = GameData.Domains.Character.BehaviorType.GetBehaviorChangeDeltaByEventSelect(behaviorType, taiwuChar.GetBaseMorality());
					EventHelper.ChangeRoleBaseBehaviorValue(taiwuChar, (int)delta);
				}
			}
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x001583EC File Offset: 0x001565EC
		private bool IsCharacterRelatedToEvent(TaiwuEvent eventItem, int charId)
		{
			bool isEmpty = eventItem.IsEmpty;
			bool result;
			if (isEmpty)
			{
				result = false;
			}
			else
			{
				bool flag = !string.IsNullOrEmpty(eventItem.EventConfig.MainRoleKey);
				if (flag)
				{
					int mainCharId = -1;
					bool flag2 = eventItem.ArgBox.Get(eventItem.EventConfig.MainRoleKey, ref mainCharId) && mainCharId == charId;
					if (flag2)
					{
						return true;
					}
				}
				bool flag3 = !string.IsNullOrEmpty(eventItem.EventConfig.TargetRoleKey);
				if (flag3)
				{
					int targetCharId = -1;
					bool flag4 = eventItem.ArgBox.Get(eventItem.EventConfig.TargetRoleKey, ref targetCharId) && targetCharId == charId;
					if (flag4)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x001584A0 File Offset: 0x001566A0
		private void OnAdvanceMonthBegin(DataContext context)
		{
			List<ItemKey> tempCreatedItemKeyList = this.GetTempCreateItemList();
			bool flag = tempCreatedItemKeyList != null && tempCreatedItemKeyList.Count > 0;
			if (flag)
			{
				DomainManager.Item.RemoveItems(context, tempCreatedItemKeyList);
			}
			tempCreatedItemKeyList.Clear();
			this.SetTempCreateItemList(tempCreatedItemKeyList, this.MainThreadDataContext);
			this._executedOncePerMonthOptions.Clear();
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x001584F8 File Offset: 0x001566F8
		private bool IsEventStay(string eventGuid, string optionKey)
		{
			bool flag = this._selectInformationData != null && this._selectInformationData.AvailableData && !this._selectInformationData.SelectComplete;
			bool result;
			if (flag)
			{
				bool isForShopping = this._selectInformationData.IsForShopping;
				if (isForShopping)
				{
					result = false;
				}
				else
				{
					this._selectInformationData.SelectForEventGuid = eventGuid;
					this._selectInformationData.SelectForOptionKey = optionKey;
					result = true;
				}
			}
			else
			{
				bool flag2 = this._cricketBettingData.IsValid && !this._cricketBettingData.IsComplete;
				if (flag2)
				{
					this._cricketBettingData.SelectForEventGuid = eventGuid;
					this._cricketBettingData.SelectForOptionKey = optionKey;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x001585A4 File Offset: 0x001567A4
		public EventArgBox GetEventArgBox()
		{
			return this._argBoxPool.Get();
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x001585C4 File Offset: 0x001567C4
		public void ReturnArgBox(EventArgBox argBox)
		{
			bool flag = argBox == null;
			if (!flag)
			{
				argBox.Clear();
				this._argBoxPool.Return(argBox);
			}
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x001585F0 File Offset: 0x001567F0
		public bool IsTriggeredEvent(string guid)
		{
			bool flag = this.ShowingEvent != null && this.ShowingEvent.EventGuid == guid;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this._triggeredEventList != null;
				if (flag2)
				{
					foreach (TaiwuEvent eventItem in this._triggeredEventList)
					{
						bool flag3 = eventItem.EventGuid == guid;
						if (flag3)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x00158694 File Offset: 0x00156894
		public int GetEventTriggeredCount(string guid)
		{
			int count = 0;
			bool flag = this.ShowingEvent != null && this.ShowingEvent.EventGuid == guid;
			if (flag)
			{
				count++;
			}
			bool flag2 = this._triggeredEventList != null;
			if (flag2)
			{
				foreach (TaiwuEvent eventItem in this._triggeredEventList)
				{
					bool flag3 = eventItem.EventGuid == guid;
					if (flag3)
					{
						count++;
					}
				}
			}
			return count;
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0015873C File Offset: 0x0015693C
		public void AddTriggeredEvent(TaiwuEvent eventItem)
		{
			bool flag = false;
			bool flag2 = this._triggeredEventList.Count > 0;
			if (flag2)
			{
				for (int i = 0; i < this._triggeredEventList.Count; i++)
				{
					bool flag3 = this._triggeredEventList[i].EventConfig.EventSortingOrder < eventItem.EventConfig.EventSortingOrder;
					if (flag3)
					{
						bool flag4 = !this._triggeredEventList.Contains(eventItem);
						if (flag4)
						{
							this._triggeredEventList.Insert(i, eventItem);
						}
						flag = true;
						break;
					}
				}
			}
			bool flag5 = !flag;
			if (flag5)
			{
				bool flag6 = !this._triggeredEventList.Contains(eventItem);
				if (flag6)
				{
					this._triggeredEventList.Add(eventItem);
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 2);
			defaultInterpolatedStringHandler.AppendLiteral("new Event triggered : ");
			defaultInterpolatedStringHandler.AppendFormatted(eventItem.EventGuid);
			defaultInterpolatedStringHandler.AppendLiteral(", _triggeredEventList.Count = ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._triggeredEventList.Count);
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x00158850 File Offset: 0x00156A50
		public void ToEvent(string eventGuid)
		{
			bool isEmpty = this.ShowingEvent.IsEmpty;
			if (isEmpty)
			{
				throw new Exception("Failed to new event because no event showing!");
			}
			bool flag = eventGuid == this.ShowingEvent.EventGuid;
			if (flag)
			{
				throw new Exception(eventGuid + " try to use ToEvent to self,this is not allowed!");
			}
			TaiwuEventItem eventConfig = this.ShowingEvent.EventConfig;
			if (eventConfig != null)
			{
				eventConfig.OnEventExit();
			}
			AdaptableLog.Info(this.ShowingEvent.EventGuid + " to event => " + eventGuid);
			bool flag2 = string.IsNullOrEmpty(eventGuid);
			if (flag2)
			{
				this.ShowingEvent = TaiwuEvent.Empty;
				bool flag3 = this.CanNextEvent() && !this.GetHasListeningEvent();
				if (flag3)
				{
					this.NextEvent();
				}
				else
				{
					Events.RaiseEventHandleComplete(this.MainThreadDataContext);
					bool flag4 = DomainManager.World.GetAdvancingMonthState() != 0 && !this.GetHasListeningEvent();
					if (flag4)
					{
						DomainManager.World.SetOnHandingMonthlyEventBlock(false, DataContextManager.GetCurrentThreadDataContext());
					}
				}
			}
			else
			{
				TaiwuEvent eventItem = this.GetEvent(eventGuid);
				bool flag5 = eventItem != null;
				if (flag5)
				{
					eventItem.ArgBox = this.ShowingEvent.ArgBox;
					this.ShowingEvent.ArgBox = null;
					bool flag6 = eventItem.EventConfig.CheckCondition();
					if (flag6)
					{
						this.ShowingEvent = eventItem;
					}
					else
					{
						eventItem.ArgBox = null;
						bool flag7 = this.CanNextEvent();
						if (flag7)
						{
							this.NextEvent();
						}
						else
						{
							this.ShowingEvent = TaiwuEvent.Empty;
						}
					}
				}
			}
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x001589C7 File Offset: 0x00156BC7
		public void SetStopAutoNextEvent(bool flag)
		{
			this._stopAutoNextEvent = flag;
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x001589D4 File Offset: 0x00156BD4
		public TaiwuEvent GetEvent(string guid)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
				TaiwuEvent taiwuEvent = (eventManagerBase != null) ? eventManagerBase.GetEvent(guid) : null;
				bool flag = taiwuEvent != null;
				if (flag)
				{
					return taiwuEvent;
				}
			}
			return null;
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x00158A22 File Offset: 0x00156C22
		public void TravelingEventCheckComplete()
		{
			this.TriggerHandled();
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x00158A2C File Offset: 0x00156C2C
		[DomainMethod]
		public void SetItemSelectResult(string key, ItemKey itemKey, bool callComplete)
		{
			bool flag = this.ShowingEvent == null || string.IsNullOrEmpty(key);
			if (!flag)
			{
				EventSelectItemData data;
				bool flag2 = this.ShowingEvent.ArgBox.Get<EventSelectItemData>("SelectItemInfo", out data);
				if (flag2)
				{
					this.ShowingEvent.ArgBox.Set(key, itemKey);
					if (callComplete)
					{
						Action onSelectFinish = data.OnSelectFinish;
						if (onSelectFinish != null)
						{
							onSelectFinish();
						}
					}
				}
			}
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x00158AA0 File Offset: 0x00156CA0
		[DomainMethod]
		public void SetItemSelectCount(string key, int count)
		{
			bool flag = this.ShowingEvent == null || string.IsNullOrEmpty(key);
			if (!flag)
			{
				EventSelectItemData data;
				bool flag2 = this.ShowingEvent.ArgBox.Get<EventSelectItemData>("SelectItemInfo", out data);
				if (flag2)
				{
					this.ShowingEvent.ArgBox.Set(key + "Count", count);
				}
			}
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00158B00 File Offset: 0x00156D00
		[DomainMethod]
		public void SetCharacterSelectResult(string key, int charId, bool callComplete)
		{
			bool flag = this.ShowingEvent == null || string.IsNullOrEmpty(key);
			if (!flag)
			{
				EventSelectCharacterData selectCharacterData;
				bool flag2 = this.ShowingEvent.ArgBox.Get<EventSelectCharacterData>("SelectCharacterData", out selectCharacterData);
				if (flag2)
				{
					this.ShowingEvent.ArgBox.Set(key, charId);
					if (callComplete)
					{
						Action onSelectComplete = selectCharacterData.OnSelectComplete;
						if (onSelectComplete != null)
						{
							onSelectComplete();
						}
					}
				}
			}
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x00158B70 File Offset: 0x00156D70
		[DomainMethod]
		public void SetCharacterMultSelectResult(string key, List<int> charIds, bool callComplete)
		{
			bool flag = this.ShowingEvent == null || string.IsNullOrEmpty(key);
			if (!flag)
			{
				EventSelectCharacterData selectCharacterData;
				bool flag2 = this.ShowingEvent.ArgBox.Get<EventSelectCharacterData>("SelectCharacterData", out selectCharacterData);
				if (flag2)
				{
					IntList characters = IntList.Create();
					characters.Items = charIds;
					this.ShowingEvent.ArgBox.Set(key, characters);
					if (callComplete)
					{
						Action onSelectComplete = selectCharacterData.OnSelectComplete;
						if (onSelectComplete != null)
						{
							onSelectComplete();
						}
					}
				}
			}
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x00158BF4 File Offset: 0x00156DF4
		[DomainMethod]
		public void SetCharacterSetSelectResult(string actionName, string key, CharacterSet characterSet)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				for (int i = 0; i < this.ListeningEventActionList.Count; i++)
				{
					bool flag2 = this.ListeningEventActionList[i].Item1.Equals(actionName);
					if (flag2)
					{
						TaiwuEvent item = this.ListeningEventActionList[i].Item2;
						bool flag3 = item != null && !item.IsEmpty;
						if (flag3)
						{
							this.ListeningEventActionList[i].Item2.ArgBox.Set(key, characterSet);
						}
					}
				}
			}
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00158C9C File Offset: 0x00156E9C
		[DomainMethod]
		public void SetSecretInformationSelectResult(string key, int informationMetaDataId)
		{
			bool flag = this._selectInformationData == null;
			if (!flag)
			{
				bool flag2 = SecretInformationDisplayData.IsSecretInformationValid(informationMetaDataId);
				if (flag2)
				{
					this.ShowingEvent.ArgBox.Set(key, informationMetaDataId);
					this._selectInformationData.SelectComplete = true;
					this.EventSelect(this._selectInformationData.SelectForEventGuid, this._selectInformationData.SelectForOptionKey, false);
				}
				else
				{
					this.UpdateEventDisplayData();
				}
				this.SetSelectInformationData(null, this.MainThreadDataContext);
			}
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00158D1C File Offset: 0x00156F1C
		[DomainMethod]
		public void SetNormalInformationSelectResult(string key, NormalInformation normalInformation)
		{
			bool flag = this._selectInformationData == null;
			if (!flag)
			{
				bool flag2 = normalInformation.IsValid();
				if (flag2)
				{
					this.ShowingEvent.ArgBox.Set(key, normalInformation);
					this._selectInformationData.SelectComplete = true;
					this.EventSelect(this._selectInformationData.SelectForEventGuid, this._selectInformationData.SelectForOptionKey, false);
				}
				else
				{
					this.UpdateEventDisplayData();
				}
				this.SetSelectInformationData(null, this.MainThreadDataContext);
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00158DA0 File Offset: 0x00156FA0
		[DomainMethod]
		public void SetCombatSkillSelectResult(short combatSkillId)
		{
			TaiwuEvent showingEvent = this.ShowingEvent;
			bool flag = showingEvent != null && !showingEvent.IsEmpty && this._selectCombatSkillData != null;
			if (flag)
			{
				bool flag2 = combatSkillId >= 0;
				if (flag2)
				{
					this.ShowingEvent.ArgBox.Set(this._selectCombatSkillData.ResultSaveKey, combatSkillId);
				}
				this.SetSelectCombatSkillData(null, this.MainThreadDataContext);
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x00158E08 File Offset: 0x00157008
		[DomainMethod]
		public void SetLifeSkillSelectResult(short lifeSkillId)
		{
			TaiwuEvent showingEvent = this.ShowingEvent;
			bool flag = showingEvent != null && !showingEvent.IsEmpty && this._selectLifeSkillData != null;
			if (flag)
			{
				bool flag2 = lifeSkillId >= 0;
				if (flag2)
				{
					this.ShowingEvent.ArgBox.Set(this._selectLifeSkillData.ResultSaveKey, lifeSkillId);
				}
				this.SetSelectLifeSkillData(null, this.MainThreadDataContext);
			}
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x00158E70 File Offset: 0x00157070
		[DomainMethod]
		public void SetCricketBettingResult(bool ok, Wager wager, int index)
		{
			bool flag = !this._cricketBettingData.IsValid;
			if (!flag)
			{
				this._cricketBettingData.IsValid = false;
				this._cricketBettingData.IsComplete = ok;
				this._cricketBettingData.IsConfirmed = ok;
				this._cricketBettingData.Wager = wager;
				this._cricketBettingData.Index = index;
				this.SetCricketBettingData(this._cricketBettingData, this.MainThreadDataContext);
				if (ok)
				{
					CricketWagerData data = this._cricketBettingData.BetRewards[index];
					DomainManager.Item.SetWager(wager, data.Wager);
					this.EventSelect(this._cricketBettingData.SelectForEventGuid, this._cricketBettingData.SelectForOptionKey, false);
				}
				else
				{
					this.UpdateEventDisplayData();
				}
			}
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x00158F38 File Offset: 0x00157138
		[DomainMethod]
		public void SetSelectCount(int count)
		{
			bool flag = this.ShowingEvent == null;
			if (!flag)
			{
				this.ShowingEvent.ArgBox.Set("SelectCountResult", count);
			}
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00158F6C File Offset: 0x0015716C
		[DomainMethod]
		public void StartHandleEventDuringAdvance()
		{
			this.NextEvent();
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x00158F78 File Offset: 0x00157178
		[DomainMethod]
		public List<TaiwuEventSummaryDisplayData> GetTriggeredEventSummaryDisplayData()
		{
			List<TaiwuEventSummaryDisplayData> list = new List<TaiwuEventSummaryDisplayData>();
			foreach (TaiwuEvent item in this._triggeredEventList)
			{
				TaiwuEventSummaryDisplayData data = item.ToSummaryDisplayData();
				bool flag = data != null;
				if (flag)
				{
					list.Add(data);
				}
			}
			return list;
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x00158FF4 File Offset: 0x001571F4
		[DomainMethod]
		public void SetEventInProcessing(string eventGuid)
		{
			foreach (TaiwuEvent eventItem in this._triggeredEventList)
			{
				bool flag = eventItem.EventGuid == eventGuid;
				if (flag)
				{
					this.ShowingEvent = eventItem;
					this._triggeredEventList.Remove(eventItem);
					this.UpdateEventDisplayData();
					break;
				}
			}
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x00159074 File Offset: 0x00157274
		public void ProcessEventWithDefaultOption(string guid, EventArgBox eventArgBox, sbyte behaviorType)
		{
			HashSet<string> selected = ObjectPool<HashSet<string>>.Instance.Get();
			selected.Clear();
			while (!string.IsNullOrEmpty(guid))
			{
				bool flag = !selected.Add(guid);
				if (flag)
				{
					throw new Exception("Loop detected when executing event " + guid);
				}
				TaiwuEvent taiwuEvent = this.GetEvent(guid);
				bool flag2 = taiwuEvent.ArgBox != null;
				if (flag2)
				{
					this.ReturnArgBox(taiwuEvent.ArgBox);
				}
				taiwuEvent.ArgBox = eventArgBox;
				this._triggeredEventList.Remove(taiwuEvent);
				bool flag3 = !taiwuEvent.EventConfig.CheckCondition();
				if (flag3)
				{
					break;
				}
				taiwuEvent.EventConfig.OnEventEnter();
				bool flag4 = string.IsNullOrEmpty(taiwuEvent.EventConfig.EscOptionKey);
				if (flag4)
				{
					bool flag5 = taiwuEvent.EventConfig.EventOptions == null || taiwuEvent.EventConfig.EventOptions.Length == 0;
					if (flag5)
					{
						TaiwuEventDomain.Logger.AppendWarning("Monthly event " + taiwuEvent.EventGuid + " has no option detected when trying to process with default option.");
						guid = string.Empty;
					}
					else
					{
						bool optionSelected = false;
						foreach (TaiwuEventOption option in taiwuEvent.EventConfig.EventOptions)
						{
							bool flag6 = EventOptionBehavior.ToBehaviorType[(int)option.Behavior] != behaviorType;
							if (!flag6)
							{
								guid = option.Select(TaiwuEventDomain._scriptRuntime);
								optionSelected = true;
								break;
							}
						}
						bool flag7 = !optionSelected;
						if (flag7)
						{
							TaiwuEventDomain.Logger.AppendWarning("Monthly event " + taiwuEvent.EventGuid + " has neither esc option nor behavior option to handle as default.");
							guid = string.Empty;
						}
					}
				}
				else
				{
					guid = taiwuEvent.EventConfig[taiwuEvent.EventConfig.EscOptionKey].Select(TaiwuEventDomain._scriptRuntime);
				}
				taiwuEvent.EventConfig.OnEventExit();
				taiwuEvent.ArgBox = null;
			}
			ObjectPool<HashSet<string>>.Instance.Return(selected);
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0015926C File Offset: 0x0015746C
		[DomainMethod]
		public void EventSelect(string eventGuid, string optionKey, bool isContinue = false)
		{
			TaiwuEvent showingEvent = this.ShowingEvent;
			bool flag = (showingEvent != null && showingEvent.IsEmpty) || eventGuid != this.ShowingEvent.EventGuid;
			if (!flag)
			{
				TaiwuEventItem eventConfig = this.ShowingEvent.EventConfig;
				EventArgBox passBox = this.ShowingEvent.ArgBox;
				TaiwuEventOption option = eventConfig[optionKey];
				string guidString;
				if (isContinue)
				{
					this.ShowInteractCheckAnimation = false;
					guidString = this._interactCheckContinueData.Item3;
					this._interactCheckContinueData = new ValueTuple<string, string, string>(string.Empty, string.Empty, string.Empty);
				}
				else
				{
					guidString = option.Select(TaiwuEventDomain._scriptRuntime);
				}
				bool showInteractCheckAnimation = this.ShowInteractCheckAnimation;
				if (showInteractCheckAnimation)
				{
					this._interactCheckContinueData = new ValueTuple<string, string, string>(eventGuid, optionKey, guidString);
					GameDataBridge.AddDisplayEvent<EventInteractCheckData>(DisplayEventType.InteractCheckAnimation, this.InteractCheckData);
				}
				else
				{
					int eventLogMainCharacterId = -1;
					this.GenerateResponseLog(optionKey, passBox.Get("ConchShip_PresetKey_EventLogMainCharacter", ref eventLogMainCharacterId) ? eventLogMainCharacterId : -1);
					bool flag2 = this.IsEventStay(eventGuid, optionKey);
					if (!flag2)
					{
						string waitConfirmKey = string.Empty;
						bool flag3 = eventConfig.ArgBox.Get("ConchShip_PresetKey_OptionWaitConfirm", ref waitConfirmKey);
						if (flag3)
						{
							this._waitConfirmOptionKey = waitConfirmKey;
							this._waitConfirmEventConfig = eventConfig;
							this._waitConfirmSelectOption = optionKey;
							eventConfig.ArgBox.Remove<string>("ConchShip_PresetKey_OptionWaitConfirm");
						}
						else
						{
							string confirmSignalKey = string.Empty;
							bool flag4 = eventConfig.ArgBox.Get("ConchShip_PresetKey_ConfirmWaitOptionSignal", ref confirmSignalKey);
							if (flag4)
							{
								bool flag5 = confirmSignalKey == this._waitConfirmOptionKey;
								if (flag5)
								{
									this._waitConfirmEventConfig.ArgBox = eventConfig.ArgBox;
									this.HandleOptionConsume(this._waitConfirmEventConfig[this._waitConfirmSelectOption], this._waitConfirmEventConfig.MainRoleKey, this._waitConfirmEventConfig.TargetRoleKey);
									this.HandlerOptionEffect(this._waitConfirmEventConfig[this._waitConfirmSelectOption]);
									this._waitConfirmOptionKey = string.Empty;
									this._waitConfirmEventConfig = null;
									this._waitConfirmSelectOption = string.Empty;
								}
								eventConfig.ArgBox.Remove<string>("ConchShip_PresetKey_ConfirmWaitOptionSignal");
							}
							this.HandleOptionConsume(eventConfig[optionKey], eventConfig.MainRoleKey, eventConfig.TargetRoleKey);
							this.HandlerOptionEffect(eventConfig[optionKey]);
						}
						eventConfig.OnEventExit();
						this.ResetArgBoxEventSelectData(passBox);
						bool flag6 = string.IsNullOrEmpty(guidString);
						if (flag6)
						{
							bool flag7;
							if (this.GetHasListeningEvent())
							{
								TaiwuEvent showingEvent2 = this._showingEvent;
								List<ValueTuple<string, TaiwuEvent>> listeningEventActionList = this.ListeningEventActionList;
								flag7 = (showingEvent2 != listeningEventActionList[listeningEventActionList.Count - 1].Item2);
							}
							else
							{
								flag7 = false;
							}
							bool flag8 = flag7;
							if (flag8)
							{
								this.ShowingEvent.ArgBox = null;
							}
							this.ShowingEvent = TaiwuEvent.Empty;
							bool hasListeningEvent = this.GetHasListeningEvent();
							if (hasListeningEvent)
							{
								this.SetDisplayingEventData(null, this.MainThreadDataContext);
								Events.RaiseEventHandleComplete(this.MainThreadDataContext);
							}
							else
							{
								bool flag9 = this.CanNextEvent();
								if (flag9)
								{
									this.NextEvent();
								}
								else
								{
									this.SetDisplayingEventData(null, this.MainThreadDataContext);
									Events.RaiseEventHandleComplete(this.MainThreadDataContext);
									bool flag10 = DomainManager.World.GetAdvancingMonthState() != 0;
									if (flag10)
									{
										DomainManager.World.SetOnHandingMonthlyEventBlock(false, DataContextManager.GetCurrentThreadDataContext());
									}
								}
							}
						}
						else
						{
							AdaptableLog.Info("select option to next Event: " + guidString);
							this.ShowingEvent.ArgBox = null;
							TaiwuEvent nextEvent = this.GetEvent(guidString);
							bool flag11 = nextEvent != null;
							if (flag11)
							{
								nextEvent.ArgBox = passBox;
								bool flag12 = nextEvent.EventConfig.CheckCondition();
								if (flag12)
								{
									this.ShowingEvent = nextEvent;
									this.UpdateEventDisplayData();
								}
								else
								{
									bool flag13 = this.CanNextEvent();
									if (flag13)
									{
										this.NextEvent();
									}
									else
									{
										this.ShowingEvent = TaiwuEvent.Empty;
										this.SetDisplayingEventData(null, this.MainThreadDataContext);
										Events.RaiseEventHandleComplete(this.MainThreadDataContext);
									}
								}
							}
							else
							{
								AdaptableLog.TagError("TaiwuEvent", "can not find event " + guidString);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00159656 File Offset: 0x00157856
		[DomainMethod]
		public void EventSelectContinue()
		{
			this.EventSelect(this._interactCheckContinueData.Item1, this._interactCheckContinueData.Item2, true);
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x00159678 File Offset: 0x00157878
		[DomainMethod]
		public List<int> GetImplementedFunctionIds(DataContext context)
		{
			return new List<int>(TaiwuEventDomain._scriptRuntime.ImplementedFunctionIds);
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x0015969C File Offset: 0x0015789C
		[DomainMethod]
		[Obsolete("use UpdateEventDisplayData instead")]
		public List<TaiwuEventDisplayData> GetEventDisplayData()
		{
			return null;
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x001596B0 File Offset: 0x001578B0
		[DomainMethod]
		public void SetShowingEventShortListArg(string key, GameData.Utilities.ShortList value)
		{
			bool flag = this.ShowingEvent == null;
			if (!flag)
			{
				this.ShowingEvent.ArgBox.Set(key, value);
			}
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x001596E8 File Offset: 0x001578E8
		private void BugFixForSpiritLandDisappear()
		{
			bool flag = DomainManager.World.GetMainStoryLineProgress() == 26;
			if (flag)
			{
				bool flag2 = !EventHelper.GlobalArgBoxContainsKey<int>("ImmortalXuMoveForSpiriteLand");
				if (flag2)
				{
					AreaAdventureData areaAdventureData = DomainManager.Adventure.GetAdventuresInArea(EventArgBox.TaiwuVillageAreaId);
					foreach (KeyValuePair<short, AdventureSiteData> pair in areaAdventureData.AdventureSites)
					{
						bool flag3 = pair.Value.TemplateId == 107 && pair.Value.SiteState == 1;
						if (flag3)
						{
							return;
						}
					}
					EventHelper.SaveGlobalArg<int>("ImmortalXuMoveForSpiriteLand", 3);
				}
			}
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x001597AC File Offset: 0x001579AC
		private void FixFirstMartialArtTournament(DataContext context)
		{
			short currMainStoryProgress = DomainManager.World.GetMainStoryLineProgress();
			bool flag = currMainStoryProgress != 22;
			if (!flag)
			{
				bool @bool = this._globalArgBox.GetBool("WaitForFirstWulinConference");
				if (@bool)
				{
					List<short> previousHosts = DomainManager.Organization.GetPreviousMartialArtTournamentHosts();
					bool flag2 = previousHosts.Count > 0;
					if (flag2)
					{
						Logger logger = TaiwuEventDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Clearing previous martial art tournament hosts: count = ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(previousHosts.Count);
						logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						previousHosts.Clear();
						DomainManager.Organization.SetPreviousMartialArtTournamentHosts(previousHosts, context);
					}
					MonthlyActionKey martialArtTournamentKey = MonthlyEventActionsManager.PredefinedKeys["MartialArtTournamentDefault"];
					MartialArtTournamentMonthlyAction action = this.GetMonthlyAction(martialArtTournamentKey) as MartialArtTournamentMonthlyAction;
					bool flag3 = action != null && action.State == 0 && action.LastFinishDate >= 0;
					if (flag3)
					{
						action.LastFinishDate = int.MinValue;
						Logger logger2 = TaiwuEventDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Fixing last martial art tournament date: ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(action.LastFinishDate);
						logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x001598E0 File Offset: 0x00157AE0
		private void FixShixiangExceptionTaskState(DataContext context)
		{
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(6);
			bool flag = argBox.Contains<int>("ConchShip_PresetKey_SectMainStoryShixiangProsperousEndDate") || DomainManager.World.GetSectMainStoryTaskStatus(6) != 0;
			if (flag)
			{
				bool flag2 = DomainManager.Extra.IsExtraTaskInProgress(233);
				if (flag2)
				{
					DomainManager.Extra.FinishTriggeredExtraTask(context, 34, 233);
					TaiwuEventDomain.Logger.Warn("Fix Shixiang Exception TaskState");
				}
			}
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x00159958 File Offset: 0x00157B58
		private void FixShixiangArgKeysWrongState(DataContext context)
		{
			bool flag = !DomainManager.World.ShixiangSettlementAffiliatedBlocksHasEnemy(context, 613);
			if (flag)
			{
				EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(6);
				bool @bool = sectArgBox.GetBool("ConchShip_PresetKey_ShixiangToFightEnemy");
				if (@bool)
				{
					sectArgBox.Remove<bool>("ConchShip_PresetKey_ShixiangToFightEnemy");
					DomainManager.Extra.SaveSectMainStoryEventArgumentBox(context, 6);
					TaiwuEventDomain.Logger.Warn("Fix Shixiang ArgKeys WrongState,key name:ConchShip_PresetKey_ShixiangToFightEnemy");
				}
			}
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x001599C8 File Offset: 0x00157BC8
		private void FixJingangInformation(DataContext context)
		{
			bool flag = DomainManager.World.GetSectMainStoryTaskStatus(11) != 0;
			if (flag)
			{
				DomainManager.Information.ReleaseAllJingangInformation(context);
			}
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x001599F8 File Offset: 0x00157BF8
		private void FixShixiangDrumEasterEggArgs(DataContext context)
		{
			EventArgBox argBox = EventHelper.GetGlobalArgBox();
			bool gotItems = false;
			argBox.Get("GivenShixiangDrumEasterEggItems", ref gotItems);
			bool enteredThisMonth = false;
			argBox.Get("EnteredShixiangDrumEasterEggThisMonth", ref enteredThisMonth);
			argBox.Remove<bool>("GivenShixiangDrumEasterEggItems");
			bool flag = gotItems;
			if (flag)
			{
				EventHelper.SaveArgToSectMainStory<bool>(6, "GivenShixiangDrumEasterEggItems", true);
			}
			argBox.Remove<bool>("EnteredShixiangDrumEasterEggThisMonth");
			bool flag2 = enteredThisMonth;
			if (flag2)
			{
				EventHelper.SaveArgToSectMainStory<bool>(6, "EnteredShixiangDrumEasterEggThisMonth", true);
			}
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x00159A70 File Offset: 0x00157C70
		private void ShaolinSectMainStoryInteractionSetData(DataContext context)
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(1);
			bool flag = !sectArgBox.Contains<bool>("ConchShip_PresetKey_ShaolinInteractionChangeFirstLoad");
			if (flag)
			{
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<bool>(context, 1, "ConchShip_PresetKey_ShaolinInteractionChangeFirstLoad", true);
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<bool>(context, 1, "ConchShip_PresetKey_ShaolinMeditationInteractionFinished", sectArgBox.GetBool("ConchShip_PresetKey_ShaolinMeditationInteractionActivated"));
			}
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00159AD0 File Offset: 0x00157CD0
		public void SetNewAdventure(short adventureId, EventArgBox argBox)
		{
			AdventureEventManager adventureEventManager = TaiwuEventDomain._managerArray[5] as AdventureEventManager;
			bool flag = adventureEventManager != null;
			if (flag)
			{
				adventureEventManager.SetNewAdventure(adventureId, argBox);
			}
			this.TriggerHandled();
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x00159B08 File Offset: 0x00157D08
		public void TriggerAdventureGlobalEvent(AdventureMapPoint node)
		{
			AdventureEventManager adventureEventManager = TaiwuEventDomain._managerArray[5] as AdventureEventManager;
			bool flag = adventureEventManager != null;
			if (flag)
			{
				adventureEventManager.TriggerGlobalEvent(node);
			}
			this.TriggerHandled();
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00159B3C File Offset: 0x00157D3C
		public void TriggerAdventureExtraEvent(string guid, AdventureMapPoint node)
		{
			AdventureEventManager adventureEventManager = TaiwuEventDomain._managerArray[5] as AdventureEventManager;
			bool flag = adventureEventManager != null;
			if (flag)
			{
				adventureEventManager.TriggerExtraEvent(guid, node);
			}
			this.TriggerHandled();
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x00159B74 File Offset: 0x00157D74
		public void SetNewAdventureByConfigData(AdventureItem configData, EventArgBox argBox)
		{
			AdventureEventManager adventureEventManager = TaiwuEventDomain._managerArray[5] as AdventureEventManager;
			bool flag = adventureEventManager != null;
			if (flag)
			{
				adventureEventManager.SetNewAdventureByConfigData(configData, argBox);
			}
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00159BA4 File Offset: 0x00157DA4
		public void AppendMarriageLook1CharId(int charId)
		{
			bool flag = this._marriageLook1CharIdList == null;
			if (flag)
			{
				this._marriageLook1CharIdList = new List<int>();
			}
			bool flag2 = !this._marriageLook1CharIdList.Contains(charId);
			if (flag2)
			{
				this._marriageLook1CharIdList.Add(charId);
				this.SetMarriageLook1CharIdList(this._marriageLook1CharIdList, this.MainThreadDataContext);
				GameData.Domains.Character.Character character;
				bool flag3 = DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag3)
				{
					character.SetAvatar(character.GetAvatar(), this.MainThreadDataContext);
				}
			}
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00159C28 File Offset: 0x00157E28
		public void RemoveMarriageLook1CharId(int charId)
		{
			bool flag = this._marriageLook1CharIdList != null && this._marriageLook1CharIdList.Contains(charId);
			if (flag)
			{
				this._marriageLook1CharIdList.Remove(charId);
				this.SetMarriageLook1CharIdList(this._marriageLook1CharIdList, this.MainThreadDataContext);
				GameData.Domains.Character.Character character;
				bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag2)
				{
					character.SetAvatar(character.GetAvatar(), this.MainThreadDataContext);
				}
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00159C9C File Offset: 0x00157E9C
		public void AppendMarriageLook2CharId(int charId)
		{
			bool flag = this._marriageLook2CharIdList == null;
			if (flag)
			{
				this._marriageLook2CharIdList = new List<int>();
			}
			bool flag2 = !this._marriageLook2CharIdList.Contains(charId);
			if (flag2)
			{
				this._marriageLook2CharIdList.Add(charId);
				this.SetMarriageLook2CharIdList(this._marriageLook2CharIdList, this.MainThreadDataContext);
				GameData.Domains.Character.Character character;
				bool flag3 = DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag3)
				{
					character.SetAvatar(character.GetAvatar(), this.MainThreadDataContext);
				}
			}
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00159D20 File Offset: 0x00157F20
		public void RemoveMarriageLook2CharId(int charId)
		{
			bool flag = this._marriageLook2CharIdList != null && this._marriageLook2CharIdList.Contains(charId);
			if (flag)
			{
				this._marriageLook2CharIdList.Remove(charId);
				this.SetMarriageLook2CharIdList(this._marriageLook2CharIdList, this.MainThreadDataContext);
				GameData.Domains.Character.Character character;
				bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag2)
				{
					character.SetAvatar(character.GetAvatar(), this.MainThreadDataContext);
				}
			}
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00159D94 File Offset: 0x00157F94
		public void ClearAllMarriageLook()
		{
			List<int> marriageLook1CharIdList = this._marriageLook1CharIdList;
			if (marriageLook1CharIdList != null)
			{
				marriageLook1CharIdList.Clear();
			}
			List<int> marriageLook2CharIdList = this._marriageLook2CharIdList;
			if (marriageLook2CharIdList != null)
			{
				marriageLook2CharIdList.Clear();
			}
			this.SetMarriageLook1CharIdList(this._marriageLook1CharIdList, this.MainThreadDataContext);
			this.SetMarriageLook2CharIdList(this._marriageLook2CharIdList, this.MainThreadDataContext);
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06001788 RID: 6024 RVA: 0x00159DEC File Offset: 0x00157FEC
		// (set) Token: 0x06001789 RID: 6025 RVA: 0x00159DF4 File Offset: 0x00157FF4
		public sbyte LegacyReason { get; private set; }

		// Token: 0x0600178A RID: 6026 RVA: 0x00159E00 File Offset: 0x00158000
		public void ClearAllTriggeredEvent()
		{
			bool flag = this.ListeningEventActionList.Count > 0;
			if (flag)
			{
				foreach (ValueTuple<string, TaiwuEvent> valueTuple in this.ListeningEventActionList)
				{
					this.ReturnArgBox(valueTuple.Item2.ArgBox);
				}
				this.ListeningEventActionList.Clear();
				this.SetHasListeningEvent(false, this.MainThreadDataContext);
			}
			foreach (TaiwuEvent queueEvent in this._triggeredEventList)
			{
				this.ReturnArgBox(queueEvent.ArgBox);
			}
			this._triggeredEventList.Clear();
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x00159EEC File Offset: 0x001580EC
		public void OnPassingLegacyStateChange(sbyte newState)
		{
			bool flag = newState == 0 && this.ShowingEvent.IsEmpty;
			if (flag)
			{
				this.SetLegacyReason(-1);
				this.NextEvent();
			}
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x00159F20 File Offset: 0x00158120
		public void OnTemporaryIntelligentCharacterRemoved(int charId)
		{
			bool flag = this.IsCharacterRelatedToEvent(this.ShowingEvent, charId);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				defaultInterpolatedStringHandler.AppendLiteral(" remove error:");
				defaultInterpolatedStringHandler.AppendFormatted(this.ShowingEvent.EventGuid);
				defaultInterpolatedStringHandler.AppendLiteral(" need this character");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			foreach (TaiwuEvent eventItem in this._triggeredEventList)
			{
				bool flag2 = this.IsCharacterRelatedToEvent(eventItem, charId);
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
					defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
					defaultInterpolatedStringHandler.AppendLiteral(" remove error:");
					defaultInterpolatedStringHandler.AppendFormatted(this.ShowingEvent.EventGuid);
					defaultInterpolatedStringHandler.AppendLiteral(" need this character");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x0015A02C File Offset: 0x0015822C
		public void SetTaiwuLocationDirtyFlag()
		{
			this._taiwuLocationChangeFlag = !this._taiwuLocationChangeFlag;
			this.SetTaiwuLocationChangeFlag(this._taiwuLocationChangeFlag, this.MainThreadDataContext);
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0015A054 File Offset: 0x00158254
		public void OnCharacterDie(int charId)
		{
			bool flag = this._triggeredEventList.Count > 0;
			if (flag)
			{
				for (int i = this._triggeredEventList.Count - 1; i >= 0; i--)
				{
					TaiwuEvent eventItem = this._triggeredEventList[i];
					bool flag2 = this.IsCharacterRelatedToEvent(eventItem, charId);
					if (flag2)
					{
						this._triggeredEventList.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x0015A0C4 File Offset: 0x001582C4
		public void GMCharacterDie(int charId)
		{
			TaiwuEvent showingEvent = this.ShowingEvent;
			bool flag = showingEvent != null && !showingEvent.IsEmpty && this.IsCharacterRelatedToEvent(this.ShowingEvent, charId);
			if (flag)
			{
				this.ShowingEvent = TaiwuEvent.Empty;
				this.UpdateEventDisplayData();
			}
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x0015A10D File Offset: 0x0015830D
		public void SetLegacyReason(sbyte reason)
		{
			this.LegacyReason = reason;
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x0015A118 File Offset: 0x00158318
		public void SetListenerWithActionName(string listenerEventGuid, EventArgBox eventBox, string actionName)
		{
			if (eventBox != null)
			{
				eventBox.Remove<bool>(actionName);
			}
			TaiwuEvent listenerEvent = TaiwuEvent.Empty;
			bool flag = !string.IsNullOrEmpty(listenerEventGuid);
			if (flag)
			{
				TaiwuEvent targetEvent = this.GetEvent(listenerEventGuid);
				bool flag2 = targetEvent != null;
				if (flag2)
				{
					targetEvent.ArgBox = eventBox;
					listenerEvent = targetEvent;
				}
				else
				{
					listenerEvent = TaiwuEvent.Empty;
				}
			}
			this.ListeningEventActionList.Add(new ValueTuple<string, TaiwuEvent>(actionName, listenerEvent));
			this.SetHasListeningEvent(this.ListeningEventActionList.Count > 0, this.MainThreadDataContext);
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x0015A19C File Offset: 0x0015839C
		public void RemoveEventInListenWithActionName(string eventGuid, string actionName)
		{
			for (int i = this.ListeningEventActionList.Count - 1; i >= 0; i--)
			{
				bool flag = this.ListeningEventActionList[i].Item1.Equals(actionName) && this.ListeningEventActionList[i].Item2.EventGuid.Equals(eventGuid);
				if (flag)
				{
					this.ListeningEventActionList.RemoveAt(i);
				}
			}
			this.SetHasListeningEvent(this.ListeningEventActionList.Count > 0, this.MainThreadDataContext);
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0015A234 File Offset: 0x00158434
		[DomainMethod]
		public void TriggerListener(string key, bool value)
		{
			ValueTuple<string, TaiwuEvent> triggeredListeningEventAction = new ValueTuple<string, TaiwuEvent>(string.Empty, TaiwuEvent.Empty);
			for (int i = this.ListeningEventActionList.Count - 1; i >= 0; i--)
			{
				bool flag = this.ListeningEventActionList[i].Item1.Equals(key);
				if (flag)
				{
					triggeredListeningEventAction = this.ListeningEventActionList[i];
					this.ListeningEventActionList.RemoveAt(i);
					break;
				}
			}
			bool flag2 = string.IsNullOrEmpty(triggeredListeningEventAction.Item1) || triggeredListeningEventAction.Item2.IsEmpty;
			if (flag2)
			{
				this.SetHasListeningEvent(this.ListeningEventActionList.Count > 0, this.MainThreadDataContext);
				Events.RaiseEventHandleComplete(this.MainThreadDataContext);
			}
			else
			{
				bool flag3 = !string.IsNullOrEmpty(key);
				if (flag3)
				{
					triggeredListeningEventAction.Item2.ArgBox.Set(key, value);
				}
				this.SetHasListeningEvent(this.ListeningEventActionList.Count > 0, this.MainThreadDataContext);
				bool flag4 = triggeredListeningEventAction.Item2.EventConfig.CheckCondition();
				if (flag4)
				{
					this.ShowingEvent = triggeredListeningEventAction.Item2;
					this.UpdateEventDisplayData();
				}
				else
				{
					Events.RaiseEventHandleComplete(this.MainThreadDataContext);
				}
			}
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0015A370 File Offset: 0x00158570
		[DomainMethod]
		public void SetListenerEventActionISerializableArg(string actionName, string key, ISerializableGameData value)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				for (int i = 0; i < this.ListeningEventActionList.Count; i++)
				{
					bool flag2 = this.ListeningEventActionList[i].Item1.Equals(actionName);
					if (flag2)
					{
						TaiwuEvent item = this.ListeningEventActionList[i].Item2;
						bool flag3 = item != null && !item.IsEmpty;
						if (flag3)
						{
							this.ListeningEventActionList[i].Item2.ArgBox.Set(key, value);
						}
					}
				}
			}
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0015A410 File Offset: 0x00158610
		[DomainMethod]
		public void SetListenerEventActionIntListArg(string actionName, string key, IntList value)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				for (int i = 0; i < this.ListeningEventActionList.Count; i++)
				{
					bool flag2 = this.ListeningEventActionList[i].Item1.Equals(actionName);
					if (flag2)
					{
						TaiwuEvent item = this.ListeningEventActionList[i].Item2;
						bool flag3 = item != null && !item.IsEmpty;
						if (flag3)
						{
							this.ListeningEventActionList[i].Item2.ArgBox.Set(key, value);
						}
					}
				}
			}
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x0015A4B8 File Offset: 0x001586B8
		[DomainMethod]
		public void SetListenerEventActionShortListArg(string actionName, string key, GameData.Utilities.ShortList value)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				for (int i = 0; i < this.ListeningEventActionList.Count; i++)
				{
					bool flag2 = this.ListeningEventActionList[i].Item1.Equals(actionName);
					if (flag2)
					{
						TaiwuEvent item = this.ListeningEventActionList[i].Item2;
						bool flag3 = item != null && !item.IsEmpty;
						if (flag3)
						{
							this.ListeningEventActionList[i].Item2.ArgBox.Set(key, value);
						}
					}
				}
			}
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x0015A560 File Offset: 0x00158760
		[DomainMethod]
		public void SetListenerEventActionItemKeyArg(string actionName, string key, ItemKey value)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				for (int i = 0; i < this.ListeningEventActionList.Count; i++)
				{
					bool flag2 = this.ListeningEventActionList[i].Item1.Equals(actionName);
					if (flag2)
					{
						TaiwuEvent item = this.ListeningEventActionList[i].Item2;
						bool flag3 = item != null && !item.IsEmpty;
						if (flag3)
						{
							this.ListeningEventActionList[i].Item2.ArgBox.Set(key, value);
						}
					}
				}
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x0015A608 File Offset: 0x00158808
		[DomainMethod]
		public void SetListenerEventActionIntArg(string actionName, string key, int value)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				for (int i = 0; i < this.ListeningEventActionList.Count; i++)
				{
					bool flag2 = this.ListeningEventActionList[i].Item1.Equals(actionName);
					if (flag2)
					{
						TaiwuEvent item = this.ListeningEventActionList[i].Item2;
						bool flag3 = item != null && !item.IsEmpty;
						if (flag3)
						{
							this.ListeningEventActionList[i].Item2.ArgBox.Set(key, value);
						}
					}
				}
			}
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x0015A6A8 File Offset: 0x001588A8
		[DomainMethod]
		public void SetListenerEventActionBoolArg(string actionName, string key, bool value)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				for (int i = 0; i < this.ListeningEventActionList.Count; i++)
				{
					bool flag2 = this.ListeningEventActionList[i].Item1.Equals(actionName);
					if (flag2)
					{
						TaiwuEvent item = this.ListeningEventActionList[i].Item2;
						bool flag3 = item != null && !item.IsEmpty;
						if (flag3)
						{
							this.ListeningEventActionList[i].Item2.ArgBox.Set(key, value);
						}
					}
				}
			}
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x0015A748 File Offset: 0x00158948
		[DomainMethod]
		public void SetListenerEventActionStringArg(string actionName, string key, string value)
		{
			bool flag = string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value);
			if (!flag)
			{
				for (int i = 0; i < this.ListeningEventActionList.Count; i++)
				{
					bool flag2 = this.ListeningEventActionList[i].Item1.Equals(actionName);
					if (flag2)
					{
						TaiwuEvent item = this.ListeningEventActionList[i].Item2;
						bool flag3 = item != null && !item.IsEmpty;
						if (flag3)
						{
							this.ListeningEventActionList[i].Item2.ArgBox.Set(key, value);
						}
					}
				}
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600179B RID: 6043 RVA: 0x0015A7F3 File Offset: 0x001589F3
		// (set) Token: 0x0600179C RID: 6044 RVA: 0x0015A7FB File Offset: 0x001589FB
		[Obsolete]
		public string ListeningAction { get; private set; }

		// Token: 0x0600179D RID: 6045 RVA: 0x0015A804 File Offset: 0x00158A04
		[Obsolete]
		public void SetListener(string listenerEventGuid, EventArgBox eventBox)
		{
			bool flag = string.IsNullOrEmpty(listenerEventGuid);
			if (flag)
			{
				this._listenerEvent = TaiwuEvent.Empty;
			}
			else
			{
				TaiwuEvent targetEvent = this.GetEvent(listenerEventGuid);
				bool flag2 = targetEvent != null;
				if (flag2)
				{
					targetEvent.ArgBox = eventBox;
					this._listenerEvent = targetEvent;
				}
				else
				{
					this._listenerEvent = TaiwuEvent.Empty;
				}
			}
			TaiwuEvent listenerEvent = this._listenerEvent;
			this.SetHasListeningEvent(listenerEvent != null && !listenerEvent.IsEmpty, this.MainThreadDataContext);
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x0015A87E File Offset: 0x00158A7E
		[Obsolete]
		public void SetWaitActionName(string actionName)
		{
			this.ListeningAction = actionName;
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x0015A88C File Offset: 0x00158A8C
		[Obsolete]
		public void SetListenerISerializableArg(string key, ISerializableGameData value)
		{
			bool flag = this._listenerEvent != null && !this._listenerEvent.IsEmpty && !string.IsNullOrEmpty(key);
			if (flag)
			{
				this._listenerEvent.ArgBox.Set(key, value);
			}
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0015A8D4 File Offset: 0x00158AD4
		[Obsolete]
		public void SetListenerIntArg(string key, int value)
		{
			bool flag = this._listenerEvent != null && !this._listenerEvent.IsEmpty && !string.IsNullOrEmpty(key);
			if (flag)
			{
				this._listenerEvent.ArgBox.Set(key, value);
			}
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x0015A91C File Offset: 0x00158B1C
		[Obsolete]
		public void SetListenerFloatArg(string key, float value)
		{
			bool flag = this._listenerEvent != null && !this._listenerEvent.IsEmpty && !string.IsNullOrEmpty(key);
			if (flag)
			{
				this._listenerEvent.ArgBox.Set(key, value);
			}
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x0015A964 File Offset: 0x00158B64
		[Obsolete]
		public void SetListenerBoolArg(string key, bool value)
		{
			bool flag = this._listenerEvent != null && !this._listenerEvent.IsEmpty && !string.IsNullOrEmpty(key);
			if (flag)
			{
				this._listenerEvent.ArgBox.Set(key, value);
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x0015A9AC File Offset: 0x00158BAC
		[Obsolete]
		public void SetListenerStringArg(string key, string value)
		{
			bool flag = this._listenerEvent != null && !this._listenerEvent.IsEmpty && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value);
			if (flag)
			{
				this._listenerEvent.ArgBox.Set(key, value);
			}
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x0015A9FC File Offset: 0x00158BFC
		[Obsolete]
		public void SetListenerItemKeyArg(string key, ItemKey value)
		{
			TaiwuEvent listenerEvent = this._listenerEvent;
			bool flag = listenerEvent != null && !listenerEvent.IsEmpty && !string.IsNullOrEmpty(key);
			if (flag)
			{
				this._listenerEvent.ArgBox.Set(key, value);
			}
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x0015AA48 File Offset: 0x00158C48
		[Obsolete]
		public void SetListenerIntListArg(string key, IntList value)
		{
			TaiwuEvent listenerEvent = this._listenerEvent;
			bool flag = listenerEvent != null && !listenerEvent.IsEmpty && !string.IsNullOrEmpty(key);
			if (flag)
			{
				this._listenerEvent.ArgBox.Set(key, value);
			}
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x0015AA94 File Offset: 0x00158C94
		[DomainMethod]
		public EventLogData GetEventLogData()
		{
			EventLogData res = new EventLogData
			{
				CharacterList = this._characterCache.Values.ToList<CharacterDisplayData>(),
				SecretInformationList = this._secretInformationCache.Values.ToList<SecretInformationDisplayData>(),
				ItemList = this._itemCache.Values.ToList<ItemDisplayData>(),
				CombatSkillList = this._combatSkillCache.Values.ToList<CombatSkillDisplayData>()
			};
			foreach (List<EventLogResultData> resultList in this._eventLogQueue)
			{
				res.ResultList.AddRange(resultList);
			}
			return res;
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x0015AB58 File Offset: 0x00158D58
		[DomainMethod]
		public void StartNewDialog(DataContext context, IntPair charIds, string dialog, string rawResponseData, EventActorData leftActor, EventActorData rightActor, string leftName, string rightName, short merchantTemplateId)
		{
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			sbyte type = 0;
			bool flag = this.IsResultCacheValid();
			if (flag)
			{
				bool flag2 = this._eventLogQueue.Count > 99;
				if (flag2)
				{
					this.EventLogDequeue();
				}
				short adventureId = DomainManager.Adventure.GetCurAdventureId();
				bool flag3 = this._isCheckValid || this._isSequential || (adventureId >= 0 && this._adventureId >= 0);
				if (flag3)
				{
					this.CompareCharacterStatusRecord(taiwuId);
					bool flag4 = this._interactingCharacters.Item1 >= 0 && this._interactingCharacters.Item1 != taiwuId;
					if (flag4)
					{
						bool flag5 = this._interactingCharacters.Item1 == charIds.First;
						if (flag5)
						{
							this.CompareCharacterStatusRecord(this._interactingCharacters.Item1);
						}
						else
						{
							bool flag6 = this._npcStatus.ContainsKey(this._interactingCharacters.Item1);
							if (flag6)
							{
								this._npcStatus.Remove(this._interactingCharacters.Item1);
							}
						}
					}
					bool flag7 = this._interactingCharacters.Item2 >= 0 && this._interactingCharacters.Item2 != taiwuId && this._interactingCharacters.Item1 != this._interactingCharacters.Item2;
					if (flag7)
					{
						bool flag8 = this._interactingCharacters.Item2 == charIds.Second;
						if (flag8)
						{
							this.CompareCharacterStatusRecord(this._interactingCharacters.Item2);
						}
						else
						{
							bool flag9 = this._npcStatus.ContainsKey(this._interactingCharacters.Item2);
							if (flag9)
							{
								this._npcStatus.Remove(this._interactingCharacters.Item2);
							}
						}
					}
				}
				else
				{
					this._npcStatus.Clear();
				}
				this._adventureId = (int)adventureId;
				this.EventLogEnqueue();
			}
			else
			{
				this._resultCache = new List<EventLogResultData>();
			}
			bool flag10 = merchantTemplateId >= 0;
			if (flag10)
			{
				type = 1;
				rightActor = new EventActorData(merchantTemplateId);
			}
			this._resultCache.Add(new EventLogResultData
			{
				Type = type,
				ValueList = new List<int>
				{
					2,
					charIds.First,
					charIds.Second
				},
				Text = dialog,
				LeftActorData = leftActor,
				RightActorData = rightActor,
				LeftName = leftName,
				RightName = rightName
			});
			this._rawResponseData = rawResponseData;
			this._interactingCharacters = new ValueTuple<int, int>(charIds.First, charIds.Second);
			this._isSequential = false;
			this._isCheckValid = true;
			this.UpdateCharacterStatusRecord(taiwuId);
			bool flag11 = charIds.First >= 0 && charIds.First != taiwuId;
			if (flag11)
			{
				this.UpdateCharacterStatusRecord(charIds.First);
			}
			bool flag12 = charIds.Second >= 0 && charIds.Second != taiwuId;
			if (flag12)
			{
				this.UpdateCharacterStatusRecord(charIds.Second);
			}
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x0015AE50 File Offset: 0x00159050
		public void CheckTaiwuStatusImmediately()
		{
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			bool shouldCheckStatusImmediately = this._shouldCheckStatusImmediately;
			if (shouldCheckStatusImmediately)
			{
				this.CompareCharacterStatusRecord(taiwuId);
			}
			this._shouldCheckStatusImmediately = true;
			this.UpdateCharacterStatusRecord(taiwuId);
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x0015AE8A File Offset: 0x0015908A
		public void BlockEventLogStatusCheck()
		{
			this._isCheckValid = false;
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x0015AE94 File Offset: 0x00159094
		public void BlockEventLogImmediateStatusCheck()
		{
			this._shouldCheckStatusImmediately = false;
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x0015AE9E File Offset: 0x0015909E
		public void SetIsSequential(bool value)
		{
			this._isSequential = value;
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x0015AEA8 File Offset: 0x001590A8
		public void RecordCharacterEnterCombat()
		{
			this._taiwuStatus.Combat = new ValueTuple<sbyte, int>(8, -1);
			this.SetIsSequential(true);
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.CombatSettlementEventCallback));
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x0015AED7 File Offset: 0x001590D7
		public void RecordCharacterEnterLifeCombat()
		{
			this._taiwuStatus.Combat = new ValueTuple<sbyte, int>(9, -1);
			this.SetIsSequential(true);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0015AEF5 File Offset: 0x001590F5
		public void RecordCharacterEnterCricketCombat()
		{
			this._taiwuStatus.Combat = new ValueTuple<sbyte, int>(10, -1);
			this.SetIsSequential(true);
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x0015AF13 File Offset: 0x00159113
		public void RecordCombatResult(bool isTaiwuWin)
		{
			this._taiwuStatus.Combat = new ValueTuple<sbyte, int>(this._taiwuStatus.Combat.Item1, isTaiwuWin ? 1 : 0);
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x0015AF40 File Offset: 0x00159140
		public void UpdateEventLogCharacterDisplayData(int charId)
		{
			bool flag = this._characterCache.ContainsKey(charId);
			if (flag)
			{
				this._characterCache[charId] = DomainManager.Character.GetCharacterDisplayData(charId);
			}
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x0015AF78 File Offset: 0x00159178
		private void CombatSettlementEventCallback(DataContext context, sbyte combatStatus)
		{
			switch (combatStatus)
			{
			case 2:
			case 4:
				this.RecordCombatResult(false);
				break;
			case 3:
			case 5:
				this.RecordCombatResult(true);
				break;
			}
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.CombatSettlementEventCallback));
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x0015AFCA File Offset: 0x001591CA
		public void RecordCharacterRelationChanged(bool isRemove, int id1, int id2, ushort type)
		{
			this._taiwuStatus.Relation.Add(new ValueTuple<bool, int, int, ushort>(isRemove, id1, id2, type));
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x0015AFE8 File Offset: 0x001591E8
		public void RecordTeammateStateChanged(bool isLosing, int id)
		{
			this._taiwuStatus.Teammate = new ValueTuple<bool, int>(isLosing, id);
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0015B000 File Offset: 0x00159200
		public void RecordFavorabilityToTaiwuChanged(int id, short value)
		{
			EventLogCharacterData status;
			bool flag = this._npcStatus.TryGetValue(id, out status);
			if (flag)
			{
				status.FavorabilityToTaiwu = value;
			}
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x0015B028 File Offset: 0x00159228
		private void GenerateResponseLog(string optionKey, int charId)
		{
			bool flag = string.IsNullOrEmpty(optionKey);
			if (!flag)
			{
				bool flag2 = string.IsNullOrEmpty(this._rawResponseData);
				if (!flag2)
				{
					bool flag3 = charId < 0;
					if (flag3)
					{
						charId = DomainManager.Taiwu.GetTaiwuCharId();
					}
					string[] responses = this._rawResponseData.Split("<$new response dialog>", StringSplitOptions.None);
					foreach (string response in responses)
					{
						bool flag4 = string.IsNullOrEmpty(response);
						if (!flag4)
						{
							string[] data = response.Split("<$optionKey>", StringSplitOptions.None);
							bool flag5 = data[0] == optionKey;
							if (flag5)
							{
								this._resultCache.Insert(1, new EventLogResultData
								{
									Type = 2,
									ValueList = new List<int>
									{
										1,
										charId,
										Convert.ToInt32(data[2])
									},
									Text = data[1]
								});
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0015B124 File Offset: 0x00159324
		private void EventLogDequeue()
		{
			List<EventLogResultData> resultList = this._eventLogQueue.Dequeue();
			foreach (EventLogResultData result in resultList)
			{
				for (int i = 1; i < 1 + result.ValueList[0]; i++)
				{
					int character = result.ValueList[i];
					bool flag = character < 0;
					if (!flag)
					{
						Dictionary<int, int> characterReferences = this._characterReferences;
						int num = character;
						int num2 = characterReferences[num];
						characterReferences[num] = num2 - 1;
						bool flag2 = this._characterReferences[character] <= 0;
						if (flag2)
						{
							this._characterReferences.Remove(character);
							this._characterCache.Remove(character);
						}
					}
				}
				sbyte type = result.Type;
				sbyte b = type;
				if (b != 11)
				{
					if (b != 21)
					{
						if (b == 29)
						{
							List<int> valueList = result.ValueList;
							int metaDataId = valueList[valueList.Count - 1];
							Dictionary<int, int> secretInformationReferences = this._secretInformationReferences;
							int num2 = metaDataId;
							int num = secretInformationReferences[num2];
							secretInformationReferences[num2] = num - 1;
							bool flag3 = this._secretInformationReferences[metaDataId] <= 0;
							if (flag3)
							{
								this._secretInformationReferences.Remove(metaDataId);
								this._secretInformationCache.Remove(metaDataId);
							}
						}
					}
					else
					{
						int combatSkillId = result.ValueList[2];
						Dictionary<int, int> combatSkillReferences = this._combatSkillReferences;
						int num2 = combatSkillId;
						int num = combatSkillReferences[num2];
						combatSkillReferences[num2] = num - 1;
						bool flag4 = this._combatSkillReferences[combatSkillId] <= 0;
						if (flag4)
						{
							this._combatSkillReferences.Remove(combatSkillId);
							this._combatSkillCache.Remove(combatSkillId);
						}
					}
				}
				else
				{
					int itemId = result.ValueList[2];
					Dictionary<int, int> itemReferences = this._itemReferences;
					int num = itemId;
					int num2 = itemReferences[num];
					itemReferences[num] = num2 - 1;
					bool flag5 = this._itemReferences[itemId] <= 0;
					if (flag5)
					{
						this._itemReferences.Remove(itemId);
						this._itemCache.Remove(itemId);
					}
				}
			}
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0015B390 File Offset: 0x00159590
		private void EventLogEnqueue()
		{
			bool flag = this._resultCache.Count == 0;
			if (!flag)
			{
				foreach (EventLogResultData result in this._resultCache)
				{
					sbyte type = result.Type;
					sbyte b = type;
					if (b != 11)
					{
						if (b != 21)
						{
							if (b == 29)
							{
								List<int> valueList = result.ValueList;
								int metadataId = valueList[valueList.Count - 1];
								HashSet<int> characterList = new HashSet<int>();
								SecretInformationDisplayData displayData = DomainManager.Information.GetSecretInformationDisplayData(metadataId, characterList);
								result.ValueList.InsertRange(2, characterList);
								result.ValueList[0] = result.ValueList.Count - 2;
								bool flag2 = !this._secretInformationReferences.ContainsKey(metadataId);
								if (flag2)
								{
									this._secretInformationReferences[metadataId] = 0;
									this._secretInformationCache[metadataId] = displayData;
								}
								Dictionary<int, int> secretInformationReferences = this._secretInformationReferences;
								int num = metadataId;
								int num2 = secretInformationReferences[num];
								secretInformationReferences[num] = num2 + 1;
							}
						}
						else
						{
							int combatSkillId = result.ValueList[2];
							bool flag3 = !this._combatSkillReferences.ContainsKey(combatSkillId);
							if (flag3)
							{
								this._combatSkillReferences[combatSkillId] = 0;
								this._combatSkillCache[combatSkillId] = DomainManager.CombatSkill.GetCombatSkillDisplayDataOnce(result.ValueList[1], (short)result.ValueList[2]);
							}
							Dictionary<int, int> combatSkillReferences = this._combatSkillReferences;
							int num = combatSkillId;
							int num2 = combatSkillReferences[num];
							combatSkillReferences[num] = num2 + 1;
						}
					}
					else
					{
						int itemId = result.ValueList[2];
						bool flag4 = !this._itemReferences.ContainsKey(itemId);
						if (flag4)
						{
							ItemKey key = this._itemKeys[itemId].Item1;
							bool isTemporary = false;
							bool flag5 = !DomainManager.Item.ItemExists(key);
							if (flag5)
							{
								key = DomainManager.Item.CreateItem(DomainManager.TaiwuEvent.MainThreadDataContext, key.ItemType, key.TemplateId);
								isTemporary = true;
							}
							this._itemReferences[itemId] = 0;
							this._itemCache[itemId] = DomainManager.Item.GetItemDisplayData(key, result.ValueList[1]);
							bool flag6 = this._itemKeys[itemId].Item2 || isTemporary;
							if (flag6)
							{
								DomainManager.Item.RemoveItem(this.MainThreadDataContext, key);
							}
							this._itemKeys.Remove(itemId);
						}
						Dictionary<int, int> itemReferences = this._itemReferences;
						int num2 = itemId;
						int num = itemReferences[num2];
						itemReferences[num2] = num + 1;
					}
					for (int i = 1; i < 1 + result.ValueList[0]; i++)
					{
						int character = result.ValueList[i];
						bool flag7 = character < 0;
						if (!flag7)
						{
							bool flag8 = !this._characterReferences.ContainsKey(character);
							if (flag8)
							{
								this._characterReferences[character] = 0;
								this._characterCache[character] = DomainManager.Character.GetCharacterDisplayData(character);
							}
							Dictionary<int, int> characterReferences = this._characterReferences;
							int num2 = character;
							int num = characterReferences[num2];
							characterReferences[num2] = num + 1;
						}
					}
				}
				this._resultCache.Add(new EventLogResultData
				{
					Type = 30,
					ValueList = new List<int>
					{
						0
					}
				});
				this._eventLogQueue.Enqueue(this._resultCache);
				this._resultCache = new List<EventLogResultData>();
			}
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0015B774 File Offset: 0x00159974
		private bool IsResultCacheValid()
		{
			foreach (EventLogResultData res in this._resultCache)
			{
				bool flag = res.Type == 2;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x0015B7DC File Offset: 0x001599DC
		private unsafe void UpdateCharacterStatusRecord(int id)
		{
			GameData.Domains.Character.Character character;
			DomainManager.Character.TryGetElement_Objects(id, out character);
			bool flag = DomainManager.Taiwu.GetTaiwuCharId() == id;
			EventLogCharacterData status;
			if (flag)
			{
				status = this._taiwuStatus;
				status.SecretInformation.Clear();
				SecretInformationCharacterDataCollection collection;
				bool flag2 = DomainManager.Information.TryGetElement_CharacterSecretInformation(id, out collection);
				if (flag2)
				{
					foreach (SecretInformationCharacterData data in collection.Collection.Values)
					{
						status.SecretInformation.Add(data.SecretInformationMetaDataId);
					}
				}
				status.NormalInformation.Clear();
				NormalInformationCollection collection2;
				bool flag3 = DomainManager.Information.TryGetElement_Information(id, out collection2);
				if (flag3)
				{
					foreach (NormalInformation data2 in collection2.GetList())
					{
						status.NormalInformation.Add(data2);
					}
				}
				status.Combat = new ValueTuple<sbyte, int>(-1, -1);
				status.Relation.Clear();
				status.SpiritualDebt = DomainManager.Extra.GetAreaSpiritualDebt(this.GetTaiwuLocation().AreaId);
				status.Teammate = new ValueTuple<bool, int>(false, -1);
				status.Profession.Clear();
				foreach (ProfessionItem profession in ((IEnumerable<ProfessionItem>)Profession.Instance))
				{
					status.Profession[profession.TemplateId] = DomainManager.Extra.GetProfessionData(profession.TemplateId).Seniority;
				}
			}
			else
			{
				bool flag4 = !this._npcStatus.ContainsKey(id);
				if (flag4)
				{
					this._npcStatus.Add(id, new EventLogCharacterData(false));
				}
				status = this._npcStatus[id];
				status.FavorabilityToTaiwu = 0;
				SettlementCharacter settlementChar;
				status.ApprovedTaiwu = (int)((DomainManager.Organization.TryGetSettlementCharacter(id, out settlementChar) && DomainManager.Organization.GetSettlementByOrgTemplateId(settlementChar.GetOrgTemplateId()) != null) ? DomainManager.Organization.GetSettlementByOrgTemplateId(settlementChar.GetOrgTemplateId()).CalcApprovingRate() : 0);
			}
			status.Happiness = (int)character.GetHappiness();
			status.Fame = character.GetFame();
			status.Infection = character.GetXiangshuInfection();
			status.InfectionStatus = XiangshuInfectionTypeHelper.GetInfectionFeatureIdThatShouldBe(status.Infection);
			status.Item.Clear();
			foreach (KeyValuePair<ItemKey, int> pair in character.GetInventory().Items)
			{
				status.Item[pair.Key] = pair.Value;
			}
			for (sbyte i = 0; i < 8; i += 1)
			{
				status.Resource[(int)i] = character.GetResource(i);
			}
			status.Exp = character.GetExp();
			status.Health = character.GetHealth();
			status.MainAttribute = character.GetCurrMainAttributes();
			status.Injury = character.GetInjuries();
			status.Poison = *character.GetPoisoned();
			status.DisorderOfQi = character.GetDisorderOfQi();
			status.CombatSkills.Clear();
			status.CombatSkills.AddRange(character.GetLearnedCombatSkills());
			status.LifeSkills.Clear();
			status.LifeSkills.AddRange(character.GetLearnedLifeSkills());
			status.Feature.Clear();
			status.Feature.AddRange(character.GetFeatureIds());
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x0015BB9C File Offset: 0x00159D9C
		private void CompareCharacterStatusRecord(int id)
		{
			GameData.Domains.Character.Character character;
			DomainManager.Character.TryGetElement_Objects(id, out character);
			bool flag = DomainManager.Taiwu.GetTaiwuCharId() == id;
			if (flag)
			{
				this.EventLogCheckCombatResult(this._taiwuStatus, character);
				this.EventLogCheckHappiness(this._taiwuStatus, character);
				this.EventLogCheckFame(this._taiwuStatus, character);
				this.EventLogCheckInfection(this._taiwuStatus, character);
				this.EventLogCheckItem(this._taiwuStatus, character);
				this.EventLogCheckResource(this._taiwuStatus, character);
				this.EventLogCheckExp(this._taiwuStatus, character);
				this.EventLogCheckHealth(this._taiwuStatus, character);
				this.EventLogCheckMainAttribute(this._taiwuStatus, character);
				this.EventLogCheckInjury(this._taiwuStatus, character);
				this.EventLogCheckPoison(this._taiwuStatus, character);
				this.EventLogCheckDisorderOfQi(this._taiwuStatus, character);
				this.EventLogCheckCombatSkills(this._taiwuStatus, character);
				this.EventLogCheckLifeSkills(this._taiwuStatus, character);
				this.EventLogCheckRelation(this._taiwuStatus, character);
				this.EventLogCheckFeature(this._taiwuStatus, character);
				this.EventLogCheckSecretInformation(this._taiwuStatus, character);
				this.EventLogCheckNormalInformation(this._taiwuStatus, character);
				this.EventLogCheckSpiritualDebt(this._taiwuStatus, character);
				this.EventLogCheckTeammate(this._taiwuStatus, character);
				this.EventLogCheckProfession(this._taiwuStatus, character);
			}
			else
			{
				EventLogCharacterData status;
				this._npcStatus.TryGetValue(id, out status);
				this.EventLogCheckHappiness(status, character);
				this.EventLogCheckFame(status, character);
				this.EventLogCheckInfection(status, character);
				this.EventLogCheckExp(status, character);
				this.EventLogCheckHealth(status, character);
				this.EventLogCheckMainAttribute(status, character);
				this.EventLogCheckInjury(status, character);
				this.EventLogCheckPoison(status, character);
				this.EventLogCheckDisorderOfQi(status, character);
				this.EventLogCheckCombatSkills(status, character);
				this.EventLogCheckLifeSkills(status, character);
				this.EventLogCheckFeature(status, character);
				this.EventLogCheckFavorabilityToTaiwu(status, character);
				this.EventLogCheckApprovedTaiwu(status, character);
			}
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x0015BD88 File Offset: 0x00159F88
		private void AddDifferenceToResultCache<T>(sbyte type, int charId, bool isLosing, ICollection<T> origList, ICollection<T> newList, bool elementIsCharacter = false, int? extraAdding = null) where T : struct
		{
			foreach (T newItem in newList)
			{
				bool flag = origList.Contains(newItem);
				if (!flag)
				{
					List<EventLogResultData> resultCache = this._resultCache;
					EventLogResultData eventLogResultData = new EventLogResultData();
					eventLogResultData.Type = type;
					eventLogResultData.IsLosing = isLosing;
					EventLogResultData eventLogResultData2 = eventLogResultData;
					List<int> list = new List<int>();
					list.Add(elementIsCharacter ? 2 : 1);
					list.Add(charId);
					List<int> list2 = list;
					if (!true)
					{
					}
					int item11;
					if (newItem is sbyte)
					{
						sbyte item = newItem as sbyte;
						item11 = (int)item;
					}
					else if (newItem is byte)
					{
						byte item2 = newItem as byte;
						item11 = (int)item2;
					}
					else if (newItem is short)
					{
						short item3 = newItem as short;
						item11 = (int)item3;
					}
					else if (newItem is ushort)
					{
						ushort item4 = newItem as ushort;
						item11 = (int)item4;
					}
					else if (newItem is int)
					{
						int item5 = newItem as int;
						item11 = item5;
					}
					else if (newItem is uint)
					{
						uint item6 = newItem as uint;
						item11 = (int)item6;
					}
					else if (newItem is long)
					{
						long item7 = newItem as long;
						item11 = (int)item7;
					}
					else if (newItem is ulong)
					{
						ulong item8 = newItem as ulong;
						item11 = (int)item8;
					}
					else if (newItem is GameData.Domains.Character.LifeSkillItem)
					{
						GameData.Domains.Character.LifeSkillItem item9 = newItem as GameData.Domains.Character.LifeSkillItem;
						item11 = (int)item9.SkillTemplateId;
					}
					else
					{
						if (!(newItem is NormalInformation))
						{
							throw new Exception("Fatal error. Event Log Result Value List doesn't support this type yet.");
						}
						NormalInformation item10 = newItem as NormalInformation;
						item11 = this.AddNormalInformationToValueList(item10, out extraAdding);
					}
					if (!true)
					{
					}
					list2.Add(item11);
					eventLogResultData2.ValueList = list;
					resultCache.Add(eventLogResultData);
					bool flag2 = extraAdding != null;
					if (flag2)
					{
						List<EventLogResultData> resultCache2 = this._resultCache;
						resultCache2[resultCache2.Count - 1].ValueList.Add(extraAdding.Value);
					}
				}
			}
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x0015C058 File Offset: 0x0015A258
		private void AddDifferenceToResultCache(sbyte type, int charId, int origValue, int newValue, int? extraAdding = null)
		{
			bool flag = origValue == newValue;
			if (!flag)
			{
				this._resultCache.Add(new EventLogResultData
				{
					Type = type,
					ValueList = new List<int>
					{
						1,
						charId,
						origValue,
						newValue
					}
				});
				bool flag2 = extraAdding != null;
				if (flag2)
				{
					List<EventLogResultData> resultCache = this._resultCache;
					resultCache[resultCache.Count - 1].ValueList.Add(extraAdding.Value);
				}
			}
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x0015C0E8 File Offset: 0x0015A2E8
		private void AddDifferenceToResultCache(int charId, bool isLosing, Dictionary<ItemKey, int> origInventory, Dictionary<ItemKey, int> newInventory)
		{
			foreach (KeyValuePair<ItemKey, int> keyValuePair in newInventory)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				int newCount = num;
				int origCount;
				int delta = origInventory.TryGetValue(itemKey, out origCount) ? (newCount - origCount) : newCount;
				bool flag = delta <= 0;
				if (!flag)
				{
					ItemKey key = itemKey;
					bool isTemporary = false;
					bool flag2 = key.Id == -1 || !DomainManager.Item.ItemExists(key);
					if (flag2)
					{
						key = DomainManager.Item.CreateItem(DomainManager.TaiwuEvent.MainThreadDataContext, itemKey.ItemType, itemKey.TemplateId);
						isTemporary = true;
					}
					this._itemKeys[key.Id] = new ValueTuple<ItemKey, bool>(key, isTemporary);
					this._resultCache.Add(new EventLogResultData
					{
						Type = 11,
						IsLosing = isLosing,
						ValueList = new List<int>
						{
							1,
							charId,
							key.Id,
							delta
						}
					});
				}
			}
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x0015C240 File Offset: 0x0015A440
		private int AddNormalInformationToValueList(NormalInformation item, out int? level)
		{
			level = new int?((int)item.Level);
			return (int)item.TemplateId;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0015C26C File Offset: 0x0015A46C
		private Location GetTaiwuLocation()
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location location = taiwu.GetLocation();
			return location.IsValid() ? location : taiwu.GetValidLocation();
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0015C2A4 File Offset: 0x0015A4A4
		private void EventLogCheckHappiness(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			this.AddDifferenceToResultCache(3, character.GetId(), status.Happiness, (int)character.GetHappiness(), null);
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x0015C2D8 File Offset: 0x0015A4D8
		private void EventLogCheckFame(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			this.AddDifferenceToResultCache(4, character.GetId(), (int)status.Fame, (int)character.GetFame(), null);
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0015C30C File Offset: 0x0015A50C
		private void EventLogCheckInfection(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			byte infection = character.GetXiangshuInfection();
			this.AddDifferenceToResultCache(6, character.GetId(), (int)status.Infection, (int)infection, null);
			this.AddDifferenceToResultCache(7, character.GetId(), (int)status.InfectionStatus, (int)XiangshuInfectionTypeHelper.GetInfectionFeatureIdThatShouldBe(infection), null);
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x0015C364 File Offset: 0x0015A564
		private void EventLogCheckItem(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			Dictionary<ItemKey, int> items = character.GetInventory().Items;
			int charId = character.GetId();
			this.AddDifferenceToResultCache(charId, false, status.Item, items);
			this.AddDifferenceToResultCache(charId, true, items, status.Item);
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0015C3A8 File Offset: 0x0015A5A8
		private void EventLogCheckResource(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			for (sbyte i = 0; i < 8; i += 1)
			{
				this.AddDifferenceToResultCache(12, character.GetId(), status.Resource[(int)i], character.GetResource(i), new int?((int)i));
			}
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0015C3F0 File Offset: 0x0015A5F0
		private void EventLogCheckExp(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			this.AddDifferenceToResultCache(27, character.GetId(), status.Exp, character.GetExp(), null);
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x0015C424 File Offset: 0x0015A624
		private void EventLogCheckHealth(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			this.AddDifferenceToResultCache(15, character.GetId(), (int)status.Health, (int)character.GetHealth(), null);
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x0015C458 File Offset: 0x0015A658
		private unsafe void EventLogCheckMainAttribute(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			MainAttributes delta = character.GetCurrMainAttributes().Subtract(status.MainAttribute);
			bool flag = delta.GetSum() == 0;
			if (!flag)
			{
				for (sbyte i = 0; i < 6; i += 1)
				{
					bool flag2 = *(ref delta.Items.FixedElementField + (IntPtr)i * 2) != 0;
					if (flag2)
					{
						this._resultCache.Add(new EventLogResultData
						{
							Type = 16,
							ValueList = new List<int>
							{
								1,
								character.GetId(),
								(int)(*(ref status.MainAttribute.Items.FixedElementField + (IntPtr)i * 2)),
								(int)(*(ref delta.Items.FixedElementField + (IntPtr)i * 2)),
								(int)i
							}
						});
					}
				}
			}
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0015C53C File Offset: 0x0015A73C
		private void EventLogCheckInjury(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			Injuries delta = character.GetInjuries().Subtract(status.Injury);
			bool flag = delta.GetSum() == 0;
			if (!flag)
			{
				for (sbyte i = 0; i < 7; i += 1)
				{
					sbyte val = delta.Get(i, true);
					bool flag2 = val != 0;
					if (flag2)
					{
						this._resultCache.Add(new EventLogResultData
						{
							Type = 17,
							ValueList = new List<int>
							{
								1,
								character.GetId(),
								(int)status.Injury.Get(i, true),
								(int)val,
								(int)i
							}
						});
					}
					sbyte val2 = delta.Get(i, false);
					bool flag3 = val2 != 0;
					if (flag3)
					{
						this._resultCache.Add(new EventLogResultData
						{
							Type = 18,
							ValueList = new List<int>
							{
								1,
								character.GetId(),
								(int)status.Injury.Get(i, false),
								(int)val2,
								(int)i
							}
						});
					}
				}
			}
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0015C67C File Offset: 0x0015A87C
		private unsafe void EventLogCheckPoison(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			PoisonInts delta = character.GetPoisoned().Subtract(ref status.Poison);
			bool flag = delta.Sum() == 0;
			if (!flag)
			{
				for (sbyte i = 0; i < 6; i += 1)
				{
					bool flag2 = *(ref delta.Items.FixedElementField + (IntPtr)i * 4) != 0;
					if (flag2)
					{
						this._resultCache.Add(new EventLogResultData
						{
							Type = 19,
							ValueList = new List<int>
							{
								1,
								character.GetId(),
								*(ref status.Poison.Items.FixedElementField + (IntPtr)i * 4),
								*(ref delta.Items.FixedElementField + (IntPtr)i * 4),
								(int)i
							}
						});
					}
				}
			}
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x0015C75C File Offset: 0x0015A95C
		private void EventLogCheckDisorderOfQi(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			this.AddDifferenceToResultCache(20, character.GetId(), (int)status.DisorderOfQi, (int)character.GetDisorderOfQi(), null);
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0015C790 File Offset: 0x0015A990
		private void EventLogCheckCombatSkills(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			this.AddDifferenceToResultCache<short>(21, character.GetId(), false, status.CombatSkills, character.GetLearnedCombatSkills(), false, null);
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0015C7C4 File Offset: 0x0015A9C4
		private void EventLogCheckLifeSkills(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			this.AddDifferenceToResultCache<GameData.Domains.Character.LifeSkillItem>(22, character.GetId(), false, status.LifeSkills, character.GetLearnedLifeSkills(), false, null);
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x0015C7F8 File Offset: 0x0015A9F8
		private void EventLogCheckFeature(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			List<short> features = character.GetFeatureIds();
			this.AddDifferenceToResultCache<short>(25, character.GetId(), false, status.Feature, features, false, null);
			this.AddDifferenceToResultCache<short>(25, character.GetId(), true, features, status.Feature, false, null);
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x0015C850 File Offset: 0x0015AA50
		private void EventLogCheckSecretInformation(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			SecretInformationCharacterDataCollection collection;
			bool flag = !DomainManager.Information.TryGetElement_CharacterSecretInformation(character.GetId(), out collection);
			if (!flag)
			{
				this.AddDifferenceToResultCache<int>(29, character.GetId(), false, status.SecretInformation, (from info in collection.Collection.Values
				select info.SecretInformationMetaDataId).ToList<int>(), false, null);
			}
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x0015C8D0 File Offset: 0x0015AAD0
		private void EventLogCheckNormalInformation(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			NormalInformationCollection collection;
			bool flag = !DomainManager.Information.TryGetElement_Information(character.GetId(), out collection);
			if (!flag)
			{
				this.AddDifferenceToResultCache<NormalInformation>(28, character.GetId(), false, status.NormalInformation, collection.GetList().ToList<NormalInformation>(), false, null);
			}
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x0015C924 File Offset: 0x0015AB24
		private void EventLogCheckCombatResult(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			bool flag = status.Combat.Item1 == -1 || status.Combat.Item2 < 0;
			if (!flag)
			{
				this._resultCache.Add(new EventLogResultData
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

		// Token: 0x060017D1 RID: 6097 RVA: 0x0015C9A0 File Offset: 0x0015ABA0
		private void EventLogCheckRelation(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			foreach (ValueTuple<bool, int, int, ushort> relation in status.Relation)
			{
				int oneWay = 0;
				bool flag = RelationType.IsOneWayRelation(relation.Item4);
				if (flag)
				{
					bool flag2 = DomainManager.Character.HasRelation(relation.Item2, relation.Item3, relation.Item4) && !DomainManager.Character.HasRelation(relation.Item3, relation.Item2, relation.Item4);
					if (flag2)
					{
						oneWay = 1;
					}
					else
					{
						bool flag3 = !DomainManager.Character.HasRelation(relation.Item2, relation.Item3, relation.Item4) && DomainManager.Character.HasRelation(relation.Item3, relation.Item2, relation.Item4);
						if (flag3)
						{
							oneWay = 2;
						}
					}
				}
				this._resultCache.Add(new EventLogResultData
				{
					Type = 24,
					IsLosing = relation.Item1,
					ValueList = new List<int>
					{
						2,
						relation.Item2,
						relation.Item3,
						(int)RelationType.GetTypeId(relation.Item4),
						oneWay
					}
				});
			}
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x0015CB14 File Offset: 0x0015AD14
		private void EventLogCheckSpiritualDebt(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			short areaId = this.GetTaiwuLocation().AreaId;
			this.AddDifferenceToResultCache(13, character.GetId(), status.SpiritualDebt, DomainManager.Extra.GetAreaSpiritualDebt(areaId), new int?((int)areaId));
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x0015CB54 File Offset: 0x0015AD54
		private void EventLogCheckTeammate(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			bool flag = status.Teammate.Item2 >= 0;
			if (flag)
			{
				this._resultCache.Add(new EventLogResultData
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

		// Token: 0x060017D4 RID: 6100 RVA: 0x0015CBD8 File Offset: 0x0015ADD8
		private void EventLogCheckProfession(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			foreach (ProfessionItem profession in ((IEnumerable<ProfessionItem>)Profession.Instance))
			{
				int templateId = profession.TemplateId;
				bool flag = !status.Profession.ContainsKey(templateId);
				if (!flag)
				{
					this.AddDifferenceToResultCache(26, character.GetId(), status.Profession[templateId], DomainManager.Extra.GetProfessionData(templateId).Seniority, new int?(templateId));
				}
			}
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x0015CC70 File Offset: 0x0015AE70
		private void EventLogCheckFavorabilityToTaiwu(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			bool flag = status.FavorabilityToTaiwu != 0;
			if (flag)
			{
				this._resultCache.Add(new EventLogResultData
				{
					Type = 5,
					ValueList = new List<int>
					{
						1,
						character.GetId(),
						(int)status.FavorabilityToTaiwu
					}
				});
			}
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x0015CCD4 File Offset: 0x0015AED4
		private void EventLogCheckApprovedTaiwu(EventLogCharacterData status, GameData.Domains.Character.Character character)
		{
			int id = character.GetId();
			SettlementCharacter settlementChar;
			bool flag = DomainManager.Organization.TryGetSettlementCharacter(id, out settlementChar);
			if (flag)
			{
				Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(settlementChar.GetOrgTemplateId());
				bool flag2 = settlement != null;
				if (flag2)
				{
					this.AddDifferenceToResultCache(23, id, status.ApprovedTaiwu, (int)settlement.CalcApprovingRate(), new int?((int)settlementChar.GetOrgTemplateId()));
				}
			}
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0015CD3A File Offset: 0x0015AF3A
		[DomainMethod]
		public void GmCmd_SaveMonthlyActionManager(DataContext context)
		{
			this.SetMonthlyEventActionManager(this._monthlyEventActionManager, context);
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x0015CD4B File Offset: 0x0015AF4B
		[DomainMethod]
		public void GmCmd_TaiwuCrossArchive()
		{
			DomainManager.TaiwuEvent.OnEvent_TaiwuCrossArchive();
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x0015CD5C File Offset: 0x0015AF5C
		[DomainMethod]
		public void GmCmd_TaiwuWantedSectPunished(DataContext context, sbyte orgTemplateId, sbyte severity)
		{
			EventArgBox argBox = new EventArgBox();
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId);
			bool flag = settlement == null;
			if (!flag)
			{
				Sect sect = settlement as Sect;
				bool flag2 = sect == null;
				if (!flag2)
				{
					GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
					sect.AddBounty(context, taiwu, severity, 0, -1);
					argBox.Set("SettlementId", settlement.GetId());
					EventHelper.TaiwuWantedSectPunished(argBox, false);
				}
			}
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x0015CDD0 File Offset: 0x0015AFD0
		[DomainMethod]
		public List<short> GetValidInteractionEventOptions(int targetCharId)
		{
			List<short> optionList = new List<short>();
			GameData.Domains.Character.Character targetChar = DomainManager.Character.GetElement_Objects(targetCharId);
			foreach (InteractionEventOptionItem optionCfg in ((IEnumerable<InteractionEventOptionItem>)InteractionEventOption.Instance))
			{
				bool flag = this.CheckInteractionEventOption(optionCfg, targetChar);
				if (flag)
				{
					optionList.Add(optionCfg.TemplateId);
				}
			}
			return optionList;
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x0015CE50 File Offset: 0x0015B050
		public bool CheckInteractionEventOption(InteractionEventOptionItem optionCfg, GameData.Domains.Character.Character targetChar)
		{
			int targetCharId = targetChar.GetId();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool flag = optionCfg.ProfessionSkill >= 0;
			if (flag)
			{
				ProfessionSkillItem skillCfg = ProfessionSkill.Instance[optionCfg.ProfessionSkill];
				int index = ProfessionSkillHandle.GetSkillIndex(skillCfg);
				bool flag2 = !DomainManager.Extra.CanExecuteProfessionSkill(skillCfg.Profession, index);
				if (flag2)
				{
					return false;
				}
			}
			bool flag3 = DomainManager.Extra.GetActionPointCurrMonth() < optionCfg.ActionPointCost;
			bool result;
			if (flag3)
			{
				result = false;
			}
			else
			{
				short settlementId = targetChar.GetOrganizationInfo().SettlementId;
				bool flag4 = settlementId >= 0;
				if (flag4)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
					Location location = settlement.GetLocation();
					bool flag5 = DomainManager.Extra.GetAreaSpiritualDebt(location.AreaId) < optionCfg.SpiritualDebtCost;
					if (flag5)
					{
						return false;
					}
				}
				ResourceInts resourceCost = optionCfg.ResourceCost;
				bool flag6 = taiwuChar.GetResources().CheckIsMeet(ref resourceCost);
				if (flag6)
				{
					result = false;
				}
				else
				{
					MainAttributes mainAttributeCost = optionCfg.MainAttributeCost;
					bool flag7 = taiwuChar.GetCurrMainAttributes().CheckIsMeet(ref mainAttributeCost);
					result = !flag7;
				}
			}
			return result;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x0015CF8E File Offset: 0x0015B18E
		public void SetInteractionEventOptionCooldown(int targetCharId, short optionTemplateId)
		{
			this._executedOncePerMonthOptions.Add(new IntPair(targetCharId, (int)optionTemplateId));
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0015CFA4 File Offset: 0x0015B1A4
		public bool IsInteractionEventOptionOffCooldown(int targetCharId, short optionTemplateId)
		{
			return !this._executedOncePerMonthOptions.Contains(new IntPair(targetCharId, (int)optionTemplateId));
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0015CFCC File Offset: 0x0015B1CC
		[DomainMethod]
		public void InitConchShipEvents()
		{
			string globalScriptsFolderPath = Path.Combine("..", "Event/EventLib/GlobalScriptCompiled");
			TaiwuEventDomain._scriptRuntime.LoadGlobalScripts(globalScriptsFolderPath);
			TaiwuEventDomain._packagesList = new List<EventPackage>();
			EventPackagePathInfo pathInfo = new EventPackagePathInfo("../Event");
			string useLoadFromPath = Path.Combine("..", "use_load_from.txt");
			string useLoadFilePath = Path.Combine("..", "use_load_file.txt");
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
				if (eventManagerBase != null)
				{
					eventManagerBase.Reset();
				}
			}
			TaiwuEventDomain._loadMethod = (File.Exists(useLoadFromPath) ? TaiwuEventDomain.EEventPackageLoadMethod.LoadFrom : (File.Exists(useLoadFilePath) ? TaiwuEventDomain.EEventPackageLoadMethod.LoadFile : TaiwuEventDomain.EEventPackageLoadMethod.LoadBuffer));
			foreach (string dllFilePath in Directory.GetFiles(pathInfo.DllDirPath, "*.dll", SearchOption.AllDirectories))
			{
				string packageName = Path.GetFileNameWithoutExtension(dllFilePath);
				this.LoadEventPackageFromAssembly(packageName, pathInfo, "ConchShip", null);
			}
			AdaptableLog.Info("ConchShip events init complete,can load mod events now");
			DlcManager.LoadAllEventPackages();
			DomainManager.Mod.LoadAllEventPackages();
			this.InitCharacterInteractionEventOptionConfigList();
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0015D0EC File Offset: 0x0015B2EC
		[DomainMethod]
		public void LoadEventsFromPath(string eventDataDirectory)
		{
			bool flag = !Directory.Exists(eventDataDirectory);
			if (flag)
			{
				throw new Exception("Directory " + eventDataDirectory + " does not exist");
			}
			EventPackagePathInfo pathInfo = new EventPackagePathInfo(eventDataDirectory);
			string[] dllFiles = Directory.GetFiles(eventDataDirectory, "*.dll", SearchOption.AllDirectories);
			string parentDirectory = new DirectoryInfo(eventDataDirectory).Parent.FullName;
			int packageCount = 0;
			foreach (string dllFilePath in dllFiles)
			{
				string packageName = Path.GetFileNameWithoutExtension(dllFilePath);
				this.LoadEventPackageFromAssembly(packageName, pathInfo, parentDirectory, null);
				packageCount++;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
			defaultInterpolatedStringHandler.AppendLiteral("load events from ");
			defaultInterpolatedStringHandler.AppendFormatted(eventDataDirectory);
			defaultInterpolatedStringHandler.AppendLiteral(" complete,");
			defaultInterpolatedStringHandler.AppendFormatted<int>(packageCount);
			defaultInterpolatedStringHandler.AppendLiteral(" packages loaded!");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0015D1D0 File Offset: 0x0015B3D0
		public void LoadEventPackageFromAssembly(string packageName, EventPackagePathInfo pathInfo, string modIdString, string dllFilePath = null)
		{
			bool flag = string.IsNullOrEmpty(dllFilePath);
			if (flag)
			{
				dllFilePath = Path.Combine(pathInfo.DllDirPath, packageName + ".dll");
			}
			Assembly assembly = this.LoadEventPackageAssembly(dllFilePath);
			EventPackage package = this.CreateEventPackageObject(assembly);
			bool flag2 = package == null;
			if (flag2)
			{
				TaiwuEventDomain.Logger.AppendWarning("Failed to load event package at " + dllFilePath + ".");
			}
			else
			{
				bool flag3 = TaiwuEventDomain._packagesList.Contains(package);
				if (flag3)
				{
					for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
					{
						EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
						if (eventManagerBase != null)
						{
							eventManagerBase.UnloadPackage(package);
						}
					}
				}
				package.SetModIdString(modIdString);
				string packagePath = Path.Combine(pathInfo.ScriptDirPath, packageName + ".twes");
				try
				{
					TaiwuEventDomain._scriptRuntime.LoadPackageScripts(package, packagePath);
				}
				catch (Exception e)
				{
					Logger logger = TaiwuEventDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to load event package at ");
					defaultInterpolatedStringHandler.AppendFormatted(packagePath);
					defaultInterpolatedStringHandler.AppendLiteral(".\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
					logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
					return;
				}
				string language = "CN";
				string languageFilePath = Path.Combine(pathInfo.LanguageDirPath, packageName + "_Language_" + language + ".txt");
				package.InitLanguage(languageFilePath);
				for (int j = 0; j < TaiwuEventDomain._managerArray.Length; j++)
				{
					EventManagerBase eventManagerBase2 = TaiwuEventDomain._managerArray[j];
					if (eventManagerBase2 != null)
					{
						eventManagerBase2.HandleEventPackage(package);
					}
				}
				TaiwuEventDomain._packagesList.Add(package);
			}
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0015D380 File Offset: 0x0015B580
		private Assembly LoadEventPackageAssembly(string path)
		{
			TaiwuEventDomain.<>c__DisplayClass212_0 CS$<>8__locals1;
			CS$<>8__locals1.path = path;
			TaiwuEventDomain.EEventPackageLoadMethod loadMethod = TaiwuEventDomain._loadMethod;
			if (!true)
			{
			}
			Assembly result;
			switch (loadMethod)
			{
			case TaiwuEventDomain.EEventPackageLoadMethod.LoadFile:
				result = Assembly.LoadFile(CS$<>8__locals1.path);
				break;
			case TaiwuEventDomain.EEventPackageLoadMethod.LoadFrom:
				result = Assembly.LoadFrom(CS$<>8__locals1.path);
				break;
			case TaiwuEventDomain.EEventPackageLoadMethod.LoadBuffer:
				result = TaiwuEventDomain.<LoadEventPackageAssembly>g__LoadBinaryWithPdb|212_0(ref CS$<>8__locals1);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to load assembly with undefined method ");
				defaultInterpolatedStringHandler.AppendFormatted<TaiwuEventDomain.EEventPackageLoadMethod>(TaiwuEventDomain._loadMethod);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0015D424 File Offset: 0x0015B624
		private EventPackage CreateEventPackageObject(Assembly assembly)
		{
			Type[] types = assembly.GetExportedTypes();
			Type baseType = typeof(EventPackage);
			foreach (Type type in types)
			{
				bool flag = type.IsSubclassOf(baseType);
				if (flag)
				{
					try
					{
						return Activator.CreateInstance(type) as EventPackage;
					}
					catch (Exception e)
					{
						Logger logger = TaiwuEventDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Failed to load event package ");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
						return null;
					}
				}
			}
			return null;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0015D4DC File Offset: 0x0015B6DC
		public List<TaiwuEventItem> GetAllEventConfigs()
		{
			List<TaiwuEventItem> allEventConfigList = new List<TaiwuEventItem>();
			TaiwuEventDomain._packagesList.ForEach(delegate(EventPackage e)
			{
				allEventConfigList.AddRange(e.GetAllEvents());
			});
			return allEventConfigList;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0015D51C File Offset: 0x0015B71C
		private void InitCharacterInteractionEventOptionConfigList()
		{
			TaiwuEventDomain._characterInteractionEventOptionList.Clear();
			List<short> keys = InteractionEventOption.Instance.GetAllKeys();
			foreach (EventPackage package in TaiwuEventDomain._packagesList)
			{
				List<TaiwuEventItem> allEvents = package.GetAllEvents();
				bool flag = keys.Count == 0;
				if (flag)
				{
					break;
				}
				foreach (TaiwuEventItem taiwuEventItem in allEvents)
				{
					bool flag2 = keys.Count == 0;
					if (flag2)
					{
						break;
					}
					TaiwuEventOption[] eventOptions = taiwuEventItem.EventOptions;
					for (int i = 0; i < eventOptions.Length; i++)
					{
						TaiwuEventOption taiwuEventOption = eventOptions[i];
						bool flag3 = keys.Count == 0;
						if (flag3)
						{
							break;
						}
						int findIndex = keys.FindIndex(delegate(short key)
						{
							InteractionEventOptionItem config = InteractionEventOption.Instance[key];
							return config.OptionGuid == taiwuEventOption.OptionGuid;
						});
						bool flag4 = findIndex >= 0;
						if (flag4)
						{
							TaiwuEventDomain._characterInteractionEventOptionList.Add(new ValueTuple<TaiwuEventOption, short, TaiwuEventItem>(taiwuEventOption, keys[findIndex], taiwuEventItem));
							keys.RemoveAt(findIndex);
						}
					}
				}
			}
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0015D6A8 File Offset: 0x0015B8A8
		[return: TupleElementNames(new string[]
		{
			"dict",
			"NoInteractionReason"
		})]
		public ValueTuple<Dictionary<short, bool>, int> GetVisibleCharacterInteractionEventOptions(int charId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			string guid = EventHelper.GetCharacterClickedSpecialNextEvent(character, new EventArgBox(), false);
			bool flag = !string.IsNullOrEmpty(guid);
			ValueTuple<Dictionary<short, bool>, int> result2;
			if (flag)
			{
				result2 = new ValueTuple<Dictionary<short, bool>, int>(null, 2);
			}
			else
			{
				RelatedCharacter relatedCharacter;
				bool flag2 = !DomainManager.Character.TryGetRelation(charId, DomainManager.Taiwu.GetTaiwuCharId(), out relatedCharacter);
				if (flag2)
				{
					result2 = new ValueTuple<Dictionary<short, bool>, int>(null, 1);
				}
				else
				{
					Dictionary<short, bool> result = new Dictionary<short, bool>();
					EventArgBox eventArgBox = this._argBoxPool.Get();
					eventArgBox.Set("RoleTaiwu", EventArgBox.TaiwuCharacterId);
					eventArgBox.Set("CharacterId", charId);
					foreach (ValueTuple<TaiwuEventOption, short, TaiwuEventItem> valueTuple in TaiwuEventDomain._characterInteractionEventOptionList)
					{
						TaiwuEventOption taiwuEventOption = valueTuple.Item1;
						short templateId = valueTuple.Item2;
						TaiwuEventItem taiwuEventItem = valueTuple.Item3;
						InteractionEventOptionItem config = InteractionEventOption.Instance[templateId];
						bool flag3 = config.InteractionType == EInteractionEventOptionInteractionType.Invalid;
						if (!flag3)
						{
							EventArgBox prevEventBox = taiwuEventItem.ArgBox;
							EventArgBox prevOptionBox = taiwuEventOption.ArgBox;
							taiwuEventItem.ArgBox = eventArgBox;
							taiwuEventOption.ArgBox = eventArgBox;
							bool isVisible = taiwuEventOption.IsVisible;
							if (isVisible)
							{
								result.Add(templateId, taiwuEventOption.IsAvailable);
							}
							taiwuEventItem.ArgBox = prevEventBox;
							taiwuEventOption.ArgBox = prevOptionBox;
						}
					}
					this._argBoxPool.Return(eventArgBox);
					result2 = new ValueTuple<Dictionary<short, bool>, int>(result, 0);
				}
			}
			return result2;
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060017E6 RID: 6118 RVA: 0x0015D834 File Offset: 0x0015BA34
		// (set) Token: 0x060017E7 RID: 6119 RVA: 0x0015D83B File Offset: 0x0015BA3B
		[Obsolete]
		public static bool IsQuickStartGame { get; private set; }

		// Token: 0x060017E8 RID: 6120 RVA: 0x0015D844 File Offset: 0x0015BA44
		public EventArgBox GetGlobalEventArgumentBox()
		{
			return this.GetGlobalArgBox();
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0015D85C File Offset: 0x0015BA5C
		public void ClearGlobalEventArgumentBox()
		{
			this._globalArgBox.Clear();
			this.SetGlobalArgBox(this._globalArgBox, this.MainThreadDataContext);
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x0015D87E File Offset: 0x0015BA7E
		public void SaveGlobalEventArgumentBox()
		{
			this.SetGlobalArgBox(this._globalArgBox, this.MainThreadDataContext);
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x0015D894 File Offset: 0x0015BA94
		public void SaveArgToGlobalArgBox<T>(string key, T value)
		{
			this._globalArgBox.GenericSet<T>(key, value);
			this.SetGlobalArgBox(this._globalArgBox, this.MainThreadDataContext);
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0015D8B8 File Offset: 0x0015BAB8
		public void ActivateNextSwordTomb()
		{
			EventHelper.SetNextSwordTombCountDownDate();
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0015D8C4 File Offset: 0x0015BAC4
		public unsafe void XiangshuMinionSurroundTaiwuVillage()
		{
			bool flag = !EventHelper.GlobalArgBoxContainsKey<bool>("TrySurroundTaiwuVillage");
			if (!flag)
			{
				EventHelper.MakeAreaGraduallyBrokenInCondition(EventArgBox.TaiwuVillageAreaId, delegate(MapBlockData blockData)
				{
					bool flag5 = blockData.CharacterSet == null;
					bool result;
					if (flag5)
					{
						result = false;
					}
					else
					{
						bool flag6 = blockData.CharacterSet.Count > 0;
						if (flag6)
						{
							int killCount2 = Math.Min(3, blockData.CharacterSet.Count);
							List<int> charIdList = blockData.CharacterSet.ToList<int>();
							CollectionUtils.Shuffle<int>(this.MainThreadDataContext.Random, charIdList);
							for (int l = 0; l < killCount2; l++)
							{
								GameData.Domains.Character.Character character2;
								bool flag7 = DomainManager.Character.TryGetElement_Objects(charIdList[l], out character2);
								if (flag7)
								{
									DomainManager.Character.MakeCharacterDead(this.MainThreadDataContext, character2, 10);
								}
							}
						}
						HashSet<int> characterSet = blockData.CharacterSet;
						result = (characterSet != null && characterSet.Count > 0);
					}
					return result;
				}, new Dictionary<short, byte>());
				MonthlyNotificationCollection notificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				List<int> areaCharIdList = new List<int>();
				for (short i = 0; i < 45; i += 1)
				{
					areaCharIdList.Clear();
					Span<MapBlockData> areaBlockCollection = DomainManager.Map.GetAreaBlocks(i);
					Span<MapBlockData> span = areaBlockCollection;
					for (int k = 0; k < span.Length; k++)
					{
						MapBlockData blockData2 = *span[k];
						bool flag2 = blockData2.CharacterSet != null;
						if (flag2)
						{
							areaCharIdList.AddRange(blockData2.CharacterSet);
						}
					}
					bool flag3 = areaCharIdList.Count <= 0;
					if (!flag3)
					{
						int killCount = (int)Math.Max(1f, (float)areaCharIdList.Count * 0.2f);
						CollectionUtils.Shuffle<int>(this.MainThreadDataContext.Random, areaCharIdList);
						for (int j = 0; j < killCount; j++)
						{
							GameData.Domains.Character.Character character;
							bool flag4 = DomainManager.Character.TryGetElement_Objects(areaCharIdList[j], out character) && character.GetCreatingType() > 0;
							if (flag4)
							{
								DomainManager.Character.MakeCharacterDead(this.MainThreadDataContext, character, 10);
							}
						}
						Location location = new Location(i, -1);
						notificationCollection.AddXiangshuKilling(location, killCount);
					}
				}
				EventHelper.EnsureTaiwuVillagerLocationForSpiritualWanderPlace();
			}
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x0015DA40 File Offset: 0x0015BC40
		[DomainMethod]
		public void SetIsQuickStartGame(bool flag)
		{
			EventArgBox globalArgBox = DomainManager.TaiwuEvent.GetGlobalArgBox();
			globalArgBox.Set("CS_PK_IsQuickStartGame", flag);
			DomainManager.TaiwuEvent.SaveGlobalEventArgumentBox();
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0015DA71 File Offset: 0x0015BC71
		public void CollectUnreleasedCalledCharacters(HashSet<int> calledCharacters)
		{
			this._monthlyEventActionManager.CollectUnreleasedCalledCharacters(calledCharacters);
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x0015DA84 File Offset: 0x0015BC84
		[DomainMethod]
		public ValueTuple<int, int> GetMonthlyActionStateAndTime(MonthlyActionKey key)
		{
			MonthlyActionBase action = this._monthlyEventActionManager.GetMonthlyAction(key);
			bool flag = action == null;
			ValueTuple<int, int> result;
			if (flag)
			{
				result = new ValueTuple<int, int>(0, 0);
			}
			else
			{
				result = new ValueTuple<int, int>((int)action.State, action.Month);
			}
			return result;
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0015DAC6 File Offset: 0x0015BCC6
		public void ResetAllConfigMonthlyActions()
		{
			this._monthlyEventActionManager.Init();
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x0015DAD8 File Offset: 0x0015BCD8
		public MonthlyActionBase GetMonthlyAction(MonthlyActionKey key)
		{
			return this._monthlyEventActionManager.GetMonthlyAction(key);
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0015DAF8 File Offset: 0x0015BCF8
		public MonthlyActionKey AddTempDynamicAction<T>(DataContext context, T action) where T : MonthlyActionBase, IDynamicAction
		{
			MonthlyActionKey key = this._monthlyEventActionManager.AddTempDynamicAction<T>(action);
			action.TriggerAction();
			this.SetMonthlyEventActionManager(this._monthlyEventActionManager, context);
			return key;
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x0015DB34 File Offset: 0x0015BD34
		public MonthlyActionKey AddWrappedConfigAction(DataContext context, short templateId, short assignedAreaId = -1)
		{
			MonthlyActionKey key = this._monthlyEventActionManager.AddWrappedConfigAction(templateId, assignedAreaId);
			this.SetMonthlyEventActionManager(this._monthlyEventActionManager, context);
			return key;
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x0015DB63 File Offset: 0x0015BD63
		public void RemoveTempDynamicAction(DataContext context, MonthlyActionKey key)
		{
			this._monthlyEventActionManager.RemoveTempDynamicAction(key);
			this.SetMonthlyEventActionManager(this._monthlyEventActionManager, context);
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x0015DB81 File Offset: 0x0015BD81
		public void ClearTaiwuBindingMonthlyActions(DataContext context)
		{
			this._monthlyEventActionManager.ClearTaiwuBindingMonthlyActions();
			this.SetMonthlyEventActionManager(this._monthlyEventActionManager, context);
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0015DBA0 File Offset: 0x0015BDA0
		public void HandleMonthlyAction(DataContext context, MonthlyActionKey key)
		{
			MonthlyActionBase monthlyAction = this._monthlyEventActionManager.GetMonthlyAction(key);
			monthlyAction.MonthlyHandler();
			this.SetMonthlyEventActionManager(this._monthlyEventActionManager, context);
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0015DBD0 File Offset: 0x0015BDD0
		public void HandleMonthlyActions()
		{
			this._monthlyEventActionManager.HandleMonthlyActions();
			this.SetMonthlyEventActionManager(this._monthlyEventActionManager, this.MainThreadDataContext);
			bool flag = EventHelper.GlobalArgBoxContainsKey<int>("ConchShip_PresetKey_DateOfNextSwordTombActivate");
			if (flag)
			{
				int date = EventHelper.GetIntFromGlobalArgBox("ConchShip_PresetKey_DateOfNextSwordTombActivate");
				bool flag2 = DomainManager.World.GetCurrDate() >= date;
				if (flag2)
				{
					EventHelper.RemoveFromGlobalArgBox<int>("ConchShip_PresetKey_DateOfNextSwordTombActivate");
					EventHelper.StartNextSwordTombCountDown();
				}
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060017F9 RID: 6137 RVA: 0x0015DC3F File Offset: 0x0015BE3F
		public EventScriptRuntime ScriptRuntime
		{
			get
			{
				return TaiwuEventDomain._scriptRuntime;
			}
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x0015DC48 File Offset: 0x0015BE48
		public bool WasTemporaryOptionSelected(string guid)
		{
			return this._selectedTemporaryOptions.Contains(guid);
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x0015DC68 File Offset: 0x0015BE68
		public void SetTemporaryOptionSelected(string guid, bool selected)
		{
			if (selected)
			{
				this._selectedTemporaryOptions.Add(guid);
			}
			else
			{
				this._selectedTemporaryOptions.Remove(guid);
			}
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x0015DC97 File Offset: 0x0015BE97
		[DomainMethod]
		public void EventScriptExecuteNext()
		{
			TaiwuEventDomain._scriptRuntime.MovingNext = true;
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x0015DCA5 File Offset: 0x0015BEA5
		[DomainMethod]
		public void SetEventScriptExecutionPause(bool isPaused)
		{
			TaiwuEventDomain._scriptRuntime.IsPaused = isPaused;
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x0015DCB4 File Offset: 0x0015BEB4
		[DomainMethod]
		public void OnCharacterClicked(DataContext context, int charId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			short characterTemplateId = character.GetTemplateId();
			bool flag = GameData.Domains.Character.Character.IsXiangshuMinion(characterTemplateId);
			if (!flag)
			{
				bool flag2 = XiangshuAvatarIds.JuniorXiangshuTemplateIds.Contains(characterTemplateId);
				if (flag2)
				{
					sbyte avatarId = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(characterTemplateId);
					this.OnEvent_PurpleBambooAvatarClicked(charId, avatarId);
				}
				else
				{
					bool flag3 = character.GetCreatingType() == 0;
					if (flag3)
					{
						this.OnEvent_FixedCharacterClicked(charId, characterTemplateId);
					}
					else
					{
						bool flag4 = character.GetCreatingType() != 3 || DomainManager.Extra.IsSpecialGroupMember(character);
						if (flag4)
						{
							this.OnEvent_CharacterClicked(charId);
						}
						else
						{
							bool flag5 = character.GetCreatingType() == 3 && SectMainStoryRelatedConstants.SectMainStoryCharacterTemplateIds.Contains(characterTemplateId);
							if (flag5)
							{
								this.OnEvent_FixedEnemyClicked(charId, characterTemplateId);
							}
						}
					}
				}
			}
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x0015DD74 File Offset: 0x0015BF74
		[DomainMethod]
		public void OnCharacterTemplateClicked(DataContext context, short characterTemplateId)
		{
			this.OnEvent_CharacterTemplateClicked(characterTemplateId);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x0015DD7F File Offset: 0x0015BF7F
		[DomainMethod]
		public void OnLetTeammateLeaveGroup(DataContext context, int charId)
		{
			this.OnEvent_LetTeammateLeaveGroup(charId);
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x0015DD8A File Offset: 0x0015BF8A
		[DomainMethod]
		public void OnInteractCaravan(int caravanId)
		{
			this.OnEvent_CaravanClicked(caravanId);
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x0015DD95 File Offset: 0x0015BF95
		[DomainMethod]
		public void OnInteractKidnappedCharacter(int charId)
		{
			this.OnEvent_KidnappedCharacterClicked(charId);
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0015DDA0 File Offset: 0x0015BFA0
		[DomainMethod]
		public void OnSectBuildingClicked(short buildingTemplateId)
		{
			this.OnEvent_SectBuildingClicked(buildingTemplateId);
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x0015DDAC File Offset: 0x0015BFAC
		[DomainMethod]
		public void OnRecordEnterGame(short mainStoryLineProgress)
		{
			bool inGuiding = DomainManager.TutorialChapter.InGuiding;
			if (!inGuiding)
			{
				this.OnEvent_RecordEnterGame(mainStoryLineProgress);
			}
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x0015DDD2 File Offset: 0x0015BFD2
		[DomainMethod]
		public void OnNewGameMonth()
		{
			this.OnEvent_NewGameMonth();
			DomainManager.Building.TriggerBuildingCompleteEvents(this.MainThreadDataContext);
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x0015DDED File Offset: 0x0015BFED
		[DomainMethod]
		public void OnCombatWithXiangshuMinionComplete(short templateId)
		{
			this.OnEvent_CombatWithXiangshuMinionComplete(templateId);
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x0015DDF8 File Offset: 0x0015BFF8
		[DomainMethod]
		public void OnBlackMaskAnimationComplete(bool maskVisible)
		{
			this.OnEvent_BlackMaskAnimationComplete(maskVisible);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0015DE03 File Offset: 0x0015C003
		[DomainMethod]
		public void OnMakingSystemOpened(BuildingBlockKey blockKey, short templateId)
		{
			this.OnEvent_MakingSystemOpened(blockKey, templateId);
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0015DE0F File Offset: 0x0015C00F
		[DomainMethod]
		public void OnCollectedMakingSystemItem(BuildingBlockKey blockKey, short templateId, bool showingGetItem)
		{
			this.OnEvent_CollectedMakingSystemItem(blockKey, templateId, showingGetItem);
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x0015DE1C File Offset: 0x0015C01C
		[DomainMethod]
		public void OnSectSpecialBuildingClicked(short templateId)
		{
			this.OnEvent_OnSectSpecialBuildingClicked(templateId);
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0015DE27 File Offset: 0x0015C027
		[DomainMethod]
		public void AnimalAvatarClicked(int animalId)
		{
			this.OnEvent_AnimalAvatarClicked(animalId);
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0015DE32 File Offset: 0x0015C032
		[DomainMethod]
		public void MainStoryFinishCatchCricket(bool result)
		{
			this.OnEvent_MainStoryFinishCatchCricket(result);
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0015DE3D File Offset: 0x0015C03D
		[DomainMethod]
		public void NpcTombClicked(int tombId)
		{
			this.OnEvent_NpcTombClicked(tombId);
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0015DE48 File Offset: 0x0015C048
		[DomainMethod]
		public void OnLifeSkillCombatForceSilent(int charId, sbyte concessionCount, sbyte inducementCount)
		{
			this.OnEvent_LifeSkillCombatForceSilent(charId, concessionCount, inducementCount);
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x0015DE55 File Offset: 0x0015C055
		[DomainMethod]
		public void TryMoveWhenMoveDisable()
		{
			this.OnEvent_TryMoveWhenMoveDisabled();
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x0015DE5F File Offset: 0x0015C05F
		[DomainMethod]
		public void TryMoveToInvalidLocationInTutorial()
		{
			this.OnEvent_TryMoveToInvalidLocationInTutorial();
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x0015DE69 File Offset: 0x0015C069
		[DomainMethod]
		public void CloseUI(string uiName, bool presetBool = false, int presetInt = -1)
		{
			DomainManager.TaiwuEvent.OnEvent_CloseUI(uiName, presetBool, presetInt);
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x0015DE7A File Offset: 0x0015C07A
		[DomainMethod]
		public void TaiwuCollectWudangHeavenlyTreeSeed(sbyte resourceType)
		{
			DomainManager.TaiwuEvent.OnEvent_TaiwuCollectWudangHeavenlyTreeSeed(resourceType);
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x0015DE89 File Offset: 0x0015C089
		[DomainMethod]
		public void TaiwuVillagerExpelled(int charId)
		{
			DomainManager.TaiwuEvent.OnEvent_TaiwuVillagerExpelled(charId);
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x0015DE98 File Offset: 0x0015C098
		[DomainMethod]
		public void TaiwuCrossArchiveFindMemory(sbyte type)
		{
			DomainManager.TaiwuEvent.OnEvent_TaiwuCrossArchiveFindMemory(type);
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x0015DEA7 File Offset: 0x0015C0A7
		[DomainMethod]
		public void UserLoadDreamBackArchive()
		{
			DomainManager.TaiwuEvent.OnEvent_UserLoadDreamBackArchive();
			this.OnEvent_RecordEnterGame(DomainManager.World.GetMainStoryLineProgress());
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x0015DEC6 File Offset: 0x0015C0C6
		[DomainMethod]
		public void OperateInventoryItem(int charId, sbyte operationType, ItemDisplayData itemData)
		{
			DomainManager.TaiwuEvent.OnEvent_OperateInventoryItem(charId, operationType, itemData);
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x0015DED7 File Offset: 0x0015C0D7
		[DomainMethod]
		public void SettlementTreasuryBuildingClicked(short templateId, byte currStatus, sbyte currPage)
		{
			DomainManager.TaiwuEvent.OnEvent_OnSettlementTreasuryBuildingClicked(templateId, currStatus, currPage);
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x0015DEE8 File Offset: 0x0015C0E8
		[DomainMethod]
		public void TriggerShixiangDrumEasterEgg()
		{
			DomainManager.TaiwuEvent.OnEvent_OnShixiangDrumClickedManyTimes();
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x0015DEF6 File Offset: 0x0015C0F6
		[DomainMethod]
		public void OnClickedPrisonBtn(short buildingTemplateId)
		{
			DomainManager.TaiwuEvent.OnEvent_OnClickedPrisonBtn(buildingTemplateId);
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0015DF05 File Offset: 0x0015C105
		[DomainMethod]
		public void OnClickedSendPrisonBtn()
		{
			DomainManager.TaiwuEvent.OnEvent_OnClickedSendPrisonBtn();
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x0015DF13 File Offset: 0x0015C113
		[DomainMethod]
		public void InteractPrisoner(int characterId, int interactPrisonerType)
		{
			DomainManager.TaiwuEvent.OnEvent_InteractPrisoner(characterId, interactPrisonerType);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x0015DF23 File Offset: 0x0015C123
		[DomainMethod]
		public void OnClickMapPickupEvent(Location location)
		{
			DomainManager.TaiwuEvent.OnEvent_TriggerMapPickupEvent(location, true);
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0015DF34 File Offset: 0x0015C134
		[DomainMethod]
		public void OnClickMapPickupNormalEvent(Location location)
		{
			bool tempDisableTriggerNormalPickupByTaiwuEscape = DomainManager.Map.TempDisableTriggerNormalPickupByTaiwuEscape;
			if (!tempDisableTriggerNormalPickupByTaiwuEscape)
			{
				DomainManager.TaiwuEvent.OnEvent_TriggerMapPickupEvent(location, false);
			}
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0015DF5F File Offset: 0x0015C15F
		[DomainMethod]
		public void OnClickDeportButton(int type, bool isGood)
		{
			DomainManager.TaiwuEvent.OnEvent_TaiwuDeportVitals(type, isGood);
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0015DF6F File Offset: 0x0015C16F
		[DomainMethod]
		public void OnSwitchToGuardedPage(byte currStatus, sbyte currPage)
		{
			this.OnEvent_SwitchToGuardedPage(currStatus, currPage);
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0015DF7C File Offset: 0x0015C17C
		public void AddJieqingMaskCharId(int charId)
		{
			bool flag = this._jieqingMaskCharIdList == null;
			if (flag)
			{
				this._jieqingMaskCharIdList = new List<int>();
			}
			bool flag2 = !this._jieqingMaskCharIdList.Contains(charId);
			if (flag2)
			{
				this._jieqingMaskCharIdList.Add(charId);
				this.SetJieqingMaskCharIdList(this._jieqingMaskCharIdList, this.MainThreadDataContext);
				GameData.Domains.Character.Character character;
				bool flag3 = DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag3)
				{
					character.SetAvatar(character.GetAvatar(), this.MainThreadDataContext);
				}
			}
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x0015E000 File Offset: 0x0015C200
		public void RemoveJieqingMaskCharId(int charId)
		{
			bool flag = this._jieqingMaskCharIdList != null && this._jieqingMaskCharIdList.Contains(charId);
			if (flag)
			{
				this._jieqingMaskCharIdList.Remove(charId);
				this.SetJieqingMaskCharIdList(this._jieqingMaskCharIdList, this.MainThreadDataContext);
				GameData.Domains.Character.Character character;
				bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag2)
				{
					character.SetAvatar(character.GetAvatar(), this.MainThreadDataContext);
				}
			}
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0015E071 File Offset: 0x0015C271
		[DomainMethod]
		public void GmCmd_AddJieqingMaskCharId(int charId)
		{
			this.AddJieqingMaskCharId(charId);
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x0015E07C File Offset: 0x0015C27C
		[DomainMethod]
		public void GmCmd_RemoveJieqingMaskCharId(int charId)
		{
			this.RemoveJieqingMaskCharId(charId);
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x0015E088 File Offset: 0x0015C288
		public TaiwuEventDomain() : base(29)
		{
			this._globalArgBox = new EventArgBox();
			this._monthlyEventActionManager = new MonthlyEventActionsManager();
			this._awayForeverLoverCharId = 0;
			this._eventCount = 0;
			this._healDoctorCharId = 0;
			this._cgName = string.Empty;
			this._notifyData = new EventNotifyData();
			this._hasListeningEvent = false;
			this._selectInformationData = new EventSelectInformationData();
			this._taiwuLocationChangeFlag = false;
			this._secretVillageOnFire = false;
			this._taiwuVillageShowShrine = false;
			this._hideAllTeammates = false;
			this._leftRoleAlternativeName = string.Empty;
			this._rightRoleAlternativeName = string.Empty;
			this._rightRoleXiangshuDisplayData = new sbyte[2];
			this._selectCombatSkillData = new EventSelectCombatSkillData();
			this._selectLifeSkillData = new EventSelectLifeSkillData();
			this._itemListOfLeft = new ItemDisplayData[3];
			this._itemListOfRight = new ItemDisplayData[3];
			this._showItemWithCricketBattleGuess = false;
			this._displayingEventData = new TaiwuEventDisplayData();
			this._tempCreateItemList = new List<ItemKey>();
			this._coverCricketJarGradeListForRight = new List<sbyte>();
			this._marriageLook1CharIdList = new List<int>();
			this._marriageLook2CharIdList = new List<int>();
			this._allCombatGroupChars = new int[3];
			this._cricketBettingData = new EventCricketBettingData();
			this._jieqingMaskCharIdList = new List<int>();
			this.OnInitializedDomainData();
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x0015E2BC File Offset: 0x0015C4BC
		private EventArgBox GetGlobalArgBox()
		{
			return this._globalArgBox;
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0015E2D4 File Offset: 0x0015C4D4
		private unsafe void SetGlobalArgBox(EventArgBox value, DataContext context)
		{
			this._globalArgBox = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
			int dataSize = this._globalArgBox.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValue_Set(12, 0, dataSize);
			pData += this._globalArgBox.Serialize(pData);
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0015E324 File Offset: 0x0015C524
		private MonthlyEventActionsManager GetMonthlyEventActionManager()
		{
			return this._monthlyEventActionManager;
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x0015E33C File Offset: 0x0015C53C
		private unsafe void SetMonthlyEventActionManager(MonthlyEventActionsManager value, DataContext context)
		{
			this._monthlyEventActionManager = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
			int dataSize = this._monthlyEventActionManager.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValue_Set(12, 1, dataSize);
			pData += this._monthlyEventActionManager.Serialize(pData);
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x0015E38C File Offset: 0x0015C58C
		[Obsolete("DomainData _awayForeverLoverCharId is no longer in use.")]
		public int GetAwayForeverLoverCharId()
		{
			return this._awayForeverLoverCharId;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0015E3A4 File Offset: 0x0015C5A4
		[Obsolete("DomainData _awayForeverLoverCharId is no longer in use.")]
		public unsafe void SetAwayForeverLoverCharId(int value, DataContext context)
		{
			this._awayForeverLoverCharId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(12, 2, 4);
			*(int*)pData = this._awayForeverLoverCharId;
			pData += 4;
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0015E3E4 File Offset: 0x0015C5E4
		[Obsolete("DomainData _eventCount is no longer in use.")]
		public ushort GetEventCount()
		{
			return this._eventCount;
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0015E3FC File Offset: 0x0015C5FC
		[Obsolete("DomainData _eventCount is no longer in use.")]
		private void SetEventCount(ushort value, DataContext context)
		{
			this._eventCount = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x0015E41C File Offset: 0x0015C61C
		[Obsolete("DomainData _healDoctorCharId is no longer in use.")]
		public int GetHealDoctorCharId()
		{
			return this._healDoctorCharId;
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x0015E434 File Offset: 0x0015C634
		[Obsolete("DomainData _healDoctorCharId is no longer in use.")]
		public void SetHealDoctorCharId(int value, DataContext context)
		{
			this._healDoctorCharId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0015E454 File Offset: 0x0015C654
		public string GetCgName()
		{
			return this._cgName;
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0015E46C File Offset: 0x0015C66C
		public void SetCgName(string value, DataContext context)
		{
			this._cgName = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0015E48C File Offset: 0x0015C68C
		public EventNotifyData GetNotifyData()
		{
			return this._notifyData;
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0015E4A4 File Offset: 0x0015C6A4
		private void SetNotifyData(EventNotifyData value, DataContext context)
		{
			this._notifyData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0015E4C4 File Offset: 0x0015C6C4
		public bool GetHasListeningEvent()
		{
			return this._hasListeningEvent;
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0015E4DC File Offset: 0x0015C6DC
		private void SetHasListeningEvent(bool value, DataContext context)
		{
			this._hasListeningEvent = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0015E4FC File Offset: 0x0015C6FC
		public EventSelectInformationData GetSelectInformationData()
		{
			return this._selectInformationData;
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x0015E514 File Offset: 0x0015C714
		public void SetSelectInformationData(EventSelectInformationData value, DataContext context)
		{
			this._selectInformationData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x0015E534 File Offset: 0x0015C734
		public bool GetTaiwuLocationChangeFlag()
		{
			return this._taiwuLocationChangeFlag;
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x0015E54C File Offset: 0x0015C74C
		public void SetTaiwuLocationChangeFlag(bool value, DataContext context)
		{
			this._taiwuLocationChangeFlag = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x0015E56C File Offset: 0x0015C76C
		public bool GetSecretVillageOnFire()
		{
			return this._secretVillageOnFire;
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x0015E584 File Offset: 0x0015C784
		public unsafe void SetSecretVillageOnFire(bool value, DataContext context)
		{
			this._secretVillageOnFire = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(12, 10, 1);
			*pData = (this._secretVillageOnFire ? 1 : 0);
			pData++;
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x0015E5C4 File Offset: 0x0015C7C4
		public bool GetTaiwuVillageShowShrine()
		{
			return this._taiwuVillageShowShrine;
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0015E5DC File Offset: 0x0015C7DC
		public unsafe void SetTaiwuVillageShowShrine(bool value, DataContext context)
		{
			this._taiwuVillageShowShrine = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(12, 11, 1);
			*pData = (this._taiwuVillageShowShrine ? 1 : 0);
			pData++;
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x0015E61C File Offset: 0x0015C81C
		public bool GetHideAllTeammates()
		{
			return this._hideAllTeammates;
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0015E634 File Offset: 0x0015C834
		public void SetHideAllTeammates(bool value, DataContext context)
		{
			this._hideAllTeammates = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x0015E654 File Offset: 0x0015C854
		public string GetLeftRoleAlternativeName()
		{
			return this._leftRoleAlternativeName;
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0015E66C File Offset: 0x0015C86C
		public void SetLeftRoleAlternativeName(string value, DataContext context)
		{
			this._leftRoleAlternativeName = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x0015E68C File Offset: 0x0015C88C
		public string GetRightRoleAlternativeName()
		{
			return this._rightRoleAlternativeName;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0015E6A4 File Offset: 0x0015C8A4
		public void SetRightRoleAlternativeName(string value, DataContext context)
		{
			this._rightRoleAlternativeName = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0015E6C4 File Offset: 0x0015C8C4
		public sbyte[] GetRightRoleXiangshuDisplayData()
		{
			return this._rightRoleXiangshuDisplayData;
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x0015E6DC File Offset: 0x0015C8DC
		public void SetRightRoleXiangshuDisplayData(sbyte[] value, DataContext context)
		{
			this._rightRoleXiangshuDisplayData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x0015E6FC File Offset: 0x0015C8FC
		public EventSelectCombatSkillData GetSelectCombatSkillData()
		{
			return this._selectCombatSkillData;
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x0015E714 File Offset: 0x0015C914
		public void SetSelectCombatSkillData(EventSelectCombatSkillData value, DataContext context)
		{
			this._selectCombatSkillData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0015E734 File Offset: 0x0015C934
		public EventSelectLifeSkillData GetSelectLifeSkillData()
		{
			return this._selectLifeSkillData;
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x0015E74C File Offset: 0x0015C94C
		public void SetSelectLifeSkillData(EventSelectLifeSkillData value, DataContext context)
		{
			this._selectLifeSkillData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0015E76C File Offset: 0x0015C96C
		public ItemDisplayData[] GetItemListOfLeft()
		{
			return this._itemListOfLeft;
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x0015E784 File Offset: 0x0015C984
		public void SetItemListOfLeft(ItemDisplayData[] value, DataContext context)
		{
			this._itemListOfLeft = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x0015E7A4 File Offset: 0x0015C9A4
		public ItemDisplayData[] GetItemListOfRight()
		{
			return this._itemListOfRight;
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x0015E7BC File Offset: 0x0015C9BC
		public void SetItemListOfRight(ItemDisplayData[] value, DataContext context)
		{
			this._itemListOfRight = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x0015E7DC File Offset: 0x0015C9DC
		public bool GetShowItemWithCricketBattleGuess()
		{
			return this._showItemWithCricketBattleGuess;
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x0015E7F4 File Offset: 0x0015C9F4
		public void SetShowItemWithCricketBattleGuess(bool value, DataContext context)
		{
			this._showItemWithCricketBattleGuess = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0015E814 File Offset: 0x0015CA14
		public TaiwuEventDisplayData GetDisplayingEventData()
		{
			return this._displayingEventData;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0015E82C File Offset: 0x0015CA2C
		public void SetDisplayingEventData(TaiwuEventDisplayData value, DataContext context)
		{
			this._displayingEventData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0015E84C File Offset: 0x0015CA4C
		public List<ItemKey> GetTempCreateItemList()
		{
			return this._tempCreateItemList;
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0015E864 File Offset: 0x0015CA64
		public void SetTempCreateItemList(List<ItemKey> value, DataContext context)
		{
			this._tempCreateItemList = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x0015E884 File Offset: 0x0015CA84
		public List<sbyte> GetCoverCricketJarGradeListForRight()
		{
			return this._coverCricketJarGradeListForRight;
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x0015E89C File Offset: 0x0015CA9C
		public void SetCoverCricketJarGradeListForRight(List<sbyte> value, DataContext context)
		{
			this._coverCricketJarGradeListForRight = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x0015E8BC File Offset: 0x0015CABC
		public List<int> GetMarriageLook1CharIdList()
		{
			return this._marriageLook1CharIdList;
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x0015E8D4 File Offset: 0x0015CAD4
		public void SetMarriageLook1CharIdList(List<int> value, DataContext context)
		{
			this._marriageLook1CharIdList = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x0015E8F4 File Offset: 0x0015CAF4
		public List<int> GetMarriageLook2CharIdList()
		{
			return this._marriageLook2CharIdList;
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x0015E90C File Offset: 0x0015CB0C
		public void SetMarriageLook2CharIdList(List<int> value, DataContext context)
		{
			this._marriageLook2CharIdList = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x0015E92C File Offset: 0x0015CB2C
		public int[] GetAllCombatGroupChars()
		{
			return this._allCombatGroupChars;
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x0015E944 File Offset: 0x0015CB44
		public void SetAllCombatGroupChars(int[] value, DataContext context)
		{
			this._allCombatGroupChars = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x0015E964 File Offset: 0x0015CB64
		public EventCricketBettingData GetCricketBettingData()
		{
			return this._cricketBettingData;
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x0015E97C File Offset: 0x0015CB7C
		public void SetCricketBettingData(EventCricketBettingData value, DataContext context)
		{
			this._cricketBettingData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x0015E99C File Offset: 0x0015CB9C
		public List<int> GetJieqingMaskCharIdList()
		{
			return this._jieqingMaskCharIdList;
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0015E9B4 File Offset: 0x0015CBB4
		public void SetJieqingMaskCharIdList(List<int> value, DataContext context)
		{
			this._jieqingMaskCharIdList = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, this.DataStates, TaiwuEventDomain.CacheInfluences, context);
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0015E9D2 File Offset: 0x0015CBD2
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x0015E9DC File Offset: 0x0015CBDC
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			int dataSize = this._globalArgBox.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValue_Set(12, 0, dataSize);
			pData += this._globalArgBox.Serialize(pData);
			int dataSize2 = this._monthlyEventActionManager.GetSerializedSize();
			byte* pData2 = OperationAdder.DynamicSingleValue_Set(12, 1, dataSize2);
			pData2 += this._monthlyEventActionManager.Serialize(pData2);
			byte* pData3 = OperationAdder.FixedSingleValue_Set(12, 2, 4);
			*(int*)pData3 = this._awayForeverLoverCharId;
			pData3 += 4;
			byte* pData4 = OperationAdder.FixedSingleValue_Set(12, 10, 1);
			*pData4 = (this._secretVillageOnFire ? 1 : 0);
			pData4++;
			byte* pData5 = OperationAdder.FixedSingleValue_Set(12, 11, 1);
			*pData5 = (this._taiwuVillageShowShrine ? 1 : 0);
			pData5++;
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0015EA9C File Offset: 0x0015CC9C
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(12, 0));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(12, 1));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(12, 2));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(12, 10));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(12, 11));
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x0015EB1C File Offset: 0x0015CD1C
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				result = Serializer.Serialize(this._awayForeverLoverCharId, dataPool);
				break;
			case 3:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
				}
				result = Serializer.Serialize(this._eventCount, dataPool);
				break;
			case 4:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
				}
				result = Serializer.Serialize(this._healDoctorCharId, dataPool);
				break;
			case 5:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				result = Serializer.Serialize(this._cgName, dataPool);
				break;
			case 6:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
				}
				result = Serializer.Serialize(this._notifyData, dataPool);
				break;
			case 7:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				result = Serializer.Serialize(this._hasListeningEvent, dataPool);
				break;
			case 8:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
				}
				result = Serializer.Serialize(this._selectInformationData, dataPool);
				break;
			case 9:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
				}
				result = Serializer.Serialize(this._taiwuLocationChangeFlag, dataPool);
				break;
			case 10:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
				}
				result = Serializer.Serialize(this._secretVillageOnFire, dataPool);
				break;
			case 11:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
				}
				result = Serializer.Serialize(this._taiwuVillageShowShrine, dataPool);
				break;
			case 12:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
				}
				result = Serializer.Serialize(this._hideAllTeammates, dataPool);
				break;
			case 13:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
				}
				result = Serializer.Serialize(this._leftRoleAlternativeName, dataPool);
				break;
			case 14:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
				}
				result = Serializer.Serialize(this._rightRoleAlternativeName, dataPool);
				break;
			case 15:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
				}
				result = Serializer.Serialize(this._rightRoleXiangshuDisplayData, dataPool);
				break;
			case 16:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
				}
				result = Serializer.Serialize(this._selectCombatSkillData, dataPool);
				break;
			case 17:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
				}
				result = Serializer.Serialize(this._selectLifeSkillData, dataPool);
				break;
			case 18:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
				}
				result = Serializer.Serialize(this._itemListOfLeft, dataPool);
				break;
			case 19:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
				}
				result = Serializer.Serialize(this._itemListOfRight, dataPool);
				break;
			case 20:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
				}
				result = Serializer.Serialize(this._showItemWithCricketBattleGuess, dataPool);
				break;
			case 21:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
				}
				result = Serializer.Serialize(this._displayingEventData, dataPool);
				break;
			case 22:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
				}
				result = Serializer.Serialize(this._tempCreateItemList, dataPool);
				break;
			case 23:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
				}
				result = Serializer.Serialize(this._coverCricketJarGradeListForRight, dataPool);
				break;
			case 24:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
				}
				result = Serializer.Serialize(this._marriageLook1CharIdList, dataPool);
				break;
			case 25:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
				}
				result = Serializer.Serialize(this._marriageLook2CharIdList, dataPool);
				break;
			case 26:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
				}
				result = Serializer.Serialize(this._allCombatGroupChars, dataPool);
				break;
			case 27:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
				}
				result = Serializer.Serialize(this._cricketBettingData, dataPool);
				break;
			case 28:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
				}
				result = Serializer.Serialize(this._jieqingMaskCharIdList, dataPool);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x0015F094 File Offset: 0x0015D294
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
				Serializer.Deserialize(dataPool, valueOffset, ref this._awayForeverLoverCharId);
				this.SetAwayForeverLoverCharId(this._awayForeverLoverCharId, context);
				break;
			case 3:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 4:
				Serializer.Deserialize(dataPool, valueOffset, ref this._healDoctorCharId);
				this.SetHealDoctorCharId(this._healDoctorCharId, context);
				break;
			case 5:
				Serializer.Deserialize(dataPool, valueOffset, ref this._cgName);
				this.SetCgName(this._cgName, context);
				break;
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 8:
				Serializer.Deserialize(dataPool, valueOffset, ref this._selectInformationData);
				this.SetSelectInformationData(this._selectInformationData, context);
				break;
			case 9:
				Serializer.Deserialize(dataPool, valueOffset, ref this._taiwuLocationChangeFlag);
				this.SetTaiwuLocationChangeFlag(this._taiwuLocationChangeFlag, context);
				break;
			case 10:
				Serializer.Deserialize(dataPool, valueOffset, ref this._secretVillageOnFire);
				this.SetSecretVillageOnFire(this._secretVillageOnFire, context);
				break;
			case 11:
				Serializer.Deserialize(dataPool, valueOffset, ref this._taiwuVillageShowShrine);
				this.SetTaiwuVillageShowShrine(this._taiwuVillageShowShrine, context);
				break;
			case 12:
				Serializer.Deserialize(dataPool, valueOffset, ref this._hideAllTeammates);
				this.SetHideAllTeammates(this._hideAllTeammates, context);
				break;
			case 13:
				Serializer.Deserialize(dataPool, valueOffset, ref this._leftRoleAlternativeName);
				this.SetLeftRoleAlternativeName(this._leftRoleAlternativeName, context);
				break;
			case 14:
				Serializer.Deserialize(dataPool, valueOffset, ref this._rightRoleAlternativeName);
				this.SetRightRoleAlternativeName(this._rightRoleAlternativeName, context);
				break;
			case 15:
				Serializer.Deserialize(dataPool, valueOffset, ref this._rightRoleXiangshuDisplayData);
				this.SetRightRoleXiangshuDisplayData(this._rightRoleXiangshuDisplayData, context);
				break;
			case 16:
				Serializer.Deserialize(dataPool, valueOffset, ref this._selectCombatSkillData);
				this.SetSelectCombatSkillData(this._selectCombatSkillData, context);
				break;
			case 17:
				Serializer.Deserialize(dataPool, valueOffset, ref this._selectLifeSkillData);
				this.SetSelectLifeSkillData(this._selectLifeSkillData, context);
				break;
			case 18:
				Serializer.Deserialize(dataPool, valueOffset, ref this._itemListOfLeft);
				this.SetItemListOfLeft(this._itemListOfLeft, context);
				break;
			case 19:
				Serializer.Deserialize(dataPool, valueOffset, ref this._itemListOfRight);
				this.SetItemListOfRight(this._itemListOfRight, context);
				break;
			case 20:
				Serializer.Deserialize(dataPool, valueOffset, ref this._showItemWithCricketBattleGuess);
				this.SetShowItemWithCricketBattleGuess(this._showItemWithCricketBattleGuess, context);
				break;
			case 21:
				Serializer.Deserialize(dataPool, valueOffset, ref this._displayingEventData);
				this.SetDisplayingEventData(this._displayingEventData, context);
				break;
			case 22:
				Serializer.Deserialize(dataPool, valueOffset, ref this._tempCreateItemList);
				this.SetTempCreateItemList(this._tempCreateItemList, context);
				break;
			case 23:
				Serializer.Deserialize(dataPool, valueOffset, ref this._coverCricketJarGradeListForRight);
				this.SetCoverCricketJarGradeListForRight(this._coverCricketJarGradeListForRight, context);
				break;
			case 24:
				Serializer.Deserialize(dataPool, valueOffset, ref this._marriageLook1CharIdList);
				this.SetMarriageLook1CharIdList(this._marriageLook1CharIdList, context);
				break;
			case 25:
				Serializer.Deserialize(dataPool, valueOffset, ref this._marriageLook2CharIdList);
				this.SetMarriageLook2CharIdList(this._marriageLook2CharIdList, context);
				break;
			case 26:
				Serializer.Deserialize(dataPool, valueOffset, ref this._allCombatGroupChars);
				this.SetAllCombatGroupChars(this._allCombatGroupChars, context);
				break;
			case 27:
				Serializer.Deserialize(dataPool, valueOffset, ref this._cricketBettingData);
				this.SetCricketBettingData(this._cricketBettingData, context);
				break;
			case 28:
				Serializer.Deserialize(dataPool, valueOffset, ref this._jieqingMaskCharIdList);
				this.SetJieqingMaskCharIdList(this._jieqingMaskCharIdList, context);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0015F5A4 File Offset: 0x0015D7A4
		public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
		{
			int argsOffset = operation.ArgsOffset;
			int result2;
			switch (operation.MethodId)
			{
			case 0:
			{
				int argsCount = operation.ArgsCount;
				int num = argsCount;
				if (num != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				MonthlyActionKey key = default(MonthlyActionKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key);
				ValueTuple<int, int> returnValue = this.GetMonthlyActionStateAndTime(key);
				result2 = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.InitConchShipEvents();
				result2 = -1;
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string key2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key2);
				bool value = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value);
				this.TriggerListener(key2, value);
				result2 = -1;
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string key3 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key3);
				ItemKey itemKey = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey);
				bool callComplete = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref callComplete);
				this.SetItemSelectResult(key3, itemKey, callComplete);
				result2 = -1;
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string key4 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key4);
				int charId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
				bool callComplete2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref callComplete2);
				this.SetCharacterSelectResult(key4, charId, callComplete2);
				result2 = -1;
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string key5 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key5);
				int informationMetaDataId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref informationMetaDataId);
				this.SetSecretInformationSelectResult(key5, informationMetaDataId);
				result2 = -1;
				break;
			}
			case 6:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string key6 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key6);
				NormalInformation normalInformation = default(NormalInformation);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref normalInformation);
				this.SetNormalInformationSelectResult(key6, normalInformation);
				result2 = -1;
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.StartHandleEventDuringAdvance();
				result2 = -1;
				break;
			}
			case 8:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<TaiwuEventSummaryDisplayData> returnValue2 = this.GetTriggeredEventSummaryDisplayData();
				result2 = Serializer.Serialize(returnValue2, returnDataPool);
				break;
			}
			case 9:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string eventGuid = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref eventGuid);
				this.SetEventInProcessing(eventGuid);
				result2 = -1;
				break;
			}
			case 10:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 2)
				{
					if (num11 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					string eventGuid2 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref eventGuid2);
					string optionKey = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref optionKey);
					bool isContinue = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isContinue);
					this.EventSelect(eventGuid2, optionKey, isContinue);
					result2 = -1;
				}
				else
				{
					string eventGuid3 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref eventGuid3);
					string optionKey2 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref optionKey2);
					this.EventSelect(eventGuid3, optionKey2, false);
					result2 = -1;
				}
				break;
			}
			case 11:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<TaiwuEventDisplayData> returnValue3 = this.GetEventDisplayData();
				result2 = Serializer.Serialize(returnValue3, returnDataPool);
				break;
			}
			case 12:
			{
				int argsCount13 = operation.ArgsCount;
				int num13 = argsCount13;
				if (num13 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_SaveMonthlyActionManager(context);
				result2 = -1;
				break;
			}
			case 13:
			{
				int argsCount14 = operation.ArgsCount;
				int num14 = argsCount14;
				if (num14 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId2);
				this.OnCharacterClicked(context, charId2);
				result2 = -1;
				break;
			}
			case 14:
			{
				int argsCount15 = operation.ArgsCount;
				int num15 = argsCount15;
				if (num15 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId3);
				this.OnLetTeammateLeaveGroup(context, charId3);
				result2 = -1;
				break;
			}
			case 15:
			{
				int argsCount16 = operation.ArgsCount;
				int num16 = argsCount16;
				if (num16 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int caravanId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref caravanId);
				this.OnInteractCaravan(caravanId);
				result2 = -1;
				break;
			}
			case 16:
			{
				int argsCount17 = operation.ArgsCount;
				int num17 = argsCount17;
				if (num17 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId4);
				this.OnInteractKidnappedCharacter(charId4);
				result2 = -1;
				break;
			}
			case 17:
			{
				int argsCount18 = operation.ArgsCount;
				int num18 = argsCount18;
				if (num18 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short buildingTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingTemplateId);
				this.OnSectBuildingClicked(buildingTemplateId);
				result2 = -1;
				break;
			}
			case 18:
			{
				int argsCount19 = operation.ArgsCount;
				int num19 = argsCount19;
				if (num19 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short mainStoryLineProgress = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref mainStoryLineProgress);
				this.OnRecordEnterGame(mainStoryLineProgress);
				result2 = -1;
				break;
			}
			case 19:
			{
				int argsCount20 = operation.ArgsCount;
				int num20 = argsCount20;
				if (num20 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.OnNewGameMonth();
				result2 = -1;
				break;
			}
			case 20:
			{
				int argsCount21 = operation.ArgsCount;
				int num21 = argsCount21;
				if (num21 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId);
				this.OnCombatWithXiangshuMinionComplete(templateId);
				result2 = -1;
				break;
			}
			case 21:
			{
				int argsCount22 = operation.ArgsCount;
				int num22 = argsCount22;
				if (num22 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool maskVisible = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref maskVisible);
				this.OnBlackMaskAnimationComplete(maskVisible);
				result2 = -1;
				break;
			}
			case 22:
			{
				int argsCount23 = operation.ArgsCount;
				int num23 = argsCount23;
				if (num23 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey);
				short templateId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId2);
				this.OnMakingSystemOpened(blockKey, templateId2);
				result2 = -1;
				break;
			}
			case 23:
			{
				int argsCount24 = operation.ArgsCount;
				int num24 = argsCount24;
				if (num24 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey2 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey2);
				short templateId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId3);
				bool showingGetItem = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref showingGetItem);
				this.OnCollectedMakingSystemItem(blockKey2, templateId3, showingGetItem);
				result2 = -1;
				break;
			}
			case 24:
			{
				int argsCount25 = operation.ArgsCount;
				int num25 = argsCount25;
				if (num25 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId4);
				this.OnSectSpecialBuildingClicked(templateId4);
				result2 = -1;
				break;
			}
			case 25:
			{
				int argsCount26 = operation.ArgsCount;
				int num26 = argsCount26;
				if (num26 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int animalId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref animalId);
				this.AnimalAvatarClicked(animalId);
				result2 = -1;
				break;
			}
			case 26:
			{
				int argsCount27 = operation.ArgsCount;
				int num27 = argsCount27;
				if (num27 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool result = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref result);
				this.MainStoryFinishCatchCricket(result);
				result2 = -1;
				break;
			}
			case 27:
			{
				int argsCount28 = operation.ArgsCount;
				int num28 = argsCount28;
				if (num28 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string eventDataDirectory = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref eventDataDirectory);
				this.LoadEventsFromPath(eventDataDirectory);
				result2 = -1;
				break;
			}
			case 28:
			{
				int argsCount29 = operation.ArgsCount;
				int num29 = argsCount29;
				if (num29 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int tombId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref tombId);
				this.NpcTombClicked(tombId);
				result2 = -1;
				break;
			}
			case 29:
			{
				int argsCount30 = operation.ArgsCount;
				int num30 = argsCount30;
				if (num30 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short lifeSkillId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref lifeSkillId);
				this.SetLifeSkillSelectResult(lifeSkillId);
				result2 = -1;
				break;
			}
			case 30:
			{
				int argsCount31 = operation.ArgsCount;
				int num31 = argsCount31;
				if (num31 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short combatSkillId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref combatSkillId);
				this.SetCombatSkillSelectResult(combatSkillId);
				result2 = -1;
				break;
			}
			case 31:
			{
				int argsCount32 = operation.ArgsCount;
				int num32 = argsCount32;
				if (num32 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId5);
				sbyte concessionCount = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref concessionCount);
				sbyte inducementCount = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref inducementCount);
				this.OnLifeSkillCombatForceSilent(charId5, concessionCount, inducementCount);
				result2 = -1;
				break;
			}
			case 32:
			{
				int argsCount33 = operation.ArgsCount;
				int num33 = argsCount33;
				if (num33 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.TryMoveWhenMoveDisable();
				result2 = -1;
				break;
			}
			case 33:
			{
				int argsCount34 = operation.ArgsCount;
				int num34 = argsCount34;
				if (num34 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.TryMoveToInvalidLocationInTutorial();
				result2 = -1;
				break;
			}
			case 34:
			{
				int argsCount35 = operation.ArgsCount;
				int num35 = argsCount35;
				if (num35 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string actionName = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref actionName);
				string key7 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key7);
				CharacterSet characterSet = default(CharacterSet);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterSet);
				this.SetCharacterSetSelectResult(actionName, key7, characterSet);
				result2 = -1;
				break;
			}
			case 35:
			{
				int argsCount36 = operation.ArgsCount;
				int num36 = argsCount36;
				if (num36 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short characterTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterTemplateId);
				this.OnCharacterTemplateClicked(context, characterTemplateId);
				result2 = -1;
				break;
			}
			case 36:
				switch (operation.ArgsCount)
				{
				case 1:
				{
					string uiName = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref uiName);
					this.CloseUI(uiName, false, -1);
					result2 = -1;
					break;
				}
				case 2:
				{
					string uiName2 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref uiName2);
					bool presetBool = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref presetBool);
					this.CloseUI(uiName2, presetBool, -1);
					result2 = -1;
					break;
				}
				case 3:
				{
					string uiName3 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref uiName3);
					bool presetBool2 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref presetBool2);
					int presetInt = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref presetInt);
					this.CloseUI(uiName3, presetBool2, presetInt);
					result2 = -1;
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 37:
			{
				int argsCount37 = operation.ArgsCount;
				int num37 = argsCount37;
				if (num37 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref flag);
				this.SetIsQuickStartGame(flag);
				result2 = -1;
				break;
			}
			case 38:
			{
				int argsCount38 = operation.ArgsCount;
				int num38 = argsCount38;
				if (num38 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte resourceType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref resourceType);
				this.TaiwuCollectWudangHeavenlyTreeSeed(resourceType);
				result2 = -1;
				break;
			}
			case 39:
			{
				int argsCount39 = operation.ArgsCount;
				int num39 = argsCount39;
				if (num39 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				EventLogData returnValue4 = this.GetEventLogData();
				result2 = Serializer.Serialize(returnValue4, returnDataPool);
				break;
			}
			case 40:
			{
				int argsCount40 = operation.ArgsCount;
				int num40 = argsCount40;
				if (num40 != 8)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				IntPair charIds = default(IntPair);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charIds);
				string dialog = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref dialog);
				string rawResponseData = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref rawResponseData);
				EventActorData leftActor = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref leftActor);
				EventActorData rightActor = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref rightActor);
				string leftName = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref leftName);
				string rightName = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref rightName);
				short merchantTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merchantTemplateId);
				this.StartNewDialog(context, charIds, dialog, rawResponseData, leftActor, rightActor, leftName, rightName, merchantTemplateId);
				result2 = -1;
				break;
			}
			case 41:
			{
				int argsCount41 = operation.ArgsCount;
				int num41 = argsCount41;
				if (num41 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId6);
				this.TaiwuVillagerExpelled(charId6);
				result2 = -1;
				break;
			}
			case 42:
			{
				int argsCount42 = operation.ArgsCount;
				int num42 = argsCount42;
				if (num42 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_TaiwuCrossArchive();
				result2 = -1;
				break;
			}
			case 43:
			{
				int argsCount43 = operation.ArgsCount;
				int num43 = argsCount43;
				if (num43 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte type = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref type);
				this.TaiwuCrossArchiveFindMemory(type);
				result2 = -1;
				break;
			}
			case 44:
			{
				int argsCount44 = operation.ArgsCount;
				int num44 = argsCount44;
				if (num44 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.UserLoadDreamBackArchive();
				result2 = -1;
				break;
			}
			case 45:
			{
				int argsCount45 = operation.ArgsCount;
				int num45 = argsCount45;
				if (num45 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId7);
				sbyte operationType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref operationType);
				ItemDisplayData itemData = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemData);
				this.OperateInventoryItem(charId7, operationType, itemData);
				result2 = -1;
				break;
			}
			case 46:
			{
				int argsCount46 = operation.ArgsCount;
				int num46 = argsCount46;
				if (num46 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string key8 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key8);
				int count = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count);
				this.SetItemSelectCount(key8, count);
				result2 = -1;
				break;
			}
			case 47:
			{
				int argsCount47 = operation.ArgsCount;
				int num47 = argsCount47;
				if (num47 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId5);
				byte currStatus = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref currStatus);
				sbyte currPage = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref currPage);
				this.SettlementTreasuryBuildingClicked(templateId5, currStatus, currPage);
				result2 = -1;
				break;
			}
			case 48:
			{
				int argsCount48 = operation.ArgsCount;
				int num48 = argsCount48;
				if (num48 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string actionName2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref actionName2);
				string key9 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key9);
				ISerializableGameData value2 = null;
				argsOffset += Serializer.DeserializeDefault<ISerializableGameData>(argDataPool, argsOffset, ref value2);
				this.SetListenerEventActionISerializableArg(actionName2, key9, value2);
				result2 = -1;
				break;
			}
			case 49:
			{
				int argsCount49 = operation.ArgsCount;
				int num49 = argsCount49;
				if (num49 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string actionName3 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref actionName3);
				string key10 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key10);
				int value3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value3);
				this.SetListenerEventActionIntArg(actionName3, key10, value3);
				result2 = -1;
				break;
			}
			case 50:
			{
				int argsCount50 = operation.ArgsCount;
				int num50 = argsCount50;
				if (num50 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string actionName4 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref actionName4);
				string key11 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key11);
				bool value4 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value4);
				this.SetListenerEventActionBoolArg(actionName4, key11, value4);
				result2 = -1;
				break;
			}
			case 51:
			{
				int argsCount51 = operation.ArgsCount;
				int num51 = argsCount51;
				if (num51 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string actionName5 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref actionName5);
				string key12 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key12);
				string value5 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value5);
				this.SetListenerEventActionStringArg(actionName5, key12, value5);
				result2 = -1;
				break;
			}
			case 52:
			{
				int argsCount52 = operation.ArgsCount;
				int num52 = argsCount52;
				if (num52 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int targetCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetCharId);
				List<short> returnValue5 = this.GetValidInteractionEventOptions(targetCharId);
				result2 = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 53:
			{
				int argsCount53 = operation.ArgsCount;
				int num53 = argsCount53;
				if (num53 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string actionName6 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref actionName6);
				string key13 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key13);
				IntList value6 = default(IntList);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value6);
				this.SetListenerEventActionIntListArg(actionName6, key13, value6);
				result2 = -1;
				break;
			}
			case 54:
			{
				int argsCount54 = operation.ArgsCount;
				int num54 = argsCount54;
				if (num54 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string actionName7 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref actionName7);
				string key14 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key14);
				ItemKey value7 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value7);
				this.SetListenerEventActionItemKeyArg(actionName7, key14, value7);
				result2 = -1;
				break;
			}
			case 55:
			{
				int argsCount55 = operation.ArgsCount;
				int num55 = argsCount55;
				if (num55 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.TriggerShixiangDrumEasterEgg();
				result2 = -1;
				break;
			}
			case 56:
			{
				int argsCount56 = operation.ArgsCount;
				int num56 = argsCount56;
				if (num56 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int characterId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterId);
				int interactPrisonerType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref interactPrisonerType);
				this.InteractPrisoner(characterId, interactPrisonerType);
				result2 = -1;
				break;
			}
			case 57:
			{
				int argsCount57 = operation.ArgsCount;
				int num57 = argsCount57;
				if (num57 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.OnClickedSendPrisonBtn();
				result2 = -1;
				break;
			}
			case 58:
			{
				int argsCount58 = operation.ArgsCount;
				int num58 = argsCount58;
				if (num58 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short buildingTemplateId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingTemplateId2);
				this.OnClickedPrisonBtn(buildingTemplateId2);
				result2 = -1;
				break;
			}
			case 59:
			{
				int argsCount59 = operation.ArgsCount;
				int num59 = argsCount59;
				if (num59 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string key15 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key15);
				List<int> charIds2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charIds2);
				bool callComplete3 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref callComplete3);
				this.SetCharacterMultSelectResult(key15, charIds2, callComplete3);
				result2 = -1;
				break;
			}
			case 60:
			{
				int argsCount60 = operation.ArgsCount;
				int num60 = argsCount60;
				if (num60 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool ok = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref ok);
				Wager wager = default(Wager);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref wager);
				int index = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index);
				this.SetCricketBettingResult(ok, wager, index);
				result2 = -1;
				break;
			}
			case 61:
			{
				int argsCount61 = operation.ArgsCount;
				int num61 = argsCount61;
				if (num61 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> returnValue6 = this.GetImplementedFunctionIds(context);
				result2 = Serializer.Serialize(returnValue6, returnDataPool);
				break;
			}
			case 62:
			{
				int argsCount62 = operation.ArgsCount;
				int num62 = argsCount62;
				if (num62 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isPaused = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isPaused);
				this.SetEventScriptExecutionPause(isPaused);
				result2 = -1;
				break;
			}
			case 63:
			{
				int argsCount63 = operation.ArgsCount;
				int num63 = argsCount63;
				if (num63 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.EventScriptExecuteNext();
				result2 = -1;
				break;
			}
			case 64:
			{
				int argsCount64 = operation.ArgsCount;
				int num64 = argsCount64;
				if (num64 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte orgTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref orgTemplateId);
				sbyte severity = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref severity);
				this.GmCmd_TaiwuWantedSectPunished(context, orgTemplateId, severity);
				result2 = -1;
				break;
			}
			case 65:
			{
				int argsCount65 = operation.ArgsCount;
				int num65 = argsCount65;
				if (num65 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.EventSelectContinue();
				result2 = -1;
				break;
			}
			case 66:
			{
				int argsCount66 = operation.ArgsCount;
				int num66 = argsCount66;
				if (num66 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int count2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count2);
				this.SetSelectCount(count2);
				result2 = -1;
				break;
			}
			case 67:
			{
				int argsCount67 = operation.ArgsCount;
				int num67 = argsCount67;
				if (num67 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string actionName8 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref actionName8);
				string key16 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key16);
				GameData.Utilities.ShortList value8 = default(GameData.Utilities.ShortList);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value8);
				this.SetListenerEventActionShortListArg(actionName8, key16, value8);
				result2 = -1;
				break;
			}
			case 68:
			{
				int argsCount68 = operation.ArgsCount;
				int num68 = argsCount68;
				if (num68 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string key17 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key17);
				GameData.Utilities.ShortList value9 = default(GameData.Utilities.ShortList);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value9);
				this.SetShowingEventShortListArg(key17, value9);
				result2 = -1;
				break;
			}
			case 69:
			{
				int argsCount69 = operation.ArgsCount;
				int num69 = argsCount69;
				if (num69 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location);
				this.OnClickMapPickupEvent(location);
				result2 = -1;
				break;
			}
			case 70:
			{
				int argsCount70 = operation.ArgsCount;
				int num70 = argsCount70;
				if (num70 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location2 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location2);
				this.OnClickMapPickupNormalEvent(location2);
				result2 = -1;
				break;
			}
			case 71:
			{
				int argsCount71 = operation.ArgsCount;
				int num71 = argsCount71;
				if (num71 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int type2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref type2);
				bool isGood = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isGood);
				this.OnClickDeportButton(type2, isGood);
				result2 = -1;
				break;
			}
			case 72:
			{
				int argsCount72 = operation.ArgsCount;
				int num72 = argsCount72;
				if (num72 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				byte currStatus2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref currStatus2);
				sbyte currPage2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref currPage2);
				this.OnSwitchToGuardedPage(currStatus2, currPage2);
				result2 = -1;
				break;
			}
			case 73:
			{
				int argsCount73 = operation.ArgsCount;
				int num73 = argsCount73;
				if (num73 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId8);
				this.GmCmd_AddJieqingMaskCharId(charId8);
				result2 = -1;
				break;
			}
			case 74:
			{
				int argsCount74 = operation.ArgsCount;
				int num74 = argsCount74;
				if (num74 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId9 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId9);
				this.GmCmd_RemoveJieqingMaskCharId(charId9);
				result2 = -1;
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result2;
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x001619C4 File Offset: 0x0015FBC4
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
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00161ABC File Offset: 0x0015FCBC
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
					result = Serializer.Serialize(this._awayForeverLoverCharId, dataPool);
				}
				break;
			}
			case 3:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					result = Serializer.Serialize(this._eventCount, dataPool);
				}
				break;
			}
			case 4:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					result = Serializer.Serialize(this._healDoctorCharId, dataPool);
				}
				break;
			}
			case 5:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (flag4)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					result = Serializer.Serialize(this._cgName, dataPool);
				}
				break;
			}
			case 6:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (flag5)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
					result = Serializer.Serialize(this._notifyData, dataPool);
				}
				break;
			}
			case 7:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (flag6)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					result = Serializer.Serialize(this._hasListeningEvent, dataPool);
				}
				break;
			}
			case 8:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (flag7)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					result = Serializer.Serialize(this._selectInformationData, dataPool);
				}
				break;
			}
			case 9:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 9);
				if (flag8)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
					result = Serializer.Serialize(this._taiwuLocationChangeFlag, dataPool);
				}
				break;
			}
			case 10:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 10);
				if (flag9)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
					result = Serializer.Serialize(this._secretVillageOnFire, dataPool);
				}
				break;
			}
			case 11:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (flag10)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
					result = Serializer.Serialize(this._taiwuVillageShowShrine, dataPool);
				}
				break;
			}
			case 12:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 12);
				if (flag11)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
					result = Serializer.Serialize(this._hideAllTeammates, dataPool);
				}
				break;
			}
			case 13:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 13);
				if (flag12)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
					result = Serializer.Serialize(this._leftRoleAlternativeName, dataPool);
				}
				break;
			}
			case 14:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 14);
				if (flag13)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
					result = Serializer.Serialize(this._rightRoleAlternativeName, dataPool);
				}
				break;
			}
			case 15:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (flag14)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
					result = Serializer.Serialize(this._rightRoleXiangshuDisplayData, dataPool);
				}
				break;
			}
			case 16:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (flag15)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
					result = Serializer.Serialize(this._selectCombatSkillData, dataPool);
				}
				break;
			}
			case 17:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this.DataStates, 17);
				if (flag16)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
					result = Serializer.Serialize(this._selectLifeSkillData, dataPool);
				}
				break;
			}
			case 18:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (flag17)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
					result = Serializer.Serialize(this._itemListOfLeft, dataPool);
				}
				break;
			}
			case 19:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 19);
				if (flag18)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
					result = Serializer.Serialize(this._itemListOfRight, dataPool);
				}
				break;
			}
			case 20:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (flag19)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
					result = Serializer.Serialize(this._showItemWithCricketBattleGuess, dataPool);
				}
				break;
			}
			case 21:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (flag20)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
					result = Serializer.Serialize(this._displayingEventData, dataPool);
				}
				break;
			}
			case 22:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (flag21)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
					result = Serializer.Serialize(this._tempCreateItemList, dataPool);
				}
				break;
			}
			case 23:
			{
				bool flag22 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (flag22)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
					result = Serializer.Serialize(this._coverCricketJarGradeListForRight, dataPool);
				}
				break;
			}
			case 24:
			{
				bool flag23 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (flag23)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
					result = Serializer.Serialize(this._marriageLook1CharIdList, dataPool);
				}
				break;
			}
			case 25:
			{
				bool flag24 = !BaseGameDataDomain.IsModified(this.DataStates, 25);
				if (flag24)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
					result = Serializer.Serialize(this._marriageLook2CharIdList, dataPool);
				}
				break;
			}
			case 26:
			{
				bool flag25 = !BaseGameDataDomain.IsModified(this.DataStates, 26);
				if (flag25)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
					result = Serializer.Serialize(this._allCombatGroupChars, dataPool);
				}
				break;
			}
			case 27:
			{
				bool flag26 = !BaseGameDataDomain.IsModified(this.DataStates, 27);
				if (flag26)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
					result = Serializer.Serialize(this._cricketBettingData, dataPool);
				}
				break;
			}
			case 28:
			{
				bool flag27 = !BaseGameDataDomain.IsModified(this.DataStates, 28);
				if (flag27)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
					result = Serializer.Serialize(this._jieqingMaskCharIdList, dataPool);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00162298 File Offset: 0x00160498
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				break;
			}
			case 3:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
				}
				break;
			}
			case 4:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (!flag3)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
				}
				break;
			}
			case 5:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (!flag4)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				break;
			}
			case 6:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (!flag5)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
				}
				break;
			}
			case 7:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (!flag6)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				break;
			}
			case 8:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (!flag7)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
				}
				break;
			}
			case 9:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 9);
				if (!flag8)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
				}
				break;
			}
			case 10:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 10);
				if (!flag9)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
				}
				break;
			}
			case 11:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (!flag10)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
				}
				break;
			}
			case 12:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 12);
				if (!flag11)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
				}
				break;
			}
			case 13:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 13);
				if (!flag12)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
				}
				break;
			}
			case 14:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 14);
				if (!flag13)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
				}
				break;
			}
			case 15:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (!flag14)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
				}
				break;
			}
			case 16:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (!flag15)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
				}
				break;
			}
			case 17:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this.DataStates, 17);
				if (!flag16)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
				}
				break;
			}
			case 18:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (!flag17)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
				}
				break;
			}
			case 19:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 19);
				if (!flag18)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 19);
				}
				break;
			}
			case 20:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (!flag19)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
				}
				break;
			}
			case 21:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (!flag20)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
				}
				break;
			}
			case 22:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (!flag21)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
				}
				break;
			}
			case 23:
			{
				bool flag22 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (!flag22)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
				}
				break;
			}
			case 24:
			{
				bool flag23 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (!flag23)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
				}
				break;
			}
			case 25:
			{
				bool flag24 = !BaseGameDataDomain.IsModified(this.DataStates, 25);
				if (!flag24)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 25);
				}
				break;
			}
			case 26:
			{
				bool flag25 = !BaseGameDataDomain.IsModified(this.DataStates, 26);
				if (!flag25)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 26);
				}
				break;
			}
			case 27:
			{
				bool flag26 = !BaseGameDataDomain.IsModified(this.DataStates, 27);
				if (!flag26)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 27);
				}
				break;
			}
			case 28:
			{
				bool flag27 = !BaseGameDataDomain.IsModified(this.DataStates, 28);
				if (!flag27)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 28);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00162888 File Offset: 0x00160A88
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
				result = BaseGameDataDomain.IsModified(this.DataStates, 2);
				break;
			case 3:
				result = BaseGameDataDomain.IsModified(this.DataStates, 3);
				break;
			case 4:
				result = BaseGameDataDomain.IsModified(this.DataStates, 4);
				break;
			case 5:
				result = BaseGameDataDomain.IsModified(this.DataStates, 5);
				break;
			case 6:
				result = BaseGameDataDomain.IsModified(this.DataStates, 6);
				break;
			case 7:
				result = BaseGameDataDomain.IsModified(this.DataStates, 7);
				break;
			case 8:
				result = BaseGameDataDomain.IsModified(this.DataStates, 8);
				break;
			case 9:
				result = BaseGameDataDomain.IsModified(this.DataStates, 9);
				break;
			case 10:
				result = BaseGameDataDomain.IsModified(this.DataStates, 10);
				break;
			case 11:
				result = BaseGameDataDomain.IsModified(this.DataStates, 11);
				break;
			case 12:
				result = BaseGameDataDomain.IsModified(this.DataStates, 12);
				break;
			case 13:
				result = BaseGameDataDomain.IsModified(this.DataStates, 13);
				break;
			case 14:
				result = BaseGameDataDomain.IsModified(this.DataStates, 14);
				break;
			case 15:
				result = BaseGameDataDomain.IsModified(this.DataStates, 15);
				break;
			case 16:
				result = BaseGameDataDomain.IsModified(this.DataStates, 16);
				break;
			case 17:
				result = BaseGameDataDomain.IsModified(this.DataStates, 17);
				break;
			case 18:
				result = BaseGameDataDomain.IsModified(this.DataStates, 18);
				break;
			case 19:
				result = BaseGameDataDomain.IsModified(this.DataStates, 19);
				break;
			case 20:
				result = BaseGameDataDomain.IsModified(this.DataStates, 20);
				break;
			case 21:
				result = BaseGameDataDomain.IsModified(this.DataStates, 21);
				break;
			case 22:
				result = BaseGameDataDomain.IsModified(this.DataStates, 22);
				break;
			case 23:
				result = BaseGameDataDomain.IsModified(this.DataStates, 23);
				break;
			case 24:
				result = BaseGameDataDomain.IsModified(this.DataStates, 24);
				break;
			case 25:
				result = BaseGameDataDomain.IsModified(this.DataStates, 25);
				break;
			case 26:
				result = BaseGameDataDomain.IsModified(this.DataStates, 26);
				break;
			case 27:
				result = BaseGameDataDomain.IsModified(this.DataStates, 27);
				break;
			case 28:
				result = BaseGameDataDomain.IsModified(this.DataStates, 28);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x00162B8C File Offset: 0x00160D8C
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (influence.TargetIndicator.DataId)
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
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00162CCC File Offset: 0x00160ECC
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (operation.DataId)
			{
			case 0:
				ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Ref_Single<EventArgBox>(operation, pResult, this._globalArgBox);
				break;
			case 1:
				ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Ref_Single<MonthlyEventActionsManager>(operation, pResult, this._monthlyEventActionManager);
				break;
			case 2:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<int>(operation, pResult, ref this._awayForeverLoverCharId);
				break;
			case 3:
				goto IL_20C;
			case 4:
				goto IL_20C;
			case 5:
				goto IL_20C;
			case 6:
				goto IL_20C;
			case 7:
				goto IL_20C;
			case 8:
				goto IL_20C;
			case 9:
				goto IL_20C;
			case 10:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<bool>(operation, pResult, ref this._secretVillageOnFire);
				break;
			case 11:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<bool>(operation, pResult, ref this._taiwuVillageShowShrine);
				break;
			case 12:
				goto IL_20C;
			case 13:
				goto IL_20C;
			case 14:
				goto IL_20C;
			case 15:
				goto IL_20C;
			case 16:
				goto IL_20C;
			case 17:
				goto IL_20C;
			case 18:
				goto IL_20C;
			case 19:
				goto IL_20C;
			case 20:
				goto IL_20C;
			case 21:
				goto IL_20C;
			case 22:
				goto IL_20C;
			case 23:
				goto IL_20C;
			case 24:
				goto IL_20C;
			case 25:
				goto IL_20C;
			case 26:
				goto IL_20C;
			case 27:
				goto IL_20C;
			case 28:
				goto IL_20C;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag = this._pendingLoadingOperationIds != null;
			if (flag)
			{
				uint currPendingOperationId = this._pendingLoadingOperationIds.Peek();
				bool flag2 = currPendingOperationId == operation.Id;
				if (flag2)
				{
					this._pendingLoadingOperationIds.Dequeue();
					bool flag3 = this._pendingLoadingOperationIds.Count <= 0;
					if (flag3)
					{
						this._pendingLoadingOperationIds = null;
						this.InitializeInternalDataOfCollections();
						this.OnLoadedArchiveData();
						DomainManager.Global.CompleteLoading(12);
					}
				}
			}
			return;
			IL_20C:
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00162F19 File Offset: 0x00161119
		private void InitializeInternalDataOfCollections()
		{
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x00162F1C File Offset: 0x0016111C
		private void TaskCheckDaoShiAskForWildFood()
		{
			bool flag = DomainManager.World.GetMainStoryLineProgress() == 3;
			if (flag)
			{
				EventHelper.SaveGlobalArg<bool>("DaoshiAskForWildFood", true);
			}
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x00162F48 File Offset: 0x00161148
		private bool CanTriggerEventType(EEventType eventType)
		{
			return DomainManager.TutorialChapter.InGuiding == (eventType == EEventType.TutorialEvent);
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x00162F6C File Offset: 0x0016116C
		public void OnEvent_TaiwuBlockChanged(Location arg0, Location arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuBlockChanged(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x00162FC0 File Offset: 0x001611C0
		public void OnEvent_CharacterClicked(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_CharacterClicked(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x00163014 File Offset: 0x00161214
		public void OnEvent_AdventureReachStartNode(short arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_AdventureReachStartNode(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x00163068 File Offset: 0x00161268
		public void OnEvent_AdventureReachTransferNode(short arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_AdventureReachTransferNode(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x001630BC File Offset: 0x001612BC
		public void OnEvent_AdventureReachEndNode(short arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_AdventureReachEndNode(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x00163110 File Offset: 0x00161310
		public void OnEvent_AdventureEnterNode(AdventureMapPoint arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_AdventureEnterNode(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x00163164 File Offset: 0x00161364
		public void OnEvent_EnterTutorialChapter(short arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_EnterTutorialChapter(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x001631B8 File Offset: 0x001613B8
		public void OnEvent_LetTeammateLeaveGroup(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_LetTeammateLeaveGroup(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0016320C File Offset: 0x0016140C
		public void OnEvent_NeedToPassLegacy(bool arg0, string arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_NeedToPassLegacy(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00163260 File Offset: 0x00161460
		public void OnEvent_CaravanClicked(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_CaravanClicked(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x001632B4 File Offset: 0x001614B4
		public void OnEvent_KidnappedCharacterClicked(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_KidnappedCharacterClicked(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00163308 File Offset: 0x00161508
		public void OnEvent_TeammateMonthAdvance(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TeammateMonthAdvance(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x0016335C File Offset: 0x0016155C
		public void OnEvent_SameBlockWithTaiwuWhenMonthAdvance(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_SameBlockWithTaiwuWhenMonthAdvance(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x001633B0 File Offset: 0x001615B0
		public void OnEvent_SameBlockWithRandomEnemyOnNewMonth()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_SameBlockWithRandomEnemyOnNewMonth();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00163400 File Offset: 0x00161600
		public void OnEvent_SameBlockWithTaiwuOnNewMonthSpecial(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_SameBlockWithTaiwuOnNewMonthSpecial(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00163454 File Offset: 0x00161654
		public void OnEvent_SectBuildingClicked(short arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_SectBuildingClicked(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x001634A8 File Offset: 0x001616A8
		public void OnEvent_SecretInformationBroadcast(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_SecretInformationBroadcast(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x001634FC File Offset: 0x001616FC
		public void OnEvent_RecordEnterGame(short arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_RecordEnterGame(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00163550 File Offset: 0x00161750
		public void OnEvent_NewGameMonth()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_NewGameMonth();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x001635A0 File Offset: 0x001617A0
		public void OnEvent_CombatWithXiangshuMinionComplete(short arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_CombatWithXiangshuMinionComplete(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x001635F4 File Offset: 0x001617F4
		public void OnEvent_BlackMaskAnimationComplete(bool arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_BlackMaskAnimationComplete(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00163648 File Offset: 0x00161848
		public void OnEvent_ConstructComplete(BuildingBlockKey arg0, short arg1, sbyte arg2)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_ConstructComplete(arg0, arg1, arg2);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0016369C File Offset: 0x0016189C
		public void OnEvent_CombatOpening(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_CombatOpening(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x001636F0 File Offset: 0x001618F0
		public void OnEvent_MakingSystemOpened(BuildingBlockKey arg0, short arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_MakingSystemOpened(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x00163744 File Offset: 0x00161944
		public void OnEvent_CollectedMakingSystemItem(BuildingBlockKey arg0, short arg1, bool arg2)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_CollectedMakingSystemItem(arg0, arg1, arg2);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00163798 File Offset: 0x00161998
		public void OnEvent_TaiwuVillageDestroyed()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuVillageDestroyed();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x001637E8 File Offset: 0x001619E8
		public void OnEvent_OnSectSpecialBuildingClicked(short arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_OnSectSpecialBuildingClicked(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0016383C File Offset: 0x00161A3C
		public void OnEvent_AnimalAvatarClicked(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_AnimalAvatarClicked(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x00163890 File Offset: 0x00161A90
		public void OnEvent_MainStoryFinishCatchCricket(bool arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_MainStoryFinishCatchCricket(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x001638E4 File Offset: 0x00161AE4
		public void OnEvent_PurpleBambooAvatarClicked(int arg0, sbyte arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_PurpleBambooAvatarClicked(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00163938 File Offset: 0x00161B38
		public void OnEvent_UserLoadDreamBackArchive()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_UserLoadDreamBackArchive();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00163988 File Offset: 0x00161B88
		public void OnEvent_NpcTombClicked(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_NpcTombClicked(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x001639DC File Offset: 0x00161BDC
		public void OnEvent_LifeSkillCombatForceSilent(int arg0, sbyte arg1, sbyte arg2)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_LifeSkillCombatForceSilent(arg0, arg1, arg2);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x00163A30 File Offset: 0x00161C30
		public void OnEvent_TryMoveWhenMoveDisabled()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TryMoveWhenMoveDisabled();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x00163A80 File Offset: 0x00161C80
		public void OnEvent_TryMoveToInvalidLocationInTutorial()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TryMoveToInvalidLocationInTutorial();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x00163AD0 File Offset: 0x00161CD0
		public void OnEvent_ProfessionExperienceChange(int arg0, int arg1, int arg2)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_ProfessionExperienceChange(arg0, arg1, arg2);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x00163B24 File Offset: 0x00161D24
		public void OnEvent_ProfessionSkillClicked(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_ProfessionSkillClicked(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x00163B78 File Offset: 0x00161D78
		public void OnEvent_TaiwuGotTianjieFulu(int arg0, ItemKey arg1, int arg2)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuGotTianjieFulu(arg0, arg1, arg2);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x00163BCC File Offset: 0x00161DCC
		public void OnEvent_TaiwuSaveCountChange(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuSaveCountChange(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x00163C20 File Offset: 0x00161E20
		public void OnEvent_CharacterTemplateClicked(short arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_CharacterTemplateClicked(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x00163C74 File Offset: 0x00161E74
		public void OnEvent_CloseUI(string arg0, bool arg1, int arg2)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_CloseUI(arg0, arg1, arg2);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00163CC8 File Offset: 0x00161EC8
		public void OnEvent_TaiwuFindMaterial(int arg0, TreasureFindResult arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuFindMaterial(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00163D1C File Offset: 0x00161F1C
		public void OnEvent_TaiwuFindExtraTreasure(TreasureFindResult arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuFindExtraTreasure(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x00163D70 File Offset: 0x00161F70
		public void OnEvent_TaiwuCollectWudangHeavenlyTreeSeed(sbyte arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuCollectWudangHeavenlyTreeSeed(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x00163DC4 File Offset: 0x00161FC4
		public void OnEvent_TaiwuVillagerExpelled(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuVillagerExpelled(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x00163E18 File Offset: 0x00162018
		public void OnEvent_TaiwuCrossArchive()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuCrossArchive();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x00163E68 File Offset: 0x00162068
		public void OnEvent_TaiwuCrossArchiveFindMemory(sbyte arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuCrossArchiveFindMemory(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x00163EBC File Offset: 0x001620BC
		public void OnEvent_DlcLoongPutJiaoEggs(int arg0, ItemKey arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_DlcLoongPutJiaoEggs(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x00163F10 File Offset: 0x00162110
		public void OnEvent_DlcLoongInteractJiao(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_DlcLoongInteractJiao(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00163F64 File Offset: 0x00162164
		public void OnEvent_DlcLoongPetJiao(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_DlcLoongPetJiao(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x00163FB8 File Offset: 0x001621B8
		public void OnEvent_JingangSectMainStoryReborn()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_JingangSectMainStoryReborn();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00164008 File Offset: 0x00162208
		public void OnEvent_JingangSectMainStoryMonkSoul()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_JingangSectMainStoryMonkSoul();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x00164058 File Offset: 0x00162258
		public void OnEvent_OperateInventoryItem(int arg0, sbyte arg1, ItemDisplayData arg2)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_OperateInventoryItem(arg0, arg1, arg2);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x001640AC File Offset: 0x001622AC
		public void OnEvent_OnSettlementTreasuryBuildingClicked(short arg0, byte arg1, sbyte arg2)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_OnSettlementTreasuryBuildingClicked(arg0, arg1, arg2);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x00164100 File Offset: 0x00162300
		public void OnEvent_OnShixiangDrumClickedManyTimes()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_OnShixiangDrumClickedManyTimes();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x00164150 File Offset: 0x00162350
		public void OnEvent_OnClickedPrisonBtn(short arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_OnClickedPrisonBtn(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x001641A4 File Offset: 0x001623A4
		public void OnEvent_OnClickedSendPrisonBtn()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_OnClickedSendPrisonBtn();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x001641F4 File Offset: 0x001623F4
		public void OnEvent_InteractPrisoner(int arg0, int arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_InteractPrisoner(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x00164248 File Offset: 0x00162448
		public void OnEvent_ClickChicken(int arg0, short arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_ClickChicken(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x0016429C File Offset: 0x0016249C
		public void OnEvent_SoulWitheringBellTransfer()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_SoulWitheringBellTransfer();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x001642EC File Offset: 0x001624EC
		public void OnEvent_CatchThief(sbyte arg0, bool arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_CatchThief(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x00164340 File Offset: 0x00162540
		public void OnEvent_ConfirmEnterSwordTomb()
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_ConfirmEnterSwordTomb();
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x00164390 File Offset: 0x00162590
		public void OnEvent_TaiwuBeHuntedArrivedSect(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuBeHuntedArrivedSect(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x001643E4 File Offset: 0x001625E4
		public void OnEvent_TaiwuBeHuntedHunterDie(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuBeHuntedHunterDie(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x00164438 File Offset: 0x00162638
		public void OnEvent_StartSectShaolinDemonSlayer(int arg0)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_StartSectShaolinDemonSlayer(arg0);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x0016448C File Offset: 0x0016268C
		public void OnEvent_TriggerMapPickupEvent(Location arg0, bool arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TriggerMapPickupEvent(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x001644E0 File Offset: 0x001626E0
		public void OnEvent_FixedCharacterClicked(int arg0, short arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_FixedCharacterClicked(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x00164534 File Offset: 0x00162734
		public void OnEvent_FixedEnemyClicked(int arg0, short arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_FixedEnemyClicked(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x00164588 File Offset: 0x00162788
		public void OnEvent_AdventureRemoved(short arg0, Location arg1, bool arg2)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_AdventureRemoved(arg0, arg1, arg2);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x001645DC File Offset: 0x001627DC
		public void OnEvent_TaiwuDeportVitals(int arg0, bool arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_TaiwuDeportVitals(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x00164630 File Offset: 0x00162830
		public void OnEvent_SwitchToGuardedPage(byte arg0, sbyte arg1)
		{
			for (int i = 0; i < TaiwuEventDomain._managerArray.Length; i++)
			{
				bool flag = !this.CanTriggerEventType((EEventType)i);
				if (!flag)
				{
					EventManagerBase eventManagerBase = TaiwuEventDomain._managerArray[i];
					if (eventManagerBase != null)
					{
						eventManagerBase.OnEventTrigger_SwitchToGuardedPage(arg0, arg1);
					}
				}
			}
			this.TriggerHandled();
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x001646A4 File Offset: 0x001628A4
		[CompilerGenerated]
		internal static Assembly <LoadEventPackageAssembly>g__LoadBinaryWithPdb|212_0(ref TaiwuEventDomain.<>c__DisplayClass212_0 A_0)
		{
			DirectoryInfo directory = Directory.GetParent(A_0.path);
			string pdbPath = Path.Combine(directory.FullName, Path.GetFileNameWithoutExtension(A_0.path) + ".pdb");
			bool flag = File.Exists(pdbPath);
			Assembly result;
			if (flag)
			{
				result = Assembly.Load(File.ReadAllBytes(A_0.path), File.ReadAllBytes(pdbPath));
			}
			else
			{
				result = Assembly.Load(File.ReadAllBytes(A_0.path));
			}
			return result;
		}

		// Token: 0x04000533 RID: 1331
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04000534 RID: 1332
		private static EventManagerBase[] _managerArray;

		// Token: 0x04000535 RID: 1333
		private LocalObjectPool<EventArgBox> _argBoxPool;

		// Token: 0x04000536 RID: 1334
		private List<TaiwuEvent> _triggeredEventList;

		// Token: 0x04000537 RID: 1335
		private TaiwuEvent _showingEvent;

		// Token: 0x0400053A RID: 1338
		[TupleElementNames(new string[]
		{
			"EventGuid",
			"OptionKey",
			"nextGuid"
		})]
		private ValueTuple<string, string, string> _interactCheckContinueData;

		// Token: 0x0400053B RID: 1339
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private TaiwuEventDisplayData _displayingEventData;

		// Token: 0x0400053C RID: 1340
		public DataContext MainThreadDataContext;

		// Token: 0x0400053D RID: 1341
		public string SeriesEventTexture;

		// Token: 0x0400053E RID: 1342
		private bool _stopAutoNextEvent;

		// Token: 0x0400053F RID: 1343
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<ItemKey> _tempCreateItemList;

		// Token: 0x04000540 RID: 1344
		private string _waitConfirmSelectOption;

		// Token: 0x04000541 RID: 1345
		private TaiwuEventItem _waitConfirmEventConfig;

		// Token: 0x04000542 RID: 1346
		private string _waitConfirmOptionKey;

		// Token: 0x04000543 RID: 1347
		[Obsolete("use arg in global argument box instead")]
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private int _awayForeverLoverCharId;

		// Token: 0x04000544 RID: 1348
		private readonly List<string> _eventEnteredList = new List<string>();

		// Token: 0x04000546 RID: 1350
		[TupleElementNames(new string[]
		{
			"listeningAction",
			"listenerEvent"
		})]
		public List<ValueTuple<string, TaiwuEvent>> ListeningEventActionList = new List<ValueTuple<string, TaiwuEvent>>();

		// Token: 0x04000547 RID: 1351
		[Obsolete]
		private TaiwuEvent _listenerEvent;

		// Token: 0x04000549 RID: 1353
		private readonly Queue<List<EventLogResultData>> _eventLogQueue = new Queue<List<EventLogResultData>>();

		// Token: 0x0400054A RID: 1354
		private readonly Dictionary<int, CharacterDisplayData> _characterCache = new Dictionary<int, CharacterDisplayData>();

		// Token: 0x0400054B RID: 1355
		private readonly Dictionary<int, int> _characterReferences = new Dictionary<int, int>();

		// Token: 0x0400054C RID: 1356
		private readonly Dictionary<int, SecretInformationDisplayData> _secretInformationCache = new Dictionary<int, SecretInformationDisplayData>();

		// Token: 0x0400054D RID: 1357
		private readonly Dictionary<int, int> _secretInformationReferences = new Dictionary<int, int>();

		// Token: 0x0400054E RID: 1358
		private readonly Dictionary<int, ItemDisplayData> _itemCache = new Dictionary<int, ItemDisplayData>();

		// Token: 0x0400054F RID: 1359
		private readonly Dictionary<int, int> _itemReferences = new Dictionary<int, int>();

		// Token: 0x04000550 RID: 1360
		private readonly Dictionary<int, CombatSkillDisplayData> _combatSkillCache = new Dictionary<int, CombatSkillDisplayData>();

		// Token: 0x04000551 RID: 1361
		private readonly Dictionary<int, int> _combatSkillReferences = new Dictionary<int, int>();

		// Token: 0x04000552 RID: 1362
		private readonly Dictionary<int, ValueTuple<ItemKey, bool>> _itemKeys = new Dictionary<int, ValueTuple<ItemKey, bool>>();

		// Token: 0x04000553 RID: 1363
		private readonly Dictionary<int, EventLogCharacterData> _npcStatus = new Dictionary<int, EventLogCharacterData>();

		// Token: 0x04000554 RID: 1364
		private readonly EventLogCharacterData _taiwuStatus = new EventLogCharacterData(true);

		// Token: 0x04000555 RID: 1365
		private List<EventLogResultData> _resultCache = new List<EventLogResultData>();

		// Token: 0x04000556 RID: 1366
		private string _rawResponseData = "";

		// Token: 0x04000557 RID: 1367
		private ValueTuple<int, int> _interactingCharacters = new ValueTuple<int, int>(-1, -1);

		// Token: 0x04000558 RID: 1368
		private int _adventureId = -1;

		// Token: 0x04000559 RID: 1369
		private bool _isCheckValid = false;

		// Token: 0x0400055A RID: 1370
		private bool _isSequential = false;

		// Token: 0x0400055B RID: 1371
		private bool _shouldCheckStatusImmediately = true;

		// Token: 0x0400055C RID: 1372
		private readonly HashSet<IntPair> _executedOncePerMonthOptions = new HashSet<IntPair>();

		// Token: 0x0400055D RID: 1373
		private static List<EventPackage> _packagesList;

		// Token: 0x0400055E RID: 1374
		[TupleElementNames(new string[]
		{
			"TaiwuEventOption",
			"templateId",
			"TaiwuEventItem"
		})]
		private static List<ValueTuple<TaiwuEventOption, short, TaiwuEventItem>> _characterInteractionEventOptionList = new List<ValueTuple<TaiwuEventOption, short, TaiwuEventItem>>();

		// Token: 0x0400055F RID: 1375
		private static TaiwuEventDomain.EEventPackageLoadMethod _loadMethod;

		// Token: 0x04000560 RID: 1376
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private EventArgBox _globalArgBox;

		// Token: 0x04000562 RID: 1378
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private MonthlyEventActionsManager _monthlyEventActionManager;

		// Token: 0x04000563 RID: 1379
		private static EventScriptRuntime _scriptRuntime;

		// Token: 0x04000564 RID: 1380
		private readonly HashSet<string> _selectedTemporaryOptions = new HashSet<string>();

		// Token: 0x04000565 RID: 1381
		[Obsolete]
		[DomainData(DomainDataType.SingleValue, false, false, true, false)]
		private ushort _eventCount;

		// Token: 0x04000566 RID: 1382
		[Obsolete("使用DisplayEventType.StartDoctorHeal方式实现")]
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private int _healDoctorCharId = -1;

		// Token: 0x04000567 RID: 1383
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private string _cgName;

		// Token: 0x04000568 RID: 1384
		[DomainData(DomainDataType.SingleValue, false, false, true, false)]
		private EventNotifyData _notifyData;

		// Token: 0x04000569 RID: 1385
		[DomainData(DomainDataType.SingleValue, false, false, true, false)]
		private bool _hasListeningEvent;

		// Token: 0x0400056A RID: 1386
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private EventSelectInformationData _selectInformationData;

		// Token: 0x0400056B RID: 1387
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private EventCricketBettingData _cricketBettingData;

		// Token: 0x0400056C RID: 1388
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private EventSelectCombatSkillData _selectCombatSkillData;

		// Token: 0x0400056D RID: 1389
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private EventSelectLifeSkillData _selectLifeSkillData;

		// Token: 0x0400056E RID: 1390
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _taiwuLocationChangeFlag;

		// Token: 0x0400056F RID: 1391
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private bool _secretVillageOnFire;

		// Token: 0x04000570 RID: 1392
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private bool _taiwuVillageShowShrine;

		// Token: 0x04000571 RID: 1393
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _hideAllTeammates;

		// Token: 0x04000572 RID: 1394
		[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 3)]
		private int[] _allCombatGroupChars;

		// Token: 0x04000573 RID: 1395
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private string _leftRoleAlternativeName;

		// Token: 0x04000574 RID: 1396
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private string _rightRoleAlternativeName;

		// Token: 0x04000575 RID: 1397
		[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 2)]
		private sbyte[] _rightRoleXiangshuDisplayData;

		// Token: 0x04000576 RID: 1398
		[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 3)]
		private ItemDisplayData[] _itemListOfLeft;

		// Token: 0x04000577 RID: 1399
		[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 3)]
		private ItemDisplayData[] _itemListOfRight;

		// Token: 0x04000578 RID: 1400
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _showItemWithCricketBattleGuess;

		// Token: 0x04000579 RID: 1401
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<sbyte> _coverCricketJarGradeListForRight;

		// Token: 0x0400057A RID: 1402
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _marriageLook1CharIdList;

		// Token: 0x0400057B RID: 1403
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _marriageLook2CharIdList;

		// Token: 0x0400057C RID: 1404
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _jieqingMaskCharIdList;

		// Token: 0x0400057D RID: 1405
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[29][];

		// Token: 0x0400057E RID: 1406
		private Queue<uint> _pendingLoadingOperationIds;

		// Token: 0x02000997 RID: 2455
		private enum EEventPackageLoadMethod
		{
			// Token: 0x04002864 RID: 10340
			LoadFile,
			// Token: 0x04002865 RID: 10341
			LoadFrom,
			// Token: 0x04002866 RID: 10342
			LoadBuffer
		}
	}
}
