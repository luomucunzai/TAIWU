using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006DE RID: 1758
	public class TeammateCommandCheckerAttack : TeammateCommandCheckerBase
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06006784 RID: 26500 RVA: 0x003B1FF3 File Offset: 0x003B01F3
		protected override bool CheckTeammateBefore
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006785 RID: 26501 RVA: 0x003B1FF6 File Offset: 0x003B01F6
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool flag = context.TeammateChar.GetAttackCommandTrickType() < 0;
			if (flag)
			{
				yield return ETeammateCommandBanReason.AttackNonTrick;
			}
			yield break;
		}
	}
}
