using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005F0 RID: 1520
	public class HuangJiaoDuJiao : LoongBase
	{
		// Token: 0x060044B8 RID: 17592 RVA: 0x002706FB File Offset: 0x0026E8FB
		public HuangJiaoDuJiao()
		{
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x00270705 File Offset: 0x0026E905
		public HuangJiaoDuJiao(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060044BA RID: 17594 RVA: 0x00270710 File Offset: 0x0026E910
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongEarthImplementSilence
				{
					SilenceFrame = 1800
				};
				yield break;
			}
		}
	}
}
