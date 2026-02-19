using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Combat.Math;
using GameData.Utilities;

namespace GameData.Domains.Character;

public static class CombatHelper
{
	public const short MaxNeiliAllocation = 100;

	public const short MaxTotalNeiliAllocation = 400;

	private static readonly short[] NeiliCumulativeCosts = new short[100]
	{
		1, 3, 6, 10, 15, 21, 28, 36, 45, 56,
		68, 81, 95, 110, 127, 145, 164, 185, 207, 231,
		256, 282, 310, 339, 370, 402, 436, 471, 508, 547,
		587, 629, 672, 717, 764, 812, 862, 914, 968, 1024,
		1081, 1140, 1201, 1264, 1329, 1396, 1465, 1536, 1609, 1684,
		1761, 1840, 1921, 2004, 2089, 2176, 2265, 2356, 2449, 2545,
		2643, 2743, 2845, 2949, 3056, 3165, 3276, 3390, 3506, 3625,
		3746, 3869, 3995, 4123, 4254, 4387, 4523, 4661, 4802, 4946,
		5092, 5241, 5392, 5546, 5703, 5862, 6024, 6189, 6357, 6528,
		6701, 6877, 7056, 7238, 7423, 7611, 7802, 7996, 8193, 8393
	};

	[Obsolete]
	public static short GetMaxTotalNeiliAllocation(sbyte consummateLevel)
	{
		short num = (short)ConsummateLevel.Instance[consummateLevel].MaxNeiliAllocation;
		if (num > 400)
		{
			return 400;
		}
		return num;
	}

	public static short GetMaxTotalNeiliAllocationConsideringFeature(sbyte consummateLevel, List<short> featureIds)
	{
		short num = Math.Min((short)ConsummateLevel.Instance[consummateLevel].MaxNeiliAllocation, (short)400);
		int num2 = featureIds.Sum((short featureId) => CharacterFeature.Instance[featureId].MaxNeiliAllocationDebuff);
		return (short)Math.Max(0, num - num2);
	}

	[Obsolete]
	public unsafe static bool CanAllocateNeili(byte neiliAllocationType, NeiliAllocation allocation, int currNeili, sbyte consummateLevel)
	{
		short num = allocation.Items[(int)neiliAllocationType];
		if (num < 100 && allocation.GetTotal() < GetMaxTotalNeiliAllocation(consummateLevel))
		{
			return currNeili >= CalcNeiliCost(num);
		}
		return false;
	}

	public unsafe static bool CanAllocateNeiliConsideringFeature(byte neiliAllocationType, NeiliAllocation allocation, int currNeili, sbyte consummateLevel, List<short> featureIds)
	{
		short num = allocation.Items[(int)neiliAllocationType];
		if (num < 100 && allocation.GetTotal() < GetMaxTotalNeiliAllocationConsideringFeature(consummateLevel, featureIds))
		{
			return currNeili >= CalcNeiliCost(num);
		}
		return false;
	}

	public unsafe static void TryAllocateToTargetAllocation(NeiliAllocation target, ref NeiliAllocation current, int maxNeili, ref int currNeili, int maxTotalAllocation = 400, int costPercent = 100)
	{
		int* ptr = stackalloc int[4];
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 4; i++)
		{
			ptr[i] = CalcNeiliCost(current.Items[i]);
		}
		int num3 = current.GetTotal();
		do
		{
			for (int j = 0; j < 4; j++)
			{
				if (num3 >= maxTotalAllocation)
				{
					num = 15;
					break;
				}
				short num4 = current.Items[j];
				if (num4 >= target.Items[j])
				{
					num |= 1 << j;
					continue;
				}
				int num5 = ptr[j] * costPercent / 100;
				if (num5 > currNeili || ptr[j] > maxNeili)
				{
					num2 |= 1 << j;
					continue;
				}
				num3++;
				num4++;
				currNeili -= num5;
				maxNeili -= ptr[j];
				current.Items[j] = num4;
				ptr[j] = CalcNeiliCost(num4);
			}
		}
		while ((num | num2) != 15);
	}

	public static int CalcNeiliCost(short currAllocation)
	{
		int num = currAllocation + 1;
		return num + num * num / 100;
	}

	public static int CalcNeiliCostInCombat(short currAllocation, sbyte qiDisorderLevel)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		CValuePercent val = CValuePercent.op_Implicit((int)QiDisorderEffect.Instance[qiDisorderLevel].NeiliCostInCombat);
		if (val <= 0)
		{
			return 0;
		}
		return Math.Max(CalcNeiliCost(currAllocation) * val / 100 * val, 1);
	}

	public static int CalcNeiliCostFromZero(short neiliAllocation)
	{
		if (neiliAllocation == 0)
		{
			return 0;
		}
		int num = neiliAllocation - 1;
		if (num < NeiliCumulativeCosts.Length)
		{
			return NeiliCumulativeCosts[num];
		}
		int num2 = NeiliCumulativeCosts[^1];
		for (int i = NeiliCumulativeCosts.Length; i < neiliAllocation; i++)
		{
			num2 += CalcNeiliCost((short)i);
		}
		return num2;
	}

	public unsafe static int CalcRequiredNeili(NeiliAllocation allocation)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			num += CalcNeiliCostFromZero(allocation.Items[i]);
		}
		return num;
	}

	[Obsolete]
	public unsafe static short CalcAllocatedNeili(int availableNeili)
	{
		int num;
		fixed (short* neiliCumulativeCosts = NeiliCumulativeCosts)
		{
			num = CollectionUtils.BinarySearch(neiliCumulativeCosts, 0, 100, availableNeili);
		}
		if (num >= 0)
		{
			return (short)(num + 1);
		}
		num = ~num;
		return (short)num;
	}
}
