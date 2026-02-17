using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Finger
{
	// Token: 0x0200039E RID: 926
	public class ChiMuGuJiang : AddWug
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06003683 RID: 13955 RVA: 0x00230F92 File Offset: 0x0022F192
		protected override int AddWugCount
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x00230F95 File Offset: 0x0022F195
		public ChiMuGuJiang()
		{
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x00230F9F File Offset: 0x0022F19F
		public ChiMuGuJiang(CombatSkillKey skillKey) : base(skillKey, 12201)
		{
			this.WugType = 0;
		}
	}
}
