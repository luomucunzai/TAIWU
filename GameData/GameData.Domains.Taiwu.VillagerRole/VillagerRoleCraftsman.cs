using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization.TaiwuVillageStoragesRecord;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.VillagerRole;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
public class VillagerRoleCraftsman : VillagerRoleBase
{
	public readonly sbyte[] CraftLifeSkillTypes = new sbyte[4] { 6, 7, 10, 11 };

	public const int GainRefineMaterialGradeOffsetChance = 50;

	public const int GainRefineMaterialGradeOffset = -1;

	public override short RoleTemplateId => 1;

	public override void ExecuteFixedAction(DataContext context)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (ArrangementTemplateId >= 0 || (WorkData != null && WorkData.WorkType == 1))
		{
			return;
		}
		BoolArray64 autoActionStates = base.AutoActionStates;
		bool flag = ((BoolArray64)(ref autoActionStates))[1] && base.SettlementTreasury.Inventory.HasRepairableItem();
		autoActionStates = base.AutoActionStates;
		bool flag2 = ((BoolArray64)(ref autoActionStates))[2];
		if (flag && flag2)
		{
			if (context.Random.Next(2) == 0)
			{
				AutoRepairItem(context);
			}
			else
			{
				AutoGainRefineMaterial(context);
			}
		}
		else if (flag)
		{
			AutoRepairItem(context);
		}
		else if (flag2)
		{
			AutoGainRefineMaterial(context);
		}
	}

	private bool AutoRepairItem(DataContext context)
	{
		int num = 0;
		int num2 = VillagerRoleFormulaImpl.Calculate(41, base.Personality);
		List<ItemKey> list = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		base.SettlementTreasury.Inventory.GetRepairableItem(list);
		foreach (ItemKey item in list)
		{
			if (num >= num2)
			{
				break;
			}
			EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(item);
			sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(item.ItemType, item.TemplateId);
			if (DomainManager.Taiwu.GetTaiwuBuildingBlockData(GetLackBuildingTemplateId(craftRequiredLifeSkillType)) == null)
			{
				continue;
			}
			short currDurability = baseEquipment.GetCurrDurability();
			sbyte craftRequiredResourceType = ItemTemplateHelper.GetCraftRequiredResourceType(item.ItemType, item.TemplateId);
			int repairNeedResourceCount = ItemTemplateHelper.GetRepairNeedResourceCount(baseEquipment.GetMaterialResources(), item, currDurability);
			if (!CheckResource(craftRequiredResourceType, repairNeedResourceCount))
			{
				continue;
			}
			short repairRequiredAttainment = ItemTemplateHelper.GetRepairRequiredAttainment(item.ItemType, item.TemplateId, currDurability);
			short lifeSkillAttainment = Character.GetLifeSkillAttainment(craftRequiredLifeSkillType);
			short durabilityCost;
			ItemKey worstUsableCraftTool = base.SettlementTreasury.Inventory.GetWorstUsableCraftTool(craftRequiredLifeSkillType, repairRequiredAttainment, lifeSkillAttainment, baseEquipment.GetGrade(), out durabilityCost);
			if (worstUsableCraftTool.IsValid())
			{
				GameData.Domains.Item.CraftTool element_CraftTools = DomainManager.Item.GetElement_CraftTools(worstUsableCraftTool.Id);
				ItemBase.OfflineRepairItem(element_CraftTools, baseEquipment, baseEquipment.GetMaxDurability(), durabilityCost);
				baseEquipment.SetCurrDurability(baseEquipment.GetCurrDurability(), context);
				short currDurability2 = element_CraftTools.GetCurrDurability();
				if (currDurability2 <= 0)
				{
					CostItem(context, worstUsableCraftTool, 1, deleteItem: true);
				}
				else
				{
					element_CraftTools.SetCurrDurability(currDurability2, context);
				}
				CostResource(context, craftRequiredResourceType, repairNeedResourceCount);
				int id = Character.GetId();
				int currDate = DomainManager.World.GetCurrDate();
				Location location = Character.GetLocation();
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				lifeRecordCollection.AddVillagerRepairItem0(id, currDate, location, item.ItemType, item.TemplateId);
				TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
				taiwuVillageStoragesRecordCollection.AddVillagerRepairItem(currDate, TaiwuVillageStorageType.Treasury, id, item.ItemType, item.TemplateId);
				num++;
			}
		}
		return num > 0;
	}

	private void AutoGainRefineMaterial(DataContext context)
	{
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		int num = VillagerRoleFormulaImpl.Calculate(42, base.Personality);
		if (context.Random.Next(100) >= num)
		{
			return;
		}
		Span<int> pWeightedElements = stackalloc int[CraftLifeSkillTypes.Length];
		for (int i = 0; i < CraftLifeSkillTypes.Length; i++)
		{
			sbyte lifeSkillType = CraftLifeSkillTypes[i];
			pWeightedElements[i] = Character.GetLifeSkillAttainment(lifeSkillType);
		}
		int randomWeightedElement = CollectionUtils.GetRandomWeightedElement(context.Random, pWeightedElements);
		sbyte lifeSkillType2 = CraftLifeSkillTypes[randomWeightedElement];
		short lifeSkillAttainment = Character.GetLifeSkillAttainment(lifeSkillType2);
		int num2 = VillagerRoleFormulaImpl.Calculate(43, lifeSkillAttainment);
		if (context.Random.Next(100) < 50)
		{
			num2 += -1;
		}
		sbyte b = Config.Material.Instance.Where((MaterialItem m) => m.RequiredLifeSkillType == lifeSkillType2 && m.RefiningEffect >= 0).Max((MaterialItem m) => m.Grade);
		int grade = Math.Clamp(num2, 0, b);
		MaterialItem materialItem = Config.Material.Instance.Where((MaterialItem m) => m.RequiredLifeSkillType == lifeSkillType2 && m.RefiningEffect >= 0).FirstOrDefault((MaterialItem m) => m.Grade == grade);
		if (materialItem == null)
		{
			return;
		}
		int id = Character.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
		if (grade < b && base.HasChickenUpgradeEffect)
		{
			BoolArray64 autoActionStates = base.AutoActionStates;
			if (((BoolArray64)(ref autoActionStates))[3])
			{
				int num3 = VillagerRoleFormulaImpl.Calculate(44, base.Personality);
				if (context.Random.Next(100) < num3)
				{
					int upgradeGrade = Math.Clamp(grade + 1, 0, b);
					MaterialItem materialItem2 = Config.Material.Instance.Where((MaterialItem m) => m.RequiredLifeSkillType == lifeSkillType2 && m.RefiningEffect >= 0).FirstOrDefault((MaterialItem m) => m.Grade == upgradeGrade);
					if (materialItem2 != null && materialItem2.RequiredAttainment <= lifeSkillAttainment)
					{
						ItemKey itemKey = DomainManager.Item.CreateMaterial(context, materialItem2.TemplateId);
						GainItem(context, itemKey, 1);
						lifeRecordCollection.AddVillagerUpgradeRefineItem(id, currDate, materialItem.ItemType, materialItem.TemplateId, itemKey.ItemType, itemKey.TemplateId);
						taiwuVillageStoragesRecordCollection.AddVillagerUpgradeRefineItem1(currDate, TaiwuVillageStorageType.Treasury, id, materialItem.ItemType, materialItem.TemplateId, itemKey.ItemType, itemKey.TemplateId);
						return;
					}
				}
			}
		}
		ItemKey itemKey2 = DomainManager.Item.CreateMaterial(context, materialItem.TemplateId);
		GainItem(context, itemKey2, 1);
		taiwuVillageStoragesRecordCollection.AddVillagerGetRefineItem(currDate, TaiwuVillageStorageType.Treasury, id, itemKey2.ItemType, itemKey2.TemplateId);
		lifeRecordCollection.AddVillagerGetRefineItem(id, currDate, itemKey2.ItemType, itemKey2.TemplateId);
	}

	private short GetLackBuildingTemplateId(sbyte lifeSkillType)
	{
		bool condition = (((uint)(lifeSkillType - 6) <= 1u || (uint)(lifeSkillType - 10) <= 1u) ? true : false);
		Tester.Assert(condition);
		return lifeSkillType switch
		{
			6 => 129, 
			7 => 139, 
			10 => 169, 
			_ => 179, 
		};
	}
}
