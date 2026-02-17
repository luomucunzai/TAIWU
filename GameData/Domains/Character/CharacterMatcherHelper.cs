using System;
using Config;
using GameData.Domains.Organization;

namespace GameData.Domains.Character
{
	// Token: 0x0200080A RID: 2058
	public static class CharacterMatcherHelper
	{
		// Token: 0x0600745C RID: 29788 RVA: 0x00442E58 File Offset: 0x00441058
		public static bool Match(this CharacterMatcherItem matcherItem, Character character)
		{
			bool flag = character.GetCreatingType() != 1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !matcherItem.AgeType.Match(character);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = !matcherItem.GenderType.Match(character);
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = !matcherItem.IdentityType.Match(character);
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = matcherItem.Organization >= 0 && character.GetOrganizationInfo().OrgTemplateId != matcherItem.Organization;
							if (flag5)
							{
								result = false;
							}
							else
							{
								ECharacterMatcherSubCondition[] subConditions = matcherItem.SubConditions;
								bool flag6 = subConditions == null || subConditions.Length <= 0;
								if (flag6)
								{
									result = true;
								}
								else
								{
									for (int i = 0; i < matcherItem.SubConditions.Length; i++)
									{
										bool flag7 = !matcherItem.SubConditions[i].Match(character);
										if (flag7)
										{
											return false;
										}
									}
									result = true;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600745D RID: 29789 RVA: 0x00442F5C File Offset: 0x0044115C
		public static bool Match(this ECharacterMatcherAgeType ageType, Character character)
		{
			if (!true)
			{
			}
			bool result;
			switch (ageType)
			{
			case ECharacterMatcherAgeType.NotRestricted:
				result = true;
				break;
			case ECharacterMatcherAgeType.Baby:
				result = (character.GetAgeGroup() == 0);
				break;
			case ECharacterMatcherAgeType.Child:
				result = (character.GetAgeGroup() == 1);
				break;
			case ECharacterMatcherAgeType.Adult:
				result = (character.GetAgeGroup() == 2);
				break;
			case ECharacterMatcherAgeType.NonBaby:
				result = (character.GetAgeGroup() != 0);
				break;
			default:
				throw new ArgumentOutOfRangeException("ageType", ageType, null);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x0600745E RID: 29790 RVA: 0x00442FDC File Offset: 0x004411DC
		public static bool Match(this ECharacterMatcherGenderType genderType, Character character)
		{
			if (!true)
			{
			}
			bool result;
			switch (genderType)
			{
			case ECharacterMatcherGenderType.NotRestricted:
				result = true;
				break;
			case ECharacterMatcherGenderType.Female:
				result = (character.GetGender() == 0);
				break;
			case ECharacterMatcherGenderType.Male:
				result = (character.GetGender() == 1);
				break;
			case ECharacterMatcherGenderType.DisplayFemale:
				result = (character.GetDisplayingGender() == 0);
				break;
			case ECharacterMatcherGenderType.DisplayMale:
				result = (character.GetDisplayingGender() == 1);
				break;
			default:
				throw new ArgumentOutOfRangeException("genderType", genderType, null);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x0600745F RID: 29791 RVA: 0x0044305C File Offset: 0x0044125C
		public static bool Match(this ECharacterMatcherIdentityType identityType, Character character)
		{
			if (!true)
			{
			}
			bool result;
			switch (identityType)
			{
			case ECharacterMatcherIdentityType.NotRestricted:
				result = true;
				break;
			case ECharacterMatcherIdentityType.Sect:
				result = OrganizationDomain.IsSect(character.GetOrganizationInfo().OrgTemplateId);
				break;
			case ECharacterMatcherIdentityType.CivilianSettlement:
			{
				OrganizationItem organizationItem = Organization.Instance[character.GetOrganizationInfo().OrgTemplateId];
				result = (organizationItem != null && organizationItem.IsCivilian);
				break;
			}
			case ECharacterMatcherIdentityType.NotSect:
				result = !OrganizationDomain.IsSect(character.GetOrganizationInfo().OrgTemplateId);
				break;
			case ECharacterMatcherIdentityType.NotTaiwuVillage:
				result = (character.GetOrganizationInfo().OrgTemplateId != 16);
				break;
			case ECharacterMatcherIdentityType.NoOrg:
				result = (character.GetOrganizationInfo().OrgTemplateId == 0);
				break;
			case ECharacterMatcherIdentityType.XiangshuInfected:
				result = (character.GetOrganizationInfo().OrgTemplateId == 20);
				break;
			case ECharacterMatcherIdentityType.NotXiangshuInfected:
				result = (character.GetOrganizationInfo().OrgTemplateId != 20);
				break;
			default:
				throw new ArgumentOutOfRangeException("identityType", identityType, null);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06007460 RID: 29792 RVA: 0x0044315C File Offset: 0x0044135C
		public static bool Match(this ECharacterMatcherSubCondition subCondition, Character character)
		{
			bool result;
			switch (subCondition)
			{
			case ECharacterMatcherSubCondition.NotActingCrazy:
				result = (!DomainManager.LegendaryBook.IsCharacterActingCrazy(character) && !character.IsCompletelyInfected());
				break;
			case ECharacterMatcherSubCondition.CanBeLocated:
				result = (!character.IsActiveExternalRelationState(60) && character.GetKidnapperId() < 0 && (character.GetLocation().IsValid() || character.IsCrossAreaTraveling()));
				break;
			case ECharacterMatcherSubCondition.CanHaveChild:
				result = character.OrgAndMonkTypeAllowMarriage();
				break;
			case ECharacterMatcherSubCondition.CanStroll:
				result = OrganizationDomain.GetOrgMemberConfig(character.GetOrganizationInfo()).CanStroll;
				break;
			case ECharacterMatcherSubCondition.NotInOthersGroup:
			{
				int leaderId = character.GetLeaderId();
				result = (leaderId < 0 || leaderId == character.GetId());
				break;
			}
			case ECharacterMatcherSubCondition.NotAssignedWithWork:
				result = !DomainManager.Taiwu.VillagerHasWork(character.GetId());
				break;
			case ECharacterMatcherSubCondition.NotTaiwu:
				result = !character.IsTaiwu();
				break;
			case ECharacterMatcherSubCondition.NotInTaiwuGroup:
				result = !DomainManager.Taiwu.IsInGroup(character.GetId());
				break;
			case ECharacterMatcherSubCondition.NotTaiwuFriendlyRelation:
				result = !DomainManager.Character.IsCharacterRelationFriendly(DomainManager.Taiwu.GetTaiwuCharId(), character.GetId());
				break;
			case ECharacterMatcherSubCondition.NotHighestGrade:
				result = (character.GetInteractionGrade() != 8);
				break;
			case ECharacterMatcherSubCondition.NotLegendaryBookConsumed:
				result = (character.GetLegendaryBookOwnerState() != 3);
				break;
			case ECharacterMatcherSubCondition.NotInSettlementPrison:
				result = !character.IsActiveExternalRelationState(32);
				break;
			case ECharacterMatcherSubCondition.CombatPowerTop30:
			{
				int topThousandCharacterRanking = DomainManager.Character.GetTopThousandCharacterRanking(character.GetId());
				result = (topThousandCharacterRanking >= 0 && topThousandCharacterRanking <= 30);
				break;
			}
			case ECharacterMatcherSubCondition.NotCrossAreaTraveling:
				result = !character.IsCrossAreaTraveling();
				break;
			case ECharacterMatcherSubCondition.InSettlement:
				result = (character.GetKidnapperId() < 0 && !character.IsActiveExternalRelationState(60) && character.GetLocation().IsValid() && DomainManager.Map.IsLocationOnSettlementBlock(character.GetLocation(), character.GetOrganizationInfo().SettlementId));
				break;
			default:
				throw new ArgumentOutOfRangeException("subCondition", subCondition, null);
			}
			return result;
		}
	}
}
