using System.Collections.Generic;

namespace GameData.Domains.Organization;

public static class OrganizationDomainHelper
{
	public static class DataIds
	{
		public const ushort Sects = 0;

		public const ushort CivilianSettlements = 1;

		public const ushort NextSettlementId = 2;

		public const ushort SectCharacters = 3;

		public const ushort CivilianSettlementCharacters = 4;

		public const ushort Factions = 5;

		public const ushort LargeSectFavorabilities = 6;

		public const ushort MartialArtTournamentPreparationInfoList = 7;

		public const ushort PreviousMartialArtTournamentHosts = 8;
	}

	public static class MethodIds
	{
		public const ushort GetDisplayData = 0;

		public const ushort GetSettlementNameRelatedData = 1;

		public const ushort GetSettlementMembers = 2;

		public const ushort GetOrganizationCombatSkillsDisplayData = 3;

		public const ushort GetSectPreparationForMartialArtTournament = 4;

		public const ushort GetMartialArtTournamentCurrentHostSettlementId = 5;

		public const ushort GmCmd_SetAllSettlementInformationVisited = 6;

		public const ushort GmCmd_GetAllFactionMembers = 7;

		public const ushort GetSettlementIdByAreaIdAndBlockId = 8;

		public const ushort GetCultureByAreaIdAndBlockId = 9;

		public const ushort CalcApprovingRateEffectAuthorityGain = 10;

		public const ushort GetSettlementTreasuryDisplayData = 11;

		public const ushort GetSettlementTreasuryRecordCollection = 12;

		public const ushort ConfirmSettlementTreasuryOperation = 13;

		public const ushort SetInscribedCharactersForCreation = 14;

		public const ushort GmCmd_UpdateSettlementTreasury = 15;

		public const ushort GmCmd_ClearSettlementTreasuryAlertTime = 16;

		public const ushort GmCmd_ClearSettlementTreasuryItemAndResource = 17;

		public const ushort GmCmd_ForceUpdateTreasuryGuards = 18;

		public const ushort AddSectBounty = 19;

		public const ushort AddSectPrisoner = 20;

		public const ushort GetSettlementPrisonDisplayData = 21;

		public const ushort GetSettlementBountyDisplayData = 22;

		public const ushort GetSettlementPrisonRecordCollection = 23;

		public const ushort GmCmd_ForceUpdateInfluencePower = 24;

		public const ushort GetBountyCharacterDisplayDataFromCharacterList = 25;

		public const ushort ForceUpdateTaiwuVillager = 26;

		public const ushort IsTaiwuSectFugitive = 27;

		public const ushort GetOrganizationTemplateIdOfTaiwuLocation = 28;

		public const ushort GetLastSettlementTreasuryOperationData = 29;

		public const ushort GmCmd_GetSettlementPrisoner = 30;

		public const ushort CheckSettlementGuardFavorabilityType = 31;
	}

