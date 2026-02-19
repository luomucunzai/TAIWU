using System.Collections.Generic;

namespace GameData.Domains.World;

public static class WorldDomainHelper
{
	public static class DataIds
	{
		public const ushort WorldId = 0;

		public const ushort XiangshuProgress = 1;

		public const ushort XiangshuAvatarTaskStatuses = 2;

		public const ushort XiangshuAvatarTasksInOrder = 3;

		public const ushort StateTaskStatuses = 4;

		public const ushort MainStoryLineProgress = 5;

		public const ushort BeatRanChenZi = 6;

		public const ushort WorldFunctionsStatuses = 7;

		public const ushort CustomTexts = 8;

		public const ushort NextCustomTextId = 9;

		public const ushort InstantNotifications = 10;

		public const ushort OnHandingMonthlyEventBlock = 11;

		public const ushort LastMonthlyNotifications = 12;

		public const ushort WorldPopulationType = 13;

		public const ushort CharacterLifespanType = 14;

		public const ushort CombatDifficulty = 15;

		public const ushort HereticsAmountType = 16;

		public const ushort BossInvasionSpeedType = 17;

		public const ushort WorldResourceAmountType = 18;

		public const ushort AllowRandomTaiwuHeir = 19;

		public const ushort RestrictOptionsBehaviorType = 20;

		public const ushort TaiwuVillageStateTemplateId = 21;

		public const ushort TaiwuVillageLandFormType = 22;

		public const ushort HideTaiwuOriginalSurname = 23;

		public const ushort AllowExecute = 24;

		public const ushort ArchiveFilesBackupInterval = 25;

		public const ushort WorldStandardPopulation = 26;

		public const ushort CurrDate = 27;

		public const ushort DaysInCurrMonth = 28;

		public const ushort AdvancingMonthState = 29;

		public const ushort CurrTaskList = 30;

		public const ushort SortedTaskList = 31;

		public const ushort WorldStateData = 32;

		public const ushort ArchiveFilesBackupCount = 33;

		public const ushort SortedMonthlyNotificationSortingGroups = 34;
	}

	public static class MethodIds
	{
		public const ushort CreateWorld = 0;

		public const ushort SetWorldCreationInfo = 1;

		public const ushort GetWorldCreationInfo = 2;

		public const ushort GetJuniorXiangshuLocations = 3;

		public const ushort HandleMonthlyEvent = 4;

		public const ushort GetMonthlyEventCollection = 5;

		public const ushort RemoveAllInvalidMonthlyEvents = 6;

		public const ushort ProcessAllMonthlyEventsWithDefaultOption = 7;

		public const ushort SpecifyWorldPopulationType = 8;

		public const ushort AdvanceDaysInMonth = 9;

		public const ushort AdvanceMonth = 10;

		public const ushort AdvanceMonth_DisplayedMonthlyNotifications = 11;

		public const ushort GmCmd_SectEmeiAddSkillBreakBonus = 12;

		public const ushort GmCmd_SectEmeiClearSkillBreakBonus = 13;

		public const ushort RefiningWugKing = 14;

		public const ushort DropPoisonsToWugJug = 15;

		public const ushort JingangMonkSoulBtnShow = 16;

		public const ushort JingangSoulTransformOpen = 17;

		public const ushort GetBaihuaLifeLinkNeiliType = 18;

		public const ushort SetLifeLinkCharacter = 19;

		public const ushort EmeiTransferBonusProgress = 20;

		public const ushort GmCmd_AddMonthlyEvent = 21;

		public const ushort CatchThief = 22;

		public const ushort TryTriggerThiefCatch = 23;

		public const ushort ShaolinStartDemonSlayerTrial = 24;

		public const ushort ShaolinInterruptDemonSlayerTrial = 25;

		public const ushort ShaolinRegenerateRestricts = 26;

		public const ushort ShaolinQueryRestrictsAreSatisfied = 27;

		public const ushort ShaolinClearTemporaryDemon = 28;

		public const ushort ShaolinGenerateTemporaryDemon = 29;

		public const ushort GetSectMainStoryTriggerConditions = 30;

		public const ushort GetSectMainStoryActiveStatus = 31;

		public const ushort SetSectMainStoryActiveStatus = 32;

		public const ushort NotifySectStoryActivated = 33;
	}

