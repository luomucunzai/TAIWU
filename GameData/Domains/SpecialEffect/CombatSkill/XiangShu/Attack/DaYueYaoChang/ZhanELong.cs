using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.DaYueYaoChang
{
	// Token: 0x0200031F RID: 799
	public class ZhanELong : ReduceResources
	{
		// Token: 0x06003433 RID: 13363 RVA: 0x00228125 File Offset: 0x00226325
		public ZhanELong()
		{
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x0022812F File Offset: 0x0022632F
		public ZhanELong(CombatSkillKey skillKey) : base(skillKey, 17013)
		{
			this.AffectFrameCount = 60;
		}
	}
}
