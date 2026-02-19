using System;

namespace Config.ConfigCells;

[Serializable]
public struct LifeSkillCombatBriberyItemTypeWeight
{
	public sbyte ItemType;

	public short Weight;

	public LifeSkillCombatBriberyItemTypeWeight(sbyte itemType, short weight)
	{
		ItemType = itemType;
		Weight = weight;
	}
}
