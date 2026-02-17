using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Neigong
{
	// Token: 0x02000156 RID: 342
	public class ZhuanJie : AddMaxPower
	{
		// Token: 0x06002AF7 RID: 10999 RVA: 0x002046D4 File Offset: 0x002028D4
		public ZhuanJie()
		{
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x002046DE File Offset: 0x002028DE
		public ZhuanJie(CombatSkillKey skillKey) : base(skillKey, 40001)
		{
		}
	}
}
