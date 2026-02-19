using System.Collections.Generic;
using System.Linq;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.OptionProactiveSkillType)]
public class AiConditionOptionProactiveSkillType : AiConditionCombatBase
{
	private readonly sbyte _equipType;

	private static bool IsValid(short skillId)
	{
		return skillId >= 0;
	}

	public AiConditionOptionProactiveSkillType(IReadOnlyList<int> ints)
	{
		_equipType = (sbyte)ints[0];
	}

	public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
	{
		sbyte equipType = _equipType;
		if (1 == 0)
		{
		}
		int num = equipType switch
		{
			1 => 0, 
			2 => 1, 
			3 => 2, 
			_ => -1, 
		};
		if (1 == 0)
		{
		}
		int num2 = num;
		if (combatChar.IsAlly && (num2 < 0 || !DomainManager.Combat.AiOptions.AutoCastSkill[num2]))
		{
			return false;
		}
		return combatChar.GetCombatSkillList(_equipType).Where(IsValid).Where(combatChar.AiCanCast)
			.Any();
	}
}
