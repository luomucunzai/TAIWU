using System;

namespace GameData.Domains.Character;

public static class CombatResourcesHelper
{
	public static CombatResources GetMax(Character character)
	{
		short lifeSkillAttainment = character.GetLifeSkillAttainment(8);
		short lifeSkillAttainment2 = character.GetLifeSkillAttainment(9);
		return new CombatResources
		{
			HealingCount = (sbyte)Math.Min(1 + lifeSkillAttainment / 60, 99),
			DetoxCount = (sbyte)Math.Min(1 + lifeSkillAttainment2 / 60, 99),
			BreathingCount = 1,
			RecoverCount = 1
		};
	}
}
