using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class CraftTool : ConfigData<CraftToolItem, short>
{
	public static class DefKey
	{
		public const short Wood0 = 0;

		public const short Wood1 = 1;

		public const short Wood2 = 2;

		public const short Wood3 = 3;

		public const short Wood4 = 4;

		public const short Wood5 = 5;

		public const short Wood6 = 6;

		public const short Wood7 = 7;

		public const short Wood8 = 8;

		public const short Metal0 = 9;

		public const short Metal1 = 10;

		public const short Metal2 = 11;

		public const short Metal3 = 12;

		public const short Metal4 = 13;

		public const short Metal5 = 14;

		public const short Metal6 = 15;

		public const short Metal7 = 16;

		public const short Metal8 = 17;

		public const short Jade0 = 18;

		public const short Jade1 = 19;

		public const short Jade2 = 20;

		public const short Jade3 = 21;

		public const short Jade4 = 22;

		public const short Jade5 = 23;

		public const short Jade6 = 24;

		public const short Jade7 = 25;

		public const short Jade8 = 26;

		public const short Fabric0 = 27;

		public const short Fabric1 = 28;

		public const short Fabric2 = 29;

		public const short Fabric3 = 30;

		public const short Fabric4 = 31;

		public const short Fabric5 = 32;

		public const short Fabric6 = 33;

		public const short Fabric7 = 34;

		public const short Fabric8 = 35;

		public const short Cooking0 = 36;

		public const short Cooking1 = 37;

		public const short Cooking2 = 38;

		public const short Cooking3 = 39;

		public const short Cooking4 = 40;

		public const short Cooking5 = 41;

		public const short Cooking6 = 42;

		public const short Cooking7 = 43;

		public const short Cooking8 = 44;

		public const short Medicine0 = 45;

		public const short Medicine1 = 46;

		public const short Medicine2 = 47;

		public const short Medicine3 = 48;

		public const short Medicine4 = 49;

		public const short Medicine5 = 50;

		public const short Medicine6 = 51;

		public const short Medicine7 = 52;

		public const short Medicine8 = 53;

		public const short Empty = 54;
	}

	public static class DefValue
	{
		public static CraftToolItem Wood0 => Instance[(short)0];

		public static CraftToolItem Wood1 => Instance[(short)1];

		public static CraftToolItem Wood2 => Instance[(short)2];

		public static CraftToolItem Wood3 => Instance[(short)3];

		public static CraftToolItem Wood4 => Instance[(short)4];

		public static CraftToolItem Wood5 => Instance[(short)5];

		public static CraftToolItem Wood6 => Instance[(short)6];

		public static CraftToolItem Wood7 => Instance[(short)7];

		public static CraftToolItem Wood8 => Instance[(short)8];

		public static CraftToolItem Metal0 => Instance[(short)9];

		public static CraftToolItem Metal1 => Instance[(short)10];

		public static CraftToolItem Metal2 => Instance[(short)11];

		public static CraftToolItem Metal3 => Instance[(short)12];

		public static CraftToolItem Metal4 => Instance[(short)13];

		public static CraftToolItem Metal5 => Instance[(short)14];

		public static CraftToolItem Metal6 => Instance[(short)15];

		public static CraftToolItem Metal7 => Instance[(short)16];

		public static CraftToolItem Metal8 => Instance[(short)17];

		public static CraftToolItem Jade0 => Instance[(short)18];

		public static CraftToolItem Jade1 => Instance[(short)19];

		public static CraftToolItem Jade2 => Instance[(short)20];

		public static CraftToolItem Jade3 => Instance[(short)21];

		public static CraftToolItem Jade4 => Instance[(short)22];

		public static CraftToolItem Jade5 => Instance[(short)23];

		public static CraftToolItem Jade6 => Instance[(short)24];

		public static CraftToolItem Jade7 => Instance[(short)25];

		public static CraftToolItem Jade8 => Instance[(short)26];

		public static CraftToolItem Fabric0 => Instance[(short)27];

		public static CraftToolItem Fabric1 => Instance[(short)28];

		public static CraftToolItem Fabric2 => Instance[(short)29];

		public static CraftToolItem Fabric3 => Instance[(short)30];

		public static CraftToolItem Fabric4 => Instance[(short)31];

		public static CraftToolItem Fabric5 => Instance[(short)32];

		public static CraftToolItem Fabric6 => Instance[(short)33];

		public static CraftToolItem Fabric7 => Instance[(short)34];

		public static CraftToolItem Fabric8 => Instance[(short)35];

		public static CraftToolItem Cooking0 => Instance[(short)36];

		public static CraftToolItem Cooking1 => Instance[(short)37];

		public static CraftToolItem Cooking2 => Instance[(short)38];

		public static CraftToolItem Cooking3 => Instance[(short)39];

		public static CraftToolItem Cooking4 => Instance[(short)40];

		public static CraftToolItem Cooking5 => Instance[(short)41];

		public static CraftToolItem Cooking6 => Instance[(short)42];

		public static CraftToolItem Cooking7 => Instance[(short)43];

		public static CraftToolItem Cooking8 => Instance[(short)44];

		public static CraftToolItem Medicine0 => Instance[(short)45];

		public static CraftToolItem Medicine1 => Instance[(short)46];

		public static CraftToolItem Medicine2 => Instance[(short)47];

		public static CraftToolItem Medicine3 => Instance[(short)48];

		public static CraftToolItem Medicine4 => Instance[(short)49];

		public static CraftToolItem Medicine5 => Instance[(short)50];

		public static CraftToolItem Medicine6 => Instance[(short)51];

		public static CraftToolItem Medicine7 => Instance[(short)52];

		public static CraftToolItem Medicine8 => Instance[(short)53];

		public static CraftToolItem Empty => Instance[(short)54];
	}

	public static CraftTool Instance = new CraftTool();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"ItemSubType", "GroupId", "ResourceType", "RequiredLifeSkillTypes", "TemplateId", "Name", "Grade", "Icon", "Desc", "MaxDurability",
		"BaseWeight", "BaseHappinessChange", "DropRate", "AttainmentBonus"
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
		_dataArray.Add(new CraftToolItem(0, 0, 6, 600, 0, 0, "icon_CraftTool_mugongxiang", 1, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 60, 100, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 2, 36, new List<sbyte> { 7 }, 30, new short[9] { 3, 4, 5, 6, 7, 8, 9, 10, 20 }, 0));
		_dataArray.Add(new CraftToolItem(1, 2, 6, 600, 1, 0, "icon_CraftTool_mugongxiang", 3, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 54, 120, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 2, 36, new List<sbyte> { 7 }, 40, new short[9] { 2, 3, 4, 5, 6, 7, 8, 9, 18 }, 0));
		_dataArray.Add(new CraftToolItem(2, 4, 6, 600, 2, 0, "icon_CraftTool_mugongxiang", 5, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 48, 60, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 2, 36, new List<sbyte> { 7 }, 50, new short[9] { 1, 2, 3, 4, 5, 6, 7, 8, 16 }, 0));
		_dataArray.Add(new CraftToolItem(3, 6, 6, 600, 3, 0, "icon_CraftTool_zhuqiyinxia", 7, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 42, 110, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 2, 36, new List<sbyte> { 7 }, 70, new short[9] { 0, 1, 2, 3, 4, 5, 6, 7, 14 }, 0));
		_dataArray.Add(new CraftToolItem(4, 8, 6, 600, 4, 0, "icon_CraftTool_zhuqiyinxia", 9, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 36, 200, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 2, 36, new List<sbyte> { 7 }, 90, new short[9] { 0, 0, 1, 2, 3, 4, 5, 6, 12 }, 0));
		_dataArray.Add(new CraftToolItem(5, 10, 6, 600, 5, 0, "icon_CraftTool_zhuqiyinxia", 11, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 30, 140, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 2, 36, new List<sbyte> { 7 }, 110, new short[9] { 0, 0, 0, 1, 2, 3, 4, 5, 10 }, 0));
		_dataArray.Add(new CraftToolItem(6, 12, 6, 600, 6, 0, "icon_CraftTool_guifubao", 13, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 24, 170, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 2, 36, new List<sbyte> { 7 }, 140, new short[9] { 0, 0, 0, 0, 1, 2, 3, 4, 8 }, 0));
		_dataArray.Add(new CraftToolItem(7, 14, 6, 600, 7, 0, "icon_CraftTool_ruyiqiankunhe", 15, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 18, 100, 14100, 21150, 5, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 2, 36, new List<sbyte> { 7 }, 180, new short[9] { 0, 0, 0, 0, 0, 1, 2, 3, 6 }, 0));
		_dataArray.Add(new CraftToolItem(8, 16, 6, 600, 8, 0, "icon_CraftTool_gongshubaoxia", 17, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 12, 120, 20500, 30750, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 2, 36, new List<sbyte> { 7 }, 230, new short[9] { 0, 0, 0, 0, 0, 0, 1, 2, 4 }, 0));
		_dataArray.Add(new CraftToolItem(9, 18, 6, 600, 0, 9, "icon_CraftTool_qingtongzhulu", 19, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 60, 100, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 2, 36, new List<sbyte> { 6 }, 30, new short[9] { 3, 4, 5, 6, 7, 8, 9, 10, 20 }, 0));
		_dataArray.Add(new CraftToolItem(10, 20, 6, 600, 1, 9, "icon_CraftTool_qingtongzhulu", 21, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 54, 120, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 2, 36, new List<sbyte> { 6 }, 40, new short[9] { 2, 3, 4, 5, 6, 7, 8, 9, 18 }, 0));
		_dataArray.Add(new CraftToolItem(11, 22, 6, 600, 2, 9, "icon_CraftTool_qingtongzhulu", 23, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 48, 60, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 2, 36, new List<sbyte> { 6 }, 50, new short[9] { 1, 2, 3, 4, 5, 6, 7, 8, 16 }, 0));
		_dataArray.Add(new CraftToolItem(12, 24, 6, 600, 3, 9, "icon_CraftTool_wujinzhulu", 25, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 42, 110, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 2, 36, new List<sbyte> { 6 }, 70, new short[9] { 0, 1, 2, 3, 4, 5, 6, 7, 14 }, 0));
		_dataArray.Add(new CraftToolItem(13, 26, 6, 600, 4, 9, "icon_CraftTool_wujinzhulu", 27, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 36, 200, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 2, 36, new List<sbyte> { 6 }, 90, new short[9] { 0, 0, 1, 2, 3, 4, 5, 6, 12 }, 0));
		_dataArray.Add(new CraftToolItem(14, 28, 6, 600, 5, 9, "icon_CraftTool_wujinzhulu", 29, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 30, 140, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 2, 36, new List<sbyte> { 6 }, 110, new short[9] { 0, 0, 0, 1, 2, 3, 4, 5, 10 }, 0));
		_dataArray.Add(new CraftToolItem(15, 30, 6, 600, 6, 9, "icon_CraftTool_qixiazhenhuolian", 31, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 24, 170, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 2, 36, new List<sbyte> { 6 }, 140, new short[9] { 0, 0, 0, 0, 1, 2, 3, 4, 8 }, 0));
		_dataArray.Add(new CraftToolItem(16, 32, 6, 600, 7, 9, "icon_CraftTool_jiuhanzhulu", 33, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 18, 100, 14100, 21150, 5, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 2, 36, new List<sbyte> { 6 }, 180, new short[9] { 0, 0, 0, 0, 0, 1, 2, 3, 6 }, 0));
		_dataArray.Add(new CraftToolItem(17, 34, 6, 600, 8, 9, "icon_CraftTool_rexielian", 35, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 12, 120, 20500, 30750, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 2, 36, new List<sbyte> { 6 }, 230, new short[9] { 0, 0, 0, 0, 0, 0, 1, 2, 4 }, 0));
		_dataArray.Add(new CraftToolItem(18, 36, 6, 600, 0, 18, "icon_CraftTool_heidansha", 37, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 60, 100, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 2, 36, new List<sbyte> { 11 }, 30, new short[9] { 3, 4, 5, 6, 7, 8, 9, 10, 20 }, 0));
		_dataArray.Add(new CraftToolItem(19, 38, 6, 600, 1, 18, "icon_CraftTool_heidansha", 39, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 54, 120, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 2, 36, new List<sbyte> { 11 }, 40, new short[9] { 2, 3, 4, 5, 6, 7, 8, 9, 18 }, 0));
		_dataArray.Add(new CraftToolItem(20, 40, 6, 600, 2, 18, "icon_CraftTool_heidansha", 41, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 48, 60, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 2, 36, new List<sbyte> { 11 }, 50, new short[9] { 1, 2, 3, 4, 5, 6, 7, 8, 16 }, 0));
		_dataArray.Add(new CraftToolItem(21, 42, 6, 600, 3, 18, "icon_CraftTool_huanglongsha", 43, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 42, 110, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 2, 36, new List<sbyte> { 11 }, 70, new short[9] { 0, 1, 2, 3, 4, 5, 6, 7, 14 }, 0));
		_dataArray.Add(new CraftToolItem(22, 44, 6, 600, 4, 18, "icon_CraftTool_huanglongsha", 45, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 36, 200, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 2, 36, new List<sbyte> { 11 }, 90, new short[9] { 0, 0, 1, 2, 3, 4, 5, 6, 12 }, 0));
		_dataArray.Add(new CraftToolItem(23, 46, 6, 600, 5, 18, "icon_CraftTool_huanglongsha", 47, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 30, 140, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 2, 36, new List<sbyte> { 11 }, 110, new short[9] { 0, 0, 0, 1, 2, 3, 4, 5, 10 }, 0));
		_dataArray.Add(new CraftToolItem(24, 48, 6, 600, 6, 18, "icon_CraftTool_zijinchenxiangsha", 49, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 24, 170, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 2, 36, new List<sbyte> { 11 }, 140, new short[9] { 0, 0, 0, 0, 1, 2, 3, 4, 8 }, 0));
		_dataArray.Add(new CraftToolItem(25, 50, 6, 600, 7, 18, "icon_CraftTool_bingjingxielousha", 51, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 18, 100, 14100, 21150, 5, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 2, 36, new List<sbyte> { 11 }, 180, new short[9] { 0, 0, 0, 0, 0, 1, 2, 3, 6 }, 0));
		_dataArray.Add(new CraftToolItem(26, 52, 6, 600, 8, 18, "icon_CraftTool_longnvsha", 53, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 12, 120, 20500, 30750, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 2, 36, new List<sbyte> { 11 }, 230, new short[9] { 0, 0, 0, 0, 0, 0, 1, 2, 4 }, 0));
		_dataArray.Add(new CraftToolItem(27, 54, 6, 600, 0, 27, "icon_CraftTool_zhenxianbao", 55, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 60, 100, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 2, 36, new List<sbyte> { 10 }, 30, new short[9] { 3, 4, 5, 6, 7, 8, 9, 10, 20 }, 0));
		_dataArray.Add(new CraftToolItem(28, 56, 6, 600, 1, 27, "icon_CraftTool_zhenxianbao", 57, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 54, 120, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 2, 36, new List<sbyte> { 10 }, 40, new short[9] { 2, 3, 4, 5, 6, 7, 8, 9, 18 }, 0));
		_dataArray.Add(new CraftToolItem(29, 58, 6, 600, 2, 27, "icon_CraftTool_zhenxianbao", 59, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 48, 60, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 2, 36, new List<sbyte> { 10 }, 50, new short[9] { 1, 2, 3, 4, 5, 6, 7, 8, 16 }, 0));
		_dataArray.Add(new CraftToolItem(30, 60, 6, 600, 3, 27, "icon_CraftTool_liuyunsuo", 61, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 42, 110, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 2, 36, new List<sbyte> { 10 }, 70, new short[9] { 0, 1, 2, 3, 4, 5, 6, 7, 14 }, 0));
		_dataArray.Add(new CraftToolItem(31, 62, 6, 600, 4, 27, "icon_CraftTool_liuyunsuo", 63, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 36, 200, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 2, 36, new List<sbyte> { 10 }, 90, new short[9] { 0, 0, 1, 2, 3, 4, 5, 6, 12 }, 0));
		_dataArray.Add(new CraftToolItem(32, 64, 6, 600, 5, 27, "icon_CraftTool_liuyunsuo", 65, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 30, 140, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 2, 36, new List<sbyte> { 10 }, 110, new short[9] { 0, 0, 0, 1, 2, 3, 4, 5, 10 }, 0));
		_dataArray.Add(new CraftToolItem(33, 66, 6, 600, 6, 27, "icon_CraftTool_huangmuzhiji", 67, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 24, 170, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 2, 36, new List<sbyte> { 10 }, 140, new short[9] { 0, 0, 0, 0, 1, 2, 3, 4, 8 }, 0));
		_dataArray.Add(new CraftToolItem(34, 68, 6, 600, 7, 27, "icon_CraftTool_qiseliuxiangsuo", 69, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 18, 100, 14100, 21150, 5, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 2, 36, new List<sbyte> { 10 }, 180, new short[9] { 0, 0, 0, 0, 0, 1, 2, 3, 6 }, 0));
		_dataArray.Add(new CraftToolItem(35, 70, 6, 600, 8, 27, "icon_CraftTool_tiannvbaoxiasuo", 71, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 12, 120, 20500, 30750, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 2, 36, new List<sbyte> { 10 }, 230, new short[9] { 0, 0, 0, 0, 0, 0, 1, 2, 4 }, 0));
		_dataArray.Add(new CraftToolItem(36, 72, 6, 600, 0, 36, "icon_CraftTool_danguo", 73, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 120, 340, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 2, 36, new List<sbyte> { 14 }, 30, new short[9] { 3, 4, 5, 6, 7, 8, 9, 10, 20 }, -1));
		_dataArray.Add(new CraftToolItem(37, 74, 6, 600, 1, 36, "icon_CraftTool_danguo", 75, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 108, 300, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 2, 36, new List<sbyte> { 14 }, 40, new short[9] { 2, 3, 4, 5, 6, 7, 8, 9, 18 }, -1));
		_dataArray.Add(new CraftToolItem(38, 76, 6, 600, 2, 36, "icon_CraftTool_danguo", 77, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 96, 320, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 2, 36, new List<sbyte> { 14 }, 50, new short[9] { 1, 2, 3, 4, 5, 6, 7, 8, 16 }, -1));
		_dataArray.Add(new CraftToolItem(39, 78, 6, 600, 3, 36, "icon_CraftTool_zaowangguo", 79, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 84, 340, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 2, 36, new List<sbyte> { 14 }, 70, new short[9] { 0, 1, 2, 3, 4, 5, 6, 7, 14 }, -1));
		_dataArray.Add(new CraftToolItem(40, 80, 6, 600, 4, 36, "icon_CraftTool_zaowangguo", 81, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 72, 400, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 2, 36, new List<sbyte> { 14 }, 90, new short[9] { 0, 0, 1, 2, 3, 4, 5, 6, 12 }, -1));
		_dataArray.Add(new CraftToolItem(41, 82, 6, 600, 5, 36, "icon_CraftTool_zaowangguo", 83, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 60, 280, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 2, 36, new List<sbyte> { 14 }, 110, new short[9] { 0, 0, 0, 1, 2, 3, 4, 5, 10 }, -1));
		_dataArray.Add(new CraftToolItem(42, 84, 6, 600, 6, 36, "icon_CraftTool_babaoLuheguo", 85, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 48, 360, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 2, 36, new List<sbyte> { 14 }, 140, new short[9] { 0, 0, 0, 0, 1, 2, 3, 4, 8 }, -1));
		_dataArray.Add(new CraftToolItem(43, 86, 6, 600, 7, 36, "icon_CraftTool_xuantiegunjinguo", 87, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 36, 480, 14100, 21150, 5, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 2, 36, new List<sbyte> { 14 }, 180, new short[9] { 0, 0, 0, 0, 0, 1, 2, 3, 6 }, -1));
		_dataArray.Add(new CraftToolItem(44, 88, 6, 600, 8, 36, "icon_CraftTool_qiandouguo", 89, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 24, 300, 20500, 30750, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 2, 36, new List<sbyte> { 14 }, 230, new short[9] { 0, 0, 0, 0, 0, 0, 1, 2, 4 }, -1));
		_dataArray.Add(new CraftToolItem(45, 90, 6, 600, 0, 45, "icon_CraftTool_taotuyaobo", 91, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 120, 180, 100, 150, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 3, 36, new List<sbyte> { 8, 9 }, 30, new short[9] { 3, 4, 5, 6, 7, 8, 9, 10, 20 }, 0));
		_dataArray.Add(new CraftToolItem(46, 92, 6, 600, 1, 45, "icon_CraftTool_taotuyaobo", 93, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 108, 220, 200, 300, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 3, 36, new List<sbyte> { 8, 9 }, 40, new short[9] { 2, 3, 4, 5, 6, 7, 8, 9, 18 }, 0));
		_dataArray.Add(new CraftToolItem(47, 94, 6, 600, 2, 45, "icon_CraftTool_taotuyaobo", 95, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 96, 280, 600, 900, 0, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 3, 36, new List<sbyte> { 8, 9 }, 50, new short[9] { 1, 2, 3, 4, 5, 6, 7, 8, 16 }, 0));
		_dataArray.Add(new CraftToolItem(48, 96, 6, 600, 3, 45, "icon_CraftTool_biyuputibo", 97, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 84, 200, 1500, 2250, 1, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 3, 36, new List<sbyte> { 8, 9 }, 70, new short[9] { 0, 1, 2, 3, 4, 5, 6, 7, 14 }, 0));
		_dataArray.Add(new CraftToolItem(49, 98, 6, 600, 4, 45, "icon_CraftTool_biyuputibo", 99, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 72, 300, 3100, 4650, 2, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 3, 36, new List<sbyte> { 8, 9 }, 90, new short[9] { 0, 0, 1, 2, 3, 4, 5, 6, 12 }, 0));
		_dataArray.Add(new CraftToolItem(50, 100, 6, 600, 5, 45, "icon_CraftTool_biyuputibo", 101, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 60, 160, 5600, 8400, 3, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 3, 36, new List<sbyte> { 8, 9 }, 110, new short[9] { 0, 0, 0, 1, 2, 3, 4, 5, 10 }, 0));
		_dataArray.Add(new CraftToolItem(51, 102, 6, 600, 6, 45, "icon_CraftTool_xieyubo", 103, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 48, 240, 9200, 13800, 4, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 3, 36, new List<sbyte> { 8, 9 }, 140, new short[9] { 0, 0, 0, 0, 1, 2, 3, 4, 8 }, 0));
		_dataArray.Add(new CraftToolItem(52, 104, 6, 600, 7, 45, "icon_CraftTool_tianxiangliuliding", 105, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 36, 260, 14100, 21150, 5, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 3, 36, new List<sbyte> { 8, 9 }, 180, new short[9] { 0, 0, 0, 0, 0, 1, 2, 3, 6 }, 0));
		_dataArray.Add(new CraftToolItem(53, 106, 6, 600, 8, 45, "icon_CraftTool_jiuchenyugulu", 107, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 24, 220, 20500, 30750, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 3, 36, new List<sbyte> { 8, 9 }, 230, new short[9] { 0, 0, 0, 0, 0, 0, 1, 2, 4 }, 0));
		_dataArray.Add(new CraftToolItem(54, 108, 6, 600, 0, -1, "icon_Weapon_kongshou", 109, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, -1, 0, new List<sbyte> { 7, 6, 11, 10, 14, 8, 9 }, 0, new short[9], 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CraftToolItem>(55);
		CreateItems0();
	}
}
