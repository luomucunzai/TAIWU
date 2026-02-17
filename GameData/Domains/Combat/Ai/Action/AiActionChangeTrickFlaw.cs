using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007E2 RID: 2018
	[AiAction(EAiActionType.ChangeTrickFlaw)]
	public class AiActionChangeTrickFlaw : AiActionChangeTrickBase
	{
		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06006A8D RID: 27277 RVA: 0x003BCC6F File Offset: 0x003BAE6F
		protected override EFlawOrAcupointType ChangeTrickType
		{
			get
			{
				return EFlawOrAcupointType.Flaw;
			}
		}

		// Token: 0x06006A8E RID: 27278 RVA: 0x003BCC72 File Offset: 0x003BAE72
		public AiActionChangeTrickFlaw(IReadOnlyList<string> strings) : base(strings)
		{
		}
	}
}
