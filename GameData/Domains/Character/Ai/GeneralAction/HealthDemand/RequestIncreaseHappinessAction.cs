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
	// Token: 0x0200089A RID: 2202
	public class RequestIncreaseHappinessAction : IGeneralAction
	{
		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06007843 RID: 30787 RVA: 0x00463340 File Offset: 0x00461540
		public sbyte ActionEnergyType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06007844 RID: 30788 RVA: 0x00463344 File Offset: 0x00461544
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.GetHappiness() < HappinessType.Ranges[3].Item1 && targetChar.GetInventory().Items.ContainsKey(this.ItemUsed) && selfChar.GetEatingItems().GetAvailableEatingSlotsCount(selfChar.GetCurrMaxEatingSlotsCount()) > 0;
		}

		// Token: 0x06007845 RID: 30789 RVA: 0x004633A0 File Offset: 0x004615A0
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddRequestTeaWine(selfCharId, location, targetChar.GetId(), (ulong)this.ItemUsed);
			CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
		}

		// Token: 0x06007846 RID: 30790 RVA: 0x004633F0 File Offset: 0x004615F0
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
				bool flag = this.ItemUsed.ItemType != 9;
				if (flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid item type ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(this.ItemUsed);
					defaultInterpolatedStringHandler.AppendLiteral(" to restore neili for ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(selfChar.GetId());
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemBase baseItem = DomainManager.Item.GetBaseItem(this.ItemUsed);
				targetChar.RemoveInventoryItem(context, this.ItemUsed, 1, false, false);
				selfChar.AddEatingItem(context, this.ItemUsed, null);
				selfChar.ChangeHappiness(context, (int)baseItem.GetHappinessChange());
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseItem.GetFavorabilityChange() * 2);
				lifeRecordCollection.AddRequestTeaWineSucceed(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestTeaWine(targetCharId, selfCharId, (ulong)this.ItemUsed);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				lifeRecordCollection.AddRequestTeaWineFail(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestTeaWine(targetCharId, selfCharId, (ulong)this.ItemUsed);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x04002160 RID: 8544
		public ItemKey ItemUsed;

		// Token: 0x04002161 RID: 8545
		public bool AgreeToRequest;
	}
}
