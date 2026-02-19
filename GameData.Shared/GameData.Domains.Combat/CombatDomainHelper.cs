using System.Collections.Generic;

namespace GameData.Domains.Combat;

public static class CombatDomainHelper
{
	public static class DataIds
	{
		public const ushort TimeScale = 0;

		public const ushort AutoCombat = 1;

		public const ushort CombatFrame = 2;

		public const ushort CombatType = 3;

		public const ushort CurrentDistance = 4;

		public const ushort DamageCompareData = 5;

		public const ushort SkillPowerAddInCombat = 6;

		public const ushort SkillPowerReduceInCombat = 7;

		public const ushort SkillPowerReplaceInCombat = 8;

		public const ushort BgmIndex = 9;

		public const ushort CombatCharacterDict = 10;

		public const ushort SelfTeam = 11;

		public const ushort SelfCharId = 12;

		public const ushort SelfTeamWisdomType = 13;

		public const ushort SelfTeamWisdomCount = 14;

		public const ushort EnemyTeam = 15;

		public const ushort EnemyCharId = 16;

		public const ushort EnemyTeamWisdomType = 17;

		public const ushort EnemyTeamWisdomCount = 18;

		public const ushort CombatStatus = 19;

		public const ushort ShowMercyOption = 20;

		public const ushort SelectedMercyOption = 21;

		public const ushort CarrierAnimalCombatCharId = 22;

		public const ushort SpecialShowCombatCharId = 23;

		public const ushort SkillAttackedIndexAndHit = 24;

		public const ushort WaitingDelaySettlement = 25;

		public const ushort ShowUseGoldenWire = 26;

		public const ushort IsPuppetCombat = 27;

		public const ushort IsPlaygroundCombat = 28;

		public const ushort SkillDataDict = 29;

		public const ushort WeaponDataDict = 30;

		public const ushort ExpectRatioData = 31;

		public const ushort TaiwuSpecialGroupCharIds = 32;
	}

	public static class MethodIds
	{
		public const ushort PlayMoveStepSound = 0;

		public const ushort ExecuteTeammateCommand = 1;

		public const ushort RemoveTeammateCommand = 2;

		public const ushort GetCombatCharDisplayData = 3;

		public const ushort SelectMercyOption = 4;

		public const ushort ChangeWeapon = 5;

		public const ushort NormalAttack = 6;

		public const ushort StartChangeTrick = 7;

		public const ushort SelectChangeTrick = 8;

		public const ushort ChangeTaiwuWeaponInnerRatio = 9;

		public const ushort GetWeaponInnerRatio = 10;

		public const ushort GetWeaponEffects = 11;

		public const ushort StartPrepareOtherAction = 12;

		public const ushort GetProactiveSkillList = 13;

		public const ushort StartPrepareSkill = 14;

		public const ushort GmCmd_ForceRecoverBreathAndStance = 15;

		public const ushort GmCmd_AddTrick = 16;

		public const ushort GmCmd_AddInjury = 17;

		public const ushort GmCmd_ForceHealAllInjury = 18;

		public const ushort GmCmd_AddPoison = 19;

		public const ushort GmCmd_ForceHealAllPoison = 20;

		public const ushort GmCmd_ForceEnemyUseSkill = 21;

		public const ushort GmCmd_ForceEnemyUseOtherAction = 22;

		public const ushort GmCmd_ForceEnemyDefeat = 23;

		public const ushort GmCmd_ForceSelfDefeat = 24;

		public const ushort GmCmd_SetNeiliAllocation = 25;

		public const ushort GmCmd_AddFlaw = 26;

		public const ushort GmCmd_HealAllFlaw = 27;

		public const ushort GmCmd_AddAcupoint = 28;

		public const ushort GmCmd_HealAllAcupoint = 29;

		public const ushort GmCmd_FightBoss = 30;

		public const ushort GmCmd_FightAnimal = 31;

		public const ushort GmCmd_EnableEnemyAi = 32;

		public const ushort GmCmd_EnableSkillFreeCast = 33;

		public const ushort GetHealInjuryBanReason = 34;

		public const ushort GetHealPoisonBanReason = 35;

		public const ushort UseItem = 36;

		public const ushort PrepareCombat = 37;

		public const ushort StartCombat = 38;

		public const ushort SetTimeScale = 39;

