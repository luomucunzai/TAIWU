using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Neigong
{
	// Token: 0x020003C5 RID: 965
	public class BaoYuanShouYi : StrengthenFiveElementsTypeSimple
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600376C RID: 14188 RVA: 0x00235914 File Offset: 0x00233B14
		protected override sbyte FiveElementsType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x00235917 File Offset: 0x00233B17
		public BaoYuanShouYi()
		{
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x00235921 File Offset: 0x00233B21
		public BaoYuanShouYi(CombatSkillKey skillKey) : base(skillKey, 4001)
		{
		}
	}
}
