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

namespace GameData.Domains.Character.Filters
{
	// Token: 0x0200083A RID: 2106
	public static class CharacterMatchers
	{
		// Token: 0x06007589 RID: 30089 RVA: 0x0044B480 File Offset: 0x00449680
		public static bool MatchAll(Character character, List<Predicate<Character>> predicates)
		{
			int i = 0;
			int count = predicates.Count;
			while (i < count)
			{
				bool flag = !predicates[i](character);
				if (flag)
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x0600758A RID: 30090 RVA: 0x0044B4C4 File Offset: 0x004496C4
		public static bool MatchNotCalledByAdventure(Character character)
		{
			return !character.IsActiveExternalRelationState(4);
		}

		// Token: 0x0600758B RID: 30091 RVA: 0x0044B4E0 File Offset: 0x004496E0
		public static bool MatchHasNoWork(Character character)
		{
			return !character.IsActiveExternalRelationState(1) && !character.IsTreasuryGuard();
		}

		// Token: 0x0600758C RID: 30092 RVA: 0x0044B508 File Offset: 0x00449708
		public static bool MatchNotAffectedByLegendaryBook(Character character)
		{
			return character.GetLegendaryBookOwnerState() <= 0;
		}

		// Token: 0x0600758D RID: 30093 RVA: 0x0044B528 File Offset: 0x00449728
		public static bool MatchCanBeCalledByAdventure(Character character)
		{
			return !character.IsActiveAdvanceMonthStatus(16);
		}

		// Token: 0x0600758E RID: 30094 RVA: 0x0044B548 File Offset: 0x00449748
		public static bool MatchNotTaiwuOwnedLegendaryBook(Character character)
		{
			return character.GetId() != DomainManager.Taiwu.GetTaiwuCharId() && character.GetLegendaryBookOwnerState() >= 0;
		}

		// Token: 0x0600758F RID: 30095 RVA: 0x0044B57C File Offset: 0x0044977C
		public static bool MatchOrganization(Character character, sbyte orgTemplateId)
		{
			return character.GetOrganizationInfo().OrgTemplateId == orgTemplateId;
		}

		// Token: 0x06007590 RID: 30096 RVA: 0x0044B59C File Offset: 0x0044979C
		public static bool MatchSettlement(Character character, short settlementId)
		{
			return character.GetOrganizationInfo().SettlementId == settlementId;
		}

		// Token: 0x06007591 RID: 30097 RVA: 0x0044B5BC File Offset: 0x004497BC
		public unsafe static bool MatchGoodAtCombatSkillType(Character character, sbyte combatSkillType)
		{
			CombatSkillShorts combatSkillAttainments = *character.GetCombatSkillAttainments();
			int ranking = 1;
			for (sbyte index = 0; index < 14; index += 1)
			{
				bool flag = *(ref combatSkillAttainments.Items.FixedElementField + (IntPtr)combatSkillType * 2) < *(ref combatSkillAttainments.Items.FixedElementField + (IntPtr)index * 2);
				if (flag)
				{
					ranking++;
				}
			}
			bool flag2 = ranking > 3;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				ArraySegmentList<short> attackSkills = character.GetCombatSkillEquipment().Attack;
				foreach (short ptr in attackSkills)
				{
					short skillTemplateId = ptr;
					bool flag3 = skillTemplateId < 0;
					if (!flag3)
					{
						bool flag4 = CombatSkill.Instance[skillTemplateId].Type == combatSkillType;
						if (flag4)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06007592 RID: 30098 RVA: 0x0044B690 File Offset: 0x00449890
		public unsafe static bool MatchGoodAtLifeSkillType(Character character, sbyte lifeSkillType)
		{
			LifeSkillShorts lifeSkillAttainments = *character.GetLifeSkillAttainments();
			int ranking = 1;
			for (sbyte index = 0; index < 14; index += 1)
			{
				bool flag = *(ref lifeSkillAttainments.Items.FixedElementField + (IntPtr)lifeSkillType * 2) < *(ref lifeSkillAttainments.Items.FixedElementField + (IntPtr)index * 2);
				if (flag)
				{
					ranking++;
				}
			}
			bool flag2 = ranking > 3;
			return !flag2 && *(ref lifeSkillAttainments.Items.FixedElementField + (IntPtr)lifeSkillType * 2) >= 100;
		}

		// Token: 0x06007593 RID: 30099 RVA: 0x0044B720 File Offset: 0x00449920
		public static bool MatchRealName(Character character, string name)
		{
			ValueTuple<string, string> realName = CharacterDomain.GetRealName(character);
			string surname = realName.Item1;
			string givenName = realName.Item2;
			return surname + givenName == name;
		}

		// Token: 0x06007594 RID: 30100 RVA: 0x0044B754 File Offset: 0x00449954
		public static bool MatchConsummateLevel(Character character, sbyte minVal, sbyte maxVal)
		{
			sbyte val = character.GetConsummateLevel();
			return val >= minVal && val <= maxVal;
		}

		// Token: 0x06007595 RID: 30101 RVA: 0x0044B77C File Offset: 0x0044997C
		public static bool MatchGender(Character character, sbyte gender)
		{
			return character.GetGender() == gender;
		}

		// Token: 0x06007596 RID: 30102 RVA: 0x0044B798 File Offset: 0x00449998
		public static bool MatchOrganizationType(Character character, sbyte orgTypeMin, sbyte orgTypeMax)
		{
			sbyte orgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
			bool isSect = Organization.Instance[orgTemplateId].IsSect;
			sbyte organizationType;
			if (!isSect)
			{
				sbyte b = orgTemplateId;
				sbyte b2 = b;
				if (b2 <= 35)
				{
					if (b2 >= 21)
					{
						organizationType = 1;
						goto IL_56;
					}
					if (b2 == 16)
					{
						organizationType = 3;
						goto IL_56;
					}
				}
				else if (b2 - 36 <= 2)
				{
					organizationType = 2;
					goto IL_56;
				}
				return false;
			}
			organizationType = 0;
			IL_56:
			return organizationType >= orgTypeMin && organizationType <= orgTypeMax;
		}

		// Token: 0x06007597 RID: 30103 RVA: 0x0044B810 File Offset: 0x00449A10
		public static bool MatchGrade(Character character, sbyte gradeMin, sbyte gradeMax)
		{
			sbyte grade = character.GetOrganizationInfo().Grade;
			return grade >= gradeMin && grade <= gradeMax;
		}

		// Token: 0x06007598 RID: 30104 RVA: 0x0044B83C File Offset: 0x00449A3C
		public static bool MatchDisplayingAge(Character character, int ageMin, int ageMax)
		{
			short displayingAge = character.GetCurrAge();
			return (int)displayingAge >= ageMin && (int)displayingAge <= ageMax;
		}

		// Token: 0x06007599 RID: 30105 RVA: 0x0044B864 File Offset: 0x00449A64
		public static bool MatchPhysiologicalAge(Character character, int ageMin, int ageMax)
		{
			short physiologicalAge = character.GetPhysiologicalAge();
			return (int)physiologicalAge >= ageMin && (int)physiologicalAge <= ageMax;
		}

		// Token: 0x0600759A RID: 30106 RVA: 0x0044B88C File Offset: 0x00449A8C
		public static bool MatchBehaviorType(Character character, sbyte minBehaviorType, sbyte maxBehaviorType)
		{
			sbyte behaviorType = character.GetBehaviorType();
			return behaviorType >= minBehaviorType && behaviorType <= maxBehaviorType;
		}

		// Token: 0x0600759B RID: 30107 RVA: 0x0044B8B4 File Offset: 0x00449AB4
		public static bool MatchAttraction(Character character, int minAttraction, int maxAttraction)
		{
			short attraction = character.GetAttraction();
			return (int)attraction >= minAttraction && (int)attraction <= maxAttraction;
		}

		// Token: 0x0600759C RID: 30108 RVA: 0x0044B8DC File Offset: 0x00449ADC
		public static bool MatchHappiness(Character character, int minHappiness, int maxHappiness)
		{
			sbyte happiness = character.GetHappiness();
			return (int)happiness >= minHappiness && (int)happiness <= maxHappiness;
		}

		// Token: 0x0600759D RID: 30109 RVA: 0x0044B904 File Offset: 0x00449B04
		public static bool MatchResource(Character character, sbyte resourceType, int minAmount, int maxAmount)
		{
			int amount = character.GetResource(resourceType);
			return amount >= minAmount && amount <= maxAmount;
		}

		// Token: 0x0600759E RID: 30110 RVA: 0x0044B92C File Offset: 0x00449B2C
		public static bool MatchHairNoSkinHead(Character character)
		{
			AvatarData avatar = character.GetAvatar();
			return avatar.GetGrowableElementShowingState(0);
		}

		// Token: 0x0600759F RID: 30111 RVA: 0x0044B94C File Offset: 0x00449B4C
		public static bool MatchLifeSkillAttainment(Character character, sbyte lifeSkillType, int minVal, int maxVal)
		{
			short lifeSkillAttainment = character.GetLifeSkillAttainment(lifeSkillType);
			return (int)lifeSkillAttainment >= minVal && (int)lifeSkillAttainment <= maxVal;
		}

		// Token: 0x060075A0 RID: 30112 RVA: 0x0044B974 File Offset: 0x00449B74
		public static bool MatchCombatSkillAttainment(Character character, sbyte combatSkillType, int minVal, int maxVal)
		{
			short combatSkillAttainment = character.GetCombatSkillAttainment(combatSkillType);
			return (int)combatSkillAttainment >= minVal && (int)combatSkillAttainment <= maxVal;
		}

		// Token: 0x060075A1 RID: 30113 RVA: 0x0044B99C File Offset: 0x00449B9C
		public static bool MatchIsMonk(Character character)
		{
			return character.GetMonkType() > 0;
		}

		// Token: 0x060075A2 RID: 30114 RVA: 0x0044B9B8 File Offset: 0x00449BB8
		public static bool MatchOrgMemberAllowMarriage(Character character)
		{
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			return orgMemberConfig.ChildGrade >= 0 && orgInfo.Principal;
		}

		// Token: 0x060075A3 RID: 30115 RVA: 0x0044B9EC File Offset: 0x00449BEC
		public static bool MatchCombatPowerRankInSect(Character character, int minRank, int maxRank)
		{
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			bool flag = orgInfo.SettlementId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(orgInfo.SettlementId);
				int ranking = settlement.GetCharacterRanking(character.GetId());
				result = (ranking >= minRank && ranking <= maxRank);
			}
			return result;
		}

		// Token: 0x060075A4 RID: 30116 RVA: 0x0044BA48 File Offset: 0x00449C48
		public static bool MatchTopThousandCombatPowerRank(Character character, int minRank, int maxRank)
		{
			int ranking = DomainManager.Character.GetTopThousandCharacterRanking(character.GetId());
			return ranking >= minRank && ranking <= maxRank;
		}

		// Token: 0x060075A5 RID: 30117 RVA: 0x0044BA7C File Offset: 0x00449C7C
		public static bool MatchPrincipal(Character character)
		{
			return character.GetOrganizationInfo().Principal;
		}

		// Token: 0x060075A6 RID: 30118 RVA: 0x0044BA9C File Offset: 0x00449C9C
		public static bool MatchSettlementLeader(Character character)
		{
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			return orgInfo.Grade == 8 && orgInfo.Principal;
		}

		// Token: 0x060075A7 RID: 30119 RVA: 0x0044BAC8 File Offset: 0x00449CC8
		public static bool MatchSettlingState(Character character, int minStateTemplateId, int maxStateTemplateId)
		{
			short settlementId = character.GetOrganizationInfo().SettlementId;
			bool flag = settlementId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				short areaId = settlement.GetLocation().AreaId;
				sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(areaId);
				result = ((int)stateTemplateId >= minStateTemplateId && (int)stateTemplateId <= maxStateTemplateId);
			}
			return result;
		}

		// Token: 0x060075A8 RID: 30120 RVA: 0x0044BB2C File Offset: 0x00449D2C
		public static bool MatchAncestry(Character character, int stateTemplateId)
		{
			short templateId = character.GetTemplateId();
			MapStateItem stateCfg = MapState.Instance[stateTemplateId];
			bool flag = CollectionUtils.Contains<short>(stateCfg.TemplateCharacterIds, templateId);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = stateCfg.SectID == 11;
				bool flag3 = flag2;
				if (flag3)
				{
					bool flag4 = templateId - 30 <= 1;
					flag3 = flag4;
				}
				result = flag3;
			}
			return result;
		}

		// Token: 0x060075A9 RID: 30121 RVA: 0x0044BB90 File Offset: 0x00449D90
		public static bool MatchMaritalStatus(Character character, bool isMarried)
		{
			bool hasAliveSpouse = DomainManager.Character.GetAliveSpouse(character.GetId()) >= 0;
			return hasAliveSpouse == isMarried;
		}

		// Token: 0x060075AA RID: 30122 RVA: 0x0044BBC0 File Offset: 0x00449DC0
		public static bool MatchCompletelyInfected(Character character)
		{
			return character.IsCompletelyInfected();
		}

		// Token: 0x060075AB RID: 30123 RVA: 0x0044BBD8 File Offset: 0x00449DD8
		public unsafe static bool MatchPreexistenceChar(Character character, int preexistenceCharId)
		{
			PreexistenceCharIds preexistenceCharIds = *character.GetPreexistenceCharIds();
			int i = 0;
			int count = preexistenceCharIds.Count;
			while (i < count)
			{
				bool flag = *(ref preexistenceCharIds.CharIds.FixedElementField + (IntPtr)i * 4) == preexistenceCharId;
				if (flag)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x060075AC RID: 30124 RVA: 0x0044BC30 File Offset: 0x00449E30
		public static bool MatchRelationTypeId(Character character, Character relatedCharacter, sbyte minId, sbyte maxId)
		{
			RelatedCharacter relation;
			bool flag = !DomainManager.Character.TryGetRelation(character.GetId(), relatedCharacter.GetId(), out relation);
			bool result;
			if (flag)
			{
				result = (minId <= -1);
			}
			else
			{
				sbyte relationTypeId = RelationType.GetTypeId(relation.RelationType);
				result = (relationTypeId >= minId && relationTypeId <= maxId);
			}
			return result;
		}

		// Token: 0x060075AD RID: 30125 RVA: 0x0044BC88 File Offset: 0x00449E88
		public static bool MatchHasInventoryItem(Character character, sbyte itemType, short templateId, int expectedAmount)
		{
			Dictionary<ItemKey, int> inventoryItems = character.GetInventory().Items;
			bool itemFound = false;
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventoryItems)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				int amount = num;
				bool flag = itemKey.ItemType != itemType || itemKey.TemplateId != templateId;
				if (!flag)
				{
					bool flag2 = amount < expectedAmount;
					if (flag2)
					{
						return false;
					}
					itemFound = true;
					break;
				}
			}
			return itemFound;
		}

		// Token: 0x060075AE RID: 30126 RVA: 0x0044BD30 File Offset: 0x00449F30
		public static bool MatchAtVisibleAdventureSite(Character character, short adventureTemplateId)
		{
			Location location = character.GetLocation();
			AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
			AdventureSiteData site;
			return adventuresInArea.AdventureSites.TryGetValue(location.BlockId, out site) && site.TemplateId == adventureTemplateId && site.SiteState == 1;
		}

		// Token: 0x060075AF RID: 30127 RVA: 0x0044BD84 File Offset: 0x00449F84
		public static bool MatchMonasticTitleOrDisplayName(Character character, string name)
		{
			NameRelatedData nameRelatedData = new NameRelatedData();
			CharacterDomain.GetNameRelatedData(character, ref nameRelatedData);
			ValueTuple<string, string> monasticTitleOrDisplayNameDetailed = nameRelatedData.GetMonasticTitleOrDisplayNameDetailed(false, true);
			string surname = monasticTitleOrDisplayNameDetailed.Item1;
			string givenName = monasticTitleOrDisplayNameDetailed.Item2;
			return name == surname + givenName;
		}
	}
}
