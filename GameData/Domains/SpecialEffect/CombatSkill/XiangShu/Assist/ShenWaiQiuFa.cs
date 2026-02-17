using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x0200032D RID: 813
	public class ShenWaiQiuFa : AddDamageByHitType
	{
		// Token: 0x06003472 RID: 13426 RVA: 0x00228BBA File Offset: 0x00226DBA
		public ShenWaiQiuFa()
		{
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x00228BC4 File Offset: 0x00226DC4
		public ShenWaiQiuFa(CombatSkillKey skillKey) : base(skillKey, 16405)
		{
			this.HitType = 0;
		}
	}
}
