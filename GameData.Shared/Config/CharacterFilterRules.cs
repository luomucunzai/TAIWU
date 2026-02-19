using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells.Character;

namespace Config;

[Serializable]
public class CharacterFilterRules : ConfigData<CharacterFilterRulesItem, short>
{
	public static class DefKey
	{
		public const short BeggerFilter = 0;

		public const short Grade4Filter = 1;

		public const short Grade5Filter = 2;

		public const short BrideFilter1 = 3;

		public const short BrideFilter2 = 4;

		public const short MarriageMenFilter = 5;

		public const short AdultCharacterFilter = 6;

		public const short CompletelyInfectedFilter = 7;

		public const short WomanLocalFilter = 8;

		public const short ManLocalFilter = 9;

		public const short BeautyLocalFilter = 10;

		public const short Grade3Filter = 11;

		public const short MoodBadFilter = 12;

		public const short RandomMaleSuccessor = 13;

		public const short RandomFemaleSuccessor = 14;

		public const short WulinConferenceFilter = 15;

		public const short WulinConferencePrincipalFilter = 16;

		public const short FemaleMarriageMenFilter = 17;

		public const short FemaleMarriageMenLocalFilter = 18;

		public const short SectNormalCompetitionRolesLow = 19;

		public const short SectNormalCompetitionRolesMiddle = 20;

		public const short SectNormalCompetitionRolesHigh = 21;

		public const short CityRolesGoodAtFistAndPalm0 = 22;

		public const short CityRolesGoodAtFinger0 = 23;

		public const short CityRolesGoodAtLeg0 = 24;

		public const short CityRolesGoodAtThrow0 = 25;

		public const short CityRolesGoodAtSword0 = 26;

		public const short CityRolesGoodAtBlade0 = 27;

		public const short CityRolesGoodAtPolearm0 = 28;

		public const short CityRolesGoodAtSpecial0 = 29;

		public const short CityRolesGoodAtWhip0 = 30;

		public const short CityRolesGoodAtControllableShot0 = 31;

		public const short CityRolesGoodAtCombatMusic0 = 32;

		public const short CityRolesGoodAtFistAndPalm1 = 33;

		public const short CityRolesGoodAtFinger1 = 34;

		public const short CityRolesGoodAtLeg1 = 35;

		public const short CityRolesGoodAtThrow1 = 36;

		public const short CityRolesGoodAtSword1 = 37;

		public const short CityRolesGoodAtBlade1 = 38;

		public const short CityRolesGoodAtPolearm1 = 39;

		public const short CityRolesGoodAtSpecial1 = 40;

		public const short CityRolesGoodAtWhip1 = 41;

		public const short CityRolesGoodAtControllableShot1 = 42;

		public const short CityRolesGoodAtCombatMusic1 = 43;

		public const short CityRolesGoodAtFistAndPalm2 = 44;

		public const short CityRolesGoodAtFinger2 = 45;

		public const short CityRolesGoodAtLeg2 = 46;

		public const short CityRolesGoodAtThrow2 = 47;

		public const short CityRolesGoodAtSword2 = 48;

		public const short CityRolesGoodAtBlade2 = 49;

		public const short CityRolesGoodAtPolearm2 = 50;

		public const short CityRolesGoodAtSpecial2 = 51;

		public const short CityRolesGoodAtWhip2 = 52;

		public const short CityRolesGoodAtControllableShot2 = 53;

		public const short CityRolesGoodAtCombatMusic2 = 54;

		public const short CityRolesGoodAtFistAndPalm3 = 55;

		public const short CityRolesGoodAtFinger3 = 56;

		public const short CityRolesGoodAtLeg3 = 57;

		public const short CityRolesGoodAtThrow3 = 58;

		public const short CityRolesGoodAtSword3 = 59;

		public const short CityRolesGoodAtBlade3 = 60;

		public const short CityRolesGoodAtPolearm3 = 61;

		public const short CityRolesGoodAtSpecial3 = 62;

		public const short CityRolesGoodAtWhip3 = 63;

