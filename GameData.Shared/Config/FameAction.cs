using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class FameAction : ConfigData<FameActionItem, short>
{
	public static class DefKey
	{
		public const short Kill = 0;

		public const short Kidnap = 2;

		public const short Rescue = 4;

		public const short MakeEnemy = 6;

		public const short SeverEnemy = 8;

		public const short MakeFriends = 10;

		public const short MakeBrothers = 12;

		public const short MakeLovers = 14;

		public const short MakeBadLovers = 15;

		public const short AdoptChild = 16;

		public const short AdoptedAsChild = 18;

		public const short GiveItem = 20;

		public const short TeachSkill = 22;

		public const short Heal = 24;

		public const short HealBad = 25;

		public const short SurpriseAttack = 26;

		public const short DisciplineEvil = 27;

		public const short StealSkill = 28;

		public const short Steal = 29;

		public const short Rob = 30;

		public const short RobGrave = 31;

		public const short Poison = 32;

		public const short Escape = 33;

		public const short FightChildren = 34;

		public const short TakeAdventageOfOthers = 35;

		public const short WinCombat = 36;

		public const short LoseCombat = 37;

		public const short ResponseKind = 38;

		public const short ResponseJust = 41;

		public const short ResponseRebel = 42;

		public const short ResponseEgoistic = 44;

		public const short RumorsAround = 46;

		public const short EasyToPickOn = 47;

		public const short GetAlms = 48;

		public const short GetBlame = 49;

		public const short GetFooled = 50;

		public const short GetDuress = 51;

		public const short GetPraised = 52;

		public const short GetRidiculed = 53;

		public const short InheritGoodOne = 54;

		public const short InheritBadOne = 55;

		public const short Indecent = 56;

		public const short Beg = 57;

		public const short Quack = 58;

		public const short KillHeretic = 59;

		public const short KillRighteous = 60;

		public const short Immoral = 61;

		public const short BreakRules = 62;

		public const short Unethical = 63;

		public const short Betrayal = 64;

		public const short Unfaithful = 65;

		public const short Rape = 66;

		public const short BeSneered = 67;

		public const short SneerSelf = 68;

		public const short CombatWithStrong = 69;

		public const short CombatWithWeak = 70;

		public const short FriendWithGoodSects = 71;

		public const short FriendWithEvilSects = 72;

		public const short FriendWithNeutralSects1 = 73;

		public const short FriendWithNeutralSects2 = 74;

		public const short WinSkill = 75;

		public const short LoseSkill = 76;

		public const short MakeFamousItem = 77;

		public const short DLCLoongDefeatLoong = 78;

		public const short IntrudeTreasury = 79;

		public const short PlunderTreasury = 80;

		public const short AidTreasury = 81;

		public const short CommitCrime = 82;

		public const short CaptureCriminals = 83;
	}

	public static class DefValue
	{
		public static FameActionItem Kill => Instance[(short)0];

		public static FameActionItem Kidnap => Instance[(short)2];

		public static FameActionItem Rescue => Instance[(short)4];

		public static FameActionItem MakeEnemy => Instance[(short)6];

		public static FameActionItem SeverEnemy => Instance[(short)8];

		public static FameActionItem MakeFriends => Instance[(short)10];

		public static FameActionItem MakeBrothers => Instance[(short)12];

		public static FameActionItem MakeLovers => Instance[(short)14];

		public static FameActionItem MakeBadLovers => Instance[(short)15];

		public static FameActionItem AdoptChild => Instance[(short)16];

		public static FameActionItem AdoptedAsChild => Instance[(short)18];

		public static FameActionItem GiveItem => Instance[(short)20];

		public static FameActionItem TeachSkill => Instance[(short)22];

		public static FameActionItem Heal => Instance[(short)24];

		public static FameActionItem HealBad => Instance[(short)25];

		public static FameActionItem SurpriseAttack => Instance[(short)26];

		public static FameActionItem DisciplineEvil => Instance[(short)27];

		public static FameActionItem StealSkill => Instance[(short)28];

		public static FameActionItem Steal => Instance[(short)29];

		public static FameActionItem Rob => Instance[(short)30];

		public static FameActionItem RobGrave => Instance[(short)31];

		public static FameActionItem Poison => Instance[(short)32];

		public static FameActionItem Escape => Instance[(short)33];

		public static FameActionItem FightChildren => Instance[(short)34];

		public static FameActionItem TakeAdventageOfOthers => Instance[(short)35];

		public static FameActionItem WinCombat => Instance[(short)36];

		public static FameActionItem LoseCombat => Instance[(short)37];

		public static FameActionItem ResponseKind => Instance[(short)38];

		public static FameActionItem ResponseJust => Instance[(short)41];

		public static FameActionItem ResponseRebel => Instance[(short)42];

		public static FameActionItem ResponseEgoistic => Instance[(short)44];

		public static FameActionItem RumorsAround => Instance[(short)46];

		public static FameActionItem EasyToPickOn => Instance[(short)47];

		public static FameActionItem GetAlms => Instance[(short)48];

		public static FameActionItem GetBlame => Instance[(short)49];

		public static FameActionItem GetFooled => Instance[(short)50];

		public static FameActionItem GetDuress => Instance[(short)51];

		public static FameActionItem GetPraised => Instance[(short)52];

		public static FameActionItem GetRidiculed => Instance[(short)53];

		public static FameActionItem InheritGoodOne => Instance[(short)54];

		public static FameActionItem InheritBadOne => Instance[(short)55];

		public static FameActionItem Indecent => Instance[(short)56];

		public static FameActionItem Beg => Instance[(short)57];

		public static FameActionItem Quack => Instance[(short)58];

		public static FameActionItem KillHeretic => Instance[(short)59];

		public static FameActionItem KillRighteous => Instance[(short)60];

		public static FameActionItem Immoral => Instance[(short)61];

		public static FameActionItem BreakRules => Instance[(short)62];

		public static FameActionItem Unethical => Instance[(short)63];

		public static FameActionItem Betrayal => Instance[(short)64];

		public static FameActionItem Unfaithful => Instance[(short)65];

		public static FameActionItem Rape => Instance[(short)66];

		public static FameActionItem BeSneered => Instance[(short)67];

		public static FameActionItem SneerSelf => Instance[(short)68];

		public static FameActionItem CombatWithStrong => Instance[(short)69];

		public static FameActionItem CombatWithWeak => Instance[(short)70];

		public static FameActionItem FriendWithGoodSects => Instance[(short)71];

		public static FameActionItem FriendWithEvilSects => Instance[(short)72];

		public static FameActionItem FriendWithNeutralSects1 => Instance[(short)73];

		public static FameActionItem FriendWithNeutralSects2 => Instance[(short)74];

		public static FameActionItem WinSkill => Instance[(short)75];

		public static FameActionItem LoseSkill => Instance[(short)76];

		public static FameActionItem MakeFamousItem => Instance[(short)77];

		public static FameActionItem DLCLoongDefeatLoong => Instance[(short)78];

		public static FameActionItem IntrudeTreasury => Instance[(short)79];

		public static FameActionItem PlunderTreasury => Instance[(short)80];

		public static FameActionItem AidTreasury => Instance[(short)81];

		public static FameActionItem CommitCrime => Instance[(short)82];

		public static FameActionItem CaptureCriminals => Instance[(short)83];
	}

	public static FameAction Instance = new FameAction();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "GoodJumpId", "BadJumpId", "NormalJumpId", "TemplateId", "Name", "Fame", "Duration", "RepeatType", "MaxStackCount", "ReductionTime" };

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
		_dataArray.Add(new FameActionItem(0, 0, -20, 120, 0, 100, 40, hasJump: true, 0, 1, 0, 300, 0));
		_dataArray.Add(new FameActionItem(1, 1, 3, 6, 1, 20, 3, hasJump: false, -1, -1, -1, 0, 300));
		_dataArray.Add(new FameActionItem(2, 2, -20, 120, 0, 100, 40, hasJump: true, 2, 3, 2, 300, 0));
		_dataArray.Add(new FameActionItem(3, 3, 3, 6, 1, 20, 3, hasJump: false, -1, -1, -1, 0, 300));
		_dataArray.Add(new FameActionItem(4, 4, 3, 6, 1, 20, 3, hasJump: true, 4, 5, 4, 0, 45));
		_dataArray.Add(new FameActionItem(5, 5, -3, 6, 1, 20, 18, hasJump: false, -1, -1, -1, 45, 0));
		_dataArray.Add(new FameActionItem(6, 6, -2, 12, 1, 10, 36, hasJump: true, 6, 7, 6, 30, 0));
		_dataArray.Add(new FameActionItem(7, 7, 2, 12, 1, 10, 6, hasJump: false, -1, -1, -1, 0, 30));
		_dataArray.Add(new FameActionItem(8, 7, 2, 12, 1, 10, 6, hasJump: true, 8, 9, 8, 0, 30));
		_dataArray.Add(new FameActionItem(9, 6, -2, 12, 1, 10, 36, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(10, 8, 2, 6, 1, 10, 3, hasJump: true, 10, 11, 10, 0, 30));
		_dataArray.Add(new FameActionItem(11, 9, -2, 6, 1, 10, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(12, 10, 4, 6, 1, 10, 3, hasJump: true, 12, 13, 12, 0, 60));
		_dataArray.Add(new FameActionItem(13, 11, -4, 6, 1, 10, 18, hasJump: false, -1, -1, -1, 60, 0));
		_dataArray.Add(new FameActionItem(14, 12, 6, 6, 1, 10, 3, hasJump: true, 14, 15, 14, 0, 90));
		_dataArray.Add(new FameActionItem(15, 13, -6, 6, 1, 10, 18, hasJump: false, -1, -1, -1, 90, 0));
		_dataArray.Add(new FameActionItem(16, 14, 3, 60, 0, 10, 10, hasJump: true, 16, 17, 16, 0, 45));
		_dataArray.Add(new FameActionItem(17, 15, -3, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 45, 0));
		_dataArray.Add(new FameActionItem(18, 16, 10, 120, 0, 5, 20, hasJump: true, 18, 19, 18, 0, 150));
		_dataArray.Add(new FameActionItem(19, 17, -10, 120, 0, 5, 40, hasJump: false, -1, -1, -1, 150, 0));
		_dataArray.Add(new FameActionItem(20, 18, 2, 6, 1, 20, 3, hasJump: true, 20, 21, 20, 0, 30));
		_dataArray.Add(new FameActionItem(21, 19, -2, 6, 1, 20, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(22, 20, 2, 6, 1, 20, 3, hasJump: true, 22, 23, 22, 0, 30));
		_dataArray.Add(new FameActionItem(23, 21, -2, 6, 1, 20, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(24, 22, 2, 6, 1, 20, 3, hasJump: true, 24, 25, 24, 0, 30));
		_dataArray.Add(new FameActionItem(25, 23, -2, 6, 1, 20, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(26, 24, -3, 120, 0, 20, 40, hasJump: true, 26, 27, 26, 45, 0));
		_dataArray.Add(new FameActionItem(27, 25, 3, 6, 1, 20, 3, hasJump: false, -1, -1, -1, 0, 45));
		_dataArray.Add(new FameActionItem(28, 26, -5, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 75, 0));
		_dataArray.Add(new FameActionItem(29, 27, -5, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 75, 0));
		_dataArray.Add(new FameActionItem(30, 28, -5, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 75, 0));
		_dataArray.Add(new FameActionItem(31, 29, -5, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 75, 0));
		_dataArray.Add(new FameActionItem(32, 30, -5, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 75, 0));
		_dataArray.Add(new FameActionItem(33, 31, -2, 6, 1, 20, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(34, 32, -2, 3, 1, 20, 9, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(35, 33, -2, 3, 1, 20, 9, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(36, 34, 3, 12, 1, 10, 6, hasJump: false, -1, -1, -1, 0, 45));
		_dataArray.Add(new FameActionItem(37, 35, -3, 12, 1, 10, 36, hasJump: false, -1, -1, -1, 45, 0));
		_dataArray.Add(new FameActionItem(38, 36, 2, 6, 1, 20, 3, hasJump: true, 38, 39, 38, 0, 30));
		_dataArray.Add(new FameActionItem(39, 37, -2, 6, 1, 20, 12, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(40, 39, -2, 6, 1, 20, 12, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(41, 38, 2, 6, 1, 20, 3, hasJump: true, 40, 41, 40, 0, 30));
		_dataArray.Add(new FameActionItem(42, 40, -2, 6, 1, 20, 12, hasJump: true, 42, 43, 42, 30, 0));
		_dataArray.Add(new FameActionItem(43, 41, 2, 6, 1, 20, 3, hasJump: false, -1, -1, -1, 0, 30));
		_dataArray.Add(new FameActionItem(44, 42, -2, 6, 1, 20, 12, hasJump: true, 44, 45, 44, 30, 0));
		_dataArray.Add(new FameActionItem(45, 43, 2, 6, 1, 20, 3, hasJump: false, -1, -1, -1, 0, 30));
		_dataArray.Add(new FameActionItem(46, 44, -2, 6, 1, 20, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(47, 45, -3, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 45, 0));
		_dataArray.Add(new FameActionItem(48, 46, -3, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 45, 0));
		_dataArray.Add(new FameActionItem(49, 47, -3, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 45, 0));
		_dataArray.Add(new FameActionItem(50, 48, -3, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 45, 0));
		_dataArray.Add(new FameActionItem(51, 49, -3, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 45, 0));
		_dataArray.Add(new FameActionItem(52, 50, 2, 6, 1, 10, 3, hasJump: false, -1, -1, -1, 0, 30));
		_dataArray.Add(new FameActionItem(53, 51, -2, 6, 1, 10, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(54, 52, 30, 120, 0, 3, 20, hasJump: false, -1, -1, -1, 0, 450));
		_dataArray.Add(new FameActionItem(55, 53, -30, 120, 0, 3, 20, hasJump: false, -1, -1, -1, 450, 0));
		_dataArray.Add(new FameActionItem(56, 54, -5, 12, 1, 10, 36, hasJump: false, -1, -1, -1, 75, 0));
		_dataArray.Add(new FameActionItem(57, 55, -5, 6, 1, 10, 18, hasJump: false, -1, -1, -1, 75, 0));
		_dataArray.Add(new FameActionItem(58, 56, -5, 6, 1, 10, 18, hasJump: false, -1, -1, -1, 75, 0));
		_dataArray.Add(new FameActionItem(59, 57, 2, 6, 1, 20, 3, hasJump: false, -1, -1, -1, 0, 30));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new FameActionItem(60, 58, -2, 6, 1, 20, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(61, 59, -3, 6, 1, 10, 18, hasJump: false, -1, -1, -1, 45, 0));
		_dataArray.Add(new FameActionItem(62, 60, -5, 60, 0, 10, 20, hasJump: false, -1, -1, -1, 75, 0));
		_dataArray.Add(new FameActionItem(63, 61, -10, 120, 0, 100, 40, hasJump: false, -1, -1, -1, 150, 0));
		_dataArray.Add(new FameActionItem(64, 62, -10, 6, 1, 5, 18, hasJump: false, -1, -1, -1, 150, 0));
		_dataArray.Add(new FameActionItem(65, 63, -2, 6, 1, 20, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(66, 64, -20, 120, 0, 100, 40, hasJump: false, -1, -1, -1, 300, 0));
		_dataArray.Add(new FameActionItem(67, 65, -2, 6, 1, 10, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(68, 66, -2, 6, 1, 10, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(69, 67, 5, 6, 1, 5, 18, hasJump: false, -1, -1, -1, 0, 75));
		_dataArray.Add(new FameActionItem(70, 68, -5, 6, 1, 5, 18, hasJump: false, -1, -1, -1, 75, 0));
		_dataArray.Add(new FameActionItem(71, 69, 1, 1, 0, 50, 0, hasJump: false, -1, -1, -1, 0, 0));
		_dataArray.Add(new FameActionItem(72, 70, -1, 1, 0, 50, 0, hasJump: false, -1, -1, -1, 0, 0));
		_dataArray.Add(new FameActionItem(73, 71, 1, 1, 0, 50, 0, hasJump: false, -1, -1, -1, 0, 0));
		_dataArray.Add(new FameActionItem(74, 71, -1, 1, 0, 50, 0, hasJump: false, -1, -1, -1, 0, 0));
		_dataArray.Add(new FameActionItem(75, 72, 3, 12, 1, 10, 6, hasJump: false, -1, -1, -1, 0, 45));
		_dataArray.Add(new FameActionItem(76, 73, -3, 12, 1, 10, 36, hasJump: false, -1, -1, -1, 45, 0));
		_dataArray.Add(new FameActionItem(77, 74, 4, 6, 1, 10, 18, hasJump: false, -1, -1, -1, 0, 60));
		_dataArray.Add(new FameActionItem(78, 75, 20, 60, 1, 5, 20, hasJump: false, -1, -1, -1, 0, 300));
		_dataArray.Add(new FameActionItem(79, 76, -30, 60, 1, 100, 20, hasJump: false, -1, -1, -1, 450, 0));
		_dataArray.Add(new FameActionItem(80, 77, -5, 36, 1, 50, 12, hasJump: false, -1, -1, -1, 75, 0));
		_dataArray.Add(new FameActionItem(81, 78, 3, 12, 0, 10, 6, hasJump: false, -1, -1, -1, 0, 75));
		_dataArray.Add(new FameActionItem(82, 79, -3, 12, 1, 50, 12, hasJump: false, -1, -1, -1, 45, 0));
		_dataArray.Add(new FameActionItem(83, 80, 3, 12, 1, 50, 12, hasJump: false, -1, -1, -1, 0, 45));
		_dataArray.Add(new FameActionItem(84, 81, -2, 6, 1, 20, 18, hasJump: false, -1, -1, -1, 30, 0));
		_dataArray.Add(new FameActionItem(85, 82, -2, 6, 1, 20, 18, hasJump: false, -1, -1, -1, 30, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<FameActionItem>(86);
		CreateItems0();
		CreateItems1();
	}
}
