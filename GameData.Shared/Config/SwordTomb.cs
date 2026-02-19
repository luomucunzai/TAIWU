using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SwordTomb : ConfigData<SwordTombItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte Monv = 0;

		public const sbyte DayueYaochang = 1;

		public const sbyte Jiuhan = 2;

		public const sbyte JinHuanger = 3;

		public const sbyte YiYihou = 4;

		public const sbyte WeiQi = 5;

		public const sbyte Yixiang = 6;

		public const sbyte Xuefeng = 7;

		public const sbyte ShuFang = 8;
	}

	public static class DefValue
	{
		public static SwordTombItem Monv => Instance[(sbyte)0];

		public static SwordTombItem DayueYaochang => Instance[(sbyte)1];

		public static SwordTombItem Jiuhan => Instance[(sbyte)2];

		public static SwordTombItem JinHuanger => Instance[(sbyte)3];

		public static SwordTombItem YiYihou => Instance[(sbyte)4];

		public static SwordTombItem WeiQi => Instance[(sbyte)5];

		public static SwordTombItem Yixiang => Instance[(sbyte)6];

		public static SwordTombItem Xuefeng => Instance[(sbyte)7];

		public static SwordTombItem ShuFang => Instance[(sbyte)8];
	}

	public static SwordTomb Instance = new SwordTomb();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "SwordTombAdventure", "XiangshuAvatarBegin", "WeakenedXiangshuAvatarBegin", "JuniorXiangshuAvatar", "PuppetXiangshuAvatar", "ImmortalXiangshuAvatar", "Legacies", "BigEventWhenRemoved", "TemplateId" };

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
		_dataArray.Add(new SwordTombItem(0, 114, 48, 129, 39, 490, 479, new short[4] { 722, 731, 740, 0 }, 0));
		_dataArray.Add(new SwordTombItem(1, 111, 57, 138, 40, 491, 480, new short[4] { 723, 732, 741, 1 }, 1));
		_dataArray.Add(new SwordTombItem(2, 108, 66, 147, 41, 492, 481, new short[4] { 724, 733, 742, 2 }, 2));
		_dataArray.Add(new SwordTombItem(3, 110, 75, 156, 42, 493, 482, new short[4] { 725, 734, 743, 3 }, 3));
		_dataArray.Add(new SwordTombItem(4, 109, 84, 165, 43, 494, 483, new short[4] { 726, 735, 744, 4 }, 4));
		_dataArray.Add(new SwordTombItem(5, 113, 93, 174, 44, 495, 484, new short[4] { 727, 736, 745, 5 }, 5));
		_dataArray.Add(new SwordTombItem(6, 116, 102, 183, 45, 496, 485, new short[4] { 728, 737, 746, 6 }, 6));
		_dataArray.Add(new SwordTombItem(7, 115, 111, 192, 46, 497, 486, new short[4] { 729, 738, 747, 7 }, 7));
		_dataArray.Add(new SwordTombItem(8, 112, 120, 201, 47, 498, 487, new short[4] { 730, 739, 748, 8 }, 8));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SwordTombItem>(9);
		CreateItems0();
	}
}
