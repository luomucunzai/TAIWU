using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x0200060B RID: 1547
	public class HeiJiaoELin : LoongBase
	{
		// Token: 0x0600454F RID: 17743 RVA: 0x00272063 File Offset: 0x00270263
		public HeiJiaoELin()
		{
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x0027206D File Offset: 0x0027026D
		public HeiJiaoELin(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06004551 RID: 17745 RVA: 0x00272078 File Offset: 0x00270278
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongWaterImplementPoison(240, 600);
				yield break;
			}
		}
	}
}
