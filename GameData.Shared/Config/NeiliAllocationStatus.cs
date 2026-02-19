using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class NeiliAllocationStatus : ConfigData<NeiliAllocationStatusItem, sbyte>
{
	public static NeiliAllocationStatus Instance = new NeiliAllocationStatus();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"TemplateId", "Type", "Name", "Desc", "MinThreshold", "MaxThreshold", "GoneMadInjuryRate", "GoneMadInjuryBonus", "PowerAddPercent", "CostNeiliAllocation",
		"AddNeiliAllocation", "MarkCount"
	};

	internal override int ToInt(sbyte value)
	{
		return value;
	}

	internal override sbyte ToTemplateId(int value)
	{
		return (sbyte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new NeiliAllocationStatusItem(0, ENeiliAllocationStatusType.Scatter, 0, 1, 0, 39, allowEqualsMin: true, 100, -50, -20, -30, 60, -2));
		_dataArray.Add(new NeiliAllocationStatusItem(1, ENeiliAllocationStatusType.Leak, 2, 3, 39, 79, allowEqualsMin: false, 50, -25, -20, -15, 30, -1));
		_dataArray.Add(new NeiliAllocationStatusItem(2, ENeiliAllocationStatusType.None, 4, 5, 79, 120, allowEqualsMin: false, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new NeiliAllocationStatusItem(3, ENeiliAllocationStatusType.Full, 6, 7, 120, 210, allowEqualsMin: false, -25, 50, 20, 30, -15, 0));
		_dataArray.Add(new NeiliAllocationStatusItem(4, ENeiliAllocationStatusType.Bulge, 8, 9, 210, 300, allowEqualsMin: false, -50, 100, 20, 60, -30, 1));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<NeiliAllocationStatusItem>(5);
		CreateItems0();
	}
}
