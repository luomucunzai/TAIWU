using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class EventOptionConsumeType : ConfigData<EventOptionConsumeTypeItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte Food = 0;

		public const sbyte Wood = 1;

		public const sbyte Metal = 2;

		public const sbyte Jade = 3;

		public const sbyte Fabric = 4;

		public const sbyte Herb = 5;

		public const sbyte Money = 6;

		public const sbyte Authority = 7;

		public const sbyte ActionPoint = 8;

		public const sbyte SpiritualDebt = 9;

		public const sbyte SpiritualDebtInCurrentArea = 10;
	}

	public static class DefValue
	{
		public static EventOptionConsumeTypeItem Food => Instance[(sbyte)0];

		public static EventOptionConsumeTypeItem Wood => Instance[(sbyte)1];

		public static EventOptionConsumeTypeItem Metal => Instance[(sbyte)2];

		public static EventOptionConsumeTypeItem Jade => Instance[(sbyte)3];

		public static EventOptionConsumeTypeItem Fabric => Instance[(sbyte)4];

		public static EventOptionConsumeTypeItem Herb => Instance[(sbyte)5];

		public static EventOptionConsumeTypeItem Money => Instance[(sbyte)6];

		public static EventOptionConsumeTypeItem Authority => Instance[(sbyte)7];

		public static EventOptionConsumeTypeItem ActionPoint => Instance[(sbyte)8];

		public static EventOptionConsumeTypeItem SpiritualDebt => Instance[(sbyte)9];

		public static EventOptionConsumeTypeItem SpiritualDebtInCurrentArea => Instance[(sbyte)10];
	}

	public static EventOptionConsumeType Instance = new EventOptionConsumeType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId" };

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
		_dataArray.Add(new EventOptionConsumeTypeItem(0));
		_dataArray.Add(new EventOptionConsumeTypeItem(1));
		_dataArray.Add(new EventOptionConsumeTypeItem(2));
		_dataArray.Add(new EventOptionConsumeTypeItem(3));
		_dataArray.Add(new EventOptionConsumeTypeItem(4));
		_dataArray.Add(new EventOptionConsumeTypeItem(5));
		_dataArray.Add(new EventOptionConsumeTypeItem(6));
		_dataArray.Add(new EventOptionConsumeTypeItem(7));
		_dataArray.Add(new EventOptionConsumeTypeItem(8));
		_dataArray.Add(new EventOptionConsumeTypeItem(9));
		_dataArray.Add(new EventOptionConsumeTypeItem(10));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<EventOptionConsumeTypeItem>(11);
		CreateItems0();
	}
}
