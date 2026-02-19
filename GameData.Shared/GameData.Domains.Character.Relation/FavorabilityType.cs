using Redzen.Random;

namespace GameData.Domains.Character.Relation;

public static class FavorabilityType
{
	public const sbyte Unknown = sbyte.MinValue;

	public const sbyte Hateful6 = -6;

	public const sbyte Hateful5 = -5;

	public const sbyte Hateful4 = -4;

	public const sbyte Hateful3 = -3;

	public const sbyte Hateful2 = -2;

	public const sbyte Hateful1 = -1;

	public const sbyte Unfamiliar = 0;

	public const sbyte Favorite1 = 1;

	public const sbyte Favorite2 = 2;

	public const sbyte Favorite3 = 3;

	public const sbyte Favorite4 = 4;

	public const sbyte Favorite5 = 5;

	public const sbyte Favorite6 = 6;

	public const int Count = 13;

	public const short MinValue = -30000;

	public const short MaxValue = 30000;

	public const short DefaultValue = 0;

	public const short BaseInitialValue = 3000;

	public const short TaiwuVillagerBonus = 9000;

	public const short UnknownValue = short.MinValue;

	public static sbyte GetFavorabilityType(short favorability)
	{
		if (favorability == short.MinValue)
		{
			return 0;
		}
		if (favorability >= 6000)
		{
			int num = 1 + (favorability - 6000) / 4000;
			if (num > 6)
			{
				return 6;
			}
			return (sbyte)num;
		}
		if (favorability <= -6000)
		{
			int num2 = -1 + (favorability + 6000) / 4000;
			if (num2 < -6)
			{
				return -6;
			}
			return (sbyte)num2;
		}
		return 0;
	}

	public static (short, short) GetFavorabilityRange(short favorability)
	{
		if (favorability >= -6000 && favorability <= 6000)
		{
			return (-6000, 6000);
		}
		short num = -30000;
		short num2 = (short)(num + 4000);
		while (num <= 30000 && (favorability < num || favorability >= num2))
		{
			num += 4000;
			num2 = (short)(num + 4000);
			if (num2 >= 30000)
			{
				break;
			}
		}
		return (num, num2);
	}

	public static short GetRandomFavorability(IRandomSource random, sbyte favorabilityType)
	{
		if (favorabilityType != 0)
		{
			int num = ((favorabilityType > 0) ? favorabilityType : (-favorabilityType));
			int num2 = (num - 1) * 4000 + 6000;
			int num3 = ((num != 6) ? 4000 : 4001);
			int num4 = num2 + random.Next(num3);
			return (short)((favorabilityType > 0) ? num4 : (-num4));
		}
		return (short)random.Next(-5999, 6000);
	}

	public static sbyte ToIndex(sbyte favorabilityType)
	{
		return (sbyte)(favorabilityType - -6);
	}
}
