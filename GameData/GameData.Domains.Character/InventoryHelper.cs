using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Item;
using GameData.Domains.Organization;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character;

public static class InventoryHelper
{
	public static int GetTotalValue(this Inventory inventory)
	{
		int num = 0;
		foreach (KeyValuePair<ItemKey, int> item in inventory.Items)
		{
			item.Deconstruct(out var key, out var value);
			ItemKey itemKey = key;
			int num2 = value;
			int num3 = (ItemTemplateHelper.IsMiscResource(itemKey.ItemType, itemKey.TemplateId) ? GlobalConfig.ResourcesWorth[ItemTemplateHelper.GetMiscResourceType(itemKey.ItemType, itemKey.TemplateId)] : DomainManager.Item.GetValue(itemKey));
			num += num3 * num2;
		}
		return num;
	}

	public static int GetTotalContribution(this Inventory inventory, Settlement settlement)
	{
		int num = 0;
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		foreach (KeyValuePair<ItemKey, int> item in inventory.Items)
		{
			item.Deconstruct(out var key, out var value);
			ItemKey itemKey = key;
			int amount = value;
			num += (ItemTemplateHelper.IsMiscResource(itemKey.ItemType, itemKey.TemplateId) ? DomainManager.Organization.CalcResourceContribution(orgTemplateId, ItemTemplateHelper.GetMiscResourceType(itemKey.ItemType, itemKey.TemplateId), amount) : DomainManager.Organization.CalcItemContribution(settlement, itemKey, amount));
		}
		return num;
	}

	public static int GetTotalWeight(this Inventory inventory)
	{
		int num = 0;
		foreach (KeyValuePair<ItemKey, int> item in inventory.Items)
		{
			item.Deconstruct(out var key, out var value);
			ItemKey itemKey = key;
			int num2 = value;
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			num += baseItem.GetWeight() * num2;
		}
		return num;
	}

