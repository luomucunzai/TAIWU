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

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x0200004D RID: 77
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public class VillagerRoleCraftsman : VillagerRoleBase
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600139B RID: 5019 RVA: 0x0013906D File Offset: 0x0013726D
		public override short RoleTemplateId
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00139070 File Offset: 0x00137270
		public override void ExecuteFixedAction(DataContext context)
		{
			bool flag = this.ArrangementTemplateId >= 0;
			if (!flag)
			{
				bool flag2 = this.WorkData != null && this.WorkData.WorkType == 1;
				if (!flag2)
				{
					bool canAutoRepairItem = base.AutoActionStates[1] && base.SettlementTreasury.Inventory.HasRepairableItem();
					bool canAutoGainRefineMaterial = base.AutoActionStates[2];
					bool flag3 = canAutoRepairItem && canAutoGainRefineMaterial;
					if (flag3)
					{
						int random = context.Random.Next(2);
						bool flag4 = random == 0;
						if (flag4)
						{
							this.AutoRepairItem(context);
						}
						else
						{
							this.AutoGainRefineMaterial(context);
						}
					}
					else
					{
						bool flag5 = canAutoRepairItem;
						if (flag5)
						{
							this.AutoRepairItem(context);
						}
						else
						{
							bool flag6 = canAutoGainRefineMaterial;
							if (flag6)
							{
								this.AutoGainRefineMaterial(context);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00139144 File Offset: 0x00137344
		private bool AutoRepairItem(DataContext context)
		{
			int repairCount = 0;
			int repairMaxCount = VillagerRoleFormulaImpl.Calculate(41, base.Personality);
			List<ItemKey> repairableItems = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
			base.SettlementTreasury.Inventory.GetRepairableItem(repairableItems);
			foreach (ItemKey itemKey in repairableItems)
			{
				bool flag = repairCount >= repairMaxCount;
				if (flag)
				{
					break;
				}
				EquipmentBase equipment = DomainManager.Item.GetBaseEquipment(itemKey);
				sbyte requiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey.ItemType, itemKey.TemplateId);
				bool flag2 = DomainManager.Taiwu.GetTaiwuBuildingBlockData(this.GetLackBuildingTemplateId(requiredLifeSkillType)) == null;
				if (!flag2)
				{
					short currDurability = equipment.GetCurrDurability();
					sbyte requiredResourceType = ItemTemplateHelper.GetCraftRequiredResourceType(itemKey.ItemType, itemKey.TemplateId);
					int requiredResourceAmount = ItemTemplateHelper.GetRepairNeedResourceCount(equipment.GetMaterialResources(), itemKey, currDurability);
					bool checkResourceAmount = base.CheckResource(requiredResourceType, requiredResourceAmount);
					bool flag3 = !checkResourceAmount;
					if (!flag3)
					{
						short attainmentRequired = ItemTemplateHelper.GetRepairRequiredAttainment(itemKey.ItemType, itemKey.TemplateId, currDurability);
						short lifeSkillAttainment = this.Character.GetLifeSkillAttainment(requiredLifeSkillType);
						short durabilityCost;
						ItemKey toolKey = base.SettlementTreasury.Inventory.GetWorstUsableCraftTool(requiredLifeSkillType, attainmentRequired, lifeSkillAttainment, equipment.GetGrade(), out durabilityCost);
						bool flag4 = !toolKey.IsValid();
						if (!flag4)
						{
							GameData.Domains.Item.CraftTool craftTool = DomainManager.Item.GetElement_CraftTools(toolKey.Id);
							ItemBase.OfflineRepairItem(craftTool, equipment, equipment.GetMaxDurability(), durabilityCost);
							equipment.SetCurrDurability(equipment.GetCurrDurability(), context);
							short craftToolDurability = craftTool.GetCurrDurability();
							bool flag5 = craftToolDurability <= 0;
							if (flag5)
							{
								base.CostItem(context, toolKey, 1, true);
							}
							else
							{
								craftTool.SetCurrDurability(craftToolDurability, context);
							}
							base.CostResource(context, requiredResourceType, requiredResourceAmount);
							int charId = this.Character.GetId();
							int currDate = DomainManager.World.GetCurrDate();
							Location location = this.Character.GetLocation();
							LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
							lifeRecordCollection.AddVillagerRepairItem0(charId, currDate, location, itemKey.ItemType, itemKey.TemplateId);
							TaiwuVillageStoragesRecordCollection storageRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
							storageRecordCollection.AddVillagerRepairItem(currDate, TaiwuVillageStorageType.Treasury, charId, itemKey.ItemType, itemKey.TemplateId);
							repairCount++;
						}
					}
				}
			}
			return repairCount > 0;
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x001393C8 File Offset: 0x001375C8
		private unsafe void AutoGainRefineMaterial(DataContext context)
		{
			int gainRefineMaterialChance = VillagerRoleFormulaImpl.Calculate(42, base.Personality);
			bool flag = context.Random.Next(100) >= gainRefineMaterialChance;
			if (!flag)
			{
				int num = this.CraftLifeSkillTypes.Length;
				Span<int> span = new Span<int>(stackalloc byte[checked(unchecked((UIntPtr)num) * 4)], num);
				Span<int> weights = span;
				for (int i = 0; i < this.CraftLifeSkillTypes.Length; i++)
				{
					sbyte type = this.CraftLifeSkillTypes[i];
					*weights[i] = (int)this.Character.GetLifeSkillAttainment(type);
				}
				int index = CollectionUtils.GetRandomWeightedElement(context.Random, weights);
				sbyte lifeSkillType = this.CraftLifeSkillTypes[index];
				short attainment = this.Character.GetLifeSkillAttainment(lifeSkillType);
				int gainRefineMaterialGrade = VillagerRoleFormulaImpl.Calculate(43, (int)attainment);
				bool flag2 = context.Random.Next(100) < 50;
				if (flag2)
				{
					gainRefineMaterialGrade += -1;
				}
				sbyte maxGrade = (from m in Config.Material.Instance
				where m.RequiredLifeSkillType == lifeSkillType && m.RefiningEffect >= 0
				select m).Max((MaterialItem m) => m.Grade);
				int grade = Math.Clamp(gainRefineMaterialGrade, 0, (int)maxGrade);
				MaterialItem materialConfig = (from m in Config.Material.Instance
				where m.RequiredLifeSkillType == lifeSkillType && m.RefiningEffect >= 0
				select m).FirstOrDefault((MaterialItem m) => (int)m.Grade == grade);
				bool flag3 = materialConfig == null;
				if (!flag3)
				{
					int charId = this.Character.GetId();
					int currDate = DomainManager.World.GetCurrDate();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					TaiwuVillageStoragesRecordCollection storageRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
					bool flag4 = grade < (int)maxGrade && base.HasChickenUpgradeEffect && base.AutoActionStates[3];
					if (flag4)
					{
						int updateChance = VillagerRoleFormulaImpl.Calculate(44, base.Personality);
						bool flag5 = context.Random.Next(100) < updateChance;
						if (flag5)
						{
							int upgradeGrade = Math.Clamp(grade + 1, 0, (int)maxGrade);
							MaterialItem updateMaterialConfig = (from m in Config.Material.Instance
							where m.RequiredLifeSkillType == lifeSkillType && m.RefiningEffect >= 0
							select m).FirstOrDefault((MaterialItem m) => (int)m.Grade == upgradeGrade);
							bool flag6 = updateMaterialConfig != null && updateMaterialConfig.RequiredAttainment <= attainment;
							if (flag6)
							{
								ItemKey upgradeItemKey = DomainManager.Item.CreateMaterial(context, updateMaterialConfig.TemplateId);
								base.GainItem(context, upgradeItemKey, 1);
								lifeRecordCollection.AddVillagerUpgradeRefineItem(charId, currDate, materialConfig.ItemType, materialConfig.TemplateId, upgradeItemKey.ItemType, upgradeItemKey.TemplateId);
								storageRecordCollection.AddVillagerUpgradeRefineItem1(currDate, TaiwuVillageStorageType.Treasury, charId, materialConfig.ItemType, materialConfig.TemplateId, upgradeItemKey.ItemType, upgradeItemKey.TemplateId);
								return;
							}
						}
					}
					ItemKey itemKey = DomainManager.Item.CreateMaterial(context, materialConfig.TemplateId);
					base.GainItem(context, itemKey, 1);
					storageRecordCollection.AddVillagerGetRefineItem(currDate, TaiwuVillageStorageType.Treasury, charId, itemKey.ItemType, itemKey.TemplateId);
					lifeRecordCollection.AddVillagerGetRefineItem(charId, currDate, itemKey.ItemType, itemKey.TemplateId);
				}
			}
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00139700 File Offset: 0x00137900
		private short GetLackBuildingTemplateId(sbyte lifeSkillType)
		{
			bool condition = lifeSkillType - 6 <= 1 || lifeSkillType - 10 <= 1;
			Tester.Assert(condition, "");
			bool flag = lifeSkillType == 6;
			short result;
			if (flag)
			{
				result = 129;
			}
			else
			{
				bool flag2 = lifeSkillType == 7;
				if (flag2)
				{
					result = 139;
				}
				else
				{
					bool flag3 = lifeSkillType == 10;
					if (flag3)
					{
						result = 169;
					}
					else
					{
						result = 179;
					}
				}
			}
			return result;
		}

		// Token: 0x04000317 RID: 791
		public readonly sbyte[] CraftLifeSkillTypes = new sbyte[]
		{
			6,
			7,
			10,
			11
		};

		// Token: 0x04000318 RID: 792
		public const int GainRefineMaterialGradeOffsetChance = 50;

		// Token: 0x04000319 RID: 793
		public const int GainRefineMaterialGradeOffset = -1;
	}
}
