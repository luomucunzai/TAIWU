using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XiangShu
{
	// Token: 0x020002D8 RID: 728
	public class RuXuYanXiang : SkillCostNeiliAllocation
	{
		// Token: 0x060032CD RID: 13005 RVA: 0x00220E94 File Offset: 0x0021F094
		public RuXuYanXiang()
		{
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x00220E9E File Offset: 0x0021F09E
		public RuXuYanXiang(CombatSkillKey skillKey) : base(skillKey, 17091)
		{
			this.CostNeiliAllocationPerGrade = 2;
		}
	}
}
