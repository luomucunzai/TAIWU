using GameData.Common;
using GameData.Domains.Character.Relation;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class AppointmentAction : BasePrioritizedAction
{
	[SerializableGameDataField]
	public int TargetCharId;

	public override short ActionType => 1;

	public override bool CheckValid(Character selfChar)
	{
		if (!base.CheckValid(selfChar))
		{
			return false;
		}
		int id = selfChar.GetId();
		if (!DomainManager.Character.IsCharacterAlive(TargetCharId))
		{
			return false;
		}
		if (DomainManager.Taiwu.TryGetElement_Appointments(id, out var value))
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(value);
			if (!settlement.GetLocation().Equals(Target.GetRealTargetLocation()))
			{
				return false;
			}
		}
		short favorability = DomainManager.Character.GetFavorability(selfChar.GetId(), TargetCharId);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		return favorabilityType >= AiHelper.PrioritizedActionConstants.PrioritizedActionMinFavorType[ActionType];
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		int leaderId = selfChar.GetLeaderId();
		if (leaderId >= 0 && leaderId != id)
		{
			DomainManager.Character.LeaveGroup(context, selfChar);
		}
		DomainManager.Taiwu.RemoveAppointment(context, id);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location realTargetLocation = Target.GetRealTargetLocation();
		lifeRecordCollection.AddDecideToFullfillAppointment(id, currDate, realTargetLocation);
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyNotificationCollection.AddGoToAppointment(id, realTargetLocation);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		int id = selfChar.GetId();
		Location realTargetLocation = Target.GetRealTargetLocation();
		lifeRecordCollection.AddCanNoLongerFullFillAppointment(id, currDate, realTargetLocation);
		if (DomainManager.Character.IsCharacterAlive(TargetCharId))
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddAppointmentCancelled(id, realTargetLocation, TargetCharId);
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddAppointmentCancelled(id, realTargetLocation);
		}
	}

	public override void OnArrival(DataContext context, Character selfChar)
	{
		base.OnArrival(context, selfChar);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		int id = selfChar.GetId();
		Location realTargetLocation = Target.GetRealTargetLocation();
		lifeRecordCollection.AddWaitForAppointment(id, currDate, TargetCharId, realTargetLocation);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		int id = selfChar.GetId();
		Location realTargetLocation = Target.GetRealTargetLocation();
		monthlyNotificationCollection.AddWaitingForAppointment(id, realTargetLocation, TargetCharId);
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 21;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = TargetCharId;
		ptr += 4;
		ptr += Target.Serialize(ptr);
		*ptr = (HasArrived ? ((byte)1) : ((byte)0));
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TargetCharId = *(int*)ptr;
		ptr += 4;
		ptr += Target.Deserialize(ptr);
		HasArrived = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
