using Config;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

public static class CostNeiliAllocationKit
{
	private const int NeiliCostValuePerGrid = 5;

	public static CastBoostEffectDisplayData GetPureCostNeiliEffectData(this CombatSkillKey skillKey, byte type, short skillId, bool applyEffect)
	{
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(skillKey.CharId);
		sbyte gridCost = Config.CombatSkill.Instance[skillId].GridCost;
		int num = gridCost * -5;
		num = (applyEffect ? element_CombatCharacterDict.ApplySpecialEffectToNeiliAllocation(type, num) : num);
		return CastBoostEffectDisplayData.CostNeiliAllocation(skillKey, type, num);
	}
}
