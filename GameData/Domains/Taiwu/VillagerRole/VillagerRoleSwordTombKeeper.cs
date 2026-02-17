using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.Filters;
using GameData.Domains.Combat;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Organization.TaiwuVillageStoragesRecord;
using GameData.Domains.Taiwu.Display;
using GameData.Domains.Taiwu.Display.VillagerRoleArrangement;
using GameData.Domains.World;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x02000053 RID: 83
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public class VillagerRoleSwordTombKeeper : VillagerRoleBase, IVillagerRoleContact, IVillagerRoleInfluence, IVillagerRoleArrangementExecutor, IVillagerRoleSelectLocation
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x0013D5F6 File Offset: 0x0013B7F6
		public override short RoleTemplateId
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0013D5FC File Offset: 0x0013B7FC
		void IVillagerRoleArrangementExecutor.ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action)
		{
			int arrangementTemplateId = this.ArrangementTemplateId;
			int num = arrangementTemplateId;
			if (num == 3)
			{
				this.ApplyGuardingSwordTombAction(context, action);
			}
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0013D624 File Offset: 0x0013B824
		public override void OfflineSetArrangement(short arrangementTemplateId, Location location)
		{
			base.OfflineSetArrangement(arrangementTemplateId, location);
			bool flag = location.IsValid() && arrangementTemplateId == 3;
			if (flag)
			{
				MapBlockData blockData = DomainManager.Map.GetBlock(location).GetRootBlock();
				this.XiangshuAvatarId = (sbyte)XiangshuAvatarIds.SwordTombBlockTemplateIds.IndexOf(blockData.TemplateId);
			}
			else
			{
				this.XiangshuAvatarId = -1;
			}
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0013D684 File Offset: 0x0013B884
		private void ApplyGuardingSwordTombAction(DataContext context, VillagerRoleArrangementAction action)
		{
			int charId = this.Character.GetId();
			Location location = this.Character.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			int infection = this.InfectionBaseValue;
			bool hasChickenUpgradeEffect = base.HasChickenUpgradeEffect;
			if (hasChickenUpgradeEffect)
			{
				infection -= infection * this.InfectionDecreaseRate / 100;
			}
			this.Character.ChangeXiangshuInfection(context, infection);
			bool flag = infection > 0;
			if (flag)
			{
				lifeRecordCollection.AddGuardingSwordTombXiangshuInfectUp(charId, currDate, location);
			}
			sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
			short templateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(this.XiangshuAvatarId, xiangshuLevel, true);
			bool successCollected = context.Random.CheckPercentProb(this.CollectInformationChance);
			bool flag2 = successCollected;
			if (flag2)
			{
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				NormalInformation information = DomainManager.Information.CalcSwordTombInformation(EInformationInfoSwordInformationType.SwordTombIntell, false, (int)this.XiangshuAvatarId);
				successCollected = DomainManager.Information.CheckAddNormalInformationToCharacter(context, taiwuCharId, information);
				bool flag3 = successCollected;
				if (flag3)
				{
					lifeRecordCollection.AddInquireSwordTomb(charId, currDate, this.XiangshuAvatarId);
					monthlyNotificationCollection.AddXiangshuNormalInformation(charId, this.XiangshuAvatarId, templateId);
				}
			}
			bool flag4 = this.XiangshuAvatarId >= 0 && context.Random.CheckPercentProb(this.FeatureGainRateA);
			if (flag4)
			{
				GameData.Domains.Character.Character character = this.Character;
				short currentLevelXiangshuTemplateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(this.XiangshuAvatarId, 0, false);
				if (!true)
				{
				}
				short featureId;
				if (currentLevelXiangshuTemplateId <= 75)
				{
					if (currentLevelXiangshuTemplateId <= 57)
					{
						if (currentLevelXiangshuTemplateId == 48)
						{
							featureId = 741;
							goto IL_212;
						}
						if (currentLevelXiangshuTemplateId == 57)
						{
							featureId = 742;
							goto IL_212;
						}
					}
					else
					{
						if (currentLevelXiangshuTemplateId == 66)
						{
							featureId = 743;
							goto IL_212;
						}
						if (currentLevelXiangshuTemplateId == 75)
						{
							featureId = 744;
							goto IL_212;
						}
					}
				}
				else if (currentLevelXiangshuTemplateId <= 93)
				{
					if (currentLevelXiangshuTemplateId == 84)
					{
						featureId = 745;
						goto IL_212;
					}
					if (currentLevelXiangshuTemplateId == 93)
					{
						featureId = 746;
						goto IL_212;
					}
				}
				else
				{
					if (currentLevelXiangshuTemplateId == 102)
					{
						featureId = 747;
						goto IL_212;
					}
					if (currentLevelXiangshuTemplateId == 111)
					{
						featureId = 748;
						goto IL_212;
					}
					if (currentLevelXiangshuTemplateId == 120)
					{
						featureId = 749;
						goto IL_212;
					}
				}
				featureId = -1;
				IL_212:
				if (!true)
				{
				}
				character.AddFeature(context, featureId, false);
			}
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0013D8B8 File Offset: 0x0013BAB8
		public override void ExecuteFixedAction(DataContext context)
		{
			bool flag = this.ArrangementTemplateId >= 0;
			if (!flag)
			{
				VillagerWorkData workData = this.WorkData;
				bool flag2 = workData != null && workData.WorkType == 1;
				if (!flag2)
				{
					bool flag3 = base.AutoActionStates[7];
					if (flag3)
					{
						this.TryAddNextAutoTravelTarget(context, new Predicate<MapBlockData>(this.AutoActionBlockFilter));
						this.AutoBattleWithRandomEnemy(context);
					}
				}
			}
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0013D928 File Offset: 0x0013BB28
		private bool AutoActionBlockFilter(MapBlockData blockData)
		{
			MapDomain mapDomain = DomainManager.Map;
			Location location = this.Character.GetLocation();
			return location.IsValid() && mapDomain.GetStateIdByAreaId(location.AreaId) == mapDomain.GetStateIdByAreaId(blockData.AreaId) && blockData.TemplateEnemyList != null && blockData.TemplateEnemyList.Any((MapTemplateEnemyInfo info) => Config.Character.Instance[info.TemplateId].ConsummateLevel <= this.Character.GetConsummateLevel());
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0013D99C File Offset: 0x0013BB9C
		private void AutoBattleWithRandomEnemy(DataContext context)
		{
			Location location = this.Character.GetLocation();
			MapBlockData block = DomainManager.Map.GetBlock(location);
			int count = VillagerRoleFormulaImpl.Calculate(33, (int)this.Character.GetPersonality(4));
			ObjectPool<List<short>> listPool = ObjectPool<List<short>>.Instance;
			List<short> range = listPool.Get();
			range.Clear();
			bool flag = block.TemplateEnemyList != null;
			if (flag)
			{
				range.AddRange(from info in block.TemplateEnemyList
				select info.TemplateId);
			}
			CollectionUtils.Shuffle<short>(context.Random, range);
			int rangeDiff = range.Count - count;
			bool flag2 = rangeDiff > 0;
			if (flag2)
			{
				range.RemoveRange(0, rangeDiff);
			}
			CharacterDomain charDomain = DomainManager.Character;
			foreach (short targetCharTemplateId in range)
			{
				GameData.Domains.Character.Character enemyChar;
				AiHelper.NpcCombatResultType combatResultType = charDomain.SimulateEnemyAttackWithEnemyChar(context, targetCharTemplateId, this.Character, out enemyChar);
				bool fightWin = combatResultType <= AiHelper.NpcCombatResultType.MinorVictory;
				bool flag3 = fightWin;
				if (flag3)
				{
					int[] enemyTeam = new int[]
					{
						enemyChar.GetId()
					};
					CombatResultDisplayData combatResultData = new CombatResultDisplayData();
					CombatConfigItem combatConfig = CombatConfig.Instance[1];
					combatResultData.CombatStatus = CombatStatusType.EnemyFail;
					CombatDomain.ResultCalcResource(combatConfig, false, this.Character, enemyTeam, combatResultData);
					CombatDomain.ResultCalcLootItem(context.Random, 50, combatConfig.CombatType, combatResultData.CombatStatus, false, combatConfig, enemyChar, enemyTeam, Array.Empty<int>(), combatResultData);
					TaiwuVillageStoragesRecordCollection storageRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
					int currDate = DomainManager.World.GetCurrDate();
					foreach (ItemDisplayData itemData in combatResultData.ItemList)
					{
						ItemKey itemKey = itemData.Key;
						ItemKey createdKey = DomainManager.Item.CreateItem(context, itemKey.ItemType, itemKey.TemplateId);
						DomainManager.Taiwu.StoreItemInTreasury(context, createdKey, itemData.Amount, false);
						storageRecordCollection.AddVillagerEnemyDropItem(currDate, TaiwuVillageStorageType.Treasury, this.Character.GetId(), itemKey.ItemType, itemKey.TemplateId);
					}
					for (sbyte i = 0; i < 8; i += 1)
					{
						int amount = combatResultData.Resource.Get((int)i);
						bool shouldTreasury = i != 7;
						DomainManager.Taiwu.AddResource(context, shouldTreasury ? ItemSourceType.Treasury : ItemSourceType.Inventory, i, amount);
						bool flag4 = shouldTreasury && amount > 0;
						if (flag4)
						{
							storageRecordCollection.AddVillagerEnemyDropResources(currDate, TaiwuVillageStorageType.Treasury, this.Character.GetId(), i, amount);
						}
					}
				}
			}
			listPool.Return(range);
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x0013DC94 File Offset: 0x0013BE94
		public bool IncreaseFavorability
		{
			get
			{
				return this.PositiveAction;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x0013DC9C File Offset: 0x0013BE9C
		public unsafe int ContactFavorabilityChange
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateSwordTombKeeperContactFavorabilityChange(this.Character.GetPersonalities(), *this.Character.GetCombatSkillAttainments());
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x0013DCBE File Offset: 0x0013BEBE
		public int ContactCharacterAmount
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateSwordTombKeeperContactCharacterAmount(this.Character.GetPersonalities());
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06001428 RID: 5160 RVA: 0x0013DCD0 File Offset: 0x0013BED0
		public int LearnActionRepeatChance
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateSwordTombKeeperLearnActionRepeatChance(this.Character.GetPersonalities());
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06001429 RID: 5161 RVA: 0x0013DCE2 File Offset: 0x0013BEE2
		public int LearnRequestSuccessChanceBonus
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateSwordTombKeeperLearnRequestSuccessChanceBonus(this.Character.GetPersonalities());
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x0013DCF4 File Offset: 0x0013BEF4
		public int CollectInformationChance
		{
			get
			{
				return Math.Clamp(VillagerRoleFormulaImpl.Calculate(35, this.CalcSwordTombKeeperLifeSkillMaxAttainment(), (int)this.Character.GetPersonality(3)), 0, 100);
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x0013DD17 File Offset: 0x0013BF17
		public int InjuredByXiangshuAvatarChance
		{
			get
			{
				return Math.Clamp(VillagerRoleFormulaImpl.Calculate(36, this.CalcSwordTombKeeperLifeSkillMaxAttainment(), (int)this.Character.GetPersonality(3)), 0, 100);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x0013DD3A File Offset: 0x0013BF3A
		private int ChickenUpgradeActionChance
		{
			get
			{
				return (int)(this.Character.GetPersonality(3) / 2);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600142D RID: 5165 RVA: 0x0013DD4A File Offset: 0x0013BF4A
		internal int FeatureGainRateA
		{
			get
			{
				return Math.Clamp(VillagerRoleFormulaImpl.Calculate(38, this.CalcSwordTombKeeperLifeSkillMaxAttainment(), (int)this.Character.GetPersonality(3)), 0, 100);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x0013DD6D File Offset: 0x0013BF6D
		internal int FeatureGainRateB
		{
			get
			{
				return Math.Clamp(VillagerRoleFormulaImpl.Calculate(39, this.CalcSwordTombKeeperLifeSkillMaxAttainment(), (int)this.Character.GetPersonality(3)), 0, 100);
			}
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0013DD90 File Offset: 0x0013BF90
		void IVillagerRoleInfluence.ApplyInfluenceAction(DataContext context)
		{
			int valueChange = this.InfluenceSettlementValueChange;
			bool flag = !this.PositiveAction;
			if (flag)
			{
				valueChange = -valueChange;
			}
			short areaId = this.Character.GetLocation().AreaId;
			MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)areaId);
			int authorityGain = 0;
			foreach (SettlementInfo settlementInfo in areaData.SettlementInfos)
			{
				bool flag2 = settlementInfo.SettlementId < 0;
				if (!flag2)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
					settlement.ChangeSafety(context, valueChange);
					authorityGain += this.GetSettlementInfluenceAuthorityGain(settlement);
				}
			}
			DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 7, authorityGain);
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x0013DE4C File Offset: 0x0013C04C
		public unsafe int InfluenceSettlementValueChange
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateSwordTombKeeperInfluenceSettlementValueChange(this.Character.GetPersonalities(), *this.Character.GetCombatSkillAttainments());
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x0013DE6E File Offset: 0x0013C06E
		public static int InjuryByXiangshuAvatarAmount
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(37, (int)DomainManager.World.GetXiangshuLevel(), DomainManager.World.GetWorldCreationSetting(1));
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x0013DE8C File Offset: 0x0013C08C
		internal int InfectionBaseValue
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(34);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x0013DE95 File Offset: 0x0013C095
		internal int InfectionDecreaseRate
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(40, this.CalcSwordTombKeeperLifeSkillMaxAttainment(), (int)this.Character.GetPersonality(3));
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x0013DEB0 File Offset: 0x0013C0B0
		public unsafe sbyte XiangshuAvatarEscapeState
		{
			get
			{
				WorldStateData worldState = *DomainManager.World.GetWorldStateData();
				bool flag = worldState.IsXiangshuAvatarAwakening(this.XiangshuAvatarId);
				sbyte result;
				if (flag)
				{
					result = 1;
				}
				else
				{
					bool flag2 = worldState.IsXiangshuAvatarAttacking(this.XiangshuAvatarId);
					if (flag2)
					{
						result = 2;
					}
					else
					{
						result = 0;
					}
				}
				return result;
			}
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0013DEFD File Offset: 0x0013C0FD
		private static IEnumerable<sbyte> CalcSwordTombKeeperLifeSkillTypes()
		{
			yield return 13;
			yield return 12;
			yield break;
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0013DF08 File Offset: 0x0013C108
		private unsafe int CalcSwordTombKeeperLifeSkillMaxAttainment()
		{
			LifeSkillShorts items = *this.Character.GetLifeSkillAttainments();
			short max = short.MinValue;
			foreach (sbyte lifeSkillType in VillagerRoleSwordTombKeeper.CalcSwordTombKeeperLifeSkillTypes())
			{
				bool flag = *items[(int)lifeSkillType] > max;
				if (flag)
				{
					max = *items[(int)lifeSkillType];
				}
			}
			return (int)max;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0013DF8C File Offset: 0x0013C18C
		Location IVillagerRoleSelectLocation.SelectNextWorkLocation(IRandomSource random, Location baseLocation)
		{
			bool flag = baseLocation.BlockId >= 0;
			Location result;
			if (flag)
			{
				bool flag2 = this.ArrangementTemplateId == 3;
				if (flag2)
				{
					sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
					short templateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(this.XiangshuAvatarId, xiangshuLevel, true);
					GameData.Domains.Character.Character xiangshuAvatar;
					bool flag3 = DomainManager.Character.TryGetFixedCharacterByTemplateId(templateId, out xiangshuAvatar);
					if (flag3)
					{
						Location location = xiangshuAvatar.GetLocation();
						bool flag4 = location.IsValid();
						if (flag4)
						{
							return location;
						}
					}
				}
				MapBlockData block = DomainManager.Map.GetBlock(baseLocation);
				List<MapBlockData> groupBlockList = block.GroupBlockList;
				result = ((groupBlockList != null && groupBlockList.Count > 0) ? block.GroupBlockList.GetRandom(random).GetLocation() : baseLocation);
			}
			else
			{
				List<MapBlockData> validBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
				DomainManager.Map.GetMapBlocksInAreaByFilters(baseLocation.AreaId, new Predicate<MapBlockData>(this.NextLocationFilter), validBlocks);
				bool flag5 = validBlocks.Count == 0;
				if (flag5)
				{
					ObjectPool<List<MapBlockData>>.Instance.Return(validBlocks);
					short stationBlockId = DomainManager.Map.GetElement_Areas((int)baseLocation.AreaId).StationBlockId;
					result = new Location(baseLocation.AreaId, stationBlockId);
				}
				else
				{
					MapBlockData nextBlock = validBlocks.GetRandom(random);
					ObjectPool<List<MapBlockData>>.Instance.Return(validBlocks);
					result = nextBlock.GetLocation();
				}
			}
			return result;
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0013E0DC File Offset: 0x0013C2DC
		public int GetSettlementInfluenceAuthorityGain(Settlement settlement)
		{
			bool flag = settlement == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int safetyValue = (int)(this.PositiveAction ? settlement.GetSafety() : (settlement.GetMaxSafety() - settlement.GetSafety()));
				result = safetyValue * (int)(50 + this.Character.GetPersonality(4) / 2) / 100;
			}
			return result;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0013E130 File Offset: 0x0013C330
		public void SelectContactTargets(IRandomSource random, List<GameData.Domains.Character.Character> selectedCharList, int selectAmount)
		{
			VillagerRoleSwordTombKeeper.<>c__DisplayClass45_0 CS$<>8__locals1 = new VillagerRoleSwordTombKeeper.<>c__DisplayClass45_0();
			CS$<>8__locals1.<>4__this = this;
			selectedCharList.Clear();
			Location location = this.Character.GetLocation();
			CS$<>8__locals1.currBlock = DomainManager.Map.GetBlock(location);
			bool flag = CS$<>8__locals1.currBlock.BelongBlockId < 0;
			if (!flag)
			{
				MapCharacterFilter.Find(new Predicate<GameData.Domains.Character.Character>(CS$<>8__locals1.<SelectContactTargets>g__CharacterFilter|0), selectedCharList, location.AreaId, false);
				bool flag2 = selectedCharList.Count > selectAmount;
				if (flag2)
				{
					selectedCharList.RemoveRange(selectAmount, selectedCharList.Count - selectAmount);
				}
			}
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0013E1BC File Offset: 0x0013C3BC
		public override IVillagerRoleArrangementDisplayData GetArrangementDisplayData()
		{
			return new GuardingSwordTombDisplayData
			{
				InformationGatheringSuccessRate = this.CollectInformationChance,
				InjuryProbability = this.InjuredByXiangshuAvatarChance,
				FeatureGainRateA = this.FeatureGainRateA,
				FeatureGainRateB = this.FeatureGainRateB,
				InfectionDecreaseRate = this.InfectionDecreaseRate,
				SwordTombId = this.XiangshuAvatarId,
				EscapeState = this.XiangshuAvatarEscapeState
			};
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0013E228 File Offset: 0x0013C428
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0013E23C File Offset: 0x0013C43C
		public override int GetSerializedSize()
		{
			int totalSize = 8;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0013E260 File Offset: 0x0013C460
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = 3;
			byte* pCurrData = pData + 2;
			*(int*)pCurrData = this.ArrangementTemplateId;
			pCurrData += 4;
			*pCurrData = (byte)this.XiangshuAvatarId;
			pCurrData++;
			*pCurrData = (this.PositiveAction ? 1 : 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0013E2B8 File Offset: 0x0013C4B8
		public unsafe override int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this.ArrangementTemplateId = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				this.XiangshuAvatarId = *(sbyte*)pCurrData;
				pCurrData++;
			}
			bool flag3 = fieldCount > 2;
			if (flag3)
			{
				this.PositiveAction = (*pCurrData != 0);
				pCurrData++;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04000325 RID: 805
		[SerializableGameDataField]
		public bool PositiveAction = true;

		// Token: 0x04000326 RID: 806
		[SerializableGameDataField]
		public sbyte XiangshuAvatarId = -1;

		// Token: 0x02000969 RID: 2409
		private static class FieldIds
		{
			// Token: 0x0400277C RID: 10108
			public const ushort ArrangementTemplateId = 0;

			// Token: 0x0400277D RID: 10109
			public const ushort XiangshuAvatarId = 1;

			// Token: 0x0400277E RID: 10110
			public const ushort PositiveAction = 2;

			// Token: 0x0400277F RID: 10111
			public const ushort Count = 3;

			// Token: 0x04002780 RID: 10112
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"ArrangementTemplateId",
				"XiangshuAvatarId",
				"PositiveAction"
			};
		}
	}
}
