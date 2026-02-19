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

namespace GameData.Domains.Taiwu.VillagerRole;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
public class VillagerRoleSwordTombKeeper : VillagerRoleBase, IVillagerRoleContact, IVillagerRoleInfluence, IVillagerRoleArrangementExecutor, IVillagerRoleSelectLocation
{
	private static class FieldIds
	{
		public const ushort ArrangementTemplateId = 0;

		public const ushort XiangshuAvatarId = 1;

		public const ushort PositiveAction = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "ArrangementTemplateId", "XiangshuAvatarId", "PositiveAction" };
	}

	[SerializableGameDataField]
	public bool PositiveAction = true;

	[SerializableGameDataField]
	public sbyte XiangshuAvatarId = -1;

	public override short RoleTemplateId => 5;

	public bool IncreaseFavorability => PositiveAction;

	public int ContactFavorabilityChange => SharedMethods.CalculateSwordTombKeeperContactFavorabilityChange(Character.GetPersonalities(), Character.GetCombatSkillAttainments());

	public int ContactCharacterAmount => SharedMethods.CalculateSwordTombKeeperContactCharacterAmount(Character.GetPersonalities());

	public int LearnActionRepeatChance => SharedMethods.CalculateSwordTombKeeperLearnActionRepeatChance(Character.GetPersonalities());

	public int LearnRequestSuccessChanceBonus => SharedMethods.CalculateSwordTombKeeperLearnRequestSuccessChanceBonus(Character.GetPersonalities());

	public int CollectInformationChance => Math.Clamp(VillagerRoleFormulaImpl.Calculate(35, CalcSwordTombKeeperLifeSkillMaxAttainment(), Character.GetPersonality(3)), 0, 100);

	public int InjuredByXiangshuAvatarChance => Math.Clamp(VillagerRoleFormulaImpl.Calculate(36, CalcSwordTombKeeperLifeSkillMaxAttainment(), Character.GetPersonality(3)), 0, 100);

	private int ChickenUpgradeActionChance => Character.GetPersonality(3) / 2;

	internal int FeatureGainRateA => Math.Clamp(VillagerRoleFormulaImpl.Calculate(38, CalcSwordTombKeeperLifeSkillMaxAttainment(), Character.GetPersonality(3)), 0, 100);

	internal int FeatureGainRateB => Math.Clamp(VillagerRoleFormulaImpl.Calculate(39, CalcSwordTombKeeperLifeSkillMaxAttainment(), Character.GetPersonality(3)), 0, 100);

	public int InfluenceSettlementValueChange => SharedMethods.CalculateSwordTombKeeperInfluenceSettlementValueChange(Character.GetPersonalities(), Character.GetCombatSkillAttainments());

	public static int InjuryByXiangshuAvatarAmount => VillagerRoleFormulaImpl.Calculate(37, DomainManager.World.GetXiangshuLevel(), DomainManager.World.GetWorldCreationSetting(1));

	internal int InfectionBaseValue => VillagerRoleFormulaImpl.Calculate(34);

	internal int InfectionDecreaseRate => VillagerRoleFormulaImpl.Calculate(40, CalcSwordTombKeeperLifeSkillMaxAttainment(), Character.GetPersonality(3));

	public sbyte XiangshuAvatarEscapeState
	{
		get
		{
			WorldStateData worldStateData = DomainManager.World.GetWorldStateData();
			if (worldStateData.IsXiangshuAvatarAwakening(XiangshuAvatarId))
			{
				return 1;
			}
			if (worldStateData.IsXiangshuAvatarAttacking(XiangshuAvatarId))
			{
				return 2;
			}
			return 0;
		}
	}

	void IVillagerRoleArrangementExecutor.ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action)
	{
		int arrangementTemplateId = ArrangementTemplateId;
		int num = arrangementTemplateId;
		if (num == 3)
		{
			ApplyGuardingSwordTombAction(context, action);
		}
	}

	public override void OfflineSetArrangement(short arrangementTemplateId, Location location)
	{
		base.OfflineSetArrangement(arrangementTemplateId, location);
		if (location.IsValid() && arrangementTemplateId == 3)
		{
			MapBlockData rootBlock = DomainManager.Map.GetBlock(location).GetRootBlock();
			XiangshuAvatarId = (sbyte)XiangshuAvatarIds.SwordTombBlockTemplateIds.IndexOf(rootBlock.TemplateId);
		}
		else
		{
			XiangshuAvatarId = -1;
		}
	}

	private void ApplyGuardingSwordTombAction(DataContext context, VillagerRoleArrangementAction action)
	{
		int id = Character.GetId();
		Location location = Character.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		int num = InfectionBaseValue;
		if (base.HasChickenUpgradeEffect)
		{
			num -= num * InfectionDecreaseRate / 100;
		}
		Character.ChangeXiangshuInfection(context, num);
		if (num > 0)
		{
			lifeRecordCollection.AddGuardingSwordTombXiangshuInfectUp(id, currDate, location);
		}
		sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
		short currentLevelXiangshuTemplateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(XiangshuAvatarId, xiangshuLevel, isWeakened: true);
		if (context.Random.CheckPercentProb(CollectInformationChance))
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			NormalInformation information = DomainManager.Information.CalcSwordTombInformation(EInformationInfoSwordInformationType.SwordTombIntell, isConsume: false, XiangshuAvatarId);
			if (DomainManager.Information.CheckAddNormalInformationToCharacter(context, taiwuCharId, information))
			{
				lifeRecordCollection.AddInquireSwordTomb(id, currDate, XiangshuAvatarId);
				monthlyNotificationCollection.AddXiangshuNormalInformation(id, XiangshuAvatarId, currentLevelXiangshuTemplateId);
			}
		}
		if (XiangshuAvatarId >= 0 && context.Random.CheckPercentProb(FeatureGainRateA))
		{
			GameData.Domains.Character.Character character = Character;
			short currentLevelXiangshuTemplateId2 = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(XiangshuAvatarId, 0);
			if (1 == 0)
			{
			}
			short featureId = currentLevelXiangshuTemplateId2 switch
			{
				48 => 741, 
				57 => 742, 
				66 => 743, 
				75 => 744, 
				84 => 745, 
				93 => 746, 
				102 => 747, 
				111 => 748, 
				120 => 749, 
				_ => -1, 
			};
			if (1 == 0)
			{
			}
			character.AddFeature(context, featureId);
		}
	}

	public override void ExecuteFixedAction(DataContext context)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (ArrangementTemplateId >= 0)
		{
			return;
		}
		VillagerWorkData workData = WorkData;
		if (workData == null || workData.WorkType != 1)
		{
			BoolArray64 autoActionStates = base.AutoActionStates;
			if (((BoolArray64)(ref autoActionStates))[7])
			{
				TryAddNextAutoTravelTarget(context, AutoActionBlockFilter);
				AutoBattleWithRandomEnemy(context);
			}
		}
	}

	private bool AutoActionBlockFilter(MapBlockData blockData)
	{
		MapDomain map = DomainManager.Map;
		Location location = Character.GetLocation();
		if (location.IsValid() && map.GetStateIdByAreaId(location.AreaId) == map.GetStateIdByAreaId(blockData.AreaId) && blockData.TemplateEnemyList != null && blockData.TemplateEnemyList.Any((MapTemplateEnemyInfo info) => Config.Character.Instance[info.TemplateId].ConsummateLevel <= Character.GetConsummateLevel()))
		{
			return true;
		}
		return false;
	}

	private void AutoBattleWithRandomEnemy(DataContext context)
	{
		Location location = Character.GetLocation();
		MapBlockData block = DomainManager.Map.GetBlock(location);
		int num = VillagerRoleFormulaImpl.Calculate(33, Character.GetPersonality(4));
		ObjectPool<List<short>> instance = ObjectPool<List<short>>.Instance;
		List<short> list = instance.Get();
		list.Clear();
		if (block.TemplateEnemyList != null)
		{
			list.AddRange(block.TemplateEnemyList.Select((MapTemplateEnemyInfo info) => info.TemplateId));
		}
		CollectionUtils.Shuffle(context.Random, list);
		int num2 = list.Count - num;
		if (num2 > 0)
		{
			list.RemoveRange(0, num2);
		}
		CharacterDomain character = DomainManager.Character;
		foreach (short item in list)
		{
			GameData.Domains.Character.Character enemyChar;
			AiHelper.NpcCombatResultType npcCombatResultType = character.SimulateEnemyAttackWithEnemyChar(context, item, Character, out enemyChar);
			if (npcCombatResultType > AiHelper.NpcCombatResultType.MinorVictory)
			{
				continue;
			}
			int[] enemyTeam = new int[1] { enemyChar.GetId() };
			CombatResultDisplayData combatResultDisplayData = new CombatResultDisplayData();
			CombatConfigItem combatConfigItem = CombatConfig.Instance[(short)1];
			combatResultDisplayData.CombatStatus = CombatStatusType.EnemyFail;
			CombatDomain.ResultCalcResource(combatConfigItem, isPlaygroundCombat: false, Character, enemyTeam, combatResultDisplayData);
			CombatDomain.ResultCalcLootItem(context.Random, 50, combatConfigItem.CombatType, combatResultDisplayData.CombatStatus, isPuppetCombat: false, combatConfigItem, enemyChar, enemyTeam, Array.Empty<int>(), combatResultDisplayData);
			TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			foreach (ItemDisplayData item2 in combatResultDisplayData.ItemList)
			{
				ItemKey key = item2.Key;
				ItemKey itemKey = DomainManager.Item.CreateItem(context, key.ItemType, key.TemplateId);
				DomainManager.Taiwu.StoreItemInTreasury(context, itemKey, item2.Amount);
				taiwuVillageStoragesRecordCollection.AddVillagerEnemyDropItem(currDate, TaiwuVillageStorageType.Treasury, Character.GetId(), key.ItemType, key.TemplateId);
			}
			for (sbyte b = 0; b < 8; b++)
			{
				int num3 = combatResultDisplayData.Resource.Get(b);
				bool flag = b != 7;
				DomainManager.Taiwu.AddResource(context, (!flag) ? ItemSourceType.Inventory : ItemSourceType.Treasury, b, num3);
				if (flag && num3 > 0)
				{
					taiwuVillageStoragesRecordCollection.AddVillagerEnemyDropResources(currDate, TaiwuVillageStorageType.Treasury, Character.GetId(), b, num3);
				}
			}
		}
		instance.Return(list);
	}

	void IVillagerRoleInfluence.ApplyInfluenceAction(DataContext context)
	{
		int num = InfluenceSettlementValueChange;
		if (!PositiveAction)
		{
			num = -num;
		}
		short areaId = Character.GetLocation().AreaId;
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(areaId);
		int num2 = 0;
		SettlementInfo[] settlementInfos = element_Areas.SettlementInfos;
		for (int i = 0; i < settlementInfos.Length; i++)
		{
			SettlementInfo settlementInfo = settlementInfos[i];
			if (settlementInfo.SettlementId >= 0)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
				settlement.ChangeSafety(context, num);
				num2 += GetSettlementInfluenceAuthorityGain(settlement);
			}
		}
		DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 7, num2);
	}

	private static IEnumerable<sbyte> CalcSwordTombKeeperLifeSkillTypes()
	{
		yield return 13;
		yield return 12;
	}

	private int CalcSwordTombKeeperLifeSkillMaxAttainment()
	{
		LifeSkillShorts lifeSkillAttainments = Character.GetLifeSkillAttainments();
		short num = short.MinValue;
		foreach (sbyte item in CalcSwordTombKeeperLifeSkillTypes())
		{
			if (lifeSkillAttainments[item] > num)
			{
				num = lifeSkillAttainments[item];
			}
		}
		return num;
	}

	Location IVillagerRoleSelectLocation.SelectNextWorkLocation(IRandomSource random, Location baseLocation)
	{
		if (baseLocation.BlockId >= 0)
		{
			if (ArrangementTemplateId == 3)
			{
				sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
				short currentLevelXiangshuTemplateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(XiangshuAvatarId, xiangshuLevel, isWeakened: true);
				if (DomainManager.Character.TryGetFixedCharacterByTemplateId(currentLevelXiangshuTemplateId, out var character))
				{
					Location location = character.GetLocation();
					if (location.IsValid())
					{
						return location;
					}
				}
			}
			MapBlockData block = DomainManager.Map.GetBlock(baseLocation);
			List<MapBlockData> groupBlockList = block.GroupBlockList;
			return (groupBlockList != null && groupBlockList.Count > 0) ? block.GroupBlockList.GetRandom(random).GetLocation() : baseLocation;
		}
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		DomainManager.Map.GetMapBlocksInAreaByFilters(baseLocation.AreaId, ((IVillagerRoleSelectLocation)this).NextLocationFilter, list);
		if (list.Count == 0)
		{
			ObjectPool<List<MapBlockData>>.Instance.Return(list);
			short stationBlockId = DomainManager.Map.GetElement_Areas(baseLocation.AreaId).StationBlockId;
			return new Location(baseLocation.AreaId, stationBlockId);
		}
		MapBlockData random2 = list.GetRandom(random);
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		return random2.GetLocation();
	}

	public int GetSettlementInfluenceAuthorityGain(Settlement settlement)
	{
		if (settlement == null)
		{
			return 0;
		}
		int num = (PositiveAction ? settlement.GetSafety() : (settlement.GetMaxSafety() - settlement.GetSafety()));
		return num * (50 + Character.GetPersonality(4) / 2) / 100;
	}

	public void SelectContactTargets(IRandomSource random, List<GameData.Domains.Character.Character> selectedCharList, int selectAmount)
	{
		selectedCharList.Clear();
		Location location = Character.GetLocation();
		MapBlockData currBlock = DomainManager.Map.GetBlock(location);
		if (currBlock.BelongBlockId >= 0)
		{
			MapCharacterFilter.Find(CharacterFilter, selectedCharList, location.AreaId);
			if (selectedCharList.Count > selectAmount)
			{
				selectedCharList.RemoveRange(selectAmount, selectedCharList.Count - selectAmount);
			}
		}
		bool CharacterFilter(GameData.Domains.Character.Character character)
		{
			if (!OrganizationDomain.IsLargeSect(character.GetOrganizationInfo().OrgTemplateId))
			{
				return false;
			}
			if (character == Character)
			{
				return false;
			}
			Location location2 = character.GetLocation();
			MapBlockData block = DomainManager.Map.GetBlock(location2);
			if (block.IsNonDeveloped())
			{
				return false;
			}
			if (block.BelongBlockId != currBlock.BelongBlockId)
			{
				return false;
			}
			return true;
		}
	}

	public override IVillagerRoleArrangementDisplayData GetArrangementDisplayData()
	{
		return new GuardingSwordTombDisplayData
		{
			InformationGatheringSuccessRate = CollectInformationChance,
			InjuryProbability = InjuredByXiangshuAvatarChance,
			FeatureGainRateA = FeatureGainRateA,
			FeatureGainRateB = FeatureGainRateB,
			InfectionDecreaseRate = InfectionDecreaseRate,
			SwordTombId = XiangshuAvatarId,
			EscapeState = XiangshuAvatarEscapeState
		};
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 8;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 3;
		ptr += 2;
		*(int*)ptr = ArrangementTemplateId;
		ptr += 4;
		*ptr = (byte)XiangshuAvatarId;
		ptr++;
		*ptr = (PositiveAction ? ((byte)1) : ((byte)0));
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ArrangementTemplateId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			XiangshuAvatarId = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 2)
		{
			PositiveAction = *ptr != 0;
			ptr++;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
