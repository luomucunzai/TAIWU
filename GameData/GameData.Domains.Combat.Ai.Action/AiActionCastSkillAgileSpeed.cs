using Config;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action;

[AiAction(EAiActionType.CastSkillAgileSpeed)]
public class AiActionCastSkillAgileSpeed : AiActionCastSkillBase
{
	protected override CombatSkillSelector Selector { get; } = new CombatSkillSelector(2, Predicate, Comparison);

	private static bool Predicate(CombatSkillItem config)
	{
		return true;
	}

	private static int Comparison(CombatSkillItem configA, CombatSkillItem configB)
	{
		if (configA.AddPercentMoveSpeedOnCast != configB.AddPercentMoveSpeedOnCast)
		{
			return configB.AddPercentMoveSpeedOnCast.CompareTo(configA.AddPercentMoveSpeedOnCast);
		}
		return (configA.AddMoveSpeedOnCast != configB.AddMoveSpeedOnCast) ? configB.AddMoveSpeedOnCast.CompareTo(configA.AddMoveSpeedOnCast) : 0;
	}
}
