using System.Collections.Generic;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action;

[AiAction(EAiActionType.ChangeWeaponAuto)]
public class AiActionChangeWeaponAuto : AiActionCombatBase
{
	private readonly int _minIndex;

	private readonly int _maxIndex;

	public AiActionChangeWeaponAuto(IReadOnlyList<int> ints)
	{
		_minIndex = ints[0];
		_maxIndex = ints[1];
	}

	public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
	{
		DataContext context = DomainManager.Combat.Context;
		int num = combatChar.AiGetFirstChangeableWeaponIndex(_minIndex, _maxIndex);
		if (num >= 0)
		{
			DomainManager.Combat.ChangeWeapon(context, num, combatChar.IsAlly);
		}
	}
}
