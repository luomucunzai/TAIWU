using System.Collections.Generic;

namespace GameData.Domains.CombatSkill;

public static class CombatSkillHelper
{
	public static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort PracticeLevel = 1;

		public const ushort ReadingState = 2;

		public const ushort ActivationState = 3;

		public const ushort ForcedBreakoutStepsCount = 4;

		public const ushort BreakoutStepsCount = 5;

		public const ushort InnerRatio = 6;

		public const ushort ObtainedNeili = 7;

		public const ushort Revoked = 8;

		public const ushort SpecialEffectId = 9;

		public const ushort Power = 10;

		public const ushort MaxPower = 11;

		public const ushort RequirementPercent = 12;

		public const ushort Direction = 13;

		public const ushort BaseScore = 14;

		public const ushort CurrInnerRatio = 15;

		public const ushort HitValue = 16;

		public const ushort Penetrations = 17;

		public const ushort CostBreathAndStancePercent = 18;

		public const ushort CostBreathPercent = 19;

		public const ushort CostStancePercent = 20;

		public const ushort CostMobilityPercent = 21;

		public const ushort AddHitValueOnCast = 22;

		public const ushort AddPenetrateResist = 23;

		public const ushort AddAvoidValueOnCast = 24;

		public const ushort FightBackPower = 25;

		public const ushort BouncePower = 26;

		public const ushort RequirementsPower = 27;

		public const ushort PlateAddMaxPower = 28;
	}

	public const ushort ArchiveFieldsCount = 10;

	public const ushort CacheFieldsCount = 19;

	public const ushort PureTemplateFieldsCount = 0;

	public const ushort WritableFieldsCount = 29;

	public const ushort ReadonlyFieldsCount = 0;

	public static readonly Dictionary<string, ushort> FieldName2FieldId = new Dictionary<string, ushort>
	{
		{ "Id", 0 },
		{ "PracticeLevel", 1 },
		{ "ReadingState", 2 },
		{ "ActivationState", 3 },
		{ "ForcedBreakoutStepsCount", 4 },
		{ "BreakoutStepsCount", 5 },
		{ "InnerRatio", 6 },
		{ "ObtainedNeili", 7 },
		{ "Revoked", 8 },
		{ "SpecialEffectId", 9 },
		{ "Power", 10 },
		{ "MaxPower", 11 },
		{ "RequirementPercent", 12 },
		{ "Direction", 13 },
		{ "BaseScore", 14 },
		{ "CurrInnerRatio", 15 },
		{ "HitValue", 16 },
		{ "Penetrations", 17 },
		{ "CostBreathAndStancePercent", 18 },
		{ "CostBreathPercent", 19 },
		{ "CostStancePercent", 20 },
		{ "CostMobilityPercent", 21 },
		{ "AddHitValueOnCast", 22 },
		{ "AddPenetrateResist", 23 },
		{ "AddAvoidValueOnCast", 24 },
		{ "FightBackPower", 25 },
		{ "BouncePower", 26 },
		{ "RequirementsPower", 27 },
		{ "PlateAddMaxPower", 28 }
	};

	public static readonly string[] FieldId2FieldName = new string[29]
	{
		"Id", "PracticeLevel", "ReadingState", "ActivationState", "ForcedBreakoutStepsCount", "BreakoutStepsCount", "InnerRatio", "ObtainedNeili", "Revoked", "SpecialEffectId",
		"Power", "MaxPower", "RequirementPercent", "Direction", "BaseScore", "CurrInnerRatio", "HitValue", "Penetrations", "CostBreathAndStancePercent", "CostBreathPercent",
		"CostStancePercent", "CostMobilityPercent", "AddHitValueOnCast", "AddPenetrateResist", "AddAvoidValueOnCast", "FightBackPower", "BouncePower", "RequirementsPower", "PlateAddMaxPower"
	};
}
