using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Neigong
{
	// Token: 0x020003C8 RID: 968
	public class TaiYiShenGong : LifeSkillAddHealCount
	{
		// Token: 0x06003773 RID: 14195 RVA: 0x00235985 File Offset: 0x00233B85
		public TaiYiShenGong()
		{
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x0023598F File Offset: 0x00233B8F
		public TaiYiShenGong(CombatSkillKey skillKey) : base(skillKey, 4006)
		{
			this.RequireLifeSkillType = 12;
		}
	}
}
