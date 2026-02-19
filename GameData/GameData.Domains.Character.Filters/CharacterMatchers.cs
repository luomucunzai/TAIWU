using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Adventure;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Relation;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Utilities;

namespace GameData.Domains.Character.Filters;

public static class CharacterMatchers
{
	public static bool MatchAll(Character character, List<Predicate<Character>> predicates)
	{
		int i = 0;
		for (int count = predicates.Count; i < count; i++)
		{
			if (!predicates[i](character))
			{
				return false;
			}
		}
		return true;
	}

	public static bool MatchNotCalledByAdventure(Character character)
	{
		return !character.IsActiveExternalRelationState(4);
	}

	public static bool MatchHasNoWork(Character character)
	{
		return !character.IsActiveExternalRelationState(1) && !character.IsTreasuryGuard();
	}

	public static bool MatchNotAffectedByLegendaryBook(Character character)
	{
		return character.GetLegendaryBookOwnerState() <= 0;
	}

	public static bool MatchCanBeCalledByAdventure(Character character)
	{
		return !character.IsActiveAdvanceMonthStatus(16);
	}

	public static bool MatchNotTaiwuOwnedLegendaryBook(Character character)
	{
		return character.GetId() != DomainManager.Taiwu.GetTaiwuCharId() && character.GetLegendaryBookOwnerState() >= 0;
	}

	public static bool MatchOrganization(Character character, sbyte orgTemplateId)
	{
		return character.GetOrganizationInfo().OrgTemplateId == orgTemplateId;
	}

	public static bool MatchSettlement(Character character, short settlementId)
	{
		return character.GetOrganizationInfo().SettlementId == settlementId;
	}

	public unsafe static bool MatchGoodAtCombatSkillType(Character character, sbyte combatSkillType)
	{
		CombatSkillShorts combatSkillAttainments = character.GetCombatSkillAttainments();
		int num = 1;
		for (sbyte b = 0; b < 14; b++)
		{
			if (combatSkillAttainments.Items[combatSkillType] < combatSkillAttainments.Items[b])
			{
				num++;
			}
		}
		if (num > 3)
		{
			return false;
		}
		ArraySegmentList<short> attack = character.GetCombatSkillEquipment().Attack;
		ArraySegmentList<short>.Enumerator enumerator = attack.GetEnumerator();
		while (enumerator.MoveNext())
		{
			short current = enumerator.Current;
			if (current < 0 || Config.CombatSkill.Instance[current].Type != combatSkillType)
			{
				continue;
			}
			return true;
		}
		return false;
	}

	public unsafe static bool MatchGoodAtLifeSkillType(Character character, sbyte lifeSkillType)
	{
		LifeSkillShorts lifeSkillAttainments = character.GetLifeSkillAttainments();
		int num = 1;
		for (sbyte b = 0; b < 14; b++)
		{
			if (lifeSkillAttainments.Items[lifeSkillType] < lifeSkillAttainments.Items[b])
			{
				num++;
			}
		}
		if (num > 3)
		{
			return false;
		}
		return lifeSkillAttainments.Items[lifeSkillType] >= 100;
	}

	public static bool MatchRealName(Character character, string name)
	{
		var (text, text2) = CharacterDomain.GetRealName(character);
		return text + text2 == name;
	}

	public static bool MatchConsummateLevel(Character character, sbyte minVal, sbyte maxVal)
	{
		sbyte consummateLevel = character.GetConsummateLevel();
		return consummateLevel >= minVal && consummateLevel <= maxVal;
	}

	public static bool MatchGender(Character character, sbyte gender)
	{
		return character.GetGender() == gender;
	}

	public static bool MatchOrganizationType(Character character, sbyte orgTypeMin, sbyte orgTypeMax)
	{
		sbyte orgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
		sbyte b;
		if (Config.Organization.Instance[orgTemplateId].IsSect)
		{
			b = 0;
		}
		else
		{
			switch (orgTemplateId)
			{
			case 21:
			case 22:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
				b = 1;
				break;
			case 36:
			case 37:
			case 38:
				b = 2;
				break;
			case 16:
				b = 3;
				break;
			default:
				return false;
			}
		}
		return b >= orgTypeMin && b <= orgTypeMax;
	}

	public static bool MatchGrade(Character character, sbyte gradeMin, sbyte gradeMax)
	{
		sbyte grade = character.GetOrganizationInfo().Grade;
		return grade >= gradeMin && grade <= gradeMax;
	}

	public static bool MatchDisplayingAge(Character character, int ageMin, int ageMax)
	{
		short currAge = character.GetCurrAge();
		return currAge >= ageMin && currAge <= ageMax;
	}

	public static bool MatchPhysiologicalAge(Character character, int ageMin, int ageMax)
	{
		short physiologicalAge = character.GetPhysiologicalAge();
		return physiologicalAge >= ageMin && physiologicalAge <= ageMax;
	}

	public static bool MatchBehaviorType(Character character, sbyte minBehaviorType, sbyte maxBehaviorType)
	{
		sbyte behaviorType = character.GetBehaviorType();
		return behaviorType >= minBehaviorType && behaviorType <= maxBehaviorType;
	}

	public static bool MatchAttraction(Character character, int minAttraction, int maxAttraction)
	{
		short attraction = character.GetAttraction();
		return attraction >= minAttraction && attraction <= maxAttraction;
	}

	public static bool MatchHappiness(Character character, int minHappiness, int maxHappiness)
	{
		sbyte happiness = character.GetHappiness();
		return happiness >= minHappiness && happiness <= maxHappiness;
	}

