using System;
using GameData.GameDataBridge;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000A5 RID: 165
	public class InterfaceFunctions
	{
		// Token: 0x06001B0D RID: 6925 RVA: 0x0017AEB0 File Offset: 0x001790B0
		[EventFunction(16)]
		private static void PlayAudio(EventScriptRuntime runtime, string audioName)
		{
			GameDataBridge.AddDisplayEvent<string>(DisplayEventType.SetMainStoryBgm, audioName);
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x0017AEBC File Offset: 0x001790BC
		[EventFunction(197)]
		private static void SpecifyEventBackground(EventScriptRuntime runtime, string backgroundName)
		{
			runtime.ArgBox.Set("ConchShip_PresetKey_SpecifyEventBackground", backgroundName);
		}
	}
}
