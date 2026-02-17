using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Finger
{
	// Token: 0x0200039C RID: 924
	public class BiYuBingCanGu : AddWug
	{
		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600367D RID: 13949 RVA: 0x00230F4A File Offset: 0x0022F14A
		protected override int AddWugCount
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x00230F4D File Offset: 0x0022F14D
		public BiYuBingCanGu()
		{
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x00230F57 File Offset: 0x0022F157
		public BiYuBingCanGu(CombatSkillKey skillKey) : base(skillKey, 12206)
		{
			this.WugType = 5;
		}
	}
}
