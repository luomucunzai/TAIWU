using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Neigong
{
	// Token: 0x02000223 RID: 547
	public class TongZiXueLianFa : StrengthenFiveElementsTypeWithBoost
	{
		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06002F45 RID: 12101 RVA: 0x00212695 File Offset: 0x00210895
		protected override sbyte FiveElementsType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06002F46 RID: 12102 RVA: 0x00212698 File Offset: 0x00210898
		protected override byte CostNeiliAllocationType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x0021269B File Offset: 0x0021089B
		public TongZiXueLianFa()
		{
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x002126A5 File Offset: 0x002108A5
		public TongZiXueLianFa(CombatSkillKey skillKey) : base(skillKey, 15006)
		{
		}
	}
}