		public const short CityRolesGoodAtControllableShot3 = 64;

		public const short CityRolesGoodAtCombatMusic3 = 65;

		public const short CityRolesGoodAtFistAndPalm4 = 66;

		public const short CityRolesGoodAtFinger4 = 67;

		public const short CityRolesGoodAtLeg4 = 68;

		public const short CityRolesGoodAtThrow4 = 69;

		public const short CityRolesGoodAtSword4 = 70;

		public const short CityRolesGoodAtBlade4 = 71;

		public const short CityRolesGoodAtPolearm4 = 72;

		public const short CityRolesGoodAtSpecial4 = 73;

		public const short CityRolesGoodAtWhip4 = 74;

		public const short CityRolesGoodAtControllableShot4 = 75;

		public const short CityRolesGoodAtCombatMusic4 = 76;

		public const short CriketRichRoles = 77;

		public const short CityRolesGoodAtForging = 78;

		public const short CityRolesGoodAtWoodworking = 79;

		public const short CityRolesGoodAtWeaving = 80;

		public const short CityRolesGoodAtJade = 81;

		public const short CityRolesGoodAtMedicine = 82;

		public const short CityRolesGoodAtToxicology = 83;

		public const short CityRolesGoodAtCooking = 84;

		public const short CityLifeScoreRoles = 85;

		public const short SectMainStoryEmeiTwoPartOne = 86;

		public const short SectMainStoryEmeiTwoPartTwo = 87;

		public const short SectMainStoryRanshanGrade1 = 90;

		public const short SectMainStoryRanshanGrade2 = 91;

		public const short SectMainStoryRanshanGrade3 = 92;

		public const short SectMainStoryRanshanGrade4 = 93;

		public const short SectMainStoryRanshanGrade8 = 94;

		public const short SectNormalCompetitionRolesLowWithHair = 95;

		public const short SectNormalCompetitionRolesMiddleWithHair = 96;

		public const short SectNormalCompetitionRolesHighWithHair = 97;

		public const short SectMainStoryZhujianGrade4 = 98;

		public const short SectMainStoryZhujianGrade2 = 99;

		public const short SectMainStoryZhujianGrade5 = 100;

		public const short SectMainStoryZhujianGrade3 = 101;

		public const short SectMainStoryZhujianGrade8 = 102;

		public const short SectMainStoryYuanshanGrade8 = 103;

		public const short SectMainStoryYuanshanGrade7 = 104;

		public const short SectMainStoryYuanshanGrade6 = 105;

		public const short SectMainStoryYuanshanGradeMidLow = 106;

		public const short SectMainStoryYuanshanXiangshuInfected = 107;
	}

	public static class DefValue
	{
		public static CharacterFilterRulesItem BeggerFilter => Instance[(short)0];

		public static CharacterFilterRulesItem Grade4Filter => Instance[(short)1];

		public static CharacterFilterRulesItem Grade5Filter => Instance[(short)2];

		public static CharacterFilterRulesItem BrideFilter1 => Instance[(short)3];

		public static CharacterFilterRulesItem BrideFilter2 => Instance[(short)4];

		public static CharacterFilterRulesItem MarriageMenFilter => Instance[(short)5];

		public static CharacterFilterRulesItem AdultCharacterFilter => Instance[(short)6];

		public static CharacterFilterRulesItem CompletelyInfectedFilter => Instance[(short)7];

		public static CharacterFilterRulesItem WomanLocalFilter => Instance[(short)8];

		public static CharacterFilterRulesItem ManLocalFilter => Instance[(short)9];

		public static CharacterFilterRulesItem BeautyLocalFilter => Instance[(short)10];

		public static CharacterFilterRulesItem Grade3Filter => Instance[(short)11];

		public static CharacterFilterRulesItem MoodBadFilter => Instance[(short)12];

		public static CharacterFilterRulesItem RandomMaleSuccessor => Instance[(short)13];

		public static CharacterFilterRulesItem RandomFemaleSuccessor => Instance[(short)14];

		public static CharacterFilterRulesItem WulinConferenceFilter => Instance[(short)15];

