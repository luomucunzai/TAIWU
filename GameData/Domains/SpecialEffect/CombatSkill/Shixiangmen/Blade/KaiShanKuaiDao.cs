using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade
{
	// Token: 0x02000408 RID: 1032
	public class KaiShanKuaiDao : GetTrick
	{
		// Token: 0x060038E1 RID: 14561 RVA: 0x0023C51D File Offset: 0x0023A71D
		public KaiShanKuaiDao()
		{
		}

		// Token: 0x060038E2 RID: 14562 RVA: 0x0023C527 File Offset: 0x0023A727
		public KaiShanKuaiDao(CombatSkillKey skillKey) : base(skillKey, 6201)
		{
			this.GetTrickType = 3;
			this.DirectCanChangeTrickType = new sbyte[]
			{
				4,
				5
			};
		}
	}
}
