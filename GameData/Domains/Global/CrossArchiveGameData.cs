using System;
using System.Collections.Generic;
using GameData.DLC;
using GameData.DLC.FiveLoong;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation.RelationTree;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.SpecialEffect;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.World;
using GameData.Domains.World.SectMainStory;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Global
{
	// Token: 0x02000687 RID: 1671
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public class CrossArchiveGameData : ISerializableGameData
	{
		// Token: 0x060054D8 RID: 21720 RVA: 0x002E7BE4 File Offset: 0x002E5DE4
		public void PackWarehouseItem(ItemKey itemKey, int amount)
		{
			bool flag = !ItemTemplateHelper.IsInheritable(itemKey.ItemType, itemKey.TemplateId);
			if (!flag)
			{
				this.WarehouseItems.Add(itemKey, amount);
				DomainManager.Item.PackCrossArchiveItem(this, itemKey);
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060054D9 RID: 21721 RVA: 0x002E7C27 File Offset: 0x002E5E27
		// (set) Token: 0x060054DA RID: 21722 RVA: 0x002E7C39 File Offset: 0x002E5E39
		public TaiwuVillageStorage StockStorage
		{
			get
			{
				return this._stockStorage ?? this._stockStorageObsolete;
			}
			set
			{
				this._stockStorage = value;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060054DB RID: 21723 RVA: 0x002E7C42 File Offset: 0x002E5E42
		// (set) Token: 0x060054DC RID: 21724 RVA: 0x002E7C54 File Offset: 0x002E5E54
		public TaiwuVillageStorage CraftStorage
		{
			get
			{
				return this._craftStorage ?? this._craftStorageObsolete;
			}
			set
			{
				this._craftStorage = value;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060054DD RID: 21725 RVA: 0x002E7C5D File Offset: 0x002E5E5D
		// (set) Token: 0x060054DE RID: 21726 RVA: 0x002E7C6F File Offset: 0x002E5E6F
		public TaiwuVillageStorage MedicineStorage
		{
			get
			{
				return this._medicineStorage ?? this._medicineStorageObsolete;
			}
			set
			{
				this._medicineStorage = value;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060054DF RID: 21727 RVA: 0x002E7C78 File Offset: 0x002E5E78
		// (set) Token: 0x060054E0 RID: 21728 RVA: 0x002E7C8A File Offset: 0x002E5E8A
		public TaiwuVillageStorage FoodStorage
		{
			get
			{
				return this._foodStorage ?? this._foodStorageObsolete;
			}
			set
			{
				this._foodStorage = value;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060054E1 RID: 21729 RVA: 0x002E7C93 File Offset: 0x002E5E93
		// (set) Token: 0x060054E2 RID: 21730 RVA: 0x002E7CA5 File Offset: 0x002E5EA5
		public SettlementTreasury TaiwuSettlementTreasury
		{
			get
			{
				return this._taiwuSettlementTreasury ?? this._taiwuSettlementTreasuryObsolete;
			}
			set
			{
				this._taiwuSettlementTreasury = value;
			}
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x002E7CB0 File Offset: 0x002E5EB0
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x002E7CC4 File Offset: 0x002E5EC4
		public int GetSerializedSize()
		{
			int totalSize = 83;
			totalSize += CrossArchiveGameData.CrossArchiveTaiwuSerializer.GetSerializedSize(this.TaiwuChar);
			bool flag = this.CombatSkills != null;
			if (flag)
			{
				totalSize += 2 + 27 * this.CombatSkills.Count;
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this.TaiwuEffects != null;
			if (flag2)
			{
				totalSize += 2;
				int elementsCount = this.TaiwuEffects.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					SpecialEffectWrapper element = this.TaiwuEffects[i];
					bool flag3 = element != null;
					if (flag3)
					{
						totalSize += 2 + element.GetSerializedSize();
					}
					else
					{
						totalSize += 2;
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ItemKey>(this.UnpackedItems);
			bool flag4 = this.TaiwuVillageBlocks != null;
			if (flag4)
			{
				totalSize += 2 + 16 * this.TaiwuVillageBlocks.Count;
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, Chicken>(this.Chicken);
			totalSize += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(this.WarehouseItems);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuCombatSkill>(this.TaiwuCombatSkills);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuLifeSkill>(this.TaiwuLifeSkills);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuCombatSkill>(this.NotLearnedCombatSkills);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuLifeSkill>(this.NotLearnedLifeSkills);
			bool flag5 = this.CombatSkillPlans != null;
			if (flag5)
			{
				totalSize += 2;
				int elementsCount2 = this.CombatSkillPlans.Length;
				for (int j = 0; j < elementsCount2; j++)
				{
					CombatSkillPlan element2 = this.CombatSkillPlans[j];
					bool flag6 = element2 != null;
					if (flag6)
					{
						totalSize += 2 + element2.GetSerializedSize();
					}
					else
					{
						totalSize += 2;
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			bool flag7 = this.CurrLifeSkillAttainmentPanelPlanIndex != null;
			if (flag7)
			{
				totalSize += 2 + this.CurrLifeSkillAttainmentPanelPlanIndex.Length;
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakPlateObsolete>(this.SkillBreakPlateObsoleteDict);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakBonusCollection>(this.SkillBreakBonusDict);
			bool flag8 = this.CombatSkillAttainmentPanelPlans != null;
			if (flag8)
			{
				totalSize += 2 + 2 * this.CombatSkillAttainmentPanelPlans.Length;
			}
			else
			{
				totalSize += 2;
			}
			bool flag9 = this.CurrCombatSkillAttainmentPanelPlanIds != null;
			if (flag9)
			{
				totalSize += 2 + this.CurrCombatSkillAttainmentPanelPlanIds.Length;
			}
			else
			{
				totalSize += 2;
			}
			bool flag10 = this.EquipmentsPlans != null;
			if (flag10)
			{
				totalSize += 2 + 99 * this.EquipmentsPlans.Length;
			}
			else
			{
				totalSize += 2;
			}
			bool flag11 = this.WeaponInnerRatios != null;
			if (flag11)
			{
				totalSize += 2 + this.WeaponInnerRatios.Length;
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfCustomTypePair.GetSerializedSize<ItemKey, ReadingBookStrategies>(this.ReadingBooks);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ObsoleteProfessionData>(this.ObsoleteProfessions);
			totalSize += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(this.TreasuryItems);
			totalSize += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(this.TroughItems);
			bool flag12 = this.ReadingEventBookIdList != null;
			if (flag12)
			{
				totalSize += 2 + 4 * this.ReadingEventBookIdList.Count;
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ShortList>(this.ReadingEventReferenceBooks);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, IntPair>(this.ClearedSkillPlateStepInfo);
			totalSize += this.CurrMasteredCombatSkillPlan.GetSerializedSize();
			bool flag13 = this.MasteredCombatSkillPlans != null;
			if (flag13)
			{
				totalSize += 2;
				int elementsCount3 = this.MasteredCombatSkillPlans.Length;
				for (int k = 0; k < elementsCount3; k++)
				{
					totalSize += this.MasteredCombatSkillPlans[k].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<sbyte, sbyte>(this.LegendaryBookBreakPlateCounts);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakPlateObsoleteList>(this.CombatSkillBreakPlateObsoleteList);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, IntList>(this.CombatSkillBreakPlateLastClearTimeList);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, IntList>(this.CombatSkillBreakPlateLastForceBreakoutStepsCount);
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<short, sbyte>(this.CombatSkillCurrBreakPlateIndex);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<sbyte, ItemKey>(this.LegendaryBookWeaponSlot);
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<sbyte, long>(this.LegendaryBookWeaponEffectId);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<sbyte, ShortList>(this.LegendaryBookSkillSlot);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<sbyte, LongList>(this.LegendaryBookSkillEffectId);
			totalSize += this.LegendaryBookBonusCountYin.GetSerializedSize();
			totalSize += this.LegendaryBookBonusCountYang.GetSerializedSize();
			bool flag14 = this.HandledOneShotEvents != null;
			if (flag14)
			{
				totalSize += 2 + 4 * this.HandledOneShotEvents.Count;
			}
			else
			{
				totalSize += 2;
			}
			bool flag15 = this.LegaciesBuildingTemplateIds != null;
			if (flag15)
			{
				totalSize += 2 + 2 * this.LegaciesBuildingTemplateIds.Count;
			}
			else
			{
				totalSize += 2;
			}
			bool flag16 = this.CollectionCrickets != null;
			if (flag16)
			{
				totalSize += 2 + 8 * this.CollectionCrickets.Length;
			}
			else
			{
				totalSize += 2;
			}
			bool flag17 = this.CollectionCricketRegen != null;
			if (flag17)
			{
				totalSize += 2 + 4 * this.CollectionCricketRegen.Length;
			}
			else
			{
				totalSize += 2;
			}
			bool flag18 = this.CollectionCricketJars != null;
			if (flag18)
			{
				totalSize += 2 + 8 * this.CollectionCricketJars.Length;
			}
			else
			{
				totalSize += 2;
			}
			bool flag19 = this.NormalInformation != null;
			if (flag19)
			{
				totalSize += 2 + this.NormalInformation.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			bool flag20 = this.JiaoPools != null;
			if (flag20)
			{
				totalSize += 2;
				int elementsCount4 = this.JiaoPools.Count;
				for (int l = 0; l < elementsCount4; l++)
				{
					JiaoPool element3 = this.JiaoPools[l];
					bool flag21 = element3 != null;
					if (flag21)
					{
						totalSize += 2 + element3.GetSerializedSize();
					}
					else
					{
						totalSize += 2;
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakBonusCollection>(this.SectEmeiSkillBreakBonus);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, ShortList>(this.SectEmeiBreakBonusTemplateIds);
			bool flag22 = this.CricketCollectionDatas != null;
			if (flag22)
			{
				totalSize += 2;
				int elementsCount5 = this.CricketCollectionDatas.Count;
				for (int m = 0; m < elementsCount5; m++)
				{
					CricketCollectionData element4 = this.CricketCollectionDatas[m];
					bool flag23 = element4 != null;
					if (flag23)
					{
						totalSize += 2 + element4.GetSerializedSize();
					}
					else
					{
						totalSize += 2;
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, SByteList>(this.AvailableReadingStrategyMap);
			totalSize += this.ExtraNeiliAllocationProgress.GetSerializedSize();
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SectEmeiBreakBonusData>(this.SectEmeiBonusData);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, IntList>(this.SectFulongOrgMemberChickens);
			bool flag24 = this._craftStorageObsolete != null;
			if (flag24)
			{
				totalSize += 2 + this._craftStorageObsolete.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			bool flag25 = this._medicineStorageObsolete != null;
			if (flag25)
			{
				totalSize += 2 + this._medicineStorageObsolete.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			bool flag26 = this._foodStorageObsolete != null;
			if (flag26)
			{
				totalSize += 2 + this._foodStorageObsolete.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			bool flag27 = this._stockStorageObsolete != null;
			if (flag27)
			{
				totalSize += 2 + this._stockStorageObsolete.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			bool flag28 = this._taiwuSettlementTreasuryObsolete != null;
			if (flag28)
			{
				totalSize += 2 + this._taiwuSettlementTreasuryObsolete.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ProfessionData>(this.Professions);
			bool flag29 = this.ExternalEquippedCombatSkills != null;
			if (flag29)
			{
				totalSize += 2 + this.ExternalEquippedCombatSkills.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			bool flag30 = this.SectZhujianGearMate != null;
			if (flag30)
			{
				totalSize += 2 + this.SectZhujianGearMate.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakPlateList>(this.CombatSkillBreakPlateList);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakPlate>(this.SkillBreakPlateDict);
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<short, int>(this.TaiwuCombatSkillProficiencies);
			bool flag31 = this.XiangshuIdInKungfuPracticeRoom != null;
			if (flag31)
			{
				totalSize += 2 + this.XiangshuIdInKungfuPracticeRoom.Count;
			}
			else
			{
				totalSize += 2;
			}
			bool flag32 = this._craftStorage != null;
			if (flag32)
			{
				totalSize += 4 + this._craftStorage.GetSerializedSize();
			}
			else
			{
				totalSize += 4;
			}
			bool flag33 = this._medicineStorage != null;
			if (flag33)
			{
				totalSize += 4 + this._medicineStorage.GetSerializedSize();
			}
			else
			{
				totalSize += 4;
			}
			bool flag34 = this._foodStorage != null;
			if (flag34)
			{
				totalSize += 4 + this._foodStorage.GetSerializedSize();
			}
			else
			{
				totalSize += 4;
			}
			bool flag35 = this._stockStorage != null;
			if (flag35)
			{
				totalSize += 4 + this._stockStorage.GetSerializedSize();
			}
			else
			{
				totalSize += 4;
			}
			bool flag36 = this._taiwuSettlementTreasury != null;
			if (flag36)
			{
				totalSize += 4 + this._taiwuSettlementTreasury.GetSerializedSize();
			}
			else
			{
				totalSize += 4;
			}
			bool flag37 = this.TaiwuVillageBlocksEx != null;
			if (flag37)
			{
				totalSize += 2;
				int elementsCount6 = this.TaiwuVillageBlocksEx.Count;
				for (int n = 0; n < elementsCount6; n++)
				{
					BuildingBlockDataEx element5 = this.TaiwuVillageBlocksEx[n];
					bool flag38 = element5 != null;
					if (flag38)
					{
						totalSize += 2 + element5.GetSerializedSize();
					}
					else
					{
						totalSize += 2;
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060054E5 RID: 21733 RVA: 0x002E854C File Offset: 0x002E674C
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 85;
			byte* pCurrData = pData + 2;
			pCurrData += CrossArchiveGameData.CrossArchiveTaiwuSerializer.Serialize(pCurrData, this.TaiwuChar);
			bool flag = this.CombatSkills != null;
			if (flag)
			{
				int elementsCount = this.CombatSkills.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pCurrData += this.CombatSkills[i].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag2 = this.TaiwuEffects != null;
			if (flag2)
			{
				int elementsCount2 = this.TaiwuEffects.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					SpecialEffectWrapper element = this.TaiwuEffects[j];
					bool flag3 = element != null;
					if (flag3)
					{
						byte* pSubDataCount = pCurrData;
						pCurrData += 2;
						int subDataSize = element.Serialize(pCurrData);
						pCurrData += subDataSize;
						Tester.Assert(subDataSize <= 65535, "");
						*(short*)pSubDataCount = (short)((ushort)subDataSize);
					}
					else
					{
						*(short*)pCurrData = 0;
						pCurrData += 2;
					}
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<int, ItemKey>(pCurrData, ref this.UnpackedItems);
			pCurrData += this.TaiwuVillageLocation.Serialize(pCurrData);
			pCurrData += this.TaiwuVillageAreaData.Serialize(pCurrData);
			bool flag4 = this.TaiwuVillageBlocks != null;
			if (flag4)
			{
				int elementsCount3 = this.TaiwuVillageBlocks.Count;
				Tester.Assert(elementsCount3 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount3);
				pCurrData += 2;
				for (int k = 0; k < elementsCount3; k++)
				{
					pCurrData += this.TaiwuVillageBlocks[k].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<int, Chicken>(pCurrData, ref this.Chicken);
			pCurrData += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(pCurrData, ref this.WarehouseItems);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuCombatSkill>(pCurrData, ref this.TaiwuCombatSkills);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuLifeSkill>(pCurrData, ref this.TaiwuLifeSkills);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuCombatSkill>(pCurrData, ref this.NotLearnedCombatSkills);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuLifeSkill>(pCurrData, ref this.NotLearnedLifeSkills);
			bool flag5 = this.CombatSkillPlans != null;
			if (flag5)
			{
				int elementsCount4 = this.CombatSkillPlans.Length;
				Tester.Assert(elementsCount4 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount4);
				pCurrData += 2;
				for (int l = 0; l < elementsCount4; l++)
				{
					CombatSkillPlan element2 = this.CombatSkillPlans[l];
					bool flag6 = element2 != null;
					if (flag6)
					{
						byte* pSubDataCount2 = pCurrData;
						pCurrData += 2;
						int subDataSize2 = element2.Serialize(pCurrData);
						pCurrData += subDataSize2;
						Tester.Assert(subDataSize2 <= 65535, "");
						*(short*)pSubDataCount2 = (short)((ushort)subDataSize2);
					}
					else
					{
						*(short*)pCurrData = 0;
						pCurrData += 2;
					}
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			*(int*)pCurrData = this.CurrCombatSkillPlanId;
			pCurrData += 4;
			bool flag7 = this.CurrLifeSkillAttainmentPanelPlanIndex != null;
			if (flag7)
			{
				int elementsCount5 = this.CurrLifeSkillAttainmentPanelPlanIndex.Length;
				Tester.Assert(elementsCount5 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount5);
				pCurrData += 2;
				for (int m = 0; m < elementsCount5; m++)
				{
					pCurrData[m] = (byte)this.CurrLifeSkillAttainmentPanelPlanIndex[m];
				}
				pCurrData += elementsCount5;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakPlateObsolete>(pCurrData, ref this.SkillBreakPlateObsoleteDict);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakBonusCollection>(pCurrData, ref this.SkillBreakBonusDict);
			bool flag8 = this.CombatSkillAttainmentPanelPlans != null;
			if (flag8)
			{
				int elementsCount6 = this.CombatSkillAttainmentPanelPlans.Length;
				Tester.Assert(elementsCount6 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount6);
				pCurrData += 2;
				for (int n = 0; n < elementsCount6; n++)
				{
					*(short*)(pCurrData + (IntPtr)n * 2) = this.CombatSkillAttainmentPanelPlans[n];
				}
				pCurrData += 2 * elementsCount6;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag9 = this.CurrCombatSkillAttainmentPanelPlanIds != null;
			if (flag9)
			{
				int elementsCount7 = this.CurrCombatSkillAttainmentPanelPlanIds.Length;
				Tester.Assert(elementsCount7 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount7);
				pCurrData += 2;
				for (int i2 = 0; i2 < elementsCount7; i2++)
				{
					pCurrData[i2] = (byte)this.CurrCombatSkillAttainmentPanelPlanIds[i2];
				}
				pCurrData += elementsCount7;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag10 = this.EquipmentsPlans != null;
			if (flag10)
			{
				int elementsCount8 = this.EquipmentsPlans.Length;
				Tester.Assert(elementsCount8 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount8);
				pCurrData += 2;
				for (int i3 = 0; i3 < elementsCount8; i3++)
				{
					pCurrData += this.EquipmentsPlans[i3].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			*(int*)pCurrData = this.CurrEquipmentPlanId;
			pCurrData += 4;
			bool flag11 = this.WeaponInnerRatios != null;
			if (flag11)
			{
				int elementsCount9 = this.WeaponInnerRatios.Length;
				Tester.Assert(elementsCount9 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount9);
				pCurrData += 2;
				for (int i4 = 0; i4 < elementsCount9; i4++)
				{
					pCurrData[i4] = (byte)this.WeaponInnerRatios[i4];
				}
				pCurrData += elementsCount9;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			*pCurrData = (byte)this.VoiceWeaponInnerRatio;
			pCurrData++;
			pCurrData += SerializationHelper.DictionaryOfCustomTypePair.Serialize<ItemKey, ReadingBookStrategies>(pCurrData, ref this.ReadingBooks);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<int, ObsoleteProfessionData>(pCurrData, ref this.ObsoleteProfessions);
			*(int*)pCurrData = this.CurrProfessionId;
			pCurrData += 4;
			pCurrData += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(pCurrData, ref this.TreasuryItems);
			pCurrData += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(pCurrData, ref this.TroughItems);
			bool flag12 = this.ReadingEventBookIdList != null;
			if (flag12)
			{
				int elementsCount10 = this.ReadingEventBookIdList.Count;
				Tester.Assert(elementsCount10 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount10);
				pCurrData += 2;
				for (int i5 = 0; i5 < elementsCount10; i5++)
				{
					*(int*)(pCurrData + (IntPtr)i5 * 4) = this.ReadingEventBookIdList[i5];
				}
				pCurrData += 4 * elementsCount10;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<int, ShortList>(pCurrData, ref this.ReadingEventReferenceBooks);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, IntPair>(pCurrData, ref this.ClearedSkillPlateStepInfo);
			pCurrData += this.TaiwuMaxNeiliAllocation.Serialize(pCurrData);
			int fieldSize = this.CurrMasteredCombatSkillPlan.Serialize(pCurrData);
			pCurrData += fieldSize;
			Tester.Assert(fieldSize <= 65535, "");
			bool flag13 = this.MasteredCombatSkillPlans != null;
			if (flag13)
			{
				int elementsCount11 = this.MasteredCombatSkillPlans.Length;
				Tester.Assert(elementsCount11 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount11);
				pCurrData += 2;
				for (int i6 = 0; i6 < elementsCount11; i6++)
				{
					int subDataSize3 = this.MasteredCombatSkillPlans[i6].Serialize(pCurrData);
					pCurrData += subDataSize3;
					Tester.Assert(subDataSize3 <= 65535, "");
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			*(int*)pCurrData = this.TaiwuExp;
			pCurrData += 4;
			pCurrData += this.TaiwuResources.Serialize(pCurrData);
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<sbyte, sbyte>(pCurrData, ref this.LegendaryBookBreakPlateCounts);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakPlateObsoleteList>(pCurrData, ref this.CombatSkillBreakPlateObsoleteList);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, IntList>(pCurrData, ref this.CombatSkillBreakPlateLastClearTimeList);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, IntList>(pCurrData, ref this.CombatSkillBreakPlateLastForceBreakoutStepsCount);
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<short, sbyte>(pCurrData, ref this.CombatSkillCurrBreakPlateIndex);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<sbyte, ItemKey>(pCurrData, ref this.LegendaryBookWeaponSlot);
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<sbyte, long>(pCurrData, ref this.LegendaryBookWeaponEffectId);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<sbyte, ShortList>(pCurrData, ref this.LegendaryBookSkillSlot);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<sbyte, LongList>(pCurrData, ref this.LegendaryBookSkillEffectId);
			int fieldSize2 = this.LegendaryBookBonusCountYin.Serialize(pCurrData);
			pCurrData += fieldSize2;
			Tester.Assert(fieldSize2 <= 65535, "");
			int fieldSize3 = this.LegendaryBookBonusCountYang.Serialize(pCurrData);
			pCurrData += fieldSize3;
			Tester.Assert(fieldSize3 <= 65535, "");
			bool flag14 = this.HandledOneShotEvents != null;
			if (flag14)
			{
				int elementsCount12 = this.HandledOneShotEvents.Count;
				Tester.Assert(elementsCount12 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount12);
				pCurrData += 2;
				for (int i7 = 0; i7 < elementsCount12; i7++)
				{
					*(int*)(pCurrData + (IntPtr)i7 * 4) = this.HandledOneShotEvents[i7];
				}
				pCurrData += 4 * elementsCount12;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag15 = this.LegaciesBuildingTemplateIds != null;
			if (flag15)
			{
				int elementsCount13 = this.LegaciesBuildingTemplateIds.Count;
				Tester.Assert(elementsCount13 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount13);
				pCurrData += 2;
				for (int i8 = 0; i8 < elementsCount13; i8++)
				{
					*(short*)(pCurrData + (IntPtr)i8 * 2) = this.LegaciesBuildingTemplateIds[i8];
				}
				pCurrData += 2 * elementsCount13;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag16 = this.CollectionCrickets != null;
			if (flag16)
			{
				int elementsCount14 = this.CollectionCrickets.Length;
				Tester.Assert(elementsCount14 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount14);
				pCurrData += 2;
				for (int i9 = 0; i9 < elementsCount14; i9++)
				{
					pCurrData += this.CollectionCrickets[i9].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag17 = this.CollectionCricketRegen != null;
			if (flag17)
			{
				int elementsCount15 = this.CollectionCricketRegen.Length;
				Tester.Assert(elementsCount15 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount15);
				pCurrData += 2;
				for (int i10 = 0; i10 < elementsCount15; i10++)
				{
					*(int*)(pCurrData + (IntPtr)i10 * 4) = this.CollectionCricketRegen[i10];
				}
				pCurrData += 4 * elementsCount15;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag18 = this.CollectionCricketJars != null;
			if (flag18)
			{
				int elementsCount16 = this.CollectionCricketJars.Length;
				Tester.Assert(elementsCount16 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount16);
				pCurrData += 2;
				for (int i11 = 0; i11 < elementsCount16; i11++)
				{
					pCurrData += this.CollectionCricketJars[i11].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			*(int*)pCurrData = this.BuildingSpaceExtraAdd;
			pCurrData += 4;
			bool flag19 = this.NormalInformation != null;
			if (flag19)
			{
				byte* pSubDataCount3 = pCurrData;
				pCurrData += 2;
				int fieldSize4 = this.NormalInformation.Serialize(pCurrData);
				pCurrData += fieldSize4;
				Tester.Assert(fieldSize4 <= 65535, "");
				*(short*)pSubDataCount3 = (short)((ushort)fieldSize4);
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag20 = this.JiaoPools != null;
			if (flag20)
			{
				int elementsCount17 = this.JiaoPools.Count;
				Tester.Assert(elementsCount17 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount17);
				pCurrData += 2;
				for (int i12 = 0; i12 < elementsCount17; i12++)
				{
					JiaoPool element3 = this.JiaoPools[i12];
					bool flag21 = element3 != null;
					if (flag21)
					{
						byte* pSubDataCount4 = pCurrData;
						pCurrData += 2;
						int subDataSize4 = element3.Serialize(pCurrData);
						pCurrData += subDataSize4;
						Tester.Assert(subDataSize4 <= 65535, "");
						*(short*)pSubDataCount4 = (short)((ushort)subDataSize4);
					}
					else
					{
						*(short*)pCurrData = 0;
						pCurrData += 2;
					}
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakBonusCollection>(pCurrData, ref this.SectEmeiSkillBreakBonus);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, ShortList>(pCurrData, ref this.SectEmeiBreakBonusTemplateIds);
			*(int*)pCurrData = this.MaxTaiwuVillageLevel;
			pCurrData += 4;
			*pCurrData = (this.IsJiaoPoolOpen ? 1 : 0);
			pCurrData++;
			bool flag22 = this.CricketCollectionDatas != null;
			if (flag22)
			{
				int elementsCount18 = this.CricketCollectionDatas.Count;
				Tester.Assert(elementsCount18 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount18);
				pCurrData += 2;
				for (int i13 = 0; i13 < elementsCount18; i13++)
				{
					CricketCollectionData element4 = this.CricketCollectionDatas[i13];
					bool flag23 = element4 != null;
					if (flag23)
					{
						byte* pSubDataCount5 = pCurrData;
						pCurrData += 2;
						int subDataSize5 = element4.Serialize(pCurrData);
						pCurrData += subDataSize5;
						Tester.Assert(subDataSize5 <= 65535, "");
						*(short*)pSubDataCount5 = (short)((ushort)subDataSize5);
					}
					else
					{
						*(short*)pCurrData = 0;
						pCurrData += 2;
					}
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			*pCurrData = this.UnlockedCombatSkillPlanCount;
			pCurrData++;
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<int, SByteList>(pCurrData, ref this.AvailableReadingStrategyMap);
			int fieldSize5 = this.ExtraNeiliAllocationProgress.Serialize(pCurrData);
			pCurrData += fieldSize5;
			Tester.Assert(fieldSize5 <= 65535, "");
			pCurrData += this.ExtraNeiliAllocation.Serialize(pCurrData);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, SectEmeiBreakBonusData>(pCurrData, ref this.SectEmeiBonusData);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, IntList>(pCurrData, ref this.SectFulongOrgMemberChickens);
			bool flag24 = this._craftStorageObsolete != null;
			if (flag24)
			{
				byte* pSubDataCount6 = pCurrData;
				pCurrData += 2;
				int fieldSize6 = this._craftStorageObsolete.Serialize(pCurrData);
				pCurrData += fieldSize6;
				Tester.Assert(fieldSize6 <= 65535, "");
				*(short*)pSubDataCount6 = (short)((ushort)fieldSize6);
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag25 = this._medicineStorageObsolete != null;
			if (flag25)
			{
				byte* pSubDataCount7 = pCurrData;
				pCurrData += 2;
				int fieldSize7 = this._medicineStorageObsolete.Serialize(pCurrData);
				pCurrData += fieldSize7;
				Tester.Assert(fieldSize7 <= 65535, "");
				*(short*)pSubDataCount7 = (short)((ushort)fieldSize7);
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag26 = this._foodStorageObsolete != null;
			if (flag26)
			{
				byte* pSubDataCount8 = pCurrData;
				pCurrData += 2;
				int fieldSize8 = this._foodStorageObsolete.Serialize(pCurrData);
				pCurrData += fieldSize8;
				Tester.Assert(fieldSize8 <= 65535, "");
				*(short*)pSubDataCount8 = (short)((ushort)fieldSize8);
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag27 = this._stockStorageObsolete != null;
			if (flag27)
			{
				byte* pSubDataCount9 = pCurrData;
				pCurrData += 2;
				int fieldSize9 = this._stockStorageObsolete.Serialize(pCurrData);
				pCurrData += fieldSize9;
				Tester.Assert(fieldSize9 <= 65535, "");
				*(short*)pSubDataCount9 = (short)((ushort)fieldSize9);
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag28 = this._taiwuSettlementTreasuryObsolete != null;
			if (flag28)
			{
				byte* pSubDataCount10 = pCurrData;
				pCurrData += 2;
				int fieldSize10 = this._taiwuSettlementTreasuryObsolete.Serialize(pCurrData);
				pCurrData += fieldSize10;
				Tester.Assert(fieldSize10 <= 65535, "");
				*(short*)pSubDataCount10 = (short)((ushort)fieldSize10);
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<int, ProfessionData>(pCurrData, ref this.Professions);
			bool flag29 = this.ExternalEquippedCombatSkills != null;
			if (flag29)
			{
				byte* pSubDataCount11 = pCurrData;
				pCurrData += 2;
				int fieldSize11 = this.ExternalEquippedCombatSkills.Serialize(pCurrData);
				pCurrData += fieldSize11;
				Tester.Assert(fieldSize11 <= 65535, "");
				*(short*)pSubDataCount11 = (short)((ushort)fieldSize11);
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag30 = this.SectZhujianGearMate != null;
			if (flag30)
			{
				byte* pSubDataCount12 = pCurrData;
				pCurrData += 2;
				int fieldSize12 = this.SectZhujianGearMate.Serialize(pCurrData);
				pCurrData += fieldSize12;
				Tester.Assert(fieldSize12 <= 65535, "");
				*(short*)pSubDataCount12 = (short)((ushort)fieldSize12);
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakPlateList>(pCurrData, ref this.CombatSkillBreakPlateList);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakPlate>(pCurrData, ref this.SkillBreakPlateDict);
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<short, int>(pCurrData, ref this.TaiwuCombatSkillProficiencies);
			bool flag31 = this.XiangshuIdInKungfuPracticeRoom != null;
			if (flag31)
			{
				int elementsCount19 = this.XiangshuIdInKungfuPracticeRoom.Count;
				Tester.Assert(elementsCount19 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount19);
				pCurrData += 2;
				for (int i14 = 0; i14 < elementsCount19; i14++)
				{
					pCurrData[i14] = (byte)this.XiangshuIdInKungfuPracticeRoom[i14];
				}
				pCurrData += elementsCount19;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag32 = this._craftStorage != null;
			if (flag32)
			{
				byte* pSubDataCount13 = pCurrData;
				pCurrData += 4;
				int fieldSize13 = this._craftStorage.Serialize(pCurrData);
				pCurrData += fieldSize13;
				Tester.Assert(fieldSize13 <= int.MaxValue, "");
				*(int*)pSubDataCount13 = fieldSize13;
			}
			else
			{
				*(int*)pCurrData = 0;
				pCurrData += 4;
			}
			bool flag33 = this._medicineStorage != null;
			if (flag33)
			{
				byte* pSubDataCount14 = pCurrData;
				pCurrData += 4;
				int fieldSize14 = this._medicineStorage.Serialize(pCurrData);
				pCurrData += fieldSize14;
				Tester.Assert(fieldSize14 <= int.MaxValue, "");
				*(int*)pSubDataCount14 = fieldSize14;
			}
			else
			{
				*(int*)pCurrData = 0;
				pCurrData += 4;
			}
			bool flag34 = this._foodStorage != null;
			if (flag34)
			{
				byte* pSubDataCount15 = pCurrData;
				pCurrData += 4;
				int fieldSize15 = this._foodStorage.Serialize(pCurrData);
				pCurrData += fieldSize15;
				Tester.Assert(fieldSize15 <= int.MaxValue, "");
				*(int*)pSubDataCount15 = fieldSize15;
			}
			else
			{
				*(int*)pCurrData = 0;
				pCurrData += 4;
			}
			bool flag35 = this._stockStorage != null;
			if (flag35)
			{
				byte* pSubDataCount16 = pCurrData;
				pCurrData += 4;
				int fieldSize16 = this._stockStorage.Serialize(pCurrData);
				pCurrData += fieldSize16;
				Tester.Assert(fieldSize16 <= int.MaxValue, "");
				*(int*)pSubDataCount16 = fieldSize16;
			}
			else
			{
				*(int*)pCurrData = 0;
				pCurrData += 4;
			}
			bool flag36 = this._taiwuSettlementTreasury != null;
			if (flag36)
			{
				byte* pSubDataCount17 = pCurrData;
				pCurrData += 4;
				int fieldSize17 = this._taiwuSettlementTreasury.Serialize(pCurrData);
				pCurrData += fieldSize17;
				Tester.Assert(fieldSize17 <= int.MaxValue, "");
				*(int*)pSubDataCount17 = fieldSize17;
			}
			else
			{
				*(int*)pCurrData = 0;
				pCurrData += 4;
			}
			bool flag37 = this.TaiwuVillageBlocksEx != null;
			if (flag37)
			{
				int elementsCount20 = this.TaiwuVillageBlocksEx.Count;
				Tester.Assert(elementsCount20 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount20);
				pCurrData += 2;
				for (int i15 = 0; i15 < elementsCount20; i15++)
				{
					BuildingBlockDataEx element5 = this.TaiwuVillageBlocksEx[i15];
					bool flag38 = element5 != null;
					if (flag38)
					{
						byte* pSubDataCount18 = pCurrData;
						pCurrData += 2;
						int subDataSize6 = element5.Serialize(pCurrData);
						pCurrData += subDataSize6;
						Tester.Assert(subDataSize6 <= 65535, "");
						*(short*)pSubDataCount18 = (short)((ushort)subDataSize6);
					}
					else
					{
						*(short*)pCurrData = 0;
						pCurrData += 2;
					}
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060054E6 RID: 21734 RVA: 0x002E97C4 File Offset: 0x002E79C4
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				pCurrData += CrossArchiveGameData.CrossArchiveTaiwuSerializer.Deserialize(pCurrData, ref this.TaiwuChar);
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				ushort elementsCount = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag3 = elementsCount > 0;
				if (flag3)
				{
					bool flag4 = this.CombatSkills == null;
					if (flag4)
					{
						this.CombatSkills = new List<CombatSkill>((int)elementsCount);
					}
					else
					{
						this.CombatSkills.Clear();
					}
					for (int i = 0; i < (int)elementsCount; i++)
					{
						CombatSkill element = new CombatSkill();
						pCurrData += element.Deserialize(pCurrData);
						this.CombatSkills.Add(element);
					}
				}
				else
				{
					List<CombatSkill> combatSkills = this.CombatSkills;
					if (combatSkills != null)
					{
						combatSkills.Clear();
					}
				}
			}
			bool flag5 = fieldCount > 2;
			if (flag5)
			{
				ushort elementsCount2 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag6 = elementsCount2 > 0;
				if (flag6)
				{
					bool flag7 = this.TaiwuEffects == null;
					if (flag7)
					{
						this.TaiwuEffects = new List<SpecialEffectWrapper>((int)elementsCount2);
					}
					else
					{
						this.TaiwuEffects.Clear();
					}
					for (int j = 0; j < (int)elementsCount2; j++)
					{
						ushort subDataCount = *(ushort*)pCurrData;
						pCurrData += 2;
						bool flag8 = subDataCount > 0;
						if (flag8)
						{
							SpecialEffectWrapper element2 = new SpecialEffectWrapper();
							pCurrData += element2.Deserialize(pCurrData);
							this.TaiwuEffects.Add(element2);
						}
						else
						{
							this.TaiwuEffects.Add(null);
						}
					}
				}
				else
				{
					List<SpecialEffectWrapper> taiwuEffects = this.TaiwuEffects;
					if (taiwuEffects != null)
					{
						taiwuEffects.Clear();
					}
				}
			}
			bool flag9 = fieldCount > 3;
			if (flag9)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ItemKey>(pCurrData, ref this.UnpackedItems);
			}
			bool flag10 = fieldCount > 4;
			if (flag10)
			{
				pCurrData += this.TaiwuVillageLocation.Deserialize(pCurrData);
			}
			bool flag11 = fieldCount > 5;
			if (flag11)
			{
				bool flag12 = this.TaiwuVillageAreaData == null;
				if (flag12)
				{
					this.TaiwuVillageAreaData = new BuildingAreaData();
				}
				pCurrData += this.TaiwuVillageAreaData.Deserialize(pCurrData);
			}
			bool flag13 = fieldCount > 6;
			if (flag13)
			{
				ushort elementsCount3 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag14 = elementsCount3 > 0;
				if (flag14)
				{
					bool flag15 = this.TaiwuVillageBlocks == null;
					if (flag15)
					{
						this.TaiwuVillageBlocks = new List<BuildingBlockData>((int)elementsCount3);
					}
					else
					{
						this.TaiwuVillageBlocks.Clear();
					}
					for (int k = 0; k < (int)elementsCount3; k++)
					{
						BuildingBlockData element3 = new BuildingBlockData();
						pCurrData += element3.Deserialize(pCurrData);
						this.TaiwuVillageBlocks.Add(element3);
					}
				}
				else
				{
					List<BuildingBlockData> taiwuVillageBlocks = this.TaiwuVillageBlocks;
					if (taiwuVillageBlocks != null)
					{
						taiwuVillageBlocks.Clear();
					}
				}
			}
			bool flag16 = fieldCount > 7;
			if (flag16)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<int, Chicken>(pCurrData, ref this.Chicken);
			}
			bool flag17 = fieldCount > 8;
			if (flag17)
			{
				pCurrData += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(pCurrData, ref this.WarehouseItems);
			}
			bool flag18 = fieldCount > 9;
			if (flag18)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuCombatSkill>(pCurrData, ref this.TaiwuCombatSkills);
			}
			bool flag19 = fieldCount > 10;
			if (flag19)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuLifeSkill>(pCurrData, ref this.TaiwuLifeSkills);
			}
			bool flag20 = fieldCount > 11;
			if (flag20)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuCombatSkill>(pCurrData, ref this.NotLearnedCombatSkills);
			}
			bool flag21 = fieldCount > 12;
			if (flag21)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuLifeSkill>(pCurrData, ref this.NotLearnedLifeSkills);
			}
			bool flag22 = fieldCount > 13;
			if (flag22)
			{
				ushort elementsCount4 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag23 = elementsCount4 > 0;
				if (flag23)
				{
					bool flag24 = this.CombatSkillPlans == null || this.CombatSkillPlans.Length != (int)elementsCount4;
					if (flag24)
					{
						this.CombatSkillPlans = new CombatSkillPlan[(int)elementsCount4];
					}
					for (int l = 0; l < (int)elementsCount4; l++)
					{
						ushort subDataCount2 = *(ushort*)pCurrData;
						pCurrData += 2;
						bool flag25 = subDataCount2 > 0;
						if (flag25)
						{
							CombatSkillPlan element4 = this.CombatSkillPlans[l] ?? new CombatSkillPlan();
							pCurrData += element4.Deserialize(pCurrData);
							this.CombatSkillPlans[l] = element4;
						}
						else
						{
							this.CombatSkillPlans[l] = null;
						}
					}
				}
				else
				{
					this.CombatSkillPlans = null;
				}
			}
			bool flag26 = fieldCount > 14;
			if (flag26)
			{
				this.CurrCombatSkillPlanId = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag27 = fieldCount > 15;
			if (flag27)
			{
				ushort elementsCount5 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag28 = elementsCount5 > 0;
				if (flag28)
				{
					bool flag29 = this.CurrLifeSkillAttainmentPanelPlanIndex == null || this.CurrLifeSkillAttainmentPanelPlanIndex.Length != (int)elementsCount5;
					if (flag29)
					{
						this.CurrLifeSkillAttainmentPanelPlanIndex = new sbyte[(int)elementsCount5];
					}
					for (int m = 0; m < (int)elementsCount5; m++)
					{
						this.CurrLifeSkillAttainmentPanelPlanIndex[m] = *(sbyte*)(pCurrData + m);
					}
					pCurrData += elementsCount5;
				}
				else
				{
					this.CurrLifeSkillAttainmentPanelPlanIndex = null;
				}
			}
			bool flag30 = fieldCount > 16;
			if (flag30)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakPlateObsolete>(pCurrData, ref this.SkillBreakPlateObsoleteDict);
			}
			bool flag31 = fieldCount > 17;
			if (flag31)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakBonusCollection>(pCurrData, ref this.SkillBreakBonusDict);
			}
			bool flag32 = fieldCount > 18;
			if (flag32)
			{
				ushort elementsCount6 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag33 = elementsCount6 > 0;
				if (flag33)
				{
					bool flag34 = this.CombatSkillAttainmentPanelPlans == null || this.CombatSkillAttainmentPanelPlans.Length != (int)elementsCount6;
					if (flag34)
					{
						this.CombatSkillAttainmentPanelPlans = new short[(int)elementsCount6];
					}
					for (int n = 0; n < (int)elementsCount6; n++)
					{
						this.CombatSkillAttainmentPanelPlans[n] = *(short*)(pCurrData + (IntPtr)n * 2);
					}
					pCurrData += 2 * elementsCount6;
				}
				else
				{
					this.CombatSkillAttainmentPanelPlans = null;
				}
			}
			bool flag35 = fieldCount > 19;
			if (flag35)
			{
				ushort elementsCount7 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag36 = elementsCount7 > 0;
				if (flag36)
				{
					bool flag37 = this.CurrCombatSkillAttainmentPanelPlanIds == null || this.CurrCombatSkillAttainmentPanelPlanIds.Length != (int)elementsCount7;
					if (flag37)
					{
						this.CurrCombatSkillAttainmentPanelPlanIds = new sbyte[(int)elementsCount7];
					}
					for (int i2 = 0; i2 < (int)elementsCount7; i2++)
					{
						this.CurrCombatSkillAttainmentPanelPlanIds[i2] = *(sbyte*)(pCurrData + i2);
					}
					pCurrData += elementsCount7;
				}
				else
				{
					this.CurrCombatSkillAttainmentPanelPlanIds = null;
				}
			}
			bool flag38 = fieldCount > 20;
			if (flag38)
			{
				ushort elementsCount8 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag39 = elementsCount8 > 0;
				if (flag39)
				{
					bool flag40 = this.EquipmentsPlans == null || this.EquipmentsPlans.Length != (int)elementsCount8;
					if (flag40)
					{
						this.EquipmentsPlans = new EquipmentPlan[(int)elementsCount8];
					}
					for (int i3 = 0; i3 < (int)elementsCount8; i3++)
					{
						EquipmentPlan element5 = this.EquipmentsPlans[i3] ?? new EquipmentPlan();
						pCurrData += element5.Deserialize(pCurrData);
						this.EquipmentsPlans[i3] = element5;
					}
				}
				else
				{
					this.EquipmentsPlans = null;
				}
			}
			bool flag41 = fieldCount > 21;
			if (flag41)
			{
				this.CurrEquipmentPlanId = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag42 = fieldCount > 22;
			if (flag42)
			{
				ushort elementsCount9 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag43 = elementsCount9 > 0;
				if (flag43)
				{
					bool flag44 = this.WeaponInnerRatios == null || this.WeaponInnerRatios.Length != (int)elementsCount9;
					if (flag44)
					{
						this.WeaponInnerRatios = new sbyte[(int)elementsCount9];
					}
					for (int i4 = 0; i4 < (int)elementsCount9; i4++)
					{
						this.WeaponInnerRatios[i4] = *(sbyte*)(pCurrData + i4);
					}
					pCurrData += elementsCount9;
				}
				else
				{
					this.WeaponInnerRatios = null;
				}
			}
			bool flag45 = fieldCount > 23;
			if (flag45)
			{
				this.VoiceWeaponInnerRatio = *(sbyte*)pCurrData;
				pCurrData++;
			}
			bool flag46 = fieldCount > 24;
			if (flag46)
			{
				pCurrData += SerializationHelper.DictionaryOfCustomTypePair.Deserialize<ItemKey, ReadingBookStrategies>(pCurrData, ref this.ReadingBooks);
			}
			bool flag47 = fieldCount > 25;
			if (flag47)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ObsoleteProfessionData>(pCurrData, ref this.ObsoleteProfessions);
			}
			bool flag48 = fieldCount > 26;
			if (flag48)
			{
				this.CurrProfessionId = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag49 = fieldCount > 27;
			if (flag49)
			{
				pCurrData += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(pCurrData, ref this.TreasuryItems);
			}
			bool flag50 = fieldCount > 28;
			if (flag50)
			{
				pCurrData += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(pCurrData, ref this.TroughItems);
			}
			bool flag51 = fieldCount > 29;
			if (flag51)
			{
				ushort elementsCount10 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag52 = elementsCount10 > 0;
				if (flag52)
				{
					bool flag53 = this.ReadingEventBookIdList == null;
					if (flag53)
					{
						this.ReadingEventBookIdList = new List<int>((int)elementsCount10);
					}
					else
					{
						this.ReadingEventBookIdList.Clear();
					}
					for (int i5 = 0; i5 < (int)elementsCount10; i5++)
					{
						this.ReadingEventBookIdList.Add(*(int*)(pCurrData + (IntPtr)i5 * 4));
					}
					pCurrData += 4 * elementsCount10;
				}
				else
				{
					List<int> readingEventBookIdList = this.ReadingEventBookIdList;
					if (readingEventBookIdList != null)
					{
						readingEventBookIdList.Clear();
					}
				}
			}
			bool flag54 = fieldCount > 30;
			if (flag54)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ShortList>(pCurrData, ref this.ReadingEventReferenceBooks);
			}
			bool flag55 = fieldCount > 31;
			if (flag55)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, IntPair>(pCurrData, ref this.ClearedSkillPlateStepInfo);
			}
			bool flag56 = fieldCount > 32;
			if (flag56)
			{
				pCurrData += this.TaiwuMaxNeiliAllocation.Deserialize(pCurrData);
			}
			bool flag57 = fieldCount > 33;
			if (flag57)
			{
				pCurrData += this.CurrMasteredCombatSkillPlan.Deserialize(pCurrData);
			}
			bool flag58 = fieldCount > 34;
			if (flag58)
			{
				ushort elementsCount11 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag59 = elementsCount11 > 0;
				if (flag59)
				{
					bool flag60 = this.MasteredCombatSkillPlans == null || this.MasteredCombatSkillPlans.Length != (int)elementsCount11;
					if (flag60)
					{
						this.MasteredCombatSkillPlans = new ShortList[(int)elementsCount11];
					}
					for (int i6 = 0; i6 < (int)elementsCount11; i6++)
					{
						ShortList element6 = default(ShortList);
						pCurrData += element6.Deserialize(pCurrData);
						this.MasteredCombatSkillPlans[i6] = element6;
					}
				}
				else
				{
					this.MasteredCombatSkillPlans = null;
				}
			}
			bool flag61 = fieldCount > 35;
			if (flag61)
			{
				this.TaiwuExp = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag62 = fieldCount > 36;
			if (flag62)
			{
				pCurrData += this.TaiwuResources.Deserialize(pCurrData);
			}
			bool flag63 = fieldCount > 37;
			if (flag63)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<sbyte, sbyte>(pCurrData, ref this.LegendaryBookBreakPlateCounts);
			}
			bool flag64 = fieldCount > 38;
			if (flag64)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakPlateObsoleteList>(pCurrData, ref this.CombatSkillBreakPlateObsoleteList);
			}
			bool flag65 = fieldCount > 39;
			if (flag65)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, IntList>(pCurrData, ref this.CombatSkillBreakPlateLastClearTimeList);
			}
			bool flag66 = fieldCount > 40;
			if (flag66)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, IntList>(pCurrData, ref this.CombatSkillBreakPlateLastForceBreakoutStepsCount);
			}
			bool flag67 = fieldCount > 41;
			if (flag67)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<short, sbyte>(pCurrData, ref this.CombatSkillCurrBreakPlateIndex);
			}
			bool flag68 = fieldCount > 42;
			if (flag68)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<sbyte, ItemKey>(pCurrData, ref this.LegendaryBookWeaponSlot);
			}
			bool flag69 = fieldCount > 43;
			if (flag69)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<sbyte, long>(pCurrData, ref this.LegendaryBookWeaponEffectId);
			}
			bool flag70 = fieldCount > 44;
			if (flag70)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<sbyte, ShortList>(pCurrData, ref this.LegendaryBookSkillSlot);
			}
			bool flag71 = fieldCount > 45;
			if (flag71)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<sbyte, LongList>(pCurrData, ref this.LegendaryBookSkillEffectId);
			}
			bool flag72 = fieldCount > 46;
			if (flag72)
			{
				pCurrData += this.LegendaryBookBonusCountYin.Deserialize(pCurrData);
			}
			bool flag73 = fieldCount > 47;
			if (flag73)
			{
				pCurrData += this.LegendaryBookBonusCountYang.Deserialize(pCurrData);
			}
			bool flag74 = fieldCount > 48;
			if (flag74)
			{
				ushort elementsCount12 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag75 = elementsCount12 > 0;
				if (flag75)
				{
					bool flag76 = this.HandledOneShotEvents == null;
					if (flag76)
					{
						this.HandledOneShotEvents = new List<int>((int)elementsCount12);
					}
					else
					{
						this.HandledOneShotEvents.Clear();
					}
					for (int i7 = 0; i7 < (int)elementsCount12; i7++)
					{
						this.HandledOneShotEvents.Add(*(int*)(pCurrData + (IntPtr)i7 * 4));
					}
					pCurrData += 4 * elementsCount12;
				}
				else
				{
					List<int> handledOneShotEvents = this.HandledOneShotEvents;
					if (handledOneShotEvents != null)
					{
						handledOneShotEvents.Clear();
					}
				}
			}
			bool flag77 = fieldCount > 49;
			if (flag77)
			{
				ushort elementsCount13 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag78 = elementsCount13 > 0;
				if (flag78)
				{
					bool flag79 = this.LegaciesBuildingTemplateIds == null;
					if (flag79)
					{
						this.LegaciesBuildingTemplateIds = new List<short>((int)elementsCount13);
					}
					else
					{
						this.LegaciesBuildingTemplateIds.Clear();
					}
					for (int i8 = 0; i8 < (int)elementsCount13; i8++)
					{
						this.LegaciesBuildingTemplateIds.Add(*(short*)(pCurrData + (IntPtr)i8 * 2));
					}
					pCurrData += 2 * elementsCount13;
				}
				else
				{
					List<short> legaciesBuildingTemplateIds = this.LegaciesBuildingTemplateIds;
					if (legaciesBuildingTemplateIds != null)
					{
						legaciesBuildingTemplateIds.Clear();
					}
				}
			}
			bool flag80 = fieldCount > 50;
			if (flag80)
			{
				ushort elementsCount14 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag81 = elementsCount14 > 0;
				if (flag81)
				{
					bool flag82 = this.CollectionCrickets == null || this.CollectionCrickets.Length != (int)elementsCount14;
					if (flag82)
					{
						this.CollectionCrickets = new ItemKey[(int)elementsCount14];
					}
					for (int i9 = 0; i9 < (int)elementsCount14; i9++)
					{
						ItemKey element7 = default(ItemKey);
						pCurrData += element7.Deserialize(pCurrData);
						this.CollectionCrickets[i9] = element7;
					}
				}
				else
				{
					this.CollectionCrickets = null;
				}
			}
			bool flag83 = fieldCount > 51;
			if (flag83)
			{
				ushort elementsCount15 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag84 = elementsCount15 > 0;
				if (flag84)
				{
					bool flag85 = this.CollectionCricketRegen == null || this.CollectionCricketRegen.Length != (int)elementsCount15;
					if (flag85)
					{
						this.CollectionCricketRegen = new int[(int)elementsCount15];
					}
					for (int i10 = 0; i10 < (int)elementsCount15; i10++)
					{
						this.CollectionCricketRegen[i10] = *(int*)(pCurrData + (IntPtr)i10 * 4);
					}
					pCurrData += 4 * elementsCount15;
				}
				else
				{
					this.CollectionCricketRegen = null;
				}
			}
			bool flag86 = fieldCount > 52;
			if (flag86)
			{
				ushort elementsCount16 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag87 = elementsCount16 > 0;
				if (flag87)
				{
					bool flag88 = this.CollectionCricketJars == null || this.CollectionCricketJars.Length != (int)elementsCount16;
					if (flag88)
					{
						this.CollectionCricketJars = new ItemKey[(int)elementsCount16];
					}
					for (int i11 = 0; i11 < (int)elementsCount16; i11++)
					{
						ItemKey element8 = default(ItemKey);
						pCurrData += element8.Deserialize(pCurrData);
						this.CollectionCricketJars[i11] = element8;
					}
				}
				else
				{
					this.CollectionCricketJars = null;
				}
			}
			bool flag89 = fieldCount > 53;
			if (flag89)
			{
				this.BuildingSpaceExtraAdd = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag90 = fieldCount > 54;
			if (flag90)
			{
				ushort fieldSize = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag91 = fieldSize > 0;
				if (flag91)
				{
					bool flag92 = this.NormalInformation == null;
					if (flag92)
					{
						this.NormalInformation = new NormalInformationCollection();
					}
					pCurrData += this.NormalInformation.Deserialize(pCurrData);
				}
				else
				{
					this.NormalInformation = null;
				}
			}
			bool flag93 = fieldCount > 55;
			if (flag93)
			{
				ushort elementsCount17 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag94 = elementsCount17 > 0;
				if (flag94)
				{
					bool flag95 = this.JiaoPools == null;
					if (flag95)
					{
						this.JiaoPools = new List<JiaoPool>((int)elementsCount17);
					}
					else
					{
						this.JiaoPools.Clear();
					}
					for (int i12 = 0; i12 < (int)elementsCount17; i12++)
					{
						ushort subDataCount3 = *(ushort*)pCurrData;
						pCurrData += 2;
						bool flag96 = subDataCount3 > 0;
						if (flag96)
						{
							JiaoPool element9 = new JiaoPool();
							pCurrData += element9.Deserialize(pCurrData);
							this.JiaoPools.Add(element9);
						}
						else
						{
							this.JiaoPools.Add(null);
						}
					}
				}
				else
				{
					List<JiaoPool> jiaoPools = this.JiaoPools;
					if (jiaoPools != null)
					{
						jiaoPools.Clear();
					}
				}
			}
			bool flag97 = fieldCount > 56;
			if (flag97)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakBonusCollection>(pCurrData, ref this.SectEmeiSkillBreakBonus);
			}
			bool flag98 = fieldCount > 57;
			if (flag98)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, ShortList>(pCurrData, ref this.SectEmeiBreakBonusTemplateIds);
			}
			bool flag99 = fieldCount > 58;
			if (flag99)
			{
				this.MaxTaiwuVillageLevel = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag100 = fieldCount > 59;
			if (flag100)
			{
				this.IsJiaoPoolOpen = (*pCurrData != 0);
				pCurrData++;
			}
			bool flag101 = fieldCount > 60;
			if (flag101)
			{
				ushort elementsCount18 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag102 = elementsCount18 > 0;
				if (flag102)
				{
					bool flag103 = this.CricketCollectionDatas == null;
					if (flag103)
					{
						this.CricketCollectionDatas = new List<CricketCollectionData>((int)elementsCount18);
					}
					else
					{
						this.CricketCollectionDatas.Clear();
					}
					for (int i13 = 0; i13 < (int)elementsCount18; i13++)
					{
						ushort subDataCount4 = *(ushort*)pCurrData;
						pCurrData += 2;
						bool flag104 = subDataCount4 > 0;
						if (flag104)
						{
							CricketCollectionData element10 = new CricketCollectionData();
							pCurrData += element10.Deserialize(pCurrData);
							this.CricketCollectionDatas.Add(element10);
						}
						else
						{
							this.CricketCollectionDatas.Add(null);
						}
					}
				}
				else
				{
					List<CricketCollectionData> cricketCollectionDatas = this.CricketCollectionDatas;
					if (cricketCollectionDatas != null)
					{
						cricketCollectionDatas.Clear();
					}
				}
			}
			bool flag105 = fieldCount > 61;
			if (flag105)
			{
				this.UnlockedCombatSkillPlanCount = *pCurrData;
				pCurrData++;
			}
			bool flag106 = fieldCount > 62;
			if (flag106)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<int, SByteList>(pCurrData, ref this.AvailableReadingStrategyMap);
			}
			bool flag107 = fieldCount > 63;
			if (flag107)
			{
				pCurrData += this.ExtraNeiliAllocationProgress.Deserialize(pCurrData);
			}
			bool flag108 = fieldCount > 64;
			if (flag108)
			{
				pCurrData += this.ExtraNeiliAllocation.Deserialize(pCurrData);
			}
			bool flag109 = fieldCount > 65;
			if (flag109)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SectEmeiBreakBonusData>(pCurrData, ref this.SectEmeiBonusData);
			}
			bool flag110 = fieldCount > 66;
			if (flag110)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, IntList>(pCurrData, ref this.SectFulongOrgMemberChickens);
			}
			bool flag111 = fieldCount > 67;
			if (flag111)
			{
				ushort fieldSize2 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag112 = fieldSize2 > 0;
				if (flag112)
				{
					bool flag113 = this._craftStorageObsolete == null;
					if (flag113)
					{
						this._craftStorageObsolete = new TaiwuVillageStorage();
					}
					pCurrData += this._craftStorageObsolete.Deserialize(pCurrData);
				}
				else
				{
					this._craftStorageObsolete = null;
				}
			}
			bool flag114 = fieldCount > 68;
			if (flag114)
			{
				ushort fieldSize3 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag115 = fieldSize3 > 0;
				if (flag115)
				{
					bool flag116 = this._medicineStorageObsolete == null;
					if (flag116)
					{
						this._medicineStorageObsolete = new TaiwuVillageStorage();
					}
					pCurrData += this._medicineStorageObsolete.Deserialize(pCurrData);
				}
				else
				{
					this._medicineStorageObsolete = null;
				}
			}
			bool flag117 = fieldCount > 69;
			if (flag117)
			{
				ushort fieldSize4 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag118 = fieldSize4 > 0;
				if (flag118)
				{
					bool flag119 = this._foodStorageObsolete == null;
					if (flag119)
					{
						this._foodStorageObsolete = new TaiwuVillageStorage();
					}
					pCurrData += this._foodStorageObsolete.Deserialize(pCurrData);
				}
				else
				{
					this._foodStorageObsolete = null;
				}
			}
			bool flag120 = fieldCount > 70;
			if (flag120)
			{
				ushort fieldSize5 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag121 = fieldSize5 > 0;
				if (flag121)
				{
					bool flag122 = this._stockStorageObsolete == null;
					if (flag122)
					{
						this._stockStorageObsolete = new TaiwuVillageStorage();
					}
					pCurrData += this._stockStorageObsolete.Deserialize(pCurrData);
				}
				else
				{
					this._stockStorageObsolete = null;
				}
			}
			bool flag123 = fieldCount > 71;
			if (flag123)
			{
				ushort fieldSize6 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag124 = fieldSize6 > 0;
				if (flag124)
				{
					bool flag125 = this._taiwuSettlementTreasuryObsolete == null;
					if (flag125)
					{
						this._taiwuSettlementTreasuryObsolete = new SettlementTreasury();
					}
					pCurrData += this._taiwuSettlementTreasuryObsolete.Deserialize(pCurrData);
				}
				else
				{
					this._taiwuSettlementTreasuryObsolete = null;
				}
			}
			bool flag126 = fieldCount > 72;
			if (flag126)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ProfessionData>(pCurrData, ref this.Professions);
			}
			bool flag127 = fieldCount > 73;
			if (flag127)
			{
				ushort fieldSize7 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag128 = fieldSize7 > 0;
				if (flag128)
				{
					bool flag129 = this.ExternalEquippedCombatSkills == null;
					if (flag129)
					{
						this.ExternalEquippedCombatSkills = new CombatSkillPlan();
					}
					pCurrData += this.ExternalEquippedCombatSkills.Deserialize(pCurrData);
				}
				else
				{
					this.ExternalEquippedCombatSkills = null;
				}
			}
			bool flag130 = fieldCount > 74;
			if (flag130)
			{
				ushort fieldSize8 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag131 = fieldSize8 > 0;
				if (flag131)
				{
					bool flag132 = this.SectZhujianGearMate == null;
					if (flag132)
					{
						this.SectZhujianGearMate = new GearMateDreamBackData();
					}
					pCurrData += this.SectZhujianGearMate.Deserialize(pCurrData);
				}
				else
				{
					this.SectZhujianGearMate = null;
				}
			}
			bool flag133 = fieldCount > 75;
			if (flag133)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakPlateList>(pCurrData, ref this.CombatSkillBreakPlateList);
			}
			bool flag134 = fieldCount > 76;
			if (flag134)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakPlate>(pCurrData, ref this.SkillBreakPlateDict);
			}
			bool flag135 = fieldCount > 77;
			if (flag135)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<short, int>(pCurrData, ref this.TaiwuCombatSkillProficiencies);
			}
			bool flag136 = fieldCount > 78;
			if (flag136)
			{
				ushort elementsCount19 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag137 = elementsCount19 > 0;
				if (flag137)
				{
					bool flag138 = this.XiangshuIdInKungfuPracticeRoom == null;
					if (flag138)
					{
						this.XiangshuIdInKungfuPracticeRoom = new List<sbyte>((int)elementsCount19);
					}
					else
					{
						this.XiangshuIdInKungfuPracticeRoom.Clear();
					}
					for (int i14 = 0; i14 < (int)elementsCount19; i14++)
					{
						this.XiangshuIdInKungfuPracticeRoom.Add(*(sbyte*)(pCurrData + i14));
					}
					pCurrData += elementsCount19;
				}
				else
				{
					List<sbyte> xiangshuIdInKungfuPracticeRoom = this.XiangshuIdInKungfuPracticeRoom;
					if (xiangshuIdInKungfuPracticeRoom != null)
					{
						xiangshuIdInKungfuPracticeRoom.Clear();
					}
				}
			}
			bool flag139 = fieldCount > 79;
			if (flag139)
			{
				int fieldSize9 = *(int*)pCurrData;
				pCurrData += 4;
				bool flag140 = fieldSize9 > 0;
				if (flag140)
				{
					bool flag141 = this._craftStorage == null;
					if (flag141)
					{
						this._craftStorage = new TaiwuVillageStorage();
					}
					pCurrData += this._craftStorage.Deserialize(pCurrData);
				}
				else
				{
					this._craftStorage = null;
				}
			}
			bool flag142 = fieldCount > 80;
			if (flag142)
			{
				int fieldSize10 = *(int*)pCurrData;
				pCurrData += 4;
				bool flag143 = fieldSize10 > 0;
				if (flag143)
				{
					bool flag144 = this._medicineStorage == null;
					if (flag144)
					{
						this._medicineStorage = new TaiwuVillageStorage();
					}
					pCurrData += this._medicineStorage.Deserialize(pCurrData);
				}
				else
				{
					this._medicineStorage = null;
				}
			}
			bool flag145 = fieldCount > 81;
			if (flag145)
			{
				int fieldSize11 = *(int*)pCurrData;
				pCurrData += 4;
				bool flag146 = fieldSize11 > 0;
				if (flag146)
				{
					bool flag147 = this._foodStorage == null;
					if (flag147)
					{
						this._foodStorage = new TaiwuVillageStorage();
					}
					pCurrData += this._foodStorage.Deserialize(pCurrData);
				}
				else
				{
					this._foodStorage = null;
				}
			}
			bool flag148 = fieldCount > 82;
			if (flag148)
			{
				int fieldSize12 = *(int*)pCurrData;
				pCurrData += 4;
				bool flag149 = fieldSize12 > 0;
				if (flag149)
				{
					bool flag150 = this._stockStorage == null;
					if (flag150)
					{
						this._stockStorage = new TaiwuVillageStorage();
					}
					pCurrData += this._stockStorage.Deserialize(pCurrData);
				}
				else
				{
					this._stockStorage = null;
				}
			}
			bool flag151 = fieldCount > 83;
			if (flag151)
			{
				int fieldSize13 = *(int*)pCurrData;
				pCurrData += 4;
				bool flag152 = fieldSize13 > 0;
				if (flag152)
				{
					bool flag153 = this._taiwuSettlementTreasury == null;
					if (flag153)
					{
						this._taiwuSettlementTreasury = new SettlementTreasury();
					}
					pCurrData += this._taiwuSettlementTreasury.Deserialize(pCurrData);
				}
				else
				{
					this._taiwuSettlementTreasury = null;
				}
			}
			bool flag154 = fieldCount > 84;
			if (flag154)
			{
				ushort elementsCount20 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag155 = elementsCount20 > 0;
				if (flag155)
				{
					bool flag156 = this.TaiwuVillageBlocksEx == null;
					if (flag156)
					{
						this.TaiwuVillageBlocksEx = new List<BuildingBlockDataEx>((int)elementsCount20);
					}
					else
					{
						this.TaiwuVillageBlocksEx.Clear();
					}
					for (int i15 = 0; i15 < (int)elementsCount20; i15++)
					{
						ushort subDataCount5 = *(ushort*)pCurrData;
						pCurrData += 2;
						bool flag157 = subDataCount5 > 0;
						if (flag157)
						{
							BuildingBlockDataEx element11 = new BuildingBlockDataEx();
							pCurrData += element11.Deserialize(pCurrData);
							this.TaiwuVillageBlocksEx.Add(element11);
						}
						else
						{
							this.TaiwuVillageBlocksEx.Add(null);
						}
					}
				}
				else
				{
					List<BuildingBlockDataEx> taiwuVillageBlocksEx = this.TaiwuVillageBlocksEx;
					if (taiwuVillageBlocksEx != null)
					{
						taiwuVillageBlocksEx.Clear();
					}
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040016A6 RID: 5798
		[SerializableGameDataField(SerializationHandler = "CrossArchiveTaiwuSerializer")]
		public Character TaiwuChar;

		// Token: 0x040016A7 RID: 5799
		public AbridgedCharacter DreamBackTaiwuAbridged;

		// Token: 0x040016A8 RID: 5800
		public int NextObjectId;

		// Token: 0x040016A9 RID: 5801
		public Dictionary<int, AbridgedCharacter> AbridgedCharacters;

		// Token: 0x040016AA RID: 5802
		public ReadonlyLifeRecords LifeRecords;

		// Token: 0x040016AB RID: 5803
		public Genealogy Genealogy;

		// Token: 0x040016AC RID: 5804
		public Dictionary<int, DeadCharacter> PreexistenceDeadCharacters;

		// Token: 0x040016AD RID: 5805
		[SerializableGameDataField]
		public ResourceInts TaiwuResources;

		// Token: 0x040016AE RID: 5806
		[SerializableGameDataField]
		public int TaiwuExp;

		// Token: 0x040016AF RID: 5807
		[SerializableGameDataField]
		public CombatSkillPlan ExternalEquippedCombatSkills;

		// Token: 0x040016B0 RID: 5808
		public int FuyuFaith;

		// Token: 0x040016B1 RID: 5809
		[SerializableGameDataField]
		public NormalInformationCollection NormalInformation;

		// Token: 0x040016B2 RID: 5810
		[SerializableGameDataField]
		public List<CombatSkill> CombatSkills;

		// Token: 0x040016B3 RID: 5811
		[SerializableGameDataField]
		public List<SpecialEffectWrapper> TaiwuEffects;

		// Token: 0x040016B4 RID: 5812
		public ItemGroupPackage ItemGroupPackage;

		// Token: 0x040016B5 RID: 5813
		[SerializableGameDataField]
		public Dictionary<int, ItemKey> UnpackedItems;

		// Token: 0x040016B6 RID: 5814
		[SerializableGameDataField]
		public Location TaiwuVillageLocation;

		// Token: 0x040016B7 RID: 5815
		[SerializableGameDataField]
		public BuildingAreaData TaiwuVillageAreaData;

		// Token: 0x040016B8 RID: 5816
		[SerializableGameDataField]
		public List<BuildingBlockData> TaiwuVillageBlocks;

		// Token: 0x040016B9 RID: 5817
		[SerializableGameDataField]
		public List<BuildingBlockDataEx> TaiwuVillageBlocksEx;

		// Token: 0x040016BA RID: 5818
		[SerializableGameDataField]
		public Dictionary<int, Chicken> Chicken;

		// Token: 0x040016BB RID: 5819
		[SerializableGameDataField]
		public List<sbyte> XiangshuIdInKungfuPracticeRoom;

		// Token: 0x040016BC RID: 5820
		public CombatSkillShorts SamsaraPlatformAddCombatSkillQualifications;

		// Token: 0x040016BD RID: 5821
		public LifeSkillShorts SamsaraPlatformAddLifeSkillQualifications;

		// Token: 0x040016BE RID: 5822
		public MainAttributes SamsaraPlatformAddMainAttributes;

		// Token: 0x040016BF RID: 5823
		[SerializableGameDataField]
		[Obsolete]
		public ItemKey[] CollectionCrickets;

		// Token: 0x040016C0 RID: 5824
		[SerializableGameDataField]
		[Obsolete]
		public ItemKey[] CollectionCricketJars;

		// Token: 0x040016C1 RID: 5825
		[SerializableGameDataField]
		[Obsolete]
		public int[] CollectionCricketRegen;

		// Token: 0x040016C2 RID: 5826
		[SerializableGameDataField]
		public List<CricketCollectionData> CricketCollectionDatas;

		// Token: 0x040016C3 RID: 5827
		[SerializableGameDataField]
		public Dictionary<ItemKey, int> WarehouseItems;

		// Token: 0x040016C4 RID: 5828
		[SerializableGameDataField]
		public Dictionary<short, TaiwuCombatSkill> TaiwuCombatSkills;

		// Token: 0x040016C5 RID: 5829
		[SerializableGameDataField]
		public Dictionary<short, TaiwuLifeSkill> TaiwuLifeSkills;

		// Token: 0x040016C6 RID: 5830
		[SerializableGameDataField]
		public Dictionary<short, TaiwuCombatSkill> NotLearnedCombatSkills;

		// Token: 0x040016C7 RID: 5831
		[SerializableGameDataField]
		public Dictionary<short, TaiwuLifeSkill> NotLearnedLifeSkills;

		// Token: 0x040016C8 RID: 5832
		[SerializableGameDataField]
		public CombatSkillPlan[] CombatSkillPlans;

		// Token: 0x040016C9 RID: 5833
		[SerializableGameDataField]
		public int CurrCombatSkillPlanId;

		// Token: 0x040016CA RID: 5834
		[SerializableGameDataField]
		public sbyte[] CurrLifeSkillAttainmentPanelPlanIndex;

		// Token: 0x040016CB RID: 5835
		[SerializableGameDataField]
		public Dictionary<short, SkillBreakPlate> SkillBreakPlateDict;

		// Token: 0x040016CC RID: 5836
		[SerializableGameDataField]
		public Dictionary<short, SkillBreakPlateObsolete> SkillBreakPlateObsoleteDict;

		// Token: 0x040016CD RID: 5837
		[SerializableGameDataField]
		public Dictionary<short, SkillBreakBonusCollection> SkillBreakBonusDict;

		// Token: 0x040016CE RID: 5838
		[SerializableGameDataField]
		public short[] CombatSkillAttainmentPanelPlans;

		// Token: 0x040016CF RID: 5839
		[SerializableGameDataField]
		public sbyte[] CurrCombatSkillAttainmentPanelPlanIds;

		// Token: 0x040016D0 RID: 5840
		[SerializableGameDataField]
		public EquipmentPlan[] EquipmentsPlans;

		// Token: 0x040016D1 RID: 5841
		[SerializableGameDataField]
		public int CurrEquipmentPlanId;

		// Token: 0x040016D2 RID: 5842
		[SerializableGameDataField]
		public sbyte[] WeaponInnerRatios;

		// Token: 0x040016D3 RID: 5843
		[SerializableGameDataField]
		public sbyte VoiceWeaponInnerRatio;

		// Token: 0x040016D4 RID: 5844
		[SerializableGameDataField]
		public Dictionary<ItemKey, ReadingBookStrategies> ReadingBooks;

		// Token: 0x040016D5 RID: 5845
		public Dictionary<int, NotificationSortingGroup> MonthlyNotificationSortingGroups;

		// Token: 0x040016D6 RID: 5846
		public List<int> PreviousTaiwuCharIds;

		// Token: 0x040016D7 RID: 5847
		[SerializableGameDataField]
		public int BuildingSpaceExtraAdd;

		// Token: 0x040016D8 RID: 5848
		[SerializableGameDataField]
		public IntList ExtraNeiliAllocationProgress;

		// Token: 0x040016D9 RID: 5849
		[SerializableGameDataField]
		public NeiliAllocation ExtraNeiliAllocation;

		// Token: 0x040016DA RID: 5850
		public Dictionary<int, string> CustomTexts;

		// Token: 0x040016DB RID: 5851
		public int NextCustomTextId;

		// Token: 0x040016DC RID: 5852
		public int FinalDateBeforeDreamBack;

		// Token: 0x040016DD RID: 5853
		public WorldCreationInfo WorldCreationInfo;

		// Token: 0x040016DE RID: 5854
		public uint WorldId;

		// Token: 0x040016DF RID: 5855
		[Obsolete]
		[SerializableGameDataField]
		public int CurrProfessionId;

		// Token: 0x040016E0 RID: 5856
		[Obsolete]
		[SerializableGameDataField]
		public Dictionary<int, ObsoleteProfessionData> ObsoleteProfessions;

		// Token: 0x040016E1 RID: 5857
		[SerializableGameDataField]
		public Dictionary<int, ProfessionData> Professions;

		// Token: 0x040016E2 RID: 5858
		[SerializableGameDataField]
		public List<int> HandledOneShotEvents;

		// Token: 0x040016E3 RID: 5859
		[SerializableGameDataField]
		public Dictionary<ItemKey, int> TreasuryItems;

		// Token: 0x040016E4 RID: 5860
		[SerializableGameDataField]
		public Dictionary<ItemKey, int> TroughItems;

		// Token: 0x040016E5 RID: 5861
		[SerializableGameDataField]
		public List<short> LegaciesBuildingTemplateIds;

		// Token: 0x040016E6 RID: 5862
		[SerializableGameDataField]
		public List<int> ReadingEventBookIdList;

		// Token: 0x040016E7 RID: 5863
		[SerializableGameDataField]
		public Dictionary<int, ShortList> ReadingEventReferenceBooks;

		// Token: 0x040016E8 RID: 5864
		[SerializableGameDataField]
		public Dictionary<int, SByteList> AvailableReadingStrategyMap;

		// Token: 0x040016E9 RID: 5865
		[SerializableGameDataField]
		public Dictionary<short, IntPair> ClearedSkillPlateStepInfo;

		// Token: 0x040016EA RID: 5866
		[SerializableGameDataField]
		public NeiliAllocation TaiwuMaxNeiliAllocation;

		// Token: 0x040016EB RID: 5867
		[SerializableGameDataField]
		public ShortList CurrMasteredCombatSkillPlan;

		// Token: 0x040016EC RID: 5868
		[SerializableGameDataField]
		public ShortList[] MasteredCombatSkillPlans;

		// Token: 0x040016ED RID: 5869
		[SerializableGameDataField]
		public byte UnlockedCombatSkillPlanCount;

		// Token: 0x040016EE RID: 5870
		[SerializableGameDataField]
		public List<JiaoPool> JiaoPools;

		// Token: 0x040016EF RID: 5871
		[SerializableGameDataField]
		public bool IsJiaoPoolOpen;

		// Token: 0x040016F0 RID: 5872
		[SerializableGameDataField]
		public int MaxTaiwuVillageLevel;

		// Token: 0x040016F1 RID: 5873
		[SerializableGameDataField]
		public Dictionary<short, int> TaiwuCombatSkillProficiencies;

		// Token: 0x040016F2 RID: 5874
		[SerializableGameDataField]
		public Dictionary<short, SkillBreakBonusCollection> SectEmeiSkillBreakBonus;

		// Token: 0x040016F3 RID: 5875
		[SerializableGameDataField]
		public Dictionary<short, ShortList> SectEmeiBreakBonusTemplateIds;

		// Token: 0x040016F4 RID: 5876
		[SerializableGameDataField]
		public Dictionary<short, SectEmeiBreakBonusData> SectEmeiBonusData;

		// Token: 0x040016F5 RID: 5877
		[SerializableGameDataField]
		public Dictionary<short, IntList> SectFulongOrgMemberChickens;

		// Token: 0x040016F6 RID: 5878
		[SerializableGameDataField]
		public GearMateDreamBackData SectZhujianGearMate;

		// Token: 0x040016F7 RID: 5879
		[Obsolete]
		[SerializableGameDataField]
		private TaiwuVillageStorage _stockStorageObsolete;

		// Token: 0x040016F8 RID: 5880
		[Obsolete]
		[SerializableGameDataField]
		private TaiwuVillageStorage _craftStorageObsolete;

		// Token: 0x040016F9 RID: 5881
		[Obsolete]
		[SerializableGameDataField]
		private TaiwuVillageStorage _medicineStorageObsolete;

		// Token: 0x040016FA RID: 5882
		[Obsolete]
		[SerializableGameDataField]
		private TaiwuVillageStorage _foodStorageObsolete;

		// Token: 0x040016FB RID: 5883
		[Obsolete]
		[SerializableGameDataField]
		private SettlementTreasury _taiwuSettlementTreasuryObsolete;

		// Token: 0x040016FC RID: 5884
		[SerializableGameDataField(SubDataMaxCount = 2147483647)]
		private TaiwuVillageStorage _stockStorage;

		// Token: 0x040016FD RID: 5885
		[SerializableGameDataField(SubDataMaxCount = 2147483647)]
		[Obsolete]
		private TaiwuVillageStorage _craftStorage;

		// Token: 0x040016FE RID: 5886
		[SerializableGameDataField(SubDataMaxCount = 2147483647)]
		[Obsolete]
		private TaiwuVillageStorage _medicineStorage;

		// Token: 0x040016FF RID: 5887
		[SerializableGameDataField(SubDataMaxCount = 2147483647)]
		[Obsolete]
		private TaiwuVillageStorage _foodStorage;

		// Token: 0x04001700 RID: 5888
		[SerializableGameDataField(SubDataMaxCount = 2147483647)]
		private SettlementTreasury _taiwuSettlementTreasury;

		// Token: 0x04001701 RID: 5889
		public EventArgBox GlobalEventArgBox;

		// Token: 0x04001702 RID: 5890
		public EventArgBox DlcEventArgBox;

		// Token: 0x04001703 RID: 5891
		public Dictionary<ulong, DlcEntryWrapper> DlcEntries;

		// Token: 0x04001704 RID: 5892
		public EventArgBox[] SectMainStoryEventArgBoxes;

		// Token: 0x04001705 RID: 5893
		public Dictionary<short, CharacterPropertyBonus> TaiwuPropertyPermanentBonuses;

		// Token: 0x04001706 RID: 5894
		public byte TaiwuPoisonImmunities;

		// Token: 0x04001707 RID: 5895
		public Dictionary<IntPair, int> CharacterTemporaryFeatures;

		// Token: 0x04001708 RID: 5896
		public List<short> SelectedUniqueLegacies;

		// Token: 0x04001709 RID: 5897
		public HashSetAsDictionary<short> ProficiencyEnoughSkills;

		// Token: 0x0400170A RID: 5898
		public HashSetAsDictionary<short> OwnedClothingSet;

		// Token: 0x0400170B RID: 5899
		public Dictionary<int, short> ClothingDisplayModifications;

		// Token: 0x0400170C RID: 5900
		public bool EnemyUnyieldingFallen;

		// Token: 0x0400170D RID: 5901
		public bool EnemyDisableAi;

		// Token: 0x0400170E RID: 5902
		public short LastTargetDistance;

		// Token: 0x0400170F RID: 5903
		public int LastCricketPlanIndex;

		// Token: 0x04001710 RID: 5904
		public List<CricketCombatPlan> CricketCombatPlans;

		// Token: 0x04001711 RID: 5905
		[SerializableGameDataField]
		public Dictionary<sbyte, sbyte> LegendaryBookBreakPlateCounts;

		// Token: 0x04001712 RID: 5906
		[SerializableGameDataField]
		public Dictionary<short, SkillBreakPlateList> CombatSkillBreakPlateList;

		// Token: 0x04001713 RID: 5907
		[SerializableGameDataField]
		public Dictionary<short, SkillBreakPlateObsoleteList> CombatSkillBreakPlateObsoleteList;

		// Token: 0x04001714 RID: 5908
		[SerializableGameDataField]
		public Dictionary<short, IntList> CombatSkillBreakPlateLastClearTimeList;

		// Token: 0x04001715 RID: 5909
		[SerializableGameDataField]
		public Dictionary<short, IntList> CombatSkillBreakPlateLastForceBreakoutStepsCount;

		// Token: 0x04001716 RID: 5910
		[SerializableGameDataField]
		public Dictionary<short, sbyte> CombatSkillCurrBreakPlateIndex;

		// Token: 0x04001717 RID: 5911
		[SerializableGameDataField]
		public Dictionary<sbyte, ItemKey> LegendaryBookWeaponSlot;

		// Token: 0x04001718 RID: 5912
		[SerializableGameDataField]
		public Dictionary<sbyte, long> LegendaryBookWeaponEffectId;

		// Token: 0x04001719 RID: 5913
		[SerializableGameDataField]
		public Dictionary<sbyte, ShortList> LegendaryBookSkillSlot;

		// Token: 0x0400171A RID: 5914
		[SerializableGameDataField]
		public Dictionary<sbyte, LongList> LegendaryBookSkillEffectId;

		// Token: 0x0400171B RID: 5915
		[SerializableGameDataField]
		public SByteList LegendaryBookBonusCountYin;

		// Token: 0x0400171C RID: 5916
		[SerializableGameDataField]
		public SByteList LegendaryBookBonusCountYang;

		// Token: 0x0400171D RID: 5917
		public List<int> InteractedCharacterList;

		// Token: 0x0400171E RID: 5918
		public List<int> FollowingNpcList;

		// Token: 0x02000B02 RID: 2818
		private static class CrossArchiveTaiwuSerializer
		{
			// Token: 0x060089A8 RID: 35240 RVA: 0x004EF56C File Offset: 0x004ED76C
			public static int GetSerializedSize(Character target)
			{
				return (4 + ((target != null) ? new int?(target.GetSerializedSize()) : null)).GetValueOrDefault();
			}

			// Token: 0x060089A9 RID: 35241 RVA: 0x004EF5C4 File Offset: 0x004ED7C4
			public unsafe static int Serialize(byte* pData, Character target)
			{
				bool flag = target != null;
				byte* pCurrData;
				if (flag)
				{
					pCurrData = pData + 4;
					int fieldSize = target.Serialize(pCurrData);
					pCurrData += fieldSize;
					*(int*)pData = fieldSize;
				}
				else
				{
					*(int*)pData = 0;
					pCurrData = pData + 4;
				}
				return (int)((long)(pCurrData - pData));
			}

			// Token: 0x060089AA RID: 35242 RVA: 0x004EF60C File Offset: 0x004ED80C
			public unsafe static int Deserialize(byte* pData, ref Character target)
			{
				bool flag = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 69, 0);
				byte* pCurrData;
				if (flag)
				{
					ushort fieldSize = *(ushort*)pData;
					pCurrData = pData + 2;
					bool flag2 = fieldSize > 0;
					if (flag2)
					{
						if (target == null)
						{
							target = new Character();
						}
						pCurrData += target.Deserialize_Legacy(pCurrData);
					}
					else
					{
						target = null;
					}
				}
				else
				{
					uint fieldSize2 = *(uint*)pData;
					pCurrData = pData + 4;
					bool flag3 = fieldSize2 > 0U;
					if (flag3)
					{
						if (target == null)
						{
							target = new Character();
						}
						pCurrData += target.Deserialize(pCurrData);
					}
					else
					{
						target = null;
					}
				}
				return (int)((long)(pCurrData - pData));
			}
		}

		// Token: 0x02000B03 RID: 2819
		private static class FieldIds
		{
			// Token: 0x04002D9C RID: 11676
			public const ushort TaiwuChar = 0;

			// Token: 0x04002D9D RID: 11677
			public const ushort CombatSkills = 1;

			// Token: 0x04002D9E RID: 11678
			public const ushort TaiwuEffects = 2;

			// Token: 0x04002D9F RID: 11679
			public const ushort UnpackedItems = 3;

			// Token: 0x04002DA0 RID: 11680
			public const ushort TaiwuVillageLocation = 4;

			// Token: 0x04002DA1 RID: 11681
			public const ushort TaiwuVillageAreaData = 5;

			// Token: 0x04002DA2 RID: 11682
			public const ushort TaiwuVillageBlocks = 6;

			// Token: 0x04002DA3 RID: 11683
			public const ushort Chicken = 7;

			// Token: 0x04002DA4 RID: 11684
			public const ushort WarehouseItems = 8;

			// Token: 0x04002DA5 RID: 11685
			public const ushort TaiwuCombatSkills = 9;

			// Token: 0x04002DA6 RID: 11686
			public const ushort TaiwuLifeSkills = 10;

			// Token: 0x04002DA7 RID: 11687
			public const ushort NotLearnedCombatSkills = 11;

			// Token: 0x04002DA8 RID: 11688
			public const ushort NotLearnedLifeSkills = 12;

			// Token: 0x04002DA9 RID: 11689
			public const ushort CombatSkillPlans = 13;

			// Token: 0x04002DAA RID: 11690
			public const ushort CurrCombatSkillPlanId = 14;

			// Token: 0x04002DAB RID: 11691
			public const ushort CurrLifeSkillAttainmentPanelPlanIndex = 15;

			// Token: 0x04002DAC RID: 11692
			public const ushort SkillBreakPlateObsoleteDict = 16;

			// Token: 0x04002DAD RID: 11693
			public const ushort SkillBreakBonusDict = 17;

			// Token: 0x04002DAE RID: 11694
			public const ushort CombatSkillAttainmentPanelPlans = 18;

			// Token: 0x04002DAF RID: 11695
			public const ushort CurrCombatSkillAttainmentPanelPlanIds = 19;

			// Token: 0x04002DB0 RID: 11696
			public const ushort EquipmentsPlans = 20;

			// Token: 0x04002DB1 RID: 11697
			public const ushort CurrEquipmentPlanId = 21;

			// Token: 0x04002DB2 RID: 11698
			public const ushort WeaponInnerRatios = 22;

			// Token: 0x04002DB3 RID: 11699
			public const ushort VoiceWeaponInnerRatio = 23;

			// Token: 0x04002DB4 RID: 11700
			public const ushort ReadingBooks = 24;

			// Token: 0x04002DB5 RID: 11701
			public const ushort ObsoleteProfessions = 25;

			// Token: 0x04002DB6 RID: 11702
			public const ushort CurrProfessionId = 26;

			// Token: 0x04002DB7 RID: 11703
			public const ushort TreasuryItems = 27;

			// Token: 0x04002DB8 RID: 11704
			public const ushort TroughItems = 28;

			// Token: 0x04002DB9 RID: 11705
			public const ushort ReadingEventBookIdList = 29;

			// Token: 0x04002DBA RID: 11706
			public const ushort ReadingEventReferenceBooks = 30;

			// Token: 0x04002DBB RID: 11707
			public const ushort ClearedSkillPlateStepInfo = 31;

			// Token: 0x04002DBC RID: 11708
			public const ushort TaiwuMaxNeiliAllocation = 32;

			// Token: 0x04002DBD RID: 11709
			public const ushort CurrMasteredCombatSkillPlan = 33;

			// Token: 0x04002DBE RID: 11710
			public const ushort MasteredCombatSkillPlans = 34;

			// Token: 0x04002DBF RID: 11711
			public const ushort TaiwuExp = 35;

			// Token: 0x04002DC0 RID: 11712
			public const ushort TaiwuResources = 36;

			// Token: 0x04002DC1 RID: 11713
			public const ushort LegendaryBookBreakPlateCounts = 37;

			// Token: 0x04002DC2 RID: 11714
			public const ushort CombatSkillBreakPlateObsoleteList = 38;

			// Token: 0x04002DC3 RID: 11715
			public const ushort CombatSkillBreakPlateLastClearTimeList = 39;

			// Token: 0x04002DC4 RID: 11716
			public const ushort CombatSkillBreakPlateLastForceBreakoutStepsCount = 40;

			// Token: 0x04002DC5 RID: 11717
			public const ushort CombatSkillCurrBreakPlateIndex = 41;

			// Token: 0x04002DC6 RID: 11718
			public const ushort LegendaryBookWeaponSlot = 42;

			// Token: 0x04002DC7 RID: 11719
			public const ushort LegendaryBookWeaponEffectId = 43;

			// Token: 0x04002DC8 RID: 11720
			public const ushort LegendaryBookSkillSlot = 44;

			// Token: 0x04002DC9 RID: 11721
			public const ushort LegendaryBookSkillEffectId = 45;

			// Token: 0x04002DCA RID: 11722
			public const ushort LegendaryBookBonusCountYin = 46;

			// Token: 0x04002DCB RID: 11723
			public const ushort LegendaryBookBonusCountYang = 47;

			// Token: 0x04002DCC RID: 11724
			public const ushort HandledOneShotEvents = 48;

			// Token: 0x04002DCD RID: 11725
			public const ushort LegaciesBuildingTemplateIds = 49;

			// Token: 0x04002DCE RID: 11726
			public const ushort CollectionCrickets = 50;

			// Token: 0x04002DCF RID: 11727
			public const ushort CollectionCricketRegen = 51;

			// Token: 0x04002DD0 RID: 11728
			public const ushort CollectionCricketJars = 52;

			// Token: 0x04002DD1 RID: 11729
			public const ushort BuildingSpaceExtraAdd = 53;

			// Token: 0x04002DD2 RID: 11730
			public const ushort NormalInformation = 54;

			// Token: 0x04002DD3 RID: 11731
			public const ushort JiaoPools = 55;

			// Token: 0x04002DD4 RID: 11732
			public const ushort SectEmeiSkillBreakBonus = 56;

			// Token: 0x04002DD5 RID: 11733
			public const ushort SectEmeiBreakBonusTemplateIds = 57;

			// Token: 0x04002DD6 RID: 11734
			public const ushort MaxTaiwuVillageLevel = 58;

			// Token: 0x04002DD7 RID: 11735
			public const ushort IsJiaoPoolOpen = 59;

			// Token: 0x04002DD8 RID: 11736
			public const ushort CricketCollectionDatas = 60;

			// Token: 0x04002DD9 RID: 11737
			public const ushort UnlockedCombatSkillPlanCount = 61;

			// Token: 0x04002DDA RID: 11738
			public const ushort AvailableReadingStrategyMap = 62;

			// Token: 0x04002DDB RID: 11739
			public const ushort ExtraNeiliAllocationProgress = 63;

			// Token: 0x04002DDC RID: 11740
			public const ushort ExtraNeiliAllocation = 64;

			// Token: 0x04002DDD RID: 11741
			public const ushort SectEmeiBonusData = 65;

			// Token: 0x04002DDE RID: 11742
			public const ushort SectFulongOrgMemberChickens = 66;

			// Token: 0x04002DDF RID: 11743
			public const ushort CraftStorageObsolete = 67;

			// Token: 0x04002DE0 RID: 11744
			public const ushort MedicineStorageObsolete = 68;

			// Token: 0x04002DE1 RID: 11745
			public const ushort FoodStorageObsolete = 69;

			// Token: 0x04002DE2 RID: 11746
			public const ushort StockStorageObsolete = 70;

			// Token: 0x04002DE3 RID: 11747
			public const ushort TaiwuSettlementTreasuryObsolete = 71;

			// Token: 0x04002DE4 RID: 11748
			public const ushort Professions = 72;

			// Token: 0x04002DE5 RID: 11749
			public const ushort ExternalEquippedCombatSkills = 73;

			// Token: 0x04002DE6 RID: 11750
			public const ushort SectZhujianGearMate = 74;

			// Token: 0x04002DE7 RID: 11751
			public const ushort CombatSkillBreakPlateList = 75;

			// Token: 0x04002DE8 RID: 11752
			public const ushort SkillBreakPlateDict = 76;

			// Token: 0x04002DE9 RID: 11753
			public const ushort TaiwuCombatSkillProficiencies = 77;

			// Token: 0x04002DEA RID: 11754
			public const ushort XiangshuIdInKungfuPracticeRoom = 78;

			// Token: 0x04002DEB RID: 11755
			public const ushort CraftStorage = 79;

			// Token: 0x04002DEC RID: 11756
			public const ushort MedicineStorage = 80;

			// Token: 0x04002DED RID: 11757
			public const ushort FoodStorage = 81;

			// Token: 0x04002DEE RID: 11758
			public const ushort StockStorage = 82;

			// Token: 0x04002DEF RID: 11759
			public const ushort TaiwuSettlementTreasury = 83;

			// Token: 0x04002DF0 RID: 11760
			public const ushort TaiwuVillageBlocksEx = 84;

			// Token: 0x04002DF1 RID: 11761
			public const ushort Count = 85;

			// Token: 0x04002DF2 RID: 11762
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"TaiwuChar",
				"CombatSkills",
				"TaiwuEffects",
				"UnpackedItems",
				"TaiwuVillageLocation",
				"TaiwuVillageAreaData",
				"TaiwuVillageBlocks",
				"Chicken",
				"WarehouseItems",
				"TaiwuCombatSkills",
				"TaiwuLifeSkills",
				"NotLearnedCombatSkills",
				"NotLearnedLifeSkills",
				"CombatSkillPlans",
				"CurrCombatSkillPlanId",
				"CurrLifeSkillAttainmentPanelPlanIndex",
				"SkillBreakPlateObsoleteDict",
				"SkillBreakBonusDict",
				"CombatSkillAttainmentPanelPlans",
				"CurrCombatSkillAttainmentPanelPlanIds",
				"EquipmentsPlans",
				"CurrEquipmentPlanId",
				"WeaponInnerRatios",
				"VoiceWeaponInnerRatio",
				"ReadingBooks",
				"ObsoleteProfessions",
				"CurrProfessionId",
				"TreasuryItems",
				"TroughItems",
				"ReadingEventBookIdList",
				"ReadingEventReferenceBooks",
				"ClearedSkillPlateStepInfo",
				"TaiwuMaxNeiliAllocation",
				"CurrMasteredCombatSkillPlan",
				"MasteredCombatSkillPlans",
				"TaiwuExp",
				"TaiwuResources",
				"LegendaryBookBreakPlateCounts",
				"CombatSkillBreakPlateObsoleteList",
				"CombatSkillBreakPlateLastClearTimeList",
				"CombatSkillBreakPlateLastForceBreakoutStepsCount",
				"CombatSkillCurrBreakPlateIndex",
				"LegendaryBookWeaponSlot",
				"LegendaryBookWeaponEffectId",
				"LegendaryBookSkillSlot",
				"LegendaryBookSkillEffectId",
				"LegendaryBookBonusCountYin",
				"LegendaryBookBonusCountYang",
				"HandledOneShotEvents",
				"LegaciesBuildingTemplateIds",
				"CollectionCrickets",
				"CollectionCricketRegen",
				"CollectionCricketJars",
				"BuildingSpaceExtraAdd",
				"NormalInformation",
				"JiaoPools",
				"SectEmeiSkillBreakBonus",
				"SectEmeiBreakBonusTemplateIds",
				"MaxTaiwuVillageLevel",
				"IsJiaoPoolOpen",
				"CricketCollectionDatas",
				"UnlockedCombatSkillPlanCount",
				"AvailableReadingStrategyMap",
				"ExtraNeiliAllocationProgress",
				"ExtraNeiliAllocation",
				"SectEmeiBonusData",
				"SectFulongOrgMemberChickens",
				"CraftStorageObsolete",
				"MedicineStorageObsolete",
				"FoodStorageObsolete",
				"StockStorageObsolete",
				"TaiwuSettlementTreasuryObsolete",
				"Professions",
				"ExternalEquippedCombatSkills",
				"SectZhujianGearMate",
				"CombatSkillBreakPlateList",
				"SkillBreakPlateDict",
				"TaiwuCombatSkillProficiencies",
				"XiangshuIdInKungfuPracticeRoom",
				"CraftStorage",
				"MedicineStorage",
				"FoodStorage",
				"StockStorage",
				"TaiwuSettlementTreasury",
				"TaiwuVillageBlocksEx"
			};
		}
	}
}
