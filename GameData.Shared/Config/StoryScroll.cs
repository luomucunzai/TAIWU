using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class StoryScroll : ConfigData<StoryScrollItem, short>
{
	public static class DefKey
	{
		public const short MonvBegin = 0;

		public const short DaYueYaoChangBegin = 6;

		public const short JiuHanBegin = 12;

		public const short JinHuangerBegin = 18;

		public const short YiYiHouBegin = 24;

		public const short WeiQiBegin = 30;

		public const short YiXiangBegin = 36;

		public const short XueFengBegin = 42;

		public const short ShuFangBegin = 48;

		public const short SectShaoLinGood = 54;

		public const short SectShaoLinBad = 55;

		public const short SectEMeiGood = 56;

		public const short SectEMeiBad = 57;

		public const short SectBaiHuaGood = 58;

		public const short SectBaiHuaBad = 59;

		public const short SectWuDangGood = 60;

		public const short SectWuDangBad = 61;

		public const short SectYuanShanGood = 62;

		public const short SectYuanShanBad = 63;

		public const short SectShiXiangGood = 64;

		public const short SectShiXiangBad = 65;

		public const short SectRanShanGood = 66;

		public const short SectRanShanBad = 67;

		public const short SectXuanNvGood = 68;

		public const short SectXuanNvBad = 69;

		public const short SectZhuJianGood = 70;

		public const short SectZhuJianBad = 71;

		public const short SectKongSangGood = 72;

		public const short SectKongSangBad = 73;

		public const short SectJinGangGood = 74;

		public const short SectJinGangBad = 75;

		public const short SectWuXianGood = 76;

		public const short SectWuXianBad = 77;

		public const short SectJieQingGood = 78;

		public const short SectJieQingBad = 79;

		public const short SectFuLongGood = 80;

		public const short SectFuLongBad = 81;

		public const short SectXueHouGood = 82;

		public const short SectXueHouBad = 83;
	}

	public static class DefValue
	{
		public static StoryScrollItem MonvBegin => Instance[(short)0];

		public static StoryScrollItem DaYueYaoChangBegin => Instance[(short)6];

		public static StoryScrollItem JiuHanBegin => Instance[(short)12];

		public static StoryScrollItem JinHuangerBegin => Instance[(short)18];

		public static StoryScrollItem YiYiHouBegin => Instance[(short)24];

		public static StoryScrollItem WeiQiBegin => Instance[(short)30];

		public static StoryScrollItem YiXiangBegin => Instance[(short)36];

		public static StoryScrollItem XueFengBegin => Instance[(short)42];

		public static StoryScrollItem ShuFangBegin => Instance[(short)48];

		public static StoryScrollItem SectShaoLinGood => Instance[(short)54];

		public static StoryScrollItem SectShaoLinBad => Instance[(short)55];

		public static StoryScrollItem SectEMeiGood => Instance[(short)56];

		public static StoryScrollItem SectEMeiBad => Instance[(short)57];

		public static StoryScrollItem SectBaiHuaGood => Instance[(short)58];

		public static StoryScrollItem SectBaiHuaBad => Instance[(short)59];

		public static StoryScrollItem SectWuDangGood => Instance[(short)60];

		public static StoryScrollItem SectWuDangBad => Instance[(short)61];

		public static StoryScrollItem SectYuanShanGood => Instance[(short)62];

		public static StoryScrollItem SectYuanShanBad => Instance[(short)63];

		public static StoryScrollItem SectShiXiangGood => Instance[(short)64];

		public static StoryScrollItem SectShiXiangBad => Instance[(short)65];

		public static StoryScrollItem SectRanShanGood => Instance[(short)66];

		public static StoryScrollItem SectRanShanBad => Instance[(short)67];

		public static StoryScrollItem SectXuanNvGood => Instance[(short)68];

		public static StoryScrollItem SectXuanNvBad => Instance[(short)69];

		public static StoryScrollItem SectZhuJianGood => Instance[(short)70];

		public static StoryScrollItem SectZhuJianBad => Instance[(short)71];

		public static StoryScrollItem SectKongSangGood => Instance[(short)72];

		public static StoryScrollItem SectKongSangBad => Instance[(short)73];

		public static StoryScrollItem SectJinGangGood => Instance[(short)74];

		public static StoryScrollItem SectJinGangBad => Instance[(short)75];

		public static StoryScrollItem SectWuXianGood => Instance[(short)76];

		public static StoryScrollItem SectWuXianBad => Instance[(short)77];

		public static StoryScrollItem SectJieQingGood => Instance[(short)78];

		public static StoryScrollItem SectJieQingBad => Instance[(short)79];

		public static StoryScrollItem SectFuLongGood => Instance[(short)80];

		public static StoryScrollItem SectFuLongBad => Instance[(short)81];

		public static StoryScrollItem SectXueHouGood => Instance[(short)82];

		public static StoryScrollItem SectXueHouBad => Instance[(short)83];
	}

	public static StoryScroll Instance = new StoryScroll();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "StoryBoss", "StorySect", "TemplateId", "StoryTypeIcon", "StoryResultMark", "StoryImage", "StoryNote" };

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
		_dataArray.Add(new StoryScrollItem(0, 0, -1, "gamelinescroll_icon_big_npcface_2001", 0, "npcface_image_2001_0", 1));
		_dataArray.Add(new StoryScrollItem(1, 0, -1, "gamelinescroll_icon_big_npcface_2001", 2, "npcface_image_2001_1", 3));
		_dataArray.Add(new StoryScrollItem(2, 0, -1, "gamelinescroll_icon_big_npcface_2001", 4, "npcface_image_2001_2", 5));
		_dataArray.Add(new StoryScrollItem(3, 0, -1, "gamelinescroll_icon_big_npcface_2001", 6, "npcface_image_2001_3", 7));
		_dataArray.Add(new StoryScrollItem(4, 0, -1, "gamelinescroll_icon_big_npcface_2001", 8, "npcface_image_2001_4_1", 9));
		_dataArray.Add(new StoryScrollItem(5, 0, -1, "gamelinescroll_icon_big_npcface_2001", 10, "npcface_image_2001_4_0", 11));
		_dataArray.Add(new StoryScrollItem(6, 1, -1, "gamelinescroll_icon_big_npcface_2002", 0, "npcface_image_2002_0", 12));
		_dataArray.Add(new StoryScrollItem(7, 1, -1, "gamelinescroll_icon_big_npcface_2002", 2, "npcface_image_2002_1", 13));
		_dataArray.Add(new StoryScrollItem(8, 1, -1, "gamelinescroll_icon_big_npcface_2002", 4, "npcface_image_2002_2", 14));
		_dataArray.Add(new StoryScrollItem(9, 1, -1, "gamelinescroll_icon_big_npcface_2002", 6, "npcface_image_2002_3", 15));
		_dataArray.Add(new StoryScrollItem(10, 1, -1, "gamelinescroll_icon_big_npcface_2002", 8, "npcface_image_2002_4_1", 16));
		_dataArray.Add(new StoryScrollItem(11, 1, -1, "gamelinescroll_icon_big_npcface_2002", 10, "npcface_image_2002_4_0", 17));
		_dataArray.Add(new StoryScrollItem(12, 2, -1, "gamelinescroll_icon_big_npcface_2003", 0, "npcface_image_2003_0", 18));
		_dataArray.Add(new StoryScrollItem(13, 2, -1, "gamelinescroll_icon_big_npcface_2003", 2, "npcface_image_2003_1", 19));
		_dataArray.Add(new StoryScrollItem(14, 2, -1, "gamelinescroll_icon_big_npcface_2003", 4, "npcface_image_2003_2", 20));
		_dataArray.Add(new StoryScrollItem(15, 2, -1, "gamelinescroll_icon_big_npcface_2003", 6, "npcface_image_2003_3", 21));
		_dataArray.Add(new StoryScrollItem(16, 2, -1, "gamelinescroll_icon_big_npcface_2003", 8, "npcface_image_2003_4_1", 22));
		_dataArray.Add(new StoryScrollItem(17, 2, -1, "gamelinescroll_icon_big_npcface_2003", 10, "npcface_image_2003_4_0", 23));
		_dataArray.Add(new StoryScrollItem(18, 3, -1, "gamelinescroll_icon_big_npcface_2004", 0, "npcface_image_2004_0", 24));
		_dataArray.Add(new StoryScrollItem(19, 3, -1, "gamelinescroll_icon_big_npcface_2004", 2, "npcface_image_2004_1", 25));
		_dataArray.Add(new StoryScrollItem(20, 3, -1, "gamelinescroll_icon_big_npcface_2004", 4, "npcface_image_2004_2", 26));
		_dataArray.Add(new StoryScrollItem(21, 3, -1, "gamelinescroll_icon_big_npcface_2004", 6, "npcface_image_2004_3", 27));
		_dataArray.Add(new StoryScrollItem(22, 3, -1, "gamelinescroll_icon_big_npcface_2004", 8, "npcface_image_2004_4_1", 28));
		_dataArray.Add(new StoryScrollItem(23, 3, -1, "gamelinescroll_icon_big_npcface_2004", 10, "npcface_image_2004_4_0", 29));
		_dataArray.Add(new StoryScrollItem(24, 4, -1, "gamelinescroll_icon_big_npcface_2005", 0, "npcface_image_2005_0", 30));
		_dataArray.Add(new StoryScrollItem(25, 4, -1, "gamelinescroll_icon_big_npcface_2005", 2, "npcface_image_2005_1", 31));
		_dataArray.Add(new StoryScrollItem(26, 4, -1, "gamelinescroll_icon_big_npcface_2005", 4, "npcface_image_2005_2", 32));
		_dataArray.Add(new StoryScrollItem(27, 4, -1, "gamelinescroll_icon_big_npcface_2005", 6, "npcface_image_2005_3", 33));
		_dataArray.Add(new StoryScrollItem(28, 4, -1, "gamelinescroll_icon_big_npcface_2005", 8, "npcface_image_2005_4_1", 34));
		_dataArray.Add(new StoryScrollItem(29, 4, -1, "gamelinescroll_icon_big_npcface_2005", 10, "npcface_image_2005_4_0", 35));
		_dataArray.Add(new StoryScrollItem(30, 5, -1, "gamelinescroll_icon_big_npcface_2006", 0, "npcface_image_2006_0", 36));
		_dataArray.Add(new StoryScrollItem(31, 5, -1, "gamelinescroll_icon_big_npcface_2006", 2, "npcface_image_2006_1", 37));
		_dataArray.Add(new StoryScrollItem(32, 5, -1, "gamelinescroll_icon_big_npcface_2006", 4, "npcface_image_2006_2", 38));
		_dataArray.Add(new StoryScrollItem(33, 5, -1, "gamelinescroll_icon_big_npcface_2006", 6, "npcface_image_2006_3", 39));
		_dataArray.Add(new StoryScrollItem(34, 5, -1, "gamelinescroll_icon_big_npcface_2006", 8, "npcface_image_2006_4_1", 40));
		_dataArray.Add(new StoryScrollItem(35, 5, -1, "gamelinescroll_icon_big_npcface_2006", 10, "npcface_image_2006_4_0", 41));
		_dataArray.Add(new StoryScrollItem(36, 6, -1, "gamelinescroll_icon_big_npcface_2007", 0, "npcface_image_2007_0", 42));
		_dataArray.Add(new StoryScrollItem(37, 6, -1, "gamelinescroll_icon_big_npcface_2007", 2, "npcface_image_2007_1", 43));
		_dataArray.Add(new StoryScrollItem(38, 6, -1, "gamelinescroll_icon_big_npcface_2007", 4, "npcface_image_2007_2", 44));
		_dataArray.Add(new StoryScrollItem(39, 6, -1, "gamelinescroll_icon_big_npcface_2007", 6, "npcface_image_2007_3", 45));
		_dataArray.Add(new StoryScrollItem(40, 6, -1, "gamelinescroll_icon_big_npcface_2007", 8, "npcface_image_2007_4_1", 46));
		_dataArray.Add(new StoryScrollItem(41, 6, -1, "gamelinescroll_icon_big_npcface_2007", 10, "npcface_image_2007_4_0", 47));
		_dataArray.Add(new StoryScrollItem(42, 7, -1, "gamelinescroll_icon_big_npcface_2008", 0, "npcface_image_2008_0", 48));
		_dataArray.Add(new StoryScrollItem(43, 7, -1, "gamelinescroll_icon_big_npcface_2008", 2, "npcface_image_2008_1", 49));
		_dataArray.Add(new StoryScrollItem(44, 7, -1, "gamelinescroll_icon_big_npcface_2008", 4, "npcface_image_2008_2", 50));
		_dataArray.Add(new StoryScrollItem(45, 7, -1, "gamelinescroll_icon_big_npcface_2008", 6, "npcface_image_2008_3", 51));
		_dataArray.Add(new StoryScrollItem(46, 7, -1, "gamelinescroll_icon_big_npcface_2008", 8, "npcface_image_2008_4_1", 52));
		_dataArray.Add(new StoryScrollItem(47, 7, -1, "gamelinescroll_icon_big_npcface_2008", 10, "npcface_image_2008_4_0", 53));
		_dataArray.Add(new StoryScrollItem(48, 8, -1, "gamelinescroll_icon_big_npcface_2009", 0, "npcface_image_2009_0", 54));
		_dataArray.Add(new StoryScrollItem(49, 8, -1, "gamelinescroll_icon_big_npcface_2009", 2, "npcface_image_2009_1", 55));
		_dataArray.Add(new StoryScrollItem(50, 8, -1, "gamelinescroll_icon_big_npcface_2009", 4, "npcface_image_2009_2", 56));
		_dataArray.Add(new StoryScrollItem(51, 8, -1, "gamelinescroll_icon_big_npcface_2009", 6, "npcface_image_2009_3", 57));
		_dataArray.Add(new StoryScrollItem(52, 8, -1, "gamelinescroll_icon_big_npcface_2009", 8, "npcface_image_2009_4_1", 58));
		_dataArray.Add(new StoryScrollItem(53, 8, -1, "gamelinescroll_icon_big_npcface_2009", 10, "npcface_image_2009_4_0", 59));
		_dataArray.Add(new StoryScrollItem(54, -1, 1, "settlementInformation_icon_sect_1", 60, "sectstory_image_1_0", 61));
		_dataArray.Add(new StoryScrollItem(55, -1, 1, "settlementInformation_icon_sect_1", 62, "sectstory_image_1_1", 63));
		_dataArray.Add(new StoryScrollItem(56, -1, 2, "settlementInformation_icon_sect_2", 60, "sectstory_image_2_0", 64));
		_dataArray.Add(new StoryScrollItem(57, -1, 2, "settlementInformation_icon_sect_2", 62, "sectstory_image_2_1", 65));
		_dataArray.Add(new StoryScrollItem(58, -1, 3, "settlementInformation_icon_sect_3", 60, "sectstory_image_3_0", 66));
		_dataArray.Add(new StoryScrollItem(59, -1, 3, "settlementInformation_icon_sect_3", 62, "sectstory_image_3_1", 67));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new StoryScrollItem(60, -1, 4, "settlementInformation_icon_sect_4", 60, "sectstory_image_4_0", 68));
		_dataArray.Add(new StoryScrollItem(61, -1, 4, "settlementInformation_icon_sect_4", 62, "sectstory_image_4_1", 69));
		_dataArray.Add(new StoryScrollItem(62, -1, 5, "settlementInformation_icon_sect_5", 60, "sectstory_image_5_0", 70));
		_dataArray.Add(new StoryScrollItem(63, -1, 5, "settlementInformation_icon_sect_5", 62, "sectstory_image_5_1", 71));
		_dataArray.Add(new StoryScrollItem(64, -1, 6, "settlementInformation_icon_sect_6", 60, "sectstory_image_6_0", 72));
		_dataArray.Add(new StoryScrollItem(65, -1, 6, "settlementInformation_icon_sect_6", 62, "sectstory_image_6_1", 73));
		_dataArray.Add(new StoryScrollItem(66, -1, 7, "settlementInformation_icon_sect_7", 60, "sectstory_image_7_0", 74));
		_dataArray.Add(new StoryScrollItem(67, -1, 7, "settlementInformation_icon_sect_7", 62, "sectstory_image_7_1", 75));
		_dataArray.Add(new StoryScrollItem(68, -1, 8, "settlementInformation_icon_sect_8", 60, "sectstory_image_8_0", 76));
		_dataArray.Add(new StoryScrollItem(69, -1, 8, "settlementInformation_icon_sect_8", 62, "sectstory_image_8_1", 77));
		_dataArray.Add(new StoryScrollItem(70, -1, 9, "settlementInformation_icon_sect_9", 60, "sectstory_image_9_0", 78));
		_dataArray.Add(new StoryScrollItem(71, -1, 9, "settlementInformation_icon_sect_9", 62, "sectstory_image_9_1", 79));
		_dataArray.Add(new StoryScrollItem(72, -1, 10, "settlementInformation_icon_sect_10", 60, "sectstory_image_10_0", 80));
		_dataArray.Add(new StoryScrollItem(73, -1, 10, "settlementInformation_icon_sect_10", 62, "sectstory_image_10_1", 81));
		_dataArray.Add(new StoryScrollItem(74, -1, 11, "settlementInformation_icon_sect_11", 60, "sectstory_image_11_0", 82));
		_dataArray.Add(new StoryScrollItem(75, -1, 11, "settlementInformation_icon_sect_11", 62, "sectstory_image_11_1", 83));
		_dataArray.Add(new StoryScrollItem(76, -1, 12, "settlementInformation_icon_sect_12", 60, "sectstory_image_12_0", 84));
		_dataArray.Add(new StoryScrollItem(77, -1, 12, "settlementInformation_icon_sect_12", 62, "sectstory_image_12_1", 85));
		_dataArray.Add(new StoryScrollItem(78, -1, 13, null, 86, null, 87));
		_dataArray.Add(new StoryScrollItem(79, -1, 13, null, 88, null, 89));
		_dataArray.Add(new StoryScrollItem(80, -1, 14, "settlementInformation_icon_sect_14", 60, "sectstory_image_14_0", 90));
		_dataArray.Add(new StoryScrollItem(81, -1, 14, "settlementInformation_icon_sect_14", 62, "sectstory_image_14_1", 91));
		_dataArray.Add(new StoryScrollItem(82, -1, 15, "settlementInformation_icon_sect_15", 60, "sectstory_image_15_0", 92));
		_dataArray.Add(new StoryScrollItem(83, -1, 15, "settlementInformation_icon_sect_15", 62, "sectstory_image_15_1", 93));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<StoryScrollItem>(84);
		CreateItems0();
		CreateItems1();
	}
}
