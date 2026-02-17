using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Finger
{
	// Token: 0x02000558 RID: 1368
	public class EMeiYingZhuaGong : ChangePowerByEquipType
	{
		// Token: 0x06004077 RID: 16503 RVA: 0x0025E3B0 File Offset: 0x0025C5B0
		public EMeiYingZhuaGong()
		{
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x0025E3BA File Offset: 0x0025C5BA
		public EMeiYingZhuaGong(CombatSkillKey skillKey) : base(skillKey, 2200)
		{
			this.AffectEquipType = 2;
		}
	}
}
