using System;

namespace Config.ConfigCells;

[Serializable]
public struct PropertyAndValueAndModifyType
{
	public readonly short PropertyId;

	public readonly short Value;

	public readonly sbyte ModifyType;

	public PropertyAndValueAndModifyType(short propertyId, short value, sbyte modifyType)
	{
		PropertyId = propertyId;
		Value = value;
		ModifyType = modifyType;
	}
}
