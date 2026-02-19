using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class UpdateLog : ConfigData<UpdateLogItem, byte>
{
	public static UpdateLog Instance = new UpdateLog();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "IncrementSortOrder", "VersionTagBackground", "VersionTagIcon", "VersionTitle", "VersionPublishDate", "OfficialLink", "SubentryTitles", "SubentryIcons", "SubentryDescriptions" };

	internal override int ToInt(byte value)
	{
		return value;
	}

	internal override byte ToTemplateId(int value)
	{
		return (byte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new UpdateLogItem(0, 0, "UpdateNote_Bg_0", "UpdateNote_Tag_0", 0, "22-11-18", "https://www.conchship.com.cn/archives/4261", new int[4] { 1, 2, 3, 4 }, new string[4] { "UpdateNote_Icon_3_2", "UpdateNote_Icon_3_3", "UpdateNote_Icon_3_4", "UpdateNote_Icon_3_5" }, new int[4] { 5, 6, 7, 8 }));
		_dataArray.Add(new UpdateLogItem(1, 1, "UpdateNote_Bg_1", "UpdateNote_Tag_1", 9, "22-11-25", "https://www.conchship.com.cn/archives/4270", new int[7] { 10, 11, 12, 13, 14, 15, 16 }, new string[7] { "UpdateNote_Icon_0_2", "UpdateNote_Icon_2_3", "UpdateNote_Icon_2_1", "UpdateNote_Icon_3_11", "UpdateNote_Icon_2_1", "UpdateNote_Icon_2_1", "UpdateNote_Icon_2_2" }, new int[7] { 17, 18, 19, 20, 21, 22, 23 }));
		_dataArray.Add(new UpdateLogItem(2, 2, "UpdateNote_Bg_2", "UpdateNote_Tag_2", 24, "22-12-09", "https://www.conchship.com.cn/archives/4276", new int[4] { 25, 26, 27, 28 }, new string[4] { "UpdateNote_Icon_0_1", "UpdateNote_Icon_2_1", "UpdateNote_Icon_2_6", "UpdateNote_Icon_3_21" }, new int[4] { 29, 30, 31, 32 }));
		_dataArray.Add(new UpdateLogItem(3, 3, "UpdateNote_Bg_3", "UpdateNote_Tag_3", 33, "23-01-18", "https://www.conchship.com.cn/archives/4282", new int[4] { 34, 35, 36, 37 }, new string[4] { "UpdateNote_Icon_0_3", "UpdateNote_Icon_3_0", "UpdateNote_Icon_3_1", "UpdateNote_Icon_2_2" }, new int[4] { 38, 39, 40, 41 }));
		_dataArray.Add(new UpdateLogItem(4, 4, "UpdateNote_Bg_4", "UpdateNote_Tag_4", 42, "23-03-16", "https://www.conchship.com.cn/archives/4297", new int[6] { 43, 44, 45, 46, 47, 48 }, new string[6] { "UpdateNote_Icon_0_4", "UpdateNote_Icon_2_5", "UpdateNote_Icon_3_7", "UpdateNote_Icon_2_6", "UpdateNote_Icon_3_6", "UpdateNote_Icon_2_1" }, new int[6] { 49, 50, 51, 52, 53, 54 }));
		_dataArray.Add(new UpdateLogItem(5, 5, "UpdateNote_Bg_5", "UpdateNote_Tag_5", 55, "23-05-10", "https://www.conchship.com.cn/archives/4303", new int[6] { 56, 57, 58, 59, 60, 61 }, new string[6] { "UpdateNote_Icon_0_5", "UpdateNote_Icon_2_8", "UpdateNote_Icon_2_6", "UpdateNote_Icon_2_2", "UpdateNote_Icon_3_20", "UpdateNote_Icon_2_7" }, new int[6] { 62, 63, 64, 65, 66, 67 }));
		_dataArray.Add(new UpdateLogItem(6, 6, "UpdateNote_Bg_6", "UpdateNote_Tag_6", 68, "23-06-20", "https://www.conchship.com.cn/archives/4323", new int[5] { 69, 70, 58, 71, 72 }, new string[5] { "UpdateNote_Icon_0_6", "UpdateNote_Icon_2_4", "UpdateNote_Icon_2_6", "UpdateNote_Icon_2_2", "UpdateNote_Icon_2_5" }, new int[5] { 73, 74, 75, 76, 77 }));
		_dataArray.Add(new UpdateLogItem(7, 7, "UpdateNote_Bg_7", "UpdateNote_Tag_7", 78, "23-07-31", "https://www.conchship.com.cn/archives/4401", new int[5] { 79, 80, 81, 58, 82 }, new string[5] { "UpdateNote_Icon_0_8", "UpdateNote_Icon_0_7", "UpdateNote_Icon_2_4", "UpdateNote_Icon_2_6", "UpdateNote_Icon_3_18" }, new int[5] { 83, 84, 85, 86, 87 }));
		_dataArray.Add(new UpdateLogItem(8, 8, "UpdateNote_Bg_8", "UpdateNote_Tag_8", 88, "23-09-10", "https://www.conchship.com.cn/archives/4414", new int[8] { 89, 90, 91, 92, 93, 94, 95, 96 }, new string[8] { "UpdateNote_Icon_0_9", "UpdateNote_Icon_0_10", "UpdateNote_Icon_2_4", "UpdateNote_Icon_2_6", "UpdateNote_Icon_3_19", "UpdateNote_Icon_3_12", "UpdateNote_Icon_2_2", "UpdateNote_Icon_2_2" }, new int[8] { 97, 98, 99, 100, 101, 102, 103, 104 }));
		_dataArray.Add(new UpdateLogItem(9, 9, "UpdateNote_Bg_9", "UpdateNote_Tag_9", 105, "23-11-10", "https://www.conchship.com.cn/archives/4432", new int[8] { 106, 107, 108, 109, 110, 111, 112, 113 }, new string[8] { "UpdateNote_Icon_0_12", "UpdateNote_Icon_1_1", "UpdateNote_Icon_0_11", "UpdateNote_Icon_1_0", "UpdateNote_Icon_2_4", "UpdateNote_Icon_2_6", "UpdateNote_Icon_2_0", "UpdateNote_Icon_3_15" }, new int[8] { 114, 115, 116, 117, 118, 119, 120, 121 }));
		_dataArray.Add(new UpdateLogItem(10, 10, "UpdateNote_Bg_10", "UpdateNote_Tag_10", 122, "24-01-05", "https://www.conchship.com.cn/archives/4442", new int[8] { 122, 123, 124, 125, 126, 127, 128, 129 }, new string[8] { "UpdateNote_Icon_0_13", "UpdateNote_Icon_2_5", "UpdateNote_Icon_2_3", "UpdateNote_Icon_2_4", "UpdateNote_Icon_2_3", "UpdateNote_Icon_2_7", "UpdateNote_Icon_2_1", "UpdateNote_Icon_2_2" }, new int[8] { 130, 131, 132, 133, 134, 135, 136, 137 }));
		_dataArray.Add(new UpdateLogItem(11, 11, "UpdateNote_Bg_11", "UpdateNote_Tag_11", 138, "24-01-31", "https://www.conchship.com.cn/new", new int[1] { 138 }, new string[1] { "UpdateNote_Icon_2_4" }, new int[1] { 139 }));
		_dataArray.Add(new UpdateLogItem(12, 12, "UpdateNote_Bg_12", "UpdateNote_Tag_12", 140, "24-04-19", "https://www.conchship.com.cn/archives/4465", new int[5] { 141, 142, 143, 144, 145 }, new string[5] { "UpdateNote_Icon_0_14", "UpdateNote_Icon_1_2", "UpdateNote_Icon_0_15", "UpdateNote_Icon_1_3", "UpdateNote_Icon_2_4" }, new int[5] { 146, 147, 148, 149, 150 }));
		_dataArray.Add(new UpdateLogItem(13, 13, "UpdateNote_Bg_13", "UpdateNote_Tag_13", 151, "24-07-09", "https://www.conchship.com.cn/archives/4579", new int[14]
		{
			152, 153, 154, 155, 156, 157, 58, 158, 159, 160,
			161, 162, 163, 164
		}, new string[14]
		{
			"UpdateNote_Icon_0_17", "UpdateNote_Icon_1_5", "UpdateNote_Icon_0_16", "UpdateNote_Icon_1_4", "UpdateNote_Icon_2_4", "UpdateNote_Icon_3_14", "UpdateNote_Icon_2_6", "UpdateNote_Icon_3_17", "UpdateNote_Icon_3_13", "UpdateNote_Icon_2_5",
			"UpdateNote_Icon_3_10", "UpdateNote_Icon_2_4", "UpdateNote_Icon_2_1", "UpdateNote_Icon_2_1"
		}, new int[14]
		{
			165, 166, 167, 168, 169, 170, 171, 172, 173, 174,
			175, 176, 177, 178
		}));
		_dataArray.Add(new UpdateLogItem(14, 14, "UpdateNote_Bg_14", "UpdateNote_Tag_14", 179, "24-09-26", "https://www.conchship.com.cn/archives/4621", new int[10] { 180, 181, 182, 183, 58, 184, 185, 186, 187, 188 }, new string[10] { "UpdateNote_Icon_0_18", "UpdateNote_Icon_1_6", "UpdateNote_Icon_3_9", "UpdateNote_Icon_3_16", "UpdateNote_Icon_2_6", "UpdateNote_Icon_2_2", "UpdateNote_Icon_2_2", "UpdateNote_Icon_2_3", "UpdateNote_Icon_2_8", "UpdateNote_Icon_3_8" }, new int[10] { 189, 190, 191, 192, 193, 194, 195, 196, 197, 198 }));
		_dataArray.Add(new UpdateLogItem(15, 15, "UpdateNote_Bg_15", "UpdateNote_Tag_15", 199, "24-12-26", "https://www.conchship.com.cn/archives/4899", new int[8] { 200, 201, 202, 203, 204, 205, 206, 207 }, new string[8] { "UpdateNote_Icon_0_19", "UpdateNote_Icon_1_7", "UpdateNote_Icon_2_4", "UpdateNote_Icon_3_22", "UpdateNote_Icon_3_23", "UpdateNote_Icon_3_24", "UpdateNote_Icon_3_34", "UpdateNote_Icon_3_18" }, new int[8] { 208, 209, 210, 211, 212, 213, 214, 215 }));
		_dataArray.Add(new UpdateLogItem(16, 16, "UpdateNote_Bg_16", "UpdateNote_Tag_16", 216, "25-01-21", "https://www.conchship.com.cn/new", new int[4] { 217, 218, 219, 220 }, new string[4] { "UpdateNote_Icon_3_31", "UpdateNote_Icon_3_29", "UpdateNote_Icon_3_30", "UpdateNote_Icon_3_26" }, new int[4] { 221, 222, 223, 224 }));
		_dataArray.Add(new UpdateLogItem(17, 17, "UpdateNote_Bg_17", "UpdateNote_Tag_17", 225, "25-03-26", "https://www.conchship.com.cn/archives/4980", new int[4] { 226, 227, 228, 229 }, new string[4] { "UpdateNote_Icon_3_28", "UpdateNote_Icon_3_25", "UpdateNote_Icon_0_8", "UpdateNote_Icon_3_27" }, new int[4] { 230, 231, 232, 233 }));
		_dataArray.Add(new UpdateLogItem(18, 18, "UpdateNote_Bg_18", "UpdateNote_Tag_18", 234, "25-04-29", "https://www.conchship.com.cn/archives/5009", new int[3] { 235, 236, 237 }, new string[3] { "UpdateNote_Icon_3_32", "UpdateNote_Icon_3_33", "UpdateNote_Icon_3_17" }, new int[3] { 238, 239, 240 }));
		_dataArray.Add(new UpdateLogItem(19, 19, "UpdateNote_Bg_19", "UpdateNote_Tag_19", 241, "25-06-05", "https://www.conchship.com.cn/archives/5036", new int[6] { 242, 243, 244, 245, 246, 247 }, new string[6] { "UpdateNote_Icon_3_35", "UpdateNote_Icon_3_36", "UpdateNote_Icon_3_37", "UpdateNote_Icon_3_38", "UpdateNote_Icon_3_39", "UpdateNote_Icon_3_40" }, new int[6] { 248, 249, 250, 251, 252, 253 }));
		_dataArray.Add(new UpdateLogItem(20, 20, "UpdateNote_Bg_20", "UpdateNote_Tag_20", 254, "25-09-22", "https://www.conchship.com.cn/archives/5087", new int[12]
		{
			255, 256, 257, 258, 259, 260, 261, 262, 263, 264,
			265, 266
		}, new string[12]
		{
			"UpdateNote_Icon_3_41", "UpdateNote_Icon_3_42", "UpdateNote_Icon_3_43", "UpdateNote_Icon_3_44", "UpdateNote_Icon_3_45", "UpdateNote_Icon_3_46", "UpdateNote_Icon_2_7", "UpdateNote_Icon_3_47", "UpdateNote_Icon_3_48", "UpdateNote_Icon_3_28",
			"UpdateNote_Icon_3_49", "UpdateNote_Icon_3_50"
		}, new int[12]
		{
			267, 268, 269, 270, 271, 272, 273, 274, 275, 276,
			277, 278
		}));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<UpdateLogItem>(21);
		CreateItems0();
	}
}
