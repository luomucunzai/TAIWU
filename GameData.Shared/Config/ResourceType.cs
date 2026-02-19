using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class ResourceType : ConfigData<ResourceTypeItem, sbyte>
{
	public static ResourceType Instance = new ResourceType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "LifeSkillType", "PossibleBuildingCoreItem", "PossibleUpgradedBuildingCoreItem", "TemplateId", "Name", "Desc", "Icon", "ImgPrefix" };

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
		_dataArray.Add(new ResourceTypeItem(0, 0, 1, "sp_icon_resource_food", "charactermenu3_10_shicai_", 1, 25, 14, new short[3] { 333, 340, 341 }, new short[0], new string[1] { "" }, new string[1] { "" }, new string[1] { "" }, new string[1] { "" }));
		_dataArray.Add(new ResourceTypeItem(1, 2, 3, "sp_icon_resource_wood", "charactermenu3_10_mucai_", 1, 25, 7, new short[2] { 335, 340 }, new short[2] { 366, 367 }, new string[2] { "se_combat_hit_wood_1", "se_combat_hit_wood_2" }, new string[1] { "se_combat_whoosh_wood" }, new string[2] { "se_combat_shock_wood_1", "se_combat_shock_wood_2" }, new string[2] { "se_combat_foot_wood_1", "se_combat_foot_wood_2" }));
		_dataArray.Add(new ResourceTypeItem(2, 4, 5, "sp_icon_resource_ston", "charactermenu3_10_jintie_", 1, 25, 6, new short[2] { 334, 336 }, new short[2] { 364, 365 }, new string[2] { "se_combat_hit_iron_1", "se_combat_hit_iron_2" }, new string[1] { "se_combat_whoosh_iron" }, new string[2] { "se_combat_shock_iron_1", "se_combat_shock_iron_2" }, new string[2] { "se_combat_foot_iron_1", "se_combat_foot_iron_2" }));
		_dataArray.Add(new ResourceTypeItem(3, 6, 7, "sp_icon_resource_jade", "charactermenu3_10_yushi_", 1, 25, 11, new short[2] { 336, 339 }, new short[2] { 372, 373 }, new string[2] { "se_combat_hit_jade_1", "se_combat_hit_jade_2" }, new string[1] { "se_combat_whoosh_jade" }, new string[2] { "se_combat_shock_jade_1", "se_combat_shock_jade_2" }, new string[2] { "se_combat_foot_jade_1", "se_combat_foot_jade_2" }));
		_dataArray.Add(new ResourceTypeItem(4, 8, 9, "sp_icon_resource_silk", "charactermenu3_10_zhiwu_", 1, 25, 10, new short[2] { 342, 341 }, new short[2] { 370, 371 }, new string[2] { "se_combat_hit_cloth_1", "se_combat_hit_cloth_2" }, new string[1] { "se_combat_whoosh_cloths" }, new string[2] { "se_combat_shock_cloth_1", "se_combat_shock_cloth_2" }, new string[2] { "se_combat_foot_cloth_1", "se_combat_foot_cloth_2" }));
		_dataArray.Add(new ResourceTypeItem(5, 10, 11, "sp_icon_resource_herbal", "charactermenu3_10_yaocai_", 1, 25, 8, new short[2] { 337, 338 }, new short[2] { 368, 369 }, new string[1] { "" }, new string[1] { "" }, new string[1] { "" }, new string[1] { "" }));
		_dataArray.Add(new ResourceTypeItem(6, 12, 13, "sp_icon_resource_money", "charactermenu3_10_jinqian_", -1, -1, -1, new short[0], new short[0], new string[1] { "" }, new string[1] { "" }, new string[1] { "" }, new string[1] { "" }));
		_dataArray.Add(new ResourceTypeItem(7, 14, 15, "sp_icon_resource_authority", "charactermenu3_10_weiwang_", -1, -1, -1, new short[0], new short[0], new string[1] { "" }, new string[1] { "" }, new string[1] { "" }, new string[1] { "" }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ResourceTypeItem>(8);
		CreateItems0();
	}
}
