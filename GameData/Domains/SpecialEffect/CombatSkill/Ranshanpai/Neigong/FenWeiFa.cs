using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong
{
	// Token: 0x02000452 RID: 1106
	public class FenWeiFa : ChangeFiveElementsDirection
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06003A90 RID: 14992 RVA: 0x002441B3 File Offset: 0x002423B3
		protected override sbyte FiveElementsType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06003A91 RID: 14993 RVA: 0x002441B6 File Offset: 0x002423B6
		protected override byte NeiliAllocationType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x002441B9 File Offset: 0x002423B9
		public FenWeiFa()
		{
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x002441C3 File Offset: 0x002423C3
		public FenWeiFa(CombatSkillKey skillKey) : base(skillKey, 7005)
		{
		}
	}
}
