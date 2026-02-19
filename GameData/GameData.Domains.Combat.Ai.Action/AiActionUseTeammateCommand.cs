using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Action;

[AiAction(EAiActionType.UseTeammateCommand)]
public class AiActionUseTeammateCommand : AiActionCombatBase
{
	private readonly ETeammateCommandImplement _implement;

	public AiActionUseTeammateCommand(IReadOnlyList<int> ints)
	{
		_implement = (ETeammateCommandImplement)ints[0];
	}

	public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
	{
		if (combatChar.GetShowTransferInjuryCommand() && _implement != ETeammateCommandImplement.TransferInjury)
		{
			return;
		}
		DataContext context = DomainManager.Combat.Context;
		foreach (CombatCharacter teammateCharacter in DomainManager.Combat.GetTeammateCharacters(combatChar.GetId()))
		{
			List<sbyte> currTeammateCommands = teammateCharacter.GetCurrTeammateCommands();
			List<bool> teammateCommandCanUse = teammateCharacter.GetTeammateCommandCanUse();
			for (int i = 0; i < currTeammateCommands.Count; i++)
			{
				if (teammateCommandCanUse.CheckIndex(i) && teammateCommandCanUse[i] && (combatChar.GetShowTransferInjuryCommand() || IsMatch(currTeammateCommands[i])))
				{
					DomainManager.Combat.ExecuteTeammateCommand(context, combatChar.IsAlly, i, teammateCharacter.GetId());
				}
			}
		}
	}

	private bool IsMatch(sbyte cmdType)
	{
		return TeammateCommand.Instance[cmdType].Implement == _implement;
	}
}
