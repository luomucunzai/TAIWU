using System;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Organization;

namespace GameData.Domains.TaiwuEvent.EventOption;

public static class OptionConsumeHelper
{
	public static bool HasConsumeResource(this OptionConsumeInfo info, int charAId, int charBId)
	{
		if (info.ConsumeType == 8)
		{
			return DomainManager.World.GetLeftDaysInCurrMonth() >= info.ConsumeCount;
		}
		if (info.ConsumeType == 9 && DomainManager.Character.TryGetElement_Objects(charBId, out var element) && element != null)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(element.GetOrganizationInfo().SettlementId);
			return DomainManager.Extra.GetAreaSpiritualDebt(settlement.GetLocation().AreaId) >= info.ConsumeCount;
		}
		if (info.ConsumeType == 10 && DomainManager.Character.TryGetElement_Objects(charAId, out var element2) && element2 != null)
		{
			return DomainManager.Extra.GetAreaSpiritualDebt(element2.GetLocation().AreaId) >= info.ConsumeCount;
		}
		if (DomainManager.Character.TryGetElement_Objects(charAId, out var element3))
		{
			sbyte consumeType = info.ConsumeType;
			if (consumeType >= 0 && consumeType < 8)
			{
				return element3.GetResource(info.ConsumeType) >= info.ConsumeCount;
			}
		}
		throw new Exception($"character {charAId} not found exception");
	}

	public static int GetHoldCount(this OptionConsumeInfo info, int charAId, int charBId)
	{
		if (info.ConsumeType == 8)
		{
			return DomainManager.Extra.GetTotalActionPointsRemaining() / 10;
		}
		if (info.ConsumeType == 9 && DomainManager.Character.TryGetElement_Objects(charBId, out var element) && element != null)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(element.GetOrganizationInfo().SettlementId);
			return DomainManager.Extra.GetAreaSpiritualDebt(settlement.GetLocation().AreaId);
		}
		if (info.ConsumeType == 10 && DomainManager.Character.TryGetElement_Objects(charAId, out var element2) && element2 != null)
		{
			return DomainManager.Extra.GetAreaSpiritualDebt(element2.GetLocation().AreaId);
		}
		if (DomainManager.Character.TryGetElement_Objects(charAId, out var element3))
		{
			sbyte consumeType = info.ConsumeType;
			if (consumeType >= 0 && consumeType < 8)
			{
				return element3.GetResource(info.ConsumeType);
			}
		}
		throw new Exception($"character {charAId} not found exception");
	}

	public static bool DoConsume(this OptionConsumeInfo info, int charIdA, int charIdB)
	{
		if (!info.AutoConsume)
		{
			return false;
		}
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (info.ConsumeType == 8)
		{
			if (charIdA != DomainManager.Taiwu.GetTaiwuCharId())
			{
				throw new Exception("consume move point can only called by taiwu exception");
			}
			DomainManager.World.AdvanceDaysInMonth(mainThreadDataContext, info.ConsumeCount);
			return true;
		}
		if (info.ConsumeType == 9 && DomainManager.Character.TryGetElement_Objects(charIdB, out var element) && element != null)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(element.GetOrganizationInfo().SettlementId);
			DataContext mainThreadDataContext2 = DomainManager.TaiwuEvent.MainThreadDataContext;
			DomainManager.Extra.ChangeAreaSpiritualDebt(mainThreadDataContext2, settlement.GetLocation().AreaId, -info.ConsumeCount);
			return true;
		}
		if (info.ConsumeType == 10 && DomainManager.Character.TryGetElement_Objects(charIdA, out var element2) && element2 != null)
		{
			DataContext mainThreadDataContext3 = DomainManager.TaiwuEvent.MainThreadDataContext;
			DomainManager.Extra.ChangeAreaSpiritualDebt(mainThreadDataContext3, element2.GetLocation().AreaId, -info.ConsumeCount);
			return true;
		}
		if (info.ConsumeType <= 7 && DomainManager.Character.TryGetElement_Objects(charIdA, out var element3))
		{
			element3.ChangeResource(mainThreadDataContext, info.ConsumeType, -info.ConsumeCount);
			return true;
		}
		return false;
	}

	public static OptionConsumeInfo ModifyOptionConsumeInfo(OptionConsumeInfo consumeInfo, EventArgBox argBox)
	{
		switch (consumeInfo.ConsumeType)
		{
		case 9:
		{
			GameData.Domains.Character.Character character = argBox.GetCharacter("CharacterId");
			short settlementId = character.GetOrganizationInfo().SettlementId;
			consumeInfo.ConsumeCount = DomainManager.Taiwu.GetSpiritualDebtFinalCost(settlementId, (short)consumeInfo.ConsumeCount);
			break;
		}
		case 10:
		{
			Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
			location = DomainManager.Map.GetBlock(location).GetRootBlock().GetLocation();
			short id = DomainManager.Organization.GetSettlementByLocation(location).GetId();
			consumeInfo.ConsumeCount = DomainManager.Taiwu.GetSpiritualDebtFinalCost(id, (short)consumeInfo.ConsumeCount);
			break;
		}
		}
		return consumeInfo;
	}
}
