using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Neigong
{
	// Token: 0x020001F8 RID: 504
	public class DaYuanShanJin : AddFiveElementsDamage
	{
		// Token: 0x06002E61 RID: 11873 RVA: 0x0020E97E File Offset: 0x0020CB7E
		public DaYuanShanJin()
		{
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x0020E988 File Offset: 0x0020CB88
		public DaYuanShanJin(CombatSkillKey skillKey) : base(skillKey, 5002)
		{
			this.RequireSelfFiveElementsType = 4;
			this.AffectFiveElementsType = (base.IsDirect ? 2 : 1);
		}
	}
}
