using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class CombatSkillType : ConfigData<CombatSkillTypeItem, sbyte>
{
	public static CombatSkillType Instance = new CombatSkillType();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"LegendaryBookWeaponSlot", "LegendaryBookSkillSlots", "LegendaryBookAddPropertyYin", "LegendaryBookAddPropertyYang", "LegendaryBookFeature", "LegendaryBookTaiwuFeature", "LegendaryBookConsumedFeature", "TemplateId", "Name", "BackgroundTexture",
		"LoadingTexture", "Icon", "DisplayIcon", "DisplayIconBig", "TipsIcon", "Desc", "LegendaryBookWeaponSlotItemSubTypes", "LegendaryBookEffectSlotYin", "LegendaryBookEffectSlotYang"
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
		_dataArray.Add(new CombatSkillTypeItem(0, 0, "charactermenu3_18_beijingtu_0", "tex_combatskilltype_0", "sp_icon_dawuxue_0", "sp_18_iconwuxuezhanshi_0", "sp_18_iconwuxue_0", "mousetip_gongfa_0", 1, 0, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, null, new List<short>
		{
			2, 43, 43, 43, 15, 43, 43, 43, 0, 43,
			43, 43
		}, new List<short>
		{
			2, 43, 43, 43, 15, 43, 43, 43, 0, 43,
			43, 43
		}, new List<sbyte> { 1, 2, 5 }, new List<sbyte> { 0, 3, 4 }, 341, 355, 369, 0));
		_dataArray.Add(new CombatSkillTypeItem(1, 2, "charactermenu3_18_beijingtu_1", "tex_combatskilltype_1", "sp_icon_dawuxue_1", "sp_18_iconwuxuezhanshi_1", "sp_18_iconwuxue_1", "mousetip_gongfa_1", 3, 10, new List<short> { 11, 12, 13, 14, 15, 16, 17, 18, 19 }, null, new List<short>
		{
			4, 44, 44, 44, 16, 44, 44, 44, 4, 44,
			44, 44
		}, new List<short>
		{
			4, 44, 44, 44, 16, 44, 44, 44, 4, 44,
			44, 44
		}, new List<sbyte> { 1, 2, 5 }, new List<sbyte> { 0, 3, 4 }, 342, 356, 370, 0));
		_dataArray.Add(new CombatSkillTypeItem(2, 4, "charactermenu3_18_beijingtu_2", "tex_combatskilltype_2", "sp_icon_dawuxue_2", "sp_18_iconwuxuezhanshi_2", "sp_18_iconwuxue_2", "mousetip_gongfa_2", 5, 20, new List<short> { 21, 22, 23, 24, 25, 26, 27, 28, 29 }, null, new List<short>
		{
			9, 45, 45, 45, 17, 45, 45, 45, 9, 45,
			45, 45
		}, new List<short>
		{
			9, 45, 45, 45, 17, 45, 45, 45, 9, 45,
			45, 45
		}, new List<sbyte> { 1, 2, 5 }, new List<sbyte> { 0, 3, 4 }, 343, 357, 371, 0));
		_dataArray.Add(new CombatSkillTypeItem(3, 6, "charactermenu3_18_beijingtu_3", "tex_combatskilltype_3", "sp_icon_dawuxue_3", "sp_18_iconwuxuezhanshi_3", "sp_18_iconwuxue_3", "mousetip_gongfa_3", 7, 30, new List<short> { 31, 32, 33, 34, 35, 36, 37, 38, 39 }, new List<short> { 4 }, new List<short>
		{
			10, 46, 46, 46, 18, 46, 46, 46, 10, 46,
			46, 46
		}, new List<short>
		{
			10, 46, 46, 46, 18, 46, 46, 46, 10, 46,
			46, 46
		}, new List<sbyte> { 2, 1, 4 }, new List<sbyte> { 0, -1, 3 }, 344, 358, 372, 0));
		_dataArray.Add(new CombatSkillTypeItem(4, 8, "charactermenu3_18_beijingtu_4", "tex_combatskilltype_4", "sp_icon_dawuxue_4", "sp_18_iconwuxuezhanshi_4", "sp_18_iconwuxue_4", "mousetip_gongfa_4", 9, 40, new List<short> { 41, 42, 43, 44, 45, 46, 47, 48, 49 }, new List<short> { 4 }, new List<short>
		{
			6, 47, 47, 47, 19, 47, 47, 47, 6, 47,
			47, 47
		}, new List<short>
		{
			6, 47, 47, 47, 19, 47, 47, 47, 6, 47,
			47, 47
		}, new List<sbyte> { 2, 1, 4 }, new List<sbyte> { 0, -1, 3 }, 345, 359, 373, 0));
		_dataArray.Add(new CombatSkillTypeItem(5, 10, "charactermenu3_18_beijingtu_5", "tex_combatskilltype_5", "sp_icon_dawuxue_5", "sp_18_iconwuxuezhanshi_5", "sp_18_iconwuxue_5", "mousetip_gongfa_5", 11, 50, new List<short> { 51, 52, 53, 54, 55, 56, 57, 58, 59 }, null, new List<short>
		{
			13, 48, 48, 48, 20, 48, 48, 48, 13, 48,
			48, 48
		}, new List<short>
		{
			13, 48, 48, 48, 20, 48, 48, 48, 13, 48,
			48, 48
		}, new List<sbyte> { 2, 1, 4 }, new List<sbyte> { 0, -1, 3 }, 346, 360, 374, 0));
		_dataArray.Add(new CombatSkillTypeItem(6, 12, "charactermenu3_18_beijingtu_6", "tex_combatskilltype_6", "sp_icon_dawuxue_6", "sp_18_iconwuxuezhanshi_6", "sp_18_iconwuxue_6", "mousetip_gongfa_6", 13, 60, new List<short> { 61, 62, 63, 64, 65, 66, 67, 68, 69 }, new List<short> { 2, 14, 15 }, new List<short>
		{
			7, 49, 49, 49, 21, 49, 49, 49, 7, 49,
			49, 49
		}, new List<short>
		{
			7, 49, 49, 49, 21, 49, 49, 49, 7, 49,
			49, 49
		}, new List<sbyte> { 2, 1, 4 }, new List<sbyte> { 0, -1, 3 }, 347, 361, 375, 0));
		_dataArray.Add(new CombatSkillTypeItem(7, 14, "charactermenu3_18_beijingtu_7", "tex_combatskilltype_7", "sp_icon_dawuxue_7", "sp_18_iconwuxuezhanshi_7", "sp_18_iconwuxue_7", "mousetip_gongfa_7", 15, 70, new List<short> { 71, 72, 73, 74, 75, 76, 77, 78, 79 }, new List<short> { 8 }, new List<short>
		{
			8, 50, 50, 50, 22, 50, 50, 50, 8, 50,
			50, 50
		}, new List<short>
		{
			8, 50, 50, 50, 22, 50, 50, 50, 8, 50,
			50, 50
		}, new List<sbyte> { 2, 1, 4 }, new List<sbyte> { 0, -1, 3 }, 348, 362, 376, 0));
		_dataArray.Add(new CombatSkillTypeItem(8, 16, "charactermenu3_18_beijingtu_8", "tex_combatskilltype_8", "sp_icon_dawuxue_8", "sp_18_iconwuxuezhanshi_8", "sp_18_iconwuxue_8", "mousetip_gongfa_8", 17, 80, new List<short> { 81, 82, 83, 84, 85, 86, 87, 88, 89 }, new List<short> { 9 }, new List<short>
		{
			5, 51, 51, 51, 23, 51, 51, 51, 5, 51,
			51, 51
		}, new List<short>
		{
			5, 51, 51, 51, 23, 51, 51, 51, 5, 51,
			51, 51
		}, new List<sbyte> { 2, 1, 4 }, new List<sbyte> { 0, -1, 3 }, 349, 363, 377, 0));
		_dataArray.Add(new CombatSkillTypeItem(9, 18, "charactermenu3_18_beijingtu_9", "tex_combatskilltype_9", "sp_icon_dawuxue_9", "sp_18_iconwuxuezhanshi_9", "sp_18_iconwuxue_9", "mousetip_gongfa_9", 19, 90, new List<short> { 91, 92, 93, 94, 95, 96, 97, 98, 99 }, new List<short> { 10 }, new List<short>
		{
			3, 52, 52, 52, 24, 52, 52, 52, 3, 52,
			52, 52
		}, new List<short>
		{
			3, 52, 52, 52, 24, 52, 52, 52, 3, 52,
			52, 52
		}, new List<sbyte> { 2, 1, 4 }, new List<sbyte> { 0, -1, 3 }, 350, 364, 378, 0));
		_dataArray.Add(new CombatSkillTypeItem(10, 20, "charactermenu3_18_beijingtu_10", "tex_combatskilltype_10", "sp_icon_dawuxue_10", "sp_18_iconwuxuezhanshi_10", "sp_18_iconwuxue_10", "mousetip_gongfa_10", 21, 100, new List<short> { 101, 102, 103, 104, 105, 106, 107, 108, 109 }, new List<short> { 1, 5, 13 }, new List<short>
		{
			12, 53, 53, 53, 25, 53, 53, 53, 12, 53,
			53, 53
		}, new List<short>
		{
			12, 53, 53, 53, 25, 53, 53, 53, 12, 53,
			53, 53
		}, new List<sbyte> { 2, 1, 4 }, new List<sbyte> { 0, -1, 3 }, 351, 365, 379, 0));
		_dataArray.Add(new CombatSkillTypeItem(11, 22, "charactermenu3_18_beijingtu_11", "tex_combatskilltype_11", "sp_icon_dawuxue_11", "sp_18_iconwuxuezhanshi_11", "sp_18_iconwuxue_11", "mousetip_gongfa_11", 23, 110, new List<short> { 111, 112, 113, 114, 115, 116, 117, 118, 119 }, new List<short> { 6, 7 }, new List<short>
		{
			11, 54, 54, 54, 26, 54, 54, 54, 11, 54,
			54, 54
		}, new List<short>
		{
			11, 54, 54, 54, 26, 54, 54, 54, 11, 54,
			54, 54
		}, new List<sbyte> { 2, 1, 4 }, new List<sbyte> { 0, -1, 3 }, 352, 366, 380, 0));
		_dataArray.Add(new CombatSkillTypeItem(12, 24, "charactermenu3_18_beijingtu_12", "tex_combatskilltype_12", "sp_icon_dawuxue_12", "sp_18_iconwuxuezhanshi_12", "sp_18_iconwuxue_12", "mousetip_gongfa_12", 25, 120, new List<short> { 121, 122, 123, 124, 125, 126, 127, 128, 129 }, new List<short> { 0, 12 }, new List<short>
		{
			1, 55, 55, 55, 27, 55, 55, 55, 1, 55,
			55, 55
		}, new List<short>
		{
			1, 55, 55, 55, 27, 55, 55, 55, 1, 55,
			55, 55
		}, new List<sbyte> { 2, 1, 4 }, new List<sbyte> { 0, -1, 3 }, 353, 367, 381, 0));
		_dataArray.Add(new CombatSkillTypeItem(13, 26, "charactermenu3_18_beijingtu_13", "tex_combatskilltype_13", "sp_icon_dawuxue_13", "sp_18_iconwuxuezhanshi_13", "sp_18_iconwuxue_13", "mousetip_gongfa_13", 27, 130, new List<short> { 131, 132, 133, 134, 135, 136, 137, 138, 139 }, new List<short> { 3, 11 }, new List<short>
		{
			14, 56, 56, 56, 28, 56, 56, 56, 14, 56,
			56, 56
		}, new List<short>
		{
			14, 56, 56, 56, 28, 56, 56, 56, 14, 56,
			56, 56
		}, new List<sbyte> { 2, 1, 4 }, new List<sbyte> { 0, -1, 3 }, 354, 368, 382, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CombatSkillTypeItem>(14);
		CreateItems0();
	}
}
