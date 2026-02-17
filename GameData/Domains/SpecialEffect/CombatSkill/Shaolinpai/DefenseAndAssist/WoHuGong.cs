using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.DefenseAndAssist
{
	// Token: 0x02000437 RID: 1079
	public class WoHuGong : NeiliAllocationChangeInjury
	{
		// Token: 0x060039E1 RID: 14817 RVA: 0x00240ECC File Offset: 0x0023F0CC
		public WoHuGong()
		{
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x00240ED6 File Offset: 0x0023F0D6
		public WoHuGong(CombatSkillKey skillKey) : base(skillKey, 1600)
		{
			this.RequireNeiliAllocationType = 2;
		}
	}
}
