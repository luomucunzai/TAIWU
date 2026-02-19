using GameData.Domains.Map;

namespace GameData.Domains.World;

public sealed class TaskConditionFixedCharacterMapArg : TaskConditionCheckArgument
{
	public int CharacterId;

	public Location Location;

	public ETaskConditionType Type;
}
