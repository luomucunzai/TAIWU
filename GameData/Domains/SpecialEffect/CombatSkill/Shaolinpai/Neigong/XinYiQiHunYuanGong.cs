using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Neigong
{
	// Token: 0x0200041F RID: 1055
	public class XinYiQiHunYuanGong : ReduceFiveElementsDamage
	{
		// Token: 0x06003953 RID: 14675 RVA: 0x0023E129 File Offset: 0x0023C329
		public XinYiQiHunYuanGong()
		{
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x0023E133 File Offset: 0x0023C333
		public XinYiQiHunYuanGong(CombatSkillKey skillKey) : base(skillKey, 1002)
		{
			this.RequireSelfFiveElementsType = 0;
			this.AffectFiveElementsType = (base.IsDirect ? 1 : 3);
		}
	}
}
