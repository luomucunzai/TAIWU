using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.DefenseAndAssist
{
	// Token: 0x02000528 RID: 1320
	public class TianMangDanXinShu : TrickAddHitOrAvoid
	{
		// Token: 0x06003F50 RID: 16208 RVA: 0x00259537 File Offset: 0x00257737
		public TianMangDanXinShu()
		{
		}

		// Token: 0x06003F51 RID: 16209 RVA: 0x00259541 File Offset: 0x00257741
		public TianMangDanXinShu(CombatSkillKey skillKey) : base(skillKey, 14605)
		{
			this.RequireTrickTypes = new sbyte[]
			{
				6,
				8,
				7
			};
		}
	}
}
