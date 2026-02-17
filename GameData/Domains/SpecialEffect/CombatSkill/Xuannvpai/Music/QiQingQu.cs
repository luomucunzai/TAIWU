using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music
{
	// Token: 0x0200026D RID: 621
	public class QiQingQu : AddHitOrReduceAvoid
	{
		// Token: 0x06003084 RID: 12420 RVA: 0x00217763 File Offset: 0x00215963
		public QiQingQu()
		{
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x0021776D File Offset: 0x0021596D
		public QiQingQu(CombatSkillKey skillKey) : base(skillKey, 8300)
		{
			this.AffectHitType = 3;
		}
	}
}
