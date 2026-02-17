using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Neigong
{
	// Token: 0x0200041A RID: 1050
	public class ALuoHanShenGong : LifeSkillAddHealCount
	{
		// Token: 0x06003947 RID: 14663 RVA: 0x0023E072 File Offset: 0x0023C272
		public ALuoHanShenGong()
		{
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x0023E07C File Offset: 0x0023C27C
		public ALuoHanShenGong(CombatSkillKey skillKey) : base(skillKey, 1006)
		{
			this.RequireLifeSkillType = 13;
		}
	}
}
