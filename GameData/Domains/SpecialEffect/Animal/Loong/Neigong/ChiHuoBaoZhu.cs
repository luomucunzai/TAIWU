using System;
using System.Collections.Generic;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005F5 RID: 1525
	public class ChiHuoBaoZhu : LoongBase
	{
		// Token: 0x060044D0 RID: 17616 RVA: 0x00270973 File Offset: 0x0026EB73
		public ChiHuoBaoZhu()
		{
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x0027097D File Offset: 0x0026EB7D
		public ChiHuoBaoZhu(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060044D2 RID: 17618 RVA: 0x00270988 File Offset: 0x0026EB88
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongFireImplementWorse(WorsenConstants.HighPercent);
				yield return new LoongFireImplementBroken();
				yield break;
			}
		}
	}
}
