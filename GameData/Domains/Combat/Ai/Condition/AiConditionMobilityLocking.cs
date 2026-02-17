using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000787 RID: 1927
	[AiCondition(EAiConditionType.MobilityLocking)]
	public class AiConditionMobilityLocking : AiConditionCheckCharBase
	{
		// Token: 0x060069BE RID: 27070 RVA: 0x003BAB66 File Offset: 0x003B8D66
		public AiConditionMobilityLocking(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x060069BF RID: 27071 RVA: 0x003BAB71 File Offset: 0x003B8D71
		protected override bool Check(CombatCharacter checkChar)
		{
			return false;
		}
	}
}
