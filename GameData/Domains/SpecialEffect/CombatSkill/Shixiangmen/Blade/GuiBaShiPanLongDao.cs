using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade
{
	// Token: 0x02000405 RID: 1029
	public class GuiBaShiPanLongDao : CastAgainOrPowerUp
	{
		// Token: 0x060038D2 RID: 14546 RVA: 0x0023C0B4 File Offset: 0x0023A2B4
		public GuiBaShiPanLongDao()
		{
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x0023C0BE File Offset: 0x0023A2BE
		public GuiBaShiPanLongDao(CombatSkillKey skillKey) : base(skillKey, 6207)
		{
			this.RequireTrickType = 3;
		}
	}
}
