using System;
using System.Collections.Generic;
using GameData.Domains.Character.Ai.PrioritizedAction;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000834 RID: 2100
	public class PrioritizedActionModification
	{
		// Token: 0x06007584 RID: 30084 RVA: 0x0044A63B File Offset: 0x0044883B
		public PrioritizedActionModification(Character character)
		{
			this.Character = character;
		}

		// Token: 0x04001FC5 RID: 8133
		public readonly Character Character;

		// Token: 0x04001FC6 RID: 8134
		public BasePrioritizedAction Action = null;

		// Token: 0x04001FC7 RID: 8135
		public bool IsNewAction = false;

		// Token: 0x04001FC8 RID: 8136
		public List<short> FailToCreateActions = null;
	}
}
