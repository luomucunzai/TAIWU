using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom
{
	// Token: 0x02000892 RID: 2194
	public class SocialStatusTeaWineAction : IGeneralAction
	{
		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x0600781B RID: 30747 RVA: 0x004623A3 File Offset: 0x004605A3
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600781C RID: 30748 RVA: 0x004623A8 File Offset: 0x004605A8
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			Inventory inventory = selfChar.GetInventory();
			return inventory.Items.ContainsKey(this.SelfTeaWineItem) && inventory.Items.ContainsKey(this.TargetTeaWineItem);
		}

		// Token: 0x0600781D RID: 30749 RVA: 0x004623E8 File Offset: 0x004605E8
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddAdviseTeaWine(selfCharId, location, targetChar.GetId(), (ulong)this.SelfTeaWineItem, (ulong)this.TargetTeaWineItem);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x0600781E RID: 30750 RVA: 0x0046243C File Offset: 0x0046063C
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			sbyte behaviorType = selfChar.GetBehaviorType();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			bool succeed = this.Succeed;
			if (succeed)
			{
				selfChar.RemoveInventoryItem(context, this.SelfTeaWineItem, 1, false, false);
				bool flag = this.SelfTeaWineItem.Id != this.TargetTeaWineItem.Id;
				if (flag)
				{
					selfChar.RemoveInventoryItem(context, this.TargetTeaWineItem, 1, false, false);
				}
				selfChar.AddEatingItem(context, this.SelfTeaWineItem, null);
				targetChar.AddEatingItem(context, this.TargetTeaWineItem, null);
				short favorChange = AiHelper.GeneralActionConstants.GetBegSucceedFavorabilityChange(context.Random, behaviorType);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, (int)favorChange);
				lifeRecordCollection.AddInviteToDrinkSucceed(selfCharId, currDate, targetCharId, location, this.SelfTeaWineItem.ItemType, this.SelfTeaWineItem.TemplateId);
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestDrinking(targetCharId, selfCharId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				short favorChange2 = AiHelper.GeneralActionConstants.GetBegFailFavorabilityChange(context.Random, behaviorType);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, (int)favorChange2);
				lifeRecordCollection.AddInviteToDrinkFail(selfCharId, currDate, targetCharId, location, this.SelfTeaWineItem.ItemType, this.SelfTeaWineItem.TemplateId);
				int secretInfoOffset2 = secretInformationCollection.AddRefuseRequestDrinking(targetCharId, selfCharId);
				int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x0400214B RID: 8523
		public bool Succeed;

		// Token: 0x0400214C RID: 8524
		public ItemKey SelfTeaWineItem;

		// Token: 0x0400214D RID: 8525
		public ItemKey TargetTeaWineItem;
	}
}
