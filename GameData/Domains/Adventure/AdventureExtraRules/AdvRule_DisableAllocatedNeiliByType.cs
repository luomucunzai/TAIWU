using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;

namespace GameData.Domains.Adventure.AdventureExtraRules
{
	// Token: 0x020008D2 RID: 2258
	[Obsolete]
	public class AdvRule_DisableAllocatedNeiliByType : IAdventureExtraRule
	{
		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06007FC7 RID: 32711 RVA: 0x004CC727 File Offset: 0x004CA927
		byte IAdventureExtraRule.RuleId
		{
			get
			{
				return 2 + this._neiliAllocationType;
			}
		}

		// Token: 0x06007FC8 RID: 32712 RVA: 0x004CC734 File Offset: 0x004CA934
		public AdvRule_DisableAllocatedNeiliByType(byte neiliAllocationType)
		{
			this._neiliAllocationType = neiliAllocationType;
			HashSet<int> taiwuTeamIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			this._targetCharacters = new Character[taiwuTeamIds.Count];
			int index = 0;
			foreach (int charId in taiwuTeamIds)
			{
				this._targetCharacters[index] = DomainManager.Character.GetElement_Objects(charId);
			}
		}

		// Token: 0x06007FC9 RID: 32713 RVA: 0x004CC7C8 File Offset: 0x004CA9C8
		public void ApplyChangesAtStart(DataContext context)
		{
		}

		// Token: 0x06007FCA RID: 32714 RVA: 0x004CC7CB File Offset: 0x004CA9CB
		public void ApplyChangesOnMove(DataContext context)
		{
		}

		// Token: 0x06007FCB RID: 32715 RVA: 0x004CC7D0 File Offset: 0x004CA9D0
		public void ApplyChangesManually(DataContext context)
		{
			foreach (Character character in this._targetCharacters)
			{
			}
		}

		// Token: 0x06007FCC RID: 32716 RVA: 0x004CC7FA File Offset: 0x004CA9FA
		public void RevertChanges(DataContext context)
		{
		}

		// Token: 0x06007FCD RID: 32717 RVA: 0x004CC7FD File Offset: 0x004CA9FD
		public void ApplyChangesToEnemyTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter)
		{
		}

		// Token: 0x06007FCE RID: 32718 RVA: 0x004CC800 File Offset: 0x004CAA00
		public void ApplyChangesToSelfTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter)
		{
		}

		// Token: 0x0400231E RID: 8990
		private readonly byte _neiliAllocationType;

		// Token: 0x0400231F RID: 8991
		private readonly Character[] _targetCharacters;
	}
}
