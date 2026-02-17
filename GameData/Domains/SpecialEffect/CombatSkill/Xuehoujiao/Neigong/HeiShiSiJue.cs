using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Neigong
{
	// Token: 0x0200021F RID: 543
	public class HeiShiSiJue : StrengthenFiveElementsTypeSimple
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06002F31 RID: 12081 RVA: 0x002122BC File Offset: 0x002104BC
		protected override sbyte FiveElementsType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x002122BF File Offset: 0x002104BF
		public HeiShiSiJue()
		{
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x002122C9 File Offset: 0x002104C9
		public HeiShiSiJue(CombatSkillKey skillKey) : base(skillKey, 15001)
		{
		}
	}
}
