using System;
using CompDevLib.Interpreter;

namespace GameData.Domains.Map
{
	// Token: 0x020008BC RID: 2236
	public class MapBlockDataValueSelector : IFieldValueSelector<MapBlockData>, IFieldValueSelector
	{
		// Token: 0x06007BC8 RID: 31688 RVA: 0x0048FB8C File Offset: 0x0048DD8C
		public ValueInfo SelectValue(MapBlockData blockData, Evaluator evaluator, string identifier)
		{
			if (!true)
			{
			}
			ValueInfo result;
			if (!(identifier == "Settlement"))
			{
				if (!(identifier == "SubBlocks"))
				{
					result = ValueInfo.Void;
				}
				else
				{
					result = evaluator.PushEvaluationResult(blockData.GroupBlockList);
				}
			}
			else
			{
				result = evaluator.PushEvaluationResult(DomainManager.Organization.GetSettlementByLocation(blockData.GetRootBlock().GetLocation()));
			}
			if (!true)
			{
			}
			return result;
		}
	}
}
