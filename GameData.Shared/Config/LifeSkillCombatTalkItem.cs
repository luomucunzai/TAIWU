using System;
using Config.Common;

namespace Config;

[Serializable]
public class LifeSkillCombatTalkItem : ConfigItem<LifeSkillCombatTalkItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string NormalContent;

	public readonly bool NeedRepalceType;

	public readonly string JustContent;

	public readonly string KindContent;

	public readonly string EvenContent;

	public readonly string RebelContent;

	public readonly string EgoisticContent;

	public LifeSkillCombatTalkItem(short templateId, int name, int normalContent, bool needRepalceType, int justContent, int kindContent, int evenContent, int rebelContent, int egoisticContent)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("LifeSkillCombatTalk_language", name);
		NormalContent = LocalStringManager.GetConfig("LifeSkillCombatTalk_language", normalContent);
		NeedRepalceType = needRepalceType;
		JustContent = LocalStringManager.GetConfig("LifeSkillCombatTalk_language", justContent);
		KindContent = LocalStringManager.GetConfig("LifeSkillCombatTalk_language", kindContent);
		EvenContent = LocalStringManager.GetConfig("LifeSkillCombatTalk_language", evenContent);
		RebelContent = LocalStringManager.GetConfig("LifeSkillCombatTalk_language", rebelContent);
		EgoisticContent = LocalStringManager.GetConfig("LifeSkillCombatTalk_language", egoisticContent);
	}

	public LifeSkillCombatTalkItem()
	{
		TemplateId = 0;
		Name = null;
		NormalContent = null;
		NeedRepalceType = false;
		JustContent = null;
		KindContent = null;
		EvenContent = null;
		RebelContent = null;
		EgoisticContent = null;
	}

	public LifeSkillCombatTalkItem(short templateId, LifeSkillCombatTalkItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		NormalContent = other.NormalContent;
		NeedRepalceType = other.NeedRepalceType;
		JustContent = other.JustContent;
		KindContent = other.KindContent;
		EvenContent = other.EvenContent;
		RebelContent = other.RebelContent;
		EgoisticContent = other.EgoisticContent;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override LifeSkillCombatTalkItem Duplicate(int templateId)
	{
		return new LifeSkillCombatTalkItem((short)templateId, this);
	}
}
