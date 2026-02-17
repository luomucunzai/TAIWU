using System;
using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006B5 RID: 1717
	public static class CombatHealBanReasonHelper
	{
		// Token: 0x0600660B RID: 26123 RVA: 0x003AA2A7 File Offset: 0x003A84A7
		public static IEnumerable<ECombatHealBanReason> ParseReasons(BoolArray32 array)
		{
			bool flag = array[0];
			if (flag)
			{
				yield return ECombatHealBanReason.NonTarget;
			}
			bool flag2 = array[1];
			if (flag2)
			{
				yield return ECombatHealBanReason.CountLack;
			}
			bool flag3 = array[2];
			if (flag3)
			{
				yield return ECombatHealBanReason.HerbLack;
			}
			bool flag4 = array[3];
			if (flag4)
			{
				yield return ECombatHealBanReason.AttainmentLack;
			}
			yield break;
		}
	}
}
