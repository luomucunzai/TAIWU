using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Neigong
{
	// Token: 0x0200054D RID: 1357
	public class XianTianYiYuanQiGong : AddFiveElementsDamage
	{
		// Token: 0x06004035 RID: 16437 RVA: 0x0025D26A File Offset: 0x0025B46A
		public XianTianYiYuanQiGong()
		{
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x0025D274 File Offset: 0x0025B474
		public XianTianYiYuanQiGong(CombatSkillKey skillKey) : base(skillKey, 2002)
		{
			this.RequireSelfFiveElementsType = 1;
			this.AffectFiveElementsType = (base.IsDirect ? 4 : 0);
		}
	}
}
