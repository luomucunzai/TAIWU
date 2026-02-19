using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells.Character;

namespace Config;

[Serializable]
public class SecretInformationAppliedSelection : ConfigData<SecretInformationAppliedSelectionItem, short>
{
	public static class DefKey
	{
		public const short OtherTopics = 0;

		public const short Agree = 1;

		public const short Disagree = 2;

		public const short Comfort = 3;

		public const short Gloat = 4;

		public const short Criticize = 5;

		public const short Support = 6;

		public const short Praise = 7;

		public const short Mock = 8;

		public const short Angry = 9;

		public const short Hurt = 10;

		public const short Punish = 11;

		public const short KillForRevenge = 12;

		public const short Provoke = 13;

		public const short RequestKeepSecret = 14;

		public const short ArgeeKeepSecret = 15;

		public const short RefuseKeepSecret = 16;

		public const short RequestRelease = 17;

		public const short RequestPrisoner = 18;

		public const short AgreeRelease = 19;

		public const short RefuseRelease = 20;

		public const short AgreeTransfer = 21;

		public const short RefuseTransfer = 22;

		public const short PrisonerAbsent = 23;

		public const short PrisonerAbsent2 = 24;

		public const short RescueByStealing = 25;

		public const short RescueByScamming = 26;

		public const short RescueByRobbing = 27;

		public const short RobPrisonerByStealing = 28;

		public const short RobPrisonerByScamming = 29;

		public const short RobPrisonerByRobbing = 30;

		public const short MakeEnemy = 31;

		public const short BeMadeEnemy = 32;

		public const short BreakUp = 33;

		public const short SeverSworn = 34;

		public const short SeverFriend = 35;

		public const short StartCombatByCharacter = 36;

		public const short Relieved = 37;

		public const short NoOtherWay = 38;

		public const short Slient = 39;

		public const short KillForKeepSecret = 40;

		public const short CombatRefuseKeepSecret = 41;

		public const short RespondCombat = 42;

		public const short DirectKidnap = 43;

		public const short DirectKidnap2 = 44;

		public const short DirectPunishKidnap = 45;

		public const short DirectKill = 46;

		public const short GiveUp = 47;

		public const short KidnapWithRope = 48;

		public const short GiveUpKidnapByBeat = 49;

		public const short GiveUpKidnapByKill = 50;

		public const short GiveUpKidnapByPunish = 51;

		public const short BeKilled = 52;

		public const short TryEscapeRescue = 53;

		public const short TryEscapeRob = 54;

		public const short HandlePrisoner = 55;

		public const short KidnapInPublicByRob = 56;

		public const short KidnapInPrivateByRob = 57;

		public const short HandlePrisoner2 = 58;

		public const short KidnapInPublicByRob2 = 59;

		public const short KidnapInPrivateByRob2 = 60;

		public const short KillInPublicByRob = 61;

		public const short KillForPunishByRob = 62;

		public const short GiveUpByRob = 63;

		public const short KidnapDetainerInPublic = 64;

		public const short KidnapDetainerInPrivate = 65;

		public const short TaiwuEscape = 66;

		public const short CharEscape = 67;

		public const short KillInPublic = 68;

		public const short KillForPunish = 69;

		public const short KillInPrivate = 70;

		public const short KidnapInPublic = 71;

		public const short KidnapForPunish = 72;

		public const short KidnapInPrivate = 73;

		public const short ScamDebating = 74;

		public const short RescueByChar = 75;

		public const short RobByChar = 76;

		public const short ResistStealPrisoner = 77;

		public const short NotResistStealPrisoner = 78;

		public const short ResistScamPrisoner = 79;

		public const short NotResistScamPrisoner = 80;

		public const short NotResistRobPrisonerEnemy = 81;

		public const short NotResistRobPrisoner = 82;

		public const short HandInPrisoner = 83;

		public const short ResistRobPrisoner = 84;

		public const short PrisonerRobbed = 85;

		public const short Threaten = 86;

		public const short SectLeaderKnown = 87;

		public const short ThreadPointer = 88;

		public const short AskCharBreakUp = 89;

		public const short BreakUpChar = 90;

		public const short ForceBreakUpChar = 91;

		public const short GiveUpWithLove = 92;

		public const short Admonish = 93;
	}

	public static class DefValue
	{
		public static SecretInformationAppliedSelectionItem OtherTopics => Instance[(short)0];

