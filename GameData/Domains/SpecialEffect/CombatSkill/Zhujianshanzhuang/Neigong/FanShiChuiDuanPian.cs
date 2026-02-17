using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Neigong
{
	// Token: 0x020001CC RID: 460
	public class FanShiChuiDuanPian : BaseSectNeigong
	{
		// Token: 0x06002D14 RID: 11540 RVA: 0x0020A4C9 File Offset: 0x002086C9
		public FanShiChuiDuanPian()
		{
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x0020A4D3 File Offset: 0x002086D3
		public FanShiChuiDuanPian(CombatSkillKey skillKey) : base(skillKey, 9000)
		{
			this.SectId = 9;
		}
	}
}
