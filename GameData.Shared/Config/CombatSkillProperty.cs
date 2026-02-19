using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class CombatSkillProperty : ConfigData<CombatSkillPropertyItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte Power = 0;

		public const sbyte MaxPower = 1;

		public const sbyte PrepareTotalProgress = 2;

		public const sbyte BreathStanceTotalCost = 3;

		public const sbyte BaseInnerRatio = 4;

		public const sbyte InnerRatioChangeRange = 5;

		public const sbyte TotalObtainableNeili = 6;

		public const sbyte ObtainedNeiliPerLoop = 7;

		public const sbyte FiveElementChangePerLoop = 8;

		public const sbyte GenericGrid = 9;

		public const sbyte MobilityCost = 10;

		public const sbyte AddMoveSpeedOnCast = 11;

		public const sbyte AddHitOnCast0 = 12;

		public const sbyte AddHitOnCast1 = 13;

		public const sbyte AddHitOnCast2 = 14;

		public const sbyte AddHitOnCast3 = 15;

		public const sbyte MobilityReduceSpeed = 16;

		public const sbyte MoveCostMobility = 17;

		public const sbyte AddOuterPenetrateResistOnCast = 18;

		public const sbyte AddInnerPenetrateResistOnCast = 19;

		public const sbyte AddAvoidOnCast0 = 20;

		public const sbyte AddAvoidOnCast1 = 21;

		public const sbyte AddAvoidOnCast2 = 22;

		public const sbyte AddAvoidOnCast3 = 23;

		public const sbyte FightBackDamage = 24;

		public const sbyte BounceRateOfOuterInjury = 25;

		public const sbyte BounceRateOfInnerInjury = 26;

		public const sbyte BounceDistance = 27;

		public const sbyte ContinuousFrames = 28;

		public const sbyte AddPercentPenetrate = 29;

		public const sbyte AddPercentTotalHit = 30;

		public const sbyte PerHitDamageRateDistribution0 = 31;

		public const sbyte PerHitDamageRateDistribution1 = 32;

		public const sbyte PerHitDamageRateDistribution2 = 33;

		public const sbyte DistanceAdditionWhenCast = 34;

		public const sbyte InjuryPartAtkRateDistributionChest = 35;

		public const sbyte InjuryPartAtkRateDistributionBelly = 36;

		public const sbyte InjuryPartAtkRateDistributionHead = 37;

		public const sbyte InjuryPartAtkRateDistributionHand = 38;

		public const sbyte InjuryPartAtkRateDistributionLeg = 39;

		public const sbyte AcupointLevel = 40;

		public const sbyte FlawLevel = 41;

		public const sbyte Poisons0 = 42;

		public const sbyte Poisons1 = 43;

		public const sbyte Poisons2 = 44;

		public const sbyte Poisons3 = 45;

		public const sbyte Poisons4 = 46;

		public const sbyte Poisons5 = 47;

		public const sbyte Requirements = 48;

		public const sbyte SlotCountAttack = 49;

		public const sbyte SlotCountAgile = 50;

		public const sbyte SlotCountDefense = 51;

		public const sbyte SlotCountAssist = 52;

		public const sbyte NeedTrick0 = 53;

		public const sbyte NeedTrick1 = 54;

		public const sbyte NeedTrick2 = 55;

		public const sbyte NeedTrick3 = 56;

		public const sbyte NeedTrick4 = 57;

		public const sbyte NeedTrick5 = 58;

		public const sbyte NeedTrick6 = 59;

		public const sbyte NeedTrick7 = 60;

		public const sbyte NeedTrick8 = 61;

		public const sbyte NeedTrick9 = 62;

		public const sbyte NeedTrick10 = 63;

		public const sbyte NeedTrick11 = 64;

		public const sbyte NeedTrick12 = 65;

		public const sbyte NeedTrick13 = 66;

		public const sbyte NeedTrick14 = 67;

		public const sbyte NeedTrick15 = 68;

		public const sbyte AgileJumpSpeed = 69;

		public const sbyte SilenceRate = 70;

		public const sbyte SilenceFrame = 71;

		public const sbyte AddPenetrate = 72;

		public const sbyte AddTotalHit = 73;
	}

	public static class DefValue
	{
		public static CombatSkillPropertyItem Power => Instance[(sbyte)0];

		public static CombatSkillPropertyItem MaxPower => Instance[(sbyte)1];

		public static CombatSkillPropertyItem PrepareTotalProgress => Instance[(sbyte)2];

		public static CombatSkillPropertyItem BreathStanceTotalCost => Instance[(sbyte)3];

		public static CombatSkillPropertyItem BaseInnerRatio => Instance[(sbyte)4];

		public static CombatSkillPropertyItem InnerRatioChangeRange => Instance[(sbyte)5];

		public static CombatSkillPropertyItem TotalObtainableNeili => Instance[(sbyte)6];

		public static CombatSkillPropertyItem ObtainedNeiliPerLoop => Instance[(sbyte)7];

		public static CombatSkillPropertyItem FiveElementChangePerLoop => Instance[(sbyte)8];

		public static CombatSkillPropertyItem GenericGrid => Instance[(sbyte)9];

		public static CombatSkillPropertyItem MobilityCost => Instance[(sbyte)10];

		public static CombatSkillPropertyItem AddMoveSpeedOnCast => Instance[(sbyte)11];

		public static CombatSkillPropertyItem AddHitOnCast0 => Instance[(sbyte)12];

		public static CombatSkillPropertyItem AddHitOnCast1 => Instance[(sbyte)13];

		public static CombatSkillPropertyItem AddHitOnCast2 => Instance[(sbyte)14];

		public static CombatSkillPropertyItem AddHitOnCast3 => Instance[(sbyte)15];

		public static CombatSkillPropertyItem MobilityReduceSpeed => Instance[(sbyte)16];

		public static CombatSkillPropertyItem MoveCostMobility => Instance[(sbyte)17];

		public static CombatSkillPropertyItem AddOuterPenetrateResistOnCast => Instance[(sbyte)18];

		public static CombatSkillPropertyItem AddInnerPenetrateResistOnCast => Instance[(sbyte)19];

		public static CombatSkillPropertyItem AddAvoidOnCast0 => Instance[(sbyte)20];

		public static CombatSkillPropertyItem AddAvoidOnCast1 => Instance[(sbyte)21];

		public static CombatSkillPropertyItem AddAvoidOnCast2 => Instance[(sbyte)22];

		public static CombatSkillPropertyItem AddAvoidOnCast3 => Instance[(sbyte)23];

		public static CombatSkillPropertyItem FightBackDamage => Instance[(sbyte)24];

		public static CombatSkillPropertyItem BounceRateOfOuterInjury => Instance[(sbyte)25];

		public static CombatSkillPropertyItem BounceRateOfInnerInjury => Instance[(sbyte)26];

		public static CombatSkillPropertyItem BounceDistance => Instance[(sbyte)27];

		public static CombatSkillPropertyItem ContinuousFrames => Instance[(sbyte)28];

		public static CombatSkillPropertyItem AddPercentPenetrate => Instance[(sbyte)29];

		public static CombatSkillPropertyItem AddPercentTotalHit => Instance[(sbyte)30];

		public static CombatSkillPropertyItem PerHitDamageRateDistribution0 => Instance[(sbyte)31];

		public static CombatSkillPropertyItem PerHitDamageRateDistribution1 => Instance[(sbyte)32];

		public static CombatSkillPropertyItem PerHitDamageRateDistribution2 => Instance[(sbyte)33];

		public static CombatSkillPropertyItem DistanceAdditionWhenCast => Instance[(sbyte)34];

		public static CombatSkillPropertyItem InjuryPartAtkRateDistributionChest => Instance[(sbyte)35];

		public static CombatSkillPropertyItem InjuryPartAtkRateDistributionBelly => Instance[(sbyte)36];

		public static CombatSkillPropertyItem InjuryPartAtkRateDistributionHead => Instance[(sbyte)37];

		public static CombatSkillPropertyItem InjuryPartAtkRateDistributionHand => Instance[(sbyte)38];

		public static CombatSkillPropertyItem InjuryPartAtkRateDistributionLeg => Instance[(sbyte)39];

		public static CombatSkillPropertyItem AcupointLevel => Instance[(sbyte)40];

		public static CombatSkillPropertyItem FlawLevel => Instance[(sbyte)41];

		public static CombatSkillPropertyItem Poisons0 => Instance[(sbyte)42];

		public static CombatSkillPropertyItem Poisons1 => Instance[(sbyte)43];

		public static CombatSkillPropertyItem Poisons2 => Instance[(sbyte)44];

		public static CombatSkillPropertyItem Poisons3 => Instance[(sbyte)45];

		public static CombatSkillPropertyItem Poisons4 => Instance[(sbyte)46];

		public static CombatSkillPropertyItem Poisons5 => Instance[(sbyte)47];

		public static CombatSkillPropertyItem Requirements => Instance[(sbyte)48];

		public static CombatSkillPropertyItem SlotCountAttack => Instance[(sbyte)49];

		public static CombatSkillPropertyItem SlotCountAgile => Instance[(sbyte)50];

		public static CombatSkillPropertyItem SlotCountDefense => Instance[(sbyte)51];

		public static CombatSkillPropertyItem SlotCountAssist => Instance[(sbyte)52];

		public static CombatSkillPropertyItem NeedTrick0 => Instance[(sbyte)53];

		public static CombatSkillPropertyItem NeedTrick1 => Instance[(sbyte)54];

		public static CombatSkillPropertyItem NeedTrick2 => Instance[(sbyte)55];

		public static CombatSkillPropertyItem NeedTrick3 => Instance[(sbyte)56];

		public static CombatSkillPropertyItem NeedTrick4 => Instance[(sbyte)57];

		public static CombatSkillPropertyItem NeedTrick5 => Instance[(sbyte)58];

		public static CombatSkillPropertyItem NeedTrick6 => Instance[(sbyte)59];

		public static CombatSkillPropertyItem NeedTrick7 => Instance[(sbyte)60];

		public static CombatSkillPropertyItem NeedTrick8 => Instance[(sbyte)61];

		public static CombatSkillPropertyItem NeedTrick9 => Instance[(sbyte)62];

		public static CombatSkillPropertyItem NeedTrick10 => Instance[(sbyte)63];

		public static CombatSkillPropertyItem NeedTrick11 => Instance[(sbyte)64];

		public static CombatSkillPropertyItem NeedTrick12 => Instance[(sbyte)65];

		public static CombatSkillPropertyItem NeedTrick13 => Instance[(sbyte)66];

		public static CombatSkillPropertyItem NeedTrick14 => Instance[(sbyte)67];

		public static CombatSkillPropertyItem NeedTrick15 => Instance[(sbyte)68];

		public static CombatSkillPropertyItem AgileJumpSpeed => Instance[(sbyte)69];

		public static CombatSkillPropertyItem SilenceRate => Instance[(sbyte)70];

		public static CombatSkillPropertyItem SilenceFrame => Instance[(sbyte)71];

		public static CombatSkillPropertyItem AddPenetrate => Instance[(sbyte)72];

		public static CombatSkillPropertyItem AddTotalHit => Instance[(sbyte)73];
	}

	public static CombatSkillProperty Instance = new CombatSkillProperty();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "PlusColor", "MinusColor", "TipsSmallIcon", "TipsIcon", "DisplayFix" };

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
		_dataArray.Add(new CombatSkillPropertyItem(0, 0, isPercent: true, "brightblue", "brightred", "mousetip_power", "mousetip_power_big", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(1, 1, isPercent: true, "brightblue", "brightred", "mousetip_maxpower", "mousetip_maxpower_big", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(2, 2, isPercent: true, "brightred", "brightblue", "mousetip_ciyao_2", "mousetip_ciyao_big_2", isInverse: true, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(3, 3, isPercent: true, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(4, 4, isPercent: true, "yellow", "yellow", "mousetip_neigongbili", "mousetip_neigongbili_big", isInverse: false, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(5, 5, isPercent: true, "brightblue", "brightred", null, "mousetip_neigongzhuanhuan_big", isInverse: false, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(6, 6, isPercent: false, "brightblue", "brightred", "mousetip_neilizongliang", "mousetip_neilizongliang_big", isInverse: false, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(7, 7, isPercent: false, "brightblue", "brightred", "mousetip_huoquneili", "mousetip_huoquneili_big", isInverse: false, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(8, 8, isPercent: false, "yellow", "yellow", null, "mousetip_wuxingzhuanyi_big", isInverse: false, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(9, 9, isPercent: false, "brightblue", "brightred", "mousetip_zhenqi_4", "mousetip_zhenqi_big_4", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(10, 10, isPercent: true, "brightred", "brightblue", "mousetip_mobilitycost", "mousetip_mobilitycost_big", isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(11, 11, isPercent: true, "brightblue", "brightred", "mousetip_ciyao_1", "mousetip_ciyao_big_1", isInverse: false, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(12, 12, isPercent: true, "brightblue", "brightred", "mousetip_mingzhong_0", "mousetip_mingzhong_big_0", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(13, 13, isPercent: true, "brightblue", "brightred", "mousetip_mingzhong_1", "mousetip_mingzhong_big_1", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(14, 14, isPercent: true, "brightblue", "brightred", "mousetip_mingzhong_2", "mousetip_mingzhong_big_2", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(15, 15, isPercent: true, "brightblue", "brightred", "mousetip_mingzhong_3", "mousetip_mingzhong_big_3", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(16, 16, isPercent: true, "brightred", "brightblue", "mousetip_shenfachixu", "mousetip_shenfachixu_big", isInverse: true, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(17, 17, isPercent: true, "brightred", "brightblue", "mousetip_shenfaxiaohao", "mousetip_shenfaxiaohao_big", isInverse: true, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(18, 18, isPercent: true, "brightblue", "brightred", "mousetip_fangyu_0", "mousetip_fangyu_big_0", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(19, 19, isPercent: true, "brightblue", "brightred", "mousetip_fangyu_1", "mousetip_fangyu_big_1", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(20, 20, isPercent: true, "brightblue", "brightred", "mousetip_huajie_0", "mousetip_huajie_big_0", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(21, 21, isPercent: true, "brightblue", "brightred", "mousetip_huajie_1", "mousetip_huajie_big_1", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(22, 22, isPercent: true, "brightblue", "brightred", "mousetip_huajie_2", "mousetip_huajie_big_2", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(23, 23, isPercent: true, "brightblue", "brightred", "mousetip_huajie_3", "mousetip_huajie_big_3", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(24, 24, isPercent: true, "brightblue", "brightred", "mousetip_fightbackdamage", "mousetip_fightbackdamage_big", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(25, 25, isPercent: true, "brightblue", "brightred", "mousetip_waishang", "mousetip_waishang_big", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(26, 26, isPercent: true, "brightblue", "brightred", "mousetip_neishang", "mousetip_neishang_big", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(27, 27, isPercent: false, "brightblue", "brightred", null, null, isInverse: false, -10, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(28, 28, isPercent: true, "brightblue", "brightred", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(29, 29, isPercent: true, "brightblue", "brightred", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(30, 30, isPercent: true, "brightblue", "brightred", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(31, 31, isPercent: false, "yellow", "yellow", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(32, 32, isPercent: false, "yellow", "yellow", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(33, 33, isPercent: false, "yellow", "yellow", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(34, 34, isPercent: false, "brightblue", "brightred", null, null, isInverse: false, -10, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(35, 35, isPercent: true, "yellow", "yellow", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(36, 36, isPercent: true, "yellow", "yellow", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(37, 37, isPercent: true, "yellow", "yellow", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(38, 38, isPercent: true, "yellow", "yellow", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(39, 39, isPercent: true, "yellow", "yellow", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(40, 40, isPercent: false, "brightblue", "brightred", "mousetip_dianxuejibie", "mousetip_dianxuejibie_big", isInverse: false, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(41, 41, isPercent: false, "brightblue", "brightred", "mousetip_pozhanjibie", "mousetip_pozhanjibie_big", isInverse: false, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(42, 42, isPercent: true, "brightblue", "brightred", "mousetip_duxing_0", "mousetip_duxing_big_0", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(43, 43, isPercent: true, "brightblue", "brightred", "mousetip_duxing_1", "mousetip_duxing_big_1", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(44, 44, isPercent: true, "brightblue", "brightred", "mousetip_duxing_2", "mousetip_duxing_big_2", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(45, 45, isPercent: true, "brightblue", "brightred", "mousetip_duxing_3", "mousetip_duxing_big_3", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(46, 46, isPercent: true, "brightblue", "brightred", "mousetip_duxing_4", "mousetip_duxing_big_4", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(47, 47, isPercent: true, "brightblue", "brightred", "mousetip_duxing_5", "mousetip_duxing_big_5", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(48, 48, isPercent: true, "brightred", "brightblue", "mousetip_requirements", "mousetip_requirements_big", isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(49, 49, isPercent: false, "brightblue", "brightred", "mousetip_zhenqi_0", "mousetip_zhenqi_big_0", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(50, 50, isPercent: false, "brightblue", "brightred", "mousetip_zhenqi_1", "mousetip_zhenqi_big_1", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(51, 51, isPercent: false, "brightblue", "brightred", "mousetip_zhenqi_2", "mousetip_zhenqi_big_2", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(52, 52, isPercent: false, "brightblue", "brightred", "mousetip_zhenqi_3", "mousetip_zhenqi_big_3", isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(53, 53, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(54, 54, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(55, 55, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(56, 56, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(57, 57, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(58, 58, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(59, 59, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new CombatSkillPropertyItem(60, 60, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(61, 61, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(62, 62, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(63, 63, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(64, 64, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(65, 65, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(66, 66, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(67, 67, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(68, 68, isPercent: false, "brightred", "brightblue", null, null, isInverse: true, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(69, 69, isPercent: true, "brightblue", "brightred", null, "mousetip_xulishijian_big", isInverse: false, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(70, 70, isPercent: true, "brightred", "brightblue", "mousetip_fengjinggailv", "mousetip_fengjinggailv_big", isInverse: true, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(71, 71, isPercent: false, "brightred", "brightblue", "mousetip_fengjingshijian", "mousetip_fengjingshijian_big", isInverse: true, -1, isDisplaySpecially: false));
		_dataArray.Add(new CombatSkillPropertyItem(72, 72, isPercent: false, "brightblue", "brightred", null, null, isInverse: false, -1, isDisplaySpecially: true));
		_dataArray.Add(new CombatSkillPropertyItem(73, 73, isPercent: false, "brightblue", "brightred", null, null, isInverse: false, -1, isDisplaySpecially: true));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CombatSkillPropertyItem>(74);
		CreateItems0();
		CreateItems1();
	}
}
