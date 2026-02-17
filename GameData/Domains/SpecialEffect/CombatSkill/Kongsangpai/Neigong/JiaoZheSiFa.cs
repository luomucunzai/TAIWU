using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Neigong
{
	// Token: 0x02000480 RID: 1152
	public class JiaoZheSiFa : StrengthenMainAttribute
	{
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06003BA4 RID: 15268 RVA: 0x0024901F File Offset: 0x0024721F
		protected override bool ConsummateLevelRelatedMainAttributesPenetrations
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x00249022 File Offset: 0x00247222
		public JiaoZheSiFa()
		{
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x0024902C File Offset: 0x0024722C
		public JiaoZheSiFa(CombatSkillKey skillKey) : base(skillKey, 10003)
		{
			this.MainAttributeType = 3;
		}
	}
}
