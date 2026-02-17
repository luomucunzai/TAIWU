using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Neigong
{
	// Token: 0x02000511 RID: 1297
	public class BianTiHuoQiFa : BaseSectNeigong
	{
		// Token: 0x06003EDE RID: 16094 RVA: 0x00257743 File Offset: 0x00255943
		public BianTiHuoQiFa()
		{
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x0025774D File Offset: 0x0025594D
		public BianTiHuoQiFa(CombatSkillKey skillKey) : base(skillKey, 14000)
		{
			this.SectId = 14;
		}
	}
}
