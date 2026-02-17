using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Neigong
{
	// Token: 0x020003CA RID: 970
	public class WuDangTaiHeGong : BaseSectNeigong
	{
		// Token: 0x06003779 RID: 14201 RVA: 0x00235AA8 File Offset: 0x00233CA8
		public WuDangTaiHeGong()
		{
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x00235AB2 File Offset: 0x00233CB2
		public WuDangTaiHeGong(CombatSkillKey skillKey) : base(skillKey, 4000)
		{
			this.SectId = 4;
		}
	}
}
