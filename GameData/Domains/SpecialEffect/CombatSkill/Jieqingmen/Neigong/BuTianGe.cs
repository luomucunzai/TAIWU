using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Neigong
{
	// Token: 0x020004EB RID: 1259
	public class BuTianGe : StrengthenMainAttribute
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06003E1E RID: 15902 RVA: 0x00254CD9 File Offset: 0x00252ED9
		protected override bool ConsummateLevelRelatedMainAttributesHitAvoid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x00254CDC File Offset: 0x00252EDC
		public BuTianGe()
		{
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x00254CE6 File Offset: 0x00252EE6
		public BuTianGe(CombatSkillKey skillKey) : base(skillKey, 13003)
		{
			this.MainAttributeType = 1;
		}
	}
}
