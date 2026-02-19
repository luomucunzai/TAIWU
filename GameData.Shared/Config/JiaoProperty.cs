using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class JiaoProperty : ConfigData<JiaoPropertyItem, short>
{
	public static class DefKey
	{
		public const short TravelTimeReduction = 0;

		public const short BaseMaxInventoryLoadBonus = 1;

		public const short BaseDropRateBonus = 2;

		public const short BaseCaptureRateBonus = 3;

		public const short BaseMaxKidnapSlotCountBonus = 4;

		public const short ExploreBonusRate = 5;

		public const short BaseValue = 6;

		public const short BaseHappinessChange = 7;

		public const short BaseFavorabilityChange = 8;

		public const short JiaoLength = 9;

		public const short JiaoWeight = 10;

		public const short JiaoLongevity = 11;

		public const short BasePrice = 12;
	}

	public static class DefValue
	{
		public static JiaoPropertyItem TravelTimeReduction => Instance[(short)0];

		public static JiaoPropertyItem BaseMaxInventoryLoadBonus => Instance[(short)1];

		public static JiaoPropertyItem BaseDropRateBonus => Instance[(short)2];

		public static JiaoPropertyItem BaseCaptureRateBonus => Instance[(short)3];

		public static JiaoPropertyItem BaseMaxKidnapSlotCountBonus => Instance[(short)4];

		public static JiaoPropertyItem ExploreBonusRate => Instance[(short)5];

		public static JiaoPropertyItem BaseValue => Instance[(short)6];

		public static JiaoPropertyItem BaseHappinessChange => Instance[(short)7];

		public static JiaoPropertyItem BaseFavorabilityChange => Instance[(short)8];

		public static JiaoPropertyItem JiaoLength => Instance[(short)9];

		public static JiaoPropertyItem JiaoWeight => Instance[(short)10];

		public static JiaoPropertyItem JiaoLongevity => Instance[(short)11];

		public static JiaoPropertyItem BasePrice => Instance[(short)12];
	}

	public static JiaoProperty Instance = new JiaoProperty();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"JiaoRecordTemplateId", "JiaoNurturanceTemplateId", "TemplateId", "Name", "EventDescUp", "EventDescDown", "NeutralityPropertyParam", "ConservedPropertyParam", "ConservedTameParam", "MaxValue",
		"TipsIcon", "SpecialDescTitle", "SpecialDesc"
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
		_dataArray.Add(new JiaoPropertyItem(0, 0, 1, 2, new int[2] { 100, 200 }, 200, 200, -5, -15, 100, 75, 10, 60, 3, 1, "mousetip_travel", null, null));
		_dataArray.Add(new JiaoPropertyItem(1, 3, 4, 5, new int[2] { 45000, 90000 }, 90000, 200, -5, -15, 100, 75, 10, 20000, 2, 2, "mousetip_bag", null, null));
		_dataArray.Add(new JiaoPropertyItem(2, 6, 7, 8, new int[2] { 400, 800 }, 800, 200, -5, -15, 100, 75, 10, 150, 6, 4, "mousetip_dropped", null, null));
		_dataArray.Add(new JiaoPropertyItem(3, 9, 10, 11, new int[2] { 400, 800 }, 800, 200, -5, -15, 100, 75, 10, 150, 33, 10, "mousetip_detention_0", null, null));
		_dataArray.Add(new JiaoPropertyItem(4, 12, 13, 14, new int[2] { 50, 100 }, 100, 200, -5, -15, 100, 75, 10, 12, 4, 3, "mousetip_detention_1", null, null));
		_dataArray.Add(new JiaoPropertyItem(5, 15, 16, 17, new int[2] { 200, 400 }, 400, 200, -5, -15, 100, 75, 10, 60, 5, 5, "mousetip_fuyusword", null, null));
		_dataArray.Add(new JiaoPropertyItem(6, 18, 19, 20, new int[2] { 450000, 900000 }, 900000, 200, -5, -15, 100, 75, 10, 123000, 7, 6, "mousetip_jiage", "价格", "此蛟被售出时的价格"));
		_dataArray.Add(new JiaoPropertyItem(7, 21, 22, 23, new int[2] { 100, 200 }, 200, 200, -5, -15, 100, 75, 10, 36, 9, 8, "mousetip_mood", "赠礼心情", "此蛟作为礼物赠予时，对方的心情额外恢复"));
		_dataArray.Add(new JiaoPropertyItem(8, 24, 25, 26, new int[2] { 54000, 108000 }, 108000, 200, -5, -15, 100, 75, 10, 21600, 10, 9, "mousetip_opinion", "赠礼好感", "此蛟作为礼物赠予时，对方的好感额外增加"));
		_dataArray.Add(new JiaoPropertyItem(9, 27, 28, 29, new int[2] { 50, 100 }, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, null, null, null));
		_dataArray.Add(new JiaoPropertyItem(10, 30, 31, 32, new int[2] { 500, 1000 }, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, null, null, null));
		_dataArray.Add(new JiaoPropertyItem(11, 33, 34, 35, new int[2] { 5000, 10000 }, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, null, null, null));
		_dataArray.Add(new JiaoPropertyItem(12, 36, 37, 38, new int[2] { 450000, 900000 }, 900000, 200, -5, -15, 100, 75, 10, -1, 7, 6, "mousetip_jiage", "价格", "此蛟被售出时的价格"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<JiaoPropertyItem>(13);
		CreateItems0();
	}
}
