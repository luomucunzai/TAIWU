using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.Delay)]
public class AiConditionDelay : AiConditionCommonBase
{
	private readonly CExpression _delayExpression;

	public AiConditionDelay(IReadOnlyList<string> strings)
	{
		_delayExpression = CExpression.FromBase64(strings[0]);
	}

	public override bool Check(AiMemoryNew memory, IAiParticipant participant)
	{
		int num = _delayExpression.Calc((IExpressionConverter)((participant is IExpressionConverter) ? participant : null));
		int num2 = memory.Ints.GetOrDefault(base.RuntimeIdStr) + 1;
		memory.Ints[base.RuntimeIdStr] = num2;
		return num2 % num == 0;
	}
}
