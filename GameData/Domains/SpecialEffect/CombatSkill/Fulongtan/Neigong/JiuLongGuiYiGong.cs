using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Neigong
{
	// Token: 0x02000514 RID: 1300
	public class JiuLongGuiYiGong : ChangeFiveElementsDirection
	{
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06003EE6 RID: 16102 RVA: 0x002577A6 File Offset: 0x002559A6
		protected override sbyte FiveElementsType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06003EE7 RID: 16103 RVA: 0x002577A9 File Offset: 0x002559A9
		protected override byte NeiliAllocationType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x002577AC File Offset: 0x002559AC
		public JiuLongGuiYiGong()
		{
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x002577B6 File Offset: 0x002559B6
		public JiuLongGuiYiGong(CombatSkillKey skillKey) : base(skillKey, 14005)
		{
		}
	}
}
