using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells.Character;

namespace Config;

[Serializable]
public class DemonSlayerTrialLevel : ConfigData<DemonSlayerTrialLevelItem, int>
{
	public static DemonSlayerTrialLevel Instance = new DemonSlayerTrialLevel();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "RewardItems", "RewardFeatureOptions", "TemplateId", "LevelName", "TotalPower", "RewardExp" };

	internal override int ToInt(int value)
	{
		return value;
	}

	internal override int ToTemplateId(int value)
	{
		return value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new DemonSlayerTrialLevelItem(0, 0, 4, 3, 18000, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 6, 1, 100)
		}, new List<short>
		{
			692, 695, 698, 701, 704, 707, 710, 713, 716, 719,
			722, 725, 728, 731
		}));
		_dataArray.Add(new DemonSlayerTrialLevelItem(1, 1, 5, 3, 27000, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 6, 2, 100)
		}, new List<short>
		{
			692, 695, 698, 701, 704, 707, 710, 713, 716, 719,
			722, 725, 728, 731
		}));
		_dataArray.Add(new DemonSlayerTrialLevelItem(2, 2, 6, 3, 36000, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 6, 3, 100)
		}, new List<short>
		{
			692, 695, 698, 701, 704, 707, 710, 713, 716, 719,
			722, 725, 728, 731
		}));
		_dataArray.Add(new DemonSlayerTrialLevelItem(3, 3, 7, 3, 45000, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 7, 1, 100)
		}, new List<short>
		{
			693, 696, 699, 702, 705, 708, 711, 714, 717, 720,
			723, 726, 729, 732
		}));
		_dataArray.Add(new DemonSlayerTrialLevelItem(4, 4, 8, 3, 54000, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 7, 2, 100)
		}, new List<short>
		{
			693, 696, 699, 702, 705, 708, 711, 714, 717, 720,
			723, 726, 729, 732
		}));
		_dataArray.Add(new DemonSlayerTrialLevelItem(5, 5, 9, 3, 63000, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 7, 3, 100)
		}, new List<short>
		{
			693, 696, 699, 702, 705, 708, 711, 714, 717, 720,
			723, 726, 729, 732
		}));
		_dataArray.Add(new DemonSlayerTrialLevelItem(6, 6, 10, 3, 72000, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 8, 1, 100)
		}, new List<short>
		{
			694, 697, 700, 703, 706, 709, 712, 715, 718, 721,
			724, 727, 730, 733
		}));
		_dataArray.Add(new DemonSlayerTrialLevelItem(7, 7, 11, 3, 81000, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 8, 2, 100)
		}, new List<short>
		{
			694, 697, 700, 703, 706, 709, 712, 715, 718, 721,
			724, 727, 730, 733
		}));
		_dataArray.Add(new DemonSlayerTrialLevelItem(8, 8, 12, 3, 90000, new List<PresetInventoryItem>
		{
			new PresetInventoryItem("Misc", 8, 3, 100)
		}, new List<short>
		{
			694, 697, 700, 703, 706, 709, 712, 715, 718, 721,
			724, 727, 730, 733
		}));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<DemonSlayerTrialLevelItem>(9);
		CreateItems0();
	}
}
