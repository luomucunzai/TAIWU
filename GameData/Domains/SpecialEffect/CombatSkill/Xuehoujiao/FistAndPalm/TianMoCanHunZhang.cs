using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.FistAndPalm
{
	// Token: 0x02000235 RID: 565
	public class TianMoCanHunZhang : StrengthenPoison
	{
		// Token: 0x06002F90 RID: 12176 RVA: 0x00213928 File Offset: 0x00211B28
		public TianMoCanHunZhang()
		{
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x00213932 File Offset: 0x00211B32
		public TianMoCanHunZhang(CombatSkillKey skillKey) : base(skillKey, 15106)
		{
			this.AffectPoisonType = 0;
		}
	}
}
