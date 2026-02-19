using System;
using Config;

namespace GameData.Domains.Adventure;

public static class SharedMethods
{
	public static sbyte GetAdventureCombatDifficulty(short adventureId, sbyte xiangshuLevel)
	{
		AdventureItem adventureItem = Config.Adventure.Instance[adventureId];
		return (sbyte)Math.Min(9, adventureItem.CombatDifficulty + (adventureItem.DifficultyAddXiangshuLevel ? xiangshuLevel : 0));
	}

	public static sbyte GetAdventureLifeSkillDifficulty(short adventureId, sbyte xiangshuLevel)
	{
		AdventureItem adventureItem = Config.Adventure.Instance[adventureId];
		return (sbyte)Math.Min(9, adventureItem.LifeSkillDifficulty + (adventureItem.DifficultyAddXiangshuLevel ? xiangshuLevel : 0));
	}
}
