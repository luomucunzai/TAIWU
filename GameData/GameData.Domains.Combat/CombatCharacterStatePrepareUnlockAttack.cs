using GameData.Common;

namespace GameData.Domains.Combat;

public class CombatCharacterStatePrepareUnlockAttack : CombatCharacterStateBase
{
	public CombatCharacterStatePrepareUnlockAttack(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.PrepareUnlockAttack)
	{
	}

	public override void OnEnter()
	{
		DataContext context = CurrentCombatDomain.Context;
		int needUnlockWeaponIndex = CombatChar.GetCombatReserveData().NeedUnlockWeaponIndex;
		CombatChar.SetNeedUnlockWeaponIndex(context, -1);
		if (CombatChar.GetCanUnlockAttack()[needUnlockWeaponIndex])
		{
			DomainManager.Combat.UnlockAttack(context, CombatChar, needUnlockWeaponIndex);
		}
		CombatChar.StateMachine.TranslateState();
	}
}
