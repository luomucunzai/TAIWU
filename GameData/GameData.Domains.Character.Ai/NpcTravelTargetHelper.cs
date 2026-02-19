using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai;

public static class NpcTravelTargetHelper
{
	public static Location GetRealTargetLocation(this NpcTravelTarget target)
	{
		if (target.TryGetFixedLocation(out var location))
		{
			return location;
		}
		if (DomainManager.Character.TryGetElement_Objects(target.TargetCharId, out var element))
		{
			Location location2 = element.GetLocation();
			if (location2.IsValid())
			{
				return location2;
			}
			int leaderId = element.GetLeaderId();
			if (DomainManager.Character.TryGetElement_CrossAreaMoveInfos((leaderId >= 0) ? leaderId : target.TargetCharId, out var value))
			{
				MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(value.ToAreaId);
				location2 = new Location(value.ToAreaId, element_Areas.StationBlockId);
			}
			else
			{
				location2 = element.GetValidLocation();
			}
			return location2;
		}
		if (DomainManager.Character.TryGetElement_Graves(target.TargetCharId, out var element2))
		{
			return element2.GetLocation();
		}
		return Location.Invalid;
	}

	public static bool IsTargetInteractable(this NpcTravelTarget target)
	{
		if (target.TryGetFixedLocation(out var _))
		{
			return true;
		}
		if (!DomainManager.Character.TryGetElement_Objects(target.TargetCharId, out var element))
		{
			return true;
		}
		if (element.IsActiveExternalRelationState(60))
		{
			return false;
		}
		if (DomainManager.Taiwu.IsInGroup(target.TargetCharId) && DomainManager.Map.IsTraveling)
		{
			return false;
		}
		return !element.IsCrossAreaTraveling();
	}
}
