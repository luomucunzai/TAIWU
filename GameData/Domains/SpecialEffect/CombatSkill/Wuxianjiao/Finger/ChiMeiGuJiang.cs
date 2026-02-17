using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Finger
{
	// Token: 0x0200039D RID: 925
	public class ChiMeiGuJiang : AddWug
	{
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06003680 RID: 13952 RVA: 0x00230F6E File Offset: 0x0022F16E
		protected override int AddWugCount
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x00230F71 File Offset: 0x0022F171
		public ChiMeiGuJiang()
		{
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x00230F7B File Offset: 0x0022F17B
		public ChiMeiGuJiang(CombatSkillKey skillKey) : base(skillKey, 12202)
		{
			this.WugType = 1;
		}
	}
}
