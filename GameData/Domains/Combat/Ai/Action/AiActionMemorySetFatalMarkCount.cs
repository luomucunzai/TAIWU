using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007D6 RID: 2006
	[AiAction(EAiActionType.MemorySetFatalMarkCount)]
	public class AiActionMemorySetFatalMarkCount : AiActionMemorySetMarkCountBase
	{
		// Token: 0x06006A76 RID: 27254 RVA: 0x003BC9F4 File Offset: 0x003BABF4
		public AiActionMemorySetFatalMarkCount(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006A77 RID: 27255 RVA: 0x003BCA00 File Offset: 0x003BAC00
		protected override int GetMarkCount(DefeatMarkCollection marks)
		{
			return marks.FatalDamageMarkCount;
		}
	}
}
