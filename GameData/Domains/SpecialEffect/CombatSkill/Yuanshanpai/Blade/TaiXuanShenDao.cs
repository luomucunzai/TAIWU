using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Blade
{
	// Token: 0x02000210 RID: 528
	public class TaiXuanShenDao : ExtraBreathOrStance
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06002EE9 RID: 12009 RVA: 0x002110C7 File Offset: 0x0020F2C7
		protected override bool IsBreath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x002110CA File Offset: 0x0020F2CA
		public TaiXuanShenDao()
		{
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x002110D4 File Offset: 0x0020F2D4
		public TaiXuanShenDao(CombatSkillKey skillKey) : base(skillKey, 5207)
		{
		}
	}
}
