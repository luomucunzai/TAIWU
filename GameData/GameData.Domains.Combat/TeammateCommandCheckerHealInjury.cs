using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class TeammateCommandCheckerHealInjury : TeammateCommandCheckerBase
{
	protected override bool CheckTeammateAfter => true;

	protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
	{
		if (!context.CurrChar.GetInjuries().HasAnyInjury())
		{
			yield return ETeammateCommandBanReason.HealInjuryNonInjury;
			yield break;
		}
		if (context.TeammateChar.GetHealInjuryCount() <= 0)
		{
			yield return ETeammateCommandBanReason.HealInjuryCountLack;
		}
		if (context.TeammateChar.GetCharacter().GetResource(5) < CombatDomain.GetHealInjuryCostHerb(context.CurrChar.GetInjuries()))
		{
			yield return ETeammateCommandBanReason.HealInjuryHerbLack;
		}
		int attainmentLackIndex = 3;
		BoolArray32 healInjuryBanReason = DomainManager.Combat.GetHealInjuryBanReason(context.TeammateChar, context.CurrChar);
		if (((BoolArray32)(ref healInjuryBanReason))[attainmentLackIndex])
		{
			yield return ETeammateCommandBanReason.HealInjuryAttainmentLack;
		}
	}
}
