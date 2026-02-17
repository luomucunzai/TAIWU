using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Neigong
{
	// Token: 0x02000224 RID: 548
	public class WuYinDaFa : FiveElementsAddHitAndAvoid
	{
		// Token: 0x06002F49 RID: 12105 RVA: 0x002126B5 File Offset: 0x002108B5
		public WuYinDaFa()
		{
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x002126BF File Offset: 0x002108BF
		public WuYinDaFa(CombatSkillKey skillKey) : base(skillKey, 15005)
		{
			this.RequireFiveElementsType = 4;
		}
	}
}
