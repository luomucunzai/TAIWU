using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;

namespace GameData.Domains.Adventure.AdventureExtraRules;

[Obsolete]
public class AdvRule_DisableAllocatedNeiliByType : IAdventureExtraRule
{
	private readonly byte _neiliAllocationType;

	private readonly GameData.Domains.Character.Character[] _targetCharacters;

	byte IAdventureExtraRule.RuleId => (byte)(2 + _neiliAllocationType);

	public AdvRule_DisableAllocatedNeiliByType(byte neiliAllocationType)
	{
		_neiliAllocationType = neiliAllocationType;
		HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
		_targetCharacters = new GameData.Domains.Character.Character[collection.Count];
		int num = 0;
		foreach (int item in collection)
		{
			_targetCharacters[num] = DomainManager.Character.GetElement_Objects(item);
		}
	}

	public void ApplyChangesAtStart(DataContext context)
	{
	}

	public void ApplyChangesOnMove(DataContext context)
	{
	}

	public void ApplyChangesManually(DataContext context)
	{
		GameData.Domains.Character.Character[] targetCharacters = _targetCharacters;
		foreach (GameData.Domains.Character.Character character in targetCharacters)
		{
		}
	}

	public void RevertChanges(DataContext context)
	{
	}

	public void ApplyChangesToEnemyTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter)
	{
	}

	public void ApplyChangesToSelfTeamOnCombatStart(DataContext context, CombatCharacter combatCharacter)
	{
	}
}
