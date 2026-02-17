using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Special
{
	// Token: 0x02000545 RID: 1349
	public class WuXingCi : AddMaxPowerOrUseRequirement
	{
		// Token: 0x0600400B RID: 16395 RVA: 0x0025CB7A File Offset: 0x0025AD7A
		public WuXingCi()
		{
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x0025CB84 File Offset: 0x0025AD84
		public WuXingCi(CombatSkillKey skillKey) : base(skillKey, 2400)
		{
			this.AffectEquipType = 2;
		}
	}
}