	public const ushort DataCount = 9;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "Sects", 0 },
		{ "CivilianSettlements", 1 },
		{ "NextSettlementId", 2 },
		{ "SectCharacters", 3 },
		{ "CivilianSettlementCharacters", 4 },
		{ "Factions", 5 },
		{ "LargeSectFavorabilities", 6 },
		{ "MartialArtTournamentPreparationInfoList", 7 },
		{ "PreviousMartialArtTournamentHosts", 8 }
	};

	public static readonly string[] DataId2FieldName = new string[9] { "Sects", "CivilianSettlements", "NextSettlementId", "SectCharacters", "CivilianSettlementCharacters", "Factions", "LargeSectFavorabilities", "MartialArtTournamentPreparationInfoList", "PreviousMartialArtTournamentHosts" };

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[9][]
	{
		SectHelper.FieldId2FieldName,
		CivilianSettlementHelper.FieldId2FieldName,
		null,
		SectCharacterHelper.FieldId2FieldName,
		CivilianSettlementCharacterHelper.FieldId2FieldName,
		null,
		null,
		null,
		null
	};

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "GetDisplayData", 0 },
		{ "GetSettlementNameRelatedData", 1 },
		{ "GetSettlementMembers", 2 },
		{ "GetOrganizationCombatSkillsDisplayData", 3 },
		{ "GetSectPreparationForMartialArtTournament", 4 },
		{ "GetMartialArtTournamentCurrentHostSettlementId", 5 },
		{ "GmCmd_SetAllSettlementInformationVisited", 6 },
		{ "GmCmd_GetAllFactionMembers", 7 },
		{ "GetSettlementIdByAreaIdAndBlockId", 8 },
		{ "GetCultureByAreaIdAndBlockId", 9 },
		{ "CalcApprovingRateEffectAuthorityGain", 10 },
		{ "GetSettlementTreasuryDisplayData", 11 },
		{ "GetSettlementTreasuryRecordCollection", 12 },
		{ "ConfirmSettlementTreasuryOperation", 13 },
		{ "SetInscribedCharactersForCreation", 14 },
		{ "GmCmd_UpdateSettlementTreasury", 15 },
		{ "GmCmd_ClearSettlementTreasuryAlertTime", 16 },
		{ "GmCmd_ClearSettlementTreasuryItemAndResource", 17 },
		{ "GmCmd_ForceUpdateTreasuryGuards", 18 },
		{ "AddSectBounty", 19 },
		{ "AddSectPrisoner", 20 },
		{ "GetSettlementPrisonDisplayData", 21 },
		{ "GetSettlementBountyDisplayData", 22 },
		{ "GetSettlementPrisonRecordCollection", 23 },
		{ "GmCmd_ForceUpdateInfluencePower", 24 },
		{ "GetBountyCharacterDisplayDataFromCharacterList", 25 },
		{ "ForceUpdateTaiwuVillager", 26 },
		{ "IsTaiwuSectFugitive", 27 },
		{ "GetOrganizationTemplateIdOfTaiwuLocation", 28 },
		{ "GetLastSettlementTreasuryOperationData", 29 },
		{ "GmCmd_GetSettlementPrisoner", 30 },
		{ "CheckSettlementGuardFavorabilityType", 31 }
	};

	public static readonly string[] MethodId2MethodName = new string[32]
	{
		"GetDisplayData", "GetSettlementNameRelatedData", "GetSettlementMembers", "GetOrganizationCombatSkillsDisplayData", "GetSectPreparationForMartialArtTournament", "GetMartialArtTournamentCurrentHostSettlementId", "GmCmd_SetAllSettlementInformationVisited", "GmCmd_GetAllFactionMembers", "GetSettlementIdByAreaIdAndBlockId", "GetCultureByAreaIdAndBlockId",
		"CalcApprovingRateEffectAuthorityGain", "GetSettlementTreasuryDisplayData", "GetSettlementTreasuryRecordCollection", "ConfirmSettlementTreasuryOperation", "SetInscribedCharactersForCreation", "GmCmd_UpdateSettlementTreasury", "GmCmd_ClearSettlementTreasuryAlertTime", "GmCmd_ClearSettlementTreasuryItemAndResource", "GmCmd_ForceUpdateTreasuryGuards", "AddSectBounty",
		"AddSectPrisoner", "GetSettlementPrisonDisplayData", "GetSettlementBountyDisplayData", "GetSettlementPrisonRecordCollection", "GmCmd_ForceUpdateInfluencePower", "GetBountyCharacterDisplayDataFromCharacterList", "ForceUpdateTaiwuVillager", "IsTaiwuSectFugitive", "GetOrganizationTemplateIdOfTaiwuLocation", "GetLastSettlementTreasuryOperationData",
		"GmCmd_GetSettlementPrisoner", "CheckSettlementGuardFavorabilityType"
	};
}
