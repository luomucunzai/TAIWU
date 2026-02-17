using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Neigong
{
	// Token: 0x020004EF RID: 1263
	public class TianYuanYangQiFa : StrengthenFiveElementsTypeSimple
	{
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06003E27 RID: 15911 RVA: 0x00254D73 File Offset: 0x00252F73
		protected override sbyte FiveElementsType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x00254D76 File Offset: 0x00252F76
		public TianYuanYangQiFa()
		{
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x00254D80 File Offset: 0x00252F80
		public TianYuanYangQiFa(CombatSkillKey skillKey) : base(skillKey, 13001)
		{
		}
	}
}
