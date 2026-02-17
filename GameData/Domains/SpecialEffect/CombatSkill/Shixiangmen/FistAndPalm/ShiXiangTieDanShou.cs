using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm
{
	// Token: 0x020003FB RID: 1019
	public class ShiXiangTieDanShou : GetTrick
	{
		// Token: 0x06003896 RID: 14486 RVA: 0x0023B07E File Offset: 0x0023927E
		public ShiXiangTieDanShou()
		{
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x0023B088 File Offset: 0x00239288
		public ShiXiangTieDanShou(CombatSkillKey skillKey) : base(skillKey, 6101)
		{
			this.GetTrickType = 6;
			this.DirectCanChangeTrickType = new sbyte[]
			{
				7,
				8
			};
		}
	}
}
