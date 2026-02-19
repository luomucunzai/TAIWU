using System;
using GameData.Domains.Item;

namespace Config.ConfigCells;

[Serializable]
public struct PresetItemTemplateId
{
	public sbyte ItemType;

	public short TemplateId;

	public PresetItemTemplateId(string type, short templateId)
	{
		ItemType = GameData.Domains.Item.ItemType.TypeName2TypeId[type];
		TemplateId = templateId;
	}
}
