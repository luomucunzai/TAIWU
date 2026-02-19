using System.Collections.Generic;

namespace GameData.Domains.TaiwuEvent;

public static class TaiwuEventDomainHelper
{
	public static class DataIds
	{
		public const ushort GlobalArgBox = 0;

		public const ushort MonthlyEventActionManager = 1;

		public const ushort AwayForeverLoverCharId = 2;

		public const ushort EventCount = 3;

		public const ushort HealDoctorCharId = 4;

		public const ushort CgName = 5;

		public const ushort NotifyData = 6;

		public const ushort HasListeningEvent = 7;

		public const ushort SelectInformationData = 8;

		public const ushort TaiwuLocationChangeFlag = 9;

		public const ushort SecretVillageOnFire = 10;

		public const ushort TaiwuVillageShowShrine = 11;

		public const ushort HideAllTeammates = 12;

		public const ushort LeftRoleAlternativeName = 13;

		public const ushort RightRoleAlternativeName = 14;

		public const ushort RightRoleXiangshuDisplayData = 15;

		public const ushort SelectCombatSkillData = 16;

		public const ushort SelectLifeSkillData = 17;

		public const ushort ItemListOfLeft = 18;

		public const ushort ItemListOfRight = 19;

		public const ushort ShowItemWithCricketBattleGuess = 20;

		public const ushort DisplayingEventData = 21;

		public const ushort TempCreateItemList = 22;

		public const ushort CoverCricketJarGradeListForRight = 23;

		public const ushort MarriageLook1CharIdList = 24;

		public const ushort MarriageLook2CharIdList = 25;

		public const ushort AllCombatGroupChars = 26;

		public const ushort CricketBettingData = 27;

		public const ushort JieqingMaskCharIdList = 28;
	}

	public static class MethodIds
	{
		public const ushort GetMonthlyActionStateAndTime = 0;

		public const ushort InitConchShipEvents = 1;

		public const ushort TriggerListener = 2;

		public const ushort SetItemSelectResult = 3;

		public const ushort SetCharacterSelectResult = 4;

		public const ushort SetSecretInformationSelectResult = 5;

		public const ushort SetNormalInformationSelectResult = 6;

		public const ushort StartHandleEventDuringAdvance = 7;

		public const ushort GetTriggeredEventSummaryDisplayData = 8;

		public const ushort SetEventInProcessing = 9;

		public const ushort EventSelect = 10;

		public const ushort GetEventDisplayData = 11;

		public const ushort GmCmd_SaveMonthlyActionManager = 12;

		public const ushort OnCharacterClicked = 13;

		public const ushort OnLetTeammateLeaveGroup = 14;

		public const ushort OnInteractCaravan = 15;

		public const ushort OnInteractKidnappedCharacter = 16;

		public const ushort OnSectBuildingClicked = 17;

		public const ushort OnRecordEnterGame = 18;

		public const ushort OnNewGameMonth = 19;

		public const ushort OnCombatWithXiangshuMinionComplete = 20;

		public const ushort OnBlackMaskAnimationComplete = 21;

		public const ushort OnMakingSystemOpened = 22;

		public const ushort OnCollectedMakingSystemItem = 23;

		public const ushort OnSectSpecialBuildingClicked = 24;

		public const ushort AnimalAvatarClicked = 25;

		public const ushort MainStoryFinishCatchCricket = 26;

		public const ushort LoadEventsFromPath = 27;

		public const ushort NpcTombClicked = 28;

		public const ushort SetLifeSkillSelectResult = 29;

		public const ushort SetCombatSkillSelectResult = 30;

		public const ushort OnLifeSkillCombatForceSilent = 31;

		public const ushort TryMoveWhenMoveDisable = 32;

		public const ushort TryMoveToInvalidLocationInTutorial = 33;

		public const ushort SetCharacterSetSelectResult = 34;

		public const ushort OnCharacterTemplateClicked = 35;

		public const ushort CloseUI = 36;

