using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x02000609 RID: 1545
	public class XuanHaiELin : LoongBase
	{
		// Token: 0x06004549 RID: 17737 RVA: 0x00271FFA File Offset: 0x002701FA
		public XuanHaiELin()
		{
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x00272004 File Offset: 0x00270204
		public XuanHaiELin(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x0600454B RID: 17739 RVA: 0x00272010 File Offset: 0x00270210
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongWaterImplementPoison(120, 1800);
				yield return new LoongWaterImplementResist();
				yield return new LoongWaterImplementMix();
				yield return new LoongBaseImplementInvincible();
				yield break;
			}
		}
	}
}
