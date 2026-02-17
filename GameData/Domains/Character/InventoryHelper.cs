using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Item;
using GameData.Domains.Organization;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character
{
	// Token: 0x02000814 RID: 2068
	public static class InventoryHelper
	{
		// Token: 0x0600749A RID: 29850 RVA: 0x0044501C File Offset: 0x0044321C
		public static int GetTotalValue(this Inventory inventory)
		{
			int totalValue = 0;
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				int amount = num;
				int value = ItemTemplateHelper.IsMiscResource(itemKey.ItemType, itemKey.TemplateId) ? ((int)GlobalConfig.ResourcesWorth[(int)ItemTemplateHelper.GetMiscResourceType(itemKey.ItemType, itemKey.TemplateId)]) : DomainManager.Item.GetValue(itemKey);
				totalValue += value * amount;
			}
			return totalValue;
		}

		// Token: 0x0600749B RID: 29851 RVA: 0x004450CC File Offset: 0x004432CC
		public static int GetTotalContribution(this Inventory inventory, Settlement settlement)
		{
			int totalValue = 0;
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				int amount = num;
				totalValue += (ItemTemplateHelper.IsMiscResource(itemKey.ItemType, itemKey.TemplateId) ? DomainManager.Organization.CalcResourceContribution(orgTemplateId, ItemTemplateHelper.GetMiscResourceType(itemKey.ItemType, itemKey.TemplateId), amount) : DomainManager.Organization.CalcItemContribution(settlement, itemKey, amount));
			}
			return totalValue;
		}

		// Token: 0x0600749C RID: 29852 RVA: 0x00445188 File Offset: 0x00443388
		public static int GetTotalWeight(this Inventory inventory)
		{
			int weight = 0;
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				int amount = num;
				ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
				weight += item.GetWeight() * amount;
			}
			return weight;
		}

		// Token: 0x0600749D RID: 29853 RVA: 0x00445210 File Offset: 0x00443410
		public static ItemKey GetBestCraftTool(this Inventory inventory, sbyte lifeSkillType, sbyte targetGrade, out short durabilityCost)
		{
			ItemKey bestTool = ItemKey.Invalid;
			int bestToolAttainmentBonus = int.MinValue;
			short minCost = short.MaxValue;
			durabilityCost = 0;
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				bool flag = itemKey.ItemType == 6;
				if (flag)
				{
					CraftToolItem toolCfg = Config.CraftTool.Instance[itemKey.TemplateId];
					bool flag2 = toolCfg.RequiredLifeSkillTypes.Contains(lifeSkillType) && (int)toolCfg.AttainmentBonus > bestToolAttainmentBonus;
					if (flag2)
					{
						ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
						durabilityCost = toolCfg.DurabilityCost[(int)targetGrade];
						bool flag3 = baseItem.GetCurrDurability() < durabilityCost;
						if (!flag3)
						{
							bool flag4 = durabilityCost > minCost;
							if (!flag4)
							{
								bestToolAttainmentBonus = (int)toolCfg.AttainmentBonus;
								bestTool = itemKey;
								minCost = durabilityCost;
							}
						}
					}
				}
			}
			return bestTool;
		}

		// Token: 0x0600749E RID: 29854 RVA: 0x00445328 File Offset: 0x00443528
		public static ItemKey GetWorstUsableCraftTool(this Inventory inventory, sbyte lifeSkillType, short requiredAttainment, short targetCharAttainment, sbyte targetGrade, out short durabilityCost)
		{
			ItemKey selectedTool = ItemKey.Invalid;
			short minCost = short.MaxValue;
			durabilityCost = 0;
			foreach (ItemKey toolItemKey in inventory.Items.Keys)
			{
				bool flag = toolItemKey.ItemType != 6;
				if (!flag)
				{
					ItemBase tool = DomainManager.Item.GetBaseItem(toolItemKey);
					bool flag2 = tool.GetCurrDurability() <= 0;
					if (!flag2)
					{
						CraftToolItem cfg = Config.CraftTool.Instance[toolItemKey.TemplateId];
						bool flag3 = !cfg.RequiredLifeSkillTypes.Contains(lifeSkillType);
						if (!flag3)
						{
							bool flag4 = targetCharAttainment + cfg.AttainmentBonus < requiredAttainment;
							if (!flag4)
							{
								short cost = cfg.DurabilityCost[(int)targetGrade];
								bool flag5 = tool.GetCurrDurability() < cost;
								if (!flag5)
								{
									bool flag6 = cost > minCost;
									if (!flag6)
									{
										selectedTool = toolItemKey;
										minCost = cost;
										durabilityCost = cost;
									}
								}
							}
						}
					}
				}
			}
			return selectedTool;
		}

		// Token: 0x0600749F RID: 29855 RVA: 0x00445448 File Offset: 0x00443648
		public static void GetCraftMaterials(this Inventory inventory, sbyte lifeSkillType, List<ItemKey> materials)
		{
			materials.Clear();
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				bool flag = itemKey.ItemType != 5;
				if (!flag)
				{
					MaterialItem materialCfg = Config.Material.Instance[itemKey.TemplateId];
					bool flag2 = materialCfg.RequiredLifeSkillType == lifeSkillType && materialCfg.CraftableItemTypes != null && materialCfg.CraftableItemTypes.Count > 0;
					if (flag2)
					{
						materials.Add(itemKey);
					}
				}
			}
		}

		// Token: 0x060074A0 RID: 29856 RVA: 0x0044550C File Offset: 0x0044370C
		public static void GetRepairableItem(this Inventory inventory, List<ItemKey> itemKeys)
		{
			itemKeys.Clear();
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				bool flag = !ItemTemplateHelper.IsRepairable(itemKey.ItemType, itemKey.TemplateId);
				if (!flag)
				{
					ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
					bool flag2 = item.GetCurrDurability() >= item.GetMaxDurability();
					if (!flag2)
					{
						itemKeys.Add(itemKey);
					}
				}
			}
		}

		// Token: 0x060074A1 RID: 29857 RVA: 0x004455C0 File Offset: 0x004437C0
		public static bool HasRepairableItem(this Inventory inventory)
		{
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventory.Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				bool flag = !ItemTemplateHelper.IsRepairable(itemKey.ItemType, itemKey.TemplateId);
				if (!flag)
				{
					ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
					bool flag2 = item.GetCurrDurability() < item.GetMaxDurability();
					if (flag2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060074A2 RID: 29858 RVA: 0x0044566C File Offset: 0x0044386C
		public unsafe static void SelectPoisonsToAdd(this Inventory inventory, IRandomSource random, short lifeSkillAttainment, sbyte grade, ItemKey itemToAttachPoisonOn, ref SpanList<ItemKey> selectedPoisons)
		{
			bool flag = !ItemTemplateHelper.IsPoisonable(itemToAttachPoisonOn.ItemType, itemToAttachPoisonOn.TemplateId);
			if (!flag)
			{
				Span<sbyte> span = new Span<sbyte>(stackalloc byte[(UIntPtr)6], 6);
				SpanList<sbyte> exceptions = span;
				bool flag2 = ModificationStateHelper.IsActive(itemToAttachPoisonOn.ModificationState, 1);
				if (flag2)
				{
					FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(itemToAttachPoisonOn);
					short poisonTemplateId = poisonEffects.GetMedicineTemplateId();
					bool flag3 = !MixedPoisonType.IsMixedPoisonItem(poisonTemplateId);
					if (flag3)
					{
						MedicineItem poisonCfg = Config.Medicine.Instance[poisonTemplateId];
						exceptions.Add(poisonCfg.PoisonType);
					}
					else
					{
						sbyte mixedPoisonType = MixedPoisonType.FromMedicineTemplateId(poisonTemplateId);
						exceptions.AddRange(MixedPoisonType.ToPoisonTypes[(int)mixedPoisonType]);
					}
				}
				Span<ItemKey> span2 = new Span<ItemKey>(stackalloc byte[checked(unchecked((UIntPtr)6) * (UIntPtr)sizeof(ItemKey))], 6);
				Span<ItemKey> selectablePoisons = span2;
				Span<int> span3 = new Span<int>(stackalloc byte[(UIntPtr)24], 6);
				Span<int> minDeviations = span3;
				selectablePoisons.Fill(ItemKey.Invalid);
				minDeviations.Fill(127);
				foreach (ItemKey itemKey in inventory.Items.Keys)
				{
					bool flag4 = itemKey.ItemType != 8;
					if (!flag4)
					{
						MedicineItem medicineCfg = Config.Medicine.Instance[itemKey.TemplateId];
						bool flag5 = medicineCfg.EffectType != EMedicineEffectType.ApplyPoison;
						if (!flag5)
						{
							sbyte poisonType = medicineCfg.PoisonType;
							short attainmentRequired = GlobalConfig.Instance.PoisonAttainments[(int)medicineCfg.Grade];
							bool flag6 = lifeSkillAttainment < attainmentRequired;
							if (!flag6)
							{
								bool flag7 = exceptions.Contains(poisonType);
								if (!flag7)
								{
									int diff = (int)(medicineCfg.Grade - grade);
									int currDeviation = diff * diff;
									bool flag8 = currDeviation > *minDeviations[(int)poisonType];
									if (!flag8)
									{
										*minDeviations[(int)poisonType] = currDeviation;
										*selectablePoisons[(int)poisonType] = itemKey;
									}
								}
							}
						}
					}
				}
				for (int i = 0; i < 6; i++)
				{
					ItemKey poisonItemKey = *selectablePoisons[i];
					bool flag9 = !poisonItemKey.IsValid();
					if (!flag9)
					{
						selectedPoisons.Add(poisonItemKey);
					}
				}
				bool flag10 = selectedPoisons.Count == 0;
				if (!flag10)
				{
					selectedPoisons.Shuffle(random);
					int maxCount = 3 - exceptions.Count;
					bool flag11 = selectedPoisons.Count > maxCount;
					if (flag11)
					{
						selectedPoisons.RemoveRange(maxCount, 6);
					}
				}
			}
		}
	}
}
