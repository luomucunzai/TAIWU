using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Stunt
{
	// Token: 0x02000130 RID: 304
	public class ZhuanJie : AddMaxPower
	{
		// Token: 0x06002A60 RID: 10848 RVA: 0x002026E5 File Offset: 0x002008E5
		public ZhuanJie()
		{
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x002026EF File Offset: 0x002008EF
		public ZhuanJie(CombatSkillKey skillKey) : base(skillKey, 40201)
		{
		}
	}
}
