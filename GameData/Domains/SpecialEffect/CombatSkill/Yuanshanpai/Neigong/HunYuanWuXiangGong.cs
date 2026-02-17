using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Neigong
{
	// Token: 0x020001F9 RID: 505
	public class HunYuanWuXiangGong : ChangeFiveElementsDirection
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06002E63 RID: 11875 RVA: 0x0020E9B1 File Offset: 0x0020CBB1
		protected override sbyte FiveElementsType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06002E64 RID: 11876 RVA: 0x0020E9B4 File Offset: 0x0020CBB4
		protected override byte NeiliAllocationType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x0020E9B7 File Offset: 0x0020CBB7
		public HunYuanWuXiangGong()
		{
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x0020E9C1 File Offset: 0x0020CBC1
		public HunYuanWuXiangGong(CombatSkillKey skillKey) : base(skillKey, 5004)
		{
		}
	}
}
