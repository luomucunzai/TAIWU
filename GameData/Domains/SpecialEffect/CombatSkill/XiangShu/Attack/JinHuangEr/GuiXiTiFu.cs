using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JinHuangEr
{
	// Token: 0x0200030C RID: 780
	public class GuiXiTiFu : RepeatNormalAttack
	{
		// Token: 0x060033E7 RID: 13287 RVA: 0x00226F9C File Offset: 0x0022519C
		public GuiXiTiFu()
		{
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x00226FA6 File Offset: 0x002251A6
		public GuiXiTiFu(CombatSkillKey skillKey) : base(skillKey, 17030)
		{
			this.RepeatTimes = 1;
		}
	}
}
