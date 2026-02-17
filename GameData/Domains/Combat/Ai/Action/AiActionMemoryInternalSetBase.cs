using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007BB RID: 1979
	public abstract class AiActionMemoryInternalSetBase<T> : AiActionCommonBase
	{
		// Token: 0x06006A36 RID: 27190 RVA: 0x003BC263 File Offset: 0x003BA463
		protected AiActionMemoryInternalSetBase(IReadOnlyList<string> strings)
		{
			this._keyL = strings[0];
			this._keyR = strings[1];
		}

		// Token: 0x06006A37 RID: 27191 RVA: 0x003BC288 File Offset: 0x003BA488
		public override void Execute(AiMemoryNew memory, IAiParticipant participant)
		{
			IDictionary<string, T> dict = this.GetMemoryDict(memory);
			T value;
			bool flag = !dict.TryGetValue(this._keyR, out value);
			if (!flag)
			{
				dict[this._keyL] = value;
			}
		}

		// Token: 0x06006A38 RID: 27192
		protected abstract IDictionary<string, T> GetMemoryDict(AiMemoryNew memory);

		// Token: 0x04001D51 RID: 7505
		private readonly string _keyL;

		// Token: 0x04001D52 RID: 7506
		private readonly string _keyR;
	}
}
