using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Blade
{
	// Token: 0x02000212 RID: 530
	public class TianGangDaoFa : AttackNeiliFiveElementsType
	{
		// Token: 0x06002EEF RID: 12015 RVA: 0x00211152 File Offset: 0x0020F352
		public TianGangDaoFa()
		{
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x0021115C File Offset: 0x0020F35C
		public TianGangDaoFa(CombatSkillKey skillKey) : base(skillKey, 5306)
		{
			this.AffectFiveElementsType = 2;
		}
	}
}
