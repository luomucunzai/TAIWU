using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Neigong
{
	// Token: 0x020004F1 RID: 1265
	public class YiSiYiCan : ChangeFiveElementsDirection
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06003E2C RID: 15916 RVA: 0x00254DC3 File Offset: 0x00252FC3
		protected override sbyte FiveElementsType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06003E2D RID: 15917 RVA: 0x00254DC6 File Offset: 0x00252FC6
		protected override byte NeiliAllocationType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x00254DC9 File Offset: 0x00252FC9
		public YiSiYiCan()
		{
		}

		// Token: 0x06003E2F RID: 15919 RVA: 0x00254DD3 File Offset: 0x00252FD3
		public YiSiYiCan(CombatSkillKey skillKey) : base(skillKey, 13005)
		{
		}
	}
}
