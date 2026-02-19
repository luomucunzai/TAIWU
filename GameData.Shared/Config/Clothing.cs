using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class Clothing : ConfigData<ClothingItem, short>
{
	public static class DefKey
	{
		public const short GeneralCombat0 = 0;

		public const short GeneralCombat1 = 1;

		public const short GeneralCombat2 = 2;

		public const short GeneralCombat3 = 3;

		public const short GeneralCombat4 = 4;

		public const short GeneralCombat5 = 5;

		public const short GeneralCombat6 = 6;

		public const short GeneralCombat7 = 7;

		public const short GeneralCombat8 = 8;

		public const short GeneralLife0 = 9;

		public const short GeneralLife1 = 10;

		public const short GeneralLife2 = 11;

		public const short GeneralLife3 = 12;

		public const short GeneralLife4 = 13;

		public const short GeneralLife5 = 14;

		public const short GeneralLife6 = 15;

		public const short GeneralLife7 = 16;

		public const short GeneralLife8 = 17;

		public const short Wudang4 = 30;

		public const short Ranshan1 = 37;

		public const short BabyClothing = 64;

		public const short ChildClothing = 65;

		public const short xiangshuMinion1 = 66;

		public const short xiangshuMinion2 = 67;

		public const short xiangshuMinion3 = 68;

		public const short SkeletonLow = 69;

		public const short SkeletonMid = 70;

		public const short SkeletonHigh = 71;

		public const short Bug = 72;

		public const short Tutorial = 73;

		public const short ISBNCloth = 74;

		public const short DLCJiaoWhite = 75;

		public const short DLCJiaoBlack = 76;

		public const short DLCJiaoGreen = 77;

		public const short DLCJiaoRed = 78;

		public const short DLCJiaoYellow = 79;

		public const short DLCChineseNewYear = 80;

		public const short DLCYearOfSnakeCloth = 92;

		public const short DLCYearOfSnakeClothBlue = 93;

		public const short DLCYearOfSnakeClothYellow = 94;

		public const short DLCYearOfHorseCloth = 95;
	}

	public static class DefValue
	{
		public static ClothingItem GeneralCombat0 => Instance[(short)0];

		public static ClothingItem GeneralCombat1 => Instance[(short)1];

		public static ClothingItem GeneralCombat2 => Instance[(short)2];

		public static ClothingItem GeneralCombat3 => Instance[(short)3];

		public static ClothingItem GeneralCombat4 => Instance[(short)4];

		public static ClothingItem GeneralCombat5 => Instance[(short)5];

		public static ClothingItem GeneralCombat6 => Instance[(short)6];

		public static ClothingItem GeneralCombat7 => Instance[(short)7];

		public static ClothingItem GeneralCombat8 => Instance[(short)8];

		public static ClothingItem GeneralLife0 => Instance[(short)9];

		public static ClothingItem GeneralLife1 => Instance[(short)10];

		public static ClothingItem GeneralLife2 => Instance[(short)11];

		public static ClothingItem GeneralLife3 => Instance[(short)12];

		public static ClothingItem GeneralLife4 => Instance[(short)13];

		public static ClothingItem GeneralLife5 => Instance[(short)14];

		public static ClothingItem GeneralLife6 => Instance[(short)15];

		public static ClothingItem GeneralLife7 => Instance[(short)16];

		public static ClothingItem GeneralLife8 => Instance[(short)17];

		public static ClothingItem Wudang4 => Instance[(short)30];

		public static ClothingItem Ranshan1 => Instance[(short)37];

		public static ClothingItem BabyClothing => Instance[(short)64];

		public static ClothingItem ChildClothing => Instance[(short)65];

		public static ClothingItem xiangshuMinion1 => Instance[(short)66];

		public static ClothingItem xiangshuMinion2 => Instance[(short)67];

		public static ClothingItem xiangshuMinion3 => Instance[(short)68];

		public static ClothingItem SkeletonLow => Instance[(short)69];

		public static ClothingItem SkeletonMid => Instance[(short)70];

		public static ClothingItem SkeletonHigh => Instance[(short)71];

		public static ClothingItem Bug => Instance[(short)72];

		public static ClothingItem Tutorial => Instance[(short)73];

		public static ClothingItem ISBNCloth => Instance[(short)74];

		public static ClothingItem DLCJiaoWhite => Instance[(short)75];

		public static ClothingItem DLCJiaoBlack => Instance[(short)76];

		public static ClothingItem DLCJiaoGreen => Instance[(short)77];

		public static ClothingItem DLCJiaoRed => Instance[(short)78];

		public static ClothingItem DLCJiaoYellow => Instance[(short)79];

		public static ClothingItem DLCChineseNewYear => Instance[(short)80];

		public static ClothingItem DLCYearOfSnakeCloth => Instance[(short)92];

		public static ClothingItem DLCYearOfSnakeClothBlue => Instance[(short)93];

		public static ClothingItem DLCYearOfSnakeClothYellow => Instance[(short)94];

		public static ClothingItem DLCYearOfHorseCloth => Instance[(short)95];
	}

	public static Clothing Instance = new Clothing();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"ItemSubType", "GroupId", "ResourceType", "MakeItemSubType", "EquipmentEffectId", "TemplateId", "Name", "Grade", "Icon", "Desc",
		"MaxDurability", "BaseWeight", "BaseHappinessChange", "DropRate", "DisplayId", "DlcName", "SmallVillageDesc"
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
		_dataArray.Add(new ClothingItem(0, 0, 3, 300, 0, 0, "icon_Clothing_shanyeshuhe", 1, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 12, 176, 2, -1, 1, 2, keepOnPassing: false, 10, 1, null, 2, 0));
		_dataArray.Add(new ClothingItem(1, 3, 3, 300, 1, 0, "icon_Clothing_shanyeshuhe", 4, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 12, 176, 2, -1, 2, 2, keepOnPassing: false, 30, 1, null, 5, 0));
		_dataArray.Add(new ClothingItem(2, 6, 3, 300, 2, 0, "icon_Clothing_shanyeshuhe", 7, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, 176, 2, -1, 3, 2, keepOnPassing: false, 60, 1, null, 8, 0));
		_dataArray.Add(new ClothingItem(3, 9, 3, 300, 3, 0, "icon_Clothing_liangongfu", 10, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 12, 176, 2, -1, 4, 2, keepOnPassing: false, 100, 1, null, 11, 0));
		_dataArray.Add(new ClothingItem(4, 12, 3, 300, 4, 0, "icon_Clothing_liangongfu", 13, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, 176, 2, -1, 5, 2, keepOnPassing: false, 150, 1, null, 14, 0));
		_dataArray.Add(new ClothingItem(5, 15, 3, 300, 5, 0, "icon_Clothing_liangongfu", 16, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 12, 176, 2, -1, 6, 2, keepOnPassing: false, 210, 1, null, 17, 0));
		_dataArray.Add(new ClothingItem(6, 18, 3, 300, 6, 0, "icon_Clothing_nabaojinqiu", 19, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, 176, 2, -1, 7, 2, keepOnPassing: false, 280, 1, null, 20, 0));
		_dataArray.Add(new ClothingItem(7, 21, 3, 300, 7, 0, "icon_Clothing_nabaojinqiu", 22, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 150, 14100, 21150, 5, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 12, 176, 2, -1, 8, 2, keepOnPassing: false, 360, 1, null, 23, 0));
		_dataArray.Add(new ClothingItem(8, 24, 3, 300, 8, 0, "icon_Clothing_nabaojinqiu", 25, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 20500, 30750, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 12, 176, 2, -1, 9, 2, keepOnPassing: false, 450, 1, null, 26, 0));
		_dataArray.Add(new ClothingItem(9, 27, 3, 300, 0, 9, "icon_Clothing_shanyeshuhe", 28, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 12, 177, 2, -1, 10, 2, keepOnPassing: false, 10, 1, null, 29, 0));
		_dataArray.Add(new ClothingItem(10, 30, 3, 300, 1, 9, "icon_Clothing_shanyeshuhe", 31, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 12, 177, 2, -1, 11, 2, keepOnPassing: false, 30, 1, null, 32, 0));
		_dataArray.Add(new ClothingItem(11, 33, 3, 300, 2, 9, "icon_Clothing_shanyeshuhe", 34, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, 177, 2, -1, 12, 2, keepOnPassing: false, 60, 1, null, 35, 0));
		_dataArray.Add(new ClothingItem(12, 36, 3, 300, 3, 9, "icon_Clothing_liangongfu", 37, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 12, 177, 2, -1, 13, 2, keepOnPassing: false, 100, 1, null, 38, 0));
		_dataArray.Add(new ClothingItem(13, 39, 3, 300, 4, 9, "icon_Clothing_liangongfu", 40, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, 177, 2, -1, 14, 2, keepOnPassing: false, 150, 1, null, 41, 0));
		_dataArray.Add(new ClothingItem(14, 42, 3, 300, 5, 9, "icon_Clothing_liangongfu", 43, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 12, 177, 2, -1, 15, 2, keepOnPassing: false, 210, 1, null, 44, 0));
		_dataArray.Add(new ClothingItem(15, 45, 3, 300, 6, 9, "icon_Clothing_nabaojinqiu", 46, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, 177, 2, -1, 16, 2, keepOnPassing: false, 280, 1, null, 47, 0));
		_dataArray.Add(new ClothingItem(16, 48, 3, 300, 7, 9, "icon_Clothing_nabaojinqiu", 49, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 50, 14100, 21150, 5, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 12, 177, 2, -1, 17, 2, keepOnPassing: false, 360, 1, null, 50, 0));
		_dataArray.Add(new ClothingItem(17, 51, 3, 300, 8, 9, "icon_Clothing_nabaojinqiu", 52, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 60, 20500, 30750, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 12, 177, 2, -1, 18, 2, keepOnPassing: false, 450, 1, null, 53, 0));
		_dataArray.Add(new ClothingItem(18, 54, 3, 301, 2, 18, "icon_Clothing_shaolinsengyi", 55, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 19, 2, keepOnPassing: false, 60, 2, null, 56, 0));
		_dataArray.Add(new ClothingItem(19, 57, 3, 301, 4, 18, "icon_Clothing_shaolinluohanpao", 58, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 20, 2, keepOnPassing: false, 150, 2, null, 59, 0));
		_dataArray.Add(new ClothingItem(20, 60, 3, 301, 6, 18, "icon_Clothing_shaolinjiasha", 61, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 21, 2, keepOnPassing: false, 280, 2, null, 62, 0));
		_dataArray.Add(new ClothingItem(21, 63, 3, 301, 2, 21, "icon_Clothing_emeixiupao", 64, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 22, 2, keepOnPassing: false, 60, 2, null, 65, 0));
		_dataArray.Add(new ClothingItem(22, 66, 3, 301, 4, 21, "icon_Clothing_emeijinyi", 67, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 23, 2, keepOnPassing: false, 150, 2, null, 68, 0));
		_dataArray.Add(new ClothingItem(23, 69, 3, 301, 6, 21, "icon_Clothing_emeibaolianyi", 70, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 24, 2, keepOnPassing: false, 280, 2, null, 71, 0));
		_dataArray.Add(new ClothingItem(24, 72, 3, 301, 2, 24, "icon_Clothing_yuzhenpi", 73, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 25, 2, keepOnPassing: false, 60, 2, null, 74, 0));
		_dataArray.Add(new ClothingItem(25, 75, 3, 301, 4, 24, "icon_Clothing_tacuipao", 76, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 26, 2, keepOnPassing: false, 150, 2, null, 77, 0));
		_dataArray.Add(new ClothingItem(26, 78, 3, 301, 6, 24, "icon_Clothing_qinghupigua", 79, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 27, 2, keepOnPassing: false, 280, 2, null, 80, 0));
		_dataArray.Add(new ClothingItem(27, 81, 3, 301, 2, 27, "icon_Clothing_wudangdaopao", 82, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 28, 2, keepOnPassing: false, 60, 2, null, 83, 0));
		_dataArray.Add(new ClothingItem(28, 84, 3, 301, 4, 27, "icon_Clothing_qingyangdaopao", 85, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 29, 2, keepOnPassing: false, 150, 2, null, 86, 0));
		_dataArray.Add(new ClothingItem(29, 87, 3, 301, 6, 27, "icon_Clothing_zhenwudaopao", 88, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 60, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 30, 2, keepOnPassing: false, 280, 2, null, 89, 0));
		_dataArray.Add(new ClothingItem(30, 87, 3, 301, 6, 27, "icon_Clothing_zhenwudaopao", 88, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 60, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 0, isSpecial: true, 4, 12, -1, 2, -1, 31, 2, keepOnPassing: false, 280, 2, null, 90, 0));
		_dataArray.Add(new ClothingItem(31, 91, 3, 301, 2, 31, "icon_Clothing_yuanshankuxingyi", 92, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 32, 2, keepOnPassing: false, 60, 2, null, 93, 0));
		_dataArray.Add(new ClothingItem(32, 94, 3, 301, 4, 31, "icon_Clothing_yuanshanhufayi", 95, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 33, 2, keepOnPassing: false, 150, 2, null, 96, 0));
		_dataArray.Add(new ClothingItem(33, 97, 3, 301, 6, 31, "icon_Clothing_yuanshanzunshipao", 98, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 34, 2, keepOnPassing: false, 280, 2, null, 99, 0));
		_dataArray.Add(new ClothingItem(34, 100, 3, 301, 2, 34, "icon_Clothing_shixianghushenjia", 101, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 100, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 35, 2, keepOnPassing: false, 60, 2, null, 102, 0));
		_dataArray.Add(new ClothingItem(35, 103, 3, 301, 4, 34, "icon_Clothing_shixiangbaizhanjia", 104, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 200, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 36, 2, keepOnPassing: false, 150, 2, null, 105, 0));
		_dataArray.Add(new ClothingItem(36, 106, 3, 301, 6, 34, "icon_Clothing_shiwangkai", 107, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 300, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 37, 2, keepOnPassing: false, 280, 2, null, 108, 0));
		_dataArray.Add(new ClothingItem(37, 109, 3, 301, 2, 37, "icon_Clothing_youfangyi", 110, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 38, 2, keepOnPassing: false, 60, 2, null, 111, 0));
		_dataArray.Add(new ClothingItem(38, 112, 3, 301, 4, 37, "icon_Clothing_wenxianshan", 113, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 39, 2, keepOnPassing: false, 150, 2, null, 114, 0));
		_dataArray.Add(new ClothingItem(39, 115, 3, 301, 6, 37, "icon_Clothing_wuchenpao", 116, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 40, 2, keepOnPassing: false, 280, 2, null, 117, 0));
		_dataArray.Add(new ClothingItem(40, 118, 3, 301, 2, 40, "icon_Clothing_xuannvsuyi", 119, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 41, 2, keepOnPassing: false, 60, 2, null, 120, 0));
		_dataArray.Add(new ClothingItem(41, 121, 3, 301, 4, 40, "icon_Clothing_xuannvlingqiu", 122, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 42, 2, keepOnPassing: false, 150, 2, null, 123, 0));
		_dataArray.Add(new ClothingItem(42, 124, 3, 301, 6, 40, "icon_Clothing_xuannvtianyi", 125, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 43, 2, keepOnPassing: false, 280, 2, null, 126, 0));
		_dataArray.Add(new ClothingItem(43, 127, 3, 301, 2, 43, "icon_Clothing_huogongzhuang", 128, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 44, 2, keepOnPassing: false, 60, 2, null, 129, 0));
		_dataArray.Add(new ClothingItem(44, 130, 3, 301, 4, 43, "icon_Clothing_mingshijinfu", 131, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 60, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 45, 2, keepOnPassing: false, 150, 2, null, 132, 0));
		_dataArray.Add(new ClothingItem(45, 133, 3, 301, 6, 43, "icon_Clothing_dajiangpao", 134, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 80, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 46, 2, keepOnPassing: false, 280, 2, null, 135, 0));
		_dataArray.Add(new ClothingItem(46, 136, 3, 301, 2, 46, "icon_Clothing_yaoshiduanqiu", 137, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 50, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 47, 2, keepOnPassing: false, 60, 2, null, 138, 0));
		_dataArray.Add(new ClothingItem(47, 139, 3, 301, 4, 46, "icon_Clothing_yaoshijinqiu", 140, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 100, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 48, 2, keepOnPassing: false, 150, 2, null, 141, 0));
		_dataArray.Add(new ClothingItem(48, 142, 3, 301, 6, 46, "icon_Clothing_yaowangbaoqiu", 143, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 150, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 49, 2, keepOnPassing: false, 280, 2, null, 144, 0));
		_dataArray.Add(new ClothingItem(49, 145, 3, 301, 2, 49, "icon_Clothing_jingangpigua", 146, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 50, 2, keepOnPassing: false, 60, 2, null, 147, 0));
		_dataArray.Add(new ClothingItem(50, 148, 3, 301, 4, 49, "icon_Clothing_zunzhebaoyi", 149, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 51, 2, keepOnPassing: false, 150, 2, null, 150, 0));
		_dataArray.Add(new ClothingItem(51, 151, 3, 301, 6, 49, "icon_Clothing_mingwangfayi", 152, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 52, 2, keepOnPassing: false, 280, 2, null, 153, 0));
		_dataArray.Add(new ClothingItem(52, 154, 3, 301, 2, 52, "icon_Clothing_miaoxiularanyi", 155, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 53, 2, keepOnPassing: false, 60, 2, null, 156, 0));
		_dataArray.Add(new ClothingItem(53, 157, 3, 301, 4, 52, "icon_Clothing_wucaiwuyi", 158, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 54, 2, keepOnPassing: false, 150, 2, null, 159, 0));
		_dataArray.Add(new ClothingItem(54, 160, 3, 301, 6, 52, "icon_Clothing_shengjiaofayi", 161, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 55, 2, keepOnPassing: false, 280, 2, null, 162, 0));
		_dataArray.Add(new ClothingItem(55, 163, 3, 301, 2, 55, "icon_Clothing_yexingyi", 164, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 56, 2, keepOnPassing: false, 60, 2, null, 165, 0));
		_dataArray.Add(new ClothingItem(56, 166, 3, 301, 4, 55, "icon_Clothing_moyingzhuang", 167, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 57, 2, keepOnPassing: false, 150, 2, null, 168, 0));
		_dataArray.Add(new ClothingItem(57, 169, 3, 301, 6, 55, "icon_Clothing_zhuaixingpao", 170, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 58, 2, keepOnPassing: false, 280, 2, null, 171, 0));
		_dataArray.Add(new ClothingItem(58, 172, 3, 301, 2, 58, "icon_Clothing_fulongchiyi", 173, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 59, 2, keepOnPassing: false, 60, 2, null, 174, 0));
		_dataArray.Add(new ClothingItem(59, 175, 3, 301, 4, 58, "icon_Clothing_fulongsihaipao", 176, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 60, 2, keepOnPassing: false, 150, 2, null, 177, 0));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new ClothingItem(60, 178, 3, 301, 6, 58, "icon_Clothing_liuhuoxuanpi", 179, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 61, 2, keepOnPassing: false, 280, 2, null, 180, 0));
		_dataArray.Add(new ClothingItem(61, 181, 3, 301, 2, 61, "icon_Clothing_xiehoujiaoyi", 182, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 600, 900, 0, 3, 900, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 12, -1, 2, -1, 62, 2, keepOnPassing: false, 60, 2, null, 183, 0));
		_dataArray.Add(new ClothingItem(62, 184, 3, 301, 4, 61, "icon_Clothing_xiehoujinyi", 185, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 12, -1, 2, -1, 63, 2, keepOnPassing: false, 150, 2, null, 186, 0));
		_dataArray.Add(new ClothingItem(63, 187, 3, 301, 6, 61, "icon_Clothing_houmuxueyi", 188, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 12, -1, 2, -1, 64, 2, keepOnPassing: false, 280, 2, null, 189, 0));
		_dataArray.Add(new ClothingItem(64, 190, 3, 303, 0, -1, "icon_Clothing_qiangbao", 191, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: false, 5, 10, 0, 0, 0, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 0, 0, keepOnPassing: false, 0, 0, null, 192, 0));
		_dataArray.Add(new ClothingItem(65, 193, 3, 303, 0, -1, "icon_Clothing_tongyi", 194, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: false, 5, 10, 0, 0, 0, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 0, 1, keepOnPassing: false, 0, 0, null, 195, 0));
		_dataArray.Add(new ClothingItem(66, 196, 3, 303, 2, 66, "icon_Clothing_yingeyi", 197, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 50, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 15, isSpecial: true, 4, 12, -1, 2, -1, 10000, 2, keepOnPassing: false, 60, 4, null, 198, 0));
		_dataArray.Add(new ClothingItem(67, 199, 3, 303, 4, 66, "icon_Clothing_yehuopao", 200, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 100, 0, 0, 2, 0, 0, 0, allowRandomCreate: true, 10, isSpecial: true, 4, 12, -1, 2, -1, 10001, 2, keepOnPassing: false, 150, 4, null, 201, 0));
		_dataArray.Add(new ClothingItem(68, 202, 3, 303, 6, 66, "icon_Clothing_xuanshiheipi", 203, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 200, 0, 0, 4, 0, 0, 0, allowRandomCreate: true, 5, isSpecial: true, 4, 12, -1, 2, -1, 10002, 2, keepOnPassing: false, 280, 4, null, 204, 0));
		_dataArray.Add(new ClothingItem(69, 196, 3, 303, 2, 69, "icon_Clothing_yingeyi", 197, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 50, 0, 0, 0, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 20000, 2, keepOnPassing: false, 60, 4, null, 205, 0));
		_dataArray.Add(new ClothingItem(70, 199, 3, 303, 4, 69, "icon_Clothing_yehuopao", 200, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 100, 0, 0, 2, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 20001, 2, keepOnPassing: false, 150, 4, null, 206, 0));
		_dataArray.Add(new ClothingItem(71, 202, 3, 303, 6, 69, "icon_Clothing_xuanshiheipi", 203, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 200, 0, 0, 4, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 20002, 2, keepOnPassing: false, 280, 4, null, 207, 0));
		_dataArray.Add(new ClothingItem(72, 208, 3, 303, 8, -1, "icon_Clothing_bageyi", 209, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30000, 2, keepOnPassing: true, 0, 4, "GiftFromConchShip1", 210, 0));
		_dataArray.Add(new ClothingItem(73, 211, 3, 303, 8, -1, "icon_Clothing_yindaoyi", 212, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30001, 2, keepOnPassing: true, 0, 4, "GiftFromConchShip1", 213, 0));
		_dataArray.Add(new ClothingItem(74, 214, 3, 303, 8, -1, "icon_Clothing_banhaoyi", 215, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 66, 2, keepOnPassing: true, 0, 4, "GiftFromConchShip2", 216, 0));
		_dataArray.Add(new ClothingItem(75, 217, 3, 303, 8, -1, "icon_Clothing_baijiaolinyi", 218, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30002, 2, keepOnPassing: true, 0, 4, "FiveLoong", 219, 0));
		_dataArray.Add(new ClothingItem(76, 220, 3, 303, 8, -1, "icon_Clothing_heijiaolinyi", 221, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30003, 2, keepOnPassing: true, 0, 4, "FiveLoong", 222, 0));
		_dataArray.Add(new ClothingItem(77, 223, 3, 303, 8, -1, "icon_Clothing_qingjiaolinyi", 224, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30005, 2, keepOnPassing: true, 0, 4, "FiveLoong", 225, 0));
		_dataArray.Add(new ClothingItem(78, 226, 3, 303, 8, -1, "icon_Clothing_chijiaolinyi", 227, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30004, 2, keepOnPassing: true, 0, 4, "FiveLoong", 228, 0));
		_dataArray.Add(new ClothingItem(79, 229, 3, 303, 8, -1, "icon_Clothing_huangjiaolinyi", 230, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30006, 2, keepOnPassing: true, 0, 4, "FiveLoong", 231, 0));
		_dataArray.Add(new ClothingItem(80, 232, 3, 303, 8, -1, "icon_Clothing_zhuyuewushuangyi", 233, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30007, 2, keepOnPassing: true, 0, 4, "HappyNewYear2024", 234, 0));
		_dataArray.Add(new ClothingItem(81, 235, 3, 301, 2, -1, "icon_Clothing_longshiyi", 236, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 600, 900, 0, 3, 900, 5, allowRandomCreate: false, 25, isSpecial: true, 4, 12, -1, 2, -1, 69, 2, keepOnPassing: false, 0, 4, null, 237, 0));
		_dataArray.Add(new ClothingItem(82, 238, 3, 301, 4, -1, "icon_Clothing_jinglongyi", 239, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 3100, 4650, 2, 5, 2100, 7, allowRandomCreate: false, 15, isSpecial: true, 4, 12, -1, 2, -1, 68, 2, keepOnPassing: false, 0, 4, null, 240, 0));
		_dataArray.Add(new ClothingItem(83, 241, 3, 303, 6, -1, "icon_Clothing_xiankepao", 242, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 9200, 13800, 4, 7, 3600, 8, allowRandomCreate: false, 5, isSpecial: true, 4, 12, -1, 2, -1, 70, 2, keepOnPassing: false, 0, 4, null, 243, 0));
		_dataArray.Add(new ClothingItem(84, 244, 3, 302, 0, 84, "icon_Clothing_danyisupao", 245, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 100, 150, 0, 1, 600, 3, allowRandomCreate: false, 0, isSpecial: true, 4, 12, 177, 2, -1, 71, 2, keepOnPassing: false, 10, 3, null, 246, 0));
		_dataArray.Add(new ClothingItem(85, 247, 3, 302, 1, 84, "icon_Clothing_tiansheyi", 248, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 200, 300, 0, 2, 1200, 4, allowRandomCreate: false, 0, isSpecial: true, 4, 12, 177, 2, -1, 72, 2, keepOnPassing: false, 30, 3, null, 249, 0));
		_dataArray.Add(new ClothingItem(86, 250, 3, 302, 2, 84, "icon_Clothing_jiangzuofu", 251, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 40, 600, 900, 0, 3, 1800, 5, allowRandomCreate: false, 0, isSpecial: true, 4, 12, 177, 2, -1, 73, 2, keepOnPassing: false, 60, 3, null, 252, 0));
		_dataArray.Add(new ClothingItem(87, 253, 3, 302, 3, 84, "icon_Clothing_qingluopi", 254, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: false, 0, isSpecial: true, 4, 12, 177, 2, -1, 74, 2, keepOnPassing: false, 100, 3, null, 255, 0));
		_dataArray.Add(new ClothingItem(88, 256, 3, 302, 4, 84, "icon_Clothing_jinyuqiu", 257, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 30, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: false, 0, isSpecial: true, 4, 12, 177, 2, -1, 75, 2, keepOnPassing: false, 150, 3, null, 258, 0));
		_dataArray.Add(new ClothingItem(89, 259, 3, 302, 5, 84, "icon_Clothing_lanshan", 260, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: false, 0, isSpecial: true, 4, 12, 177, 2, -1, 76, 2, keepOnPassing: false, 210, 3, null, 261, 0));
		_dataArray.Add(new ClothingItem(90, 262, 3, 302, 6, 84, "icon_Clothing_guqingjuanjia", 263, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 20, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: false, 0, isSpecial: true, 4, 12, 177, 2, -1, 77, 2, keepOnPassing: false, 280, 3, null, 264, 0));
		_dataArray.Add(new ClothingItem(91, 265, 3, 302, 7, 84, "icon_Clothing_bodaibaoyi", 266, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 10, 14100, 21150, 5, 8, 9000, 8, allowRandomCreate: false, 0, isSpecial: true, 4, 12, 177, 2, -1, 78, 2, keepOnPassing: false, 360, 3, null, 267, 0));
		_dataArray.Add(new ClothingItem(92, 268, 3, 303, 8, 92, "icon_Clothing_bixiaolingsheyi", 269, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30008, 2, keepOnPassing: true, 0, 4, "YearOfSnakeCloth", 270, 0));
		_dataArray.Add(new ClothingItem(93, 271, 3, 303, 8, 92, "icon_Clothing_qingxiaolingshey", 269, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30009, 2, keepOnPassing: true, 0, 4, "YearOfSnakeCloth", 272, 0));
		_dataArray.Add(new ClothingItem(94, 273, 3, 303, 8, 92, "icon_Clothing_qiongxiaolingsheyi", 269, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30010, 2, keepOnPassing: true, 0, 4, "YearOfSnakeCloth", 274, 0));
		_dataArray.Add(new ClothingItem(95, 275, 3, 303, 8, 95, "icon_Clothing_xiangjuyi", 276, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 5, 0, 0, 0, 6, 0, 0, 0, allowRandomCreate: false, 0, isSpecial: true, 4, 12, -1, 2, -1, 30011, 2, keepOnPassing: true, 0, 4, "HappyNewYear2026", 277, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ClothingItem>(96);
		CreateItems0();
		CreateItems1();
	}
}
