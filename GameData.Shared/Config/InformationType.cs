using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class InformationType : ConfigData<InformationTypeItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte Area = 0;

		public const sbyte Sect = 1;

		public const sbyte LifeSkill = 2;

		public const sbyte Western = 3;

		public const sbyte Scenic = 4;

		public const sbyte SwordTomb = 5;

		public const sbyte Profession = 6;
	}

	public static class DefValue
	{
		public static InformationTypeItem Area => Instance[(sbyte)0];

		public static InformationTypeItem Sect => Instance[(sbyte)1];

		public static InformationTypeItem LifeSkill => Instance[(sbyte)2];

		public static InformationTypeItem Western => Instance[(sbyte)3];

		public static InformationTypeItem Scenic => Instance[(sbyte)4];

		public static InformationTypeItem SwordTomb => Instance[(sbyte)5];

		public static InformationTypeItem Profession => Instance[(sbyte)6];
	}

	public static InformationType Instance = new InformationType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "DescGain", "DescEffect", "DescEffectWay" };

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
		_dataArray.Add(new InformationTypeItem(0, 0, 1, 2, 3, 4));
		_dataArray.Add(new InformationTypeItem(1, 5, 6, 7, 8, 4));
		_dataArray.Add(new InformationTypeItem(2, 9, 10, 11, 12, 4));
		_dataArray.Add(new InformationTypeItem(3, 13, 14, 15, 16, 4));
		_dataArray.Add(new InformationTypeItem(4, 17, 18, 18, 18, 18));
		_dataArray.Add(new InformationTypeItem(5, 19, 20, 21, 22, 4));
		_dataArray.Add(new InformationTypeItem(6, 23, 24, 25, 26, 4));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<InformationTypeItem>(7);
		CreateItems0();
	}
}
