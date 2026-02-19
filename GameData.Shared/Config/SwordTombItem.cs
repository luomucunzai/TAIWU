using System;
using Config.Common;

namespace Config;

[Serializable]
public class SwordTombItem : ConfigItem<SwordTombItem, sbyte>
{
	public readonly sbyte TemplateId;

	public readonly short SwordTombAdventure;

	public readonly short XiangshuAvatarBegin;

	public readonly short WeakenedXiangshuAvatarBegin;

	public readonly short JuniorXiangshuAvatar;

	public readonly short PuppetXiangshuAvatar;

	public readonly short ImmortalXiangshuAvatar;

	public readonly short[] Legacies;

	public readonly short BigEventWhenRemoved;

	public SwordTombItem(sbyte templateId, short swordTombAdventure, short xiangshuAvatarBegin, short weakenedXiangshuAvatarBegin, short juniorXiangshuAvatar, short puppetXiangshuAvatar, short immortalXiangshuAvatar, short[] legacies, short bigEventWhenRemoved)
	{
		TemplateId = templateId;
		SwordTombAdventure = swordTombAdventure;
		XiangshuAvatarBegin = xiangshuAvatarBegin;
		WeakenedXiangshuAvatarBegin = weakenedXiangshuAvatarBegin;
		JuniorXiangshuAvatar = juniorXiangshuAvatar;
		PuppetXiangshuAvatar = puppetXiangshuAvatar;
		ImmortalXiangshuAvatar = immortalXiangshuAvatar;
		Legacies = legacies;
		BigEventWhenRemoved = bigEventWhenRemoved;
	}

	public SwordTombItem()
	{
		TemplateId = 0;
		SwordTombAdventure = 0;
		XiangshuAvatarBegin = 0;
		WeakenedXiangshuAvatarBegin = 0;
		JuniorXiangshuAvatar = 0;
		PuppetXiangshuAvatar = 0;
		ImmortalXiangshuAvatar = 0;
		Legacies = null;
		BigEventWhenRemoved = 0;
	}

	public SwordTombItem(sbyte templateId, SwordTombItem other)
	{
		TemplateId = templateId;
		SwordTombAdventure = other.SwordTombAdventure;
		XiangshuAvatarBegin = other.XiangshuAvatarBegin;
		WeakenedXiangshuAvatarBegin = other.WeakenedXiangshuAvatarBegin;
		JuniorXiangshuAvatar = other.JuniorXiangshuAvatar;
		PuppetXiangshuAvatar = other.PuppetXiangshuAvatar;
		ImmortalXiangshuAvatar = other.ImmortalXiangshuAvatar;
		Legacies = other.Legacies;
		BigEventWhenRemoved = other.BigEventWhenRemoved;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override SwordTombItem Duplicate(int templateId)
	{
		return new SwordTombItem((sbyte)templateId, this);
	}
}
