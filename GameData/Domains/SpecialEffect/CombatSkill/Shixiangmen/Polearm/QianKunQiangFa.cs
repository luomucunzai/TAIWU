using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm
{
	// Token: 0x020003EC RID: 1004
	public class QianKunQiangFa : AddPowerAndNeiliAllocationByMoving
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06003841 RID: 14401 RVA: 0x00239AD6 File Offset: 0x00237CD6
		protected override int MoveCostMobilityAddPercent
		{
			get
			{
				return -50;
			}
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x00239ADA File Offset: 0x00237CDA
		public QianKunQiangFa()
		{
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x00239AE4 File Offset: 0x00237CE4
		public QianKunQiangFa(CombatSkillKey skillKey) : base(skillKey, 6304)
		{
			this.AddNeiliAllocationType = 2;
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x00239AFB File Offset: 0x00237CFB
		protected override void OnDistanceChangedAddNeiliAllocation(DataContext context)
		{
			base.OnDistanceChangedAddNeiliAllocation(context);
			base.CombatChar.ChangeNeiliAllocation(context, 0, (int)this.AddNeiliAllocationUnit, true, true);
			base.ShowSpecialEffectTips(2);
		}
	}
}
