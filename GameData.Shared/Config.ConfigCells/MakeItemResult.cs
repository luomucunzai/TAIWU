using System;
using GameData.Domains.Item;

namespace Config.ConfigCells;

[Serializable]
public struct MakeItemResult
{
	public sbyte ItemType;

	public short TemplateId;

	public MakeItemResult(string typeName, short templateId)
	{
		ItemType = GameData.Domains.Item.ItemType.TypeName2TypeId[typeName];
		TemplateId = templateId;
	}
}
