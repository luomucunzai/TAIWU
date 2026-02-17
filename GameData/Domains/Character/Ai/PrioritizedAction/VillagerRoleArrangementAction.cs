using System;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x0200086B RID: 2155
	[SerializableGameData(NotForDisplayModule = true)]
	public class VillagerRoleArrangementAction : BasePrioritizedAction
	{
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06007757 RID: 30551 RVA: 0x0045C8CB File Offset: 0x0045AACB
		public override short ActionType
		{
			get
			{
				return 21;
			}
		}

		// Token: 0x06007758 RID: 30552 RVA: 0x0045C8D0 File Offset: 0x0045AAD0
		public override bool CheckValid(Character selfChar)
		{
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(selfChar.GetId());
			bool flag = villagerRole == null || villagerRole.ArrangementTemplateId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = villagerRole.WorkData == null;
				if (flag2)
				{
					result = false;
				}
				else
				{
					Location workLocation = villagerRole.WorkData.Location;
					bool flag3 = workLocation.BlockId >= 0 && !DomainManager.Map.CheckLocationsHasSameRoot(workLocation, this.Target.GetRealTargetLocation());
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = workLocation.BlockId < 0 && workLocation.AreaId != this.Target.GetRealTargetLocation().AreaId;
						result = (!flag4 && base.CheckValid(selfChar));
					}
				}
			}
			return result;
		}

		// Token: 0x06007759 RID: 30553 RVA: 0x0045C994 File Offset: 0x0045AB94
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(selfChar.GetId());
			Location location = villagerRole.WorkData.Location;
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection.AddVillagerPrioritizedActions(selfCharId, currDate, location);
		}

		// Token: 0x0600775A RID: 30554 RVA: 0x0045C9E8 File Offset: 0x0045ABE8
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection.AddVillagerPrioritizedActionsStop(selfCharId, currDate, location);
		}

		// Token: 0x0600775B RID: 30555 RVA: 0x0045CA24 File Offset: 0x0045AC24
		public override bool Execute(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(selfCharId);
			IVillagerRoleArrangementExecutor action = (IVillagerRoleArrangementExecutor)villagerRole;
			action.ExecuteArrangementAction(context, this);
			Location nextLocation = action.SelectNextWorkLocation(context.Random, villagerRole.WorkData.Location);
			this.Target = new NpcTravelTarget(nextLocation, this.Target.RemainingMonth);
			return false;
		}
	}
}
