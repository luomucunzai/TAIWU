using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007D7 RID: 2007
	[AiAction(EAiActionType.MemorySetChangeTrickCountByFlawCost)]
	public class AiActionMemorySetChangeTrickCountByFlawCost : AiActionMemorySetCharValueBase
	{
		// Token: 0x06006A78 RID: 27256 RVA: 0x003BCA08 File Offset: 0x003BAC08
		public AiActionMemorySetChangeTrickCountByFlawCost(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings, ints)
		{
		}

		// Token: 0x06006A79 RID: 27257 RVA: 0x003BCA14 File Offset: 0x003BAC14
		protected override int GetCharValue(CombatCharacter checkChar)
		{
			return (int)checkChar.GetChangeTrickCount() / CFormulaHelper.CalcCostChangeTrickCount(checkChar, EFlawOrAcupointType.Flaw);
		}
	}
}
