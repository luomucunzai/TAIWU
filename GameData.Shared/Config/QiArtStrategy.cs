using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class QiArtStrategy : ConfigData<QiArtStrategyItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte ConcentrationGainAccordingToStrategies = 39;
	}

	public static class DefValue
	{
		public static QiArtStrategyItem ConcentrationGainAccordingToStrategies => Instance[(sbyte)39];
	}

	public static QiArtStrategy Instance = new QiArtStrategy();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "Dialog", "ExtractGroup", "ExtractWeight" };

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
		_dataArray.Add(new QiArtStrategyItem(0, 0, 1, 2, 1, 4, 10, 3, 0, -1, -1, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(1, 3, 4, 5, 1, 4, 10, 3, 0, -1, -1, 0, 0, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(2, 6, 7, 8, 1, 4, 10, 3, 0, -1, -1, 0, 0, 0, 0, 100, 100, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(3, 9, 10, 11, 1, 2, 15, 3, 0, -1, -1, 50, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(4, 12, 13, 14, 1, 2, 15, 3, 0, -1, -1, 0, 0, 50, 150, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(5, 15, 16, 17, 1, 2, 15, 3, 0, -1, -1, 0, 0, 0, 0, 50, 150, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(6, 18, 19, 20, 1, 1, 20, 6, 50, -1, -1, 200, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(7, 21, 22, 23, 1, 1, 20, 6, 50, -1, -1, 0, 0, 200, 200, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(8, 24, 25, 26, 1, 1, 20, 6, 50, -1, -1, 0, 0, 0, 0, 200, 200, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(9, 27, 28, 29, 1, 4, 10, 3, 0, -1, -1, 0, 0, 0, 0, 0, 0, 50, 50, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(10, 30, 31, 32, 1, 4, 10, 3, 0, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(11, 33, 34, 35, 1, 4, 10, 3, 0, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(12, 36, 37, 38, 1, 2, 15, 3, 0, -1, -1, 0, 0, 0, 0, 0, 0, 30, 70, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(13, 39, 40, 41, 1, 2, 15, 3, 0, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 30, 70, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(14, 42, 43, 44, 1, 2, 15, 3, 0, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 70, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(15, 45, 46, 47, 1, 1, 20, 6, 100, -1, -1, 0, 0, 0, 0, 0, 0, 100, 100, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(16, 48, 49, 50, 1, 1, 20, 6, 100, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 100, 100, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(17, 51, 52, 53, 1, 1, 20, 6, 100, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 100, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(18, 54, 55, 56, 2, 1, 30, 9, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(19, 57, 58, 59, 2, 1, 30, 9, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(20, 60, 61, 62, 2, 1, 30, 9, 0, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(21, 63, 64, 65, 2, 1, 30, 9, 0, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(22, 66, 67, 68, 2, 1, 30, 9, 0, 1, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(23, 69, 70, 71, 2, 1, 30, 9, 0, 1, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(24, 72, 73, 74, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(25, 75, 76, 77, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(26, 78, 79, 80, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(27, 81, 82, 83, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(28, 84, 85, 86, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(29, 87, 88, 89, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(30, 90, 91, 92, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(31, 93, 94, 95, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 3, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(32, 96, 97, 98, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(33, 99, 100, 101, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 2, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(34, 102, 103, 104, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(35, 105, 106, 107, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(36, 108, 109, 110, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 2, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(37, 111, 112, 113, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 3, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(38, 114, 115, 116, 2, 1, 20, 6, 25, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 1, clearOtherEffect: false));
		_dataArray.Add(new QiArtStrategyItem(39, 117, 118, 119, 2, 5, 0, 9, 0, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: true));
		_dataArray.Add(new QiArtStrategyItem(40, 120, 121, 122, 2, 15, 20, 3, 0, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, clearOtherEffect: true));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<QiArtStrategyItem>(41);
		CreateItems0();
	}
}
