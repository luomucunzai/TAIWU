using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character.Relation;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.Ai;

public static class AiHelper
{
	public static class CombatRelatedConstants
	{
		public const sbyte MajorVictoryExpGainRatio = 100;

		public const sbyte MinorVictoryExpGainRatio = 100;

		public static readonly short[] ExpGainLevels = new short[9] { 50, 100, 200, 350, 550, 850, 1200, 1600, 2200 };

		public static readonly sbyte[] AvoidCombatDefeatMarkCount = new sbyte[4] { 10, 20, 30, 10 };
	}

	public static class NpcMovementConstants
	{
		public const sbyte CommonMovementDuration = 3;

		public const sbyte MaxTimeForMovementPerMonth = 30;

		public const sbyte RandomMovementChance = 30;
	}

	public static class LuckEventConstants
	{
		public const sbyte ResourceEventChance = 15;

		public const sbyte AccessoryEventChance = 10;

		public const sbyte WeaponEventChance = 5;

		public const sbyte ArmorEventChance = 5;

		public const sbyte CombatSkillBookEventChance = 3;

		public const sbyte LifeSkillBookEventChance = 5;

		public const sbyte HealthEventChance = 8;

		public const sbyte ResourceEventPeopleCount = 5;

		public const sbyte ItemEventPeopleCount = 3;

		public const sbyte HealthEventPeopleCount = 5;
	}

	public static class DemandActionType
	{
		public const sbyte Request = 0;

		public const sbyte Steal = 1;

		public const sbyte Scam = 2;

		public const sbyte Rob = 3;

		public const sbyte RobGrave = 4;

		public const sbyte Count = 5;

		public static readonly sbyte[] ToPersonalityType = new sbyte[5] { -1, 0, 1, 3, -1 };

		public static sbyte ToMainAttributeType(sbyte actionType, bool isSkill)
		{
			return actionType switch
			{
				0 => -1, 
				1 => (sbyte)((!isSkill) ? 1 : 5), 
				2 => 2, 
				3 => 0, 
				4 => 3, 
				_ => throw new ArgumentOutOfRangeException("actionType"), 
			};
		}
	}

	public static class HarmActionType
	{
		public const sbyte Attack = 0;

		public const sbyte Poison = 1;

		public const sbyte PlotHarm = 2;

		public const sbyte Count = 3;

		public static readonly sbyte[] ToPersonalityType = new sbyte[3] { 3, 0, 1 };

		public static sbyte ToMainAttributeType(sbyte actionType)
		{
			return actionType switch
			{
				0 => -1, 
				1 => 4, 
				2 => 4, 
				_ => throw new ArgumentOutOfRangeException("actionType"), 
			};
		}
	}

	public static class GainExpActionType
	{
		public const sbyte PlayCombat = 0;

		public const sbyte BeatCombat = 1;

		public const sbyte LifeSkillBattle = 2;

		public const sbyte CricketBattle = 3;

		public const sbyte Reading = 4;

		public const sbyte Stroll = 5;

		public const int Count = 6;

		public const int RequireTargetCount = 4;

		public static readonly sbyte[][] Priorities = new sbyte[5][]
		{
			new sbyte[6] { 0, 1, 2, 3, 4, 5 },
			new sbyte[6] { 4, 5, 3, 2, 0, 1 },
			new sbyte[6] { 5, 3, 2, 4, 0, 1 },
			new sbyte[6] { 3, 2, 4, 5, 0, 1 },
			new sbyte[6] { 1, 0, 2, 3, 4, 5 }
		};

		public static readonly sbyte[] Weights = new sbyte[6] { 50, 30, 20, 15, 10, 5 };
	}

	public static class ActionTargetType
	{
		public const sbyte Invalid = -1;

		public const sbyte Parent = 0;

		public const sbyte Sibling = 1;

		public const sbyte SwornBrotherOrSister = 2;

		public const sbyte Mentor = 3;

		public const sbyte HusbandOrWife = 4;

		public const sbyte Child = 5;

		public const sbyte Adore = 6;

		public const sbyte Friend = 7;

		public const sbyte Count = 8;

		public static sbyte[][] Priorities = new sbyte[5][]
		{
			new sbyte[8] { 0, 1, 2, 3, 4, 5, 7, 6 },
			new sbyte[8] { 0, 1, 4, 5, 3, 2, 6, 7 },
			new sbyte[8] { 1, 0, 4, 5, 2, 3, 7, 6 },
			new sbyte[8] { 6, 4, 5, 2, 7, 1, 0, 3 },
			new sbyte[8] { 5, 6, 0, 1, 3, 2, 4, 7 }
		};

		public static readonly sbyte[][] PriorityScores = new sbyte[5][]
		{
			new sbyte[8] { 8, 7, 6, 5, 4, 3, 1, 2 },
			new sbyte[8] { 8, 7, 3, 4, 6, 5, 2, 1 },
			new sbyte[8] { 7, 8, 4, 3, 6, 5, 1, 2 },
			new sbyte[8] { 2, 3, 5, 1, 7, 6, 8, 4 },
			new sbyte[8] { 6, 5, 3, 4, 2, 8, 7, 1 }
		};

		public static sbyte GetActionTargetType(ushort selfToTarget)
		{
			if (RelationType.ContainParentRelations(selfToTarget))
			{
				return 0;
			}
			if (RelationType.ContainBrotherOrSisterRelations(selfToTarget))
			{
				return 1;
			}
			if (RelationType.HasRelation(selfToTarget, 512))
			{
				return 2;
			}
			if (RelationType.HasRelation(selfToTarget, 2048))
			{
				return 3;
			}
			if (RelationType.HasRelation(selfToTarget, 1024))
			{
				return 4;
			}
			if (RelationType.ContainChildRelations(selfToTarget))
			{
				return 5;
			}
			if (RelationType.HasRelation(selfToTarget, 16384))
			{
				return 6;
			}
			if (RelationType.HasRelation(selfToTarget, 8192))
			{
				return 7;
			}
			return -1;
		}
	}

	public static class GeneralActionConstants
	{
		public static readonly byte[][] EnergyGainSpeed = new byte[5][]
		{
			new byte[5] { 50, 25, 75, 100, 50 },
			new byte[5] { 75, 25, 50, 50, 100 },
			new byte[5] { 50, 50, 50, 75, 75 },
			new byte[5] { 25, 75, 50, 50, 100 },
			new byte[5] { 50, 50, 50, 100, 50 }
		};

		public static readonly sbyte[] AskForHelpRespondBaseChance = new sbyte[5] { 45, 60, 30, 15, 0 };

		public static readonly sbyte[] AskToTeachSkillRespondBaseChance = new sbyte[5] { -10, 20, 0, 10, -20 };

		public static readonly sbyte[][] StartStealingChance = new sbyte[5][]
		{
			new sbyte[3],
			new sbyte[3] { 10, 0, 20 },
			new sbyte[3] { 20, 5, 40 },
			new sbyte[3] { 40, 10, 80 },
			new sbyte[3] { 30, 15, 60 }
		};

		public static readonly sbyte[][] StartScammingChance = new sbyte[5][]
		{
			new sbyte[3],
			new sbyte[3] { 10, 0, 20 },
			new sbyte[3] { 30, 5, 60 },
			new sbyte[3] { 40, 15, 80 },
			new sbyte[3] { 20, 10, 40 }
		};

