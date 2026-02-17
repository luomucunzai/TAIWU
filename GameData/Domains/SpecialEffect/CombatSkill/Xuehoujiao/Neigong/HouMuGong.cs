using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Neigong
{
	// Token: 0x02000220 RID: 544
	public class HouMuGong : BaseSectNeigong
	{
		// Token: 0x06002F34 RID: 12084 RVA: 0x002122D9 File Offset: 0x002104D9
		public HouMuGong()
		{
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x002122E3 File Offset: 0x002104E3
		public HouMuGong(CombatSkillKey skillKey) : base(skillKey, 15000)
		{
			this.SectId = 15;
		}
	}
}
