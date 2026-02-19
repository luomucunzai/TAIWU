using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains.Adventure;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.Relation;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Domains.TaiwuEvent;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai;

public static class PrioritizedActionTypeHelper
{
	public static BasePrioritizedAction CreatePrioritizedAction(short prioritizedActionTemplateId)
	{
		if (1 == 0)
		{
		}
		BasePrioritizedAction result = prioritizedActionTemplateId switch
		{
			0 => new JoinSectAction(), 
			1 => new AppointmentAction(), 
			2 => new ProtectFriendOrFamilyAction(), 
			3 => new RescueFriendOrFamilyAction(), 
			4 => new MournAction(), 
			5 => new VisitFriendOrFamilyAction(), 
			6 => new FindTreasureAction(), 
			7 => new FindSpecialMaterialAction(), 
			8 => new TakeRevengeAction(), 
			9 => new ContestForLegendaryBookAction(), 
			11 => new SectStoryYuanshanToFightDemonAction(), 
			12 => new SectStoryShixiangToFightEnemyAction(), 
			10 => new AdoptInfantAction(), 
			13 => new SectStoryEmeiToFightComradeAction(), 
			14 => new DejaVuAction(), 
			15 => new GuardTreasuryAction(), 
			16 => new SectStoryBaihuaToCureManic(), 
			17 => new HuntFugitiveAction(), 
			18 => new EscapeFromPrisonAction(), 
			19 => new SeekAsylumAction(), 
			20 => new EscortPrisonerAction(), 
			21 => new VillagerRoleArrangementAction(), 
			22 => new HuntTaiwuAction(), 
			_ => throw new Exception($"Invalid PrioritizedActionType {prioritizedActionTemplateId}"), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public static int GetMaxDurationByPrioritizedActionTemplateId(short templateId)
	{
		int duration = PrioritizedActions.Instance[templateId].Duration;
		return (duration >= 0) ? duration : int.MaxValue;
	}

	public static BasePrioritizedAction TryCreatePrioritizedAction(DataContext context, Character character, short prioritizedActionTemplateId, ref PrioritizedActionConditions generalConditions)
	{
		PrioritizedActionsItem prioritizedActionsItem = PrioritizedActions.Instance[prioritizedActionTemplateId];
		if (prioritizedActionsItem.IsAdultOnly && !generalConditions.IsAdult)
		{
			return null;
		}
		if (prioritizedActionsItem.IsNonLeader && !generalConditions.IsLeader)
		{
			return null;
		}
		if (prioritizedActionsItem.IsNonTaiwuTeammate && generalConditions.IsTaiwuTeammate)
		{
			return null;
		}
		if (prioritizedActionsItem.IsNonMonk && !generalConditions.IsAllowMarriage)
		{
			return null;
		}
		if (prioritizedActionsItem.LoafChance >= 0 && !generalConditions.CanStroll && generalConditions.LoafDice >= prioritizedActionsItem.LoafChance)
		{
			return null;
		}
		if (prioritizedActionsItem.OrgTemplateId.Length != 0 && !prioritizedActionsItem.OrgTemplateId.Contains(generalConditions.OrgTemplateId))
		{
			return null;
		}
		if (!prioritizedActionsItem.OrgGrade.Contains(generalConditions.OrgGrade))
		{
			return null;
		}
		if (1 == 0)
		{
		}
		BasePrioritizedAction result = prioritizedActionTemplateId switch
		{
			0 => TryCreateAction_JoinSect(context, character), 
			1 => TryCreateAction_Appointment(context, character), 
			2 => TryCreateAction_ProtectFriendOrFamily(context, character), 
			3 => TryCreateAction_RescueFriendOrFamily(context, character), 
			4 => TryCreateAction_Mourn(context, character), 
			5 => TryCreateAction_VisitFriendOrFamily(context, character), 
			6 => TryCreateAction_FindTreasure(context, character), 
			7 => TryCreateAction_FindSpecialMaterial(context, character), 
			8 => TryCreateAction_TakeRevenge(context, character), 
			9 => TryCreateAction_ContestForLegendaryBook(context, character), 
			11 => TryCreateAction_SectStoryYuanshanToFightDemon(context, character), 
			12 => TryCreateAction_SectStoryShixiangToFightEnemy(context, character), 
			10 => TryCreateAction_AdoptInfant(context, character), 
			13 => TryCreateAction_SectStoryEmeiToFightComrade(context, character), 
			14 => TryCreateAction_DejaVuAction(context, character), 
			15 => TryCreateAction_GuardTreasury(context, character), 
			16 => TryCreateAction_SectStoryBaihuaToCureManic(context, character), 
			17 => TryCreateAction_HuntFugitive(context, character), 
			18 => TryCreateAction_EscapeFromPrison(context, character), 
			19 => TryCreateAction_SeekAsylum(context, character), 
			20 => TryCreateAction_EscortPrisoner(context, character), 
			21 => TryCreateAction_VillagerRoleArrangement(context, character), 
			22 => TryCreateAction_HuntTaiwu(context, character), 
			_ => null, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static JoinSectAction TryCreateAction_JoinSect(DataContext context, Character selfChar)
	{
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		OrganizationItem organizationItem = Config.Organization.Instance[organizationInfo.OrgTemplateId];
		if (!organizationItem.IsCivilian)
		{
			return null;
		}
		sbyte b = selfChar.GetIdealSect();
		if (b < 0)
		{
			return null;
		}
		int num = 0;
		List<PersonalNeed> personalNeeds = selfChar.GetPersonalNeeds();
		foreach (PersonalNeed item in personalNeeds)
		{
			if (item.TemplateId != 26)
			{
				continue;
			}
			b = item.OrgTemplateId;
			num = 100;
			break;
		}
		int id = selfChar.GetId();
		HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(id, 32768);
		foreach (int item2 in relatedCharIds)
		{
			if (DomainManager.Character.IsCharacterAlive(item2))
			{
				short favorability = DomainManager.Character.GetFavorability(id, item2);
				sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
				if (favorabilityType < 0)
				{
					num += 10 - favorabilityType;
				}
			}
		}
		num += AiHelper.PrioritizedActionConstants.CivilianGradeJoinSectChance[organizationInfo.Grade];
		if (!context.Random.CheckPercentProb(num))
		{
			return null;
		}
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(b);
		int maxDurationByPrioritizedActionTemplateId = GetMaxDurationByPrioritizedActionTemplateId(0);
		return new JoinSectAction
		{
			Target = new NpcTravelTarget(settlementByOrgTemplateId.GetLocation(), maxDurationByPrioritizedActionTemplateId),
			SettlementId = settlementByOrgTemplateId.GetId()
		};
	}

	private static AppointmentAction TryCreateAction_Appointment(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		if (!DomainManager.Taiwu.TryGetElement_Appointments(id, out var value))
		{
			return null;
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		short favorability = DomainManager.Character.GetFavorability(id, taiwuCharId);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		Settlement settlement = DomainManager.Organization.GetSettlement(value);
		int maxDuration = GetMaxDurationByPrioritizedActionTemplateId(1) + favorabilityType;
		return new AppointmentAction
		{
			Target = new NpcTravelTarget(settlement.GetLocation(), maxDuration),
			TargetCharId = taiwuCharId
		};
	}

	private static ProtectFriendOrFamilyAction TryCreateAction_ProtectFriendOrFamily(DataContext context, Character selfChar)
	{
		OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(selfChar.GetOrganizationInfo());
		int selfCharId = selfChar.GetId();
		Location selfLocation = selfChar.GetLocation();
		int leaderId = selfChar.GetLeaderId();
		List<int>[] prioritizedActionTargets = context.AdvanceMonthRelatedData.PrioritizedTargets.Get();
		int favorType = 0;
		int num = selfChar.SelectMaxPriorityActionTarget(prioritizedActionTargets, delegate(int charId)
		{
			if (!DomainManager.Character.IsTargetForVengeance(charId))
			{
				return false;
			}
			if (!DomainManager.Character.TryGetElement_Objects(charId, out var element) || element.GetKidnapperId() >= 0 || (leaderId >= 0 && leaderId == element.GetLeaderId()) || element.IsCompletelyInfected())
			{
				return false;
			}
			Location location = element.GetLocation();
			if (!location.IsValid())
			{
				location = element.GetValidLocation();
			}
			if (location.AreaId != selfLocation.AreaId && DomainManager.Map.GetTotalTimeCost(selfChar, selfLocation.AreaId, location.AreaId) > 90)
			{
				return false;
			}
			favorType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetRelation(selfCharId, charId).Favorability);
			if (!orgMemberCfg.CanStroll && favorType < 5)
			{
				return false;
			}
			int percentProb = favorType * 20 - 40;
			return context.Random.CheckPercentProb(percentProb);
		});
		if (num < 0)
		{
			return null;
		}
		int maxDuration = GetMaxDurationByPrioritizedActionTemplateId(2) + favorType;
		return new ProtectFriendOrFamilyAction
		{
			Target = new NpcTravelTarget(num, maxDuration)
		};
	}

	private static RescueFriendOrFamilyAction TryCreateAction_RescueFriendOrFamily(DataContext context, Character selfChar)
	{
		OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(selfChar.GetOrganizationInfo());
		int selfCharId = selfChar.GetId();
		Location selfLocation = selfChar.GetLocation();
		List<int>[] prioritizedActionTargets = context.AdvanceMonthRelatedData.PrioritizedTargets.Get();
		int favorType = 0;
		int num = selfChar.SelectMaxPriorityActionTarget(prioritizedActionTargets, delegate(int charId)
		{
			if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
			{
				return false;
			}
			int kidnapperId = element.GetKidnapperId();
			if (kidnapperId < 0 || kidnapperId == selfCharId)
			{
				return false;
			}
			Location location = element.GetLocation();
			if (!location.IsValid())
			{
				location = element.GetValidLocation();
			}
			if (location.AreaId != selfLocation.AreaId && DomainManager.Map.GetTotalTimeCost(selfChar, selfLocation.AreaId, location.AreaId) > 90)
			{
				return false;
			}
			favorType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetRelation(selfCharId, charId).Favorability);
			if (!orgMemberCfg.CanStroll && favorType < 5)
			{
				return false;
			}
			int percentProb = favorType * 20 - 40;
			return context.Random.CheckPercentProb(percentProb);
		});
		if (num < 0)
		{
			return null;
		}
		int maxDuration = GetMaxDurationByPrioritizedActionTemplateId(3) + favorType;
		return new RescueFriendOrFamilyAction
		{
			Target = new NpcTravelTarget(num, maxDuration)
		};
	}

	private static MournAction TryCreateAction_Mourn(DataContext context, Character selfChar)
	{
		int selfCharId = selfChar.GetId();
		int num = -1;
		sbyte b = 7;
		sbyte behaviorType = selfChar.GetBehaviorType();
		Location location = selfChar.GetLocation();
		sbyte[] array = AiHelper.ActionTargetType.PriorityScores[behaviorType];
		OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(selfChar.GetOrganizationInfo());
		foreach (PersonalNeed personalNeed in selfChar.GetPersonalNeeds())
		{
			if (personalNeed.TemplateId != 22 || !DomainManager.Character.TryGetRelation(selfCharId, personalNeed.CharId, out var relation) || !DomainManager.Character.TryGetElement_Graves(personalNeed.CharId, out var element))
			{
				continue;
			}
			sbyte actionTargetType = AiHelper.ActionTargetType.GetActionTargetType(relation.RelationType);
			if (array[actionTargetType] >= array[b] && context.Random.CheckPercentProb(80))
			{
				Location location2 = element.GetLocation();
				if (location2.AreaId == location.AreaId || DomainManager.Map.GetTotalTimeCost(selfChar, location.AreaId, location2.AreaId) <= 90)
				{
					b = actionTargetType;
					num = personalNeed.CharId;
				}
			}
		}
		if (num < 0)
		{
			StrictTempObjectContainer<List<int>[]> prioritizedTargets = context.AdvanceMonthRelatedData.PrioritizedTargets;
			num = selfChar.SelectMaxPriorityActionTarget(prioritizedTargets.Get(), delegate(int charId)
			{
				if (!DomainManager.Character.TryGetElement_Graves(charId, out var element2))
				{
					return false;
				}
				Location location3 = element2.GetLocation();
				if (location3.AreaId != location.AreaId && DomainManager.Map.GetTotalTimeCost(selfChar, location.AreaId, location3.AreaId) > 90)
				{
					return false;
				}
				if (!orgMemberCfg.CanStroll)
				{
					short favorability = DomainManager.Character.GetFavorability(selfCharId, charId);
					sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
					if (favorabilityType < 5)
					{
						return false;
					}
				}
				return context.Random.CheckPercentProb(40);
			});
		}
		if (num < 0)
		{
			return null;
		}
		int maxDurationByPrioritizedActionTemplateId = GetMaxDurationByPrioritizedActionTemplateId(4);
		return new MournAction
		{
			Target = new NpcTravelTarget(num, maxDurationByPrioritizedActionTemplateId)
		};
	}

	private static VisitFriendOrFamilyAction TryCreateAction_VisitFriendOrFamily(DataContext context, Character selfChar)
	{
		Location selfLocation = selfChar.GetLocation();
		StrictTempObjectContainer<List<int>[]> prioritizedTargets = context.AdvanceMonthRelatedData.PrioritizedTargets;
		int leaderId = selfChar.GetLeaderId();
		int selfCharId = selfChar.GetId();
		int num = selfChar.SelectMaxPriorityActionTarget(prioritizedTargets.Get(), delegate(int charId)
		{
			if (!DomainManager.Character.TryGetElement_Objects(charId, out var element) || element.GetKidnapperId() >= 0 || (leaderId >= 0 && leaderId == element.GetLeaderId()))
			{
				return false;
			}
			if (element.GetCreatingType() != 1)
			{
				return false;
			}
			if (element.IsCompletelyInfected())
			{
				return false;
			}
			Location location = element.GetLocation();
			if (!location.IsValid())
			{
				location = element.GetValidLocation();
			}
			if (location.AreaId != selfLocation.AreaId && DomainManager.Map.GetTotalTimeCost(selfChar, selfLocation.AreaId, location.AreaId) > 90)
			{
				return false;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetRelation(selfCharId, charId).Favorability);
			return context.Random.CheckPercentProb(favorabilityType * 10 - 10);
		});
		if (num < 0)
		{
			return null;
		}
		int maxDurationByPrioritizedActionTemplateId = GetMaxDurationByPrioritizedActionTemplateId(5);
		return new VisitFriendOrFamilyAction
		{
			Target = new NpcTravelTarget(num, maxDurationByPrioritizedActionTemplateId)
		};
	}

	private static FindTreasureAction TryCreateAction_FindTreasure(DataContext context, Character selfChar)
	{
		Location targetLocation = Location.Invalid;
		foreach (PersonalNeed personalNeed in selfChar.GetPersonalNeeds())
		{
			if (personalNeed.TemplateId != 24)
			{
				continue;
			}
			targetLocation = personalNeed.Location;
			break;
		}
		if (!targetLocation.IsValid())
		{
			return null;
		}
		sbyte behaviorType = selfChar.GetBehaviorType();
		sbyte percentProb = AiHelper.PrioritizedActionConstants.FindTreasureBaseChance[behaviorType];
		if (!context.Random.CheckPercentProb(percentProb))
		{
			return null;
		}
		int maxDurationByPrioritizedActionTemplateId = GetMaxDurationByPrioritizedActionTemplateId(6);
		return new FindTreasureAction
		{
			Target = new NpcTravelTarget(targetLocation, maxDurationByPrioritizedActionTemplateId)
		};
	}

	private static FindSpecialMaterialAction TryCreateAction_FindSpecialMaterial(DataContext context, Character selfChar)
	{
		sbyte behaviorType = selfChar.GetBehaviorType();
		sbyte percentProb = AiHelper.PrioritizedActionConstants.FindTreasureBaseChance[behaviorType];
		if (!context.Random.CheckPercentProb(percentProb))
		{
			return null;
		}
		Location location = selfChar.GetLocation();
		if (!location.IsValid())
		{
			return null;
		}
		Location targetLocation = Location.Invalid;
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
		foreach (KeyValuePair<short, AdventureSiteData> adventureSite in adventuresInArea.AdventureSites)
		{
			adventureSite.Deconstruct(out var key, out var value);
			short blockId = key;
			AdventureSiteData adventureSiteData = value;
			AdventureItem adventureItem = Config.Adventure.Instance[adventureSiteData.TemplateId];
			sbyte type = adventureItem.Type;
			if (type <= 14 && type >= 9 && adventureSiteData.RemainingMonths == adventureItem.KeepTime)
			{
				targetLocation = new Location(location.AreaId, blockId);
				break;
			}
		}
		if (!targetLocation.IsValid())
		{
			return null;
		}
		int maxDurationByPrioritizedActionTemplateId = GetMaxDurationByPrioritizedActionTemplateId(7);
		return new FindSpecialMaterialAction
		{
			Target = new NpcTravelTarget(targetLocation, maxDurationByPrioritizedActionTemplateId)
		};
	}

	private static TakeRevengeAction TryCreateAction_TakeRevenge(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		sbyte behaviorType = selfChar.GetBehaviorType();
		Location location = selfChar.GetLocation();
		sbyte b = AiHelper.PrioritizedActionConstants.TakeRevengeChance[behaviorType];
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(organizationInfo);
		if (!orgMemberConfig.CanStroll)
		{
			b /= 2;
		}
		b = (sbyte)Math.Clamp(DomainManager.SpecialEffect.ModifyValue(id, 296, b), 0, 127);
		if (!context.Random.CheckPercentProb(b))
		{
			return null;
		}
		int num = -1;
		foreach (PersonalNeed personalNeed in selfChar.GetPersonalNeeds())
		{
			if (personalNeed.TemplateId != 21)
			{
				continue;
			}
			num = personalNeed.CharId;
			break;
		}
		short favorability;
		sbyte favorabilityType;
		if (num < 0)
		{
			HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(id, 32768);
			CharacterMatcherItem canBeRevengeTarget = CharacterMatcher.DefValue.CanBeRevengeTarget;
			foreach (int item in relatedCharIds)
			{
				if (!DomainManager.Character.TryGetElement_Objects(item, out var element) || !canBeRevengeTarget.Match(element))
				{
					continue;
				}
				favorability = DomainManager.Character.GetFavorability(id, item);
				favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
				if (favorabilityType <= AiHelper.PrioritizedActionConstants.TakeRevengeMaxFavorType[behaviorType])
				{
					Location location2 = element.GetLocation();
					if (!location2.IsValid())
					{
						location2 = element.GetValidLocation();
					}
					if (location2.AreaId == location.AreaId || DomainManager.Map.GetTotalTimeCost(selfChar, location.AreaId, location2.AreaId) <= 90)
					{
						num = item;
						break;
					}
				}
			}
			if (num < 0)
			{
				return null;
			}
		}
		favorability = DomainManager.Character.GetFavorability(id, num);
		favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		int maxDuration = GetMaxDurationByPrioritizedActionTemplateId(8) + Math.Abs(favorabilityType);
		NpcTravelTarget target = new NpcTravelTarget(num, maxDuration);
		return new TakeRevengeAction
		{
			Target = target
		};
	}

	private static ContestForLegendaryBookAction TryCreateAction_ContestForLegendaryBook(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		if (!DomainManager.Character.IsCharacterTryingToContestLegendaryBook(id))
		{
			return null;
		}
		sbyte happiness = selfChar.GetHappiness();
		sbyte behaviorType = selfChar.GetBehaviorType();
		int num = Math.Abs(happiness) - 60 + AiHelper.LegendaryBookRelatedConstants.ContestForLegendaryBookChanceAdjust[behaviorType];
		List<sbyte> charOwnedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(id);
		int num2 = charOwnedBookTypes?.Count ?? 0;
		if (charOwnedBookTypes != null)
		{
			num >>= num2;
		}
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		OrganizationItem organizationItem = Config.Organization.Instance[organizationInfo.OrgTemplateId];
		if (organizationInfo.Grade == 8 && organizationInfo.Principal)
		{
			num /= 5;
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		Span<sbyte> span = stackalloc sbyte[14];
		int num3 = 0;
		for (sbyte b = 0; b < 14; b++)
		{
			int owner = DomainManager.LegendaryBook.GetOwner(b);
			if (owner >= 0 && owner != id && (owner != taiwuCharId || DomainManager.Taiwu.CanTaiwuBeSneakyHarmfulActionTarget()) && DomainManager.Extra.GetContestForLegendaryBookCharacterSet(b).GetCount() < 2)
			{
				int num4 = num;
				if (organizationItem.LegendaryBookTendency == b)
				{
					num4 *= 2;
				}
				if (context.Random.CheckPercentProb(num4))
				{
					span[num3] = b;
					num3++;
				}
			}
		}
		if (num3 == 0)
		{
			return null;
		}
		sbyte b2 = span[context.Random.Next(num3)];
		int owner2 = DomainManager.LegendaryBook.GetOwner(b2);
		int maxDurationByPrioritizedActionTemplateId = GetMaxDurationByPrioritizedActionTemplateId(9);
		NpcTravelTarget target = new NpcTravelTarget(owner2, maxDurationByPrioritizedActionTemplateId);
		return new ContestForLegendaryBookAction
		{
			Target = target,
			LegendaryBookType = b2
		};
	}

	private static SectStoryYuanshanToFightDemonAction TryCreateAction_SectStoryYuanshanToFightDemon(DataContext context, Character selfChar)
	{
		Location targetLocation = DomainManager.Character.TryGetYuanShanEnemyLocation(context);
		return targetLocation.IsValid() ? new SectStoryYuanshanToFightDemonAction
		{
			Target = new NpcTravelTarget(targetLocation, GetMaxDurationByPrioritizedActionTemplateId(11))
		} : null;
	}

	private static SectStoryShixiangToFightEnemyAction TryCreateAction_SectStoryShixiangToFightEnemy(DataContext context, Character selfChar)
	{
		Location targetLocation = DomainManager.Character.TryGetShiXiangEnemyLocation(context);
		return targetLocation.IsValid() ? new SectStoryShixiangToFightEnemyAction
		{
			Target = new NpcTravelTarget(targetLocation, GetMaxDurationByPrioritizedActionTemplateId(12))
		} : null;
	}

	private static AdoptInfantAction TryCreateAction_AdoptInfant(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		if (!DomainManager.Character.TryGetBloodRelationInfant(id, out var infantId))
		{
			if (!DomainManager.Character.TryGetClosestInfant(id, out var infantData))
			{
				return null;
			}
			(infantId, _) = infantData;
		}
		if (DomainManager.Character.InfantHasPotentialAdopter(infantId))
		{
			return null;
		}
		if (!DomainManager.Character.TryGetElement_Objects(infantId, out var _) || context.Random.CheckProb(50, 100))
		{
			return null;
		}
		return new AdoptInfantAction
		{
			Target = new NpcTravelTarget(infantId, GetMaxDurationByPrioritizedActionTemplateId(10))
		};
	}

	private static SectStoryEmeiToFightComradeAction TryCreateAction_SectStoryEmeiToFightComrade(DataContext context, Character selfChar)
	{
		if (!DomainManager.Character.IsEMeiCharacterPsychotic(selfChar.GetId()))
		{
			return null;
		}
		DomainManager.World.GetEmeiPotentialVictims(selfChar, out var charIds);
		if (charIds.Count == 0)
		{
			return null;
		}
		return new SectStoryEmeiToFightComradeAction
		{
			Target = new NpcTravelTarget(charIds.GetRandom(context.Random), GetMaxDurationByPrioritizedActionTemplateId(13))
		};
	}

	private static DejaVuAction TryCreateAction_DejaVuAction(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		if (DomainManager.Extra.GetDejaVuEventCharacters().Contains(id))
		{
			return null;
		}
		short templateId;
		bool flag = !DomainManager.Extra.TryGetDreamBackLifeRecordByRelatedCharId(id, out templateId);
		bool flag2 = flag;
		if (flag2)
		{
			bool flag3;
			switch (DomainManager.Extra.GetDreamBackRelationTypeWithTaiwu(id).Item1)
			{
			case 0:
			case 2048:
			case 4096:
			case ushort.MaxValue:
				flag3 = true;
				break;
			default:
				flag3 = false;
				break;
			}
			flag2 = flag3;
		}
		if (flag2)
		{
			return null;
		}
		if (!selfChar.IsNearbyLocation(DomainManager.Taiwu.GetTaiwu().GetLocation(), 5))
		{
			return null;
		}
		return new DejaVuAction
		{
			Target = new NpcTravelTarget(DomainManager.Taiwu.GetTaiwuCharId(), GetMaxDurationByPrioritizedActionTemplateId(14))
		};
	}

	private static GuardTreasuryAction TryCreateAction_GuardTreasury(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		if (organizationInfo.SettlementId < 0 || !OrganizationDomain.IsSect(organizationInfo.OrgTemplateId))
		{
			return null;
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(organizationInfo.SettlementId);
		if (!settlement.Treasuries.IsGuard(id))
		{
			return null;
		}
		if (DomainManager.LegendaryBook.IsCharacterLegendaryBookOwnerOrContest(id))
		{
			return null;
		}
		return new GuardTreasuryAction
		{
			Target = new NpcTravelTarget(settlement.GetLocation(), GetMaxDurationByPrioritizedActionTemplateId(15))
		};
	}

	private static SectStoryBaihuaToCureManic TryCreateAction_SectStoryBaihuaToCureManic(DataContext context, Character selfChar)
	{
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
		int currDate = DomainManager.World.GetCurrDate();
		int arg = -1;
		if (!sectMainStoryEventArgBox.Get("ConchShip_PresetKey_BaihuaAnimalsBackDate", ref arg) || currDate - arg < 6)
		{
			return null;
		}
		if (!DomainManager.Character.BaihuaManicCharIds.TryTake(out var result))
		{
			return null;
		}
		return new SectStoryBaihuaToCureManic
		{
			Target = new NpcTravelTarget(result, GetMaxDurationByPrioritizedActionTemplateId(16))
		};
	}

	private static EscapeFromPrisonAction TryCreateAction_EscapeFromPrison(DataContext context, Character selfChar)
	{
		sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(selfChar.GetId());
		if (fugitiveBountySect < 0)
		{
			return null;
		}
		Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(fugitiveBountySect);
		SettlementBounty bounty = sect.Prison.GetBounty(selfChar.GetId());
		if (bounty == null)
		{
			return null;
		}
		PunishmentSeverityItem punishmentSeverityItem = PunishmentSeverity.Instance[bounty.PunishmentSeverity];
		if (!punishmentSeverityItem.EscapeActions.Exist(18))
		{
			return null;
		}
		Location targetLocation = selfChar.CalcFurthestEscapeDestination(context.Random);
		return new EscapeFromPrisonAction
		{
			Target = new NpcTravelTarget(targetLocation, int.MaxValue)
		};
	}

	private static HuntFugitiveAction TryCreateAction_HuntFugitive(DataContext context, Character selfChar)
	{
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		if (!OrganizationDomain.IsSect(organizationInfo.OrgTemplateId))
		{
			return null;
		}
		Sect element_Sects = DomainManager.Organization.GetElement_Sects(organizationInfo.SettlementId);
		List<SettlementBounty> bounties = element_Sects.Prison.Bounties;
		if (bounties.Count == 0)
		{
			return null;
		}
		int num = -1;
		int maxDuration = int.MaxValue;
		sbyte consummateLevel = selfChar.GetConsummateLevel();
		foreach (SettlementBounty item in bounties)
		{
			if (!DomainManager.Character.TryGetElement_Objects(item.CharId, out var element) || (element.GetOrganizationInfo().OrgTemplateId != 0 && element.GetId() != DomainManager.Taiwu.GetTaiwuCharId()) || item.RequiredConsummateLevel > consummateLevel || element.GetConsummateLevel() > consummateLevel || item.CurrentHunterId >= 0 || element.GetKidnapperId() >= 0)
			{
				continue;
			}
			num = item.CharId;
			maxDuration = item.ExpireDate - DomainManager.World.GetCurrDate();
			break;
		}
		if (num < 0)
		{
			return null;
		}
		return new HuntFugitiveAction
		{
			Target = new NpcTravelTarget(num, maxDuration)
		};
	}

	private static SeekAsylumAction TryCreateAction_SeekAsylum(DataContext context, Character selfChar)
	{
		sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(selfChar.GetId());
		if (fugitiveBountySect < 0)
		{
			return null;
		}
		Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(fugitiveBountySect);
		SettlementBounty bounty = sect.Prison.GetBounty(selfChar.GetId());
		if (bounty == null)
		{
			return null;
		}
		PunishmentSeverityItem punishmentSeverityItem = PunishmentSeverity.Instance[bounty.PunishmentSeverity];
		if (!punishmentSeverityItem.EscapeActions.Exist(19))
		{
			return null;
		}
		Span<sbyte> span = stackalloc sbyte[14];
		SpanList<sbyte> result = span;
		DomainManager.Organization.GetSectTemplateIdsByFavorability(fugitiveBountySect, -1, ref result);
		if (result.Count == 0)
		{
			return null;
		}
		sbyte random = result.GetRandom(context.Random);
		Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(random);
		short id = settlementByOrgTemplateId.GetId();
		sbyte gender = selfChar.GetGender();
		if (!OrganizationDomain.MeetGenderRestriction(random, gender))
		{
			return null;
		}
		OrganizationInfo orgInfo = new OrganizationInfo(random, 0, principal: true, id);
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo);
		if (orgMemberConfig.Gender >= 0 && orgMemberConfig.Gender != gender)
		{
			return null;
		}
		if (orgMemberConfig.ChildGrade < 0 && (DomainManager.Character.GetAliveSpouse(selfChar.GetId()) >= 0 || DomainManager.Character.GetAliveChild(selfChar.GetId()) >= 0))
		{
			return null;
		}
		return new SeekAsylumAction
		{
			Target = new NpcTravelTarget(settlementByOrgTemplateId.GetLocation(), int.MaxValue),
			SettlementId = id
		};
	}

	private static EscortPrisonerAction TryCreateAction_EscortPrisoner(DataContext context, Character selfChar)
	{
		if (!selfChar.IsActiveExternalRelationState(2))
		{
			return null;
		}
		int id = selfChar.GetId();
		sbyte orgTemplateId = selfChar.GetOrganizationInfo().OrgTemplateId;
		List<KidnappedCharacter> collection = DomainManager.Character.GetKidnappedCharacters(id).GetCollection();
		foreach (KidnappedCharacter item in collection)
		{
			sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(item.CharId);
			if (fugitiveBountySect < 0 || fugitiveBountySect != orgTemplateId)
			{
				continue;
			}
			Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(fugitiveBountySect);
			return new EscortPrisonerAction
			{
				Target = new NpcTravelTarget(settlementByOrgTemplateId.GetLocation(), int.MaxValue)
				{
					TargetCharId = item.CharId
				}
			};
		}
		return null;
	}

	private static VillagerRoleArrangementAction TryCreateAction_VillagerRoleArrangement(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		if (selfChar.GetOrganizationInfo().OrgTemplateId != 16)
		{
			return null;
		}
		VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(id);
		if (villagerRole == null || villagerRole.ArrangementTemplateId < 0)
		{
			return null;
		}
		if (villagerRole.WorkData == null || villagerRole.WorkData.AreaId < 0)
		{
			return null;
		}
		if (!(villagerRole is IVillagerRoleSelectLocation villagerRoleSelectLocation))
		{
			return null;
		}
		Location targetLocation = villagerRoleSelectLocation.SelectNextWorkLocation(context.Random, villagerRole.WorkData.Location);
		return new VillagerRoleArrangementAction
		{
			Target = new NpcTravelTarget(targetLocation, int.MaxValue)
		};
	}

	private static HuntTaiwuAction TryCreateAction_HuntTaiwu(DataContext context, Character selfChar)
	{
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(13);
		if (!DomainManager.Map.IsLocationInSettlementInfluenceRange(selfChar.GetLocation(), settlementIdByOrgTemplateId))
		{
			return null;
		}
		if (selfChar.IsTreasuryGuard())
		{
			return null;
		}
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<short> featureIds = taiwu.GetFeatureIds();
		bool flag = false;
		short maxJieqingPunish = 595;
		for (int i = 0; i < featureIds.Count; i++)
		{
			short num = featureIds[i];
			if (num >= 595 && num <= 599)
			{
				flag = true;
				if (num >= maxJieqingPunish)
				{
					maxJieqingPunish = num;
				}
			}
		}
		if (!flag)
		{
			return null;
		}
		if (DomainManager.Taiwu.JieqingHuntTaiwu)
		{
			return null;
		}
		if (DomainManager.Character.TryGetCharacterPrioritizedAction(selfChar.GetId(), out var action) && action.ActionType == 17)
		{
			return null;
		}
		if (!CheckHuntTaiwuCondition(maxJieqingPunish, selfChar.GetConsummateLevel(), taiwu.GetConsummateLevel()))
		{
			return null;
		}
		return new HuntTaiwuAction
		{
			Target = new NpcTravelTarget(DomainManager.Taiwu.GetTaiwuCharId(), GetDuration())
		};
		int GetDuration()
		{
			return (maxJieqingPunish - 595 + 1) * 3;
		}
	}

	public static bool CheckHuntTaiwuCondition(short featureId, int killerConsummateLevel, int taiwuConsummateLevel)
	{
		Tester.Assert(featureId >= 595 && featureId <= 599);
		int num = killerConsummateLevel - taiwuConsummateLevel;
		if (featureId == 599 && ((num >= 5 && num <= 6) || killerConsummateLevel == GlobalConfig.Instance.MaxConsummateLevel))
		{
			return true;
		}
		if (featureId == 598 && ((num >= 3 && num <= 4) || killerConsummateLevel == GlobalConfig.Instance.MaxConsummateLevel))
		{
			return true;
		}
		if (featureId == 597 && ((num >= 1 && num <= 2) || killerConsummateLevel == GlobalConfig.Instance.MaxConsummateLevel))
		{
			return true;
		}
		if (featureId == 596 && (num == 0 || killerConsummateLevel == 0))
		{
			return true;
		}
		if (featureId == 595 && ((num >= -2 && num <= -1) || killerConsummateLevel == 0))
		{
			return true;
		}
		return false;
	}
}
