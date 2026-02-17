using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.DefenseAndAssist
{
	// Token: 0x02000525 RID: 1317
	public class NuXiangGong : NeiliAllocationChangeInjury
	{
		// Token: 0x06003F3C RID: 16188 RVA: 0x0025909C File Offset: 0x0025729C
		public NuXiangGong()
		{
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x002590A6 File Offset: 0x002572A6
		public NuXiangGong(CombatSkillKey skillKey) : base(skillKey, 14600)
		{
			this.RequireNeiliAllocationType = 0;
		}
	}
}
