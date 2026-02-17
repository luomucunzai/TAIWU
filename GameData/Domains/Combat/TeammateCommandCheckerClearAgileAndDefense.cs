using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006ED RID: 1773
	public class TeammateCommandCheckerClearAgileAndDefense : TeammateCommandCheckerBase
	{
		// Token: 0x060067AC RID: 26540 RVA: 0x003B21EB File Offset: 0x003B03EB
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool flag = context.CurrChar.GetAffectingMoveSkillId() < 0 && context.CurrChar.GetAffectingDefendSkillId() < 0;
			if (flag)
			{
				yield return ETeammateCommandBanReason.Negative;
			}
			yield break;
		}
	}
}
