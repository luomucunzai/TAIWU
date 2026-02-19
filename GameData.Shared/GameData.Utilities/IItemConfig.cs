namespace GameData.Utilities;

public interface IItemConfig
{
	short TemplateId { get; }

	sbyte ItemType { get; }

	short ItemSubType { get; }

	string Name { get; }

	sbyte Grade { get; }

	short GroupId { get; }

	sbyte MaxUseDistance => 0;
}
