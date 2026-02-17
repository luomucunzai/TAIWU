using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JiuHan
{
	// Token: 0x02000304 RID: 772
	public class QingShenSiWu : AddAcupoint
	{
		// Token: 0x060033CE RID: 13262 RVA: 0x00226B15 File Offset: 0x00224D15
		public QingShenSiWu()
		{
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x00226B1F File Offset: 0x00224D1F
		public QingShenSiWu(CombatSkillKey skillKey) : base(skillKey, 17024)
		{
			this.AcupointCount = 2;
		}
	}
}
