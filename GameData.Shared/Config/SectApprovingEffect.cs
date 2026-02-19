using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SectApprovingEffect : ConfigData<SectApprovingEffectItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte Shaolin = 0;

		public const sbyte Emei = 1;

		public const sbyte Baihua = 2;

		public const sbyte Wudang = 3;

		public const sbyte Yuanshan = 4;

		public const sbyte Shixiang = 5;

		public const sbyte Ranshan = 6;

		public const sbyte Xuannv = 7;

		public const sbyte Zhujian = 8;

		public const sbyte Kongsang = 9;

		public const sbyte Jingang = 10;

		public const sbyte Wuxian = 11;

		public const sbyte Jieqing = 12;

		public const sbyte Fulong = 13;

		public const sbyte Xuehou = 14;
	}

	public static class DefValue
	{
		public static SectApprovingEffectItem Shaolin => Instance[(sbyte)0];

		public static SectApprovingEffectItem Emei => Instance[(sbyte)1];

		public static SectApprovingEffectItem Baihua => Instance[(sbyte)2];

		public static SectApprovingEffectItem Wudang => Instance[(sbyte)3];

		public static SectApprovingEffectItem Yuanshan => Instance[(sbyte)4];

		public static SectApprovingEffectItem Shixiang => Instance[(sbyte)5];

		public static SectApprovingEffectItem Ranshan => Instance[(sbyte)6];

		public static SectApprovingEffectItem Xuannv => Instance[(sbyte)7];

		public static SectApprovingEffectItem Zhujian => Instance[(sbyte)8];

		public static SectApprovingEffectItem Kongsang => Instance[(sbyte)9];

		public static SectApprovingEffectItem Jingang => Instance[(sbyte)10];

		public static SectApprovingEffectItem Wuxian => Instance[(sbyte)11];

		public static SectApprovingEffectItem Jieqing => Instance[(sbyte)12];

		public static SectApprovingEffectItem Fulong => Instance[(sbyte)13];

		public static SectApprovingEffectItem Xuehou => Instance[(sbyte)14];
	}

	public static SectApprovingEffect Instance = new SectApprovingEffect();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "RequirementSubstitutions", "TemplateId", "Name", "Desc", "Icon" };

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
		_dataArray.Add(new SectApprovingEffectItem(0, 0, 1, "combatskilltree_icon_0_1", new List<sbyte> { 13 }, new short[2] { 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, 100, 100));
		_dataArray.Add(new SectApprovingEffectItem(1, 2, 3, "combatskilltree_icon_0_2", new List<sbyte>(), new short[2] { 150, 50 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, 100, 100));
		_dataArray.Add(new SectApprovingEffectItem(2, 4, 5, "combatskilltree_icon_0_3", new List<sbyte> { 8 }, new short[2] { 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, 100, 100));
		_dataArray.Add(new SectApprovingEffectItem(3, 6, 7, "combatskilltree_icon_0_4", new List<sbyte> { 12 }, new short[2] { 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, 100, 100));
		_dataArray.Add(new SectApprovingEffectItem(4, 8, 9, "combatskilltree_icon_0_5", new List<sbyte>(), new short[2] { 100, 100 }, new short[5] { 150, 150, 100, 50, 50 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, 100, 100));
		_dataArray.Add(new SectApprovingEffectItem(5, 10, 11, "combatskilltree_icon_0_6", new List<sbyte>(), new short[2] { 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 50, 50, 50, 50, 50 }, new short[5] { 50, 50, 50, 50, 50 }, 500, 500));
		_dataArray.Add(new SectApprovingEffectItem(6, 12, 13, "combatskilltree_icon_0_7", new List<sbyte>(), new short[2] { 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 200, 200, 200, 200, 200 }, new short[5] { 200, 200, 200, 200, 200 }, 20, 20));
		_dataArray.Add(new SectApprovingEffectItem(7, 14, 15, "combatskilltree_icon_0_8", new List<sbyte>(), new short[2] { 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 50, 50, 50, 50, 50 }, new short[5] { 150, 150, 150, 150, 150 }, 20, 200));
		_dataArray.Add(new SectApprovingEffectItem(8, 16, 17, "combatskilltree_icon_0_9", new List<sbyte> { 6, 7, 10, 11 }, new short[2] { 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, 100, 100));
		_dataArray.Add(new SectApprovingEffectItem(9, 18, 19, "combatskilltree_icon_0_10", new List<sbyte>(), new short[2] { 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 50, 75, 100, 125, 150 }, new short[5] { 50, 75, 100, 125, 150 }, 100, 100));
		_dataArray.Add(new SectApprovingEffectItem(10, 20, 21, "combatskilltree_icon_0_11", new List<sbyte>(), new short[2] { 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 150, 150, 150, 150, 150 }, new short[5] { 50, 50, 50, 50, 50 }, 200, 20));
		_dataArray.Add(new SectApprovingEffectItem(11, 22, 23, "combatskilltree_icon_0_12", new List<sbyte> { 9 }, new short[2] { 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, 100, 100));
		_dataArray.Add(new SectApprovingEffectItem(12, 24, 25, "combatskilltree_icon_0_13", new List<sbyte>(), new short[2] { 50, 150 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, 100, 100));
		_dataArray.Add(new SectApprovingEffectItem(13, 26, 27, "combatskilltree_icon_0_14", new List<sbyte>(), new short[2] { 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 150, 125, 100, 75, 50 }, new short[5] { 150, 125, 100, 75, 50 }, 100, 100));
		_dataArray.Add(new SectApprovingEffectItem(14, 28, 29, "combatskilltree_icon_0_15", new List<sbyte>(), new short[2] { 100, 100 }, new short[5] { 50, 50, 100, 150, 150 }, new short[5] { 100, 100, 100, 100, 100 }, new short[5] { 100, 100, 100, 100, 100 }, 100, 100));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SectApprovingEffectItem>(15);
		CreateItems0();
	}
}
