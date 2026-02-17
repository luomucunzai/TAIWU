using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005FC RID: 1532
	public class BaiJiaoXiWei : LoongBase
	{
		// Token: 0x060044F8 RID: 17656 RVA: 0x00270F33 File Offset: 0x0026F133
		public BaiJiaoXiWei()
		{
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x00270F3D File Offset: 0x0026F13D
		public BaiJiaoXiWei(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060044FA RID: 17658 RVA: 0x00270F48 File Offset: 0x0026F148
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongMetalImplementJump();
				yield break;
			}
		}
	}
}
