using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Neigong
{
	// Token: 0x02000262 RID: 610
	public class TaiYuanYuNvGong : ChangeNeiliAllocation
	{
		// Token: 0x06003058 RID: 12376 RVA: 0x00216C10 File Offset: 0x00214E10
		public TaiYuanYuNvGong()
		{
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x00216C1A File Offset: 0x00214E1A
		public TaiYuanYuNvGong(CombatSkillKey skillKey) : base(skillKey, 8007)
		{
			this.AffectNeiliAllocationType = 1;
		}
	}
}
