using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class AdventureType : ConfigData<AdventureTypeItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte None = 0;

		public const sbyte MainStoryLine = 1;

		public const sbyte SectMainStoryLine = 2;

		public const sbyte Interaction = 3;

		public const sbyte HereticStronghold = 4;

		public const sbyte RighteousStronghold = 5;

		public const sbyte SeasonalEvent = 6;

		public const sbyte ContestForBride = 7;

		public const sbyte ContestForGroom = 8;

		public const sbyte MaterialResourceFood = 9;

		public const sbyte MaterialResourceWood = 10;

		public const sbyte MaterialResourceMetal = 11;

		public const sbyte MaterialRresourceJade = 12;

		public const sbyte MaterialResourceFabric = 13;

		public const sbyte MaterialResourceHerb = 14;

		public const sbyte SwordTomb = 15;

		public const sbyte InteractionOfLove = 16;

		public const sbyte LegendaryBook = 17;
	}

	public static class DefValue
	{
		public static AdventureTypeItem None => Instance[(sbyte)0];

		public static AdventureTypeItem MainStoryLine => Instance[(sbyte)1];

		public static AdventureTypeItem SectMainStoryLine => Instance[(sbyte)2];

		public static AdventureTypeItem Interaction => Instance[(sbyte)3];

		public static AdventureTypeItem HereticStronghold => Instance[(sbyte)4];

		public static AdventureTypeItem RighteousStronghold => Instance[(sbyte)5];

		public static AdventureTypeItem SeasonalEvent => Instance[(sbyte)6];

		public static AdventureTypeItem ContestForBride => Instance[(sbyte)7];

		public static AdventureTypeItem ContestForGroom => Instance[(sbyte)8];

		public static AdventureTypeItem MaterialResourceFood => Instance[(sbyte)9];

		public static AdventureTypeItem MaterialResourceWood => Instance[(sbyte)10];

		public static AdventureTypeItem MaterialResourceMetal => Instance[(sbyte)11];

		public static AdventureTypeItem MaterialRresourceJade => Instance[(sbyte)12];

		public static AdventureTypeItem MaterialResourceFabric => Instance[(sbyte)13];

		public static AdventureTypeItem MaterialResourceHerb => Instance[(sbyte)14];

		public static AdventureTypeItem SwordTomb => Instance[(sbyte)15];

		public static AdventureTypeItem InteractionOfLove => Instance[(sbyte)16];

		public static AdventureTypeItem LegendaryBook => Instance[(sbyte)17];
	}

	public static AdventureType Instance = new AdventureType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "DisplayName", "ColorName" };

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
		_dataArray.Add(new AdventureTypeItem(0, 0, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(1, 1, isTrivial: false, "mainstoryadventure"));
		_dataArray.Add(new AdventureTypeItem(2, 2, isTrivial: false, "sectstoryadventure"));
		_dataArray.Add(new AdventureTypeItem(3, 3, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(4, 4, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(5, 5, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(6, 6, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(7, 7, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(8, 7, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(9, 8, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(10, 8, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(11, 8, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(12, 8, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(13, 8, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(14, 8, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(15, 9, isTrivial: false, "swordtomb"));
		_dataArray.Add(new AdventureTypeItem(16, 10, isTrivial: true, "normaladventure"));
		_dataArray.Add(new AdventureTypeItem(17, 11, isTrivial: true, "normaladventure"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<AdventureTypeItem>(18);
		CreateItems0();
	}
}
