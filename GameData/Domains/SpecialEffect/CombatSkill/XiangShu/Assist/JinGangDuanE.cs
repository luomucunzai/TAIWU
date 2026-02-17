using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000326 RID: 806
	public class JinGangDuanE : AddDamageByFiveElementsType
	{
		// Token: 0x0600344F RID: 13391 RVA: 0x00228561 File Offset: 0x00226761
		public JinGangDuanE()
		{
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x0022856B File Offset: 0x0022676B
		public JinGangDuanE(CombatSkillKey skillKey) : base(skillKey, 16401)
		{
			this.CounteringType = 1;
			this.CounteredType = 3;
		}
	}
}
