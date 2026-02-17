using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Neigong
{
	// Token: 0x020003F2 RID: 1010
	public class GuangMingShiZiJin : StrengthenMainAttribute
	{
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06003867 RID: 14439 RVA: 0x0023A429 File Offset: 0x00238629
		protected override bool ConsummateLevelRelatedMainAttributesHitAvoid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x0023A42C File Offset: 0x0023862C
		public GuangMingShiZiJin()
		{
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x0023A436 File Offset: 0x00238636
		public GuangMingShiZiJin(CombatSkillKey skillKey) : base(skillKey, 6003)
		{
			this.MainAttributeType = 0;
		}
	}
}
