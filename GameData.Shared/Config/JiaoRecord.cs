using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class JiaoRecord : ConfigData<JiaoRecordItem, short>
{
	public static class DefKey
	{
		public const short Incubate = 0;

		public const short Grow = 1;

		public const short PlayWithFish = 2;

		public const short PlayWithBoat = 3;

		public const short PlayWithLongFeng = 4;

		public const short PlayInstrument = 5;

		public const short Paint = 6;

		public const short GiveTreasure = 7;

		public const short GiveCraft = 8;

		public const short DanceWithMusic = 9;

		public const short Chat = 10;

		public const short RandomGrow = 11;

		public const short HeightGrow = 12;

		public const short WeightGrow = 13;

		public const short LifeGrow = 14;

		public const short Escape = 15;

		public const short TameBad = 16;

		public const short TameGood = 17;

		public const short BecomeAdult = 18;

		public const short StartBreeding = 19;

		public const short FinishBreeding = 20;

		public const short RandomGrowGift = 21;

		public const short TameBadGift = 22;

		public const short TameGoodGift = 23;

		public const short GrowGift = 24;

		public const short GrowPercent = 25;

		public const short RandomGrowPercent = 26;

		public const short TameBadPercent = 27;

		public const short TameGoodPercent = 28;

		public const short GrowFloat = 29;

		public const short RandomGrowFloat = 30;

		public const short TameBadFloat = 31;

		public const short TameGoodFloat = 32;

		public const short PlayWithFabric = 33;
	}

	public static class DefValue
	{
		public static JiaoRecordItem Incubate => Instance[(short)0];

		public static JiaoRecordItem Grow => Instance[(short)1];

		public static JiaoRecordItem PlayWithFish => Instance[(short)2];

		public static JiaoRecordItem PlayWithBoat => Instance[(short)3];

		public static JiaoRecordItem PlayWithLongFeng => Instance[(short)4];

		public static JiaoRecordItem PlayInstrument => Instance[(short)5];

		public static JiaoRecordItem Paint => Instance[(short)6];

		public static JiaoRecordItem GiveTreasure => Instance[(short)7];

		public static JiaoRecordItem GiveCraft => Instance[(short)8];

		public static JiaoRecordItem DanceWithMusic => Instance[(short)9];

		public static JiaoRecordItem Chat => Instance[(short)10];

		public static JiaoRecordItem RandomGrow => Instance[(short)11];

		public static JiaoRecordItem HeightGrow => Instance[(short)12];

		public static JiaoRecordItem WeightGrow => Instance[(short)13];

		public static JiaoRecordItem LifeGrow => Instance[(short)14];

		public static JiaoRecordItem Escape => Instance[(short)15];

		public static JiaoRecordItem TameBad => Instance[(short)16];

		public static JiaoRecordItem TameGood => Instance[(short)17];

		public static JiaoRecordItem BecomeAdult => Instance[(short)18];

		public static JiaoRecordItem StartBreeding => Instance[(short)19];

		public static JiaoRecordItem FinishBreeding => Instance[(short)20];

		public static JiaoRecordItem RandomGrowGift => Instance[(short)21];

		public static JiaoRecordItem TameBadGift => Instance[(short)22];

		public static JiaoRecordItem TameGoodGift => Instance[(short)23];

		public static JiaoRecordItem GrowGift => Instance[(short)24];

		public static JiaoRecordItem GrowPercent => Instance[(short)25];

		public static JiaoRecordItem RandomGrowPercent => Instance[(short)26];

		public static JiaoRecordItem TameBadPercent => Instance[(short)27];

		public static JiaoRecordItem TameGoodPercent => Instance[(short)28];

		public static JiaoRecordItem GrowFloat => Instance[(short)29];

		public static JiaoRecordItem RandomGrowFloat => Instance[(short)30];

		public static JiaoRecordItem TameBadFloat => Instance[(short)31];

		public static JiaoRecordItem TameGoodFloat => Instance[(short)32];

		public static JiaoRecordItem PlayWithFabric => Instance[(short)33];
	}

	public static JiaoRecord Instance = new JiaoRecord();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "Parameters" };

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
		_dataArray.Add(new JiaoRecordItem(0, 0, 1, new string[4] { "Jiao1Name", "JiaoEggName", "", "" }));
		_dataArray.Add(new JiaoRecordItem(1, 2, 3, new string[4] { "Jiao1Name", "Nurturance", "PropertyType", "Integer" }));
		_dataArray.Add(new JiaoRecordItem(2, 4, 5, new string[4] { "Jiao1Name", "Float", "", "" }));
		_dataArray.Add(new JiaoRecordItem(3, 4, 6, new string[4] { "Jiao1Name", "Integer", "", "" }));
		_dataArray.Add(new JiaoRecordItem(4, 4, 7, new string[4] { "Jiao1Name", "Integer", "", "" }));
		_dataArray.Add(new JiaoRecordItem(5, 4, 8, new string[4] { "Jiao1Name", "Integer", "", "" }));
		_dataArray.Add(new JiaoRecordItem(6, 4, 9, new string[4] { "Jiao1Name", "Integer", "", "" }));
		_dataArray.Add(new JiaoRecordItem(7, 4, 10, new string[4] { "Jiao1Name", "", "", "" }));
		_dataArray.Add(new JiaoRecordItem(8, 4, 11, new string[4] { "Jiao1Name", "", "", "" }));
		_dataArray.Add(new JiaoRecordItem(9, 4, 12, new string[4] { "Jiao1Name", "", "", "" }));
		_dataArray.Add(new JiaoRecordItem(10, 4, 13, new string[4] { "Jiao1Name", "", "", "" }));
		_dataArray.Add(new JiaoRecordItem(11, 4, 14, new string[4] { "Jiao1Name", "PropertyType", "Integer", "" }));
		_dataArray.Add(new JiaoRecordItem(12, 2, 15, new string[4] { "Jiao1Name", "Integer", "", "" }));
		_dataArray.Add(new JiaoRecordItem(13, 2, 16, new string[4] { "Jiao1Name", "Integer", "", "" }));
		_dataArray.Add(new JiaoRecordItem(14, 2, 17, new string[4] { "Jiao1Name", "Integer", "", "" }));
		_dataArray.Add(new JiaoRecordItem(15, 18, 19, new string[4] { "Jiao1Name", "", "", "" }));
		_dataArray.Add(new JiaoRecordItem(16, 20, 21, new string[4] { "Jiao1Name", "PropertyType", "Integer", "" }));
		_dataArray.Add(new JiaoRecordItem(17, 20, 22, new string[4] { "Jiao1Name", "PropertyType", "Integer", "" }));
		_dataArray.Add(new JiaoRecordItem(18, 23, 24, new string[4] { "Jiao1Name", "", "", "" }));
		_dataArray.Add(new JiaoRecordItem(19, 25, 26, new string[4] { "Jiao1Name", "Jiao2Name", "", "" }));
		_dataArray.Add(new JiaoRecordItem(20, 27, 28, new string[4] { "Jiao1Name", "Jiao2Name", "JiaoEggName", "" }));
		_dataArray.Add(new JiaoRecordItem(21, 4, 29, null));
		_dataArray.Add(new JiaoRecordItem(22, 20, 30, new string[4] { "Jiao1Name", "PropertyType", "", "" }));
		_dataArray.Add(new JiaoRecordItem(23, 20, 31, new string[4] { "Jiao1Name", "PropertyType", "", "" }));
		_dataArray.Add(new JiaoRecordItem(24, 2, 32, new string[4] { "Jiao1Name", "Nurturance", "PropertyType", "" }));
		_dataArray.Add(new JiaoRecordItem(25, 2, 33, new string[4] { "Jiao1Name", "Nurturance", "PropertyType", "Integer" }));
		_dataArray.Add(new JiaoRecordItem(26, 4, 34, new string[4] { "Jiao1Name", "PropertyType", "Integer", "" }));
		_dataArray.Add(new JiaoRecordItem(27, 20, 35, new string[4] { "Jiao1Name", "PropertyType", "Integer", "" }));
		_dataArray.Add(new JiaoRecordItem(28, 20, 36, new string[4] { "Jiao1Name", "PropertyType", "Integer", "" }));
		_dataArray.Add(new JiaoRecordItem(29, 2, 3, new string[4] { "Jiao1Name", "Nurturance", "PropertyType", "Float" }));
		_dataArray.Add(new JiaoRecordItem(30, 4, 14, new string[4] { "Jiao1Name", "PropertyType", "Float", "" }));
		_dataArray.Add(new JiaoRecordItem(31, 20, 21, new string[4] { "Jiao1Name", "PropertyType", "Float", "" }));
		_dataArray.Add(new JiaoRecordItem(32, 20, 22, new string[4] { "Jiao1Name", "PropertyType", "Float", "" }));
		_dataArray.Add(new JiaoRecordItem(33, 4, 37, new string[4] { "Jiao1Name", "Integer", "", "" }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<JiaoRecordItem>(34);
		CreateItems0();
	}
}
