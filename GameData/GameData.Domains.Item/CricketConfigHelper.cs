using System;
using Config;

namespace GameData.Domains.Item;

public static class CricketConfigHelper
{
	public static bool IsCombine(this (short colorId, short partId) tuple)
	{
		return tuple.partId > 0;
	}

	public static int CalcCatchLucky(this (short colorId, short partId) tuple)
	{
		short num = CricketParts.Instance[tuple.colorId].CatchInfluence;
		if (tuple.IsCombine())
		{
			num += CricketParts.Instance[tuple.partId].CatchInfluence;
		}
		return num;
	}

	public static sbyte CalcLevel(this (short colorId, short partId) tuple)
	{
		sbyte b = CricketParts.Instance[tuple.colorId].Level;
		if (tuple.IsCombine())
		{
			b = Math.Max(b, CricketParts.Instance[tuple.partId].Level);
		}
		return b;
	}
}
