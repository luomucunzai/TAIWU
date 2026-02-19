using GameData.Common;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Action;

public abstract class AiActionCastSkillBase : AiActionCombatBase
{
	protected abstract CombatSkillSelector Selector { get; }

	public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
	{
		DataContext context = DomainManager.Combat.Context;
		short num = Selector.Select(memory, combatChar);
		if (num > 0)
		{
			DomainManager.Combat.StartPrepareSkill(context, num, combatChar.IsAlly);
		}
	}
}
