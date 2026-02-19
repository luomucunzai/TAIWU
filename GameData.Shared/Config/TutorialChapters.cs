using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TutorialChapters : ConfigData<TutorialChaptersItem, short>
{
	public static class DefKey
	{
		public const short Chapter1 = 0;

		public const short Chapter2 = 1;

		public const short Chapter3 = 2;

		public const short Chapter4 = 3;

		public const short Chapter5 = 4;

		public const short Chapter6 = 5;

		public const short Chapter7 = 6;

		public const short Chapter8 = 7;

		public const short AvailableAfterMainLine = 8;
	}

	public static class DefValue
	{
		public static TutorialChaptersItem Chapter1 => Instance[(short)0];

		public static TutorialChaptersItem Chapter2 => Instance[(short)1];

		public static TutorialChaptersItem Chapter3 => Instance[(short)2];

		public static TutorialChaptersItem Chapter4 => Instance[(short)3];

		public static TutorialChaptersItem Chapter5 => Instance[(short)4];

		public static TutorialChaptersItem Chapter6 => Instance[(short)5];

		public static TutorialChaptersItem Chapter7 => Instance[(short)6];

		public static TutorialChaptersItem Chapter8 => Instance[(short)7];

		public static TutorialChaptersItem AvailableAfterMainLine => Instance[(short)8];
	}

	public static TutorialChapters Instance = new TutorialChapters();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "MainCharacter", "FixedCharacters", "TemplateId", "Name", "Desc", "MapAreaPresetKey", "StartBlockCoordinate", "Head", "Tail" };

	internal override int ToInt(short value)
	{
		return value;
	}

	internal override short ToTemplateId(int value)
	{
		return (short)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new TutorialChaptersItem(0, 0, 1, 444, new Tuple<string, short>[1]
		{
			new Tuple<string, short>("Father", 453)
		}, "Born2", new byte[2] { 13, 8 }, 1, 2, 3));
		_dataArray.Add(new TutorialChaptersItem(1, 4, 5, 444, new Tuple<string, short>[1]
		{
			new Tuple<string, short>("Father", 453)
		}, "Born2", new byte[2] { 9, 9 }, 11, 2, 3));
		_dataArray.Add(new TutorialChaptersItem(2, 6, 7, 444, new Tuple<string, short>[2]
		{
			new Tuple<string, short>("Father", 453),
			new Tuple<string, short>("Woodie", 456)
		}, "Born2", new byte[2] { 9, 9 }, 11, 2, 3));
		_dataArray.Add(new TutorialChaptersItem(3, 8, 9, 444, new Tuple<string, short>[2]
		{
			new Tuple<string, short>("Father", 453),
			new Tuple<string, short>("Xuxiangong", 448)
		}, "Born2", new byte[2] { 9, 9 }, 5, 2, 3));
		_dataArray.Add(new TutorialChaptersItem(4, 10, 11, 444, new Tuple<string, short>[0], "Born2", new byte[2] { 9, 9 }, 5, 2, 3));
		_dataArray.Add(new TutorialChaptersItem(5, 12, 13, 444, new Tuple<string, short>[3]
		{
			new Tuple<string, short>("Father", 453),
			new Tuple<string, short>("Xuxiangong", 448),
			new Tuple<string, short>("LittleMonk", 436)
		}, "Born2", new byte[2] { 9, 9 }, 5, 2, 3));
		_dataArray.Add(new TutorialChaptersItem(6, 14, 15, 444, new Tuple<string, short>[2]
		{
			new Tuple<string, short>("LittleMonk", 436),
			new Tuple<string, short>("BigShadow", 439)
		}, "Born2", new byte[2] { 9, 9 }, 6, 2, 3));
		_dataArray.Add(new TutorialChaptersItem(7, 16, 17, 453, new Tuple<string, short>[1]
		{
			new Tuple<string, short>("Xuxiangong", 448)
		}, "Born2", new byte[2] { 9, 9 }, 6, 2, 3));
		_dataArray.Add(new TutorialChaptersItem(8, 18, 18, -1, new Tuple<string, short>[0], null, null, -1, 19, 20));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TutorialChaptersItem>(9);
		CreateItems0();
	}
}
