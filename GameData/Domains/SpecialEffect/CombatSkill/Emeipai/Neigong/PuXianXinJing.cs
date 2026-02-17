using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Neigong
{
	// Token: 0x0200054C RID: 1356
	public class PuXianXinJing : FiveElementsAddHitAndAvoid
	{
		// Token: 0x06004033 RID: 16435 RVA: 0x0025D249 File Offset: 0x0025B449
		public PuXianXinJing()
		{
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x0025D253 File Offset: 0x0025B453
		public PuXianXinJing(CombatSkillKey skillKey) : base(skillKey, 2005)
		{
			this.RequireFiveElementsType = 1;
		}
	}
}
