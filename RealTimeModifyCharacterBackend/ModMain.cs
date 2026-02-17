using System;
using TaiwuModdingLib.Core.Plugin;

namespace RealTimeModifyCharacterBackend
{
	[PluginConfig("实时修改人物", "Yan", "1.0.0")]
	public class ModMain : TaiwuRemakeHarmonyPlugin
	{
		public override void Initialize()
		{
			base.HarmonyInstance.PatchAll(typeof(RealTimeModifyCharacterPatch));
			base.HarmonyInstance.PatchAll(typeof(XuanNuRenaissancePatch));
		}
	}
}
