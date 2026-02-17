using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Neigong
{
	// Token: 0x0200038D RID: 909
	public class ChiXianFa : StrengthenFiveElementsTypeSimple
	{
		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600363D RID: 13885 RVA: 0x002302A1 File Offset: 0x0022E4A1
		protected override sbyte FiveElementsType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x002302A4 File Offset: 0x0022E4A4
		public ChiXianFa()
		{
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x002302AE File Offset: 0x0022E4AE
		public ChiXianFa(CombatSkillKey skillKey) : base(skillKey, 12002)
		{
		}
	}
}
