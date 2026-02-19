using System;

namespace GameData.Domains.Building;

public static class BuildingOperationType
{
	public const sbyte Invalid = -1;

	public const sbyte Build = 0;

	public const sbyte Remove = 1;

	[Obsolete]
	public const sbyte Upgrade = 2;

	[Obsolete]
	public const sbyte Collect = 3;
}
