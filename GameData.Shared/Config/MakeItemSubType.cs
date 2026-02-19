using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells;
using GameData.Domains.Map;

namespace Config;

[Serializable]
public class MakeItemSubType : ConfigData<MakeItemSubTypeItem, short>
{
	public static MakeItemSubType Instance = new MakeItemSubType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "RefiningEffect", "Result", "TemplateId", "Name", "FilterName", "Desc", "Icon" };

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
		_dataArray.Add(new MakeItemSubTypeItem(0, 0, 1, isOdd: false, -1, 2, 15, null, 0, 1, new MaterialResources(0, 1, 0, 0, 0, 0), new MakeItemResult("Misc", 227), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(1, 3, 4, isOdd: false, -1, 5, 15, null, 360, 1, new MaterialResources(0, 0, 0, 0, 0, 1), new MakeItemResult("Medicine", 346), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(2, 6, 7, isOdd: false, -1, 8, 15, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Weapon", 3), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(3, 9, 10, isOdd: false, -1, 11, 15, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Weapon", 12), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(4, 12, 13, isOdd: false, 2, 14, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Weapon", 21), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(5, 15, 16, isOdd: false, 0, 17, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Weapon", 30), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(6, 18, 19, isOdd: false, 5, 20, 15, null, 0, 12, new MaterialResources(0, 2, 2, 12, 2, 0), new MakeItemResult("Weapon", 39), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(7, 21, 22, isOdd: false, 7, 23, 15, null, 0, 12, new MaterialResources(0, 2, 2, 2, 12, 0), new MakeItemResult("Weapon", 48), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(8, 24, 13, isOdd: false, 2, 25, 15, null, 0, 6, new MaterialResources(0, 1, 6, 1, 1, 0), new MakeItemResult("Weapon", 57), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(9, 26, 16, isOdd: false, 0, 27, 15, null, 0, 6, new MaterialResources(0, 6, 1, 1, 1, 0), new MakeItemResult("Weapon", 93), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(10, 28, 29, isOdd: false, 4, 30, 15, null, 0, 6, new MaterialResources(0, 1, 1, 6, 1, 0), new MakeItemResult("Weapon", 75), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(11, 31, 32, isOdd: false, 3, 33, 15, null, 0, 6, new MaterialResources(0, 1, 6, 1, 1, 0), new MakeItemResult("Weapon", 66), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(12, 34, 35, isOdd: false, 1, 36, 15, null, 0, 6, new MaterialResources(0, 6, 1, 1, 1, 0), new MakeItemResult("Weapon", 102), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(13, 37, 19, isOdd: false, 5, 38, 15, null, 0, 6, new MaterialResources(0, 1, 1, 6, 1, 0), new MakeItemResult("Weapon", 84), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(14, 39, 13, isOdd: false, 2, 40, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Weapon", 111), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(15, 41, 16, isOdd: false, 0, 42, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Weapon", 147), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(16, 43, 29, isOdd: false, 4, 44, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Weapon", 129), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(17, 45, 32, isOdd: false, 3, 46, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Weapon", 120), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(18, 47, 35, isOdd: false, 1, 48, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Weapon", 156), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(19, 49, 19, isOdd: false, 5, 50, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Weapon", 138), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(20, 51, 13, isOdd: false, 2, 52, 15, null, 0, 12, new MaterialResources(0, 2, 12, 2, 2, 0), new MakeItemResult("Weapon", 165), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(21, 53, 16, isOdd: false, 0, 54, 15, null, 0, 12, new MaterialResources(0, 12, 2, 2, 2, 0), new MakeItemResult("Weapon", 201), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(22, 55, 29, isOdd: false, 4, 56, 15, null, 0, 12, new MaterialResources(0, 2, 2, 12, 2, 0), new MakeItemResult("Weapon", 183), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(23, 57, 32, isOdd: false, 3, 58, 15, null, 0, 12, new MaterialResources(0, 2, 12, 2, 2, 0), new MakeItemResult("Weapon", 174), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(24, 59, 35, isOdd: false, 1, 60, 15, null, 0, 12, new MaterialResources(0, 12, 2, 2, 2, 0), new MakeItemResult("Weapon", 210), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(25, 61, 19, isOdd: false, 5, 62, 15, null, 0, 12, new MaterialResources(0, 2, 2, 12, 2, 0), new MakeItemResult("Weapon", 192), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(26, 63, 13, isOdd: false, 2, 64, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Weapon", 219), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(27, 65, 16, isOdd: false, 0, 66, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Weapon", 255), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(28, 67, 29, isOdd: false, 4, 68, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Weapon", 237), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(29, 69, 32, isOdd: false, 3, 70, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Weapon", 228), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(30, 71, 35, isOdd: false, 1, 72, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Weapon", 264), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(31, 73, 19, isOdd: false, 5, 74, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Weapon", 246), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(32, 75, 13, isOdd: false, 2, 76, 15, null, 0, 24, new MaterialResources(0, 4, 24, 4, 4, 0), new MakeItemResult("Weapon", 273), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(33, 77, 13, isOdd: false, 2, 78, 15, null, 0, 24, new MaterialResources(0, 4, 24, 4, 4, 0), new MakeItemResult("Weapon", 282), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(34, 79, 16, isOdd: false, 0, 80, 15, null, 0, 24, new MaterialResources(0, 24, 4, 4, 4, 0), new MakeItemResult("Weapon", 327), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(35, 81, 29, isOdd: false, 4, 82, 15, null, 0, 24, new MaterialResources(0, 4, 4, 24, 4, 0), new MakeItemResult("Weapon", 309), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(36, 83, 84, isOdd: false, 6, 85, 15, null, 0, 24, new MaterialResources(0, 4, 4, 4, 24, 0), new MakeItemResult("Weapon", 345), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(37, 86, 84, isOdd: false, 6, 87, 15, null, 0, 24, new MaterialResources(0, 4, 4, 4, 24, 0), new MakeItemResult("Weapon", 354), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(38, 75, 32, isOdd: false, 3, 88, 15, null, 0, 24, new MaterialResources(0, 4, 24, 4, 4, 0), new MakeItemResult("Weapon", 291), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(39, 77, 32, isOdd: false, 3, 89, 15, null, 0, 24, new MaterialResources(0, 4, 24, 4, 4, 0), new MakeItemResult("Weapon", 300), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(40, 90, 35, isOdd: false, 1, 91, 15, null, 0, 24, new MaterialResources(0, 24, 4, 4, 4, 0), new MakeItemResult("Weapon", 336), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(41, 92, 19, isOdd: false, 5, 93, 15, null, 0, 24, new MaterialResources(0, 4, 4, 24, 4, 0), new MakeItemResult("Weapon", 318), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(42, 83, 22, isOdd: false, 7, 94, 15, null, 0, 24, new MaterialResources(0, 4, 4, 4, 24, 0), new MakeItemResult("Weapon", 363), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(43, 86, 22, isOdd: false, 7, 95, 15, null, 0, 24, new MaterialResources(0, 4, 4, 4, 24, 0), new MakeItemResult("Weapon", 372), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(44, 96, 13, isOdd: false, 2, 97, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Weapon", 381), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(45, 98, 16, isOdd: false, 0, 99, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Weapon", 417), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(46, 100, 29, isOdd: false, 4, 101, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Weapon", 399), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(47, 102, 32, isOdd: false, 3, 103, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Weapon", 390), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(48, 104, 35, isOdd: false, 1, 105, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Weapon", 426), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(49, 106, 19, isOdd: false, 5, 107, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Weapon", 408), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(50, 108, 13, isOdd: false, 2, 109, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 435), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(51, 110, 13, isOdd: false, 2, 111, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 444), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(52, 112, 13, isOdd: false, 2, 113, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 453), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(53, 114, 16, isOdd: false, 0, 115, 15, null, 0, 30, new MaterialResources(0, 30, 5, 5, 5, 0), new MakeItemResult("Weapon", 507), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(54, 116, 29, isOdd: false, 4, 117, 15, null, 0, 30, new MaterialResources(0, 5, 5, 30, 5, 0), new MakeItemResult("Weapon", 489), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(55, 118, 32, isOdd: false, 3, 119, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 462), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(56, 120, 32, isOdd: false, 3, 121, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 471), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(57, 122, 32, isOdd: false, 3, 123, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 480), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(58, 124, 35, isOdd: false, 1, 125, 15, null, 0, 30, new MaterialResources(0, 30, 5, 5, 5, 0), new MakeItemResult("Weapon", 516), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(59, 126, 19, isOdd: false, 5, 127, 15, null, 0, 30, new MaterialResources(0, 5, 5, 30, 5, 0), new MakeItemResult("Weapon", 498), 1, 2, 0, 4));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new MakeItemSubTypeItem(60, 108, 13, isOdd: false, 2, 128, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 525), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(61, 110, 13, isOdd: false, 2, 129, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 534), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(62, 112, 13, isOdd: false, 2, 130, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 543), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(63, 131, 16, isOdd: false, 0, 132, 15, null, 0, 30, new MaterialResources(0, 30, 5, 5, 5, 0), new MakeItemResult("Weapon", 597), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(64, 133, 29, isOdd: false, 4, 134, 15, null, 0, 30, new MaterialResources(0, 5, 5, 30, 5, 0), new MakeItemResult("Weapon", 579), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(65, 118, 32, isOdd: false, 3, 135, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 552), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(66, 120, 32, isOdd: false, 3, 136, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 561), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(67, 122, 32, isOdd: false, 3, 137, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 570), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(68, 138, 35, isOdd: false, 1, 139, 15, null, 0, 30, new MaterialResources(0, 30, 5, 5, 5, 0), new MakeItemResult("Weapon", 606), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(69, 140, 19, isOdd: false, 5, 141, 15, null, 0, 30, new MaterialResources(0, 5, 5, 30, 5, 0), new MakeItemResult("Weapon", 588), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(70, 142, 13, isOdd: false, 2, 143, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 615), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(71, 144, 13, isOdd: false, 2, 145, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 624), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(72, 146, 16, isOdd: false, 0, 147, 15, null, 0, 30, new MaterialResources(0, 30, 5, 5, 5, 0), new MakeItemResult("Weapon", 669), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(73, 148, 16, isOdd: false, 0, 149, 15, null, 0, 30, new MaterialResources(0, 30, 5, 5, 5, 0), new MakeItemResult("Weapon", 678), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(74, 150, 29, isOdd: false, 4, 151, 15, null, 0, 30, new MaterialResources(0, 5, 5, 30, 5, 0), new MakeItemResult("Weapon", 651), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(75, 142, 32, isOdd: false, 3, 152, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 633), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(76, 144, 32, isOdd: false, 3, 153, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 642), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(77, 146, 35, isOdd: false, 1, 154, 15, null, 0, 30, new MaterialResources(0, 30, 5, 5, 5, 0), new MakeItemResult("Weapon", 687), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(78, 148, 35, isOdd: false, 1, 155, 15, null, 0, 30, new MaterialResources(0, 30, 5, 5, 5, 0), new MakeItemResult("Weapon", 696), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(79, 156, 19, isOdd: false, 5, 157, 15, null, 0, 30, new MaterialResources(0, 5, 5, 30, 5, 0), new MakeItemResult("Weapon", 660), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(80, 158, 13, isOdd: false, 2, 159, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 705), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(81, 160, 16, isOdd: false, 0, 161, 15, null, 0, 30, new MaterialResources(0, 30, 5, 5, 5, 0), new MakeItemResult("Weapon", 741), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(82, 162, 29, isOdd: false, 4, 163, 15, null, 0, 30, new MaterialResources(0, 5, 5, 30, 5, 0), new MakeItemResult("Weapon", 723), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(83, 164, 32, isOdd: false, 3, 165, 15, null, 0, 30, new MaterialResources(0, 5, 30, 5, 5, 0), new MakeItemResult("Weapon", 714), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(84, 166, 35, isOdd: false, 1, 167, 15, null, 0, 30, new MaterialResources(0, 30, 5, 5, 5, 0), new MakeItemResult("Weapon", 750), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(85, 168, 19, isOdd: false, 5, 169, 15, null, 0, 30, new MaterialResources(0, 5, 5, 30, 5, 0), new MakeItemResult("Weapon", 732), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(86, 170, 16, isOdd: false, 0, 171, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Weapon", 759), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(87, 172, 84, isOdd: false, 6, 173, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Weapon", 777), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(88, 174, 35, isOdd: false, 1, 175, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Weapon", 768), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(89, 176, 22, isOdd: false, 7, 177, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Weapon", 786), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(90, 178, 16, isOdd: false, 0, 179, 15, null, 0, 24, new MaterialResources(0, 24, 4, 4, 4, 0), new MakeItemResult("Weapon", 795), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(91, 180, 84, isOdd: false, 6, 181, 15, null, 0, 24, new MaterialResources(0, 4, 4, 4, 24, 0), new MakeItemResult("Weapon", 813), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(92, 182, 35, isOdd: false, 1, 183, 15, null, 0, 24, new MaterialResources(0, 24, 4, 4, 4, 0), new MakeItemResult("Weapon", 804), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(93, 184, 22, isOdd: false, 7, 185, 15, null, 0, 24, new MaterialResources(0, 4, 4, 4, 24, 0), new MakeItemResult("Weapon", 822), 1, 2, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(94, 186, 13, isOdd: false, 2, 187, 15, null, 0, 12, new MaterialResources(0, 0, 12, 0, 0, 0), new MakeItemResult("Accessory", 0), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(95, 188, 16, isOdd: false, 0, 189, 15, null, 0, 12, new MaterialResources(0, 12, 0, 0, 0, 0), new MakeItemResult("Accessory", 18), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(96, 190, 16, isOdd: false, 0, 191, 15, null, 0, 12, new MaterialResources(0, 12, 0, 0, 0, 0), new MakeItemResult("Accessory", 27), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(97, 192, 16, isOdd: false, 0, 193, 15, null, 0, 12, new MaterialResources(0, 12, 0, 0, 0, 0), new MakeItemResult("Accessory", 36), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(98, 194, 29, isOdd: false, 4, 195, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 99), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(99, 196, 29, isOdd: false, 4, 197, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 108), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(100, 198, 29, isOdd: false, 4, 199, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 117), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(101, 200, 29, isOdd: false, 4, 201, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 126), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(102, 202, 29, isOdd: false, 4, 203, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 135), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(103, 204, 29, isOdd: false, 4, 205, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 144), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(104, 206, 29, isOdd: false, 4, 207, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 153), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(105, 208, 84, isOdd: false, 6, 209, 15, null, 0, 12, new MaterialResources(0, 0, 0, 0, 12, 0), new MakeItemResult("Accessory", 72), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(106, 210, 32, isOdd: false, 3, 211, 15, null, 0, 12, new MaterialResources(0, 0, 12, 0, 0, 0), new MakeItemResult("Accessory", 9), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(107, 212, 35, isOdd: false, 1, 213, 15, null, 0, 12, new MaterialResources(0, 12, 0, 0, 0, 0), new MakeItemResult("Accessory", 45), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(108, 214, 35, isOdd: false, 1, 215, 15, null, 0, 12, new MaterialResources(0, 12, 0, 0, 0, 0), new MakeItemResult("Accessory", 54), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(109, 216, 35, isOdd: false, 1, 217, 15, null, 0, 12, new MaterialResources(0, 12, 0, 0, 0, 0), new MakeItemResult("Accessory", 63), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(110, 218, 19, isOdd: false, 5, 219, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 162), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(111, 220, 19, isOdd: false, 5, 221, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 171), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(112, 222, 19, isOdd: false, 5, 223, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 180), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(113, 224, 19, isOdd: false, 5, 225, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 189), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(114, 226, 19, isOdd: false, 5, 227, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 198), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(115, 198, 19, isOdd: false, 5, 228, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 207), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(116, 229, 19, isOdd: false, 5, 230, 15, null, 0, 12, new MaterialResources(0, 0, 0, 12, 0, 0), new MakeItemResult("Accessory", 216), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(117, 231, 22, isOdd: false, 7, 232, 15, null, 0, 12, new MaterialResources(0, 0, 0, 0, 12, 0), new MakeItemResult("Accessory", 81), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(118, 233, 13, isOdd: false, 2, 234, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Armor", 0), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(119, 235, 16, isOdd: false, 0, 236, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Armor", 18), 1, 3, 0, 4));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new MakeItemSubTypeItem(120, 237, 29, isOdd: false, 4, 238, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Armor", 108), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(121, 239, 84, isOdd: false, 6, 240, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 36), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(122, 241, 84, isOdd: false, 6, 242, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 45), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(123, 243, 84, isOdd: false, 6, 244, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 54), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(124, 245, 84, isOdd: false, 6, 246, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 63), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(125, 247, 32, isOdd: false, 3, 248, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Armor", 9), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(126, 249, 35, isOdd: false, 1, 250, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Armor", 27), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(127, 251, 19, isOdd: false, 5, 252, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Armor", 117), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(128, 239, 22, isOdd: false, 7, 240, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 99), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(129, 241, 22, isOdd: false, 7, 242, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 72), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(130, 243, 22, isOdd: false, 7, 244, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 81), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(131, 245, 22, isOdd: false, 7, 253, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 90), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(132, 254, 13, isOdd: false, 2, 255, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Armor", 126), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(133, 256, 16, isOdd: false, 0, 257, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Armor", 144), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(134, 258, 29, isOdd: false, 4, 259, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Armor", 234), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(135, 239, 84, isOdd: false, 6, 260, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 162), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(136, 241, 84, isOdd: false, 6, 261, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 180), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(137, 243, 84, isOdd: false, 6, 262, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 171), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(138, 245, 84, isOdd: false, 6, 263, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 189), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(139, 264, 32, isOdd: false, 3, 265, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Armor", 135), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(140, 266, 35, isOdd: false, 1, 267, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Armor", 153), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(141, 268, 19, isOdd: false, 5, 269, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Armor", 243), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(142, 239, 22, isOdd: false, 7, 260, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 207), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(143, 241, 22, isOdd: false, 7, 261, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 198), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(144, 243, 22, isOdd: false, 7, 262, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 225), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(145, 245, 22, isOdd: false, 7, 270, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 216), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(146, 271, 13, isOdd: false, 2, 272, 15, null, 0, 36, new MaterialResources(0, 6, 36, 6, 6, 0), new MakeItemResult("Armor", 252), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(147, 273, 13, isOdd: false, 2, 274, 15, null, 0, 36, new MaterialResources(0, 6, 36, 6, 6, 0), new MakeItemResult("Armor", 261), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(148, 275, 16, isOdd: false, 0, 276, 15, null, 0, 36, new MaterialResources(0, 36, 6, 6, 6, 0), new MakeItemResult("Armor", 288), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(149, 277, 29, isOdd: false, 4, 278, 15, null, 0, 36, new MaterialResources(0, 6, 6, 36, 6, 0), new MakeItemResult("Armor", 378), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(150, 239, 84, isOdd: false, 6, 279, 15, null, 0, 36, new MaterialResources(0, 6, 6, 6, 36, 0), new MakeItemResult("Armor", 324), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(151, 241, 84, isOdd: false, 6, 280, 15, null, 0, 36, new MaterialResources(0, 6, 6, 6, 36, 0), new MakeItemResult("Armor", 306), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(152, 243, 84, isOdd: false, 6, 281, 15, null, 0, 36, new MaterialResources(0, 6, 6, 6, 36, 0), new MakeItemResult("Armor", 315), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(153, 245, 84, isOdd: false, 6, 282, 15, null, 0, 36, new MaterialResources(0, 6, 6, 6, 36, 0), new MakeItemResult("Armor", 333), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(154, 271, 32, isOdd: false, 3, 283, 15, null, 0, 36, new MaterialResources(0, 6, 36, 6, 6, 0), new MakeItemResult("Armor", 270), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(155, 273, 32, isOdd: false, 3, 284, 15, null, 0, 36, new MaterialResources(0, 6, 36, 6, 6, 0), new MakeItemResult("Armor", 279), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(156, 285, 35, isOdd: false, 1, 286, 15, null, 0, 36, new MaterialResources(0, 36, 6, 6, 6, 0), new MakeItemResult("Armor", 297), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(157, 287, 19, isOdd: false, 5, 288, 15, null, 0, 36, new MaterialResources(0, 6, 6, 36, 6, 0), new MakeItemResult("Armor", 387), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(158, 239, 22, isOdd: false, 7, 279, 15, null, 0, 36, new MaterialResources(0, 6, 6, 6, 36, 0), new MakeItemResult("Armor", 342), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(159, 241, 22, isOdd: false, 7, 280, 15, null, 0, 36, new MaterialResources(0, 6, 6, 6, 36, 0), new MakeItemResult("Armor", 369), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(160, 243, 22, isOdd: false, 7, 281, 15, null, 0, 36, new MaterialResources(0, 6, 6, 6, 36, 0), new MakeItemResult("Armor", 351), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(161, 245, 22, isOdd: false, 7, 289, 15, null, 0, 36, new MaterialResources(0, 6, 6, 6, 36, 0), new MakeItemResult("Armor", 360), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(162, 290, 13, isOdd: false, 2, 291, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Armor", 396), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(163, 292, 16, isOdd: false, 0, 293, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Armor", 414), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(164, 294, 29, isOdd: false, 4, 295, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Armor", 504), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(165, 239, 84, isOdd: false, 6, 296, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 459), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(166, 241, 84, isOdd: false, 6, 297, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 432), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(167, 243, 84, isOdd: false, 6, 298, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 450), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(168, 245, 84, isOdd: false, 6, 299, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 441), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(169, 300, 32, isOdd: false, 3, 301, 15, null, 0, 18, new MaterialResources(0, 3, 18, 3, 3, 0), new MakeItemResult("Armor", 405), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(170, 302, 35, isOdd: false, 1, 303, 15, null, 0, 18, new MaterialResources(0, 18, 3, 3, 3, 0), new MakeItemResult("Armor", 423), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(171, 304, 19, isOdd: false, 5, 305, 15, null, 0, 18, new MaterialResources(0, 3, 3, 18, 3, 0), new MakeItemResult("Armor", 513), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(172, 239, 22, isOdd: false, 7, 296, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 477), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(173, 241, 22, isOdd: false, 7, 297, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 468), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(174, 243, 22, isOdd: false, 7, 298, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 486), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(175, 245, 22, isOdd: false, 7, 306, 15, null, 0, 18, new MaterialResources(0, 3, 3, 3, 18, 0), new MakeItemResult("Armor", 495), 1, 3, 0, 4));
		_dataArray.Add(new MakeItemSubTypeItem(176, 307, 84, isOdd: false, -1, 308, 15, null, 0, 18, new MaterialResources(0, 0, 0, 0, 18, 0), new MakeItemResult("Clothing", 0), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(177, 309, 22, isOdd: false, -1, 310, 0, null, 0, 18, new MaterialResources(0, 0, 0, 0, 18, 0), new MakeItemResult("Clothing", 9), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(178, 311, 16, isOdd: false, 0, 312, 15, null, 0, 60, new MaterialResources(0, 60, 0, 0, 0, 0), new MakeItemResult("Carrier", 0), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(179, 313, 35, isOdd: false, 1, 314, 15, null, 0, 60, new MaterialResources(0, 60, 0, 0, 0, 0), new MakeItemResult("Carrier", 9), -1, -1, -1, -1));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new MakeItemSubTypeItem(180, 315, 316, isOdd: false, -1, 317, 15, null, 0, 12, new MaterialResources(0, 0, 0, 0, 12, 0), new MakeItemResult("Accessory", 90), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(181, 318, 319, isOdd: false, -1, 320, 15, null, 0, 12, new MaterialResources(0, 0, 0, 0, 12, 0), new MakeItemResult("Misc", 73), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(182, 321, 322, isOdd: false, -1, 323, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 9), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(183, 324, 325, isOdd: false, -1, 326, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 18), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(184, 327, 328, isOdd: false, -1, 329, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 26), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(185, 330, 331, isOdd: false, -1, 332, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 33), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(186, 333, 334, isOdd: false, -1, 335, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 39), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(187, 336, 337, isOdd: false, -1, 338, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 44), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(188, 339, 340, isOdd: false, -1, 341, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 48), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(189, 342, 343, isOdd: false, -1, 344, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 51), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(190, 345, 346, isOdd: false, -1, 347, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 60), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(191, 348, 349, isOdd: false, -1, 350, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 68), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(192, 351, 352, isOdd: false, -1, 353, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 75), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(193, 354, 355, isOdd: false, -1, 356, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 81), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(194, 357, 358, isOdd: false, -1, 359, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 86), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(195, 360, 361, isOdd: false, -1, 362, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 90), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(196, 363, 364, isOdd: false, -1, 365, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 93), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(197, 366, 367, isOdd: false, -1, 368, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 102), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(198, 369, 370, isOdd: false, -1, 371, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 110), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(199, 372, 373, isOdd: false, -1, 374, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 117), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(200, 375, 376, isOdd: false, -1, 377, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 123), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(201, 378, 379, isOdd: false, -1, 380, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 128), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(202, 381, 382, isOdd: false, -1, 383, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 132), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(203, 384, 385, isOdd: false, -1, 386, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 135), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(204, 387, 388, isOdd: false, -1, 389, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 144), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(205, 390, 391, isOdd: false, -1, 392, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 152), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(206, 393, 394, isOdd: false, -1, 395, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 159), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(207, 396, 397, isOdd: false, -1, 398, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 165), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(208, 399, 400, isOdd: false, -1, 401, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 170), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(209, 402, 403, isOdd: false, -1, 404, 15, null, 0, 6, new MaterialResources(6, 0, 0, 0, 0, 0), new MakeItemResult("Food", 174), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(210, 405, 406, isOdd: false, -1, 407, 15, null, 0, 12, new MaterialResources(0, 0, 0, 0, 0, 12), new MakeItemResult("Medicine", 0), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(211, 408, 409, isOdd: false, -1, 410, 15, null, 0, 12, new MaterialResources(0, 0, 0, 0, 0, 12), new MakeItemResult("Medicine", 9), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(212, 411, 412, isOdd: false, -1, 413, 15, null, 0, 12, new MaterialResources(0, 0, 0, 0, 0, 12), new MakeItemResult("Medicine", 18), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(213, 414, 415, isOdd: false, -1, 416, 15, null, 0, 12, new MaterialResources(0, 0, 0, 0, 0, 12), new MakeItemResult("Medicine", 27), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(214, 417, 418, isOdd: false, -1, 419, 15, null, 0, 12, new MaterialResources(0, 0, 0, 0, 0, 12), new MakeItemResult("Medicine", 36), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(215, 420, 421, isOdd: false, -1, 422, 15, null, 0, 12, new MaterialResources(0, 0, 0, 0, 0, 12), new MakeItemResult("Medicine", 45), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(216, 423, 424, isOdd: false, -1, 425, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 60), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(217, 423, 426, isOdd: false, -1, 427, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 172), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(218, 423, 428, isOdd: false, -1, 429, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 208), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(219, 423, 430, isOdd: false, -1, 431, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 280), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(220, 423, 432, isOdd: false, -1, 433, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 72), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(221, 423, 434, isOdd: false, -1, 435, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 148), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(222, 423, 436, isOdd: false, -1, 437, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 220), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(223, 423, 438, isOdd: false, -1, 439, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 292), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(224, 423, 440, isOdd: false, -1, 441, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 88), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(225, 423, 442, isOdd: false, -1, 443, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 160), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(226, 423, 444, isOdd: false, -1, 445, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 232), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(227, 423, 446, isOdd: false, -1, 447, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 304), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(228, 423, 448, isOdd: false, -1, 449, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 100), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(229, 423, 450, isOdd: false, -1, 451, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 196), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(230, 423, 452, isOdd: false, -1, 453, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 244), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(231, 423, 454, isOdd: false, -1, 455, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 316), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(232, 423, 456, isOdd: false, -1, 457, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 112), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(233, 423, 458, isOdd: false, -1, 459, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 184), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(234, 423, 460, isOdd: false, -1, 461, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 256), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(235, 423, 462, isOdd: false, -1, 463, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 328), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(236, 423, 464, isOdd: false, -1, 465, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 124), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(237, 423, 466, isOdd: false, -1, 467, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 136), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(238, 423, 468, isOdd: false, -1, 469, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 268), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(239, 423, 470, isOdd: false, -1, 471, 15, null, 0, 8, new MaterialResources(0, 0, 0, 0, 0, 8), new MakeItemResult("Medicine", 340), -1, -1, -1, -1));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new MakeItemSubTypeItem(240, 472, 473, isOdd: true, -1, 474, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 274), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(241, 472, 475, isOdd: true, -1, 476, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 54), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(242, 472, 477, isOdd: true, -1, 478, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 166), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(243, 472, 479, isOdd: true, -1, 480, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 202), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(244, 472, 481, isOdd: true, -1, 482, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 286), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(245, 472, 483, isOdd: true, -1, 484, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 66), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(246, 472, 485, isOdd: true, -1, 486, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 142), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(247, 472, 487, isOdd: true, -1, 488, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 214), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(248, 472, 489, isOdd: true, -1, 490, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 298), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(249, 472, 491, isOdd: true, -1, 492, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 82), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(250, 472, 493, isOdd: true, -1, 494, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 154), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(251, 472, 495, isOdd: true, -1, 496, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 226), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(252, 472, 497, isOdd: true, -1, 498, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 310), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(253, 472, 499, isOdd: true, -1, 500, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 94), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(254, 472, 501, isOdd: true, -1, 502, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 190), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(255, 472, 503, isOdd: true, -1, 504, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 238), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(256, 472, 505, isOdd: true, -1, 506, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 322), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(257, 472, 507, isOdd: true, -1, 508, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 106), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(258, 472, 509, isOdd: true, -1, 510, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 178), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(259, 472, 511, isOdd: true, -1, 512, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 250), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(260, 472, 513, isOdd: true, -1, 514, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 334), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(261, 472, 515, isOdd: true, -1, 516, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 118), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(262, 472, 517, isOdd: true, -1, 518, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 130), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(263, 472, 519, isOdd: true, -1, 520, 0, null, 0, 6, new MaterialResources(0, 0, 0, 0, 0, 6), new MakeItemResult("Medicine", 262), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(264, 521, 522, isOdd: false, -1, 523, 15, null, 0, 24, new MaterialResources(0, 0, 24, 0, 0, 0), new MakeItemResult("Accessory", 250), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(265, 524, 525, isOdd: false, -1, 526, 15, null, 0, 24, new MaterialResources(0, 0, 0, 24, 0, 0), new MakeItemResult("Accessory", 251), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(266, 527, 528, isOdd: false, -1, 529, 15, null, 0, 24, new MaterialResources(0, 0, 0, 0, 24, 0), new MakeItemResult("Accessory", 252), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(267, 530, 531, isOdd: false, -1, 532, 15, null, 0, 24, new MaterialResources(0, 0, 0, 24, 0, 0), new MakeItemResult("Accessory", 253), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(268, 533, 534, isOdd: false, -1, 535, 15, null, 0, 24, new MaterialResources(0, 0, 24, 0, 0, 0), new MakeItemResult("Accessory", 254), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(269, 536, 537, isOdd: false, -1, 538, 15, null, 0, 24, new MaterialResources(0, 0, 0, 24, 0, 0), new MakeItemResult("Accessory", 255), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(270, 539, 540, isOdd: false, -1, 541, 15, null, 0, 24, new MaterialResources(0, 0, 0, 24, 0, 0), new MakeItemResult("Accessory", 256), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(271, 542, 543, isOdd: false, -1, 544, 15, null, 0, 24, new MaterialResources(0, 24, 0, 0, 0, 0), new MakeItemResult("Accessory", 257), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(272, 545, 546, isOdd: false, -1, 547, 15, null, 0, 24, new MaterialResources(0, 0, 24, 0, 0, 0), new MakeItemResult("Accessory", 258), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(273, 548, 549, isOdd: false, -1, 550, 15, null, 0, 24, new MaterialResources(0, 0, 0, 24, 0, 0), new MakeItemResult("Accessory", 259), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(274, 551, 552, isOdd: false, -1, 553, 15, null, 0, 24, new MaterialResources(0, 0, 24, 0, 0, 0), new MakeItemResult("Accessory", 260), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(275, 554, 555, isOdd: false, -1, 556, 15, null, 0, 24, new MaterialResources(0, 0, 0, 0, 0, 24), new MakeItemResult("Accessory", 261), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(276, 557, 558, isOdd: false, -1, 559, 15, null, 0, 30, new MaterialResources(0, 0, 0, 0, 30, 0), new MakeItemResult("Misc", 331), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(277, 560, 561, isOdd: false, -1, 562, 0, null, 0, 18, new MaterialResources(0, 0, 0, 0, 18, 0), new MakeItemResult("Clothing", 84), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(278, 563, 564, isOdd: false, -1, 565, 15, null, 0, 24, new MaterialResources(0, 0, 24, 0, 0, 0), new MakeItemResult("Accessory", 262), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(279, 566, 567, isOdd: false, -1, 568, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 263), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(280, 569, 570, isOdd: false, -1, 571, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 264), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(281, 572, 573, isOdd: false, -1, 574, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 265), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(282, 575, 576, isOdd: false, -1, 577, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 266), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(283, 578, 579, isOdd: false, -1, 580, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 267), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(284, 581, 582, isOdd: false, -1, 583, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 268), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(285, 584, 585, isOdd: false, -1, 586, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 269), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(286, 587, 588, isOdd: false, -1, 589, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 270), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(287, 590, 591, isOdd: false, -1, 592, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 271), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(288, 593, 594, isOdd: false, -1, 595, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 272), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(289, 596, 597, isOdd: false, -1, 598, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 273), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(290, 599, 600, isOdd: false, -1, 601, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 274), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(291, 602, 603, isOdd: false, -1, 604, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 275), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(292, 605, 606, isOdd: false, -1, 607, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 276), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(293, 608, 609, isOdd: false, -1, 610, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 277), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(294, 611, 612, isOdd: false, -1, 613, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 278), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(295, 614, 615, isOdd: false, -1, 616, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 279), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(296, 617, 618, isOdd: false, -1, 619, 15, null, 0, 18, new MaterialResources(0, 0, 18, 0, 0, 0), new MakeItemResult("Accessory", 280), -1, -1, -1, -1));
		_dataArray.Add(new MakeItemSubTypeItem(297, 620, 621, isOdd: false, -1, 622, 15, null, 0, 24, new MaterialResources(0, 0, 0, 24, 0, 0), new MakeItemResult("Accessory", 281), -1, -1, -1, -1));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MakeItemSubTypeItem>(298);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
	}
}
