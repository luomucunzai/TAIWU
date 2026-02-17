using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm
{
	// Token: 0x0200051B RID: 1307
	public class DuanGuQuan : AddMaxPowerOrUseRequirement
	{
		// Token: 0x06003F04 RID: 16132 RVA: 0x00257FC0 File Offset: 0x002561C0
		public DuanGuQuan()
		{
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x00257FCA File Offset: 0x002561CA
		public DuanGuQuan(CombatSkillKey skillKey) : base(skillKey, 14100)
		{
			this.AffectEquipType = 1;
		}
	}
}
