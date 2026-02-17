using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x02000610 RID: 1552
	public class QingJiaoTieJi : LoongBase
	{
		// Token: 0x06004565 RID: 17765 RVA: 0x002723EB File Offset: 0x002705EB
		public QingJiaoTieJi()
		{
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x002723F5 File Offset: 0x002705F5
		public QingJiaoTieJi(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06004567 RID: 17767 RVA: 0x00272400 File Offset: 0x00270600
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongWoodImplementHeal();
				yield break;
			}
		}
	}
}
