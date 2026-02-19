using System.Collections.Generic;
using Config;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.OptionTeammateCommand)]
public class AiConditionOptionTeammateCommand : AiConditionCombatBase
{
	private readonly ETeammateCommandImplement _implement;

	public AiConditionOptionTeammateCommand(IReadOnlyList<int> ints)
	{
		_implement = (ETeammateCommandImplement)ints[0];
	}

	public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
	{
		if (combatChar.GetShowTransferInjuryCommand() && _implement != ETeammateCommandImplement.TransferInjury)
		{
			return false;
		}
		bool[] autoUseTeammateCommand = DomainManager.Combat.AiOptions.AutoUseTeammateCommand;
		ETeammateCommandOption valueOrDefault = CombatDomain.TeammateCommandOptions.GetValueOrDefault(_implement, ETeammateCommandOption.Invalid);
		if (combatChar.IsAlly && (!autoUseTeammateCommand.CheckIndex((int)valueOrDefault) || !autoUseTeammateCommand[(int)valueOrDefault]))
		{
			return false;
		}
		foreach (CombatCharacter teammateCharacter in DomainManager.Combat.GetTeammateCharacters(combatChar.GetId()))
		{
			List<sbyte> currTeammateCommands = teammateCharacter.GetCurrTeammateCommands();
			List<bool> teammateCommandCanUse = teammateCharacter.GetTeammateCommandCanUse();
			for (int i = 0; i < currTeammateCommands.Count; i++)
			{
				if (teammateCommandCanUse.CheckIndex(i) && teammateCommandCanUse[i] && (combatChar.GetShowTransferInjuryCommand() || IsMatch(currTeammateCommands[i])))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool IsMatch(sbyte cmdType)
	{
		return TeammateCommand.Instance[cmdType].Implement == _implement;
	}
}
