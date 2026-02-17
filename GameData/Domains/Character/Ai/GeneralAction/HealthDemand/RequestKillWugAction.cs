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
	// Token: 0x020008A0 RID: 2208
	public class RequestKillWugAction : IGeneralAction
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06007861 RID: 30817 RVA: 0x00464309 File Offset: 0x00462509
		public sbyte ActionEnergyType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06007862 RID: 30818 RVA: 0x0046430C File Offset: 0x0046250C
		public unsafe bool CheckValid(Character selfChar, Character targetChar)
		{
			return (*selfChar.GetEatingItems()).IndexOfWug(this.WugType, false) >= 0 && targetChar.GetInventory().Items.ContainsKey(this.ItemUsed) && selfChar.GetEatingItems().GetAvailableEatingSlotsCount(selfChar.GetCurrMaxEatingSlotsCount()) > 0;
		}

		// Token: 0x06007863 RID: 30819 RVA: 0x00464368 File Offset: 0x00462568
		public unsafe void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			EatingItems eatingItems = *selfChar.GetEatingItems();
			ulong wugItemKey = *(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)eatingItems.IndexOfWug(this.WugType, false) * 8);
			monthlyEventCollection.AddRequestKillWug(selfCharId, location, targetChar.GetId(), (ulong)this.ItemUsed, wugItemKey);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x06007864 RID: 30820 RVA: 0x004643E0 File Offset: 0x004625E0
		public unsafe void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			EatingItems eatingItems = *selfChar.GetEatingItems();
			ItemKey wugItemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)eatingItems.IndexOfWug(this.WugType, false) * 8));
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				bool flag = this.ItemUsed.ItemType != 8;
				if (flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid item type ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(this.ItemUsed);
					defaultInterpolatedStringHandler.AppendLiteral(" to kill wug ");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.WugType);
					defaultInterpolatedStringHandler.AppendLiteral(" for ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(selfChar.GetId());
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemBase baseItem = DomainManager.Item.GetBaseItem(this.ItemUsed);
				targetChar.RemoveInventoryItem(context, this.ItemUsed, 1, false, false);
				selfChar.AddEatingItem(context, this.ItemUsed, null);
				selfChar.ChangeHappiness(context, (int)baseItem.GetHappinessChange());
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseItem.GetFavorabilityChange() * 5);
				lifeRecordCollection.AddRequestKillWugSucceed(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId, wugItemKey.ItemType, wugItemKey.TemplateId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestKillWug(targetCharId, selfCharId);
				DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -6000);
				lifeRecordCollection.AddRequestKillWugFail(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId, wugItemKey.ItemType, wugItemKey.TemplateId);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestKillWug(targetCharId, selfCharId);
				DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x0400216E RID: 8558
		public ItemKey ItemUsed;

		// Token: 0x0400216F RID: 8559
		public sbyte WugType;

		// Token: 0x04002170 RID: 8560
		public bool AgreeToRequest;
	}
}
