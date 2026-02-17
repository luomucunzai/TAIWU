using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.FistAndPalm
{
	// Token: 0x020004BE RID: 1214
	public class JinGangChui : ReduceEnemyTrick
	{
		// Token: 0x06003D04 RID: 15620 RVA: 0x0024F9B5 File Offset: 0x0024DBB5
		public JinGangChui()
		{
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x0024F9BF File Offset: 0x0024DBBF
		public JinGangChui(CombatSkillKey skillKey) : base(skillKey, 11100)
		{
			this.AffectTrickType = 6;
		}
	}
}
