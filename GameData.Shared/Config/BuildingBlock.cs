using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class BuildingBlock : ConfigData<BuildingBlockItem, short>
{
	public static class DefKey
	{
		public const short EmptyBlock = 0;

		public const short NormalResourceBegin = 1;

		public const short SpecialResourceBegin = 11;

		public const short Xuansheku = 16;

		public const short Qixiangyuan = 17;

		public const short UselessResourceBegin = 21;

		public const short Ruins = 23;

		public const short TaiwuVillage = 44;

		public const short TaiwuShrine = 45;

		public const short Residence = 46;

		public const short ComfortableHouse = 47;

		public const short Warehouse = 48;

		public const short ChickenCoop = 49;

		public const short SamsaraPlatform = 50;

		public const short TeaHorseCaravan = 51;

		public const short KungfuPracticeRoom = 52;

		public const short IceWall = 56;

		public const short PhoenixPlatform = 91;

		public const short StrategyRoom = 98;

		public const short BookCollectionRoom = 105;

		public const short MakeupRoom = 112;

		public const short BirthDeathStreamer = 119;

		public const short TeaHouse = 121;

		public const short Tavern = 122;

		public const short FourSeasMansion = 124;

		public const short TeaPlantation = 127;

		public const short Distillery = 128;

		public const short ForgingRoom = 129;

		public const short WoodworkingRoom = 139;

		public const short MedicineRoom = 149;

		public const short Hospital = 152;

		public const short ToxicologyRoom = 159;

		public const short PoisonHospital = 162;

		public const short WeavingRoom = 169;

		public const short JadeRoom = 179;

		public const short LifeElixirRoom = 195;

		public const short SutraReadingRoom = 202;

		public const short Kitchen = 203;

		public const short GamblingHouse = 215;

		public const short Brothel = 216;

		public const short RecruitBeauty = 217;

		public const short Pawnshop = 222;

		public const short ExcellentPersonShop = 223;

		public const short Jingcheng = 224;

		public const short Chengdu = 225;

		public const short Guizhou = 226;

		public const short Xiangyang = 227;

		public const short Taiyuan = 228;

		public const short Guangzhou = 229;

		public const short Qingzhou = 230;

		public const short Jiangling = 231;

		public const short Fuzhou = 232;

		public const short LiaoYang = 233;

		public const short Qinzhou = 234;

		public const short Dali = 235;

		public const short Shouchun = 236;

		public const short Hangzhou = 237;

		public const short Yangzhou = 238;

		public const short Shaolin = 239;

		public const short Emei = 240;

		public const short Baihua = 241;

		public const short Wudang = 242;

		public const short Yuanshan = 243;

		public const short Shixiang = 244;

		public const short Ranshan = 245;

		public const short Xuannv = 246;

		public const short Zhujian = 247;

		public const short Kongsang = 248;

		public const short Jingang = 249;

		public const short Wuxian = 250;

		public const short Jieqing = 251;

		public const short Fulong = 252;

		public const short Xuehou = 253;

		public const short Cunzhuang = 254;

		public const short Shizhen = 255;

		public const short Guanzhai = 256;

		public const short BambooHouse1 = 257;

		public const short BambooHouse2 = 258;

		public const short ShaolinSpecialBuilding = 259;

		public const short EmeiSpecialBuilding = 260;

		public const short BaihuaSpecialBuilding = 261;

		public const short WudangSpecialBuilding = 262;

		public const short YuanshanSpecialBuilding = 263;

		public const short ShixiangSpecialBuilding = 264;

		public const short RanshanSpecialBuilding = 265;

		public const short XuannvSpecialBuilding = 266;

		public const short ZhujianSpecialBuilding = 267;

		public const short KongsangSpecialBuilding = 268;

		public const short JingangSpecialBuilding = 269;

		public const short WuxianSpecialBuilding = 270;

		public const short JieqingSpecialBuilding = 271;

		public const short FulongSpecialBuilding = 272;

		public const short XuehouSpecialBuilding = 273;

		public const short MerchantBuildingBegin = 274;

		public const short MerchantBuildingEnd = 280;

		public const short Academy = 281;

		public const short MilitaryAcademy = 282;

		public const short BodhidharmaStupa = 283;

		public const short TreasuryZhucheng = 284;

		public const short TreasuryShizhen = 285;

		public const short TreasuryCunzhuang = 286;

		public const short TreasuryGuanzhai = 287;

		public const short TreasuryShaolin = 288;

		public const short TreasuryEmei = 289;

		public const short TreasuryBaihua = 290;

		public const short TreasuryWudang = 291;

		public const short TreasuryYuanshan = 292;

		public const short TreasuryShixiang = 293;

		public const short TreasuryRanshan = 294;

		public const short TreasuryXuannv = 295;

		public const short TreasuryZhujian = 296;

		public const short TreasuryKongsang = 297;

		public const short TreasuryJingang = 298;

		public const short TreasuryWuxian = 299;

		public const short TreasuryJieqing = 300;

		public const short TreasuryFulong = 301;

		public const short TreasuryXuehou = 302;

		public const short PrisonShaolin = 303;

		public const short PrisonEmei = 304;

		public const short PrisonBaihua = 305;

		public const short PrisonWudang = 306;

		public const short PrisonYuanshan = 307;

		public const short PrisonShixiang = 308;

		public const short PrisonRanshan = 309;

		public const short PrisonXuannv = 310;

		public const short PrisonZhujian = 311;

		public const short PrisonKongsang = 312;

		public const short PrisonJingang = 313;

		public const short PrisonWuxian = 314;

		public const short PrisonJieqing = 315;

		public const short PrisonFulong = 316;

		public const short PrisonXuehou = 317;

		public const short WuHuZhenBao = 318;

		public const short QiWenXingTai = 319;
	}

	public static class DefValue
	{
		public static BuildingBlockItem EmptyBlock => Instance[(short)0];

		public static BuildingBlockItem NormalResourceBegin => Instance[(short)1];

		public static BuildingBlockItem SpecialResourceBegin => Instance[(short)11];

		public static BuildingBlockItem Xuansheku => Instance[(short)16];

		public static BuildingBlockItem Qixiangyuan => Instance[(short)17];

		public static BuildingBlockItem UselessResourceBegin => Instance[(short)21];

		public static BuildingBlockItem Ruins => Instance[(short)23];

		public static BuildingBlockItem TaiwuVillage => Instance[(short)44];

		public static BuildingBlockItem TaiwuShrine => Instance[(short)45];

		public static BuildingBlockItem Residence => Instance[(short)46];

		public static BuildingBlockItem ComfortableHouse => Instance[(short)47];

		public static BuildingBlockItem Warehouse => Instance[(short)48];

		public static BuildingBlockItem ChickenCoop => Instance[(short)49];

		public static BuildingBlockItem SamsaraPlatform => Instance[(short)50];

		public static BuildingBlockItem TeaHorseCaravan => Instance[(short)51];

		public static BuildingBlockItem KungfuPracticeRoom => Instance[(short)52];

		public static BuildingBlockItem IceWall => Instance[(short)56];

		public static BuildingBlockItem PhoenixPlatform => Instance[(short)91];

		public static BuildingBlockItem StrategyRoom => Instance[(short)98];

		public static BuildingBlockItem BookCollectionRoom => Instance[(short)105];

		public static BuildingBlockItem MakeupRoom => Instance[(short)112];

		public static BuildingBlockItem BirthDeathStreamer => Instance[(short)119];

		public static BuildingBlockItem TeaHouse => Instance[(short)121];

		public static BuildingBlockItem Tavern => Instance[(short)122];

		public static BuildingBlockItem FourSeasMansion => Instance[(short)124];

		public static BuildingBlockItem TeaPlantation => Instance[(short)127];

		public static BuildingBlockItem Distillery => Instance[(short)128];

		public static BuildingBlockItem ForgingRoom => Instance[(short)129];

		public static BuildingBlockItem WoodworkingRoom => Instance[(short)139];

		public static BuildingBlockItem MedicineRoom => Instance[(short)149];

		public static BuildingBlockItem Hospital => Instance[(short)152];

		public static BuildingBlockItem ToxicologyRoom => Instance[(short)159];

		public static BuildingBlockItem PoisonHospital => Instance[(short)162];

		public static BuildingBlockItem WeavingRoom => Instance[(short)169];

		public static BuildingBlockItem JadeRoom => Instance[(short)179];

		public static BuildingBlockItem LifeElixirRoom => Instance[(short)195];

		public static BuildingBlockItem SutraReadingRoom => Instance[(short)202];

		public static BuildingBlockItem Kitchen => Instance[(short)203];

		public static BuildingBlockItem GamblingHouse => Instance[(short)215];

		public static BuildingBlockItem Brothel => Instance[(short)216];

		public static BuildingBlockItem RecruitBeauty => Instance[(short)217];

		public static BuildingBlockItem Pawnshop => Instance[(short)222];

		public static BuildingBlockItem ExcellentPersonShop => Instance[(short)223];

		public static BuildingBlockItem Jingcheng => Instance[(short)224];

		public static BuildingBlockItem Chengdu => Instance[(short)225];

		public static BuildingBlockItem Guizhou => Instance[(short)226];

		public static BuildingBlockItem Xiangyang => Instance[(short)227];

		public static BuildingBlockItem Taiyuan => Instance[(short)228];

		public static BuildingBlockItem Guangzhou => Instance[(short)229];

		public static BuildingBlockItem Qingzhou => Instance[(short)230];

		public static BuildingBlockItem Jiangling => Instance[(short)231];

		public static BuildingBlockItem Fuzhou => Instance[(short)232];

		public static BuildingBlockItem LiaoYang => Instance[(short)233];

		public static BuildingBlockItem Qinzhou => Instance[(short)234];

		public static BuildingBlockItem Dali => Instance[(short)235];

		public static BuildingBlockItem Shouchun => Instance[(short)236];

		public static BuildingBlockItem Hangzhou => Instance[(short)237];

		public static BuildingBlockItem Yangzhou => Instance[(short)238];

		public static BuildingBlockItem Shaolin => Instance[(short)239];

		public static BuildingBlockItem Emei => Instance[(short)240];

		public static BuildingBlockItem Baihua => Instance[(short)241];

		public static BuildingBlockItem Wudang => Instance[(short)242];

		public static BuildingBlockItem Yuanshan => Instance[(short)243];

		public static BuildingBlockItem Shixiang => Instance[(short)244];

		public static BuildingBlockItem Ranshan => Instance[(short)245];

		public static BuildingBlockItem Xuannv => Instance[(short)246];

		public static BuildingBlockItem Zhujian => Instance[(short)247];

		public static BuildingBlockItem Kongsang => Instance[(short)248];

		public static BuildingBlockItem Jingang => Instance[(short)249];

		public static BuildingBlockItem Wuxian => Instance[(short)250];

		public static BuildingBlockItem Jieqing => Instance[(short)251];

		public static BuildingBlockItem Fulong => Instance[(short)252];

		public static BuildingBlockItem Xuehou => Instance[(short)253];

		public static BuildingBlockItem Cunzhuang => Instance[(short)254];

		public static BuildingBlockItem Shizhen => Instance[(short)255];

		public static BuildingBlockItem Guanzhai => Instance[(short)256];

		public static BuildingBlockItem BambooHouse1 => Instance[(short)257];

		public static BuildingBlockItem BambooHouse2 => Instance[(short)258];

		public static BuildingBlockItem ShaolinSpecialBuilding => Instance[(short)259];

		public static BuildingBlockItem EmeiSpecialBuilding => Instance[(short)260];

		public static BuildingBlockItem BaihuaSpecialBuilding => Instance[(short)261];

		public static BuildingBlockItem WudangSpecialBuilding => Instance[(short)262];

		public static BuildingBlockItem YuanshanSpecialBuilding => Instance[(short)263];

		public static BuildingBlockItem ShixiangSpecialBuilding => Instance[(short)264];

		public static BuildingBlockItem RanshanSpecialBuilding => Instance[(short)265];

		public static BuildingBlockItem XuannvSpecialBuilding => Instance[(short)266];

		public static BuildingBlockItem ZhujianSpecialBuilding => Instance[(short)267];

		public static BuildingBlockItem KongsangSpecialBuilding => Instance[(short)268];

		public static BuildingBlockItem JingangSpecialBuilding => Instance[(short)269];

		public static BuildingBlockItem WuxianSpecialBuilding => Instance[(short)270];

		public static BuildingBlockItem JieqingSpecialBuilding => Instance[(short)271];

		public static BuildingBlockItem FulongSpecialBuilding => Instance[(short)272];

		public static BuildingBlockItem XuehouSpecialBuilding => Instance[(short)273];

		public static BuildingBlockItem MerchantBuildingBegin => Instance[(short)274];

		public static BuildingBlockItem MerchantBuildingEnd => Instance[(short)280];

		public static BuildingBlockItem Academy => Instance[(short)281];

		public static BuildingBlockItem MilitaryAcademy => Instance[(short)282];

		public static BuildingBlockItem BodhidharmaStupa => Instance[(short)283];

		public static BuildingBlockItem TreasuryZhucheng => Instance[(short)284];

		public static BuildingBlockItem TreasuryShizhen => Instance[(short)285];

		public static BuildingBlockItem TreasuryCunzhuang => Instance[(short)286];

		public static BuildingBlockItem TreasuryGuanzhai => Instance[(short)287];

		public static BuildingBlockItem TreasuryShaolin => Instance[(short)288];

		public static BuildingBlockItem TreasuryEmei => Instance[(short)289];

		public static BuildingBlockItem TreasuryBaihua => Instance[(short)290];

		public static BuildingBlockItem TreasuryWudang => Instance[(short)291];

		public static BuildingBlockItem TreasuryYuanshan => Instance[(short)292];

		public static BuildingBlockItem TreasuryShixiang => Instance[(short)293];

		public static BuildingBlockItem TreasuryRanshan => Instance[(short)294];

		public static BuildingBlockItem TreasuryXuannv => Instance[(short)295];

		public static BuildingBlockItem TreasuryZhujian => Instance[(short)296];

		public static BuildingBlockItem TreasuryKongsang => Instance[(short)297];

		public static BuildingBlockItem TreasuryJingang => Instance[(short)298];

		public static BuildingBlockItem TreasuryWuxian => Instance[(short)299];

		public static BuildingBlockItem TreasuryJieqing => Instance[(short)300];

		public static BuildingBlockItem TreasuryFulong => Instance[(short)301];

		public static BuildingBlockItem TreasuryXuehou => Instance[(short)302];

		public static BuildingBlockItem PrisonShaolin => Instance[(short)303];

		public static BuildingBlockItem PrisonEmei => Instance[(short)304];

		public static BuildingBlockItem PrisonBaihua => Instance[(short)305];

		public static BuildingBlockItem PrisonWudang => Instance[(short)306];

		public static BuildingBlockItem PrisonYuanshan => Instance[(short)307];

		public static BuildingBlockItem PrisonShixiang => Instance[(short)308];

		public static BuildingBlockItem PrisonRanshan => Instance[(short)309];

		public static BuildingBlockItem PrisonXuannv => Instance[(short)310];

		public static BuildingBlockItem PrisonZhujian => Instance[(short)311];

		public static BuildingBlockItem PrisonKongsang => Instance[(short)312];

		public static BuildingBlockItem PrisonJingang => Instance[(short)313];

		public static BuildingBlockItem PrisonWuxian => Instance[(short)314];

		public static BuildingBlockItem PrisonJieqing => Instance[(short)315];

		public static BuildingBlockItem PrisonFulong => Instance[(short)316];

		public static BuildingBlockItem PrisonXuehou => Instance[(short)317];

		public static BuildingBlockItem WuHuZhenBao => Instance[(short)318];

		public static BuildingBlockItem QiWenXingTai => Instance[(short)319];
	}

	public static BuildingBlock Instance = new BuildingBlock();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"BuildingCoreItem", "DependBuildings", "ExpandBuildings", "RequireLifeSkillType", "RequireCombatSkillType", "RequirePersonalityType", "BaseMaintenanceCost", "AddReadingLifeSkillBookEfficiency", "ReduceCombatSkillCost", "AddCombatSkillBreakout",
		"AddLifeSkillAttainment", "AddReadingLifeSkillBookFlash", "ReduceMakeRequirementLifeSkillType", "VillagerRoleTemplateIds", "SuccesEvent", "FailEvent", "IdleEvent", "MerchantId", "ExpandInfos", "BelongOrganization",
		"AvailableOrganization", "TemplateId", "Name", "Desc", "FuncDesc", "Icon", "FuncIcon", "Color", "LeaderName", "MemberName",
		"EffectDesc", "ApprovingRate"
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
		_dataArray.Add(new BuildingBlockItem(0, 0, 1, 2, null, null, EBuildingBlockType.Empty, EBuildingBlockClass.Invalid, 0, canOpenManageOutTaiwu: false, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), -1, -1, -1, 3, 4, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 5, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 1, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(1, 6, 7, 8, "SectSpecial/1_shuiyu", null, EBuildingBlockType.NormalResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 500, 0, 0, 0, 0, 0, 0, 0 }, 100, new sbyte[7] { 2, 0, 0, 0, 0, 0, 0 }, 333, new List<short>(), new List<short> { 24, 89, 125, 133, 217 }, 14, -1, -1, 9, 10, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 250 }, 5, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 0, 20, 33 }, 11, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(2, 12, 13, 14, "SectSpecial/2_shihan", null, EBuildingBlockType.NormalResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 500, 0, 0, 0, 0, 0 }, 100, new sbyte[7] { 0, 0, 2, 0, 0, 0, 0 }, 334, new List<short>(), new List<short> { 25, 103, 135, 136, 183 }, 6, -1, -1, 15, 16, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 250 }, 5, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 1, 21, 34 }, 17, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(3, 18, 19, 20, "SectSpecial/3_shulin", null, EBuildingBlockType.NormalResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 500, 0, 0, 0, 0, 0, 0 }, 100, new sbyte[7] { 0, 2, 0, 0, 0, 0, 0 }, 335, new List<short>(), new List<short> { 26, 145, 146 }, 7, -1, -1, 21, 22, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 250 }, 5, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 2, 22, 35, 36 }, 23, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(4, 24, 25, 26, "SectSpecial/4_gufeng", null, EBuildingBlockType.NormalResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 250, 250, 0, 0, 0, 0 }, 100, new sbyte[7] { 0, 0, 1, 1, 0, 0, 0 }, 336, new List<short>(), new List<short> { 27, 54, 117, 193 }, 4, -1, -1, 27, 28, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 250 }, 5, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 3, 23, 24, 37 }, 29, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(5, 30, 31, 32, "SectSpecial/5_caoyao", null, EBuildingBlockType.NormalResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 0, 0, 0, 500, 0, 0 }, 100, new sbyte[7] { 0, 0, 0, 0, 0, 2, 0 }, 337, new List<short>(), new List<short> { 28, 155, 156, 200 }, 8, -1, -1, 33, 34, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 250 }, 5, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 4, 25, 38 }, 35, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(6, 36, 37, 38, "SectSpecial/6_duzhao", null, EBuildingBlockType.NormalResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 0, 0, 0, 500, 0, 0 }, 100, new sbyte[7] { 0, 0, 0, 0, 0, 0, 2 }, 338, new List<short>(), new List<short> { 29, 143, 165, 166 }, 9, -1, -1, 39, 40, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 250 }, 5, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 5, 26, 39 }, 41, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(7, 42, 43, 44, "SectSpecial/7_huagu", null, EBuildingBlockType.NormalResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 0, 0, 500, 0, 0, 0 }, 100, new sbyte[7] { 0, 0, 0, 0, 2, 0, 0 }, 342, new List<short>(), new List<short> { 30, 110, 175, 176, 220 }, 10, -1, -1, 45, 46, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 250 }, 5, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 6, 27, 40, 41 }, 47, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(8, 48, 49, 50, "SectSpecial/8_baoshi", null, EBuildingBlockType.NormalResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 0, 500, 0, 0, 0, 0 }, 100, new sbyte[7] { 0, 0, 0, 2, 0, 0, 0 }, 339, new List<short>(), new List<short> { 31, 96, 173, 185, 186 }, 11, -1, -1, 51, 52, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 250 }, 5, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 7, 28, 42 }, 53, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(9, 54, 55, 56, "SectSpecial/9_woye", null, EBuildingBlockType.NormalResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 250, 250, 0, 0, 0, 0, 0, 0 }, 100, new sbyte[7] { 1, 1, 0, 0, 0, 0, 0 }, 340, new List<short>(), new List<short> { 32, 127, 128, 209, 210 }, 14, -1, -1, 57, 58, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 250 }, 5, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 8, 29, 30, 43 }, 59, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(10, 60, 61, 62, "SectSpecial/10_shouqun", null, EBuildingBlockType.NormalResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 250, 0, 0, 0, 250, 0, 0, 0 }, 100, new sbyte[7] { 1, 0, 0, 0, 1, 0, 0 }, 341, new List<short>(), new List<short> { 33, 153, 163, 207 }, 15, -1, -1, 63, 64, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 250 }, 5, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 9, 31, 32, 44 }, 65, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(11, 66, 67, 68, "SectSpecial/101_huochi", null, EBuildingBlockType.SpecialResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 1000, 0, 0, 0, 0, 0 }, 100, new sbyte[7] { 0, 0, 3, 0, 0, 0, 0 }, 364, new List<short>(), new List<short> { 34, 138, 184 }, 6, -1, -1, 69, 70, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 500 }, 10, new List<ResourceInfo>(), 100, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 10, 45 }, 71, -1, new string[2] { "buildingarea_industry_icon_6", "buildingarea_industry_base_6" }, new string[2] { "buildingarea_industry_icon_6", "buildingarea_detailed_base_6" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(12, 72, 73, 74, "SectSpecial/102_yuntiekeng", null, EBuildingBlockType.SpecialResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 1000, 0, 0, 0, 0, 0 }, 100, new sbyte[7] { 0, 0, 3, 0, 0, 0, 0 }, 365, new List<short>(), new List<short> { 35, 134, 208 }, 6, -1, -1, 75, 76, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 500 }, 10, new List<ResourceInfo>(), 100, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 11, 46 }, 77, -1, new string[2] { "buildingarea_industry_icon_6", "buildingarea_industry_base_6" }, new string[2] { "buildingarea_industry_icon_6", "buildingarea_detailed_base_6" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(13, 78, 79, 80, "SectSpecial/103_milin", null, EBuildingBlockType.SpecialResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 1000, 0, 0, 0, 0, 0, 0 }, 100, new sbyte[7] { 0, 3, 0, 0, 0, 0, 0 }, 366, new List<short>(), new List<short> { 36, 144, 164 }, 7, -1, -1, 81, 82, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 500 }, 10, new List<ResourceInfo>(), 100, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 12, 47 }, 83, -1, new string[2] { "buildingarea_industry_icon_6", "buildingarea_industry_base_6" }, new string[2] { "buildingarea_industry_icon_6", "buildingarea_detailed_base_6" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(14, 84, 85, 86, "SectSpecial/104_taishilin", null, EBuildingBlockType.SpecialResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 1000, 0, 0, 0, 0, 0, 0 }, 100, new sbyte[7] { 0, 3, 0, 0, 0, 0, 0 }, 367, new List<short>(), new List<short> { 37, 148, 201 }, 7, -1, -1, 87, 88, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 500 }, 10, new List<ResourceInfo>(), 100, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 13, 48 }, 89, -1, new string[2] { "buildingarea_industry_icon_6", "buildingarea_industry_base_6" }, new string[2] { "buildingarea_industry_icon_6", "buildingarea_detailed_base_6" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(15, 90, 91, 92, "SectSpecial/105_changchunjian", null, EBuildingBlockType.SpecialResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 0, 0, 0, 1000, 0, 0 }, 100, new sbyte[7] { 0, 0, 0, 0, 0, 3, 0 }, 368, new List<short>(), new List<short> { 38, 90, 126, 158, 221 }, 8, -1, -1, 93, 94, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 500 }, 10, new List<ResourceInfo>(), 100, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 14, 49 }, 95, -1, new string[2] { "buildingarea_industry_icon_6", "buildingarea_industry_base_6" }, new string[2] { "buildingarea_industry_icon_6", "buildingarea_detailed_base_6" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(16, 96, 97, 98, "SectSpecial/106_xuansheku", null, EBuildingBlockType.SpecialResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 0, 0, 0, 1000, 0, 0 }, 100, new sbyte[7] { 0, 0, 0, 0, 0, 0, 3 }, 369, new List<short>(), new List<short> { 39, 168, 174 }, 9, -1, -1, 99, 100, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 500 }, 10, new List<ResourceInfo>(), 100, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 15, 50 }, 101, -1, new string[2] { "buildingarea_industry_icon_6", "buildingarea_industry_base_6" }, new string[2] { "buildingarea_industry_icon_6", "buildingarea_detailed_base_6" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(17, 102, 103, 104, "SectSpecial/107_qixiangyuan", null, EBuildingBlockType.SpecialResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 0, 0, 1000, 0, 0, 0 }, 100, new sbyte[7] { 0, 0, 0, 0, 3, 0, 0 }, 370, new List<short>(), new List<short> { 40, 111, 178 }, 10, -1, -1, 105, 106, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 500 }, 10, new List<ResourceInfo>(), 100, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 16, 51 }, 107, -1, new string[2] { "buildingarea_industry_icon_6", "buildingarea_industry_base_6" }, new string[2] { "buildingarea_industry_icon_6", "buildingarea_detailed_base_6" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(18, 108, 109, 110, "SectSpecial/108_fengshuilongxue", null, EBuildingBlockType.SpecialResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 0, 0, 1000, 0, 0, 0 }, 100, new sbyte[7] { 0, 0, 0, 0, 3, 0, 0 }, 371, new List<short>(), new List<short> { 41, 118, 194 }, 4, -1, -1, 111, 112, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 500 }, 10, new List<ResourceInfo>(), 100, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 17, 52 }, 113, -1, new string[2] { "buildingarea_industry_icon_6", "buildingarea_industry_base_6" }, new string[2] { "buildingarea_industry_icon_6", "buildingarea_detailed_base_6" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(19, 114, 115, 116, "SectSpecial/109_lingmai", null, EBuildingBlockType.SpecialResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 0, 1000, 0, 0, 0, 0 }, 100, new sbyte[7] { 0, 0, 0, 3, 0, 0, 0 }, 372, new List<short>(), new List<short> { 42, 104, 188 }, 11, -1, -1, 117, 118, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 500 }, 10, new List<ResourceInfo>(), 100, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 18, 53 }, 119, -1, new string[2] { "buildingarea_industry_icon_6", "buildingarea_industry_base_6" }, new string[2] { "buildingarea_industry_icon_6", "buildingarea_detailed_base_6" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(20, 120, 121, 122, "SectSpecial/110_xuanbing", null, EBuildingBlockType.SpecialResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 0, 1000, 0, 0, 0, 0 }, 100, new sbyte[7] { 0, 0, 0, 3, 0, 0, 0 }, 373, new List<short>(), new List<short> { 43, 56, 97, 154, 212 }, 11, -1, -1, 123, 124, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 500 }, 10, new List<ResourceInfo>(), 100, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 19, 54 }, 125, -1, new string[2] { "buildingarea_industry_icon_6", "buildingarea_industry_base_6" }, new string[2] { "buildingarea_industry_icon_6", "buildingarea_detailed_base_6" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(21, 126, 127, 128, "SectSpecial/20001_zhachaodui", null, EBuildingBlockType.UselessResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 100, 100, 0, 0, 100, 100, 0, 0 }, 100, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 129, 130, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, 250 }, 10, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 131, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(22, 132, 133, 134, "SectSpecial/20002_luanshidui", null, EBuildingBlockType.UselessResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 200, 200, 0, 0, 0, 0 }, 100, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 135, 136, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, 250 }, 10, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 137, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(23, 138, 139, 140, "SectSpecial/20003_feixu", null, EBuildingBlockType.UselessResource, EBuildingBlockClass.BornResource, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 0, 0, 0, 0, 1000, 100 }, 100, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 141, 142, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, 250 }, 10, new List<ResourceInfo>(), 50, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 143, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(24, 144, 145, 146, "SectSpecial/10001_diyan", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 1000, 250, 0, 500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 1 }, new List<short>(), 14, -1, 6, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 20)
		}, 100, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1], isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 0 }, new List<short> { 20 }, -1, new List<ShortList>(), -1, new List<short> { 55 }, 149, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(25, 150, 151, 152, "SectSpecial/10002_kuangjing", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 1000, 500, 0, 250, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 2 }, new List<short>(), 6, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 20)
		}, 100, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 1 }, new List<short> { 21 }, -1, new List<ShortList>(), -1, new List<short> { 56 }, 153, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(26, 154, 155, 156, "SectSpecial/10003_shunong", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 500, 250, 0, 1000, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 3 }, new List<short>(), 7, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 20)
		}, 100, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 2 }, new List<short> { 22 }, -1, new List<ShortList>(), -1, new List<short> { 57 }, 157, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(27, 158, 159, 160, "SectSpecial/10004_shibei", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 250, 1000, 500, 0, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 4 }, new List<short>(), 4, -1, 5, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 20)
		}, 100, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 6 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 3 }, new List<short> { 23 }, -1, new List<ShortList>(), -1, new List<short> { 58 }, 161, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(28, 162, 163, 164, "SectSpecial/10005_yaonong", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 500, 0, 0, 1000, 250, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 5 }, new List<short>(), 8, -1, 0, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 20)
		}, 100, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 4 }, new List<short> { 24 }, -1, new List<ShortList>(), -1, new List<short> { 59 }, 165, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(29, 166, 167, 168, "SectSpecial/10006_niqu", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 500, 1000, 0, 0, 0, 250, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 6 }, new List<short>(), 9, -1, 0, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 20)
		}, 100, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 5 }, new List<short> { 25 }, -1, new List<ShortList>(), -1, new List<short> { 60 }, 169, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(30, 170, 171, 172, "SectSpecial/10007_huanong", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 1000, 0, 500, 250, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 7 }, new List<short>(), 10, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 20)
		}, 100, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 6 }, new List<short> { 26 }, -1, new List<ShortList>(), -1, new List<short> { 61 }, 173, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(31, 174, 175, 176, "SectSpecial/10008_baojing", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 500, 1000, 250, 0, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 8 }, new List<short>(), 11, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 20)
		}, 100, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 7 }, new List<short> { 27 }, -1, new List<ShortList>(), -1, new List<short> { 62 }, 177, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(32, 178, 179, 180, "SectSpecial/10009_tongche", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 1000, 250, 0, 500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 9 }, new List<short>(), 14, -1, 6, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 20)
		}, 100, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1], isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 8 }, new List<short> { 28 }, -1, new List<ShortList>(), -1, new List<short> { 63 }, 181, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(33, 182, 183, 184, "SectSpecial/10010_muchang", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 1000, 250, 0, 0, 500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 10 }, new List<short>(), 14, -1, 2, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 20)
		}, 100, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1], isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 9 }, new List<short> { 29 }, -1, new List<ShortList>(), -1, new List<short> { 64 }, 185, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(34, 186, 187, 188, "SectSpecial/11001_liulisuo", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 2500, 7500, 5000, 0, 0, 0, 1000 }, 50, new sbyte[7], 92, new List<short> { 11 }, new List<short>(), 6, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 10 }, new List<short> { 30 }, -1, new List<ShortList>(), -1, new List<short> { 65 }, 189, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(35, 190, 191, 192, "SectSpecial/11002_huobaodui", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 7500, 0, 0, 2500, 5000, 0, 1000 }, 50, new sbyte[7], 93, new List<short> { 12 }, new List<short>(), 6, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 11 }, new List<short> { 31 }, -1, new List<ShortList>(), -1, new List<short> { 66 }, 193, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(36, 194, 195, 196, "SectSpecial/11003_hulinqiang", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 7500, 2500, 0, 5000, 0, 0, 1000 }, 50, new sbyte[7], 94, new List<short> { 13 }, new List<short>(), 7, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 12 }, new List<short> { 32 }, -1, new List<ShortList>(), -1, new List<short> { 67 }, 197, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(37, 198, 199, 200, "SectSpecial/11004_xuankongzhan", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 5000, 2500, 0, 7500, 0, 0, 1000 }, 50, new sbyte[7], 95, new List<short> { 14 }, new List<short>(), 7, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 13 }, new List<short> { 33 }, -1, new List<ShortList>(), -1, new List<short> { 68 }, 201, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(38, 202, 203, 204, "SectSpecial/11005_yinjianqu", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 7500, 0, 5000, 2500, 0, 0, 1000 }, 50, new sbyte[7], 96, new List<short> { 15 }, new List<short>(), 8, -1, 0, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 14 }, new List<short> { 34 }, -1, new List<ShortList>(), -1, new List<short> { 69 }, 205, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(39, 206, 207, 208, "SectSpecial/11006_ershilao", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 7500, 0, 5000, 0, 0, 2500, 0, 1000 }, 50, new sbyte[7], 97, new List<short> { 16 }, new List<short>(), 9, -1, 0, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 15 }, new List<short> { 35 }, -1, new List<ShortList>(), -1, new List<short> { 70 }, 209, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(40, 210, 211, 212, "SectSpecial/11007_yunpeng", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 5000, 2500, 0, 7500, 0, 0, 1000 }, 50, new sbyte[7], 98, new List<short> { 17 }, new List<short>(), 10, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 16 }, new List<short> { 36 }, -1, new List<ShortList>(), -1, new List<short> { 71 }, 213, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(41, 214, 215, 216, "SectSpecial/11008_furenju", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 5000, 0, 0, 7500, 2500, 0, 0, 1000 }, 50, new sbyte[7], 99, new List<short> { 18 }, new List<short>(), 4, -1, 5, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 6 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 17 }, new List<short> { 37 }, -1, new List<ShortList>(), -1, new List<short> { 72 }, 217, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(42, 218, 219, 220, "SectSpecial/11009_miling", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 2500, 0, 7500, 0, 5000, 0, 1000 }, 50, new sbyte[7], 100, new List<short> { 19 }, new List<short>(), 11, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 18 }, new List<short> { 38 }, -1, new List<ShortList>(), -1, new List<short> { 73 }, 221, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(43, 222, 223, 224, "SectSpecial/11010_bingyixiang", "Func_ResourceUp", EBuildingBlockType.Building, EBuildingBlockClass.Resource, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 0, 7500, 5000, 2500, 0, 0, 1000 }, 50, new sbyte[7], 101, new List<short> { 20 }, new List<short>(), 11, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: true, 800, 0, 0, new List<short> { 19 }, new List<short> { 39 }, -1, new List<ShortList>(), -1, new List<short> { 74 }, 225, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(44, 226, 227, 228, "Main/1001_taiwucun", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 15, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 50, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 229, 230, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, 20, new List<ResourceInfo>(), 1000, mustMaintenance: true, isUnique: true, 1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 107, 108, 109 }, 231, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(45, 232, 233, 234, "SectSpecial/1005_taiwushicitang", null, EBuildingBlockType.Building, EBuildingBlockClass.Villiage, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 0, 500, 500, 500, 500, 0, 0, 500 }, 50, new sbyte[7], 197, new List<short>(), new List<short>(), 4, -1, 5, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, -1 }, 20, new List<ResourceInfo>(), 500, mustMaintenance: true, isUnique: true, 1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 6 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 111 }, 236, -1, new string[2] { "buildingarea_industry_icon_0", "buildingarea_industry_base_0" }, new string[2] { "buildingarea_industry_icon_0", "buildingarea_detailed_base_0" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(46, 237, 238, 239, "SectSpecial/1002_jusuo", null, EBuildingBlockType.Building, EBuildingBlockClass.Villiage, 9, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 500, 500, 500, 500, 500, 500, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 240, 241, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 250, 250 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(0, 10)
		}, 100, mustMaintenance: true, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 113 }, 242, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(47, 243, 244, 245, "SectSpecial/1003_xiangfang", null, EBuildingBlockType.Building, EBuildingBlockClass.Villiage, 3, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 2500, 2500, 2500, 2500, 2500, 2500, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 246, 247, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: true, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 114 }, 248, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(48, 249, 250, 251, "SectSpecial/1004_cangku", null, EBuildingBlockType.Building, EBuildingBlockClass.Villiage, 9, canOpenManageOutTaiwu: true, 1, null, new ushort[8] { 0, 1500, 1500, 0, 0, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 252, 253, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 250, 250 }, 10, new List<ResourceInfo>(), 100, mustMaintenance: true, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 112 }, 254, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(49, 255, 256, 257, "SectSpecial/1007_yuanjishe", null, EBuildingBlockType.Building, EBuildingBlockClass.Villiage, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8] { 500, 500, 500, 500, 500, 500, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 258, 259, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>(), 500, mustMaintenance: false, isUnique: false, 1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 110 }, 260, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(50, 261, 262, 263, "SectSpecial/2507_lunhuitai", "Func_Special", EBuildingBlockType.Building, EBuildingBlockClass.Villiage, 6, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 1500, 1500, 1500, 1500, 1500, 1500, 0, 500 }, 50, new sbyte[7], 199, new List<short>(), new List<short>(), 15, -1, -1, 264, 265, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, -1 }, 20, new List<ResourceInfo>(), 1000, mustMaintenance: false, isUnique: true, 1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 115 }, 266, -1, new string[2] { "buildingarea_industry_icon_0", "buildingarea_industry_base_0" }, new string[2] { "buildingarea_industry_icon_0", "buildingarea_detailed_base_0" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(51, 267, 268, 269, "SectSpecial/2610_chamabang", "Func_Special", EBuildingBlockType.Building, EBuildingBlockClass.Villiage, 20, canOpenManageOutTaiwu: false, 1, null, new ushort[8] { 2500, 2500, 2500, 2500, 2500, 2500, 10000, 1500 }, 50, new sbyte[7], 198, new List<short>(), new List<short>(), 15, -1, -1, 270, 271, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, -1 }, 20, new List<ResourceInfo>(), 2500, mustMaintenance: false, isUnique: true, 1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 116 }, 272, -1, new string[2] { "buildingarea_industry_icon_0", "buildingarea_industry_base_0" }, new string[2] { "buildingarea_industry_icon_0", "buildingarea_detailed_base_0" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(52, 273, 274, 275, "SectSpecial/1008_liangongfang", null, EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8] { 0, 500, 1000, 0, 500, 1000, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short>
		{
			53, 54, 55, 56, 57, 58, 59, 60, 61, 62,
			63, 64, 65, 66, 67, 68, 69, 70, 71, 72,
			73, 74, 75, 76, 77, 78, 79, 80, 81, 82,
			83, 84
		}, 15, -1, -1, 276, 277, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 250, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 278, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(53, 279, 280, 281, "SectSpecial/2030_biaoju", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 1500, 0, 2000, 0, 0, 1500, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), 4, -1, 5, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 6 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, -50, new List<short> { 40 }, new List<short> { 43 }, -1, new List<ShortList>(), -1, new List<short> { 76 }, 284, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(54, 285, 286, 287, "SectSpecial/2032_lianshenfeng", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 5000, 5000, 0, 0, 5000, 0, 2500 }, 50, new sbyte[7], 116, new List<short> { 52, 4 }, new List<short>(), 4, -1, 5, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 6 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 41 }, new List<short> { 44 }, -1, new List<ShortList>(), -1, new List<short> { 75 }, 288, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(55, 289, 290, 291, "SectSpecial/2031_hziketing", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 2500, 1500, 0, 1500, 2500, 0, 0, 1000 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), 4, -1, 5, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 6 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, -50, new List<short> { 42 }, new List<short> { 45 }, -1, new List<ShortList>(), -1, new List<short> { 77 }, 292, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(56, 293, 294, 295, "SectSpecial/2033_xuanbingbi", "Func_Special", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 5000, 10000, 20000, 5000, 0, 0, 15000 }, 50, new sbyte[7], 117, new List<short> { 52, 20 }, new List<short>(), 15, -1, -1, 296, 297, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 1000)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 78 }, 298, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(57, 299, 300, 301, "SectSpecial/2002_jingshi", "Func_CombatSkill0", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 2500, 0, 2500, 0, 5000, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 0, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			9, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 0, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 79 }, 302, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(58, 303, 304, 305, "SectSpecial/2003_zhuanglin", "Func_CombatSkill1", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 5000, 2500, 0, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 1, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, 9, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 80 }, 306, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(59, 307, 308, 309, "SectSpecial/2004_juezhitang", "Func_CombatSkill2", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 0, 0, 2500, 2500, 5000, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 2, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, 9, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 2, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 81 }, 310, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new BuildingBlockItem(60, 311, 312, 313, "SectSpecial/2005_murenzhen", "Func_CombatSkill3", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 5000, 2500, 0, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 3, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, 9, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 3, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 82 }, 314, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(61, 315, 316, 317, "SectSpecial/2006_tongrenzhen", "Func_CombatSkill4", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 2500, 5000, 0, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 4, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, 9, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 4, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 83 }, 318, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(62, 319, 320, 321, "SectSpecial/2007_heinitan", "Func_CombatSkill5", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 5000, 2500, 0, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 5, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, 9, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 5, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 84 }, 322, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(63, 323, 324, 325, "SectSpecial/2008_bachang", "Func_CombatSkill6", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 2500, 5000, 0, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 6, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, 9, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 6, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 85 }, 326, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(64, 327, 328, 329, "SectSpecial/2009_shijiantai", "Func_CombatSkill7", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 2500, 0, 5000, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 7, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, 9, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 7, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 86 }, 330, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(65, 331, 332, 333, "SectSpecial/2010_shendaotang", "Func_CombatSkill8", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 2500, 5000, 2500, 0, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 8, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, 9, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 8, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 87 }, 334, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(66, 335, 336, 337, "SectSpecial/2011_yanwuchang", "Func_CombatSkill9", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 2500, 5000, 0, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 9, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, 9,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 9, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 88 }, 338, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(67, 339, 340, 341, "SectSpecial/2012_yirenguan", "Func_CombatSkill10", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 5000, 0, 2500, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 10, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			9, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 10, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 89 }, 342, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(68, 343, 344, 345, "SectSpecial/2013_fengshi", "Func_CombatSkill11", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 2500, 0, 2500, 5000, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 11, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, 9, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 11, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 90 }, 346, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(69, 347, 348, 349, "SectSpecial/2014_tianjige", "Func_CombatSkill12", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 0, 2500, 5000, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 12, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, 9, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 12, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 91 }, 350, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(70, 351, 352, 353, "SectSpecial/2015_konggu", "Func_CombatSkill13", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 0, 0, 2500, 5000, 2500, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 52 }, new List<short>(), -1, 13, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, 9
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 1000, mustMaintenance: false, isUnique: true, 0, -1, 13, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 92 }, 354, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(71, 355, 356, 357, "SectSpecial/2016_geshita", "Func_CombatSkill0", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 5000, 0, 10000, 0, 15000, 0, 0 }, 50, new sbyte[7], 102, new List<short> { 52 }, new List<short>(), -1, 0, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			12, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 0, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 93 }, 358, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(72, 359, 360, 361, "SectSpecial/2017_lingyunjie", "Func_CombatSkill1", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 15000, 10000, 0, 5000, 0, 0, 0 }, 50, new sbyte[7], 103, new List<short> { 52 }, new List<short>(), -1, 1, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, 12, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 94 }, 362, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(73, 363, 364, 365, "SectSpecial/2018_mishi", "Func_CombatSkill2", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 0, 0, 5000, 10000, 15000, 0, 0 }, 50, new sbyte[7], 104, new List<short> { 52 }, new List<short>(), -1, 2, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, 12, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 2, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 95 }, 366, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(74, 367, 368, 369, "SectSpecial/2019_bohulao", "Func_CombatSkill3", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 15000, 10000, 0, 5000, 0, 0, 0 }, 50, new sbyte[7], 105, new List<short> { 52 }, new List<short>(), -1, 3, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, 12, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 3, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 96 }, 370, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(75, 371, 372, 373, "SectSpecial/2020_miantieerbi", "Func_CombatSkill4", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 5000, 15000, 0, 10000, 0, 0, 0 }, 50, new sbyte[7], 106, new List<short> { 52 }, new List<short>(), -1, 4, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, 12, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 4, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 97 }, 374, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(76, 375, 376, 377, "SectSpecial/2021_daoqiongdou", "Func_CombatSkill5", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 15000, 5000, 0, 10000, 0, 0, 0 }, 50, new sbyte[7], 107, new List<short> { 52 }, new List<short>(), -1, 5, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, 12, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 5, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 98 }, 378, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(77, 379, 380, 381, "SectSpecial/2022_anshi", "Func_CombatSkill6", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 10000, 15000, 0, 5000, 0, 0, 0 }, 50, new sbyte[7], 108, new List<short> { 52 }, new List<short>(), -1, 6, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, 12, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 6, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 99 }, 382, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(78, 383, 384, 385, "SectSpecial/2023_jianzhong", "Func_CombatSkill7", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 10000, 0, 15000, 5000, 0, 0, 0 }, 50, new sbyte[7], 109, new List<short> { 52 }, new List<short>(), -1, 7, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, 12, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 7, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 100 }, 386, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(79, 387, 388, 389, "SectSpecial/2024_xiuluochang", "Func_CombatSkill8", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 10000, 15000, 5000, 0, 0, 0, 0 }, 50, new sbyte[7], 110, new List<short> { 52 }, new List<short>(), -1, 8, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, 12, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 8, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 101 }, 390, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(80, 391, 392, 393, "SectSpecial/2025_tiejuzhen", "Func_CombatSkill9", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 10000, 15000, 0, 5000, 0, 0, 0 }, 50, new sbyte[7], 111, new List<short> { 52 }, new List<short>(), -1, 9, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, 12,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 9, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 102 }, 394, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(81, 395, 396, 397, "SectSpecial/2026_bazhentu", "Func_CombatSkill10", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 15000, 0, 10000, 5000, 0, 0, 0 }, 50, new sbyte[7], 112, new List<short> { 52 }, new List<short>(), -1, 10, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			12, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 10, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 103 }, 398, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(82, 399, 400, 401, "SectSpecial/2027_qiansixiang", "Func_CombatSkill11", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 10000, 0, 5000, 15000, 0, 0, 0 }, 50, new sbyte[7], 113, new List<short> { 52 }, new List<short>(), -1, 11, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, 12, -1, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 11, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 104 }, 402, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(83, 403, 404, 405, "SectSpecial/2028_zhuiyingdong", "Func_CombatSkill12", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 0, 5000, 15000, 10000, 0, 0, 0 }, 50, new sbyte[7], 114, new List<short> { 52 }, new List<short>(), -1, 12, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, 12, -1
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 12, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 105 }, 406, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(84, 407, 408, 409, "SectSpecial/2029_qixianlou", "Func_CombatSkill13", EBuildingBlockType.Building, EBuildingBlockClass.Kungfu, 1, canOpenManageOutTaiwu: false, 1, "473a32", new ushort[8] { 0, 0, 0, 10000, 15000, 5000, 0, 0 }, 50, new sbyte[7], 115, new List<short> { 52 }, new List<short>(), -1, 13, 3, 282, 283, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, 12
		}, new short[2] { 1500, 1500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, 13, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 106 }, 410, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(85, 411, 412, 413, "SectSpecial/1009_qinshe", null, EBuildingBlockType.Building, EBuildingBlockClass.Music, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8] { 0, 500, 0, 500, 1500, 0, 2500, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 86, 87, 88, 89, 90, 91 }, 0, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 250, mustMaintenance: false, isUnique: false, 0, 0, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 117 }, 414, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(86, 415, 416, 417, "SectSpecial/2102_yuefang", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Music, 1, canOpenManageOutTaiwu: false, 1, "406056", new ushort[8] { 0, 1250, 0, 1250, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 85 }, new List<short>(), 0, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 50, 0, new List<short> { 46 }, new List<short> { 49 }, -1, new List<ShortList>(), -1, new List<short> { 143 }, 418, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(87, 419, 420, 421, "SectSpecial/2103_zhiyinge", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Music, 1, canOpenManageOutTaiwu: false, 1, "406056", new ushort[8] { 0, 2500, 0, 2500, 5000, 0, 0, 2500 }, 50, new sbyte[7], 118, new List<short> { 85 }, new List<short>(), 0, -1, 1, 147, 235, new short[16]
		{
			12, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 47 }, new List<short> { 50 }, -1, new List<ShortList>(), -1, new List<short> { 168 }, 422, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(88, 423, 424, 425, "SectSpecial/2104_baixiyuan", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Music, 1, canOpenManageOutTaiwu: false, 1, "406056", new ushort[8] { 0, 2000, 0, 2000, 5000, 0, 5000, 0 }, 50, new sbyte[7], -1, new List<short> { 85 }, new List<short>(), 0, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 50, 0, new List<short> { 48 }, new List<short> { 51 }, -1, new List<ShortList>(), -1, new List<short> { 152 }, 426, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(89, 427, 428, 429, "SectSpecial/2105_yiqingju", "Func_LiveSkill0", EBuildingBlockType.Building, EBuildingBlockClass.Music, 1, canOpenManageOutTaiwu: false, 1, "406056", new ushort[8] { 0, 1500, 0, 1500, 5000, 0, 0, 1000 }, 50, new sbyte[7], 119, new List<short> { 85, 1 }, new List<short>(), 0, -1, -1, 430, 431, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 0, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 186 }, 432, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(90, 433, 434, 435, "SectSpecial/2106_xianjing", "Func_LiveSkill0", EBuildingBlockType.Building, EBuildingBlockClass.Music, 1, canOpenManageOutTaiwu: false, 1, "406056", new ushort[8] { 0, 2500, 0, 2500, 15000, 0, 0, 5000 }, 50, new sbyte[7], 120, new List<short> { 85, 15 }, new List<short>(), 0, -1, -1, 436, 437, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 0, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 202 }, 438, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(91, 439, 440, 441, "SectSpecial/2107_fenghuangtai", "Func_Special", EBuildingBlockType.Building, EBuildingBlockClass.Music, 1, canOpenManageOutTaiwu: false, 1, "406056", new ushort[8] { 0, 7500, 0, 7500, 15000, 0, 0, 15000 }, 50, new sbyte[7], 121, new List<short> { 85 }, new List<short>(), 0, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 1000)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 225 }, 442, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(92, 443, 444, 445, "SectSpecial/1010_yixuan", null, EBuildingBlockType.Building, EBuildingBlockClass.Chess, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8] { 0, 500, 0, 1500, 500, 0, 2500, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 93, 94, 95, 96, 97, 98 }, 1, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 250, mustMaintenance: false, isUnique: false, 0, 1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 118 }, 446, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(93, 447, 448, 449, "SectSpecial/2202_qiguan", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Chess, 1, canOpenManageOutTaiwu: false, 1, "414141", new ushort[8] { 0, 1250, 0, 2500, 1250, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 92 }, new List<short>(), 1, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 50, 0, new List<short> { 52 }, new List<short> { 55 }, -1, new List<ShortList>(), -1, new List<short> { 144 }, 450, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(94, 451, 452, 453, "SectSpecial/2203_douyitai", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Chess, 1, canOpenManageOutTaiwu: false, 1, "414141", new ushort[8] { 0, 2500, 0, 5000, 2500, 0, 0, 2500 }, 50, new sbyte[7], 122, new List<short> { 92 }, new List<short>(), 1, -1, 1, 147, 235, new short[16]
		{
			-1, 12, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 53 }, new List<short> { 56 }, -1, new List<ShortList>(), -1, new List<short> { 169 }, 454, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(95, 455, 456, 457, "SectSpecial/2204_shipuyuan", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Chess, 1, canOpenManageOutTaiwu: false, 1, "414141", new ushort[8] { 0, 2000, 0, 5000, 2000, 0, 5000, 0 }, 50, new sbyte[7], -1, new List<short> { 92 }, new List<short>(), 1, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 50, 0, new List<short> { 54 }, new List<short> { 57 }, -1, new List<ShortList>(), -1, new List<short> { 153 }, 458, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(96, 459, 460, 461, "SectSpecial/2205_liuliguan", "Func_LiveSkill1", EBuildingBlockType.Building, EBuildingBlockClass.Chess, 1, canOpenManageOutTaiwu: false, 1, "414141", new ushort[8] { 0, 1500, 0, 5000, 1500, 0, 0, 1000 }, 50, new sbyte[7], 123, new List<short> { 92, 8 }, new List<short>(), 1, -1, -1, 462, 463, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 187 }, 464, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(97, 465, 466, 467, "SectSpecial/2206_jingzhongtai", "Func_LiveSkill1", EBuildingBlockType.Building, EBuildingBlockClass.Chess, 1, canOpenManageOutTaiwu: false, 1, "414141", new ushort[8] { 0, 5000, 0, 10000, 5000, 0, 0, 5000 }, 50, new sbyte[7], 124, new List<short> { 92, 20 }, new List<short>(), 1, -1, -1, 468, 469, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 203 }, 470, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(98, 471, 472, 473, "SectSpecial/2207_fanglveshi", "Func_Special", EBuildingBlockType.Building, EBuildingBlockClass.Chess, 1, canOpenManageOutTaiwu: false, 1, "414141", new ushort[8] { 5000, 5000, 0, 15000, 5000, 0, 0, 15000 }, 50, new sbyte[7], 125, new List<short> { 92 }, new List<short>(), 1, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 1000)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 226 }, 474, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(99, 475, 476, 477, "SectSpecial/1011_shufang", null, EBuildingBlockType.Building, EBuildingBlockClass.Poem, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8] { 0, 1500, 0, 500, 1000, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 100, 101, 102, 103, 104, 105 }, 2, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 250, mustMaintenance: false, isUnique: false, 0, 2, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 119 }, 478, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(100, 479, 480, 481, "SectSpecial/2302_shupu", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Poem, 1, canOpenManageOutTaiwu: false, 1, "394a5d", new ushort[8] { 0, 2500, 0, 1000, 1500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 99 }, new List<short>(), 2, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 50, 0, new List<short> { 58 }, new List<short> { 61 }, -1, new List<ShortList>(), -1, new List<short> { 145 }, 482, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(101, 483, 484, 485, "SectSpecial/2303_shuyuan", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Poem, 1, canOpenManageOutTaiwu: false, 1, "394a5d", new ushort[8] { 0, 5000, 0, 1500, 3500, 0, 0, 2500 }, 50, new sbyte[7], 126, new List<short> { 99 }, new List<short>(), 2, -1, 1, 147, 235, new short[16]
		{
			-1, -1, 12, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 59 }, new List<short> { 62 }, -1, new List<ShortList>(), -1, new List<short> { 170 }, 486, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(102, 487, 488, 489, "SectSpecial/2304_hanyuan", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Poem, 1, canOpenManageOutTaiwu: false, 1, "394a5d", new ushort[8] { 0, 5000, 0, 1500, 2500, 0, 5000, 0 }, 50, new sbyte[7], -1, new List<short> { 99 }, new List<short>(), 2, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 50, 0, new List<short> { 60 }, new List<short> { 63 }, -1, new List<ShortList>(), -1, new List<short> { 154 }, 490, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(103, 491, 492, 493, "SectSpecial/2305_longguishan", "Func_LiveSkill2", EBuildingBlockType.Building, EBuildingBlockClass.Poem, 1, canOpenManageOutTaiwu: false, 1, "394a5d", new ushort[8] { 0, 5000, 0, 1000, 2000, 0, 0, 1000 }, 50, new sbyte[7], 127, new List<short> { 99, 2 }, new List<short>(), 2, -1, -1, 494, 495, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 2, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 188 }, 496, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(104, 497, 498, 499, "SectSpecial/2306_zhaoyelou", "Func_LiveSkill2", EBuildingBlockType.Building, EBuildingBlockClass.Poem, 1, canOpenManageOutTaiwu: false, 1, "394a5d", new ushort[8] { 0, 10000, 0, 2500, 7500, 0, 0, 5000 }, 50, new sbyte[7], 128, new List<short> { 99, 19 }, new List<short>(), 2, -1, -1, 500, 501, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 2, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 204 }, 502, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(105, 503, 504, 505, "SectSpecial/2307_cangshuge", "Func_Special", EBuildingBlockType.Building, EBuildingBlockClass.Poem, 1, canOpenManageOutTaiwu: false, 1, "394a5d", new ushort[8] { 0, 15000, 2500, 5000, 7500, 0, 0, 15000 }, 50, new sbyte[7], 129, new List<short> { 99 }, new List<short>(), 2, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 1000)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 506, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(106, 507, 508, 509, "SectSpecial/1012_huage", null, EBuildingBlockType.Building, EBuildingBlockClass.Painting, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8] { 0, 500, 0, 1000, 1500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 107, 108, 109, 110, 111, 112 }, 3, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 250, mustMaintenance: false, isUnique: false, 0, 3, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 120 }, 510, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(107, 511, 512, 513, "SectSpecial/2402_huapu", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Painting, 1, canOpenManageOutTaiwu: false, 1, "a25968", new ushort[8] { 0, 1000, 0, 1500, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 106 }, new List<short>(), 3, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 50, 0, new List<short> { 64 }, new List<short> { 67 }, -1, new List<ShortList>(), -1, new List<short> { 146 }, 514, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(108, 515, 516, 517, "SectSpecial/2403_danqingguan", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Painting, 1, canOpenManageOutTaiwu: false, 1, "a25968", new ushort[8] { 0, 2500, 0, 2500, 5000, 0, 0, 2500 }, 50, new sbyte[7], 130, new List<short> { 106 }, new List<short>(), 3, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, 12, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 65 }, new List<short> { 68 }, -1, new List<ShortList>(), -1, new List<short> { 171 }, 518, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(109, 519, 520, 521, "SectSpecial/2404_liuguangyuan", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Painting, 1, canOpenManageOutTaiwu: false, 1, "a25968", new ushort[8] { 0, 1000, 0, 2000, 5000, 0, 10000, 0 }, 50, new sbyte[7], -1, new List<short> { 106 }, new List<short>(), 3, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 50, 0, new List<short> { 66 }, new List<short> { 69 }, -1, new List<ShortList>(), -1, new List<short> { 155 }, 522, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(110, 523, 524, 525, "SectSpecial/2405_wuseku", "Func_LiveSkill3", EBuildingBlockType.Building, EBuildingBlockClass.Painting, 1, canOpenManageOutTaiwu: false, 1, "a25968", new ushort[8] { 0, 1000, 0, 2000, 5000, 0, 0, 1000 }, 50, new sbyte[7], 131, new List<short> { 106, 7 }, new List<short>(), 3, -1, -1, 526, 527, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 3, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 189 }, 528, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(111, 529, 530, 531, "SectSpecial/2406_tianxiangcaige", "Func_LiveSkill3", EBuildingBlockType.Building, EBuildingBlockClass.Painting, 1, canOpenManageOutTaiwu: false, 1, "a25968", new ushort[8] { 0, 2000, 0, 5000, 10000, 0, 15000, 5000 }, 50, new sbyte[7], 132, new List<short> { 106, 17 }, new List<short>(), 3, -1, -1, 532, 533, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 3, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 205 }, 534, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(112, 535, 536, 537, "SectSpecial/2407_huayingxuan", "Func_Special", EBuildingBlockType.Building, EBuildingBlockClass.Painting, 1, canOpenManageOutTaiwu: false, 1, "a25968", new ushort[8] { 0, 2500, 0, 7500, 15000, 5000, 0, 15000 }, 50, new sbyte[7], 133, new List<short> { 106 }, new List<short>(), 3, -1, 1, 147, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 1000)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 227 }, 538, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(113, 539, 540, 541, "SectSpecial/1013_guanxingtai", null, EBuildingBlockType.Building, EBuildingBlockClass.Math, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8] { 0, 1000, 0, 1500, 500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 114, 115, 116, 117, 118, 119 }, 4, -1, 5, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 250, mustMaintenance: false, isUnique: false, 0, 4, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 6 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 121 }, 542, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(114, 543, 544, 545, "SectSpecial/2502_zhanbuguan", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Math, 1, canOpenManageOutTaiwu: false, 1, "423d59", new ushort[8] { 0, 1500, 0, 2500, 1000, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 113 }, new List<short>(), 4, -1, 5, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 6 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, -50, 0, new List<short> { 70 }, new List<short> { 73 }, -1, new List<ShortList>(), -1, new List<short> { 147 }, 546, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(115, 547, 548, 549, "SectSpecial/2503_fangshiguan", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Math, 1, canOpenManageOutTaiwu: false, 1, "423d59", new ushort[8] { 0, 2500, 0, 5000, 2500, 0, 0, 2500 }, 50, new sbyte[7], 134, new List<short> { 113 }, new List<short>(), 4, -1, 5, 282, 235, new short[16]
		{
			-1, -1, -1, -1, 12, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 6 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 71 }, new List<short> { 74 }, -1, new List<ShortList>(), -1, new List<short> { 172 }, 550, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(116, 551, 552, 553, "SectSpecial/2504_jitiangaotai", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Math, 1, canOpenManageOutTaiwu: false, 1, "423d59", new ushort[8] { 0, 2500, 0, 5000, 1500, 0, 5000, 0 }, 50, new sbyte[7], -1, new List<short> { 113 }, new List<short>(), 4, -1, 5, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 6 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, -50, 0, new List<short> { 72 }, new List<short> { 75 }, -1, new List<ShortList>(), -1, new List<short> { 156 }, 554, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(117, 555, 556, 557, "SectSpecial/2505_zhaixinglou", "Func_LiveSkill4", EBuildingBlockType.Building, EBuildingBlockClass.Math, 1, canOpenManageOutTaiwu: false, 1, "423d59", new ushort[8] { 0, 1500, 0, 2500, 1000, 0, 15000, 1000 }, 50, new sbyte[7], 135, new List<short> { 113, 4 }, new List<short>(), 4, -1, -1, 558, 559, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 4, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 190 }, 560, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(118, 561, 562, 563, "SectSpecial/2506_kunlunta", "Func_LiveSkill4", EBuildingBlockType.Building, EBuildingBlockClass.Math, 1, canOpenManageOutTaiwu: false, 1, "423d59", new ushort[8] { 0, 7500, 0, 10000, 2500, 0, 0, 5000 }, 50, new sbyte[7], 136, new List<short> { 113, 18 }, new List<short>(), 4, -1, -1, 564, 565, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 4, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 206 }, 566, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(119, 567, 568, 569, "SectSpecial/2508_shengmieliangxingfan", "Func_LiveSkill4", EBuildingBlockType.Building, EBuildingBlockClass.Math, 1, canOpenManageOutTaiwu: false, 1, "423d59", new ushort[8] { 0, 10000, 0, 15000, 5000, 0, 0, 15000 }, 50, new sbyte[7], 137, new List<short> { 113 }, new List<short>(), 4, -1, 5, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 1000)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 6 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 228 }, 570, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new BuildingBlockItem(120, 571, 572, 573, "SectSpecial/1014_ganquanting", null, EBuildingBlockType.Building, EBuildingBlockClass.Appraisal, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8] { 1500, 1000, 0, 0, 0, 500, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 121, 122, 123, 124, 125, 126, 127, 128 }, 5, -1, 1, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 250, mustMaintenance: false, isUnique: false, 0, 5, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 122 }, 575, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(121, 576, 577, 578, "SectSpecial/2602_chaguan", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Appraisal, 1, canOpenManageOutTaiwu: false, 1, "9a694f", new ushort[8] { 2500, 500, 0, 0, 0, 1000, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 120 }, new List<short>(), 5, -1, 1, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 250, 250 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, 50, new List<short> { 76 }, new List<short> { 82 }, -1, new List<ShortList>(), -1, new List<short> { 133 }, 579, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(122, 580, 581, 582, "SectSpecial/2603_jiusi", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Appraisal, 1, canOpenManageOutTaiwu: false, 1, "9a694f", new ushort[8] { 2500, 1000, 0, 0, 0, 500, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 120 }, new List<short>(), 5, -1, 1, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, 50, new List<short> { 77 }, new List<short> { 83 }, -1, new List<ShortList>(), -1, new List<short> { 134 }, 583, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(123, 584, 585, 586, "SectSpecial/2604_wenxiangyuan", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Appraisal, 1, canOpenManageOutTaiwu: false, 1, "9a694f", new ushort[8] { 5000, 2500, 0, 0, 0, 2500, 0, 2500 }, 50, new sbyte[7], 138, new List<short> { 120 }, new List<short>(), 5, -1, 1, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, 12, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 78 }, new List<short> { 84 }, -1, new List<ShortList>(), -1, new List<short> { 173 }, 587, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(124, 588, 589, 590, "SectSpecial/2605_sihaifu", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Appraisal, 1, canOpenManageOutTaiwu: false, 1, "9a694f", new ushort[8] { 5000, 2500, 0, 0, 0, 1500, 5000, 0 }, 50, new sbyte[7], -1, new List<short> { 120 }, new List<short>(), 5, -1, 1, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 50, new List<short> { 79 }, new List<short> { 85 }, -1, new List<ShortList>(), -1, new List<short> { 157 }, 591, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(125, 592, 593, 594, "SectSpecial/2606_hufang", "Func_LiveSkill5", EBuildingBlockType.Building, EBuildingBlockClass.Appraisal, 1, canOpenManageOutTaiwu: false, 1, "9a694f", new ushort[8] { 5000, 2000, 0, 0, 0, 1000, 0, 1000 }, 50, new sbyte[7], 139, new List<short> { 120, 1 }, new List<short>(), 5, -1, -1, 595, 596, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 5, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 191 }, 597, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(126, 598, 599, 600, "SectSpecial/2607_shenxianyuan", "Func_LiveSkill5", EBuildingBlockType.Building, EBuildingBlockClass.Appraisal, 1, canOpenManageOutTaiwu: false, 1, "9a694f", new ushort[8] { 12500, 2500, 0, 0, 0, 2500, 12500, 5000 }, 50, new sbyte[7], 140, new List<short> { 120, 15 }, new List<short>(), 5, -1, -1, 601, 602, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 5, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 207 }, 603, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(127, 604, 605, 606, "SectSpecial/2608_chayuan", "Func_GetItem1", EBuildingBlockType.Building, EBuildingBlockClass.Appraisal, 1, canOpenManageOutTaiwu: false, 1, "9a694f", new ushort[8] { 15000, 5000, 0, 0, 0, 10000, 0, 15000 }, 50, new sbyte[7], 141, new List<short> { 120, 9 }, new List<short>(), 5, -1, 1, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 1000)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 80 }, new List<short> { 86 }, -1, new List<ShortList>(), -1, new List<short> { 246 }, 607, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: true, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(128, 608, 609, 610, "SectSpecial/2609_zhengjiufang", "Func_GetItem2", EBuildingBlockType.Building, EBuildingBlockClass.Appraisal, 1, canOpenManageOutTaiwu: false, 1, "9a694f", new ushort[8] { 15000, 10000, 0, 0, 0, 5000, 0, 15000 }, 50, new sbyte[7], 142, new List<short> { 120, 9 }, new List<short>(), 5, -1, 1, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 1000)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 4 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 81 }, new List<short> { 87 }, -1, new List<ShortList>(), -1, new List<short> { 247 }, 611, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: true, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(129, 612, 613, 614, "SectSpecial/1015_huolianshi", null, EBuildingBlockType.Building, EBuildingBlockClass.Forging, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8] { 0, 500, 2500, 0, 0, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 130, 131, 132, 133, 134, 135, 136, 137, 138 }, 6, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(2, 10)
		}, 250, mustMaintenance: false, isUnique: false, 0, 6, -1, -1, -1, -1, canMakeItem: true, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 123 }, 615, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: true, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(130, 616, 617, 618, "SectSpecial/2702_tiejiangpu", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Forging, 1, canOpenManageOutTaiwu: false, 1, "583936", new ushort[8] { 0, 1500, 2500, 0, 1000, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 129 }, new List<short>(), 6, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(2, 20)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, 50, new List<short> { 88 }, new List<short> { 93 }, -1, new List<ShortList>(), -1, new List<short> { 135 }, 619, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(131, 620, 621, 622, "SectSpecial/2703_duanyefang", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Forging, 1, canOpenManageOutTaiwu: false, 1, "583936", new ushort[8] { 0, 2500, 5000, 0, 1500, 0, 5000, 2500 }, 50, new sbyte[7], 143, new List<short> { 129 }, new List<short>(), 6, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, 12, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 89 }, new List<short> { 94 }, -1, new List<ShortList>(), -1, new List<short> { 174 }, 623, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(132, 624, 625, 626, "SectSpecial/2704_jinpu", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Forging, 1, canOpenManageOutTaiwu: false, 1, "583936", new ushort[8] { 0, 1500, 5000, 0, 500, 0, 15000, 0 }, 50, new sbyte[7], -1, new List<short> { 129 }, new List<short>(), 6, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(2, 100)
		}, 1000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 50, new List<short> { 90 }, new List<short> { 95 }, -1, new List<ShortList>(), -1, new List<short> { 162 }, 627, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(133, 628, 629, 630, "SectSpecial/2705_shuipai", "Func_LiveSkill6", EBuildingBlockType.Building, EBuildingBlockClass.Forging, 1, canOpenManageOutTaiwu: false, 1, "583936", new ushort[8] { 0, 2000, 5000, 0, 1000, 0, 0, 1000 }, 50, new sbyte[7], -1, new List<short> { 129, 1 }, new List<short>(), 6, -1, -1, 631, 632, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(2, 20)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 6, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 192 }, 633, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(134, 634, 635, 636, "SectSpecial/2706_yuntiekuangchang", "Func_LiveSkill6", EBuildingBlockType.Building, EBuildingBlockClass.Forging, 1, canOpenManageOutTaiwu: false, 1, "583936", new ushort[8] { 0, 5000, 12500, 0, 2500, 0, 0, 5000 }, 50, new sbyte[7], 144, new List<short> { 129, 12 }, new List<short>(), 6, -1, -1, 637, 638, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(2, 100)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 6, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 208 }, 639, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(135, 640, 641, 642, "SectSpecial/2707_taoxichi", "Func_GetMoreItem", EBuildingBlockType.Building, EBuildingBlockClass.Forging, 1, canOpenManageOutTaiwu: false, 1, "583936", new ushort[8] { 0, 10000, 20000, 0, 5000, 0, 0, 7500 }, 50, new sbyte[7], 145, new List<short> { 129, 2 }, new List<short>(), 6, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(2, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 91 }, new List<short> { 96 }, -1, new List<ShortList>(), -1, new List<short> { 231 }, 643, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(136, 644, 645, 646, "SectSpecial/2708_jinglianshi", "Func_GetBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Forging, 1, canOpenManageOutTaiwu: false, 1, "583936", new ushort[8] { 0, 10000, 20000, 0, 5000, 0, 0, 7500 }, 50, new sbyte[7], 146, new List<short> { 129, 2 }, new List<short>(), 6, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(2, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 92 }, new List<short> { 97 }, -1, new List<ShortList>(), -1, new List<short> { 232 }, 647, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(137, 648, 649, 650, "SectSpecial/2709_longjingqixingquan", "Func_MakeBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Forging, 1, canOpenManageOutTaiwu: false, 1, "583936", new ushort[8] { 0, 15000, 55000, 0, 10000, 0, 0, 35000 }, 50, new sbyte[7], 147, new List<short> { 129 }, new List<short>(), 6, -1, -1, 651, 652, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(2, 200)
		}, 15000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: true, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 653, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(138, 654, 655, 656, "SectSpecial/2710_shenhuozhu", "Func_ReduceRequirement", EBuildingBlockType.Building, EBuildingBlockClass.Forging, 1, canOpenManageOutTaiwu: false, 1, "583936", new ushort[8] { 0, 7500, 25000, 0, 2500, 0, 25000, 15000 }, 50, new sbyte[7], 148, new List<short> { 129, 11 }, new List<short>(), 6, -1, -1, 657, 658, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(2, 100)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, 6, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 218 }, 659, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(139, 660, 661, 662, "SectSpecial/1016_mugongfang", null, EBuildingBlockType.Building, EBuildingBlockClass.Woodworking, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8] { 0, 2500, 500, 0, 0, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 140, 141, 142, 143, 144, 145, 146, 147, 148 }, 7, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(1, 10)
		}, 250, mustMaintenance: false, isUnique: false, 0, 7, -1, -1, -1, -1, canMakeItem: true, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 124 }, 663, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: true, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(140, 664, 665, 666, "SectSpecial/2802_mugongpu", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Woodworking, 1, canOpenManageOutTaiwu: false, 1, "4c6b5d", new ushort[8] { 0, 2500, 1500, 0, 1000, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 139 }, new List<short>(), 7, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(1, 20)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, 50, new List<short> { 98 }, new List<short> { 103 }, -1, new List<ShortList>(), -1, new List<short> { 136 }, 667, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(141, 668, 669, 670, "SectSpecial/2803_zhimufang", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Woodworking, 1, canOpenManageOutTaiwu: false, 1, "4c6b5d", new ushort[8] { 0, 5000, 2500, 0, 1500, 0, 5000, 2500 }, 50, new sbyte[7], 149, new List<short> { 139 }, new List<short>(), 7, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, 12, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 99 }, new List<short> { 104 }, -1, new List<ShortList>(), -1, new List<short> { 175 }, 671, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(142, 672, 673, 674, "SectSpecial/2804_yingzaofang", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Woodworking, 1, canOpenManageOutTaiwu: false, 1, "4c6b5d", new ushort[8] { 0, 5000, 1500, 0, 500, 0, 15000, 0 }, 50, new sbyte[7], -1, new List<short> { 139 }, new List<short>(), 7, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(1, 100)
		}, 1000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 50, new List<short> { 100 }, new List<short> { 105 }, -1, new List<ShortList>(), -1, new List<short> { 163 }, 675, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(143, 676, 677, 678, "SectSpecial/2805_heizhichi", "Func_LiveSkill7", EBuildingBlockType.Building, EBuildingBlockClass.Woodworking, 1, canOpenManageOutTaiwu: false, 1, "4c6b5d", new ushort[8] { 0, 5000, 2000, 0, 1000, 0, 0, 1000 }, 50, new sbyte[7], -1, new List<short> { 139, 6 }, new List<short>(), 7, -1, -1, 679, 680, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(1, 20)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 7, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 193 }, 681, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(144, 682, 683, 684, "SectSpecial/2806_muliaobiaoben", "Func_LiveSkill7", EBuildingBlockType.Building, EBuildingBlockClass.Woodworking, 1, canOpenManageOutTaiwu: false, 1, "4c6b5d", new ushort[8] { 0, 12500, 5000, 0, 2500, 0, 0, 5000 }, 50, new sbyte[7], 150, new List<short> { 139, 13 }, new List<short>(), 7, -1, -1, 685, 686, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(1, 100)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 7, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 209 }, 687, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(145, 688, 689, 690, "SectSpecial/2807_famuchang", "Func_GetMoreItem", EBuildingBlockType.Building, EBuildingBlockClass.Woodworking, 1, canOpenManageOutTaiwu: false, 1, "4c6b5d", new ushort[8] { 0, 20000, 10000, 0, 5000, 0, 0, 7500 }, 50, new sbyte[7], 151, new List<short> { 139, 3 }, new List<short>(), 7, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(1, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 101 }, new List<short> { 106 }, -1, new List<ShortList>(), -1, new List<short> { 233 }, 691, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(146, 692, 693, 694, "SectSpecial/2808_linchang", "Func_GetBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Woodworking, 1, canOpenManageOutTaiwu: false, 1, "4c6b5d", new ushort[8] { 0, 20000, 10000, 0, 5000, 0, 0, 7500 }, 50, new sbyte[7], 152, new List<short> { 139, 3 }, new List<short>(), 7, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(1, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 102 }, new List<short> { 107 }, -1, new List<ShortList>(), -1, new List<short> { 234 }, 695, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(147, 696, 697, 698, "SectSpecial/2809_youmingyao", "Func_MakeBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Woodworking, 1, canOpenManageOutTaiwu: false, 1, "4c6b5d", new ushort[8] { 0, 55000, 15000, 0, 10000, 0, 0, 35000 }, 50, new sbyte[7], 153, new List<short> { 139 }, new List<short>(), 7, -1, -1, 699, 700, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(1, 200)
		}, 15000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: true, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 701, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(148, 702, 703, 704, "SectSpecial/2810_shenmulin", "Func_ReduceRequirement", EBuildingBlockType.Building, EBuildingBlockClass.Woodworking, 1, canOpenManageOutTaiwu: false, 1, "4c6b5d", new ushort[8] { 0, 25000, 7500, 0, 2500, 0, 25000, 15000 }, 50, new sbyte[7], 154, new List<short> { 139, 14 }, new List<short>(), 7, -1, -1, 705, 706, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(1, 100)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, 7, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 219 }, 707, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(149, 708, 709, 710, "SectSpecial/1017_yaofang", null, EBuildingBlockType.Building, EBuildingBlockClass.Medicine, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8] { 0, 0, 0, 0, 500, 2500, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 150, 151, 152, 153, 154, 155, 156, 157, 158 }, 8, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(5, 10)
		}, 250, mustMaintenance: false, isUnique: false, 0, 8, -1, -1, -1, -1, canMakeItem: true, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 125 }, 711, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: true, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(150, 712, 713, 714, "SectSpecial/2902_shuyaopu", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Medicine, 1, canOpenManageOutTaiwu: false, 1, "698455", new ushort[8] { 0, 0, 0, 1000, 1500, 2500, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 149 }, new List<short>(), 8, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 20)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, -50, new List<short> { 108 }, new List<short> { 113 }, -1, new List<ShortList>(), -1, new List<short> { 137 }, 715, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(151, 716, 717, 718, "SectSpecial/2903_yaoshiguan", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Medicine, 1, canOpenManageOutTaiwu: false, 1, "698455", new ushort[8] { 0, 0, 0, 1500, 2500, 5000, 5000, 2500 }, 50, new sbyte[7], 155, new List<short> { 149 }, new List<short>(), 8, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, 12, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 109 }, new List<short> { 114 }, -1, new List<ShortList>(), -1, new List<short> { 176 }, 719, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(152, 720, 721, 722, "SectSpecial/2904_bingfang", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Medicine, 1, canOpenManageOutTaiwu: false, 1, "698455", new ushort[8] { 0, 0, 0, 500, 1500, 5000, 15000, 0 }, 50, new sbyte[7], -1, new List<short> { 149 }, new List<short>(), 8, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 100)
		}, 1000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, -50, new List<short> { 110 }, new List<short> { 115 }, -1, new List<ShortList>(), -1, new List<short> { 164 }, 723, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(153, 724, 725, 726, "SectSpecial/2905_shouyiguan", "Func_LiveSkill8", EBuildingBlockType.Building, EBuildingBlockClass.Medicine, 1, canOpenManageOutTaiwu: false, 1, "698455", new ushort[8] { 0, 0, 0, 1000, 2000, 5000, 0, 1000 }, 50, new sbyte[7], -1, new List<short> { 149, 10 }, new List<short>(), 8, -1, -1, 727, 728, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 20)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 8, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 194 }, 729, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(154, 730, 731, 732, "SectSpecial/2906_yingaibingguan", "Func_LiveSkill8", EBuildingBlockType.Building, EBuildingBlockClass.Medicine, 1, canOpenManageOutTaiwu: false, 1, "698455", new ushort[8] { 0, 0, 0, 2500, 5000, 12500, 0, 5000 }, 50, new sbyte[7], 156, new List<short> { 149, 20 }, new List<short>(), 8, -1, -1, 733, 734, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 100)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 8, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 210 }, 735, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(155, 736, 737, 738, "SectSpecial/2907_yaopu", "Func_GetMoreItem", EBuildingBlockType.Building, EBuildingBlockClass.Medicine, 1, canOpenManageOutTaiwu: false, 1, "698455", new ushort[8] { 0, 0, 0, 5000, 10000, 20000, 0, 7500 }, 50, new sbyte[7], 157, new List<short> { 149, 5 }, new List<short>(), 8, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 111 }, new List<short> { 116 }, -1, new List<ShortList>(), -1, new List<short> { 239 }, 739, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(156, 740, 741, 742, "SectSpecial/2908_yangyaoshi", "Func_GetBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Medicine, 1, canOpenManageOutTaiwu: false, 1, "698455", new ushort[8] { 0, 0, 0, 5000, 10000, 20000, 0, 7500 }, 50, new sbyte[7], 158, new List<short> { 149, 5 }, new List<short>(), 8, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 112 }, new List<short> { 117 }, -1, new List<ShortList>(), -1, new List<short> { 240 }, 743, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(157, 744, 745, 746, "SectSpecial/2909_shuihuoqinglu", "Func_MakeBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Medicine, 1, canOpenManageOutTaiwu: false, 1, "698455", new ushort[8] { 0, 0, 0, 10000, 15000, 55000, 0, 35000 }, 50, new sbyte[7], 159, new List<short> { 149 }, new List<short>(), 8, -1, -1, 747, 748, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 200)
		}, 15000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: true, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 749, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(158, 750, 751, 752, "SectSpecial/2910_shennongjian", "Func_ReduceRequirement", EBuildingBlockType.Building, EBuildingBlockClass.Medicine, 1, canOpenManageOutTaiwu: false, 1, "698455", new ushort[8] { 0, 0, 0, 2500, 7500, 25000, 25000, 15000 }, 50, new sbyte[7], 160, new List<short> { 149, 15 }, new List<short>(), 8, -1, -1, 753, 754, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 100)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, 8, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 220 }, 755, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(159, 756, 757, 758, "SectSpecial/1018_youshi", null, EBuildingBlockType.Building, EBuildingBlockClass.Toxicology, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8] { 500, 0, 0, 0, 0, 2500, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 160, 161, 162, 163, 164, 165, 166, 167, 168 }, 9, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(5, 10)
		}, 250, mustMaintenance: false, isUnique: false, 0, 9, -1, -1, -1, -1, canMakeItem: true, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 126 }, 759, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: true, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(160, 760, 761, 762, "SectSpecial/3002_dushi", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Toxicology, 1, canOpenManageOutTaiwu: false, 1, "6b6c9e", new ushort[8] { 1500, 0, 1000, 0, 0, 2500, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 159 }, new List<short>(), 9, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 20)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, -50, new List<short> { 118 }, new List<short> { 123 }, -1, new List<ShortList>(), -1, new List<short> { 138 }, 763, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(161, 764, 765, 766, "SectSpecial/3003_anlao", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Toxicology, 1, canOpenManageOutTaiwu: false, 1, "6b6c9e", new ushort[8] { 2500, 0, 1500, 0, 0, 5000, 5000, 2500 }, 50, new sbyte[7], 161, new List<short> { 159 }, new List<short>(), 9, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, 12,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 119 }, new List<short> { 124 }, -1, new List<ShortList>(), -1, new List<short> { 177 }, 767, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(162, 768, 769, 770, "SectSpecial/3004_miyi", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Toxicology, 1, canOpenManageOutTaiwu: false, 1, "6b6c9e", new ushort[8] { 1500, 0, 500, 0, 0, 5000, 15000, 0 }, 50, new sbyte[7], -1, new List<short> { 159 }, new List<short>(), 9, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 100)
		}, 1000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, -50, new List<short> { 120 }, new List<short> { 125 }, -1, new List<ShortList>(), -1, new List<short> { 165 }, 771, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(163, 772, 773, 774, "SectSpecial/3005_huanyangshi", "Func_LiveSkill9", EBuildingBlockType.Building, EBuildingBlockClass.Toxicology, 1, canOpenManageOutTaiwu: false, 1, "6b6c9e", new ushort[8] { 2000, 0, 1000, 0, 0, 5000, 0, 1000 }, 50, new sbyte[7], -1, new List<short> { 159, 10 }, new List<short>(), 9, -1, -1, 775, 776, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 20)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 9, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 195 }, 777, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(164, 778, 779, 780, "SectSpecial/3006_wurenju", "Func_LiveSkill9", EBuildingBlockType.Building, EBuildingBlockClass.Toxicology, 1, canOpenManageOutTaiwu: false, 1, "6b6c9e", new ushort[8] { 5000, 0, 2500, 0, 0, 12500, 0, 5000 }, 50, new sbyte[7], 162, new List<short> { 159, 13 }, new List<short>(), 9, -1, -1, 781, 782, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 100)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 9, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 211 }, 783, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(165, 784, 785, 786, "SectSpecial/3007_lianzhangchi", "Func_GetMoreItem", EBuildingBlockType.Building, EBuildingBlockClass.Toxicology, 1, canOpenManageOutTaiwu: false, 1, "6b6c9e", new ushort[8] { 10000, 0, 5000, 0, 0, 20000, 0, 7500 }, 50, new sbyte[7], 163, new List<short> { 159, 6 }, new List<short>(), 9, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 121 }, new List<short> { 126 }, -1, new List<ShortList>(), -1, new List<short> { 241 }, 787, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(166, 788, 789, 790, "SectSpecial/3008_feirenchi", "Func_GetBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Toxicology, 1, canOpenManageOutTaiwu: false, 1, "6b6c9e", new ushort[8] { 10000, 0, 5000, 0, 0, 20000, 0, 7500 }, 50, new sbyte[7], 164, new List<short> { 159, 6 }, new List<short>(), 9, -1, 0, 574, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 2 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 122 }, new List<short> { 127 }, -1, new List<ShortList>(), -1, new List<short> { 242 }, 791, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(167, 792, 793, 794, "SectSpecial/3009_xuechi", "Func_MakeBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Toxicology, 1, canOpenManageOutTaiwu: false, 1, "6b6c9e", new ushort[8] { 15000, 0, 10000, 0, 0, 55000, 0, 35000 }, 50, new sbyte[7], 165, new List<short> { 159 }, new List<short>(), 9, -1, -1, 795, 796, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 200)
		}, 15000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: true, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 797, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(168, 798, 799, 800, "SectSpecial/3010_shenlongzhu", "Func_ReduceRequirement", EBuildingBlockType.Building, EBuildingBlockClass.Toxicology, 1, canOpenManageOutTaiwu: false, 1, "6b6c9e", new ushort[8] { 7500, 0, 2500, 0, 0, 25000, 25000, 15000 }, 50, new sbyte[7], 166, new List<short> { 159, 16 }, new List<short>(), 9, -1, -1, 801, 802, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(5, 100)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, 9, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 221 }, 803, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(169, 804, 805, 806, "SectSpecial/1019_xiulou", null, EBuildingBlockType.Building, EBuildingBlockClass.Weaving, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8] { 0, 500, 0, 0, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 170, 171, 172, 173, 174, 175, 176, 177, 178 }, 10, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(4, 10)
		}, 250, mustMaintenance: false, isUnique: false, 0, 10, -1, -1, -1, -1, canMakeItem: true, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 127 }, 807, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: true, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(170, 808, 809, 810, "SectSpecial/3102_buzhuang", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Weaving, 1, canOpenManageOutTaiwu: false, 1, "212b3c", new ushort[8] { 0, 1500, 0, 1000, 2500, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 169 }, new List<short>(), 10, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(4, 20)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, 50, new List<short> { 128 }, new List<short> { 133 }, -1, new List<ShortList>(), -1, new List<short> { 139 }, 811, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(171, 812, 813, 814, "SectSpecial/3103_zhizaofang", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Weaving, 1, canOpenManageOutTaiwu: false, 1, "212b3c", new ushort[8] { 0, 2500, 0, 1500, 5000, 0, 5000, 2500 }, 50, new sbyte[7], 167, new List<short> { 169 }, new List<short>(), 10, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			12, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 129 }, new List<short> { 134 }, -1, new List<ShortList>(), -1, new List<short> { 178 }, 815, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(172, 816, 817, 818, "SectSpecial/3104_jinxiuge", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Weaving, 1, canOpenManageOutTaiwu: false, 1, "212b3c", new ushort[8] { 0, 1500, 0, 500, 5000, 0, 15000, 0 }, 50, new sbyte[7], -1, new List<short> { 169 }, new List<short>(), 10, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(4, 100)
		}, 1000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 50, new List<short> { 130 }, new List<short> { 135 }, -1, new List<ShortList>(), -1, new List<short> { 166 }, 819, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(173, 820, 821, 822, "SectSpecial/3105_zhuixingge", "Func_LiveSkill10", EBuildingBlockType.Building, EBuildingBlockClass.Weaving, 1, canOpenManageOutTaiwu: false, 1, "212b3c", new ushort[8] { 0, 2000, 0, 1000, 5000, 0, 0, 1000 }, 50, new sbyte[7], -1, new List<short> { 169, 8 }, new List<short>(), 10, -1, -1, 823, 824, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(4, 20)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 10, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 196 }, 825, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(174, 826, 827, 828, "SectSpecial/3106_shegulingji", "Func_LiveSkill10", EBuildingBlockType.Building, EBuildingBlockClass.Weaving, 1, canOpenManageOutTaiwu: false, 1, "212b3c", new ushort[8] { 0, 5000, 0, 2500, 12500, 0, 0, 5000 }, 50, new sbyte[7], 168, new List<short> { 169, 16 }, new List<short>(), 10, -1, -1, 829, 830, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(4, 100)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 10, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 212 }, 831, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(175, 832, 833, 834, "SectSpecial/3107_baihuapu", "Func_GetMoreItem", EBuildingBlockType.Building, EBuildingBlockClass.Weaving, 1, canOpenManageOutTaiwu: false, 1, "212b3c", new ushort[8] { 0, 10000, 0, 5000, 20000, 0, 0, 7500 }, 50, new sbyte[7], 169, new List<short> { 169, 7 }, new List<short>(), 10, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(4, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 131 }, new List<short> { 136 }, -1, new List<ShortList>(), -1, new List<short> { 235 }, 835, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(176, 836, 837, 838, "SectSpecial/3108_qizhenyuan", "Func_GetBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Weaving, 1, canOpenManageOutTaiwu: false, 1, "212b3c", new ushort[8] { 0, 10000, 0, 5000, 20000, 0, 0, 7500 }, 50, new sbyte[7], 170, new List<short> { 169, 7 }, new List<short>(), 10, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(4, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 132 }, new List<short> { 137 }, -1, new List<ShortList>(), -1, new List<short> { 236 }, 839, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(177, 840, 841, 842, "SectSpecial/3109_caisangyuan", "Func_MakeBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Weaving, 1, canOpenManageOutTaiwu: false, 1, "212b3c", new ushort[8] { 0, 15000, 0, 10000, 55000, 0, 0, 35000 }, 50, new sbyte[7], 171, new List<short> { 169 }, new List<short>(), 10, -1, -1, 843, 844, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(4, 200)
		}, 15000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: true, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 845, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(178, 846, 847, 848, "SectSpecial/3110_shencaisuo", "Func_ReduceRequirement", EBuildingBlockType.Building, EBuildingBlockClass.Weaving, 1, canOpenManageOutTaiwu: false, 1, "212b3c", new ushort[8] { 0, 7500, 0, 2500, 25000, 0, 25000, 15000 }, 50, new sbyte[7], 172, new List<short> { 169, 17 }, new List<short>(), 10, -1, -1, 849, 850, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(4, 100)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, 10, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 222 }, 851, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(179, 852, 853, 854, "SectSpecial/1020_qiaojiangwu", null, EBuildingBlockType.Building, EBuildingBlockClass.Jade, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8] { 0, 0, 500, 2500, 0, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 180, 181, 182, 183, 184, 185, 186, 187, 188 }, 11, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(3, 10)
		}, 250, mustMaintenance: false, isUnique: false, 0, 11, -1, -1, -1, -1, canMakeItem: true, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 128 }, 855, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: true, 0, new List<short>(), -1));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new BuildingBlockItem(180, 856, 857, 858, "SectSpecial/3202_zhubaopu", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Jade, 1, canOpenManageOutTaiwu: false, 1, "624166", new ushort[8] { 0, 0, 1500, 2500, 1000, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 179 }, new List<short>(), 11, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(3, 20)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, 50, new List<short> { 138 }, new List<short> { 143 }, -1, new List<ShortList>(), -1, new List<short> { 140 }, 859, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(181, 860, 861, 862, "SectSpecial/3203_maoshifang", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Jade, 1, canOpenManageOutTaiwu: false, 1, "624166", new ushort[8] { 0, 0, 2500, 5000, 1500, 0, 5000, 2500 }, 50, new sbyte[7], 173, new List<short> { 179 }, new List<short>(), 11, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, 12, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 139 }, new List<short> { 144 }, -1, new List<ShortList>(), -1, new List<short> { 179 }, 863, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(182, 864, 865, 866, "SectSpecial/3204_linlangge", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Jade, 1, canOpenManageOutTaiwu: false, 1, "624166", new ushort[8] { 0, 0, 1500, 5000, 500, 0, 15000, 0 }, 50, new sbyte[7], -1, new List<short> { 179 }, new List<short>(), 11, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(3, 100)
		}, 1000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 50, new List<short> { 140 }, new List<short> { 145 }, -1, new List<ShortList>(), -1, new List<short> { 167 }, 867, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(183, 868, 869, 870, "SectSpecial/3205_gongyulou", "Func_LiveSkill11", EBuildingBlockType.Building, EBuildingBlockClass.Jade, 1, canOpenManageOutTaiwu: false, 1, "624166", new ushort[8] { 0, 0, 2000, 5000, 1000, 0, 0, 1000 }, 50, new sbyte[7], -1, new List<short> { 179, 2 }, new List<short>(), 11, -1, -1, 871, 872, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(3, 20)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 11, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 197 }, 873, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(184, 874, 875, 876, "SectSpecial/3206_chixuejing", "Func_LiveSkill11", EBuildingBlockType.Building, EBuildingBlockClass.Jade, 1, canOpenManageOutTaiwu: false, 1, "624166", new ushort[8] { 0, 0, 5000, 12500, 2500, 0, 0, 5000 }, 50, new sbyte[7], 174, new List<short> { 179, 11 }, new List<short>(), 11, -1, -1, 877, 878, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(3, 100)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 11, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 213 }, 879, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(185, 880, 881, 882, "SectSpecial/3207_huanbichi", "Func_GetMoreItem", EBuildingBlockType.Building, EBuildingBlockClass.Jade, 1, canOpenManageOutTaiwu: false, 1, "624166", new ushort[8] { 0, 0, 10000, 20000, 5000, 0, 0, 7500 }, 50, new sbyte[7], 175, new List<short> { 179, 8 }, new List<short>(), 11, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(3, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 141 }, new List<short> { 146 }, -1, new List<ShortList>(), -1, new List<short> { 237 }, 883, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(186, 884, 885, 886, "SectSpecial/3208_jingangjieyutai", "Func_GetBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Jade, 1, canOpenManageOutTaiwu: false, 1, "624166", new ushort[8] { 0, 0, 10000, 20000, 5000, 0, 0, 7500 }, 50, new sbyte[7], 176, new List<short> { 179, 8 }, new List<short>(), 11, -1, 4, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(3, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 1 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 142 }, new List<short> { 147 }, -1, new List<ShortList>(), -1, new List<short> { 238 }, 887, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(187, 888, 889, 890, "SectSpecial/3209_linglongtai", "Func_MakeBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Jade, 1, canOpenManageOutTaiwu: false, 1, "624166", new ushort[8] { 0, 0, 15000, 55000, 10000, 0, 0, 35000 }, 50, new sbyte[7], 177, new List<short> { 179 }, new List<short>(), 11, -1, -1, 891, 892, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(3, 200)
		}, 15000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: true, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 893, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(188, 894, 895, 896, "SectSpecial/3210_shenguangbi", "Func_ReduceRequirement", EBuildingBlockType.Building, EBuildingBlockClass.Jade, 1, canOpenManageOutTaiwu: false, 1, "624166", new ushort[8] { 0, 0, 7500, 25000, 2500, 0, 25000, 15000 }, 50, new sbyte[7], 178, new List<short> { 179, 19 }, new List<short>(), 11, -1, -1, 897, 898, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(3, 100)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, 11, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 223 }, 899, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(189, 900, 901, 902, "SectSpecial/1021_yunfang", null, EBuildingBlockType.Building, EBuildingBlockClass.Taoism, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8] { 0, 500, 0, 1000, 0, 1500, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 190, 191, 192, 193, 194, 195 }, 12, -1, 3, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 250, mustMaintenance: false, isUnique: false, 0, 12, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 129 }, 903, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(190, 904, 905, 906, "SectSpecial/3302_fashidaochang", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Taoism, 1, canOpenManageOutTaiwu: false, 1, "414f8a", new ushort[8] { 0, 500, 0, 1000, 0, 2500, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 189 }, new List<short>(), 12, -1, 3, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, -50, new List<short> { 148 }, new List<short> { 151 }, -1, new List<ShortList>(), -1, new List<short> { 148 }, 907, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(191, 908, 909, 910, "SectSpecial/3303_daoguan", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Taoism, 1, canOpenManageOutTaiwu: false, 1, "414f8a", new ushort[8] { 0, 2000, 0, 3000, 0, 5000, 0, 2500 }, 50, new sbyte[7], 179, new List<short> { 189 }, new List<short>(), 12, -1, 3, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, 12, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 149 }, new List<short> { 152 }, -1, new List<ShortList>(), -1, new List<short> { 180 }, 911, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(192, 912, 913, 914, "SectSpecial/3304_sanqingdian", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Taoism, 1, canOpenManageOutTaiwu: false, 1, "414f8a", new ushort[8] { 0, 1000, 0, 2000, 0, 5000, 10000, 0 }, 50, new sbyte[7], -1, new List<short> { 189 }, new List<short>(), 12, -1, 3, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, -50, new List<short> { 150 }, new List<short> { 153 }, -1, new List<ShortList>(), -1, new List<short> { 158 }, 915, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(193, 916, 917, 918, "SectSpecial/3305_biguya", "Func_LiveSkill12", EBuildingBlockType.Building, EBuildingBlockClass.Taoism, 1, canOpenManageOutTaiwu: false, 1, "414f8a", new ushort[8] { 0, 1000, 0, 2000, 0, 5000, 0, 1000 }, 50, new sbyte[7], 180, new List<short> { 189, 4 }, new List<short>(), 12, -1, -1, 919, 920, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 12, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 198 }, 921, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(194, 922, 923, 924, "SectSpecial/3306_zhaoyuandong", "Func_LiveSkill12", EBuildingBlockType.Building, EBuildingBlockClass.Taoism, 1, canOpenManageOutTaiwu: false, 1, "414f8a", new ushort[8] { 0, 3000, 0, 5000, 2000, 10000, 0, 5000 }, 50, new sbyte[7], 181, new List<short> { 189, 18 }, new List<short>(), 12, -1, -1, 925, 926, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 12, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 214 }, 927, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(195, 928, 929, 930, "SectSpecial/3307_danfang", "Func_Special", EBuildingBlockType.Building, EBuildingBlockClass.Taoism, 1, canOpenManageOutTaiwu: false, 1, "414f8a", new ushort[8] { 0, 2500, 0, 7500, 5000, 15000, 0, 15000 }, 50, new sbyte[7], 182, new List<short> { 189 }, new List<short>(), 12, -1, 3, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 1000)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 229 }, 931, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(196, 932, 933, 934, "SectSpecial/1022_chanfang", null, EBuildingBlockType.Building, EBuildingBlockClass.Buddhism, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8] { 0, 1000, 0, 0, 1500, 500, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 197, 198, 199, 200, 201, 202 }, 13, -1, 3, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 250, mustMaintenance: false, isUnique: false, 0, 13, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 130 }, 935, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(197, 936, 937, 938, "SectSpecial/3402_siyuan", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Buddhism, 1, canOpenManageOutTaiwu: false, 1, "33393f", new ushort[8] { 0, 1000, 0, 0, 2500, 500, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 196 }, new List<short>(), 13, -1, 3, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, -50, new List<short> { 154 }, new List<short> { 157 }, -1, new List<ShortList>(), -1, new List<short> { 149 }, 939, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(198, 940, 941, 942, "SectSpecial/3403_fota", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Buddhism, 1, canOpenManageOutTaiwu: false, 1, "33393f", new ushort[8] { 0, 3000, 0, 0, 5000, 2000, 0, 2500 }, 50, new sbyte[7], 183, new List<short> { 196 }, new List<short>(), 13, -1, 3, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, 12, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 155 }, new List<short> { 158 }, -1, new List<ShortList>(), -1, new List<short> { 181 }, 943, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(199, 944, 945, 946, "SectSpecial/3404_fatang", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Buddhism, 1, canOpenManageOutTaiwu: false, 1, "33393f", new ushort[8] { 0, 2000, 0, 0, 5000, 1000, 10000, 0 }, 50, new sbyte[7], -1, new List<short> { 196 }, new List<short>(), 13, -1, 3, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, -50, new List<short> { 156 }, new List<short> { 159 }, -1, new List<ShortList>(), -1, new List<short> { 159 }, 947, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(200, 948, 949, 950, "SectSpecial/3405_jianxingdong", "Func_LiveSkill13", EBuildingBlockType.Building, EBuildingBlockClass.Buddhism, 1, canOpenManageOutTaiwu: false, 1, "33393f", new ushort[8] { 0, 2000, 0, 0, 5000, 1000, 0, 1000 }, 50, new sbyte[7], 184, new List<short> { 196, 5 }, new List<short>(), 13, -1, -1, 951, 952, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 13, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 199 }, 953, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(201, 954, 955, 956, "SectSpecial/3406_kurongtai", "Func_LiveSkill13", EBuildingBlockType.Building, EBuildingBlockClass.Buddhism, 1, canOpenManageOutTaiwu: false, 1, "33393f", new ushort[8] { 0, 5000, 0, 2000, 10000, 3000, 0, 5000 }, 50, new sbyte[7], 185, new List<short> { 196, 14 }, new List<short>(), 13, -1, -1, 957, 958, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 13, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 215 }, 959, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(202, 960, 961, 962, "SectSpecial/3407_yuejingge", "Func_Special", EBuildingBlockType.Building, EBuildingBlockClass.Buddhism, 1, canOpenManageOutTaiwu: false, 1, "33393f", new ushort[8] { 0, 7500, 0, 2500, 15000, 5000, 0, 15000 }, 50, new sbyte[7], 186, new List<short> { 196 }, new List<short>(), 13, -1, 3, 282, 235, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 1000)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 5 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 230 }, 963, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(203, 964, 965, 966, "SectSpecial/1023_shijiao", null, EBuildingBlockType.Building, EBuildingBlockClass.Cooking, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8] { 2500, 500, 0, 0, 0, 0, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 204, 205, 206, 207, 208, 209, 210, 211, 212 }, 14, -1, 6, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(0, 10)
		}, 250, mustMaintenance: false, isUnique: false, 0, 14, -1, -1, -1, -1, canMakeItem: true, upgradeMakeItem: false, -1, new short[1], isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 131 }, 967, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: true, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(204, 968, 969, 970, "SectSpecial/3502_jiulou", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Cooking, 1, canOpenManageOutTaiwu: false, 1, "815f47", new ushort[8] { 2500, 1500, 0, 0, 0, 1000, 0, 0 }, 50, new sbyte[7], -1, new List<short> { 203 }, new List<short>(), 14, -1, 6, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(0, 20)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1], isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, 50, new List<short> { 160 }, new List<short> { 165 }, -1, new List<ShortList>(), -1, new List<short> { 141 }, 971, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(205, 972, 973, 974, "SectSpecial/3503_baijiayan", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Cooking, 1, canOpenManageOutTaiwu: false, 1, "815f47", new ushort[8] { 5000, 2500, 0, 0, 0, 1500, 5000, 2500 }, 50, new sbyte[7], 187, new List<short> { 203 }, new List<short>(), 14, -1, 6, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, 12, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1], isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 161 }, new List<short> { 166 }, -1, new List<ShortList>(), -1, new List<short> { 182 }, 975, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(206, 976, 977, 978, "SectSpecial/3504_zhengyange", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Cooking, 1, canOpenManageOutTaiwu: false, 1, "815f47", new ushort[8] { 5000, 1500, 0, 0, 0, 500, 15000, 0 }, 50, new sbyte[7], -1, new List<short> { 203 }, new List<short>(), 14, -1, 6, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(0, 100)
		}, 1000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1], isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 50, new List<short> { 162 }, new List<short> { 167 }, -1, new List<ShortList>(), -1, new List<short> { 160 }, 979, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(207, 980, 981, 982, "SectSpecial/3505_baishouyuan", "Func_LiveSkill14", EBuildingBlockType.Building, EBuildingBlockClass.Cooking, 1, canOpenManageOutTaiwu: false, 1, "815f47", new ushort[8] { 5000, 2000, 0, 0, 0, 1000, 0, 1000 }, 50, new sbyte[7], -1, new List<short> { 203, 10 }, new List<short>(), 14, -1, -1, 983, 984, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(0, 20)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 14, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 200 }, 985, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(208, 986, 987, 988, "SectSpecial/3506_zaowangxuanding", "Func_LiveSkill14", EBuildingBlockType.Building, EBuildingBlockClass.Cooking, 1, canOpenManageOutTaiwu: false, 1, "815f47", new ushort[8] { 12500, 5000, 0, 0, 0, 2500, 0, 5000 }, 50, new sbyte[7], 188, new List<short> { 203, 12 }, new List<short>(), 14, -1, -1, 989, 990, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(0, 100)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 14, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 216 }, 991, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(209, 992, 993, 994, "SectSpecial/3507_sijiyuan", "Func_GetMoreItem", EBuildingBlockType.Building, EBuildingBlockClass.Cooking, 1, canOpenManageOutTaiwu: false, 1, "815f47", new ushort[8] { 20000, 10000, 0, 0, 0, 5000, 0, 7500 }, 50, new sbyte[7], 189, new List<short> { 203, 9 }, new List<short>(), 14, -1, 6, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(0, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1], isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 163 }, new List<short> { 168 }, -1, new List<ShortList>(), -1, new List<short> { 243 }, 995, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(210, 996, 997, 998, "SectSpecial/3508_tianchengxiang", "Func_GetBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Cooking, 1, canOpenManageOutTaiwu: false, 1, "815f47", new ushort[8] { 20000, 10000, 0, 0, 0, 5000, 0, 7500 }, 50, new sbyte[7], 190, new List<short> { 203, 9 }, new List<short>(), 14, -1, 6, 147, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(0, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1], isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 164 }, new List<short> { 169 }, -1, new List<ShortList>(), -1, new List<short> { 244 }, 999, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(211, 1000, 1001, 1002, "SectSpecial/3509_xiangliaoguan", "Func_MakeBetterItem", EBuildingBlockType.Building, EBuildingBlockClass.Cooking, 1, canOpenManageOutTaiwu: false, 1, "815f47", new ushort[8] { 55000, 15000, 0, 0, 0, 10000, 0, 35000 }, 50, new sbyte[7], 191, new List<short> { 203 }, new List<short>(), 14, -1, -1, 1003, 1004, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(0, 200)
		}, 15000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: true, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1005, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(212, 1006, 1007, 1008, "SectSpecial/3510_longgong", "Func_ReduceRequirement", EBuildingBlockType.Building, EBuildingBlockClass.Cooking, 1, canOpenManageOutTaiwu: false, 1, "815f47", new ushort[8] { 25000, 7500, 0, 0, 0, 2500, 25000, 15000 }, 50, new sbyte[7], 192, new List<short> { 203, 20 }, new List<short>(), 14, -1, -1, 1009, 1010, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(0, 100)
		}, 5000, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, 14, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 224 }, 1011, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(213, 1012, 1013, 1014, "SectSpecial/1024_changjie", null, EBuildingBlockType.Building, EBuildingBlockClass.Eclectic, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8] { 500, 500, 500, 500, 500, 500, 0, 0 }, 50, new sbyte[7], -1, new List<short>(), new List<short> { 214, 215, 216, 217, 218, 219, 220, 221, 222, 223 }, 15, -1, 2, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 20, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 250, mustMaintenance: false, isUnique: false, 0, 15, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 3 }, isShop: true, needLeader: true, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 132 }, 1015, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(214, 1016, 1017, 1018, "SectSpecial/3602_shiji", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Eclectic, 1, canOpenManageOutTaiwu: false, 1, "bababa", new ushort[8] { 500, 500, 500, 500, 500, 500, 5000, 500 }, 50, new sbyte[7], -1, new List<short> { 213 }, new List<short>(), 15, -1, 2, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 3 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, 50, new List<short> { 170 }, new List<short> { 178 }, -1, new List<ShortList>(), -1, new List<short> { 142 }, 1019, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(215, 1020, 1021, 1022, "SectSpecial/3603_dufang", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Eclectic, 1, canOpenManageOutTaiwu: false, 1, "bababa", new ushort[8] { 750, 750, 750, 750, 750, 750, 2500, 0 }, 50, new sbyte[7], -1, new List<short> { 213 }, new List<short>(), 15, -1, 2, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 3 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, 50, new List<short> { 171 }, new List<short> { 179 }, -1, new List<ShortList>(), -1, new List<short> { 151 }, 1023, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(216, 1024, 1025, 1026, "SectSpecial/3604_qinglou", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Eclectic, 1, canOpenManageOutTaiwu: false, 1, "bababa", new ushort[8] { 750, 750, 750, 750, 750, 750, 0, 250 }, 50, new sbyte[7], -1, new List<short> { 213 }, new List<short>(), 15, -1, 2, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 500, 500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 50)
		}, 500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 3 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 800, 0, 50, new List<short> { 172 }, new List<short> { 180 }, -1, new List<ShortList>(), -1, new List<short> { 150 }, 1027, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(217, 1028, 1029, 1030, "SectSpecial/3605_huafang", "Func_GetMoney1", EBuildingBlockType.Building, EBuildingBlockClass.Eclectic, 1, canOpenManageOutTaiwu: false, 1, "bababa", new ushort[8] { 2000, 0, 2000, 2000, 2000, 0, 10000, 2500 }, 50, new sbyte[7], 193, new List<short> { 213, 1 }, new List<short>(), 15, -1, 2, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, 9
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 3 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 173 }, new List<short> { 181 }, -1, new List<ShortList>(), -1, new List<short> { 183 }, 1031, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(218, 1032, 1033, 1034, "SectSpecial/3606_goulanwashe", "Func_GetVillager", EBuildingBlockType.Building, EBuildingBlockClass.Eclectic, 1, canOpenManageOutTaiwu: false, 1, "bababa", new ushort[8] { 2000, 0, 2000, 2000, 2000, 0, 10000, 2500 }, 50, new sbyte[7], 194, new List<short> { 213 }, new List<short>(), 15, -1, 2, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, 12
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 50)
		}, 2500, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 3 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 174 }, new List<short> { 182 }, -1, new List<ShortList>(), -1, new List<short> { 184 }, 1035, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(219, 1036, 1037, 1038, "SectSpecial/3607_youyuan", "Func_GetAuthority1", EBuildingBlockType.Building, EBuildingBlockClass.Eclectic, 1, canOpenManageOutTaiwu: false, 1, "bababa", new ushort[8] { 1500, 1500, 1500, 1500, 1500, 1500, 5000, 0 }, 50, new sbyte[7], -1, new List<short> { 213 }, new List<short>(), 15, -1, 2, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 1000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 3 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 175 }, new List<short> { 183 }, -1, new List<ShortList>(), -1, new List<short> { 161 }, 1039, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(220, 1040, 1041, 1042, "SectSpecial/3608_dayouyuan", "Func_LiveSkill15", EBuildingBlockType.Building, EBuildingBlockClass.Eclectic, 1, canOpenManageOutTaiwu: false, 1, "bababa", new ushort[8] { 1000, 1000, 1000, 1000, 1000, 1000, 10000, 1000 }, 50, new sbyte[7], 195, new List<short> { 213, 7 }, new List<short>(), 15, -1, -1, 1043, 1044, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 100)
		}, 500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, -1, 15, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 201 }, 1045, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(221, 1046, 1047, 1048, "SectSpecial/3609_miaopu", "Func_LiveSkill15", EBuildingBlockType.Building, EBuildingBlockClass.Eclectic, 1, canOpenManageOutTaiwu: false, 1, "bababa", new ushort[8] { 2500, 2500, 2500, 2500, 2500, 2500, 25000, 5000 }, 50, new sbyte[7], 196, new List<short> { 213, 15 }, new List<short>(), 15, -1, -1, 1049, 1050, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 750, 750 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 500)
		}, 2500, mustMaintenance: false, isUnique: true, 0, -1, -1, -1, 15, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, new List<short> { 217 }, 1051, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(222, 1052, 1053, 1054, "SectSpecial/3610_dangpu", "Func_GetMoney2", EBuildingBlockType.Building, EBuildingBlockClass.Eclectic, 1, canOpenManageOutTaiwu: false, 1, "bababa", new ushort[8] { 5000, 5000, 5000, 5000, 5000, 5000, 50000, 15000 }, 50, new sbyte[7], -1, new List<short> { 213 }, new List<short>(), 15, -1, 2, 574, 148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(6, 1000)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 3 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 1600, 0, 0, new List<short> { 176 }, new List<short> { 184 }, -1, new List<ShortList>(), -1, new List<short> { 245 }, 1055, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(223, 1056, 1057, 1058, "SectSpecial/3611_xianshiguan", "Func_GetAuthority2", EBuildingBlockType.Building, EBuildingBlockClass.Eclectic, 1, canOpenManageOutTaiwu: false, 1, "bababa", new ushort[8] { 5000, 5000, 5000, 5000, 5000, 5000, 50000, 15000 }, 50, new sbyte[7], -1, new List<short> { 213 }, new List<short>(), 15, -1, 2, 574, 148, new short[16]
		{
			9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
			9, 9, 9, 9, 9, 9
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, 1500 }, 10, new List<ResourceInfo>
		{
			new ResourceInfo(7, 100)
		}, 5000, mustMaintenance: false, isUnique: false, 0, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, new short[1] { 3 }, isShop: true, needLeader: true, needShopProgress: true, isCollectResourceBuilding: false, 4800, 0, 0, new List<short> { 177 }, new List<short> { 185 }, -1, new List<ShortList>(), -1, new List<short> { 185 }, 1059, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(224, 1060, 1061, 1062, "Main/30001_jingcheng", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1063, 1064, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1065, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(225, 1066, 1067, 1068, "Main/30002_chengdu", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1069, 1070, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1071, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(226, 1072, 1073, 1074, "Main/30003_guizhou", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1075, 1076, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1077, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(227, 1078, 1079, 1080, "Main/30004_xiangyang", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1081, 1082, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1083, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(228, 1084, 1085, 1086, "Main/30005_taiyuan", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1087, 1088, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1089, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(229, 1090, 1091, 1092, "Main/30006_guangzhou", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1093, 1094, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1095, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(230, 1096, 1097, 1098, "Main/30007_qingzhou", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1099, 1100, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1101, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(231, 1102, 1103, 1104, "Main/30008_jiangling", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1105, 1106, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1107, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(232, 1108, 1109, 1110, "Main/30009_fuzhou", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1111, 1112, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1113, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(233, 1114, 1115, 1116, "Main/30010_liaoyang", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1117, 1118, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1119, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(234, 1120, 1121, 1122, "Main/30011_qinzhou", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1123, 1124, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1125, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(235, 1126, 1127, 1128, "Main/30012_dali", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1129, 1130, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1131, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(236, 1132, 1133, 1134, "Main/30013_shouchun", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1135, 1136, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1137, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(237, 1138, 1139, 1140, "Main/30014_hangzhou", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1141, 1142, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1143, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(238, 1144, 1145, 1146, "Main/30015_yangzhou", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1147, 1148, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1149, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(239, 1150, 1151, 1152, "Main/31001_shaolinsi", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1153, 1154, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1155, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new BuildingBlockItem(240, 1156, 1157, 1158, "Main/31002_emeipai", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1159, 1160, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1161, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(241, 1162, 1163, 1164, "Main/31003_baihuagu", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1165, 1166, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1167, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(242, 1168, 1169, 1170, "Main/31004_wudangpai", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1171, 1172, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1173, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(243, 1174, 1175, 1176, "Main/31005_daxiaoyuanshan", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1177, 1178, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1179, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(244, 1180, 1181, 1182, "Main/31006_shixiangmen", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1183, 1184, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1185, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(245, 1186, 1187, 1188, "Main/31007_ranshanpai", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1189, 1190, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1191, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(246, 1192, 1193, 1194, "Main/31008_xuannvfeng", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1195, 1196, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1197, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(247, 1198, 1199, 1200, "Main/31009_zhujianshanzhuang", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1201, 1202, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1203, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(248, 1204, 1205, 1206, "Main/31010_kongsangpai", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1207, 1208, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1209, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(249, 1210, 1211, 1212, "Main/31011_wuliangjingangzong", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1213, 1214, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1215, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(250, 1216, 1217, 1218, "Main/31012_wuxianjiao", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1219, 1220, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1221, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(251, 1222, 1223, 1224, "Main/31013_jieqingya", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1225, 1226, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1227, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(252, 1228, 1229, 1230, "Main/31014_fulongtan", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1231, 1232, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1233, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(253, 1234, 1235, 1236, "Main/31015_xuehoujiao", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1237, 1238, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1239, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(254, 1240, 1241, 1242, "Main/32001_cunzhuang", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1243, 1244, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1245, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(255, 1246, 1247, 1248, "Main/32002_shizhen", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1249, 1250, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1251, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(256, 1252, 1253, 1254, "Main/32003_guanzhai", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1255, 1256, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1257, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(257, 1258, 1259, 1260, "Main/40001_zhulu", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1261, 1262, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: true, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1263, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(258, 1258, 1264, 1265, "Main/40001_zhulu", null, EBuildingBlockType.MainBuilding, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 2, null, new ushort[8] { 0, 100, 0, 0, 0, 0, 0, 0 }, 0, new sbyte[7], 233, new List<short>(), new List<short>(), 15, -1, -1, 1266, 1267, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: true, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1268, -1, new string[2] { "buildingarea_industry_icon_4", "buildingarea_industry_base_4" }, new string[2] { "buildingarea_industry_icon_4", "buildingarea_detailed_base_4" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(259, 1269, 1270, 1271, "SectSpecial/50001_cangjingge", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1272, 1273, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1274, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(260, 1275, 1276, 1277, "SectSpecial/50002_jinding", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1278, 1279, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1280, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(261, 1281, 1282, 1283, "SectSpecial/50003_bailutan", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1284, 1285, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1286, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(262, 1287, 1288, 1289, "SectSpecial/50004_qixingtai", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1290, 1291, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1292, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(263, 1293, 1294, 1295, "SectSpecial/50005_yuanshanshilao", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1296, 1297, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1298, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(264, 1299, 1300, 1301, "SectSpecial/50006_shiwanglei", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1302, 1303, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1304, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(265, 1305, 1306, 1307, "SectSpecial/50007_qinglangge", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1308, 1309, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1310, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(266, 1311, 1312, 1313, "SectSpecial/50008_yimingjue", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1314, 1315, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1316, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(267, 1317, 1318, 1319, "SectSpecial/50009_zushijianlu", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1320, 1321, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1322, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(268, 1323, 1324, 1325, "SectSpecial/50010_yaowanglu", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1326, 1327, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1328, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(269, 1329, 1330, 1331, "SectSpecial/50011_jingangzunxiang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1332, 1333, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1334, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(270, 1335, 1336, 1337, "SectSpecial/50012_wuduyugu", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1338, 1339, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1340, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(271, 1341, 1342, 1343, "SectSpecial/50013_wushengyuan", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1344, 1345, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1346, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(272, 1347, 1348, 1349, "SectSpecial/50014_jinlongdian", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1350, 1351, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1352, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(273, 1353, 1354, 1355, "SectSpecial/50015_xuechi", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1356, 1357, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1358, -1, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(274, 1359, 1360, 1361, "Merchant/40004_funiubang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1362, 1363, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), 0, null, 1364, -1, new string[2] { "buildingarea_industry_icon_3", "buildingarea_industry_base_3" }, new string[2] { "buildingarea_industry_icon_3", "buildingarea_detailed_base_3" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(275, 1365, 1366, 1367, "Merchant/40005_wensanshuhaige", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1368, 1369, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), 1, null, 1370, -1, new string[2] { "buildingarea_industry_icon_3", "buildingarea_industry_base_3" }, new string[2] { "buildingarea_industry_icon_3", "buildingarea_detailed_base_3" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(276, 1371, 1372, 1373, "Merchant/40006_wuhushanghui", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1374, 1375, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), 2, null, 1376, -1, new string[2] { "buildingarea_industry_icon_3", "buildingarea_industry_base_3" }, new string[2] { "buildingarea_industry_icon_3", "buildingarea_detailed_base_3" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(277, 1377, 1378, 1379, "Merchant/40007_dawukui", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1380, 1381, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), 3, null, 1382, -1, new string[2] { "buildingarea_industry_icon_3", "buildingarea_industry_base_3" }, new string[2] { "buildingarea_industry_icon_3", "buildingarea_detailed_base_3" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(278, 1383, 1384, 1385, "Merchant/40008_huichuntang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1386, 1387, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), 4, null, 1388, -1, new string[2] { "buildingarea_industry_icon_3", "buildingarea_industry_base_3" }, new string[2] { "buildingarea_industry_icon_3", "buildingarea_detailed_base_3" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(279, 1389, 1390, 1391, "Merchant/40009_gongshufang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1392, 1393, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), 5, null, 1394, -1, new string[2] { "buildingarea_industry_icon_3", "buildingarea_industry_base_3" }, new string[2] { "buildingarea_industry_icon_3", "buildingarea_detailed_base_3" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(280, 1395, 1396, 1397, "Merchant/40010_qihuozai", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1398, 1399, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), 6, null, 1400, -1, new string[2] { "buildingarea_industry_icon_3", "buildingarea_industry_base_3" }, new string[2] { "buildingarea_industry_icon_3", "buildingarea_detailed_base_3" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(281, 1401, 1402, 1403, "SectSpecial/1025_taiwushuyuan", null, EBuildingBlockType.Building, EBuildingBlockClass.Villiage, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8], 50, new sbyte[7], 235, new List<short>(), new List<short>(), 15, -1, -1, 1404, 1405, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, -1 }, 20, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, 1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1406, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 1, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(282, 1407, 1408, 1409, "SectSpecial/1026_taiwuwuyuan", null, EBuildingBlockType.Building, EBuildingBlockClass.Villiage, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8], 50, new sbyte[7], 236, new List<short>(), new List<short>(), 15, -1, -1, 1410, 1411, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { 1500, -1 }, 20, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, 1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1412, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 1, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(283, 1413, 1414, 1415, "SectSpecial/50016_damoshelita", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: false, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1416, 1417, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1418, -1, new string[2] { "buildingarea_industry_icon_7", "buildingarea_industry_base_7" }, new string[2] { "buildingarea_industry_icon_7", "buildingarea_detailed_base_7" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(284, 1419, 1420, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryZhucheng", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1422, 1423, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1424, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(285, 1425, 1420, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryShizhen", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1426, 1427, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1428, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(286, 1429, 1420, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryCunzhuang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1430, 1431, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1432, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(287, 1433, 1420, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryGuanzhai", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1434, 1435, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1436, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(288, 1437, 1438, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryShaolin", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1439, 1440, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1441, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(289, 1442, 1443, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryEmei", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1444, 1445, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1446, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(290, 1447, 1448, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryBaihua", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1449, 1450, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1451, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(291, 1452, 1453, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryWudang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1454, 1455, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1456, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(292, 1457, 1458, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryYuanshan", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1459, 1460, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1461, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(293, 1462, 1463, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryShixiang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1464, 1465, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1466, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(294, 1467, 1468, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryRanshan", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1469, 1470, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1471, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(295, 1472, 1473, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryXuannv", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1474, 1475, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1476, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(296, 1477, 1478, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryZhujian", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1479, 1480, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1481, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(297, 1482, 1483, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryKongsang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1484, 1485, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1486, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(298, 1487, 1488, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryJingang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1489, 1490, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1491, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(299, 1492, 1493, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryWuxian", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1494, 1495, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1496, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new BuildingBlockItem(300, 1497, 1498, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryJieqing", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1499, 1500, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1501, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(301, 1502, 1503, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryFulong", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1504, 1505, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1506, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(302, 1507, 1508, 1421, "SettlementTreasury/Icon_Buildingblock_TreasuryXuehou", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1509, 1510, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1511, -1, new string[2] { "buildingarea_industry_icon_1", "buildingarea_industry_base_1" }, new string[2] { "buildingarea_industry_icon_1", "buildingarea_detailed_base_1" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(303, 1512, 1513, 1514, "SettlementPrison/Icon_Buildingblock_PrisonShaolin", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1515, 1516, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1517, 1, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(304, 1518, 1519, 1514, "SettlementPrison/Icon_Buildingblock_PrisonEmei", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1520, 1521, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1522, 2, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(305, 1523, 1524, 1514, "SettlementPrison/Icon_Buildingblock_PrisonBaihua", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1525, 1526, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1527, 3, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(306, 1528, 1529, 1514, "SettlementPrison/Icon_Buildingblock_PrisonWudang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1530, 1531, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1532, 4, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(307, 1533, 1534, 1514, "SettlementPrison/Icon_Buildingblock_PrisonYuanshan", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1535, 1536, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1537, 5, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(308, 1538, 1539, 1514, "SettlementPrison/Icon_Buildingblock_PrisonShixiang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1540, 1541, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1542, 6, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(309, 1543, 1544, 1514, "SettlementPrison/Icon_Buildingblock_PrisonRanshan", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1545, 1546, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1547, 7, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(310, 1548, 1549, 1514, "SettlementPrison/Icon_Buildingblock_PrisonXuannv", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1550, 1551, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1552, 8, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(311, 1553, 1554, 1514, "SettlementPrison/Icon_Buildingblock_PrisonZhujian", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1555, 1556, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1557, 9, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(312, 1558, 1559, 1514, "SettlementPrison/Icon_Buildingblock_PrisonKongsang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1560, 1561, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1562, 10, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(313, 1563, 1564, 1514, "SettlementPrison/Icon_Buildingblock_PrisonJingang", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1565, 1566, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1567, 11, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(314, 1568, 1569, 1514, "SettlementPrison/Icon_Buildingblock_PrisonWuxian", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1570, 1571, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1572, 12, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(315, 1573, 1574, 1514, "SettlementPrison/Icon_Buildingblock_PrisonJieqing", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1575, 1576, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1577, 13, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(316, 1578, 1579, 1514, "SettlementPrison/Icon_Buildingblock_PrisonFulong", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1580, 1581, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1582, 14, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(317, 1583, 1584, 1514, "SettlementPrison/Icon_Buildingblock_PrisonXuehou", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1585, 1586, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: false, isUnique: false, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1587, 15, new string[2] { "buildingarea_industry_icon_2", "buildingarea_industry_base_2" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(318, 1588, 1589, 1590, "Merchant/40006_wuhushanghui", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8], 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1591, 1592, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), 2, null, 1593, -1, new string[2] { "buildingarea_industry_icon_3", "buildingarea_industry_base_3" }, new string[2] { "buildingarea_industry_icon_2", "buildingarea_detailed_base_2" }, artisanOrderAvailable: false, 0, new List<short>(), -1));
		_dataArray.Add(new BuildingBlockItem(319, 1594, 1595, 1596, "Main/gang_qiwenxingtai", null, EBuildingBlockType.Building, EBuildingBlockClass.Static, 1, canOpenManageOutTaiwu: true, 1, null, new ushort[8] { 10000, 10000, 10000, 10000, 10000, 10000, 10000, 5000 }, 0, new sbyte[7], -1, new List<short>(), new List<short>(), 15, -1, -1, 1597, 1598, new short[16]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1
		}, new short[14]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1
		}, new short[2] { -1, -1 }, -1, new List<ResourceInfo>(), -1, mustMaintenance: true, isUnique: true, -1, -1, -1, -1, -1, -1, canMakeItem: false, upgradeMakeItem: false, -1, null, isShop: false, needLeader: false, needShopProgress: false, isCollectResourceBuilding: false, -1, 0, 0, new List<short>(), new List<short>(), -1, new List<ShortList>(), -1, null, 1599, 13, new string[2] { "buildingarea_industry_icon_5", "buildingarea_industry_base_5" }, new string[2] { "buildingarea_industry_icon_5", "buildingarea_detailed_base_5" }, artisanOrderAvailable: false, 0, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 300));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<BuildingBlockItem>(320);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
		CreateItems5();
	}
}
