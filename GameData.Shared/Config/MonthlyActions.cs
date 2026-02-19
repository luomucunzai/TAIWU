using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells.Character;

namespace Config;

[Serializable]
public class MonthlyActions : ConfigData<MonthlyActionsItem, short>
{
	public static class DefKey
	{
		public const short ContestForBrideJingcheng = 0;

		public const short ContestForBrideChengdu = 1;

		public const short ContestForBrideGuizhou = 2;

		public const short ContestForBrideXiangyang = 3;

		public const short ContestForBrideTaiyuan = 4;

		public const short ContestForBrideGuangzhou = 5;

		public const short ContestForBrideQingzhou = 6;

		public const short ContestForBrideJiangling = 7;

		public const short ContestForBrideFuzhou = 8;

		public const short ContestForBrideLiaoyang = 9;

		public const short ContestForBrideQinzhou = 10;

		public const short ContestForBrideDali = 11;

		public const short ContestForBrideShouchun = 12;

		public const short ContestForBrideHangzhou = 13;

		public const short ContestForBrideYangzhou = 14;

		public const short EnemyNestXiuluochangPrepare = 23;

		public const short EnemyNestFlurryofDemonsPrepare = 24;

		public const short EnemyNestDeadEndPrepare = 25;

		public const short EnemyNestEvilGroundPrepare = 26;

		public const short MartialArtTournamentPrepare = 30;

		public const short BrideOpenContestJingcheng = 31;

		public const short BrideOpenContestChengdu = 32;

		public const short BrideOpenContestGuizhou = 33;

		public const short BrideOpenContestXiangyang = 34;

		public const short BrideOpenContestTaiyuan = 35;

		public const short BrideOpenContestGuangzhou = 36;

		public const short BrideOpenContestQingzhou = 37;

		public const short BrideOpenContestJiangling = 38;

		public const short BrideOpenContestFuzhou = 39;

		public const short BrideOpenContestLiaoyang = 40;

		public const short BrideOpenContestQinzhou = 41;

		public const short BrideOpenContestDali = 42;

		public const short BrideOpenContestShouchun = 43;

		public const short BrideOpenContestHangzhou = 44;

		public const short BrideOpenContestYangzhou = 45;

		public const short SectNormalCompetitionShaolin = 46;

		public const short SectNormalCompetitionEmei = 47;

		public const short SectNormalCompetitionBaihua = 48;

		public const short SectNormalCompetitionWudang = 49;

		public const short SectNormalCompetitionYuanshan = 50;

		public const short SectNormalCompetitionShixiang = 51;

		public const short SectNormalCompetitionRanshan = 52;

		public const short SectNormalCompetitionXuannv = 53;

		public const short SectNormalCompetitionZhujian = 54;

		public const short SectNormalCompetitionKongsang = 55;

		public const short SectNormalCompetitionJingang = 56;

		public const short SectNormalCompetitionWuxian = 57;

		public const short SectNormalCompetitionJieqing = 58;

		public const short SectNormalCompetitionFulong = 59;

		public const short SectNormalCompetitionXuehou = 60;

		public const short SpringMarketPrepare = 61;

		public const short CityCombatSkillCompetitionFistAndPalm = 62;

		public const short CityCombatSkillCompetitionFinger = 63;

		public const short CityCombatSkillCompetitionLeg = 64;

		public const short CityCombatSkillCompetitionThrow = 65;

		public const short CityCombatSkillCompetitionSword = 66;

		public const short CityCombatSkillCompetitionBlade = 67;

		public const short CityCombatSkillCompetitionPolearm = 68;

		public const short CityCombatSkillCompetitionSpecial = 69;

		public const short CityCombatSkillCompetitionWhip = 70;

		public const short CityCombatSkillCompetitionControllableShot = 71;

		public const short CityCombatSkillCompetitionCombatMusic = 72;

		public const short CricketConference = 73;

		public const short CityLifeSkillCompetitionForging = 74;

		public const short CityLifeSkillCompetitionWoodworking = 75;

		public const short CityLifeSkillCompetitionWeaving = 76;

		public const short CityLifeSkillCompetitionJade = 77;

		public const short CityLifeSkillCompetitionMedicine = 78;

		public const short CityLifeSkillCompetitionToxicology = 79;

		public const short CityLifeSkillCompetitionCooking = 80;