		public static readonly sbyte[][] StartRobbingChance = new sbyte[5][]
		{
			new sbyte[3] { 10, 0, 60 },
			new sbyte[3],
			new sbyte[3] { 20, 5, 20 },
			new sbyte[3] { 30, 10, 40 },
			new sbyte[3] { 40, 15, 80 }
		};

		public static readonly sbyte[][] StartRobbingFromGraveChance = new sbyte[5][]
		{
			new sbyte[3],
			new sbyte[3] { 10, 0, 20 },
			new sbyte[3] { 20, 5, 40 },
			new sbyte[3] { 40, 15, 80 },
			new sbyte[3] { 30, 10, 60 }
		};

		public const short ExtendFavorSpiritualDebtCost = 200;

		public const sbyte MerchantPraiseSpiritualDebtCost = 40;

		public const short CultivateWillMoneyCost = 10000;

		public const short CultivateWillAuthorityCost = 1000;

		public static readonly sbyte[] SocialStatusHealChance = new sbyte[5] { 30, 40, 20, 10, 0 };

		public static readonly sbyte[] SocialStatusDrinkTeaChance = new sbyte[5] { 30, 40, 20, 10, 0 };

		public static readonly sbyte[] SocialStatusDrinkWineChance = new sbyte[5] { 20, 10, 30, 40, 0 };

		public static readonly sbyte[] SocialStatusSellItemChance = new sbyte[5] { 10, 40, 30, 20, 0 };

		public static readonly sbyte[][] CricketBattleGradeOffsets = new sbyte[5][]
		{
			new sbyte[1] { 1 },
			new sbyte[2] { 0, 1 },
			new sbyte[3] { -1, 0, 1 },
			new sbyte[2] { 0, -1 },
			new sbyte[1] { -1 }
		};

		public const sbyte CaringCharBonusChance = 30;

		public const sbyte HappinessChangeOnRequestFail = -3;

		public const short FavorabilityChangeOnRequestFail = -3000;

		public const short FavorabilityChangeOnMedicineRequestFail = -6000;

		public static readonly sbyte[] ResourceLevelToHappinessChange = new sbyte[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

		public static readonly short[] ResourceLevelToFavorabilityChange = new short[9] { 300, 600, 900, 1500, 2100, 2700, 3600, 4500, 5400 };

		public const sbyte BarbStartChance = 10;

		public const sbyte BarbBonusChance = 10;

		public const short BarbMistakeFavorabilityChange = -1500;

		public const short BarbFailFavorabilityChange = -3000;

		public const short SocialStatusActionSucceedFavorabilityChange = 3000;

		public static readonly sbyte[] StartBegChance = new sbyte[5] { 60, 100, 80, 40, 20 };

		public static readonly sbyte[] BegSuccessChance = new sbyte[5] { 0, 30, 40, 20, 10 };

		public const sbyte LifeSkillBattleMinFavorType = 2;

		public const sbyte PlayCombatMinFavorType = 3;

		public const sbyte BeatCombatMaxFavorType = 2;

		public static readonly sbyte[] ReligiousLifeSkillActionBaseChance = new sbyte[5] { 0, 20, 10, -10, -20 };

		public static readonly sbyte[][] CricketBattleTargetGradeAdjust = new sbyte[5][]
		{
			new sbyte[1] { 1 },
			new sbyte[2] { 0, 1 },
			new sbyte[3] { -1, 0, 1 },
			new sbyte[2] { -1, 0 },
			new sbyte[1] { -1 }
		};

		public const short AttainmentRequiredForLifeSkillActions = 200;

		public const short IncreaseLifeSkillQualificationThreshold = 90;

		public static readonly sbyte[] AddPoisonOnTransferItemChance = new sbyte[5] { 20, 35, 50, 80, 65 };

		public const sbyte InfectionAttackTaiwuCoolDown = 3;

		public const sbyte InsaneAttackTaiwuCoolDown = 3;

		public const sbyte EnemyAttackTaiwuCoolDown = 3;

		public static readonly sbyte[] TakeFromTreasuryChance = new sbyte[5] { 40, 50, 60, 70, 80 };

		public static readonly sbyte[] StoreInTreasuryChance = new sbyte[5] { 80, 70, 60, 50, 40 };

		public static sbyte GetAskForHelpRespondChance(sbyte targetBehaviorType, sbyte favorabilityType)
		{
			return (sbyte)(AskForHelpRespondBaseChance[targetBehaviorType] + (favorabilityType - 3) * 10);
		}

		public static int GetAskToTeachSkillRespondChance(Character selfChar, Character targetChar, sbyte favorabilityType, sbyte skillGrade)
		{
			OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
			OrganizationInfo organizationInfo2 = targetChar.GetOrganizationInfo();
			int num = Config.Organization.Instance[organizationInfo2.OrgTemplateId].TeachingOutsiderProb;
			if (organizationInfo.OrgTemplateId == organizationInfo2.OrgTemplateId && organizationInfo.SettlementId == organizationInfo2.SettlementId && organizationInfo.Grade >= skillGrade)
			{
				num *= 3;
			}
			num += AskToTeachSkillRespondBaseChance[targetChar.GetBehaviorType()] + (favorabilityType - 3) * 10;
			return (num > 100) ? 100 : num;
		}

		public static sbyte GetResourceHappinessChange(sbyte resourceType, int amount)
		{
			sbyte b = ResourceTypeHelper.ResourceAmountToGrade(resourceType, amount);
			return (sbyte)((b >= 0) ? ResourceLevelToHappinessChange[b] : 0);
		}

		public static short GetResourceFavorabilityChange(sbyte resourceType, int amount)
		{
			sbyte b = ResourceTypeHelper.ResourceAmountToGrade(resourceType, amount);
			return (short)((b >= 0) ? ResourceLevelToFavorabilityChange[b] : 0);
		}

		public static short GetBegSucceedFavorabilityChange(IRandomSource random, sbyte behaviorType)
		{
			if (1 == 0)
			{
			}
			short result = behaviorType switch
			{
				0 => 1000, 
				1 => 3000, 
				2 => 2000, 
				3 => (short)random.Next(-3000, 3001), 
				4 => 0, 
				_ => throw new ArgumentException($"Invalid behavior type {behaviorType}"), 
			};
			if (1 == 0)
			{
			}
			return result;
		}

		public static short GetBegFailFavorabilityChange(IRandomSource random, sbyte behaviorType)
		{
			if (1 == 0)
			{
			}
			short result = behaviorType switch
			{
				0 => 0, 
				1 => -1000, 
				2 => -2000, 
				3 => (short)random.Next(-3000, 3001), 
				4 => -3000, 
				_ => throw new ArgumentException($"Invalid behavior type {behaviorType}"), 
			};
			if (1 == 0)
			{
			}
			return result;
		}
	}

	public static class ActionTargetRelationCategory
	{
		public static sbyte GetTargetRelationCategory(ushort currRelationType)
		{
			if (RelationType.HasRelation(currRelationType, 32768))
			{
				return 2;
			}
			if (currRelationType == 0)
			{
				return 0;
			}
			return 1;
		}
	}

	public static class UpdateStatusConstants
	{
		public static readonly sbyte[] LostChildHappinessChange = new sbyte[5] { -20, -25, -15, -10, 0 };

		public const sbyte LostChildHealthChange = -72;

		public const short KidnappedCharFavorabilityChange = -3000;

