using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Polearm
{
	// Token: 0x02000413 RID: 1043
	public class DaZhiPuTiZhangFa : CastAgainOrPowerUp
	{
		// Token: 0x06003926 RID: 14630 RVA: 0x0023D784 File Offset: 0x0023B984
		public DaZhiPuTiZhangFa()
		{
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x0023D78E File Offset: 0x0023B98E
		public DaZhiPuTiZhangFa(CombatSkillKey skillKey) : base(skillKey, 1307)
		{
			this.RequireTrickType = 5;
		}
	}
}
