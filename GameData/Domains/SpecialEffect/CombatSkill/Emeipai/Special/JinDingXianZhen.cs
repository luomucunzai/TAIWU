using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Special
{
	// Token: 0x02000541 RID: 1345
	public class JinDingXianZhen : CastAgainOrPowerUp
	{
		// Token: 0x06003FF9 RID: 16377 RVA: 0x0025C70D File Offset: 0x0025A90D
		public JinDingXianZhen()
		{
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x0025C717 File Offset: 0x0025A917
		public JinDingXianZhen(CombatSkillKey skillKey) : base(skillKey, 2407)
		{
			this.RequireTrickType = 4;
		}
	}
}
