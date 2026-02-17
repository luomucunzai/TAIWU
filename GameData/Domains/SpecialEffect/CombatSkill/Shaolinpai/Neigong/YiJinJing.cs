using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Neigong
{
	// Token: 0x02000421 RID: 1057
	public class YiJinJing : ChangeNeiliAllocation
	{
		// Token: 0x06003958 RID: 14680 RVA: 0x0023E180 File Offset: 0x0023C380
		public YiJinJing()
		{
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x0023E18A File Offset: 0x0023C38A
		public YiJinJing(CombatSkillKey skillKey) : base(skillKey, 1007)
		{
			this.AffectNeiliAllocationType = 2;
		}
	}
}
