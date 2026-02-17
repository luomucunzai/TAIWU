using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Finger
{
	// Token: 0x020003A2 RID: 930
	public class WangGuJinCanJiang : AddWug
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06003693 RID: 13971 RVA: 0x00231203 File Offset: 0x0022F403
		protected override int AddWugCount
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x00231207 File Offset: 0x0022F407
		public WangGuJinCanJiang()
		{
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x00231211 File Offset: 0x0022F411
		public WangGuJinCanJiang(CombatSkillKey skillKey) : base(skillKey, 12207)
		{
			this.WugType = 6;
		}
	}
}
