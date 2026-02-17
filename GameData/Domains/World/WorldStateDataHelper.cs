using System;
using System.Collections.Generic;
using Config;
using GameData.DLC.FiveLoong;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Utilities;

namespace GameData.Domains.World
{
	// Token: 0x02000030 RID: 48
	public static class WorldStateDataHelper
	{
		// Token: 0x06000ECD RID: 3789 RVA: 0x000F859C File Offset: 0x000F679C
		public static void DetectWarehouseOverload(this WorldStateData data)
		{
			int warehouseCurrLoad = DomainManager.Taiwu.GetWarehouseCurrLoad();
			int warehouseMaxLoad = DomainManager.Taiwu.GetWarehouseMaxLoad();
			bool flag = warehouseCurrLoad > warehouseMaxLoad;
			if (flag)
			{
				data.SetWorldState(9);
				data.AddOverloadStorageType(WorldStateData.EStorageType.Warehouse);
			}
			int troughCurrLoad = DomainManager.Taiwu.GetTroughCurrLoad();
			int troughMaxLoad = DomainManager.Taiwu.GetTroughMaxLoad();
			bool flag2 = troughCurrLoad > troughMaxLoad;
			if (flag2)
			{
				data.SetWorldState(9);
				data.AddOverloadStorageType(WorldStateData.EStorageType.Trough);
			}
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x000F8610 File Offset: 0x000F6810
		public unsafe static void DetectResourceOverload(this WorldStateData data)
		{
			int maxAmount = DomainManager.Taiwu.GetMaterialResourceMaxCount();
			ResourceInts resources = *DomainManager.Taiwu.GetTotalResources();
			for (sbyte resourceType = 0; resourceType < 6; resourceType += 1)
			{
				bool flag = *(ref resources.Items.FixedElementField + (IntPtr)resourceType * 4) > maxAmount;
				if (flag)
				{
					data.SetWorldState(10);
					data.AddOverloadingResourceType(resourceType);
				}
			}
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x000F8678 File Offset: 0x000F6878
		public static void DetectInventoryOverload(this WorldStateData data)
		{
			List<IntPair> overloadChars = DomainManager.Taiwu.GetOverweightSanctionPercent();
			bool flag = overloadChars.Count > 0;
			if (flag)
			{
				data.SetWorldState(8);
			}
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x000F86A8 File Offset: 0x000F68A8
		public static void DetectInjuries(this WorldStateData data)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Injuries injuries = taiwu.GetInjuries();
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				ValueTuple<sbyte, sbyte> valueTuple = injuries.Get(bodyPart);
				sbyte outer = valueTuple.Item1;
				sbyte inner = valueTuple.Item2;
				bool flag = outer >= 2;
				if (flag)
				{
					data.AddOuterInjuryBodyPart(bodyPart);
				}
				bool flag2 = inner >= 2;
				if (flag2)
				{
					data.AddInnerInjuryBodyPart(bodyPart);
				}
			}
			bool flag3 = data.AnyOuterInjury();
			if (flag3)
			{
				data.SetWorldState(11);
			}
			bool flag4 = data.AnyInnerInjury();
			if (flag4)
			{
				data.SetWorldState(12);
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x000F8748 File Offset: 0x000F6948
		public unsafe static void DetectPoisons(this WorldStateData data)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			PoisonInts poisoned = *taiwu.GetPoisoned();
			for (sbyte poisonType = 0; poisonType < 6; poisonType += 1)
			{
				sbyte level = PoisonsAndLevels.CalcPoisonedLevel(*(ref poisoned.Items.FixedElementField + (IntPtr)poisonType * 4));
				bool flag = level <= 0;
				if (!flag)
				{
					data.SetWorldState(14);
					data.AddPoisonType(poisonType);
				}
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x000F87BC File Offset: 0x000F69BC
		public static void DetectDisorderOfQi(this WorldStateData data)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			short disorderOfQi = taiwu.GetDisorderOfQi();
			sbyte disorderOfQiLevel = DisorderLevelOfQi.GetDisorderLevelOfQi(disorderOfQi);
			bool flag = disorderOfQiLevel != 0;
			if (flag)
			{
				data.SetWorldState(13);
			}
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x000F87F4 File Offset: 0x000F69F4
		public static void DetectTeammateInjuries(this WorldStateData data)
		{
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			HashSet<int> ids = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int id in ids)
			{
				bool flag = id == taiwuId;
				if (!flag)
				{
					Injuries injuries = DomainManager.Character.GetElement_Objects(id).GetInjuries();
					for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
					{
						ValueTuple<sbyte, sbyte> valueTuple = injuries.Get(bodyPart);
						sbyte outer = valueTuple.Item1;
						sbyte inner = valueTuple.Item2;
						bool flag2 = outer >= 2 || inner >= 2;
						if (flag2)
						{
							data.SetWorldState(37);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x000F88D8 File Offset: 0x000F6AD8
		public static void DetectMainStory(this WorldStateData data)
		{
			bool exorcismEnabled = DomainManager.Extra.GetExorcismEnabled();
			if (exorcismEnabled)
			{
				data.SetWorldState(50);
			}
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x000F8900 File Offset: 0x000F6B00
		public static void DetectXiangshuAvatars(this WorldStateData data)
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
			AreaAdventureData areaAdventures = DomainManager.Adventure.GetAdventuresInArea(taiwuVillageLocation.AreaId);
			foreach (KeyValuePair<short, AdventureSiteData> keyValuePair in areaAdventures.AdventureSites)
			{
				short num;
				AdventureSiteData adventureSiteData2;
				keyValuePair.Deconstruct(out num, out adventureSiteData2);
				AdventureSiteData adventureSiteData = adventureSiteData2;
				bool flag = adventureSiteData.SiteState != 1;
				if (!flag)
				{
					sbyte xiangshuAvatarId = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureSiteData.TemplateId);
					bool flag2 = xiangshuAvatarId < 0;
					if (!flag2)
					{
						bool flag3 = adventureSiteData.RemainingMonths > 0;
						if (flag3)
						{
							data.SetWorldState(17);
							data.AddAwakeningXiangshuAvatar(xiangshuAvatarId);
						}
						else
						{
							short xiangshuTemplateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(xiangshuAvatarId, xiangshuLevel, true);
							GameData.Domains.Character.Character xiangshu;
							bool flag4 = DomainManager.Character.TryGetFixedCharacterByTemplateId(xiangshuTemplateId, out xiangshu) && xiangshu.GetLocation().IsValid();
							if (flag4)
							{
								data.SetWorldState(18);
								data.AddAttackingXiangshuAvatar(xiangshuAvatarId);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x000F8A38 File Offset: 0x000F6C38
		public static void DetectXiangshuInvasionProgress(this WorldStateData data)
		{
			bool flag = !DomainManager.TutorialChapter.GetInGuiding() && DomainManager.World.GetMainStoryLineProgress() > 2;
			if (flag)
			{
				data.SetWorldState((short)GameData.Domains.World.SharedMethods.GetInvasionWorldStateTemplateId());
			}
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x000F8A74 File Offset: 0x000F6C74
		public static void DetectXiangshuInfection(this WorldStateData data)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			byte xiangshuInfection = taiwu.GetXiangshuInfection();
			bool flag = xiangshuInfection >= 200;
			if (flag)
			{
				data.SetWorldState(16);
			}
			else
			{
				bool flag2 = xiangshuInfection >= 100;
				if (flag2)
				{
					data.SetWorldState(15);
				}
			}
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x000F8AC4 File Offset: 0x000F6CC4
		public static void DetectMartialArtTournament(this WorldStateData data)
		{
			MonthlyActionKey key = new MonthlyActionKey(2, 0);
			MonthlyActionBase action = DomainManager.TaiwuEvent.GetMonthlyAction(key);
			bool flag = action == null;
			if (!flag)
			{
				switch (action.State)
				{
				case 1:
					data.SetWorldState(19);
					break;
				case 2:
				case 3:
					data.SetWorldState(20);
					break;
				case 5:
					data.SetWorldState(21);
					break;
				}
			}
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x000F8B3C File Offset: 0x000F6D3C
		public static void DetectChangeWorldCreation(this WorldStateData data)
		{
			bool canResetWorldSettings = DomainManager.Extra.GetCanResetWorldSettings();
			if (canResetWorldSettings)
			{
				data.SetWorldState(38);
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x000F8B64 File Offset: 0x000F6D64
		public static void DetectLoongDebuff(this WorldStateData data)
		{
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			foreach (KeyValuePair<short, LoongInfo> keyValuePair in DomainManager.Extra.FiveLoongDict)
			{
				short num;
				LoongInfo loongInfo2;
				keyValuePair.Deconstruct(out num, out loongInfo2);
				LoongInfo loongInfo = loongInfo2;
				ushort debuffCount;
				bool flag = loongInfo.CharacterDebuffCounts != null && loongInfo.CharacterDebuffCounts.TryGetValue(taiwuId, out debuffCount) && debuffCount > 0;
				if (flag)
				{
					data.SetWorldState(loongInfo.ConfigData.WorldState);
				}
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x000F8C04 File Offset: 0x000F6E04
		public static void DetectInFulongFlameArea(this WorldStateData data)
		{
			bool isTaiwuInFulongFlameArea = DomainManager.Map.GetIsTaiwuInFulongFlameArea();
			if (isTaiwuInFulongFlameArea)
			{
				data.SetWorldState(44);
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x000F8C2C File Offset: 0x000F6E2C
		public static void DetectTribulation(this WorldStateData data)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
			TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
			bool isTriggeringTribulation = skillsData.IsTriggeringTribulation;
			if (isTriggeringTribulation)
			{
				data.SetWorldState(45);
			}
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x000F8C60 File Offset: 0x000F6E60
		public static void DetectSectMainStory(this WorldStateData data)
		{
			bool flag = !DomainManager.World.CheckCurrMainStoryLineProgressInRange(16, 28);
			if (!flag)
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				Location location = taiwu.GetLocation();
				bool flag2 = !location.IsValid();
				if (flag2)
				{
					location = taiwu.GetValidLocation();
				}
				bool flag3 = MapAreaData.IsBrokenArea(location.AreaId);
				if (!flag3)
				{
					MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)location.AreaId);
					MapAreaItem areaCfg = areaData.GetConfig();
					MapStateItem mapStateCfg = MapState.Instance[areaCfg.StateID];
					bool flag4 = mapStateCfg.SectID < 0;
					if (!flag4)
					{
						bool flag5 = DomainManager.World.SectMainStoryTriggeredThisMonth(mapStateCfg.SectID);
						if (!flag5)
						{
							bool flag6 = !DomainManager.World.CheckSectMainStoryAvailable(mapStateCfg.SectID);
							if (!flag6)
							{
								sbyte worldState = Organization.Instance[mapStateCfg.SectID].TaskReadyWorldState;
								bool flag7 = worldState < 0;
								if (!flag7)
								{
									WorldStateItem config = WorldState.Instance[worldState];
									bool flag8 = config.TriggerArea >= 0 && areaData.GetTemplateId() != config.TriggerArea;
									if (!flag8)
									{
										EventArgBox eventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(mapStateCfg.SectID);
										sbyte sectID = mapStateCfg.SectID;
										if (!true)
										{
										}
										bool flag9;
										if (sectID != 1)
										{
											flag9 = (sectID != 2 || !eventArgBox.Contains<int>("ConchShip_PresetKey_EmeiAdventureTwoAppearDate"));
										}
										else
										{
											flag9 = !eventArgBox.GetBool("ConchShip_PresetKey_ShaolinLearnedAny");
										}
										if (!true)
										{
										}
										bool sectSpecificCondition = flag9;
										bool flag10 = !sectSpecificCondition;
										if (!flag10)
										{
											data.SetWorldState((short)worldState);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x000F8E18 File Offset: 0x000F7018
		public static void DetectTaiwuWanted(this WorldStateData data)
		{
			bool flag = DomainManager.World.GetMainStoryLineProgress() < 16;
			if (!flag)
			{
				foreach (int charId in DomainManager.Taiwu.GetGroupCharIds().GetCollection())
				{
					sbyte sectOrgTemplateId;
					SettlementBounty bounty = DomainManager.Organization.GetBounty(charId, out sectOrgTemplateId);
					bool flag2 = bounty == null || sectOrgTemplateId < 0;
					if (!flag2)
					{
						data.SetWorldState(49);
						break;
					}
				}
			}
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x000F8EB8 File Offset: 0x000F70B8
		public static void DetectTeammateDying(this WorldStateData data)
		{
			HashSet<int> ids = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int id in ids)
			{
				GameData.Domains.Character.Character groupChar = DomainManager.Character.GetElement_Objects(id);
				EHealthType type = groupChar.GetHealthType();
				bool flag = type <= EHealthType.CriticallyIll;
				bool flag2 = flag;
				if (flag2)
				{
					data.SetWorldState(51);
					break;
				}
			}
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x000F8F50 File Offset: 0x000F7150
		public static void DetectHomelessVillager(this WorldStateData data)
		{
			bool flag = DomainManager.Building.GetHomeless().GetCount() > 0 && !DomainManager.TutorialChapter.GetInGuiding() && DomainManager.World.GetMainStoryLineProgress() >= 9;
			if (flag)
			{
				data.SetWorldState(52);
			}
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x000F8FA0 File Offset: 0x000F71A0
		public static void DetectNeiliConflicting(this WorldStateData data)
		{
			HashSet<int> ids = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int id in ids)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(id);
				NeiliTypeItem neiliTypeConfig = NeiliType.Instance[character.GetNeiliType()];
				bool showConflictingWorldState = neiliTypeConfig.ShowConflictingWorldState;
				if (showConflictingWorldState)
				{
					data.SetWorldState(53);
					break;
				}
			}
		}
	}
}
