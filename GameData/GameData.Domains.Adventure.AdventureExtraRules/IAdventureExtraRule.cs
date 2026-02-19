using System;
using GameData.Common;
using GameData.Domains.Combat;

namespace GameData.Domains.Adventure.AdventureExtraRules;

[Obsolete]
public interface IAdventureExtraRule
{
	byte RuleId { get; }

	void ApplyChangesAtStart(DataContext context);

	void ApplyChangesOnMove(DataContext context);

	void ApplyChangesManually(DataContext context);

	void RevertChanges(DataContext context);

	void ApplyChangesToEnemyTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter);

	void ApplyChangesToSelfTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter);
}
