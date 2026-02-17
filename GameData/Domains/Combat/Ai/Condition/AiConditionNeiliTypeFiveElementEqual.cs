using System;
using System.Collections.Generic;
using Config;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000755 RID: 1877
	[AiCondition(EAiConditionType.NeiliTypeFiveElementEqual)]
	public class AiConditionNeiliTypeFiveElementEqual : AiConditionCheckCharBase
	{
		// Token: 0x06006959 RID: 26969 RVA: 0x003B9D79 File Offset: 0x003B7F79
		public AiConditionNeiliTypeFiveElementEqual(IReadOnlyList<int> ints) : base(ints)
		{
			this._fiveElementsType = (sbyte)ints[1];
		}

		// Token: 0x0600695A RID: 26970 RVA: 0x003B9D94 File Offset: 0x003B7F94
		protected override bool Check(CombatCharacter checkChar)
		{
			return NeiliType.Instance[checkChar.GetNeiliType()].FiveElements == (byte)this._fiveElementsType;
		}

		// Token: 0x04001D05 RID: 7429
		private readonly sbyte _fiveElementsType;
	}
}
