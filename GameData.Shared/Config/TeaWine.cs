using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TeaWine : ConfigData<TeaWineItem, short>
{
	public static class DefKey
	{
		public const short WineOuter0 = 0;

		public const short WineOuter1 = 1;

		public const short WineOuter2 = 2;

		public const short WineOuter3 = 3;

		public const short WineOuter4 = 4;

		public const short WineOuter5 = 5;

		public const short WineOuter6 = 6;

		public const short WineOuter7 = 7;

		public const short WineOuter8 = 8;

		public const short WineInner0 = 9;

		public const short WineInner1 = 10;

		public const short WineInner2 = 11;

		public const short WineInner3 = 12;

		public const short WineInner4 = 13;

		public const short WineInner5 = 14;

		public const short WineInner6 = 15;

		public const short WineInner7 = 16;

		public const short WineInner8 = 17;

		public const short TeaOuter0 = 18;

		public const short TeaOuter1 = 19;

		public const short TeaOuter2 = 20;

		public const short TeaOuter3 = 21;

		public const short TeaOuter4 = 22;

		public const short TeaOuter5 = 23;

		public const short TeaOuter6 = 24;

		public const short TeaOuter7 = 25;

		public const short TeaOuter8 = 26;

		public const short TeaInner0 = 27;

		public const short TeaInner1 = 28;

		public const short TeaInner2 = 29;

		public const short TeaInner3 = 30;

		public const short TeaInner4 = 31;

		public const short TeaInner5 = 32;

		public const short TeaInner6 = 33;

		public const short TeaInner7 = 34;

		public const short TeaInner8 = 35;
	}

	public static class DefValue
	{
		public static TeaWineItem WineOuter0 => Instance[(short)0];

		public static TeaWineItem WineOuter1 => Instance[(short)1];

		public static TeaWineItem WineOuter2 => Instance[(short)2];

		public static TeaWineItem WineOuter3 => Instance[(short)3];

		public static TeaWineItem WineOuter4 => Instance[(short)4];

		public static TeaWineItem WineOuter5 => Instance[(short)5];

		public static TeaWineItem WineOuter6 => Instance[(short)6];

		public static TeaWineItem WineOuter7 => Instance[(short)7];

		public static TeaWineItem WineOuter8 => Instance[(short)8];

		public static TeaWineItem WineInner0 => Instance[(short)9];

		public static TeaWineItem WineInner1 => Instance[(short)10];

		public static TeaWineItem WineInner2 => Instance[(short)11];

		public static TeaWineItem WineInner3 => Instance[(short)12];

		public static TeaWineItem WineInner4 => Instance[(short)13];

		public static TeaWineItem WineInner5 => Instance[(short)14];

		public static TeaWineItem WineInner6 => Instance[(short)15];

		public static TeaWineItem WineInner7 => Instance[(short)16];

		public static TeaWineItem WineInner8 => Instance[(short)17];

		public static TeaWineItem TeaOuter0 => Instance[(short)18];

		public static TeaWineItem TeaOuter1 => Instance[(short)19];

		public static TeaWineItem TeaOuter2 => Instance[(short)20];

		public static TeaWineItem TeaOuter3 => Instance[(short)21];

		public static TeaWineItem TeaOuter4 => Instance[(short)22];

		public static TeaWineItem TeaOuter5 => Instance[(short)23];

		public static TeaWineItem TeaOuter6 => Instance[(short)24];

		public static TeaWineItem TeaOuter7 => Instance[(short)25];

		public static TeaWineItem TeaOuter8 => Instance[(short)26];

		public static TeaWineItem TeaInner0 => Instance[(short)27];

		public static TeaWineItem TeaInner1 => Instance[(short)28];

		public static TeaWineItem TeaInner2 => Instance[(short)29];

		public static TeaWineItem TeaInner3 => Instance[(short)30];

		public static TeaWineItem TeaInner4 => Instance[(short)31];

		public static TeaWineItem TeaInner5 => Instance[(short)32];

		public static TeaWineItem TeaInner6 => Instance[(short)33];

		public static TeaWineItem TeaInner7 => Instance[(short)34];

		public static TeaWineItem TeaInner8 => Instance[(short)35];
	}

	public static TeaWine Instance = new TeaWine();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"ItemSubType", "GroupId", "ResourceType", "BreakBonusEffect", "TemplateId", "Name", "Grade", "Icon", "BigIcon", "Desc",
		"BaseWeight", "BaseHappinessChange", "DropRate"
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
		_dataArray.Add(new TeaWineItem(0, 0, 9, 901, 0, 0, "icon_TeaWine_guanwailaojiu", "bigIcon_TeaWine_guanwailaojiu", 1, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 100, 150, 0, 2, 600, 3, allowRandomCreate: true, 50, isSpecial: false, 0, 36, 39, 1, 400, 1, 60, 0, 0, 0, 0, 50, 50, 0, 0, 0, 0, -50, -50, 20, 0, 0, 8, 1));
		_dataArray.Add(new TeaWineItem(1, 2, 9, 901, 1, 0, "icon_TeaWine_guanwailaojiu", "bigIcon_TeaWine_guanwailaojiu", 3, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 200, 300, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 36, 39, 1, 600, 1, 60, 0, 0, 0, 0, 55, 55, 0, 0, 0, 0, -55, -55, 25, 0, 0, 12, 1));
		_dataArray.Add(new TeaWineItem(2, 4, 9, 901, 2, 0, "icon_TeaWine_guanwailaojiu", "bigIcon_TeaWine_guanwailaojiu", 5, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 600, 900, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 36, 39, 1, 800, 1, 60, 0, 0, 0, 0, 60, 60, 0, 0, 0, 0, -60, -60, 30, 0, 0, 16, 1));
		_dataArray.Add(new TeaWineItem(3, 6, 9, 901, 3, 0, "icon_TeaWine_huadiaojiu", "bigIcon_TeaWine_huadiaojiu", 7, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 1500, 2250, 1, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 36, 39, 1, 1000, 1, 60, 0, 0, 0, 0, 70, 70, 0, 0, 0, 0, -70, -70, 35, 0, 0, 24, 1));
		_dataArray.Add(new TeaWineItem(4, 8, 9, 901, 4, 0, "icon_TeaWine_huadiaojiu", "bigIcon_TeaWine_huadiaojiu", 9, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 3100, 4650, 2, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 36, 39, 1, 1200, 1, 60, 0, 0, 0, 0, 80, 80, 0, 0, 0, 0, -80, -80, 40, 0, 0, 32, 1));
		_dataArray.Add(new TeaWineItem(5, 10, 9, 901, 5, 0, "icon_TeaWine_huadiaojiu", "bigIcon_TeaWine_huadiaojiu", 11, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 5600, 8400, 3, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 36, 39, 1, 1400, 1, 60, 0, 0, 0, 0, 90, 90, 0, 0, 0, 0, -90, -90, 45, 0, 0, 40, 1));
		_dataArray.Add(new TeaWineItem(6, 12, 9, 901, 6, 0, "icon_TeaWine_jiannanshaochun", "bigIcon_TeaWine_jiannanshaochun", 13, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 9200, 13800, 4, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 36, 39, 1, 1600, 1, 60, 0, 0, 0, 0, 105, 105, 0, 0, 0, 0, -105, -105, 50, 0, 0, 52, 1));
		_dataArray.Add(new TeaWineItem(7, 14, 9, 901, 7, 0, "icon_TeaWine_maotaibaijiu", "bigIcon_TeaWine_maotaibaijiu", 15, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 14100, 21150, 5, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 36, 39, 1, 1800, 1, 60, 0, 0, 0, 0, 125, 125, 0, 0, 0, 0, -125, -125, 55, 0, 0, 68, 1));
		_dataArray.Add(new TeaWineItem(8, 16, 9, 901, 8, 0, "icon_TeaWine_houerjiu", "bigIcon_TeaWine_houerjiu", 17, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 20500, 30750, 6, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 36, 39, 1, 2000, 1, 60, 0, 0, 0, 0, 150, 150, 0, 0, 0, 0, -150, -150, 60, 0, 0, 88, 1));
		_dataArray.Add(new TeaWineItem(9, 18, 9, 901, 0, 9, "icon_TeaWine_gaoliangjiu", "bigIcon_TeaWine_gaoliangjiu", 19, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 100, 150, 0, 2, 600, 3, allowRandomCreate: true, 50, isSpecial: false, 0, 36, 39, 1, 400, 1, 60, 50, 50, 50, 50, 0, 0, -50, -50, -50, -50, 0, 0, 20, 0, 0, 8, 0));
		_dataArray.Add(new TeaWineItem(10, 20, 9, 901, 1, 9, "icon_TeaWine_gaoliangjiu", "bigIcon_TeaWine_gaoliangjiu", 21, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 200, 300, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 36, 39, 1, 600, 1, 60, 55, 55, 55, 55, 0, 0, -55, -55, -55, -55, 0, 0, 25, 0, 0, 12, 0));
		_dataArray.Add(new TeaWineItem(11, 22, 9, 901, 2, 9, "icon_TeaWine_gaoliangjiu", "bigIcon_TeaWine_gaoliangjiu", 23, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 600, 900, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 36, 39, 1, 800, 1, 60, 60, 60, 60, 60, 0, 0, -60, -60, -60, -60, 0, 0, 30, 0, 0, 16, 0));
		_dataArray.Add(new TeaWineItem(12, 24, 9, 901, 3, 9, "icon_TeaWine_xiyuputaojiu", "bigIcon_TeaWine_xiyuputaojiu", 25, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 1500, 2250, 1, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 36, 39, 1, 1000, 1, 60, 70, 70, 70, 70, 0, 0, -70, -70, -70, -70, 0, 0, 35, 0, 0, 24, 0));
		_dataArray.Add(new TeaWineItem(13, 26, 9, 901, 4, 9, "icon_TeaWine_xiyuputaojiu", "bigIcon_TeaWine_xiyuputaojiu", 27, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 3100, 4650, 2, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 36, 39, 1, 1200, 1, 60, 80, 80, 80, 80, 0, 0, -80, -80, -80, -80, 0, 0, 40, 0, 0, 32, 0));
		_dataArray.Add(new TeaWineItem(14, 28, 9, 901, 5, 9, "icon_TeaWine_xiyuputaojiu", "bigIcon_TeaWine_xiyuputaojiu", 29, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 5600, 8400, 3, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 36, 39, 1, 1400, 1, 60, 90, 90, 90, 90, 0, 0, -90, -90, -90, -90, 0, 0, 45, 0, 0, 40, 0));
		_dataArray.Add(new TeaWineItem(15, 30, 9, 901, 6, 9, "icon_TeaWine_xinghuafenqing", "bigIcon_TeaWine_xinghuafenqing", 31, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 9200, 13800, 4, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 36, 39, 1, 1600, 1, 60, 105, 105, 105, 105, 0, 0, -105, -105, -105, -105, 0, 0, 50, 0, 0, 52, 0));
		_dataArray.Add(new TeaWineItem(16, 32, 9, 901, 7, 9, "icon_TeaWine_lanlingmeijiu", "bigIcon_TeaWine_lanlingmeijiu", 33, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 14100, 21150, 5, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 36, 39, 1, 1800, 1, 60, 125, 125, 125, 125, 0, 0, -125, -125, -125, -125, 0, 0, 55, 0, 0, 68, 0));
		_dataArray.Add(new TeaWineItem(17, 34, 9, 901, 8, 9, "icon_TeaWine_dukangjiu", "bigIcon_TeaWine_dukangjiu", 35, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 100, 20500, 30750, 6, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 36, 39, 1, 2000, 1, 60, 150, 150, 150, 150, 0, 0, -150, -150, -150, -150, 0, 0, 60, 0, 0, 88, 0));
		_dataArray.Add(new TeaWineItem(18, 36, 9, 900, 0, 18, "icon_TeaWine_puercha", "bigIcon_TeaWine_puercha", 37, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 100, 150, 0, 2, 600, 3, allowRandomCreate: true, 50, isSpecial: false, 0, 36, 36, 1, -400, 1, 60, 0, 0, 0, 0, -50, -50, 0, 0, 0, 0, 50, 50, 0, 20, 20, 8, -1));
		_dataArray.Add(new TeaWineItem(19, 38, 9, 900, 1, 18, "icon_TeaWine_puercha", "bigIcon_TeaWine_puercha", 39, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 200, 300, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 36, 36, 1, -600, 1, 60, 0, 0, 0, 0, -55, -55, 0, 0, 0, 0, 55, 55, 0, 25, 40, 12, -1));
		_dataArray.Add(new TeaWineItem(20, 40, 9, 900, 2, 18, "icon_TeaWine_puercha", "bigIcon_TeaWine_puercha", 41, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 600, 900, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 36, 36, 1, -800, 1, 60, 0, 0, 0, 0, -60, -60, 0, 0, 0, 0, 60, 60, 0, 30, 60, 16, -1));
		_dataArray.Add(new TeaWineItem(21, 42, 9, 900, 3, 18, "icon_TeaWine_junshanyinzhen", "bigIcon_TeaWine_junshanyinzhen", 43, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 1500, 2250, 1, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 36, 36, 1, -1000, 1, 60, 0, 0, 0, 0, -70, -70, 0, 0, 0, 0, 70, 70, 0, 35, 90, 24, -1));
		_dataArray.Add(new TeaWineItem(22, 44, 9, 900, 4, 18, "icon_TeaWine_junshanyinzhen", "bigIcon_TeaWine_junshanyinzhen", 45, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 3100, 4650, 2, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 36, 36, 1, -1200, 1, 60, 0, 0, 0, 0, -80, -80, 0, 0, 0, 0, 80, 80, 0, 40, 120, 32, -1));
		_dataArray.Add(new TeaWineItem(23, 46, 9, 900, 5, 18, "icon_TeaWine_junshanyinzhen", "bigIcon_TeaWine_junshanyinzhen", 47, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 5600, 8400, 3, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 36, 36, 1, -1400, 1, 60, 0, 0, 0, 0, -90, -90, 0, 0, 0, 0, 90, 90, 0, 45, 150, 40, -1));
		_dataArray.Add(new TeaWineItem(24, 48, 9, 900, 6, 18, "icon_TeaWine_guzhuzisun", "bigIcon_TeaWine_guzhuzisun", 49, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 9200, 13800, 4, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 36, 36, 1, -1600, 1, 60, 0, 0, 0, 0, -105, -105, 0, 0, 0, 0, 105, 105, 0, 50, 190, 52, -1));
		_dataArray.Add(new TeaWineItem(25, 50, 9, 900, 7, 18, "icon_TeaWine_dahongpao", "bigIcon_TeaWine_dahongpao", 51, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 14100, 21150, 5, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 36, 36, 1, -1800, 1, 60, 0, 0, 0, 0, -125, -125, 0, 0, 0, 0, 125, 125, 0, 55, 240, 68, -1));
		_dataArray.Add(new TeaWineItem(26, 52, 9, 900, 8, 18, "icon_TeaWine_mengdinghuangya", "bigIcon_TeaWine_mengdinghuangya", 53, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 20500, 30750, 6, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 36, 36, 1, -2000, 1, 60, 0, 0, 0, 0, -150, -150, 0, 0, 0, 0, 150, 150, 0, 60, 300, 88, -1));
		_dataArray.Add(new TeaWineItem(27, 54, 9, 900, 0, 27, "icon_TeaWine_zhuyeqingcha", "bigIcon_TeaWine_zhuyeqingcha", 55, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 100, 150, 0, 2, 600, 3, allowRandomCreate: true, 50, isSpecial: false, 0, 36, 36, 1, -400, 1, 60, -50, -50, -50, -50, 0, 0, 50, 50, 50, 50, 0, 0, 0, 20, 20, 8, -1));
		_dataArray.Add(new TeaWineItem(28, 56, 9, 900, 1, 27, "icon_TeaWine_zhuyeqingcha", "bigIcon_TeaWine_zhuyeqingcha", 57, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 200, 300, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 36, 36, 1, -600, 1, 60, -55, -55, -55, -55, 0, 0, 55, 55, 55, 55, 0, 0, 0, 25, 40, 12, -1));
		_dataArray.Add(new TeaWineItem(29, 58, 9, 900, 2, 27, "icon_TeaWine_zhuyeqingcha", "bigIcon_TeaWine_zhuyeqingcha", 59, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 600, 900, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 36, 36, 1, -800, 1, 60, -60, -60, -60, -60, 0, 0, 60, 60, 60, 60, 0, 0, 0, 30, 60, 16, -1));
		_dataArray.Add(new TeaWineItem(30, 60, 9, 900, 3, 27, "icon_TeaWine_xinyangmaojian", "bigIcon_TeaWine_xinyangmaojian", 61, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 1500, 2250, 1, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 36, 36, 1, -1000, 1, 60, -70, -70, -70, -70, 0, 0, 70, 70, 70, 70, 0, 0, 0, 35, 90, 24, -1));
		_dataArray.Add(new TeaWineItem(31, 62, 9, 900, 4, 27, "icon_TeaWine_xinyangmaojian", "bigIcon_TeaWine_xinyangmaojian", 63, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 3100, 4650, 2, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 36, 36, 1, -1200, 1, 60, -80, -80, -80, -80, 0, 0, 80, 80, 80, 80, 0, 0, 0, 40, 120, 32, -1));
		_dataArray.Add(new TeaWineItem(32, 64, 9, 900, 5, 27, "icon_TeaWine_xinyangmaojian", "bigIcon_TeaWine_xinyangmaojian", 65, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 5600, 8400, 3, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 36, 36, 1, -1400, 1, 60, -90, -90, -90, -90, 0, 0, 90, 90, 90, 90, 0, 0, 0, 45, 150, 40, -1));
		_dataArray.Add(new TeaWineItem(33, 66, 9, 900, 6, 27, "icon_TeaWine_xihulongjingcha", "bigIcon_TeaWine_xihulongjingcha", 67, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 9200, 13800, 4, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 36, 36, 1, -1600, 1, 60, -105, -105, -105, -105, 0, 0, 105, 105, 105, 105, 0, 0, 0, 50, 190, 52, -1));
		_dataArray.Add(new TeaWineItem(34, 68, 9, 900, 7, 27, "icon_TeaWine_fangshanluya", "bigIcon_TeaWine_fangshanluya", 69, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 14100, 21150, 5, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 36, 36, 1, -1800, 1, 60, -125, -125, -125, -125, 0, 0, 125, 125, 125, 125, 0, 0, 0, 55, 240, 68, -1));
		_dataArray.Add(new TeaWineItem(35, 70, 9, 900, 8, 27, "icon_TeaWine_mengdingganlu", "bigIcon_TeaWine_mengdingganlu", 71, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 20500, 30750, 6, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 36, 36, 1, -2000, 1, 60, -150, -150, -150, -150, 0, 0, 150, 150, 150, 150, 0, 0, 0, 60, 300, 88, -1));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TeaWineItem>(36);
		CreateItems0();
	}

	public static int GetCharacterPropertyBonus(int key, ECharacterPropertyReferencedType property)
	{
		return Instance._dataArray[key].GetCharacterPropertyBonusInt(property);
	}

	public static int GetCharacterPropertyBonus(short[] keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int num2 = keys.Length; i < num2; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}

	public static int GetCharacterPropertyBonus(List<short> keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int count = keys.Count; i < count; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}

	public static int GetCharacterPropertyBonus(int[] keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int num2 = keys.Length; i < num2; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}

	public static int GetCharacterPropertyBonus(List<int> keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int count = keys.Count; i < count; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}
}
