using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Throw
{
	// Token: 0x0200050E RID: 1294
	public class JiuLianJinWu : AccumulateNeiliAllocationToStrengthen
	{
		// Token: 0x06003ED7 RID: 16087 RVA: 0x002576C9 File Offset: 0x002558C9
		public JiuLianJinWu()
		{
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x002576D3 File Offset: 0x002558D3
		public JiuLianJinWu(CombatSkillKey skillKey) : base(skillKey, 14305)
		{
			this.RequireNeiliAllocationType = 0;
		}
	}
}
