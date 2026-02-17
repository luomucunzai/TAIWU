using System;
using GameData.Common;
using GameData.Domains.Combat;

namespace GameData.Domains.Adventure.AdventureExtraRules
{
	// Token: 0x020008D3 RID: 2259
	[Obsolete]
	public class AdvRule_DisableHealing : IAdventureExtraRule
	{
		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06007FCF RID: 32719 RVA: 0x004CC803 File Offset: 0x004CAA03
		byte IAdventureExtraRule.RuleId
		{
			get
			{
				return this._isDisableOuterInjury ? 1 : 0;
			}
		}

		// Token: 0x06007FD0 RID: 32720 RVA: 0x004CC811 File Offset: 0x004CAA11
		public AdvRule_DisableHealing(bool isDisableOuterInjury)
		{
			this._isDisableOuterInjury = isDisableOuterInjury;
		}

		// Token: 0x06007FD1 RID: 32721 RVA: 0x004CC822 File Offset: 0x004CAA22
		public void ApplyChangesAtStart(DataContext context)
		{
		}

		// Token: 0x06007FD2 RID: 32722 RVA: 0x004CC825 File Offset: 0x004CAA25
		public void ApplyChangesOnMove(DataContext context)
		{
		}

		// Token: 0x06007FD3 RID: 32723 RVA: 0x004CC828 File Offset: 0x004CAA28
		public void ApplyChangesManually(DataContext context)
		{
		}

		// Token: 0x06007FD4 RID: 32724 RVA: 0x004CC82B File Offset: 0x004CAA2B
		public void RevertChanges(DataContext context)
		{
		}

		// Token: 0x06007FD5 RID: 32725 RVA: 0x004CC82E File Offset: 0x004CAA2E
		public void ApplyChangesToEnemyTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter)
		{
		}

		// Token: 0x06007FD6 RID: 32726 RVA: 0x004CC831 File Offset: 0x004CAA31
		public void ApplyChangesToSelfTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter)
		{
		}

		// Token: 0x04002320 RID: 8992
		private readonly bool _isDisableOuterInjury;
	}
}
