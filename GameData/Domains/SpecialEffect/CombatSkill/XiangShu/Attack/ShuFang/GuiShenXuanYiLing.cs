using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ShuFang
{
	// Token: 0x020002E2 RID: 738
	public class GuiShenXuanYiLing : SilenceSkillAndMinAttribute
	{
		// Token: 0x06003315 RID: 13077 RVA: 0x00222E6C File Offset: 0x0022106C
		public GuiShenXuanYiLing()
		{
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x00222E76 File Offset: 0x00221076
		public GuiShenXuanYiLing(CombatSkillKey skillKey) : base(skillKey, 17085)
		{
			this.SilenceSkillCount = 6;
		}
	}
}
