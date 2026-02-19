using System;
using Config;
using GameData.Domains.Organization;

namespace GameData.Domains.Character;

public static class CharacterMatcherHelper
{
	public static bool Match(this CharacterMatcherItem matcherItem, Character character)
	{
		if (character.GetCreatingType() != 1)
		{
			return false;
		}
		if (!matcherItem.AgeType.Match(character))
		{
			return false;
		}
		if (!matcherItem.GenderType.Match(character))
		{
			return false;
		}
		if (!matcherItem.IdentityType.Match(character))
		{
			return false;
		}
		if (matcherItem.Organization >= 0 && character.GetOrganizationInfo().OrgTemplateId != matcherItem.Organization)
		{
			return false;
		}
		ECharacterMatcherSubCondition[] subConditions = matcherItem.SubConditions;
		if (subConditions == null || subConditions.Length <= 0)
		{
			return true;
		}
		for (int i = 0; i < matcherItem.SubConditions.Length; i++)
		{
			if (!matcherItem.SubConditions[i].Match(character))
			{
				return false;
			}
		}
		return true;
	}

	public static bool Match(this ECharacterMatcherAgeType ageType, Character character)
	{
		if (1 == 0)
		{
		}
		bool result = ageType switch
		{
			ECharacterMatcherAgeType.NotRestricted => true, 
			ECharacterMatcherAgeType.Baby => character.GetAgeGroup() == 0, 
			ECharacterMatcherAgeType.Child => character.GetAgeGroup() == 1, 
			ECharacterMatcherAgeType.Adult => character.GetAgeGroup() == 2, 
			ECharacterMatcherAgeType.NonBaby => character.GetAgeGroup() != 0, 
			_ => throw new ArgumentOutOfRangeException("ageType", ageType, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static bool Match(this ECharacterMatcherGenderType genderType, Character character)
	{
		if (1 == 0)
		{
		}
		bool result = genderType switch
		{
			ECharacterMatcherGenderType.NotRestricted => true, 
			ECharacterMatcherGenderType.Female => character.GetGender() == 0, 
			ECharacterMatcherGenderType.Male => character.GetGender() == 1, 
			ECharacterMatcherGenderType.DisplayFemale => character.GetDisplayingGender() == 0, 
			ECharacterMatcherGenderType.DisplayMale => character.GetDisplayingGender() == 1, 
			_ => throw new ArgumentOutOfRangeException("genderType", genderType, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static bool Match(this ECharacterMatcherIdentityType identityType, Character character)
	{
		if (1 == 0)
		{
		}
		bool result = identityType switch
		{
			ECharacterMatcherIdentityType.NotRestricted => true, 
			ECharacterMatcherIdentityType.Sect => OrganizationDomain.IsSect(character.GetOrganizationInfo().OrgTemplateId), 
			ECharacterMatcherIdentityType.CivilianSettlement => Config.Organization.Instance[character.GetOrganizationInfo().OrgTemplateId]?.IsCivilian ?? false, 
			ECharacterMatcherIdentityType.NotSect => !OrganizationDomain.IsSect(character.GetOrganizationInfo().OrgTemplateId), 
			ECharacterMatcherIdentityType.NotTaiwuVillage => character.GetOrganizationInfo().OrgTemplateId != 16, 
			ECharacterMatcherIdentityType.NoOrg => character.GetOrganizationInfo().OrgTemplateId == 0, 
			ECharacterMatcherIdentityType.XiangshuInfected => character.GetOrganizationInfo().OrgTemplateId == 20, 
			ECharacterMatcherIdentityType.NotXiangshuInfected => character.GetOrganizationInfo().OrgTemplateId != 20, 
			_ => throw new ArgumentOutOfRangeException("identityType", identityType, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static bool Match(this ECharacterMatcherSubCondition subCondition, Character character)
	{
		switch (subCondition)
		{
		case ECharacterMatcherSubCondition.NotActingCrazy:
			return !DomainManager.LegendaryBook.IsCharacterActingCrazy(character) && !character.IsCompletelyInfected();
		case ECharacterMatcherSubCondition.CanBeLocated:
			return !character.IsActiveExternalRelationState(60) && character.GetKidnapperId() < 0 && (character.GetLocation().IsValid() || character.IsCrossAreaTraveling());
		case ECharacterMatcherSubCondition.NotCrossAreaTraveling:
			return !character.IsCrossAreaTraveling();
		case ECharacterMatcherSubCondition.CanHaveChild:
			return character.OrgAndMonkTypeAllowMarriage();
		case ECharacterMatcherSubCondition.CanStroll:
			return OrganizationDomain.GetOrgMemberConfig(character.GetOrganizationInfo()).CanStroll;
		case ECharacterMatcherSubCondition.NotInOthersGroup:
		{
			int leaderId = character.GetLeaderId();
			return leaderId < 0 || leaderId == character.GetId();
		}
		case ECharacterMatcherSubCondition.NotAssignedWithWork:
			return !DomainManager.Taiwu.VillagerHasWork(character.GetId());
		case ECharacterMatcherSubCondition.NotTaiwu:
			return !character.IsTaiwu();
		case ECharacterMatcherSubCondition.NotInTaiwuGroup:
			return !DomainManager.Taiwu.IsInGroup(character.GetId());
		case ECharacterMatcherSubCondition.NotTaiwuFriendlyRelation:
			return !DomainManager.Character.IsCharacterRelationFriendly(DomainManager.Taiwu.GetTaiwuCharId(), character.GetId());
		case ECharacterMatcherSubCondition.NotHighestGrade:
			return character.GetInteractionGrade() != 8;
		case ECharacterMatcherSubCondition.NotLegendaryBookConsumed:
			return character.GetLegendaryBookOwnerState() != 3;
		case ECharacterMatcherSubCondition.NotInSettlementPrison:
			return !character.IsActiveExternalRelationState(32);
		case ECharacterMatcherSubCondition.CombatPowerTop30:
		{
			int topThousandCharacterRanking = DomainManager.Character.GetTopThousandCharacterRanking(character.GetId());
			return topThousandCharacterRanking >= 0 && topThousandCharacterRanking <= 30;
		}
		case ECharacterMatcherSubCondition.InSettlement:
			return character.GetKidnapperId() < 0 && !character.IsActiveExternalRelationState(60) && character.GetLocation().IsValid() && DomainManager.Map.IsLocationOnSettlementBlock(character.GetLocation(), character.GetOrganizationInfo().SettlementId);
		default:
			throw new ArgumentOutOfRangeException("subCondition", subCondition, null);
		}
	}
}
