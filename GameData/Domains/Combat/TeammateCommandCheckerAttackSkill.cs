using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006DF RID: 1759
	public class TeammateCommandCheckerAttackSkill : TeammateCommandCheckerBase
	{
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06006787 RID: 26503 RVA: 0x003B2016 File Offset: 0x003B0216
		protected override bool CheckTeammateBefore
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006788 RID: 26504 RVA: 0x003B2019 File Offset: 0x003B0219
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			short skillId = context.TeammateChar.GetAttackCommandSkillId();
			bool flag = skillId < 0;
			if (flag)
			{
				yield return ETeammateCommandBanReason.AttackSkillNonSkill;
			}
			else
			{
				bool flag2 = !DomainManager.Combat.SkillDirectionCanCast(context.TeammateChar, skillId);
				if (flag2)
				{
					yield return ETeammateCommandBanReason.AttackSkillBanned;
				}
			}
			yield break;
		}
	}
}
