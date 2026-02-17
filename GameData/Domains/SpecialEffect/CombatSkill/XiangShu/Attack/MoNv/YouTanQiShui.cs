using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.MoNv
{
	// Token: 0x020002F8 RID: 760
	public class YouTanQiShui : AddPoison
	{
		// Token: 0x0600338B RID: 13195 RVA: 0x00225618 File Offset: 0x00223818
		public YouTanQiShui()
		{
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x00225622 File Offset: 0x00223822
		public YouTanQiShui(CombatSkillKey skillKey) : base(skillKey, 17001)
		{
			this.PoisonTypeCount = 3;
		}
	}
}
