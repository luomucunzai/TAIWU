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
	// Token: 0x020000CF RID: 207
	public class ModEventManager : EventManagerBase
	{
		// Token: 0x06001C93 RID: 7315 RVA: 0x00183C80 File Offset: 0x00181E80
		public ModEventManager()
		{
			this._eventDictionary = new Dictionary<string, TaiwuEvent>();
			this._headEventList = new List<TaiwuEvent>();
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x00183CA0 File Offset: 0x00181EA0
		public override TaiwuEvent GetEvent(string eventGuid)
		{
			TaiwuEvent eventData;
			this._eventDictionary.TryGetValue(eventGuid, out eventData);
			return eventData;
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x00183CC4 File Offset: 0x00181EC4
		public override void HandleEventPackage(EventPackage package)
		{
			List<TaiwuEventItem> list = package.GetEventsByType(EEventType.ModEvent);
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

		// Token: 0x06001C96 RID: 7318 RVA: 0x00183F10 File Offset: 0x00182110
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

		// Token: 0x06001C97 RID: 7319 RVA: 0x00183F7C File Offset: 0x0018217C
		public override void Reset()
		{
			this._eventDictionary.Clear();
			this._headEventList.Clear();
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x00183F98 File Offset: 0x00182198
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

		// Token: 0x06001C99 RID: 7321 RVA: 0x00184088 File Offset: 0x00182288
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

		// Token: 0x06001C9A RID: 7322 RVA: 0x001841A8 File Offset: 0x001823A8
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

		// Token: 0x06001C9B RID: 7323 RVA: 0x00184280 File Offset: 0x00182480
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

		// Token: 0x06001C9C RID: 7324 RVA: 0x00184358 File Offset: 0x00182558
		public override void OnEventTrigger_NeedToPassLegacy(bool arg0, string arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.NeedToPassLegacy;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("IsTaiwuDying", arg0);
					argBox.Set("OnFinishPassingLegacyEvent", arg1);
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

		// Token: 0x06001C9D RID: 7325 RVA: 0x00184440 File Offset: 0x00182640
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

		// Token: 0x06001C9E RID: 7326 RVA: 0x00184518 File Offset: 0x00182718
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

		// Token: 0x06001C9F RID: 7327 RVA: 0x001845F0 File Offset: 0x001827F0
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

		// Token: 0x06001CA0 RID: 7328 RVA: 0x00184714 File Offset: 0x00182914
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

		// Token: 0x06001CA1 RID: 7329 RVA: 0x00184814 File Offset: 0x00182A14
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

		// Token: 0x06001CA2 RID: 7330 RVA: 0x001848DC File Offset: 0x00182ADC
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

		// Token: 0x06001CA3 RID: 7331 RVA: 0x001849DC File Offset: 0x00182BDC
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

		// Token: 0x06001CA4 RID: 7332 RVA: 0x00184AB4 File Offset: 0x00182CB4
		public override void OnEventTrigger_SecretInformationBroadcast(int arg0)
		{
			EventArgBox argBox = null;
			int i = 0;
			while (i < this._headEventList.Count)
			{
				TaiwuEvent taiwuEvent = this._headEventList[i];
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.SecretInformationBroadcast;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("metaDataId", arg0);
					bool flag3 = DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid);
					if (flag3)
					{
						taiwuEvent = new TaiwuEvent
						{
							EventGuid = Guid.NewGuid().ToString(),
							ArgBox = argBox,
							EventConfig = taiwuEvent.EventConfig,
							ExtendEventOptions = new List<ValueTuple<string, string>>(taiwuEvent.ExtendEventOptions)
						};
					}
					else
					{
						taiwuEvent.ArgBox = argBox;
					}
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
				IL_EF:
				i++;
				continue;
				goto IL_EF;
			}
			bool flag5 = argBox != null;
			if (flag5)
			{
				DomainManager.TaiwuEvent.ReturnArgBox(argBox);
			}
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x00184BE4 File Offset: 0x00182DE4
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

		// Token: 0x06001CA6 RID: 7334 RVA: 0x00184CBC File Offset: 0x00182EBC
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

		// Token: 0x06001CA7 RID: 7335 RVA: 0x00184D84 File Offset: 0x00182F84
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

		// Token: 0x06001CA8 RID: 7336 RVA: 0x00184E5C File Offset: 0x0018305C
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

		// Token: 0x06001CA9 RID: 7337 RVA: 0x00184F34 File Offset: 0x00183134
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

		// Token: 0x06001CAA RID: 7338 RVA: 0x00185030 File Offset: 0x00183230
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

		// Token: 0x06001CAB RID: 7339 RVA: 0x00185108 File Offset: 0x00183308
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

		// Token: 0x06001CAC RID: 7340 RVA: 0x001851F8 File Offset: 0x001833F8
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

		// Token: 0x06001CAD RID: 7341 RVA: 0x001852F4 File Offset: 0x001834F4
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

		// Token: 0x06001CAE RID: 7342 RVA: 0x001853C8 File Offset: 0x001835C8
		public override void OnEventTrigger_OnSectSpecialBuildingClicked(short arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.OnSectSpecialBuildingClicked;
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

		// Token: 0x06001CAF RID: 7343 RVA: 0x001854A0 File Offset: 0x001836A0
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

		// Token: 0x06001CB0 RID: 7344 RVA: 0x00185578 File Offset: 0x00183778
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

		// Token: 0x06001CB1 RID: 7345 RVA: 0x00185650 File Offset: 0x00183850
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

		// Token: 0x06001CB2 RID: 7346 RVA: 0x00185738 File Offset: 0x00183938
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

		// Token: 0x06001CB3 RID: 7347 RVA: 0x00185800 File Offset: 0x00183A00
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

		// Token: 0x06001CB4 RID: 7348 RVA: 0x001858D8 File Offset: 0x00183AD8
		public override void OnEventTrigger_LifeSkillCombatForceSilent(int arg0, sbyte arg1, sbyte arg2)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.LifeSkillCombatForceSilent;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterId", arg0);
					argBox.Set("ConcessionCount", arg1);
					argBox.Set("InducementCount", arg2);
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

		// Token: 0x06001CB5 RID: 7349 RVA: 0x001859D0 File Offset: 0x00183BD0
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

		// Token: 0x04000697 RID: 1687
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04000698 RID: 1688
		private Dictionary<string, TaiwuEvent> _eventDictionary;

		// Token: 0x04000699 RID: 1689
		private List<TaiwuEvent> _headEventList;
	}
}