		public const ushort SetPlayerAutoCombat = 40;

		public const ushort GetAiTestData = 41;

		public const ushort SetAiOptions = 42;

		public const ushort SetMoveState = 43;

		public const ushort GetCombatResultDisplayData = 44;

		public const ushort SelectGetItem = 45;

		public const ushort Surrender = 46;

		public const ushort EnterBossPuppetCombat = 47;

		public const ushort RepairItem = 48;

		public const ushort PrepareEnemyEquipments = 49;

		public const ushort EnableBulletTime = 50;

		public const ushort GmCmd_SetImmortal = 51;

		public const ushort CancelChangeTrick = 52;

		public const ushort ClearAllReserveAction = 53;

		public const ushort InterruptSurrender = 54;

		public const ushort IsInCombat = 55;

		public const ushort GmCmd_FightTestOrgMember = 56;

		public const ushort GmCmd_FightRandomEnemy = 57;

		public const ushort GmCmd_ForceRecoverMobilityValue = 58;

		public const ushort GmCmd_UnitTestSetDistanceToTarget = 59;

		public const ushort GmCmd_UnitTestEquipSkill = 60;

		public const ushort GmCmd_UnitTestPrepare = 61;

		public const ushort GmCmd_UnitTestClearAllEquipSkill = 62;

		public const ushort GetFatalDamageStepDisplayData = 63;

		public const ushort GetMindDamageStepDisplayData = 64;

		public const ushort GetBodyPartDamageStepDisplayData = 65;

		public const ushort GetCompleteDamageStepDisplayData = 66;

		public const ushort GmCmd_ForceRecoverWugCount = 67;

		public const ushort GmCmd_FightCharacter = 68;

		public const ushort GetChangeTrickDisplayData = 69;

		public const ushort ClearAffectingDefenseSkillManual = 70;

		public const ushort ClearDefendInBlockAttackSkill = 71;

		public const ushort GmCmd_HealAllFatal = 72;

		public const ushort GmCmd_HealAllDefeatMark = 73;

		public const ushort GmCmd_AddAllDefeatMark = 74;

		public const ushort GmCmd_AddFatal = 75;

		public const ushort GmCmd_HealAllDie = 76;

		public const ushort GmCmd_AddDie = 77;

		public const ushort GmCmd_HealAllMind = 78;

		public const ushort GmCmd_HealInjury = 79;

		public const ushort GmCmd_AddMind = 80;

		public const ushort SimulatePrepareCombat = 81;

		public const ushort SetTargetDistance = 82;

		public const ushort ClearTargetDistance = 83;

		public const ushort SetJumpThreshold = 84;

		public const ushort GetPreviewAttackRange = 85;

		public const ushort SetPuppetUnyieldingFallen = 86;

		public const ushort SetPuppetDisableAi = 87;

		public const ushort InterruptSkillManual = 88;

		public const ushort ProcessCombatTeammateCommands = 89;

		public const ushort ClearAffectingMoveSkillManual = 90;

		public const ushort UnlockAttack = 91;

		public const ushort IgnoreAllRawCreate = 92;

		public const ushort IgnoreRawCreate = 93;

		public const ushort DoRawCreate = 94;

		public const ushort GetAllCanRawCreateEquipmentSlots = 95;

		public const ushort GetUnlockSimulateResult = 96;

		public const ushort GetDefeatMarksCountOutOfCombat = 97;

		public const ushort ApplyCombatResultDataEffect = 98;

		public const ushort ClearReserveNormalAttack = 99;

		public const ushort ApplyVitalOnTeammate = 100;

		public const ushort RevertVitalOnTeammate = 101;

		public const ushort GmCmd_ForceRecoverTeammateCommand = 102;

		public const ushort RequestValidItemsInCombat = 103;

		public const ushort RequestSwordFragmentSkillIds = 104;

		public const ushort UseSpecialItem = 105;

		public const ushort NormalAttackImmediate = 106;
	}

