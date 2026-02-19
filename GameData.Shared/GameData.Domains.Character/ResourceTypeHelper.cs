using System.Runtime.CompilerServices;
using Config;

namespace GameData.Domains.Character;

public static class ResourceTypeHelper
{
	public static int GetTotalWorth(this ResourceInts resources)
	{
		int num = 0;
		for (sbyte b = 0; b < 8; b++)
		{
			num += ResourceAmountToWorth(b, resources[b]);
		}
		return num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ResourceAmountToWorth(sbyte resourceType, int amount)
	{
		return GlobalConfig.ResourcesWorth[resourceType] * amount;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WorthToResourceAmount(sbyte resourceType, int worth)
	{
		return worth / GlobalConfig.ResourcesWorth[resourceType];
	}

	public static sbyte ResourceAmountToGrade(sbyte resourceType, int amount)
	{
		return ResourceWorthToGrade(ResourceAmountToWorth(resourceType, amount));
	}

	public static int GradeToResourceAmount(sbyte resourceType, sbyte grade)
	{
		int baseValue = Accessory.Instance[grade].BaseValue;
		return WorthToResourceAmount(resourceType, baseValue);
	}

	public static sbyte ResourceWorthToGrade(int worth)
	{
		for (sbyte b = 8; b >= 0; b--)
		{
			AccessoryItem accessoryItem = Accessory.Instance[b];
			if (worth >= accessoryItem.BaseValue)
			{
				return b;
			}
		}
		return -1;
	}
}
