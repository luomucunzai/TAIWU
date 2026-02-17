using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm
{
	// Token: 0x02000520 RID: 1312
	public class LiHuoLiuYangZhang : AttackNeiliFiveElementsType
	{
		// Token: 0x06003F21 RID: 16161 RVA: 0x00258A7A File Offset: 0x00256C7A
		public LiHuoLiuYangZhang()
		{
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x00258A84 File Offset: 0x00256C84
		public LiHuoLiuYangZhang(CombatSkillKey skillKey) : base(skillKey, 14106)
		{
			this.AffectFiveElementsType = 0;
		}
	}
}
