using Config;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action;

[AiAction(EAiActionType.CastSkillDefendBlock)]
public class AiActionCastSkillDefendBlock : AiActionCastSkillBase
{
	protected override CombatSkillSelector Selector { get; } = new CombatSkillSelector(3, Predicate, Comparison);

	private static bool Predicate(CombatSkillItem config)
	{
		return false;
	}

	private static int Comparison(CombatSkillItem configA, CombatSkillItem configB)
	{
		return 0;
	}
}
