using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Neigong
{
	// Token: 0x02000266 RID: 614
	public class YaoChiXianYuFa : StrengthenFiveElementsTypeWithBoost
	{
		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06003060 RID: 12384 RVA: 0x00216CA6 File Offset: 0x00214EA6
		protected override sbyte FiveElementsType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06003061 RID: 12385 RVA: 0x00216CA9 File Offset: 0x00214EA9
		protected override byte CostNeiliAllocationType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x00216CAC File Offset: 0x00214EAC
		public YaoChiXianYuFa()
		{
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x00216CB6 File Offset: 0x00214EB6
		public YaoChiXianYuFa(CombatSkillKey skillKey) : base(skillKey, 8006)
		{
		}
	}
}
