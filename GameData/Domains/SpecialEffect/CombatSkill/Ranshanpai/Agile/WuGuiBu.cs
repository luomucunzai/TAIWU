using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Agile
{
	// Token: 0x0200046D RID: 1133
	public class WuGuiBu : ChangeAttackHitType
	{
		// Token: 0x06003B30 RID: 15152 RVA: 0x00246FC4 File Offset: 0x002451C4
		public WuGuiBu()
		{
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x00246FCE File Offset: 0x002451CE
		public WuGuiBu(CombatSkillKey skillKey) : base(skillKey, 7400)
		{
			this.HitType = 1;
		}
	}
}
