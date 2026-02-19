using System;
using Config.Common;

namespace Config;

[Serializable]
public class UpdateLogItem : ConfigItem<UpdateLogItem, byte>
{
	public readonly byte TemplateId;

	public readonly sbyte IncrementSortOrder;

	public readonly string VersionTagBackground;

	public readonly string VersionTagIcon;

	public readonly string VersionTitle;

	public readonly string VersionPublishDate;

	public readonly string OfficialLink;

	public readonly string[] SubentryTitles;

	public readonly string[] SubentryIcons;

	public readonly string[] SubentryDescriptions;

	public UpdateLogItem(byte templateId, sbyte incrementSortOrder, string versionTagBackground, string versionTagIcon, int versionTitle, string versionPublishDate, string officialLink, int[] subentryTitles, string[] subentryIcons, int[] subentryDescriptions)
	{
		TemplateId = templateId;
		IncrementSortOrder = incrementSortOrder;
		VersionTagBackground = versionTagBackground;
		VersionTagIcon = versionTagIcon;
		VersionTitle = LocalStringManager.GetConfig("UpdateLog_language", versionTitle);
		VersionPublishDate = versionPublishDate;
		OfficialLink = officialLink;
		SubentryTitles = LocalStringManager.ConvertConfigList("UpdateLog_language", subentryTitles);
		SubentryIcons = subentryIcons;
		SubentryDescriptions = LocalStringManager.ConvertConfigList("UpdateLog_language", subentryDescriptions);
	}

	public UpdateLogItem()
	{
		TemplateId = 0;
		IncrementSortOrder = 0;
		VersionTagBackground = null;
		VersionTagIcon = null;
		VersionTitle = null;
		VersionPublishDate = null;
		OfficialLink = null;
		SubentryTitles = null;
		SubentryIcons = null;
		SubentryDescriptions = null;
	}

	public UpdateLogItem(byte templateId, UpdateLogItem other)
	{
		TemplateId = templateId;
		IncrementSortOrder = other.IncrementSortOrder;
		VersionTagBackground = other.VersionTagBackground;
		VersionTagIcon = other.VersionTagIcon;
		VersionTitle = other.VersionTitle;
		VersionPublishDate = other.VersionPublishDate;
		OfficialLink = other.OfficialLink;
		SubentryTitles = other.SubentryTitles;
		SubentryIcons = other.SubentryIcons;
		SubentryDescriptions = other.SubentryDescriptions;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override UpdateLogItem Duplicate(int templateId)
	{
		return new UpdateLogItem((byte)templateId, this);
	}
}
