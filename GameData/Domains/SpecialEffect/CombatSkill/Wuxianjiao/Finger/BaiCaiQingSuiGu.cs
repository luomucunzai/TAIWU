using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Finger
{
	// Token: 0x0200039B RID: 923
	public class BaiCaiQingSuiGu : AddWug
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600367A RID: 13946 RVA: 0x00230F25 File Offset: 0x0022F125
		protected override int AddWugCount
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x00230F29 File Offset: 0x0022F129
		public BaiCaiQingSuiGu()
		{
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x00230F33 File Offset: 0x0022F133
		public BaiCaiQingSuiGu(CombatSkillKey skillKey) : base(skillKey, 12208)
		{
			this.WugType = 7;
		}
	}
}
