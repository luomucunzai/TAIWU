using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Sword
{
	// Token: 0x020003BD RID: 957
	public class ChunYangJianYi : PowerUpOnCast
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06003735 RID: 14133 RVA: 0x002348DC File Offset: 0x00232ADC
		private int AddPowerPercent
		{
			get
			{
				return base.IsDirect ? 12 : 6;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06003736 RID: 14134 RVA: 0x002348EB File Offset: 0x00232AEB
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x002348EE File Offset: 0x00232AEE
		public ChunYangJianYi()
		{
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x002348F8 File Offset: 0x00232AF8
		public ChunYangJianYi(CombatSkillKey skillKey) : base(skillKey, 4204)
		{
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x00234908 File Offset: 0x00232B08
		public override void OnEnable(DataContext context)
		{
			bool flag = base.IsDirect == this.CharObj.HasVirginity();
			if (flag)
			{
				this.PowerUpValue = (int)this.CharObj.GetInnerRatio() * this.AddPowerPercent / 100;
			}
			base.OnEnable(context);
		}
	}
}
