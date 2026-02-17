using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.HealthDemand
{
	// Token: 0x02000899 RID: 2201
	public class RequestRecoverMainAttributeAction : IGeneralAction
	{
		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x0600783E RID: 30782 RVA: 0x00463077 File Offset: 0x00461277
		public sbyte ActionEnergyType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600783F RID: 30783 RVA: 0x0046307C File Offset: 0x0046127C
		public unsafe bool CheckValid(Character selfChar, Character targetChar)
		{
			MainAttributes currMainAttributes = selfChar.GetCurrMainAttributes();
			MainAttributes maxMainAttributes = selfChar.GetMaxMainAttributes();
			return *(ref currMainAttributes.Items.FixedElementField + (IntPtr)this.MainAttributeType * 2) < *(ref maxMainAttributes.Items.FixedElementField + (IntPtr)this.MainAttributeType * 2) && targetChar.GetInventory().Items.ContainsKey(this.ItemUsed) && selfChar.GetEatingItems().GetAvailableEatingSlotsCount(selfChar.GetCurrMaxEatingSlotsCount()) > 0;
		}

		// Token: 0x06007840 RID: 30784 RVA: 0x004630FC File Offset: 0x004612FC
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddRequestFood(selfCharId, location, targetChar.GetId(), (ulong)this.ItemUsed, (int)this.MainAttributeType);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x06007841 RID: 30785 RVA: 0x0046314C File Offset: 0x0046134C
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				bool flag = this.ItemUsed.ItemType != 7;
				if (flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid item type ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(this.ItemUsed);
					defaultInterpolatedStringHandler.AppendLiteral(" to restore main attribute ");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.MainAttributeType);
					defaultInterpolatedStringHandler.AppendLiteral(" for ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(selfChar.GetId());
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemBase baseItem = DomainManager.Item.GetBaseItem(this.ItemUsed);
				targetChar.RemoveInventoryItem(context, this.ItemUsed, 1, false, false);
				selfChar.AddEatingItem(context, this.ItemUsed, null);
				selfChar.ChangeHappiness(context, (int)baseItem.GetHappinessChange());
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseItem.GetFavorabilityChange() * 2);
				lifeRecordCollection.AddRequestFoodSucceed(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestFood(targetCharId, selfCharId, (ulong)this.ItemUsed);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				lifeRecordCollection.AddRequestFoodFail(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestFood(targetCharId, selfCharId, (ulong)this.ItemUsed);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x0400215D RID: 8541
		public ItemKey ItemUsed;

		// Token: 0x0400215E RID: 8542
		public sbyte MainAttributeType;

		// Token: 0x0400215F RID: 8543
		public bool AgreeToRequest;
	}
}
