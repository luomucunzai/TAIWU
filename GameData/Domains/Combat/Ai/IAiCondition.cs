using System;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000712 RID: 1810
	public interface IAiCondition
	{
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06006850 RID: 26704
		int GroupId { get; }

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06006851 RID: 26705
		// (set) Token: 0x06006852 RID: 26706
		int RuntimeId { get; set; }

		// Token: 0x06006853 RID: 26707
		bool Check(AiMemoryNew memory, IAiParticipant participant);
	}
}
