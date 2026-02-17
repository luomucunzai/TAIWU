using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Neigong
{
	// Token: 0x02000264 RID: 612
	public class XiHuaZhenJing : FiveElementsAddHitAndAvoid
	{
		// Token: 0x0600305C RID: 12380 RVA: 0x00216C64 File Offset: 0x00214E64
		public XiHuaZhenJing()
		{
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x00216C6E File Offset: 0x00214E6E
		public XiHuaZhenJing(CombatSkillKey skillKey) : base(skillKey, 8004)
		{
			this.RequireFiveElementsType = 2;
		}
	}
}
