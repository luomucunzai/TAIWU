using System;

namespace Config.ConfigCells.Character;

[Serializable]
public struct PropertyAndValue
{
	public readonly short PropertyId;

	public readonly short Value;

	public PropertyAndValue(short propertyId, short value)
	{
		PropertyId = propertyId;
		Value = value;
	}
}
