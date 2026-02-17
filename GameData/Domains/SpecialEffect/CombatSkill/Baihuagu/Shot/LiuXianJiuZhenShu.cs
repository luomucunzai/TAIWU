using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot
{
	// Token: 0x020005BA RID: 1466
	public class LiuXianJiuZhenShu : StrengthenPoison
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x0600438D RID: 17293 RVA: 0x0026BCE8 File Offset: 0x00269EE8
		protected override bool Variant1
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x0026BCEB File Offset: 0x00269EEB
		public LiuXianJiuZhenShu()
		{
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x0026BCF5 File Offset: 0x00269EF5
		public LiuXianJiuZhenShu(CombatSkillKey skillKey) : base(skillKey, 3206)
		{
			this.AffectPoisonType = 5;
		}
	}
}
