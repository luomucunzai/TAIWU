using System;
using Config;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Implement
{
	// Token: 0x02000581 RID: 1409
	public static class CostNeiliAllocationKit
	{
		// Token: 0x060041C1 RID: 16833 RVA: 0x00264028 File Offset: 0x00262228
		public static CastBoostEffectDisplayData GetPureCostNeiliEffectData(this CombatSkillKey skillKey, byte type, short skillId, bool applyEffect)
		{
			CombatCharacter combatChar = DomainManager.Combat.GetElement_CombatCharacterDict(skillKey.CharId);
			sbyte gridCost = Config.CombatSkill.Instance[skillId].GridCost;
			int costValue = (int)(gridCost * -5);
			costValue = (applyEffect ? combatChar.ApplySpecialEffectToNeiliAllocation(type, costValue) : costValue);
			return CastBoostEffectDisplayData.CostNeiliAllocation(skillKey, type, costValue);
		}

		// Token: 0x04001362 RID: 4962
		private const int NeiliCostValuePerGrid = 5;
	}
}
