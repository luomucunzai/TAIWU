using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using Config.ConfigCells.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.Organization;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.Filters
{
	// Token: 0x02000839 RID: 2105
	public static class CharacterFilterRules
	{
		// Token: 0x06007587 RID: 30087 RVA: 0x0044A6A0 File Offset: 0x004488A0
		public unsafe static void GetRandomCharacterPropertiesWithRulesId(IRandomSource randomSource, short characterFilterRulesId, ref TemporaryIntelligentCharacterCreationInfo charCreationInfo)
		{
			CharacterFilterRulesItem config = CharacterFilterRules.Instance[characterFilterRulesId];
			sbyte settlingStateTemplateId = (sbyte)(randomSource.Next(15) + 1);
			sbyte gender = -1;
			sbyte organizationType = -1;
			bool? isMonk = null;
			bool? sectOnly = null;
			bool? orgAllowMarriage = null;
			sbyte orgTypeMaxVal = 3;
			charCreationInfo.OrgInfo.Grade = (sbyte)randomSource.Next(0, 9);
			charCreationInfo.OrgInfo.Principal = true;
			charCreationInfo.Resources.Initialize();
			sbyte currXiangshuLevel = DomainManager.World.GetXiangshuLevel();
			foreach (CharacterFilterElement rule in config.RulesList)
			{
				bool flag = rule.ValueMin > rule.ValueMax;
				if (flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Rule ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(characterFilterRulesId);
					defaultInterpolatedStringHandler.AppendLiteral(" Type ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(rule.PropertyType);
					defaultInterpolatedStringHandler.AppendLiteral(": value range error: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(rule.ValueMin);
					defaultInterpolatedStringHandler.AppendLiteral(", ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(rule.ValueMax);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int value = (rule.ValueMin < rule.ValueMax) ? randomSource.Next(rule.ValueMin, rule.ValueMax + 1) : rule.ValueMin;
				switch (rule.PropertyType)
				{
				case 0:
				{
					bool flag2 = rule.PropertySubType == 1;
					if (flag2)
					{
						value += (int)currXiangshuLevel;
					}
					charCreationInfo.OrgInfo.Grade = (sbyte)Math.Clamp(value, 0, 8);
					break;
				}
				case 1:
				{
					bool flag3 = charCreationInfo.ActualAge != null;
					if (flag3)
					{
						throw new Exception("ActualAge is being assigned twice for creating temporary intelligent character.");
					}
					charCreationInfo.ActualAge = new short?((short)value);
					break;
				}
				case 2:
					gender = (sbyte)value;
					break;
				case 3:
				{
					ValueTuple<short, short> valueTuple = BehaviorType.Ranges[value];
					short min = valueTuple.Item1;
					short max = valueTuple.Item2;
					charCreationInfo.Morality = new short?((short)randomSource.Next((int)min, (int)(max + 1)));
					break;
				}
				case 4:
				{
					bool flag4 = charCreationInfo.ActualAge != null;
					if (flag4)
					{
						throw new Exception("ActualAge is being assigned twice for creating temporary intelligent character.");
					}
					charCreationInfo.ActualAge = new short?((short)value);
					break;
				}
				case 5:
					charCreationInfo.BaseAttraction = new short?((short)value);
					break;
				case 6:
					settlingStateTemplateId = (sbyte)value;
					break;
				case 8:
					charCreationInfo.IsCompletelyInfected = new bool?(value == 1);
					break;
				case 9:
					sectOnly = new bool?(rule.ValueMin == 0 && (rule.ValueMax == 0 || rule.ValueMax < 0));
					orgTypeMaxVal = (sbyte)Math.Max(rule.ValueMax, rule.ValueMin);
					organizationType = (sbyte)value;
					break;
				case 10:
					charCreationInfo.Happiness = new sbyte?((sbyte)value);
					break;
				case 11:
					isMonk = new bool?(value == 1);
					break;
				case 12:
					orgAllowMarriage = new bool?(value == 1);
					break;
				case 16:
				{
					bool flag5 = rule.PropertySubType == 1;
					if (flag5)
					{
						value += (int)currXiangshuLevel;
					}
					charCreationInfo.ConsummateLevel = new sbyte?((sbyte)Math.Clamp(value, 0, 127));
					break;
				}
				case 19:
					charCreationInfo.GoodAtLifeSkillType = new sbyte?((sbyte)rule.PropertySubType);
					break;
				case 20:
					charCreationInfo.GoodAtCombatSkillType = new sbyte?((sbyte)rule.PropertySubType);
					break;
				case 21:
					*(ref charCreationInfo.Resources.Items.FixedElementField + (IntPtr)rule.PropertySubType * 4) += value;
					break;
				case 22:
					charCreationInfo.HairNoSkinHead = true;
					break;
				}
			}
			bool flag6 = settlingStateTemplateId < 1;
			if (flag6)
			{
				bool flag7 = !charCreationInfo.Location.IsValid();
				if (flag7)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
					defaultInterpolatedStringHandler.AppendLiteral("CharacterFilterRules ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(characterFilterRulesId);
					defaultInterpolatedStringHandler.AppendLiteral(" requires a valid location.");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				settlingStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(charCreationInfo.Location.AreaId);
			}
			MapStateItem settlingStateCfg = MapState.Instance[settlingStateTemplateId];
			bool flag8 = organizationType == -1;
			if (flag8)
			{
				organizationType = (sbyte)randomSource.Next((isMonk == null) ? 0 : 1, 4);
			}
			bool flag9 = isMonk != null || orgAllowMarriage.GetValueOrDefault();
			if (flag9)
			{
				bool valueOrDefault = sectOnly.GetValueOrDefault();
				if (valueOrDefault)
				{
					throw new Exception("Unable to create non-monk or allow marriage character in sect only context.");
				}
				bool flag10 = organizationType == 0;
				if (flag10)
				{
					organizationType = (sbyte)randomSource.Next(1, (int)(orgTypeMaxVal + 1));
				}
			}
			else
			{
				bool flag11 = orgAllowMarriage != null;
				if (flag11)
				{
					throw new Exception("OrgMemberAllowMarriage cannot be used for creating temporary character in current context.");
				}
			}
			switch (organizationType)
			{
			case 0:
				charCreationInfo.OrgInfo.OrgTemplateId = settlingStateCfg.SectID;
				charCreationInfo.OrgInfo.SettlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(charCreationInfo.OrgInfo.OrgTemplateId);
				break;
			case 1:
				charCreationInfo.OrgInfo.OrgTemplateId = MapArea.Instance[(short)settlingStateCfg.MainAreaID].OrganizationId[0];
				charCreationInfo.OrgInfo.SettlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(charCreationInfo.OrgInfo.OrgTemplateId);
				break;
			case 2:
				charCreationInfo.OrgInfo.SettlementId = DomainManager.Map.GetRandomSettlementId(settlingStateTemplateId - 1, randomSource, false);
				charCreationInfo.OrgInfo.OrgTemplateId = DomainManager.Organization.GetSettlement(charCreationInfo.OrgInfo.SettlementId).GetOrgTemplateId();
				break;
			case 3:
				charCreationInfo.OrgInfo.OrgTemplateId = 16;
				charCreationInfo.OrgInfo.SettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid organization type ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(organizationType);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			sbyte genderRestriction = OrganizationDomain.GetOrgMemberConfig(charCreationInfo.OrgInfo.OrgTemplateId, charCreationInfo.OrgInfo.Grade).Gender;
			bool flag12 = genderRestriction == -1;
			if (flag12)
			{
				genderRestriction = Organization.Instance[charCreationInfo.OrgInfo.OrgTemplateId].GenderRestriction;
			}
			bool flag13 = gender == -1;
			if (flag13)
			{
				gender = ((genderRestriction == -1) ? Gender.GetRandom(randomSource) : genderRestriction);
			}
			Tester.Assert(genderRestriction == -1 || genderRestriction == gender, "");
			charCreationInfo.CharTemplateId = OrganizationDomain.GetCharacterTemplateId(charCreationInfo.OrgInfo.OrgTemplateId, settlingStateTemplateId, gender);
		}

		// Token: 0x06007588 RID: 30088 RVA: 0x0044AD84 File Offset: 0x00448F84
		public static void ToPredicates(short characterFilterRulesId, List<Predicate<Character>> predicates, sbyte curStateTemplateId = -1)
		{
			predicates.Clear();
			predicates.Add(new Predicate<Character>(CharacterMatchers.MatchNotCalledByAdventure));
			predicates.Add(new Predicate<Character>(CharacterMatchers.MatchCanBeCalledByAdventure));
			predicates.Add(new Predicate<Character>(CharacterMatchers.MatchHasNoWork));
			predicates.Add(new Predicate<Character>(CharacterMatchers.MatchNotAffectedByLegendaryBook));
			CharacterFilterRulesItem config = CharacterFilterRules.Instance[characterFilterRulesId];
			HashSet<int> propertyTypes = new HashSet<int>();
			using (List<CharacterFilterElement>.Enumerator enumerator = config.RulesList.GetEnumerator())
			{
				Predicate<Character> <>9__6;
				while (enumerator.MoveNext())
				{
					CharacterFilterElement rule = enumerator.Current;
					bool flag = !propertyTypes.Add(rule.PropertyType);
					if (flag)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 1);
						defaultInterpolatedStringHandler.AppendLiteral("The same property type ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(rule.PropertyType);
						defaultInterpolatedStringHandler.AppendLiteral(" is filtered more than once");
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					switch (rule.PropertyType)
					{
					case 0:
					{
						sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
						sbyte minVal = (sbyte)(rule.ValueMin + (int)((rule.PropertySubType == 1) ? xiangshuLevel : 0));
						sbyte maxVal = (sbyte)(rule.ValueMax + (int)((rule.PropertySubType == 1) ? xiangshuLevel : 0));
						predicates.Add((Character character) => CharacterMatchers.MatchGrade(character, minVal, maxVal));
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
					{
						bool flag2 = propertyTypes.Contains(1);
						if (flag2)
						{
							throw new Exception("PhysiologicalAge and DisplayingAge can't be filtered at the same time.");
						}
						predicates.Add((Character character) => CharacterMatchers.MatchDisplayingAge(character, rule.ValueMin, rule.ValueMax));
						break;
					}
					case 5:
						predicates.Add((Character character) => CharacterMatchers.MatchAttraction(character, rule.ValueMin, rule.ValueMax));
						break;
					case 6:
					{
						bool flag3 = rule.ValueMin >= 0;
						if (flag3)
						{
							predicates.Add((Character character) => CharacterMatchers.MatchSettlingState(character, rule.ValueMin, rule.ValueMax));
						}
						else
						{
							bool flag4 = curStateTemplateId >= 0;
							if (!flag4)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 1);
								defaultInterpolatedStringHandler.AppendLiteral("CharacterFilterRules ");
								defaultInterpolatedStringHandler.AppendFormatted<short>(characterFilterRulesId);
								defaultInterpolatedStringHandler.AppendLiteral(" requires a valid curStateTemplateId.");
								throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							Predicate<Character> item;
							if ((item = <>9__6) == null)
							{
								item = (<>9__6 = ((Character character) => CharacterMatchers.MatchSettlingState(character, (int)curStateTemplateId, (int)curStateTemplateId)));
							}
							predicates.Add(item);
						}
						break;
					}
					case 7:
						predicates.Add((Character character) => CharacterMatchers.MatchMaritalStatus(character, false));
						break;
					case 8:
					{
						bool flag5 = rule.ValueMin == 1;
						if (flag5)
						{
							predicates.Add(new Predicate<Character>(CharacterMatchers.MatchCompletelyInfected));
						}
						else
						{
							predicates.Add((Character character) => !CharacterMatchers.MatchCompletelyInfected(character));
						}
						break;
					}
					case 9:
						predicates.Add((Character character) => CharacterMatchers.MatchOrganizationType(character, (sbyte)rule.ValueMin, (sbyte)rule.ValueMax));
						break;
					case 10:
						predicates.Add((Character character) => CharacterMatchers.MatchHappiness(character, (int)((sbyte)rule.ValueMin), (int)((sbyte)rule.ValueMax)));
						break;
					case 11:
					{
						bool flag6 = rule.ValueMin == 1;
						if (flag6)
						{
							predicates.Add(new Predicate<Character>(CharacterMatchers.MatchIsMonk));
						}
						else
						{
							predicates.Add((Character character) => !CharacterMatchers.MatchIsMonk(character));
						}
						break;
					}
					case 12:
					{
						bool flag7 = rule.ValueMin == 1;
						if (flag7)
						{
							predicates.Add(new Predicate<Character>(CharacterMatchers.MatchOrgMemberAllowMarriage));
						}
						else
						{
							predicates.Add((Character character) => !CharacterMatchers.MatchOrgMemberAllowMarriage(character));
						}
						break;
					}
					case 13:
						predicates.Add((Character character) => CharacterMatchers.MatchCombatPowerRankInSect(character, rule.ValueMin, rule.ValueMax));
						break;
					case 14:
					{
						bool flag8 = rule.ValueMin == 1;
						if (flag8)
						{
							predicates.Add(new Predicate<Character>(CharacterMatchers.MatchPrincipal));
						}
						else
						{
							predicates.Add((Character character) => !CharacterMatchers.MatchPrincipal(character));
						}
						break;
					}
					case 15:
					{
						bool flag9 = rule.ValueMin == 1;
						if (flag9)
						{
							predicates.Add(new Predicate<Character>(CharacterMatchers.MatchSettlementLeader));
						}
						else
						{
							predicates.Add((Character character) => !CharacterMatchers.MatchSettlementLeader(character));
						}
						break;
					}
					case 16:
					{
						sbyte xiangshuLevel2 = DomainManager.World.GetXiangshuLevel();
						sbyte minVal = (sbyte)(rule.ValueMin + (int)((rule.PropertySubType == 1) ? xiangshuLevel2 : 0));
						sbyte maxVal = (sbyte)(rule.ValueMax + (int)((rule.PropertySubType == 1) ? xiangshuLevel2 : 0));
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
					{
						bool flag10 = rule.ValueMin == 1;
						if (flag10)
						{
							predicates.Add((Character character) => CharacterMatchers.MatchGoodAtLifeSkillType(character, (sbyte)rule.PropertySubType));
						}
						else
						{
							predicates.Add((Character character) => !CharacterMatchers.MatchGoodAtLifeSkillType(character, (sbyte)rule.PropertySubType));
						}
						break;
					}
					case 20:
					{
						bool flag11 = rule.ValueMin == 1;
						if (flag11)
						{
							predicates.Add((Character character) => CharacterMatchers.MatchGoodAtCombatSkillType(character, (sbyte)rule.PropertySubType));
						}
						else
						{
							predicates.Add((Character character) => !CharacterMatchers.MatchGoodAtCombatSkillType(character, (sbyte)rule.PropertySubType));
						}
						break;
					}
					case 21:
						predicates.Add((Character character) => CharacterMatchers.MatchResource(character, (sbyte)rule.PropertySubType, rule.ValueMin, rule.ValueMax));
						break;
					case 22:
						predicates.Add(new Predicate<Character>(CharacterMatchers.MatchHairNoSkinHead));
						break;
					}
				}
			}
		}
	}
}