		public const short KidnappedCharBecomeEnemyFavorTypeThreshold = -2;

		public static readonly (short min, short max)[] SameGroupFavorabilityChange = new(short, short)[5]
		{
			(0, 300),
			(0, 1200),
			(0, 600),
			(0, 1200),
			(0, 300)
		};

		public static readonly sbyte[] EatForbiddenFoodChance = new sbyte[5] { 0, 5, 10, 20, 15 };

		public static readonly sbyte[] EatForbiddenFoodHappinessChange = new sbyte[5] { -15, -10, -5, 5, 0 };
	}

	public static class ActivePreparationConstants
	{
		public static readonly sbyte[] AddPoisonBonusChance = new sbyte[5] { 0, 0, 5, 10, 20 };

		public static readonly sbyte[] GradePoisonLevel = new sbyte[9] { 1, 1, 1, 2, 2, 2, 2, 3, 3 };

		public static readonly short[] GradePoisonValue = new short[9] { 10, 20, 30, 20, 40, 60, 80, 60, 90 };
	}

	public static class FixedActionConstants
	{
		public static readonly sbyte[] BoyAndGirlFriendMakeLoveBaseChance = new sbyte[5] { 5, 10, 15, 25, 20 };

		public static readonly sbyte[] RapeBaseChance = new sbyte[5] { 0, 0, 3, 9, 6 };

		public const sbyte RapeImpregnateChance = 20;

		public const sbyte MutualMakeLoveImpregnateChance = 60;

		public const sbyte FertilityDivisor = 20;

		public const short RapeFavorabilityChange = -30000;

		public static readonly sbyte[] ReleaseKidnappedCharResistanceThreshold = new sbyte[5] { 75, 50, 75, 50, 75 };

		public static readonly sbyte[] ReleaseKidnappedCharChance = new sbyte[5] { 20, 50, 30, 40, 10 };

		public static readonly sbyte[] JoinGroupBaseChance = new sbyte[5] { 10, 15, 20, 0, 5 };

		public static readonly sbyte[] JoinGroupFavorabilityReq = new sbyte[7] { 4, 3, 3, 3, 4, 4, 5 };

		public static readonly sbyte[] StayInGroupMonth = new sbyte[7] { 12, 6, 12, 12, 6, 6, 6 };

		public static readonly sbyte[] LeaveGroupChancePerMonth = new sbyte[7] { 10, 30, 15, 15, 10, 30, 30 };

		public static readonly sbyte[] MaxLeaveGroupChance = new sbyte[7] { 60, 90, 90, 90, 60, 90, 90 };

		public static readonly sbyte NpcGroupSoftMaxCount = 5;

		[Obsolete("Use GameData.Domains.Character.Ai.AiHelper.JoinGroupRelationType.ToPersonalityType instead.")]
		public static sbyte[] JoinGroupChanceRelatedPersonalityType => JoinGroupRelationType.ToPersonalityType;

		[Obsolete("Use GameData.Domains.Character.Ai.AiHelper.JoinGroupRelationType.Priorities instead.")]
		public static sbyte[][] JoinGroupTypeOrders => JoinGroupRelationType.Priorities;
	}

	public static class JoinGroupRelationType
	{
		public const sbyte Invalid = -1;

		public const sbyte HusbandAndWife = 0;

		public const sbyte MentorAndMentee = 1;

		public const sbyte ParentAndChild = 2;

		public const sbyte Siblings = 3;

		public const sbyte Lovers = 4;

		public const sbyte SwornBrotherAndSister = 5;

		public const sbyte Friends = 6;

		public const sbyte Count = 7;

		public static readonly sbyte[][] Priorities = new sbyte[5][]
		{
			new sbyte[7] { 3, 2, 1, 0, 6, 5, 4 },
			new sbyte[7] { 0, 2, 3, 6, 1, 4, 5 },
			new sbyte[7] { 2, 0, 1, 5, 4, 6, 3 },
			new sbyte[7] { 4, 5, 6, 1, 0, 3, 2 },
			new sbyte[7] { 1, 0, 2, 4, 3, 5, 6 }
		};

		public static readonly sbyte[] ToPersonalityType = new sbyte[7] { 5, 1, 0, 4, 2, 3, 6 };

		public static readonly Comparison<(Character obj, RelatedCharacter relation)>[] Comparisons = new Comparison<(Character, RelatedCharacter)>[7] { CompareHusbandAndWife, CompareMentors, CompareParents, CompareSiblings, CompareLovers, CompareSiblings, CompareFriends };

		public static sbyte GetJoinGroupRelationType(ushort selfToTarget, ushort targetToSelf)
		{
			if (RelationType.HasRelation(selfToTarget, 1024))
			{
				return 0;
			}
			if (RelationType.HasRelation(selfToTarget, 2048))
			{
				return 1;
			}
			if (RelationType.ContainParentRelations(selfToTarget))
			{
				return 2;
			}
			if (RelationType.ContainBrotherOrSisterRelations(selfToTarget))
			{
				return 3;
			}
			if (RelationType.HasRelation(selfToTarget, 16384) && RelationType.HasRelation(targetToSelf, 16384))
			{
				return 4;
			}
			if (RelationType.HasRelation(selfToTarget, 512))
			{
				return 5;
			}
			if (RelationType.HasRelation(selfToTarget, 8192))
			{
				return 6;
			}
			return -1;
		}

		private static int CompareHusbandAndWife((Character obj, RelatedCharacter relation) a, (Character obj, RelatedCharacter relation) b)
		{
			int num = a.obj.GetOrganizationInfo().Grade.CompareTo(b.obj.GetOrganizationInfo().Grade);
			if (num != 0)
			{
				return num;
			}
			int num2 = a.obj.GetGender().CompareTo(b.obj.GetGender());
			if (num2 != 0)
			{
				return num2;
			}
			int num3 = DomainManager.Character.GetReversedRelatedCharIdCount(a.obj.GetId(), 16384).CompareTo(DomainManager.Character.GetReversedRelatedCharIdCount(b.obj.GetId(), 16384));
			if (num3 != 0)
			{
				return num3;
			}
			return a.obj.GetId().CompareTo(b.obj.GetId());
		}

		private static int CompareMentors((Character obj, RelatedCharacter relation) a, (Character obj, RelatedCharacter relation) b)
		{
			return a.relation.EstablishmentDate.CompareTo(b.relation.EstablishmentDate);
		}

		private static int CompareParents((Character obj, RelatedCharacter relation) a, (Character obj, RelatedCharacter relation) b)
		{
			return RelationType.GetParentRelation(a.relation.RelationType).CompareTo(RelationType.GetParentRelation(b.relation.RelationType));
		}

		private static int CompareSiblings((Character obj, RelatedCharacter relation) a, (Character obj, RelatedCharacter relation) b)
		{
			int num = a.obj.GetCurrAge().CompareTo(b.obj.GetCurrAge());
			if (num != 0)
			{
				return num;
			}
			return a.obj.GetId().CompareTo(b.obj.GetId());
		}

		private static int CompareLovers((Character obj, RelatedCharacter relation) a, (Character obj, RelatedCharacter relation) b)
		{
			int num = a.relation.Favorability.CompareTo(b.relation.Favorability);
			if (num != 0)
			{
				return num;
			}
			int num2 = a.obj.GetGender().CompareTo(b.obj.GetGender());
			if (num2 != 0)
			{
				return num2;
			}
			int num3 = DomainManager.Character.GetReversedRelatedCharIdCount(a.obj.GetId(), 16384).CompareTo(DomainManager.Character.GetReversedRelatedCharIdCount(b.obj.GetId(), 16384));
			if (num3 != 0)
			{
				return num3;
			}
			return a.obj.GetId().CompareTo(b.obj.GetId());
		}

