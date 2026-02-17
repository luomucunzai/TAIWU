using System;
using GameData.Common;
using GameData.Domains.Combat;

namespace GameData.Domains.Adventure.AdventureExtraRules
{
	// Token: 0x020008D4 RID: 2260
	[Obsolete]
	public interface IAdventureExtraRule
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06007FD7 RID: 32727
		byte RuleId { get; }

		// Token: 0x06007FD8 RID: 32728
		void ApplyChangesAtStart(DataContext context);

		// Token: 0x06007FD9 RID: 32729
		void ApplyChangesOnMove(DataContext context);

		// Token: 0x06007FDA RID: 32730
		void ApplyChangesManually(DataContext context);

		// Token: 0x06007FDB RID: 32731
		void RevertChanges(DataContext context);

		// Token: 0x06007FDC RID: 32732
		void ApplyChangesToEnemyTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter);

		// Token: 0x06007FDD RID: 32733
		void ApplyChangesToSelfTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter);
	}
}
