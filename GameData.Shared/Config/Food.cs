using System;
using System.Collections.Generic;
using Config.Common;
using GameData.Domains.Character;

namespace Config;

[Serializable]
public class Food : ConfigData<FoodItem, short>
{
	public static class DefKey
	{
		public const short Yeguo0 = 0;

		public const short Yeguo1 = 1;

		public const short Yeguo2 = 2;

		public const short Yeguo3 = 3;

		public const short Yeguo4 = 4;

		public const short Yeguo5 = 5;

		public const short Yeguo6 = 6;

		public const short Yeguo7 = 7;

		public const short Yeguo8 = 8;

		public const short Qingzhujidan0 = 9;

		public const short Qingzhujidan1 = 10;

		public const short Qingzhujidan2 = 11;

		public const short Qingzhujidan3 = 12;

		public const short Qingzhujidan4 = 13;

		public const short Qingzhujidan5 = 14;

		public const short Qingzhujidan6 = 15;

		public const short Qingzhujidan7 = 16;

		public const short Qingzhujidan8 = 17;

		public const short Zhiya0 = 26;

		public const short Zhiya1 = 27;

		public const short Zhiya2 = 28;

		public const short Zhiya3 = 29;

		public const short Zhiya4 = 30;

		public const short Zhiya5 = 31;

		public const short Zhiya6 = 32;

		public const short Tankaoturou0 = 51;

		public const short Tankaoturou1 = 52;

		public const short Tankaoturou2 = 53;

		public const short Tankaoturou3 = 54;

		public const short Tankaoturou4 = 55;

		public const short Tankaoturou5 = 56;

		public const short Tankaoturou6 = 57;

		public const short Tankaoturou7 = 58;

		public const short Tankaoturou8 = 59;

		public const short Ximoyangshenghui0 = 68;

		public const short Ximoyangshenghui1 = 69;

		public const short Ximoyangshenghui2 = 70;

		public const short Ximoyangshenghui3 = 71;

		public const short Ximoyangshenghui4 = 72;

		public const short Ximoyangshenghui5 = 73;

		public const short Ximoyangshenghui6 = 74;

		public const short Baxiongzhang0 = 90;

		public const short Baxiongzhang1 = 91;

		public const short Baxiongzhang2 = 92;

		public const short Chuibing0 = 93;

		public const short Chuibing1 = 94;

		public const short Chuibing2 = 95;

		public const short Chuibing3 = 96;

		public const short Chuibing4 = 97;

		public const short Chuibing5 = 98;

		public const short Chuibing6 = 99;

		public const short Chuibing7 = 100;

		public const short Chuibing8 = 101;

		public const short Shengjiandoufu0 = 102;

		public const short Shengjiandoufu1 = 103;

		public const short Shengjiandoufu2 = 104;

		public const short Shengjiandoufu3 = 105;

		public const short Shengjiandoufu4 = 106;

		public const short Shengjiandoufu5 = 107;

		public const short Shengjiandoufu6 = 108;

		public const short Shengjiandoufu7 = 109;

		public const short Taiqingtang0 = 132;

		public const short Taiqingtang1 = 133;

		public const short Taiqingtang2 = 134;

		public const short Luandunyupian0 = 135;

		public const short Luandunyupian1 = 136;

		public const short Luandunyupian2 = 137;

		public const short Luandunyupian3 = 138;

		public const short Luandunyupian4 = 139;

		public const short Luandunyupian5 = 140;

		public const short Luandunyupian6 = 141;

		public const short Luandunyupian7 = 142;

		public const short Luandunyupian8 = 143;

		public const short Baizhuoxia0 = 144;

		public const short Baizhuoxia1 = 145;

		public const short Baizhuoxia2 = 146;

		public const short Baizhuoxia3 = 147;

		public const short Baizhuoxia4 = 148;

		public const short Baizhuoxia5 = 149;

		public const short Baizhuoxia6 = 150;

		public const short Baizhuoxia7 = 151;

		public const short Xianhuanglu0 = 174;

		public const short Xianhuanglu1 = 175;

		public const short Xianhuanglu2 = 176;
	}

	public static class DefValue
	{
		public static FoodItem Yeguo0 => Instance[(short)0];

		public static FoodItem Yeguo1 => Instance[(short)1];

		public static FoodItem Yeguo2 => Instance[(short)2];

		public static FoodItem Yeguo3 => Instance[(short)3];

		public static FoodItem Yeguo4 => Instance[(short)4];

		public static FoodItem Yeguo5 => Instance[(short)5];

		public static FoodItem Yeguo6 => Instance[(short)6];

		public static FoodItem Yeguo7 => Instance[(short)7];

		public static FoodItem Yeguo8 => Instance[(short)8];

		public static FoodItem Qingzhujidan0 => Instance[(short)9];

		public static FoodItem Qingzhujidan1 => Instance[(short)10];

		public static FoodItem Qingzhujidan2 => Instance[(short)11];

		public static FoodItem Qingzhujidan3 => Instance[(short)12];

		public static FoodItem Qingzhujidan4 => Instance[(short)13];

		public static FoodItem Qingzhujidan5 => Instance[(short)14];

		public static FoodItem Qingzhujidan6 => Instance[(short)15];

		public static FoodItem Qingzhujidan7 => Instance[(short)16];

		public static FoodItem Qingzhujidan8 => Instance[(short)17];

		public static FoodItem Zhiya0 => Instance[(short)26];

		public static FoodItem Zhiya1 => Instance[(short)27];

		public static FoodItem Zhiya2 => Instance[(short)28];

