using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class ConsummateLevel : ConfigData<ConsummateLevelItem, sbyte>
{
	public static ConsummateLevel Instance = new ConsummateLevel();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "Grade", "MaxNeiliAllocation" };

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
		_dataArray.Add(new ConsummateLevelItem(0, 0, 1, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new ConsummateLevelItem(1, 2, 3, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new ConsummateLevelItem(2, 4, 5, 0, 80, 0, 0, 0, 0, 100, 0, 0, 0, 0));
		_dataArray.Add(new ConsummateLevelItem(3, 6, 7, 0, 100, 0, 0, 0, 0, 100, 0, 0, 0, 0));
		_dataArray.Add(new ConsummateLevelItem(4, 8, 9, 1, 120, 33, -33, 10, 10, 100, 0, 0, 0, 0));
		_dataArray.Add(new ConsummateLevelItem(5, 10, 11, 1, 140, 33, -33, 10, 10, 100, 0, 0, 0, 0));
		_dataArray.Add(new ConsummateLevelItem(6, 12, 13, 2, 160, 33, -33, 10, 10, 100, 0, 100, 0, 0));
		_dataArray.Add(new ConsummateLevelItem(7, 14, 15, 2, 180, 33, -33, 10, 10, 100, 0, 100, 0, 0));
		_dataArray.Add(new ConsummateLevelItem(8, 16, 17, 3, 200, 33, -33, 10, 10, 100, 0, 100, 25, 0));
		_dataArray.Add(new ConsummateLevelItem(9, 18, 19, 3, 220, 33, -33, 10, 10, 100, 0, 100, 25, 0));
		_dataArray.Add(new ConsummateLevelItem(10, 20, 21, 4, 240, 66, -66, 25, 25, 100, 0, 100, 25, 0));
		_dataArray.Add(new ConsummateLevelItem(11, 22, 23, 4, 260, 66, -66, 25, 25, 100, 0, 100, 25, 0));
		_dataArray.Add(new ConsummateLevelItem(12, 24, 25, 5, 280, 66, -66, 25, 25, 100, 100, 100, 25, 0));
		_dataArray.Add(new ConsummateLevelItem(13, 26, 27, 5, 300, 66, -66, 25, 25, 100, 100, 100, 25, 0));
		_dataArray.Add(new ConsummateLevelItem(14, 28, 29, 6, 320, 66, -66, 25, 25, 100, 100, 100, 25, 6));
		_dataArray.Add(new ConsummateLevelItem(15, 30, 31, 6, 340, 66, -66, 25, 25, 100, 100, 100, 25, 6));
		_dataArray.Add(new ConsummateLevelItem(16, 32, 33, 7, 360, 99, -99, 45, 45, 100, 100, 100, 25, 6));
		_dataArray.Add(new ConsummateLevelItem(17, 34, 35, 7, 380, 99, -99, 45, 45, 100, 100, 100, 25, 6));
		_dataArray.Add(new ConsummateLevelItem(18, 36, 37, 8, 400, 99, -99, 70, 70, 100, 100, 100, 25, 6));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ConsummateLevelItem>(19);
		CreateItems0();
	}
}
