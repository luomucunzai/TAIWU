using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.HealthDemand
{
	// Token: 0x0200089B RID: 2203
	public class RequestHealInjuryAction : IGeneralAction
	{
		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06007848 RID: 30792 RVA: 0x004635CA File Offset: 0x004617CA
		public sbyte ActionEnergyType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06007849 RID: 30793 RVA: 0x004635D0 File Offset: 0x004617D0
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			bool flag = !selfChar.GetInjuries().HasAnyInjury(this.IsInnerInjury);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !targetChar.GetInventory().Items.ContainsKey(this.ItemUsed);
				if (flag2)
				{
					result = false;
				}
				else
				{
					MedicineItem medicineCfg = Config.Medicine.Instance[this.ItemUsed.TemplateId];
					bool flag3 = medicineCfg.Duration == 0;
					if (flag3)
					{
						result = (medicineCfg.RequiredMainAttributeType < 0 || selfChar.GetCurrMainAttribute(medicineCfg.RequiredMainAttributeType) >= (short)medicineCfg.RequiredMainAttributeValue);
					}
					else
					{
						result = (selfChar.GetEatingItems().GetAvailableEatingSlotsCount(selfChar.GetCurrMaxEatingSlotsCount()) > 0);
					}
				}
			}
			return result;
		}

		// Token: 0x0600784A RID: 30794 RVA: 0x00463688 File Offset: 0x00461888
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = targetChar.GetLocation();
			bool isInnerInjury = this.IsInnerInjury;
			if (isInnerInjury)
			{
				monthlyEventCollection.AddRequestHealInnerInjuryByItem(selfCharId, location, targetCharId, (ulong)this.ItemUsed, -1);
			}
			else
			{
				monthlyEventCollection.AddRequestHealOuterInjuryByItem(selfCharId, location, targetCharId, (ulong)this.ItemUsed, -1);
			}
			CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
		}

		// Token: 0x0600784B RID: 30795 RVA: 0x004636FC File Offset: 0x004618FC
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				bool flag = this.ItemUsed.ItemType != 8;
				if (flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid item type ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(this.ItemUsed);
					defaultInterpolatedStringHandler.AppendLiteral(" to heal injury for ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(selfChar.GetId());
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemBase baseItem = DomainManager.Item.GetBaseItem(this.ItemUsed);
				targetChar.RemoveInventoryItem(context, this.ItemUsed, 1, false, false);
				MedicineItem medicineCfg = Config.Medicine.Instance[this.ItemUsed.TemplateId];
				bool flag2 = medicineCfg.Duration == 0;
				if (flag2)
				{
					selfChar.ApplyTopicalMedicine(context, this.ItemUsed);
					DomainManager.Item.RemoveItem(context, this.ItemUsed);
				}
				else
				{
					selfChar.AddEatingItem(context, this.ItemUsed, null);
				}
				selfChar.ChangeHappiness(context, (int)baseItem.GetHappinessChange());
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseItem.GetFavorabilityChange() * 5);
				bool isInnerInjury = this.IsInnerInjury;
				if (isInnerInjury)
				{
					lifeRecordCollection.AddRequestHealInnerInjurySucceed(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				}
				else
				{
					lifeRecordCollection.AddRequestHealOuterInjurySucceed(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				}
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestHealInjury(targetCharId, selfCharId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -6000);
				bool isInnerInjury2 = this.IsInnerInjury;
				if (isInnerInjury2)
				{
					lifeRecordCollection.AddRequestHealInnerInjuryFail(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				}
				else
				{
					lifeRecordCollection.AddRequestHealOuterInjuryFail(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				}
				int secretInfoOffset2 = secretInformationCollection.AddRefuseRequestHealInjury(targetCharId, selfCharId);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x04002162 RID: 8546
		public ItemKey ItemUsed;

		// Token: 0x04002163 RID: 8547
		public bool IsInnerInjury;

		// Token: 0x04002164 RID: 8548
		public bool AgreeToRequest;
	}
}
