using GameData.GameDataBridge;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class InterfaceFunctions
{
	[EventFunction(16)]
	private static void PlayAudio(EventScriptRuntime runtime, string audioName)
	{
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.SetMainStoryBgm, audioName);
	}

	[EventFunction(197)]
	private static void SpecifyEventBackground(EventScriptRuntime runtime, string backgroundName)
	{
		runtime.ArgBox.Set("ConchShip_PresetKey_SpecifyEventBackground", backgroundName);
	}
}
