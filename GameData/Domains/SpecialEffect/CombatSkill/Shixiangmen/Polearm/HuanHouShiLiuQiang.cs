using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm
{
	// Token: 0x020003EA RID: 1002
	public class HuanHouShiLiuQiang : PowerUpByMainAttribute
	{
		// Token: 0x06003836 RID: 14390 RVA: 0x002396F9 File Offset: 0x002378F9
		public HuanHouShiLiuQiang()
		{
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x00239703 File Offset: 0x00237903
		public HuanHouShiLiuQiang(CombatSkillKey skillKey) : base(skillKey, 6303)
		{
			this.RequireMainAttributeType = 3;
		}
	}
}
