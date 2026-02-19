using System.Collections.Generic;

namespace GameData.Domains.Global;

public static class GlobalDomainHelper
{
	public static class DataIds
	{
		public const ushort Global = 0;

		public const ushort LoadedAllArchiveData = 1;

		public const ushort SavingWorld = 2;

		public const ushort InscribedCharacters = 3;

		public const ushort GlobalFlags = 4;
	}

	public static class MethodIds
	{
		public const ushort EnterNewWorld = 0;

		public const ushort LoadWorld = 1;

		public const ushort LoadEnding = 2;

		public const ushort SaveWorld = 3;

		public const ushort LeaveWorld = 4;

		public const ushort GetArchivesInfo = 5;

		public const ushort DeleteArchive = 6;

		public const ushort InscribeCharacter = 7;

		public const ushort RemoveInscribedCharacter = 8;

		public const ushort SetGameVersion = 9;

		public const ushort SetCompressionLevel = 10;

		public const ushort PackAllCrossArchiveGameData = 11;

		public const ushort SetGlobalFlag = 12;

		public const ushort GetGlobalFlag = 13;

		public const ushort CheckDriveSpace = 14;

		public const ushort UpdateSharedGlobalSettings = 15;

		public const ushort ReloadAllConfigData = 16;
	}

	public const ushort DataCount = 5;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "Global", 0 },
		{ "LoadedAllArchiveData", 1 },
		{ "SavingWorld", 2 },
		{ "InscribedCharacters", 3 },
		{ "GlobalFlags", 4 }
	};

	public static readonly string[] DataId2FieldName = new string[5] { "Global", "LoadedAllArchiveData", "SavingWorld", "InscribedCharacters", "GlobalFlags" };

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[5][];

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "EnterNewWorld", 0 },
		{ "LoadWorld", 1 },
		{ "LoadEnding", 2 },
		{ "SaveWorld", 3 },
		{ "LeaveWorld", 4 },
		{ "GetArchivesInfo", 5 },
		{ "DeleteArchive", 6 },
		{ "InscribeCharacter", 7 },
		{ "RemoveInscribedCharacter", 8 },
		{ "SetGameVersion", 9 },
		{ "SetCompressionLevel", 10 },
		{ "PackAllCrossArchiveGameData", 11 },
		{ "SetGlobalFlag", 12 },
		{ "GetGlobalFlag", 13 },
		{ "CheckDriveSpace", 14 },
		{ "UpdateSharedGlobalSettings", 15 },
		{ "ReloadAllConfigData", 16 }
	};

	public static readonly string[] MethodId2MethodName = new string[17]
	{
		"EnterNewWorld", "LoadWorld", "LoadEnding", "SaveWorld", "LeaveWorld", "GetArchivesInfo", "DeleteArchive", "InscribeCharacter", "RemoveInscribedCharacter", "SetGameVersion",
		"SetCompressionLevel", "PackAllCrossArchiveGameData", "SetGlobalFlag", "GetGlobalFlag", "CheckDriveSpace", "UpdateSharedGlobalSettings", "ReloadAllConfigData"
	};
}
