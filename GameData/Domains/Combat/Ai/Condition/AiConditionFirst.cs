using System;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200074C RID: 1868
	[AiCondition(EAiConditionType.First)]
	public class AiConditionFirst : AiConditionCommonBase
	{
		// Token: 0x06006947 RID: 26951 RVA: 0x003B9B14 File Offset: 0x003B7D14
		public override bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			bool orDefault = memory.Booleans.GetOrDefault(base.RuntimeIdStr);
			bool result;
			if (orDefault)
			{
				result = false;
			}
			else
			{
				memory.Booleans[base.RuntimeIdStr] = true;
				result = true;
			}
			return result;
		}
	}
}
