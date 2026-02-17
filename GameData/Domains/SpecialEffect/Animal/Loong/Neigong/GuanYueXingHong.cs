using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005FA RID: 1530
	public class GuanYueXingHong : LoongBase
	{
		// Token: 0x060044F2 RID: 17650 RVA: 0x00270ECB File Offset: 0x0026F0CB
		public GuanYueXingHong()
		{
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x00270ED5 File Offset: 0x0026F0D5
		public GuanYueXingHong(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060044F4 RID: 17652 RVA: 0x00270EE0 File Offset: 0x0026F0E0
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongMetalImplementJump();
				yield return new LoongMetalImplementTeleport();
				yield return new LoongMetalImplementMindMark();
				yield return new LoongBaseImplementInvincible();
				yield break;
			}
		}
	}
}
