using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Finger
{
	// Token: 0x02000278 RID: 632
	public class ChangXiShiErShi : CastAgainOrPowerUp
	{
		// Token: 0x060030BA RID: 12474 RVA: 0x00218636 File Offset: 0x00216836
		public ChangXiShiErShi()
		{
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x00218640 File Offset: 0x00216840
		public ChangXiShiErShi(CombatSkillKey skillKey) : base(skillKey, 8205)
		{
			this.RequireTrickType = 7;
		}
	}
}
