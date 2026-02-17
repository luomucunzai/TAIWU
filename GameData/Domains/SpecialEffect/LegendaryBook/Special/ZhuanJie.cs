using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Special
{
	// Token: 0x02000136 RID: 310
	public class ZhuanJie : AddMaxPower
	{
		// Token: 0x06002A71 RID: 10865 RVA: 0x0020298D File Offset: 0x00200B8D
		public ZhuanJie()
		{
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x00202997 File Offset: 0x00200B97
		public ZhuanJie(CombatSkillKey skillKey) : base(skillKey, 41001)
		{
		}
	}
}
