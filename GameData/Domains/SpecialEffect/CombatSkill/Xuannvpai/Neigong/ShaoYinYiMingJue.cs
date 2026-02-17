using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Neigong
{
	// Token: 0x02000260 RID: 608
	public class ShaoYinYiMingJue : StrengthenMainAttribute
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06003052 RID: 12370 RVA: 0x00216BC8 File Offset: 0x00214DC8
		protected override bool ConsummateLevelRelatedMainAttributesPenetrations
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x00216BCB File Offset: 0x00214DCB
		public ShaoYinYiMingJue()
		{
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x00216BD5 File Offset: 0x00214DD5
		public ShaoYinYiMingJue(CombatSkillKey skillKey) : base(skillKey, 8003)
		{
			this.MainAttributeType = 4;
		}
	}
}
