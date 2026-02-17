using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000321 RID: 801
	public class DongLaiYiQi : AddDamageByFiveElementsType
	{
		// Token: 0x0600343A RID: 13370 RVA: 0x002281CC File Offset: 0x002263CC
		public DongLaiYiQi()
		{
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x002281D6 File Offset: 0x002263D6
		public DongLaiYiQi(CombatSkillKey skillKey) : base(skillKey, 16400)
		{
			this.CounteringType = 4;
			this.CounteredType = 0;
		}
	}
}
