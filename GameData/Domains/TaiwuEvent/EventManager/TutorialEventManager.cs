using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config.EventConfig;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.TaiwuEvent.EventManager
{
	// Token: 0x020000D1 RID: 209
	public class TutorialEventManager : EventManagerBase
	{
		// Token: 0x06001CC9 RID: 7369 RVA: 0x00186908 File Offset: 0x00184B08
		public TutorialEventManager()
		{
			this._eventDictionary = new Dictionary<string, TaiwuEvent>();
			this._headEventList = new List<TaiwuEvent>();
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x00186928 File Offset: 0x00184B28
		public override TaiwuEvent GetEvent(string eventGuid)
		{
			TaiwuEvent eventData;
			this._eventDictionary.TryGetValue(eventGuid, out eventData);
			return eventData;
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x0018694C File Offset: 0x00184B4C
		public override void HandleEventPackage(EventPackage package)
		{
			List<TaiwuEventItem> list = package.GetEventsByType(EEventType.TutorialEvent);
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
			this._argBox = DomainManager.TaiwuEvent.GetEventArgBox();
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x00186BA8 File Offset: 0x00184DA8
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
			EventArgBox argBox = this._argBox;
			if (argBox != null)
			{
				argBox.Clear();
			}
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x00186C24 File Offset: 0x00184E24
		public override void Reset()
		{
			this._eventDictionary.Clear();
			this._headEventList.Clear();
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x00186C40 File Offset: 0x00184E40
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

		// Token: 0x06001CCF RID: 7375 RVA: 0x00186D30 File Offset: 0x00184F30
		public override void OnEventTrigger_EnterTutorialChapter(short arg0)
		{
			this._argBox.Set("ChapterIndex", arg0);
			DomainManager.TutorialChapter.SetTutorialEventArgBox((int)arg0, this._argBox);
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.EnterTutorialChapter;
				if (flag)
				{
					taiwuEvent.ArgBox = this._argBox;
					bool flag2 = taiwuEvent.EventConfig.CheckCondition();
					if (flag2)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
					}
				}
			}
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x00186DE8 File Offset: 0x00184FE8
		public override void OnEventTrigger_TaiwuBlockChanged(Location arg0, Location arg1)
		{
			this._argBox.Set("BlockFrom", arg0);
			this._argBox.Set("BlockTo", arg1);
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TaiwuBlockChanged;
				if (flag)
				{
					taiwuEvent.ArgBox = this._argBox;
					bool flag2 = taiwuEvent.EventConfig.CheckCondition();
					if (flag2)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x00186EB4 File Offset: 0x001850B4
		public override void OnEventTrigger_BlackMaskAnimationComplete(bool arg0)
		{
			this._argBox.Set("MaskVisible", arg0);
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.BlackMaskAnimationComplete;
				if (flag)
				{
					taiwuEvent.ArgBox = this._argBox;
					bool flag2 = taiwuEvent.EventConfig.CheckCondition();
					if (flag2)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x00186F64 File Offset: 0x00185164
		public override void OnEventTrigger_CombatOpening(int arg0)
		{
			this._argBox.Set("CharacterId", arg0);
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.CombatOpening;
				if (flag)
				{
					taiwuEvent.ArgBox = this._argBox;
					bool flag2 = taiwuEvent.EventConfig.CheckCondition();
					if (flag2)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x00187014 File Offset: 0x00185214
		public override void OnEventTrigger_NewGameMonth()
		{
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.NewGameMonth;
				if (flag)
				{
					taiwuEvent.ArgBox = this._argBox;
					bool flag2 = taiwuEvent.EventConfig.CheckCondition();
					if (flag2)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
			}
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x001870B0 File Offset: 0x001852B0
		public override void OnEventTrigger_TryMoveWhenMoveDisabled()
		{
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TryMoveWhenMoveDisabled;
				if (flag)
				{
					bool flag2 = DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid);
					if (!flag2)
					{
						taiwuEvent.ArgBox = this._argBox;
						bool flag3 = taiwuEvent.EventConfig.CheckCondition();
						if (flag3)
						{
							DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						}
						else
						{
							taiwuEvent.ArgBox = null;
						}
					}
				}
			}
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x00187164 File Offset: 0x00185364
		public override void OnEventTrigger_TryMoveToInvalidLocationInTutorial()
		{
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TryMoveToInvalidLocationInTutorial;
				if (flag)
				{
					bool flag2 = DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid);
					if (!flag2)
					{
						taiwuEvent.ArgBox = this._argBox;
						bool flag3 = taiwuEvent.EventConfig.CheckCondition();
						if (flag3)
						{
							DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						}
						else
						{
							taiwuEvent.ArgBox = null;
						}
					}
				}
			}
		}

		// Token: 0x0400069D RID: 1693
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400069E RID: 1694
		private readonly Dictionary<string, TaiwuEvent> _eventDictionary;

		// Token: 0x0400069F RID: 1695
		private readonly List<TaiwuEvent> _headEventList;

		// Token: 0x040006A0 RID: 1696
		private EventArgBox _argBox;
	}
}
