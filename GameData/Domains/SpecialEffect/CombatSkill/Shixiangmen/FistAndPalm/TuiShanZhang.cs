using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm
{
	// Token: 0x020003FC RID: 1020
	public class TuiShanZhang : AddHitOrReduceAvoid
	{
		// Token: 0x06003898 RID: 14488 RVA: 0x0023B0B3 File Offset: 0x002392B3
		public TuiShanZhang()
		{
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x0023B0BD File Offset: 0x002392BD
		public TuiShanZhang(CombatSkillKey skillKey) : base(skillKey, 6100)
		{
			this.AffectHitType = 0;
		}
	}
}
