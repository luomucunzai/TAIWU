namespace GameData.DLC.Shared;

public static class LoveEventId
{
	public static sbyte[] UnmarriedFavorite4DateEvents = new sbyte[5] { 1, 2, 3, 4, 5 };

	public static sbyte[] UnmarriedFavorite5DateEvents = new sbyte[5] { 6, 7, 8, 9, 10 };

	public static sbyte[] UnmarriedFavorite6DateEvents = new sbyte[5] { 11, 12, 13, 14, 15 };

	public static sbyte[] MarriedFavorite4DateEvents = new sbyte[2] { 17, 18 };

	public static sbyte[] MarriedFavorite5DateEvents = new sbyte[1] { 19 };

	public static sbyte[] MarriedFavorite6DateEvents = new sbyte[3] { 22, 23, 24 };

	public static (sbyte, sbyte, sbyte)[] MarriedNeedConditionDateEvents = new(sbyte, sbyte, sbyte)[3]
	{
		(4, 1, 16),
		(5, 3, 20),
		(5, 10, 21)
	};

	public const sbyte ChatAndSendGift = 0;

	public const sbyte UnmarriedFavorite4One = 1;

	public const sbyte UnmarriedFavorite4Two = 2;

	public const sbyte UnmarriedFavorite4Three = 3;

	public const sbyte UnmarriedFavorite4Four = 4;

	public const sbyte UnmarriedFavorite4Five = 5;

	public const sbyte UnmarriedFavorite5One = 6;

	public const sbyte UnmarriedFavorite5Two = 7;

	public const sbyte UnmarriedFavorite5Three = 8;

	public const sbyte UnmarriedFavorite5Four = 9;

	public const sbyte UnmarriedFavorite5Five = 10;

	public const sbyte UnmarriedFavorite6One = 11;

	public const sbyte UnmarriedFavorite6Two = 12;

	public const sbyte UnmarriedFavorite6Three = 13;

	public const sbyte UnmarriedFavorite6Four = 14;

	public const sbyte UnmarriedFavorite6Five = 15;

	public const sbyte MarriedFavorite4One = 16;

	public const sbyte MarriedFavorite4Two = 17;

	public const sbyte MarriedFavorite4Three = 18;

	public const sbyte MarriedFavorite5One = 19;

	public const sbyte MarriedFavorite5Two = 20;

	public const sbyte MarriedFavorite5Three = 21;

	public const sbyte MarriedFavorite6One = 22;

	public const sbyte MarriedFavorite6Two = 23;

	public const sbyte MarriedFavorite6Three = 24;

	public const sbyte FirstDateEvent = 25;
}
