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
	// Token: 0x0200089C RID: 2204
	public class RequestDetoxPoisonAction : IGeneralAction
	{
		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x0600784D RID: 30797 RVA: 0x00463963 File Offset: 0x00461B63
		public sbyte ActionEnergyType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600784E RID: 30798 RVA: 0x00463968 File Offset: 0x00461B68
		public unsafe bool CheckValid(Character selfChar, Character targetChar)
		{
			PoisonInts poisoned = *selfChar.GetPoisoned();
			return *(ref poisoned.Items.FixedElementField + (IntPtr)this.PoisonType * 4) > 0 && targetChar.GetInventory().Items.ContainsKey(this.ItemUsed) && selfChar.GetEatingItems().GetAvailableEatingSlotsCount(selfChar.GetCurrMaxEatingSlotsCount()) > 0;
		}

		// Token: 0x0600784F RID: 30799 RVA: 0x004639D0 File Offset: 0x00461BD0
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddRequestHealPoisonByItem(selfCharId, location, targetChar.GetId(), (ulong)this.ItemUsed, this.PoisonType);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x06007850 RID: 30800 RVA: 0x00463A20 File Offset: 0x00461C20
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				bool flag = this.ItemUsed.ItemType != 8;
				if (flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid item type ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(this.ItemUsed);
					defaultInterpolatedStringHandler.AppendLiteral(" to detox poison for ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(selfChar.GetId());
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemBase baseItem = DomainManager.Item.GetBaseItem(this.ItemUsed);
				targetChar.RemoveInventoryItem(context, this.ItemUsed, 1, false, false);
				selfChar.AddEatingItem(context, this.ItemUsed, null);
				selfChar.ChangeHappiness(context, (int)baseItem.GetHappinessChange());
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseItem.GetFavorabilityChange() * 5);
				lifeRecordCollection.AddRequestDetoxPoisonSucceed(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId, this.PoisonType);
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestDetoxPoison(targetCharId, selfCharId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -6000);
				lifeRecordCollection.AddRequestDetoxPoisonFail(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId, this.PoisonType);
				int secretInfoOffset2 = secretInformationCollection.AddRefuseRequestDetoxPoison(targetCharId, selfCharId);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x04002165 RID: 8549
		public ItemKey ItemUsed;

		// Token: 0x04002166 RID: 8550
		public sbyte PoisonType;

		// Token: 0x04002167 RID: 8551
		public bool AgreeToRequest;
	}
}
