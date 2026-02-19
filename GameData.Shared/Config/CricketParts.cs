using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class CricketParts : ConfigData<CricketPartsItem, short>
{
	public static class DefKey
	{
		public const short Trash = 0;

		public const short XiuHuaZhen = 1;

		public const short LiangTouQiang = 2;

		public const short ChuiLing = 3;

		public const short PaoMaHuang = 4;

		public const short YuChuTou = 5;

		public const short PiPaoXuanJia = 6;

		public const short FanShengMing = 7;

		public const short ZhuShaE = 8;

		public const short TouTuo = 9;

		public const short TieDanZi = 10;

		public const short ChiXu = 11;

		public const short YuWei = 12;

		public const short YouZhiDeng = 13;

		public const short ZhenSanSe = 14;

		public const short CaoSanDuan = 15;

		public const short ZhenZiHuang = 16;

		public const short MeiHuaChi = 17;

		public const short TianLanQing = 18;

		public const short SanDuanJin = 19;

		public const short SanTaiZi = 20;

		public const short BaBai = 21;

		public const short RealCyan = 22;

		public const short RealYellow = 23;

		public const short RealPurple = 24;

		public const short RealRed = 25;

		public const short RealBlack = 26;

		public const short RealWhite = 27;

		public const short SharpHead = 31;

		public const short RoundWings = 46;

		public const short SesameTeeth = 70;

		public const short Red = 127;

		public const short Black = 133;

		public const short White = 139;
	}

	public static class DefValue
	{
		public static CricketPartsItem Trash => Instance[(short)0];

		public static CricketPartsItem XiuHuaZhen => Instance[(short)1];

		public static CricketPartsItem LiangTouQiang => Instance[(short)2];

		public static CricketPartsItem ChuiLing => Instance[(short)3];

		public static CricketPartsItem PaoMaHuang => Instance[(short)4];

		public static CricketPartsItem YuChuTou => Instance[(short)5];

		public static CricketPartsItem PiPaoXuanJia => Instance[(short)6];

		public static CricketPartsItem FanShengMing => Instance[(short)7];

		public static CricketPartsItem ZhuShaE => Instance[(short)8];

		public static CricketPartsItem TouTuo => Instance[(short)9];

		public static CricketPartsItem TieDanZi => Instance[(short)10];

		public static CricketPartsItem ChiXu => Instance[(short)11];

		public static CricketPartsItem YuWei => Instance[(short)12];

		public static CricketPartsItem YouZhiDeng => Instance[(short)13];

		public static CricketPartsItem ZhenSanSe => Instance[(short)14];

		public static CricketPartsItem CaoSanDuan => Instance[(short)15];

		public static CricketPartsItem ZhenZiHuang => Instance[(short)16];

		public static CricketPartsItem MeiHuaChi => Instance[(short)17];

		public static CricketPartsItem TianLanQing => Instance[(short)18];

		public static CricketPartsItem SanDuanJin => Instance[(short)19];

		public static CricketPartsItem SanTaiZi => Instance[(short)20];

		public static CricketPartsItem BaBai => Instance[(short)21];

		public static CricketPartsItem RealCyan => Instance[(short)22];

		public static CricketPartsItem RealYellow => Instance[(short)23];

		public static CricketPartsItem RealPurple => Instance[(short)24];

		public static CricketPartsItem RealRed => Instance[(short)25];

		public static CricketPartsItem RealBlack => Instance[(short)26];

		public static CricketPartsItem RealWhite => Instance[(short)27];

		public static CricketPartsItem SharpHead => Instance[(short)31];

		public static CricketPartsItem RoundWings => Instance[(short)46];

		public static CricketPartsItem SesameTeeth => Instance[(short)70];

		public static CricketPartsItem Red => Instance[(short)127];

		public static CricketPartsItem Black => Instance[(short)133];

		public static CricketPartsItem White => Instance[(short)139];
	}

	public static CricketParts Instance = new CricketParts();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "Color", "Taste" };

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
		_dataArray.Add(new CricketPartsItem(0, new int[1], ECricketPartsType.Trash, 1, 0, 0, 1, 600, 2, 15, 150, 50, 30, 0, mustSuccessLoud: false, 0, 0, 8, null, 20, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0));
		_dataArray.Add(new CricketPartsItem(1, new int[1] { 2 }, ECricketPartsType.King, 3, 8, 0, 6, 10800, 18, 30750, 30750, 0, 75, 28, mustSuccessLoud: true, 1, 0, -70, null, 40, 50, 1, 1, 1, 85, 45, -10, 0, 0, 50, 100, 0));
		_dataArray.Add(new CricketPartsItem(2, new int[1] { 4 }, ECricketPartsType.King, 5, 8, 0, 10, 10800, 18, 30750, 30750, 30, 95, 28, mustSuccessLoud: true, 1, 0, -70, null, 110, 110, 7, 13, 7, 20, 8, 15, 0, 0, 100, 100, 0));
		_dataArray.Add(new CricketPartsItem(3, new int[1] { 6 }, ECricketPartsType.King, 7, 8, 0, 7, 10800, 18, 30750, 30750, -20, 150, 28, mustSuccessLoud: true, 1, 0, -70, null, 80, 70, 22, 5, 6, 40, 5, 0, 0, 0, 40, 100, 0));
		_dataArray.Add(new CricketPartsItem(4, new int[1] { 8 }, ECricketPartsType.King, 9, 8, 0, 12, 10800, 18, 30750, 30750, 50, 105, 28, mustSuccessLoud: true, 1, 0, -70, null, 140, 150, 4, 7, 4, 55, 5, -5, 55, 10, 120, 100, 0));
		_dataArray.Add(new CricketPartsItem(5, new int[1] { 10 }, ECricketPartsType.King, 11, 8, 0, 10, 10800, 18, 30750, 30750, 60, 120, 14, mustSuccessLoud: true, 2, 0, -80, null, 120, 120, 9, 12, 13, 25, 10, 10, 30, 8, 45, 100, 0));
		_dataArray.Add(new CricketPartsItem(6, new int[1] { 12 }, ECricketPartsType.King, 13, 8, 0, 15, 10800, 18, 30750, 30750, 90, 110, 14, mustSuccessLoud: true, 2, 0, -80, null, 280, 200, 6, 6, 6, 0, 0, 20, 80, 5, 70, 100, 0));
		_dataArray.Add(new CricketPartsItem(7, new int[1] { 14 }, ECricketPartsType.King, 15, 8, 0, 9, 10800, 18, 30750, 30750, 35, 100, 14, mustSuccessLoud: true, 2, 0, -80, null, 90, 90, 7, 11, 9, 65, 12, -15, 0, 0, 95, 100, 0));
		_dataArray.Add(new CricketPartsItem(8, new int[1] { 16 }, ECricketPartsType.King, 17, 8, 0, 10, 10800, 18, 30750, 30750, 55, 110, 14, mustSuccessLoud: true, 2, 0, -80, null, 140, 100, 8, 9, 13, 30, 18, 0, 30, 8, 40, 100, 0));
		_dataArray.Add(new CricketPartsItem(9, new int[1] { 18 }, ECricketPartsType.King, 19, 8, 0, 12, 10800, 18, 30750, 30750, 75, 0, 7, mustSuccessLoud: true, 3, 0, -90, null, 180, 160, 6, 16, 10, 20, 10, 10, 45, 18, 45, 100, 0));
		_dataArray.Add(new CricketPartsItem(10, new int[1] { 20 }, ECricketPartsType.King, 21, 8, 0, 10, 10800, 18, 30750, 30750, 80, 90, 7, mustSuccessLoud: true, 3, 0, -90, null, 170, 170, 5, 24, 8, 0, 0, 30, 35, 10, 65, 100, 0));
		_dataArray.Add(new CricketPartsItem(11, new int[1] { 22 }, ECricketPartsType.King, 23, 8, 0, 10, 10800, 18, 30750, 30750, 70, 130, 7, mustSuccessLoud: true, 3, 0, -90, null, 190, 130, 8, 10, 10, 35, 18, 5, 30, 8, 50, 100, 0));
		_dataArray.Add(new CricketPartsItem(12, new int[1] { 24 }, ECricketPartsType.King, 25, 8, 0, 8, 10800, 18, 30750, 30750, 45, 105, 7, mustSuccessLoud: true, 3, 0, -90, null, 90, 90, 4, 3, 24, 40, 24, 0, 0, 0, 40, 100, 0));
		_dataArray.Add(new CricketPartsItem(13, new int[1] { 26 }, ECricketPartsType.King, 27, 8, 0, 12, 10800, 18, 30750, 30750, 75, 135, 6, mustSuccessLoud: true, 3, 0, -100, null, 180, 120, 8, 15, 15, 25, 8, 10, 60, 8, 75, 100, 0));
		_dataArray.Add(new CricketPartsItem(14, new int[1] { 28 }, ECricketPartsType.King, 29, 8, 0, 12, 10800, 18, 30750, 30750, 85, 145, 6, mustSuccessLoud: true, 2, 0, -100, null, 220, 180, 13, 10, 12, 20, 10, 0, 45, 10, 40, 100, 0));
		_dataArray.Add(new CricketPartsItem(15, new int[1] { 30 }, ECricketPartsType.King, 31, 8, 0, 10, 10800, 18, 30750, 30750, 65, 130, 6, mustSuccessLoud: true, 2, 0, -100, null, 150, 200, 10, 9, 11, 45, 14, 0, 25, 15, 55, 100, 0));
		_dataArray.Add(new CricketPartsItem(16, new int[1] { 32 }, ECricketPartsType.King, 33, 8, 0, 12, 10800, 18, 30750, 30750, 60, 150, 6, mustSuccessLoud: true, 2, 0, -100, null, 160, 190, 11, 12, 10, 25, 20, 20, 20, 20, 45, 100, 0));
		_dataArray.Add(new CricketPartsItem(17, new int[1] { 34 }, ECricketPartsType.King, 35, 8, 0, 7, 10800, 18, 30750, 30750, 0, 160, 5, mustSuccessLoud: true, 2, 0, -110, null, 90, 140, 24, 1, 14, 70, 10, -20, 0, 0, 80, 100, 0));
		_dataArray.Add(new CricketPartsItem(18, new int[1] { 36 }, ECricketPartsType.King, 37, 8, 0, 15, 10800, 18, 30750, 30750, 80, 155, 4, mustSuccessLoud: true, 1, 0, -120, null, 290, 290, 12, 11, 13, 35, 15, 0, 35, 15, 45, 100, 0));
		_dataArray.Add(new CricketPartsItem(19, new int[1] { 38 }, ECricketPartsType.King, 39, 8, 0, 10, 10800, 18, 30750, 30750, 55, 145, 3, mustSuccessLoud: true, 1, 0, -130, null, 150, 150, 20, 20, 20, 50, 20, 0, 30, 10, 50, 100, 0));
		_dataArray.Add(new CricketPartsItem(20, new int[1] { 40 }, ECricketPartsType.King, 41, 8, 0, 9, 10800, 18, 30750, 30750, 70, 125, 2, mustSuccessLoud: true, 1, 0, -140, null, 80, 460, 8, 7, 9, 75, 1, 15, 75, 50, 85, 100, 0));
		_dataArray.Add(new CricketPartsItem(21, new int[1] { 42 }, ECricketPartsType.King, 43, 8, 0, 12, 10800, 18, 30750, 30750, 100, 110, 1, mustSuccessLoud: true, 1, 0, -150, null, 180, 800, 1, 1, 1, 95, 1, -55, 95, 25, 800, 100, 0));
		_dataArray.Add(new CricketPartsItem(22, new int[1] { 44 }, ECricketPartsType.RealColor, 45, 7, 0, 10, 9000, 16, 21150, 21150, 80, 125, 1, mustSuccessLoud: true, 0, 0, -60, null, 200, 150, 10, 11, 12, 15, 5, 15, 35, 10, 65, 80, 0));
		_dataArray.Add(new CricketPartsItem(23, new int[1] { 46 }, ECricketPartsType.RealColor, 47, 7, 0, 10, 9000, 16, 21150, 21150, 70, 120, 1, mustSuccessLoud: true, 0, 0, -60, null, 150, 200, 9, 13, 10, 25, 8, 10, 30, 12, 45, 80, 0));
		_dataArray.Add(new CricketPartsItem(24, new int[1] { 48 }, ECricketPartsType.RealColor, 49, 7, 0, 10, 9000, 16, 21150, 21150, 60, 115, 1, mustSuccessLoud: true, 0, 0, -60, null, 140, 140, 11, 8, 11, 35, 10, 5, 25, 15, 50, 80, 0));
		_dataArray.Add(new CricketPartsItem(25, new int[1] { 50 }, ECricketPartsType.RealColor, 51, 7, 0, 10, 9000, 16, 21150, 21150, 50, 110, 2, mustSuccessLoud: true, 0, 0, -60, null, 100, 100, 6, 6, 15, 50, 14, -5, 0, 0, 55, 80, 0));
		_dataArray.Add(new CricketPartsItem(26, new int[1] { 52 }, ECricketPartsType.RealColor, 53, 7, 0, 10, 9000, 16, 21150, 21150, 40, 105, 2, mustSuccessLoud: true, 0, 0, -60, null, 130, 90, 7, 15, 9, 20, 5, 5, 50, 8, 80, 80, 0));
		_dataArray.Add(new CricketPartsItem(27, new int[1] { 54 }, ECricketPartsType.RealColor, 55, 7, 0, 10, 9000, 16, 21150, 21150, 30, 100, 2, mustSuccessLoud: true, 0, 0, -60, null, 90, 130, 15, 4, 8, 45, 12, -10, 0, 0, 60, 80, 0));
		_dataArray.Add(new CricketPartsItem(28, new int[1] { 56 }, ECricketPartsType.Parts, 57, 0, 3, 4, 600, 2, 150, 150, 5, 5, 92, mustSuccessLoud: false, 0, 0, 8, null, 10, 10, 0, 1, 0, 0, 0, 0, 0, 0, 9, 5, 1));
		_dataArray.Add(new CricketPartsItem(29, new int[1] { 58 }, ECricketPartsType.Parts, 59, 0, 3, 4, 600, 2, 150, 150, 5, 5, 84, mustSuccessLoud: false, 0, 0, 8, null, 10, 10, 0, 1, 0, 0, 0, 0, 0, 0, 12, 5, 1));
		_dataArray.Add(new CricketPartsItem(30, new int[1] { 60 }, ECricketPartsType.Parts, 61, 0, 3, 4, 600, 2, 150, 150, 5, 5, 76, mustSuccessLoud: false, 0, 0, 8, null, 10, 10, 0, 1, 0, 0, 0, 0, 0, 0, 15, 5, 1));
		_dataArray.Add(new CricketPartsItem(31, new int[1] { 62 }, ECricketPartsType.Parts, 63, 1, 3, 4, 1200, 4, 300, 300, 10, 10, 68, mustSuccessLoud: false, 0, 0, 4, null, 20, 10, 0, 2, 0, 0, 0, 0, 0, 0, 18, 10, 1));
		_dataArray.Add(new CricketPartsItem(32, new int[1] { 64 }, ECricketPartsType.Parts, 65, 1, 3, 4, 1200, 4, 300, 300, 10, 10, 60, mustSuccessLoud: false, 0, 0, 4, null, 20, 10, 0, 2, 0, 0, 0, 0, 0, 0, 21, 10, 1));
		_dataArray.Add(new CricketPartsItem(33, new int[1] { 66 }, ECricketPartsType.Parts, 67, 1, 3, 4, 1200, 4, 300, 300, 10, 10, 52, mustSuccessLoud: false, 0, 0, 4, null, 20, 10, 0, 2, 0, 0, 0, 0, 0, 0, 24, 10, 1));
		_dataArray.Add(new CricketPartsItem(34, new int[1] { 68 }, ECricketPartsType.Parts, 69, 2, 3, 5, 1800, 6, 900, 900, 15, 15, 44, mustSuccessLoud: false, 0, 0, 4, null, 20, 20, 0, 3, 0, 0, 0, 0, 0, 0, 27, 15, 1));
		_dataArray.Add(new CricketPartsItem(35, new int[1] { 70 }, ECricketPartsType.Parts, 71, 2, 3, 5, 1800, 6, 900, 900, 15, 15, 36, mustSuccessLoud: false, 0, 0, 4, null, 20, 20, 0, 3, 0, 0, 0, 0, 0, 0, 30, 15, 1));
		_dataArray.Add(new CricketPartsItem(36, new int[1] { 72 }, ECricketPartsType.Parts, 73, 2, 3, 5, 1800, 6, 900, 900, 15, 15, 28, mustSuccessLoud: false, 0, 0, 4, null, 20, 20, 0, 3, 0, 0, 0, 0, 0, 0, 33, 15, 1));
		_dataArray.Add(new CricketPartsItem(37, new int[1] { 74 }, ECricketPartsType.Parts, 75, 3, 1, 5, 3000, 8, 2250, 2250, 20, 20, 20, mustSuccessLoud: false, 0, 0, 4, null, 30, 20, 0, 4, 0, 0, 0, 0, 0, 0, 36, 20, 1));
		_dataArray.Add(new CricketPartsItem(38, new int[1] { 76 }, ECricketPartsType.Parts, 77, 3, 1, 5, 3000, 8, 2250, 2250, 20, 20, 16, mustSuccessLoud: false, 0, 0, 4, null, 30, 20, 0, 4, 0, 0, 0, 0, 0, 0, 39, 20, 1));
		_dataArray.Add(new CricketPartsItem(39, new int[1] { 78 }, ECricketPartsType.Parts, 79, 3, 1, 5, 3000, 8, 2250, 2250, 20, 20, 12, mustSuccessLoud: false, 0, 0, 4, null, 30, 20, 0, 4, 0, 0, 0, 0, 0, 0, 42, 20, 1));
		_dataArray.Add(new CricketPartsItem(40, new int[1] { 80 }, ECricketPartsType.Parts, 81, 4, 1, 6, 4200, 10, 4650, 4650, 25, 25, 8, mustSuccessLoud: false, 0, 0, 8, null, 30, 30, 0, 5, 0, 0, 0, 0, 0, 0, 45, 25, 1));
		_dataArray.Add(new CricketPartsItem(41, new int[1] { 82 }, ECricketPartsType.Parts, 83, 4, 1, 6, 4200, 10, 4650, 4650, 25, 25, 6, mustSuccessLoud: false, 0, 0, 8, null, 30, 30, 0, 5, 0, 0, 0, 0, 0, 0, 48, 25, 1));
		_dataArray.Add(new CricketPartsItem(42, new int[1] { 84 }, ECricketPartsType.Parts, 85, 5, 1, 6, 5400, 12, 8400, 8400, 30, 30, 4, mustSuccessLoud: false, 0, 0, 12, null, 40, 40, 0, 6, 0, 0, 0, 0, 0, 0, 51, 30, 1));
		_dataArray.Add(new CricketPartsItem(43, new int[1] { 86 }, ECricketPartsType.Parts, 87, 5, 1, 6, 5400, 12, 8400, 8400, 30, 30, 3, mustSuccessLoud: false, 0, 0, 12, null, 40, 40, 0, 7, 0, 0, 0, 0, 0, 0, 54, 30, 1));
		_dataArray.Add(new CricketPartsItem(44, new int[1] { 88 }, ECricketPartsType.Parts, 89, 6, 1, 7, 7200, 14, 13800, 13800, 35, 35, 2, mustSuccessLoud: false, 0, 0, 16, null, 50, 50, 0, 8, 0, 0, 0, 0, 0, 0, 57, 35, 1));
		_dataArray.Add(new CricketPartsItem(45, new int[1] { 90 }, ECricketPartsType.Parts, 91, 6, 1, 7, 7200, 14, 13800, 13800, 35, 35, 1, mustSuccessLoud: false, 0, 0, 16, null, 50, 50, 0, 9, 0, 0, 0, 0, 0, 0, 60, 35, 1));
		_dataArray.Add(new CricketPartsItem(46, new int[1] { 92 }, ECricketPartsType.Parts, 93, 0, 3, 4, 600, 2, 150, 150, 5, 5, 92, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 1, 0, 0, 0, 0, 0, 5, 1, 0, 5, 1));
		_dataArray.Add(new CricketPartsItem(47, new int[1] { 94 }, ECricketPartsType.Parts, 95, 0, 3, 4, 600, 2, 150, 150, 5, 5, 84, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 1, 0, 0, 0, 0, 0, 7, 1, 0, 5, 1));
		_dataArray.Add(new CricketPartsItem(48, new int[1] { 96 }, ECricketPartsType.Parts, 97, 0, 3, 4, 600, 2, 150, 150, 5, 5, 76, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 1, 0, 0, 0, 0, 0, 9, 1, 0, 5, 1));
		_dataArray.Add(new CricketPartsItem(49, new int[1] { 98 }, ECricketPartsType.Parts, 99, 1, 3, 4, 1200, 4, 300, 300, 10, 10, 68, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 2, 0, 0, 0, 0, 0, 11, 2, 0, 10, 1));
		_dataArray.Add(new CricketPartsItem(50, new int[1] { 100 }, ECricketPartsType.Parts, 101, 1, 3, 4, 1200, 4, 300, 300, 10, 10, 60, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 2, 0, 0, 0, 0, 0, 13, 2, 0, 10, 1));
		_dataArray.Add(new CricketPartsItem(51, new int[1] { 102 }, ECricketPartsType.Parts, 103, 1, 3, 4, 1200, 4, 300, 300, 40, -10, 52, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 2, 0, 0, 0, 0, 0, 15, 2, 0, 10, 1));
		_dataArray.Add(new CricketPartsItem(52, new int[1] { 104 }, ECricketPartsType.Parts, 105, 2, 1, 5, 1800, 6, 900, 900, 15, 15, 44, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 3, 0, 0, 0, 0, 0, 17, 3, 0, 15, 1));
		_dataArray.Add(new CricketPartsItem(53, new int[1] { 106 }, ECricketPartsType.Parts, 107, 2, 1, 5, 1800, 6, 900, 900, 50, -20, 36, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 3, 0, 0, 0, 0, 0, 19, 3, 0, 15, 1));
		_dataArray.Add(new CricketPartsItem(54, new int[1] { 108 }, ECricketPartsType.Parts, 109, 2, 1, 5, 1800, 6, 900, 900, 15, 15, 28, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 3, 0, 0, 0, 0, 0, 21, 3, 0, 15, 1));
		_dataArray.Add(new CricketPartsItem(55, new int[1] { 110 }, ECricketPartsType.Parts, 111, 3, 3, 5, 3000, 8, 2250, 2250, 20, 20, 20, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 4, 0, 0, 0, 0, 0, 23, 4, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(56, new int[1] { 112 }, ECricketPartsType.Parts, 113, 3, 3, 5, 3000, 8, 2250, 2250, 20, 20, 16, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 4, 0, 0, 0, 0, 0, 25, 4, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(57, new int[1] { 114 }, ECricketPartsType.Parts, 115, 3, 3, 5, 3000, 8, 2250, 2250, 20, 20, 12, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 4, 0, 0, 0, 0, 0, 27, 4, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(58, new int[1] { 116 }, ECricketPartsType.Parts, 117, 4, 1, 6, 4200, 10, 4650, 4650, -20, 35, 8, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 5, 0, 0, 0, 0, 0, 30, 5, 0, 25, 1));
		_dataArray.Add(new CricketPartsItem(59, new int[1] { 118 }, ECricketPartsType.Parts, 119, 4, 1, 6, 4200, 10, 4650, 4650, 25, 25, 6, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 5, 0, 0, 0, 0, 0, 32, 5, 0, 25, 1));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new CricketPartsItem(60, new int[1] { 120 }, ECricketPartsType.Parts, 121, 5, 1, 6, 5400, 12, 8400, 8400, 30, 30, 4, mustSuccessLoud: false, 0, 0, 12, null, 0, 0, 6, 0, 0, 0, 0, 0, 34, 6, 0, 30, 1));
		_dataArray.Add(new CricketPartsItem(61, new int[1] { 122 }, ECricketPartsType.Parts, 123, 5, 1, 6, 5400, 12, 8400, 8400, 30, 30, 3, mustSuccessLoud: false, 0, 0, 12, null, 0, 0, 7, 0, 0, 0, 0, 0, 36, 6, 0, 30, 1));
		_dataArray.Add(new CricketPartsItem(62, new int[1] { 124 }, ECricketPartsType.Parts, 125, 6, 1, 7, 7200, 14, 13800, 13800, 35, 35, 2, mustSuccessLoud: false, 0, 0, 16, null, 0, 0, 8, 0, 0, 0, 0, 0, 38, 7, 0, 35, 1));
		_dataArray.Add(new CricketPartsItem(63, new int[1] { 126 }, ECricketPartsType.Parts, 127, 6, 1, 7, 7200, 14, 13800, 13800, 35, 35, 1, mustSuccessLoud: false, 0, 0, 16, null, 0, 0, 9, 0, 0, 0, 0, 0, 40, 7, 0, 35, 1));
		_dataArray.Add(new CricketPartsItem(64, new int[1] { 128 }, ECricketPartsType.Parts, 129, 0, 3, 4, 600, 2, 150, 150, 5, 5, 92, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 0, 0, 1, 5, 1, -3, 0, 0, 0, 5, 1));
		_dataArray.Add(new CricketPartsItem(65, new int[1] { 130 }, ECricketPartsType.Parts, 131, 0, 3, 4, 600, 2, 150, 150, 5, 5, 84, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 0, 0, 1, 7, 1, -4, 0, 0, 0, 5, 1));
		_dataArray.Add(new CricketPartsItem(66, new int[1] { 132 }, ECricketPartsType.Parts, 133, 0, 3, 4, 600, 2, 150, 150, 5, 5, 76, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 0, 0, 1, 9, 1, -5, 0, 0, 0, 5, 1));
		_dataArray.Add(new CricketPartsItem(67, new int[1] { 134 }, ECricketPartsType.Parts, 135, 1, 3, 4, 1200, 4, 300, 300, 10, 10, 68, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 2, 11, 2, -6, 0, 0, 0, 10, 1));
		_dataArray.Add(new CricketPartsItem(68, new int[1] { 136 }, ECricketPartsType.Parts, 137, 1, 3, 4, 1200, 4, 300, 300, 10, 10, 60, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 2, 13, 2, -7, 0, 0, 0, 10, 1));
		_dataArray.Add(new CricketPartsItem(69, new int[1] { 138 }, ECricketPartsType.Parts, 139, 1, 3, 4, 1200, 4, 300, 300, 10, 10, 52, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 2, 15, 2, -8, 0, 0, 0, 10, 1));
		_dataArray.Add(new CricketPartsItem(70, new int[1] { 140 }, ECricketPartsType.Parts, 141, 2, 1, 5, 1800, 6, 900, 900, 15, 15, 44, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 3, 17, 3, -9, 0, 0, 0, 15, 1));
		_dataArray.Add(new CricketPartsItem(71, new int[1] { 142 }, ECricketPartsType.Parts, 143, 2, 1, 5, 1800, 6, 900, 900, 15, 15, 36, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 3, 19, 3, -10, 0, 0, 0, 15, 1));
		_dataArray.Add(new CricketPartsItem(72, new int[1] { 144 }, ECricketPartsType.Parts, 145, 2, 3, 5, 1800, 6, 900, 900, 15, 15, 28, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 3, 21, 3, -11, 0, 0, 0, 15, 1));
		_dataArray.Add(new CricketPartsItem(73, new int[1] { 146 }, ECricketPartsType.Parts, 147, 3, 1, 5, 3000, 8, 2250, 2250, 20, 20, 20, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 4, 23, 4, -12, 0, 0, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(74, new int[1] { 148 }, ECricketPartsType.Parts, 149, 3, 1, 5, 3000, 8, 2250, 2250, 20, 20, 16, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 4, 25, 4, -13, 0, 0, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(75, new int[1] { 150 }, ECricketPartsType.Parts, 151, 3, 1, 5, 3000, 8, 2250, 2250, 20, 20, 12, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 4, 27, 4, -14, 0, 0, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(76, new int[1] { 152 }, ECricketPartsType.Parts, 153, 4, 1, 6, 4200, 10, 4650, 4650, 25, 25, 8, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 0, 0, 5, 30, 5, -15, 0, 0, 0, 25, 1));
		_dataArray.Add(new CricketPartsItem(77, new int[1] { 154 }, ECricketPartsType.Parts, 155, 4, 1, 6, 4200, 10, 4650, 4650, 25, 25, 6, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 0, 0, 5, 32, 5, -16, 0, 0, 0, 25, 1));
		_dataArray.Add(new CricketPartsItem(78, new int[1] { 156 }, ECricketPartsType.Parts, 157, 5, 1, 6, 5400, 12, 8400, 8400, 30, 30, 4, mustSuccessLoud: false, 0, 0, 12, null, 0, 0, 0, 0, 6, 34, 6, -17, 0, 0, 0, 30, 1));
		_dataArray.Add(new CricketPartsItem(79, new int[1] { 158 }, ECricketPartsType.Parts, 159, 5, 1, 6, 5400, 12, 8400, 8400, 30, 30, 3, mustSuccessLoud: false, 0, 0, 12, null, 0, 0, 0, 0, 7, 36, 6, -18, 0, 0, 0, 30, 1));
		_dataArray.Add(new CricketPartsItem(80, new int[1] { 160 }, ECricketPartsType.Parts, 161, 6, 1, 7, 7200, 14, 13800, 13800, 35, 35, 2, mustSuccessLoud: false, 0, 0, 16, null, 0, 0, 0, 0, 8, 38, 7, -19, 0, 0, 0, 35, 1));
		_dataArray.Add(new CricketPartsItem(81, new int[1] { 162 }, ECricketPartsType.Parts, 163, 6, 1, 7, 7200, 14, 13800, 13800, 35, 35, 1, mustSuccessLoud: false, 0, 0, 16, null, 0, 0, 0, 0, 9, 40, 7, -20, 0, 0, 0, 35, 1));
		_dataArray.Add(new CricketPartsItem(82, new int[1] { 164 }, ECricketPartsType.Parts, 165, 3, 3, 4, 3000, 8, 2250, 2250, 20, 20, 8, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 0, 15, 3, -5, 15, 3, 25, 20, 1));
		_dataArray.Add(new CricketPartsItem(83, new int[1] { 166 }, ECricketPartsType.Parts, 167, 4, 1, 5, 4200, 10, 4650, 4650, 25, 25, 4, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 0, 0, 0, 20, 4, -5, 20, 4, 30, 25, 1));
		_dataArray.Add(new CricketPartsItem(84, new int[1] { 168 }, ECricketPartsType.Parts, 169, 5, 1, 5, 5400, 12, 8400, 8400, 30, 30, 2, mustSuccessLoud: false, 0, 0, 12, null, 0, 0, 0, 0, 0, 25, 5, -5, 25, 5, 35, 30, 1));
		_dataArray.Add(new CricketPartsItem(85, new int[1] { 170 }, ECricketPartsType.Parts, 171, 3, 3, 5, 3000, 8, 2250, 2250, 20, 20, 8, mustSuccessLoud: false, 0, 0, 4, null, 10, 10, 2, 2, 2, 0, 0, 0, 0, 0, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(86, new int[1] { 172 }, ECricketPartsType.Parts, 173, 4, 1, 6, 4200, 10, 4650, 4650, 25, 25, 4, mustSuccessLoud: false, 0, 0, 8, null, 20, 20, 3, 3, 3, 0, 0, 0, 0, 0, 0, 25, 1));
		_dataArray.Add(new CricketPartsItem(87, new int[1] { 174 }, ECricketPartsType.Parts, 175, 5, 1, 6, 5400, 12, 8400, 8400, 30, 30, 2, mustSuccessLoud: false, 0, 0, 12, null, 30, 30, 4, 4, 4, 0, 0, 0, 0, 0, 0, 30, 1));
		_dataArray.Add(new CricketPartsItem(88, new int[1] { 176 }, ECricketPartsType.Parts, 177, 3, 3, 4, 3000, 8, 2250, 2250, 30, 20, 8, mustSuccessLoud: false, 0, 0, 4, null, 50, 0, 0, 0, 0, 0, 0, 0, 30, 4, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(89, new int[1] { 178 }, ECricketPartsType.Parts, 179, 4, 1, 5, 4200, 10, 4650, 4650, 35, 25, 4, mustSuccessLoud: false, 0, 0, 8, null, 60, 0, 0, 0, 0, 0, 0, 0, 35, 5, 0, 25, 1));
		_dataArray.Add(new CricketPartsItem(90, new int[1] { 180 }, ECricketPartsType.Parts, 181, 5, 1, 5, 5400, 12, 8400, 8400, 40, 30, 2, mustSuccessLoud: false, 0, 0, 12, null, 70, 0, 0, 0, 0, 0, 0, 0, 40, 6, 0, 30, 1));
		_dataArray.Add(new CricketPartsItem(91, new int[1] { 182 }, ECricketPartsType.Parts, 183, 3, 1, 4, 3000, 8, 2250, 2250, 20, 20, 8, mustSuccessLoud: false, 0, 0, 4, null, 0, 50, 0, 0, 0, 30, 4, -20, 0, 0, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(92, new int[1] { 184 }, ECricketPartsType.Parts, 185, 4, 1, 5, 4200, 10, 4650, 4650, 25, 25, 4, mustSuccessLoud: false, 0, 0, 8, null, 0, 60, 0, 0, 0, 35, 5, -20, 0, 0, 0, 25, 1));
		_dataArray.Add(new CricketPartsItem(93, new int[1] { 186 }, ECricketPartsType.Parts, 187, 5, 1, 5, 5400, 12, 8400, 8400, 30, 30, 2, mustSuccessLoud: false, 0, 0, 12, null, 0, 70, 0, 0, 0, 40, 6, -20, 0, 0, 0, 30, 1));
		_dataArray.Add(new CricketPartsItem(94, new int[1] { 188 }, ECricketPartsType.Parts, 189, 3, 3, 6, 3000, 8, 2250, 2250, 25, 20, 8, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 0, 0, 0, 0, 35, 7, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(95, new int[1] { 190 }, ECricketPartsType.Parts, 191, 4, 3, 7, 4200, 10, 4650, 4650, 30, 25, 4, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 0, 0, 0, 0, 0, 0, 40, 8, 0, 25, 1));
		_dataArray.Add(new CricketPartsItem(96, new int[1] { 192 }, ECricketPartsType.Parts, 193, 5, 3, 7, 5400, 12, 8400, 8400, 35, 30, 2, mustSuccessLoud: false, 0, 0, 12, null, 0, 0, 0, 0, 0, 0, 0, 0, 45, 9, 0, 30, 1));
		_dataArray.Add(new CricketPartsItem(97, new int[1] { 194 }, ECricketPartsType.Parts, 195, 3, 1, 5, 3000, 8, 2250, 2250, 20, 20, 8, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 50, 20, 1));
		_dataArray.Add(new CricketPartsItem(98, new int[1] { 196 }, ECricketPartsType.Parts, 197, 4, 1, 6, 4200, 10, 4650, 4650, 25, 25, 4, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 55, 25, 1));
		_dataArray.Add(new CricketPartsItem(99, new int[1] { 198 }, ECricketPartsType.Parts, 199, 5, 1, 6, 5400, 12, 8400, 8400, 30, 30, 2, mustSuccessLoud: false, 0, 0, 12, null, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0, 60, 30, 1));
		_dataArray.Add(new CricketPartsItem(100, new int[1] { 200 }, ECricketPartsType.Parts, 201, 3, 3, 4, 3000, 8, 2250, 2250, 20, 20, 8, mustSuccessLoud: false, 0, 0, 4, null, 0, 0, 0, 0, 0, 35, 7, -25, 0, 0, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(101, new int[1] { 202 }, ECricketPartsType.Parts, 203, 4, 1, 5, 4200, 10, 4650, 4650, 25, 25, 4, mustSuccessLoud: false, 0, 0, 8, null, 0, 0, 0, 0, 0, 40, 8, -25, 0, 0, 0, 25, 1));
		_dataArray.Add(new CricketPartsItem(102, new int[1] { 204 }, ECricketPartsType.Parts, 205, 5, 1, 5, 5400, 12, 8400, 8400, 30, 30, 2, mustSuccessLoud: false, 0, 0, 12, null, 0, 0, 0, 0, 0, 45, 9, -25, 0, 0, 0, 30, 1));
		_dataArray.Add(new CricketPartsItem(103, new int[1] { 206 }, ECricketPartsType.Parts, 207, 3, 3, 6, 3000, 8, 2250, 2250, 30, 20, 8, mustSuccessLoud: false, 0, 0, 4, null, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 1));
		_dataArray.Add(new CricketPartsItem(104, new int[1] { 208 }, ECricketPartsType.Parts, 209, 4, 3, 7, 4200, 10, 4650, 4650, 35, 25, 4, mustSuccessLoud: false, 0, 0, 8, null, 70, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 1));
		_dataArray.Add(new CricketPartsItem(105, new int[1] { 210 }, ECricketPartsType.Parts, 211, 5, 1, 7, 5400, 12, 8400, 8400, 50, -20, 2, mustSuccessLoud: false, 0, 0, 12, null, 90, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 1));
		_dataArray.Add(new CricketPartsItem(106, new int[1] { 212 }, ECricketPartsType.Parts, 213, 6, 1, 8, 7200, 14, 13800, 13800, 25, 45, 1, mustSuccessLoud: false, 0, 0, 16, null, 0, 0, 12, 0, 0, 0, 0, 0, 35, 5, 0, 35, 1));
		_dataArray.Add(new CricketPartsItem(107, new int[1] { 214 }, ECricketPartsType.Parts, 215, 6, 1, 8, 7200, 14, 13800, 13800, 35, 35, 1, mustSuccessLoud: false, 0, 0, 16, null, 60, 60, 0, 12, 0, 0, 0, 0, 0, 0, 50, 35, 1));
		_dataArray.Add(new CricketPartsItem(108, new int[1] { 216 }, ECricketPartsType.Parts, 217, 6, 1, 8, 7200, 14, 13800, 13800, 35, 35, 1, mustSuccessLoud: false, 0, 0, 16, null, 0, 0, 0, 0, 12, 35, 5, -15, 0, 0, 0, 35, 1));
		_dataArray.Add(new CricketPartsItem(109, new int[2] { 218, 219 }, ECricketPartsType.Cyan, 220, 5, 2, 4, 5400, 12, 8400, 8400, 50, 50, 24, mustSuccessLoud: false, 0, 22, 16, "84b8af", 150, 80, 2, 3, 4, 5, 5, 10, 25, 5, 15, 30, 1));
		_dataArray.Add(new CricketPartsItem(110, new int[2] { 221, 221 }, ECricketPartsType.Cyan, 222, 5, 2, 4, 5400, 12, 8400, 8400, 50, 50, 16, mustSuccessLoud: false, 0, 24, 16, "a4d0bc", 130, 100, 3, 4, 5, 8, 4, 10, 25, 4, 20, 30, 1));
		_dataArray.Add(new CricketPartsItem(111, new int[2] { 223, 223 }, ECricketPartsType.Cyan, 224, 5, 2, 4, 5400, 12, 8400, 8400, 50, 50, 8, mustSuccessLoud: false, 0, 26, 16, "b3dad8", 140, 90, 5, 4, 5, 7, 4, 10, 20, 5, 20, 30, 1));
		_dataArray.Add(new CricketPartsItem(112, new int[2] { 225, 226 }, ECricketPartsType.Cyan, 227, 5, 2, 5, 5400, 12, 8400, 8400, 50, 50, 4, mustSuccessLoud: false, 0, 28, 16, "7da598", 150, 80, 5, 6, 6, 6, 5, 10, 25, 7, 20, 30, 1));
		_dataArray.Add(new CricketPartsItem(113, new int[2] { 228, 228 }, ECricketPartsType.Cyan, 229, 5, 2, 5, 5400, 12, 8400, 8400, 50, 50, 2, mustSuccessLoud: false, 0, 30, 16, "647f9e", 160, 100, 5, 6, 6, 7, 6, 10, 25, 8, 15, 30, 1));
		_dataArray.Add(new CricketPartsItem(114, new int[2] { 230, 231 }, ECricketPartsType.Cyan, 232, 5, 2, 5, 5400, 12, 8400, 8400, 50, 50, 1, mustSuccessLoud: false, 0, 32, 16, "5a8aa7", 150, 120, 6, 6, 7, 9, 6, 10, 20, 7, 20, 30, 1));
		_dataArray.Add(new CricketPartsItem(115, new int[2] { 233, 234 }, ECricketPartsType.Yellow, 235, 4, 2, 4, 4200, 10, 4650, 4650, 40, 40, 24, mustSuccessLoud: false, 0, 20, 12, "c8a353", 100, 160, 1, 4, 2, 15, 5, 0, 15, 5, 30, 25, 1));
		_dataArray.Add(new CricketPartsItem(116, new int[2] { 236, 236 }, ECricketPartsType.Yellow, 237, 4, 2, 4, 4200, 10, 4650, 4650, 40, 40, 16, mustSuccessLoud: false, 0, 22, 12, "ceb882", 90, 120, 3, 7, 3, 18, 7, 0, 10, 6, 35, 25, 1));
		_dataArray.Add(new CricketPartsItem(117, new int[2] { 238, 238 }, ECricketPartsType.Yellow, 239, 4, 2, 4, 4200, 10, 4650, 4650, 40, 40, 8, mustSuccessLoud: false, 0, 24, 12, "966e3a", 100, 140, 2, 6, 5, 10, 6, 0, 15, 8, 35, 25, 1));
		_dataArray.Add(new CricketPartsItem(118, new int[2] { 240, 240 }, ECricketPartsType.Yellow, 241, 4, 2, 5, 4200, 10, 4650, 4650, 40, 40, 4, mustSuccessLoud: false, 0, 26, 12, "e5d8a6", 90, 160, 4, 7, 5, 15, 5, 0, 10, 6, 30, 25, 1));
		_dataArray.Add(new CricketPartsItem(119, new int[2] { 242, 242 }, ECricketPartsType.Yellow, 243, 4, 2, 5, 4200, 10, 4650, 4650, 40, 40, 2, mustSuccessLoud: false, 0, 28, 12, "57431f", 110, 160, 3, 8, 6, 15, 6, 0, 18, 7, 30, 25, 1));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new CricketPartsItem(120, new int[2] { 244, 244 }, ECricketPartsType.Yellow, 245, 4, 2, 5, 4200, 10, 4650, 4650, 40, 40, 1, mustSuccessLoud: false, 0, 30, 12, "bfae65", 120, 170, 4, 9, 5, 18, 7, 0, 15, 7, 35, 25, 1));
		_dataArray.Add(new CricketPartsItem(121, new int[2] { 246, 247 }, ECricketPartsType.Purple, 248, 3, 2, 4, 3000, 8, 2250, 2250, 30, 30, 24, mustSuccessLoud: false, 0, 16, 8, "7d638b", 70, 70, 4, 2, 3, 25, 6, -10, 10, 8, 35, 20, 1));
		_dataArray.Add(new CricketPartsItem(122, new int[2] { 249, 249 }, ECricketPartsType.Purple, 250, 3, 2, 4, 3000, 8, 2250, 2250, 30, 30, 16, mustSuccessLoud: false, 0, 18, 8, "948299", 60, 70, 4, 2, 4, 30, 7, -10, 10, 7, 35, 20, 1));
		_dataArray.Add(new CricketPartsItem(123, new int[2] { 251, 252 }, ECricketPartsType.Purple, 253, 3, 2, 4, 3000, 8, 2250, 2250, 30, 30, 8, mustSuccessLoud: false, 0, 20, 8, "685177", 80, 80, 5, 2, 5, 20, 5, -10, 15, 10, 30, 20, 1));
		_dataArray.Add(new CricketPartsItem(124, new int[2] { 254, 254 }, ECricketPartsType.Purple, 255, 3, 2, 5, 3000, 8, 2250, 2250, 30, 30, 4, mustSuccessLoud: false, 0, 22, 8, "4d2c52", 90, 80, 5, 3, 5, 25, 6, -10, 12, 12, 30, 20, 1));
		_dataArray.Add(new CricketPartsItem(125, new int[2] { 256, 257 }, ECricketPartsType.Purple, 258, 3, 2, 5, 3000, 8, 2250, 2250, 30, 30, 2, mustSuccessLoud: false, 0, 24, 8, "833557", 70, 70, 6, 4, 6, 30, 8, -10, 15, 10, 35, 20, 1));
		_dataArray.Add(new CricketPartsItem(126, new int[2] { 259, 259 }, ECricketPartsType.Purple, 260, 3, 2, 5, 3000, 8, 2250, 2250, 30, 30, 1, mustSuccessLoud: false, 0, 26, 8, "d294d3", 80, 80, 7, 5, 6, 30, 7, -10, 12, 12, 35, 20, 1));
		_dataArray.Add(new CricketPartsItem(127, new int[2] { 261, 262 }, ECricketPartsType.Red, 263, 2, 2, 4, 1800, 6, 900, 900, 20, 20, 24, mustSuccessLoud: false, 0, 12, 4, "9d4143", 50, 50, 2, 2, 5, 35, 7, -10, 0, 0, 30, 15, 1));
		_dataArray.Add(new CricketPartsItem(128, new int[2] { 264, 264 }, ECricketPartsType.Red, 265, 2, 2, 4, 1800, 6, 900, 900, 20, 20, 16, mustSuccessLoud: false, 0, 14, 4, "b87681", 50, 60, 3, 3, 5, 40, 7, -10, 0, 0, 25, 15, 1));
		_dataArray.Add(new CricketPartsItem(129, new int[2] { 266, 266 }, ECricketPartsType.Red, 267, 2, 2, 4, 1800, 6, 900, 900, 20, 20, 8, mustSuccessLoud: false, 0, 16, 4, "874a58", 60, 60, 2, 4, 6, 40, 9, -10, 0, 0, 30, 15, 1));
		_dataArray.Add(new CricketPartsItem(130, new int[2] { 268, 268 }, ECricketPartsType.Red, 269, 2, 2, 5, 1800, 6, 900, 900, 20, 20, 4, mustSuccessLoud: false, 0, 18, 4, "b85f4f", 70, 60, 4, 5, 7, 35, 7, -10, 0, 0, 25, 15, 1));
		_dataArray.Add(new CricketPartsItem(131, new int[2] { 270, 271 }, ECricketPartsType.Red, 272, 2, 2, 5, 1800, 6, 900, 900, 20, 20, 2, mustSuccessLoud: false, 0, 20, 4, "ac544d", 60, 50, 5, 4, 8, 35, 9, -10, 0, 0, 30, 15, 1));
		_dataArray.Add(new CricketPartsItem(132, new int[2] { 273, 274 }, ECricketPartsType.Red, 275, 2, 2, 5, 1800, 6, 900, 900, 20, 20, 1, mustSuccessLoud: false, 0, 22, 4, "c95955", 60, 70, 4, 4, 9, 45, 9, -10, 0, 0, 30, 15, 1));
		_dataArray.Add(new CricketPartsItem(133, new int[2] { 276, 277 }, ECricketPartsType.Black, 278, 1, 2, 4, 1200, 4, 300, 300, 10, 10, 24, mustSuccessLoud: false, 0, 8, 4, "333333", 70, 60, 1, 4, 1, 10, 3, 5, 20, 5, 55, 10, 1));
		_dataArray.Add(new CricketPartsItem(134, new int[2] { 279, 279 }, ECricketPartsType.Black, 280, 1, 2, 4, 1200, 4, 300, 300, 10, 10, 16, mustSuccessLoud: false, 0, 10, 4, "6b6b6b", 60, 40, 2, 5, 2, 8, 3, 5, 25, 6, 60, 10, 1));
		_dataArray.Add(new CricketPartsItem(135, new int[2] { 281, 281 }, ECricketPartsType.Black, 282, 1, 2, 4, 1200, 4, 300, 300, 10, 10, 8, mustSuccessLoud: false, 0, 12, 4, "4d4b42", 80, 60, 1, 6, 2, 10, 4, 5, 25, 7, 55, 10, 1));
		_dataArray.Add(new CricketPartsItem(136, new int[2] { 283, 283 }, ECricketPartsType.Black, 284, 1, 2, 5, 1200, 4, 300, 300, 10, 10, 4, mustSuccessLoud: false, 0, 14, 4, "4a4b48", 90, 50, 2, 7, 4, 8, 4, 5, 20, 6, 55, 10, 1));
		_dataArray.Add(new CricketPartsItem(137, new int[2] { 285, 286 }, ECricketPartsType.Black, 287, 1, 2, 5, 1200, 4, 300, 300, 10, 10, 2, mustSuccessLoud: false, 0, 16, 4, "525254", 90, 70, 1, 8, 4, 10, 4, 5, 25, 8, 60, 10, 1));
		_dataArray.Add(new CricketPartsItem(138, new int[2] { 288, 288 }, ECricketPartsType.Black, 289, 1, 2, 5, 1200, 4, 300, 300, 10, 10, 1, mustSuccessLoud: false, 0, 18, 4, "404040", 80, 70, 2, 9, 4, 12, 4, 5, 30, 10, 60, 10, 1));
		_dataArray.Add(new CricketPartsItem(139, new int[2] { 290, 291 }, ECricketPartsType.White, 292, 0, 2, 4, 600, 2, 150, 150, 0, 0, 24, mustSuccessLoud: false, 0, 6, 8, "dfdfdf", 40, 30, 3, 1, 2, 35, 5, -10, 0, 0, 30, 5, 1));
		_dataArray.Add(new CricketPartsItem(140, new int[2] { 293, 293 }, ECricketPartsType.White, 294, 0, 2, 4, 600, 2, 150, 150, 0, 0, 16, mustSuccessLoud: false, 0, 8, 8, "b6b4a8", 50, 40, 4, 2, 2, 30, 6, -10, 0, 0, 25, 5, 1));
		_dataArray.Add(new CricketPartsItem(141, new int[2] { 295, 295 }, ECricketPartsType.White, 296, 0, 2, 4, 600, 2, 150, 150, 0, 0, 8, mustSuccessLoud: false, 0, 10, 8, "cccbc4", 40, 50, 5, 2, 2, 35, 6, -10, 0, 0, 35, 5, 1));
		_dataArray.Add(new CricketPartsItem(142, new int[2] { 297, 298 }, ECricketPartsType.White, 299, 0, 2, 5, 600, 2, 150, 150, 0, 0, 4, mustSuccessLoud: false, 0, 12, 8, "d3c6b3", 50, 40, 6, 1, 3, 35, 7, -10, 0, 0, 30, 5, 1));
		_dataArray.Add(new CricketPartsItem(143, new int[2] { 300, 300 }, ECricketPartsType.White, 301, 0, 2, 5, 600, 2, 150, 150, 0, 0, 2, mustSuccessLoud: false, 0, 14, 8, "d9ebef", 40, 50, 8, 2, 2, 40, 7, -10, 0, 0, 35, 5, 1));
		_dataArray.Add(new CricketPartsItem(144, new int[2] { 302, 302 }, ECricketPartsType.White, 303, 0, 2, 5, 600, 2, 150, 150, 0, 0, 1, mustSuccessLoud: false, 0, 16, 8, "d6d8c1", 50, 40, 9, 2, 4, 35, 8, -10, 0, 0, 30, 5, 1));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CricketPartsItem>(145);
		CreateItems0();
		CreateItems1();
		CreateItems2();
	}
}
