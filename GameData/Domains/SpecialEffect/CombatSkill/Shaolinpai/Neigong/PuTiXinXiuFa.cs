using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Neigong
{
	// Token: 0x0200041D RID: 1053
	public class PuTiXinXiuFa : TransferFiveElementsNeili
	{
		// Token: 0x0600394E RID: 14670 RVA: 0x0023E0D2 File Offset: 0x0023C2D2
		public PuTiXinXiuFa()
		{
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x0023E0DC File Offset: 0x0023C2DC
		public PuTiXinXiuFa(CombatSkillKey skillKey) : base(skillKey, 1004)
		{
			this.SrcFiveElementsType = (base.IsDirect ? 2 : 1);
			this.DestFiveElementsType = 0;
		}
	}
}
