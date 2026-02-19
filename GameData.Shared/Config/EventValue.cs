using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class EventValue : ConfigData<EventValueItem, int>
{
	public static class DefKey
	{
		public const int Taiwu = 0;

		public const int CurrDate = 1;

		public const int CurrMonth = 2;

		public const int CurrYear = 3;

		public const int LeftDaysInCurrMonth = 4;

		public const int XiangshuLevel = 5;

		public const int Shaolin = 7;

		public const int Emei = 8;

		public const int Baihua = 9;

		public const int Wudang = 10;

		public const int Yuanshan = 11;

		public const int Shixiang = 12;

		public const int Ranshan = 13;

		public const int Xuannv = 14;

		public const int Zhujian = 15;

		public const int Kongsang = 16;

		public const int Jingang = 17;

		public const int Wuxian = 18;

		public const int Jieqing = 19;

		public const int Fulong = 20;

		public const int Xuehou = 21;

		public const int TaiwuVillage = 22;

		public const int Jingcheng = 23;

		public const int Chengdu = 24;

		public const int Guizhou = 25;

		public const int Xiangyang = 26;

		public const int Taiyuan = 27;

		public const int Guangzhou = 28;

		public const int Qingzhou = 29;

		public const int Jiangling = 30;

		public const int Fuzhou = 31;

		public const int Liaoyang = 32;

		public const int Qinzhou = 33;

		public const int Dali = 34;

		public const int Shouchun = 35;

		public const int Hangzhou = 36;

		public const int Yangzhou = 37;

		public const int InteractingCharacter = 6;
	}

	public static class DefValue
	{
		public static EventValueItem Taiwu => Instance[0];

		public static EventValueItem CurrDate => Instance[1];

		public static EventValueItem CurrMonth => Instance[2];

		public static EventValueItem CurrYear => Instance[3];

		public static EventValueItem LeftDaysInCurrMonth => Instance[4];

		public static EventValueItem XiangshuLevel => Instance[5];

		public static EventValueItem Shaolin => Instance[7];

		public static EventValueItem Emei => Instance[8];

		public static EventValueItem Baihua => Instance[9];

		public static EventValueItem Wudang => Instance[10];

		public static EventValueItem Yuanshan => Instance[11];

		public static EventValueItem Shixiang => Instance[12];

		public static EventValueItem Ranshan => Instance[13];

		public static EventValueItem Xuannv => Instance[14];

		public static EventValueItem Zhujian => Instance[15];

		public static EventValueItem Kongsang => Instance[16];

		public static EventValueItem Jingang => Instance[17];

		public static EventValueItem Wuxian => Instance[18];

		public static EventValueItem Jieqing => Instance[19];

		public static EventValueItem Fulong => Instance[20];

		public static EventValueItem Xuehou => Instance[21];

		public static EventValueItem TaiwuVillage => Instance[22];

		public static EventValueItem Jingcheng => Instance[23];

		public static EventValueItem Chengdu => Instance[24];

		public static EventValueItem Guizhou => Instance[25];

		public static EventValueItem Xiangyang => Instance[26];

		public static EventValueItem Taiyuan => Instance[27];

		public static EventValueItem Guangzhou => Instance[28];

		public static EventValueItem Qingzhou => Instance[29];

		public static EventValueItem Jiangling => Instance[30];

		public static EventValueItem Fuzhou => Instance[31];

		public static EventValueItem Liaoyang => Instance[32];

		public static EventValueItem Qinzhou => Instance[33];

		public static EventValueItem Dali => Instance[34];

		public static EventValueItem Shouchun => Instance[35];

		public static EventValueItem Hangzhou => Instance[36];

		public static EventValueItem Yangzhou => Instance[37];

		public static EventValueItem InteractingCharacter => Instance[6];
	}

	public static EventValue Instance = new EventValue();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "EventArgument", "TemplateId", "Name", "ArgBoxKey", "Alias" };

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
		_dataArray.Add(new EventValueItem(0, EEventValueType.Global, 0, null, 6, "Taiwu"));
		_dataArray.Add(new EventValueItem(1, EEventValueType.Global, 1, null, 1, "CurrDate"));
		_dataArray.Add(new EventValueItem(2, EEventValueType.Global, 2, null, 1, "CurrMonth"));
		_dataArray.Add(new EventValueItem(3, EEventValueType.Global, 3, null, 1, "CurrYear"));
		_dataArray.Add(new EventValueItem(4, EEventValueType.Global, 4, null, 1, "LeftDaysInCurrMonth"));
		_dataArray.Add(new EventValueItem(5, EEventValueType.Global, 5, null, 1, "XiangshuLevel"));
		_dataArray.Add(new EventValueItem(6, EEventValueType.Event, 37, "CharacterId", 6, null));
		_dataArray.Add(new EventValueItem(7, EEventValueType.Global, 6, null, 10, "Shaolin"));
		_dataArray.Add(new EventValueItem(8, EEventValueType.Global, 7, null, 10, "Emei"));
		_dataArray.Add(new EventValueItem(9, EEventValueType.Global, 8, null, 10, "Baihua"));
		_dataArray.Add(new EventValueItem(10, EEventValueType.Global, 9, null, 10, "Wudang"));
		_dataArray.Add(new EventValueItem(11, EEventValueType.Global, 10, null, 10, "Yuanshan"));
		_dataArray.Add(new EventValueItem(12, EEventValueType.Global, 11, null, 10, "Shixiang"));
		_dataArray.Add(new EventValueItem(13, EEventValueType.Global, 12, null, 10, "Ranshan"));
		_dataArray.Add(new EventValueItem(14, EEventValueType.Global, 13, null, 10, "Xuannv"));
		_dataArray.Add(new EventValueItem(15, EEventValueType.Global, 14, null, 10, "Zhujian"));
		_dataArray.Add(new EventValueItem(16, EEventValueType.Global, 15, null, 10, "Kongsang"));
		_dataArray.Add(new EventValueItem(17, EEventValueType.Global, 16, null, 10, "Jingang"));
		_dataArray.Add(new EventValueItem(18, EEventValueType.Global, 17, null, 10, "Wuxian"));
		_dataArray.Add(new EventValueItem(19, EEventValueType.Global, 18, null, 10, "Jieqing"));
		_dataArray.Add(new EventValueItem(20, EEventValueType.Global, 19, null, 10, "Fulong"));
		_dataArray.Add(new EventValueItem(21, EEventValueType.Global, 20, null, 10, "Xuehou"));
		_dataArray.Add(new EventValueItem(22, EEventValueType.Global, 21, null, 10, "TaiwuVillage"));
		_dataArray.Add(new EventValueItem(23, EEventValueType.Global, 22, null, 10, "Jingcheng"));
		_dataArray.Add(new EventValueItem(24, EEventValueType.Global, 23, null, 10, "Chengdu"));
		_dataArray.Add(new EventValueItem(25, EEventValueType.Global, 24, null, 10, "Guizhou"));
		_dataArray.Add(new EventValueItem(26, EEventValueType.Global, 25, null, 10, "Xiangyang"));
		_dataArray.Add(new EventValueItem(27, EEventValueType.Global, 26, null, 10, "Taiyuan"));
		_dataArray.Add(new EventValueItem(28, EEventValueType.Global, 27, null, 10, "Guangzhou"));
		_dataArray.Add(new EventValueItem(29, EEventValueType.Global, 28, null, 10, "Qingzhou"));
		_dataArray.Add(new EventValueItem(30, EEventValueType.Global, 29, null, 10, "Jiangling"));
		_dataArray.Add(new EventValueItem(31, EEventValueType.Global, 30, null, 10, "Fuzhou"));
		_dataArray.Add(new EventValueItem(32, EEventValueType.Global, 31, null, 10, "Liaoyang"));
		_dataArray.Add(new EventValueItem(33, EEventValueType.Global, 32, null, 10, "Qinzhou"));
		_dataArray.Add(new EventValueItem(34, EEventValueType.Global, 33, null, 10, "Dali"));
		_dataArray.Add(new EventValueItem(35, EEventValueType.Global, 34, null, 10, "Shouchun"));
		_dataArray.Add(new EventValueItem(36, EEventValueType.Global, 35, null, 10, "Hangzhou"));
		_dataArray.Add(new EventValueItem(37, EEventValueType.Global, 36, null, 10, "Yangzhou"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<EventValueItem>(38);
		CreateItems0();
	}
}
