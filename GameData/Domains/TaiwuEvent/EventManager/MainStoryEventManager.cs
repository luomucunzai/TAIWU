using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config.EventConfig;
using GameData.Domains.Building;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.TaiwuEvent.EventManager
{
	// Token: 0x020000CE RID: 206
	public class MainStoryEventManager : EventManagerBase
	{
		// Token: 0x06001C7A RID: 7290 RVA: 0x001828A5 File Offset: 0x00180AA5
		public MainStoryEventManager()
		{
			this._eventDictionary = new Dictionary<string, TaiwuEvent>();
			this._headEventList = new List<TaiwuEvent>();
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x001828C8 File Offset: 0x00180AC8
		public override TaiwuEvent GetEvent(string eventGuid)
		{
			TaiwuEvent eventData;
			this._eventDictionary.TryGetValue(eventGuid, out eventData);
			return eventData;
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x001828EC File Offset: 0x00180AEC
		public override void HandleEventPackage(EventPackage package)
		{
			List<TaiwuEventItem> list = package.GetEventsByType(EEventType.MainStoryEvent);
			for (int i = 0; i < list.Count; i++)
			{
				TaiwuEvent taiwuEvent = new TaiwuEvent
				{
					EventGuid = list[i].Guid.ToString(),
					EventConfig = list[i],
					ExtendEventOptions = new List<ValueTuple<string, string>>()
				};
				bool flag = !list[i].IsHeadEvent && list[i].TriggerType != EventTrigger.None;
				if (flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(102, 1);
					defaultInterpolatedStringHandler.AppendLiteral("event ");
					defaultInterpolatedStringHandler.AppendFormatted<Guid>(list[i].Guid);
					defaultInterpolatedStringHandler.AppendLiteral(" selected a TriggerType but IsHeadEvent set as false,this means TriggerType will not take effect");
					AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
				}
				bool flag2 = !this._eventDictionary.ContainsKey(list[i].Guid.ToString());
				if (flag2)
				{
					this._eventDictionary.Add(list[i].Guid.ToString(), taiwuEvent);
					bool isHeadEvent = list[i].IsHeadEvent;
					if (isHeadEvent)
					{
						this._headEventList.Add(taiwuEvent);
					}
				}
				else
				{
					this._eventDictionary[taiwuEvent.EventGuid] = taiwuEvent;
					bool isHeadEvent2 = list[i].IsHeadEvent;
					if (isHeadEvent2)
					{
						int index = this._headEventList.FindIndex((TaiwuEvent e) => e.EventGuid == taiwuEvent.EventGuid);
						bool flag3 = index >= 0;
						if (flag3)
						{
							this._headEventList[index] = taiwuEvent;
						}
						else
						{
							this._headEventList.Add(taiwuEvent);
						}
					}
				}
			}
			foreach (KeyValuePair<string, TaiwuEvent> pair in this._eventDictionary)
			{
				pair.Value.EventConfig.TaiwuEvent = pair.Value;
			}
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x00182B38 File Offset: 0x00180D38
		public override void ClearExtendOptions()
		{
			foreach (KeyValuePair<string, TaiwuEvent> pair in this._eventDictionary)
			{
				List<ValueTuple<string, string>> extendEventOptions = pair.Value.ExtendEventOptions;
				if (extendEventOptions != null)
				{
					extendEventOptions.Clear();
				}
			}
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x00182BA4 File Offset: 0x00180DA4
		public override void Reset()
		{
			this._eventDictionary.Clear();
			this._headEventList.Clear();
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x00182BC0 File Offset: 0x00180DC0
		public override void UnloadPackage(EventPackage package)
		{
			List<string> toRemoveList = new List<string>();
			foreach (KeyValuePair<string, TaiwuEvent> pair in this._eventDictionary)
			{
				bool flag = pair.Value.EventConfig.Package.Equals(package);
				if (flag)
				{
					toRemoveList.Add(pair.Key);
				}
			}
			foreach (string guid in toRemoveList)
			{
				TaiwuEvent taiwuEvent;
				bool flag2 = this._eventDictionary.TryGetValue(guid, out taiwuEvent);
				if (flag2)
				{
					this._eventDictionary.Remove(guid);
					this._headEventList.Remove(taiwuEvent);
				}
			}
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x00182CB0 File Offset: 0x00180EB0
		public override void OnEventTrigger_TaiwuBlockChanged(Location arg0, Location arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TaiwuBlockChanged;
				if (flag)
				{
					bool flag2 = DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid);
					if (flag2)
					{
						bool flag3 = taiwuEvent.ArgBox != null;
						if (flag3)
						{
							continue;
						}
					}
					bool flag4 = argBox == null;
					if (flag4)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("BlockFrom", arg0);
					argBox.Set("BlockTo", arg1);
					taiwuEvent.ArgBox = argBox;
					bool flag5 = taiwuEvent.EventConfig.CheckCondition();
					if (flag5)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag6 = argBox != null;
			if (flag6)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x00182DD0 File Offset: 0x00180FD0
		public override void OnEventTrigger_RecordEnterGame(short arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.RecordEnterGame;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("MainStoryLine", arg0);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x00182EA8 File Offset: 0x001810A8
		public override void OnEventTrigger_NewGameMonth()
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.NewGameMonth;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x00182F70 File Offset: 0x00181170
		public override void OnEventTrigger_CombatWithXiangshuMinionComplete(short arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.CombatWithXiangshuMinionComplete;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("TemplateId", arg0);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x00183048 File Offset: 0x00181248
		public override void OnEventTrigger_BlackMaskAnimationComplete(bool arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.BlackMaskAnimationComplete;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("MaskVisible", arg0);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x00183120 File Offset: 0x00181320
		public override void OnEventTrigger_ConstructComplete(BuildingBlockKey arg0, short arg1, sbyte arg2)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.ConstructComplete;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("BuildingBlockKey", arg0);
					argBox.Set("TemplateId", arg1);
					argBox.Set("Level", arg2);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x0018321C File Offset: 0x0018141C
		public override void OnEventTrigger_CombatOpening(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.CombatOpening;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterId", arg0);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x001832F4 File Offset: 0x001814F4
		public override void OnEventTrigger_MakingSystemOpened(BuildingBlockKey arg0, short arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.MakingSystemOpened;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("BuildingBlockKey", arg0);
					argBox.Set("TemplateId", arg1);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x001833E4 File Offset: 0x001815E4
		public override void OnEventTrigger_SectBuildingClicked(short arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.SectBuildingClicked;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("BuildingTemplateId", arg0);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x001834BC File Offset: 0x001816BC
		public override void OnEventTrigger_CollectedMakingSystemItem(BuildingBlockKey arg0, short arg1, bool arg2)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.CollectedMakingSystemItem;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("BuildingBlockKey", arg0);
					argBox.Set("TemplateId", arg1);
					argBox.Set("ShowingGetItem", arg2);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x001835B8 File Offset: 0x001817B8
		public override void OnEventTrigger_MainStoryFinishCatchCricket(bool arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.MainStoryFinishCatchCricket;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CricketCatchSuccess", arg0);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x00183690 File Offset: 0x00181890
		public override void OnEventTrigger_PurpleBambooAvatarClicked(int arg0, sbyte arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.PurpleBambooAvatarClicked;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterId", arg0);
					argBox.Set("XiangshuAvatarId", arg1);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x00183778 File Offset: 0x00181978
		public override void OnEventTrigger_UserLoadDreamBackArchive()
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.UserLoadDreamBackArchive;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x00183840 File Offset: 0x00181A40
		public override void OnEventTrigger_TaiwuVillageDestroyed()
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TaiwuVillageDestroyed;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.World.SetTaiwuVillageDestroyed();
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x00183914 File Offset: 0x00181B14
		public override void OnEventTrigger_TaiwuCrossArchive()
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TaiwuCrossArchive;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x001839DC File Offset: 0x00181BDC
		public override void OnEventTrigger_TaiwuCrossArchiveFindMemory(sbyte arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TaiwuCrossArchiveFindMemory;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("DreamBackUnlockStateType", arg0);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x00183AB4 File Offset: 0x00181CB4
		public override void OnEventTrigger_ConfirmEnterSwordTomb()
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType != EventTrigger.ConfirmEnterSwordTomb;
				if (!flag)
				{
					if (argBox == null)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					taiwuEvent.ArgBox = argBox;
					bool flag2 = taiwuEvent.EventConfig.CheckCondition();
					if (flag2)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag3 = argBox != null;
			if (flag3)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x00183B78 File Offset: 0x00181D78
		public override void OnEventTrigger_AdventureRemoved(short arg0, Location arg1, bool arg2)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.AdventureRemoved;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("AdventureTemplateId", arg0);
					argBox.Set("AdventureLocation", arg1);
					argBox.Set("IsComplete", arg2);
					taiwuEvent.ArgBox = argBox;
					bool flag3 = taiwuEvent.EventConfig.CheckCondition();
					if (flag3)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
			bool flag4 = argBox != null;
			if (flag4)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x04000694 RID: 1684
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04000695 RID: 1685
		private Dictionary<string, TaiwuEvent> _eventDictionary;

		// Token: 0x04000696 RID: 1686
		private List<TaiwuEvent> _headEventList;
	}
}
