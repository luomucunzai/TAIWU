using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XiangShu
{
	// Token: 0x020002DA RID: 730
	public class XieWangWuZheng : SkillCostNeiliAllocation
	{
		// Token: 0x060032DA RID: 13018 RVA: 0x00221206 File Offset: 0x0021F406
		public XieWangWuZheng()
		{
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x00221210 File Offset: 0x0021F410
		public XieWangWuZheng(CombatSkillKey skillKey) : base(skillKey, 17094)
		{
			this.CostNeiliAllocationPerGrade = 4;
		}
	}
}
