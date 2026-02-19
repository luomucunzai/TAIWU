using System;

namespace Config.ConfigCells;

[Serializable]
public struct CombatStateProperty
{
	public short SpecialEffectDataId;

	public short Value;

	public sbyte ModifyType;

	public CombatStateProperty(short specialEffectDataId, short value, sbyte modifyType)
	{
		SpecialEffectDataId = specialEffectDataId;
		Value = value;
		ModifyType = modifyType;
	}
}
