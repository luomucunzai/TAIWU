using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000751 RID: 1873
	[AiCondition(EAiConditionType.TrickCountMoreOrEqual)]
	public class AiConditionTrickCountMoreOrEqual : AiConditionCheckCharBase
	{
		// Token: 0x06006951 RID: 26961 RVA: 0x003B9C5E File Offset: 0x003B7E5E
		public AiConditionTrickCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
			this._trickType = (sbyte)ints[1];
			this._trickCount = ints[2];
		}

		// Token: 0x06006952 RID: 26962 RVA: 0x003B9C84 File Offset: 0x003B7E84
		protected override bool Check(CombatCharacter checkChar)
		{
			return (int)checkChar.GetTrickCount(this._trickType) >= this._trickCount;
		}

		// Token: 0x04001D00 RID: 7424
		private readonly sbyte _trickType;

		// Token: 0x04001D01 RID: 7425
		private readonly int _trickCount;
	}
}
