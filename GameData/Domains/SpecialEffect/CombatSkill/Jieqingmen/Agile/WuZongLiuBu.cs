using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Agile
{
	// Token: 0x0200050A RID: 1290
	public class WuZongLiuBu : MakeTricksInterchangeable
	{
		// Token: 0x06003EC1 RID: 16065 RVA: 0x002570C0 File Offset: 0x002552C0
		public WuZongLiuBu()
		{
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x002570CA File Offset: 0x002552CA
		public WuZongLiuBu(CombatSkillKey skillKey) : base(skillKey, 13401)
		{
			this.AffectTrickTypes = new List<sbyte>
			{
				7,
				4,
				1
			};
		}
	}
}
