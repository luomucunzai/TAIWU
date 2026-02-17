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
	// Token: 0x020000D0 RID: 208
	public class NpcEventManager : EventManagerBase
	{
		// Token: 0x06001CB7 RID: 7351 RVA: 0x00185AD8 File Offset: 0x00183CD8
		public NpcEventManager()
		{
			this._eventDictionary = new Dictionary<string, TaiwuEvent>();
			this._headEventList = new List<TaiwuEvent>();
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x00185AF8 File Offset: 0x00183CF8
		public override TaiwuEvent GetEvent(string eventGuid)
		{
			TaiwuEvent eventData;
			this._eventDictionary.TryGetValue(eventGuid, out eventData);
			return eventData;
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x00185B1C File Offset: 0x00183D1C
		public override void HandleEventPackage(EventPackage package)
		{
			List<TaiwuEventItem> list = package.GetEventsByType(EEventType.NpcInteractEvent);
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

		// Token: 0x06001CBA RID: 7354 RVA: 0x00185D68 File Offset: 0x00183F68
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

		// Token: 0x06001CBB RID: 7355 RVA: 0x00185DD4 File Offset: 0x00183FD4
		public override void Reset()
		{
			this._eventDictionary.Clear();
			this._headEventList.Clear();
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x00185DF0 File Offset: 0x00183FF0
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

		// Token: 0x06001CBD RID: 7357 RVA: 0x00185EE0 File Offset: 0x001840E0
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

		// Token: 0x06001CBE RID: 7358 RVA: 0x00186000 File Offset: 0x00184200
		public override void OnEventTrigger_CharacterClicked(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.CharacterClicked;
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

		// Token: 0x06001CBF RID: 7359 RVA: 0x001860D8 File Offset: 0x001842D8
		public override void OnEventTrigger_LetTeammateLeaveGroup(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.LetTeammateLeaveGroup;
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

		// Token: 0x06001CC0 RID: 7360 RVA: 0x001861B0 File Offset: 0x001843B0
		public override void OnEventTrigger_CaravanClicked(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.CaravanClicked;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CaravanId", arg0);
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

		// Token: 0x06001CC1 RID: 7361 RVA: 0x00186288 File Offset: 0x00184488
		public override void OnEventTrigger_KidnappedCharacterClicked(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.KidnappedCharacterClicked;
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

		// Token: 0x06001CC2 RID: 7362 RVA: 0x00186360 File Offset: 0x00184560
		public override void OnEventTrigger_TeammateMonthAdvance(int arg0)
		{
			EventArgBox argBox = null;
			int i = 0;
			while (i < this._headEventList.Count)
			{
				TaiwuEvent taiwuEvent = this._headEventList[i];
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TeammateMonthAdvance;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					bool flag3 = DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid);
					if (flag3)
					{
						TaiwuEventItem config = taiwuEvent.EventConfig;
						taiwuEvent = new TaiwuEvent(taiwuEvent);
						taiwuEvent.EventConfig = config;
						taiwuEvent.EventGuid = Guid.NewGuid().ToString();
						argBox.Set("EventInstanceGuid", taiwuEvent.EventGuid);
					}
					argBox.Set("CharacterId", arg0);
					taiwuEvent.ArgBox = argBox;
					bool flag4 = taiwuEvent.EventConfig.CheckCondition();
					if (flag4)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
				IL_E3:
				i++;
				continue;
				goto IL_E3;
			}
			bool flag5 = argBox != null;
			if (flag5)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x00186484 File Offset: 0x00184684
		public override void OnEventTrigger_SameBlockWithTaiwuWhenMonthAdvance(int arg0)
		{
			EventArgBox argBox = null;
			int i = 0;
			while (i < this._headEventList.Count)
			{
				TaiwuEvent taiwuEvent = this._headEventList[i];
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.SameBlockWithTaiwuWhenMonthAdvance;
				if (flag)
				{
					bool flag2 = DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid);
					if (flag2)
					{
						taiwuEvent = new TaiwuEvent(taiwuEvent);
					}
					taiwuEvent.EventGuid = Guid.NewGuid().ToString();
					bool flag3 = argBox == null;
					if (flag3)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterId", arg0);
					taiwuEvent.ArgBox = argBox;
					bool flag4 = taiwuEvent.EventConfig.CheckCondition();
					if (flag4)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
				IL_BF:
				i++;
				continue;
				goto IL_BF;
			}
			bool flag5 = argBox != null;
			if (flag5)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x00186584 File Offset: 0x00184784
		public override void OnEventTrigger_SameBlockWithRandomEnemyOnNewMonth()
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.SameBlockWithRandomEnemyOnNewMonth;
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

		// Token: 0x06001CC5 RID: 7365 RVA: 0x0018664C File Offset: 0x0018484C
		public override void OnEventTrigger_SameBlockWithTaiwuOnNewMonthSpecial(int arg0)
		{
			EventArgBox argBox = null;
			int i = 0;
			while (i < this._headEventList.Count)
			{
				TaiwuEvent taiwuEvent = this._headEventList[i];
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.SameBlockWithTaiwuOnNewMonthSpecial;
				if (flag)
				{
					bool flag2 = DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid);
					if (flag2)
					{
						taiwuEvent = new TaiwuEvent(taiwuEvent);
					}
					taiwuEvent.EventGuid = Guid.NewGuid().ToString();
					bool flag3 = argBox == null;
					if (flag3)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterId", arg0);
					taiwuEvent.ArgBox = argBox;
					bool flag4 = taiwuEvent.EventConfig.CheckCondition();
					if (flag4)
					{
						DomainManager.TaiwuEvent.AddTriggeredEvent(taiwuEvent);
						argBox = null;
					}
					else
					{
						taiwuEvent.ArgBox = null;
					}
				}
				IL_BF:
				i++;
				continue;
				goto IL_BF;
			}
			bool flag5 = argBox != null;
			if (flag5)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x0018674C File Offset: 0x0018494C
		public override void OnEventTrigger_AnimalAvatarClicked(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.AnimalAvatarClicked;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("AnimalId", arg0);
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

		// Token: 0x06001CC7 RID: 7367 RVA: 0x00186824 File Offset: 0x00184A24
		public override void OnEventTrigger_NpcTombClicked(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.NpcTombClicked;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("TombId", arg0);
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

		// Token: 0x0400069A RID: 1690
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400069B RID: 1691
		private Dictionary<string, TaiwuEvent> _eventDictionary;

		// Token: 0x0400069C RID: 1692
		private List<TaiwuEvent> _headEventList;
	}
}
