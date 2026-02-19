using System;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Organization;

namespace GameData.Domains.TaiwuEvent.EventOption;

public class OptionConditionAreaSpiritualDebt : TaiwuEventOptionConditionBase
{
	public readonly short SpiritualDebtValue;

	public readonly Func<MapAreaData, short, bool> ConditionChecker;

	public OptionConditionAreaSpiritualDebt(short id, short spiritualDebtValue, Func<MapAreaData, short, bool> checkFunc)
		: base(id)
	{
		SpiritualDebtValue = spiritualDebtValue;
		ConditionChecker = checkFunc;
	}

	public override bool CheckCondition(EventArgBox box)
	{
		GameData.Domains.Character.Character character = box.GetCharacter("CharacterId");
		if (character != null)
		{
			short settlementId = character.GetOrganizationInfo().SettlementId;
			if (settlementId >= 0)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(settlement.GetLocation().AreaId);
				return ConditionChecker(element_Areas, SpiritualDebtValue);
			}
		}
		return false;
	}

	public bool CheckCondition(short settlementId)
	{
		if (settlementId >= 0)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(settlement.GetLocation().AreaId);
			return ConditionChecker(element_Areas, SpiritualDebtValue);
		}
		return false;
	}

	public override (short, string[]) GetDisplayData(EventArgBox box)
	{
		string text = string.Empty;
		GameData.Domains.Character.Character character = box.GetCharacter("CharacterId");
		if (character != null)
		{
			short settlementId = character.GetOrganizationInfo().SettlementId;
			if (settlementId >= 0)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(settlement.GetLocation().AreaId);
				MapAreaItem item = MapArea.Instance.GetItem(element_Areas.GetTemplateId());
				if (item != null)
				{
					text = item.Name;
				}
			}
		}
		return (Id, new string[2]
		{
			text,
			SpiritualDebtValue.ToString()
		});
	}

	public (short, string[]) GetDisplayData(short settlementId)
	{
		string text = string.Empty;
		if (settlementId >= 0)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(settlement.GetLocation().AreaId);
			MapAreaItem item = MapArea.Instance.GetItem(element_Areas.GetTemplateId());
			if (item != null)
			{
				text = item.Name;
			}
		}
		return (Id, new string[2]
		{
			text,
			SpiritualDebtValue.ToString()
		});
	}
}
