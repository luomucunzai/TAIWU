using System;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x0200085C RID: 2140
	[SerializableGameData(NotForDisplayModule = true)]
	public class GuardTreasuryAction : BasePrioritizedAction
	{
		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060076EF RID: 30447 RVA: 0x00459924 File Offset: 0x00457B24
		public override short ActionType
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x060076F0 RID: 30448 RVA: 0x00459928 File Offset: 0x00457B28
		public override bool CheckValid(Character selfChar)
		{
			short settlementId = selfChar.GetOrganizationInfo().SettlementId;
			bool flag = settlementId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				bool flag2 = !settlement.Treasuries.IsGuard(selfChar.GetId());
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = DomainManager.LegendaryBook.IsCharacterLegendaryBookOwnerOrContest(selfChar.GetId());
					result = (!flag3 && base.CheckValid(selfChar));
				}
			}
			return result;
		}

		// Token: 0x060076F1 RID: 30449 RVA: 0x004599A0 File Offset: 0x00457BA0
		public override void OnStart(DataContext context, Character selfChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int charId = selfChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			short settlementId = selfChar.GetOrganizationInfo().SettlementId;
			Location location = selfChar.GetLocation();
			lifeRecordCollection.AddDecideToGuardTreasury(charId, currDate, location, settlementId);
			bool flag = selfChar.GetLeaderId() != selfChar.GetId();
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, selfChar, true);
			}
		}

		// Token: 0x060076F2 RID: 30450 RVA: 0x00459A10 File Offset: 0x00457C10
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int charId = selfChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			short settlementId = DomainManager.Organization.GetSettlementByLocation(this.Target.GetRealTargetLocation()).GetId();
			Location location = selfChar.GetLocation();
			lifeRecordCollection.AddFinishGuardingTreasury(charId, currDate, location, settlementId);
			selfChar.RemoveFeatureGroup(context, 536);
		}

		// Token: 0x060076F3 RID: 30451 RVA: 0x00459A78 File Offset: 0x00457C78
		public override bool Execute(DataContext context, Character selfChar)
		{
			return false;
		}
	}
}
