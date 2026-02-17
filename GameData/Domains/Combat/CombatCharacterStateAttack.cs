using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Item;

namespace GameData.Domains.Combat
{
	// Token: 0x02000694 RID: 1684
	public class CombatCharacterStateAttack : CombatCharacterStateBase
	{
		// Token: 0x060061CE RID: 25038 RVA: 0x00378787 File Offset: 0x00376987
		public CombatCharacterStateAttack(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.Attack)
		{
			this.IsUpdateOnPause = true;
			this.RequireDelayFallen = true;
		}

		// Token: 0x060061CF RID: 25039 RVA: 0x003787A4 File Offset: 0x003769A4
		public override void OnEnter()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this._inAttackRange = this.CurrentCombatDomain.InAttackRange(this.CombatChar);
			this._enemyChar = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, true);
			this._trickType = (this.CombatChar.GetChangeTrickAttack() ? this.CombatChar.ChangeTrickType : this.CombatChar.GetAttackingTrickType());
			this._isFightBack = this.CombatChar.GetIsFightBack();
			bool flag = this.CombatChar.PursueAttackCount == 0;
			if (flag)
			{
				this.CombatChar.NormalAttackHitType = this.CurrentCombatDomain.GetAttackHitType(this.CombatChar, this._trickType);
			}
			this.CombatChar.NormalAttackHitType = (sbyte)DomainManager.SpecialEffect.ModifyData(this.CombatChar.GetId(), -1, 68, (int)this.CombatChar.NormalAttackHitType, -1, -1, -1);
			this.CombatChar.NormalAttackBodyPart = (this.CombatChar.GetChangeTrickAttack() ? ((this.CombatChar.NormalAttackHitType != 3) ? this.CombatChar.ChangeTrickBodyPart : -1) : this.CurrentCombatDomain.GetAttackBodyPart(this.CombatChar, this._enemyChar, context.Random, -1, this._trickType, this.CombatChar.NormalAttackHitType));
			bool flag2 = this.CurrentCombatDomain.IsMainCharacter(this.CombatChar);
			if (flag2)
			{
				this.CurrentCombatDomain.ForceAllTeammateLeaveCombatField(context, this.CombatChar.IsAlly);
			}
			this.CurrentCombatDomain.UpdateDamageCompareData(CombatContext.Create(this.CombatChar, null, -1, -1, -1, null));
			bool flag3 = this.CurrentCombatDomain.IsPlayingMoveAni(this._enemyChar);
			if (flag3)
			{
				this._enemyChar.SetAnimationToLoop(this.CurrentCombatDomain.GetIdleAni(this._enemyChar), context);
			}
			this.InitAttack(context);
		}

		// Token: 0x060061D0 RID: 25040 RVA: 0x0037898C File Offset: 0x00376B8C
		public override void OnExit()
		{
			this.CombatChar.IsBreakAttacking = false;
			this.CombatChar.PursueAttackCount = 0;
			this.CombatChar.SetAnimationToLoop(this.CurrentCombatDomain.GetIdleAni(this.CombatChar), this.CombatChar.GetDataContext());
		}

		// Token: 0x060061D1 RID: 25041 RVA: 0x003789DC File Offset: 0x00376BDC
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
				bool flag2 = this.CurrentCombatDomain.IsCharacterFallen(this.CombatChar);
				if (flag2)
				{
					this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Idle);
					result = false;
				}
				else
				{
					bool flag3 = this._moveAniFrame > 0;
					if (flag3)
					{
						this._moveAniFrame -= 1;
						bool flag4 = this._moveAniFrame == 0;
						if (flag4)
						{
							this.PlayAttackAnimation();
						}
						result = false;
					}
					else
					{
						bool isFightBack = this._enemyChar.GetIsFightBack();
						if (isFightBack)
						{
							DataContext context = this.CombatChar.GetDataContext();
							sbyte fightBackTrickType = this._enemyChar.GetWeaponTricks()[(int)this._enemyChar.GetWeaponTrickIndex()];
							this.CombatChar.NormalAttackLeftRepeatTimes = 0;
							this.CombatChar.SetAttackingTrickType(-1, context);
							this.CombatChar.StateMachine.TranslateState();
							Events.RaiseNormalAttackAllEnd(context, this.CombatChar, this._enemyChar);
							this.CombatChar.NormalAttackRecovery(context);
							this.CombatChar.FinishFreeAttack();
							bool flag5 = this._enemyChar.StateMachine.GetCurrentStateType() == CombatCharacterStateType.PrepareAttack;
							if (flag5)
							{
								CombatCharacter enemyChar = this._enemyChar;
								enemyChar.NormalAttackLeftRepeatTimes += 1;
							}
							else
							{
								this._enemyChar.IsAutoNormalAttacking = true;
							}
							this._enemyChar.SetAttackingTrickType(fightBackTrickType, context);
							this._enemyChar.StateMachine.TranslateState(CombatCharacterStateType.Attack);
							bool flag6 = !this.CombatChar.IsAutoNormalAttacking;
							if (flag6)
							{
								this.CombatChar.SetWeaponTrickIndex((this.CombatChar.GetWeaponTrickIndex() + 1) % 6, context);
							}
							result = false;
						}
						else
						{
							bool flag7 = this._damageFrame > 0;
							if (flag7)
							{
								this._damageFrame -= 1;
								bool flag8 = this._damageFrame == 0;
								if (flag8)
								{
									DataContext context2 = this.CombatChar.GetDataContext();
									bool inAttackRange = this._inAttackRange;
									if (inAttackRange)
									{
										CombatContext combatContext = CombatContext.Create(this.CombatChar, null, -1, -1, -1, null);
										this._isCritical = combatContext.CheckCritical(this.CombatChar.NormalAttackHitType);
										this.CurrentCombatDomain.CalcNormalAttack(combatContext.Critical(this._isCritical), this._trickType);
									}
									else
									{
										this.CombatChar.SetAttackOutOfRange(true, context2);
										Events.RaiseNormalAttackOutOfRange(context2, this.CombatChar.GetId(), this.CombatChar.IsAlly);
									}
								}
							}
							bool flag9 = this._pursueFrame > 0;
							if (flag9)
							{
								this._pursueFrame -= 1;
								bool flag10 = this._pursueFrame == 0 && !this._isFightBack && this.CurrentCombatDomain.CanPursue(this.CombatChar, this._isCritical) && this._weaponId == this.CurrentCombatDomain.GetUsingWeaponKey(this.CombatChar).Id;
								if (flag10)
								{
									CombatCharacter combatChar = this.CombatChar;
									combatChar.PursueAttackCount += 1;
									this.OnEnter();
								}
							}
							bool flag11 = this._attackAniFrame > 0;
							if (flag11)
							{
								this._attackAniFrame -= 1;
								bool flag12 = this._attackAniFrame == this._attackEndWaitFrame;
								if (flag12)
								{
									DataContext context3 = this.CombatChar.GetDataContext();
									bool needRepeatAttack = this.CombatChar.NormalAttackLeftRepeatTimes > 0 && !this.CurrentCombatDomain.IsCharacterFallen(this._enemyChar) && !this.CurrentCombatDomain.IsCharacterFallen(this.CombatChar);
									this.CombatChar.SetAttackingTrickType(-1, context3);
									bool isFightBack2 = this.CombatChar.GetIsFightBack();
									if (isFightBack2)
									{
										this.CurrentCombatDomain.SetDisplayPosition(context3, !this.CombatChar.IsAlly, int.MinValue);
									}
									else
									{
										bool flag13 = !needRepeatAttack;
										if (flag13)
										{
											this.CurrentCombatDomain.SetDisplayPosition(context3, this.CombatChar.IsAlly, int.MinValue);
										}
									}
								}
								bool flag14 = this._attackAniFrame == 0;
								if (flag14)
								{
									DataContext context4 = this.CombatChar.GetDataContext();
									this.CombatChar.SetAnimationTimeScale(1f, context4);
									bool flag15 = !this.CombatChar.IsAutoNormalAttacking;
									if (flag15)
									{
										this.CombatChar.SetWeaponTrickIndex((this.CombatChar.GetWeaponTrickIndex() + 1) % 6, context4);
									}
									this.CurrentCombatDomain.ClearDamageCompareData(context4);
									Events.RaiseNormalAttackAllEnd(context4, this.CombatChar, this._enemyChar);
									this.CombatChar.NormalAttackRecovery(context4);
									this.CombatChar.FinishFreeAttack();
									bool isFightBack3 = this.CombatChar.GetIsFightBack();
									if (isFightBack3)
									{
										this.CombatChar.SetIsFightBack(false, context4);
										this.CombatChar.FightBackWithHit = false;
										this.CombatChar.FightBackHitType = -1;
									}
									bool changeTrickAttack = this.CombatChar.GetChangeTrickAttack();
									if (changeTrickAttack)
									{
										this.CombatChar.SetChangeTrickAttack(false, context4);
									}
									bool flag16 = this.CombatChar.AttackForceHitCount > 0;
									if (flag16)
									{
										CombatCharacter combatChar2 = this.CombatChar;
										combatChar2.AttackForceHitCount -= 1;
									}
									bool flag17 = this.CombatChar.AttackForceMissCount > 0;
									if (flag17)
									{
										CombatCharacter combatChar3 = this.CombatChar;
										combatChar3.AttackForceMissCount -= 1;
									}
									bool flag18 = this.CombatChar.NormalAttackLeftRepeatTimes > 0 && !this.CurrentCombatDomain.IsCharacterFallen(this._enemyChar) && !this.CurrentCombatDomain.IsCharacterFallen(this.CombatChar);
									if (flag18)
									{
										sbyte trickType = this.CombatChar.GetWeaponTricks()[(int)this.CombatChar.GetWeaponTrickIndex()];
										trickType = (sbyte)DomainManager.SpecialEffect.ModifyData(this.CombatChar.GetId(), -1, 83, (int)trickType, -1, -1, -1);
										this.CombatChar.SetAttackingTrickType(trickType, context4);
										bool normalAttackRepeatIsFightBack = this.CombatChar.NormalAttackRepeatIsFightBack;
										if (normalAttackRepeatIsFightBack)
										{
											this.CombatChar.SetIsFightBack(true, context4);
										}
										CombatCharacter combatChar4 = this.CombatChar;
										combatChar4.NormalAttackLeftRepeatTimes -= 1;
										this.CombatChar.IsAutoNormalAttacking = true;
										this.CombatChar.PursueAttackCount = 0;
										this.OnEnter();
									}
									else
									{
										this.CombatChar.NormalAttackRepeatIsFightBack = false;
										this.CombatChar.StateMachine.TranslateState();
										bool flag19 = this._enemyChar.GetAnimationToLoop() == this.CurrentCombatDomain.GetIdleAni(this.CombatChar);
										if (flag19)
										{
											this.CurrentCombatDomain.SetProperLoopAniAndParticle(context4, this._enemyChar, false);
										}
									}
								}
							}
							result = false;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060061D2 RID: 25042 RVA: 0x00379074 File Offset: 0x00377274
		private void InitAttack(DataContext context)
		{
			this._weaponId = this.CurrentCombatDomain.GetUsingWeaponKey(this.CombatChar).Id;
			GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(this._weaponId);
			ValueTuple<string, string, string, string> attackEffect = this.CurrentCombatDomain.GetAttackEffect(this.CombatChar, weapon, this._trickType);
			string aniName = attackEffect.Item1;
			string fullAniName = attackEffect.Item2;
			string particle = attackEffect.Item3;
			string sound = attackEffect.Item4;
			this._aniIndex = (int)weapon.GetWeaponAction();
			this._attackAniName = aniName;
			this._attackParticleName = particle;
			this._attackSound = sound;
			float animTime = AnimDataCollection.Data[fullAniName].Duration;
			this._attackAniFrame = this.CombatChar.CalcNormalAttackAnimationFrames(animTime);
			float frameTime = (float)this._attackAniFrame / 60f;
			this.CombatChar.SetAnimationTimeScale(animTime / frameTime, context);
			this._damageFrame = (short)Math.Max(Math.Round((double)(AnimDataCollection.Data[fullAniName].Events["act0"][0] * 60f)), 1.0);
			this._pursueFrame = ((this._inAttackRange && !this.CombatChar.GetIsFightBack() && this.CombatChar.PursueAttackCount < 5 && this.CombatChar.AnimalConfig == null) ? ((short)Math.Round((double)(AnimDataCollection.Data[fullAniName].Events["hit"][0] * 60f))) : -1);
			this.StartAttack();
		}

		// Token: 0x060061D3 RID: 25043 RVA: 0x003791F4 File Offset: 0x003773F4
		private void StartAttack()
		{
			DataContext context = this.CombatChar.GetDataContext();
			short distance = this.CurrentCombatDomain.GetCurrentDistance();
			TrickTypeItem trickData = TrickType.Instance[this._trickType];
			int weaponIndex = this.CombatChar.GetUsingWeaponIndex();
			BossItem bossConfig = this.CombatChar.BossConfig;
			sbyte b;
			if (bossConfig == null)
			{
				AnimalItem animalConfig = this.CombatChar.AnimalConfig;
				b = ((animalConfig != null) ? animalConfig.AttackDistances[weaponIndex] : trickData.AttackDistance[this._aniIndex]);
			}
			else
			{
				b = bossConfig.AttackDistances[(int)this.CombatChar.GetBossPhase()][weaponIndex];
			}
			sbyte displayDistance = b;
			bool flag = this._inAttackRange && this.CombatChar.PursueAttackCount == 0 && !this.CombatChar.GetIsFightBack() && displayDistance > 0 && (short)displayDistance != distance;
			if (flag)
			{
				this._moveAniFrame = 9;
				this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, this.CurrentCombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, (short)displayDistance));
			}
			else
			{
				this._moveAniFrame = 0;
				this.PlayAttackAnimation();
			}
			this._attackEndWaitFrame = 6;
			this._attackAniFrame += this._attackEndWaitFrame;
		}

		// Token: 0x060061D4 RID: 25044 RVA: 0x0037932C File Offset: 0x0037752C
		private void PlayAttackAnimation()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this.CombatChar.SetAnimationToPlayOnce(this._attackAniName, context);
			this.CombatChar.SetParticleToPlay(this._attackParticleName, context);
			this.CombatChar.SetAttackSoundToPlay(this._attackSound, context);
		}

		// Token: 0x04001A82 RID: 6786
		private bool _inAttackRange;

		// Token: 0x04001A83 RID: 6787
		private CombatCharacter _enemyChar;

		// Token: 0x04001A84 RID: 6788
		private sbyte _trickType;

		// Token: 0x04001A85 RID: 6789
		private bool _isFightBack;

		// Token: 0x04001A86 RID: 6790
		private bool _isCritical;

		// Token: 0x04001A87 RID: 6791
		private short _moveAniFrame;

		// Token: 0x04001A88 RID: 6792
		private short _attackAniFrame;

		// Token: 0x04001A89 RID: 6793
		private short _damageFrame;

		// Token: 0x04001A8A RID: 6794
		private short _pursueFrame;

		// Token: 0x04001A8B RID: 6795
		private short _attackEndWaitFrame;

		// Token: 0x04001A8C RID: 6796
		private int _weaponId;

		// Token: 0x04001A8D RID: 6797
		private int _aniIndex;

		// Token: 0x04001A8E RID: 6798
		private string _attackAniName;

		// Token: 0x04001A8F RID: 6799
		private string _attackParticleName;

		// Token: 0x04001A90 RID: 6800
		private string _attackSound;
	}
}
