using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.Relation;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization.Display;
using GameData.Domains.Taiwu.Display;
using GameData.Domains.Taiwu.Display.VillagerRoleArrangement;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.VillagerRole;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
public class VillagerRoleHead : VillagerRoleBase, IVillagerRoleArrangementExecutor, IVillagerRoleSelectLocation
{
	public override short RoleTemplateId => 6;

	private int AutoActionAffectCount => VillagerRoleFormulaImpl.Calculate(11, base.Personality);

	private int AutoActionFavorChange => VillagerRoleFormulaImpl.Calculate(12, Character.GetLifeSkillAttainment(4));

	private int AutoActionFavorIncreaseRate => VillagerRoleFormulaImpl.Calculate(13, Character.GetBehaviorType());

	public int ArrangementSpecialRuleCount => VillagerRoleFormulaImpl.Calculate(15, base.Personality);

	public int ArrangementSpecialRuleRange => VillagerRoleFormulaImpl.Calculate(14, GlobalConfig.Instance.ModifySeverityDefaultRange);

	[Obsolete]
	public int VillagerFavorabilityChange => Character.GetPersonality(6) * 300;

	[Obsolete]
	public int PersonalityBonusPercent => SharedMethods.CalculateVillageHeadPersonalityBonusPercent(Character.GetPersonalities());

	[Obsolete]
	public int AttainmentBonusPercent => SharedMethods.CalculateVillageHeadAttainmentBonusPercent(Character.GetPersonalities());

	private int ChickenUpgradeTotalCount(DataContext context)
	{
		return (!base.HasChickenUpgradeEffect || !context.Random.CheckPercentProb(VillagerRoleFormulaImpl.Calculate(12, base.Personality))) ? 1 : 2;
	}

