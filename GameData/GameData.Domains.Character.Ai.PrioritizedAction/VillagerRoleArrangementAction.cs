using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class VillagerRoleArrangementAction : BasePrioritizedAction
{
	public override short ActionType => 21;

	public override bool CheckValid(Character selfChar)
	{
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(selfChar.GetId());
		if (villagerRole == null || villagerRole.ArrangementTemplateId < 0)
		{
			return false;
		}
		if (villagerRole.WorkData == null)
		{
			return false;
		}
		Location location = villagerRole.WorkData.Location;
		if (location.BlockId >= 0 && !DomainManager.Map.CheckLocationsHasSameRoot(location, Target.GetRealTargetLocation()))
		{
			return false;
		}
		if (location.BlockId < 0 && location.AreaId != Target.GetRealTargetLocation().AreaId)
		{
			return false;
		}
		return base.CheckValid(selfChar);
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(selfChar.GetId());
		Location location = villagerRole.WorkData.Location;
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddVillagerPrioritizedActions(id, currDate, location);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		Location location = selfChar.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddVillagerPrioritizedActionsStop(id, currDate, location);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(id);
		IVillagerRoleArrangementExecutor villagerRoleArrangementExecutor = (IVillagerRoleArrangementExecutor)villagerRole;
		villagerRoleArrangementExecutor.ExecuteArrangementAction(context, this);
		Location targetLocation = villagerRoleArrangementExecutor.SelectNextWorkLocation(context.Random, villagerRole.WorkData.Location);
		Target = new NpcTravelTarget(targetLocation, Target.RemainingMonth);
		return false;
	}
}
