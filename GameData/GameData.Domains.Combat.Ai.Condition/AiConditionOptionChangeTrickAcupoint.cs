using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.OptionChangeTrickAcupoint)]
public class AiConditionOptionChangeTrickAcupoint : AiConditionCombatBase
{
	private readonly sbyte _bodyPart;

	public AiConditionOptionChangeTrickAcupoint(IReadOnlyList<int> ints)
	{
		_bodyPart = (sbyte)ints[0];
	}

	public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
	{
		if (combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoAttack)
		{
			return false;
		}
		if (combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoChangeTrick)
		{
			return false;
		}
		if (!DomainManager.Combat.CanNormalAttack(combatChar.IsAlly) || !combatChar.GetCanChangeTrick())
		{
			return false;
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!combatChar.IsAlly);
		if (combatCharacter.GetAcupointCount()[_bodyPart] >= combatCharacter.GetMaxAcupointCount())
		{
			return false;
		}
		if (!combatCharacter.ContainsBodyPart(_bodyPart))
		{
			return false;
		}
		int num = CFormulaHelper.CalcCostChangeTrickCount(combatChar, EFlawOrAcupointType.Acupoint);
		return combatChar.GetChangeTrickCount() >= num;
	}
}
