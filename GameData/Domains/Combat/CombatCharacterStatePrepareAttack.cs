using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Item;

namespace GameData.Domains.Combat
{
	// Token: 0x0200069E RID: 1694
	public class CombatCharacterStatePrepareAttack : CombatCharacterStateBase
	{
		// Token: 0x0600620E RID: 25102 RVA: 0x0037C4B9 File Offset: 0x0037A6B9
		public CombatCharacterStatePrepareAttack(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.PrepareAttack)
		{
		}

		// Token: 0x0600620F RID: 25103 RVA: 0x0037C4C8 File Offset: 0x0037A6C8
		public override void OnEnter()
		{
			DataContext context = this.CombatChar.GetDataContext();
			CombatCharacterStatePrepareAttack.EType type = this.OnEnterCheckAttackType(context);
			bool flag = type == CombatCharacterStatePrepareAttack.EType.Prefer;
			if (flag)
			{
				this.CombatChar.StateMachine.TranslateState();
			}
			else
			{
				bool flag2 = type == CombatCharacterStatePrepareAttack.EType.NoPrepare;
				if (flag2)
				{
					this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Attack);
				}
				else
				{
					this._leftPrepareFrame = this.CombatChar.CalcNormalAttackStartupFrames();
					Weapon weapon = DomainManager.Item.GetElement_Weapons(this.CurrentCombatDomain.GetUsingWeaponKey(this.CombatChar).Id);
					sbyte aniIndex = weapon.GetWeaponAction();
					sbyte trickType = this.CombatChar.GetAttackingTrickType();
					ValueTuple<string, string> prepareAni = this.CurrentCombatDomain.GetPrepareAttackAni(this.CombatChar, trickType, (int)aniIndex);
					float aniTime = AnimDataCollection.Data[prepareAni.Item2].Duration;
					float prepareTime = (float)this._leftPrepareFrame / 60f;
					this.CombatChar.SetAnimationTimeScale(aniTime / prepareTime, context);
					this.CombatChar.SetAnimationToPlayOnce(prepareAni.Item1, context);
					this.CombatChar.SetAnimationToLoop(null, context);
				}
			}
		}

		// Token: 0x06006210 RID: 25104 RVA: 0x0037C5E4 File Offset: 0x0037A7E4
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
				this._leftPrepareFrame--;
				bool flag2 = this._leftPrepareFrame <= 0;
				if (flag2)
				{
					DataContext context = this.CombatChar.GetDataContext();
					Events.RaiseNormalAttackPrepareEnd(context, this.CombatChar.GetId(), this.CombatChar.IsAlly);
					this.CombatChar.SetAnimationTimeScale(1f, context);
					this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Attack);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06006211 RID: 25105 RVA: 0x0037C678 File Offset: 0x0037A878
		private CombatCharacterStatePrepareAttack.EType OnEnterCheckAttackType(DataContext context)
		{
			bool noPrepare = false;
			bool flag = this.CombatChar.NeedNormalAttackSkipPrepare > 0;
			if (flag)
			{
				this.CombatChar.NeedNormalAttackSkipPrepare--;
				this.CombatChar.IsAutoNormalAttacking = true;
				noPrepare = true;
			}
			else
			{
				bool needChangeTrickAttack = this.CombatChar.NeedChangeTrickAttack;
				if (needChangeTrickAttack)
				{
					this.CombatChar.NeedChangeTrickAttack = false;
					this.CombatChar.SetChangeTrickAttack(true, context);
				}
				else
				{
					bool needFreeAttack = this.CombatChar.NeedFreeAttack;
					if (needFreeAttack)
					{
						this.CombatChar.NeedFreeAttack = false;
						this.CombatChar.IsAutoNormalAttacking = true;
					}
					else
					{
						bool flag2 = this.TryChangeToUnlockAttack(context);
						if (flag2)
						{
							return CombatCharacterStatePrepareAttack.EType.Prefer;
						}
						bool needNormalAttackImmediate = this.CombatChar.NeedNormalAttackImmediate;
						if (needNormalAttackImmediate)
						{
							this.CombatChar.NeedNormalAttackImmediate = false;
						}
						else
						{
							this.CombatChar.SetReserveNormalAttack(false, context);
						}
					}
				}
			}
			bool flag3 = !noPrepare && this.CombatChar.NextAttackNoPrepare;
			if (flag3)
			{
				this.CombatChar.NextAttackNoPrepare = false;
				noPrepare = true;
			}
			sbyte trickType = this.CombatChar.GetChangeTrickAttack() ? this.CombatChar.ChangeTrickType : this.CombatChar.GetWeaponTricks()[(int)this.CombatChar.GetWeaponTrickIndex()];
			trickType = (sbyte)DomainManager.SpecialEffect.ModifyData(this.CombatChar.GetId(), -1, 83, (int)trickType, -1, -1, -1);
			this.CombatChar.SetAttackingTrickType(trickType, context);
			return noPrepare ? CombatCharacterStatePrepareAttack.EType.NoPrepare : CombatCharacterStatePrepareAttack.EType.Normal;
		}

		// Token: 0x06006212 RID: 25106 RVA: 0x0037C7F0 File Offset: 0x0037A9F0
		private bool TryChangeToUnlockAttack(DataContext context)
		{
			int usingIndex = this.CombatChar.GetUsingWeaponIndex();
			bool flag = !this.CombatChar.CanUnlockAttackByConfig(usingIndex);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool changeToUnlockAttack = DomainManager.SpecialEffect.ModifyData(this.CombatChar.GetId(), -1, 307, false, -1, -1, -1);
				bool flag2 = !changeToUnlockAttack;
				if (flag2)
				{
					result = false;
				}
				else
				{
					this.CombatChar.NeedNormalAttackImmediate = false;
					this.CombatChar.SetReserveNormalAttack(false, context);
					this.CombatChar.NeedUnlockAttack = true;
					this.CombatChar.UnlockWeaponIndex = usingIndex;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04001AB3 RID: 6835
		private int _leftPrepareFrame;

		// Token: 0x02000B4C RID: 2892
		public enum EType
		{
			// Token: 0x04002FC7 RID: 12231
			Normal,
			// Token: 0x04002FC8 RID: 12232
			NoPrepare,
			// Token: 0x04002FC9 RID: 12233
			Prefer
		}
	}
}