	public const ushort DataCount = 35;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "WorldId", 0 },
		{ "XiangshuProgress", 1 },
		{ "XiangshuAvatarTaskStatuses", 2 },
		{ "XiangshuAvatarTasksInOrder", 3 },
		{ "StateTaskStatuses", 4 },
		{ "MainStoryLineProgress", 5 },
		{ "BeatRanChenZi", 6 },
		{ "WorldFunctionsStatuses", 7 },
		{ "CustomTexts", 8 },
		{ "NextCustomTextId", 9 },
		{ "InstantNotifications", 10 },
		{ "OnHandingMonthlyEventBlock", 11 },
		{ "LastMonthlyNotifications", 12 },
		{ "WorldPopulationType", 13 },
		{ "CharacterLifespanType", 14 },
		{ "CombatDifficulty", 15 },
		{ "HereticsAmountType", 16 },
		{ "BossInvasionSpeedType", 17 },
		{ "WorldResourceAmountType", 18 },
		{ "AllowRandomTaiwuHeir", 19 },
		{ "RestrictOptionsBehaviorType", 20 },
		{ "TaiwuVillageStateTemplateId", 21 },
		{ "TaiwuVillageLandFormType", 22 },
		{ "HideTaiwuOriginalSurname", 23 },
		{ "AllowExecute", 24 },
		{ "ArchiveFilesBackupInterval", 25 },
		{ "WorldStandardPopulation", 26 },
		{ "CurrDate", 27 },
		{ "DaysInCurrMonth", 28 },
		{ "AdvancingMonthState", 29 },
		{ "CurrTaskList", 30 },
		{ "SortedTaskList", 31 },
		{ "WorldStateData", 32 },
		{ "ArchiveFilesBackupCount", 33 },
		{ "SortedMonthlyNotificationSortingGroups", 34 }
	};

	public static readonly string[] DataId2FieldName = new string[35]
	{
		"WorldId", "XiangshuProgress", "XiangshuAvatarTaskStatuses", "XiangshuAvatarTasksInOrder", "StateTaskStatuses", "MainStoryLineProgress", "BeatRanChenZi", "WorldFunctionsStatuses", "CustomTexts", "NextCustomTextId",
		"InstantNotifications", "OnHandingMonthlyEventBlock", "LastMonthlyNotifications", "WorldPopulationType", "CharacterLifespanType", "CombatDifficulty", "HereticsAmountType", "BossInvasionSpeedType", "WorldResourceAmountType", "AllowRandomTaiwuHeir",
		"RestrictOptionsBehaviorType", "TaiwuVillageStateTemplateId", "TaiwuVillageLandFormType", "HideTaiwuOriginalSurname", "AllowExecute", "ArchiveFilesBackupInterval", "WorldStandardPopulation", "CurrDate", "DaysInCurrMonth", "AdvancingMonthState",
		"CurrTaskList", "SortedTaskList", "WorldStateData", "ArchiveFilesBackupCount", "SortedMonthlyNotificationSortingGroups"
	};

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[35][];

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "CreateWorld", 0 },
		{ "SetWorldCreationInfo", 1 },
		{ "GetWorldCreationInfo", 2 },
		{ "GetJuniorXiangshuLocations", 3 },
		{ "HandleMonthlyEvent", 4 },
		{ "GetMonthlyEventCollection", 5 },
		{ "RemoveAllInvalidMonthlyEvents", 6 },
		{ "ProcessAllMonthlyEventsWithDefaultOption", 7 },
		{ "SpecifyWorldPopulationType", 8 },
		{ "AdvanceDaysInMonth", 9 },
		{ "AdvanceMonth", 10 },
		{ "AdvanceMonth_DisplayedMonthlyNotifications", 11 },
		{ "GmCmd_SectEmeiAddSkillBreakBonus", 12 },
		{ "GmCmd_SectEmeiClearSkillBreakBonus", 13 },
		{ "RefiningWugKing", 14 },
		{ "DropPoisonsToWugJug", 15 },
		{ "JingangMonkSoulBtnShow", 16 },
		{ "JingangSoulTransformOpen", 17 },
		{ "GetBaihuaLifeLinkNeiliType", 18 },
		{ "SetLifeLinkCharacter", 19 },
		{ "EmeiTransferBonusProgress", 20 },
		{ "GmCmd_AddMonthlyEvent", 21 },
		{ "CatchThief", 22 },
		{ "TryTriggerThiefCatch", 23 },
		{ "ShaolinStartDemonSlayerTrial", 24 },
		{ "ShaolinInterruptDemonSlayerTrial", 25 },
		{ "ShaolinRegenerateRestricts", 26 },
		{ "ShaolinQueryRestrictsAreSatisfied", 27 },
		{ "ShaolinClearTemporaryDemon", 28 },
		{ "ShaolinGenerateTemporaryDemon", 29 },
		{ "GetSectMainStoryTriggerConditions", 30 },
		{ "GetSectMainStoryActiveStatus", 31 },
		{ "SetSectMainStoryActiveStatus", 32 },
		{ "NotifySectStoryActivated", 33 }
	};

	public static readonly string[] MethodId2MethodName = new string[34]
	{
		"CreateWorld", "SetWorldCreationInfo", "GetWorldCreationInfo", "GetJuniorXiangshuLocations", "HandleMonthlyEvent", "GetMonthlyEventCollection", "RemoveAllInvalidMonthlyEvents", "ProcessAllMonthlyEventsWithDefaultOption", "SpecifyWorldPopulationType", "AdvanceDaysInMonth",
		"AdvanceMonth", "AdvanceMonth_DisplayedMonthlyNotifications", "GmCmd_SectEmeiAddSkillBreakBonus", "GmCmd_SectEmeiClearSkillBreakBonus", "RefiningWugKing", "DropPoisonsToWugJug", "JingangMonkSoulBtnShow", "JingangSoulTransformOpen", "GetBaihuaLifeLinkNeiliType", "SetLifeLinkCharacter",
		"EmeiTransferBonusProgress", "GmCmd_AddMonthlyEvent", "CatchThief", "TryTriggerThiefCatch", "ShaolinStartDemonSlayerTrial", "ShaolinInterruptDemonSlayerTrial", "ShaolinRegenerateRestricts", "ShaolinQueryRestrictsAreSatisfied", "ShaolinClearTemporaryDemon", "ShaolinGenerateTemporaryDemon",
		"GetSectMainStoryTriggerConditions", "GetSectMainStoryActiveStatus", "SetSectMainStoryActiveStatus", "NotifySectStoryActivated"
	};
}
