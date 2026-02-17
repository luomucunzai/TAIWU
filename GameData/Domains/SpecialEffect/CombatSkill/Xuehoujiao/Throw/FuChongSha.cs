using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Throw
{
	// Token: 0x02000219 RID: 537
	public class FuChongSha : PoisonDisableAgileOrDefense
	{
		// Token: 0x06002F13 RID: 12051 RVA: 0x00211A22 File Offset: 0x0020FC22
		public FuChongSha()
		{
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x00211A2C File Offset: 0x0020FC2C
		public FuChongSha(CombatSkillKey skillKey) : base(skillKey, 15401)
		{
			this.RequirePoisonType = 4;
		}
	}
}
