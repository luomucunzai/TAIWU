using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000322 RID: 802
	public class FenShenJingJie : AddDamageByHitType
	{
		// Token: 0x0600343C RID: 13372 RVA: 0x002281F4 File Offset: 0x002263F4
		public FenShenJingJie()
		{
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x002281FE File Offset: 0x002263FE
		public FenShenJingJie(CombatSkillKey skillKey) : base(skillKey, 16408)
		{
			this.HitType = 2;
		}
	}
}
