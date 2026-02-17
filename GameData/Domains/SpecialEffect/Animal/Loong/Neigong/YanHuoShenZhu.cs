using System;
using System.Collections.Generic;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005F4 RID: 1524
	public class YanHuoShenZhu : LoongBase
	{
		// Token: 0x060044CD RID: 17613 RVA: 0x0027093D File Offset: 0x0026EB3D
		public YanHuoShenZhu()
		{
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x00270947 File Offset: 0x0026EB47
		public YanHuoShenZhu(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060044CF RID: 17615 RVA: 0x00270954 File Offset: 0x0026EB54
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongFireImplementWorse(WorsenConstants.SpecialPercentLoongFire);
				yield return new LoongFireImplementBroken();
				yield return new LoongFireImplementBrokenHit();
				yield return new LoongBaseImplementInvincible();
				yield break;
			}
		}
	}
}
