using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class TeammateCommandCheckerHealPoison : TeammateCommandCheckerBase
{
	protected override bool CheckTeammateAfter => true;

	protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
	{
		if (!context.CurrChar.GetPoison().IsNonZero())
		{
			yield return ETeammateCommandBanReason.HealPoisonNonPoison;
			yield break;
		}
		if (context.TeammateChar.GetHealPoisonCount() <= 0)
		{
			yield return ETeammateCommandBanReason.HealPoisonCountLack;
		}
		if (context.TeammateChar.GetCharacter().GetResource(5) < CombatDomain.GetHealPoisonCostHerb(context.CurrChar.GetPoison()))
		{
			yield return ETeammateCommandBanReason.HealPoisonHerbLack;
		}
		int attainmentLackIndex = 3;
		BoolArray32 healPoisonBanReason = DomainManager.Combat.GetHealPoisonBanReason(context.TeammateChar, context.CurrChar);
		if (((BoolArray32)(ref healPoisonBanReason))[attainmentLackIndex])
		{
			yield return ETeammateCommandBanReason.HealPoisonAttainmentLack;
		}
	}
}
