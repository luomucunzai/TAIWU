using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Neigong
{
	// Token: 0x02000512 RID: 1298
	public class DaYuYangShenGong : StrengthenFiveElementsTypeWithBoost
	{
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06003EE0 RID: 16096 RVA: 0x00257765 File Offset: 0x00255965
		protected override sbyte FiveElementsType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06003EE1 RID: 16097 RVA: 0x00257768 File Offset: 0x00255968
		protected override byte CostNeiliAllocationType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x0025776B File Offset: 0x0025596B
		public DaYuYangShenGong()
		{
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x00257775 File Offset: 0x00255975
		public DaYuYangShenGong(CombatSkillKey skillKey) : base(skillKey, 14007)
		{
		}
	}
}
