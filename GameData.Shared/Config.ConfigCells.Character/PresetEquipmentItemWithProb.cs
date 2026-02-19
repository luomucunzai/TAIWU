using System;
using GameData.Domains.Item;

namespace Config.ConfigCells.Character;

[Serializable]
public struct PresetEquipmentItemWithProb
{
	public sbyte Type;

	public short TemplateId;

	public sbyte Prob;

	public PresetEquipmentItemWithProb(string typeName, short templateId, sbyte prob)
	{
		Type = ItemType.TypeName2TypeId[typeName];
		TemplateId = templateId;
		Prob = prob;
	}
}
