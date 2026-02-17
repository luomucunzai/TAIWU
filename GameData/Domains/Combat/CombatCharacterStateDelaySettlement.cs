using System;
using GameData.Common;

namespace GameData.Domains.Combat
{
	// Token: 0x0200069A RID: 1690
	public class CombatCharacterStateDelaySettlement : CombatCharacterStateBase
	{
		// Token: 0x060061FA RID: 25082 RVA: 0x0037BAB1 File Offset: 0x00379CB1
		public CombatCharacterStateDelaySettlement(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.DelaySettlement)
		{
			this.IsUpdateOnPause = true;
		}

		// Token: 0x060061FB RID: 25083 RVA: 0x0037BAC6 File Offset: 0x00379CC6
		public override void OnEnter()
		{
			this._leftFrame = (short)(Math.Ceiling((double)DomainManager.Combat.GetTimeScale()) + 1.0);
		}

		// Token: 0x060061FC RID: 25084 RVA: 0x0037BAEC File Offset: 0x00379CEC
		public override void OnExit()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this.CombatChar.NeedDelaySettlement = false;
			this.CurrentCombatDomain.SetShowMercyOption(context, EShowMercyOption.Invalid);
		}

		// Token: 0x060061FD RID: 25085 RVA: 0x0037BB20 File Offset: 0x00379D20
		public override bool OnUpdate()
		{
			bool flag = !base.OnUpdate();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._leftFrame > 0;
				if (flag2)
				{
					this._leftFrame -= 1;
					bool flag3 = this._leftFrame == 0;
					if (flag3)
					{
						this.CurrentCombatDomain.CombatSettlement(this.CombatChar.GetDataContext(), this.CombatChar.IsAlly ? CombatStatusType.EnemyFail : CombatStatusType.SelfFail);
						this.CombatChar.StateMachine.TranslateState();
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x04001AAB RID: 6827
		private short _leftFrame;
	}
}
