using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LifeSkillCombatTalk : ConfigData<LifeSkillCombatTalkItem, short>
{
	public static class DefKey
	{
		public const short Prepare_Prologue = 0;

		public const short Prepare_Ban = 1;

		public const short Prepare_Verify = 2;

		public const short Prepare_Give_Away = 3;

		public const short Prepare_Secret_Succeeded = 4;

		public const short Prepare_Tempt_Succeeded = 5;

		public const short Prepare_Secret_Failed = 6;

		public const short Prepare_Tempt_Failed = 7;

		public const short Prepare_Ai_Secret = 8;

		public const short Prepare_Ai_Tempt = 9;

		public const short Prepare_Decide_Theme = 10;

		public const short Combat_Prologue = 11;

		public const short Combat_CreateUnit = 12;

		public const short Combat_Conflict_Win = 13;

		public const short Combat_Conflict_Lose = 14;

		public const short Combat_Give_In = 15;

		public const short Combat_Force_Give_In = 16;

		public const short Combat_Refuse_Force_Give_In = 17;

		public const short Combat_Failed = 18;

		public const short Combat_Succeeded = 19;
	}

	public static class DefValue
	{
		public static LifeSkillCombatTalkItem Prepare_Prologue => Instance[(short)0];

		public static LifeSkillCombatTalkItem Prepare_Ban => Instance[(short)1];

		public static LifeSkillCombatTalkItem Prepare_Verify => Instance[(short)2];

		public static LifeSkillCombatTalkItem Prepare_Give_Away => Instance[(short)3];

		public static LifeSkillCombatTalkItem Prepare_Secret_Succeeded => Instance[(short)4];

		public static LifeSkillCombatTalkItem Prepare_Tempt_Succeeded => Instance[(short)5];

		public static LifeSkillCombatTalkItem Prepare_Secret_Failed => Instance[(short)6];

		public static LifeSkillCombatTalkItem Prepare_Tempt_Failed => Instance[(short)7];

		public static LifeSkillCombatTalkItem Prepare_Ai_Secret => Instance[(short)8];

		public static LifeSkillCombatTalkItem Prepare_Ai_Tempt => Instance[(short)9];

		public static LifeSkillCombatTalkItem Prepare_Decide_Theme => Instance[(short)10];

		public static LifeSkillCombatTalkItem Combat_Prologue => Instance[(short)11];

		public static LifeSkillCombatTalkItem Combat_CreateUnit => Instance[(short)12];

		public static LifeSkillCombatTalkItem Combat_Conflict_Win => Instance[(short)13];

		public static LifeSkillCombatTalkItem Combat_Conflict_Lose => Instance[(short)14];

		public static LifeSkillCombatTalkItem Combat_Give_In => Instance[(short)15];

		public static LifeSkillCombatTalkItem Combat_Force_Give_In => Instance[(short)16];

		public static LifeSkillCombatTalkItem Combat_Refuse_Force_Give_In => Instance[(short)17];

		public static LifeSkillCombatTalkItem Combat_Failed => Instance[(short)18];

		public static LifeSkillCombatTalkItem Combat_Succeeded => Instance[(short)19];
	}

	public static LifeSkillCombatTalk Instance = new LifeSkillCombatTalk();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "NormalContent", "JustContent", "KindContent", "EvenContent", "RebelContent", "EgoisticContent" };

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
		_dataArray.Add(new LifeSkillCombatTalkItem(0, 0, 1, needRepalceType: false, 2, 3, 4, 5, 6));
		_dataArray.Add(new LifeSkillCombatTalkItem(1, 7, 8, needRepalceType: false, 9, 10, 11, 12, 13));
		_dataArray.Add(new LifeSkillCombatTalkItem(2, 14, 15, needRepalceType: false, 16, 17, 18, 19, 20));
		_dataArray.Add(new LifeSkillCombatTalkItem(3, 21, 22, needRepalceType: false, 23, 24, 25, 26, 27));
		_dataArray.Add(new LifeSkillCombatTalkItem(4, 28, 29, needRepalceType: false, 30, 31, 32, 33, 34));
		_dataArray.Add(new LifeSkillCombatTalkItem(5, 35, 36, needRepalceType: false, 37, 38, 39, 40, 41));
		_dataArray.Add(new LifeSkillCombatTalkItem(6, 42, 43, needRepalceType: false, 44, 45, 46, 47, 48));
		_dataArray.Add(new LifeSkillCombatTalkItem(7, 49, 50, needRepalceType: false, 51, 52, 53, 54, 55));
		_dataArray.Add(new LifeSkillCombatTalkItem(8, 56, 57, needRepalceType: false, 58, 59, 60, 61, 62));
		_dataArray.Add(new LifeSkillCombatTalkItem(9, 63, 64, needRepalceType: false, 65, 66, 67, 68, 69));
		_dataArray.Add(new LifeSkillCombatTalkItem(10, 70, 71, needRepalceType: false, 72, 73, 74, 75, 76));
		_dataArray.Add(new LifeSkillCombatTalkItem(11, 77, 78, needRepalceType: false, 79, 80, 81, 82, 83));
		_dataArray.Add(new LifeSkillCombatTalkItem(12, 84, 85, needRepalceType: true, 86, 87, 88, 89, 90));
		_dataArray.Add(new LifeSkillCombatTalkItem(13, 91, 92, needRepalceType: true, 93, 94, 95, 96, 97));
		_dataArray.Add(new LifeSkillCombatTalkItem(14, 98, 99, needRepalceType: true, 100, 101, 102, 103, 104));
		_dataArray.Add(new LifeSkillCombatTalkItem(15, 105, 106, needRepalceType: false, 107, 108, 109, 110, 111));
		_dataArray.Add(new LifeSkillCombatTalkItem(16, 112, 113, needRepalceType: false, 114, 115, 116, 117, 118));
		_dataArray.Add(new LifeSkillCombatTalkItem(17, 119, 120, needRepalceType: false, 121, 122, 123, 124, 125));
		_dataArray.Add(new LifeSkillCombatTalkItem(18, 126, 127, needRepalceType: false, 128, 129, 130, 131, 132));
		_dataArray.Add(new LifeSkillCombatTalkItem(19, 133, 134, needRepalceType: false, 135, 136, 137, 138, 139));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<LifeSkillCombatTalkItem>(20);
		CreateItems0();
	}
}
