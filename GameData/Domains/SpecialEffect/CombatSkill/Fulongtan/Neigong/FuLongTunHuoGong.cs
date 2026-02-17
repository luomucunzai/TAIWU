using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Neigong
{
	// Token: 0x02000513 RID: 1299
	public class FuLongTunHuoGong : FiveElementsAddPenetrateAndResist
	{
		// Token: 0x06003EE4 RID: 16100 RVA: 0x00257785 File Offset: 0x00255985
		public FuLongTunHuoGong()
		{
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x0025778F File Offset: 0x0025598F
		public FuLongTunHuoGong(CombatSkillKey skillKey) : base(skillKey, 14001)
		{
			this.RequireFiveElementsType = 3;
		}
	}
}
