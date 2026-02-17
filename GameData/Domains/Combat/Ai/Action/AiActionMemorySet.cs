using System;
using System.Collections.Generic;
using GameData.Combat.Math;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007CB RID: 1995
	[AiAction(EAiActionType.MemorySet)]
	public class AiActionMemorySet : AiActionCommonBase
	{
		// Token: 0x06006A60 RID: 27232 RVA: 0x003BC889 File Offset: 0x003BAA89
		public AiActionMemorySet(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
			this._valueExpression = CExpression.FromBase64(strings[1]);
		}

		// Token: 0x06006A61 RID: 27233 RVA: 0x003BC8B2 File Offset: 0x003BAAB2
		public override void Execute(AiMemoryNew memory, IAiParticipant participant)
		{
			memory.Ints[this._key] = this._valueExpression.Calc(participant as IExpressionConverter);
		}

		// Token: 0x04001D61 RID: 7521
		private readonly string _key;

		// Token: 0x04001D62 RID: 7522
		private readonly CExpression _valueExpression;
	}
}
