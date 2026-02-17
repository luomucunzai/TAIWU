using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x0200032C RID: 812
	public class ShenTuFuSha : AddDamageByFiveElementsType
	{
		// Token: 0x06003470 RID: 13424 RVA: 0x00228B92 File Offset: 0x00226D92
		public ShenTuFuSha()
		{
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x00228B9C File Offset: 0x00226D9C
		public ShenTuFuSha(CombatSkillKey skillKey) : base(skillKey, 16407)
		{
			this.CounteringType = 2;
			this.CounteredType = 1;
		}
	}
}
