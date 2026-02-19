using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class PersonalNeed : ConfigData<PersonalNeedItem, sbyte>
{
	public static class DefKey
	{
		public const sbyte IncreaseHappiness = 0;

		public const sbyte IncreaseHealth = 1;

		public const sbyte RestoreDisorderOfQi = 2;

		public const sbyte IncreaseNeili = 3;

		public const sbyte HealInjury = 4;

		public const sbyte HealPoison = 5;

		public const sbyte RecoverMainAttribute = 6;

		public const sbyte KillWug = 7;

		public const sbyte GainResource = 8;

		public const sbyte SpendResource = 9;

		public const sbyte GainItem = 10;

		public const sbyte RepairItem = 11;

		public const sbyte AddPoisonToItem = 12;

		public const sbyte SpendItem = 13;

		public const sbyte LearnCombatSkill = 14;

		public const sbyte LearnLifeSkill = 15;

		public const sbyte GainExp = 16;

		public const sbyte AskForHelpOnReading = 17;

		public const sbyte AskForHelpOnBreakout = 18;

		public const sbyte TakeCareOfOther = 19;

		public const sbyte TeamUp = 20;

		public const sbyte GetRevenge = 21;

		public const sbyte MournForTheDead = 22;

		public const sbyte MakeLove = 23;

		public const sbyte FindTreasure = 24;

		public const sbyte CreateRelation = 25;

		public const sbyte JoinOrganization = 26;
	}

	public static class DefValue
	{
		public static PersonalNeedItem IncreaseHappiness => Instance[(sbyte)0];

		public static PersonalNeedItem IncreaseHealth => Instance[(sbyte)1];

		public static PersonalNeedItem RestoreDisorderOfQi => Instance[(sbyte)2];

		public static PersonalNeedItem IncreaseNeili => Instance[(sbyte)3];

		public static PersonalNeedItem HealInjury => Instance[(sbyte)4];

		public static PersonalNeedItem HealPoison => Instance[(sbyte)5];

		public static PersonalNeedItem RecoverMainAttribute => Instance[(sbyte)6];

		public static PersonalNeedItem KillWug => Instance[(sbyte)7];

		public static PersonalNeedItem GainResource => Instance[(sbyte)8];

		public static PersonalNeedItem SpendResource => Instance[(sbyte)9];

		public static PersonalNeedItem GainItem => Instance[(sbyte)10];

		public static PersonalNeedItem RepairItem => Instance[(sbyte)11];

		public static PersonalNeedItem AddPoisonToItem => Instance[(sbyte)12];

		public static PersonalNeedItem SpendItem => Instance[(sbyte)13];

		public static PersonalNeedItem LearnCombatSkill => Instance[(sbyte)14];

		public static PersonalNeedItem LearnLifeSkill => Instance[(sbyte)15];

		public static PersonalNeedItem GainExp => Instance[(sbyte)16];

		public static PersonalNeedItem AskForHelpOnReading => Instance[(sbyte)17];

		public static PersonalNeedItem AskForHelpOnBreakout => Instance[(sbyte)18];

		public static PersonalNeedItem TakeCareOfOther => Instance[(sbyte)19];

		public static PersonalNeedItem TeamUp => Instance[(sbyte)20];

		public static PersonalNeedItem GetRevenge => Instance[(sbyte)21];

		public static PersonalNeedItem MournForTheDead => Instance[(sbyte)22];

		public static PersonalNeedItem MakeLove => Instance[(sbyte)23];

		public static PersonalNeedItem FindTreasure => Instance[(sbyte)24];

		public static PersonalNeedItem CreateRelation => Instance[(sbyte)25];

		public static PersonalNeedItem JoinOrganization => Instance[(sbyte)26];
	}

	public static PersonalNeed Instance = new PersonalNeed();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name" };

	internal override int ToInt(sbyte value)
	{
		return value;
	}

	internal override sbyte ToTemplateId(int value)
	{
		return (sbyte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new PersonalNeedItem(0, 0, matchType: false, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(1, 1, matchType: false, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(2, 2, matchType: false, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(3, 3, matchType: false, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(4, 4, matchType: true, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(5, 5, matchType: true, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(6, 6, matchType: true, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(7, 7, matchType: true, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(8, 8, matchType: true, overwrite: false, combine: true, 3));
		_dataArray.Add(new PersonalNeedItem(9, 9, matchType: true, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(10, 10, matchType: true, overwrite: false, combine: false, 6));
		_dataArray.Add(new PersonalNeedItem(11, 11, matchType: true, overwrite: false, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(12, 12, matchType: true, overwrite: false, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(13, 13, matchType: false, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(14, 14, matchType: true, overwrite: false, combine: false, 12));
		_dataArray.Add(new PersonalNeedItem(15, 15, matchType: true, overwrite: false, combine: false, 12));
		_dataArray.Add(new PersonalNeedItem(16, 16, matchType: false, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(17, 17, matchType: false, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(18, 18, matchType: false, overwrite: true, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(19, 19, matchType: false, overwrite: false, combine: false, 36));
		_dataArray.Add(new PersonalNeedItem(20, 20, matchType: false, overwrite: false, combine: false, 6));
		_dataArray.Add(new PersonalNeedItem(21, 21, matchType: false, overwrite: false, combine: false, 9));
		_dataArray.Add(new PersonalNeedItem(22, 22, matchType: false, overwrite: false, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(23, 23, matchType: false, overwrite: false, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(24, 24, matchType: false, overwrite: false, combine: false, 6));
		_dataArray.Add(new PersonalNeedItem(25, 25, matchType: true, overwrite: false, combine: false, 3));
		_dataArray.Add(new PersonalNeedItem(26, 26, matchType: false, overwrite: false, combine: false, 12));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<PersonalNeedItem>(27);
		CreateItems0();
	}
}
