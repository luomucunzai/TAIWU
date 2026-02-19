using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class ReadingStrategy : ConfigData<ReadingStrategyItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte DoubleCurrentPageStrategyAddValues = 4;

		public const sbyte DoubleCurrentPageStrategyEfficiencyChange = 5;

		public const sbyte CostBookDurabilityToAddReadingProgress = 6;

		public const sbyte CostBookDurabilityToAddReadingEfficiency = 7;

		public const sbyte ReduceIntCostButNoExpGain = 12;

		public const sbyte IntGainAccordingToStrategies = 14;

		public const sbyte NotesByAdoptiveFather = 18;
	}

	public static class DefValue
	{
		public static ReadingStrategyItem DoubleCurrentPageStrategyAddValues => Instance[(sbyte)4];

		public static ReadingStrategyItem DoubleCurrentPageStrategyEfficiencyChange => Instance[(sbyte)5];

		public static ReadingStrategyItem CostBookDurabilityToAddReadingProgress => Instance[(sbyte)6];

		public static ReadingStrategyItem CostBookDurabilityToAddReadingEfficiency => Instance[(sbyte)7];

		public static ReadingStrategyItem ReduceIntCostButNoExpGain => Instance[(sbyte)12];

		public static ReadingStrategyItem IntGainAccordingToStrategies => Instance[(sbyte)14];

		public static ReadingStrategyItem NotesByAdoptiveFather => Instance[(sbyte)18];
	}

	public static ReadingStrategy Instance = new ReadingStrategy();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "Dialog" };

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
		_dataArray.Add(new ReadingStrategyItem(0, 0, 1, 2, 1, 4, 10, 3, 0, 20, 20, 0, 0, 0, 0, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(1, 3, 4, 5, 2, 4, 10, 3, 0, 0, 0, 60, 60, 0, 0, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(2, 6, 7, 8, 1, 2, 15, 3, 0, 10, 30, 0, 0, 0, 0, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(3, 9, 10, 11, 2, 2, 15, 3, 0, 0, 0, 45, 75, 0, 0, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(4, 12, 13, 14, 1, 1, 15, 6, 0, 0, 0, 0, 0, 0, 0, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(5, 15, 16, 17, 2, 1, 15, 6, 0, 0, 0, 0, 0, 0, 0, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(6, 18, 19, 20, 1, 1, 20, 9, 1, 40, 40, 0, 0, 0, 0, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(7, 21, 22, 23, 2, 1, 20, 9, 1, 0, 0, 90, 90, 0, 0, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(8, 24, 25, 26, 1, 1, 20, 3, 0, 0, 0, 0, 0, 15, 0, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(9, 27, 28, 29, 2, 1, 20, 3, 0, 0, 0, 0, 0, 0, 25, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(10, 30, 31, 32, 1, 1, 25, 6, 0, 30, 30, 0, 0, 0, 0, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(11, 33, 34, 35, 2, 1, 25, 6, 0, 0, 0, 75, 75, 0, 0, 0, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(12, 36, 37, 38, 3, 4, 0, 6, 0, 0, 0, 0, 0, 0, 0, -10, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(13, 39, 40, 41, 3, 2, 10, 9, 0, 0, 0, 0, 0, 0, 0, -10, skipPage: false, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(14, 42, 43, 44, 3, 1, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, skipPage: false, clearPageStrategies: true));
		_dataArray.Add(new ReadingStrategyItem(15, 45, 46, 47, 3, 4, 20, 3, 0, 0, 0, 0, 0, 0, 0, 0, skipPage: false, clearPageStrategies: true));
		_dataArray.Add(new ReadingStrategyItem(16, 48, 49, 50, 3, 2, 0, 12, 0, 0, 0, 0, 0, 0, -50, 0, skipPage: true, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(17, 51, 52, 53, 3, 1, 30, 3, 0, 0, 0, 0, 0, 0, 0, 0, skipPage: true, clearPageStrategies: false));
		_dataArray.Add(new ReadingStrategyItem(18, 54, 55, 56, 0, 0, 0, 99, 0, 100, 100, 0, 0, 0, 0, 0, skipPage: false, clearPageStrategies: false));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ReadingStrategyItem>(19);
		CreateItems0();
	}
}
