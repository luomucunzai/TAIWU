using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.OptionChangeWeaponIndex)]
public class AiConditionOptionChangeWeaponIndex : AiConditionCombatBase
{
	private readonly int _weaponIndex;

	public AiConditionOptionChangeWeaponIndex(IReadOnlyList<int> ints)
	{
		_weaponIndex = ints[0];
	}

	public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
	{
		if (combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoAttack)
		{
			return false;
		}
		if (combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoChangeWeapon)
		{
			return false;
		}
		ItemKey[] weapons = combatChar.GetWeapons();
		if (!weapons.CheckIndex(_weaponIndex))
		{
			return false;
		}
		ItemKey itemKey = weapons[_weaponIndex];
		if (!itemKey.IsValid())
		{
			return false;
		}
		CombatWeaponData element_WeaponDataDict = DomainManager.Combat.GetElement_WeaponDataDict(itemKey.Id);
		return element_WeaponDataDict.GetCanChangeTo();
	}
}
