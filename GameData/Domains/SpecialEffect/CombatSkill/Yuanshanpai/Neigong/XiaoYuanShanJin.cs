using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Neigong
{
	// Token: 0x020001FB RID: 507
	public class XiaoYuanShanJin : FiveElementsAddPenetrateAndResist
	{
		// Token: 0x06002E69 RID: 11881 RVA: 0x0020E9F2 File Offset: 0x0020CBF2
		public XiaoYuanShanJin()
		{
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x0020E9FC File Offset: 0x0020CBFC
		public XiaoYuanShanJin(CombatSkillKey skillKey) : base(skillKey, 5001)
		{
			this.RequireFiveElementsType = 4;
		}
	}
}
