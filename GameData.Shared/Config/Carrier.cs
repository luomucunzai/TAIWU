using System;
using System.Collections.Generic;
using Config.Common;
using GameData.Domains.Item;

namespace Config;

[Serializable]
public class Carrier : ConfigData<CarrierItem, short>
{
	public static class DefKey
	{
		public const short CarDrop0 = 0;

		public const short CarDrop1 = 1;

		public const short CarDrop2 = 2;

		public const short CarDrop3 = 3;

		public const short CarDrop4 = 4;

		public const short CarDrop5 = 5;

		public const short CarDrop6 = 6;

		public const short CarDrop7 = 7;

		public const short CarDrop8 = 8;

		public const short CarCapture0 = 9;

		public const short CarCapture1 = 10;

		public const short CarCapture2 = 11;

		public const short CarCapture3 = 12;

		public const short CarCapture4 = 13;

		public const short CarCapture5 = 14;

		public const short CarCapture6 = 15;

		public const short CarCapture7 = 16;

		public const short CarCapture8 = 17;

		public const short Horse0 = 18;

		public const short Horse1 = 19;

		public const short Horse2 = 20;

		public const short Horse3 = 21;

		public const short Horse4 = 22;

		public const short Horse5 = 23;

		public const short Horse6 = 24;

		public const short Horse7 = 25;

		public const short Horse8 = 26;

		public const short Monkey0 = 27;

		public const short Eagle0 = 28;

		public const short Pig0 = 29;

		public const short Bear0 = 30;

		public const short Bull0 = 31;

		public const short Snake0 = 32;

		public const short Jaguar0 = 33;

		public const short Lion0 = 34;

		public const short Tiger0 = 35;

		public const short Monkey1 = 36;

		public const short Eagle1 = 37;

		public const short Pig1 = 38;

		public const short Bear1 = 39;

		public const short Bull1 = 40;

		public const short Snake1 = 41;

		public const short Jaguar1 = 42;

		public const short Lion1 = 43;

		public const short Tiger1 = 44;

		public const short HorseOfHuanxin = 45;

		public const short JiaoWhite = 46;

		public const short JiaoBlack = 47;

		public const short JiaoGreen = 48;

		public const short JiaoRed = 49;

		public const short JiaoYellow = 50;

		public const short JiaoWB = 51;

		public const short JiaoWG = 52;

		public const short JiaoWR = 53;

		public const short JiaoWY = 54;

		public const short JiaoBG = 55;

		public const short JiaoBR = 56;

		public const short JiaoBY = 57;

		public const short JiaoGR = 58;

		public const short JiaoGY = 59;

		public const short JiaoRY = 60;

		public const short JiaoWBG = 61;

		public const short JiaoWBR = 62;

		public const short JiaoWBY = 63;

		public const short JiaoWGR = 64;

		public const short JiaoWGY = 65;

		public const short JiaoWRY = 66;

		public const short JiaoBGR = 67;

		public const short JiaoBGY = 68;

		public const short JiaoBRY = 69;

		public const short JiaoGRY = 70;

		public const short JiaoWBGR = 71;

		public const short JiaoWBGY = 72;

		public const short JiaoWBRY = 73;

		public const short JiaoWGRY = 74;

		public const short JiaoBGRY = 75;

		public const short JiaoWGRYB = 76;

		public const short Qiuniu = 77;

		public const short Yazi = 78;

		public const short Chaofeng = 79;

		public const short Pulao = 80;

		public const short Suanni = 81;

		public const short Baxia = 82;

		public const short Bian = 83;

		public const short Fuxi = 84;

		public const short Chiwen = 85;
	}

	public static class DefValue
	{
		public static CarrierItem CarDrop0 => Instance[(short)0];

		public static CarrierItem CarDrop1 => Instance[(short)1];

		public static CarrierItem CarDrop2 => Instance[(short)2];

		public static CarrierItem CarDrop3 => Instance[(short)3];

		public static CarrierItem CarDrop4 => Instance[(short)4];

		public static CarrierItem CarDrop5 => Instance[(short)5];

		public static CarrierItem CarDrop6 => Instance[(short)6];

		public static CarrierItem CarDrop7 => Instance[(short)7];

		public static CarrierItem CarDrop8 => Instance[(short)8];

		public static CarrierItem CarCapture0 => Instance[(short)9];

		public static CarrierItem CarCapture1 => Instance[(short)10];

		public static CarrierItem CarCapture2 => Instance[(short)11];

