using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Blade
{
	// Token: 0x020004CF RID: 1231
	public class JinGangXiangFuDao : ReduceEnemyNeiliAllocation
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06003D62 RID: 15714 RVA: 0x002517DD File Offset: 0x0024F9DD
		protected override byte AffectNeiliAllocationType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x002517E0 File Offset: 0x0024F9E0
		public JinGangXiangFuDao()
		{
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x002517EA File Offset: 0x0024F9EA
		public JinGangXiangFuDao(CombatSkillKey skillKey) : base(skillKey, 11204)
		{
		}
	}
}
