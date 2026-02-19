using System;
using Config;
using GameData.Domains.Item;

namespace GameData.Domains.Extra;

public static class SharedMethods
{
	public static int GetTreeExtraNameTextTemplateId(int miscTemplateId)
	{
		return miscTemplateId switch
		{
			272 => 6, 
			273 => 7, 
			274 => 8, 
			275 => 9, 
			276 => 10, 
			277 => 11, 
			278 => 12, 
			279 => 13, 
			280 => 14, 
			281 => 15, 
			282 => 16, 
			283 => 17, 
			394 => 20, 
			395 => 21, 
			396 => 22, 
			397 => 23, 
			398 => 24, 
			399 => 25, 
			400 => 26, 
			401 => 27, 
			402 => 28, 
			403 => 29, 
			404 => 30, 
			405 => 31, 
			_ => throw new Exception($"Exception HeavenlyTree Seed TemplateId: {miscTemplateId}"), 
		};
	}

	public static string GetTreeName(int miscTemplateId)
	{
		int treeExtraNameTextTemplateId = GetTreeExtraNameTextTemplateId(miscTemplateId);
		return ExtraNameText.Instance[treeExtraNameTextTemplateId].Content;
	}

	public static int GetFoodAddCarrierDurability(short carrierTemplateId, short materialTemplateId, int count = 1)
	{
		CarrierItem carrierItem = Carrier.Instance[carrierTemplateId];
		MaterialItem materialItem = Material.Instance[materialTemplateId];
		int num = GlobalConfig.Instance.FoodGradeAddCarrierDurability[materialItem.Grade];
		if (carrierItem.LoveFoodType.Contains(materialTemplateId))
		{
			num += GlobalConfig.Instance.LikeFoodAddCarrierDurability;
		}
		else if (carrierItem.HateFoodType.Contains(materialTemplateId))
		{
			num += GlobalConfig.Instance.DislikeFoodAddCarrierDurability;
		}
		return num * count;
	}

	public static int GetFoodAddCarrierTamePoint(short carrierId, short materialId, int count = 1)
	{
		CarrierItem carrierItem = Carrier.Instance[carrierId];
		MaterialItem materialItem = Material.Instance[materialId];
		int num = Material.Instance[materialId].Grade + 1;
		if (carrierItem.LoveFoodType.Contains(materialItem.TemplateId))
		{
			num = Math.Max(1, (int)Math.Ceiling((float)num * 1.5f));
		}
		else if (carrierItem.HateFoodType.Contains(materialItem.TemplateId))
		{
			num = Math.Max(1, (int)Math.Floor((float)num * 0.5f));
		}
		return num * count;
	}

	public static int GetExchangeToolSpiritualDebtCost(sbyte grade)
	{
		return (grade + 1) * 50;
	}

	public static int GetFixItemAttainmentNeed(sbyte grade)
	{
		return grade * (grade + 1) * 5 / 2 + 5;
	}

	public static sbyte GetItemRequireLifeSkillType(ItemKey itemKey)
	{
		sbyte resourceType = ItemTemplateHelper.GetResourceType(itemKey.ItemType, itemKey.TemplateId);
		if (resourceType < 0)
		{
			return -1;
		}
		return ResourceType.Instance[resourceType].LifeSkillType;
	}
}
