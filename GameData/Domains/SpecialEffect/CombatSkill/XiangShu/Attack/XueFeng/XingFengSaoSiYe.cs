using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XueFeng
{
	// Token: 0x020002D1 RID: 721
	public class XingFengSaoSiYe : PowerUpOnCast
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060032A5 RID: 12965 RVA: 0x0022039F File Offset: 0x0021E59F
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x002203A2 File Offset: 0x0021E5A2
		public XingFengSaoSiYe()
		{
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x002203AC File Offset: 0x0021E5AC
		public XingFengSaoSiYe(CombatSkillKey skillKey) : base(skillKey, 17072)
		{
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x002203BC File Offset: 0x0021E5BC
		public override void OnEnable(DataContext context)
		{
			this.PowerUpValue = 10 * base.CombatChar.GetOldInjuries().GetSum();
			base.OnEnable(context);
		}

		// Token: 0x04000EFD RID: 3837
		private const sbyte AddPowerUnit = 10;
	}
}
