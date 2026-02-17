using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x02000878 RID: 2168
	public class RequestRepairItemAction : IGeneralAction
	{
		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06007799 RID: 30617 RVA: 0x0045E4EE File Offset: 0x0045C6EE
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600779A RID: 30618 RVA: 0x0045E4F4 File Offset: 0x0045C6F4
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.GetInventory().Items.ContainsKey(this.TargetItem) && targetChar.GetInventory().Items.ContainsKey(this.ToolUsed) && targetChar.GetResource(this.ResourceType) >= this.ResourceAmount;
		}

		// Token: 0x0600779B RID: 30619 RVA: 0x0045E550 File Offset: 0x0045C750
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = selfChar.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddRequestRepairItem(selfCharId, location, targetCharId, (ulong)this.TargetItem, (ulong)this.ToolUsed, this.ResourceAmount, this.ResourceType);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x0600779C RID: 30620 RVA: 0x0045E5B4 File Offset: 0x0045C7B4
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
				CraftTool craftTool = DomainManager.Item.GetElement_CraftTools(this.ToolUsed.Id);
				ItemBase itemToRepair = DomainManager.Item.GetBaseItem(this.TargetItem);
				ItemBase.OfflineRepairItem(craftTool, itemToRepair, itemToRepair.GetMaxDurability(), this.ToolDurabilityCost);
				itemToRepair.SetCurrDurability(itemToRepair.GetCurrDurability(), context);
				craftTool.SetCurrDurability(craftTool.GetCurrDurability(), context);
				targetChar.ChangeResource(context, this.ResourceType, -this.ResourceAmount);
				int favorabilityChange = itemToRepair.GetFavorabilityChange();
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, favorabilityChange);
				selfChar.ChangeHappiness(context, (int)DomainManager.Item.GetBaseItem(this.TargetItem).GetHappinessChange());
				lifeRecordCollection.AddRequestRepairItemSucceed(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestRepairItem(targetCharId, selfCharId, (ulong)this.TargetItem);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				selfChar.ChangeHappiness(context, -3);
				lifeRecordCollection.AddRequestRepairItemFail(selfCharId, currDate, targetCharId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestRepairItem(targetCharId, selfCharId, (ulong)this.TargetItem);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x040020FB RID: 8443
		public ItemKey TargetItem;

		// Token: 0x040020FC RID: 8444
		public bool AgreeToRequest;

		// Token: 0x040020FD RID: 8445
		public ItemKey ToolUsed;

		// Token: 0x040020FE RID: 8446
		public sbyte ResourceType;

		// Token: 0x040020FF RID: 8447
		public int ResourceAmount;

		// Token: 0x04002100 RID: 8448
		public short ToolDurabilityCost;
	}
}
