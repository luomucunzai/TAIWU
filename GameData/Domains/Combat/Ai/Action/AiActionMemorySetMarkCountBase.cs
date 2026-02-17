using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007BD RID: 1981
	public abstract class AiActionMemorySetMarkCountBase : AiActionMemorySetCharValueBase
	{
		// Token: 0x06006A3C RID: 27196 RVA: 0x003BC32D File Offset: 0x003BA52D
		protected AiActionMemorySetMarkCountBase(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006A3D RID: 27197 RVA: 0x003BC33C File Offset: 0x003BA53C
		protected sealed override int GetCharValue(CombatCharacter checkChar)
		{
			DefeatMarkCollection marks = checkChar.GetDefeatMarkCollection();
			return this.GetMarkCount(marks);
		}

		// Token: 0x06006A3E RID: 27198
		protected abstract int GetMarkCount(DefeatMarkCollection marks);
	}
}
