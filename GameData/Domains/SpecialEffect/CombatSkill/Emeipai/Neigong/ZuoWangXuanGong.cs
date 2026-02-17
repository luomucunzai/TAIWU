using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Neigong
{
	// Token: 0x0200054E RID: 1358
	public class ZuoWangXuanGong : StrengthenFiveElementsTypeWithBoost
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06004037 RID: 16439 RVA: 0x0025D29D File Offset: 0x0025B49D
		protected override sbyte FiveElementsType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06004038 RID: 16440 RVA: 0x0025D2A0 File Offset: 0x0025B4A0
		protected override byte CostNeiliAllocationType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x0025D2A3 File Offset: 0x0025B4A3
		public ZuoWangXuanGong()
		{
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x0025D2AD File Offset: 0x0025B4AD
		public ZuoWangXuanGong(CombatSkillKey skillKey) : base(skillKey, 2006)
		{
		}
	}
}
