using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006E0 RID: 1760
	public class TeammateCommandCheckerDefendSkill : TeammateCommandCheckerBase
	{
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x0600678A RID: 26506 RVA: 0x003B2039 File Offset: 0x003B0239
		protected override bool CheckTeammateBefore
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600678B RID: 26507 RVA: 0x003B203C File Offset: 0x003B023C
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			short skillId = context.TeammateChar.GetDefendCommandSkillId();
			bool flag = skillId < 0;
			if (flag)
			{
				yield return ETeammateCommandBanReason.DefendSkillNonSkill;
			}
			else
			{
				bool flag2 = !DomainManager.Combat.SkillDirectionCanCast(context.TeammateChar, skillId);
				if (flag2)
				{
					yield return ETeammateCommandBanReason.DefendSkillBanned;
				}
			}
			yield break;
		}
	}
}