		private static int CompareFriends((Character obj, RelatedCharacter relation) a, (Character obj, RelatedCharacter relation) b)
		{
			int num = a.obj.GetOrganizationInfo().Grade.CompareTo(b.obj.GetOrganizationInfo().Grade);
			if (num != 0)
			{
				return num;
			}
			return a.obj.GetId().CompareTo(b.obj.GetId());
		}
	}

	public enum NpcCombatResultType : sbyte
	{
		MajorVictory,
		MinorVictory,
		MajorDefeat,
		MinorDefeat
	}

	public enum CombatResultHandleType : sbyte
	{
		Kill,
		Kidnap,
		Release
	}

	public static class NpcCombat
	{
		public static readonly sbyte[] HandleEnemyInPublicChance = new sbyte[5] { 100, 75, 50, 0, 25 };

		public static readonly CombatResultHandleType[][] ResultHandleTypePriorities = new CombatResultHandleType[5][]
		{
			new CombatResultHandleType[3]
			{
				CombatResultHandleType.Kill,
				CombatResultHandleType.Kidnap,
				CombatResultHandleType.Release
			},
			new CombatResultHandleType[3]
			{
				CombatResultHandleType.Release,
				CombatResultHandleType.Kidnap,
				CombatResultHandleType.Kill
			},
			new CombatResultHandleType[3]
			{
				CombatResultHandleType.Kidnap,
				CombatResultHandleType.Release,
				CombatResultHandleType.Kill
			},
			new CombatResultHandleType[3]
			{
				CombatResultHandleType.Kidnap,
				CombatResultHandleType.Kill,
				CombatResultHandleType.Release
			},
			new CombatResultHandleType[3]
			{
				CombatResultHandleType.Kill,
				CombatResultHandleType.Kidnap,
				CombatResultHandleType.Release
			}
		};

		public static readonly sbyte[] CombatTypePowerScale = new sbyte[3] { 40, 70, 100 };

		public static readonly sbyte[] CombatTypeSkillAttackInjuryCount = new sbyte[3] { 2, 4, 6 };

		public static readonly sbyte[] CombatTypeWeaponAttackInjuryCount = new sbyte[3] { 1, 2, 3 };

		public static readonly sbyte[] CombatTypePoisonScale = new sbyte[3] { 25, 50, 100 };
	}

	public static class LegendaryBookContestActionType
	{
		public const sbyte CombatPlay = 0;

		public const sbyte Gift = 1;

		public const sbyte Poison = 2;

		public const sbyte PlotHarm = 3;

		public const sbyte Challenge = 4;

		public const sbyte Request = 5;

		public const sbyte Trade = 6;

		public const sbyte Steal = 7;

		public const sbyte Scam = 8;

		public const sbyte Rob = 9;

		public static readonly sbyte[] ToPersonalityType = new sbyte[10] { 4, 2, 0, 1, 3, 2, 4, 0, 1, 3 };

		public static readonly sbyte[][] IndirectActionPriorities = new sbyte[5][]
		{
			new sbyte[2] { 0, 1 },
			new sbyte[2] { 1, 0 },
			new sbyte[4] { 0, 1, 2, 3 },
			new sbyte[2] { 2, 3 },
			new sbyte[2] { 3, 2 }
		};

		public static readonly sbyte[][] DirectActionNpcTargetPriorities = new sbyte[5][]
		{
			new sbyte[1] { 4 },
			new sbyte[1] { 9 },
			new sbyte[2] { 4, 9 },
			new sbyte[1] { 9 },
			new sbyte[1] { 9 }
		};

		public static readonly sbyte[][] DirectActionTaiwuTargetPriorities = new sbyte[5][]
		{
			new sbyte[3] { 4, 6, 5 },
			new sbyte[3] { 5, 6, 4 },
			new sbyte[6] { 6, 7, 5, 8, 4, 9 },
			new sbyte[3] { 8, 7, 9 },
			new sbyte[3] { 9, 7, 8 }
		};
	}

	public static class LegendaryBookRelatedConstants
	{
		public const byte TargetConsummateLevel = 18;

		public const byte ConsummateLevelGrowthPerMonth = 2;

		public const byte ShockedCharacterActCrazyRate = 50;

		public const sbyte StrongerCharacterIndirectContestChance = 25;

		public const sbyte ContestActionBaseChance = 60;

		public const sbyte ContestForLegendaryBookCooldown = 12;

		public const sbyte ContestForLegendaryBookDuration = 36;

		public const sbyte ContestForLegendaryBookMaxCharCount = 2;

		public const sbyte AffectedByLegendaryBookOwnerFeatureRate = 50;

		public const sbyte ShockedOwnerUpgradeEnemyNestCount = 4;

		public const sbyte InsaneOwnerUpgradeEnemyNestCount = 7;

		public const sbyte AttackTaiwuForLegendaryBookCoolDown = 12;

		public static readonly sbyte[] ContestForLegendaryBookChanceAdjust = new sbyte[5] { -20, -10, 0, 10, 20 };

		public static readonly sbyte[] IdleDuringContestActionChance = new sbyte[5] { 30, 40, 20, 0, 10 };

		public static readonly short[] LegendaryBookAdventures = new short[14]
		{
			149, 146, 147, 156, 157, 145, 148, 154, 152, 158,
			155, 150, 153, 151
		};
	}

	public static class MixedPoisonHarmfulActionType
	{
		public const sbyte Attack = 0;

		public const sbyte Poison = 1;

		public const sbyte PlotHarm = 2;

		public const sbyte Rape = 3;

		public const int Count = 4;
	}

	public static class PrioritizedActionConstants
	{
		public static readonly sbyte[] PrioritizedActionBaseDurations = new sbyte[9] { 12, 0, 4, 4, 3, 3, 6, 6, 6 };

		public static readonly sbyte[] PrioritizedActionCooldowns = new sbyte[9] { 6, 0, 3, 3, 12, 6, 3, 0, 6 };

		public static readonly sbyte[] PrioritizedActionMinFavorType = new sbyte[9] { -128, 2, 2, 2, -128, 2, -128, -128, -128 };

		public const sbyte CooldownBonusOnAdventureEnd = 3;

		public const sbyte CannotStrollCharCheckActionChance = 25;

		public const sbyte JoinSectBaseChancePerEnemy = 10;

		public const sbyte AskForProtectionThreshold = 75;

		public static readonly sbyte[] CivilianGradeJoinSectChance = new sbyte[9] { 0, 0, 0, -25, -50, -75, -100, -100, -100 };

		public static readonly sbyte[] FindTreasureBaseChance = new sbyte[5] { 15, 20, 25, 35, 30 };

		public static readonly sbyte[] TakeRevengeChance = new sbyte[5] { 50, 30, 40, 60, 70 };

		public static readonly sbyte[] TakeRevengeMaxFavorType = new sbyte[5] { -3, -5, -4, -1, -2 };

