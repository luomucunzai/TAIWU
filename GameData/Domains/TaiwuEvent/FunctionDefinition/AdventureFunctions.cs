using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000A0 RID: 160
	public class AdventureFunctions
	{
		// Token: 0x06001A58 RID: 6744 RVA: 0x001774E0 File Offset: 0x001756E0
		[EventFunction(145)]
		private static void CreateConfigMonthlyAction(EventScriptRuntime runtime, short monthlyActionId, short areaTemplateId)
		{
			short areaId = DomainManager.Map.GetAreaIdByAreaTemplateId(areaTemplateId);
			DomainManager.TaiwuEvent.AddWrappedConfigAction(runtime.Context, monthlyActionId, areaId);
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00177510 File Offset: 0x00175710
		[EventFunction(67)]
		private static void CreateAdventureSite(EventScriptRuntime runtime, short adventureId, MapBlockData mapBlockData)
		{
			Location location = (mapBlockData != null) ? mapBlockData.GetLocation() : DomainManager.Taiwu.GetTaiwu().GetValidLocation();
			AdventureFunctions.InitializeAdventureIdToMonthlyActionMap();
			bool flag = AdventureFunctions._adventureIdWithMonthlyActionSet.Contains(adventureId);
			if (flag)
			{
				AdaptableLog.Warning(Adventure.Instance[adventureId].Name + " should be created with a monthly action. Use instead.", false);
			}
			DomainManager.Adventure.TryCreateAdventureSite(runtime.Context, location.AreaId, location.BlockId, adventureId, MonthlyActionKey.Invalid);
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00177594 File Offset: 0x00175794
		private static void InitializeAdventureIdToMonthlyActionMap()
		{
			bool flag = AdventureFunctions._adventureIdWithMonthlyActionSet != null;
			if (!flag)
			{
				AdventureFunctions._adventureIdWithMonthlyActionSet = new HashSet<short>();
				foreach (MonthlyActionsItem monthlyActionCfg in ((IEnumerable<MonthlyActionsItem>)MonthlyActions.Instance))
				{
					bool flag2 = monthlyActionCfg.AdventureId >= 0;
					if (flag2)
					{
						AdventureFunctions._adventureIdWithMonthlyActionSet.Add(monthlyActionCfg.AdventureId);
					}
				}
			}
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x00177618 File Offset: 0x00175818
		[EventFunction(119)]
		private static void GenerateAdventureMap(EventScriptRuntime runtime, string startNodeKey)
		{
			DomainManager.Adventure.GenerateAdventureMap(runtime.Context, startNodeKey);
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x00177630 File Offset: 0x00175830
		[EventFunction(124)]
		private static void ExitAdventure(EventScriptRuntime runtime, bool isAdventureCompleted, string postAdventureEvent = null)
		{
			short adventureId = DomainManager.Adventure.GetCurAdventureId();
			bool flag = adventureId < 0;
			if (!flag)
			{
				DomainManager.Adventure.ExitAdventure(runtime.Context, isAdventureCompleted);
				bool flag2 = !string.IsNullOrEmpty(postAdventureEvent);
				if (flag2)
				{
					EventArgBox argBox = DomainManager.TaiwuEvent.GetEventArgBox();
					argBox.Set("AdventureId", adventureId);
					argBox.Set("IsComplete", isAdventureCompleted);
					DomainManager.TaiwuEvent.SetListenerWithActionName(postAdventureEvent, argBox, "ExitAdventure");
				}
				GameDataBridge.AddDisplayEvent(DisplayEventType.ExitAdventure);
			}
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x001776B2 File Offset: 0x001758B2
		[EventFunction(125)]
		private static void FinishAdventureEvent(EventScriptRuntime runtime)
		{
			DomainManager.Adventure.OnEventHandleFinished(runtime.Context);
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x001776C6 File Offset: 0x001758C6
		[EventFunction(126)]
		private static void SelectAdventureBranch(EventScriptRuntime runtime, string branchKey)
		{
			DomainManager.Adventure.SelectBranch(runtime.Context, branchKey);
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x001776DC File Offset: 0x001758DC
		[EventFunction(144)]
		private static int GetAdventureCharacterCount(EventScriptRuntime runtime, bool isMajor, int groupId)
		{
			return isMajor ? runtime.Current.ArgBox.GetAdventureMajorCharacterCount(groupId) : runtime.Current.ArgBox.GetAdventureParticipateCharacterCount(groupId);
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x00177718 File Offset: 0x00175918
		[EventFunction(143)]
		private static int GetAdventureCharacter(EventScriptRuntime runtime, bool isMajor, int groupId, int index)
		{
			return isMajor ? runtime.Current.ArgBox.GetAdventureMajorCharacter(groupId, index).GetId() : runtime.Current.ArgBox.GetAdventureParticipateCharacter(groupId, index).GetId();
		}

		// Token: 0x04000610 RID: 1552
		private static HashSet<short> _adventureIdWithMonthlyActionSet;
	}
}
