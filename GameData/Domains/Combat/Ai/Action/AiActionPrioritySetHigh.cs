using System;
using System.Collections.Generic;
using Config;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007F0 RID: 2032
	[AiAction(EAiActionType.PrioritySetHigh)]
	public class AiActionPrioritySetHigh : AiActionCombatBase
	{
		// Token: 0x06006AAF RID: 27311 RVA: 0x003BD391 File Offset: 0x003BB591
		public AiActionPrioritySetHigh(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
		}

		// Token: 0x06006AB0 RID: 27312 RVA: 0x003BD3A8 File Offset: 0x003BB5A8
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			int skillId;
			bool flag = !memory.Ints.TryGetValue(this._key, out skillId) || skillId < 0 || skillId >= CombatSkill.Instance.Count;
			if (!flag)
			{
				memory.SetPriority((short)skillId, EAiPriority.High);
			}
		}

		// Token: 0x04001D78 RID: 7544
		private readonly string _key;
	}
}
