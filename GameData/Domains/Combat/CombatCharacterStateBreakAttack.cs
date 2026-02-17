using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Item;

namespace GameData.Domains.Combat
{
	// Token: 0x02000696 RID: 1686
	public class CombatCharacterStateBreakAttack : CombatCharacterStateBase
	{
		// Token: 0x060061EB RID: 25067 RVA: 0x0037A65C File Offset: 0x0037885C
		public CombatCharacterStateBreakAttack(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.BreakAttack)
		{
			this.IsUpdateOnPause = true;
			this.RequireDelayFallen = true;
		}

		// Token: 0x060061EC RID: 25068 RVA: 0x0037A678 File Offset: 0x00378878
		public override void OnEnter()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this.CombatChar.NeedBreakAttack = false;
			this.CombatChar.IsBreakAttacking = true;
			this.CombatChar.IsAutoNormalAttacking = true;
			Weapon weapon = DomainManager.Item.GetElement_Weapons(this.CurrentCombatDomain.GetUsingWeaponKey(this.CombatChar).Id);
			sbyte aniIndex = weapon.GetWeaponAction();
			sbyte trickType = this.CombatChar.GetWeaponTricks()[(int)this.CombatChar.GetWeaponTrickIndex()];
			trickType = (sbyte)DomainManager.SpecialEffect.ModifyData(this.CombatChar.GetId(), -1, 83, (int)trickType, -1, -1, -1);
			this.CombatChar.SetAttackingTrickType(trickType, context);
			ValueTuple<string, string> prepareAttackAni = this.CurrentCombatDomain.GetPrepareAttackAni(this.CombatChar, trickType, (int)aniIndex);
			string aniName = prepareAttackAni.Item1;
			string fullAniName = prepareAttackAni.Item2;
			bool flag = string.IsNullOrEmpty(aniName);
			if (flag)
			{
				this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Attack);
			}
			else
			{
				this._leftPrepareFrame = AnimDataCollection.GetDurationFrame(fullAniName);
				this._leftParticleFrame = AnimDataCollection.GetEventFrame(fullAniName, "break_p0", 0);
				this._leftAudioFrame = AnimDataCollection.GetEventFrame(fullAniName, "break_a0", 0);
				this.CombatChar.SetAnimationToPlayOnce(aniName, context);
				this.CombatChar.SetAnimationToLoop(null, context);
				DomainManager.Combat.ShowSpecialEffectTips(this.CombatChar.GetId(), 1662, 0);
			}
		}

		// Token: 0x060061ED RID: 25069 RVA: 0x0037A7D4 File Offset: 0x003789D4
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
				DataContext context = this.CombatChar.GetDataContext();
				bool flag2 = this._leftParticleFrame == 0;
				if (flag2)
				{
					this.CombatChar.SetParticleToPlay("Particle_A_B0", context);
				}
				this._leftParticleFrame--;
				bool flag3 = this._leftAudioFrame == 0;
				if (flag3)
				{
					this.CombatChar.SetAttackSoundToPlay("se_a_b0", context);
				}
				this._leftAudioFrame--;
				this._leftPrepareFrame--;
				bool flag4 = this._leftPrepareFrame > 0;
				if (flag4)
				{
					result = false;
				}
				else
				{
					Events.RaiseNormalAttackPrepareEnd(context, this.CombatChar.GetId(), this.CombatChar.IsAlly);
					this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Attack);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x04001A98 RID: 6808
		private const string CommonParticle = "Particle_A_B0";

		// Token: 0x04001A99 RID: 6809
		private const string CommonAudio = "se_a_b0";

		// Token: 0x04001A9A RID: 6810
		private int _leftPrepareFrame;

		// Token: 0x04001A9B RID: 6811
		private int _leftParticleFrame;

		// Token: 0x04001A9C RID: 6812
		private int _leftAudioFrame;
	}
}