		public static CarrierItem CarCapture3 => Instance[(short)12];

		public static CarrierItem CarCapture4 => Instance[(short)13];

		public static CarrierItem CarCapture5 => Instance[(short)14];

		public static CarrierItem CarCapture6 => Instance[(short)15];

		public static CarrierItem CarCapture7 => Instance[(short)16];

		public static CarrierItem CarCapture8 => Instance[(short)17];

		public static CarrierItem Horse0 => Instance[(short)18];

		public static CarrierItem Horse1 => Instance[(short)19];

		public static CarrierItem Horse2 => Instance[(short)20];

		public static CarrierItem Horse3 => Instance[(short)21];

		public static CarrierItem Horse4 => Instance[(short)22];

		public static CarrierItem Horse5 => Instance[(short)23];

		public static CarrierItem Horse6 => Instance[(short)24];

		public static CarrierItem Horse7 => Instance[(short)25];

		public static CarrierItem Horse8 => Instance[(short)26];

		public static CarrierItem Monkey0 => Instance[(short)27];

		public static CarrierItem Eagle0 => Instance[(short)28];

		public static CarrierItem Pig0 => Instance[(short)29];

		public static CarrierItem Bear0 => Instance[(short)30];

		public static CarrierItem Bull0 => Instance[(short)31];

		public static CarrierItem Snake0 => Instance[(short)32];

		public static CarrierItem Jaguar0 => Instance[(short)33];

		public static CarrierItem Lion0 => Instance[(short)34];

		public static CarrierItem Tiger0 => Instance[(short)35];

		public static CarrierItem Monkey1 => Instance[(short)36];

		public static CarrierItem Eagle1 => Instance[(short)37];

		public static CarrierItem Pig1 => Instance[(short)38];

		public static CarrierItem Bear1 => Instance[(short)39];

		public static CarrierItem Bull1 => Instance[(short)40];

		public static CarrierItem Snake1 => Instance[(short)41];

		public static CarrierItem Jaguar1 => Instance[(short)42];

		public static CarrierItem Lion1 => Instance[(short)43];

		public static CarrierItem Tiger1 => Instance[(short)44];

		public static CarrierItem HorseOfHuanxin => Instance[(short)45];

		public static CarrierItem JiaoWhite => Instance[(short)46];

		public static CarrierItem JiaoBlack => Instance[(short)47];

		public static CarrierItem JiaoGreen => Instance[(short)48];

		public static CarrierItem JiaoRed => Instance[(short)49];

		public static CarrierItem JiaoYellow => Instance[(short)50];

		public static CarrierItem JiaoWB => Instance[(short)51];

		public static CarrierItem JiaoWG => Instance[(short)52];

		public static CarrierItem JiaoWR => Instance[(short)53];

		public static CarrierItem JiaoWY => Instance[(short)54];

		public static CarrierItem JiaoBG => Instance[(short)55];

		public static CarrierItem JiaoBR => Instance[(short)56];

		public static CarrierItem JiaoBY => Instance[(short)57];

		public static CarrierItem JiaoGR => Instance[(short)58];

		public static CarrierItem JiaoGY => Instance[(short)59];

		public static CarrierItem JiaoRY => Instance[(short)60];

		public static CarrierItem JiaoWBG => Instance[(short)61];

		public static CarrierItem JiaoWBR => Instance[(short)62];

		public static CarrierItem JiaoWBY => Instance[(short)63];

		public static CarrierItem JiaoWGR => Instance[(short)64];

		public static CarrierItem JiaoWGY => Instance[(short)65];

		public static CarrierItem JiaoWRY => Instance[(short)66];

		public static CarrierItem JiaoBGR => Instance[(short)67];

		public static CarrierItem JiaoBGY => Instance[(short)68];

		public static CarrierItem JiaoBRY => Instance[(short)69];

		public static CarrierItem JiaoGRY => Instance[(short)70];

		public static CarrierItem JiaoWBGR => Instance[(short)71];

		public static CarrierItem JiaoWBGY => Instance[(short)72];

		public static CarrierItem JiaoWBRY => Instance[(short)73];

		public static CarrierItem JiaoWGRY => Instance[(short)74];

		public static CarrierItem JiaoBGRY => Instance[(short)75];

		public static CarrierItem JiaoWGRYB => Instance[(short)76];

		public static CarrierItem Qiuniu => Instance[(short)77];

		public static CarrierItem Yazi => Instance[(short)78];

		public static CarrierItem Chaofeng => Instance[(short)79];

