using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Sword
{
	// Token: 0x0200038A RID: 906
	public class ShenChiZhanYaoJian : AttackSkillFiveElementsType
	{
		// Token: 0x06003627 RID: 13863 RVA: 0x0022F92B File Offset: 0x0022DB2B
		public ShenChiZhanYaoJian()
		{
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x0022F935 File Offset: 0x0022DB35
		public ShenChiZhanYaoJian(CombatSkillKey skillKey) : base(skillKey, 12305)
		{
			this.AffectFiveElementsType = 4;
		}
	}
}
