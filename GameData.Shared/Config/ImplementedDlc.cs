using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class ImplementedDlc : ConfigData<ImplementedDlcItem, byte>
{
	public static class DefKey
	{
		public const byte InteractOfLove = 0;

		public const byte GiftFromConchShip1 = 1;

		public const byte GiftFromConchShip2 = 2;

		public const byte FiveLoong = 3;

		public const byte HappyNewYear2024 = 4;

		public const byte YearOfSnakeCloth = 5;

		public const byte HappyNewYear2026 = 6;
	}

	public static class DefValue
	{
		public static ImplementedDlcItem InteractOfLove => Instance[(byte)0];

		public static ImplementedDlcItem GiftFromConchShip1 => Instance[(byte)1];

		public static ImplementedDlcItem GiftFromConchShip2 => Instance[(byte)2];

		public static ImplementedDlcItem FiveLoong => Instance[(byte)3];

		public static ImplementedDlcItem HappyNewYear2024 => Instance[(byte)4];

		public static ImplementedDlcItem YearOfSnakeCloth => Instance[(byte)5];

		public static ImplementedDlcItem HappyNewYear2026 => Instance[(byte)6];
	}

	public static ImplementedDlc Instance = new ImplementedDlc();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name" };

	internal override int ToInt(byte value)
	{
		return value;
	}

	internal override byte ToTemplateId(int value)
	{
		return (byte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new ImplementedDlcItem(0, 2305890u, "InteractOfLove"));
		_dataArray.Add(new ImplementedDlcItem(1, 2241120u, "GiftFromConchShip1"));
		_dataArray.Add(new ImplementedDlcItem(2, 2172690u, "GiftFromConchShip2"));
		_dataArray.Add(new ImplementedDlcItem(3, 2764950u, "FiveLoong"));
		_dataArray.Add(new ImplementedDlcItem(4, 2764960u, "HappyNewYear2024"));
		_dataArray.Add(new ImplementedDlcItem(5, 3464590u, "YearOfSnakeCloth"));
		_dataArray.Add(new ImplementedDlcItem(6, 4395170u, "HappyNewYear2026"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ImplementedDlcItem>(7);
		CreateItems0();
	}
}
