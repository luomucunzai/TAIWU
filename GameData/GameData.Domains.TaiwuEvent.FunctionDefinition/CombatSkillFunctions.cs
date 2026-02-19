using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class CombatSkillFunctions
{
	[EventFunction(64)]
	private static void SetCharCombatSkillPracticeLevel(EventScriptRuntime runtime, GameData.Domains.Character.Character character, short skillTemplateId, int practiceLevel)
	{
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillTemplateId));
		element_CombatSkills.SetPracticeLevel((sbyte)practiceLevel, runtime.Context);
	}
}
