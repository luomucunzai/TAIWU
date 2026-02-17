using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Neigong
{
	// Token: 0x02000265 RID: 613
	public class XuanNvXinJue : BaseSectNeigong
	{
		// Token: 0x0600305E RID: 12382 RVA: 0x00216C85 File Offset: 0x00214E85
		public XuanNvXinJue()
		{
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x00216C8F File Offset: 0x00214E8F
		public XuanNvXinJue(CombatSkillKey skillKey) : base(skillKey, 8000)
		{
			this.SectId = 8;
		}
	}
}
