using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword
{
	// Token: 0x02000443 RID: 1091
	public class LaiQuJianFa : ReduceEnemyTrick
	{
		// Token: 0x06003A1F RID: 14879 RVA: 0x0024223D File Offset: 0x0024043D
		public LaiQuJianFa()
		{
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x00242247 File Offset: 0x00240447
		public LaiQuJianFa(CombatSkillKey skillKey) : base(skillKey, 7200)
		{
			this.AffectTrickType = 4;
		}
	}
}
