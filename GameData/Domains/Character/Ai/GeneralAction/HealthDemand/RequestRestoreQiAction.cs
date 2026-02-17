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
	// Token: 0x0200089E RID: 2206
	public class RequestRestoreQiAction : IGeneralAction
	{
		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06007857 RID: 30807 RVA: 0x00463E49 File Offset: 0x00462049
		public sbyte ActionEnergyType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06007858 RID: 30808 RVA: 0x00463E4C File Offset: 0x0046204C
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.GetDisorderOfQi() > 0 && targetChar.GetInventory().Items.ContainsKey(this.ItemUsed) && selfChar.GetEatingItems().GetAvailableEatingSlotsCount(selfChar.GetCurrMaxEatingSlotsCount()) > 0;
		}

		// Token: 0x06007859 RID: 30809 RVA: 0x00463E98 File Offset: 0x00462098
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddRequestHealDisorderOfQi(selfCharId, location, targetChar.GetId(), (ulong)this.ItemUsed);
			CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
		}

		// Token: 0x0600785A RID: 30810 RVA: 0x00463EE8 File Offset: 0x004620E8
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
				bool flag = this.ItemUsed.ItemType != 8;
				if (flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid item type ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(this.ItemUsed);
					defaultInterpolatedStringHandler.AppendLiteral(" to restore disorder of Qi for ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(selfChar.GetId());
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemBase baseItem = DomainManager.Item.GetBaseItem(this.ItemUsed);
				targetChar.RemoveInventoryItem(context, this.ItemUsed, 1, false, false);
				selfChar.AddEatingItem(context, this.ItemUsed, null);
				selfChar.ChangeHappiness(context, (int)baseItem.GetHappinessChange());
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseItem.GetFavorabilityChange() * 5);
				lifeRecordCollection.AddRequestHealDisorderOfQiSucceed(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestRestoreDisorderOfQi(targetCharId, selfCharId);
				DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -6000);
				lifeRecordCollection.AddRequestHealDisorderOfQiFail(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestRestoreDisorderOfQi(targetCharId, selfCharId);
				DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x0400216A RID: 8554
		public ItemKey ItemUsed;

		// Token: 0x0400216B RID: 8555
		public bool AgreeToRequest;
	}
}
