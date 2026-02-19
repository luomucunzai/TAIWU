using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SecretInformation : ConfigData<SecretInformationItem, short>
{
	public static class DefKey
	{
		public const short Die = 0;

		public const short KillInPublic = 1;

		public const short KidnapInPublic = 2;

		public const short KillForPunishment = 3;

		public const short KidnapForPunishment = 4;

		public const short UnexpectedResourceGain = 5;

		public const short UnexpectedItemGain = 6;

		public const short UnexpectedSkillBookGain = 7;

		public const short UnexpectedCure = 8;

		public const short UnexpectedResourceLose = 9;

		public const short UnexpectedItemLose = 10;

		public const short UnexpectedSkillBookLose = 11;

		public const short UnexpectedHarm = 12;

		public const short LifeSkillBattleWin = 13;

		public const short CricketBattleWin = 14;

		public const short MajorVictoryInCombat = 15;

		public const short MinorVictoryInCombat = 16;

		public const short Mourn = 17;

		public const short OfferProtection = 18;

		public const short LoseFetus = 19;

		public const short LoseFetus2 = 20;

		public const short GiveBirthToChild = 21;

		public const short GiveBirthToChild2 = 22;

		public const short AbandonChild = 23;

		public const short ReleaseKidnappedCharacter = 24;

		public const short RescueKidnappedCharacter = 25;

		public const short KidnappedCharacterEscaped = 26;

		public const short ReadBookFail = 27;

		public const short BreakoutFail = 28;

		public const short LoseOverloadingItem = 29;

		public const short SeverEnemy = 30;

		public const short BecomeEnemy = 31;

		public const short BecomeFriend = 32;

		public const short SeverFriend = 33;

		public const short BecomeLover = 34;

		public const short BreakupWithLover = 35;

		public const short BecomeHusbandAndWife = 36;

		public const short BecomeSwornBrothersAndSisters = 37;

		public const short SeverSwornBrothersAndSisters = 38;

		public const short GetAdopted = 39;

		public const short AdoptChild = 40;

		public const short GivingResource = 41;

		public const short GiveItem = 42;

		public const short BuildGrave = 43;

		public const short Cure = 44;

		public const short RepairItem = 45;

		public const short InstructOnLifeSkill = 46;

		public const short InstructOnCombatSkill = 47;

		public const short AcceptRequestHealInjury = 48;

		public const short AcceptRequestDetoxPoison = 49;

		public const short AcceptRequestIncreaseHealth = 50;

		public const short AcceptRequestRestoreDisorderOfQi = 51;

		public const short AcceptRequestIncreaseNeili = 52;

		public const short AcceptRequestKillWug = 53;

		public const short AcceptRequestFood = 54;

		public const short AcceptRequestTeaWine = 55;

		public const short AcceptRequestResource = 56;

		public const short AcceptRequestItem = 57;

		public const short AcceptRequestDrinking = 58;

		public const short AcceptRequestGivingMoney = 59;

		public const short AcceptRequestInstructionOnReading = 60;

		public const short AcceptRequestInstructionOnBreakout = 61;

		public const short AcceptRequestRepairItem = 62;

		public const short AcceptRequestAddPoisonToItem = 63;

		public const short AcceptRequestInstructionOnLifeSkill = 64;

		public const short AcceptRequestInstructionOnCombatSkill = 65;

		public const short RehaircutSuccess = 66;

		public const short RehaircutIncompleted = 67;

		public const short RehaircutFail = 68;

		public const short RefuseRequestHealInjury = 69;

		public const short RefuseRequestDetoxPoison = 70;

		public const short RefuseRequestIncreaseHealth = 71;

		public const short RefuseRequestRestoreDisorderOfQi = 72;

		public const short RefuseRequestIncreaseNeili = 73;

		public const short RefuseRequestKillWug = 74;

		public const short RefuseRequestFood = 75;

		public const short RefuseRequestTeaWine = 76;

		public const short RefuseRequestResource = 77;

		public const short RefuseRequestItem = 78;

		public const short RefuseRequestDrinking = 79;

		public const short RefuseRequestGivingMoney = 80;

		public const short RefuseRequestInstructionOnReading = 81;

		public const short RefuseRequestInstructionOnBreakout = 82;

		public const short RefuseRequestRepairItem = 83;

		public const short RefuseRequestAddPoisonToItem = 84;

		public const short RefuseRequestInstructionOnLifeSkill = 85;

		public const short RefuseRequestInstructionOnCombatSkill = 86;

		public const short RobGraveResource = 87;

		public const short StealResource = 88;

		public const short ScamResource = 89;

		public const short RobResource = 90;

		public const short RobGraveItem = 91;

		public const short StealItem = 92;

		public const short ScamItem = 93;

		public const short RobItem = 94;

		public const short KillInPrivate = 95;

		public const short KidnapInPrivate = 96;

		public const short PoisonEnemy = 97;

		public const short PlotHarmEnemy = 98;

		public const short StealLifeSkill = 99;

		public const short ScamLifeSkill = 100;

		public const short StealCombatSkill = 101;

		public const short ScamCombatSkill = 102;

		public const short AddPoisonToItem = 103;

		public const short MonkBreakRule = 104;

		public const short MakeLoveIllegal = 105;

		public const short Rape = 106;

		public const short LoseFetusFatherUnknown = 107;

		public const short GiveBirthToChildFatherUnknown = 108;

		public const short DatingWithCrush = 109;

		public const short ForcingSilence = 110;

		public const short RetrieveChild = 111;

		public const short SolveScripture1 = 112;

		public const short SolveScripture2 = 113;

		public const short SolveScripture3 = 114;

		public const short SolveScripture4 = 115;

		public const short PrisonBreak = 116;
	}

	public static class DefValue
	{
		public static SecretInformationItem Die => Instance[(short)0];

		public static SecretInformationItem KillInPublic => Instance[(short)1];

		public static SecretInformationItem KidnapInPublic => Instance[(short)2];

		public static SecretInformationItem KillForPunishment => Instance[(short)3];

		public static SecretInformationItem KidnapForPunishment => Instance[(short)4];

		public static SecretInformationItem UnexpectedResourceGain => Instance[(short)5];

		public static SecretInformationItem UnexpectedItemGain => Instance[(short)6];

		public static SecretInformationItem UnexpectedSkillBookGain => Instance[(short)7];

		public static SecretInformationItem UnexpectedCure => Instance[(short)8];

		public static SecretInformationItem UnexpectedResourceLose => Instance[(short)9];

		public static SecretInformationItem UnexpectedItemLose => Instance[(short)10];

		public static SecretInformationItem UnexpectedSkillBookLose => Instance[(short)11];

		public static SecretInformationItem UnexpectedHarm => Instance[(short)12];

		public static SecretInformationItem LifeSkillBattleWin => Instance[(short)13];

		public static SecretInformationItem CricketBattleWin => Instance[(short)14];

		public static SecretInformationItem MajorVictoryInCombat => Instance[(short)15];

		public static SecretInformationItem MinorVictoryInCombat => Instance[(short)16];

		public static SecretInformationItem Mourn => Instance[(short)17];

		public static SecretInformationItem OfferProtection => Instance[(short)18];

		public static SecretInformationItem LoseFetus => Instance[(short)19];

		public static SecretInformationItem LoseFetus2 => Instance[(short)20];

		public static SecretInformationItem GiveBirthToChild => Instance[(short)21];

		public static SecretInformationItem GiveBirthToChild2 => Instance[(short)22];

		public static SecretInformationItem AbandonChild => Instance[(short)23];

		public static SecretInformationItem ReleaseKidnappedCharacter => Instance[(short)24];

		public static SecretInformationItem RescueKidnappedCharacter => Instance[(short)25];

		public static SecretInformationItem KidnappedCharacterEscaped => Instance[(short)26];

		public static SecretInformationItem ReadBookFail => Instance[(short)27];

		public static SecretInformationItem BreakoutFail => Instance[(short)28];

		public static SecretInformationItem LoseOverloadingItem => Instance[(short)29];

		public static SecretInformationItem SeverEnemy => Instance[(short)30];

		public static SecretInformationItem BecomeEnemy => Instance[(short)31];

		public static SecretInformationItem BecomeFriend => Instance[(short)32];

		public static SecretInformationItem SeverFriend => Instance[(short)33];

		public static SecretInformationItem BecomeLover => Instance[(short)34];

		public static SecretInformationItem BreakupWithLover => Instance[(short)35];

		public static SecretInformationItem BecomeHusbandAndWife => Instance[(short)36];

		public static SecretInformationItem BecomeSwornBrothersAndSisters => Instance[(short)37];

		public static SecretInformationItem SeverSwornBrothersAndSisters => Instance[(short)38];

		public static SecretInformationItem GetAdopted => Instance[(short)39];

		public static SecretInformationItem AdoptChild => Instance[(short)40];

		public static SecretInformationItem GivingResource => Instance[(short)41];

		public static SecretInformationItem GiveItem => Instance[(short)42];

		public static SecretInformationItem BuildGrave => Instance[(short)43];

		public static SecretInformationItem Cure => Instance[(short)44];

		public static SecretInformationItem RepairItem => Instance[(short)45];

		public static SecretInformationItem InstructOnLifeSkill => Instance[(short)46];

		public static SecretInformationItem InstructOnCombatSkill => Instance[(short)47];

		public static SecretInformationItem AcceptRequestHealInjury => Instance[(short)48];

		public static SecretInformationItem AcceptRequestDetoxPoison => Instance[(short)49];

		public static SecretInformationItem AcceptRequestIncreaseHealth => Instance[(short)50];

		public static SecretInformationItem AcceptRequestRestoreDisorderOfQi => Instance[(short)51];

		public static SecretInformationItem AcceptRequestIncreaseNeili => Instance[(short)52];

		public static SecretInformationItem AcceptRequestKillWug => Instance[(short)53];

		public static SecretInformationItem AcceptRequestFood => Instance[(short)54];

		public static SecretInformationItem AcceptRequestTeaWine => Instance[(short)55];

		public static SecretInformationItem AcceptRequestResource => Instance[(short)56];

		public static SecretInformationItem AcceptRequestItem => Instance[(short)57];

		public static SecretInformationItem AcceptRequestDrinking => Instance[(short)58];

		public static SecretInformationItem AcceptRequestGivingMoney => Instance[(short)59];

		public static SecretInformationItem AcceptRequestInstructionOnReading => Instance[(short)60];

		public static SecretInformationItem AcceptRequestInstructionOnBreakout => Instance[(short)61];

		public static SecretInformationItem AcceptRequestRepairItem => Instance[(short)62];

		public static SecretInformationItem AcceptRequestAddPoisonToItem => Instance[(short)63];

		public static SecretInformationItem AcceptRequestInstructionOnLifeSkill => Instance[(short)64];

		public static SecretInformationItem AcceptRequestInstructionOnCombatSkill => Instance[(short)65];

		public static SecretInformationItem RehaircutSuccess => Instance[(short)66];

		public static SecretInformationItem RehaircutIncompleted => Instance[(short)67];

		public static SecretInformationItem RehaircutFail => Instance[(short)68];

		public static SecretInformationItem RefuseRequestHealInjury => Instance[(short)69];

		public static SecretInformationItem RefuseRequestDetoxPoison => Instance[(short)70];

		public static SecretInformationItem RefuseRequestIncreaseHealth => Instance[(short)71];

		public static SecretInformationItem RefuseRequestRestoreDisorderOfQi => Instance[(short)72];

		public static SecretInformationItem RefuseRequestIncreaseNeili => Instance[(short)73];

		public static SecretInformationItem RefuseRequestKillWug => Instance[(short)74];

		public static SecretInformationItem RefuseRequestFood => Instance[(short)75];

		public static SecretInformationItem RefuseRequestTeaWine => Instance[(short)76];

		public static SecretInformationItem RefuseRequestResource => Instance[(short)77];

		public static SecretInformationItem RefuseRequestItem => Instance[(short)78];

		public static SecretInformationItem RefuseRequestDrinking => Instance[(short)79];

		public static SecretInformationItem RefuseRequestGivingMoney => Instance[(short)80];

		public static SecretInformationItem RefuseRequestInstructionOnReading => Instance[(short)81];

		public static SecretInformationItem RefuseRequestInstructionOnBreakout => Instance[(short)82];

		public static SecretInformationItem RefuseRequestRepairItem => Instance[(short)83];

		public static SecretInformationItem RefuseRequestAddPoisonToItem => Instance[(short)84];

		public static SecretInformationItem RefuseRequestInstructionOnLifeSkill => Instance[(short)85];

		public static SecretInformationItem RefuseRequestInstructionOnCombatSkill => Instance[(short)86];

		public static SecretInformationItem RobGraveResource => Instance[(short)87];

		public static SecretInformationItem StealResource => Instance[(short)88];

		public static SecretInformationItem ScamResource => Instance[(short)89];

		public static SecretInformationItem RobResource => Instance[(short)90];

		public static SecretInformationItem RobGraveItem => Instance[(short)91];

		public static SecretInformationItem StealItem => Instance[(short)92];

		public static SecretInformationItem ScamItem => Instance[(short)93];

		public static SecretInformationItem RobItem => Instance[(short)94];

		public static SecretInformationItem KillInPrivate => Instance[(short)95];

		public static SecretInformationItem KidnapInPrivate => Instance[(short)96];

		public static SecretInformationItem PoisonEnemy => Instance[(short)97];

		public static SecretInformationItem PlotHarmEnemy => Instance[(short)98];

		public static SecretInformationItem StealLifeSkill => Instance[(short)99];

		public static SecretInformationItem ScamLifeSkill => Instance[(short)100];

		public static SecretInformationItem StealCombatSkill => Instance[(short)101];

		public static SecretInformationItem ScamCombatSkill => Instance[(short)102];

		public static SecretInformationItem AddPoisonToItem => Instance[(short)103];

		public static SecretInformationItem MonkBreakRule => Instance[(short)104];

		public static SecretInformationItem MakeLoveIllegal => Instance[(short)105];

		public static SecretInformationItem Rape => Instance[(short)106];

		public static SecretInformationItem LoseFetusFatherUnknown => Instance[(short)107];

		public static SecretInformationItem GiveBirthToChildFatherUnknown => Instance[(short)108];

		public static SecretInformationItem DatingWithCrush => Instance[(short)109];

		public static SecretInformationItem ForcingSilence => Instance[(short)110];

		public static SecretInformationItem RetrieveChild => Instance[(short)111];

		public static SecretInformationItem SolveScripture1 => Instance[(short)112];

		public static SecretInformationItem SolveScripture2 => Instance[(short)113];

		public static SecretInformationItem SolveScripture3 => Instance[(short)114];

		public static SecretInformationItem SolveScripture4 => Instance[(short)115];

		public static SecretInformationItem PrisonBreak => Instance[(short)116];
	}

	public static SecretInformation Instance = new SecretInformation();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "DisseminationId", "DefaultEffectId", "StructGroupId", "Name", "Desc", "Parameters", "ParametersUiName", "BlockSizeArgs", "BroadcastDesc" };

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
		_dataArray.Add(new SecretInformationItem(0, 0, 1, new string[4] { "Character", "Location", "", "" }, -1, new int[4] { 2, 3, 4, 5 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 0, 0, 1, 0, 0, autoBroadCast: true, 0, -1, 5, -3, 10, 3, ESecretInformationInitialTarget.None, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 0, 0, 0, 1, 250, 10000, 5, 425, 2500, 6, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(1, 7, 8, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 10, 11 }, new sbyte[13]
		{
			24, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: true, 0, -1, 5, -3, 10, 12, ESecretInformationInitialTarget.None, new int[1], new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 1, 1, 15, 8, 250, 10000, 5, 425, 2500, 12, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(2, 13, 14, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 15, 16 }, new sbyte[13]
		{
			24, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: true, 0, -1, 5, -3, 10, 12, ESecretInformationInitialTarget.None, new int[1], new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 2, 2, 156, 8, 250, 10000, 5, 425, 2500, 17, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(3, 18, 19, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 20, 21 }, new sbyte[13]
		{
			21, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: true, 0, -1, 5, -3, 10, 6, ESecretInformationInitialTarget.None, new int[1], new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 3, 3, 109, 7, 250, 10000, 5, 425, 2500, 22, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(4, 18, 23, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 24, 25 }, new sbyte[13]
		{
			21, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: true, 0, -1, 5, -3, 10, 6, ESecretInformationInitialTarget.None, new int[1], new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 4, 4, 250, 7, 250, 10000, 5, 425, 2500, 26, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(5, 27, 28, new string[4] { "Character", "Resource", "Location", "" }, -1, new int[4] { 2, 29, 3, 30 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 0, 0, 1, 1, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Area, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 5, 5, 297, 3, 100, 10000, 0, 425, 1000, 31, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(6, 27, 28, new string[4] { "Character", "ItemKey", "Location", "" }, -1, new int[4] { 2, 29, 3, 32 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 1, 0, 0, 1, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Area, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 5, 6, 297, 3, 100, 10000, 0, 425, 1000, 31, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(7, 27, 33, new string[4] { "Character", "ItemKey", "Location", "" }, -1, new int[4] { 2, 29, 3, 34 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 1, 0, 0, 1, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Area, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 6, 7, 297, 3, 100, 10000, 0, 425, 1000, 31, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(8, 27, 35, new string[4] { "Character", "Location", "", "" }, -1, new int[4] { 2, 3, 36, 37 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 0, 0, 1, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Area, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 7, 8, 297, 3, 100, 10000, 0, 425, 1000, 38, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(9, 39, 40, new string[4] { "Character", "Resource", "Location", "" }, -1, new int[4] { 2, 29, 3, 41 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 0, 0, 1, 1, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Area, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 8, 9, 312, 3, 100, 10000, 0, 425, 1000, 42, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(10, 39, 40, new string[4] { "Character", "ItemKey", "Location", "" }, -1, new int[4] { 2, 29, 3, 43 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 1, 0, 0, 1, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Area, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 8, 10, 312, 3, 100, 10000, 0, 425, 1000, 42, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(11, 39, 44, new string[4] { "Character", "ItemKey", "Location", "" }, -1, new int[4] { 2, 29, 3, 45 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 1, 0, 0, 1, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Area, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 9, 11, 312, 3, 100, 10000, 0, 425, 1000, 42, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(12, 39, 46, new string[4] { "Character", "Location", "", "" }, -1, new int[4] { 2, 3, 47, 48 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 0, 0, 1, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Area, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 10, 12, 312, 3, 100, 10000, 0, 425, 1000, 49, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(13, 50, 51, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 52, 53 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Nearest, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 11, 13, 327, 1, 10, 10000, 1, 3100, 500, 54, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(14, 50, 55, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 56, 57 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Nearest, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 12, 14, 327, 1, 10, 10000, 1, 3100, 500, 58, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(15, 50, 59, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 60, 61 }, new sbyte[13]
		{
			2, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 200, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Nearest, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 13, 15, 327, 2, 10, 10000, 1, 3100, 500, 62, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(16, 50, 63, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 64, 65 }, new sbyte[13]
		{
			2, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 200, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Nearest, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 14, 16, 327, 2, 10, 10000, 1, 3100, 500, 62, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(17, 66, 67, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 68, 69 }, new sbyte[13]
		{
			2, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[1], new int[2] { 0, 1 }, new int[1] { 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 15, 17, 374, 2, 50, 10000, 5, 550, 1000, 70, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(18, 71, 72, new string[4] { "Character", "Character", "Character", "" }, -1, new int[4] { 2, 73, 74, 75 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[3] { 0, 1, 2 }, new int[3] { 0, 1, 2 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 16, 18, 421, 3, 100, 10000, 5, 550, 2500, 76, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(19, 77, 78, new string[4] { "Character", "Location", "", "" }, -1, new int[4] { 2, 3, 79, 80 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 0, 0, 1, 0, 0, autoBroadCast: false, 300, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[1], new int[1], new int[1], isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 17, 19, 518, 6, 200, 10000, 0, 300, 2500, 81, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(20, 77, 82, new string[4] { "Character", "Character", "Location", "" }, -1, new int[4] { 2, 9, 3, 83 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 1, 0, 0, autoBroadCast: false, 200, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[1], new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 18, 20, 533, 3, 100, 10000, 0, 300, 1000, 84, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(21, 85, 86, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 87, 88 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 300, 1, 3, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[1], new int[2] { 0, 1 }, new int[1], isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: true, ESecretInformationValueType.Negative, 20, 21, 580, 6, 200, 10000, 0, 300, 2500, 89, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(22, 85, 90, new string[4] { "Character", "Character", "Character", "" }, -1, new int[4] { 2, 73, 74, 91 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 3, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 200, 1, 3, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[1], new int[3] { 0, 1, 2 }, new int[2] { 0, 2 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: true, ESecretInformationValueType.Normal, 21, 22, 627, 3, 100, 10000, 0, 300, 1000, 92, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(23, 93, 94, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 95, 96 }, new sbyte[13]
		{
			7, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 2, 10, 48, ESecretInformationInitialTarget.Local, new int[1], new int[2] { 0, 1 }, new int[1], isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 19, 23, 724, 7, 200, 10000, 0, 300, 2500, 97, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(24, 98, 99, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 100, 101 }, new sbyte[13]
		{
			5, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 200, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 22, 24, 771, 5, 250, 10000, 5, 150, 2500, 102, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(25, 103, 104, new string[4] { "Character", "Character", "Character", "" }, -1, new int[4] { 2, 73, 74, 105 }, new sbyte[13]
		{
			5, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 200, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[3] { 0, 1, 2 }, new int[3] { 0, 1, 2 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 23, 25, 818, 5, 250, 10000, 5, 150, 2500, 106, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(26, 107, 108, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 109, 110 }, new sbyte[13]
		{
			5, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 200, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 24, 26, 915, 5, 250, 10000, 5, 150, 2500, 111, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(27, 112, 113, new string[4] { "Character", "ItemKey", "", "" }, -1, new int[4] { 2, 114, 115, 116 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 200, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 25, 27, 962, 1, 10, 10000, 1, 1225, 500, 117, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(28, 118, 119, new string[4] { "Character", "CombatSkill", "", "" }, -1, new int[4] { 2, 120, 121, 122 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 1, 0, 0, 0, 0, autoBroadCast: false, 200, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 26, 28, 977, 1, 10, 10000, 1, 1225, 500, 123, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(29, 124, 125, new string[4] { "Character", "ItemKey", "Location", "" }, -1, new int[4] { 2, 29, 3, 126 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 1, 0, 0, 1, 0, 0, autoBroadCast: false, 200, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[1], new int[1], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 27, 29, 992, 3, 25, 10000, 1, 2475, 1000, 127, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(30, 128, 129, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 130, 131 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 200, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 28, 30, 1054, 3, 10, 10000, 5, 3050, 500, 132, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(31, 133, 134, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 135, 136 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 200, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 35, 31, 1101, 3, 10, 10000, 5, 3050, 500, 137, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(32, 138, 139, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 140, 141 }, new sbyte[13]
		{
			4, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 300, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 31, 32, 1148, 4, 25, 10000, 5, 1175, 500, 142, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(33, 143, 144, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 145, 146 }, new sbyte[13]
		{
			4, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 300, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 37, 33, 1195, 4, 25, 10000, 5, 1175, 500, 147, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(34, 148, 149, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 150, 151 }, new sbyte[13]
		{
			5, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 400, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: true, ESecretInformationValueType.Positive, 29, 34, 1242, 5, 50, 10000, 5, 675, 1000, 152, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(35, 153, 154, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 155, 156 }, new sbyte[13]
		{
			5, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 400, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: true, ESecretInformationValueType.Negative, 36, 35, 1289, 5, 50, 10000, 5, 675, 1000, 157, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(36, 158, 159, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 160, 161 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: true, ESecretInformationValueType.Positive, 30, 36, 1336, 6, 50, 10000, 5, 800, 1500, 162, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(37, 163, 164, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 165, 166 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 32, 37, 1383, 6, 50, 10000, 5, 800, 1500, 167, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(38, 168, 169, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 170, 171 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 38, 38, 1430, 6, 50, 10000, 5, 800, 1500, 172, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(39, 173, 174, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 175, 176 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 33, 39, 1477, 6, 100, 10000, 5, 550, 2500, 177, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(40, 178, 179, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 180, 181 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 34, 40, 1524, 6, 100, 10000, 5, 550, 2500, 182, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(41, 183, 184, new string[4] { "Character", "Character", "Resource", "" }, -1, new int[4] { 2, 9, 29, 185 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 1, 0, autoBroadCast: false, 100, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 39, 41, 1571, 1, 10, 10000, 5, 3050, 500, 186, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(42, 183, 187, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 188 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 40, 42, 1571, 1, 10, 10000, 5, 3050, 500, 186, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(43, 189, 190, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 191, 192 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Nearest, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 41, 43, 1007, 1, 10, 10000, 5, 3050, 500, 193, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(44, 194, 195, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 196, 197 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 42, 44, 1571, 1, 10, 10000, 5, 3050, 500, 198, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(45, 199, 200, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 201 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 43, 45, 1571, 1, 10, 10000, 5, 3050, 500, 202, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(46, 203, 204, new string[4] { "Character", "Character", "LifeSkill", "" }, -1, new int[4] { 2, 9, 114, 205 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 1, 0, 0, 0, autoBroadCast: false, 200, -1, 5, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 44, 46, 1571, 1, 10, 10000, 5, 3050, 500, 206, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(47, 203, 207, new string[4] { "Character", "Character", "CombatSkill", "" }, -1, new int[4] { 2, 9, 120, 208 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 1, 0, 0, 0, 0, autoBroadCast: false, 200, -1, 5, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 45, 47, 1571, 1, 10, 10000, 5, 3050, 500, 206, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(48, 209, 210, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 211, 212 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 46, 48, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(49, 209, 214, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 215, 216 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 47, 49, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(50, 209, 217, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 218, 219 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 48, 50, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(51, 209, 220, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 221, 222 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 49, 51, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(52, 209, 223, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 224, 225 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 50, 52, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(53, 209, 226, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 227, 228 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 51, 53, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(54, 209, 229, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 230 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 52, 54, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(55, 209, 231, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 232 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 53, 55, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(56, 209, 233, new string[4] { "Character", "Character", "Resource", "" }, -1, new int[4] { 2, 9, 29, 234 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 1, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 54, 56, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(57, 209, 235, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 236 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 55, 57, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(58, 209, 237, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 238, 239 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 56, 58, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(59, 209, 240, new string[4] { "Character", "Character", "Integer", "" }, -1, new int[4] { 2, 9, 29, 241 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 1, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 57, 59, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new SecretInformationItem(60, 209, 242, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 243 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 58, 60, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(61, 209, 244, new string[4] { "Character", "Character", "CombatSkill", "" }, -1, new int[4] { 2, 9, 120, 245 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 1, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 59, 61, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(62, 209, 246, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 247 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 60, 62, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(63, 209, 248, new string[4] { "Character", "Character", "ItemKey", "ItemKey" }, -1, new int[4] { 2, 9, 249, 250 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 2, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 61, 63, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(64, 209, 251, new string[4] { "Character", "Character", "LifeSkill", "" }, -1, new int[4] { 2, 9, 114, 252 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 1, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 62, 64, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(65, 209, 253, new string[4] { "Character", "Character", "CombatSkill", "" }, -1, new int[4] { 2, 9, 120, 254 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 1, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 63, 65, 1618, 1, 1, 10000, 5, 12425, 200, 213, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(66, 255, 256, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 257, 258 }, new sbyte[13]
		{
			2, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[1] { 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 64, 66, 1712, 2, 10, 10000, 1, 3100, 500, 259, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(67, 255, 260, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 261, 262 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[1] { 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 65, 67, 1759, 3, 10, 10000, 1, 3725, 1000, 263, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(68, 255, 264, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 265, 266 }, new sbyte[13]
		{
			4, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[1] { 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 66, 68, 1806, 4, 10, 10000, 1, 4350, 1500, 267, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(69, 268, 269, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 270, 271 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 67, 69, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(70, 268, 273, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 274, 275 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 68, 70, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(71, 268, 276, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 277, 278 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 69, 71, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(72, 268, 279, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 280, 281 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 70, 72, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(73, 268, 282, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 283, 284 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 71, 73, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(74, 268, 285, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 286, 287 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 72, 74, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(75, 268, 288, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 289 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 73, 75, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(76, 268, 290, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 291 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 74, 76, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(77, 268, 292, new string[4] { "Character", "Character", "Resource", "" }, -1, new int[4] { 2, 9, 29, 293 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 1, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 75, 77, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(78, 268, 294, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 295 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 76, 78, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(79, 268, 296, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 297, 298 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 77, 79, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(80, 268, 299, new string[4] { "Character", "Character", "Integer", "" }, -1, new int[4] { 2, 9, 29, 300 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 1, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 78, 80, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(81, 268, 301, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 302 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 79, 81, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(82, 268, 303, new string[4] { "Character", "Character", "CombatSkill", "" }, -1, new int[4] { 2, 9, 114, 304 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 1, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 80, 82, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(83, 268, 305, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 306 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 81, 83, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(84, 268, 307, new string[4] { "Character", "Character", "ItemKey", "ItemKey" }, -1, new int[4] { 2, 9, 249, 250 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 2, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 82, 84, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(85, 268, 308, new string[4] { "Character", "Character", "LifeSkill", "" }, -1, new int[4] { 2, 9, 114, 309 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 1, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 83, 85, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(86, 268, 310, new string[4] { "Character", "Character", "CombatSkill", "" }, -1, new int[4] { 2, 9, 120, 311 }, new sbyte[13]
		{
			1, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 1, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -1, 10, 3, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 84, 86, 1665, 1, 1, 10000, 5, 12425, 200, 272, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(87, 312, 313, new string[4] { "Character", "Character", "Resource", "" }, -1, new int[4] { 2, 9, 29, 314 }, new sbyte[13]
		{
			5, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 1, 0, autoBroadCast: false, 300, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[1], new int[2] { 0, 1 }, new int[1], isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 85, 87, 1853, 5, 250, 10000, 1, 100, 1000, 315, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(88, 316, 317, new string[4] { "Character", "Character", "Resource", "" }, -1, new int[4] { 2, 9, 29, 318 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 1, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 86, 88, 1900, 6, 500, 10000, 1, 100, 2500, 319, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(89, 320, 321, new string[4] { "Character", "Character", "Resource", "" }, -1, new int[4] { 2, 9, 29, 322 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 1, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 87, 89, 1947, 6, 500, 10000, 1, 100, 2500, 323, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(90, 324, 325, new string[4] { "Character", "Character", "Resource", "" }, -1, new int[4] { 2, 9, 29, 326 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 1, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 88, 90, 1994, 6, 500, 10000, 1, 100, 2500, 327, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(91, 312, 328, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 329 }, new sbyte[13]
		{
			5, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 300, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[1], new int[2] { 0, 1 }, new int[1], isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 89, 91, 1853, 5, 250, 10000, 1, 100, 1000, 315, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(92, 316, 330, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 331 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 90, 92, 1900, 6, 500, 10000, 1, 100, 2500, 319, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(93, 320, 332, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 333 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 91, 93, 1947, 6, 500, 10000, 1, 100, 2500, 323, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(94, 324, 334, new string[4] { "Character", "Character", "ItemKey", "" }, -1, new int[4] { 2, 9, 29, 335 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 92, 94, 1994, 6, 500, 10000, 1, 100, 2500, 327, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(95, 7, 336, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 337, 338 }, new sbyte[13]
		{
			27, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 1000, 1, 3, 2, 10, 12, ESecretInformationInitialTarget.Local, new int[1], new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 93, 95, 62, 9, 1000, 10000, 1, 100, 2500, 339, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(96, 13, 340, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 341, 342 }, new sbyte[13]
		{
			27, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 1000, 1, 3, 2, 10, 12, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 94, 96, 203, 9, 1000, 10000, 1, 100, 2500, 343, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(97, 344, 345, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 346, 347 }, new sbyte[13]
		{
			8, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 95, 97, 2041, 8, 500, 10000, 1, 100, 2500, 348, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(98, 349, 350, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 351, 352 }, new sbyte[13]
		{
			8, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 96, 98, 2088, 8, 500, 10000, 1, 100, 2500, 353, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(99, 354, 355, new string[4] { "Character", "Character", "LifeSkill", "" }, -1, new int[4] { 2, 9, 114, 356 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 1, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 97, 99, 2135, 6, 250, 10000, 1, 100, 1000, 357, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(100, 354, 358, new string[4] { "Character", "Character", "LifeSkill", "" }, -1, new int[4] { 2, 9, 114, 359 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 1, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 98, 100, 2135, 6, 250, 10000, 1, 100, 1000, 357, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(101, 360, 361, new string[4] { "Character", "Character", "CombatSkill", "" }, -1, new int[4] { 2, 9, 120, 362 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 1, 0, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 99, 101, 2135, 6, 250, 10000, 1, 100, 1000, 363, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(102, 360, 364, new string[4] { "Character", "Character", "CombatSkill", "" }, -1, new int[4] { 2, 9, 120, 365 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 1, 0, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 100, 102, 2135, 6, 250, 10000, 1, 100, 1000, 363, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(103, 366, 367, new string[4] { "Character", "ItemKey", "ItemKey", "" }, -1, new int[4] { 2, 249, 250, 368 }, new sbyte[13]
		{
			3, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 2, 0, 0, 0, 0, 0, autoBroadCast: false, 300, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[1], new int[1], new int[1], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 101, 103, 2182, 3, 10, 10000, 1, 3100, 1000, 369, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(104, 370, 371, new string[4] { "Character", "ItemKey", "", "" }, -1, new int[4] { 2, 29, 372, 373 }, new sbyte[13]
		{
			5, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 1, 0, 0, 0, 0, 0, autoBroadCast: false, 300, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[1], new int[1], new int[1], isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 102, 104, 2197, 5, 10, 10000, 1, 3100, 1000, 374, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(105, 375, 376, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 377, 378 }, new sbyte[13]
		{
			7, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 2, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: true, ESecretInformationValueType.Negative, 103, 105, 2212, 7, 1000, 10000, 1, 100, 2500, 379, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(106, 380, 381, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 382, 383 }, new sbyte[13]
		{
			27, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 1000, 1, 3, 2, 10, 12, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: true, ESecretInformationValueType.Negative, 104, 106, 2259, 9, 1000, 10000, 1, 100, 2500, 384, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(107, 385, 386, new string[4] { "Character", "Character", "Location", "" }, 1, new int[4] { 2, 387, 3, 388 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 1, 0, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[1], new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: true, ESecretInformationValueType.Negative, 17, 19, 518, 6, 200, 10000, 0, 300, 2500, 81, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(108, 385, 86, new string[4] { "Character", "Character", "Character", "" }, 2, new int[4] { 2, 9, 389, 390 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 3, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[1], new int[3] { 0, 1, 2 }, new int[2] { 0, 2 }, isGeneralRelationCharactersNeedSnapshot: true, isRelationCharactersAliveStateNeedSnapshot: true, ESecretInformationValueType.Negative, 20, 21, 580, 6, 200, 10000, 0, 300, 2500, 89, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(109, 391, 392, new string[4] { "Character", "Character", "Location", "" }, -1, new int[4] { 2, 9, 3, 393 }, new sbyte[13]
		{
			4, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 1, 0, 0, autoBroadCast: false, 300, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: true, ESecretInformationValueType.Positive, 105, 109, 3578, 4, 100, 10000, 5, 800, 2500, 394, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(110, 395, 396, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 397, 398 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, 1, 3, 1, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[1], new int[1], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 106, 110, 3590, 6, 500, 10000, 1, 100, 2500, 399, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(111, 400, 401, new string[4] { "Character", "Character", "", "" }, -1, new int[4] { 2, 9, 402, 403 }, new sbyte[13]
		{
			6, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 2, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 500, -1, 5, 0, 10, 6, ESecretInformationInitialTarget.Local, new int[2] { 0, 1 }, new int[2] { 0, 1 }, new int[2] { 0, 1 }, isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Positive, 107, 111, 3651, 6, 100, 10000, 5, 550, 2500, 92, autoDissemination: true));
		_dataArray.Add(new SecretInformationItem(112, 404, 405, new string[4] { "Character", "", "", "" }, -1, new int[4] { 2, 406, 407, 408 }, new sbyte[13]
		{
			27, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -3, 5, -1, ESecretInformationInitialTarget.None, new int[1], new int[0], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 108, 112, 3712, 1, 10000, 10000, 1, 25, -1, 409, autoDissemination: false));
		_dataArray.Add(new SecretInformationItem(113, 404, 410, new string[4] { "Character", "", "", "" }, -1, new int[4] { 2, 411, 412, 413 }, new sbyte[13]
		{
			27, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -3, 10, -1, ESecretInformationInitialTarget.None, new int[1], new int[0], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 109, 113, 3713, 1, 10000, 10000, 1, 25, -1, 409, autoDissemination: false));
		_dataArray.Add(new SecretInformationItem(114, 404, 414, new string[4] { "Character", "", "", "" }, -1, new int[4] { 2, 415, 416, 417 }, new sbyte[13]
		{
			27, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -3, 15, -1, ESecretInformationInitialTarget.None, new int[1], new int[0], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 110, 114, 3714, 1, 10000, 10000, 1, 25, -1, 409, autoDissemination: false));
		_dataArray.Add(new SecretInformationItem(115, 404, 418, new string[4] { "Character", "", "", "" }, -1, new int[4] { 2, 419, 420, 421 }, new sbyte[13]
		{
			27, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 0, 0, 0, 0, 0, autoBroadCast: false, 100, -1, 5, -3, 20, -1, ESecretInformationInitialTarget.None, new int[1], new int[0], new int[0], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Normal, 111, 115, 3715, 1, 10000, 10000, 1, 25, -1, 409, autoDissemination: false));
		_dataArray.Add(new SecretInformationItem(116, 422, 423, new string[4] { "Character", "Location", "", "" }, -1, new int[4] { 2, 3, 424, 425 }, new sbyte[13]
		{
			27, 1, 6, 22, 1, 3, 10, 0, 3, 11,
			1, 5, 15
		}, 1, 0, 0, 0, 1, 0, 0, autoBroadCast: true, 0, -1, 5, -3, 10, 12, ESecretInformationInitialTarget.None, new int[1], new int[1], new int[1], isGeneralRelationCharactersNeedSnapshot: false, isRelationCharactersAliveStateNeedSnapshot: false, ESecretInformationValueType.Negative, 112, 116, 3716, 6, 250, 10000, 5, 425, 2500, 426, autoDissemination: true));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SecretInformationItem>(117);
		CreateItems0();
		CreateItems1();
	}
}
