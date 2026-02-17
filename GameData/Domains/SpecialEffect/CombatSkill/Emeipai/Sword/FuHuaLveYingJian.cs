using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Sword
{
	// Token: 0x0200053A RID: 1338
	public class FuHuaLveYingJian : AddHitOrReduceAvoid
	{
		// Token: 0x06003FD2 RID: 16338 RVA: 0x0025BB85 File Offset: 0x00259D85
		public FuHuaLveYingJian()
		{
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x0025BB8F File Offset: 0x00259D8F
		public FuHuaLveYingJian(CombatSkillKey skillKey) : base(skillKey, 2300)
		{
			this.AffectHitType = 2;
		}
	}
}
