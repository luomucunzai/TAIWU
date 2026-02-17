using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Blade
{
	// Token: 0x020004CE RID: 1230
	public class JinGangDaoFa : ReduceEnemyTrick
	{
		// Token: 0x06003D60 RID: 15712 RVA: 0x002517BC File Offset: 0x0024F9BC
		public JinGangDaoFa()
		{
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x002517C6 File Offset: 0x0024F9C6
		public JinGangDaoFa(CombatSkillKey skillKey) : base(skillKey, 11200)
		{
			this.AffectTrickType = 3;
		}
	}
}
