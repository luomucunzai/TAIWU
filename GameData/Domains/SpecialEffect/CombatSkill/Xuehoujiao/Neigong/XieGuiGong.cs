using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Neigong
{
	// Token: 0x02000225 RID: 549
	public class XieGuiGong : ReduceFiveElementsDamage
	{
		// Token: 0x06002F4B RID: 12107 RVA: 0x002126D6 File Offset: 0x002108D6
		public XieGuiGong()
		{
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x002126E0 File Offset: 0x002108E0
		public XieGuiGong(CombatSkillKey skillKey) : base(skillKey, 15002)
		{
			this.RequireSelfFiveElementsType = 4;
			this.AffectFiveElementsType = (base.IsDirect ? 2 : 1);
		}
	}
}
