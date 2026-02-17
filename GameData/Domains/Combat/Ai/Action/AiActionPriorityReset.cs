using System;
using System.Collections.Generic;
using Config;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007F2 RID: 2034
	[AiAction(EAiActionType.PriorityReset)]
	public class AiActionPriorityReset : AiActionCombatBase
	{
		// Token: 0x06006AB3 RID: 27315 RVA: 0x003BD452 File Offset: 0x003BB652
		public AiActionPriorityReset(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
		}

		// Token: 0x06006AB4 RID: 27316 RVA: 0x003BD468 File Offset: 0x003BB668
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			int skillId;
			bool flag = !memory.Ints.TryGetValue(this._key, out skillId) || skillId < 0 || skillId >= CombatSkill.Instance.Count;
			if (!flag)
			{
				memory.SetPriority((short)skillId, EAiPriority.Middle);
			}
		}

		// Token: 0x04001D7A RID: 7546
		private readonly string _key;
	}
}
