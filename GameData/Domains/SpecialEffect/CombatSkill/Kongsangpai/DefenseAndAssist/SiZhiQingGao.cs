using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist
{
	// Token: 0x0200049D RID: 1181
	public class SiZhiQingGao : NeiliAllocationChangeInjury
	{
		// Token: 0x06003C62 RID: 15458 RVA: 0x0024D4B3 File Offset: 0x0024B6B3
		public SiZhiQingGao()
		{
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x0024D4BD File Offset: 0x0024B6BD
		public SiZhiQingGao(CombatSkillKey skillKey) : base(skillKey, 10700)
		{
			this.RequireNeiliAllocationType = 3;
		}
	}
}
