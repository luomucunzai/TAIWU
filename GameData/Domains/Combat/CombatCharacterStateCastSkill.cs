using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.Combat
{
	// Token: 0x02000697 RID: 1687
	public class CombatCharacterStateCastSkill : CombatCharacterStateBase
	{
		// Token: 0x060061EE RID: 25070 RVA: 0x0037A8B3 File Offset: 0x00378AB3
		public CombatCharacterStateCastSkill(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.CastSkill)
		{
			this.RequireDelayFallen = true;
			this.IsUpdateOnPause = true;
		}

		// Token: 0x060061EF RID: 25071 RVA: 0x0037A8DC File Offset: 0x00378ADC
		public override void OnEnter()
		{
			this._translateStateDelayedFrame = -1;
			DataContext context = this.CombatChar.GetDataContext();
			short skillId = this.CombatChar.GetPreparingSkillId();
			CombatCharacter enemyChar = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, true);
			this._configData = CombatSkill.Instance[skillId];
			this._attackEndWaitFrame = 6;
			bool flag = this._configData.EquipType == 1;
			if (flag)
			{
				this.CombatChar.SkillAttackBodyPart = this.CurrentCombatDomain.GetAttackBodyPart(this.CombatChar, enemyChar, context.Random, this._configData.TemplateId, -1, -1);
			}
			Events.RaisePrepareSkillChangeDistance(context, this.CombatChar, enemyChar, skillId);
			Events.RaisePrepareSkillEnd(context, this.CombatChar.GetId(), this.CombatChar.IsAlly, skillId);
			bool flag2 = this._configData.EquipType == 1;
			if (flag2)
			{
				this.CurrentCombatDomain.CalcAttackSkillDataCompare(CombatContext.Create(this.CombatChar, null, -1, skillId, -1, null));
				this._outOfRange = !this.CurrentCombatDomain.InAttackRange(this.CombatChar);
				bool flag3 = !this._outOfRange;
				if (flag3)
				{
					short distance = (this._configData.PlayerCastBossSkillDistance == null || this.CombatChar.BossConfig != null) ? this._configData.DistanceWhenFourStepAnimation[0] : this._configData.PlayerCastBossSkillDistance[0];
					this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, this.CurrentCombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, distance));
				}
				bool flag4 = this.CurrentCombatDomain.IsPlayingMoveAni(enemyChar);
				if (flag4)
				{
					enemyChar.SetAnimationToLoop(this.CurrentCombatDomain.GetIdleAni(enemyChar), context);
				}
				this.PlayPrepareFinishAni();
				Events.RaiseCastAttackSkillBegin(context, this.CombatChar, enemyChar, skillId);
			}
			this.CombatChar.SetPreparingSkillId(-1, context);
		}

		// Token: 0x060061F0 RID: 25072 RVA: 0x0037AAC8 File Offset: 0x00378CC8
		public override void OnExit()
		{
			DataContext context = this.CombatChar.GetDataContext();
			this.CombatChar.SetSkillPreparePercent(0, context);
			this.CombatChar.SetSkillSoundToPlay(string.Empty, context);
			this.CombatChar.SetParticleToPlay(string.Empty, context);
		}

		// Token: 0x060061F1 RID: 25073 RVA: 0x0037AB14 File Offset: 0x00378D14
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
				bool flag2 = this._configData.EquipType == 1;
				if (flag2)
				{
					bool flag3 = this._prepareFinishAniFrame > 0;
					if (flag3)
					{
						this._prepareFinishAniFrame -= 1;
						bool flag4 = this._prepareFinishAniFrame == 0;
						if (flag4)
						{
							DataContext context = this.CombatChar.GetDataContext();
							bool isBoss = this.CombatChar.BossConfig != null;
							string aniName = (string.IsNullOrEmpty(this._configData.PlayerCastBossSkillAni) || isBoss) ? this._configData.CastAnimation : this._configData.PlayerCastBossSkillAni;
							string musicWeaponFix = (this._configData.Type != 13 || isBoss) ? "" : this.CurrentCombatDomain.GetMusicWeaponNameFix(this.CombatChar.GetWeaponData(-1));
							bool flag5 = isBoss;
							if (flag5)
							{
								aniName = this.CombatChar.BossConfig.AniPrefix[(int)this.CombatChar.GetBossPhase()] + aniName;
							}
							else
							{
								aniName += musicWeaponFix;
							}
							AnimData aniData = AnimDataCollection.Data[aniName];
							this._skillAniFrame = (short)Math.Round((double)(aniData.Duration * 60f));
							this._skillAniFrame += this._attackEndWaitFrame;
							for (int i = 0; i < 4; i++)
							{
								short[] damageFrame = this._damageFrame;
								int num = i;
								Dictionary<string, float[]> events = aniData.Events;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
								defaultInterpolatedStringHandler.AppendLiteral("act");
								defaultInterpolatedStringHandler.AppendFormatted<int>(i + 1);
								damageFrame[num] = (short)Math.Round((double)(events[defaultInterpolatedStringHandler.ToStringAndClear()][0] * 60f));
							}
							this._skillAniFrameCounter = 0;
							this.CombatChar.SetAnimationToPlayOnce((!isBoss) ? aniName : this._configData.CastAnimation, context);
							bool flag6 = !string.IsNullOrEmpty(this._configData.CastParticle);
							if (flag6)
							{
								string castParticle = (string.IsNullOrEmpty(this._configData.PlayerCastBossSkillParticle) || isBoss) ? this._configData.CastParticle : this._configData.PlayerCastBossSkillParticle;
								this.CombatChar.SetParticleToPlay((!isBoss) ? (castParticle + musicWeaponFix) : this._configData.CastParticle, context);
							}
							bool flag7 = !string.IsNullOrEmpty(this._configData.CastSoundEffect);
							if (flag7)
							{
								string castSound = (string.IsNullOrEmpty(this._configData.PlayerCastBossSkillSound) || isBoss) ? this._configData.CastSoundEffect : this._configData.PlayerCastBossSkillSound;
								this.CombatChar.SetSkillSoundToPlay(castSound + musicWeaponFix, context);
							}
							bool flag8 = !string.IsNullOrEmpty(this._configData.CastPetAnimation) && isBoss;
							if (flag8)
							{
								this.CombatChar.SetSkillPetAnimation(this._configData.CastPetAnimation, context);
							}
							bool flag9 = !string.IsNullOrEmpty(this._configData.CastPetParticle) && isBoss;
							if (flag9)
							{
								this.CombatChar.SetPetParticle(this._configData.CastPetParticle, context);
							}
							this.CombatChar.SetAnimationToLoop(this.CurrentCombatDomain.GetIdleAni(this.CombatChar), context);
						}
						return false;
					}
					this._skillAniFrameCounter += 1;
					for (int j = 0; j < this._damageFrame.Length; j++)
					{
						bool flag10 = this._damageFrame[j] == this._skillAniFrameCounter;
						if (flag10)
						{
							DataContext context2 = this.CombatChar.GetDataContext();
							bool flag11 = j == 3 || this.CombatChar.SkillHitType[j] >= 0;
							if (flag11)
							{
								bool outOfRange = this._outOfRange;
								if (outOfRange)
								{
									this.CombatChar.SetAttackOutOfRange(true, context2);
								}
								else
								{
									this.CurrentCombatDomain.CalcSkillAttack(CombatContext.Create(this.CombatChar, null, -1, -1, -1, null), j);
								}
								bool flag12 = j == 3 && this._configData.WeaponDurableCost > 0;
								if (flag12)
								{
									this.CurrentCombatDomain.CostDurability(context2, this.CombatChar, this.CurrentCombatDomain.GetUsingWeaponKey(this.CombatChar), (int)this._configData.WeaponDurableCost);
								}
							}
							bool flag13 = !this._outOfRange;
							if (flag13)
							{
								short distance = (this._configData.PlayerCastBossSkillDistance == null || this.CombatChar.BossConfig != null) ? this._configData.DistanceWhenFourStepAnimation[j + 1] : this._configData.PlayerCastBossSkillDistance[j + 1];
								this.CurrentCombatDomain.SetDisplayPosition(context2, !this.CombatChar.IsAlly, this.CurrentCombatDomain.GetDisplayPosition(!this.CombatChar.IsAlly, distance));
							}
							Events.RaiseAttackSkillAttackEndOfAll(context2, this.CombatChar, j);
							bool flag14 = j < 3;
							if (flag14)
							{
								this.CombatChar.SetAttackSkillAttackIndex((byte)(j + 1), context2);
							}
							break;
						}
					}
					bool flag15 = this._skillAniFrameCounter == this._skillAniFrame - this._attackEndWaitFrame;
					if (flag15)
					{
						DataContext context3 = this.CombatChar.GetDataContext();
						sbyte power = (sbyte)this.CombatChar.GetAttackSkillPower();
						this.CombatChar.SetPerformingSkillId(-1, context3);
						this.CombatChar.SetAttackSkillPower(0, context3);
						this.CombatChar.SetSkillPetAnimation(null, context3);
						this.CurrentCombatDomain.SetDisplayPosition(context3, this.CombatChar.IsAlly, int.MinValue);
						this.CurrentCombatDomain.SetDisplayPosition(context3, !this.CombatChar.IsAlly, int.MinValue);
						int finalCriticalOdds = this.CurrentCombatDomain.GetFinalCriticalOdds(this.CombatChar);
						this.CurrentCombatDomain.ClearDamageCompareData(context3);
						DomainManager.Combat.RaiseCastSkillEnd(context3, this.CombatChar.GetId(), this.CombatChar.IsAlly, this._configData.TemplateId, power, false, finalCriticalOdds);
						CombatCharacter character = this.CombatChar;
						CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!character.IsAlly, true);
						DomainManager.Combat.AddToCheckFallenSet(character.GetId());
						DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
						bool flag16 = this.CombatChar.GetAutoCastingSkill() && this.CombatChar.NeedUseSkillFreeId < 0;
						if (flag16)
						{
							this.CombatChar.SetAutoCastingSkill(false, context3);
						}
					}
					bool flag17 = this._skillAniFrameCounter >= this._skillAniFrame;
					if (flag17)
					{
						DataContext context4 = this.CombatChar.GetDataContext();
						CombatCharacter enemyChar2 = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, true);
						bool flag18 = enemyChar2.GetAnimationToLoop() == this.CurrentCombatDomain.GetIdleAni(enemyChar2);
						if (flag18)
						{
							this.CurrentCombatDomain.SetProperLoopAniAndParticle(context4, enemyChar2, false);
						}
						this.CombatChar.StateMachine.TranslateState();
					}
				}
				else
				{
					bool flag19 = this._translateStateDelayedFrame < 0;
					if (flag19)
					{
						DataContext context5 = this.CombatChar.GetDataContext();
						this.CurrentCombatDomain.ApplyAgileOrDefenseSkill(this.CombatChar, this._configData);
						this.CombatChar.SetPerformingSkillId(-1, context5);
						this._translateStateDelayedFrame = ((this._configData.EquipType == 2) ? 1 : 0);
						DomainManager.Combat.RaiseCastSkillEnd(context5, this.CombatChar.GetId(), this.CombatChar.IsAlly, this._configData.TemplateId, 0, false, 0);
					}
				}
				bool flag20 = this._translateStateDelayedFrame < 0;
				if (flag20)
				{
					result = false;
				}
				else
				{
					bool flag21 = this._translateStateDelayedFrame <= 0;
					if (flag21)
					{
						this.CombatChar.StateMachine.TranslateState();
					}
					this._translateStateDelayedFrame--;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060061F2 RID: 25074 RVA: 0x0037B308 File Offset: 0x00379508
		private void PlayPrepareFinishAni()
		{
			DataContext context = this.CombatChar.GetDataContext();
			bool isBoss = this.CombatChar.BossConfig != null;
			string musicWeaponFix = (this._configData.Type != 13) ? "" : this.CurrentCombatDomain.GetMusicWeaponNameFix(this.CombatChar.GetWeaponData(-1));
			string prepareFinishAni = (string.IsNullOrEmpty(this._configData.PlayerCastBossSkillPrepareAni) || isBoss) ? (this._configData.PrepareAnimation + musicWeaponFix + "_1_1") : (this._configData.PlayerCastBossSkillPrepareAni + musicWeaponFix + "_1_1");
			string prepareFinishAniName = (!isBoss) ? prepareFinishAni : "C_007_1";
			float duration = AnimDataCollection.Data[(!isBoss) ? prepareFinishAniName : (this.CombatChar.BossConfig.AniPrefix[(int)this.CombatChar.GetBossPhase()] + "C_007_1")].Duration;
			this._prepareFinishAniFrame = (short)Math.Round((double)(duration * 60f + 3f));
			this.CombatChar.SetAnimationToPlayOnce(prepareFinishAniName, context);
			this.CombatChar.SetSkillSoundToPlay("se_combat_preskill", context);
		}

		// Token: 0x04001A9D RID: 6813
		private short _prepareFinishAniFrame;

		// Token: 0x04001A9E RID: 6814
		private short _skillAniFrame;

		// Token: 0x04001A9F RID: 6815
		private readonly short[] _damageFrame = new short[4];

		// Token: 0x04001AA0 RID: 6816
		private short _skillAniFrameCounter;

		// Token: 0x04001AA1 RID: 6817
		private CombatSkillItem _configData;

		// Token: 0x04001AA2 RID: 6818
		private bool _outOfRange;

		// Token: 0x04001AA3 RID: 6819
		private short _attackEndWaitFrame;

		// Token: 0x04001AA4 RID: 6820
		private int _translateStateDelayedFrame;
	}
}
