using GameData.Common;

namespace GameData.Domains.Combat;

public class CombatCharacterStateIdle : CombatCharacterStateBase
{
	public CombatCharacterStateIdle(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.Idle)
	{
	}

	public override void OnEnter()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CurrentCombatDomain.SetProperLoopAniAndParticle(dataContext, CombatChar);
	}

	public override bool OnUpdate()
	{
		base.OnUpdate();
		DataContext dataContext = CombatChar.GetDataContext();
		if (CombatChar.NeedChangeWeaponIndex >= 0 && !CombatChar.PreparingTeammateCommand())
		{
			CurrentCombatDomain.ChangeWeapon(dataContext, CombatChar, CombatChar.NeedChangeWeaponIndex);
		}
		return false;
	}
}
