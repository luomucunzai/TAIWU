using System;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x0200070F RID: 1807
	public interface IAiAction
	{
		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06006847 RID: 26695
		int GroupId { get; }

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06006848 RID: 26696
		// (set) Token: 0x06006849 RID: 26697
		int RuntimeId { get; set; }

		// Token: 0x0600684A RID: 26698
		void Execute(AiMemoryNew memory, IAiParticipant participant);
	}
}
