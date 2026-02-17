using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x0200059F RID: 1439
	public class PowerUpByMainAttribute : PowerUpOnCast
	{
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060042C1 RID: 17089 RVA: 0x00268522 File Offset: 0x00266722
		private CValuePercent AddPowerFactor
		{
			get
			{
				return base.IsDirect ? 40 : 60;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060042C2 RID: 17090 RVA: 0x00268537 File Offset: 0x00266737
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x0026853A File Offset: 0x0026673A
		public PowerUpByMainAttribute()
		{
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x00268544 File Offset: 0x00266744
		public PowerUpByMainAttribute(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x00268550 File Offset: 0x00266750
		public unsafe override void OnEnable(DataContext context)
		{
			short currValue = *(ref this.CharObj.GetCurrMainAttributes().Items.FixedElementField + (IntPtr)this.RequireMainAttributeType * 2);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.PowerUpValue = (int)currValue;
			}
			else
			{
				this.PowerUpValue = (int)(*(ref this.CharObj.GetMaxMainAttributes().Items.FixedElementField + (IntPtr)this.RequireMainAttributeType * 2) - currValue);
			}
			this.PowerUpValue *= this.AddPowerFactor;
			base.OnEnable(context);
		}

		// Token: 0x040013C1 RID: 5057
		protected sbyte RequireMainAttributeType;
	}
}
