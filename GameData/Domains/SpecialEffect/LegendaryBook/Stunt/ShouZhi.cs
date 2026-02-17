using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Stunt
{
	// Token: 0x0200012D RID: 301
	public class ShouZhi : ReduceGridCost
	{
		// Token: 0x06002A56 RID: 10838 RVA: 0x0020252F File Offset: 0x0020072F
		public ShouZhi()
		{
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x00202539 File Offset: 0x00200739
		public ShouZhi(CombatSkillKey skillKey) : base(skillKey, 40206)
		{
		}
	}
}
