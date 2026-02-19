using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LifeSkill : ConfigData<LifeSkillItem, short>
{
	public static LifeSkill Instance = new LifeSkill();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "Type", "SkillBookId", "ProvidedReadingStrategies", "UnlockBuildingList", "TemplateId", "Name", "Desc", "UnlockInformationList" };

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
		_dataArray.Add(new LifeSkillItem(0, 0, 0, 1, 0, 1, 0, new List<byte> { 0, 1 }, 8, new List<ShortList>
		{
			new ShortList(86),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(1, 2, 1, 3, 0, 1, 1, new List<byte> { 0, 1 }, 12, new List<ShortList>
		{
			new ShortList(87),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(2, 4, 2, 5, 0, 2, 2, new List<byte> { 0, 1 }, 16, new List<ShortList>
		{
			new ShortList(88),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(3, 6, 3, 7, 0, 2, 3, new List<byte> { 0, 1 }, 20, new List<ShortList>
		{
			new ShortList(89),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(4, 8, 4, 9, 0, 3, 4, new List<byte> { 0, 1 }, 24, new List<ShortList>
		{
			new ShortList(90),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(5, 10, 5, 11, 0, 3, 5, new List<byte> { 0, 1 }, 28, new List<ShortList>
		{
			new ShortList(91),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(6, 12, 6, 13, 0, 4, 6, new List<byte> { 0, 1 }, 32, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(7, 14, 7, 15, 0, 4, 7, new List<byte> { 0, 1 }, 36, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(8, 16, 8, 17, 0, 5, 8, new List<byte> { 0, 1 }, 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(9, 18, 0, 19, 1, 1, 9, new List<byte> { 8, 9 }, 8, new List<ShortList>
		{
			new ShortList(93),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(10, 20, 1, 21, 1, 1, 10, new List<byte> { 8, 9 }, 12, new List<ShortList>
		{
			new ShortList(94),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(11, 22, 2, 23, 1, 2, 11, new List<byte> { 8, 9 }, 16, new List<ShortList>
		{
			new ShortList(95),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(12, 24, 3, 25, 1, 2, 12, new List<byte> { 8, 9 }, 20, new List<ShortList>
		{
			new ShortList(96),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(13, 26, 4, 27, 1, 3, 13, new List<byte> { 8, 9 }, 24, new List<ShortList>
		{
			new ShortList(97),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(14, 28, 5, 29, 1, 3, 14, new List<byte> { 8, 9 }, 28, new List<ShortList>
		{
			new ShortList(98),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(15, 30, 6, 31, 1, 4, 15, new List<byte> { 8, 9 }, 32, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(16, 32, 7, 33, 1, 4, 16, new List<byte> { 8, 9 }, 36, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(17, 34, 8, 35, 1, 5, 17, new List<byte> { 8, 9 }, 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(18, 36, 0, 37, 2, 1, 18, new List<byte> { 10, 11 }, 8, new List<ShortList>
		{
			new ShortList(100),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(19, 38, 1, 39, 2, 1, 19, new List<byte> { 10, 11 }, 12, new List<ShortList>
		{
			new ShortList(101),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(20, 40, 2, 41, 2, 2, 20, new List<byte> { 10, 11 }, 16, new List<ShortList>
		{
			new ShortList(102),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(21, 42, 3, 43, 2, 2, 21, new List<byte> { 10, 11 }, 20, new List<ShortList>
		{
			new ShortList(103),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(22, 44, 4, 45, 2, 3, 22, new List<byte> { 10, 11 }, 24, new List<ShortList>
		{
			new ShortList(104),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(23, 46, 5, 47, 2, 3, 23, new List<byte> { 10, 11 }, 28, new List<ShortList>
		{
			new ShortList(105),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(24, 48, 6, 49, 2, 4, 24, new List<byte> { 10, 11 }, 32, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(25, 50, 7, 51, 2, 4, 25, new List<byte> { 10, 11 }, 36, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(26, 52, 8, 53, 2, 5, 26, new List<byte> { 10, 11 }, 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(27, 54, 0, 55, 3, 1, 27, new List<byte> { 4, 5 }, 8, new List<ShortList>
		{
			new ShortList(107),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(28, 56, 1, 57, 3, 1, 28, new List<byte> { 4, 5 }, 12, new List<ShortList>
		{
			new ShortList(108),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(29, 58, 2, 59, 3, 2, 29, new List<byte> { 4, 5 }, 16, new List<ShortList>
		{
			new ShortList(109),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(30, 60, 3, 61, 3, 2, 30, new List<byte> { 4, 5 }, 20, new List<ShortList>
		{
			new ShortList(110),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(31, 62, 4, 63, 3, 3, 31, new List<byte> { 4, 5 }, 24, new List<ShortList>
		{
			new ShortList(111),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(32, 64, 5, 65, 3, 3, 32, new List<byte> { 4, 5 }, 28, new List<ShortList>
		{
			new ShortList(112),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(33, 66, 6, 67, 3, 4, 33, new List<byte> { 4, 5 }, 32, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(34, 68, 7, 69, 3, 4, 34, new List<byte> { 4, 5 }, 36, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(35, 70, 8, 71, 3, 5, 35, new List<byte> { 4, 5 }, 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(36, 72, 0, 73, 4, 1, 36, new List<byte> { 12, 13 }, 8, new List<ShortList>
		{
			new ShortList(114),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(37, 74, 1, 75, 4, 1, 37, new List<byte> { 12, 13 }, 12, new List<ShortList>
		{
			new ShortList(115),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(38, 76, 2, 77, 4, 2, 38, new List<byte> { 12, 13 }, 16, new List<ShortList>
		{
			new ShortList(116),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(39, 78, 3, 79, 4, 2, 39, new List<byte> { 12, 13 }, 20, new List<ShortList>
		{
			new ShortList(117),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(40, 80, 4, 81, 4, 3, 40, new List<byte> { 12, 13 }, 24, new List<ShortList>
		{
			new ShortList(118),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(41, 82, 5, 83, 4, 3, 41, new List<byte> { 12, 13 }, 28, new List<ShortList>
		{
			new ShortList(119),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(42, 84, 6, 85, 4, 4, 42, new List<byte> { 12, 13 }, 32, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(43, 86, 7, 87, 4, 4, 43, new List<byte> { 12, 13 }, 36, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(44, 88, 8, 89, 4, 5, 44, new List<byte> { 12, 13 }, 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(45, 90, 0, 91, 5, 1, 45, new List<byte> { 2, 3 }, 8, new List<ShortList>
		{
			new ShortList(121, 122),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(46, 92, 1, 93, 5, 1, 46, new List<byte> { 2, 3 }, 12, new List<ShortList>
		{
			new ShortList(123),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(47, 94, 2, 95, 5, 2, 47, new List<byte> { 2, 3 }, 16, new List<ShortList>
		{
			new ShortList(124),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(48, 96, 3, 97, 5, 2, 48, new List<byte> { 2, 3 }, 20, new List<ShortList>
		{
			new ShortList(125),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(49, 98, 4, 99, 5, 3, 49, new List<byte> { 2, 3 }, 24, new List<ShortList>
		{
			new ShortList(126),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(50, 100, 5, 101, 5, 3, 50, new List<byte> { 2, 3 }, 28, new List<ShortList>
		{
			new ShortList(127, 128),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(51, 102, 6, 103, 5, 4, 51, new List<byte> { 2, 3 }, 32, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(52, 104, 7, 105, 5, 4, 52, new List<byte> { 2, 3 }, 36, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(53, 106, 8, 107, 5, 5, 53, new List<byte> { 2, 3 }, 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(54, 108, 0, 109, 6, 1, 54, new List<byte>(), 8, new List<ShortList>
		{
			new ShortList(130),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(55, 110, 1, 111, 6, 1, 55, new List<byte>(), 12, new List<ShortList>
		{
			new ShortList(131),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(56, 112, 2, 113, 6, 2, 56, new List<byte>(), 16, new List<ShortList>
		{
			new ShortList(132),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(57, 114, 3, 115, 6, 2, 57, new List<byte>(), 20, new List<ShortList>
		{
			new ShortList(133),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(58, 116, 4, 117, 6, 3, 58, new List<byte>(), 24, new List<ShortList>
		{
			new ShortList(134),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(59, 118, 5, 119, 6, 3, 59, new List<byte>(), 28, new List<ShortList>
		{
			new ShortList(135, 136),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new LifeSkillItem(60, 120, 6, 121, 6, 4, 60, new List<byte>(), 32, new List<ShortList>
		{
			new ShortList(137),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(61, 122, 7, 123, 6, 4, 61, new List<byte>(), 36, new List<ShortList>
		{
			new ShortList(138),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(62, 124, 8, 125, 6, 5, 62, new List<byte>(), 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(63, 126, 0, 127, 7, 1, 63, new List<byte>(), 8, new List<ShortList>
		{
			new ShortList(140),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(64, 128, 1, 129, 7, 1, 64, new List<byte>(), 12, new List<ShortList>
		{
			new ShortList(141),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(65, 130, 2, 131, 7, 2, 65, new List<byte>(), 16, new List<ShortList>
		{
			new ShortList(142),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(66, 132, 3, 133, 7, 2, 66, new List<byte>(), 20, new List<ShortList>
		{
			new ShortList(143),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(67, 134, 4, 135, 7, 3, 67, new List<byte>(), 24, new List<ShortList>
		{
			new ShortList(144),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(68, 136, 5, 137, 7, 3, 68, new List<byte>(), 28, new List<ShortList>
		{
			new ShortList(145, 146),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(69, 138, 6, 139, 7, 4, 69, new List<byte>(), 32, new List<ShortList>
		{
			new ShortList(147),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(70, 140, 7, 141, 7, 4, 70, new List<byte>(), 36, new List<ShortList>
		{
			new ShortList(148),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(71, 142, 8, 143, 7, 5, 71, new List<byte>(), 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(72, 144, 0, 145, 8, 1, 72, new List<byte>(), 8, new List<ShortList>
		{
			new ShortList(150),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(73, 146, 1, 147, 8, 1, 73, new List<byte>(), 12, new List<ShortList>
		{
			new ShortList(151),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(74, 148, 2, 149, 8, 2, 74, new List<byte>(), 16, new List<ShortList>
		{
			new ShortList(152),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(75, 150, 3, 151, 8, 2, 75, new List<byte>(), 20, new List<ShortList>
		{
			new ShortList(153),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(76, 152, 4, 153, 8, 3, 76, new List<byte>(), 24, new List<ShortList>
		{
			new ShortList(154),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(77, 154, 5, 155, 8, 3, 77, new List<byte>(), 28, new List<ShortList>
		{
			new ShortList(155, 156),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(78, 156, 6, 157, 8, 4, 78, new List<byte>(), 32, new List<ShortList>
		{
			new ShortList(157),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(79, 158, 7, 159, 8, 4, 79, new List<byte>(), 36, new List<ShortList>
		{
			new ShortList(158),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(80, 160, 8, 161, 8, 5, 80, new List<byte>(), 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(81, 162, 0, 163, 9, 1, 81, new List<byte>(), 8, new List<ShortList>
		{
			new ShortList(160),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(82, 164, 1, 165, 9, 1, 82, new List<byte>(), 12, new List<ShortList>
		{
			new ShortList(161),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(83, 166, 2, 167, 9, 2, 83, new List<byte>(), 16, new List<ShortList>
		{
			new ShortList(162),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(84, 168, 3, 169, 9, 2, 84, new List<byte>(), 20, new List<ShortList>
		{
			new ShortList(163),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(85, 170, 4, 171, 9, 3, 85, new List<byte>(), 24, new List<ShortList>
		{
			new ShortList(164),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(86, 172, 5, 173, 9, 3, 86, new List<byte>(), 28, new List<ShortList>
		{
			new ShortList(165, 166),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(87, 174, 6, 175, 9, 4, 87, new List<byte>(), 32, new List<ShortList>
		{
			new ShortList(167),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(88, 176, 7, 177, 9, 4, 88, new List<byte>(), 36, new List<ShortList>
		{
			new ShortList(168),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(89, 178, 8, 179, 9, 5, 89, new List<byte>(), 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(90, 180, 0, 181, 10, 1, 90, new List<byte>(), 8, new List<ShortList>
		{
			new ShortList(170),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(91, 182, 1, 183, 10, 1, 91, new List<byte>(), 12, new List<ShortList>
		{
			new ShortList(171),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(92, 184, 2, 185, 10, 2, 92, new List<byte>(), 16, new List<ShortList>
		{
			new ShortList(172),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(93, 186, 3, 187, 10, 2, 93, new List<byte>(), 20, new List<ShortList>
		{
			new ShortList(173),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(94, 188, 4, 189, 10, 3, 94, new List<byte>(), 24, new List<ShortList>
		{
			new ShortList(174),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(95, 190, 5, 191, 10, 3, 95, new List<byte>(), 28, new List<ShortList>
		{
			new ShortList(175, 176),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(96, 192, 6, 193, 10, 4, 96, new List<byte>(), 32, new List<ShortList>
		{
			new ShortList(177),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(97, 194, 7, 195, 10, 4, 97, new List<byte>(), 36, new List<ShortList>
		{
			new ShortList(178),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(98, 196, 8, 197, 10, 5, 98, new List<byte>(), 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(99, 198, 0, 199, 11, 1, 99, new List<byte>(), 8, new List<ShortList>
		{
			new ShortList(180),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(100, 200, 1, 201, 11, 1, 100, new List<byte>(), 12, new List<ShortList>
		{
			new ShortList(181),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(101, 202, 2, 203, 11, 2, 101, new List<byte>(), 16, new List<ShortList>
		{
			new ShortList(182),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(102, 204, 3, 205, 11, 2, 102, new List<byte>(), 20, new List<ShortList>
		{
			new ShortList(183),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(103, 206, 4, 207, 11, 3, 103, new List<byte>(), 24, new List<ShortList>
		{
			new ShortList(184),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(104, 208, 5, 209, 11, 3, 104, new List<byte>(), 28, new List<ShortList>
		{
			new ShortList(185, 186),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(105, 210, 6, 211, 11, 4, 105, new List<byte>(), 32, new List<ShortList>
		{
			new ShortList(187),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(106, 212, 7, 213, 11, 4, 106, new List<byte>(), 36, new List<ShortList>
		{
			new ShortList(188),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(107, 214, 8, 215, 11, 5, 107, new List<byte>(), 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(108, 216, 0, 217, 12, 1, 108, new List<byte> { 14, 15 }, 8, new List<ShortList>
		{
			new ShortList(190),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(109, 218, 1, 219, 12, 1, 109, new List<byte> { 14, 15 }, 12, new List<ShortList>
		{
			new ShortList(191),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(110, 220, 2, 221, 12, 2, 110, new List<byte> { 14, 15 }, 16, new List<ShortList>
		{
			new ShortList(192),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(111, 222, 3, 223, 12, 2, 111, new List<byte> { 14, 15 }, 20, new List<ShortList>
		{
			new ShortList(193),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(112, 224, 4, 225, 12, 3, 112, new List<byte> { 14, 15 }, 24, new List<ShortList>
		{
			new ShortList(194),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(113, 226, 5, 227, 12, 3, 113, new List<byte> { 14, 15 }, 28, new List<ShortList>
		{
			new ShortList(195),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(114, 228, 6, 229, 12, 4, 114, new List<byte> { 14, 15 }, 32, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(115, 230, 7, 231, 12, 4, 115, new List<byte> { 14, 15 }, 36, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(116, 232, 8, 233, 12, 5, 116, new List<byte> { 14, 15 }, 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(117, 234, 0, 235, 13, 1, 117, new List<byte> { 16, 17 }, 8, new List<ShortList>
		{
			new ShortList(197),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(118, 236, 1, 237, 13, 1, 118, new List<byte> { 16, 17 }, 12, new List<ShortList>
		{
			new ShortList(198),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(119, 238, 2, 239, 13, 2, 119, new List<byte> { 16, 17 }, 16, new List<ShortList>
		{
			new ShortList(199),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new LifeSkillItem(120, 240, 3, 241, 13, 2, 120, new List<byte> { 16, 17 }, 20, new List<ShortList>
		{
			new ShortList(200),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(121, 242, 4, 243, 13, 3, 121, new List<byte> { 16, 17 }, 24, new List<ShortList>
		{
			new ShortList(201),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(122, 244, 5, 245, 13, 3, 122, new List<byte> { 16, 17 }, 28, new List<ShortList>
		{
			new ShortList(202),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(123, 246, 6, 247, 13, 4, 123, new List<byte> { 16, 17 }, 32, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(124, 248, 7, 249, 13, 4, 124, new List<byte> { 16, 17 }, 36, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(125, 250, 8, 251, 13, 5, 125, new List<byte> { 16, 17 }, 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(126, 252, 0, 253, 14, 1, 126, new List<byte>(), 8, new List<ShortList>
		{
			new ShortList(204),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(127, 254, 1, 255, 14, 1, 127, new List<byte>(), 12, new List<ShortList>
		{
			new ShortList(205),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(128, 256, 2, 257, 14, 2, 128, new List<byte>(), 16, new List<ShortList>
		{
			new ShortList(206),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(129, 258, 3, 259, 14, 2, 129, new List<byte>(), 20, new List<ShortList>
		{
			new ShortList(207),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(130, 260, 4, 261, 14, 3, 130, new List<byte>(), 24, new List<ShortList>
		{
			new ShortList(208),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(131, 262, 5, 263, 14, 3, 131, new List<byte>(), 28, new List<ShortList>
		{
			new ShortList(209, 210),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(132, 264, 6, 265, 14, 4, 132, new List<byte>(), 32, new List<ShortList>
		{
			new ShortList(211),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(133, 266, 7, 267, 14, 4, 133, new List<byte>(), 36, new List<ShortList>
		{
			new ShortList(212),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(134, 268, 8, 269, 14, 5, 134, new List<byte>(), 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(135, 270, 0, 271, 15, 1, 135, new List<byte> { 6, 7 }, 8, new List<ShortList>
		{
			new ShortList(214, 215),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(136, 272, 1, 273, 15, 1, 136, new List<byte> { 6, 7 }, 12, new List<ShortList>
		{
			new ShortList(216, 217),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(137, 274, 2, 275, 15, 2, 137, new List<byte> { 6, 7 }, 16, new List<ShortList>
		{
			new ShortList(218),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 0, 1 }));
		_dataArray.Add(new LifeSkillItem(138, 276, 3, 277, 15, 2, 138, new List<byte> { 6, 7 }, 20, new List<ShortList>
		{
			new ShortList(219),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(139, 278, 4, 279, 15, 3, 139, new List<byte> { 6, 7 }, 24, new List<ShortList>
		{
			new ShortList(220),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(140, 280, 5, 281, 15, 3, 140, new List<byte> { 6, 7 }, 28, new List<ShortList>
		{
			new ShortList(221),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 1, 0, 0, 0 }));
		_dataArray.Add(new LifeSkillItem(141, 282, 6, 283, 15, 4, 141, new List<byte> { 6, 7 }, 32, new List<ShortList>
		{
			new ShortList(222, 223),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(142, 284, 7, 285, 15, 4, 142, new List<byte> { 6, 7 }, 36, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
		_dataArray.Add(new LifeSkillItem(143, 286, 8, 287, 15, 5, 143, new List<byte> { 6, 7 }, 40, new List<ShortList>
		{
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1),
			new ShortList(-1)
		}, new sbyte[5] { 0, 0, 0, 1, 0 }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<LifeSkillItem>(144);
		CreateItems0();
		CreateItems1();
		CreateItems2();
	}
}
