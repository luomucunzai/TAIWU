using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Neigong
{
	// Token: 0x020003CB RID: 971
	public class XiSuiJinJing : FiveElementsAddHitAndAvoid
	{
		// Token: 0x0600377B RID: 14203 RVA: 0x00235AC9 File Offset: 0x00233CC9
		public XiSuiJinJing()
		{
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x00235AD3 File Offset: 0x00233CD3
		public XiSuiJinJing(CombatSkillKey skillKey) : base(skillKey, 4004)
		{
			this.RequireFiveElementsType = 3;
		}
	}
}
