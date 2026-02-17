using System;
using System.Collections.Generic;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007DA RID: 2010
	[AiAction(EAiActionType.MemorySetBestAttackCombatSkill)]
	public class AiActionMemorySetBestAttackCombatSkill : AiActionCombatBase
	{
		// Token: 0x06006A7E RID: 27262 RVA: 0x003BCA92 File Offset: 0x003BAC92
		public AiActionMemorySetBestAttackCombatSkill(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
		}

		// Token: 0x06006A7F RID: 27263 RVA: 0x003BCAB4 File Offset: 0x003BACB4
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			memory.Ints[this._key] = (int)this._selector.Select(memory, combatChar);
		}

		// Token: 0x04001D67 RID: 7527
		private readonly CombatSkillSelector _selector = new CombatSkillSelector(1);

		// Token: 0x04001D68 RID: 7528
		private readonly string _key;
	}
}