	public static bool MatchResource(Character character, sbyte resourceType, int minAmount, int maxAmount)
	{
		int resource = character.GetResource(resourceType);
		return resource >= minAmount && resource <= maxAmount;
	}

	public static bool MatchHairNoSkinHead(Character character)
	{
		AvatarData avatar = character.GetAvatar();
		return avatar.GetGrowableElementShowingState(0);
	}

	public static bool MatchLifeSkillAttainment(Character character, sbyte lifeSkillType, int minVal, int maxVal)
	{
		short lifeSkillAttainment = character.GetLifeSkillAttainment(lifeSkillType);
		return lifeSkillAttainment >= minVal && lifeSkillAttainment <= maxVal;
	}

	public static bool MatchCombatSkillAttainment(Character character, sbyte combatSkillType, int minVal, int maxVal)
	{
		short combatSkillAttainment = character.GetCombatSkillAttainment(combatSkillType);
		return combatSkillAttainment >= minVal && combatSkillAttainment <= maxVal;
	}

	public static bool MatchIsMonk(Character character)
	{
		return character.GetMonkType() != 0;
	}

	public static bool MatchOrgMemberAllowMarriage(Character character)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(organizationInfo);
		return orgMemberConfig.ChildGrade >= 0 && organizationInfo.Principal;
	}

	public static bool MatchCombatPowerRankInSect(Character character, int minRank, int maxRank)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		if (organizationInfo.SettlementId < 0)
		{
			return false;
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(organizationInfo.SettlementId);
		int characterRanking = settlement.GetCharacterRanking(character.GetId());
		return characterRanking >= minRank && characterRanking <= maxRank;
	}

	public static bool MatchTopThousandCombatPowerRank(Character character, int minRank, int maxRank)
	{
		int topThousandCharacterRanking = DomainManager.Character.GetTopThousandCharacterRanking(character.GetId());
		return topThousandCharacterRanking >= minRank && topThousandCharacterRanking <= maxRank;
	}

	public static bool MatchPrincipal(Character character)
	{
		return character.GetOrganizationInfo().Principal;
	}

	public static bool MatchSettlementLeader(Character character)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		return organizationInfo.Grade == 8 && organizationInfo.Principal;
	}

	public static bool MatchSettlingState(Character character, int minStateTemplateId, int maxStateTemplateId)
	{
		short settlementId = character.GetOrganizationInfo().SettlementId;
		if (settlementId < 0)
		{
			return false;
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		short areaId = settlement.GetLocation().AreaId;
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(areaId);
		return stateTemplateIdByAreaId >= minStateTemplateId && stateTemplateIdByAreaId <= maxStateTemplateId;
	}

	public static bool MatchAncestry(Character character, int stateTemplateId)
	{
		short templateId = character.GetTemplateId();
		MapStateItem mapStateItem = MapState.Instance[stateTemplateId];
		if (CollectionUtils.Contains(mapStateItem.TemplateCharacterIds, templateId))
		{
			return true;
		}
		bool flag = mapStateItem.SectID == 11;
		bool flag2 = flag;
		if (flag2)
		{
			bool flag3 = (uint)(templateId - 30) <= 1u;
			flag2 = flag3;
		}
		return flag2;
	}

	public static bool MatchMaritalStatus(Character character, bool isMarried)
	{
		bool flag = DomainManager.Character.GetAliveSpouse(character.GetId()) >= 0;
		return flag == isMarried;
	}

	public static bool MatchCompletelyInfected(Character character)
	{
		return character.IsCompletelyInfected();
	}

	public unsafe static bool MatchPreexistenceChar(Character character, int preexistenceCharId)
	{
		PreexistenceCharIds preexistenceCharIds = character.GetPreexistenceCharIds();
		int i = 0;
		for (int count = preexistenceCharIds.Count; i < count; i++)
		{
			if (preexistenceCharIds.CharIds[i] == preexistenceCharId)
			{
				return true;
			}
		}
		return false;
	}

	public static bool MatchRelationTypeId(Character character, Character relatedCharacter, sbyte minId, sbyte maxId)
	{
		if (!DomainManager.Character.TryGetRelation(character.GetId(), relatedCharacter.GetId(), out var relation))
		{
			return minId <= -1;
		}
		sbyte typeId = RelationType.GetTypeId(relation.RelationType);
		return typeId >= minId && typeId <= maxId;
	}

	public static bool MatchHasInventoryItem(Character character, sbyte itemType, short templateId, int expectedAmount)
	{
		Dictionary<ItemKey, int> items = character.GetInventory().Items;
		bool result = false;
		foreach (var (itemKey2, num2) in items)
		{
			if (itemKey2.ItemType != itemType || itemKey2.TemplateId != templateId)
			{
				continue;
			}
			if (num2 < expectedAmount)
			{
				return false;
			}
			result = true;
			break;
		}
		return result;
	}

	public static bool MatchAtVisibleAdventureSite(Character character, short adventureTemplateId)
	{
		Location location = character.GetLocation();
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
		AdventureSiteData value;
		return adventuresInArea.AdventureSites.TryGetValue(location.BlockId, out value) && value.TemplateId == adventureTemplateId && value.SiteState == 1;
	}

	public static bool MatchMonasticTitleOrDisplayName(Character character, string name)
	{
		NameRelatedData data = new NameRelatedData();
		CharacterDomain.GetNameRelatedData(character, ref data);
		var (text, text2) = data.GetMonasticTitleOrDisplayNameDetailed(isTaiwu: false, ignoreNickName: true);
		return name == text + text2;
	}
}
