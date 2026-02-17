using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Character.Relation;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x0200084B RID: 2123
	public static class AiHelper
	{
		// Token: 0x02000C1A RID: 3098
		public static class CombatRelatedConstants
		{
			// Token: 0x0400344F RID: 13391
			public const sbyte MajorVictoryExpGainRatio = 100;

			// Token: 0x04003450 RID: 13392
			public const sbyte MinorVictoryExpGainRatio = 100;

			// Token: 0x04003451 RID: 13393
			public static readonly short[] ExpGainLevels = new short[]
			{
				50,
				100,
				200,
				350,
				550,
				850,
				1200,
				1600,
				2200
			};

			// Token: 0x04003452 RID: 13394
			public static readonly sbyte[] AvoidCombatDefeatMarkCount = new sbyte[]
			{
				10,
				20,
				30,
				10
			};
		}

		// Token: 0x02000C1B RID: 3099
		public static class NpcMovementConstants
		{
			// Token: 0x04003453 RID: 13395
			public const sbyte CommonMovementDuration = 3;

			// Token: 0x04003454 RID: 13396
			public const sbyte MaxTimeForMovementPerMonth = 30;

			// Token: 0x04003455 RID: 13397
			public const sbyte RandomMovementChance = 30;
		}

		// Token: 0x02000C1C RID: 3100
		public static class LuckEventConstants
		{
			// Token: 0x04003456 RID: 13398
			public const sbyte ResourceEventChance = 15;

			// Token: 0x04003457 RID: 13399
			public const sbyte AccessoryEventChance = 10;

			// Token: 0x04003458 RID: 13400
			public const sbyte WeaponEventChance = 5;

			// Token: 0x04003459 RID: 13401
			public const sbyte ArmorEventChance = 5;

			// Token: 0x0400345A RID: 13402
			public const sbyte CombatSkillBookEventChance = 3;

			// Token: 0x0400345B RID: 13403
			public const sbyte LifeSkillBookEventChance = 5;

			// Token: 0x0400345C RID: 13404
			public const sbyte HealthEventChance = 8;

			// Token: 0x0400345D RID: 13405
			public const sbyte ResourceEventPeopleCount = 5;

			// Token: 0x0400345E RID: 13406
			public const sbyte ItemEventPeopleCount = 3;

			// Token: 0x0400345F RID: 13407
			public const sbyte HealthEventPeopleCount = 5;
		}

		// Token: 0x02000C1D RID: 3101
		public static class DemandActionType
		{
			// Token: 0x06008E25 RID: 36389 RVA: 0x004FAE6C File Offset: 0x004F906C
			public static sbyte ToMainAttributeType(sbyte actionType, bool isSkill)
			{
				sbyte result;
				switch (actionType)
				{
				case 0:
					result = -1;
					break;
				case 1:
					result = (isSkill ? 5 : 1);
					break;
				case 2:
					result = 2;
					break;
				case 3:
					result = 0;
					break;
				case 4:
					result = 3;
					break;
				default:
					throw new ArgumentOutOfRangeException("actionType");
				}
				return result;
			}

			// Token: 0x04003460 RID: 13408
			public const sbyte Request = 0;

			// Token: 0x04003461 RID: 13409
			public const sbyte Steal = 1;

			// Token: 0x04003462 RID: 13410
			public const sbyte Scam = 2;

			// Token: 0x04003463 RID: 13411
			public const sbyte Rob = 3;

			// Token: 0x04003464 RID: 13412
			public const sbyte RobGrave = 4;

			// Token: 0x04003465 RID: 13413
			public const sbyte Count = 5;

			// Token: 0x04003466 RID: 13414
			public static readonly sbyte[] ToPersonalityType = new sbyte[]
			{
				-1,
				0,
				1,
				3,
				-1
			};
		}

		// Token: 0x02000C1E RID: 3102
		public static class HarmActionType
		{
			// Token: 0x06008E27 RID: 36391 RVA: 0x004FAED8 File Offset: 0x004F90D8
			public static sbyte ToMainAttributeType(sbyte actionType)
			{
				sbyte result;
				switch (actionType)
				{
				case 0:
					result = -1;
					break;
				case 1:
					result = 4;
					break;
				case 2:
					result = 4;
					break;
				default:
					throw new ArgumentOutOfRangeException("actionType");
				}
				return result;
			}

			// Token: 0x04003467 RID: 13415
			public const sbyte Attack = 0;

			// Token: 0x04003468 RID: 13416
			public const sbyte Poison = 1;

			// Token: 0x04003469 RID: 13417
			public const sbyte PlotHarm = 2;

			// Token: 0x0400346A RID: 13418
			public const sbyte Count = 3;

			// Token: 0x0400346B RID: 13419
			public static readonly sbyte[] ToPersonalityType = new sbyte[]
			{
				3,
				0,
				1
			};
		}

		// Token: 0x02000C1F RID: 3103
		public static class GainExpActionType
		{
			// Token: 0x0400346C RID: 13420
			public const sbyte PlayCombat = 0;

			// Token: 0x0400346D RID: 13421
			public const sbyte BeatCombat = 1;

			// Token: 0x0400346E RID: 13422
			public const sbyte LifeSkillBattle = 2;

			// Token: 0x0400346F RID: 13423
			public const sbyte CricketBattle = 3;

			// Token: 0x04003470 RID: 13424
			public const sbyte Reading = 4;

			// Token: 0x04003471 RID: 13425
			public const sbyte Stroll = 5;

			// Token: 0x04003472 RID: 13426
			public const int Count = 6;

			// Token: 0x04003473 RID: 13427
			public const int RequireTargetCount = 4;

			// Token: 0x04003474 RID: 13428
			public static readonly sbyte[][] Priorities = new sbyte[][]
			{
				new sbyte[]
				{
					0,
					1,
					2,
					3,
					4,
					5
				},
				new sbyte[]
				{
					4,
					5,
					3,
					2,
					0,
					1
				},
				new sbyte[]
				{
					5,
					3,
					2,
					4,
					0,
					1
				},
				new sbyte[]
				{
					3,
					2,
					4,
					5,
					0,
					1
				},
				new sbyte[]
				{
					1,
					0,
					2,
					3,
					4,
					5
				}
			};

			// Token: 0x04003475 RID: 13429
			public static readonly sbyte[] Weights = new sbyte[]
			{
				50,
				30,
				20,
				15,
				10,
				5
			};
		}

		// Token: 0x02000C20 RID: 3104
		public static class ActionTargetType
		{
			// Token: 0x06008E2A RID: 36394 RVA: 0x004FAFC0 File Offset: 0x004F91C0
			public static sbyte GetActionTargetType(ushort selfToTarget)
			{
				bool flag = RelationType.ContainParentRelations(selfToTarget);
				sbyte result;
				if (flag)
				{
					result = 0;
				}
				else
				{
					bool flag2 = RelationType.ContainBrotherOrSisterRelations(selfToTarget);
					if (flag2)
					{
						result = 1;
					}
					else
					{
						bool flag3 = RelationType.HasRelation(selfToTarget, 512);
						if (flag3)
						{
							result = 2;
						}
						else
						{
							bool flag4 = RelationType.HasRelation(selfToTarget, 2048);
							if (flag4)
							{
								result = 3;
							}
							else
							{
								bool flag5 = RelationType.HasRelation(selfToTarget, 1024);
								if (flag5)
								{
									result = 4;
								}
								else
								{
									bool flag6 = RelationType.ContainChildRelations(selfToTarget);
									if (flag6)
									{
										result = 5;
									}
									else
									{
										bool flag7 = RelationType.HasRelation(selfToTarget, 16384);
										if (flag7)
										{
											result = 6;
										}
										else
										{
											bool flag8 = RelationType.HasRelation(selfToTarget, 8192);
											if (flag8)
											{
												result = 7;
											}
											else
											{
												result = -1;
											}
										}
									}
								}
							}
						}
					}
				}
				return result;
			}

			// Token: 0x04003476 RID: 13430
			public const sbyte Invalid = -1;

			// Token: 0x04003477 RID: 13431
			public const sbyte Parent = 0;

			// Token: 0x04003478 RID: 13432
			public const sbyte Sibling = 1;

			// Token: 0x04003479 RID: 13433
			public const sbyte SwornBrotherOrSister = 2;

			// Token: 0x0400347A RID: 13434
			public const sbyte Mentor = 3;

			// Token: 0x0400347B RID: 13435
			public const sbyte HusbandOrWife = 4;

			// Token: 0x0400347C RID: 13436
			public const sbyte Child = 5;

			// Token: 0x0400347D RID: 13437
			public const sbyte Adore = 6;

			// Token: 0x0400347E RID: 13438
			public const sbyte Friend = 7;

			// Token: 0x0400347F RID: 13439
			public const sbyte Count = 8;

			// Token: 0x04003480 RID: 13440
			public static sbyte[][] Priorities = new sbyte[][]
			{
				new sbyte[]
				{
					0,
					1,
					2,
					3,
					4,
					5,
					7,
					6
				},
				new sbyte[]
				{
					0,
					1,
					4,
					5,
					3,
					2,
					6,
					7
				},
				new sbyte[]
				{
					1,
					0,
					4,
					5,
					2,
					3,
					7,
					6
				},
				new sbyte[]
				{
					6,
					4,
					5,
					2,
					7,
					1,
					0,
					3
				},
				new sbyte[]
				{
					5,
					6,
					0,
					1,
					3,
					2,
					4,
					7
				}
			};

			// Token: 0x04003481 RID: 13441
			public static readonly sbyte[][] PriorityScores = new sbyte[][]
			{
				new sbyte[]
				{
					8,
					7,
					6,
					5,
					4,
					3,
					1,
					2
				},
				new sbyte[]
				{
					8,
					7,
					3,
					4,
					6,
					5,
					2,
					1
				},
				new sbyte[]
				{
					7,
					8,
					4,
					3,
					6,
					5,
					1,
					2
				},
				new sbyte[]
				{
					2,
					3,
					5,
					1,
					7,
					6,
					8,
					4
				},
				new sbyte[]
				{
					6,
					5,
					3,
					4,
					2,
					8,
					7,
					1
				}
			};
		}

		// Token: 0x02000C21 RID: 3105
		public static class GeneralActionConstants
		{
			// Token: 0x06008E2C RID: 36396 RVA: 0x004FB158 File Offset: 0x004F9358
			public static sbyte GetAskForHelpRespondChance(sbyte targetBehaviorType, sbyte favorabilityType)
			{
				return AiHelper.GeneralActionConstants.AskForHelpRespondBaseChance[(int)targetBehaviorType] + (favorabilityType - 3) * 10;
			}

			// Token: 0x06008E2D RID: 36397 RVA: 0x004FB17C File Offset: 0x004F937C
			public static int GetAskToTeachSkillRespondChance(Character selfChar, Character targetChar, sbyte favorabilityType, sbyte skillGrade)
			{
				OrganizationInfo selfOrgInfo = selfChar.GetOrganizationInfo();
				OrganizationInfo targetOrgInfo = targetChar.GetOrganizationInfo();
				int chance = (int)Organization.Instance[targetOrgInfo.OrgTemplateId].TeachingOutsiderProb;
				bool flag = selfOrgInfo.OrgTemplateId == targetOrgInfo.OrgTemplateId && selfOrgInfo.SettlementId == targetOrgInfo.SettlementId && selfOrgInfo.Grade >= skillGrade;
				if (flag)
				{
					chance *= 3;
				}
				chance += (int)(AiHelper.GeneralActionConstants.AskToTeachSkillRespondBaseChance[(int)targetChar.GetBehaviorType()] + (favorabilityType - 3) * 10);
				return (chance > 100) ? 100 : chance;
			}

			// Token: 0x06008E2E RID: 36398 RVA: 0x004FB208 File Offset: 0x004F9408
			public static sbyte GetResourceHappinessChange(sbyte resourceType, int amount)
			{
				sbyte grade = ResourceTypeHelper.ResourceAmountToGrade(resourceType, amount);
				return (grade < 0) ? 0 : AiHelper.GeneralActionConstants.ResourceLevelToHappinessChange[(int)grade];
			}

			// Token: 0x06008E2F RID: 36399 RVA: 0x004FB230 File Offset: 0x004F9430
			public static short GetResourceFavorabilityChange(sbyte resourceType, int amount)
			{
				sbyte grade = ResourceTypeHelper.ResourceAmountToGrade(resourceType, amount);
				return (grade < 0) ? 0 : AiHelper.GeneralActionConstants.ResourceLevelToFavorabilityChange[(int)grade];
			}

			// Token: 0x06008E30 RID: 36400 RVA: 0x004FB258 File Offset: 0x004F9458
			public static short GetBegSucceedFavorabilityChange(IRandomSource random, sbyte behaviorType)
			{
				if (!true)
				{
				}
				short result;
				switch (behaviorType)
				{
				case 0:
					result = 1000;
					break;
				case 1:
					result = 3000;
					break;
				case 2:
					result = 2000;
					break;
				case 3:
					result = (short)random.Next(-3000, 3001);
					break;
				case 4:
					result = 0;
					break;
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid behavior type ");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(behaviorType);
					throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				if (!true)
				{
				}
				return result;
			}

			// Token: 0x06008E31 RID: 36401 RVA: 0x004FB2EC File Offset: 0x004F94EC
			public static short GetBegFailFavorabilityChange(IRandomSource random, sbyte behaviorType)
			{
				if (!true)
				{
				}
				short result;
				switch (behaviorType)
				{
				case 0:
					result = 0;
					break;
				case 1:
					result = -1000;
					break;
				case 2:
					result = -2000;
					break;
				case 3:
					result = (short)random.Next(-3000, 3001);
					break;
				case 4:
					result = -3000;
					break;
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid behavior type ");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(behaviorType);
					throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				if (!true)
				{
				}
				return result;
			}

			// Token: 0x06008E32 RID: 36402 RVA: 0x004FB380 File Offset: 0x004F9580
			// Note: this type is marked as 'beforefieldinit'.
			static GeneralActionConstants()
			{
				sbyte[][] array = new sbyte[5][];
				array[0] = new sbyte[]
				{
					1
				};
				array[1] = new sbyte[]
				{
					0,
					1
				};
				array[2] = new sbyte[]
				{
					-1,
					0,
					1
				};
				int num = 3;
				sbyte[] array2 = new sbyte[2];
				array2[0] = -1;
				array[num] = array2;
				array[4] = new sbyte[]
				{
					-1
				};
				AiHelper.GeneralActionConstants.CricketBattleTargetGradeAdjust = array;
				AiHelper.GeneralActionConstants.AddPoisonOnTransferItemChance = new sbyte[]
				{
					20,
					35,
					50,
					80,
					65
				};
				AiHelper.GeneralActionConstants.TakeFromTreasuryChance = new sbyte[]
				{
					40,
					50,
					60,
					70,
					80
				};
				AiHelper.GeneralActionConstants.StoreInTreasuryChance = new sbyte[]
				{
					80,
					70,
					60,
					50,
					40
				};
			}

			// Token: 0x04003482 RID: 13442
			public static readonly byte[][] EnergyGainSpeed = new byte[][]
			{
				new byte[]
				{
					50,
					25,
					75,
					100,
					50
				},
				new byte[]
				{
					75,
					25,
					50,
					50,
					100
				},
				new byte[]
				{
					50,
					50,
					50,
					75,
					75
				},
				new byte[]
				{
					25,
					75,
					50,
					50,
					100
				},
				new byte[]
				{
					50,
					50,
					50,
					100,
					50
				}
			};

			// Token: 0x04003483 RID: 13443
			public static readonly sbyte[] AskForHelpRespondBaseChance = new sbyte[]
			{
				45,
				60,
				30,
				15,
				0
			};

			// Token: 0x04003484 RID: 13444
			public static readonly sbyte[] AskToTeachSkillRespondBaseChance = new sbyte[]
			{
				-10,
				20,
				0,
				10,
				-20
			};

			// Token: 0x04003485 RID: 13445
			public static readonly sbyte[][] StartStealingChance = new sbyte[][]
			{
				new sbyte[3],
				new sbyte[]
				{
					10,
					0,
					20
				},
				new sbyte[]
				{
					20,
					5,
					40
				},
				new sbyte[]
				{
					40,
					10,
					80
				},
				new sbyte[]
				{
					30,
					15,
					60
				}
			};

			// Token: 0x04003486 RID: 13446
			public static readonly sbyte[][] StartScammingChance = new sbyte[][]
			{
				new sbyte[3],
				new sbyte[]
				{
					10,
					0,
					20
				},
				new sbyte[]
				{
					30,
					5,
					60
				},
				new sbyte[]
				{
					40,
					15,
					80
				},
				new sbyte[]
				{
					20,
					10,
					40
				}
			};

			// Token: 0x04003487 RID: 13447
			public static readonly sbyte[][] StartRobbingChance = new sbyte[][]
			{
				new sbyte[]
				{
					10,
					0,
					60
				},
				new sbyte[3],
				new sbyte[]
				{
					20,
					5,
					20
				},
				new sbyte[]
				{
					30,
					10,
					40
				},
				new sbyte[]
				{
					40,
					15,
					80
				}
			};

			// Token: 0x04003488 RID: 13448
			public static readonly sbyte[][] StartRobbingFromGraveChance = new sbyte[][]
			{
				new sbyte[3],
				new sbyte[]
				{
					10,
					0,
					20
				},
				new sbyte[]
				{
					20,
					5,
					40
				},
				new sbyte[]
				{
					40,
					15,
					80
				},
				new sbyte[]
				{
					30,
					10,
					60
				}
			};

			// Token: 0x04003489 RID: 13449
			public const short ExtendFavorSpiritualDebtCost = 200;

			// Token: 0x0400348A RID: 13450
			public const sbyte MerchantPraiseSpiritualDebtCost = 40;

			// Token: 0x0400348B RID: 13451
			public const short CultivateWillMoneyCost = 10000;

			// Token: 0x0400348C RID: 13452
			public const short CultivateWillAuthorityCost = 1000;

			// Token: 0x0400348D RID: 13453
			public static readonly sbyte[] SocialStatusHealChance = new sbyte[]
			{
				30,
				40,
				20,
				10,
				0
			};

			// Token: 0x0400348E RID: 13454
			public static readonly sbyte[] SocialStatusDrinkTeaChance = new sbyte[]
			{
				30,
				40,
				20,
				10,
				0
			};

			// Token: 0x0400348F RID: 13455
			public static readonly sbyte[] SocialStatusDrinkWineChance = new sbyte[]
			{
				20,
				10,
				30,
				40,
				0
			};

			// Token: 0x04003490 RID: 13456
			public static readonly sbyte[] SocialStatusSellItemChance = new sbyte[]
			{
				10,
				40,
				30,
				20,
				0
			};

			// Token: 0x04003491 RID: 13457
			public static readonly sbyte[][] CricketBattleGradeOffsets = new sbyte[][]
			{
				new sbyte[]
				{
					1
				},
				new sbyte[]
				{
					0,
					1
				},
				new sbyte[]
				{
					-1,
					0,
					1
				},
				new sbyte[]
				{
					0,
					-1
				},
				new sbyte[]
				{
					-1
				}
			};

			// Token: 0x04003492 RID: 13458
			public const sbyte CaringCharBonusChance = 30;

			// Token: 0x04003493 RID: 13459
			public const sbyte HappinessChangeOnRequestFail = -3;

			// Token: 0x04003494 RID: 13460
			public const short FavorabilityChangeOnRequestFail = -3000;

			// Token: 0x04003495 RID: 13461
			public const short FavorabilityChangeOnMedicineRequestFail = -6000;

			// Token: 0x04003496 RID: 13462
			public static readonly sbyte[] ResourceLevelToHappinessChange = new sbyte[]
			{
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9
			};

			// Token: 0x04003497 RID: 13463
			public static readonly short[] ResourceLevelToFavorabilityChange = new short[]
			{
				300,
				600,
				900,
				1500,
				2100,
				2700,
				3600,
				4500,
				5400
			};

			// Token: 0x04003498 RID: 13464
			public const sbyte BarbStartChance = 10;

			// Token: 0x04003499 RID: 13465
			public const sbyte BarbBonusChance = 10;

			// Token: 0x0400349A RID: 13466
			public const short BarbMistakeFavorabilityChange = -1500;

			// Token: 0x0400349B RID: 13467
			public const short BarbFailFavorabilityChange = -3000;

			// Token: 0x0400349C RID: 13468
			public const short SocialStatusActionSucceedFavorabilityChange = 3000;

			// Token: 0x0400349D RID: 13469
			public static readonly sbyte[] StartBegChance = new sbyte[]
			{
				60,
				100,
				80,
				40,
				20
			};

			// Token: 0x0400349E RID: 13470
			public static readonly sbyte[] BegSuccessChance = new sbyte[]
			{
				0,
				30,
				40,
				20,
				10
			};

			// Token: 0x0400349F RID: 13471
			public const sbyte LifeSkillBattleMinFavorType = 2;

			// Token: 0x040034A0 RID: 13472
			public const sbyte PlayCombatMinFavorType = 3;

			// Token: 0x040034A1 RID: 13473
			public const sbyte BeatCombatMaxFavorType = 2;

			// Token: 0x040034A2 RID: 13474
			public static readonly sbyte[] ReligiousLifeSkillActionBaseChance = new sbyte[]
			{
				0,
				20,
				10,
				-10,
				-20
			};

			// Token: 0x040034A3 RID: 13475
			public static readonly sbyte[][] CricketBattleTargetGradeAdjust;

			// Token: 0x040034A4 RID: 13476
			public const short AttainmentRequiredForLifeSkillActions = 200;

			// Token: 0x040034A5 RID: 13477
			public const short IncreaseLifeSkillQualificationThreshold = 90;

			// Token: 0x040034A6 RID: 13478
			public static readonly sbyte[] AddPoisonOnTransferItemChance;

			// Token: 0x040034A7 RID: 13479
			public const sbyte InfectionAttackTaiwuCoolDown = 3;

			// Token: 0x040034A8 RID: 13480
			public const sbyte InsaneAttackTaiwuCoolDown = 3;

			// Token: 0x040034A9 RID: 13481
			public const sbyte EnemyAttackTaiwuCoolDown = 3;

			// Token: 0x040034AA RID: 13482
			public static readonly sbyte[] TakeFromTreasuryChance;

			// Token: 0x040034AB RID: 13483
			public static readonly sbyte[] StoreInTreasuryChance;
		}

		// Token: 0x02000C22 RID: 3106
		public static class ActionTargetRelationCategory
		{
			// Token: 0x06008E33 RID: 36403 RVA: 0x004FB760 File Offset: 0x004F9960
			public static sbyte GetTargetRelationCategory(ushort currRelationType)
			{
				bool flag = RelationType.HasRelation(currRelationType, 32768);
				sbyte result;
				if (flag)
				{
					result = 2;
				}
				else
				{
					bool flag2 = currRelationType == 0;
					if (flag2)
					{
						result = 0;
					}
					else
					{
						result = 1;
					}
				}
				return result;
			}
		}

		// Token: 0x02000C23 RID: 3107
		public static class UpdateStatusConstants
		{
			// Token: 0x040034AC RID: 13484
			public static readonly sbyte[] LostChildHappinessChange = new sbyte[]
			{
				-20,
				-25,
				-15,
				-10,
				0
			};

			// Token: 0x040034AD RID: 13485
			public const sbyte LostChildHealthChange = -72;

			// Token: 0x040034AE RID: 13486
			public const short KidnappedCharFavorabilityChange = -3000;

			// Token: 0x040034AF RID: 13487
			public const short KidnappedCharBecomeEnemyFavorTypeThreshold = -2;

			// Token: 0x040034B0 RID: 13488
			[TupleElementNames(new string[]
			{
				"min",
				"max"
			})]
			public static readonly ValueTuple<short, short>[] SameGroupFavorabilityChange = new ValueTuple<short, short>[]
			{
				new ValueTuple<short, short>(0, 300),
				new ValueTuple<short, short>(0, 1200),
				new ValueTuple<short, short>(0, 600),
				new ValueTuple<short, short>(0, 1200),
				new ValueTuple<short, short>(0, 300)
			};

			// Token: 0x040034B1 RID: 13489
			public static readonly sbyte[] EatForbiddenFoodChance = new sbyte[]
			{
				0,
				5,
				10,
				20,
				15
			};

			// Token: 0x040034B2 RID: 13490
			public static readonly sbyte[] EatForbiddenFoodHappinessChange = new sbyte[]
			{
				-15,
				-10,
				-5,
				5,
				0
			};
		}

		// Token: 0x02000C24 RID: 3108
		public static class ActivePreparationConstants
		{
			// Token: 0x040034B3 RID: 13491
			public static readonly sbyte[] AddPoisonBonusChance = new sbyte[]
			{
				0,
				0,
				5,
				10,
				20
			};

			// Token: 0x040034B4 RID: 13492
			public static readonly sbyte[] GradePoisonLevel = new sbyte[]
			{
				1,
				1,
				1,
				2,
				2,
				2,
				2,
				3,
				3
			};

			// Token: 0x040034B5 RID: 13493
			public static readonly short[] GradePoisonValue = new short[]
			{
				10,
				20,
				30,
				20,
				40,
				60,
				80,
				60,
				90
			};
		}

		// Token: 0x02000C25 RID: 3109
		public static class FixedActionConstants
		{
			// Token: 0x1700064F RID: 1615
			// (get) Token: 0x06008E36 RID: 36406 RVA: 0x004FB899 File Offset: 0x004F9A99
			[Obsolete("Use GameData.Domains.Character.Ai.AiHelper.JoinGroupRelationType.ToPersonalityType instead.")]
			public static sbyte[] JoinGroupChanceRelatedPersonalityType
			{
				get
				{
					return AiHelper.JoinGroupRelationType.ToPersonalityType;
				}
			}

			// Token: 0x17000650 RID: 1616
			// (get) Token: 0x06008E37 RID: 36407 RVA: 0x004FB8A0 File Offset: 0x004F9AA0
			[Obsolete("Use GameData.Domains.Character.Ai.AiHelper.JoinGroupRelationType.Priorities instead.")]
			public static sbyte[][] JoinGroupTypeOrders
			{
				get
				{
					return AiHelper.JoinGroupRelationType.Priorities;
				}
			}

			// Token: 0x040034B6 RID: 13494
			public static readonly sbyte[] BoyAndGirlFriendMakeLoveBaseChance = new sbyte[]
			{
				5,
				10,
				15,
				25,
				20
			};

			// Token: 0x040034B7 RID: 13495
			public static readonly sbyte[] RapeBaseChance = new sbyte[]
			{
				0,
				0,
				3,
				9,
				6
			};

			// Token: 0x040034B8 RID: 13496
			public const sbyte RapeImpregnateChance = 20;

			// Token: 0x040034B9 RID: 13497
			public const sbyte MutualMakeLoveImpregnateChance = 60;

			// Token: 0x040034BA RID: 13498
			public const sbyte FertilityDivisor = 20;

			// Token: 0x040034BB RID: 13499
			public const short RapeFavorabilityChange = -30000;

			// Token: 0x040034BC RID: 13500
			public static readonly sbyte[] ReleaseKidnappedCharResistanceThreshold = new sbyte[]
			{
				75,
				50,
				75,
				50,
				75
			};

			// Token: 0x040034BD RID: 13501
			public static readonly sbyte[] ReleaseKidnappedCharChance = new sbyte[]
			{
				20,
				50,
				30,
				40,
				10
			};

			// Token: 0x040034BE RID: 13502
			public static readonly sbyte[] JoinGroupBaseChance = new sbyte[]
			{
				10,
				15,
				20,
				0,
				5
			};

			// Token: 0x040034BF RID: 13503
			public static readonly sbyte[] JoinGroupFavorabilityReq = new sbyte[]
			{
				4,
				3,
				3,
				3,
				4,
				4,
				5
			};

			// Token: 0x040034C0 RID: 13504
			public static readonly sbyte[] StayInGroupMonth = new sbyte[]
			{
				12,
				6,
				12,
				12,
				6,
				6,
				6
			};

			// Token: 0x040034C1 RID: 13505
			public static readonly sbyte[] LeaveGroupChancePerMonth = new sbyte[]
			{
				10,
				30,
				15,
				15,
				10,
				30,
				30
			};

			// Token: 0x040034C2 RID: 13506
			public static readonly sbyte[] MaxLeaveGroupChance = new sbyte[]
			{
				60,
				90,
				90,
				90,
				60,
				90,
				90
			};

			// Token: 0x040034C3 RID: 13507
			public static readonly sbyte NpcGroupSoftMaxCount = 5;
		}

		// Token: 0x02000C26 RID: 3110
		public static class JoinGroupRelationType
		{
			// Token: 0x06008E39 RID: 36409 RVA: 0x004FB984 File Offset: 0x004F9B84
			public static sbyte GetJoinGroupRelationType(ushort selfToTarget, ushort targetToSelf)
			{
				bool flag = RelationType.HasRelation(selfToTarget, 1024);
				sbyte result;
				if (flag)
				{
					result = 0;
				}
				else
				{
					bool flag2 = RelationType.HasRelation(selfToTarget, 2048);
					if (flag2)
					{
						result = 1;
					}
					else
					{
						bool flag3 = RelationType.ContainParentRelations(selfToTarget);
						if (flag3)
						{
							result = 2;
						}
						else
						{
							bool flag4 = RelationType.ContainBrotherOrSisterRelations(selfToTarget);
							if (flag4)
							{
								result = 3;
							}
							else
							{
								bool flag5 = RelationType.HasRelation(selfToTarget, 16384) && RelationType.HasRelation(targetToSelf, 16384);
								if (flag5)
								{
									result = 4;
								}
								else
								{
									bool flag6 = RelationType.HasRelation(selfToTarget, 512);
									if (flag6)
									{
										result = 5;
									}
									else
									{
										bool flag7 = RelationType.HasRelation(selfToTarget, 8192);
										if (flag7)
										{
											result = 6;
										}
										else
										{
											result = -1;
										}
									}
								}
							}
						}
					}
				}
				return result;
			}

			// Token: 0x06008E3A RID: 36410 RVA: 0x004FBA30 File Offset: 0x004F9C30
			private static int CompareHusbandAndWife([TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> a, [TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> b)
			{
				int gradeCompare = a.Item1.GetOrganizationInfo().Grade.CompareTo(b.Item1.GetOrganizationInfo().Grade);
				bool flag = gradeCompare != 0;
				int result;
				if (flag)
				{
					result = gradeCompare;
				}
				else
				{
					int genderCompare = a.Item1.GetGender().CompareTo(b.Item1.GetGender());
					bool flag2 = genderCompare != 0;
					if (flag2)
					{
						result = genderCompare;
					}
					else
					{
						int simpAndLoverCountCompare = DomainManager.Character.GetReversedRelatedCharIdCount(a.Item1.GetId(), 16384).CompareTo(DomainManager.Character.GetReversedRelatedCharIdCount(b.Item1.GetId(), 16384));
						bool flag3 = simpAndLoverCountCompare != 0;
						if (flag3)
						{
							result = simpAndLoverCountCompare;
						}
						else
						{
							result = a.Item1.GetId().CompareTo(b.Item1.GetId());
						}
					}
				}
				return result;
			}

			// Token: 0x06008E3B RID: 36411 RVA: 0x004FBB1C File Offset: 0x004F9D1C
			private static int CompareMentors([TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> a, [TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> b)
			{
				return a.Item2.EstablishmentDate.CompareTo(b.Item2.EstablishmentDate);
			}

			// Token: 0x06008E3C RID: 36412 RVA: 0x004FBB4C File Offset: 0x004F9D4C
			private static int CompareParents([TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> a, [TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> b)
			{
				return RelationType.GetParentRelation(a.Item2.RelationType).CompareTo(RelationType.GetParentRelation(b.Item2.RelationType));
			}

			// Token: 0x06008E3D RID: 36413 RVA: 0x004FBB88 File Offset: 0x004F9D88
			private static int CompareSiblings([TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> a, [TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> b)
			{
				int ageCompare = a.Item1.GetCurrAge().CompareTo(b.Item1.GetCurrAge());
				bool flag = ageCompare != 0;
				int result;
				if (flag)
				{
					result = ageCompare;
				}
				else
				{
					result = a.Item1.GetId().CompareTo(b.Item1.GetId());
				}
				return result;
			}

			// Token: 0x06008E3E RID: 36414 RVA: 0x004FBBE4 File Offset: 0x004F9DE4
			private static int CompareLovers([TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> a, [TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> b)
			{
				int favorabilityCompare = a.Item2.Favorability.CompareTo(b.Item2.Favorability);
				bool flag = favorabilityCompare != 0;
				int result;
				if (flag)
				{
					result = favorabilityCompare;
				}
				else
				{
					int genderCompare = a.Item1.GetGender().CompareTo(b.Item1.GetGender());
					bool flag2 = genderCompare != 0;
					if (flag2)
					{
						result = genderCompare;
					}
					else
					{
						int simpAndLoverCountCompare = DomainManager.Character.GetReversedRelatedCharIdCount(a.Item1.GetId(), 16384).CompareTo(DomainManager.Character.GetReversedRelatedCharIdCount(b.Item1.GetId(), 16384));
						bool flag3 = simpAndLoverCountCompare != 0;
						if (flag3)
						{
							result = simpAndLoverCountCompare;
						}
						else
						{
							result = a.Item1.GetId().CompareTo(b.Item1.GetId());
						}
					}
				}
				return result;
			}

			// Token: 0x06008E3F RID: 36415 RVA: 0x004FBCC0 File Offset: 0x004F9EC0
			private static int CompareFriends([TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> a, [TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})] ValueTuple<Character, RelatedCharacter> b)
			{
				int gradeCompare = a.Item1.GetOrganizationInfo().Grade.CompareTo(b.Item1.GetOrganizationInfo().Grade);
				bool flag = gradeCompare != 0;
				int result;
				if (flag)
				{
					result = gradeCompare;
				}
				else
				{
					result = a.Item1.GetId().CompareTo(b.Item1.GetId());
				}
				return result;
			}

			// Token: 0x040034C4 RID: 13508
			public const sbyte Invalid = -1;

			// Token: 0x040034C5 RID: 13509
			public const sbyte HusbandAndWife = 0;

			// Token: 0x040034C6 RID: 13510
			public const sbyte MentorAndMentee = 1;

			// Token: 0x040034C7 RID: 13511
			public const sbyte ParentAndChild = 2;

			// Token: 0x040034C8 RID: 13512
			public const sbyte Siblings = 3;

			// Token: 0x040034C9 RID: 13513
			public const sbyte Lovers = 4;

			// Token: 0x040034CA RID: 13514
			public const sbyte SwornBrotherAndSister = 5;

			// Token: 0x040034CB RID: 13515
			public const sbyte Friends = 6;

			// Token: 0x040034CC RID: 13516
			public const sbyte Count = 7;

			// Token: 0x040034CD RID: 13517
			public static readonly sbyte[][] Priorities = new sbyte[][]
			{
				new sbyte[]
				{
					3,
					2,
					1,
					0,
					6,
					5,
					4
				},
				new sbyte[]
				{
					0,
					2,
					3,
					6,
					1,
					4,
					5
				},
				new sbyte[]
				{
					2,
					0,
					1,
					5,
					4,
					6,
					3
				},
				new sbyte[]
				{
					4,
					5,
					6,
					1,
					0,
					3,
					2
				},
				new sbyte[]
				{
					1,
					0,
					2,
					4,
					3,
					5,
					6
				}
			};

			// Token: 0x040034CE RID: 13518
			public static readonly sbyte[] ToPersonalityType = new sbyte[]
			{
				5,
				1,
				0,
				4,
				2,
				3,
				6
			};

			// Token: 0x040034CF RID: 13519
			[TupleElementNames(new string[]
			{
				"obj",
				"relation"
			})]
			public static readonly Comparison<ValueTuple<Character, RelatedCharacter>>[] Comparisons = new Comparison<ValueTuple<Character, RelatedCharacter>>[]
			{
				new Comparison<ValueTuple<Character, RelatedCharacter>>(AiHelper.JoinGroupRelationType.CompareHusbandAndWife),
				new Comparison<ValueTuple<Character, RelatedCharacter>>(AiHelper.JoinGroupRelationType.CompareMentors),
				new Comparison<ValueTuple<Character, RelatedCharacter>>(AiHelper.JoinGroupRelationType.CompareParents),
				new Comparison<ValueTuple<Character, RelatedCharacter>>(AiHelper.JoinGroupRelationType.CompareSiblings),
				new Comparison<ValueTuple<Character, RelatedCharacter>>(AiHelper.JoinGroupRelationType.CompareLovers),
				new Comparison<ValueTuple<Character, RelatedCharacter>>(AiHelper.JoinGroupRelationType.CompareSiblings),
				new Comparison<ValueTuple<Character, RelatedCharacter>>(AiHelper.JoinGroupRelationType.CompareFriends)
			};
		}

		// Token: 0x02000C27 RID: 3111
		public enum NpcCombatResultType : sbyte
		{
			// Token: 0x040034D1 RID: 13521
			MajorVictory,
			// Token: 0x040034D2 RID: 13522
			MinorVictory,
			// Token: 0x040034D3 RID: 13523
			MajorDefeat,
			// Token: 0x040034D4 RID: 13524
			MinorDefeat
		}

		// Token: 0x02000C28 RID: 3112
		public enum CombatResultHandleType : sbyte
		{
			// Token: 0x040034D6 RID: 13526
			Kill,
			// Token: 0x040034D7 RID: 13527
			Kidnap,
			// Token: 0x040034D8 RID: 13528
			Release
		}

		// Token: 0x02000C29 RID: 3113
		public static class NpcCombat
		{
			// Token: 0x06008E41 RID: 36417 RVA: 0x004FBE30 File Offset: 0x004FA030
			// Note: this type is marked as 'beforefieldinit'.
			static NpcCombat()
			{
				AiHelper.CombatResultHandleType[][] array = new AiHelper.CombatResultHandleType[5][];
				array[0] = new AiHelper.CombatResultHandleType[]
				{
					AiHelper.CombatResultHandleType.Kill,
					AiHelper.CombatResultHandleType.Kidnap,
					AiHelper.CombatResultHandleType.Release
				};
				int num = 1;
				AiHelper.CombatResultHandleType[] array2 = new AiHelper.CombatResultHandleType[3];
				array2[0] = AiHelper.CombatResultHandleType.Release;
				array2[1] = AiHelper.CombatResultHandleType.Kidnap;
				array[num] = array2;
				int num2 = 2;
				AiHelper.CombatResultHandleType[] array3 = new AiHelper.CombatResultHandleType[3];
				array3[0] = AiHelper.CombatResultHandleType.Kidnap;
				array3[1] = AiHelper.CombatResultHandleType.Release;
				array[num2] = array3;
				array[3] = new AiHelper.CombatResultHandleType[]
				{
					AiHelper.CombatResultHandleType.Kidnap,
					AiHelper.CombatResultHandleType.Kill,
					AiHelper.CombatResultHandleType.Release
				};
				array[4] = new AiHelper.CombatResultHandleType[]
				{
					AiHelper.CombatResultHandleType.Kill,
					AiHelper.CombatResultHandleType.Kidnap,
					AiHelper.CombatResultHandleType.Release
				};
				AiHelper.NpcCombat.ResultHandleTypePriorities = array;
				AiHelper.NpcCombat.CombatTypePowerScale = new sbyte[]
				{
					40,
					70,
					100
				};
				AiHelper.NpcCombat.CombatTypeSkillAttackInjuryCount = new sbyte[]
				{
					2,
					4,
					6
				};
				AiHelper.NpcCombat.CombatTypeWeaponAttackInjuryCount = new sbyte[]
				{
					1,
					2,
					3
				};
				AiHelper.NpcCombat.CombatTypePoisonScale = new sbyte[]
				{
					25,
					50,
					100
				};
			}

			// Token: 0x040034D9 RID: 13529
			public static readonly sbyte[] HandleEnemyInPublicChance = new sbyte[]
			{
				100,
				75,
				50,
				0,
				25
			};

			// Token: 0x040034DA RID: 13530
			public static readonly AiHelper.CombatResultHandleType[][] ResultHandleTypePriorities;

			// Token: 0x040034DB RID: 13531
			public static readonly sbyte[] CombatTypePowerScale;

			// Token: 0x040034DC RID: 13532
			public static readonly sbyte[] CombatTypeSkillAttackInjuryCount;

			// Token: 0x040034DD RID: 13533
			public static readonly sbyte[] CombatTypeWeaponAttackInjuryCount;

			// Token: 0x040034DE RID: 13534
			public static readonly sbyte[] CombatTypePoisonScale;
		}

		// Token: 0x02000C2A RID: 3114
		public static class LegendaryBookContestActionType
		{
			// Token: 0x06008E42 RID: 36418 RVA: 0x004FBF0C File Offset: 0x004FA10C
			// Note: this type is marked as 'beforefieldinit'.
			static LegendaryBookContestActionType()
			{
				sbyte[][] array = new sbyte[5][];
				array[0] = new sbyte[]
				{
					0,
					1
				};
				int num = 1;
				sbyte[] array2 = new sbyte[2];
				array2[0] = 1;
				array[num] = array2;
				array[2] = new sbyte[]
				{
					0,
					1,
					2,
					3
				};
				array[3] = new sbyte[]
				{
					2,
					3
				};
				array[4] = new sbyte[]
				{
					3,
					2
				};
				AiHelper.LegendaryBookContestActionType.IndirectActionPriorities = array;
				AiHelper.LegendaryBookContestActionType.DirectActionNpcTargetPriorities = new sbyte[][]
				{
					new sbyte[]
					{
						4
					},
					new sbyte[]
					{
						9
					},
					new sbyte[]
					{
						4,
						9
					},
					new sbyte[]
					{
						9
					},
					new sbyte[]
					{
						9
					}
				};
				AiHelper.LegendaryBookContestActionType.DirectActionTaiwuTargetPriorities = new sbyte[][]
				{
					new sbyte[]
					{
						4,
						6,
						5
					},
					new sbyte[]
					{
						5,
						6,
						4
					},
					new sbyte[]
					{
						6,
						7,
						5,
						8,
						4,
						9
					},
					new sbyte[]
					{
						8,
						7,
						9
					},
					new sbyte[]
					{
						9,
						7,
						8
					}
				};
			}

			// Token: 0x040034DF RID: 13535
			public const sbyte CombatPlay = 0;

			// Token: 0x040034E0 RID: 13536
			public const sbyte Gift = 1;

			// Token: 0x040034E1 RID: 13537
			public const sbyte Poison = 2;

			// Token: 0x040034E2 RID: 13538
			public const sbyte PlotHarm = 3;

			// Token: 0x040034E3 RID: 13539
			public const sbyte Challenge = 4;

			// Token: 0x040034E4 RID: 13540
			public const sbyte Request = 5;

			// Token: 0x040034E5 RID: 13541
			public const sbyte Trade = 6;

			// Token: 0x040034E6 RID: 13542
			public const sbyte Steal = 7;

			// Token: 0x040034E7 RID: 13543
			public const sbyte Scam = 8;

			// Token: 0x040034E8 RID: 13544
			public const sbyte Rob = 9;

			// Token: 0x040034E9 RID: 13545
			public static readonly sbyte[] ToPersonalityType = new sbyte[]
			{
				4,
				2,
				0,
				1,
				3,
				2,
				4,
				0,
				1,
				3
			};

			// Token: 0x040034EA RID: 13546
			public static readonly sbyte[][] IndirectActionPriorities;

			// Token: 0x040034EB RID: 13547
			public static readonly sbyte[][] DirectActionNpcTargetPriorities;

			// Token: 0x040034EC RID: 13548
			public static readonly sbyte[][] DirectActionTaiwuTargetPriorities;
		}

		// Token: 0x02000C2B RID: 3115
		public static class LegendaryBookRelatedConstants
		{
			// Token: 0x040034ED RID: 13549
			public const byte TargetConsummateLevel = 18;

			// Token: 0x040034EE RID: 13550
			public const byte ConsummateLevelGrowthPerMonth = 2;

			// Token: 0x040034EF RID: 13551
			public const byte ShockedCharacterActCrazyRate = 50;

			// Token: 0x040034F0 RID: 13552
			public const sbyte StrongerCharacterIndirectContestChance = 25;

			// Token: 0x040034F1 RID: 13553
			public const sbyte ContestActionBaseChance = 60;

			// Token: 0x040034F2 RID: 13554
			public const sbyte ContestForLegendaryBookCooldown = 12;

			// Token: 0x040034F3 RID: 13555
			public const sbyte ContestForLegendaryBookDuration = 36;

			// Token: 0x040034F4 RID: 13556
			public const sbyte ContestForLegendaryBookMaxCharCount = 2;

			// Token: 0x040034F5 RID: 13557
			public const sbyte AffectedByLegendaryBookOwnerFeatureRate = 50;

			// Token: 0x040034F6 RID: 13558
			public const sbyte ShockedOwnerUpgradeEnemyNestCount = 4;

			// Token: 0x040034F7 RID: 13559
			public const sbyte InsaneOwnerUpgradeEnemyNestCount = 7;

			// Token: 0x040034F8 RID: 13560
			public const sbyte AttackTaiwuForLegendaryBookCoolDown = 12;

			// Token: 0x040034F9 RID: 13561
			public static readonly sbyte[] ContestForLegendaryBookChanceAdjust = new sbyte[]
			{
				-20,
				-10,
				0,
				10,
				20
			};

			// Token: 0x040034FA RID: 13562
			public static readonly sbyte[] IdleDuringContestActionChance = new sbyte[]
			{
				30,
				40,
				20,
				0,
				10
			};

			// Token: 0x040034FB RID: 13563
			public static readonly short[] LegendaryBookAdventures = new short[]
			{
				149,
				146,
				147,
				156,
				157,
				145,
				148,
				154,
				152,
				158,
				155,
				150,
				153,
				151
			};
		}

		// Token: 0x02000C2C RID: 3116
		public static class MixedPoisonHarmfulActionType
		{
			// Token: 0x040034FC RID: 13564
			public const sbyte Attack = 0;

			// Token: 0x040034FD RID: 13565
			public const sbyte Poison = 1;

			// Token: 0x040034FE RID: 13566
			public const sbyte PlotHarm = 2;

			// Token: 0x040034FF RID: 13567
			public const sbyte Rape = 3;

			// Token: 0x04003500 RID: 13568
			public const int Count = 4;
		}

		// Token: 0x02000C2D RID: 3117
		public static class PrioritizedActionConstants
		{
			// Token: 0x06008E44 RID: 36420 RVA: 0x004FC0A0 File Offset: 0x004FA2A0
			// Note: this type is marked as 'beforefieldinit'.
			static PrioritizedActionConstants()
			{
				sbyte[][] array = new sbyte[5][];
				array[0] = new sbyte[]
				{
					0,
					2,
					1
				};
				array[1] = new sbyte[]
				{
					0,
					2,
					1
				};
				int num = 2;
				sbyte[] array2 = new sbyte[3];
				array2[0] = 2;
				array2[1] = 1;
				array[num] = array2;
				array[3] = new sbyte[]
				{
					1,
					0,
					2
				};
				array[4] = new sbyte[]
				{
					1,
					0,
					2
				};
				AiHelper.PrioritizedActionConstants.TakeRevengeActionPriorities = array;
			}

			// Token: 0x04003501 RID: 13569
			public static readonly sbyte[] PrioritizedActionBaseDurations = new sbyte[]
			{
				12,
				0,
				4,
				4,
				3,
				3,
				6,
				6,
				6
			};

			// Token: 0x04003502 RID: 13570
			public static readonly sbyte[] PrioritizedActionCooldowns = new sbyte[]
			{
				6,
				0,
				3,
				3,
				12,
				6,
				3,
				0,
				6
			};

			// Token: 0x04003503 RID: 13571
			public static readonly sbyte[] PrioritizedActionMinFavorType = new sbyte[]
			{
				sbyte.MinValue,
				2,
				2,
				2,
				sbyte.MinValue,
				2,
				sbyte.MinValue,
				sbyte.MinValue,
				sbyte.MinValue
			};

			// Token: 0x04003504 RID: 13572
			public const sbyte CooldownBonusOnAdventureEnd = 3;

			// Token: 0x04003505 RID: 13573
			public const sbyte CannotStrollCharCheckActionChance = 25;

			// Token: 0x04003506 RID: 13574
			public const sbyte JoinSectBaseChancePerEnemy = 10;

			// Token: 0x04003507 RID: 13575
			public const sbyte AskForProtectionThreshold = 75;

			// Token: 0x04003508 RID: 13576
			public static readonly sbyte[] CivilianGradeJoinSectChance = new sbyte[]
			{
				0,
				0,
				0,
				-25,
				-50,
				-75,
				-100,
				-100,
				-100
			};

			// Token: 0x04003509 RID: 13577
			public static readonly sbyte[] FindTreasureBaseChance = new sbyte[]
			{
				15,
				20,
				25,
				35,
				30
			};

			// Token: 0x0400350A RID: 13578
			public static readonly sbyte[] TakeRevengeChance = new sbyte[]
			{
				50,
				30,
				40,
				60,
				70
			};

			// Token: 0x0400350B RID: 13579
			public static readonly sbyte[] TakeRevengeMaxFavorType = new sbyte[]
			{
				-3,
				-5,
				-4,
				-1,
				-2
			};

			// Token: 0x0400350C RID: 13580
			public static readonly sbyte[][] PrioritizedActionPriorities = new sbyte[][]
			{
				new sbyte[]
				{
					0,
					1,
					3,
					2,
					8,
					4,
					5,
					7,
					6
				},
				new sbyte[]
				{
					0,
					3,
					2,
					1,
					4,
					5,
					7,
					6,
					8
				},
				new sbyte[]
				{
					0,
					1,
					7,
					6,
					3,
					2,
					5,
					4,
					8
				},
				new sbyte[]
				{
					0,
					8,
					3,
					2,
					7,
					6,
					1,
					5,
					4
				},
				new sbyte[]
				{
					0,
					7,
					6,
					1,
					8,
					4,
					3,
					2,
					5
				}
			};

			// Token: 0x0400350D RID: 13581
			public static readonly sbyte[][] RescueFriendOrFamilyActionPriorities = new sbyte[][]
			{
				new sbyte[]
				{
					3,
					1,
					2
				},
				new sbyte[]
				{
					3,
					1,
					2
				},
				new sbyte[]
				{
					1,
					2,
					3
				},
				new sbyte[]
				{
					2,
					3,
					1
				},
				new sbyte[]
				{
					2,
					3,
					1
				}
			};

			// Token: 0x0400350E RID: 13582
			public static readonly sbyte[][] TakeRevengeActionPriorities;

			// Token: 0x0400350F RID: 13583
			public const sbyte TakeRevengeActionBaseChance = 60;

			// Token: 0x04003510 RID: 13584
			public const sbyte PlotHarmMinDamage = 6;

			// Token: 0x04003511 RID: 13585
			public const sbyte PlotHarmMaxDamage = 12;

			// Token: 0x04003512 RID: 13586
			public const sbyte MournOthersBaseChance = 40;

			// Token: 0x04003513 RID: 13587
			public const sbyte MournOthersWithNeedChance = 80;

			// Token: 0x04003514 RID: 13588
			public const sbyte DejaVuMaxDistanceToTaiwu = 5;
		}

		// Token: 0x02000C2E RID: 3118
		public static class RelationsRelatedConstants
		{
			// Token: 0x04003515 RID: 13589
			public static readonly sbyte[] IncestSexRelationChanceFactor = new sbyte[]
			{
				5,
				10,
				15,
				25,
				20
			};

			// Token: 0x04003516 RID: 13590
			public static readonly sbyte[] StartAdoreChanceChangePerAdored = new sbyte[]
			{
				-50,
				-40,
				-30,
				-10,
				-20
			};

			// Token: 0x04003517 RID: 13591
			public static readonly sbyte[] BecomeEnemyHappinessChange = new sbyte[]
			{
				0,
				-5,
				-10,
				5,
				0
			};

			// Token: 0x04003518 RID: 13592
			public static readonly sbyte[] SeverEnemyHappinessChange = new sbyte[]
			{
				0,
				5,
				10,
				-5,
				0
			};

			// Token: 0x04003519 RID: 13593
			public static readonly sbyte[] SeverEnemyNotMutuallyChance = new sbyte[]
			{
				30,
				10,
				20,
				50,
				40
			};

			// Token: 0x0400351A RID: 13594
			public static readonly sbyte[] ConfessLoveSucceedHappinessChange = new sbyte[]
			{
				5,
				10,
				5,
				10,
				5
			};

			// Token: 0x0400351B RID: 13595
			public static readonly sbyte[] ConfessLoveSucceedNeedForSexChance = new sbyte[]
			{
				0,
				10,
				20,
				40,
				30
			};

			// Token: 0x0400351C RID: 13596
			public static readonly sbyte[] ConfessLoveFailedHappinessChange = new sbyte[]
			{
				-5,
				-10,
				-5,
				-10,
				-5
			};

			// Token: 0x0400351D RID: 13597
			public static readonly sbyte[] BecomeSingleNeedNewLoveChance = new sbyte[]
			{
				0,
				10,
				20,
				40,
				30
			};

			// Token: 0x0400351E RID: 13598
			public static readonly sbyte[] ConfessLoveFailedNeedForRapeChance = new sbyte[]
			{
				0,
				0,
				5,
				15,
				10
			};

			// Token: 0x0400351F RID: 13599
			public static readonly sbyte[] ConfessLoveOrProposeFailedBecomeEnemyChance = new sbyte[]
			{
				5,
				0,
				10,
				20,
				15
			};

			// Token: 0x04003520 RID: 13600
			public static readonly sbyte[] BreakupBecomeEnemyChance = new sbyte[]
			{
				10,
				0,
				20,
				40,
				30
			};

			// Token: 0x04003521 RID: 13601
			public static readonly sbyte[] BreakupMutuallyChance = new sbyte[]
			{
				30,
				10,
				20,
				50,
				40
			};

			// Token: 0x04003522 RID: 13602
			public static readonly sbyte[] BreakupHappinessChange = new sbyte[]
			{
				0,
				-5,
				-10,
				-5,
				0
			};

			// Token: 0x04003523 RID: 13603
			public static readonly sbyte[] ProposeMarriageSucceedHappinessChange = new sbyte[]
			{
				15,
				15,
				15,
				15,
				15
			};

			// Token: 0x04003524 RID: 13604
			public static readonly sbyte[] ProposeMarriageFailHappinessChange = new sbyte[]
			{
				-5,
				-10,
				-5,
				-10,
				-5
			};

			// Token: 0x04003525 RID: 13605
			public static readonly sbyte[] BecomeFriendHappinessChange = new sbyte[]
			{
				10,
				10,
				5,
				0,
				0
			};

			// Token: 0x04003526 RID: 13606
			public static readonly sbyte[] SeverFriendHappinessChange = new sbyte[]
			{
				-10,
				-10,
				-5,
				0,
				0
			};

			// Token: 0x04003527 RID: 13607
			public static readonly sbyte[] BecomeSwornOrAdoptedFamilyHappinessChange = new sbyte[]
			{
				15,
				15,
				10,
				5,
				5
			};

			// Token: 0x04003528 RID: 13608
			public static readonly sbyte[] SeverSwornOrAdoptedFamilyHappinessChange = new sbyte[]
			{
				-15,
				-15,
				-10,
				-5,
				-5
			};

			// Token: 0x04003529 RID: 13609
			public static readonly sbyte[] SeverHusbandOrWifeHappinessChange = new sbyte[]
			{
				-10,
				-15,
				-10,
				-15,
				-10
			};

			// Token: 0x0400352A RID: 13610
			public static readonly sbyte[] BecomeMentorHappinessChange = new sbyte[]
			{
				0,
				5,
				10,
				-5,
				0
			};

			// Token: 0x0400352B RID: 13611
			public const short ConfessLoveFavorabilityChange = 3000;

			// Token: 0x0400352C RID: 13612
			public const short BreakupFavorabilityChange = -12000;

			// Token: 0x0400352D RID: 13613
			public const short ProposeMarriageSucceedFavorabilityChange = 12000;

			// Token: 0x0400352E RID: 13614
			public const short ProposeMarriageFailFavorabilityChange = -3000;

			// Token: 0x0400352F RID: 13615
			public const short BecomeSwornOrAdoptedFamilyFavorabilityChange = 12000;

			// Token: 0x04003530 RID: 13616
			public const short FriendshipFavorabilityChange = 3000;

			// Token: 0x04003531 RID: 13617
			public const short SeverSwornOrAdoptedFavorabilityChange = -12000;

			// Token: 0x04003532 RID: 13618
			public const short SeverHusbandOrWifeFavorabilityChange = -12000;

			// Token: 0x04003533 RID: 13619
			public const short BecomeMentorFavorabilityChange = 3000;
		}

		// Token: 0x02000C2F RID: 3119
		public static class Relation
		{
			// Token: 0x06008E46 RID: 36422 RVA: 0x004FC484 File Offset: 0x004FA684
			public unsafe static bool CheckChangeRelationTypeChance(IRandomSource random, ref Personalities personalities, sbyte personalityType, int multiplier = 1)
			{
				return random.CheckPercentProb(30 + (int)(*(ref personalities.Items.FixedElementField + personalityType) * 3) * multiplier);
			}

			// Token: 0x06008E47 RID: 36423 RVA: 0x004FC4B4 File Offset: 0x004FA6B4
			public static AiRelationsItem GetAiRelationConfig(short aiRelationsTemplateId)
			{
				return AiRelations.Instance[aiRelationsTemplateId];
			}

			// Token: 0x06008E48 RID: 36424 RVA: 0x004FC4D4 File Offset: 0x004FA6D4
			public static bool CanStartRelation(Character selfChar, Character targetChar, ushort relationType)
			{
				int charId = selfChar.GetId();
				int relatedCharId = targetChar.GetId();
				bool flag = !RelationTypeHelper.AllowAddingRelation(charId, relatedCharId, relationType);
				if (!flag)
				{
					RelatedCharacter selfToTarget = DomainManager.Character.GetRelation(charId, relatedCharId);
					RelatedCharacter targetToSelf = DomainManager.Character.GetRelation(relatedCharId, charId);
					if (relationType <= 512)
					{
						if (relationType == 64)
						{
							return AiHelper.Relation.CanStartRelation_AdoptiveParent(charId, selfToTarget, selfChar.GetCurrAge(), relatedCharId, targetToSelf, targetChar.GetCurrAge());
						}
						if (relationType == 128)
						{
							return AiHelper.Relation.CanStartRelation_AdoptiveChild(charId, selfToTarget, selfChar.GetCurrAge(), relatedCharId, targetToSelf, targetChar.GetCurrAge());
						}
						if (relationType == 512)
						{
							return AiHelper.Relation.CanStartRelation_SwornBrotherOrSister(selfToTarget, selfChar.GetBehaviorType(), targetToSelf, targetChar.GetBehaviorType());
						}
					}
					else if (relationType <= 8192)
					{
						if (relationType == 1024)
						{
							return AiHelper.Relation.CanStartRelation_HusbandOrWife(charId, selfToTarget, selfChar.GetBehaviorType(), relatedCharId, targetToSelf, targetChar.GetBehaviorType());
						}
						if (relationType == 8192)
						{
							return AiHelper.Relation.CanStartRelation_Friend(selfToTarget, selfChar.GetBehaviorType(), targetToSelf, targetChar.GetBehaviorType());
						}
					}
					else if (relationType != 16384)
					{
						if (relationType == 32768)
						{
							return AiHelper.Relation.CanStartRelation_Enemy(selfToTarget, selfChar.GetBehaviorType());
						}
					}
					else
					{
						bool flag2 = RelationType.HasRelation(selfToTarget.RelationType, 16384);
						if (flag2)
						{
							return AiHelper.Relation.CanStartRelation_BoyOrGirlFriend(selfToTarget, selfChar.GetBehaviorType(), targetToSelf, targetChar.GetBehaviorType());
						}
						return AiHelper.Relation.CanStartRelation_Adored(selfToTarget, selfChar.GetBehaviorType());
					}
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported relation type for starting in event: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(relationType);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				return false;
			}

			// Token: 0x06008E49 RID: 36425 RVA: 0x004FC6A0 File Offset: 0x004FA8A0
			public static bool CanStartRelation_Enemy(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
			{
				bool flag = (selfToTarget.RelationType & 32768) > 0;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
					AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(0);
					short maxFavorabilityReq = relationsCfg.MaxFavorability[(int)selfBehaviorType];
					result = ((short)favorabilityTypeToTarget <= maxFavorabilityReq);
				}
				return result;
			}

			// Token: 0x06008E4A RID: 36426 RVA: 0x004FC6F0 File Offset: 0x004FA8F0
			public static bool CanEndRelation_Enemy(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
			{
				bool flag = (selfToTarget.RelationType & 32768) == 0;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
					AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(1);
					short minSelfFavorabilityReq = relationsCfg.MinFavorability[(int)selfBehaviorType];
					result = ((short)favorabilityTypeToTarget >= minSelfFavorabilityReq);
				}
				return result;
			}

			// Token: 0x06008E4B RID: 36427 RVA: 0x004FC740 File Offset: 0x004FA940
			public static bool CanStartRelation_Friend(RelatedCharacter selfToTarget, sbyte selfBehaviorType, RelatedCharacter targetToSelf, sbyte targetBehaviorType)
			{
				bool flag = (selfToTarget.RelationType & 8192) > 0;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
					sbyte targetFavorabilityTypeToSelf = FavorabilityType.GetFavorabilityType(targetToSelf.Favorability);
					AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(6);
					short minSelfFavorabilityReq = relationsCfg.MinFavorability[(int)selfBehaviorType];
					short minTargetFavorabilityReq = relationsCfg.MinFavorability[(int)targetBehaviorType];
					result = ((short)favorabilityTypeToTarget >= minSelfFavorabilityReq && (short)targetFavorabilityTypeToSelf >= minTargetFavorabilityReq);
				}
				return result;
			}

			// Token: 0x06008E4C RID: 36428 RVA: 0x004FC7B0 File Offset: 0x004FA9B0
			public static bool CanEndRelation_Friend(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
			{
				bool flag = (selfToTarget.RelationType & 8192) == 0;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
					AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(7);
					short maxFavorabilityReq = relationsCfg.MaxFavorability[(int)selfBehaviorType];
					result = ((short)favorabilityTypeToTarget <= maxFavorabilityReq);
				}
				return result;
			}

			// Token: 0x06008E4D RID: 36429 RVA: 0x004FC800 File Offset: 0x004FAA00
			public static bool CanStartRelation_SwornBrotherOrSister(RelatedCharacter selfToTarget, sbyte selfBehaviorType, RelatedCharacter targetToSelf, sbyte targetBehaviorType)
			{
				bool flag = RelationType.ContainBloodExclusionRelations(selfToTarget.RelationType);
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					bool flag2 = !RelationType.HasRelation(selfToTarget.RelationType, 8192);
					if (flag2)
					{
						result = false;
					}
					else
					{
						sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
						sbyte targetFavorabilityTypeToSelf = FavorabilityType.GetFavorabilityType(targetToSelf.Favorability);
						AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(8);
						short minSelfFavorabilityReq = relationsCfg.MinFavorability[(int)selfBehaviorType];
						short minTargetFavorabilityReq = relationsCfg.MinFavorability[(int)targetBehaviorType];
						result = ((short)favorabilityTypeToTarget >= minSelfFavorabilityReq && (short)targetFavorabilityTypeToSelf >= minTargetFavorabilityReq);
					}
				}
				return result;
			}

			// Token: 0x06008E4E RID: 36430 RVA: 0x004FC88C File Offset: 0x004FAA8C
			public static bool CanEndRelation_SwornBrotherOrSister(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
			{
				bool flag = (selfToTarget.RelationType & 512) == 0;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
					AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(9);
					short maxFavorabilityReq = relationsCfg.MaxFavorability[(int)selfBehaviorType];
					result = ((short)favorabilityTypeToTarget <= maxFavorabilityReq);
				}
				return result;
			}

			// Token: 0x06008E4F RID: 36431 RVA: 0x004FC8DC File Offset: 0x004FAADC
			public static bool CanStartRelation_Adored(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
			{
				bool flag = (selfToTarget.RelationType & 16384) > 0;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
					AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(2);
					short minFavorabilityReq = relationsCfg.MinFavorability[(int)selfBehaviorType];
					result = ((short)favorabilityTypeToTarget >= minFavorabilityReq);
				}
				return result;
			}

			// Token: 0x06008E50 RID: 36432 RVA: 0x004FC92C File Offset: 0x004FAB2C
			public static bool CanStartRelation_BoyOrGirlFriend(RelatedCharacter selfToTarget, sbyte selfBehaviorType, RelatedCharacter targetToSelf, sbyte targetBehaviorType)
			{
				bool flag = (selfToTarget.RelationType & 16384) == 0 || (targetToSelf.RelationType & 16384) > 0;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
					AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(3);
					short minSelfFavorabilityReq = relationsCfg.MinFavorability[(int)selfBehaviorType];
					result = ((short)favorabilityTypeToTarget >= minSelfFavorabilityReq);
				}
				return result;
			}

			// Token: 0x06008E51 RID: 36433 RVA: 0x004FC98C File Offset: 0x004FAB8C
			public static bool CanEndRelation_BoyOrGirlFriend(RelatedCharacter selfToTarget, sbyte selfBehaviorType, RelatedCharacter targetToSelf, sbyte targetBehaviorType)
			{
				bool flag = (selfToTarget.RelationType & 16384) == 0 || (targetToSelf.RelationType & 16384) == 0;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
					AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(4);
					short maxFavorabilityReq = relationsCfg.MaxFavorability[(int)selfBehaviorType];
					result = ((short)favorabilityTypeToTarget <= maxFavorabilityReq);
				}
				return result;
			}

			// Token: 0x06008E52 RID: 36434 RVA: 0x004FC9EC File Offset: 0x004FABEC
			public static bool CanStartRelation_HusbandOrWife(int selfCharId, RelatedCharacter selfToTarget, sbyte selfBehaviorType, int targetCharId, RelatedCharacter targetToSelf, sbyte targetBehaviorType)
			{
				bool flag = (selfToTarget.RelationType & 16384) == 0 || (targetToSelf.RelationType & 16384) == 0 || RelationType.ContainBloodExclusionRelations(selfToTarget.RelationType) || DomainManager.Character.GetAliveSpouse(selfCharId) >= 0 || DomainManager.Character.GetAliveSpouse(targetCharId) >= 0;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
					AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(5);
					short minSelfFavorabilityReq = relationsCfg.MinFavorability[(int)selfBehaviorType];
					result = ((short)favorabilityTypeToTarget >= minSelfFavorabilityReq);
				}
				return result;
			}

			// Token: 0x06008E53 RID: 36435 RVA: 0x004FCA78 File Offset: 0x004FAC78
			public static bool CanEndRelation_HusbandOrWife(RelatedCharacter selfToTarget, sbyte selfBehaviorType)
			{
				bool flag = (selfToTarget.RelationType & 1024) == 0;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					sbyte favorabilityTypeToTarget = FavorabilityType.GetFavorabilityType(selfToTarget.Favorability);
					AiRelationsItem relationsCfg = AiHelper.Relation.GetAiRelationConfig(12);
					short maxFavorabilityReq = relationsCfg.MaxFavorability[(int)selfBehaviorType];
					result = ((short)favorabilityTypeToTarget <= maxFavorabilityReq);
				}
				return result;
			}

			// Token: 0x06008E54 RID: 36436 RVA: 0x004FCAC8 File Offset: 0x004FACC8
			public static bool CanStartRelation_AdoptiveParent(int selfCharId, RelatedCharacter selfToTarget, short selfAge, int targetCharId, RelatedCharacter targetToSelf, short targetAge)
			{
				return !RelationType.ContainBloodExclusionRelations(selfToTarget.RelationType) && FavorabilityType.GetFavorabilityType(selfToTarget.Favorability) >= 4 && FavorabilityType.GetFavorabilityType(targetToSelf.Favorability) >= 4 && selfAge < 30 && targetAge >= 30 && DomainManager.Character.GetAliveParent(selfCharId) < 0 && DomainManager.Character.GetAliveChild(targetCharId) < 0;
			}

			// Token: 0x06008E55 RID: 36437 RVA: 0x004FCB30 File Offset: 0x004FAD30
			public static bool CanStartRelation_AdoptiveChild(int selfCharId, RelatedCharacter selfToTarget, short selfAge, int targetCharId, RelatedCharacter targetToSelf, short targetAge)
			{
				return !RelationType.ContainBloodExclusionRelations(selfToTarget.RelationType) && FavorabilityType.GetFavorabilityType(selfToTarget.Favorability) >= 4 && FavorabilityType.GetFavorabilityType(targetToSelf.Favorability) >= 4 && selfAge >= 30 && targetAge < 30 && DomainManager.Character.GetAliveChild(selfCharId) < 0 && DomainManager.Character.GetAliveParent(targetCharId) < 0;
			}

			// Token: 0x06008E56 RID: 36438 RVA: 0x004FCB98 File Offset: 0x004FAD98
			public static int GetStartOrEndRelationChance(AiRelationsItem aiRelationsCfg, Character selfChar, Character targetChar, ushort curRelationType, sbyte sectFavorability, int multiplier = 1)
			{
				sbyte selfBehaviorType = selfChar.GetBehaviorType();
				sbyte targetBehaviorType = targetChar.GetBehaviorType();
				sbyte selfFameType = selfChar.GetFameType();
				sbyte targetFameType = targetChar.GetFameType();
				int prob = 0;
				bool flag = aiRelationsCfg.NoncontradictoryBehaviorAjust != 0 && !BehaviorType.IsContradictory(selfBehaviorType, targetBehaviorType);
				if (flag)
				{
					prob += (int)aiRelationsCfg.NoncontradictoryBehaviorAjust;
				}
				bool flag2 = aiRelationsCfg.NoncontradictoryFameAjust != 0 && !FameType.IsContradictory(selfFameType, targetFameType);
				if (flag2)
				{
					prob += (int)aiRelationsCfg.NoncontradictoryFameAjust;
				}
				bool flag3 = aiRelationsCfg.EnemySectMemberAdjust != 0 && sectFavorability == -1;
				if (flag3)
				{
					prob += (int)aiRelationsCfg.EnemySectMemberAdjust;
				}
				else
				{
					bool flag4 = aiRelationsCfg.FriendlySectMemberAdjust != 0 && sectFavorability == 1;
					if (flag4)
					{
						prob += (int)aiRelationsCfg.FriendlySectMemberAdjust;
					}
				}
				int scale = 100;
				RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(selfChar.GetId());
				switch (aiRelationsCfg.TemplateId)
				{
				case 0:
				{
					bool flag5 = selfChar.GetFeatureIds().Contains(485) || targetChar.GetFeatureIds().Contains(485);
					if (flag5)
					{
						scale *= 10;
					}
					break;
				}
				case 2:
				{
					int adoredCount = relatedCharacters.Adored.GetCount();
					bool flag6 = adoredCount > 0;
					if (flag6)
					{
						sbyte behaviorFactor = AiHelper.RelationsRelatedConstants.StartAdoreChanceChangePerAdored[(int)selfBehaviorType];
						int featureFactor = AiHelper.Relation.CalcAdoreMultiplePeopleChanceFactor(selfChar);
						scale += adoredCount * (int)behaviorFactor * featureFactor / 100;
					}
					bool flag7 = selfChar.GetFeatureIds().Contains(484) || targetChar.GetFeatureIds().Contains(484);
					if (flag7)
					{
						scale *= 10;
					}
					break;
				}
				case 3:
				{
					List<PersonalNeed> personalNeeds = selfChar.GetPersonalNeeds();
					bool flag8 = personalNeeds.Exists((PersonalNeed need) => need.TemplateId == 25 && need.RelationType == 16384);
					if (flag8)
					{
						scale = scale * 150 / 100;
					}
					break;
				}
				case 4:
					scale += 20 * (relatedCharacters.Adored.GetCount() - 1);
					break;
				case 8:
				{
					scale -= 20 * relatedCharacters.SwornBrothersAndSisters.GetCount();
					List<PersonalNeed> personalNeeds2 = selfChar.GetPersonalNeeds();
					bool flag9 = personalNeeds2.Exists((PersonalNeed need) => need.TemplateId == 25 && need.RelationType == 512);
					if (flag9)
					{
						scale = scale * 150 / 100;
					}
					bool flag10 = RelationType.HasRelation(curRelationType, 16384);
					if (flag10)
					{
						prob -= 2000;
					}
					break;
				}
				}
				bool flag11 = RelationType.ContainDirectBloodRelations(curRelationType) || RelationType.HasRelation(curRelationType, 1024);
				if (flag11)
				{
					prob += (int)aiRelationsCfg.Probability[(int)selfBehaviorType].BloodRelationsProb;
				}
				else
				{
					bool flag12 = RelationType.ContainNonBloodFamilyRelations(curRelationType) || RelationType.HasRelation(curRelationType, 2048) || RelationType.HasRelation(curRelationType, 4096);
					if (flag12)
					{
						prob += (int)aiRelationsCfg.Probability[(int)selfBehaviorType].SwornOrAdoptiveRelationsProb;
					}
					else
					{
						prob += (int)aiRelationsCfg.Probability[(int)selfBehaviorType].DefaultProb;
					}
				}
				return Math.Max(0, prob * scale * multiplier / 100);
			}

			// Token: 0x06008E57 RID: 36439 RVA: 0x004FCEBC File Offset: 0x004FB0BC
			public static int GetStartRelationSuccessRate_Adored(Character selfChar, Character targetChar, RelatedCharacter selfToTarget, RelatedCharacter targetToSelf)
			{
				int successRate = AiHelper.Relation.GetStartRelationSuccessRate_SexRelationBaseRate(targetChar, selfChar, targetToSelf, selfToTarget, null);
				bool flag = successRate == int.MinValue;
				int result;
				if (flag)
				{
					result = 0;
				}
				else
				{
					bool flag2 = RelationType.ContainDirectBloodRelations(selfToTarget.RelationType);
					if (flag2)
					{
						sbyte behaviorType = selfChar.GetBehaviorType();
						successRate = successRate * (int)AiHelper.RelationsRelatedConstants.IncestSexRelationChanceFactor[(int)behaviorType] / 100;
					}
					result = successRate;
				}
				return result;
			}

			// Token: 0x06008E58 RID: 36440 RVA: 0x004FCF14 File Offset: 0x004FB114
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
						PhaseProbList = new List<int>
						{
							0,
							0
						}
					};
					DomainManager.TaiwuEvent.InteractCheckData = eventInteractCheckData;
					DomainManager.TaiwuEvent.ShowInteractCheckAnimation = true;
				}
				int successRate = AiHelper.Relation.GetStartRelationSuccessRate_SexRelationBaseRate(selfChar, targetChar, selfToTarget, targetToSelf, eventInteractCheckData);
				bool flag = successRate == int.MinValue;
				int result;
				if (flag)
				{
					result = 0;
				}
				else
				{
					successRate -= 20;
					int selfAliveSpouse = DomainManager.Character.GetAliveSpouse(selfChar.GetId());
					int targetAliveSpouse = DomainManager.Character.GetAliveSpouse(targetChar.GetId());
					int fixRate = 0;
					bool flag2 = selfAliveSpouse >= 0 && selfAliveSpouse != targetChar.GetId();
					if (flag2)
					{
						fixRate -= 60;
						successRate -= 60;
					}
					bool flag3 = targetAliveSpouse >= 0 && targetAliveSpouse != selfChar.GetId();
					if (flag3)
					{
						fixRate -= 60;
						successRate -= 60;
					}
					successRate += (int)DomainManager.Extra.GetAiActionSuccessRateAdjust(selfChar.GetId(), targetChar.GetId(), 10, RelationType.GetTypeId(16384));
					bool flag4 = RelationType.ContainDirectBloodRelations(targetToSelf.RelationType);
					if (flag4)
					{
						sbyte behaviorType = targetChar.GetBehaviorType();
						successRate = successRate * (int)AiHelper.RelationsRelatedConstants.IncestSexRelationChanceFactor[(int)behaviorType] / 100;
					}
					bool flag5 = eventInteractCheckData != null;
					if (flag5)
					{
						eventInteractCheckData.ConfessionLoveSecularFixProbDict.Add(3, fixRate);
						eventInteractCheckData.PhaseProbList[0] = successRate;
						eventInteractCheckData.PhaseProbList[1] = successRate;
					}
					result = successRate;
				}
				return result;
			}

			// Token: 0x06008E59 RID: 36441 RVA: 0x004FD0DC File Offset: 0x004FB2DC
			public static int GetStartRelationSuccessRate_HusbandOrWife(Character selfChar, Character targetChar, RelatedCharacter selfToTarget, RelatedCharacter targetToSelf)
			{
				int successRate = AiHelper.Relation.GetStartRelationSuccessRate_SexRelationBaseRate(selfChar, targetChar, selfToTarget, targetToSelf, null);
				bool flag = successRate == int.MinValue;
				int result;
				if (flag)
				{
					result = 0;
				}
				else
				{
					successRate -= 40;
					result = successRate;
				}
				return result;
			}

			// Token: 0x06008E5A RID: 36442 RVA: 0x004FD110 File Offset: 0x004FB310
			public static int GetStartRelationSuccessRate_SexRelationBaseRate(Character selfChar, Character targetChar, RelatedCharacter selfToTarget, RelatedCharacter targetToSelf, EventInteractCheckData eventInteractCheckData = null)
			{
				int successRate = 100;
				sbyte selfGender = selfChar.GetGender();
				sbyte selfDisplayingGender = selfChar.GetDisplayingGender();
				sbyte targetGender = targetChar.GetGender();
				sbyte targetDisplayingGender = targetChar.GetDisplayingGender();
				if (eventInteractCheckData != null)
				{
					eventInteractCheckData.ConfessionLovePureFixProbDict.Add(0, 0);
				}
				if (eventInteractCheckData != null)
				{
					eventInteractCheckData.ConfessionLovePureFixProbDict.Add(2, 0);
				}
				bool isTargetBisexual = targetChar.GetBisexual();
				bool isGenderSame = selfGender == targetGender;
				bool isDisplayingGenderSame = selfDisplayingGender == targetDisplayingGender;
				bool flag = (isTargetBisexual && !isGenderSame && !isDisplayingGenderSame) || (!isTargetBisexual && isGenderSame && isDisplayingGenderSame);
				int result;
				if (flag)
				{
					bool flag2 = eventInteractCheckData != null;
					if (flag2)
					{
						eventInteractCheckData.ConfessionLovePureFixProbDict[0] = int.MinValue;
					}
					result = int.MinValue;
				}
				else
				{
					bool flag3 = !isTargetBisexual && isGenderSame && !isDisplayingGenderSame;
					if (flag3)
					{
						bool flag4 = eventInteractCheckData != null;
						if (flag4)
						{
							eventInteractCheckData.ConfessionLovePureFixProbDict[0] = -20;
						}
						successRate -= 20;
					}
					bool flag5 = isTargetBisexual && !isGenderSame && isDisplayingGenderSame;
					if (flag5)
					{
						bool flag6 = eventInteractCheckData != null;
						if (flag6)
						{
							eventInteractCheckData.ConfessionLovePureFixProbDict[2] = -20;
						}
						successRate -= 20;
					}
					int fixRate = 0;
					bool flag7 = isGenderSame && selfChar.GetBisexual() && targetChar.GetBisexual();
					if (flag7)
					{
						fixRate = 20;
						successRate += 20;
					}
					if (eventInteractCheckData != null)
					{
						eventInteractCheckData.ConfessionLovePureFixProbDict.Add(1, fixRate);
					}
					fixRate = 0;
					bool flag8 = RelationType.HasRelation(targetToSelf.RelationType, 1024);
					if (flag8)
					{
						successRate += 40;
						fixRate += 40;
					}
					bool flag9 = RelationType.ContainDirectBloodRelations(targetToSelf.RelationType);
					if (flag9)
					{
						successRate -= 160;
						fixRate -= 160;
					}
					bool flag10 = RelationType.ContainNonBloodFamilyRelations(targetToSelf.RelationType);
					if (flag10)
					{
						successRate -= 80;
						fixRate -= 80;
					}
					bool flag11 = RelationType.HasRelation(selfToTarget.RelationType, 2048) || RelationType.HasRelation(selfToTarget.RelationType, 4096);
					if (flag11)
					{
						successRate -= 40;
						fixRate -= 40;
					}
					if (eventInteractCheckData != null)
					{
						eventInteractCheckData.ConfessionLoveSecularFixProbDict.Add(1, fixRate);
					}
					fixRate = 0;
					short selfCharAge = selfChar.GetCurrAge();
					short targetCharAge = targetChar.GetCurrAge();
					bool flag12 = selfChar.GetGender() == targetChar.GetGender();
					if (flag12)
					{
						int temFixRate = Math.Clamp(3 * Math.Abs((int)(selfCharAge - targetCharAge)), 0, 60);
						successRate -= temFixRate;
						fixRate -= temFixRate;
					}
					else
					{
						int temFixRate2 = (selfChar.GetGender() == 1) ? ((selfCharAge > targetCharAge) ? Math.Clamp((int)(3 * (selfCharAge - targetCharAge - 10)), 0, 60) : Math.Clamp((int)(3 * (targetCharAge - selfCharAge)), 0, 60)) : ((targetCharAge > selfCharAge) ? Math.Clamp((int)(3 * (targetCharAge - selfCharAge - 10)), 0, 60) : Math.Clamp((int)(3 * (selfCharAge - targetCharAge)), 0, 60));
						successRate -= temFixRate2;
						fixRate -= temFixRate2;
					}
					if (eventInteractCheckData != null)
					{
						eventInteractCheckData.ConfessionLovePureFixProbDict.Add(4, fixRate);
					}
					fixRate = 0;
					sbyte targetToSelfFavorType = FavorabilityType.GetFavorabilityType(targetToSelf.Favorability);
					bool flag13 = targetToSelfFavorType >= 3;
					if (flag13)
					{
						int temFixRate3 = (int)((targetToSelfFavorType - 3) * 60);
						successRate += temFixRate3;
						fixRate += temFixRate3;
					}
					if (eventInteractCheckData != null)
					{
						eventInteractCheckData.ConfessionLovePureFixProbDict.Add(5, fixRate);
					}
					fixRate = 0;
					sbyte selfAttractionType = AttractionType.GetAttractionType(selfChar.GetAttraction());
					sbyte targetAttractionType = AttractionType.GetAttractionType(targetChar.GetAttraction());
					int temFixRate4 = (int)((selfAttractionType - targetAttractionType) * 10);
					successRate += temFixRate4;
					fixRate += temFixRate4;
					if (eventInteractCheckData != null)
					{
						eventInteractCheckData.ConfessionLovePureFixProbDict.Add(6, fixRate);
					}
					fixRate = 0;
					int temFixRate5 = (int)((selfChar.GetInteractionGrade() - targetChar.GetInteractionGrade()) * 10);
					successRate += temFixRate5;
					fixRate += temFixRate5;
					if (eventInteractCheckData != null)
					{
						eventInteractCheckData.ConfessionLoveSecularFixProbDict.Add(0, fixRate);
					}
					fixRate = 0;
					bool flag14 = selfChar.GetDisplayingGender() == 0;
					if (flag14)
					{
						fixRate += 20;
						successRate += 20;
					}
					if (eventInteractCheckData != null)
					{
						eventInteractCheckData.ConfessionLoveSecularFixProbDict.Add(2, fixRate);
					}
					int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
					bool flag15 = selfChar.GetId() != taiwuId && targetChar.GetId() != taiwuId;
					if (flag15)
					{
						successRate += 20;
					}
					fixRate = 0;
					RelatedCharacters targetRelatedChars = DomainManager.Character.GetRelatedCharacters(targetChar.GetId());
					HashSet<int> adoreSet = targetRelatedChars.Adored.GetCollection();
					int selfCharId = selfChar.GetId();
					int adoredCount = 0;
					foreach (int adoredCharId in adoreSet)
					{
						bool flag16 = DomainManager.Character.IsCharacterAlive(adoredCharId) && adoredCharId != selfCharId;
						if (flag16)
						{
							adoredCount++;
						}
					}
					bool flag17 = adoredCount > 0;
					if (flag17)
					{
						int featureFactor = AiHelper.Relation.CalcAdoreMultiplePeopleChanceFactor(targetChar);
						sbyte behaviorFactor = AiHelper.RelationsRelatedConstants.StartAdoreChanceChangePerAdored[(int)targetChar.GetBehaviorType()];
						int temFixRate6 = adoredCount * (int)behaviorFactor * featureFactor / 100;
						successRate += temFixRate6;
						fixRate += temFixRate6;
					}
					if (eventInteractCheckData != null)
					{
						eventInteractCheckData.ConfessionLovePureFixProbDict.Add(3, fixRate);
					}
					fixRate = 0;
					int childCount = 0;
					RelatedCharacters selfRelatedChars = DomainManager.Character.GetRelatedCharacters(selfChar.GetId());
					childCount += selfRelatedChars.BloodChildren.GetCount() + selfRelatedChars.StepChildren.GetCount();
					childCount += targetRelatedChars.BloodChildren.GetCount() + targetRelatedChars.StepChildren.GetCount();
					int temFixRate7 = 10 * childCount;
					successRate -= temFixRate7;
					fixRate -= temFixRate7;
					if (eventInteractCheckData != null)
					{
						eventInteractCheckData.ConfessionLoveSecularFixProbDict.Add(4, fixRate);
					}
					fixRate = 0;
					bool flag18 = selfChar.GetMonkType() > 0;
					if (flag18)
					{
						successRate -= 80;
						fixRate -= 80;
					}
					bool flag19 = targetChar.GetMonkType() > 0;
					if (flag19)
					{
						successRate -= 80;
						fixRate -= 80;
					}
					if (eventInteractCheckData != null)
					{
						eventInteractCheckData.ConfessionLoveSecularFixProbDict.Add(6, fixRate);
					}
					fixRate = 0;
					bool flag20 = selfChar.GetFertility() <= 0;
					if (flag20)
					{
						fixRate -= 30;
						successRate -= 30;
					}
					bool flag21 = targetChar.GetFertility() <= 0;
					if (flag21)
					{
						fixRate -= 30;
						successRate -= 30;
					}
					if (eventInteractCheckData != null)
					{
						eventInteractCheckData.ConfessionLoveSecularFixProbDict.Add(5, fixRate);
					}
					result = successRate;
				}
				return result;
			}

			// Token: 0x06008E5B RID: 36443 RVA: 0x004FD754 File Offset: 0x004FB954
			private static int CalcAdoreMultiplePeopleChanceFactor(Character character)
			{
				int featureFactor = 0;
				int featureFactorCount = 0;
				foreach (short featureId in character.GetFeatureIds())
				{
					short currFactor = CharacterFeature.Instance[featureId].AdoreMultiplePeopleChanceFactor;
					bool flag = currFactor == 100;
					if (!flag)
					{
						featureFactor += (int)currFactor;
						featureFactorCount++;
					}
				}
				bool flag2 = featureFactorCount != 0;
				int result;
				if (flag2)
				{
					result = featureFactor / featureFactorCount;
				}
				else
				{
					result = 100;
				}
				return result;
			}
		}
	}
}
