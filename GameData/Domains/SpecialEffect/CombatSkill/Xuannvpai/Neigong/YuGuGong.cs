using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Neigong
{
	// Token: 0x02000267 RID: 615
	public class YuGuGong : FiveElementsAddPenetrateAndResist
	{
		// Token: 0x06003064 RID: 12388 RVA: 0x00216CC6 File Offset: 0x00214EC6
		public YuGuGong()
		{
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x00216CD0 File Offset: 0x00214ED0
		public YuGuGong(CombatSkillKey skillKey) : base(skillKey, 8001)
		{
			this.RequireFiveElementsType = 2;
		}
	}
}
