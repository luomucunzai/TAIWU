using System;
using GameData.Common;

namespace GameData.Domains.Combat
{
	// Token: 0x020006A4 RID: 1700
	public class CombatCharacterStateSelectChangeTrick : CombatCharacterStateBase
	{
		// Token: 0x0600622D RID: 25133 RVA: 0x0037E322 File Offset: 0x0037C522
		public CombatCharacterStateSelectChangeTrick(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.SelectChangeTrick)
		{
			this.IsUpdateOnPause = true;
		}

		// Token: 0x0600622E RID: 25134 RVA: 0x0037E336 File Offset: 0x0037C536
		public override void OnEnter()
		{
			this.CombatChar.SetChangingTrick(true, this.CombatChar.GetDataContext());
		}

		// Token: 0x0600622F RID: 25135 RVA: 0x0037E354 File Offset: 0x0037C554
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
				bool flag2 = !this.CombatChar.NeedShowChangeTrick || !this.CombatChar.GetCanChangeTrick();
				if (flag2)
				{
					DataContext context = this.CombatChar.GetDataContext();
					bool needShowChangeTrick = this.CombatChar.NeedShowChangeTrick;
					if (needShowChangeTrick)
					{
						this.CombatChar.SetNeedShowChangeTrick(context, false);
					}
					this.CombatChar.SetChangingTrick(false, context);
					this.CombatChar.StateMachine.TranslateState();
				}
				result = false;
			}
			return result;
		}
	}
}
