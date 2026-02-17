using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006F0 RID: 1776
	public class TeammateCommandCheckerInterruptOtherAction : TeammateCommandCheckerAccelerateCast
	{
		// Token: 0x060067B2 RID: 26546 RVA: 0x003B224B File Offset: 0x003B044B
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool flag = context.CurrChar.GetPreparingOtherAction() < 0 && !context.CurrChar.GetPreparingItem().IsValid();
			if (flag)
			{
				yield return ETeammateCommandBanReason.Negative;
			}
			yield break;
		}
	}
}