		public static CharacterFilterRulesItem WulinConferencePrincipalFilter => Instance[(short)16];

		public static CharacterFilterRulesItem FemaleMarriageMenFilter => Instance[(short)17];

		public static CharacterFilterRulesItem FemaleMarriageMenLocalFilter => Instance[(short)18];

		public static CharacterFilterRulesItem SectNormalCompetitionRolesLow => Instance[(short)19];

		public static CharacterFilterRulesItem SectNormalCompetitionRolesMiddle => Instance[(short)20];

		public static CharacterFilterRulesItem SectNormalCompetitionRolesHigh => Instance[(short)21];

		public static CharacterFilterRulesItem CityRolesGoodAtFistAndPalm0 => Instance[(short)22];

		public static CharacterFilterRulesItem CityRolesGoodAtFinger0 => Instance[(short)23];

		public static CharacterFilterRulesItem CityRolesGoodAtLeg0 => Instance[(short)24];

		public static CharacterFilterRulesItem CityRolesGoodAtThrow0 => Instance[(short)25];

		public static CharacterFilterRulesItem CityRolesGoodAtSword0 => Instance[(short)26];

		public static CharacterFilterRulesItem CityRolesGoodAtBlade0 => Instance[(short)27];

		public static CharacterFilterRulesItem CityRolesGoodAtPolearm0 => Instance[(short)28];

		public static CharacterFilterRulesItem CityRolesGoodAtSpecial0 => Instance[(short)29];

		public static CharacterFilterRulesItem CityRolesGoodAtWhip0 => Instance[(short)30];

		public static CharacterFilterRulesItem CityRolesGoodAtControllableShot0 => Instance[(short)31];

		public static CharacterFilterRulesItem CityRolesGoodAtCombatMusic0 => Instance[(short)32];

		public static CharacterFilterRulesItem CityRolesGoodAtFistAndPalm1 => Instance[(short)33];

		public static CharacterFilterRulesItem CityRolesGoodAtFinger1 => Instance[(short)34];

		public static CharacterFilterRulesItem CityRolesGoodAtLeg1 => Instance[(short)35];

		public static CharacterFilterRulesItem CityRolesGoodAtThrow1 => Instance[(short)36];

		public static CharacterFilterRulesItem CityRolesGoodAtSword1 => Instance[(short)37];

		public static CharacterFilterRulesItem CityRolesGoodAtBlade1 => Instance[(short)38];

		public static CharacterFilterRulesItem CityRolesGoodAtPolearm1 => Instance[(short)39];

		public static CharacterFilterRulesItem CityRolesGoodAtSpecial1 => Instance[(short)40];

		public static CharacterFilterRulesItem CityRolesGoodAtWhip1 => Instance[(short)41];

		public static CharacterFilterRulesItem CityRolesGoodAtControllableShot1 => Instance[(short)42];

		public static CharacterFilterRulesItem CityRolesGoodAtCombatMusic1 => Instance[(short)43];

		public static CharacterFilterRulesItem CityRolesGoodAtFistAndPalm2 => Instance[(short)44];

		public static CharacterFilterRulesItem CityRolesGoodAtFinger2 => Instance[(short)45];

		public static CharacterFilterRulesItem CityRolesGoodAtLeg2 => Instance[(short)46];

		public static CharacterFilterRulesItem CityRolesGoodAtThrow2 => Instance[(short)47];

		public static CharacterFilterRulesItem CityRolesGoodAtSword2 => Instance[(short)48];

		public static CharacterFilterRulesItem CityRolesGoodAtBlade2 => Instance[(short)49];

		public static CharacterFilterRulesItem CityRolesGoodAtPolearm2 => Instance[(short)50];

		public static CharacterFilterRulesItem CityRolesGoodAtSpecial2 => Instance[(short)51];

		public static CharacterFilterRulesItem CityRolesGoodAtWhip2 => Instance[(short)52];

		public static CharacterFilterRulesItem CityRolesGoodAtControllableShot2 => Instance[(short)53];

