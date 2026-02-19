using System;
using System.Collections.Generic;
using Redzen.Numerics.Distributions.Double;
using Redzen.Random;

namespace GameData.Utilities;

public static class RedzenHelper
{
	public static bool CheckPercentProb(this IRandomSource randomSource, int percentProb)
	{
		if (percentProb > 0)
		{
			return randomSource.Next(100) < percentProb;
		}
		return false;
	}

	public static bool CheckProb(this IRandomSource randomSource, int threshold, int max)
	{
		if (max > 0)
		{
			return randomSource.Next(max) < threshold;
		}
		return false;
	}

	public static T GetRandomElement<T>(this IRandomSource randomSource, T[] array)
	{
		int num = randomSource.Next(array.Length);
		return array[num];
	}

	public static T GetRandomElement<T>(this IRandomSource randomSource, List<T> list)
	{
		int index = randomSource.Next(list.Count);
		return list[index];
	}

	public static int NormalDistribute(IRandomSource randomSource, float mean, float stdDev)
	{
		return (int)Math.Round(ZigguratGaussian.Sample(randomSource, (double)mean, (double)stdDev));
	}

	public static int NormalDistribute(IRandomSource randomSource, float mean, float stdDev, int min, int max)
	{
		int num = (int)Math.Round(ZigguratGaussian.Sample(randomSource, (double)mean, (double)stdDev));
		if (num < min)
		{
			return min;
		}
		if (num > max)
		{
			return max;
		}
		return num;
	}

	public static float NormalDistribute(IRandomSource randomSource, float mean, float stdDev, float min, float max)
	{
		float num = (float)ZigguratGaussian.Sample(randomSource, (double)mean, (double)stdDev);
		if (num < min)
		{
			return min;
		}
		if (num > max)
		{
			return max;
		}
		return num;
	}

	public static int SkewDistribute(IRandomSource randomSource, float mean, float stdDev, float skewness, int min = int.MinValue, int max = int.MaxValue)
	{
		Tester.Assert((double)Math.Abs(skewness) > 1.0);
		double num = ZigguratGaussian.Sample(randomSource);
		if (skewness > 0f)
		{
			if (num > 0.0)
			{
				num *= (double)skewness;
			}
		}
		else if (num < 0.0)
		{
			num *= (double)(0f - skewness);
		}
		int num2 = (int)Math.Round((double)mean + num * (double)stdDev);
		if (num2 < min)
		{
			return min;
		}
		if (num2 > max)
		{
			return max;
		}
		return num2;
	}

	public static int GetNormalDistributedRangedValue(IRandomSource randomSource, int min, int max)
	{
		float num = (float)(max - min) / 2f;
		float mean = (float)min + num;
		float stdDev = num / 2.326348f;
		return NormalDistribute(randomSource, mean, stdDev, min, max);
	}
}
