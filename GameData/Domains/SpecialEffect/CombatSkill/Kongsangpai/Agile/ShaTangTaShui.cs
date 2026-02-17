using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Agile
{
	// Token: 0x020004A4 RID: 1188
	public class ShaTangTaShui : ChangeLegSkillHit
	{
		// Token: 0x06003C95 RID: 15509 RVA: 0x0024E316 File Offset: 0x0024C516
		public ShaTangTaShui()
		{
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x0024E320 File Offset: 0x0024C520
		public ShaTangTaShui(CombatSkillKey skillKey) : base(skillKey, 10502)
		{
			this.BuffHitType = 1;
		}
	}
}
