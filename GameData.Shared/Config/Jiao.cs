using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class Jiao : ConfigData<JiaoItem, short>
{
	public static class DefKey
	{
		public const short JiaoWhite = 0;

		public const short JiaoBlack = 1;

		public const short JiaoGreen = 2;

		public const short JiaoRed = 3;

		public const short JiaoYellow = 4;

		public const short JiaoWB = 5;

		public const short JiaoWG = 6;

		public const short JiaoWR = 7;

		public const short JiaoWY = 8;

		public const short JiaoBG = 9;

		public const short JiaoBR = 10;

		public const short JiaoBY = 11;

		public const short JiaoGR = 12;

		public const short JiaoGY = 13;

		public const short JiaoRY = 14;

		public const short JiaoWBG = 15;

		public const short JiaoWBR = 16;

		public const short JiaoWBY = 17;

		public const short JiaoWGR = 18;

		public const short JiaoWGY = 19;

		public const short JiaoWRY = 20;

		public const short JiaoBGR = 21;

		public const short JiaoBGY = 22;

		public const short JiaoBRY = 23;

		public const short JiaoGRY = 24;

		public const short JiaoWBGR = 25;

		public const short JiaoWBGY = 26;

		public const short JiaoWBRY = 27;

		public const short JiaoWGRY = 28;

		public const short JiaoBGRY = 29;

		public const short JiaoWGRYB = 30;

		public const short Qiuniu = 31;

		public const short Yazi = 32;

		public const short Chaofeng = 33;

		public const short Pulao = 34;

		public const short Suanni = 35;

		public const short Baxia = 36;

		public const short Bian = 37;

		public const short Fuxi = 38;

		public const short Chiwen = 39;
	}

	public static class DefValue
	{
		public static JiaoItem JiaoWhite => Instance[(short)0];

		public static JiaoItem JiaoBlack => Instance[(short)1];

		public static JiaoItem JiaoGreen => Instance[(short)2];

		public static JiaoItem JiaoRed => Instance[(short)3];

		public static JiaoItem JiaoYellow => Instance[(short)4];

		public static JiaoItem JiaoWB => Instance[(short)5];

		public static JiaoItem JiaoWG => Instance[(short)6];

		public static JiaoItem JiaoWR => Instance[(short)7];

		public static JiaoItem JiaoWY => Instance[(short)8];

		public static JiaoItem JiaoBG => Instance[(short)9];

		public static JiaoItem JiaoBR => Instance[(short)10];

		public static JiaoItem JiaoBY => Instance[(short)11];

		public static JiaoItem JiaoGR => Instance[(short)12];

		public static JiaoItem JiaoGY => Instance[(short)13];

		public static JiaoItem JiaoRY => Instance[(short)14];

		public static JiaoItem JiaoWBG => Instance[(short)15];

		public static JiaoItem JiaoWBR => Instance[(short)16];

		public static JiaoItem JiaoWBY => Instance[(short)17];

		public static JiaoItem JiaoWGR => Instance[(short)18];

		public static JiaoItem JiaoWGY => Instance[(short)19];

		public static JiaoItem JiaoWRY => Instance[(short)20];

		public static JiaoItem JiaoBGR => Instance[(short)21];

		public static JiaoItem JiaoBGY => Instance[(short)22];

		public static JiaoItem JiaoBRY => Instance[(short)23];

		public static JiaoItem JiaoGRY => Instance[(short)24];

		public static JiaoItem JiaoWBGR => Instance[(short)25];

		public static JiaoItem JiaoWBGY => Instance[(short)26];

		public static JiaoItem JiaoWBRY => Instance[(short)27];

		public static JiaoItem JiaoWGRY => Instance[(short)28];

		public static JiaoItem JiaoBGRY => Instance[(short)29];

		public static JiaoItem JiaoWGRYB => Instance[(short)30];

		public static JiaoItem Qiuniu => Instance[(short)31];

		public static JiaoItem Yazi => Instance[(short)32];

		public static JiaoItem Chaofeng => Instance[(short)33];

		public static JiaoItem Pulao => Instance[(short)34];

		public static JiaoItem Suanni => Instance[(short)35];

		public static JiaoItem Baxia => Instance[(short)36];

		public static JiaoItem Bian => Instance[(short)37];

		public static JiaoItem Fuxi => Instance[(short)38];

		public static JiaoItem Chiwen => Instance[(short)39];
	}

	public static Jiao Instance = new Jiao();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"AdvantageProperty", "IndexOfMiscTemplate", "IndexOfCharacterTemplate", "IndexOfCarrierTemplate", "IndexOfAnimalTemplate", "EggMaterial", "TeenagerMaterial", "TemplateId", "Name", "AdvantagePropertyValue",
		"ShadowImage", "BellowSound"
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
		_dataArray.Add(new JiaoItem(0, 0, 2, 150, 500, 40, 0, 0, 0, 0, 0, 4650, 9300, 10, 4200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai" }, 289, 696, 46, 39, 280, 311, null, null));
		_dataArray.Add(new JiaoItem(1, 1, 2, 150, 500, 0, 0, 2, 0, 0, 0, 4650, 9300, 10, 4200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_hei" }, 290, 697, 47, 40, 281, 312, null, null));
		_dataArray.Add(new JiaoItem(2, 2, 2, 150, 500, 0, 0, 0, 60, 0, 0, 4650, 9300, 10, 4200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_qing" }, 291, 698, 48, 41, 282, 313, null, null));
		_dataArray.Add(new JiaoItem(3, 3, 2, 150, 500, 0, 0, 0, 0, 60, 30, 4650, 9300, 10, 4200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_hong" }, 292, 699, 49, 42, 283, 314, null, null));
		_dataArray.Add(new JiaoItem(4, 4, 2, 150, 500, 0, 12000, 0, 0, 0, 0, 4650, 9300, 10, 4200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_huang" }, 293, 700, 50, 43, 284, 315, null, null));
		_dataArray.Add(new JiaoItem(5, 5, 3, 200, 1500, 40, 0, 2, 0, 0, 0, 8400, 16800, 12, 5400, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_hei" }, 294, 701, 51, 44, 285, 316, null, null));
		_dataArray.Add(new JiaoItem(6, 6, 3, 200, 1500, 40, 0, 0, 60, 0, 0, 8400, 16800, 12, 5400, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_qing" }, 295, 702, 52, 45, 286, 317, null, null));
		_dataArray.Add(new JiaoItem(7, 7, 3, 200, 1500, 40, 0, 0, 0, 60, 30, 8400, 16800, 12, 5400, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_hong" }, 296, 703, 53, 46, 287, 318, null, null));
		_dataArray.Add(new JiaoItem(8, 8, 3, 200, 1500, 40, 12000, 0, 0, 0, 0, 8400, 16800, 12, 5400, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_huang" }, 297, 704, 54, 47, 288, 319, null, null));
		_dataArray.Add(new JiaoItem(9, 9, 3, 200, 1500, 0, 0, 2, 60, 0, 0, 8400, 16800, 12, 5400, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_qing" }, 298, 705, 55, 48, 289, 320, null, null));
		_dataArray.Add(new JiaoItem(10, 10, 3, 200, 1500, 0, 0, 2, 0, 60, 30, 8400, 16800, 12, 5400, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_hong" }, 299, 706, 56, 49, 290, 321, null, null));
		_dataArray.Add(new JiaoItem(11, 11, 3, 200, 1500, 0, 12000, 2, 0, 0, 0, 8400, 16800, 12, 5400, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_huang" }, 300, 707, 57, 50, 291, 322, null, null));
		_dataArray.Add(new JiaoItem(12, 12, 3, 200, 1500, 0, 0, 0, 60, 60, 30, 8400, 16800, 12, 5400, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_hong" }, 301, 708, 58, 51, 292, 323, null, null));
		_dataArray.Add(new JiaoItem(13, 13, 3, 200, 1500, 0, 12000, 0, 60, 0, 0, 8400, 16800, 12, 5400, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_huang" }, 302, 709, 59, 52, 293, 324, null, null));
		_dataArray.Add(new JiaoItem(14, 14, 3, 200, 1500, 0, 12000, 0, 0, 60, 30, 8400, 16800, 12, 5400, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_hong", "eff_building_jiaopool_wuse_3_huang" }, 303, 710, 60, 53, 294, 325, null, null));
		_dataArray.Add(new JiaoItem(15, 15, 4, 300, 2500, 40, 0, 2, 60, 0, 0, 13800, 27600, 14, 7200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_qing" }, 304, 711, 61, 54, 295, 326, null, null));
		_dataArray.Add(new JiaoItem(16, 16, 4, 300, 2500, 40, 0, 2, 0, 60, 30, 13800, 27600, 14, 7200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_hong" }, 305, 712, 62, 55, 296, 327, null, null));
		_dataArray.Add(new JiaoItem(17, 17, 4, 300, 2500, 40, 12000, 2, 0, 0, 0, 13800, 27600, 14, 7200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_huang" }, 306, 713, 63, 56, 297, 328, null, null));
		_dataArray.Add(new JiaoItem(18, 18, 4, 300, 2500, 40, 0, 0, 60, 60, 30, 13800, 27600, 14, 7200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_hong" }, 307, 714, 64, 57, 298, 329, null, null));
		_dataArray.Add(new JiaoItem(19, 19, 4, 300, 2500, 40, 12000, 0, 60, 0, 0, 13800, 27600, 14, 7200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_huang" }, 308, 715, 65, 58, 299, 330, null, null));
		_dataArray.Add(new JiaoItem(20, 20, 4, 300, 2500, 40, 12000, 0, 0, 60, 30, 13800, 27600, 14, 7200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_hong", "eff_building_jiaopool_wuse_3_huang" }, 309, 716, 66, 59, 300, 331, null, null));
		_dataArray.Add(new JiaoItem(21, 21, 4, 300, 2500, 0, 0, 2, 60, 60, 30, 13800, 27600, 14, 7200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_hong" }, 310, 717, 67, 60, 301, 332, null, null));
		_dataArray.Add(new JiaoItem(22, 22, 4, 300, 2500, 0, 12000, 2, 60, 0, 0, 13800, 27600, 14, 7200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_huang" }, 311, 718, 68, 61, 302, 333, null, null));
		_dataArray.Add(new JiaoItem(23, 23, 4, 300, 2500, 0, 12000, 2, 0, 60, 30, 13800, 27600, 14, 7200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_hong", "eff_building_jiaopool_wuse_3_huang" }, 312, 719, 69, 62, 303, 334, null, null));
		_dataArray.Add(new JiaoItem(24, 24, 4, 300, 2500, 0, 12000, 0, 60, 60, 30, 13800, 27600, 14, 7200, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_hong", "eff_building_jiaopool_wuse_3_huang" }, 313, 720, 70, 63, 304, 335, null, null));
		_dataArray.Add(new JiaoItem(25, 25, 5, 400, 3500, 40, 0, 2, 60, 60, 30, 21150, 42300, 16, 9000, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_hong" }, 314, 721, 71, 64, 305, 336, null, null));
		_dataArray.Add(new JiaoItem(26, 26, 5, 400, 3500, 40, 12000, 2, 60, 0, 0, 21150, 42300, 16, 9000, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_huang" }, 315, 722, 72, 65, 306, 337, null, null));
		_dataArray.Add(new JiaoItem(27, 27, 5, 400, 3500, 40, 12000, 2, 0, 60, 30, 21150, 42300, 16, 9000, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_hong", "eff_building_jiaopool_wuse_3_huang" }, 316, 723, 73, 66, 307, 338, null, null));
		_dataArray.Add(new JiaoItem(28, 28, 5, 400, 3500, 40, 12000, 0, 60, 60, 30, 21150, 42300, 16, 9000, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_hong", "eff_building_jiaopool_wuse_3_huang" }, 317, 724, 74, 67, 308, 339, null, null));
		_dataArray.Add(new JiaoItem(29, 29, 5, 400, 3500, 0, 12000, 2, 60, 60, 30, 21150, 42300, 16, 9000, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_hong", "eff_building_jiaopool_wuse_3_huang" }, 318, 725, 75, 68, 309, 340, null, null));
		_dataArray.Add(new JiaoItem(30, 30, 5, 450, 4500, 40, 12000, 2, 60, 60, 30, 30750, 61500, 18, 10800, 8, -1, -1, 0, new List<string> { "eff_building_jiaopool_wuse_3_bai", "eff_building_jiaopool_wuse_3_hei", "eff_building_jiaopool_wuse_3_qing", "eff_building_jiaopool_wuse_3_hong", "eff_building_jiaopool_wuse_3_huang" }, 319, 726, 76, 69, 310, 341, null, null));
		_dataArray.Add(new JiaoItem(31, 31, -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 54, 0, 8, 7, 54, 30, new List<string> { "" }, -1, 727, 77, 70, -1, -1, "NpcFace_shadow_qiuniu", "ui_building_jiaochi_hualong_chuxian_qiuniu"));
		_dataArray.Add(new JiaoItem(32, 32, -1, -1, -1, 0, 0, 18, 0, 0, 0, 0, 0, 0, 0, 8, 4, 18, 30, new List<string> { "" }, -1, 728, 78, 71, -1, -1, "NpcFace_shadow_yazi", "ui_building_jiaochi_hualong_chuxian_yazi"));
		_dataArray.Add(new JiaoItem(33, 33, -1, -1, -1, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 90, 20, new List<string> { "" }, -1, 729, 79, 72, -1, -1, "NpcFace_shadow_chaofeng", "ui_building_jiaochi_hualong_chuxian_chaofeng"));
		_dataArray.Add(new JiaoItem(34, 34, -1, -1, -1, 0, 0, 0, 200, 0, 0, 0, 0, 0, 0, 8, 2, 200, 30, new List<string> { "" }, -1, 730, 80, 73, -1, -1, "NpcFace_shadow_pulao", "ui_building_jiaochi_hualong_chuxian_pulao"));
		_dataArray.Add(new JiaoItem(35, 35, -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 184500, 0, 0, 8, 6, 184500, 60, new List<string> { "" }, -1, 731, 81, 74, -1, -1, "NpcFace_shadow_suanni", "ui_building_jiaochi_hualong_chuxian_suanni"));
		_dataArray.Add(new JiaoItem(36, 36, -1, -1, -1, 0, 30000, 0, 0, 0, 0, 0, 0, 0, 0, 8, 1, 30000, 30, new List<string> { "" }, -1, 732, 82, 75, -1, -1, "NpcFace_shadow_baxia", "ui_building_jiaochi_hualong_chuxian_baxia"));
		_dataArray.Add(new JiaoItem(37, 37, -1, -1, -1, 0, 0, 0, 0, 200, 0, 0, 0, 0, 0, 8, 3, 200, 20, new List<string> { "" }, -1, 733, 83, 76, -1, -1, "NpcFace_shadow_bian", "ui_building_jiaochi_hualong_chuxian_bian"));
		_dataArray.Add(new JiaoItem(38, 38, -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 32400, 8, 8, 32400, 60, new List<string> { "" }, -1, 734, 84, 77, -1, -1, "NpcFace_shadow_fuxi", "ui_building_jiaochi_hualong_chuxian_loong"));
		_dataArray.Add(new JiaoItem(39, 39, -1, -1, -1, 0, 0, 0, 0, 0, 80, 0, 0, 0, 0, 8, 5, 80, 30, new List<string> { "" }, -1, 735, 85, 78, -1, -1, "NpcFace_shadow_chiwen", "ui_building_jiaochi_hualong_chuxian_chiwen"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<JiaoItem>(40);
		CreateItems0();
	}
}
