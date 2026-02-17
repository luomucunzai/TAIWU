using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Finger
{
	// Token: 0x02000460 RID: 1120
	public class TianDiYuanYiZhi : AttackNeiliFiveElementsType
	{
		// Token: 0x06003AE8 RID: 15080 RVA: 0x00245BF0 File Offset: 0x00243DF0
		public TianDiYuanYiZhi()
		{
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x00245BFA File Offset: 0x00243DFA
		public TianDiYuanYiZhi(CombatSkillKey skillKey) : base(skillKey, 7106)
		{
			this.AffectFiveElementsType = 4;
		}
	}
}
