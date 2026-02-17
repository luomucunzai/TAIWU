using System;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x0200084E RID: 2126
	public static class NpcTravelTargetHelper
	{
		// Token: 0x0600767E RID: 30334 RVA: 0x00455410 File Offset: 0x00453610
		public static Location GetRealTargetLocation(this NpcTravelTarget target)
		{
			Location fixedLocation;
			bool flag = target.TryGetFixedLocation(out fixedLocation);
			Location result;
			if (flag)
			{
				result = fixedLocation;
			}
			else
			{
				Character targetChar;
				bool flag2 = DomainManager.Character.TryGetElement_Objects(target.TargetCharId, out targetChar);
				if (flag2)
				{
					Location location = targetChar.GetLocation();
					bool flag3 = location.IsValid();
					if (flag3)
					{
						result = location;
					}
					else
					{
						int targetLeaderId = targetChar.GetLeaderId();
						CrossAreaMoveInfo moveInfo;
						bool flag4 = DomainManager.Character.TryGetElement_CrossAreaMoveInfos((targetLeaderId >= 0) ? targetLeaderId : target.TargetCharId, out moveInfo);
						if (flag4)
						{
							MapAreaData targetAreaData = DomainManager.Map.GetElement_Areas((int)moveInfo.ToAreaId);
							location = new Location(moveInfo.ToAreaId, targetAreaData.StationBlockId);
						}
						else
						{
							location = targetChar.GetValidLocation();
						}
						result = location;
					}
				}
				else
				{
					Grave targetGrave;
					bool flag5 = DomainManager.Character.TryGetElement_Graves(target.TargetCharId, out targetGrave);
					if (flag5)
					{
						Location location2 = targetGrave.GetLocation();
						result = location2;
					}
					else
					{
						result = Location.Invalid;
					}
				}
			}
			return result;
		}

		// Token: 0x0600767F RID: 30335 RVA: 0x00455504 File Offset: 0x00453704
		public static bool IsTargetInteractable(this NpcTravelTarget target)
		{
			Location location;
			bool flag = target.TryGetFixedLocation(out location);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				Character character;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(target.TargetCharId, out character);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = character.IsActiveExternalRelationState(60);
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = DomainManager.Taiwu.IsInGroup(target.TargetCharId) && DomainManager.Map.IsTraveling;
						result = (!flag4 && !character.IsCrossAreaTraveling());
					}
				}
			}
			return result;
		}
	}
}
