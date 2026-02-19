using System.Collections.Generic;

namespace GameData.Domains.Adventure;

public static class AdventureDomainHelper
{
	public static class DataIds
	{
		public const ushort PlayerPos = 0;

		public const ushort Personalities = 1;

		public const ushort PersonalitiesCost = 2;

		public const ushort PersonalitiesGain = 3;

		public const ushort IndicatePath = 4;

		public const ushort ArrangeableNodes = 5;

		public const ushort ArrangedNodes = 6;

		public const ushort PerceivedNodes = 7;

		public const ushort ErrorInfo = 8;

		public const ushort AdBranchOpenRecord = 9;

		public const ushort CurAdventureId = 10;

		public const ushort OperationBlock = 11;

		public const ushort AdventureState = 12;

		public const ushort CurMapTrunk = 13;

		public const ushort AdvParameters = 14;

		public const ushort EnterItems = 15;

		public const ushort AllowExitAdventure = 16;

		public const ushort AdventureAreas = 17;

		public const ushort EnemyNestSites = 18;

		public const ushort BrokenAreaEnemies = 19;

		public const ushort SpentCharList = 20;

		public const ushort CurBranchChosenChar = 21;

		public const ushort EscapingRandomEnemies = 22;

		public const ushort TeamDetectedNodes = 23;

		public const ushort ActionPointWithhold = 24;
	}

	public static class MethodIds
	{
		public const ushort GmCmd_GenerateAdventure = 0;

		public const ushort EnterAdventureByConfigData = 1;

		public const ushort SelectBranchForPreview = 2;

		public const ushort ClearBranchForPreview = 3;

		public const ushort EnterAdventure = 4;

		public const ushort ArrangeNode = 5;

		public const ushort PerceiveNode = 6;

		public const ushort ConfirmPath = 7;

		public const ushort ConfirmArrived = 8;

		public const ushort ExitAdventure = 9;

		public const ushort SwitchBranch = 10;

		public const ushort ClearPathArrangement = 11;

		public const ushort GetAdventuresInArea = 12;

		public const ushort GetAwakeSwordTombs = 13;

		public const ushort GetAttackingSwordTombs = 14;

		public const ushort SetCharacterToAdvanceBranch = 15;

		public const ushort CanSetCharacterToAdvanceBranch = 16;

		public const ushort GetAdventureSiteDataDict = 17;

		public const ushort GetAdventureResultDisplayData = 18;

		public const ushort SelectGetItem = 19;

		public const ushort GetPathContentList = 20;

		public const ushort GetAdventureSpentCharList = 21;

		public const ushort GetAdventureGainsItemList = 22;

		public const ushort QueryAdventureLocation = 23;

		public const ushort QueryAdventureLocationFirstInCurrent = 24;

		public const ushort GetCharacterDisplayDataListInAdventure = 25;

		public const ushort TryInvokeConfirmEnterEvent = 26;

		public const ushort GmCmd_SetAdventureParameter = 27;

		public const ushort GmCmd_GetAdventureParameter = 28;
	}

	public const ushort DataCount = 25;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "PlayerPos", 0 },
		{ "Personalities", 1 },
		{ "PersonalitiesCost", 2 },
		{ "PersonalitiesGain", 3 },
		{ "IndicatePath", 4 },
		{ "ArrangeableNodes", 5 },
		{ "ArrangedNodes", 6 },
		{ "PerceivedNodes", 7 },
		{ "ErrorInfo", 8 },
		{ "AdBranchOpenRecord", 9 },
		{ "CurAdventureId", 10 },
		{ "OperationBlock", 11 },
		{ "AdventureState", 12 },
		{ "CurMapTrunk", 13 },
		{ "AdvParameters", 14 },
		{ "EnterItems", 15 },
		{ "AllowExitAdventure", 16 },
		{ "AdventureAreas", 17 },
		{ "EnemyNestSites", 18 },
		{ "BrokenAreaEnemies", 19 },
		{ "SpentCharList", 20 },
		{ "CurBranchChosenChar", 21 },
		{ "EscapingRandomEnemies", 22 },
		{ "TeamDetectedNodes", 23 },
		{ "ActionPointWithhold", 24 }
	};

	public static readonly string[] DataId2FieldName = new string[25]
	{
		"PlayerPos", "Personalities", "PersonalitiesCost", "PersonalitiesGain", "IndicatePath", "ArrangeableNodes", "ArrangedNodes", "PerceivedNodes", "ErrorInfo", "AdBranchOpenRecord",
		"CurAdventureId", "OperationBlock", "AdventureState", "CurMapTrunk", "AdvParameters", "EnterItems", "AllowExitAdventure", "AdventureAreas", "EnemyNestSites", "BrokenAreaEnemies",
		"SpentCharList", "CurBranchChosenChar", "EscapingRandomEnemies", "TeamDetectedNodes", "ActionPointWithhold"
	};

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[25][];

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "GmCmd_GenerateAdventure", 0 },
		{ "EnterAdventureByConfigData", 1 },
		{ "SelectBranchForPreview", 2 },
		{ "ClearBranchForPreview", 3 },
		{ "EnterAdventure", 4 },
		{ "ArrangeNode", 5 },
		{ "PerceiveNode", 6 },
		{ "ConfirmPath", 7 },
		{ "ConfirmArrived", 8 },
		{ "ExitAdventure", 9 },
		{ "SwitchBranch", 10 },
		{ "ClearPathArrangement", 11 },
		{ "GetAdventuresInArea", 12 },
		{ "GetAwakeSwordTombs", 13 },
		{ "GetAttackingSwordTombs", 14 },
		{ "SetCharacterToAdvanceBranch", 15 },
		{ "CanSetCharacterToAdvanceBranch", 16 },
		{ "GetAdventureSiteDataDict", 17 },
		{ "GetAdventureResultDisplayData", 18 },
		{ "SelectGetItem", 19 },
		{ "GetPathContentList", 20 },
		{ "GetAdventureSpentCharList", 21 },
		{ "GetAdventureGainsItemList", 22 },
		{ "QueryAdventureLocation", 23 },
		{ "QueryAdventureLocationFirstInCurrent", 24 },
		{ "GetCharacterDisplayDataListInAdventure", 25 },
		{ "TryInvokeConfirmEnterEvent", 26 },
		{ "GmCmd_SetAdventureParameter", 27 },
		{ "GmCmd_GetAdventureParameter", 28 }
	};

	public static readonly string[] MethodId2MethodName = new string[29]
	{
		"GmCmd_GenerateAdventure", "EnterAdventureByConfigData", "SelectBranchForPreview", "ClearBranchForPreview", "EnterAdventure", "ArrangeNode", "PerceiveNode", "ConfirmPath", "ConfirmArrived", "ExitAdventure",
		"SwitchBranch", "ClearPathArrangement", "GetAdventuresInArea", "GetAwakeSwordTombs", "GetAttackingSwordTombs", "SetCharacterToAdvanceBranch", "CanSetCharacterToAdvanceBranch", "GetAdventureSiteDataDict", "GetAdventureResultDisplayData", "SelectGetItem",
		"GetPathContentList", "GetAdventureSpentCharList", "GetAdventureGainsItemList", "QueryAdventureLocation", "QueryAdventureLocationFirstInCurrent", "GetCharacterDisplayDataListInAdventure", "TryInvokeConfirmEnterEvent", "GmCmd_SetAdventureParameter", "GmCmd_GetAdventureParameter"
	};
}
