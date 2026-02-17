using System;
using GameData.Common;

namespace GameData.Domains.Combat
{
	// Token: 0x0200069B RID: 1691
	public class CombatCharacterStateIdle : CombatCharacterStateBase
	{
		// Token: 0x060061FE RID: 25086 RVA: 0x0037BBB2 File Offset: 0x00379DB2
		public CombatCharacterStateIdle(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.Idle)
		{
		}

		// Token: 0x060061FF RID: 25087 RVA: 0x0037BBC0 File Offset: 0x00379DC0
		public override void OnEnter()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this.CurrentCombatDomain.SetProperLoopAniAndParticle(context, this.CombatChar, false);
			bool flag = this.CombatChar.NeedChangeWeaponIndex >= 0 && !this.CombatChar.PreparingTeammateCommand();
			if (flag)
			{
				this.CurrentCombatDomain.ChangeWeapon(context, this.CombatChar, this.CombatChar.NeedChangeWeaponIndex, false, false);
			}
		}

		// Token: 0x06006200 RID: 25088 RVA: 0x0037BC34 File Offset: 0x00379E34
		public override bool OnUpdate()
		{
			base.OnUpdate();
			return false;
		}
	}
}