		public const short SectMainStoryEmeiTwo = 81;

		public const short SectMainStoryRanshan = 82;

		public const short SectMainStoryZhujian = 83;

		public const short SectMainStoryYuanshan = 84;
	}

	public static class DefValue
	{
		public static MonthlyActionsItem ContestForBrideJingcheng => Instance[(short)0];

		public static MonthlyActionsItem ContestForBrideChengdu => Instance[(short)1];

		public static MonthlyActionsItem ContestForBrideGuizhou => Instance[(short)2];

		public static MonthlyActionsItem ContestForBrideXiangyang => Instance[(short)3];

		public static MonthlyActionsItem ContestForBrideTaiyuan => Instance[(short)4];

		public static MonthlyActionsItem ContestForBrideGuangzhou => Instance[(short)5];

		public static MonthlyActionsItem ContestForBrideQingzhou => Instance[(short)6];

		public static MonthlyActionsItem ContestForBrideJiangling => Instance[(short)7];

		public static MonthlyActionsItem ContestForBrideFuzhou => Instance[(short)8];

		public static MonthlyActionsItem ContestForBrideLiaoyang => Instance[(short)9];

		public static MonthlyActionsItem ContestForBrideQinzhou => Instance[(short)10];

		public static MonthlyActionsItem ContestForBrideDali => Instance[(short)11];

		public static MonthlyActionsItem ContestForBrideShouchun => Instance[(short)12];

		public static MonthlyActionsItem ContestForBrideHangzhou => Instance[(short)13];

		public static MonthlyActionsItem ContestForBrideYangzhou => Instance[(short)14];

		public static MonthlyActionsItem EnemyNestXiuluochangPrepare => Instance[(short)23];

		public static MonthlyActionsItem EnemyNestFlurryofDemonsPrepare => Instance[(short)24];

		public static MonthlyActionsItem EnemyNestDeadEndPrepare => Instance[(short)25];

		public static MonthlyActionsItem EnemyNestEvilGroundPrepare => Instance[(short)26];

		public static MonthlyActionsItem MartialArtTournamentPrepare => Instance[(short)30];

		public static MonthlyActionsItem BrideOpenContestJingcheng => Instance[(short)31];

		public static MonthlyActionsItem BrideOpenContestChengdu => Instance[(short)32];

		public static MonthlyActionsItem BrideOpenContestGuizhou => Instance[(short)33];

		public static MonthlyActionsItem BrideOpenContestXiangyang => Instance[(short)34];

		public static MonthlyActionsItem BrideOpenContestTaiyuan => Instance[(short)35];

		public static MonthlyActionsItem BrideOpenContestGuangzhou => Instance[(short)36];

		public static MonthlyActionsItem BrideOpenContestQingzhou => Instance[(short)37];

		public static MonthlyActionsItem BrideOpenContestJiangling => Instance[(short)38];

		public static MonthlyActionsItem BrideOpenContestFuzhou => Instance[(short)39];

		public static MonthlyActionsItem BrideOpenContestLiaoyang => Instance[(short)40];

		public static MonthlyActionsItem BrideOpenContestQinzhou => Instance[(short)41];

		public static MonthlyActionsItem BrideOpenContestDali => Instance[(short)42];

		public static MonthlyActionsItem BrideOpenContestShouchun => Instance[(short)43];

		public static MonthlyActionsItem BrideOpenContestHangzhou => Instance[(short)44];

		public static MonthlyActionsItem BrideOpenContestYangzhou => Instance[(short)45];

		public static MonthlyActionsItem SectNormalCompetitionShaolin => Instance[(short)46];

		public static MonthlyActionsItem SectNormalCompetitionEmei => Instance[(short)47];

		public static MonthlyActionsItem SectNormalCompetitionBaihua => Instance[(short)48];

		public static MonthlyActionsItem SectNormalCompetitionWudang => Instance[(short)49];

		public static MonthlyActionsItem SectNormalCompetitionYuanshan => Instance[(short)50];

		public static MonthlyActionsItem SectNormalCompetitionShixiang => Instance[(short)51];

		public static MonthlyActionsItem SectNormalCompetitionRanshan => Instance[(short)52];

		public static MonthlyActionsItem SectNormalCompetitionXuannv => Instance[(short)53];

		public static MonthlyActionsItem SectNormalCompetitionZhujian => Instance[(short)54];

