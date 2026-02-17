using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger
{
	// Token: 0x020005D1 RID: 1489
	public class WanHuaLingYueZhi : AttackNeiliFiveElementsType
	{
		// Token: 0x0600440A RID: 17418 RVA: 0x0026DC9D File Offset: 0x0026BE9D
		public WanHuaLingYueZhi()
		{
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x0026DCA7 File Offset: 0x0026BEA7
		public WanHuaLingYueZhi(CombatSkillKey skillKey) : base(skillKey, 3106)
		{
			this.AffectFiveElementsType = 3;
		}
	}
}
