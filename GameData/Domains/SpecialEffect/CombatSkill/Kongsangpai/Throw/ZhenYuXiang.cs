using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw
{
	// Token: 0x0200047E RID: 1150
	public class ZhenYuXiang : PowerUpByPoison
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06003B9B RID: 15259 RVA: 0x00248E54 File Offset: 0x00247054
		protected override sbyte RequirePoisonType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06003B9C RID: 15260 RVA: 0x00248E57 File Offset: 0x00247057
		protected override short DirectStateId
		{
			get
			{
				return 210;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06003B9D RID: 15261 RVA: 0x00248E5E File Offset: 0x0024705E
		protected override short ReverseStateId
		{
			get
			{
				return 211;
			}
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x00248E65 File Offset: 0x00247065
		public ZhenYuXiang()
		{
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x00248E6F File Offset: 0x0024706F
		public ZhenYuXiang(CombatSkillKey skillKey) : base(skillKey, 10400)
		{
		}
	}
}
