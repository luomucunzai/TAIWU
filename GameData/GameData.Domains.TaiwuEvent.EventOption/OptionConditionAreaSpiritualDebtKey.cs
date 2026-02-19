using System;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Map;

namespace GameData.Domains.TaiwuEvent.EventOption;

public class OptionConditionAreaSpiritualDebtKey : TaiwuEventOptionConditionBase
{
	[Obsolete]
	public readonly string AreaArgBoxKey;

	public readonly short SpiritualDebtValue;

	public readonly Func<MapAreaData, short, bool> ConditionChecker;

	public OptionConditionAreaSpiritualDebtKey(short id, string key, short spiritualDebtValue, Func<MapAreaData, short, bool> checkFunc)
		: base(id)
	{
		AreaArgBoxKey = key;
		SpiritualDebtValue = spiritualDebtValue;
		ConditionChecker = checkFunc;
	}

	public override bool CheckCondition(EventArgBox box)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(location.AreaId);
		return ConditionChecker(element_Areas, SpiritualDebtValue);
	}

	public override (short, string[]) GetDisplayData(EventArgBox box)
	{
		string text = string.Empty;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(location.AreaId);
		MapAreaItem item = MapArea.Instance.GetItem(element_Areas.GetTemplateId());
		if (item != null)
		{
			text = item.Name;
		}
		return (Id, new string[2]
		{
			text,
			SpiritualDebtValue.ToString()
		});
	}
}
