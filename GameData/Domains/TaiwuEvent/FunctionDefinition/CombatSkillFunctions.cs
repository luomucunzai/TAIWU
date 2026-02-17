using System;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000A3 RID: 163
	public class CombatSkillFunctions
	{
		// Token: 0x06001AB8 RID: 6840 RVA: 0x0017931C File Offset: 0x0017751C
		[EventFunction(64)]
		private static void SetCharCombatSkillPracticeLevel(EventScriptRuntime runtime, Character character, short skillTemplateId, int practiceLevel)
		{
			CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillTemplateId));
			skill.SetPracticeLevel((sbyte)practiceLevel, runtime.Context);
		}
	}
}
