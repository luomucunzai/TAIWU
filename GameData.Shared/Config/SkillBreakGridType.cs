using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SkillBreakGridType : ConfigData<SkillBreakGridTypeItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte StartPoint = 0;

		public const sbyte EndPoint = 1;

		public const sbyte Bonus = 2;

		public const sbyte Normal = 3;

		public const sbyte Portal = 21;

		public const sbyte PrevGood = 22;

		public const sbyte PrevBad = 23;
	}

	public static class DefValue
	{
		public static SkillBreakGridTypeItem StartPoint => Instance[(sbyte)0];

		public static SkillBreakGridTypeItem EndPoint => Instance[(sbyte)1];

		public static SkillBreakGridTypeItem Bonus => Instance[(sbyte)2];

		public static SkillBreakGridTypeItem Normal => Instance[(sbyte)3];

		public static SkillBreakGridTypeItem Portal => Instance[(sbyte)21];

		public static SkillBreakGridTypeItem PrevGood => Instance[(sbyte)22];

		public static SkillBreakGridTypeItem PrevBad => Instance[(sbyte)23];
	}

	public static SkillBreakGridType Instance = new SkillBreakGridType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Type", "Desc", "FontColor" };

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
		_dataArray.Add(new SkillBreakGridTypeItem(0, 0, ESkillBreakGridTypeType.StartPoint, 1, "darkbrown", 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: true, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new SkillBreakGridTypeItem(1, 2, ESkillBreakGridTypeType.EndPoint, 3, "darkbrown", 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: true, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new SkillBreakGridTypeItem(2, 4, ESkillBreakGridTypeType.Bonus, 5, "yellow", 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: true, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new SkillBreakGridTypeItem(3, 6, ESkillBreakGridTypeType.Normal, 7, "lightgrey", 0, 1, 0, -1, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new SkillBreakGridTypeItem(4, 8, ESkillBreakGridTypeType.Special, 9, "darkpurple", 0, 1, 0, -1, 0, 0, 30, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 15));
		_dataArray.Add(new SkillBreakGridTypeItem(5, 10, ESkillBreakGridTypeType.Special, 11, "darkpurple", 0, 1, 0, -1, 0, 0, 0, 0, 0, 0, 0, 2, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 3));
		_dataArray.Add(new SkillBreakGridTypeItem(6, 12, ESkillBreakGridTypeType.Special, 13, "darkpurple", 0, 1, 0, -1, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: true, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 3));
		_dataArray.Add(new SkillBreakGridTypeItem(7, 14, ESkillBreakGridTypeType.Special, 15, "darkpurple", 0, 1, 0, -1, -100, 60, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 15));
		_dataArray.Add(new SkillBreakGridTypeItem(8, 16, ESkillBreakGridTypeType.Special, 17, "darkpurple", 0, 1, 0, -1, 0, 0, 0, 0, 0, 0, 100, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 3));
		_dataArray.Add(new SkillBreakGridTypeItem(9, 18, ESkillBreakGridTypeType.Special, 19, "darkpurple", 0, 0, 0, -1, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 3));
		_dataArray.Add(new SkillBreakGridTypeItem(10, 20, ESkillBreakGridTypeType.Special, 21, "darkpurple", 0, 0, 0, -1, -30, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: true, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 3));
		_dataArray.Add(new SkillBreakGridTypeItem(11, 22, ESkillBreakGridTypeType.Special, 23, "darkpurple", 0, 1, 3, -1, -60, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 3));
		_dataArray.Add(new SkillBreakGridTypeItem(12, 24, ESkillBreakGridTypeType.Special, 25, "darkpurple", 0, 1, 0, -1, -60, 0, 0, 20, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 1, 0, 0, 1, 15));
		_dataArray.Add(new SkillBreakGridTypeItem(13, 26, ESkillBreakGridTypeType.Special, 27, "darkpurple", 0, 1, 0, -1, 0, 0, -50, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: true, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 2, 0, 0, 0, 1, 15));
		_dataArray.Add(new SkillBreakGridTypeItem(14, 28, ESkillBreakGridTypeType.Special, 29, "darkpurple", 10, 1, 0, -1, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 3));
		_dataArray.Add(new SkillBreakGridTypeItem(15, 30, ESkillBreakGridTypeType.Special, 31, "darkpurple", 0, 1, 0, -1, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 3, 0, 1, 3));
		_dataArray.Add(new SkillBreakGridTypeItem(16, 32, ESkillBreakGridTypeType.Special, 33, "darkpurple", 0, 1, 0, -1, 30, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: true, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 3));
		_dataArray.Add(new SkillBreakGridTypeItem(17, 34, ESkillBreakGridTypeType.Special, 35, "darkpurple", 0, 1, 0, -1, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 1, 0, 0, 0, 1, 15));
		_dataArray.Add(new SkillBreakGridTypeItem(18, 36, ESkillBreakGridTypeType.Special, 37, "darkpurple", 0, 1, 3, -1, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: true, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 15));
		_dataArray.Add(new SkillBreakGridTypeItem(19, 38, ESkillBreakGridTypeType.Special, 39, "darkpurple", 0, 1, 0, -1, 0, 0, 0, 0, 5, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 15));
		_dataArray.Add(new SkillBreakGridTypeItem(20, 40, ESkillBreakGridTypeType.Special, 41, "darkpurple", 0, 1, 0, -1, 0, 0, 0, 0, 0, 5, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 1, 15));
		_dataArray.Add(new SkillBreakGridTypeItem(21, 42, ESkillBreakGridTypeType.Special, 43, "darkpurple", 0, 1, 0, -1, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: true, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 2, 3));
		_dataArray.Add(new SkillBreakGridTypeItem(22, 44, ESkillBreakGridTypeType.Normal, 45, "brightblue", 0, 1, 0, 100, 0, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 0, 0, 0));
		_dataArray.Add(new SkillBreakGridTypeItem(23, 46, ESkillBreakGridTypeType.Normal, 47, "brightred", 0, 1, 0, -1, 100, 0, 0, 0, 0, 0, 0, 1, nextStepCanJumpToSame: false, neighborFailedToCanSelect: false, randomNeighborNormalConvertToSameGrid: false, allNeighborNormalConvertToSpecialGrid: false, allNeighborSpecialConvertToNormalGrid: false, clearNeighborMaxPower: false, ignoreEffectAddMaxPower: false, 0, 0, 0, 6, 0, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SkillBreakGridTypeItem>(24);
		CreateItems0();
	}
}
