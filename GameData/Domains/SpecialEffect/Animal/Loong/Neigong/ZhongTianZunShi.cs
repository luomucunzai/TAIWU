using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005EF RID: 1519
	public class ZhongTianZunShi : LoongBase
	{
		// Token: 0x060044B5 RID: 17589 RVA: 0x002706C7 File Offset: 0x0026E8C7
		public ZhongTianZunShi()
		{
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x002706D1 File Offset: 0x0026E8D1
		public ZhongTianZunShi(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060044B7 RID: 17591 RVA: 0x002706DC File Offset: 0x0026E8DC
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongEarthImplementSilencePower
				{
					SilenceFrame = 2700
				};
				yield break;
			}
		}
	}
}
