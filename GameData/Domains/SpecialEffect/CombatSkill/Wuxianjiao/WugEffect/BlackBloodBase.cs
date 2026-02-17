using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x0200034A RID: 842
	public class BlackBloodBase : WugEffectBase
	{
		// Token: 0x06003501 RID: 13569 RVA: 0x0022B1C3 File Offset: 0x002293C3
		protected BlackBloodBase()
		{
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x0022B1CD File Offset: 0x002293CD
		protected BlackBloodBase(int charId, int type, short wugTemplateId, short effectId) : base(charId, type, wugTemplateId, effectId)
		{
			this.CostWugCount = 6;
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x0022B1E4 File Offset: 0x002293E4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(base.IsGood ? 119 : 120, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(base.IsGood ? 122 : 123, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(261, EDataModifyType.AddPercent, -1);
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				base.CreateAffectedData(269, EDataModifyType.Custom, -1);
			}
			else
			{
				base.CreateAffectedData(127, EDataModifyType.Add, -1);
				base.CreateAffectedData(132, EDataModifyType.Add, -1);
			}
			bool canChangeToGrown = base.CanChangeToGrown;
			if (canChangeToGrown)
			{
				Events.RegisterHandler_EatingItem(new Events.OnEatingItem(this.OnEatingItem));
			}
			else
			{
				Events.RegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x0022B29C File Offset: 0x0022949C
		public override void OnDisable(DataContext context)
		{
			bool canChangeToGrown = base.CanChangeToGrown;
			if (canChangeToGrown)
			{
				Events.UnRegisterHandler_EatingItem(new Events.OnEatingItem(this.OnEatingItem));
			}
			else
			{
				Events.UnRegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
			base.OnDisable(context);
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x0022B2E2 File Offset: 0x002294E2
		protected override void AddAffectDataAndEvent(DataContext context)
		{
			Events.RegisterHandler_HealedInjury(new Events.OnHealedInjury(this.OnHealedInjury));
			Events.RegisterHandler_HealedPoison(new Events.OnHealedPoison(this.OnHealedPoison));
			Events.RegisterHandler_UsedMedicine(new Events.OnUsedMedicine(this.OnUsedMedicine));
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x0022B31B File Offset: 0x0022951B
		protected override void ClearAffectDataAndEvent(DataContext context)
		{
			Events.UnRegisterHandler_HealedInjury(new Events.OnHealedInjury(this.OnHealedInjury));
			Events.UnRegisterHandler_HealedPoison(new Events.OnHealedPoison(this.OnHealedPoison));
			Events.UnRegisterHandler_UsedMedicine(new Events.OnUsedMedicine(this.OnUsedMedicine));
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x0022B354 File Offset: 0x00229554
		private void OnEatingItem(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey)
		{
			bool flag = character.GetId() != base.CharacterId || itemKey.ItemType != 9 || !base.CanAffect;
			if (!flag)
			{
				bool flag2 = Config.TeaWine.Instance[itemKey.TemplateId].ItemSubType == 900;
				if (flag2)
				{
					this.ChangeToGrown(context);
				}
			}
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x0022B3B4 File Offset: 0x002295B4
		private void OnAdvanceMonthFinish(DataContext context)
		{
			bool flag = !base.IsElite || !base.CanAffect;
			if (!flag)
			{
				int injuryCount = this.CharObj.GetInjuries().GetSum();
				int changeValue = injuryCount * 200;
				bool flag2 = changeValue <= 0;
				if (!flag2)
				{
					this.CharObj.ChangeDisorderOfQi(context, changeValue);
					LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
					base.AddLifeRecord(new WugEffectBase.LifeRecordAddTemplate(lifeRecord.AddWugKingBlackBloodChangeDisorderOfQi));
				}
			}
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x0022B434 File Offset: 0x00229634
		private void OnHealedInjury(DataContext context, int doctorId, int patientId, bool isAlly, sbyte healMarkCount)
		{
			bool flag = patientId != base.CharacterId;
			if (!flag)
			{
				this.OnAffected(context);
			}
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x0022B45C File Offset: 0x0022965C
		private void OnHealedPoison(DataContext context, int doctorId, int patientId, bool isAlly, sbyte healMarkCount)
		{
			bool flag = patientId != base.CharacterId;
			if (!flag)
			{
				this.OnAffected(context);
			}
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x0022B484 File Offset: 0x00229684
		private void OnUsedMedicine(DataContext context, int charId, ItemKey itemKey)
		{
			bool flag = charId != base.CharacterId || itemKey.ItemType != 8;
			if (!flag)
			{
				EMedicineEffectType instantEffectType = Config.Medicine.Instance[itemKey.TemplateId].EffectType;
				bool flag2 = instantEffectType == EMedicineEffectType.Invalid || instantEffectType - EMedicineEffectType.DetoxWug <= 1;
				bool flag3 = flag2;
				if (!flag3)
				{
					this.OnAffected(context);
				}
			}
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x0022B4EC File Offset: 0x002296EC
		private void OnAffected(DataContext context)
		{
			bool flag = !base.CanAffect;
			if (!flag)
			{
				base.ShowEffectTips(context, 1);
				base.CostWugInCombat(context);
			}
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x0022B51C File Offset: 0x0022971C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				switch (fieldId)
				{
				case 119:
				case 122:
					num = 50;
					goto IL_B1;
				case 120:
				case 123:
					num = -50;
					goto IL_B1;
				case 121:
				case 124:
				case 125:
				case 126:
					goto IL_AD;
				case 127:
					break;
				default:
					if (fieldId != 132)
					{
						if (fieldId != 261)
						{
							goto IL_AD;
						}
						num = (base.IsGood ? 50 : -50);
						goto IL_B1;
					}
					break;
				}
				num = ((!base.IsElite) ? 0 : (base.IsGood ? -1 : 1));
				goto IL_B1;
				IL_AD:
				num = 0;
				IL_B1:
				if (!true)
				{
				}
				int returnValue = num;
				ushort fieldId2 = dataKey.FieldId;
				bool flag2 = fieldId2 == 127 || fieldId2 == 132;
				bool flag3 = flag2 && returnValue != 0;
				if (flag3)
				{
					base.ShowEffectTips(DomainManager.Combat.Context, 2);
				}
				result = returnValue;
			}
			return result;
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x0022B62C File Offset: 0x0022982C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				bool flag2 = fieldId != 269 && dataValue;
				if (!true)
				{
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x04000F98 RID: 3992
		private const sbyte ChangeHealEffect = 50;

		// Token: 0x04000F99 RID: 3993
		private const int DisorderOfQiPerInjury = 200;
	}
}