		public const ushort SetIsQuickStartGame = 37;

		public const ushort TaiwuCollectWudangHeavenlyTreeSeed = 38;

		public const ushort GetEventLogData = 39;

		public const ushort StartNewDialog = 40;

		public const ushort TaiwuVillagerExpelled = 41;

		public const ushort GmCmd_TaiwuCrossArchive = 42;

		public const ushort TaiwuCrossArchiveFindMemory = 43;

		public const ushort UserLoadDreamBackArchive = 44;

		public const ushort OperateInventoryItem = 45;

		public const ushort SetItemSelectCount = 46;

		public const ushort SettlementTreasuryBuildingClicked = 47;

		public const ushort SetListenerEventActionISerializableArg = 48;

		public const ushort SetListenerEventActionIntArg = 49;

		public const ushort SetListenerEventActionBoolArg = 50;

		public const ushort SetListenerEventActionStringArg = 51;

		public const ushort GetValidInteractionEventOptions = 52;

		public const ushort SetListenerEventActionIntListArg = 53;

		public const ushort SetListenerEventActionItemKeyArg = 54;

		public const ushort TriggerShixiangDrumEasterEgg = 55;

		public const ushort InteractPrisoner = 56;

		public const ushort OnClickedSendPrisonBtn = 57;

		public const ushort OnClickedPrisonBtn = 58;

		public const ushort SetCharacterMultSelectResult = 59;

		public const ushort SetCricketBettingResult = 60;

		public const ushort GetImplementedFunctionIds = 61;

		public const ushort SetEventScriptExecutionPause = 62;

		public const ushort EventScriptExecuteNext = 63;

		public const ushort GmCmd_TaiwuWantedSectPunished = 64;

		public const ushort EventSelectContinue = 65;

		public const ushort SetSelectCount = 66;

		public const ushort SetListenerEventActionShortListArg = 67;

		public const ushort SetShowingEventShortListArg = 68;

		public const ushort OnClickMapPickupEvent = 69;

		public const ushort OnClickMapPickupNormalEvent = 70;

		public const ushort OnClickDeportButton = 71;

		public const ushort OnSwitchToGuardedPage = 72;

		public const ushort GmCmd_AddJieqingMaskCharId = 73;

		public const ushort GmCmd_RemoveJieqingMaskCharId = 74;
	}

