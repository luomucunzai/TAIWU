using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.FistAndPalm
{
	// Token: 0x02000552 RID: 1362
	public class ShiErShouLanChanSiQuan : GameData.Domains.SpecialEffect.CombatSkill.Common.Attack.AttackHitType
	{
		// Token: 0x06004052 RID: 16466 RVA: 0x0025D9DE File Offset: 0x0025BBDE
		public ShiErShouLanChanSiQuan()
		{
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x0025D9E8 File Offset: 0x0025BBE8
		public ShiErShouLanChanSiQuan(CombatSkillKey skillKey) : base(skillKey, 2102)
		{
			this.AffectHitType = 2;
		}
	}
}
