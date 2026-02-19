using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public static class CombatHealBanReasonHelper
{
	public static IEnumerable<ECombatHealBanReason> ParseReasons(BoolArray32 array)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (((BoolArray32)(ref array))[0])
		{
			yield return ECombatHealBanReason.NonTarget;
		}
		if (((BoolArray32)(ref array))[1])
		{
			yield return ECombatHealBanReason.CountLack;
		}
		if (((BoolArray32)(ref array))[2])
		{
			yield return ECombatHealBanReason.HerbLack;
		}
		if (((BoolArray32)(ref array))[3])
		{
			yield return ECombatHealBanReason.AttainmentLack;
		}
	}
}
