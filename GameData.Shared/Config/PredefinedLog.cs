using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class PredefinedLog : ConfigData<PredefinedLogItem, short>
{
	public static class DefKey
	{
		public const short SecretInformationForcedToBeErased = 0;

		public const short LoadModConfigFail = 1;

		public const short LoadModConfigFailWithoutName = 2;

		public const short LoadModSettingsFail = 3;

		public const short LoadModSystemSettingsFail = 4;

		public const short CombatFrameConfigNegative = 5;

		public const short ProfessionSkillAnimationRuntimeException = 6;

		public const short CombatSpecialEffectRuntimeException = 7;

		public const short CombatRuntimeException = 8;

		public const short VideoFormatUnSupportedException = 9;

		public const short TaskTemplateIdNotExist = 10;

		public const short MapRuntimeException = 11;

		public const short CharacterRuntimeException = 12;

		public const short CombatSkillRuntimeException = 13;

		public const short NetRuntimeException = 14;

		public const short LifeSkillCombatOperationExpired = 15;

		public const short DreamBackWorldNotMatch = 16;

		public const short JiaoChangeloongRefreshIndexException = 17;

		public const short GeneralRefreshIndexException = 18;

		public const short WorldRuntimeException = 19;

		public const short DuplicatedItemDetected = 20;

		public const short UnownedItemDetected = 21;

		public const short EventChangedWithNotEntering = 22;

		public const short CombatAiLoadException = 23;

		public const short ModConfigNotFoundException = 24;

		public const short ValueCanNotBeEmpty = 25;

		public const short ValueNotFoundInConfigTable = 26;

		public const short EventNotFoundInAnyEventGroup = 27;

		public const short InvalidEnumIndex = 28;

		public const short LuaScriptingException = 29;

		public const short CacheCorruptedNoChickenInSettlement = 30;

		public const short CacheCorruptedSetSettlementHasNoChicken = 31;

		public const short CacheCorruptedRemoveNonExistsChicken = 32;

		public const short UnknownPunishmentType = 33;

		public const short GetNotAliveDisplayingAge = 34;

		public const short ConfigurationParseFailed = 35;
	}

	public static class DefValue
	{
		public static PredefinedLogItem SecretInformationForcedToBeErased => Instance[(short)0];

		public static PredefinedLogItem LoadModConfigFail => Instance[(short)1];

		public static PredefinedLogItem LoadModConfigFailWithoutName => Instance[(short)2];

		public static PredefinedLogItem LoadModSettingsFail => Instance[(short)3];

		public static PredefinedLogItem LoadModSystemSettingsFail => Instance[(short)4];

		public static PredefinedLogItem CombatFrameConfigNegative => Instance[(short)5];

		public static PredefinedLogItem ProfessionSkillAnimationRuntimeException => Instance[(short)6];

		public static PredefinedLogItem CombatSpecialEffectRuntimeException => Instance[(short)7];

		public static PredefinedLogItem CombatRuntimeException => Instance[(short)8];

		public static PredefinedLogItem VideoFormatUnSupportedException => Instance[(short)9];

		public static PredefinedLogItem TaskTemplateIdNotExist => Instance[(short)10];

		public static PredefinedLogItem MapRuntimeException => Instance[(short)11];

		public static PredefinedLogItem CharacterRuntimeException => Instance[(short)12];

		public static PredefinedLogItem CombatSkillRuntimeException => Instance[(short)13];

		public static PredefinedLogItem NetRuntimeException => Instance[(short)14];

		public static PredefinedLogItem LifeSkillCombatOperationExpired => Instance[(short)15];

		public static PredefinedLogItem DreamBackWorldNotMatch => Instance[(short)16];

		public static PredefinedLogItem JiaoChangeloongRefreshIndexException => Instance[(short)17];

		public static PredefinedLogItem GeneralRefreshIndexException => Instance[(short)18];

		public static PredefinedLogItem WorldRuntimeException => Instance[(short)19];

		public static PredefinedLogItem DuplicatedItemDetected => Instance[(short)20];

		public static PredefinedLogItem UnownedItemDetected => Instance[(short)21];

		public static PredefinedLogItem EventChangedWithNotEntering => Instance[(short)22];

		public static PredefinedLogItem CombatAiLoadException => Instance[(short)23];

		public static PredefinedLogItem ModConfigNotFoundException => Instance[(short)24];

		public static PredefinedLogItem ValueCanNotBeEmpty => Instance[(short)25];

		public static PredefinedLogItem ValueNotFoundInConfigTable => Instance[(short)26];

		public static PredefinedLogItem EventNotFoundInAnyEventGroup => Instance[(short)27];

		public static PredefinedLogItem InvalidEnumIndex => Instance[(short)28];

		public static PredefinedLogItem LuaScriptingException => Instance[(short)29];

		public static PredefinedLogItem CacheCorruptedNoChickenInSettlement => Instance[(short)30];

		public static PredefinedLogItem CacheCorruptedSetSettlementHasNoChicken => Instance[(short)31];

		public static PredefinedLogItem CacheCorruptedRemoveNonExistsChicken => Instance[(short)32];

		public static PredefinedLogItem UnknownPunishmentType => Instance[(short)33];

		public static PredefinedLogItem GetNotAliveDisplayingAge => Instance[(short)34];

		public static PredefinedLogItem ConfigurationParseFailed => Instance[(short)35];
	}

	public static PredefinedLog Instance = new PredefinedLog();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Info" };

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
		_dataArray.Add(new PredefinedLogItem(0, 0, 1));
		_dataArray.Add(new PredefinedLogItem(1, 2, 3));
		_dataArray.Add(new PredefinedLogItem(2, 2, 4));
		_dataArray.Add(new PredefinedLogItem(3, 5, 6));
		_dataArray.Add(new PredefinedLogItem(4, 7, 8));
		_dataArray.Add(new PredefinedLogItem(5, 9, 10));
		_dataArray.Add(new PredefinedLogItem(6, 11, 12));
		_dataArray.Add(new PredefinedLogItem(7, 13, 14));
		_dataArray.Add(new PredefinedLogItem(8, 15, 16));
		_dataArray.Add(new PredefinedLogItem(9, 17, 18));
		_dataArray.Add(new PredefinedLogItem(10, 19, 20));
		_dataArray.Add(new PredefinedLogItem(11, 21, 22));
		_dataArray.Add(new PredefinedLogItem(12, 23, 24));
		_dataArray.Add(new PredefinedLogItem(13, 25, 26));
		_dataArray.Add(new PredefinedLogItem(14, 27, 28));
		_dataArray.Add(new PredefinedLogItem(15, 29, 30));
		_dataArray.Add(new PredefinedLogItem(16, 31, 32));
		_dataArray.Add(new PredefinedLogItem(17, 33, 34));
		_dataArray.Add(new PredefinedLogItem(18, 35, 36));
		_dataArray.Add(new PredefinedLogItem(19, 37, 38));
		_dataArray.Add(new PredefinedLogItem(20, 39, 40));
		_dataArray.Add(new PredefinedLogItem(21, 41, 42));
		_dataArray.Add(new PredefinedLogItem(22, 43, 44));
		_dataArray.Add(new PredefinedLogItem(23, 45, 46));
		_dataArray.Add(new PredefinedLogItem(24, 47, 48));
		_dataArray.Add(new PredefinedLogItem(25, 49, 49));
		_dataArray.Add(new PredefinedLogItem(26, 50, 51));
		_dataArray.Add(new PredefinedLogItem(27, 52, 53));
		_dataArray.Add(new PredefinedLogItem(28, 54, 55));
		_dataArray.Add(new PredefinedLogItem(29, 56, 57));
		_dataArray.Add(new PredefinedLogItem(30, 58, 59));
		_dataArray.Add(new PredefinedLogItem(31, 58, 60));
		_dataArray.Add(new PredefinedLogItem(32, 61, 62));
		_dataArray.Add(new PredefinedLogItem(33, 63, 64));
		_dataArray.Add(new PredefinedLogItem(34, 65, 66));
		_dataArray.Add(new PredefinedLogItem(35, 67, 68));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<PredefinedLogItem>(36);
		CreateItems0();
	}

	public static void Show(short predefinedLogId)
	{
		Instance[predefinedLogId].Log();
	}

	public static void Show(short predefinedLogId, object arg0)
	{
		Instance[predefinedLogId].Log(arg0);
	}

	public static void Show(short predefinedLogId, object arg0, object arg1)
	{
		Instance[predefinedLogId].Log(arg0, arg1);
	}

	public static void Show(short predefinedLogId, object arg0, object arg1, object arg2)
	{
		Instance[predefinedLogId].Log(arg0, arg1, arg2);
	}

	public static void Show(short predefinedLogId, params object[] parameters)
	{
		Instance[predefinedLogId].Log(parameters);
	}
}
