using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade
{
	// Token: 0x02000407 RID: 1031
	public class JiuNiuErHuDao : PowerUpByMainAttribute
	{
		// Token: 0x060038DF RID: 14559 RVA: 0x0023C4FC File Offset: 0x0023A6FC
		public JiuNiuErHuDao()
		{
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x0023C506 File Offset: 0x0023A706
		public JiuNiuErHuDao(CombatSkillKey skillKey) : base(skillKey, 6203)
		{
			this.RequireMainAttributeType = 0;
		}
	}
}
