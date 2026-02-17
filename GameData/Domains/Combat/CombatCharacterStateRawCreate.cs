using System;
using GameData.Common;

namespace GameData.Domains.Combat
{
	// Token: 0x020006A3 RID: 1699
	public class CombatCharacterStateRawCreate : CombatCharacterStateBase
	{
		// Token: 0x0600622B RID: 25131 RVA: 0x0037E28A File Offset: 0x0037C48A
		public CombatCharacterStateRawCreate(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.RawCreate)
		{
			this.IsUpdateOnPause = true;
		}

		// Token: 0x0600622C RID: 25132 RVA: 0x0037E2A0 File Offset: 0x0037C4A0
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
				bool flag2 = this.CombatChar.IsAlly && this.CombatChar.AnyRawCreate;
				if (flag2)
				{
					result = false;
				}
				else
				{
					DataContext context = this.CurrentCombatDomain.Context;
					bool anyRawCreate = this.CombatChar.AnyRawCreate;
					if (anyRawCreate)
					{
						this.CombatChar.AutoAllRawCreate(context);
					}
					this.CombatChar.StateMachine.TranslateState();
					result = false;
				}
			}
			return result;
		}
	}
}
