using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Agile
{
	// Token: 0x0200025E RID: 606
	public class YouHunGuiBu : ChangeLegSkillHit
	{
		// Token: 0x0600304E RID: 12366 RVA: 0x00216B74 File Offset: 0x00214D74
		public YouHunGuiBu()
		{
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x00216B7E File Offset: 0x00214D7E
		public YouHunGuiBu(CombatSkillKey skillKey) : base(skillKey, 15602)
		{
			this.BuffHitType = 2;
		}
	}
}
