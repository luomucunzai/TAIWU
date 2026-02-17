using System;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007B7 RID: 1975
	public abstract class AiActionCommonBase : IAiAction
	{
		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06006A24 RID: 27172 RVA: 0x003BC104 File Offset: 0x003BA304
		public int GroupId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06006A25 RID: 27173 RVA: 0x003BC107 File Offset: 0x003BA307
		// (set) Token: 0x06006A26 RID: 27174 RVA: 0x003BC10F File Offset: 0x003BA30F
		public int RuntimeId { get; set; }

		// Token: 0x06006A27 RID: 27175
		public abstract void Execute(AiMemoryNew memory, IAiParticipant participant);
	}
}
