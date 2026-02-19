using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LifeSkillType : ConfigData<LifeSkillTypeItem, sbyte>
{
	public static LifeSkillType Instance = new LifeSkillType();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"InformationTemplateId", "TemplateId", "Name", "Desc", "Icon", "DisplayIcon", "DisplayIconBig", "BackgroundTexture", "LoadingTexture", "AttainmentEffectTexture",
		"SkillList", "MakeDesc", "DialogInBattle"
	};

	internal override int ToInt(sbyte value)
	{
		return value;
	}

	internal override sbyte ToTemplateId(int value)
	{
		return (sbyte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new LifeSkillTypeItem(0, 0, 1, "sp_icon_dajiyi_0", "sp_14_iconjiyizhanshi_0", "sp_14_iconone_0", "charactermenu3_14_part_beijing_0", "tex_lifeskilltype_0", "charactermenu3_16_chahua_0", 30, new short[9] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, 2, 3, 0));
		_dataArray.Add(new LifeSkillTypeItem(1, 4, 5, "sp_icon_dajiyi_1", "sp_14_iconjiyizhanshi_1", "sp_14_iconone_1", "charactermenu3_14_part_beijing_1", "tex_lifeskilltype_1", "charactermenu3_16_chahua_1", 31, new short[9] { 9, 10, 11, 12, 13, 14, 15, 16, 17 }, 6, 7, 0));
		_dataArray.Add(new LifeSkillTypeItem(2, 8, 9, "sp_icon_dajiyi_2", "sp_14_iconjiyizhanshi_2", "sp_14_iconone_2", "charactermenu3_14_part_beijing_2", "tex_lifeskilltype_2", "charactermenu3_16_chahua_2", 32, new short[9] { 18, 19, 20, 21, 22, 23, 24, 25, 26 }, 10, 11, 0));
		_dataArray.Add(new LifeSkillTypeItem(3, 12, 13, "sp_icon_dajiyi_3", "sp_14_iconjiyizhanshi_3", "sp_14_iconone_3", "charactermenu3_14_part_beijing_3", "tex_lifeskilltype_3", "charactermenu3_16_chahua_3", 33, new short[9] { 27, 28, 29, 30, 31, 32, 33, 34, 35 }, 14, 15, 0));
		_dataArray.Add(new LifeSkillTypeItem(4, 16, 17, "sp_icon_dajiyi_4", "sp_14_iconjiyizhanshi_4", "sp_14_iconone_4", "charactermenu3_14_part_beijing_4", "tex_lifeskilltype_4", "charactermenu3_16_chahua_4", 34, new short[9] { 36, 37, 38, 39, 40, 41, 42, 43, 44 }, 18, 19, 0));
		_dataArray.Add(new LifeSkillTypeItem(5, 20, 21, "sp_icon_dajiyi_5", "sp_14_iconjiyizhanshi_5", "sp_14_iconone_5", "charactermenu3_14_part_beijing_5", "tex_lifeskilltype_5", "charactermenu3_16_chahua_5", 35, new short[9] { 45, 46, 47, 48, 49, 50, 51, 52, 53 }, 22, 11, 0));
		_dataArray.Add(new LifeSkillTypeItem(6, 23, 24, "sp_icon_dajiyi_6", "sp_14_iconjiyizhanshi_6", "sp_14_iconone_6", "charactermenu3_14_part_beijing_6", "tex_lifeskilltype_6", "charactermenu3_16_chahua_6", 36, new short[9] { 54, 55, 56, 57, 58, 59, 60, 61, 62 }, 23, 25, 0));
		_dataArray.Add(new LifeSkillTypeItem(7, 26, 27, "sp_icon_dajiyi_7", "sp_14_iconjiyizhanshi_7", "sp_14_iconone_7", "charactermenu3_14_part_beijing_7", "tex_lifeskilltype_7", "charactermenu3_16_chahua_7", 37, new short[9] { 63, 64, 65, 66, 67, 68, 69, 70, 71 }, 26, 28, 0));
		_dataArray.Add(new LifeSkillTypeItem(8, 29, 30, "sp_icon_dajiyi_8", "sp_14_iconjiyizhanshi_8", "sp_14_iconone_8", "charactermenu3_14_part_beijing_8", "tex_lifeskilltype_8", "charactermenu3_16_chahua_8", 38, new short[9] { 72, 73, 74, 75, 76, 77, 78, 79, 80 }, 31, 32, 0));
		_dataArray.Add(new LifeSkillTypeItem(9, 33, 34, "sp_icon_dajiyi_9", "sp_14_iconjiyizhanshi_9", "sp_14_iconone_9", "charactermenu3_14_part_beijing_9", "tex_lifeskilltype_9", "charactermenu3_16_chahua_9", 39, new short[9] { 81, 82, 83, 84, 85, 86, 87, 88, 89 }, 35, 25, 0));
		_dataArray.Add(new LifeSkillTypeItem(10, 36, 37, "sp_icon_dajiyi_10", "sp_14_iconjiyizhanshi_10", "sp_14_iconone_10", "charactermenu3_14_part_beijing_10", "tex_lifeskilltype_10", "charactermenu3_16_chahua_10", 40, new short[9] { 90, 91, 92, 93, 94, 95, 96, 97, 98 }, 36, 28, 0));
		_dataArray.Add(new LifeSkillTypeItem(11, 38, 39, "sp_icon_dajiyi_11", "sp_14_iconjiyizhanshi_11", "sp_14_iconone_11", "charactermenu3_14_part_beijing_11", "tex_lifeskilltype_11", "charactermenu3_16_chahua_11", 41, new short[9] { 99, 100, 101, 102, 103, 104, 105, 106, 107 }, 40, 25, 0));
		_dataArray.Add(new LifeSkillTypeItem(12, 41, 42, "sp_icon_dajiyi_12", "sp_14_iconjiyizhanshi_12", "sp_14_iconone_12", "charactermenu3_14_part_beijing_12", "tex_lifeskilltype_12", "charactermenu3_16_chahua_12", 42, new short[9] { 108, 109, 110, 111, 112, 113, 114, 115, 116 }, 43, 44, 0));
		_dataArray.Add(new LifeSkillTypeItem(13, 45, 46, "sp_icon_dajiyi_13", "sp_14_iconjiyizhanshi_13", "sp_14_iconone_13", "charactermenu3_14_part_beijing_13", "tex_lifeskilltype_13", "charactermenu3_16_chahua_13", 43, new short[9] { 117, 118, 119, 120, 121, 122, 123, 124, 125 }, 47, 48, 0));
		_dataArray.Add(new LifeSkillTypeItem(14, 49, 50, "sp_icon_dajiyi_14", "sp_14_iconjiyizhanshi_14", "sp_14_iconone_14", "charactermenu3_14_part_beijing_14", "tex_lifeskilltype_14", "charactermenu3_16_chahua_14", 44, new short[9] { 126, 127, 128, 129, 130, 131, 132, 133, 134 }, 51, 52, 0));
		_dataArray.Add(new LifeSkillTypeItem(15, 53, 54, "sp_icon_dajiyi_15", "sp_14_iconjiyizhanshi_15", "sp_14_iconone_15", "charactermenu3_14_part_beijing_15", "tex_lifeskilltype_15", "charactermenu3_16_chahua_15", 45, new short[9] { 135, 136, 137, 138, 139, 140, 141, 142, 143 }, 55, 11, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<LifeSkillTypeItem>(16);
		CreateItems0();
	}
}
