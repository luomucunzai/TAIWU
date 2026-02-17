using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006E4 RID: 1764
	public class TeammateCommandCheckerHealAcupoint : TeammateCommandCheckerBase
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06006796 RID: 26518 RVA: 0x003B20C5 File Offset: 0x003B02C5
		protected override bool CheckTeammateAfter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006797 RID: 26519 RVA: 0x003B20C8 File Offset: 0x003B02C8
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool flag = context.CurrChar.GetAcupointCollection().GetTotalCount() <= 0;
			if (flag)
			{
				yield return ETeammateCommandBanReason.HealAcupointNonAcupoint;
			}
			yield break;
		}
	}
}
