using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells.Character;

namespace Config;

[Serializable]
public class CharacterFilterRulesItem : ConfigItem<CharacterFilterRulesItem, short>
{
	public readonly short TemplateId;

	public readonly List<CharacterFilterElement> RulesList;

	public CharacterFilterRulesItem(short templateId, List<CharacterFilterElement> rulesList)
	{
		TemplateId = templateId;
		RulesList = rulesList;
	}

	public CharacterFilterRulesItem()
	{
		TemplateId = 0;
		RulesList = new List<CharacterFilterElement>();
	}

	public CharacterFilterRulesItem(short templateId, CharacterFilterRulesItem other)
	{
		TemplateId = templateId;
		RulesList = other.RulesList;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override CharacterFilterRulesItem Duplicate(int templateId)
	{
		return new CharacterFilterRulesItem((short)templateId, this);
	}
}
