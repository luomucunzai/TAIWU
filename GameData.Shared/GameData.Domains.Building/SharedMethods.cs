using System;
using System.Collections.Generic;
using Config;
using Config.Common;
using GameData.Combat.Math;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Organization.Display;
using GameData.Utilities;

namespace GameData.Domains.Building;

public static class SharedMethods
{
	public const int ResourceBlockBaseValueCount = 5;

	public static sbyte GetHerbMaterialTempGrade(sbyte grade, List<short> makeItemSubtypeIdList, short makeItemSubTypeId)
	{
		if (makeItemSubtypeIdList.FindIndex((short item) => item == makeItemSubTypeId) == -1)
		{
			return grade;
		}
		MakeItemSubTypeItem makeItemSubTypeItem = MakeItemSubType.Instance[makeItemSubTypeId];
		return GetHerbMaterialTempGrade(grade, isManual: true, !makeItemSubTypeItem.IsOdd);
	}

	public static sbyte GetHerbMaterialTempGrade(sbyte grade, bool isManual, bool isMain)
	{
		if (!isManual)
		{
			return grade;
		}
		return (sbyte)(grade switch
		{
			1 => (!isMain) ? 1 : 4, 
			3 => isMain ? 5 : 2, 
			5 => isMain ? 6 : 3, 
			7 => isMain ? 7 : 4, 
			_ => 1, 
		});
	}

	public static short GetMakeRequiredLifeSkillAttainment(int materialTemplateId, int makeItemSubTypeId, bool isManual, bool isPerfect, int effectValue)
	{
		MaterialItem materialItem = Material.Instance[materialTemplateId];
		MakeItemSubTypeItem makeItemSubTypeItem = MakeItemSubType.Instance[makeItemSubTypeId];
		int num = materialItem.RequiredAttainment;
		sbyte b = materialItem.Grade;
		if (makeItemSubTypeItem.Result.ItemType == 8)
		{
			bool isMain = !makeItemSubTypeItem.IsOdd;
			b = GetHerbMaterialTempGrade(b, isManual, isMain);
			num = GlobalConfig.Instance.MakeMadicineAttainments[b];
		}
		if (isManual)
		{
			num += makeItemSubTypeItem.ExtraLifeSkill * (b + 1);
		}
		if (isPerfect)
		{
			num = num * 3 / 2;
		}
		num = GetRequiredLifeSkillAttainmentByBuildingEffect(num, effectValue);
		return (short)num;
	}

	public static short GetRequiredLifeSkillAttainmentByBuildingEffect(int attainment, int effectValue)
	{
		return (short)(attainment * (100 - effectValue) / 100);
	}

	public static bool CheckItemCanFeedChicken(ItemKey itemKey)
	{
		if (ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 1204)
		{
			return true;
		}
		if (itemKey.ItemType != 11)
		{
			return itemKey.ItemType == 5;
		}
		return true;
	}

