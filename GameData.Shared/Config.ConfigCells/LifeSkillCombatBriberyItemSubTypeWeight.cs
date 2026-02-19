using System;

namespace Config.ConfigCells;

[Serializable]
public struct LifeSkillCombatBriberyItemSubTypeWeight
{
	public short ItemSubType;

	public short Weight;

	public LifeSkillCombatBriberyItemSubTypeWeight(short itemSubType, short weight)
	{
		ItemSubType = itemSubType;
		Weight = weight;
	}
}
