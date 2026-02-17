using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong
{
	// Token: 0x020004B5 RID: 1205
	public class JinGangDingJing : StrengthenFiveElementsTypeWithBoost
	{
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x0024F4B8 File Offset: 0x0024D6B8
		protected override sbyte FiveElementsType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06003CE6 RID: 15590 RVA: 0x0024F4BB File Offset: 0x0024D6BB
		protected override byte CostNeiliAllocationType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x0024F4BE File Offset: 0x0024D6BE
		public JinGangDingJing()
		{
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x0024F4C8 File Offset: 0x0024D6C8
		public JinGangDingJing(CombatSkillKey skillKey) : base(skillKey, 11006)
		{
		}
	}
}
