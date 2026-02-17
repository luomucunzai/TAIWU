using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom
{
	// Token: 0x02000890 RID: 2192
	public class SocialStatusRepairAction : IGeneralAction
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06007811 RID: 30737 RVA: 0x00462028 File Offset: 0x00460228
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06007812 RID: 30738 RVA: 0x0046202C File Offset: 0x0046022C
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.GetResource(this.ResourceType) >= this.Amount && selfChar.GetInventory().Items.ContainsKey(this.ToolUsed) && DomainManager.Item.GetBaseItem(this.ToolUsed).GetCurrDurability() > 0 && targetChar.GetInventory().Items.ContainsKey(this.RepairedItem);
		}

		// Token: 0x06007813 RID: 30739 RVA: 0x0046209C File Offset: 0x0046029C
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddAdviseRepairItem(selfCharId, location, targetChar.GetId(), (ulong)this.RepairedItem, (ulong)this.ToolUsed, this.ResourceType, this.Amount);
			CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
		}

		// Token: 0x06007814 RID: 30740 RVA: 0x00462100 File Offset: 0x00460300
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			selfChar.ChangeResource(context, this.ResourceType, this.Amount);
			ItemBase item = DomainManager.Item.GetBaseItem(this.RepairedItem);
			item.SetCurrDurability(item.GetMaxDurability(), context);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, 3000);
			lifeRecordCollection.AddRepairItemSucceed(selfCharId, currDate, targetCharId, location);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int secretInfoOffset = secretInformationCollection.AddRepairItem(selfCharId, targetCharId, (ulong)this.RepairedItem);
			int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
		}

		// Token: 0x04002143 RID: 8515
		public sbyte ResourceType;

		// Token: 0x04002144 RID: 8516
		public int Amount;

		// Token: 0x04002145 RID: 8517
		public ItemKey ToolUsed;

		// Token: 0x04002146 RID: 8518
		public ItemKey RepairedItem;
	}
}
