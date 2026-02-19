using GameData.Domains.Map;

namespace GameData.Domains.World;

public sealed class TaskConditionTaiwuMapArg : TaskConditionCheckArgument
{
	public Location Location;

	public ETaskConditionType Type;
}
