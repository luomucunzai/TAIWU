using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007BC RID: 1980
	public abstract class AiActionMemorySetCharValueBase : AiActionCombatBase
	{
		// Token: 0x06006A39 RID: 27193 RVA: 0x003BC2C3 File Offset: 0x003BA4C3
		protected AiActionMemorySetCharValueBase(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			this._key = strings[0];
			this._isAlly = (ints[0] == 1);
		}

		// Token: 0x06006A3A RID: 27194 RVA: 0x003BC2EC File Offset: 0x003BA4EC
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			CombatCharacter checkChar = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == this._isAlly, false);
			memory.Ints[this._key] = this.GetCharValue(checkChar);
		}

		// Token: 0x06006A3B RID: 27195
		protected abstract int GetCharValue(CombatCharacter checkChar);

		// Token: 0x04001D53 RID: 7507
		private readonly string _key;

		// Token: 0x04001D54 RID: 7508
		private readonly bool _isAlly;
	}
}
