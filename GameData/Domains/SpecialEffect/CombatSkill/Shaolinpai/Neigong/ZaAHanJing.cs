using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Neigong
{
	// Token: 0x02000422 RID: 1058
	public class ZaAHanJing : ChangeFiveElementsDirection
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600395A RID: 14682 RVA: 0x0023E1A1 File Offset: 0x0023C3A1
		protected override sbyte FiveElementsType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600395B RID: 14683 RVA: 0x0023E1A4 File Offset: 0x0023C3A4
		protected override byte NeiliAllocationType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x0023E1A7 File Offset: 0x0023C3A7
		public ZaAHanJing()
		{
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x0023E1B1 File Offset: 0x0023C3B1
		public ZaAHanJing(CombatSkillKey skillKey) : base(skillKey, 1005)
		{
		}
	}
}
