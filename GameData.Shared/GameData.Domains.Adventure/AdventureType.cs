using System;

namespace GameData.Domains.Adventure;

[Obsolete("Use Config.AdventureType instead.")]
public static class AdventureType
{
	public const sbyte None = 0;

	public const sbyte EnemyNest = 1;

	public const sbyte ContestForBride = 2;

	public const sbyte ResourceFood = 3;

	public const sbyte ResourceWood = 4;

	public const sbyte ResourceMetal = 5;

	public const sbyte ResourceJade = 6;

	public const sbyte ResourceFabric = 7;

	public const sbyte ResourceHerb = 8;

	public const sbyte MainStoryLine = 9;

	public const sbyte RegionalMainStoryLine = 10;

	public const sbyte SwordTomb = 11;

	public const int Count = 12;

	public const sbyte MaterialResourceAdventureBegin = 3;

	public const sbyte MaterialResourceAdventureEnd = 8;

	[Obsolete("Use AdventureTypeItem.IsTrivial instead.")]
	public static bool IsTrivial(short type)
	{
		if (type != 11 && type != 10)
		{
			return type != 9;
		}
		return false;
	}
}
