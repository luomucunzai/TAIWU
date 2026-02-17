using System;
using GameData.Common;
using GameData.Domains.Character.Relation;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000853 RID: 2131
	[SerializableGameData(NotForDisplayModule = true)]
	public class AppointmentAction : BasePrioritizedAction
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060076A7 RID: 30375 RVA: 0x00457E04 File Offset: 0x00456004
		public override short ActionType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060076A8 RID: 30376 RVA: 0x00457E08 File Offset: 0x00456008
		public override bool CheckValid(Character selfChar)
		{
			bool flag = !base.CheckValid(selfChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int selfCharId = selfChar.GetId();
				bool flag2 = !DomainManager.Character.IsCharacterAlive(this.TargetCharId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					short targetSettlementId;
					bool flag3 = DomainManager.Taiwu.TryGetElement_Appointments(selfCharId, out targetSettlementId);
					if (flag3)
					{
						Settlement settlement = DomainManager.Organization.GetSettlement(targetSettlementId);
						bool flag4 = !settlement.GetLocation().Equals(this.Target.GetRealTargetLocation());
						if (flag4)
						{
							return false;
						}
					}
					short favorability = DomainManager.Character.GetFavorability(selfChar.GetId(), this.TargetCharId);
					sbyte favorType = FavorabilityType.GetFavorabilityType(favorability);
					result = (favorType >= AiHelper.PrioritizedActionConstants.PrioritizedActionMinFavorType[(int)this.ActionType]);
				}
			}
			return result;
		}

		// Token: 0x060076A9 RID: 30377 RVA: 0x00457ED4 File Offset: 0x004560D4
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			int groupLeader = selfChar.GetLeaderId();
			bool flag = groupLeader >= 0 && groupLeader != selfCharId;
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, selfChar, true);
			}
			DomainManager.Taiwu.RemoveAppointment(context, selfCharId);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location targetLocation = this.Target.GetRealTargetLocation();
			lifeRecordCollection.AddDecideToFullfillAppointment(selfCharId, currDate, targetLocation);
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotifications.AddGoToAppointment(selfCharId, targetLocation);
		}

		// Token: 0x060076AA RID: 30378 RVA: 0x00457F64 File Offset: 0x00456164
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			int selfCharId = selfChar.GetId();
			Location targetLocation = this.Target.GetRealTargetLocation();
			lifeRecordCollection.AddCanNoLongerFullFillAppointment(selfCharId, currDate, targetLocation);
			bool flag = DomainManager.Character.IsCharacterAlive(this.TargetCharId);
			if (flag)
			{
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotifications.AddAppointmentCancelled(selfCharId, targetLocation, this.TargetCharId);
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddAppointmentCancelled(selfCharId, targetLocation);
			}
		}

		// Token: 0x060076AB RID: 30379 RVA: 0x00457FF0 File Offset: 0x004561F0
		public override void OnArrival(DataContext context, Character selfChar)
		{
			base.OnArrival(context, selfChar);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			int selfCharId = selfChar.GetId();
			Location targetLocation = this.Target.GetRealTargetLocation();
			lifeRecordCollection.AddWaitForAppointment(selfCharId, currDate, this.TargetCharId, targetLocation);
		}

		// Token: 0x060076AC RID: 30380 RVA: 0x00458040 File Offset: 0x00456240
		public override bool Execute(DataContext context, Character selfChar)
		{
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			int selfCharId = selfChar.GetId();
			Location targetLocation = this.Target.GetRealTargetLocation();
			monthlyNotifications.AddWaitingForAppointment(selfCharId, targetLocation, this.TargetCharId);
			return false;
		}

		// Token: 0x060076AE RID: 30382 RVA: 0x0045808C File Offset: 0x0045628C
		public override int GetSerializedSize()
		{
			int totalSize = 21;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060076AF RID: 30383 RVA: 0x004580B4 File Offset: 0x004562B4
		public unsafe override int Serialize(byte* pData)
		{
			*(int*)pData = this.TargetCharId;
			byte* pCurrData = pData + 4;
			pCurrData += this.Target.Serialize(pCurrData);
			*pCurrData = (this.HasArrived ? 1 : 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060076B0 RID: 30384 RVA: 0x00458108 File Offset: 0x00456308
		public unsafe override int Deserialize(byte* pData)
		{
			this.TargetCharId = *(int*)pData;
			byte* pCurrData = pData + 4;
			pCurrData += this.Target.Deserialize(pCurrData);
			this.HasArrived = (*pCurrData != 0);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040020C7 RID: 8391
		[SerializableGameDataField]
		public int TargetCharId;
	}
}
