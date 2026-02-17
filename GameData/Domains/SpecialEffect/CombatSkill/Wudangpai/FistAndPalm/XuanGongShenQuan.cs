using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm
{
	// Token: 0x020003D3 RID: 979
	public class XuanGongShenQuan : AttackSkillFiveElementsType
	{
		// Token: 0x060037A5 RID: 14245 RVA: 0x00236807 File Offset: 0x00234A07
		public XuanGongShenQuan()
		{
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x00236811 File Offset: 0x00234A11
		public XuanGongShenQuan(CombatSkillKey skillKey) : base(skillKey, 4105)
		{
			this.AffectFiveElementsType = 0;
		}
	}
}
