using System;
using GameData.Common;
using GameData.Domains.Character.Relation;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x0200086C RID: 2156
	[SerializableGameData(NotForDisplayModule = true)]
	public class VisitFriendOrFamilyAction : BasePrioritizedAction
	{
		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x0600775D RID: 30557 RVA: 0x0045CA94 File Offset: 0x0045AC94
		public override short ActionType
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x0600775E RID: 30558 RVA: 0x0045CA98 File Offset: 0x0045AC98
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
				Character character;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(this.Target.TargetCharId, out character);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = character.IsCompletelyInfected();
					if (flag3)
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
			}
			return result;
		}

		// Token: 0x0600775F RID: 30559 RVA: 0x0045CB2C File Offset: 0x0045AD2C
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddDecideToVisit(selfCharId, currDate, this.Target.TargetCharId, currLocation);
		}

		// Token: 0x06007760 RID: 30560 RVA: 0x0045CB74 File Offset: 0x0045AD74
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddFinishVisit(selfCharId, currDate, this.Target.TargetCharId, currLocation);
		}

		// Token: 0x06007761 RID: 30561 RVA: 0x0045CBBC File Offset: 0x0045ADBC
		public override bool Execute(DataContext context, Character selfChar)
		{
			PersonalNeed need = PersonalNeed.CreatePersonalNeed(19, this.Target.TargetCharId);
			selfChar.AddPersonalNeed(context, need);
			return false;
		}
	}
}
