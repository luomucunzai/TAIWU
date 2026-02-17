using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Throw
{
	// Token: 0x0200050F RID: 1295
	public class LeiHuoDa : GetTrick
	{
		// Token: 0x06003ED9 RID: 16089 RVA: 0x002576EA File Offset: 0x002558EA
		public LeiHuoDa()
		{
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x002576F4 File Offset: 0x002558F4
		public LeiHuoDa(CombatSkillKey skillKey) : base(skillKey, 14301)
		{
			this.GetTrickType = 0;
			this.DirectCanChangeTrickType = new sbyte[]
			{
				1,
				2
			};
		}
	}
}
