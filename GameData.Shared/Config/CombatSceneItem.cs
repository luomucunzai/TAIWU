using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class CombatSceneItem : ConfigItem<CombatSceneItem, short>
{
	public readonly short TemplateId;

	public readonly List<string> PrefabPath;

	public CombatSceneItem(short templateId, List<string> prefabPath)
	{
		TemplateId = templateId;
		PrefabPath = prefabPath;
	}

	public CombatSceneItem()
	{
		TemplateId = 0;
		PrefabPath = new List<string> { "" };
	}

	public CombatSceneItem(short templateId, CombatSceneItem other)
	{
		TemplateId = templateId;
		PrefabPath = other.PrefabPath;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override CombatSceneItem Duplicate(int templateId)
	{
		return new CombatSceneItem((short)templateId, this);
	}
}
