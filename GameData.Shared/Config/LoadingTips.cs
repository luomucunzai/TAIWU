using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LoadingTips : ConfigData<LoadingTipsItem, int>
{
	public static class DefKey
	{
		public const int PoemBegin = 0;

		public const int PoemEnd = 59;

		public const int CommonTipsBegin = 60;

		public const int CommonTipsEnd = 91;
	}

	public static class DefValue
	{
		public static LoadingTipsItem PoemBegin => Instance[0];

		public static LoadingTipsItem PoemEnd => Instance[59];

		public static LoadingTipsItem CommonTipsBegin => Instance[60];

		public static LoadingTipsItem CommonTipsEnd => Instance[91];
	}

	public static LoadingTips Instance = new LoadingTips();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Content" };

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
		_dataArray.Add(new LoadingTipsItem(0, 0, 0));
		_dataArray.Add(new LoadingTipsItem(1, 1, 0));
		_dataArray.Add(new LoadingTipsItem(2, 2, 0));
		_dataArray.Add(new LoadingTipsItem(3, 3, 0));
		_dataArray.Add(new LoadingTipsItem(4, 4, 0));
		_dataArray.Add(new LoadingTipsItem(5, 5, 0));
		_dataArray.Add(new LoadingTipsItem(6, 6, 0));
		_dataArray.Add(new LoadingTipsItem(7, 7, 0));
		_dataArray.Add(new LoadingTipsItem(8, 8, 0));
		_dataArray.Add(new LoadingTipsItem(9, 9, 0));
		_dataArray.Add(new LoadingTipsItem(10, 10, 0));
		_dataArray.Add(new LoadingTipsItem(11, 11, 0));
		_dataArray.Add(new LoadingTipsItem(12, 12, 0));
		_dataArray.Add(new LoadingTipsItem(13, 13, 0));
		_dataArray.Add(new LoadingTipsItem(14, 14, 0));
		_dataArray.Add(new LoadingTipsItem(15, 15, 0));
		_dataArray.Add(new LoadingTipsItem(16, 16, 0));
		_dataArray.Add(new LoadingTipsItem(17, 17, 0));
		_dataArray.Add(new LoadingTipsItem(18, 18, 0));
		_dataArray.Add(new LoadingTipsItem(19, 19, 0));
		_dataArray.Add(new LoadingTipsItem(20, 20, 0));
		_dataArray.Add(new LoadingTipsItem(21, 21, 0));
		_dataArray.Add(new LoadingTipsItem(22, 22, 0));
		_dataArray.Add(new LoadingTipsItem(23, 23, 0));
		_dataArray.Add(new LoadingTipsItem(24, 24, 0));
		_dataArray.Add(new LoadingTipsItem(25, 25, 0));
		_dataArray.Add(new LoadingTipsItem(26, 26, 0));
		_dataArray.Add(new LoadingTipsItem(27, 27, 0));
		_dataArray.Add(new LoadingTipsItem(28, 28, 0));
		_dataArray.Add(new LoadingTipsItem(29, 29, 0));
		_dataArray.Add(new LoadingTipsItem(30, 30, 0));
		_dataArray.Add(new LoadingTipsItem(31, 31, 0));
		_dataArray.Add(new LoadingTipsItem(32, 32, 0));
		_dataArray.Add(new LoadingTipsItem(33, 33, 0));
		_dataArray.Add(new LoadingTipsItem(34, 34, 0));
		_dataArray.Add(new LoadingTipsItem(35, 35, 0));
		_dataArray.Add(new LoadingTipsItem(36, 36, 0));
		_dataArray.Add(new LoadingTipsItem(37, 37, 0));
		_dataArray.Add(new LoadingTipsItem(38, 38, 0));
		_dataArray.Add(new LoadingTipsItem(39, 39, 0));
		_dataArray.Add(new LoadingTipsItem(40, 40, 0));
		_dataArray.Add(new LoadingTipsItem(41, 41, 0));
		_dataArray.Add(new LoadingTipsItem(42, 42, 0));
		_dataArray.Add(new LoadingTipsItem(43, 43, 0));
		_dataArray.Add(new LoadingTipsItem(44, 44, 0));
		_dataArray.Add(new LoadingTipsItem(45, 45, 0));
		_dataArray.Add(new LoadingTipsItem(46, 46, 0));
		_dataArray.Add(new LoadingTipsItem(47, 47, 0));
		_dataArray.Add(new LoadingTipsItem(48, 48, 0));
		_dataArray.Add(new LoadingTipsItem(49, 49, 0));
		_dataArray.Add(new LoadingTipsItem(50, 50, 0));
		_dataArray.Add(new LoadingTipsItem(51, 51, 0));
		_dataArray.Add(new LoadingTipsItem(52, 52, 0));
		_dataArray.Add(new LoadingTipsItem(53, 53, 0));
		_dataArray.Add(new LoadingTipsItem(54, 54, 0));
		_dataArray.Add(new LoadingTipsItem(55, 55, 0));
		_dataArray.Add(new LoadingTipsItem(56, 56, 0));
		_dataArray.Add(new LoadingTipsItem(57, 57, 0));
		_dataArray.Add(new LoadingTipsItem(58, 58, 0));
		_dataArray.Add(new LoadingTipsItem(59, 59, 0));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new LoadingTipsItem(60, 60, 0));
		_dataArray.Add(new LoadingTipsItem(61, 61, 0));
		_dataArray.Add(new LoadingTipsItem(62, 62, 0));
		_dataArray.Add(new LoadingTipsItem(63, 63, 0));
		_dataArray.Add(new LoadingTipsItem(64, 64, 0));
		_dataArray.Add(new LoadingTipsItem(65, 65, 0));
		_dataArray.Add(new LoadingTipsItem(66, 66, 0));
		_dataArray.Add(new LoadingTipsItem(67, 67, 0));
		_dataArray.Add(new LoadingTipsItem(68, 68, 0));
		_dataArray.Add(new LoadingTipsItem(69, 69, 0));
		_dataArray.Add(new LoadingTipsItem(70, 70, 0));
		_dataArray.Add(new LoadingTipsItem(71, 71, 0));
		_dataArray.Add(new LoadingTipsItem(72, 72, 0));
		_dataArray.Add(new LoadingTipsItem(73, 73, 0));
		_dataArray.Add(new LoadingTipsItem(74, 74, 0));
		_dataArray.Add(new LoadingTipsItem(75, 75, 0));
		_dataArray.Add(new LoadingTipsItem(76, 76, 0));
		_dataArray.Add(new LoadingTipsItem(77, 77, 0));
		_dataArray.Add(new LoadingTipsItem(78, 78, 0));
		_dataArray.Add(new LoadingTipsItem(79, 79, 0));
		_dataArray.Add(new LoadingTipsItem(80, 80, 0));
		_dataArray.Add(new LoadingTipsItem(81, 81, 0));
		_dataArray.Add(new LoadingTipsItem(82, 82, 0));
		_dataArray.Add(new LoadingTipsItem(83, 83, 0));
		_dataArray.Add(new LoadingTipsItem(84, 84, 0));
		_dataArray.Add(new LoadingTipsItem(85, 85, 0));
		_dataArray.Add(new LoadingTipsItem(86, 86, 0));
		_dataArray.Add(new LoadingTipsItem(87, 87, 0));
		_dataArray.Add(new LoadingTipsItem(88, 88, 0));
		_dataArray.Add(new LoadingTipsItem(89, 89, 0));
		_dataArray.Add(new LoadingTipsItem(90, 90, 0));
		_dataArray.Add(new LoadingTipsItem(91, 91, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<LoadingTipsItem>(92);
		CreateItems0();
		CreateItems1();
	}
}
