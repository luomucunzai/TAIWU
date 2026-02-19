namespace GameData.Domains.World;

public sealed class TaskConditionRelationArg : TaskConditionCheckArgument
{
	public int CharacterId;

	public ushort RelationType;

	public ETaskConditionType Type;
}
