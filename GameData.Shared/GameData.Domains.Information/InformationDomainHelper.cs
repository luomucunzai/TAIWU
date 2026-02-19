using System.Collections.Generic;

namespace GameData.Domains.Information;

public static class InformationDomainHelper
{
	public static class DataIds
	{
		public const ushort Information = 0;

		public const ushort SecretInformationCollection = 1;

		public const ushort SecretInformationMetaData = 2;

		public const ushort NextMetaDataId = 3;

		public const ushort CharacterSecretInformation = 4;

		public const ushort BroadcastSecretInformation = 5;

		public const ushort TaiwuReceivedNormalInformationInMonth = 6;

		public const ushort TaiwuReceivedSecretInformationInMonth = 7;

		public const ushort TaiwuReceivedInformation = 8;

		public const ushort TaiwuTmpInformation = 9;
	}

	public static class MethodIds
	{
		public const ushort GetCharacterNormalInformation = 0;

		public const ushort AddNormalInformationToCharacter = 1;

		public const ushort DeleteTmpInformation = 2;

		public const ushort GetNormalInformationUsedCount = 3;

		public const ushort GetSecretInformationDisplayPackage = 4;

		public const ushort GetSecretInformationDisplayPackageFromCharacter = 5;

		public const ushort GetSecretInformationDisplayPackageFromBroadcast = 6;

		public const ushort GetSecretInformationDisplayPackageForSelections = 7;

		public const ushort DiscardSecretInformation = 8;

		public const ushort GetSecretInformationDisplayData = 9;

		public const ushort GmCmd_CreateSecretInformationByCharacterIds = 10;

		public const ushort GmCmd_MakeCharacterReceiveSecretInformation = 11;

		public const ushort DisseminateSecretInformation = 12;

		public const ushort GetCharacterDisplayDataWithInfoList = 13;

		public const ushort GmCmd_MakeSecretInformationBroadcast = 14;

		public const ushort PerformProfessionLiteratiSkill3 = 15;

		public const ushort PerformProfessionLiteratiSkill2 = 16;

		public const ushort GetNormalInformationUsedCountAndMax = 17;

		public const ushort SettleSecretInformationShopTrade = 18;

		public const ushort GmCmd_DisseminationSecretInformationToRandomCharacters = 19;
	}

	public const ushort DataCount = 10;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "Information", 0 },
		{ "SecretInformationCollection", 1 },
		{ "SecretInformationMetaData", 2 },
		{ "NextMetaDataId", 3 },
		{ "CharacterSecretInformation", 4 },
		{ "BroadcastSecretInformation", 5 },
		{ "TaiwuReceivedNormalInformationInMonth", 6 },
		{ "TaiwuReceivedSecretInformationInMonth", 7 },
		{ "TaiwuReceivedInformation", 8 },
		{ "TaiwuTmpInformation", 9 }
	};

	public static readonly string[] DataId2FieldName = new string[10] { "Information", "SecretInformationCollection", "SecretInformationMetaData", "NextMetaDataId", "CharacterSecretInformation", "BroadcastSecretInformation", "TaiwuReceivedNormalInformationInMonth", "TaiwuReceivedSecretInformationInMonth", "TaiwuReceivedInformation", "TaiwuTmpInformation" };

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[10][]
	{
		null,
		null,
		SecretInformationMetaDataHelper.FieldId2FieldName,
		null,
		null,
		null,
		null,
		null,
		null,
		null
	};

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "GetCharacterNormalInformation", 0 },
		{ "AddNormalInformationToCharacter", 1 },
		{ "DeleteTmpInformation", 2 },
		{ "GetNormalInformationUsedCount", 3 },
		{ "GetSecretInformationDisplayPackage", 4 },
		{ "GetSecretInformationDisplayPackageFromCharacter", 5 },
		{ "GetSecretInformationDisplayPackageFromBroadcast", 6 },
		{ "GetSecretInformationDisplayPackageForSelections", 7 },
		{ "DiscardSecretInformation", 8 },
		{ "GetSecretInformationDisplayData", 9 },
		{ "GmCmd_CreateSecretInformationByCharacterIds", 10 },
		{ "GmCmd_MakeCharacterReceiveSecretInformation", 11 },
		{ "DisseminateSecretInformation", 12 },
		{ "GetCharacterDisplayDataWithInfoList", 13 },
		{ "GmCmd_MakeSecretInformationBroadcast", 14 },
		{ "PerformProfessionLiteratiSkill3", 15 },
		{ "PerformProfessionLiteratiSkill2", 16 },
		{ "GetNormalInformationUsedCountAndMax", 17 },
		{ "SettleSecretInformationShopTrade", 18 },
		{ "GmCmd_DisseminationSecretInformationToRandomCharacters", 19 }
	};

	public static readonly string[] MethodId2MethodName = new string[20]
	{
		"GetCharacterNormalInformation", "AddNormalInformationToCharacter", "DeleteTmpInformation", "GetNormalInformationUsedCount", "GetSecretInformationDisplayPackage", "GetSecretInformationDisplayPackageFromCharacter", "GetSecretInformationDisplayPackageFromBroadcast", "GetSecretInformationDisplayPackageForSelections", "DiscardSecretInformation", "GetSecretInformationDisplayData",
		"GmCmd_CreateSecretInformationByCharacterIds", "GmCmd_MakeCharacterReceiveSecretInformation", "DisseminateSecretInformation", "GetCharacterDisplayDataWithInfoList", "GmCmd_MakeSecretInformationBroadcast", "PerformProfessionLiteratiSkill3", "PerformProfessionLiteratiSkill2", "GetNormalInformationUsedCountAndMax", "SettleSecretInformationShopTrade", "GmCmd_DisseminationSecretInformationToRandomCharacters"
	};
}