	public static bool IsBuildingCanSoldItem(BuildingBlockItem config, ItemKey itemKey)
	{
		if (!ItemTemplateHelper.IsTransferable(itemKey.ItemType, itemKey.TemplateId))
		{
			return false;
		}
		if (itemKey.ItemType == 5)
		{
			return false;
		}
		if (config.TemplateId == 47)
		{
			sbyte itemType = itemKey.ItemType;
			if (itemType == 7 || itemType == 9)
			{
				return true;
			}
			return false;
		}
		if (config.RequireLifeSkillType == 14)
		{
			if (itemKey.ItemType == 7 || (itemKey.ItemType == 5 && Material.Instance[itemKey.TemplateId].RequiredLifeSkillType == 14 && Material.Instance[itemKey.TemplateId].RefiningEffect == -1))
			{
				return true;
			}
			return false;
		}
		if (config.RequireLifeSkillType == 5 && config.TemplateId == 121)
		{
			short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			if (itemKey.ItemType == 9 && itemSubType == 900)
			{
				return true;
			}
			return false;
		}
		if (config.RequireLifeSkillType == 5 && config.TemplateId == 122)
		{
			short itemSubType2 = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			if (itemKey.ItemType == 9 && itemSubType2 == 901)
			{
				return true;
			}
			return false;
		}
		if (config.RequireLifeSkillType == 5 && config.TemplateId == 124)
		{
			if (itemKey.ItemType == 9)
			{
				return true;
			}
			return false;
		}
		if (config.RequireLifeSkillType == 15)
		{
			short itemSubType3 = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			if (itemKey.ItemType == 10 || itemKey.ItemType == 6 || itemKey.ItemType == 11 || itemSubType3 == 1204 || itemSubType3 == 1201 || itemSubType3 == 1203 || (itemKey.ItemType == 12 && itemKey.TemplateId == 9) || (itemKey.ItemType == 12 && itemKey.TemplateId == 91))
			{
				return true;
			}
			return false;
		}
		if (config.RequireLifeSkillType == 8)
		{
			if (ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 800 || (itemKey.ItemType == 5 && Material.Instance[itemKey.TemplateId].RequiredLifeSkillType == 8 && Material.Instance[itemKey.TemplateId].RefiningEffect == -1))
			{
				return true;
			}
			return false;
		}
		if (config.RequireLifeSkillType == 9)
		{
			if (ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 801 || (itemKey.ItemType == 5 && Material.Instance[itemKey.TemplateId].RequiredLifeSkillType == 9 && Material.Instance[itemKey.TemplateId].RefiningEffect == -1))
			{
				return true;
			}
			return false;
		}
		if (config.RequireLifeSkillType == 6)
		{
			if ((itemKey.ItemType == 0 && Weapon.Instance[itemKey.TemplateId].ResourceType == 2) || (itemKey.ItemType == 1 && Armor.Instance[itemKey.TemplateId].ResourceType == 2) || (itemKey.ItemType == 2 && Accessory.Instance[itemKey.TemplateId].ResourceType == 2) || (itemKey.ItemType == 4 && Carrier.Instance[itemKey.TemplateId].ResourceType == 2) || (itemKey.ItemType == 3 && Clothing.Instance[itemKey.TemplateId].ResourceType == 2) || (itemKey.ItemType == 5 && Material.Instance[itemKey.TemplateId].RequiredLifeSkillType == 6))
			{
				return true;
			}
			return false;
		}
		if (config.RequireLifeSkillType == 7)
		{
			if ((itemKey.ItemType == 0 && Weapon.Instance[itemKey.TemplateId].ResourceType == 1) || (itemKey.ItemType == 1 && Armor.Instance[itemKey.TemplateId].ResourceType == 1) || (itemKey.ItemType == 2 && Accessory.Instance[itemKey.TemplateId].ResourceType == 1) || (itemKey.ItemType == 4 && Carrier.Instance[itemKey.TemplateId].ResourceType == 1) || (itemKey.ItemType == 3 && Clothing.Instance[itemKey.TemplateId].ResourceType == 1) || (itemKey.ItemType == 5 && Material.Instance[itemKey.TemplateId].RequiredLifeSkillType == 7))
			{
				return true;
			}
			return false;
		}
		if (config.RequireLifeSkillType == 11)
		{
			if ((itemKey.ItemType == 0 && Weapon.Instance[itemKey.TemplateId].ResourceType == 3) || (itemKey.ItemType == 1 && Armor.Instance[itemKey.TemplateId].ResourceType == 3) || (itemKey.ItemType == 2 && Accessory.Instance[itemKey.TemplateId].ResourceType == 3) || (itemKey.ItemType == 4 && Carrier.Instance[itemKey.TemplateId].ResourceType == 3) || (itemKey.ItemType == 3 && Clothing.Instance[itemKey.TemplateId].ResourceType == 3) || (itemKey.ItemType == 5 && Material.Instance[itemKey.TemplateId].RequiredLifeSkillType == 11))
			{
				return true;
			}
			return false;
		}
		if (config.RequireLifeSkillType == 10)
		{
			short itemSubType4 = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			if ((itemKey.ItemType == 0 && Weapon.Instance[itemKey.TemplateId].ResourceType == 4) || (itemKey.ItemType == 1 && Armor.Instance[itemKey.TemplateId].ResourceType == 4) || (itemKey.ItemType == 2 && Accessory.Instance[itemKey.TemplateId].ResourceType == 4) || (itemKey.ItemType == 4 && Carrier.Instance[itemKey.TemplateId].ResourceType == 4) || (itemKey.ItemType == 3 && Clothing.Instance[itemKey.TemplateId].ResourceType == 4) || (itemKey.ItemType == 5 && Material.Instance[itemKey.TemplateId].RequiredLifeSkillType == 10) || itemSubType4 == 1206)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static List<sbyte> GetBuildingCanSoldItemTypeList(BuildingBlockItem config, out short itemSubType)
	{
		itemSubType = -1;
		List<sbyte> list = new List<sbyte>();
		switch (config.RequireLifeSkillType)
		{
		case 14:
			list.Add(7);
			break;
		case 5:
			list.Add(9);
			break;
		case 15:
			list.Add(10);
			list.Add(6);
			list.Add(12);
			list.Add(11);
			break;
		case 8:
			list.Add(8);
			itemSubType = 800;
			break;
		case 9:
			list.Add(8);
			itemSubType = 801;
			break;
		case 6:
		case 11:
			list.Add(0);
			list.Add(1);
			list.Add(2);
			break;
		case 7:
			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(4);
			break;
		case 10:
			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.Add(12);
			break;
		}
		return list;
	}

	public static int GetExpandBuildingCost(int baseCost, int addCostPerLevel, sbyte buildingLevel)
	{
		return baseCost + baseCost * addCostPerLevel * buildingLevel / 100;
	}

	public static int GetShopManageProgressDelta(short buildingTemplateId, int attainment)
	{
		if (buildingTemplateId == 105)
		{
			return attainment;
		}
		return GlobalConfig.Instance.ShopManageProgressBaseDelta + attainment;
	}

	public static int GetResourceReturnOfRemoveBuilding(BuildingBlockItem config, sbyte level, sbyte resourceType, BuildingBlockData blockData, BuildingBlockDataEx blockDataEx)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		CValuePercent val = CValuePercent.op_Implicit((int)config.RemoveGetResourcePercent);
		int num = config.BaseBuildCost[resourceType];
		if (config.Class == EBuildingBlockClass.BornResource)
		{
			num *= level;
		}
		else if (level > 1)
		{
			BoolArray64 val2 = BoolArray64.op_Implicit(blockDataEx.LevelUnlockedFlags);
			IReadOnlyList<ResourceInfo> readOnlyList = config.TemplateId switch
			{
				46 => GlobalConfig.Instance.ResidentUnlockCost, 
				48 => GlobalConfig.Instance.WarehouseUnlockCost, 
				47 => GlobalConfig.Instance.ComfortableHouseUnlockCost, 
				_ => null, 
			};
			if (readOnlyList != null)
			{
				for (int i = 0; i < readOnlyList.Count; i++)
				{
					if (((BoolArray64)(ref val2))[i] && readOnlyList[i].ResourceType == resourceType)
					{
						num += readOnlyList[i].ResourceCount;
					}
				}
			}
		}
		return num * val;
	}

	public static bool BuildingCanUpgrade(BuildingBlockItem config)
	{
		if (config.MaxLevel > 1)
		{
			return config.TemplateId != 50;
		}
		return false;
	}

	public static int GetTaiwuShrineEffect(sbyte leaderFameType, int attainment)
	{
		return BuildingFormula.DefValue.TaiwuShrineEffect.Calculate(leaderFameType, attainment);
	}

	public static short CalcPawnshopKeepItemTimeIndex()
	{
		return GlobalConfig.BuildingPawnshopKeepItemTime[^1];
	}

	public static int CalcSafetyOrCultureFactorSettlementPickValue(short requiredValue, short value)
	{
		if (requiredValue > 0)
		{
			if (value <= requiredValue)
			{
				return 0;
			}
			return (value - requiredValue) / 5 + 5;
		}
		if (value >= -requiredValue)
		{
			return 0;
		}
		return (-requiredValue - value) / 5 + 5;
	}

	public static List<SettlementDisplayData> PickSafetyOrCultureFactorSettlements(BuildingBlockItem config, IList<SettlementDisplayData> source, out int addition)
	{
		List<SettlementDisplayData> list = new List<SettlementDisplayData>();
		list.AddRange(source);
		List<SettlementDisplayData> list2 = new List<SettlementDisplayData>();
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (list[num].AreaTemplateId == 136 || list[num].AreaTemplateId == 137 || list[num].AreaTemplateId == 0)
			{
				list.RemoveAt(num);
			}
		}
		addition = 0;
		if (config.RequireSafety != 0)
		{
			if (list.Count > 10 && config.RequireSafety > 0)
			{
				list.Sort((SettlementDisplayData l, SettlementDisplayData r) => r.Safety - l.Safety);
			}
			else if (list.Count > 10 && config.RequireSafety < 0)
			{
				list.Sort((SettlementDisplayData l, SettlementDisplayData r) => l.Safety - r.Safety);
			}
			for (int num2 = 0; num2 < Math.Min(10, list.Count); num2++)
			{
				if (list[num2].SettlementId >= 0)
				{
					addition += CalcSafetyOrCultureFactorSettlementPickValue(config.RequireSafety, list[num2].Safety);
					list2.Add(list[num2]);
				}
			}
		}
		if (config.RequireCulture != 0)
		{
			if (list.Count > 10 && config.RequireCulture > 0)
			{
				list.Sort((SettlementDisplayData l, SettlementDisplayData r) => r.Culture - l.Culture);
			}
			else if (list.Count > 10 && config.RequireCulture < 0)
			{
				list.Sort((SettlementDisplayData l, SettlementDisplayData r) => l.Culture - r.Culture);
			}
			for (int num3 = 0; num3 < Math.Min(10, list.Count); num3++)
			{
				if (list[num3].SettlementId >= 0)
				{
					addition += CalcSafetyOrCultureFactorSettlementPickValue(config.RequireCulture, list[num3].Culture);
					list2.Add(list[num3]);
				}
			}
		}
		return list2;
	}

