using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x0200057C RID: 1404
	public abstract class StrengthenFiveElementsTypeSimple : StrengthenFiveElementsType
	{
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x00263128 File Offset: 0x00261328
		protected override int DirectAddPower
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600418E RID: 16782 RVA: 0x0026312C File Offset: 0x0026132C
		protected override int ReverseReduceCostPercent
		{
			get
			{
				return -10;
			}
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x00263130 File Offset: 0x00261330
		protected StrengthenFiveElementsTypeSimple()
		{
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x0026313A File Offset: 0x0026133A
		protected StrengthenFiveElementsTypeSimple(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}
	}
}
