using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Neigong
{
	// Token: 0x020001F7 RID: 503
	public class BuNianXinJue : TransferFiveElementsNeili
	{
		// Token: 0x06002E5F RID: 11871 RVA: 0x0020E94B File Offset: 0x0020CB4B
		public BuNianXinJue()
		{
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x0020E955 File Offset: 0x0020CB55
		public BuNianXinJue(CombatSkillKey skillKey) : base(skillKey, 5003)
		{
			this.SrcFiveElementsType = (base.IsDirect ? 0 : 1);
			this.DestFiveElementsType = 4;
		}
	}
}
