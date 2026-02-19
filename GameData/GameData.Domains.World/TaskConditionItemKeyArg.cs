using GameData.Domains.Item;

namespace GameData.Domains.World;

public sealed class TaskConditionItemKeyArg : TaskConditionCheckArgument
{
	public ItemKey ItemKey;

	public ETaskConditionType Type;
}
