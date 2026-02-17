using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.DefenseAndAssist
{
	// Token: 0x020004FD RID: 1277
	public class JiuYaoXingJunYan : TrickAddHitOrAvoid
	{
		// Token: 0x06003E6C RID: 15980 RVA: 0x00255D00 File Offset: 0x00253F00
		public JiuYaoXingJunYan()
		{
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x00255D0A File Offset: 0x00253F0A
		public JiuYaoXingJunYan(CombatSkillKey skillKey) : base(skillKey, 13605)
		{
			this.RequireTrickTypes = new sbyte[]
			{
				0,
				2,
				1
			};
		}
	}
}
