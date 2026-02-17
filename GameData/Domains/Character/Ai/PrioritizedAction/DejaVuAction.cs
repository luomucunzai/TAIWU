using System;
using GameData.Common;
using GameData.Domains.World.MonthlyEvent;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000856 RID: 2134
	[SerializableGameData(NotForDisplayModule = true)]
	public class DejaVuAction : BasePrioritizedAction
	{
		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060076CC RID: 30412 RVA: 0x00458D43 File Offset: 0x00456F43
		public override short ActionType
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x060076CD RID: 30413 RVA: 0x00458D48 File Offset: 0x00456F48
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
				bool flag2 = DomainManager.Extra.GetDejaVuEventCharacters().Contains(selfChar.GetId());
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = this.Target.TargetCharId != DomainManager.Taiwu.GetTaiwuCharId();
					if (flag3)
					{
						result = false;
					}
					else
					{
						Character target;
						bool flag4 = !DomainManager.Character.TryGetElement_Objects(this.Target.TargetCharId, out target);
						result = (!flag4 && selfChar.GetLocation().AreaId == target.GetLocation().AreaId);
					}
				}
			}
			return result;
		}

		// Token: 0x060076CE RID: 30414 RVA: 0x00458DF0 File Offset: 0x00456FF0
		public override void OnStart(DataContext context, Character selfChar)
		{
			Character target;
			bool flag = !DomainManager.Character.TryGetElement_Objects(this.Target.TargetCharId, out target);
			if (!flag)
			{
				bool flag2 = selfChar.GetLeaderId() != selfChar.GetId();
				if (flag2)
				{
					DomainManager.Character.LeaveGroup(context, selfChar, true);
				}
				DomainManager.Character.GroupMove(context, selfChar, target.GetLocation());
			}
		}

		// Token: 0x060076CF RID: 30415 RVA: 0x00458E54 File Offset: 0x00457054
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
		}

		// Token: 0x060076D0 RID: 30416 RVA: 0x00458E58 File Offset: 0x00457058
		public override bool Execute(DataContext context, Character selfChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddCrossArchiveReunionWithAcquaintance(selfChar.GetId(), selfChar.GetLocation());
			return false;
		}
	}
}
