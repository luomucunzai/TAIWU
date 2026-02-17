using System;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000E5 RID: 229
	public static class SpecialEffectUtils
	{
		// Token: 0x06002955 RID: 10581 RVA: 0x0020044C File Offset: 0x001FE64C
		public static bool HasAzureMarrowMakeLoveEffect(int charId)
		{
			return DomainManager.SpecialEffect.ModifyData(charId, -1, 299, false, -1, -1, -1);
		}
	}
}
