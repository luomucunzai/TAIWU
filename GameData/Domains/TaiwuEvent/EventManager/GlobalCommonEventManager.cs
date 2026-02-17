using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config.EventConfig;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.TaiwuEvent.EventManager
{
	// Token: 0x020000CC RID: 204
	public class GlobalCommonEventManager : EventManagerBase
	{
		// Token: 0x06001C48 RID: 7240 RVA: 0x0017FEC2 File Offset: 0x0017E0C2
		public GlobalCommonEventManager()
		{
			this._eventDictionary = new Dictionary<string, TaiwuEvent>();
			this._headEventList = new List<TaiwuEvent>();
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x0017FEE4 File Offset: 0x0017E0E4
		public override TaiwuEvent GetEvent(string eventGuid)
		{
			TaiwuEvent eventData;
			this._eventDictionary.TryGetValue(eventGuid, out eventData);
			return eventData;
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x0017FF08 File Offset: 0x0017E108
		public override void HandleEventPackage(EventPackage package)
		{
			List<TaiwuEventItem> list = package.GetEventsByType(EEventType.GlobalCommonEvent);
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

		// Token: 0x06001C4B RID: 7243 RVA: 0x00180164 File Offset: 0x0017E364
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

		// Token: 0x06001C4C RID: 7244 RVA: 0x001801D0 File Offset: 0x0017E3D0
		public override void Reset()
		{
			this._eventDictionary.Clear();
			this._headEventList.Clear();
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x001801EC File Offset: 0x0017E3EC
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

		// Token: 0x06001C4E RID: 7246 RVA: 0x001802DC File Offset: 0x0017E4DC
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

		// Token: 0x06001C4F RID: 7247 RVA: 0x001803C4 File Offset: 0x0017E5C4
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

		// Token: 0x06001C50 RID: 7248 RVA: 0x0018049C File Offset: 0x0017E69C
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

		// Token: 0x06001C51 RID: 7249 RVA: 0x00180574 File Offset: 0x0017E774
		public override void OnEventTrigger_OnSettlementTreasuryBuildingClicked(short arg0, byte arg1, sbyte arg2)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.OnSettlementTreasuryBuildingClicked;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("BuildingTemplateId", arg0);
					argBox.Set("CustomStatus", arg1);
					argBox.Set("CurrentPage", arg2);
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

		// Token: 0x06001C52 RID: 7250 RVA: 0x0018066C File Offset: 0x0017E86C
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

		// Token: 0x06001C53 RID: 7251 RVA: 0x00180734 File Offset: 0x0017E934
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

		// Token: 0x06001C54 RID: 7252 RVA: 0x00180864 File Offset: 0x0017EA64
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

		// Token: 0x06001C55 RID: 7253 RVA: 0x00180984 File Offset: 0x0017EB84
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

		// Token: 0x06001C56 RID: 7254 RVA: 0x00180A5C File Offset: 0x0017EC5C
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

		// Token: 0x06001C57 RID: 7255 RVA: 0x00180B54 File Offset: 0x0017ED54
		public override void OnEventTrigger_ProfessionExperienceChange(int arg0, int arg1, int arg2)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.ProfessionExperienceChange && !DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid);
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("ProfessionTemplateId", arg0);
					argBox.Set("PrevExp", arg1);
					argBox.Set("NewExp", arg2);
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

		// Token: 0x06001C58 RID: 7256 RVA: 0x00180C60 File Offset: 0x0017EE60
		public override void OnEventTrigger_ProfessionSkillClicked(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.ProfessionSkillClicked;
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

		// Token: 0x06001C59 RID: 7257 RVA: 0x00180D38 File Offset: 0x0017EF38
		public override void OnEventTrigger_TaiwuGotTianjieFulu(int arg0, ItemKey arg1, int arg2)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TaiwuGotTianjieFulu && !DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid);
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterId", arg0);
					argBox.Set("ItemKey", arg1);
					argBox.Set("Count", arg2);
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

		// Token: 0x06001C5A RID: 7258 RVA: 0x00180E48 File Offset: 0x0017F048
		public override void OnEventTrigger_TaiwuSaveCountChange(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TaiwuSaveCountChange && !DomainManager.TaiwuEvent.IsTriggeredEvent(taiwuEvent.EventGuid);
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("SaveCount", arg0);
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

		// Token: 0x06001C5B RID: 7259 RVA: 0x00180F3C File Offset: 0x0017F13C
		public override void OnEventTrigger_CharacterTemplateClicked(short arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.CharacterTemplateClicked;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterTemplateId", arg0);
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

		// Token: 0x06001C5C RID: 7260 RVA: 0x00181014 File Offset: 0x0017F214
		public override void OnEventTrigger_DlcLoongPutJiaoEggs(int arg0, ItemKey arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.DlcLoongPutJiaoEggs;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("PoolId", arg0);
					argBox.Set("EggItemKey", arg1);
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

		// Token: 0x06001C5D RID: 7261 RVA: 0x00181104 File Offset: 0x0017F304
		public override void OnEventTrigger_DlcLoongInteractJiao(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.DlcLoongInteractJiao;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("PoolId", arg0);
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

		// Token: 0x06001C5E RID: 7262 RVA: 0x001811DC File Offset: 0x0017F3DC
		public override void OnEventTrigger_DlcLoongPetJiao(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.DlcLoongPetJiao;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("PoolId", arg0);
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

		// Token: 0x06001C5F RID: 7263 RVA: 0x001812B4 File Offset: 0x0017F4B4
		public override void OnEventTrigger_CloseUI(string arg0, bool arg1, int arg2)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.CloseUI;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("UIName", arg0);
					argBox.Set("PresetBool", arg1);
					argBox.Set("PresetInt", arg2);
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

		// Token: 0x06001C60 RID: 7264 RVA: 0x001813AC File Offset: 0x0017F5AC
		public override void OnEventTrigger_TaiwuFindMaterial(int arg0, TreasureFindResult arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TaiwuFindMaterial;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("BrokenLevel", arg0);
					argBox.Set("FindResult", arg1);
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

		// Token: 0x06001C61 RID: 7265 RVA: 0x0018149C File Offset: 0x0017F69C
		public override void OnEventTrigger_TaiwuFindExtraTreasure(TreasureFindResult arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TaiwuFindExtraTreasure;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("FindResult", arg0);
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

		// Token: 0x06001C62 RID: 7266 RVA: 0x0018157C File Offset: 0x0017F77C
		public override void OnEventTrigger_TaiwuCollectWudangHeavenlyTreeSeed(sbyte arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TaiwuCollectWudangHeavenlyTreeSeed;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("ResourceType", arg0);
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

		// Token: 0x06001C63 RID: 7267 RVA: 0x00181654 File Offset: 0x0017F854
		public override void OnEventTrigger_TaiwuVillagerExpelled(int arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.TaiwuVillagerExpelled;
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

		// Token: 0x06001C64 RID: 7268 RVA: 0x0018172C File Offset: 0x0017F92C
		public override void OnEventTrigger_JingangSectMainStoryReborn()
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.JingangSectMainStoryReborn;
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

		// Token: 0x06001C65 RID: 7269 RVA: 0x001817F4 File Offset: 0x0017F9F4
		public override void OnEventTrigger_JingangSectMainStoryMonkSoul()
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.JingangSectMainStoryMonkSoul;
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

		// Token: 0x06001C66 RID: 7270 RVA: 0x001818BC File Offset: 0x0017FABC
		public override void OnEventTrigger_OperateInventoryItem(int arg0, sbyte arg1, ItemDisplayData arg2)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.OperateInventoryItem;
				if (flag)
				{
					taiwuEvent.EventConfig.IgnoreShowingEvent = true;
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("RoleTaiwu", DomainManager.Taiwu.GetTaiwuCharId());
					argBox.Set("CharacterId", arg0);
					argBox.Set("InventoryItemOperationType", arg1);
					argBox.Set("GetItemKeyCount", arg2.Amount);
					argBox.Set("GetItemKey", arg2.Key);
					argBox.Set("SelectItemKey", arg2.Key);
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

		// Token: 0x06001C67 RID: 7271 RVA: 0x00181A0C File Offset: 0x0017FC0C
		public override void OnEventTrigger_OnShixiangDrumClickedManyTimes()
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.OnShixiangDrumClickedManyTimes;
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

		// Token: 0x06001C68 RID: 7272 RVA: 0x00181AD4 File Offset: 0x0017FCD4
		public override void OnEventTrigger_OnClickedPrisonBtn(short arg0)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.OnClickedPrisonBtn;
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

		// Token: 0x06001C69 RID: 7273 RVA: 0x00181BAC File Offset: 0x0017FDAC
		public override void OnEventTrigger_OnClickedSendPrisonBtn()
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.OnClickedSendPrisonBtn;
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

		// Token: 0x06001C6A RID: 7274 RVA: 0x00181C74 File Offset: 0x0017FE74
		public override void OnEventTrigger_InteractPrisoner(int arg0, int arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.InteractPrisoner;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterId", arg0);
					argBox.Set("InteractPrisonerType", arg1);
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

		// Token: 0x06001C6B RID: 7275 RVA: 0x00181D5C File Offset: 0x0017FF5C
		public override void OnEventTrigger_ClickChicken(int arg0, short arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.ClickChicken;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("ChickenId", arg0);
					argBox.Set("ChickenTemplateId", arg1);
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

		// Token: 0x06001C6C RID: 7276 RVA: 0x00181E44 File Offset: 0x00180044
		public override void OnEventTrigger_SoulWitheringBellTransfer()
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType != EventTrigger.SoulWitheringBellTransfer;
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

		// Token: 0x06001C6D RID: 7277 RVA: 0x00181F08 File Offset: 0x00180108
		public override void OnEventTrigger_CatchThief(sbyte arg0, bool arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType != EventTrigger.CatchThief;
				if (!flag)
				{
					if (argBox == null)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("ThiefLevel", arg0);
					argBox.Set("IsTimeout", arg1);
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

		// Token: 0x06001C6E RID: 7278 RVA: 0x00181FEC File Offset: 0x001801EC
		public override void OnEventTrigger_TaiwuBeHuntedArrivedSect(int characterId)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType != EventTrigger.TaiwuBeHuntedArrivedSect;
				if (!flag)
				{
					if (argBox == null)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterId", characterId);
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

		// Token: 0x06001C6F RID: 7279 RVA: 0x001820C0 File Offset: 0x001802C0
		public override void OnEventTrigger_TaiwuBeHuntedHunterDie(int characterId)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType != EventTrigger.TaiwuBeHuntedHunterDie;
				if (!flag)
				{
					if (argBox == null)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterId", characterId);
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

		// Token: 0x06001C70 RID: 7280 RVA: 0x00182194 File Offset: 0x00180394
		public override void OnEventTrigger_StartSectShaolinDemonSlayer(int bossIndex)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType != EventTrigger.StartSectShaolinDemonSlayer;
				if (!flag)
				{
					if (argBox == null)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("BossIndex", bossIndex);
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

		// Token: 0x06001C71 RID: 7281 RVA: 0x00182268 File Offset: 0x00180468
		public override void OnEventTrigger_TriggerMapPickupEvent(Location location, bool isEvent)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType != EventTrigger.TriggerMapPickupEvent;
				if (!flag)
				{
					if (argBox == null)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("Location", location);
					argBox.Set("IsEvent", isEvent);
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

		// Token: 0x06001C72 RID: 7282 RVA: 0x00182354 File Offset: 0x00180554
		public override void OnEventTrigger_FixedCharacterClicked(int arg0, short arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.FixedCharacterClicked;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterId", arg0);
					argBox.Set("CharacterTemplateId", arg1);
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

		// Token: 0x06001C73 RID: 7283 RVA: 0x0018243C File Offset: 0x0018063C
		public override void OnEventTrigger_FixedEnemyClicked(int arg0, short arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.FixedEnemyClicked;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CharacterId", arg0);
					argBox.Set("CharacterTemplateId", arg1);
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

		// Token: 0x06001C74 RID: 7284 RVA: 0x00182524 File Offset: 0x00180724
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

		// Token: 0x06001C75 RID: 7285 RVA: 0x00182620 File Offset: 0x00180820
		public override void OnEventTrigger_TaiwuDeportVitals(int arg0, bool arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType != EventTrigger.TaiwuDeportVitals;
				if (!flag)
				{
					if (argBox == null)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("VitalType", arg0);
					argBox.Set("IsGoodEnd", arg1);
					DomainManager.Extra.SetCurrentVitalIndex(arg0);
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

		// Token: 0x06001C76 RID: 7286 RVA: 0x00182710 File Offset: 0x00180910
		public override void OnEventTrigger_SwitchToGuardedPage(byte arg0, sbyte arg1)
		{
			EventArgBox argBox = null;
			foreach (TaiwuEvent taiwuEvent in this._headEventList)
			{
				bool flag = taiwuEvent.EventConfig.TriggerType == EventTrigger.SwitchToGuardedPage;
				if (flag)
				{
					bool flag2 = argBox == null;
					if (flag2)
					{
						argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					}
					argBox.Set("CustomStatus", arg0);
					argBox.Set("CurrentPage", arg1);
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

		// Token: 0x0400068F RID: 1679
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04000690 RID: 1680
		private Dictionary<string, TaiwuEvent> _eventDictionary;

		// Token: 0x04000691 RID: 1681
		private List<TaiwuEvent> _headEventList;

		// Token: 0x04000692 RID: 1682
		private EventArgBox _argBox;
	}
}
