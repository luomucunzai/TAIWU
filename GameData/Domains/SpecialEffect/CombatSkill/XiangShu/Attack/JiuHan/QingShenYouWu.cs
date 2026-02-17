using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JiuHan
{
	// Token: 0x02000305 RID: 773
	public class QingShenYouWu : AddAcupoint
	{
		// Token: 0x060033D0 RID: 13264 RVA: 0x00226B36 File Offset: 0x00224D36
		public QingShenYouWu()
		{
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x00226B40 File Offset: 0x00224D40
		public QingShenYouWu(CombatSkillKey skillKey) : base(skillKey, 17021)
		{
			this.AcupointCount = 1;
		}
	}
}
