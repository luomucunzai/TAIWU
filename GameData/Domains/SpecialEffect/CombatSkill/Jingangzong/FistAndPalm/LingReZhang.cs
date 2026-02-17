using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.FistAndPalm
{
	// Token: 0x020004C1 RID: 1217
	public class LingReZhang : AccumulateNeiliAllocationToStrengthen
	{
		// Token: 0x06003D0B RID: 15627 RVA: 0x0024FA7E File Offset: 0x0024DC7E
		public LingReZhang()
		{
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x0024FA88 File Offset: 0x0024DC88
		public LingReZhang(CombatSkillKey skillKey) : base(skillKey, 11104)
		{
			this.RequireNeiliAllocationType = 2;
		}
	}
}
