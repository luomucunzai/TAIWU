using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.DefenseAndAssist
{
	// Token: 0x0200055F RID: 1375
	public class EMeiHuBuGong : NeiliAllocationChangeInjury
	{
		// Token: 0x060040A6 RID: 16550 RVA: 0x0025F3CD File Offset: 0x0025D5CD
		public EMeiHuBuGong()
		{
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x0025F3D7 File Offset: 0x0025D5D7
		public EMeiHuBuGong(CombatSkillKey skillKey) : base(skillKey, 2700)
		{
			this.RequireNeiliAllocationType = 1;
		}
	}
}
