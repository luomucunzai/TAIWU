using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Agile
{
	// Token: 0x0200056B RID: 1387
	public class TanTuiSuoDi : ChangeAttackHitType
	{
		// Token: 0x060040F5 RID: 16629 RVA: 0x00260D6F File Offset: 0x0025EF6F
		public TanTuiSuoDi()
		{
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x00260D79 File Offset: 0x0025EF79
		public TanTuiSuoDi(CombatSkillKey skillKey) : base(skillKey, 2500)
		{
			this.HitType = 2;
		}
	}
}