		public static FoodItem Zhiya3 => Instance[(short)29];

		public static FoodItem Zhiya4 => Instance[(short)30];

		public static FoodItem Zhiya5 => Instance[(short)31];

		public static FoodItem Zhiya6 => Instance[(short)32];

		public static FoodItem Tankaoturou0 => Instance[(short)51];

		public static FoodItem Tankaoturou1 => Instance[(short)52];

		public static FoodItem Tankaoturou2 => Instance[(short)53];

		public static FoodItem Tankaoturou3 => Instance[(short)54];

		public static FoodItem Tankaoturou4 => Instance[(short)55];

		public static FoodItem Tankaoturou5 => Instance[(short)56];

		public static FoodItem Tankaoturou6 => Instance[(short)57];

		public static FoodItem Tankaoturou7 => Instance[(short)58];

		public static FoodItem Tankaoturou8 => Instance[(short)59];

		public static FoodItem Ximoyangshenghui0 => Instance[(short)68];

		public static FoodItem Ximoyangshenghui1 => Instance[(short)69];

		public static FoodItem Ximoyangshenghui2 => Instance[(short)70];

		public static FoodItem Ximoyangshenghui3 => Instance[(short)71];

		public static FoodItem Ximoyangshenghui4 => Instance[(short)72];

		public static FoodItem Ximoyangshenghui5 => Instance[(short)73];

		public static FoodItem Ximoyangshenghui6 => Instance[(short)74];

		public static FoodItem Baxiongzhang0 => Instance[(short)90];

		public static FoodItem Baxiongzhang1 => Instance[(short)91];

		public static FoodItem Baxiongzhang2 => Instance[(short)92];

		public static FoodItem Chuibing0 => Instance[(short)93];

		public static FoodItem Chuibing1 => Instance[(short)94];

		public static FoodItem Chuibing2 => Instance[(short)95];

		public static FoodItem Chuibing3 => Instance[(short)96];

		public static FoodItem Chuibing4 => Instance[(short)97];

		public static FoodItem Chuibing5 => Instance[(short)98];

		public static FoodItem Chuibing6 => Instance[(short)99];

		public static FoodItem Chuibing7 => Instance[(short)100];

		public static FoodItem Chuibing8 => Instance[(short)101];

		public static FoodItem Shengjiandoufu0 => Instance[(short)102];

		public static FoodItem Shengjiandoufu1 => Instance[(short)103];

		public static FoodItem Shengjiandoufu2 => Instance[(short)104];

		public static FoodItem Shengjiandoufu3 => Instance[(short)105];

		public static FoodItem Shengjiandoufu4 => Instance[(short)106];

		public static FoodItem Shengjiandoufu5 => Instance[(short)107];

		public static FoodItem Shengjiandoufu6 => Instance[(short)108];

		public static FoodItem Shengjiandoufu7 => Instance[(short)109];

		public static FoodItem Taiqingtang0 => Instance[(short)132];

		public static FoodItem Taiqingtang1 => Instance[(short)133];

		public static FoodItem Taiqingtang2 => Instance[(short)134];

		public static FoodItem Luandunyupian0 => Instance[(short)135];

		public static FoodItem Luandunyupian1 => Instance[(short)136];

		public static FoodItem Luandunyupian2 => Instance[(short)137];

		public static FoodItem Luandunyupian3 => Instance[(short)138];

		public static FoodItem Luandunyupian4 => Instance[(short)139];

		public static FoodItem Luandunyupian5 => Instance[(short)140];

		public static FoodItem Luandunyupian6 => Instance[(short)141];

		public static FoodItem Luandunyupian7 => Instance[(short)142];

		public static FoodItem Luandunyupian8 => Instance[(short)143];

		public static FoodItem Baizhuoxia0 => Instance[(short)144];

		public static FoodItem Baizhuoxia1 => Instance[(short)145];

		public static FoodItem Baizhuoxia2 => Instance[(short)146];

		public static FoodItem Baizhuoxia3 => Instance[(short)147];

		public static FoodItem Baizhuoxia4 => Instance[(short)148];

		public static FoodItem Baizhuoxia5 => Instance[(short)149];

		public static FoodItem Baizhuoxia6 => Instance[(short)150];

		public static FoodItem Baizhuoxia7 => Instance[(short)151];

		public static FoodItem Xianhuanglu0 => Instance[(short)174];

		public static FoodItem Xianhuanglu1 => Instance[(short)175];

		public static FoodItem Xianhuanglu2 => Instance[(short)176];
	}

