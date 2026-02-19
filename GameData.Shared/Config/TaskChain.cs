using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TaskChain : ConfigData<TaskChainItem, int>
{
	public static class DefKey
	{
		public const int MainStory = 0;

		public const int SideQuest_Construction = 14;

		public const int SectMainStory_Kongsang = 27;

		public const int SectMainStory_Xuehou = 28;

		public const int SectMainStory_Xuehou_Jixi = 29;

		public const int SectMainStory_Shaolin = 30;

		public const int SectMainStory_Xuannv = 31;

		public const int SectMainStory_Wudang = 32;

		public const int SectMainStory_Yuanshan = 33;

		public const int SectMainStory_Shixiang = 34;

		public const int SectMainStory_Jingang = 35;

		public const int SectMainStory_Wuxian = 36;

		public const int SectMainStory_Emei = 37;

		public const int SectMainStory_Jieqing = 38;

		public const int SectMainStory_Jieqing_Stones = 39;

		public const int SectMainStory_Ranshan = 40;

		public const int SectMainStory_Wudang_HeavenlyTree = 41;

		public const int PlayerShadowInMirrorChain = 42;

		public const int CrossArchive_MainPlotChain = 43;

		public const int CrossArchive_FetchPast = 44;

		public const int LoongDLC = 45;

		public const int LoongDLCCaptureLoong = 46;

		public const int LoongDLCNurtureJiao = 47;

		public const int SectMainStory_Baihua = 48;

		public const int SectMainStory_Ranshan_Sanshi = 49;

		public const int SectMainStory_Baihua_Combat = 50;

		public const int SectMainStory_Baihua_Relationship = 51;

		public const int SectMainStory_Fulong = 52;

		public const int ChickenMap = 53;

		public const int SectMainStory_Zhujian = 54;

		public const int SectMainStory_ZhujianHeritage = 55;

		public const int SectMainStory_RemakeEmei = 56;

		public const int PlantTrees = 57;
	}

	public static class DefValue
	{
		public static TaskChainItem MainStory => Instance[0];

		public static TaskChainItem SideQuest_Construction => Instance[14];

		public static TaskChainItem SectMainStory_Kongsang => Instance[27];

		public static TaskChainItem SectMainStory_Xuehou => Instance[28];

		public static TaskChainItem SectMainStory_Xuehou_Jixi => Instance[29];

		public static TaskChainItem SectMainStory_Shaolin => Instance[30];

		public static TaskChainItem SectMainStory_Xuannv => Instance[31];

		public static TaskChainItem SectMainStory_Wudang => Instance[32];

		public static TaskChainItem SectMainStory_Yuanshan => Instance[33];

		public static TaskChainItem SectMainStory_Shixiang => Instance[34];

		public static TaskChainItem SectMainStory_Jingang => Instance[35];

		public static TaskChainItem SectMainStory_Wuxian => Instance[36];

		public static TaskChainItem SectMainStory_Emei => Instance[37];

		public static TaskChainItem SectMainStory_Jieqing => Instance[38];

		public static TaskChainItem SectMainStory_Jieqing_Stones => Instance[39];

		public static TaskChainItem SectMainStory_Ranshan => Instance[40];

		public static TaskChainItem SectMainStory_Wudang_HeavenlyTree => Instance[41];

		public static TaskChainItem PlayerShadowInMirrorChain => Instance[42];

		public static TaskChainItem CrossArchive_MainPlotChain => Instance[43];

		public static TaskChainItem CrossArchive_FetchPast => Instance[44];

		public static TaskChainItem LoongDLC => Instance[45];

		public static TaskChainItem LoongDLCCaptureLoong => Instance[46];

		public static TaskChainItem LoongDLCNurtureJiao => Instance[47];

		public static TaskChainItem SectMainStory_Baihua => Instance[48];

		public static TaskChainItem SectMainStory_Ranshan_Sanshi => Instance[49];

		public static TaskChainItem SectMainStory_Baihua_Combat => Instance[50];

		public static TaskChainItem SectMainStory_Baihua_Relationship => Instance[51];

		public static TaskChainItem SectMainStory_Fulong => Instance[52];

		public static TaskChainItem ChickenMap => Instance[53];

		public static TaskChainItem SectMainStory_Zhujian => Instance[54];

		public static TaskChainItem SectMainStory_ZhujianHeritage => Instance[55];

		public static TaskChainItem SectMainStory_RemakeEmei => Instance[56];

		public static TaskChainItem PlantTrees => Instance[57];
	}

	public static TaskChain Instance = new TaskChain();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "NextTaskChain", "TaskList", "StartConditions", "RemoveCondtions", "Sect", "TemplateId", "Group", "Type", "Name" };

	internal override int ToInt(int value)
	{
		return value;
	}

	internal override int ToTemplateId(int value)
	{
		return value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new TaskChainItem(0, ETaskChainGroup.MainStory, ETaskChainType.Line, 1, 28, -1, new List<int>
		{
			2, 3, 4, 5, 19, 20, 21, 235, 24, 25,
			26, 27, 30, 48, 50, 52, 64, 67, 68, 69,
			70, 71, 72, 74, 75, 76, 77
		}, new List<int>(), new List<int>(), 0, 0));
		_dataArray.Add(new TaskChainItem(1, ETaskChainGroup.MainStory, ETaskChainType.Parallel, 16, 25, -1, new List<int> { 33, 34, 35, 36, 37, 38, 39, 40, 41, 42 }, new List<int>(), new List<int>(), 1, 0));
		_dataArray.Add(new TaskChainItem(2, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 21, 23, -1, new List<int> { 53 }, new List<int>(), new List<int>(), 24, 0));
		_dataArray.Add(new TaskChainItem(3, ETaskChainGroup.MainStory, ETaskChainType.Parallel, 28, 29, -1, new List<int> { 78, 79, 80, 81, 82, 83, 84, 85, 86, 87 }, new List<int>(), new List<int>(), 3, 0));
		_dataArray.Add(new TaskChainItem(4, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 1, 2, -1, new List<int> { 0 }, new List<int>(), new List<int>(), 4, 0));
		_dataArray.Add(new TaskChainItem(5, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 1, 2, -1, new List<int> { 1 }, new List<int>(), new List<int>(), 5, 0));
		_dataArray.Add(new TaskChainItem(6, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 3, 5, -1, new List<int> { 6, 7, 8, 9 }, new List<int>(), new List<int>(), 6, 0));
		_dataArray.Add(new TaskChainItem(7, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 4, 5, -1, new List<int> { 10, 11 }, new List<int>(), new List<int>(), 7, 0));
		_dataArray.Add(new TaskChainItem(8, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 4, 5, -1, new List<int> { 16 }, new List<int>(), new List<int>(), 8, 0));
		_dataArray.Add(new TaskChainItem(9, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 4, 5, -1, new List<int> { 15 }, new List<int>(), new List<int>(), 9, 0));
		_dataArray.Add(new TaskChainItem(10, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 5, 6, -1, new List<int> { 17 }, new List<int>(), new List<int>(), 10, 0));
		_dataArray.Add(new TaskChainItem(11, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 5, 6, -1, new List<int> { 18 }, new List<int>(), new List<int>(), 11, 0));
		_dataArray.Add(new TaskChainItem(12, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 4, 5, -1, new List<int> { 12 }, new List<int>(), new List<int>(), 12, 0));
		_dataArray.Add(new TaskChainItem(13, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 5, 6, -1, new List<int> { 13 }, new List<int>(), new List<int>(), 13, 0));
		_dataArray.Add(new TaskChainItem(14, ETaskChainGroup.OptionalTasks, ETaskChainType.Parallel, 9, 10, -1, new List<int> { 22, 23 }, new List<int>(), new List<int>(), 14, 0));
		_dataArray.Add(new TaskChainItem(15, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 12, 13, -1, new List<int> { 28 }, new List<int>(), new List<int>(), 15, 0));
		_dataArray.Add(new TaskChainItem(16, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 13, 26, -1, new List<int> { 29 }, new List<int>(), new List<int>(), 16, 0));
		_dataArray.Add(new TaskChainItem(17, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 14, 15, -1, new List<int> { 32 }, new List<int>(), new List<int>(), 17, 0));
		_dataArray.Add(new TaskChainItem(18, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 16, 19, -1, new List<int> { 31, 44, 45, 46, 47 }, new List<int>(), new List<int>(), 18, 0));
		_dataArray.Add(new TaskChainItem(19, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 99, 99, -1, new List<int>(), new List<int>(), new List<int>(), 19, 0));
		_dataArray.Add(new TaskChainItem(20, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 19, 99, -1, new List<int> { 88, 49 }, new List<int>(), new List<int>(), 20, 0));
		_dataArray.Add(new TaskChainItem(21, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 20, 28, -1, new List<int> { 51 }, new List<int>(), new List<int>(), 21, 0));
		_dataArray.Add(new TaskChainItem(22, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 22, 28, -1, new List<int> { 65, 66 }, new List<int>(), new List<int>(), 22, 0));
		_dataArray.Add(new TaskChainItem(23, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 16, 25, -1, new List<int> { 43 }, new List<int>(), new List<int>(), 23, 0));
		_dataArray.Add(new TaskChainItem(24, ETaskChainGroup.OptionalTasks, ETaskChainType.Parallel, 21, 28, -1, new List<int> { 54, 55, 56, 57, 58, 59, 60, 61, 62, 63 }, new List<int>(), new List<int>(), 2, 0));
		_dataArray.Add(new TaskChainItem(25, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 5, 6, -1, new List<int> { 14 }, new List<int>(), new List<int>(), 25, 0));
		_dataArray.Add(new TaskChainItem(26, ETaskChainGroup.OptionalTasks, ETaskChainType.Parallel, 9, 28, -1, new List<int>(), new List<int> { 230 }, new List<int> { 246 }, 26, 0));
		_dataArray.Add(new TaskChainItem(27, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int>
		{
			105, 106, 107, 108, 109, 110, 111, 112, 113, 114,
			115, 116, 117, 118
		}, new List<int>(), new List<int>(), 27, 10));
		_dataArray.Add(new TaskChainItem(28, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int> { 119, 120, 121, 122, 123, 124, 125, 126, 134, 135 }, new List<int>(), new List<int>(), 28, 15));
		_dataArray.Add(new TaskChainItem(29, ETaskChainGroup.SectMainStory, ETaskChainType.Parallel, 0, 99, -1, new List<int> { 127, 128, 129, 130, 131, 132, 133 }, new List<int>(), new List<int>(), 28, 15));
		_dataArray.Add(new TaskChainItem(30, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int>
		{
			136, 220, 137, 221, 222, 223, 224, 225, 226, 227,
			228, 229, 231, 230, 138
		}, new List<int>(), new List<int>(), 29, 1));
		_dataArray.Add(new TaskChainItem(31, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int>
		{
			139, 140, 141, 142, 143, 144, 145, 146, 256, 147,
			148, 246, 253, 247, 248, 249, 250, 251, 252
		}, new List<int>(), new List<int>(), 30, 8));
		_dataArray.Add(new TaskChainItem(32, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int> { 149, 150, 151, 241, 242, 243, 154 }, new List<int>(), new List<int>(), 31, 4));
		_dataArray.Add(new TaskChainItem(33, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int>
		{
			392, 393, 394, 395, 396, 397, 398, 399, 400, 401,
			402, 403, 404, 406
		}, new List<int>(), new List<int>(), 32, 5));
		_dataArray.Add(new TaskChainItem(34, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int>
		{
			232, 233, 169, 170, 171, 172, 173, 174, 245, 175,
			176, 234, 177, 178
		}, new List<int>(), new List<int>(), 33, 6));
		_dataArray.Add(new TaskChainItem(35, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int>
		{
			179, 180, 181, 286, 182, 183, 184, 185, 186, 187,
			188, 189, 190, 191, 192, 286, 287, 288, 289, 290
		}, new List<int>(), new List<int>(), 34, 11));
		_dataArray.Add(new TaskChainItem(36, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int>
		{
			275, 276, 277, 193, 278, 195, 279, 194, 280, 281,
			196, 282, 283, 197, 284, 199, 198, 285, 291, 292
		}, new List<int>(), new List<int>(), 35, 12));
		_dataArray.Add(new TaskChainItem(37, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int> { 200, 201, 202, 203, 255, 254, 204, 258, 259 }, new List<int>(), new List<int>(), 36, 2));
		_dataArray.Add(new TaskChainItem(38, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int> { 207, 208, 209, 210, 211, 212, 213, 214 }, new List<int>(), new List<int>(), 37, 13));
		_dataArray.Add(new TaskChainItem(39, ETaskChainGroup.SectMainStory, ETaskChainType.Parallel, 0, 99, -1, new List<int> { 205, 206, 215 }, new List<int>(), new List<int>(), 37, 13));
		_dataArray.Add(new TaskChainItem(40, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int> { 293, 294, 295, 296, 322, 323, 297, 298, 299, 300 }, new List<int>(), new List<int>(), 38, 7));
		_dataArray.Add(new TaskChainItem(41, ETaskChainGroup.SectMainStory, ETaskChainType.Parallel, 0, 99, -1, new List<int> { 236, 237, 238, 240, 244, 152, 239, 153 }, new List<int>(), new List<int>(), 31, 4));
		_dataArray.Add(new TaskChainItem(42, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 0, 99, -1, new List<int> { 257 }, new List<int>(), new List<int>(), 39, 0));
		_dataArray.Add(new TaskChainItem(43, ETaskChainGroup.MainStory, ETaskChainType.Line, 0, 99, -1, new List<int> { 260 }, new List<int>(), new List<int>(), 40, 0));
		_dataArray.Add(new TaskChainItem(44, ETaskChainGroup.OptionalTasks, ETaskChainType.Parallel, 0, 99, -1, new List<int> { 261, 262, 263 }, new List<int>(), new List<int>(), 41, 0));
		_dataArray.Add(new TaskChainItem(45, ETaskChainGroup.OptionalTasks, ETaskChainType.Parallel, 0, 99, -1, new List<int> { 264 }, new List<int>(), new List<int>(), 42, 0));
		_dataArray.Add(new TaskChainItem(46, ETaskChainGroup.OptionalTasks, ETaskChainType.Parallel, 0, 99, -1, new List<int> { 265, 268, 269, 270, 271, 272 }, new List<int>(), new List<int>(), 43, 0));
		_dataArray.Add(new TaskChainItem(47, ETaskChainGroup.OptionalTasks, ETaskChainType.Parallel, 0, 99, -1, new List<int> { 266, 273, 274 }, new List<int>(), new List<int>(), 44, 0));
		_dataArray.Add(new TaskChainItem(48, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int>
		{
			301, 302, 303, 304, 305, 306, 312, 325, 326, 327,
			328
		}, new List<int>(), new List<int>(), 45, 3));
		_dataArray.Add(new TaskChainItem(49, ETaskChainGroup.SectMainStory, ETaskChainType.Parallel, 0, 99, -1, new List<int> { 296, 319, 320, 321 }, new List<int>(), new List<int>(), 38, 7));
		_dataArray.Add(new TaskChainItem(50, ETaskChainGroup.SectMainStory, ETaskChainType.Parallel, 0, 99, -1, new List<int> { 307, 308, 309, 310, 311 }, new List<int>(), new List<int>(), 45, 3));
		_dataArray.Add(new TaskChainItem(51, ETaskChainGroup.SectMainStory, ETaskChainType.Parallel, 0, 99, -1, new List<int> { 313, 314, 315, 316, 317, 318, 324 }, new List<int>(), new List<int>(), 45, 3));
		_dataArray.Add(new TaskChainItem(52, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int>
		{
			329, 330, 346, 343, 331, 332, 333, 334, 335, 336,
			345, 344, 337, 338, 339, 340, 341
		}, new List<int>(), new List<int>(), 46, 14));
		_dataArray.Add(new TaskChainItem(53, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 0, 99, -1, new List<int> { 342 }, new List<int>(), new List<int>(), 47, 0));
		_dataArray.Add(new TaskChainItem(54, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int>
		{
			347, 348, 349, 350, 351, 352, 353, 371, 354, 355,
			356, 372, 357, 358, 359, 360, 373, 361, 365, 366,
			367, 374
		}, new List<int>(), new List<int>(), 48, 9));
		_dataArray.Add(new TaskChainItem(55, ETaskChainGroup.SectMainStory, ETaskChainType.Parallel, 0, 99, -1, new List<int> { 362, 368, 369, 370, 363, 364 }, new List<int>(), new List<int>(), 48, 9));
		_dataArray.Add(new TaskChainItem(56, ETaskChainGroup.SectMainStory, ETaskChainType.Line, 0, 99, -1, new List<int>
		{
			375, 376, 377, 378, 379, 380, 381, 382, 383, 384,
			385, 386, 387, 388, 389, 390, 391
		}, new List<int>(), new List<int>(), 36, 2));
		_dataArray.Add(new TaskChainItem(57, ETaskChainGroup.OptionalTasks, ETaskChainType.Line, 0, 99, -1, new List<int> { 405 }, new List<int>(), new List<int>(), 49, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TaskChainItem>(58);
		CreateItems0();
	}
}
