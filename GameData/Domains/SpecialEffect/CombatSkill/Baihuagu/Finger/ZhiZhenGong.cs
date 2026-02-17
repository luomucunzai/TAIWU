using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger
{
	// Token: 0x020005D4 RID: 1492
	public class ZhiZhenGong : AddHitOrReduceAvoid
	{
		// Token: 0x0600441F RID: 17439 RVA: 0x0026E2B0 File Offset: 0x0026C4B0
		public ZhiZhenGong()
		{
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x0026E2BA File Offset: 0x0026C4BA
		public ZhiZhenGong(CombatSkillKey skillKey) : base(skillKey, 3100)
		{
			this.AffectHitType = 1;
		}
	}
}
