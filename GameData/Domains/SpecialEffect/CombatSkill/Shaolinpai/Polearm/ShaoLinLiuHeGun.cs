using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Polearm
{
	// Token: 0x02000415 RID: 1045
	public class ShaoLinLiuHeGun : AddMaxPowerOrUseRequirement
	{
		// Token: 0x06003931 RID: 14641 RVA: 0x0023DAD6 File Offset: 0x0023BCD6
		public ShaoLinLiuHeGun()
		{
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x0023DAE0 File Offset: 0x0023BCE0
		public ShaoLinLiuHeGun(CombatSkillKey skillKey) : base(skillKey, 1300)
		{
			this.AffectEquipType = 3;
		}
	}
}
