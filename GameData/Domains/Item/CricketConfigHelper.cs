using System;
using System.Runtime.CompilerServices;
using Config;

namespace GameData.Domains.Item
{
	// Token: 0x02000667 RID: 1639
	public static class CricketConfigHelper
	{
		// Token: 0x06004F8B RID: 20363 RVA: 0x002B5057 File Offset: 0x002B3257
		public static bool IsCombine([TupleElementNames(new string[]
		{
			"colorId",
			"partId"
		})] this ValueTuple<short, short> tuple)
		{
			return tuple.Item2 > 0;
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x002B5064 File Offset: 0x002B3264
		public static int CalcCatchLucky([TupleElementNames(new string[]
		{
			"colorId",
			"partId"
		})] this ValueTuple<short, short> tuple)
		{
			short value = CricketParts.Instance[tuple.Item1].CatchInfluence;
			bool flag = tuple.IsCombine();
			if (flag)
			{
				value += CricketParts.Instance[tuple.Item2].CatchInfluence;
			}
			return (int)value;
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x002B50B0 File Offset: 0x002B32B0
		public static sbyte CalcLevel([TupleElementNames(new string[]
		{
			"colorId",
			"partId"
		})] this ValueTuple<short, short> tuple)
		{
			sbyte level = CricketParts.Instance[tuple.Item1].Level;
			bool flag = tuple.IsCombine();
			if (flag)
			{
				level = Math.Max(level, CricketParts.Instance[tuple.Item2].Level);
			}
			return level;
		}
	}
}
