using System;
using System.Collections.Generic;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005F6 RID: 1526
	public class ChiJiaoHuoJing : LoongBase
	{
		// Token: 0x060044D3 RID: 17619 RVA: 0x002709A7 File Offset: 0x0026EBA7
		public ChiJiaoHuoJing()
		{
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x002709B1 File Offset: 0x0026EBB1
		public ChiJiaoHuoJing(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060044D5 RID: 17621 RVA: 0x002709BC File Offset: 0x0026EBBC
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongFireImplementWorse(WorsenConstants.DefaultPercent);
				yield break;
			}
		}
	}
}
