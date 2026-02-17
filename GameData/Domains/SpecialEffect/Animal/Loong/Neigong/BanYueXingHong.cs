using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005FB RID: 1531
	public class BanYueXingHong : LoongBase
	{
		// Token: 0x060044F5 RID: 17653 RVA: 0x00270EFF File Offset: 0x0026F0FF
		public BanYueXingHong()
		{
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x00270F09 File Offset: 0x0026F109
		public BanYueXingHong(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060044F7 RID: 17655 RVA: 0x00270F14 File Offset: 0x0026F114
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongMetalImplementJump();
				yield return new LoongMetalImplementTeleport();
				yield break;
			}
		}
	}
}
