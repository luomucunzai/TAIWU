using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Leg
{
	// Token: 0x0200015C RID: 348
	public class ZhuanJie : AddMaxPower
	{
		// Token: 0x06002B06 RID: 11014 RVA: 0x00204828 File Offset: 0x00202A28
		public ZhuanJie()
		{
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x00204832 File Offset: 0x00202A32
		public ZhuanJie(CombatSkillKey skillKey) : base(skillKey, 40501)
		{
		}
	}
}
