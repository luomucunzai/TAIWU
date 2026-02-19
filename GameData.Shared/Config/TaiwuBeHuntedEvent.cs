using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TaiwuBeHuntedEvent : ConfigData<TaiwuBeHuntedEventItem, short>
{
	public static class DefKey
	{
		public const short Shaolin = 0;

		public const short Emei = 1;

		public const short Baihua = 2;

		public const short Wudang = 3;

		public const short Yuanshan = 4;

		public const short Shixiang = 5;

		public const short Ranshan = 6;

		public const short Xuannv = 7;

		public const short Zhujian = 8;

		public const short Kongsang = 9;

		public const short Jingang = 10;

		public const short Wuxian = 11;

		public const short Jieqing = 12;

		public const short Fulong = 13;

		public const short Xuehou = 14;
	}

	public static class DefValue
	{
		public static TaiwuBeHuntedEventItem Shaolin => Instance[(short)0];

		public static TaiwuBeHuntedEventItem Emei => Instance[(short)1];

		public static TaiwuBeHuntedEventItem Baihua => Instance[(short)2];

		public static TaiwuBeHuntedEventItem Wudang => Instance[(short)3];

		public static TaiwuBeHuntedEventItem Yuanshan => Instance[(short)4];

		public static TaiwuBeHuntedEventItem Shixiang => Instance[(short)5];

		public static TaiwuBeHuntedEventItem Ranshan => Instance[(short)6];

		public static TaiwuBeHuntedEventItem Xuannv => Instance[(short)7];

		public static TaiwuBeHuntedEventItem Zhujian => Instance[(short)8];

		public static TaiwuBeHuntedEventItem Kongsang => Instance[(short)9];

		public static TaiwuBeHuntedEventItem Jingang => Instance[(short)10];

		public static TaiwuBeHuntedEventItem Wuxian => Instance[(short)11];

		public static TaiwuBeHuntedEventItem Jieqing => Instance[(short)12];

		public static TaiwuBeHuntedEventItem Fulong => Instance[(short)13];

		public static TaiwuBeHuntedEventItem Xuehou => Instance[(short)14];
	}

	public static TaiwuBeHuntedEvent Instance = new TaiwuBeHuntedEvent();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"LifeSkillCombatTypes", "TemplateId", "Name", "HeadEvent", "ResistEvent", "ResistWinEvent", "ResistLoseEvent", "PersuadeEvent", "PersuadeWinEvent", "PersuadeLoseEvent",
		"BribeEvent", "BribeConfirmEvent", "SurrenderEvent", "PunishEvent1", "PunishEvent2", "PunishEvent3", "PunishEvent4"
	};

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
		_dataArray.Add(new TaiwuBeHuntedEventItem(0, 0, 1, 2, 3, 4, 5, new List<sbyte> { 13 }, 6, 7, 8, 9, 10, 11, 12, 13, 14));
		_dataArray.Add(new TaiwuBeHuntedEventItem(1, 15, 16, 17, 18, 19, 20, new List<sbyte> { 13, 12 }, 21, 22, 23, 24, 25, 26, 27, 28, 29));
		_dataArray.Add(new TaiwuBeHuntedEventItem(2, 30, 31, 32, 33, 34, 35, new List<sbyte> { 8 }, 36, 37, 38, 39, 40, 41, 42, 43, 44));
		_dataArray.Add(new TaiwuBeHuntedEventItem(3, 45, 46, 47, 48, 49, 50, new List<sbyte> { 12 }, 51, 52, 53, 54, 55, 56, 57, 58, 59));
		_dataArray.Add(new TaiwuBeHuntedEventItem(4, 60, 61, 62, 63, 64, 65, new List<sbyte> { 13, 12 }, 66, 67, 68, 69, 70, 71, 72, 73, 74));
		_dataArray.Add(new TaiwuBeHuntedEventItem(5, 75, 76, 77, 78, 79, 80, new List<sbyte> { 15 }, 81, 82, 83, 84, 85, 86, 87, 88, 89));
		_dataArray.Add(new TaiwuBeHuntedEventItem(6, 90, 91, 92, 93, 94, 95, new List<sbyte> { 4 }, 96, 97, 98, 99, 100, 101, 102, 103, 104));
		_dataArray.Add(new TaiwuBeHuntedEventItem(7, 105, 106, 107, 108, 109, 110, new List<sbyte> { 0 }, 111, 112, 113, 114, 115, 116, 117, 118, 119));
		_dataArray.Add(new TaiwuBeHuntedEventItem(8, 120, 121, 122, 123, 124, 125, new List<sbyte> { 6, 7, 10, 11 }, 126, 127, 128, 129, 130, 131, 132, 133, 134));
		_dataArray.Add(new TaiwuBeHuntedEventItem(9, 135, 136, 137, 138, 139, 140, new List<sbyte> { 8, 9 }, 141, 142, 143, 144, 145, 146, 147, 148, 149));
		_dataArray.Add(new TaiwuBeHuntedEventItem(10, 150, 151, 152, 153, 154, 155, new List<sbyte> { 13 }, 156, 157, 158, 159, 160, 161, 162, 163, 164));
		_dataArray.Add(new TaiwuBeHuntedEventItem(11, 165, 166, 167, 168, 169, 170, new List<sbyte> { 9 }, 171, 172, 173, 174, 175, 176, 177, 178, 179));
		_dataArray.Add(new TaiwuBeHuntedEventItem(12, 180, 181, 182, 183, 184, 185, new List<sbyte> { 1 }, 186, 187, 188, 189, 190, 191, 192, 193, 194));
		_dataArray.Add(new TaiwuBeHuntedEventItem(13, 195, 196, 197, 198, 199, 200, new List<sbyte> { 14 }, 201, 202, 203, 204, 205, 206, 207, 208, 209));
		_dataArray.Add(new TaiwuBeHuntedEventItem(14, 210, 211, 212, 213, 214, 215, new List<sbyte> { 15 }, 216, 217, 218, 219, 220, 221, 222, 223, 224));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TaiwuBeHuntedEventItem>(15);
		CreateItems0();
	}
}
