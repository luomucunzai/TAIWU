using System;
using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000780 RID: 1920
	[AiCondition(EAiConditionType.MemoryInternalAbove)]
	public class AiConditionMemoryInternalAbove : AiConditionCommonBase
	{
		// Token: 0x060069B0 RID: 27056 RVA: 0x003BA922 File Offset: 0x003B8B22
		public AiConditionMemoryInternalAbove(IReadOnlyList<string> strings)
		{
			this._keyL = strings[0];
			this._keyR = strings[1];
		}

		// Token: 0x060069B1 RID: 27057 RVA: 0x003BA948 File Offset: 0x003B8B48
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			return memory.Ints.GetOrDefault(this._keyL) > memory.Ints.GetOrDefault(this._keyR);
		}

		// Token: 0x04001D28 RID: 7464
		private readonly string _keyL;

		// Token: 0x04001D29 RID: 7465
		private readonly string _keyR;
	}
}
