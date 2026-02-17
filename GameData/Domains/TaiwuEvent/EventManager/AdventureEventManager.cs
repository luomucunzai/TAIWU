using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using Config.EventConfig;
using GameData.Domains.Adventure;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.TaiwuEvent.EventManager
{
	// Token: 0x020000CA RID: 202
	internal class AdventureEventManager : EventManagerBase
	{
		// Token: 0x06001BEA RID: 7146 RVA: 0x0017E99A File Offset: 0x0017CB9A
		public AdventureEventManager()
		{
			this._eventDictionary = new Dictionary<string, TaiwuEvent>();
			this._monitoringEventDictionary = new Dictionary<string, TaiwuEvent>();
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x0017E9BC File Offset: 0x0017CBBC
		public void SetNewAdventureByConfigData(AdventureItem configData, EventArgBox argBox)
		{
			this._adventureConfig = configData;
			this._argBox = argBox;
			AdventureEventManager.Logger.Info("SetNewAdventureByConfigData: " + configData.Name);
			this._monitoringEventDictionary.Clear();
			for (int i = 0; i < this._adventureConfig.StartNodes.Count; i++)
			{
				this.AddEvent(this._adventureConfig.StartNodes[i].EventId.ToString());
			}
			for (int j = 0; j < this._adventureConfig.TransferNodes.Count; j++)
			{
				this.AddEvent(this._adventureConfig.TransferNodes[j].EventId.ToString());
			}
			for (int k = 0; k < this._adventureConfig.EndNodes.Count; k++)
			{
				this.AddEvent(this._adventureConfig.EndNodes[k].EventId.ToString());
			}
			foreach (AdventureBaseBranch baseBranch in this._adventureConfig.BaseBranches)
			{
				this.AddEvent(baseBranch.GlobalEvent);
				foreach (AdventurePersonalityContentWeights personality in baseBranch.PersonalityContentWeights)
				{
					foreach (ValueTuple<string, short> eventWeight in personality.EventWeights)
					{
						this.AddEvent(eventWeight.Item1);
					}
					foreach (ValueTuple<string, short> bonusWeight in personality.BonusWeights)
					{
						this.AddEvent(bonusWeight.Item1);
					}
				}
			}
			foreach (AdventureAdvancedBranch advancedBranch in this._adventureConfig.AdvancedBranches)
			{
				this.AddEvent(advancedBranch.GlobalEvent);
				foreach (AdventurePersonalityContentWeights personality2 in advancedBranch.PersonalityContentWeights)
				{
					foreach (ValueTuple<string, short> eventWeight2 in personality2.EventWeights)
					{
						this.AddEvent(eventWeight2.Item1);
					}
					foreach (ValueTuple<string, short> bonusWeight2 in personality2.BonusWeights)
					{
						this.AddEvent(bonusWeight2.Item1);
					}
				}
			}
			for (int l = 0; l < this._adventureConfig.AdventureParams.Count; l++)
			{
				string argName = this._adventureConfig.AdventureParams[l].Item1;
				bool flag = !string.IsNullOrEmpty(argName);
				if (flag)
				{
					EventHelper.SetAdventureParameter(argName, 0);
				}
			}
			AdventureEventManager.Logger.Info("Trigger enter event: " + this._adventureConfig.EnterEvent);
			TaiwuEvent enterEvent = this.GetEvent(this._adventureConfig.EnterEvent);
			bool flag2 = enterEvent == null;
			if (flag2)
			{
				throw new Exception("Failed to find enter event " + this._adventureConfig.EnterEvent + " of adventure " + this._adventureConfig.Name);
			}
			enterEvent.ArgBox = this._argBox;
			bool flag3 = enterEvent.EventConfig.CheckCondition();
			if (flag3)
			{
				DomainManager.TaiwuEvent.AddTriggeredEvent(enterEvent);
			}
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x0017EDB0 File Offset: 0x0017CFB0
		public void TriggerGlobalEvent(AdventureMapPoint curNode)
		{
			int curBranchId = DomainManager.Adventure.GetCurMapTrunk().BranchIndex;
			string globalEventGuid = (curBranchId < this._adventureConfig.BaseBranches.Count) ? this._adventureConfig.BaseBranches[curBranchId].GlobalEvent : this._adventureConfig.AdvancedBranches[curBranchId - this._adventureConfig.BaseBranches.Count].GlobalEvent;
			bool flag = string.IsNullOrEmpty(globalEventGuid);
			if (!flag)
			{
				TaiwuEvent globalEvent = this.GetEvent(globalEventGuid);
				this._argBox.Set("AdventurePoint", curNode);
				globalEvent.ArgBox = this._argBox;
				bool flag2 = globalEvent.EventConfig.CheckCondition();
				if (flag2)
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(globalEvent);
				}
			}
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x0017EE74 File Offset: 0x0017D074
		public void TriggerExtraEvent(string guid, AdventureMapPoint curNode)
		{
			bool flag = curNode.NodeContentType != 10;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Incorrect node content type for TriggerExtraEvent: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(curNode.NodeContentType);
				defaultInterpolatedStringHandler.AppendLiteral(" given.");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag2 = string.IsNullOrEmpty(guid);
			if (!flag2)
			{
				TaiwuEvent globalEvent = this.GetEvent(guid);
				bool flag3 = globalEvent.EventConfig.TriggerType != EventTrigger.AdventureEnterNode;
				if (flag3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Incorrect event trigger type for TriggerExtraEvent with event ");
					defaultInterpolatedStringHandler.AppendFormatted(guid);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(globalEvent.EventConfig.TriggerType);
					defaultInterpolatedStringHandler.AppendLiteral(" given.");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this._argBox.Set("AdventurePoint", curNode);
				globalEvent.ArgBox = this._argBox;
				bool flag4 = globalEvent.EventConfig.CheckCondition();
				if (flag4)
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(globalEvent);
				}
			}
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x0017EF9C File Offset: 0x0017D19C
		public void SetNewAdventure(short adventureId, EventArgBox argBox)
		{
			this._adventureConfig = Adventure.Instance.GetItem(adventureId);
			bool flag = this._adventureConfig == null;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (flag)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("failed get adventure config of id ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(adventureId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Logger logger = AdventureEventManager.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 2);
			defaultInterpolatedStringHandler.AppendLiteral("SetNewAdventure: ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(adventureId);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted(this._adventureConfig.Name);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			this._argBox = argBox;
			this._monitoringEventDictionary.Clear();
			for (int i = 0; i < this._adventureConfig.StartNodes.Count; i++)
			{
				this.AddEvent(this._adventureConfig.StartNodes[i].EventId.ToString());
			}
			for (int j = 0; j < this._adventureConfig.TransferNodes.Count; j++)
			{
				this.AddEvent(this._adventureConfig.TransferNodes[j].EventId.ToString());
			}
			for (int k = 0; k < this._adventureConfig.EndNodes.Count; k++)
			{
				this.AddEvent(this._adventureConfig.EndNodes[k].EventId.ToString());
			}
			foreach (AdventureBaseBranch baseBranch in this._adventureConfig.BaseBranches)
			{
				this.AddEvent(baseBranch.GlobalEvent);
				foreach (AdventurePersonalityContentWeights personality in baseBranch.PersonalityContentWeights)
				{
					foreach (ValueTuple<string, short> eventWeight in personality.EventWeights)
					{
						this.AddEvent(eventWeight.Item1);
					}
					foreach (ValueTuple<string, short> bonusWeight in personality.BonusWeights)
					{
						this.AddEvent(bonusWeight.Item1);
					}
				}
			}
			foreach (AdventureAdvancedBranch advancedBranch in this._adventureConfig.AdvancedBranches)
			{
				this.AddEvent(advancedBranch.GlobalEvent);
				foreach (AdventurePersonalityContentWeights personality2 in advancedBranch.PersonalityContentWeights)
				{
					foreach (ValueTuple<string, short> eventWeight2 in personality2.EventWeights)
					{
						this.AddEvent(eventWeight2.Item1);
					}
					foreach (ValueTuple<string, short> bonusWeight2 in personality2.BonusWeights)
					{
						this.AddEvent(bonusWeight2.Item1);
					}
				}
			}
			for (int l = 0; l < this._adventureConfig.AdventureParams.Count; l++)
			{
				string argName = this._adventureConfig.AdventureParams[l].Item1;
				bool flag2 = !string.IsNullOrEmpty(argName);
				if (flag2)
				{
					EventHelper.SetAdventureParameter(argName, 0);
				}
			}
			AdventureEventManager.Logger.Info("Trigger enter event: " + this._adventureConfig.EnterEvent);
			TaiwuEvent enterEvent = this.GetEvent(this._adventureConfig.EnterEvent);
			bool flag3 = enterEvent == null;
			if (flag3)
			{
				throw new Exception("Failed to find enter event " + this._adventureConfig.EnterEvent + " of adventure " + this._adventureConfig.Name);
			}
			enterEvent.ArgBox = this._argBox;
			bool flag4 = enterEvent.EventConfig.CheckCondition();
			if (flag4)
			{
				DomainManager.TaiwuEvent.AddTriggeredEvent(enterEvent);
			}
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x0017F414 File Offset: 0x0017D614
		public void AddEvent(string eventGuid)
		{
			bool flag = string.IsNullOrEmpty(eventGuid);
			if (!flag)
			{
				bool flag2 = this._monitoringEventDictionary.ContainsKey(eventGuid);
				if (!flag2)
				{
					TaiwuEvent eventItem = this.GetEvent(eventGuid);
					bool flag3 = eventItem != null;
					if (flag3)
					{
						this._monitoringEventDictionary.Add(eventItem.EventGuid, eventItem);
					}
				}
			}
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x0017F464 File Offset: 0x0017D664
		public override TaiwuEvent GetEvent(string eventGuid)
		{
			TaiwuEvent eventData;
			this._eventDictionary.TryGetValue(eventGuid, out eventData);
			return eventData;
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x0017F488 File Offset: 0x0017D688
		public override void HandleEventPackage(EventPackage package)
		{
			List<TaiwuEventItem> list = package.GetEventsByType(EEventType.AdventureEvent);
			for (int i = 0; i < list.Count; i++)
			{
				TaiwuEvent taiwuEvent = new TaiwuEvent
				{
					EventGuid = list[i].Guid.ToString(),
					EventConfig = list[i],
					ExtendEventOptions = new List<ValueTuple<string, string>>()
				};
				bool flag = !this._eventDictionary.ContainsKey(list[i].Guid.ToString());
				if (flag)
				{
					this._eventDictionary.Add(list[i].Guid.ToString(), taiwuEvent);
				}
				else
				{
					this._eventDictionary[taiwuEvent.EventGuid] = taiwuEvent;
				}
			}
			foreach (KeyValuePair<string, TaiwuEvent> pair in this._eventDictionary)
			{
				pair.Value.EventConfig.TaiwuEvent = pair.Value;
			}
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x0017F5B8 File Offset: 0x0017D7B8
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

		// Token: 0x06001BF3 RID: 7155 RVA: 0x0017F624 File Offset: 0x0017D824
		public override void Reset()
		{
			this._eventDictionary.Clear();
			this._monitoringEventDictionary.Clear();
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x0017F640 File Offset: 0x0017D840
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
				this._eventDictionary.Remove(guid);
				this._monitoringEventDictionary.Remove(guid);
			}
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x0017F718 File Offset: 0x0017D918
		public override void OnEventTrigger_AdventureReachStartNode(short arg0)
		{
			AdventureStartNode node = this._adventureConfig.StartNodes[(int)arg0];
			this._argBox.Set("AdventureNodeKey", node.NodeKey);
			TaiwuEvent eventItem;
			bool flag = this._monitoringEventDictionary.TryGetValue(node.EventId.ToString(), out eventItem);
			if (flag)
			{
				bool flag2 = eventItem.EventConfig.TriggerType != EventTrigger.AdventureReachStartNode;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(90, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Incorrect event trigger type for EventTrigger_AdventureReachStartNode with event ");
					defaultInterpolatedStringHandler.AppendFormatted(eventItem.EventGuid);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(eventItem.EventConfig.TriggerType);
					defaultInterpolatedStringHandler.AppendLiteral(" given.");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				eventItem.ArgBox = this._argBox;
				bool flag3 = eventItem.EventConfig.CheckCondition();
				if (flag3)
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(eventItem);
				}
			}
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x0017F818 File Offset: 0x0017DA18
		public override void OnEventTrigger_AdventureReachTransferNode(short arg0)
		{
			int startNodeLength = this._adventureConfig.StartNodes.Count;
			AdventureTransferNode node = this._adventureConfig.TransferNodes[(int)arg0 - startNodeLength];
			this._argBox.Set("AdventureNodeKey", node.NodeKey);
			TaiwuEvent eventItem;
			bool flag = this._monitoringEventDictionary.TryGetValue(node.EventId.ToString(), out eventItem);
			if (flag)
			{
				bool flag2 = eventItem.EventConfig.TriggerType != EventTrigger.AdventureReachTransferNode;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(93, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Incorrect event trigger type for EventTrigger_AdventureReachTransferNode with event ");
					defaultInterpolatedStringHandler.AppendFormatted(eventItem.EventGuid);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(eventItem.EventConfig.TriggerType);
					defaultInterpolatedStringHandler.AppendLiteral(" given.");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				eventItem.ArgBox = this._argBox;
				bool flag3 = eventItem.EventConfig.CheckCondition();
				if (flag3)
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(eventItem);
				}
			}
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x0017F930 File Offset: 0x0017DB30
		public override void OnEventTrigger_AdventureReachEndNode(short arg0)
		{
			int startNodeLength = this._adventureConfig.StartNodes.Count;
			int transferNodeLength = this._adventureConfig.TransferNodes.Count;
			AdventureEndNode node = this._adventureConfig.EndNodes[(int)arg0 - startNodeLength - transferNodeLength];
			this._argBox.Set("AdventureNodeKey", node.NodeKey);
			TaiwuEvent eventItem;
			bool flag = this._monitoringEventDictionary.TryGetValue(node.EventId.ToString(), out eventItem);
			if (flag)
			{
				bool flag2 = eventItem.EventConfig.TriggerType != EventTrigger.AdventureReachEndNode;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(88, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Incorrect event trigger type for EventTrigger_AdventureReachEndNode with event ");
					defaultInterpolatedStringHandler.AppendFormatted(eventItem.EventGuid);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(eventItem.EventConfig.TriggerType);
					defaultInterpolatedStringHandler.AppendLiteral(" given.");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				eventItem.ArgBox = this._argBox;
				bool flag3 = eventItem.EventConfig.CheckCondition();
				if (flag3)
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(eventItem);
				}
			}
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x0017FA5C File Offset: 0x0017DC5C
		public override void OnEventTrigger_AdventureEnterNode(AdventureMapPoint arg0)
		{
			this._argBox.Set("AdventurePoint", arg0);
			int eventIndex = arg0.NodeContentIndex;
			int branchIndex = (int)arg0.AffiliatedBranchIdx;
			AdventurePersonalityContentWeights[] branchContentWeights = (branchIndex < this._adventureConfig.BaseBranches.Count) ? this._adventureConfig.BaseBranches[branchIndex].PersonalityContentWeights : this._adventureConfig.AdvancedBranches[branchIndex - this._adventureConfig.BaseBranches.Count].PersonalityContentWeights;
			bool flag = !branchContentWeights.CheckIndex((int)arg0.SevenElementType);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid seven element type ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(arg0.SevenElementType);
				defaultInterpolatedStringHandler.AppendLiteral(" of node ");
				defaultInterpolatedStringHandler.AppendFormatted(arg0.GetDetailedInfo());
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			AdventurePersonalityContentWeights sevenElementWeights = branchContentWeights[(int)arg0.SevenElementType];
			sbyte nodeContentType = arg0.NodeContentType;
			if (!true)
			{
			}
			ValueTuple<string, short>[] array;
			if (nodeContentType != 0)
			{
				if (nodeContentType != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Incorrect node content type for EventTrigger_AdventureEnterNode: ");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(arg0.NodeContentType);
					defaultInterpolatedStringHandler.AppendLiteral(" given.");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				array = sevenElementWeights.BonusWeights;
			}
			else
			{
				array = sevenElementWeights.EventWeights;
			}
			if (!true)
			{
			}
			ValueTuple<string, short>[] eventWeights = array;
			bool flag2 = !eventWeights.CheckIndex(eventIndex);
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid eventIndex ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(eventIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" of node ");
				defaultInterpolatedStringHandler.AppendFormatted(arg0.GetDetailedInfo());
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			string eventGuid = eventWeights[eventIndex].Item1;
			TaiwuEvent eventItem;
			bool flag3 = this._monitoringEventDictionary.TryGetValue(eventGuid, out eventItem);
			if (flag3)
			{
				bool flag4 = eventItem.EventConfig.TriggerType != EventTrigger.AdventureEnterNode;
				if (flag4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(85, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Incorrect event trigger type for EventTrigger_AdventureEnterNode with event ");
					defaultInterpolatedStringHandler.AppendFormatted(eventGuid);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(eventItem.EventConfig.TriggerType);
					defaultInterpolatedStringHandler.AppendLiteral(" given.");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				eventItem.ArgBox = this._argBox;
				bool flag5 = eventItem.EventConfig.CheckCondition();
				if (flag5)
				{
					DomainManager.TaiwuEvent.AddTriggeredEvent(eventItem);
				}
			}
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x0017FCE4 File Offset: 0x0017DEE4
		public override void OnEventTrigger_CombatOpening(int arg0)
		{
			bool flag = this._argBox == null || this._monitoringEventDictionary.Count <= 0;
			if (!flag)
			{
				this._argBox.Set("CharacterId", arg0);
				foreach (KeyValuePair<string, TaiwuEvent> pair in this._monitoringEventDictionary)
				{
					bool flag2 = pair.Value.EventConfig.TriggerType == EventTrigger.CombatOpening;
					if (flag2)
					{
						pair.Value.ArgBox = this._argBox;
						bool flag3 = pair.Value.EventConfig.CheckCondition();
						if (flag3)
						{
							DomainManager.TaiwuEvent.AddTriggeredEvent(pair.Value);
						}
						else
						{
							pair.Value.ArgBox = null;
						}
					}
				}
			}
		}

		// Token: 0x0400068A RID: 1674
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400068B RID: 1675
		private Dictionary<string, TaiwuEvent> _eventDictionary;

		// Token: 0x0400068C RID: 1676
		private Dictionary<string, TaiwuEvent> _monitoringEventDictionary;

		// Token: 0x0400068D RID: 1677
		private AdventureItem _adventureConfig;

		// Token: 0x0400068E RID: 1678
		private EventArgBox _argBox;
	}
}
