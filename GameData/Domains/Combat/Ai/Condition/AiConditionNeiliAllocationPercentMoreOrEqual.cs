using System;
using System.Collections.Generic;
using GameData.Combat.Math;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000772 RID: 1906
	[AiCondition(EAiConditionType.NeiliAllocationPercentMoreOrEqual)]
	public class AiConditionNeiliAllocationPercentMoreOrEqual : AiConditionCheckCharBase
	{
		// Token: 0x06006994 RID: 27028 RVA: 0x003BA50F File Offset: 0x003B870F
		public AiConditionNeiliAllocationPercentMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
			this._neiliAllocationType = (sbyte)ints[1];
			this._percent = ints[2];
		}

		// Token: 0x06006995 RID: 27029 RVA: 0x003BA538 File Offset: 0x003B8738
		protected unsafe override bool Check(CombatCharacter checkChar)
		{
			short neiliAllocation = *checkChar.GetNeiliAllocation()[(int)this._neiliAllocationType];
			short maxNeiliAllocation = *checkChar.GetOriginNeiliAllocation()[(int)this._neiliAllocationType];
			return CValuePercent.ParseInt((int)neiliAllocation, (int)maxNeiliAllocation) >= this._percent;
		}

		// Token: 0x04001D15 RID: 7445
		private readonly sbyte _neiliAllocationType;

		// Token: 0x04001D16 RID: 7446
		private readonly int _percent;
	}
}
