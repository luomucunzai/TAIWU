using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Sword
{
	// Token: 0x02000386 RID: 902
	public class GuiMianZhuiHunJian : AddPowerAndNeiliAllocationByMoving
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06003614 RID: 13844 RVA: 0x0022F4D5 File Offset: 0x0022D6D5
		protected override sbyte AddPowerUnit
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06003615 RID: 13845 RVA: 0x0022F4D9 File Offset: 0x0022D6D9
		protected override int MoveCostMobilityAddPercent
		{
			get
			{
				return -100;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06003616 RID: 13846 RVA: 0x0022F4DD File Offset: 0x0022D6DD
		protected override sbyte AddNeiliAllocationUnit
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x0022F4E0 File Offset: 0x0022D6E0
		public GuiMianZhuiHunJian()
		{
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x0022F4EA File Offset: 0x0022D6EA
		public GuiMianZhuiHunJian(CombatSkillKey skillKey) : base(skillKey, 12304)
		{
			this.AddNeiliAllocationType = 1;
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x0022F501 File Offset: 0x0022D701
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(175, EDataModifyType.Custom, -1);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x0022F522 File Offset: 0x0022D722
		protected override void OnDistanceChangedAddNeiliAllocation(DataContext context)
		{
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x0022F528 File Offset: 0x0022D728
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 175;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}
}
