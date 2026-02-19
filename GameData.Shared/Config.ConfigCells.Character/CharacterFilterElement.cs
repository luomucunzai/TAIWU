using System;

namespace Config.ConfigCells.Character;

[Serializable]
public struct CharacterFilterElement
{
	public int PropertyType;

	public int PropertySubType;

	public int ValueMin;

	public int ValueMax;

	public CharacterFilterElement(int[] propertyType, int valueMin, int valueMax)
	{
		PropertyType = propertyType[0];
		PropertySubType = ((propertyType.Length > 1) ? propertyType[1] : (-1));
		ValueMin = valueMin;
		ValueMax = valueMax;
	}
}