		public static MonthlyActionsItem SectNormalCompetitionKongsang => Instance[(short)55];

		public static MonthlyActionsItem SectNormalCompetitionJingang => Instance[(short)56];

		public static MonthlyActionsItem SectNormalCompetitionWuxian => Instance[(short)57];

		public static MonthlyActionsItem SectNormalCompetitionJieqing => Instance[(short)58];

		public static MonthlyActionsItem SectNormalCompetitionFulong => Instance[(short)59];

		public static MonthlyActionsItem SectNormalCompetitionXuehou => Instance[(short)60];

		public static MonthlyActionsItem SpringMarketPrepare => Instance[(short)61];

		public static MonthlyActionsItem CityCombatSkillCompetitionFistAndPalm => Instance[(short)62];

		public static MonthlyActionsItem CityCombatSkillCompetitionFinger => Instance[(short)63];

		public static MonthlyActionsItem CityCombatSkillCompetitionLeg => Instance[(short)64];

		public static MonthlyActionsItem CityCombatSkillCompetitionThrow => Instance[(short)65];

		public static MonthlyActionsItem CityCombatSkillCompetitionSword => Instance[(short)66];

		public static MonthlyActionsItem CityCombatSkillCompetitionBlade => Instance[(short)67];

		public static MonthlyActionsItem CityCombatSkillCompetitionPolearm => Instance[(short)68];

		public static MonthlyActionsItem CityCombatSkillCompetitionSpecial => Instance[(short)69];

		public static MonthlyActionsItem CityCombatSkillCompetitionWhip => Instance[(short)70];

		public static MonthlyActionsItem CityCombatSkillCompetitionControllableShot => Instance[(short)71];

		public static MonthlyActionsItem CityCombatSkillCompetitionCombatMusic => Instance[(short)72];

		public static MonthlyActionsItem CricketConference => Instance[(short)73];

		public static MonthlyActionsItem CityLifeSkillCompetitionForging => Instance[(short)74];

		public static MonthlyActionsItem CityLifeSkillCompetitionWoodworking => Instance[(short)75];

		public static MonthlyActionsItem CityLifeSkillCompetitionWeaving => Instance[(short)76];

		public static MonthlyActionsItem CityLifeSkillCompetitionJade => Instance[(short)77];

		public static MonthlyActionsItem CityLifeSkillCompetitionMedicine => Instance[(short)78];

		public static MonthlyActionsItem CityLifeSkillCompetitionToxicology => Instance[(short)79];

		public static MonthlyActionsItem CityLifeSkillCompetitionCooking => Instance[(short)80];

		public static MonthlyActionsItem SectMainStoryEmeiTwo => Instance[(short)81];

		public static MonthlyActionsItem SectMainStoryRanshan => Instance[(short)82];

		public static MonthlyActionsItem SectMainStoryZhujian => Instance[(short)83];

		public static MonthlyActionsItem SectMainStoryYuanshan => Instance[(short)84];
	}

