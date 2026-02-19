using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class EnemyNest : ConfigData<EnemyNestItem, short>
{
	public static class DefKey
	{
		public const short ViciousBeggarsNest = 0;

		public const short ThievesCamp = 1;

		public const short BanditsStronghold = 2;

		public const short TraitorsGang = 3;

		public const short VillainsValley = 4;

		public const short Mixiangzhen = 5;

		public const short MassGrave = 6;

		public const short HereticHome = 7;

		public const short EvilGround = 8;

		public const short Xiuluochang = 9;

		public const short FlurryofDemons = 10;

		public const short DeadEnd = 11;

		public const short HallOfTheRighteous = 12;

		public const short HerosLeague = 13;

		public const short UnchartedTerritory = 14;
	}

	public static class DefValue
	{
		public static EnemyNestItem ViciousBeggarsNest => Instance[(short)0];

		public static EnemyNestItem ThievesCamp => Instance[(short)1];

		public static EnemyNestItem BanditsStronghold => Instance[(short)2];

		public static EnemyNestItem TraitorsGang => Instance[(short)3];

		public static EnemyNestItem VillainsValley => Instance[(short)4];

		public static EnemyNestItem Mixiangzhen => Instance[(short)5];

		public static EnemyNestItem MassGrave => Instance[(short)6];

		public static EnemyNestItem HereticHome => Instance[(short)7];

		public static EnemyNestItem EvilGround => Instance[(short)8];

		public static EnemyNestItem Xiuluochang => Instance[(short)9];

		public static EnemyNestItem FlurryofDemons => Instance[(short)10];

		public static EnemyNestItem DeadEnd => Instance[(short)11];

		public static EnemyNestItem HallOfTheRighteous => Instance[(short)12];

		public static EnemyNestItem HerosLeague => Instance[(short)13];

		public static EnemyNestItem UnchartedTerritory => Instance[(short)14];
	}

	public static EnemyNest Instance = new EnemyNest();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "Members", "Leader", "MonthlyActionId", "AdventureId", "TemplateId", "TipTitle", "TipDesc", "SpawnAmountFactors" };

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
		_dataArray.Add(new EnemyNestItem(0, 0, 1, 0, -1, new List<short> { 228, 229, 230, 231, 232 }, 232, new List<short> { 4, 4, 4, 4, 0 }, 15, 29, 50, 250, 50, 500, 36));
		_dataArray.Add(new EnemyNestItem(1, 0, 2, 0, -1, new List<short> { 233, 234, 235, 236, 237 }, 237, new List<short> { 4, 4, 4, 3, 0 }, 16, 30, 100, 500, 100, 1000, 36));
		_dataArray.Add(new EnemyNestItem(2, 0, 3, 0, -1, new List<short> { 238, 239, 240, 241, 242 }, 242, new List<short> { 4, 4, 3, 3, 0 }, 18, 31, 150, 1000, 200, 2000, 36));
		_dataArray.Add(new EnemyNestItem(3, 4, 5, 0, -1, new List<short>
		{
			243, 244, 245, 246, 247, 248, 249, 250, 251, 252,
			253, 254, 255, 256, 257
		}, -1, new List<short>
		{
			4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
			4, 4, 4, 4, 4
		}, 19, 33, 200, 1500, 300, 3000, 1));
		_dataArray.Add(new EnemyNestItem(4, 0, 6, 0, -1, new List<short> { 258, 259, 260, 261, 262 }, 262, new List<short> { 3, 3, 3, 3, 0 }, 17, 34, 250, 2250, 450, 4500, 36));
		_dataArray.Add(new EnemyNestItem(5, 4, 7, 0, -1, new List<short> { 263, 264, 265, 266, 267 }, 267, new List<short> { 3, 3, 3, 2, 0 }, 20, 35, 300, 3000, 600, 6000, 1));
		_dataArray.Add(new EnemyNestItem(6, 4, 8, 0, -1, new List<short> { 268, 269, 270, 271, 272 }, 272, new List<short> { 3, 3, 2, 2, 0 }, 22, 36, 350, 4000, 800, 8000, 1));
		_dataArray.Add(new EnemyNestItem(7, 9, 10, 0, -1, new List<short> { 273, 274, 275, 276, 277 }, 277, new List<short> { 3, 2, 2, 2, 0 }, 21, 38, 400, 5000, 1000, 10000, 36));
		_dataArray.Add(new EnemyNestItem(8, 9, 11, 0, 6, new List<short> { 278, 279, 280, 281, 282 }, 282, new List<short> { 2, 2, 2, 2, 0 }, 26, 39, 450, 6500, 1300, 13000, 36));
		_dataArray.Add(new EnemyNestItem(9, 4, 12, 0, 5, new List<short> { 283, 284, 285, 286, 287 }, 287, new List<short> { 2, 2, 2, 1, 0 }, 23, 40, 500, 8000, 1600, 16000, 1));
		_dataArray.Add(new EnemyNestItem(10, 9, 13, 0, 4, new List<short> { 288, 289, 290, 291, 292 }, 292, new List<short> { 2, 2, 1, 1, 0 }, 24, 41, 550, 10000, 2000, 20000, 36));
		_dataArray.Add(new EnemyNestItem(11, 9, 14, 0, 3, new List<short> { 293, 294, 295, 296, 297 }, 297, new List<short> { 2, 1, 1, 1, 0 }, 25, 43, 600, 12000, 2400, 24000, 36));
		_dataArray.Add(new EnemyNestItem(12, 15, 16, 1, -1, new List<short> { 307, 308, 309 }, 309, new List<short> { 4, 4, 3 }, 27, 32, -150, 1500, 300, 3000, 36));
		_dataArray.Add(new EnemyNestItem(13, 15, 17, 1, -1, new List<short> { 310, 311, 312 }, 312, new List<short> { 3, 3, 2 }, 28, 37, -350, 5000, 1000, 10000, 36));
		_dataArray.Add(new EnemyNestItem(14, 18, 19, 1, -1, new List<short> { 313, 314, 315 }, 315, new List<short> { 2, 2, 1 }, 29, 42, -550, 10000, 2000, 20000, 36));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<EnemyNestItem>(15);
		CreateItems0();
	}
}