		public static SecretInformationAppliedSelectionItem Agree => Instance[(short)1];

		public static SecretInformationAppliedSelectionItem Disagree => Instance[(short)2];

		public static SecretInformationAppliedSelectionItem Comfort => Instance[(short)3];

		public static SecretInformationAppliedSelectionItem Gloat => Instance[(short)4];

		public static SecretInformationAppliedSelectionItem Criticize => Instance[(short)5];

		public static SecretInformationAppliedSelectionItem Support => Instance[(short)6];

		public static SecretInformationAppliedSelectionItem Praise => Instance[(short)7];

		public static SecretInformationAppliedSelectionItem Mock => Instance[(short)8];

		public static SecretInformationAppliedSelectionItem Angry => Instance[(short)9];

		public static SecretInformationAppliedSelectionItem Hurt => Instance[(short)10];

		public static SecretInformationAppliedSelectionItem Punish => Instance[(short)11];

		public static SecretInformationAppliedSelectionItem KillForRevenge => Instance[(short)12];

		public static SecretInformationAppliedSelectionItem Provoke => Instance[(short)13];

		public static SecretInformationAppliedSelectionItem RequestKeepSecret => Instance[(short)14];

		public static SecretInformationAppliedSelectionItem ArgeeKeepSecret => Instance[(short)15];

		public static SecretInformationAppliedSelectionItem RefuseKeepSecret => Instance[(short)16];

		public static SecretInformationAppliedSelectionItem RequestRelease => Instance[(short)17];

		public static SecretInformationAppliedSelectionItem RequestPrisoner => Instance[(short)18];

		public static SecretInformationAppliedSelectionItem AgreeRelease => Instance[(short)19];

		public static SecretInformationAppliedSelectionItem RefuseRelease => Instance[(short)20];

		public static SecretInformationAppliedSelectionItem AgreeTransfer => Instance[(short)21];

		public static SecretInformationAppliedSelectionItem RefuseTransfer => Instance[(short)22];

		public static SecretInformationAppliedSelectionItem PrisonerAbsent => Instance[(short)23];

		public static SecretInformationAppliedSelectionItem PrisonerAbsent2 => Instance[(short)24];

		public static SecretInformationAppliedSelectionItem RescueByStealing => Instance[(short)25];

		public static SecretInformationAppliedSelectionItem RescueByScamming => Instance[(short)26];

		public static SecretInformationAppliedSelectionItem RescueByRobbing => Instance[(short)27];

		public static SecretInformationAppliedSelectionItem RobPrisonerByStealing => Instance[(short)28];

		public static SecretInformationAppliedSelectionItem RobPrisonerByScamming => Instance[(short)29];

		public static SecretInformationAppliedSelectionItem RobPrisonerByRobbing => Instance[(short)30];

		public static SecretInformationAppliedSelectionItem MakeEnemy => Instance[(short)31];

		public static SecretInformationAppliedSelectionItem BeMadeEnemy => Instance[(short)32];

		public static SecretInformationAppliedSelectionItem BreakUp => Instance[(short)33];

		public static SecretInformationAppliedSelectionItem SeverSworn => Instance[(short)34];

		public static SecretInformationAppliedSelectionItem SeverFriend => Instance[(short)35];

		public static SecretInformationAppliedSelectionItem StartCombatByCharacter => Instance[(short)36];

		public static SecretInformationAppliedSelectionItem Relieved => Instance[(short)37];

		public static SecretInformationAppliedSelectionItem NoOtherWay => Instance[(short)38];

		public static SecretInformationAppliedSelectionItem Slient => Instance[(short)39];

		public static SecretInformationAppliedSelectionItem KillForKeepSecret => Instance[(short)40];

		public static SecretInformationAppliedSelectionItem CombatRefuseKeepSecret => Instance[(short)41];

		public static SecretInformationAppliedSelectionItem RespondCombat => Instance[(short)42];

		public static SecretInformationAppliedSelectionItem DirectKidnap => Instance[(short)43];

		public static SecretInformationAppliedSelectionItem DirectKidnap2 => Instance[(short)44];

		public static SecretInformationAppliedSelectionItem DirectPunishKidnap => Instance[(short)45];

		public static SecretInformationAppliedSelectionItem DirectKill => Instance[(short)46];

		public static SecretInformationAppliedSelectionItem GiveUp => Instance[(short)47];

		public static SecretInformationAppliedSelectionItem KidnapWithRope => Instance[(short)48];

