using System;
using TaiwuModdingLib.Core.Plugin;

namespace RealTimeModifyCharacterBackend
{
	// Token: 0x02000007 RID: 7
	[PluginConfig("实时修改人物", "Yan", "1.0.0")]
	public class ModMain : TaiwuRemakeHarmonyPlugin
	{
		// Token: 0x06000014 RID: 20 RVA: 0x000033A5 File Offset: 0x000015A5
		public override void Initialize()
		{
			base.HarmonyInstance.PatchAll(typeof(RealTimeModifyCharacterPatch));
			base.HarmonyInstance.PatchAll(typeof(XuanNuRenaissancePatch));
		}
	}
}
