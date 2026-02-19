using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TeammateBubble : ConfigData<TeammateBubbleItem, short>
{
	public static class DefKey
	{
		public const short WildAnimal = 155;

		public const short FedAnimal = 156;

		public const short Family = 158;

		public const short Friend = 159;

		public const short Enemy = 160;

		public const short Actor = 161;

		public const short Reactor = 162;

		public const short PartlyInfected = 163;

		public const short CompletelyInfected = 164;

		public const short LegendaryBookShocked = 165;

		public const short LegendaryBookInsane = 166;

		public const short NonEnemyGrave = 167;

		public const short Leader = 168;

		public const short BrokenArea = 187;

		public const short FulongFlame = 188;
	}

	public static class DefValue
	{
		public static TeammateBubbleItem WildAnimal => Instance[(short)155];

		public static TeammateBubbleItem FedAnimal => Instance[(short)156];

		public static TeammateBubbleItem Family => Instance[(short)158];

		public static TeammateBubbleItem Friend => Instance[(short)159];

		public static TeammateBubbleItem Enemy => Instance[(short)160];

		public static TeammateBubbleItem Actor => Instance[(short)161];

		public static TeammateBubbleItem Reactor => Instance[(short)162];

		public static TeammateBubbleItem PartlyInfected => Instance[(short)163];

		public static TeammateBubbleItem CompletelyInfected => Instance[(short)164];

		public static TeammateBubbleItem LegendaryBookShocked => Instance[(short)165];

		public static TeammateBubbleItem LegendaryBookInsane => Instance[(short)166];

		public static TeammateBubbleItem NonEnemyGrave => Instance[(short)167];

		public static TeammateBubbleItem Leader => Instance[(short)168];

		public static TeammateBubbleItem BrokenArea => Instance[(short)187];

		public static TeammateBubbleItem FulongFlame => Instance[(short)188];
	}

	public static TeammateBubble Instance = new TeammateBubble();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"MapStateTemplateId", "MapBlockTemplateId", "CharacterTemplateIdList", "CharacterFeatureTemplateIdList", "AdventureTemplateIdList", "AdventureTypeTemplateIdList", "TemplateId", "Name", "BubbleElementType", "PersonalityType",
		"SpecialDesc0", "SpecialDesc1", "SpecialDesc2", "SpecialDesc3", "SpecialDesc4", "FamilyDesc", "FriendDesc"
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
		_dataArray.Add(new TeammateBubbleItem(0, 0, ETeammateBubbleBubbleElementType.TaiwuVillage, 180, -1, 0, null, null, null, null, 2, 1, 2, 3, 4, 5, 6, 6, new int[5] { 7, 8, 9, 10, 11 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(1, 12, ETeammateBubbleBubbleElementType.City, 180, -1, 1, null, null, null, null, 2, 13, 14, 15, 16, 17, 18, 19, new int[5] { 20, 20, 20, 20, 20 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(2, 21, ETeammateBubbleBubbleElementType.City, 180, -1, 2, null, null, null, null, 2, 22, 23, 24, 25, 26, 27, 28, new int[5] { 29, 29, 29, 29, 29 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(3, 30, ETeammateBubbleBubbleElementType.City, 180, -1, 3, null, null, null, null, 2, 31, 32, 33, 34, 35, 36, 37, new int[5] { 38, 38, 38, 38, 38 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(4, 39, ETeammateBubbleBubbleElementType.City, 180, -1, 4, null, null, null, null, 2, 40, 41, 42, 43, 44, 45, 46, new int[5] { 47, 47, 47, 47, 47 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(5, 48, ETeammateBubbleBubbleElementType.City, 180, -1, 5, null, null, null, null, 2, 49, 50, 51, 52, 53, 54, 55, new int[5] { 56, 56, 56, 56, 56 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(6, 57, ETeammateBubbleBubbleElementType.City, 180, -1, 6, null, null, null, null, 2, 58, 59, 60, 61, 62, 63, 64, new int[5] { 65, 65, 65, 65, 65 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(7, 66, ETeammateBubbleBubbleElementType.City, 180, -1, 7, null, null, null, null, 2, 67, 68, 69, 70, 71, 72, 73, new int[5] { 74, 74, 74, 74, 74 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(8, 75, ETeammateBubbleBubbleElementType.City, 180, -1, 8, null, null, null, null, 2, 76, 77, 78, 79, 80, 81, 82, new int[5] { 83, 83, 83, 83, 83 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(9, 84, ETeammateBubbleBubbleElementType.City, 180, -1, 9, null, null, null, null, 2, 85, 86, 87, 88, 89, 90, 91, new int[5] { 92, 92, 92, 92, 92 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(10, 93, ETeammateBubbleBubbleElementType.City, 180, -1, 10, null, null, null, null, 2, 94, 95, 96, 97, 98, 99, 100, new int[5] { 101, 101, 101, 101, 101 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(11, 102, ETeammateBubbleBubbleElementType.City, 180, -1, 11, null, null, null, null, 2, 103, 104, 105, 106, 107, 108, 109, new int[5] { 110, 110, 110, 110, 110 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(12, 111, ETeammateBubbleBubbleElementType.City, 180, -1, 12, null, null, null, null, 2, 112, 113, 114, 115, 116, 117, 118, new int[5] { 119, 119, 119, 119, 119 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(13, 120, ETeammateBubbleBubbleElementType.City, 180, -1, 13, null, null, null, null, 2, 121, 122, 123, 124, 125, 126, 127, new int[5] { 128, 128, 128, 128, 128 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(14, 129, ETeammateBubbleBubbleElementType.City, 180, -1, 14, null, null, null, null, 2, 130, 131, 132, 133, 134, 135, 136, new int[5] { 137, 137, 137, 137, 137 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(15, 138, ETeammateBubbleBubbleElementType.City, 180, -1, 15, null, null, null, null, 2, 139, 140, 141, 142, 143, 144, 145, new int[5] { 146, 146, 146, 146, 146 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(16, 147, ETeammateBubbleBubbleElementType.Organization, 180, -1, 19, null, null, null, null, 1, 148, 149, 150, 151, 152, 153, 154, new int[5] { 155, 156, 157, 158, 159 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(17, 160, ETeammateBubbleBubbleElementType.Organization, 180, -1, 20, null, null, null, null, 1, 161, 162, 163, 164, 165, 166, 167, new int[5] { 168, 169, 170, 171, 172 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(18, 173, ETeammateBubbleBubbleElementType.Organization, 180, -1, 21, null, null, null, null, 1, 174, 175, 176, 177, 178, 179, 180, new int[5] { 181, 182, 183, 184, 185 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(19, 186, ETeammateBubbleBubbleElementType.Organization, 180, -1, 22, null, null, null, null, 1, 187, 188, 189, 190, 191, 192, 193, new int[5] { 194, 195, 196, 197, 198 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(20, 199, ETeammateBubbleBubbleElementType.Organization, 180, -1, 23, null, null, null, null, 1, 200, 201, 202, 203, 204, 205, 206, new int[5] { 207, 208, 209, 210, 211 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(21, 212, ETeammateBubbleBubbleElementType.Organization, 180, -1, 24, null, null, null, null, 1, 213, 214, 215, 216, 217, 218, 219, new int[5] { 220, 221, 222, 223, 224 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(22, 225, ETeammateBubbleBubbleElementType.Organization, 180, -1, 25, null, null, null, null, 1, 226, 227, 228, 229, 230, 231, 232, new int[5] { 233, 234, 235, 236, 237 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(23, 238, ETeammateBubbleBubbleElementType.Organization, 180, -1, 26, null, null, null, null, 1, 239, 240, 241, 242, 243, 244, 245, new int[5] { 246, 247, 248, 249, 250 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(24, 251, ETeammateBubbleBubbleElementType.Organization, 180, -1, 27, null, null, null, null, 1, 252, 253, 254, 255, 256, 257, 258, new int[5] { 259, 260, 261, 262, 263 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(25, 264, ETeammateBubbleBubbleElementType.Organization, 180, -1, 28, null, null, null, null, 1, 265, 266, 267, 268, 269, 270, 271, new int[5] { 272, 273, 274, 275, 276 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(26, 277, ETeammateBubbleBubbleElementType.Organization, 180, -1, 29, null, null, null, null, 1, 278, 279, 280, 281, 282, 283, 284, new int[5] { 285, 286, 287, 288, 289 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(27, 290, ETeammateBubbleBubbleElementType.Organization, 180, -1, 30, null, null, null, null, 1, 291, 292, 293, 294, 295, 296, 297, new int[5] { 298, 299, 300, 301, 302 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(28, 303, ETeammateBubbleBubbleElementType.Organization, 180, -1, 31, null, null, null, null, 1, 304, 305, 306, 307, 308, 309, 310, new int[5] { 311, 312, 313, 314, 315 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(29, 316, ETeammateBubbleBubbleElementType.Organization, 180, -1, 32, null, null, null, null, 1, 317, 318, 319, 320, 321, 322, 323, new int[5] { 324, 325, 326, 327, 328 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(30, 329, ETeammateBubbleBubbleElementType.Organization, 180, -1, 33, null, null, null, null, 1, 330, 331, 332, 333, 334, 335, 336, new int[5] { 337, 338, 339, 340, 341 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(31, 342, ETeammateBubbleBubbleElementType.Village, 180, -1, -1, null, null, null, null, 6, 343, 344, 345, 346, 347, 348, 349, new int[5] { 350, 351, 352, 353, 354 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(32, 355, ETeammateBubbleBubbleElementType.Chicken, 180, -1, -1, null, null, null, null, 6, 356, 357, 358, 359, 360, 361, 362, new int[5] { 363, 364, 365, 366, 367 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(33, 368, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 4, 8, 9 }, null, 6, 369, 370, 371, 372, 373, 374, 375, new int[5] { 376, 377, 378, 379, 380 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(34, 381, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 5, 7, 10 }, null, 6, 382, 383, 384, 385, 386, 387, 388, new int[5] { 389, 390, 391, 392, 393 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(35, 394, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 144 }, null, 6, 395, 396, 397, 398, 399, 400, 401, new int[5] { 402, 403, 404, 405, 406 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(36, 407, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 3 }, null, 6, 408, 409, 410, 411, 412, 413, 414, new int[5] { 415, 416, 417, 418, 419 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(37, 420, ETeammateBubbleBubbleElementType.SummerCombatMatch, 180, -1, -1, null, null, new List<short>
		{
			0, 1, 2, 11, 12, 13, 14, 15, 16, 17,
			18
		}, null, 6, 421, 422, 423, 424, 425, 426, 427, new int[5] { 428, 429, 430, 431, 432 }, new string[3] { "CombatSkillType", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(38, 433, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 25 }, null, 6, 434, 435, 436, 437, 438, 439, 440, new int[5] { 441, 442, 443, 444, 445 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(39, 446, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 22 }, null, 6, 447, 448, 436, 449, 450, 451, 452, new int[5] { 441, 442, 443, 444, 445 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(40, 453, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 24 }, null, 6, 454, 455, 436, 456, 457, 458, 459, new int[5] { 441, 442, 443, 444, 445 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(41, 460, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 21 }, null, 6, 461, 462, 436, 463, 464, 465, 466, new int[5] { 441, 442, 443, 444, 445 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(42, 467, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 19 }, null, 6, 468, 469, 436, 470, 471, 472, 473, new int[5] { 441, 442, 443, 444, 445 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(43, 474, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 23 }, null, 6, 475, 476, 436, 477, 478, 479, 480, new int[5] { 481, 482, 483, 484, 485 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(44, 486, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 20 }, null, 6, 487, 488, 436, 489, 490, 491, 492, new int[5] { 493, 494, 495, 496, 497 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(45, 498, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 27 }, null, 6, 499, 500, 501, 502, 503, 504, 505, new int[5] { 506, 507, 508, 509, 510 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(46, 511, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 19, null, null, null, null, 6, 512, 513, 514, 515, 516, 517, 518, new int[5] { 519, 520, 521, 522, 523 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(47, 524, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 20, null, null, null, null, 6, 525, 526, 527, 528, 529, 530, 531, new int[5] { 532, 533, 534, 535, 536 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(48, 537, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 21, null, null, null, null, 6, 538, 539, 540, 541, 542, 543, 544, new int[5] { 545, 546, 547, 548, 549 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(49, 550, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 22, null, null, null, null, 6, 551, 552, 553, 554, 555, 556, 557, new int[5] { 558, 559, 560, 561, 562 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(50, 563, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 23, null, null, null, null, 6, 564, 565, 566, 567, 568, 569, 570, new int[5] { 571, 572, 573, 574, 575 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(51, 576, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 24, null, null, null, null, 6, 577, 578, 579, 580, 581, 582, 583, new int[5] { 584, 585, 586, 587, 588 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(52, 589, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 25, null, null, null, null, 6, 590, 591, 592, 593, 594, 595, 596, new int[5] { 597, 598, 599, 600, 601 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(53, 602, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 26, null, null, null, null, 6, 603, 604, 605, 606, 607, 608, 609, new int[5] { 610, 611, 612, 613, 614 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(54, 615, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 27, null, null, null, null, 6, 616, 617, 618, 619, 620, 621, 622, new int[5] { 623, 624, 625, 626, 627 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(55, 628, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 28, null, null, null, null, 6, 629, 630, 631, 632, 633, 634, 635, new int[5] { 636, 637, 638, 639, 640 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(56, 641, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 29, null, null, null, null, 6, 642, 643, 644, 645, 646, 647, 648, new int[5] { 649, 650, 651, 652, 653 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(57, 654, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 30, null, null, null, null, 6, 655, 656, 657, 658, 659, 660, 661, new int[5] { 662, 663, 664, 665, 666 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(58, 667, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 31, null, null, null, null, 6, 668, 669, 670, 671, 672, 673, 674, new int[5] { 675, 676, 677, 678, 679 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(59, 680, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 32, null, null, null, null, 6, 681, 682, 683, 684, 685, 686, 687, new int[5] { 688, 689, 690, 691, 692 }, new string[3] { "", "", "" }));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new TeammateBubbleItem(60, 693, ETeammateBubbleBubbleElementType.SectCombatMatch, 180, -1, 33, null, null, null, null, 6, 694, 695, 696, 697, 698, 699, 700, new int[5] { 701, 702, 703, 704, 705 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(61, 706, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 58 }, null, 6, 707, 708, 709, 710, 711, 712, 712, new int[5] { 713, 714, 715, 716, 717 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(62, 718, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 46 }, null, 6, 719, 720, 721, 722, 723, 724, 724, new int[5] { 725, 726, 727, 728, 729 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(63, 730, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 68 }, null, 6, 731, 732, 733, 734, 735, 736, 736, new int[5] { 737, 738, 739, 740, 741 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(64, 742, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 66 }, null, 6, 743, 744, 745, 746, 747, 748, 748, new int[5] { 749, 750, 751, 752, 753 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(65, 754, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 50 }, null, 6, 755, 756, 757, 758, 759, 760, 760, new int[5] { 761, 762, 763, 764, 765 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(66, 766, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 44 }, null, 6, 767, 768, 769, 770, 771, 772, 772, new int[5] { 773, 774, 775, 776, 777 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(67, 778, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 72 }, null, 6, 779, 780, 781, 782, 783, 784, 784, new int[5] { 785, 786, 787, 788, 789 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(68, 790, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 54 }, null, 6, 791, 792, 793, 794, 795, 796, 796, new int[5] { 797, 798, 799, 800, 801 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(69, 802, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 52 }, null, 6, 803, 804, 805, 806, 807, 808, 808, new int[5] { 809, 810, 811, 812, 813 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(70, 814, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 56 }, null, 6, 815, 816, 817, 818, 819, 820, 820, new int[5] { 821, 822, 823, 824, 825 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(71, 826, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 48 }, null, 6, 827, 828, 829, 830, 831, 832, 832, new int[5] { 833, 834, 835, 836, 837 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(72, 838, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 62 }, null, 6, 839, 840, 841, 842, 843, 844, 844, new int[5] { 845, 846, 847, 848, 849 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(73, 850, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 70 }, null, 6, 851, 852, 853, 854, 855, 856, 856, new int[5] { 857, 858, 859, 860, 861 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(74, 862, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 60 }, null, 6, 863, 864, 865, 866, 867, 868, 868, new int[5] { 869, 870, 871, 872, 873 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(75, 874, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 64 }, null, 6, 875, 876, 877, 878, 879, 880, 880, new int[5] { 881, 882, 883, 884, 885 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(76, 886, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 59 }, null, 6, 887, 888, 889, 890, 891, 892, 892, new int[5] { 893, 894, 895, 896, 897 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(77, 898, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 47 }, null, 6, 899, 900, 901, 902, 903, 904, 904, new int[5] { 905, 906, 907, 908, 909 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(78, 910, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 69 }, null, 6, 911, 912, 913, 914, 915, 916, 916, new int[5] { 917, 918, 919, 920, 921 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(79, 922, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 67 }, null, 6, 923, 924, 925, 926, 927, 928, 928, new int[5] { 929, 930, 931, 932, 933 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(80, 934, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 51 }, null, 6, 935, 936, 937, 938, 939, 940, 940, new int[5] { 941, 942, 943, 944, 945 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(81, 946, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 45 }, null, 6, 947, 948, 949, 950, 951, 952, 952, new int[5] { 953, 954, 955, 956, 957 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(82, 958, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 73 }, null, 6, 959, 960, 961, 962, 963, 964, 964, new int[5] { 965, 966, 967, 968, 969 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(83, 970, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 55 }, null, 6, 971, 972, 973, 974, 975, 976, 976, new int[5] { 977, 978, 979, 980, 981 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(84, 982, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 53 }, null, 6, 983, 984, 985, 986, 987, 988, 988, new int[5] { 989, 990, 991, 992, 993 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(85, 994, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 57 }, null, 6, 995, 996, 997, 998, 999, 1000, 1000, new int[5] { 1001, 1002, 1003, 1004, 1005 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(86, 1006, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 49 }, null, 6, 1007, 1008, 1009, 1010, 1011, 1012, 1012, new int[5] { 1013, 1014, 1015, 1016, 1017 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(87, 1018, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 63 }, null, 6, 1019, 1020, 1021, 1022, 1023, 1024, 1024, new int[5] { 1025, 1026, 1027, 1028, 1029 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(88, 1030, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 71 }, null, 6, 1031, 1032, 1033, 1034, 1035, 1036, 1036, new int[5] { 1037, 1038, 1039, 1040, 1041 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(89, 1042, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 61 }, null, 6, 1043, 1044, 1045, 1046, 1047, 1048, 1048, new int[5] { 1049, 1050, 1051, 1052, 1053 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(90, 1054, ETeammateBubbleBubbleElementType.SettlementAdventure, 180, -1, -1, null, null, new List<short> { 65 }, null, 6, 1055, 1056, 1057, 1058, 1059, 1060, 1060, new int[5] { 1061, 1062, 1063, 1064, 1065 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(91, 1066, ETeammateBubbleBubbleElementType.SwordGrave, 180, -1, -1, null, null, new List<short> { 108 }, null, 3, 1067, 1068, 1069, 1070, 1071, 1072, 1072, new int[5] { 1073, 1073, 1073, 1073, 1073 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(92, 1074, ETeammateBubbleBubbleElementType.SwordGrave, 180, -1, -1, null, null, new List<short> { 109 }, null, 3, 1075, 1076, 1077, 1078, 1079, 1080, 1080, new int[5] { 1081, 1081, 1081, 1081, 1081 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(93, 1082, ETeammateBubbleBubbleElementType.SwordGrave, 180, -1, -1, null, null, new List<short> { 110 }, null, 3, 1083, 1084, 1085, 1086, 1087, 1088, 1088, new int[5] { 1089, 1089, 1089, 1089, 1089 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(94, 1090, ETeammateBubbleBubbleElementType.SwordGrave, 180, -1, -1, null, null, new List<short> { 111 }, null, 3, 1091, 1092, 1093, 1094, 1095, 1096, 1096, new int[5] { 1097, 1097, 1097, 1097, 1097 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(95, 1098, ETeammateBubbleBubbleElementType.SwordGrave, 180, -1, -1, null, null, new List<short> { 112 }, null, 3, 1099, 1100, 1101, 1102, 1103, 1104, 1104, new int[5] { 1105, 1105, 1105, 1105, 1105 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(96, 1106, ETeammateBubbleBubbleElementType.SwordGrave, 180, -1, -1, null, null, new List<short> { 113 }, null, 3, 1107, 1108, 1109, 1110, 1111, 1112, 1112, new int[5] { 1113, 1113, 1113, 1113, 1113 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(97, 1114, ETeammateBubbleBubbleElementType.SwordGrave, 180, -1, -1, null, null, new List<short> { 114 }, null, 3, 1115, 1116, 1117, 1118, 1119, 1120, 1120, new int[5] { 1121, 1121, 1121, 1121, 1121 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(98, 1122, ETeammateBubbleBubbleElementType.SwordGrave, 180, -1, -1, null, null, new List<short> { 115 }, null, 3, 1123, 1124, 1125, 1126, 1127, 1128, 1128, new int[5] { 1129, 1129, 1129, 1129, 1129 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(99, 1130, ETeammateBubbleBubbleElementType.SwordGrave, 180, -1, -1, null, null, new List<short> { 116 }, null, 3, 1131, 1132, 1133, 1134, 1135, 1136, 1136, new int[5] { 1137, 1137, 1137, 1137, 1137 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(100, 1138, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 97 }, null, 3, 1139, 1140, 1141, 1142, 1143, 1144, 1145, new int[5] { 1146, 1147, 1148, 1149, 1150 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(101, 1151, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 98 }, null, 3, 1152, 1153, 1154, 1155, 1156, 1157, 1158, new int[5] { 1159, 1160, 1161, 1162, 1163 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(102, 1164, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 99 }, null, 3, 1165, 1166, 1167, 1168, 1169, 1170, 1171, new int[5] { 1172, 1173, 1174, 1175, 1176 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(103, 1177, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 6 }, null, 3, 1178, 1179, 1180, 1181, 1182, 1183, 1184, new int[5] { 1185, 1186, 1187, 1188, 1189 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(104, 1190, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 100 }, null, 3, 1191, 1192, 1193, 1194, 1195, 1196, 1197, new int[5] { 1198, 1199, 1200, 1201, 1202 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(105, 1203, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 101 }, null, 3, 1204, 1205, 1206, 1207, 1208, 1209, 1210, new int[5] { 1211, 1212, 1213, 1214, 1215 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(106, 1216, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 102 }, null, 3, 1217, 1218, 1219, 1220, 1221, 1222, 1223, new int[5] { 1224, 1225, 1226, 1227, 1228 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(107, 1229, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 103 }, null, 3, 1230, 1231, 1232, 1233, 1234, 1235, 1236, new int[5] { 1237, 1238, 1239, 1240, 1241 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(108, 1242, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 104 }, null, 3, 1243, 1244, 1245, 1246, 1247, 1248, 1249, new int[5] { 1250, 1251, 1252, 1253, 1254 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(109, 1255, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 105 }, null, 3, 1256, 1257, 1258, 1259, 1260, 1261, 1262, new int[5] { 1263, 1264, 1265, 1266, 1267 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(110, 1268, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 106 }, null, 3, 1269, 1270, 1271, 1272, 1273, 1274, 1275, new int[5] { 1276, 1277, 1278, 1279, 1280 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(111, 1281, ETeammateBubbleBubbleElementType.Story, 180, -1, -1, null, null, new List<short> { 107 }, null, 3, 1282, 1283, 1284, 1285, 1286, 1287, 1288, new int[5] { 1289, 1290, 1291, 1292, 1293 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(112, 1294, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 29 }, null, 3, 1295, 1296, 1297, 1298, 1299, 1300, 1301, new int[5] { 1302, 1303, 1304, 1305, 1306 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(113, 1307, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 30 }, null, 3, 1308, 1309, 1310, 1311, 1312, 1313, 1314, new int[5] { 1315, 1316, 1317, 1318, 1319 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(114, 1320, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 31 }, null, 3, 1321, 1322, 1323, 1324, 1325, 1326, 1327, new int[5] { 1328, 1329, 1330, 1331, 1332 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(115, 1333, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 33 }, null, 3, 1334, 1335, 1336, 1337, 1338, 1339, 1340, new int[5] { 1341, 1342, 1343, 1344, 1345 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(116, 1346, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 34 }, null, 3, 1347, 1348, 1349, 1350, 1351, 1352, 1353, new int[5] { 1354, 1355, 1356, 1357, 1358 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(117, 1359, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 35 }, null, 3, 1360, 1361, 1362, 1363, 1364, 1365, 1366, new int[5] { 1367, 1368, 1369, 1370, 1371 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(118, 1372, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 36 }, null, 3, 1373, 1374, 1375, 1376, 1377, 1378, 1379, new int[5] { 1380, 1381, 1382, 1383, 1384 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(119, 1385, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 38 }, null, 3, 1386, 1387, 1388, 1389, 1390, 1391, 1392, new int[5] { 1393, 1394, 1395, 1396, 1397 }, new string[3] { "", "", "" }));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new TeammateBubbleItem(120, 1398, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 39 }, null, 3, 1399, 1400, 1401, 1402, 1403, 1404, 1405, new int[5] { 1406, 1407, 1408, 1409, 1410 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(121, 1411, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 40 }, null, 3, 1412, 1413, 1414, 1415, 1416, 1417, 1418, new int[5] { 1419, 1420, 1421, 1422, 1423 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(122, 1424, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 41 }, null, 3, 1425, 1426, 1427, 1428, 1429, 1430, 1431, new int[5] { 1432, 1433, 1434, 1435, 1436 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(123, 1437, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 43 }, null, 3, 1438, 1439, 1440, 1441, 1442, 1443, 1444, new int[5] { 1445, 1446, 1447, 1448, 1449 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(124, 1450, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 32 }, null, 3, 1451, 1452, 1453, 1454, 1455, 1456, 1457, new int[5] { 1458, 1459, 1460, 1461, 1462 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(125, 1463, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 37 }, null, 3, 1464, 1465, 1466, 1454, 1467, 1468, 1469, new int[5] { 1470, 1471, 1472, 1473, 1474 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(126, 1475, ETeammateBubbleBubbleElementType.EnemyNest, 180, -1, -1, null, null, new List<short> { 42 }, null, 3, 1476, 1477, 1478, 1454, 1479, 1480, 1481, new int[5] { 1482, 1483, 1484, 1485, 1486 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(127, 1487, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 74 }, null, 5, 1488, 1489, 1490, 1491, 1492, 1493, 1494, new int[5] { 1495, 1496, 1497, 1498, 1499 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(128, 1500, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 75 }, null, 5, 1501, 1502, 1503, 1504, 1505, 1506, 1507, new int[5] { 1508, 1509, 1510, 1511, 1512 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(129, 1513, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 76 }, null, 5, 1514, 1515, 1516, 1517, 1518, 1519, 1520, new int[5] { 1521, 1522, 1523, 1524, 1525 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(130, 1526, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 77 }, null, 5, 1527, 1528, 1529, 1530, 1531, 1532, 1533, new int[5] { 1534, 1535, 1536, 1537, 1538 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(131, 1539, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 78 }, null, 5, 1540, 1541, 1542, 1543, 1544, 1545, 1546, new int[5] { 1547, 1548, 1549, 1550, 1551 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(132, 1552, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 79 }, null, 5, 1553, 1554, 1555, 1556, 1557, 1558, 1559, new int[5] { 1560, 1561, 1562, 1563, 1564 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(133, 1565, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 80 }, null, 5, 1566, 1567, 1568, 1569, 1570, 1571, 1572, new int[5] { 1573, 1574, 1575, 1576, 1577 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(134, 1578, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 81 }, null, 5, 1579, 1580, 1581, 1582, 1583, 1584, 1585, new int[5] { 1586, 1587, 1588, 1589, 1590 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(135, 1591, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 82 }, null, 5, 1592, 1593, 1594, 1595, 1596, 1597, 1598, new int[5] { 1599, 1600, 1601, 1602, 1603 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(136, 1604, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 83 }, null, 5, 1605, 1606, 1607, 1608, 1609, 1610, 1611, new int[5] { 1612, 1613, 1614, 1615, 1616 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(137, 1617, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 84 }, null, 5, 1618, 1619, 1620, 1621, 1622, 1623, 1624, new int[5] { 1625, 1626, 1627, 1628, 1629 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(138, 1630, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 85 }, null, 5, 1631, 1632, 1633, 1634, 1635, 1636, 1637, new int[5] { 1638, 1639, 1640, 1641, 1642 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(139, 1643, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 86 }, null, 5, 1644, 1645, 1646, 1647, 1648, 1649, 1650, new int[5] { 1651, 1652, 1653, 1654, 1655 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(140, 1656, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 87 }, null, 5, 1657, 1658, 1659, 1660, 1661, 1662, 1663, new int[5] { 1664, 1665, 1666, 1667, 1668 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(141, 1669, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 88 }, null, 5, 1670, 1671, 1672, 1673, 1674, 1675, 1676, new int[5] { 1677, 1678, 1679, 1680, 1681 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(142, 1682, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 89 }, null, 5, 1683, 1684, 1685, 1686, 1648, 1687, 1688, new int[5] { 1689, 1690, 1691, 1692, 1693 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(143, 1694, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 90 }, null, 5, 1695, 1696, 1697, 1698, 1699, 1700, 1701, new int[5] { 1702, 1703, 1704, 1705, 1706 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(144, 1707, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 91 }, null, 5, 1708, 1709, 1710, 1711, 1712, 1713, 1714, new int[5] { 1715, 1716, 1717, 1718, 1719 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(145, 1720, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 95 }, null, 5, 1721, 1722, 1723, 1724, 1725, 1726, 1727, new int[5] { 1728, 1729, 1730, 1731, 1732 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(146, 1733, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 96 }, null, 5, 1734, 1735, 1736, 1737, 1738, 1739, 1740, new int[5] { 1741, 1742, 1743, 1744, 1745 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(147, 1746, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 92 }, null, 5, 1747, 1748, 1749, 1750, 1751, 1752, 1753, new int[5] { 1754, 1755, 1756, 1757, 1758 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(148, 1759, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 93 }, null, 5, 1760, 1761, 1762, 1763, 1764, 1765, 1766, new int[5] { 1767, 1768, 1769, 1770, 1771 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(149, 1772, ETeammateBubbleBubbleElementType.Treasure, 180, -1, -1, null, null, new List<short> { 94 }, null, 5, 1773, 1774, 1775, 1776, 1777, 1778, 1779, new int[5] { 1780, 1781, 1782, 1783, 1784 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(150, 1785, ETeammateBubbleBubbleElementType.Queerbook, 180, -1, -1, null, null, new List<short>
		{
			145, 146, 147, 148, 149, 150, 151, 152, 153, 154,
			155, 156, 157, 158
		}, null, 1, 1786, 1787, 1788, 1789, 1790, 1791, 1792, new int[5] { 1793, 1794, 1795, 1796, 1797 }, new string[3] { "Adventure", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(151, 1798, ETeammateBubbleBubbleElementType.None, 180, -1, -1, null, null, null, null, 0, 1799, 1800, 1801, 1802, 1803, 1804, 1805, new int[5] { 1806, 1807, 1808, 1809, 1810 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(152, 1811, ETeammateBubbleBubbleElementType.None, 180, -1, -1, new List<short>
		{
			228, 229, 230, 231, 232, 233, 234, 235, 236, 237,
			238, 239, 240, 241, 242, 243, 244, 245, 246, 247,
			248, 249, 250, 251, 252, 253, 254, 255, 256, 257,
			258, 259, 260, 261, 262, 263, 264, 265, 266, 267,
			268, 269, 270, 271, 272, 273, 274, 275, 276, 277,
			278, 279, 280, 281, 282, 283, 284, 285, 286, 287,
			288, 289, 290, 291, 292, 293, 294, 295, 296, 297
		}, null, null, null, 0, 1812, 1813, 1814, 1815, 1816, 1817, 1818, new int[5] { 1819, 1820, 1821, 1822, 1823 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(153, 1824, ETeammateBubbleBubbleElementType.None, 180, -1, -1, new List<short> { 307, 308, 309, 310, 311, 312, 313, 314, 315 }, null, null, null, 0, 1825, 1826, 1827, 1828, 1829, 1830, 1831, new int[5] { 1832, 1833, 1834, 1835, 1836 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(154, 1837, ETeammateBubbleBubbleElementType.None, 180, -1, -1, new List<short> { 298, 299, 300, 301, 302, 303, 304, 305, 306 }, null, null, null, 0, 1838, 1839, 1840, 1841, 1842, 1843, 1844, new int[5] { 1845, 1846, 1847, 1848, 1849 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(155, 1850, ETeammateBubbleBubbleElementType.Animal, 180, -1, -1, new List<short>
		{
			210, 211, 212, 213, 214, 215, 216, 217, 218, 219,
			220, 221, 222, 223, 224, 225, 226, 227
		}, null, null, null, 0, 1851, 1852, 1853, 1854, 1855, 1856, 1856, new int[5] { 1857, 1858, 1859, 1860, 1861 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(156, 1862, ETeammateBubbleBubbleElementType.Animal, 180, -1, -1, null, null, null, null, 0, 1863, 1864, 1865, 1866, 1867, 1868, 1869, new int[5] { 1870, 1871, 1872, 1873, 1874 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(157, 1875, ETeammateBubbleBubbleElementType.Caravan, 180, -1, -1, null, null, null, null, 0, 1876, 1877, 1878, 1879, 1880, 1881, 1882, new int[5] { 1883, 1884, 1885, 1886, 1887 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(158, 1888, ETeammateBubbleBubbleElementType.RelatedCharacter, 180, -1, -1, null, null, null, null, 5, 1889, 1890, 1891, 1892, 1893, 1894, 1895, new int[5] { 1896, 1897, 1898, 1899, 1900 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(159, 1901, ETeammateBubbleBubbleElementType.RelatedCharacter, 180, -1, -1, null, null, null, null, 5, 1902, 1903, 1904, 1905, 1906, 1907, 1908, new int[5] { 1909, 1910, 1911, 1912, 1913 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(160, 1914, ETeammateBubbleBubbleElementType.RelatedCharacter, 180, -1, -1, null, null, null, null, 3, 1915, 1916, 1917, 1918, 1919, 1920, 1921, new int[5] { 1922, 1923, 1924, 1925, 1926 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(161, 1927, ETeammateBubbleBubbleElementType.None, 180, -1, -1, null, null, null, null, 1, 1928, 1929, 1930, 1931, 1932, 1933, 1934, new int[5] { 1935, 1936, 1937, 1938, 1939 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(162, 1940, ETeammateBubbleBubbleElementType.None, 180, -1, -1, null, null, null, null, 1, 1941, 1942, 1943, 1944, 1945, 1946, 1947, new int[5] { 1948, 1949, 1950, 1951, 1952 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(163, 1953, ETeammateBubbleBubbleElementType.Infected, 180, -1, -1, null, new List<short> { 217 }, null, null, 3, 1954, 1955, 1956, 1957, 1958, 1959, 1960, new int[5] { 1961, 1962, 1963, 1964, 1965 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(164, 1966, ETeammateBubbleBubbleElementType.Infected, 180, -1, -1, null, new List<short> { 218 }, null, null, 3, 1967, 1968, 1969, 1970, 1971, 1972, 1973, new int[5] { 1974, 1975, 1976, 1977, 1978 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(165, 1979, ETeammateBubbleBubbleElementType.LegendaryBookInsane, 180, -1, -1, null, new List<short> { 204, 205 }, null, null, 3, 1980, 1981, 1982, 1983, 1984, 1985, 1986, new int[5] { 1987, 1988, 1989, 1990, 1991 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(166, 1992, ETeammateBubbleBubbleElementType.LegendaryBookInsane, 180, -1, -1, null, null, null, null, 3, 1993, 1994, 1995, 1996, 1997, 1998, 1999, new int[5] { 2000, 2001, 2002, 2003, 2004 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(167, 2005, ETeammateBubbleBubbleElementType.Grave, 180, -1, -1, null, null, null, null, 5, 2006, 2007, 2008, 2009, 2010, 2011, 2012, new int[5] { 2013, 2014, 2015, 2016, 2017 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(168, 2018, ETeammateBubbleBubbleElementType.SectLeader, 180, -1, -1, null, new List<short> { 405 }, null, null, 1, 2019, 2020, 2021, 2022, 2023, 2024, 2025, new int[5] { 2026, 2027, 2028, 2029, 2030 }, new string[3] { "Settlement", "OrgGrade", "" }));
		_dataArray.Add(new TeammateBubbleItem(169, 2031, ETeammateBubbleBubbleElementType.Cricket, 180, -1, -1, null, null, null, null, 5, 2032, 2033, 2034, 2035, 2036, 2037, 2038, new int[5] { 2039, 2040, 2041, 2042, 2043 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(170, 2044, ETeammateBubbleBubbleElementType.Worker, 180, -1, -1, null, null, null, null, 5, 2045, 2046, 2047, 2048, 2049, 2050, 2051, new int[5] { 2052, 2053, 2054, 2055, 2056 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(171, 2057, ETeammateBubbleBubbleElementType.Lost, 180, -1, -1, null, null, null, null, 5, 2058, 2059, 2060, 2061, 2062, 2063, 2063, new int[5] { 2064, 2065, 2066, 2067, 2068 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(172, 2069, ETeammateBubbleBubbleElementType.Traveling, 180, 1, -1, null, null, null, null, 4, 2070, 2071, 2072, 2073, 2074, 2075, 2076, new int[5] { 2077, 2077, 2077, 2077, 2077 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(173, 2078, ETeammateBubbleBubbleElementType.Traveling, 180, 2, -1, null, null, null, null, 4, 2079, 2080, 2081, 2082, 2083, 2084, 2085, new int[5] { 2086, 2086, 2086, 2086, 2086 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(174, 2087, ETeammateBubbleBubbleElementType.Traveling, 180, 3, -1, null, null, null, null, 4, 2088, 2089, 2090, 2091, 2092, 2093, 2094, new int[5] { 2095, 2095, 2095, 2095, 2095 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(175, 2096, ETeammateBubbleBubbleElementType.Traveling, 180, 4, -1, null, null, null, null, 4, 2097, 2098, 2099, 2100, 2101, 2102, 2103, new int[5] { 2104, 2104, 2104, 2104, 2104 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(176, 2105, ETeammateBubbleBubbleElementType.Traveling, 180, 5, -1, null, null, null, null, 4, 2106, 2107, 2108, 2109, 2110, 2111, 2112, new int[5] { 2113, 2113, 2113, 2113, 2113 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(177, 2114, ETeammateBubbleBubbleElementType.Traveling, 180, 6, -1, null, null, null, null, 4, 2115, 2116, 2117, 2118, 2119, 2120, 2121, new int[5] { 2122, 2122, 2122, 2122, 2122 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(178, 2123, ETeammateBubbleBubbleElementType.Traveling, 180, 7, -1, null, null, null, null, 4, 2124, 2125, 2126, 2127, 2128, 2129, 2130, new int[5] { 2131, 2131, 2131, 2131, 2131 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(179, 2132, ETeammateBubbleBubbleElementType.Traveling, 180, 8, -1, null, null, null, null, 4, 2133, 2134, 2135, 2136, 2137, 2138, 2139, new int[5] { 2140, 2140, 2140, 2140, 2140 }, new string[3] { "", "", "" }));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new TeammateBubbleItem(180, 2141, ETeammateBubbleBubbleElementType.Traveling, 180, 9, -1, null, null, null, null, 4, 2142, 2143, 2144, 2145, 2146, 2147, 2148, new int[5] { 2149, 2149, 2149, 2149, 2149 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(181, 2150, ETeammateBubbleBubbleElementType.Traveling, 180, 10, -1, null, null, null, null, 4, 2151, 2152, 2153, 2154, 2155, 2156, 2157, new int[5] { 2158, 2158, 2158, 2158, 2158 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(182, 2159, ETeammateBubbleBubbleElementType.Traveling, 180, 11, -1, null, null, null, null, 4, 2160, 2161, 2162, 2163, 2164, 2165, 2166, new int[5] { 2167, 2167, 2167, 2167, 2167 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(183, 2168, ETeammateBubbleBubbleElementType.Traveling, 180, 12, -1, null, null, null, null, 4, 2169, 2170, 2171, 2172, 2173, 2174, 2175, new int[5] { 2176, 2176, 2176, 2176, 2176 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(184, 2177, ETeammateBubbleBubbleElementType.Traveling, 180, 13, -1, null, null, null, null, 4, 2178, 2179, 2180, 2181, 2182, 2183, 2184, new int[5] { 2185, 2185, 2185, 2185, 2185 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(185, 2186, ETeammateBubbleBubbleElementType.Traveling, 180, 14, -1, null, null, null, null, 4, 2187, 2188, 2189, 2190, 2191, 2192, 2193, new int[5] { 2194, 2194, 2194, 2194, 2194 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(186, 2195, ETeammateBubbleBubbleElementType.Traveling, 180, 15, -1, null, null, null, null, 4, 2196, 2197, 2198, 2199, 2200, 2201, 2202, new int[5] { 2203, 2203, 2203, 2203, 2203 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(187, 2204, ETeammateBubbleBubbleElementType.DestroyedArea, 180, -1, -1, null, null, null, null, 4, 2205, 2206, 2207, 2208, 2209, 2210, 2210, new int[5] { 2211, 2212, 2213, 2214, 2215 }, new string[3] { "", "", "" }));
		_dataArray.Add(new TeammateBubbleItem(188, 2216, ETeammateBubbleBubbleElementType.StoryMapblockEffect, 180, -1, -1, null, null, null, null, 3, 2217, 2218, 2219, 2220, 2221, 2222, 2222, new int[5] { 2223, 2224, 2225, 2226, 2227 }, new string[3] { "", "", "" }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TeammateBubbleItem>(189);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
	}
}
