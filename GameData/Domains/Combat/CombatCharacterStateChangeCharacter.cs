using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x02000699 RID: 1689
	public class CombatCharacterStateChangeCharacter : CombatCharacterStateBase
	{
		// Token: 0x060061F7 RID: 25079 RVA: 0x0037B7EB File Offset: 0x003799EB
		public CombatCharacterStateChangeCharacter(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.ChangeCharacter)
		{
			this.IsUpdateOnPause = true;
		}

		// Token: 0x060061F8 RID: 25080 RVA: 0x0037B800 File Offset: 0x00379A00
		public override void OnEnter()
		{
			this._leftWaitFrame = (short)Math.Round(90.0);
			DataContext context = this.CombatChar.GetDataContext();
			this.CombatChar.SetAnimationToPlayOnce("M_003", context);
			this.CombatChar.SetAnimationToLoop(this.CurrentCombatDomain.GetIdleAni(this.CombatChar), context);
			bool flag = !this.CurrentCombatDomain.IsMainCharacter(this.CombatChar);
			if (flag)
			{
				this.CombatChar.SetBreathValue(15000, context);
				this.CombatChar.SetStanceValue(2000, context);
				this.CombatChar.SetMobilityValue(MoveSpecialConstants.MaxMobility * 50 / 100, context);
				this.CombatChar.ClearAllDoingOrReserveCommand(context);
				this.CombatChar.ClearAllSound(context);
				this.CombatChar.SetExecutingTeammateCommand(this.CombatChar.ExecutingTeammateCommandConfig.TemplateId, context);
				int teammateIndex = this.CurrentCombatDomain.GetCharacterList(this.CombatChar.IsAlly).IndexOf(this.CombatChar.GetId()) - 1;
				this.CurrentCombatDomain.GetMainCharacter(this.CombatChar.IsAlly).TeammateHasCommand[teammateIndex] = true;
			}
		}

		// Token: 0x060061F9 RID: 25081 RVA: 0x0037B938 File Offset: 0x00379B38
		public override bool OnUpdate()
		{
			this._leftWaitFrame -= 1;
			bool flag = this._leftWaitFrame == 0;
			if (flag)
			{
				DataContext context = this.CombatChar.GetDataContext();
				bool flag2 = !this.CurrentCombatDomain.IsMainCharacter(this.CombatChar);
				if (flag2)
				{
					this.CombatChar.ResetTeammateCommandLeftTime(context);
				}
				else
				{
					bool selfFallen = this.CurrentCombatDomain.IsCharacterFallen(this.CombatChar);
					CombatCharacter enemyChar = this.CurrentCombatDomain.GetMainCharacter(!this.CombatChar.IsAlly);
					bool endCombat = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, false).ChangeCharId < 0 && (selfFallen || this.CurrentCombatDomain.IsCharacterFallen(enemyChar));
					int[] charList = this.CurrentCombatDomain.GetCharacterList(this.CombatChar.IsAlly);
					for (int i = 1; i < charList.Length; i++)
					{
						bool flag3 = charList[i] < 0;
						if (!flag3)
						{
							this.CurrentCombatDomain.GetElement_CombatCharacterDict(charList[i]).SetVisible(false, context);
						}
					}
					bool flag4 = endCombat;
					if (flag4)
					{
						this.CurrentCombatDomain.EndCombat(context, selfFallen ? this.CombatChar : enemyChar, false, true);
					}
				}
				this.CombatChar.StateMachine.TranslateState();
				Events.RaiseCombatCharChanged(context, this.CombatChar.IsAlly);
			}
			return false;
		}

		// Token: 0x04001AAA RID: 6826
		private short _leftWaitFrame;
	}
}
