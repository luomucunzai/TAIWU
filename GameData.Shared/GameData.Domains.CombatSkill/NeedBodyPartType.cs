namespace GameData.Domains.CombatSkill;

public static class NeedBodyPartType
{
	public const sbyte Head = 0;

	public const sbyte Chest = 1;

	public const sbyte Belly = 2;

	public const sbyte BothHands = 3;

	public const sbyte AnyHand = 4;

	public const sbyte BothLegs = 5;

	public const sbyte AnyLeg = 6;

	public static bool Contains(sbyte needBodyPart, sbyte bodyPart)
	{
		switch (needBodyPart)
		{
		case 0:
			return bodyPart == 2;
		case 1:
			return bodyPart == 0;
		case 2:
			return bodyPart == 1;
		case 3:
		case 4:
			if (bodyPart != 3)
			{
				return bodyPart == 4;
			}
			return true;
		case 5:
		case 6:
			if (bodyPart != 5)
			{
				return bodyPart == 6;
			}
			return true;
		default:
			return false;
		}
	}
}
