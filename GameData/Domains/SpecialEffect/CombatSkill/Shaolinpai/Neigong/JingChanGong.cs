using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Neigong
{
	// Token: 0x0200041B RID: 1051
	public class JingChanGong : BaseSectNeigong
	{
		// Token: 0x06003949 RID: 14665 RVA: 0x0023E094 File Offset: 0x0023C294
		public JingChanGong()
		{
		}

		// Token: 0x0600394A RID: 14666 RVA: 0x0023E09E File Offset: 0x0023C29E
		public JingChanGong(CombatSkillKey skillKey) : base(skillKey, 1000)
		{
			this.SectId = 1;
		}
	}
}
