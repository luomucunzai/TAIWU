using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x0200060E RID: 1550
	public class CangKongWuLan : LoongBase
	{
		// Token: 0x0600455F RID: 17759 RVA: 0x00272380 File Offset: 0x00270580
		public CangKongWuLan()
		{
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x0027238A File Offset: 0x0027058A
		public CangKongWuLan(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06004561 RID: 17761 RVA: 0x00272398 File Offset: 0x00270598
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongWoodImplementHeal();
				yield return new LoongWoodImplementRange(1, 4);
				yield return new LoongBaseImplementInvincible();
				yield break;
			}
		}
	}
}
