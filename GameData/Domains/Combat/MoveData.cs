using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Combat
{
	// Token: 0x020006CC RID: 1740
	public struct MoveData
	{
		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06006701 RID: 26369 RVA: 0x003AF8A1 File Offset: 0x003ADAA1
		// (set) Token: 0x06006702 RID: 26370 RVA: 0x003AF8A9 File Offset: 0x003ADAA9
		public int JumpPreparedProgress { readonly get; private set; }

		// Token: 0x06006703 RID: 26371 RVA: 0x003AF8B2 File Offset: 0x003ADAB2
		public void Init(DataContext context, CombatCharacter combatChar)
		{
			this._combatChar = combatChar;
			this.JumpMoveSkillId = -1;
			this.CanPartlyJump = false;
			this.MaxJumpForwardDist = 0;
			this.MaxJumpBackwardDist = 0;
			this.Reset();
			this.ResetJumpState(context, false);
			this.ClearSkillPrepareMoveDist();
		}

		// Token: 0x06006704 RID: 26372 RVA: 0x003AF8EF File Offset: 0x003ADAEF
		public void Reset()
		{
			this.MoveCd = 0;
			this._frameCounter = -1;
			this._aniTotalFrame = -1;
			this._moveCdLongerThanAni = false;
			this._combatChar.SetAnimationTimeScale(1f, this._combatChar.GetDataContext());
		}

		// Token: 0x06006705 RID: 26373 RVA: 0x003AF92C File Offset: 0x003ADB2C
		public void ResetJumpState(DataContext context, bool calcPreparedMove = true)
		{
			bool flag = this.JumpPreparedProgress == 0 && this._combatChar.GetJumpPreparedDistance() == 0;
			if (!flag)
			{
				bool flag2 = calcPreparedMove && this.CanPartlyJump && this._combatChar.GetJumpPreparedDistance() >= 10;
				if (flag2)
				{
					this.StartMove(context, (int)(this._combatChar.GetJumpPreparedDistance() / 10 * 10), true);
				}
				this.JumpPreparedProgress = 0;
				this._reducePrepareFrameCounter = 0;
				bool flag3 = this._combatChar.GetJumpPrepareProgress() > 0;
				if (flag3)
				{
					this._combatChar.SetJumpPrepareProgress(0, context);
				}
				bool flag4 = this._combatChar.GetJumpPreparedDistance() > 0;
				if (flag4)
				{
					this._combatChar.SetJumpPreparedDistance(0, context);
				}
				bool flag5 = !this._combatChar.NeedPauseJumpMove;
				if (flag5)
				{
					this._combatChar.PauseJumpMoveSkillId = -1;
				}
			}
		}

		// Token: 0x06006706 RID: 26374 RVA: 0x003AFA0C File Offset: 0x003ADC0C
		public void UpdateJumpPrepare(DataContext context)
		{
			bool flag = this._combatChar.GetAnimationToLoop() != "C_007";
			if (flag)
			{
				this._combatChar.SetAnimationToLoop("C_007", context);
			}
			this.JumpPreparedProgress += this.GetJumpSpeed();
			bool flag2 = this.JumpPreparedProgress >= this.PrepareProgressUnit;
			if (flag2)
			{
				short preparedDistance = this._combatChar.GetJumpPreparedDistance() + 10;
				short maxJumpDistance = this._combatChar.MoveForward ? this.MaxJumpForwardDist : this.MaxJumpBackwardDist;
				this.JumpPreparedProgress -= this.PrepareProgressUnit;
				this.UpdateJumpProgress(context);
				this._combatChar.SetJumpPreparedDistance((preparedDistance >= maxJumpDistance) ? 0 : preparedDistance, context);
				bool flag3 = preparedDistance >= maxJumpDistance;
				if (flag3)
				{
					this.StartMove(context, (int)maxJumpDistance, true);
				}
			}
			else
			{
				this.UpdateJumpProgress(context);
			}
		}

		// Token: 0x06006707 RID: 26375 RVA: 0x003AFAF0 File Offset: 0x003ADCF0
		private int GetJumpSpeed()
		{
			return CombatSkillDomain.CalcJumpSpeed(this._combatChar.GetId(), this.JumpMoveSkillId);
		}

		// Token: 0x06006708 RID: 26376 RVA: 0x003AFB18 File Offset: 0x003ADD18
		public void ReduceJumpPrepare(DataContext context)
		{
			this._reducePrepareFrameCounter += 1;
			bool flag = (int)this._reducePrepareFrameCounter < MoveSpecialConstants.ReduceJumpProgressFrame;
			if (!flag)
			{
				this._reducePrepareFrameCounter = 0;
				this.JumpPreparedProgress -= this.PrepareProgressUnit * MoveSpecialConstants.ReduceJumpProgressPercent;
				bool flag2 = this.JumpPreparedProgress <= 0 && this._combatChar.GetJumpPreparedDistance() > 0;
				if (flag2)
				{
					short preparedDistance = this._combatChar.GetJumpPreparedDistance() - 10;
					this._combatChar.SetJumpPreparedDistance(preparedDistance, context);
					this.JumpPreparedProgress += this.PrepareProgressUnit;
				}
				bool flag3 = this._combatChar.GetJumpPreparedDistance() <= 0;
				if (flag3)
				{
					this.JumpPreparedProgress = Math.Max(this.JumpPreparedProgress, 0);
				}
				this.UpdateJumpProgress(context);
			}
		}

		// Token: 0x06006709 RID: 26377 RVA: 0x003AFBF4 File Offset: 0x003ADDF4
		private void UpdateJumpProgress(DataContext context)
		{
			sbyte progress = (sbyte)(this.JumpPreparedProgress * 100 / this.PrepareProgressUnit);
			bool flag = this._combatChar.GetJumpPrepareProgress() != progress;
			if (flag)
			{
				this._combatChar.SetJumpPrepareProgress(progress, context);
			}
		}

		// Token: 0x0600670A RID: 26378 RVA: 0x003AFC38 File Offset: 0x003ADE38
		public bool IsJumpMove(bool forward)
		{
			return (forward ? this._combatChar.MoveData.MaxJumpForwardDist : this._combatChar.MoveData.MaxJumpBackwardDist) > 0;
		}

		// Token: 0x0600670B RID: 26379 RVA: 0x003AFC74 File Offset: 0x003ADE74
		public bool PreparingJumpMove()
		{
			return this.PrepareProgressUnit > 0;
		}

		// Token: 0x0600670C RID: 26380 RVA: 0x003AFC90 File Offset: 0x003ADE90
		public void StartMove(DataContext context, int moveDist = 1, bool isJump = false)
		{
			int charId = this._combatChar.GetId();
			bool isMove = DomainManager.SpecialEffect.ModifyData(charId, -1, 157, true, -1, -1, -1);
			this.MoveCd = this._combatChar.GetMoveCd();
			this.SetMoveAnimation(context, isJump, isMove);
			if (isJump)
			{
				moveDist = DomainManager.SpecialEffect.ModifyValue(this._combatChar.GetId(), this.JumpMoveSkillId, 165, moveDist, this._combatChar.MoveForward ? 1 : 0, -1, -1, 0, 0, 0, 0);
				this.SetJumpAnimation(context);
			}
			Events.RaiseMoveBegin(context, this._combatChar, moveDist, isJump);
			this.MoveChangeDistance(context, moveDist, isJump);
			Events.RaiseMoveEnd(context, this._combatChar, moveDist, isJump);
			this.DoMoveCost(context);
			this.UpdateCanMoveInSkillPrepareDist(moveDist);
		}

		// Token: 0x0600670D RID: 26381 RVA: 0x003AFD5C File Offset: 0x003ADF5C
		public void UpdateMove(DataContext context, CombatDomain currentCombatDomain)
		{
			this.MoveCd -= 1;
			this._frameCounter += 1;
			bool flag = this._frameCounter == this._aniTotalFrame;
			if (flag)
			{
				bool moveCdLongerThanAni = this._moveCdLongerThanAni;
				if (moveCdLongerThanAni)
				{
					currentCombatDomain.SetProperLoopAniAndParticle(context, this._combatChar, false);
				}
				else
				{
					this._frameCounter = 0;
				}
			}
			bool flag2 = this.MoveCd == 0 && this._combatChar.KeepMoving && !currentCombatDomain.CanMove(this._combatChar, this._combatChar.MoveForward);
			if (flag2)
			{
				currentCombatDomain.SetProperLoopAniAndParticle(context, this._combatChar, false);
			}
		}

		// Token: 0x0600670E RID: 26382 RVA: 0x003AFE03 File Offset: 0x003AE003
		public void ClearSkillPrepareMoveDist()
		{
			this.CanMoveForwardInSkillPrepareDist = 0;
			this.CanMoveBackwardInSkillPrepareDist = 0;
		}

		// Token: 0x0600670F RID: 26383 RVA: 0x003AFE14 File Offset: 0x003AE014
		private void DoMoveCost(DataContext context)
		{
			short moveSkillId = this._combatChar.GetAffectingMoveSkillId();
			bool flag = moveSkillId < 0;
			if (!flag)
			{
				int costMobility = DomainManager.Combat.GetSkillMoveCostMobility(this._combatChar, moveSkillId);
				bool flag2 = Config.CombatSkill.Instance[moveSkillId].MaxJumpDistance > 0 && (this._combatChar.MoveForward ? (this.MaxJumpForwardDist <= 0) : (this.MaxJumpBackwardDist <= 0));
				if (flag2)
				{
					costMobility = costMobility * GlobalConfig.Instance.AgileSkillNonJumpDirectionCostMobilityPercent / 100;
				}
				bool flag3 = costMobility > 0;
				if (flag3)
				{
					DomainManager.Combat.ChangeMobilityValue(context, this._combatChar, -costMobility, false, null, false);
				}
			}
		}

		// Token: 0x06006710 RID: 26384 RVA: 0x003AFEC0 File Offset: 0x003AE0C0
		private void SetMoveAnimation(DataContext context, bool isJump, bool isMove)
		{
			string moveAni;
			bool anyChanged;
			if (isMove)
			{
				ValueTuple<string, bool> valueTuple = DomainManager.Combat.SetProperLoopAniAndParticle(context, this._combatChar, true);
				moveAni = valueTuple.Item1;
				anyChanged = valueTuple.Item2;
			}
			else
			{
				moveAni = DomainManager.Combat.GetIdleAni(this._combatChar);
				anyChanged = (moveAni != this._combatChar.GetAnimationToLoop());
				bool flag = anyChanged;
				if (flag)
				{
					this._combatChar.SetAnimationToLoop(moveAni, context);
				}
			}
			float aniTimeScale = isJump ? 1f : Math.Max(3f / (float)this.MoveCd, 1f);
			bool flag2 = !anyChanged && Math.Abs(this._combatChar.GetAnimationTimeScale() - aniTimeScale) < 0.1f;
			if (!flag2)
			{
				bool flag3 = moveAni != null;
				if (flag3)
				{
					AnimData moveAniData = AnimDataCollection.Data[moveAni];
					this._aniTotalFrame = (short)(moveAniData.Duration * 60f / aniTimeScale);
					this._frameCounter = 0;
					this._moveCdLongerThanAni = (this.MoveCd > this._aniTotalFrame);
					this._combatChar.SetAnimationTimeScale(aniTimeScale, context);
				}
				else
				{
					this._aniTotalFrame = -1;
				}
			}
		}

		// Token: 0x06006711 RID: 26385 RVA: 0x003AFFDC File Offset: 0x003AE1DC
		private void SetJumpAnimation(DataContext context)
		{
			string jumpAni = this._combatChar.MoveForward ? "M_003_fly" : "M_004_fly";
			this._combatChar.SetAnimationToPlayOnce(jumpAni, context);
			DomainManager.Combat.PlayWhooshSound(context, this._combatChar);
			BossItem bossConfig = this._combatChar.BossConfig;
			bool flag = ((bossConfig != null) ? bossConfig.JumpMoveParticles : null) != null;
			if (flag)
			{
				this._combatChar.SetParticleToPlay(this._combatChar.BossConfig.JumpMoveParticles[this._combatChar.MoveForward ? 0 : 1], context);
			}
			AnimalItem animalConfig = this._combatChar.AnimalConfig;
			bool flag2 = ((animalConfig != null) ? animalConfig.JumpMoveParticles : null) != null;
			if (flag2)
			{
				this._combatChar.SetParticleToPlay(this._combatChar.AnimalConfig.JumpMoveParticles[this._combatChar.MoveForward ? 0 : 1], context);
			}
		}

		// Token: 0x06006712 RID: 26386 RVA: 0x003B00C8 File Offset: 0x003AE2C8
		private void MoveChangeDistance(DataContext context, int moveDist, bool isJump)
		{
			bool flag = isJump && this._combatChar.GetAffectingMoveSkillId() == 757;
			if (flag)
			{
				short aiTargetDist = this._combatChar.AiController.GetTargetDistance();
				bool flag2 = aiTargetDist >= 0;
				if (flag2)
				{
					DomainManager.Combat.ChangeDistance(context, this._combatChar, (int)(aiTargetDist - DomainManager.Combat.GetCurrentDistance()), false, false);
				}
			}
			else
			{
				int moveDistWithDirection = this._combatChar.MoveForward ? (-moveDist) : moveDist;
				bool flag3 = isJump && this._combatChar.PauseJumpMoveSkillId >= 0 && this._combatChar.GetPreparingOtherAction() < 0 && this._combatChar.GetPreparingSkillId() < 0 && Config.CombatSkill.Instance[this._combatChar.PauseJumpMoveSkillId].JumpAni != null;
				if (flag3)
				{
					this._combatChar.PauseJumpMoveDistance = moveDistWithDirection;
					this._combatChar.NeedPauseJumpMove = true;
				}
				else
				{
					DomainManager.Combat.ChangeDistance(context, this._combatChar, moveDistWithDirection);
				}
			}
		}

		// Token: 0x06006713 RID: 26387 RVA: 0x003B01D0 File Offset: 0x003AE3D0
		private void UpdateCanMoveInSkillPrepareDist(int moveDist)
		{
			bool flag = this._combatChar.GetPreparingSkillId() < 0;
			if (!flag)
			{
				bool moveForward = this._combatChar.MoveForward;
				if (moveForward)
				{
					this.CanMoveForwardInSkillPrepareDist = (short)((int)this.CanMoveForwardInSkillPrepareDist - Math.Abs(moveDist));
				}
				else
				{
					this.CanMoveBackwardInSkillPrepareDist = (short)((int)this.CanMoveBackwardInSkillPrepareDist - Math.Abs(moveDist));
				}
			}
		}

		// Token: 0x04001C04 RID: 7172
		private CombatCharacter _combatChar;

		// Token: 0x04001C05 RID: 7173
		public short MoveCd;

		// Token: 0x04001C06 RID: 7174
		private short _frameCounter;

		// Token: 0x04001C07 RID: 7175
		private short _aniTotalFrame;

		// Token: 0x04001C08 RID: 7176
		private bool _moveCdLongerThanAni;

		// Token: 0x04001C09 RID: 7177
		public short JumpMoveSkillId;

		// Token: 0x04001C0A RID: 7178
		public bool CanPartlyJump;

		// Token: 0x04001C0B RID: 7179
		public short MaxJumpForwardDist;

		// Token: 0x04001C0C RID: 7180
		public short MaxJumpBackwardDist;

		// Token: 0x04001C0D RID: 7181
		public int PrepareProgressUnit;

		// Token: 0x04001C0F RID: 7183
		public byte JumpPrepareDirection;

		// Token: 0x04001C10 RID: 7184
		private sbyte _reducePrepareFrameCounter;

		// Token: 0x04001C11 RID: 7185
		public short CanMoveForwardInSkillPrepareDist;

		// Token: 0x04001C12 RID: 7186
		public short CanMoveBackwardInSkillPrepareDist;
	}
}
