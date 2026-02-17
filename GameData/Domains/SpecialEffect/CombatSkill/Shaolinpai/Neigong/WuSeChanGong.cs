using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Neigong
{
	// Token: 0x0200041E RID: 1054
	public class WuSeChanGong : StrengthenMainAttribute
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06003950 RID: 14672 RVA: 0x0023E105 File Offset: 0x0023C305
		protected override bool ConsummateLevelRelatedMainAttributesHitAvoid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x0023E108 File Offset: 0x0023C308
		public WuSeChanGong()
		{
		}

		// Token: 0x06003952 RID: 14674 RVA: 0x0023E112 File Offset: 0x0023C312
		public WuSeChanGong(CombatSkillKey skillKey) : base(skillKey, 1003)
		{
			this.MainAttributeType = 2;
		}
	}
}
