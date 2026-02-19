using System.Collections.Generic;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

public class OperationQuestionRhetorical : OperationQuestion
{
	public OperationQuestionRhetorical()
	{
	}

	public OperationQuestionRhetorical(sbyte playerId, int stamp, int gridIndex, int basePoint, IEnumerable<sbyte> wantUseEffectCards)
		: base(playerId, stamp, gridIndex, basePoint, wantUseEffectCards)
	{
	}
}
