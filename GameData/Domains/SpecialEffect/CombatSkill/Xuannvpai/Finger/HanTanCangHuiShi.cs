using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Finger
{
	// Token: 0x0200027A RID: 634
	public class HanTanCangHuiShi : AccumulateNeiliAllocationToStrengthen
	{
		// Token: 0x060030C1 RID: 12481 RVA: 0x00218735 File Offset: 0x00216935
		public HanTanCangHuiShi()
		{
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x0021873F File Offset: 0x0021693F
		public HanTanCangHuiShi(CombatSkillKey skillKey) : base(skillKey, 8204)
		{
			this.RequireNeiliAllocationType = 1;
		}
	}
}