		public static readonly sbyte[][] PrioritizedActionPriorities = new sbyte[5][]
		{
			new sbyte[9] { 0, 1, 3, 2, 8, 4, 5, 7, 6 },
			new sbyte[9] { 0, 3, 2, 1, 4, 5, 7, 6, 8 },
			new sbyte[9] { 0, 1, 7, 6, 3, 2, 5, 4, 8 },
			new sbyte[9] { 0, 8, 3, 2, 7, 6, 1, 5, 4 },
			new sbyte[9] { 0, 7, 6, 1, 8, 4, 3, 2, 5 }
		};

		public static readonly sbyte[][] RescueFriendOrFamilyActionPriorities = new sbyte[5][]
		{
			new sbyte[3] { 3, 1, 2 },
			new sbyte[3] { 3, 1, 2 },
			new sbyte[3] { 1, 2, 3 },
			new sbyte[3] { 2, 3, 1 },
			new sbyte[3] { 2, 3, 1 }
		};

		public static readonly sbyte[][] TakeRevengeActionPriorities = new sbyte[5][]
		{
			new sbyte[3] { 0, 2, 1 },
			new sbyte[3] { 0, 2, 1 },
			new sbyte[3] { 2, 1, 0 },
			new sbyte[3] { 1, 0, 2 },
			new sbyte[3] { 1, 0, 2 }
		};

		public const sbyte TakeRevengeActionBaseChance = 60;

		public const sbyte PlotHarmMinDamage = 6;

		public const sbyte PlotHarmMaxDamage = 12;

		public const sbyte MournOthersBaseChance = 40;

		public const sbyte MournOthersWithNeedChance = 80;

		public const sbyte DejaVuMaxDistanceToTaiwu = 5;
	}

	public static class RelationsRelatedConstants
	{
		public static readonly sbyte[] IncestSexRelationChanceFactor = new sbyte[5] { 5, 10, 15, 25, 20 };

		public static readonly sbyte[] StartAdoreChanceChangePerAdored = new sbyte[5] { -50, -40, -30, -10, -20 };

		public static readonly sbyte[] BecomeEnemyHappinessChange = new sbyte[5] { 0, -5, -10, 5, 0 };

		public static readonly sbyte[] SeverEnemyHappinessChange = new sbyte[5] { 0, 5, 10, -5, 0 };

		public static readonly sbyte[] SeverEnemyNotMutuallyChance = new sbyte[5] { 30, 10, 20, 50, 40 };

		public static readonly sbyte[] ConfessLoveSucceedHappinessChange = new sbyte[5] { 5, 10, 5, 10, 5 };

		public static readonly sbyte[] ConfessLoveSucceedNeedForSexChance = new sbyte[5] { 0, 10, 20, 40, 30 };

		public static readonly sbyte[] ConfessLoveFailedHappinessChange = new sbyte[5] { -5, -10, -5, -10, -5 };

		public static readonly sbyte[] BecomeSingleNeedNewLoveChance = new sbyte[5] { 0, 10, 20, 40, 30 };

		public static readonly sbyte[] ConfessLoveFailedNeedForRapeChance = new sbyte[5] { 0, 0, 5, 15, 10 };

		public static readonly sbyte[] ConfessLoveOrProposeFailedBecomeEnemyChance = new sbyte[5] { 5, 0, 10, 20, 15 };

		public static readonly sbyte[] BreakupBecomeEnemyChance = new sbyte[5] { 10, 0, 20, 40, 30 };

		public static readonly sbyte[] BreakupMutuallyChance = new sbyte[5] { 30, 10, 20, 50, 40 };

		public static readonly sbyte[] BreakupHappinessChange = new sbyte[5] { 0, -5, -10, -5, 0 };

		public static readonly sbyte[] ProposeMarriageSucceedHappinessChange = new sbyte[5] { 15, 15, 15, 15, 15 };

		public static readonly sbyte[] ProposeMarriageFailHappinessChange = new sbyte[5] { -5, -10, -5, -10, -5 };

		public static readonly sbyte[] BecomeFriendHappinessChange = new sbyte[5] { 10, 10, 5, 0, 0 };

		public static readonly sbyte[] SeverFriendHappinessChange = new sbyte[5] { -10, -10, -5, 0, 0 };

		public static readonly sbyte[] BecomeSwornOrAdoptedFamilyHappinessChange = new sbyte[5] { 15, 15, 10, 5, 5 };

		public static readonly sbyte[] SeverSwornOrAdoptedFamilyHappinessChange = new sbyte[5] { -15, -15, -10, -5, -5 };

		public static readonly sbyte[] SeverHusbandOrWifeHappinessChange = new sbyte[5] { -10, -15, -10, -15, -10 };

		public static readonly sbyte[] BecomeMentorHappinessChange = new sbyte[5] { 0, 5, 10, -5, 0 };

		public const short ConfessLoveFavorabilityChange = 3000;

		public const short BreakupFavorabilityChange = -12000;

		public const short ProposeMarriageSucceedFavorabilityChange = 12000;

		public const short ProposeMarriageFailFavorabilityChange = -3000;

		public const short BecomeSwornOrAdoptedFamilyFavorabilityChange = 12000;

		public const short FriendshipFavorabilityChange = 3000;

		public const short SeverSwornOrAdoptedFavorabilityChange = -12000;

		public const short SeverHusbandOrWifeFavorabilityChange = -12000;

		public const short BecomeMentorFavorabilityChange = 3000;
	}

	public static class Relation
	{
		public unsafe static bool CheckChangeRelationTypeChance(IRandomSource random, ref Personalities personalities, sbyte personalityType, int multiplier = 1)
		{
			return random.CheckPercentProb(30 + personalities.Items[personalityType] * 3 * multiplier);
		}

		public static AiRelationsItem GetAiRelationConfig(short aiRelationsTemplateId)
		{
			return AiRelations.Instance[aiRelationsTemplateId];
		}

		public static bool CanStartRelation(Character selfChar, Character targetChar, ushort relationType)
		{
			int id = selfChar.GetId();
			int id2 = targetChar.GetId();
			if (!RelationTypeHelper.AllowAddingRelation(id, id2, relationType))
			{
				return false;
			}
			RelatedCharacter relation = DomainManager.Character.GetRelation(id, id2);
			RelatedCharacter relation2 = DomainManager.Character.GetRelation(id2, id);
			switch (relationType)
			{
			case 32768:
				return CanStartRelation_Enemy(relation, selfChar.GetBehaviorType());
			case 16384:
				if (RelationType.HasRelation(relation.RelationType, 16384))
				{
					return CanStartRelation_BoyOrGirlFriend(relation, selfChar.GetBehaviorType(), relation2, targetChar.GetBehaviorType());
				}
				return CanStartRelation_Adored(relation, selfChar.GetBehaviorType());
			case 1024:
				return CanStartRelation_HusbandOrWife(id, relation, selfChar.GetBehaviorType(), id2, relation2, targetChar.GetBehaviorType());
			case 8192:
				return CanStartRelation_Friend(relation, selfChar.GetBehaviorType(), relation2, targetChar.GetBehaviorType());
			case 512:
				return CanStartRelation_SwornBrotherOrSister(relation, selfChar.GetBehaviorType(), relation2, targetChar.GetBehaviorType());
			case 64:
				return CanStartRelation_AdoptiveParent(id, relation, selfChar.GetCurrAge(), id2, relation2, targetChar.GetCurrAge());
			case 128:
				return CanStartRelation_AdoptiveChild(id, relation, selfChar.GetCurrAge(), id2, relation2, targetChar.GetCurrAge());
			default:
				throw new Exception($"Unsupported relation type for starting in event: {relationType}");
			}
		}