	public static bool IsBuildingProduceMoneyAuthority(BuildingBlockItem buildingBlockItem, ShopEventItem shopEventItem)
	{
		if (buildingBlockItem.IsShop && shopEventItem != null)
		{
			return shopEventItem.ResourceGoods != -1;
		}
		return false;
	}

	public static bool IsBuildingSoldItem(BuildingBlockItem buildingBlockItem, ShopEventItem shopEventItem)
	{
		if (buildingBlockItem.IsShop && shopEventItem != null)
		{
			return shopEventItem.ExchangeResourceGoods != -1;
		}
		return false;
	}

	public static sbyte GetBuildingResourceGoodsOrExchangeResourceGoodsType(BuildingBlockItem buildingBlockItem, ShopEventItem shopEventItem)
	{
		if (buildingBlockItem.IsShop && shopEventItem != null && (shopEventItem.ResourceGoods != -1 || shopEventItem.ExchangeResourceGoods != -1))
		{
			if (shopEventItem.ResourceGoods != -1)
			{
				return shopEventItem.ResourceGoods;
			}
			return shopEventItem.ExchangeResourceGoods;
		}
		return -1;
	}

	public static bool BuildingRequireSafetyOrCulture(BuildingBlockItem buildingBlockItem)
	{
		if (buildingBlockItem.RequireCulture == 0)
		{
			return buildingBlockItem.RequireSafety != 0;
		}
		return true;
	}

