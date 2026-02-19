using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class Chicken : ConfigData<ChickenItem, short>
{
	public static class DefKey
	{
		public const short King = 63;
	}

	public static class DefValue
	{
		public static ChickenItem King => Instance[(short)63];
	}

	public static Chicken Instance = new Chicken();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "PersonalityType", "FeatureId", "EventActorTemplateId", "TemplateId", "Name", "Desc", "Display", "Grade", "PersonalityValue", "EventDesc" };

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
		_dataArray.Add(new ChickenItem(0, 0, 1, "tex_chicken_clever0", 0, 1, 1, 262, 219, 2));
		_dataArray.Add(new ChickenItem(1, 3, 4, "tex_chicken_clever1", 1, 1, 2, 263, 220, 5));
		_dataArray.Add(new ChickenItem(2, 6, 7, "tex_chicken_clever2", 2, 1, 3, 264, 221, 8));
		_dataArray.Add(new ChickenItem(3, 9, 10, "tex_chicken_clever3", 3, 1, 5, 265, 222, 11));
		_dataArray.Add(new ChickenItem(4, 12, 13, "tex_chicken_clever4", 4, 1, 7, 266, 223, 14));
		_dataArray.Add(new ChickenItem(5, 15, 16, "tex_chicken_clever5", 5, 1, 9, 267, 224, 17));
		_dataArray.Add(new ChickenItem(6, 18, 19, "tex_chicken_clever6", 6, 1, 12, 268, 225, 20));
		_dataArray.Add(new ChickenItem(7, 21, 22, "tex_chicken_clever7", 7, 1, 15, 269, 226, 23));
		_dataArray.Add(new ChickenItem(8, 24, 25, "tex_chicken_clever8", 8, 1, 18, 270, 227, 26));
		_dataArray.Add(new ChickenItem(9, 27, 28, "tex_chicken_lucky0", 0, 5, 1, 271, 228, 29));
		_dataArray.Add(new ChickenItem(10, 30, 31, "tex_chicken_lucky1", 1, 5, 2, 272, 229, 32));
		_dataArray.Add(new ChickenItem(11, 33, 34, "tex_chicken_lucky2", 2, 5, 3, 273, 230, 35));
		_dataArray.Add(new ChickenItem(12, 36, 37, "tex_chicken_lucky3", 3, 5, 5, 274, 231, 38));
		_dataArray.Add(new ChickenItem(13, 39, 40, "tex_chicken_lucky4", 4, 5, 7, 275, 232, 41));
		_dataArray.Add(new ChickenItem(14, 42, 43, "tex_chicken_lucky5", 5, 5, 9, 276, 233, 44));
		_dataArray.Add(new ChickenItem(15, 45, 46, "tex_chicken_lucky6", 6, 5, 12, 277, 234, 47));
		_dataArray.Add(new ChickenItem(16, 48, 49, "tex_chicken_lucky7", 7, 5, 15, 278, 235, 50));
		_dataArray.Add(new ChickenItem(17, 51, 52, "tex_chicken_lucky8", 8, 5, 18, 279, 236, 53));
		_dataArray.Add(new ChickenItem(18, 54, 55, "tex_chicken_perceptive0", 0, 6, 1, 280, 237, 56));
		_dataArray.Add(new ChickenItem(19, 57, 58, "tex_chicken_perceptive1", 1, 6, 2, 281, 238, 59));
		_dataArray.Add(new ChickenItem(20, 60, 61, "tex_chicken_perceptive2", 2, 6, 3, 282, 239, 62));
		_dataArray.Add(new ChickenItem(21, 63, 64, "tex_chicken_perceptive3", 3, 6, 5, 283, 240, 65));
		_dataArray.Add(new ChickenItem(22, 66, 67, "tex_chicken_perceptive4", 4, 6, 7, 284, 241, 68));
		_dataArray.Add(new ChickenItem(23, 69, 70, "tex_chicken_perceptive5", 5, 6, 9, 285, 242, 71));
		_dataArray.Add(new ChickenItem(24, 72, 73, "tex_chicken_perceptive6", 6, 6, 12, 286, 243, 74));
		_dataArray.Add(new ChickenItem(25, 75, 76, "tex_chicken_perceptive7", 7, 6, 15, 287, 244, 77));
		_dataArray.Add(new ChickenItem(26, 78, 79, "tex_chicken_perceptive8", 8, 6, 18, 288, 245, 80));
		_dataArray.Add(new ChickenItem(27, 81, 82, "tex_chicken_firm0", 0, 4, 1, 289, 246, 83));
		_dataArray.Add(new ChickenItem(28, 84, 85, "tex_chicken_firm1", 1, 4, 2, 290, 247, 86));
		_dataArray.Add(new ChickenItem(29, 87, 88, "tex_chicken_firm2", 2, 4, 3, 291, 248, 89));
		_dataArray.Add(new ChickenItem(30, 90, 91, "tex_chicken_firm3", 3, 4, 5, 292, 249, 92));
		_dataArray.Add(new ChickenItem(31, 93, 94, "tex_chicken_firm4", 4, 4, 7, 293, 250, 95));
		_dataArray.Add(new ChickenItem(32, 96, 97, "tex_chicken_firm5", 5, 4, 9, 294, 251, 98));
		_dataArray.Add(new ChickenItem(33, 99, 100, "tex_chicken_firm6", 6, 4, 12, 295, 252, 101));
		_dataArray.Add(new ChickenItem(34, 102, 103, "tex_chicken_firm7", 7, 4, 15, 296, 253, 104));
		_dataArray.Add(new ChickenItem(35, 105, 106, "tex_chicken_firm8", 8, 4, 18, 297, 254, 107));
		_dataArray.Add(new ChickenItem(36, 108, 109, "tex_chicken_calm0", 0, 0, 1, 298, 255, 110));
		_dataArray.Add(new ChickenItem(37, 111, 112, "tex_chicken_calm1", 1, 0, 2, 299, 256, 113));
		_dataArray.Add(new ChickenItem(38, 114, 115, "tex_chicken_calm2", 2, 0, 3, 300, 257, 116));
		_dataArray.Add(new ChickenItem(39, 117, 118, "tex_chicken_calm3", 3, 0, 5, 301, 258, 119));
		_dataArray.Add(new ChickenItem(40, 120, 121, "tex_chicken_calm4", 4, 0, 7, 302, 259, 122));
		_dataArray.Add(new ChickenItem(41, 123, 124, "tex_chicken_calm5", 5, 0, 9, 303, 260, 125));
		_dataArray.Add(new ChickenItem(42, 126, 127, "tex_chicken_calm6", 6, 0, 12, 304, 261, 128));
		_dataArray.Add(new ChickenItem(43, 129, 130, "tex_chicken_calm7", 7, 0, 15, 305, 262, 131));
		_dataArray.Add(new ChickenItem(44, 132, 133, "tex_chicken_calm8", 8, 0, 18, 306, 263, 134));
		_dataArray.Add(new ChickenItem(45, 135, 136, "tex_chicken_enthusiastic0", 0, 2, 1, 307, 264, 137));
		_dataArray.Add(new ChickenItem(46, 138, 139, "tex_chicken_enthusiastic1", 1, 2, 2, 308, 265, 140));
		_dataArray.Add(new ChickenItem(47, 141, 142, "tex_chicken_enthusiastic2", 2, 2, 3, 309, 266, 143));
		_dataArray.Add(new ChickenItem(48, 144, 145, "tex_chicken_enthusiastic3", 3, 2, 5, 310, 267, 146));
		_dataArray.Add(new ChickenItem(49, 147, 148, "tex_chicken_enthusiastic4", 4, 2, 7, 311, 268, 149));
		_dataArray.Add(new ChickenItem(50, 150, 151, "tex_chicken_enthusiastic5", 5, 2, 9, 312, 269, 152));
		_dataArray.Add(new ChickenItem(51, 153, 154, "tex_chicken_enthusiastic6", 6, 2, 12, 313, 270, 155));
		_dataArray.Add(new ChickenItem(52, 156, 157, "tex_chicken_enthusiastic7", 7, 2, 15, 314, 271, 158));
		_dataArray.Add(new ChickenItem(53, 159, 160, "tex_chicken_enthusiastic8", 8, 2, 18, 315, 272, 161));
		_dataArray.Add(new ChickenItem(54, 162, 163, "tex_chicken_brave0", 0, 3, 1, 316, 273, 164));
		_dataArray.Add(new ChickenItem(55, 165, 166, "tex_chicken_brave1", 1, 3, 2, 317, 274, 167));
		_dataArray.Add(new ChickenItem(56, 168, 169, "tex_chicken_brave2", 2, 3, 3, 318, 275, 170));
		_dataArray.Add(new ChickenItem(57, 171, 172, "tex_chicken_brave3", 3, 3, 5, 319, 276, 173));
		_dataArray.Add(new ChickenItem(58, 174, 175, "tex_chicken_brave4", 4, 3, 7, 320, 277, 176));
		_dataArray.Add(new ChickenItem(59, 177, 178, "tex_chicken_brave5", 5, 3, 9, 321, 278, 179));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new ChickenItem(60, 180, 181, "tex_chicken_brave6", 6, 3, 12, 322, 279, 182));
		_dataArray.Add(new ChickenItem(61, 183, 184, "tex_chicken_brave7", 7, 3, 15, 323, 280, 185));
		_dataArray.Add(new ChickenItem(62, 186, 187, "tex_chicken_brave8", 8, 3, 18, 324, 281, 188));
		_dataArray.Add(new ChickenItem(63, 189, 190, "tex_chicken_dawang", 8, 5, 18, 334, -1, 191));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ChickenItem>(64);
		CreateItems0();
		CreateItems1();
	}
}
