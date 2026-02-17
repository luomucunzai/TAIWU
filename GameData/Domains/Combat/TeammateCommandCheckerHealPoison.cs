using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006E2 RID: 1762
	public class TeammateCommandCheckerHealPoison : TeammateCommandCheckerBase
	{
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06006790 RID: 26512 RVA: 0x003B207F File Offset: 0x003B027F
		protected override bool CheckTeammateAfter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006791 RID: 26513 RVA: 0x003B2082 File Offset: 0x003B0282
		protected unsafe override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool flag = !context.CurrChar.GetPoison().IsNonZero();
			if (flag)
			{
				yield return ETeammateCommandBanReason.HealPoisonNonPoison;
				yield break;
			}
			bool flag2 = context.TeammateChar.GetHealPoisonCount() <= 0;
			if (flag2)
			{
				yield return ETeammateCommandBanReason.HealPoisonCountLack;
			}
			bool flag3 = context.TeammateChar.GetCharacter().GetResource(5) < CombatDomain.GetHealPoisonCostHerb(*context.CurrChar.GetPoison());
			if (flag3)
			{
				yield return ETeammateCommandBanReason.HealPoisonHerbLack;
			}
			int attainmentLackIndex = 3;
			bool flag4 = DomainManager.Combat.GetHealPoisonBanReason(context.TeammateChar, context.CurrChar)[attainmentLackIndex];
			if (flag4)
			{
				yield return ETeammateCommandBanReason.HealPoisonAttainmentLack;
			}
			yield break;
		}
	}
}