		public static CharacterFilterRulesItem CityRolesGoodAtCombatMusic2 => Instance[(short)54];

		public static CharacterFilterRulesItem CityRolesGoodAtFistAndPalm3 => Instance[(short)55];

		public static CharacterFilterRulesItem CityRolesGoodAtFinger3 => Instance[(short)56];

		public static CharacterFilterRulesItem CityRolesGoodAtLeg3 => Instance[(short)57];

		public static CharacterFilterRulesItem CityRolesGoodAtThrow3 => Instance[(short)58];

		public static CharacterFilterRulesItem CityRolesGoodAtSword3 => Instance[(short)59];

		public static CharacterFilterRulesItem CityRolesGoodAtBlade3 => Instance[(short)60];

		public static CharacterFilterRulesItem CityRolesGoodAtPolearm3 => Instance[(short)61];

		public static CharacterFilterRulesItem CityRolesGoodAtSpecial3 => Instance[(short)62];

		public static CharacterFilterRulesItem CityRolesGoodAtWhip3 => Instance[(short)63];

		public static CharacterFilterRulesItem CityRolesGoodAtControllableShot3 => Instance[(short)64];

		public static CharacterFilterRulesItem CityRolesGoodAtCombatMusic3 => Instance[(short)65];

		public static CharacterFilterRulesItem CityRolesGoodAtFistAndPalm4 => Instance[(short)66];

		public static CharacterFilterRulesItem CityRolesGoodAtFinger4 => Instance[(short)67];

		public static CharacterFilterRulesItem CityRolesGoodAtLeg4 => Instance[(short)68];

		public static CharacterFilterRulesItem CityRolesGoodAtThrow4 => Instance[(short)69];

		public static CharacterFilterRulesItem CityRolesGoodAtSword4 => Instance[(short)70];

		public static CharacterFilterRulesItem CityRolesGoodAtBlade4 => Instance[(short)71];

		public static CharacterFilterRulesItem CityRolesGoodAtPolearm4 => Instance[(short)72];

		public static CharacterFilterRulesItem CityRolesGoodAtSpecial4 => Instance[(short)73];

		public static CharacterFilterRulesItem CityRolesGoodAtWhip4 => Instance[(short)74];

		public static CharacterFilterRulesItem CityRolesGoodAtControllableShot4 => Instance[(short)75];

		public static CharacterFilterRulesItem CityRolesGoodAtCombatMusic4 => Instance[(short)76];

		public static CharacterFilterRulesItem CriketRichRoles => Instance[(short)77];

		public static CharacterFilterRulesItem CityRolesGoodAtForging => Instance[(short)78];

		public static CharacterFilterRulesItem CityRolesGoodAtWoodworking => Instance[(short)79];

		public static CharacterFilterRulesItem CityRolesGoodAtWeaving => Instance[(short)80];

		public static CharacterFilterRulesItem CityRolesGoodAtJade => Instance[(short)81];

		public static CharacterFilterRulesItem CityRolesGoodAtMedicine => Instance[(short)82];

		public static CharacterFilterRulesItem CityRolesGoodAtToxicology => Instance[(short)83];

		public static CharacterFilterRulesItem CityRolesGoodAtCooking => Instance[(short)84];

		public static CharacterFilterRulesItem CityLifeScoreRoles => Instance[(short)85];

		public static CharacterFilterRulesItem SectMainStoryEmeiTwoPartOne => Instance[(short)86];

		public static CharacterFilterRulesItem SectMainStoryEmeiTwoPartTwo => Instance[(short)87];

		public static CharacterFilterRulesItem SectMainStoryRanshanGrade1 => Instance[(short)90];

		public static CharacterFilterRulesItem SectMainStoryRanshanGrade2 => Instance[(short)91];

		public static CharacterFilterRulesItem SectMainStoryRanshanGrade3 => Instance[(short)92];

		public static CharacterFilterRulesItem SectMainStoryRanshanGrade4 => Instance[(short)93];

		public static CharacterFilterRulesItem SectMainStoryRanshanGrade8 => Instance[(short)94];