	public const ushort DataCount = 29;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "GlobalArgBox", 0 },
		{ "MonthlyEventActionManager", 1 },
		{ "AwayForeverLoverCharId", 2 },
		{ "EventCount", 3 },
		{ "HealDoctorCharId", 4 },
		{ "CgName", 5 },
		{ "NotifyData", 6 },
		{ "HasListeningEvent", 7 },
		{ "SelectInformationData", 8 },
		{ "TaiwuLocationChangeFlag", 9 },
		{ "SecretVillageOnFire", 10 },
		{ "TaiwuVillageShowShrine", 11 },
		{ "HideAllTeammates", 12 },
		{ "LeftRoleAlternativeName", 13 },
		{ "RightRoleAlternativeName", 14 },
		{ "RightRoleXiangshuDisplayData", 15 },
		{ "SelectCombatSkillData", 16 },
		{ "SelectLifeSkillData", 17 },
		{ "ItemListOfLeft", 18 },
		{ "ItemListOfRight", 19 },
		{ "ShowItemWithCricketBattleGuess", 20 },
		{ "DisplayingEventData", 21 },
		{ "TempCreateItemList", 22 },
		{ "CoverCricketJarGradeListForRight", 23 },
		{ "MarriageLook1CharIdList", 24 },
		{ "MarriageLook2CharIdList", 25 },
		{ "AllCombatGroupChars", 26 },
		{ "CricketBettingData", 27 },
		{ "JieqingMaskCharIdList", 28 }
	};

	public static readonly string[] DataId2FieldName = new string[29]
	{
		"GlobalArgBox", "MonthlyEventActionManager", "AwayForeverLoverCharId", "EventCount", "HealDoctorCharId", "CgName", "NotifyData", "HasListeningEvent", "SelectInformationData", "TaiwuLocationChangeFlag",
		"SecretVillageOnFire", "TaiwuVillageShowShrine", "HideAllTeammates", "LeftRoleAlternativeName", "RightRoleAlternativeName", "RightRoleXiangshuDisplayData", "SelectCombatSkillData", "SelectLifeSkillData", "ItemListOfLeft", "ItemListOfRight",
		"ShowItemWithCricketBattleGuess", "DisplayingEventData", "TempCreateItemList", "CoverCricketJarGradeListForRight", "MarriageLook1CharIdList", "MarriageLook2CharIdList", "AllCombatGroupChars", "CricketBettingData", "JieqingMaskCharIdList"
	};

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[29][];

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "GetMonthlyActionStateAndTime", 0 },
		{ "InitConchShipEvents", 1 },
		{ "TriggerListener", 2 },
		{ "SetItemSelectResult", 3 },
		{ "SetCharacterSelectResult", 4 },
		{ "SetSecretInformationSelectResult", 5 },
		{ "SetNormalInformationSelectResult", 6 },
		{ "StartHandleEventDuringAdvance", 7 },
		{ "GetTriggeredEventSummaryDisplayData", 8 },
		{ "SetEventInProcessing", 9 },
		{ "EventSelect", 10 },
		{ "GetEventDisplayData", 11 },
		{ "GmCmd_SaveMonthlyActionManager", 12 },
		{ "OnCharacterClicked", 13 },
		{ "OnLetTeammateLeaveGroup", 14 },
		{ "OnInteractCaravan", 15 },
		{ "OnInteractKidnappedCharacter", 16 },
		{ "OnSectBuildingClicked", 17 },
		{ "OnRecordEnterGame", 18 },
		{ "OnNewGameMonth", 19 },
		{ "OnCombatWithXiangshuMinionComplete", 20 },
		{ "OnBlackMaskAnimationComplete", 21 },
		{ "OnMakingSystemOpened", 22 },
		{ "OnCollectedMakingSystemItem", 23 },
		{ "OnSectSpecialBuildingClicked", 24 },
		{ "AnimalAvatarClicked", 25 },
		{ "MainStoryFinishCatchCricket", 26 },
		{ "LoadEventsFromPath", 27 },
		{ "NpcTombClicked", 28 },
		{ "SetLifeSkillSelectResult", 29 },
		{ "SetCombatSkillSelectResult", 30 },
		{ "OnLifeSkillCombatForceSilent", 31 },
		{ "TryMoveWhenMoveDisable", 32 },
		{ "TryMoveToInvalidLocationInTutorial", 33 },
		{ "SetCharacterSetSelectResult", 34 },
		{ "OnCharacterTemplateClicked", 35 },
		{ "CloseUI", 36 },
		{ "SetIsQuickStartGame", 37 },
		{ "TaiwuCollectWudangHeavenlyTreeSeed", 38 },
		{ "GetEventLogData", 39 },
		{ "StartNewDialog", 40 },
		{ "TaiwuVillagerExpelled", 41 },
		{ "GmCmd_TaiwuCrossArchive", 42 },
		{ "TaiwuCrossArchiveFindMemory", 43 },
		{ "UserLoadDreamBackArchive", 44 },
		{ "OperateInventoryItem", 45 },
		{ "SetItemSelectCount", 46 },
		{ "SettlementTreasuryBuildingClicked", 47 },
		{ "SetListenerEventActionISerializableArg", 48 },
		{ "SetListenerEventActionIntArg", 49 },
		{ "SetListenerEventActionBoolArg", 50 },
		{ "SetListenerEventActionStringArg", 51 },
		{ "GetValidInteractionEventOptions", 52 },
		{ "SetListenerEventActionIntListArg", 53 },
		{ "SetListenerEventActionItemKeyArg", 54 },
		{ "TriggerShixiangDrumEasterEgg", 55 },
		{ "InteractPrisoner", 56 },
		{ "OnClickedSendPrisonBtn", 57 },
		{ "OnClickedPrisonBtn", 58 },
		{ "SetCharacterMultSelectResult", 59 },
		{ "SetCricketBettingResult", 60 },
		{ "GetImplementedFunctionIds", 61 },
		{ "SetEventScriptExecutionPause", 62 },
		{ "EventScriptExecuteNext", 63 },
		{ "GmCmd_TaiwuWantedSectPunished", 64 },
		{ "EventSelectContinue", 65 },
		{ "SetSelectCount", 66 },
		{ "SetListenerEventActionShortListArg", 67 },
		{ "SetShowingEventShortListArg", 68 },
		{ "OnClickMapPickupEvent", 69 },
		{ "OnClickMapPickupNormalEvent", 70 },
		{ "OnClickDeportButton", 71 },
		{ "OnSwitchToGuardedPage", 72 },
		{ "GmCmd_AddJieqingMaskCharId", 73 },
		{ "GmCmd_RemoveJieqingMaskCharId", 74 }
	};

	public static readonly string[] MethodId2MethodName = new string[75]
	{
		"GetMonthlyActionStateAndTime", "InitConchShipEvents", "TriggerListener", "SetItemSelectResult", "SetCharacterSelectResult", "SetSecretInformationSelectResult", "SetNormalInformationSelectResult", "StartHandleEventDuringAdvance", "GetTriggeredEventSummaryDisplayData", "SetEventInProcessing",
		"EventSelect", "GetEventDisplayData", "GmCmd_SaveMonthlyActionManager", "OnCharacterClicked", "OnLetTeammateLeaveGroup", "OnInteractCaravan", "OnInteractKidnappedCharacter", "OnSectBuildingClicked", "OnRecordEnterGame", "OnNewGameMonth",
		"OnCombatWithXiangshuMinionComplete", "OnBlackMaskAnimationComplete", "OnMakingSystemOpened", "OnCollectedMakingSystemItem", "OnSectSpecialBuildingClicked", "AnimalAvatarClicked", "MainStoryFinishCatchCricket", "LoadEventsFromPath", "NpcTombClicked", "SetLifeSkillSelectResult",
		"SetCombatSkillSelectResult", "OnLifeSkillCombatForceSilent", "TryMoveWhenMoveDisable", "TryMoveToInvalidLocationInTutorial", "SetCharacterSetSelectResult", "OnCharacterTemplateClicked", "CloseUI", "SetIsQuickStartGame", "TaiwuCollectWudangHeavenlyTreeSeed", "GetEventLogData",
		"StartNewDialog", "TaiwuVillagerExpelled", "GmCmd_TaiwuCrossArchive", "TaiwuCrossArchiveFindMemory", "UserLoadDreamBackArchive", "OperateInventoryItem", "SetItemSelectCount", "SettlementTreasuryBuildingClicked", "SetListenerEventActionISerializableArg", "SetListenerEventActionIntArg",
		"SetListenerEventActionBoolArg", "SetListenerEventActionStringArg", "GetValidInteractionEventOptions", "SetListenerEventActionIntListArg", "SetListenerEventActionItemKeyArg", "TriggerShixiangDrumEasterEgg", "InteractPrisoner", "OnClickedSendPrisonBtn", "OnClickedPrisonBtn", "SetCharacterMultSelectResult",
		"SetCricketBettingResult", "GetImplementedFunctionIds", "SetEventScriptExecutionPause", "EventScriptExecuteNext", "GmCmd_TaiwuWantedSectPunished", "EventSelectContinue", "SetSelectCount", "SetListenerEventActionShortListArg", "SetShowingEventShortListArg", "OnClickMapPickupEvent",
		"OnClickMapPickupNormalEvent", "OnClickDeportButton", "OnSwitchToGuardedPage", "GmCmd_AddJieqingMaskCharId", "GmCmd_RemoveJieqingMaskCharId"
	};
}
