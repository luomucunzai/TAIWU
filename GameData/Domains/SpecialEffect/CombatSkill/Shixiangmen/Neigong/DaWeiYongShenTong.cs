using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Neigong
{
	// Token: 0x020003F0 RID: 1008
	public class DaWeiYongShenTong : ReduceMainAttribute
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x0600385A RID: 14426 RVA: 0x0023A279 File Offset: 0x00238479
		private int MaxAddValue
		{
			get
			{
				return base.IsDirect ? 50 : 100;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600385B RID: 14427 RVA: 0x0023A289 File Offset: 0x00238489
		protected override bool IsAffect
		{
			get
			{
				return this.AffectChar.GetDefeatMarkCollection().FatalDamageMarkCount >= 4;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600385C RID: 14428 RVA: 0x0023A2A1 File Offset: 0x002384A1
		protected override sbyte MainAttributeType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600385D RID: 14429 RVA: 0x0023A2A4 File Offset: 0x002384A4
		private int CurrAddValue
		{
			get
			{
				return Math.Min((int)(base.CurrMainAttribute / 2), this.MaxAddValue) * (base.IsDirect ? -1 : 1);
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600385E RID: 14430 RVA: 0x0023A2C6 File Offset: 0x002384C6
		private CombatCharacter AffectChar
		{
			get
			{
				return base.IsDirect ? base.CombatChar : base.EnemyChar;
			}
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x0023A2DE File Offset: 0x002384DE
		public DaWeiYongShenTong()
		{
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x0023A2E8 File Offset: 0x002384E8
		public DaWeiYongShenTong(CombatSkillKey skillKey) : base(skillKey, 6004)
		{
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x0023A2F8 File Offset: 0x002384F8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x0023A315 File Offset: 0x00238515
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			base.OnDisable(context);
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x0023A334 File Offset: 0x00238534
		private void OnCombatBegin(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.AppendAffectedData(context, 191, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.AppendAffectedAllEnemyData(context, 191, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x0023A36C File Offset: 0x0023856C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != this.AffectChar.GetId() || dataKey.FieldId != 191 || !this.IsAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = !base.IsDirect && !base.IsCurrent;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					EDamageType damageType = (EDamageType)dataKey.CustomParam1;
					bool flag3 = damageType != EDamageType.Direct;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						bool flag4 = Math.Abs(this.CurrAddValue) > 0;
						if (flag4)
						{
							base.ShowSpecialEffectTips(0);
						}
						result = this.CurrAddValue;
					}
				}
			}
			return result;
		}

		// Token: 0x0400107D RID: 4221
		private const int NeedMarkCount = 4;

		// Token: 0x0400107E RID: 4222
		private const int UnitAffect = 2;
	}
}
