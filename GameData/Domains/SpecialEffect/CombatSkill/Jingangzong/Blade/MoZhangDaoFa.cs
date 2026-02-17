using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Blade
{
	// Token: 0x020004D4 RID: 1236
	public class MoZhangDaoFa : GameData.Domains.SpecialEffect.CombatSkill.Common.Attack.AttackHitType
	{
		// Token: 0x06003D83 RID: 15747 RVA: 0x002522A0 File Offset: 0x002504A0
		public MoZhangDaoFa()
		{
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x002522AA File Offset: 0x002504AA
		public MoZhangDaoFa(CombatSkillKey skillKey) : base(skillKey, 11202)
		{
			this.AffectHitType = 0;
		}
	}
}
