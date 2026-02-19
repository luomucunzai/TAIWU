using GameData.Domains.Character;
using GameData.Domains.Extra;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.GameDataBridge;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class SectMainStoryInternalFunctions
{
	[EventFunction(187)]
	private static void OpenEmeiCombatSkillSpecialBreak(GameData.Domains.Character.Character character)
	{
		short templateId = character.GetTemplateId();
		bool flag;
		switch (templateId)
		{
		case 636:
		case 679:
		case 680:
		case 681:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		bool arg = flag;
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenCombatSkillSpecialBreak, templateId, arg);
	}

	[EventFunction(188)]
	private static void SectStoryEmeiSetMemberInsaneState(EventScriptRuntime runtime, bool isOn)
	{
		if (isOn)
		{
			DomainManager.Extra.SaveArgToSectMainStoryEventArgBox(runtime.Context, 2, "ConchShip_PresetKey_EmeiKillEachOtherStage", 2);
		}
		else
		{
			DomainManager.Extra.RemoveArgToSectMainStoryEventArgBox<int>(runtime.Context, 2, "ConchShip_PresetKey_EmeiKillEachOtherStage");
		}
	}

	[EventFunction(195)]
	private static void OpenYuanshanMiniGame(EventScriptRuntime runtime, string onFinishEvent, int stage)
	{
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.OpenYuanshanMiniGame(stage, onFinishEvent, runtime.Current.ArgBox);
	}

	[EventFunction(196)]
	private static int ProcessYuanshanMiniGameResults(EventScriptRuntime runtime)
	{
		return GameData.Domains.TaiwuEvent.EventHelper.EventHelper.ProcessYuanshanMiniGameResults(runtime.Current.ArgBox);
	}

	[EventFunction(207)]
	private static bool IsVitalInPrison(EventScriptRuntime runtime, int index)
	{
		return DomainManager.Extra.GetSectYuanshanThreeVitals()[index].IsInPrison;
	}

	[EventFunction(208)]
	private static void SetVitalInPrison(EventScriptRuntime runtime, int index, bool value)
	{
		DomainManager.Extra.SetVitalInPrison(runtime.Context, (SectStoryThreeVitalsCharacterType)index, value);
	}

	[EventFunction(209)]
	private static void PlayVitalAnim(EventScriptRuntime runtime, int index, bool value)
	{
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.PlayVitalAnim, index, value);
	}

	[EventFunction(211)]
	private static bool AreVitalsDemon(EventScriptRuntime runtime)
	{
		return DomainManager.Extra.AreVitalsDemon();
	}

	[EventFunction(212)]
	private static int GetCurrentVitalIndex(EventScriptRuntime runtime)
	{
		return DomainManager.Extra.GetCurrentVitalIndex();
	}

	[EventFunction(214)]
	private static void InitThreeVitals(EventScriptRuntime runtime)
	{
		DomainManager.Extra.InitThreeVitals(runtime.Context);
	}

	[EventFunction(244)]
	private static void SectMainStoryUnlockUI(EventScriptRuntime runtime, sbyte organizationId, string afterEvent)
	{
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.ShowSectMainStoryUnlock(organizationId, afterEvent, runtime.Current.ArgBox);
	}
}
