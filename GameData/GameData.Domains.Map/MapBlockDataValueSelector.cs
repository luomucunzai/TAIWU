using CompDevLib.Interpreter;

namespace GameData.Domains.Map;

public class MapBlockDataValueSelector : IFieldValueSelector<MapBlockData>, IFieldValueSelector
{
	public ValueInfo SelectValue(MapBlockData blockData, Evaluator evaluator, string identifier)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		if (1 == 0)
		{
		}
		ValueInfo result = ((identifier == "Settlement") ? evaluator.PushEvaluationResult((object)DomainManager.Organization.GetSettlementByLocation(blockData.GetRootBlock().GetLocation())) : ((!(identifier == "SubBlocks")) ? ValueInfo.Void : evaluator.PushEvaluationResult((object)blockData.GroupBlockList)));
		if (1 == 0)
		{
		}
		return result;
	}
}
