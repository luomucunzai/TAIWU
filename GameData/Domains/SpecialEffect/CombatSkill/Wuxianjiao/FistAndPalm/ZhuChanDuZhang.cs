using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.FistAndPalm
{
	// Token: 0x0200039A RID: 922
	public class ZhuChanDuZhang : AddInjuryByPoisonMark
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06003677 RID: 13943 RVA: 0x00230EFA File Offset: 0x0022F0FA
		protected override bool AlsoAddFlaw
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x00230EFD File Offset: 0x0022F0FD
		public ZhuChanDuZhang()
		{
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x00230F07 File Offset: 0x0022F107
		public ZhuChanDuZhang(CombatSkillKey skillKey) : base(skillKey, 12105)
		{
			this.RequirePoisonType = 3;
			this.IsInnerInjury = false;
		}
	}
}