		public static CarrierItem Pulao => Instance[(short)80];

		public static CarrierItem Suanni => Instance[(short)81];

		public static CarrierItem Baxia => Instance[(short)82];

		public static CarrierItem Bian => Instance[(short)83];

		public static CarrierItem Fuxi => Instance[(short)84];

		public static CarrierItem Chiwen => Instance[(short)85];
	}

	public static Carrier Instance = new Carrier();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"ItemSubType", "GroupId", "ResourceType", "MakeItemSubType", "EquipmentEffectId", "CharacterIdInCombat", "CombatState", "LoveFoodType", "HateFoodType", "TravelSkeleton",
		"TemplateId", "Name", "Grade", "Icon", "Desc", "MaxDurability", "BaseWeight", "BaseHappinessChange", "DropRate", "TamePoint",
		"StandDisplay"
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
		_dataArray.Add(new CarrierItem(0, 0, 4, 400, 0, 0, "icon_Carrier_dulunche", 1, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 1, 36, 178, 7, -1, 10, 4000, 1, 20, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 0, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(1, 2, 4, 400, 1, 0, "icon_Carrier_dulunche", 3, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 1, 36, 178, 7, -1, 15, 6000, 1, 30, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 1, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(2, 4, 4, 400, 2, 0, "icon_Carrier_dulunche", 5, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 1, 36, 178, 7, -1, 20, 8000, 1, 40, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 2, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(3, 6, 4, 400, 3, 0, "icon_Carrier_shuangyuananche", 7, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 1, 36, 178, 7, -1, 25, 10000, 2, 50, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 3, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(4, 8, 4, 400, 4, 0, "icon_Carrier_shuangyuananche", 9, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 1, 36, 178, 7, -1, 30, 12000, 2, 60, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 4, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(5, 10, 4, 400, 5, 0, "icon_Carrier_shuangyuananche", 11, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 1, 36, 178, 7, -1, 35, 14000, 2, 70, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 5, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(6, 12, 4, 400, 6, 0, "icon_Carrier_qilinhuagaiche", 13, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 1, 36, 178, 7, -1, 40, 16000, 3, 80, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 6, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(7, 14, 4, 400, 7, 0, "icon_Carrier_bajiaobaolouche", 15, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 1, 36, 178, 7, -1, 45, 18000, 3, 90, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 7, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(8, 16, 4, 400, 8, 0, "icon_Carrier_xuanlongyu", 17, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 1, 36, 178, 7, -1, 50, 20000, 3, 100, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 8, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(9, 18, 4, 400, 0, 9, "icon_Carrier_zhubanqiao", 19, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 1, 36, 179, 7, -1, 10, 4000, 1, 0, 20, 10, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 9, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(10, 20, 4, 400, 1, 9, "icon_Carrier_zhubanqiao", 21, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 1, 36, 179, 7, -1, 15, 6000, 1, 0, 30, 15, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 10, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(11, 22, 4, 400, 2, 9, "icon_Carrier_zhubanqiao", 23, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 1, 36, 179, 7, -1, 20, 8000, 1, 0, 40, 20, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 11, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(12, 24, 4, 400, 3, 9, "icon_Carrier_pulundache", 25, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 1, 36, 179, 7, -1, 25, 10000, 2, 0, 50, 25, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 12, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(13, 26, 4, 400, 4, 9, "icon_Carrier_pulundache", 27, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 1, 36, 179, 7, -1, 30, 12000, 2, 0, 60, 30, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 13, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(14, 28, 4, 400, 5, 9, "icon_Carrier_pulundache", 29, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 1, 36, 179, 7, -1, 35, 14000, 2, 0, 70, 35, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 14, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(15, 30, 4, 400, 6, 9, "icon_Carrier_fenghouche", 31, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 1, 36, 179, 7, -1, 40, 16000, 3, 0, 80, 40, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 15, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(16, 32, 4, 400, 7, 9, "icon_Carrier_longqiaoyunchuan", 33, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 1, 36, 179, 7, -1, 45, 18000, 3, 0, 90, 45, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 16, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(17, 34, 4, 400, 8, 9, "icon_Carrier_qixiangche", 35, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: true, inheritable: true, detachable: true, 300, 0, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 1, 36, 179, 7, -1, 50, 20000, 3, 0, 100, 50, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 17, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(18, 36, 4, 401, 0, 18, "icon_Carrier_luozi", 37, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 0, 12, -1, 7, -1, 35, 2000, 1, 0, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 18, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(19, 38, 4, 401, 1, 18, "icon_Carrier_luozi", 39, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 0, 12, -1, 7, -1, 40, 3000, 1, 0, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 19, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(20, 40, 4, 401, 2, 18, "icon_Carrier_luozi", 41, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 0, 12, -1, 7, -1, 45, 4000, 1, 0, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 20, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(21, 42, 4, 401, 3, 18, "icon_Carrier_guniu", 43, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 0, 12, -1, 7, -1, 50, 5000, 2, 0, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 21, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(22, 44, 4, 401, 4, 18, "icon_Carrier_guniu", 45, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 12, -1, 7, -1, 55, 6000, 2, 0, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 22, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(23, 46, 4, 401, 5, 18, "icon_Carrier_guniu", 47, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 0, 12, -1, 7, -1, 60, 7000, 2, 0, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 23, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(24, 48, 4, 401, 6, 18, "icon_Carrier_guanwaimingju", 49, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 12, -1, 7, -1, 65, 8000, 3, 0, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 24, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(25, 50, 4, 401, 7, 18, "icon_Carrier_qingniu", 51, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 12, -1, 7, -1, 70, 9000, 3, 0, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 25, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(26, 52, 4, 401, 8, 18, "icon_Carrier_hanxuebaoma", 53, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 0, 12, -1, 7, -1, 75, 10000, 3, 0, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 26, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(27, 54, 4, 402, 2, 27, "icon_Carrier_houzi", 55, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 900, 900, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 0, 12, -1, 7, -1, 15, 4000, 1, 25, 25, 5, 210, 148, new sbyte[7], isFlying: false, 0, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 27, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 20));
		_dataArray.Add(new CarrierItem(28, 56, 4, 402, 2, 27, "icon_Carrier_eying", 57, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 900, 900, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 0, 12, -1, 7, -1, 15, 4000, 1, 25, 25, 5, 211, 149, new sbyte[7], isFlying: true, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, null, 28, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 20));
		_dataArray.Add(new CarrierItem(29, 58, 4, 402, 2, 27, "icon_Carrier_yezhu", 59, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 900, 900, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 0, 12, -1, 7, -1, 15, 4000, 1, 25, 25, 5, 212, 150, new sbyte[7], isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, null, 29, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 20));
		_dataArray.Add(new CarrierItem(30, 60, 4, 402, 3, 27, "icon_Carrier_zongxiong", 61, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 2250, 2250, 1, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 0, 12, -1, 7, -1, 20, 5000, 2, 30, 30, 10, 213, 151, new sbyte[7], isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, null, 30, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 20));
		_dataArray.Add(new CarrierItem(31, 62, 4, 402, 3, 27, "icon_Carrier_yeniu", 63, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 2250, 2250, 1, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 0, 12, -1, 7, -1, 20, 5000, 2, 30, 30, 10, 214, 152, new sbyte[7], isFlying: false, 0, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 31, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 20));
		_dataArray.Add(new CarrierItem(32, 64, 4, 402, 3, 27, "icon_Carrier_jushe", 65, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 2250, 2250, 1, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 0, 12, -1, 7, -1, 20, 5000, 2, 30, 30, 10, 215, 153, new sbyte[7], isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, null, 32, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 20));
		_dataArray.Add(new CarrierItem(33, 66, 4, 402, 4, 27, "icon_Carrier_huabao", 67, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 4650, 4650, 2, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 12, -1, 7, -1, 25, 6000, 2, 35, 35, 15, 216, 154, new sbyte[7], isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, null, 33, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 20));
		_dataArray.Add(new CarrierItem(34, 68, 4, 402, 4, 27, "icon_Carrier_shizi", 69, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 4650, 4650, 2, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 12, -1, 7, -1, 25, 6000, 2, 35, 35, 15, 217, 155, new sbyte[7], isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, null, 34, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 20));
		_dataArray.Add(new CarrierItem(35, 70, 4, 402, 4, 27, "icon_Carrier_laohu", 71, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 4650, 4650, 2, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 0, 12, -1, 7, -1, 25, 6000, 2, 35, 35, 15, 218, 156, new sbyte[7], isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, null, 35, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 20));
		_dataArray.Add(new CarrierItem(36, 72, 4, 402, 5, 27, "icon_Carrier_linghou", 73, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 8400, 8400, 3, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 0, 12, -1, 7, -1, 30, 7000, 2, 40, 40, 20, 219, 157, new sbyte[7], isFlying: false, 0, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 36, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 40));
		_dataArray.Add(new CarrierItem(37, 74, 4, 402, 5, 27, "icon_Carrier_jinpeng", 75, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 8400, 8400, 3, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 0, 12, -1, 7, -1, 30, 7000, 2, 40, 40, 20, 220, 158, new sbyte[7], isFlying: true, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, null, 37, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 40));
		_dataArray.Add(new CarrierItem(38, 76, 4, 402, 5, 27, "icon_Carrier_xuanzhu", 77, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 8400, 8400, 3, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 0, 12, -1, 7, -1, 30, 7000, 2, 40, 40, 20, 221, 159, new sbyte[7], isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, null, 38, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 40));
		_dataArray.Add(new CarrierItem(39, 78, 4, 402, 6, 27, "icon_Carrier_baixiong", 79, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 13800, 13800, 4, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 12, -1, 7, -1, 35, 8000, 3, 45, 45, 25, 222, 160, new sbyte[7], isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, null, 39, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 40));
		_dataArray.Add(new CarrierItem(40, 80, 4, 402, 6, 27, "icon_Carrier_kuiniu", 81, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 13800, 13800, 4, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 12, -1, 7, -1, 35, 8000, 3, 45, 45, 25, 223, 161, new sbyte[7], isFlying: false, 0, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, null, 40, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 40));
		_dataArray.Add(new CarrierItem(41, 82, 4, 402, 6, 27, "icon_Carrier_bamang", 83, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 13800, 13800, 4, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 0, 12, -1, 7, -1, 35, 8000, 3, 45, 45, 25, 224, 162, new sbyte[7], isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, null, 41, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 40));
		_dataArray.Add(new CarrierItem(42, 84, 4, 402, 7, 27, "icon_Carrier_heibao", 85, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 21150, 21150, 5, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 12, -1, 7, -1, 40, 9000, 3, 50, 50, 30, 225, 163, new sbyte[7], isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, null, 42, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 40));
		_dataArray.Add(new CarrierItem(43, 86, 4, 402, 7, 27, "icon_Carrier_jinshi", 87, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 21150, 21150, 5, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 12, -1, 7, -1, 40, 9000, 3, 50, 50, 30, 226, 164, new sbyte[7], isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, null, 43, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 40));
		_dataArray.Add(new CarrierItem(44, 88, 4, 402, 7, 27, "icon_Carrier_baihu", 89, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 21150, 21150, 5, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 0, 12, -1, 7, -1, 40, 9000, 3, 50, 50, 30, 227, 165, new sbyte[7], isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, null, 44, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 40));
		_dataArray.Add(new CarrierItem(45, 90, 4, 401, 2, -1, "icon_Carrier_luozi", 41, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 300, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, 0, -1, -1, 7, -1, 40, 10000, 0, 0, 0, 0, -1, -1, new sbyte[7], isFlying: false, -1, null, null, null, 20, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 0));
		_dataArray.Add(new CarrierItem(46, 91, 4, 403, 5, -1, "icon_Carrier_baijiao", 92, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 696, 168, new sbyte[7] { 35, 0, 0, 0, 0, 0, 0 }, isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baijiao", 45, new PoisonsAndLevels(60, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0), 60));
		_dataArray.Add(new CarrierItem(47, 93, 4, 403, 5, -1, "icon_Carrier_baijiao", 94, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 697, 169, new sbyte[7] { 0, 35, 0, 0, 0, 0, 0 }, isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_heijiao", 45, new PoisonsAndLevels(0, 0, 0, 0, 60, 2, 0, 0, 0, 0, 0, 0), 60));
		_dataArray.Add(new CarrierItem(48, 95, 4, 403, 5, -1, "icon_Carrier_baijiao", 96, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 698, 170, new sbyte[7] { 0, 0, 35, 0, 0, 0, 0 }, isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_qingjiao", 45, new PoisonsAndLevels(0, 0, 60, 2, 0, 0, 0, 0, 0, 0, 0, 0), 60));
		_dataArray.Add(new CarrierItem(49, 97, 4, 403, 5, -1, "icon_Carrier_baijiao", 98, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 699, 171, new sbyte[7] { 0, 0, 0, 35, 0, 0, 0 }, isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_chijiao", 45, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 60, 2, 0, 0, 0, 0), 60));
		_dataArray.Add(new CarrierItem(50, 99, 4, 403, 5, -1, "icon_Carrier_baijiao", 100, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 700, 172, new sbyte[7] { 0, 0, 0, 0, 35, 0, 0 }, isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_huangjiao", 45, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 0, 0, 60, 2, 0, 0), 60));
		_dataArray.Add(new CarrierItem(51, 101, 4, 403, 5, -1, "icon_Carrier_baiheijiao", 102, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 701, 173, new sbyte[7] { 20, 20, 0, 0, 0, 0, 0 }, isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiheijiao", 45, new PoisonsAndLevels(80, 2, 0, 0, 80, 2, 0, 0, 0, 0, 0, 0), 80));
		_dataArray.Add(new CarrierItem(52, 103, 4, 403, 5, -1, "icon_Carrier_baiheijiao", 104, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 702, 174, new sbyte[7] { 20, 0, 20, 0, 0, 0, 0 }, isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiqingjiao", 45, new PoisonsAndLevels(80, 2, 80, 2, 0, 0, 0, 0, 0, 0, 0, 0), 80));
		_dataArray.Add(new CarrierItem(53, 105, 4, 403, 5, -1, "icon_Carrier_baiheijiao", 106, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 703, 175, new sbyte[7] { 20, 0, 0, 20, 0, 0, 0 }, isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baichijiao", 45, new PoisonsAndLevels(80, 2, 0, 0, 0, 0, 80, 2, 0, 0, 0, 0), 80));
		_dataArray.Add(new CarrierItem(54, 107, 4, 403, 5, -1, "icon_Carrier_baiheijiao", 108, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 704, 176, new sbyte[7] { 20, 0, 0, 0, 20, 0, 0 }, isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baihuangjiao", 45, new PoisonsAndLevels(80, 2, 0, 0, 0, 0, 0, 0, 80, 2, 0, 0), 80));
		_dataArray.Add(new CarrierItem(55, 109, 4, 403, 5, -1, "icon_Carrier_baiheijiao", 110, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 705, 177, new sbyte[7] { 0, 20, 20, 0, 0, 0, 0 }, isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_heiqingjiao", 45, new PoisonsAndLevels(0, 0, 80, 2, 80, 2, 0, 0, 0, 0, 0, 0), 80));
		_dataArray.Add(new CarrierItem(56, 111, 4, 403, 5, -1, "icon_Carrier_baiheijiao", 112, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 706, 178, new sbyte[7] { 0, 20, 0, 20, 0, 0, 0 }, isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_heichijiao", 45, new PoisonsAndLevels(0, 0, 0, 0, 80, 2, 80, 2, 0, 0, 0, 0), 80));
		_dataArray.Add(new CarrierItem(57, 113, 4, 403, 5, -1, "icon_Carrier_baiheijiao", 114, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 707, 179, new sbyte[7] { 0, 20, 0, 0, 20, 0, 0 }, isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_heihuangjiao", 45, new PoisonsAndLevels(0, 0, 0, 0, 80, 2, 0, 0, 80, 2, 0, 0), 80));
		_dataArray.Add(new CarrierItem(58, 115, 4, 403, 5, -1, "icon_Carrier_baiheijiao", 116, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 708, 180, new sbyte[7] { 0, 0, 20, 20, 0, 0, 0 }, isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_qingchijiao", 45, new PoisonsAndLevels(0, 0, 80, 2, 0, 0, 80, 2, 0, 0, 0, 0), 80));
		_dataArray.Add(new CarrierItem(59, 117, 4, 403, 5, -1, "icon_Carrier_baiheijiao", 118, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 709, 181, new sbyte[7] { 0, 0, 20, 0, 20, 0, 0 }, isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_qinghuangjiao", 45, new PoisonsAndLevels(0, 0, 80, 2, 0, 0, 0, 0, 80, 2, 0, 0), 80));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new CarrierItem(60, 119, 4, 403, 5, -1, "icon_Carrier_baiheijiao", 120, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 710, 182, new sbyte[7] { 0, 0, 0, 20, 20, 0, 0 }, isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_chihuangjiao", 45, new PoisonsAndLevels(0, 0, 0, 0, 0, 0, 80, 2, 80, 2, 0, 0), 80));
		_dataArray.Add(new CarrierItem(61, 121, 4, 403, 5, -1, "icon_Carrier_baiheiqingjiao", 122, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 711, 183, new sbyte[7] { 15, 15, 15, 0, 20, 0, 0 }, isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiheiqingjiao", 45, new PoisonsAndLevels(100, 2, 100, 2, 100, 2, 0, 0, 0, 0, 0, 0), 80));
		_dataArray.Add(new CarrierItem(62, 123, 4, 403, 5, -1, "icon_Carrier_baiheiqingjiao", 124, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 712, 184, new sbyte[7] { 15, 15, 0, 15, 0, 0, 0 }, isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiheichijiao", 45, new PoisonsAndLevels(100, 2, 0, 0, 100, 2, 100, 2, 0, 0, 0, 0), 80));
		_dataArray.Add(new CarrierItem(63, 125, 4, 403, 5, -1, "icon_Carrier_baiheiqingjiao", 126, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 713, 185, new sbyte[7] { 15, 15, 0, 0, 15, 0, 0 }, isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiheihuangjiao", 45, new PoisonsAndLevels(100, 2, 0, 0, 100, 2, 0, 0, 100, 2, 0, 0), 80));
		_dataArray.Add(new CarrierItem(64, 127, 4, 403, 5, -1, "icon_Carrier_baiheiqingjiao", 128, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 714, 186, new sbyte[7] { 15, 0, 15, 15, 0, 0, 0 }, isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiqingchijiao", 45, new PoisonsAndLevels(100, 2, 100, 2, 0, 0, 100, 2, 0, 0, 0, 0), 80));
		_dataArray.Add(new CarrierItem(65, 129, 4, 403, 5, -1, "icon_Carrier_baiheiqingjiao", 130, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 715, 187, new sbyte[7] { 15, 0, 15, 0, 15, 0, 0 }, isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiqinghuangjiao", 45, new PoisonsAndLevels(100, 2, 100, 2, 0, 0, 0, 0, 100, 2, 0, 0), 80));
		_dataArray.Add(new CarrierItem(66, 131, 4, 403, 5, -1, "icon_Carrier_baiheiqingjiao", 132, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 716, 188, new sbyte[7] { 15, 0, 0, 15, 15, 0, 0 }, isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baichihuangjiao", 45, new PoisonsAndLevels(100, 2, 0, 0, 0, 0, 100, 2, 100, 2, 0, 0), 80));
		_dataArray.Add(new CarrierItem(67, 133, 4, 403, 5, -1, "icon_Carrier_baiheiqingjiao", 134, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 717, 189, new sbyte[7] { 0, 15, 15, 15, 0, 0, 0 }, isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_heiqingchijiao", 45, new PoisonsAndLevels(0, 0, 100, 2, 100, 2, 100, 2, 0, 0, 0, 0), 80));
		_dataArray.Add(new CarrierItem(68, 135, 4, 403, 5, -1, "icon_Carrier_baiheiqingjiao", 136, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 718, 190, new sbyte[7] { 0, 15, 15, 0, 15, 0, 0 }, isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_heiqinghuangjiao", 45, new PoisonsAndLevels(0, 0, 100, 2, 100, 2, 0, 0, 100, 2, 0, 0), 80));
		_dataArray.Add(new CarrierItem(69, 137, 4, 403, 5, -1, "icon_Carrier_baiheiqingjiao", 138, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 719, 191, new sbyte[7] { 0, 15, 0, 15, 15, 0, 0 }, isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_heichihuangjiao", 45, new PoisonsAndLevels(0, 0, 0, 0, 100, 2, 100, 2, 100, 2, 0, 0), 80));
		_dataArray.Add(new CarrierItem(70, 139, 4, 403, 5, -1, "icon_Carrier_baiheiqingjiao", 140, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 720, 192, new sbyte[7] { 0, 0, 15, 15, 15, 0, 0 }, isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_qingchihuangjiao", 45, new PoisonsAndLevels(0, 0, 100, 2, 0, 0, 100, 2, 100, 2, 0, 0), 80));
		_dataArray.Add(new CarrierItem(71, 141, 4, 403, 5, -1, "icon_Carrier_baiheiqingchijiao", 142, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 721, 193, new sbyte[7] { 10, 10, 10, 10, 0, 0, 0 }, isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiheiqingchijiao", 45, new PoisonsAndLevels(100, 3, 100, 3, 100, 3, 100, 3, 0, 0, 0, 0), 80));
		_dataArray.Add(new CarrierItem(72, 143, 4, 403, 5, -1, "icon_Carrier_baiheiqingchijiao", 144, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 722, 194, new sbyte[7] { 10, 10, 10, 0, 10, 0, 0 }, isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiheiqinghuangjiao", 45, new PoisonsAndLevels(100, 3, 100, 3, 100, 3, 0, 0, 100, 3, 0, 0), 80));
		_dataArray.Add(new CarrierItem(73, 145, 4, 403, 5, -1, "icon_Carrier_baiheiqingchijiao", 146, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 723, 195, new sbyte[7] { 10, 10, 0, 10, 10, 0, 0 }, isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiheichihuangjiao", 45, new PoisonsAndLevels(100, 3, 0, 0, 100, 3, 100, 3, 100, 3, 0, 0), 80));
		_dataArray.Add(new CarrierItem(74, 147, 4, 403, 5, -1, "icon_Carrier_baiheiqingchijiao", 148, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 724, 196, new sbyte[7] { 10, 0, 10, 10, 10, 0, 0 }, isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiqingchihuangjiao", 45, new PoisonsAndLevels(100, 3, 100, 3, 0, 0, 100, 3, 100, 3, 0, 0), 80));
		_dataArray.Add(new CarrierItem(75, 149, 4, 403, 5, -1, "icon_Carrier_baiheiqingchijiao", 150, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 725, 197, new sbyte[7] { 0, 10, 10, 10, 10, 0, 0 }, isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_heiqingchihuangjiao", 45, new PoisonsAndLevels(0, 0, 100, 3, 100, 3, 100, 3, 100, 3, 0, 0), 80));
		_dataArray.Add(new CarrierItem(76, 151, 4, 403, 5, -1, "icon_Carrier_baiqingchihuangheijiao", 152, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 600, 0, 15, 20, 3, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 726, 198, new sbyte[7] { 10, 10, 10, 10, 10, 0, 0 }, isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_baiqingchihuangheijiao", 45, new PoisonsAndLevels(100, 3, 100, 3, 100, 3, 100, 3, 100, 3, 0, 0), 80));
		_dataArray.Add(new CarrierItem(77, 153, 4, 404, 8, -1, "icon_Carrier_qiuniu", 154, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 900, 0, 15, 20, 6, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 727, 199, new sbyte[7], isFlying: false, 0, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, "NpcFace_qiuniu", 46, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 100));
		_dataArray.Add(new CarrierItem(78, 155, 4, 404, 8, -1, "icon_Carrier_yazi", 156, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 900, 0, 15, 20, 6, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 728, 200, new sbyte[7], isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, "NpcFace_yazi", 47, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 100));
		_dataArray.Add(new CarrierItem(79, 157, 4, 404, 8, -1, "icon_Carrier_chaofeng", 158, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 900, 0, 15, 20, 6, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 729, 201, new sbyte[7], isFlying: false, 0, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, "NpcFace_chaofeng", 48, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 100));
		_dataArray.Add(new CarrierItem(80, 159, 4, 404, 8, -1, "icon_Carrier_pulao", 160, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 900, 0, 15, 20, 6, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 730, 202, new sbyte[7], isFlying: false, 0, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, "NpcFace_pulao", 49, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 100));
		_dataArray.Add(new CarrierItem(81, 161, 4, 404, 8, -1, "icon_Carrier_suanni", 162, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 900, 0, 15, 20, 6, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 731, 203, new sbyte[7], isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, "NpcFace_suanni", 50, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 100));
		_dataArray.Add(new CarrierItem(82, 163, 4, 404, 8, -1, "icon_Carrier_baxia", 164, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 900, 0, 15, 20, 6, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 732, 204, new sbyte[7], isFlying: false, 0, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, "NpcFace_baxia", 51, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 100));
		_dataArray.Add(new CarrierItem(83, 165, 4, 404, 8, -1, "icon_Carrier_bian", 166, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 900, 0, 15, 20, 6, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 733, 205, new sbyte[7], isFlying: false, 0, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, "NpcFace_bian", 52, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 100));
		_dataArray.Add(new CarrierItem(84, 167, 4, 404, 8, -1, "icon_Carrier_fuxi", 168, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 900, 0, 15, 20, 6, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 734, 206, new sbyte[7], isFlying: false, 0, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, "NpcFace_fuxi", 53, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 100));
		_dataArray.Add(new CarrierItem(85, 169, 4, 404, 8, -1, "icon_Carrier_chiwen", 170, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: true, 900, 0, 15, 20, 6, -1, 150, 8, allowRandomCreate: true, -1, isSpecial: true, 0, 36, -1, 7, -1, 0, 0, 0, 0, 0, 0, 735, 207, new sbyte[7], isFlying: false, 0, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, "NpcFace_chiwen", 54, new PoisonsAndLevels(default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short), default(short)), 100));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CarrierItem>(86);
		CreateItems0();
		CreateItems1();
	}
}
