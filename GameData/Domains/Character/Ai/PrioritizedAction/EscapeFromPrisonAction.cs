using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000857 RID: 2135
	[SerializableGameData(NotForDisplayModule = true, IsExtensible = true, NoCopyConstructors = true)]
	public class EscapeFromPrisonAction : ExtensiblePrioritizedAction
	{
		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060076D2 RID: 30418 RVA: 0x00458E92 File Offset: 0x00457092
		public override short ActionType
		{
			get
			{
				return 18;
			}
		}

		// Token: 0x060076D3 RID: 30419 RVA: 0x00458E98 File Offset: 0x00457098
		public override bool CheckValid(Character selfChar)
		{
			bool flag = selfChar.GetOrganizationInfo().OrgTemplateId != 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = DomainManager.Organization.GetFugitiveBountySect(selfChar.GetId()) < 0;
				result = (!flag2 && base.CheckValid(selfChar));
			}
			return result;
		}

		// Token: 0x060076D4 RID: 30420 RVA: 0x00458EE4 File Offset: 0x004570E4
		public override void OnStart(DataContext context, Character selfChar)
		{
			string srcArea = DomainManager.Map.GetElement_Areas((int)selfChar.GetLocation().AreaId).GetConfig().Name;
			string dstArea = DomainManager.Map.GetElement_Areas((int)this.Target.GetRealTargetLocation().AreaId).GetConfig().Name;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 3);
			defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
			defaultInterpolatedStringHandler.AppendLiteral(" 开始畏罪潜逃: ");
			defaultInterpolatedStringHandler.AppendFormatted(srcArea);
			defaultInterpolatedStringHandler.AppendLiteral(" => ");
			defaultInterpolatedStringHandler.AppendFormatted(dstArea);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			bool flag = selfChar.GetLeaderId() >= 0;
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, selfChar, true);
			}
			int selfCharId = selfChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			sbyte bountySectId = DomainManager.Organization.GetFugitiveBountySect(selfChar.GetId());
			Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(bountySectId);
			short settlementId = sect.GetId();
			SettlementBounty bounty = sect.Prison.GetBounty(selfCharId);
			Location targetLocation = new Location(this.Target.GetRealTargetLocation().AreaId, -1);
			LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecords.AddDecideToEscapePunishment(selfCharId, currDate, location, bounty.PunishmentType, settlementId, targetLocation);
		}

		// Token: 0x060076D5 RID: 30421 RVA: 0x00459044 File Offset: 0x00457244
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
			defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
			defaultInterpolatedStringHandler.AppendLiteral(" 结束了畏罪潜逃.");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			int selfCharId = selfChar.GetId();
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecords.AddFinishEscapePunishment(selfCharId, currDate, location);
			sbyte idealSectId = selfChar.GetIdealSect();
			bool flag = idealSectId >= 0 && idealSectId != DomainManager.Organization.GetFugitiveBountySect(selfCharId);
			if (flag)
			{
				selfChar.AddPersonalNeed(context, PersonalNeed.CreatePersonalNeed(26, idealSectId));
			}
		}

		// Token: 0x060076D6 RID: 30422 RVA: 0x004590E8 File Offset: 0x004572E8
		public override void OnArrival(DataContext context, Character selfChar)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 2);
			defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
			defaultInterpolatedStringHandler.AppendLiteral(" 到达目的地: ");
			defaultInterpolatedStringHandler.AppendFormatted(DomainManager.Map.GetElement_Areas((int)selfChar.GetValidLocation().AreaId).GetConfig().Name);
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			base.OnArrival(context, selfChar);
		}

		// Token: 0x060076D7 RID: 30423 RVA: 0x00459154 File Offset: 0x00457354
		public override bool Execute(DataContext context, Character selfChar)
		{
			return false;
		}
	}
}
