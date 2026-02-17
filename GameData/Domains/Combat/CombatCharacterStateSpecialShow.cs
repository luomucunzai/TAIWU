using System;
using Config;
using GameData.Common;

namespace GameData.Domains.Combat
{
	// Token: 0x020006A6 RID: 1702
	public class CombatCharacterStateSpecialShow : CombatCharacterStateBase
	{
		// Token: 0x0600623F RID: 25151 RVA: 0x0037ECD6 File Offset: 0x0037CED6
		public CombatCharacterStateSpecialShow(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.SpecialShow)
		{
			this.IsUpdateOnPause = true;
		}

		// Token: 0x06006240 RID: 25152 RVA: 0x0037ECEC File Offset: 0x0037CEEC
		public override void OnEnter()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this.CombatChar.NeedEnterSpecialShow = false;
			this._specialShowChar = this.CurrentCombatDomain.GetElement_CombatCharacterDict(this.CurrentCombatDomain.GetSpecialShowCombatCharId());
			this._enterFrame = 34;
			this._castAniFrame = 0;
			this._hitFrame = 0;
			this._leaveFrame = 0;
			int displayPos = this.CurrentCombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, CombatItemUse.DefValue.UseThrowPoison.Distance);
			this._specialShowChar.SetVisible(true, context);
			this._specialShowChar.SetDisplayPosition(displayPos, context);
			this._specialShowChar.SetAnimationToLoop(this.CurrentCombatDomain.GetIdleAni(this._specialShowChar), context);
		}

		// Token: 0x06006241 RID: 25153 RVA: 0x0037EDA6 File Offset: 0x0037CFA6
		public override void OnExit()
		{
		}

		// Token: 0x06006242 RID: 25154 RVA: 0x0037EDAC File Offset: 0x0037CFAC
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
				bool flag2 = this._enterFrame > 0;
				if (flag2)
				{
					this._enterFrame -= 1;
					bool flag3 = this._enterFrame == 0;
					if (flag3)
					{
						DataContext context = this.CombatChar.GetDataContext();
						CombatItemUseItem throwConfig = CombatItemUse.Instance[10];
						this._specialShowChar.SetAnimationToPlayOnce(throwConfig.Animation, context);
						this._specialShowChar.SetParticleToPlay(throwConfig.Particle, context);
						this._specialShowChar.SetAttackSoundToPlay(throwConfig.Sound, context);
						this._castAniFrame = (short)Math.Round((double)(AnimDataCollection.Data[throwConfig.Animation].Duration * 60f));
						this._hitFrame = (short)Math.Round((double)(AnimDataCollection.Data[throwConfig.Animation].Events["act0"][0] * 60f));
					}
					result = false;
				}
				else
				{
					bool flag4 = this._hitFrame > 0;
					if (flag4)
					{
						this._hitFrame -= 1;
						bool flag5 = this._hitFrame == 0;
						if (flag5)
						{
							DataContext context2 = this.CombatChar.GetDataContext();
							CombatCharacter enemyChar = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, false);
							short stateId = 142 + this.CurrentCombatDomain.CombatConfig.TemplateId - 164;
							enemyChar.SetAnimationToPlayOnce(this.CurrentCombatDomain.GetHittedAni(enemyChar, 2), context2);
							this.CurrentCombatDomain.AddCombatState(context2, enemyChar, 0, stateId);
						}
					}
					bool flag6 = this._castAniFrame > 0;
					if (flag6)
					{
						this._castAniFrame -= 1;
						bool flag7 = this._castAniFrame == 0;
						if (flag7)
						{
							this._specialShowChar.SetDisplayPosition(int.MinValue, this.CombatChar.GetDataContext());
							this._leaveFrame = 48;
						}
						result = false;
					}
					else
					{
						bool flag8 = this._leaveFrame > 0;
						if (flag8)
						{
							this._leaveFrame -= 1;
							bool flag9 = this._leaveFrame == 0;
							if (flag9)
							{
								this._specialShowChar.SetVisible(false, this.CombatChar.GetDataContext());
								this.CombatChar.StateMachine.TranslateState();
							}
							result = false;
						}
						else
						{
							result = false;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001ABA RID: 6842
		private CombatCharacter _specialShowChar;

		// Token: 0x04001ABB RID: 6843
		private short _enterFrame;

		// Token: 0x04001ABC RID: 6844
		private short _castAniFrame;

		// Token: 0x04001ABD RID: 6845
		private short _hitFrame;

		// Token: 0x04001ABE RID: 6846
		private short _leaveFrame;
	}
}
