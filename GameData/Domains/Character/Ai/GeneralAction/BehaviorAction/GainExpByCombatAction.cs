using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction
{
	// Token: 0x020008A2 RID: 2210
	public class GainExpByCombatAction : IGeneralAction
	{
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600786B RID: 30827 RVA: 0x00464720 File Offset: 0x00462920
		public sbyte ActionEnergyType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600786C RID: 30828 RVA: 0x00464724 File Offset: 0x00462924
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return !selfChar.NeedToAvoidCombat(this.CombatType) && !targetChar.NeedToAvoidCombat(this.CombatType);
		}

		// Token: 0x0600786D RID: 30829 RVA: 0x00464758 File Offset: 0x00462958
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			Location location = targetChar.GetLocation();
			CombatType combatType = this.CombatType;
			CombatType combatType2 = combatType;
			if (combatType2 != CombatType.Play)
			{
				if (combatType2 == CombatType.Beat)
				{
					monthlyEventCollection.AddRequestNormalCombat(selfCharId, location, targetCharId);
				}
			}
			else
			{
				monthlyEventCollection.AddRequestPlayCombat(selfCharId, location, targetCharId);
			}
			CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
		}

		// Token: 0x0600786E RID: 30830 RVA: 0x004647C4 File Offset: 0x004629C4
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool flag = targetCharId == DomainManager.Taiwu.GetTaiwuCharId();
			if (!flag)
			{
				DomainManager.Character.SimulateCharacterCombat(context, selfChar, targetChar, this.CombatType, false, 1);
			}
		}

		// Token: 0x04002173 RID: 8563
		public CombatType CombatType;
	}
}
