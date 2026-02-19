using System;
using Config;

namespace GameData.Domains.World;

public static class SharedMethods
{
	public static sbyte CalcMonthInYear(int date)
	{
		return (sbyte)(date % 12);
	}

	public static sbyte GetXiangshuLevel(sbyte xiangshuProgress)
	{
		return (sbyte)(xiangshuProgress / 2);
	}

	public static sbyte GetMaxGradeOfXiangshuInfection(sbyte xiangshuProgress)
	{
		sbyte xiangshuLevel = GetXiangshuLevel(xiangshuProgress);
		if (xiangshuLevel == 0)
		{
			return -1;
		}
		return Math.Min(xiangshuLevel, 8);
	}

	public static bool SmallVillageXiangshu(short orgTemplateId, bool includeXiangshuInfected = true)
	{
		if (SmallVillageXiangshuProgress())
		{
			if (orgTemplateId != 19)
			{
				return orgTemplateId == 20 && includeXiangshuInfected;
			}
			return true;
		}
		return false;
	}

	public static bool SmallVillageXiangshuProgress()
	{
		short mainStoryLineProgress = ExternalDataBridge.Context.MainStoryLineProgress;
		if (mainStoryLineProgress != 4)
		{
			return mainStoryLineProgress == 5;
		}
		return true;
	}

	public static short GetGainResourcePercent(byte worldResourceType)
	{
		byte worldResourceAmountType = GetWorldResourceAmountType();
		return WorldResource.Instance[worldResourceType].InfluenceFactors[worldResourceAmountType];
	}

	public static byte GetWorldResourceAmountType()
	{
		return ExternalDataBridge.Context.WorldResourceAmountType;
	}

	public static double GetApproveTaiwuDisplayData(short approvingRate)
	{
		return Math.Round((float)approvingRate / 10f, 1);
	}

	public static string GetApproveTaiwuDisplayDataString(short approvingRate)
	{
		return Math.Round((float)approvingRate / 10f, 1) + "%";
	}

	public static sbyte GetInvasionWorldStateTemplateId()
	{
		return GetXiangshuLevel(ExternalDataBridge.Context.XiangshuProgress) switch
		{
			0 => 46, 
			1 => 47, 
			2 => 0, 
			3 => 1, 
			4 => 2, 
			5 => 3, 
			6 => 4, 
			7 => 5, 
			8 => 6, 
			_ => 48, 
		};
	}
}
