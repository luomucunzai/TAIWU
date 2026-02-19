using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LegacyPoint : ConfigData<LegacyPointItem, short>
{
	public static class DefKey
	{
		public const short MeetNewPeople = 0;

		public const short MakeFriends = 1;

		public const short BecomeLovers = 2;

		public const short MarryLovedOne = 3;

		public const short HaveChildren = 4;

		public const short SwornBrothersAndSisters = 5;

		public const short AdoptChildren = 6;

		public const short GetAdopted = 7;

		public const short CombatToPlay = 8;

		public const short CombatToTest = 9;

		public const short CombatToBeat = 10;

		public const short CombatToKill = 11;

		public const short SaveTheInfected = 12;

		public const short DestroyEnemyNest = 13;

		public const short LearnLifeSkill = 14;

		public const short ReadLifeSkillNormalPage = 15;

		public const short ReadLifeSkillIncompletePage = 16;

		public const short FinishLifeSkillBook = 17;

		public const short LifeSkillBattleWin = 18;

		public const short TeachLifeSkillInShrine = 19;

		public const short LearnCombatSkill = 20;

		public const short ReadCombatSkillNormalPage = 21;

		public const short ReadCombatSkillIncompletePage = 22;

		public const short ProficiencyEnough = 23;

		public const short BreakoutCombatSkill = 24;

		public const short TeachCombatSkillInShrine = 25;

		public const short ConstructVillage = 26;

		public const short ExpandVillage = 27;

		public const short GainResources = 28;

		public const short ManagementGain = 29;

		public const short CraftValuableItem = 30;

		public const short HireGoodWorker = 31;

		public const short CatchCricket = 32;

		public const short CricketBattle = 33;

		public const short GainInformation = 34;

		public const short UseInformation = 35;

		public const short DeliverSaluteToSect = 36;

		public const short UnlockStation = 37;

		public const short GetSupportFromSectMembers = 38;

		public const short CompleteAdventure = 39;

		public const short PrimaryProfession = 42;

		public const short MiddleProfession = 43;

		public const short AdvancedProfession = 44;

		public const short MasterProfession = 45;

		public const short GainExtraLegacyPoint = 46;
	}

	public static class DefValue
	{
		public static LegacyPointItem MeetNewPeople => Instance[(short)0];

		public static LegacyPointItem MakeFriends => Instance[(short)1];

		public static LegacyPointItem BecomeLovers => Instance[(short)2];

		public static LegacyPointItem MarryLovedOne => Instance[(short)3];

		public static LegacyPointItem HaveChildren => Instance[(short)4];

		public static LegacyPointItem SwornBrothersAndSisters => Instance[(short)5];

		public static LegacyPointItem AdoptChildren => Instance[(short)6];

		public static LegacyPointItem GetAdopted => Instance[(short)7];

		public static LegacyPointItem CombatToPlay => Instance[(short)8];

		public static LegacyPointItem CombatToTest => Instance[(short)9];

		public static LegacyPointItem CombatToBeat => Instance[(short)10];

		public static LegacyPointItem CombatToKill => Instance[(short)11];

		public static LegacyPointItem SaveTheInfected => Instance[(short)12];

		public static LegacyPointItem DestroyEnemyNest => Instance[(short)13];

		public static LegacyPointItem LearnLifeSkill => Instance[(short)14];

		public static LegacyPointItem ReadLifeSkillNormalPage => Instance[(short)15];

		public static LegacyPointItem ReadLifeSkillIncompletePage => Instance[(short)16];

		public static LegacyPointItem FinishLifeSkillBook => Instance[(short)17];

		public static LegacyPointItem LifeSkillBattleWin => Instance[(short)18];

		public static LegacyPointItem TeachLifeSkillInShrine => Instance[(short)19];

		public static LegacyPointItem LearnCombatSkill => Instance[(short)20];

		public static LegacyPointItem ReadCombatSkillNormalPage => Instance[(short)21];

		public static LegacyPointItem ReadCombatSkillIncompletePage => Instance[(short)22];

		public static LegacyPointItem ProficiencyEnough => Instance[(short)23];

		public static LegacyPointItem BreakoutCombatSkill => Instance[(short)24];

		public static LegacyPointItem TeachCombatSkillInShrine => Instance[(short)25];

		public static LegacyPointItem ConstructVillage => Instance[(short)26];

		public static LegacyPointItem ExpandVillage => Instance[(short)27];

		public static LegacyPointItem GainResources => Instance[(short)28];

		public static LegacyPointItem ManagementGain => Instance[(short)29];

		public static LegacyPointItem CraftValuableItem => Instance[(short)30];

		public static LegacyPointItem HireGoodWorker => Instance[(short)31];

		public static LegacyPointItem CatchCricket => Instance[(short)32];

		public static LegacyPointItem CricketBattle => Instance[(short)33];

		public static LegacyPointItem GainInformation => Instance[(short)34];

		public static LegacyPointItem UseInformation => Instance[(short)35];

		public static LegacyPointItem DeliverSaluteToSect => Instance[(short)36];

		public static LegacyPointItem UnlockStation => Instance[(short)37];

		public static LegacyPointItem GetSupportFromSectMembers => Instance[(short)38];

		public static LegacyPointItem CompleteAdventure => Instance[(short)39];

		public static LegacyPointItem PrimaryProfession => Instance[(short)42];

		public static LegacyPointItem MiddleProfession => Instance[(short)43];

		public static LegacyPointItem AdvancedProfession => Instance[(short)44];

		public static LegacyPointItem MasterProfession => Instance[(short)45];

		public static LegacyPointItem GainExtraLegacyPoint => Instance[(short)46];
	}

	public static LegacyPoint Instance = new LegacyPoint();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "Type", "BonusTypes", "TemplateId", "Name", "ConditionDesc" };

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
		_dataArray.Add(new LegacyPointItem(0, 0, 0, 5, 500, new byte[5] { 12, 2, 3, 4, 14 }, 1));
		_dataArray.Add(new LegacyPointItem(1, 2, 0, 10, 500, new byte[5] { 12, 2, 3, 4, 14 }, 3));
		_dataArray.Add(new LegacyPointItem(2, 4, 0, 250, 500, new byte[5] { 12, 2, 3, 4, 14 }, 5));
		_dataArray.Add(new LegacyPointItem(3, 6, 0, 500, 500, new byte[5] { 12, 2, 3, 4, 14 }, 7));
		_dataArray.Add(new LegacyPointItem(4, 8, 0, 250, 500, new byte[5] { 12, 2, 3, 4, 14 }, 9));
		_dataArray.Add(new LegacyPointItem(5, 10, 0, 100, 500, new byte[5] { 12, 2, 3, 4, 14 }, 11));
		_dataArray.Add(new LegacyPointItem(6, 12, 0, 250, 500, new byte[5] { 12, 2, 3, 4, 14 }, 13));
		_dataArray.Add(new LegacyPointItem(7, 14, 0, 250, 500, new byte[5] { 12, 2, 3, 4, 14 }, 15));
		_dataArray.Add(new LegacyPointItem(8, 16, 1, 5, 500, new byte[9] { 1, 11, 8, 9, 10, 2, 3, 4, 14 }, 17));
		_dataArray.Add(new LegacyPointItem(9, 18, 1, 10, 500, new byte[9] { 1, 11, 8, 9, 10, 2, 3, 4, 14 }, 19));
		_dataArray.Add(new LegacyPointItem(10, 20, 1, 10, 500, new byte[9] { 1, 11, 8, 9, 10, 2, 3, 4, 14 }, 21));
		_dataArray.Add(new LegacyPointItem(11, 22, 1, 20, 500, new byte[9] { 1, 11, 8, 9, 10, 2, 3, 4, 14 }, 23));
		_dataArray.Add(new LegacyPointItem(12, 24, 1, 20, 1000, new byte[9] { 1, 11, 8, 9, 10, 2, 3, 4, 14 }, 25));
		_dataArray.Add(new LegacyPointItem(13, 26, 1, 50, 1000, new byte[9] { 1, 11, 8, 9, 10, 2, 3, 4, 14 }, 27));
		_dataArray.Add(new LegacyPointItem(14, 28, 2, 20, 500, new byte[5] { 8, 2, 3, 4, 14 }, 29));
		_dataArray.Add(new LegacyPointItem(15, 30, 2, 10, 500, new byte[5] { 8, 2, 3, 4, 14 }, 31));
		_dataArray.Add(new LegacyPointItem(16, 32, 2, 20, 500, new byte[5] { 8, 2, 3, 4, 14 }, 33));
		_dataArray.Add(new LegacyPointItem(17, 34, 2, 20, 500, new byte[5] { 8, 2, 3, 4, 14 }, 35));
		_dataArray.Add(new LegacyPointItem(18, 36, 2, 10, 500, new byte[5] { 8, 2, 3, 4, 14 }, 37));
		_dataArray.Add(new LegacyPointItem(19, 38, 2, 5, 500, new byte[5] { 8, 2, 3, 4, 14 }, 39));
		_dataArray.Add(new LegacyPointItem(20, 40, 3, 20, 500, new byte[5] { 8, 2, 3, 4, 14 }, 41));
		_dataArray.Add(new LegacyPointItem(21, 30, 3, 10, 500, new byte[5] { 8, 2, 3, 4, 14 }, 42));
		_dataArray.Add(new LegacyPointItem(22, 32, 3, 20, 500, new byte[5] { 8, 2, 3, 4, 14 }, 43));
		_dataArray.Add(new LegacyPointItem(23, 44, 3, 20, 500, new byte[5] { 8, 2, 3, 4, 14 }, 45));
		_dataArray.Add(new LegacyPointItem(24, 46, 3, 20, 500, new byte[6] { 8, 9, 2, 3, 4, 14 }, 47));
		_dataArray.Add(new LegacyPointItem(25, 48, 3, 5, 500, new byte[6] { 8, 9, 2, 3, 4, 14 }, 49));
		_dataArray.Add(new LegacyPointItem(26, 50, 4, 100, 1000, new byte[4] { 2, 3, 4, 14 }, 51));
		_dataArray.Add(new LegacyPointItem(27, 52, 4, 20, 1000, new byte[4] { 2, 3, 4, 14 }, 53));
		_dataArray.Add(new LegacyPointItem(28, 54, 4, 5, 500, new byte[4] { 2, 3, 4, 14 }, 55));
		_dataArray.Add(new LegacyPointItem(29, 56, 4, 5, 500, new byte[4] { 2, 3, 4, 14 }, 57));
		_dataArray.Add(new LegacyPointItem(30, 58, 4, 10, 500, new byte[4] { 2, 3, 4, 14 }, 59));
		_dataArray.Add(new LegacyPointItem(31, 60, 4, 20, 500, new byte[4] { 2, 3, 4, 14 }, 61));
		_dataArray.Add(new LegacyPointItem(32, 62, 5, 10, 500, new byte[2] { 2, 3 }, 63));
		_dataArray.Add(new LegacyPointItem(33, 64, 5, 10, 500, new byte[2] { 2, 3 }, 65));
		_dataArray.Add(new LegacyPointItem(34, 66, 5, 10, 500, new byte[2] { 2, 3 }, 67));
		_dataArray.Add(new LegacyPointItem(35, 68, 5, 10, 500, new byte[2] { 2, 3 }, 69));
		_dataArray.Add(new LegacyPointItem(36, 70, 5, 100, 500, new byte[2] { 2, 3 }, 71));
		_dataArray.Add(new LegacyPointItem(37, 72, 5, 100, 500, new byte[4] { 2, 3, 4, 14 }, 73));
		_dataArray.Add(new LegacyPointItem(38, 74, 5, 10, 500, new byte[4] { 2, 3, 4, 14 }, 75));
		_dataArray.Add(new LegacyPointItem(39, 76, 5, 20, 500, new byte[4] { 2, 3, 4, 14 }, 77));
		_dataArray.Add(new LegacyPointItem(40, 78, 6, 250, 2500, new byte[7] { 1, 8, 9, 10, 3, 4, 14 }, 79));
		_dataArray.Add(new LegacyPointItem(41, 80, 6, 2500, 7500, new byte[7] { 1, 8, 9, 10, 3, 4, 14 }, 81));
		_dataArray.Add(new LegacyPointItem(42, 82, 7, 50, 500, new byte[11]
		{
			12, 1, 11, 8, 9, 10, 2, 3, 4, 13,
			14
		}, 83));
		_dataArray.Add(new LegacyPointItem(43, 84, 7, 50, 500, new byte[11]
		{
			12, 1, 11, 8, 9, 10, 2, 3, 4, 13,
			14
		}, 85));
		_dataArray.Add(new LegacyPointItem(44, 86, 7, 100, 1000, new byte[11]
		{
			12, 1, 11, 8, 9, 10, 2, 3, 4, 13,
			14
		}, 87));
		_dataArray.Add(new LegacyPointItem(45, 88, 7, 200, 2000, new byte[11]
		{
			12, 1, 11, 8, 9, 10, 2, 3, 4, 13,
			14
		}, 89));
		_dataArray.Add(new LegacyPointItem(46, 90, -1, -1, -1, new byte[10] { 12, 1, 11, 8, 9, 10, 2, 3, 4, 13 }, 91));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<LegacyPointItem>(47);
		CreateItems0();
	}
}
