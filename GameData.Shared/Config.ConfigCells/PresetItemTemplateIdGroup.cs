using System;
using GameData.Domains.Item;

namespace Config.ConfigCells;

[Serializable]
public struct PresetItemTemplateIdGroup
{
	public sbyte ItemType;

	public short StartId;

	public sbyte GroupLength;

	public PresetItemTemplateIdGroup(string typeName, short startId, sbyte groupLength)
	{
		ItemType = GameData.Domains.Item.ItemType.TypeName2TypeId[typeName];
		StartId = startId;
		GroupLength = groupLength;
	}
}
