using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Neigong
{
	// Token: 0x0200025F RID: 607
	public class DiNvShenGong : TransferFiveElementsNeili
	{
		// Token: 0x06003050 RID: 12368 RVA: 0x00216B95 File Offset: 0x00214D95
		public DiNvShenGong()
		{
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x00216B9F File Offset: 0x00214D9F
		public DiNvShenGong(CombatSkillKey skillKey) : base(skillKey, 8005)
		{
			this.SrcFiveElementsType = (base.IsDirect ? 1 : 3);
			this.DestFiveElementsType = 2;
		}
	}
}
