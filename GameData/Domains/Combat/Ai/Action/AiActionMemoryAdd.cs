using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007CC RID: 1996
	[AiAction(EAiActionType.MemoryAdd)]
	public class AiActionMemoryAdd : AiActionCommonBase
	{
		// Token: 0x06006A62 RID: 27234 RVA: 0x003BC8D8 File Offset: 0x003BAAD8
		public AiActionMemoryAdd(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
			this._valueExpression = CExpression.FromBase64(strings[1]);
		}

		// Token: 0x06006A63 RID: 27235 RVA: 0x003BC901 File Offset: 0x003BAB01
		public override void Execute(AiMemoryNew memory, IAiParticipant participant)
		{
			memory.Ints[this._key] = memory.Ints.GetOrDefault(this._key) + this._valueExpression.Calc(participant as IExpressionConverter);
		}

		// Token: 0x04001D63 RID: 7523
		private readonly string _key;

		// Token: 0x04001D64 RID: 7524
		private readonly CExpression _valueExpression;
	}
}
