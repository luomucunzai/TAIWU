using Config;

namespace GameData.Domains.Character.SortFilter;

public static class CharacterFilterSubType
{
	public const sbyte MapState = 0;

	public const sbyte RelationDisplayType = 1;

	public const sbyte VillagerRole = 2;

	public static readonly int[] SubTypeToFilterCount = new int[3]
	{
		16,
		10,
		Config.VillagerRole.Instance.Count + 1
	};
}
