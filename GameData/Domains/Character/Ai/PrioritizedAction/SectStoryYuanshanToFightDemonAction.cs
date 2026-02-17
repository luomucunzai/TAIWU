using System;
using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000868 RID: 2152
	[SerializableGameData(NotForDisplayModule = true)]
	public class SectStoryYuanshanToFightDemonAction : BasePrioritizedAction
	{
		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x0600773C RID: 30524 RVA: 0x0045BFFD File Offset: 0x0045A1FD
		public override short ActionType
		{
			get
			{
				return 11;
			}
		}

		// Token: 0x0600773D RID: 30525 RVA: 0x0045C004 File Offset: 0x0045A204
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			int groupLeader = selfChar.GetLeaderId();
			bool flag = groupLeader >= 0 && groupLeader != selfCharId;
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, selfChar, true);
			}
		}

		// Token: 0x0600773E RID: 30526 RVA: 0x0045C040 File Offset: 0x0045A240
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
		}

		// Token: 0x0600773F RID: 30527 RVA: 0x0045C044 File Offset: 0x0045A244
		public override bool Execute(DataContext context, Character selfChar)
		{
			return false;
		}

		// Token: 0x06007740 RID: 30528 RVA: 0x0045C058 File Offset: 0x0045A258
		public override bool CheckValid(Character selfChar)
		{
			return false;
		}
	}
}
