using System;
using GameData.Domains.Character;
using GameData.Domains.Character.Filters;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions.CustomActions
{
	// Token: 0x02000098 RID: 152
	[Obsolete]
	public static class CallCharacterPredefinedRules
	{
		// Token: 0x060019B9 RID: 6585 RVA: 0x00170C98 File Offset: 0x0016EE98
		[Obsolete]
		public static bool MatchParticipateCharacter_ContestForTaiwuBride(Character character)
		{
			bool flag = !CharacterMatchers.MatchNotCalledByAdventure(character);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !CharacterMatchers.MatchDisplayingAge(character, 16, 50);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = !CharacterMatchers.MatchMaritalStatus(character, false);
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = !CharacterMatchers.MatchOrganizationType(character, 0, 2);
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = !CharacterMatchers.MatchOrgMemberAllowMarriage(character);
							if (flag5)
							{
								result = false;
							}
							else
							{
								bool flag6 = CharacterMatchers.MatchIsMonk(character);
								if (flag6)
								{
									result = false;
								}
								else
								{
									sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
									bool flag7 = !CharacterMatchers.MatchGrade(character, 0, xiangshuLevel);
									result = !flag7;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x00170D40 File Offset: 0x0016EF40
		[Obsolete]
		public static bool MatchParticipateCharacter_SectMartialArtContest(Character character, sbyte orgTemplateId)
		{
			bool flag = !CharacterMatchers.MatchNotCalledByAdventure(character);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = character.GetOrganizationInfo().OrgTemplateId != orgTemplateId;
				if (flag2)
				{
					result = false;
				}
				else
				{
					sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
					sbyte minGrade = (xiangshuLevel > 6) ? 6 : 0;
					sbyte maxGrade = minGrade + 1;
					bool flag3 = !CharacterMatchers.MatchGrade(character, minGrade, maxGrade);
					result = !flag3;
				}
			}
			return result;
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x00170DB4 File Offset: 0x0016EFB4
		[Obsolete]
		public static bool MatchMajorCharacter_SectMartialArtContest(Character character, sbyte orgTemplateId)
		{
			bool flag = !CharacterMatchers.MatchNotCalledByAdventure(character);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !CharacterMatchers.MatchOrganization(character, orgTemplateId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
					sbyte grade = xiangshuLevel + 2;
					bool flag3 = !CharacterMatchers.MatchGrade(character, grade, grade);
					result = !flag3;
				}
			}
			return result;
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x00170E11 File Offset: 0x0016F011
		[Obsolete]
		public static bool MatchParticipateCharacter_CombatSkillTypeContest(Character character, sbyte combatSkillType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x00170E19 File Offset: 0x0016F019
		[Obsolete]
		public static bool MatchParticipateCharacter_LifeSkillTypeContest(Character character, sbyte lifeSkillType)
		{
			throw new NotImplementedException();
		}
	}
}
