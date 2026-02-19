using System;
using Config.Common;

namespace Config;

[Serializable]
public class CharacterTitleItem : ConfigItem<CharacterTitleItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly int Duration;

	public CharacterTitleItem(short templateId, int name, int duration)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("CharacterTitle_language", name);
		Duration = duration;
	}

	public CharacterTitleItem()
	{
		TemplateId = 0;
		Name = null;
		Duration = -1;
	}

	public CharacterTitleItem(short templateId, CharacterTitleItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Duration = other.Duration;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override CharacterTitleItem Duplicate(int templateId)
	{
		return new CharacterTitleItem((short)templateId, this);
	}
}