	public static MonthlyActions Instance = new MonthlyActions();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "EnterMonthList", "MapState", "MapArea", "MajorTargetFilterList", "ParticipateTargetFilterList", "AdventureId", "NotificationId", "TemplateId", "Name" };

	internal override int ToInt(short value)
	{
		return value;
	}

	internal override short ToTemplateId(int value)
	{
		return (short)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new MonthlyActionsItem(0, 0, new List<sbyte> { 8 }, -1, -1, new List<short> { 1 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 58, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(1, 1, new List<sbyte> { 4 }, -1, -1, new List<short> { 2 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 44, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(2, 2, new List<sbyte> { 11 }, -1, -1, new List<short> { 3 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 52, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(3, 3, new List<sbyte> { 6 }, -1, -1, new List<short> { 4 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 70, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(4, 4, new List<sbyte> { 9 }, -1, -1, new List<short> { 5 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 68, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(5, 5, new List<sbyte> { 10 }, -1, -1, new List<short> { 6 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 50, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(6, 6, new List<sbyte> { 5 }, -1, -1, new List<short> { 7 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 64, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(7, 7, new List<sbyte> { 1 }, -1, -1, new List<short> { 8 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 56, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(8, 8, new List<sbyte> { 5 }, -1, -1, new List<short> { 9 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 48, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(9, 9, new List<sbyte> { 2 }, -1, -1, new List<short> { 10 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 60, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(10, 10, new List<sbyte> { 1 }, -1, -1, new List<short> { 11 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 62, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(11, 11, new List<sbyte> { 7 }, -1, -1, new List<short> { 12 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1),
			new CharacterFilterRequirement(new int[1] { 8 }, 3, 10)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 5),
			new CharacterFilterRequirement(new int[1] { 9 }, 3, 10)
		}, 46, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(12, 12, new List<sbyte> { 3 }, -1, -1, new List<short> { 13 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 66, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(13, 13, new List<sbyte> { 11 }, -1, -1, new List<short> { 14 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 54, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(14, 14, new List<sbyte> { 0 }, -1, -1, new List<short> { 15 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[2] { 3, 4 }, 1),
			new CharacterFilterRequirement(new int[1] { 10 }, 5, 7)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 5 }, 8)
		}, 72, 139, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 6, 2, 36, 0));
		_dataArray.Add(new MonthlyActionsItem(15, 15, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 29, 119, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(16, 16, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 30, 120, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(17, 17, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 34, 123, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(18, 18, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 31, 121, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(19, 19, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 33, 122, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(20, 20, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 35, 124, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(21, 21, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 38, 126, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(22, 22, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 36, 125, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(23, 23, new List<sbyte>(), -1, -1, new List<short>(), 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 11, 89 }, 1)
		}, new CharacterFilterRequirement[0], 40, 128, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 2, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(24, 24, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 7 }, 3, 5)
		}, new CharacterFilterRequirement[0], 41, 129, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 2, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(25, 25, new List<sbyte>(), -1, -1, new List<short>(), 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 12 }, 1)
		}, new CharacterFilterRequirement[0], 43, 130, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 2, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(26, 26, new List<sbyte>(), -1, -1, new List<short>(), 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[2] { 1, 88 }, 1)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 2 }, 0, 3)
		}, 39, 127, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 2, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(27, 27, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 32, -1, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(28, 28, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 37, -1, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(29, 29, new List<sbyte>(), -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 42, -1, isEnemyNest: true, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(30, 30, new List<sbyte>(), -1, -1, new List<short>(), 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 15 }, 30, 45),
			new CharacterFilterRequirement(new int[1] { 16 }, 15, 15)
		}, 105, -1, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(31, 31, new List<sbyte>(), -1, -1, new List<short> { 1 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 59, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(32, 32, new List<sbyte>(), -1, -1, new List<short> { 2 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 45, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(33, 33, new List<sbyte>(), -1, -1, new List<short> { 3 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 53, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(34, 34, new List<sbyte>(), -1, -1, new List<short> { 4 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 71, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(35, 35, new List<sbyte>(), -1, -1, new List<short> { 5 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 69, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(36, 36, new List<sbyte>(), -1, -1, new List<short> { 6 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 51, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(37, 37, new List<sbyte>(), -1, -1, new List<short> { 7 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 65, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(38, 38, new List<sbyte>(), -1, -1, new List<short> { 8 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 57, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(39, 39, new List<sbyte>(), -1, -1, new List<short> { 9 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 49, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(40, 40, new List<sbyte>(), -1, -1, new List<short> { 10 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 61, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(41, 41, new List<sbyte>(), -1, -1, new List<short> { 11 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 63, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(42, 42, new List<sbyte>(), -1, -1, new List<short> { 12 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 8 }, 3, 10)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 47, 181, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(43, 43, new List<sbyte>(), -1, -1, new List<short> { 13 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 67, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(44, 44, new List<sbyte>(), -1, -1, new List<short> { 14 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 55, 181, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(45, 45, new List<sbyte>(), -1, -1, new List<short> { 15 }, 1, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 10 }, 5, 7)
		}, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 17 }, 8)
		}, 73, 181, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 6, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(46, 46, new List<sbyte> { 3 }, -1, -1, new List<short> { 17 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(47, 47, new List<sbyte> { 3 }, -1, -1, new List<short> { 18 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(48, 48, new List<sbyte> { 3 }, -1, -1, new List<short> { 19 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(49, 49, new List<sbyte> { 3 }, -1, -1, new List<short> { 20 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(50, 50, new List<sbyte> { 3 }, -1, -1, new List<short> { 21 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(51, 51, new List<sbyte> { 3 }, -1, -1, new List<short> { 22 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(52, 52, new List<sbyte> { 3 }, -1, -1, new List<short> { 23 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(53, 53, new List<sbyte> { 3 }, -1, -1, new List<short> { 24 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 97 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 95 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 96 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(54, 54, new List<sbyte> { 3 }, -1, -1, new List<short> { 25 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(55, 55, new List<sbyte> { 3 }, -1, -1, new List<short> { 26 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(56, 56, new List<sbyte> { 3 }, -1, -1, new List<short> { 27 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(57, 57, new List<sbyte> { 3 }, -1, -1, new List<short> { 28 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(58, 58, new List<sbyte> { 3 }, -1, -1, new List<short> { 29 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(59, 59, new List<sbyte> { 3 }, -1, -1, new List<short> { 30 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new MonthlyActionsItem(60, 60, new List<sbyte> { 3 }, -1, -1, new List<short> { 31 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 21 }, 1)
		}, new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 19 }, 4, 8),
			new CharacterFilterRequirement(new int[1] { 20 }, 4, 8)
		}, 26, 182, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 24, 1));
		_dataArray.Add(new MonthlyActionsItem(61, 61, new List<sbyte> { 1 }, -1, -1, new List<short>(), 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[0], 3, 131, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: false, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(62, 62, new List<sbyte> { 3 }, -1, -1, new List<short> { 1, 2, 4, 6, 8, 10, 11, 12, 14, 15 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[5]
		{
			new CharacterFilterRequirement(new int[1] { 22 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 33 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 44 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 55 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 66 }, 2, 4)
		}, 14, 132, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(63, 63, new List<sbyte> { 3 }, -1, -1, new List<short> { 1, 2, 3, 7, 8, 10, 12, 13, 15 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[5]
		{
			new CharacterFilterRequirement(new int[1] { 23 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 34 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 45 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 56 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 67 }, 2, 4)
		}, 18, 132, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(64, 64, new List<sbyte> { 3 }, -1, -1, new List<short> { 5, 10, 15 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[5]
		{
			new CharacterFilterRequirement(new int[1] { 24 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 35 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 46 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 57 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 68 }, 2, 4)
		}, 16, 132, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(65, 65, new List<sbyte> { 3 }, -1, -1, new List<short> { 10, 13, 14, 15 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[5]
		{
			new CharacterFilterRequirement(new int[1] { 25 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 36 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 47 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 58 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 69 }, 2, 4)
		}, 11, 132, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(66, 66, new List<sbyte> { 3 }, -1, -1, new List<short> { 2, 4, 5, 7, 9, 12, 13 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[5]
		{
			new CharacterFilterRequirement(new int[1] { 26 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 37 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 48 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 59 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 70 }, 2, 4)
		}, 1, 132, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(67, 67, new List<sbyte> { 3 }, -1, -1, new List<short> { 5, 6, 9, 11, 10 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[5]
		{
			new CharacterFilterRequirement(new int[1] { 27 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 38 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 49 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 60 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 71 }, 2, 4)
		}, 0, 132, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(68, 68, new List<sbyte> { 3 }, -1, -1, new List<short> { 1, 6, 9 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[5]
		{
			new CharacterFilterRequirement(new int[1] { 28 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 39 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 50 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 61 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 72 }, 2, 4)
		}, 12, 132, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(69, 69, new List<sbyte> { 3 }, -1, -1, new List<short> { 2, 7, 11 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[5]
		{
			new CharacterFilterRequirement(new int[1] { 29 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 40 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 51 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 62 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 73 }, 2, 4)
		}, 13, 132, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(70, 70, new List<sbyte> { 3 }, -1, -1, new List<short> { 4, 12 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[5]
		{
			new CharacterFilterRequirement(new int[1] { 30 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 41 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 52 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 63 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 74 }, 2, 4)
		}, 15, 132, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(71, 71, new List<sbyte> { 3 }, -1, -1, new List<short> { 3, 9 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[5]
		{
			new CharacterFilterRequirement(new int[1] { 31 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 42 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 53 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 64 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 75 }, 2, 4)
		}, 17, 132, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(72, 72, new List<sbyte> { 3 }, -1, -1, new List<short> { 3, 8 }, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[5]
		{
			new CharacterFilterRequirement(new int[1] { 32 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 43 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 54 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 65 }, 2, 4),
			new CharacterFilterRequirement(new int[1] { 76 }, 2, 4)
		}, 2, 132, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(73, 73, new List<sbyte> { 4 }, -1, -1, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 77 }, 15, 20)
		}, 27, 134, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(74, 74, new List<sbyte> { 9 }, -1, -1, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 78 }, 10, 12),
			new CharacterFilterRequirement(new int[1] { 85 }, 5, 10)
		}, 21, 135, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(75, 75, new List<sbyte> { 9 }, -1, -1, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 79 }, 10, 12),
			new CharacterFilterRequirement(new int[1] { 85 }, 5, 10)
		}, 25, 135, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(76, 76, new List<sbyte> { 9 }, -1, -1, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 80 }, 10, 12),
			new CharacterFilterRequirement(new int[1] { 85 }, 5, 10)
		}, 24, 135, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(77, 77, new List<sbyte> { 9 }, -1, -1, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 81 }, 10, 12),
			new CharacterFilterRequirement(new int[1] { 85 }, 5, 10)
		}, 22, 135, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(78, 78, new List<sbyte> { 9 }, -1, -1, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 82 }, 10, 12),
			new CharacterFilterRequirement(new int[1] { 85 }, 5, 10)
		}, 23, 135, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(79, 79, new List<sbyte> { 9 }, -1, -1, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 83 }, 10, 12),
			new CharacterFilterRequirement(new int[1] { 85 }, 5, 10)
		}, 20, 135, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(80, 80, new List<sbyte> { 9 }, -1, -1, new List<short>
		{
			1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
			11, 12, 13, 14, 15
		}, 2, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 84 }, 10, 12),
			new CharacterFilterRequirement(new int[1] { 85 }, 5, 10)
		}, 19, 135, isEnemyNest: false, allowTemporaryMajorCharacter: false, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: false, canActionBeforehand: false, 2, 2, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(81, 81, new List<sbyte>(), -1, -1, new List<short> { 18 }, 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[0], new CharacterFilterRequirement[2]
		{
			new CharacterFilterRequirement(new int[1] { 86 }, 4, 4),
			new CharacterFilterRequirement(new int[1] { 87 }, 11, 11)
		}, 179, 278, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: true, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(82, 82, new List<sbyte>(), -1, -1, new List<short> { 23 }, 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 94 }, 1, 1)
		}, new CharacterFilterRequirement[4]
		{
			new CharacterFilterRequirement(new int[1] { 90 }, 3, 3),
			new CharacterFilterRequirement(new int[1] { 91 }, 3, 3),
			new CharacterFilterRequirement(new int[1] { 92 }, 3, 3),
			new CharacterFilterRequirement(new int[1] { 93 }, 1, 1)
		}, 170, 313, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(83, 83, new List<sbyte>(), -1, -1, new List<short> { 25 }, 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 102 }, 1, 1)
		}, new CharacterFilterRequirement[3]
		{
			new CharacterFilterRequirement(new int[1] { 101 }, 1, 1),
			new CharacterFilterRequirement(new int[1] { 100 }, 1, 1),
			new CharacterFilterRequirement(new int[1] { 99 }, 1, 1)
		}, 193, 366, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 0, 0, 0, 0));
		_dataArray.Add(new MonthlyActionsItem(84, 84, new List<sbyte>(), -1, -1, new List<short> { 21 }, 0, majorTargetMoveVisible: false, new CharacterFilterRequirement[1]
		{
			new CharacterFilterRequirement(new int[1] { 103 }, 1, 1)
		}, new CharacterFilterRequirement[4]
		{
			new CharacterFilterRequirement(new int[1] { 104 }, 1, 1),
			new CharacterFilterRequirement(new int[1] { 105 }, 1, 1),
			new CharacterFilterRequirement(new int[1] { 106 }, 10, 10),
			new CharacterFilterRequirement(new int[1] { 107 }, 1, 1)
		}, 196, 391, isEnemyNest: false, allowTemporaryMajorCharacter: true, allowTemporaryParticipateCharacter: true, willConvertTemporaryMajorCharacters: false, willConvertTemporaryParticipateCharacters: true, canActionBeforehand: true, 0, 0, 0, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MonthlyActionsItem>(85);
		CreateItems0();
		CreateItems1();
	}
}
