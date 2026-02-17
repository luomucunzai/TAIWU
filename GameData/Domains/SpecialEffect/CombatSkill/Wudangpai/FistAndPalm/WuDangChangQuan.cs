using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm
{
	// Token: 0x020003D0 RID: 976
	public class WuDangChangQuan : AddMaxPowerOrUseRequirement
	{
		// Token: 0x06003799 RID: 14233 RVA: 0x00236589 File Offset: 0x00234789
		public WuDangChangQuan()
		{
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x00236593 File Offset: 0x00234793
		public WuDangChangQuan(CombatSkillKey skillKey) : base(skillKey, 4100)
		{
			this.AffectEquipType = 4;
		}
	}
}
