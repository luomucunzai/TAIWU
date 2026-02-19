using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class MonthlyNotificationSortingGroup : ConfigData<MonthlyNotificationSortingGroupItem, short>
{
	public static MonthlyNotificationSortingGroup Instance = new MonthlyNotificationSortingGroup();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc" };

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
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(0, 0, 1, 0, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(1, 2, 3, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(2, 4, 5, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(3, 6, 7, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(4, 8, 9, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(5, 10, 11, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(6, 12, 13, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(7, 14, 15, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(8, 16, 17, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(9, 18, 19, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(10, 20, 21, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(11, 22, 23, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(12, 24, 25, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(13, 26, 27, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(14, 28, 29, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(15, 30, 31, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(16, 32, 33, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(17, 34, 35, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(18, 36, 37, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(19, 38, 39, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(20, 40, 41, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(21, 42, 43, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(22, 44, 45, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(23, 46, 47, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(24, 48, 49, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(25, 50, 51, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(26, 52, 53, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(27, 54, 55, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(28, 56, 57, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(29, 58, 59, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(30, 60, 61, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(31, 62, 63, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(32, 64, 65, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(33, 66, 67, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(34, 68, 69, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(35, 70, 71, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(36, 72, 73, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(37, 74, 75, 10, onTop: true, hidden: true, 2305890u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(38, 76, 77, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(39, 78, 79, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(40, 80, 81, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(41, 82, 83, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(42, 84, 85, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(43, 86, 87, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(44, 88, 89, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(45, 90, 91, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(46, 92, 93, 10, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(47, 94, 95, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(48, 96, 97, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(49, 98, 99, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(50, 100, 101, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(51, 102, 103, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(52, 104, 105, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(53, 106, 107, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(54, 108, 109, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(55, 110, 111, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(56, 112, 113, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(57, 114, 115, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(58, 116, 117, 9000, onTop: true, hidden: true, 2764950u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(59, 118, 119, 9000, onTop: true, hidden: true, 2764950u));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(60, 120, 121, 800, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(61, 122, 123, 800, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(62, 124, 125, 800, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(63, 126, 127, 800, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(64, 128, 129, 800, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(65, 130, 131, 800, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(66, 132, 133, 800, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(67, 134, 135, 800, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(68, 136, 137, 1200, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(69, 138, 139, 1200, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(70, 140, 141, 1200, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(71, 142, 143, 1200, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(72, 144, 145, 1200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(73, 146, 147, 1200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(74, 148, 149, 1200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(75, 150, 151, 1300, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(76, 152, 153, 1315, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(77, 154, 155, 1306, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(78, 156, 157, 1302, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(79, 158, 159, 1310, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(80, 160, 161, 1304, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(81, 162, 163, 1312, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(82, 164, 165, 1311, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(83, 166, 167, 1307, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(84, 168, 169, 1303, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(85, 170, 171, 1100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(86, 172, 173, 1100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(87, 174, 175, 1100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(88, 176, 177, 1100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(89, 178, 179, 1100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(90, 180, 181, 900, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(91, 182, 183, 900, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(92, 184, 185, 900, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(93, 186, 187, 900, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(94, 188, 189, 900, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(95, 190, 191, 900, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(96, 192, 193, 900, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(97, 194, 195, 900, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(98, 196, 197, 900, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(99, 198, 199, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(100, 200, 201, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(101, 202, 203, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(102, 204, 205, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(103, 206, 207, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(104, 208, 209, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(105, 210, 211, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(106, 212, 213, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(107, 214, 215, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(108, 216, 217, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(109, 218, 219, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(110, 220, 221, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(111, 222, 223, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(112, 224, 225, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(113, 226, 227, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(114, 228, 229, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(115, 230, 231, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(116, 232, 233, 1000, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(117, 234, 235, 700, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(118, 236, 237, 700, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(119, 238, 239, 700, onTop: true, hidden: true, 0u));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(120, 240, 241, 700, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(121, 242, 243, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(122, 244, 245, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(123, 246, 247, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(124, 248, 249, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(125, 250, 251, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(126, 252, 253, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(127, 254, 255, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(128, 256, 257, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(129, 258, 259, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(130, 260, 261, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(131, 262, 263, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(132, 264, 265, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(133, 266, 267, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(134, 268, 269, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(135, 270, 271, 500, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(136, 272, 273, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(137, 274, 275, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(138, 276, 277, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(139, 278, 279, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(140, 280, 281, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(141, 282, 283, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(142, 284, 285, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(143, 286, 287, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(144, 288, 289, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(145, 290, 291, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(146, 292, 293, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(147, 294, 295, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(148, 296, 297, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(149, 298, 299, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(150, 300, 301, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(151, 302, 303, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(152, 304, 305, 100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(153, 306, 307, 100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(154, 308, 309, 100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(155, 310, 311, 100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(156, 312, 313, 100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(157, 314, 315, 200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(158, 316, 317, 200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(159, 318, 319, 200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(160, 320, 321, 200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(161, 322, 323, 200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(162, 324, 325, 200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(163, 326, 327, 200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(164, 328, 329, 200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(165, 330, 331, 1, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(166, 332, 333, 1, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(167, 334, 335, 1, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(168, 336, 337, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(169, 338, 339, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(170, 340, 341, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(171, 342, 343, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(172, 344, 345, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(173, 346, 347, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(174, 348, 349, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(175, 350, 351, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(176, 352, 353, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(177, 354, 355, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(178, 356, 357, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(179, 358, 359, 300, onTop: true, hidden: true, 0u));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(180, 360, 361, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(181, 362, 363, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(182, 364, 365, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(183, 366, 367, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(184, 368, 369, 400, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(185, 370, 371, 400, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(186, 372, 373, 400, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(187, 374, 375, 400, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(188, 376, 377, 400, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(189, 378, 379, 400, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(190, 380, 381, 400, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(191, 382, 383, 600, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(192, 384, 385, 900, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(193, 386, 387, 400, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(194, 388, 389, 1314, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(195, 390, 391, 1309, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(196, 392, 393, 601, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(197, 394, 395, 100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(198, 396, 397, 200, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(199, 398, 399, 1316, onTop: true, hidden: false, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(200, 400, 401, 100, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(201, 402, 403, 1320, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(202, 404, 405, 300, onTop: true, hidden: true, 0u));
		_dataArray.Add(new MonthlyNotificationSortingGroupItem(203, 406, 407, 600, onTop: true, hidden: true, 0u));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MonthlyNotificationSortingGroupItem>(204);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
	}
}
