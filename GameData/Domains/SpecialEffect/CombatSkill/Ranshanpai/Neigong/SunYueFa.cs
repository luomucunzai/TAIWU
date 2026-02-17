using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong
{
	// Token: 0x02000456 RID: 1110
	public class SunYueFa : StrengthenMainAttribute
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06003AA4 RID: 15012 RVA: 0x00244569 File Offset: 0x00242769
		protected override bool ConsummateLevelRelatedMainAttributesHitAvoid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x0024456C File Offset: 0x0024276C
		public SunYueFa()
		{
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x00244576 File Offset: 0x00242776
		public SunYueFa(CombatSkillKey skillKey) : base(skillKey, 7003)
		{
			this.MainAttributeType = 5;
		}
	}
}
