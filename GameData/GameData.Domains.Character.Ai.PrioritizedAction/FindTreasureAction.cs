using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class FindTreasureAction : BasePrioritizedAction
{
	public override short ActionType => 6;

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		Location realTargetLocation = Target.GetRealTargetLocation();
		lifeRecordCollection.AddDecideToFindLostItem(id, currDate, location, realTargetLocation);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddFinishFIndingLostItem(id, currDate, location);
	}

	public unsafe override bool Execute(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		sbyte grade = selfChar.GetOrganizationInfo().Grade;
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		MapBlockData block = DomainManager.Map.GetBlock(location);
		Personalities personalities = selfChar.GetPersonalities();
		sbyte luck = personalities.Items[5];
		int percentProb = block.CalcFindTreasureChance(luck);
		if (context.Random.CheckPercentProb(percentProb))
		{
			ItemKeyAndDate random = block.Items.Keys.GetRandom(context.Random);
			ItemKey itemKey = random.ItemKey;
			int amount = block.Items[random];
			DomainManager.Map.RemoveBlockItem(context, block, random);
			selfChar.AddInventoryItem(context, itemKey, amount);
			lifeRecordCollection.AddFindLostItemSucceed(id, currDate, location, itemKey.ItemType, itemKey.TemplateId);
			return ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId) >= grade;
		}
		lifeRecordCollection.AddFindLostItemFail(id, currDate, location);
		return false;
	}
}