	public static ItemKey GetBestCraftTool(this Inventory inventory, sbyte lifeSkillType, sbyte targetGrade, out short durabilityCost)
	{
		ItemKey result = ItemKey.Invalid;
		int num = int.MinValue;
		short num2 = short.MaxValue;
		durabilityCost = 0;
		foreach (var (itemKey2, _) in inventory.Items)
		{
			if (itemKey2.ItemType != 6)
			{
				continue;
			}
			CraftToolItem craftToolItem = Config.CraftTool.Instance[itemKey2.TemplateId];
			if (craftToolItem.RequiredLifeSkillTypes.Contains(lifeSkillType) && craftToolItem.AttainmentBonus > num)
			{
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey2);
				durabilityCost = craftToolItem.DurabilityCost[targetGrade];
				if (baseItem.GetCurrDurability() >= durabilityCost && durabilityCost <= num2)
				{
					num = craftToolItem.AttainmentBonus;
					result = itemKey2;
					num2 = durabilityCost;
				}
			}
		}
		return result;
	}

	public static ItemKey GetWorstUsableCraftTool(this Inventory inventory, sbyte lifeSkillType, short requiredAttainment, short targetCharAttainment, sbyte targetGrade, out short durabilityCost)
	{
		ItemKey result = ItemKey.Invalid;
		short num = short.MaxValue;
		durabilityCost = 0;
		foreach (ItemKey key in inventory.Items.Keys)
		{
			if (key.ItemType != 6)
			{
				continue;
			}
			ItemBase baseItem = DomainManager.Item.GetBaseItem(key);
			if (baseItem.GetCurrDurability() <= 0)
			{
				continue;
			}
			CraftToolItem craftToolItem = Config.CraftTool.Instance[key.TemplateId];
			if (craftToolItem.RequiredLifeSkillTypes.Contains(lifeSkillType) && targetCharAttainment + craftToolItem.AttainmentBonus >= requiredAttainment)
			{
				short num2 = craftToolItem.DurabilityCost[targetGrade];
				if (baseItem.GetCurrDurability() >= num2 && num2 <= num)
				{
					result = key;
					num = num2;
					durabilityCost = num2;
				}
			}
		}
		return result;
	}

	public static void GetCraftMaterials(this Inventory inventory, sbyte lifeSkillType, List<ItemKey> materials)
	{
		materials.Clear();
		foreach (var (item, _) in inventory.Items)
		{
			if (item.ItemType == 5)
			{
				MaterialItem materialItem = Config.Material.Instance[item.TemplateId];
				if (materialItem.RequiredLifeSkillType == lifeSkillType && materialItem.CraftableItemTypes != null && materialItem.CraftableItemTypes.Count > 0)
				{
					materials.Add(item);
				}
			}
		}
	}

	public static void GetRepairableItem(this Inventory inventory, List<ItemKey> itemKeys)
	{
		itemKeys.Clear();
		foreach (var (itemKey2, _) in inventory.Items)
		{
			if (ItemTemplateHelper.IsRepairable(itemKey2.ItemType, itemKey2.TemplateId))
			{
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey2);
				if (baseItem.GetCurrDurability() < baseItem.GetMaxDurability())
				{
					itemKeys.Add(itemKey2);
				}
			}
		}
	}

	public static bool HasRepairableItem(this Inventory inventory)
	{
		foreach (var (itemKey2, _) in inventory.Items)
		{
			if (ItemTemplateHelper.IsRepairable(itemKey2.ItemType, itemKey2.TemplateId))
			{
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey2);
				if (baseItem.GetCurrDurability() < baseItem.GetMaxDurability())
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void SelectPoisonsToAdd(this Inventory inventory, IRandomSource random, short lifeSkillAttainment, sbyte grade, ItemKey itemToAttachPoisonOn, ref SpanList<ItemKey> selectedPoisons)
	{
		if (!ItemTemplateHelper.IsPoisonable(itemToAttachPoisonOn.ItemType, itemToAttachPoisonOn.TemplateId))
		{
			return;
		}
		Span<sbyte> span = stackalloc sbyte[6];
		SpanList<sbyte> spanList = span;
		if (ModificationStateHelper.IsActive(itemToAttachPoisonOn.ModificationState, 1))
		{
			FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(itemToAttachPoisonOn);
			short medicineTemplateId = poisonEffects.GetMedicineTemplateId();
			if (!MixedPoisonType.IsMixedPoisonItem(medicineTemplateId))
			{
				MedicineItem medicineItem = Config.Medicine.Instance[medicineTemplateId];
				spanList.Add(medicineItem.PoisonType);
			}
			else
			{
				sbyte b = MixedPoisonType.FromMedicineTemplateId(medicineTemplateId);
				spanList.AddRange(MixedPoisonType.ToPoisonTypes[b]);
			}
		}
		Span<ItemKey> span2 = stackalloc ItemKey[6];
		Span<int> span3 = stackalloc int[6];
		span2.Fill(ItemKey.Invalid);
		span3.Fill(127);
		foreach (ItemKey key in inventory.Items.Keys)
		{
			if (key.ItemType != 8)
			{
				continue;
			}
			MedicineItem medicineItem2 = Config.Medicine.Instance[key.TemplateId];
			if (medicineItem2.EffectType != EMedicineEffectType.ApplyPoison)
			{
				continue;
			}
			sbyte poisonType = medicineItem2.PoisonType;
			short num = GlobalConfig.Instance.PoisonAttainments[medicineItem2.Grade];
			if (lifeSkillAttainment >= num && !spanList.Contains(poisonType))
			{
				int num2 = medicineItem2.Grade - grade;
				int num3 = num2 * num2;
				if (num3 <= span3[poisonType])
				{
					span3[poisonType] = num3;
					span2[poisonType] = key;
				}
			}
		}
		for (int i = 0; i < 6; i++)
		{
			ItemKey value = span2[i];
			if (value.IsValid())
			{
				selectedPoisons.Add(value);
			}
		}
		if (selectedPoisons.Count != 0)
		{
			selectedPoisons.Shuffle(random);
			int num4 = 3 - spanList.Count;
			if (selectedPoisons.Count > num4)
			{
				selectedPoisons.RemoveRange(num4, 6);
			}
		}
	}
}
