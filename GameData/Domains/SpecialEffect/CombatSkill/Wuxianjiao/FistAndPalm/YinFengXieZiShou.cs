using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.FistAndPalm
{
	// Token: 0x02000399 RID: 921
	public class YinFengXieZiShou : PowerUpByPoison
	{
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06003672 RID: 13938 RVA: 0x00230ECF File Offset: 0x0022F0CF
		protected override sbyte RequirePoisonType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06003673 RID: 13939 RVA: 0x00230ED2 File Offset: 0x0022F0D2
		protected override short DirectStateId
		{
			get
			{
				return 212;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06003674 RID: 13940 RVA: 0x00230ED9 File Offset: 0x0022F0D9
		protected override short ReverseStateId
		{
			get
			{
				return 213;
			}
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x00230EE0 File Offset: 0x0022F0E0
		public YinFengXieZiShou()
		{
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x00230EEA File Offset: 0x0022F0EA
		public YinFengXieZiShou(CombatSkillKey skillKey) : base(skillKey, 12100)
		{
		}
	}
}
