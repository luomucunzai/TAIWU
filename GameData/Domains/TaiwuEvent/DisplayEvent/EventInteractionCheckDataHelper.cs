using System;
using System.Collections.Generic;

namespace GameData.Domains.TaiwuEvent.DisplayEvent
{
	// Token: 0x020000D5 RID: 213
	public class EventInteractionCheckDataHelper
	{
		// Token: 0x060025BF RID: 9663 RVA: 0x001CD2B4 File Offset: 0x001CB4B4
		public static EventInteractCheckData EventInteractCheckData(short interactCheckTemplateId)
		{
			return new EventInteractCheckData
			{
				InteractCheckTemplateId = interactCheckTemplateId,
				PhaseProbList = new List<int>(),
				FailPhase = 5
			};
		}
	}
}
