using System;
using Config.Common;

namespace Config;

[Serializable]
public class BehaviorTypeItem : ConfigItem<BehaviorTypeItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string Desc;

	public readonly sbyte ExchangeBook;

	public readonly string Icon;

	public readonly string[] BetrayTips;

	public BehaviorTypeItem(short templateId, int name, int desc, sbyte exchangeBook, string icon, int[] betrayTips)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("BehaviorType_language", name);
		Desc = LocalStringManager.GetConfig("BehaviorType_language", desc);
		ExchangeBook = exchangeBook;
		Icon = icon;
		BetrayTips = LocalStringManager.ConvertConfigList("BehaviorType_language", betrayTips);
	}

	public BehaviorTypeItem()
	{
		TemplateId = 0;
		Name = null;
		Desc = null;
		ExchangeBook = 0;
		Icon = null;
		BetrayTips = null;
	}

	public BehaviorTypeItem(short templateId, BehaviorTypeItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Desc = other.Desc;
		ExchangeBook = other.ExchangeBook;
		Icon = other.Icon;
		BetrayTips = other.BetrayTips;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override BehaviorTypeItem Duplicate(int templateId)
	{
		return new BehaviorTypeItem((short)templateId, this);
	}
}
