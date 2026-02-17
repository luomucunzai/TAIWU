using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiXiang
{
	// Token: 0x020002CE RID: 718
	public class XinHuaWuXin : ReduceHitOddsAddAcupoint
	{
		// Token: 0x06003295 RID: 12949 RVA: 0x0021FEF7 File Offset: 0x0021E0F7
		public XinHuaWuXin()
		{
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x0021FF01 File Offset: 0x0021E101
		public XinHuaWuXin(CombatSkillKey skillKey) : base(skillKey, 17063)
		{
			this.AcupointCount = 4;
		}
	}
}
