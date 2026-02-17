using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword
{
	// Token: 0x02000445 RID: 1093
	public class TaiBaoJiuHuaJian : AccumulateNeiliAllocationToStrengthen
	{
		// Token: 0x06003A24 RID: 14884 RVA: 0x00242282 File Offset: 0x00240482
		public TaiBaoJiuHuaJian()
		{
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x0024228C File Offset: 0x0024048C
		public TaiBaoJiuHuaJian(CombatSkillKey skillKey) : base(skillKey, 7205)
		{
			this.RequireNeiliAllocationType = 3;
		}
	}
}
