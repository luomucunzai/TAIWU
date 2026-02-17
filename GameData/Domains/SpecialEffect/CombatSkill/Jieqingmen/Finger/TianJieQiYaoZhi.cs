using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger
{
	// Token: 0x020004F6 RID: 1270
	public class TianJieQiYaoZhi : ReduceEnemyNeiliAllocation
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06003E4A RID: 15946 RVA: 0x002554FF File Offset: 0x002536FF
		protected override byte AffectNeiliAllocationType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x00255502 File Offset: 0x00253702
		public TianJieQiYaoZhi()
		{
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x0025550C File Offset: 0x0025370C
		public TianJieQiYaoZhi(CombatSkillKey skillKey) : base(skillKey, 13104)
		{
		}
	}
}
