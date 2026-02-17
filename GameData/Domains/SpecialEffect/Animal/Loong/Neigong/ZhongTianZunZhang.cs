using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005EE RID: 1518
	public class ZhongTianZunZhang : LoongBase
	{
		// Token: 0x060044B2 RID: 17586 RVA: 0x00270691 File Offset: 0x0026E891
		public ZhongTianZunZhang()
		{
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x0027069B File Offset: 0x0026E89B
		public ZhongTianZunZhang(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060044B4 RID: 17588 RVA: 0x002706A8 File Offset: 0x0026E8A8
		protected override IEnumerable<ISpecialEffectImplement> Implements
		{
			get
			{
				yield return new LoongEarthImplementSilenceColorful
				{
					SilenceFrame = 3600
				};
				yield return new LoongBaseImplementInvincible();
				yield break;
			}
		}
	}
}
