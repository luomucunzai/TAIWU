using GameData.Domains.Building;

namespace GameData.Domains.World;

public sealed class TaskConditionBuildingBlockArg : TaskConditionCheckArgument
{
	public BuildingBlockKey BuildingBlockKey;

	public ETaskConditionType Type;
}