		public static CharacterFilterRulesItem SectNormalCompetitionRolesLowWithHair => Instance[(short)95];

		public static CharacterFilterRulesItem SectNormalCompetitionRolesMiddleWithHair => Instance[(short)96];

		public static CharacterFilterRulesItem SectNormalCompetitionRolesHighWithHair => Instance[(short)97];

		public static CharacterFilterRulesItem SectMainStoryZhujianGrade4 => Instance[(short)98];

		public static CharacterFilterRulesItem SectMainStoryZhujianGrade2 => Instance[(short)99];

		public static CharacterFilterRulesItem SectMainStoryZhujianGrade5 => Instance[(short)100];

		public static CharacterFilterRulesItem SectMainStoryZhujianGrade3 => Instance[(short)101];

		public static CharacterFilterRulesItem SectMainStoryZhujianGrade8 => Instance[(short)102];

		public static CharacterFilterRulesItem SectMainStoryYuanshanGrade8 => Instance[(short)103];

		public static CharacterFilterRulesItem SectMainStoryYuanshanGrade7 => Instance[(short)104];

		public static CharacterFilterRulesItem SectMainStoryYuanshanGrade6 => Instance[(short)105];

		public static CharacterFilterRulesItem SectMainStoryYuanshanGradeMidLow => Instance[(short)106];

		public static CharacterFilterRulesItem SectMainStoryYuanshanXiangshuInfected => Instance[(short)107];
	}

