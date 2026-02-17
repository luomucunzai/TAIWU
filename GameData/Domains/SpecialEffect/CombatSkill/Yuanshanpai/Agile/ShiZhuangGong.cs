using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Agile
{
	// Token: 0x02000216 RID: 534
	public class ShiZhuangGong : ChangeLegSkillHit
	{
		// Token: 0x06002F02 RID: 12034 RVA: 0x0021143C File Offset: 0x0020F63C
		public ShiZhuangGong()
		{
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x00211446 File Offset: 0x0020F646
		public ShiZhuangGong(CombatSkillKey skillKey) : base(skillKey, 5400)
		{
			this.BuffHitType = 0;
		}
	}
}
