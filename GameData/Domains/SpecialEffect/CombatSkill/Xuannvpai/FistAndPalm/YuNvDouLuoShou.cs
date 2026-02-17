using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.FistAndPalm
{
	// Token: 0x02000276 RID: 630
	public class YuNvDouLuoShou : ReduceEnemyTrick
	{
		// Token: 0x060030B1 RID: 12465 RVA: 0x00218411 File Offset: 0x00216611
		public YuNvDouLuoShou()
		{
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x0021841B File Offset: 0x0021661B
		public YuNvDouLuoShou(CombatSkillKey skillKey) : base(skillKey, 8100)
		{
			this.AffectTrickType = 8;
		}
	}
}
