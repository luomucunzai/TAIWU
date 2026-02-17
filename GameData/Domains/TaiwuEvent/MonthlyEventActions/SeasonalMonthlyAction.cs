using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.Adventure;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions
{
	// Token: 0x02000095 RID: 149
	[SerializableGameData(NotForDisplayModule = true)]
	public class SeasonalMonthlyAction : MonthlyActionBase, IMonthlyActionGroup, ISerializableGameData
	{
		// Token: 0x06001984 RID: 6532 RVA: 0x0016ECE6 File Offset: 0x0016CEE6
		public SeasonalMonthlyAction(MonthlyActionKey key)
		{
			this.Key = key;
			this._prevSpringDate = 0;
			this._prevSummerDate = 0;
			this._prevAutumnDate = 0;
			this._prevWinterDate = 0;
			this._monthlyActions = new Dictionary<Location, ConfigMonthlyAction>();
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x0016ED20 File Offset: 0x0016CF20
		public override void TriggerAction()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			sbyte currMonth = DomainManager.World.GetCurrMonthInYear();
			sbyte b = currMonth;
			sbyte b2 = b;
			switch (b2)
			{
			case 0:
				DomainManager.World.GetMonthlyNotificationCollection().AddMarketComing(1);
				break;
			case 1:
				this.CreateSpringMarkets(context);
				break;
			case 2:
				this.CreateSummerCombatSkillCompetitions(context);
				break;
			case 3:
			case 5:
			case 6:
			case 7:
				break;
			case 4:
				this.CreateAutumnCricketConference(context);
				break;
			case 8:
				this.CreateWinterLifeSkillCompetitions(context);
				break;
			default:
				if (b2 == 11)
				{
					DomainManager.World.GetMonthlyNotificationCollection().AddMarketComing(2);
				}
				break;
			}
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x0016EDCC File Offset: 0x0016CFCC
		public override void MonthlyHandler()
		{
			bool flag = DomainManager.World.GetMainStoryLineProgress() < 16;
			if (!flag)
			{
				this.TriggerAction();
				foreach (KeyValuePair<Location, ConfigMonthlyAction> keyValuePair in this._monthlyActions)
				{
					Location location;
					ConfigMonthlyAction configMonthlyAction;
					keyValuePair.Deconstruct(out location, out configMonthlyAction);
					ConfigMonthlyAction action = configMonthlyAction;
					action.MonthlyHandler();
				}
			}
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x0016EE54 File Offset: 0x0016D054
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<SeasonalMonthlyAction>(this);
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x0016EE6C File Offset: 0x0016D06C
		public override void FillEventArgBox(EventArgBox eventArgBox)
		{
			Location location;
			eventArgBox.Get<Location>("AdventureLocation", out location);
			bool flag = !location.IsValid();
			if (!flag)
			{
				ConfigMonthlyAction configAction = this.GetConfigAction(location.AreaId, location.BlockId);
				configAction.EnsurePrerequisites();
				configAction.FillEventArgBox(eventArgBox);
			}
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0016EEBC File Offset: 0x0016D0BC
		public override void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
			foreach (KeyValuePair<Location, ConfigMonthlyAction> keyValuePair in this._monthlyActions)
			{
				Location location;
				ConfigMonthlyAction configMonthlyAction;
				keyValuePair.Deconstruct(out location, out configMonthlyAction);
				ConfigMonthlyAction action = configMonthlyAction;
				action.CollectCalledCharacters(calledCharacters);
			}
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x0016EF28 File Offset: 0x0016D128
		public void DeactivateSubAction(short areaId, short blockId, bool isComplete)
		{
			Location location = new Location(areaId, blockId);
			ConfigMonthlyAction action = this._monthlyActions[location];
			action.Deactivate(isComplete);
			this._monthlyActions.Remove(location);
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x0016EF64 File Offset: 0x0016D164
		public ConfigMonthlyAction GetConfigAction(short areaId, short blockId)
		{
			return this._monthlyActions[new Location(areaId, blockId)];
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0016EF88 File Offset: 0x0016D188
		public ConfigMonthlyAction CreateNewConfigAction(short templateId, Location location)
		{
			ConfigMonthlyAction action = new ConfigMonthlyAction(templateId, -1)
			{
				Key = this.Key,
				Location = location
			};
			bool flag = !location.IsValid();
			if (flag)
			{
				action.SelectLocation();
			}
			this._monthlyActions.Add(action.Location, action);
			return action;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0016EFE0 File Offset: 0x0016D1E0
		private void CreateSpringMarkets(DataContext context)
		{
			this._prevSpringDate = DomainManager.World.GetCurrDate();
			List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
			List<short> blockIds = ObjectPool<List<short>>.Instance.Get();
			for (sbyte stateId = 0; stateId < 15; stateId += 1)
			{
				DomainManager.Map.GetStateSettlementIds(stateId, settlementIds, true, false);
				settlementIds.RemoveAll((short id) => !this.IsValidSettlement(id));
				bool flag = settlementIds.Count == 0;
				if (!flag)
				{
					CollectionUtils.Shuffle<short>(context.Random, settlementIds);
					int count = context.Random.Next(3, 6);
					int i = 0;
					while (i < settlementIds.Count && i < count)
					{
						Settlement settlement = DomainManager.Organization.GetSettlement(settlementIds[i]);
						Location location = settlement.GetLocation();
						AreaAdventureData areaAdventures = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
						blockIds.Clear();
						DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, blockIds);
						blockIds.RemoveAll((short blockId) => areaAdventures.AdventureSites.ContainsKey(blockId));
						short blockId2 = blockIds.GetRandom(context.Random);
						DomainManager.Adventure.TryCreateAdventureSite(context, location.AreaId, blockId2, 3, MonthlyActionKey.Invalid);
						i++;
					}
				}
			}
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddMarketAppeared();
			ObjectPool<List<short>>.Instance.Return(settlementIds);
			ObjectPool<List<short>>.Instance.Return(blockIds);
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x0016F174 File Offset: 0x0016D374
		private void CreateSummerCombatSkillCompetitions(DataContext context)
		{
			this._prevSummerDate = DomainManager.World.GetCurrDate();
			List<short> actionIds = ObjectPool<List<short>>.Instance.Get();
			List<short> blockIds = ObjectPool<List<short>>.Instance.Get();
			SeasonalMonthlyAction.<>c__DisplayClass18_0 CS$<>8__locals1;
			CS$<>8__locals1.allSelectableOrgTemplateIds = ObjectPool<HashSet<sbyte>>.Instance.Get();
			List<sbyte> currSelectableOrgTemplateIds = ObjectPool<List<sbyte>>.Instance.Get();
			actionIds.Clear();
			blockIds.Clear();
			CS$<>8__locals1.allSelectableOrgTemplateIds.Clear();
			actionIds.AddRange(ConfigMonthlyActionDefines.CombatSkillTypeToMonthlyAction.Values);
			CollectionUtils.Shuffle<short>(context.Random, actionIds);
			for (sbyte orgTemplateId = 21; orgTemplateId <= 35; orgTemplateId += 1)
			{
				short settlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(orgTemplateId);
				bool flag = this.IsValidSettlement(settlementId);
				if (flag)
				{
					CS$<>8__locals1.allSelectableOrgTemplateIds.Add(orgTemplateId);
				}
			}
			foreach (short actionId in actionIds)
			{
				bool flag2 = !context.Random.CheckPercentProb(20);
				if (!flag2)
				{
					bool flag3 = CS$<>8__locals1.allSelectableOrgTemplateIds.Count == 0;
					if (flag3)
					{
						break;
					}
					SeasonalMonthlyAction.<CreateSummerCombatSkillCompetitions>g__GetCurrSelectableOrgTemplateIds|18_0(MonthlyActions.Instance[actionId].MapBlockSubType, currSelectableOrgTemplateIds, ref CS$<>8__locals1);
					bool flag4 = currSelectableOrgTemplateIds.Count == 0;
					if (flag4)
					{
						break;
					}
					sbyte orgTemplateId2 = currSelectableOrgTemplateIds.GetRandom(context.Random);
					Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId2);
					Location settlementLocation = settlement.GetLocation();
					AreaAdventureData areaAdventures = DomainManager.Adventure.GetAdventuresInArea(settlementLocation.AreaId);
					blockIds.Clear();
					DomainManager.Map.GetSettlementBlocks(settlementLocation.AreaId, settlementLocation.BlockId, blockIds);
					blockIds.RemoveAll((short blockId) => areaAdventures.AdventureSites.ContainsKey(blockId));
					bool flag5 = blockIds.Count == 0;
					if (!flag5)
					{
						short blockId2 = blockIds.GetRandom(context.Random);
						Location location = new Location(settlementLocation.AreaId, blockId2);
						ConfigMonthlyAction action = this.CreateNewConfigAction(actionId, location);
						action.TriggerAction();
						CS$<>8__locals1.allSelectableOrgTemplateIds.Remove(orgTemplateId2);
					}
				}
			}
			ObjectPool<List<short>>.Instance.Return(blockIds);
			ObjectPool<List<short>>.Instance.Return(actionIds);
			ObjectPool<List<sbyte>>.Instance.Return(currSelectableOrgTemplateIds);
			ObjectPool<HashSet<sbyte>>.Instance.Return(CS$<>8__locals1.allSelectableOrgTemplateIds);
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x0016F3F8 File Offset: 0x0016D5F8
		private void CreateAutumnCricketConference(DataContext context)
		{
			int currDate = DomainManager.World.GetCurrDate();
			bool flag = currDate - this._prevAutumnDate < 36;
			if (!flag)
			{
				this._prevAutumnDate = currDate;
				short actionId = 73;
				ConfigMonthlyAction action = this.CreateNewConfigAction(actionId, Location.Invalid);
				action.TriggerAction();
			}
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0016F444 File Offset: 0x0016D644
		private void CreateWinterLifeSkillCompetitions(DataContext context)
		{
			this._prevWinterDate = DomainManager.World.GetCurrDate();
			List<short> actionIds = ObjectPool<List<short>>.Instance.Get();
			actionIds.Clear();
			actionIds.AddRange(ConfigMonthlyActionDefines.MonthlyActionToLifeSkillType.Keys);
			short actionId = actionIds.GetRandom(context.Random);
			ConfigMonthlyAction action = this.CreateNewConfigAction(actionId, Location.Invalid);
			action.TriggerAction();
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0016F4A8 File Offset: 0x0016D6A8
		private bool IsValidSettlement(short settlementId)
		{
			SeasonalMonthlyAction.<>c__DisplayClass21_0 CS$<>8__locals1 = new SeasonalMonthlyAction.<>c__DisplayClass21_0();
			List<short> blockIds = ObjectPool<List<short>>.Instance.Get();
			blockIds.Clear();
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			Location location = settlement.GetLocation();
			CS$<>8__locals1.areaAdventures = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
			bool flag = CS$<>8__locals1.<IsValidSettlement>g__IsBlockIdValid|0(location.BlockId);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, blockIds);
				bool hasValidBlock = blockIds.Exists(new Predicate<short>(CS$<>8__locals1.<IsValidSettlement>g__IsBlockIdValid|0));
				ObjectPool<List<short>>.Instance.Return(blockIds);
				result = hasValidBlock;
			}
			return result;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0016F54E File Offset: 0x0016D74E
		public SeasonalMonthlyAction()
		{
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0016F558 File Offset: 0x0016D758
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x0016F56C File Offset: 0x0016D76C
		public override int GetSerializedSize()
		{
			int totalSize = 28;
			bool flag = this._monthlyActions != null;
			if (flag)
			{
				totalSize += 2;
				foreach (KeyValuePair<Location, ConfigMonthlyAction> keyValuePair in this._monthlyActions)
				{
					Location location2;
					ConfigMonthlyAction configMonthlyAction;
					keyValuePair.Deconstruct(out location2, out configMonthlyAction);
					Location location = location2;
					ConfigMonthlyAction action = configMonthlyAction;
					totalSize += location.GetSerializedSize();
					totalSize += action.GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x0016F618 File Offset: 0x0016D818
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData;
			pCurrData += this.Key.Serialize(pCurrData);
			*pCurrData = (byte)this.State;
			pCurrData++;
			*(int*)pCurrData = this.Month;
			pCurrData += 4;
			*(int*)pCurrData = this.LastFinishDate;
			pCurrData += 4;
			*(int*)pCurrData = this._prevSpringDate;
			pCurrData += 4;
			*(int*)pCurrData = this._prevSummerDate;
			pCurrData += 4;
			*(int*)pCurrData = this._prevAutumnDate;
			pCurrData += 4;
			*(int*)pCurrData = this._prevWinterDate;
			pCurrData += 4;
			bool flag = this._monthlyActions != null;
			if (flag)
			{
				int elementsCount = this._monthlyActions.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				foreach (KeyValuePair<Location, ConfigMonthlyAction> keyValuePair in this._monthlyActions)
				{
					Location location2;
					ConfigMonthlyAction configMonthlyAction;
					keyValuePair.Deconstruct(out location2, out configMonthlyAction);
					Location location = location2;
					ConfigMonthlyAction action = configMonthlyAction;
					pCurrData += location.Serialize(pCurrData);
					pCurrData += action.Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x0016F764 File Offset: 0x0016D964
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + this.Key.Deserialize(pData);
			this.State = *(sbyte*)pCurrData;
			pCurrData++;
			this.Month = *(int*)pCurrData;
			pCurrData += 4;
			this.LastFinishDate = *(int*)pCurrData;
			pCurrData += 4;
			this._prevSpringDate = *(int*)pCurrData;
			pCurrData += 4;
			this._prevSummerDate = *(int*)pCurrData;
			pCurrData += 4;
			this._prevAutumnDate = *(int*)pCurrData;
			pCurrData += 4;
			this._prevWinterDate = *(int*)pCurrData;
			pCurrData += 4;
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = this._monthlyActions == null;
			if (flag)
			{
				this._monthlyActions = new Dictionary<Location, ConfigMonthlyAction>();
			}
			else
			{
				this._monthlyActions.Clear();
			}
			for (int i = 0; i < (int)elementsCount; i++)
			{
				Location location = default(Location);
				ConfigMonthlyAction action = new ConfigMonthlyAction();
				pCurrData += location.Deserialize(pCurrData);
				pCurrData += action.Deserialize(pCurrData);
				this._monthlyActions.Add(location, action);
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0016F880 File Offset: 0x0016DA80
		[CompilerGenerated]
		internal static void <CreateSummerCombatSkillCompetitions>g__GetCurrSelectableOrgTemplateIds|18_0(List<short> blockSubTypes, List<sbyte> orgTemplateIds, ref SeasonalMonthlyAction.<>c__DisplayClass18_0 A_2)
		{
			orgTemplateIds.Clear();
			foreach (short blockId in blockSubTypes)
			{
				sbyte orgTemplateId = (sbyte)(21 + (blockId - 1));
				Tester.Assert(orgTemplateId >= 21 && orgTemplateId <= 35, "");
				bool flag = !A_2.allSelectableOrgTemplateIds.Contains(orgTemplateId);
				if (!flag)
				{
					orgTemplateIds.Add(orgTemplateId);
				}
			}
		}

		// Token: 0x040005C0 RID: 1472
		private const int CombatSkillCompetitionRate = 20;

		// Token: 0x040005C1 RID: 1473
		private const int MinCountPerState = 3;

		// Token: 0x040005C2 RID: 1474
		private const int MaxCountPerState = 5;

		// Token: 0x040005C3 RID: 1475
		[SerializableGameDataField]
		private int _prevSpringDate;

		// Token: 0x040005C4 RID: 1476
		[SerializableGameDataField]
		private int _prevSummerDate;

		// Token: 0x040005C5 RID: 1477
		[SerializableGameDataField]
		private int _prevAutumnDate;

		// Token: 0x040005C6 RID: 1478
		[SerializableGameDataField]
		private int _prevWinterDate;

		// Token: 0x040005C7 RID: 1479
		[SerializableGameDataField]
		private Dictionary<Location, ConfigMonthlyAction> _monthlyActions;
	}
}
