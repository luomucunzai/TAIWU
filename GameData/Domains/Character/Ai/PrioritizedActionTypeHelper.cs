using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace GameData.Domains.Character.Ai
{
	// Token: 0x02000850 RID: 2128
	public static class PrioritizedActionTypeHelper
	{
		// Token: 0x06007681 RID: 30337 RVA: 0x00455B88 File Offset: 0x00453D88
		public static BasePrioritizedAction CreatePrioritizedAction(short prioritizedActionTemplateId)
		{
			if (!true)
			{
			}
			BasePrioritizedAction result;
			switch (prioritizedActionTemplateId)
			{
			case 0:
				result = new JoinSectAction();
				break;
			case 1:
				result = new AppointmentAction();
				break;
			case 2:
				result = new ProtectFriendOrFamilyAction();
				break;
			case 3:
				result = new RescueFriendOrFamilyAction();
				break;
			case 4:
				result = new MournAction();
				break;
			case 5:
				result = new VisitFriendOrFamilyAction();
				break;
			case 6:
				result = new FindTreasureAction();
				break;
			case 7:
				result = new FindSpecialMaterialAction();
				break;
			case 8:
				result = new TakeRevengeAction();
				break;
			case 9:
				result = new ContestForLegendaryBookAction();
				break;
			case 10:
				result = new AdoptInfantAction();
				break;
			case 11:
				result = new SectStoryYuanshanToFightDemonAction();
				break;
			case 12:
				result = new SectStoryShixiangToFightEnemyAction();
				break;
			case 13:
				result = new SectStoryEmeiToFightComradeAction();
				break;
			case 14:
				result = new DejaVuAction();
				break;
			case 15:
				result = new GuardTreasuryAction();
				break;
			case 16:
				result = new SectStoryBaihuaToCureManic();
				break;
			case 17:
				result = new HuntFugitiveAction();
				break;
			case 18:
				result = new EscapeFromPrisonAction();
				break;
			case 19:
				result = new SeekAsylumAction();
				break;
			case 20:
				result = new EscortPrisonerAction();
				break;
			case 21:
				result = new VillagerRoleArrangementAction();
				break;
			case 22:
				result = new HuntTaiwuAction();
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid PrioritizedActionType ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(prioritizedActionTemplateId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06007682 RID: 30338 RVA: 0x00455D14 File Offset: 0x00453F14
		public static int GetMaxDurationByPrioritizedActionTemplateId(short templateId)
		{
			int duration = PrioritizedActions.Instance[templateId].Duration;
			return (duration >= 0) ? duration : int.MaxValue;
		}

		// Token: 0x06007683 RID: 30339 RVA: 0x00455D44 File Offset: 0x00453F44
		public static BasePrioritizedAction TryCreatePrioritizedAction(DataContext context, Character character, short prioritizedActionTemplateId, ref PrioritizedActionConditions generalConditions)
		{
			PrioritizedActionsItem config = PrioritizedActions.Instance[prioritizedActionTemplateId];
			bool flag = config.IsAdultOnly && !generalConditions.IsAdult;
			BasePrioritizedAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = config.IsNonLeader && !generalConditions.IsLeader;
				if (flag2)
				{
					result = null;
				}
				else
				{
					bool flag3 = config.IsNonTaiwuTeammate && generalConditions.IsTaiwuTeammate;
					if (flag3)
					{
						result = null;
					}
					else
					{
						bool flag4 = config.IsNonMonk && !generalConditions.IsAllowMarriage;
						if (flag4)
						{
							result = null;
						}
						else
						{
							bool flag5 = config.LoafChance >= 0 && !generalConditions.CanStroll && generalConditions.LoafDice >= config.LoafChance;
							if (flag5)
							{
								result = null;
							}
							else
							{
								bool flag6 = config.OrgTemplateId.Length != 0 && !config.OrgTemplateId.Contains(generalConditions.OrgTemplateId);
								if (flag6)
								{
									result = null;
								}
								else
								{
									bool flag7 = !config.OrgGrade.Contains(generalConditions.OrgGrade);
									if (flag7)
									{
										result = null;
									}
									else
									{
										if (!true)
										{
										}
										BasePrioritizedAction basePrioritizedAction;
										switch (prioritizedActionTemplateId)
										{
										case 0:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_JoinSect(context, character);
											break;
										case 1:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_Appointment(context, character);
											break;
										case 2:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_ProtectFriendOrFamily(context, character);
											break;
										case 3:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_RescueFriendOrFamily(context, character);
											break;
										case 4:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_Mourn(context, character);
											break;
										case 5:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_VisitFriendOrFamily(context, character);
											break;
										case 6:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_FindTreasure(context, character);
											break;
										case 7:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_FindSpecialMaterial(context, character);
											break;
										case 8:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_TakeRevenge(context, character);
											break;
										case 9:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_ContestForLegendaryBook(context, character);
											break;
										case 10:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_AdoptInfant(context, character);
											break;
										case 11:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_SectStoryYuanshanToFightDemon(context, character);
											break;
										case 12:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_SectStoryShixiangToFightEnemy(context, character);
											break;
										case 13:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_SectStoryEmeiToFightComrade(context, character);
											break;
										case 14:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_DejaVuAction(context, character);
											break;
										case 15:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_GuardTreasury(context, character);
											break;
										case 16:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_SectStoryBaihuaToCureManic(context, character);
											break;
										case 17:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_HuntFugitive(context, character);
											break;
										case 18:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_EscapeFromPrison(context, character);
											break;
										case 19:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_SeekAsylum(context, character);
											break;
										case 20:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_EscortPrisoner(context, character);
											break;
										case 21:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_VillagerRoleArrangement(context, character);
											break;
										case 22:
											basePrioritizedAction = PrioritizedActionTypeHelper.TryCreateAction_HuntTaiwu(context, character);
											break;
										default:
											basePrioritizedAction = null;
											break;
										}
										if (!true)
										{
										}
										result = basePrioritizedAction;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007684 RID: 30340 RVA: 0x00455FF4 File Offset: 0x004541F4
		private static JoinSectAction TryCreateAction_JoinSect(DataContext context, Character selfChar)
		{
			OrganizationInfo orgInfo = selfChar.GetOrganizationInfo();
			OrganizationItem orgCfg = Organization.Instance[orgInfo.OrgTemplateId];
			bool flag = !orgCfg.IsCivilian;
			JoinSectAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				sbyte idealSect = selfChar.GetIdealSect();
				bool flag2 = idealSect < 0;
				if (flag2)
				{
					result = null;
				}
				else
				{
					int chance = 0;
					List<PersonalNeed> personalNeeds = selfChar.GetPersonalNeeds();
					foreach (PersonalNeed personalNeed in personalNeeds)
					{
						bool flag3 = personalNeed.TemplateId != 26;
						if (!flag3)
						{
							idealSect = personalNeed.OrgTemplateId;
							chance = 100;
							break;
						}
					}
					int selfCharId = selfChar.GetId();
					HashSet<int> enemyIds = DomainManager.Character.GetRelatedCharIds(selfCharId, 32768);
					foreach (int enemyId in enemyIds)
					{
						bool flag4 = !DomainManager.Character.IsCharacterAlive(enemyId);
						if (!flag4)
						{
							short favorability = DomainManager.Character.GetFavorability(selfCharId, enemyId);
							sbyte favorType = FavorabilityType.GetFavorabilityType(favorability);
							bool flag5 = favorType < 0;
							if (flag5)
							{
								chance += (int)(10 - favorType);
							}
						}
					}
					chance += (int)AiHelper.PrioritizedActionConstants.CivilianGradeJoinSectChance[(int)orgInfo.Grade];
					bool flag6 = !context.Random.CheckPercentProb(chance);
					if (flag6)
					{
						result = null;
					}
					else
					{
						Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(idealSect);
						int maxDuration = PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(0);
						JoinSectAction action = new JoinSectAction
						{
							Target = new NpcTravelTarget(settlement.GetLocation(), maxDuration),
							SettlementId = settlement.GetId()
						};
						result = action;
					}
				}
			}
			return result;
		}

		// Token: 0x06007685 RID: 30341 RVA: 0x004561C0 File Offset: 0x004543C0
		private static AppointmentAction TryCreateAction_Appointment(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			short targetSettlementId;
			bool flag = !DomainManager.Taiwu.TryGetElement_Appointments(selfCharId, out targetSettlementId);
			AppointmentAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
				short favorability = DomainManager.Character.GetFavorability(selfCharId, taiwuId);
				sbyte favorType = FavorabilityType.GetFavorabilityType(favorability);
				Settlement settlement = DomainManager.Organization.GetSettlement(targetSettlementId);
				int maxDuration = PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(1) + (int)favorType;
				AppointmentAction action = new AppointmentAction
				{
					Target = new NpcTravelTarget(settlement.GetLocation(), maxDuration),
					TargetCharId = taiwuId
				};
				result = action;
			}
			return result;
		}

		// Token: 0x06007686 RID: 30342 RVA: 0x00456254 File Offset: 0x00454454
		private static ProtectFriendOrFamilyAction TryCreateAction_ProtectFriendOrFamily(DataContext context, Character selfChar)
		{
			OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(selfChar.GetOrganizationInfo());
			int selfCharId = selfChar.GetId();
			Location selfLocation = selfChar.GetLocation();
			int leaderId = selfChar.GetLeaderId();
			List<int>[] prioritizedActionTargets = context.AdvanceMonthRelatedData.PrioritizedTargets.Get();
			int favorType = 0;
			int targetCharId = selfChar.SelectMaxPriorityActionTarget(prioritizedActionTargets, delegate(int charId)
			{
				bool flag2 = !DomainManager.Character.IsTargetForVengeance(charId);
				bool result2;
				if (flag2)
				{
					result2 = false;
				}
				else
				{
					Character targetChar;
					bool flag3 = !DomainManager.Character.TryGetElement_Objects(charId, out targetChar) || targetChar.GetKidnapperId() >= 0 || (leaderId >= 0 && leaderId == targetChar.GetLeaderId()) || targetChar.IsCompletelyInfected();
					if (flag3)
					{
						result2 = false;
					}
					else
					{
						Location targetLocation = targetChar.GetLocation();
						bool flag4 = !targetLocation.IsValid();
						if (flag4)
						{
							targetLocation = targetChar.GetValidLocation();
						}
						bool flag5 = targetLocation.AreaId != selfLocation.AreaId && DomainManager.Map.GetTotalTimeCost(selfChar, selfLocation.AreaId, targetLocation.AreaId) > 90;
						if (flag5)
						{
							result2 = false;
						}
						else
						{
							RelatedCharacter relation = DomainManager.Character.GetRelation(selfCharId, charId);
							favorType = (int)FavorabilityType.GetFavorabilityType(relation.Favorability);
							bool flag6 = !orgMemberCfg.CanStroll && favorType < 5;
							if (flag6)
							{
								result2 = false;
							}
							else
							{
								int chance = favorType * 20 - 40;
								result2 = context.Random.CheckPercentProb(chance);
							}
						}
					}
				}
				return result2;
			});
			bool flag = targetCharId < 0;
			ProtectFriendOrFamilyAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int maxDuration = PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(2) + favorType;
				result = new ProtectFriendOrFamilyAction
				{
					Target = new NpcTravelTarget(targetCharId, maxDuration)
				};
			}
			return result;
		}

		// Token: 0x06007687 RID: 30343 RVA: 0x0045632C File Offset: 0x0045452C
		private static RescueFriendOrFamilyAction TryCreateAction_RescueFriendOrFamily(DataContext context, Character selfChar)
		{
			OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(selfChar.GetOrganizationInfo());
			int selfCharId = selfChar.GetId();
			Location selfLocation = selfChar.GetLocation();
			List<int>[] prioritizedActionTargets = context.AdvanceMonthRelatedData.PrioritizedTargets.Get();
			int favorType = 0;
			int targetCharId = selfChar.SelectMaxPriorityActionTarget(prioritizedActionTargets, delegate(int charId)
			{
				Character targetChar;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out targetChar);
				bool result2;
				if (flag2)
				{
					result2 = false;
				}
				else
				{
					int kidnapperId = targetChar.GetKidnapperId();
					bool flag3 = kidnapperId < 0 || kidnapperId == selfCharId;
					if (flag3)
					{
						result2 = false;
					}
					else
					{
						Location targetLocation = targetChar.GetLocation();
						bool flag4 = !targetLocation.IsValid();
						if (flag4)
						{
							targetLocation = targetChar.GetValidLocation();
						}
						bool flag5 = targetLocation.AreaId != selfLocation.AreaId && DomainManager.Map.GetTotalTimeCost(selfChar, selfLocation.AreaId, targetLocation.AreaId) > 90;
						if (flag5)
						{
							result2 = false;
						}
						else
						{
							RelatedCharacter relation = DomainManager.Character.GetRelation(selfCharId, charId);
							favorType = (int)FavorabilityType.GetFavorabilityType(relation.Favorability);
							bool flag6 = !orgMemberCfg.CanStroll && favorType < 5;
							if (flag6)
							{
								result2 = false;
							}
							else
							{
								int chance = favorType * 20 - 40;
								result2 = context.Random.CheckPercentProb(chance);
							}
						}
					}
				}
				return result2;
			});
			bool flag = targetCharId < 0;
			RescueFriendOrFamilyAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int maxDuration = PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(3) + favorType;
				result = new RescueFriendOrFamilyAction
				{
					Target = new NpcTravelTarget(targetCharId, maxDuration)
				};
			}
			return result;
		}

		// Token: 0x06007688 RID: 30344 RVA: 0x004563F4 File Offset: 0x004545F4
		private static MournAction TryCreateAction_Mourn(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = -1;
			sbyte currMaxActionTargetType = 7;
			sbyte behaviorType = selfChar.GetBehaviorType();
			Location location = selfChar.GetLocation();
			sbyte[] priorityScores = AiHelper.ActionTargetType.PriorityScores[(int)behaviorType];
			OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(selfChar.GetOrganizationInfo());
			foreach (PersonalNeed need in selfChar.GetPersonalNeeds())
			{
				bool flag = need.TemplateId != 22;
				if (!flag)
				{
					RelatedCharacter relation;
					bool flag2 = !DomainManager.Character.TryGetRelation(selfCharId, need.CharId, out relation);
					if (!flag2)
					{
						Grave grave;
						bool flag3 = !DomainManager.Character.TryGetElement_Graves(need.CharId, out grave);
						if (!flag3)
						{
							sbyte actionTargetType = AiHelper.ActionTargetType.GetActionTargetType(relation.RelationType);
							bool flag4 = priorityScores[(int)actionTargetType] < priorityScores[(int)currMaxActionTargetType];
							if (!flag4)
							{
								bool flag5 = !context.Random.CheckPercentProb(80);
								if (!flag5)
								{
									Location graveLocation = grave.GetLocation();
									bool flag6 = graveLocation.AreaId != location.AreaId && DomainManager.Map.GetTotalTimeCost(selfChar, location.AreaId, graveLocation.AreaId) > 90;
									if (!flag6)
									{
										currMaxActionTargetType = actionTargetType;
										targetCharId = need.CharId;
									}
								}
							}
						}
					}
				}
			}
			bool flag7 = targetCharId < 0;
			if (flag7)
			{
				StrictTempObjectContainer<List<int>[]> prioritizedActionTargets = context.AdvanceMonthRelatedData.PrioritizedTargets;
				targetCharId = selfChar.SelectMaxPriorityActionTarget(prioritizedActionTargets.Get(), delegate(int charId)
				{
					Grave grave2;
					bool flag9 = !DomainManager.Character.TryGetElement_Graves(charId, out grave2);
					bool result2;
					if (flag9)
					{
						result2 = false;
					}
					else
					{
						Location graveLocation2 = grave2.GetLocation();
						bool flag10 = graveLocation2.AreaId != location.AreaId && DomainManager.Map.GetTotalTimeCost(selfChar, location.AreaId, graveLocation2.AreaId) > 90;
						if (flag10)
						{
							result2 = false;
						}
						else
						{
							bool flag11 = !orgMemberCfg.CanStroll;
							if (flag11)
							{
								short favorability = DomainManager.Character.GetFavorability(selfCharId, charId);
								sbyte favorType = FavorabilityType.GetFavorabilityType(favorability);
								bool flag12 = favorType < 5;
								if (flag12)
								{
									return false;
								}
							}
							result2 = context.Random.CheckPercentProb(40);
						}
					}
					return result2;
				});
			}
			bool flag8 = targetCharId < 0;
			MournAction result;
			if (flag8)
			{
				result = null;
			}
			else
			{
				int maxDuration = PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(4);
				MournAction action = new MournAction
				{
					Target = new NpcTravelTarget(targetCharId, maxDuration)
				};
				result = action;
			}
			return result;
		}

		// Token: 0x06007689 RID: 30345 RVA: 0x0045662C File Offset: 0x0045482C
		private static VisitFriendOrFamilyAction TryCreateAction_VisitFriendOrFamily(DataContext context, Character selfChar)
		{
			Location selfLocation = selfChar.GetLocation();
			StrictTempObjectContainer<List<int>[]> prioritizedActionTargets = context.AdvanceMonthRelatedData.PrioritizedTargets;
			int leaderId = selfChar.GetLeaderId();
			int selfCharId = selfChar.GetId();
			int targetCharId = selfChar.SelectMaxPriorityActionTarget(prioritizedActionTargets.Get(), delegate(int charId)
			{
				Character targetChar;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out targetChar) || targetChar.GetKidnapperId() >= 0 || (leaderId >= 0 && leaderId == targetChar.GetLeaderId());
				bool result2;
				if (flag2)
				{
					result2 = false;
				}
				else
				{
					bool flag3 = targetChar.GetCreatingType() != 1;
					if (flag3)
					{
						result2 = false;
					}
					else
					{
						bool flag4 = targetChar.IsCompletelyInfected();
						if (flag4)
						{
							result2 = false;
						}
						else
						{
							Location targetLocation = targetChar.GetLocation();
							bool flag5 = !targetLocation.IsValid();
							if (flag5)
							{
								targetLocation = targetChar.GetValidLocation();
							}
							bool flag6 = targetLocation.AreaId != selfLocation.AreaId && DomainManager.Map.GetTotalTimeCost(selfChar, selfLocation.AreaId, targetLocation.AreaId) > 90;
							if (flag6)
							{
								result2 = false;
							}
							else
							{
								RelatedCharacter relation = DomainManager.Character.GetRelation(selfCharId, charId);
								sbyte favorType = FavorabilityType.GetFavorabilityType(relation.Favorability);
								result2 = context.Random.CheckPercentProb((int)(favorType * 10 - 10));
							}
						}
					}
				}
				return result2;
			});
			bool flag = targetCharId < 0;
			VisitFriendOrFamilyAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int maxDuration = PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(5);
				VisitFriendOrFamilyAction action = new VisitFriendOrFamilyAction
				{
					Target = new NpcTravelTarget(targetCharId, maxDuration)
				};
				result = action;
			}
			return result;
		}

		// Token: 0x0600768A RID: 30346 RVA: 0x004566E4 File Offset: 0x004548E4
		private static FindTreasureAction TryCreateAction_FindTreasure(DataContext context, Character selfChar)
		{
			Location targetLocation = Location.Invalid;
			foreach (PersonalNeed need in selfChar.GetPersonalNeeds())
			{
				bool flag = need.TemplateId != 24;
				if (!flag)
				{
					targetLocation = need.Location;
					break;
				}
			}
			bool flag2 = !targetLocation.IsValid();
			FindTreasureAction result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				sbyte behaviorType = selfChar.GetBehaviorType();
				sbyte chance = AiHelper.PrioritizedActionConstants.FindTreasureBaseChance[(int)behaviorType];
				bool flag3 = !context.Random.CheckPercentProb((int)chance);
				if (flag3)
				{
					result = null;
				}
				else
				{
					int maxDuration = PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(6);
					FindTreasureAction action = new FindTreasureAction
					{
						Target = new NpcTravelTarget(targetLocation, maxDuration)
					};
					result = action;
				}
			}
			return result;
		}

		// Token: 0x0600768B RID: 30347 RVA: 0x004567C0 File Offset: 0x004549C0
		private static FindSpecialMaterialAction TryCreateAction_FindSpecialMaterial(DataContext context, Character selfChar)
		{
			sbyte behaviorType = selfChar.GetBehaviorType();
			sbyte chance = AiHelper.PrioritizedActionConstants.FindTreasureBaseChance[(int)behaviorType];
			bool flag = !context.Random.CheckPercentProb((int)chance);
			FindSpecialMaterialAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Location selfLocation = selfChar.GetLocation();
				bool flag2 = !selfLocation.IsValid();
				if (flag2)
				{
					result = null;
				}
				else
				{
					Location targetLocation = Location.Invalid;
					AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(selfLocation.AreaId);
					foreach (KeyValuePair<short, AdventureSiteData> keyValuePair in adventuresInArea.AdventureSites)
					{
						short num;
						AdventureSiteData adventureSiteData;
						keyValuePair.Deconstruct(out num, out adventureSiteData);
						short blockId = num;
						AdventureSiteData site = adventureSiteData;
						AdventureItem adventureCfg = Adventure.Instance[site.TemplateId];
						sbyte type = adventureCfg.Type;
						bool flag3 = type <= 14 && type >= 9 && site.RemainingMonths == (short)adventureCfg.KeepTime;
						if (flag3)
						{
							targetLocation = new Location(selfLocation.AreaId, blockId);
							break;
						}
					}
					bool flag4 = !targetLocation.IsValid();
					if (flag4)
					{
						result = null;
					}
					else
					{
						int maxDuration = PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(7);
						FindSpecialMaterialAction action = new FindSpecialMaterialAction
						{
							Target = new NpcTravelTarget(targetLocation, maxDuration)
						};
						result = action;
					}
				}
			}
			return result;
		}

		// Token: 0x0600768C RID: 30348 RVA: 0x00456918 File Offset: 0x00454B18
		private static TakeRevengeAction TryCreateAction_TakeRevenge(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			sbyte behaviorType = selfChar.GetBehaviorType();
			Location selfLocation = selfChar.GetLocation();
			sbyte chance = AiHelper.PrioritizedActionConstants.TakeRevengeChance[(int)behaviorType];
			OrganizationInfo orgInfo = selfChar.GetOrganizationInfo();
			OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			bool flag = !orgMemberCfg.CanStroll;
			if (flag)
			{
				chance /= 2;
			}
			chance = (sbyte)Math.Clamp(DomainManager.SpecialEffect.ModifyValue(selfCharId, 296, (int)chance, -1, -1, -1, 0, 0, 0, 0), 0, 127);
			bool flag2 = !context.Random.CheckPercentProb((int)chance);
			TakeRevengeAction result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				int targetCharId = -1;
				foreach (PersonalNeed need in selfChar.GetPersonalNeeds())
				{
					bool flag3 = need.TemplateId != 21;
					if (!flag3)
					{
						targetCharId = need.CharId;
						break;
					}
				}
				bool flag4 = targetCharId < 0;
				short favorability;
				sbyte favorType;
				if (flag4)
				{
					HashSet<int> enemyIds = DomainManager.Character.GetRelatedCharIds(selfCharId, 32768);
					CharacterMatcherItem matcher = CharacterMatcher.DefValue.CanBeRevengeTarget;
					foreach (int enemyId in enemyIds)
					{
						Character targetChar;
						bool flag5 = !DomainManager.Character.TryGetElement_Objects(enemyId, out targetChar) || !matcher.Match(targetChar);
						if (!flag5)
						{
							favorability = DomainManager.Character.GetFavorability(selfCharId, enemyId);
							favorType = FavorabilityType.GetFavorabilityType(favorability);
							bool flag6 = favorType > AiHelper.PrioritizedActionConstants.TakeRevengeMaxFavorType[(int)behaviorType];
							if (!flag6)
							{
								Location targetLocation = targetChar.GetLocation();
								bool flag7 = !targetLocation.IsValid();
								if (flag7)
								{
									targetLocation = targetChar.GetValidLocation();
								}
								bool flag8 = targetLocation.AreaId != selfLocation.AreaId && DomainManager.Map.GetTotalTimeCost(selfChar, selfLocation.AreaId, targetLocation.AreaId) > 90;
								if (!flag8)
								{
									targetCharId = enemyId;
									break;
								}
							}
						}
					}
					bool flag9 = targetCharId < 0;
					if (flag9)
					{
						return null;
					}
				}
				favorability = DomainManager.Character.GetFavorability(selfCharId, targetCharId);
				favorType = FavorabilityType.GetFavorabilityType(favorability);
				int maxDuration = PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(8) + (int)Math.Abs(favorType);
				NpcTravelTarget travelTarget = new NpcTravelTarget(targetCharId, maxDuration);
				TakeRevengeAction action = new TakeRevengeAction
				{
					Target = travelTarget
				};
				result = action;
			}
			return result;
		}

		// Token: 0x0600768D RID: 30349 RVA: 0x00456B88 File Offset: 0x00454D88
		private unsafe static ContestForLegendaryBookAction TryCreateAction_ContestForLegendaryBook(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			bool flag = !DomainManager.Character.IsCharacterTryingToContestLegendaryBook(selfCharId);
			ContestForLegendaryBookAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				sbyte happiness = selfChar.GetHappiness();
				sbyte behaviorType = selfChar.GetBehaviorType();
				int baseChance = (int)(Math.Abs(happiness) - 60 + AiHelper.LegendaryBookRelatedConstants.ContestForLegendaryBookChanceAdjust[(int)behaviorType]);
				List<sbyte> ownedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(selfCharId);
				int ownedBookCount = (ownedBookTypes != null) ? ownedBookTypes.Count : 0;
				bool flag2 = ownedBookTypes != null;
				if (flag2)
				{
					baseChance >>= ownedBookCount;
				}
				OrganizationInfo orgInfo = selfChar.GetOrganizationInfo();
				OrganizationItem orgCfg = Organization.Instance[orgInfo.OrgTemplateId];
				bool flag3 = orgInfo.Grade == 8 && orgInfo.Principal;
				if (flag3)
				{
					baseChance /= 5;
				}
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				Span<sbyte> span = new Span<sbyte>(stackalloc byte[(UIntPtr)14], 14);
				Span<sbyte> selectableBookTypes = span;
				int selectableCount = 0;
				for (sbyte combatSkillType = 0; combatSkillType < 14; combatSkillType += 1)
				{
					int ownerId = DomainManager.LegendaryBook.GetOwner(combatSkillType);
					bool flag4 = ownerId < 0 || ownerId == selfCharId;
					if (!flag4)
					{
						bool flag5 = ownerId == taiwuCharId && !DomainManager.Taiwu.CanTaiwuBeSneakyHarmfulActionTarget();
						if (!flag5)
						{
							bool flag6 = DomainManager.Extra.GetContestForLegendaryBookCharacterSet(combatSkillType).GetCount() >= 2;
							if (!flag6)
							{
								int chance = baseChance;
								bool flag7 = orgCfg.LegendaryBookTendency == combatSkillType;
								if (flag7)
								{
									chance *= 2;
								}
								bool flag8 = !context.Random.CheckPercentProb(chance);
								if (!flag8)
								{
									*selectableBookTypes[selectableCount] = combatSkillType;
									selectableCount++;
								}
							}
						}
					}
				}
				bool flag9 = selectableCount == 0;
				if (flag9)
				{
					result = null;
				}
				else
				{
					sbyte selectedBookType = *selectableBookTypes[context.Random.Next(selectableCount)];
					int targetCharId = DomainManager.LegendaryBook.GetOwner(selectedBookType);
					int maxDuration = PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(9);
					NpcTravelTarget travelTarget = new NpcTravelTarget(targetCharId, maxDuration);
					ContestForLegendaryBookAction action = new ContestForLegendaryBookAction
					{
						Target = travelTarget,
						LegendaryBookType = selectedBookType
					};
					result = action;
				}
			}
			return result;
		}

		// Token: 0x0600768E RID: 30350 RVA: 0x00456D98 File Offset: 0x00454F98
		private static SectStoryYuanshanToFightDemonAction TryCreateAction_SectStoryYuanshanToFightDemon(DataContext context, Character selfChar)
		{
			Location location = DomainManager.Character.TryGetYuanShanEnemyLocation(context);
			object result;
			if (!location.IsValid())
			{
				result = null;
			}
			else
			{
				(result = new SectStoryYuanshanToFightDemonAction()).Target = new NpcTravelTarget(location, PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(11));
			}
			return result;
		}

		// Token: 0x0600768F RID: 30351 RVA: 0x00456DDC File Offset: 0x00454FDC
		private static SectStoryShixiangToFightEnemyAction TryCreateAction_SectStoryShixiangToFightEnemy(DataContext context, Character selfChar)
		{
			Location location = DomainManager.Character.TryGetShiXiangEnemyLocation(context);
			object result;
			if (!location.IsValid())
			{
				result = null;
			}
			else
			{
				(result = new SectStoryShixiangToFightEnemyAction()).Target = new NpcTravelTarget(location, PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(12));
			}
			return result;
		}

		// Token: 0x06007690 RID: 30352 RVA: 0x00456E20 File Offset: 0x00455020
		private static AdoptInfantAction TryCreateAction_AdoptInfant(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			int infantId;
			bool flag = !DomainManager.Character.TryGetBloodRelationInfant(selfCharId, out infantId);
			if (flag)
			{
				ValueTuple<int, int> infantData;
				bool flag2 = !DomainManager.Character.TryGetClosestInfant(selfCharId, out infantData);
				if (flag2)
				{
					return null;
				}
				infantId = infantData.Item1;
			}
			bool flag3 = DomainManager.Character.InfantHasPotentialAdopter(infantId);
			AdoptInfantAction result;
			if (flag3)
			{
				result = null;
			}
			else
			{
				Character character;
				bool flag4 = !DomainManager.Character.TryGetElement_Objects(infantId, out character) || context.Random.CheckProb(50, 100);
				if (flag4)
				{
					result = null;
				}
				else
				{
					result = new AdoptInfantAction
					{
						Target = new NpcTravelTarget(infantId, PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(10))
					};
				}
			}
			return result;
		}

		// Token: 0x06007691 RID: 30353 RVA: 0x00456ED0 File Offset: 0x004550D0
		private static SectStoryEmeiToFightComradeAction TryCreateAction_SectStoryEmeiToFightComrade(DataContext context, Character selfChar)
		{
			bool flag = !DomainManager.Character.IsEMeiCharacterPsychotic(selfChar.GetId());
			SectStoryEmeiToFightComradeAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				List<int> charIds;
				DomainManager.World.GetEmeiPotentialVictims(selfChar, out charIds);
				bool flag2 = charIds.Count == 0;
				if (flag2)
				{
					result = null;
				}
				else
				{
					result = new SectStoryEmeiToFightComradeAction
					{
						Target = new NpcTravelTarget(charIds.GetRandom(context.Random), PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(13))
					};
				}
			}
			return result;
		}

		// Token: 0x06007692 RID: 30354 RVA: 0x00456F40 File Offset: 0x00455140
		private static DejaVuAction TryCreateAction_DejaVuAction(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			bool flag = DomainManager.Extra.GetDejaVuEventCharacters().Contains(selfCharId);
			DejaVuAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				short num;
				bool flag2 = !DomainManager.Extra.TryGetDreamBackLifeRecordByRelatedCharId(selfCharId, out num);
				bool flag3 = flag2;
				if (flag3)
				{
					ushort item = DomainManager.Extra.GetDreamBackRelationTypeWithTaiwu(selfCharId).Item1;
					if (item <= 2048)
					{
						if (item != 0 && item != 2048)
						{
							goto IL_87;
						}
					}
					else if (item != 4096 && item != 65535)
					{
						goto IL_87;
					}
					bool flag4 = true;
					goto IL_8A;
					IL_87:
					flag4 = false;
					IL_8A:
					flag3 = flag4;
				}
				bool flag5 = flag3;
				if (flag5)
				{
					result = null;
				}
				else
				{
					bool flag6 = !selfChar.IsNearbyLocation(DomainManager.Taiwu.GetTaiwu().GetLocation(), 5);
					if (flag6)
					{
						result = null;
					}
					else
					{
						result = new DejaVuAction
						{
							Target = new NpcTravelTarget(DomainManager.Taiwu.GetTaiwuCharId(), PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(14))
						};
					}
				}
			}
			return result;
		}

		// Token: 0x06007693 RID: 30355 RVA: 0x00457030 File Offset: 0x00455230
		private static GuardTreasuryAction TryCreateAction_GuardTreasury(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			OrganizationInfo orgInfo = selfChar.GetOrganizationInfo();
			bool flag = orgInfo.SettlementId < 0 || !OrganizationDomain.IsSect(orgInfo.OrgTemplateId);
			GuardTreasuryAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(orgInfo.SettlementId);
				bool flag2 = !settlement.Treasuries.IsGuard(selfCharId);
				if (flag2)
				{
					result = null;
				}
				else
				{
					bool flag3 = DomainManager.LegendaryBook.IsCharacterLegendaryBookOwnerOrContest(selfCharId);
					if (flag3)
					{
						result = null;
					}
					else
					{
						result = new GuardTreasuryAction
						{
							Target = new NpcTravelTarget(settlement.GetLocation(), PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(15))
						};
					}
				}
			}
			return result;
		}

		// Token: 0x06007694 RID: 30356 RVA: 0x004570D4 File Offset: 0x004552D4
		private static SectStoryBaihuaToCureManic TryCreateAction_SectStoryBaihuaToCureManic(DataContext context, Character selfChar)
		{
			EventArgBox sectArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			int currDate = DomainManager.World.GetCurrDate();
			int animalDate = -1;
			bool flag = !sectArgBox.Get("ConchShip_PresetKey_BaihuaAnimalsBackDate", ref animalDate) || currDate - animalDate < 6;
			SectStoryBaihuaToCureManic result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int targetCharId;
				bool flag2 = !DomainManager.Character.BaihuaManicCharIds.TryTake(out targetCharId);
				if (flag2)
				{
					result = null;
				}
				else
				{
					result = new SectStoryBaihuaToCureManic
					{
						Target = new NpcTravelTarget(targetCharId, PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(16))
					};
				}
			}
			return result;
		}

		// Token: 0x06007695 RID: 30357 RVA: 0x0045715C File Offset: 0x0045535C
		private static EscapeFromPrisonAction TryCreateAction_EscapeFromPrison(DataContext context, Character selfChar)
		{
			sbyte bountySectTemplateId = DomainManager.Organization.GetFugitiveBountySect(selfChar.GetId());
			bool flag = bountySectTemplateId < 0;
			EscapeFromPrisonAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(bountySectTemplateId);
				SettlementBounty bounty = sect.Prison.GetBounty(selfChar.GetId());
				bool flag2 = bounty == null;
				if (flag2)
				{
					result = null;
				}
				else
				{
					PunishmentSeverityItem severityCfg = PunishmentSeverity.Instance[bounty.PunishmentSeverity];
					bool flag3 = !severityCfg.EscapeActions.Exist(18);
					if (flag3)
					{
						result = null;
					}
					else
					{
						Location destination = selfChar.CalcFurthestEscapeDestination(context.Random);
						result = new EscapeFromPrisonAction
						{
							Target = new NpcTravelTarget(destination, int.MaxValue)
						};
					}
				}
			}
			return result;
		}

		// Token: 0x06007696 RID: 30358 RVA: 0x00457218 File Offset: 0x00455418
		private static HuntFugitiveAction TryCreateAction_HuntFugitive(DataContext context, Character selfChar)
		{
			OrganizationInfo orgInfo = selfChar.GetOrganizationInfo();
			bool flag = !OrganizationDomain.IsSect(orgInfo.OrgTemplateId);
			HuntFugitiveAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Sect sect = DomainManager.Organization.GetElement_Sects(orgInfo.SettlementId);
				List<SettlementBounty> bounties = sect.Prison.Bounties;
				bool flag2 = bounties.Count == 0;
				if (flag2)
				{
					result = null;
				}
				else
				{
					int selectedCharId = -1;
					int duration = int.MaxValue;
					sbyte consummateLevel = selfChar.GetConsummateLevel();
					foreach (SettlementBounty bounty in bounties)
					{
						Character targetChar;
						bool flag3 = !DomainManager.Character.TryGetElement_Objects(bounty.CharId, out targetChar);
						if (!flag3)
						{
							bool flag4 = targetChar.GetOrganizationInfo().OrgTemplateId != 0 && targetChar.GetId() != DomainManager.Taiwu.GetTaiwuCharId();
							if (!flag4)
							{
								bool flag5 = bounty.RequiredConsummateLevel > consummateLevel || targetChar.GetConsummateLevel() > consummateLevel;
								if (!flag5)
								{
									bool flag6 = bounty.CurrentHunterId >= 0;
									if (!flag6)
									{
										bool flag7 = targetChar.GetKidnapperId() >= 0;
										if (!flag7)
										{
											selectedCharId = bounty.CharId;
											duration = bounty.ExpireDate - DomainManager.World.GetCurrDate();
											break;
										}
									}
								}
							}
						}
					}
					bool flag8 = selectedCharId < 0;
					if (flag8)
					{
						result = null;
					}
					else
					{
						result = new HuntFugitiveAction
						{
							Target = new NpcTravelTarget(selectedCharId, duration)
						};
					}
				}
			}
			return result;
		}

		// Token: 0x06007697 RID: 30359 RVA: 0x004573B4 File Offset: 0x004555B4
		private unsafe static SeekAsylumAction TryCreateAction_SeekAsylum(DataContext context, Character selfChar)
		{
			sbyte bountySectTemplateId = DomainManager.Organization.GetFugitiveBountySect(selfChar.GetId());
			bool flag = bountySectTemplateId < 0;
			SeekAsylumAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(bountySectTemplateId);
				SettlementBounty bounty = sect.Prison.GetBounty(selfChar.GetId());
				bool flag2 = bounty == null;
				if (flag2)
				{
					result = null;
				}
				else
				{
					PunishmentSeverityItem severityCfg = PunishmentSeverity.Instance[bounty.PunishmentSeverity];
					bool flag3 = !severityCfg.EscapeActions.Exist(19);
					if (flag3)
					{
						result = null;
					}
					else
					{
						Span<sbyte> span = new Span<sbyte>(stackalloc byte[(UIntPtr)14], 14);
						SpanList<sbyte> hostileSectTemplateIds = span;
						DomainManager.Organization.GetSectTemplateIdsByFavorability(bountySectTemplateId, -1, ref hostileSectTemplateIds);
						bool flag4 = hostileSectTemplateIds.Count == 0;
						if (flag4)
						{
							result = null;
						}
						else
						{
							sbyte targetSectTemplateId = hostileSectTemplateIds.GetRandom(context.Random);
							Settlement targetSect = DomainManager.Organization.GetSettlementByOrgTemplateId(targetSectTemplateId);
							short targetSettlementId = targetSect.GetId();
							sbyte selfGender = selfChar.GetGender();
							bool flag5 = !OrganizationDomain.MeetGenderRestriction(targetSectTemplateId, selfGender);
							if (flag5)
							{
								result = null;
							}
							else
							{
								OrganizationInfo targetOrgInfo = new OrganizationInfo(targetSectTemplateId, 0, true, targetSettlementId);
								OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(targetOrgInfo);
								bool flag6 = orgMemberCfg.Gender >= 0 && orgMemberCfg.Gender != selfGender;
								if (flag6)
								{
									result = null;
								}
								else
								{
									bool flag7 = orgMemberCfg.ChildGrade < 0 && (DomainManager.Character.GetAliveSpouse(selfChar.GetId()) >= 0 || DomainManager.Character.GetAliveChild(selfChar.GetId()) >= 0);
									if (flag7)
									{
										result = null;
									}
									else
									{
										result = new SeekAsylumAction
										{
											Target = new NpcTravelTarget(targetSect.GetLocation(), int.MaxValue),
											SettlementId = targetSettlementId
										};
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007698 RID: 30360 RVA: 0x0045757C File Offset: 0x0045577C
		private static EscortPrisonerAction TryCreateAction_EscortPrisoner(DataContext context, Character selfChar)
		{
			bool flag = !selfChar.IsActiveExternalRelationState(2);
			EscortPrisonerAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int selfCharId = selfChar.GetId();
				sbyte selfOrgTemplateId = selfChar.GetOrganizationInfo().OrgTemplateId;
				List<KidnappedCharacter> kidnappedCharList = DomainManager.Character.GetKidnappedCharacters(selfCharId).GetCollection();
				foreach (KidnappedCharacter kidnappedChar in kidnappedCharList)
				{
					sbyte bountySectTemplateId = DomainManager.Organization.GetFugitiveBountySect(kidnappedChar.CharId);
					bool flag2 = bountySectTemplateId < 0;
					if (!flag2)
					{
						bool flag3 = bountySectTemplateId != selfOrgTemplateId;
						if (!flag3)
						{
							Settlement sect = DomainManager.Organization.GetSettlementByOrgTemplateId(bountySectTemplateId);
							return new EscortPrisonerAction
							{
								Target = new NpcTravelTarget(sect.GetLocation(), int.MaxValue)
								{
									TargetCharId = kidnappedChar.CharId
								}
							};
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06007699 RID: 30361 RVA: 0x00457680 File Offset: 0x00455880
		private static VillagerRoleArrangementAction TryCreateAction_VillagerRoleArrangement(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			bool flag = selfChar.GetOrganizationInfo().OrgTemplateId != 16;
			VillagerRoleArrangementAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(selfCharId);
				bool flag2 = villagerRole == null || villagerRole.ArrangementTemplateId < 0;
				if (flag2)
				{
					result = null;
				}
				else
				{
					bool flag3 = villagerRole.WorkData == null || villagerRole.WorkData.AreaId < 0;
					if (flag3)
					{
						result = null;
					}
					else
					{
						IVillagerRoleSelectLocation roleSelectLocation = villagerRole as IVillagerRoleSelectLocation;
						bool flag4 = roleSelectLocation == null;
						if (flag4)
						{
							result = null;
						}
						else
						{
							Location location = roleSelectLocation.SelectNextWorkLocation(context.Random, villagerRole.WorkData.Location);
							result = new VillagerRoleArrangementAction
							{
								Target = new NpcTravelTarget(location, int.MaxValue)
							};
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600769A RID: 30362 RVA: 0x0045774C File Offset: 0x0045594C
		private static HuntTaiwuAction TryCreateAction_HuntTaiwu(DataContext context, Character selfChar)
		{
			short jieqingSettlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(13);
			bool flag = !DomainManager.Map.IsLocationInSettlementInfluenceRange(selfChar.GetLocation(), jieqingSettlementId);
			HuntTaiwuAction result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = selfChar.IsTreasuryGuard();
				if (flag2)
				{
					result = null;
				}
				else
				{
					Character taiwu = DomainManager.Taiwu.GetTaiwu();
					List<short> featureIds = taiwu.GetFeatureIds();
					bool hasFeature = false;
					PrioritizedActionTypeHelper.<>c__DisplayClass25_0 CS$<>8__locals1;
					CS$<>8__locals1.maxJieqingPunish = 595;
					for (int i = 0; i < featureIds.Count; i++)
					{
						short featureId = featureIds[i];
						bool flag3 = featureId >= 595 && featureId <= 599;
						if (flag3)
						{
							hasFeature = true;
							bool flag4 = featureId >= CS$<>8__locals1.maxJieqingPunish;
							if (flag4)
							{
								CS$<>8__locals1.maxJieqingPunish = featureId;
							}
						}
					}
					bool flag5 = !hasFeature;
					if (flag5)
					{
						result = null;
					}
					else
					{
						bool jieqingHuntTaiwu = DomainManager.Taiwu.JieqingHuntTaiwu;
						if (jieqingHuntTaiwu)
						{
							result = null;
						}
						else
						{
							BasePrioritizedAction prioritizedAction;
							bool flag6 = DomainManager.Character.TryGetCharacterPrioritizedAction(selfChar.GetId(), out prioritizedAction);
							if (flag6)
							{
								bool flag7 = prioritizedAction.ActionType == 17;
								if (flag7)
								{
									return null;
								}
							}
							bool flag8 = !PrioritizedActionTypeHelper.CheckHuntTaiwuCondition(CS$<>8__locals1.maxJieqingPunish, (int)selfChar.GetConsummateLevel(), (int)taiwu.GetConsummateLevel());
							if (flag8)
							{
								result = null;
							}
							else
							{
								result = new HuntTaiwuAction
								{
									Target = new NpcTravelTarget(DomainManager.Taiwu.GetTaiwuCharId(), PrioritizedActionTypeHelper.<TryCreateAction_HuntTaiwu>g__GetDuration|25_0(ref CS$<>8__locals1))
								};
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600769B RID: 30363 RVA: 0x004578C8 File Offset: 0x00455AC8
		public static bool CheckHuntTaiwuCondition(short featureId, int killerConsummateLevel, int taiwuConsummateLevel)
		{
			Tester.Assert(featureId >= 595 && featureId <= 599, "");
			int consummateLevelGap = killerConsummateLevel - taiwuConsummateLevel;
			bool flag = featureId == 599 && ((consummateLevelGap >= 5 && consummateLevelGap <= 6) || killerConsummateLevel == (int)GlobalConfig.Instance.MaxConsummateLevel);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = featureId == 598 && ((consummateLevelGap >= 3 && consummateLevelGap <= 4) || killerConsummateLevel == (int)GlobalConfig.Instance.MaxConsummateLevel);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = featureId == 597 && ((consummateLevelGap >= 1 && consummateLevelGap <= 2) || killerConsummateLevel == (int)GlobalConfig.Instance.MaxConsummateLevel);
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = featureId == 596 && (consummateLevelGap == 0 || killerConsummateLevel == 0);
						if (flag4)
						{
							result = true;
						}
						else
						{
							bool flag5 = featureId == 595 && ((consummateLevelGap >= -2 && consummateLevelGap <= -1) || killerConsummateLevel == 0);
							result = flag5;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600769C RID: 30364 RVA: 0x004579CC File Offset: 0x00455BCC
		[CompilerGenerated]
		internal static int <TryCreateAction_HuntTaiwu>g__GetDuration|25_0(ref PrioritizedActionTypeHelper.<>c__DisplayClass25_0 A_0)
		{
			return (int)((A_0.maxJieqingPunish - 595 + 1) * 3);
		}
	}
}