	public static CharacterFilterRules Instance = new CharacterFilterRules();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId" };

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
		_dataArray.Add(new CharacterFilterRulesItem(0, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 16, 90),
			new CharacterFilterElement(new int[1] { 2 }, 1, 1),
			new CharacterFilterElement(new int[1] { 9 }, 2, 4)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(1, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 5, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(2, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 4, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(3, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 5 }, 800, 900),
			new CharacterFilterElement(new int[1], 6, 7),
			new CharacterFilterElement(new int[1] { 2 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 18, 20),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[1] { 9 }, 1, 1),
			new CharacterFilterElement(new int[1] { 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 11 }, 0, 0)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(4, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 5 }, 0, 100),
			new CharacterFilterElement(new int[1], 6, 7),
			new CharacterFilterElement(new int[1] { 2 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 18, 20),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[1] { 9 }, 1, 1),
			new CharacterFilterElement(new int[1] { 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 11 }, 0, 0)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(5, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 2 }, 1, 1),
			new CharacterFilterElement(new int[1], 4, 7),
			new CharacterFilterElement(new int[1] { 4 }, 16, 60),
			new CharacterFilterElement(new int[1] { 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 9 }, 0, 2),
			new CharacterFilterElement(new int[1] { 11 }, 0, 0),
			new CharacterFilterElement(new int[1] { 12 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(6, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 1 }, 16, 90)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(7, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 8 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(8, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 4, 7),
			new CharacterFilterElement(new int[1] { 2 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 16, 30),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[1] { 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 11 }, 0, 0),
			new CharacterFilterElement(new int[1] { 9 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(9, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 4, 7),
			new CharacterFilterElement(new int[1] { 2 }, 1, 1),
			new CharacterFilterElement(new int[1] { 4 }, 16, 30),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[1] { 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 11 }, 0, 0),
			new CharacterFilterElement(new int[1] { 12 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(10, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 4, 7),
			new CharacterFilterElement(new int[1] { 2 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 16, 30),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[1] { 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 5 }, 400, 900),
			new CharacterFilterElement(new int[1] { 11 }, 0, 0),
			new CharacterFilterElement(new int[1] { 9 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(11, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 6, 6),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 3, 90)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(12, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 10 }, -119, -30),
			new CharacterFilterElement(new int[1] { 4 }, 3, 90),
			new CharacterFilterElement(new int[1] { 15 }, 0, 0)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(13, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 0, 2),
			new CharacterFilterElement(new int[1] { 4 }, 12, 20),
			new CharacterFilterElement(new int[1] { 2 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(14, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 0, 2),
			new CharacterFilterElement(new int[1] { 4 }, 12, 20),
			new CharacterFilterElement(new int[1] { 2 }, 0, 0)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(15, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 15 }, 0, 0),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, 1, 15),
			new CharacterFilterElement(new int[1] { 13 }, 1, 5)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(16, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 8, 8),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, 1, 15),
			new CharacterFilterElement(new int[1] { 14 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(17, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 2 }, 1, 1),
			new CharacterFilterElement(new int[2] { 0, 1 }, -8, 1),
			new CharacterFilterElement(new int[1] { 4 }, 16, 50),
			new CharacterFilterElement(new int[1] { 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 9 }, 0, 2),
			new CharacterFilterElement(new int[1] { 11 }, 0, 0),
			new CharacterFilterElement(new int[1] { 12 }, 1, 1),
			new CharacterFilterElement(new int[1] { 5 }, 500, 900),
			new CharacterFilterElement(new int[1] { 15 }, 0, 0)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(18, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 2 }, 1, 1),
			new CharacterFilterElement(new int[2] { 0, 1 }, -8, 1),
			new CharacterFilterElement(new int[1] { 4 }, 16, 30),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[1] { 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 11 }, 0, 0),
			new CharacterFilterElement(new int[1] { 12 }, 1, 1),
			new CharacterFilterElement(new int[1] { 15 }, 0, 0)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(19, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[2] { 0, 1 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 17, 90)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(20, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[2] { 0, 1 }, 1, 1),
			new CharacterFilterElement(new int[1] { 4 }, 17, 90)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(21, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[2] { 0, 1 }, 2, 2),
			new CharacterFilterElement(new int[1] { 4 }, 17, 90)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(22, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 3 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(23, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 4 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(24, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 5 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(25, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 6 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(26, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(27, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 8 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(28, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 9 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(29, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 10 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(30, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 11 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(31, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 12 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(32, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 13 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(33, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 3 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(34, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 4 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(35, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 5 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(36, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 6 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(37, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(38, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 8 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(39, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 9 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(40, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 10 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(41, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 11 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(42, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 12 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(43, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 13 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 6, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(44, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 3 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 8, 9),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(45, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 4 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 8, 9),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(46, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 5 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 8, 9),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(47, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 6 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 8, 9),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(48, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 8, 9),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(49, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 8 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 8, 9),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(50, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 9 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 8, 9),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(51, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 10 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 8, 9),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(52, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 11 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 8, 9),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(53, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 12 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 8, 9),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(54, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 13 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 8, 9),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(55, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 3 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 10, 11),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(56, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 4 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 10, 11),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(57, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 5 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 10, 11),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(58, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 6 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 10, 11),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(59, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 10, 11),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new CharacterFilterRulesItem(60, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 8 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 10, 11),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(61, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 9 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 10, 11),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(62, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 10 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 10, 11),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(63, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 11 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 10, 11),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(64, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 12 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 10, 11),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(65, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 13 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 10, 11),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(66, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 3 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 12, 13),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(67, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 4 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 12, 13),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(68, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 5 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 12, 13),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(69, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 6 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 12, 13),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(70, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 7 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 12, 13),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(71, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 8 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 12, 13),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(72, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 9 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 12, 13),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(73, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 10 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 12, 13),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(74, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 11 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 12, 13),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(75, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 12 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 12, 13),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(76, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 20, 13 }, 1, 1),
			new CharacterFilterElement(new int[1] { 16 }, 12, 13),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1], 0, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(77, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 21, 6 }, 5000, 99999999),
			new CharacterFilterElement(new int[1], 4, 7),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1] { 9 }, 0, 2)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(78, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 19, 6 }, 1, 1),
			new CharacterFilterElement(new int[1], 1, 5),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1] { 9 }, 0, 2)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(79, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 19, 7 }, 1, 1),
			new CharacterFilterElement(new int[1], 1, 5),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1] { 9 }, 0, 2)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(80, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 19, 10 }, 1, 1),
			new CharacterFilterElement(new int[1], 1, 5),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1] { 9 }, 0, 2)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(81, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 19, 11 }, 1, 1),
			new CharacterFilterElement(new int[1], 1, 5),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1] { 9 }, 0, 2)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(82, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 19, 8 }, 1, 1),
			new CharacterFilterElement(new int[1], 1, 5),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1] { 9 }, 0, 2)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(83, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 19, 9 }, 1, 1),
			new CharacterFilterElement(new int[1], 1, 5),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1] { 9 }, 0, 2)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(84, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[2] { 19, 14 }, 1, 1),
			new CharacterFilterElement(new int[1], 1, 5),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90),
			new CharacterFilterElement(new int[1] { 9 }, 0, 2)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(85, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 9 }, 0, 2),
			new CharacterFilterElement(new int[1] { 4 }, 3, 9999)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(86, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 6 }, 2, 2),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1], 7, 7),
			new CharacterFilterElement(new int[1] { 4 }, 16, 90)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(87, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 6 }, 2, 2),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1], 1, 6),
			new CharacterFilterElement(new int[1] { 4 }, 16, 90)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(88, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 4, 4),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[1] { 4 }, 10, 90)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(89, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 5, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 3, 90)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(90, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 4, 4),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 16, 40),
			new CharacterFilterElement(new int[1] { 6 }, 7, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(91, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 3, 3),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 16, 40),
			new CharacterFilterElement(new int[1] { 6 }, 7, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(92, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 2, 2),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 16, 40),
			new CharacterFilterElement(new int[1] { 6 }, 7, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(93, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 1, 1),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 16, 40),
			new CharacterFilterElement(new int[1] { 6 }, 7, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(94, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 8, 8),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, 7, 7)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(95, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[2] { 0, 1 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 17, 90),
			new CharacterFilterElement(new int[1] { 22 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(96, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[2] { 0, 1 }, 1, 1),
			new CharacterFilterElement(new int[1] { 4 }, 17, 90),
			new CharacterFilterElement(new int[1] { 22 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(97, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 6 }, -1, -1),
			new CharacterFilterElement(new int[2] { 0, 1 }, 2, 2),
			new CharacterFilterElement(new int[1] { 4 }, 17, 90),
			new CharacterFilterElement(new int[1] { 22 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(98, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 4, 4),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 60, 90),
			new CharacterFilterElement(new int[1] { 6 }, 9, 9),
			new CharacterFilterElement(new int[1] { 2 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(99, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 2, 2),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 60, 90),
			new CharacterFilterElement(new int[1] { 6 }, 9, 9),
			new CharacterFilterElement(new int[1] { 2 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(100, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 5, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 60, 90),
			new CharacterFilterElement(new int[1] { 6 }, 9, 9),
			new CharacterFilterElement(new int[1] { 2 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(101, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 3, 3),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 60, 90),
			new CharacterFilterElement(new int[1] { 6 }, 9, 9),
			new CharacterFilterElement(new int[1] { 2 }, 1, 1)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(102, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 8, 8),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 18, 90),
			new CharacterFilterElement(new int[1] { 6 }, 9, 9)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(103, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 8, 8),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 18, 90),
			new CharacterFilterElement(new int[1] { 6 }, 5, 5)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(104, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 7, 7),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 18, 90),
			new CharacterFilterElement(new int[1] { 6 }, 5, 5)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(105, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 6, 6),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 18, 90),
			new CharacterFilterElement(new int[1] { 6 }, 5, 5)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(106, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1], 0, 5),
			new CharacterFilterElement(new int[1] { 9 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 18, 90),
			new CharacterFilterElement(new int[1] { 6 }, 5, 5)
		}));
		_dataArray.Add(new CharacterFilterRulesItem(107, new List<CharacterFilterElement>
		{
			new CharacterFilterElement(new int[1] { 8 }, 1, 1),
			new CharacterFilterElement(new int[2] { 0, 1 }, 0, 0),
			new CharacterFilterElement(new int[1] { 4 }, 18, 90)
		}));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CharacterFilterRulesItem>(108);
		CreateItems0();
		CreateItems1();
	}
}