	public const ushort DataCount = 33;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "TimeScale", 0 },
		{ "AutoCombat", 1 },
		{ "CombatFrame", 2 },
		{ "CombatType", 3 },
		{ "CurrentDistance", 4 },
		{ "DamageCompareData", 5 },
		{ "SkillPowerAddInCombat", 6 },
		{ "SkillPowerReduceInCombat", 7 },
		{ "SkillPowerReplaceInCombat", 8 },
		{ "BgmIndex", 9 },
		{ "CombatCharacterDict", 10 },
		{ "SelfTeam", 11 },
		{ "SelfCharId", 12 },
		{ "SelfTeamWisdomType", 13 },
		{ "SelfTeamWisdomCount", 14 },
		{ "EnemyTeam", 15 },
		{ "EnemyCharId", 16 },
		{ "EnemyTeamWisdomType", 17 },
		{ "EnemyTeamWisdomCount", 18 },
		{ "CombatStatus", 19 },
		{ "ShowMercyOption", 20 },
		{ "SelectedMercyOption", 21 },
		{ "CarrierAnimalCombatCharId", 22 },
		{ "SpecialShowCombatCharId", 23 },
		{ "SkillAttackedIndexAndHit", 24 },
		{ "WaitingDelaySettlement", 25 },
		{ "ShowUseGoldenWire", 26 },
		{ "IsPuppetCombat", 27 },
		{ "IsPlaygroundCombat", 28 },
		{ "SkillDataDict", 29 },
		{ "WeaponDataDict", 30 },
		{ "ExpectRatioData", 31 },
		{ "TaiwuSpecialGroupCharIds", 32 }
	};

	public static readonly string[] DataId2FieldName = new string[33]
	{
		"TimeScale", "AutoCombat", "CombatFrame", "CombatType", "CurrentDistance", "DamageCompareData", "SkillPowerAddInCombat", "SkillPowerReduceInCombat", "SkillPowerReplaceInCombat", "BgmIndex",
		"CombatCharacterDict", "SelfTeam", "SelfCharId", "SelfTeamWisdomType", "SelfTeamWisdomCount", "EnemyTeam", "EnemyCharId", "EnemyTeamWisdomType", "EnemyTeamWisdomCount", "CombatStatus",
		"ShowMercyOption", "SelectedMercyOption", "CarrierAnimalCombatCharId", "SpecialShowCombatCharId", "SkillAttackedIndexAndHit", "WaitingDelaySettlement", "ShowUseGoldenWire", "IsPuppetCombat", "IsPlaygroundCombat", "SkillDataDict",
		"WeaponDataDict", "ExpectRatioData", "TaiwuSpecialGroupCharIds"
	};

	public static readonly string[][] DataId2ObjectFieldId2FieldName;

	public static readonly Dictionary<string, ushort> MethodName2MethodId;

	public static readonly string[] MethodId2MethodName;

	static CombatDomainHelper()
	{
		string[][] array = new string[33][];
		array[10] = CombatCharacterHelper.FieldId2FieldName;
		array[29] = CombatSkillDataHelper.FieldId2FieldName;
		array[30] = CombatWeaponDataHelper.FieldId2FieldName;
		DataId2ObjectFieldId2FieldName = array;
		MethodName2MethodId = new Dictionary<string, ushort>
		{
			{ "PlayMoveStepSound", 0 },
			{ "ExecuteTeammateCommand", 1 },
			{ "RemoveTeammateCommand", 2 },
			{ "GetCombatCharDisplayData", 3 },
			{ "SelectMercyOption", 4 },
			{ "ChangeWeapon", 5 },
			{ "NormalAttack", 6 },
			{ "StartChangeTrick", 7 },
			{ "SelectChangeTrick", 8 },
			{ "ChangeTaiwuWeaponInnerRatio", 9 },
			{ "GetWeaponInnerRatio", 10 },
			{ "GetWeaponEffects", 11 },
			{ "StartPrepareOtherAction", 12 },
			{ "GetProactiveSkillList", 13 },
			{ "StartPrepareSkill", 14 },
			{ "GmCmd_ForceRecoverBreathAndStance", 15 },
			{ "GmCmd_AddTrick", 16 },
			{ "GmCmd_AddInjury", 17 },
			{ "GmCmd_ForceHealAllInjury", 18 },
			{ "GmCmd_AddPoison", 19 },
			{ "GmCmd_ForceHealAllPoison", 20 },
			{ "GmCmd_ForceEnemyUseSkill", 21 },
			{ "GmCmd_ForceEnemyUseOtherAction", 22 },
			{ "GmCmd_ForceEnemyDefeat", 23 },
			{ "GmCmd_ForceSelfDefeat", 24 },
			{ "GmCmd_SetNeiliAllocation", 25 },
			{ "GmCmd_AddFlaw", 26 },
			{ "GmCmd_HealAllFlaw", 27 },
			{ "GmCmd_AddAcupoint", 28 },
			{ "GmCmd_HealAllAcupoint", 29 },
			{ "GmCmd_FightBoss", 30 },
			{ "GmCmd_FightAnimal", 31 },
			{ "GmCmd_EnableEnemyAi", 32 },
			{ "GmCmd_EnableSkillFreeCast", 33 },
			{ "GetHealInjuryBanReason", 34 },
			{ "GetHealPoisonBanReason", 35 },
			{ "UseItem", 36 },
			{ "PrepareCombat", 37 },
			{ "StartCombat", 38 },
			{ "SetTimeScale", 39 },
			{ "SetPlayerAutoCombat", 40 },
			{ "GetAiTestData", 41 },
			{ "SetAiOptions", 42 },
			{ "SetMoveState", 43 },
			{ "GetCombatResultDisplayData", 44 },
			{ "SelectGetItem", 45 },
			{ "Surrender", 46 },
			{ "EnterBossPuppetCombat", 47 },
			{ "RepairItem", 48 },
			{ "PrepareEnemyEquipments", 49 },
			{ "EnableBulletTime", 50 },
			{ "GmCmd_SetImmortal", 51 },
			{ "CancelChangeTrick", 52 },
			{ "ClearAllReserveAction", 53 },
			{ "InterruptSurrender", 54 },
			{ "IsInCombat", 55 },
			{ "GmCmd_FightTestOrgMember", 56 },
			{ "GmCmd_FightRandomEnemy", 57 },
			{ "GmCmd_ForceRecoverMobilityValue", 58 },
			{ "GmCmd_UnitTestSetDistanceToTarget", 59 },
			{ "GmCmd_UnitTestEquipSkill", 60 },
			{ "GmCmd_UnitTestPrepare", 61 },
			{ "GmCmd_UnitTestClearAllEquipSkill", 62 },
			{ "GetFatalDamageStepDisplayData", 63 },
			{ "GetMindDamageStepDisplayData", 64 },
			{ "GetBodyPartDamageStepDisplayData", 65 },
			{ "GetCompleteDamageStepDisplayData", 66 },
			{ "GmCmd_ForceRecoverWugCount", 67 },
			{ "GmCmd_FightCharacter", 68 },
			{ "GetChangeTrickDisplayData", 69 },
			{ "ClearAffectingDefenseSkillManual", 70 },
			{ "ClearDefendInBlockAttackSkill", 71 },
			{ "GmCmd_HealAllFatal", 72 },
			{ "GmCmd_HealAllDefeatMark", 73 },
			{ "GmCmd_AddAllDefeatMark", 74 },
			{ "GmCmd_AddFatal", 75 },
			{ "GmCmd_HealAllDie", 76 },
			{ "GmCmd_AddDie", 77 },
			{ "GmCmd_HealAllMind", 78 },
			{ "GmCmd_HealInjury", 79 },
			{ "GmCmd_AddMind", 80 },
			{ "SimulatePrepareCombat", 81 },
			{ "SetTargetDistance", 82 },
			{ "ClearTargetDistance", 83 },
			{ "SetJumpThreshold", 84 },
			{ "GetPreviewAttackRange", 85 },
			{ "SetPuppetUnyieldingFallen", 86 },
			{ "SetPuppetDisableAi", 87 },
			{ "InterruptSkillManual", 88 },
			{ "ProcessCombatTeammateCommands", 89 },
			{ "ClearAffectingMoveSkillManual", 90 },
			{ "UnlockAttack", 91 },
			{ "IgnoreAllRawCreate", 92 },
			{ "IgnoreRawCreate", 93 },
			{ "DoRawCreate", 94 },
			{ "GetAllCanRawCreateEquipmentSlots", 95 },
			{ "GetUnlockSimulateResult", 96 },
			{ "GetDefeatMarksCountOutOfCombat", 97 },
			{ "ApplyCombatResultDataEffect", 98 },
			{ "ClearReserveNormalAttack", 99 },
			{ "ApplyVitalOnTeammate", 100 },
			{ "RevertVitalOnTeammate", 101 },
			{ "GmCmd_ForceRecoverTeammateCommand", 102 },
			{ "RequestValidItemsInCombat", 103 },
			{ "RequestSwordFragmentSkillIds", 104 },
			{ "UseSpecialItem", 105 },
			{ "NormalAttackImmediate", 106 }
		};
		MethodId2MethodName = new string[107]
		{
			"PlayMoveStepSound", "ExecuteTeammateCommand", "RemoveTeammateCommand", "GetCombatCharDisplayData", "SelectMercyOption", "ChangeWeapon", "NormalAttack", "StartChangeTrick", "SelectChangeTrick", "ChangeTaiwuWeaponInnerRatio",
			"GetWeaponInnerRatio", "GetWeaponEffects", "StartPrepareOtherAction", "GetProactiveSkillList", "StartPrepareSkill", "GmCmd_ForceRecoverBreathAndStance", "GmCmd_AddTrick", "GmCmd_AddInjury", "GmCmd_ForceHealAllInjury", "GmCmd_AddPoison",
			"GmCmd_ForceHealAllPoison", "GmCmd_ForceEnemyUseSkill", "GmCmd_ForceEnemyUseOtherAction", "GmCmd_ForceEnemyDefeat", "GmCmd_ForceSelfDefeat", "GmCmd_SetNeiliAllocation", "GmCmd_AddFlaw", "GmCmd_HealAllFlaw", "GmCmd_AddAcupoint", "GmCmd_HealAllAcupoint",
			"GmCmd_FightBoss", "GmCmd_FightAnimal", "GmCmd_EnableEnemyAi", "GmCmd_EnableSkillFreeCast", "GetHealInjuryBanReason", "GetHealPoisonBanReason", "UseItem", "PrepareCombat", "StartCombat", "SetTimeScale",
			"SetPlayerAutoCombat", "GetAiTestData", "SetAiOptions", "SetMoveState", "GetCombatResultDisplayData", "SelectGetItem", "Surrender", "EnterBossPuppetCombat", "RepairItem", "PrepareEnemyEquipments",
			"EnableBulletTime", "GmCmd_SetImmortal", "CancelChangeTrick", "ClearAllReserveAction", "InterruptSurrender", "IsInCombat", "GmCmd_FightTestOrgMember", "GmCmd_FightRandomEnemy", "GmCmd_ForceRecoverMobilityValue", "GmCmd_UnitTestSetDistanceToTarget",
			"GmCmd_UnitTestEquipSkill", "GmCmd_UnitTestPrepare", "GmCmd_UnitTestClearAllEquipSkill", "GetFatalDamageStepDisplayData", "GetMindDamageStepDisplayData", "GetBodyPartDamageStepDisplayData", "GetCompleteDamageStepDisplayData", "GmCmd_ForceRecoverWugCount", "GmCmd_FightCharacter", "GetChangeTrickDisplayData",
			"ClearAffectingDefenseSkillManual", "ClearDefendInBlockAttackSkill", "GmCmd_HealAllFatal", "GmCmd_HealAllDefeatMark", "GmCmd_AddAllDefeatMark", "GmCmd_AddFatal", "GmCmd_HealAllDie", "GmCmd_AddDie", "GmCmd_HealAllMind", "GmCmd_HealInjury",
			"GmCmd_AddMind", "SimulatePrepareCombat", "SetTargetDistance", "ClearTargetDistance", "SetJumpThreshold", "GetPreviewAttackRange", "SetPuppetUnyieldingFallen", "SetPuppetDisableAi", "InterruptSkillManual", "ProcessCombatTeammateCommands",
			"ClearAffectingMoveSkillManual", "UnlockAttack", "IgnoreAllRawCreate", "IgnoreRawCreate", "DoRawCreate", "GetAllCanRawCreateEquipmentSlots", "GetUnlockSimulateResult", "GetDefeatMarksCountOutOfCombat", "ApplyCombatResultDataEffect", "ClearReserveNormalAttack",
			"ApplyVitalOnTeammate", "RevertVitalOnTeammate", "GmCmd_ForceRecoverTeammateCommand", "RequestValidItemsInCombat", "RequestSwordFragmentSkillIds", "UseSpecialItem", "NormalAttackImmediate"
		};
	}
}
