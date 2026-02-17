using System;
using GameData.Domains.Character;
using GameData.Domains.Extra;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.GameDataBridge;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000AA RID: 170
	public class SectMainStoryInternalFunctions
	{
		// Token: 0x06001B2D RID: 6957 RVA: 0x0017B5C0 File Offset: 0x001797C0
		[EventFunction(187)]
		private static void OpenEmeiCombatSkillSpecialBreak(Character character)
		{
			short charTemplateId = character.GetTemplateId();
			if (charTemplateId >= 679)
			{
				if (charTemplateId > 681)
				{
					goto IL_28;
				}
			}
			else if (charTemplateId != 636)
			{
				goto IL_28;
			}
			bool flag = true;
			goto IL_2A;
			IL_28:
			flag = false;
			IL_2A:
			bool isEmeiWhiteGibbon = flag;
			GameDataBridge.AddDisplayEvent<short, bool>(DisplayEventType.OpenCombatSkillSpecialBreak, charTemplateId, isEmeiWhiteGibbon);
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0017B604 File Offset: 0x00179804
		[EventFunction(188)]
		private static void SectStoryEmeiSetMemberInsaneState(EventScriptRuntime runtime, bool isOn)
		{
			if (isOn)
			{
				DomainManager.Extra.SaveArgToSectMainStoryEventArgBox<int>(runtime.Context, 2, "ConchShip_PresetKey_EmeiKillEachOtherStage", 2);
			}
			else
			{
				DomainManager.Extra.RemoveArgToSectMainStoryEventArgBox<int>(runtime.Context, 2, "ConchShip_PresetKey_EmeiKillEachOtherStage");
			}
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x0017B648 File Offset: 0x00179848
		[EventFunction(195)]
		private static void OpenYuanshanMiniGame(EventScriptRuntime runtime, string onFinishEvent, int stage)
		{
			EventHelper.OpenYuanshanMiniGame(stage, onFinishEvent, runtime.Current.ArgBox);
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0017B660 File Offset: 0x00179860
		[EventFunction(196)]
		private static int ProcessYuanshanMiniGameResults(EventScriptRuntime runtime)
		{
			return EventHelper.ProcessYuanshanMiniGameResults(runtime.Current.ArgBox);
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0017B684 File Offset: 0x00179884
		[EventFunction(207)]
		private static bool IsVitalInPrison(EventScriptRuntime runtime, int index)
		{
			return DomainManager.Extra.GetSectYuanshanThreeVitals()[index].IsInPrison;
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x0017B6AC File Offset: 0x001798AC
		[EventFunction(208)]
		private static void SetVitalInPrison(EventScriptRuntime runtime, int index, bool value)
		{
			DomainManager.Extra.SetVitalInPrison(runtime.Context, (SectStoryThreeVitalsCharacterType)index, value);
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x0017B6CF File Offset: 0x001798CF
		[EventFunction(209)]
		private static void PlayVitalAnim(EventScriptRuntime runtime, int index, bool value)
		{
			GameDataBridge.AddDisplayEvent<int, bool>(DisplayEventType.PlayVitalAnim, index, value);
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x0017B6DC File Offset: 0x001798DC
		[EventFunction(211)]
		private static bool AreVitalsDemon(EventScriptRuntime runtime)
		{
			return DomainManager.Extra.AreVitalsDemon();
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x0017B6F8 File Offset: 0x001798F8
		[EventFunction(212)]
		private static int GetCurrentVitalIndex(EventScriptRuntime runtime)
		{
			return DomainManager.Extra.GetCurrentVitalIndex();
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x0017B714 File Offset: 0x00179914
		[EventFunction(214)]
		private static void InitThreeVitals(EventScriptRuntime runtime)
		{
			DomainManager.Extra.InitThreeVitals(runtime.Context);
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x0017B728 File Offset: 0x00179928
		[EventFunction(244)]
		private static void SectMainStoryUnlockUI(EventScriptRuntime runtime, sbyte organizationId, string afterEvent)
		{
			EventHelper.ShowSectMainStoryUnlock(organizationId, afterEvent, runtime.Current.ArgBox);
		}
	}
}
