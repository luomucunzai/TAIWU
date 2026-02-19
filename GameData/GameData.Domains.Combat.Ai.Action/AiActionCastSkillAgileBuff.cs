using Config;
using GameData.Domains.Combat.Ai.Selector;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Action;

[AiAction(EAiActionType.CastSkillAgileBuff)]
public class AiActionCastSkillAgileBuff : AiActionCastSkillBase
{
	protected override CombatSkillSelector Selector { get; } = new CombatSkillSelector(2, Predicate, Comparison);

	private static bool Predicate(CombatSkillItem config)
	{
		return config.AddHitOnCast.Sum() > 0;
	}

	private static int Comparison(CombatSkillItem configA, CombatSkillItem configB)
	{
		return (configA.AddHitOnCast.Sum() != configB.AddHitOnCast.Sum()) ? configB.AddHitOnCast.Sum().CompareTo(configA.AddHitOnCast.Sum()) : 0;
	}
}
