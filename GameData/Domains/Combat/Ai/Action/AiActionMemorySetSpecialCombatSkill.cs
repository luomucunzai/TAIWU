using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007D8 RID: 2008
	[AiAction(EAiActionType.MemorySetSpecialCombatSkill)]
	public class AiActionMemorySetSpecialCombatSkill : AiActionCombatBase
	{
		// Token: 0x06006A7A RID: 27258 RVA: 0x003BCA34 File Offset: 0x003BAC34
		public AiActionMemorySetSpecialCombatSkill(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			this._key = strings[0];
			this._skillId = (short)ints[0];
		}

		// Token: 0x06006A7B RID: 27259 RVA: 0x003BCA59 File Offset: 0x003BAC59
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			memory.Ints[this._key] = (int)this._skillId;
		}

		// Token: 0x04001D65 RID: 7525
		private readonly string _key;

		// Token: 0x04001D66 RID: 7526
		private readonly short _skillId;
	}
}
