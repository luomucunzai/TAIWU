using System;
using Config.Common;

namespace Config;

[Serializable]
public class MapBlockMatcherItem : ConfigItem<MapBlockMatcherItem, short>
{
	public readonly short TemplateId;

	public readonly EMapBlockType[] IncludeTypes;

	public readonly EMapBlockSubType[] IncludeSubTypes;

	public readonly EMapBlockType[] ExcludeTypes;

	public readonly EMapBlockSubType[] ExcludeSubTypes;

	public readonly bool ExcludeBlocksWithAdventure;

	public readonly bool ExcludeBlocksWithEffect;

	public MapBlockMatcherItem(short templateId, EMapBlockType[] includeTypes, EMapBlockSubType[] includeSubTypes, EMapBlockType[] excludeTypes, EMapBlockSubType[] excludeSubTypes, bool excludeBlocksWithAdventure, bool excludeBlocksWithEffect)
	{
		TemplateId = templateId;
		IncludeTypes = includeTypes;
		IncludeSubTypes = includeSubTypes;
		ExcludeTypes = excludeTypes;
		ExcludeSubTypes = excludeSubTypes;
		ExcludeBlocksWithAdventure = excludeBlocksWithAdventure;
		ExcludeBlocksWithEffect = excludeBlocksWithEffect;
	}

	public MapBlockMatcherItem()
	{
		TemplateId = 0;
		IncludeTypes = null;
		IncludeSubTypes = null;
		ExcludeTypes = null;
		ExcludeSubTypes = null;
		ExcludeBlocksWithAdventure = false;
		ExcludeBlocksWithEffect = false;
	}

	public MapBlockMatcherItem(short templateId, MapBlockMatcherItem other)
	{
		TemplateId = templateId;
		IncludeTypes = other.IncludeTypes;
		IncludeSubTypes = other.IncludeSubTypes;
		ExcludeTypes = other.ExcludeTypes;
		ExcludeSubTypes = other.ExcludeSubTypes;
		ExcludeBlocksWithAdventure = other.ExcludeBlocksWithAdventure;
		ExcludeBlocksWithEffect = other.ExcludeBlocksWithEffect;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override MapBlockMatcherItem Duplicate(int templateId)
	{
		return new MapBlockMatcherItem((short)templateId, this);
	}
}
