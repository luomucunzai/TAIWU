using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x02000870 RID: 2160
	public class RequestItemAction : IGeneralAction
	{
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06007771 RID: 30577 RVA: 0x0045CE47 File Offset: 0x0045B047
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06007772 RID: 30578 RVA: 0x0045CE4C File Offset: 0x0045B04C
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return targetChar.GetInventory().Items.ContainsKey(this.TargetItem);
		}

		// Token: 0x06007773 RID: 30579 RVA: 0x0045CE74 File Offset: 0x0045B074
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = targetChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddRequestItem(selfCharId, location, targetCharId, (ulong)this.TargetItem, this.Amount);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x06007774 RID: 30580 RVA: 0x0045CEC4 File Offset: 0x0045B0C4
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				bool flag = this.PoisonsToAdd != null;
				if (flag)
				{
					ValueTuple<ItemKey, bool> valueTuple = targetChar.AttachPoisonsToInventoryItem(context, this.TargetItem, this.PoisonsToAdd);
					this.TargetItem = valueTuple.Item1;
				}
				DomainManager.Character.TransferInventoryItem(context, targetChar, selfChar, this.TargetItem, this.Amount);
				ItemBase itemBase = DomainManager.Item.GetBaseItem(this.TargetItem);
				int favorabilityChange = itemBase.GetFavorabilityChange() * 2;
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, favorabilityChange);
				selfChar.ChangeHappiness(context, (int)itemBase.GetHappinessChange());
				lifeRecordCollection.AddRequestItemSucceed(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestItem(selfCharId, targetCharId, (ulong)this.TargetItem);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				selfChar.ChangeHappiness(context, -3);
				lifeRecordCollection.AddRequestItemFail(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestItem(selfCharId, targetCharId, (ulong)this.TargetItem);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x040020E0 RID: 8416
		public ItemKey TargetItem;

		// Token: 0x040020E1 RID: 8417
		public int Amount;

		// Token: 0x040020E2 RID: 8418
		public bool AgreeToRequest;

		// Token: 0x040020E3 RID: 8419
		public ItemKey[] PoisonsToAdd;
	}
}
