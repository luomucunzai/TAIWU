using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TutorialVideo : ConfigData<TutorialVideoItem, short>
{
	public static class DefKey
	{
		public const short Tutorial_Chapter_1_1 = 0;

		public const short Tutorial_Chapter_1_2 = 1;

		public const short Tutorial_Chapter_1_3 = 2;

		public const short Tutorial_Chapter_2_1 = 3;

		public const short Tutorial_Chapter_2_2 = 4;

		public const short Tutorial_Chapter_2_3 = 5;

		public const short Tutorial_Chapter_3_1 = 6;

		public const short Tutorial_Chapter_3_2 = 7;

		public const short Tutorial_Chapter_3_3 = 8;

		public const short Tutorial_Chapter_3_4 = 9;

		public const short Tutorial_Chapter_4_1 = 10;

		public const short Tutorial_Chapter_4_2 = 11;

		public const short Tutorial_Chapter_4_3 = 12;

		public const short Tutorial_Chapter_5_1 = 13;

		public const short Tutorial_Chapter_5_2 = 14;

		public const short Tutorial_Chapter_6_1 = 15;

		public const short Tutorial_Chapter_6_2 = 16;

		public const short Tutorial_Chapter_6_3 = 17;

		public const short Tutorial_Chapter_6_4 = 18;

		public const short Tutorial_Chapter_6_5 = 19;

		public const short Tutorial_Chapter_7_1 = 20;

		public const short Tutorial_Chapter_7_2 = 21;

		public const short Tutorial_Chapter_AdvanceMonth = 22;
	}

	public static class DefValue
	{
		public static TutorialVideoItem Tutorial_Chapter_1_1 => Instance[(short)0];

		public static TutorialVideoItem Tutorial_Chapter_1_2 => Instance[(short)1];

		public static TutorialVideoItem Tutorial_Chapter_1_3 => Instance[(short)2];

		public static TutorialVideoItem Tutorial_Chapter_2_1 => Instance[(short)3];

		public static TutorialVideoItem Tutorial_Chapter_2_2 => Instance[(short)4];

		public static TutorialVideoItem Tutorial_Chapter_2_3 => Instance[(short)5];

		public static TutorialVideoItem Tutorial_Chapter_3_1 => Instance[(short)6];

		public static TutorialVideoItem Tutorial_Chapter_3_2 => Instance[(short)7];

		public static TutorialVideoItem Tutorial_Chapter_3_3 => Instance[(short)8];

		public static TutorialVideoItem Tutorial_Chapter_3_4 => Instance[(short)9];

		public static TutorialVideoItem Tutorial_Chapter_4_1 => Instance[(short)10];

		public static TutorialVideoItem Tutorial_Chapter_4_2 => Instance[(short)11];

		public static TutorialVideoItem Tutorial_Chapter_4_3 => Instance[(short)12];

		public static TutorialVideoItem Tutorial_Chapter_5_1 => Instance[(short)13];

		public static TutorialVideoItem Tutorial_Chapter_5_2 => Instance[(short)14];

		public static TutorialVideoItem Tutorial_Chapter_6_1 => Instance[(short)15];

		public static TutorialVideoItem Tutorial_Chapter_6_2 => Instance[(short)16];

		public static TutorialVideoItem Tutorial_Chapter_6_3 => Instance[(short)17];

		public static TutorialVideoItem Tutorial_Chapter_6_4 => Instance[(short)18];

		public static TutorialVideoItem Tutorial_Chapter_6_5 => Instance[(short)19];

		public static TutorialVideoItem Tutorial_Chapter_7_1 => Instance[(short)20];

		public static TutorialVideoItem Tutorial_Chapter_7_2 => Instance[(short)21];

		public static TutorialVideoItem Tutorial_Chapter_AdvanceMonth => Instance[(short)22];
	}

	public static TutorialVideo Instance = new TutorialVideo();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "Chapter", "TemplateId", "VideoPath", "Name", "PartsTitle", "PartsDesc", "PartVideos", "ChapterName", "VideoSummary" };

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
		_dataArray.Add(new TutorialVideoItem(0, "Tutorial_Chapter_1_1", 0, new int[2] { 1, 2 }, new int[2] { 3, 4 }, new string[2] { "Tutorial_Chapter_1_1a", "Tutorial_Chapter_1_1b" }, 0, 0, 5, 6));
		_dataArray.Add(new TutorialVideoItem(1, "Tutorial_Chapter_1_2", 7, new int[2] { 8, 9 }, new int[2] { 10, 11 }, new string[2] { "Tutorial_Chapter_1_2a", "Tutorial_Chapter_1_2b" }, 0, 1, 5, 12));
		_dataArray.Add(new TutorialVideoItem(2, "Tutorial_Chapter_1_3", 13, new int[2] { 14, 15 }, new int[2] { 16, 17 }, new string[2] { "Tutorial_Chapter_1_3a", "Tutorial_Chapter_1_3b" }, 0, 2, 5, 18));
		_dataArray.Add(new TutorialVideoItem(3, "Tutorial_Chapter_2_1", 19, new int[1] { 20 }, new int[1] { 21 }, new string[1] { "Tutorial_Chapter_2_1a" }, 1, 0, 22, 23));
		_dataArray.Add(new TutorialVideoItem(4, "Tutorial_Chapter_2_2", 24, new int[4] { 25, 26, 24, 27 }, new int[4] { 28, 29, 30, 31 }, new string[4] { "Tutorial_Chapter_2_2a", "Tutorial_Chapter_2_2b", "Tutorial_Chapter_2_2c", "Tutorial_Chapter_2_2d" }, 1, 1, 22, 32));
		_dataArray.Add(new TutorialVideoItem(5, "Tutorial_Chapter_2_3", 32, new int[2] { 33, 34 }, new int[2] { 35, 36 }, new string[2] { "Tutorial_Chapter_2_3a", "Tutorial_Chapter_2_3b" }, 1, 2, 22, 37));
		_dataArray.Add(new TutorialVideoItem(6, "Tutorial_Chapter_3_1", 38, new int[2] { 39, 40 }, new int[2] { 41, 42 }, new string[2] { "Tutorial_Chapter_3_1a", "Tutorial_Chapter_3_1b" }, 2, 0, 43, 44));
		_dataArray.Add(new TutorialVideoItem(7, "Tutorial_Chapter_3_2", 45, new int[2] { 46, 45 }, new int[2] { 47, 48 }, new string[2] { "Tutorial_Chapter_3_2a", "Tutorial_Chapter_3_2b" }, 2, 1, 43, 49));
		_dataArray.Add(new TutorialVideoItem(8, "Tutorial_Chapter_3_3", 50, new int[3] { 51, 52, 53 }, new int[3] { 21, 54, 55 }, new string[3] { "Tutorial_Chapter_3_3a", "Tutorial_Chapter_3_3b", "Tutorial_Chapter_3_3c" }, 2, 2, 43, 50));
		_dataArray.Add(new TutorialVideoItem(9, "Tutorial_Chapter_3_4", 56, new int[5] { 57, 58, 59, 60, 56 }, new int[5] { 61, 62, 63, 64, 65 }, new string[5] { "Tutorial_Chapter_3_4a", "Tutorial_Chapter_3_4b", "Tutorial_Chapter_3_4c", "Tutorial_Chapter_3_4d", "Tutorial_Chapter_3_4e" }, 2, 3, 43, 66));
		_dataArray.Add(new TutorialVideoItem(10, "Tutorial_Chapter_4_1", 67, new int[2] { 1, 2 }, new int[2] { 68, 69 }, new string[2] { "Tutorial_Chapter_4_1a", "Tutorial_Chapter_4_1b" }, 3, 0, 70, 71));
		_dataArray.Add(new TutorialVideoItem(11, "Tutorial_Chapter_4_2", 72, new int[4] { 73, 74, 75, 76 }, new int[4] { 77, 78, 79, 80 }, new string[3] { "Tutorial_Chapter_4_2a", "Tutorial_Chapter_4_2b", "Tutorial_Chapter_4_2c" }, 3, 1, 70, 81));
		_dataArray.Add(new TutorialVideoItem(12, "Tutorial_Chapter_4_3", 82, new int[2] { 82, 83 }, new int[2] { 84, 85 }, new string[2] { "Tutorial_Chapter_4_3a", "Tutorial_Chapter_4_3b" }, 3, 3, 70, 82));
		_dataArray.Add(new TutorialVideoItem(13, "Tutorial_Chapter_5_1", 86, new int[3] { 87, 88, 89 }, new int[3] { 90, 91, 92 }, new string[3] { "Tutorial_Chapter_5_1a", "Tutorial_Chapter_5_1b", "Tutorial_Chapter_5_1c" }, 4, 0, 93, 94));
		_dataArray.Add(new TutorialVideoItem(14, "Tutorial_Chapter_5_2", 95, new int[1] { 95 }, new int[1] { 96 }, new string[1] { "Tutorial_Chapter_5_2a" }, 4, 1, 93, 97));
		_dataArray.Add(new TutorialVideoItem(15, "Tutorial_Chapter_6_1", 98, new int[3] { 99, 100, 98 }, new int[3] { 101, 102, 103 }, new string[3] { "Tutorial_Chapter_6_1a", "Tutorial_Chapter_6_1b", "Tutorial_Chapter_6_1c" }, 5, 0, 104, 105));
		_dataArray.Add(new TutorialVideoItem(16, "Tutorial_Chapter_6_2", 106, new int[2] { 107, 108 }, new int[2] { 109, 110 }, new string[2] { "Tutorial_Chapter_6_2a", "Tutorial_Chapter_6_2b" }, 5, 1, 104, 108));
		_dataArray.Add(new TutorialVideoItem(17, "Tutorial_Chapter_6_3", 111, new int[3] { 111, 112, 113 }, new int[3] { 114, 115, 116 }, new string[3] { "Tutorial_Chapter_6_3a", "Tutorial_Chapter_6_3b", "Tutorial_Chapter_6_3c" }, 5, 2, 104, 117));
		_dataArray.Add(new TutorialVideoItem(18, "Tutorial_Chapter_6_4", 118, new int[2] { 119, 120 }, new int[2] { 121, 122 }, new string[2] { "Tutorial_Chapter_6_4a", "Tutorial_Chapter_6_4b" }, 5, 4, 104, 123));
		_dataArray.Add(new TutorialVideoItem(19, "Tutorial_Chapter_6_5", 124, new int[7] { 125, 126, 127, 128, 129, 130, 131 }, new int[7] { 132, 133, 134, 135, 136, 137, 138 }, new string[5] { "Tutorial_Chapter_6_5a", "Tutorial_Chapter_6_5b", "Tutorial_Chapter_6_5c", "Tutorial_Chapter_6_5d", "Tutorial_Chapter_6_5e" }, 5, 5, 104, 139));
		_dataArray.Add(new TutorialVideoItem(20, "Tutorial_Chapter_7_1", 140, new int[3] { 141, 140, 142 }, new int[3] { 143, 144, 145 }, new string[3] { "Tutorial_Chapter_7_1a", "Tutorial_Chapter_7_1b", "Tutorial_Chapter_7_1c" }, 6, 0, 146, 140));
		_dataArray.Add(new TutorialVideoItem(21, "Tutorial_Chapter_7_2", 147, new int[2] { 147, 148 }, new int[2] { 149, 150 }, new string[2] { "Tutorial_Chapter_7_2a", "Tutorial_Chapter_7_2b" }, 6, 1, 146, 151));
		_dataArray.Add(new TutorialVideoItem(22, "Tutorial_Chapter_AdvanceMonth", 152, new int[1] { 153 }, new int[1] { 154 }, new string[1] { "Tutorial_Chapter_AdvanceMonth" }, -1, -1, 155, 37));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TutorialVideoItem>(23);
		CreateItems0();
	}
}
