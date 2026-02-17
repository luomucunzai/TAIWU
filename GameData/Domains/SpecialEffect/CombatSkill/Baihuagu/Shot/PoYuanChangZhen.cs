using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot
{
	// Token: 0x020005BB RID: 1467
	public class PoYuanChangZhen : ReduceEnemyNeiliAllocation
	{
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06004390 RID: 17296 RVA: 0x0026BD0C File Offset: 0x00269F0C
		protected override byte AffectNeiliAllocationType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x0026BD0F File Offset: 0x00269F0F
		public PoYuanChangZhen()
		{
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0026BD19 File Offset: 0x00269F19
		public PoYuanChangZhen(CombatSkillKey skillKey) : base(skillKey, 3204)
		{
		}
	}
}
