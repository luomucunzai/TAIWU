using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Finger
{
	// Token: 0x020003A0 RID: 928
	public class JiuYinShiChiGu : AddWug
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06003689 RID: 13961 RVA: 0x00230FDA File Offset: 0x0022F1DA
		protected override int AddWugCount
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x00230FDE File Offset: 0x0022F1DE
		public JiuYinShiChiGu()
		{
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x00230FE8 File Offset: 0x0022F1E8
		public JiuYinShiChiGu(CombatSkillKey skillKey) : base(skillKey, 12205)
		{
			this.WugType = 4;
		}
	}
}