	public override void ExecuteFixedAction(DataContext context)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		BoolArray64 autoActionStates = base.AutoActionStates;
		if (!((BoolArray64)(ref autoActionStates))[8])
		{
			return;
		}
		int autoActionAffectCount = AutoActionAffectCount;
		int autoActionFavorChange = AutoActionFavorChange;
		int autoActionFavorIncreaseRate = AutoActionFavorIncreaseRate;
		Location location = Character.GetLocation();
		List<MapBlockData> obj = context.AdvanceMonthRelatedData.Blocks.Occupy();
		List<GameData.Domains.Character.Character> list = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		list.Clear();
		DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, obj, 2, includeCenter: true);
		foreach (MapBlockData item in obj)
		{
			if (item.CharacterSet == null)
			{
				continue;
			}
			foreach (int item2 in item.CharacterSet)
			{
				if (Character.GetId() != item2)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
					if (element_Objects.IsInteractableAsIntelligentCharacter())
					{
						list.Add(element_Objects);
					}
				}
			}
		}
		context.AdvanceMonthRelatedData.Blocks.Release(ref obj);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		for (int num = ChickenUpgradeTotalCount(context); num > 0; num--)
		{
			int num2 = autoActionAffectCount;
			CollectionUtils.Shuffle(context.Random, list);
			foreach (var (index, index2) in RandomUtils.GetRandomUnrepeatedIntPair(context.Random, list.Count))
			{
				if (--num2 < 0)
				{
					break;
				}
				GameData.Domains.Character.Character character = list[index];
				GameData.Domains.Character.Character character2 = list[index2];
				bool flag = context.Random.CheckPercentProb(autoActionFavorIncreaseRate);
				if (flag)
				{
					DomainManager.Character.ChangeFavorabilityOptional(context, character, character2, autoActionFavorChange, 5);
					lifeRecordCollection.AddVillagerFavorabilityUp(Character.GetId(), currDate, character.GetId(), character2.GetId());
					lifeRecordCollection.AddVillagerFavorabilityUpPerson(character.GetId(), currDate, Character.GetId(), character2.GetId());
				}
				else
				{
					DomainManager.Character.ChangeFavorabilityOptional(context, character, character2, -autoActionFavorChange, 5);
					lifeRecordCollection.AddVillagerFavorabilityDown(Character.GetId(), currDate, character.GetId(), character2.GetId());
					lifeRecordCollection.AddVillagerFavorabilityDownPerson(character.GetId(), currDate, Character.GetId(), character2.GetId());
				}
				TryCreateRelations(context, character, character2, flag);
			}
		}
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list);
	}

	private bool TryCreateRelations(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar, bool isIncrease)
	{
		return (isIncrease ? HandleMakeFriend(context, character, relatedChar) : HandleMakeEnemy(context, character, relatedChar)) || HandleAdore(context, character, relatedChar) || HandleMarriage(context, character, relatedChar) || HandleSwornBrotherAndSister(context, character, relatedChar) || HandleAdoption(context, character, relatedChar) || HandleAdoption(context, relatedChar, character);
	}

	private bool CheckRelationShouldStart(DataContext context, AiRelationsItem relationCfg, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar, bool useMax = true)
	{
		RelatedCharacter relation = DomainManager.Character.GetRelation(character.GetId(), relatedChar.GetId());
		sbyte sectFavorability = DomainManager.Organization.GetSectFavorability(character.GetOrganizationInfo().OrgTemplateId, relatedChar.GetOrganizationInfo().OrgTemplateId);
		int startOrEndRelationChance = AiHelper.Relation.GetStartOrEndRelationChance(relationCfg, character, relatedChar, relation.RelationType, sectFavorability);
		relation = DomainManager.Character.GetRelation(relatedChar.GetId(), character.GetId());
		sectFavorability = DomainManager.Organization.GetSectFavorability(relatedChar.GetOrganizationInfo().OrgTemplateId, character.GetOrganizationInfo().OrgTemplateId);
		startOrEndRelationChance = (useMax ? new Func<int, int, int>(Math.Max) : new Func<int, int, int>(Math.Min))(startOrEndRelationChance, AiHelper.Relation.GetStartOrEndRelationChance(relationCfg, relatedChar, character, relation.RelationType, sectFavorability));
		return context.Random.CheckPercentProb(startOrEndRelationChance);
	}

	private bool HandleMakeFriend(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
	{
		int currDate = DomainManager.World.GetCurrDate();
		Location location = Character.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = Character.GetId();
		int id2 = character.GetId();
		int id3 = relatedChar.GetId();
		if (!AiHelper.Relation.CanStartRelation(character, relatedChar, 8192) || !AiHelper.Relation.CanStartRelation(relatedChar, character, 8192))
		{
			return false;
		}
		if (!CheckRelationShouldStart(context, AiRelations.DefValue.StartFriendRelation, character, relatedChar))
		{
			return false;
		}
		sbyte behaviorType = character.GetBehaviorType();
		GameData.Domains.Character.Character.ApplyBecomeFriend(context, character, relatedChar, behaviorType, selfIsTaiwuPeople: true, targetIsTaiwuPeople: true);
		lifeRecordCollection.AddVillagerMakeFriends(id, currDate, id2, id3, location);
		return true;
	}

	private bool HandleMakeEnemy(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
	{
		int currDate = DomainManager.World.GetCurrDate();
		Location location = Character.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = Character.GetId();
		int id2 = character.GetId();
		int id3 = relatedChar.GetId();
		if (!AiHelper.Relation.CanStartRelation(character, relatedChar, 32768))
		{
			return false;
		}
		if (!CheckRelationShouldStart(context, AiRelations.DefValue.StartEnemyRelation, character, relatedChar))
		{
			return false;
		}
		GameData.Domains.Character.Character.ApplyAddRelation_Enemy(context, character, relatedChar, selfIsTaiwuPeople: true, 0);
		lifeRecordCollection.AddVillagerMakeEnemy(id, currDate, id2, id3, location);
		return true;
	}

	private bool HandleAdore(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
	{
		int currDate = DomainManager.World.GetCurrDate();
		Location location = Character.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = Character.GetId();
		sbyte behaviorType = character.GetBehaviorType();
		if (character.GetAgeGroup() != 2)
		{
			return false;
		}
		if (relatedChar.GetAgeGroup() != 2)
		{
			return false;
		}
		int id2 = character.GetId();
		int id3 = relatedChar.GetId();
		RelatedCharacter relation = DomainManager.Character.GetRelation(id2, id3);
		RelatedCharacter relation2 = DomainManager.Character.GetRelation(id3, id2);
		if (!CheckRelationShouldStart(context, AiRelations.DefValue.StartAdoredRelation, character, relatedChar, useMax: false))
		{
			return false;
		}
		bool flag = RelationType.HasRelation(relation.RelationType, 16384);
		bool flag2 = RelationType.HasRelation(relation2.RelationType, 16384);
		if (flag && flag2)
		{
			return false;
		}
		bool flag3 = flag || AiHelper.Relation.CanStartRelation(character, relatedChar, 16384);
		bool flag4 = flag2 || AiHelper.Relation.CanStartRelation(relatedChar, character, 16384);
		if (!(flag3 && flag4))
		{
			return false;
		}
		int percentProb = Math.Min(flag ? 100 : AiHelper.Relation.GetStartRelationSuccessRate_Adored(character, relatedChar, relation, relation2), flag2 ? 100 : AiHelper.Relation.GetStartRelationSuccessRate_Adored(relatedChar, character, relation2, relation));
		if (!context.Random.CheckPercentProb(percentProb))
		{
			return false;
		}
		GameData.Domains.Character.Character.ApplyAddRelation_Adore(context, character, relatedChar, behaviorType, targetLovesBack: true, selfIsTaiwuPeople: true, targetIsTaiwuPeople: true);
		lifeRecordCollection.AddVillagerConfessLoveSucceed(id, currDate, id2, id3, location);
		return true;
	}

	private bool HandleMarriage(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
	{
		int currDate = DomainManager.World.GetCurrDate();
		Location location = Character.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = Character.GetId();
		sbyte behaviorType = character.GetBehaviorType();
		if (character.GetAgeGroup() != 2)
		{
			return false;
		}
		if (relatedChar.GetAgeGroup() != 2)
		{
			return false;
		}
		if (!character.OrgAndMonkTypeAllowMarriage())
		{
			return false;
		}
		if (!relatedChar.OrgAndMonkTypeAllowMarriage())
		{
			return false;
		}
		int id2 = character.GetId();
		int id3 = relatedChar.GetId();
		if (!AiHelper.Relation.CanStartRelation(character, relatedChar, 1024))
		{
			return false;
		}
		if (!CheckRelationShouldStart(context, AiRelations.DefValue.StartHusbandOrWifeRelation, character, relatedChar))
		{
			return false;
		}
		RelatedCharacter relation = DomainManager.Character.GetRelation(id2, id3);
		RelatedCharacter relation2 = DomainManager.Character.GetRelation(id3, id2);
		int percentProb = Math.Max(AiHelper.Relation.GetStartRelationSuccessRate_HusbandOrWife(character, relatedChar, relation, relation2), AiHelper.Relation.GetStartRelationSuccessRate_HusbandOrWife(relatedChar, character, relation2, relation));
		if (!context.Random.CheckPercentProb(percentProb))
		{
			return false;
		}
		GameData.Domains.Character.Character.ApplyBecomeHusbandOrWife(context, character, relatedChar, behaviorType, succeed: true, selfIsTaiwuPeople: true, targetIsTaiwuPeople: true);
		lifeRecordCollection.AddVillagerGetMarried(id, currDate, id2, id3, location);
		return true;
	}

	private bool HandleSwornBrotherAndSister(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
	{
		int currDate = DomainManager.World.GetCurrDate();
		Location location = Character.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = Character.GetId();
		int id2 = character.GetId();
		int id3 = relatedChar.GetId();
		if (!AiHelper.Relation.CanStartRelation(character, relatedChar, 512))
		{
			return false;
		}
		if (!CheckRelationShouldStart(context, AiRelations.DefValue.StartSwornBrotherOrSisterRelation, character, relatedChar))
		{
			return false;
		}
		RelatedCharacter relation = DomainManager.Character.GetRelation(id2, id3);
		RelatedCharacter relation2 = DomainManager.Character.GetRelation(id3, id2);
		bool flag = RelationType.HasRelation(relation.RelationType, 16384);
		bool flag2 = RelationType.HasRelation(relation2.RelationType, 16384);
		if (flag || flag2)
		{
			return false;
		}
		int percentProb = Math.Max(AiHelper.Relation.GetStartRelationSuccessRate_HusbandOrWife(character, relatedChar, relation, relation2), AiHelper.Relation.GetStartRelationSuccessRate_HusbandOrWife(relatedChar, character, relation2, relation));
		if (!context.Random.CheckPercentProb(percentProb))
		{
			return false;
		}
		sbyte behaviorType = character.GetBehaviorType();
		GameData.Domains.Character.Character.ApplyBecomeSwornBrotherOrSister(context, character, relatedChar, behaviorType, selfIsTaiwuPeople: true, targetIsTaiwuPeople: true);
		lifeRecordCollection.AddVillagerBecomeBrothers(id, currDate, id2, id3, location);
		return true;
	}

	private bool HandleAdoption(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
	{
		int currDate = DomainManager.World.GetCurrDate();
		Location location = Character.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int id = Character.GetId();
		int id2 = character.GetId();
		int id3 = relatedChar.GetId();
		if (!AiHelper.Relation.CanStartRelation(character, relatedChar, 128) && !AiHelper.Relation.CanStartRelation(relatedChar, character, 128))
		{
			return false;
		}
		if (!CheckRelationShouldStart(context, AiRelations.DefValue.AdoptingRelation, character, relatedChar))
		{
			return false;
		}
		sbyte behaviorType = character.GetBehaviorType();
		GameData.Domains.Character.Character.ApplyAddRelation_AdoptiveChild(context, character, relatedChar, behaviorType, selfIsTaiwuPeople: true, targetIsTaiwuPeople: true);
		lifeRecordCollection.AddVillagerAdopt(id, currDate, id2, id3, location);
		return true;
	}

	void IVillagerRoleArrangementExecutor.ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action)
	{
		if (TryGetWorkingStateCustomizeKey(out var _))
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			bool removedAny;
			int authorityCost = GetAuthorityCost(removeExceeded: true, out removedAny);
			taiwu.ChangeResource(context, 7, -authorityCost);
		}
	}

	public int GetAuthorityCost(bool removeExceeded, out bool removedAny)
	{
		removedAny = false;
		if (!TryGetWorkingStateCustomizeKey(out var key))
		{
			return 0;
		}
		(sbyte stateTemplateId, bool isSect) tuple = PunishmentSeverityCustomizeData.DecodePunishmentSeverityCustomizeKey(key);
		sbyte item = tuple.stateTemplateId;
		bool item2 = tuple.isSect;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int resource = taiwu.GetResource(7);
		int num = 0;
		if (DomainManager.Extra.TryGetElement_CityPunishmentSeverityCustomizeDict(key, out var value))
		{
			List<PunishmentSeverityCustomizeData> items = value.Items;
			if (items != null && items.Count > 0)
			{
				for (int num2 = value.Items.Count - 1; num2 >= 0; num2--)
				{
					PunishmentSeverityCustomizeData punishmentSeverityCustomizeData = value.Items[num2];
					PunishmentTypeItem punishmentTypeItem = PunishmentType.Instance[punishmentSeverityCustomizeData.PunishmentTypeTemplateId];
					sbyte severity = punishmentTypeItem.GetSeverity(item, item2);
					int num3 = Math.Abs(severity - punishmentSeverityCustomizeData.CustomizedPunishmentSeverityTemplateId);
					if (num3 > GlobalConfig.Instance.ModifySeverityDefaultRange)
					{
						int baseCost = (severity + 1) * GlobalConfig.Instance.ModifySeverityCostFactor;
						int num4 = CalcModificationAuthorityCost(baseCost);
						if (removeExceeded && num4 + num > resource)
						{
							value.Items.RemoveAt(num2);
							removedAny = true;
						}
						else
						{
							num += num4;
						}
					}
				}
			}
		}
		return num;
	}

	public bool TryGetWorkingStateCustomizeKey(out short key)
	{
		key = 0;
		if (WorkData == null || WorkData.AreaId < 0)
		{
			return false;
		}
		short templateId = DomainManager.Map.GetElement_Areas(WorkData.AreaId).GetTemplateId();
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(WorkData.AreaId);
		MapStateItem mapStateItem = MapState.Instance[stateTemplateIdByAreaId];
		if (mapStateItem.MainAreaID == templateId)
		{
			key = PunishmentSeverityCustomizeData.GetPunishmentSeverityCustomizeKey(stateTemplateIdByAreaId, isSect: false);
			return true;
		}
		if (mapStateItem.SectAreaID == templateId)
		{
			key = PunishmentSeverityCustomizeData.GetPunishmentSeverityCustomizeKey(stateTemplateIdByAreaId, isSect: true);
			return true;
		}
		return false;
	}

	public int CalcModificationAuthorityCost(int baseCost)
	{
		return Math.Max(0, VillagerRoleFormulaImpl.Calculate(16, baseCost, Character.GetLifeSkillAttainment(4)));
	}

	public override IVillagerRoleArrangementDisplayData GetArrangementDisplayData()
	{
		return new TaiwuEnvoyDisplayData
		{
			SpecialRuleCount = ArrangementSpecialRuleCount,
			MonthlyAuthorityCost = CalcModificationAuthorityCost(100)
		};
	}
}
