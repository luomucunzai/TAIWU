using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x0200060A RID: 1546
	public class HeiShuiELin : LoongBase
	{
		// Token: 0x0600454C RID: 17740 RVA: 0x0027202F File Offset: 0x0027022F
		public HeiShuiELin()
		{
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x00272039 File Offset: 0x00270239
		public HeiShuiELin(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x0600454E RID: 17742 RVA: 0x00272044 File Offset: 0x00270244
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongWaterImplementPoison(180, 1200);
				yield return new LoongWaterImplementResist();
				yield break;
			}
		}
	}
}