		public static SecretInformationAppliedSelectionItem GiveUpKidnapByBeat => Instance[(short)49];

		public static SecretInformationAppliedSelectionItem GiveUpKidnapByKill => Instance[(short)50];

		public static SecretInformationAppliedSelectionItem GiveUpKidnapByPunish => Instance[(short)51];

		public static SecretInformationAppliedSelectionItem BeKilled => Instance[(short)52];

		public static SecretInformationAppliedSelectionItem TryEscapeRescue => Instance[(short)53];

		public static SecretInformationAppliedSelectionItem TryEscapeRob => Instance[(short)54];

		public static SecretInformationAppliedSelectionItem HandlePrisoner => Instance[(short)55];

		public static SecretInformationAppliedSelectionItem KidnapInPublicByRob => Instance[(short)56];

		public static SecretInformationAppliedSelectionItem KidnapInPrivateByRob => Instance[(short)57];

		public static SecretInformationAppliedSelectionItem HandlePrisoner2 => Instance[(short)58];

		public static SecretInformationAppliedSelectionItem KidnapInPublicByRob2 => Instance[(short)59];

		public static SecretInformationAppliedSelectionItem KidnapInPrivateByRob2 => Instance[(short)60];

		public static SecretInformationAppliedSelectionItem KillInPublicByRob => Instance[(short)61];

		public static SecretInformationAppliedSelectionItem KillForPunishByRob => Instance[(short)62];

		public static SecretInformationAppliedSelectionItem GiveUpByRob => Instance[(short)63];

		public static SecretInformationAppliedSelectionItem KidnapDetainerInPublic => Instance[(short)64];

		public static SecretInformationAppliedSelectionItem KidnapDetainerInPrivate => Instance[(short)65];

		public static SecretInformationAppliedSelectionItem TaiwuEscape => Instance[(short)66];

		public static SecretInformationAppliedSelectionItem CharEscape => Instance[(short)67];

		public static SecretInformationAppliedSelectionItem KillInPublic => Instance[(short)68];

		public static SecretInformationAppliedSelectionItem KillForPunish => Instance[(short)69];

		public static SecretInformationAppliedSelectionItem KillInPrivate => Instance[(short)70];

		public static SecretInformationAppliedSelectionItem KidnapInPublic => Instance[(short)71];

		public static SecretInformationAppliedSelectionItem KidnapForPunish => Instance[(short)72];

		public static SecretInformationAppliedSelectionItem KidnapInPrivate => Instance[(short)73];

		public static SecretInformationAppliedSelectionItem ScamDebating => Instance[(short)74];

		public static SecretInformationAppliedSelectionItem RescueByChar => Instance[(short)75];

		public static SecretInformationAppliedSelectionItem RobByChar => Instance[(short)76];

		public static SecretInformationAppliedSelectionItem ResistStealPrisoner => Instance[(short)77];

		public static SecretInformationAppliedSelectionItem NotResistStealPrisoner => Instance[(short)78];

		public static SecretInformationAppliedSelectionItem ResistScamPrisoner => Instance[(short)79];

		public static SecretInformationAppliedSelectionItem NotResistScamPrisoner => Instance[(short)80];

		public static SecretInformationAppliedSelectionItem NotResistRobPrisonerEnemy => Instance[(short)81];

		public static SecretInformationAppliedSelectionItem NotResistRobPrisoner => Instance[(short)82];

		public static SecretInformationAppliedSelectionItem HandInPrisoner => Instance[(short)83];

		public static SecretInformationAppliedSelectionItem ResistRobPrisoner => Instance[(short)84];

		public static SecretInformationAppliedSelectionItem PrisonerRobbed => Instance[(short)85];

		public static SecretInformationAppliedSelectionItem Threaten => Instance[(short)86];

		public static SecretInformationAppliedSelectionItem SectLeaderKnown => Instance[(short)87];

		public static SecretInformationAppliedSelectionItem ThreadPointer => Instance[(short)88];

		public static SecretInformationAppliedSelectionItem AskCharBreakUp => Instance[(short)89];

		public static SecretInformationAppliedSelectionItem BreakUpChar => Instance[(short)90];

		public static SecretInformationAppliedSelectionItem ForceBreakUpChar => Instance[(short)91];

		public static SecretInformationAppliedSelectionItem GiveUpWithLove => Instance[(short)92];

		public static SecretInformationAppliedSelectionItem Admonish => Instance[(short)93];
	}

