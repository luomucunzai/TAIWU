using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class DemonSlayerTrial : ConfigData<DemonSlayerTrialItem, int>
{
	public static DemonSlayerTrial Instance = new DemonSlayerTrial();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "CharacterId", "FirstTimeRewards", "TemplateId", "Desc" };

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
		_dataArray.Add(new DemonSlayerTrialItem(0, 0, 863, 263));
		_dataArray.Add(new DemonSlayerTrialItem(1, 1, 864, 264));
		_dataArray.Add(new DemonSlayerTrialItem(2, 2, 865, 265));
		_dataArray.Add(new DemonSlayerTrialItem(3, 3, 866, 266));
		_dataArray.Add(new DemonSlayerTrialItem(4, 4, 867, 267));
		_dataArray.Add(new DemonSlayerTrialItem(5, 5, 868, 268));
		_dataArray.Add(new DemonSlayerTrialItem(6, 6, 869, 269));
		_dataArray.Add(new DemonSlayerTrialItem(7, 7, 870, 270));
		_dataArray.Add(new DemonSlayerTrialItem(8, 8, 871, 271));
		_dataArray.Add(new DemonSlayerTrialItem(9, 9, 872, 272));
		_dataArray.Add(new DemonSlayerTrialItem(10, 10, 873, 273));
		_dataArray.Add(new DemonSlayerTrialItem(11, 11, 874, 274));
		_dataArray.Add(new DemonSlayerTrialItem(12, 12, 875, 275));
		_dataArray.Add(new DemonSlayerTrialItem(13, 13, 876, 276));
		_dataArray.Add(new DemonSlayerTrialItem(14, 14, 877, 277));
		_dataArray.Add(new DemonSlayerTrialItem(15, 15, 878, 278));
		_dataArray.Add(new DemonSlayerTrialItem(16, 16, 879, 279));
		_dataArray.Add(new DemonSlayerTrialItem(17, 17, 880, 280));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<DemonSlayerTrialItem>(18);
		CreateItems0();
	}
}
