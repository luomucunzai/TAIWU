using System;
using GameData.Common;
using GameData.Domains.Character.Relation;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000863 RID: 2147
	[SerializableGameData(NotForDisplayModule = true)]
	public class ProtectFriendOrFamilyAction : BasePrioritizedAction
	{
		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06007718 RID: 30488 RVA: 0x0045ADD5 File Offset: 0x00458FD5
		public override short ActionType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06007719 RID: 30489 RVA: 0x0045ADD8 File Offset: 0x00458FD8
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
				bool flag2 = !DomainManager.Character.IsCharacterAlive(this.Target.TargetCharId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					short favorability = DomainManager.Character.GetFavorability(selfChar.GetId(), this.Target.TargetCharId);
					sbyte favorType = FavorabilityType.GetFavorabilityType(favorability);
					result = (favorType >= AiHelper.PrioritizedActionConstants.PrioritizedActionMinFavorType[(int)this.ActionType]);
				}
			}
			return result;
		}

		// Token: 0x0600771A RID: 30490 RVA: 0x0045AE54 File Offset: 0x00459054
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			int groupLeader = selfChar.GetLeaderId();
			bool flag = groupLeader >= 0 && groupLeader != selfCharId;
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, selfChar, true);
			}
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddDecideToProtect(selfCharId, currDate, this.Target.TargetCharId, currLocation);
			bool flag2 = DomainManager.Character.IsTaiwuPeople(selfCharId) || DomainManager.Character.IsTaiwuPeople(this.Target.TargetCharId);
			if (flag2)
			{
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotifications.AddGoToProtect(selfCharId, this.Target.TargetCharId);
			}
		}

		// Token: 0x0600771B RID: 30491 RVA: 0x0045AF14 File Offset: 0x00459114
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddFinishProtection(selfCharId, currDate, this.Target.TargetCharId, currLocation);
		}

		// Token: 0x0600771C RID: 30492 RVA: 0x0045AF5C File Offset: 0x0045915C
		public override bool Execute(DataContext context, Character selfChar)
		{
			return false;
		}
	}
}
