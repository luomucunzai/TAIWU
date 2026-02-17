using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiXiang
{
	// Token: 0x020002CD RID: 717
	public class WuXinHuaXin : ReduceHitOddsAddAcupoint
	{
		// Token: 0x06003293 RID: 12947 RVA: 0x0021FED6 File Offset: 0x0021E0D6
		public WuXinHuaXin()
		{
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x0021FEE0 File Offset: 0x0021E0E0
		public WuXinHuaXin(CombatSkillKey skillKey) : base(skillKey, 17060)
		{
			this.AcupointCount = 2;
		}
	}
}
