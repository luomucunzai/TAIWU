using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class Cricket : ConfigData<CricketItem, short>
{
	public static class DefKey
	{
		public const short Cricket0 = 0;

		public const short Cricket1 = 1;

		public const short Cricket2 = 2;

		public const short Cricket3 = 3;

		public const short Cricket4 = 4;

		public const short Cricket5 = 5;

		public const short Cricket6 = 6;

		public const short Cricket7 = 7;

		public const short Cricket8 = 8;
	}

	public static class DefValue
	{
		public static CricketItem Cricket0 => Instance[(short)0];

		public static CricketItem Cricket1 => Instance[(short)1];

		public static CricketItem Cricket2 => Instance[(short)2];

		public static CricketItem Cricket3 => Instance[(short)3];

		public static CricketItem Cricket4 => Instance[(short)4];

		public static CricketItem Cricket5 => Instance[(short)5];

		public static CricketItem Cricket6 => Instance[(short)6];

		public static CricketItem Cricket7 => Instance[(short)7];

		public static CricketItem Cricket8 => Instance[(short)8];
	}

	public static Cricket Instance = new Cricket();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "ItemSubType", "GroupId", "ResourceType", "TemplateId", "Name", "Grade", "Icon", "Desc" };

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
		_dataArray.Add(new CricketItem(0, 0, 11, 1100, 0, 0, "icon_Cricket_cuzhi_9", 1, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, 0, isSpecial: true, 0, 12));
		_dataArray.Add(new CricketItem(1, 0, 11, 1100, 1, 0, "icon_Cricket_cuzhi_8", 2, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, 0, isSpecial: true, 0, 12));
		_dataArray.Add(new CricketItem(2, 0, 11, 1100, 2, 0, "icon_Cricket_cuzhi_7", 3, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, 0, isSpecial: true, 0, 12));
		_dataArray.Add(new CricketItem(3, 0, 11, 1100, 3, 0, "icon_Cricket_cuzhi_6", 4, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 0, 0, 1, 0, 0, 8, allowRandomCreate: false, 0, isSpecial: true, 0, 12));
		_dataArray.Add(new CricketItem(4, 0, 11, 1100, 4, 0, "icon_Cricket_cuzhi_5", 5, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 0, 0, 2, 0, 0, 8, allowRandomCreate: false, 0, isSpecial: true, 0, 12));
		_dataArray.Add(new CricketItem(5, 0, 11, 1100, 5, 0, "icon_Cricket_cuzhi_4", 6, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 0, 0, 3, 0, 0, 8, allowRandomCreate: false, 0, isSpecial: true, 0, 12));
		_dataArray.Add(new CricketItem(6, 0, 11, 1100, 6, 0, "icon_Cricket_cuzhi_3", 7, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 0, 0, 4, 0, 0, 8, allowRandomCreate: false, 0, isSpecial: true, 0, 12));
		_dataArray.Add(new CricketItem(7, 0, 11, 1100, 7, 0, "icon_Cricket_cuzhi_2", 8, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 0, 0, 5, 0, 0, 8, allowRandomCreate: false, 0, isSpecial: true, 0, 12));
		_dataArray.Add(new CricketItem(8, 0, 11, 1100, 8, 0, "icon_Cricket_cuzhi_1", 9, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: false, repairable: false, inheritable: true, 0, 10, 0, 0, 6, 0, 0, 8, allowRandomCreate: false, 0, isSpecial: true, 0, 12));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CricketItem>(9);
		CreateItems0();
	}
}
