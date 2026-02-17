using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Agile
{
	// Token: 0x020003B1 RID: 945
	public class SheXingGong : AttackChangeMobility
	{
		// Token: 0x060036F0 RID: 14064 RVA: 0x00232FCC File Offset: 0x002311CC
		public SheXingGong()
		{
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x00232FD6 File Offset: 0x002311D6
		public SheXingGong(CombatSkillKey skillKey) : base(skillKey, 12600)
		{
			this.RequireWeaponSubType = 7;
		}
	}
}
