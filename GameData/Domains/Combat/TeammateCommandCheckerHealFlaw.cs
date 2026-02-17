using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006E3 RID: 1763
	public class TeammateCommandCheckerHealFlaw : TeammateCommandCheckerBase
	{
		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06006793 RID: 26515 RVA: 0x003B20A2 File Offset: 0x003B02A2
		protected override bool CheckTeammateAfter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006794 RID: 26516 RVA: 0x003B20A5 File Offset: 0x003B02A5
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool flag = context.CurrChar.GetFlawCollection().GetTotalCount() <= 0;
			if (flag)
			{
				yield return ETeammateCommandBanReason.HealFlawNonFlaw;
			}
			yield break;
		}
	}
}
