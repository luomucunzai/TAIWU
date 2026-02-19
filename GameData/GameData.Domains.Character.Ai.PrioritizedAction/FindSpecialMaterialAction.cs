using GameData.Common;
using GameData.Domains.Adventure;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class FindSpecialMaterialAction : BasePrioritizedAction
{
	public override short ActionType => 7;

	public override bool CheckValid(Character selfChar)
	{
		if (!base.CheckValid(selfChar))
		{
			return false;
		}
		Location realTargetLocation = Target.GetRealTargetLocation();
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(realTargetLocation.AreaId);
		AdventureSiteData value;
		return adventuresInArea.AdventureSites.TryGetValue(realTargetLocation.BlockId, out value) && value.IsMaterialResource();
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		Location realTargetLocation = Target.GetRealTargetLocation();
		lifeRecordCollection.AddDecideToFindSpecialMaterial(id, currDate, location, realTargetLocation);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddFinishFIndingSpecialMaterial(id, currDate, location);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		return false;
	}
}
