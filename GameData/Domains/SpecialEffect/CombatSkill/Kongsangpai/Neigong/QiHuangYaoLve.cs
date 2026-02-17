using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Neigong
{
	// Token: 0x02000482 RID: 1154
	public class QiHuangYaoLve : BaseSectNeigong
	{
		// Token: 0x06003BAB RID: 15275 RVA: 0x002490EC File Offset: 0x002472EC
		public QiHuangYaoLve()
		{
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x002490F6 File Offset: 0x002472F6
		public QiHuangYaoLve(CombatSkillKey skillKey) : base(skillKey, 10000)
		{
			this.SectId = 10;
		}
	}
}
