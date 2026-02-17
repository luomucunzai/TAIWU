using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x0200060F RID: 1551
	public class CangKongJiYu : LoongBase
	{
		// Token: 0x06004562 RID: 17762 RVA: 0x002723B7 File Offset: 0x002705B7
		public CangKongJiYu()
		{
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x002723C1 File Offset: 0x002705C1
		public CangKongJiYu(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06004564 RID: 17764 RVA: 0x002723CC File Offset: 0x002705CC
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongWoodImplementHeal();
				yield return new LoongWoodImplementRange(1, 2);
				yield break;
			}
		}
	}
}