	public static int CalcCricketRegenTime(sbyte jarGrade)
	{
		return 3 - Grade.GetGroup(jarGrade);
	}

	public static (bool, int count) HasBuildingCore(BuildingBlockItem config, List<ItemDisplayData> canUseBuildingCore)
	{
		if (canUseBuildingCore != null)
		{
			foreach (ItemDisplayData item in canUseBuildingCore)
			{
				if (item.Key.ItemType == 12 && item.Key.TemplateId == config.BuildingCoreItem)
				{
					return (true, count: item.Amount);
				}
			}
		}
		return (false, count: 0);
	}

	public static bool HasEffect(BuildingBlockItem config)
	{
		if (config == null)
		{
			return false;
		}
		if (config.TemplateId == 52)
		{
			return true;
		}
		List<short> expandInfos = config.ExpandInfos;
		if (expandInfos != null && expandInfos.Count > 0)
		{
			return true;
		}
		if (config.UpgradeMakeItem || config.ReduceMakeRequirementLifeSkillType > -1)
		{
			return true;
		}
		return false;
	}

	public static bool BuildingIsShopWithEvent(BuildingBlockItem config)
	{
		if (config.IsShop)
		{
			List<short> succesEvent = config.SuccesEvent;
			if (succesEvent == null)
			{
				return false;
			}
			return succesEvent.Count > 0;
		}
		return false;
	}

