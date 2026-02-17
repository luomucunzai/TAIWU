using System;
using System.Collections.Generic;
using Config;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007F1 RID: 2033
	[AiAction(EAiActionType.PrioritySetLow)]
	public class AiActionPrioritySetLow : AiActionCombatBase
	{
		// Token: 0x06006AB1 RID: 27313 RVA: 0x003BD3F2 File Offset: 0x003BB5F2
		public AiActionPrioritySetLow(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
		}

		// Token: 0x06006AB2 RID: 27314 RVA: 0x003BD408 File Offset: 0x003BB608
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			int skillId;
			bool flag = !memory.Ints.TryGetValue(this._key, out skillId) || skillId < 0 || skillId >= CombatSkill.Instance.Count;
			if (!flag)
			{
				memory.SetPriority((short)skillId, EAiPriority.Low);
			}
		}

		// Token: 0x04001D79 RID: 7545
		private readonly string _key;
	}
}
