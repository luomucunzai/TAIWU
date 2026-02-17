using System;
using GameData.Common;

namespace GameData.Domains.Combat
{
	// Token: 0x020006A1 RID: 1697
	public class CombatCharacterStatePrepareUnlockAttack : CombatCharacterStateBase
	{
		// Token: 0x0600621E RID: 25118 RVA: 0x0037D617 File Offset: 0x0037B817
		public CombatCharacterStatePrepareUnlockAttack(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.PrepareUnlockAttack)
		{
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x0037D624 File Offset: 0x0037B824
		public override void OnEnter()
		{
			DataContext context = this.CurrentCombatDomain.Context;
			int needUnlockAttackWeaponIndex = this.CombatChar.GetCombatReserveData().NeedUnlockWeaponIndex;
			this.CombatChar.SetNeedUnlockWeaponIndex(context, -1);
			bool flag = this.CombatChar.GetCanUnlockAttack()[needUnlockAttackWeaponIndex];
			if (flag)
			{
				DomainManager.Combat.UnlockAttack(context, this.CombatChar, needUnlockAttackWeaponIndex);
			}
			this.CombatChar.StateMachine.TranslateState();
		}
	}
}
