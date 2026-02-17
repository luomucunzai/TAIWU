using System;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006C8 RID: 1736
	public static class CMath
	{
		// Token: 0x060066EE RID: 26350 RVA: 0x003AF479 File Offset: 0x003AD679
		public static int ClampFatalMarkCount(int markCount)
		{
			return MathUtils.Clamp(markCount, 0, GlobalConfig.Instance.MaxFatalMarkCount);
		}
	}
}
