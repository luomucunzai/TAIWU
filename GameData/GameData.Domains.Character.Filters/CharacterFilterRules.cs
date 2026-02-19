using System;
using System.Collections.Generic;
using Config;
using Config.ConfigCells.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Organization;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.Filters;

public static class CharacterFilterRules
{
	public unsafe static void GetRandomCharacterPropertiesWithRulesId(IRandomSource randomSource, short characterFilterRulesId, ref TemporaryIntelligentCharacterCreationInfo charCreationInfo)
	{
		CharacterFilterRulesItem characterFilterRulesItem = Config.CharacterFilterRules.Instance[characterFilterRulesId];
		sbyte b = (sbyte)(randomSource.Next(15) + 1);
		sbyte b2 = -1;
		sbyte b3 = -1;
		bool? flag = null;
		bool? flag2 = null;
		bool? flag3 = null;
		sbyte b4 = 3;
		charCreationInfo.OrgInfo.Grade = (sbyte)randomSource.Next(0, 9);
		charCreationInfo.OrgInfo.Principal = true;
		charCreationInfo.Resources.Initialize();
		sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
		foreach (CharacterFilterElement rules in characterFilterRulesItem.RulesList)
		{
			if (rules.ValueMin > rules.ValueMax)
			{
				throw new Exception($"Rule {characterFilterRulesId} Type {rules.PropertyType}: value range error: {rules.ValueMin}, {rules.ValueMax}");
			}
			int num = ((rules.ValueMin < rules.ValueMax) ? randomSource.Next(rules.ValueMin, rules.ValueMax + 1) : rules.ValueMin);
			switch (rules.PropertyType)
			{
			case 0:
				if (rules.PropertySubType == 1)
				{
					num += xiangshuLevel;
				}
				charCreationInfo.OrgInfo.Grade = (sbyte)Math.Clamp(num, 0, 8);
				break;
			case 1:
				if (charCreationInfo.ActualAge.HasValue)
				{
					throw new Exception("ActualAge is being assigned twice for creating temporary intelligent character.");
				}
				charCreationInfo.ActualAge = (short)num;
				break;
			case 2:
				b2 = (sbyte)num;
				break;
			case 3:
			{
				(short min, short max) tuple = BehaviorType.Ranges[num];
				short item = tuple.min;
				short item2 = tuple.max;
				charCreationInfo.Morality = (short)randomSource.Next((int)item, item2 + 1);
				break;
			}
			case 4:
				if (charCreationInfo.ActualAge.HasValue)
				{
					throw new Exception("ActualAge is being assigned twice for creating temporary intelligent character.");
				}
				charCreationInfo.ActualAge = (short)num;
				break;
			case 5:
				charCreationInfo.BaseAttraction = (short)num;
				break;
			case 6:
				b = (sbyte)num;
				break;
			case 8:
				charCreationInfo.IsCompletelyInfected = num == 1;
				break;
			case 9:
				flag2 = rules.ValueMin == 0 && (rules.ValueMax == 0 || rules.ValueMax < 0);
				b4 = (sbyte)Math.Max(rules.ValueMax, rules.ValueMin);
				b3 = (sbyte)num;
				break;
			case 10:
				charCreationInfo.Happiness = (sbyte)num;
				break;
			case 11:
				flag = num == 1;
				break;
			case 12:
				flag3 = num == 1;
				break;
			case 16:
				if (rules.PropertySubType == 1)
				{
					num += xiangshuLevel;
				}
				charCreationInfo.ConsummateLevel = (sbyte)Math.Clamp(num, 0, 127);
				break;
			case 19:
				charCreationInfo.GoodAtLifeSkillType = (sbyte)rules.PropertySubType;
				break;
			case 20:
				charCreationInfo.GoodAtCombatSkillType = (sbyte)rules.PropertySubType;
				break;
			case 21:
			{
				ref int reference = ref charCreationInfo.Resources.Items[rules.PropertySubType];
				reference += num;
				break;
			}
			case 22:
				charCreationInfo.HairNoSkinHead = true;
				break;
			}
		}
		if (b < 1)
		{
			if (!charCreationInfo.Location.IsValid())
			{
				throw new Exception($"CharacterFilterRules {characterFilterRulesId} requires a valid location.");
			}
			b = DomainManager.Map.GetStateTemplateIdByAreaId(charCreationInfo.Location.AreaId);
		}
		MapStateItem mapStateItem = MapState.Instance[b];
		if (b3 == -1)
		{
			b3 = (sbyte)randomSource.Next(flag.HasValue ? 1 : 0, 4);
		}
		if (flag.HasValue || flag3 == true)
		{
			if (flag2 == true)
			{
				throw new Exception("Unable to create non-monk or allow marriage character in sect only context.");
			}
			if (b3 == 0)
			{
				b3 = (sbyte)randomSource.Next(1, b4 + 1);
			}
		}
		else if (flag3.HasValue)
		{
			throw new Exception("OrgMemberAllowMarriage cannot be used for creating temporary character in current context.");
		}
		switch (b3)
		{
		case 0:
			charCreationInfo.OrgInfo.OrgTemplateId = mapStateItem.SectID;
			charCreationInfo.OrgInfo.SettlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(charCreationInfo.OrgInfo.OrgTemplateId);
			break;
		case 1:
			charCreationInfo.OrgInfo.OrgTemplateId = MapArea.Instance[mapStateItem.MainAreaID].OrganizationId[0];
			charCreationInfo.OrgInfo.SettlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(charCreationInfo.OrgInfo.OrgTemplateId);
			break;
		case 2:
			charCreationInfo.OrgInfo.SettlementId = DomainManager.Map.GetRandomSettlementId((sbyte)(b - 1), randomSource);
			charCreationInfo.OrgInfo.OrgTemplateId = DomainManager.Organization.GetSettlement(charCreationInfo.OrgInfo.SettlementId).GetOrgTemplateId();
			break;
		case 3:
			charCreationInfo.OrgInfo.OrgTemplateId = 16;
			charCreationInfo.OrgInfo.SettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			break;
		default:
			throw new Exception($"Invalid organization type {b3}");
		}
		sbyte b5 = OrganizationDomain.GetOrgMemberConfig(charCreationInfo.OrgInfo.OrgTemplateId, charCreationInfo.OrgInfo.Grade).Gender;
		if (b5 == -1)
		{
			b5 = Config.Organization.Instance[charCreationInfo.OrgInfo.OrgTemplateId].GenderRestriction;
		}
		if (b2 == -1)
		{
			b2 = ((b5 == -1) ? Gender.GetRandom(randomSource) : b5);
		}
		Tester.Assert(b5 == -1 || b5 == b2);
		charCreationInfo.CharTemplateId = OrganizationDomain.GetCharacterTemplateId(charCreationInfo.OrgInfo.OrgTemplateId, b, b2);
	}

