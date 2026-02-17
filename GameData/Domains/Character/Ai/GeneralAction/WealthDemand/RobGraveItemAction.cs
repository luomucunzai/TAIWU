using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x02000874 RID: 2164
	public class RobGraveItemAction : IGeneralAction
	{
		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06007785 RID: 30597 RVA: 0x0045DE82 File Offset: 0x0045C082
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06007786 RID: 30598 RVA: 0x0045DE88 File Offset: 0x0045C088
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			Grave grave;
			return DomainManager.Character.TryGetElement_Graves(this.TargetGraveId, out grave) && grave.GetInventory().Items.ContainsKey(this.TargetItem);
		}

		// Token: 0x06007787 RID: 30599 RVA: 0x0045DEC7 File Offset: 0x0045C0C7
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("cannot be digging the current Taiwu's grave when he or she is still alive.");
		}

		// Token: 0x06007788 RID: 30600 RVA: 0x0045DED4 File Offset: 0x0045C0D4
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = selfCharId != taiwuCharId;
			if (flag)
			{
				selfChar.ChangeCurrMainAttribute(context, 3, (int)(-(int)GlobalConfig.Instance.HarmfulActionCost));
			}
			bool flag2 = DomainManager.Character.IsTaiwuPeople(this.TargetGraveId);
			if (flag2)
			{
				monthlyNotificationCollection.AddDigItem(selfCharId, location, this.TargetGraveId, this.TargetItem.ItemType, this.TargetItem.TemplateId);
			}
			bool succeed = this.Succeed;
			if (succeed)
			{
				Grave grave = DomainManager.Character.GetElement_Graves(this.TargetGraveId);
				grave.RemoveInventoryItem(context, this.TargetItem, this.Amount, false);
				selfChar.AddInventoryItem(context, this.TargetItem, this.Amount, false);
				lifeRecordCollection.AddRobItemFromGraveSucceed(selfCharId, currDate, this.TargetGraveId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddRobGraveItem(selfCharId, this.TargetGraveId, (ulong)this.TargetItem);
				DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				lifeRecordCollection.AddRobItemFromGraveFail(selfCharId, currDate, this.TargetGraveId, location, this.TargetItem.ItemType, this.TargetItem.TemplateId);
			}
		}

		// Token: 0x040020F0 RID: 8432
		public ItemKey TargetItem;

		// Token: 0x040020F1 RID: 8433
		public int Amount;

		// Token: 0x040020F2 RID: 8434
		public bool Succeed;

		// Token: 0x040020F3 RID: 8435
		public int TargetGraveId;
	}
}
