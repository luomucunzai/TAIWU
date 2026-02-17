using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character;

namespace GameData.Domains.World.SectMainStory
{
	// Token: 0x02000032 RID: 50
	public static class SectMainStoryRelatedConstants
	{
		// Token: 0x06000EF9 RID: 3833 RVA: 0x000F9BE0 File Offset: 0x000F7DE0
		public static short GetEnemyLevelByXiangshuLevel()
		{
			sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
			if (!true)
			{
			}
			short result;
			if (xiangshuLevel > 3)
			{
				switch (xiangshuLevel)
				{
				case 4:
					result = 1;
					break;
				case 5:
					result = 2;
					break;
				case 6:
					result = 3;
					break;
				default:
					result = 4;
					break;
				}
			}
			else
			{
				result = 0;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x000F9C34 File Offset: 0x000F7E34
		public static short GetEnemyTemplateIdByXiangshuLevel(sbyte orgTemplateId)
		{
			if (!true)
			{
			}
			short num;
			switch (orgTemplateId)
			{
			case 1:
				num = 320;
				goto IL_7E;
			case 3:
				num = 336;
				goto IL_7E;
			case 5:
				num = 352;
				goto IL_7E;
			case 6:
				num = 360;
				goto IL_7E;
			case 7:
				num = 368;
				goto IL_7E;
			case 10:
				num = 392;
				goto IL_7E;
			case 11:
				num = 400;
				goto IL_7E;
			}
			throw new ArgumentOutOfRangeException("orgTemplateId");
			IL_7E:
			if (!true)
			{
			}
			short templateBase = num;
			short level = SectMainStoryRelatedConstants.GetEnemyLevelByXiangshuLevel();
			return templateBase - level;
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x000F9CD4 File Offset: 0x000F7ED4
		public static short GetTemplateIdOfFixedCharacterCombatWith(short beginTemplateId, short length = 5, bool consummateLevelIncrease = true)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			sbyte consummateLevel = taiwu.GetConsummateLevel();
			short result;
			if (consummateLevelIncrease)
			{
				for (int i = (int)beginTemplateId; i < (int)(beginTemplateId + length); i++)
				{
					CharacterItem config = Config.Character.Instance[i];
					bool flag = config.ConsummateLevel >= consummateLevel;
					if (flag)
					{
						return config.TemplateId;
					}
				}
				result = beginTemplateId + length - 1;
			}
			else
			{
				for (int j = (int)(beginTemplateId + length - 1); j >= (int)beginTemplateId; j--)
				{
					CharacterItem config2 = Config.Character.Instance[j];
					bool flag2 = config2.ConsummateLevel >= consummateLevel;
					if (flag2)
					{
						return config2.TemplateId;
					}
				}
				result = beginTemplateId;
			}
			return result;
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x000F9D98 File Offset: 0x000F7F98
		public static short GetTemplateIdOfXiangshuMinionCombatWith()
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			sbyte consummateLevel = taiwu.GetConsummateLevel();
			for (int i = 306; i >= 298; i--)
			{
				CharacterItem config = Config.Character.Instance[i];
				bool flag = config.ConsummateLevel < consummateLevel;
				if (flag)
				{
					return config.TemplateId;
				}
			}
			return 298;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x000F9E08 File Offset: 0x000F8008
		public static short GetBeginEnemyTemplateIdByXiangshuLevel()
		{
			sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
			if (!true)
			{
			}
			short result;
			if (xiangshuLevel > 3)
			{
				switch (xiangshuLevel)
				{
				case 4:
					result = 273;
					break;
				case 5:
					result = 278;
					break;
				case 6:
					result = 283;
					break;
				default:
					result = 288;
					break;
				}
			}
			else
			{
				result = 268;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x000F9E70 File Offset: 0x000F8070
		public static short GetYuanshanMemberEnemy()
		{
			return SectMainStoryRelatedConstants.GetEnemyTemplateIdByXiangshuLevel(5);
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x000F9E88 File Offset: 0x000F8088
		public static short GetYuanshanDemonMinionFirstTemplateId(short demonTemplateId)
		{
			if (!true)
			{
			}
			short result;
			switch (demonTemplateId)
			{
			case 585:
				result = 588;
				break;
			case 586:
				result = 593;
				break;
			case 587:
				result = 598;
				break;
			default:
				throw new ArgumentOutOfRangeException("demonTemplateId");
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x000F9EE0 File Offset: 0x000F80E0
		public static short GetYuanshanDemonMinionTemplateId(short demonTemplateId)
		{
			short level = SectMainStoryRelatedConstants.GetEnemyLevelByXiangshuLevel();
			short minionTemplate = SectMainStoryRelatedConstants.GetYuanshanDemonMinionFirstTemplateId(demonTemplateId);
			return minionTemplate + level;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x000F9F04 File Offset: 0x000F8104
		public static short GetJingangMemberEnemy()
		{
			return SectMainStoryRelatedConstants.GetEnemyTemplateIdByXiangshuLevel(11);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x000F9F20 File Offset: 0x000F8120
		public static short GetRanshanZhuxianTemplateId()
		{
			sbyte consummateLevel = DomainManager.Taiwu.GetTaiwu().GetConsummateLevel();
			if (!true)
			{
			}
			short result;
			if (consummateLevel > 14)
			{
				if (consummateLevel > 16)
				{
					result = 665;
				}
				else
				{
					result = 664;
				}
			}
			else
			{
				result = 663;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x0400012F RID: 303
		public const short KongsangProsperousEndCountDown = 1;

		// Token: 0x04000130 RID: 304
		public const short XuehouProsperousEndCountDown = 1;

		// Token: 0x04000131 RID: 305
		public const short ShaolinProsperousEndCountDown = 1;

		// Token: 0x04000132 RID: 306
		public const short XuannvProsperousEndCountDown = 1;

		// Token: 0x04000133 RID: 307
		public const short WudangProsperousEndCountDown = 1;

		// Token: 0x04000134 RID: 308
		public const short YuanshanProsperousEndCountDown = 1;

		// Token: 0x04000135 RID: 309
		public const short RanshanProsperousEndCountDown = 1;

		// Token: 0x04000136 RID: 310
		public const short EmeiProsperousEndCountDown = 1;

		// Token: 0x04000137 RID: 311
		public const short ShixiangProsperousEndCountDown = 1;

		// Token: 0x04000138 RID: 312
		public const short JingangProsperousEndCountDown = 1;

		// Token: 0x04000139 RID: 313
		public const short WuxianProsperousEndCountDown = 1;

		// Token: 0x0400013A RID: 314
		public const short JieqingProsperousEndCountDown = 1;

		// Token: 0x0400013B RID: 315
		public const short ZhujianProsperousEndCountDown = 1;

		// Token: 0x0400013C RID: 316
		public const short BaihuaProsperousEndCountDown = 1;

		// Token: 0x0400013D RID: 317
		public const short FulongProsperousEndCountDown = 1;

		// Token: 0x0400013E RID: 318
		public const short KongsangFailingEndCountDown = 1;

		// Token: 0x0400013F RID: 319
		public const short XuehouFailingEndCountDown = 1;

		// Token: 0x04000140 RID: 320
		public const short ShaolinFailingEndCountDown = 1;

		// Token: 0x04000141 RID: 321
		public const short XuannvFailingEndCountDown = 1;

		// Token: 0x04000142 RID: 322
		public const short WudangFailingEndCountDown = 1;

		// Token: 0x04000143 RID: 323
		public const short YuanshanFailingEndCountDown = 1;

		// Token: 0x04000144 RID: 324
		public const short RanshanFailingEndCountDown = 1;

		// Token: 0x04000145 RID: 325
		public const short EmeiFailingEndCountDown = 1;

		// Token: 0x04000146 RID: 326
		public const short ShixiangFailingEndCountDown = 1;

		// Token: 0x04000147 RID: 327
		public const short JingangFailingEndCountDown = 1;

		// Token: 0x04000148 RID: 328
		public const short WuxianFailingEndCountDown = 1;

		// Token: 0x04000149 RID: 329
		public const short JieqingFailingEndCountDown = 1;

		// Token: 0x0400014A RID: 330
		public const short ZhujianFailingEndCountDown = 1;

		// Token: 0x0400014B RID: 331
		public const short BaihuaFailingEndCountDown = 1;

		// Token: 0x0400014C RID: 332
		public const short FulongFailingEndCountDown = 1;

		// Token: 0x0400014D RID: 333
		public const int EndingFavorabilityChange = 12000;

		// Token: 0x0400014E RID: 334
		public const int SendGiftCostTime = 3;

		// Token: 0x0400014F RID: 335
		public const sbyte EndGetInformationLevel = 8;

		// Token: 0x04000150 RID: 336
		public static short[] SectMainStoryCharacterTemplateIds = new short[]
		{
			668,
			667,
			522,
			537,
			538,
			539,
			543,
			585,
			586,
			587,
			631,
			636,
			672,
			673,
			674,
			673,
			675,
			676,
			677,
			608,
			609,
			610,
			611,
			612,
			613,
			614,
			615,
			616,
			617,
			679,
			637,
			623,
			750,
			751,
			752,
			753,
			754,
			755,
			756,
			757,
			758,
			759,
			747,
			660,
			661,
			662,
			625,
			781,
			786,
			808,
			809,
			810,
			813,
			812,
			820,
			821,
			822,
			823,
			824,
			825,
			826,
			827,
			828,
			829,
			835,
			836
		};

		// Token: 0x04000151 RID: 337
		public const short KongsangAdventureCountDown = 240;

		// Token: 0x04000152 RID: 338
		public const short LiaoWumingSendMessageCountDown1 = 3;

		// Token: 0x04000153 RID: 339
		public const short LiaoWumingSendMessageCountDown2 = 1;

		// Token: 0x04000154 RID: 340
		public const int KongsangAdventureCreateDistance = 5;

		// Token: 0x04000155 RID: 341
		public const int KongsangMonthEventAttainmentRequire = 100;

		// Token: 0x04000156 RID: 342
		public const int KongsangEnding0GetMedicineCount = 9;

		// Token: 0x04000157 RID: 343
		public const int KongsangEnding1GetPoisonCount = 1;

		// Token: 0x04000158 RID: 344
		public const int KongsangAdventureGetPoisonCount = 99;

		// Token: 0x04000159 RID: 345
		public const int TripodVesselOfMedicineCost = 5000;

		// Token: 0x0400015A RID: 346
		public const int TripodVesselActionCooldown = 2;

		// Token: 0x0400015B RID: 347
		public const int TripodVesselOfMedicineHealthIncrease = 360;

		// Token: 0x0400015C RID: 348
		public const int TryPoisonChangeFavorabilityValue = 3000;

		// Token: 0x0400015D RID: 349
		public const int LiaoWumingInitialFavorabilityValue = 9999;

		// Token: 0x0400015E RID: 350
		public static readonly int[] TripodVesselOfMedicineProb = new int[]
		{
			10,
			20,
			31,
			51,
			71,
			81,
			91,
			97,
			100
		};

		// Token: 0x0400015F RID: 351
		public static readonly int[] LiaoWumingAi2TaskInfo = new int[]
		{
			107,
			110,
			113,
			108,
			111,
			106,
			109,
			112
		};

		// Token: 0x04000160 RID: 352
		public static readonly int[] CanNotMeetSkeletonCharacters = new int[]
		{
			668
		};

		// Token: 0x04000161 RID: 353
		public static readonly int[] CanNotGraveInteractionCharavters = new int[]
		{
			521
		};

		// Token: 0x04000162 RID: 354
		public static readonly short[][] MakeMixedPoison = new short[][]
		{
			new short[]
			{
				0,
				27,
				36
			},
			new short[]
			{
				0,
				36,
				45
			},
			new short[]
			{
				0,
				36,
				9
			},
			new short[]
			{
				0,
				36,
				18
			},
			new short[]
			{
				27,
				36,
				45
			},
			new short[]
			{
				27,
				36,
				9
			},
			new short[]
			{
				27,
				36,
				18
			},
			new short[]
			{
				0,
				27,
				45
			},
			new short[]
			{
				0,
				27,
				9
			},
			new short[]
			{
				0,
				27,
				18
			},
			new short[]
			{
				9,
				18,
				45
			},
			new short[]
			{
				36,
				9,
				18
			},
			new short[]
			{
				0,
				9,
				18
			},
			new short[]
			{
				27,
				9,
				18
			},
			new short[]
			{
				36,
				18,
				45
			},
			new short[]
			{
				0,
				18,
				45
			},
			new short[]
			{
				27,
				18,
				45
			},
			new short[]
			{
				36,
				9,
				45
			},
			new short[]
			{
				0,
				9,
				45
			},
			new short[]
			{
				27,
				9,
				45
			}
		};

		// Token: 0x04000163 RID: 355
		public const short GetBellTriggerEventCountDown = 3;

		// Token: 0x04000164 RID: 356
		public const int MeetXuehouSkeletonProb = 30;

		// Token: 0x04000165 RID: 357
		public const short DefeatXuehouOldManCountDown = 3;

		// Token: 0x04000166 RID: 358
		public const short GiveBellToXuehouOldManCountDown = 3;

		// Token: 0x04000167 RID: 359
		public const short XuehouAdventureThreeCountDown = 3;

		// Token: 0x04000168 RID: 360
		public const short XuehouAdventureFourCountDown = 3;

		// Token: 0x04000169 RID: 361
		public const short JixiAdventureDuration = 9;

		// Token: 0x0400016A RID: 362
		public const short JixiAdventureFourDuration = 6;

		// Token: 0x0400016B RID: 363
		public const short VillagerCountConst = 3;

		// Token: 0x0400016C RID: 364
		public const short XuehouComingCoolDownTime = 3;

		// Token: 0x0400016D RID: 365
		public const short JixiAdventureOneAddDisorderOfQi = 2000;

		// Token: 0x0400016E RID: 366
		public const short JixiFavorChangeToTaiwuOne = 4000;

		// Token: 0x0400016F RID: 367
		public const short JixiFavorChangeToTaiwuTwo = 1000;

		// Token: 0x04000170 RID: 368
		public const sbyte JixiTruthClueConst = 2;

		// Token: 0x04000171 RID: 369
		public const short TaiwuCookNeedFoodResource = 1000;

		// Token: 0x04000172 RID: 370
		public const short TaiwuCookAttainmentThreshold = 300;

		// Token: 0x04000173 RID: 371
		public const short MonthEventChangeFavorability = 600;

		// Token: 0x04000174 RID: 372
		public const short XuehouAdventure1Cost = 50;

		// Token: 0x04000175 RID: 373
		[Obsolete]
		public const int ShaolinLifeCombatPracticeLevel = 30;

		// Token: 0x04000176 RID: 374
		public const int ShaolinSutraPavilionCost = 30;

		// Token: 0x04000177 RID: 375
		public const int ShaolinRepairPagodaCost = 20;

		// Token: 0x04000178 RID: 376
		public const int ShaolinRepairPagodaSpiritualDebt = 500;

		// Token: 0x04000179 RID: 377
		public const int CombatSkillBreakoutTimeCost = 10;

		// Token: 0x0400017A RID: 378
		public const int HighestGradeSutraDurability = 15;

		// Token: 0x0400017B RID: 379
		public const short ShaolinStudyForBodhidharmaChallengeInterval = 6;

		// Token: 0x0400017C RID: 380
		public const short ShaolinReadSutraNeedPageCount = 3;

		// Token: 0x0400017D RID: 381
		public const int WaitForTaoistMonkCountDown = 3;

		// Token: 0x0400017E RID: 382
		public static ValueTuple<short, short>[] SnakeItemInfo = new ValueTuple<short, short>[]
		{
			new ValueTuple<short, short>(555, 248),
			new ValueTuple<short, short>(567, 252),
			new ValueTuple<short, short>(573, 254),
			new ValueTuple<short, short>(561, 250),
			new ValueTuple<short, short>(564, 251),
			new ValueTuple<short, short>(558, 249),
			new ValueTuple<short, short>(570, 253),
			new ValueTuple<short, short>(576, 255),
			new ValueTuple<short, short>(579, 256),
			new ValueTuple<short, short>(582, 257)
		};

		// Token: 0x0400017F RID: 383
		public const int ModifySkillBookTimeCost = 10;

		// Token: 0x04000180 RID: 384
		public const short HeavenlyTreeInfluenceRange = 3;

		// Token: 0x04000181 RID: 385
		public const short WudangXiangshuMinionHealthReduction = 60;

		// Token: 0x04000182 RID: 386
		public const short WudangHeavenlyTreeGrowthPoisonThreshold = 900;

		// Token: 0x04000183 RID: 387
		public const short MaxGetExtraSeedCount = 7;

		// Token: 0x04000184 RID: 388
		public const short ReadBookAddHeavenlyTreeGrowPoint = 100;

		// Token: 0x04000185 RID: 389
		public const sbyte MythInYuanshanMoveDelay = 1;

		// Token: 0x04000186 RID: 390
		public const sbyte InfectedAbsorbance = 10;

		// Token: 0x04000187 RID: 391
		public const sbyte PowerTranslateMultiplier = 2;

		// Token: 0x04000188 RID: 392
		public const int DemonDistanceToSettlement = 5;

		// Token: 0x04000189 RID: 393
		public const int DemonDistanceToOther = 4;

		// Token: 0x0400018A RID: 394
		public const int DemonMinionStep = 2;

		// Token: 0x0400018B RID: 395
		public const int DemonFightStep = 2;

		// Token: 0x0400018C RID: 396
		public const int DemonFightCountMin = 5;

		// Token: 0x0400018D RID: 397
		public const int DemonFightCountMax = 10;

		// Token: 0x0400018E RID: 398
		public const int DemonFightWinScore = 50;

		// Token: 0x0400018F RID: 399
		public const int DemonFightAttackCountWin = 2;

		// Token: 0x04000190 RID: 400
		public const int DemonFightAttackCountTie = 3;

		// Token: 0x04000191 RID: 401
		public const int DemonFightAttackCountLose = 4;

		// Token: 0x04000192 RID: 402
		public const int DemonFightDefenseCountWin = 2;

		// Token: 0x04000193 RID: 403
		public const int DemonFightDefenseCountTie = 1;

		// Token: 0x04000194 RID: 404
		public const int DemonFightDefenseCountLose = 0;

		// Token: 0x04000195 RID: 405
		public const int DemonFightCooldownWin = 1;

		// Token: 0x04000196 RID: 406
		public const int DemonFightCooldownTie = 3;

		// Token: 0x04000197 RID: 407
		public const int DemonFightCooldownLose = 6;

		// Token: 0x04000198 RID: 408
		public const int DemonFightPowerReductionRateWin = 3;

		// Token: 0x04000199 RID: 409
		public const int DemonFightPowerReductionRateTie = 1;

		// Token: 0x0400019A RID: 410
		public const int ShixiangKilledCountLimitMin = 6;

		// Token: 0x0400019B RID: 411
		public const int ShixiangKilledCountLimitMax = 12;

		// Token: 0x0400019C RID: 412
		public const int ShixiangFightAttackCount = 2;

		// Token: 0x0400019D RID: 413
		public const int ShixiangFightDefenseCount = 2;

		// Token: 0x0400019E RID: 414
		public const int ShixiangAdventureDuration = 3;

		// Token: 0x0400019F RID: 415
		public const int ShixiangEnemyCountFloor = 10;

		// Token: 0x040001A0 RID: 416
		public const int ShixiangEnemyCountCeiling = 15;

		// Token: 0x040001A1 RID: 417
		public const int ShixiangTraitorCountFloor = 15;

		// Token: 0x040001A2 RID: 418
		public const int ShixiangTraitorCountCeiling = 20;

		// Token: 0x040001A3 RID: 419
		public const int ShixiangKillTraitorTaskTimeLimit = 36;

		// Token: 0x040001A4 RID: 420
		public const int ShixiangBarbarianMasterCountFloor = 30;

		// Token: 0x040001A5 RID: 421
		public const int ShixiangBarbarianMasterCountCeiling = 40;

		// Token: 0x040001A6 RID: 422
		public const int MockShixiangEventThresholdCount = 3;

		// Token: 0x040001A7 RID: 423
		public const int EclecticAttainmentNeed = 150;

		// Token: 0x040001A8 RID: 424
		public const int MonthEventRequestBookAuthorityChange = 250;

		// Token: 0x040001A9 RID: 425
		public const int MonthEventRequestSkillBookFavorChange = 3000;

		// Token: 0x040001AA RID: 426
		public const int MonthEventTalkRightFavorChange = 1500;

		// Token: 0x040001AB RID: 427
		public const int MonthEventTalkWrongFavorChange = -500;

		// Token: 0x040001AC RID: 428
		public const int SelectCombatWithBarbariansFavorChange = 3000;

		// Token: 0x040001AD RID: 429
		public const int ShixiangMemberFavorChange = 3000;

		// Token: 0x040001AE RID: 430
		public const int ShixiangNormalWorldProb = 1;

		// Token: 0x040001AF RID: 431
		public const int ShixiangMemberInteractChance = 30;

		// Token: 0x040001B0 RID: 432
		public static short[] BarbarianMasterHairColorTemplateId = new short[]
		{
			6,
			12,
			42
		};

		// Token: 0x040001B1 RID: 433
		public const int EmeiAdventureTwoDuration = 6;

		// Token: 0x040001B2 RID: 434
		public const int EmeiBreakBonusCostExpBaseValue = 500;

		// Token: 0x040001B3 RID: 435
		public const int EmeiBreakBonusCostExpPerTimes = 500;

		// Token: 0x040001B4 RID: 436
		public const int EmeiBreakBonusCountPerRefresh = 5;

		// Token: 0x040001B5 RID: 437
		public const int EmeiAdventureTwoPathEventTriggerCountLimit = 6;

		// Token: 0x040001B6 RID: 438
		public static ValueTuple<short, short>[] XuannvUnlockMusicCountToFavorability = new ValueTuple<short, short>[]
		{
			new ValueTuple<short, short>(0, 14000),
			new ValueTuple<short, short>(10, 18000),
			new ValueTuple<short, short>(20, 22000),
			new ValueTuple<short, short>(30, 26000),
			new ValueTuple<short, short>(40, 30000)
		};

		// Token: 0x040001B7 RID: 439
		public const int JingangWrongdoingFrequency = 2;

		// Token: 0x040001B8 RID: 440
		public static int[] JingangTaiwuSpreadSecInfoCount = new int[]
		{
			3,
			2,
			1,
			1
		};

		// Token: 0x040001B9 RID: 441
		public static int[] JingangSecInfoSpreadSpeed = new int[]
		{
			1,
			3,
			4,
			5
		};

		// Token: 0x040001BA RID: 442
		public static int[] JingangSecInfoOpenCount = new int[]
		{
			5,
			10,
			15,
			20
		};

		// Token: 0x040001BB RID: 443
		public static int JingangEventFrequency1 = 3;

		// Token: 0x040001BC RID: 444
		public static int JingangEventFrequency2 = 6;

		// Token: 0x040001BD RID: 445
		public const int RanXinduFavorabilityInitial = 10000;

		// Token: 0x040001BE RID: 446
		public const int RanXinDuFavorabilityDeltaGood = 1000;

		// Token: 0x040001BF RID: 447
		public const int RanXinDuFavorabilityDeltaBad = -1000;

		// Token: 0x040001C0 RID: 448
		public const int RanXinDuFavorabilityDeltaAgreed = 3000;

		// Token: 0x040001C1 RID: 449
		public const int RanXinDuFavorabilityDeltaRefused = -3000;

		// Token: 0x040001C2 RID: 450
		public const int DisorderOfQiDeltaAfterDrunk = 2000;

		// Token: 0x040001C3 RID: 451
		public const int RanXinduFavorabilityDeltaRamble = 500;

		// Token: 0x040001C4 RID: 452
		public const int RanXinduFavorabilityDeltaGuard = 3000;

		// Token: 0x040001C5 RID: 453
		public const int RanXinduFavorabilityDeltaPlaySerious = 1000;

		// Token: 0x040001C6 RID: 454
		public const int RanXinduFavorabilityDeltaPlayNormal = 500;

		// Token: 0x040001C7 RID: 455
		public const sbyte LiceWugInjuryDelta = 1;

		// Token: 0x040001C8 RID: 456
		public const sbyte BambooStripsWugInjuryDelta = 1;

		// Token: 0x040001C9 RID: 457
		public const int AphrodisiacWugDisorderOfQiDelta = 1000;

		// Token: 0x040001CA RID: 458
		public const int NewParanoiaCharacterChance = 25;

		// Token: 0x040001CB RID: 459
		public const int WuxianBadEnding0CountDown = 3;

		// Token: 0x040001CC RID: 460
		public const int WuxianBadEnding1CountDown = 1;

		// Token: 0x040001CD RID: 461
		public const int WuxianHappyEndingCountDown = 1;

		// Token: 0x040001CE RID: 462
		public const int WuxianHappyEndingEventCountDown = 3;

		// Token: 0x040001CF RID: 463
		public static List<string[]> RanXinduFairyTexts = new List<string[]>
		{
			new string[]
			{
				"Event_SectStoryWuxian_RanXinduFairy_0_1",
				"Event_SectStoryWuxian_RanXinduFairy_0_2",
				"Event_SectStoryWuxian_RanXinduFairy_0_3",
				"Event_SectStoryWuxian_RanXinduFairy_0_4",
				"Event_SectStoryWuxian_RanXinduFairy_0_5",
				"Event_SectStoryWuxian_RanXinduFairy_0_6",
				"Event_SectStoryWuxian_RanXinduFairy_0_7",
				"Event_SectStoryWuxian_RanXinduFairy_0_8",
				"Event_SectStoryWuxian_RanXinduFairy_0_9",
				"Event_SectStoryWuxian_RanXinduFairy_0_10",
				"Event_SectStoryWuxian_RanXinduFairy_0_11",
				"Event_SectStoryWuxian_RanXinduFairy_0_12",
				"Event_SectStoryWuxian_RanXinduFairy_0_13",
				"Event_SectStoryWuxian_RanXinduFairy_0_14",
				"Event_SectStoryWuxian_RanXinduFairy_0_15"
			},
			new string[]
			{
				"Event_SectStoryWuxian_RanXinduFairy_1_1",
				"Event_SectStoryWuxian_RanXinduFairy_1_2",
				"Event_SectStoryWuxian_RanXinduFairy_1_3",
				"Event_SectStoryWuxian_RanXinduFairy_1_4"
			},
			new string[]
			{
				"Event_SectStoryWuxian_RanXinduFairy_2_1",
				"Event_SectStoryWuxian_RanXinduFairy_2_2",
				"Event_SectStoryWuxian_RanXinduFairy_2_3",
				"Event_SectStoryWuxian_RanXinduFairy_2_4",
				"Event_SectStoryWuxian_RanXinduFairy_2_5"
			},
			new string[]
			{
				"Event_SectStoryWuxian_RanXinduFairy_3_1",
				"Event_SectStoryWuxian_RanXinduFairy_3_2",
				"Event_SectStoryWuxian_RanXinduFairy_3_3",
				"Event_SectStoryWuxian_RanXinduFairy_3_4",
				"Event_SectStoryWuxian_RanXinduFairy_3_5"
			}
		};

		// Token: 0x040001D0 RID: 464
		public static int[] PoisonItemsFromRanXinduCount = new int[]
		{
			1,
			2,
			3,
			3,
			3,
			2,
			1
		};

		// Token: 0x040001D1 RID: 465
		public const int ZhangLingFavorabilityInitial = 14000;

		// Token: 0x040001D2 RID: 466
		public const int ThreeCorpsesFavorabilityInitial = 10000;

		// Token: 0x040001D3 RID: 467
		public const int ThreeCorpsesFavorabilityDeltaPositive1 = 2000;

		// Token: 0x040001D4 RID: 468
		public const int ThreeCorpsesFavorabilityDeltaNegative1 = 1000;

		// Token: 0x040001D5 RID: 469
		public const int RanshanMenteeFavorabilityDelta = 6000;

		// Token: 0x040001D6 RID: 470
		public const int RanshanMenteeHappinessDelta = -20;

		// Token: 0x040001D7 RID: 471
		public const int RanshanThreeCorpsesMoveSteps = 2;

		// Token: 0x040001D8 RID: 472
		public const int RanshanTeachingDuration = 24;

		// Token: 0x040001D9 RID: 473
		public const int RanshanThreeCorpsesFavorabilityDeltaCombat = 1000;

		// Token: 0x040001DA RID: 474
		public static List<short> RanshanThreeCorpsesCharacterTemplateIdList = new List<short>
		{
			660,
			661,
			662
		};

		// Token: 0x040001DB RID: 475
		public const int ThreeCorpsesFollowMaxSteps = 2;

		// Token: 0x040001DC RID: 476
		public const int BaihuaAdventureFourDuration = 1;

		// Token: 0x040001DD RID: 477
		public const int BaihuaDreamAboutPastLastDuration = 3;

		// Token: 0x040001DE RID: 478
		public const int BaihuaAmbushNeedGroupCount = 1;

		// Token: 0x040001DF RID: 479
		public const int BaihuaAnimalsBackToPandemicStart = 6;

		// Token: 0x040001E0 RID: 480
		public const int BaihuaManicLowSustain = 6;

		// Token: 0x040001E1 RID: 481
		public const int BaihuaManicLowToHighDuration = 3;

		// Token: 0x040001E2 RID: 482
		public const int BaihuaManicHighToAdventureDuration = 3;

		// Token: 0x040001E3 RID: 483
		public static int FulongZealotStartRobTime = 3;

		// Token: 0x040001E4 RID: 484
		public static int FulongAdventureOneFavorabilityChange = 800;

		// Token: 0x040001E5 RID: 485
		public static short FulongLazuliStartFavorability = 10000;

		// Token: 0x040001E6 RID: 486
		public static int FulongLazuliFavorabilityAdd = 6000;

		// Token: 0x040001E7 RID: 487
		public const int FulongOutLawResupplyCooldown = 6;

		// Token: 0x040001E8 RID: 488
		public const int FulongOutLawRobCooldown = 1;

		// Token: 0x040001E9 RID: 489
		public const int FulongOutlawRobResourceTypeCount = 2;

		// Token: 0x040001EA RID: 490
		public const int FulongOutLawMinimumCount = 5;

		// Token: 0x040001EB RID: 491
		public const int FulongOutLawMaximumCount = 9;

		// Token: 0x040001EC RID: 492
		public const int FulongOutLawMinimumCountTaiwu = 3;

		// Token: 0x040001ED RID: 493
		public const int FulongOutLawMaximumCountTaiwu = 6;

		// Token: 0x040001EE RID: 494
		public const int FulongOutLawVictimFavorabilityChange = 6000;

		// Token: 0x040001EF RID: 495
		public const int FulongOutLawVictimFavorabilityChangeFamily = 8000;

		// Token: 0x040001F0 RID: 496
		public static readonly sbyte[] FulongOutLawFightBehaviorBonus = new sbyte[]
		{
			30,
			20,
			10,
			25,
			40
		};

		// Token: 0x040001F1 RID: 497
		public static readonly sbyte[] FulongOutLawDestination = new sbyte[]
		{
			16,
			35,
			33,
			29
		};

		// Token: 0x040001F2 RID: 498
		public static int FulongChickenFeatherNeedCount = 12;

		// Token: 0x040001F3 RID: 499
		public const short YuanshanMinionTemplateOffsetMax = 4;
	}
}
