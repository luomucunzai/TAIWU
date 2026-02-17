using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006E1 RID: 1761
	public class TeammateCommandCheckerHealInjury : TeammateCommandCheckerBase
	{
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x0600678D RID: 26509 RVA: 0x003B205C File Offset: 0x003B025C
		protected override bool CheckTeammateAfter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x003B205F File Offset: 0x003B025F
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool flag = !context.CurrChar.GetInjuries().HasAnyInjury();
			if (flag)
			{
				yield return ETeammateCommandBanReason.HealInjuryNonInjury;
				yield break;
			}
			bool flag2 = context.TeammateChar.GetHealInjuryCount() <= 0;
			if (flag2)
			{
				yield return ETeammateCommandBanReason.HealInjuryCountLack;
			}
			bool flag3 = context.TeammateChar.GetCharacter().GetResource(5) < CombatDomain.GetHealInjuryCostHerb(context.CurrChar.GetInjuries());
			if (flag3)
			{
				yield return ETeammateCommandBanReason.HealInjuryHerbLack;
			}
			int attainmentLackIndex = 3;
			bool flag4 = DomainManager.Combat.GetHealInjuryBanReason(context.TeammateChar, context.CurrChar)[attainmentLackIndex];
			if (flag4)
			{
				yield return ETeammateCommandBanReason.HealInjuryAttainmentLack;
			}
			yield break;
		}
	}
}