	public static void ToPredicates(short characterFilterRulesId, List<Predicate<Character>> predicates, sbyte curStateTemplateId = -1)
	{
		predicates.Clear();
		predicates.Add(CharacterMatchers.MatchNotCalledByAdventure);
		predicates.Add(CharacterMatchers.MatchCanBeCalledByAdventure);
		predicates.Add(CharacterMatchers.MatchHasNoWork);
		predicates.Add(CharacterMatchers.MatchNotAffectedByLegendaryBook);
		CharacterFilterRulesItem characterFilterRulesItem = Config.CharacterFilterRules.Instance[characterFilterRulesId];
		HashSet<int> hashSet = new HashSet<int>();
		foreach (CharacterFilterElement rule in characterFilterRulesItem.RulesList)
		{
			if (!hashSet.Add(rule.PropertyType))
			{
				throw new Exception($"The same property type {rule.PropertyType} is filtered more than once");
			}
			switch (rule.PropertyType)
			{
			case 0:
			{
				sbyte xiangshuLevel2 = DomainManager.World.GetXiangshuLevel();
				sbyte minVal2 = (sbyte)(rule.ValueMin + ((rule.PropertySubType == 1) ? xiangshuLevel2 : 0));
				sbyte maxVal2 = (sbyte)(rule.ValueMax + ((rule.PropertySubType == 1) ? xiangshuLevel2 : 0));
				predicates.Add((Character character) => CharacterMatchers.MatchGrade(character, minVal2, maxVal2));
				break;
			}
			case 1:
				predicates.Add((Character character) => CharacterMatchers.MatchPhysiologicalAge(character, rule.ValueMin, rule.ValueMax));
				break;
			case 2:
				predicates.Add((Character character) => CharacterMatchers.MatchGender(character, (sbyte)rule.ValueMin));
				break;
			case 3:
				predicates.Add((Character character) => CharacterMatchers.MatchBehaviorType(character, (sbyte)rule.ValueMin, (sbyte)rule.ValueMax));
				break;
			case 4:
				if (hashSet.Contains(1))
				{
					throw new Exception("PhysiologicalAge and DisplayingAge can't be filtered at the same time.");
				}
				predicates.Add((Character character) => CharacterMatchers.MatchDisplayingAge(character, rule.ValueMin, rule.ValueMax));
				break;
			case 5:
				predicates.Add((Character character) => CharacterMatchers.MatchAttraction(character, rule.ValueMin, rule.ValueMax));
				break;
			case 6:
				if (rule.ValueMin >= 0)
				{
					predicates.Add((Character character) => CharacterMatchers.MatchSettlingState(character, rule.ValueMin, rule.ValueMax));
					break;
				}
				if (curStateTemplateId >= 0)
				{
					predicates.Add((Character character) => CharacterMatchers.MatchSettlingState(character, curStateTemplateId, curStateTemplateId));
					break;
				}
				throw new Exception($"CharacterFilterRules {characterFilterRulesId} requires a valid curStateTemplateId.");
			case 7:
				predicates.Add((Character character) => CharacterMatchers.MatchMaritalStatus(character, isMarried: false));
				break;
			case 8:
				if (rule.ValueMin == 1)
				{
					predicates.Add(CharacterMatchers.MatchCompletelyInfected);
					break;
				}
				predicates.Add((Character character) => !CharacterMatchers.MatchCompletelyInfected(character));
				break;
			case 9:
				predicates.Add((Character character) => CharacterMatchers.MatchOrganizationType(character, (sbyte)rule.ValueMin, (sbyte)rule.ValueMax));
				break;
			case 10:
				predicates.Add((Character character) => CharacterMatchers.MatchHappiness(character, (sbyte)rule.ValueMin, (sbyte)rule.ValueMax));
				break;
			case 11:
				if (rule.ValueMin == 1)
				{
					predicates.Add(CharacterMatchers.MatchIsMonk);
					break;
				}
				predicates.Add((Character character) => !CharacterMatchers.MatchIsMonk(character));
				break;
			case 12:
				if (rule.ValueMin == 1)
				{
					predicates.Add(CharacterMatchers.MatchOrgMemberAllowMarriage);
					break;
				}
				predicates.Add((Character character) => !CharacterMatchers.MatchOrgMemberAllowMarriage(character));
				break;
			case 13:
				predicates.Add((Character character) => CharacterMatchers.MatchCombatPowerRankInSect(character, rule.ValueMin, rule.ValueMax));
				break;
			case 14:
				if (rule.ValueMin == 1)
				{
					predicates.Add(CharacterMatchers.MatchPrincipal);
					break;
				}
				predicates.Add((Character character) => !CharacterMatchers.MatchPrincipal(character));
				break;
			case 15:
				if (rule.ValueMin == 1)
				{
					predicates.Add(CharacterMatchers.MatchSettlementLeader);
					break;
				}
				predicates.Add((Character character) => !CharacterMatchers.MatchSettlementLeader(character));
				break;
			case 16:
			{
				sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
				sbyte minVal = (sbyte)(rule.ValueMin + ((rule.PropertySubType == 1) ? xiangshuLevel : 0));
				sbyte maxVal = (sbyte)(rule.ValueMax + ((rule.PropertySubType == 1) ? xiangshuLevel : 0));
				predicates.Add((Character character) => CharacterMatchers.MatchConsummateLevel(character, minVal, maxVal));
				break;
			}
			case 17:
				predicates.Add((Character character) => CharacterMatchers.MatchLifeSkillAttainment(character, (sbyte)rule.PropertySubType, rule.ValueMin, rule.ValueMax));
				break;
			case 18:
				predicates.Add((Character character) => CharacterMatchers.MatchCombatSkillAttainment(character, (sbyte)rule.PropertySubType, rule.ValueMin, rule.ValueMax));
				break;
			case 19:
				if (rule.ValueMin == 1)
				{
					predicates.Add((Character character) => CharacterMatchers.MatchGoodAtLifeSkillType(character, (sbyte)rule.PropertySubType));
				}
				else
				{
					predicates.Add((Character character) => !CharacterMatchers.MatchGoodAtLifeSkillType(character, (sbyte)rule.PropertySubType));
				}
				break;
			case 20:
				if (rule.ValueMin == 1)
				{
					predicates.Add((Character character) => CharacterMatchers.MatchGoodAtCombatSkillType(character, (sbyte)rule.PropertySubType));
				}
				else
				{
					predicates.Add((Character character) => !CharacterMatchers.MatchGoodAtCombatSkillType(character, (sbyte)rule.PropertySubType));
				}
				break;
			case 21:
				predicates.Add((Character character) => CharacterMatchers.MatchResource(character, (sbyte)rule.PropertySubType, rule.ValueMin, rule.ValueMax));
				break;
			case 22:
				predicates.Add(CharacterMatchers.MatchHairNoSkinHead);
				break;
			}
		}
	}
}
