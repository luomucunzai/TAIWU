using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006DB RID: 1755
	public class TeammateCommandCheckerAccelerateCast : TeammateCommandCheckerBase
	{
		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x0600677B RID: 26491 RVA: 0x003B1F8A File Offset: 0x003B018A
		protected override bool CheckTeammateAfter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600677C RID: 26492 RVA: 0x003B1F8D File Offset: 0x003B018D
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool flag = context.CurrChar.GetPreparingSkillId() < 0 || context.CurrChar.GetSkillPreparePercent() >= 100;
			if (flag)
			{
				yield return ETeammateCommandBanReason.AccelerateNotPreparing;
			}
			yield break;
		}
	}
}
