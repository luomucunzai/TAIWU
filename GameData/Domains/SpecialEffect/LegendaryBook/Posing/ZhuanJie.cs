using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Posing
{
	// Token: 0x0200013C RID: 316
	public class ZhuanJie : AddMaxPower
	{
		// Token: 0x06002A84 RID: 10884 RVA: 0x00202BCF File Offset: 0x00200DCF
		public ZhuanJie()
		{
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x00202BD9 File Offset: 0x00200DD9
		public ZhuanJie(CombatSkillKey skillKey) : base(skillKey, 40101)
		{
		}
	}
}
