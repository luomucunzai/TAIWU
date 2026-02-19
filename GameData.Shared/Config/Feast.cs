using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class Feast : ConfigData<FeastItem, short>
{
	public static class DefKey
	{
		public const short Fruit = 0;

		public const short Vegetable = 1;

		public const short WhiteMeat = 2;

		public const short RedMeat = 3;

		public const short SeaFood = 4;

		public const short Tea = 5;

		public const short Wine = 6;

		public const short Mixed = 7;

		public const short HighestMixed = 8;

		public const short None = 9;
	}

	public static class DefValue
	{
		public static FeastItem Fruit => Instance[(short)0];

		public static FeastItem Vegetable => Instance[(short)1];

		public static FeastItem WhiteMeat => Instance[(short)2];

		public static FeastItem RedMeat => Instance[(short)3];

		public static FeastItem SeaFood => Instance[(short)4];

		public static FeastItem Tea => Instance[(short)5];

		public static FeastItem Wine => Instance[(short)6];

		public static FeastItem Mixed => Instance[(short)7];

		public static FeastItem HighestMixed => Instance[(short)8];

		public static FeastItem None => Instance[(short)9];
	}

	public static Feast Instance = new Feast();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "RequirementType", "TemplateId", "Name", "Desc", "ConditionDesc", "EffectDesc", "RequirementData" };

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
		_dataArray.Add(new FeastItem(0, 0, 1, 2, 3, 7, EFeastType.Fruit, 50, 100, 0, 100, 0, 0, 0, 0, ignoreHate: false, forceLove: false, new List<EFeastRequirementType> { EFeastRequirementType.FoodTypeFruit }, new List<int[]> { new int[2] { 3, 0 } }));
		_dataArray.Add(new FeastItem(1, 4, 5, 6, 7, 5, EFeastType.Vegetable, 50, 100, 0, 0, 100, 0, 0, 0, ignoreHate: false, forceLove: false, new List<EFeastRequirementType> { EFeastRequirementType.FoodTypeVegetarian }, new List<int[]> { new int[2] { 3, 0 } }));
		_dataArray.Add(new FeastItem(2, 8, 9, 10, 11, 7, EFeastType.WhiteMeat, 50, 100, 0, 0, 0, 500, 0, 0, ignoreHate: false, forceLove: false, new List<EFeastRequirementType> { EFeastRequirementType.FoodTypeBird }, new List<int[]> { new int[2] { 3, 0 } }));
		_dataArray.Add(new FeastItem(3, 12, 13, 14, 15, 7, EFeastType.RedMeat, 50, 100, 0, 0, 0, 0, 500, 0, ignoreHate: false, forceLove: false, new List<EFeastRequirementType> { EFeastRequirementType.FoodTypeBeast }, new List<int[]> { new int[2] { 3, 0 } }));
		_dataArray.Add(new FeastItem(4, 16, 17, 18, 19, 7, EFeastType.SeaFood, 50, 100, 0, 0, 0, 0, 0, 500, ignoreHate: false, forceLove: false, new List<EFeastRequirementType> { EFeastRequirementType.FoodTypeFish }, new List<int[]> { new int[2] { 3, 0 } }));
		_dataArray.Add(new FeastItem(5, 20, 21, 22, 23, 5, EFeastType.Tea, 0, 200, 0, 0, 0, 0, 0, 0, ignoreHate: false, forceLove: false, new List<EFeastRequirementType> { EFeastRequirementType.SubTypeTea }, new List<int[]> { new int[2] { 2, 0 } }));
		_dataArray.Add(new FeastItem(6, 24, 25, 26, 27, 5, EFeastType.Wine, 100, 0, 0, 0, 0, 0, 0, 0, ignoreHate: false, forceLove: false, new List<EFeastRequirementType> { EFeastRequirementType.SubTypeWine }, new List<int[]> { new int[2] { 2, 0 } }));
		_dataArray.Add(new FeastItem(7, 28, 29, 30, 31, 4, EFeastType.Mixed, 50, 50, 0, 0, 0, 0, 0, 0, ignoreHate: true, forceLove: false, new List<EFeastRequirementType> { EFeastRequirementType.SubTypeDiff }, new List<int[]> { new int[2] { 3, 0 } }));
		_dataArray.Add(new FeastItem(8, 32, 33, 34, 35, 8, EFeastType.HighestMixed, 150, 300, 0, 0, 0, 0, 0, 0, ignoreHate: true, forceLove: true, new List<EFeastRequirementType> { EFeastRequirementType.SubTypeDiff }, new List<int[]> { new int[2] { 3, 8 } }));
		_dataArray.Add(new FeastItem(9, 36, 37, 38, 39, 0, EFeastType.Invalid, 0, 0, 0, 0, 0, 0, 0, 0, ignoreHate: false, forceLove: false, null, null));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<FeastItem>(10);
		CreateItems0();
	}
}
