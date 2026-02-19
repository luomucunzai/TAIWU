using GameData.Domains.Character;

namespace GameData.Domains.Taiwu;

public static class CombatSkillPlanHelper
{
	public static void Record(this CombatSkillPlan plan, GameData.Domains.Character.Character character)
	{
		CombatSkillEquipment combatSkillEquipment = character.GetCombatSkillEquipment();
		CombatSkillPlan sourceObject = combatSkillEquipment.GetSourceObject<CombatSkillPlan>();
		if (sourceObject != null)
		{
			plan.CopyFrom(sourceObject);
			return;
		}
		short[] equippedCombatSkills = character.GetEquippedCombatSkills();
		plan.Record(equippedCombatSkills);
	}
}
