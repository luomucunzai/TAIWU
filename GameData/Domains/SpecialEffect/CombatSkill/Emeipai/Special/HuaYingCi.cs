using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Special
{
	// Token: 0x02000540 RID: 1344
	public class HuaYingCi : ReduceEnemyNeiliAllocation
	{
		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06003FF6 RID: 16374 RVA: 0x0025C6F0 File Offset: 0x0025A8F0
		protected override byte AffectNeiliAllocationType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x0025C6F3 File Offset: 0x0025A8F3
		public HuaYingCi()
		{
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x0025C6FD File Offset: 0x0025A8FD
		public HuaYingCi(CombatSkillKey skillKey) : base(skillKey, 2404)
		{
		}
	}
}
