using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SecretInformationAppliedResult : ConfigData<SecretInformationAppliedResultItem, short>
{
	public static SecretInformationAppliedResult Instance = new SecretInformationAppliedResult();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "InnerResultEvent", "SelectionIds", "SecretInformation", "CombatConfigId", "SpecialConditionId", "SpecialConditionResultIds", "TemplateId", "ResultEventGuid", "Texts" };

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
		_dataArray.Add(new SecretInformationAppliedResultItem(0, -1, "05e87c45-f14e-49ef-8769-cbaced4753ae", endEventAfterJump: true, revealCharacters: false, new int[5] { 0, 1, 2, 3, 4 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(1, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 5, 6, 7, 8, 9 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 3, 0, 1600, -1, -1, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(2, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 10, 11, 12, 13, 14 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, -3, 0, -3200, 1, 1, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(3, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 15, 16, 17, 18, 19 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 3, 0, 1600, -2, -2, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(4, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 20, 21, 22, 23, 24 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, -3, 0, -3200, 2, 2, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(5, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 25, 26, 27, 28, 29 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 3, 0, 1600, 3, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(6, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 30, 31, 32, 33, 34 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, -3, 0, -3200, 3, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(7, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 35, 36, 37, 38, 39 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 3, 0, 1600, -1, -1, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(8, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 40, 41, 42, 43, 44 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, -3, 0, -3200, 1, 1, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(9, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 45, 46, 47, 48, 49 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, -3, -1600, -1600, -2, -2, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(10, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 50, 51, 52, 53, 54 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, -3, -3200, -3200, 2, 2, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(11, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 55, 56, 57, 58, 59 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 38, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 10, 0, 6000, 3, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(12, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 60, 61, 62, 63, 64 }, new short[1] { 41 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, -10, 0, -12000, 1, 1, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(13, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 65, 66, 67, 68, 69 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 39, new List<ShortList>
		{
			new ShortList(-1)
		}, 10, 0, 6000, 0, 3, 3, isFavorabilityCost: true));
		_dataArray.Add(new SecretInformationAppliedResultItem(14, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 70, 71, 72, 73, 74 }, new short[2] { 40, 0 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -10, 0, -12000, 0, 1, 1, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(15, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 75, 76, 77, 78, 79 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: false, 17, new List<ShortList>
		{
			new ShortList(18, 19, 30, 31, 27, 23, 28, 19, 18),
			new ShortList(69)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(16, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 80, 81, 82, 83, 84 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 2, noGuard: false, 17, new List<ShortList>
		{
			new ShortList(20, 24, 30, 31, 27, 21, 29, 19, 18),
			new ShortList(67)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(17, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 85, 86, 87, 88, 89 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 2, noGuard: false, 17, new List<ShortList>
		{
			new ShortList(22, 25, 30, 31, 27, 23, 28, 19, 18),
			new ShortList(68)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(18, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 90, 91, 92, 93, 94 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -3000, -6000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(19, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 95, 96, 97, 98, 99 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -6000, -3000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(20, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 100, 101, 102, 103, 104 }, new short[2] { 69, 47 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(21, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 100, 105, 106, 107, 108 }, new short[2] { 68, 70 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 5, -5, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(22, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 100, 109, 110, 111, 112 }, new short[3] { 68, 70, 47 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 5, -5, -9000, -12000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(23, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 100, 113, 114, 115, 116 }, new short[2] { 68, 70 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 5, -5, -9000, -12000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(24, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 117, 118, 119, 120, 121 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 27, new List<ShortList>
		{
			new ShortList(62),
			new ShortList(56, 57),
			new ShortList(59, 60)
		}, -3, 3, -9000, -6000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(25, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 122, 123, 124, 125, 126 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 27, new List<ShortList>
		{
			new ShortList(62),
			new ShortList(56, 57),
			new ShortList(59, 60)
		}, -5, 5, -12000, -9000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(26, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 127, 128, 129, 130, 131 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(27, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 132, 133, 134, 135, 136 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 27, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(28, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 137, 138, 139, 140, 141 }, new short[3] { 71, 73, 47 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(29, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 137, 142, 143, 144, 145 }, new short[2] { 72, 47 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(30, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 146, 147, 148, 149, 150 }, new short[1] { 66 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(31, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 151, 152, 153, 154, 155 }, new short[1] { 67 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(32, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 156, 157, 158, 159, 160 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 33, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(33, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 161, 162, 163, 164, 165 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 32, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(34, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 166, 167, 168, 169, 170 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 2, noGuard: false, 25, new List<ShortList>
		{
			new ShortList(277, 278, 278),
			new ShortList(40)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(35, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 171, 172, 173, 174, 175 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 24, new List<ShortList>
		{
			new ShortList(36, 37, 38),
			new ShortList(33)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(36, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 176, 177, 178, 179, 180 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(41, 44, 30, 31, 46, 23, 28, 44, 41)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(37, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 181, 182, 183, 184, 185 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 2, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(42, 45, 30, 31, 58, 23, 28, 44, 41)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(38, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 186, 187, 188, 189, 190 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 2, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(43, 46, 30, 31, 27, 23, 28, 44, 41)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(39, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 191, 192, 193, 194, 195 }, new short[1] { 42 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(40, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 196, 197, 198, 199, 200 }, new short[1] { 42 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(41, 18, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 90, 201, 202, 203, 204 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -3000, -6000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(42, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 100, 205, 206, 207, 208 }, new short[3] { 68, 70, 47 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(43, 22, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 209, 210, 211, 212, 213 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 5, -5, -9000, -12000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(44, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 95, 214, 215, 216, 217 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -6000, -3000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(45, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 218, 219, 220, 221, 222 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 28, new List<ShortList>
		{
			new ShortList(63),
			new ShortList(58),
			new ShortList(61)
		}, -3, 3, -9000, -6000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(46, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 223, 224, 225, 226, 227 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 27, new List<ShortList>
		{
			new ShortList(63),
			new ShortList(56, 57),
			new ShortList(59, 60)
		}, -5, 5, -12000, -9000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(47, 28, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 228, 229, 230, 231, 232 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(48, 23, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 233, 234, 235, 236, 237 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(49, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 238, 239, 240, 241, 242 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(1, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 10, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(50, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 243, 244, 245, 246, 247 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(95, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 10, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(51, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 248, 249, 250, 251, 252 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(3, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 10, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(52, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 253, 254, 255, 256, 257 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(2, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 5, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(53, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 258, 259, 260, 261, 262 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(96, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 5, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(54, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 263, 264, 265, 266, 267 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(4, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 5, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(55, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 268, 269, 270, 271, 272 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(56, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 273, 274, 275, 276, 277 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(1, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 10, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(57, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 273, 278, 279, 280, 281 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(95, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 10, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(58, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 273, 282, 283, 284, 285 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(3, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 10, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(59, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 286, 287, 288, 289, 290 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(2, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 5, isFavorabilityCost: false));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new SecretInformationAppliedResultItem(60, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 291, 292, 293, 294, 295 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(96, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 5, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(61, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 296, 297, 298, 299, 300 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(4, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 5, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(62, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 301, 302, 303, 304, 305 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(63, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 301, 306, 307, 308, 309 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(64, -1, "24b66f5e-cd47-486c-ad8f-6e069bd8dd71", endEventAfterJump: true, revealCharacters: false, new int[5] { 310, 311, 312, 313, 314 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(65, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 315, 316, 317, 318, 319 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 30, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(66, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 320, 321, 322, 323, 324 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 31, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(67, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 325, 326, 327, 328, 329 }, new short[3] { 45, 69, 47 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(68, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 325, 330, 331, 332, 333 }, new short[3] { 44, 46, 47 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(69, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 325, 334, 335, 336, 337 }, new short[2] { 43, 47 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(70, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 325, 338, 339, 340, 341 }, new short[2] { 48, 49 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 34, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(71, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 325, 342, 343, 344, 345 }, new short[2] { 48, 50 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 34, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(72, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 325, 346, 347, 348, 349 }, new short[2] { 48, 51 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 34, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(73, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 350, 351, 352, 353, 354 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 35, new List<ShortList>
		{
			new ShortList(74, 28)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(74, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 355, 356, 357, 358, 359 }, new short[1] { 67 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(75, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 360, 361, 362, 363, 364 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 18, new List<ShortList>
		{
			new ShortList(76, 77, 78, 79, 80)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(76, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 365, 366, 367, 368, 369 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(77, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 370, 371, 372, 373, 374 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(78, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 375, 376, 377, 378, 379 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(79, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 380, 381, 382, 383, 384 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(80, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 385, 386, 387, 388, 389 }, new short[1] { 53 }, new List<ShortList>
		{
			new ShortList(25, 5, 1, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(81, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 390, 391, 392, 393, 394 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: false, 21, new List<ShortList>
		{
			new ShortList(84, 85, 86, 87, 27, 89, 88, 85, 84),
			new ShortList(82, 83),
			new ShortList(98, 97)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(82, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 395, 396, 397, 398, 399 }, new short[1] { 42 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(83, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 400, 401, 402, 403, 404 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(84, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 90, 405, 406, 407, 408 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(85, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 409, 410, 411, 412, 413 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -9000, -6000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(86, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 414, 415, 416, 417, 418 }, new short[1] { 66 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -9000, -6000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(87, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 151, 419, 420, 421, 422 }, new short[1] { 67 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(88, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 423, 424, 425, 426, 427 }, new short[3] { 71, 73, 78 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(89, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 428, 429, 430, 431, 432 }, new short[2] { 68, 70 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -5, 5, -12000, -9000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(90, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 433, 434, 435, 436, 437 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 19, new List<ShortList>
		{
			new ShortList(91, 92, 93, 94),
			new ShortList(95, 96)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(91, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 438, 439, 440, 441, 442 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(92, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 443, 444, 445, 446, 447 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(93, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 448, 449, 450, 451, 452 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(94, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 453, 454, 455, 456, 457 }, new short[1] { 74 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(95, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 458, 459, 460, 461, 462 }, new short[1] { 53 }, new List<ShortList>
		{
			new ShortList(25, 5, 1, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(96, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 463, 464, 465, 466, 467 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(97, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 468, 469, 470, 471, 472 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(98, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 473, 474, 475, 476, 477 }, new short[1] { 42 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(99, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 478, 479, 480, 481, 482 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: false, 20, new List<ShortList>
		{
			new ShortList(105, 106, 107, 108, 27, 109, 110, 106, 105),
			new ShortList(100, 101, 102, 103, 104)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(100, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 483, 484, 485, 486, 487 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(101, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 488, 489, 490, 491, 492 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(102, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 493, 494, 495, 496, 497 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(103, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 498, 499, 500, 501, 502 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(104, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 503, 504, 505, 506, 507 }, new short[1] { 42 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(105, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 508, 509, 510, 511, 512 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(25, 5, 1, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(106, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 513, 514, 515, 516, 517 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -9000, -6000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(107, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 518, 519, 520, 521, 522 }, new short[1] { 66 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -9000, -6000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(108, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 523, 524, 525, 526, 527 }, new short[1] { 67 }, new List<ShortList>
		{
			new ShortList(25, 5, 1, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -9000, -6000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(109, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 528, 529, 530, 531, 532 }, new short[2] { 68, 70 }, new List<ShortList>
		{
			new ShortList(25, 5, 1, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(110, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 423, 533, 534, 535, 536 }, new short[2] { 71, 73 }, new List<ShortList>
		{
			new ShortList(25, 5, 1, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(111, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 537, 538, 539, 540, 541 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 18, new List<ShortList>
		{
			new ShortList(112, 113, 114, 115, 116)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(112, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 542, 543, 544, 545, 546 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(113, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 547, 548, 549, 550, 551 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(114, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 552, 553, 554, 555, 556 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(115, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 557, 558, 559, 560, 561 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(116, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 562, 563, 564, 565, 566 }, new short[1] { 54 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(117, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 567, 568, 569, 570, 571 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: false, 21, new List<ShortList>
		{
			new ShortList(120, 121, 122, 123, 27, 125, 124, 121, 120),
			new ShortList(118, 119),
			new ShortList(145, 144)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(118, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 572, 573, 574, 575, 576 }, new short[1] { 42 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(119, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 577, 578, 579, 580, 581 }, new short[1] { 55 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new SecretInformationAppliedResultItem(120, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 582, 583, 584, 585, 586 }, new short[1] { 55 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(121, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 95, 587, 588, 589, 590 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(122, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 591, 592, 593, 594, 595 }, new short[1] { 66 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -9000, -6000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(123, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 151, 596, 597, 598, 599 }, new short[1] { 58 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(124, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 600, 601, 602, 603, 604 }, new short[3] { 56, 57, 63 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(125, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 605, 606, 607, 608, 609 }, new short[2] { 61, 62 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(126, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 238, 610, 611, 612, 613 }, new short[1] { 55 }, new List<ShortList>
		{
			new ShortList(1, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 10, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(127, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 243, 614, 615, 616, 617 }, new short[1] { 55 }, new List<ShortList>
		{
			new ShortList(95, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 10, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(128, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 253, 618, 619, 620, 621 }, new short[1] { 55 }, new List<ShortList>
		{
			new ShortList(2, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 5, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(129, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 258, 622, 623, 624, 625 }, new short[1] { 55 }, new List<ShortList>
		{
			new ShortList(96, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 5, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(130, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 268, 626, 627, 628, 629 }, new short[1] { 55 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(131, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 630, 631, 632, 633, 634 }, new short[2] { 64, 65 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(132, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 635, 636, 637, 638, 639 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(2, 5, 1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 5, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(133, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 640, 641, 642, 643, 644 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(96, 5, 1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 5, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(134, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 630, 645, 646, 647, 648 }, new short[2] { 59, 60 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(135, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 635, 649, 650, 651, 652 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(2, 5, 1)
		}, -1, noGuard: false, 31, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 5, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(136, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 640, 653, 654, 655, 656 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(96, 5, 1)
		}, -1, noGuard: false, 31, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 5, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(137, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 657, 658, 659, 660, 661 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 19, new List<ShortList>
		{
			new ShortList(138, 139, 140, 141),
			new ShortList(142, 143)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(138, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 438, 662, 663, 664, 665 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(139, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 443, 666, 667, 668, 669 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(140, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 448, 670, 671, 672, 673 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(141, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 674, 675, 676, 677, 678 }, new short[1] { 74 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(142, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 458, 679, 680, 681, 682 }, new short[1] { 54 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(143, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 683, 684, 685, 686, 687 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(144, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 688, 689, 690, 691, 692 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(145, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 473, 693, 694, 695, 696 }, new short[1] { 42 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(146, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 697, 698, 699, 700, 701 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: false, 20, new List<ShortList>
		{
			new ShortList(120, 121, 122, 123, 27, 125, 124, 121, 120),
			new ShortList(147, 148, 149, 150, 151)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(147, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 702, 703, 704, 705, 706 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(148, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 707, 708, 709, 710, 711 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(149, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 712, 713, 714, 715, 716 }, new short[1] { 39 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(150, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 717, 718, 719, 720, 721 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(151, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 722, 723, 724, 725, 726 }, new short[1] { 42 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(152, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 727, 728, 729, 730, 731 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(24, 5, 1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 5, 0, 6000, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(153, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 732, 733, 734, 735, 736 }, new short[1] { 75 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, -5, 0, -6000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(154, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 737, 738, 739, 740, 741 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 29, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 5, 0, 6000, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(155, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 732, 742, 743, 744, 745 }, new short[1] { 76 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, -5, 0, -6000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(156, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 746, 747, 748, 749, 750 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 26, new List<ShortList>
		{
			new ShortList(158, 167, 174),
			new ShortList(160, 159),
			new ShortList(168),
			new ShortList(175)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(157, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 751, 752, 753, 754, 755 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 17, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(158, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 756, 757, 758, 759, 760 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: true, -1, new List<ShortList>
		{
			new ShortList(161, 162, 163, 164, 27, 23, 165, 162, 161),
			new ShortList(69)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(159, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 761, 762, 763, 764, 765 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(25, 3, 1, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, -1600, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(160, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 766, 767, 768, 769, 770 }, new short[2] { 77, 78 }, new List<ShortList>
		{
			new ShortList(25, 3, 1, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, -1600, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(161, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 771, 772, 773, 774, 775 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(162, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 776, 777, 778, 779, 780 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(163, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 146, 781, 782, 783, 784 }, new short[1] { 66 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(164, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 785, 786, 787, 788, 789 }, new short[1] { 67 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(165, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 790, 791, 792, 793, 794 }, new short[3] { 71, 73, 78 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(166, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 268, 795, 796, 797, 798 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(167, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 799, 800, 801, 802, 803 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(169, 170)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(168, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 804, 805, 806, 807, 808 }, new short[1] { 79 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, -3200, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(169, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 809, 810, 811, 812, 813 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(170, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 814, 815, 816, 817, 818 }, new short[1] { 80 }, new List<ShortList>
		{
			new ShortList(25, 3, 1, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(171, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 819, 820, 821, 822, 823 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: true, 22, new List<ShortList>
		{
			new ShortList(161, 162, 163, 164, 27, 23, 165, 162, 161),
			new ShortList(69),
			new ShortList(173, 172)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(172, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 824, 825, 826, 827, 828 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(173, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 829, 830, 831, 832, 833 }, new short[2] { 77, 78 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(174, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 834, 835, 836, 837, 838 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: true, -1, new List<ShortList>
		{
			new ShortList(177, 178, 179, 180, 181, 182, 165, 178, 177),
			new ShortList(69)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(175, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 839, 840, 841, 842, 843 }, new short[2] { 82, 84 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, -4800, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(176, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 844, 845, 846, 847, 848 }, new short[1] { 85 }, new List<ShortList>
		{
			new ShortList(25, 3, 1, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(177, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 90, 849, 850, 851, 852 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -9000, -6000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(178, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 844, 853, 854, 855, 856 }, new short[1] { 85 }, new List<ShortList>
		{
			new ShortList(25, 3, 1, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(179, 163, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 857, 858, 859, 860, 861 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new SecretInformationAppliedResultItem(180, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 151, 862, 863, 864, 865 }, new short[1] { 67 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(181, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 132, 866, 867, 868, 869 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(25, 3, 1, 5)
		}, -1, noGuard: false, 27, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(182, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 870, 871, 872, 873, 874 }, new short[2] { 68, 70 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(183, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 875, 876, 877, 878, 879 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 26, new List<ShortList>
		{
			new ShortList(184, 192, 199),
			new ShortList(186, 185),
			new ShortList(193),
			new ShortList(200)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(184, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 880, 881, 882, 883, 884 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: true, -1, new List<ShortList>
		{
			new ShortList(187, 188, 189, 190, 27, 23, 191, 188, 187),
			new ShortList(69)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(185, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 885, 886, 887, 888, 889 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 29, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, -1600, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(186, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 766, 890, 891, 892, 893 }, new short[2] { 77, 78 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, -1600, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(187, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 90, 894, 895, 896, 897 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(188, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 776, 898, 899, 900, 901 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 29, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(189, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 146, 902, 903, 904, 905 }, new short[1] { 66 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 29, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(190, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 151, 906, 907, 908, 909 }, new short[1] { 67 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(191, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 137, 910, 911, 912, 913 }, new short[3] { 71, 73, 78 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(192, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 914, 915, 916, 917, 918 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(194, 195)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(193, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 919, 920, 921, 922, 923 }, new short[1] { 79 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, -3200, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(194, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 809, 924, 925, 926, 927 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -9000, -6000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(195, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 928, 929, 930, 931, 932 }, new short[1] { 81 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(196, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 933, 934, 935, 936, 937 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: true, 22, new List<ShortList>
		{
			new ShortList(187, 188, 189, 190, 27, 23, 191, 188, 187),
			new ShortList(69),
			new ShortList(173, 172)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(197, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 938, 939, 940, 941, 942 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 29, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(198, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 829, 943, 944, 945, 946 }, new short[2] { 77, 78 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(199, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 947, 948, 949, 950, 951 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: true, -1, new List<ShortList>
		{
			new ShortList(202, 203, 204, 205, 206, 207, 191, 203, 202),
			new ShortList(69)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(200, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 839, 952, 953, 954, 955 }, new short[2] { 82, 84 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, -4800, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(201, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 956, 957, 958, 959, 960 }, new short[1] { 85 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 29, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(202, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 90, 961, 962, 963, 964 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, -3, -9000, -6000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(203, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 956, 965, 966, 967, 968 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 29, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -6000, -9000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(204, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 146, 969, 970, 971, 972 }, new short[1] { 66 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 29, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(205, 31, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 973, 974, 975, 976, 977 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(206, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 132, 978, 979, 980, 981 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 29, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(207, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 982, 983, 984, 985, 986 }, new short[2] { 68, 70 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(208, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 987, 988, 989, 990, 991 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(24, 3, 1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 5, 0, 6000, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(209, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 992, 993, 994, 995, 996 }, new short[4] { 25, 26, 27, 0 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -5, 0, -6000, 0, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(210, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 997, 998, 999, 1000, 1001 }, new short[1] { 55 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 5, 0, 6000, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(211, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1002, 1003, 1004, 1005, 1006 }, new short[4] { 28, 29, 30, 0 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -5, 0, -6000, 0, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(212, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1007, 1008, 1009, 1010, 1011 }, new short[0], new List<ShortList>
		{
			new ShortList(32, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 5, 5, 3000, 3000, -1, -1, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(213, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1012, 1013, 1014, 1015, 1016 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 37, new List<ShortList>
		{
			new ShortList(-1)
		}, 10, 0, 6000, 0, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(214, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1017, 1018, 1019, 1020, 1021 }, new short[0], new List<ShortList>
		{
			new ShortList(37, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 10, 10, 9000, 9000, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(215, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1022, 1023, 1024, 1025, 1026 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(31, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 0, -3000, 0, 1, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(216, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1027, 1028, 1029, 1030, 1031 }, new short[0], new List<ShortList>
		{
			new ShortList(33, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -5, -5, -6000, -6000, 3, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(217, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1037, 1038, 1039, 1040, 1041 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(35, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -10, -10, -9000, -9000, 5, 5, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(218, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1042, 1043, 1044, 1045, 1046 }, new short[0], new List<ShortList>
		{
			new ShortList(38, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -10, -10, -12000, -12000, 5, 5, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(219, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1047, 1048, 1049, 1050, 1051 }, new short[0], new List<ShortList>
		{
			new ShortList(30, 5, 3)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 3, 0, 3000, 0, -3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(220, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1052, 1053, 1054, 1055, 1056 }, new short[0], new List<ShortList>
		{
			new ShortList(32, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 5, 5, 3000, 3000, -1, -1, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(221, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1057, 1058, 1059, 1060, 1061 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 36, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 10, 0, 6000, 0, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(222, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1062, 1063, 1064, 1065, 1066 }, new short[0], new List<ShortList>
		{
			new ShortList(37, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 10, 10, 9000, 9000, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(223, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1067, 1068, 1069, 1070, 1071 }, new short[0], new List<ShortList>
		{
			new ShortList(31, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, -3, 0, -3000, 0, 1, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(224, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1072, 1073, 1074, 1075, 1076 }, new short[0], new List<ShortList>
		{
			new ShortList(33, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -5, -5, -6000, -6000, 3, 3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(225, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1082, 1083, 1084, 1085, 1086 }, new short[0], new List<ShortList>
		{
			new ShortList(35, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -10, -10, -9000, -9000, 5, 5, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(226, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1087, 1088, 1089, 1090, 1091 }, new short[0], new List<ShortList>
		{
			new ShortList(38, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -10, -10, -12000, -12000, 5, 5, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(227, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1092, 1093, 1094, 1095, 1096 }, new short[0], new List<ShortList>
		{
			new ShortList(30, 3, 5)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 3, 0, 3000, 0, -3, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(228, -1, "5966af43-b141-43fc-9446-f2c30c57b933", endEventAfterJump: false, revealCharacters: true, new int[5] { 1097, 1098, 1099, 1100, 1101 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(229, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1032, 1033, 1034, 1035, 1036 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, -10, 0, -9000, 0, 5, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(230, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1077, 1078, 1079, 1080, 1081 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, -10, 0, -9000, 0, 5, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(231, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1107, 1108, 1109, 1110, 1111 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 43, new List<ShortList>
		{
			new ShortList(233, 235, 236),
			new ShortList(234, 235, 265),
			new ShortList(239, 241, 242),
			new ShortList(240, 241, 266)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(232, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1112, 1113, 1114, 1115, 1116 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(233, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1112, 1117, 1118, 1119, 1120 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(234, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1121, 1122, 1123, 1124, 1125 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(235, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1126, 1127, 1128, 1129, 1130 }, new short[4] { 14, 13, 90, 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(236, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1131, 1132, 1133, 1134, 1135 }, new short[4] { 14, 13, 90, 10 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(237, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1141, 1142, 1143, 1144, 1145 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 43, new List<ShortList>
		{
			new ShortList(233, 235, 236),
			new ShortList(234, 235, 265),
			new ShortList(239, 241, 242),
			new ShortList(240, 241, 266)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(238, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1146, 1147, 1148, 1149, 1150 }, new short[2] { 3, 4 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(239, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1146, 1147, 1148, 1149, 1150 }, new short[2] { 3, 4 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new SecretInformationAppliedResultItem(240, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1151, 1152, 1153, 1154, 1155 }, new short[2] { 3, 4 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(241, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1126, 1127, 1128, 1129, 1130 }, new short[2] { 3, 4 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(242, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1146, 1147, 1148, 1149, 1150 }, new short[2] { 3, 4 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(243, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1156, 1157, 1158, 1159, 1160 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 44, new List<ShortList>
		{
			new ShortList(245, 246, 248, 247),
			new ShortList(252, 253, 250),
			new ShortList(36, 37)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(244, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1161, 1162, 1163, 1164, 1165 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(245, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1161, 1166, 1167, 1168, 1169 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(246, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1170, 1171, 1172, 1173, 1174 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(247, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1175, 1176, 1177, 1178, 1179 }, new short[1] { 36 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(248, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1131, 1132, 1133, 1134, 1135 }, new short[3] { 14, 13, 10 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(249, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1180, 1181, 1182, 1183, 1184 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 44, new List<ShortList>
		{
			new ShortList(245, 246, 248, 247),
			new ShortList(252, 253, 250),
			new ShortList(36, 37)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(250, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1146, 1147, 1148, 1149, 1150 }, new short[2] { 1, 2 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(251, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1185, 1186, 1187, 1188, 1189 }, new short[2] { 3, 4 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(252, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1185, 1190, 1191, 1192, 1193 }, new short[2] { 3, 4 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(253, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1194, 1195, 1196, 1197, 1198 }, new short[2] { 3, 4 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(254, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1199, 1200, 1201, 1202, 1203 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(255, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1204, 1205, 1206, 1207, 1208 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(256, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1209, 1210, 1211, 1212, 1213 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(257, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1214, 1215, 1216, 1217, 1218 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(258, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1219, 1220, 1221, 1222, 1223 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(259, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1224, 1225, 1226, 1227, 1228 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(260, -1, "324eb6e5-d208-4773-8126-8e14a79b6890", endEventAfterJump: false, revealCharacters: true, new int[5] { 1102, 1103, 1104, 1105, 1106 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(261, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1229, 1230, 1231, 1232, 1233 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 47, new List<ShortList>
		{
			new ShortList(14, 13)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(262, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1234, 1235, 1236, 1237, 1238 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 48, new List<ShortList>
		{
			new ShortList(209, 208)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(263, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1239, 1240, 1241, 1242, 1243 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 48, new List<ShortList>
		{
			new ShortList(211, 210)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(264, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1244, 1245, 1246, 1247, 1248 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 50, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(265, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1136, 1137, 1138, 1139, 1140 }, new short[4] { 14, 13, 90, 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(266, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1136, 1137, 1138, 1139, 1140 }, new short[2] { 3, 4 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(267, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1249, 1250, 1251, 1252, 1253 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 49, new List<ShortList>
		{
			new ShortList(268, 269, 270)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(268, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1254, 1255, 1256, 1257, 1258 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(269, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1259, 1260, 1261, 1262, 1263 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(270, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1264, 1265, 1266, 1267, 1268 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(271, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1269, 1270, 1271, 1272, 1273 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 51, new List<ShortList>
		{
			new ShortList(272),
			new ShortList(273),
			new ShortList(274),
			new ShortList(270)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(272, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1274, 1275, 1276, 1277, 1278 }, new short[2] { 91, 92 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(273, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1279, 1280, 1281, 1282, 1283 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(274, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1284, 1285, 1286, 1287, 1288 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(275, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1289, 1290, 1291, 1292, 1293 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 52, new List<ShortList>
		{
			new ShortList(276),
			new ShortList(273),
			new ShortList(270)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(276, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 1294, 1295, 1296, 1297, 1298 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(277, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1299, 1300, 1301, 1302, 1303 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(41, 279, 30, 31, 281, 23, 28, 279, 41)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(278, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1304, 1305, 1306, 1307, 1308 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, 2, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(43, 280, 30, 31, 281, 23, 28, 279, 41)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(279, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 95, 1309, 1310, 1311, 1312 }, new short[1] { 38 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 38, new List<ShortList>
		{
			new ShortList(-1)
		}, -3, 3, -6000, -3000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(280, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 1313, 1314, 1315, 1316, 1317 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 27, new List<ShortList>
		{
			new ShortList(286),
			new ShortList(282, 283),
			new ShortList(284, 285)
		}, -5, 5, -12000, -9000, 3, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(281, -1, null, endEventAfterJump: true, revealCharacters: false, new int[5] { 132, 1318, 1319, 1320, 1321 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 27, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(282, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 273, 1322, 1323, 1324, 1325 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(1, 3, 5)
		}, -1, noGuard: false, 38, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 10, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(283, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 273, 1326, 1327, 1328, 1329 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(95, 3, 5)
		}, -1, noGuard: false, 38, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 10, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(284, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 286, 1330, 1331, 1332, 1333 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(2, 3, 5)
		}, -1, noGuard: false, 38, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 5, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(285, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 291, 1334, 1335, 1336, 1337 }, new short[1] { 52 }, new List<ShortList>
		{
			new ShortList(96, 3, 5)
		}, -1, noGuard: false, 38, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 5, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(286, -1, null, endEventAfterJump: true, revealCharacters: true, new int[5] { 301, 1338, 1339, 1340, 1341 }, new short[1] { 37 }, new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, 38, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
		_dataArray.Add(new SecretInformationAppliedResultItem(287, -1, "56b42732-239a-48b6-bfcd-a9fae0fa1147", endEventAfterJump: false, revealCharacters: true, new int[5] { 1342, 1343, 1344, 1345, 1346 }, new short[0], new List<ShortList>
		{
			new ShortList(-1)
		}, -1, noGuard: false, -1, new List<ShortList>
		{
			new ShortList(-1)
		}, 0, 0, 0, 0, 0, 0, isFavorabilityCost: false));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SecretInformationAppliedResultItem>(288);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
	}
}
