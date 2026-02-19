using System;
using GameData.Common;
using GameData.Domains.Combat;

namespace GameData.Domains.Adventure.AdventureExtraRules;

[Obsolete]
public class AdvRule_DisableHealing : IAdventureExtraRule
{
	private readonly bool _isDisableOuterInjury;

	byte IAdventureExtraRule.RuleId => (byte)(_isDisableOuterInjury ? 1 : 0);

	public AdvRule_DisableHealing(bool isDisableOuterInjury)
	{
		_isDisableOuterInjury = isDisableOuterInjury;
	}

	public void ApplyChangesAtStart(DataContext context)
	{
	}

	public void ApplyChangesOnMove(DataContext context)
	{
	}

	public void ApplyChangesManually(DataContext context)
	{
	}

	public void RevertChanges(DataContext context)
	{
	}

	public void ApplyChangesToEnemyTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter)
	{
	}

	public void ApplyChangesToSelfTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter)
	{
	}
}
