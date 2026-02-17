using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.Combat
{
	// Token: 0x020006A9 RID: 1705
	public class CombatCharacterStateUnlockAttack : CombatCharacterStateBase
	{
		// Token: 0x06006272 RID: 25202 RVA: 0x00380A0D File Offset: 0x0037EC0D
		public CombatCharacterStateUnlockAttack(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.UnlockAttack)
		{
			this.IsUpdateOnPause = true;
			this.RequireDelayFallen = true;
		}

		// Token: 0x06006273 RID: 25203 RVA: 0x00380A28 File Offset: 0x0037EC28
		public override void OnEnter()
		{
			base.OnEnter();
			this._initWeaponIndex = this.CombatChar.GetUsingWeaponIndex();
			this.InvokeAnimations();
		}

		// Token: 0x06006274 RID: 25204 RVA: 0x00380A4C File Offset: 0x0037EC4C
		public override void OnExit()
		{
			DataContext context = this.CurrentCombatDomain.Context;
			this.CombatChar.UnlockWeaponIndex = -1;
			this.CombatChar.SetUsingWeaponIndex(this._initWeaponIndex, context);
			this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, int.MinValue);
		}

		// Token: 0x06006275 RID: 25205 RVA: 0x00380AA4 File Offset: 0x0037ECA4
		private void InvokeAnimations()
		{
			this.CombatChar.NeedUnlockAttack = false;
			bool flag = this.CombatChar.UnlockEffectId < 0;
			if (flag)
			{
				short predefinedLogId = 8;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
				defaultInterpolatedStringHandler.AppendFormatted<CombatCharacter>(this.CombatChar);
				defaultInterpolatedStringHandler.AppendLiteral(" invoke unlock attack on ");
				defaultInterpolatedStringHandler.AppendFormatted(this.CombatChar.UnlockWeapon.GetName());
				PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
				this.CombatChar.StateMachine.TranslateState();
			}
			else
			{
				DataContext context = this.CurrentCombatDomain.Context;
				this.CombatChar.SetUsingWeaponIndex(this.CombatChar.UnlockWeaponIndex, context);
				this.CombatChar.SetAttackSoundToPlay(this.CombatChar.UnlockEffect.Sound, context);
				this.CombatChar.SetAnimationToPlayOnce("J_ready", context);
				this.CombatChar.SetParticleToPlay("Particle_J_ready", context);
				this.CurrentCombatDomain.ShowSpecialEffectTips(this.CombatChar.GetId(), (int)this.CombatChar.UnlockEffect.EffectId, 0);
				base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.OnReady), AnimDataCollection.GetDurationFrame("J_ready"));
			}
		}

		// Token: 0x06006276 RID: 25206 RVA: 0x00380BDC File Offset: 0x0037EDDC
		private void OnReady()
		{
			this.CalcUnlockEffect();
			DataContext context = this.CurrentCombatDomain.Context;
			WeaponUnlockEffectItem effect = this.CombatChar.UnlockEffect;
			Events.RaiseNormalAttackPrepareEnd(context, this.CombatChar.GetId(), this.CombatChar.IsAlly);
			this.CombatChar.SetAnimationToPlayOnce(effect.Animation, context);
			this.CombatChar.SetParticleToPlay(effect.Particle, context);
			this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, this.CurrentCombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, (short)effect.DisplayPosition[0]));
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.OnAct1), AnimDataCollection.GetEventFrame(effect.Animation, "act1", 0));
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.OnAct2), AnimDataCollection.GetEventFrame(effect.Animation, "act2", 0));
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.OnAct3), AnimDataCollection.GetEventFrame(effect.Animation, "act3", 0));
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.FinishAttack), AnimDataCollection.GetDurationFrame(effect.Animation));
		}

		// Token: 0x06006277 RID: 25207 RVA: 0x00380D0C File Offset: 0x0037EF0C
		private void FinishAttack()
		{
			DataContext context = this.CurrentCombatDomain.Context;
			Events.RaiseUnlockAttackEnd(context, this.CombatChar);
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this.CombatChar.IsAlly, true);
			Events.RaiseNormalAttackAllEnd(context, this.CombatChar, enemyChar);
			CombatCharacterStateType properState = this.CombatChar.StateMachine.GetProperState();
			bool flag = properState == CombatCharacterStateType.UnlockAttack;
			if (flag)
			{
				this.InvokeAnimations();
			}
			else
			{
				this.CombatChar.StateMachine.TranslateState(properState);
			}
		}

		// Token: 0x06006278 RID: 25208 RVA: 0x00380D90 File Offset: 0x0037EF90
		private void CalcUnlockEffect()
		{
			DataContext context = this.CurrentCombatDomain.Context;
			CombatCharacter enemyChar = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, false);
			bool clearAgile = this.CombatChar.UnlockEffect.ClearAgile;
			if (clearAgile)
			{
				this.CurrentCombatDomain.ClearAffectingAgileSkillByEffect(context, enemyChar, this.CombatChar);
			}
			bool clearDefense = this.CombatChar.UnlockEffect.ClearDefense;
			if (clearDefense)
			{
				this.CurrentCombatDomain.ClearAffectingDefenseSkill(context, enemyChar);
			}
		}

		// Token: 0x06006279 RID: 25209 RVA: 0x00380E0F File Offset: 0x0037F00F
		private void OnAct1()
		{
			this.CalcAttackEffect(1);
		}

		// Token: 0x0600627A RID: 25210 RVA: 0x00380E19 File Offset: 0x0037F019
		private void OnAct2()
		{
			this.CalcAttackEffect(2);
		}

		// Token: 0x0600627B RID: 25211 RVA: 0x00380E23 File Offset: 0x0037F023
		private void OnAct3()
		{
			this.CalcAttackEffect(3);
		}

		// Token: 0x0600627C RID: 25212 RVA: 0x00380E30 File Offset: 0x0037F030
		private void CalcAttackEffect(int index)
		{
			this.CurrentCombatDomain.CalcUnlockAttack(this.CombatChar, index - 1);
			DataContext context = this.CurrentCombatDomain.Context;
			this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, this.CurrentCombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, (short)this.CombatChar.UnlockEffect.DisplayPosition[index]));
		}

		// Token: 0x04001AE4 RID: 6884
		private const string ReadyAnimationName = "J_ready";

		// Token: 0x04001AE5 RID: 6885
		private const string ReadyParticleName = "Particle_J_ready";

		// Token: 0x04001AE6 RID: 6886
		private int _initWeaponIndex;
	}
}
