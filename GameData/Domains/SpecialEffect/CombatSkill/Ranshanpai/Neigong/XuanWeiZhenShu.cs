using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong
{
	// Token: 0x02000458 RID: 1112
	public class XuanWeiZhenShu : ChangeNeiliAllocation
	{
		// Token: 0x06003AB5 RID: 15029 RVA: 0x00244D46 File Offset: 0x00242F46
		public XuanWeiZhenShu()
		{
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x00244D50 File Offset: 0x00242F50
		public XuanWeiZhenShu(CombatSkillKey skillKey) : base(skillKey, 7007)
		{
			this.AffectNeiliAllocationType = 3;
		}
	}
}
