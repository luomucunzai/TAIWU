using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007D9 RID: 2009
	[AiAction(EAiActionType.MemorySetLastPrepareCombatSkill)]
	public class AiActionMemorySetLastPrepareCombatSkill : AiActionMemorySetCharValueBase
	{
		// Token: 0x06006A7C RID: 27260 RVA: 0x003BCA74 File Offset: 0x003BAC74
		public AiActionMemorySetLastPrepareCombatSkill(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006A7D RID: 27261 RVA: 0x003BCA80 File Offset: 0x003BAC80
		protected override int GetCharValue(CombatCharacter checkChar)
		{
			return (int)checkChar.AiController.Environment.LastPrepareSkill;
		}
	}
}
