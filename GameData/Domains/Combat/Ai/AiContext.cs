using System;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000716 RID: 1814
	public struct AiContext
	{
		// Token: 0x06006875 RID: 26741 RVA: 0x003B6CB4 File Offset: 0x003B4EB4
		public static implicit operator AiContext([TupleElementNames(new string[]
		{
			"memory",
			"participant"
		})] ValueTuple<AiMemoryNew, IAiParticipant> tup)
		{
			return new AiContext
			{
				Memory = tup.Item1,
				Participant = tup.Item2
			};
		}

		// Token: 0x06006876 RID: 26742 RVA: 0x003B6CE4 File Offset: 0x003B4EE4
		public void Deconstruct(out AiMemoryNew memory, out IAiParticipant participant)
		{
			memory = this.Memory;
			participant = this.Participant;
		}

		// Token: 0x04001C99 RID: 7321
		public AiMemoryNew Memory;

		// Token: 0x04001C9A RID: 7322
		public IAiParticipant Participant;
	}
}
