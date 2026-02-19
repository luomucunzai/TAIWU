using GameData.Domains.Character;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.OptionUseItemWine)]
public class AiConditionOptionUseItemWine : AiConditionOptionUseItemCommonBase
{
	public AiConditionOptionUseItemWine()
		: base(EItemSelectorType.Wine)
	{
	}

	protected override bool ExtraCheck(CombatCharacter combatChar)
	{
		GameData.Domains.Character.Character character = combatChar.GetCharacter();
		if (character.IsForbiddenToDrinkingWines())
		{
			return false;
		}
		return character.GetAgeGroup() == 2;
	}
}
