using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.DaYueYaoChang
{
	// Token: 0x0200031C RID: 796
	public class SheDuanSanZhan : AddPowerAndRepeat
	{
		// Token: 0x0600342D RID: 13357 RVA: 0x002280B1 File Offset: 0x002262B1
		public SheDuanSanZhan()
		{
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x002280BB File Offset: 0x002262BB
		public SheDuanSanZhan(CombatSkillKey skillKey) : base(skillKey, 17014)
		{
			this.AutoCastReducePower = -20;
		}
	}
}
