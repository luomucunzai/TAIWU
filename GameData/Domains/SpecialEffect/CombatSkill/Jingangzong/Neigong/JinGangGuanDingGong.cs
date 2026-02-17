using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong
{
	// Token: 0x020004B6 RID: 1206
	public class JinGangGuanDingGong : FiveElementsAddHitAndAvoid
	{
		// Token: 0x06003CE9 RID: 15593 RVA: 0x0024F4D8 File Offset: 0x0024D6D8
		public JinGangGuanDingGong()
		{
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x0024F4E2 File Offset: 0x0024D6E2
		public JinGangGuanDingGong(CombatSkillKey skillKey) : base(skillKey, 11004)
		{
			this.RequireFiveElementsType = 0;
		}
	}
}
