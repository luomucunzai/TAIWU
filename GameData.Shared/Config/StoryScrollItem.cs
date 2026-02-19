using System;
using Config.Common;

namespace Config;

[Serializable]
public class StoryScrollItem : ConfigItem<StoryScrollItem, short>
{
	public readonly short TemplateId;

	public readonly sbyte StoryBoss;

	public readonly sbyte StorySect;

	public readonly string StoryTypeIcon;

	public readonly string StoryResultMark;

	public readonly string StoryImage;

	public readonly string StoryNote;

	public StoryScrollItem(short templateId, sbyte storyBoss, sbyte storySect, string storyTypeIcon, int storyResultMark, string storyImage, int storyNote)
	{
		TemplateId = templateId;
		StoryBoss = storyBoss;
		StorySect = storySect;
		StoryTypeIcon = storyTypeIcon;
		StoryResultMark = LocalStringManager.GetConfig("StoryScroll_language", storyResultMark);
		StoryImage = storyImage;
		StoryNote = LocalStringManager.GetConfig("StoryScroll_language", storyNote);
	}

	public StoryScrollItem()
	{
		TemplateId = 0;
		StoryBoss = 0;
		StorySect = 0;
		StoryTypeIcon = null;
		StoryResultMark = null;
		StoryImage = null;
		StoryNote = null;
	}

	public StoryScrollItem(short templateId, StoryScrollItem other)
	{
		TemplateId = templateId;
		StoryBoss = other.StoryBoss;
		StorySect = other.StorySect;
		StoryTypeIcon = other.StoryTypeIcon;
		StoryResultMark = other.StoryResultMark;
		StoryImage = other.StoryImage;
		StoryNote = other.StoryNote;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override StoryScrollItem Duplicate(int templateId)
	{
		return new StoryScrollItem((short)templateId, this);
	}
}
