using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.FistAndPalm
{
	// Token: 0x020004C0 RID: 1216
	public class JinGangZhuMieShou : CastAgainOrPowerUp
	{
		// Token: 0x06003D09 RID: 15625 RVA: 0x0024FA5D File Offset: 0x0024DC5D
		public JinGangZhuMieShou()
		{
		}

		// Token: 0x06003D0A RID: 15626 RVA: 0x0024FA67 File Offset: 0x0024DC67
		public JinGangZhuMieShou(CombatSkillKey skillKey) : base(skillKey, 11105)
		{
			this.RequireTrickType = 6;
		}
	}
}
