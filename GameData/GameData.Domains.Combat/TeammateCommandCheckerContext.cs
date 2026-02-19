using System.Collections.Generic;
using Config;

namespace GameData.Domains.Combat;

public struct TeammateCommandCheckerContext
{
	public CombatCharacter CurrChar;

	public CombatCharacter TeammateChar;

	private bool _extraHasTeammateBefore;

	private bool _extraHasTeammateAfter;

	public IReadOnlyList<byte> CdPercent => TeammateChar.GetTeammateCommandCdPercent();

	public bool HasTeammateBefore => CurrChar.TeammateBeforeMainChar >= 0 || _extraHasTeammateBefore;

	public bool HasTeammateAfter => CurrChar.TeammateAfterMainChar >= 0 || _extraHasTeammateAfter;

	public void InitExtraFields()
	{
		int[] characterList = DomainManager.Combat.GetCharacterList(TeammateChar.IsAlly);
		for (int i = 0; i < CurrChar.TeammateHasCommand.Length; i++)
		{
			if (!CurrChar.TeammateHasCommand[i])
			{
				continue;
			}
			TeammateCommandItem executingTeammateCommandConfig = DomainManager.Combat.GetElement_CombatCharacterDict(characterList[i + 1]).ExecutingTeammateCommandConfig;
			if (executingTeammateCommandConfig.IntoCombatField)
			{
				if (executingTeammateCommandConfig.PosOffset > 0)
				{
					_extraHasTeammateBefore = true;
				}
				else
				{
					_extraHasTeammateAfter = true;
				}
			}
		}
	}
}
