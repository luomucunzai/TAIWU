using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using Config.Common;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Common.SingleValueCollection;
using GameData.Dependencies;
using GameData.Domains.Building.Display;
using GameData.Domains.Building.SamsaraPlatformRecord;
using GameData.Domains.Building.ShopEvent;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Organization.Display;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using GameData.Utilities.RandomGenerator;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Building
{
	// Token: 0x020008BF RID: 2239
	[GameDataDomain(9)]
	public class BuildingDomain : BaseGameDataDomain
	{
		// Token: 0x06007BDD RID: 31709 RVA: 0x0049007B File Offset: 0x0048E27B
		private void OnInitializedDomainData()
		{
			this._buildingResidents = new Dictionary<int, BuildingBlockKey>();
			this._buildingComfortableHouses = new Dictionary<int, BuildingBlockKey>();
		}

		// Token: 0x06007BDE RID: 31710 RVA: 0x00490094 File Offset: 0x0048E294
		private void InitializeOnInitializeGameDataModule()
		{
		}

		// Token: 0x06007BDF RID: 31711 RVA: 0x00490097 File Offset: 0x0048E297
		private void InitializeOnEnterNewWorld()
		{
			this.InitializeCricketCollection();
			this.InitializeSamsaraPlatform();
		}

		// Token: 0x06007BE0 RID: 31712 RVA: 0x004900A8 File Offset: 0x0048E2A8
		private void OnLoadedArchiveData()
		{
			Dictionary<int, VillagerWorkData> villagerWorkDict = DomainManager.Taiwu.GetVillagerWorkDict();
			DataContext context = DataContextManager.GetCurrentThreadDataContext();
			foreach (int charId in villagerWorkDict.Keys)
			{
				VillagerWorkData workData = villagerWorkDict[charId];
				BuildingBlockKey blockKey = new BuildingBlockKey(workData.AreaId, workData.BlockId, workData.BuildingBlockIndex);
				switch (workData.WorkType)
				{
				case 0:
					this.SetBuildingOperator(context, blockKey, (int)workData.WorkerIndex, workData.CharacterId);
					break;
				case 1:
					this.SetShopBuildingManager(context, blockKey, (int)workData.WorkerIndex, workData.CharacterId, false);
					break;
				}
			}
			this.FixResidentData(context);
			this.InitializeBuildingResidents(context);
			this.InitializeNextChickenId();
			this.RefreshSettlementChickenIdLists();
		}

		// Token: 0x06007BE1 RID: 31713 RVA: 0x004901AC File Offset: 0x0048E3AC
		public override void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
		{
			this.UpgradeTeaHorseCaravanByAwareness(context);
		}

		// Token: 0x06007BE2 RID: 31714 RVA: 0x004901B8 File Offset: 0x0048E3B8
		public override void OnUpdate(DataContext context)
		{
			base.OnUpdate(context);
			bool flag = !this._needUpdateEffects;
			if (!flag)
			{
				this.UpdateTaiwuVillageBuildingEffect();
				this._needUpdateEffects = false;
			}
		}

		// Token: 0x06007BE3 RID: 31715 RVA: 0x004901EC File Offset: 0x0048E3EC
		[DomainMethod]
		public BuildingAreaData GetBuildingAreaData(Location location)
		{
			return this.GetElement_BuildingAreas(location);
		}

		// Token: 0x06007BE4 RID: 31716 RVA: 0x00490208 File Offset: 0x0048E408
		[DomainMethod]
		public List<BuildingBlockData> GetBuildingBlockList(Location location)
		{
			List<BuildingBlockData> blockList = new List<BuildingBlockData>();
			foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> entry in this._buildingBlocks)
			{
				bool flag = entry.Key.AreaId == location.AreaId && entry.Key.BlockId == location.BlockId;
				if (flag)
				{
					blockList.Add(entry.Value);
				}
			}
			blockList.Sort((BuildingBlockData block1, BuildingBlockData block2) => block1.BlockIndex.CompareTo(block2.BlockIndex));
			return blockList;
		}

		// Token: 0x06007BE5 RID: 31717 RVA: 0x004902CC File Offset: 0x0048E4CC
		[DomainMethod]
		public BuildingBlockData GetBuildingBlockData(BuildingBlockKey blockKey)
		{
			return this.GetElement_BuildingBlocks(blockKey);
		}

		// Token: 0x06007BE6 RID: 31718 RVA: 0x004902E8 File Offset: 0x0048E4E8
		[DomainMethod]
		public void SetBuildingCustomName(DataContext context, BuildingBlockKey blockKey, string name)
		{
			int textId;
			bool hasCustomName = this._customBuildingName.TryGetValue(blockKey, out textId);
			bool flag = hasCustomName ? (DomainManager.World.GetElement_CustomTexts(textId) == name) : string.IsNullOrEmpty(name);
			if (!flag)
			{
				bool flag2 = hasCustomName;
				if (flag2)
				{
					DomainManager.World.UnregisterCustomText(context, textId);
					this.RemoveElement_CustomBuildingName(blockKey, context);
				}
				bool flag3 = !string.IsNullOrEmpty(name);
				if (flag3)
				{
					textId = DomainManager.World.RegisterCustomText(context, name);
					this.AddElement_CustomBuildingName(blockKey, textId, context);
				}
			}
		}

		// Token: 0x06007BE7 RID: 31719 RVA: 0x0049036C File Offset: 0x0048E56C
		[DomainMethod]
		public int GetEmptyBlockCount(short areaId, short blockId)
		{
			int count = 0;
			BuildingAreaData buildingArea = this._buildingAreas[new Location(areaId, blockId)];
			for (short blockIndex = 0; blockIndex < (short)(buildingArea.Width * buildingArea.Width); blockIndex += 1)
			{
				bool flag = this.IsBuildingBlocksEmpty(areaId, blockId, blockIndex, buildingArea.Width, 1);
				if (flag)
				{
					count++;
				}
			}
			return count;
		}

		// Token: 0x06007BE8 RID: 31720 RVA: 0x004903D0 File Offset: 0x0048E5D0
		[DomainMethod]
		public int GetSutraReadingRoomBuffValue()
		{
			short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			int buildingEffect = this.GetBuildingBlockEffect(settlementId, EBuildingScaleEffect.ReadingStrategyCost, -1);
			return Math.Clamp(100 - buildingEffect, 0, 100);
		}

		// Token: 0x06007BE9 RID: 31721 RVA: 0x00490404 File Offset: 0x0048E604
		[DomainMethod]
		public int PracticingCombatSkillInPracticeRoom(DataContext context, BuildingBlockKey blockKey, short skillTemplateId, int count, int cost)
		{
			int proficiency = 0;
			CombatSkillKey combatSkillKey = new CombatSkillKey(DomainManager.Taiwu.GetTaiwuCharId(), skillTemplateId);
			int factor = 1;
			int reduceCostDependBonus = 0;
			int addBreakoutDependBonus = 0;
			int val;
			int origin = DomainManager.Extra.TryGetElement_CombatSkillProficiencies(combatSkillKey, out val) ? val : 0;
			List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
			BuildingBlockData blockData = this.GetBuildingBlockData(blockKey);
			short templateId = blockData.TemplateId;
			Location location = new Location(blockKey.AreaId, blockKey.BlockId);
			bool isTaiwuVillage = DomainManager.Taiwu.GetTaiwuVillageLocation() == location;
			BuildingAreaData areaData = this.GetElement_BuildingAreas(location);
			sbyte buildingWidth = BuildingBlock.Instance[this._buildingBlocks[blockKey].TemplateId].Width;
			areaData.GetNeighborBlocks(blockKey.BuildingBlockIndex, buildingWidth, neighborList, null, 2);
			foreach (short buildingBlockIndex in neighborList)
			{
				BuildingBlockData neighborBlock = this._buildingBlocks[new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, buildingBlockIndex)];
				bool flag = neighborBlock.RootBlockIndex >= 0;
				if (flag)
				{
					neighborBlock = this._buildingBlocks[new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborBlock.RootBlockIndex)];
				}
				bool flag2 = neighborBlock.TemplateId != 0 && neighborBlock.CanUse();
				if (flag2)
				{
					BuildingBlockItem config = BuildingBlock.Instance[neighborBlock.TemplateId];
					bool flag3 = config.DependBuildings != null && config.DependBuildings.Contains(templateId);
					if (flag3)
					{
						bool flag4 = config.ReduceCombatSkillCost >= 0;
						if (flag4)
						{
							reduceCostDependBonus = 1;
						}
						bool flag5 = config.AddCombatSkillBreakout >= 0;
						if (flag5)
						{
							addBreakoutDependBonus = 1;
						}
					}
				}
			}
			factor += reduceCostDependBonus + addBreakoutDependBonus;
			bool flag6 = !isTaiwuVillage;
			if (flag6)
			{
				factor *= 3;
			}
			ObjectPool<List<short>>.Instance.Return(neighborList);
			for (int i = 0; i < count; i++)
			{
				proficiency += context.Random.Next(GlobalConfig.Instance.BaseCombatSkillPracticeProficiencyDelta[0], GlobalConfig.Instance.BaseCombatSkillPracticeProficiencyDelta[1]) * factor;
			}
			DomainManager.Extra.ConsumeActionPoint(context, cost);
			DomainManager.Extra.ChangeCombatSkillProficiency(context, combatSkillKey, proficiency);
			int addSeniority = ProfessionFormulaImpl.Calculate(113, DomainManager.Extra.GetElement_CombatSkillProficiencies(combatSkillKey) - origin);
			DomainManager.Extra.ChangeProfessionSeniority(context, 3, addSeniority, true, false);
			return proficiency;
		}

		// Token: 0x06007BEA RID: 31722 RVA: 0x00490690 File Offset: 0x0048E890
		public List<short> GetAvailableBuildingBlocks(List<short> buildingBlocks, short areaId, short blockId, sbyte areaWidth, sbyte buildingWidth)
		{
			List<short> availableIndexes = new List<short>();
			foreach (short index in buildingBlocks)
			{
				bool flag = this.IsBuildingBlocksEmpty(areaId, blockId, index, areaWidth, buildingWidth);
				if (flag)
				{
					availableIndexes.Add(index);
				}
			}
			return availableIndexes;
		}

		// Token: 0x06007BEB RID: 31723 RVA: 0x00490704 File Offset: 0x0048E904
		public short GetRootBlockContainingIndex(short areaId, short blockId, short index, sbyte areaWidth, sbyte buildingWidth)
		{
			for (int i = 0; i < (int)buildingWidth; i++)
			{
				for (int j = 0; j < (int)buildingWidth; j++)
				{
					short indexWithOffset = (short)((int)index - i * (int)areaWidth - j);
					bool flag = this.IsBuildingBlocksEmpty(areaId, blockId, indexWithOffset, areaWidth, buildingWidth);
					if (flag)
					{
						return indexWithOffset;
					}
				}
			}
			return -1;
		}

		// Token: 0x06007BEC RID: 31724 RVA: 0x00490764 File Offset: 0x0048E964
		public bool RootContainBlock(short areaId, short blockId, short rootIndex, short blockIndex, sbyte areaWidth, sbyte buildingWidth)
		{
			for (int i = 0; i < (int)buildingWidth; i++)
			{
				for (int j = 0; j < (int)buildingWidth; j++)
				{
					short index = (short)((int)rootIndex + i * (int)areaWidth + j);
					bool flag = index == blockIndex;
					if (flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007BED RID: 31725 RVA: 0x004907BC File Offset: 0x0048E9BC
		public bool IsBuildingBlocksEmpty(short areaId, short blockId, short rootIndex, sbyte areaWidth, sbyte buildingWidth)
		{
			int x = (int)(rootIndex % (short)areaWidth);
			int y = (int)(rootIndex / (short)areaWidth);
			bool flag = x + (int)buildingWidth > (int)areaWidth || y + (int)buildingWidth > (int)areaWidth;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < (int)buildingWidth; i++)
				{
					for (int j = 0; j < (int)buildingWidth; j++)
					{
						short index = (short)((int)rootIndex + i * (int)areaWidth + j);
						BuildingBlockKey key = new BuildingBlockKey(areaId, blockId, index);
						bool flag2 = !this._buildingBlocks.ContainsKey(key);
						if (!flag2)
						{
							BuildingBlockData buildingBlock = this._buildingBlocks[key];
							bool flag3 = buildingBlock.TemplateId > 0 || buildingBlock.RootBlockIndex >= 0;
							if (flag3)
							{
								return false;
							}
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06007BEE RID: 31726 RVA: 0x00490894 File Offset: 0x0048EA94
		public void ResetAllChildrenBlocks(DataContext context, BuildingBlockKey blockKey, short templateId, sbyte level = 1)
		{
			BuildingBlockData buildingBlock = this.GetElement_BuildingBlocks(blockKey);
			bool flag = buildingBlock.TemplateId < 0;
			if (!flag)
			{
				int buildingWidth = (int)Math.Max(BuildingBlock.Instance[buildingBlock.TemplateId].Width, BuildingBlock.Instance[templateId].Width);
				MapBlockItem mapBlockData = MapBlock.Instance[DomainManager.Map.GetBlock(blockKey.AreaId, blockKey.BlockId).TemplateId];
				sbyte areaWidth = mapBlockData.BuildingAreaWidth;
				int blockX = (int)(buildingBlock.BlockIndex % (short)areaWidth);
				int blockY = (int)(buildingBlock.BlockIndex / (short)areaWidth);
				for (int i = blockX; i < Math.Min(blockX + buildingWidth, (int)areaWidth); i++)
				{
					for (int j = blockY; j < Math.Min(blockY + buildingWidth, (int)areaWidth); j++)
					{
						short index = (short)(j * (int)areaWidth + i);
						BuildingBlockKey key = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, index);
						bool flag2 = templateId > 0 && templateId != 23 && index != blockKey.BuildingBlockIndex;
						if (flag2)
						{
							this._buildingBlocks[key].ResetData(-1, -1, buildingBlock.BlockIndex);
						}
						else
						{
							this._buildingBlocks[key].ResetData(templateId, level, -1);
						}
						this.SetElement_BuildingBlocks(key, this._buildingBlocks[key], context);
					}
				}
			}
		}

		// Token: 0x06007BEF RID: 31727 RVA: 0x00490A08 File Offset: 0x0048EC08
		public void AddBuilding(DataContext context, short areaId, short blockId, short centerIndex, short templateId, sbyte level, sbyte areaWidth)
		{
			ExtraDomain extraDomain = DomainManager.Extra;
			sbyte buildingWidth = BuildingBlock.Instance[templateId].Width;
			for (int i = 0; i < (int)buildingWidth; i++)
			{
				for (int j = 0; j < (int)buildingWidth; j++)
				{
					short index = (short)((int)centerIndex + i * (int)areaWidth + j);
					BuildingBlockKey key = new BuildingBlockKey(areaId, blockId, index);
					bool flag = this._buildingBlocks.ContainsKey(key);
					if (flag)
					{
						Logger logger = BuildingDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Building ");
						defaultInterpolatedStringHandler.AppendFormatted(BuildingBlock.Instance[templateId].Name);
						defaultInterpolatedStringHandler.AppendLiteral(" with key ");
						defaultInterpolatedStringHandler.AppendFormatted<BuildingBlockKey>(key);
						defaultInterpolatedStringHandler.AppendLiteral(" is being added to a occupied place at ");
						defaultInterpolatedStringHandler.AppendFormatted(MapBlock.Instance[DomainManager.Map.GetBlock(areaId, blockId).TemplateId].Name);
						logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					BuildingBlockData val = (index == centerIndex) ? new BuildingBlockData(index, templateId, level, -1) : new BuildingBlockData(index, -1, -1, centerIndex);
					this.AddElement_BuildingBlocks(key, val, context);
					extraDomain.ModifyBuildingExtraData(context, key, null);
				}
			}
		}

		// Token: 0x06007BF0 RID: 31728 RVA: 0x00490B58 File Offset: 0x0048ED58
		public void PlaceBuilding(DataContext context, short areaId, short blockId, short rootIndex, BuildingBlockData blockData, sbyte areaWidth)
		{
			sbyte buildingWidth = BuildingBlock.Instance[blockData.TemplateId].Width;
			for (int i = 0; i < (int)buildingWidth; i++)
			{
				for (int j = 0; j < (int)buildingWidth; j++)
				{
					short index = (short)((int)rootIndex + i * (int)areaWidth + j);
					BuildingBlockKey key = new BuildingBlockKey(areaId, blockId, index);
					BuildingBlockData val = (index == rootIndex) ? blockData : new BuildingBlockData(index, -1, -1, rootIndex);
					this.SetElement_BuildingBlocks(key, val, context);
				}
			}
		}

		// Token: 0x06007BF1 RID: 31729 RVA: 0x00490BDC File Offset: 0x0048EDDC
		[Obsolete]
		public void PlaceBuildingAtRandomBlock(DataContext context, short areaId, short blockId, short templateId, bool forcePlace)
		{
			BuildingDomain.<>c__DisplayClass29_0 CS$<>8__locals1 = new BuildingDomain.<>c__DisplayClass29_0();
			CS$<>8__locals1.areaId = areaId;
			CS$<>8__locals1.blockId = blockId;
			CS$<>8__locals1.<>4__this = this;
			BuildingBlockItem buildingConfig = BuildingBlock.Instance[templateId];
			List<short> availableBlockIndices = ObjectPool<List<short>>.Instance.Get();
			availableBlockIndices.Clear();
			CS$<>8__locals1.buildingArea = this._buildingAreas[new Location(CS$<>8__locals1.areaId, CS$<>8__locals1.blockId)];
			CS$<>8__locals1.areaWidth = CS$<>8__locals1.buildingArea.Width;
			CS$<>8__locals1.buildingWidth = buildingConfig.Width;
			for (short blockIndex = 0; blockIndex < (short)(CS$<>8__locals1.buildingArea.Width * CS$<>8__locals1.buildingArea.Width); blockIndex += 1)
			{
				bool flag = this.IsBuildingBlocksEmpty(CS$<>8__locals1.areaId, CS$<>8__locals1.blockId, blockIndex, CS$<>8__locals1.areaWidth, buildingConfig.Width);
				if (flag)
				{
					availableBlockIndices.Add(blockIndex);
				}
			}
			bool flag2 = availableBlockIndices.Count == 0;
			if (flag2)
			{
				bool flag3 = !forcePlace;
				if (flag3)
				{
					ObjectPool<List<short>>.Instance.Return(availableBlockIndices);
					return;
				}
				CS$<>8__locals1.<PlaceBuildingAtRandomBlock>g__CalcAvailableBlockIndices|4(availableBlockIndices, (short buildingTemplateId) => BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.UselessResource && BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.Empty);
				bool flag4 = availableBlockIndices.Count == 0;
				if (flag4)
				{
					CS$<>8__locals1.<PlaceBuildingAtRandomBlock>g__CalcAvailableBlockIndices|4(availableBlockIndices, (short buildingTemplateId) => !BuildingBlockData.IsResource(BuildingBlock.Instance[buildingTemplateId].Type) && BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.Empty);
				}
			}
			availableBlockIndices.Sort((short l, short r) => BuildingDomain.<PlaceBuildingAtRandomBlock>g__GetDisToCenter|29_3(l, CS$<>8__locals1.buildingArea.Width) - BuildingDomain.<PlaceBuildingAtRandomBlock>g__GetDisToCenter|29_3(r, CS$<>8__locals1.buildingArea.Width));
			short selectedIndex = availableBlockIndices[0];
			BuildingBlockKey selectedBlockKey = new BuildingBlockKey(CS$<>8__locals1.areaId, CS$<>8__locals1.blockId, selectedIndex);
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(selectedBlockKey);
			blockData.ResetData(templateId, 1, -1);
			this.PlaceBuilding(context, CS$<>8__locals1.areaId, CS$<>8__locals1.blockId, selectedIndex, blockData, CS$<>8__locals1.buildingArea.Width);
		}

		// Token: 0x06007BF2 RID: 31730 RVA: 0x00490DB8 File Offset: 0x0048EFB8
		public void PlaceBuildingAtBlock(DataContext context, short areaId, short blockId, short templateId, bool forcePlace, bool isRandom)
		{
			BuildingDomain.<>c__DisplayClass30_0 CS$<>8__locals1 = new BuildingDomain.<>c__DisplayClass30_0();
			CS$<>8__locals1.areaId = areaId;
			CS$<>8__locals1.blockId = blockId;
			CS$<>8__locals1.<>4__this = this;
			BuildingBlockItem buildingConfig = BuildingBlock.Instance[templateId];
			List<short> availableBlockIndices = ObjectPool<List<short>>.Instance.Get();
			availableBlockIndices.Clear();
			CS$<>8__locals1.buildingArea = this._buildingAreas[new Location(CS$<>8__locals1.areaId, CS$<>8__locals1.blockId)];
			CS$<>8__locals1.areaWidth = CS$<>8__locals1.buildingArea.Width;
			CS$<>8__locals1.buildingWidth = buildingConfig.Width;
			for (short blockIndex = 0; blockIndex < (short)(CS$<>8__locals1.buildingArea.Width * CS$<>8__locals1.buildingArea.Width); blockIndex += 1)
			{
				bool flag = CS$<>8__locals1.<PlaceBuildingAtBlock>g__IsEdge|4(blockIndex);
				if (!flag)
				{
					bool flag2 = this.IsBuildingBlocksEmpty(CS$<>8__locals1.areaId, CS$<>8__locals1.blockId, blockIndex, CS$<>8__locals1.areaWidth, buildingConfig.Width);
					if (flag2)
					{
						availableBlockIndices.Add(blockIndex);
					}
				}
			}
			bool flag3 = availableBlockIndices.Count == 0;
			if (flag3)
			{
				bool flag4 = !forcePlace;
				if (flag4)
				{
					ObjectPool<List<short>>.Instance.Return(availableBlockIndices);
					return;
				}
				CS$<>8__locals1.<PlaceBuildingAtBlock>g__CalcAvailableBlockIndices|5(availableBlockIndices, (short buildingTemplateId) => BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.UselessResource && BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.Empty);
				bool flag5 = availableBlockIndices.Count == 0;
				if (flag5)
				{
					CS$<>8__locals1.<PlaceBuildingAtBlock>g__CalcAvailableBlockIndices|5(availableBlockIndices, (short buildingTemplateId) => !BuildingBlockData.IsResource(BuildingBlock.Instance[buildingTemplateId].Type) && BuildingBlock.Instance[buildingTemplateId].Type != EBuildingBlockType.Empty);
				}
			}
			short selectedIndex;
			if (isRandom)
			{
				selectedIndex = availableBlockIndices.GetRandom(context.Random);
			}
			else
			{
				availableBlockIndices.Sort((short l, short r) => BuildingDomain.<PlaceBuildingAtBlock>g__GetDisToCenter|30_3(l, CS$<>8__locals1.buildingArea.Width) - BuildingDomain.<PlaceBuildingAtBlock>g__GetDisToCenter|30_3(r, CS$<>8__locals1.buildingArea.Width));
				selectedIndex = availableBlockIndices[0];
			}
			BuildingBlockKey selectedBlockKey = new BuildingBlockKey(CS$<>8__locals1.areaId, CS$<>8__locals1.blockId, selectedIndex);
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(selectedBlockKey);
			blockData.ResetData(templateId, 1, -1);
			this.PlaceBuilding(context, CS$<>8__locals1.areaId, CS$<>8__locals1.blockId, selectedIndex, blockData, CS$<>8__locals1.buildingArea.Width);
		}

		// Token: 0x06007BF3 RID: 31731 RVA: 0x00490FC0 File Offset: 0x0048F1C0
		public void XiangshuDestroyTaiwuVillageBuilding(DataContext context, Location location)
		{
			bool flag = !DomainManager.Taiwu.GetTaiwuVillageLocation().Equals(location);
			if (!flag)
			{
				List<BuildingBlockData> destroyableBuildingList = new List<BuildingBlockData>();
				DomainManager.Building.GetDestroyableBuildings(location, destroyableBuildingList);
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
				int destroyCount = context.Random.Next(6, 19);
				for (int i = 0; i < destroyCount; i++)
				{
					bool flag2 = destroyableBuildingList.Count == 0;
					if (flag2)
					{
						DomainManager.World.SetTaiwuVillageDestroyed();
						break;
					}
					int index = context.Random.Next(destroyableBuildingList.Count);
					BuildingBlockData blockData = destroyableBuildingList[index];
					BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, blockData.BlockIndex);
					sbyte maxDurability = BuildingBlock.Instance[blockData.TemplateId].MaxDurability;
					int durabilityLoss = context.Random.Next((int)(maxDurability / 2), (int)(maxDurability + maxDurability / 2 + 1));
					while (durabilityLoss > 0)
					{
						bool flag3 = durabilityLoss >= (int)blockData.Durability;
						if (flag3)
						{
							durabilityLoss -= (int)blockData.Durability;
						}
						else
						{
							blockData.Durability = (sbyte)((int)blockData.Durability - durabilityLoss);
							durabilityLoss = 0;
						}
					}
					bool flag4 = blockData.TemplateId == 46;
					if (flag4)
					{
						this.RemoveExceedingResidents(context, blockKey);
					}
					this.SetElement_BuildingBlocks(new BuildingBlockKey(location.AreaId, location.BlockId, blockData.BlockIndex), blockData, context);
				}
			}
		}

		// Token: 0x06007BF4 RID: 31732 RVA: 0x00491158 File Offset: 0x0048F358
		public void GetDestroyableBuildings(Location location, List<BuildingBlockData> destroyableBuildingList)
		{
			destroyableBuildingList.Clear();
			BuildingAreaData buildingArea = this._buildingAreas[location];
			sbyte areaWidth = buildingArea.Width;
			for (short blockIndex = 0; blockIndex < (short)(buildingArea.Width * buildingArea.Width); blockIndex += 1)
			{
				BuildingBlockKey key = new BuildingBlockKey(location.AreaId, location.BlockId, blockIndex);
				BuildingBlockData blockData = this._buildingBlocks[key];
				bool flag = blockData.RootBlockIndex >= 0;
				if (!flag)
				{
					BuildingBlockItem buildingCfg = BuildingBlock.Instance[blockData.TemplateId];
					bool flag2 = !BuildingBlockData.IsBuilding(buildingCfg.Type);
					if (!flag2)
					{
						bool flag3 = buildingCfg.MaxDurability <= 0;
						if (!flag3)
						{
							bool flag4 = this.BuildingBlockLevel(key) <= 1 && blockData.Durability <= 0;
							if (flag4)
							{
								bool flag5 = buildingCfg.Type == EBuildingBlockType.MainBuilding;
								if (flag5)
								{
									destroyableBuildingList.Clear();
									break;
								}
							}
							else
							{
								destroyableBuildingList.Add(blockData);
							}
						}
					}
				}
			}
		}

		// Token: 0x06007BF5 RID: 31733 RVA: 0x00491268 File Offset: 0x0048F468
		public static bool HasBuilt(Location location, BuildingAreaData buildingArea, short buildingTemplateId, bool checkUsable = true)
		{
			for (short blockIndex = 0; blockIndex < (short)(buildingArea.Width * buildingArea.Width); blockIndex += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, blockIndex);
				BuildingBlockData buildingBlock = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = buildingBlock.TemplateId == buildingTemplateId && (!checkUsable || buildingBlock.CanUse());
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007BF6 RID: 31734 RVA: 0x004912E0 File Offset: 0x0048F4E0
		public bool CanBuild(BuildingBlockKey blockKey, short buildingTemplateId = -1)
		{
			Location location = new Location(blockKey.AreaId, blockKey.BlockId);
			bool flag = !DomainManager.TutorialChapter.InGuiding;
			if (flag)
			{
				bool flag2 = !this.GetTaiwuBuildingAreas().Contains(location);
				if (flag2)
				{
					return false;
				}
			}
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			bool flag3 = blockData.TemplateId != 0;
			bool result;
			if (flag3)
			{
				result = false;
			}
			else
			{
				BuildingAreaData buildingArea = this.GetElement_BuildingAreas(location);
				sbyte buildingWidth = BuildingBlock.Instance[buildingTemplateId].Width;
				this.IsBuildingBlocksEmpty(blockKey.AreaId, blockKey.BlockId, blockKey.BuildingBlockIndex, buildingArea.Width, buildingWidth);
				List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
				bool canBuild = true;
				bool flag4 = buildingTemplateId >= 0;
				if (flag4)
				{
					BuildingBlockItem configData = BuildingBlock.Instance[buildingTemplateId];
					canBuild = (configData.Type != EBuildingBlockType.MainBuilding);
					bool inGuiding = DomainManager.TutorialChapter.InGuiding;
					if (inGuiding)
					{
						canBuild = true;
					}
					sbyte minLevel;
					canBuild = (canBuild && (!configData.IsUnique || !BuildingDomain.HasBuilt(location, buildingArea, buildingTemplateId, true)) && this.AllDependBuildingAvailable(blockKey, buildingTemplateId, out minLevel));
					bool flag5 = canBuild && configData.Class != EBuildingBlockClass.BornResource;
					if (flag5)
					{
						ushort[] costResource = configData.BaseBuildCost;
						ResourceInts resource = this.GetAllTaiwuResources();
						for (sbyte type = 0; type < 8; type += 1)
						{
							bool flag6 = resource.Get((int)type) < (int)costResource[(int)type];
							if (flag6)
							{
								canBuild = false;
								break;
							}
						}
					}
				}
				ObjectPool<List<short>>.Instance.Return(neighborList);
				result = canBuild;
			}
			return result;
		}

		// Token: 0x06007BF7 RID: 31735 RVA: 0x0049147C File Offset: 0x0048F67C
		[Obsolete]
		public bool CanUpgrade(BuildingBlockKey blockKey, out bool dependencyIsNotMeet)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			dependencyIsNotMeet = !this.UpgradeIsMeetDependency(blockKey);
			bool flag = (((configData.Type != EBuildingBlockType.Building && configData.Type != EBuildingBlockType.MainBuilding && configData.Type != EBuildingBlockType.NormalResource) || this.BuildingBlockLevel(blockKey) >= configData.MaxLevel) | dependencyIsNotMeet) || blockData.OperationType != -1;
			return !flag;
		}

		// Token: 0x06007BF8 RID: 31736 RVA: 0x00491500 File Offset: 0x0048F700
		[Obsolete]
		[DomainMethod]
		public bool CanAutoExpand(DataContext context, BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData;
			bool flag = !this.TryGetElement_BuildingBlocks(blockKey, out blockData);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
				List<short> autoExpandList = DomainManager.Extra.GetAutoExpandBlockIndexList();
				int[] resourceCost = new int[8];
				int[] resourceChangeBeforeExpand = new int[8];
				DomainManager.Taiwu.CalcResourceChangeBeforeExpand(resourceChangeBeforeExpand);
				for (int i = 0; i < autoExpandList.Count; i++)
				{
					BuildingBlockKey currBlockKey = new BuildingBlockKey(location.AreaId, location.BlockId, autoExpandList[i]);
					BuildingBlockData currBlockData = DomainManager.Building.GetElement_BuildingBlocks(currBlockKey);
					bool isBreak = currBlockKey.Equals(blockKey);
					BuildingBlockItem configData = BuildingBlock.Instance[currBlockData.TemplateId];
					bool flag2 = configData.MaxLevel == this.BuildingBlockLevel(currBlockKey);
					if (!flag2)
					{
						bool flag3 = currBlockData.OperationType == -1;
						if (flag3)
						{
							bool flag5;
							bool flag4 = DomainManager.Building.HaveEnoughResourceToExpandBuilding(currBlockData, resourceChangeBeforeExpand, true) && DomainManager.Building.CanUpgrade(currBlockKey, out flag5);
							if (!flag4)
							{
								return false;
							}
						}
						bool flag6 = isBreak;
						if (flag6)
						{
							break;
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06007BF9 RID: 31737 RVA: 0x00491631 File Offset: 0x0048F831
		public IEnumerable<BuildingBlockData> GetBuildingBlocksAtLocation(Location location, Predicate<BuildingBlockData> condition = null)
		{
			BuildingAreaData areaData = this.GetElement_BuildingAreas(location);
			short num;
			for (short blockIndex = 0; blockIndex < (short)(areaData.Width * areaData.Width); blockIndex = num + 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, blockIndex);
				BuildingBlockData buildingBlock = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = condition == null || condition(buildingBlock);
				if (flag)
				{
					yield return buildingBlock;
				}
				blockKey = default(BuildingBlockKey);
				buildingBlock = null;
				num = blockIndex;
			}
			yield break;
		}

		// Token: 0x06007BFA RID: 31738 RVA: 0x00491650 File Offset: 0x0048F850
		public static BuildingBlockData FindBuilding(Location location, BuildingAreaData buildingArea, short buildingTemplateId, bool checkUsable = true)
		{
			for (short blockIndex = 0; blockIndex < (short)(buildingArea.Width * buildingArea.Width); blockIndex += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, blockIndex);
				BuildingBlockData buildingBlock = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = buildingBlock.TemplateId == buildingTemplateId && (!checkUsable || buildingBlock.CanUse());
				if (flag)
				{
					return buildingBlock;
				}
			}
			return null;
		}

		// Token: 0x06007BFB RID: 31739 RVA: 0x004916C8 File Offset: 0x0048F8C8
		public static BuildingBlockKey FindBuildingKey(Location location, BuildingAreaData buildingArea, short buildingTemplateId, bool checkUsable = true)
		{
			for (short blockIndex = 0; blockIndex < (short)(buildingArea.Width * buildingArea.Width); blockIndex += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, blockIndex);
				BuildingBlockData buildingBlock = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = buildingBlock.TemplateId == buildingTemplateId && (!checkUsable || buildingBlock.CanUse());
				if (flag)
				{
					return blockKey;
				}
			}
			return BuildingBlockKey.Invalid;
		}

		// Token: 0x06007BFC RID: 31740 RVA: 0x00491744 File Offset: 0x0048F944
		public List<BuildingBlockKey> FindAllBuildingsWithSameTemplate(Location location, BuildingAreaData buildingArea, short buildingTemplateId)
		{
			List<BuildingBlockKey> buildingBlockKeys = new List<BuildingBlockKey>();
			for (short blockIndex = 0; blockIndex < (short)(buildingArea.Width * buildingArea.Width); blockIndex += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, blockIndex);
				BuildingBlockData buildingBlock = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = buildingBlock.TemplateId == buildingTemplateId && buildingBlock.CanUse();
				if (flag)
				{
					buildingBlockKeys.Add(blockKey);
				}
			}
			return buildingBlockKeys;
		}

		// Token: 0x06007BFD RID: 31741 RVA: 0x004917C0 File Offset: 0x0048F9C0
		public unsafe bool AllDependBuildingAvailable(BuildingBlockKey blockKey, short buildingTemplateId, out sbyte minLevel)
		{
			List<short> dependBuildings = BuildingBlock.Instance[buildingTemplateId].DependBuildings;
			bool hasAllDependBuildings = true;
			minLevel = sbyte.MaxValue;
			bool flag = dependBuildings.Count > 0;
			if (flag)
			{
				Location location = new Location(blockKey.AreaId, blockKey.BlockId);
				BuildingAreaData areaData = this.GetElement_BuildingAreas(location);
				List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
				int count = dependBuildings.Count;
				Span<bool> span = new Span<bool>(stackalloc byte[(UIntPtr)count], count);
				Span<bool> dependBuildingFound = span;
				int count2 = dependBuildings.Count;
				Span<sbyte> span2 = new Span<sbyte>(stackalloc byte[(UIntPtr)count2], count2);
				Span<sbyte> dependBuildingLevel = span2;
				sbyte buildingWidth = BuildingBlock.Instance[this._buildingBlocks[blockKey].TemplateId].Width;
				areaData.GetNeighborBlocks(blockKey.BuildingBlockIndex, buildingWidth, neighborList, null, 2);
				for (int i = 0; i < neighborList.Count; i++)
				{
					BuildingBlockKey neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborList[i]);
					BuildingBlockData neighborBlock = this._buildingBlocks[neighborKey];
					bool flag2 = neighborBlock.RootBlockIndex >= 0;
					if (flag2)
					{
						neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborBlock.RootBlockIndex);
						neighborBlock = this._buildingBlocks[neighborKey];
					}
					bool flag3 = neighborBlock.TemplateId != 0 && neighborBlock.CanUse();
					if (flag3)
					{
						int dependIndex = dependBuildings.IndexOf(neighborBlock.TemplateId);
						bool flag4 = dependIndex >= 0;
						if (flag4)
						{
							*dependBuildingFound[dependIndex] = true;
							*dependBuildingLevel[dependIndex] = Math.Max(*dependBuildingLevel[dependIndex], this.BuildingBlockLevel(neighborKey));
						}
					}
				}
				hasAllDependBuildings = !dependBuildingFound.Contains(false);
				minLevel = dependBuildingLevel.Min<sbyte>();
				ObjectPool<List<short>>.Instance.Return(neighborList);
			}
			return hasAllDependBuildings;
		}

		// Token: 0x06007BFE RID: 31742 RVA: 0x004919A8 File Offset: 0x0048FBA8
		[DomainMethod]
		public bool AllDependBuildingAvailable(BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData;
			bool flag = this.TryGetElement_BuildingBlocks(blockKey, out blockData);
			if (flag)
			{
				bool flag2 = blockData != null && blockData.ConfigData != null;
				if (flag2)
				{
					sbyte b;
					return this.AllDependBuildingAvailable(blockKey, blockData.TemplateId, out b);
				}
			}
			return false;
		}

		// Token: 0x06007BFF RID: 31743 RVA: 0x004919F4 File Offset: 0x0048FBF4
		public void SetBuildingOperator(DataContext context, BuildingBlockKey blockKey, int index, int charId)
		{
			bool flag = !this._buildingOperatorDict.ContainsKey(blockKey);
			CharacterList charList;
			if (flag)
			{
				charList = default(CharacterList);
				for (int i = 0; i < 3; i++)
				{
					charList.Add(-1);
				}
				this.AddElement_BuildingOperatorDict(blockKey, charList, context);
			}
			else
			{
				charList = this._buildingOperatorDict[blockKey];
			}
			charList.GetCollection()[index] = charId;
			this.SetElement_BuildingOperatorDict(blockKey, charList, context);
		}

		// Token: 0x06007C00 RID: 31744 RVA: 0x00491A70 File Offset: 0x0048FC70
		public void SetShopBuildingManager(DataContext context, BuildingBlockKey blockKey, int index, int charId, bool setArtisanOrder = true)
		{
			bool flag = !this._shopManagerDict.ContainsKey(blockKey);
			CharacterList charList;
			if (flag)
			{
				charList = default(CharacterList);
				for (int i = 0; i < 7; i++)
				{
					charList.Add(-1);
				}
				this.AddElement_ShopManagerDict(blockKey, charList, context);
			}
			else
			{
				charList = this._shopManagerDict[blockKey];
			}
			charList.GetCollection()[index] = charId;
			BuildingBlockData blockData;
			bool flag2 = setArtisanOrder && this.TryGetElement_BuildingBlocks(blockKey, out blockData) && BuildingBlock.Instance[blockData.TemplateId].ArtisanOrderAvailable;
			if (flag2)
			{
				DomainManager.Extra.SetBuildingOrderArtisan(context, blockKey, charList.GetCollection()[0]);
			}
			this.SetElement_ShopManagerDict(blockKey, charList, context);
		}

		// Token: 0x06007C01 RID: 31745 RVA: 0x00491B34 File Offset: 0x0048FD34
		public bool IsShopManager(int charId)
		{
			VillagerWorkData workData;
			return DomainManager.Taiwu.TryGetElement_VillagerWork(charId, out workData) && workData.WorkType == 1;
		}

		// Token: 0x06007C02 RID: 31746 RVA: 0x00491B64 File Offset: 0x0048FD64
		public sbyte GetCollectBuildingResourceType(BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			bool flag = !configData.IsCollectResourceBuilding;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Building ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(blockData.TemplateId);
				defaultInterpolatedStringHandler.AppendLiteral(" is not a collect resource building");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag2 = this._CollectBuildingResourceType.ContainsKey(blockKey);
			if (!flag2)
			{
				BuildingBlockItem resourceConfig = BuildingBlock.Instance[configData.DependBuildings[0]];
				for (int i = 0; i < resourceConfig.CollectResourcePercent.Length; i++)
				{
					bool flag3 = resourceConfig.CollectResourcePercent[i] > 0;
					if (flag3)
					{
						return (sbyte)i;
					}
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Building ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(blockData.TemplateId);
				defaultInterpolatedStringHandler.AppendLiteral(" has no collectable resource type");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this._CollectBuildingResourceType[blockKey];
		}

		// Token: 0x06007C03 RID: 31747 RVA: 0x00491C8C File Offset: 0x0048FE8C
		public sbyte GetLifeSkillByResourceType(sbyte resourceType)
		{
			return (resourceType < 6) ? Config.ResourceType.Instance[resourceType].LifeSkillType : 9;
		}

		// Token: 0x06007C04 RID: 31748 RVA: 0x00491CB8 File Offset: 0x0048FEB8
		[DomainMethod]
		public BuildingFormulaContextBridge GetBuildingFormulaContextBridge(BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData = this.GetBuildingBlockData(blockKey);
			this._formulaContextBridge.Initialize(blockKey, blockData.ConfigData, this._formulaArgHandler, true);
			return this._formulaContextBridge;
		}

		// Token: 0x06007C05 RID: 31749 RVA: 0x00491CF4 File Offset: 0x0048FEF4
		[Obsolete]
		[DomainMethod]
		public int GetAttainmentOfBuilding(BuildingBlockKey blockKey, bool isAverage = false)
		{
			bool hasManager;
			int value = isAverage ? this.BuildingTotalAverageAttainment(blockKey, -1, out hasManager, false) : this.BuildingTotalAttainment(blockKey, -1, out hasManager, false);
			return hasManager ? value : 0;
		}

		// Token: 0x06007C06 RID: 31750 RVA: 0x00491D2C File Offset: 0x0048FF2C
		[Obsolete]
		public int GetAttainmentOfBuildingWhetherCanWork(BuildingBlockKey blockKey, bool isAverage = false)
		{
			int resourceAttainment = 0;
			sbyte resourceType = this.GetCollectBuildingResourceType(blockKey);
			CharacterList managerList;
			DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out managerList);
			for (int i = 0; i < managerList.GetCount(); i++)
			{
				int charId = managerList.GetCollection()[i];
				bool flag = charId >= 0;
				if (flag)
				{
					GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
					resourceAttainment += (int)this.BaseWorkContribution;
					resourceAttainment += (int)manageChar.GetLifeSkillAttainment(this.GetLifeSkillByResourceType(resourceType));
				}
			}
			return isAverage ? (resourceAttainment / managerList.GetCount()) : resourceAttainment;
		}

		// Token: 0x06007C07 RID: 31751 RVA: 0x00491DC8 File Offset: 0x0048FFC8
		[Obsolete]
		public int GetAttainmentOfBuilding(BuildingBlockKey blockKey, sbyte resourceType, bool isAverage = false)
		{
			int resourceAttainment = 0;
			CharacterList managerList;
			DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out managerList);
			for (int i = 0; i < managerList.GetCount(); i++)
			{
				int charId = managerList.GetCollection()[i];
				bool flag = charId >= 0 && DomainManager.Taiwu.CanWork(charId);
				if (flag)
				{
					GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
					resourceAttainment += (int)this.BaseWorkContribution;
					resourceAttainment += (int)manageChar.GetLifeSkillAttainment(this.GetLifeSkillByResourceType(resourceType));
				}
			}
			return isAverage ? (resourceAttainment / managerList.GetCount()) : resourceAttainment;
		}

		// Token: 0x06007C08 RID: 31752 RVA: 0x00491E64 File Offset: 0x00490064
		[Obsolete]
		public int GetShopBuildingMaxAttainment(BuildingBlockKey blockKey, bool isAverage = false)
		{
			int resourceAttainment = 0;
			CharacterList managerList;
			DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out managerList);
			for (int i = 0; i < managerList.GetCount(); i++)
			{
				int charId = managerList.GetCollection()[i];
				bool flag = charId >= 0 && DomainManager.Taiwu.CanWork(charId);
				if (flag)
				{
					GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
					resourceAttainment += (int)this.BaseWorkContribution;
					resourceAttainment += (int)manageChar.GetMaxCombatSkillAttainment();
				}
			}
			return isAverage ? (resourceAttainment / managerList.GetCount()) : resourceAttainment;
		}

		// Token: 0x06007C09 RID: 31753 RVA: 0x00491EFC File Offset: 0x004900FC
		public int GetShopBuildingMaxAttainmentWhetherCanWork(BuildingBlockKey blockKey, bool isAverage = false)
		{
			int resourceAttainment = 0;
			CharacterList managerList;
			DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out managerList);
			for (int i = 0; i < managerList.GetCount(); i++)
			{
				int charId = managerList.GetCollection()[i];
				bool flag = charId >= 0;
				if (flag)
				{
					GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
					resourceAttainment += (int)this.BaseWorkContribution;
					resourceAttainment += (int)manageChar.GetMaxCombatSkillAttainment();
				}
			}
			return isAverage ? (resourceAttainment / managerList.GetCount()) : resourceAttainment;
		}

		// Token: 0x06007C0A RID: 31754 RVA: 0x00491F88 File Offset: 0x00490188
		[Obsolete]
		public int GetShopBuildingAttainment(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
		{
			int resourceAttainment = 0;
			CharacterList managerList;
			DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out managerList);
			for (int i = 0; i < managerList.GetCount(); i++)
			{
				int charId = managerList.GetCollection()[i];
				bool flag = charId >= 0 && DomainManager.Taiwu.CanWork(charId);
				if (flag)
				{
					GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
					resourceAttainment += (int)this.BaseWorkContribution;
					resourceAttainment += (int)manageChar.GetLifeSkillAttainment(BuildingBlock.Instance[blockData.TemplateId].RequireLifeSkillType);
				}
			}
			return isAverage ? (resourceAttainment / managerList.GetCount()) : resourceAttainment;
		}

		// Token: 0x06007C0B RID: 31755 RVA: 0x00492034 File Offset: 0x00490234
		public int GetSpecialBuildingAttainment(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
		{
			int resourceAttainment = 0;
			CharacterList managerList;
			DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out managerList);
			for (int i = 0; i < managerList.GetCount(); i++)
			{
				int charId = managerList.GetCollection()[i];
				bool flag = charId >= 0 && DomainManager.Taiwu.CanWork(charId);
				if (flag)
				{
					GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
					resourceAttainment += (int)this.BaseWorkContribution;
					resourceAttainment += (int)manageChar.GetLifeSkillAttainment(BuildingBlock.Instance[blockData.TemplateId].RequireLifeSkillType);
				}
			}
			return isAverage ? (resourceAttainment / managerList.GetCount()) : resourceAttainment;
		}

		// Token: 0x06007C0C RID: 31756 RVA: 0x004920E0 File Offset: 0x004902E0
		public int GetShopBuildingAttainmentWhetherCanWork(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
		{
			int resourceAttainment = 0;
			CharacterList managerList;
			DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out managerList);
			for (int i = 0; i < managerList.GetCount(); i++)
			{
				int charId = managerList.GetCollection()[i];
				bool flag = charId >= 0;
				if (flag)
				{
					GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
					resourceAttainment += (int)this.BaseWorkContribution;
					resourceAttainment += (int)manageChar.GetLifeSkillAttainment(BuildingBlock.Instance[blockData.TemplateId].RequireLifeSkillType);
				}
			}
			return isAverage ? (resourceAttainment / managerList.GetCount()) : resourceAttainment;
		}

		// Token: 0x06007C0D RID: 31757 RVA: 0x00492180 File Offset: 0x00490380
		[DomainMethod]
		public int GetBuildingAttainment(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
		{
			bool hasManager;
			int value = isAverage ? this.BuildingTotalAverageAttainment(blockKey, -1, out hasManager, false) : (this.UseMaxAttainment(blockData) ? this.BuildingMaxAttainment(blockKey, -1, out hasManager) : this.BuildingTotalAttainment(blockKey, -1, out hasManager, false));
			return hasManager ? value : 0;
		}

		// Token: 0x06007C0E RID: 31758 RVA: 0x004921CC File Offset: 0x004903CC
		private bool UseMaxAttainment(BuildingBlockData blockData)
		{
			BuildingBlockItem configData = blockData.ConfigData;
			bool flag;
			if (configData != null)
			{
				List<short> expandInfos = configData.ExpandInfos;
				flag = (expandInfos != null && expandInfos.Count > 0);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				foreach (short scaleId in configData.ExpandInfos)
				{
					int formulaId = BuildingScale.Instance[scaleId].Formula;
					bool flag3 = formulaId < 0;
					if (!flag3)
					{
						BuildingFormulaItem formula = BuildingFormula.Instance[formulaId];
						EBuildingFormulaArgType[] arguments = formula.Arguments;
						bool flag4 = arguments != null && arguments.Length > 0;
						if (flag4)
						{
							foreach (EBuildingFormulaArgType arg in formula.Arguments)
							{
								bool flag5 = arg == EBuildingFormulaArgType.MaxAttainment;
								if (flag5)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06007C0F RID: 31759 RVA: 0x004922D8 File Offset: 0x004904D8
		[Obsolete]
		public int GetBuildingAttainmentWhetherCanWork(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
		{
			BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
			bool flag = this.IsDependKungfuPracticeRoom(config);
			int result;
			if (flag)
			{
				result = this.GetShopBuildingMaxAttainmentWhetherCanWork(blockKey, isAverage);
			}
			else
			{
				result = this.GetShopBuildingAttainmentWhetherCanWork(blockData, blockKey, isAverage);
			}
			return result;
		}

		// Token: 0x06007C10 RID: 31760 RVA: 0x0049231C File Offset: 0x0049051C
		[DomainMethod]
		public int[] GetBuildingShopManagerAutoArrangeSorted(BuildingBlockKey blockKey, int[] managerCharacterIds)
		{
			bool flag = managerCharacterIds == null;
			int[] result2;
			if (flag)
			{
				result2 = Array.Empty<int>();
			}
			else
			{
				Dictionary<int, ValueTuple<int, int, int>> values = new Dictionary<int, ValueTuple<int, int, int>>();
				BuildingBlockData blockData;
				bool flag2 = this.TryGetElement_BuildingBlocks(blockKey, out blockData);
				if (flag2)
				{
					BuildingBlockItem blockConfig = BuildingBlock.Instance.GetItem(blockData.TemplateId);
					bool flag3 = blockConfig != null;
					if (flag3)
					{
						sbyte lifeSkillType = this.GetNeedLifeSkillType(blockConfig, blockKey);
						foreach (int managerCharId in managerCharacterIds)
						{
							GameData.Domains.Character.Character managerChar;
							bool flag4 = DomainManager.Character.TryGetElement_Objects(managerCharId, out managerChar);
							if (flag4)
							{
								short partAttainment = managerChar.GetLifeSkillAttainment(lifeSkillType);
								int partSuccessRate = this.BuildingManageHarvestSuccessRate(blockKey, managerCharId) * 2;
								int partSpecialRate = this.BuildingManageHarvestSpecialSuccessRate(blockKey, managerCharId) / 2;
								ValueTuple<int, int, int> value = new ValueTuple<int, int, int>((int)partAttainment, partSuccessRate, partSpecialRate);
								values[managerCharId] = value;
							}
						}
					}
				}
				int[] result = managerCharacterIds.ToArray<int>();
				Array.Sort<int>(result, delegate(int charIdA, int charIdB)
				{
					ValueTuple<int, int, int> valuesA;
					values.TryGetValue(charIdA, out valuesA);
					ValueTuple<int, int, int> valuesB;
					values.TryGetValue(charIdB, out valuesB);
					int result3 = (valuesB.Item1 + valuesB.Item2 + valuesB.Item3).CompareTo(valuesA.Item1 + valuesA.Item2 + valuesA.Item3);
					bool flag5 = result3 == 0;
					if (flag5)
					{
						result3 = charIdA.CompareTo(charIdB);
					}
					return result3;
				});
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06007C11 RID: 31761 RVA: 0x0049242C File Offset: 0x0049062C
		public bool IsDependKungfuPracticeRoom(BuildingBlockItem config)
		{
			return config.DependBuildings.Count > 0 && config.DependBuildings[0] == 52 && config.IsShop;
		}

		// Token: 0x06007C12 RID: 31762 RVA: 0x00492468 File Offset: 0x00490668
		public int GetBuildingAttainmentUniversalWhetherCanWork(BuildingBlockData blockData, BuildingBlockKey blockKey, bool isAverage = false)
		{
			bool flag;
			return this.BuildingTotalAttainment(blockKey, -1, out flag, true);
		}

		// Token: 0x06007C13 RID: 31763 RVA: 0x00492480 File Offset: 0x00490680
		[Obsolete]
		public short GetLevelByDistance(short level, int distance)
		{
			bool flag = distance <= 0;
			short result;
			if (flag)
			{
				result = level;
			}
			else
			{
				result = (short)((double)level * (1.0 / (double)distance));
			}
			return result;
		}

		// Token: 0x06007C14 RID: 31764 RVA: 0x004924B4 File Offset: 0x004906B4
		[Obsolete]
		public double GetNumByDistance(double num, int distance)
		{
			bool flag = distance <= 0;
			double result;
			if (flag)
			{
				result = num;
			}
			else
			{
				result = num * (1.0 / (double)distance);
			}
			return result;
		}

		// Token: 0x06007C15 RID: 31765 RVA: 0x004924E4 File Offset: 0x004906E4
		public int CalcResourceChangeFactor(BuildingBlockData blockData)
		{
			BuildingBlockItem dependConfig = BuildingBlock.Instance[BuildingBlock.Instance[blockData.TemplateId].DependBuildings[0]];
			int factor = 1;
			for (int i = 0; i < dependConfig.CollectResourcePercent.Length; i++)
			{
				bool flag = dependConfig.CollectResourcePercent[i] != 0;
				if (flag)
				{
					factor = (int)dependConfig.CollectResourcePercent[i];
					break;
				}
			}
			return factor;
		}

		// Token: 0x06007C16 RID: 31766 RVA: 0x00492557 File Offset: 0x00490757
		private void BuildingRemoveVillagerWork(DataContext context, BuildingBlockKey blockKey)
		{
			this.RemoveAllOperatorsInBuilding(context, blockKey);
			this.RemoveAllManagersInBuilding(context, blockKey);
		}

		// Token: 0x06007C17 RID: 31767 RVA: 0x0049256C File Offset: 0x0049076C
		private void BuildingRemoveResident(DataContext context, BuildingBlockData blockData, BuildingBlockKey blockKey)
		{
			bool flag = blockData.TemplateId == 46;
			if (flag)
			{
				this.RemoveResidence(context, blockKey);
			}
			else
			{
				bool flag2 = blockData.TemplateId == 47;
				if (flag2)
				{
					this.RemoveComfortableHouse(context, blockKey);
				}
			}
		}

		// Token: 0x06007C18 RID: 31768 RVA: 0x004925AC File Offset: 0x004907AC
		public bool IsHaveSpecifyBuilding(short templateId, GameData.Domains.Character.Character character)
		{
			foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> pair in this._buildingBlocks)
			{
				bool flag = pair.Value.TemplateId == templateId && pair.Value.CanUse() && character.GetLocation().AreaId == pair.Key.AreaId && character.GetLocation().BlockId == pair.Key.BlockId;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C19 RID: 31769 RVA: 0x00492660 File Offset: 0x00490860
		public ValueTuple<BuildingBlockKey, BuildingBlockData> GetSpecifyBuildingBlockData(short templateId)
		{
			foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> pair in this._buildingBlocks)
			{
				bool flag = pair.Value.TemplateId == templateId && pair.Value.CanUse();
				if (flag)
				{
					return new ValueTuple<BuildingBlockKey, BuildingBlockData>(pair.Key, pair.Value);
				}
			}
			return new ValueTuple<BuildingBlockKey, BuildingBlockData>(BuildingBlockKey.Invalid, null);
		}

		// Token: 0x06007C1A RID: 31770 RVA: 0x004926FC File Offset: 0x004908FC
		public unsafe int CreateCharacterByRecruitCharacterData(DataContext context, RecruitCharacterData recruitCharacterData)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			recruitCharacterData.Recalculate();
			OrganizationInfo orgInfo = taiwuChar.GetOrganizationInfo();
			orgInfo.Grade = 0;
			IntelligentCharacterCreationInfo info = new IntelligentCharacterCreationInfo(location, orgInfo, recruitCharacterData.TemplateId)
			{
				Age = recruitCharacterData.Age,
				BirthMonth = recruitCharacterData.BirthMonth,
				Gender = recruitCharacterData.Gender,
				Transgender = recruitCharacterData.Transgender,
				BaseAttraction = recruitCharacterData.BaseAttraction,
				Avatar = recruitCharacterData.AvatarData,
				CombatSkillQualificationGrowthType = recruitCharacterData.CombatSkillQualificationGrowthType,
				LifeSkillQualificationGrowthType = recruitCharacterData.LifeSkillQualificationGrowthType,
				InitializeSectSkills = false,
				AllowRandomGrowingGradeAdjust = false,
				GrowingSectGrade = recruitCharacterData.PeopleLevel,
				DisableBeReincarnatedBySavedSoul = true
			};
			GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref info);
			int charId = character.GetId();
			DomainManager.Character.CompleteCreatingCharacter(charId);
			character.SetBaseMorality(recruitCharacterData.GetBaseMorality(), context);
			character.SetFullName(recruitCharacterData.FullName, context);
			character.SetFeatureIds(recruitCharacterData.FeatureIds.ToList<short>(), context);
			short clothingTemplateId = recruitCharacterData.ClothingTemplateId;
			bool flag = clothingTemplateId < 0;
			if (!flag)
			{
				bool flag2 = character.GetEquipment()[4].TemplateId == clothingTemplateId;
				if (!flag2)
				{
					ItemKey itemKey = DomainManager.Item.CreateItem(context, 3, clothingTemplateId);
					character.AddInventoryItem(context, itemKey, 1, false);
					character.ChangeEquipment(context, -1, 4, itemKey);
				}
			}
			DomainManager.Extra.SetCharTeammateCommands(context, charId, new SByteList(recruitCharacterData.TeammateCommands));
			MainAttributes origin = recruitCharacterData.MainAttributes;
			MainAttributes basic = character.GetBaseMainAttributes();
			MainAttributes result = character.GetMaxMainAttributes();
			for (int i = 0; i < 6; i++)
			{
				int delta = (int)(*(ref origin.Items.FixedElementField + (IntPtr)i * 2) - *(ref result.Items.FixedElementField + (IntPtr)i * 2));
				ref short ptr = ref basic.Items.FixedElementField + (IntPtr)i * 2;
				ptr += (short)delta;
				*(ref basic.Items.FixedElementField + (IntPtr)i * 2) = Math.Max(*(ref basic.Items.FixedElementField + (IntPtr)i * 2), 0);
			}
			character.SetBaseMainAttributes(basic, context);
			character.SetCurrMainAttributes(character.GetMaxMainAttributes(), context);
			CombatSkillShorts origin2 = recruitCharacterData.CombatSkillQualifications;
			CombatSkillShorts basic2 = *character.GetBaseCombatSkillQualifications();
			CombatSkillShorts result2 = *character.GetCombatSkillQualifications();
			for (int j = 0; j < 14; j++)
			{
				int delta2 = (int)(*(ref origin2.Items.FixedElementField + (IntPtr)j * 2) - *(ref result2.Items.FixedElementField + (IntPtr)j * 2));
				ref short ptr2 = ref basic2.Items.FixedElementField + (IntPtr)j * 2;
				ptr2 += (short)delta2;
				*(ref basic2.Items.FixedElementField + (IntPtr)j * 2) = Math.Max(*(ref basic2.Items.FixedElementField + (IntPtr)j * 2), 0);
			}
			character.SetBaseCombatSkillQualifications(ref basic2, context);
			LifeSkillShorts origin3 = recruitCharacterData.LifeSkillQualifications;
			LifeSkillShorts basic3 = *character.GetBaseLifeSkillQualifications();
			LifeSkillShorts result3 = *character.GetLifeSkillQualifications();
			for (int k = 0; k < 16; k++)
			{
				int delta3 = (int)(*(ref origin3.Items.FixedElementField + (IntPtr)k * 2) - *(ref result3.Items.FixedElementField + (IntPtr)k * 2));
				ref short ptr3 = ref basic3.Items.FixedElementField + (IntPtr)k * 2;
				ptr3 += (short)delta3;
				*(ref basic3.Items.FixedElementField + (IntPtr)k * 2) = Math.Max(*(ref basic3.Items.FixedElementField + (IntPtr)k * 2), 0);
			}
			character.SetBaseLifeSkillQualifications(ref basic3, context);
			return charId;
		}

		// Token: 0x06007C1B RID: 31771 RVA: 0x00492AEA File Offset: 0x00490CEA
		public void InitBuildingEffect()
		{
			this.InitAllAreaBuildingBlockEffectsCache();
		}

		// Token: 0x06007C1C RID: 31772 RVA: 0x00492AF4 File Offset: 0x00490CF4
		private void UpdateTaiwuVillageBuildingEffect()
		{
			foreach (Location location in this._taiwuBuildingAreas)
			{
				this.UpdateLocationBuildingBlockEffectsCache(location);
			}
			this.TriggerCacheUpdate();
			this._needUpdateEffects = false;
		}

		// Token: 0x06007C1D RID: 31773 RVA: 0x00492B5C File Offset: 0x00490D5C
		private void TriggerCacheUpdate()
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			List<short> settlementBlocks = ObjectPool<List<short>>.Instance.Get();
			DomainManager.Map.GetSettlementBlocksAndAffiliatedBlocks(taiwuVillageLocation.AreaId, taiwuVillageLocation.BlockId, settlementBlocks);
			DataContext context = DataContextManager.GetCurrentThreadDataContext();
			for (int i = 0; i < settlementBlocks.Count; i++)
			{
				MapBlockData blockData = DomainManager.Map.GetBlockData(taiwuVillageLocation.AreaId, settlementBlocks[i]);
				bool flag = blockData.CharacterSet != null;
				if (flag)
				{
					foreach (int charId in blockData.CharacterSet)
					{
						GameData.Domains.Character.Character character;
						bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (flag2)
						{
							character.SetLocation(character.GetLocation(), context);
						}
					}
				}
				bool flag3 = blockData.InfectedCharacterSet != null;
				if (flag3)
				{
					foreach (int charId2 in blockData.InfectedCharacterSet)
					{
						GameData.Domains.Character.Character character2;
						bool flag4 = DomainManager.Character.TryGetElement_Objects(charId2, out character2);
						if (flag4)
						{
							character2.SetLocation(character2.GetLocation(), context);
						}
					}
				}
			}
			HashSet<int> taiwuGroupCharIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId3 in taiwuGroupCharIds)
			{
				GameData.Domains.Character.Character character3;
				bool flag5 = DomainManager.Character.TryGetElement_Objects(charId3, out character3);
				if (flag5)
				{
					character3.SetLocation(character3.GetLocation(), context);
				}
			}
			ObjectPool<List<short>>.Instance.Return(settlementBlocks);
		}

		// Token: 0x06007C1E RID: 31774 RVA: 0x00492D50 File Offset: 0x00490F50
		public override void FixAbnormalDomainArchiveData(DataContext context)
		{
			this.FixBuildingEarningsData(context);
			this.FixVillageResidentData(context);
			this.FixVillageStatusData(context);
			this.FixBuildingBlockData(context);
			this.FixBuildingCollectResourceTypeData(context);
			this.FixStoneRoomData(context);
			this.FixTeaHouseCaravan(context);
			this.FixBuildingLegacyData(context);
			this.FixChickenKing(context);
			this.FixSoldBuildingItemValueZero();
			this.FixObsoleteBuilding(context);
			this.FixBuildingOperateData(context);
			this.FixObsoleteChickenTask(context);
			this.BuildingVersionUpdateFix(context);
			this.FixBuildingBlockDataEx(context);
			this.FixFeastSetAutoRefill(context);
		}

		// Token: 0x06007C1F RID: 31775 RVA: 0x00492DE0 File Offset: 0x00490FE0
		private void FixFeastSetAutoRefill(DataContext context)
		{
			bool flag = !DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 79, 19);
			if (!flag)
			{
				BuildingDomain buildingDomain = DomainManager.Building;
				foreach (Location location in buildingDomain.GetTaiwuBuildingAreas())
				{
					BuildingAreaData areaData = buildingDomain.GetBuildingAreaData(location);
					short index = 0;
					short len = (short)(areaData.Width * areaData.Width);
					while (index < len)
					{
						BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
						BuildingBlockData blockData;
						bool flag2 = buildingDomain.TryGetElement_BuildingBlocks(blockKey, out blockData);
						if (flag2)
						{
							bool flag3 = blockData.TemplateId == 47;
							if (flag3)
							{
								DomainManager.Extra.FeastSetAutoRefill(context, blockKey, true);
								Logger logger = BuildingDomain.Logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
								defaultInterpolatedStringHandler.AppendLiteral("FixFeastSetAutoRefill true ,blockKey.BuildingBlockIndex is : ");
								defaultInterpolatedStringHandler.AppendFormatted<short>(blockKey.BuildingBlockIndex);
								logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							}
						}
						index += 1;
					}
				}
			}
		}

		// Token: 0x06007C20 RID: 31776 RVA: 0x00492F18 File Offset: 0x00491118
		private void FixBuildingBlockDataEx(DataContext context)
		{
			bool flag = !DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 78, 22);
			if (!flag)
			{
				BuildingDomain buildingDomain = DomainManager.Building;
				foreach (Location location in buildingDomain.GetTaiwuBuildingAreas())
				{
					BuildingAreaData areaData = buildingDomain.GetBuildingAreaData(location);
					short index = 0;
					short len = (short)(areaData.Width * areaData.Width);
					while (index < len)
					{
						BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
						BuildingBlockData blockData;
						bool flag2 = buildingDomain.TryGetElement_BuildingBlocks(blockKey, out blockData);
						if (flag2)
						{
							bool flag3 = blockData.TemplateId <= 0;
							if (flag3)
							{
								DomainManager.Extra.ResetBuildingExtraData(context, blockKey);
								Logger logger = BuildingDomain.Logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
								defaultInterpolatedStringHandler.AppendLiteral("FixBuildingBlockDataEx true ,blockKey.BuildingBlockIndex is : ");
								defaultInterpolatedStringHandler.AppendFormatted<short>(blockKey.BuildingBlockIndex);
								logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							}
						}
						index += 1;
					}
				}
			}
		}

		// Token: 0x06007C21 RID: 31777 RVA: 0x00493050 File Offset: 0x00491250
		private void BuildingVersionUpdateFix(DataContext context)
		{
			bool flag = !DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 78, 0);
			if (!flag)
			{
				DomainManager.Extra.WipeAllVillagerRoleData(context);
				BuildingDomain buildingDomain = DomainManager.Building;
				foreach (Location location in buildingDomain.GetTaiwuBuildingAreas())
				{
					BuildingAreaData areaData = buildingDomain.GetBuildingAreaData(location);
					short index = 0;
					short len = (short)(areaData.Width * areaData.Width);
					while (index < len)
					{
						BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
						BuildingBlockData blockData;
						bool flag2 = buildingDomain.TryGetElement_BuildingBlocks(blockKey, out blockData);
						if (flag2)
						{
							BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
							bool flag3 = config != null && config.IsShop;
							if (flag3)
							{
								DomainManager.Building.SetBuildingAutoWork(context, blockKey.BuildingBlockIndex, true);
								Logger logger = BuildingDomain.Logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 1);
								defaultInterpolatedStringHandler.AppendLiteral("SetBuildingAutoWork true ,blockKey.BuildingBlockIndex is : ");
								defaultInterpolatedStringHandler.AppendFormatted<short>(blockKey.BuildingBlockIndex);
								logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							bool needSet = false;
							bool flag4 = config != null;
							if (flag4)
							{
								bool flag5 = blockData.Durability > config.MaxDurability;
								if (flag5)
								{
									blockData.Durability = config.MaxDurability;
									needSet = true;
								}
								bool flag6 = blockData.OperationType == 2 || blockData.OperationType == 1 || blockData.OperationType == 3;
								if (flag6)
								{
									blockData.OperationType = -1;
									blockData.OperationProgress = 0;
									needSet = true;
								}
								bool flag7 = blockData.OperationType == 0;
								if (flag7)
								{
									this.CompleteBuilding(context, blockKey, blockData);
									blockData.OperationType = -1;
									blockData.OperationProgress = 0;
									needSet = true;
								}
							}
							bool flag8 = needSet;
							if (flag8)
							{
								this.RemoveAllOperatorsInBuilding(context, blockKey);
								DomainManager.Building.SetElement_BuildingBlocks(blockKey, blockData, context);
							}
						}
						index += 1;
					}
				}
				foreach (CharacterList list in this._shopManagerDict.Values.ToArray<CharacterList>())
				{
					foreach (int charId in list.GetCollection().ToArray())
					{
						DomainManager.Taiwu.RemoveVillagerWork(context, charId, true);
					}
				}
				this.ClearShopManagerDict(context);
				TaiwuDomain taiwuDomain = DomainManager.Taiwu;
				foreach (int charId2 in taiwuDomain.GetVillagerWorkDict().Keys.ToArray<int>())
				{
					VillagerWorkData workData;
					bool flag9 = taiwuDomain.TryGetElement_VillagerWork(charId2, out workData) && workData.BuildingBlockIndex >= 0;
					if (flag9)
					{
						taiwuDomain.RemoveVillagerWork(context, charId2, true);
					}
				}
			}
		}

		// Token: 0x06007C22 RID: 31778 RVA: 0x00493364 File Offset: 0x00491564
		public void FixResidentAndComfortableHouseCount(DataContext context)
		{
			BuildingDomain buildingDomain = DomainManager.Building;
			foreach (Location location in buildingDomain.GetTaiwuBuildingAreas())
			{
				BuildingAreaData areaData = buildingDomain.GetBuildingAreaData(location);
				short index = 0;
				short len = (short)(areaData.Width * areaData.Width);
				while (index < len)
				{
					BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
					BuildingBlockData blockData;
					bool flag = !buildingDomain.TryGetElement_BuildingBlocks(blockKey, out blockData);
					if (!flag)
					{
						BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
						bool flag2 = config == null;
						if (!flag2)
						{
							BuildingBlockDataEx dataEx = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)blockKey);
							short templateId = config.TemplateId;
							short num = templateId;
							CharacterList residenceChars;
							if (num != 46)
							{
								if (num == 47)
								{
									CharacterList comfortableHouseChars;
									if (this._comfortableHouses.TryGetValue(blockKey, out comfortableHouseChars))
									{
										int maxCapacity = BuildingScale.DefValue.ComfortableHouseCapacity.GetLevelEffect((int)dataEx.CalcUnlockedLevelCount());
										int currentCount = comfortableHouseChars.GetCount();
										for (int i = currentCount - 1; i >= maxCapacity; i--)
										{
											int charId = comfortableHouseChars[i];
											this.RemoveFromComfortableHouse(context, charId, this._buildingComfortableHouses[charId]);
										}
									}
								}
							}
							else if (this._residences.TryGetValue(blockKey, out residenceChars))
							{
								int maxCapacity2 = BuildingScale.DefValue.ResidenceCapacity.GetLevelEffect((int)dataEx.CalcUnlockedLevelCount());
								int currentCount2 = residenceChars.GetCount();
								for (int j = currentCount2 - 1; j >= maxCapacity2; j--)
								{
									int charId2 = residenceChars[j];
									this.RemoveFromResidence(context, charId2, this._buildingResidents[charId2]);
								}
							}
						}
					}
					index += 1;
				}
			}
		}

		// Token: 0x06007C23 RID: 31779 RVA: 0x00493574 File Offset: 0x00491774
		private void FixObsoleteChickenTask(DataContext context)
		{
			bool flag = this.AllChickenInTaiwuVillage(context);
			if (flag)
			{
				DomainManager.Extra.FinishTriggeredExtraTask(context, 53, 342);
			}
		}

		// Token: 0x06007C24 RID: 31780 RVA: 0x004935A4 File Offset: 0x004917A4
		private unsafe void FixObsoleteBuilding(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData buildingArea = this.GetBuildingAreaData(location);
			List<short> removedObsoleteBuildingTemplateIds = new List<short>();
			Span<short> span = new Span<short>(stackalloc byte[(UIntPtr)4], 2);
			SpanList<short> templateIds = span;
			templateIds.Add(281);
			templateIds.Add(282);
			foreach (short ptr in templateIds)
			{
				short templateId = ptr;
				BuildingBlockData building = BuildingDomain.FindBuilding(location, buildingArea, templateId, false);
				bool flag = building == null;
				if (!flag)
				{
					BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, building.BlockIndex);
					this.BuildingRemoveVillagerWork(context, blockKey);
					ushort[] requireResource = BuildingBlock.Instance[templateId].BaseBuildCost;
					GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
					for (sbyte type = 0; type < 8; type += 1)
					{
						bool flag2 = requireResource[(int)type] > 0;
						if (flag2)
						{
							taiwu.ChangeResource(context, type, (int)requireResource[(int)type]);
						}
					}
					removedObsoleteBuildingTemplateIds.Add(templateId);
					this.ResetAllChildrenBlocks(context, blockKey, 0, -1);
					this.UpdateTaiwuVillageBuildingEffect();
				}
			}
			bool flag3 = removedObsoleteBuildingTemplateIds.Count != 0;
			if (flag3)
			{
				GameDataBridge.AddDisplayEvent<List<short>>(DisplayEventType.ProfessionObsoleteBuildingRemoved, removedObsoleteBuildingTemplateIds);
			}
		}

		// Token: 0x06007C25 RID: 31781 RVA: 0x004936E8 File Offset: 0x004918E8
		private void FixBuildingOperateData(DataContext context)
		{
			Dictionary<int, VillagerWorkData> villageWork = DomainManager.Taiwu.GetVillagerWorkDict();
			List<int> chaIdList = ObjectPool<List<int>>.Instance.Get();
			chaIdList.Clear();
			foreach (int charId in villageWork.Keys)
			{
				GameData.Domains.Character.Character character;
				bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag)
				{
					bool flag2 = character.GetOrganizationInfo().OrgTemplateId != 16;
					if (flag2)
					{
						chaIdList.Add(charId);
						Logger logger = BuildingDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 1);
						defaultInterpolatedStringHandler.AppendLiteral("FixBuildingOperateData ,remove charId: ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
						defaultInterpolatedStringHandler.AppendLiteral(",because is not belong taiwu");
						logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				else
				{
					chaIdList.Add(charId);
					Logger logger2 = BuildingDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
					defaultInterpolatedStringHandler.AppendLiteral("FixBuildingOperateData ,remove charId: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
					defaultInterpolatedStringHandler.AppendLiteral(",because dead");
					logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			foreach (int charId2 in chaIdList)
			{
				DomainManager.Taiwu.RemoveVillagerWork(context, charId2, true);
			}
			ObjectPool<List<int>>.Instance.Return(chaIdList);
		}

		// Token: 0x06007C26 RID: 31782 RVA: 0x00493878 File Offset: 0x00491A78
		private void FixSoldBuildingItemValueZero()
		{
			foreach (KeyValuePair<BuildingBlockKey, BuildingEarningsData> pair in this._collectBuildingEarningsData)
			{
				for (int i = pair.Value.ShopSoldItemEarnList.Count - 1; i >= 0; i--)
				{
					bool flag = pair.Value.ShopSoldItemEarnList[i].Second == 0;
					if (flag)
					{
						pair.Value.ShopSoldItemEarnList[i] = new IntPair(-1, -1);
						Logger logger = BuildingDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
						defaultInterpolatedStringHandler.AppendLiteral("FixSoldBuildingItemValueZero ,index is ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(pair.Key.BuildingBlockIndex);
						logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
		}

		// Token: 0x06007C27 RID: 31783 RVA: 0x00493974 File Offset: 0x00491B74
		private void FixBuildingLegacyData(DataContext context)
		{
			List<short> templateIdList = DomainManager.Extra.GetLegaciesBuildingTemplateIdList();
			for (int i = templateIdList.Count - 1; i >= 0; i--)
			{
				BuildingBlockItem config = BuildingBlock.Instance[templateIdList[i]];
				bool flag = BuildingBlockData.IsUsefulResource(config.Type);
				if (flag)
				{
					templateIdList.RemoveAt(i);
					ItemKey itemKey = DomainManager.Item.CreateItem(context, 12, config.BuildingCoreItem);
					DomainManager.Taiwu.WarehouseAdd(context, itemKey, 1);
					Logger logger = BuildingDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Transfer legacy normal resource to buildingCore,buildingTemplateId is：");
					defaultInterpolatedStringHandler.AppendFormatted<int>(i);
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			DomainManager.Extra.SetLegaciesBuildingTemplateIdList(templateIdList, context);
		}

		// Token: 0x06007C28 RID: 31784 RVA: 0x00493A40 File Offset: 0x00491C40
		private void FixTeaHouseCaravan(DataContext context)
		{
			List<ItemKey> items = new List<ItemKey>();
			for (int i = this._teaHorseCaravanData.CarryGoodsList.Count - 1; i >= 0; i--)
			{
				ItemKey item = this._teaHorseCaravanData.CarryGoodsList[i].Item1;
				bool flag = !DomainManager.Item.ItemExists(item);
				if (flag)
				{
					this._teaHorseCaravanData.CarryGoodsList.RemoveAt(i);
					Logger logger = BuildingDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Removing not exist tea horse item ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(item);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					bool flag2 = !items.Contains(item);
					if (flag2)
					{
						items.Add(item);
					}
					else
					{
						bool flag3 = !ItemTemplateHelper.IsStackable(item.ItemType, item.TemplateId);
						if (flag3)
						{
							this._teaHorseCaravanData.CarryGoodsList.RemoveAt(i);
							Logger logger2 = BuildingDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Removing duplicate tea horse item ");
							defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(item);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
				}
			}
			this.SetTeaHorseCaravanData(this._teaHorseCaravanData, context);
			DomainManager.Building.UpgradeTeaHorseCaravanByAwareness(context);
		}

		// Token: 0x06007C29 RID: 31785 RVA: 0x00493BA8 File Offset: 0x00491DA8
		private void FixStoneRoomData(DataContext context)
		{
			List<int> charIdList = DomainManager.Extra.GetStoneRoomCharList();
			for (int i = charIdList.Count - 1; i >= 0; i--)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charIdList[i]);
				bool flag = !character.IsCompletelyInfected();
				if (flag)
				{
					Logger logger = BuildingDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Fixing not infected stone room character ");
					defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					charIdList.Remove(charIdList[i]);
					character.DeactivateExternalRelationState(context, 8);
				}
				else
				{
					bool flag2 = !character.IsActiveExternalRelationState(8);
					if (flag2)
					{
						Logger logger2 = BuildingDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Fixing ");
						defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
						defaultInterpolatedStringHandler.AppendLiteral("'s not activated external relation state: CapturedInStoneRoom.");
						logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						character.DeactivateExternalRelationState(context, 8);
					}
				}
			}
			DomainManager.Extra.SetStoneRoomCharList(charIdList, context);
		}

		// Token: 0x06007C2A RID: 31786 RVA: 0x00493CC8 File Offset: 0x00491EC8
		private void FixBuildingCollectResourceTypeData(DataContext context)
		{
			List<BuildingBlockKey> blockKeys = new List<BuildingBlockKey>();
			foreach (KeyValuePair<BuildingBlockKey, sbyte> pair in this._CollectBuildingResourceType)
			{
				BuildingBlockData blockData;
				bool flag = this.TryGetElement_BuildingBlocks(pair.Key, out blockData);
				if (flag)
				{
					bool flag2 = blockData.TemplateId <= 0;
					if (flag2)
					{
						blockKeys.Add(pair.Key);
					}
					else
					{
						BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
						bool flag3 = config.DependBuildings.Count == 0 || !config.IsCollectResourceBuilding;
						if (flag3)
						{
							blockKeys.Add(pair.Key);
						}
						else
						{
							BuildingBlockItem dependConfig = BuildingBlock.Instance[config.DependBuildings[0]];
							bool flag4 = dependConfig == null || dependConfig.CollectResourcePercent == null;
							if (flag4)
							{
								blockKeys.Add(pair.Key);
							}
							else
							{
								int count = 0;
								for (int i = 0; i < dependConfig.CollectResourcePercent.Length; i++)
								{
									bool flag5 = dependConfig.CollectResourcePercent[i] > 0;
									if (flag5)
									{
										count++;
									}
								}
								bool flag6 = count == 1 && !blockKeys.Contains(pair.Key);
								if (flag6)
								{
									blockKeys.Add(pair.Key);
								}
							}
						}
					}
				}
			}
			for (int j = 0; j < blockKeys.Count; j++)
			{
				Logger logger = BuildingDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Fixing buildingCollectResourceTypeData,wrong blockIndex:");
				defaultInterpolatedStringHandler.AppendFormatted<short>(blockKeys[j].BuildingBlockIndex);
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				this.RemoveElement_CollectBuildingResourceType(blockKeys[j], context);
			}
		}

		// Token: 0x06007C2B RID: 31787 RVA: 0x00493ECC File Offset: 0x004920CC
		private void FixBuildingBlockData(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			bool isOldSaveData = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 78, 0);
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey rootBlockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData rootBlockData = DomainManager.Building.GetElement_BuildingBlocks(rootBlockKey);
				bool flag = rootBlockData.TemplateId <= 0;
				if (!flag)
				{
					BuildingBlockItem config = BuildingBlock.Instance[rootBlockData.TemplateId];
					bool flag2 = config.Width == 2;
					if (flag2)
					{
						BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index + 1);
						BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
						bool flag3 = blockData.RootBlockIndex != index;
						if (flag3)
						{
							blockData.ResetData(-1, -1, index);
							this.SetElement_BuildingBlocks(blockKey, blockData, context);
							Logger logger = BuildingDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Fixing buildingBlockData,wrong blockIndex:");
							defaultInterpolatedStringHandler.AppendFormatted<int>((int)(index + 1));
							logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index + (short)areaData.Width);
						blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
						bool flag4 = blockData.RootBlockIndex != index;
						if (flag4)
						{
							blockData.ResetData(-1, -1, index);
							this.SetElement_BuildingBlocks(blockKey, blockData, context);
							Logger logger2 = BuildingDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Fixing buildingBlockData,wrong blockIndex:");
							defaultInterpolatedStringHandler.AppendFormatted<int>((int)(index + (short)areaData.Width));
							logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index + (short)areaData.Width + 1);
						blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
						bool flag5 = blockData.RootBlockIndex != index;
						if (flag5)
						{
							blockData.ResetData(-1, -1, index);
							this.SetElement_BuildingBlocks(blockKey, blockData, context);
							Logger logger3 = BuildingDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Fixing buildingBlockData,wrong blockIndex:");
							defaultInterpolatedStringHandler.AppendFormatted<int>((int)(index + (short)areaData.Width + 1));
							logger3.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
				}
			}
		}

		// Token: 0x06007C2C RID: 31788 RVA: 0x00494134 File Offset: 0x00492334
		private void FixVillageStatusData(DataContext context)
		{
			List<int> memberList = new List<int>();
			DomainManager.Organization.GetElement_CivilianSettlements(DomainManager.Taiwu.GetTaiwuVillageSettlementId()).GetMembers().GetAllMembers(memberList);
			memberList.Remove(DomainManager.Taiwu.GetTaiwuCharId());
			for (int i = 0; i < memberList.Count; i++)
			{
				bool flag = !DomainManager.Taiwu.IsInGroup(memberList[i]) && !this._buildingResidents.ContainsKey(memberList[i]) && !this._homeless.Contains(memberList[i]);
				if (flag)
				{
					this._homeless.Add(memberList[i]);
					Logger logger = BuildingDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Fixing homeless data,homeless add charId:");
					defaultInterpolatedStringHandler.AppendFormatted<int>(memberList[i]);
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			List<int> charList = new List<int>();
			for (int j = this._homeless.GetCount() - 1; j >= 0; j--)
			{
				bool flag2 = charList.Contains(this._homeless.GetCollection()[j]);
				if (flag2)
				{
					Logger logger2 = BuildingDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Fixing homeless data,homeless repeat charId:");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this._homeless.GetCollection()[j]);
					logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					this._homeless.Remove(this._homeless.GetCollection()[j]);
				}
				else
				{
					charList.Add(this._homeless.GetCollection()[j]);
				}
			}
			for (int k = this._homeless.GetCount() - 1; k >= 0; k--)
			{
				int id = this._homeless.GetCollection()[k];
				bool flag3 = this._buildingResidents.ContainsKey(id) || DomainManager.Taiwu.IsInGroup(id);
				if (flag3)
				{
					Logger logger3 = BuildingDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Fixing homeless data,homeless with residents or group repeat charId:");
					defaultInterpolatedStringHandler.AppendFormatted<int>(id);
					logger3.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					this._homeless.Remove(id);
				}
			}
			for (int l = this._homeless.GetCount() - 1; l >= 0; l--)
			{
				bool flag4 = !memberList.Contains(this._homeless.GetCollection()[l]);
				if (flag4)
				{
					Logger logger4 = BuildingDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Fixing homeless data,homeless is not village,charId:");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this._homeless.GetCollection()[l]);
					logger4.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					this._homeless.Remove(this._homeless.GetCollection()[l]);
				}
			}
			this.SetHomeless(this._homeless, context);
			charList.Clear();
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> pair in this._residences)
			{
				for (int m = 0; m < pair.Value.GetCount(); m++)
				{
					int id2 = pair.Value.GetCollection()[m];
					bool flag5 = !memberList.Contains(id2) || DomainManager.Taiwu.IsInGroup(id2);
					if (flag5)
					{
						charList.Add(id2);
					}
				}
			}
			for (int n = 0; n < charList.Count; n++)
			{
				Logger logger5 = BuildingDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Fixing residents data,charId:");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charList[n]);
				defaultInterpolatedStringHandler.AppendLiteral(" is not villager or in group");
				logger5.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				this.RemoveTaiwuResident(context, charList[n], true);
			}
			charList.Clear();
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> pair2 in this._comfortableHouses)
			{
				for (int i2 = 0; i2 < pair2.Value.GetCount(); i2++)
				{
					int id3 = pair2.Value.GetCollection()[i2];
					bool flag6 = !this.IsCharacterAbleToJoinFeast(id3);
					if (flag6)
					{
						charList.Add(id3);
					}
				}
			}
			for (int i3 = 0; i3 < charList.Count; i3++)
			{
				BuildingBlockKey blockKey;
				bool flag7 = this._buildingComfortableHouses.TryGetValue(charList[i3], out blockKey);
				if (flag7)
				{
					Logger logger6 = BuildingDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Fixing residents data,charId:");
					defaultInterpolatedStringHandler.AppendFormatted<int>(charList[i3]);
					defaultInterpolatedStringHandler.AppendLiteral(" is not able to join feast");
					logger6.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					this.RemoveFromComfortableHouse(context, charList[i3], blockKey);
				}
			}
		}

		// Token: 0x06007C2D RID: 31789 RVA: 0x004946C4 File Offset: 0x004928C4
		private void FixVillageResidentData(DataContext context)
		{
			List<BuildingBlockKey> waitToDelete = new List<BuildingBlockKey>();
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> pair in this._residences)
			{
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(pair.Key);
				bool flag = blockData.TemplateId != 46;
				if (flag)
				{
					waitToDelete.Add(pair.Key);
				}
			}
			for (int i = 0; i < waitToDelete.Count; i++)
			{
				Logger logger = BuildingDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Fixing village residences data,wrong buildingBlcokIndex:");
				defaultInterpolatedStringHandler.AppendFormatted<short>(waitToDelete[i].BuildingBlockIndex);
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				this.RemoveResidence(context, waitToDelete[i]);
			}
			waitToDelete.Clear();
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> pair2 in this._comfortableHouses)
			{
				BuildingBlockData blockData2 = DomainManager.Building.GetElement_BuildingBlocks(pair2.Key);
				bool flag2 = blockData2.TemplateId != 47;
				if (flag2)
				{
					waitToDelete.Add(pair2.Key);
				}
			}
			for (int j = 0; j < waitToDelete.Count; j++)
			{
				Logger logger2 = BuildingDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Fixing village comfortableHouses data,wrong buildingBlockIndex:");
				defaultInterpolatedStringHandler.AppendFormatted<short>(waitToDelete[j].BuildingBlockIndex);
				logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				this.RemoveComfortableHouse(context, waitToDelete[j]);
			}
		}

		// Token: 0x06007C2E RID: 31790 RVA: 0x004948A8 File Offset: 0x00492AA8
		private void FixBuildingEarningsData(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag2 = blockData.TemplateId != 222;
				if (!flag2)
				{
					BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
					bool flag3 = configData.SuccesEvent.Count <= 0;
					if (!flag3)
					{
						short eventTemplateId = configData.SuccesEvent[0];
						ShopEventItem eventConfig = ShopEvent.Instance[eventTemplateId];
						bool flag4 = eventConfig.ItemList.Count > 0 || eventConfig.ItemGradeProbList.Count > 0;
						if (flag4)
						{
							BuildingEarningsData data;
							bool flag5 = !this.TryGetElement_CollectBuildingEarningsData(blockKey, out data);
							if (!flag5)
							{
								bool flag6 = data.CollectionItemList != null;
								if (flag6)
								{
									bool flag = false;
									for (int i = 0; i < data.CollectionItemList.Count; i++)
									{
										ItemKey itemKey = data.CollectionItemList[i];
										bool flag7 = itemKey.Id < 0;
										if (flag7)
										{
											Logger logger = BuildingDomain.Logger;
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 3);
											defaultInterpolatedStringHandler.AppendLiteral("Fixing buildingEarnData item id,wrong item index:");
											defaultInterpolatedStringHandler.AppendFormatted<int>(i);
											defaultInterpolatedStringHandler.AppendLiteral(", ItemType:");
											defaultInterpolatedStringHandler.AppendFormatted<sbyte>(itemKey.ItemType);
											defaultInterpolatedStringHandler.AppendLiteral(",TemplateId:");
											defaultInterpolatedStringHandler.AppendFormatted<short>(itemKey.TemplateId);
											logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
											ItemKey item = DomainManager.Item.CreateItem(context, itemKey.ItemType, itemKey.TemplateId);
											data.CollectionItemList[i] = item;
											flag = true;
										}
									}
									bool flag8 = flag;
									if (flag8)
									{
										this.SetElement_CollectBuildingEarningsData(blockKey, data, context);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007C2F RID: 31791 RVA: 0x00494AD4 File Offset: 0x00492CD4
		public void TaiwuResourceGrow(DataContext context)
		{
		}

		// Token: 0x06007C30 RID: 31792 RVA: 0x00494AD7 File Offset: 0x00492CD7
		[Obsolete]
		public bool CanAutoUpgrade(BuildingBlockKey blockKey)
		{
			return false;
		}

		// Token: 0x06007C31 RID: 31793 RVA: 0x00494ADC File Offset: 0x00492CDC
		public void TriggerBuildingCompleteEvents(DataContext context)
		{
			foreach (BuildingBlockKey buildingKey in this._newCompleteOperationBuildings)
			{
				BuildingBlockData blockData = this.GetBuildingBlockData(buildingKey);
				DomainManager.TaiwuEvent.OnEvent_ConstructComplete(buildingKey, blockData.TemplateId, this.BuildingBlockLevel(buildingKey));
			}
			this._newCompleteOperationBuildings.Clear();
			this.SetNewCompleteOperationBuildings(this._newCompleteOperationBuildings, context);
		}

		// Token: 0x06007C32 RID: 31794 RVA: 0x00494B68 File Offset: 0x00492D68
		public unsafe void UpdateResourceBlockEffectsOnAdvanceMonth(DataContext context)
		{
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			foreach (KeyValuePair<Location, BuildingAreaData> keyValuePair in this._buildingAreas)
			{
				Location location2;
				BuildingAreaData buildingAreaData;
				keyValuePair.Deconstruct(out location2, out buildingAreaData);
				Location location = location2;
				ValueTuple<ResourceInts, int> valueTuple = this.CalcResourceBlockIncomeEffects(location);
				ResourceInts resourcesChange = valueTuple.Item1;
				int expGain = valueTuple.Item2;
				ResourceInts npcResChange = resourcesChange;
				this.FinalizeResourceBlockIncomeEffectValues(ref npcResChange, false);
				Settlement settlement = DomainManager.Organization.GetSettlementByLocation(location);
				OrgMemberCollection members = settlement.GetMembers();
				bool hasResources = resourcesChange.IsNonZero();
				bool hasExp = expGain > 0;
				bool flag = !hasResources && !hasExp;
				if (!flag)
				{
					for (sbyte grade = 0; grade <= 8; grade += 1)
					{
						HashSet<int> gradeMembers = members.GetMembers(grade);
						foreach (int charId in gradeMembers)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							bool flag2 = !character.IsInteractableAsIntelligentCharacter();
							if (!flag2)
							{
								bool flag3 = hasResources;
								if (flag3)
								{
									bool flag4 = charId == taiwuId;
									if (flag4)
									{
										ResourceInts taiwuResChange = resourcesChange;
										this.FinalizeResourceBlockIncomeEffectValues(ref taiwuResChange, true);
										character.ChangeResources(context, ref taiwuResChange);
									}
									else
									{
										OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(character.GetOrganizationInfo());
										CValuePercent ratio = orgMemberCfg.ResourceIncomeRatio;
										ResourceInts charResChange = npcResChange;
										for (sbyte resType = 0; resType < 8; resType += 1)
										{
											*charResChange[(int)resType] *= ratio;
										}
										character.ChangeResources(context, ref charResChange);
									}
								}
								bool flag5 = hasExp;
								if (flag5)
								{
									character.ChangeExp(context, expGain);
									bool flag6 = taiwuId == charId;
									if (flag6)
									{
										InstantNotificationCollection monthlyNotifications = DomainManager.World.GetInstantNotificationCollection();
										monthlyNotifications.AddBuildingExp(expGain);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007C33 RID: 31795 RVA: 0x00494DB0 File Offset: 0x00492FB0
		public void SerialUpdate(DataContext context)
		{
			this.ClearBuildingException();
			List<Location> taiwuBuildingList = this.GetTaiwuBuildingAreas();
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
			List<short> expandedResourceList = ObjectPool<List<short>>.Instance.Get();
			List<ValueTuple<BuildingBlockKey, BuildingBlockData>> needMaintenanceList = new List<ValueTuple<BuildingBlockKey, BuildingBlockData>>();
			this._newBrokenBuildings.Clear();
			this._alreadyUpdateShopProgressBlock.Clear();
			for (int i = 0; i < taiwuBuildingList.Count; i++)
			{
				Location location = taiwuBuildingList[i];
				BuildingAreaData areaData = this.GetElement_BuildingAreas(location);
				MapAreaData mapAreaData = DomainManager.Map.GetElement_Areas((int)location.AreaId);
				int settlementIndex = DomainManager.Map.GetElement_Areas((int)location.AreaId).GetSettlementIndex(location.BlockId);
				short settlementId = mapAreaData.SettlementInfos[settlementIndex].SettlementId;
				expandedResourceList.Clear();
				needMaintenanceList.Clear();
				for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
				{
					BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
					BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
					bool flag = blockData.RootBlockIndex >= 0;
					if (!flag)
					{
						BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
						bool flag2 = configData.Type != EBuildingBlockType.Building && !BuildingBlockData.IsResource(configData.Type);
						if (!flag2)
						{
							bool flag3 = configData.TemplateId == 45 && blockData.CanUse();
							if (flag3)
							{
								int tmp = this.CalculateGainAuthorityByShrinePerMonth(blockKey);
								taiwuChar.ChangeResource(context, 7, tmp);
							}
							bool flag4 = blockData.Durability == 0 && this.BuildingBlockLevel(blockKey) == 1 && configData.DestoryType == 1;
							if (flag4)
							{
								int authorityAmount = this.GetAllTaiwuResources().Get(7);
								this.ConsumeResource(context, 7, Math.Min(authorityAmount, (int)configData.MaxDurability));
							}
							bool flag5 = configData.BaseMaintenanceCost.Count > 0 && blockData.NeedMaintenanceCost();
							if (flag5)
							{
								needMaintenanceList.Add(new ValueTuple<BuildingBlockKey, BuildingBlockData>(blockKey, blockData));
							}
							bool flag6 = this.CanUpdateShopProgress(blockKey);
							if (flag6)
							{
								int buildingAttainment = this.GetBuildingAttainment(blockData, blockKey, false);
								int delta = GameData.Domains.Building.SharedMethods.GetShopManageProgressDelta(blockData.TemplateId, buildingAttainment);
								int resBuildingBonus = this.GetBuildingBlockEffect(settlementId, EBuildingScaleEffect.ShopProgressBonus, -1);
								delta *= resBuildingBonus;
								blockData.OfflineChangeShopProgress(delta);
								this.SetElement_BuildingBlocks(blockKey, blockData, context);
							}
							bool flag7 = !BuildingBlockData.IsResource(configData.Type) || blockData.OperationType != -1 || expandedResourceList.Contains(index);
							if (!flag7)
							{
								sbyte buildingWidth = configData.Width;
								List<int> neighborDistanceList = ObjectPool<List<int>>.Instance.Get();
								List<short> neighborRangeOneList = ObjectPool<List<short>>.Instance.Get();
								areaData.GetNeighborBlocks(index, buildingWidth, neighborList, neighborDistanceList, 2);
								areaData.GetNeighborBlocks(index, buildingWidth, neighborRangeOneList, null, 1);
								this.UpdateResourceBlock(context, settlementId, blockKey, blockData, neighborList, expandedResourceList, neighborDistanceList, neighborRangeOneList);
							}
						}
					}
				}
				foreach (ValueTuple<BuildingBlockKey, BuildingBlockData> valueTuple in needMaintenanceList)
				{
					BuildingBlockKey blockKey2 = valueTuple.Item1;
					BuildingBlockData blockData2 = valueTuple.Item2;
					bool flag8 = !this.UpdateBlockMaintenance(context, blockKey2, blockData2, taiwuChar);
					if (flag8)
					{
						this._newBrokenBuildings.Add(blockKey2);
					}
				}
				BuildingBlockKey samsaraPlatformKey = BuildingDomain.FindBuildingKey(location, areaData, 50, true);
				bool flag9 = !samsaraPlatformKey.IsInvalid;
				if (flag9)
				{
					this.AddSamsaraPlatformProgress(context, this.BuildingBlockLevel(samsaraPlatformKey));
				}
			}
			ObjectPool<List<short>>.Instance.Return(neighborList);
			ObjectPool<List<short>>.Instance.Return(expandedResourceList);
			this.UpdateChickenInstances(context);
			this.UpdateCricketRegenProgress(context);
			this.ApplyCricketAuthorityGain(context);
			this.UpdateResidentsHappinessAndFavor(context);
			this.UpdateTeaHorseCaravan(context);
			this.UpdateStoneRoomData(context);
			this.UpdateKungfuPracticeRoom(context);
		}

		// Token: 0x06007C34 RID: 31796 RVA: 0x004951BC File Offset: 0x004933BC
		public int CalculateGainAuthorityByShrinePerMonth(BuildingBlockKey shrineBlockKey)
		{
			bool flag = !this.HasShopManagerLeader(shrineBlockKey);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte fameType = this.BuildLeaderFameType(shrineBlockKey);
				bool flag2;
				int attainment = this.BuildingTotalAttainment(shrineBlockKey, -1, out flag2, false);
				result = GameData.Domains.Building.SharedMethods.GetTaiwuShrineEffect(fameType, attainment);
			}
			return result;
		}

		// Token: 0x06007C35 RID: 31797 RVA: 0x004951FC File Offset: 0x004933FC
		private void UpdateKungfuPracticeRoom(DataContext context)
		{
			bool flag = !this.IsTaiwuVillageHaveSpecifyBuilding(52, false);
			if (!flag)
			{
				List<sbyte> xiangshuIdList = DomainManager.Extra.GetXiangshuIdInKungfuPracticeRoom();
				for (sbyte i = 0; i < 9; i += 1)
				{
					bool flag2 = DomainManager.World.GetXiangshuAvatarFavorabilityType(i) >= 3 && !xiangshuIdList.Contains(i);
					if (flag2)
					{
						xiangshuIdList.Add(i);
						MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
						switch (i)
						{
						case 0:
							monthlyEventCollection.AddVillageWoodenManByMonv();
							break;
						case 1:
							monthlyEventCollection.AddVillageWoodenManByDayueYaochang();
							break;
						case 2:
							monthlyEventCollection.AddVillageWoodenManByJiuhan();
							break;
						case 3:
							monthlyEventCollection.AddVillageWoodenManByJinHuanger();
							break;
						case 4:
							monthlyEventCollection.AddVillageWoodenManByYiYihou();
							break;
						case 5:
							monthlyEventCollection.AddVillageWoodenManByWeiQi();
							break;
						case 6:
							monthlyEventCollection.AddVillageWoodenManByYixiang();
							break;
						case 7:
							monthlyEventCollection.AddVillageWoodenManByXuefeng();
							break;
						case 8:
							monthlyEventCollection.AddVillageWoodenManByShuFang();
							break;
						}
					}
				}
				DomainManager.Extra.SetXiangshuIdInKungfuPracticeRoom(xiangshuIdList, context);
			}
		}

		// Token: 0x06007C36 RID: 31798 RVA: 0x0049531C File Offset: 0x0049351C
		private void UpdateStoneRoomData(DataContext context)
		{
			List<GameData.Domains.Character.Character> characterList = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			List<GameData.Domains.Character.Character> characterListAll = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			List<Predicate<GameData.Domains.Character.Character>> predicates = ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Get();
			predicates.Clear();
			characterListAll.Clear();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			sbyte taiwuConsummateLevel = DomainManager.Taiwu.GetTaiwu().GetConsummateLevel();
			bool flag = taiwuConsummateLevel < 6;
			if (!flag)
			{
				predicates.Add(new Predicate<GameData.Domains.Character.Character>(CharacterMatchers.MatchNotCalledByAdventure));
				predicates.Add((GameData.Domains.Character.Character character) => CharacterMatchers.MatchConsummateLevel(character, 0, taiwuConsummateLevel - 6));
				for (short i = 0; i < 135; i += 1)
				{
					characterList.Clear();
					MapAreaData mapAreaData = DomainManager.Map.GetElement_Areas((int)i);
					bool flag2 = !mapAreaData.StationUnlocked;
					if (!flag2)
					{
						MapCharacterFilter.FindInfected(predicates, characterList, i);
						bool flag3 = characterList.Count == 0;
						if (!flag3)
						{
							characterListAll.AddRange(characterList);
						}
					}
				}
				int num = context.Random.Next(1, 4);
				num = Math.Min(characterListAll.Count, num);
				for (int j = 0; j < num; j++)
				{
					bool flag4 = DomainManager.Extra.IsStoneRoomFull();
					if (flag4)
					{
						break;
					}
					int index = context.Random.Next(0, characterListAll.Count);
					GameData.Domains.Character.Character selectedChar = characterListAll[index];
					int charId = selectedChar.GetId();
					Location srcLocation = selectedChar.GetLocation();
					CollectionUtils.SwapAndRemove<GameData.Domains.Character.Character>(characterListAll, index);
					bool flag5 = selectedChar.IsActiveExternalRelationState(32);
					if (flag5)
					{
						Logger logger = BuildingDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(84, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Trying to add ");
						defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(selectedChar);
						defaultInterpolatedStringHandler.AppendLiteral(" at ");
						defaultInterpolatedStringHandler.AppendFormatted<Location>(srcLocation);
						defaultInterpolatedStringHandler.AppendLiteral(" to stone room when the character is already in settlement prison.");
						logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					else
					{
						bool flag6 = selectedChar.IsActiveExternalRelationState(8);
						if (flag6)
						{
							Logger logger2 = BuildingDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(77, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Trying to add ");
							defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(selectedChar);
							defaultInterpolatedStringHandler.AppendLiteral(" at ");
							defaultInterpolatedStringHandler.AppendFormatted<Location>(srcLocation);
							defaultInterpolatedStringHandler.AppendLiteral(" to stone room when the character is already in stone room.");
							logger2.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						else
						{
							bool flag7 = context.Random.CheckPercentProb(GlobalConfig.Instance.ImprisonInStoneHouseChance);
							if (flag7)
							{
								DomainManager.Extra.AddStoneRoomCharacter(context, selectedChar);
								short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
								monthlyNotifications.AddStoneHouseInfectedKidnapped(settlementId, charId);
								lifeRecordCollection.AddXiangshuInfectedPrisonTaiwuVillage(charId, currDate, srcLocation);
							}
							else
							{
								sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(srcLocation.AreaId);
								sbyte sectId = MapState.Instance[stateTemplateId].SectID;
								Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(sectId);
								sect.AddPrisoner(context, selectedChar, 39);
								lifeRecordCollection.AddXiangshuInfectedPrisonSettlement(charId, currDate, srcLocation, sect.GetId());
							}
						}
					}
				}
				ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(characterList);
				ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(characterListAll);
				ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Return(predicates);
			}
		}

		// Token: 0x06007C37 RID: 31799 RVA: 0x00495670 File Offset: 0x00493870
		public sbyte GetResourceBlockGrowthChance(BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData = this.GetBuildingBlockData(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			bool flag = this.BuildingBlockLevel(blockKey) >= configData.MaxLevel;
			sbyte result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int buildingAddGrowthOdds = 0;
				BuildingAreaData areaData = this.GetBuildingAreaData(blockKey.GetLocation());
				List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
				List<int> neighborDistanceList = ObjectPool<List<int>>.Instance.Get();
				areaData.GetNeighborBlocks(blockKey.BuildingBlockIndex, configData.Width, neighborList, neighborDistanceList, 2);
				for (int i = 0; i < neighborList.Count; i++)
				{
					BuildingBlockKey neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborList[i]);
					BuildingBlockData neighborBlock = this.GetElement_BuildingBlocks(neighborKey);
					sbyte neighborLevel = this.BuildingBlockLevel(neighborKey);
					bool flag2 = neighborBlock.RootBlockIndex >= 0;
					if (flag2)
					{
						neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborBlock.RootBlockIndex);
						neighborBlock = this.GetElement_BuildingBlocks(neighborKey);
					}
					BuildingBlockItem neighborConfig = BuildingBlock.Instance[neighborBlock.TemplateId];
					bool flag3 = !neighborConfig.IsCollectResourceBuilding || neighborConfig.DependBuildings[0] != blockData.TemplateId || !neighborBlock.CanUse() || !this._shopManagerDict.ContainsKey(neighborKey);
					if (!flag3)
					{
						List<int> managerList = this._shopManagerDict[neighborKey].GetCollection();
						int expansionProgressGain = 0;
						bool hasValidManager = false;
						foreach (int charId in managerList)
						{
							bool flag4 = charId < 0 || !DomainManager.Taiwu.CanWork(charId);
							if (!flag4)
							{
								GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
								expansionProgressGain += (int)manageChar.GetLifeSkillAttainment(configData.RequireLifeSkillType);
								hasValidManager = true;
							}
						}
						bool flag5 = !hasValidManager;
						if (!flag5)
						{
							buildingAddGrowthOdds += (int)neighborLevel * (50 + expansionProgressGain) / 100;
						}
					}
				}
				ObjectPool<List<short>>.Instance.Return(neighborList);
				ObjectPool<List<int>>.Instance.Return(neighborDistanceList);
				int totalChance = buildingAddGrowthOdds * (int)blockData.Durability / (int)configData.MaxDurability;
				result = (sbyte)Math.Clamp(totalChance, 0, 100);
			}
			return result;
		}

		// Token: 0x06007C38 RID: 31800 RVA: 0x004958D4 File Offset: 0x00493AD4
		public sbyte GetResourceBlockExpandChance(BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData = this.GetBuildingBlockData(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			bool flag = this.BuildingBlockLevel(blockKey) >= configData.MaxLevel;
			sbyte result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int buildingAddExpandOdds = 0;
				BuildingAreaData areaData = this.GetBuildingAreaData(blockKey.GetLocation());
				List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
				List<int> neighborDistanceList = ObjectPool<List<int>>.Instance.Get();
				areaData.GetNeighborBlocks(blockKey.BuildingBlockIndex, configData.Width, neighborList, neighborDistanceList, 2);
				for (int i = 0; i < neighborList.Count; i++)
				{
					BuildingBlockKey neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborList[i]);
					BuildingBlockData neighborBlock = this.GetElement_BuildingBlocks(neighborKey);
					bool flag2 = neighborBlock.RootBlockIndex >= 0;
					if (flag2)
					{
						neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborBlock.RootBlockIndex);
						neighborBlock = this.GetElement_BuildingBlocks(neighborKey);
					}
					BuildingBlockItem neighborConfig = BuildingBlock.Instance[neighborBlock.TemplateId];
					bool flag3 = !neighborConfig.IsCollectResourceBuilding || neighborConfig.DependBuildings[0] != blockData.TemplateId || !neighborBlock.CanUse() || !this._shopManagerDict.ContainsKey(neighborKey);
					if (!flag3)
					{
						List<int> managerList = this._shopManagerDict[neighborKey].GetCollection();
						int expansionProgressGain = 0;
						bool hasValidManager = false;
						foreach (int charId in managerList)
						{
							bool flag4 = charId < 0 || !DomainManager.Taiwu.CanWork(charId);
							if (!flag4)
							{
								GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
								expansionProgressGain += (int)manageChar.GetLifeSkillAttainment(configData.RequireLifeSkillType);
								hasValidManager = true;
							}
						}
						bool flag5 = !hasValidManager;
						if (!flag5)
						{
							buildingAddExpandOdds += (int)this.BuildingBlockLevel(blockKey) * (50 + expansionProgressGain) / 100;
						}
					}
				}
				ObjectPool<List<short>>.Instance.Return(neighborList);
				ObjectPool<List<int>>.Instance.Return(neighborDistanceList);
				int totalChance = buildingAddExpandOdds * (int)blockData.Durability / (int)configData.MaxDurability;
				result = (sbyte)Math.Clamp(totalChance, 0, 100);
			}
			return result;
		}

		// Token: 0x06007C39 RID: 31801 RVA: 0x00495B34 File Offset: 0x00493D34
		private void UpdateResourceBlock(DataContext context, short settlementId, BuildingBlockKey blockKey, BuildingBlockData blockData, List<short> neighborList, List<short> expandedResourceList, List<int> neighborDistanceList, List<short> neighborRangeOneList)
		{
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			IRandomSource random = context.Random;
			int buildingAddGrowthOdds = 0;
			int buildingAddExpandOdds = 0;
			for (int i = 0; i < neighborList.Count; i++)
			{
				BuildingBlockKey neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborList[i]);
				BuildingBlockData neighborBlock = this.GetElement_BuildingBlocks(neighborKey);
				sbyte neighborLevel = this.BuildingBlockLevel(neighborKey);
				bool flag2 = neighborBlock.RootBlockIndex >= 0;
				if (flag2)
				{
					neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborBlock.RootBlockIndex);
					neighborBlock = this.GetElement_BuildingBlocks(neighborKey);
				}
				BuildingBlockItem neighborConfig = BuildingBlock.Instance[neighborBlock.TemplateId];
				bool flag3 = !neighborConfig.IsCollectResourceBuilding || neighborConfig.DependBuildings[0] != blockData.TemplateId || !neighborBlock.CanUse() || !this._shopManagerDict.ContainsKey(neighborKey);
				if (!flag3)
				{
					List<int> managerList = this._shopManagerDict[neighborKey].GetCollection();
					sbyte resourceType = this.GetCollectBuildingResourceType(neighborKey);
					sbyte b = (resourceType < 6) ? resourceType : 5;
					sbyte b2 = (resourceType < 6) ? Config.ResourceType.Instance[resourceType].LifeSkillType : 9;
					int expansionProgressGain = 0;
					bool hasValidManager = false;
					foreach (int charId in managerList)
					{
						bool flag4 = charId < 0 || !DomainManager.Taiwu.CanWork(charId);
						if (!flag4)
						{
							GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
							expansionProgressGain += (int)manageChar.GetLifeSkillAttainment(configData.RequireLifeSkillType);
							hasValidManager = true;
						}
					}
					bool hasManager;
					int manageProgressGain = GameData.Domains.Building.SharedMethods.GetShopManageProgressDelta(neighborConfig.TemplateId, this.BuildingTotalAttainment(neighborKey, resourceType, out hasManager, false));
					int resBuildingBonus = this.GetBuildingBlockEffect(settlementId, EBuildingScaleEffect.ShopProgressBonus, -1);
					manageProgressGain *= resBuildingBonus;
					bool flag5 = !hasValidManager || !hasManager;
					if (!flag5)
					{
						buildingAddGrowthOdds += (int)neighborLevel * (50 + expansionProgressGain) / 100;
						buildingAddExpandOdds += (int)neighborLevel * (50 + expansionProgressGain) / 100;
						bool flag = true;
						bool flag6 = neighborConfig.SuccesEvent.Count > 0 && ShopEvent.Instance.GetItem(neighborConfig.SuccesEvent[0]).ItemList.Count > 0;
						if (flag6)
						{
							BuildingEarningsData data;
							this.TryGetElement_CollectBuildingEarningsData(neighborKey, out data);
							bool flag7 = data == null || data.CollectionItemList == null;
							if (flag7)
							{
								flag = true;
							}
							else
							{
								bool flag8 = data.CollectionItemList.Count >= (int)neighborLevel;
								if (flag8)
								{
									flag = false;
								}
							}
						}
						bool flag9 = flag;
						if (flag9)
						{
							bool flag10 = !this._alreadyUpdateShopProgressBlock.ContainsKey(settlementId) || !this._alreadyUpdateShopProgressBlock[settlementId].Contains(neighborBlock);
							if (flag10)
							{
								neighborBlock.OfflineChangeShopProgress(manageProgressGain);
							}
							this.<UpdateResourceBlock>g__UpdateShopProgressBlock|108_0(settlementId, neighborBlock);
						}
					}
				}
			}
			bool blockDataChanged = false;
			bool flag11 = blockData.Durability < configData.MaxDurability;
			if (flag11)
			{
				blockData.Durability += 1;
				blockDataChanged = true;
			}
			bool flag12 = blockDataChanged;
			if (flag12)
			{
				this.SetElement_BuildingBlocks(blockKey, blockData, context);
			}
		}

		// Token: 0x06007C3A RID: 31802 RVA: 0x00495EA0 File Offset: 0x004940A0
		private bool UpdateBlockMaintenance(DataContext context, BuildingBlockKey blockKey, BuildingBlockData blockData, GameData.Domains.Character.Character taiwuChar)
		{
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			bool enough = true;
			ResourceInts resource = this.GetAllTaiwuResources();
			int[] resourceCosts = GameData.Domains.Building.SharedMethods.GetFinalMaintenanceCost(configData);
			sbyte resType = 0;
			while ((int)resType < resourceCosts.Length)
			{
				bool flag = resource.Get((int)resType) >= resourceCosts[(int)resType];
				if (!flag)
				{
					enough = false;
					break;
				}
				resType += 1;
			}
			bool flag2 = enough;
			if (flag2)
			{
				sbyte resType2 = 0;
				while ((int)resType2 < resourceCosts.Length)
				{
					this.ConsumeResource(context, resType2, resourceCosts[(int)resType2]);
					resType2 += 1;
				}
			}
			return enough;
		}

		// Token: 0x06007C3B RID: 31803 RVA: 0x00495F40 File Offset: 0x00494140
		public void UpdateBrokenBuildings(DataContext context)
		{
			foreach (BuildingBlockKey blockKey in this._newBrokenBuildings)
			{
				BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
				BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
				MapAreaData mapAreaData = DomainManager.Map.GetElement_Areas((int)blockKey.AreaId);
				int settlementIndex = mapAreaData.GetSettlementIndex(blockKey.BlockId);
				short settlementId = mapAreaData.SettlementInfos[settlementIndex].SettlementId;
				BuildingBlockData buildingBlockData = blockData;
				buildingBlockData.Durability -= 1;
				bool flag = blockData.Durability > 0;
				if (flag)
				{
					this.SetElement_BuildingBlocks(blockKey, blockData, context);
					return;
				}
				blockData.Durability = 0;
				bool flag2 = configData.DestoryType == 0;
				if (flag2)
				{
					MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
					monthlyNotifications.AddBuildingRuined(settlementId, blockData.TemplateId);
					this.BuildingRemoveResident(context, blockData, blockKey);
					this.BuildingRemoveVillagerWork(context, blockKey);
					this.ClearBuildingBlockEarningsData(context, blockKey, blockData.TemplateId == 222);
					this.ResetAllChildrenBlocks(context, blockKey, 23, 1);
					blockData.ResetData(23, 1, -1);
				}
				else
				{
					bool flag3 = configData.DestoryType == 1;
					if (flag3)
					{
						InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotificationCollection();
						instantNotifications.AddBuildingLoseAuthority(settlementId, blockData.TemplateId);
					}
				}
				this.SetElement_BuildingBlocks(blockKey, blockData, context);
			}
			this.UpdateTaiwuVillageBuildingEffect();
		}

		// Token: 0x06007C3C RID: 31804 RVA: 0x004960D4 File Offset: 0x004942D4
		public void ParallelUpdate(DataContext context)
		{
			foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> entry in this._buildingBlocks)
			{
				BuildingBlockKey blockKey = entry.Key;
				BuildingBlockData blockData = entry.Value;
				bool flag = blockData.RootBlockIndex >= 0;
				if (!flag)
				{
					BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
					bool flag2 = configData.Type != EBuildingBlockType.Building && !BuildingBlockData.IsResource(configData.Type) && configData.TemplateId != 44;
					if (!flag2)
					{
						ParallelBuildingModification modification = new ParallelBuildingModification
						{
							BlockKey = blockKey,
							BlockData = blockData
						};
						MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)blockKey.AreaId);
						int settlementIndex = DomainManager.Map.GetElement_Areas((int)blockKey.AreaId).GetSettlementIndex(blockKey.BlockId);
						short settlementId = areaData.SettlementInfos[settlementIndex].SettlementId;
						bool flag3 = blockData.CanUse() && configData.IsShop;
						if (flag3)
						{
							this.OfflineUpdateShopManagement(modification, settlementId, configData, blockKey, blockData, context);
						}
						this.OfflineUpdateOperation(context, modification, settlementId, configData, blockKey, blockData);
						ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
						recorder.RecordType(ParallelModificationType.UpdateBuilding);
						recorder.RecordParameterClass<ParallelBuildingModification>(modification);
					}
				}
			}
		}

		// Token: 0x06007C3D RID: 31805 RVA: 0x00496254 File Offset: 0x00494454
		public void TutorialUpdate(DataContext context)
		{
			foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> entry in this._buildingBlocks)
			{
				BuildingBlockKey blockKey = entry.Key;
				BuildingBlockData blockData = entry.Value;
				bool flag = blockData.RootBlockIndex >= 0;
				if (!flag)
				{
					BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
					bool flag2 = configData.TemplateId == 258 && blockData.OperationType == 0 && DomainManager.Taiwu.VillagerHasWork(DomainManager.Taiwu.GetTaiwuCharId());
					if (flag2)
					{
						DomainManager.Extra.ModifyBuildingExtraData(context, blockKey, delegate(BuildingBlockDataEx blockDataEx)
						{
							blockDataEx.LevelUnlockedFlags = 1UL;
						});
						blockData.Durability = configData.MaxDurability;
						blockData.OperationType = -1;
						blockData.OperationProgress = 0;
						int settlementIndex = DomainManager.Map.GetElement_Areas((int)blockKey.AreaId).GetSettlementIndex(blockKey.BlockId);
						MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)blockKey.AreaId);
						short settlementId = areaData.SettlementInfos[settlementIndex].SettlementId;
						MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
						monthlyNotifications.AddBuildingConstructionCompleted(settlementId, blockData.TemplateId);
						DomainManager.Taiwu.RemoveVillagerWork(context, DomainManager.Taiwu.GetTaiwuCharId(), true);
					}
				}
			}
		}

		// Token: 0x06007C3E RID: 31806 RVA: 0x004963E8 File Offset: 0x004945E8
		private void UpdateTeaHorseCaravan(DataContext context)
		{
			bool flag = this._teaHorseCaravanData == null || this._teaHorseCaravanData.CaravanState < 1 || this._teaHorseCaravanData.CaravanState == 4;
			if (!flag)
			{
				this._teaHorseCaravanData.IsShowExchangeReplenishment = false;
				bool flag2 = this._teaHorseCaravanData.CaravanState == 1;
				if (flag2)
				{
					this._teaHorseCaravanData.CaravanState = 2;
				}
				bool flag3 = this._teaHorseCaravanData.StartMonth >= 360;
				if (flag3)
				{
					MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
					monthlyNotifications.AddWesternMerchantBackAfterLong();
					this._teaHorseCaravanData.CaravanState = 4;
					this._teaHorseCaravanData.IsShowExchangeReplenishment = false;
					this._teaHorseCaravanData.IsShowSeachReplenishment = false;
					this._teaHorseCaravanData.DistanceToTaiwuVillage = 0;
					this._teaHorseCaravanData.Terrain = 5;
				}
				TeaHorseCaravanData teaHorseCaravanData = this._teaHorseCaravanData;
				teaHorseCaravanData.StartMonth += 1;
				bool isStartSearch = this._teaHorseCaravanData.IsStartSearch;
				if (isStartSearch)
				{
					this._teaHorseCaravanData.IsStartSearch = false;
				}
				else
				{
					sbyte currSolarTerm = DomainManager.World.GetCurrMonthInYear();
					List<TeaHorseCaravanTerrainItem> terrainList = new List<TeaHorseCaravanTerrainItem>();
					short i = 0;
					while ((int)i < TeaHorseCaravanTerrain.Instance.Count)
					{
						terrainList.Add(TeaHorseCaravanTerrain.Instance.GetItem(i));
						i += 1;
					}
					IEnumerable<ValueTuple<short, short>> terrainWeights = from a in terrainList
					select new ValueTuple<short, short>(a.TemplateId, (short)a.Weighted);
					short terrainTemplateId = RandomUtils.GetRandomResult<short>(terrainWeights, context.Random);
					this._teaHorseCaravanData.Terrain = terrainTemplateId;
					List<TeaHorseCaravanWeatherItem> weatherList = new List<TeaHorseCaravanWeatherItem>();
					short j = 0;
					while ((int)j < TeaHorseCaravanWeather.Instance.Count)
					{
						TeaHorseCaravanWeatherItem item = TeaHorseCaravanWeather.Instance.GetItem(j);
						bool flag4 = item.NeedSolarTerms.Contains(currSolarTerm) && item.NeedTerrain.Contains((sbyte)terrainTemplateId);
						if (flag4)
						{
							weatherList.Add(TeaHorseCaravanWeather.Instance.GetItem(j));
						}
						j += 1;
					}
					IEnumerable<ValueTuple<short, short>> weatherWeights = from a in weatherList
					select new ValueTuple<short, short>(a.TemplateId, (short)a.Weighted);
					short weatherTemplateId = RandomUtils.GetRandomResult<short>(weatherWeights, context.Random);
					this._teaHorseCaravanData.Weather = weatherTemplateId;
					short lostProb = 0;
					bool flag5 = this._teaHorseCaravanData.LackReplenishmentTurn > 0;
					if (flag5)
					{
						lostProb = Math.Clamp((short)MathF.Pow(2f, (float)(this._teaHorseCaravanData.LackReplenishmentTurn - 1)), 0, 100);
					}
					bool flag6 = lostProb > 0 && context.Random.CheckProb((int)lostProb, 100);
					if (flag6)
					{
						bool flag7 = context.Random.Next(2) == 0;
						if (flag7)
						{
							bool flag8 = this._teaHorseCaravanData.CarryGoodsList.Count > 0;
							if (flag8)
							{
								int index = context.Random.Next(this._teaHorseCaravanData.CarryGoodsList.Count);
								ItemKey lostItem = this._teaHorseCaravanData.CarryGoodsList[index].Item1;
								this._teaHorseCaravanData.CarryGoodsList.RemoveAt(index);
								DomainManager.Item.RemoveItem(context, lostItem);
							}
							else
							{
								bool flag9 = this._teaHorseCaravanData.ExchangeGoodsList.Count > 0;
								if (flag9)
								{
									int index2 = context.Random.Next(this._teaHorseCaravanData.ExchangeGoodsList.Count);
									this._teaHorseCaravanData.ExchangeGoodsList.RemoveAt(index2);
								}
							}
						}
						else
						{
							bool flag10 = this._teaHorseCaravanData.ExchangeGoodsList.Count > 0;
							if (flag10)
							{
								int index3 = context.Random.Next(this._teaHorseCaravanData.ExchangeGoodsList.Count);
								this._teaHorseCaravanData.ExchangeGoodsList.RemoveAt(index3);
							}
							else
							{
								bool flag11 = this._teaHorseCaravanData.CarryGoodsList.Count > 0;
								if (flag11)
								{
									int index4 = context.Random.Next(this._teaHorseCaravanData.CarryGoodsList.Count);
									ItemKey lostItem2 = this._teaHorseCaravanData.CarryGoodsList[index4].Item1;
									this._teaHorseCaravanData.CarryGoodsList.RemoveAt(index4);
									DomainManager.Item.RemoveItem(context, lostItem2);
								}
							}
						}
					}
					this._teaHorseCaravanData.CaravanReplenishment = (short)Math.Clamp((int)(this._teaHorseCaravanData.CaravanReplenishment - this._caravanReplenishmentCostPerMonth - (short)TeaHorseCaravanWeather.Instance.GetItem(weatherTemplateId).ReplenishmentChange), 0, (int)this._caravanReplenishmentInitValue);
					bool flag12 = this._teaHorseCaravanData.CaravanReplenishment == 0;
					if (flag12)
					{
						TeaHorseCaravanData teaHorseCaravanData2 = this._teaHorseCaravanData;
						teaHorseCaravanData2.LackReplenishmentTurn += 1;
						MonthlyNotificationCollection monthlyNotifications2 = DomainManager.World.GetMonthlyNotificationCollection();
						monthlyNotifications2.AddWesternMerchanLackReplenishment();
					}
					else
					{
						this._teaHorseCaravanData.LackReplenishmentTurn = 0;
					}
					List<TeaHorseCaravanEventItem> eventList = new List<TeaHorseCaravanEventItem>();
					short k = 0;
					while ((int)k < TeaHorseCaravanEvent.Instance.Count)
					{
						eventList.Add(TeaHorseCaravanEvent.Instance.GetItem(k));
						k += 1;
					}
					IEnumerable<ValueTuple<short, short>> eventWeights = from a in eventList
					select new ValueTuple<short, short>(a.TemplateId, (short)a.Weighted);
					short eventTemplateId = RandomUtils.GetRandomResult<short>(eventWeights, context.Random);
					TeaHorseCaravanEventItem eventConfig = TeaHorseCaravanEvent.Instance.GetItem(eventTemplateId);
					bool flag13 = eventConfig.EventType == 0 && (eventConfig.ReplenishmentTrigger != 1 || this._teaHorseCaravanData.CaravanReplenishment != 0);
					if (flag13)
					{
						this._teaHorseCaravanData.IsShowExchangeReplenishment = true;
						this._teaHorseCaravanData.ExchangeReplenishmentAmountMax = (short)context.Random.Next((int)eventConfig.ExchangeMin, (int)(eventConfig.ExchangeMax + 1));
						this._teaHorseCaravanData.ExchangeReplenishmentRemainAmount = this._teaHorseCaravanData.ExchangeReplenishmentAmountMax;
						this._teaHorseCaravanData.DiaryList.Add(eventTemplateId);
					}
					else
					{
						bool flag14 = eventConfig.EventType == 1 && (eventConfig.ReplenishmentTrigger != 1 || this._teaHorseCaravanData.CaravanReplenishment != 0);
						if (flag14)
						{
							this._teaHorseCaravanData.IsShowSeachReplenishment = true;
							this._teaHorseCaravanData.SearchReplenishmentMax = (short)context.Random.Next((int)eventConfig.SearchMin, (int)(eventConfig.SearchMax + 1));
							this._teaHorseCaravanData.SearchReplenishmentAmount = (short)context.Random.Next((int)eventConfig.SolarSearchMin, (int)(eventConfig.SolarSearchMax + 1));
							this._teaHorseCaravanData.DiaryList.Add(eventTemplateId);
						}
						else
						{
							bool flag15 = (eventConfig.EventType == 2 || eventConfig.EventType == 3) && (eventConfig.ReplenishmentTrigger != 1 || this._teaHorseCaravanData.CaravanReplenishment != 0);
							if (flag15)
							{
								this._teaHorseCaravanData.CaravanAwareness = (short)Math.Max((int)(this._teaHorseCaravanData.CaravanAwareness + eventConfig.AwarenessChange), 0);
								this._teaHorseCaravanData.DiaryList.Add(eventTemplateId);
							}
							else
							{
								bool flag16 = eventConfig.EventType == 4 && (eventConfig.ReplenishmentTrigger != 1 || this._teaHorseCaravanData.CaravanReplenishment != 0);
								if (flag16)
								{
									sbyte grade = (sbyte)context.Random.Next((int)eventConfig.GetItemGradeMin, (int)(eventConfig.GetItemGradeMax + 1));
									ItemKey itemKey = this.GetWestRandomItemByGarde(context, grade);
									this._teaHorseCaravanData.ExchangeGoodsList.Add(itemKey);
									this._teaHorseCaravanData.DiaryList.Add(eventTemplateId);
								}
								else
								{
									bool flag17 = eventConfig.EventType == 5 && (eventConfig.ReplenishmentTrigger != 1 || this._teaHorseCaravanData.CaravanReplenishment != 0);
									if (flag17)
									{
										int lostNum = context.Random.Next((int)eventConfig.LoseGoodsNumMin, (int)(eventConfig.LoseGoodsNumMax + 1));
										for (int l = 0; l < lostNum; l++)
										{
											bool flag18 = context.Random.Next(2) == 0;
											if (flag18)
											{
												bool flag19 = this._teaHorseCaravanData.CarryGoodsList.Count > 0;
												if (flag19)
												{
													int index5 = context.Random.Next(this._teaHorseCaravanData.CarryGoodsList.Count);
													ItemKey lostItem3 = this._teaHorseCaravanData.CarryGoodsList[index5].Item1;
													this._teaHorseCaravanData.CarryGoodsList.RemoveAt(index5);
													DomainManager.Item.RemoveItem(context, lostItem3);
												}
												else
												{
													bool flag20 = this._teaHorseCaravanData.ExchangeGoodsList.Count > 0;
													if (flag20)
													{
														int index6 = context.Random.Next(this._teaHorseCaravanData.ExchangeGoodsList.Count);
														this._teaHorseCaravanData.ExchangeGoodsList.RemoveAt(index6);
													}
												}
											}
											else
											{
												bool flag21 = this._teaHorseCaravanData.ExchangeGoodsList.Count > 0;
												if (flag21)
												{
													int index7 = context.Random.Next(this._teaHorseCaravanData.ExchangeGoodsList.Count);
													this._teaHorseCaravanData.ExchangeGoodsList.RemoveAt(index7);
												}
												else
												{
													bool flag22 = this._teaHorseCaravanData.CarryGoodsList.Count > 0;
													if (flag22)
													{
														int index8 = context.Random.Next(this._teaHorseCaravanData.CarryGoodsList.Count);
														ItemKey lostItem4 = this._teaHorseCaravanData.CarryGoodsList[index8].Item1;
														this._teaHorseCaravanData.CarryGoodsList.RemoveAt(index8);
														DomainManager.Item.RemoveItem(context, lostItem4);
													}
												}
											}
										}
										this._teaHorseCaravanData.DiaryList.Add(eventTemplateId);
									}
									else
									{
										bool flag23 = (eventConfig.EventType == 6 || eventConfig.EventType == 7) && (eventConfig.ReplenishmentTrigger != 1 || this._teaHorseCaravanData.CaravanReplenishment != 0);
										if (flag23)
										{
											int replementChange = context.Random.Next((int)eventConfig.ReplenishmentChangeMin, (int)(eventConfig.ReplenishmentChangeMax + 1));
											this._teaHorseCaravanData.CaravanReplenishment = (short)Math.Clamp((int)this._teaHorseCaravanData.CaravanReplenishment + replementChange, 0, (int)this._caravanReplenishmentInitValue);
											this._teaHorseCaravanData.DiaryList.Add(eventTemplateId);
										}
									}
								}
							}
						}
					}
					MonthlyNotificationCollection monthlyNotification = DomainManager.World.GetMonthlyNotificationCollection();
					switch (eventConfig.TemplateId)
					{
					case 0:
						monthlyNotification.AddWesternMerchanFindMirage();
						break;
					case 1:
						monthlyNotification.AddWesternMerchanFindBigfoot();
						break;
					case 2:
						monthlyNotification.AddWesternMerchanFindAnimal();
						break;
					case 3:
						monthlyNotification.AddWesternMerchanFindPlant();
						break;
					case 4:
						DomainManager.Information.GiveOrUpgradeWesternRegionInformation(context, DomainManager.Taiwu.GetTaiwuCharId(), WesternRegion.Instance.GetAllKeys().GetRandom(context.Random));
						monthlyNotification.AddWesternMerchanGetInformation();
						break;
					case 5:
						monthlyNotification.AddWesternMerchanFindSettlement();
						break;
					case 6:
						monthlyNotification.AddWesternMerchanFindWeather();
						break;
					case 7:
						monthlyNotification.AddWesternMerchanLost();
						break;
					case 8:
						monthlyNotification.AddWesternMerchanMeetTheif();
						break;
					case 9:
						monthlyNotification.AddWesternMerchanGoodsDamage();
						break;
					case 10:
						monthlyNotification.AddWesternMerchanFindWreckage();
						break;
					case 11:
						monthlyNotification.AddWesternMerchanHelpPasserby();
						break;
					case 12:
						monthlyNotification.AddWesternMerchanUnacclimatized();
						break;
					case 13:
						monthlyNotification.AddWesternMerchanGetHelp();
						break;
					case 14:
						monthlyNotification.AddWesternMerchanFindVenison();
						break;
					case 15:
					case 16:
					case 17:
						monthlyNotification.AddWesternMerchanFindFruit();
						break;
					case 18:
					case 19:
					case 20:
						monthlyNotification.AddWesternMerchanFindVillage();
						break;
					case 21:
					case 22:
					case 23:
						monthlyNotification.AddWesternMerchanMeetMerchan();
						break;
					}
					bool flag24 = this._teaHorseCaravanData.CaravanState == 2 && this._teaHorseCaravanData.CaravanReplenishment != 0 && this._teaHorseCaravanData.CarryGoodsList.Count > 0;
					if (flag24)
					{
						int itemIndex = 0;
						ValueTuple<ItemKey, sbyte> itemKey2 = this._teaHorseCaravanData.CarryGoodsList[itemIndex];
						ItemKey exchangeItem = ItemKey.Invalid;
						int exchangeValue = ItemTemplateHelper.GetBaseValue(itemKey2.Item1.ItemType, itemKey2.Item1.TemplateId) + (int)this._teaHorseCaravanData.CaravanAwareness;
						int exchangeGrade = 0;
						for (int m = 0; m < this._westTreasureTemplateId.Count; m++)
						{
							int value = ItemTemplateHelper.GetBaseValue(12, this._westTreasureTemplateId[m]);
							bool flag25 = exchangeValue >= value;
							if (flag25)
							{
								exchangeGrade = m;
							}
						}
						exchangeGrade += 4;
						bool flag26 = exchangeGrade > 4;
						if (flag26)
						{
							bool flag27 = context.Random.CheckProb(40, 100);
							if (flag27)
							{
								exchangeItem = this.GetWestRandomItemByGarde(context, (sbyte)exchangeGrade);
							}
							else
							{
								exchangeItem = this.GetWestRandomItemByGarde(context, (sbyte)(exchangeGrade - 1));
							}
						}
						else
						{
							int baseValue = ItemTemplateHelper.GetBaseValue(12, this._westTreasureTemplateId[0]);
							int baseProb = ((int)this._teaHorseCaravanData.CaravanAwareness + exchangeValue) * 100 / baseValue - GlobalConfig.Instance.CaravanExchangeProbPenalize;
							int getProb = Math.Clamp(baseProb, 0, 100);
							bool flag28 = context.Random.CheckProb(getProb, 100);
							if (flag28)
							{
								exchangeItem = this.GetWestRandomItemByGarde(context, (sbyte)exchangeGrade);
							}
						}
						bool flag29 = !exchangeItem.Equals(ItemKey.Invalid);
						if (flag29)
						{
							this._teaHorseCaravanData.ExchangeGoodsList.Add(exchangeItem);
							this._teaHorseCaravanData.CarryGoodsList.RemoveAt(itemIndex);
						}
					}
					bool flag30 = this._teaHorseCaravanData.CaravanState == 2;
					if (flag30)
					{
						TeaHorseCaravanData teaHorseCaravanData3 = this._teaHorseCaravanData;
						teaHorseCaravanData3.DistanceToTaiwuVillage += 1;
					}
					else
					{
						bool flag31 = this._teaHorseCaravanData.CaravanState == 3;
						if (flag31)
						{
							this._teaHorseCaravanData.DistanceToTaiwuVillage = (short)Math.Max(0, (int)(this._teaHorseCaravanData.DistanceToTaiwuVillage - 1));
						}
					}
					bool flag32 = this._teaHorseCaravanData.CaravanState == 2;
					if (flag32)
					{
						this._teaHorseCaravanData.CaravanAwareness = (short)Math.Max(0, (int)(this._teaHorseCaravanData.CaravanAwareness + 100));
					}
					bool flag33 = this._teaHorseCaravanData.CarryGoodsList.Count == 0;
					if (flag33)
					{
						this._teaHorseCaravanData.CaravanState = 3;
					}
					bool flag34 = this._teaHorseCaravanData.CarryGoodsList.Count == 0 && this._teaHorseCaravanData.ExchangeGoodsList.Count == 0;
					if (flag34)
					{
						this.ResetTeaHorseCaravanData(context);
						MonthlyNotificationCollection monthlyNotifications3 = DomainManager.World.GetMonthlyNotificationCollection();
						monthlyNotifications3.AddWesternMerchantLoseContact();
					}
					bool flag35 = this._teaHorseCaravanData.CaravanState == 3 && this._teaHorseCaravanData.DistanceToTaiwuVillage == 0;
					if (flag35)
					{
						this._teaHorseCaravanData.CaravanState = 4;
						this._teaHorseCaravanData.IsShowExchangeReplenishment = false;
						this._teaHorseCaravanData.IsShowSeachReplenishment = false;
						this._teaHorseCaravanData.Terrain = 5;
						MonthlyNotificationCollection monthlyNotifications4 = DomainManager.World.GetMonthlyNotificationCollection();
						monthlyNotifications4.AddWesternMerchantBackSucceed();
					}
					this.SetTeaHorseCaravanData(this._teaHorseCaravanData, context);
					DomainManager.Building.UpgradeTeaHorseCaravanByAwareness(context);
				}
			}
		}

		// Token: 0x06007C3F RID: 31807 RVA: 0x004972E8 File Offset: 0x004954E8
		internal void CompleteBuilding(DataContext ctx, BuildingBlockKey blockKey, BuildingBlockData blockData)
		{
			blockData.Durability = blockData.ConfigData.MaxDurability;
			bool flag = blockData.TemplateId == 46;
			if (flag)
			{
				this.SetResidenceAutoCheckIn(ctx, blockKey.BuildingBlockIndex, true);
			}
			bool flag2 = blockData.TemplateId == 47;
			if (flag2)
			{
				this.SetComfortableAutoCheckIn(ctx, blockKey.BuildingBlockIndex, true);
				DomainManager.Extra.FeastSetAutoRefill(ctx, blockKey, true);
			}
			EBuildingBlockType type = blockData.ConfigData.Type;
			bool flag3 = type <= EBuildingBlockType.SpecialResource;
			bool flag4 = flag3;
			if (flag4)
			{
				DomainManager.Extra.SetResourceBlockExtraData(ctx, new ResourceBlockExtraData(blockKey));
			}
		}

		// Token: 0x06007C40 RID: 31808 RVA: 0x00497382 File Offset: 0x00495582
		private void OnBuildingRemoved(DataContext context, BuildingBlockKey blockKey)
		{
			DomainManager.Extra.TryRemoveResourceBlockExtraData(context, blockKey);
			DomainManager.Extra.TryRemoveBuildingArtisanOrder(context, blockKey);
		}

		// Token: 0x06007C41 RID: 31809 RVA: 0x004973A0 File Offset: 0x004955A0
		private void OfflineUpdateOperation(DataContext context, ParallelBuildingModification modification, short settlementId, BuildingBlockItem configData, BuildingBlockKey blockKey, BuildingBlockData blockData)
		{
			bool flag = blockData.OperationType == -1 || !this._buildingOperatorDict.ContainsKey(blockKey);
			if (!flag)
			{
				BuildingBlockDataEx blockDataEx = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)blockKey);
				List<int> charList = this._buildingOperatorDict[blockKey].GetCollection();
				int addProgress = 0;
				short maxProgress = configData.OperationTotalProgress[(int)blockData.OperationType];
				bool operationStopping = blockData.OperationStopping;
				if (operationStopping)
				{
					addProgress = (int)maxProgress;
				}
				else
				{
					addProgress += this.GetOperationSumValue(charList);
					bool flag2 = charList.Count == 0;
					if (flag2)
					{
						bool flag3 = blockData.OperationType == 0;
						if (flag3)
						{
							this.AddBuildingException(blockKey, blockData, BuildingExceptionType.BuildStoppedForWorkerShortage);
						}
					}
				}
				bool flag4 = addProgress <= 0;
				if (!flag4)
				{
					bool flag5 = maxProgress > 0;
					if (flag5)
					{
						blockData.OperationProgress = (short)Math.Clamp((int)blockData.OperationProgress + ((!blockData.OperationStopping) ? addProgress : (-addProgress)), 0, (int)maxProgress);
					}
					bool flag6 = blockData.OperationProgress != (blockData.OperationStopping ? 0 : maxProgress);
					if (!flag6)
					{
						modification.FreeOperator = true;
						MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
						sbyte operationType = blockData.OperationType;
						sbyte b = operationType;
						if (b != 0)
						{
							if (b == 1)
							{
								bool flag7 = !blockData.OperationStopping;
								if (flag7)
								{
									modification.RemoveMakeItemData = true;
									modification.RemoveEventBookData = true;
									modification.RemoveResidence = true;
									modification.FreeShopManager = true;
									sbyte level = DomainManager.Building.BuildingBlockLevel(blockKey);
									for (sbyte type = 0; type < 8; type += 1)
									{
										int returnCount = GameData.Domains.Building.SharedMethods.GetResourceReturnOfRemoveBuilding(configData, level, type, blockData, blockDataEx);
										modification.SetResource(type, returnCount);
									}
									bool flag8 = this._CollectBuildingResourceType.ContainsKey(blockKey);
									if (flag8)
									{
										modification.RemoveCollectResourceType = true;
									}
									modification.ResetAllChildrenBlocks = true;
									monthlyNotifications.AddBuildingDemolitionCompleted(settlementId, blockData.TemplateId);
									modification.BuildingOperationComplete = true;
									DomainManager.Extra.ResetBuildingExtraData(context, blockKey);
									this.OnBuildingRemoved(context, blockKey);
								}
							}
						}
						else
						{
							bool flag9 = !blockData.OperationStopping;
							if (flag9)
							{
								modification.AddBuilding = true;
								monthlyNotifications.AddBuildingConstructionCompleted(settlementId, blockData.TemplateId);
								this._newCompleteOperationBuildings.Add(blockKey);
								modification.BuildingOperationComplete = true;
								this.CompleteBuilding(context, blockKey, blockData);
							}
							else
							{
								modification.ResetAllChildrenBlocks = true;
								bool flag10 = GameData.Domains.Building.SharedMethods.NeedCostResourceToBuild(configData);
								if (flag10)
								{
									for (sbyte type2 = 0; type2 < 8; type2 += 1)
									{
										modification.SetResource(type2, (int)(configData.BaseBuildCost[(int)type2] / 2));
									}
								}
								bool flag11 = configData.BuildingCoreItem != -1;
								if (flag11)
								{
									DomainManager.Taiwu.ReturnBuildingCoreItem(DataContextManager.GetCurrentThreadDataContext(), configData);
								}
							}
						}
						blockData.OperationType = -1;
						blockData.OperationProgress = 0;
					}
				}
			}
		}

		// Token: 0x06007C42 RID: 31810 RVA: 0x00497688 File Offset: 0x00495888
		private void OfflineUpdateShopManagement(ParallelBuildingModification modification, short settlementId, BuildingBlockItem buildingBlockCfg, BuildingBlockKey blockKey, BuildingBlockData blockData, DataContext context)
		{
			BuildingDomain.<>c__DisplayClass117_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.blockKey = blockKey;
			CS$<>8__locals1.blockData = blockData;
			CS$<>8__locals1.modification = modification;
			CS$<>8__locals1.settlementId = settlementId;
			CS$<>8__locals1.buildingBlockCfg = buildingBlockCfg;
			bool flag = !this.HasShopManagerLeader(CS$<>8__locals1.blockKey);
			if (flag)
			{
				this.AddBuildingException(CS$<>8__locals1.blockKey, CS$<>8__locals1.blockData, BuildingExceptionType.ManageStoppedForNoLeader);
			}
			else
			{
				bool flag2 = this.ShopBuildingCanTeach(CS$<>8__locals1.blockKey) && this.HasShopManagerLeader(CS$<>8__locals1.blockKey);
				if (flag2)
				{
					this.UpdateShopBuildingTeach(context, CS$<>8__locals1.modification, CS$<>8__locals1.blockKey, CS$<>8__locals1.blockData);
				}
				CS$<>8__locals1.random = context.Random;
				bool flag3 = CS$<>8__locals1.blockData.TemplateId == 105;
				if (flag3)
				{
					BuildingEarningsData data;
					bool flag4 = !this._collectBuildingEarningsData.ContainsKey(CS$<>8__locals1.blockKey) || (this.TryGetElement_CollectBuildingEarningsData(CS$<>8__locals1.blockKey, out data) && data != null && data.FixBookInfoList.Count <= 0);
					if (!flag4)
					{
						ItemKey itemKey = data.FixBookInfoList[0];
						bool flag5 = !itemKey.IsValid();
						if (!flag5)
						{
							GameData.Domains.Item.SkillBook skillBook = DomainManager.Item.GetElement_SkillBooks(itemKey.Id);
							bool flag6 = !skillBook.CanFix();
							if (flag6)
							{
								CS$<>8__locals1.blockData.OfflineResetShopProgress();
							}
							else
							{
								short needProgress = skillBook.GetFixProgress().Item2;
								bool flag7 = CS$<>8__locals1.blockData.ShopProgress >= needProgress;
								if (flag7)
								{
									CS$<>8__locals1.blockData.OfflineResetShopProgress();
									ParallelBuildingModification modification2 = CS$<>8__locals1.modification;
									if (modification2.FixBookList == null)
									{
										modification2.FixBookList = new List<ItemKey>();
									}
									CS$<>8__locals1.modification.FixBookList.Add(itemKey);
								}
							}
						}
					}
				}
				else
				{
					sbyte level = GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(CS$<>8__locals1.blockData.TemplateId);
					BuildingEarningsData earningsData;
					bool flag8 = this.TryGetElement_CollectBuildingEarningsData(CS$<>8__locals1.blockKey, out earningsData) && earningsData != null && (int)level <= earningsData.RecruitLevelList.Count;
					if (!flag8)
					{
						bool flag9 = CS$<>8__locals1.buildingBlockCfg.IsShop && CS$<>8__locals1.buildingBlockCfg.SuccesEvent.Count > 0;
						if (flag9)
						{
							ShopEventItem successShopEventConfig = ShopEvent.Instance[CS$<>8__locals1.buildingBlockCfg.SuccesEvent[0]];
							List<sbyte> resourceList = successShopEventConfig.ResourceList;
							bool flag10 = resourceList != null && resourceList.Count > 0;
							if (flag10)
							{
								sbyte resourceType = this.GetCollectBuildingResourceType(CS$<>8__locals1.blockKey);
								sbyte getResourceType = (resourceType < 6) ? resourceType : 5;
								int amount = this.CalcResourceOutputCount(CS$<>8__locals1.blockKey, getResourceType);
								DomainManager.Taiwu.GetTaiwu().ChangeResource(context, getResourceType, amount);
								this.ApplyBuildingResourceOutputSetting(context, CS$<>8__locals1.blockKey, getResourceType, amount);
							}
						}
						bool flag11 = earningsData != null && (int)level <= earningsData.CollectionItemList.Count + earningsData.CollectionResourceList.Count;
						if (!flag11)
						{
							bool flag12 = CS$<>8__locals1.blockData.ShopProgress < CS$<>8__locals1.buildingBlockCfg.MaxProduceValue;
							if (!flag12)
							{
								CS$<>8__locals1.blockData.OfflineResetShopProgress();
								CS$<>8__locals1.date = DomainManager.World.GetCurrDate();
								CS$<>8__locals1.shopEventCollection = this.GetOrCreateShopEventCollection(CS$<>8__locals1.blockKey);
								CS$<>8__locals1.monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
								bool flag13 = CS$<>8__locals1.buildingBlockCfg.IsShop && CS$<>8__locals1.buildingBlockCfg.SuccesEvent.Count > 0;
								if (flag13)
								{
									ShopEventItem successShopEventConfig2 = ShopEvent.Instance[CS$<>8__locals1.buildingBlockCfg.SuccesEvent[0]];
									ShopEventItem failShopEventConfig = ShopEvent.Instance[CS$<>8__locals1.buildingBlockCfg.FailEvent[0]];
									bool flag14 = successShopEventConfig2.ItemList.Count > 0;
									if (flag14)
									{
										List<TemplateKey> itemProbList = ObjectPool<List<TemplateKey>>.Instance.Get();
										itemProbList.Clear();
										int from = 0;
										int end = successShopEventConfig2.ItemList.Count;
										sbyte resourceType2 = -1;
										bool flag15 = successShopEventConfig2.ResourceList.Count > 1;
										if (flag15)
										{
											resourceType2 = this.GetCollectBuildingResourceType(CS$<>8__locals1.blockKey);
											bool flag16 = resourceType2 == successShopEventConfig2.ResourceList[0];
											if (flag16)
											{
												end = successShopEventConfig2.ItemList.Count / 2;
											}
											else
											{
												from = successShopEventConfig2.ItemList.Count / 2;
											}
										}
										for (int i = from; i < end; i++)
										{
											bool hasManager;
											int prob = (successShopEventConfig2.ItemList[i].Amount + this.BuildingTotalAttainment(CS$<>8__locals1.blockKey, resourceType2, out hasManager, false) / 30) * this.BuildingProductivityByMaxDependencies(CS$<>8__locals1.blockKey) / 100;
											bool flag17 = hasManager && CS$<>8__locals1.random.CheckPercentProb(prob);
											if (flag17)
											{
												PresetInventoryItem item = successShopEventConfig2.ItemList[i];
												itemProbList.Add(new TemplateKey(item.Type, item.TemplateId));
											}
										}
										bool flag18 = itemProbList.Count > 0;
										if (flag18)
										{
											TemplateKey templateKey = itemProbList[CS$<>8__locals1.random.Next(0, itemProbList.Count)];
											CS$<>8__locals1.modification.AddCollectableEarning(templateKey);
											CS$<>8__locals1.shopEventCollection.AddCollectItemSuccessRecord(successShopEventConfig2, CS$<>8__locals1.date, templateKey.ItemType, templateKey.TemplateId);
											CS$<>8__locals1.monthlyNotifications.AddBuildingIncome(CS$<>8__locals1.settlementId, CS$<>8__locals1.buildingBlockCfg.TemplateId);
										}
										else
										{
											CS$<>8__locals1.shopEventCollection.AddFailureRecord(failShopEventConfig, CS$<>8__locals1.date);
										}
										ObjectPool<List<TemplateKey>>.Instance.Return(itemProbList);
									}
									bool flag19 = successShopEventConfig2.ItemGradeProbList.Count > 0;
									if (flag19)
									{
										List<sbyte> itemProbList2 = ObjectPool<List<sbyte>>.Instance.Get();
										itemProbList2.Clear();
										sbyte j = 0;
										while ((int)j < successShopEventConfig2.ItemGradeProbList.Count)
										{
											int prob2 = (int)successShopEventConfig2.ItemGradeProbList[(int)j] + this.GetBuildingAttainment(CS$<>8__locals1.blockData, CS$<>8__locals1.blockKey, true) / (int)this.AttainmentToProb;
											bool flag20 = CS$<>8__locals1.random.CheckPercentProb(prob2);
											if (flag20)
											{
												itemProbList2.Add(j);
											}
											j += 1;
										}
										bool flag21 = itemProbList2.Count > 0;
										if (flag21)
										{
											sbyte gradeLevel = itemProbList2[CS$<>8__locals1.random.Next(0, itemProbList2.Count)];
											ItemKey itemKey2 = this.GetRandomItemByGrade(CS$<>8__locals1.random, gradeLevel, -1);
											TemplateKey templateKey2 = new TemplateKey(itemKey2.ItemType, itemKey2.TemplateId);
											CS$<>8__locals1.modification.AddCollectableEarning(templateKey2);
											CS$<>8__locals1.shopEventCollection.AddManageEclecticBuildingSuccess6(CS$<>8__locals1.date, templateKey2.ItemType, templateKey2.TemplateId, 6, ItemTemplateHelper.GetBaseValue(templateKey2.ItemType, templateKey2.TemplateId));
										}
										else
										{
											CS$<>8__locals1.shopEventCollection.AddFailureRecord(failShopEventConfig, CS$<>8__locals1.date);
										}
									}
									bool flag22 = successShopEventConfig2.ResourceGoods != -1;
									if (flag22)
									{
										sbyte resourceType3;
										int amount2;
										BuildingProduceDependencyData buildingProduceDependencyData;
										bool shopExist = this.TryCalcShopManagementYieldAmount(CS$<>8__locals1.random, CS$<>8__locals1.blockKey, out resourceType3, out amount2, out buildingProduceDependencyData, 0);
										bool flag23 = shopExist;
										if (flag23)
										{
											ParallelBuildingModification modification2 = CS$<>8__locals1.modification;
											if (modification2.BuildingMoneyPrestigeSuccessRateCompensationChanged == null)
											{
												modification2.BuildingMoneyPrestigeSuccessRateCompensationChanged = new Dictionary<BuildingBlockKey, int>();
											}
											CS$<>8__locals1.modification.BuildingMoneyPrestigeSuccessRateCompensationChanged[CS$<>8__locals1.blockKey] = 0;
											bool flag24 = resourceType3 >= 0 && amount2 > 0;
											if (flag24)
											{
												CS$<>8__locals1.modification.AddCollectableResources(resourceType3, amount2);
												CS$<>8__locals1.shopEventCollection.AddCollectResourceSuccessRecord(successShopEventConfig2, CS$<>8__locals1.date, resourceType3, amount2);
												CS$<>8__locals1.monthlyNotifications.AddBuildingIncome(CS$<>8__locals1.settlementId, CS$<>8__locals1.buildingBlockCfg.TemplateId);
												List<int> characterList = DomainManager.Building.GetElement_ShopManagerDict(CS$<>8__locals1.blockKey).GetCollection();
												for (int k = 0; k < characterList.Count; k++)
												{
													int charId = characterList[k];
													GameData.Domains.Character.Character character;
													bool flag25 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
													if (!flag25)
													{
														bool flag26 = character.GetAgeGroup() != 2;
														if (!flag26)
														{
															int count = amount2 * (int)GlobalConfig.Instance.ShopBuildingSharePencent[(k == 0) ? 0 : 1] / 100;
															CS$<>8__locals1.modification.AddShopSalaryResources(charId, resourceType3, count);
														}
													}
												}
											}
										}
									}
									bool flag27 = successShopEventConfig2.RecruitPeopleProb.Count > 0;
									if (flag27)
									{
										BuildingBlockDataEx extraData = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)CS$<>8__locals1.blockKey);
										int peopleLevel = this.CalcRecruitGrade(CS$<>8__locals1.random, CS$<>8__locals1.blockKey, ref extraData.CumulatedScore);
										ParallelBuildingModification modification2 = CS$<>8__locals1.modification;
										if (modification2.RecruitLevelList == null)
										{
											modification2.RecruitLevelList = new List<IntPair>();
										}
										CS$<>8__locals1.modification.RecruitLevelList.Add(new IntPair(peopleLevel, 0));
										InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotificationCollection();
										instantNotifications.AddCandidateArrived(CS$<>8__locals1.settlementId, CS$<>8__locals1.blockData.TemplateId);
										bool flag28 = CS$<>8__locals1.blockData.TemplateId == 223;
										if (flag28)
										{
											CS$<>8__locals1.shopEventCollection.AddRecruitWithCostSuccessRecord(successShopEventConfig2, CS$<>8__locals1.date, 7, (int)GlobalConfig.Instance.RecruitPeopleCost);
										}
										else
										{
											CS$<>8__locals1.shopEventCollection.AddRecruitSuccessRecord(successShopEventConfig2, CS$<>8__locals1.date);
										}
									}
									BuildingEarningsData data2;
									bool flag29 = successShopEventConfig2.ExchangeResourceGoods != -1 && this.TryGetElement_CollectBuildingEarningsData(CS$<>8__locals1.blockKey, out data2);
									if (flag29)
									{
										int count2 = 0;
										for (int l = 0; l < data2.ShopSoldItemList.Count; l++)
										{
											bool flag30 = data2.ShopSoldItemList[l].TemplateId == -1 || !CS$<>8__locals1.random.CheckProb(50, 100);
											if (!flag30)
											{
												this.<OfflineUpdateShopManagement>g__SellItemSuccess|117_0(successShopEventConfig2, earningsData, l, ref CS$<>8__locals1);
												count2++;
											}
										}
										bool flag31 = count2 == 0;
										if (flag31)
										{
											for (int m = 0; m < data2.ShopSoldItemList.Count; m++)
											{
												bool flag32 = data2.ShopSoldItemList[m].TemplateId == -1;
												if (!flag32)
												{
													this.<OfflineUpdateShopManagement>g__SellItemSuccess|117_0(successShopEventConfig2, earningsData, m, ref CS$<>8__locals1);
													break;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007C43 RID: 31811 RVA: 0x004980F4 File Offset: 0x004962F4
		private unsafe void UpdateShopBuildingTeach(DataContext context, ParallelBuildingModification modification, BuildingBlockKey blockKey, BuildingBlockData blockData)
		{
			CharacterList managerList = this.GetElement_ShopManagerDict(blockKey);
			bool flag = managerList.GetRealCount() <= 1;
			if (!flag)
			{
				ShopEventCollection shopEventCollection = this.GetOrCreateShopEventCollection(blockKey);
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int date = DomainManager.World.GetCurrDate();
				short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
				BuildingBlockItem buildingConfig = BuildingBlock.Instance[blockData.TemplateId];
				CValuePercentBonus buildingProbBonus = (buildingConfig.RequireLifeSkillType >= 0) ? this.GetBuildingBlockEffect(settlementId, EBuildingScaleEffect.ShopManagerQualificationImproveRate, (int)buildingConfig.RequireLifeSkillType) : 0;
				int leaderId = managerList[0];
				GameData.Domains.Character.Character leaderCharacter = DomainManager.Character.GetElement_Objects(leaderId);
				for (int index = 1; index < 7; index++)
				{
					int memberId = managerList[index];
					bool flag2 = memberId < 0;
					if (!flag2)
					{
						GameData.Domains.Character.Character memberCharacter = DomainManager.Character.GetElement_Objects(memberId);
						sbyte alreadyAddCount;
						DomainManager.Extra.TryGetElement_TaiwuVillagerPotentialData(memberId, out alreadyAddCount);
						sbyte requirePersonalityValue = *memberCharacter.GetPersonalities()[(int)buildingConfig.RequirePersonalityType];
						sbyte skillType = (buildingConfig.RequireLifeSkillType >= 0) ? buildingConfig.RequireLifeSkillType : buildingConfig.RequireCombatSkillType;
						bool flag3 = alreadyAddCount < GlobalConfig.Instance.TaiwuVillagerMaxPotential;
						if (flag3)
						{
							bool flag4 = alreadyAddCount != 0 && alreadyAddCount % 4 == 0;
							if (flag4)
							{
								DomainManager.Extra.TrySetShopVillagerQualificationImprove(memberCharacter, skillType, buildingConfig.RequireLifeSkillType >= 0);
								bool flag5 = buildingConfig.RequireLifeSkillType >= 0;
								if (flag5)
								{
									shopEventCollection.AddBaseDevelopLifeSkill(date, memberId, skillType);
									lifeRecordCollection.AddShopBuildingBaseDevelopLifeSkill(memberId, date, buildingConfig.TemplateId, skillType);
								}
								else
								{
									shopEventCollection.AddBaseDevelopCombatSkill(date, memberId, skillType);
									lifeRecordCollection.AddShopBuildingBaseDevelopCombatSkill(memberId, date, buildingConfig.TemplateId, skillType);
								}
							}
							int personalityAddProb = (int)(requirePersonalityValue / 3) * buildingProbBonus;
							bool flag6 = context.Random.CheckPercentProb(personalityAddProb);
							if (flag6)
							{
								DomainManager.Extra.TrySetShopVillagerQualificationImprove(memberCharacter, skillType, buildingConfig.RequireLifeSkillType >= 0);
								bool flag7 = buildingConfig.RequireLifeSkillType >= 0;
								if (flag7)
								{
									shopEventCollection.AddPersonalityDevelopLifeSkill(date, memberId, skillType);
									lifeRecordCollection.AddShopBuildingPersonalityDevelopLifeSkill(memberId, date, buildingConfig.TemplateId, skillType);
								}
								else
								{
									shopEventCollection.AddPersonalityDevelopCombatSkill(date, memberId, skillType);
									lifeRecordCollection.AddShopBuildingPersonalityDevelopCombatSkill(memberId, date, buildingConfig.TemplateId, skillType);
								}
							}
							short leaderQualification = (buildingConfig.RequireLifeSkillType >= 0) ? leaderCharacter.GetLifeSkillQualification(buildingConfig.RequireLifeSkillType) : leaderCharacter.GetLifeSkillQualification(buildingConfig.RequireCombatSkillType);
							short memberQualification = (buildingConfig.RequireLifeSkillType >= 0) ? memberCharacter.GetLifeSkillQualification(buildingConfig.RequireLifeSkillType) : memberCharacter.GetLifeSkillQualification(buildingConfig.RequireCombatSkillType);
							int qualificationAddProb = (int)((leaderQualification - memberQualification) * 3) * buildingProbBonus;
							bool flag8 = context.Random.CheckPercentProb(qualificationAddProb);
							if (flag8)
							{
								DomainManager.Extra.TrySetShopVillagerQualificationImprove(memberCharacter, skillType, buildingConfig.RequireLifeSkillType >= 0);
								bool flag9 = buildingConfig.RequireLifeSkillType >= 0;
								if (flag9)
								{
									shopEventCollection.AddLeaderDevelopLifeSkill(date, memberId, leaderId, skillType);
									lifeRecordCollection.AddShopBuildingLeaderDevelopLifeSkill(memberId, date, leaderId, buildingConfig.TemplateId, skillType);
								}
								else
								{
									shopEventCollection.AddLeaderDevelopCombatSkill(date, memberId, leaderId, skillType);
									lifeRecordCollection.AddShopBuildingLeaderDevelopCombatSkill(memberId, date, leaderId, buildingConfig.TemplateId, skillType);
								}
							}
							DomainManager.Extra.UpdateTaiwuVillagerPotentialData(context, memberId, alreadyAddCount += 1);
						}
						short memberAttainment = (buildingConfig.RequireLifeSkillType >= 0) ? memberCharacter.GetLifeSkillAttainment(buildingConfig.RequireLifeSkillType) : memberCharacter.GetCombatSkillAttainment(buildingConfig.RequireCombatSkillType);
						int point = (int)(memberAttainment * (short)(100 + requirePersonalityValue) / 100);
						bool flag10 = buildingConfig.RequireLifeSkillType >= 0;
						if (flag10)
						{
							bool flag11;
							List<ValueTuple<short, byte>> readBooks = this.ShopBuildingTeachLifeSkillBook(leaderCharacter, memberCharacter, point, blockData.TemplateId, out flag11);
							foreach (ValueTuple<short, byte> valueTuple in readBooks)
							{
								short skillTemplateId = valueTuple.Item1;
								byte pageId = valueTuple.Item2;
								modification.AddLearnLifeSkill(memberId, skillTemplateId, pageId);
								Config.LifeSkillItem skillConfig = LifeSkill.Instance[skillTemplateId];
								SkillBookItem itemConfig = Config.SkillBook.Instance[skillConfig.SkillBookId];
								lifeRecordCollection.AddShopBuildingLearnLifeSkill(memberId, date, buildingConfig.TemplateId, itemConfig.ItemType, itemConfig.TemplateId, (int)(pageId + 1));
								shopEventCollection.AddLearnLifeSkill(date, memberId, itemConfig.ItemType, itemConfig.TemplateId, (int)(pageId + 1));
							}
						}
						else
						{
							bool flag11;
							List<ValueTuple<short, byte, sbyte>> readBooks2 = this.ShopBuildingTeachCombatSkillBook(leaderCharacter, memberCharacter, point, blockData.TemplateId, out flag11);
							foreach (ValueTuple<short, byte, sbyte> valueTuple2 in readBooks2)
							{
								short skillTemplateId2 = valueTuple2.Item1;
								byte pageInternalIndex = valueTuple2.Item2;
								modification.AddLearnCombatSkill(memberId, skillTemplateId2, pageInternalIndex);
								CombatSkillItem skillConfig2 = Config.CombatSkill.Instance[skillTemplateId2];
								SkillBookItem itemConfig2 = Config.SkillBook.Instance[skillConfig2.BookId];
								byte pageId2 = CombatSkillStateHelper.GetPageId(pageInternalIndex);
								lifeRecordCollection.AddShopBuildingLearnCombatSkill(memberId, date, buildingConfig.TemplateId, itemConfig2.ItemType, itemConfig2.TemplateId, (int)(pageId2 + 1));
								shopEventCollection.AddLearnCombatSkill(date, memberId, itemConfig2.ItemType, itemConfig2.TemplateId, (int)(pageId2 + 1));
							}
						}
					}
				}
			}
		}

		// Token: 0x06007C44 RID: 31812 RVA: 0x00498674 File Offset: 0x00496874
		[Obsolete]
		[return: TupleElementNames(new string[]
		{
			"skillTemplateId",
			"pageInternalIndex"
		})]
		private unsafe ValueTuple<short, byte> ShopManagerSelectCombatSkillToLearn(IRandomSource random, GameData.Domains.Character.Character character, sbyte combatSkillType, sbyte maxGrade)
		{
			int charId = character.GetId();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> combatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
			sbyte minLearnableGrade = sbyte.MaxValue;
			sbyte bestLearnableSectPriority = sbyte.MinValue;
			GameData.Domains.CombatSkill.CombatSkill learnableCombatSkill = null;
			sbyte minUnlearnedGrade = sbyte.MaxValue;
			sbyte bestUnlearnedSectPriority = sbyte.MinValue;
			CombatSkillItem unlearnedCombatSkill = null;
			bool learnRandomGrade = random.CheckPercentProb((int)GlobalConfig.Instance.ShopManagerLearnRandomGradeChance);
			BuildingDomain.<>c__DisplayClass119_0 CS$<>8__locals1;
			CS$<>8__locals1.idealSectId = character.GetIdealSect();
			CS$<>8__locals1.stateSectId = DomainManager.Taiwu.GetTaiwuVillageStateSect();
			CS$<>8__locals1.behaviorType = character.GetBehaviorType();
			foreach (CombatSkillItem combatSkillCfg in ((IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance))
			{
				bool flag = combatSkillCfg.Type != combatSkillType;
				if (!flag)
				{
					bool flag2 = combatSkillCfg.BookId < 0;
					if (!flag2)
					{
						bool flag3 = combatSkillCfg.Grade > maxGrade;
						if (!flag3)
						{
							bool flag4 = combatSkillCfg.SectId < 0;
							if (!flag4)
							{
								sbyte currSectPriority = BuildingDomain.<ShopManagerSelectCombatSkillToLearn>g__GetSectPriority|119_0(combatSkillCfg.SectId, ref CS$<>8__locals1);
								GameData.Domains.CombatSkill.CombatSkill combatSkill;
								bool flag5 = combatSkills.TryGetValue(combatSkillCfg.TemplateId, out combatSkill);
								if (flag5)
								{
									bool flag6 = combatSkill.GetActivationState() > 0;
									if (!flag6)
									{
										ushort readingState = combatSkill.GetReadingState();
										bool flag7 = !CombatSkillStateHelper.HasReadOutlinePages(readingState) || !CombatSkillStateHelper.IsReadNormalPagesMeetConditionOfBreakout(readingState);
										if (!flag7)
										{
											bool flag8 = minLearnableGrade > combatSkillCfg.Grade;
											if (flag8)
											{
												bestLearnableSectPriority = BuildingDomain.<ShopManagerSelectCombatSkillToLearn>g__GetSectPriority|119_0(combatSkillCfg.SectId, ref CS$<>8__locals1);
												minLearnableGrade = combatSkillCfg.Grade;
												learnableCombatSkill = combatSkill;
											}
											else
											{
												bool flag9 = minLearnableGrade == combatSkillCfg.Grade && currSectPriority > bestLearnableSectPriority;
												if (flag9)
												{
													bestLearnableSectPriority = currSectPriority;
													learnableCombatSkill = combatSkill;
												}
											}
										}
									}
								}
								else
								{
									bool flag10 = minUnlearnedGrade > combatSkillCfg.Grade;
									if (flag10)
									{
										minUnlearnedGrade = combatSkillCfg.Grade;
										bestUnlearnedSectPriority = currSectPriority;
										unlearnedCombatSkill = combatSkillCfg;
									}
									else
									{
										bool flag11 = minUnlearnedGrade == combatSkillCfg.Grade && currSectPriority > bestUnlearnedSectPriority;
										if (flag11)
										{
											bestUnlearnedSectPriority = currSectPriority;
											unlearnedCombatSkill = combatSkillCfg;
										}
									}
								}
							}
						}
					}
				}
			}
			bool flag12 = learnableCombatSkill != null;
			ValueTuple<short, byte> result;
			if (flag12)
			{
				ushort readingState2 = learnableCombatSkill.GetReadingState();
				bool flag13 = !CombatSkillStateHelper.HasReadOutlinePages(readingState2);
				if (flag13)
				{
					byte outlinePageIndex = (byte)random.Next(5);
					result = new ValueTuple<short, byte>(learnableCombatSkill.GetId().SkillTemplateId, outlinePageIndex);
				}
				else
				{
					Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)15], 15);
					SpanList<byte> unreadPages = span;
					for (int i = 0; i < 5; i++)
					{
						byte directIndex = (byte)(5 + i);
						bool flag14 = ((int)readingState2 & 1 << (int)directIndex) == 0;
						if (flag14)
						{
							unreadPages.Add(directIndex);
						}
						byte reverseIndex = directIndex + 5;
						bool flag15 = ((int)readingState2 & 1 << (int)reverseIndex) == 0;
						if (flag15)
						{
							unreadPages.Add(reverseIndex);
						}
						unreadPages.Add((byte)(i + 1));
					}
					byte pageInternalIndex = unreadPages.GetRandom(random);
					result = new ValueTuple<short, byte>(learnableCombatSkill.GetId().SkillTemplateId, pageInternalIndex);
				}
			}
			else
			{
				bool flag16 = unlearnedCombatSkill != null;
				if (flag16)
				{
					short combatSkillTemplateId = unlearnedCombatSkill.TemplateId;
					byte outlinePageIndex2 = (byte)random.Next(5);
					result = new ValueTuple<short, byte>(combatSkillTemplateId, outlinePageIndex2);
				}
				else
				{
					result = new ValueTuple<short, byte>(-1, 0);
				}
			}
			return result;
		}

		// Token: 0x06007C45 RID: 31813 RVA: 0x004989C8 File Offset: 0x00496BC8
		[Obsolete]
		[return: TupleElementNames(new string[]
		{
			"skillTemplateId",
			"pageId"
		})]
		private unsafe ValueTuple<short, byte> ShopManagerSelectLifeSkillToLearn(IRandomSource random, GameData.Domains.Character.Character character, sbyte lifeSkillType, sbyte maxGrade)
		{
			List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = character.GetLearnedLifeSkills();
			sbyte currMinGrade = sbyte.MaxValue;
			int currSelectedIndex = -1;
			bool learnRandomGrade = random.CheckPercentProb((int)GlobalConfig.Instance.ShopManagerLearnRandomGradeChance);
			List<int> validSkillIndices = null;
			bool flag = learnRandomGrade;
			if (flag)
			{
				validSkillIndices = ObjectPool<List<int>>.Instance.Get();
				validSkillIndices.Clear();
			}
			BoolArray16 gradesLearned = default(BoolArray16);
			int i = 0;
			int length = learnedLifeSkills.Count;
			while (i < length)
			{
				GameData.Domains.Character.LifeSkillItem learnedLifeSkill = learnedLifeSkills[i];
				Config.LifeSkillItem lifeSkillCfg = LifeSkill.Instance[learnedLifeSkill.SkillTemplateId];
				bool flag2 = lifeSkillCfg.Type != lifeSkillType;
				if (!flag2)
				{
					bool flag3 = lifeSkillCfg.Grade > maxGrade;
					if (!flag3)
					{
						gradesLearned.Set((int)lifeSkillCfg.Grade, true);
						bool flag4 = learnedLifeSkill.IsAllPagesRead();
						if (!flag4)
						{
							bool flag5 = validSkillIndices != null;
							if (flag5)
							{
								validSkillIndices.Add(i);
							}
							else
							{
								bool flag6 = currMinGrade > lifeSkillCfg.Grade;
								if (flag6)
								{
									currMinGrade = lifeSkillCfg.Grade;
									currSelectedIndex = i;
								}
							}
						}
					}
				}
				i++;
			}
			bool flag7 = validSkillIndices != null;
			if (flag7)
			{
				bool flag8 = validSkillIndices.Count > 0;
				if (flag8)
				{
					currSelectedIndex = validSkillIndices.GetRandom(random);
				}
				ObjectPool<List<int>>.Instance.Return(validSkillIndices);
			}
			bool flag9 = currSelectedIndex >= 0;
			if (flag9)
			{
				GameData.Domains.Character.LifeSkillItem lifeSkillItem = learnedLifeSkills[currSelectedIndex];
				Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)5], 5);
				SpanList<byte> unlearnedPages = span;
				for (byte pageId = 0; pageId < 5; pageId += 1)
				{
					bool flag10 = !lifeSkillItem.IsPageRead(pageId);
					if (flag10)
					{
						unlearnedPages.Add(pageId);
					}
				}
				bool flag11 = unlearnedPages.Count > 0;
				if (flag11)
				{
					return new ValueTuple<short, byte>(lifeSkillItem.SkillTemplateId, unlearnedPages.GetRandom(random));
				}
			}
			else
			{
				bool flag12 = learnRandomGrade;
				if (flag12)
				{
					Span<sbyte> span2 = new Span<sbyte>(stackalloc byte[(UIntPtr)maxGrade], (int)maxGrade);
					SpanList<sbyte> unlearnedGrades = span2;
					for (sbyte grade = 0; grade < maxGrade; grade += 1)
					{
						bool flag13 = !gradesLearned.Get((int)grade);
						if (flag13)
						{
							unlearnedGrades.Add(grade);
						}
					}
					bool flag14 = unlearnedGrades.Count > 0;
					if (flag14)
					{
						sbyte grade2 = unlearnedGrades.GetRandom(random);
						short skillTemplateId = Config.LifeSkillType.Instance[lifeSkillType].SkillList[(int)grade2];
						return new ValueTuple<short, byte>(skillTemplateId, (byte)random.Next(5));
					}
				}
				else
				{
					for (sbyte grade3 = 0; grade3 < maxGrade; grade3 += 1)
					{
						bool flag15 = gradesLearned.Get((int)grade3);
						if (!flag15)
						{
							short skillTemplateId2 = Config.LifeSkillType.Instance[lifeSkillType].SkillList[(int)grade3];
							return new ValueTuple<short, byte>(skillTemplateId2, (byte)random.Next(5));
						}
					}
				}
			}
			return new ValueTuple<short, byte>(-1, 0);
		}

		// Token: 0x06007C46 RID: 31814 RVA: 0x00498CA4 File Offset: 0x00496EA4
		[return: TupleElementNames(new string[]
		{
			"skillTemplateId",
			"pageId"
		})]
		private List<ValueTuple<short, byte>> ShopBuildingTeachLifeSkillBook(GameData.Domains.Character.Character leader, GameData.Domains.Character.Character member, int point, short buildingTemplateId, out bool hasMemberUnread)
		{
			List<ValueTuple<short, byte>> result = new List<ValueTuple<short, byte>>();
			BuildingBlockItem buildingConfig = BuildingBlock.Instance[buildingTemplateId];
			List<GameData.Domains.Character.LifeSkillItem> leaderLearnedLifeSkills = leader.GetLearnedLifeSkills();
			List<GameData.Domains.Character.LifeSkillItem> leaderCanTeach = leaderLearnedLifeSkills.Where(delegate(GameData.Domains.Character.LifeSkillItem a)
			{
				Config.LifeSkillItem config = LifeSkill.Instance[a.SkillTemplateId];
				return config.Type == buildingConfig.RequireLifeSkillType;
			}).ToList<GameData.Domains.Character.LifeSkillItem>();
			leaderCanTeach.Sort(delegate(GameData.Domains.Character.LifeSkillItem a, GameData.Domains.Character.LifeSkillItem b)
			{
				sbyte aGrade = LifeSkill.Instance[a.SkillTemplateId].Grade;
				sbyte bGrade = LifeSkill.Instance[b.SkillTemplateId].Grade;
				return aGrade.CompareTo(bGrade);
			});
			List<GameData.Domains.Character.LifeSkillItem> memberLearnedLifeSkills = member.GetLearnedLifeSkills();
			List<short> memberLearnedLifeSkillTemplateIds = (from a in memberLearnedLifeSkills
			select a.SkillTemplateId).ToList<short>();
			List<byte> unreadPages = new List<byte>();
			hasMemberUnread = false;
			for (int i = 0; i < leaderCanTeach.Count; i++)
			{
				GameData.Domains.Character.LifeSkillItem skillItem = leaderCanTeach[i];
				Config.LifeSkillItem skillCfg = LifeSkill.Instance[skillItem.SkillTemplateId];
				int pageCost = (int)SkillGradeData.Instance[skillCfg.Grade].ReadingAttainmentRequirement;
				unreadPages.Clear();
				bool flag = !memberLearnedLifeSkillTemplateIds.Contains(skillItem.SkillTemplateId);
				if (flag)
				{
					for (byte pageId = 0; pageId < 5; pageId += 1)
					{
						bool flag2 = skillItem.IsPageRead(pageId);
						if (flag2)
						{
							unreadPages.Add(pageId);
						}
					}
				}
				else
				{
					GameData.Domains.Character.LifeSkillItem memberSkillItem = memberLearnedLifeSkills.Find((GameData.Domains.Character.LifeSkillItem a) => a.SkillTemplateId == skillItem.SkillTemplateId);
					for (byte pageId2 = 0; pageId2 < 5; pageId2 += 1)
					{
						bool flag3 = skillItem.IsPageRead(pageId2) && !memberSkillItem.IsPageRead(pageId2);
						if (flag3)
						{
							unreadPages.Add(pageId2);
						}
					}
				}
				hasMemberUnread |= (unreadPages.Count > 0);
				for (int j = 0; j < unreadPages.Count; j++)
				{
					point -= pageCost;
					bool flag4 = point >= 0;
					if (!flag4)
					{
						break;
					}
					result.Add(new ValueTuple<short, byte>(skillItem.SkillTemplateId, unreadPages[j]));
				}
			}
			return result;
		}

		// Token: 0x06007C47 RID: 31815 RVA: 0x00498EF0 File Offset: 0x004970F0
		[return: TupleElementNames(new string[]
		{
			"skillTemplateId",
			"pageInternalIndex",
			"direct"
		})]
		private List<ValueTuple<short, byte, sbyte>> ShopBuildingTeachCombatSkillBook(GameData.Domains.Character.Character leader, GameData.Domains.Character.Character member, int point, short buildingTemplateId, out bool hasMemberUnread)
		{
			List<ValueTuple<short, byte, sbyte>> result = new List<ValueTuple<short, byte, sbyte>>();
			BuildingBlockItem buildingConfig = BuildingBlock.Instance[buildingTemplateId];
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> leaderLearnedDict = DomainManager.CombatSkill.GetCharCombatSkills(leader.GetId());
			List<short> leaderLearnedTemplateIdList = leaderLearnedDict.Keys.ToList<short>();
			List<short> leaderCanTeach = leaderLearnedTemplateIdList.Where(delegate(short combatSkillTemplateId)
			{
				CombatSkillItem config = Config.CombatSkill.Instance[combatSkillTemplateId];
				return config.Type == buildingConfig.RequireCombatSkillType;
			}).ToList<short>();
			leaderCanTeach.Sort(delegate(short a, short b)
			{
				sbyte aGrade = Config.CombatSkill.Instance[a].Grade;
				sbyte bGrade = Config.CombatSkill.Instance[b].Grade;
				return aGrade.CompareTo(bGrade);
			});
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> memberLearnedDict = DomainManager.CombatSkill.GetCharCombatSkills(member.GetId());
			List<short> memberLearnedTemplateIdList = memberLearnedDict.Keys.ToList<short>();
			List<ValueTuple<byte, sbyte>> unreadPages = new List<ValueTuple<byte, sbyte>>();
			hasMemberUnread = false;
			for (int i = 0; i < leaderCanTeach.Count; i++)
			{
				short skillTemplateId = leaderCanTeach[i];
				CombatSkillItem skillCfg = Config.CombatSkill.Instance[skillTemplateId];
				int pageCost = (int)SkillGradeData.Instance[skillCfg.Grade].ReadingAttainmentRequirement;
				unreadPages.Clear();
				GameData.Domains.CombatSkill.CombatSkill leaderSkillItem = leaderLearnedDict[skillTemplateId];
				ushort leaderReadingState = leaderSkillItem.GetReadingState();
				bool flag = !memberLearnedTemplateIdList.Contains(skillTemplateId);
				if (flag)
				{
					for (int j = 0; j < 5; j++)
					{
						byte outlineIndex = (byte)j;
						bool flag2 = ((int)leaderReadingState & 1 << (int)outlineIndex) != 0;
						if (flag2)
						{
							unreadPages.Add(new ValueTuple<byte, sbyte>(outlineIndex, -1));
						}
						byte directIndex = (byte)(5 + j);
						bool flag3 = ((int)leaderReadingState & 1 << (int)directIndex) != 0;
						if (flag3)
						{
							unreadPages.Add(new ValueTuple<byte, sbyte>(directIndex, 0));
						}
						byte reverseIndex = directIndex + 5;
						bool flag4 = ((int)leaderReadingState & 1 << (int)reverseIndex) != 0;
						if (flag4)
						{
							unreadPages.Add(new ValueTuple<byte, sbyte>(reverseIndex, 1));
						}
					}
				}
				else
				{
					GameData.Domains.CombatSkill.CombatSkill memberSkillItem = memberLearnedDict[skillTemplateId];
					ushort memberReadingState = memberSkillItem.GetReadingState();
					for (int k = 0; k < 5; k++)
					{
						byte outlineIndex2 = (byte)k;
						bool flag5 = ((int)leaderReadingState & 1 << (int)outlineIndex2) != 0 && ((int)memberReadingState & 1 << (int)outlineIndex2) == 0;
						if (flag5)
						{
							unreadPages.Add(new ValueTuple<byte, sbyte>(outlineIndex2, -1));
						}
						byte directIndex2 = (byte)(5 + k);
						bool flag6 = ((int)leaderReadingState & 1 << (int)directIndex2) != 0 && ((int)memberReadingState & 1 << (int)directIndex2) == 0;
						if (flag6)
						{
							unreadPages.Add(new ValueTuple<byte, sbyte>(directIndex2, 0));
						}
						byte reverseIndex2 = directIndex2 + 5;
						bool flag7 = ((int)leaderReadingState & 1 << (int)reverseIndex2) != 0 && ((int)memberReadingState & 1 << (int)reverseIndex2) == 0;
						if (flag7)
						{
							unreadPages.Add(new ValueTuple<byte, sbyte>(reverseIndex2, 1));
						}
					}
				}
				hasMemberUnread |= (unreadPages.Count > 0);
				for (int l = 0; l < unreadPages.Count; l++)
				{
					point -= pageCost;
					bool flag8 = point >= 0;
					if (!flag8)
					{
						break;
					}
					result.Add(new ValueTuple<short, byte, sbyte>(skillTemplateId, unreadPages[l].Item1, unreadPages[l].Item2));
				}
			}
			return result;
		}

		// Token: 0x06007C48 RID: 31816 RVA: 0x00499220 File Offset: 0x00497420
		private BuildingBlockKey GetBuildingInRange(BuildingBlockKey blockKey, sbyte width, int range, short targetBuildingTemplateId)
		{
			List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
			BuildingAreaData areaData = this.GetElement_BuildingAreas(new Location(blockKey.AreaId, blockKey.BlockId));
			areaData.GetNeighborBlocks(blockKey.BuildingBlockIndex, width, neighborList, null, range);
			BuildingBlockKey result = BuildingBlockKey.Invalid;
			foreach (short neighborBlockIndex in neighborList)
			{
				BuildingBlockKey neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborBlockIndex);
				BuildingBlockData buildingBlockData;
				bool flag = !this.TryGetElement_BuildingBlocks(neighborKey, out buildingBlockData);
				if (!flag)
				{
					bool flag2 = buildingBlockData.RootBlockIndex >= 0;
					if (flag2)
					{
						neighborKey.BuildingBlockIndex = buildingBlockData.RootBlockIndex;
						buildingBlockData = this.GetBuildingBlockData(neighborKey);
					}
					bool flag3 = buildingBlockData.TemplateId == targetBuildingTemplateId;
					if (flag3)
					{
						result = neighborKey;
						break;
					}
				}
			}
			ObjectPool<List<short>>.Instance.Return(neighborList);
			return result;
		}

		// Token: 0x06007C49 RID: 31817 RVA: 0x0049932C File Offset: 0x0049752C
		private void GetInfluencedBuildingBlocks(BuildingBlockKey blockKey, List<BuildingBlockData> influencedBlocks)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			BuildingBlockItem blockCfg = blockData.ConfigData;
			List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
			BuildingAreaData areaData = this.GetElement_BuildingAreas(new Location(blockKey.AreaId, blockKey.BlockId));
			areaData.GetNeighborBlocks(blockKey.BuildingBlockIndex, blockCfg.Width, neighborList, null, 2);
			foreach (short neighborIndex in neighborList)
			{
				BuildingBlockKey neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborIndex);
				BuildingBlockData neighborBlock;
				bool flag = !this.TryGetElement_BuildingBlocks(neighborKey, out neighborBlock);
				if (!flag)
				{
					bool flag2 = blockData.CanInfluenceBuildingBlock(neighborBlock);
					if (flag2)
					{
						influencedBlocks.Add(blockData);
					}
				}
			}
			ObjectPool<List<short>>.Instance.Return(neighborList);
		}

		// Token: 0x06007C4A RID: 31818 RVA: 0x00499410 File Offset: 0x00497610
		private void RemoveAllOperatorsInBuilding(DataContext context, BuildingBlockKey blockKey)
		{
			bool flag = !this._buildingOperatorDict.ContainsKey(blockKey);
			if (!flag)
			{
				List<int> operators = this._buildingOperatorDict[blockKey].GetCollection();
				for (int i = 0; i < 3; i++)
				{
					bool flag2 = operators[i] >= 0;
					if (flag2)
					{
						DomainManager.Taiwu.RemoveVillagerWork(context, operators[i], true);
					}
				}
			}
		}

		// Token: 0x06007C4B RID: 31819 RVA: 0x00499484 File Offset: 0x00497684
		private void RemoveAllManagersInBuilding(DataContext context, BuildingBlockKey blockKey)
		{
			bool flag = this._shopManagerDict.ContainsKey(blockKey);
			if (flag)
			{
				List<int> shopManagers = this._shopManagerDict[blockKey].GetCollection();
				for (int i = 0; i < 7; i++)
				{
					bool flag2 = shopManagers[i] >= 0;
					if (flag2)
					{
						DomainManager.Taiwu.RemoveVillagerWork(context, shopManagers[i], true);
					}
				}
			}
		}

		// Token: 0x06007C4C RID: 31820 RVA: 0x004994F4 File Offset: 0x004976F4
		public void UpdateTaiwuBuildingAutoOperation(DataContext context)
		{
			foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> entry in this._buildingBlocks)
			{
				BuildingBlockKey blockKey = entry.Key;
				BuildingBlockData blockData = entry.Value;
				bool flag = blockData.RootBlockIndex >= 0;
				if (!flag)
				{
					BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
					bool flag2 = configData.Type != EBuildingBlockType.Building && !BuildingBlockData.IsResource(configData.Type) && configData.TemplateId != 44;
					if (!flag2)
					{
						ParallelBuildingModification modification = new ParallelBuildingModification
						{
							BlockKey = blockKey,
							BlockData = blockData
						};
						bool flag3 = blockData.CanUse() && configData.IsShop;
						if (flag3)
						{
							Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
							bool flag4 = taiwuVillageLocation.AreaId == modification.BlockKey.AreaId && taiwuVillageLocation.BlockId == modification.BlockKey.BlockId;
							if (flag4)
							{
								this.TaiwuVillageAutoArrangeShopManager(context, modification);
								this.TaiwuVillageAutoAddShopSoldItem(context, modification);
							}
						}
					}
				}
			}
		}

		// Token: 0x06007C4D RID: 31821 RVA: 0x00499648 File Offset: 0x00497848
		private void TaiwuVillageAutoArrangeShopManager(DataContext context, ParallelBuildingModification modification)
		{
			BuildingBlockItem configData = BuildingBlock.Instance[modification.BlockData.TemplateId];
			bool flag = !configData.IsShop;
			if (!flag)
			{
				List<short> autoWorkList = DomainManager.Extra.GetAutoWorkBlockIndexList();
				bool flag2 = !modification.FreeShopManager && autoWorkList.Contains(modification.BlockKey.BuildingBlockIndex);
				if (flag2)
				{
					CharacterList shopManager;
					this.TryGetElement_ShopManagerDict(modification.BlockKey, out shopManager);
					bool flag3 = shopManager.GetCount() == 0;
					if (!flag3)
					{
						this.QuickArrangeShopManager(context, modification.BlockKey, false);
					}
				}
			}
		}

		// Token: 0x06007C4E RID: 31822 RVA: 0x004996D8 File Offset: 0x004978D8
		private void TaiwuVillageAutoAddShopSoldItem(DataContext context, ParallelBuildingModification modification)
		{
			List<short> autoSoldList = DomainManager.Extra.GetAutoSoldBlockIndexList();
			bool flag = !modification.FreeShopManager && autoSoldList.Contains(modification.BlockKey.BuildingBlockIndex);
			if (flag)
			{
				this.AutoAddShopSoldItem(context, modification.BlockKey);
			}
		}

		// Token: 0x06007C4F RID: 31823 RVA: 0x00499720 File Offset: 0x00497920
		private void AutoCheckInResidence(DataContext context, BuildingBlockKey blockKey)
		{
			List<short> autoCheckInList = DomainManager.Extra.GetAutoCheckInResidenceList();
			bool flag = autoCheckInList.Contains(blockKey.BuildingBlockIndex);
			if (flag)
			{
				this.QuickFillResidence(context, blockKey);
			}
		}

		// Token: 0x06007C50 RID: 31824 RVA: 0x00499754 File Offset: 0x00497954
		private void AutoCheckInComfortable(DataContext context, BuildingBlockKey blockKey)
		{
			List<short> autoCheckInList = DomainManager.Extra.GetAutoCheckInComfortableList();
			bool flag = autoCheckInList.Contains(blockKey.BuildingBlockIndex) && !DomainManager.Extra.GetFeast(blockKey).CheckAvoidAutoCheckIn();
			if (flag)
			{
				this.QuickFillComfortableHouse(context, blockKey);
			}
		}

		// Token: 0x06007C51 RID: 31825 RVA: 0x004997A0 File Offset: 0x004979A0
		public void ComplementUpdateBuilding(DataContext context, ParallelBuildingModification modification)
		{
			InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotificationCollection();
			short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			bool isTaiwuVillage = modification.BlockKey.AreaId == taiwuLocation.AreaId && modification.BlockKey.BlockId == taiwuLocation.BlockId;
			bool removeFromAutoExpand = modification.RemoveFromAutoExpand;
			if (removeFromAutoExpand)
			{
				List<short> autoExpandList = DomainManager.Extra.GetAutoExpandBlockIndexList();
				autoExpandList.Remove(modification.BlockData.BlockIndex);
				DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandList, context);
			}
			bool resetAllChildrenBlocks = modification.ResetAllChildrenBlocks;
			if (resetAllChildrenBlocks)
			{
				this.ResetAllChildrenBlocks(context, modification.BlockKey, 0, -1);
				this.SetBuildingCustomName(context, modification.BlockKey, null);
			}
			this.SetNewCompleteOperationBuildings(this._newCompleteOperationBuildings, context);
			this.SetElement_BuildingBlocks(modification.BlockKey, modification.BlockData, context);
			bool freeOperator = modification.FreeOperator;
			if (freeOperator)
			{
				this.RemoveAllOperatorsInBuilding(context, modification.BlockKey);
			}
			bool addBuilding = modification.AddBuilding;
			if (addBuilding)
			{
				bool flag = modification.BlockData.TemplateId == 46;
				if (flag)
				{
					this.AddResidence(context, modification.BlockKey);
				}
				else
				{
					bool flag2 = modification.BlockData.TemplateId == 47;
					if (flag2)
					{
						this.AddComfortableHouse(context, modification.BlockKey);
					}
				}
			}
			bool removeResidence = modification.RemoveResidence;
			if (removeResidence)
			{
				bool flag3 = this._residences.ContainsKey(modification.BlockKey);
				if (flag3)
				{
					this.RemoveResidence(context, modification.BlockKey);
				}
				else
				{
					bool flag4 = this._comfortableHouses.ContainsKey(modification.BlockKey);
					if (flag4)
					{
						this.RemoveComfortableHouse(context, modification.BlockKey);
					}
				}
			}
			bool flag5 = isTaiwuVillage;
			if (flag5)
			{
				this.AutoCheckInComfortable(context, modification.BlockKey);
				this.AutoCheckInResidence(context, modification.BlockKey);
			}
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			taiwuChar.ChangeResources(context, ref modification.DeltaResources);
			BuildingEarningsData data;
			bool flag6 = this.TryGetElement_CollectBuildingEarningsData(modification.BlockKey, out data);
			if (flag6)
			{
				bool flag7 = modification.BlockData.TemplateId == 222 && data.CollectionItemList != null;
				if (flag7)
				{
					short keepTimeIndex = GameData.Domains.Building.SharedMethods.CalcPawnshopKeepItemTimeIndex();
					for (int i = 0; i < data.CollectionItemList.Count; i++)
					{
						bool flag8 = (short)data.CollectionItemList[i].ModificationState >= GlobalConfig.BuildingPawnshopKeepItemTime[(int)keepTimeIndex];
						if (flag8)
						{
							data.CollectionItemList.RemoveAt(i);
						}
						else
						{
							data.CollectionItemList[i] = new ItemKey(data.CollectionItemList[i].ItemType, data.CollectionItemList[i].ModificationState + 1, data.CollectionItemList[i].TemplateId, data.CollectionItemList[i].Id);
						}
					}
				}
				bool flag9 = data.RecruitLevelList != null;
				if (flag9)
				{
					for (int j = 0; j < data.RecruitLevelList.Count; j++)
					{
						bool flag10 = data.RecruitLevelList[j].Second >= 3;
						if (flag10)
						{
							this.OfflineHandleRecruitPeopleLeave(context, modification.BlockKey, j);
							instantNotifications.AddCandidateLeaved(settlementId, modification.BlockData.TemplateId);
						}
						else
						{
							data.RecruitLevelList[j] = new IntPair(data.RecruitLevelList[j].First, data.RecruitLevelList[j].Second + 1);
						}
					}
				}
				this.SetElement_CollectBuildingEarningsData(modification.BlockKey, data, context);
			}
			bool flag11 = modification.CollectableEarnings != null || modification.CollectableResources != null || modification.ShopSoldItems != null || modification.RecruitLevelList != null;
			if (flag11)
			{
				BuildingEarningsData earningsData;
				bool flag12 = !this.TryGetElement_CollectBuildingEarningsData(modification.BlockKey, out earningsData);
				if (flag12)
				{
					earningsData = new BuildingEarningsData();
					this.AddElement_CollectBuildingEarningsData(modification.BlockKey, earningsData, context);
				}
				bool flag13 = modification.CollectableEarnings != null;
				if (flag13)
				{
					foreach (TemplateKey templateKey in modification.CollectableEarnings)
					{
						ItemKey item = DomainManager.Item.CreateItem(context, templateKey.ItemType, templateKey.TemplateId);
						DomainManager.Item.SetOwner(item, ItemOwnerType.Building, (int)modification.BlockData.TemplateId);
						earningsData.CollectionItemList.Add(item);
					}
				}
				bool flag14 = modification.CollectableResources != null;
				if (flag14)
				{
					foreach (IntPair resourcePair in modification.CollectableResources)
					{
						earningsData.CollectionResourceList.Add(new IntPair(resourcePair.First, resourcePair.Second));
					}
				}
				bool flag15 = modification.ShopBuildingSalaryList != null;
				if (flag15)
				{
					ShopEventCollection shopEventCollection = this.GetOrCreateShopEventCollection(modification.BlockKey);
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					int date = DomainManager.World.GetCurrDate();
					foreach (ValueTuple<int, sbyte, int> valueTuple in modification.ShopBuildingSalaryList)
					{
						int charId = valueTuple.Item1;
						sbyte resourceType = valueTuple.Item2;
						int count = valueTuple.Item3;
						GameData.Domains.Character.Character character;
						bool flag16 = DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (flag16)
						{
							character.ChangeResource(context, resourceType, count);
							lifeRecordCollection.AddTaiwuVillagerSalaryReceived(charId, date, modification.BlockData.TemplateId, count, resourceType);
							shopEventCollection.AddSalaryReceived(date, charId, count, resourceType);
						}
					}
				}
				bool flag17 = modification.RecruitLevelList != null;
				if (flag17)
				{
					earningsData.RecruitLevelList.AddRange(modification.RecruitLevelList);
				}
				bool flag18 = modification.ShopSoldItems != null;
				if (flag18)
				{
					for (int k = 0; k < modification.ShopSoldItems.Count; k++)
					{
						bool flag19 = modification.ShopSoldItems[k].Item1 == -2;
						if (flag19)
						{
							int count2 = (int)GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(modification.BlockData.TemplateId) - earningsData.ShopSoldItemList.Count;
							for (int l = 0; l < count2; l++)
							{
								earningsData.ShopSoldItemList.Add(ItemKey.Invalid);
								earningsData.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
							}
						}
						else
						{
							sbyte index = modification.ShopSoldItems[k].Item1;
							bool flag20 = (int)index < earningsData.ShopSoldItemList.Count && index >= 0;
							if (flag20)
							{
								DomainManager.Item.RemoveItem(context, earningsData.ShopSoldItemList[(int)index]);
								earningsData.ShopSoldItemList[(int)index] = ItemKey.Invalid;
								earningsData.ShopSoldItemEarnList[(int)index] = new IntPair(modification.ShopSoldItems[k].Item2.First, modification.ShopSoldItems[k].Item2.Second);
							}
						}
					}
				}
				this.SetElement_CollectBuildingEarningsData(modification.BlockKey, earningsData, context);
			}
			bool flag21 = modification.LearnCombatSkills != null;
			if (flag21)
			{
				foreach (ValueTuple<int, short, byte> learnSkillInfo in modification.LearnCombatSkills)
				{
					GameData.Domains.Character.Character character2;
					bool flag22 = !DomainManager.Character.TryGetElement_Objects(learnSkillInfo.Item1, out character2);
					if (!flag22)
					{
						CombatSkillKey combatSkillKey = new CombatSkillKey(learnSkillInfo.Item1, learnSkillInfo.Item2);
						GameData.Domains.CombatSkill.CombatSkill combatSkill;
						bool flag23 = DomainManager.CombatSkill.TryGetElement_CombatSkills(combatSkillKey, out combatSkill);
						if (flag23)
						{
							ushort readingSate = combatSkill.GetReadingState();
							readingSate = CombatSkillStateHelper.SetPageRead(readingSate, learnSkillInfo.Item3);
							combatSkill.SetReadingState(readingSate, context);
						}
						else
						{
							ushort readingState = CombatSkillStateHelper.SetPageRead(0, learnSkillInfo.Item3);
							character2.LearnNewCombatSkill(context, learnSkillInfo.Item2, readingState);
						}
						DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, character2.GetId(), learnSkillInfo.Item2, learnSkillInfo.Item3);
					}
				}
			}
			bool flag24 = modification.LearnLifeSkills != null;
			if (flag24)
			{
				foreach (ValueTuple<int, short, byte> learnSkillInfo2 in modification.LearnLifeSkills)
				{
					GameData.Domains.Character.Character character3;
					bool flag25 = !DomainManager.Character.TryGetElement_Objects(learnSkillInfo2.Item1, out character3);
					if (!flag25)
					{
						int lifeSkillIndex = character3.FindLearnedLifeSkillIndex(learnSkillInfo2.Item2);
						bool flag26 = lifeSkillIndex >= 0;
						if (flag26)
						{
							character3.ReadLifeSkillPage(context, lifeSkillIndex, learnSkillInfo2.Item3);
						}
						else
						{
							character3.LearnNewLifeSkill(context, learnSkillInfo2.Item2, (byte)(1 << (int)learnSkillInfo2.Item3));
						}
					}
				}
			}
			bool flag27 = modification.FixBookList != null && modification.FixBookList.Count > 0;
			if (flag27)
			{
				GameData.Domains.Item.SkillBook skillBook = DomainManager.Item.GetElement_SkillBooks(modification.FixBookList[0].Id);
				bool flag28 = !skillBook.CanFix();
				if (flag28)
				{
					return;
				}
				sbyte incompletePage = skillBook.GetFixProgress().Item1;
				ushort pageState = skillBook.GetPageIncompleteState();
				pageState = SkillBookStateHelper.SetPageIncompleteState(pageState, (byte)incompletePage, 0);
				skillBook.SetPageIncompleteState(pageState, DataContextManager.GetCurrentThreadDataContext());
				bool flag29 = !skillBook.CanFix();
				if (flag29)
				{
					DomainManager.World.GetInstantNotificationCollection().AddBookRepairSuccess(data.FixBookInfoList[0].ItemType, data.FixBookInfoList[0].TemplateId);
				}
			}
			bool freeShopManager = modification.FreeShopManager;
			if (freeShopManager)
			{
				this.RemoveAllManagersInBuilding(context, modification.BlockKey);
				this.ClearBuildingBlockEarningsData(context, modification.BlockKey, modification.BlockData.TemplateId == 222);
			}
			MakeItemData makeItemData;
			bool flag30 = modification.RemoveMakeItemData && this.TryGetElement_MakeItemDict(modification.BlockKey, out makeItemData);
			if (flag30)
			{
				this.RemoveElement_MakeItemDict(modification.BlockKey, context);
			}
			bool flag31 = modification.RemoveEventBookData && this._shopEventCollections != null && this._shopEventCollections.ContainsKey(modification.BlockKey);
			if (flag31)
			{
				this._shopEventCollections.Remove(modification.BlockKey);
			}
			bool removeCollectResourceType = modification.RemoveCollectResourceType;
			if (removeCollectResourceType)
			{
				this.RemoveElement_CollectBuildingResourceType(modification.BlockKey, context);
			}
			bool addBuilding2 = modification.AddBuilding;
			if (addBuilding2)
			{
				DomainManager.Taiwu.AddLegacyPoint(context, 26, 100);
				bool flag32 = DomainManager.Extra.IsExtraTaskInProgress(22);
				if (flag32)
				{
					DomainManager.Extra.FinishTriggeredExtraTask(context, 14, 22);
				}
				BuildingBlockItem config = BuildingBlock.Instance[modification.BlockData.TemplateId];
				bool flag33 = BuildingBlockData.IsUsefulResource(config.Type);
				if (flag33)
				{
					List<short> list = DomainManager.Extra.GetLegaciesBuildingTemplateIdList();
					list.Remove(modification.BlockData.TemplateId);
					DomainManager.Extra.SetLegaciesBuildingTemplateIdList(list, context);
				}
			}
			bool flag34 = modification.BuildingMoneyPrestigeSuccessRateCompensationChanged != null;
			if (flag34)
			{
				DomainManager.Extra.UpdateBuildingMoneyPrestigeSuccessRateCompensation(context, modification.BuildingMoneyPrestigeSuccessRateCompensationChanged);
				modification.BuildingMoneyPrestigeSuccessRateCompensationChanged.Clear();
			}
			bool buildingOperationComplete = modification.BuildingOperationComplete;
			if (buildingOperationComplete)
			{
				this.UpdateTaiwuVillageBuildingEffect();
			}
		}

		// Token: 0x06007C52 RID: 31826 RVA: 0x0049A394 File Offset: 0x00498594
		public void AddBuildingException(BuildingBlockKey buildingBlockKey, BuildingBlockData buildingBlockData, BuildingExceptionType buildingExceptionType)
		{
			BuildingExceptionItem exceptionItem;
			bool flag = !this._buildingExceptionData.BuildingExceptionDict.TryGetValue(buildingBlockKey, out exceptionItem);
			if (flag)
			{
				exceptionItem = new BuildingExceptionItem();
				this._buildingExceptionData.BuildingExceptionDict[buildingBlockKey] = exceptionItem;
			}
			bool flag2 = !exceptionItem.ExceptionTypeList.Contains((sbyte)buildingExceptionType);
			if (flag2)
			{
				exceptionItem.ExceptionTypeList.Add((sbyte)buildingExceptionType);
			}
		}

		// Token: 0x06007C53 RID: 31827 RVA: 0x0049A3F7 File Offset: 0x004985F7
		private void ClearBuildingException()
		{
			this._buildingExceptionData.BuildingExceptionDict.Clear();
		}

		// Token: 0x06007C54 RID: 31828 RVA: 0x0049A40A File Offset: 0x0049860A
		[DomainMethod]
		public BuildingExceptionData GetBuildingExceptionData()
		{
			return this._buildingExceptionData;
		}

		// Token: 0x06007C55 RID: 31829 RVA: 0x0049A414 File Offset: 0x00498614
		public int GetBuildingBlockEffect(Location location, EBuildingScaleEffect effectType, int subType = -1)
		{
			IBuildingEffectValue effectValue = this.GetBuildingBlockEffectObject(location, effectType);
			bool flag = effectValue == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = ((subType < 0) ? effectValue.Get() : effectValue.Get(subType));
			}
			return result;
		}

		// Token: 0x06007C56 RID: 31830 RVA: 0x0049A450 File Offset: 0x00498650
		public int GetBuildingBlockEffect(short settlementId, EBuildingScaleEffect effectType, int subType = -1)
		{
			IBuildingEffectValue effect = this.GetBuildingBlockEffectObject(settlementId, effectType);
			bool flag = effect == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = ((subType < 0) ? effect.Get() : effect.Get(subType));
			}
			return result;
		}

		// Token: 0x06007C57 RID: 31831 RVA: 0x0049A48C File Offset: 0x0049868C
		public IBuildingEffectValue GetBuildingBlockEffectObject(Location location, EBuildingScaleEffect effectType)
		{
			bool flag = DomainManager.World.GetMainStoryLineProgress() < 1;
			IBuildingEffectValue result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = !location.IsValid();
				if (flag2)
				{
					result = null;
				}
				else
				{
					MapBlockData mapBlock = DomainManager.Map.GetBelongSettlementBlock(location);
					bool flag3 = mapBlock == null;
					if (flag3)
					{
						result = null;
					}
					else
					{
						Location settlementLocation = new Location(mapBlock.AreaId, mapBlock.BlockId);
						IBuildingEffectValue[] effects;
						result = (this._buildingBlockEffectsCache.TryGetValue(settlementLocation, out effects) ? effects[(int)effectType] : null);
					}
				}
			}
			return result;
		}

		// Token: 0x06007C58 RID: 31832 RVA: 0x0049A510 File Offset: 0x00498710
		public IBuildingEffectValue GetBuildingBlockEffectObject(short settlementId, EBuildingScaleEffect effectType)
		{
			bool flag = DomainManager.World.GetMainStoryLineProgress() < 1;
			IBuildingEffectValue result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = settlementId < 0;
				if (flag2)
				{
					result = null;
				}
				else
				{
					Location location = DomainManager.Organization.GetSettlement(settlementId).GetLocation();
					IBuildingEffectValue[] effects;
					result = (this._buildingBlockEffectsCache.TryGetValue(location, out effects) ? effects[(int)effectType] : null);
				}
			}
			return result;
		}

		// Token: 0x06007C59 RID: 31833 RVA: 0x0049A56C File Offset: 0x0049876C
		private void InitAllAreaBuildingBlockEffectsCache()
		{
			this._buildingBlockEffectsCache.Clear();
			foreach (KeyValuePair<Location, BuildingAreaData> keyValuePair in this._buildingAreas)
			{
				Location location2;
				BuildingAreaData buildingAreaData;
				keyValuePair.Deconstruct(out location2, out buildingAreaData);
				Location location = location2;
				bool flag = !MapAreaData.IsRegularArea(location.AreaId);
				if (!flag)
				{
					this.UpdateLocationBuildingBlockEffectsCache(location);
				}
			}
		}

		// Token: 0x06007C5A RID: 31834 RVA: 0x0049A5FC File Offset: 0x004987FC
		private unsafe void UpdateLocationBuildingBlockEffectsCache(Location location)
		{
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)20], 5);
			Span<int> baseValues = span;
			IBuildingEffectValue[] bonuses = this._buildingBlockEffectsCache.GetOrDefault(location);
			bool flag = bonuses == null;
			if (flag)
			{
				bonuses = new IBuildingEffectValue[33];
				for (int i = 0; i < bonuses.Length; i++)
				{
					bonuses[i] = this.InitBonus((EBuildingScaleEffect)i);
				}
				this._buildingBlockEffectsCache.Add(location, bonuses);
			}
			else
			{
				for (int j = 0; j < bonuses.Length; j++)
				{
					bonuses[j].Clear();
				}
			}
			for (short templateId = 1; templateId < 21; templateId += 1)
			{
				this.CalcResourceBlockEffectBaseValues(location, templateId, ref baseValues);
				BuildingBlockItem buildingBlockCfg = BuildingBlock.Instance[templateId];
				List<short> expandInfos = buildingBlockCfg.ExpandInfos;
				bool flag2 = expandInfos == null || expandInfos.Count <= 0;
				if (!flag2)
				{
					foreach (short buildingScaleId in buildingBlockCfg.ExpandInfos)
					{
						BuildingScaleItem buildingScaleCfg = BuildingScale.Instance[buildingScaleId];
						bool flag3 = buildingScaleCfg.Effect == EBuildingScaleEffect.Invalid;
						if (!flag3)
						{
							bool flag4 = buildingScaleCfg.Formula >= 0;
							if (flag4)
							{
								int delta = this.CalcResourceBlockTotalEffectValue(buildingScaleCfg.Formula, baseValues);
								bonuses[(int)buildingScaleCfg.Effect].Change(delta);
							}
						}
					}
				}
			}
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			bool needManage = DomainManager.Taiwu.GetTaiwuVillageLocation() == location;
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag5 = blockData.TemplateId < 0;
				if (!flag5)
				{
					BuildingBlockItem buildingBlockCfg2 = blockData.ConfigData;
					bool flag6 = buildingBlockCfg2.Class == EBuildingBlockClass.BornResource;
					if (!flag6)
					{
						List<short> expandInfos = buildingBlockCfg2.ExpandInfos;
						bool flag7 = expandInfos == null || expandInfos.Count <= 0;
						if (!flag7)
						{
							bool flag8 = !this.HasShopManagerLeader(blockKey) && needManage;
							if (!flag8)
							{
								sbyte b;
								bool canUse = blockData.CanUse() && this.AllDependBuildingAvailable(blockKey, blockData.TemplateId, out b);
								bool flag9 = !canUse;
								if (!flag9)
								{
									this._formulaContextBridge.Initialize(blockKey, buildingBlockCfg2, this._formulaArgHandler, false);
									foreach (short buildingScaleId2 in buildingBlockCfg2.ExpandInfos)
									{
										BuildingScaleItem buildingScaleCfg2 = BuildingScale.Instance[buildingScaleId2];
										bool flag10 = buildingScaleCfg2.Effect == EBuildingScaleEffect.Invalid;
										if (!flag10)
										{
											this.ApplyScaleEffect(blockKey, buildingScaleCfg2, bonuses[(int)buildingScaleCfg2.Effect]);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007C5B RID: 31835 RVA: 0x0049A92C File Offset: 0x00498B2C
		private static int CalcBuildingFormulaContextArg(BuildingBlockKey blockKey, EBuildingFormulaArgType argType)
		{
			if (!true)
			{
			}
			int result;
			switch (argType)
			{
			case EBuildingFormulaArgType.MaxAttainment:
			{
				bool flag;
				result = DomainManager.Building.BuildingMaxAttainment(blockKey, -1, out flag);
				break;
			}
			case EBuildingFormulaArgType.TotalAttainment:
			{
				bool flag;
				result = DomainManager.Building.BuildingTotalAttainment(blockKey, -1, out flag, false);
				break;
			}
			case EBuildingFormulaArgType.LeaderFameType:
				result = (int)DomainManager.Building.BuildLeaderFameType(blockKey);
				break;
			default:
				throw new ArgumentOutOfRangeException("argType", argType, null);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06007C5C RID: 31836 RVA: 0x0049A9A0 File Offset: 0x00498BA0
		private IBuildingEffectValue InitBonus(EBuildingScaleEffect effect)
		{
			if (!true)
			{
			}
			BuildingEffectValue result;
			switch (effect)
			{
			case EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor:
			case EBuildingScaleEffect.BreakOutSuccessRate:
			case EBuildingScaleEffect.CombatSkillAttainment:
				result = new BuildingEffectGroupValue(14);
				goto IL_54;
			case EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor:
			case EBuildingScaleEffect.ShopManagerQualificationImproveRate:
			case EBuildingScaleEffect.MakeItemAttainmentRequirementReduction:
			case EBuildingScaleEffect.LifeSkillAttainment:
				result = new BuildingEffectGroupValue(16);
				goto IL_54;
			}
			result = new BuildingEffectValue();
			IL_54:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06007C5D RID: 31837 RVA: 0x0049AA0C File Offset: 0x00498C0C
		private void ApplyScaleEffect(BuildingBlockKey blockKey, BuildingScaleItem scaleCfg, IBuildingEffectValue value)
		{
			int delta = 0;
			List<int> levelEffect = scaleCfg.LevelEffect;
			bool flag = levelEffect != null && levelEffect.Count > 0;
			if (flag)
			{
				sbyte level = this.BuildingBlockLevel(blockKey);
				delta = scaleCfg.GetLevelEffect((int)level);
			}
			else
			{
				bool flag2 = scaleCfg.Formula >= 0;
				if (flag2)
				{
					BuildingFormulaItem formula = BuildingFormula.Instance[scaleCfg.Formula];
					delta = formula.Calculate(this._formulaContextBridge);
				}
			}
			switch (scaleCfg.Effect)
			{
			case EBuildingScaleEffect.CombatSkillReadingSpeedBonusFactor:
			case EBuildingScaleEffect.BreakOutSuccessRate:
			case EBuildingScaleEffect.CombatSkillAttainment:
				value.Change((int)scaleCfg.CombatSkillType, delta);
				return;
			case EBuildingScaleEffect.LifeSkillReadingSpeedBonusFactor:
			case EBuildingScaleEffect.ShopManagerQualificationImproveRate:
			case EBuildingScaleEffect.MakeItemAttainmentRequirementReduction:
			case EBuildingScaleEffect.LifeSkillAttainment:
				value.Change((int)scaleCfg.LifeSkillType, delta);
				return;
			}
			value.Change(delta);
		}

		// Token: 0x06007C5E RID: 31838 RVA: 0x0049AAE8 File Offset: 0x00498CE8
		public IReadOnlyList<int> GetSettlementChickenIdList(int settlementId)
		{
			List<int> list;
			IReadOnlyList<int> result;
			if (!this._settlementChickenIdLists.TryGetValue(settlementId, out list))
			{
				IReadOnlyList<int> readOnlyList = Array.Empty<int>();
				result = readOnlyList;
			}
			else
			{
				IReadOnlyList<int> readOnlyList = list;
				result = readOnlyList;
			}
			return result;
		}

		// Token: 0x06007C5F RID: 31839 RVA: 0x0049AB14 File Offset: 0x00498D14
		private void AddChickenInSettlementChickenIdLists(int chickenId, int settlementId)
		{
			List<int> list;
			bool flag = !this._settlementChickenIdLists.TryGetValue(settlementId, out list);
			if (flag)
			{
				list = new List<int>();
				this._settlementChickenIdLists.Add(settlementId, list);
			}
			list.Add(chickenId);
		}

		// Token: 0x06007C60 RID: 31840 RVA: 0x0049AB58 File Offset: 0x00498D58
		private void RemoveChickenInSettlementChickenIdLists(int chickenId, int settlementId)
		{
			List<int> list;
			bool flag = !this._settlementChickenIdLists.TryGetValue(settlementId, out list);
			if (flag)
			{
				PredefinedLogItem item = PredefinedLog.Instance[30];
				object[] array = new object[4];
				array[0] = string.Join(", ", this._settlementChickenIdLists.Select(delegate(KeyValuePair<int, List<int>> x)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
					defaultInterpolatedStringHandler.AppendFormatted<int>(x.Key);
					defaultInterpolatedStringHandler.AppendLiteral(": [");
					defaultInterpolatedStringHandler.AppendFormatted(string.Join(", ", from x in x.Value
					select x.ToString()));
					defaultInterpolatedStringHandler.AppendLiteral("]");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}));
				array[1] = string.Join(", ", this._chicken.Select(delegate(KeyValuePair<int, Chicken> x)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
					defaultInterpolatedStringHandler.AppendFormatted<int>(x.Value.CurrentSettlementId);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(x.Key);
					defaultInterpolatedStringHandler.AppendLiteral("(");
					defaultInterpolatedStringHandler.AppendFormatted<short>(x.Value.TemplateId);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}));
				array[2] = new StackTrace().ToString();
				array[3] = settlementId;
				item.Log(array);
			}
			else
			{
				bool flag2 = !list.Remove(chickenId);
				if (flag2)
				{
					PredefinedLogItem item2 = PredefinedLog.Instance[31];
					object[] array2 = new object[5];
					array2[0] = string.Join(", ", this._settlementChickenIdLists.Select(delegate(KeyValuePair<int, List<int>> x)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
						defaultInterpolatedStringHandler.AppendFormatted<int>(x.Key);
						defaultInterpolatedStringHandler.AppendLiteral(": [");
						defaultInterpolatedStringHandler.AppendFormatted(string.Join(", ", from x in x.Value
						select x.ToString()));
						defaultInterpolatedStringHandler.AppendLiteral("]");
						return defaultInterpolatedStringHandler.ToStringAndClear();
					}));
					array2[1] = string.Join(", ", this._chicken.Select(delegate(KeyValuePair<int, Chicken> x)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
						defaultInterpolatedStringHandler.AppendFormatted<int>(x.Value.CurrentSettlementId);
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(x.Key);
						defaultInterpolatedStringHandler.AppendLiteral("(");
						defaultInterpolatedStringHandler.AppendFormatted<short>(x.Value.TemplateId);
						defaultInterpolatedStringHandler.AppendLiteral(")");
						return defaultInterpolatedStringHandler.ToStringAndClear();
					}));
					array2[2] = new StackTrace().ToString();
					array2[3] = settlementId;
					array2[4] = chickenId;
					item2.Log(array2);
				}
				else
				{
					bool flag3 = list.Count == 0;
					if (flag3)
					{
						this._settlementChickenIdLists.Remove(settlementId);
					}
				}
			}
		}

		// Token: 0x06007C61 RID: 31841 RVA: 0x0049ACF7 File Offset: 0x00498EF7
		private void ClearSettlementChickenIdLists()
		{
			this._settlementChickenIdLists.Clear();
		}

		// Token: 0x06007C62 RID: 31842 RVA: 0x0049AD05 File Offset: 0x00498F05
		private void TransferChickenInSettlementChickenIdLists(int chickenId, int fromSettlementId, int toSettlementId)
		{
			this.RemoveChickenInSettlementChickenIdLists(chickenId, fromSettlementId);
			this.AddChickenInSettlementChickenIdLists(chickenId, toSettlementId);
		}

		// Token: 0x06007C63 RID: 31843 RVA: 0x0049AD1C File Offset: 0x00498F1C
		public void RefreshSettlementChickenIdLists()
		{
			this.ClearSettlementChickenIdLists();
			foreach (KeyValuePair<int, Chicken> chicken in this._chicken)
			{
				this.AddChickenInSettlementChickenIdLists(chicken.Key, chicken.Value.CurrentSettlementId);
			}
		}

		// Token: 0x06007C64 RID: 31844 RVA: 0x0049AD90 File Offset: 0x00498F90
		[DomainMethod]
		public int AddChicken(DataContext context, int settlementId, short templateId)
		{
			int id = this.GenerateNextChickenId();
			Chicken chicken = new Chicken
			{
				Id = id,
				TemplateId = templateId,
				CurrentSettlementId = settlementId
			};
			this.AddElement_Chicken(id, chicken, context);
			this.AddChickenInSettlementChickenIdLists(id, settlementId);
			return id;
		}

		// Token: 0x06007C65 RID: 31845 RVA: 0x0049ADE0 File Offset: 0x00498FE0
		[DomainMethod]
		public void RemoveChicken(DataContext context, int id)
		{
			Chicken chicken;
			bool flag = this._chicken.TryGetValue(id, out chicken);
			if (flag)
			{
				this.RemoveChickenInSettlementChickenIdLists(id, chicken.CurrentSettlementId);
				this.RemoveElement_Chicken(id, context);
			}
			else
			{
				PredefinedLogItem item = PredefinedLog.Instance[32];
				object[] array = new object[4];
				array[0] = string.Join(", ", this._settlementChickenIdLists.Select(delegate(KeyValuePair<int, List<int>> x)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
					defaultInterpolatedStringHandler.AppendFormatted<int>(x.Key);
					defaultInterpolatedStringHandler.AppendLiteral(": [");
					defaultInterpolatedStringHandler.AppendFormatted(string.Join(", ", from x in x.Value
					select x.ToString()));
					defaultInterpolatedStringHandler.AppendLiteral("]");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}));
				array[1] = string.Join(", ", this._chicken.Select(delegate(KeyValuePair<int, Chicken> x)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
					defaultInterpolatedStringHandler.AppendFormatted<int>(x.Value.CurrentSettlementId);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(x.Key);
					defaultInterpolatedStringHandler.AppendLiteral("(");
					defaultInterpolatedStringHandler.AppendFormatted<short>(x.Value.TemplateId);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}));
				array[2] = new StackTrace().ToString();
				array[3] = id;
				item.Log(array);
			}
		}

		// Token: 0x06007C66 RID: 31846 RVA: 0x0049AEBC File Offset: 0x004990BC
		[DomainMethod]
		public void RemoveAllChicken(DataContext context)
		{
			this.ClearSettlementChickenIdLists();
			this.ClearChicken(context);
		}

		// Token: 0x06007C67 RID: 31847 RVA: 0x0049AED0 File Offset: 0x004990D0
		[DomainMethod]
		public void MoveChicken(DataContext context, int id, int targetSettlementId)
		{
			Chicken chicken;
			bool flag = !this.TryGetElement_Chicken(id, out chicken);
			if (!flag)
			{
				this.TransferChickenInSettlementChickenIdLists(id, chicken.CurrentSettlementId, targetSettlementId);
				chicken.CurrentSettlementId = targetSettlementId;
				this.SetElement_Chicken(id, chicken, context);
				bool flag2;
				if (this.GetSettlementChickenIdList(id).Count == 0)
				{
					List<short> chickenMapInfo = this.ChickenMapInfo;
					flag2 = (chickenMapInfo != null && chickenMapInfo.Remove((short)chicken.CurrentSettlementId));
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					DomainManager.Extra.FinishTriggeredExtraTask(context, 53, 342);
					bool flag4 = this.ChickenMapInfo.Count > 0;
					if (flag4)
					{
						DomainManager.Extra.TriggerExtraTask(context, 53, 342);
					}
				}
			}
		}

		// Token: 0x06007C68 RID: 31848 RVA: 0x0049AF80 File Offset: 0x00499180
		[DomainMethod]
		public void TransferChicken(DataContext context, int id, int targetSettlementId)
		{
			Chicken chicken;
			bool flag = !this.TryGetElement_Chicken(id, out chicken);
			if (!flag)
			{
				chicken.Happiness = 100;
				this.SetElement_Chicken(id, chicken, context);
				this.MoveChicken(context, id, targetSettlementId);
			}
		}

		// Token: 0x06007C69 RID: 31849 RVA: 0x0049AFC0 File Offset: 0x004991C0
		[DomainMethod]
		public List<int> GetLocationChicken(Location location)
		{
			int sourceSettlementId = (int)DomainManager.Organization.GetSettlementByLocation(location).GetId();
			return this.GetSettlementChickenList(sourceSettlementId, false);
		}

		// Token: 0x06007C6A RID: 31850 RVA: 0x0049AFED File Offset: 0x004991ED
		[DomainMethod]
		public bool AllChickenInTaiwuVillage(DataContext context)
		{
			return this._settlementChickenIdLists.Count == 1;
		}

		// Token: 0x06007C6B RID: 31851 RVA: 0x0049B000 File Offset: 0x00499200
		[DomainMethod]
		public bool ClickChickenMap(DataContext context)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location taiwuValidLocation = taiwu.GetValidLocation();
			short costTime = short.MaxValue;
			bool taskInProgress = DomainManager.Extra.IsExtraTaskInProgress(338);
			List<int> loseFeatherChickens = DomainManager.Extra.GetSectFulongLoseFeatherChickens();
			Location filterLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			bool flag = this.ChickenMapInfo == null;
			if (flag)
			{
				this.ChickenMapInfo = new List<short>();
			}
			Dictionary<int, List<short>> chickenInfo = new Dictionary<int, List<short>>();
			foreach (KeyValuePair<int, Chicken> pair in this._chicken)
			{
				bool flag2 = taskInProgress && loseFeatherChickens != null && loseFeatherChickens.Contains(pair.Key);
				if (!flag2)
				{
					Settlement chickenSettlement = DomainManager.Organization.GetSettlement((short)pair.Value.CurrentSettlementId);
					Location chickenLocation = chickenSettlement.GetLocation();
					bool flag3 = taiwuValidLocation.AreaId == chickenLocation.AreaId && chickenLocation != filterLocation;
					if (flag3)
					{
						bool flag4 = chickenInfo.ContainsKey((int)chickenLocation.AreaId);
						if (flag4)
						{
							chickenInfo[(int)chickenLocation.AreaId].Add((short)pair.Value.CurrentSettlementId);
						}
						else
						{
							chickenInfo.Add((int)chickenLocation.AreaId, new List<short>
							{
								(short)pair.Value.CurrentSettlementId
							});
						}
					}
				}
			}
			bool flag5 = chickenInfo.Count > 0;
			bool result;
			if (flag5)
			{
				DomainManager.Extra.FinishTriggeredExtraTask(context, 53, 342);
				DomainManager.Extra.TriggerExtraTask(context, 53, 342);
				this.ChickenMapInfo = chickenInfo.Values.ElementAt(0).Distinct<short>().ToList<short>();
				result = true;
			}
			else
			{
				foreach (KeyValuePair<int, Chicken> pair2 in this._chicken)
				{
					bool flag6 = taskInProgress && loseFeatherChickens != null && loseFeatherChickens.Contains(pair2.Key);
					if (!flag6)
					{
						Settlement chickenSettlement2 = DomainManager.Organization.GetSettlement((short)pair2.Value.CurrentSettlementId);
						Location chickenLocation2 = chickenSettlement2.GetLocation();
						bool flag7 = chickenLocation2 == filterLocation;
						if (!flag7)
						{
							CrossAreaMoveInfo crossAreaMoveInfo = DomainManager.Map.CalcAreaTravelRoute(taiwu, taiwuValidLocation.AreaId, taiwuValidLocation.BlockId, chickenLocation2.AreaId);
							short time = crossAreaMoveInfo.Route.GetTotalTimeCost();
							bool flag8 = time < costTime;
							if (flag8)
							{
								costTime = time;
								chickenInfo.Clear();
								chickenInfo.Add((int)chickenLocation2.AreaId, new List<short>
								{
									(short)pair2.Value.CurrentSettlementId
								});
							}
							else
							{
								bool flag9 = time == costTime;
								if (flag9)
								{
									bool flag10 = chickenInfo.ContainsKey((int)chickenLocation2.AreaId);
									if (flag10)
									{
										chickenInfo[(int)chickenLocation2.AreaId].Add((short)pair2.Value.CurrentSettlementId);
									}
									else
									{
										chickenInfo.Add((int)chickenLocation2.AreaId, new List<short>
										{
											(short)pair2.Value.CurrentSettlementId
										});
									}
								}
							}
						}
					}
				}
				DomainManager.Extra.FinishTriggeredExtraTask(context, 53, 342);
				bool flag11 = chickenInfo.Count > 0;
				if (flag11)
				{
					this.ChickenMapInfo = chickenInfo.Values.ElementAt(context.Random.Next(0, chickenInfo.Count)).Distinct<short>().ToList<short>();
					DomainManager.Extra.TriggerExtraTask(context, 53, 342);
					result = true;
				}
				else
				{
					this.ChickenMapInfo.Clear();
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06007C6C RID: 31852 RVA: 0x0049B3F8 File Offset: 0x004995F8
		[DomainMethod]
		public void ClickChickenSign(DataContext context, int chickenId)
		{
			List<int> loseFeatherChickens = DomainManager.Extra.GetSectFulongLoseFeatherChickens();
			bool flag = !DomainManager.Extra.IsExtraTaskInProgress(338);
			if (!flag)
			{
				foreach (KeyValuePair<int, Chicken> pair in this._chicken)
				{
					bool flag2 = pair.Key == chickenId && !loseFeatherChickens.Contains(chickenId);
					if (flag2)
					{
						DomainManager.TaiwuEvent.OnEvent_ClickChicken(chickenId, pair.Value.TemplateId);
						break;
					}
				}
			}
		}

		// Token: 0x06007C6D RID: 31853 RVA: 0x0049B4A4 File Offset: 0x004996A4
		[DomainMethod]
		public bool IsInFulongSeekFeatherTask(DataContext context)
		{
			return DomainManager.Extra.IsExtraTaskInProgress(338);
		}

		// Token: 0x06007C6E RID: 31854 RVA: 0x0049B4C8 File Offset: 0x004996C8
		[DomainMethod]
		public List<int> GetSettlementChickenList(int sourceSettlementId, bool ignoreFulong = true)
		{
			List<int> chickenList = this.GetSettlementChickenIdList(sourceSettlementId).ToList<int>();
			List<int> result;
			if (ignoreFulong)
			{
				result = chickenList;
			}
			else
			{
				bool flag = DomainManager.World.GetSectMainStoryTaskStatus(14) == 0;
				if (flag)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement((short)sourceSettlementId);
					bool flag2 = DomainManager.Taiwu.GetTaiwuVillageLocation().Equals(settlement.GetLocation());
					if (flag2)
					{
						EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(14);
						bool chickenKingLeaveHome = false;
						argBox.Get("ConchShip_PresetKey_FulongChickenKingLeaveHome", ref chickenKingLeaveHome);
						bool flag3 = chickenKingLeaveHome;
						if (flag3)
						{
							for (int i = chickenList.Count - 1; i >= 0; i--)
							{
								Chicken chicken = this._chicken[chickenList[i]];
								bool flag4 = chicken.TemplateId == 63;
								if (flag4)
								{
									chickenList.RemoveAt(i);
									break;
								}
							}
						}
					}
				}
				result = chickenList;
			}
			return result;
		}

		// Token: 0x06007C6F RID: 31855 RVA: 0x0049B5C0 File Offset: 0x004997C0
		public int GetChickenKingId()
		{
			foreach (KeyValuePair<int, Chicken> pair in this._chicken)
			{
				bool flag = pair.Value.TemplateId == 63;
				if (flag)
				{
					return pair.Key;
				}
			}
			return -1;
		}

		// Token: 0x06007C70 RID: 31856 RVA: 0x0049B638 File Offset: 0x00499838
		[DomainMethod]
		public List<Chicken> GetSettlementChickenDataList(Location location)
		{
			int sourceSettlementId = (int)DomainManager.Organization.GetSettlementByLocation(location).GetId();
			List<int> chickenKeyList = this.GetSettlementChickenList(sourceSettlementId, true);
			this.MakeSureChickenKingFirst(ref chickenKeyList, location);
			List<Chicken> chickenList = new List<Chicken>();
			for (int i = 0; i < chickenKeyList.Count; i++)
			{
				Chicken chickenData = this.GetChickenData(chickenKeyList[i]);
				chickenList.Add(chickenData);
			}
			return chickenList;
		}

		// Token: 0x06007C71 RID: 31857 RVA: 0x0049B6A8 File Offset: 0x004998A8
		private void MakeSureChickenKingFirst(ref List<int> chickenKeyList, Location location)
		{
			bool flag = DomainManager.Taiwu.GetTaiwuVillageLocation().Equals(location);
			if (flag)
			{
				bool flag2 = chickenKeyList.Count > 0 && this._chicken[chickenKeyList[0]].TemplateId != 63;
				if (flag2)
				{
					int kingId = this.GetChickenKingId();
					bool flag3 = kingId >= 0;
					if (flag3)
					{
						chickenKeyList.Remove(kingId);
						chickenKeyList.Insert(0, kingId);
					}
				}
			}
		}

		// Token: 0x06007C72 RID: 31858 RVA: 0x0049B72C File Offset: 0x0049992C
		private void FixChickenKing(DataContext context)
		{
			short mainStoryLineProgress = DomainManager.World.GetMainStoryLineProgress();
			bool flag = mainStoryLineProgress < 16;
			if (!flag)
			{
				bool flag2 = this._chicken.Count == 1 && DomainManager.Extra.IsDreamBack();
				if (flag2)
				{
					this.ForceInitMapBlockChicken(context);
				}
				else
				{
					foreach (int key in this._chicken.Keys)
					{
						bool flag3 = this._chicken[key].TemplateId == 63;
						if (flag3)
						{
							return;
						}
					}
					short chickenTemplateId = 63;
					int id = this.AddChicken(context, (int)DomainManager.Taiwu.GetTaiwuVillageSettlementId(), chickenTemplateId);
					this.SetChickenHappiness(context, id, 100);
					BuildingDomain.Logger.Warn("Fix chicken king missing.");
				}
			}
		}

		// Token: 0x06007C73 RID: 31859 RVA: 0x0049B81C File Offset: 0x00499A1C
		[DomainMethod]
		public List<int> GetSettlementChickenIdList(Location location)
		{
			int sourceSettlementId = (int)DomainManager.Organization.GetSettlementByLocation(location).GetId();
			List<int> chickenIdList = this.GetSettlementChickenList(sourceSettlementId, true);
			this.MakeSureChickenKingFirst(ref chickenIdList, location);
			return chickenIdList;
		}

		// Token: 0x06007C74 RID: 31860 RVA: 0x0049B854 File Offset: 0x00499A54
		[DomainMethod]
		public Chicken GetChickenData(int id)
		{
			Chicken chicken;
			return (!this.TryGetElement_Chicken(id, out chicken)) ? default(Chicken) : chicken;
		}

		// Token: 0x06007C75 RID: 31861 RVA: 0x0049B880 File Offset: 0x00499A80
		[DomainMethod]
		public List<Chicken> GetChickenDataList(List<int> idList)
		{
			List<Chicken> chickenList = new List<Chicken>();
			bool flag = idList == null;
			List<Chicken> result;
			if (flag)
			{
				result = chickenList;
			}
			else
			{
				foreach (int id in idList)
				{
					Chicken chicken;
					bool flag2 = this.TryGetElement_Chicken(id, out chicken);
					if (flag2)
					{
						chickenList.Add(chicken);
					}
					else
					{
						chickenList.Add(default(Chicken));
					}
				}
				result = chickenList;
			}
			return result;
		}

		// Token: 0x06007C76 RID: 31862 RVA: 0x0049B914 File Offset: 0x00499B14
		[DomainMethod]
		public void SetNickNameByChickenId(DataContext context, int id, string nickname)
		{
			bool flag = nickname == null;
			if (flag)
			{
				nickname = string.Empty;
			}
			bool hasNickname = DomainManager.Extra.CheckChickenHasNickname(id);
			bool flag2 = hasNickname;
			int textId;
			if (flag2)
			{
				textId = DomainManager.Extra.GetElement_NicknameDict(id);
				string customName;
				bool flag3 = DomainManager.World.TryGetElement_CustomTexts(textId, out customName) && !string.IsNullOrEmpty(customName) && customName.Equals(nickname);
				if (flag3)
				{
					return;
				}
				DomainManager.World.UnregisterCustomText(context, textId);
			}
			textId = DomainManager.World.RegisterCustomText(context, nickname);
			DomainManager.Extra.SetNicknameByChickenId(id, textId, context);
		}

		// Token: 0x06007C77 RID: 31863 RVA: 0x0049B9A4 File Offset: 0x00499BA4
		[Obsolete]
		[DomainMethod]
		public List<string> GetChickensNicknameByIdList(Location location)
		{
			return this.GetChickensNicknameByLocation(location);
		}

		// Token: 0x06007C78 RID: 31864 RVA: 0x0049B9C0 File Offset: 0x00499BC0
		[DomainMethod]
		public List<string> GetChickensNicknameByLocation(Location location)
		{
			int sourceSettlementId = (int)DomainManager.Organization.GetSettlementByLocation(location).GetId();
			List<int> chickens = this.GetSettlementChickenList(sourceSettlementId, true);
			return this.GetChickenNicknameList(chickens);
		}

		// Token: 0x06007C79 RID: 31865 RVA: 0x0049B9F4 File Offset: 0x00499BF4
		[DomainMethod]
		public List<string> GetChickenNicknameList(List<int> chickenIdList)
		{
			List<string> nicknames = new List<string>();
			bool flag = chickenIdList == null;
			List<string> result;
			if (flag)
			{
				result = nicknames;
			}
			else
			{
				foreach (int id in chickenIdList)
				{
					bool flag2 = DomainManager.Extra.CheckChickenHasNickname(id);
					if (flag2)
					{
						int textId = DomainManager.Extra.GetElement_NicknameDict(id);
						nicknames.Add(DomainManager.World.GetElement_CustomTexts(textId));
					}
					else
					{
						nicknames.Add("");
					}
				}
				result = nicknames;
			}
			return result;
		}

		// Token: 0x06007C7A RID: 31866 RVA: 0x0049BAA0 File Offset: 0x00499CA0
		[DomainMethod]
		[Obsolete("可以使用FeedChickenWithArgs替代")]
		public sbyte FeedChicken(DataContext context, int id, ItemKey itemKey)
		{
			return this.FeedChickenWithArgs(context, id, itemKey, ItemSourceType.Inventory);
		}

		// Token: 0x06007C7B RID: 31867 RVA: 0x0049BABC File Offset: 0x00499CBC
		[DomainMethod]
		public bool SetFulongChicken(DataContext context, short orgMemberTemplateId, int chickenId)
		{
			IntList chickenIds;
			DomainManager.Extra.TryGetElement_SectFulongOrgMemberChickens(orgMemberTemplateId, out chickenIds);
			List<int> items = chickenIds.Items;
			bool flag = items != null && items.Contains(chickenId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ref List<int> ptr = ref chickenIds.Items;
				if (ptr == null)
				{
					ptr = new List<int>();
				}
				chickenIds.Items.Add(chickenId);
				DomainManager.Extra.AddOrSetFulongChickens(context, orgMemberTemplateId, chickenIds);
				result = true;
			}
			return result;
		}

		// Token: 0x06007C7C RID: 31868 RVA: 0x0049BB28 File Offset: 0x00499D28
		[DomainMethod]
		public bool UnsetFulongChicken(DataContext context, short orgMemberTemplateId, int chickenId)
		{
			IntList chickenIds;
			DomainManager.Extra.TryGetElement_SectFulongOrgMemberChickens(orgMemberTemplateId, out chickenIds);
			List<int> items = chickenIds.Items;
			bool flag = items == null || !items.Contains(chickenId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				chickenIds.Items.Remove(chickenId);
				DomainManager.Extra.AddOrSetFulongChickens(context, orgMemberTemplateId, chickenIds);
				result = true;
			}
			return result;
		}

		// Token: 0x06007C7D RID: 31869 RVA: 0x0049BB82 File Offset: 0x00499D82
		public IEnumerable<Chicken> GetFulongChickens(short orgMemberTemplateId)
		{
			IntList chickens;
			bool flag = !DomainManager.Extra.TryGetElement_SectFulongOrgMemberChickens(orgMemberTemplateId, out chickens);
			if (flag)
			{
				yield break;
			}
			List<int> items = chickens.Items;
			bool flag2 = items == null || items.Count <= 0;
			if (flag2)
			{
				yield break;
			}
			short taiwuSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			foreach (int chickenId in chickens.Items)
			{
				Chicken chicken;
				bool flag3 = this._chicken.TryGetValue(chickenId, out chicken) && chicken.CurrentSettlementId == (int)taiwuSettlementId;
				if (flag3)
				{
					yield return chicken;
				}
				chicken = default(Chicken);
			}
			List<int>.Enumerator enumerator = default(List<int>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06007C7E RID: 31870 RVA: 0x0049BB9C File Offset: 0x00499D9C
		public bool HasFulongChicken(VillagerRoleItem role)
		{
			IntList chickenList;
			bool flag;
			if (DomainManager.Extra.TryGetElement_SectFulongOrgMemberChickens(role.OrganizationMember, out chickenList))
			{
				List<int> items = chickenList.Items;
				flag = (items == null || items.Count <= 0);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			return !flag2 && BuildingDomain.IsVillagerRoleExtraEffectUnlockState(role);
		}

		// Token: 0x06007C7F RID: 31871 RVA: 0x0049BBEC File Offset: 0x00499DEC
		[DomainMethod]
		public List<bool> GetVillagerRoleExtraEffectUnlockState()
		{
			List<bool> result = new List<bool>();
			foreach (VillagerRoleItem roleConfig in ((IEnumerable<VillagerRoleItem>)VillagerRole.Instance))
			{
				result.Add(BuildingDomain.IsVillagerRoleExtraEffectUnlockState(roleConfig));
			}
			return result;
		}

		// Token: 0x06007C80 RID: 31872 RVA: 0x0049BC50 File Offset: 0x00499E50
		private static bool IsVillagerRoleExtraEffectUnlockState(VillagerRoleItem roleConfig)
		{
			bool found = false;
			int[] hasPersonalityCount = new int[roleConfig.NeedPersonalityList.Length];
			foreach (Chicken chicken in DomainManager.Building.GetFulongChickens(roleConfig.OrganizationMember))
			{
				found = true;
				ChickenItem chickenConfig = Chicken.Instance[chicken.TemplateId];
				for (int i = 0; i < roleConfig.NeedPersonalityList.Length; i++)
				{
					bool flag = chickenConfig.PersonalityType == roleConfig.NeedPersonalityList[i].PersonalityType;
					if (flag)
					{
						hasPersonalityCount[i]++;
					}
				}
			}
			bool flag2 = !found;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				bool isUnlock = true;
				for (int j = 0; j < roleConfig.NeedPersonalityList.Length; j++)
				{
					bool flag3 = hasPersonalityCount[j] < roleConfig.NeedPersonalityList[j].NeedCount;
					if (flag3)
					{
						isUnlock = false;
						break;
					}
				}
				result = isUnlock;
			}
			return result;
		}

		// Token: 0x06007C81 RID: 31873 RVA: 0x0049BD78 File Offset: 0x00499F78
		private sbyte FeedChickenWithArgs(DataContext context, int templateId, ItemKey itemKey, ItemSourceType itemSourceType)
		{
			sbyte resValue = 0;
			Chicken chicken;
			bool flag = !this.TryGetElement_ChickenByTemplateId(templateId, out chicken);
			sbyte result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 1204;
				if (flag2)
				{
					resValue = GlobalConfig.Instance.ChickenMiscTaste;
					chicken.Happiness += GlobalConfig.Instance.ChickenMiscTaste;
				}
				else
				{
					bool flag3 = itemKey.ItemType == 11;
					if (flag3)
					{
						ItemDisplayData cricket = DomainManager.Item.GetItemDisplayData(itemKey, -1);
						bool flag4 = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId) >= 7;
						CricketPartsItem part;
						if (flag4)
						{
							part = CricketParts.Instance[(int)(cricket.CricketPartId + cricket.CricketColorId)];
						}
						else
						{
							part = CricketParts.Instance[cricket.CricketPartId];
						}
						sbyte taste = part.Taste;
						bool flag5 = part.Type != ECricketPartsType.Trash && part.Type != ECricketPartsType.RealColor && part.Type != ECricketPartsType.King;
						if (flag5)
						{
							taste += CricketParts.Instance[cricket.CricketColorId].Taste;
						}
						resValue = taste;
						chicken.Happiness += taste;
					}
					else
					{
						bool flag6 = itemKey.ItemType == 5;
						if (flag6)
						{
							resValue = DomainManager.Item.GetElement_Materials(itemKey.Id).GetHappinessChange();
							chicken.Happiness += resValue;
						}
					}
				}
				bool flag7 = chicken.Happiness < 0;
				if (flag7)
				{
					chicken.Happiness = 100;
				}
				chicken.Happiness = Math.Max(0, Math.Min(chicken.Happiness, 100));
				this.SetElement_Chicken(chicken.Id, chicken, context);
				if (itemSourceType != ItemSourceType.Inventory)
				{
					if (itemSourceType == ItemSourceType.Trough)
					{
						DomainManager.Extra.TroughRemove(context, itemKey, 1, true);
					}
				}
				else
				{
					GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
					taiwu.RemoveInventoryItem(context, itemKey, 1, true, false);
				}
				result = resValue;
			}
			return result;
		}

		// Token: 0x06007C82 RID: 31874 RVA: 0x0049BF80 File Offset: 0x0049A180
		public bool TryGetFirstChickenInSettlement(Settlement settlement, out Chicken chicken)
		{
			chicken = default(Chicken);
			short settlementId = settlement.GetId();
			foreach (KeyValuePair<int, Chicken> kvp in this._chicken)
			{
				bool flag = kvp.Value.CurrentSettlementId == (int)settlementId;
				if (flag)
				{
					chicken = kvp.Value;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C83 RID: 31875 RVA: 0x0049C00C File Offset: 0x0049A20C
		public bool TryGetElement_ChickenByTemplateId(int templateId, out Chicken value)
		{
			foreach (int key in this._chicken.Keys)
			{
				bool flag = (int)this._chicken[key].TemplateId == templateId;
				if (flag)
				{
					value = this._chicken[key];
					return true;
				}
			}
			value = default(Chicken);
			return false;
		}

		// Token: 0x06007C84 RID: 31876 RVA: 0x0049C0A0 File Offset: 0x0049A2A0
		internal bool CanSettlementHaveChicken(Location settlementLocation)
		{
			short blockId = settlementLocation.BlockId;
			bool flag = blockId >= 0;
			if (flag)
			{
				MapBlockData block = DomainManager.Map.GetBlock(settlementLocation.AreaId, blockId);
				bool flag2 = block.TemplateId == 34 || block.TemplateId == 35 || block.TemplateId == 36 || block.BlockType == EMapBlockType.City;
				if (flag2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C85 RID: 31877 RVA: 0x0049C110 File Offset: 0x0049A310
		internal List<int> GetChickenSettlements()
		{
			List<int> chickenSettlements = new List<int>();
			for (short areaId = 0; areaId < 135; areaId += 1)
			{
				MapAreaData area = DomainManager.Map.GetElement_Areas((int)areaId);
				foreach (SettlementInfo settlementInfo in area.SettlementInfos)
				{
					bool flag = this.CanSettlementHaveChicken(new Location(areaId, settlementInfo.BlockId));
					if (flag)
					{
						chickenSettlements.Add((int)settlementInfo.SettlementId);
					}
				}
			}
			return chickenSettlements;
		}

		// Token: 0x06007C86 RID: 31878 RVA: 0x0049C1A0 File Offset: 0x0049A3A0
		protected List<int> _GetChickenVillageSettlements()
		{
			List<int> chickenSettlements = new List<int>();
			for (short areaId = 0; areaId < 135; areaId += 1)
			{
				MapAreaData area = DomainManager.Map.GetElement_Areas((int)areaId);
				foreach (SettlementInfo settlementInfo in area.SettlementInfos)
				{
					short blockId = settlementInfo.BlockId;
					bool flag = blockId >= 0;
					if (flag)
					{
						MapBlockData block = DomainManager.Map.GetBlock(areaId, blockId);
						bool flag2 = block.TemplateId == 34;
						if (flag2)
						{
							chickenSettlements.Add((int)settlementInfo.SettlementId);
						}
					}
				}
			}
			return chickenSettlements;
		}

		// Token: 0x06007C87 RID: 31879 RVA: 0x0049C258 File Offset: 0x0049A458
		[DomainMethod]
		public void InitMapBlockChicken(DataContext context)
		{
			bool flag = DomainManager.Extra.IsDreamBack() && this._chicken.Count != 1;
			if (!flag)
			{
				this.ForceInitMapBlockChicken(context);
			}
		}

		// Token: 0x06007C88 RID: 31880 RVA: 0x0049C294 File Offset: 0x0049A494
		internal void ForceInitMapBlockChicken(DataContext context)
		{
			List<short> chicken = new List<short>();
			List<int> chickenSettlements = this.GetChickenSettlements();
			for (int i = 0; i < Chicken.Instance.Count; i++)
			{
				bool flag = Chicken.Instance[i].TemplateId == 63;
				if (!flag)
				{
					chicken.Add(Chicken.Instance[i].TemplateId);
				}
			}
			CollectionUtils.Shuffle<int>(context.Random, chickenSettlements);
			bool hasChickenKing = false;
			Chicken king = default(Chicken);
			foreach (int key in this._chicken.Keys)
			{
				bool flag2 = this._chicken[key].TemplateId == 63;
				if (flag2)
				{
					hasChickenKing = true;
					king = this._chicken[key];
				}
			}
			this.ClearChicken(context);
			bool flag3 = hasChickenKing;
			if (flag3)
			{
				this.AddElement_Chicken(this._chicken.Count, king, context);
			}
			while (chicken.Count > 0 && chickenSettlements.Count > 0)
			{
				short chickTemplateId = chicken[0];
				int chickSettlement = chickenSettlements[0];
				this.AddChicken(context, chickSettlement, chickTemplateId);
				chicken.RemoveAt(0);
				chickenSettlements.RemoveAt(0);
			}
		}

		// Token: 0x06007C89 RID: 31881 RVA: 0x0049C408 File Offset: 0x0049A608
		[DomainMethod]
		public bool IsHaveChickenKing(DataContext context)
		{
			short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			List<int> idList = this.GetSettlementChickenList((int)settlementId, true);
			bool flag = idList.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < idList.Count; i++)
				{
					bool flag2 = this._chicken[idList[i]].TemplateId == 63;
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007C8A RID: 31882 RVA: 0x0049C484 File Offset: 0x0049A684
		public void UpdateChickenInstances(DataContext context)
		{
			Dictionary<int, List<int>> chickenSettlementTable = new Dictionary<int, List<int>>();
			foreach (int settlementId in this.GetChickenSettlements())
			{
				chickenSettlementTable.Add(settlementId, new List<int>());
			}
			foreach (int id in this._chicken.Keys.ToArray<int>())
			{
				Chicken chicken = this._chicken[id];
				bool flag = !chickenSettlementTable.ContainsKey(chicken.CurrentSettlementId);
				if (flag)
				{
					chickenSettlementTable.Add(chicken.CurrentSettlementId, new List<int>());
				}
				chickenSettlementTable[chicken.CurrentSettlementId].Add(chicken.Id);
				bool flag2 = id != chicken.Id;
				if (flag2)
				{
					chicken.Id = id;
					this.SetElement_Chicken(id, chicken, context);
				}
			}
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			Chicken[] array2 = this._chicken.Values.ToArray<Chicken>();
			for (int j = 0; j < array2.Length; j++)
			{
				Chicken item = array2[j];
				Chicken chicken2 = item;
				bool flag3 = chicken2.TemplateId == 63;
				if (flag3)
				{
					bool flag4 = chicken2.Happiness != 100;
					if (flag4)
					{
						this.SetChickenHappiness(context, chicken2.Id, 100);
					}
				}
				else
				{
					bool flag5 = (int)DomainManager.Taiwu.GetTaiwuVillageSettlementId() == chicken2.CurrentSettlementId;
					if (flag5)
					{
						int chickenDecayMultiple = 1;
						Location location = DomainManager.Organization.GetSettlement(DomainManager.Taiwu.GetTaiwuVillageSettlementId()).GetLocation();
						int chickenCoopCount = 0;
						foreach (BuildingBlockData buildingBlock in DomainManager.Building.GetBuildingBlockList(location))
						{
							bool flag6 = buildingBlock.TemplateId == 49;
							if (flag6)
							{
								chickenCoopCount++;
							}
						}
						bool flag7 = chickenCoopCount == 0;
						if (flag7)
						{
							chickenDecayMultiple = 3;
						}
						chicken2.Happiness = (sbyte)Math.Max(0, (int)chicken2.Happiness - chickenDecayMultiple * context.Random.Next((int)GlobalConfig.Instance.ChickenDecayMin, (int)GlobalConfig.Instance.ChickenDecayMax));
						this.SetChickenHappiness(context, chicken2.Id, chicken2.Happiness);
						while (chicken2.Happiness < 50 && DomainManager.Extra.TroughItems.Count > 0)
						{
							ItemKey itemKey = (from pair in DomainManager.Extra.TroughItems
							orderby ItemTemplateHelper.GetGrade(pair.Key.ItemType, pair.Key.TemplateId)
							select pair).First<KeyValuePair<ItemKey, int>>().Key;
							sbyte change = this.FeedChickenWithArgs(context, (int)chicken2.TemplateId, itemKey, ItemSourceType.Trough);
							chicken2.Happiness += change;
						}
						bool flag8 = chicken2.Happiness <= 0;
						if (flag8)
						{
							int? nextTargetSettlementId = null;
							foreach (KeyValuePair<int, List<int>> keyValuePair in chickenSettlementTable)
							{
								int num;
								List<int> list2;
								keyValuePair.Deconstruct(out num, out list2);
								int settlementId2 = num;
								List<int> list = list2;
								bool flag9 = list.Count == 0;
								if (flag9)
								{
									nextTargetSettlementId = new int?(settlementId2);
									list.Add(chicken2.Id);
									break;
								}
							}
							chickenSettlementTable[chicken2.CurrentSettlementId].Remove(chicken2.Id);
							bool flag10 = nextTargetSettlementId != null;
							if (!flag10)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
								defaultInterpolatedStringHandler.AppendLiteral("chicken: #");
								defaultInterpolatedStringHandler.AppendFormatted<int>(chicken2.Id);
								defaultInterpolatedStringHandler.AppendLiteral(" cannot find a next settlement for escape");
								throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							chicken2.CurrentSettlementId = nextTargetSettlementId.Value;
							monthlyNotifications.AddChickenEscaped(chicken2.TemplateId);
						}
					}
					short featureId = Chicken.Instance[chicken2.TemplateId].FeatureId;
					bool flag11 = featureId >= 0;
					if (flag11)
					{
						int[] characterList = (from data in DomainManager.Organization.GetSettlementMembers((short)chicken2.CurrentSettlementId)
						select data.CharacterId).Where(delegate(int characterId)
						{
							GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(characterId);
							List<short> chickenFeatures = character2.GetChickenFeatures();
							return !chickenFeatures.Contains(featureId);
						}).ToArray<int>();
						bool flag12 = characterList.Any<int>();
						if (flag12)
						{
							CollectionUtils.Shuffle<int>(context.Random, characterList);
							int characterId2 = characterList.First<int>();
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(characterId2);
							character.AddFeature(context, featureId, false);
						}
					}
					this.SetElement_Chicken(item.Id, chicken2, context);
				}
			}
		}

		// Token: 0x06007C8B RID: 31883 RVA: 0x0049C9AC File Offset: 0x0049ABAC
		public void SetChickenHappiness(DataContext context, int chickenId, sbyte happiness)
		{
			Chicken chicken = this.GetChickenData(chickenId);
			chicken.Happiness = Math.Clamp(happiness, 0, 100);
			this.SetElement_Chicken(chickenId, chicken, context);
		}

		// Token: 0x06007C8C RID: 31884 RVA: 0x0049C9DC File Offset: 0x0049ABDC
		private int GenerateNextChickenId()
		{
			int id = this._nextChickenId;
			this._nextChickenId++;
			return id;
		}

		// Token: 0x06007C8D RID: 31885 RVA: 0x0049CA04 File Offset: 0x0049AC04
		private void InitializeNextChickenId()
		{
			foreach (int id in this._chicken.Keys)
			{
				bool flag = id >= this._nextChickenId;
				if (flag)
				{
					this._nextChickenId = id + 1;
				}
			}
		}

		// Token: 0x06007C8E RID: 31886 RVA: 0x0049CA74 File Offset: 0x0049AC74
		public bool isChickenBlessingInfoEmpty()
		{
			return this._chickenBlessingInfoData == null || this._chickenBlessingInfoData.Count == 0;
		}

		// Token: 0x06007C8F RID: 31887 RVA: 0x0049CA9F File Offset: 0x0049AC9F
		public void ClearChickenBlessingInfo(DataContext context)
		{
			this.ClearChickenBlessingInfoData(context);
		}

		// Token: 0x06007C90 RID: 31888 RVA: 0x0049CAAC File Offset: 0x0049ACAC
		internal sbyte BuildingBlockLevel(BuildingBlockKey blockKey)
		{
			bool isInvalid = blockKey.IsInvalid;
			sbyte result;
			if (isInvalid)
			{
				result = 0;
			}
			else
			{
				Location taiwuLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
				Location location = new Location(blockKey.AreaId, blockKey.BlockId);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
				bool flag = !taiwuLocation.Equals(location);
				if (flag)
				{
					result = Math.Min(blockData.Level, config.MaxLevel);
				}
				else
				{
					bool flag2 = config == null;
					if (flag2)
					{
						result = 1;
					}
					else
					{
						bool flag3 = config.MaxLevel > 1;
						if (flag3)
						{
							bool flag4 = config.Type == EBuildingBlockType.UselessResource;
							if (flag4)
							{
								result = blockData.Level;
							}
							else
							{
								BuildingBlockDataEx dataEx;
								bool flag5 = DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)blockKey, out dataEx);
								if (flag5)
								{
									result = Math.Min(config.MaxLevel, dataEx.CalcUnlockedLevelCount());
								}
								else
								{
									result = config.MaxLevel;
								}
							}
						}
						else
						{
							result = config.MaxLevel;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007C91 RID: 31889 RVA: 0x0049CBB0 File Offset: 0x0049ADB0
		internal void BuildingBlockDependencies(BuildingBlockKey blockKey, Action<BuildingBlockData, int, BuildingBlockKey> onDependencyFound)
		{
			BuildingAreaData areaData;
			BuildingBlockData buildingBlockData;
			bool flag = !this.TryGetElement_BuildingAreas(blockKey.GetLocation(), out areaData) || !this.TryGetElement_BuildingBlocks(blockKey, out buildingBlockData);
			if (!flag)
			{
				BuildingBlockItem buildingBlockConfig = BuildingBlock.Instance.GetItem(buildingBlockData.TemplateId);
				List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
				List<int> neighborDistanceList = ObjectPool<List<int>>.Instance.Get();
				Dictionary<short, int> resultMap = ObjectPool<Dictionary<short, int>>.Instance.Get();
				resultMap.Clear();
				areaData.GetNeighborBlocks(blockKey.BuildingBlockIndex, buildingBlockConfig.Width, neighborList, neighborDistanceList, 2);
				foreach (short dependBlockTemplateId in buildingBlockConfig.DependBuildings)
				{
					for (int i = 0; i < neighborList.Count; i++)
					{
						BuildingBlockKey neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborList[i]);
						BuildingBlockData neighborBlock;
						bool flag2 = !this.TryGetElement_BuildingBlocks(neighborKey, out neighborBlock);
						if (!flag2)
						{
							bool flag3 = neighborBlock.RootBlockIndex >= 0;
							if (flag3)
							{
								neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, neighborBlock.RootBlockIndex);
								neighborBlock = this.GetElement_BuildingBlocks(neighborKey);
							}
							bool flag4 = neighborBlock.TemplateId != dependBlockTemplateId;
							if (!flag4)
							{
								int currentDistance = neighborDistanceList[i];
								int existDistance;
								bool flag5 = resultMap.TryGetValue(neighborKey.BuildingBlockIndex, out existDistance) && existDistance < currentDistance;
								if (!flag5)
								{
									resultMap[neighborKey.BuildingBlockIndex] = currentDistance;
								}
							}
						}
					}
				}
				foreach (KeyValuePair<short, int> pair in resultMap)
				{
					BuildingBlockKey neighborKey2 = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, pair.Key);
					onDependencyFound(this.GetElement_BuildingBlocks(neighborKey2), pair.Value, neighborKey2);
				}
				ObjectPool<Dictionary<short, int>>.Instance.Return(resultMap);
				ObjectPool<List<short>>.Instance.Return(neighborList);
				ObjectPool<List<int>>.Instance.Return(neighborDistanceList);
			}
		}

		// Token: 0x06007C92 RID: 31890 RVA: 0x0049CE1C File Offset: 0x0049B01C
		internal void BuildingBlockInfluences(BuildingBlockKey blockKey, Action<BuildingBlockData, int> onInfluenceFound)
		{
			BuildingAreaData areaData;
			BuildingBlockData buildingBlockData;
			bool flag = !this.TryGetElement_BuildingAreas(blockKey.GetLocation(), out areaData) || !this.TryGetElement_BuildingBlocks(blockKey, out buildingBlockData);
			if (!flag)
			{
				BuildingBlockItem buildingBlockConfig = BuildingBlock.Instance.GetItem(buildingBlockData.TemplateId);
				List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
				List<int> neighborDistanceList = ObjectPool<List<int>>.Instance.Get();
				areaData.GetNeighborBlocks(blockKey.BuildingBlockIndex, buildingBlockConfig.Width, neighborList, neighborDistanceList, 2);
				buildingBlockData.CalcInfluences(neighborList.Select(delegate(short index)
				{
					BuildingBlockKey neighborKey = new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, index);
					BuildingBlockData neighbor;
					bool flag2 = this.TryGetElement_BuildingBlocks(neighborKey, out neighbor);
					BuildingBlockData result;
					if (flag2)
					{
						result = neighbor;
					}
					else
					{
						result = null;
					}
					return result;
				}), neighborDistanceList, onInfluenceFound);
				ObjectPool<List<short>>.Instance.Return(neighborList);
				ObjectPool<List<int>>.Instance.Return(neighborDistanceList);
			}
		}

		// Token: 0x06007C93 RID: 31891 RVA: 0x0049CEEC File Offset: 0x0049B0EC
		internal int BuildingTotalAttainment(BuildingBlockKey blockKey, sbyte resourceType, out bool hasManager, bool ignoreCanWork = false)
		{
			BuildingBlockData blockData;
			CharacterList managerList;
			bool flag = this.TryGetElement_BuildingBlocks(blockKey, out blockData) && this.TryGetElement_ShopManagerDict(blockKey, out managerList);
			int result;
			if (flag)
			{
				hasManager = false;
				int sum = 0;
				int leaderValue = 150;
				BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
				bool isCollectResourceBuilding = config.IsCollectResourceBuilding;
				if (isCollectResourceBuilding)
				{
					int i = 0;
					while (i < managerList.GetCount())
					{
						int charId = managerList.GetCollection()[i];
						bool flag2 = GameData.Domains.Character.Character.IsCharacterIdValid(charId) && (ignoreCanWork || DomainManager.Taiwu.CanWork(charId));
						if (flag2)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							bool flag3 = character.GetAgeGroup() != 2;
							if (!flag3)
							{
								GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
								bool flag4 = i == 0;
								if (flag4)
								{
									leaderValue += (int)manageChar.GetLifeSkillAttainment(config.RequireLifeSkillType);
								}
								else
								{
									sum += (int)(50 + manageChar.GetLifeSkillAttainment(config.RequireLifeSkillType));
								}
								hasManager = true;
							}
						}
						else
						{
							bool flag5 = i == 0 && config.NeedLeader;
							if (flag5)
							{
								return 0;
							}
						}
						IL_117:
						i++;
						continue;
						goto IL_117;
					}
				}
				else
				{
					int j = 0;
					while (j < managerList.GetCount())
					{
						int charId2 = managerList.GetCollection()[j];
						bool flag6 = GameData.Domains.Character.Character.IsCharacterIdValid(charId2) && (ignoreCanWork || DomainManager.Taiwu.CanWork(charId2));
						if (flag6)
						{
							GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(charId2);
							bool flag7 = character2.GetAgeGroup() != 2;
							if (!flag7)
							{
								GameData.Domains.Character.Character manageChar2 = DomainManager.Character.GetElement_Objects(charId2);
								short attainment = manageChar2.GetLifeSkillAttainment(config.RequireLifeSkillType);
								bool flag8 = j == 0;
								if (flag8)
								{
									leaderValue += (int)attainment;
								}
								else
								{
									sum += (int)(50 + attainment);
								}
								hasManager = true;
							}
						}
						else
						{
							bool flag9 = j == 0 && config.NeedLeader;
							if (flag9)
							{
								return 0;
							}
						}
						IL_1F5:
						j++;
						continue;
						goto IL_1F5;
					}
				}
				result = leaderValue + sum / GlobalConfig.Instance.BuildingTotalAttainmentFinalDivisor;
			}
			else
			{
				hasManager = false;
				result = 0;
			}
			return result;
		}

		// Token: 0x06007C94 RID: 31892 RVA: 0x0049D128 File Offset: 0x0049B328
		internal int BuildingMaxAttainment(BuildingBlockKey blockKey, sbyte resourceType, out bool hasManager)
		{
			hasManager = false;
			BuildingBlockData blockData;
			CharacterList managerList;
			bool flag = !this.TryGetElement_BuildingBlocks(blockKey, out blockData) || !this.TryGetElement_ShopManagerDict(blockKey, out managerList);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
				int charId = managerList.GetCollection()[0];
				bool flag2 = !GameData.Domains.Character.Character.IsCharacterIdValid(charId) || !DomainManager.Taiwu.CanWork(charId);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
					hasManager = true;
					result = (int)(150 + ((config.RequireLifeSkillType >= 0) ? manageChar.GetLifeSkillAttainment(config.RequireLifeSkillType) : manageChar.GetCombatSkillAttainment(config.RequireCombatSkillType)));
				}
			}
			return result;
		}

		// Token: 0x06007C95 RID: 31893 RVA: 0x0049D1E8 File Offset: 0x0049B3E8
		internal int BuildingTotalAverageAttainment(BuildingBlockKey blockKey, sbyte resourceType, out bool hasManager, bool ignoreCanWork = false)
		{
			BuildingBlockData blockData;
			CharacterList managerList;
			bool flag = this.TryGetElement_BuildingBlocks(blockKey, out blockData) && this.TryGetElement_ShopManagerDict(blockKey, out managerList);
			int result;
			if (flag)
			{
				hasManager = false;
				int sum = 0;
				BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
				bool isCollectResourceBuilding = config.IsCollectResourceBuilding;
				if (isCollectResourceBuilding)
				{
					for (int i = 0; i < managerList.GetCount(); i++)
					{
						int charId = managerList.GetCollection()[i];
						bool flag2 = GameData.Domains.Character.Character.IsCharacterIdValid(charId) && (ignoreCanWork || DomainManager.Taiwu.CanWork(charId));
						if (flag2)
						{
							GameData.Domains.Character.Character manageChar = DomainManager.Character.GetElement_Objects(charId);
							sum += (int)manageChar.GetLifeSkillAttainment(config.RequireLifeSkillType);
							hasManager = true;
						}
					}
				}
				else
				{
					for (int j = 0; j < managerList.GetCount(); j++)
					{
						int charId2 = managerList.GetCollection()[j];
						bool flag3 = GameData.Domains.Character.Character.IsCharacterIdValid(charId2) && (ignoreCanWork || DomainManager.Taiwu.CanWork(charId2));
						if (flag3)
						{
							GameData.Domains.Character.Character manageChar2 = DomainManager.Character.GetElement_Objects(charId2);
							short attainment = manageChar2.GetLifeSkillAttainment(config.RequireLifeSkillType);
							sum += (int)attainment;
							hasManager = true;
						}
					}
				}
				result = sum / managerList.GetCount();
			}
			else
			{
				hasManager = false;
				result = 0;
			}
			return result;
		}

		// Token: 0x06007C96 RID: 31894 RVA: 0x0049D348 File Offset: 0x0049B548
		internal sbyte BuildLeaderFameType(BuildingBlockKey blockKey)
		{
			GameData.Domains.Character.Character leader = this.GetShopManagerLeader(blockKey);
			bool flag = leader == null;
			sbyte result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte fame = leader.GetFame();
				result = FameType.GetFameType(fame);
			}
			return result;
		}

		// Token: 0x06007C97 RID: 31895 RVA: 0x0049D37C File Offset: 0x0049B57C
		internal int BuildingProductivityByMaxDependencies(BuildingBlockKey blockKey)
		{
			BuildingAreaData areaData;
			BuildingBlockData buildingBlockData;
			bool flag = !this.TryGetElement_BuildingAreas(blockKey.GetLocation(), out areaData) || !this.TryGetElement_BuildingBlocks(blockKey, out buildingBlockData);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int total = 0;
				BuildingBlockItem buildingBlockConfig = BuildingBlock.Instance.GetItem(buildingBlockData.TemplateId);
				List<ValueTuple<BuildingBlockData, int>> set = new List<ValueTuple<BuildingBlockData, int>>();
				this.BuildingBlockDependencies(blockKey, delegate(BuildingBlockData data, int distance, BuildingBlockKey _)
				{
					set.Add(new ValueTuple<BuildingBlockData, int>(data, distance));
				});
				using (List<short>.Enumerator enumerator = buildingBlockConfig.DependBuildings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						short dependTemplateId = enumerator.Current;
						int max = 0;
						IEnumerable<ValueTuple<BuildingBlockData, int>> set2 = set;
						Func<ValueTuple<BuildingBlockData, int>, bool> predicate;
						Func<ValueTuple<BuildingBlockData, int>, bool> <>9__1;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((ValueTuple<BuildingBlockData, int> b) => b.Item1.TemplateId == dependTemplateId));
						}
						foreach (ValueTuple<BuildingBlockData, int> dependency in set2.Where(predicate))
						{
							int value = this.BuildingProductivityBySingleDependency(new BuildingBlockKey(blockKey.AreaId, blockKey.BlockId, dependency.Item1.BlockIndex), dependency.Item2);
							bool flag2 = value > max;
							if (flag2)
							{
								max = value;
							}
						}
						total += max;
					}
				}
				result = total;
			}
			return result;
		}

		// Token: 0x06007C98 RID: 31896 RVA: 0x0049D4FC File Offset: 0x0049B6FC
		internal int BuildingProductivityBySingleDependency(BuildingBlockKey buildingBlockKey, int dependencyDistance)
		{
			return (dependencyDistance > 1) ? 20 : 100;
		}

		// Token: 0x06007C99 RID: 31897 RVA: 0x0049D518 File Offset: 0x0049B718
		internal ResourceInts BuildingBaseYield(BuildingBlockData resourceBlockData)
		{
			ResourceInts res = default(ResourceInts);
			BuildingBlockItem config = BuildingBlock.Instance.GetItem(resourceBlockData.TemplateId);
			for (int i = 0; i < config.CollectResourcePercent.Length; i++)
			{
				bool flag = config.CollectResourcePercent[i] > 0;
				if (flag)
				{
					int value = (int)(50 + 50 * config.CollectResourcePercent[i]);
					res.Add((sbyte)Math.Min(i, 5), value);
				}
			}
			return res;
		}

		// Token: 0x06007C9A RID: 31898 RVA: 0x0049D594 File Offset: 0x0049B794
		internal int BuildingResourceYieldLevel(BuildingBlockKey blockKey, short templateId)
		{
			sbyte level = this.BuildingBlockLevel(blockKey);
			return (int)GameData.Domains.Building.SharedMethods.GetBuildingLevelEffect(templateId, (int)level);
		}

		// Token: 0x06007C9B RID: 31899 RVA: 0x0049D5B8 File Offset: 0x0049B7B8
		internal int BuildingRandomCorrection(int value, IRandomSource randomSource)
		{
			return value * randomSource.Next(GlobalConfig.Instance.BuildingOutputRandomFactorLowerLimit, GlobalConfig.Instance.BuildingOutputRandomFactorUpperLimit) / 100;
		}

		// Token: 0x06007C9C RID: 31900 RVA: 0x0049D5EC File Offset: 0x0049B7EC
		internal int BuildingManagerSpecificMaxCharacterId(BuildingBlockKey blockKey, Func<int, int> spec)
		{
			int maxSpecValue = 0;
			int maxSpecValueCharId = -1;
			CharacterList managerList;
			DomainManager.Building.TryGetElement_ShopManagerDict(blockKey, out managerList);
			for (int i = 0; i < managerList.GetCount(); i++)
			{
				int charId = managerList.GetCollection()[i];
				bool flag = charId >= 0 && DomainManager.Taiwu.CanWork(charId);
				if (flag)
				{
					int specValue = spec(charId);
					bool flag2 = specValue >= maxSpecValue;
					if (flag2)
					{
						maxSpecValue = specValue;
						maxSpecValueCharId = charId;
					}
				}
			}
			return maxSpecValueCharId;
		}

		// Token: 0x06007C9D RID: 31901 RVA: 0x0049D678 File Offset: 0x0049B878
		internal static int BuildingManagerAttraction(int charId)
		{
			GameData.Domains.Character.Character manager;
			bool flag = DomainManager.Character.TryGetElement_Objects(charId, out manager);
			int result;
			if (flag)
			{
				result = (int)manager.GetAttraction();
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06007C9E RID: 31902 RVA: 0x0049D6A8 File Offset: 0x0049B8A8
		internal unsafe static int BuildingManagerPersonalitiesSum(int charId)
		{
			GameData.Domains.Character.Character manager;
			bool flag = DomainManager.Character.TryGetElement_Objects(charId, out manager);
			int result;
			if (flag)
			{
				Personalities personalities = manager.GetPersonalities();
				int sum = 0;
				for (int i = 0; i < 7; i++)
				{
					sum += (int)(*(ref personalities.Items.FixedElementField + i));
				}
				result = sum;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06007C9F RID: 31903 RVA: 0x0049D708 File Offset: 0x0049B908
		internal int BuildingManageHarvestSuccessRate(BuildingBlockKey blockKey, int charId)
		{
			BuildingBlockData blockData;
			bool flag = this.TryGetElement_BuildingBlocks(blockKey, out blockData);
			int result;
			if (flag)
			{
				short templateId = blockData.TemplateId;
				int attainment = this.GetBuildingAttainment(blockData, blockKey, false);
				result = attainment / 5;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06007CA0 RID: 31904 RVA: 0x0049D74C File Offset: 0x0049B94C
		internal int BuildingManageHarvestSpecialSuccessRate(BuildingBlockKey blockKey, int charId)
		{
			GameData.Domains.Character.Character targetChar = GameData.Domains.Character.Character.IsCharacterIdValid(charId) ? DomainManager.Character.GetElement_Objects(charId) : this.GetShopManagerLeader(blockKey);
			BuildingBlockData blockData;
			bool flag = this.TryGetElement_BuildingBlocks(blockKey, out blockData);
			if (flag)
			{
				short templateId = blockData.TemplateId;
				short num = templateId;
				if (num == 215)
				{
					int personalities = (targetChar != null) ? targetChar.GetPersonalities().GetSum() : 0;
					return personalities / 10;
				}
				if (num == 216)
				{
					int attraction = (int)((targetChar != null) ? targetChar.GetAttraction() : 0);
					return attraction / 30;
				}
			}
			return -1;
		}

		// Token: 0x06007CA1 RID: 31905 RVA: 0x0049D7E5 File Offset: 0x0049B9E5
		internal int BuildingManageHarvestSuccessRate(BuildingBlockKey blockKey)
		{
			return this.BuildingManageHarvestSuccessRate(blockKey, -1);
		}

		// Token: 0x06007CA2 RID: 31906 RVA: 0x0049D7F0 File Offset: 0x0049B9F0
		public void GetTopLevelBlocks(short templateId, Location location, List<ValueTuple<BuildingBlockKey, int>> blocks, int count)
		{
			blocks.Clear();
			IEnumerable<BuildingBlockData> buildingBlocksAtLocation = this.GetBuildingBlocksAtLocation(location, null);
			foreach (BuildingBlockData block in buildingBlocksAtLocation)
			{
				bool flag = block.TemplateId != templateId;
				if (!flag)
				{
					BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, block.BlockIndex);
					sbyte b2;
					bool canUse = block.CanUse() && this.AllDependBuildingAvailable(blockKey, block.TemplateId, out b2);
					bool flag2 = !canUse;
					if (!flag2)
					{
						sbyte level = this.BuildingBlockLevel(blockKey);
						blocks.Add(new ValueTuple<BuildingBlockKey, int>(blockKey, (int)level));
					}
				}
			}
			blocks.Sort((ValueTuple<BuildingBlockKey, int> a, ValueTuple<BuildingBlockKey, int> b) => b.Item2.CompareTo(a.Item2));
			bool flag3 = blocks.Count > count;
			if (flag3)
			{
				blocks.RemoveRange(count, blocks.Count - count);
			}
		}

		// Token: 0x06007CA3 RID: 31907 RVA: 0x0049D8FC File Offset: 0x0049BAFC
		public void ConsumeResource(DataContext context, sbyte resourceType, int delta)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int orig = taiwu.GetResource(resourceType);
			int consume = Math.Min(orig, delta);
			taiwu.ChangeResource(context, resourceType, -consume);
			delta -= consume;
			bool flag = delta <= 0;
			if (!flag)
			{
				SettlementTreasury treasury = DomainManager.Taiwu.GetTaiwuTreasury();
				orig = treasury.Resources.Get((int)resourceType);
				consume = Math.Min(orig, delta);
				treasury.Resources.Subtract(resourceType, consume);
				DomainManager.Taiwu.SetTaiwuTreasury(context, treasury);
				delta -= consume;
				bool flag2 = delta <= 0;
				if (!flag2)
				{
					this.ConsumeStorageResource(context, resourceType, delta);
				}
			}
		}

		// Token: 0x06007CA4 RID: 31908 RVA: 0x0049D9A0 File Offset: 0x0049BBA0
		private void ConsumeStorageResource(DataContext context, sbyte resourceType, int delta)
		{
			TaiwuVillageStorage storage = DomainManager.Extra.GetStockStorage();
			int orig = storage.Resources.Get((int)resourceType);
			int consume = Math.Min(orig, delta);
			storage.Resources.Subtract(resourceType, consume);
			DomainManager.Extra.CommitTaiwuVillageStorages(context);
		}

		// Token: 0x06007CA5 RID: 31909 RVA: 0x0049D9E8 File Offset: 0x0049BBE8
		private ResourceInts GetAllTaiwuResources()
		{
			ResourceInts res = default(ResourceInts);
			ResourceInts resource = DomainManager.Taiwu.GetAllResources(ItemSourceType.Resources).Item2;
			res.Add(ref resource);
			resource = DomainManager.Taiwu.GetAllResources(ItemSourceType.Treasury).Item2;
			res.Add(ref resource);
			resource = DomainManager.Taiwu.GetAllResources(ItemSourceType.StockStorageWarehouse).Item2;
			res.Add(ref resource);
			return res;
		}

		// Token: 0x06007CA6 RID: 31910 RVA: 0x0049DA58 File Offset: 0x0049BC58
		[DomainMethod]
		public List<BuildingBlockData> GetTaiwuVillageResourceBlockEffectInfo(DataContext context, short templateId)
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			List<ValueTuple<BuildingBlockKey, int>> blocks = new List<ValueTuple<BuildingBlockKey, int>>();
			List<BuildingBlockData> blockDataList = new List<BuildingBlockData>();
			this.GetTopLevelBlocks(templateId, taiwuVillageLocation, blocks, int.MaxValue);
			for (int i = 0; i < blocks.Count; i++)
			{
				blockDataList.Add(this.GetBuildingBlockData(blocks[i].Item1));
			}
			return blockDataList;
		}

		// Token: 0x06007CA7 RID: 31911 RVA: 0x0049DAC5 File Offset: 0x0049BCC5
		[DomainMethod]
		public int CalculateBuildingManageHarvestSuccessRate(BuildingBlockKey blockKey)
		{
			return this.BuildingManageHarvestSuccessRate(blockKey);
		}

		// Token: 0x06007CA8 RID: 31912 RVA: 0x0049DACE File Offset: 0x0049BCCE
		[DomainMethod]
		public int[] CalculateBuildingManageHarvestSuccessRates(BuildingBlockKey blockKey)
		{
			return new int[]
			{
				this.BuildingManageHarvestSuccessRate(blockKey),
				this.BuildingManageHarvestSpecialSuccessRate(blockKey, -1)
			};
		}

		// Token: 0x06007CA9 RID: 31913 RVA: 0x0049DAEC File Offset: 0x0049BCEC
		[DomainMethod]
		public int CalcExtraTaiwuGroupMaxCountByStrategyRoom()
		{
			short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			return this.GetBuildingBlockEffect(settlementId, EBuildingScaleEffect.TaiwuGroupMaxCount, -1);
		}

		// Token: 0x06007CAA RID: 31914 RVA: 0x0049DB15 File Offset: 0x0049BD15
		[DomainMethod]
		public int GetStoreLocation(int type)
		{
			return DomainManager.Extra.GetBuildingDefaultStoreLocation().GetMakeData(type);
		}

		// Token: 0x06007CAB RID: 31915 RVA: 0x0049DB27 File Offset: 0x0049BD27
		[DomainMethod]
		public void SetStoreLocation(DataContext context, int type, int value)
		{
			DomainManager.Extra.GetBuildingDefaultStoreLocation().SetMakeData(type, value);
			DomainManager.Extra.SetBuildingDefaultStoreLocation(DomainManager.Extra.GetBuildingDefaultStoreLocation(), context);
		}

		// Token: 0x06007CAC RID: 31916 RVA: 0x0049DB54 File Offset: 0x0049BD54
		public void CreateBuildingArea(DataContext context, short mapAreaId, short mapBlockId, short mapBlockTemplateId)
		{
			IRandomSource random = context.Random;
			MapBlockItem mapBlockData = MapBlock.Instance[mapBlockTemplateId];
			sbyte areaWidth = mapBlockData.BuildingAreaWidth;
			int blockCount = (int)(areaWidth * areaWidth);
			sbyte maxLevel = mapBlockData.CenterBuildingMaxLevel;
			MapBlockItem mapBlockItem = MapBlock.Instance[mapBlockTemplateId];
			bool flag = this.IsLandFormTypeRandom(mapBlockItem);
			sbyte landFormType;
			if (flag)
			{
				sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(mapAreaId);
				MapStateItem mapStateItem = MapState.Instance[stateTemplateId];
				landFormType = (sbyte)mapStateItem.BornMapType.GetRandom(context.Random);
			}
			else
			{
				landFormType = ((mapBlockTemplateId == 0) ? DomainManager.World.GetTaiwuVillageLandFormType() : mapBlockData.LandFormType);
			}
			LandFormTypeItem landFormData = LandFormType.Instance[landFormType];
			Location location = new Location(mapAreaId, mapBlockId);
			BuildingAreaData areaData = new BuildingAreaData(areaWidth, landFormType);
			List<short> allBlockList = ObjectPool<List<short>>.Instance.Get();
			List<short> canUseBlockList = ObjectPool<List<short>>.Instance.Get();
			allBlockList.Clear();
			canUseBlockList.Clear();
			short i = 0;
			while ((int)i < blockCount)
			{
				allBlockList.Add(i);
				bool flag2 = i >= (short)areaWidth && (int)i < blockCount - (int)areaWidth && i % (short)areaWidth != 0 && i % (short)areaWidth != (short)(areaWidth - 1);
				if (flag2)
				{
					canUseBlockList.Add(i);
				}
				i += 1;
			}
			short centerIndex = areaData.GetCenterBlockIndex();
			sbyte buildingWidth = BuildingBlock.Instance[mapBlockData.CenterBuilding].Width;
			sbyte level = (sbyte)Math.Max(random.Next((int)(maxLevel / 2), (int)(maxLevel + 1)), 1);
			short templateId = mapBlockData.CenterBuilding;
			this.AddBuilding(context, mapAreaId, mapBlockId, centerIndex, templateId, level, areaWidth);
			for (int x = 0; x < (int)buildingWidth; x++)
			{
				for (int y = 0; y < (int)buildingWidth; y++)
				{
					short index = (short)((int)centerIndex + x * (int)areaWidth + y);
					allBlockList.Remove(index);
					canUseBlockList.Remove(index);
				}
			}
			List<short> centerNeighborList = ObjectPool<List<short>>.Instance.Get();
			areaData.GetNeighborBlocks(centerIndex, buildingWidth, centerNeighborList, null, 1);
			List<short> centerNeighborsWidth2 = ObjectPool<List<short>>.Instance.Get();
			this.GetAvailableRootBlocks(centerNeighborList, centerNeighborsWidth2, mapAreaId, mapBlockId, areaWidth, 2, true);
			List<short> indirectNeighbors = ObjectPool<List<short>>.Instance.Get();
			List<short> indirectNeighborsWidth2 = ObjectPool<List<short>>.Instance.Get();
			List<short> temp = ObjectPool<List<short>>.Instance.Get();
			bool flag3 = !string.IsNullOrEmpty(mapBlockData.FixedBuildingImage);
			if (flag3)
			{
				int[] blockSubTypeList = CustomBuildingBlockConfig.Data[mapBlockData.FixedBuildingImage];
				short blockId = 0;
				while ((int)blockId < blockSubTypeList.Length)
				{
					bool flag4 = blockSubTypeList[(int)blockId] == -1;
					if (!flag4)
					{
						BuildingBlockKey tempKey = new BuildingBlockKey(mapAreaId, mapBlockId, blockId);
						BuildingBlockItem buildingBlock = BuildingBlock.Instance[blockSubTypeList[(int)blockId]];
						bool flag5 = this._buildingBlocks.ContainsKey(tempKey);
						if (!flag5)
						{
							bool flag6 = buildingBlock.Width == 1;
							if (flag6)
							{
								this.AddElement_BuildingBlocks(tempKey, new BuildingBlockData(blockId, buildingBlock.TemplateId, (sbyte)((buildingBlock.MaxLevel > 1) ? random.Next(1, (int)buildingBlock.MaxLevel) : 1), -1), context);
								allBlockList.Remove(blockId);
								canUseBlockList.Remove(blockId);
							}
							else
							{
								bool flag7 = buildingBlock.Width == 2;
								if (flag7)
								{
									this.AddBuilding(context, mapAreaId, mapBlockId, blockId, buildingBlock.TemplateId, (sbyte)((buildingBlock.MaxLevel > 1) ? random.Next(1, (int)buildingBlock.MaxLevel) : 1), areaWidth);
									allBlockList.Remove(blockId);
									allBlockList.Remove(blockId + 1);
									allBlockList.Remove(blockId + (short)areaWidth);
									allBlockList.Remove(blockId + (short)areaWidth + 1);
									canUseBlockList.Remove(blockId);
									canUseBlockList.Remove(blockId + 1);
									canUseBlockList.Remove(blockId + (short)areaWidth);
									canUseBlockList.Remove(blockId + (short)areaWidth + 1);
								}
							}
						}
					}
					blockId += 1;
				}
			}
			List<short> presetList = ObjectPool<List<short>>.Instance.Get();
			presetList.Clear();
			presetList.AddRange(mapBlockData.PresetBuildingList);
			presetList.Sort(new Comparison<short>(this.ComparePresetBuildings));
			for (int j = 0; j < presetList.Count; j++)
			{
				short buildingId = presetList[j];
				bool flag8 = buildingId == 287 && mapAreaId == 138;
				if (!flag8)
				{
					BuildingBlockItem currBuildingCfg = BuildingBlock.Instance[buildingId];
					buildingWidth = currBuildingCfg.Width;
					bool flag9 = BuildingBlockData.IsResource(currBuildingCfg.Type);
					short blockIndex;
					if (flag9)
					{
						blockIndex = canUseBlockList[random.Next(0, canUseBlockList.Count)];
					}
					else
					{
						if (!true)
						{
						}
						short randomAvailableBuildingBlock;
						if (buildingWidth != 2)
						{
							randomAvailableBuildingBlock = this.GetRandomAvailableBuildingBlock(context, canUseBlockList, centerNeighborList, indirectNeighbors, mapAreaId, mapBlockId, areaWidth, buildingWidth);
						}
						else
						{
							randomAvailableBuildingBlock = this.GetRandomAvailableBuildingBlock(context, canUseBlockList, centerNeighborsWidth2, indirectNeighborsWidth2, mapAreaId, mapBlockId, areaWidth, buildingWidth);
						}
						if (!true)
						{
						}
						blockIndex = randomAvailableBuildingBlock;
						bool flag10 = blockIndex < 0;
						if (flag10)
						{
							Logger logger = BuildingDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Failed to create building ");
							defaultInterpolatedStringHandler.AppendFormatted(currBuildingCfg.Name);
							defaultInterpolatedStringHandler.AppendLiteral(" at ");
							defaultInterpolatedStringHandler.AppendFormatted(BuildingBlock.Instance[templateId].Name);
							defaultInterpolatedStringHandler.AppendLiteral("(");
							defaultInterpolatedStringHandler.AppendFormatted<short>(templateId);
							defaultInterpolatedStringHandler.AppendLiteral("): no available space.");
							logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
							goto IL_622;
						}
					}
					temp.Clear();
					areaData.GetNeighborBlocks(blockIndex, buildingWidth, temp, null, 1);
					indirectNeighbors.AddRange(temp);
					this.GetAvailableRootBlocks(temp, indirectNeighborsWidth2, mapAreaId, mapBlockId, areaWidth, 2, false);
					level = (sbyte)Math.Clamp(random.Next((int)(maxLevel / 2), (int)(maxLevel + 1)), 1, (int)currBuildingCfg.MaxLevel);
					this.AddBuilding(context, mapAreaId, mapBlockId, blockIndex, buildingId, level, areaWidth);
					for (int x2 = 0; x2 < (int)buildingWidth; x2++)
					{
						for (int y2 = 0; y2 < (int)buildingWidth; y2++)
						{
							short index2 = (short)((int)blockIndex + x2 * (int)areaWidth + y2);
							bool flag11 = centerNeighborList.Contains(index2);
							if (flag11)
							{
								centerNeighborList.Remove(index2);
							}
							allBlockList.Remove(index2);
							canUseBlockList.Remove(index2);
						}
					}
				}
				IL_622:;
			}
			ObjectPool<List<short>>.Instance.Return(presetList);
			bool flag12 = location.AreaId == 138;
			if (flag12)
			{
				short merchantBuildingId = (short)(274 + context.Random.Next(7));
				buildingWidth = BuildingBlock.Instance[merchantBuildingId].Width;
				if (!true)
				{
				}
				short randomAvailableBuildingBlock;
				if (buildingWidth != 2)
				{
					randomAvailableBuildingBlock = this.GetRandomAvailableBuildingBlock(context, canUseBlockList, centerNeighborList, indirectNeighbors, mapAreaId, mapBlockId, areaWidth, buildingWidth);
				}
				else
				{
					randomAvailableBuildingBlock = this.GetRandomAvailableBuildingBlock(context, canUseBlockList, centerNeighborsWidth2, indirectNeighborsWidth2, mapAreaId, mapBlockId, areaWidth, buildingWidth);
				}
				if (!true)
				{
				}
				short blockIndex2 = randomAvailableBuildingBlock;
				bool flag13 = blockIndex2 < 0;
				if (flag13)
				{
					Logger logger2 = BuildingDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to create building ");
					defaultInterpolatedStringHandler.AppendFormatted(BuildingBlock.Instance[merchantBuildingId].Name);
					defaultInterpolatedStringHandler.AppendLiteral(" at ");
					defaultInterpolatedStringHandler.AppendFormatted(BuildingBlock.Instance[templateId].Name);
					defaultInterpolatedStringHandler.AppendLiteral("(");
					defaultInterpolatedStringHandler.AppendFormatted<short>(templateId);
					defaultInterpolatedStringHandler.AppendLiteral("): no available space.");
					logger2.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				temp.Clear();
				areaData.GetNeighborBlocks(blockIndex2, buildingWidth, temp, null, 1);
				indirectNeighbors.AddRange(temp);
				this.GetAvailableRootBlocks(temp, indirectNeighborsWidth2, mapAreaId, mapBlockId, areaWidth, 2, false);
				level = (sbyte)Math.Max(random.Next((int)(maxLevel / 2), (int)(maxLevel + 1)), 1);
				this.AddBuilding(context, mapAreaId, mapBlockId, blockIndex2, merchantBuildingId, level, areaWidth);
				for (int x3 = 0; x3 < (int)buildingWidth; x3++)
				{
					for (int y3 = 0; y3 < (int)buildingWidth; y3++)
					{
						short index3 = (short)((int)blockIndex2 + x3 * (int)areaWidth + y3);
						bool flag14 = centerNeighborList.Contains(index3);
						if (flag14)
						{
							centerNeighborList.Remove(index3);
						}
						allBlockList.Remove(index3);
						canUseBlockList.Remove(index3);
					}
				}
			}
			List<short> randomBuildingList = ObjectPool<List<short>>.Instance.Get();
			randomBuildingList.Clear();
			randomBuildingList.AddRange(mapBlockData.RandomBuildingList);
			randomBuildingList.Sort(new Comparison<short>(this.ComparePresetBuildings));
			short k = 0;
			while ((int)k < randomBuildingList.Count)
			{
				int isBuild = random.Next(0, 2);
				bool flag15 = isBuild == 1;
				if (flag15)
				{
					short buildingId2 = randomBuildingList[(int)k];
					buildingWidth = BuildingBlock.Instance[buildingId2].Width;
					bool flag16 = BuildingBlockData.IsResource(BuildingBlock.Instance[buildingId2].Type);
					short blockIndex3;
					if (flag16)
					{
						blockIndex3 = canUseBlockList[random.Next(0, canUseBlockList.Count)];
					}
					else
					{
						if (!true)
						{
						}
						short randomAvailableBuildingBlock;
						if (buildingWidth != 2)
						{
							randomAvailableBuildingBlock = this.GetRandomAvailableBuildingBlock(context, canUseBlockList, centerNeighborList, indirectNeighbors, mapAreaId, mapBlockId, areaWidth, buildingWidth);
						}
						else
						{
							randomAvailableBuildingBlock = this.GetRandomAvailableBuildingBlock(context, canUseBlockList, centerNeighborsWidth2, indirectNeighborsWidth2, mapAreaId, mapBlockId, areaWidth, buildingWidth);
						}
						if (!true)
						{
						}
						blockIndex3 = randomAvailableBuildingBlock;
						bool flag17 = blockIndex3 < 0;
						if (flag17)
						{
							Logger logger3 = BuildingDomain.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Failed to create building ");
							defaultInterpolatedStringHandler.AppendFormatted(BuildingBlock.Instance[buildingId2].Name);
							defaultInterpolatedStringHandler.AppendLiteral(" at ");
							defaultInterpolatedStringHandler.AppendFormatted(BuildingBlock.Instance[templateId].Name);
							defaultInterpolatedStringHandler.AppendLiteral("(");
							defaultInterpolatedStringHandler.AppendFormatted<short>(templateId);
							defaultInterpolatedStringHandler.AppendLiteral("): no available space.");
							logger3.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
							goto IL_ABB;
						}
					}
					temp.Clear();
					areaData.GetNeighborBlocks(blockIndex3, buildingWidth, temp, null, 1);
					indirectNeighbors.AddRange(temp);
					this.GetAvailableRootBlocks(temp, indirectNeighborsWidth2, mapAreaId, mapBlockId, areaWidth, 2, false);
					level = (sbyte)Math.Max(random.Next((int)(maxLevel / 2), (int)(maxLevel + 1)), 1);
					BuildingBlockKey tempKey2 = new BuildingBlockKey(mapAreaId, mapBlockId, blockIndex3);
					bool flag18 = this._buildingBlocks.ContainsKey(tempKey2);
					if (flag18)
					{
						BuildingBlockData buildingBlockData = this.GetBuildingBlockData(tempKey2);
						this.AddBuilding(context, mapAreaId, mapBlockId, blockIndex3, buildingId2, level, areaWidth);
					}
					this.AddBuilding(context, mapAreaId, mapBlockId, blockIndex3, buildingId2, level, areaWidth);
					for (int x4 = 0; x4 < (int)buildingWidth; x4++)
					{
						for (int y4 = 0; y4 < (int)buildingWidth; y4++)
						{
							short index4 = (short)((int)blockIndex3 + x4 * (int)areaWidth + y4);
							bool flag19 = centerNeighborList.Contains(index4);
							if (flag19)
							{
								centerNeighborList.Remove(index4);
							}
							allBlockList.Remove(index4);
							canUseBlockList.Remove(index4);
						}
					}
				}
				IL_ABB:
				k += 1;
				continue;
				goto IL_ABB;
			}
			ObjectPool<List<short>>.Instance.Return(randomBuildingList);
			bool flag20 = mapBlockTemplateId == 0;
			if (flag20)
			{
				sbyte centerWidth = BuildingBlock.Instance[mapBlockData.CenterBuilding].Width;
				for (int x5 = -1; x5 < (int)(centerWidth + 1); x5++)
				{
					for (int y5 = -1; y5 < (int)(centerWidth + 1); y5++)
					{
						short index5 = (short)((int)centerIndex + y5 * (int)areaWidth + x5);
						canUseBlockList.Remove(index5);
					}
				}
			}
			BuildingFormulaItem resourceInitLevelFormula = (mapBlockItem.TemplateId == 0) ? BuildingFormula.DefValue.TaiwuVillageResourceInitLevel : BuildingFormula.DefValue.NonTaiwuVillageResourceInitLevel;
			for (int l = 0; l < landFormData.NormalResource.Length; l++)
			{
				int randomCount = (int)landFormData.NormalResource[l] * (int)((float)blockCount / 1.6f) / 100;
				bool flag21 = randomCount > 0;
				if (flag21)
				{
					short buildingId3 = (short)(1 + l);
					for (int m = 1; m <= randomCount; m++)
					{
						bool flag22 = m >= 5 || random.CheckPercentProb(m * 20);
						if (flag22)
						{
							short blockIndex4 = canUseBlockList[random.Next(0, canUseBlockList.Count)];
							sbyte resLevel = (sbyte)resourceInitLevelFormula.Calculate();
							this.AddElement_BuildingBlocks(new BuildingBlockKey(mapAreaId, mapBlockId, blockIndex4), new BuildingBlockData(blockIndex4, buildingId3, resLevel, -1), context);
							allBlockList.Remove(blockIndex4);
							canUseBlockList.Remove(blockIndex4);
						}
					}
				}
			}
			for (int n = 0; n < landFormData.SpecialResource.Length; n++)
			{
				int randomCount2 = (int)landFormData.SpecialResource[n] * (int)((float)blockCount / 1.6f) / 100;
				bool flag23 = randomCount2 > 0;
				if (flag23)
				{
					short buildingId4 = (short)(11 + n);
					for (int j2 = 1; j2 <= randomCount2; j2++)
					{
						bool flag24 = j2 >= 5 || random.CheckPercentProb(j2 * 20);
						if (flag24)
						{
							short blockIndex5 = canUseBlockList[random.Next(0, canUseBlockList.Count)];
							sbyte resLevel2 = (sbyte)resourceInitLevelFormula.Calculate();
							this.AddElement_BuildingBlocks(new BuildingBlockKey(mapAreaId, mapBlockId, blockIndex5), new BuildingBlockData(blockIndex5, buildingId4, resLevel2, -1), context);
							allBlockList.Remove(blockIndex5);
							canUseBlockList.Remove(blockIndex5);
						}
					}
				}
			}
			BuildingFormulaItem uselessResourceFormula = BuildingFormula.DefValue.UselessResourceInitLevel;
			bool flag25 = mapBlockItem.TemplateId == 0;
			if (flag25)
			{
				for (int i2 = 0; i2 < 3; i2++)
				{
					short buildingId5 = (short)(21 + i2);
					for (int j3 = 1; j3 <= 10; j3++)
					{
						bool flag26 = canUseBlockList.Count < 1;
						if (flag26)
						{
							break;
						}
						short blockIndex6 = canUseBlockList[random.Next(0, canUseBlockList.Count)];
						sbyte resLevel3 = (sbyte)uselessResourceFormula.Calculate();
						this.AddElement_BuildingBlocks(new BuildingBlockKey(mapAreaId, mapBlockId, blockIndex6), new BuildingBlockData(blockIndex6, buildingId5, resLevel3, -1), context);
						allBlockList.Remove(blockIndex6);
						canUseBlockList.Remove(blockIndex6);
					}
				}
			}
			short i3 = 0;
			while ((int)i3 < allBlockList.Count)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(mapAreaId, mapBlockId, allBlockList[(int)i3]);
				this.AddElement_BuildingBlocks(blockKey, new BuildingBlockData(allBlockList[(int)i3], 0, -1, -1), context);
				i3 += 1;
			}
			this.AddElement_BuildingAreas(location, areaData, context);
			ObjectPool<List<short>>.Instance.Return(allBlockList);
			ObjectPool<List<short>>.Instance.Return(canUseBlockList);
			ObjectPool<List<short>>.Instance.Return(centerNeighborList);
			ObjectPool<List<short>>.Instance.Return(centerNeighborsWidth2);
			ObjectPool<List<short>>.Instance.Return(indirectNeighbors);
			ObjectPool<List<short>>.Instance.Return(indirectNeighborsWidth2);
			ObjectPool<List<short>>.Instance.Return(temp);
		}

		// Token: 0x06007CAD RID: 31917 RVA: 0x0049EA04 File Offset: 0x0049CC04
		private int ComparePresetBuildings(short templateIdA, short templateIdB)
		{
			BuildingBlockItem cfgA = BuildingBlock.Instance[templateIdA];
			BuildingBlockItem cfgB = BuildingBlock.Instance[templateIdB];
			return cfgA.DependBuildings.Count.CompareTo(cfgB.DependBuildings.Count);
		}

		// Token: 0x06007CAE RID: 31918 RVA: 0x0049EA4C File Offset: 0x0049CC4C
		public void AddTaiwuBuildingArea(DataContext context, Location location)
		{
			this._taiwuBuildingAreas.Add(location);
			this.SetTaiwuBuildingAreas(this._taiwuBuildingAreas, context);
			this.InitializeResidences(context, location);
			this.InitializeComfortableHouses(context, location);
			this.InitializeBuildingExtraData(context, location);
			DomainManager.Extra.InitializeResourceBlockExtraData(context, location);
		}

		// Token: 0x06007CAF RID: 31919 RVA: 0x0049EAA0 File Offset: 0x0049CCA0
		private void InitializeBuildingExtraData(DataContext context, Location location)
		{
			BuildingDomain.<>c__DisplayClass234_0 CS$<>8__locals1 = new BuildingDomain.<>c__DisplayClass234_0();
			CS$<>8__locals1.location = location;
			CS$<>8__locals1.<>4__this = this;
			IEnumerable<BuildingBlockData> blocks = this.GetBuildingBlocksAtLocation(CS$<>8__locals1.location, null);
			foreach (BuildingBlockData block in blocks)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(CS$<>8__locals1.location.AreaId, CS$<>8__locals1.location.BlockId, block.BlockIndex);
				bool flag = block.TemplateId == 44;
				if (flag)
				{
					DomainManager.Extra.ModifyBuildingExtraData(context, blockKey, new Action<BuildingBlockDataEx>(CS$<>8__locals1.<InitializeBuildingExtraData>g__HandleTaiwuVillageInitUnlockSlot|0));
				}
				else
				{
					bool flag2 = block.TemplateId >= 0 && block.ConfigData.Class == EBuildingBlockClass.BornResource;
					if (flag2)
					{
						DomainManager.Extra.ModifyBuildingExtraData(context, blockKey, new Action<BuildingBlockDataEx>(CS$<>8__locals1.<InitializeBuildingExtraData>g__HandleResourceLevel|1));
					}
					else
					{
						DomainManager.Extra.ModifyBuildingExtraData(context, blockKey, null);
					}
				}
			}
		}

		// Token: 0x06007CB0 RID: 31920 RVA: 0x0049EBAC File Offset: 0x0049CDAC
		private short GetRandomAvailableBuildingBlock(DataContext context, List<short> allValidBlocks, List<short> indexes, List<short> backupIndexes, short areaId, short blockId, sbyte areaWidth, sbyte buildingWidth)
		{
			this.RemoveUnavailableBuildingBlocks(allValidBlocks, indexes, areaId, blockId, areaWidth, buildingWidth);
			bool flag = indexes.Count == 0;
			short index;
			if (flag)
			{
				this.RemoveUnavailableBuildingBlocks(allValidBlocks, backupIndexes, areaId, blockId, areaWidth, buildingWidth);
				bool flag2 = backupIndexes.Count == 0;
				if (flag2)
				{
					return -1;
				}
				index = backupIndexes[context.Random.Next(0, backupIndexes.Count)];
			}
			else
			{
				index = indexes[context.Random.Next(0, indexes.Count)];
			}
			return index;
		}

		// Token: 0x06007CB1 RID: 31921 RVA: 0x0049EC3C File Offset: 0x0049CE3C
		public void RemoveUnavailableBuildingBlocks(List<short> validBlockList, List<short> buildingBlocks, short areaId, short blockId, sbyte areaWidth, sbyte buildingWidth)
		{
			List<short> availableIndexes = ObjectPool<List<short>>.Instance.Get();
			availableIndexes.Clear();
			foreach (short index in buildingBlocks)
			{
				bool flag = this.IsAllBlocksInValidBlockList(validBlockList, index, areaWidth, buildingWidth);
				if (flag)
				{
					availableIndexes.Add(index);
				}
			}
			buildingBlocks.Clear();
			buildingBlocks.AddRange(availableIndexes);
			ObjectPool<List<short>>.Instance.Return(availableIndexes);
		}

		// Token: 0x06007CB2 RID: 31922 RVA: 0x0049ECD0 File Offset: 0x0049CED0
		private void GetAvailableRootBlocks(List<short> buildingBlocks, List<short> availableIndexes, short areaId, short blockId, sbyte areaWidth, sbyte buildingWidth, bool clearFirst = true)
		{
			if (clearFirst)
			{
				availableIndexes.Clear();
			}
			foreach (short index in buildingBlocks)
			{
				for (int i = 0; i < (int)buildingWidth; i++)
				{
					for (int j = 0; j < (int)buildingWidth; j++)
					{
						short indexWithOffset = (short)((int)index - i * (int)areaWidth - j);
						bool flag = this.IsBuildingBlocksEmpty(areaId, blockId, indexWithOffset, areaWidth, buildingWidth) && !availableIndexes.Contains(indexWithOffset);
						if (flag)
						{
							availableIndexes.Add(indexWithOffset);
						}
					}
				}
			}
		}

		// Token: 0x06007CB3 RID: 31923 RVA: 0x0049ED94 File Offset: 0x0049CF94
		private bool IsAllBlocksInValidBlockList(List<short> allValidBlock, short rootIndex, sbyte areaWidth, sbyte buildingWidth)
		{
			int x = (int)(rootIndex % (short)areaWidth);
			int y = (int)(rootIndex / (short)areaWidth);
			int blocks = 0;
			int i = 0;
			while (i < (int)buildingWidth && x + i < (int)areaWidth)
			{
				int j = 0;
				while (j < (int)buildingWidth && y + j < (int)areaWidth)
				{
					short index = (short)((int)rootIndex + i * (int)areaWidth + j);
					bool flag = !allValidBlock.Contains(index);
					if (flag)
					{
						return false;
					}
					blocks++;
					j++;
				}
				i++;
			}
			return blocks == (int)(buildingWidth * buildingWidth);
		}

		// Token: 0x06007CB4 RID: 31924 RVA: 0x0049EE2C File Offset: 0x0049D02C
		private bool IsLandFormTypeRandom(MapBlockItem config)
		{
			return (config.SubType == EMapBlockSubType.Village || config.SubType == EMapBlockSubType.Town || config.SubType == EMapBlockSubType.WalledTown) && config.LandFormType == -1 && config.BuildingAreaWidth != -1;
		}

		// Token: 0x06007CB5 RID: 31925 RVA: 0x0049EE80 File Offset: 0x0049D080
		private void InitializeCricketCollection()
		{
			for (int i = 0; i < 15; i++)
			{
				this._collectionCrickets[i] = ItemKey.Invalid;
				this._collectionCricketJars[i] = ItemKey.Invalid;
				this._collectionCricketRegen[i] = 0;
			}
		}

		// Token: 0x06007CB6 RID: 31926 RVA: 0x0049EECC File Offset: 0x0049D0CC
		[DomainMethod]
		public void CricketCollectionAdd(DataContext context, int index, bool isCricket, ItemKey itemKey)
		{
			Tester.Assert(itemKey.Id >= 0, "");
			Tester.Assert(index >= 0 && index < 17, "");
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			bool flag = DomainManager.Taiwu.GetWarehouseAllItemKey().Contains(itemKey);
			if (flag)
			{
				DomainManager.Taiwu.RemoveItem(context, itemKey, 1, 2, false, false);
			}
			else
			{
				DomainManager.Taiwu.RemoveItem(context, itemKey, 1, 1, false, false);
			}
			if (isCricket)
			{
				cricketCollectionData[index].Cricket = itemKey;
				DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 44);
			}
			else
			{
				cricketCollectionData[index].CricketJar = itemKey;
				DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 44);
			}
			cricketCollectionData[index].CricketRegen = 0;
			DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionData, context);
		}

		// Token: 0x06007CB7 RID: 31927 RVA: 0x0049EFB8 File Offset: 0x0049D1B8
		[DomainMethod]
		public void CricketCollectionRemove(DataContext context, int index, bool isCricket)
		{
			Tester.Assert(index >= 0 && index < 17, "");
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			if (isCricket)
			{
				ItemKey itemKey = cricketCollectionData[index].Cricket;
				this.OfflineRemoveACricket(context, itemKey, cricketCollectionData, index, ItemSourceType.Inventory);
			}
			else
			{
				ItemKey itemKey2 = cricketCollectionData[index].CricketJar;
				this.OfflineRemoveAJar(context, itemKey2, cricketCollectionData, index, ItemSourceType.Inventory);
			}
			cricketCollectionData[index].CricketRegen = 0;
			DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionData, context);
		}

		// Token: 0x06007CB8 RID: 31928 RVA: 0x0049F04C File Offset: 0x0049D24C
		[DomainMethod]
		public void CricketCollectionBatchRemoveCricket(DataContext context, ItemSourceType sourceType)
		{
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			for (int i = 0; i < cricketCollectionData.Count; i++)
			{
				ItemKey itemKey = cricketCollectionData[i].Cricket;
				bool flag = !itemKey.IsValid();
				if (!flag)
				{
					this.OfflineRemoveACricket(context, itemKey, cricketCollectionData, i, sourceType);
					cricketCollectionData[i].CricketRegen = 0;
				}
			}
			DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionData, context);
		}

		// Token: 0x06007CB9 RID: 31929 RVA: 0x0049F0C4 File Offset: 0x0049D2C4
		[DomainMethod]
		public void CricketCollectionBatchRemoveJar(DataContext context, ItemSourceType sourceType)
		{
			this.CricketCollectionBatchRemoveCricket(context, sourceType);
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			for (int i = 0; i < cricketCollectionData.Count; i++)
			{
				ItemKey itemKey = cricketCollectionData[i].CricketJar;
				bool flag = !itemKey.IsValid();
				if (!flag)
				{
					this.OfflineRemoveAJar(context, itemKey, cricketCollectionData, i, sourceType);
				}
			}
			DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionData, context);
		}

		// Token: 0x06007CBA RID: 31930 RVA: 0x0049F144 File Offset: 0x0049D344
		private void OfflineRemoveAJar(DataContext context, ItemKey itemKey, List<CricketCollectionData> cricketCollectionData, int i, ItemSourceType sourceType)
		{
			DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, 44);
			bool flag = !DomainManager.Taiwu.CanTransferItemToWarehouse(context) && sourceType == ItemSourceType.Inventory;
			if (flag)
			{
				sourceType = ItemSourceType.Warehouse;
			}
			DomainManager.Taiwu.AddItem(context, itemKey, 1, (sbyte)sourceType, false);
			cricketCollectionData[i].CricketJar = ItemKey.Invalid;
		}

		// Token: 0x06007CBB RID: 31931 RVA: 0x0049F1A4 File Offset: 0x0049D3A4
		private void OfflineRemoveACricket(DataContext context, ItemKey itemKey, List<CricketCollectionData> cricketCollectionData, int i, ItemSourceType sourceType)
		{
			DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, 44);
			bool flag = !DomainManager.Taiwu.CanTransferItemToWarehouse(context) && sourceType == ItemSourceType.Inventory;
			if (flag)
			{
				sourceType = ItemSourceType.Warehouse;
			}
			DomainManager.Taiwu.AddItem(context, itemKey, 1, (sbyte)sourceType, false);
			cricketCollectionData[i].Cricket = ItemKey.Invalid;
		}

		// Token: 0x06007CBC RID: 31932 RVA: 0x0049F204 File Offset: 0x0049D404
		private bool IsCricket(KeyValuePair<ItemKey, int> pair)
		{
			short subType = ItemTemplateHelper.GetItemSubType(pair.Key.ItemType, pair.Key.TemplateId);
			return subType == 1100;
		}

		// Token: 0x06007CBD RID: 31933 RVA: 0x0049F23C File Offset: 0x0049D43C
		private bool IsCricketJar(KeyValuePair<ItemKey, int> pair)
		{
			short subType = ItemTemplateHelper.GetItemSubType(pair.Key.ItemType, pair.Key.TemplateId);
			return subType == 1201;
		}

		// Token: 0x06007CBE RID: 31934 RVA: 0x0049F274 File Offset: 0x0049D474
		[DomainMethod]
		public void CricketCollectionBatchAddCricket(DataContext context)
		{
			bool canTransfer = DomainManager.Taiwu.CanTransferItemToWarehouse(context);
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			List<BuildingDomain.ItemWithSource> combinedList = new List<BuildingDomain.ItemWithSource>();
			List<KeyValuePair<ItemKey, int>> inventoryItems = BuildingDomain.GetInventoryCrickets(canTransfer, taiwuChar);
			List<KeyValuePair<ItemKey, int>> warehouseItems = this.GetWarehouseCrickets();
			List<ValueTuple<List<KeyValuePair<ItemKey, int>>, ItemSourceType>> sources = new List<ValueTuple<List<KeyValuePair<ItemKey, int>>, ItemSourceType>>
			{
				new ValueTuple<List<KeyValuePair<ItemKey, int>>, ItemSourceType>(inventoryItems, ItemSourceType.Inventory),
				new ValueTuple<List<KeyValuePair<ItemKey, int>>, ItemSourceType>(warehouseItems, ItemSourceType.Warehouse)
			};
			for (int i = 0; i < sources.Count; i++)
			{
				for (int j = 0; j < sources[i].Item1.Count; j++)
				{
					combinedList.Add(new BuildingDomain.ItemWithSource
					{
						ItemKey = sources[i].Item1[j].Key,
						Amount = sources[i].Item1[j].Value,
						Source = i,
						IndexInSource = j
					});
				}
			}
			combinedList.Sort(new Comparison<BuildingDomain.ItemWithSource>(BuildingDomain.<CricketCollectionBatchAddCricket>g__Compare|254_0));
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			int indexInCombinedList = 0;
			for (int k = 0; k < cricketCollectionData.Count; k++)
			{
				ItemKey cricketInCollection = cricketCollectionData[k].Cricket;
				ItemKey cricketJarInCollection = cricketCollectionData[k].CricketJar;
				bool flag = !cricketJarInCollection.IsValid() || cricketInCollection.IsValid();
				if (!flag)
				{
					bool flag2 = indexInCombinedList > combinedList.Count - 1;
					if (!flag2)
					{
						BuildingDomain.ItemWithSource itemWithSource = combinedList[indexInCombinedList];
						ItemKey itemKey = sources[itemWithSource.Source].Item1[itemWithSource.IndexInSource].Key;
						DomainManager.Taiwu.RemoveItem(context, itemKey, 1, (sbyte)sources[itemWithSource.Source].Item2, false, false);
						itemWithSource.Amount--;
						combinedList[indexInCombinedList] = itemWithSource;
						cricketCollectionData[k].Cricket = itemKey;
						DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 44);
						cricketCollectionData[k].CricketRegen = 0;
						bool flag3 = itemWithSource.Amount == 0;
						if (flag3)
						{
							indexInCombinedList++;
						}
					}
				}
			}
			DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionData, context);
		}

		// Token: 0x06007CBF RID: 31935 RVA: 0x0049F4F4 File Offset: 0x0049D6F4
		private List<KeyValuePair<ItemKey, int>> GetTroughCrickets()
		{
			return DomainManager.Extra.TroughItems.Where(new Func<KeyValuePair<ItemKey, int>, bool>(this.IsCricket)).ToList<KeyValuePair<ItemKey, int>>();
		}

		// Token: 0x06007CC0 RID: 31936 RVA: 0x0049F528 File Offset: 0x0049D728
		private List<KeyValuePair<ItemKey, int>> GetWarehouseCrickets()
		{
			return DomainManager.Taiwu.WarehouseItems.Where(new Func<KeyValuePair<ItemKey, int>, bool>(this.IsCricket)).ToList<KeyValuePair<ItemKey, int>>();
		}

		// Token: 0x06007CC1 RID: 31937 RVA: 0x0049F55C File Offset: 0x0049D75C
		private static List<KeyValuePair<ItemKey, int>> GetInventoryCrickets(bool canTransfer, GameData.Domains.Character.Character taiwuChar)
		{
			List<KeyValuePair<ItemKey, int>> inventoryItems = new List<KeyValuePair<ItemKey, int>>();
			if (canTransfer)
			{
				List<ItemDisplayData> cricketDisplayDataList = DomainManager.Character.GetInventoryItems(taiwuChar.GetId(), 1100);
				foreach (ItemDisplayData itemDisplayData in cricketDisplayDataList)
				{
					inventoryItems.Add(new KeyValuePair<ItemKey, int>(itemDisplayData.Key, itemDisplayData.Amount));
				}
			}
			return inventoryItems;
		}

		// Token: 0x06007CC2 RID: 31938 RVA: 0x0049F5F0 File Offset: 0x0049D7F0
		[DomainMethod]
		public void CricketCollectionBatchAddCricketJar(DataContext context)
		{
			bool canTransfer = DomainManager.Taiwu.CanTransferItemToWarehouse(context);
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			List<BuildingDomain.ItemWithSource> combinedList = new List<BuildingDomain.ItemWithSource>();
			List<KeyValuePair<ItemKey, int>> inventoryItems = BuildingDomain.GetInventoryCricketJars(canTransfer, taiwuChar);
			List<KeyValuePair<ItemKey, int>> warehouseItems = this.GetWarehouseCricketJars();
			List<ValueTuple<List<KeyValuePair<ItemKey, int>>, ItemSourceType>> sources = new List<ValueTuple<List<KeyValuePair<ItemKey, int>>, ItemSourceType>>
			{
				new ValueTuple<List<KeyValuePair<ItemKey, int>>, ItemSourceType>(inventoryItems, ItemSourceType.Inventory),
				new ValueTuple<List<KeyValuePair<ItemKey, int>>, ItemSourceType>(warehouseItems, ItemSourceType.Warehouse)
			};
			for (int i = 0; i < sources.Count; i++)
			{
				for (int j = 0; j < sources[i].Item1.Count; j++)
				{
					combinedList.Add(new BuildingDomain.ItemWithSource
					{
						ItemKey = sources[i].Item1[j].Key,
						Amount = sources[i].Item1[j].Value,
						Source = i,
						IndexInSource = j
					});
				}
			}
			combinedList.Sort(new Comparison<BuildingDomain.ItemWithSource>(BuildingDomain.<CricketCollectionBatchAddCricketJar>g__Compare|258_0));
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			int indexInCombinedList = 0;
			for (int k = 0; k < cricketCollectionData.Count; k++)
			{
				ItemKey cricketJarInCollection = cricketCollectionData[k].CricketJar;
				bool flag = cricketJarInCollection.IsValid();
				if (!flag)
				{
					bool flag2 = indexInCombinedList > combinedList.Count - 1;
					if (!flag2)
					{
						BuildingDomain.ItemWithSource itemWithSource = combinedList[indexInCombinedList];
						ItemKey itemKey = sources[itemWithSource.Source].Item1[itemWithSource.IndexInSource].Key;
						DomainManager.Taiwu.RemoveItem(context, itemKey, 1, (sbyte)sources[itemWithSource.Source].Item2, false, false);
						itemWithSource.Amount--;
						combinedList[indexInCombinedList] = itemWithSource;
						cricketCollectionData[k].CricketJar = itemKey;
						DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 44);
						cricketCollectionData[k].CricketRegen = 0;
						bool flag3 = itemWithSource.Amount == 0;
						if (flag3)
						{
							indexInCombinedList++;
						}
					}
				}
			}
			DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionData, context);
		}

		// Token: 0x06007CC3 RID: 31939 RVA: 0x0049F854 File Offset: 0x0049DA54
		private List<KeyValuePair<ItemKey, int>> GetTroughCricketJars()
		{
			return DomainManager.Extra.TroughItems.Where(new Func<KeyValuePair<ItemKey, int>, bool>(this.IsCricketJar)).ToList<KeyValuePair<ItemKey, int>>();
		}

		// Token: 0x06007CC4 RID: 31940 RVA: 0x0049F888 File Offset: 0x0049DA88
		private List<KeyValuePair<ItemKey, int>> GetWarehouseCricketJars()
		{
			return DomainManager.Taiwu.WarehouseItems.Where(new Func<KeyValuePair<ItemKey, int>, bool>(this.IsCricketJar)).ToList<KeyValuePair<ItemKey, int>>();
		}

		// Token: 0x06007CC5 RID: 31941 RVA: 0x0049F8BC File Offset: 0x0049DABC
		private static List<KeyValuePair<ItemKey, int>> GetInventoryCricketJars(bool canTransfer, GameData.Domains.Character.Character taiwuChar)
		{
			List<KeyValuePair<ItemKey, int>> inventoryItems = new List<KeyValuePair<ItemKey, int>>();
			if (canTransfer)
			{
				List<ItemDisplayData> cricketDisplayDataList = DomainManager.Character.GetInventoryItems(taiwuChar.GetId(), 1201);
				foreach (ItemDisplayData itemDisplayData in cricketDisplayDataList)
				{
					inventoryItems.Add(new KeyValuePair<ItemKey, int>(itemDisplayData.Key, itemDisplayData.Amount));
				}
			}
			return inventoryItems;
		}

		// Token: 0x06007CC6 RID: 31942 RVA: 0x0049F950 File Offset: 0x0049DB50
		[DomainMethod]
		public List<ItemDisplayData> GetCricketOrJarFromSourceStorage(DataContext context, short itemSubType, ItemSourceType sourceType)
		{
			bool flag = itemSubType != 1100 && itemSubType != 1201;
			if (flag)
			{
				throw new ArgumentException("itemSubType must be Cricket or CricketJar");
			}
			bool flag2 = sourceType != ItemSourceType.Inventory && sourceType != ItemSourceType.Warehouse && sourceType != ItemSourceType.Trough;
			if (flag2)
			{
				throw new ArgumentException("sourceType must be Inventory or Warehouse or Trough");
			}
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			List<ItemDisplayData> result = null;
			bool canTransfer = DomainManager.Taiwu.CanTransferItemToWarehouse(context);
			switch (sourceType)
			{
			case ItemSourceType.Inventory:
			{
				bool flag3 = !canTransfer;
				if (flag3)
				{
					throw new InvalidOperationException("Cannot transfer item");
				}
				result = DomainManager.Character.GetInventoryItems(taiwuChar.GetId(), itemSubType);
				break;
			}
			case ItemSourceType.Warehouse:
				result = DomainManager.Taiwu.GetWarehouseItemsBySubType(context, itemSubType);
				break;
			case ItemSourceType.Trough:
			{
				Dictionary<ItemKey, int> items = (from pair in DomainManager.Extra.TroughItems
				where ItemTemplateHelper.GetItemSubType(pair.Key.ItemType, pair.Key.TemplateId) == itemSubType
				select pair).ToDictionary((KeyValuePair<ItemKey, int> pair) => pair.Key, (KeyValuePair<ItemKey, int> pair) => pair.Value);
				result = CharacterDomain.GetItemDisplayData(taiwuChar.GetId(), items, ItemSourceType.Trough);
				break;
			}
			}
			return result;
		}

		// Token: 0x06007CC7 RID: 31943 RVA: 0x0049FAC0 File Offset: 0x0049DCC0
		[DomainMethod]
		public void SmartOperateCricketOrJarCollection(DataContext context, int collectionIndex, short itemSubType, ItemSourceType sourceType, ItemKey itemKey)
		{
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			bool flag = collectionIndex < 0 || collectionIndex >= cricketCollectionData.Count;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("collectionIndex", collectionIndex, null);
			}
			ItemKey cricket = cricketCollectionData[collectionIndex].Cricket;
			ItemKey jar = cricketCollectionData[collectionIndex].CricketJar;
			bool flag2 = itemSubType == 1100;
			if (flag2)
			{
				bool flag3 = !jar.IsValid();
				if (flag3)
				{
					throw new InvalidOperationException("No jar in the collection");
				}
				bool flag4 = cricket.IsValid();
				if (flag4)
				{
					this.OfflineRemoveACricket(context, cricket, cricketCollectionData, collectionIndex, sourceType);
				}
				bool flag5 = itemKey.IsValid();
				if (flag5)
				{
					cricketCollectionData[collectionIndex].Cricket = itemKey;
					DomainManager.Taiwu.RemoveItem(context, itemKey, 1, (sbyte)sourceType, false, false);
					DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 44);
					cricketCollectionData[collectionIndex].CricketRegen = 0;
				}
			}
			else
			{
				bool flag6 = itemSubType == 1201;
				if (!flag6)
				{
					throw new ArgumentException("itemSubType must be Cricket or CricketJar");
				}
				bool flag7 = jar.IsValid();
				if (flag7)
				{
					this.OfflineRemoveAJar(context, jar, cricketCollectionData, collectionIndex, sourceType);
				}
				bool flag8 = itemKey.IsValid();
				if (flag8)
				{
					DomainManager.Taiwu.RemoveItem(context, itemKey, 1, (sbyte)sourceType, false, false);
					DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 44);
					cricketCollectionData[collectionIndex].CricketJar = itemKey;
				}
				else
				{
					bool flag9 = cricket.IsValid();
					if (flag9)
					{
						this.OfflineRemoveACricket(context, cricket, cricketCollectionData, collectionIndex, sourceType);
					}
				}
			}
			DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionData, context);
		}

		// Token: 0x06007CC8 RID: 31944 RVA: 0x0049FC68 File Offset: 0x0049DE68
		[DomainMethod]
		public CricketCollectionBatchButtonStateDisplayData GetBatchButtonEnableState(DataContext context)
		{
			bool hasCricket = false;
			bool hasEmptyJar = false;
			bool hasJar = false;
			bool hasEmptySlot = false;
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			foreach (CricketCollectionData slot in cricketCollectionData)
			{
				bool flag = slot.CricketJar.IsValid();
				if (flag)
				{
					hasJar = true;
					bool flag2 = slot.Cricket.IsValid();
					if (flag2)
					{
						hasCricket = true;
					}
					else
					{
						hasEmptyJar = true;
					}
				}
				else
				{
					hasEmptySlot = true;
				}
			}
			bool canTransfer = DomainManager.Taiwu.CanTransferItemToWarehouse(context);
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool hasSourceCricket = BuildingDomain.GetInventoryCrickets(canTransfer, taiwuChar).Count > 0 || this.GetWarehouseCrickets().Count > 0;
			bool hasSourceCricketJar = BuildingDomain.GetInventoryCricketJars(canTransfer, taiwuChar).Count > 0 || this.GetWarehouseCricketJars().Count > 0;
			return new CricketCollectionBatchButtonStateDisplayData
			{
				HasCricketInCollection = hasCricket,
				HasJarInCollection = hasJar,
				HasCricketInSources = hasSourceCricket,
				HasEmptyJarInCollection = hasEmptyJar,
				HasJarInSources = hasSourceCricketJar,
				HasEmptyPositionInCollection = hasEmptySlot
			};
		}

		// Token: 0x06007CC9 RID: 31945 RVA: 0x0049FD9C File Offset: 0x0049DF9C
		[DomainMethod]
		public ItemDisplayData[] GetCollectionCrickets(DataContext context)
		{
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			ItemDisplayData[] crickets = new ItemDisplayData[cricketCollectionData.Count];
			for (int i = 0; i < crickets.Length; i++)
			{
				bool flag = !cricketCollectionData[i].Cricket.Equals(ItemKey.Invalid);
				if (flag)
				{
					crickets[i] = DomainManager.Item.GetItemDisplayData(cricketCollectionData[i].Cricket, -1);
				}
				else
				{
					crickets[i] = new ItemDisplayData();
				}
			}
			return crickets;
		}

		// Token: 0x06007CCA RID: 31946 RVA: 0x0049FE20 File Offset: 0x0049E020
		[DomainMethod]
		public ItemDisplayData[] GetCollectionJars(DataContext context)
		{
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			ItemDisplayData[] jars = new ItemDisplayData[cricketCollectionData.Count];
			for (int i = 0; i < jars.Length; i++)
			{
				bool flag = !cricketCollectionData[i].CricketJar.Equals(ItemKey.Invalid);
				if (flag)
				{
					jars[i] = DomainManager.Item.GetItemDisplayData(cricketCollectionData[i].CricketJar, -1);
				}
				else
				{
					jars[i] = new ItemDisplayData();
				}
			}
			return jars;
		}

		// Token: 0x06007CCB RID: 31947 RVA: 0x0049FEA4 File Offset: 0x0049E0A4
		[DomainMethod]
		public int[] GetCollectionCricketRegen(DataContext context)
		{
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			int[] result = new int[cricketCollectionData.Count];
			for (int i = 0; i < cricketCollectionData.Count; i++)
			{
				result[i] = cricketCollectionData[i].CricketRegen;
			}
			return result;
		}

		// Token: 0x06007CCC RID: 31948 RVA: 0x0049FEF8 File Offset: 0x0049E0F8
		[DomainMethod]
		public int GetAuthorityGain(DataContext context)
		{
			return BuildingDomain.GetCricketAuthorityGain();
		}

		// Token: 0x06007CCD RID: 31949 RVA: 0x0049FF10 File Offset: 0x0049E110
		public static int GetCricketAuthorityGain()
		{
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			int authorityGain = 0;
			for (int i = 0; i < cricketCollectionData.Count; i++)
			{
				bool flag = cricketCollectionData[i].Cricket.Equals(ItemKey.Invalid);
				if (!flag)
				{
					GameData.Domains.Item.Cricket cricketData;
					bool flag2 = !DomainManager.Item.TryGetElement_Crickets(cricketCollectionData[i].Cricket.Id, out cricketData);
					if (flag2)
					{
						Logger logger = BuildingDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(96, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Cannot get cricket with id ");
						defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(cricketCollectionData[i].Cricket);
						defaultInterpolatedStringHandler.AppendLiteral(" at the position ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(i);
						defaultInterpolatedStringHandler.AppendLiteral(" of cricket collection while getting authority gain.");
						logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					else
					{
						int level = (int)((cricketData.GetColorData().Level >= cricketData.GetPartsData().Level) ? cricketData.GetColorData().Level : cricketData.GetPartsData().Level);
						int winless = (int)cricketData.GetWinsCount();
						int defeat = (int)cricketData.GetLossesCount();
						authorityGain += Math.Max(winless - defeat, 0) * level * ((defeat <= 0) ? 100 : 50) / 50;
					}
				}
			}
			return authorityGain;
		}

		// Token: 0x06007CCE RID: 31950 RVA: 0x004A0060 File Offset: 0x0049E260
		public void ApplyCricketAuthorityGain(DataContext context)
		{
			bool flag = DomainManager.World.GetCurrMonthInYear() != (sbyte)GlobalConfig.Instance.CricketActiveStartMonth;
			if (!flag)
			{
				int authorityGain = this.GetAuthorityGain(context);
				bool flag2 = authorityGain <= 0;
				if (!flag2)
				{
					GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
					taiwu.ChangeResource(context, 7, authorityGain);
				}
			}
		}

		// Token: 0x06007CCF RID: 31951 RVA: 0x004A00B8 File Offset: 0x0049E2B8
		public void UpdateCricketRegenProgress(DataContext context)
		{
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			for (int i = 0; i < 17; i++)
			{
				bool flag = cricketCollectionData[i].Cricket.Equals(ItemKey.Invalid);
				if (!flag)
				{
					GameData.Domains.Item.Cricket cricket = DomainManager.Item.GetElement_Crickets(cricketCollectionData[i].Cricket.Id);
					short[] injuries = cricket.GetInjuries();
					bool hasInjury = injuries.Exist((short value) => value > 0);
					bool flag2 = cricket.GetCurrDurability() == 0 || (cricket.GetCurrDurability() >= cricket.GetMaxDurability() && !hasInjury);
					if (!flag2)
					{
						MiscItem jarConfig = Config.Misc.Instance[cricketCollectionData[i].CricketJar.TemplateId];
						cricketCollectionData[i].CricketRegen++;
						bool flag3 = cricketCollectionData[i].CricketRegen >= GameData.Domains.Building.SharedMethods.CalcCricketRegenTime(jarConfig.Grade);
						if (flag3)
						{
							cricketCollectionData[i].CricketRegen = 0;
							cricket.SetCurrDurability(Math.Min(cricket.GetMaxDurability(), cricket.GetCurrDurability() + 1), context);
							bool flag4 = hasInjury && context.Random.CheckPercentProb((int)jarConfig.CricketHealInjuryOdds);
							if (flag4)
							{
								List<int> typeRandomPool = ObjectPool<List<int>>.Instance.Get();
								typeRandomPool.Clear();
								for (int injuryIndex = 0; injuryIndex < injuries.Length; injuryIndex++)
								{
									bool flag5 = injuries[injuryIndex] > 0;
									if (flag5)
									{
										typeRandomPool.Add(injuryIndex);
									}
								}
								int healIndex = typeRandomPool[context.Random.Next(typeRandomPool.Count)];
								injuries[healIndex] = (short)Math.Max((int)(injuries[healIndex] - ((healIndex < 2) ? 5 : 1)), 0);
								cricket.SetInjuries(injuries, context);
								ObjectPool<List<int>>.Instance.Return(typeRandomPool);
							}
						}
					}
				}
			}
			DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionData, context);
		}

		// Token: 0x06007CD0 RID: 31952 RVA: 0x004A02C8 File Offset: 0x0049E4C8
		public override void PackCrossArchiveGameData(CrossArchiveGameData crossArchiveGameData)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
				bool flag = config == null || config.TemplateId < 0;
				if (!flag)
				{
					bool isPawnshop = config.TemplateId == 222;
					DataContext context = DataContextManager.GetCurrentThreadDataContext();
					BuildingEarningsData data;
					bool flag2;
					if (this.TryGetElement_CollectBuildingEarningsData(blockKey, out data))
					{
						List<ItemKey> fixBookInfoList = data.FixBookInfoList;
						flag2 = (fixBookInfoList != null && fixBookInfoList.Count > 0);
					}
					else
					{
						flag2 = false;
					}
					bool flag3 = flag2;
					if (flag3)
					{
						if (crossArchiveGameData.WarehouseItems == null)
						{
							crossArchiveGameData.WarehouseItems = new Dictionary<ItemKey, int>();
						}
						foreach (ItemKey itemKey in data.FixBookInfoList)
						{
							bool flag4 = itemKey.IsValid();
							if (flag4)
							{
								crossArchiveGameData.PackWarehouseItem(itemKey, 1);
							}
						}
						blockData.OfflineResetShopProgress();
						data.FixBookInfoList.Clear();
					}
					this.ClearBuildingBlockEarningsData(context, blockKey, isPawnshop);
					bool flag5 = blockData.OperationType != -1;
					if (flag5)
					{
						this.SetStopOperation(context, blockKey, true);
					}
				}
			}
			List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
			crossArchiveGameData.CricketCollectionDatas = cricketCollectionData;
			foreach (CricketCollectionData cricketData in cricketCollectionData)
			{
				bool flag6 = cricketData.Cricket.IsValid();
				if (flag6)
				{
					DomainManager.Item.PackCrossArchiveItem(crossArchiveGameData, cricketData.Cricket);
				}
				bool flag7 = cricketData.CricketJar.IsValid();
				if (flag7)
				{
					DomainManager.Item.PackCrossArchiveItem(crossArchiveGameData, cricketData.CricketJar);
				}
			}
			crossArchiveGameData.TaiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			crossArchiveGameData.TaiwuVillageAreaData = this.GetBuildingAreaData(crossArchiveGameData.TaiwuVillageLocation);
			crossArchiveGameData.TaiwuVillageBlocks = this.GetBuildingBlockList(crossArchiveGameData.TaiwuVillageLocation);
			crossArchiveGameData.TaiwuVillageBlocksEx = DomainManager.Extra.GetBuildingExtraDataListForCrossArchive();
			crossArchiveGameData.Chicken = new Dictionary<int, Chicken>();
			bool flag8 = this._chicken != null;
			if (flag8)
			{
				foreach (KeyValuePair<int, Chicken> pair in this._chicken)
				{
					crossArchiveGameData.Chicken[pair.Key] = pair.Value;
				}
			}
			crossArchiveGameData.XiangshuIdInKungfuPracticeRoom = new List<sbyte>();
			crossArchiveGameData.XiangshuIdInKungfuPracticeRoom.AddRange(DomainManager.Extra.GetXiangshuIdInKungfuPracticeRoom());
			crossArchiveGameData.SamsaraPlatformAddCombatSkillQualifications = this._samsaraPlatformAddCombatSkillQualifications;
			crossArchiveGameData.SamsaraPlatformAddLifeSkillQualifications = this._samsaraPlatformAddLifeSkillQualifications;
			crossArchiveGameData.SamsaraPlatformAddMainAttributes = this._samsaraPlatformAddMainAttributes;
		}

		// Token: 0x06007CD1 RID: 31953 RVA: 0x004A0600 File Offset: 0x0049E800
		public override void UnpackCrossArchiveGameData(DataContext context, CrossArchiveGameData crossArchiveGameData)
		{
			this.SetSamsaraPlatformAddCombatSkillQualifications(ref crossArchiveGameData.SamsaraPlatformAddCombatSkillQualifications, context);
			this.SetSamsaraPlatformAddLifeSkillQualifications(ref crossArchiveGameData.SamsaraPlatformAddLifeSkillQualifications, context);
			this.SetSamsaraPlatformAddMainAttributes(crossArchiveGameData.SamsaraPlatformAddMainAttributes, context);
		}

		// Token: 0x06007CD2 RID: 31954 RVA: 0x004A0630 File Offset: 0x0049E830
		public void UnpackCrossArchiveGameData_Building(DataContext context, CrossArchiveGameData crossArchiveGameData)
		{
			this.SetAllVillagerHomeless(context);
			Location location = crossArchiveGameData.TaiwuVillageLocation;
			short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			bool flag = location != DomainManager.Taiwu.GetTaiwuVillageLocation();
			if (!flag)
			{
				List<BuildingBlockKey> toResetBlocks = new List<BuildingBlockKey>();
				this.SetElement_BuildingAreas(location, crossArchiveGameData.TaiwuVillageAreaData, context);
				Dictionary<BuildingBlockKey, BuildingBlockDataEx> dictionary;
				if (crossArchiveGameData.TaiwuVillageBlocksEx == null)
				{
					dictionary = new Dictionary<BuildingBlockKey, BuildingBlockDataEx>();
				}
				else
				{
					dictionary = crossArchiveGameData.TaiwuVillageBlocksEx.ToDictionary((BuildingBlockDataEx data) => data.Key);
				}
				Dictionary<BuildingBlockKey, BuildingBlockDataEx> extraDataDict = dictionary;
				foreach (BuildingBlockData blockData in crossArchiveGameData.TaiwuVillageBlocks)
				{
					BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, blockData.BlockIndex);
					this.SetElement_BuildingBlocks(blockKey, blockData, context);
					BuildingBlockDataEx extraData;
					bool flag2 = extraDataDict.TryGetValue(blockKey, out extraData);
					if (flag2)
					{
						DomainManager.Extra.ModifyBuildingExtraData(context, blockKey, extraData);
						bool flag3 = blockData.TemplateId == 44;
						if (flag3)
						{
							DomainManager.Extra.TriggerTaiwuVillageVowMonthlyEventFromCrossArchive(context, extraData);
						}
					}
					bool flag4 = blockData.TemplateId == 46;
					if (flag4)
					{
						this.QuickFillResidence(context, blockKey);
					}
					else
					{
						bool flag5 = blockData.TemplateId == 47;
						if (flag5)
						{
							this.QuickFillComfortableHouse(context, blockKey);
						}
					}
				}
				DomainManager.Extra.InitFeasts(context);
				DomainManager.Extra.InitializeResourceBlockExtraData(context, DomainManager.Taiwu.GetTaiwuVillageLocation());
				foreach (BuildingBlockKey blockKey2 in toResetBlocks)
				{
					this.ResetAllChildrenBlocks(context, blockKey2, 0, 1);
				}
				crossArchiveGameData.TaiwuVillageBlocks = null;
				crossArchiveGameData.TaiwuVillageBlocksEx = null;
				List<CricketCollectionData> cricketCollectionData = DomainManager.Extra.GetCricketCollectionDataList();
				bool flag6 = crossArchiveGameData.CricketCollectionDatas != null && crossArchiveGameData.CricketCollectionDatas.Count != 0;
				if (flag6)
				{
					for (int index = 0; index < crossArchiveGameData.CricketCollectionDatas.Count; index++)
					{
						CricketCollectionData cricketData = crossArchiveGameData.CricketCollectionDatas[index];
						bool flag7 = cricketData.Cricket.IsValid();
						if (flag7)
						{
							cricketCollectionData[index].Cricket = DomainManager.Item.UnpackCrossArchiveItem(context, crossArchiveGameData, cricketData.Cricket);
							DomainManager.Item.SetOwner(cricketCollectionData[index].Cricket, ItemOwnerType.Building, 44);
						}
						bool flag8 = cricketData.CricketJar.IsValid();
						if (flag8)
						{
							cricketCollectionData[index].CricketJar = DomainManager.Item.UnpackCrossArchiveItem(context, crossArchiveGameData, cricketData.CricketJar);
							DomainManager.Item.SetOwner(cricketCollectionData[index].CricketJar, ItemOwnerType.Building, 44);
						}
						cricketCollectionData[index].CricketRegen = cricketData.CricketRegen;
					}
					crossArchiveGameData.CricketCollectionDatas = null;
					DomainManager.Extra.SetCricketCollectionDataList(cricketCollectionData, context);
				}
				this.InitBuildingEffect();
				DomainManager.World.SetWorldFunctionsStatus(context, 11);
				this.ClearChicken(context);
				bool flag9 = crossArchiveGameData.Chicken != null;
				if (flag9)
				{
					int id = 0;
					foreach (KeyValuePair<int, Chicken> pair in crossArchiveGameData.Chicken)
					{
						bool flag10 = pair.Value.TemplateId != 63;
						if (flag10)
						{
							Chicken chicken = pair.Value;
							chicken.Id = id;
							this.AddElement_Chicken(id, chicken, context);
							id++;
						}
					}
				}
				this.InitializeNextChickenId();
				crossArchiveGameData.Chicken = null;
				bool flag11 = crossArchiveGameData.XiangshuIdInKungfuPracticeRoom != null;
				if (flag11)
				{
					DomainManager.Extra.SetXiangshuIdInKungfuPracticeRoom(crossArchiveGameData.XiangshuIdInKungfuPracticeRoom, context);
					crossArchiveGameData.XiangshuIdInKungfuPracticeRoom = null;
				}
			}
		}

		// Token: 0x06007CD3 RID: 31955 RVA: 0x004A0A48 File Offset: 0x0049EC48
		[DomainMethod]
		public ValueTuple<short, BuildingBlockData> GmCmd_BuildImmediately(DataContext context, short buildingTemplateId, BuildingBlockKey blockKey, sbyte level)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			this.BuildingRemoveVillagerWork(context, blockKey);
			this.BuildingRemoveResident(context, blockData, blockKey);
			level = Math.Clamp(level, 1, BuildingBlock.Instance[buildingTemplateId].MaxLevel);
			this.ResetAllChildrenBlocks(context, blockKey, buildingTemplateId, level);
			DomainManager.Extra.ModifyBuildingExtraData(context, blockKey, delegate(BuildingBlockDataEx dataEx)
			{
				for (int i = 0; i < (int)level; i++)
				{
					dataEx.UnlockLevelSlot(i);
				}
			});
			this.UpdateTaiwuVillageBuildingEffect();
			return new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, blockData);
		}

		// Token: 0x06007CD4 RID: 31956 RVA: 0x004A0AE4 File Offset: 0x0049ECE4
		[DomainMethod]
		public ValueTuple<short, BuildingBlockData> GmCmd_RemoveBuildingImmediately(DataContext context, BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			this.BuildingRemoveVillagerWork(context, blockKey);
			this.BuildingRemoveResident(context, blockData, blockKey);
			this.ResetAllChildrenBlocks(context, blockKey, 0, -1);
			this.UpdateTaiwuVillageBuildingEffect();
			DomainManager.Extra.ResetBuildingExtraData(context, blockKey);
			this.OnBuildingRemoved(context, blockKey);
			return new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, this.GetElement_BuildingBlocks(blockKey));
		}

		// Token: 0x06007CD5 RID: 31957 RVA: 0x004A0B4C File Offset: 0x0049ED4C
		[DomainMethod]
		public void GmCmd_AddLegacyBuilding(DataContext context, short buildingTemplateId)
		{
			List<short> list = DomainManager.Extra.GetLegaciesBuildingTemplateIdList();
			list.Add(buildingTemplateId);
			DomainManager.Extra.SetLegaciesBuildingTemplateIdList(list, context);
		}

		// Token: 0x06007CD6 RID: 31958 RVA: 0x004A0B7C File Offset: 0x0049ED7C
		[DomainMethod]
		public List<Chicken> GmCmd_GetChickenData()
		{
			List<Chicken> chicken = new List<Chicken>();
			chicken.AddRange(this._chicken.Values);
			return chicken;
		}

		// Token: 0x06007CD7 RID: 31959 RVA: 0x004A0BA8 File Offset: 0x0049EDA8
		[DomainMethod]
		public bool GmCmd_BeatMinionPerform(DataContext context, sbyte grade, int repeat)
		{
			CombatResultDisplayData combatResultData = new CombatResultDisplayData
			{
				CombatStatus = CombatStatusType.EnemyFail
			};
			short createdTemplateId = (short)(298 + (int)grade);
			int[] enemyTeam = new int[]
			{
				-1
			};
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			List<ItemDisplayData> lootItemList = new List<ItemDisplayData>();
			for (int i = 0; i < repeat; i++)
			{
				GameData.Domains.Character.Character createRandomEnemy = DomainManager.Character.CreateRandomEnemy(context, createdTemplateId, null);
				int createRandomEnemyId = createRandomEnemy.GetId();
				DomainManager.Character.CompleteCreatingCharacter(createRandomEnemyId);
				DomainManager.Adventure.AddTemporaryEnemyId(createRandomEnemyId);
				enemyTeam[0] = createRandomEnemyId;
				CombatConfigItem combatConfig = CombatConfig.Instance[1];
				CombatDomain.ResultCalcExp(combatConfig, false, taiwuChar, enemyTeam, combatResultData);
				CombatDomain.ResultCalcResource(combatConfig, false, taiwuChar, enemyTeam, combatResultData);
				CombatDomain.ResultCalcAreaSpiritualDebt(true, taiwuChar, enemyTeam, combatResultData);
				CombatDomain.ResultCalcLootItem(context.Random, 50, combatConfig.CombatType, combatResultData.CombatStatus, false, combatConfig, taiwuChar, enemyTeam, Array.Empty<int>(), combatResultData);
				GameData.Domains.Character.Character enemyChar = createRandomEnemy;
				ItemKey[] enemyEquips = enemyChar.GetEquipment();
				foreach (ItemDisplayData itemKey in combatResultData.ItemList)
				{
					ItemKey key = itemKey.Key;
					sbyte slotIndex = (sbyte)enemyEquips.IndexOf(key);
					bool flag = slotIndex >= 0;
					if (flag)
					{
						enemyChar.ChangeEquipment(context, slotIndex, -1, ItemKey.Invalid);
					}
					enemyChar.RemoveInventoryItem(context, key, 1, false, false);
					lootItemList.Add(itemKey);
				}
				combatResultData.ItemList.Clear();
			}
			ExtraDomain.MergeKeyForItemDisplayDataList(lootItemList);
			taiwuChar.AddInventoryItemList(context, (from d in lootItemList
			select d.Key).ToList<ItemKey>());
			GameDataBridge.AddDisplayEvent<List<ItemDisplayData>, bool, bool, bool>(DisplayEventType.OpenGetItem_Item, lootItemList, false, false, true);
			return true;
		}

		// Token: 0x06007CD8 RID: 31960 RVA: 0x004A0D90 File Offset: 0x0049EF90
		[DomainMethod]
		public bool GmCmd_BuildingCollectPerform(DataContext context, int totalAttainment, short buildingTemplateId, int repeat)
		{
			BuildingBlockItem buildingConfig = BuildingBlock.Instance.GetItem(buildingTemplateId);
			bool flag = buildingConfig == null || !buildingConfig.DependBuildings.CheckIndex(0);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				BuildingBlockData dependency = new BuildingBlockData
				{
					TemplateId = buildingConfig.DependBuildings[0]
				};
				BuildingBlockItem dependencyConfig = BuildingBlock.Instance.GetItem(dependency.TemplateId);
				int totalCount = 0;
				sbyte resourceType = -1;
				sbyte i = 0;
				while ((int)i < dependencyConfig.CollectResourcePercent.Length)
				{
					bool flag2 = dependencyConfig.CollectResourcePercent[(int)i] <= 0;
					if (!flag2)
					{
						resourceType = i;
						break;
					}
					i += 1;
				}
				List<ItemDisplayData> itemDisplayDataList = new List<ItemDisplayData>();
				for (int j = 0; j < repeat; j++)
				{
					BuildingProduceDependencyData dependencyData = BuildingProduceDependencyData.Invalid;
					dependencyData.Level = buildingConfig.MaxLevel;
					dependencyData.TemplateId = dependency.TemplateId;
					dependencyData.BlockBaseYieldFactor = this.BuildingBaseYield(dependency).Get((int)Math.Min(resourceType, 5));
					dependencyData.ResourceYieldLevelFactor = (int)GameData.Domains.Building.SharedMethods.GetBuildingLevelEffect(dependency.TemplateId, (int)dependencyConfig.MaxLevel);
					dependencyData.ProductivityFactor = GlobalConfig.Instance.CollectResourceBuildingProductivityDistanceOne[0];
					dependencyData.TotalAttainmentFactor = totalAttainment;
					dependencyData.GainResourcePercentFactor = DomainManager.World.GetGainResourcePercent(1);
					dependencyData.ResourceSingleOutputValuation = dependencyData.ResourceBuildingOutput;
					totalCount += dependencyData.ResourceSingleOutputValuation;
					bool flag3 = buildingConfig.IsShop && buildingConfig.SuccesEvent.Count > 0;
					if (flag3)
					{
						ShopEventItem successShopEventConfig = ShopEvent.Instance[buildingConfig.SuccesEvent[0]];
						bool flag4 = successShopEventConfig.ItemList.Count > 0;
						if (flag4)
						{
							List<TemplateKey> itemProbList = ObjectPool<List<TemplateKey>>.Instance.Get();
							itemProbList.Clear();
							int from = 0;
							int end = successShopEventConfig.ItemList.Count;
							bool flag5 = successShopEventConfig.ResourceList.Count > 1;
							if (flag5)
							{
								end = successShopEventConfig.ItemList.Count / 2;
							}
							for (int k = from; k < end; k++)
							{
								int prob = successShopEventConfig.ItemList[k].Amount + totalAttainment / 30;
								bool flag6 = context.Random.CheckPercentProb(prob);
								if (flag6)
								{
									PresetInventoryItem item = successShopEventConfig.ItemList[k];
									itemProbList.Add(new TemplateKey(item.Type, item.TemplateId));
								}
							}
							bool flag7 = itemProbList.Count > 0;
							if (flag7)
							{
								TemplateKey itemTemplateKey = itemProbList[context.Random.Next(0, itemProbList.Count)];
								ItemKey itemKey = DomainManager.Item.CreateItem(context, itemTemplateKey.ItemType, itemTemplateKey.TemplateId);
								DomainManager.Taiwu.GetTaiwu().AddInventoryItem(context, itemKey, 1, false);
								int existItemIndex = itemDisplayDataList.FindIndex((ItemDisplayData data) => data.Key.Equals(itemKey));
								bool flag8 = existItemIndex >= 0;
								if (flag8)
								{
									itemDisplayDataList[existItemIndex].Amount++;
								}
								else
								{
									ItemDisplayData itemDisplayData = DomainManager.Item.GetItemDisplayData(itemKey, -1);
									itemDisplayDataList.Add(itemDisplayData);
								}
							}
							ObjectPool<List<TemplateKey>>.Instance.Return(itemProbList);
						}
					}
				}
				ItemDisplayData resourceDisplayData = new ItemDisplayData
				{
					Key = new ItemKey(12, 0, (short)(321 + (int)resourceType), -1),
					Amount = totalCount
				};
				itemDisplayDataList.Add(resourceDisplayData);
				DomainManager.Taiwu.GetTaiwu().ChangeResource(context, resourceType, totalCount);
				GameDataBridge.AddDisplayEvent<List<ItemDisplayData>, bool, bool, bool>(DisplayEventType.OpenGetItem_Item, itemDisplayDataList, false, false, true);
				result = true;
			}
			return result;
		}

		// Token: 0x06007CD9 RID: 31961 RVA: 0x004A1144 File Offset: 0x0049F344
		public void InitializeOwnedItems()
		{
			for (int i = 0; i < 15; i++)
			{
				DomainManager.Item.SetOwner(this._collectionCrickets[i], ItemOwnerType.Building, 44);
				DomainManager.Item.SetOwner(this._collectionCricketJars[i], ItemOwnerType.Building, 44);
			}
			foreach (KeyValuePair<BuildingBlockKey, BuildingEarningsData> pair in this._collectBuildingEarningsData)
			{
				BuildingBlockData blockData;
				bool flag = !DomainManager.Building.TryGetElement_BuildingBlocks(pair.Key, out blockData);
				if (!flag)
				{
					foreach (ItemKey itemKey in pair.Value.CollectionItemList)
					{
						DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, (int)blockData.TemplateId);
					}
					foreach (ItemKey itemKey2 in pair.Value.ShopSoldItemList)
					{
						DomainManager.Item.SetOwner(itemKey2, ItemOwnerType.Building, (int)blockData.TemplateId);
					}
					foreach (ItemKey itemKey3 in pair.Value.FixBookInfoList)
					{
						DomainManager.Item.SetOwner(itemKey3, ItemOwnerType.Building, (int)blockData.TemplateId);
					}
				}
			}
			for (int j = 0; j < this._teaHorseCaravanData.CarryGoodsList.Count; j++)
			{
				ItemKey itemKey4 = this._teaHorseCaravanData.CarryGoodsList[j].Item1;
				DomainManager.Item.SetOwner(itemKey4, ItemOwnerType.Building, 51);
			}
			for (int k = 0; k < this._teaHorseCaravanData.ExchangeGoodsList.Count; k++)
			{
				ItemKey itemKey5 = this._teaHorseCaravanData.ExchangeGoodsList[k];
				DomainManager.Item.SetOwner(itemKey5, ItemOwnerType.Building, 51);
			}
			List<CricketCollectionData> cricketCollectionDataList = DomainManager.Extra.GetCricketCollectionDataList();
			for (int l = 0; l < cricketCollectionDataList.Count; l++)
			{
				DomainManager.Item.SetOwner(cricketCollectionDataList[l].Cricket, ItemOwnerType.Building, 44);
				DomainManager.Item.SetOwner(cricketCollectionDataList[l].CricketJar, ItemOwnerType.Building, 44);
			}
			IReadOnlyDictionary<ulong, Feast> feasts = DomainManager.Extra.GetAllFeasts();
			foreach (Feast feast in feasts.Values)
			{
				for (int m = 0; m < GlobalConfig.Instance.FeastCount; m++)
				{
					ItemKey dish = feast.GetDish(m);
					bool flag2 = dish.IsValid();
					if (flag2)
					{
						DomainManager.Item.SetOwner(dish, ItemOwnerType.Building, 47);
					}
				}
				for (int n = 0; n < GlobalConfig.Instance.FeastGiftCount; n++)
				{
					ItemKey gift = feast.GetGift(n);
					bool flag3 = gift.Id >= 0;
					if (flag3)
					{
						DomainManager.Item.SetOwner(gift, ItemOwnerType.Building, 47);
					}
				}
			}
		}

		// Token: 0x06007CDA RID: 31962 RVA: 0x004A153C File Offset: 0x0049F73C
		[DomainMethod]
		public unsafe MakeItemData StartMakeItem(DataContext context, StartMakeArguments startMakeArguments)
		{
			int charId = startMakeArguments.CharId;
			BuildingBlockKey buildingBlockKey = startMakeArguments.BuildingBlockKey;
			ItemDisplayData tool = startMakeArguments.Tool;
			ItemDisplayData material = startMakeArguments.Material;
			sbyte itemType = startMakeArguments.ItemType;
			List<short> itemList = startMakeArguments.ItemList;
			short makeItemSubTypeId = startMakeArguments.MakeItemSubTypeId;
			ResourceInts resourceCount = startMakeArguments.ResourceCount;
			ResourceInts needResource = startMakeArguments.NeedResource;
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			int makeCount = itemList.Count;
			bool flag = tool.Key.IsValid();
			if (flag)
			{
				sbyte grade = ItemTemplateHelper.GetGrade(material.Key.ItemType, material.Key.TemplateId);
				CraftToolItem toolConfig = Config.CraftTool.Instance[tool.Key.TemplateId];
				int cost = (int)toolConfig.DurabilityCost[(int)grade] * makeCount;
				DomainManager.Item.ReduceToolDurability(context, charId, tool.Key, cost, tool.ItemSourceType);
			}
			bool flag2 = charId == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag2)
			{
				for (sbyte i = 0; i < 8; i += 1)
				{
					int value = needResource.Get((int)i);
					bool flag3 = value > 0;
					if (flag3)
					{
						this.ConsumeResource(context, i, value);
					}
				}
			}
			else
			{
				needResource = needResource.GetReversed();
				character.ChangeResources(context, ref needResource);
			}
			DomainManager.Taiwu.RemoveItem(context, material.Key, makeCount, material.ItemSourceType, true, false);
			MaterialResources materialResources = default(MaterialResources);
			for (int j = 0; j < 6; j++)
			{
				*(ref materialResources.Items.FixedElementField + (IntPtr)j * 2) = (short)(*(ref resourceCount.Items.FixedElementField + (IntPtr)j * 4));
			}
			short configTime = MakeItemSubType.Instance[makeItemSubTypeId].Time;
			short time = (short)((int)configTime * (makeCount / 3 + 1));
			MakeItemData makeItemData = new MakeItemData
			{
				ProductItemIdList = itemList,
				ProductItemType = itemType,
				LeftTime = time,
				ToolKey = tool.Key,
				MaterialKey = material.Key,
				MaterialResources = materialResources,
				IsPerfect = startMakeArguments.IsPerfect
			};
			this.AddElement_MakeItemDict(buildingBlockKey, makeItemData, context);
			return makeItemData;
		}

		// Token: 0x06007CDB RID: 31963 RVA: 0x004A1764 File Offset: 0x0049F964
		[DomainMethod]
		public unsafe bool CheckMakeCondition(MakeConditionArguments makeConditionArguments)
		{
			int charId = makeConditionArguments.CharId;
			BuildingBlockKey buildingBlockKey = makeConditionArguments.BuildingBlockKey;
			ItemKey toolKey = makeConditionArguments.ToolKey;
			ItemKey materialKey = makeConditionArguments.MaterialKey;
			short makeCount = makeConditionArguments.MakeCount;
			ResourceInts resourceCount = makeConditionArguments.ResourceCount;
			short makeItemSubTypeId = makeConditionArguments.MakeItemSubTypeId;
			bool isManual = makeConditionArguments.IsManual;
			bool isPerfect = makeConditionArguments.IsPerfect;
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			ResourceInts needResources = default(ResourceInts);
			for (int i = 0; i < 8; i++)
			{
				short amount = ItemTemplateHelper.GetCraftMaterialRequiredResourceAmount(materialKey.TemplateId);
				*(ref needResources.Items.FixedElementField + (IntPtr)i * 4) = *(ref resourceCount.Items.FixedElementField + (IntPtr)i * 4) * (int)makeCount * (int)amount;
			}
			bool resourceIsMeet = ((charId == DomainManager.Taiwu.GetTaiwuCharId()) ? this.GetAllTaiwuResources() : (*character.GetResources())).CheckIsMeet(ref needResources);
			bool flag = !resourceIsMeet;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte grade = ItemTemplateHelper.GetGrade(materialKey.ItemType, materialKey.TemplateId);
				CraftToolItem toolConfig = Config.CraftTool.Instance[toolKey.TemplateId];
				short oneCost = toolConfig.DurabilityCost[(int)grade];
				int totalCost = (int)(oneCost * makeCount);
				bool toolIsMeet = this.CheckTool(toolKey, totalCost, (int)oneCost, -1);
				bool flag2 = !toolIsMeet;
				if (flag2)
				{
					result = false;
				}
				else
				{
					MaterialItem materialConfig = Config.Material.Instance[materialKey.TemplateId];
					int requirementReducePercent = 0;
					bool flag3 = !buildingBlockKey.IsInvalid;
					if (flag3)
					{
						requirementReducePercent = this.GetBuildingEffectForMake(buildingBlockKey, materialConfig.RequiredLifeSkillType).Item1;
					}
					short requiredAttainment = GameData.Domains.Building.SharedMethods.GetMakeRequiredLifeSkillAttainment((int)materialKey.TemplateId, (int)makeItemSubTypeId, isManual, isPerfect, requirementReducePercent);
					bool lifeSkillIsMeet = this.CheckLifeSkill(materialConfig.RequiredLifeSkillType, (int)requiredAttainment, toolKey);
					bool flag4 = !lifeSkillIsMeet;
					result = !flag4;
				}
			}
			return result;
		}

		// Token: 0x06007CDC RID: 31964 RVA: 0x004A192C File Offset: 0x0049FB2C
		[DomainMethod]
		public void TryShowNotifications()
		{
			bool outsideMakeItem = this._outsideMakeItem;
			if (outsideMakeItem)
			{
				DomainManager.World.GetInstantNotifications().AddMakeItemOutsideSettlement();
				this._outsideMakeItem = false;
			}
		}

		// Token: 0x06007CDD RID: 31965 RVA: 0x004A1960 File Offset: 0x0049FB60
		[DomainMethod]
		public List<ItemDisplayData> GetMakeItems(DataContext context, BuildingBlockKey buildingBlockKey)
		{
			MakeItemData makeData = this.GetElement_MakeItemDict(buildingBlockKey);
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			List<ItemDisplayData> displayDatas = new List<ItemDisplayData>();
			this.RemoveElement_MakeItemDict(buildingBlockKey, context);
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			Location location = taiwuChar.GetLocation();
			bool flag = location == taiwuVillageLocation;
			if (!flag)
			{
				MapBlockData blockData = DomainManager.Map.GetBlock(location);
				bool flag2 = blockData.IsCityTown();
				if (flag2)
				{
				}
			}
			sbyte playerEffectFactor = 2;
			Dictionary<short, int> productItemDict = new Dictionary<short, int>();
			using (List<short>.Enumerator enumerator = makeData.ProductItemIdList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					short productItemId = enumerator.Current;
					ItemKey productItemKey;
					switch (makeData.ProductItemType)
					{
					case 0:
						productItemKey = DomainManager.Item.CreateWeapon(context, productItemId, playerEffectFactor);
						break;
					case 1:
						productItemKey = DomainManager.Item.CreateArmor(context, productItemId, playerEffectFactor);
						break;
					case 2:
						productItemKey = DomainManager.Item.CreateAccessory(context, productItemId, playerEffectFactor);
						break;
					default:
						productItemKey = DomainManager.Item.CreateItem(context, makeData.ProductItemType, productItemId);
						break;
					}
					ItemBase productItem = DomainManager.Item.GetBaseItem(productItemKey);
					bool flag3 = ItemType.IsEquipmentItemType(productItemKey.ItemType);
					if (flag3)
					{
						EquipmentBase equipItem = DomainManager.Item.GetBaseEquipment(productItemKey);
						equipItem.SetMaterialResources(makeData.MaterialResources, context);
						bool flag4 = ItemType.IsEquipmentEffectType(productItemKey.ItemType) && makeData.IsPerfect;
						if (flag4)
						{
							List<short> effectIdList = EquipmentEffect.Instance.GetAllKeys().Where(delegate(short k)
							{
								EquipmentEffectItem effectItem = EquipmentEffect.Instance[k];
								bool special = effectItem.Special;
								bool result;
								if (special)
								{
									result = false;
								}
								else
								{
									sbyte type = effectItem.Type;
									if (!true)
									{
									}
									bool flag14;
									switch (type)
									{
									case 0:
									{
										sbyte itemType = productItemKey.ItemType;
										bool flag13 = itemType <= 2;
										flag14 = flag13;
										break;
									}
									case 1:
										flag14 = (productItemKey.ItemType == 0);
										break;
									case 2:
										flag14 = (productItemKey.ItemType == 1);
										break;
									default:
										flag14 = false;
										break;
									}
									if (!true)
									{
									}
									result = flag14;
								}
								return result;
							}).ToList<short>();
							short effectId = -1;
							bool flag5 = effectIdList.Count > 0;
							if (flag5)
							{
								effectId = effectIdList.GetRandom(context.Random);
							}
							equipItem.ApplyDurabilityEquipmentEffectChange(context, (int)equipItem.GetEquipmentEffectId(), (int)effectId);
							equipItem.SetEquipmentEffectId(effectId, context);
							equipItem.SetCurrDurability(equipItem.GetMaxDurability(), context);
						}
					}
					bool stackable = productItem.GetStackable();
					if (stackable)
					{
						int amount;
						bool flag6 = !productItemDict.TryGetValue(productItemId, out amount);
						if (flag6)
						{
							productItemDict.Add(productItemId, 1);
							displayDatas.Add(DomainManager.Item.GetItemDisplayData(productItem, 1, DomainManager.Taiwu.GetTaiwuCharId(), -1));
						}
						else
						{
							ItemDisplayData displayData = displayDatas.Find((ItemDisplayData d) => d.Key.TemplateId == productItemId);
							bool flag7 = displayData != null;
							if (flag7)
							{
								displayData.Amount = amount + 1;
								productItemDict[productItemId] = displayData.Amount;
							}
						}
					}
					else
					{
						displayDatas.Add(DomainManager.Item.GetItemDisplayData(productItem, 1, DomainManager.Taiwu.GetTaiwuCharId(), -1));
					}
					ItemSourceType itemSource = DomainManager.Extra.GetBuildingDefaultStoreLocation().GetMakeType(-1);
					bool flag8 = itemSource == ItemSourceType.Inventory && !DomainManager.Taiwu.CanTransferItemToWarehouse(context);
					if (flag8)
					{
						itemSource = ItemSourceType.Warehouse;
						this._outsideMakeItem = true;
					}
					DomainManager.Taiwu.AddItem(context, productItemKey, 1, itemSource, false);
					bool flag9 = productItem.GetGrade() >= DomainManager.World.GetXiangshuLevel();
					if (flag9)
					{
						DomainManager.Taiwu.AddLegacyPoint(context, 30, 100);
					}
					sbyte skillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(productItemKey.ItemType, productItemKey.TemplateId);
					bool flag10 = skillType - 6 <= 1 || skillType - 10 <= 1;
					bool flag11 = flag10;
					if (flag11)
					{
						int seniority = ProfessionFormulaImpl.Calculate(16, (int)productItem.GetGrade());
						DomainManager.Extra.ChangeProfessionSeniority(context, 2, seniority, true, false);
					}
					bool flag12 = ItemTemplateHelper.GetItemSubType(productItemKey.ItemType, productItemKey.TemplateId) == 800;
					if (flag12)
					{
						sbyte grade = ItemTemplateHelper.GetGrade(productItemKey.ItemType, productItemKey.TemplateId);
						int seniority2 = ProfessionFormulaImpl.Calculate(82, (int)grade);
						DomainManager.Extra.ChangeProfessionSeniority(context, 13, seniority2, true, false);
					}
				}
			}
			return displayDatas;
		}

		// Token: 0x06007CDE RID: 31966 RVA: 0x004A1DE8 File Offset: 0x0049FFE8
		[DomainMethod]
		public MakeItemData GetMakingItemData(BuildingBlockKey buildingBlockKey)
		{
			MakeItemData data;
			this._makeItemDict.TryGetValue(buildingBlockKey, out data);
			return data;
		}

		// Token: 0x06007CDF RID: 31967 RVA: 0x004A1E0C File Offset: 0x004A000C
		public void UpdateMakingProgressOnMonthChange(DataContext context)
		{
			foreach (KeyValuePair<BuildingBlockKey, MakeItemData> makeItem in this._makeItemDict)
			{
				bool flag = makeItem.Value.LeftTime == 0;
				if (!flag)
				{
					BuildingBlockData blockData = this._buildingBlocks[makeItem.Key];
					sbyte b;
					bool flag2 = blockData.CanUse() && this.AllDependBuildingAvailable(makeItem.Key, blockData.TemplateId, out b);
					if (flag2)
					{
						MakeItemData value = makeItem.Value;
						value.LeftTime -= 1;
						this.SetElement_MakeItemDict(makeItem.Key, makeItem.Value, context);
					}
				}
			}
		}

		// Token: 0x06007CE0 RID: 31968 RVA: 0x004A1EE0 File Offset: 0x004A00E0
		[DomainMethod]
		public MakeResult GetMakeResult(short materialTemplateId, ItemKey toolKey, BuildingBlockKey buildingBlockKey, sbyte lifeSkillType, List<short> makeItemSubtypeIdList, short makeItemSubTypeId = -1)
		{
			MakeItemSubTypeItem makeItemSubTypeConfig = MakeItemSubType.Instance[makeItemSubtypeIdList.First<short>()];
			sbyte itemType = makeItemSubTypeConfig.Result.ItemType;
			bool makeIsManual = makeItemSubTypeId != -1;
			ValueTuple<int, bool> buildingEffectForMake = this.GetBuildingEffectForMake(buildingBlockKey, lifeSkillType);
			int attainmentEffect = buildingEffectForMake.Item1;
			bool upgradeMakeItem = buildingEffectForMake.Item2;
			short totalAttainment = this.GetLifeSkillTotalAttainment(lifeSkillType, toolKey);
			List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = DomainManager.Taiwu.GetTaiwu().GetLearnedLifeSkills();
			sbyte grade;
			int requiredAttainment;
			this.GetMaterialGradeAndAttainment(materialTemplateId, itemType, lifeSkillType, (int)totalAttainment, makeItemSubtypeIdList, out grade, out requiredAttainment, learnedLifeSkills, makeItemSubTypeId, attainmentEffect);
			int resultIndex;
			MakeResultStage[] makeResultStageArray;
			this.GetMakeResultStageArray(materialTemplateId, grade, requiredAttainment, (int)totalAttainment, makeItemSubtypeIdList, attainmentEffect, lifeSkillType, out resultIndex, out makeResultStageArray, makeItemSubTypeId, makeIsManual, upgradeMakeItem);
			string upgradeBuildingName = string.Empty;
			BuildingBlock.Instance.Iterate(delegate(BuildingBlockItem item)
			{
				bool flag = item.UpgradeMakeItem && item.RequireLifeSkillType == lifeSkillType;
				bool result;
				if (flag)
				{
					upgradeBuildingName = item.Name;
					result = false;
				}
				else
				{
					result = true;
				}
				return result;
			});
			return new MakeResult(resultIndex, makeResultStageArray, upgradeBuildingName, upgradeMakeItem);
		}

		// Token: 0x06007CE1 RID: 31969 RVA: 0x004A1FD8 File Offset: 0x004A01D8
		private void GetMakeResultStageArray(short materialTemplateId, sbyte grade, int requiredAttainment, int totalAttainment, List<short> makeItemSubtypeIdList, int attainmentEffect, sbyte lifeSkillType, out int resultIndex, out MakeResultStage[] makeResultStageArray, short makeItemSubTypeId = -1, bool makeIsManual = false, bool upgradeMakeItem = false)
		{
			bool checkOne = makeIsManual || makeItemSubtypeIdList.Count == 1;
			bool flag = makeItemSubTypeId == -1;
			if (flag)
			{
				makeItemSubTypeId = makeItemSubtypeIdList.First<short>();
			}
			MakeItemSubTypeItem makeItemSubTypeConfig = MakeItemSubType.Instance[makeItemSubTypeId];
			sbyte itemType = makeItemSubTypeConfig.Result.ItemType;
			short subTypeExtraLifeSkill = 0;
			if (makeIsManual)
			{
				subTypeExtraLifeSkill = makeItemSubTypeConfig.ExtraLifeSkill * (short)(grade + 1);
			}
			makeResultStageArray = new MakeResultStage[3];
			resultIndex = 0;
			for (int i = 0; i < 3; i++)
			{
				sbyte tempGrade = grade;
				sbyte targetGrade;
				int finalRequirement;
				this.GetStageRequirementAndGrade(i, tempGrade, itemType, requiredAttainment, subTypeExtraLifeSkill, out targetGrade, out finalRequirement);
				finalRequirement = Math.Max(0, finalRequirement);
				bool lifeSkillIsMeet = totalAttainment >= finalRequirement;
				bool flag2 = checkOne;
				bool addStage;
				if (flag2)
				{
					sbyte baseGrade = ItemTemplateHelper.GetGrade(itemType, makeItemSubTypeConfig.Result.TemplateId);
					ValueTuple<bool, sbyte, short> finalGradeAndId = this.GetFinalGradeAndId(baseGrade, targetGrade, makeItemSubTypeConfig.Result.ItemType, makeItemSubTypeConfig.Result.TemplateId);
					bool success = finalGradeAndId.Item1;
					sbyte finalGrade = finalGradeAndId.Item2;
					short finialTemplateId = finalGradeAndId.Item3;
					bool exist = makeResultStageArray.Exist((MakeResultStage s) => s.IsInit && s.TemplateId == finialTemplateId);
					addStage = (success && !exist);
					bool flag3 = addStage;
					if (flag3)
					{
						makeResultStageArray[i] = new MakeResultStage(finalRequirement, lifeSkillIsMeet, itemType, finialTemplateId, makeItemSubTypeId);
					}
				}
				else
				{
					List<short> templateIdList = new List<short>();
					List<short> subTypeIdList = new List<short>();
					foreach (short subTypeId in makeItemSubtypeIdList)
					{
						MakeItemSubTypeItem subTypeConfig = MakeItemSubType.Instance[subTypeId];
						bool flag4 = itemType == 8 && lifeSkillType == 8;
						int num;
						if (flag4)
						{
							List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = DomainManager.Taiwu.GetTaiwu().GetLearnedLifeSkills();
							this.GetMaterialGradeAndAttainment(materialTemplateId, itemType, lifeSkillType, totalAttainment, makeItemSubtypeIdList, out grade, out num, learnedLifeSkills, subTypeId, attainmentEffect);
						}
						tempGrade = grade;
						sbyte targetGradeMore;
						this.GetStageRequirementAndGrade(i, tempGrade, itemType, requiredAttainment, subTypeExtraLifeSkill, out targetGradeMore, out num);
						sbyte tempBaseGrade = ItemTemplateHelper.GetGrade(itemType, subTypeConfig.Result.TemplateId);
						ValueTuple<bool, sbyte, short> finalGradeAndId = this.GetFinalGradeAndId(tempBaseGrade, targetGradeMore, subTypeConfig.Result.ItemType, subTypeConfig.Result.TemplateId);
						bool success2 = finalGradeAndId.Item1;
						sbyte finalGrade2 = finalGradeAndId.Item2;
						short finialTemplateId = finalGradeAndId.Item3;
						bool exist2 = makeResultStageArray.Exist((MakeResultStage s) => s.IsInit && s.TemplateIdList != null && s.TemplateIdList.Contains(finialTemplateId));
						bool flag5 = success2 && !exist2;
						if (flag5)
						{
							templateIdList.Add(finialTemplateId);
							subTypeIdList.Add(subTypeId);
						}
					}
					addStage = (templateIdList.Count > 0);
					bool flag6 = addStage;
					if (flag6)
					{
						makeResultStageArray[i] = new MakeResultStage(finalRequirement, lifeSkillIsMeet, itemType, templateIdList, subTypeIdList);
					}
				}
				bool flag7 = addStage && lifeSkillIsMeet;
				if (flag7)
				{
					bool flag8 = i < 2 || (upgradeMakeItem && i == 2);
					if (flag8)
					{
						resultIndex = i;
					}
				}
			}
		}

		// Token: 0x06007CE2 RID: 31970 RVA: 0x004A2308 File Offset: 0x004A0508
		[return: TupleElementNames(new string[]
		{
			"success",
			"finalGrade",
			"finialTemplateId"
		})]
		private ValueTuple<bool, sbyte, short> GetFinalGradeAndId(sbyte baseGrade, sbyte targetGrade, sbyte itemType, short baseTemplateId)
		{
			short baseGroupId = ItemTemplateHelper.GetGroupId(itemType, baseTemplateId);
			bool flag = baseGroupId < 0;
			ValueTuple<bool, sbyte, short> result;
			if (flag)
			{
				result = new ValueTuple<bool, sbyte, short>(true, Convert.ToSByte(baseGrade), baseTemplateId);
			}
			else
			{
				targetGrade = Math.Clamp(targetGrade, 0, 8);
				int gradeOffset = Math.Max((int)(targetGrade - baseGrade), -2);
				short resultTemplateId = Convert.ToInt16((int)baseTemplateId + gradeOffset);
				int resultGroupId = (int)(ItemTemplateHelper.CheckTemplateValid(itemType, resultTemplateId) ? ItemTemplateHelper.GetGroupId(itemType, resultTemplateId) : -1);
				bool success = resultGroupId == (int)baseGroupId;
				result = new ValueTuple<bool, sbyte, short>(success, Convert.ToSByte(targetGrade), resultTemplateId);
			}
			return result;
		}

		// Token: 0x06007CE3 RID: 31971 RVA: 0x004A2388 File Offset: 0x004A0588
		private void GetMaterialGradeAndAttainment(short materialTemplateId, sbyte itemType, sbyte lifeSkillType, int totalAttainment, List<short> makeItemSubtypeIdList, out sbyte grade, out int requiredAttainment, List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills, short makeItemSubTypeId = -1, int attainmentEffect = 0)
		{
			MaterialItem materialConfig = Config.Material.Instance[materialTemplateId];
			grade = materialConfig.Grade;
			requiredAttainment = (int)GameData.Domains.Building.SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect((int)materialConfig.RequiredAttainment, attainmentEffect);
			bool flag = itemType == 8 && lifeSkillType == 8;
			if (flag)
			{
				grade = GameData.Domains.Building.SharedMethods.GetHerbMaterialTempGrade(grade, makeItemSubtypeIdList, makeItemSubTypeId);
				requiredAttainment = (int)GlobalConfig.Instance.MakeMadicineAttainments[(int)grade];
				requiredAttainment = (int)GameData.Domains.Building.SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect(requiredAttainment, attainmentEffect);
			}
			else
			{
				bool flag2 = itemType == 7;
				if (flag2)
				{
					int finishedCookingCount = 0;
					if (learnedLifeSkills != null)
					{
						learnedLifeSkills.ForEach(delegate(GameData.Domains.Character.LifeSkillItem item)
						{
							bool flag4 = item.IsAllPagesRead();
							if (flag4)
							{
								Config.LifeSkillItem skillConfig = LifeSkill.Instance[item.SkillTemplateId];
								bool flag5 = skillConfig.Type == 14;
								if (flag5)
								{
									int finishedCookingCount = finishedCookingCount;
									finishedCookingCount++;
								}
							}
						});
					}
					int cookingUpgradeGrade = finishedCookingCount;
					sbyte originGrade = grade;
					for (int i = 0; i < cookingUpgradeGrade; i++)
					{
						sbyte targetGrade = (sbyte)Math.Min(6, (int)originGrade + i);
						ValueTuple<bool, sbyte, short> finalGradeAndId = this.GetFinalGradeAndId(originGrade, targetGrade, itemType, materialTemplateId);
						sbyte finalGrade = finalGradeAndId.Item2;
						short finalId = finalGradeAndId.Item3;
						MaterialItem config = Config.Material.Instance[finalId];
						short foodRequire = GameData.Domains.Building.SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect((int)config.RequiredAttainment, attainmentEffect);
						bool flag3 = totalAttainment >= (int)foodRequire;
						if (!flag3)
						{
							break;
						}
						requiredAttainment = (int)foodRequire;
						grade = finalGrade;
					}
				}
			}
		}

		// Token: 0x06007CE4 RID: 31972 RVA: 0x004A24C4 File Offset: 0x004A06C4
		private void GetStageRequirementAndGrade(int i, sbyte startGrade, sbyte itemType, int requirement, short subTypeExtraLifeSkill, out sbyte targetGrade, out int targetRequirement)
		{
			bool flag = i == 0;
			if (flag)
			{
				targetRequirement = this.GetStageRequiredAttainment(i, requirement, subTypeExtraLifeSkill);
				targetGrade = ((itemType == 7) ? startGrade : Convert.ToSByte((int)(startGrade - 1)));
			}
			else
			{
				bool flag2 = i == 1;
				if (flag2)
				{
					targetRequirement = this.GetStageRequiredAttainment(i, requirement, subTypeExtraLifeSkill);
					targetGrade = ((itemType == 7) ? Convert.ToSByte((int)(startGrade + 1)) : startGrade);
				}
				else
				{
					targetRequirement = this.GetStageRequiredAttainment(i, requirement, subTypeExtraLifeSkill);
					targetGrade = ((itemType == 7) ? Convert.ToSByte((int)(startGrade + 2)) : Convert.ToSByte((int)(startGrade + 1)));
				}
			}
		}

		// Token: 0x06007CE5 RID: 31973 RVA: 0x004A2554 File Offset: 0x004A0754
		private int GetStageRequiredAttainment(int i, int attainment, short subTypeExtraLifeSkill)
		{
			return attainment * GlobalConfig.Instance.MakeItemStageAttainmentFactor[i] / 100 + (int)subTypeExtraLifeSkill;
		}

		// Token: 0x06007CE6 RID: 31974 RVA: 0x004A257C File Offset: 0x004A077C
		[return: TupleElementNames(new string[]
		{
			"grade",
			"templateId"
		})]
		public ValueTuple<sbyte, short> GetMakeResultTargetItemGradeAndTemplateId(short materialTemplateId, short totalAttainment, sbyte lifeSkillType, List<short> makeItemSubtypeIdList, short makeItemSubTypeId, List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills, IRandomSource randomSource, bool upgradeMakeItem, int attainmentEffect)
		{
			MakeItemSubTypeItem makeItemSubTypeConfig = MakeItemSubType.Instance[makeItemSubTypeId];
			sbyte itemType = makeItemSubTypeConfig.Result.ItemType;
			sbyte grade;
			int requiredAttainment;
			this.GetMaterialGradeAndAttainment(materialTemplateId, itemType, lifeSkillType, (int)totalAttainment, makeItemSubtypeIdList, out grade, out requiredAttainment, learnedLifeSkills, makeItemSubTypeId, attainmentEffect);
			int resultIndex;
			MakeResultStage[] makeResultStageArray;
			this.GetMakeResultStageArray(materialTemplateId, grade, requiredAttainment, (int)totalAttainment, makeItemSubtypeIdList, attainmentEffect, lifeSkillType, out resultIndex, out makeResultStageArray, makeItemSubTypeId, false, upgradeMakeItem);
			MakeResultStage stage = makeResultStageArray[resultIndex];
			return stage.LifeSkillIsMeet ? stage.GetGradeAndId(randomSource) : new ValueTuple<sbyte, short>(0, -1);
		}

		// Token: 0x06007CE7 RID: 31975 RVA: 0x004A2600 File Offset: 0x004A0800
		[DomainMethod]
		[Obsolete("可以用RepairItemOptional替代")]
		public ItemDisplayData RepairItem(DataContext context, int charId, ItemKey toolKey, ItemKey itemKey)
		{
			return this.RepairItemOptional(context, charId, toolKey, itemKey, 1);
		}

		// Token: 0x06007CE8 RID: 31976 RVA: 0x004A2620 File Offset: 0x004A0820
		[DomainMethod]
		public ItemDisplayData RepairItemOptional(DataContext context, int charId, ItemKey toolKey, ItemKey itemKey, sbyte toolSourceType)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
			short durability = item.GetCurrDurability();
			sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
			bool flag = toolKey.IsValid() && toolKey.TemplateId != 54;
			if (flag)
			{
				CraftToolItem toolConfig = Config.CraftTool.Instance[toolKey.TemplateId];
				short cost = toolConfig.DurabilityCost[(int)grade];
				DomainManager.Item.ReduceToolDurability(context, charId, toolKey, (int)cost, toolSourceType);
			}
			EquipmentBase equip = DomainManager.Item.GetBaseEquipment(itemKey);
			ResourceInts needResources = ItemTemplateHelper.GetRepairNeedResources(equip.GetMaterialResources(), itemKey, durability);
			bool flag2 = charId == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag2)
			{
				for (sbyte i = 0; i < 8; i += 1)
				{
					int value = needResources.Get((int)i);
					bool flag3 = value > 0;
					if (flag3)
					{
						this.ConsumeResource(context, i, value);
					}
				}
			}
			else
			{
				needResources = needResources.GetReversed();
				character.ChangeResources(context, ref needResources);
			}
			sbyte skillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey.ItemType, itemKey.TemplateId);
			bool flag4 = skillType - 6 <= 1 || skillType - 10 <= 1;
			bool flag5 = flag4;
			if (flag5)
			{
				int seniority = (item.GetCurrDurability() > 0) ? ProfessionFormulaImpl.Calculate(18, (int)grade, (int)item.GetCurrDurability(), (int)item.GetMaxDurability()) : ProfessionFormulaImpl.Calculate(19, (int)grade);
				DomainManager.Extra.ChangeProfessionSeniority(context, 2, seniority, true, false);
			}
			item.SetCurrDurability(item.GetMaxDurability(), context);
			return DomainManager.Item.GetItemDisplayData(item, charId, -1, -1);
		}

		// Token: 0x06007CE9 RID: 31977 RVA: 0x004A27D0 File Offset: 0x004A09D0
		[DomainMethod]
		public List<ItemDisplayData> RepairItemList(DataContext context, int charId, List<MultiplyOperation> operationList)
		{
			List<ItemDisplayData> dataList = new List<ItemDisplayData>();
			foreach (MultiplyOperation operation in operationList)
			{
				for (int i = 0; i < operation.Count; i++)
				{
					ItemDisplayData result = this.RepairItemOptional(context, charId, operation.Tool, operation.Target, operation.ToolItemSourceType);
					dataList.Add(result);
				}
			}
			return dataList;
		}

		// Token: 0x06007CEA RID: 31978 RVA: 0x004A2868 File Offset: 0x004A0A68
		[DomainMethod]
		public unsafe bool CheckRepairConditionIsMeet(int charId, ItemKey toolKey, ItemKey itemKey, BuildingBlockKey buildingBlockKey)
		{
			bool targetIsMeet = DomainManager.Item.CheckItemNeedRepair(itemKey);
			bool flag = !targetIsMeet;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
				sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
				CraftToolItem toolConfig = Config.CraftTool.Instance[toolKey.TemplateId];
				short cost = toolConfig.DurabilityCost[(int)grade];
				bool toolIsMeet = this.CheckTool(toolKey, (int)cost, (int)cost, -1);
				bool flag2 = !toolIsMeet;
				if (flag2)
				{
					result = false;
				}
				else
				{
					EquipmentBase equip = DomainManager.Item.GetBaseEquipment(itemKey);
					ResourceInts needResources = ItemTemplateHelper.GetRepairNeedResources(equip.GetMaterialResources(), itemKey, item.GetCurrDurability());
					bool resourceIsMeet = ((charId == DomainManager.Taiwu.GetTaiwuCharId()) ? this.GetAllTaiwuResources() : (*character.GetResources())).CheckIsMeet(ref needResources);
					bool flag3 = !resourceIsMeet;
					if (flag3)
					{
						result = false;
					}
					else
					{
						sbyte requiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey.ItemType, itemKey.TemplateId);
						int requireAttainment = (int)ItemTemplateHelper.GetRepairRequiredAttainment(itemKey.ItemType, itemKey.TemplateId, item.GetCurrDurability());
						bool flag4 = !buildingBlockKey.IsInvalid;
						if (flag4)
						{
							int attainmentEffect = this.GetBuildingEffectForMake(buildingBlockKey, requiredLifeSkillType).Item1;
							requireAttainment = (int)GameData.Domains.Building.SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect(requireAttainment, attainmentEffect);
						}
						requireAttainment = Math.Max(0, requireAttainment);
						bool lifeSkillIsMeet = this.CheckLifeSkill(requiredLifeSkillType, requireAttainment, toolKey);
						result = lifeSkillIsMeet;
					}
				}
			}
			return result;
		}

		// Token: 0x06007CEB RID: 31979 RVA: 0x004A29D4 File Offset: 0x004A0BD4
		[DomainMethod]
		public ValueTuple<bool, ItemDisplayData> AddItemPoison(DataContext context, int charId, ItemDisplayData tool, ItemDisplayData target, ItemDisplayData[] poisons, List<ItemDisplayData> condensePoisonItemList)
		{
			ItemKey targetKey = target.Key;
			ItemBase targetBaseItem = DomainManager.Item.GetBaseItem(targetKey);
			CraftToolItem toolConfig = Config.CraftTool.Instance[tool.Key.TemplateId];
			FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(targetKey);
			bool flag = ModificationStateHelper.IsActive(targetKey.ModificationState, 1);
			if (flag)
			{
				bool flag2 = poisonEffects.IsValid && !poisonEffects.IsIdentified && !ItemType.IsEquipmentItemType(targetKey.ItemType);
				if (flag2)
				{
					return new ValueTuple<bool, ItemDisplayData>(false, new ItemDisplayData());
				}
			}
			short durabilityCost = 0;
			ItemBase resultItems = targetBaseItem;
			List<short> condensedMedicineTemplateIdList = new List<short>();
			for (int i = 0; i < poisons.Length; i++)
			{
				ItemDisplayData poison = poisons[i];
				bool flag3 = poison == null || !poison.Key.HasTemplate;
				if (!flag3)
				{
					MedicineItem poisonMedicineConfig = Config.Medicine.Instance[poison.Key.TemplateId];
					sbyte grade = poisonMedicineConfig.Grade;
					List<PoisonSlot> poisonSlotList = poisonEffects.PoisonSlotList;
					PoisonSlot slot = (poisonSlotList != null) ? poisonSlotList.GetOrDefault(i) : null;
					sbyte? b;
					if (slot == null)
					{
						b = null;
					}
					else
					{
						MedicineItem medicineConfig = slot.MedicineConfig;
						b = ((medicineConfig != null) ? new sbyte?(medicineConfig.Grade) : null);
					}
					sbyte? b2 = b;
					sbyte slotPoisonGrade = b2.GetValueOrDefault(-1);
					bool needKeepOldCondenseData = poisonMedicineConfig.Grade <= slotPoisonGrade && slot != null && slot.IsCondensed;
					condensedMedicineTemplateIdList.Clear();
					bool flag4 = condensePoisonItemList != null && condensePoisonItemList.Count > 0;
					if (flag4)
					{
						foreach (ItemDisplayData itemData in condensePoisonItemList)
						{
							MedicineItem condensedMedicineConfig = Config.Medicine.Instance[itemData.Key.TemplateId];
							bool flag5 = condensedMedicineConfig.PoisonType == poisonMedicineConfig.PoisonType;
							if (flag5)
							{
								for (int j = 0; j < itemData.Amount; j++)
								{
									condensedMedicineTemplateIdList.Add(condensedMedicineConfig.TemplateId);
								}
							}
						}
					}
					bool flag6 = condensedMedicineTemplateIdList.Count > 0;
					if (flag6)
					{
						sbyte condensedGrade = condensedMedicineTemplateIdList.Max((short m) => ItemTemplateHelper.GetGrade(8, m));
						grade = Math.Max(grade, condensedGrade);
					}
					else
					{
						bool flag7 = needKeepOldCondenseData;
						if (flag7)
						{
							condensedMedicineTemplateIdList.AddRange(slot.CondensedMedicineTemplateIdList);
						}
					}
					bool flag8 = condensedMedicineTemplateIdList.Count > 0 || poison.Key.IsValid();
					if (flag8)
					{
						short cost = toolConfig.DurabilityCost[(int)grade];
						durabilityCost = Math.Max(durabilityCost, cost);
					}
					bool flag9 = !poison.Key.IsValid();
					if (flag9)
					{
						if (slot != null)
						{
							slot.SetPoison(poison.Key.TemplateId, condensedMedicineTemplateIdList);
						}
					}
					else
					{
						ValueTuple<ItemBase, bool> valueTuple = DomainManager.Item.SetAttachedPoisons(context, targetBaseItem, poison.Key.TemplateId, true, condensedMedicineTemplateIdList);
						ItemBase newItemObj = valueTuple.Item1;
						bool keyChanged = valueTuple.Item2;
						resultItems = newItemObj;
						bool flag10 = keyChanged;
						if (flag10)
						{
							ItemKey oldKey = targetKey;
							ItemKey newKey = newItemObj.GetItemKey();
							this.ChangeItem(context, oldKey, target.ItemSourceType, newKey, charId);
							targetKey = newItemObj.GetItemKey();
							targetBaseItem = newItemObj;
						}
						DomainManager.Taiwu.RemoveItem(context, poison.Key, 1, poison.ItemSourceType, true, false);
					}
				}
			}
			DomainManager.Item.SetPoisonsIdentified(context, resultItems.GetItemKey(), true);
			bool flag11 = tool.Key.IsValid();
			if (flag11)
			{
				DomainManager.Item.ReduceToolDurability(context, charId, tool.Key, (int)durabilityCost, tool.ItemSourceType);
			}
			bool flag12 = condensePoisonItemList != null && condensePoisonItemList.Count > 0;
			if (flag12)
			{
				foreach (ItemDisplayData data in condensePoisonItemList)
				{
					DomainManager.Taiwu.RemoveItem(context, data.Key, data.Amount, data.ItemSourceType, true, false);
				}
			}
			return new ValueTuple<bool, ItemDisplayData>(true, DomainManager.Item.GetItemDisplayData(resultItems, 1, charId, target.ItemSourceType));
		}

		// Token: 0x06007CEC RID: 31980 RVA: 0x004A2E48 File Offset: 0x004A1048
		[DomainMethod]
		public bool CheckAddPoisonCondition(int charId, ItemKey toolKey, ItemKey targetKey, ItemKey[] poisonKeys, BuildingBlockKey buildingBlockKey, FullPoisonEffects tempPoisonEffects)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			bool targetIsMeet = DomainManager.Item.GetBaseItem(targetKey).GetPoisonable();
			bool flag = !targetIsMeet;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(targetKey);
				bool hasChanged = !poisonEffects.SameOf(tempPoisonEffects) || (poisonEffects.IsValid && !poisonEffects.IsIdentified);
				bool flag2 = !hasChanged;
				if (flag2)
				{
					result = false;
				}
				else
				{
					CraftToolItem toolConfig = Config.CraftTool.Instance[toolKey.TemplateId];
					int requireAttainment = 0;
					short durabilityCost = 0;
					for (int index = 0; index < poisonKeys.Length; index++)
					{
						ItemKey poisonKey = poisonKeys[index];
						bool flag3 = !poisonKey.HasTemplate;
						if (!flag3)
						{
							short attainment = ItemTemplateHelper.GetPoisonRequiredAttainment(8, poisonKey.TemplateId);
							bool isCondensed = tempPoisonEffects.PoisonSlotList.CheckIndex(index) && tempPoisonEffects.PoisonSlotList[index].IsCondensed;
							bool flag4 = isCondensed;
							if (flag4)
							{
								short condensedAttainment = tempPoisonEffects.PoisonSlotList[index].CondensedMedicineTemplateIdList.Max((short m) => ItemTemplateHelper.GetPoisonRequiredAttainment(8, m));
								int bonus = GlobalConfig.Instance.CondensePoisonRequiredAttainmentBonus;
								condensedAttainment = Convert.ToInt16((int)condensedAttainment * (100 + bonus) / 100);
								attainment = Math.Max(attainment, condensedAttainment);
							}
							sbyte grade = ItemTemplateHelper.GetGrade(8, poisonKey.TemplateId);
							bool flag5 = isCondensed;
							if (flag5)
							{
								sbyte condensedGrade = tempPoisonEffects.PoisonSlotList[index].CondensedMedicineTemplateIdList.Max((short m) => ItemTemplateHelper.GetGrade(8, m));
								grade = Math.Max(grade, condensedGrade);
							}
							bool flag6 = isCondensed || poisonKey.IsValid();
							if (flag6)
							{
								requireAttainment = Math.Max((int)attainment, requireAttainment);
								short cost = toolConfig.DurabilityCost[(int)grade];
								durabilityCost = Math.Max(durabilityCost, cost);
							}
						}
					}
					bool toolIsMeet = this.CheckTool(toolKey, (int)durabilityCost, (int)durabilityCost, 9);
					bool flag7 = !toolIsMeet;
					if (flag7)
					{
						result = false;
					}
					else
					{
						bool flag8 = !buildingBlockKey.IsInvalid;
						if (flag8)
						{
							int attainmentEffect = this.GetBuildingEffectForMake(buildingBlockKey, 9).Item1;
							requireAttainment = (int)GameData.Domains.Building.SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect(requireAttainment, attainmentEffect);
						}
						bool lifeSkillIsMeet = this.CheckLifeSkill(9, requireAttainment, toolKey);
						bool flag9 = !lifeSkillIsMeet;
						result = !flag9;
					}
				}
			}
			return result;
		}

		// Token: 0x06007CED RID: 31981 RVA: 0x004A30C8 File Offset: 0x004A12C8
		[DomainMethod]
		public ValueTuple<bool, List<ItemDisplayData>> RemoveItemPoison(DataContext context, int charId, ItemDisplayData tool, ItemDisplayData target, ItemDisplayData[] medicines, bool isExtract)
		{
			List<ItemDisplayData> resultItemList = new List<ItemDisplayData>();
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			ItemKey[] equipment = character.GetEquipment();
			ItemKey targetKey = target.Key;
			ItemBase targetBaseItem = DomainManager.Item.GetBaseItem(targetKey);
			List<short> poisonMedicineIds = target.PoisonEffects.GetAllMedicineTemplateIds(false);
			CraftToolItem toolConfig = Config.CraftTool.Instance[tool.Key.TemplateId];
			List<ItemDisplayData> finialMedicineIdList = new List<ItemDisplayData>();
			for (int i = 0; i < medicines.Length; i++)
			{
				ItemDisplayData medicine = medicines[i];
				bool flag = medicine == null || !medicine.Key.IsValid() || poisonMedicineIds.Contains(medicine.Key.TemplateId);
				if (!flag)
				{
					MedicineItem config = Config.Medicine.Instance[medicine.Key.TemplateId];
					bool flag2 = !poisonMedicineIds.Exist((short id) => id > -1 && Config.Medicine.Instance[id].PoisonType == config.PoisonType);
					if (!flag2)
					{
						bool flag3 = medicine.HasAnyPoison && !medicine.PoisonIsIdentified;
						if (flag3)
						{
							resultItemList.Add(new ItemDisplayData());
							return new ValueTuple<bool, List<ItemDisplayData>>(false, resultItemList);
						}
						finialMedicineIdList.Add(medicine);
					}
				}
			}
			List<short> addedMedicineList = null;
			if (isExtract)
			{
				addedMedicineList = new List<short>();
			}
			ItemBase resultItem = null;
			short durabilityCost = 0;
			foreach (ItemDisplayData medicine2 in finialMedicineIdList)
			{
				MedicineItem config = Config.Medicine.Instance[medicine2.Key.TemplateId];
				short targetPoisonId = poisonMedicineIds.Find((short id) => id > -1 && Config.Medicine.Instance[id].PoisonType == config.PoisonType);
				ValueTuple<ItemBase, bool> valueTuple = DomainManager.Item.SetAttachedPoisons(context, targetBaseItem, targetPoisonId, false, null);
				ItemBase newItemObj = valueTuple.Item1;
				bool keyChanged = valueTuple.Item2;
				resultItem = newItemObj;
				bool flag4 = keyChanged;
				if (flag4)
				{
					ItemKey oldKey = targetKey;
					ItemKey newKey = newItemObj.GetItemKey();
					this.ChangeItem(context, oldKey, target.ItemSourceType, newKey, charId);
					targetKey = newItemObj.GetItemKey();
					targetBaseItem = newItemObj;
				}
				DomainManager.Taiwu.RemoveItem(context, medicine2.Key, 1, medicine2.ItemSourceType, true, false);
				sbyte grade = ItemTemplateHelper.GetGrade(8, targetPoisonId);
				short cost = toolConfig.DurabilityCost[(int)grade];
				durabilityCost = Math.Max(durabilityCost, cost);
				if (isExtract)
				{
					addedMedicineList.Add(targetPoisonId);
					PoisonSlot targetSlot = target.PoisonEffects.PoisonSlotList.Find((PoisonSlot s) => s.MedicineTemplateId == targetPoisonId);
					bool isCondensed = targetSlot.IsCondensed;
					if (isCondensed)
					{
						addedMedicineList.AddRange(targetSlot.CondensedMedicineTemplateIdList);
					}
				}
				int seniority = ProfessionFormulaImpl.Calculate(84, (int)grade);
				DomainManager.Extra.ChangeProfessionSeniority(context, 13, seniority, true, false);
			}
			bool flag5 = tool.Key.IsValid();
			if (flag5)
			{
				DomainManager.Item.ReduceToolDurability(context, charId, tool.Key, (int)durabilityCost, tool.ItemSourceType);
			}
			ItemDisplayData resultItemData = DomainManager.Item.GetItemDisplayData(resultItem, 1, charId, target.ItemSourceType);
			resultItemList.Add(resultItemData);
			if (isExtract)
			{
				foreach (short templateId in addedMedicineList)
				{
					ItemKey itemKey = DomainManager.Item.CreateItem(context, 8, templateId);
					DomainManager.Taiwu.AddItem(context, itemKey, 1, target.ItemSourceTypeEnum, false);
					ItemBase itemBase = DomainManager.Item.GetBaseItem(itemKey);
					int findIndex = resultItemList.FindIndex((ItemDisplayData d) => d.Key.Equals(itemKey));
					bool flag6 = findIndex >= 0;
					if (flag6)
					{
						resultItemList[findIndex].Amount++;
					}
					else
					{
						ItemDisplayData itemData = DomainManager.Item.GetItemDisplayData(itemBase, 1, charId, target.ItemSourceType);
						resultItemList.Add(itemData);
					}
				}
			}
			return new ValueTuple<bool, List<ItemDisplayData>>(true, resultItemList);
		}

		// Token: 0x06007CEE RID: 31982 RVA: 0x004A3530 File Offset: 0x004A1730
		[DomainMethod]
		public bool CheckRemovePoisonCondition(int charId, ItemKey toolKey, ItemKey targetKey, ItemKey[] medicineKeys, BuildingBlockKey buildingBlockKey, bool isExtract)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			CraftToolItem toolConfig = Config.CraftTool.Instance[toolKey.TemplateId];
			short durabilityCost = 0;
			FullPoisonEffects poisonEffect = DomainManager.Item.GetPoisonEffects(targetKey);
			for (int index = 0; index < medicineKeys.Length; index++)
			{
				ItemKey medicineKey = medicineKeys[index];
				bool flag = !medicineKey.IsValid();
				if (!flag)
				{
					short poisonId = poisonEffect.GetMedicineTemplateIdAt(index);
					sbyte medicineGrade = ItemTemplateHelper.GetGrade(medicineKey.ItemType, medicineKey.TemplateId);
					sbyte poisonGrade = ItemTemplateHelper.GetGrade(8, poisonId);
					short cost = toolConfig.DurabilityCost[(int)poisonGrade];
					durabilityCost = Math.Max(durabilityCost, cost);
					bool targetIsMeet = poisonEffect.IsValid && medicineGrade >= poisonGrade;
					bool flag2 = !targetIsMeet;
					bool result;
					if (flag2)
					{
						result = false;
					}
					else
					{
						int requireAttainment = (int)ItemTemplateHelper.GetPoisonRequiredAttainment(8, poisonId);
						if (isExtract)
						{
							int bonus = GlobalConfig.Instance.CondensePoisonRequiredAttainmentBonus;
							requireAttainment = (int)Convert.ToInt16(requireAttainment * (100 + bonus) / 100);
						}
						bool flag3 = !buildingBlockKey.IsInvalid;
						if (flag3)
						{
							int attainmentEffect = this.GetBuildingEffectForMake(buildingBlockKey, 9).Item1;
							requireAttainment = (int)GameData.Domains.Building.SharedMethods.GetRequiredLifeSkillAttainmentByBuildingEffect(requireAttainment, attainmentEffect);
						}
						bool lifeSkillIsMeet = this.CheckLifeSkill(9, requireAttainment, toolKey);
						bool flag4 = !lifeSkillIsMeet;
						if (!flag4)
						{
							goto IL_139;
						}
						result = false;
					}
					return result;
				}
				IL_139:;
			}
			bool toolIsMeet = this.CheckTool(toolKey, (int)durabilityCost, (int)durabilityCost, 9);
			bool flag5 = !toolIsMeet;
			return !flag5;
		}

		// Token: 0x06007CEF RID: 31983 RVA: 0x004A36B4 File Offset: 0x004A18B4
		[DomainMethod]
		public ItemDisplayData RefineItem(DataContext context, int charId, ItemDisplayData tool, ItemDisplayData target, ItemDisplayData[] materialItemArray, List<ItemSourceChange> changeList)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			ItemBase equipBaseItem = DomainManager.Item.GetBaseItem(target.Key);
			CraftToolItem toolConfig = Config.CraftTool.Instance[tool.Key.TemplateId];
			bool flag = ModificationStateHelper.IsActive(equipBaseItem.GetModificationState(), 2);
			RefiningEffects refinedEffects;
			if (flag)
			{
				refinedEffects = DomainManager.Item.GetRefinedEffects(target.Key);
			}
			else
			{
				refinedEffects.Initialize();
			}
			short[] materialTemplateIdArray = (from d in materialItemArray
			select d.Key.TemplateId).ToArray<short>();
			ResourceInts needResources = ItemTemplateHelper.GetRefineRequiredResources(refinedEffects.GetAllMaterialTemplateIds(), materialTemplateIdArray);
			bool flag2 = charId == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag2)
			{
				for (sbyte i = 0; i < 8; i += 1)
				{
					int value = needResources.Get((int)i);
					bool flag3 = value <= 0;
					if (!flag3)
					{
						this.ConsumeResource(context, i, value);
						sbyte skillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(target.Key.ItemType, target.Key.TemplateId);
						bool flag4 = skillType - 6 <= 1 || skillType - 10 <= 1;
						bool flag5 = flag4;
						if (flag5)
						{
							int seniority = ProfessionFormulaImpl.Calculate(20, value);
							DomainManager.Extra.ChangeProfessionSeniority(context, 2, seniority, true, false);
						}
					}
				}
			}
			else
			{
				needResources = needResources.GetReversed();
				character.ChangeResources(context, ref needResources);
			}
			short durabilityCost = 0;
			for (int j = 0; j < materialItemArray.Length; j++)
			{
				ItemDisplayData materialItem = materialItemArray[j];
				short curId = materialItem.Key.TemplateId;
				short oldId = refinedEffects.GetMaterialTemplateIdAt(j);
				bool isSame = curId == oldId;
				bool flag6 = isSame;
				if (!flag6)
				{
					ValueTuple<ItemBase, bool> valueTuple = DomainManager.Item.SetRefinedEffects(context, equipBaseItem, j, curId);
					ItemBase newItemObj = valueTuple.Item1;
					bool keyChanged = valueTuple.Item2;
					bool flag7 = keyChanged;
					if (flag7)
					{
						ItemKey oldKey = target.Key;
						ItemKey newKey = newItemObj.GetItemKey();
						equipBaseItem = newItemObj;
						this.ChangeItem(context, oldKey, target.ItemSourceType, newKey, charId);
					}
					else
					{
						ItemKey[] equipment = character.GetEquipment();
						character.SetEquipment(equipment, context);
					}
					short cost = 0;
					bool flag8 = curId >= 0;
					if (flag8)
					{
						sbyte grade = ItemTemplateHelper.GetGrade(5, curId);
						short curCost = toolConfig.DurabilityCost[(int)grade];
						cost = Math.Max(cost, curCost);
					}
					bool flag9 = oldId >= 0;
					if (flag9)
					{
						sbyte oldGrade = ItemTemplateHelper.GetGrade(5, oldId);
						short oldCost = toolConfig.DurabilityCost[(int)oldGrade];
						cost = Math.Max(cost, oldCost);
					}
					durabilityCost = Math.Max(durabilityCost, cost);
				}
			}
			ItemKey key = tool.Key;
			bool flag10 = key.IsValid();
			if (flag10)
			{
				DomainManager.Item.ReduceToolDurability(context, charId, tool.Key, (int)durabilityCost, tool.ItemSourceType);
			}
			foreach (ItemSourceChange change in changeList)
			{
				foreach (ItemKeyAndCount itemKeyAndCount in change.Items)
				{
					int num;
					itemKeyAndCount.Deconstruct(out key, out num);
					ItemKey itemKey = key;
					int count = num;
					bool flag11 = count > 0;
					if (flag11)
					{
						ItemKey newKey2 = DomainManager.Item.CreateMaterial(context, itemKey.TemplateId);
						DomainManager.Taiwu.AddItem(context, newKey2, count, change.ItemSourceType, false);
					}
					else
					{
						bool flag12 = count < 0;
						if (flag12)
						{
							DomainManager.Taiwu.RemoveItem(context, itemKey, -count, change.ItemSourceType, true, false);
						}
					}
				}
			}
			return DomainManager.Item.GetItemDisplayData(equipBaseItem, 1, charId, target.ItemSourceType);
		}

		// Token: 0x06007CF0 RID: 31984 RVA: 0x004A3AB4 File Offset: 0x004A1CB4
		[DomainMethod]
		public unsafe bool CheckRefineCondition(int charId, ItemKey toolKey, ItemKey equipItemKey, ItemDisplayData[] materialItemData, BuildingBlockKey buildingBlockKey)
		{
			ItemBase equipBaseItem = DomainManager.Item.GetBaseItem(equipItemKey);
			bool flag = !equipBaseItem.GetRefinable();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = ModificationStateHelper.IsActive(equipBaseItem.GetModificationState(), 2);
				RefiningEffects refinedEffects;
				if (flag2)
				{
					refinedEffects = DomainManager.Item.GetRefinedEffects(equipItemKey);
				}
				else
				{
					refinedEffects.Initialize();
				}
				short[] materialTemplateIdArray = (from d in materialItemData
				select d.Key.TemplateId).ToArray<short>();
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				ResourceInts needResources = ItemTemplateHelper.GetRefineRequiredResources(refinedEffects.GetAllMaterialTemplateIds(), materialTemplateIdArray);
				bool resourceIsMeet = ((charId == DomainManager.Taiwu.GetTaiwuCharId()) ? this.GetAllTaiwuResources() : (*character.GetResources())).CheckIsMeet(ref needResources);
				bool flag3 = !resourceIsMeet;
				if (flag3)
				{
					result = false;
				}
				else
				{
					sbyte requiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(equipItemKey.ItemType, equipItemKey.TemplateId);
					LifeSkillShorts needLifeSkill = default(LifeSkillShorts);
					bool haveNotChange = true;
					int requirementReducePercent = 0;
					bool flag4 = !buildingBlockKey.IsInvalid;
					if (flag4)
					{
						requirementReducePercent = this.GetBuildingEffectForMake(buildingBlockKey, requiredLifeSkillType).Item1;
					}
					CraftToolItem toolConfig = Config.CraftTool.Instance[toolKey.TemplateId];
					short durabilityCost = 0;
					for (int i = 0; i < materialItemData.Length; i++)
					{
						short oldId = refinedEffects.GetMaterialTemplateIdAt(i);
						short curId = materialItemData[i].Key.TemplateId;
						bool isSame = oldId == curId;
						bool flag5 = !isSame;
						if (flag5)
						{
							haveNotChange = false;
						}
						bool flag6 = curId >= 0 || oldId >= 0;
						if (flag6)
						{
							short id = curId;
							bool flag7 = curId < 0;
							if (flag7)
							{
								id = oldId;
							}
							MaterialItem materialConfig = Config.Material.Instance[id];
							short lifeSkillAttainment = *(ref needLifeSkill.Items.FixedElementField + (IntPtr)materialConfig.RequiredLifeSkillType * 2);
							int reduce = (materialConfig.RequiredAttainment == (short)requiredLifeSkillType) ? requirementReducePercent : 0;
							*(ref needLifeSkill.Items.FixedElementField + (IntPtr)materialConfig.RequiredLifeSkillType * 2) = (short)Math.Max((int)lifeSkillAttainment, (int)materialConfig.RequiredAttainment * (100 - reduce) / 100);
							bool flag8 = !isSame;
							if (flag8)
							{
								short cost = toolConfig.DurabilityCost[(int)materialConfig.Grade];
								bool flag9 = oldId >= 0;
								if (flag9)
								{
									MaterialItem oldMaterialConfig = Config.Material.Instance[oldId];
									short oldCost = toolConfig.DurabilityCost[(int)oldMaterialConfig.Grade];
									cost = Math.Max(cost, oldCost);
								}
								durabilityCost = Math.Max(durabilityCost, cost);
							}
						}
					}
					bool flag10 = haveNotChange;
					if (flag10)
					{
						result = false;
					}
					else
					{
						bool toolIsMeet = this.CheckTool(toolKey, (int)durabilityCost, (int)durabilityCost, -1);
						bool flag11 = !toolIsMeet;
						if (flag11)
						{
							result = false;
						}
						else
						{
							bool lifeSkillMeet = this.CheckLifeSkill(ref needLifeSkill, toolKey);
							bool flag12 = !lifeSkillMeet;
							result = !flag12;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007CF1 RID: 31985 RVA: 0x004A3D8C File Offset: 0x004A1F8C
		[DomainMethod]
		public ItemDisplayData WeaveClothingItem(DataContext context, ItemDisplayData tool, ItemDisplayData target, short weaveClothingTemplateId)
		{
			bool flag = tool.Key.IsValid();
			if (flag)
			{
				sbyte grade = ItemTemplateHelper.GetGrade(target.Key.ItemType, weaveClothingTemplateId);
				CraftToolItem toolConfig = Config.CraftTool.Instance[tool.Key.TemplateId];
				short cost = toolConfig.DurabilityCost[(int)grade];
				bool flag2 = cost > 0;
				if (flag2)
				{
					DomainManager.Item.ReduceToolDurability(context, DomainManager.Taiwu.GetTaiwuCharId(), tool.Key, (int)cost, tool.ItemSourceType);
				}
			}
			DomainManager.Extra.SetClothingDisplayModification(context, target.Key, weaveClothingTemplateId);
			ItemDisplayData result = target.Clone(-1);
			result.WeavedClothingTemplateId = weaveClothingTemplateId;
			return result;
		}

		// Token: 0x06007CF2 RID: 31986 RVA: 0x004A3E40 File Offset: 0x004A2040
		public ItemDisplayData ProfessionDoctorMakeMedicine(DataContext context, ItemDisplayData medicine, ItemDisplayData tool, int makeCount)
		{
			int needItemCount = makeCount * 3;
			Tester.Assert(medicine.Amount >= makeCount, "");
			short targetTemplateId;
			bool canMedicineUpgrade = ItemTemplateHelper.CanMedicineUpgrade(medicine.Key.ItemType, medicine.Key.TemplateId, out targetTemplateId);
			Tester.Assert(canMedicineUpgrade, "");
			List<ItemKey> keyList = medicine.GetOperationKeyListFromPool(needItemCount, false);
			foreach (ItemKey key in keyList)
			{
				DomainManager.Taiwu.RemoveItem(context, key, 1, medicine.ItemSourceTypeEnum, true, false);
			}
			ItemDisplayData.ReturnItemKeyListToPool(keyList);
			bool flag = tool.Key.IsValid();
			if (flag)
			{
				sbyte grade = ItemTemplateHelper.GetGrade(medicine.Key.ItemType, medicine.Key.TemplateId);
				CraftToolItem toolConfig = Config.CraftTool.Instance[tool.Key.TemplateId];
				int cost = (int)toolConfig.DurabilityCost[(int)grade] * makeCount;
				DomainManager.Item.ReduceToolDurability(context, DomainManager.Taiwu.GetTaiwuCharId(), tool.Key, cost, tool.ItemSourceType);
			}
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			Location location = taiwuChar.GetLocation();
			bool isInSettlement = false;
			bool flag2 = location == taiwuVillageLocation;
			if (flag2)
			{
				isInSettlement = true;
			}
			else
			{
				MapBlockData blockData = DomainManager.Map.GetBlock(location);
				bool flag3 = blockData.IsCityTown();
				if (flag3)
				{
					isInSettlement = true;
				}
			}
			ItemKey productItemKey = DomainManager.Item.CreateMedicine(context, targetTemplateId);
			bool flag4 = isInSettlement;
			if (flag4)
			{
				taiwuChar.AddInventoryItem(context, productItemKey, makeCount, false);
			}
			else
			{
				DomainManager.Taiwu.WarehouseAdd(context, productItemKey, makeCount);
			}
			bool flag5 = ItemTemplateHelper.GetItemSubType(productItemKey.ItemType, productItemKey.TemplateId) == 800;
			if (flag5)
			{
				sbyte grade2 = ItemTemplateHelper.GetGrade(productItemKey.ItemType, productItemKey.TemplateId);
				int seniority = ProfessionFormulaImpl.Calculate(83, (int)grade2);
				DomainManager.Extra.ChangeProfessionSeniority(context, 13, seniority * makeCount, true, false);
			}
			ItemDisplayData itemData = DomainManager.Item.GetItemDisplayData(productItemKey, -1);
			itemData.Amount = makeCount;
			return itemData;
		}

		// Token: 0x06007CF3 RID: 31987 RVA: 0x004A4084 File Offset: 0x004A2284
		[DomainMethod]
		[return: TupleElementNames(new string[]
		{
			"attainmentEffect",
			"upgradeMakeItem"
		})]
		public ValueTuple<int, bool> GetBuildingEffectForMake(BuildingBlockKey buildingBlockKey, sbyte skillType)
		{
			bool upgradeMakeItem = false;
			BuildingAreaData areaData = this.GetElement_BuildingAreas(buildingBlockKey.GetLocation());
			int attainmentEffect = 0;
			List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
			List<int> neighborDistanceList = ObjectPool<List<int>>.Instance.Get();
			sbyte buildingWidth = BuildingBlock.Instance[this._buildingBlocks[buildingBlockKey].TemplateId].Width;
			areaData.GetNeighborBlocks(buildingBlockKey.BuildingBlockIndex, buildingWidth, neighborList, neighborDistanceList, 2);
			foreach (short blockIndex in neighborList)
			{
				BuildingBlockKey neighborKey = new BuildingBlockKey(buildingBlockKey.AreaId, buildingBlockKey.BlockId, blockIndex);
				BuildingBlockData neighborBlock = this._buildingBlocks[neighborKey];
				bool flag = neighborBlock.RootBlockIndex >= 0;
				if (flag)
				{
					neighborKey = new BuildingBlockKey(buildingBlockKey.AreaId, buildingBlockKey.BlockId, (short)((byte)neighborBlock.RootBlockIndex));
					neighborBlock = this._buildingBlocks[neighborKey];
				}
				bool flag2 = neighborBlock.TemplateId != 0 && neighborBlock.CanUse();
				if (flag2)
				{
					sbyte level = this.BuildingBlockLevel(neighborKey);
					BuildingBlockItem neighborConfig = BuildingBlock.Instance[neighborBlock.TemplateId];
					bool flag3 = neighborConfig.ReduceMakeRequirementLifeSkillType == skillType;
					if (flag3)
					{
						foreach (short scaleId in neighborConfig.ExpandInfos)
						{
							BuildingScaleItem scaleCfg = BuildingScale.Instance[scaleId];
							bool flag4 = scaleCfg != null && scaleCfg.LifeSkillType == skillType && scaleCfg.Effect == EBuildingScaleEffect.MakeItemAttainmentRequirementReduction;
							if (flag4)
							{
								attainmentEffect += scaleCfg.GetLevelEffect((int)level);
							}
						}
					}
					bool flag5 = neighborConfig.RequireLifeSkillType == skillType && neighborConfig.UpgradeMakeItem;
					if (flag5)
					{
						upgradeMakeItem = true;
					}
				}
			}
			ObjectPool<List<short>>.Instance.Return(neighborList);
			ObjectPool<List<int>>.Instance.Return(neighborDistanceList);
			return new ValueTuple<int, bool>(attainmentEffect, upgradeMakeItem);
		}

		// Token: 0x06007CF4 RID: 31988 RVA: 0x004A42BC File Offset: 0x004A24BC
		private void ChangeItem(DataContext context, ItemKey oldKey, sbyte itemSourceType, ItemKey newKey, int charId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			ItemKey[] equipment = character.GetEquipment();
			sbyte destSlot = (sbyte)equipment.IndexOf(oldKey);
			int planIndex;
			int slotIndex;
			bool isInPlan = DomainManager.Taiwu.FindItemInEquipmentPlan(oldKey, out planIndex, out slotIndex);
			sbyte legendaryBookWeaponSlot = DomainManager.Extra.GetLegendaryBookWeaponSlotByItemKey(oldKey);
			bool isInLegendaryBook = legendaryBookWeaponSlot >= 0;
			bool deleteItem = newKey.Id != oldKey.Id;
			DomainManager.Taiwu.RemoveItem(context, oldKey, 1, itemSourceType, deleteItem, false);
			DomainManager.Taiwu.AddItem(context, newKey, 1, itemSourceType, false);
			bool flag = !deleteItem;
			if (flag)
			{
				bool flag2 = destSlot > -1;
				if (flag2)
				{
					character.ChangeEquipment(context, -1, destSlot, newKey);
				}
				bool flag3 = isInPlan;
				if (flag3)
				{
					DomainManager.Taiwu.SetEquipmentPlan(context, newKey, planIndex, slotIndex);
				}
				bool flag4 = isInLegendaryBook;
				if (flag4)
				{
					DomainManager.Extra.SetLegendaryBookWeaponSlot(context, legendaryBookWeaponSlot, newKey);
				}
			}
		}

		// Token: 0x06007CF5 RID: 31989 RVA: 0x004A439C File Offset: 0x004A259C
		private short GetLifeSkillTotalAttainment(sbyte type, ItemKey toolKey)
		{
			int attainment = (int)DomainManager.Taiwu.GetTaiwu().GetLifeSkillAttainment(type);
			bool flag = ItemTemplateHelper.IsEmptyTool(toolKey.ItemType, toolKey.TemplateId);
			if (flag)
			{
				bool flag2 = type - 6 <= 1 || type - 10 <= 1;
				bool skillTypeIsMeet = flag2;
				bool flag3 = skillTypeIsMeet && DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(6);
				int percent;
				if (flag3)
				{
					ProfessionData professionData = DomainManager.Extra.GetProfessionData(2);
					percent = professionData.GetSeniorityEmptyToolAttainmentBonus();
				}
				else
				{
					percent = ProfessionData.SeniorityToEmptyToolAttainmentBonus(0);
				}
				int toolAttainment = attainment * percent / 100;
				attainment += toolAttainment;
			}
			else
			{
				GameData.Domains.Item.CraftTool toolItem;
				bool flag4 = toolKey.IsValid() && DomainManager.Item.TryGetElement_CraftTools(toolKey.Id, out toolItem);
				if (flag4)
				{
					CraftToolItem toolConfig = Config.CraftTool.Instance[toolKey.TemplateId];
					bool flag5 = toolConfig != null && toolConfig.RequiredLifeSkillTypes.Contains(type);
					if (flag5)
					{
						attainment += (int)toolItem.GetRealAttainmentBonus();
					}
				}
			}
			attainment = Math.Max(0, attainment);
			return (short)attainment;
		}

		// Token: 0x06007CF6 RID: 31990 RVA: 0x004A44A4 File Offset: 0x004A26A4
		private bool CheckTool(ItemKey toolKey, int totalCost, int oneCost, sbyte skillType = -1)
		{
			bool flag = !toolKey.IsValid() || ItemTemplateHelper.IsEmptyTool(toolKey.ItemType, toolKey.TemplateId);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				GameData.Domains.Item.CraftTool tool = DomainManager.Item.GetElement_CraftTools(toolKey.Id);
				short durability = tool.GetCurrDurability();
				bool toolIsMeet = (int)durability >= totalCost || (int)durability + oneCost > totalCost;
				bool flag2 = skillType > -1;
				if (flag2)
				{
					toolIsMeet = (toolIsMeet && tool.GetRequiredLifeSkillTypes().Contains(skillType));
				}
				result = toolIsMeet;
			}
			return result;
		}

		// Token: 0x06007CF7 RID: 31991 RVA: 0x004A4524 File Offset: 0x004A2724
		public unsafe bool CheckLifeSkill(ref LifeSkillShorts need, ItemKey toolKey)
		{
			for (sbyte i = 0; i < 8; i += 1)
			{
				short totalAttainment = this.GetLifeSkillTotalAttainment(i, toolKey);
				bool flag = totalAttainment < *(ref need.Items.FixedElementField + (IntPtr)i * 2);
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007CF8 RID: 31992 RVA: 0x004A4574 File Offset: 0x004A2774
		public bool CheckLifeSkill(sbyte lifeSkillType, int requireAttainment, ItemKey toolKey)
		{
			return (int)this.GetLifeSkillTotalAttainment(lifeSkillType, toolKey) >= requireAttainment;
		}

		// Token: 0x06007CF9 RID: 31993 RVA: 0x004A4598 File Offset: 0x004A2798
		public short GetLifeSkillTotalAttainment(int charId, sbyte type, ItemKey toolKey)
		{
			bool flag = charId == DomainManager.Taiwu.GetTaiwuCharId();
			short result;
			if (flag)
			{
				result = this.GetLifeSkillTotalAttainment(type, toolKey);
			}
			else
			{
				short attainment = DomainManager.Character.GetElement_Objects(charId).GetLifeSkillAttainment(type);
				GameData.Domains.Item.CraftTool toolItem;
				bool flag2 = toolKey.IsValid() && DomainManager.Item.TryGetElement_CraftTools(toolKey.Id, out toolItem);
				if (flag2)
				{
					CraftToolItem toolConfig = Config.CraftTool.Instance[toolKey.TemplateId];
					bool flag3 = toolConfig != null && toolConfig.RequiredLifeSkillTypes.Contains(type);
					if (flag3)
					{
						attainment += toolItem.GetAttainmentBonus();
					}
				}
				attainment = Math.Max(0, attainment);
				result = attainment;
			}
			return result;
		}

		// Token: 0x06007CFA RID: 31994 RVA: 0x004A4640 File Offset: 0x004A2840
		[DomainMethod]
		public ValueTuple<short, BuildingBlockData> Build(DataContext context, BuildingBlockKey blockKey, short buildingTemplateId, int[] workers)
		{
			bool flag = !this.CanBuild(blockKey, buildingTemplateId);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Can not build building ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(buildingTemplateId);
				defaultInterpolatedStringHandler.AppendLiteral(" on block ");
				defaultInterpolatedStringHandler.AppendFormatted<BuildingBlockKey>(blockKey);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.SetVillageBuildWork(context, blockKey, workers);
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			BuildingBlockItem configData = BuildingBlock.Instance[buildingTemplateId];
			bool flag2 = BuildingBlockData.CanUpgrade(configData.Type) && GameData.Domains.Building.SharedMethods.NeedCostResourceToBuild(configData);
			if (flag2)
			{
				for (sbyte type = 0; type < 8; type += 1)
				{
					bool flag3 = configData.BaseBuildCost[(int)type] > 0;
					if (flag3)
					{
						this.ConsumeResource(context, type, (int)configData.BaseBuildCost[(int)type]);
					}
				}
			}
			this.CostBuildingCore(context, configData);
			MapBlockItem mapBlockData = MapBlock.Instance[DomainManager.Map.GetBlock(blockKey.AreaId, blockKey.BlockId).TemplateId];
			sbyte areaWidth = mapBlockData.BuildingAreaWidth;
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			blockData.TemplateId = buildingTemplateId;
			blockData.OfflineResetShopProgress();
			blockData.OperationType = 0;
			blockData.OperationProgress = 0;
			blockData.OperationStopping = false;
			this.PlaceBuilding(context, blockKey.AreaId, blockKey.BlockId, blockKey.BuildingBlockIndex, blockData, areaWidth);
			return new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, blockData);
		}

		// Token: 0x06007CFB RID: 31995 RVA: 0x004A47B8 File Offset: 0x004A29B8
		private void CostBuildingCore(DataContext context, BuildingBlockItem configData)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool flag = configData.BuildingCoreItem != -1;
			if (flag)
			{
				ItemKey tempItem = ItemKey.Invalid;
				foreach (ItemKey item in DomainManager.Taiwu.WarehouseItems.Keys)
				{
					bool flag2 = item.ItemType == 12 && item.TemplateId == configData.BuildingCoreItem;
					if (flag2)
					{
						tempItem = item;
						break;
					}
				}
				bool flag3 = !tempItem.Equals(ItemKey.Invalid);
				if (flag3)
				{
					DomainManager.Taiwu.WarehouseRemove(context, tempItem, 1, false);
				}
				else
				{
					Inventory inventory = taiwuChar.GetInventory();
					foreach (ItemKey item2 in inventory.Items.Keys)
					{
						bool flag4 = item2.ItemType == 12 && item2.TemplateId == configData.BuildingCoreItem;
						if (flag4)
						{
							taiwuChar.RemoveInventoryItem(context, item2, 1, true, false);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06007CFC RID: 31996 RVA: 0x004A4910 File Offset: 0x004A2B10
		private int ClacBuildingTimeCost(int[] workers, BuildingBlockItem configData, sbyte buildingOperationType)
		{
			int speed = 0;
			foreach (int charId in workers)
			{
				bool flag = charId >= 0;
				if (flag)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					speed += (int)(character.GetLifeSkillAttainment(configData.RequireLifeSkillType) + DomainManager.Building.BaseWorkContribution);
				}
			}
			int totalProgress = (int)configData.OperationTotalProgress[(int)buildingOperationType];
			return Convert.ToInt32(Math.Ceiling((double)((float)totalProgress / (float)speed)));
		}

		// Token: 0x06007CFD RID: 31997 RVA: 0x004A4998 File Offset: 0x004A2B98
		[Obsolete]
		public bool UpgradeIsMeetDependency(BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			sbyte minLevel;
			bool dependBuildingAvailable = this.AllDependBuildingAvailable(blockKey, blockData.TemplateId, out minLevel);
			bool dependencyIsNotMeet = !dependBuildingAvailable || this.BuildingBlockLevel(blockKey) >= minLevel;
			return !dependencyIsNotMeet;
		}

		// Token: 0x06007CFE RID: 31998 RVA: 0x004A49DC File Offset: 0x004A2BDC
		[Obsolete]
		public bool UpgradeIsHaveEnoughResource(BuildingBlockData blockData, bool checkResourceChanges = false)
		{
			return false;
		}

		// Token: 0x06007CFF RID: 31999 RVA: 0x004A49DF File Offset: 0x004A2BDF
		[Obsolete]
		public bool HaveEnoughResourceToExpandBuilding(BuildingBlockData blockData, int[] resourceChanges, bool isChangeSourceData = false)
		{
			return false;
		}

		// Token: 0x06007D00 RID: 32000 RVA: 0x004A49E4 File Offset: 0x004A2BE4
		[Obsolete]
		[DomainMethod]
		public ValueTuple<short, BuildingBlockData> Upgrade(DataContext context, BuildingBlockKey blockKey, int[] workers)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			bool flag2;
			bool flag = !this.CanUpgrade(blockKey, out flag2) || !this.UpgradeIsHaveEnoughResource(blockData, false);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Can not upgrade building on block ");
				defaultInterpolatedStringHandler.AppendFormatted<BuildingBlockKey>(blockKey);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag3 = this._buildingOperatorDict.ContainsKey(blockKey);
			if (flag3)
			{
				List<int> operators = this._buildingOperatorDict[blockKey].GetCollection();
				for (int i = 0; i < operators.Count; i++)
				{
					bool flag4 = operators[i] >= 0;
					if (flag4)
					{
						DomainManager.Taiwu.RemoveVillagerWork(context, operators[i], true);
					}
				}
			}
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			bool flag5 = configData.Type == EBuildingBlockType.NormalResource;
			if (flag5)
			{
				this.CostBuildingCore(context, configData);
			}
			this.SetVillageBuildWorkAndBlockData(context, blockKey, blockData, workers, 2);
			this._buildingAutoExpandStoppedNotifiedSet.Remove(blockKey);
			return new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, blockData);
		}

		// Token: 0x06007D01 RID: 32001 RVA: 0x004A4B0C File Offset: 0x004A2D0C
		[DomainMethod]
		public ValueTuple<short, BuildingBlockData> Remove(DataContext context, BuildingBlockKey blockKey, int[] workers)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			bool flag = (configData.Type != EBuildingBlockType.Building && !BuildingBlockData.IsResource(configData.Type)) || blockData.OperationType != -1 || configData.Class == EBuildingBlockClass.Static || configData.OperationTotalProgress[1] < 0;
			ValueTuple<short, BuildingBlockData> result;
			if (flag)
			{
				result = new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, blockData);
			}
			else
			{
				this.SetVillageBuildWorkAndBlockData(context, blockKey, blockData, workers, 1);
				result = new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, blockData);
			}
			return result;
		}

		// Token: 0x06007D02 RID: 32002 RVA: 0x004A4B98 File Offset: 0x004A2D98
		[Obsolete]
		[DomainMethod]
		public ValueTuple<short, BuildingBlockData> Collect(DataContext context, BuildingBlockKey blockKey, int[] workers)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			bool flag = !BuildingBlockData.IsResource(configData.Type) || blockData.OperationType != -1 || configData.OperationTotalProgress[1] < 0;
			ValueTuple<short, BuildingBlockData> result;
			if (flag)
			{
				result = new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, blockData);
			}
			else
			{
				this.SetVillageBuildWorkAndBlockData(context, blockKey, blockData, workers, 3);
				result = new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, blockData);
			}
			return result;
		}

		// Token: 0x06007D03 RID: 32003 RVA: 0x004A4C12 File Offset: 0x004A2E12
		private void SetVillageBuildWorkAndBlockData(DataContext context, BuildingBlockKey blockKey, BuildingBlockData blockData, int[] workers, sbyte buildingOperationType)
		{
			this.SetVillageBuildWork(context, blockKey, workers);
			blockData.OperationType = buildingOperationType;
			blockData.OperationProgress = 0;
			blockData.OperationStopping = false;
			this.SetElement_BuildingBlocks(blockKey, blockData, context);
		}

		// Token: 0x06007D04 RID: 32004 RVA: 0x004A4C40 File Offset: 0x004A2E40
		private void SetVillageBuildWork(DataContext context, BuildingBlockKey blockKey, int[] workers)
		{
			for (int i = 0; i < workers.Length; i++)
			{
				int workerCharId = workers[i];
				bool flag = workerCharId >= 0;
				if (flag)
				{
					VillagerWorkData workData = new VillagerWorkData(workerCharId, 0, blockKey.AreaId, blockKey.BlockId);
					workData.BuildingBlockIndex = blockKey.BuildingBlockIndex;
					workData.WorkerIndex = (sbyte)i;
					DomainManager.Taiwu.SetVillagerWork(context, workerCharId, workData, false);
				}
			}
		}

		// Token: 0x06007D05 RID: 32005 RVA: 0x004A4CAC File Offset: 0x004A2EAC
		[DomainMethod]
		public ValueTuple<short, BuildingBlockData> SetStopOperation(DataContext context, BuildingBlockKey blockKey, bool stop)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			bool flag = stop && (blockData.OperationProgress == 0 || blockData.OperationType == 0);
			if (flag)
			{
				BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
				GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
				bool flag2 = blockData.OperationType == 0;
				if (flag2)
				{
					this.ResetAllChildrenBlocks(context, blockKey, 0, -1);
					bool flag3 = GameData.Domains.Building.SharedMethods.NeedCostResourceToBuild(configData);
					if (flag3)
					{
						for (sbyte type = 0; type < 8; type += 1)
						{
							taiwuChar.ChangeResource(context, type, (int)configData.BaseBuildCost[(int)type]);
						}
					}
					bool flag4 = configData.BuildingCoreItem != -1;
					if (flag4)
					{
						DomainManager.Taiwu.ReturnBuildingCoreItem(DataContextManager.GetCurrentThreadDataContext(), configData);
					}
					this.SetBuildingCustomName(context, blockKey, null);
				}
				blockData.OperationType = -1;
				this.SetElement_BuildingBlocks(blockKey, blockData, context);
				bool flag5 = this._buildingOperatorDict.ContainsKey(blockKey);
				if (flag5)
				{
					List<int> operators = this._buildingOperatorDict[blockKey].GetCollection();
					for (int i = 0; i < operators.Count; i++)
					{
						bool flag6 = operators[i] >= 0;
						if (flag6)
						{
							DomainManager.Taiwu.RemoveVillagerWork(context, operators[i], true);
						}
					}
				}
			}
			else
			{
				blockData.OperationStopping = stop;
			}
			this.SetElement_BuildingBlocks(blockKey, blockData, context);
			return new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, blockData);
		}

		// Token: 0x06007D06 RID: 32006 RVA: 0x004A4E30 File Offset: 0x004A3030
		[DomainMethod]
		public void SetOperator(DataContext context, BuildingBlockKey blockKey, sbyte index, int charId)
		{
			bool flag = this._buildingOperatorDict.ContainsKey(blockKey);
			if (flag)
			{
				int currCharId = this._buildingOperatorDict[blockKey].GetCollection()[(int)index];
				bool flag2 = currCharId == charId;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Same operator already exist, id: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = currCharId >= 0;
				if (flag3)
				{
					DomainManager.Taiwu.RemoveVillagerWork(context, currCharId, true);
				}
			}
			bool flag4 = charId >= 0;
			if (flag4)
			{
				VillagerWorkData workData = new VillagerWorkData(charId, 0, blockKey.AreaId, blockKey.BlockId);
				workData.BuildingBlockIndex = blockKey.BuildingBlockIndex;
				workData.WorkerIndex = index;
				DomainManager.Taiwu.SetVillagerWork(context, charId, workData, false);
			}
		}

		// Token: 0x06007D07 RID: 32007 RVA: 0x004A4F0C File Offset: 0x004A310C
		[DomainMethod]
		[Obsolete]
		public ValueTuple<short, BuildingBlockData> SetBuildingMaintenance(DataContext context, BuildingBlockKey blockKey, bool maintenance)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			bool flag = !maintenance && configData.MustMaintenance;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Building ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(blockData.TemplateId);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(configData.Name);
				defaultInterpolatedStringHandler.AppendLiteral(" must maintenance");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag2 = configData.BaseMaintenanceCost.Count > 0;
			if (flag2)
			{
				blockData.Maintenance = maintenance;
				this.SetElement_BuildingBlocks(blockKey, blockData, context);
			}
			return new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, blockData);
		}

		// Token: 0x06007D08 RID: 32008 RVA: 0x004A4FD8 File Offset: 0x004A31D8
		[DomainMethod]
		public ValueTuple<short, BuildingBlockData> Repair(DataContext context, BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			bool flag = blockData.Durability == configData.MaxDurability;
			ValueTuple<short, BuildingBlockData> result;
			if (flag)
			{
				result = new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, blockData);
			}
			else
			{
				ResourceInts resource = this.GetAllTaiwuResources();
				int cost = GameData.Domains.Building.SharedMethods.CalcRepairBuildingCost(blockData, configData);
				bool enough = resource.Get(6) >= cost;
				bool flag2 = enough;
				if (flag2)
				{
					this.ConsumeResource(context, 6, cost);
					blockData.Durability = configData.MaxDurability;
					this.SetElement_BuildingBlocks(blockKey, blockData, context);
				}
				result = new ValueTuple<short, BuildingBlockData>(blockKey.BuildingBlockIndex, blockData);
			}
			return result;
		}

		// Token: 0x06007D09 RID: 32009 RVA: 0x004A5080 File Offset: 0x004A3280
		[DomainMethod]
		public void ConfirmPlanBuilding(DataContext context, List<IntPair> operateRecord, Location location)
		{
			bool flag = operateRecord == null;
			if (!flag)
			{
				HashSet<BuildingBlockKey> downgradedSet = new HashSet<BuildingBlockKey>();
				foreach (IntPair element in operateRecord)
				{
					this.PlanResetBuilding(context, new BuildingBlockKey(location.AreaId, location.BlockId, (short)element.First), new BuildingBlockKey(location.AreaId, location.BlockId, (short)element.Second), downgradedSet);
				}
			}
		}

		// Token: 0x06007D0A RID: 32010 RVA: 0x004A5118 File Offset: 0x004A3318
		public void PlanResetBuilding(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey, HashSet<BuildingBlockKey> downgradedSet)
		{
			this.ExchangeBlockData(context, originalBlockKey, nowBlockKey, downgradedSet);
			this.ExchangeBlockDataEx(context, originalBlockKey, nowBlockKey);
			this.ExchangeCollectBuildingResourceType(context, originalBlockKey, nowBlockKey);
			this.ExchangeBuildingOperator(context, originalBlockKey, nowBlockKey);
			this.ExchangeCustomBuildingName(context, originalBlockKey, nowBlockKey);
			this.ExchangeCollectBuildingEarningsData(context, originalBlockKey, nowBlockKey);
			this.ExchangeShopManager(context, originalBlockKey, nowBlockKey);
			this.ExchangeShopEvent(context, originalBlockKey, nowBlockKey);
			this.ExchangeMakeItem(context, originalBlockKey, nowBlockKey);
			this.ExchangeResident(context, originalBlockKey, nowBlockKey);
			this.ExchangeComfortableHouses(context, originalBlockKey, nowBlockKey);
			this.ExchangeBuildingResident(context, originalBlockKey, nowBlockKey);
			this.ExchangeBuildingComfortableHouse(context, originalBlockKey, nowBlockKey);
			this.ExchangeAutoWorkList(context, originalBlockKey, nowBlockKey);
			this.ExchangeAutoSoldList(context, originalBlockKey, nowBlockKey);
			this.ExchangeAutoExpandList(context, originalBlockKey, nowBlockKey);
			this.ExchangeShopArrangeResultFirstList(context, originalBlockKey, nowBlockKey);
			this.ExchangeAutoCheckComfortableInList(context, originalBlockKey, nowBlockKey);
			this.ExchangeAutoCheckResidenceInList(context, originalBlockKey, nowBlockKey);
			this.ExchangeExtraBlockData(context, originalBlockKey, nowBlockKey);
			this.UpdateTaiwuVillageBuildingEffect();
		}

		// Token: 0x06007D0B RID: 32011 RVA: 0x004A51F7 File Offset: 0x004A33F7
		private void ExchangeExtraBlockData(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			DomainManager.Extra.ExchangeBuildingArtisanOrder(context, originalBlockKey, nowBlockKey);
			DomainManager.Extra.ExchangeResourceBlockExtraData(context, originalBlockKey, nowBlockKey);
			DomainManager.Extra.ExchangeFeast(context, originalBlockKey, nowBlockKey);
		}

		// Token: 0x06007D0C RID: 32012 RVA: 0x004A5224 File Offset: 0x004A3424
		private void ExchangeAutoCheckComfortableInList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			List<short> autoCheckInComfortableList = DomainManager.Extra.GetAutoCheckInComfortableList();
			bool flag = !autoCheckInComfortableList.Contains(originalBlockKey.BuildingBlockIndex);
			if (!flag)
			{
				autoCheckInComfortableList.Remove(originalBlockKey.BuildingBlockIndex);
				bool flag2 = !autoCheckInComfortableList.Contains(nowBlockKey.BuildingBlockIndex);
				if (flag2)
				{
					autoCheckInComfortableList.Add(nowBlockKey.BuildingBlockIndex);
				}
				DomainManager.Extra.SetAutoCheckInComfortableList(autoCheckInComfortableList, context);
			}
		}

		// Token: 0x06007D0D RID: 32013 RVA: 0x004A528C File Offset: 0x004A348C
		private void ExchangeAutoCheckResidenceInList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			List<short> autoCheckInResidenceList = DomainManager.Extra.GetAutoCheckInResidenceList();
			bool flag = !autoCheckInResidenceList.Contains(originalBlockKey.BuildingBlockIndex);
			if (!flag)
			{
				autoCheckInResidenceList.Remove(originalBlockKey.BuildingBlockIndex);
				bool flag2 = !autoCheckInResidenceList.Contains(nowBlockKey.BuildingBlockIndex);
				if (flag2)
				{
					autoCheckInResidenceList.Add(nowBlockKey.BuildingBlockIndex);
				}
				DomainManager.Extra.SetAutoCheckInResidenceList(autoCheckInResidenceList, context);
			}
		}

		// Token: 0x06007D0E RID: 32014 RVA: 0x004A52F4 File Offset: 0x004A34F4
		private void ExchangeShopArrangeResultFirstList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			List<short> resultFirstList = DomainManager.Extra.GetShopArrangeResultFirstList();
			bool flag = !resultFirstList.Contains(originalBlockKey.BuildingBlockIndex);
			if (!flag)
			{
				resultFirstList.Remove(originalBlockKey.BuildingBlockIndex);
				bool flag2 = !resultFirstList.Contains(nowBlockKey.BuildingBlockIndex);
				if (flag2)
				{
					resultFirstList.Add(nowBlockKey.BuildingBlockIndex);
				}
				DomainManager.Extra.SetShopArrangeResultFirstList(resultFirstList, context);
			}
		}

		// Token: 0x06007D0F RID: 32015 RVA: 0x004A535C File Offset: 0x004A355C
		private void ExchangeAutoExpandList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			List<short> autoExpandList = DomainManager.Extra.GetAutoExpandBlockIndexList();
			bool flag = !autoExpandList.Contains(originalBlockKey.BuildingBlockIndex);
			if (!flag)
			{
				int index = autoExpandList.IndexOf(originalBlockKey.BuildingBlockIndex);
				autoExpandList.Remove(originalBlockKey.BuildingBlockIndex);
				bool flag2 = !autoExpandList.Contains(nowBlockKey.BuildingBlockIndex);
				if (flag2)
				{
					autoExpandList.Insert(index, nowBlockKey.BuildingBlockIndex);
				}
				DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandList, context);
			}
		}

		// Token: 0x06007D10 RID: 32016 RVA: 0x004A53D4 File Offset: 0x004A35D4
		private void ExchangeAutoWorkList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			List<short> autoWorkList = DomainManager.Extra.GetAutoWorkBlockIndexList();
			bool flag = !autoWorkList.Contains(originalBlockKey.BuildingBlockIndex);
			if (!flag)
			{
				autoWorkList.Remove(originalBlockKey.BuildingBlockIndex);
				bool flag2 = !autoWorkList.Contains(nowBlockKey.BuildingBlockIndex);
				if (flag2)
				{
					autoWorkList.Add(nowBlockKey.BuildingBlockIndex);
				}
				DomainManager.Extra.SetAutoWorkBlockIndexList(autoWorkList, context);
			}
		}

		// Token: 0x06007D11 RID: 32017 RVA: 0x004A543C File Offset: 0x004A363C
		private void ExchangeAutoSoldList(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			List<short> autoSoldList = DomainManager.Extra.GetAutoSoldBlockIndexList();
			bool flag = !autoSoldList.Contains(originalBlockKey.BuildingBlockIndex);
			if (!flag)
			{
				autoSoldList.Remove(originalBlockKey.BuildingBlockIndex);
				bool flag2 = !autoSoldList.Contains(nowBlockKey.BuildingBlockIndex);
				if (flag2)
				{
					autoSoldList.Add(nowBlockKey.BuildingBlockIndex);
				}
				DomainManager.Extra.SetAutoSoldBlockIndexList(autoSoldList, context);
			}
		}

		// Token: 0x06007D12 RID: 32018 RVA: 0x004A54A4 File Offset: 0x004A36A4
		private void ExchangeBuildingResident(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			bool flag = this._buildingResidents == null;
			if (!flag)
			{
				List<int> idList = new List<int>();
				foreach (KeyValuePair<int, BuildingBlockKey> pair in this._buildingResidents)
				{
					bool flag2 = pair.Value.Equals(originalBlockKey);
					if (flag2)
					{
						idList.Add(pair.Key);
					}
				}
				for (int i = 0; i < idList.Count; i++)
				{
					this._buildingResidents[idList[i]] = nowBlockKey;
				}
			}
		}

		// Token: 0x06007D13 RID: 32019 RVA: 0x004A5568 File Offset: 0x004A3768
		private void ExchangeBuildingComfortableHouse(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			bool flag = this._buildingComfortableHouses == null;
			if (!flag)
			{
				List<int> idList = new List<int>();
				foreach (KeyValuePair<int, BuildingBlockKey> pair in this._buildingComfortableHouses)
				{
					bool flag2 = pair.Value.Equals(originalBlockKey);
					if (flag2)
					{
						idList.Add(pair.Key);
					}
				}
				for (int i = 0; i < idList.Count; i++)
				{
					this._buildingComfortableHouses[idList[i]] = nowBlockKey;
				}
			}
		}

		// Token: 0x06007D14 RID: 32020 RVA: 0x004A562C File Offset: 0x004A382C
		private void ExchangeComfortableHouses(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			CharacterList charListOri;
			bool isHaveOri = this.TryGetElement_ComfortableHouses(originalBlockKey, out charListOri);
			bool flag = !isHaveOri;
			if (!flag)
			{
				CharacterList charListNow;
				bool isHaveNow = this.TryGetElement_ComfortableHouses(nowBlockKey, out charListNow);
				bool flag2 = !isHaveNow;
				if (flag2)
				{
					this.AddElement_ComfortableHouses(nowBlockKey, charListOri, context);
				}
				else
				{
					this.SetElement_ComfortableHouses(nowBlockKey, charListOri, context);
				}
				this.RemoveElement_ComfortableHouses(originalBlockKey, context);
			}
		}

		// Token: 0x06007D15 RID: 32021 RVA: 0x004A5688 File Offset: 0x004A3888
		private void ExchangeResident(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			CharacterList charListOri;
			bool isHaveOri = this.TryGetElement_Residences(originalBlockKey, out charListOri);
			bool flag = !isHaveOri;
			if (!flag)
			{
				CharacterList charListNow;
				bool isHaveNow = this.TryGetElement_Residences(nowBlockKey, out charListNow);
				bool flag2 = !isHaveNow;
				if (flag2)
				{
					this.AddElement_Residences(nowBlockKey, charListOri, context);
				}
				else
				{
					this.SetElement_Residences(nowBlockKey, charListOri, context);
				}
				this.RemoveElement_Residences(originalBlockKey, context);
			}
		}

		// Token: 0x06007D16 RID: 32022 RVA: 0x004A56E4 File Offset: 0x004A38E4
		private void ExchangeMakeItem(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			MakeItemData itemDataOri;
			bool isHaveOri = this.TryGetElement_MakeItemDict(originalBlockKey, out itemDataOri);
			bool flag = !isHaveOri;
			if (!flag)
			{
				MakeItemData itemDataNow;
				bool isHaveNow = this.TryGetElement_MakeItemDict(nowBlockKey, out itemDataNow);
				bool flag2 = !isHaveNow;
				if (flag2)
				{
					this.AddElement_MakeItemDict(nowBlockKey, itemDataOri, context);
				}
				else
				{
					this.SetElement_MakeItemDict(nowBlockKey, itemDataOri, context);
				}
				this.RemoveElement_MakeItemDict(originalBlockKey, context);
			}
		}

		// Token: 0x06007D17 RID: 32023 RVA: 0x004A5740 File Offset: 0x004A3940
		private void ExchangeShopEvent(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			bool flag = this._shopEventCollections == null;
			if (!flag)
			{
				ShopEventCollection originalCollection;
				bool flag2 = !this._shopEventCollections.Remove(originalBlockKey, out originalCollection);
				if (!flag2)
				{
					this._shopEventCollections[nowBlockKey] = originalCollection;
				}
			}
		}

		// Token: 0x06007D18 RID: 32024 RVA: 0x004A5784 File Offset: 0x004A3984
		private void ExchangeShopManager(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			CharacterList charListOri;
			bool isHaveOri = this.TryGetElement_ShopManagerDict(originalBlockKey, out charListOri);
			bool flag = !isHaveOri;
			if (!flag)
			{
				CharacterList charListNow;
				bool isHaveNow = this.TryGetElement_ShopManagerDict(nowBlockKey, out charListNow);
				bool flag2 = !isHaveNow;
				if (flag2)
				{
					this.AddElement_ShopManagerDict(nowBlockKey, charListOri, context);
				}
				else
				{
					this.SetElement_ShopManagerDict(nowBlockKey, charListOri, context);
				}
				for (int i = 0; i < charListOri.GetCount(); i++)
				{
					bool flag3 = charListOri.GetCollection()[i] == -1;
					if (!flag3)
					{
						DomainManager.Taiwu.ChangeVillagerWorkBuildingBlockIndex(charListOri.GetCollection()[i], nowBlockKey.BuildingBlockIndex, context);
					}
				}
				this.RemoveElement_ShopManagerDict(originalBlockKey, context);
			}
		}

		// Token: 0x06007D19 RID: 32025 RVA: 0x004A583C File Offset: 0x004A3A3C
		private void ExchangeCollectBuildingEarningsData(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			BuildingEarningsData earnDataOri;
			bool isHaveOri = this.TryGetElement_CollectBuildingEarningsData(originalBlockKey, out earnDataOri);
			bool flag = !isHaveOri;
			if (!flag)
			{
				BuildingEarningsData earnDataNow;
				bool isHaveNow = this.TryGetElement_CollectBuildingEarningsData(nowBlockKey, out earnDataNow);
				bool flag2 = !isHaveNow;
				if (flag2)
				{
					this.AddElement_CollectBuildingEarningsData(nowBlockKey, earnDataOri, context);
				}
				else
				{
					this.SetElement_CollectBuildingEarningsData(nowBlockKey, earnDataOri, context);
				}
				this.RemoveElement_CollectBuildingEarningsData(originalBlockKey, context);
			}
		}

		// Token: 0x06007D1A RID: 32026 RVA: 0x004A5898 File Offset: 0x004A3A98
		private void ExchangeCustomBuildingName(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			int id;
			bool isHaveOri = this.TryGetElement_CustomBuildingName(originalBlockKey, out id);
			bool flag = !isHaveOri;
			if (!flag)
			{
				int id2;
				bool isHaveNow = this.TryGetElement_CustomBuildingName(nowBlockKey, out id2);
				bool flag2 = !isHaveNow;
				if (flag2)
				{
					this.AddElement_CustomBuildingName(nowBlockKey, id, context);
				}
				else
				{
					this.SetElement_CustomBuildingName(nowBlockKey, id, context);
				}
				this.RemoveElement_CustomBuildingName(originalBlockKey, context);
			}
		}

		// Token: 0x06007D1B RID: 32027 RVA: 0x004A58F4 File Offset: 0x004A3AF4
		private void ExchangeBuildingOperator(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			CharacterList charListOri;
			bool isHaveOri = this.TryGetElement_BuildingOperatorDict(originalBlockKey, out charListOri);
			bool flag = !isHaveOri;
			if (!flag)
			{
				CharacterList charListNow;
				bool isHaveNow = this.TryGetElement_BuildingOperatorDict(nowBlockKey, out charListNow);
				bool flag2 = !isHaveNow;
				if (flag2)
				{
					this.AddElement_BuildingOperatorDict(nowBlockKey, charListOri, context);
				}
				else
				{
					this.SetElement_BuildingOperatorDict(nowBlockKey, charListOri, context);
				}
				this.RemoveElement_BuildingOperatorDict(originalBlockKey, context);
				for (int i = 0; i < charListOri.GetCount(); i++)
				{
					bool flag3 = charListOri.GetCollection()[i] == -1;
					if (!flag3)
					{
						DomainManager.Taiwu.ChangeVillagerWorkBuildingBlockIndex(charListOri.GetCollection()[i], nowBlockKey.BuildingBlockIndex, context);
					}
				}
			}
		}

		// Token: 0x06007D1C RID: 32028 RVA: 0x004A59AC File Offset: 0x004A3BAC
		private void ExchangeCollectBuildingResourceType(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			sbyte typeOri;
			bool isHaveOri = this.TryGetElement_CollectBuildingResourceType(originalBlockKey, out typeOri);
			bool flag = !isHaveOri;
			if (!flag)
			{
				sbyte typeNow;
				bool isHaveNow = this.TryGetElement_CollectBuildingResourceType(nowBlockKey, out typeNow);
				bool flag2 = !isHaveNow;
				if (flag2)
				{
					this.AddElement_CollectBuildingResourceType(nowBlockKey, typeOri, context);
				}
				else
				{
					this.SetElement_CollectBuildingResourceType(nowBlockKey, typeOri, context);
				}
				this.RemoveElement_CollectBuildingResourceType(originalBlockKey, context);
			}
		}

		// Token: 0x06007D1D RID: 32029 RVA: 0x004A5A08 File Offset: 0x004A3C08
		private void ExchangeBlockData(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey, HashSet<BuildingBlockKey> downgradedSet)
		{
			MapBlockItem mapBlockData = MapBlock.Instance[DomainManager.Map.GetBlock(originalBlockKey.AreaId, originalBlockKey.BlockId).TemplateId];
			sbyte areaWidth = mapBlockData.BuildingAreaWidth;
			BuildingBlockData blockData = this.GetElement_BuildingBlocks(originalBlockKey).Clone();
			BuildingBlockItem blockConfig = BuildingBlock.Instance.GetItem(blockData.TemplateId);
			bool downgraded = downgradedSet.Contains(originalBlockKey);
			downgradedSet.Remove(originalBlockKey);
			downgradedSet.Add(nowBlockKey);
			bool flag = blockConfig.Class == EBuildingBlockClass.BornResource;
			if (flag)
			{
				bool flag2 = !downgraded;
				if (flag2)
				{
					bool flag3 = blockConfig.Type == EBuildingBlockType.UselessResource;
					if (flag3)
					{
						Tester.Assert(blockData.Level > 1, "");
						BuildingBlockData buildingBlockData = blockData;
						buildingBlockData.Level -= 1;
					}
					else
					{
						BuildingBlockDataEx dataExOri = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)originalBlockKey);
						sbyte level = dataExOri.CalcUnlockedLevelCount();
						Tester.Assert(level > 1, "");
						dataExOri.LevelUnlockedFlags >>= 1;
						DomainManager.Extra.SetBuildingBlockDataEx(context, originalBlockKey, dataExOri);
					}
				}
			}
			else
			{
				blockData.Durability = blockConfig.MaxDurability / 2;
			}
			for (int i = 0; i < (int)blockConfig.Width; i++)
			{
				for (int j = 0; j < (int)blockConfig.Width; j++)
				{
					short index = (short)((int)originalBlockKey.BuildingBlockIndex + i * (int)areaWidth + j);
					BuildingBlockKey key = new BuildingBlockKey(originalBlockKey.AreaId, originalBlockKey.BlockId, index);
					BuildingBlockData val = new BuildingBlockData(index, 0, -1, -1);
					this.SetElement_BuildingBlocks(key, val, context);
				}
			}
			for (int k = 0; k < (int)blockConfig.Width; k++)
			{
				for (int l = 0; l < (int)blockConfig.Width; l++)
				{
					short index2 = (short)((int)nowBlockKey.BuildingBlockIndex + k * (int)areaWidth + l);
					BuildingBlockKey key2 = new BuildingBlockKey(nowBlockKey.AreaId, nowBlockKey.BlockId, index2);
					BuildingBlockData val2 = (index2 == nowBlockKey.BuildingBlockIndex) ? blockData : new BuildingBlockData(index2, -1, -1, nowBlockKey.BuildingBlockIndex);
					val2.BlockIndex = index2;
					this.SetElement_BuildingBlocks(key2, val2, context);
				}
			}
		}

		// Token: 0x06007D1E RID: 32030 RVA: 0x004A5C44 File Offset: 0x004A3E44
		private void ExchangeBlockDataEx(DataContext context, BuildingBlockKey originalBlockKey, BuildingBlockKey nowBlockKey)
		{
			BuildingBlockDataEx dataExOri = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)originalBlockKey);
			BuildingBlockDataEx dataExNow = DomainManager.Extra.GetElement_BuildingBlockDataEx((ulong)nowBlockKey);
			BuildingBlockDataEx buildingBlockDataEx = dataExNow;
			BuildingBlockDataEx buildingBlockDataEx2 = dataExOri;
			BuildingBlockKey key = dataExOri.Key;
			BuildingBlockKey key2 = dataExNow.Key;
			buildingBlockDataEx.Key = key;
			buildingBlockDataEx2.Key = key2;
			DomainManager.Extra.SetBuildingBlockDataEx(context, nowBlockKey, dataExOri);
			DomainManager.Extra.SetBuildingBlockDataEx(context, originalBlockKey, dataExNow);
		}

		// Token: 0x06007D1F RID: 32031 RVA: 0x004A5CB0 File Offset: 0x004A3EB0
		private void InitializeComfortableHouses(DataContext context, Location location)
		{
			List<BuildingBlockKey> comfortableHouses = this.FindAllBuildingsWithSameTemplate(location, this._buildingAreas[location], 47);
			foreach (BuildingBlockKey buildingBlockKey in comfortableHouses)
			{
				this.AddElement_ComfortableHouses(buildingBlockKey, default(CharacterList), context);
			}
		}

		// Token: 0x06007D20 RID: 32032 RVA: 0x004A5D24 File Offset: 0x004A3F24
		private void InitializeResidences(DataContext context, Location location)
		{
			List<BuildingBlockKey> residences = this.FindAllBuildingsWithSameTemplate(location, this._buildingAreas[location], 46);
			ExtraDomain extraDomain = DomainManager.Extra;
			foreach (BuildingBlockKey buildingBlockKey in residences)
			{
				BuildingBlockData block = this._buildingBlocks[buildingBlockKey];
				extraDomain.ModifyBuildingExtraData(context, buildingBlockKey, delegate(BuildingBlockDataEx blockDataEx)
				{
					for (int i = 0; i < 7; i++)
					{
						blockDataEx.LevelUnlockedFlags = BitOperation.SetBit(blockDataEx.LevelUnlockedFlags, i, true);
					}
				});
				this.SetElement_BuildingBlocks(buildingBlockKey, block, context);
				this.AddElement_Residences(buildingBlockKey, default(CharacterList), context);
			}
			this.UpdateHomeless(context, true);
		}

		// Token: 0x06007D21 RID: 32033 RVA: 0x004A5DEC File Offset: 0x004A3FEC
		private void InitializeBuildingResidents(DataContext context)
		{
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> keyValuePair in this._residences)
			{
				BuildingBlockKey buildingBlockKey3;
				CharacterList characterList;
				keyValuePair.Deconstruct(out buildingBlockKey3, out characterList);
				BuildingBlockKey buildingBlockKey = buildingBlockKey3;
				CharacterList value = characterList;
				foreach (int charId in value.GetCollection())
				{
					this._buildingResidents.Add(charId, buildingBlockKey);
				}
			}
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> keyValuePair in this._comfortableHouses)
			{
				BuildingBlockKey buildingBlockKey3;
				CharacterList characterList;
				keyValuePair.Deconstruct(out buildingBlockKey3, out characterList);
				BuildingBlockKey buildingBlockKey2 = buildingBlockKey3;
				CharacterList value2 = characterList;
				foreach (int charId2 in value2.GetCollection())
				{
					this._buildingComfortableHouses.Add(charId2, buildingBlockKey2);
				}
			}
		}

		// Token: 0x06007D22 RID: 32034 RVA: 0x004A5F44 File Offset: 0x004A4144
		private void FixResidentData(DataContext context)
		{
			List<int> charList = new List<int>();
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> pair in this._residences)
			{
				for (int i = pair.Value.GetCount() - 1; i >= 0; i--)
				{
					int id = pair.Value.GetCollection()[i];
					bool flag = !charList.Contains(id);
					if (flag)
					{
						charList.Add(id);
					}
					else
					{
						pair.Value.GetCollection().Remove(id);
					}
				}
			}
			charList.Clear();
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> pair2 in this._comfortableHouses)
			{
				for (int j = pair2.Value.GetCount() - 1; j >= 0; j--)
				{
					int id2 = pair2.Value.GetCollection()[j];
					bool flag2 = !charList.Contains(id2);
					if (flag2)
					{
						charList.Add(id2);
					}
					else
					{
						pair2.Value.GetCollection().Remove(id2);
					}
				}
			}
		}

		// Token: 0x06007D23 RID: 32035 RVA: 0x004A60E4 File Offset: 0x004A42E4
		public void AddResidence(DataContext context, BuildingBlockKey key)
		{
			bool flag = this._residences.ContainsKey(key);
			if (flag)
			{
				this.SetElement_Residences(key, default(CharacterList), context);
			}
			else
			{
				this.AddElement_Residences(key, default(CharacterList), context);
			}
		}

		// Token: 0x06007D24 RID: 32036 RVA: 0x004A6128 File Offset: 0x004A4328
		public void RemoveResidence(DataContext context, BuildingBlockKey key)
		{
			CharacterList charList;
			bool flag = !this._residences.TryGetValue(key, out charList);
			if (!flag)
			{
				this.RemoveElement_Residences(key, context);
				List<int> collection = charList.GetCollection();
				foreach (int charId in collection)
				{
					GameData.Domains.Character.Character characeter;
					bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out characeter);
					if (flag2)
					{
						this.AddTaiwuResident(context, charId, false);
					}
					this._buildingResidents.Remove(charId);
				}
				charList.Clear();
				List<short> autoCheckInList = DomainManager.Extra.GetAutoCheckInResidenceList();
				bool flag3 = autoCheckInList.Contains(key.BuildingBlockIndex);
				if (flag3)
				{
					autoCheckInList.Remove(key.BuildingBlockIndex);
					DomainManager.Extra.SetAutoCheckInResidenceList(autoCheckInList, context);
				}
			}
		}

		// Token: 0x06007D25 RID: 32037 RVA: 0x004A6214 File Offset: 0x004A4414
		public void AddComfortableHouse(DataContext context, BuildingBlockKey key)
		{
			DomainManager.Extra.SetFeast(context, key, DomainManager.Extra.GetFeast(key));
			bool flag = this._comfortableHouses.ContainsKey(key);
			if (flag)
			{
				this.SetElement_ComfortableHouses(key, default(CharacterList), context);
			}
			else
			{
				this.AddElement_ComfortableHouses(key, default(CharacterList), context);
			}
		}

		// Token: 0x06007D26 RID: 32038 RVA: 0x004A6270 File Offset: 0x004A4470
		public void RemoveComfortableHouse(DataContext context, BuildingBlockKey key)
		{
			DomainManager.Extra.RemoveFeast(context, key);
			CharacterList charList;
			bool flag = !this._comfortableHouses.TryGetValue(key, out charList);
			if (!flag)
			{
				this.RemoveElement_ComfortableHouses(key, context);
				List<int> collection = charList.GetCollection();
				foreach (int charId in collection)
				{
					this._buildingComfortableHouses.Remove(charId);
				}
				List<short> autoCheckInList = DomainManager.Extra.GetAutoCheckInComfortableList();
				bool flag2 = autoCheckInList.Contains(key.BuildingBlockIndex);
				if (flag2)
				{
					autoCheckInList.Remove(key.BuildingBlockIndex);
					DomainManager.Extra.SetAutoCheckInComfortableList(autoCheckInList, context);
				}
			}
		}

		// Token: 0x06007D27 RID: 32039 RVA: 0x004A6340 File Offset: 0x004A4540
		private void RemoveExceedingResidents(DataContext context, BuildingBlockKey key)
		{
			CharacterList characterList;
			bool flag = this._residences.TryGetValue(key, out characterList);
			if (flag)
			{
				CharacterList newList = default(CharacterList);
				int capacity = BuildingScale.DefValue.ResidenceCapacity.GetLevelEffect((int)this.BuildingBlockLevel(key));
				List<int> collection = characterList.GetCollection();
				for (int i = 0; i < Math.Min(capacity, collection.Count); i++)
				{
					int charId = collection[i];
					newList.Add(charId);
				}
				for (int j = capacity; j < collection.Count; j++)
				{
					int charId2 = collection[j];
					this._buildingResidents.Remove(charId2);
					this._homeless.Add(charId2);
				}
				characterList.Clear();
				this.SetElement_Residences(key, newList, context);
				this.SetHomeless(this._homeless, context);
			}
		}

		// Token: 0x06007D28 RID: 32040 RVA: 0x004A6428 File Offset: 0x004A4628
		public void SetAllResidenceAutoCheckIn(DataContext context)
		{
			List<Location> taiwuBuildingList = this.GetTaiwuBuildingAreas();
			for (int i = 0; i < taiwuBuildingList.Count; i++)
			{
				Location location = taiwuBuildingList[i];
				BuildingAreaData areaData = this.GetElement_BuildingAreas(location);
				for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
				{
					BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
					BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
					bool flag = blockData.TemplateId == 46;
					if (flag)
					{
						this.SetResidenceAutoCheckIn(context, blockKey.BuildingBlockIndex, true);
					}
				}
			}
		}

		// Token: 0x06007D29 RID: 32041 RVA: 0x004A64D0 File Offset: 0x004A46D0
		[DomainMethod]
		public void AddToResidence(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
		{
			this.RemoveTaiwuResident(context, charId, true);
			bool flag = !this._residences.ContainsKey(buildingBlockKey);
			if (flag)
			{
				this.AddElement_Residences(buildingBlockKey, default(CharacterList), context);
			}
			this.AddCharToResidence(context, charId, buildingBlockKey);
		}

		// Token: 0x06007D2A RID: 32042 RVA: 0x004A6517 File Offset: 0x004A4717
		[DomainMethod]
		public void RemoveFromResidence(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
		{
			this.RemoveCharFromResidence(context, charId, buildingBlockKey);
			this.AddTaiwuResident(context, charId, false);
		}

		// Token: 0x06007D2B RID: 32043 RVA: 0x004A6530 File Offset: 0x004A4730
		[DomainMethod]
		public void RemoveAllFormResidence(DataContext context, BuildingBlockKey buildingBlockKey)
		{
			CharacterList charIdList = this._residences[buildingBlockKey];
			for (int i = charIdList.GetCount() - 1; i >= 0; i--)
			{
				int charId = charIdList.GetCollection()[i];
				this.RemoveCharFromResidence(context, charId, buildingBlockKey);
				this.AddTaiwuResident(context, charId, false);
			}
		}

		// Token: 0x06007D2C RID: 32044 RVA: 0x004A658C File Offset: 0x004A478C
		[DomainMethod]
		public void RemoveAllFromComfortableHouse(DataContext context, BuildingBlockKey buildingBlockKey)
		{
			bool flag = !this._comfortableHouses.ContainsKey(buildingBlockKey);
			if (!flag)
			{
				CharacterList charIdList = this._comfortableHouses[buildingBlockKey];
				for (int i = charIdList.GetCount() - 1; i >= 0; i--)
				{
					int charId = charIdList.GetCollection()[i];
					this.RemoveCharFromComfortableHouse(context, charId, buildingBlockKey);
				}
			}
		}

		// Token: 0x06007D2D RID: 32045 RVA: 0x004A65F4 File Offset: 0x004A47F4
		[DomainMethod]
		public List<int> SortedComfortableHousePeople(DataContext context, List<int> charIdList)
		{
			for (int index = charIdList.Count - 1; index >= 0; index--)
			{
				int id = charIdList[index];
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(id, out character) || character.GetAgeGroup() == 0;
				if (flag)
				{
					charIdList.RemoveAt(index);
				}
			}
			charIdList.Sort((int l, int r) => DomainManager.Character.GetElement_Objects(l).GetHappiness().CompareTo(DomainManager.Character.GetElement_Objects(r).GetHappiness()));
			return charIdList;
		}

		// Token: 0x06007D2E RID: 32046 RVA: 0x004A667C File Offset: 0x004A487C
		[DomainMethod]
		public void ReplaceCharacterInResidence(DataContext context, int charId, BuildingBlockKey buildingBlockKey, sbyte index)
		{
			CharacterList residence = this._residences[buildingBlockKey];
			Tester.Assert(index >= 0 && residence.GetCount() > (int)index, "");
			List<int> collection = residence.GetCollection();
			int replacedCharId = collection[(int)index];
			bool flag = charId < 0;
			if (flag)
			{
				this.RemoveFromResidence(context, replacedCharId, buildingBlockKey);
			}
			else
			{
				bool flag2 = this._homeless.Contains(charId);
				if (flag2)
				{
					this._homeless.Remove(charId);
					collection[(int)index] = charId;
					this._homeless.Add(replacedCharId);
					this.SetHomeless(this._homeless, context);
					this._buildingResidents.Add(charId, buildingBlockKey);
					this._buildingResidents.Remove(replacedCharId);
				}
				else
				{
					BuildingBlockKey srcBuildingBlockKey = this._buildingResidents[charId];
					CharacterList srcResidence;
					bool flag3 = this._residences.TryGetValue(srcBuildingBlockKey, out srcResidence);
					if (!flag3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
						defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
						defaultInterpolatedStringHandler.AppendLiteral("'s previous living status is unknown.");
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					List<int> srcResidenceCollection = srcResidence.GetCollection();
					int srcIndex = srcResidenceCollection.IndexOf(charId);
					Tester.Assert(srcIndex >= 0, "");
					srcResidenceCollection[srcIndex] = replacedCharId;
					collection[(int)index] = charId;
					this.SetElement_Residences(srcBuildingBlockKey, srcResidence, context);
					this._buildingResidents[replacedCharId] = srcBuildingBlockKey;
					this._buildingResidents[charId] = buildingBlockKey;
				}
				this.SetElement_Residences(buildingBlockKey, residence, context);
			}
		}

		// Token: 0x06007D2F RID: 32047 RVA: 0x004A6808 File Offset: 0x004A4A08
		[DomainMethod]
		public void ReplaceCharacterInComfortableHouse(DataContext context, int charIdB, BuildingBlockKey buildingBlockKey, sbyte index)
		{
			CharacterList comfortableHouse = this._comfortableHouses[buildingBlockKey];
			Tester.Assert(index >= 0 && comfortableHouse.GetCount() > (int)index, "");
			List<int> collection = comfortableHouse.GetCollection();
			int replacedCharId = collection[(int)index];
			bool flag = charIdB < 0;
			if (flag)
			{
				this.RemoveFromComfortableHouse(context, replacedCharId, buildingBlockKey);
			}
			else
			{
				BuildingBlockKey srcBuildingBlockKey;
				bool flag2 = this._buildingComfortableHouses.TryGetValue(charIdB, out srcBuildingBlockKey);
				if (flag2)
				{
					CharacterList srcComfortableHouse = this._comfortableHouses.GetValueOrDefault(srcBuildingBlockKey);
					List<int> srcComfortableHouseCollection = srcComfortableHouse.GetCollection();
					int srcIndex = srcComfortableHouseCollection.IndexOf(charIdB);
					Tester.Assert(srcIndex >= 0, "");
					srcComfortableHouseCollection[srcIndex] = replacedCharId;
					collection[(int)index] = charIdB;
					this.SetElement_ComfortableHouses(srcBuildingBlockKey, srcComfortableHouse, context);
					this._buildingComfortableHouses[replacedCharId] = srcBuildingBlockKey;
					this._buildingComfortableHouses[charIdB] = buildingBlockKey;
				}
				else
				{
					collection[(int)index] = charIdB;
					this._buildingComfortableHouses.Add(charIdB, buildingBlockKey);
					this._buildingComfortableHouses.Remove(replacedCharId);
				}
				this.SetElement_ComfortableHouses(buildingBlockKey, comfortableHouse, context);
			}
		}

		// Token: 0x06007D30 RID: 32048 RVA: 0x004A6928 File Offset: 0x004A4B28
		[DomainMethod]
		public void AddToComfortableHouse(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
		{
			BuildingBlockKey srcBuildingBlockKey;
			bool flag = this._buildingComfortableHouses.TryGetValue(charId, out srcBuildingBlockKey);
			if (flag)
			{
				bool flag2 = this._comfortableHouses.ContainsKey(srcBuildingBlockKey);
				if (flag2)
				{
					this.RemoveCharFromComfortableHouse(context, charId, srcBuildingBlockKey);
				}
			}
			bool flag3 = !this._comfortableHouses.ContainsKey(buildingBlockKey);
			if (flag3)
			{
				this.AddElement_ComfortableHouses(buildingBlockKey, default(CharacterList), context);
			}
			this.AddCharToComfortableHouse(context, charId, buildingBlockKey);
		}

		// Token: 0x06007D31 RID: 32049 RVA: 0x004A6994 File Offset: 0x004A4B94
		[DomainMethod]
		public void RemoveFromComfortableHouse(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
		{
			this.RemoveCharFromComfortableHouse(context, charId, buildingBlockKey);
		}

		// Token: 0x06007D32 RID: 32050 RVA: 0x004A69A4 File Offset: 0x004A4BA4
		[DomainMethod]
		public CharacterList QuickFillResidence(DataContext context, BuildingBlockKey buildingBlockKey)
		{
			bool flag = !this._residences.ContainsKey(buildingBlockKey);
			if (flag)
			{
				this.AddElement_Residences(buildingBlockKey, default(CharacterList), context);
			}
			sbyte level = this.BuildingBlockLevel(buildingBlockKey);
			int[] collection = this._homeless.GetCollection().ToArray();
			foreach (int homelessChar in collection)
			{
				bool flag2 = this._residences[buildingBlockKey].GetCount() >= BuildingScale.DefValue.ResidenceCapacity.GetLevelEffect((int)level);
				if (flag2)
				{
					break;
				}
				bool flag3 = DomainManager.Character.GetElement_Objects(homelessChar).IsCompletelyInfected();
				if (!flag3)
				{
					this.AddCharToResidence(context, homelessChar, buildingBlockKey);
					CharacterList homeless = this._homeless;
					homeless.Remove(homelessChar);
					this.SetHomeless(homeless, context);
				}
			}
			return this._residences[buildingBlockKey];
		}

		// Token: 0x06007D33 RID: 32051 RVA: 0x004A6A94 File Offset: 0x004A4C94
		[DomainMethod]
		public CharacterList QuickFillComfortableHouse(DataContext context, BuildingBlockKey buildingBlockKey)
		{
			sbyte level = this.BuildingBlockLevel(buildingBlockKey);
			int capacity = BuildingScale.DefValue.ComfortableHouseCapacity.GetLevelEffect((int)level);
			int addCount = capacity;
			bool flag = this._comfortableHouses.ContainsKey(buildingBlockKey);
			if (flag)
			{
				addCount = capacity - this._comfortableHouses[buildingBlockKey].GetCount();
				bool flag2 = addCount <= 0;
				if (flag2)
				{
					return this._comfortableHouses[buildingBlockKey];
				}
			}
			List<int> charIdList = ObjectPool<List<int>>.Instance.Get();
			charIdList.Clear();
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> pair in this._residences)
			{
				charIdList.AddRange(pair.Value.GetCollection());
			}
			charIdList.AddRange(this._homeless.GetCollection());
			for (int index = charIdList.Count - 1; index >= 0; index--)
			{
				int id = charIdList[index];
				GameData.Domains.Character.Character character;
				bool flag3 = this._buildingComfortableHouses.ContainsKey(id) || !DomainManager.Character.TryGetElement_Objects(id, out character) || character.GetAgeGroup() == 0;
				if (flag3)
				{
					charIdList.RemoveAt(index);
				}
			}
			charIdList.Sort((int l, int r) => DomainManager.Character.GetElement_Objects(l).GetHappiness().CompareTo(DomainManager.Character.GetElement_Objects(r).GetHappiness()));
			List<int> charAdd = charIdList.GetRange(0, Math.Min(addCount, charIdList.Count));
			for (int i = 0; i < charAdd.Count; i++)
			{
				int charId = charAdd[i];
				this.AddToComfortableHouse(context, charId, buildingBlockKey);
			}
			ObjectPool<List<int>>.Instance.Return(charIdList);
			CharacterList characterList;
			this._comfortableHouses.TryGetValue(buildingBlockKey, out characterList);
			return characterList;
		}

		// Token: 0x06007D34 RID: 32052 RVA: 0x004A6C80 File Offset: 0x004A4E80
		[DomainMethod]
		public CharacterList GetCharsInResidence(DataContext context, BuildingBlockKey key)
		{
			return this._residences.ContainsKey(key) ? this._residences[key] : default(CharacterList);
		}

		// Token: 0x06007D35 RID: 32053 RVA: 0x004A6CB8 File Offset: 0x004A4EB8
		[DomainMethod]
		public List<CharacterList> GetAllResidents(DataContext context, BuildingBlockKey blockKey, bool homelessFirst)
		{
			List<CharacterList> allResidents = new List<CharacterList>(4);
			CharacterList infected = default(CharacterList);
			CharacterList homeless = default(CharacterList);
			List<int> collection = this._homeless.GetCollection();
			foreach (int charId in collection)
			{
				bool flag = DomainManager.Character.GetElement_Objects(charId).IsCompletelyInfected();
				if (flag)
				{
					CharacterList charList = infected;
					charList.Add(charId);
					infected = charList;
				}
				else
				{
					CharacterList charList2 = homeless;
					charList2.Add(charId);
					homeless = charList2;
				}
			}
			if (homelessFirst)
			{
				allResidents.Add(homeless);
				allResidents.Add(default(CharacterList));
				foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in this._residences)
				{
					bool flag2 = !residence.Key.Equals(blockKey);
					if (flag2)
					{
						CharacterList charList3 = allResidents[1];
						charList3.AddRange(residence.Value.GetCollection());
						allResidents[1] = charList3;
					}
				}
			}
			else
			{
				allResidents.Add(default(CharacterList));
				foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence2 in this._residences)
				{
					bool flag3 = !residence2.Key.Equals(blockKey);
					if (flag3)
					{
						CharacterList charList4 = allResidents[0];
						charList4.AddRange(residence2.Value.GetCollection());
						allResidents[0] = charList4;
					}
				}
				allResidents.Add(homeless);
			}
			allResidents.Add(infected);
			return allResidents;
		}

		// Token: 0x06007D36 RID: 32054 RVA: 0x004A6EC8 File Offset: 0x004A50C8
		[DomainMethod]
		public CharacterList GetCharsInComfortableHouse(DataContext context, BuildingBlockKey key)
		{
			bool flag = !this._comfortableHouses.ContainsKey(key);
			CharacterList result;
			if (flag)
			{
				result = default(CharacterList);
			}
			else
			{
				result = this._comfortableHouses[key];
			}
			return result;
		}

		// Token: 0x06007D37 RID: 32055 RVA: 0x004A6F08 File Offset: 0x004A5108
		[DomainMethod]
		public CharacterList GetHomeless(DataContext context)
		{
			return this._homeless;
		}

		// Token: 0x06007D38 RID: 32056 RVA: 0x004A6F20 File Offset: 0x004A5120
		[DomainMethod]
		public void SetResidenceAutoCheckIn(DataContext context, short blockIndex, bool isAutoCheckIn)
		{
			List<short> autoCheckInList = DomainManager.Extra.GetAutoCheckInResidenceList();
			bool flag = isAutoCheckIn && !autoCheckInList.Contains(blockIndex);
			if (flag)
			{
				autoCheckInList.Add(blockIndex);
				DomainManager.Extra.SetAutoCheckInResidenceList(autoCheckInList, context);
			}
			bool flag2 = !isAutoCheckIn && autoCheckInList.Contains(blockIndex);
			if (flag2)
			{
				autoCheckInList.Remove(blockIndex);
				DomainManager.Extra.SetAutoCheckInResidenceList(autoCheckInList, context);
			}
		}

		// Token: 0x06007D39 RID: 32057 RVA: 0x004A6F8C File Offset: 0x004A518C
		[DomainMethod]
		public void SetComfortableAutoCheckIn(DataContext context, short blockIndex, bool isAutoCheckIn)
		{
			List<short> autoCheckInList = DomainManager.Extra.GetAutoCheckInComfortableList();
			bool flag = isAutoCheckIn && !autoCheckInList.Contains(blockIndex);
			if (flag)
			{
				autoCheckInList.Add(blockIndex);
				DomainManager.Extra.SetAutoCheckInComfortableList(autoCheckInList, context);
			}
			bool flag2 = !isAutoCheckIn && autoCheckInList.Contains(blockIndex);
			if (flag2)
			{
				autoCheckInList.Remove(blockIndex);
				DomainManager.Extra.SetAutoCheckInComfortableList(autoCheckInList, context);
			}
		}

		// Token: 0x06007D3A RID: 32058 RVA: 0x004A6FF8 File Offset: 0x004A51F8
		[DomainMethod]
		[return: TupleElementNames(new string[]
		{
			"sumCapacity",
			"residentsCount",
			"villagerSumCount"
		})]
		public ValueTuple<int, int, int> GetResidenceInfo()
		{
			int capacity = 0;
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in this._residences)
			{
				sbyte level = this.BuildingBlockLevel(residence.Key);
				bool flag = level <= 0;
				if (!flag)
				{
					capacity += BuildingScale.DefValue.ResidenceCapacity.GetLevelEffect((int)level);
				}
			}
			return new ValueTuple<int, int, int>(capacity, this._buildingResidents.Count, this._buildingResidents.Count + this._homeless.GetCount());
		}

		// Token: 0x06007D3B RID: 32059 RVA: 0x004A70A4 File Offset: 0x004A52A4
		public byte GetLivingStatus(int charId)
		{
			bool flag = DomainManager.Taiwu.IsInGroup(charId);
			byte result;
			if (flag)
			{
				result = 1;
			}
			else
			{
				BuildingBlockKey resident;
				bool flag2 = this._buildingResidents.TryGetValue(charId, out resident);
				if (flag2)
				{
					result = 2;
				}
				else
				{
					bool flag3 = this._homeless.Contains(charId);
					if (flag3)
					{
						result = 0;
					}
					else
					{
						Logger logger = BuildingDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Given character ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
						defaultInterpolatedStringHandler.AppendLiteral(" is not a taiwu resident.");
						logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x06007D3C RID: 32060 RVA: 0x004A7138 File Offset: 0x004A5338
		public void AddTaiwuResident(DataContext context, int charId, bool autoHousing = false)
		{
			if (autoHousing)
			{
				foreach (KeyValuePair<BuildingBlockKey, CharacterList> residence in this._residences)
				{
					BuildingBlockData blockData = this._buildingBlocks[residence.Key];
					sbyte level = this.BuildingBlockLevel(residence.Key);
					bool flag = level <= 0;
					if (!flag)
					{
						bool flag2 = residence.Value.GetCount() == BuildingScale.DefValue.ResidenceCapacity.GetLevelEffect((int)level);
						if (!flag2)
						{
							this.AddCharToResidence(context, charId, residence.Key);
							return;
						}
					}
				}
			}
			this._homeless.Add(charId);
			this.SetHomeless(this._homeless, context);
		}

		// Token: 0x06007D3D RID: 32061 RVA: 0x004A7218 File Offset: 0x004A5418
		public void MakeTargetHomeless(DataContext context, int charId)
		{
			bool flag = this._homeless.Contains(charId);
			if (!flag)
			{
				bool flag2 = !this._buildingResidents.ContainsKey(charId);
				if (!flag2)
				{
					this.RemoveTaiwuResident(context, charId, false);
					this._homeless.Add(charId);
					this.SetHomeless(this._homeless, context);
				}
			}
		}

		// Token: 0x06007D3E RID: 32062 RVA: 0x004A7274 File Offset: 0x004A5474
		public void RemoveTaiwuResident(DataContext context, int charId, bool removeFromHomeless = true)
		{
			BuildingBlockKey buildingBlockKey;
			bool flag = this._buildingResidents.TryGetValue(charId, out buildingBlockKey);
			if (flag)
			{
				bool flag2 = this._residences.ContainsKey(buildingBlockKey);
				if (flag2)
				{
					this.RemoveCharFromResidence(context, charId, buildingBlockKey);
				}
			}
			else
			{
				bool flag3 = removeFromHomeless && this._homeless.Contains(charId);
				if (flag3)
				{
					this._homeless.Remove(charId);
					this.SetHomeless(this._homeless, context);
				}
			}
		}

		// Token: 0x06007D3F RID: 32063 RVA: 0x004A72E8 File Offset: 0x004A54E8
		private void UpdateHomeless(DataContext context, bool updateAll)
		{
			bool flag = this._homeless.GetCount() <= 0;
			if (!flag)
			{
				List<int> collection = this._homeless.GetCollection();
				if (updateAll)
				{
					this._homeless = default(CharacterList);
					this.SetHomeless(this._homeless, context);
					foreach (int homelessChar in collection)
					{
						this.AddTaiwuResident(context, homelessChar, !DomainManager.Character.GetElement_Objects(homelessChar).IsCompletelyInfected());
					}
				}
				else
				{
					foreach (int homelessChar2 in collection)
					{
						bool flag2 = DomainManager.Character.GetElement_Objects(homelessChar2).IsCompletelyInfected();
						if (!flag2)
						{
							CharacterList homeless = this._homeless;
							homeless.Remove(homelessChar2);
							this.SetHomeless(homeless, context);
							this.AddTaiwuResident(context, homelessChar2, true);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06007D40 RID: 32064 RVA: 0x004A741C File Offset: 0x004A561C
		public void SetAllVillagerHomeless(DataContext context)
		{
			foreach (int charId in this._buildingResidents.Keys)
			{
				this.MakeTargetHomeless(context, charId);
			}
		}

		// Token: 0x06007D41 RID: 32065 RVA: 0x004A747C File Offset: 0x004A567C
		private void UpdateResidentsHappinessAndFavor(DataContext context)
		{
			BuildingDomain.<>c__DisplayClass393_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			CS$<>8__locals1.taiwu = DomainManager.Taiwu.GetTaiwu();
			List<int> collection = this._homeless.GetCollection();
			foreach (int charId in collection)
			{
				BuildingDomain.<UpdateResidentsHappinessAndFavor>g__UpdateHomelessHappinessAndFavor|393_1(charId, ref CS$<>8__locals1);
			}
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> pair in this._residences)
			{
				BuildingBlockKey blockKey = pair.Key;
				collection = pair.Value.GetCollection();
				BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
				bool flag = blockData.CanUse();
				if (!flag)
				{
					foreach (int charId2 in collection)
					{
						BuildingDomain.<UpdateResidentsHappinessAndFavor>g__UpdateHomelessHappinessAndFavor|393_1(charId2, ref CS$<>8__locals1);
					}
				}
			}
		}

		// Token: 0x06007D42 RID: 32066 RVA: 0x004A75C0 File Offset: 0x004A57C0
		private void AddCharToResidence(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
		{
			this._buildingResidents.Add(charId, buildingBlockKey);
			CharacterList charList = this._residences[buildingBlockKey];
			charList.Add(charId);
			this.SetElement_Residences(buildingBlockKey, charList, context);
		}

		// Token: 0x06007D43 RID: 32067 RVA: 0x004A75FC File Offset: 0x004A57FC
		private void RemoveCharFromResidence(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
		{
			this._buildingResidents.Remove(charId);
			CharacterList charList = this._residences[buildingBlockKey];
			charList.Remove(charId);
			this.SetElement_Residences(buildingBlockKey, charList, context);
		}

		// Token: 0x06007D44 RID: 32068 RVA: 0x004A7638 File Offset: 0x004A5838
		private void AddCharToComfortableHouse(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
		{
			this._buildingComfortableHouses.Add(charId, buildingBlockKey);
			CharacterList charList = this._comfortableHouses[buildingBlockKey];
			charList.Add(charId);
			this.SetElement_ComfortableHouses(buildingBlockKey, charList, context);
		}

		// Token: 0x06007D45 RID: 32069 RVA: 0x004A7674 File Offset: 0x004A5874
		private void RemoveCharFromComfortableHouse(DataContext context, int charId, BuildingBlockKey buildingBlockKey)
		{
			this._buildingComfortableHouses.Remove(charId);
			CharacterList charList = this._comfortableHouses[buildingBlockKey];
			charList.Remove(charId);
			this.SetElement_ComfortableHouses(buildingBlockKey, charList, context);
		}

		// Token: 0x06007D46 RID: 32070 RVA: 0x004A76AF File Offset: 0x004A58AF
		public void SetCharacterParticipantFeast(int charId, short feastType, int happiness, ItemKey dishCopy)
		{
			this._feastParticipants[charId] = new ValueTuple<short, int, ItemKey>(feastType, happiness, dishCopy);
		}

		// Token: 0x06007D47 RID: 32071 RVA: 0x004A76C8 File Offset: 0x004A58C8
		public void ClearFeastParticipants()
		{
			this._feastParticipants.Clear();
		}

		// Token: 0x06007D48 RID: 32072 RVA: 0x004A76D8 File Offset: 0x004A58D8
		public Dictionary<BuildingBlockKey, CharacterList> GetAllComfortableHouses()
		{
			return this._comfortableHouses;
		}

		// Token: 0x06007D49 RID: 32073 RVA: 0x004A76F0 File Offset: 0x004A58F0
		public bool IsCharacterParticipantFeast(int charId, out ValueTuple<short, int, ItemKey> value)
		{
			return this._feastParticipants.TryGetValue(charId, out value);
		}

		// Token: 0x06007D4A RID: 32074 RVA: 0x004A7710 File Offset: 0x004A5910
		public bool IsCharacterAbleToJoinFeast(int charId)
		{
			bool flag = DomainManager.Taiwu.IsInGroup(charId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character character;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = character.GetCreatingType() != 1;
					result = (!flag3 && ((character.GetOrganizationInfo().OrgTemplateId == 16) ? (character.GetAgeGroup() != 0) : (character.GetLocation().AreaId == DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId && DomainManager.Character.GetFavorabilityType(charId, DomainManager.Taiwu.GetTaiwuCharId()) >= 2 && CharacterMatcher.DefValue.CanJoinFeast.Match(character))));
				}
			}
			return result;
		}

		// Token: 0x06007D4B RID: 32075 RVA: 0x004A77C4 File Offset: 0x004A59C4
		public void FeastAdvanceMonth_Complement(DataContext context)
		{
			foreach (KeyValuePair<int, ValueTuple<short, int, ItemKey>> keyValuePair in this._feastParticipants)
			{
				int num;
				ValueTuple<short, int, ItemKey> valueTuple;
				keyValuePair.Deconstruct(out num, out valueTuple);
				int id = num;
				ValueTuple<short, int, ItemKey> data = valueTuple;
				GameData.Domains.Character.Character character;
				bool flag = DomainManager.Character.TryGetElement_Objects(id, out character);
				if (flag)
				{
					bool flag2 = character.GetEatingItems().GetAvailableEatingSlot(character.GetCurrMaxEatingSlotsCount()) >= 0;
					if (flag2)
					{
						character.AddEatingItem(context, data.Item3, null);
					}
				}
			}
		}

		// Token: 0x06007D4C RID: 32076 RVA: 0x004A7868 File Offset: 0x004A5A68
		public void TryRemoveFeastCustomer(DataContext context, int charId)
		{
			BuildingBlockKey buildingBlockKey;
			bool flag = this._buildingComfortableHouses.TryGetValue(charId, out buildingBlockKey) && this._comfortableHouses.ContainsKey(buildingBlockKey) && !this.IsCharacterAbleToJoinFeast(charId);
			if (flag)
			{
				this.RemoveCharFromComfortableHouse(context, charId, buildingBlockKey);
			}
		}

		// Token: 0x06007D4D RID: 32077 RVA: 0x004A78B0 File Offset: 0x004A5AB0
		[DomainMethod]
		public unsafe List<CharacterDisplayData> GetFeastTargetCharList(DataContext context, BuildingBlockKey buildingBlockKey)
		{
			CharacterList selectedCharList = this.GetCharsInComfortableHouse(context, buildingBlockKey);
			List<CharacterDisplayData> charDataList = new List<CharacterDisplayData>();
			List<CharacterList> allResidents = this.GetAllResidents(context, buildingBlockKey, false);
			foreach (CharacterList residents in allResidents)
			{
				foreach (int charId2 in residents.GetCollection())
				{
					bool flag = selectedCharList.Contains(charId2);
					if (!flag)
					{
						bool flag2 = !this.IsCharacterAbleToJoinFeast(charId2);
						if (!flag2)
						{
							CharacterDisplayData charData = DomainManager.Character.GetCharacterDisplayData(charId2);
							charDataList.Add(charData);
						}
					}
				}
			}
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(buildingBlockKey.AreaId);
			Span<MapBlockData> span = blocks;
			for (int i = 0; i < span.Length; i++)
			{
				MapBlockData mapBlockData = *span[i];
				bool flag3 = mapBlockData.CharacterSet != null;
				if (flag3)
				{
					using (HashSet<int>.Enumerator enumerator3 = mapBlockData.CharacterSet.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							int charId = enumerator3.Current;
							bool flag4 = selectedCharList.Contains(charId);
							if (!flag4)
							{
								bool flag5 = charDataList.Any((CharacterDisplayData d) => d.CharacterId == charId);
								if (!flag5)
								{
									bool flag6 = !this.IsCharacterAbleToJoinFeast(charId);
									if (!flag6)
									{
										CharacterDisplayData charData2 = DomainManager.Character.GetCharacterDisplayData(charId);
										charDataList.Add(charData2);
									}
								}
							}
						}
					}
				}
			}
			return charDataList;
		}

		// Token: 0x06007D4E RID: 32078 RVA: 0x004A7AAC File Offset: 0x004A5CAC
		[DomainMethod]
		public int GetTaiwuVillageResourceBlockEffect(DataContext context, EBuildingScaleEffect effectType)
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			return this.GetBuildingBlockEffect(taiwuVillageLocation, effectType, -1);
		}

		// Token: 0x06007D4F RID: 32079 RVA: 0x004A7AD4 File Offset: 0x004A5CD4
		[DomainMethod]
		public int GetTaiwuLocationResourceBlockEffect(DataContext context, EBuildingScaleEffect effectType)
		{
			Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
			return this.GetBuildingBlockEffect(location, effectType, -1);
		}

		// Token: 0x06007D50 RID: 32080 RVA: 0x004A7B00 File Offset: 0x004A5D00
		private unsafe int CalcResourceBlockTotalEffectValue(int formulaTemplateId, Span<int> baseValues)
		{
			BuildingFormulaItem formula = BuildingFormula.Instance[formulaTemplateId];
			int total = 0;
			for (int i = 0; i < baseValues.Length; i++)
			{
				total += formula.Calculate(*baseValues[i]);
			}
			return total;
		}

		// Token: 0x06007D51 RID: 32081 RVA: 0x004A7B4C File Offset: 0x004A5D4C
		[return: TupleElementNames(new string[]
		{
			"resourcesChange",
			"expGain"
		})]
		public unsafe ValueTuple<ResourceInts, int> CalcResourceBlockIncomeEffects(Location location)
		{
			ResourceInts resources = default(ResourceInts);
			int expGain = 0;
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)20], 5);
			Span<int> baseValues = span;
			for (short templateId = 1; templateId < 11; templateId += 1)
			{
				this.CalcResourceBlockEffectBaseValues(location, templateId, ref baseValues);
				BuildingBlockItem buildingBlockCfg = BuildingBlock.Instance[templateId];
				List<short> expandInfos = buildingBlockCfg.ExpandInfos;
				bool flag = expandInfos == null || expandInfos.Count <= 0;
				if (!flag)
				{
					foreach (short buildingScaleId in buildingBlockCfg.ExpandInfos)
					{
						BuildingScaleItem buildingScaleCfg = BuildingScale.Instance[buildingScaleId];
						EBuildingScaleClass @class = buildingScaleCfg.Class;
						EBuildingScaleClass ebuildingScaleClass = @class;
						if (ebuildingScaleClass != EBuildingScaleClass.MemberResourceIncome)
						{
							if (ebuildingScaleClass == EBuildingScaleClass.MemberExpIncome)
							{
								int income = this.CalcResourceBlockTotalEffectValue(buildingScaleCfg.Formula, baseValues);
								expGain += income;
							}
						}
						else
						{
							int income2 = this.CalcResourceBlockTotalEffectValue(buildingScaleCfg.Formula, baseValues);
							resources.Add(buildingScaleCfg.ResourceType, income2);
						}
					}
				}
			}
			return new ValueTuple<ResourceInts, int>(resources, expGain);
		}

		// Token: 0x06007D52 RID: 32082 RVA: 0x004A7C8C File Offset: 0x004A5E8C
		public unsafe void CalcResourceBlockEffectBaseValues(Location location, short templateId, ref Span<int> values)
		{
			List<ValueTuple<BuildingBlockKey, int>> blocks = new List<ValueTuple<BuildingBlockKey, int>>();
			this.GetTopLevelBlocks(templateId, location, blocks, 5);
			for (int i = 0; i < values.Length; i++)
			{
				int percentage = BuildingDomain.<CalcResourceBlockEffectBaseValues>g__GetPercentage|411_0(i);
				*values[i] = ((blocks.Count > i) ? (blocks[i].Item2 * percentage / 100) : 0);
			}
		}

		// Token: 0x06007D53 RID: 32083 RVA: 0x004A7CF0 File Offset: 0x004A5EF0
		public unsafe void FinalizeResourceBlockIncomeEffectValues(ref ResourceInts resourceChange, bool isTaiwu)
		{
			CValuePercent gainResPercent = DomainManager.World.GetGainResourcePercent(isTaiwu ? 1 : 8);
			CValuePercent moneyAuthorityPercent = DomainManager.World.GetGainResourcePercent(isTaiwu ? 9 : 8);
			for (sbyte resourceType = 0; resourceType < 6; resourceType += 1)
			{
				*resourceChange[(int)resourceType] *= gainResPercent;
			}
			for (sbyte resourceType2 = 6; resourceType2 <= 7; resourceType2 += 1)
			{
				*resourceChange[(int)resourceType2] *= moneyAuthorityPercent;
			}
		}

		// Token: 0x06007D54 RID: 32084 RVA: 0x004A7D7C File Offset: 0x004A5F7C
		private void InitializeSamsaraPlatform()
		{
			this._samsaraPlatformAddMainAttributes.Initialize();
			this._samsaraPlatformAddCombatSkillQualifications.Initialize();
			this._samsaraPlatformAddLifeSkillQualifications.Initialize();
			for (int i = 0; i < this._samsaraPlatformSlots.Length; i++)
			{
				this._samsaraPlatformSlots[i] = new IntPair(-1, 0);
			}
		}

		// Token: 0x06007D55 RID: 32085 RVA: 0x004A7DD8 File Offset: 0x004A5FD8
		private List<SamsaraPlatformCharDisplayData> GetSamsaraPlatformCharList(DataContext context, bool excludeCharactersInSlot)
		{
			List<SamsaraPlatformCharDisplayData> dataList = new List<SamsaraPlatformCharDisplayData>();
			HashSet<int> sourceSet = ObjectPool<HashSet<int>>.Instance.Get();
			HashSet<int> waitingReincarnationChars = ObjectPool<HashSet<int>>.Instance.Get();
			DomainManager.Character.GetAllRelatedDeadCharIds(DomainManager.Taiwu.GetTaiwuCharId(), sourceSet, false);
			DomainManager.Character.GetTaiwuVillageDeadCharacter(sourceSet);
			DomainManager.Character.GetAllWaitingReincarnationCharIds(waitingReincarnationChars);
			foreach (int charId in sourceSet)
			{
				bool flag = !waitingReincarnationChars.Contains(charId) || this.IsCharOnSamsaraPlatform(charId, false) || ProfessionSkillHandle.BuddhistMonkSkill_IsDirectedSamsaraCharacter(charId);
				if (!flag)
				{
					if (excludeCharactersInSlot)
					{
						bool waiting = false;
						foreach (IntPair pair in this._samsaraPlatformSlots)
						{
							bool flag2 = pair.First == charId;
							if (flag2)
							{
								waiting = true;
								break;
							}
						}
						bool flag3 = waiting;
						if (flag3)
						{
							continue;
						}
					}
					DeadCharacter deadChar = DomainManager.Character.GetDeadCharacter(charId);
					SamsaraPlatformCharDisplayData displayData = new SamsaraPlatformCharDisplayData
					{
						Id = charId,
						TemplateId = deadChar.TemplateId,
						NameRelatedData = DomainManager.Character.GetNameRelatedData(charId),
						AvatarRelatedData = deadChar.GenerateAvatarRelatedData(),
						MainAttributes = deadChar.BaseMainAttributes,
						CombatSkillQualifications = deadChar.BaseCombatSkillQualifications,
						LifeSkillQualifications = deadChar.BaseLifeSkillQualifications
					};
					dataList.Add(displayData);
				}
			}
			bool flag4 = DomainManager.Extra.IsExtraTaskInProgress(183) || DomainManager.Extra.IsExtraTaskInProgress(184);
			if (flag4)
			{
				DeadCharacter deadCharacter = DomainManager.Character.TryGetDeadCharacterByTemplateId(748);
				bool flag5 = deadCharacter == null;
				if (flag5)
				{
					GameData.Domains.Character.Character monk = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, 748);
					deadCharacter = DomainManager.Character.SectMainStoryJingangCreateDeadMonk(context, monk);
				}
				int charId2 = DomainManager.Character.TryGetDeadCharacterIdByTemplateId(748);
				Tester.Assert(charId2 != -1, "");
				SamsaraPlatformCharDisplayData displayData2 = new SamsaraPlatformCharDisplayData
				{
					Id = charId2,
					TemplateId = deadCharacter.TemplateId,
					NameRelatedData = DomainManager.Character.GetNameRelatedData(charId2),
					AvatarRelatedData = deadCharacter.GenerateAvatarRelatedData(),
					MainAttributes = deadCharacter.BaseMainAttributes,
					CombatSkillQualifications = deadCharacter.BaseCombatSkillQualifications,
					LifeSkillQualifications = deadCharacter.BaseLifeSkillQualifications
				};
				dataList.Add(displayData2);
			}
			ObjectPool<HashSet<int>>.Instance.Return(sourceSet);
			ObjectPool<HashSet<int>>.Instance.Return(waitingReincarnationChars);
			return dataList;
		}

		// Token: 0x06007D56 RID: 32086 RVA: 0x004A8098 File Offset: 0x004A6298
		[DomainMethod]
		public List<SamsaraPlatformCharDisplayData> GetSamsaraPlatformCharList(DataContext context)
		{
			return this.GetSamsaraPlatformCharList(context, false);
		}

		// Token: 0x06007D57 RID: 32087 RVA: 0x004A80B4 File Offset: 0x004A62B4
		[DomainMethod]
		public void SetSamsaraPlatformChar(DataContext context, sbyte destinyType, int charId)
		{
			this.SetElement_SamsaraPlatformSlots((int)destinyType, new IntPair(charId, 0), context);
			bool flag = DomainManager.Extra.IsExtraTaskChainInProgress(35);
			if (flag)
			{
				int monkId = DomainManager.Character.TryGetDeadCharacterIdByTemplateId(748);
				bool monkOnSamsaraPlatform = false;
				foreach (IntPair item in this._samsaraPlatformSlots)
				{
					bool flag2 = item.First == monkId;
					if (flag2)
					{
						monkOnSamsaraPlatform = true;
					}
				}
				bool flag3 = DomainManager.Extra.IsExtraTaskInProgress(183) && monkOnSamsaraPlatform;
				if (flag3)
				{
					DomainManager.Extra.TriggerExtraTask(context, 35, 184);
				}
				bool flag4 = DomainManager.Extra.IsExtraTaskInProgress(184) && !monkOnSamsaraPlatform;
				if (flag4)
				{
					DomainManager.Extra.TriggerExtraTask(context, 35, 183);
				}
			}
		}

		// Token: 0x06007D58 RID: 32088 RVA: 0x004A818C File Offset: 0x004A638C
		[DomainMethod]
		public unsafe CharacterDisplayData SamsaraPlatformReborn(DataContext context, sbyte destinyType)
		{
			IntPair slotInfo = this._samsaraPlatformSlots[(int)destinyType];
			bool flag = DomainManager.Extra.IsExtraTaskInProgress(184) && DomainManager.Character.GetDeadCharacter(slotInfo.First).TemplateId == 748;
			CharacterDisplayData result;
			if (flag)
			{
				DomainManager.TaiwuEvent.OnEvent_JingangSectMainStoryReborn();
				SamsaraPlatformRecordCollection collection = DomainManager.Extra.GetSamsaraPlatformRecordCollection();
				int date = DomainManager.World.GetCurrDate();
				int charId = slotInfo.First;
				collection.AddSamsaraFailed(date, charId, destinyType);
				DomainManager.Extra.CommitSamsaraPlatformRecord(context);
				result = null;
			}
			else
			{
				bool flag2 = slotInfo.First < 0 || slotInfo.Second < (int)GlobalConfig.Instance.SamsaraPlatformMaxProgress;
				if (flag2)
				{
					result = null;
				}
				else
				{
					DestinyTypeItem destinyConfig = DestinyType.Instance[destinyType];
					bool bornInSect = context.Random.CheckPercentProb((int)GlobalConfig.Instance.SamsaraPlatformBornInSectOdds);
					List<int> motherRandomPool = ObjectPool<List<int>>.Instance.Get();
					motherRandomPool.Clear();
					bool flag3 = bornInSect;
					if (flag3)
					{
						this.AddSamsaraPlatformSectRandomPool(destinyConfig, motherRandomPool);
					}
					bool flag4 = motherRandomPool.Count == 0;
					if (flag4)
					{
						this.AddSamsaraPlatformCityRandomPool(destinyConfig, motherRandomPool);
					}
					bool flag5 = motherRandomPool.Count == 0 && !bornInSect;
					if (flag5)
					{
						this.AddSamsaraPlatformSectRandomPool(destinyConfig, motherRandomPool);
					}
					int motherId = -1;
					bool flag6 = motherRandomPool.Count > 0;
					if (flag6)
					{
						motherId = motherRandomPool[context.Random.Next(motherRandomPool.Count)];
						GameData.Domains.Character.Character mother = DomainManager.Character.GetElement_Objects(motherId);
						LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
						this.AddElement_SamsaraPlatformBornDict(motherId, new IntPair(slotInfo.First, (int)destinyType), context);
						PregnantState pregnantState;
						bool flag7 = !DomainManager.Character.TryGetPregnantState(motherId, out pregnantState);
						if (flag7)
						{
							DomainManager.Character.RemovePregnantLock(context, motherId);
							DomainManager.Character.MakePregnantWithoutMale(context, mother);
						}
						bool flag8 = destinyType == 0;
						if (flag8)
						{
							lifeRecordCollection.AddPregnantWithSamsara0(motherId, DomainManager.World.GetCurrDate(), mother.GetLocation());
						}
						else
						{
							bool flag9 = destinyType == 1;
							if (flag9)
							{
								lifeRecordCollection.AddPregnantWithSamsara1(motherId, DomainManager.World.GetCurrDate(), mother.GetLocation());
							}
							else
							{
								bool flag10 = destinyType == 2;
								if (flag10)
								{
									lifeRecordCollection.AddPregnantWithSamsara2(motherId, DomainManager.World.GetCurrDate(), mother.GetLocation());
								}
								else
								{
									bool flag11 = destinyType == 3;
									if (flag11)
									{
										lifeRecordCollection.AddPregnantWithSamsara3(motherId, DomainManager.World.GetCurrDate(), mother.GetLocation());
									}
									else
									{
										bool flag12 = destinyType == 4;
										if (flag12)
										{
											lifeRecordCollection.AddPregnantWithSamsara4(motherId, DomainManager.World.GetCurrDate(), mother.GetLocation());
										}
										else
										{
											bool flag13 = destinyType == 5;
											if (flag13)
											{
												lifeRecordCollection.AddPregnantWithSamsara5(motherId, DomainManager.World.GetCurrDate(), mother.GetLocation());
											}
										}
									}
								}
							}
						}
					}
					ObjectPool<List<int>>.Instance.Return(motherRandomPool);
					DeadCharacter deadChar = DomainManager.Character.GetDeadCharacter(slotInfo.First);
					MainAttributes mainAttributes = deadChar.BaseMainAttributes;
					CombatSkillShorts combatSkillQualifications = deadChar.BaseCombatSkillQualifications;
					LifeSkillShorts lifeSkillQualifications = deadChar.BaseLifeSkillQualifications;
					List<Location> taiwuBuildingList = this.GetTaiwuBuildingAreas();
					sbyte buildingLevel = 0;
					for (int i = 0; i < taiwuBuildingList.Count; i++)
					{
						Location location = taiwuBuildingList[i];
						BuildingAreaData areaData = this.GetElement_BuildingAreas(location);
						BuildingBlockKey samsaraPlatformKey = BuildingDomain.FindBuildingKey(location, areaData, 50, true);
						BuildingBlockDataEx blockDataEx;
						bool flag14 = !samsaraPlatformKey.IsInvalid && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)samsaraPlatformKey, out blockDataEx);
						if (flag14)
						{
							buildingLevel = blockDataEx.CalcUnlockedLevelCount();
							break;
						}
					}
					int addPercent = (int)(GlobalConfig.Instance.SamsaraPlatformAddBasePercent + GlobalConfig.Instance.SamsaraPlatformAddPercentPerLevel * buildingLevel);
					for (int j = 0; j < 6; j++)
					{
						*(ref this._samsaraPlatformAddMainAttributes.Items.FixedElementField + (IntPtr)j * 2) = (short)Math.Max((int)(*(ref this._samsaraPlatformAddMainAttributes.Items.FixedElementField + (IntPtr)j * 2)), (int)(*(ref mainAttributes.Items.FixedElementField + (IntPtr)j * 2)) * addPercent / 100);
					}
					this.SetSamsaraPlatformAddMainAttributes(this._samsaraPlatformAddMainAttributes, context);
					for (int k = 0; k < 14; k++)
					{
						*(ref this._samsaraPlatformAddCombatSkillQualifications.Items.FixedElementField + (IntPtr)k * 2) = (short)Math.Max((int)(*(ref this._samsaraPlatformAddCombatSkillQualifications.Items.FixedElementField + (IntPtr)k * 2)), (int)(*(ref combatSkillQualifications.Items.FixedElementField + (IntPtr)k * 2)) * addPercent / 100);
					}
					this.SetSamsaraPlatformAddCombatSkillQualifications(ref this._samsaraPlatformAddCombatSkillQualifications, context);
					for (int l = 0; l < 16; l++)
					{
						*(ref this._samsaraPlatformAddLifeSkillQualifications.Items.FixedElementField + (IntPtr)l * 2) = (short)Math.Max((int)(*(ref this._samsaraPlatformAddLifeSkillQualifications.Items.FixedElementField + (IntPtr)l * 2)), (int)(*(ref lifeSkillQualifications.Items.FixedElementField + (IntPtr)l * 2)) * addPercent / 100);
					}
					this.SetSamsaraPlatformAddLifeSkillQualifications(ref this._samsaraPlatformAddLifeSkillQualifications, context);
					this.SetElement_SamsaraPlatformSlots((int)destinyType, new IntPair(-1, 0), context);
					CharacterDisplayData motherData = null;
					bool flag15 = motherId >= 0;
					if (flag15)
					{
						OrganizationInfo orgInfo = DomainManager.Character.GetElement_Objects(motherId).GetOrganizationInfo();
						short settlementId = orgInfo.SettlementId;
						motherData = DomainManager.Character.GetCharacterDisplayData(motherId);
						motherData.Location = DomainManager.Organization.GetSettlement(settlementId).GetLocation();
						SamsaraPlatformRecordCollection collection2 = DomainManager.Extra.GetSamsaraPlatformRecordCollection();
						int date2 = DomainManager.World.GetCurrDate();
						int charId2 = slotInfo.First;
						collection2.AddSamsaraSuccess(date2, charId2, destinyType, settlementId, orgInfo.OrgTemplateId, orgInfo.Grade, orgInfo.Principal, motherData.Gender, motherId);
						DomainManager.Extra.CommitSamsaraPlatformRecord(context);
					}
					int sumMainAttributeAndQualifications = mainAttributes.GetSum() + lifeSkillQualifications.GetSum() + combatSkillQualifications.GetSum();
					int addSeniority = ProfessionFormulaImpl.Calculate(46, sumMainAttributeAndQualifications);
					DomainManager.Extra.ChangeProfessionSeniority(context, 6, addSeniority, true, false);
					result = motherData;
				}
			}
			return result;
		}

		// Token: 0x06007D59 RID: 32089 RVA: 0x004A8776 File Offset: 0x004A6976
		[DomainMethod]
		public void SectMainStoryJingangClickMonkSoulBtn(DataContext context)
		{
			DomainManager.TaiwuEvent.OnEvent_JingangSectMainStoryMonkSoul();
		}

		// Token: 0x06007D5A RID: 32090 RVA: 0x004A8784 File Offset: 0x004A6984
		private void AddSamsaraPlatformSectRandomPool(DestinyTypeItem destinyConfig, List<int> motherRandomPool)
		{
			foreach (sbyte sectId in destinyConfig.SectList)
			{
				OrgMemberCollection members = DomainManager.Organization.GetSettlementByOrgTemplateId(sectId).GetMembers();
				foreach (sbyte grade in destinyConfig.OrganizationGradeRange)
				{
					HashSet<int> memberSet = members.GetMembers(grade);
					foreach (int charId in memberSet)
					{
						GameData.Domains.Character.Character character;
						bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character) && DomainManager.Character.CanSelectForForcePregnancy(character);
						if (flag)
						{
							motherRandomPool.Add(charId);
						}
					}
				}
			}
		}

		// Token: 0x06007D5B RID: 32091 RVA: 0x004A8884 File Offset: 0x004A6A84
		private void AddSamsaraPlatformCityRandomPool(DestinyTypeItem destinyConfig, List<int> motherRandomPool)
		{
			List<Settlement> settlements = new List<Settlement>();
			DomainManager.Organization.GetAllCivilianSettlements(settlements);
			for (int i = 0; i < settlements.Count; i++)
			{
				OrgMemberCollection members = settlements[i].GetMembers();
				foreach (sbyte grade in destinyConfig.OrganizationGradeRange)
				{
					HashSet<int> memberSet = members.GetMembers(grade);
					foreach (int charId in memberSet)
					{
						GameData.Domains.Character.Character character;
						bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character) && DomainManager.Character.CanSelectForForcePregnancy(character);
						if (flag)
						{
							motherRandomPool.Add(charId);
						}
					}
				}
			}
		}

		// Token: 0x06007D5C RID: 32092 RVA: 0x004A896C File Offset: 0x004A6B6C
		[Obsolete]
		private bool CanCharPregnantBySamsaraPlatform(int charId)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			List<short> featureIds = character.GetFeatureIds();
			bool flag = character.GetGender() != 0 || character.GetAgeGroup() != 2 || character.GetKidnapperId() >= 0 || featureIds.Contains(218);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				PregnantState pregnantState;
				bool isPregnant = DomainManager.Character.TryGetPregnantState(charId, out pregnantState);
				bool flag2 = (isPregnant && (!pregnantState.IsHuman || this._samsaraPlatformBornDict.ContainsKey(charId))) || ProfessionSkillHandle.BuddhistMonkSkill_IsDirectedSamsaraMother(charId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					HashSet<int> husbandIds = DomainManager.Character.GetRelatedCharIds(charId, 1024);
					bool hasAliveHusband = false;
					foreach (int husbandId in husbandIds)
					{
						bool flag3 = DomainManager.Character.IsCharacterAlive(husbandId);
						if (flag3)
						{
							hasAliveHusband = true;
							break;
						}
					}
					result = hasAliveHusband;
				}
			}
			return result;
		}

		// Token: 0x06007D5D RID: 32093 RVA: 0x004A8A74 File Offset: 0x004A6C74
		private void AddSamsaraPlatformProgress(DataContext context, sbyte level)
		{
			HashSet<int> relatedChars = ObjectPool<HashSet<int>>.Instance.Get();
			DomainManager.Character.GetAllRelatedDeadCharIds(DomainManager.Taiwu.GetTaiwuCharId(), relatedChars, false);
			for (int i = 0; i < this._samsaraPlatformSlots.Length; i++)
			{
				IntPair slotInfo = this._samsaraPlatformSlots[i];
				bool flag = slotInfo.First >= 0;
				if (flag)
				{
					slotInfo.Second = Math.Min(slotInfo.Second + (int)level, (int)GlobalConfig.Instance.SamsaraPlatformMaxProgress);
					bool flag2 = DomainManager.Extra.IsExtraTaskInProgress(184) && DomainManager.Character.GetDeadCharacter(slotInfo.First).TemplateId == 748;
					if (flag2)
					{
						slotInfo.Second = 1;
					}
					bool flag3 = relatedChars.Contains(slotInfo.First) && slotInfo.Second >= (int)GlobalConfig.Instance.SamsaraPlatformMaxProgress;
					if (flag3)
					{
						DomainManager.World.GetInstantNotificationCollection().AddReincarnationArchitectureReincarnationEnd(slotInfo.First);
					}
					this.SetElement_SamsaraPlatformSlots(i, slotInfo, context);
				}
			}
		}

		// Token: 0x06007D5E RID: 32094 RVA: 0x004A8B94 File Offset: 0x004A6D94
		public bool IsCharOnSamsaraPlatform(int charId, bool checkSlots = true)
		{
			if (checkSlots)
			{
				for (int i = 0; i < this._samsaraPlatformSlots.Length; i++)
				{
					bool flag = this._samsaraPlatformSlots[i].First == charId;
					if (flag)
					{
						return true;
					}
				}
			}
			foreach (IntPair bornInfo in this._samsaraPlatformBornDict.Values)
			{
				bool flag2 = bornInfo.First == charId;
				if (flag2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007D5F RID: 32095 RVA: 0x004A8C44 File Offset: 0x004A6E44
		public void TryRemoveSamsaraPlatformBornData(DataContext context, int motherId)
		{
			bool flag = this._samsaraPlatformBornDict.ContainsKey(motherId);
			if (flag)
			{
				this.RemoveElement_SamsaraPlatformBornDict(motherId, context);
			}
		}

		// Token: 0x06007D60 RID: 32096 RVA: 0x004A8C6C File Offset: 0x004A6E6C
		[DomainMethod]
		public List<SamsaraPlatformCharDisplayData> GetSwapSoulCeremonySoulCharIdList(DataContext context)
		{
			return this.GetSamsaraPlatformCharList(context, true);
		}

		// Token: 0x06007D61 RID: 32097 RVA: 0x004A8C88 File Offset: 0x004A6E88
		[DomainMethod]
		public List<int> GetSwapSoulCeremonyBodyCharIdList()
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			HashSet<int> charIdHashSet = new HashSet<int>();
			OrgMemberCollection memberCollection = DomainManager.Organization.GetSettlementByOrgTemplateId(16).GetMembers();
			List<int> taiwuVillagerList = new List<int>();
			memberCollection.GetAllMembers(taiwuVillagerList);
			charIdHashSet.UnionWith(taiwuVillagerList);
			CharacterSet charSet = DomainManager.Taiwu.GetGroupCharIds();
			bool flag = charSet.GetCount() > 0;
			if (flag)
			{
				charIdHashSet.UnionWith(charSet.GetCollection());
			}
			KidnappedCharacterList kidnappedCharacterList;
			bool flag2 = DomainManager.Character.TryGetKidnappedCharacters(taiwuCharId, out kidnappedCharacterList);
			if (flag2)
			{
				charIdHashSet.UnionWith(kidnappedCharacterList.GetCollection().ConvertAll<int>((KidnappedCharacter e) => e.CharId));
			}
			charIdHashSet.RemoveWhere(delegate(int e)
			{
				bool flag3 = e == taiwuCharId;
				bool result2;
				if (flag3)
				{
					result2 = true;
				}
				else
				{
					GameData.Domains.Character.Character character;
					bool flag4 = !DomainManager.Character.TryGetElement_Objects(e, out character);
					result2 = (flag4 || character.GetAgeGroup() != 2);
				}
				return result2;
			});
			List<int> result = new List<int>();
			result.AddRange(charIdHashSet);
			return result;
		}

		// Token: 0x06007D62 RID: 32098 RVA: 0x004A8D80 File Offset: 0x004A6F80
		[DomainMethod]
		public byte TrySwapSoulCeremony(DataContext context, int soulCharId, int bodyCharId, AvatarExtraData avatarExtraData)
		{
			DeadCharacter deadCharacter;
			GameData.Domains.Character.Character bodyCharacter;
			byte result = this.GetPossessionResult(soulCharId, bodyCharId, out deadCharacter, out bodyCharacter, false);
			bool flag = result != PossessionResult.Success;
			byte result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				DomainManager.Extra.TryAddCharacterAvatarExtraData(context, bodyCharId, avatarExtraData);
				GameData.Domains.Character.Character temp;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(this._temporaryPossessionCharId, out temp);
				if (flag2)
				{
					result2 = PossessionResult.InvalidBodyCharId;
				}
				else
				{
					DomainManager.Character.Possession(context, bodyCharId, deadCharacter, bodyCharacter, new AvatarData(temp.GetAvatar()));
					DomainManager.Character.PossessionRemoveWaitingReincarnationChar(context, soulCharId);
					DomainManager.Extra.RecordPossessionData(context, bodyCharId, new PossessionData(soulCharId));
					this.RemoveTemporaryPossessionCharacter(context);
					GameDataBridge.AddDisplayEvent<List<int>, bool>(DisplayEventType.OpenGetItem_Character, new List<int>
					{
						bodyCharacter.GetId()
					}, true);
					DomainManager.Taiwu.SetVillagerRole(context, bodyCharId, -1);
					result2 = result;
				}
			}
			return result2;
		}

		// Token: 0x06007D63 RID: 32099 RVA: 0x004A8E5C File Offset: 0x004A705C
		[DomainMethod]
		public PossessionPreview GetPossessionPreview(DataContext context, int soulCharId, int bodyCharId)
		{
			DeadCharacter deadCharacter;
			GameData.Domains.Character.Character bodyCharacter;
			PossessionPreview preview = new PossessionPreview
			{
				Result = this.GetPossessionResult(soulCharId, bodyCharId, out deadCharacter, out bodyCharacter, false)
			};
			bool flag = preview.Result != PossessionResult.Success;
			PossessionPreview result;
			if (flag)
			{
				result = preview;
			}
			else
			{
				this.RemoveTemporaryPossessionCharacter(context);
				GameData.Domains.Character.Character temporaryCharacter = DomainManager.Character.CreateTemporaryCopyOfCharacter(context, bodyCharacter);
				this._temporaryPossessionCharId = temporaryCharacter.GetId();
				DomainManager.Character.Possession(context, this._temporaryPossessionCharId, deadCharacter, temporaryCharacter, null);
				preview.Id = this._temporaryPossessionCharId;
				preview.Age = temporaryCharacter.GetActualAge();
				preview.BirthDate = temporaryCharacter.GetBirthDate();
				preview.Health = temporaryCharacter.GetHealth();
				preview.Gender = temporaryCharacter.GetGender();
				preview.BaseMorality = temporaryCharacter.GetBaseMorality();
				preview.Happiness = temporaryCharacter.GetHappiness();
				preview.Fame = temporaryCharacter.GetFame();
				preview.Personalities = temporaryCharacter.GetPersonalities();
				preview.BirthFeatureId = temporaryCharacter.GetGroupFeature(183);
				preview.FeatureIds = new List<short>();
				preview.BaseMainAttributes = deadCharacter.BaseMainAttributes;
				preview.BaseLifeSkillQualifications = deadCharacter.BaseLifeSkillQualifications;
				preview.BaseCombatSkillQualifications = deadCharacter.BaseCombatSkillQualifications;
				preview.ConsummateLevel = temporaryCharacter.GetConsummateLevel();
				preview.NeiliAllocation = temporaryCharacter.GetNeiliAllocation();
				preview.CurrNeili = temporaryCharacter.GetCurrNeili();
				preview.BaseNeiliProportionOfFiveElements = temporaryCharacter.GetBaseNeiliProportionOfFiveElements();
				preview.CharacterSamsaraData = DomainManager.Character.GetCharacterSamsaraData(this._temporaryPossessionCharId);
				foreach (short featureId in temporaryCharacter.GetFeatureIds())
				{
					bool flag2 = CharacterFeature.Instance[featureId].MutexGroupId != 183;
					if (flag2)
					{
						preview.FeatureIds.Add(featureId);
					}
				}
				result = preview;
			}
			return result;
		}

		// Token: 0x06007D64 RID: 32100 RVA: 0x004A9044 File Offset: 0x004A7244
		public byte GetPossessionResult(int soulCharId, int bodyCharId, out DeadCharacter deadCharacter, out GameData.Domains.Character.Character bodyCharacter, bool mustGetKidnapped = false)
		{
			deadCharacter = null;
			bodyCharacter = null;
			bool flag = !DomainManager.Character.TryGetDeadCharacter(soulCharId, out deadCharacter);
			byte result;
			if (flag)
			{
				result = PossessionResult.InvalidSoulCharId;
			}
			else
			{
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(bodyCharId, out bodyCharacter);
				if (flag2)
				{
					result = PossessionResult.InvalidBodyCharId;
				}
				else
				{
					if (mustGetKidnapped)
					{
						KidnappedCharacterList list;
						bool flag3 = !DomainManager.Character.TryGetKidnappedCharacters(DomainManager.Taiwu.GetTaiwuCharId(), out list);
						if (flag3)
						{
							return PossessionResult.BodyCharNotKidnappedByTaiwu;
						}
						bool isBodyCharKidnappedByTaiwu = false;
						foreach (KidnappedCharacter victim in list.GetCollection())
						{
							bool flag4 = victim.CharId == bodyCharId;
							if (flag4)
							{
								isBodyCharKidnappedByTaiwu = true;
								break;
							}
						}
						bool flag5 = !isBodyCharKidnappedByTaiwu;
						if (flag5)
						{
							return PossessionResult.BodyCharNotKidnappedByTaiwu;
						}
					}
					result = PossessionResult.Success;
				}
			}
			return result;
		}

		// Token: 0x06007D65 RID: 32101 RVA: 0x004A9144 File Offset: 0x004A7344
		[DomainMethod]
		public void SetTemporaryPossessionCharacterAvatar(DataContext context, AvatarData avatar, AvatarExtraData avatarExtraData)
		{
			GameData.Domains.Character.Character character;
			bool flag = this._temporaryPossessionCharId >= 0 && DomainManager.Character.TryGetElement_Objects(this._temporaryPossessionCharId, out character);
			if (flag)
			{
				DomainManager.Extra.TryAddCharacterAvatarExtraData(context, this._temporaryPossessionCharId, avatarExtraData);
				character.SetAvatar(avatar, context);
			}
		}

		// Token: 0x06007D66 RID: 32102 RVA: 0x004A9194 File Offset: 0x004A7394
		public void RemoveTemporaryPossessionCharacter(DataContext context)
		{
			GameData.Domains.Character.Character character;
			bool flag = this._temporaryPossessionCharId >= 0 && DomainManager.Character.TryGetElement_Objects(this._temporaryPossessionCharId, out character);
			if (flag)
			{
				DomainManager.Extra.TryRemoveCharacterAvatarExtraData(context, this._temporaryPossessionCharId);
				DomainManager.Character.RemoveTemporaryIntelligentCharacter(context, character);
				this._temporaryPossessionCharId = -1;
			}
		}

		// Token: 0x06007D67 RID: 32103 RVA: 0x004A91EC File Offset: 0x004A73EC
		[DomainMethod]
		public SamsaraPlatformRecordCollection GetSamsaraPlatformRecord()
		{
			return DomainManager.Extra.GetSamsaraPlatformRecordCollection();
		}

		// Token: 0x06007D68 RID: 32104 RVA: 0x004A91F8 File Offset: 0x004A73F8
		[DomainMethod]
		public ShopEventCollection GetOrCreateShopEventCollection(BuildingBlockKey buildingBlockKey)
		{
			if (this._shopEventCollections == null)
			{
				this._shopEventCollections = new Dictionary<BuildingBlockKey, ShopEventCollection>();
			}
			ShopEventCollection shopEventCollection;
			bool flag = this._shopEventCollections.TryGetValue(buildingBlockKey, out shopEventCollection);
			ShopEventCollection result;
			if (flag)
			{
				result = shopEventCollection;
			}
			else
			{
				shopEventCollection = new ShopEventCollection();
				this._shopEventCollections.Add(buildingBlockKey, shopEventCollection);
				result = shopEventCollection;
			}
			return result;
		}

		// Token: 0x06007D69 RID: 32105 RVA: 0x004A9248 File Offset: 0x004A7448
		[DomainMethod]
		public bool HasShopManagerLeader(BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData;
			bool flag = !this._buildingBlocks.TryGetValue(blockKey, out blockData) || blockData.TemplateId < 0;
			return !flag && (this.GetShopManagerLeader(blockKey) != null || !blockData.ConfigData.NeedLeader);
		}

		// Token: 0x06007D6A RID: 32106 RVA: 0x004A9298 File Offset: 0x004A7498
		public GameData.Domains.Character.Character GetShopManagerLeader(BuildingBlockKey blockKey)
		{
			CharacterList characterList;
			bool flag = !this._shopManagerDict.TryGetValue(blockKey, out characterList) || characterList.GetCount() == 0;
			GameData.Domains.Character.Character result;
			if (flag)
			{
				result = null;
			}
			else
			{
				List<int> collection = characterList.GetCollection();
				int charId = collection[0];
				GameData.Domains.Character.Character character;
				result = (DomainManager.Character.TryGetElement_Objects(charId, out character) ? character : null);
			}
			return result;
		}

		// Token: 0x06007D6B RID: 32107 RVA: 0x004A92F8 File Offset: 0x004A74F8
		[DomainMethod]
		public void SetShopManager(DataContext context, BuildingBlockKey blockKey, sbyte index, int charId)
		{
			bool allowChild = index != 0;
			bool flag = this._shopManagerDict.ContainsKey(blockKey);
			if (flag)
			{
				int currCharId = this._shopManagerDict[blockKey].GetCollection()[(int)index];
				bool flag2 = currCharId == -1 && charId == -1;
				if (flag2)
				{
					return;
				}
				bool flag3 = currCharId == charId;
				if (flag3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Same shop manager already exist, id: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag4 = currCharId >= 0;
				if (flag4)
				{
					DomainManager.Taiwu.RemoveVillagerWork(context, currCharId, true);
				}
			}
			bool flag5 = charId >= 0;
			if (flag5)
			{
				VillagerWorkData workData = new VillagerWorkData(charId, 1, blockKey.AreaId, blockKey.BlockId);
				workData.BuildingBlockIndex = blockKey.BuildingBlockIndex;
				workData.WorkerIndex = index;
				DomainManager.Taiwu.SetVillagerWork(context, charId, workData, allowChild);
			}
			BuildingBlockData blockData;
			bool flag6 = DomainManager.Building.TryGetElement_BuildingBlocks(blockKey, out blockData);
			if (flag6)
			{
				BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
				bool flag7 = GameData.Domains.Building.SharedMethods.HasEffect(config);
				if (flag7)
				{
					this._needUpdateEffects = true;
				}
			}
		}

		// Token: 0x06007D6C RID: 32108 RVA: 0x004A9430 File Offset: 0x004A7630
		[DomainMethod]
		public void SetCollectBuildingResourceType(DataContext context, BuildingBlockKey blockKey, sbyte resourceType)
		{
			bool flag = this._CollectBuildingResourceType.ContainsKey(blockKey);
			if (flag)
			{
				this.SetElement_CollectBuildingResourceType(blockKey, resourceType, context);
			}
			else
			{
				this.AddElement_CollectBuildingResourceType(blockKey, resourceType, context);
			}
		}

		// Token: 0x06007D6D RID: 32109 RVA: 0x004A9464 File Offset: 0x004A7664
		[DomainMethod]
		public void ClearBuildingBlockEarningsData(DataContext context, BuildingBlockKey key, bool isPawnShop)
		{
			BuildingEarningsData data;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(key, out data);
			if (flag)
			{
				foreach (ItemKey itemKey in data.ShopSoldItemList)
				{
					bool flag2 = itemKey.IsValid();
					if (flag2)
					{
						DomainManager.Item.GetBaseItem(itemKey).ResetOwner();
						DomainManager.Taiwu.WarehouseAdd(context, itemKey, 1);
					}
				}
				foreach (ItemKey itemKey2 in data.FixBookInfoList)
				{
					bool flag3 = itemKey2.IsValid();
					if (flag3)
					{
						DomainManager.Item.GetBaseItem(itemKey2).ResetOwner();
						DomainManager.Taiwu.WarehouseAdd(context, itemKey2, 1);
					}
				}
				bool flag4 = !isPawnShop;
				if (flag4)
				{
					foreach (ItemKey itemKey3 in data.CollectionItemList)
					{
						bool flag5 = itemKey3.IsValid();
						if (flag5)
						{
							DomainManager.Item.GetBaseItem(itemKey3).ResetOwner();
							ItemSourceType sourceType = this.ApplyBuildingItemOutputSetting(key, itemKey3);
							DomainManager.Taiwu.AddItem(context, itemKey3, 1, sourceType, false);
						}
					}
				}
				foreach (IntPair earn in data.ShopSoldItemEarnList)
				{
					bool flag6 = earn.First == 6;
					if (flag6)
					{
						DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 6, earn.Second);
						this.ApplyBuildingResourceOutputSetting(context, key, (sbyte)earn.First, earn.Second);
					}
					bool flag7 = earn.First == 7;
					if (flag7)
					{
						DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 7, earn.Second);
					}
				}
				foreach (IntPair earn2 in data.CollectionResourceList)
				{
					bool flag8 = earn2.First == 6;
					if (flag8)
					{
						DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 6, earn2.Second);
						this.ApplyBuildingResourceOutputSetting(context, key, (sbyte)earn2.First, earn2.Second);
					}
					bool flag9 = earn2.First == 7;
					if (flag9)
					{
						DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 7, earn2.Second);
					}
				}
			}
			bool flag10 = this._shopManagerDict.ContainsKey(key);
			if (flag10)
			{
				this.RemoveElement_ShopManagerDict(key, context);
			}
			bool flag11 = this._collectBuildingEarningsData.ContainsKey(key);
			if (flag11)
			{
				this.RemoveElement_CollectBuildingEarningsData(key, context);
			}
			this.RemoveBuildingResourceOutputSetting(context, (int)key.BuildingBlockIndex);
		}

		// Token: 0x06007D6E RID: 32110 RVA: 0x004A9790 File Offset: 0x004A7990
		private BuildingOptionAutoGiveMemberPreset GetArrangementSetting(BuildingBlockKey blockKey)
		{
			BuildingBlockDataEx extraData;
			DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)blockKey, out extraData);
			return ((extraData != null) ? extraData.ArrangementSetting : null) ?? new BuildingOptionAutoGiveMemberPreset();
		}

		// Token: 0x06007D6F RID: 32111 RVA: 0x004A97CC File Offset: 0x004A79CC
		private void GetBaseArrangementVillagerList(BuildingBlockKey blockKey, out List<int> roleVillagersForWork, out List<int> noRoleVillagersForWork)
		{
			BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
			List<int> villagersForWork = DomainManager.Taiwu.GetVillagersForWork(true, false);
			List<int> haveRoleVillagerIds = new List<int>();
			bool flag = blockData.ConfigData.VillagerRoleTemplateIds != null;
			if (flag)
			{
				foreach (short villagerRoleTemplateId in blockData.ConfigData.VillagerRoleTemplateIds)
				{
					DomainManager.Extra.GetVillagerRoleCharactersByTemplateId(villagerRoleTemplateId, ref haveRoleVillagerIds);
				}
			}
			roleVillagersForWork = new List<int>();
			noRoleVillagersForWork = new List<int>();
			foreach (int charId in villagersForWork)
			{
				bool flag2 = haveRoleVillagerIds.Contains(charId);
				if (flag2)
				{
					roleVillagersForWork.Add(charId);
				}
				else
				{
					noRoleVillagersForWork.Add(charId);
				}
			}
		}

		// Token: 0x06007D70 RID: 32112 RVA: 0x004A98B8 File Offset: 0x004A7AB8
		private void SortByAttainment(BuildingBlockItem buildingConfig, ref List<int> ids)
		{
			ids.Sort(delegate(int idLeft, int idRight)
			{
				GameData.Domains.Character.Character characterLeft = DomainManager.Character.GetElement_Objects(idLeft);
				GameData.Domains.Character.Character characterRight = DomainManager.Character.GetElement_Objects(idRight);
				bool flag = buildingConfig.RequireLifeSkillType >= 0;
				int result;
				if (flag)
				{
					result = (int)(characterRight.GetLifeSkillAttainment(buildingConfig.RequireLifeSkillType) - characterLeft.GetLifeSkillAttainment(buildingConfig.RequireLifeSkillType));
				}
				else
				{
					bool flag2 = buildingConfig.RequireCombatSkillType >= 0;
					if (flag2)
					{
						result = (int)(characterRight.GetCombatSkillAttainment(buildingConfig.RequireCombatSkillType) - characterLeft.GetCombatSkillAttainment(buildingConfig.RequireCombatSkillType));
					}
					else
					{
						result = 0;
					}
				}
				return result;
			});
		}

		// Token: 0x06007D71 RID: 32113 RVA: 0x004A98E8 File Offset: 0x004A7AE8
		private void SortByQualifications(BuildingBlockItem buildingConfig, ref List<int> ids, bool descending = true)
		{
			ids.Sort(delegate(int idLeft, int idRight)
			{
				GameData.Domains.Character.Character characterLeft = DomainManager.Character.GetElement_Objects(idLeft);
				GameData.Domains.Character.Character characterRight = DomainManager.Character.GetElement_Objects(idRight);
				int value = 0;
				bool flag = buildingConfig.RequireLifeSkillType >= 0;
				if (flag)
				{
					value = (int)(characterRight.GetLifeSkillQualification(buildingConfig.RequireLifeSkillType) - characterLeft.GetLifeSkillQualification(buildingConfig.RequireLifeSkillType));
				}
				else
				{
					bool flag2 = buildingConfig.RequireCombatSkillType >= 0;
					if (flag2)
					{
						value = (int)(characterRight.GetCombatSkillQualification(buildingConfig.RequireCombatSkillType) - characterLeft.GetCombatSkillQualification(buildingConfig.RequireCombatSkillType));
					}
				}
				return descending ? value : (-value);
			});
		}

		// Token: 0x06007D72 RID: 32114 RVA: 0x004A9920 File Offset: 0x004A7B20
		private void SortByReadBookTotalValue(BuildingBlockItem buildingConfig, ref List<int> ids, bool descending = true)
		{
			ids.Sort(delegate(int idLeft, int idRight)
			{
				GameData.Domains.Character.Character characterLeft = DomainManager.Character.GetElement_Objects(idLeft);
				GameData.Domains.Character.Character characterRight = DomainManager.Character.GetElement_Objects(idRight);
				int value = 0;
				bool flag = buildingConfig.RequireLifeSkillType >= 0;
				if (flag)
				{
					value = characterRight.GetLearnedLifeSkillTotalValue(buildingConfig.RequireLifeSkillType) - characterLeft.GetLearnedLifeSkillTotalValue(buildingConfig.RequireLifeSkillType);
				}
				else
				{
					bool flag2 = buildingConfig.RequireCombatSkillType >= 0;
					if (flag2)
					{
						value = characterRight.GetLearnedCombatSkillTotalValue(buildingConfig.RequireCombatSkillType) - characterLeft.GetLearnedCombatSkillTotalValue(buildingConfig.RequireCombatSkillType);
					}
				}
				return descending ? value : (-value);
			});
		}

		// Token: 0x06007D73 RID: 32115 RVA: 0x004A9958 File Offset: 0x004A7B58
		private void GetLeaderArrangementVillagerList(BuildingBlockKey blockKey, out List<int> roleVillagersForWork, out List<int> noRoleVillagersForWork)
		{
			BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
			BuildingOptionAutoGiveMemberPreset presetData = this.GetArrangementSetting(blockKey);
			bool flag = !presetData.GetIsInfluenceLeader();
			if (flag)
			{
				roleVillagersForWork = null;
				noRoleVillagersForWork = null;
			}
			else
			{
				this.GetBaseArrangementVillagerList(blockKey, out roleVillagersForWork, out noRoleVillagersForWork);
				switch (presetData.PickRuleForLeader)
				{
				case 0:
					this.SortByAttainment(blockData.ConfigData, ref roleVillagersForWork);
					this.SortByAttainment(blockData.ConfigData, ref noRoleVillagersForWork);
					break;
				case 1:
					this.SortByQualifications(blockData.ConfigData, ref roleVillagersForWork, true);
					this.SortByQualifications(blockData.ConfigData, ref noRoleVillagersForWork, true);
					break;
				case 2:
					this.SortByReadBookTotalValue(blockData.ConfigData, ref roleVillagersForWork, true);
					this.SortByReadBookTotalValue(blockData.ConfigData, ref noRoleVillagersForWork, true);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				BuildingOptionAutoGiveMemberPreset.RoleRule roleRuleForLeader = (BuildingOptionAutoGiveMemberPreset.RoleRule)presetData.RoleRuleForLeader;
				bool onlyRole = roleRuleForLeader.HasFlag(BuildingOptionAutoGiveMemberPreset.RoleRule.OnlyRole);
				bool flag2 = onlyRole;
				if (flag2)
				{
					noRoleVillagersForWork.Clear();
				}
			}
		}

		// Token: 0x06007D74 RID: 32116 RVA: 0x004A9A54 File Offset: 0x004A7C54
		private void GetMemberArrangementVillagerList(BuildingBlockKey blockKey, out List<int> roleVillagersForWork, out List<int> noRoleVillagersForWork)
		{
			BuildingDomain.<>c__DisplayClass455_0 CS$<>8__locals1 = new BuildingDomain.<>c__DisplayClass455_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.blockKey = blockKey;
			BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(CS$<>8__locals1.blockKey);
			BuildingOptionAutoGiveMemberPreset presetData = this.GetArrangementSetting(CS$<>8__locals1.blockKey);
			bool flag = !presetData.GetIsInfluenceMember();
			if (flag)
			{
				roleVillagersForWork = null;
				noRoleVillagersForWork = null;
			}
			else
			{
				this.GetBaseArrangementVillagerList(CS$<>8__locals1.blockKey, out roleVillagersForWork, out noRoleVillagersForWork);
				BuildingOptionAutoGiveMemberPreset.RoleRule roleRuleForMember = (BuildingOptionAutoGiveMemberPreset.RoleRule)presetData.RoleRuleForMember;
				bool flag2 = roleRuleForMember.HasFlag(BuildingOptionAutoGiveMemberPreset.RoleRule.AllowChild);
				if (flag2)
				{
					List<int> childAvailableForWork = DomainManager.Taiwu.GetAllChildAvailableForWork(true);
					noRoleVillagersForWork.AddRange(childAvailableForWork);
				}
				bool flag3 = roleRuleForMember.HasFlag(BuildingOptionAutoGiveMemberPreset.RoleRule.NotAllowRole);
				if (flag3)
				{
					roleVillagersForWork.Clear();
					noRoleVillagersForWork.RemoveAll((int id) => DomainManager.Extra.GetVillagerRole(id) != null);
				}
				bool flag4 = !roleRuleForMember.HasFlag(BuildingOptionAutoGiveMemberPreset.RoleRule.AllowNoPotential);
				if (flag4)
				{
					roleVillagersForWork.RemoveAll((int id) => DomainManager.Taiwu.GetTaiwuVillagerLeftPotentialCount(id) == 0);
					noRoleVillagersForWork.RemoveAll((int id) => DomainManager.Taiwu.GetTaiwuVillagerLeftPotentialCount(id) == 0);
				}
				bool flag5 = !roleRuleForMember.HasFlag(BuildingOptionAutoGiveMemberPreset.RoleRule.AllowNoReadableBook);
				if (flag5)
				{
					roleVillagersForWork.RemoveAll(new Predicate<int>(CS$<>8__locals1.<GetMemberArrangementVillagerList>g__Match|3));
					noRoleVillagersForWork.RemoveAll(new Predicate<int>(CS$<>8__locals1.<GetMemberArrangementVillagerList>g__Match|3));
				}
				switch (presetData.PickRuleForMember)
				{
				case 0:
					this.SortByAttainment(blockData.ConfigData, ref roleVillagersForWork);
					this.SortByAttainment(blockData.ConfigData, ref noRoleVillagersForWork);
					break;
				case 1:
					this.SortByQualifications(blockData.ConfigData, ref roleVillagersForWork, false);
					this.SortByQualifications(blockData.ConfigData, ref noRoleVillagersForWork, false);
					break;
				case 2:
					this.SortByReadBookTotalValue(blockData.ConfigData, ref roleVillagersForWork, false);
					this.SortByReadBookTotalValue(blockData.ConfigData, ref noRoleVillagersForWork, false);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Token: 0x06007D75 RID: 32117 RVA: 0x004A9C78 File Offset: 0x004A7E78
		[DomainMethod]
		public List<int> QuickArrangeShopManager(DataContext context, BuildingBlockKey blockKey, bool onlyCheck = false)
		{
			BuildingDomain.<>c__DisplayClass456_0 CS$<>8__locals1 = new BuildingDomain.<>c__DisplayClass456_0();
			CS$<>8__locals1.onlyCheck = onlyCheck;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.blockKey = blockKey;
			CS$<>8__locals1.presetData = this.GetArrangementSetting(CS$<>8__locals1.blockKey);
			CS$<>8__locals1.charIds = new List<int>();
			for (sbyte index = 0; index < 7; index += 1)
			{
				CS$<>8__locals1.charIds.Add(-1);
				bool flag = !CS$<>8__locals1.presetData.GetIsInfluenceLeader() && index == 0;
				if (!flag)
				{
					bool flag2 = !CS$<>8__locals1.presetData.GetIsInfluenceMember() && index != 0;
					if (!flag2)
					{
						bool onlyCheck2 = CS$<>8__locals1.onlyCheck;
						if (!onlyCheck2)
						{
							this.SetShopManager(CS$<>8__locals1.context, CS$<>8__locals1.blockKey, index, -1);
						}
					}
				}
			}
			bool isInfluenceLeader = CS$<>8__locals1.presetData.GetIsInfluenceLeader();
			if (isInfluenceLeader)
			{
				List<int> roleVillagersForWork;
				List<int> noRoleVillagersForWork;
				this.GetLeaderArrangementVillagerList(CS$<>8__locals1.blockKey, out roleVillagersForWork, out noRoleVillagersForWork);
				bool unlockChar = !CS$<>8__locals1.presetData.LockCharForLeader;
				CS$<>8__locals1.<QuickArrangeShopManager>g__FillLeaders|0(ref roleVillagersForWork, ref CS$<>8__locals1.charIds, unlockChar);
				CS$<>8__locals1.<QuickArrangeShopManager>g__FillLeaders|0(ref noRoleVillagersForWork, ref CS$<>8__locals1.charIds, unlockChar);
			}
			bool isInfluenceMember = CS$<>8__locals1.presetData.GetIsInfluenceMember();
			if (isInfluenceMember)
			{
				List<int> roleVillagersForWork2;
				List<int> noRoleVillagersForWork2;
				this.GetMemberArrangementVillagerList(CS$<>8__locals1.blockKey, out roleVillagersForWork2, out noRoleVillagersForWork2);
				int leaderIndex = 0;
				roleVillagersForWork2.RemoveAll((int id) => CS$<>8__locals1.charIds[leaderIndex] == id);
				noRoleVillagersForWork2.RemoveAll((int id) => CS$<>8__locals1.charIds[leaderIndex] == id);
				bool unlockChar2 = !CS$<>8__locals1.presetData.LockCharForMember;
				CS$<>8__locals1.<QuickArrangeShopManager>g__FillMembers|1(ref roleVillagersForWork2, ref CS$<>8__locals1.charIds, unlockChar2);
				CS$<>8__locals1.<QuickArrangeShopManager>g__FillMembers|1(ref noRoleVillagersForWork2, ref CS$<>8__locals1.charIds, unlockChar2);
			}
			return CS$<>8__locals1.charIds;
		}

		// Token: 0x06007D76 RID: 32118 RVA: 0x004A9E60 File Offset: 0x004A8060
		[DomainMethod]
		public bool CanQuickArrangeShopManager(BuildingBlockKey blockKey)
		{
			CharacterList characterList;
			this._shopManagerDict.TryGetValue(blockKey, out characterList);
			List<int> collection = characterList.GetCollection();
			List<int> tempList = this.QuickArrangeShopManager(null, blockKey, true);
			bool flag = tempList == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (int id in tempList)
				{
					bool flag2 = id >= 0 && id != collection.GetOrDefault(-1);
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007D77 RID: 32119 RVA: 0x004A9F04 File Offset: 0x004A8104
		[DomainMethod]
		public List<int> QuickArrangeBuildOperator(DataContext context, short buildingTemplateId, BuildingBlockKey blockKey, sbyte operationType)
		{
			bool flag = operationType == 0;
			int needValue;
			if (flag)
			{
				BuildingBlockItem buildingConfig = BuildingBlock.Instance[buildingTemplateId];
				needValue = (int)buildingConfig.OperationTotalProgress[(int)operationType];
			}
			else
			{
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				BuildingBlockItem buildingConfig = BuildingBlock.Instance[blockData.TemplateId];
				needValue = (int)(buildingConfig.OperationTotalProgress[(int)operationType] - blockData.OperationProgress);
			}
			List<int> charIds = new List<int>();
			for (sbyte index = 0; index < 3; index += 1)
			{
				charIds.Add(-1);
			}
			List<int> villagersForWork = DomainManager.Taiwu.GetVillagersForWork(true, false);
			villagersForWork.Sort(delegate(int leftId, int rightId)
			{
				GameData.Domains.Character.Character leftCharacter = DomainManager.Character.GetElement_Objects(leftId);
				GameData.Domains.Character.Character rightCharacter = DomainManager.Character.GetElement_Objects(rightId);
				return leftCharacter.GetPersonalities().GetSum() - rightCharacter.GetPersonalities().GetSum();
			});
			this.<QuickArrangeBuildOperator>g__FindMinSumCombination|458_1(villagersForWork, needValue, ref charIds);
			return charIds;
		}

		// Token: 0x06007D78 RID: 32120 RVA: 0x004A9FD8 File Offset: 0x004A81D8
		[DomainMethod]
		public int GetOperationLeftTime(DataContext context, short buildingTemplateId, BuildingBlockKey blockKey, sbyte operationType, List<int> operatorList)
		{
			int needValue = 0;
			BuildingBlockData blockData;
			int progress = (int)(DomainManager.Building.TryGetElement_BuildingBlocks(blockKey, out blockData) ? blockData.OperationProgress : 0);
			BuildingBlockItem buildingConfig = BuildingBlock.Instance[buildingTemplateId];
			needValue = (int)buildingConfig.OperationTotalProgress[(int)operationType] - progress;
			int sumValue = 0;
			foreach (int charId in operatorList)
			{
				bool flag = charId < 0;
				if (!flag)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					int personalitySum = character.GetPersonalities().GetSum();
					sumValue += (int)this.BaseWorkContribution + personalitySum;
				}
			}
			return (int)Math.Ceiling((double)((float)needValue / (float)sumValue));
		}

		// Token: 0x06007D79 RID: 32121 RVA: 0x004AA0AC File Offset: 0x004A82AC
		[DomainMethod]
		public int GetBuildingOperationLeftTime(DataContext context, BuildingBlockKey blockKey, sbyte operationType)
		{
			BuildingBlockData blockData;
			bool haveBlockData = DomainManager.Building.TryGetElement_BuildingBlocks(blockKey, out blockData);
			bool flag = !haveBlockData;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				BuildingBlockItem buildingConfig = BuildingBlock.Instance[blockData.TemplateId];
				int needValue = (int)(buildingConfig.OperationTotalProgress[(int)operationType] - blockData.OperationProgress);
				CharacterList operatorDict;
				bool haveOperate = DomainManager.Building.TryGetElement_BuildingOperatorDict(blockKey, out operatorDict);
				bool flag2 = !haveOperate;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					int sumValue = 0;
					foreach (int charId in operatorDict.GetCollection())
					{
						bool flag3 = charId < 0;
						if (!flag3)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							int personalitySum = character.GetPersonalities().GetSum();
							sumValue += (int)this.BaseWorkContribution + personalitySum;
						}
					}
					bool flag4 = sumValue == 0;
					if (flag4)
					{
						result = -1;
					}
					else
					{
						result = (int)Math.Ceiling((double)((float)needValue / (float)sumValue));
					}
				}
			}
			return result;
		}

		// Token: 0x06007D7A RID: 32122 RVA: 0x004AA1C4 File Offset: 0x004A83C4
		public int GetOperationSumValue(List<int> operatorList)
		{
			int sumValue = 0;
			foreach (int charId in operatorList)
			{
				bool flag = charId < 0 || !DomainManager.Taiwu.CanWork(charId);
				if (!flag)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					int personalitySum = character.GetPersonalities().GetSum();
					sumValue += (int)this.BaseWorkContribution + personalitySum;
				}
			}
			return sumValue;
		}

		// Token: 0x06007D7B RID: 32123 RVA: 0x004AA260 File Offset: 0x004A8460
		[DomainMethod]
		public bool ShopBuildingCanTeach(BuildingBlockKey blockKey)
		{
			CharacterList shopManagerDict;
			bool flag = this.TryGetElement_ShopManagerDict(blockKey, out shopManagerDict);
			bool result;
			if (flag)
			{
				BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
				int leaderId = shopManagerDict[0];
				bool flag2 = leaderId == -1;
				if (flag2)
				{
					result = false;
				}
				else
				{
					VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(leaderId);
					bool flag3 = villagerRole == null;
					if (flag3)
					{
						result = false;
					}
					else
					{
						BuildingBlockItem buildingConfig = BuildingBlock.Instance[blockData.TemplateId];
						result = buildingConfig.VillagerRoleTemplateIds.Contains(villagerRole.RoleTemplateId);
					}
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06007D7C RID: 32124 RVA: 0x004AA2EC File Offset: 0x004A84EC
		[DomainMethod]
		public unsafe ShopBuildingTeachBookData GetShopBuildingTeachBookData(BuildingBlockKey blockKey, int memberId)
		{
			ShopBuildingTeachBookData data = ShopBuildingTeachBookData.CreateDefault();
			bool flag = !this.HasShopManagerLeader(blockKey);
			ShopBuildingTeachBookData result;
			if (flag)
			{
				data.TeachBookResult = 1;
				result = data;
			}
			else
			{
				bool flag2 = !this.ShopBuildingCanTeach(blockKey);
				if (flag2)
				{
					data.TeachBookResult = 2;
					result = data;
				}
				else
				{
					BuildingBlockItem buildingConfig = BuildingBlock.Instance[this.GetElement_BuildingBlocks(blockKey).TemplateId];
					GameData.Domains.Character.Character memberCharacter = DomainManager.Character.GetElement_Objects(memberId);
					sbyte requirePersonalityValue = *memberCharacter.GetPersonalities()[(int)buildingConfig.RequirePersonalityType];
					short memberAttainment = (buildingConfig.RequireLifeSkillType >= 0) ? memberCharacter.GetLifeSkillAttainment(buildingConfig.RequireLifeSkillType) : memberCharacter.GetCombatSkillAttainment(buildingConfig.RequireCombatSkillType);
					int point = (int)(memberAttainment * (short)(100 + requirePersonalityValue) / 100);
					GameData.Domains.Character.Character leader = this.GetShopManagerLeader(blockKey);
					bool flag3 = buildingConfig.RequireLifeSkillType >= 0;
					if (flag3)
					{
						bool hasMemberUnread;
						List<ValueTuple<short, byte>> readBooks = this.ShopBuildingTeachLifeSkillBook(leader, memberCharacter, point, buildingConfig.TemplateId, out hasMemberUnread);
						data.TeachBookResult = ((readBooks.Count > 0) ? 0 : (hasMemberUnread ? 4 : 3));
						bool flag4 = readBooks.Count > 0;
						if (flag4)
						{
							foreach (ValueTuple<short, byte> valueTuple in readBooks)
							{
								short skillTemplateId = valueTuple.Item1;
								byte pageId = valueTuple.Item2;
								Config.LifeSkillItem skillConfig = LifeSkill.Instance[skillTemplateId];
								data.TeachBookInfo.Add(new ValueTuple<short, byte, sbyte>(skillConfig.SkillBookId, pageId, -1));
							}
						}
					}
					else
					{
						bool hasMemberUnread2;
						List<ValueTuple<short, byte, sbyte>> readBooks2 = this.ShopBuildingTeachCombatSkillBook(leader, memberCharacter, point, buildingConfig.TemplateId, out hasMemberUnread2);
						data.TeachBookResult = ((readBooks2.Count > 0) ? 0 : (hasMemberUnread2 ? 4 : 3));
						bool flag5 = readBooks2.Count > 0;
						if (flag5)
						{
							foreach (ValueTuple<short, byte, sbyte> valueTuple2 in readBooks2)
							{
								short skillTemplateId2 = valueTuple2.Item1;
								byte pageInternalIndex = valueTuple2.Item2;
								sbyte direct = valueTuple2.Item3;
								CombatSkillItem skillConfig2 = Config.CombatSkill.Instance[skillTemplateId2];
								byte pageId2 = CombatSkillStateHelper.GetPageId(pageInternalIndex);
								data.TeachBookInfo.Add(new ValueTuple<short, byte, sbyte>(skillConfig2.BookId, pageId2, direct));
							}
						}
					}
					result = data;
				}
			}
			return result;
		}

		// Token: 0x06007D7D RID: 32125 RVA: 0x004AA55C File Offset: 0x004A875C
		[DomainMethod]
		[Obsolete("Use CalcResourceOutputCount() replace", false)]
		public int CalcResourceOutput(DataContext context, BuildingBlockKey key)
		{
			return 0;
		}

		// Token: 0x06007D7E RID: 32126 RVA: 0x004AA570 File Offset: 0x004A8770
		public void CalcResourceBlockProductivityList(BuildingBlockKey key, [TupleElementNames(new string[]
		{
			null,
			"productivity"
		})] List<ValueTuple<BuildingBlockKey, int>> productivityList)
		{
			productivityList.Clear();
			List<ValueTuple<BuildingBlockKey, int>> distanceOneBlocks = new List<ValueTuple<BuildingBlockKey, int>>();
			this.BuildingBlockDependencies(key, delegate(BuildingBlockData dependency, int distance, BuildingBlockKey blockKey)
			{
				bool flag = distance == 1;
				if (flag)
				{
					distanceOneBlocks.Add(new ValueTuple<BuildingBlockKey, int>(blockKey, (int)this.BuildingBlockLevel(blockKey)));
				}
				else
				{
					bool flag2 = distance == 2;
					if (flag2)
					{
						productivityList.Add(new ValueTuple<BuildingBlockKey, int>(blockKey, GlobalConfig.Instance.CollectResourceBuildingProductivityDistanceMore));
					}
				}
			});
			distanceOneBlocks.Sort(([TupleElementNames(new string[]
			{
				"key",
				"level"
			})] ValueTuple<BuildingBlockKey, int> a, [TupleElementNames(new string[]
			{
				"key",
				"level"
			})] ValueTuple<BuildingBlockKey, int> b) => b.Item2.CompareTo(a.Item2));
			int[] distanceOneProductivityConfig = GlobalConfig.Instance.CollectResourceBuildingProductivityDistanceOne;
			for (int index = distanceOneBlocks.Count - 1; index >= 0; index--)
			{
				ValueTuple<BuildingBlockKey, int> distanceOneBlock = distanceOneBlocks[index];
				int productivity = distanceOneProductivityConfig[index];
				productivityList.Add(new ValueTuple<BuildingBlockKey, int>(distanceOneBlock.Item1, productivity));
			}
			productivityList.Reverse();
		}

		// Token: 0x06007D7F RID: 32127 RVA: 0x004AA650 File Offset: 0x004A8850
		public int CalcResourceOutputCountBySingleSpecifiedDependency(BuildingBlockKey key, sbyte resourceType, BuildingBlockData dependency, int productivity, out BuildingProduceDependencyData dependencyData)
		{
			dependencyData = BuildingProduceDependencyData.Invalid;
			BuildingBlockData blockData;
			bool flag = this.TryGetElement_BuildingBlocks(key, out blockData);
			int result;
			if (flag)
			{
				BuildingBlockKey dependencyKey = new BuildingBlockKey(key.AreaId, key.BlockId, dependency.BlockIndex);
				dependencyData.Level = this.BuildingBlockLevel(dependencyKey);
				dependencyData.TemplateId = dependency.TemplateId;
				dependencyData.BlockBaseYieldFactor = this.BuildingBaseYield(dependency).Get((int)Math.Min(resourceType, 5));
				dependencyData.ResourceYieldLevelFactor = this.BuildingResourceYieldLevel(dependencyKey, dependency.TemplateId);
				dependencyData.ProductivityFactor = productivity;
				bool hasManager;
				dependencyData.TotalAttainmentFactor = this.BuildingTotalAttainment(key, resourceType, out hasManager, false);
				dependencyData.GainResourcePercentFactor = DomainManager.World.GetGainResourcePercent(1);
				dependencyData.ResourceSingleOutputValuation = dependencyData.ResourceBuildingOutput;
				CValuePercentBonus resBuildingBonus = DomainManager.Building.GetBuildingBlockEffect(key.GetLocation(), EBuildingScaleEffect.BuildingMaterialResourceIncomeBonus, -1);
				dependencyData.ResourceSingleOutputValuation *= resBuildingBonus;
				bool flag2 = !hasManager;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = dependencyData.ResourceSingleOutputValuation;
				}
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06007D80 RID: 32128 RVA: 0x004AA76C File Offset: 0x004A896C
		[DomainMethod]
		public int CalcResourceOutputCount(BuildingBlockKey key, sbyte resourceType)
		{
			BuildingBlockData blockData;
			bool flag = this.TryGetElement_BuildingBlocks(key, out blockData);
			int result;
			if (flag)
			{
				int totalCount = 0;
				List<ValueTuple<BuildingBlockKey, int>> productivityList = new List<ValueTuple<BuildingBlockKey, int>>();
				this.CalcResourceBlockProductivityList(key, productivityList);
				foreach (ValueTuple<BuildingBlockKey, int> element in productivityList)
				{
					BuildingProduceDependencyData buildingProduceDependencyData;
					totalCount += this.CalcResourceOutputCountBySingleSpecifiedDependency(key, resourceType, this.GetBuildingBlockData(element.Item1), element.Item2, out buildingProduceDependencyData);
				}
				result = totalCount;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06007D81 RID: 32129 RVA: 0x004AA804 File Offset: 0x004A8A04
		private int CalcResourceOutputCount(BuildingBlockKey key, sbyte resourceType, Dictionary<BuildingBlockKey, BuildingProduceDependencyData> dependencyDataDict)
		{
			BuildingBlockData blockData;
			bool flag = this.TryGetElement_BuildingBlocks(key, out blockData);
			int result;
			if (flag)
			{
				int totalCount = 0;
				List<ValueTuple<BuildingBlockKey, int>> productivityList = new List<ValueTuple<BuildingBlockKey, int>>();
				this.CalcResourceBlockProductivityList(key, productivityList);
				foreach (ValueTuple<BuildingBlockKey, int> element in productivityList)
				{
					BuildingProduceDependencyData dependencyData;
					totalCount += this.CalcResourceOutputCountBySingleSpecifiedDependency(key, resourceType, this.GetBuildingBlockData(element.Item1), element.Item2, out dependencyData);
					dependencyDataDict.Add(element.Item1, dependencyData);
				}
				result = totalCount;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06007D82 RID: 32130 RVA: 0x004AA8B0 File Offset: 0x004A8AB0
		[DomainMethod]
		public BuildingEarningsData GetBuildingEarningData(BuildingBlockKey blockKey)
		{
			BuildingEarningsData earningData;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(blockKey, out earningData);
			BuildingEarningsData result;
			if (flag)
			{
				result = earningData;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06007D83 RID: 32131 RVA: 0x004AA8D8 File Offset: 0x004A8AD8
		[DomainMethod]
		public List<int> GetBuildingOperatesData(BuildingBlockKey blockKey)
		{
			List<int> charIds = new List<int>();
			Dictionary<int, VillagerWorkData> villageWork = DomainManager.Taiwu.GetVillagerWorkDict();
			foreach (KeyValuePair<int, VillagerWorkData> keyValuePair in villageWork)
			{
				int num;
				VillagerWorkData villagerWorkData;
				keyValuePair.Deconstruct(out num, out villagerWorkData);
				int charId = num;
				VillagerWorkData workData = villagerWorkData;
				bool flag = workData.BuildingBlockIndex == blockKey.BuildingBlockIndex && workData.WorkType == 0;
				if (flag)
				{
					charIds.Add(charId);
				}
			}
			return charIds;
		}

		// Token: 0x06007D84 RID: 32132 RVA: 0x004AA97C File Offset: 0x004A8B7C
		[DomainMethod]
		public int GetBuildingBuildPeopleAttainments(BuildingBlockKey blockKey)
		{
			List<int> charIds = this.GetBuildingOperatesData(blockKey);
			int attainments = 0;
			for (int i = 0; i < charIds.Count; i++)
			{
				BuildingBlockData blockData;
				bool flag = this.TryGetElement_BuildingBlocks(blockKey, out blockData);
				if (flag)
				{
					attainments += (int)this.BaseWorkContribution;
					attainments += (int)DomainManager.Character.GetElement_Objects(charIds[i]).GetLifeSkillAttainment(BuildingBlock.Instance[blockData.TemplateId].RequireLifeSkillType);
				}
			}
			return attainments;
		}

		// Token: 0x06007D85 RID: 32133 RVA: 0x004AA9FC File Offset: 0x004A8BFC
		[DomainMethod]
		public void SetBuildingResourceOutputSetting(DataContext context, int blockIndex, BuildingResourceOutputSetting setting)
		{
			DomainManager.Extra.SetBuildingResourceOutputSetting(context, blockIndex, setting);
		}

		// Token: 0x06007D86 RID: 32134 RVA: 0x004AAA0C File Offset: 0x004A8C0C
		[DomainMethod]
		public BuildingResourceOutputSetting GetBuildingResourceOutputSetting(int blockIndex)
		{
			return DomainManager.Extra.GetBuildingResourceOutputSetting(blockIndex);
		}

		// Token: 0x06007D87 RID: 32135 RVA: 0x004AAA19 File Offset: 0x004A8C19
		public void RemoveBuildingResourceOutputSetting(DataContext context, int blockIndex)
		{
			DomainManager.Extra.RemoveBuildingResourceOutputSetting(context, blockIndex);
		}

		// Token: 0x06007D88 RID: 32136 RVA: 0x004AAA28 File Offset: 0x004A8C28
		public void SetCollectBuildingEarningsData(DataContext context, BuildingBlockKey blockKey, BuildingEarningsData data)
		{
			BuildingEarningsData buildingEarningsData;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(blockKey, out buildingEarningsData);
			if (flag)
			{
				this.SetElement_CollectBuildingEarningsData(blockKey, data, context);
			}
			else
			{
				this.AddElement_CollectBuildingEarningsData(blockKey, data, context);
			}
		}

		// Token: 0x06007D89 RID: 32137 RVA: 0x004AAA5C File Offset: 0x004A8C5C
		[DomainMethod]
		public BuildingBlockKey AcceptBuildingBlockCollectEarning(DataContext context, BuildingBlockKey key, int earningDataIndex, bool isPutInInventory, bool isSetData = true, bool isCostMoney = false)
		{
			BuildingEarningsData data;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(key, out data) && earningDataIndex >= 0;
			if (flag)
			{
				bool flag2 = earningDataIndex < data.CollectionItemList.Count;
				if (flag2)
				{
					ItemKey itemKey = data.CollectionItemList[earningDataIndex];
					itemKey.ModificationState = 0;
					data.CollectionItemList.RemoveAt(earningDataIndex);
					BuildingBlockData buildingBlock = this.GetBuildingBlockData(key);
					DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, (int)buildingBlock.TemplateId);
					if (isCostMoney)
					{
						this.ConsumeResource(context, 6, ItemTemplateHelper.GetBaseValue(itemKey.ItemType, itemKey.TemplateId));
					}
					ItemSourceType sourceType = this.ApplyBuildingItemOutputSetting(key, itemKey);
					DomainManager.Taiwu.AddItem(context, itemKey, 1, sourceType, false);
					DomainManager.Building.AddBuildingGainLegacy(context, key);
					this.BuildingChangeProfessionSeniority(context, itemKey);
				}
				bool flag3 = earningDataIndex < data.CollectionResourceList.Count;
				if (flag3)
				{
					IntPair resourceIntPair = data.CollectionResourceList[earningDataIndex];
					data.CollectionResourceList.RemoveAt(earningDataIndex - data.CollectionItemList.Count);
					DomainManager.Character.GetElement_Objects(DomainManager.Taiwu.GetTaiwuCharId()).ChangeResource(context, (sbyte)resourceIntPair.First, resourceIntPair.Second);
					this.ApplyBuildingResourceOutputSetting(context, key, (sbyte)resourceIntPair.First, resourceIntPair.Second);
					DomainManager.Building.AddBuildingGainLegacy(context, key);
				}
				if (isSetData)
				{
					this.SetElement_CollectBuildingEarningsData(key, data, context);
				}
			}
			return key;
		}

		// Token: 0x06007D8A RID: 32138 RVA: 0x004AABDC File Offset: 0x004A8DDC
		private void ApplyBuildingResourceOutputSetting(DataContext context, BuildingBlockKey key, sbyte resourceType, int amount)
		{
			bool isInvalid = key.IsInvalid;
			if (!isInvalid)
			{
				BuildingResourceOutputSetting buildingResourceOutputSetting = this.GetBuildingResourceOutputSetting((int)key.BuildingBlockIndex);
				sbyte storageType;
				bool flag = buildingResourceOutputSetting.ResourceStorage.TryGetValue(resourceType, out storageType) && storageType != -1;
				if (flag)
				{
					bool isMoney = resourceType == 6;
					ItemSourceType dest = BuildingResourceOutputSetting.GetItemSourceTypeByStorageType((TaiwuVillageStorageType)storageType, false, isMoney);
					DomainManager.Taiwu.TransferResource(context, ItemSourceType.Inventory, dest, resourceType, amount);
				}
			}
		}

		// Token: 0x06007D8B RID: 32139 RVA: 0x004AAC48 File Offset: 0x004A8E48
		public ItemSourceType ApplyBuildingItemOutputSetting(BuildingBlockKey key, ItemKey itemKey)
		{
			bool isInvalid = key.IsInvalid;
			ItemSourceType result;
			if (isInvalid)
			{
				result = ItemSourceType.Warehouse;
			}
			else
			{
				BuildingResourceOutputSetting buildingResourceOutputSetting = this.GetBuildingResourceOutputSetting((int)key.BuildingBlockIndex);
				sbyte resourceType = ItemTemplateHelper.GetResourceType(itemKey.ItemType, itemKey.TemplateId);
				sbyte storageType;
				bool flag = !buildingResourceOutputSetting.ItemStorage.TryGetValue(resourceType, out storageType);
				if (flag)
				{
					result = ItemSourceType.Warehouse;
				}
				else
				{
					bool isMaterialItem = itemKey.ItemType == 5;
					bool isMoney = resourceType == 6;
					ItemSourceType dest = BuildingResourceOutputSetting.GetItemSourceTypeByStorageType((TaiwuVillageStorageType)storageType, isMaterialItem, isMoney);
					bool flag2 = dest == ItemSourceType.Inventory;
					if (flag2)
					{
						result = ItemSourceType.Warehouse;
					}
					else
					{
						result = dest;
					}
				}
			}
			return result;
		}

		// Token: 0x06007D8C RID: 32140 RVA: 0x004AACD8 File Offset: 0x004A8ED8
		[DomainMethod]
		public void AcceptBuildingBlockCollectEarningQuick(DataContext context, BuildingBlockKey key, bool isPutInInventory)
		{
			BuildingEarningsData data;
			bool flag = !this.TryGetElement_CollectBuildingEarningsData(key, out data);
			if (!flag)
			{
				bool flag2 = data.CollectionItemList != null;
				if (flag2)
				{
					int count = data.CollectionItemList.Count;
					for (int i = 0; i < count; i++)
					{
						this.AcceptBuildingBlockCollectEarning(context, key, 0, isPutInInventory, false, false);
					}
				}
				bool flag3 = data.CollectionResourceList != null;
				if (flag3)
				{
					int tmpCount = data.CollectionResourceList.Count;
					for (int j = 0; j < tmpCount; j++)
					{
						this.AcceptBuildingBlockCollectEarning(context, key, 0, false, true, false);
					}
				}
				this.SetElement_CollectBuildingEarningsData(key, data, context);
			}
		}

		// Token: 0x06007D8D RID: 32141 RVA: 0x004AAD8C File Offset: 0x004A8F8C
		private void OfflineHandleRecruitPeopleLeave(DataContext context, BuildingBlockKey buildingBlockKey, int earningDataIndex)
		{
			BuildingEarningsData data;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(buildingBlockKey, out data) && data != null && data.RecruitLevelList.Count > 0 && earningDataIndex >= 0;
			if (flag)
			{
				DomainManager.Extra.RequestRecruitCharacterData(context, buildingBlockKey, earningDataIndex, true);
				data.RecruitLevelList.RemoveAt(earningDataIndex);
			}
		}

		// Token: 0x06007D8E RID: 32142 RVA: 0x004AADE4 File Offset: 0x004A8FE4
		[DomainMethod]
		public int AcceptBuildingBlockRecruitPeople(DataContext context, BuildingBlockKey key, int earningDataIndex, bool isSetData = true)
		{
			int charId = -1;
			BuildingEarningsData data;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(key, out data) && data != null && data.RecruitLevelList.Count > 0 && earningDataIndex >= 0;
			if (flag)
			{
				bool flag2 = earningDataIndex >= data.RecruitLevelList.Count;
				if (flag2)
				{
					earningDataIndex = 0;
				}
				RecruitCharacterData recruitCharacterData = DomainManager.Extra.RequestRecruitCharacterData(context, key, earningDataIndex, false);
				charId = this.CreateCharacterByRecruitCharacterData(context, recruitCharacterData);
				this.OfflineHandleRecruitPeopleLeave(context, key, earningDataIndex);
				InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotificationCollection();
				instantNotifications.AddJoinTaiwuVillage(charId);
				BuildingBlockData blockData = DomainManager.Building.GetBuildingBlockData(key);
				bool flag3 = blockData.TemplateId == 223;
				if (flag3)
				{
					this.ConsumeResource(context, 7, (int)GlobalConfig.Instance.RecruitPeopleCost);
				}
				DomainManager.Taiwu.AddLegacyPoint(context, 31, 100);
				if (isSetData)
				{
					this.SetElement_CollectBuildingEarningsData(key, data, context);
				}
			}
			return charId;
		}

		// Token: 0x06007D8F RID: 32143 RVA: 0x004AAED4 File Offset: 0x004A90D4
		[DomainMethod]
		public List<int> AcceptBuildingBlockRecruitPeopleQuick(DataContext context, BuildingBlockKey key)
		{
			List<int> charIdList = new List<int>();
			BuildingEarningsData data;
			bool flag = !this.TryGetElement_CollectBuildingEarningsData(key, out data) || data.RecruitLevelList == null;
			List<int> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int tmpCount = data.RecruitLevelList.Count;
				for (int i = 0; i < tmpCount; i++)
				{
					charIdList.Add(this.AcceptBuildingBlockRecruitPeople(context, key, 0, false));
				}
				this.SetElement_CollectBuildingEarningsData(key, data, context);
				result = charIdList;
			}
			return result;
		}

		// Token: 0x06007D90 RID: 32144 RVA: 0x004AAF50 File Offset: 0x004A9150
		[DomainMethod]
		public void RejectBuildingBlockRecruitPeople(DataContext context, BuildingBlockKey key, int earningDataIndex, bool isSetData = true)
		{
			BuildingEarningsData data;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(key, out data) && data != null && data.RecruitLevelList.Count > 0 && earningDataIndex >= 0;
			if (flag)
			{
				bool flag2 = earningDataIndex >= data.RecruitLevelList.Count;
				if (flag2)
				{
					earningDataIndex = 0;
				}
				this.OfflineHandleRecruitPeopleLeave(context, key, earningDataIndex);
				BuildingBlockData blockData;
				bool flag3 = this.TryGetElement_BuildingBlocks(key, out blockData);
				if (flag3)
				{
					InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotificationCollection();
					short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
					instantNotifications.AddCandidateLeaved(settlementId, blockData.TemplateId);
				}
				if (isSetData)
				{
					this.SetElement_CollectBuildingEarningsData(key, data, context);
				}
			}
		}

		// Token: 0x06007D91 RID: 32145 RVA: 0x004AAFF8 File Offset: 0x004A91F8
		[DomainMethod]
		public void RejectBuildingBlockRecruitPeopleQuick(DataContext context, BuildingBlockKey key)
		{
			BuildingEarningsData data;
			bool flag = !this.TryGetElement_CollectBuildingEarningsData(key, out data) || data.RecruitLevelList == null;
			if (!flag)
			{
				int tmpCount = data.RecruitLevelList.Count;
				for (int i = 0; i < tmpCount; i++)
				{
					this.RejectBuildingBlockRecruitPeople(context, key, 0, false);
				}
				this.SetElement_CollectBuildingEarningsData(key, data, context);
			}
		}

		// Token: 0x06007D92 RID: 32146 RVA: 0x004AB058 File Offset: 0x004A9258
		[Obsolete]
		[DomainMethod]
		public void ShopBuildingSoldItemAdd(DataContext context, BuildingBlockKey key, int earningDataIndex, ItemKey itemKey, bool isFromInventory)
		{
			BuildingBlockData blockData;
			bool flag = !this.TryGetElement_BuildingBlocks(key, out blockData) || earningDataIndex < 0;
			if (!flag)
			{
				sbyte slot = GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(blockData.TemplateId);
				BuildingEarningsData earningsData;
				bool flag2 = !this.TryGetElement_CollectBuildingEarningsData(key, out earningsData);
				if (flag2)
				{
					earningsData = new BuildingEarningsData();
					this.AddElement_CollectBuildingEarningsData(key, earningsData, context);
					for (int i = 0; i < (int)slot; i++)
					{
						earningsData.ShopSoldItemList.Add(ItemKey.Invalid);
						earningsData.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
					}
				}
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				bool flag3 = DomainManager.Taiwu.GetWarehouseAllItemKey().Contains(itemKey);
				if (flag3)
				{
					DomainManager.Taiwu.WarehouseRemove(context, itemKey, 1, false);
				}
				else
				{
					taiwu.RemoveInventoryItem(context, itemKey, 1, false, false);
				}
				bool flag4 = earningDataIndex >= earningsData.ShopSoldItemList.Count;
				if (flag4)
				{
					for (int j = 0; j < earningDataIndex - earningsData.ShopSoldItemList.Count + 1; j++)
					{
						earningsData.ShopSoldItemList.Add(ItemKey.Invalid);
						earningsData.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
					}
				}
				earningsData.ShopSoldItemList[earningDataIndex] = itemKey;
				earningsData.ShopSoldItemEarnList[earningDataIndex] = new IntPair(-1, -1);
				this.SetElement_CollectBuildingEarningsData(key, earningsData, context);
			}
		}

		// Token: 0x06007D93 RID: 32147 RVA: 0x004AB1C4 File Offset: 0x004A93C4
		[Obsolete]
		[DomainMethod]
		public void ShopBuildingSoldItemChange(DataContext context, BuildingBlockKey key, int earningDataIndex, ItemKey itemKey, bool isFromInventory)
		{
			BuildingBlockData blockData;
			bool flag = !this.TryGetElement_BuildingBlocks(key, out blockData) || earningDataIndex < 0;
			if (!flag)
			{
				sbyte slot = GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(blockData.TemplateId);
				BuildingEarningsData earningsData;
				bool flag2 = !this.TryGetElement_CollectBuildingEarningsData(key, out earningsData);
				if (flag2)
				{
					earningsData = new BuildingEarningsData();
					this.AddElement_CollectBuildingEarningsData(key, earningsData, context);
					for (int i = 0; i < (int)slot; i++)
					{
						earningsData.ShopSoldItemList.Add(ItemKey.Invalid);
						earningsData.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
					}
				}
				bool flag3 = earningsData.ShopSoldItemList[earningDataIndex].Id == itemKey.Id;
				if (!flag3)
				{
					if (isFromInventory)
					{
						GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
						bool flag4 = itemKey.IsValid();
						if (flag4)
						{
							taiwu.RemoveInventoryItem(context, itemKey, 1, false, false);
						}
						taiwu.AddInventoryItem(context, earningsData.ShopSoldItemList[earningDataIndex], 1, false);
					}
					else
					{
						bool flag5 = itemKey.IsValid();
						if (flag5)
						{
							DomainManager.Taiwu.WarehouseRemove(context, itemKey, 1, false);
						}
						DomainManager.Taiwu.WarehouseAdd(context, earningsData.ShopSoldItemList[earningDataIndex], 1);
					}
					earningsData.ShopSoldItemList[earningDataIndex] = itemKey;
					earningsData.ShopSoldItemEarnList[earningDataIndex] = new IntPair(-1, -1);
					this.SetElement_CollectBuildingEarningsData(key, earningsData, context);
				}
			}
		}

		// Token: 0x06007D94 RID: 32148 RVA: 0x004AB330 File Offset: 0x004A9530
		[DomainMethod]
		public void ShopBuildingMultiChangeSoldItem(DataContext context, BuildingBlockKey key, List<ItemKey> itemList, List<int> operateTypeList)
		{
			BuildingBlockData blockData;
			bool flag = !this.TryGetElement_BuildingBlocks(key, out blockData) || itemList.Count != operateTypeList.Count;
			if (!flag)
			{
				BuildingBlockItem config = BuildingBlock.Instance[blockData.TemplateId];
				sbyte slot = GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(config.TemplateId);
				bool flag2 = config.TemplateId == 105;
				if (flag2)
				{
					for (int i = 0; i < itemList.Count; i++)
					{
						ItemKey itemKey = itemList[i];
						bool flag3 = !itemKey.IsValid();
						if (!flag3)
						{
							switch (operateTypeList[i])
							{
							case 1:
								this.AddFixBook(context, key, itemKey, ItemSourceType.Inventory);
								break;
							case 2:
								this.ChangeFixBook(context, key, ItemKey.Invalid, ItemSourceType.Inventory);
								break;
							case 3:
								this.AddFixBook(context, key, itemKey, ItemSourceType.Warehouse);
								break;
							case 4:
								this.ChangeFixBook(context, key, ItemKey.Invalid, ItemSourceType.Warehouse);
								break;
							case 5:
								this.AddFixBook(context, key, itemKey, ItemSourceType.Treasury);
								break;
							case 6:
								this.ChangeFixBook(context, key, ItemKey.Invalid, ItemSourceType.Treasury);
								break;
							}
						}
					}
				}
				else
				{
					BuildingEarningsData earningsData;
					bool flag4 = !this.TryGetElement_CollectBuildingEarningsData(key, out earningsData);
					if (flag4)
					{
						earningsData = new BuildingEarningsData();
						for (int j = 0; j < (int)slot; j++)
						{
							earningsData.ShopSoldItemList.Add(ItemKey.Invalid);
							earningsData.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
						}
						this.AddElement_CollectBuildingEarningsData(key, earningsData, context);
					}
					bool flag5 = earningsData.ShopSoldItemList.Count < (int)slot;
					if (flag5)
					{
						int count = (int)slot - earningsData.ShopSoldItemList.Count;
						for (int k = 0; k < count; k++)
						{
							earningsData.ShopSoldItemList.Add(ItemKey.Invalid);
							earningsData.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
						}
					}
					GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
					for (int l = 0; l < itemList.Count; l++)
					{
						ItemKey itemKey2 = itemList[l];
						bool flag6 = !itemKey2.IsValid();
						if (!flag6)
						{
							switch (operateTypeList[l])
							{
							case 1:
							{
								taiwu.RemoveInventoryItem(context, itemKey2, 1, false, false);
								sbyte index = this.FindFirstCanUseIndex(earningsData);
								bool flag7 = index >= 0 && (int)index < earningsData.ShopSoldItemList.Count;
								if (flag7)
								{
									this.AddBuildingEarningsDataShopSoldItem((int)index, itemKey2, earningsData, blockData.TemplateId);
								}
								break;
							}
							case 2:
							{
								sbyte index = this.FindItemKeyIndex(earningsData, itemKey2);
								this.RemoveBuildingEarningsDataShopSoldItem((int)index, itemKey2, earningsData, blockData.TemplateId);
								taiwu.AddInventoryItem(context, itemKey2, 1, false);
								break;
							}
							case 3:
							{
								DomainManager.Taiwu.WarehouseRemove(context, itemKey2, 1, false);
								sbyte index = this.FindFirstCanUseIndex(earningsData);
								bool flag8 = index >= 0 && (int)index < earningsData.ShopSoldItemList.Count;
								if (flag8)
								{
									this.AddBuildingEarningsDataShopSoldItem((int)index, itemKey2, earningsData, blockData.TemplateId);
								}
								break;
							}
							case 4:
							{
								sbyte index = this.FindItemKeyIndex(earningsData, itemKey2);
								this.RemoveBuildingEarningsDataShopSoldItem((int)index, itemKey2, earningsData, blockData.TemplateId);
								DomainManager.Taiwu.WarehouseAdd(context, itemKey2, 1);
								break;
							}
							case 5:
							{
								DomainManager.Taiwu.RemoveItem(context, itemKey2, 1, ItemSourceType.Treasury, false, false);
								sbyte index = this.FindFirstCanUseIndex(earningsData);
								bool flag9 = index >= 0 && (int)index < earningsData.ShopSoldItemList.Count;
								if (flag9)
								{
									this.AddBuildingEarningsDataShopSoldItem((int)index, itemKey2, earningsData, blockData.TemplateId);
								}
								break;
							}
							case 6:
							{
								sbyte index = this.FindItemKeyIndex(earningsData, itemKey2);
								this.RemoveBuildingEarningsDataShopSoldItem((int)index, itemKey2, earningsData, blockData.TemplateId);
								DomainManager.Taiwu.AddItem(context, itemKey2, 1, ItemSourceType.Treasury, false);
								break;
							}
							case 7:
							{
								DomainManager.Extra.RemoveStockStorageItem(context, 1, itemKey2, 1, false);
								sbyte index = this.FindFirstCanUseIndex(earningsData);
								bool flag10 = index >= 0 && (int)index < earningsData.ShopSoldItemList.Count;
								if (flag10)
								{
									this.AddBuildingEarningsDataShopSoldItem((int)index, itemKey2, earningsData, blockData.TemplateId);
								}
								break;
							}
							case 8:
							{
								sbyte index = this.FindItemKeyIndex(earningsData, itemKey2);
								this.RemoveBuildingEarningsDataShopSoldItem((int)index, itemKey2, earningsData, blockData.TemplateId);
								DomainManager.Extra.AddStockStorageItem(context, 1, itemKey2, 1);
								break;
							}
							}
						}
					}
					this.SetElement_CollectBuildingEarningsData(key, earningsData, context);
				}
			}
		}

		// Token: 0x06007D95 RID: 32149 RVA: 0x004AB7C0 File Offset: 0x004A99C0
		private void AddBuildingEarningsDataShopSoldItem(int index, ItemKey itemKey, BuildingEarningsData earningsData, short templateId)
		{
			earningsData.ShopSoldItemList[index] = itemKey;
			DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, (int)templateId);
		}

		// Token: 0x06007D96 RID: 32150 RVA: 0x004AB7E1 File Offset: 0x004A99E1
		private void RemoveBuildingEarningsDataShopSoldItem(int index, ItemKey itemKey, BuildingEarningsData earningsData, short templateId)
		{
			earningsData.ShopSoldItemList[index] = ItemKey.Invalid;
			DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, (int)templateId);
		}

		// Token: 0x06007D97 RID: 32151 RVA: 0x004AB808 File Offset: 0x004A9A08
		private BuildingOptionAutoAddSoldItemPreset GetAutoAddSoldItemSetting(BuildingBlockKey blockKey)
		{
			BuildingBlockDataEx extraData;
			DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)blockKey, out extraData);
			return ((extraData != null) ? extraData.SoldItemSetting : null) ?? new BuildingOptionAutoAddSoldItemPreset();
		}

		// Token: 0x06007D98 RID: 32152 RVA: 0x004AB844 File Offset: 0x004A9A44
		private void AutoAddShopSoldItem(DataContext context, BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData = this.GetBuildingBlockData(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			IReadOnlyDictionary<ItemKey, int> goodsShelf = DomainManager.Taiwu.GetItems(ItemSourceType.StockStorageGoodsShelf);
			bool flag = goodsShelf.Count == 0;
			if (!flag)
			{
				BuildingBlockKey betterBlockKey = this.GetMoreBetterSoldBuilding(blockKey, blockData);
				BuildingBlockData betterBlockData = null;
				BuildingEarningsData betterEarningsData = null;
				bool isHaveBetter = false;
				bool flag2 = !betterBlockKey.Equals(BuildingBlockKey.Invalid);
				if (flag2)
				{
					this.TryGetElement_BuildingBlocks(betterBlockKey, out betterBlockData);
					isHaveBetter = this.TryGetElement_CollectBuildingEarningsData(betterBlockKey, out betterEarningsData);
					bool flag3 = betterEarningsData == null;
					if (flag3)
					{
						BuildingDomain.<AutoAddShopSoldItem>g__InitBuildingEarningsData|492_1(GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(betterBlockData.TemplateId), ref betterEarningsData);
					}
				}
				BuildingEarningsData earningsData;
				bool isHave = this.TryGetElement_CollectBuildingEarningsData(blockKey, out earningsData);
				bool flag4 = earningsData == null;
				if (flag4)
				{
					BuildingDomain.<AutoAddShopSoldItem>g__InitBuildingEarningsData|492_1(GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(blockData.TemplateId), ref earningsData);
				}
				BuildingOptionAutoAddSoldItemPreset settings = this.GetAutoAddSoldItemSetting(blockKey);
				List<ItemKeyAndCount> goodsList = new List<ItemKeyAndCount>();
				List<ItemKey> canSoldList = new List<ItemKey>();
				foreach (KeyValuePair<ItemKey, int> keyValuePair in goodsShelf)
				{
					ItemKey itemKey3;
					int num;
					keyValuePair.Deconstruct(out itemKey3, out num);
					ItemKey itemKey = itemKey3;
					int count = num;
					bool result = GameData.Domains.Building.SharedMethods.IsBuildingCanSoldItem(configData, itemKey);
					bool flag5 = !result;
					if (!flag5)
					{
						List<sbyte> itemTypeList = settings.ItemTypeList;
						bool flag6 = itemTypeList != null && itemTypeList.Count > 0 && !settings.ItemTypeList.Contains(itemKey.ItemType);
						if (!flag6)
						{
							sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
							bool flag7 = grade < settings.MinGrade || grade > settings.MaxGrade;
							if (!flag7)
							{
								goodsList.Add(new ItemKeyAndCount(itemKey, count));
							}
						}
					}
				}
				bool isGradeOrderHigh = settings.GradeOrder == 1;
				bool isPropertyOrderMaxValue = settings.PropertyOrderEnum.HasFlag(BuildingOptionAutoAddSoldItemPreset.EPropertyOrder.MaxValue);
				bool isPropertyOrderMaxAmount = settings.PropertyOrderEnum.HasFlag(BuildingOptionAutoAddSoldItemPreset.EPropertyOrder.MaxAmount);
				goodsList.Sort(delegate(ItemKeyAndCount a, ItemKeyAndCount b)
				{
					sbyte aGrade = ItemTemplateHelper.GetGrade(a.ItemKey.ItemType, a.ItemKey.TemplateId);
					sbyte bGrade = ItemTemplateHelper.GetGrade(b.ItemKey.ItemType, b.ItemKey.TemplateId);
					bool flag16 = aGrade != bGrade;
					int result2;
					if (flag16)
					{
						result2 = (isGradeOrderHigh ? bGrade.CompareTo(aGrade) : aGrade.CompareTo(bGrade));
					}
					else
					{
						int aValue = DomainManager.Item.GetValue(a.ItemKey);
						int bValue = DomainManager.Item.GetValue(b.ItemKey);
						bool flag17 = aValue != bValue;
						if (flag17)
						{
							result2 = (isPropertyOrderMaxValue ? bValue.CompareTo(aValue) : aValue.CompareTo(bValue));
						}
						else
						{
							bool flag18 = a.Count != b.Count;
							if (flag18)
							{
								result2 = (isPropertyOrderMaxAmount ? b.Count.CompareTo(a.Count) : a.Count.CompareTo(b.Count));
							}
							else
							{
								result2 = 0;
							}
						}
					}
					return result2;
				});
				foreach (ItemKeyAndCount itemKeyAndCount in goodsList)
				{
					ItemKey itemKey3;
					int num;
					itemKeyAndCount.Deconstruct(out itemKey3, out num);
					ItemKey itemKey2 = itemKey3;
					int count2 = num;
					for (int i = 0; i < count2; i++)
					{
						canSoldList.Add(itemKey2);
					}
				}
				bool flag8 = canSoldList.Count <= 0;
				if (!flag8)
				{
					bool flag9 = !betterBlockKey.Equals(BuildingBlockKey.Invalid) && betterBlockData != null;
					if (flag9)
					{
						int betterLeftCapacity = (int)GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(betterBlockData.TemplateId) - this.GetShopSoldItemCount(betterEarningsData) - this.GetShopSoldEarnCount(betterEarningsData);
						for (int j = 0; j < Math.Min(canSoldList.Count, betterLeftCapacity); j++)
						{
							int index = (int)this.FindFirstCanUseIndex(betterEarningsData);
							bool flag10 = index < 0 && betterEarningsData.ShopSoldItemList.Count < (int)GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(betterBlockData.TemplateId);
							if (flag10)
							{
								betterEarningsData.ShopSoldItemList.Add(ItemKey.Invalid);
								betterEarningsData.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
								index = betterEarningsData.ShopSoldItemList.Count - 1;
							}
							bool flag11 = index >= 0 && index < betterEarningsData.ShopSoldItemList.Count;
							if (flag11)
							{
								DomainManager.Extra.RemoveStockStorageItem(context, 1, canSoldList[j], 1, false);
								this.AddBuildingEarningsDataShopSoldItem(index, canSoldList[j], betterEarningsData, blockData.TemplateId);
								canSoldList.RemoveAt(j);
							}
						}
						bool flag12 = isHaveBetter;
						if (flag12)
						{
							this.SetElement_CollectBuildingEarningsData(betterBlockKey, betterEarningsData, context);
						}
						else
						{
							this.AddElement_CollectBuildingEarningsData(betterBlockKey, betterEarningsData, context);
						}
					}
					int leftCapacity = (int)GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(blockData.TemplateId) - this.GetShopSoldItemCount(earningsData) - this.GetShopSoldEarnCount(earningsData);
					for (int k = 0; k < Math.Min(canSoldList.Count, leftCapacity); k++)
					{
						int index2 = (int)this.FindFirstCanUseIndex(earningsData);
						bool flag13 = index2 < 0 && earningsData.ShopSoldItemList.Count < (int)GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(blockData.TemplateId);
						if (flag13)
						{
							earningsData.ShopSoldItemList.Add(ItemKey.Invalid);
							earningsData.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
							index2 = earningsData.ShopSoldItemList.Count - 1;
						}
						bool flag14 = index2 >= 0 && index2 < earningsData.ShopSoldItemList.Count;
						if (flag14)
						{
							DomainManager.Extra.RemoveStockStorageItem(context, 1, canSoldList[k], 1, false);
							this.AddBuildingEarningsDataShopSoldItem(index2, canSoldList[k], earningsData, blockData.TemplateId);
						}
					}
					bool flag15 = isHave;
					if (flag15)
					{
						this.SetElement_CollectBuildingEarningsData(blockKey, earningsData, context);
					}
					else
					{
						this.AddElement_CollectBuildingEarningsData(blockKey, earningsData, context);
					}
				}
			}
		}

		// Token: 0x06007D99 RID: 32153 RVA: 0x004ABD90 File Offset: 0x004A9F90
		[DomainMethod]
		public void QuickAddShopSoldItem(DataContext context, BuildingBlockKey blockKey)
		{
			this.QuickRemoveShopSoldItem(context, blockKey);
			this.AutoAddShopSoldItem(context, blockKey);
			BuildingEarningsData earningsData;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(blockKey, out earningsData);
			if (flag)
			{
				this.SetElement_CollectBuildingEarningsData(blockKey, earningsData, context);
			}
		}

		// Token: 0x06007D9A RID: 32154 RVA: 0x004ABDC8 File Offset: 0x004A9FC8
		[DomainMethod]
		public void QuickRemoveShopSoldItem(DataContext context, BuildingBlockKey blockKey)
		{
			BuildingEarningsData earningsData;
			bool flag = !this.TryGetElement_CollectBuildingEarningsData(blockKey, out earningsData);
			if (!flag)
			{
				sbyte i = 0;
				while ((int)i < earningsData.ShopSoldItemList.Count)
				{
					ItemKey shopSoldItem = earningsData.ShopSoldItemList[(int)i];
					IntPair shopSoldItemEarn = earningsData.ShopSoldItemEarnList[(int)i];
					bool flag2 = !shopSoldItem.Equals(ItemKey.Invalid) && shopSoldItemEarn.First == -1;
					if (flag2)
					{
						BuildingBlockData blockData = this.GetBuildingBlockData(blockKey);
						this.RemoveBuildingEarningsDataShopSoldItem((int)i, shopSoldItem, earningsData, blockData.TemplateId);
						DomainManager.Extra.AddStockStorageItem(context, 1, shopSoldItem, 1);
					}
					i += 1;
				}
				this.SetElement_CollectBuildingEarningsData(blockKey, earningsData, context);
			}
		}

		// Token: 0x06007D9B RID: 32155 RVA: 0x004ABE80 File Offset: 0x004AA080
		public sbyte FindFirstCanUseIndex(BuildingEarningsData earningsData)
		{
			sbyte index = -1;
			sbyte i = 0;
			while ((int)i < earningsData.ShopSoldItemList.Count)
			{
				bool flag = earningsData.ShopSoldItemList[(int)i].Equals(ItemKey.Invalid) && earningsData.ShopSoldItemEarnList[(int)i].First == -1;
				if (flag)
				{
					index = i;
					break;
				}
				i += 1;
			}
			return index;
		}

		// Token: 0x06007D9C RID: 32156 RVA: 0x004ABEF4 File Offset: 0x004AA0F4
		public sbyte FindItemKeyIndex(BuildingEarningsData earningsData, ItemKey itemKey)
		{
			sbyte index = 0;
			sbyte i = 0;
			while ((int)i < earningsData.ShopSoldItemList.Count)
			{
				bool flag = earningsData.ShopSoldItemList[(int)i].Equals(itemKey);
				if (flag)
				{
					index = i;
					break;
				}
				i += 1;
			}
			return index;
		}

		// Token: 0x06007D9D RID: 32157 RVA: 0x004ABF48 File Offset: 0x004AA148
		public int GetShopSoldItemCount(BuildingEarningsData earningsData)
		{
			bool flag = earningsData == null || earningsData.ShopSoldItemList == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int count = 0;
				for (int i = 0; i < earningsData.ShopSoldItemList.Count; i++)
				{
					bool flag2 = !earningsData.ShopSoldItemList[i].Equals(ItemKey.Invalid);
					if (flag2)
					{
						count++;
					}
				}
				result = count;
			}
			return result;
		}

		// Token: 0x06007D9E RID: 32158 RVA: 0x004ABFBC File Offset: 0x004AA1BC
		public int GetShopSoldEarnCount(BuildingEarningsData earningsData)
		{
			bool flag = earningsData == null || earningsData.ShopSoldItemEarnList == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int count = 0;
				for (int i = 0; i < earningsData.ShopSoldItemEarnList.Count; i++)
				{
					bool flag2 = earningsData.ShopSoldItemEarnList[i].First != -1;
					if (flag2)
					{
						count++;
					}
				}
				result = count;
			}
			return result;
		}

		// Token: 0x06007D9F RID: 32159 RVA: 0x004AC02C File Offset: 0x004AA22C
		[DomainMethod]
		public void ShopBuildingSoldItemReceive(DataContext context, BuildingBlockKey key, int earningDataIndex, bool isSetData = true)
		{
			BuildingEarningsData data;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(key, out data) && earningDataIndex >= 0;
			if (flag)
			{
				IntPair resourceIntPair = data.ShopSoldItemEarnList[earningDataIndex];
				DomainManager.Character.GetElement_Objects(DomainManager.Taiwu.GetTaiwuCharId()).ChangeResource(context, (sbyte)resourceIntPair.First, resourceIntPair.Second);
				this.ApplyBuildingResourceOutputSetting(context, key, (sbyte)resourceIntPair.First, resourceIntPair.Second);
				data.ShopSoldItemList[earningDataIndex] = ItemKey.Invalid;
				data.ShopSoldItemEarnList[earningDataIndex] = new IntPair(-1, -1);
				DomainManager.Building.AddBuildingGainLegacy(context, key);
			}
			if (isSetData)
			{
				this.SetElement_CollectBuildingEarningsData(key, data, context);
			}
		}

		// Token: 0x06007DA0 RID: 32160 RVA: 0x004AC0E8 File Offset: 0x004AA2E8
		[DomainMethod]
		public void ShopBuildingSoldItemReceiveQuick(DataContext context, BuildingBlockKey key)
		{
			BuildingEarningsData data;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(key, out data);
			if (flag)
			{
				bool flag2 = data == null;
				if (!flag2)
				{
					for (int i = 0; i < data.ShopSoldItemEarnList.Count; i++)
					{
						bool flag3 = data.ShopSoldItemEarnList[i].First != -1;
						if (flag3)
						{
							this.ShopBuildingSoldItemReceive(context, key, i, false);
						}
					}
					this.SetElement_CollectBuildingEarningsData(key, data, context);
				}
			}
		}

		// Token: 0x06007DA1 RID: 32161 RVA: 0x004AC160 File Offset: 0x004AA360
		[DomainMethod]
		public List<ItemDisplayData> QuickCollectShopItem(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			List<ItemKey> itemKeyList = new List<ItemKey>();
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = blockData.TemplateId <= 0;
				if (!flag)
				{
					BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
					bool flag2 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ItemList.Count > 0;
					if (flag2)
					{
						bool flag3 = blockData.TemplateId == 222;
						if (!flag3)
						{
							this.CollectItemQuick(context, blockKey, itemKeyList);
						}
					}
				}
			}
			foreach (Feast feast in DomainManager.Extra.GetAllFeasts().Values)
			{
				for (int i = 0; i < GlobalConfig.Instance.FeastGiftCount; i++)
				{
					ItemKey gift = feast.GetGift(i);
					bool flag4 = gift != ItemKey.Invalid && !ItemTemplateHelper.IsMiscResource(gift.ItemType, gift.TemplateId);
					if (flag4)
					{
						itemKeyList.Add(gift);
					}
				}
				DomainManager.Extra.FeastReceiveGift(context, feast.BuildingBlockKey, -1);
			}
			return DomainManager.Item.GetItemDisplayDataListOptional(itemKeyList, DomainManager.Taiwu.GetTaiwuCharId(), -1, false);
		}

		// Token: 0x06007DA2 RID: 32162 RVA: 0x004AC340 File Offset: 0x004AA540
		[DomainMethod]
		public List<ItemDisplayData> QuickCollectSingleShopItem(DataContext context, BuildingBlockKey blockKey)
		{
			List<ItemKey> itemKeyList = new List<ItemKey>();
			BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
			bool flag = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ItemList.Count > 0;
			if (flag)
			{
				this.CollectItemQuick(context, blockKey, itemKeyList);
			}
			return DomainManager.Item.GetItemDisplayDataList(itemKeyList, DomainManager.Taiwu.GetTaiwuCharId());
		}

		// Token: 0x06007DA3 RID: 32163 RVA: 0x004AC3D0 File Offset: 0x004AA5D0
		public void CollectItemQuick(DataContext context, BuildingBlockKey key, List<ItemKey> itemKeyList)
		{
			BuildingEarningsData data;
			bool flag = !this.TryGetElement_CollectBuildingEarningsData(key, out data);
			if (!flag)
			{
				bool flag2 = data.CollectionItemList != null;
				if (flag2)
				{
					int count = data.CollectionItemList.Count;
					for (int i = 0; i < count; i++)
					{
						this.CollectShopItem(context, key, 0, itemKeyList);
					}
				}
				this.SetElement_CollectBuildingEarningsData(key, data, context);
			}
		}

		// Token: 0x06007DA4 RID: 32164 RVA: 0x004AC438 File Offset: 0x004AA638
		public BuildingBlockKey CollectShopItem(DataContext context, BuildingBlockKey key, int earningDataIndex, List<ItemKey> itemKeyList)
		{
			BuildingEarningsData data;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(key, out data) && earningDataIndex >= 0;
			if (flag)
			{
				bool flag2 = earningDataIndex < data.CollectionItemList.Count;
				if (flag2)
				{
					ItemKey itemKey = data.CollectionItemList[earningDataIndex];
					itemKey.ModificationState = 0;
					data.CollectionItemList.RemoveAt(earningDataIndex);
					ItemSourceType sourceType = this.ApplyBuildingItemOutputSetting(key, itemKey);
					DomainManager.Taiwu.AddItem(context, itemKey, 1, sourceType, false);
					itemKeyList.Add(itemKey);
					this.AddBuildingGainLegacy(context, key);
					this.BuildingChangeProfessionSeniority(context, itemKey);
				}
			}
			return key;
		}

		// Token: 0x06007DA5 RID: 32165 RVA: 0x004AC4D4 File Offset: 0x004AA6D4
		public void AddBuildingGainLegacy(DataContext context, BuildingBlockKey key)
		{
			BuildingBlockData blockData;
			bool flag = DomainManager.Building.TryGetElement_BuildingBlocks(key, out blockData);
			if (flag)
			{
				BuildingBlockItem config = BuildingBlock.Instance.GetItem(blockData.TemplateId);
				DomainManager.Taiwu.AddLegacyPoint(context, config.IsCollectResourceBuilding ? 28 : 29, 100);
			}
		}

		// Token: 0x06007DA6 RID: 32166 RVA: 0x004AC524 File Offset: 0x004AA724
		private void BuildingChangeProfessionSeniority(DataContext context, ItemKey itemKey)
		{
			short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			int price = DomainManager.Item.GetBaseItem(itemKey).GetValue();
			bool flag = itemSubType == 901;
			if (flag)
			{
				ProfessionFormulaItem formulaItem = ProfessionFormula.Instance[54];
				DomainManager.Extra.ChangeProfessionSeniority(context, 7, formulaItem.Calculate(price), true, false);
			}
			else
			{
				bool flag2 = itemSubType == 900;
				if (flag2)
				{
					ProfessionFormulaItem formulaItem2 = ProfessionFormula.Instance[105];
					DomainManager.Extra.ChangeProfessionSeniority(context, 16, formulaItem2.Calculate(price), true, false);
				}
			}
		}

		// Token: 0x06007DA7 RID: 32167 RVA: 0x004AC5C0 File Offset: 0x004AA7C0
		[DomainMethod]
		public int QuickCollectShopItemCount(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			int count = 0;
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = blockData.TemplateId <= 0;
				if (!flag)
				{
					BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
					bool flag2 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ItemList.Count > 0;
					if (flag2)
					{
						bool flag3 = blockData.TemplateId == 222;
						if (!flag3)
						{
							BuildingEarningsData data;
							bool flag4 = !this.TryGetElement_CollectBuildingEarningsData(blockKey, out data);
							if (!flag4)
							{
								bool flag5 = data.CollectionItemList != null;
								if (flag5)
								{
									count += data.CollectionItemList.Count;
								}
							}
						}
					}
				}
			}
			return count;
		}

		// Token: 0x06007DA8 RID: 32168 RVA: 0x004AC6EC File Offset: 0x004AA8EC
		[DomainMethod]
		public void QuickCollectShopSoldItem(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = blockData.TemplateId <= 0;
				if (!flag)
				{
					BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
					bool flag2 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ExchangeResourceGoods != -1;
					if (flag2)
					{
						this.ShopBuildingSoldItemReceiveQuick(context, blockKey);
					}
					bool flag3 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ResourceGoods != -1;
					if (flag3)
					{
						this.AcceptBuildingBlockCollectEarningQuick(context, blockKey, false);
					}
				}
			}
		}

		// Token: 0x06007DA9 RID: 32169 RVA: 0x004AC810 File Offset: 0x004AAA10
		[DomainMethod]
		public void QuickCollectSingleShopSoldItem(DataContext context, BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
			bool flag = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ExchangeResourceGoods != -1;
			if (flag)
			{
				this.ShopBuildingSoldItemReceiveQuick(context, blockKey);
			}
			bool flag2 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ResourceGoods != -1;
			if (flag2)
			{
				this.AcceptBuildingBlockCollectEarningQuick(context, blockKey, false);
			}
		}

		// Token: 0x06007DAA RID: 32170 RVA: 0x004AC8BC File Offset: 0x004AAABC
		[DomainMethod]
		public int QuickCollectShopSoldItemCount(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			int count = 0;
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = blockData.TemplateId <= 0;
				if (!flag)
				{
					BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
					bool flag2 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ExchangeResourceGoods != -1;
					if (flag2)
					{
						BuildingEarningsData data;
						bool flag3 = this.TryGetElement_CollectBuildingEarningsData(blockKey, out data);
						if (flag3)
						{
							bool flag4 = data == null;
							if (flag4)
							{
								goto IL_184;
							}
							for (int i = 0; i < data.ShopSoldItemEarnList.Count; i++)
							{
								bool flag5 = data.ShopSoldItemEarnList[i].First != -1;
								if (flag5)
								{
									count++;
								}
							}
						}
					}
					bool flag6 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ResourceGoods != -1;
					if (flag6)
					{
						BuildingEarningsData data2;
						bool flag7 = !this.TryGetElement_CollectBuildingEarningsData(blockKey, out data2);
						if (!flag7)
						{
							bool flag8 = data2.CollectionResourceList != null;
							if (flag8)
							{
								count += data2.CollectionResourceList.Count;
							}
						}
					}
				}
				IL_184:;
			}
			return count;
		}

		// Token: 0x06007DAB RID: 32171 RVA: 0x004ACA74 File Offset: 0x004AAC74
		[DomainMethod]
		public List<int> QuickRecruitPeople(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			List<int> charIdList = new List<int>();
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = blockData.TemplateId <= 0;
				if (!flag)
				{
					BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
					bool flag2 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).RecruitPeopleProb.Count > 0;
					if (flag2)
					{
						bool flag3 = blockData.TemplateId == 223;
						if (!flag3)
						{
							this.RecruitPeopleQuick(context, blockKey, charIdList);
						}
					}
				}
			}
			return charIdList;
		}

		// Token: 0x06007DAC RID: 32172 RVA: 0x004ACB78 File Offset: 0x004AAD78
		[DomainMethod]
		public List<int> QuickRecruitSingleBuildingPeople(DataContext context, BuildingBlockKey blockKey)
		{
			List<int> charIdList = new List<int>();
			BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
			BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
			bool flag = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).RecruitPeopleProb.Count > 0;
			if (flag)
			{
				this.RecruitPeopleQuick(context, blockKey, charIdList);
			}
			return charIdList;
		}

		// Token: 0x06007DAD RID: 32173 RVA: 0x004ACBF4 File Offset: 0x004AADF4
		public void RecruitPeopleQuick(DataContext context, BuildingBlockKey key, List<int> charIdList)
		{
			BuildingEarningsData data;
			bool flag = !this.TryGetElement_CollectBuildingEarningsData(key, out data) || data.RecruitLevelList == null;
			if (!flag)
			{
				int tmpCount = data.RecruitLevelList.Count;
				for (int i = 0; i < tmpCount; i++)
				{
					this.RecruitPeople(context, key, 0, charIdList);
				}
				this.SetElement_CollectBuildingEarningsData(key, data, context);
			}
		}

		// Token: 0x06007DAE RID: 32174 RVA: 0x004ACC54 File Offset: 0x004AAE54
		public void RecruitPeople(DataContext context, BuildingBlockKey key, int earningDataIndex, List<int> charIdList)
		{
			charIdList.Add(this.AcceptBuildingBlockRecruitPeople(context, key, earningDataIndex, false));
		}

		// Token: 0x06007DAF RID: 32175 RVA: 0x004ACC6C File Offset: 0x004AAE6C
		[DomainMethod]
		public int QuickRecruitPeopleCount(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			int count = 0;
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = blockData.TemplateId <= 0;
				if (!flag)
				{
					BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
					bool flag2 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).RecruitPeopleProb.Count > 0;
					if (flag2)
					{
						bool flag3 = blockData.TemplateId == 223;
						if (!flag3)
						{
							BuildingEarningsData data;
							bool flag4 = !this.TryGetElement_CollectBuildingEarningsData(blockKey, out data) || data.RecruitLevelList == null;
							if (!flag4)
							{
								count += data.RecruitLevelList.Count;
							}
						}
					}
				}
			}
			return count;
		}

		// Token: 0x06007DB0 RID: 32176 RVA: 0x004ACD94 File Offset: 0x004AAF94
		[DomainMethod]
		public void QuickCollectBuildingEarn(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = blockData.TemplateId <= 0;
				if (!flag)
				{
					BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
					bool flag2 = configData.SuccesEvent.Count != 0 && (ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ItemList.Count > 0 || ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ResourceGoods != -1);
					if (flag2)
					{
						this.AcceptBuildingBlockCollectEarningQuick(context, blockKey, false);
					}
					bool flag3 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ExchangeResourceGoods != -1;
					if (flag3)
					{
						bool flag4 = blockData.TemplateId == 222;
						if (flag4)
						{
							goto IL_18F;
						}
						this.ShopBuildingSoldItemReceiveQuick(context, blockKey);
					}
					bool flag5 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).RecruitPeopleProb.Count > 0;
					if (flag5)
					{
						bool flag6 = blockData.TemplateId == 223;
						if (!flag6)
						{
							this.AcceptBuildingBlockRecruitPeopleQuick(context, blockKey);
						}
					}
				}
				IL_18F:;
			}
		}

		// Token: 0x06007DB1 RID: 32177 RVA: 0x004ACF50 File Offset: 0x004AB150
		[DomainMethod]
		public int QuickCollectBuildingEarnCount(DataContext context)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			int count = 0;
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = blockData.TemplateId <= 0;
				if (!flag)
				{
					BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
					bool flag2 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).RecruitPeopleProb.Count > 0;
					if (flag2)
					{
						bool flag3 = blockData.TemplateId == 223;
						if (flag3)
						{
							goto IL_299;
						}
						BuildingEarningsData data;
						bool flag4 = !this.TryGetElement_CollectBuildingEarningsData(blockKey, out data) || data.RecruitLevelList == null;
						if (flag4)
						{
							goto IL_299;
						}
						count += data.RecruitLevelList.Count;
					}
					bool flag5 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ExchangeResourceGoods != -1;
					if (flag5)
					{
						BuildingEarningsData data2;
						bool flag6 = this.TryGetElement_CollectBuildingEarningsData(blockKey, out data2);
						if (flag6)
						{
							bool flag7 = data2 == null;
							if (flag7)
							{
								goto IL_299;
							}
							for (int i = 0; i < data2.ShopSoldItemEarnList.Count; i++)
							{
								bool flag8 = data2.ShopSoldItemEarnList[i].First != -1;
								if (flag8)
								{
									count++;
								}
							}
						}
					}
					bool flag9 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ResourceGoods != -1;
					if (flag9)
					{
						BuildingEarningsData data3;
						bool flag10 = !this.TryGetElement_CollectBuildingEarningsData(blockKey, out data3);
						if (flag10)
						{
							goto IL_299;
						}
						bool flag11 = data3.CollectionResourceList != null;
						if (flag11)
						{
							count += data3.CollectionResourceList.Count;
						}
					}
					bool flag12 = configData.SuccesEvent.Count != 0 && ShopEvent.Instance.GetItem(configData.SuccesEvent[0]).ItemList.Count > 0;
					if (flag12)
					{
						bool flag13 = blockData.TemplateId == 222;
						if (!flag13)
						{
							BuildingEarningsData data4;
							bool flag14 = !this.TryGetElement_CollectBuildingEarningsData(blockKey, out data4);
							if (!flag14)
							{
								bool flag15 = data4.CollectionItemList != null;
								if (flag15)
								{
									count += data4.CollectionItemList.Count;
								}
							}
						}
					}
				}
				IL_299:;
			}
			return count;
		}

		// Token: 0x06007DB2 RID: 32178 RVA: 0x004AD21C File Offset: 0x004AB41C
		[DomainMethod]
		public void SetBuildingAutoWork(DataContext context, short blockIndex, bool isAutoWork)
		{
			List<short> autoWorkList = DomainManager.Extra.GetAutoWorkBlockIndexList();
			bool flag = isAutoWork && !autoWorkList.Contains(blockIndex);
			if (flag)
			{
				autoWorkList.Add(blockIndex);
				DomainManager.Extra.SetAutoWorkBlockIndexList(autoWorkList, context);
			}
			bool flag2 = !isAutoWork && autoWorkList.Contains(blockIndex);
			if (flag2)
			{
				autoWorkList.Remove(blockIndex);
				DomainManager.Extra.SetAutoWorkBlockIndexList(autoWorkList, context);
			}
		}

		// Token: 0x06007DB3 RID: 32179 RVA: 0x004AD288 File Offset: 0x004AB488
		[DomainMethod]
		public bool GetBuildingIsAutoWork(short blockIndex)
		{
			List<short> autoWorkList = DomainManager.Extra.GetAutoWorkBlockIndexList();
			return autoWorkList.Contains(blockIndex);
		}

		// Token: 0x06007DB4 RID: 32180 RVA: 0x004AD2AC File Offset: 0x004AB4AC
		[DomainMethod]
		public void SetBuildingAutoSold(DataContext context, short blockIndex, bool isAutoSold)
		{
			List<short> autoSoldList = DomainManager.Extra.GetAutoSoldBlockIndexList();
			bool flag = isAutoSold && !autoSoldList.Contains(blockIndex);
			if (flag)
			{
				autoSoldList.Add(blockIndex);
				DomainManager.Extra.SetAutoSoldBlockIndexList(autoSoldList, context);
			}
			bool flag2 = !isAutoSold && autoSoldList.Contains(blockIndex);
			if (flag2)
			{
				autoSoldList.Remove(blockIndex);
				DomainManager.Extra.SetAutoSoldBlockIndexList(autoSoldList, context);
			}
		}

		// Token: 0x06007DB5 RID: 32181 RVA: 0x004AD318 File Offset: 0x004AB518
		[DomainMethod]
		public bool GetBuildingIsAutoSold(short blockIndex)
		{
			List<short> autoSoldList = DomainManager.Extra.GetAutoSoldBlockIndexList();
			return autoSoldList.Contains(blockIndex);
		}

		// Token: 0x06007DB6 RID: 32182 RVA: 0x004AD33C File Offset: 0x004AB53C
		[DomainMethod]
		public bool GetResidenceIsAutoCheckIn(short blockIndex)
		{
			List<short> autoCheckInList = DomainManager.Extra.GetAutoCheckInResidenceList();
			return autoCheckInList.Contains(blockIndex);
		}

		// Token: 0x06007DB7 RID: 32183 RVA: 0x004AD360 File Offset: 0x004AB560
		[DomainMethod]
		public bool GetComfortableIsAutoCheckIn(short blockIndex)
		{
			List<short> autoCheckInList = DomainManager.Extra.GetAutoCheckInComfortableList();
			return autoCheckInList.Contains(blockIndex);
		}

		// Token: 0x06007DB8 RID: 32184 RVA: 0x004AD384 File Offset: 0x004AB584
		[DomainMethod]
		public void SetUnlockedWorkingVillagers(DataContext context, int charId, bool add)
		{
			bool flag = charId < 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("CharId is illegal, id: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<int> list = DomainManager.Extra.GetUnlockedWorkingVillagers();
			bool flag2 = add && !list.Contains(charId);
			if (flag2)
			{
				list.Add(charId);
			}
			bool flag3 = !add && list.Contains(charId);
			if (flag3)
			{
				list.Remove(charId);
			}
			DomainManager.Extra.SetUnlockedWorkingVillagers(list, context);
		}

		// Token: 0x06007DB9 RID: 32185 RVA: 0x004AD418 File Offset: 0x004AB618
		public void TryRemoveUnlockedWorkingVillager(DataContext context, int charId)
		{
			bool flag = charId < 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("CharId is illegal, id: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<int> list = DomainManager.Extra.GetUnlockedWorkingVillagers();
			bool flag2 = list.Contains(charId);
			if (flag2)
			{
				list.Remove(charId);
				DomainManager.Extra.SetUnlockedWorkingVillagers(list, context);
			}
		}

		// Token: 0x06007DBA RID: 32186 RVA: 0x004AD488 File Offset: 0x004AB688
		[DomainMethod]
		public List<sbyte> GetXiangshuIdInKungfuRoom()
		{
			return DomainManager.Extra.GetXiangshuIdInKungfuPracticeRoom();
		}

		// Token: 0x06007DBB RID: 32187 RVA: 0x004AD4A4 File Offset: 0x004AB6A4
		[DomainMethod]
		[Obsolete]
		public void SetShopIsResultFirst(DataContext context, short blockIndex, bool resultFirst)
		{
			List<short> efficiencyFirstList = DomainManager.Extra.GetShopArrangeResultFirstList();
			bool flag = resultFirst && !efficiencyFirstList.Contains(blockIndex);
			if (flag)
			{
				efficiencyFirstList.Add(blockIndex);
				DomainManager.Extra.SetShopArrangeResultFirstList(efficiencyFirstList, context);
			}
			bool flag2 = !resultFirst && efficiencyFirstList.Contains(blockIndex);
			if (flag2)
			{
				efficiencyFirstList.Remove(blockIndex);
				DomainManager.Extra.SetShopArrangeResultFirstList(efficiencyFirstList, context);
			}
		}

		// Token: 0x06007DBC RID: 32188 RVA: 0x004AD510 File Offset: 0x004AB710
		[DomainMethod]
		[Obsolete]
		public bool GetShopIsResultFirst(short blockIndex)
		{
			List<short> efficiencyFirstList = DomainManager.Extra.GetShopArrangeResultFirstList();
			return efficiencyFirstList.Contains(blockIndex);
		}

		// Token: 0x06007DBD RID: 32189 RVA: 0x004AD534 File Offset: 0x004AB734
		[DomainMethod]
		[Obsolete]
		public void SetBuildingAutoExpand(DataContext context, short blockIndex, bool isAutoExpand)
		{
			List<short> autoExpandList = DomainManager.Extra.GetAutoExpandBlockIndexList();
			bool flag = isAutoExpand && !autoExpandList.Contains(blockIndex);
			if (flag)
			{
				autoExpandList.Insert(0, blockIndex);
				DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandList, context);
			}
			bool flag2 = !isAutoExpand && autoExpandList.Contains(blockIndex);
			if (flag2)
			{
				autoExpandList.Remove(blockIndex);
				DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandList, context);
				Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, blockIndex);
				BuildingBlockData blockData;
				bool flag3 = this.TryGetElement_BuildingBlocks(blockKey, out blockData);
				if (flag3)
				{
					bool flag4 = blockData.OperationType == -1;
					if (flag4)
					{
						this.RemoveAllOperatorsInBuilding(context, blockKey);
					}
				}
			}
		}

		// Token: 0x06007DBE RID: 32190 RVA: 0x004AD5F0 File Offset: 0x004AB7F0
		[DomainMethod]
		[Obsolete]
		public bool GetBuildingIsAutoExpand(short blockIndex)
		{
			List<short> autoExpandList = DomainManager.Extra.GetAutoExpandBlockIndexList();
			return autoExpandList.Contains(blockIndex);
		}

		// Token: 0x06007DBF RID: 32191 RVA: 0x004AD614 File Offset: 0x004AB814
		[DomainMethod]
		[Obsolete]
		public void SetBuildingAutoExpandUp(DataContext context, short blockIndex)
		{
			List<short> autoExpandList = DomainManager.Extra.GetAutoExpandBlockIndexList();
			int index = autoExpandList.IndexOf(blockIndex);
			bool flag = index <= 0;
			if (!flag)
			{
				List<short> list = autoExpandList;
				int index2 = index;
				List<short> list2 = autoExpandList;
				int index3 = index - 1;
				short value = autoExpandList[index - 1];
				short value2 = autoExpandList[index];
				list[index2] = value;
				list2[index3] = value2;
				DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandList, context);
			}
		}

		// Token: 0x06007DC0 RID: 32192 RVA: 0x004AD684 File Offset: 0x004AB884
		[DomainMethod]
		public void SetBuildingAutoExpandDown(DataContext context, short blockIndex)
		{
			List<short> autoExpandList = DomainManager.Extra.GetAutoExpandBlockIndexList();
			int index = autoExpandList.IndexOf(blockIndex);
			bool flag = index < 0 || index >= autoExpandList.Count - 1;
			if (!flag)
			{
				List<short> list = autoExpandList;
				int index2 = index;
				List<short> list2 = autoExpandList;
				int index3 = index + 1;
				short value = autoExpandList[index + 1];
				short value2 = autoExpandList[index];
				list[index2] = value;
				list2[index3] = value2;
				DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandList, context);
			}
		}

		// Token: 0x06007DC1 RID: 32193 RVA: 0x004AD704 File Offset: 0x004AB904
		[DomainMethod]
		[Obsolete]
		public void SetBuildingAutoExpandUpTop(DataContext context, short blockIndex)
		{
			List<short> autoExpandList = DomainManager.Extra.GetAutoExpandBlockIndexList();
			int index = autoExpandList.IndexOf(blockIndex);
			bool flag = index <= 0;
			if (!flag)
			{
				short tmp = autoExpandList[index];
				for (int i = index - 1; i >= 0; i--)
				{
					autoExpandList[i + 1] = autoExpandList[i];
				}
				autoExpandList[0] = tmp;
				DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandList, context);
			}
		}

		// Token: 0x06007DC2 RID: 32194 RVA: 0x004AD780 File Offset: 0x004AB980
		[DomainMethod]
		[Obsolete]
		public void SetBuildingAutoExpandDownBottom(DataContext context, short blockIndex)
		{
			List<short> autoExpandList = DomainManager.Extra.GetAutoExpandBlockIndexList();
			int index = autoExpandList.IndexOf(blockIndex);
			bool flag = index < 0 || index >= autoExpandList.Count - 1;
			if (!flag)
			{
				short tmp = autoExpandList[index];
				for (int i = index; i < autoExpandList.Count - 1; i++)
				{
					autoExpandList[i] = autoExpandList[i + 1];
				}
				List<short> list = autoExpandList;
				list[list.Count - 1] = tmp;
				DomainManager.Extra.SetAutoExpandBlockIndexList(autoExpandList, context);
			}
		}

		// Token: 0x06007DC3 RID: 32195 RVA: 0x004AD814 File Offset: 0x004ABA14
		private bool CanUpdateShopProgress(BuildingBlockKey blockKey)
		{
			BuildingBlockData blockData;
			bool flag = !this.TryGetElement_BuildingBlocks(blockKey, out blockData);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this.HasShopManagerLeader(blockKey);
				if (flag2)
				{
					result = false;
				}
				else
				{
					BuildingBlockItem configData = blockData.ConfigData;
					bool flag3 = !blockData.CanUse() || !configData.NeedShopProgress;
					if (flag3)
					{
						result = false;
					}
					else
					{
						sbyte b;
						bool flag4 = !this.AllDependBuildingAvailable(blockKey, blockData.TemplateId, out b);
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = configData.TemplateId == 105;
							if (flag5)
							{
								result = true;
							}
							else
							{
								ShopEventItem shopEventCfg = ShopEvent.Instance[configData.SuccesEvent[0]];
								sbyte slot = GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(configData.TemplateId);
								bool flag6 = shopEventCfg.ExchangeResourceGoods >= 0;
								if (flag6)
								{
									BuildingEarningsData earningsData;
									result = (this.TryGetElement_CollectBuildingEarningsData(blockKey, out earningsData) && earningsData != null && earningsData.ShopSoldItemList != null && !earningsData.ShopSoldItemList.All(ItemKey.Invalid));
								}
								else
								{
									bool flag7 = shopEventCfg.ResourceGoods >= 0;
									if (flag7)
									{
										BuildingEarningsData earningsData2;
										result = (!this.TryGetElement_CollectBuildingEarningsData(blockKey, out earningsData2) || earningsData2.CollectionResourceList == null || earningsData2.CollectionResourceList.Count < (int)slot);
									}
									else
									{
										bool flag8 = shopEventCfg.ItemList.Count > 0 || shopEventCfg.ItemGradeProbList.Count > 0;
										if (flag8)
										{
											BuildingEarningsData earningsData3;
											result = (!this.TryGetElement_CollectBuildingEarningsData(blockKey, out earningsData3) || earningsData3.CollectionItemList == null || earningsData3.CollectionItemList.Count < (int)slot);
										}
										else
										{
											bool flag9 = shopEventCfg.RecruitPeopleProb.Count > 0;
											BuildingEarningsData earningsData4;
											result = (flag9 && (!this.TryGetElement_CollectBuildingEarningsData(blockKey, out earningsData4) || earningsData4.RecruitLevelList == null || earningsData4.RecruitLevelList.Count < (int)slot));
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007DC4 RID: 32196 RVA: 0x004AD9F4 File Offset: 0x004ABBF4
		public int CalcRecruitGrade(IRandomSource random, BuildingBlockKey blockKey, ref int score)
		{
			bool flag;
			int attainment = this.BuildingTotalAttainment(blockKey, -1, out flag, false);
			score += attainment * (random.NextBool() ? 80 : 120) / 100;
			int[] thresholds = GlobalConfig.Instance.RecruitCharacterGradeScoreThresholds;
			sbyte grade;
			for (grade = 8; grade >= 0; grade -= 1)
			{
				bool flag2 = score > thresholds[(int)grade];
				if (flag2)
				{
					break;
				}
			}
			score = Math.Max(score - thresholds[(int)grade], 0);
			return (int)grade;
		}

		// Token: 0x06007DC5 RID: 32197 RVA: 0x004ADA6C File Offset: 0x004ABC6C
		public int CalcSoldItemValue(IRandomSource random, BuildingBlockKey blockKey, BuildingBlockData blockData, ItemKey itemKey, out int basePrice, out BuildingProduceDependencyData dependencyData)
		{
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			int basicPrice = baseItem.GetValue();
			basePrice = Math.Max(basicPrice, 1);
			int num;
			return this.CalcSoldItemValueBySpecificBaseLine(random, blockKey, blockData, out dependencyData, basePrice, out num);
		}

		// Token: 0x06007DC6 RID: 32198 RVA: 0x004ADAAC File Offset: 0x004ABCAC
		public int CalcSoldItemValueBySpecificBaseLine(IRandomSource random, BuildingBlockKey blockKey, BuildingBlockData blockData, out BuildingProduceDependencyData dependencyData, int baseLine, out int saleMiddleValue)
		{
			dependencyData = BuildingProduceDependencyData.Invalid;
			dependencyData.RandomFactorLowerLimit = (float)GlobalConfig.Instance.BuildingOutputRandomFactorLowerLimit;
			dependencyData.RandomFactorUpperLimit = (float)GlobalConfig.Instance.BuildingOutputRandomFactorUpperLimit;
			dependencyData.TemplateId = blockData.TemplateId;
			dependencyData.Level = this.BuildingBlockLevel(blockKey);
			BuildingBlockItem buildingBlockConfig = BuildingBlock.Instance.GetItem(blockData.TemplateId);
			dependencyData.ProductivityFactor = this.BuildingProductivityByMaxDependencies(blockKey);
			dependencyData.SafetyCultureFactor = this.CalcSafetyOrCultureFactor(buildingBlockConfig);
			bool flag;
			dependencyData.TotalAttainmentFactor = dependencyData.BuildSaleItemAttainmentFactor(this.BuildingTotalAttainment(blockKey, -1, out flag, false));
			dependencyData.GainResourcePercentFactor = DomainManager.World.GetGainResourcePercent(2);
			saleMiddleValue = dependencyData.CalcSaleItemPrice(baseLine);
			int saleValue = this.BuildingRandomCorrection(saleMiddleValue, random);
			BuildingBlockItem blockItem = BuildingBlock.Instance[blockData.TemplateId];
			ShopEventItem shopEventItem = ShopEvent.Instance[blockItem.SuccesEvent[0]];
			sbyte resourceType = GameData.Domains.Building.SharedMethods.GetBuildingResourceGoodsOrExchangeResourceGoodsType(blockItem, shopEventItem);
			CValuePercentBonus resBuildingBonus = DomainManager.Building.GetBuildingBlockEffect(blockKey.GetLocation(), (resourceType == 7) ? EBuildingScaleEffect.BuildingAuthorityIncomeBonus : EBuildingScaleEffect.BuildingMoneyIncomeBonus, -1);
			return saleValue * resBuildingBonus;
		}

		// Token: 0x06007DC7 RID: 32199 RVA: 0x004ADBDC File Offset: 0x004ABDDC
		private int CalcSafetyOrCultureFactor(BuildingBlockItem config)
		{
			int addition;
			GameData.Domains.Building.SharedMethods.PickSafetyOrCultureFactorSettlements(config, DomainManager.Taiwu.GetAllVisitedSettlements(), out addition);
			return 100 + addition;
		}

		// Token: 0x06007DC8 RID: 32200 RVA: 0x004ADC08 File Offset: 0x004ABE08
		public ItemKey GetRandomItemByGrade(IRandomSource randomSource, sbyte grade, short itemSubType = -1)
		{
			bool flag = itemSubType == -1;
			if (flag)
			{
				itemSubType = ItemSubType.GetRandom(randomSource);
				while (!ItemSubType.IsHobbyType(itemSubType))
				{
					itemSubType = ItemSubType.GetRandom(randomSource);
				}
			}
			short templateId = ItemDomain.GetRandomItemIdInSubType(randomSource, itemSubType, grade);
			sbyte itemType = ItemSubType.GetType(itemSubType);
			ItemKey itemKey = new ItemKey(itemType, 0, templateId, -1);
			return itemKey;
		}

		// Token: 0x06007DC9 RID: 32201 RVA: 0x004ADC68 File Offset: 0x004ABE68
		public BuildingBlockData GetBuildingBlockData(int templateId)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = (int)blockData.TemplateId == templateId;
				if (flag)
				{
					return blockData;
				}
			}
			return null;
		}

		// Token: 0x06007DCA RID: 32202 RVA: 0x004ADCEC File Offset: 0x004ABEEC
		public bool IsShopNeedManager(BuildingBlockKey blockKey, BuildingBlockData blockData)
		{
			BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
			return this.GetBuildingAttainmentUniversalWhetherCanWork(blockData, blockKey, false) < (int)configData.MaxProduceValue;
		}

		// Token: 0x06007DCB RID: 32203 RVA: 0x004ADD20 File Offset: 0x004ABF20
		public sbyte GetNeedLifeSkillType(BuildingBlockItem config, BuildingBlockKey blockKey)
		{
			return config.RequireLifeSkillType;
		}

		// Token: 0x06007DCC RID: 32204 RVA: 0x004ADD38 File Offset: 0x004ABF38
		public bool IsTaiwuVillageHaveSpecifyBuilding(short templateId, bool notBuild = false)
		{
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = blockData.TemplateId == templateId;
				if (flag)
				{
					return !notBuild || blockData.OperationType != 0;
				}
			}
			return false;
		}

		// Token: 0x06007DCD RID: 32205 RVA: 0x004ADDD4 File Offset: 0x004ABFD4
		public BuildingBlockKey GetMoreBetterSoldBuilding(BuildingBlockKey orgiBlockKey, BuildingBlockData orgiBlockData)
		{
			BuildingBlockKey betterBlockKey = BuildingBlockKey.Invalid;
			int orgiSoldValue = (int)(40 + GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(orgiBlockData.TemplateId) * 2) * (100 + this.GetBuildingAttainmentUniversalWhetherCanWork(orgiBlockData, orgiBlockKey, false) / 4) / 100;
			Location location = DomainManager.Taiwu.GetTaiwuVillageLocation();
			BuildingAreaData areaData = DomainManager.Building.GetElement_BuildingAreas(location);
			List<short> autoSoldList = DomainManager.Extra.GetAutoSoldBlockIndexList();
			for (short index = 0; index < (short)(areaData.Width * areaData.Width); index += 1)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData blockData = DomainManager.Building.GetElement_BuildingBlocks(blockKey);
				bool flag = !autoSoldList.Contains(blockKey.BuildingBlockIndex);
				if (!flag)
				{
					BuildingEarningsData earnData = this.GetBuildingEarningData(blockKey);
					bool flag2 = blockKey.BuildingBlockIndex != orgiBlockKey.BuildingBlockIndex && blockData.TemplateId == orgiBlockData.TemplateId && this.GetShopSoldItemCount(earnData) + this.GetShopSoldEarnCount(earnData) < (int)GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(blockData.TemplateId);
					if (flag2)
					{
						int soldValue = this.GetBuildingAttainmentUniversalWhetherCanWork(blockData, blockKey, false) * (int)GameData.Domains.Building.SharedMethods.GetBuildingSlotCount(orgiBlockData.TemplateId);
						bool flag3 = soldValue > orgiSoldValue;
						if (flag3)
						{
							orgiSoldValue = soldValue;
							betterBlockKey = blockKey;
						}
					}
				}
			}
			return betterBlockKey;
		}

		// Token: 0x06007DCE RID: 32206 RVA: 0x004ADF1C File Offset: 0x004AC11C
		[DomainMethod]
		public bool NearDependBuildings(BuildingAreaData areaData, Location location, short rootIndex, BuildingBlockItem configData)
		{
			List<short> dependBuildings = configData.DependBuildings;
			bool flag = dependBuildings.Count == 0;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				List<short> neighborList = ObjectPool<List<short>>.Instance.Get();
				areaData.GetNeighborBlocks(rootIndex, configData.Width, neighborList, null, 2);
				bool[] dependBuildingFound = new bool[dependBuildings.Count];
				for (int i = 0; i < neighborList.Count; i++)
				{
					BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, neighborList[i]);
					BuildingBlockData blockData;
					bool isGet = this.TryGetElement_BuildingBlocks(blockKey, out blockData);
					bool flag2 = isGet;
					if (flag2)
					{
						int dependIndex = dependBuildings.IndexOf(blockData.TemplateId);
						int dependIndexRoot = -1;
						bool flag3 = blockData.RootBlockIndex >= 0;
						if (flag3)
						{
							BuildingBlockKey rootBlockKey = new BuildingBlockKey(location.AreaId, location.BlockId, blockData.RootBlockIndex);
							BuildingBlockData blockDataRoot;
							bool isGetRoot = this.TryGetElement_BuildingBlocks(rootBlockKey, out blockDataRoot);
							bool flag4 = isGetRoot;
							if (flag4)
							{
								dependIndexRoot = dependBuildings.IndexOf(blockDataRoot.TemplateId);
							}
						}
						bool flag5 = dependIndex >= 0;
						if (flag5)
						{
							dependBuildingFound[dependIndex] = true;
						}
						bool flag6 = dependIndexRoot >= 0;
						if (flag6)
						{
							dependBuildingFound[dependIndexRoot] = true;
						}
					}
				}
				ObjectPool<List<short>>.Instance.Return(neighborList);
				result = !dependBuildingFound.Exist(false);
			}
			return result;
		}

		// Token: 0x06007DCF RID: 32207 RVA: 0x004AE074 File Offset: 0x004AC274
		[DomainMethod]
		public void AddFixBook(DataContext context, BuildingBlockKey key, ItemKey itemKey, ItemSourceType itemSourceType)
		{
			BuildingEarningsData earningsData;
			bool flag = !this.TryGetElement_CollectBuildingEarningsData(key, out earningsData);
			if (flag)
			{
				earningsData = new BuildingEarningsData();
				this.AddElement_CollectBuildingEarningsData(key, earningsData, context);
			}
			DomainManager.Taiwu.RemoveItem(context, itemKey, 1, itemSourceType, false, false);
			this.AddBuildingEarningsDataFixBook(context, key, earningsData, itemKey);
		}

		// Token: 0x06007DD0 RID: 32208 RVA: 0x004AE0C4 File Offset: 0x004AC2C4
		[DomainMethod]
		public ValueTuple<short, BuildingBlockData> ChangeFixBook(DataContext context, BuildingBlockKey key, ItemKey itemKey, ItemSourceType itemSourceType)
		{
			BuildingBlockData blockData = null;
			bool flag = this.TryGetElement_BuildingBlocks(key, out blockData);
			if (flag)
			{
				blockData.OfflineResetShopProgress();
				this.SetElement_BuildingBlocks(key, blockData, context);
			}
			BuildingEarningsData earningsData;
			bool flag2 = this.TryGetElement_CollectBuildingEarningsData(key, out earningsData);
			if (flag2)
			{
				bool flag3 = earningsData.FixBookInfoList.Count <= 0;
				if (flag3)
				{
					return new ValueTuple<short, BuildingBlockData>(key.BuildingBlockIndex, blockData);
				}
				ItemKey book = earningsData.FixBookInfoList[0];
				this.RemoveBuildingEarningsDataFixBook(context, key, earningsData, book);
				DomainManager.Taiwu.AddItem(context, book, 1, itemSourceType, false);
				bool flag4 = itemKey.IsValid();
				if (flag4)
				{
					DomainManager.Taiwu.RemoveItem(context, itemKey, 1, itemSourceType, false, false);
					this.AddBuildingEarningsDataFixBook(context, key, earningsData, itemKey);
				}
			}
			return new ValueTuple<short, BuildingBlockData>(key.BuildingBlockIndex, blockData);
		}

		// Token: 0x06007DD1 RID: 32209 RVA: 0x004AE198 File Offset: 0x004AC398
		[DomainMethod]
		public void ReceiveFixBook(DataContext context, BuildingBlockKey key, bool isPutInInventory)
		{
			BuildingEarningsData earningsData;
			bool flag = this.TryGetElement_CollectBuildingEarningsData(key, out earningsData);
			if (flag)
			{
				bool flag2 = earningsData.FixBookInfoList.Count <= 0;
				if (!flag2)
				{
					ItemKey book = earningsData.FixBookInfoList[0];
					earningsData.FixBookInfoList.Clear();
					this.RemoveBuildingEarningsDataFixBook(context, key, earningsData, book);
					if (isPutInInventory)
					{
						GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
						taiwu.AddInventoryItem(context, book, 1, false);
					}
					else
					{
						DomainManager.Taiwu.WarehouseAdd(context, book, 1);
					}
				}
			}
		}

		// Token: 0x06007DD2 RID: 32210 RVA: 0x004AE224 File Offset: 0x004AC424
		[DomainMethod]
		public int GetFixBookProgress(DataContext context, BuildingBlockKey key)
		{
			BuildingBlockData data;
			bool flag = this.TryGetElement_BuildingBlocks(key, out data);
			if (flag)
			{
				BuildingEarningsData earningsDate;
				bool flag2 = this.TryGetElement_CollectBuildingEarningsData(key, out earningsDate);
				if (flag2)
				{
					bool flag3 = earningsDate.FixBookInfoList.Count > 0 && earningsDate.FixBookInfoList[0].IsValid();
					if (flag3)
					{
						GameData.Domains.Item.SkillBook skillBook = DomainManager.Item.GetElement_SkillBooks(earningsDate.FixBookInfoList[0].Id);
						return (int)(data.ShopProgress * 100 / skillBook.GetFixProgress().Item2);
					}
				}
			}
			return 0;
		}

		// Token: 0x06007DD3 RID: 32211 RVA: 0x004AE2BB File Offset: 0x004AC4BB
		private void AddBuildingEarningsDataFixBook(DataContext context, BuildingBlockKey key, BuildingEarningsData earningsData, ItemKey itemKey)
		{
			earningsData.FixBookInfoList.Clear();
			earningsData.FixBookInfoList.Add(itemKey);
			this.SetElement_CollectBuildingEarningsData(key, earningsData, context);
			DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 105);
		}

		// Token: 0x06007DD4 RID: 32212 RVA: 0x004AE2F3 File Offset: 0x004AC4F3
		private void RemoveBuildingEarningsDataFixBook(DataContext context, BuildingBlockKey key, BuildingEarningsData earningsData, ItemKey itemKey)
		{
			earningsData.FixBookInfoList.Clear();
			this.SetElement_CollectBuildingEarningsData(key, earningsData, context);
			DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, 105);
		}

		// Token: 0x06007DD5 RID: 32213 RVA: 0x004AE320 File Offset: 0x004AC520
		[DomainMethod]
		public List<ItemDisplayData> GetTaiwuCanFixBookItemDataList(ItemSourceType itemSourceType)
		{
			List<ItemDisplayData> list = DomainManager.Taiwu.GetAllItems(itemSourceType, false).Item2;
			list.RemoveAll(delegate(ItemDisplayData d)
			{
				short itemSubType = ItemTemplateHelper.GetItemSubType(d.Key.ItemType, d.Key.TemplateId);
				bool flag = itemSubType - 1000 <= 1;
				bool flag2 = flag;
				bool result;
				if (flag2)
				{
					SkillBookPageDisplayData bookInfo = DomainManager.Item.GetSkillBookPagesInfo(d.Key);
					result = !bookInfo.CanFix();
				}
				else
				{
					result = true;
				}
				return result;
			});
			return list;
		}

		// Token: 0x06007DD6 RID: 32214 RVA: 0x004AE36C File Offset: 0x004AC56C
		[DomainMethod]
		public void SetTeaHorseCaravanState(DataContext context, sbyte state)
		{
			bool flag = this._teaHorseCaravanData == null;
			if (!flag)
			{
				this._teaHorseCaravanData.CaravanState = state;
				this.SetTeaHorseCaravanData(this._teaHorseCaravanData, context);
			}
		}

		// Token: 0x06007DD7 RID: 32215 RVA: 0x004AE3A4 File Offset: 0x004AC5A4
		[DomainMethod]
		public void SetTeaHorseCaravanWeather(DataContext context, short weatherId)
		{
			bool flag = this._teaHorseCaravanData == null;
			if (!flag)
			{
				this._teaHorseCaravanData.Weather = weatherId;
				this.SetTeaHorseCaravanData(this._teaHorseCaravanData, context);
			}
		}

		// Token: 0x06007DD8 RID: 32216 RVA: 0x004AE3DC File Offset: 0x004AC5DC
		[DomainMethod]
		public void GetBackTeaHorseCarryItem(DataContext context, ItemKey itemKey, sbyte itemSource)
		{
			this.TeaHorseCarryRemove(context, itemKey);
			bool flag = itemSource == 2;
			if (flag)
			{
				DomainManager.Taiwu.WarehouseAdd(context, itemKey, 1);
			}
			else
			{
				bool flag2 = itemSource == 3;
				if (flag2)
				{
					DomainManager.Taiwu.StoreItemInTreasury(context, itemKey, 1, false);
				}
				else
				{
					bool flag3 = itemSource == 4;
					if (flag3)
					{
						DomainManager.Extra.AddStockStorageItem(context, 1, itemKey, 1);
					}
					else
					{
						DomainManager.Taiwu.GetTaiwu().AddInventoryItem(context, itemKey, 1, false);
					}
				}
			}
		}

		// Token: 0x06007DD9 RID: 32217 RVA: 0x004AE454 File Offset: 0x004AC654
		[DomainMethod]
		public void AddItemToTeaHorseCarryItem(DataContext context, ItemKey itemKey, sbyte itemSource)
		{
			bool flag = this._teaHorseCaravanData == null;
			if (flag)
			{
				this._teaHorseCaravanData = new TeaHorseCaravanData();
			}
			bool flag2 = itemSource == 2;
			if (flag2)
			{
				int count = DomainManager.Taiwu.GetWarehouseItemCount(itemKey);
				bool flag3 = count > 0;
				if (flag3)
				{
					DomainManager.Taiwu.WarehouseRemove(context, itemKey, 1, false);
					this.TeaHorseCarryAdd(context, itemKey, 2);
				}
			}
			else
			{
				bool flag4 = itemSource == 3;
				if (flag4)
				{
					int count2 = DomainManager.Taiwu.GetTreasuryItemCount(itemKey);
					bool flag5 = count2 > 0;
					if (flag5)
					{
						DomainManager.Taiwu.RemoveTaiwuItemFromTaiwuStorage(context, itemKey, 1, false);
						this.TeaHorseCarryAdd(context, itemKey, 3);
					}
				}
				else
				{
					bool flag6 = itemSource == 4;
					if (flag6)
					{
						int count3 = DomainManager.Extra.GetStockStorageGoosShelfItemCount(itemKey);
						bool flag7 = count3 > 0;
						if (flag7)
						{
							DomainManager.Extra.RemoveStockStorageItem(context, 1, itemKey, 1, false);
							this.TeaHorseCarryAdd(context, itemKey, 4);
						}
					}
					else
					{
						GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
						ItemKey[] equipments = taiwu.GetEquipment();
						int slotIndex = equipments.IndexOf(itemKey);
						bool flag8 = slotIndex >= 0;
						if (flag8)
						{
							taiwu.ChangeEquipment(context, (sbyte)slotIndex, -1, ItemKey.Invalid);
						}
						int count4 = taiwu.GetInventory().GetInventoryItemCount(itemKey);
						bool flag9 = count4 > 0;
						if (flag9)
						{
							taiwu.RemoveInventoryItem(context, itemKey, 1, false, false);
							this.TeaHorseCarryAdd(context, itemKey, 1);
						}
					}
				}
			}
		}

		// Token: 0x06007DDA RID: 32218 RVA: 0x004AE5B2 File Offset: 0x004AC7B2
		private void TeaHorseCarryAdd(DataContext context, ItemKey itemKey, sbyte from)
		{
			this._teaHorseCaravanData.CarryGoodsList.Add(new ValueTuple<ItemKey, sbyte>(itemKey, from));
			this.SetTeaHorseCaravanData(this._teaHorseCaravanData, context);
			DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Building, 51);
		}

		// Token: 0x06007DDB RID: 32219 RVA: 0x004AE5EC File Offset: 0x004AC7EC
		private void TeaHorseCarryRemove(DataContext context, ItemKey itemKey)
		{
			for (int i = 0; i < this._teaHorseCaravanData.CarryGoodsList.Count; i++)
			{
				bool flag = this._teaHorseCaravanData.CarryGoodsList[i].Item1.Equals(itemKey);
				if (flag)
				{
					this._teaHorseCaravanData.CarryGoodsList.RemoveAt(i);
					this.SetTeaHorseCaravanData(this._teaHorseCaravanData, context);
					DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Building, 51);
					break;
				}
			}
		}

		// Token: 0x06007DDC RID: 32220 RVA: 0x004AE674 File Offset: 0x004AC874
		[DomainMethod]
		public void ExchangeItemToReplenishment(DataContext context, List<ItemKey> carryItems, List<ItemKey> gainItems)
		{
			short getReplenishmentNum = 0;
			bool flag = carryItems != null;
			if (flag)
			{
				foreach (ItemKey item in carryItems)
				{
					foreach (ValueTuple<ItemKey, sbyte> ele in this._teaHorseCaravanData.CarryGoodsList)
					{
						ItemKey item3 = ele.Item1;
						bool flag2 = item3.Equals(item);
						if (flag2)
						{
							this._teaHorseCaravanData.CarryGoodsList.Remove(ele);
							DomainManager.Item.RemoveItem(context, item);
							getReplenishmentNum += this.GradeToReplenishment(ItemTemplateHelper.GetGrade(item.ItemType, item.TemplateId));
							break;
						}
					}
				}
			}
			bool flag3 = gainItems != null;
			if (flag3)
			{
				foreach (ItemKey item2 in gainItems)
				{
					bool flag4 = this._teaHorseCaravanData.ExchangeGoodsList.Contains(item2);
					if (flag4)
					{
						this._teaHorseCaravanData.ExchangeGoodsList.Remove(item2);
						getReplenishmentNum += this.GradeToReplenishment(ItemTemplateHelper.GetGrade(item2.ItemType, item2.TemplateId));
					}
				}
			}
			getReplenishmentNum = (short)Math.Min((int)getReplenishmentNum, (int)(this._caravanReplenishmentInitValue - this._teaHorseCaravanData.CaravanReplenishment));
			TeaHorseCaravanData teaHorseCaravanData = this._teaHorseCaravanData;
			teaHorseCaravanData.CaravanReplenishment += Math.Min(getReplenishmentNum, this._teaHorseCaravanData.ExchangeReplenishmentRemainAmount);
			this._teaHorseCaravanData.CaravanReplenishment = Math.Min(this._teaHorseCaravanData.CaravanReplenishment, this._caravanReplenishmentInitValue);
			this._teaHorseCaravanData.ExchangeReplenishmentRemainAmount = (short)Math.Max(0, (int)(this._teaHorseCaravanData.ExchangeReplenishmentRemainAmount - getReplenishmentNum));
			this.SetTeaHorseCaravanData(this._teaHorseCaravanData, context);
		}

		// Token: 0x06007DDD RID: 32221 RVA: 0x004AE890 File Offset: 0x004ACA90
		private short GradeToReplenishment(sbyte grade)
		{
			return (short)((grade + 1) * 5 + 5);
		}

		// Token: 0x06007DDE RID: 32222 RVA: 0x004AE8AC File Offset: 0x004ACAAC
		private void ResetTeaHorseCaravanData(DataContext context)
		{
			this._teaHorseCaravanData.DiaryList = new List<short>();
			this._teaHorseCaravanData.CaravanState = 0;
			this._teaHorseCaravanData.IsStartSearch = false;
			this._teaHorseCaravanData.CaravanAwareness = this._caravanAwarenessInitValue;
			this._teaHorseCaravanData.CaravanReplenishment = this._caravanReplenishmentInitValue;
			this._teaHorseCaravanData.LackReplenishmentTurn = 0;
			this._teaHorseCaravanData.IsShowExchangeReplenishment = false;
			this._teaHorseCaravanData.IsShowSeachReplenishment = false;
			this._teaHorseCaravanData.DistanceToTaiwuVillage = 0;
			this._teaHorseCaravanData.StartMonth = 0;
			this._teaHorseCaravanData.ExchangeReplenishmentAmountMax = 0;
			this._teaHorseCaravanData.SearchReplenishmentMax = 0;
			this._teaHorseCaravanData.SearchReplenishmentAmount = 0;
			this._teaHorseCaravanData.Terrain = 5;
			this.SetTeaHorseCaravanData(this._teaHorseCaravanData, context);
		}

		// Token: 0x06007DDF RID: 32223 RVA: 0x004AE980 File Offset: 0x004ACB80
		public ItemKey GetWestRandomItemByGarde(DataContext context, sbyte grade)
		{
			short templateId = this.WestItemArrayTwo[context.Random.Next(0, this.WestItemArrayTwo.Length)][(int)(grade - 4)];
			sbyte itemType = ItemSubType.GetType(1203);
			ItemKey itemKey = new ItemKey(itemType, 0, templateId, -1);
			return itemKey;
		}

		// Token: 0x06007DE0 RID: 32224 RVA: 0x004AE9CC File Offset: 0x004ACBCC
		[DomainMethod]
		public void StartSearchReplenishment(DataContext context)
		{
			this._teaHorseCaravanData.IsStartSearch = true;
			this._teaHorseCaravanData.CaravanReplenishment = (short)Math.Min((int)(this._teaHorseCaravanData.CaravanReplenishment + this._teaHorseCaravanData.SearchReplenishmentAmount), (int)this._caravanReplenishmentInitValue);
			this._teaHorseCaravanData.SearchReplenishmentMax = (short)Math.Max(0, (int)(this._teaHorseCaravanData.SearchReplenishmentMax - this._teaHorseCaravanData.SearchReplenishmentAmount));
			bool flag = this._teaHorseCaravanData.CaravanReplenishment >= this._caravanReplenishmentInitValue || this._teaHorseCaravanData.SearchReplenishmentMax <= 0;
			if (flag)
			{
				this._teaHorseCaravanData.IsStartSearch = false;
				this._teaHorseCaravanData.IsShowSeachReplenishment = false;
			}
			this.SetTeaHorseCaravanData(this._teaHorseCaravanData, context);
		}

		// Token: 0x06007DE1 RID: 32225 RVA: 0x004AEA90 File Offset: 0x004ACC90
		[DomainMethod]
		public void QuickGetExchangeItem(DataContext context)
		{
			bool flag = this._teaHorseCaravanData == null;
			if (!flag)
			{
				for (int i = 0; i < this._teaHorseCaravanData.ExchangeGoodsList.Count; i++)
				{
					ItemKey itemKey = this._teaHorseCaravanData.ExchangeGoodsList[i];
					DomainManager.Taiwu.CreateWarehouseItem(context, itemKey.ItemType, itemKey.TemplateId, 1);
				}
				this._teaHorseCaravanData.ExchangeGoodsList.Clear();
				this.SetTeaHorseCaravanData(this._teaHorseCaravanData, context);
				this.ResetTeaHorseCaravanData(context);
			}
		}

		// Token: 0x06007DE2 RID: 32226 RVA: 0x004AEB20 File Offset: 0x004ACD20
		[DomainMethod]
		public void DealInfectedPeople(DataContext context, List<int> charList, byte dealType)
		{
			bool flag = charList == null;
			if (!flag)
			{
				OrganizationInfo taiwuOrg = DomainManager.Taiwu.GetTaiwu().GetOrganizationInfo();
				Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
				OrganizationInfo tarOrgInfo = new OrganizationInfo(taiwuOrg.OrgTemplateId, 0, true, taiwuOrg.SettlementId);
				int secretSignCount = 0;
				int seniorityValue = 0;
				ProfessionFormulaItem formulaItem = ProfessionFormula.Instance[38];
				int faith = 0;
				int faithCount = 0;
				for (int i = 0; i < charList.Count; i++)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charList[i]);
					Location location = character.GetLocation();
					int value = DomainManager.Character.GetSpiritualDebtChangeByInfected(character, 50);
					int happinessNormal = (int)((HappinessType.Ranges[3].Item1 + HappinessType.Ranges[3].Item2) / 2);
					switch (dealType)
					{
					case 1:
						DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, value, true, true);
						DomainManager.Character.GroupMove(context, character, taiwuVillageLocation);
						DomainManager.Character.RemoveXiangshuInfection(context, character, 0);
						DomainManager.Organization.ChangeOrganization(context, character, tarOrgInfo);
						character.SetHappiness((sbyte)happinessNormal, context);
						secretSignCount += (int)ProfessionRelatedConstants.TaoistMonkSkill2GetSecretSignCountBySave[(int)character.GetConsummateLevel()];
						seniorityValue += formulaItem.Calculate((int)character.GetOrganizationInfo().Grade);
						character.SavedFromInfected(context, true);
						faith += character.SaveFromInfectedGainFaith;
						faithCount++;
						break;
					case 2:
						DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, value, true, true);
						DomainManager.Character.RemoveXiangshuInfection(context, character, 0);
						character.SetHappiness((sbyte)happinessNormal, context);
						secretSignCount += (int)ProfessionRelatedConstants.TaoistMonkSkill2GetSecretSignCountBySave[(int)character.GetConsummateLevel()];
						seniorityValue += formulaItem.Calculate((int)character.GetOrganizationInfo().Grade);
						character.SavedFromInfected(context, true);
						faith += character.SaveFromInfectedGainFaith;
						faithCount++;
						break;
					case 3:
					{
						DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, -value, true, true);
						List<Location> locations = ObjectPool<List<Location>>.Instance.Get();
						DomainManager.Map.GetAreaNotSettlementLocations(locations, location.AreaId);
						Location randomLocation = locations[context.Random.Next(0, locations.Count)];
						DomainManager.Map.OnInfectedCharacterLocationChanged(context, character.GetId(), character.GetLocation(), randomLocation);
						DomainManager.Character.GroupMove(context, character, randomLocation);
						ObjectPool<List<Location>>.Instance.Return(locations);
						DomainManager.Extra.TryRemoveStoneRoomCharacter(context, character, true);
						break;
					}
					case 4:
						DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, -value, true, true);
						DomainManager.Character.GroupMove(context, character, taiwuVillageLocation);
						DomainManager.Character.MakeCharacterDead(context, character, 9, new CharacterDeathInfo(character.GetValidLocation())
						{
							KillerId = DomainManager.Taiwu.GetTaiwuCharId()
						});
						secretSignCount += (int)ProfessionRelatedConstants.TaoistMonkSkill2GetSecretSignCount[(int)character.GetConsummateLevel()];
						seniorityValue += formulaItem.Calculate((int)character.GetOrganizationInfo().Grade) / 2;
						break;
					}
				}
				bool flag2 = seniorityValue > 0;
				if (flag2)
				{
					DomainManager.Extra.ChangeProfessionSeniority(context, 5, seniorityValue, true, false);
				}
				bool flag3 = secretSignCount > 0 && DomainManager.Extra.IsProfessionalSkillUnlocked(5, 2);
				if (flag3)
				{
					GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
					ItemKey itemKey = DomainManager.Item.CreateItem(context, 12, 234);
					taiwuChar.AddInventoryItem(context, itemKey, secretSignCount, false);
					DomainManager.TaiwuEvent.OnEvent_TaiwuGotTianjieFulu(-1, itemKey, secretSignCount);
					DomainManager.World.GetInstantNotificationCollection().AddGetItem(taiwuChar.GetId(), 12, 234);
				}
				bool flag4 = faith > 0;
				if (flag4)
				{
					DomainManager.World.GetInstantNotificationCollection().AddGainFuyuFaith2(faithCount, faith);
				}
			}
		}

		// Token: 0x06007DE3 RID: 32227 RVA: 0x004AEF0C File Offset: 0x004AD10C
		internal bool TryCalcShopManagementYieldAmount(IRandomSource random, BuildingBlockKey blockKey, out sbyte resourceType, out int amount, out BuildingProduceDependencyData dependencyData, int valuationInclination = 0)
		{
			resourceType = -1;
			amount = 0;
			dependencyData = BuildingProduceDependencyData.Invalid;
			BuildingBlockData blockData;
			BuildingBlockDataEx blockDataEx;
			bool flag = !this.TryGetElement_BuildingBlocks(blockKey, out blockData) || !DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)blockKey, out blockDataEx);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
				bool flag2 = configData.IsShop && configData.SuccesEvent.Count > 0;
				if (flag2)
				{
					ShopEventItem successShopEventConfig = ShopEvent.Instance[configData.SuccesEvent[0]];
					bool flag3 = valuationInclination == 1;
					if (flag3)
					{
						random = new MaxRandomGenerator();
					}
					else
					{
						bool flag4 = valuationInclination == -1;
						if (flag4)
						{
							random = new MinRandomGenerator();
						}
					}
					sbyte level = blockDataEx.CalcUnlockedLevelCount();
					bool flag5 = successShopEventConfig.ResourceGoods != -1;
					if (flag5)
					{
						bool flag6 = !this.HasShopManagerLeader(blockKey);
						if (flag6)
						{
							this.AddBuildingException(blockKey, blockData, BuildingExceptionType.ManageStoppedForNoLeader);
						}
						dependencyData.TemplateId = blockData.TemplateId;
						dependencyData.Level = level;
						dependencyData.ProductivityFactor = this.BuildingProductivityByMaxDependencies(blockKey);
						dependencyData.SafetyCultureFactor = this.CalcSafetyOrCultureFactor(configData);
						bool hasManager;
						dependencyData.TotalAttainmentFactor = this.BuildingTotalAttainment(blockKey, -1, out hasManager, false);
						dependencyData.GainResourcePercentFactor = DomainManager.World.GetGainResourcePercent(9);
						bool flag7 = blockData.TemplateId == 215;
						if (flag7)
						{
							dependencyData.RandomFactorUpperLimit = (float)(GlobalConfig.Instance.BuildingOutputRandomFactorUpperLimit * 3);
							dependencyData.RandomFactorLowerLimit = (float)(GlobalConfig.Instance.BuildingOutputRandomFactorLowerLimit / 2);
							resourceType = successShopEventConfig.ResourceGoods;
							amount = this.BuildingRandomCorrection(dependencyData.GamblingHouseOutput, random);
							CValuePercentBonus resBuildingBonus = DomainManager.Building.GetBuildingBlockEffect(blockKey.GetLocation(), EBuildingScaleEffect.BuildingMoneyIncomeBonus, -1);
							amount *= resBuildingBonus;
							bool flag8 = hasManager;
							if (flag8)
							{
								int compensation;
								bool flag9 = !DomainManager.Extra.TryGetElement_BuildingMoneyPrestigeSuccessRateCompensation((ulong)blockKey, out compensation);
								if (flag9)
								{
									compensation = 0;
								}
								if (!true)
								{
								}
								bool flag10 = valuationInclination != -1 && (valuationInclination == 1 || random.CheckPercentProb(this.BuildingManageHarvestSpecialSuccessRate(blockKey, -1) + compensation));
								if (!true)
								{
								}
								bool specialSuccessCheck = flag10;
								bool flag11 = specialSuccessCheck;
								if (flag11)
								{
									amount *= 3;
								}
								else
								{
									amount /= 2;
								}
								return true;
							}
						}
						else
						{
							bool flag12 = blockData.TemplateId == 216;
							if (flag12)
							{
								dependencyData.RandomFactorUpperLimit = (float)(GlobalConfig.Instance.BuildingOutputRandomFactorUpperLimit * 3);
								dependencyData.RandomFactorLowerLimit = (float)(GlobalConfig.Instance.BuildingOutputRandomFactorLowerLimit / 2);
								resourceType = successShopEventConfig.ResourceGoods;
								amount = this.BuildingRandomCorrection(dependencyData.BrothelOutput, random);
								CValuePercentBonus resBuildingBonus2 = DomainManager.Building.GetBuildingBlockEffect(blockKey.GetLocation(), EBuildingScaleEffect.BuildingMoneyIncomeBonus, -1);
								amount *= resBuildingBonus2;
								bool flag13 = hasManager;
								if (flag13)
								{
									int compensation2;
									bool flag14 = !DomainManager.Extra.TryGetElement_BuildingMoneyPrestigeSuccessRateCompensation((ulong)blockKey, out compensation2);
									if (flag14)
									{
										compensation2 = 0;
									}
									if (!true)
									{
									}
									bool flag10 = valuationInclination != -1 && (valuationInclination == 1 || random.CheckPercentProb(this.BuildingManageHarvestSpecialSuccessRate(blockKey, -1) + compensation2));
									if (!true)
									{
									}
									bool specialSuccessCheck2 = flag10;
									bool flag15 = specialSuccessCheck2;
									if (flag15)
									{
										amount *= 3;
									}
									else
									{
										amount /= 2;
									}
									return true;
								}
							}
							else
							{
								resourceType = successShopEventConfig.ResourceGoods;
								dependencyData.RandomFactorUpperLimit = (float)GlobalConfig.Instance.BuildingOutputRandomFactorUpperLimit;
								dependencyData.RandomFactorLowerLimit = (float)GlobalConfig.Instance.BuildingOutputRandomFactorLowerLimit;
								amount = ((resourceType == 7) ? dependencyData.AuthorityBuildingOutput : dependencyData.MoneyBuildingOutput);
								CValuePercentBonus resBuildingBonus3 = DomainManager.Building.GetBuildingBlockEffect(blockKey.GetLocation(), (resourceType == 7) ? EBuildingScaleEffect.BuildingAuthorityIncomeBonus : EBuildingScaleEffect.BuildingMoneyIncomeBonus, -1);
								amount *= resBuildingBonus3;
								amount = this.BuildingRandomCorrection(amount, random);
								bool flag16 = dependencyData.ProductivityFactor == 0;
								if (flag16)
								{
									this.AddBuildingException(blockKey, blockData, BuildingExceptionType.ManageStoppedForDependency);
								}
								bool flag17 = hasManager;
								if (flag17)
								{
									return true;
								}
							}
						}
					}
					result = false;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06007DE4 RID: 32228 RVA: 0x004AF32C File Offset: 0x004AD52C
		[DomainMethod]
		public BuildingManageYieldTipsData GetShopManagementYieldTipsData(DataContext context, BuildingBlockKey blockKey)
		{
			BuildingManageYieldTipsData tipsData = new BuildingManageYieldTipsData(0);
			BuildingBlockData blockData;
			bool flag = this.TryGetElement_BuildingBlocks(blockKey, out blockData);
			if (flag)
			{
				BuildingBlockItem configData = BuildingBlock.Instance.GetItem(blockData.TemplateId);
				sbyte lifeSkillType = this.GetNeedLifeSkillType(configData, blockKey);
				ShopEventItem successShopEventConfig = ShopEvent.Instance[configData.SuccesEvent[0]];
				int addition;
				List<SettlementDisplayData> settlements = GameData.Domains.Building.SharedMethods.PickSafetyOrCultureFactorSettlements(configData, DomainManager.Taiwu.GetAllVisitedSettlements(), out addition);
				bool flag2 = settlements.Count > 0 && addition > 0;
				if (flag2)
				{
					bool isCulture = configData.RequireCulture != 0;
					tipsData.SafetyOrCultureFactorSettlementsAndPickValue = new Dictionary<int, SettlementDisplayData>();
					foreach (SettlementDisplayData settlement in settlements)
					{
						int value = GameData.Domains.Building.SharedMethods.CalcSafetyOrCultureFactorSettlementPickValue((short)(isCulture ? configData.RequireCulture : configData.RequireSafety), isCulture ? settlement.Culture : settlement.Safety);
						bool flag3 = value <= 0;
						if (!flag3)
						{
							tipsData.SafetyOrCultureFactorSettlementsAndPickValue.Add(settlement.SettlementId, settlement);
						}
					}
				}
				bool isCollectResourceBuilding = configData.IsCollectResourceBuilding;
				if (isCollectResourceBuilding)
				{
					sbyte resourceType = this.GetCollectBuildingResourceType(blockKey);
					bool flag4 = resourceType > 5;
					if (flag4)
					{
						resourceType = 5;
					}
					tipsData.ResourceOutputValuation = this.CalcResourceOutputCount(blockKey, resourceType, tipsData.ProduceDependencies);
					tipsData.ProduceResourceType = resourceType;
					tipsData.ManagerAttainment = 0;
					CharacterList managerList;
					bool flag5 = this.TryGetElement_ShopManagerDict(blockKey, out managerList);
					if (flag5)
					{
						for (int i = 0; i < managerList.GetCount(); i++)
						{
							int charId = managerList.GetCollection()[i];
							GameData.Domains.Character.Character character;
							bool flag6 = GameData.Domains.Character.Character.IsCharacterIdValid(charId) && DomainManager.Character.TryGetElement_Objects(charId, out character);
							if (flag6)
							{
								tipsData.ManagerAttainment += (int)character.GetLifeSkillAttainment(lifeSkillType);
							}
						}
					}
				}
				else
				{
					bool flag7 = GameData.Domains.Building.SharedMethods.IsBuildingProduceMoneyAuthority(configData, successShopEventConfig);
					if (flag7)
					{
						sbyte b;
						this.TryCalcShopManagementYieldAmount(context.Random, blockKey, out b, out tipsData.ManageProduceValuationMin, out tipsData.BuildingProduceDependencyData, -1);
						this.TryCalcShopManagementYieldAmount(context.Random, blockKey, out b, out tipsData.ManageProduceValuationMax, out tipsData.BuildingProduceDependencyData, 1);
						tipsData.ProduceResourceType = successShopEventConfig.ResourceGoods;
						bool flag8 = tipsData.ManageProduceValuationMin > tipsData.ManageProduceValuationMax;
						if (flag8)
						{
							ref int ptr = ref tipsData.ManageProduceValuationMin;
							int manageProduceValuationMax = tipsData.ManageProduceValuationMax;
							int manageProduceValuationMin = tipsData.ManageProduceValuationMin;
							ptr = manageProduceValuationMax;
							tipsData.ManageProduceValuationMax = manageProduceValuationMin;
						}
						tipsData.ManagerAttainment = 0;
						CharacterList managerList2;
						bool flag9 = this.TryGetElement_ShopManagerDict(blockKey, out managerList2);
						if (flag9)
						{
							for (int j = 0; j < managerList2.GetCount(); j++)
							{
								int charId2 = managerList2.GetCollection()[j];
								GameData.Domains.Character.Character character2;
								bool flag10 = GameData.Domains.Character.Character.IsCharacterIdValid(charId2) && DomainManager.Character.TryGetElement_Objects(charId2, out character2);
								if (flag10)
								{
									tipsData.ManagerAttainment += (int)character2.GetLifeSkillAttainment(lifeSkillType);
								}
							}
						}
					}
					else
					{
						bool flag11 = GameData.Domains.Building.SharedMethods.IsBuildingSoldItem(configData, successShopEventConfig);
						if (flag11)
						{
							int manageProduceValuationMin;
							tipsData.ManageProduceValuationMin = this.CalcSoldItemValueBySpecificBaseLine(new MinRandomGenerator(), blockKey, blockData, out tipsData.BuildingProduceDependencyData, 100, out manageProduceValuationMin);
							tipsData.ManageProduceValuationMax = this.CalcSoldItemValueBySpecificBaseLine(new MaxRandomGenerator(), blockKey, blockData, out tipsData.BuildingProduceDependencyData, 100, out manageProduceValuationMin);
						}
					}
				}
			}
			return tipsData;
		}

		// Token: 0x06007DE5 RID: 32229 RVA: 0x004AF69C File Offset: 0x004AD89C
		public void UpgradeTeaHorseCaravanByAwareness(DataContext context)
		{
			bool flag = this._teaHorseCaravanData == null;
			if (!flag)
			{
				Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
				BuildingAreaData taiwuBuildingAreaData = DomainManager.Building.GetBuildingAreaData(taiwuVillageLocation);
				BuildingBlockKey key = BuildingDomain.FindBuildingKey(taiwuVillageLocation, taiwuBuildingAreaData, 51, false);
				BuildingBlockDataEx blockDataEx;
				bool flag2 = !DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)key, out blockDataEx);
				if (!flag2)
				{
					sbyte currentLevel = blockDataEx.CalcUnlockedLevelCount();
					short[] table = GlobalConfig.Instance.TeaHorseCaravanLevelToAwareness;
					int targetLevel = -1;
					for (int i = table.Length - 1; i >= 0; i--)
					{
						bool flag3 = this._teaHorseCaravanData.CaravanAwareness < table[i];
						if (!flag3)
						{
							targetLevel = i + 1;
							break;
						}
					}
					bool flag4 = targetLevel == -1 || targetLevel <= (int)currentLevel;
					if (!flag4)
					{
						DomainManager.Extra.UpgradeToLevelUnchecked(context, key, blockDataEx, targetLevel);
					}
				}
			}
		}

		// Token: 0x06007DE6 RID: 32230 RVA: 0x004AF784 File Offset: 0x004AD984
		[DomainMethod]
		public TaiwuShrineDisplayData GetShrineDisplayData(DataContext context, short areaId, short blockId, short buildingBlockIndex)
		{
			BuildingBlockKey blockKey = new BuildingBlockKey(areaId, blockId, buildingBlockIndex);
			TaiwuShrineDisplayData displayData = new TaiwuShrineDisplayData
			{
				Authority = this.CalculateGainAuthorityByShrinePerMonth(blockKey),
				CharIdList = this.GetTaiwuShrineStudentList()
			};
			displayData.CharIdList.Insert(0, DomainManager.Taiwu.GetTaiwuCharId());
			return displayData;
		}

		// Token: 0x06007DE7 RID: 32231 RVA: 0x004AF7D8 File Offset: 0x004AD9D8
		[DomainMethod]
		public void TeachSkill(DataContext context, int characterId, SkillQualificationBonus bonus)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(characterId);
			bool flag = character == null;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not found Character with charId:");
				defaultInterpolatedStringHandler.AppendFormatted<int>(characterId);
				throw new KeyNotFoundException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			List<SkillQualificationBonus> bonusList = character.GetSkillQualificationBonuses();
			bool flag2 = bonusList.Count < 11;
			if (flag2)
			{
				bonusList.Add(bonus);
				sbyte skillGroup = bonus.GetSkillGroupAndType().Item1;
				bool flag3 = skillGroup == 1 && !character.GetLearnedCombatSkills().Contains(bonus.SkillId);
				if (flag3)
				{
					DomainManager.Character.LearnCombatSkill(context, characterId, bonus.SkillId, 0);
					int seniority = ProfessionFormulaImpl.Calculate(52, (int)Config.CombatSkill.Instance[bonus.SkillId].Grade);
					DomainManager.Extra.ChangeProfessionSeniority(context, 7, seniority, true, false);
				}
				else
				{
					bool flag4 = skillGroup == 0 && character.FindLearnedLifeSkillIndex(bonus.SkillId) < 0;
					if (flag4)
					{
						DomainManager.Character.LearnLifeSkill(context, characterId, bonus.SkillId, 0);
						int seniority2 = ProfessionFormulaImpl.Calculate(103, (int)LifeSkill.Instance[bonus.SkillId].Grade);
						DomainManager.Extra.ChangeProfessionSeniority(context, 16, seniority2, true, false);
					}
				}
				this.ConsumeResource(context, 7, (int)(this._shrineBuyTimes * GlobalConfig.Instance.ShrineAuthorityPerTime));
				this.SetShrineBuyTimes(this._shrineBuyTimes + 1, context);
				character.SetSkillQualificationBonuses(bonusList, context);
			}
		}

		// Token: 0x06007DE8 RID: 32232 RVA: 0x004AF958 File Offset: 0x004ADB58
		private List<int> GetTaiwuShrineStudentList()
		{
			List<int> list = new List<int>();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			OrgMemberCollection members = settlement.GetMembers();
			List<int> memberCharIdList = new List<int>();
			members.GetAllMembers(memberCharIdList);
			foreach (int charId in memberCharIdList)
			{
				bool flag = charId == taiwuCharId;
				if (!flag)
				{
					GameData.Domains.Character.Character character;
					bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (flag2)
					{
						bool flag3 = character.GetAgeGroup() == 0;
						if (!flag3)
						{
							bool flag4 = character.IsCompletelyInfected();
							if (!flag4)
							{
								list.Add(charId);
							}
						}
					}
				}
			}
			HashSet<int> groupHashSet = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			list.Sort(delegate(int l, int r)
			{
				bool flag5 = groupHashSet.Contains(l) && !groupHashSet.Contains(r);
				int result;
				if (flag5)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
				return result;
			});
			return list;
		}

		// Token: 0x06007DE9 RID: 32233 RVA: 0x004AFA70 File Offset: 0x004ADC70
		[Obsolete]
		private int GetAutoAddAuthorityValue(BuildingBlockKey key)
		{
			BuildingBlockData buildingBlock = this.GetElement_BuildingBlocks(key);
			return this.GetAutoAddAuthorityValue(buildingBlock);
		}

		// Token: 0x06007DEA RID: 32234 RVA: 0x004AFA94 File Offset: 0x004ADC94
		[Obsolete]
		private int GetAutoAddAuthorityValue(BuildingBlockData buildingBlock)
		{
			float[] taiwuFameRateArray = new float[]
			{
				0f,
				0.25f,
				0.5f,
				0.75f,
				1f
			};
			byte[] worldXiangshuLevelArray = new byte[]
			{
				0,
				1,
				2,
				4,
				8,
				16,
				32,
				64,
				128
			};
			sbyte[] taiwuRelationFameLevelArray = new sbyte[]
			{
				-15,
				-10,
				-5,
				0,
				5,
				10,
				15
			};
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			sbyte taiwuFameType = FameType.GetFameType(taiwu.GetFameType());
			sbyte worldXiangshuLevel = DomainManager.World.GetXiangshuLevel();
			bool flag = !worldXiangshuLevelArray.CheckIndex((int)worldXiangshuLevel);
			if (flag)
			{
				worldXiangshuLevel = (sbyte)(worldXiangshuLevelArray.Length - 1);
			}
			float baseValue = (float)worldXiangshuLevelArray[(int)worldXiangshuLevel] * taiwuFameRateArray[(int)taiwuFameType];
			int additionValue = 100;
			HashSet<int> friendCharIds = new HashSet<int>();
			DomainManager.Character.GetAllRelatedCharIds(taiwu.GetId(), friendCharIds, true);
			foreach (int charId in friendCharIds)
			{
				GameData.Domains.Character.Character character;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag2)
				{
					sbyte fameType = FameType.GetFameType(character.GetFameType());
					RelatedCharacter relation = DomainManager.Character.GetRelation(charId, taiwu.GetId());
					ushort relationType = relation.RelationType;
					short favorability = relation.Favorability;
					bool flag3 = favorability > 0 && RelationType.ContainPositiveRelations(relationType);
					if (flag3)
					{
						additionValue += (int)taiwuRelationFameLevelArray[(int)fameType];
					}
					else
					{
						bool flag4 = favorability < 0 && RelationType.ContainNegativeRelations(relationType);
						if (flag4)
						{
							additionValue -= (int)taiwuRelationFameLevelArray[(int)fameType];
						}
					}
				}
			}
			return Math.Max(0, (int)(baseValue * (float)additionValue / 100f));
		}

		// Token: 0x06007DEB RID: 32235 RVA: 0x004AFC30 File Offset: 0x004ADE30
		[Obsolete("Instead by CalculateGainAuthorityByShrinePerMonth")]
		private void AddAuthorityOnSerialUpdate(DataContext context)
		{
			sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
			bool flag = currMonthInYear != GlobalConfig.Instance.ShrineAuthorityAddMonth;
			if (!flag)
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				Location taiwuVilLocation = this._taiwuBuildingAreas[0];
				BuildingAreaData taiwuVilAreaData = this.GetBuildingAreaData(taiwuVilLocation);
				short templateId = 45;
				BuildingBlockData buildingBlock = BuildingDomain.FindBuilding(taiwuVilLocation, taiwuVilAreaData, templateId, true);
				bool flag2 = buildingBlock != null;
				if (flag2)
				{
					int addValue = this.GetAutoAddAuthorityValue(buildingBlock);
					taiwu.ChangeResource(context, 7, addValue);
				}
			}
		}

		// Token: 0x06007DEC RID: 32236 RVA: 0x004AFCB4 File Offset: 0x004ADEB4
		[Obsolete]
		public void SpiritualDebtInteractionEmei(DataContext context)
		{
			List<Location> taiwuBuildingList = this.GetTaiwuBuildingAreas();
			for (int i = 0; i < taiwuBuildingList.Count; i++)
			{
				Location location = taiwuBuildingList[i];
				BuildingAreaData areaData = this.GetElement_BuildingAreas(location);
				short index = 0;
				short len = (short)(areaData.Width * areaData.Width);
				while (index < len)
				{
					BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
					BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
					bool flag = blockData.RootBlockIndex >= 0;
					if (!flag)
					{
						BuildingBlockItem configData = BuildingBlock.Instance[blockData.TemplateId];
						bool flag2 = !BuildingBlockData.IsResource(configData.Type);
						if (!flag2)
						{
							sbyte chance = this.GetResourceBlockGrowthChance(blockKey);
							BuildingBlockDataEx blockDataEx;
							bool flag3 = chance > 0 && DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)blockKey, out blockDataEx);
							if (flag3)
							{
								sbyte level = blockDataEx.CalcUnlockedLevelCount();
								sbyte oldLevel = level;
								sbyte newLevel = (sbyte)Math.Min((int)level + context.Random.Next(1, 4), (int)configData.MaxLevel);
								bool flag4 = oldLevel != newLevel;
								if (flag4)
								{
									this.SetElement_BuildingBlocks(blockKey, blockData, context);
								}
							}
						}
					}
					index += 1;
				}
			}
		}

		// Token: 0x06007DED RID: 32237 RVA: 0x004AFDFF File Offset: 0x004ADFFF
		public int GetVillagerRoleCapacity(short roleTemplateId)
		{
			return VillagerRole.Instance[roleTemplateId].MaxCount;
		}

		// Token: 0x06007DEE RID: 32238 RVA: 0x004AFE11 File Offset: 0x004AE011
		[Obsolete]
		private void UpdateVillagerRoleCapacities()
		{
		}

		// Token: 0x06007DEF RID: 32239 RVA: 0x004AFE14 File Offset: 0x004AE014
		public void GetVillagerRoleCapacitiesDetail(short roleTemplateId, ref List<IntPair> detailList)
		{
			List<Location> taiwuBuildingList = this.GetTaiwuBuildingAreas();
			for (int i = 0; i < taiwuBuildingList.Count; i++)
			{
				Location location = taiwuBuildingList[i];
				BuildingAreaData areaData = this.GetElement_BuildingAreas(location);
				short index = 0;
				short len = (short)(areaData.Width * areaData.Width);
				while (index < len)
				{
					BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
					BuildingBlockData blockData = this.GetElement_BuildingBlocks(blockKey);
					BuildingBlockDataEx blockDataEx;
					bool flag = !DomainManager.Extra.TryGetElement_BuildingBlockDataEx((ulong)blockKey, out blockDataEx);
					if (!flag)
					{
						sbyte level = blockDataEx.CalcUnlockedLevelCount();
						bool flag2 = blockData.RootBlockIndex >= 0;
						if (!flag2)
						{
							bool flag3 = level <= 0;
							if (!flag3)
							{
								BuildingBlockItem buildingCfg = BuildingBlock.Instance[blockData.TemplateId];
								bool flag4 = buildingCfg.ExpandInfos == null;
								if (!flag4)
								{
									bool flag5 = buildingCfg.VillagerRoleTemplateIds == null || !buildingCfg.VillagerRoleTemplateIds.Exist(roleTemplateId);
									if (!flag5)
									{
										foreach (short buildingScaleId in buildingCfg.ExpandInfos)
										{
											BuildingScaleItem buildingScaleCfg = BuildingScale.Instance[buildingScaleId];
											detailList.Add(new IntPair((int)blockData.TemplateId, buildingScaleCfg.LevelEffect.GetOrLast((int)(level - 1))));
										}
									}
								}
							}
						}
					}
					index += 1;
				}
			}
		}

		// Token: 0x06007DF0 RID: 32240 RVA: 0x004AFFC0 File Offset: 0x004AE1C0
		public BuildingDomain() : base(25)
		{
			this._buildingAreas = new Dictionary<Location, BuildingAreaData>(0);
			this._buildingBlocks = new Dictionary<BuildingBlockKey, BuildingBlockData>(0);
			this._taiwuBuildingAreas = new List<Location>();
			this._CollectBuildingResourceType = new Dictionary<BuildingBlockKey, sbyte>(0);
			this._buildingOperatorDict = new Dictionary<BuildingBlockKey, CharacterList>(0);
			this._customBuildingName = new Dictionary<BuildingBlockKey, int>(0);
			this._newCompleteOperationBuildings = new List<BuildingBlockKey>();
			this._chickenBlessingInfoData = new Dictionary<int, ChickenBlessingInfoData>(0);
			this._chicken = new Dictionary<int, Chicken>(0);
			this._collectionCrickets = new ItemKey[15];
			this._collectionCricketJars = new ItemKey[15];
			this._collectionCricketRegen = new int[15];
			this._makeItemDict = new Dictionary<BuildingBlockKey, MakeItemData>(0);
			this._residences = new Dictionary<BuildingBlockKey, CharacterList>(0);
			this._comfortableHouses = new Dictionary<BuildingBlockKey, CharacterList>(0);
			this._homeless = default(CharacterList);
			this._samsaraPlatformAddMainAttributes = default(MainAttributes);
			this._samsaraPlatformAddCombatSkillQualifications = default(CombatSkillShorts);
			this._samsaraPlatformAddLifeSkillQualifications = default(LifeSkillShorts);
			this._samsaraPlatformSlots = new IntPair[6];
			this._samsaraPlatformBornDict = new Dictionary<int, IntPair>(0);
			this._collectBuildingEarningsData = new Dictionary<BuildingBlockKey, BuildingEarningsData>(0);
			this._shopManagerDict = new Dictionary<BuildingBlockKey, CharacterList>(0);
			this._teaHorseCaravanData = new TeaHorseCaravanData();
			this._shrineBuyTimes = 0;
			this.OnInitializedDomainData();
		}

		// Token: 0x06007DF1 RID: 32241 RVA: 0x004B0324 File Offset: 0x004AE524
		public BuildingAreaData GetElement_BuildingAreas(Location elementId)
		{
			return this._buildingAreas[elementId];
		}

		// Token: 0x06007DF2 RID: 32242 RVA: 0x004B0344 File Offset: 0x004AE544
		public bool TryGetElement_BuildingAreas(Location elementId, out BuildingAreaData value)
		{
			return this._buildingAreas.TryGetValue(elementId, out value);
		}

		// Token: 0x06007DF3 RID: 32243 RVA: 0x004B0364 File Offset: 0x004AE564
		private unsafe void AddElement_BuildingAreas(Location elementId, BuildingAreaData value, DataContext context)
		{
			this._buildingAreas.Add(elementId, value);
			this._modificationsBuildingAreas.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Add<Location>(9, 0, elementId, 2);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007DF4 RID: 32244 RVA: 0x004B03B8 File Offset: 0x004AE5B8
		private unsafe void SetElement_BuildingAreas(Location elementId, BuildingAreaData value, DataContext context)
		{
			this._buildingAreas[elementId] = value;
			this._modificationsBuildingAreas.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Set<Location>(9, 0, elementId, 2);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007DF5 RID: 32245 RVA: 0x004B0409 File Offset: 0x004AE609
		private void RemoveElement_BuildingAreas(Location elementId, DataContext context)
		{
			this._buildingAreas.Remove(elementId);
			this._modificationsBuildingAreas.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Remove<Location>(9, 0, elementId);
		}

		// Token: 0x06007DF6 RID: 32246 RVA: 0x004B0443 File Offset: 0x004AE643
		private void ClearBuildingAreas(DataContext context)
		{
			this._buildingAreas.Clear();
			this._modificationsBuildingAreas.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Clear(9, 0);
		}

		// Token: 0x06007DF7 RID: 32247 RVA: 0x004B047C File Offset: 0x004AE67C
		public BuildingBlockData GetElement_BuildingBlocks(BuildingBlockKey elementId)
		{
			return this._buildingBlocks[elementId];
		}

		// Token: 0x06007DF8 RID: 32248 RVA: 0x004B049C File Offset: 0x004AE69C
		public bool TryGetElement_BuildingBlocks(BuildingBlockKey elementId, out BuildingBlockData value)
		{
			return this._buildingBlocks.TryGetValue(elementId, out value);
		}

		// Token: 0x06007DF9 RID: 32249 RVA: 0x004B04BC File Offset: 0x004AE6BC
		private unsafe void AddElement_BuildingBlocks(BuildingBlockKey elementId, BuildingBlockData value, DataContext context)
		{
			this._buildingBlocks.Add(elementId, value);
			this._modificationsBuildingBlocks.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Add<BuildingBlockKey>(9, 1, elementId, 16);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007DFA RID: 32250 RVA: 0x004B0510 File Offset: 0x004AE710
		private unsafe void SetElement_BuildingBlocks(BuildingBlockKey elementId, BuildingBlockData value, DataContext context)
		{
			this._buildingBlocks[elementId] = value;
			this._modificationsBuildingBlocks.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Set<BuildingBlockKey>(9, 1, elementId, 16);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007DFB RID: 32251 RVA: 0x004B0562 File Offset: 0x004AE762
		private void RemoveElement_BuildingBlocks(BuildingBlockKey elementId, DataContext context)
		{
			this._buildingBlocks.Remove(elementId);
			this._modificationsBuildingBlocks.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Remove<BuildingBlockKey>(9, 1, elementId);
		}

		// Token: 0x06007DFC RID: 32252 RVA: 0x004B059C File Offset: 0x004AE79C
		private void ClearBuildingBlocks(DataContext context)
		{
			this._buildingBlocks.Clear();
			this._modificationsBuildingBlocks.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Clear(9, 1);
		}

		// Token: 0x06007DFD RID: 32253 RVA: 0x004B05D4 File Offset: 0x004AE7D4
		public List<Location> GetTaiwuBuildingAreas()
		{
			return this._taiwuBuildingAreas;
		}

		// Token: 0x06007DFE RID: 32254 RVA: 0x004B05EC File Offset: 0x004AE7EC
		private unsafe void SetTaiwuBuildingAreas(List<Location> value, DataContext context)
		{
			this._taiwuBuildingAreas = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, this.DataStates, BuildingDomain.CacheInfluences, context);
			int elementsCount = this._taiwuBuildingAreas.Count;
			int contentSize = 4 * elementsCount;
			int dataSize = 2 + contentSize;
			byte* pData = OperationAdder.DynamicSingleValue_Set(9, 2, dataSize);
			*(short*)pData = (short)((ushort)elementsCount);
			pData += 2;
			for (int i = 0; i < elementsCount; i++)
			{
				pData += this._taiwuBuildingAreas[i].Serialize(pData);
			}
		}

		// Token: 0x06007DFF RID: 32255 RVA: 0x004B066C File Offset: 0x004AE86C
		public sbyte GetElement_CollectBuildingResourceType(BuildingBlockKey elementId)
		{
			return this._CollectBuildingResourceType[elementId];
		}

		// Token: 0x06007E00 RID: 32256 RVA: 0x004B068C File Offset: 0x004AE88C
		public bool TryGetElement_CollectBuildingResourceType(BuildingBlockKey elementId, out sbyte value)
		{
			return this._CollectBuildingResourceType.TryGetValue(elementId, out value);
		}

		// Token: 0x06007E01 RID: 32257 RVA: 0x004B06AC File Offset: 0x004AE8AC
		private unsafe void AddElement_CollectBuildingResourceType(BuildingBlockKey elementId, sbyte value, DataContext context)
		{
			this._CollectBuildingResourceType.Add(elementId, value);
			this._modificationsCollectBuildingResourceType.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Add<BuildingBlockKey>(9, 3, elementId, 1);
			*pData = (byte)value;
			pData++;
		}

		// Token: 0x06007E02 RID: 32258 RVA: 0x004B06FC File Offset: 0x004AE8FC
		private unsafe void SetElement_CollectBuildingResourceType(BuildingBlockKey elementId, sbyte value, DataContext context)
		{
			this._CollectBuildingResourceType[elementId] = value;
			this._modificationsCollectBuildingResourceType.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Set<BuildingBlockKey>(9, 3, elementId, 1);
			*pData = (byte)value;
			pData++;
		}

		// Token: 0x06007E03 RID: 32259 RVA: 0x004B074A File Offset: 0x004AE94A
		private void RemoveElement_CollectBuildingResourceType(BuildingBlockKey elementId, DataContext context)
		{
			this._CollectBuildingResourceType.Remove(elementId);
			this._modificationsCollectBuildingResourceType.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Remove<BuildingBlockKey>(9, 3, elementId);
		}

		// Token: 0x06007E04 RID: 32260 RVA: 0x004B0784 File Offset: 0x004AE984
		private void ClearCollectBuildingResourceType(DataContext context)
		{
			this._CollectBuildingResourceType.Clear();
			this._modificationsCollectBuildingResourceType.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Clear(9, 3);
		}

		// Token: 0x06007E05 RID: 32261 RVA: 0x004B07BC File Offset: 0x004AE9BC
		public CharacterList GetElement_BuildingOperatorDict(BuildingBlockKey elementId)
		{
			return this._buildingOperatorDict[elementId];
		}

		// Token: 0x06007E06 RID: 32262 RVA: 0x004B07DC File Offset: 0x004AE9DC
		public bool TryGetElement_BuildingOperatorDict(BuildingBlockKey elementId, out CharacterList value)
		{
			return this._buildingOperatorDict.TryGetValue(elementId, out value);
		}

		// Token: 0x06007E07 RID: 32263 RVA: 0x004B07FB File Offset: 0x004AE9FB
		private void AddElement_BuildingOperatorDict(BuildingBlockKey elementId, CharacterList value, DataContext context)
		{
			this._buildingOperatorDict.Add(elementId, value);
			this._modificationsBuildingOperatorDict.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, BuildingDomain.CacheInfluences, context);
		}

		// Token: 0x06007E08 RID: 32264 RVA: 0x004B082C File Offset: 0x004AEA2C
		private void SetElement_BuildingOperatorDict(BuildingBlockKey elementId, CharacterList value, DataContext context)
		{
			this._buildingOperatorDict[elementId] = value;
			this._modificationsBuildingOperatorDict.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, BuildingDomain.CacheInfluences, context);
		}

		// Token: 0x06007E09 RID: 32265 RVA: 0x004B085D File Offset: 0x004AEA5D
		private void RemoveElement_BuildingOperatorDict(BuildingBlockKey elementId, DataContext context)
		{
			this._buildingOperatorDict.Remove(elementId);
			this._modificationsBuildingOperatorDict.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, BuildingDomain.CacheInfluences, context);
		}

		// Token: 0x06007E0A RID: 32266 RVA: 0x004B088D File Offset: 0x004AEA8D
		private void ClearBuildingOperatorDict(DataContext context)
		{
			this._buildingOperatorDict.Clear();
			this._modificationsBuildingOperatorDict.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, BuildingDomain.CacheInfluences, context);
		}

		// Token: 0x06007E0B RID: 32267 RVA: 0x004B08BC File Offset: 0x004AEABC
		public int GetElement_CustomBuildingName(BuildingBlockKey elementId)
		{
			return this._customBuildingName[elementId];
		}

		// Token: 0x06007E0C RID: 32268 RVA: 0x004B08DC File Offset: 0x004AEADC
		public bool TryGetElement_CustomBuildingName(BuildingBlockKey elementId, out int value)
		{
			return this._customBuildingName.TryGetValue(elementId, out value);
		}

		// Token: 0x06007E0D RID: 32269 RVA: 0x004B08FC File Offset: 0x004AEAFC
		private unsafe void AddElement_CustomBuildingName(BuildingBlockKey elementId, int value, DataContext context)
		{
			this._customBuildingName.Add(elementId, value);
			this._modificationsCustomBuildingName.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Add<BuildingBlockKey>(9, 5, elementId, 4);
			*(int*)pData = value;
			pData += 4;
		}

		// Token: 0x06007E0E RID: 32270 RVA: 0x004B094C File Offset: 0x004AEB4C
		private unsafe void SetElement_CustomBuildingName(BuildingBlockKey elementId, int value, DataContext context)
		{
			this._customBuildingName[elementId] = value;
			this._modificationsCustomBuildingName.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Set<BuildingBlockKey>(9, 5, elementId, 4);
			*(int*)pData = value;
			pData += 4;
		}

		// Token: 0x06007E0F RID: 32271 RVA: 0x004B099A File Offset: 0x004AEB9A
		private void RemoveElement_CustomBuildingName(BuildingBlockKey elementId, DataContext context)
		{
			this._customBuildingName.Remove(elementId);
			this._modificationsCustomBuildingName.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Remove<BuildingBlockKey>(9, 5, elementId);
		}

		// Token: 0x06007E10 RID: 32272 RVA: 0x004B09D4 File Offset: 0x004AEBD4
		private void ClearCustomBuildingName(DataContext context)
		{
			this._customBuildingName.Clear();
			this._modificationsCustomBuildingName.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Clear(9, 5);
		}

		// Token: 0x06007E11 RID: 32273 RVA: 0x004B0A0C File Offset: 0x004AEC0C
		private List<BuildingBlockKey> GetNewCompleteOperationBuildings()
		{
			return this._newCompleteOperationBuildings;
		}

		// Token: 0x06007E12 RID: 32274 RVA: 0x004B0A24 File Offset: 0x004AEC24
		private unsafe void SetNewCompleteOperationBuildings(List<BuildingBlockKey> value, DataContext context)
		{
			this._newCompleteOperationBuildings = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, BuildingDomain.CacheInfluences, context);
			int elementsCount = this._newCompleteOperationBuildings.Count;
			int contentSize = 8 * elementsCount;
			int dataSize = 2 + contentSize;
			byte* pData = OperationAdder.DynamicSingleValue_Set(9, 6, dataSize);
			*(short*)pData = (short)((ushort)elementsCount);
			pData += 2;
			for (int i = 0; i < elementsCount; i++)
			{
				pData += this._newCompleteOperationBuildings[i].Serialize(pData);
			}
		}

		// Token: 0x06007E13 RID: 32275 RVA: 0x004B0AA4 File Offset: 0x004AECA4
		[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
		public ChickenBlessingInfoData GetElement_ChickenBlessingInfoData(int elementId)
		{
			return this._chickenBlessingInfoData[elementId];
		}

		// Token: 0x06007E14 RID: 32276 RVA: 0x004B0AC4 File Offset: 0x004AECC4
		[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
		public bool TryGetElement_ChickenBlessingInfoData(int elementId, out ChickenBlessingInfoData value)
		{
			return this._chickenBlessingInfoData.TryGetValue(elementId, out value);
		}

		// Token: 0x06007E15 RID: 32277 RVA: 0x004B0AE4 File Offset: 0x004AECE4
		[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
		private unsafe void AddElement_ChickenBlessingInfoData(int elementId, ChickenBlessingInfoData value, DataContext context)
		{
			this._chickenBlessingInfoData.Add(elementId, value);
			this._modificationsChickenBlessingInfoData.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, BuildingDomain.CacheInfluences, context);
			int dataSize = value.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValueCollection_Add<int>(9, 7, elementId, dataSize);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007E16 RID: 32278 RVA: 0x004B0B40 File Offset: 0x004AED40
		[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
		private unsafe void SetElement_ChickenBlessingInfoData(int elementId, ChickenBlessingInfoData value, DataContext context)
		{
			this._chickenBlessingInfoData[elementId] = value;
			this._modificationsChickenBlessingInfoData.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, BuildingDomain.CacheInfluences, context);
			int dataSize = value.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValueCollection_Set<int>(9, 7, elementId, dataSize);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007E17 RID: 32279 RVA: 0x004B0B9A File Offset: 0x004AED9A
		[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
		private void RemoveElement_ChickenBlessingInfoData(int elementId, DataContext context)
		{
			this._chickenBlessingInfoData.Remove(elementId);
			this._modificationsChickenBlessingInfoData.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<int>(9, 7, elementId);
		}

		// Token: 0x06007E18 RID: 32280 RVA: 0x004B0BD4 File Offset: 0x004AEDD4
		[Obsolete("DomainData _chickenBlessingInfoData is no longer in use.")]
		private void ClearChickenBlessingInfoData(DataContext context)
		{
			this._chickenBlessingInfoData.Clear();
			this._modificationsChickenBlessingInfoData.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(9, 7);
		}

		// Token: 0x06007E19 RID: 32281 RVA: 0x004B0C0C File Offset: 0x004AEE0C
		public Chicken GetElement_Chicken(int elementId)
		{
			return this._chicken[elementId];
		}

		// Token: 0x06007E1A RID: 32282 RVA: 0x004B0C2C File Offset: 0x004AEE2C
		public bool TryGetElement_Chicken(int elementId, out Chicken value)
		{
			return this._chicken.TryGetValue(elementId, out value);
		}

		// Token: 0x06007E1B RID: 32283 RVA: 0x004B0C4C File Offset: 0x004AEE4C
		private unsafe void AddElement_Chicken(int elementId, Chicken value, DataContext context)
		{
			this._chicken.Add(elementId, value);
			this._modificationsChicken.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Add<int>(9, 8, elementId, 12);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007E1C RID: 32284 RVA: 0x004B0CA0 File Offset: 0x004AEEA0
		private unsafe void SetElement_Chicken(int elementId, Chicken value, DataContext context)
		{
			this._chicken[elementId] = value;
			this._modificationsChicken.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Set<int>(9, 8, elementId, 12);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007E1D RID: 32285 RVA: 0x004B0CF3 File Offset: 0x004AEEF3
		private void RemoveElement_Chicken(int elementId, DataContext context)
		{
			this._chicken.Remove(elementId);
			this._modificationsChicken.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Remove<int>(9, 8, elementId);
		}

		// Token: 0x06007E1E RID: 32286 RVA: 0x004B0D2D File Offset: 0x004AEF2D
		private void ClearChicken(DataContext context)
		{
			this._chicken.Clear();
			this._modificationsChicken.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Clear(9, 8);
		}

		// Token: 0x06007E1F RID: 32287 RVA: 0x004B0D64 File Offset: 0x004AEF64
		[Obsolete("DomainData _collectionCrickets is no longer in use.")]
		public ItemKey[] GetCollectionCrickets()
		{
			return this._collectionCrickets;
		}

		// Token: 0x06007E20 RID: 32288 RVA: 0x004B0D7C File Offset: 0x004AEF7C
		[Obsolete("DomainData _collectionCrickets is no longer in use.")]
		public unsafe void SetCollectionCrickets(ItemKey[] value, DataContext context)
		{
			this._collectionCrickets = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(9, 9, 120);
			for (int i = 0; i < 15; i++)
			{
				pData += this._collectionCrickets[i].Serialize(pData);
			}
		}

		// Token: 0x06007E21 RID: 32289 RVA: 0x004B0DD8 File Offset: 0x004AEFD8
		[Obsolete("DomainData _collectionCricketJars is no longer in use.")]
		public ItemKey[] GetCollectionCricketJars()
		{
			return this._collectionCricketJars;
		}

		// Token: 0x06007E22 RID: 32290 RVA: 0x004B0DF0 File Offset: 0x004AEFF0
		[Obsolete("DomainData _collectionCricketJars is no longer in use.")]
		public unsafe void SetCollectionCricketJars(ItemKey[] value, DataContext context)
		{
			this._collectionCricketJars = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(9, 10, 120);
			for (int i = 0; i < 15; i++)
			{
				pData += this._collectionCricketJars[i].Serialize(pData);
			}
		}

		// Token: 0x06007E23 RID: 32291 RVA: 0x004B0E4C File Offset: 0x004AF04C
		[Obsolete("DomainData _collectionCricketRegen is no longer in use.")]
		public int[] GetCollectionCricketRegen()
		{
			return this._collectionCricketRegen;
		}

		// Token: 0x06007E24 RID: 32292 RVA: 0x004B0E64 File Offset: 0x004AF064
		[Obsolete("DomainData _collectionCricketRegen is no longer in use.")]
		public unsafe void SetCollectionCricketRegen(int[] value, DataContext context)
		{
			this._collectionCricketRegen = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(9, 11, 60);
			for (int i = 0; i < 15; i++)
			{
				*(int*)(pData + (IntPtr)i * 4) = this._collectionCricketRegen[i];
			}
			pData += 60;
		}

		// Token: 0x06007E25 RID: 32293 RVA: 0x004B0EC0 File Offset: 0x004AF0C0
		private MakeItemData GetElement_MakeItemDict(BuildingBlockKey elementId)
		{
			return this._makeItemDict[elementId];
		}

		// Token: 0x06007E26 RID: 32294 RVA: 0x004B0EE0 File Offset: 0x004AF0E0
		private bool TryGetElement_MakeItemDict(BuildingBlockKey elementId, out MakeItemData value)
		{
			return this._makeItemDict.TryGetValue(elementId, out value);
		}

		// Token: 0x06007E27 RID: 32295 RVA: 0x004B0F00 File Offset: 0x004AF100
		private unsafe void AddElement_MakeItemDict(BuildingBlockKey elementId, MakeItemData value, DataContext context)
		{
			this._makeItemDict.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, BuildingDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 12, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 12, elementId, 0);
			}
		}

		// Token: 0x06007E28 RID: 32296 RVA: 0x004B0F68 File Offset: 0x004AF168
		private unsafe void SetElement_MakeItemDict(BuildingBlockKey elementId, MakeItemData value, DataContext context)
		{
			this._makeItemDict[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, BuildingDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<BuildingBlockKey>(9, 12, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<BuildingBlockKey>(9, 12, elementId, 0);
			}
		}

		// Token: 0x06007E29 RID: 32297 RVA: 0x004B0FCD File Offset: 0x004AF1CD
		private void RemoveElement_MakeItemDict(BuildingBlockKey elementId, DataContext context)
		{
			this._makeItemDict.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<BuildingBlockKey>(9, 12, elementId);
		}

		// Token: 0x06007E2A RID: 32298 RVA: 0x004B0FFC File Offset: 0x004AF1FC
		private void ClearMakeItemDict(DataContext context)
		{
			this._makeItemDict.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(9, 12);
		}

		// Token: 0x06007E2B RID: 32299 RVA: 0x004B102C File Offset: 0x004AF22C
		private CharacterList GetElement_Residences(BuildingBlockKey elementId)
		{
			return this._residences[elementId];
		}

		// Token: 0x06007E2C RID: 32300 RVA: 0x004B104C File Offset: 0x004AF24C
		private bool TryGetElement_Residences(BuildingBlockKey elementId, out CharacterList value)
		{
			return this._residences.TryGetValue(elementId, out value);
		}

		// Token: 0x06007E2D RID: 32301 RVA: 0x004B106C File Offset: 0x004AF26C
		private unsafe void AddElement_Residences(BuildingBlockKey elementId, CharacterList value, DataContext context)
		{
			this._residences.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, BuildingDomain.CacheInfluences, context);
			int dataSize = value.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 13, elementId, dataSize);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007E2E RID: 32302 RVA: 0x004B10BC File Offset: 0x004AF2BC
		private unsafe void SetElement_Residences(BuildingBlockKey elementId, CharacterList value, DataContext context)
		{
			this._residences[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, BuildingDomain.CacheInfluences, context);
			int dataSize = value.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValueCollection_Set<BuildingBlockKey>(9, 13, elementId, dataSize);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007E2F RID: 32303 RVA: 0x004B110B File Offset: 0x004AF30B
		private void RemoveElement_Residences(BuildingBlockKey elementId, DataContext context)
		{
			this._residences.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<BuildingBlockKey>(9, 13, elementId);
		}

		// Token: 0x06007E30 RID: 32304 RVA: 0x004B113A File Offset: 0x004AF33A
		private void ClearResidences(DataContext context)
		{
			this._residences.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(9, 13);
		}

		// Token: 0x06007E31 RID: 32305 RVA: 0x004B1168 File Offset: 0x004AF368
		private CharacterList GetElement_ComfortableHouses(BuildingBlockKey elementId)
		{
			return this._comfortableHouses[elementId];
		}

		// Token: 0x06007E32 RID: 32306 RVA: 0x004B1188 File Offset: 0x004AF388
		private bool TryGetElement_ComfortableHouses(BuildingBlockKey elementId, out CharacterList value)
		{
			return this._comfortableHouses.TryGetValue(elementId, out value);
		}

		// Token: 0x06007E33 RID: 32307 RVA: 0x004B11A8 File Offset: 0x004AF3A8
		private unsafe void AddElement_ComfortableHouses(BuildingBlockKey elementId, CharacterList value, DataContext context)
		{
			this._comfortableHouses.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, BuildingDomain.CacheInfluences, context);
			int dataSize = value.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 14, elementId, dataSize);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007E34 RID: 32308 RVA: 0x004B11F8 File Offset: 0x004AF3F8
		private unsafe void SetElement_ComfortableHouses(BuildingBlockKey elementId, CharacterList value, DataContext context)
		{
			this._comfortableHouses[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, BuildingDomain.CacheInfluences, context);
			int dataSize = value.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValueCollection_Set<BuildingBlockKey>(9, 14, elementId, dataSize);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007E35 RID: 32309 RVA: 0x004B1247 File Offset: 0x004AF447
		private void RemoveElement_ComfortableHouses(BuildingBlockKey elementId, DataContext context)
		{
			this._comfortableHouses.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<BuildingBlockKey>(9, 14, elementId);
		}

		// Token: 0x06007E36 RID: 32310 RVA: 0x004B1276 File Offset: 0x004AF476
		private void ClearComfortableHouses(DataContext context)
		{
			this._comfortableHouses.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(9, 14);
		}

		// Token: 0x06007E37 RID: 32311 RVA: 0x004B12A4 File Offset: 0x004AF4A4
		public CharacterList GetHomeless()
		{
			return this._homeless;
		}

		// Token: 0x06007E38 RID: 32312 RVA: 0x004B12BC File Offset: 0x004AF4BC
		public unsafe void SetHomeless(CharacterList value, DataContext context)
		{
			this._homeless = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, BuildingDomain.CacheInfluences, context);
			int dataSize = this._homeless.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValue_Set(9, 15, dataSize);
			pData += this._homeless.Serialize(pData);
		}

		// Token: 0x06007E39 RID: 32313 RVA: 0x004B130C File Offset: 0x004AF50C
		public MainAttributes GetSamsaraPlatformAddMainAttributes()
		{
			return this._samsaraPlatformAddMainAttributes;
		}

		// Token: 0x06007E3A RID: 32314 RVA: 0x004B1324 File Offset: 0x004AF524
		private unsafe void SetSamsaraPlatformAddMainAttributes(MainAttributes value, DataContext context)
		{
			this._samsaraPlatformAddMainAttributes = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(9, 16, 12);
			pData += this._samsaraPlatformAddMainAttributes.Serialize(pData);
		}

		// Token: 0x06007E3B RID: 32315 RVA: 0x004B1368 File Offset: 0x004AF568
		public ref CombatSkillShorts GetSamsaraPlatformAddCombatSkillQualifications()
		{
			return ref this._samsaraPlatformAddCombatSkillQualifications;
		}

		// Token: 0x06007E3C RID: 32316 RVA: 0x004B1380 File Offset: 0x004AF580
		private unsafe void SetSamsaraPlatformAddCombatSkillQualifications(ref CombatSkillShorts value, DataContext context)
		{
			this._samsaraPlatformAddCombatSkillQualifications = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(9, 17, 28);
			pData += this._samsaraPlatformAddCombatSkillQualifications.Serialize(pData);
		}

		// Token: 0x06007E3D RID: 32317 RVA: 0x004B13CC File Offset: 0x004AF5CC
		public ref LifeSkillShorts GetSamsaraPlatformAddLifeSkillQualifications()
		{
			return ref this._samsaraPlatformAddLifeSkillQualifications;
		}

		// Token: 0x06007E3E RID: 32318 RVA: 0x004B13E4 File Offset: 0x004AF5E4
		private unsafe void SetSamsaraPlatformAddLifeSkillQualifications(ref LifeSkillShorts value, DataContext context)
		{
			this._samsaraPlatformAddLifeSkillQualifications = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(9, 18, 32);
			pData += this._samsaraPlatformAddLifeSkillQualifications.Serialize(pData);
		}

		// Token: 0x06007E3F RID: 32319 RVA: 0x004B1430 File Offset: 0x004AF630
		public IntPair GetElement_SamsaraPlatformSlots(int index)
		{
			return this._samsaraPlatformSlots[index];
		}

		// Token: 0x06007E40 RID: 32320 RVA: 0x004B1450 File Offset: 0x004AF650
		public unsafe void SetElement_SamsaraPlatformSlots(int index, IntPair value, DataContext context)
		{
			this._samsaraPlatformSlots[index] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, this._dataStatesSamsaraPlatformSlots, BuildingDomain.CacheInfluencesSamsaraPlatformSlots, context);
			byte* pData = OperationAdder.FixedElementList_Set(9, 19, index, 8);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007E41 RID: 32321 RVA: 0x004B1498 File Offset: 0x004AF698
		public IntPair GetElement_SamsaraPlatformBornDict(int elementId)
		{
			return this._samsaraPlatformBornDict[elementId];
		}

		// Token: 0x06007E42 RID: 32322 RVA: 0x004B14B8 File Offset: 0x004AF6B8
		public bool TryGetElement_SamsaraPlatformBornDict(int elementId, out IntPair value)
		{
			return this._samsaraPlatformBornDict.TryGetValue(elementId, out value);
		}

		// Token: 0x06007E43 RID: 32323 RVA: 0x004B14D8 File Offset: 0x004AF6D8
		private unsafe void AddElement_SamsaraPlatformBornDict(int elementId, IntPair value, DataContext context)
		{
			this._samsaraPlatformBornDict.Add(elementId, value);
			this._modificationsSamsaraPlatformBornDict.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Add<int>(9, 20, elementId, 8);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007E44 RID: 32324 RVA: 0x004B152C File Offset: 0x004AF72C
		private unsafe void SetElement_SamsaraPlatformBornDict(int elementId, IntPair value, DataContext context)
		{
			this._samsaraPlatformBornDict[elementId] = value;
			this._modificationsSamsaraPlatformBornDict.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValueCollection_Set<int>(9, 20, elementId, 8);
			pData += value.Serialize(pData);
		}

		// Token: 0x06007E45 RID: 32325 RVA: 0x004B1580 File Offset: 0x004AF780
		private void RemoveElement_SamsaraPlatformBornDict(int elementId, DataContext context)
		{
			this._samsaraPlatformBornDict.Remove(elementId);
			this._modificationsSamsaraPlatformBornDict.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Remove<int>(9, 20, elementId);
		}

		// Token: 0x06007E46 RID: 32326 RVA: 0x004B15BC File Offset: 0x004AF7BC
		private void ClearSamsaraPlatformBornDict(DataContext context)
		{
			this._samsaraPlatformBornDict.Clear();
			this._modificationsSamsaraPlatformBornDict.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.FixedSingleValueCollection_Clear(9, 20);
		}

		// Token: 0x06007E47 RID: 32327 RVA: 0x004B15F8 File Offset: 0x004AF7F8
		public BuildingEarningsData GetElement_CollectBuildingEarningsData(BuildingBlockKey elementId)
		{
			return this._collectBuildingEarningsData[elementId];
		}

		// Token: 0x06007E48 RID: 32328 RVA: 0x004B1618 File Offset: 0x004AF818
		public bool TryGetElement_CollectBuildingEarningsData(BuildingBlockKey elementId, out BuildingEarningsData value)
		{
			return this._collectBuildingEarningsData.TryGetValue(elementId, out value);
		}

		// Token: 0x06007E49 RID: 32329 RVA: 0x004B1638 File Offset: 0x004AF838
		private unsafe void AddElement_CollectBuildingEarningsData(BuildingBlockKey elementId, BuildingEarningsData value, DataContext context)
		{
			this._collectBuildingEarningsData.Add(elementId, value);
			this._modificationsCollectBuildingEarningsData.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, BuildingDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 21, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 21, elementId, 0);
			}
		}

		// Token: 0x06007E4A RID: 32330 RVA: 0x004B16AC File Offset: 0x004AF8AC
		private unsafe void SetElement_CollectBuildingEarningsData(BuildingBlockKey elementId, BuildingEarningsData value, DataContext context)
		{
			this._collectBuildingEarningsData[elementId] = value;
			this._modificationsCollectBuildingEarningsData.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, BuildingDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<BuildingBlockKey>(9, 21, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<BuildingBlockKey>(9, 21, elementId, 0);
			}
		}

		// Token: 0x06007E4B RID: 32331 RVA: 0x004B171E File Offset: 0x004AF91E
		private void RemoveElement_CollectBuildingEarningsData(BuildingBlockKey elementId, DataContext context)
		{
			this._collectBuildingEarningsData.Remove(elementId);
			this._modificationsCollectBuildingEarningsData.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<BuildingBlockKey>(9, 21, elementId);
		}

		// Token: 0x06007E4C RID: 32332 RVA: 0x004B175A File Offset: 0x004AF95A
		private void ClearCollectBuildingEarningsData(DataContext context)
		{
			this._collectBuildingEarningsData.Clear();
			this._modificationsCollectBuildingEarningsData.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, BuildingDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(9, 21);
		}

		// Token: 0x06007E4D RID: 32333 RVA: 0x004B1794 File Offset: 0x004AF994
		public CharacterList GetElement_ShopManagerDict(BuildingBlockKey elementId)
		{
			return this._shopManagerDict[elementId];
		}

		// Token: 0x06007E4E RID: 32334 RVA: 0x004B17B4 File Offset: 0x004AF9B4
		public bool TryGetElement_ShopManagerDict(BuildingBlockKey elementId, out CharacterList value)
		{
			return this._shopManagerDict.TryGetValue(elementId, out value);
		}

		// Token: 0x06007E4F RID: 32335 RVA: 0x004B17D3 File Offset: 0x004AF9D3
		private void AddElement_ShopManagerDict(BuildingBlockKey elementId, CharacterList value, DataContext context)
		{
			this._shopManagerDict.Add(elementId, value);
			this._modificationsShopManagerDict.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, BuildingDomain.CacheInfluences, context);
		}

		// Token: 0x06007E50 RID: 32336 RVA: 0x004B1805 File Offset: 0x004AFA05
		private void SetElement_ShopManagerDict(BuildingBlockKey elementId, CharacterList value, DataContext context)
		{
			this._shopManagerDict[elementId] = value;
			this._modificationsShopManagerDict.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, BuildingDomain.CacheInfluences, context);
		}

		// Token: 0x06007E51 RID: 32337 RVA: 0x004B1837 File Offset: 0x004AFA37
		private void RemoveElement_ShopManagerDict(BuildingBlockKey elementId, DataContext context)
		{
			this._shopManagerDict.Remove(elementId);
			this._modificationsShopManagerDict.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, BuildingDomain.CacheInfluences, context);
		}

		// Token: 0x06007E52 RID: 32338 RVA: 0x004B1868 File Offset: 0x004AFA68
		private void ClearShopManagerDict(DataContext context)
		{
			this._shopManagerDict.Clear();
			this._modificationsShopManagerDict.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, BuildingDomain.CacheInfluences, context);
		}

		// Token: 0x06007E53 RID: 32339 RVA: 0x004B1898 File Offset: 0x004AFA98
		public TeaHorseCaravanData GetTeaHorseCaravanData()
		{
			return this._teaHorseCaravanData;
		}

		// Token: 0x06007E54 RID: 32340 RVA: 0x004B18B0 File Offset: 0x004AFAB0
		private unsafe void SetTeaHorseCaravanData(TeaHorseCaravanData value, DataContext context)
		{
			this._teaHorseCaravanData = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, this.DataStates, BuildingDomain.CacheInfluences, context);
			int dataSize = this._teaHorseCaravanData.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValue_Set(9, 23, dataSize);
			pData += this._teaHorseCaravanData.Serialize(pData);
		}

		// Token: 0x06007E55 RID: 32341 RVA: 0x004B1900 File Offset: 0x004AFB00
		public ushort GetShrineBuyTimes()
		{
			return this._shrineBuyTimes;
		}

		// Token: 0x06007E56 RID: 32342 RVA: 0x004B1918 File Offset: 0x004AFB18
		public unsafe void SetShrineBuyTimes(ushort value, DataContext context)
		{
			this._shrineBuyTimes = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, this.DataStates, BuildingDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(9, 24, 2);
			*(short*)pData = (short)this._shrineBuyTimes;
			pData += 2;
		}

		// Token: 0x06007E57 RID: 32343 RVA: 0x004B1958 File Offset: 0x004AFB58
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06007E58 RID: 32344 RVA: 0x004B1964 File Offset: 0x004AFB64
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			foreach (KeyValuePair<Location, BuildingAreaData> entry in this._buildingAreas)
			{
				Location elementId = entry.Key;
				BuildingAreaData value = entry.Value;
				byte* pData = OperationAdder.FixedSingleValueCollection_Add<Location>(9, 0, elementId, 2);
				pData += value.Serialize(pData);
			}
			foreach (KeyValuePair<BuildingBlockKey, BuildingBlockData> entry2 in this._buildingBlocks)
			{
				BuildingBlockKey elementId2 = entry2.Key;
				BuildingBlockData value2 = entry2.Value;
				byte* pData2 = OperationAdder.FixedSingleValueCollection_Add<BuildingBlockKey>(9, 1, elementId2, 16);
				pData2 += value2.Serialize(pData2);
			}
			int elementsCount = this._taiwuBuildingAreas.Count;
			int contentSize = 4 * elementsCount;
			int dataSize = 2 + contentSize;
			byte* pData3 = OperationAdder.DynamicSingleValue_Set(9, 2, dataSize);
			*(short*)pData3 = (short)((ushort)elementsCount);
			pData3 += 2;
			for (int i = 0; i < elementsCount; i++)
			{
				pData3 += this._taiwuBuildingAreas[i].Serialize(pData3);
			}
			foreach (KeyValuePair<BuildingBlockKey, sbyte> entry3 in this._CollectBuildingResourceType)
			{
				BuildingBlockKey elementId3 = entry3.Key;
				sbyte value3 = entry3.Value;
				byte* pData4 = OperationAdder.FixedSingleValueCollection_Add<BuildingBlockKey>(9, 3, elementId3, 1);
				*pData4 = (byte)value3;
				pData4++;
			}
			foreach (KeyValuePair<BuildingBlockKey, int> entry4 in this._customBuildingName)
			{
				BuildingBlockKey elementId4 = entry4.Key;
				int value4 = entry4.Value;
				byte* pData5 = OperationAdder.FixedSingleValueCollection_Add<BuildingBlockKey>(9, 5, elementId4, 4);
				*(int*)pData5 = value4;
				pData5 += 4;
			}
			int elementsCount2 = this._newCompleteOperationBuildings.Count;
			int contentSize2 = 8 * elementsCount2;
			int dataSize2 = 2 + contentSize2;
			byte* pData6 = OperationAdder.DynamicSingleValue_Set(9, 6, dataSize2);
			*(short*)pData6 = (short)((ushort)elementsCount2);
			pData6 += 2;
			for (int j = 0; j < elementsCount2; j++)
			{
				pData6 += this._newCompleteOperationBuildings[j].Serialize(pData6);
			}
			foreach (KeyValuePair<int, ChickenBlessingInfoData> entry5 in this._chickenBlessingInfoData)
			{
				int elementId5 = entry5.Key;
				ChickenBlessingInfoData value5 = entry5.Value;
				int dataSize3 = value5.GetSerializedSize();
				byte* pData7 = OperationAdder.DynamicSingleValueCollection_Add<int>(9, 7, elementId5, dataSize3);
				pData7 += value5.Serialize(pData7);
			}
			foreach (KeyValuePair<int, Chicken> entry6 in this._chicken)
			{
				int elementId6 = entry6.Key;
				Chicken value6 = entry6.Value;
				byte* pData8 = OperationAdder.FixedSingleValueCollection_Add<int>(9, 8, elementId6, 12);
				pData8 += value6.Serialize(pData8);
			}
			byte* pData9 = OperationAdder.FixedSingleValue_Set(9, 9, 120);
			for (int k = 0; k < 15; k++)
			{
				pData9 += this._collectionCrickets[k].Serialize(pData9);
			}
			byte* pData10 = OperationAdder.FixedSingleValue_Set(9, 10, 120);
			for (int l = 0; l < 15; l++)
			{
				pData10 += this._collectionCricketJars[l].Serialize(pData10);
			}
			byte* pData11 = OperationAdder.FixedSingleValue_Set(9, 11, 60);
			for (int m = 0; m < 15; m++)
			{
				*(int*)(pData11 + (IntPtr)m * 4) = this._collectionCricketRegen[m];
			}
			pData11 += 60;
			foreach (KeyValuePair<BuildingBlockKey, MakeItemData> entry7 in this._makeItemDict)
			{
				BuildingBlockKey elementId7 = entry7.Key;
				MakeItemData value7 = entry7.Value;
				bool flag = value7 != null;
				if (flag)
				{
					int contentSize3 = value7.GetSerializedSize();
					byte* pData12 = OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 12, elementId7, contentSize3);
					pData12 += value7.Serialize(pData12);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 12, elementId7, 0);
				}
			}
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> entry8 in this._residences)
			{
				BuildingBlockKey elementId8 = entry8.Key;
				CharacterList value8 = entry8.Value;
				int dataSize4 = value8.GetSerializedSize();
				byte* pData13 = OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 13, elementId8, dataSize4);
				pData13 += value8.Serialize(pData13);
			}
			foreach (KeyValuePair<BuildingBlockKey, CharacterList> entry9 in this._comfortableHouses)
			{
				BuildingBlockKey elementId9 = entry9.Key;
				CharacterList value9 = entry9.Value;
				int dataSize5 = value9.GetSerializedSize();
				byte* pData14 = OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 14, elementId9, dataSize5);
				pData14 += value9.Serialize(pData14);
			}
			int dataSize6 = this._homeless.GetSerializedSize();
			byte* pData15 = OperationAdder.DynamicSingleValue_Set(9, 15, dataSize6);
			pData15 += this._homeless.Serialize(pData15);
			byte* pData16 = OperationAdder.FixedSingleValue_Set(9, 16, 12);
			pData16 += this._samsaraPlatformAddMainAttributes.Serialize(pData16);
			byte* pData17 = OperationAdder.FixedSingleValue_Set(9, 17, 28);
			pData17 += this._samsaraPlatformAddCombatSkillQualifications.Serialize(pData17);
			byte* pData18 = OperationAdder.FixedSingleValue_Set(9, 18, 32);
			pData18 += this._samsaraPlatformAddLifeSkillQualifications.Serialize(pData18);
			byte* pData19 = OperationAdder.FixedElementList_InsertRange(9, 19, 0, 6, 48);
			for (int n = 0; n < 6; n++)
			{
				pData19 += this._samsaraPlatformSlots[n].Serialize(pData19);
			}
			foreach (KeyValuePair<int, IntPair> entry10 in this._samsaraPlatformBornDict)
			{
				int elementId10 = entry10.Key;
				IntPair value10 = entry10.Value;
				byte* pData20 = OperationAdder.FixedSingleValueCollection_Add<int>(9, 20, elementId10, 8);
				pData20 += value10.Serialize(pData20);
			}
			foreach (KeyValuePair<BuildingBlockKey, BuildingEarningsData> entry11 in this._collectBuildingEarningsData)
			{
				BuildingBlockKey elementId11 = entry11.Key;
				BuildingEarningsData value11 = entry11.Value;
				bool flag2 = value11 != null;
				if (flag2)
				{
					int contentSize4 = value11.GetSerializedSize();
					byte* pData21 = OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 21, elementId11, contentSize4);
					pData21 += value11.Serialize(pData21);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<BuildingBlockKey>(9, 21, elementId11, 0);
				}
			}
			int dataSize7 = this._teaHorseCaravanData.GetSerializedSize();
			byte* pData22 = OperationAdder.DynamicSingleValue_Set(9, 23, dataSize7);
			pData22 += this._teaHorseCaravanData.Serialize(pData22);
			byte* pData23 = OperationAdder.FixedSingleValue_Set(9, 24, 2);
			*(short*)pData23 = (short)this._shrineBuyTimes;
			pData23 += 2;
		}

		// Token: 0x06007E59 RID: 32345 RVA: 0x004B214C File Offset: 0x004B034C
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 0));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 1));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(9, 2));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 3));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 5));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(9, 6));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(9, 7));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 8));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 9));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 10));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 11));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(9, 12));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(9, 13));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(9, 14));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(9, 15));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 16));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 17));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 18));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedElementList_GetAll(9, 19));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValueCollection_GetAll(9, 20));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(9, 21));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(9, 23));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(9, 24));
		}

		// Token: 0x06007E5A RID: 32346 RVA: 0x004B2340 File Offset: 0x004B0540
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
					this._modificationsBuildingAreas.Reset();
				}
				result = Serializer.SerializeModifications<Location>(this._buildingAreas, dataPool);
				break;
			case 1:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					this._modificationsBuildingBlocks.Reset();
				}
				result = Serializer.SerializeModifications<BuildingBlockKey>(this._buildingBlocks, dataPool);
				break;
			case 2:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				result = Serializer.Serialize(this._taiwuBuildingAreas, dataPool);
				break;
			case 3:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					this._modificationsCollectBuildingResourceType.Reset();
				}
				result = Serializer.SerializeModifications<BuildingBlockKey>(this._CollectBuildingResourceType, dataPool);
				break;
			case 4:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					this._modificationsBuildingOperatorDict.Reset();
				}
				result = Serializer.SerializeModifications<BuildingBlockKey>(this._buildingOperatorDict, dataPool);
				break;
			case 5:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					this._modificationsCustomBuildingName.Reset();
				}
				result = Serializer.SerializeModifications<BuildingBlockKey>(this._customBuildingName, dataPool);
				break;
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					this._modificationsChickenBlessingInfoData.Reset();
				}
				result = Serializer.SerializeModifications<int>(this._chickenBlessingInfoData, dataPool);
				break;
			case 8:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					this._modificationsChicken.Reset();
				}
				result = Serializer.SerializeModifications<int>(this._chicken, dataPool);
				break;
			case 9:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
				}
				result = Serializer.Serialize(this._collectionCrickets, dataPool);
				break;
			case 10:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
				}
				result = Serializer.Serialize(this._collectionCricketJars, dataPool);
				break;
			case 11:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
				}
				result = Serializer.Serialize(this._collectionCricketRegen, dataPool);
				break;
			case 12:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
				}
				result = Serializer.Serialize(this._homeless, dataPool);
				break;
			case 16:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
				}
				result = Serializer.Serialize(this._samsaraPlatformAddMainAttributes, dataPool);
				break;
			case 17:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
				}
				result = Serializer.Serialize(this._samsaraPlatformAddCombatSkillQualifications, dataPool);
				break;
			case 18:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
				}
				result = Serializer.Serialize(this._samsaraPlatformAddLifeSkillQualifications, dataPool);
				break;
			case 19:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesSamsaraPlatformSlots, (int)subId0);
				}
				result = Serializer.Serialize(this._samsaraPlatformSlots[(int)subId0], dataPool);
				break;
			case 20:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
					this._modificationsSamsaraPlatformBornDict.Reset();
				}
				result = Serializer.SerializeModifications<int>(this._samsaraPlatformBornDict, dataPool);
				break;
			case 21:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
					this._modificationsCollectBuildingEarningsData.Reset();
				}
				result = Serializer.SerializeModifications<BuildingBlockKey>(this._collectBuildingEarningsData, dataPool);
				break;
			case 22:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
					this._modificationsShopManagerDict.Reset();
				}
				result = Serializer.SerializeModifications<BuildingBlockKey>(this._shopManagerDict, dataPool);
				break;
			case 23:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
				}
				result = Serializer.Serialize(this._teaHorseCaravanData, dataPool);
				break;
			case 24:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
				}
				result = Serializer.Serialize(this._shrineBuyTimes, dataPool);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007E5B RID: 32347 RVA: 0x004B2880 File Offset: 0x004B0A80
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 3:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 4:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 5:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 8:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 9:
				Serializer.Deserialize(dataPool, valueOffset, ref this._collectionCrickets);
				this.SetCollectionCrickets(this._collectionCrickets, context);
				break;
			case 10:
				Serializer.Deserialize(dataPool, valueOffset, ref this._collectionCricketJars);
				this.SetCollectionCricketJars(this._collectionCricketJars, context);
				break;
			case 11:
				Serializer.Deserialize(dataPool, valueOffset, ref this._collectionCricketRegen);
				this.SetCollectionCricketRegen(this._collectionCricketRegen, context);
				break;
			case 12:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
				Serializer.Deserialize(dataPool, valueOffset, ref this._homeless);
				this.SetHomeless(this._homeless, context);
				break;
			case 16:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 17:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 18:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 19:
			{
				IntPair value = default(IntPair);
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				this._samsaraPlatformSlots[(int)subId0] = value;
				this.SetElement_SamsaraPlatformSlots((int)subId0, value, context);
				break;
			}
			case 20:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 21:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 22:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 23:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 24:
				Serializer.Deserialize(dataPool, valueOffset, ref this._shrineBuyTimes);
				this.SetShrineBuyTimes(this._shrineBuyTimes, context);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06007E5C RID: 32348 RVA: 0x004B2D70 File Offset: 0x004B0F70
		public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
		{
			int argsOffset = operation.ArgsOffset;
			int result;
			switch (operation.MethodId)
			{
			case 0:
			{
				int argsCount = operation.ArgsCount;
				int num = argsCount;
				if (num != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey);
				List<ShopEventData> returnValue = this.GetShopEventDataList(blockKey);
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey2 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey2);
				sbyte index = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index);
				int charId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
				this.SetShopManager(context, blockKey2, index, charId);
				result = -1;
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey3 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey3);
				sbyte resourceType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref resourceType);
				this.SetCollectBuildingResourceType(context, blockKey3, resourceType);
				result = -1;
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key);
				bool isPawnShop = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isPawnShop);
				this.ClearBuildingBlockEarningsData(context, key, isPawnShop);
				result = -1;
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key2 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key2);
				int returnValue2 = this.CalcResourceOutput(context, key2);
				result = Serializer.Serialize(returnValue2, returnDataPool);
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey4 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey4);
				BuildingEarningsData returnValue3 = this.GetBuildingEarningData(blockKey4);
				result = Serializer.Serialize(returnValue3, returnDataPool);
				break;
			}
			case 6:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey5 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey5);
				List<int> returnValue4 = this.GetBuildingOperatesData(blockKey5);
				result = Serializer.Serialize(returnValue4, returnDataPool);
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey6 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey6);
				int returnValue5 = this.GetBuildingBuildPeopleAttainments(blockKey6);
				result = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 8:
				switch (operation.ArgsCount)
				{
				case 3:
				{
					BuildingBlockKey key3 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key3);
					int earningDataIndex = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref earningDataIndex);
					bool isPutInInventory = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isPutInInventory);
					BuildingBlockKey returnValue6 = this.AcceptBuildingBlockCollectEarning(context, key3, earningDataIndex, isPutInInventory, true, false);
					result = Serializer.Serialize(returnValue6, returnDataPool);
					break;
				}
				case 4:
				{
					BuildingBlockKey key4 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key4);
					int earningDataIndex2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref earningDataIndex2);
					bool isPutInInventory2 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isPutInInventory2);
					bool isSetData = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isSetData);
					BuildingBlockKey returnValue7 = this.AcceptBuildingBlockCollectEarning(context, key4, earningDataIndex2, isPutInInventory2, isSetData, false);
					result = Serializer.Serialize(returnValue7, returnDataPool);
					break;
				}
				case 5:
				{
					BuildingBlockKey key5 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key5);
					int earningDataIndex3 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref earningDataIndex3);
					bool isPutInInventory3 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isPutInInventory3);
					bool isSetData2 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isSetData2);
					bool isCostMoney = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isCostMoney);
					BuildingBlockKey returnValue8 = this.AcceptBuildingBlockCollectEarning(context, key5, earningDataIndex3, isPutInInventory3, isSetData2, isCostMoney);
					result = Serializer.Serialize(returnValue8, returnDataPool);
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				break;
			case 9:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key6 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key6);
				bool isPutInInventory4 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isPutInInventory4);
				this.AcceptBuildingBlockCollectEarningQuick(context, key6, isPutInInventory4);
				result = -1;
				break;
			}
			case 10:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 2)
				{
					if (num10 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					BuildingBlockKey key7 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key7);
					int earningDataIndex4 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref earningDataIndex4);
					bool isSetData3 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isSetData3);
					int returnValue9 = this.AcceptBuildingBlockRecruitPeople(context, key7, earningDataIndex4, isSetData3);
					result = Serializer.Serialize(returnValue9, returnDataPool);
				}
				else
				{
					BuildingBlockKey key8 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key8);
					int earningDataIndex5 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref earningDataIndex5);
					int returnValue10 = this.AcceptBuildingBlockRecruitPeople(context, key8, earningDataIndex5, true);
					result = Serializer.Serialize(returnValue10, returnDataPool);
				}
				break;
			}
			case 11:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key9 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key9);
				List<int> returnValue11 = this.AcceptBuildingBlockRecruitPeopleQuick(context, key9);
				result = Serializer.Serialize(returnValue11, returnDataPool);
				break;
			}
			case 12:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key10 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key10);
				int earningDataIndex6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref earningDataIndex6);
				ItemKey itemKey = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey);
				bool isFromInventory = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isFromInventory);
				this.ShopBuildingSoldItemAdd(context, key10, earningDataIndex6, itemKey, isFromInventory);
				result = -1;
				break;
			}
			case 13:
			{
				int argsCount13 = operation.ArgsCount;
				int num13 = argsCount13;
				if (num13 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key11 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key11);
				int earningDataIndex7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref earningDataIndex7);
				ItemKey itemKey2 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey2);
				bool isFromInventory2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isFromInventory2);
				this.ShopBuildingSoldItemChange(context, key11, earningDataIndex7, itemKey2, isFromInventory2);
				result = -1;
				break;
			}
			case 14:
			{
				int argsCount14 = operation.ArgsCount;
				int num14 = argsCount14;
				if (num14 != 2)
				{
					if (num14 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					BuildingBlockKey key12 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key12);
					int earningDataIndex8 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref earningDataIndex8);
					bool isSetData4 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isSetData4);
					this.ShopBuildingSoldItemReceive(context, key12, earningDataIndex8, isSetData4);
					result = -1;
				}
				else
				{
					BuildingBlockKey key13 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key13);
					int earningDataIndex9 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref earningDataIndex9);
					this.ShopBuildingSoldItemReceive(context, key13, earningDataIndex9, true);
					result = -1;
				}
				break;
			}
			case 15:
			{
				int argsCount15 = operation.ArgsCount;
				int num15 = argsCount15;
				if (num15 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key14 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key14);
				this.ShopBuildingSoldItemReceiveQuick(context, key14);
				result = -1;
				break;
			}
			case 16:
			{
				int argsCount16 = operation.ArgsCount;
				int num16 = argsCount16;
				if (num16 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<ItemDisplayData> returnValue12 = this.QuickCollectShopItem(context);
				result = Serializer.Serialize(returnValue12, returnDataPool);
				break;
			}
			case 17:
			{
				int argsCount17 = operation.ArgsCount;
				int num17 = argsCount17;
				if (num17 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int returnValue13 = this.QuickCollectShopItemCount(context);
				result = Serializer.Serialize(returnValue13, returnDataPool);
				break;
			}
			case 18:
			{
				int argsCount18 = operation.ArgsCount;
				int num18 = argsCount18;
				if (num18 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.QuickCollectShopSoldItem(context);
				result = -1;
				break;
			}
			case 19:
			{
				int argsCount19 = operation.ArgsCount;
				int num19 = argsCount19;
				if (num19 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int returnValue14 = this.QuickCollectShopSoldItemCount(context);
				result = Serializer.Serialize(returnValue14, returnDataPool);
				break;
			}
			case 20:
			{
				int argsCount20 = operation.ArgsCount;
				int num20 = argsCount20;
				if (num20 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> returnValue15 = this.QuickRecruitPeople(context);
				result = Serializer.Serialize(returnValue15, returnDataPool);
				break;
			}
			case 21:
			{
				int argsCount21 = operation.ArgsCount;
				int num21 = argsCount21;
				if (num21 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int returnValue16 = this.QuickRecruitPeopleCount(context);
				result = Serializer.Serialize(returnValue16, returnDataPool);
				break;
			}
			case 22:
			{
				int argsCount22 = operation.ArgsCount;
				int num22 = argsCount22;
				if (num22 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.QuickCollectBuildingEarn(context);
				result = -1;
				break;
			}
			case 23:
			{
				int argsCount23 = operation.ArgsCount;
				int num23 = argsCount23;
				if (num23 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int returnValue17 = this.QuickCollectBuildingEarnCount(context);
				result = Serializer.Serialize(returnValue17, returnDataPool);
				break;
			}
			case 24:
			{
				int argsCount24 = operation.ArgsCount;
				int num24 = argsCount24;
				if (num24 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key15 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key15);
				ItemKey itemKey3 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey3);
				ItemSourceType itemSourceType = ItemSourceType.Equipment;
				argsOffset += Serializer.DeserializeDefault<ItemSourceType>(argDataPool, argsOffset, ref itemSourceType);
				this.AddFixBook(context, key15, itemKey3, itemSourceType);
				result = -1;
				break;
			}
			case 25:
			{
				int argsCount25 = operation.ArgsCount;
				int num25 = argsCount25;
				if (num25 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key16 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key16);
				ItemKey itemKey4 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey4);
				ItemSourceType itemSourceType2 = ItemSourceType.Equipment;
				argsOffset += Serializer.DeserializeDefault<ItemSourceType>(argDataPool, argsOffset, ref itemSourceType2);
				ValueTuple<short, BuildingBlockData> returnValue18 = this.ChangeFixBook(context, key16, itemKey4, itemSourceType2);
				result = Serializer.Serialize(returnValue18, returnDataPool);
				break;
			}
			case 26:
			{
				int argsCount26 = operation.ArgsCount;
				int num26 = argsCount26;
				if (num26 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key17 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key17);
				bool isPutInInventory5 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isPutInInventory5);
				this.ReceiveFixBook(context, key17, isPutInInventory5);
				result = -1;
				break;
			}
			case 27:
			{
				int argsCount27 = operation.ArgsCount;
				int num27 = argsCount27;
				if (num27 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key18 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key18);
				int returnValue19 = this.GetFixBookProgress(context, key18);
				result = Serializer.Serialize(returnValue19, returnDataPool);
				break;
			}
			case 28:
			{
				int argsCount28 = operation.ArgsCount;
				int num28 = argsCount28;
				if (num28 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte state = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref state);
				this.SetTeaHorseCaravanState(context, state);
				result = -1;
				break;
			}
			case 29:
			{
				int argsCount29 = operation.ArgsCount;
				int num29 = argsCount29;
				if (num29 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<ItemKey> carryItems = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref carryItems);
				List<ItemKey> gainItems = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref gainItems);
				this.ExchangeItemToReplenishment(context, carryItems, gainItems);
				result = -1;
				break;
			}
			case 30:
			{
				int argsCount30 = operation.ArgsCount;
				int num30 = argsCount30;
				if (num30 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.StartSearchReplenishment(context);
				result = -1;
				break;
			}
			case 31:
			{
				int argsCount31 = operation.ArgsCount;
				int num31 = argsCount31;
				if (num31 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.QuickGetExchangeItem(context);
				result = -1;
				break;
			}
			case 32:
			{
				int argsCount32 = operation.ArgsCount;
				int num32 = argsCount32;
				if (num32 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short areaId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId);
				short blockId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockId);
				short buildingBlockIndex = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockIndex);
				TaiwuShrineDisplayData returnValue20 = this.GetShrineDisplayData(context, areaId, blockId, buildingBlockIndex);
				result = Serializer.Serialize(returnValue20, returnDataPool);
				break;
			}
			case 33:
			{
				int argsCount33 = operation.ArgsCount;
				int num33 = argsCount33;
				if (num33 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int characterId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterId);
				SkillQualificationBonus bonus = default(SkillQualificationBonus);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bonus);
				this.TeachSkill(context, characterId, bonus);
				result = -1;
				break;
			}
			case 34:
			{
				int argsCount34 = operation.ArgsCount;
				int num34 = argsCount34;
				if (num34 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int index2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index2);
				bool isCricket = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isCricket);
				ItemKey itemKey5 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey5);
				this.CricketCollectionAdd(context, index2, isCricket, itemKey5);
				result = -1;
				break;
			}
			case 35:
			{
				int argsCount35 = operation.ArgsCount;
				int num35 = argsCount35;
				if (num35 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int index3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index3);
				bool isCricket2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isCricket2);
				this.CricketCollectionRemove(context, index3, isCricket2);
				result = -1;
				break;
			}
			case 36:
			{
				int argsCount36 = operation.ArgsCount;
				int num36 = argsCount36;
				if (num36 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemDisplayData[] returnValue21 = this.GetCollectionCrickets(context);
				result = Serializer.Serialize(returnValue21, returnDataPool);
				break;
			}
			case 37:
			{
				int argsCount37 = operation.ArgsCount;
				int num37 = argsCount37;
				if (num37 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemDisplayData[] returnValue22 = this.GetCollectionJars(context);
				result = Serializer.Serialize(returnValue22, returnDataPool);
				break;
			}
			case 38:
			{
				int argsCount38 = operation.ArgsCount;
				int num38 = argsCount38;
				if (num38 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int[] returnValue23 = this.GetCollectionCricketRegen(context);
				result = Serializer.Serialize(returnValue23, returnDataPool);
				break;
			}
			case 39:
			{
				int argsCount39 = operation.ArgsCount;
				int num39 = argsCount39;
				if (num39 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int returnValue24 = this.GetAuthorityGain(context);
				result = Serializer.Serialize(returnValue24, returnDataPool);
				break;
			}
			case 40:
			{
				int argsCount40 = operation.ArgsCount;
				int num40 = argsCount40;
				if (num40 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short buildingTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingTemplateId);
				BuildingBlockKey blockKey7 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey7);
				sbyte level = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref level);
				ValueTuple<short, BuildingBlockData> returnValue25 = this.GmCmd_BuildImmediately(context, buildingTemplateId, blockKey7, level);
				result = Serializer.Serialize(returnValue25, returnDataPool);
				break;
			}
			case 41:
			{
				int argsCount41 = operation.ArgsCount;
				int num41 = argsCount41;
				if (num41 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey8 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey8);
				ValueTuple<short, BuildingBlockData> returnValue26 = this.GmCmd_RemoveBuildingImmediately(context, blockKey8);
				result = Serializer.Serialize(returnValue26, returnDataPool);
				break;
			}
			case 42:
			{
				int argsCount42 = operation.ArgsCount;
				int num42 = argsCount42;
				if (num42 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				StartMakeArguments startMakeArguments = default(StartMakeArguments);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref startMakeArguments);
				MakeItemData returnValue27 = this.StartMakeItem(context, startMakeArguments);
				result = Serializer.Serialize(returnValue27, returnDataPool);
				break;
			}
			case 43:
			{
				int argsCount43 = operation.ArgsCount;
				int num43 = argsCount43;
				if (num43 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				MakeConditionArguments makeConditionArguments = default(MakeConditionArguments);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref makeConditionArguments);
				bool returnValue28 = this.CheckMakeCondition(makeConditionArguments);
				result = Serializer.Serialize(returnValue28, returnDataPool);
				break;
			}
			case 44:
			{
				int argsCount44 = operation.ArgsCount;
				int num44 = argsCount44;
				if (num44 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey buildingBlockKey = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey);
				List<ItemDisplayData> returnValue29 = this.GetMakeItems(context, buildingBlockKey);
				result = Serializer.Serialize(returnValue29, returnDataPool);
				break;
			}
			case 45:
			{
				int argsCount45 = operation.ArgsCount;
				int num45 = argsCount45;
				if (num45 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey buildingBlockKey2 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey2);
				MakeItemData returnValue30 = this.GetMakingItemData(buildingBlockKey2);
				result = Serializer.Serialize(returnValue30, returnDataPool);
				break;
			}
			case 46:
			{
				int argsCount46 = operation.ArgsCount;
				int num46 = argsCount46;
				if (num46 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId2);
				ItemKey toolKey = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey);
				ItemKey itemKey6 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey6);
				ItemDisplayData returnValue31 = this.RepairItem(context, charId2, toolKey, itemKey6);
				result = Serializer.Serialize(returnValue31, returnDataPool);
				break;
			}
			case 47:
			{
				int argsCount47 = operation.ArgsCount;
				int num47 = argsCount47;
				if (num47 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId3);
				ItemKey toolKey2 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey2);
				ItemKey itemKey7 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey7);
				BuildingBlockKey buildingBlockKey3 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey3);
				bool returnValue32 = this.CheckRepairConditionIsMeet(charId3, toolKey2, itemKey7, buildingBlockKey3);
				result = Serializer.Serialize(returnValue32, returnDataPool);
				break;
			}
			case 48:
			{
				int argsCount48 = operation.ArgsCount;
				int num48 = argsCount48;
				if (num48 != 5)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId4);
				ItemDisplayData tool = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref tool);
				ItemDisplayData target = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref target);
				ItemDisplayData[] poisons = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref poisons);
				List<ItemDisplayData> condensePoisonItemList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref condensePoisonItemList);
				ValueTuple<bool, ItemDisplayData> returnValue33 = this.AddItemPoison(context, charId4, tool, target, poisons, condensePoisonItemList);
				result = Serializer.Serialize(returnValue33, returnDataPool);
				break;
			}
			case 49:
			{
				int argsCount49 = operation.ArgsCount;
				int num49 = argsCount49;
				if (num49 != 6)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId5);
				ItemKey toolKey3 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey3);
				ItemKey targetKey = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetKey);
				ItemKey[] poisonKeys = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref poisonKeys);
				BuildingBlockKey buildingBlockKey4 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey4);
				FullPoisonEffects tempPoisonEffects = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref tempPoisonEffects);
				bool returnValue34 = this.CheckAddPoisonCondition(charId5, toolKey3, targetKey, poisonKeys, buildingBlockKey4, tempPoisonEffects);
				result = Serializer.Serialize(returnValue34, returnDataPool);
				break;
			}
			case 50:
			{
				int argsCount50 = operation.ArgsCount;
				int num50 = argsCount50;
				if (num50 != 5)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId6);
				ItemDisplayData tool2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref tool2);
				ItemDisplayData target2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref target2);
				ItemDisplayData[] medicines = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref medicines);
				bool isExtract = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isExtract);
				ValueTuple<bool, List<ItemDisplayData>> returnValue35 = this.RemoveItemPoison(context, charId6, tool2, target2, medicines, isExtract);
				result = Serializer.Serialize(returnValue35, returnDataPool);
				break;
			}
			case 51:
			{
				int argsCount51 = operation.ArgsCount;
				int num51 = argsCount51;
				if (num51 != 6)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId7);
				ItemKey toolKey4 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey4);
				ItemKey targetKey2 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetKey2);
				ItemKey[] medicineKeys = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref medicineKeys);
				BuildingBlockKey buildingBlockKey5 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey5);
				bool isExtract2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isExtract2);
				bool returnValue36 = this.CheckRemovePoisonCondition(charId7, toolKey4, targetKey2, medicineKeys, buildingBlockKey5, isExtract2);
				result = Serializer.Serialize(returnValue36, returnDataPool);
				break;
			}
			case 52:
			{
				int argsCount52 = operation.ArgsCount;
				int num52 = argsCount52;
				if (num52 != 5)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId8);
				ItemDisplayData tool3 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref tool3);
				ItemDisplayData target3 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref target3);
				ItemDisplayData[] materialItemArray = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref materialItemArray);
				List<ItemSourceChange> changeList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref changeList);
				ItemDisplayData returnValue37 = this.RefineItem(context, charId8, tool3, target3, materialItemArray, changeList);
				result = Serializer.Serialize(returnValue37, returnDataPool);
				break;
			}
			case 53:
			{
				int argsCount53 = operation.ArgsCount;
				int num53 = argsCount53;
				if (num53 != 5)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId9 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId9);
				ItemKey toolKey5 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey5);
				ItemKey equipItemKey = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref equipItemKey);
				ItemDisplayData[] materialItemData = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref materialItemData);
				BuildingBlockKey buildingBlockKey6 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey6);
				bool returnValue38 = this.CheckRefineCondition(charId9, toolKey5, equipItemKey, materialItemData, buildingBlockKey6);
				result = Serializer.Serialize(returnValue38, returnDataPool);
				break;
			}
			case 54:
			{
				int argsCount54 = operation.ArgsCount;
				int num54 = argsCount54;
				if (num54 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey9 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey9);
				short buildingTemplateId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingTemplateId2);
				int[] workers = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref workers);
				ValueTuple<short, BuildingBlockData> returnValue39 = this.Build(context, blockKey9, buildingTemplateId2, workers);
				result = Serializer.Serialize(returnValue39, returnDataPool);
				break;
			}
			case 55:
			{
				int argsCount55 = operation.ArgsCount;
				int num55 = argsCount55;
				if (num55 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey10 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey10);
				int[] workers2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref workers2);
				ValueTuple<short, BuildingBlockData> returnValue40 = this.Upgrade(context, blockKey10, workers2);
				result = Serializer.Serialize(returnValue40, returnDataPool);
				break;
			}
			case 56:
			{
				int argsCount56 = operation.ArgsCount;
				int num56 = argsCount56;
				if (num56 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey11 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey11);
				int[] workers3 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref workers3);
				ValueTuple<short, BuildingBlockData> returnValue41 = this.Remove(context, blockKey11, workers3);
				result = Serializer.Serialize(returnValue41, returnDataPool);
				break;
			}
			case 57:
			{
				int argsCount57 = operation.ArgsCount;
				int num57 = argsCount57;
				if (num57 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey12 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey12);
				bool stop = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref stop);
				ValueTuple<short, BuildingBlockData> returnValue42 = this.SetStopOperation(context, blockKey12, stop);
				result = Serializer.Serialize(returnValue42, returnDataPool);
				break;
			}
			case 58:
			{
				int argsCount58 = operation.ArgsCount;
				int num58 = argsCount58;
				if (num58 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey13 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey13);
				sbyte index4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index4);
				int charId10 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId10);
				this.SetOperator(context, blockKey13, index4, charId10);
				result = -1;
				break;
			}
			case 59:
			{
				int argsCount59 = operation.ArgsCount;
				int num59 = argsCount59;
				if (num59 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey14 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey14);
				bool maintenance = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref maintenance);
				ValueTuple<short, BuildingBlockData> returnValue43 = this.SetBuildingMaintenance(context, blockKey14, maintenance);
				result = Serializer.Serialize(returnValue43, returnDataPool);
				break;
			}
			case 60:
			{
				int argsCount60 = operation.ArgsCount;
				int num60 = argsCount60;
				if (num60 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey15 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey15);
				ValueTuple<short, BuildingBlockData> returnValue44 = this.Repair(context, blockKey15);
				result = Serializer.Serialize(returnValue44, returnDataPool);
				break;
			}
			case 61:
			{
				int argsCount61 = operation.ArgsCount;
				int num61 = argsCount61;
				if (num61 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<IntPair> operateRecord = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref operateRecord);
				Location location = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location);
				this.ConfirmPlanBuilding(context, operateRecord, location);
				result = -1;
				break;
			}
			case 62:
			{
				int argsCount62 = operation.ArgsCount;
				int num62 = argsCount62;
				if (num62 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId11 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId11);
				BuildingBlockKey buildingBlockKey7 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey7);
				this.AddToResidence(context, charId11, buildingBlockKey7);
				result = -1;
				break;
			}
			case 63:
			{
				int argsCount63 = operation.ArgsCount;
				int num63 = argsCount63;
				if (num63 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId12 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId12);
				BuildingBlockKey buildingBlockKey8 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey8);
				this.RemoveFromResidence(context, charId12, buildingBlockKey8);
				result = -1;
				break;
			}
			case 64:
			{
				int argsCount64 = operation.ArgsCount;
				int num64 = argsCount64;
				if (num64 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId13 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId13);
				BuildingBlockKey buildingBlockKey9 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey9);
				sbyte index5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index5);
				this.ReplaceCharacterInResidence(context, charId13, buildingBlockKey9, index5);
				result = -1;
				break;
			}
			case 65:
			{
				int argsCount65 = operation.ArgsCount;
				int num65 = argsCount65;
				if (num65 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charIdB = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charIdB);
				BuildingBlockKey buildingBlockKey10 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey10);
				sbyte index6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref index6);
				this.ReplaceCharacterInComfortableHouse(context, charIdB, buildingBlockKey10, index6);
				result = -1;
				break;
			}
			case 66:
			{
				int argsCount66 = operation.ArgsCount;
				int num66 = argsCount66;
				if (num66 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId14 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId14);
				BuildingBlockKey buildingBlockKey11 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey11);
				this.AddToComfortableHouse(context, charId14, buildingBlockKey11);
				result = -1;
				break;
			}
			case 67:
			{
				int argsCount67 = operation.ArgsCount;
				int num67 = argsCount67;
				if (num67 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId15 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId15);
				BuildingBlockKey buildingBlockKey12 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey12);
				this.RemoveFromComfortableHouse(context, charId15, buildingBlockKey12);
				result = -1;
				break;
			}
			case 68:
			{
				int argsCount68 = operation.ArgsCount;
				int num68 = argsCount68;
				if (num68 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey buildingBlockKey13 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey13);
				CharacterList returnValue45 = this.QuickFillResidence(context, buildingBlockKey13);
				result = Serializer.Serialize(returnValue45, returnDataPool);
				break;
			}
			case 69:
			{
				int argsCount69 = operation.ArgsCount;
				int num69 = argsCount69;
				if (num69 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key19 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key19);
				CharacterList returnValue46 = this.GetCharsInResidence(context, key19);
				result = Serializer.Serialize(returnValue46, returnDataPool);
				break;
			}
			case 70:
			{
				int argsCount70 = operation.ArgsCount;
				int num70 = argsCount70;
				if (num70 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey16 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey16);
				bool homelessFirst = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref homelessFirst);
				List<CharacterList> returnValue47 = this.GetAllResidents(context, blockKey16, homelessFirst);
				result = Serializer.Serialize(returnValue47, returnDataPool);
				break;
			}
			case 71:
			{
				int argsCount71 = operation.ArgsCount;
				int num71 = argsCount71;
				if (num71 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key20 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key20);
				CharacterList returnValue48 = this.GetCharsInComfortableHouse(context, key20);
				result = Serializer.Serialize(returnValue48, returnDataPool);
				break;
			}
			case 72:
			{
				int argsCount72 = operation.ArgsCount;
				int num72 = argsCount72;
				if (num72 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				CharacterList returnValue49 = this.GetHomeless(context);
				result = Serializer.Serialize(returnValue49, returnDataPool);
				break;
			}
			case 73:
			{
				int argsCount73 = operation.ArgsCount;
				int num73 = argsCount73;
				if (num73 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<SamsaraPlatformCharDisplayData> returnValue50 = this.GetSamsaraPlatformCharList(context);
				result = Serializer.Serialize(returnValue50, returnDataPool);
				break;
			}
			case 74:
			{
				int argsCount74 = operation.ArgsCount;
				int num74 = argsCount74;
				if (num74 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte destinyType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref destinyType);
				int charId16 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId16);
				this.SetSamsaraPlatformChar(context, destinyType, charId16);
				result = -1;
				break;
			}
			case 75:
			{
				int argsCount75 = operation.ArgsCount;
				int num75 = argsCount75;
				if (num75 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte destinyType2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref destinyType2);
				CharacterDisplayData returnValue51 = this.SamsaraPlatformReborn(context, destinyType2);
				result = Serializer.Serialize(returnValue51, returnDataPool);
				break;
			}
			case 76:
			{
				int argsCount76 = operation.ArgsCount;
				int num76 = argsCount76;
				if (num76 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location2 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location2);
				BuildingAreaData returnValue52 = this.GetBuildingAreaData(location2);
				result = Serializer.Serialize(returnValue52, returnDataPool);
				break;
			}
			case 77:
			{
				int argsCount77 = operation.ArgsCount;
				int num77 = argsCount77;
				if (num77 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location3 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location3);
				List<BuildingBlockData> returnValue53 = this.GetBuildingBlockList(location3);
				result = Serializer.Serialize(returnValue53, returnDataPool);
				break;
			}
			case 78:
			{
				int argsCount78 = operation.ArgsCount;
				int num78 = argsCount78;
				if (num78 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey17 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey17);
				BuildingBlockData returnValue54 = this.GetBuildingBlockData(blockKey17);
				result = Serializer.Serialize(returnValue54, returnDataPool);
				break;
			}
			case 79:
			{
				int argsCount79 = operation.ArgsCount;
				int num79 = argsCount79;
				if (num79 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey18 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey18);
				string name = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref name);
				this.SetBuildingCustomName(context, blockKey18, name);
				result = -1;
				break;
			}
			case 80:
			{
				int argsCount80 = operation.ArgsCount;
				int num80 = argsCount80;
				if (num80 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short areaId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId2);
				short blockId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockId2);
				int returnValue55 = this.GetEmptyBlockCount(areaId2, blockId2);
				result = Serializer.Serialize(returnValue55, returnDataPool);
				break;
			}
			case 81:
			{
				int argsCount81 = operation.ArgsCount;
				int num81 = argsCount81;
				if (num81 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int settlementId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId);
				short templateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId);
				int returnValue56 = this.AddChicken(context, settlementId, templateId);
				result = Serializer.Serialize(returnValue56, returnDataPool);
				break;
			}
			case 82:
			{
				int argsCount82 = operation.ArgsCount;
				int num82 = argsCount82;
				if (num82 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int id = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref id);
				this.RemoveChicken(context, id);
				result = -1;
				break;
			}
			case 83:
			{
				int argsCount83 = operation.ArgsCount;
				int num83 = argsCount83;
				if (num83 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.RemoveAllChicken(context);
				result = -1;
				break;
			}
			case 84:
			{
				int argsCount84 = operation.ArgsCount;
				int num84 = argsCount84;
				if (num84 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int id2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref id2);
				int targetSettlementId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetSettlementId);
				this.MoveChicken(context, id2, targetSettlementId);
				result = -1;
				break;
			}
			case 85:
			{
				int argsCount85 = operation.ArgsCount;
				int num85 = argsCount85;
				if (num85 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int id3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref id3);
				int targetSettlementId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref targetSettlementId2);
				this.TransferChicken(context, id3, targetSettlementId2);
				result = -1;
				break;
			}
			case 86:
			{
				int argsCount86 = operation.ArgsCount;
				int num86 = argsCount86;
				if (num86 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location4 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location4);
				List<int> returnValue57 = this.GetLocationChicken(location4);
				result = Serializer.Serialize(returnValue57, returnDataPool);
				break;
			}
			case 87:
			{
				int argsCount87 = operation.ArgsCount;
				int num87 = argsCount87;
				if (num87 != 1)
				{
					if (num87 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					int sourceSettlementId = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref sourceSettlementId);
					bool ignoreFulong = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref ignoreFulong);
					List<int> returnValue58 = this.GetSettlementChickenList(sourceSettlementId, ignoreFulong);
					result = Serializer.Serialize(returnValue58, returnDataPool);
				}
				else
				{
					int sourceSettlementId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref sourceSettlementId2);
					List<int> returnValue59 = this.GetSettlementChickenList(sourceSettlementId2, true);
					result = Serializer.Serialize(returnValue59, returnDataPool);
				}
				break;
			}
			case 88:
			{
				int argsCount88 = operation.ArgsCount;
				int num88 = argsCount88;
				if (num88 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int id4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref id4);
				Chicken returnValue60 = this.GetChickenData(id4);
				result = Serializer.Serialize(returnValue60, returnDataPool);
				break;
			}
			case 89:
			{
				int argsCount89 = operation.ArgsCount;
				int num89 = argsCount89;
				if (num89 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int id5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref id5);
				ItemKey itemKey8 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey8);
				sbyte returnValue61 = this.FeedChicken(context, id5, itemKey8);
				result = Serializer.Serialize(returnValue61, returnDataPool);
				break;
			}
			case 90:
			{
				int argsCount90 = operation.ArgsCount;
				int num90 = argsCount90;
				if (num90 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.InitMapBlockChicken(context);
				result = -1;
				break;
			}
			case 91:
			{
				int argsCount91 = operation.ArgsCount;
				int num91 = argsCount91;
				if (num91 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue62 = this.IsHaveChickenKing(context);
				result = Serializer.Serialize(returnValue62, returnDataPool);
				break;
			}
			case 92:
			{
				int argsCount92 = operation.ArgsCount;
				int num92 = argsCount92;
				if (num92 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey buildingBlockKey14 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey14);
				this.RemoveAllFormResidence(context, buildingBlockKey14);
				result = -1;
				break;
			}
			case 93:
			{
				int argsCount93 = operation.ArgsCount;
				int num93 = argsCount93;
				if (num93 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingAreaData areaData = new BuildingAreaData();
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaData);
				Location location5 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location5);
				short rootIndex = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref rootIndex);
				BuildingBlockItem configData = null;
				argsOffset += Serializer.DeserializeDefault<BuildingBlockItem>(argDataPool, argsOffset, ref configData);
				bool returnValue63 = this.NearDependBuildings(areaData, location5, rootIndex, configData);
				result = Serializer.Serialize(returnValue63, returnDataPool);
				break;
			}
			case 94:
			{
				int argsCount94 = operation.ArgsCount;
				int num94 = argsCount94;
				if (num94 != 2)
				{
					if (num94 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					BuildingBlockData blockData = new BuildingBlockData();
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockData);
					BuildingBlockKey blockKey19 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey19);
					bool isAverage = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAverage);
					int returnValue64 = this.GetBuildingAttainment(blockData, blockKey19, isAverage);
					result = Serializer.Serialize(returnValue64, returnDataPool);
				}
				else
				{
					BuildingBlockData blockData2 = new BuildingBlockData();
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockData2);
					BuildingBlockKey blockKey20 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey20);
					int returnValue65 = this.GetBuildingAttainment(blockData2, blockKey20, false);
					result = Serializer.Serialize(returnValue65, returnDataPool);
				}
				break;
			}
			case 95:
			{
				int argsCount95 = operation.ArgsCount;
				int num95 = argsCount95;
				if (num95 != 1)
				{
					if (num95 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					BuildingBlockKey blockKey21 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey21);
					bool isAverage2 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAverage2);
					int returnValue66 = this.GetAttainmentOfBuilding(blockKey21, isAverage2);
					result = Serializer.Serialize(returnValue66, returnDataPool);
				}
				else
				{
					BuildingBlockKey blockKey22 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey22);
					int returnValue67 = this.GetAttainmentOfBuilding(blockKey22, false);
					result = Serializer.Serialize(returnValue67, returnDataPool);
				}
				break;
			}
			case 96:
			{
				int argsCount96 = operation.ArgsCount;
				int num96 = argsCount96;
				if (num96 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key21 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key21);
				sbyte resourceType2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref resourceType2);
				int returnValue68 = this.CalcResourceOutputCount(key21, resourceType2);
				result = Serializer.Serialize(returnValue68, returnDataPool);
				break;
			}
			case 97:
			{
				int argsCount97 = operation.ArgsCount;
				int num97 = argsCount97;
				if (num97 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> charList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charList);
				byte dealType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref dealType);
				this.DealInfectedPeople(context, charList, dealType);
				result = -1;
				break;
			}
			case 98:
			{
				int argsCount98 = operation.ArgsCount;
				int num98 = argsCount98;
				if (num98 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey23 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey23);
				List<ItemDisplayData> returnValue69 = this.QuickCollectSingleShopItem(context, blockKey23);
				result = Serializer.Serialize(returnValue69, returnDataPool);
				break;
			}
			case 99:
			{
				int argsCount99 = operation.ArgsCount;
				int num99 = argsCount99;
				if (num99 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey24 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey24);
				this.QuickCollectSingleShopSoldItem(context, blockKey24);
				result = -1;
				break;
			}
			case 100:
			{
				int argsCount100 = operation.ArgsCount;
				int num100 = argsCount100;
				if (num100 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey25 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey25);
				List<int> returnValue70 = this.QuickRecruitSingleBuildingPeople(context, blockKey25);
				result = Serializer.Serialize(returnValue70, returnDataPool);
				break;
			}
			case 101:
			{
				int argsCount101 = operation.ArgsCount;
				int num101 = argsCount101;
				if (num101 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey buildingBlockKey15 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey15);
				CharacterList returnValue71 = this.QuickFillComfortableHouse(context, buildingBlockKey15);
				result = Serializer.Serialize(returnValue71, returnDataPool);
				break;
			}
			case 102:
			{
				int argsCount102 = operation.ArgsCount;
				int num102 = argsCount102;
				if (num102 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey buildingBlockKey16 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey16);
				this.RemoveAllFromComfortableHouse(context, buildingBlockKey16);
				result = -1;
				break;
			}
			case 103:
			{
				int argsCount103 = operation.ArgsCount;
				int num103 = argsCount103;
				if (num103 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> charIdList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charIdList);
				List<int> returnValue72 = this.SortedComfortableHousePeople(context, charIdList);
				result = Serializer.Serialize(returnValue72, returnDataPool);
				break;
			}
			case 104:
			{
				int argsCount104 = operation.ArgsCount;
				int num104 = argsCount104;
				if (num104 != 5)
				{
					if (num104 != 6)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					short materialTemplateId = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref materialTemplateId);
					ItemKey toolKey6 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey6);
					BuildingBlockKey buildingBlockKey17 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey17);
					sbyte lifeSkillType = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref lifeSkillType);
					List<short> makeItemSubtypeIdList = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref makeItemSubtypeIdList);
					short makeItemSubTypeId = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref makeItemSubTypeId);
					MakeResult returnValue73 = this.GetMakeResult(materialTemplateId, toolKey6, buildingBlockKey17, lifeSkillType, makeItemSubtypeIdList, makeItemSubTypeId);
					result = Serializer.Serialize(returnValue73, returnDataPool);
				}
				else
				{
					short materialTemplateId2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref materialTemplateId2);
					ItemKey toolKey7 = default(ItemKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey7);
					BuildingBlockKey buildingBlockKey18 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey18);
					sbyte lifeSkillType2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref lifeSkillType2);
					List<short> makeItemSubtypeIdList2 = null;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref makeItemSubtypeIdList2);
					MakeResult returnValue74 = this.GetMakeResult(materialTemplateId2, toolKey7, buildingBlockKey18, lifeSkillType2, makeItemSubtypeIdList2, -1);
					result = Serializer.Serialize(returnValue74, returnDataPool);
				}
				break;
			}
			case 105:
			{
				int argsCount105 = operation.ArgsCount;
				int num105 = argsCount105;
				if (num105 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int returnValue75 = this.GetSutraReadingRoomBuffValue();
				result = Serializer.Serialize(returnValue75, returnDataPool);
				break;
			}
			case 106:
			{
				int argsCount106 = operation.ArgsCount;
				int num106 = argsCount106;
				if (num106 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex);
				bool isAutoWork = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAutoWork);
				this.SetBuildingAutoWork(context, blockIndex, isAutoWork);
				result = -1;
				break;
			}
			case 107:
			{
				int argsCount107 = operation.ArgsCount;
				int num107 = argsCount107;
				if (num107 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex2);
				bool returnValue76 = this.GetBuildingIsAutoWork(blockIndex2);
				result = Serializer.Serialize(returnValue76, returnDataPool);
				break;
			}
			case 108:
			{
				int argsCount108 = operation.ArgsCount;
				int num108 = argsCount108;
				if (num108 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key22 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key22);
				List<ItemKey> itemList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemList);
				List<int> operateTypeList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref operateTypeList);
				this.ShopBuildingMultiChangeSoldItem(context, key22, itemList, operateTypeList);
				result = -1;
				break;
			}
			case 109:
			{
				int argsCount109 = operation.ArgsCount;
				int num109 = argsCount109;
				if (num109 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId17 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId17);
				List<MultiplyOperation> operationList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref operationList);
				List<ItemDisplayData> returnValue77 = this.RepairItemList(context, charId17, operationList);
				result = Serializer.Serialize(returnValue77, returnDataPool);
				break;
			}
			case 110:
			{
				int argsCount110 = operation.ArgsCount;
				int num110 = argsCount110;
				if (num110 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex3);
				bool isAutoSold = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAutoSold);
				this.SetBuildingAutoSold(context, blockIndex3, isAutoSold);
				result = -1;
				break;
			}
			case 111:
			{
				int argsCount111 = operation.ArgsCount;
				int num111 = argsCount111;
				if (num111 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex4);
				bool returnValue78 = this.GetBuildingIsAutoSold(blockIndex4);
				result = Serializer.Serialize(returnValue78, returnDataPool);
				break;
			}
			case 112:
			{
				int argsCount112 = operation.ArgsCount;
				int num112 = argsCount112;
				if (num112 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<sbyte> returnValue79 = this.GetXiangshuIdInKungfuRoom();
				result = Serializer.Serialize(returnValue79, returnDataPool);
				break;
			}
			case 113:
			{
				int argsCount113 = operation.ArgsCount;
				int num113 = argsCount113;
				if (num113 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId18 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId18);
				ItemKey toolKey8 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolKey8);
				ItemKey itemKey9 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey9);
				sbyte toolSourceType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref toolSourceType);
				ItemDisplayData returnValue80 = this.RepairItemOptional(context, charId18, toolKey8, itemKey9, toolSourceType);
				result = Serializer.Serialize(returnValue80, returnDataPool);
				break;
			}
			case 114:
			{
				int argsCount114 = operation.ArgsCount;
				int num114 = argsCount114;
				if (num114 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex5);
				bool resultFirst = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref resultFirst);
				this.SetShopIsResultFirst(context, blockIndex5, resultFirst);
				result = -1;
				break;
			}
			case 115:
			{
				int argsCount115 = operation.ArgsCount;
				int num115 = argsCount115;
				if (num115 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex6);
				bool returnValue81 = this.GetShopIsResultFirst(blockIndex6);
				result = Serializer.Serialize(returnValue81, returnDataPool);
				break;
			}
			case 116:
			{
				int argsCount116 = operation.ArgsCount;
				int num116 = argsCount116;
				if (num116 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex7);
				this.SetBuildingAutoExpandUpTop(context, blockIndex7);
				result = -1;
				break;
			}
			case 117:
			{
				int argsCount117 = operation.ArgsCount;
				int num117 = argsCount117;
				if (num117 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex8);
				this.SetBuildingAutoExpandDown(context, blockIndex8);
				result = -1;
				break;
			}
			case 118:
			{
				int argsCount118 = operation.ArgsCount;
				int num118 = argsCount118;
				if (num118 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex9 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex9);
				bool returnValue82 = this.GetBuildingIsAutoExpand(blockIndex9);
				result = Serializer.Serialize(returnValue82, returnDataPool);
				break;
			}
			case 119:
			{
				int argsCount119 = operation.ArgsCount;
				int num119 = argsCount119;
				if (num119 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex10 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex10);
				bool isAutoExpand = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAutoExpand);
				this.SetBuildingAutoExpand(context, blockIndex10, isAutoExpand);
				result = -1;
				break;
			}
			case 120:
			{
				int argsCount120 = operation.ArgsCount;
				int num120 = argsCount120;
				if (num120 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex11 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex11);
				this.SetBuildingAutoExpandDownBottom(context, blockIndex11);
				result = -1;
				break;
			}
			case 121:
			{
				int argsCount121 = operation.ArgsCount;
				int num121 = argsCount121;
				if (num121 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex12 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex12);
				this.SetBuildingAutoExpandUp(context, blockIndex12);
				result = -1;
				break;
			}
			case 122:
			{
				int argsCount122 = operation.ArgsCount;
				int num122 = argsCount122;
				if (num122 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int id6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref id6);
				string nickname = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref nickname);
				this.SetNickNameByChickenId(context, id6, nickname);
				result = -1;
				break;
			}
			case 123:
			{
				int argsCount123 = operation.ArgsCount;
				int num123 = argsCount123;
				if (num123 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location6 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location6);
				List<string> returnValue83 = this.GetChickensNicknameByIdList(location6);
				result = Serializer.Serialize(returnValue83, returnDataPool);
				break;
			}
			case 124:
			{
				int argsCount124 = operation.ArgsCount;
				int num124 = argsCount124;
				if (num124 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location7 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location7);
				List<Chicken> returnValue84 = this.GetSettlementChickenDataList(location7);
				result = Serializer.Serialize(returnValue84, returnDataPool);
				break;
			}
			case 125:
			{
				int argsCount125 = operation.ArgsCount;
				int num125 = argsCount125;
				if (num125 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short weatherId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref weatherId);
				this.SetTeaHorseCaravanWeather(context, weatherId);
				result = -1;
				break;
			}
			case 126:
			{
				int argsCount126 = operation.ArgsCount;
				int num126 = argsCount126;
				if (num126 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex13 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex13);
				bool returnValue85 = this.GetComfortableIsAutoCheckIn(blockIndex13);
				result = Serializer.Serialize(returnValue85, returnDataPool);
				break;
			}
			case 127:
			{
				int argsCount127 = operation.ArgsCount;
				int num127 = argsCount127;
				if (num127 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex14 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex14);
				bool returnValue86 = this.GetResidenceIsAutoCheckIn(blockIndex14);
				result = Serializer.Serialize(returnValue86, returnDataPool);
				break;
			}
			case 128:
			{
				int argsCount128 = operation.ArgsCount;
				int num128 = argsCount128;
				if (num128 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex15 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex15);
				bool isAutoCheckIn = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAutoCheckIn);
				this.SetComfortableAutoCheckIn(context, blockIndex15, isAutoCheckIn);
				result = -1;
				break;
			}
			case 129:
			{
				int argsCount129 = operation.ArgsCount;
				int num129 = argsCount129;
				if (num129 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short blockIndex16 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex16);
				bool isAutoCheckIn2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAutoCheckIn2);
				this.SetResidenceAutoCheckIn(context, blockIndex16, isAutoCheckIn2);
				result = -1;
				break;
			}
			case 130:
			{
				int argsCount130 = operation.ArgsCount;
				int num130 = argsCount130;
				if (num130 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short buildingTemplateId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingTemplateId3);
				this.GmCmd_AddLegacyBuilding(context, buildingTemplateId3);
				result = -1;
				break;
			}
			case 131:
			{
				int argsCount131 = operation.ArgsCount;
				int num131 = argsCount131;
				if (num131 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId19 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId19);
				bool add = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref add);
				this.SetUnlockedWorkingVillagers(context, charId19, add);
				result = -1;
				break;
			}
			case 132:
			{
				int argsCount132 = operation.ArgsCount;
				int num132 = argsCount132;
				if (num132 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey26 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey26);
				bool returnValue87 = this.CanAutoExpand(context, blockKey26);
				result = Serializer.Serialize(returnValue87, returnDataPool);
				break;
			}
			case 133:
			{
				int argsCount133 = operation.ArgsCount;
				int num133 = argsCount133;
				if (num133 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemDisplayData tool4 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref tool4);
				ItemDisplayData target4 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref target4);
				short weaveClothingTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref weaveClothingTemplateId);
				ItemDisplayData returnValue88 = this.WeaveClothingItem(context, tool4, target4, weaveClothingTemplateId);
				result = Serializer.Serialize(returnValue88, returnDataPool);
				break;
			}
			case 134:
			{
				int argsCount134 = operation.ArgsCount;
				int num134 = argsCount134;
				if (num134 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<Chicken> returnValue89 = this.GmCmd_GetChickenData();
				result = Serializer.Serialize(returnValue89, returnDataPool);
				break;
			}
			case 135:
			{
				int argsCount135 = operation.ArgsCount;
				int num135 = argsCount135;
				if (num135 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int soulCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref soulCharId);
				int bodyCharId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bodyCharId);
				PossessionPreview returnValue90 = this.GetPossessionPreview(context, soulCharId, bodyCharId);
				result = Serializer.Serialize(returnValue90, returnDataPool);
				break;
			}
			case 136:
			{
				int argsCount136 = operation.ArgsCount;
				int num136 = argsCount136;
				if (num136 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int soulCharId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref soulCharId2);
				int bodyCharId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref bodyCharId2);
				AvatarExtraData avatarExtraData = new AvatarExtraData();
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref avatarExtraData);
				byte returnValue91 = this.TrySwapSoulCeremony(context, soulCharId2, bodyCharId2, avatarExtraData);
				result = Serializer.Serialize(returnValue91, returnDataPool);
				break;
			}
			case 137:
			{
				int argsCount137 = operation.ArgsCount;
				int num137 = argsCount137;
				if (num137 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey itemKey10 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey10);
				sbyte itemSource = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSource);
				this.GetBackTeaHorseCarryItem(context, itemKey10, itemSource);
				result = -1;
				break;
			}
			case 138:
			{
				int argsCount138 = operation.ArgsCount;
				int num138 = argsCount138;
				if (num138 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey itemKey11 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey11);
				sbyte itemSource2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSource2);
				this.AddItemToTeaHorseCarryItem(context, itemKey11, itemSource2);
				result = -1;
				break;
			}
			case 139:
			{
				int argsCount139 = operation.ArgsCount;
				int num139 = argsCount139;
				if (num139 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				AvatarData avatar = new AvatarData();
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref avatar);
				AvatarExtraData avatarExtraData2 = new AvatarExtraData();
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref avatarExtraData2);
				this.SetTemporaryPossessionCharacterAvatar(context, avatar, avatarExtraData2);
				result = -1;
				break;
			}
			case 140:
			{
				int argsCount140 = operation.ArgsCount;
				int num140 = argsCount140;
				if (num140 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> returnValue92 = this.GetSwapSoulCeremonyBodyCharIdList();
				result = Serializer.Serialize(returnValue92, returnDataPool);
				break;
			}
			case 141:
			{
				int argsCount141 = operation.ArgsCount;
				int num141 = argsCount141;
				if (num141 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey27 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey27);
				int[] managerCharacterIds = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref managerCharacterIds);
				int[] returnValue93 = this.GetBuildingShopManagerAutoArrangeSorted(blockKey27, managerCharacterIds);
				result = Serializer.Serialize(returnValue93, returnDataPool);
				break;
			}
			case 142:
			{
				int argsCount142 = operation.ArgsCount;
				int num142 = argsCount142;
				if (num142 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.SectMainStoryJingangClickMonkSoulBtn(context);
				result = -1;
				break;
			}
			case 143:
			{
				int argsCount143 = operation.ArgsCount;
				int num143 = argsCount143;
				if (num143 != 2)
				{
					if (num143 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					BuildingBlockKey key23 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key23);
					int earningDataIndex10 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref earningDataIndex10);
					bool isSetData5 = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isSetData5);
					this.RejectBuildingBlockRecruitPeople(context, key23, earningDataIndex10, isSetData5);
					result = -1;
				}
				else
				{
					BuildingBlockKey key24 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key24);
					int earningDataIndex11 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref earningDataIndex11);
					this.RejectBuildingBlockRecruitPeople(context, key24, earningDataIndex11, true);
					result = -1;
				}
				break;
			}
			case 144:
			{
				int argsCount144 = operation.ArgsCount;
				int num144 = argsCount144;
				if (num144 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey key25 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key25);
				this.RejectBuildingBlockRecruitPeopleQuick(context, key25);
				result = -1;
				break;
			}
			case 145:
			{
				int argsCount145 = operation.ArgsCount;
				int num145 = argsCount145;
				if (num145 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey28 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey28);
				BuildingManageYieldTipsData returnValue94 = this.GetShopManagementYieldTipsData(context, blockKey28);
				result = Serializer.Serialize(returnValue94, returnDataPool);
				break;
			}
			case 146:
			{
				int argsCount146 = operation.ArgsCount;
				int num146 = argsCount146;
				if (num146 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey29 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey29);
				int returnValue95 = this.CalculateBuildingManageHarvestSuccessRate(blockKey29);
				result = Serializer.Serialize(returnValue95, returnDataPool);
				break;
			}
			case 147:
			{
				int argsCount147 = operation.ArgsCount;
				int num147 = argsCount147;
				if (num147 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey buildingBlockKey19 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey19);
				ShopEventCollection returnValue96 = this.GetOrCreateShopEventCollection(buildingBlockKey19);
				result = Serializer.Serialize(returnValue96, returnDataPool);
				break;
			}
			case 148:
			{
				int argsCount148 = operation.ArgsCount;
				int num148 = argsCount148;
				if (num148 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				SamsaraPlatformRecordCollection returnValue97 = this.GetSamsaraPlatformRecord();
				result = Serializer.Serialize(returnValue97, returnDataPool);
				break;
			}
			case 149:
			{
				int argsCount149 = operation.ArgsCount;
				int num149 = argsCount149;
				if (num149 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<SamsaraPlatformCharDisplayData> returnValue98 = this.GetSwapSoulCeremonySoulCharIdList(context);
				result = Serializer.Serialize(returnValue98, returnDataPool);
				break;
			}
			case 150:
			{
				int argsCount150 = operation.ArgsCount;
				int num150 = argsCount150;
				if (num150 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.CricketCollectionBatchAddCricketJar(context);
				result = -1;
				break;
			}
			case 151:
			{
				int argsCount151 = operation.ArgsCount;
				int num151 = argsCount151;
				if (num151 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.CricketCollectionBatchAddCricket(context);
				result = -1;
				break;
			}
			case 152:
			{
				int argsCount152 = operation.ArgsCount;
				int num152 = argsCount152;
				if (num152 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemSourceType sourceType = ItemSourceType.Equipment;
				argsOffset += Serializer.DeserializeDefault<ItemSourceType>(argDataPool, argsOffset, ref sourceType);
				this.CricketCollectionBatchRemoveJar(context, sourceType);
				result = -1;
				break;
			}
			case 153:
			{
				int argsCount153 = operation.ArgsCount;
				int num153 = argsCount153;
				if (num153 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemSourceType sourceType2 = ItemSourceType.Equipment;
				argsOffset += Serializer.DeserializeDefault<ItemSourceType>(argDataPool, argsOffset, ref sourceType2);
				this.CricketCollectionBatchRemoveCricket(context, sourceType2);
				result = -1;
				break;
			}
			case 154:
			{
				int argsCount154 = operation.ArgsCount;
				int num154 = argsCount154;
				if (num154 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short itemSubType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSubType);
				ItemSourceType sourceType3 = ItemSourceType.Equipment;
				argsOffset += Serializer.DeserializeDefault<ItemSourceType>(argDataPool, argsOffset, ref sourceType3);
				List<ItemDisplayData> returnValue99 = this.GetCricketOrJarFromSourceStorage(context, itemSubType, sourceType3);
				result = Serializer.Serialize(returnValue99, returnDataPool);
				break;
			}
			case 155:
			{
				int argsCount155 = operation.ArgsCount;
				int num155 = argsCount155;
				if (num155 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int collectionIndex = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref collectionIndex);
				short itemSubType2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSubType2);
				ItemSourceType sourceType4 = ItemSourceType.Equipment;
				argsOffset += Serializer.DeserializeDefault<ItemSourceType>(argDataPool, argsOffset, ref sourceType4);
				ItemKey itemKey12 = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey12);
				this.SmartOperateCricketOrJarCollection(context, collectionIndex, itemSubType2, sourceType4, itemKey12);
				result = -1;
				break;
			}
			case 156:
			{
				int argsCount156 = operation.ArgsCount;
				int num156 = argsCount156;
				if (num156 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				CricketCollectionBatchButtonStateDisplayData returnValue100 = this.GetBatchButtonEnableState(context);
				result = Serializer.Serialize(returnValue100, returnDataPool);
				break;
			}
			case 157:
			{
				int argsCount157 = operation.ArgsCount;
				int num157 = argsCount157;
				if (num157 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey30 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey30);
				int[] returnValue101 = this.CalculateBuildingManageHarvestSuccessRates(blockKey30);
				result = Serializer.Serialize(returnValue101, returnDataPool);
				break;
			}
			case 158:
			{
				int argsCount158 = operation.ArgsCount;
				int num158 = argsCount158;
				if (num158 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey31 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey31);
				int[] workers4 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref workers4);
				ValueTuple<short, BuildingBlockData> returnValue102 = this.Collect(context, blockKey31, workers4);
				result = Serializer.Serialize(returnValue102, returnDataPool);
				break;
			}
			case 159:
			{
				int argsCount159 = operation.ArgsCount;
				int num159 = argsCount159;
				if (num159 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short orgMemberTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref orgMemberTemplateId);
				int chickenId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref chickenId);
				bool returnValue103 = this.UnsetFulongChicken(context, orgMemberTemplateId, chickenId);
				result = Serializer.Serialize(returnValue103, returnDataPool);
				break;
			}
			case 160:
			{
				int argsCount160 = operation.ArgsCount;
				int num160 = argsCount160;
				if (num160 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short orgMemberTemplateId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref orgMemberTemplateId2);
				int chickenId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref chickenId2);
				bool returnValue104 = this.SetFulongChicken(context, orgMemberTemplateId2, chickenId2);
				result = Serializer.Serialize(returnValue104, returnDataPool);
				break;
			}
			case 161:
			{
				int argsCount161 = operation.ArgsCount;
				int num161 = argsCount161;
				if (num161 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> idList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref idList);
				List<Chicken> returnValue105 = this.GetChickenDataList(idList);
				result = Serializer.Serialize(returnValue105, returnDataPool);
				break;
			}
			case 162:
			{
				int argsCount162 = operation.ArgsCount;
				int num162 = argsCount162;
				if (num162 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> chickenIdList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref chickenIdList);
				List<string> returnValue106 = this.GetChickenNicknameList(chickenIdList);
				result = Serializer.Serialize(returnValue106, returnDataPool);
				break;
			}
			case 163:
			{
				int argsCount163 = operation.ArgsCount;
				int num163 = argsCount163;
				if (num163 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location8 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location8);
				List<int> returnValue107 = this.GetSettlementChickenIdList(location8);
				result = Serializer.Serialize(returnValue107, returnDataPool);
				break;
			}
			case 164:
			{
				int argsCount164 = operation.ArgsCount;
				int num164 = argsCount164;
				if (num164 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location9 = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location9);
				List<string> returnValue108 = this.GetChickensNicknameByLocation(location9);
				result = Serializer.Serialize(returnValue108, returnDataPool);
				break;
			}
			case 165:
			{
				int argsCount165 = operation.ArgsCount;
				int num165 = argsCount165;
				if (num165 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue109 = this.AllChickenInTaiwuVillage(context);
				result = Serializer.Serialize(returnValue109, returnDataPool);
				break;
			}
			case 166:
			{
				int argsCount166 = operation.ArgsCount;
				int num166 = argsCount166;
				if (num166 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<bool> returnValue110 = this.GetVillagerRoleExtraEffectUnlockState();
				result = Serializer.Serialize(returnValue110, returnDataPool);
				break;
			}
			case 167:
			{
				int argsCount167 = operation.ArgsCount;
				int num167 = argsCount167;
				if (num167 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue111 = this.ClickChickenMap(context);
				result = Serializer.Serialize(returnValue111, returnDataPool);
				break;
			}
			case 168:
			{
				int argsCount168 = operation.ArgsCount;
				int num168 = argsCount168;
				if (num168 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int chickenId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref chickenId3);
				this.ClickChickenSign(context, chickenId3);
				result = -1;
				break;
			}
			case 169:
			{
				int argsCount169 = operation.ArgsCount;
				int num169 = argsCount169;
				if (num169 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue112 = this.IsInFulongSeekFeatherTask(context);
				result = Serializer.Serialize(returnValue112, returnDataPool);
				break;
			}
			case 170:
			{
				int argsCount170 = operation.ArgsCount;
				int num170 = argsCount170;
				if (num170 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int blockIndex17 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex17);
				BuildingResourceOutputSetting setting = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref setting);
				this.SetBuildingResourceOutputSetting(context, blockIndex17, setting);
				result = -1;
				break;
			}
			case 171:
			{
				int argsCount171 = operation.ArgsCount;
				int num171 = argsCount171;
				if (num171 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int blockIndex18 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockIndex18);
				BuildingResourceOutputSetting returnValue113 = this.GetBuildingResourceOutputSetting(blockIndex18);
				result = Serializer.Serialize(returnValue113, returnDataPool);
				break;
			}
			case 172:
			{
				int argsCount172 = operation.ArgsCount;
				int num172 = argsCount172;
				if (num172 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingExceptionData returnValue114 = this.GetBuildingExceptionData();
				result = Serializer.Serialize(returnValue114, returnDataPool);
				break;
			}
			case 173:
			{
				int argsCount173 = operation.ArgsCount;
				int num173 = argsCount173;
				if (num173 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey32 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey32);
				bool returnValue115 = this.AllDependBuildingAvailable(blockKey32);
				result = Serializer.Serialize(returnValue115, returnDataPool);
				break;
			}
			case 174:
			{
				int argsCount174 = operation.ArgsCount;
				int num174 = argsCount174;
				if (num174 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey33 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey33);
				short skillTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillTemplateId);
				int count = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count);
				int cost = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref cost);
				int returnValue116 = this.PracticingCombatSkillInPracticeRoom(context, blockKey33, skillTemplateId, count, cost);
				result = Serializer.Serialize(returnValue116, returnDataPool);
				break;
			}
			case 175:
			{
				int argsCount175 = operation.ArgsCount;
				int num175 = argsCount175;
				if (num175 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey34 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey34);
				bool returnValue117 = this.HasShopManagerLeader(blockKey34);
				result = Serializer.Serialize(returnValue117, returnDataPool);
				break;
			}
			case 176:
			{
				int argsCount176 = operation.ArgsCount;
				int num176 = argsCount176;
				if (num176 != 1)
				{
					if (num176 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					BuildingBlockKey blockKey35 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey35);
					bool onlyCheck = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref onlyCheck);
					List<int> returnValue118 = this.QuickArrangeShopManager(context, blockKey35, onlyCheck);
					result = Serializer.Serialize(returnValue118, returnDataPool);
				}
				else
				{
					BuildingBlockKey blockKey36 = default(BuildingBlockKey);
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey36);
					List<int> returnValue119 = this.QuickArrangeShopManager(context, blockKey36, false);
					result = Serializer.Serialize(returnValue119, returnDataPool);
				}
				break;
			}
			case 177:
			{
				int argsCount177 = operation.ArgsCount;
				int num177 = argsCount177;
				if (num177 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short buildingTemplateId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingTemplateId4);
				BuildingBlockKey blockKey37 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey37);
				sbyte operationType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref operationType);
				List<int> returnValue120 = this.QuickArrangeBuildOperator(context, buildingTemplateId4, blockKey37, operationType);
				result = Serializer.Serialize(returnValue120, returnDataPool);
				break;
			}
			case 178:
			{
				int argsCount178 = operation.ArgsCount;
				int num178 = argsCount178;
				if (num178 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey38 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey38);
				bool returnValue121 = this.ShopBuildingCanTeach(blockKey38);
				result = Serializer.Serialize(returnValue121, returnDataPool);
				break;
			}
			case 179:
			{
				int argsCount179 = operation.ArgsCount;
				int num179 = argsCount179;
				if (num179 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short buildingTemplateId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingTemplateId5);
				BuildingBlockKey blockKey39 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey39);
				sbyte operationType2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref operationType2);
				List<int> operatorList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref operatorList);
				int returnValue122 = this.GetOperationLeftTime(context, buildingTemplateId5, blockKey39, operationType2, operatorList);
				result = Serializer.Serialize(returnValue122, returnDataPool);
				break;
			}
			case 180:
			{
				int argsCount180 = operation.ArgsCount;
				int num180 = argsCount180;
				if (num180 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey40 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey40);
				sbyte operationType3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref operationType3);
				int returnValue123 = this.GetBuildingOperationLeftTime(context, blockKey40, operationType3);
				result = Serializer.Serialize(returnValue123, returnDataPool);
				break;
			}
			case 181:
			{
				int argsCount181 = operation.ArgsCount;
				int num181 = argsCount181;
				if (num181 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey41 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey41);
				int memberId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref memberId);
				ShopBuildingTeachBookData returnValue124 = this.GetShopBuildingTeachBookData(blockKey41, memberId);
				result = Serializer.Serialize(returnValue124, returnDataPool);
				break;
			}
			case 182:
			{
				int argsCount182 = operation.ArgsCount;
				int num182 = argsCount182;
				if (num182 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int returnValue125 = this.CalcExtraTaiwuGroupMaxCountByStrategyRoom();
				result = Serializer.Serialize(returnValue125, returnDataPool);
				break;
			}
			case 183:
			{
				int argsCount183 = operation.ArgsCount;
				int num183 = argsCount183;
				if (num183 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemSourceType itemSourceType3 = ItemSourceType.Equipment;
				argsOffset += Serializer.DeserializeDefault<ItemSourceType>(argDataPool, argsOffset, ref itemSourceType3);
				List<ItemDisplayData> returnValue126 = this.GetTaiwuCanFixBookItemDataList(itemSourceType3);
				result = Serializer.Serialize(returnValue126, returnDataPool);
				break;
			}
			case 184:
			{
				int argsCount184 = operation.ArgsCount;
				int num184 = argsCount184;
				if (num184 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ValueTuple<int, int, int> returnValue127 = this.GetResidenceInfo();
				result = Serializer.Serialize(returnValue127, returnDataPool);
				break;
			}
			case 185:
			{
				int argsCount185 = operation.ArgsCount;
				int num185 = argsCount185;
				if (num185 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				EBuildingScaleEffect effectType = EBuildingScaleEffect.MigrateSpeedBonusFactor;
				argsOffset += Serializer.DeserializeDefault<EBuildingScaleEffect>(argDataPool, argsOffset, ref effectType);
				int returnValue128 = this.GetTaiwuVillageResourceBlockEffect(context, effectType);
				result = Serializer.Serialize(returnValue128, returnDataPool);
				break;
			}
			case 186:
			{
				int argsCount186 = operation.ArgsCount;
				int num186 = argsCount186;
				if (num186 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				EBuildingScaleEffect effectType2 = EBuildingScaleEffect.MigrateSpeedBonusFactor;
				argsOffset += Serializer.DeserializeDefault<EBuildingScaleEffect>(argDataPool, argsOffset, ref effectType2);
				int returnValue129 = this.GetTaiwuLocationResourceBlockEffect(context, effectType2);
				result = Serializer.Serialize(returnValue129, returnDataPool);
				break;
			}
			case 187:
			{
				int argsCount187 = operation.ArgsCount;
				int num187 = argsCount187;
				if (num187 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId2);
				List<BuildingBlockData> returnValue130 = this.GetTaiwuVillageResourceBlockEffectInfo(context, templateId2);
				result = Serializer.Serialize(returnValue130, returnDataPool);
				break;
			}
			case 188:
			{
				int argsCount188 = operation.ArgsCount;
				int num188 = argsCount188;
				if (num188 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey42 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey42);
				bool returnValue131 = this.CanQuickArrangeShopManager(blockKey42);
				result = Serializer.Serialize(returnValue131, returnDataPool);
				break;
			}
			case 189:
			{
				int argsCount189 = operation.ArgsCount;
				int num189 = argsCount189;
				if (num189 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey43 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey43);
				BuildingFormulaContextBridge returnValue132 = this.GetBuildingFormulaContextBridge(blockKey43);
				result = Serializer.Serialize(returnValue132, returnDataPool);
				break;
			}
			case 190:
			{
				int argsCount190 = operation.ArgsCount;
				int num190 = argsCount190;
				if (num190 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey buildingBlockKey20 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey20);
				sbyte skillType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref skillType);
				ValueTuple<int, bool> returnValue133 = this.GetBuildingEffectForMake(buildingBlockKey20, skillType);
				result = Serializer.Serialize(returnValue133, returnDataPool);
				break;
			}
			case 191:
			{
				int argsCount191 = operation.ArgsCount;
				int num191 = argsCount191;
				if (num191 != 3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int totalAttainment = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref totalAttainment);
				short buildingTemplateId6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingTemplateId6);
				int repeat = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref repeat);
				bool returnValue134 = this.GmCmd_BuildingCollectPerform(context, totalAttainment, buildingTemplateId6, repeat);
				result = Serializer.Serialize(returnValue134, returnDataPool);
				break;
			}
			case 192:
			{
				int argsCount192 = operation.ArgsCount;
				int num192 = argsCount192;
				if (num192 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte grade = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref grade);
				int repeat2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref repeat2);
				bool returnValue135 = this.GmCmd_BeatMinionPerform(context, grade, repeat2);
				result = Serializer.Serialize(returnValue135, returnDataPool);
				break;
			}
			case 193:
			{
				int argsCount193 = operation.ArgsCount;
				int num193 = argsCount193;
				if (num193 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int type = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref type);
				int returnValue136 = this.GetStoreLocation(type);
				result = Serializer.Serialize(returnValue136, returnDataPool);
				break;
			}
			case 194:
			{
				int argsCount194 = operation.ArgsCount;
				int num194 = argsCount194;
				if (num194 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int type2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref type2);
				int value = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value);
				this.SetStoreLocation(context, type2, value);
				result = -1;
				break;
			}
			case 195:
			{
				int argsCount195 = operation.ArgsCount;
				int num195 = argsCount195;
				if (num195 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey buildingBlockKey21 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingBlockKey21);
				List<CharacterDisplayData> returnValue137 = this.GetFeastTargetCharList(context, buildingBlockKey21);
				result = Serializer.Serialize(returnValue137, returnDataPool);
				break;
			}
			case 196:
			{
				int argsCount196 = operation.ArgsCount;
				int num196 = argsCount196;
				if (num196 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.TryShowNotifications();
				result = -1;
				break;
			}
			case 197:
			{
				int argsCount197 = operation.ArgsCount;
				int num197 = argsCount197;
				if (num197 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey44 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey44);
				this.QuickRemoveShopSoldItem(context, blockKey44);
				result = -1;
				break;
			}
			case 198:
			{
				int argsCount198 = operation.ArgsCount;
				int num198 = argsCount198;
				if (num198 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				BuildingBlockKey blockKey45 = default(BuildingBlockKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockKey45);
				this.QuickAddShopSoldItem(context, blockKey45);
				result = -1;
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007E5D RID: 32349 RVA: 0x004BAA40 File Offset: 0x004B8C40
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			switch (dataId)
			{
			case 0:
				this._modificationsBuildingAreas.ChangeRecording(monitoring);
				break;
			case 1:
				this._modificationsBuildingBlocks.ChangeRecording(monitoring);
				break;
			case 2:
				break;
			case 3:
				this._modificationsCollectBuildingResourceType.ChangeRecording(monitoring);
				break;
			case 4:
				this._modificationsBuildingOperatorDict.ChangeRecording(monitoring);
				break;
			case 5:
				this._modificationsCustomBuildingName.ChangeRecording(monitoring);
				break;
			case 6:
				break;
			case 7:
				this._modificationsChickenBlessingInfoData.ChangeRecording(monitoring);
				break;
			case 8:
				this._modificationsChicken.ChangeRecording(monitoring);
				break;
			case 9:
				break;
			case 10:
				break;
			case 11:
				break;
			case 12:
				break;
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			case 16:
				break;
			case 17:
				break;
			case 18:
				break;
			case 19:
				break;
			case 20:
				this._modificationsSamsaraPlatformBornDict.ChangeRecording(monitoring);
				break;
			case 21:
				this._modificationsCollectBuildingEarningsData.ChangeRecording(monitoring);
				break;
			case 22:
				this._modificationsShopManagerDict.ChangeRecording(monitoring);
				break;
			case 23:
				break;
			case 24:
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06007E5E RID: 32350 RVA: 0x004BABC4 File Offset: 0x004B8DC4
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
					int offset = Serializer.SerializeModifications<Location>(this._buildingAreas, dataPool, this._modificationsBuildingAreas);
					this._modificationsBuildingAreas.Reset();
					result = offset;
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					int offset2 = Serializer.SerializeModifications<BuildingBlockKey>(this._buildingBlocks, dataPool, this._modificationsBuildingBlocks);
					this._modificationsBuildingBlocks.Reset();
					result = offset2;
				}
				break;
			}
			case 2:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
					result = Serializer.Serialize(this._taiwuBuildingAreas, dataPool);
				}
				break;
			}
			case 3:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (flag4)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					int offset3 = Serializer.SerializeModifications<BuildingBlockKey>(this._CollectBuildingResourceType, dataPool, this._modificationsCollectBuildingResourceType);
					this._modificationsCollectBuildingResourceType.Reset();
					result = offset3;
				}
				break;
			}
			case 4:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (flag5)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					int offset4 = Serializer.SerializeModifications<BuildingBlockKey>(this._buildingOperatorDict, dataPool, this._modificationsBuildingOperatorDict);
					this._modificationsBuildingOperatorDict.Reset();
					result = offset4;
				}
				break;
			}
			case 5:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (flag6)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					int offset5 = Serializer.SerializeModifications<BuildingBlockKey>(this._customBuildingName, dataPool, this._modificationsCustomBuildingName);
					this._modificationsCustomBuildingName.Reset();
					result = offset5;
				}
				break;
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (flag7)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					int offset6 = Serializer.SerializeModifications<int>(this._chickenBlessingInfoData, dataPool, this._modificationsChickenBlessingInfoData);
					this._modificationsChickenBlessingInfoData.Reset();
					result = offset6;
				}
				break;
			}
			case 8:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (flag8)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					int offset7 = Serializer.SerializeModifications<int>(this._chicken, dataPool, this._modificationsChicken);
					this._modificationsChicken.Reset();
					result = offset7;
				}
				break;
			}
			case 9:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 9);
				if (flag9)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
					result = Serializer.Serialize(this._collectionCrickets, dataPool);
				}
				break;
			}
			case 10:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 10);
				if (flag10)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
					result = Serializer.Serialize(this._collectionCricketJars, dataPool);
				}
				break;
			}
			case 11:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (flag11)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
					result = Serializer.Serialize(this._collectionCricketRegen, dataPool);
				}
				break;
			}
			case 12:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (flag12)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
					result = Serializer.Serialize(this._homeless, dataPool);
				}
				break;
			}
			case 16:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (flag13)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
					result = Serializer.Serialize(this._samsaraPlatformAddMainAttributes, dataPool);
				}
				break;
			}
			case 17:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 17);
				if (flag14)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
					result = Serializer.Serialize(this._samsaraPlatformAddCombatSkillQualifications, dataPool);
				}
				break;
			}
			case 18:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (flag15)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
					result = Serializer.Serialize(this._samsaraPlatformAddLifeSkillQualifications, dataPool);
				}
				break;
			}
			case 19:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this._dataStatesSamsaraPlatformSlots, (int)subId0);
				if (flag16)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this._dataStatesSamsaraPlatformSlots, (int)subId0);
					result = Serializer.Serialize(this._samsaraPlatformSlots[(int)subId0], dataPool);
				}
				break;
			}
			case 20:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (flag17)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
					int offset8 = Serializer.SerializeModifications<int>(this._samsaraPlatformBornDict, dataPool, this._modificationsSamsaraPlatformBornDict);
					this._modificationsSamsaraPlatformBornDict.Reset();
					result = offset8;
				}
				break;
			}
			case 21:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (flag18)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
					int offset9 = Serializer.SerializeModifications<BuildingBlockKey>(this._collectBuildingEarningsData, dataPool, this._modificationsCollectBuildingEarningsData);
					this._modificationsCollectBuildingEarningsData.Reset();
					result = offset9;
				}
				break;
			}
			case 22:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (flag19)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
					int offset10 = Serializer.SerializeModifications<BuildingBlockKey>(this._shopManagerDict, dataPool, this._modificationsShopManagerDict);
					this._modificationsShopManagerDict.Reset();
					result = offset10;
				}
				break;
			}
			case 23:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (flag20)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
					result = Serializer.Serialize(this._teaHorseCaravanData, dataPool);
				}
				break;
			}
			case 24:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (flag21)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
					result = Serializer.Serialize(this._shrineBuyTimes, dataPool);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007E5F RID: 32351 RVA: 0x004BB344 File Offset: 0x004B9544
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
					this._modificationsBuildingAreas.Reset();
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					this._modificationsBuildingBlocks.Reset();
				}
				break;
			}
			case 2:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (!flag3)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				break;
			}
			case 3:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (!flag4)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					this._modificationsCollectBuildingResourceType.Reset();
				}
				break;
			}
			case 4:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (!flag5)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					this._modificationsBuildingOperatorDict.Reset();
				}
				break;
			}
			case 5:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (!flag6)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					this._modificationsCustomBuildingName.Reset();
				}
				break;
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (!flag7)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					this._modificationsChickenBlessingInfoData.Reset();
				}
				break;
			}
			case 8:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (!flag8)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					this._modificationsChicken.Reset();
				}
				break;
			}
			case 9:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 9);
				if (!flag9)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 9);
				}
				break;
			}
			case 10:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 10);
				if (!flag10)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
				}
				break;
			}
			case 11:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (!flag11)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
				}
				break;
			}
			case 12:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (!flag12)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
				}
				break;
			}
			case 16:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (!flag13)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
				}
				break;
			}
			case 17:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 17);
				if (!flag14)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 17);
				}
				break;
			}
			case 18:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (!flag15)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
				}
				break;
			}
			case 19:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this._dataStatesSamsaraPlatformSlots, (int)subId0);
				if (!flag16)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesSamsaraPlatformSlots, (int)subId0);
				}
				break;
			}
			case 20:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (!flag17)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
					this._modificationsSamsaraPlatformBornDict.Reset();
				}
				break;
			}
			case 21:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (!flag18)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
					this._modificationsCollectBuildingEarningsData.Reset();
				}
				break;
			}
			case 22:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (!flag19)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
					this._modificationsShopManagerDict.Reset();
				}
				break;
			}
			case 23:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (!flag20)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
				}
				break;
			}
			case 24:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (!flag21)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06007E60 RID: 32352 RVA: 0x004BB8D8 File Offset: 0x004B9AD8
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
				result = BaseGameDataDomain.IsModified(this.DataStates, 0);
				break;
			case 1:
				result = BaseGameDataDomain.IsModified(this.DataStates, 1);
				break;
			case 2:
				result = BaseGameDataDomain.IsModified(this.DataStates, 2);
				break;
			case 3:
				result = BaseGameDataDomain.IsModified(this.DataStates, 3);
				break;
			case 4:
				result = BaseGameDataDomain.IsModified(this.DataStates, 4);
				break;
			case 5:
				result = BaseGameDataDomain.IsModified(this.DataStates, 5);
				break;
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
				result = BaseGameDataDomain.IsModified(this.DataStates, 7);
				break;
			case 8:
				result = BaseGameDataDomain.IsModified(this.DataStates, 8);
				break;
			case 9:
				result = BaseGameDataDomain.IsModified(this.DataStates, 9);
				break;
			case 10:
				result = BaseGameDataDomain.IsModified(this.DataStates, 10);
				break;
			case 11:
				result = BaseGameDataDomain.IsModified(this.DataStates, 11);
				break;
			case 12:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 13:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
				result = BaseGameDataDomain.IsModified(this.DataStates, 15);
				break;
			case 16:
				result = BaseGameDataDomain.IsModified(this.DataStates, 16);
				break;
			case 17:
				result = BaseGameDataDomain.IsModified(this.DataStates, 17);
				break;
			case 18:
				result = BaseGameDataDomain.IsModified(this.DataStates, 18);
				break;
			case 19:
				result = BaseGameDataDomain.IsModified(this._dataStatesSamsaraPlatformSlots, (int)subId0);
				break;
			case 20:
				result = BaseGameDataDomain.IsModified(this.DataStates, 20);
				break;
			case 21:
				result = BaseGameDataDomain.IsModified(this.DataStates, 21);
				break;
			case 22:
				result = BaseGameDataDomain.IsModified(this.DataStates, 22);
				break;
			case 23:
				result = BaseGameDataDomain.IsModified(this.DataStates, 23);
				break;
			case 24:
				result = BaseGameDataDomain.IsModified(this.DataStates, 24);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007E61 RID: 32353 RVA: 0x004BBBB0 File Offset: 0x004B9DB0
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (influence.TargetIndicator.DataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
				break;
			case 9:
				break;
			case 10:
				break;
			case 11:
				break;
			case 12:
				break;
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			case 16:
				break;
			case 17:
				break;
			case 18:
				break;
			case 19:
				break;
			case 20:
				break;
			case 21:
				break;
			case 22:
				break;
			case 23:
				break;
			case 24:
				break;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06007E62 RID: 32354 RVA: 0x004BBCD8 File Offset: 0x004B9ED8
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (operation.DataId)
			{
			case 0:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Ref<Location, BuildingAreaData>(operation, pResult, this._buildingAreas, 2);
				break;
			case 1:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Ref<BuildingBlockKey, BuildingBlockData>(operation, pResult, this._buildingBlocks, 16);
				break;
			case 2:
				ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_List<Location>(operation, pResult, this._taiwuBuildingAreas, 4);
				break;
			case 3:
				ResponseProcessor.ProcessSingleValueCollection_BasicType_Fixed_Value<BuildingBlockKey, sbyte>(operation, pResult, this._CollectBuildingResourceType);
				break;
			case 4:
				goto IL_2E7;
			case 5:
				ResponseProcessor.ProcessSingleValueCollection_BasicType_Fixed_Value<BuildingBlockKey, int>(operation, pResult, this._customBuildingName);
				break;
			case 6:
				ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_List<BuildingBlockKey>(operation, pResult, this._newCompleteOperationBuildings, 8);
				break;
			case 7:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<int, ChickenBlessingInfoData>(operation, pResult, this._chickenBlessingInfoData);
				break;
			case 8:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Value<int, Chicken>(operation, pResult, this._chicken, 12);
				break;
			case 9:
				ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Array<ItemKey>(operation, pResult, this._collectionCrickets, 15, 8);
				break;
			case 10:
				ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Array<ItemKey>(operation, pResult, this._collectionCricketJars, 15, 8);
				break;
			case 11:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array<int>(operation, pResult, this._collectionCricketRegen, 15);
				break;
			case 12:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<BuildingBlockKey, MakeItemData>(operation, pResult, this._makeItemDict);
				break;
			case 13:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<BuildingBlockKey, CharacterList>(operation, pResult, this._residences);
				break;
			case 14:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<BuildingBlockKey, CharacterList>(operation, pResult, this._comfortableHouses);
				break;
			case 15:
				ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Value_Single<CharacterList>(operation, pResult, ref this._homeless);
				break;
			case 16:
				ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<MainAttributes>(operation, pResult, ref this._samsaraPlatformAddMainAttributes);
				break;
			case 17:
				ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<CombatSkillShorts>(operation, pResult, ref this._samsaraPlatformAddCombatSkillQualifications);
				break;
			case 18:
				ResponseProcessor.ProcessSingleValue_CustomType_Fixed_Value_Single<LifeSkillShorts>(operation, pResult, ref this._samsaraPlatformAddLifeSkillQualifications);
				break;
			case 19:
				ResponseProcessor.ProcessElementList_CustomType_Fixed_Value<IntPair>(operation, pResult, this._samsaraPlatformSlots, 6, 8);
				break;
			case 20:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Fixed_Value<int, IntPair>(operation, pResult, this._samsaraPlatformBornDict, 8);
				break;
			case 21:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<BuildingBlockKey, BuildingEarningsData>(operation, pResult, this._collectBuildingEarningsData);
				break;
			case 22:
				goto IL_2E7;
			case 23:
				ResponseProcessor.ProcessSingleValue_CustomType_Dynamic_Ref_Single<TeaHorseCaravanData>(operation, pResult, this._teaHorseCaravanData);
				break;
			case 24:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<ushort>(operation, pResult, ref this._shrineBuyTimes);
				break;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag = this._pendingLoadingOperationIds != null;
			if (flag)
			{
				uint currPendingOperationId = this._pendingLoadingOperationIds.Peek();
				bool flag2 = currPendingOperationId == operation.Id;
				if (flag2)
				{
					this._pendingLoadingOperationIds.Dequeue();
					bool flag3 = this._pendingLoadingOperationIds.Count <= 0;
					if (flag3)
					{
						this._pendingLoadingOperationIds = null;
						this.InitializeInternalDataOfCollections();
						this.OnLoadedArchiveData();
						DomainManager.Global.CompleteLoading(9);
					}
				}
			}
			return;
			IL_2E7:
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06007E63 RID: 32355 RVA: 0x004BC000 File Offset: 0x004BA200
		private void InitializeInternalDataOfCollections()
		{
		}

		// Token: 0x06007E64 RID: 32356 RVA: 0x004BC004 File Offset: 0x004BA204
		[DomainMethod]
		[Obsolete]
		public List<ShopEventData> GetShopEventDataList(BuildingBlockKey blockKey)
		{
			bool flag = this._shopEventDict == null;
			if (flag)
			{
				this._shopEventDict = new Dictionary<BuildingBlockKey, List<ShopEventData>>();
			}
			List<ShopEventData> shopEventDataList;
			bool flag2 = this._shopEventDict.TryGetValue(blockKey, out shopEventDataList);
			List<ShopEventData> result;
			if (flag2)
			{
				result = shopEventDataList;
			}
			else
			{
				List<ShopEventData> data = new List<ShopEventData>();
				this._shopEventDict.Add(blockKey, data);
				result = data;
			}
			return result;
		}

		// Token: 0x06007E66 RID: 32358 RVA: 0x004BC080 File Offset: 0x004BA280
		[CompilerGenerated]
		internal static int <PlaceBuildingAtRandomBlock>g__GetDisToCenter|29_3(short blockIndex, sbyte width)
		{
			int x = (int)(blockIndex % (short)width);
			int y = (int)(blockIndex / (short)width);
			int center = (int)(width / 2 - 1);
			return Math.Abs(x - center) + Math.Abs(y - center);
		}

		// Token: 0x06007E67 RID: 32359 RVA: 0x004BC0B4 File Offset: 0x004BA2B4
		[CompilerGenerated]
		internal static int <PlaceBuildingAtBlock>g__GetDisToCenter|30_3(short blockIndex, sbyte width)
		{
			int x = (int)(blockIndex % (short)width);
			int y = (int)(blockIndex / (short)width);
			int center = (int)(width / 2 - 1);
			return Math.Abs(x - center) + Math.Abs(y - center);
		}

		// Token: 0x06007E68 RID: 32360 RVA: 0x004BC0E8 File Offset: 0x004BA2E8
		[CompilerGenerated]
		private void <UpdateResourceBlock>g__UpdateShopProgressBlock|108_0(short settlementId, BuildingBlockData blockData)
		{
			bool flag = !this._alreadyUpdateShopProgressBlock.ContainsKey(settlementId);
			if (flag)
			{
				this._alreadyUpdateShopProgressBlock.Add(settlementId, new List<BuildingBlockData>
				{
					blockData
				});
			}
			else
			{
				bool flag2 = !this._alreadyUpdateShopProgressBlock[settlementId].Contains(blockData);
				if (flag2)
				{
					this._alreadyUpdateShopProgressBlock[settlementId].Add(blockData);
				}
			}
		}

		// Token: 0x06007E69 RID: 32361 RVA: 0x004BC158 File Offset: 0x004BA358
		[CompilerGenerated]
		private void <OfflineUpdateShopManagement>g__SellItemSuccess|117_0(ShopEventItem successShopEventConfig, BuildingEarningsData data, int index, ref BuildingDomain.<>c__DisplayClass117_0 A_4)
		{
			sbyte resourceType = successShopEventConfig.ExchangeResourceGoods;
			int num;
			BuildingProduceDependencyData buildingProduceDependencyData;
			int soldValue = this.CalcSoldItemValue(A_4.random, A_4.blockKey, A_4.blockData, data.ShopSoldItemList[index], out num, out buildingProduceDependencyData);
			int soldPrice = soldValue / (int)GlobalConfig.ResourcesWorth[(int)resourceType];
			ParallelBuildingModification modification = A_4.modification;
			if (modification.ShopSoldItems == null)
			{
				modification.ShopSoldItems = new List<ValueTuple<sbyte, IntPair>>();
			}
			A_4.modification.ShopSoldItems.Add(new ValueTuple<sbyte, IntPair>((sbyte)index, new IntPair((int)resourceType, soldPrice)));
			ItemKey soldItem = data.ShopSoldItemList[index];
			A_4.shopEventCollection.AddSellItemSuccessRecord(successShopEventConfig, A_4.date, soldItem.ItemType, soldItem.TemplateId, resourceType, soldPrice);
			A_4.monthlyNotifications.AddBuildingIncome(A_4.settlementId, A_4.buildingBlockCfg.TemplateId);
		}

		// Token: 0x06007E6A RID: 32362 RVA: 0x004BC234 File Offset: 0x004BA434
		[CompilerGenerated]
		internal static sbyte <ShopManagerSelectCombatSkillToLearn>g__GetSectPriority|119_0(sbyte sectId, ref BuildingDomain.<>c__DisplayClass119_0 A_1)
		{
			bool flag = sectId == A_1.stateSectId;
			sbyte result;
			if (flag)
			{
				result = 3;
			}
			else
			{
				bool flag2 = sectId == A_1.idealSectId;
				if (flag2)
				{
					result = 2;
				}
				else
				{
					short mainMorality = Organization.Instance[sectId].MainMorality;
					bool flag3 = mainMorality >= -500 && mainMorality <= 500 && GameData.Domains.Character.BehaviorType.GetBehaviorType(mainMorality) == A_1.behaviorType;
					if (flag3)
					{
						result = 1;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x06007E6B RID: 32363 RVA: 0x004BC2A8 File Offset: 0x004BA4A8
		[CompilerGenerated]
		internal static int <CricketCollectionBatchAddCricket>g__Compare|254_0(BuildingDomain.ItemWithSource a, BuildingDomain.ItemWithSource b)
		{
			GameData.Domains.Item.Cricket cricketA = DomainManager.Item.GetElement_Crickets(a.ItemKey.Id);
			GameData.Domains.Item.Cricket cricketB = DomainManager.Item.GetElement_Crickets(b.ItemKey.Id);
			bool flag = cricketA == null || cricketB == null;
			if (flag)
			{
				bool flag2 = cricketA == null && cricketB == null;
				if (flag2)
				{
					return 0;
				}
				bool flag3 = cricketA == null;
				if (flag3)
				{
					return -1;
				}
				bool flag4 = cricketB == null;
				if (flag4)
				{
					return 1;
				}
			}
			short durabilityNumberA = cricketA.GetCurrDurability();
			short durabilityNumberB = cricketB.GetCurrDurability();
			float durabilityA = (float)durabilityNumberA / (float)cricketA.GetMaxDurability();
			float durabilityB = (float)durabilityNumberB / (float)cricketB.GetMaxDurability();
			bool flag5 = durabilityNumberA == 0 && durabilityNumberB > 0;
			int result;
			if (flag5)
			{
				result = 1;
			}
			else
			{
				bool flag6 = durabilityNumberB == 0 && durabilityNumberA > 0;
				if (flag6)
				{
					result = -1;
				}
				else
				{
					int durabilityComparison = durabilityA.CompareTo(durabilityB);
					bool flag7 = durabilityComparison != 0;
					if (flag7)
					{
						result = durabilityComparison;
					}
					else
					{
						sbyte gradeA = cricketA.GetGrade();
						int gradeComparison = cricketB.GetGrade().CompareTo(gradeA);
						bool flag8 = gradeComparison != 0;
						if (flag8)
						{
							result = gradeComparison;
						}
						else
						{
							result = a.ItemKey.Id.CompareTo(b.ItemKey.Id);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007E6C RID: 32364 RVA: 0x004BC3EC File Offset: 0x004BA5EC
		[CompilerGenerated]
		internal static int <CricketCollectionBatchAddCricketJar>g__Compare|258_0(BuildingDomain.ItemWithSource a, BuildingDomain.ItemWithSource b)
		{
			GameData.Domains.Item.Misc cricketJarA = DomainManager.Item.GetElement_Misc(a.ItemKey.Id);
			GameData.Domains.Item.Misc cricketJarB = DomainManager.Item.GetElement_Misc(b.ItemKey.Id);
			sbyte gradeA = cricketJarA.GetGrade();
			int gradeComparison = cricketJarB.GetGrade().CompareTo(gradeA);
			bool flag = gradeComparison != 0;
			int result;
			if (flag)
			{
				result = gradeComparison;
			}
			else
			{
				result = a.ItemKey.Id.CompareTo(b.ItemKey.Id);
			}
			return result;
		}

		// Token: 0x06007E6D RID: 32365 RVA: 0x004BC474 File Offset: 0x004BA674
		[CompilerGenerated]
		internal static int <UpdateResidentsHappinessAndFavor>g__ClampDelta|393_0(int curValue, int delta)
		{
			bool flag = curValue <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = ((curValue + delta < 0) ? (-curValue) : delta);
			}
			return result;
		}

		// Token: 0x06007E6E RID: 32366 RVA: 0x004BC4A0 File Offset: 0x004BA6A0
		[CompilerGenerated]
		internal static void <UpdateResidentsHappinessAndFavor>g__UpdateHomelessHappinessAndFavor|393_1(int charId, ref BuildingDomain.<>c__DisplayClass393_0 A_1)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			short favorability = DomainManager.Character.GetFavorability(charId, A_1.taiwuCharId);
			bool flag = favorability > 0;
			if (flag)
			{
				int delta = BuildingDomain.<UpdateResidentsHappinessAndFavor>g__ClampDelta|393_0((int)favorability, (int)GlobalConfig.Instance.HomelessFavorabilityChangePerMonth);
				bool flag2 = (int)favorability + delta < 0;
				if (flag2)
				{
					delta = (int)(-(int)favorability);
				}
				bool flag3 = favorability > 0;
				if (flag3)
				{
					DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(A_1.context, character, A_1.taiwu, delta);
				}
			}
			sbyte happiness = character.GetHappiness();
			bool flag4 = happiness > 0;
			if (flag4)
			{
				int delta2 = BuildingDomain.<UpdateResidentsHappinessAndFavor>g__ClampDelta|393_0((int)happiness, (int)GlobalConfig.Instance.HomelessHappinessChangePerMonth);
				character.ChangeHappiness(A_1.context, delta2);
			}
		}

		// Token: 0x06007E6F RID: 32367 RVA: 0x004BC551 File Offset: 0x004BA751
		[CompilerGenerated]
		internal static int <CalcResourceBlockEffectBaseValues>g__GetPercentage|411_0(int index)
		{
			return 100 * (5 - index) / 5;
		}

		// Token: 0x06007E70 RID: 32368 RVA: 0x004BC55C File Offset: 0x004BA75C
		[CompilerGenerated]
		private List<int> <QuickArrangeBuildOperator>g__FindMinSumCombination|458_1(List<int> list, int threshold, ref List<int> result)
		{
			bool flag = list == null || list.Count == 0;
			List<int> result2;
			if (flag)
			{
				result2 = result;
			}
			else
			{
				int i = list.Count;
				int minSum = int.MaxValue;
				for (int j = 0; j < i; j++)
				{
					int charId = list[j];
					result[0] = charId;
					int workValue = this.<QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(charId);
					bool flag2 = workValue > threshold;
					if (flag2)
					{
						bool flag3 = workValue < minSum;
						if (flag3)
						{
							minSum = workValue;
							result[0] = charId;
						}
					}
				}
				for (int k = 0; k < i - 1; k++)
				{
					bool flag4 = this.<QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(list[k]) + this.<QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(list[k + 1]) > minSum;
					if (!flag4)
					{
						result[0] = list[k];
						result[1] = list[k + 1];
						for (int l = k + 1; l < i; l++)
						{
							int sum = this.<QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(list[k]) + this.<QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(list[l]);
							bool flag5 = sum > minSum;
							if (flag5)
							{
								break;
							}
							bool flag6 = sum > threshold;
							if (flag6)
							{
								minSum = sum;
								result[0] = list[k];
								result[1] = list[l];
							}
						}
					}
				}
				for (int m = 0; m < i - 2; m++)
				{
					bool flag7 = this.<QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(list[m]) + this.<QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(list[m + 1]) + this.<QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(list[m + 2]) > minSum;
					if (flag7)
					{
						break;
					}
					result[0] = list[m];
					result[1] = list[m + 1];
					result[2] = list[m + 2];
					int left = m + 1;
					int right = i - 1;
					while (left < right)
					{
						int sum2 = this.<QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(list[m]) + this.<QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(list[left]) + this.<QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(list[right]);
						bool flag8 = sum2 <= threshold;
						if (flag8)
						{
							left++;
						}
						else
						{
							bool flag9 = sum2 < minSum;
							if (flag9)
							{
								minSum = sum2;
								result[0] = list[m];
								result[1] = list[left];
								result[2] = list[right];
							}
							right--;
						}
					}
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06007E71 RID: 32369 RVA: 0x004BC828 File Offset: 0x004BAA28
		[CompilerGenerated]
		private int <QuickArrangeBuildOperator>g__GetCharacterWorkValueSum|458_2(int charId)
		{
			bool flag = charId < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				result = character.GetPersonalities().GetSum() + (int)this.BaseWorkContribution;
			}
			return result;
		}

		// Token: 0x06007E72 RID: 32370 RVA: 0x004BC868 File Offset: 0x004BAA68
		[CompilerGenerated]
		internal static void <AutoAddShopSoldItem>g__InitBuildingEarningsData|492_1(sbyte level, ref BuildingEarningsData earningsData)
		{
			earningsData = new BuildingEarningsData();
			for (int i = 0; i < (int)level; i++)
			{
				earningsData.ShopSoldItemList.Add(ItemKey.Invalid);
				earningsData.ShopSoldItemEarnList.Add(new IntPair(-1, -1));
			}
		}

		// Token: 0x04002253 RID: 8787
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002254 RID: 8788
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private Dictionary<Location, BuildingAreaData> _buildingAreas;

		// Token: 0x04002255 RID: 8789
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private Dictionary<BuildingBlockKey, BuildingBlockData> _buildingBlocks;

		// Token: 0x04002256 RID: 8790
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private List<Location> _taiwuBuildingAreas;

		// Token: 0x04002257 RID: 8791
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
		private Dictionary<BuildingBlockKey, sbyte> _CollectBuildingResourceType;

		// Token: 0x04002258 RID: 8792
		[DomainData(DomainDataType.SingleValueCollection, false, false, true, false)]
		private Dictionary<BuildingBlockKey, CharacterList> _buildingOperatorDict;

		// Token: 0x04002259 RID: 8793
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private Dictionary<BuildingBlockKey, int> _customBuildingName;

		// Token: 0x0400225A RID: 8794
		private bool _needUpdateEffects;

		// Token: 0x0400225B RID: 8795
		public const short DependMaxDistance = 2;

		// Token: 0x0400225C RID: 8796
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private List<BuildingBlockKey> _newCompleteOperationBuildings;

		// Token: 0x0400225D RID: 8797
		private List<BuildingBlockKey> _newBrokenBuildings = new List<BuildingBlockKey>();

		// Token: 0x0400225E RID: 8798
		private Dictionary<short, List<BuildingBlockData>> _alreadyUpdateShopProgressBlock = new Dictionary<short, List<BuildingBlockData>>();

		// Token: 0x0400225F RID: 8799
		public readonly short BaseWorkContribution = 250;

		// Token: 0x04002260 RID: 8800
		public readonly short AttainmentToProb = 20;

		// Token: 0x04002261 RID: 8801
		private readonly BuildingExceptionData _buildingExceptionData = new BuildingExceptionData();

		// Token: 0x04002262 RID: 8802
		private readonly HashSet<BuildingBlockKey> _buildingAutoExpandStoppedNotifiedSet = new HashSet<BuildingBlockKey>();

		// Token: 0x04002263 RID: 8803
		private readonly BuildingFormulaContextBridge _formulaContextBridge = new BuildingFormulaContextBridge();

		// Token: 0x04002264 RID: 8804
		private readonly Dictionary<Location, IBuildingEffectValue[]> _buildingBlockEffectsCache = new Dictionary<Location, IBuildingEffectValue[]>();

		// Token: 0x04002265 RID: 8805
		private readonly BuildingFormulaContextBridge.CalcArgument _formulaArgHandler = new BuildingFormulaContextBridge.CalcArgument(BuildingDomain.CalcBuildingFormulaContextArg);

		// Token: 0x04002266 RID: 8806
		[Obsolete]
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private Dictionary<int, ChickenBlessingInfoData> _chickenBlessingInfoData;

		// Token: 0x04002267 RID: 8807
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private Dictionary<int, Chicken> _chicken;

		// Token: 0x04002268 RID: 8808
		private int _nextChickenId;

		// Token: 0x04002269 RID: 8809
		public List<short> ChickenMapInfo = null;

		// Token: 0x0400226A RID: 8810
		private readonly Dictionary<int, List<int>> _settlementChickenIdLists = new Dictionary<int, List<int>>();

		// Token: 0x0400226B RID: 8811
		[Obsolete("Use GameData.Domains.Extra.CricketCollectionData.CricketCollectionCapacity instead")]
		public const int CricketCollectionCapacity = 15;

		// Token: 0x0400226C RID: 8812
		[Obsolete("Use GameData.Domains.Extra.CricketCollectionData instead")]
		[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 15)]
		private ItemKey[] _collectionCrickets;

		// Token: 0x0400226D RID: 8813
		[Obsolete("Use GameData.Domains.Extra.CricketCollectionData instead")]
		[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 15)]
		private ItemKey[] _collectionCricketJars;

		// Token: 0x0400226E RID: 8814
		[Obsolete("Use GameData.Domains.Extra.CricketCollectionData instead")]
		[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 15)]
		private int[] _collectionCricketRegen;

		// Token: 0x0400226F RID: 8815
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private Dictionary<BuildingBlockKey, MakeItemData> _makeItemDict;

		// Token: 0x04002270 RID: 8816
		private bool _outsideMakeItem = false;

		// Token: 0x04002271 RID: 8817
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private readonly Dictionary<BuildingBlockKey, CharacterList> _residences;

		// Token: 0x04002272 RID: 8818
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private readonly Dictionary<BuildingBlockKey, CharacterList> _comfortableHouses;

		// Token: 0x04002273 RID: 8819
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private CharacterList _homeless;

		// Token: 0x04002274 RID: 8820
		private Dictionary<int, BuildingBlockKey> _buildingResidents;

		// Token: 0x04002275 RID: 8821
		private Dictionary<int, BuildingBlockKey> _buildingComfortableHouses;

		// Token: 0x04002276 RID: 8822
		private Dictionary<int, ValueTuple<short, int, ItemKey>> _feastParticipants = new Dictionary<int, ValueTuple<short, int, ItemKey>>();

		// Token: 0x04002277 RID: 8823
		private const int ResourceBlockBaseValueCount = 5;

		// Token: 0x04002278 RID: 8824
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private MainAttributes _samsaraPlatformAddMainAttributes;

		// Token: 0x04002279 RID: 8825
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private CombatSkillShorts _samsaraPlatformAddCombatSkillQualifications;

		// Token: 0x0400227A RID: 8826
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private LifeSkillShorts _samsaraPlatformAddLifeSkillQualifications;

		// Token: 0x0400227B RID: 8827
		[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 6)]
		private readonly IntPair[] _samsaraPlatformSlots;

		// Token: 0x0400227C RID: 8828
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
		private readonly Dictionary<int, IntPair> _samsaraPlatformBornDict;

		// Token: 0x0400227D RID: 8829
		private int _temporaryPossessionCharId = -1;

		// Token: 0x0400227E RID: 8830
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, false)]
		private Dictionary<BuildingBlockKey, BuildingEarningsData> _collectBuildingEarningsData;

		// Token: 0x0400227F RID: 8831
		[DomainData(DomainDataType.SingleValueCollection, false, false, true, false)]
		private Dictionary<BuildingBlockKey, CharacterList> _shopManagerDict;

		// Token: 0x04002280 RID: 8832
		[DomainData(DomainDataType.SingleValue, true, false, true, false)]
		private TeaHorseCaravanData _teaHorseCaravanData;

		// Token: 0x04002281 RID: 8833
		private Dictionary<BuildingBlockKey, ShopEventCollection> _shopEventCollections;

		// Token: 0x04002282 RID: 8834
		public readonly short _caravanReplenishmentInitValue = 100;

		// Token: 0x04002283 RID: 8835
		public readonly short _caravanAwarenessInitValue = 100;

		// Token: 0x04002284 RID: 8836
		private readonly short _caravanReplenishmentCostPerMonth = 5;

		// Token: 0x04002285 RID: 8837
		private List<short> _westTreasureTemplateId = new List<short>
		{
			28,
			29,
			30,
			31,
			32
		};

		// Token: 0x04002286 RID: 8838
		private readonly short[][] WestItemArrayTwo = new short[][]
		{
			new short[]
			{
				28,
				29,
				30,
				31,
				32
			},
			new short[]
			{
				33,
				34,
				35,
				36,
				37
			},
			new short[]
			{
				38,
				39,
				40,
				41,
				42
			},
			new short[]
			{
				43,
				44,
				45,
				46,
				47
			},
			new short[]
			{
				48,
				49,
				50,
				51,
				52
			},
			new short[]
			{
				53,
				54,
				55,
				56,
				57
			},
			new short[]
			{
				58,
				59,
				60,
				61,
				62
			},
			new short[]
			{
				63,
				64,
				65,
				66,
				67
			},
			new short[]
			{
				68,
				69,
				70,
				71,
				72
			}
		};

		// Token: 0x04002287 RID: 8839
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private ushort _shrineBuyTimes;

		// Token: 0x04002288 RID: 8840
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[25][];

		// Token: 0x04002289 RID: 8841
		private SingleValueCollectionModificationCollection<Location> _modificationsBuildingAreas = SingleValueCollectionModificationCollection<Location>.Create();

		// Token: 0x0400228A RID: 8842
		private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsBuildingBlocks = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

		// Token: 0x0400228B RID: 8843
		private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsCollectBuildingResourceType = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

		// Token: 0x0400228C RID: 8844
		private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsBuildingOperatorDict = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

		// Token: 0x0400228D RID: 8845
		private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsCustomBuildingName = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

		// Token: 0x0400228E RID: 8846
		private SingleValueCollectionModificationCollection<int> _modificationsChickenBlessingInfoData = SingleValueCollectionModificationCollection<int>.Create();

		// Token: 0x0400228F RID: 8847
		private SingleValueCollectionModificationCollection<int> _modificationsChicken = SingleValueCollectionModificationCollection<int>.Create();

		// Token: 0x04002290 RID: 8848
		private static readonly DataInfluence[][] CacheInfluencesSamsaraPlatformSlots = new DataInfluence[6][];

		// Token: 0x04002291 RID: 8849
		private readonly byte[] _dataStatesSamsaraPlatformSlots = new byte[2];

		// Token: 0x04002292 RID: 8850
		private SingleValueCollectionModificationCollection<int> _modificationsSamsaraPlatformBornDict = SingleValueCollectionModificationCollection<int>.Create();

		// Token: 0x04002293 RID: 8851
		private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsCollectBuildingEarningsData = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

		// Token: 0x04002294 RID: 8852
		private SingleValueCollectionModificationCollection<BuildingBlockKey> _modificationsShopManagerDict = SingleValueCollectionModificationCollection<BuildingBlockKey>.Create();

		// Token: 0x04002295 RID: 8853
		private Queue<uint> _pendingLoadingOperationIds;

		// Token: 0x04002296 RID: 8854
		[Obsolete]
		private Dictionary<BuildingBlockKey, List<ShopEventData>> _shopEventDict;

		// Token: 0x02000C76 RID: 3190
		private struct ItemWithSource
		{
			// Token: 0x04003638 RID: 13880
			public ItemKey ItemKey;

			// Token: 0x04003639 RID: 13881
			public int Amount;

			// Token: 0x0400363A RID: 13882
			public int Source;

			// Token: 0x0400363B RID: 13883
			public int IndexInSource;
		}

		// Token: 0x02000C77 RID: 3191
		public enum MultiSelectOperateType
		{
			// Token: 0x0400363D RID: 13885
			InventoryToSold = 1,
			// Token: 0x0400363E RID: 13886
			SoldToInventory,
			// Token: 0x0400363F RID: 13887
			WarehouseToSold,
			// Token: 0x04003640 RID: 13888
			SoldToWarehouse,
			// Token: 0x04003641 RID: 13889
			TreasuryToSold,
			// Token: 0x04003642 RID: 13890
			SoldToTreasury,
			// Token: 0x04003643 RID: 13891
			StockStorageGoodsShelfToSold,
			// Token: 0x04003644 RID: 13892
			SoldToStockStorageGoodsShelf
		}

		// Token: 0x02000C78 RID: 3192
		public class TeaHorseCaravanState
		{
			// Token: 0x04003645 RID: 13893
			public const sbyte None = 0;

			// Token: 0x04003646 RID: 13894
			public const sbyte Ready = 1;

			// Token: 0x04003647 RID: 13895
			public const sbyte Forward = 2;

			// Token: 0x04003648 RID: 13896
			public const sbyte Return = 3;

			// Token: 0x04003649 RID: 13897
			public const sbyte ReadyGetItem = 4;
		}

		// Token: 0x02000C79 RID: 3193
		public enum SaveInfectedType
		{
			// Token: 0x0400364B RID: 13899
			Save = 1,
			// Token: 0x0400364C RID: 13900
			Release,
			// Token: 0x0400364D RID: 13901
			Expel,
			// Token: 0x0400364E RID: 13902
			Kill
		}
	}
}
