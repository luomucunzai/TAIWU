using System;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x0200085B RID: 2139
	[SerializableGameData(NotForDisplayModule = true)]
	public class FindTreasureAction : BasePrioritizedAction
	{
		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060076EA RID: 30442 RVA: 0x00459758 File Offset: 0x00457958
		public override short ActionType
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x060076EB RID: 30443 RVA: 0x0045975C File Offset: 0x0045795C
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			Location targetLocation = this.Target.GetRealTargetLocation();
			lifeRecordCollection.AddDecideToFindLostItem(selfCharId, currDate, currLocation, targetLocation);
		}

		// Token: 0x060076EC RID: 30444 RVA: 0x004597A8 File Offset: 0x004579A8
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddFinishFIndingLostItem(selfCharId, currDate, currLocation);
		}

		// Token: 0x060076ED RID: 30445 RVA: 0x004597E4 File Offset: 0x004579E4
		public unsafe override bool Execute(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			sbyte selfGrade = selfChar.GetOrganizationInfo().Grade;
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			bool flag = !currLocation.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MapBlockData currBlockData = DomainManager.Map.GetBlock(currLocation);
				sbyte luck = *(ref selfChar.GetPersonalities().Items.FixedElementField + 5);
				int chance = currBlockData.CalcFindTreasureChance(luck);
				bool flag2 = context.Random.CheckPercentProb(chance);
				if (flag2)
				{
					ItemKeyAndDate itemAndDates = currBlockData.Items.Keys.GetRandom(context.Random);
					ItemKey itemKey = itemAndDates.ItemKey;
					int amount = currBlockData.Items[itemAndDates];
					DomainManager.Map.RemoveBlockItem(context, currBlockData, itemAndDates);
					selfChar.AddInventoryItem(context, itemKey, amount, false);
					lifeRecordCollection.AddFindLostItemSucceed(selfCharId, currDate, currLocation, itemKey.ItemType, itemKey.TemplateId);
					result = (ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId) >= selfGrade);
				}
				else
				{
					lifeRecordCollection.AddFindLostItemFail(selfCharId, currDate, currLocation);
					result = false;
				}
			}
			return result;
		}
	}
}
