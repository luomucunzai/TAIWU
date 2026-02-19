using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells.Character;

namespace Config;

[Serializable]
public class SkillBreakPlateGridBonusType : ConfigData<SkillBreakPlateGridBonusTypeItem, short>
{
	public static SkillBreakPlateGridBonusType Instance = new SkillBreakPlateGridBonusType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "ExtraBonusFitCombatSkillTypes", "ExtraBonusFitLifeSkillTypes", "ExtraBonusFitItemSubTypes", "CharacterPropertyBonusList", "CombatSkillPropertyBonusList", "TemplateId", "Name", "Desc" };

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
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(0, 0, 1, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(6, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(1, 2, 3, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(7, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(2, 4, 5, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(8, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(3, 6, 7, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(9, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(4, 8, 9, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(10, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(5, 10, 11, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(11, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(6, 12, 13, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(12, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(7, 14, 15, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(13, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(8, 16, 17, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(14, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(9, 18, 19, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(15, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(10, 20, 21, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(16, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(11, 22, 23, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(17, 10)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(12, 24, 25, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(18, 5)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(13, 26, 27, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(19, 5)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(14, 28, 29, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(20, 5)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(15, 30, 31, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(21, 5)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(16, 32, 33, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(22, 5)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(17, 34, 35, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(23, 5)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(18, 36, 37, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(24, 5)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(19, 38, 39, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(25, 5)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(20, 40, 41, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(26, 5)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(21, 42, 43, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(27, 5)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(22, 44, 45, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(28, 3)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(23, 46, 47, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(29, 3)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(24, 48, 49, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(30, 3)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(25, 50, 51, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(31, 3)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(26, 52, 53, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(32, 3)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(27, 54, 55, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(33, 3)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(28, 56, 57, isExtraBonus: true, new sbyte[1], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(101, 50)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.NeigongAndPassiveSkill, 1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(29, 58, 59, isExtraBonus: true, new sbyte[1], new sbyte[0], new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(102, 20)
		}, new PropertyAndValue[0], ESkillBreakPlateGridBonusTypeAppearType.NeigongAndPassiveSkill, 1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(30, 60, 61, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(0, 5)
		}, ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(31, 62, 63, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(1, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(32, 64, 65, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(2, -10)
		}, ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(33, 66, 67, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(3, -5)
		}, ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(34, 68, 69, isExtraBonus: true, new sbyte[1], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(4, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.NeigongAndPassiveSkillExclude, 0));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(35, 70, 71, isExtraBonus: true, new sbyte[1], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(5, 5)
		}, ESkillBreakPlateGridBonusTypeAppearType.NeigongAndPassiveSkillExclude, 0));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(36, 72, 73, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(6, 30)
		}, ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(37, 74, 75, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(7, 3)
		}, ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(38, 76, 77, isExtraBonus: true, new sbyte[1], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(8, 1)
		}, ESkillBreakPlateGridBonusTypeAppearType.NeigongOnlyExcludeMix, 1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(39, 78, 79, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(9, 1)
		}, ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(40, 80, 81, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(10, -3)
		}, ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(41, 82, 83, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(11, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(42, 84, 85, isExtraBonus: true, new sbyte[1] { 1 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(12, 15),
			new PropertyAndValue(10, 5)
		}, ESkillBreakPlateGridBonusTypeAppearType.PosingAndStrength, 3));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(43, 86, 87, isExtraBonus: true, new sbyte[1] { 1 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(13, 15),
			new PropertyAndValue(10, 5)
		}, ESkillBreakPlateGridBonusTypeAppearType.PosingAndTechnique, 3));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(44, 88, 89, isExtraBonus: true, new sbyte[1] { 1 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(14, 15),
			new PropertyAndValue(10, 5)
		}, ESkillBreakPlateGridBonusTypeAppearType.PosingAndSpeed, 3));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(45, 90, 91, isExtraBonus: true, new sbyte[1] { 1 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(15, 15),
			new PropertyAndValue(10, 5)
		}, ESkillBreakPlateGridBonusTypeAppearType.PosingAndMind, 3));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(46, 92, 93, isExtraBonus: true, new sbyte[1] { 1 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(16, -15)
		}, ESkillBreakPlateGridBonusTypeAppearType.PosingAndSpeedCost, 3));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(47, 94, 95, isExtraBonus: true, new sbyte[1] { 1 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(17, -15)
		}, ESkillBreakPlateGridBonusTypeAppearType.PosingAndMoveCost, 3));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(48, 96, 97, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(18, 15),
			new PropertyAndValue(3, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.DefendSkillAndOuterDef, 4));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(49, 98, 99, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(19, 15),
			new PropertyAndValue(3, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.DefendSkillAndInnerDef, 4));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(50, 100, 101, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(20, 15),
			new PropertyAndValue(3, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.DefendSkillAndAvoidStrength, 4));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(51, 102, 103, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(21, 15),
			new PropertyAndValue(3, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.DefendSkillAndAvoidTechnique, 4));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(52, 104, 105, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(22, 15),
			new PropertyAndValue(3, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.DefendSkillAndAvoidSpeed, 4));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(53, 106, 107, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(23, 15),
			new PropertyAndValue(3, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.DefendSkillAndAvoidMind, 4));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(54, 108, 109, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(24, 15),
			new PropertyAndValue(3, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.DefendSkillAndFightbackPower, 4));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(55, 110, 111, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(25, 15),
			new PropertyAndValue(3, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.DefendSkillAndBouncePowerOuter, 4));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(56, 112, 113, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(26, 15),
			new PropertyAndValue(3, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.DefendSkillAndBouncePowerInner, 4));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(57, 114, 115, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(27, 5),
			new PropertyAndValue(3, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.DefendSkillAndBounceDistance, 4));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(58, 116, 117, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(28, 25),
			new PropertyAndValue(3, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.DefendSkillOnly, 4));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(59, 118, 119, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(29, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(60, 120, 121, isExtraBonus: false, new sbyte[0], new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(30, 10)
		}, ESkillBreakPlateGridBonusTypeAppearType.Never, -1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(61, 122, 123, isExtraBonus: true, new sbyte[1] { 8 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(31, 10),
			new PropertyAndValue(32, -10),
			new PropertyAndValue(30, -20)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTechniqueHitDistribution, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(62, 124, 125, isExtraBonus: true, new sbyte[1] { 3 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(31, 10),
			new PropertyAndValue(33, -10),
			new PropertyAndValue(30, -20)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndSpeedHitDistribution, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(63, 126, 127, isExtraBonus: true, new sbyte[1] { 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(32, 10),
			new PropertyAndValue(31, -10),
			new PropertyAndValue(30, -20)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndStrengthHitDistribution, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(64, 128, 129, isExtraBonus: true, new sbyte[1] { 9 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(32, 10),
			new PropertyAndValue(33, -10),
			new PropertyAndValue(30, -20)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndSpeedHitDistribution, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(65, 130, 131, isExtraBonus: true, new sbyte[1] { 7 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(33, 10),
			new PropertyAndValue(31, -10),
			new PropertyAndValue(30, -20)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndStrengthHitDistribution, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(66, 132, 133, isExtraBonus: true, new sbyte[1] { 6 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(33, 10),
			new PropertyAndValue(32, -10),
			new PropertyAndValue(30, -20)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTechniqueHitDistribution, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(67, 134, 135, isExtraBonus: true, new sbyte[1] { 5 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(34, 5),
			new PropertyAndValue(29, -20)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillOnly, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(68, 136, 137, isExtraBonus: true, new sbyte[1] { 3 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(35, 300),
			new PropertyAndValue(34, -5)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndHitChest, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(69, 138, 139, isExtraBonus: true, new sbyte[1] { 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(36, 300),
			new PropertyAndValue(34, -5)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndHitBelly, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(70, 140, 141, isExtraBonus: true, new sbyte[1] { 7 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(37, 3000),
			new PropertyAndValue(34, -5)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndHitHead, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(71, 142, 143, isExtraBonus: true, new sbyte[1] { 9 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(38, 300),
			new PropertyAndValue(34, -5)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndHitBothHands, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(72, 144, 145, isExtraBonus: true, new sbyte[1] { 8 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(39, 300),
			new PropertyAndValue(34, -5)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndHitBothLegs, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(73, 146, 147, isExtraBonus: true, new sbyte[1] { 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(40, 1),
			new PropertyAndValue(2, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndHasAtkAcupointEffect, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(74, 148, 149, isExtraBonus: true, new sbyte[1] { 3 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(41, 1),
			new PropertyAndValue(2, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndHasAtkFlawEffect, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(75, 150, 151, isExtraBonus: true, new sbyte[0], new sbyte[1] { 9 }, new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(28, -100)
		}, new PropertyAndValue[1]
		{
			new PropertyAndValue(42, 50)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndHotPoison, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(76, 152, 153, isExtraBonus: true, new sbyte[0], new sbyte[1] { 9 }, new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(29, -100)
		}, new PropertyAndValue[1]
		{
			new PropertyAndValue(43, 50)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndGloomyPoison, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(77, 154, 155, isExtraBonus: true, new sbyte[0], new sbyte[1] { 9 }, new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(30, -100)
		}, new PropertyAndValue[1]
		{
			new PropertyAndValue(44, 50)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndColdPoison, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(78, 156, 157, isExtraBonus: true, new sbyte[0], new sbyte[1] { 9 }, new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(31, -100)
		}, new PropertyAndValue[1]
		{
			new PropertyAndValue(45, 50)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndRedPoison, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(79, 158, 159, isExtraBonus: true, new sbyte[0], new sbyte[1] { 9 }, new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(32, -100)
		}, new PropertyAndValue[1]
		{
			new PropertyAndValue(46, 50)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndRottenPoison, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(80, 160, 161, isExtraBonus: true, new sbyte[0], new sbyte[1] { 9 }, new short[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(33, -100)
		}, new PropertyAndValue[1]
		{
			new PropertyAndValue(47, 50)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndIllusoryPoison, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(81, 162, 163, isExtraBonus: true, new sbyte[1] { 2 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(48, -10)
		}, ESkillBreakPlateGridBonusTypeAppearType.NoLimit, 0));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(82, 164, 165, isExtraBonus: true, new sbyte[1], new sbyte[0], new short[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(18, -25),
			new PropertyAndValue(19, -25)
		}, new PropertyAndValue[1]
		{
			new PropertyAndValue(49, 1)
		}, ESkillBreakPlateGridBonusTypeAppearType.NeigongOnly, 1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(83, 166, 167, isExtraBonus: true, new sbyte[1], new sbyte[0], new short[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(20, -25),
			new PropertyAndValue(21, -25)
		}, new PropertyAndValue[1]
		{
			new PropertyAndValue(50, 1)
		}, ESkillBreakPlateGridBonusTypeAppearType.NeigongOnly, 1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(84, 168, 169, isExtraBonus: true, new sbyte[1], new sbyte[0], new short[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(24, -25),
			new PropertyAndValue(25, -25)
		}, new PropertyAndValue[1]
		{
			new PropertyAndValue(51, 1)
		}, ESkillBreakPlateGridBonusTypeAppearType.NeigongOnly, 1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(85, 170, 171, isExtraBonus: true, new sbyte[1], new sbyte[0], new short[0], new PropertyAndValue[2]
		{
			new PropertyAndValue(22, -25),
			new PropertyAndValue(23, -25)
		}, new PropertyAndValue[1]
		{
			new PropertyAndValue(52, 1)
		}, ESkillBreakPlateGridBonusTypeAppearType.NeigongOnly, 1));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(86, 172, 173, isExtraBonus: true, new sbyte[1] { 6 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(53, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(87, 174, 175, isExtraBonus: true, new sbyte[1] { 6 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(54, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(88, 176, 177, isExtraBonus: true, new sbyte[1] { 6 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(55, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(89, 178, 179, isExtraBonus: true, new sbyte[3] { 8, 7, 9 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(56, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(90, 180, 181, isExtraBonus: true, new sbyte[3] { 8, 7, 9 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(57, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(91, 182, 183, isExtraBonus: true, new sbyte[3] { 8, 7, 9 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(58, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(92, 184, 185, isExtraBonus: true, new sbyte[2] { 3, 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(59, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(93, 186, 187, isExtraBonus: true, new sbyte[2] { 3, 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(60, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(94, 188, 189, isExtraBonus: true, new sbyte[2] { 3, 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(61, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(95, 190, 191, isExtraBonus: true, new sbyte[1] { 13 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(62, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(96, 192, 193, isExtraBonus: true, new sbyte[1] { 11 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(63, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(97, 194, 195, isExtraBonus: true, new sbyte[1] { 10 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(64, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(98, 196, 197, isExtraBonus: true, new sbyte[1] { 12 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(65, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(99, 198, 199, isExtraBonus: true, new sbyte[0], new sbyte[1] { 8 }, new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(66, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(100, 200, 201, isExtraBonus: true, new sbyte[0], new sbyte[1] { 9 }, new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(67, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(101, 202, 203, isExtraBonus: true, new sbyte[1] { 11 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(68, -1),
			new PropertyAndValue(70, 33),
			new PropertyAndValue(71, 600)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCost, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(102, 204, 205, isExtraBonus: true, new sbyte[1] { 1 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[1]
		{
			new PropertyAndValue(69, 20)
		}, ESkillBreakPlateGridBonusTypeAppearType.PosingAndJumpPrepareFrame, 3));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(103, 206, 207, isExtraBonus: true, new sbyte[1] { 6 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(53, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(104, 208, 209, isExtraBonus: true, new sbyte[1] { 6 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(54, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(105, 210, 211, isExtraBonus: true, new sbyte[1] { 6 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(55, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(106, 212, 213, isExtraBonus: true, new sbyte[3] { 8, 7, 9 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(56, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(107, 214, 215, isExtraBonus: true, new sbyte[3] { 8, 7, 9 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(57, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(108, 216, 217, isExtraBonus: true, new sbyte[3] { 8, 7, 9 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(58, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(109, 218, 219, isExtraBonus: true, new sbyte[2] { 3, 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(59, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(110, 220, 221, isExtraBonus: true, new sbyte[2] { 3, 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(60, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(111, 222, 223, isExtraBonus: true, new sbyte[2] { 3, 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(61, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(112, 224, 225, isExtraBonus: true, new sbyte[1] { 11 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(63, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(113, 226, 227, isExtraBonus: true, new sbyte[1] { 12 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(65, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(114, 228, 229, isExtraBonus: true, new sbyte[0], new sbyte[1] { 8 }, new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(66, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(115, 230, 231, isExtraBonus: true, new sbyte[0], new sbyte[1] { 9 }, new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(67, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(116, 232, 233, isExtraBonus: true, new sbyte[1] { 11 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(68, 1),
			new PropertyAndValue(29, 20),
			new PropertyAndValue(72, 100)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(117, 234, 235, isExtraBonus: true, new sbyte[1] { 6 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(53, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(118, 236, 237, isExtraBonus: true, new sbyte[1] { 6 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(54, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(119, 238, 239, isExtraBonus: true, new sbyte[1] { 6 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(55, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(120, 240, 241, isExtraBonus: true, new sbyte[3] { 8, 7, 9 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(56, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(121, 242, 243, isExtraBonus: true, new sbyte[3] { 8, 7, 9 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(57, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(122, 244, 245, isExtraBonus: true, new sbyte[3] { 8, 7, 9 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(58, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(123, 246, 247, isExtraBonus: true, new sbyte[2] { 3, 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(59, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(124, 248, 249, isExtraBonus: true, new sbyte[2] { 3, 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(60, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(125, 250, 251, isExtraBonus: true, new sbyte[2] { 3, 4 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(61, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(126, 252, 253, isExtraBonus: true, new sbyte[1] { 13 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(62, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 50)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(127, 254, 255, isExtraBonus: true, new sbyte[1] { 11 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(63, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(128, 256, 257, isExtraBonus: true, new sbyte[1] { 10 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(64, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 50)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(129, 258, 259, isExtraBonus: true, new sbyte[1] { 12 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(65, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(130, 260, 261, isExtraBonus: true, new sbyte[0], new sbyte[1] { 8 }, new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(66, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(131, 262, 263, isExtraBonus: true, new sbyte[0], new sbyte[1] { 9 }, new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(67, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
		_dataArray.Add(new SkillBreakPlateGridBonusTypeItem(132, 264, 265, isExtraBonus: true, new sbyte[1] { 11 }, new sbyte[0], new short[0], new PropertyAndValue[0], new PropertyAndValue[3]
		{
			new PropertyAndValue(68, 1),
			new PropertyAndValue(30, 20),
			new PropertyAndValue(73, 25)
		}, ESkillBreakPlateGridBonusTypeAppearType.AttackSkillAndTrickCostExist, 2));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SkillBreakPlateGridBonusTypeItem>(133);
		CreateItems0();
		CreateItems1();
		CreateItems2();
	}
}
