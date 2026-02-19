namespace GameData.Domains.Character.ParallelModifications;

public readonly struct AddOrIncreaseInjuryParams
{
	public readonly sbyte BodyPartType;

	public readonly bool IsInnerInjury;

	public readonly sbyte InjuryValue;

	public AddOrIncreaseInjuryParams(sbyte bodyPartType, bool isInnerInjury, sbyte injuryValue)
	{
		BodyPartType = bodyPartType;
		IsInnerInjury = isInnerInjury;
		InjuryValue = injuryValue;
	}
}
