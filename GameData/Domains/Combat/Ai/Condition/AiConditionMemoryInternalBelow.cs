using System;
using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000781 RID: 1921
	[AiCondition(EAiConditionType.MemoryInternalBelow)]
	public class AiConditionMemoryInternalBelow : AiConditionCommonBase
	{
		// Token: 0x060069B2 RID: 27058 RVA: 0x003BA97E File Offset: 0x003B8B7E
		public AiConditionMemoryInternalBelow(IReadOnlyList<string> strings)
		{
			this._keyL = strings[0];
			this._keyR = strings[1];
		}

		// Token: 0x060069B3 RID: 27059 RVA: 0x003BA9A4 File Offset: 0x003B8BA4
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			return memory.Ints.GetOrDefault(this._keyL) < memory.Ints.GetOrDefault(this._keyR);
		}

		// Token: 0x04001D2A RID: 7466
		private readonly string _keyL;

		// Token: 0x04001D2B RID: 7467
		private readonly string _keyR;
	}
}