	public static Food Instance = new Food();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"ItemSubType", "GroupId", "ResourceType", "BreakBonusEffect", "FoodType", "TemplateId", "Name", "Grade", "Icon", "BigIcon",
		"Desc", "BaseWeight", "BaseHappinessChange", "DropRate"
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
		_dataArray.Add(new FoodItem(0, 0, 7, 700, 0, 0, "icon_Food_yeguo", "bigIcon_Food_yeguo", 1, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 10, 50, 25, 0, 1, 600, 3, allowRandomCreate: true, 50, isSpecial: false, 0, 3, 35, 1, 1, new MainAttributes(default(short), default(short), default(short), default(short), default(short), default(short)), 0, 0, 0, 0, 0, 0, 25, 25, 25, 25, 0, 0, 25, 25, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(1, 2, 7, 700, 1, 0, "icon_Food_yeguo", "bigIcon_Food_yeguo", 3, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 100, 50, 0, 2, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 3, 35, 1, 1, new MainAttributes(default(short), default(short), default(short), default(short), default(short), default(short)), 0, 0, 0, 0, 0, 0, 30, 30, 30, 30, 0, 0, 30, 30, 30, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(2, 4, 7, 700, 2, 0, "icon_Food_yeguo", "bigIcon_Food_yeguo", 5, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 10, 300, 150, 0, 3, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 35, 1, 1, new MainAttributes(default(short), default(short), default(short), default(short), default(short), default(short)), 0, 0, 0, 0, 0, 0, 35, 35, 35, 35, 0, 0, 35, 35, 35, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(3, 6, 7, 700, 3, 0, "icon_Food_haitangguo", "bigIcon_Food_haitangguo", 7, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 10, 750, 375, 0, 4, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 35, 1, 1, new MainAttributes(default(short), default(short), default(short), default(short), default(short), default(short)), 0, 0, 0, 0, 0, 0, 40, 40, 40, 40, 0, 0, 40, 40, 40, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(4, 8, 7, 700, 4, 0, "icon_Food_haitangguo", "bigIcon_Food_haitangguo", 9, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 1550, 775, 1, 5, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 35, 1, 1, new MainAttributes(default(short), default(short), default(short), default(short), default(short), default(short)), 0, 0, 0, 0, 0, 0, 45, 45, 45, 45, 0, 0, 45, 45, 45, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(5, 10, 7, 700, 5, 0, "icon_Food_haitangguo", "bigIcon_Food_haitangguo", 11, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 10, 2800, 1400, 2, 6, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 35, 1, 1, new MainAttributes(default(short), default(short), default(short), default(short), default(short), default(short)), 0, 0, 0, 0, 0, 0, 50, 50, 50, 50, 0, 0, 50, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(6, 12, 7, 700, 6, 0, "icon_Food_yeshiliu", "bigIcon_Food_yeshiliu", 13, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 4600, 2300, 3, 7, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 35, 1, 1, new MainAttributes(default(short), default(short), default(short), default(short), default(short), default(short)), 0, 0, 0, 0, 0, 0, 60, 60, 60, 60, 0, 0, 60, 60, 60, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(7, 14, 7, 700, 7, 0, "icon_Food_rencanguo", "bigIcon_Food_rencanguo", 15, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 7050, 3525, 4, 8, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 35, 1, 1, new MainAttributes(default(short), default(short), default(short), default(short), default(short), default(short)), 0, 0, 0, 0, 0, 0, 75, 75, 75, 75, 0, 0, 75, 75, 75, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(8, 16, 7, 700, 8, 0, "icon_Food_pantaoxianguo", "bigIcon_Food_pantaoxianguo", 17, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 10250, 5125, 5, 9, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 35, 1, 1, new MainAttributes(default(short), default(short), default(short), default(short), default(short), default(short)), 0, 0, 0, 0, 0, 0, 95, 95, 95, 95, 0, 0, 95, 95, 95, 95, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(9, 18, 7, 701, 0, 9, "icon_Food_qingzhujidan", "bigIcon_Food_qingzhujidan", 19, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 10, 50, 25, 0, 2, 600, 3, allowRandomCreate: true, 50, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(10, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Egg
		}));
		_dataArray.Add(new FoodItem(10, 20, 7, 701, 1, 9, "icon_Food_qingzhujidan", "bigIcon_Food_qingzhujidan", 21, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 10, 100, 50, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(15, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Egg
		}));
		_dataArray.Add(new FoodItem(11, 22, 7, 701, 2, 9, "icon_Food_qingzhujidan", "bigIcon_Food_qingzhujidan", 23, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(20, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Egg,
			EFoodFoodType.Rice
		}));
		_dataArray.Add(new FoodItem(12, 24, 7, 701, 3, 9, "icon_Food_yangzhouchaofan", "bigIcon_Food_yangzhouchaofan", 25, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(25, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Egg,
			EFoodFoodType.Rice
		}));
		_dataArray.Add(new FoodItem(13, 26, 7, 701, 4, 9, "icon_Food_yangzhouchaofan", "bigIcon_Food_yangzhouchaofan", 27, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(30, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Egg,
			EFoodFoodType.Soup
		}));
		_dataArray.Add(new FoodItem(14, 28, 7, 701, 5, 9, "icon_Food_yangzhouchaofan", "bigIcon_Food_yangzhouchaofan", 29, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(35, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Egg,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(15, 30, 7, 701, 6, 9, "icon_Food_mohuangdan", "bigIcon_Food_mohuangdan", 31, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 10, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(40, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Egg
		}));
		_dataArray.Add(new FoodItem(16, 32, 7, 701, 7, 9, "icon_Food_tiandiyikouxiang", "bigIcon_Food_tiandiyikouxiang", 33, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 10, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(45, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Egg
		}));
		_dataArray.Add(new FoodItem(17, 34, 7, 701, 8, 9, "icon_Food_jinpaofan", "bigIcon_Food_jinpaofan", 35, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(50, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Egg,
			EFoodFoodType.Rice
		}));
		_dataArray.Add(new FoodItem(18, 36, 7, 701, 1, 18, "icon_Food_jiaohuaji", "bigIcon_Food_jiaohuaji", 37, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 100, 50, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 20, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(19, 38, 7, 701, 2, 18, "icon_Food_jiaohuaji", "bigIcon_Food_jiaohuaji", 39, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 25, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(20, 40, 7, 701, 3, 18, "icon_Food_jiangbaojiding", "bigIcon_Food_jiangbaojiding", 41, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 30, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(21, 42, 7, 701, 4, 18, "icon_Food_jiangbaojiding", "bigIcon_Food_jiangbaojiding", 43, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 35, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(22, 44, 7, 701, 5, 18, "icon_Food_jiangbaojiding", "bigIcon_Food_jiangbaojiding", 45, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 40, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Egg
		}));
		_dataArray.Add(new FoodItem(23, 46, 7, 701, 6, 18, "icon_Food_furongjipian", "bigIcon_Food_furongjipian", 47, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 45, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Flower
		}));
		_dataArray.Add(new FoodItem(24, 48, 7, 701, 7, 18, "icon_Food_baizhanxiangyaji", "bigIcon_Food_baizhanxiangyaji", 49, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 50, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(25, 50, 7, 701, 8, 18, "icon_Food_naixiangxinfaji", "bigIcon_Food_naixiangxinfaji", 51, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 55, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(26, 52, 7, 701, 2, 26, "icon_Food_zhiya", "bigIcon_Food_zhiya", 53, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 50, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 30, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(27, 54, 7, 701, 3, 26, "icon_Food_huluya", "bigIcon_Food_huluya", 55, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 60, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 35, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(28, 56, 7, 701, 4, 26, "icon_Food_huluya", "bigIcon_Food_huluya", 57, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 50, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 40, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Rice,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(29, 58, 7, 701, 5, 26, "icon_Food_huluya", "bigIcon_Food_huluya", 59, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 45, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(30, 60, 7, 701, 6, 26, "icon_Food_meixiexifen", "bigIcon_Food_meixiexifen", 61, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 50, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Rice
		}));
		_dataArray.Add(new FoodItem(31, 62, 7, 701, 7, 26, "icon_Food_taibaiya", "bigIcon_Food_taibaiya", 63, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 50, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 55, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Wine
		}));
		_dataArray.Add(new FoodItem(32, 64, 7, 701, 8, 26, "icon_Food_jinlingyanshuiya", "bigIcon_Food_jinlingyanshuiya", 65, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 60, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(33, 66, 7, 701, 3, 33, "icon_Food_efenqian", "bigIcon_Food_efenqian", 67, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(40, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(34, 68, 7, 701, 4, 33, "icon_Food_efenqian", "bigIcon_Food_efenqian", 69, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 50, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(45, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(35, 70, 7, 701, 5, 33, "icon_Food_efenqian", "bigIcon_Food_efenqian", 71, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 60, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(50, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(36, 72, 7, 701, 6, 33, "icon_Food_jiansunzhenge", "bigIcon_Food_jiansunzhenge", 73, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 50, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(55, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(37, 74, 7, 701, 7, 33, "icon_Food_xiangsutiane", "bigIcon_Food_xiangsutiane", 75, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 60, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(60, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(38, 76, 7, 701, 8, 33, "icon_Food_baizhachune", "bigIcon_Food_baizhachune", 77, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 50, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(65, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(39, 78, 7, 701, 4, 39, "icon_Food_wuyisu", "bigIcon_Food_wuyisu", 79, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 50, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(40, 80, 7, 701, 5, 39, "icon_Food_wuyisu", "bigIcon_Food_wuyisu", 81, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 55, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Soup
		}));
		_dataArray.Add(new FoodItem(41, 82, 7, 701, 6, 39, "icon_Food_wudaitaoji", "bigIcon_Food_wudaitaoji", 83, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 50, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 60, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(42, 84, 7, 701, 7, 39, "icon_Food_chongcaowujishang", "bigIcon_Food_chongcaowujishang", 85, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 65, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Soup
		}));
		_dataArray.Add(new FoodItem(43, 86, 7, 701, 8, 39, "icon_Food_jinzhiwuwan", "bigIcon_Food_jinzhiwuwan", 87, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 70, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(44, 88, 7, 701, 5, 44, "icon_Food_zhagaochunzi", "bigIcon_Food_zhagaochunzi", 89, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 10, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 60, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(45, 90, 7, 701, 6, 44, "icon_Food_aoyinchun", "bigIcon_Food_aoyinchun", 91, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 65, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(46, 92, 7, 701, 7, 44, "icon_Food_bainiaoguichao", "bigIcon_Food_bainiaoguichao", 93, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 70, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(47, 94, 7, 701, 8, 44, "icon_Food_qiankunguo", "bigIcon_Food_qiankunguo", 95, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 75, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 125, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(48, 96, 7, 701, 6, 48, "icon_Food_fengchuanhua", "bigIcon_Food_fengchuanhua", 97, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 70, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Flower
		}));
		_dataArray.Add(new FoodItem(49, 98, 7, 701, 7, 48, "icon_Food_fuguiwucaihui", "bigIcon_Food_fuguiwucaihui", 99, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 75, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 125, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(50, 100, 7, 701, 8, 48, "icon_Food_chenlushang", "bigIcon_Food_chenlushang", 101, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 80, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(51, 102, 7, 701, 0, 51, "icon_Food_tankaoturou", "bigIcon_Food_tankaoturou", 103, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 50, 25, 0, 2, 600, 3, allowRandomCreate: true, 50, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 10, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(52, 104, 7, 701, 1, 51, "icon_Food_tankaoturou", "bigIcon_Food_tankaoturou", 105, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 100, 50, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 15, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(53, 106, 7, 701, 2, 51, "icon_Food_tankaoturou", "bigIcon_Food_tankaoturou", 107, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 20, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(54, 108, 7, 701, 3, 51, "icon_Food_xianguotu", "bigIcon_Food_xianguotu", 109, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 25, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(55, 110, 7, 701, 4, 51, "icon_Food_xianguotu", "bigIcon_Food_xianguotu", 111, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 30, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(56, 112, 7, 701, 5, 51, "icon_Food_xianguotu", "bigIcon_Food_xianguotu", 113, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 35, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(57, 114, 7, 701, 6, 51, "icon_Food_yingxisuantiao", "bigIcon_Food_yingxisuantiao", 115, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 40, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(58, 116, 7, 701, 7, 51, "icon_Food_yutugeng", "bigIcon_Food_yutugeng", 117, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 45, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(59, 118, 7, 701, 8, 51, "icon_Food_boxiagong", "bigIcon_Food_boxiagong", 119, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 50, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new FoodItem(60, 120, 7, 701, 1, 60, "icon_Food_huiguorou", "bigIcon_Food_huiguorou", 121, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 100, 50, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(20, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(61, 122, 7, 701, 2, 60, "icon_Food_huiguorou", "bigIcon_Food_huiguorou", 123, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(25, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Rice
		}));
		_dataArray.Add(new FoodItem(62, 124, 7, 701, 3, 60, "icon_Food_meicaikourou", "bigIcon_Food_meicaikourou", 125, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(30, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(63, 126, 7, 701, 4, 60, "icon_Food_meicaikourou", "bigIcon_Food_meicaikourou", 127, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(35, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(64, 128, 7, 701, 5, 60, "icon_Food_meicaikourou", "bigIcon_Food_meicaikourou", 129, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(40, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(65, 130, 7, 701, 6, 60, "icon_Food_shuijingyaorou", "bigIcon_Food_shuijingyaorou", 131, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(45, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(66, 132, 7, 701, 7, 60, "icon_Food_xiefenshizitou", "bigIcon_Food_xiefenshizitou", 133, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(50, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(67, 134, 7, 701, 8, 60, "icon_Food_dongporou", "bigIcon_Food_dongporou", 135, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(55, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Wine
		}));
		_dataArray.Add(new FoodItem(68, 136, 7, 701, 2, 68, "icon_Food_ximayangshenghui", "bigIcon_Food_ximayangshenghui", 137, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 30, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(69, 138, 7, 701, 3, 68, "icon_Food_yangzawu", "bigIcon_Food_yangzawu", 139, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 35, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(70, 140, 7, 701, 4, 68, "icon_Food_yangzawu", "bigIcon_Food_yangzawu", 141, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 50, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 40, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(71, 142, 7, 701, 5, 68, "icon_Food_yangzawu", "bigIcon_Food_yangzawu", 143, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 45, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(72, 144, 7, 701, 6, 68, "icon_Food_huokaoyangyao", "bigIcon_Food_huokaoyangyao", 145, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 50, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(73, 146, 7, 701, 7, 68, "icon_Food_xiaolingzhi", "bigIcon_Food_xiaolingzhi", 147, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 55, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(74, 148, 7, 701, 8, 68, "icon_Food_yangfangcangyu", "bigIcon_Food_yangfangcangyu", 149, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 50, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 60, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(75, 150, 7, 701, 3, 75, "icon_Food_sheyaoqiao", "bigIcon_Food_sheyaoqiao", 151, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 40, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Bird
		}));
		_dataArray.Add(new FoodItem(76, 152, 7, 701, 4, 75, "icon_Food_sheyaoqiao", "bigIcon_Food_sheyaoqiao", 153, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 45, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Wine
		}));
		_dataArray.Add(new FoodItem(77, 154, 7, 701, 5, 75, "icon_Food_sheyaoqiao", "bigIcon_Food_sheyaoqiao", 155, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 50, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(78, 156, 7, 701, 6, 75, "icon_Food_yaozhuguishehui", "bigIcon_Food_yaozhuguishehui", 157, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 55, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(79, 158, 7, 701, 7, 75, "icon_Food_mudanniangshefu", "bigIcon_Food_mudanniangshefu", 159, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 60, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Flower
		}));
		_dataArray.Add(new FoodItem(80, 160, 7, 701, 8, 75, "icon_Food_longhudou", "bigIcon_Food_longhudou", 161, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 65, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(81, 162, 7, 701, 4, 81, "icon_Food_qingcuanlurou", "bigIcon_Food_qingcuanlurou", 163, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 50, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(82, 164, 7, 701, 5, 81, "icon_Food_qingcuanlurou", "bigIcon_Food_qingcuanlurou", 165, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 55, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(83, 166, 7, 701, 6, 81, "icon_Food_sanshengmeihuashang", "bigIcon_Food_sanshengmeihuashang", 167, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 60, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Fruit,
			EFoodFoodType.Soup
		}));
		_dataArray.Add(new FoodItem(84, 168, 7, 701, 7, 81, "icon_Food_lurouyubaigeng", "bigIcon_Food_lurouyubaigeng", 169, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 65, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(85, 170, 7, 701, 8, 81, "icon_Food_yudinglu", "bigIcon_Food_yudinglu", 171, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 70, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(86, 172, 7, 701, 5, 86, "icon_Food_xiangbazhi", "bigIcon_Food_xiangbazhi", 173, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 60, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(60, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(87, 174, 7, 701, 6, 86, "icon_Food_jinsixiangba", "bigIcon_Food_jinsixiangba", 175, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 60, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(65, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(88, 176, 7, 701, 7, 86, "icon_Food_longnaohai", "bigIcon_Food_longnaohai", 177, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 60, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(70, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(89, 178, 7, 701, 8, 86, "icon_Food_ziyujiangxiangba", "bigIcon_Food_ziyujiangxiangba", 179, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 60, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(75, 0, 0, 0, 0, 0), 0, 0, 0, 0, 0, 0, 125, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Wine
		}));
		_dataArray.Add(new FoodItem(90, 180, 7, 701, 6, 90, "icon_Food_bɑxiongzhang", "bigIcon_Food_bɑxiongzhang", 181, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 80, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 70, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(91, 182, 7, 701, 7, 90, "icon_Food_babaoxiongzhang", "bigIcon_Food_babaoxiongzhang", 183, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 80, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 75, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 125, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast
		}));
		_dataArray.Add(new FoodItem(92, 184, 7, 701, 8, 90, "icon_Food_yipindawangzhang", "bigIcon_Food_yipindawangzhang", 185, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 80, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 80, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Beast,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(93, 186, 7, 700, 0, 93, "icon_Food_chuibing", "bigIcon_Food_chuibing", 187, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 50, 25, 0, 2, 600, 3, allowRandomCreate: true, 50, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 10, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(94, 188, 7, 700, 1, 93, "icon_Food_chuibing", "bigIcon_Food_chuibing", 189, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 100, 50, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 15, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(95, 190, 7, 700, 2, 93, "icon_Food_chuibing", "bigIcon_Food_chuibing", 191, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 20, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(96, 192, 7, 700, 3, 93, "icon_Food_furongbing", "bigIcon_Food_furongbing", 193, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 25, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Noodle,
			EFoodFoodType.Flower
		}));
		_dataArray.Add(new FoodItem(97, 194, 7, 700, 4, 93, "icon_Food_furongbing", "bigIcon_Food_furongbing", 195, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 30, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(98, 196, 7, 700, 5, 93, "icon_Food_furongbing", "bigIcon_Food_furongbing", 197, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 35, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(99, 198, 7, 700, 6, 93, "icon_Food_zimuchunjian", "bigIcon_Food_zimuchunjian", 199, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 40, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(100, 200, 7, 700, 7, 93, "icon_Food_yingtaobiluo", "bigIcon_Food_yingtaobiluo", 201, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 45, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Noodle,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(101, 202, 7, 700, 8, 93, "icon_Food_shoudaiguixiantao", "bigIcon_Food_shoudaiguixiantao", 203, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 50, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(102, 204, 7, 700, 1, 102, "icon_Food_shengjiandoufu", "bigIcon_Food_shengjiandoufu", 205, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 100, 50, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 20), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Bean
		}));
		_dataArray.Add(new FoodItem(103, 206, 7, 700, 2, 102, "icon_Food_shengjiandoufu", "bigIcon_Food_shengjiandoufu", 207, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 25), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Bean,
			EFoodFoodType.Soup
		}));
		_dataArray.Add(new FoodItem(104, 208, 7, 700, 3, 102, "icon_Food_yuxiangdoufu", "bigIcon_Food_yuxiangdoufu", 209, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 30), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Bean
		}));
		_dataArray.Add(new FoodItem(105, 210, 7, 700, 4, 102, "icon_Food_yuxiangdoufu", "bigIcon_Food_yuxiangdoufu", 211, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 35), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Bean,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(106, 212, 7, 700, 5, 102, "icon_Food_yuxiangdoufu", "bigIcon_Food_yuxiangdoufu", 213, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 40), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Bean
		}));
		_dataArray.Add(new FoodItem(107, 214, 7, 700, 6, 102, "icon_Food_yipindoufu", "bigIcon_Food_yipindoufu", 215, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 45), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Bean
		}));
		_dataArray.Add(new FoodItem(108, 216, 7, 700, 7, 102, "icon_Food_dongpodoufu", "bigIcon_Food_dongpodoufu", 217, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 50), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Bean
		}));
		_dataArray.Add(new FoodItem(109, 218, 7, 700, 8, 102, "icon_Food_baibiqingyun", "bigIcon_Food_baibiqingyun", 219, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 55), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Bean,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(110, 220, 7, 700, 2, 110, "icon_Food_xianggulengtao", "bigIcon_Food_xianggulengtao", 221, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 30, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Mushroom
		}));
		_dataArray.Add(new FoodItem(111, 222, 7, 700, 3, 110, "icon_Food_yinerxianggushang", "bigIcon_Food_yinerxianggushang", 223, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 35, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Mushroom,
			EFoodFoodType.Soup
		}));
		_dataArray.Add(new FoodItem(112, 224, 7, 700, 4, 110, "icon_Food_yinerxianggushang", "bigIcon_Food_yinerxianggushang", 225, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 40, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Mushroom,
			EFoodFoodType.Vegetable,
			EFoodFoodType.Rice,
			EFoodFoodType.Soup
		}));
		_dataArray.Add(new FoodItem(113, 226, 7, 700, 5, 110, "icon_Food_yinerxianggushang", "bigIcon_Food_yinerxianggushang", 227, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 45, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Mushroom
		}));
		_dataArray.Add(new FoodItem(114, 228, 7, 700, 6, 110, "icon_Food_taijishang", "bigIcon_Food_taijishang", 229, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 50, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Mushroom,
			EFoodFoodType.Egg
		}));
		_dataArray.Add(new FoodItem(115, 230, 7, 700, 7, 110, "icon_Food_banyuechenjiang", "bigIcon_Food_banyuechenjiang", 231, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 55, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Mushroom,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(116, 232, 7, 700, 8, 110, "icon_Food_luohanzhai", "bigIcon_Food_luohanzhai", 233, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 60, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Mushroom,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(117, 234, 7, 700, 3, 117, "icon_Food_qinggongniangsun", "bigIcon_Food_qinggongniangsun", 235, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 40, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(118, 236, 7, 700, 4, 117, "icon_Food_qinggongniangsun", "bigIcon_Food_qinggongniangsun", 237, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 45, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(119, 238, 7, 700, 5, 117, "icon_Food_qinggongniangsun", "bigIcon_Food_qinggongniangsun", 239, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 50, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Vegetable
		}));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new FoodItem(120, 240, 7, 700, 6, 117, "icon_Food_litanggeng", "bigIcon_Food_litanggeng", 241, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 55, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(121, 242, 7, 700, 7, 117, "icon_Food_shisetougeng", "bigIcon_Food_shisetougeng", 243, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 60, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(122, 244, 7, 700, 8, 117, "icon_Food_shisuifeicuidɑ", "bigIcon_Food_shisuifeicuidɑ", 245, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 65, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(123, 246, 7, 700, 4, 123, "icon_Food_lianzitougeng", "bigIcon_Food_lianzitougeng", 247, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 50, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(124, 248, 7, 700, 5, 123, "icon_Food_lianzitougeng", "bigIcon_Food_lianzitougeng", 249, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 55, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Vegetable,
			EFoodFoodType.Rice
		}));
		_dataArray.Add(new FoodItem(125, 250, 7, 700, 6, 123, "icon_Food_lianrongsutuo", "bigIcon_Food_lianrongsutuo", 251, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 60, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Vegetable,
			EFoodFoodType.Noodle
		}));
		_dataArray.Add(new FoodItem(126, 252, 7, 700, 7, 123, "icon_Food_bingtangxianglian", "bigIcon_Food_bingtangxianglian", 253, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 65, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(127, 254, 7, 700, 8, 123, "icon_Food_qibaoqizi", "bigIcon_Food_qibaoqizi", 255, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 70, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Vegetable,
			EFoodFoodType.Rice
		}));
		_dataArray.Add(new FoodItem(128, 256, 7, 700, 5, 128, "icon_Food_yurongbaiguo", "bigIcon_Food_yurongbaiguo", 257, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 60), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(129, 258, 7, 700, 6, 128, "icon_Food_yanwoyinxingyu", "bigIcon_Food_yanwoyinxingyu", 259, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 65), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(130, 260, 7, 700, 7, 128, "icon_Food_jinbozhenzhugeng", "bigIcon_Food_jinbozhenzhugeng", 261, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 70), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(131, 262, 7, 700, 8, 128, "icon_Food_shiliyinxing", "bigIcon_Food_shiliyinxing", 263, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 75), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 125, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(132, 264, 7, 700, 6, 132, "icon_Food_taiqingtang", "bigIcon_Food_taiqingtang", 265, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 70, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Mushroom
		}));
		_dataArray.Add(new FoodItem(133, 266, 7, 700, 7, 132, "icon_Food_yudaihoutougeng", "bigIcon_Food_yudaihoutougeng", 267, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 75, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 125, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Mushroom
		}));
		_dataArray.Add(new FoodItem(134, 268, 7, 700, 8, 132, "icon_Food_qiongjiangbaiyuantou", "bigIcon_Food_qiongjiangbaiyuantou", 269, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 80, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Vegetarian,
			EFoodFoodType.Mushroom
		}));
		_dataArray.Add(new FoodItem(135, 270, 7, 701, 0, 135, "icon_Food_luandunyupian", "bigIcon_Food_luandunyupian", 271, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 50, 25, 0, 2, 600, 3, allowRandomCreate: true, 50, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 10), 0, 0, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(136, 272, 7, 701, 1, 135, "icon_Food_luandunyupian", "bigIcon_Food_luandunyupian", 273, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 100, 50, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 15), 0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(137, 274, 7, 701, 2, 135, "icon_Food_luandunyupian", "bigIcon_Food_luandunyupian", 275, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 20), 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(138, 276, 7, 701, 3, 135, "icon_Food_huokaohulayu", "bigIcon_Food_huokaohulayu", 277, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 25), 0, 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(139, 278, 7, 701, 4, 135, "icon_Food_huokaohulayu", "bigIcon_Food_huokaohulayu", 279, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 30), 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Wine
		}));
		_dataArray.Add(new FoodItem(140, 280, 7, 701, 5, 135, "icon_Food_huokaohulayu", "bigIcon_Food_huokaohulayu", 281, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 35), 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(141, 282, 7, 701, 6, 135, "icon_Food_shengzhigusuyu", "bigIcon_Food_shengzhigusuyu", 283, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 40), 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(142, 284, 7, 701, 7, 135, "icon_Food_xihucuyu", "bigIcon_Food_xihucuyu", 285, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 45), 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(143, 286, 7, 701, 8, 135, "icon_Food_dujuanzuiyu", "bigIcon_Food_dujuanzuiyu", 287, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 50), 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(144, 288, 7, 701, 1, 144, "icon_Food_baizhuoha", "bigIcon_Food_baizhuoha", 289, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 100, 50, 0, 4, 1200, 4, allowRandomCreate: true, 45, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 20, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(145, 290, 7, 701, 2, 144, "icon_Food_baizhuoha", "bigIcon_Food_baizhuoha", 291, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 25, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Wine
		}));
		_dataArray.Add(new FoodItem(146, 292, 7, 701, 3, 144, "icon_Food_gailaha", "bigIcon_Food_gailaha", 293, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 30, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(147, 294, 7, 701, 4, 144, "icon_Food_gailaha", "bigIcon_Food_gailaha", 295, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 35, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(148, 296, 7, 701, 5, 144, "icon_Food_gailaha", "bigIcon_Food_gailaha", 297, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 40, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(149, 298, 7, 701, 6, 144, "icon_Food_cuanwangchaoqingxia", "bigIcon_Food_cuanwangchaoqingxia", 299, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 45, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(150, 300, 7, 701, 7, 144, "icon_Food_qunxiangeng", "bigIcon_Food_qunxiangeng", 301, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 50, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(151, 302, 7, 701, 8, 144, "icon_Food_longjingxiaren", "bigIcon_Food_longjingxiaren", 303, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 55, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Tea
		}));
		_dataArray.Add(new FoodItem(152, 304, 7, 701, 2, 152, "icon_Food_liyukuai", "bigIcon_Food_liyukuai", 305, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 300, 150, 0, 6, 1800, 5, allowRandomCreate: true, 40, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 30, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(153, 306, 7, 701, 3, 152, "icon_Food_ganshaoyanli", "bigIcon_Food_ganshaoyanli", 307, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 35, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(154, 308, 7, 701, 4, 152, "icon_Food_ganshaoyanli", "bigIcon_Food_ganshaoyanli", 309, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 40, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(155, 310, 7, 701, 5, 152, "icon_Food_ganshaoyanli", "bigIcon_Food_ganshaoyanli", 311, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 45, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(156, 312, 7, 701, 6, 152, "icon_Food_songshuliyu", "bigIcon_Food_songshuliyu", 313, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 50, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(157, 314, 7, 701, 7, 152, "icon_Food_baihuayudu", "bigIcon_Food_baihuayudu", 315, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 55, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(158, 316, 7, 701, 8, 152, "icon_Food_longxuhui", "bigIcon_Food_longxuhui", 317, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 60, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(159, 318, 7, 701, 3, 159, "icon_Food_qingzhengxie", "bigIcon_Food_qingzhengxie", 319, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 750, 375, 0, 8, 3000, 6, allowRandomCreate: true, 35, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 40), 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(160, 320, 7, 701, 4, 159, "icon_Food_qingzhengxie", "bigIcon_Food_qingzhengxie", 321, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 45), 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(161, 322, 7, 701, 5, 159, "icon_Food_qingzhengxie", "bigIcon_Food_qingzhengxie", 323, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 50), 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(162, 324, 7, 701, 6, 159, "icon_Food_xieniangcheng", "bigIcon_Food_xieniangcheng", 325, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 55), 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(163, 326, 7, 701, 7, 159, "icon_Food_naixianghexie", "bigIcon_Food_naixianghexie", 327, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 60), 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Fruit
		}));
		_dataArray.Add(new FoodItem(164, 328, 7, 701, 8, 159, "icon_Food_chengcuxishouxie", "bigIcon_Food_chengcuxishouxie", 329, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 20, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 0, 65), 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(165, 330, 7, 701, 4, 165, "icon_Food_luyukuai", "bigIcon_Food_luyukuai", 331, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 1550, 775, 1, 10, 4200, 7, allowRandomCreate: true, 30, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 50, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(166, 332, 7, 701, 5, 165, "icon_Food_luyukuai", "bigIcon_Food_luyukuai", 333, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 55, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(167, 334, 7, 701, 6, 165, "icon_Food_cuanluyuqinggeng", "bigIcon_Food_cuanluyuqinggeng", 335, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 60, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(168, 336, 7, 701, 7, 165, "icon_Food_saixiegeng", "bigIcon_Food_saixiegeng", 337, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 65, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Mushroom,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(169, 338, 7, 701, 8, 165, "icon_Food_tihuqizhenlu", "bigIcon_Food_tihuqizhenlu", 339, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 70, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Mushroom
		}));
		_dataArray.Add(new FoodItem(170, 340, 7, 701, 5, 170, "icon_Food_yuankebaoyu", "bigIcon_Food_yuankebaoyu", 341, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 2800, 1400, 2, 12, 5400, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 60, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(171, 342, 7, 701, 6, 170, "icon_Food_jiangjiujueming", "bigIcon_Food_jiangjiujueming", 343, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 65, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Wine
		}));
		_dataArray.Add(new FoodItem(172, 344, 7, 701, 7, 170, "icon_Food_baibɑsibao", "bigIcon_Food_baibɑsibao", 345, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 70, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Bird,
			EFoodFoodType.Vegetable
		}));
		_dataArray.Add(new FoodItem(173, 346, 7, 701, 8, 170, "icon_Food_fotiaoqiang", "bigIcon_Food_fotiaoqiang", 347, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 50, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 75, 0, 0, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 125, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Mushroom
		}));
		_dataArray.Add(new FoodItem(174, 348, 7, 701, 6, 174, "icon_Food_xianhuanglu", "bigIcon_Food_xianhuanglu", 349, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 4600, 2300, 3, 14, 7200, 8, allowRandomCreate: true, 20, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 70, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 105, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
		_dataArray.Add(new FoodItem(175, 350, 7, 701, 7, 174, "icon_Food_meixueshenglongpian", "bigIcon_Food_meixueshenglongpian", 351, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 30, 7050, 3525, 4, 16, 9000, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 75, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 125, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish,
			EFoodFoodType.Flower
		}));
		_dataArray.Add(new FoodItem(176, 352, 7, 701, 8, 174, "icon_Food_shaoqinhuangyugu", "bigIcon_Food_shaoqinhuangyugu", 353, transferable: true, stackable: true, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 0, 40, 10250, 5125, 5, 18, 10800, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 3, 38, 1, 1, new MainAttributes(0, 0, 0, 0, 80, 0), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new List<EFoodFoodType>
		{
			EFoodFoodType.Meat,
			EFoodFoodType.Fish
		}));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<FoodItem>(177);
		CreateItems0();
		CreateItems1();
		CreateItems2();
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