	public static sbyte GetBuildingSlotCount(short buildingTemplateId)
	{
		switch (buildingTemplateId)
		{
		case 47:
			return (sbyte)GlobalConfig.Instance.FeastCount;
		case 105:
			return 1;
		default:
		{
			BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingTemplateId];
			int num = 1;
			for (int i = 0; i < buildingBlockItem.ExpandInfos.Count; i++)
			{
				BuildingScaleItem buildingScaleItem = BuildingScale.Instance[buildingBlockItem.ExpandInfos[i]];
				if (buildingScaleItem.Class == EBuildingScaleClass.Slot)
				{
					num = buildingScaleItem.LevelEffect[0];
					break;
				}
			}
			return (sbyte)num;
		}
		}
	}

	public static sbyte GetBuildingFixedBuff(short buildingTemplateId)
	{
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingTemplateId];
		sbyte result = 0;
		if (buildingBlockItem.ExpandInfos == null)
		{
			return result;
		}
		for (int i = 0; i < buildingBlockItem.ExpandInfos.Count; i++)
		{
			BuildingScaleItem buildingScaleItem = BuildingScale.Instance[buildingBlockItem.ExpandInfos[i]];
			if (buildingScaleItem.Class == EBuildingScaleClass.FixedBuff)
			{
				result = (sbyte)buildingScaleItem.LevelEffect[0];
				break;
			}
		}
		return result;
	}

	public static sbyte GetBuildingLevelEffect(short buildingTemplateId, int level)
	{
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[buildingTemplateId];
		sbyte result = 0;
		if (buildingBlockItem.ExpandInfos == null)
		{
			return result;
		}
		for (int i = 0; i < buildingBlockItem.ExpandInfos.Count; i++)
		{
			BuildingScaleItem buildingScaleItem = BuildingScale.Instance[buildingBlockItem.ExpandInfos[i]];
			if (buildingScaleItem.Class == EBuildingScaleClass.LevelEffect)
			{
				result = (sbyte)buildingScaleItem.LevelEffect.GetOrLast(level - 1);
				break;
			}
		}
		return result;
	}

	public static int CalcRepairBuildingCost(BuildingBlockData blockData, BuildingBlockItem config)
	{
		return (config.MaxDurability - blockData.Durability) * config.BaseRepairCost;
	}

	public unsafe static (short, short) CalcVillagerRoleLifeSkillAndPersonalityType(short roleTemplateId, LifeSkillShorts attainments, Personalities personalities)
	{
		switch (roleTemplateId)
		{
		case 0:
			return (14, 6);
		case 1:
		{
			Span<short> span4 = stackalloc short[4] { 6, 7, 10, 11 };
			short* ptr4 = stackalloc short[span4.Length];
			for (int l = 0; l < span4.Length; l++)
			{
				ptr4[l] = attainments.Get(span4[l]);
			}
			return (span4[CollectionUtils.GetMaxIndex(ptr4, span4.Length)], 4);
		}
		case 2:
		{
			Span<short> span2 = stackalloc short[2] { 8, 9 };
			short* ptr2 = stackalloc short[span2.Length];
			for (int j = 0; j < span2.Length; j++)
			{
				ptr2[j] = attainments.Get(span2[j]);
			}
			return (span2[CollectionUtils.GetMaxIndex(ptr2, span2.Length)], 0);
		}
		case 3:
			return (15, 2);
		case 4:
		{
			Span<short> span3 = stackalloc short[4] { 0, 1, 2, 3 };
			short* ptr3 = stackalloc short[span3.Length];
			for (int k = 0; k < span3.Length; k++)
			{
				ptr3[k] = attainments.Get(span3[k]);
			}
			return (span3[CollectionUtils.GetMaxIndex(ptr3, span3.Length)], 1);
		}
		case 5:
		{
			Span<short> span = stackalloc short[2] { 13, 12 };
			short* ptr = stackalloc short[span.Length];
			for (int i = 0; i < span.Length; i++)
			{
				ptr[i] = attainments.Get(span[i]);
			}
			return (span[CollectionUtils.GetMaxIndex(ptr, span.Length)], 3);
		}
		case 6:
			return (4, 5);
		default:
			return (-1, -1);
		}
	}

	public static bool NeedCostResourceToBuild(BuildingBlockItem buildingBlockConfigItem)
	{
		return buildingBlockConfigItem.Class != EBuildingBlockClass.BornResource;
	}

	public static int[] GetFinalMaintenanceCost(BuildingBlockItem configData)
	{
		int[] array = new int[8];
		foreach (ResourceInfo item in configData.BaseMaintenanceCost)
		{
			array[item.ResourceType] += item.ResourceCount;
		}
		return array;
	}

	public static int CalcResourceBlockTotalEffectValue(int formulaTemplateId, Span<int> baseValues)
	{
		BuildingFormulaItem formula = BuildingFormula.Instance[formulaTemplateId];
		int num = 0;
		for (int i = 0; i < baseValues.Length; i++)
		{
			num += formula.Calculate(baseValues[i]);
		}
		return num;
	}

	public static int GetResourceBlockEffectPercentage(int index)
	{
		if (5 <= index)
		{
			return 0;
		}
		return 100 * (5 - index) / 5;
	}

	public static int GetResourceBlockEffectPercentageValue(int level, int percentage)
	{
		return level * percentage / 100;
	}

	public static List<short> GetResourceBlockEffectScaleTemplateIdList(short templateId)
	{
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[templateId];
		List<short> list = new List<short>();
		if (buildingBlockItem.ExpandInfos.Count > 0)
		{
			foreach (short expandInfo in buildingBlockItem.ExpandInfos)
			{
				if (BuildingScale.Instance[expandInfo].Class != EBuildingScaleClass.LevelEffect)
				{
					list.Add(expandInfo);
				}
			}
		}
		return list;
	}

	public static bool HaveResourceBlockEffect(short buildingScaleTemplateId)
	{
		BuildingScaleItem buildingScaleItem = BuildingScale.Instance[buildingScaleTemplateId];
		if (buildingScaleItem.Class == EBuildingScaleClass.MemberExpIncome || buildingScaleItem.Class == EBuildingScaleClass.MemberResourceIncome || buildingScaleItem.Formula > 0)
		{
			return true;
		}
		return false;
	}

	public static bool HaveUsefulResourceBlockEffect(short buildingBlockTemplateId, int rank)
	{
		if (BuildingBlock.Instance[buildingBlockTemplateId].Class == EBuildingBlockClass.BornResource)
		{
			return rank < 5;
		}
		return false;
	}
}
