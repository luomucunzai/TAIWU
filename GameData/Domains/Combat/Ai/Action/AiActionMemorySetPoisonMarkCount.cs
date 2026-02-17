using System;
using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007D4 RID: 2004
	[AiAction(EAiActionType.MemorySetPoisonMarkCount)]
	public class AiActionMemorySetPoisonMarkCount : AiActionMemorySetMarkCountBase
	{
		// Token: 0x06006A72 RID: 27250 RVA: 0x003BC9C2 File Offset: 0x003BABC2
		public AiActionMemorySetPoisonMarkCount(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006A73 RID: 27251 RVA: 0x003BC9CE File Offset: 0x003BABCE
		protected override int GetMarkCount(DefeatMarkCollection marks)
		{
			return marks.PoisonMarkList.Sum();
		}
	}
}
