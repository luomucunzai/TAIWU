using System.Collections.Generic;

namespace GameData.Domains.CombatSkill;

public static class CombatSkillDomainHelper
{
	public static class DataIds
	{
		public const ushort CombatSkills = 0;
	}

	public static class MethodIds
	{
		public const ushort GetCombatSkillDisplayData = 0;

		public const ushort GetCombatSkillBreakStepCount = 1;

		public const ushort GetCharacterEquipCombatSkillDisplayData = 2;

		public const ushort GetCombatSkillDisplayDataOnce = 3;

		public const ushort GetEffectDescriptionData = 4;

		public const ushort CalcTaiwuExtraDeltaNeiliPerLoop = 5;

		public const ushort CalcTaiwuExtraDeltaNeiliAllocationPerLoop = 6;

		public const ushort GetCombatSkillPreviewDisplayDataOnce = 7;

		public const ushort GetCombatSkillBreakoutStepsMaxPower = 8;

		public const ushort GetCombatSkillBreakBonuses = 9;

		public const ushort SetActivePage = 10;

		public const ushort DeActivePage = 11;
	}

	public const ushort DataCount = 1;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort> { { "CombatSkills", 0 } };

	public static readonly string[] DataId2FieldName = new string[1] { "CombatSkills" };

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[1][] { CombatSkillHelper.FieldId2FieldName };

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "GetCombatSkillDisplayData", 0 },
		{ "GetCombatSkillBreakStepCount", 1 },
		{ "GetCharacterEquipCombatSkillDisplayData", 2 },
		{ "GetCombatSkillDisplayDataOnce", 3 },
		{ "GetEffectDescriptionData", 4 },
		{ "CalcTaiwuExtraDeltaNeiliPerLoop", 5 },
		{ "CalcTaiwuExtraDeltaNeiliAllocationPerLoop", 6 },
		{ "GetCombatSkillPreviewDisplayDataOnce", 7 },
		{ "GetCombatSkillBreakoutStepsMaxPower", 8 },
		{ "GetCombatSkillBreakBonuses", 9 },
		{ "SetActivePage", 10 },
		{ "DeActivePage", 11 }
	};

	public static readonly string[] MethodId2MethodName = new string[12]
	{
		"GetCombatSkillDisplayData", "GetCombatSkillBreakStepCount", "GetCharacterEquipCombatSkillDisplayData", "GetCombatSkillDisplayDataOnce", "GetEffectDescriptionData", "CalcTaiwuExtraDeltaNeiliPerLoop", "CalcTaiwuExtraDeltaNeiliAllocationPerLoop", "GetCombatSkillPreviewDisplayDataOnce", "GetCombatSkillBreakoutStepsMaxPower", "GetCombatSkillBreakBonuses",
		"SetActivePage", "DeActivePage"
	};
}
