using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Sword
{
	// Token: 0x02000388 RID: 904
	public class LvRanJianFa : ReduceEnemyTrick
	{
		// Token: 0x0600361F RID: 13855 RVA: 0x0022F5C8 File Offset: 0x0022D7C8
		public LvRanJianFa()
		{
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x0022F5D2 File Offset: 0x0022D7D2
		public LvRanJianFa(CombatSkillKey skillKey) : base(skillKey, 12300)
		{
			this.AffectTrickType = 5;
		}
	}
}