	public static SecretInformationAppliedSelection Instance = new SecretInformationAppliedSelection();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "MutexSelectionIds", "MainAttributeCost", "SpecialConditionId", "SpecialConditionId2", "PlayerBehaviorTypeIds", "ResultId1", "ResultId2", "TemplateId", "Text", "SelectionTexts" };

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
		_dataArray.Add(new SecretInformationAppliedSelectionItem(0, 0, new int[5] { 1, 2, 3, 4, 5 }, new short[0], -1, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 0, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Esc));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(1, 6, new int[5] { 7, 8, 9, 10, 11 }, new short[0], 9001, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 1, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(2, 12, new int[5] { 13, 14, 15, 16, 17 }, new short[0], 9002, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 2, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(3, 18, new int[5] { 19, 20, 21, 22, 23 }, new short[0], 9003, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 3, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(4, 24, new int[5] { 25, 26, 27, 28, 29 }, new short[0], 9004, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[2] { 3, 4 }, -6, 4, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(5, 30, new int[5] { 31, 32, 33, 34, 35 }, new short[0], 9005, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 6, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(6, 36, new int[5] { 37, 38, 39, 40, 41 }, new short[0], 9006, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[2] { 3, 4 }, -6, 5, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(7, 42, new int[5] { 43, 44, 45, 46, 47 }, new short[0], 9007, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 7, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(8, 48, new int[5] { 49, 50, 51, 52, 53 }, new short[0], 9008, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[2] { 3, 4 }, -6, 8, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(9, 54, new int[5] { 55, 56, 57, 58, 59 }, new short[0], 9009, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 9, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(10, 60, new int[5] { 61, 62, 63, 64, 65 }, new short[0], 9010, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 10, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(11, 66, new int[5] { 67, 68, 69, 70, 71 }, new short[0], 7001, 3, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { 1, 0, 1, -1 }, new short[2] { 0, 1 }, -6, 16, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(12, 72, new int[5] { 73, 74, 75, 76, 77 }, new short[0], 7002, 3, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 17, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(13, 78, new int[5] { 79, 80, 81, 82, 83 }, new short[0], 7003, 3, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[2] { 3, 4 }, -6, 15, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(14, 84, new int[5] { 85, 86, 87, 88, 89 }, new short[0], 1001, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[3] { 2, 3, 4 }, -6, 261, 14, new sbyte[5] { 6, 5, 4, 5, 6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(15, 90, new int[5] { 91, 92, 93, 94, 95 }, new short[0], 1002, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[3] { 2, 3, 4 }, -6, 11, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(16, 96, new int[5] { 97, 98, 99, 100, 101 }, new short[0], 1003, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 12, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(17, 102, new int[5] { 103, 104, 105, 106, 107 }, new short[0], 5001, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(16, 1, 3)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 262, 209, new sbyte[5] { 5, 4, 4, 5, 6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(18, 108, new int[5] { 109, 110, 111, 112, 113 }, new short[0], 5002, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(16, 1, 3)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 263, 211, new sbyte[5] { 5, 4, 4, 5, 6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(19, 114, new int[5] { 115, 116, 117, 118, 119 }, new short[0], 5003, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(16, 1, 5)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 152, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(20, 120, new int[5] { 121, 122, 123, 124, 125 }, new short[0], 5004, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(16, 1, 5)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 153, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(21, 126, new int[5] { 127, 128, 129, 130, 131 }, new short[0], 5005, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(16, 1, 5)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 154, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(22, 132, new int[5] { 133, 134, 135, 136, 137 }, new short[0], 5006, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(16, 1, 5)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 155, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(23, 138, new int[5] { 139, 140, 141, 142, 143 }, new short[1], 5007, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(16, 1, 3, 5)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 0, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(24, 138, new int[5] { 144, 145, 146, 147, 148 }, new short[1], 5007, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(16, 1, 5, 5)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 0, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(25, 149, new int[5] { 150, 151, 152, 153, 154 }, new short[0], 5008, 3, new PropertyAndValue(1, 20), new List<ShortList>
		{
			new ShortList(16, 1, 3)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 75, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(26, 155, new int[5] { 156, 157, 158, 159, 160 }, new short[0], 5009, 3, new PropertyAndValue(2, 20), new List<ShortList>
		{
			new ShortList(16, 1, 3)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 90, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(27, 161, new int[5] { 162, 163, 164, 165, 166 }, new short[0], 5010, 3, new PropertyAndValue(0, 20), new List<ShortList>
		{
			new ShortList(16, 1, 3)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 99, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(28, 167, new int[5] { 168, 169, 170, 171, 172 }, new short[0], 5011, 3, new PropertyAndValue(1, 20), new List<ShortList>
		{
			new ShortList(16, 1, 3)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 111, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(29, 173, new int[5] { 174, 175, 176, 177, 178 }, new short[0], 5012, 3, new PropertyAndValue(2, 20), new List<ShortList>
		{
			new ShortList(16, 1, 3)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 137, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(30, 179, new int[5] { 180, 181, 182, 183, 184 }, new short[0], 5013, 3, new PropertyAndValue(0, 20), new List<ShortList>
		{
			new ShortList(16, 1, 3)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 146, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(31, 185, new int[5] { 186, 187, 188, 189, 190 }, new short[0], 8001, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(15, 5, 3, -16, 5)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 215, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(32, 191, new int[5] { 192, 193, 194, 195, 196 }, new short[1], 8002, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(15, 3, 5, -16, 5)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 223, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(33, 197, new int[5] { 198, 199, 200, 201, 202 }, new short[0], 8003, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(15, 5, 3, -12)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 217, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(34, 203, new int[5] { 204, 205, 206, 207, 208 }, new short[0], 8004, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(15, 5, 3, -10)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 218, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(35, 209, new int[5] { 210, 211, 212, 213, 214 }, new short[0], 8005, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(15, 5, 3, -14)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 216, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(36, 215, new int[5] { 216, 217, 218, 219, 220 }, new short[0], 2001, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 35, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(37, 221, new int[5] { 222, 223, 224, 225, 226 }, new short[0], 2002, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, -1, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(38, 227, new int[5] { 228, 229, 230, 231, 232 }, new short[0], 2003, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, -1, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(39, 233, new int[5] { 234, 235, 236, 237, 238 }, new short[0], 2004, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, -1, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(40, 239, new int[5] { 240, 241, 242, 243, 244 }, new short[0], 3001, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 17, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(41, 233, new int[5] { 245, 246, 247, 248, 249 }, new short[0], 3002, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 34, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(42, 215, new int[5] { 250, 251, 252, 253, 254 }, new short[0], 3003, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 33, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(43, 255, new int[5] { 256, 257, 258, 259, 260 }, new short[0], 6001, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 70, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(44, 255, new int[5] { 261, 262, 263, 264, 265 }, new short[0], 6002, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 71, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(45, 255, new int[5] { 266, 267, 268, 269, 270 }, new short[0], 6003, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 72, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(46, 271, new int[5] { 272, 273, 274, 275, 276 }, new short[0], 6004, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 22, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(47, 277, new int[5] { 278, 279, 280, 281, 282 }, new short[0], 6005, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 55, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(48, 283, new int[5] { 284, 285, 286, 287, 288 }, new short[0], 10001, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 73, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(49, 289, new int[5] { 290, 291, 292, 293, 294 }, new short[0], 10002, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 69, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(50, 289, new int[5] { 295, 296, 297, 298, 299 }, new short[0], 10003, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 68, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(51, 289, new int[5] { 300, 301, 302, 303, 304 }, new short[0], 10004, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 67, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(52, 305, new int[5] { 306, 307, 308, 309, 310 }, new short[0], 9999, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 64, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(53, 311, new int[5] { 312, 313, 314, 315, 316 }, new short[0], 8001, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 81, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(54, 311, new int[5] { 317, 318, 319, 320, 321 }, new short[0], 8002, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 117, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(55, 322, new int[5] { 323, 324, 325, 326, 327 }, new short[0], 8003, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 131, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(56, 328, new int[5] { 329, 330, 331, 332, 333 }, new short[0], 8004, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 128, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(57, 334, new int[5] { 335, 336, 337, 338, 339 }, new short[0], 8005, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 129, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(58, 322, new int[5] { 340, 341, 342, 343, 344 }, new short[0], 8006, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 134, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(59, 328, new int[5] { 345, 346, 347, 348, 349 }, new short[0], 8007, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 135, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new SecretInformationAppliedSelectionItem(60, 334, new int[5] { 350, 351, 352, 353, 354 }, new short[0], 8008, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 136, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(61, 355, new int[5] { 356, 357, 358, 359, 360 }, new short[0], 8009, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 126, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(62, 361, new int[5] { 362, 363, 364, 365, 366 }, new short[0], 8010, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 127, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(63, 277, new int[5] { 367, 368, 369, 370, 371 }, new short[0], 8011, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 130, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(64, 328, new int[5] { 372, 373, 374, 375, 376 }, new short[0], 8012, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 132, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(65, 334, new int[5] { 377, 378, 379, 380, 381 }, new short[0], 8013, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 133, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(66, 227, new int[5] { 382, 383, 384, 385, 386 }, new short[0], 8014, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 65, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(67, 221, new int[5] { 387, 388, 389, 390, 391 }, new short[0], 8015, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 66, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(68, 392, new int[5] { 393, 394, 395, 396, 397 }, new short[0], 8016, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 49, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(69, 271, new int[5] { 398, 399, 400, 401, 402 }, new short[0], 8017, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 51, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(70, 361, new int[5] { 403, 404, 405, 406, 407 }, new short[0], 8018, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 50, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(71, 328, new int[5] { 408, 409, 410, 411, 412 }, new short[0], 8019, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 52, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(72, 255, new int[5] { 413, 414, 415, 416, 417 }, new short[0], 8020, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 54, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(73, 334, new int[5] { 418, 419, 420, 421, 422 }, new short[0], 8021, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 53, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(74, 423, new int[5] { 424, 425, 426, 427, 428 }, new short[0], 8022, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 32, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(75, 233, new int[5] { 429, 430, 431, 432, 433 }, new short[0], 8023, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 156, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(76, 233, new int[5] { 434, 435, 436, 437, 438 }, new short[0], 8024, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 183, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(77, 439, new int[5] { 440, 441, 442, 443, 444 }, new short[0], 8025, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 157, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(78, 445, new int[5] { 446, 447, 448, 449, 450 }, new short[0], 8026, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 166, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(79, 451, new int[5] { 452, 453, 454, 455, 456 }, new short[0], 8027, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 32, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(80, 457, new int[5] { 458, 459, 460, 461, 462 }, new short[0], 8028, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 171, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(81, 457, new int[5] { 463, 464, 465, 466, 467 }, new short[0], 8029, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 196, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(82, 468, new int[5] { 469, 470, 471, 472, 473 }, new short[0], 8030, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 176, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(83, 468, new int[5] { 474, 475, 476, 477, 478 }, new short[0], 8031, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 201, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(84, 479, new int[5] { 480, 481, 482, 483, 484 }, new short[0], 8032, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 33, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(85, 485, new int[5] { 486, 487, 488, 489, 490 }, new short[0], 8033, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, -1, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(86, 491, new int[5] { 492, 493, 494, 495, 496 }, new short[0], 1004, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 228, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(87, 221, new int[5] { 497, 498, 499, 500, 501 }, new short[0], 9011, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, -1, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(88, 502, new int[5] { 503, 504, 505, 506, 507 }, new short[0], 1005, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(53, 0, 3),
			new ShortList(53, 1, 3),
			new ShortList(53, 2, 3)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 260, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(89, 508, new int[5] { 509, 510, 511, 512, 513 }, new short[0], 1006, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(15, 3, 0, -12),
			new ShortList(15, 3, 0, -11),
			new ShortList(15, 3, 1, -12),
			new ShortList(15, 3, 1, -11),
			new ShortList(15, 3, 2, -12),
			new ShortList(15, 3, 2, -11)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 267, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(90, 197, new int[5] { 514, 515, 516, 517, 518 }, new short[0], 1007, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(15, 5, 3, -10)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 271, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(91, 519, new int[5] { 520, 521, 522, 523, 524 }, new short[0], 1008, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 275, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(92, 289, new int[5] { 525, 526, 527, 528, 529 }, new short[0], 1009, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, -1, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
		_dataArray.Add(new SecretInformationAppliedSelectionItem(93, 530, new int[5] { 531, 532, 533, 534, 535 }, new short[0], 1010, 0, default(PropertyAndValue), new List<ShortList>
		{
			new ShortList(-1)
		}, new List<ShortList>
		{
			new ShortList(-1)
		}, new sbyte[4] { -1, -1, -1, -1 }, new short[0], -6, 287, -1, new sbyte[5] { -6, -6, -6, -6, -6 }, ESecretInformationAppliedSelectionHotKey.Unbound));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SecretInformationAppliedSelectionItem>(94);
		CreateItems0();
		CreateItems1();
	}
}