		public static bool CanStartRelation_Enemy(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
		{
			if ((selfToTarget.RelationType & 0x8000) != 0)
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
			AiRelationsItem aiRelationConfig = GetAiRelationConfig(0);
			short num = aiRelationConfig.MaxFavorability[selfBehaviorType];
			return favorabilityType <= num;
		}

		public static bool CanEndRelation_Enemy(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
		{
			if ((selfToTarget.RelationType & 0x8000) == 0)
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
			AiRelationsItem aiRelationConfig = GetAiRelationConfig(1);
			short num = aiRelationConfig.MinFavorability[selfBehaviorType];
			return favorabilityType >= num;
		}

		public static bool CanStartRelation_Friend(RelatedCharacter selfToTarget, sbyte selfBehaviorType, RelatedCharacter targetToSelf, sbyte targetBehaviorType)
		{
			if ((selfToTarget.RelationType & 0x2000) != 0)
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
			sbyte favorabilityType2 = FavorabilityType.GetFavorabilityType(targetToSelf.Favorability);
			AiRelationsItem aiRelationConfig = GetAiRelationConfig(6);
			short num = aiRelationConfig.MinFavorability[selfBehaviorType];
			short num2 = aiRelationConfig.MinFavorability[targetBehaviorType];
			return favorabilityType >= num && favorabilityType2 >= num2;
		}

		public static bool CanEndRelation_Friend(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
		{
			if ((selfToTarget.RelationType & 0x2000) == 0)
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
			AiRelationsItem aiRelationConfig = GetAiRelationConfig(7);
			short num = aiRelationConfig.MaxFavorability[selfBehaviorType];
			return favorabilityType <= num;
		}

		public static bool CanStartRelation_SwornBrotherOrSister(RelatedCharacter selfToTarget, sbyte selfBehaviorType, RelatedCharacter targetToSelf, sbyte targetBehaviorType)
		{
			if (RelationType.ContainBloodExclusionRelations(selfToTarget.RelationType))
			{
				return false;
			}
			if (!RelationType.HasRelation(selfToTarget.RelationType, 8192))
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
			sbyte favorabilityType2 = FavorabilityType.GetFavorabilityType(targetToSelf.Favorability);
			AiRelationsItem aiRelationConfig = GetAiRelationConfig(8);
			short num = aiRelationConfig.MinFavorability[selfBehaviorType];
			short num2 = aiRelationConfig.MinFavorability[targetBehaviorType];
			return favorabilityType >= num && favorabilityType2 >= num2;
		}

		public static bool CanEndRelation_SwornBrotherOrSister(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
		{
			if ((selfToTarget.RelationType & 0x200) == 0)
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
			AiRelationsItem aiRelationConfig = GetAiRelationConfig(9);
			short num = aiRelationConfig.MaxFavorability[selfBehaviorType];
			return favorabilityType <= num;
		}

		public static bool CanStartRelation_Adored(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
		{
			if ((selfToTarget.RelationType & 0x4000) != 0)
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
			AiRelationsItem aiRelationConfig = GetAiRelationConfig(2);
			short num = aiRelationConfig.MinFavorability[selfBehaviorType];
			return favorabilityType >= num;
		}

		public static bool CanStartRelation_BoyOrGirlFriend(RelatedCharacter selfToTarget, sbyte selfBehaviorType, RelatedCharacter targetToSelf, sbyte targetBehaviorType)
		{
			if ((selfToTarget.RelationType & 0x4000) == 0 || (targetToSelf.RelationType & 0x4000) != 0)
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
			AiRelationsItem aiRelationConfig = GetAiRelationConfig(3);
			short num = aiRelationConfig.MinFavorability[selfBehaviorType];
			return favorabilityType >= num;
		}

		public static bool CanEndRelation_BoyOrGirlFriend(RelatedCharacter selfToTarget, sbyte selfBehaviorType, RelatedCharacter targetToSelf, sbyte targetBehaviorType)
		{
			if ((selfToTarget.RelationType & 0x4000) == 0 || (targetToSelf.RelationType & 0x4000) == 0)
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
			AiRelationsItem aiRelationConfig = GetAiRelationConfig(4);
			short num = aiRelationConfig.MaxFavorability[selfBehaviorType];
			return favorabilityType <= num;
		}

		public static bool CanStartRelation_HusbandOrWife(int selfCharId, RelatedCharacter selfToTarget, sbyte selfBehaviorType, int targetCharId, RelatedCharacter targetToSelf, sbyte targetBehaviorType)
		{
			if ((selfToTarget.RelationType & 0x4000) == 0 || (targetToSelf.RelationType & 0x4000) == 0 || RelationType.ContainBloodExclusionRelations(selfToTarget.RelationType) || DomainManager.Character.GetAliveSpouse(selfCharId) >= 0 || DomainManager.Character.GetAliveSpouse(targetCharId) >= 0)
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
			AiRelationsItem aiRelationConfig = GetAiRelationConfig(5);
			short num = aiRelationConfig.MinFavorability[selfBehaviorType];
			return favorabilityType >= num;
		}

		public static bool CanEndRelation_HusbandOrWife(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
		{
			if ((selfToTarget.RelationType & 0x400) == 0)
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
			AiRelationsItem aiRelationConfig = GetAiRelationConfig(12);
			short num = aiRelationConfig.MaxFavorability[selfBehaviorType];
			return favorabilityType <= num;
		}

		public static bool CanStartRelation_AdoptiveParent(int selfCharId, RelatedCharacter selfToTarget, short selfAge, int targetCharId, RelatedCharacter targetToSelf, short targetAge)
		{
			return !RelationType.ContainBloodExclusionRelations(selfToTarget.RelationType) && FavorabilityType.GetFavorabilityType(selfToTarget.Favorability) >= 4 && FavorabilityType.GetFavorabilityType(targetToSelf.Favorability) >= 4 && selfAge < 30 && targetAge >= 30 && DomainManager.Character.GetAliveParent(selfCharId) < 0 && DomainManager.Character.GetAliveChild(targetCharId) < 0;
		}

		public static bool CanStartRelation_AdoptiveChild(int selfCharId, RelatedCharacter selfToTarget, short selfAge, int targetCharId, RelatedCharacter targetToSelf, short targetAge)
		{
			return !RelationType.ContainBloodExclusionRelations(selfToTarget.RelationType) && FavorabilityType.GetFavorabilityType(selfToTarget.Favorability) >= 4 && FavorabilityType.GetFavorabilityType(targetToSelf.Favorability) >= 4 && selfAge >= 30 && targetAge < 30 && DomainManager.Character.GetAliveChild(selfCharId) < 0 && DomainManager.Character.GetAliveParent(targetCharId) < 0;
		}

		public static int GetStartOrEndRelationChance(AiRelationsItem aiRelationsCfg, Character selfChar, Character targetChar, ushort curRelationType, sbyte sectFavorability, int multiplier = 1)
		{
			sbyte behaviorType = selfChar.GetBehaviorType();
			sbyte behaviorType2 = targetChar.GetBehaviorType();
			sbyte fameType = selfChar.GetFameType();
			sbyte fameType2 = targetChar.GetFameType();
			int num = 0;
			if (aiRelationsCfg.NoncontradictoryBehaviorAjust != 0 && !BehaviorType.IsContradictory(behaviorType, behaviorType2))
			{
				num += aiRelationsCfg.NoncontradictoryBehaviorAjust;
			}
			if (aiRelationsCfg.NoncontradictoryFameAjust != 0 && !FameType.IsContradictory(fameType, fameType2))
			{
				num += aiRelationsCfg.NoncontradictoryFameAjust;
			}
			if (aiRelationsCfg.EnemySectMemberAdjust != 0 && sectFavorability == -1)
			{
				num += aiRelationsCfg.EnemySectMemberAdjust;
			}
			else if (aiRelationsCfg.FriendlySectMemberAdjust != 0 && sectFavorability == 1)
			{
				num += aiRelationsCfg.FriendlySectMemberAdjust;
			}
			int num2 = 100;
			RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(selfChar.GetId());
			switch (aiRelationsCfg.TemplateId)
			{
			case 0:
				if (selfChar.GetFeatureIds().Contains(485) || targetChar.GetFeatureIds().Contains(485))
				{
					num2 *= 10;
				}
				break;
			case 2:
			{
				int count = relatedCharacters.Adored.GetCount();
				if (count > 0)
				{
					sbyte b = RelationsRelatedConstants.StartAdoreChanceChangePerAdored[behaviorType];
					int num3 = CalcAdoreMultiplePeopleChanceFactor(selfChar);
					num2 += count * b * num3 / 100;
				}
				if (selfChar.GetFeatureIds().Contains(484) || targetChar.GetFeatureIds().Contains(484))
				{
					num2 *= 10;
				}
				break;
			}
			case 3:
			{
				List<PersonalNeed> personalNeeds2 = selfChar.GetPersonalNeeds();
				if (personalNeeds2.Exists((PersonalNeed need) => need.TemplateId == 25 && need.RelationType == 16384))
				{
					num2 = num2 * 150 / 100;
				}
				break;
			}
			case 4:
				num2 += 20 * (relatedCharacters.Adored.GetCount() - 1);
				break;
			case 8:
			{
				num2 -= 20 * relatedCharacters.SwornBrothersAndSisters.GetCount();
				List<PersonalNeed> personalNeeds = selfChar.GetPersonalNeeds();
				if (personalNeeds.Exists((PersonalNeed need) => need.TemplateId == 25 && need.RelationType == 512))
				{
					num2 = num2 * 150 / 100;
				}
				if (RelationType.HasRelation(curRelationType, 16384))
				{
					num -= 2000;
				}
				break;
			}
			}
			num = ((!RelationType.ContainDirectBloodRelations(curRelationType) && !RelationType.HasRelation(curRelationType, 1024)) ? ((!RelationType.ContainNonBloodFamilyRelations(curRelationType) && !RelationType.HasRelation(curRelationType, 2048) && !RelationType.HasRelation(curRelationType, 4096)) ? (num + aiRelationsCfg.Probability[behaviorType].DefaultProb) : (num + aiRelationsCfg.Probability[behaviorType].SwornOrAdoptiveRelationsProb)) : (num + aiRelationsCfg.Probability[behaviorType].BloodRelationsProb));
			return Math.Max(0, num * num2 * multiplier / 100);
		}

		public static int GetStartRelationSuccessRate_Adored(Character selfChar, Character targetChar, RelatedCharacter selfToTarget, RelatedCharacter targetToSelf)
		{
			int num = GetStartRelationSuccessRate_SexRelationBaseRate(targetChar, selfChar, targetToSelf, selfToTarget);
			if (num == int.MinValue)
			{
				return 0;
			}
			if (RelationType.ContainDirectBloodRelations(selfToTarget.RelationType))
			{
				sbyte behaviorType = selfChar.GetBehaviorType();
				num = num * RelationsRelatedConstants.IncestSexRelationChanceFactor[behaviorType] / 100;
			}
			return num;
		}

		public static int GetStartRelationSuccessRate_BoyOrGirlFriend(Character selfChar, Character targetChar, RelatedCharacter selfToTarget, RelatedCharacter targetToSelf, bool showCheckAnim = false)
		{
			EventInteractCheckData eventInteractCheckData = null;
			if (showCheckAnim)
			{
				eventInteractCheckData = new EventInteractCheckData(5)
				{
					ConfessionLovePureFixProbDict = new Dictionary<int, int>(),
					ConfessionLoveSecularFixProbDict = new Dictionary<int, int>(),
					FailPhase = -1,
					SelfNameRelatedData = DomainManager.Character.GetNameRelatedData(selfChar.GetId()),
					TargetNameRelatedData = DomainManager.Character.GetNameRelatedData(targetChar.GetId()),
					PhaseProbList = new List<int> { 0, 0 }
				};
				DomainManager.TaiwuEvent.InteractCheckData = eventInteractCheckData;
				DomainManager.TaiwuEvent.ShowInteractCheckAnimation = true;
			}
			int startRelationSuccessRate_SexRelationBaseRate = GetStartRelationSuccessRate_SexRelationBaseRate(selfChar, targetChar, selfToTarget, targetToSelf, eventInteractCheckData);
			if (startRelationSuccessRate_SexRelationBaseRate == int.MinValue)
			{
				return 0;
			}
			startRelationSuccessRate_SexRelationBaseRate -= 20;
			int aliveSpouse = DomainManager.Character.GetAliveSpouse(selfChar.GetId());
			int aliveSpouse2 = DomainManager.Character.GetAliveSpouse(targetChar.GetId());
			int num = 0;
			if (aliveSpouse >= 0 && aliveSpouse != targetChar.GetId())
			{
				num -= 60;
				startRelationSuccessRate_SexRelationBaseRate -= 60;
			}
			if (aliveSpouse2 >= 0 && aliveSpouse2 != selfChar.GetId())
			{
				num -= 60;
				startRelationSuccessRate_SexRelationBaseRate -= 60;
			}
			startRelationSuccessRate_SexRelationBaseRate += DomainManager.Extra.GetAiActionSuccessRateAdjust(selfChar.GetId(), targetChar.GetId(), 10, RelationType.GetTypeId(16384));
			if (RelationType.ContainDirectBloodRelations(targetToSelf.RelationType))
			{
				sbyte behaviorType = targetChar.GetBehaviorType();
				startRelationSuccessRate_SexRelationBaseRate = startRelationSuccessRate_SexRelationBaseRate * RelationsRelatedConstants.IncestSexRelationChanceFactor[behaviorType] / 100;
			}
			if (eventInteractCheckData != null)
			{
				eventInteractCheckData.ConfessionLoveSecularFixProbDict.Add(3, num);
				eventInteractCheckData.PhaseProbList[0] = startRelationSuccessRate_SexRelationBaseRate;
				eventInteractCheckData.PhaseProbList[1] = startRelationSuccessRate_SexRelationBaseRate;
			}
			return startRelationSuccessRate_SexRelationBaseRate;
		}

		public static int GetStartRelationSuccessRate_HusbandOrWife(Character selfChar, Character targetChar, RelatedCharacter selfToTarget, RelatedCharacter targetToSelf)
		{
			int startRelationSuccessRate_SexRelationBaseRate = GetStartRelationSuccessRate_SexRelationBaseRate(selfChar, targetChar, selfToTarget, targetToSelf);
			if (startRelationSuccessRate_SexRelationBaseRate == int.MinValue)
			{
				return 0;
			}
			return startRelationSuccessRate_SexRelationBaseRate - 40;
		}

		public static int GetStartRelationSuccessRate_SexRelationBaseRate(Character selfChar, Character targetChar, RelatedCharacter selfToTarget, RelatedCharacter targetToSelf, EventInteractCheckData eventInteractCheckData = null)
		{
			int num = 100;
			sbyte gender = selfChar.GetGender();
			sbyte displayingGender = selfChar.GetDisplayingGender();
			sbyte gender2 = targetChar.GetGender();
			sbyte displayingGender2 = targetChar.GetDisplayingGender();
			eventInteractCheckData?.ConfessionLovePureFixProbDict.Add(0, 0);
			eventInteractCheckData?.ConfessionLovePureFixProbDict.Add(2, 0);
			bool bisexual = targetChar.GetBisexual();
			bool flag = gender == gender2;
			bool flag2 = displayingGender == displayingGender2;
			if ((bisexual && !flag && !flag2) || (!bisexual && flag && flag2))
			{
				if (eventInteractCheckData != null)
				{
					eventInteractCheckData.ConfessionLovePureFixProbDict[0] = int.MinValue;
				}
				return int.MinValue;
			}
			if (!bisexual && flag && !flag2)
			{
				if (eventInteractCheckData != null)
				{
					eventInteractCheckData.ConfessionLovePureFixProbDict[0] = -20;
				}
				num -= 20;
			}
			if (bisexual && !flag && flag2)
			{
				if (eventInteractCheckData != null)
				{
					eventInteractCheckData.ConfessionLovePureFixProbDict[2] = -20;
				}
				num -= 20;
			}
			int value = 0;
			if (flag && selfChar.GetBisexual() && targetChar.GetBisexual())
			{
				value = 20;
				num += 20;
			}
			eventInteractCheckData?.ConfessionLovePureFixProbDict.Add(1, value);
			value = 0;
			if (RelationType.HasRelation(targetToSelf.RelationType, 1024))
			{
				num += 40;
				value += 40;
			}
			if (RelationType.ContainDirectBloodRelations(targetToSelf.RelationType))
			{
				num -= 160;
				value -= 160;
			}
			if (RelationType.ContainNonBloodFamilyRelations(targetToSelf.RelationType))
			{
				num -= 80;
				value -= 80;
			}
			if (RelationType.HasRelation(selfToTarget.RelationType, 2048) || RelationType.HasRelation(selfToTarget.RelationType, 4096))
			{
				num -= 40;
				value -= 40;
			}
			eventInteractCheckData?.ConfessionLoveSecularFixProbDict.Add(1, value);
			value = 0;
			short currAge = selfChar.GetCurrAge();
			short currAge2 = targetChar.GetCurrAge();
			if (selfChar.GetGender() == targetChar.GetGender())
			{
				int num2 = Math.Clamp(3 * Math.Abs(currAge - currAge2), 0, 60);
				num -= num2;
				value -= num2;
			}
			else
			{
				int num3 = ((selfChar.GetGender() != 1) ? ((currAge2 > currAge) ? Math.Clamp(3 * (currAge2 - currAge - 10), 0, 60) : Math.Clamp(3 * (currAge - currAge2), 0, 60)) : ((currAge > currAge2) ? Math.Clamp(3 * (currAge - currAge2 - 10), 0, 60) : Math.Clamp(3 * (currAge2 - currAge), 0, 60)));
				num -= num3;
				value -= num3;
			}
			eventInteractCheckData?.ConfessionLovePureFixProbDict.Add(4, value);
			value = 0;
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(targetToSelf.Favorability);
			if (favorabilityType >= 3)
			{
				int num4 = (favorabilityType - 3) * 60;
				num += num4;
				value += num4;
			}
			eventInteractCheckData?.ConfessionLovePureFixProbDict.Add(5, value);
			value = 0;
			sbyte attractionType = AttractionType.GetAttractionType(selfChar.GetAttraction());
			sbyte attractionType2 = AttractionType.GetAttractionType(targetChar.GetAttraction());
			int num5 = (attractionType - attractionType2) * 10;
			num += num5;
			value += num5;
			eventInteractCheckData?.ConfessionLovePureFixProbDict.Add(6, value);
			value = 0;
			int num6 = (selfChar.GetInteractionGrade() - targetChar.GetInteractionGrade()) * 10;
			num += num6;
			value += num6;
			eventInteractCheckData?.ConfessionLoveSecularFixProbDict.Add(0, value);
			value = 0;
			if (selfChar.GetDisplayingGender() == 0)
			{
				value += 20;
				num += 20;
			}
			eventInteractCheckData?.ConfessionLoveSecularFixProbDict.Add(2, value);
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			if (selfChar.GetId() != taiwuCharId && targetChar.GetId() != taiwuCharId)
			{
				num += 20;
			}
			value = 0;
			RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(targetChar.GetId());
			HashSet<int> collection = relatedCharacters.Adored.GetCollection();
			int id = selfChar.GetId();
			int num7 = 0;
			foreach (int item in collection)
			{
				if (DomainManager.Character.IsCharacterAlive(item) && item != id)
				{
					num7++;
				}
			}
			if (num7 > 0)
			{
				int num8 = CalcAdoreMultiplePeopleChanceFactor(targetChar);
				sbyte b = RelationsRelatedConstants.StartAdoreChanceChangePerAdored[targetChar.GetBehaviorType()];
				int num9 = num7 * b * num8 / 100;
				num += num9;
				value += num9;
			}
			eventInteractCheckData?.ConfessionLovePureFixProbDict.Add(3, value);
			value = 0;
			int num10 = 0;
			RelatedCharacters relatedCharacters2 = DomainManager.Character.GetRelatedCharacters(selfChar.GetId());
			num10 += relatedCharacters2.BloodChildren.GetCount() + relatedCharacters2.StepChildren.GetCount();
			num10 += relatedCharacters.BloodChildren.GetCount() + relatedCharacters.StepChildren.GetCount();
			int num11 = 10 * num10;
			num -= num11;
			value -= num11;
			eventInteractCheckData?.ConfessionLoveSecularFixProbDict.Add(4, value);
			value = 0;
			if (selfChar.GetMonkType() != 0)
			{
				num -= 80;
				value -= 80;
			}
			if (targetChar.GetMonkType() != 0)
			{
				num -= 80;
				value -= 80;
			}
			eventInteractCheckData?.ConfessionLoveSecularFixProbDict.Add(6, value);
			value = 0;
			if (selfChar.GetFertility() <= 0)
			{
				value -= 30;
				num -= 30;
			}
			if (targetChar.GetFertility() <= 0)
			{
				value -= 30;
				num -= 30;
			}
			eventInteractCheckData?.ConfessionLoveSecularFixProbDict.Add(5, value);
			return num;
		}

		private static int CalcAdoreMultiplePeopleChanceFactor(Character character)
		{
			int num = 0;
			int num2 = 0;
			foreach (short featureId in character.GetFeatureIds())
			{
				short adoreMultiplePeopleChanceFactor = CharacterFeature.Instance[featureId].AdoreMultiplePeopleChanceFactor;
				if (adoreMultiplePeopleChanceFactor != 100)
				{
					num += adoreMultiplePeopleChanceFactor;
					num2++;
				}
			}
			if (num2 != 0)
			{
				return num / num2;
			}
			return 100;
		}
	}
}
