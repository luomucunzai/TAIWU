using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x02000695 RID: 1685
	public class CombatCharacterStateBase
	{
		// Token: 0x060061D5 RID: 25045 RVA: 0x0037937F File Offset: 0x0037757F
		protected void DelayCall(CombatCharacterStateBase.CombatCharacterStateDelayCallRequest request, int frame)
		{
			this.DelayCall(request, null, frame);
		}

		// Token: 0x060061D6 RID: 25046 RVA: 0x0037938C File Offset: 0x0037758C
		protected void DelayCall(CombatCharacterStateBase.CombatCharacterStateDelayCallRequest request, CombatCharacterStateBase.CombatCharacterStateDelayCallTickPercent tickPercent, int frame)
		{
			bool flag = frame <= 0;
			if (flag)
			{
				if (request != null)
				{
					request();
				}
				if (tickPercent != null)
				{
					tickPercent(100);
				}
			}
			else
			{
				this._delayCallList.Enqueue(new CombatCharacterStateBase.DelayCallData(request, tickPercent, frame));
			}
		}

		// Token: 0x060061D7 RID: 25047 RVA: 0x003793D7 File Offset: 0x003775D7
		public CombatCharacterStateBase(CombatDomain combatDomain, CombatCharacter combatChar, CombatCharacterStateType type)
		{
			this.CurrentCombatDomain = combatDomain;
			this.CombatChar = combatChar;
			this.StateType = type;
			this.IsUpdateOnPause = false;
		}

		// Token: 0x060061D8 RID: 25048 RVA: 0x00379408 File Offset: 0x00377608
		public virtual void OnEnter()
		{
			this.AutoUpdateDelayCall = true;
			this._delayCallList.Clear();
		}

		// Token: 0x060061D9 RID: 25049 RVA: 0x0037941E File Offset: 0x0037761E
		public virtual void OnExit()
		{
		}

		// Token: 0x060061DA RID: 25050 RVA: 0x00379424 File Offset: 0x00377624
		public virtual bool OnUpdate()
		{
			bool flag = this.CombatChar.ChangeCharId >= 0 && !this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, false).NeedSelectMercyOption;
			bool result;
			if (flag)
			{
				this.ChangeCurrChar();
				result = false;
			}
			else
			{
				bool flag2 = this.CheckCommonTranslateState();
				if (flag2)
				{
					this.CombatChar.StateMachine.TranslateState();
					result = false;
				}
				else
				{
					bool flag3 = !DomainManager.Combat.Pause;
					if (flag3)
					{
						DataContext context = this.CombatChar.GetDataContext();
						int[] charList = this.CurrentCombatDomain.GetCharacterList(this.CombatChar.IsAlly);
						int mainCharId = (this.CombatChar.IsAlly ? this.CurrentCombatDomain.GetSelfTeam() : this.CurrentCombatDomain.GetEnemyTeam())[0];
						this.TimeUpdate(context, this.CombatChar, true);
						bool flag4 = this.CurrentCombatDomain.IsMainCharacter(this.CombatChar);
						if (flag4)
						{
							bool flag5 = this.CombatChar.ExecuteReserveTeammateCommand(context) || this.CombatChar.UpdateTeammateCharStatus(context);
							if (flag5)
							{
								return false;
							}
						}
						else
						{
							CombatCharacter mainChar = this.CurrentCombatDomain.GetElement_CombatCharacterDict(mainCharId);
							this.TimeUpdate(context, mainChar, true);
							bool flag6 = this.CombatChar.TeammateCommandLeftFrame > 0;
							if (flag6)
							{
								this.CombatChar.ReduceTeammateCommandLeftTime(context);
							}
							bool flag7 = (this.CombatChar.TeammateCommandLeftFrame == 0 && this.CombatChar.GetPreparingSkillId() < 0) || this.CurrentCombatDomain.IsCharacterFallen(mainChar);
							if (flag7)
							{
								this.CombatChar.ResetTeammateCommandCd(context, this.CombatChar.ExecutingTeammateCommandIndex, -1, true, true);
								this.CombatChar.TeammateCommandLeftFrame = -1;
								this.CombatChar.ExecutingTeammateCommandIndex = -1;
								this.CombatChar.ChangeCharId = mainCharId;
							}
						}
						for (int i = 1; i < charList.Length; i++)
						{
							int charId = charList[i];
							bool flag8 = charId < 0 || charId == this.CombatChar.GetId() || charId == mainCharId;
							if (!flag8)
							{
								CombatCharacter teammateChar = this.CurrentCombatDomain.GetElement_CombatCharacterDict(charId);
								bool flag9 = !this.CurrentCombatDomain.IsCharacterFallen(teammateChar);
								if (flag9)
								{
									this.TimeUpdate(context, teammateChar, false);
								}
							}
						}
						bool isMoving = this.CombatChar.IsMoving;
						if (isMoving)
						{
							this.CombatChar.MoveData.UpdateMove(context, this.CurrentCombatDomain);
						}
						else
						{
							bool flag10 = this.CombatChar.KeepMoving && this.CurrentCombatDomain.CanMove(this.CombatChar, this.CombatChar.MoveForward);
							if (flag10)
							{
								bool flag11 = this.CombatChar.MoveData.IsJumpMove(this.CombatChar.MoveForward);
								if (flag11)
								{
									this.CombatChar.MoveData.UpdateJumpPrepare(context);
								}
								else
								{
									this.CombatChar.MoveData.StartMove(context, 1, false);
								}
							}
							else
							{
								bool flag12 = !this.CombatChar.KeepMoving && (this.CombatChar.MoveData.JumpPreparedProgress > 0 || this.CombatChar.GetJumpPreparedDistance() > 0);
								if (flag12)
								{
									this.CombatChar.MoveData.ReduceJumpPrepare(context);
								}
							}
						}
					}
					bool autoUpdateDelayCall = this.AutoUpdateDelayCall;
					if (autoUpdateDelayCall)
					{
						this.UpdateDelayCall();
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060061DB RID: 25051 RVA: 0x003797A0 File Offset: 0x003779A0
		protected void UpdateDelayCall()
		{
			int count = this._delayCallList.Count;
			while (count > 0)
			{
				count--;
				CombatCharacterStateBase.DelayCallData request = this._delayCallList.Dequeue();
				bool flag = !request.Tick();
				if (flag)
				{
					this._delayCallList.Enqueue(request);
				}
			}
		}

		// Token: 0x060061DC RID: 25052 RVA: 0x003797F4 File Offset: 0x003779F4
		private void ChangeCurrChar()
		{
			DataContext context = this.CombatChar.GetDataContext();
			CombatCharacter newChar = this.CurrentCombatDomain.GetElement_CombatCharacterDict(this.CombatChar.ChangeCharId);
			this.CombatChar.ClearAllDoingOrReserveCommand(context);
			this.CombatChar.SetAffectingDefendSkillId(-1, context);
			this.CombatChar.SetAffectingMoveSkillId(-1, context);
			this.CombatChar.SetAnimationToLoop(null, context);
			this.CombatChar.SetParticleToLoop(null, context);
			bool flag = this.CombatChar.ChangeCharFailAni != null;
			if (flag)
			{
				this.CombatChar.SetAnimationToPlayOnce(this.CombatChar.ChangeCharFailAni, context);
				bool flag2 = this.CombatChar.ChangeCharFailParticle != "";
				if (flag2)
				{
					this.CombatChar.SetParticleToPlay(this.CombatChar.ChangeCharFailParticle, context);
				}
				bool flag3 = this.CombatChar.ChangeCharFailSound != "";
				if (flag3)
				{
					this.CombatChar.SetDieSoundToPlay(this.CombatChar.ChangeCharFailSound, context);
				}
			}
			else
			{
				this.CombatChar.SetAnimationToPlayOnce("M_004", context);
				this.CombatChar.SetParticleToPlay(null, context);
			}
			this.CurrentCombatDomain.SetCombatCharacter(context, this.CombatChar.IsAlly, this.CombatChar.ChangeCharId);
			bool flag4 = !this.CurrentCombatDomain.IsMainCharacter(this.CombatChar);
			if (flag4)
			{
				newChar.TeammateHasCommand[this.CurrentCombatDomain.GetCharacterList(this.CombatChar.IsAlly).IndexOf(this.CombatChar.GetId()) - 1] = false;
			}
			bool flag5 = this.CombatChar.ExecutingTeammateCommandChangeDistance != 0;
			if (flag5)
			{
				this.CurrentCombatDomain.ChangeDistance(context, this.CombatChar, this.CombatChar.ExecutingTeammateCommandChangeDistance);
				this.CombatChar.ExecutingTeammateCommandChangeDistance = 0;
			}
			this.CombatChar.SetExecutingTeammateCommand(-1, context);
			this.CombatChar.SetTeammateCommandTimePercent(0, context);
			this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Idle);
			this.CombatChar.ChangeCharId = -1;
			this.CombatChar.ClearAllSound(context);
			newChar.SetCurrentPosition(this.CombatChar.GetCurrentPosition(), context);
			this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, int.MinValue);
			newChar.StateMachine.TranslateState(CombatCharacterStateType.ChangeCharacter);
		}

		// Token: 0x060061DD RID: 25053 RVA: 0x00379A54 File Offset: 0x00377C54
		private bool CheckCommonTranslateState()
		{
			CombatCharacterStateType properState = this.CombatChar.StateMachine.GetProperState();
			bool flag = this.StateType == CombatCharacterStateType.Idle;
			if (flag)
			{
				bool flag2 = !this.CombatChar.IsMoving && properState != this.StateType;
				if (flag2)
				{
					return true;
				}
				bool flag3 = this.CombatChar.IsMoving && properState == CombatCharacterStateType.PrepareAttack;
				if (flag3)
				{
					return true;
				}
			}
			bool flag4 = this.StateType == CombatCharacterStateType.PrepareAttack;
			return flag4 && properState == CombatCharacterStateType.ChangeBossPhase;
		}

		// Token: 0x060061DE RID: 25054 RVA: 0x00379AE0 File Offset: 0x00377CE0
		private void TimeUpdate(DataContext context, CombatCharacter combatChar, bool isMainOrCurrChar = true)
		{
			CombatCharacterStateBase.TimeUpdateRecoverStandard(context, combatChar, isMainOrCurrChar);
			CombatCharacterStateBase.TimeUpdateFlawAcupoint(context, combatChar);
			CombatCharacterStateBase.TimeUpdateMindMark(context, combatChar);
			CombatCharacterStateBase.TimeUpdateAutoHeal(context, combatChar);
			CombatCharacterStateBase.TimeUpdateNeiliAllocation(context, combatChar);
			CombatCharacterStateBase.TimeUpdateMain(context, combatChar, isMainOrCurrChar);
		}

		// Token: 0x060061DF RID: 25055 RVA: 0x00379B18 File Offset: 0x00377D18
		private static void TimeUpdateRecoverStandard(DataContext context, CombatCharacter combatChar, bool isMainOrCurrChar)
		{
			bool preparingSkill = combatChar.GetPreparingSkillId() >= 0;
			bool flag = !isMainOrCurrChar || combatChar.GetPreparingOtherAction() != -1;
			if (!flag)
			{
				CombatDomain combatDomain = DomainManager.Combat;
				bool flag2 = combatChar.GetBreathValue() < combatChar.GetMaxBreathValue() && !preparingSkill;
				if (flag2)
				{
					combatDomain.RecoverBreathValue(context, combatChar);
					bool flag3 = combatDomain.IsCurrentCombatCharacter(combatChar);
					if (flag3)
					{
						combatDomain.UpdateSkillCostBreathStanceCanUse(context, combatChar);
					}
				}
				bool flag4 = combatChar.GetAffectingMoveSkillId() < 0 && (!preparingSkill || combatChar.MoveData.CanMoveForwardInSkillPrepareDist > 0 || combatChar.MoveData.CanMoveBackwardInSkillPrepareDist > 0);
				if (flag4)
				{
					combatDomain.RecoverMobilityValue(context, combatChar);
				}
				ItemKey[] weapons = combatChar.GetWeapons();
				for (int i = 0; i < 3; i++)
				{
					bool flag5 = !weapons[i].IsValid();
					if (!flag5)
					{
						int addValue = combatChar.GetRecoverUnlockAttackValue(weapons[i]);
						bool flag6 = addValue > 0;
						if (flag6)
						{
							combatChar.ChangeUnlockAttackValue(context, i, addValue);
						}
					}
				}
			}
		}

		// Token: 0x060061E0 RID: 25056 RVA: 0x00379C2C File Offset: 0x00377E2C
		private static void TimeUpdateFlawAcupoint(DataContext context, CombatCharacter combatChar)
		{
			CombatDomain combatDomain = DomainManager.Combat;
			FlawOrAcupointCollection flaw = combatChar.GetFlawCollection();
			FlawOrAcupointCollection acupoint = combatChar.GetAcupointCollection();
			byte[] flawCount = combatChar.GetFlawCount();
			byte[] acupointCount = combatChar.GetAcupointCount();
			short flawSpeed = combatChar.GetRecoveryOfFlaw();
			short acupointSpeed = combatChar.GetRecoveryOfAcupoint();
			FlawOrAcupointCollection.ReduceKeepTimeResult flawRetValue = flaw.ReduceKeepTime(combatChar, (int)flawSpeed, flawCount, true);
			FlawOrAcupointCollection.ReduceKeepTimeResult acupointRetValue = acupoint.ReduceKeepTime(combatChar, (int)acupointSpeed, acupointCount, false);
			bool dataChanged = flawRetValue.DataChanged;
			if (dataChanged)
			{
				combatChar.SetFlawCollection(flaw, context);
			}
			bool dataChanged2 = acupointRetValue.DataChanged;
			if (dataChanged2)
			{
				combatChar.SetAcupointCollection(acupoint, context);
			}
			bool flag = flawRetValue.CountChanged || acupointRetValue.CountChanged;
			if (flag)
			{
				bool countChanged = flawRetValue.CountChanged;
				if (countChanged)
				{
					combatChar.SetFlawCount(combatChar.GetFlawCount(), context);
					bool flag2 = combatDomain.IsMainCharacter(combatChar);
					if (flag2)
					{
						combatDomain.UpdateAllTeammateCommandUsable(context, combatChar.IsAlly, ETeammateCommandImplement.HealFlaw);
					}
				}
				bool countChanged2 = acupointRetValue.CountChanged;
				if (countChanged2)
				{
					combatChar.SetAcupointCount(combatChar.GetAcupointCount(), context);
					bool flag3 = combatDomain.IsMainCharacter(combatChar);
					if (flag3)
					{
						combatDomain.UpdateAllTeammateCommandUsable(context, combatChar.IsAlly, ETeammateCommandImplement.HealFlaw);
					}
				}
				combatDomain.UpdateBodyDefeatMark(context, combatChar);
			}
			for (int i = 0; i < flawRetValue.RemovedList.Count; i++)
			{
				ValueTuple<sbyte, sbyte> removedFlaw = flawRetValue.RemovedList[i];
				Events.RaiseFlawRemoved(context, combatChar, removedFlaw.Item1, removedFlaw.Item2);
			}
			for (int j = 0; j < acupointRetValue.RemovedList.Count; j++)
			{
				ValueTuple<sbyte, sbyte> removedAcupoint = acupointRetValue.RemovedList[j];
				Events.RaiseAcuPointRemoved(context, combatChar, removedAcupoint.Item1, removedAcupoint.Item2);
			}
		}

		// Token: 0x060061E1 RID: 25057 RVA: 0x00379DDC File Offset: 0x00377FDC
		private static void TimeUpdateMindMark(DataContext context, CombatCharacter combatChar)
		{
			MindMarkList mindMarkList = combatChar.GetMindMarkTime();
			List<SilenceFrameData> markList = mindMarkList.MarkList;
			bool flag = markList == null || markList.Count <= 0;
			if (!flag)
			{
				int mindRecoverSpeed = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 187, 1, -1, -1, -1, 0, 0, 0, 0);
				int oldMarkCount = mindMarkList.MarkList.Count;
				for (int i = mindMarkList.MarkList.Count - 1; i >= 0; i--)
				{
					SilenceFrameData mark = mindMarkList.MarkList[i];
					mark.Tick(mindRecoverSpeed);
					bool silencing = mark.Silencing;
					if (silencing)
					{
						mindMarkList.MarkList[i] = mark;
					}
					else
					{
						mindMarkList.MarkList.RemoveAt(i);
					}
				}
				int newMarkCount = mindMarkList.MarkList.Count;
				combatChar.AddInfinityMindMarkProgress(context, oldMarkCount - newMarkCount);
				combatChar.SetMindMarkTime(mindMarkList, context);
				combatChar.GetDefeatMarkCollection().SyncMindMark(context, combatChar);
			}
		}

		// Token: 0x060061E2 RID: 25058 RVA: 0x00379EDC File Offset: 0x003780DC
		private static void TimeUpdateAutoHeal(DataContext context, CombatCharacter combatChar)
		{
			CombatCharacterStateBase.<>c__DisplayClass23_0 CS$<>8__locals1;
			CS$<>8__locals1.bodyPart2Delta = ObjectPool<Dictionary<sbyte, OuterAndInnerInts>>.Instance.Get();
			CS$<>8__locals1.injuries = combatChar.GetInjuries();
			CS$<>8__locals1.oldInjuries = combatChar.GetOldInjuries();
			CS$<>8__locals1.injuriesChanged = false;
			CS$<>8__locals1.oldInjuriesChanged = false;
			int outerSpeed = (int)combatChar.OuterInjuryAutoHealSpeeds.Max<short>();
			int innerSpeed = (int)combatChar.InnerInjuryAutoHealSpeeds.Max<short>();
			outerSpeed = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 188, outerSpeed, -1, -1, -1, 0, 0, 0, 0);
			innerSpeed = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 188, innerSpeed, -1, -1, -1, 0, 0, 0, 0);
			InjuryAutoHealCollection autoHealCollection = combatChar.GetInjuryAutoHealCollection();
			bool flag = autoHealCollection.UpdateProgress(CS$<>8__locals1.bodyPart2Delta, outerSpeed, innerSpeed);
			if (flag)
			{
				combatChar.SetInjuryAutoHealCollection(autoHealCollection, context);
				CombatCharacterStateBase.<TimeUpdateAutoHeal>g__ApplyDeltas|23_0(false, ref CS$<>8__locals1);
			}
			short oldOuterSpeed = combatChar.OuterOldInjuryAutoHealSpeeds.Max<short>();
			short oldInnerSpeed = combatChar.InnerOldInjuryAutoHealSpeeds.Max<short>();
			InjuryAutoHealCollection oldAutoHealCollection = combatChar.GetOldInjuryAutoHealCollection();
			bool flag2 = oldAutoHealCollection.UpdateProgress(CS$<>8__locals1.bodyPart2Delta, (int)oldOuterSpeed, (int)oldInnerSpeed);
			if (flag2)
			{
				combatChar.SetOldInjuryAutoHealCollection(oldAutoHealCollection, context);
				CombatCharacterStateBase.<TimeUpdateAutoHeal>g__ApplyDeltas|23_0(true, ref CS$<>8__locals1);
			}
			ObjectPool<Dictionary<sbyte, OuterAndInnerInts>>.Instance.Return(CS$<>8__locals1.bodyPart2Delta);
			bool oldInjuriesChanged = CS$<>8__locals1.oldInjuriesChanged;
			if (oldInjuriesChanged)
			{
				combatChar.SetOldInjuries(CS$<>8__locals1.oldInjuries, context);
			}
			bool injuriesChanged = CS$<>8__locals1.injuriesChanged;
			if (injuriesChanged)
			{
				DomainManager.Combat.SetInjuries(context, combatChar, CS$<>8__locals1.injuries, true, false);
			}
		}

		// Token: 0x060061E3 RID: 25059 RVA: 0x0037A044 File Offset: 0x00378244
		private unsafe static void TimeUpdateNeiliAllocation(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.TickNeiliAllocationCd(context);
			if (!flag)
			{
				NeiliAllocation neiliAllocation = combatChar.GetNeiliAllocation();
				NeiliAllocation originNeiliAllocation = combatChar.GetOriginNeiliAllocation();
				int recoveryOfQiDisorder = (int)combatChar.GetCharacter().GetRecoveryOfQiDisorder();
				for (byte type = 0; type < 4; type += 1)
				{
					int currValue = (int)(*neiliAllocation[(int)type]);
					int originValue = (int)(*originNeiliAllocation[(int)type]);
					bool flag2 = currValue == originValue;
					if (!flag2)
					{
						GameData.Domains.Character.Character charObj = combatChar.GetCharacter();
						int costNeili = 0;
						bool flag3 = currValue < originValue;
						if (flag3)
						{
							sbyte qiDisorderLevel = DisorderLevelOfQi.GetDisorderLevelOfQi(charObj.GetDisorderOfQi());
							costNeili = CombatHelper.CalcNeiliCostInCombat((short)currValue, qiDisorderLevel);
							bool flag4 = charObj.GetCurrNeili() < costNeili;
							if (flag4)
							{
								combatChar.SetNeiliAllocationRecoverProgress(context, type, 0);
								goto IL_18D;
							}
						}
						int totalProgress = (currValue > originValue) ? GlobalConfig.Instance.CombatNeiliAllocationAutoReduceTotalProgress : GlobalConfig.Instance.CombatNeiliAllocationAutoAddTotalProgress;
						combatChar.NeiliAllocationAutoRecoverProgress[(int)type] += CFormula.CalcNeiliAllocationAutoRecoverProgress(recoveryOfQiDisorder, currValue, originValue);
						bool flag5 = combatChar.NeiliAllocationAutoRecoverProgress[(int)type] >= totalProgress;
						if (flag5)
						{
							combatChar.NeiliAllocationAutoRecoverProgress[(int)type] = 0;
							combatChar.SetNeiliAllocationRecoverProgress(context, type, 0);
							bool flag6 = currValue > originValue;
							if (flag6)
							{
								combatChar.ChangeNeiliAllocation(context, type, -1, false, true);
							}
							else
							{
								combatChar.ChangeNeiliAllocation(context, type, 1, false, true);
								charObj.ChangeCurrNeili(context, -costNeili);
							}
						}
						else
						{
							short recoverPercent = (short)((currValue > originValue) ? ((totalProgress - combatChar.NeiliAllocationAutoRecoverProgress[(int)type]) * 100 / totalProgress) : (combatChar.NeiliAllocationAutoRecoverProgress[(int)type] * 100 / totalProgress));
							combatChar.SetNeiliAllocationRecoverProgress(context, type, recoverPercent);
						}
					}
					IL_18D:;
				}
			}
		}

		// Token: 0x060061E4 RID: 25060 RVA: 0x0037A1F4 File Offset: 0x003783F4
		private static void TimeUpdateMain(DataContext context, CombatCharacter combatChar, bool isMainOrCurrChar)
		{
			bool flag = !isMainOrCurrChar;
			if (!flag)
			{
				CombatCharacterStateBase.TimeUpdateMainAgile(context, combatChar);
				CombatCharacterStateBase.TimeUpdateNormalAttackRecovery(context, combatChar);
				DomainManager.Combat.UpdateWeaponCd(context, combatChar);
				DomainManager.Combat.UpdateSkillCd(context, combatChar);
				CombatCharacterStateBase.TimeUpdateMainDefend(context, combatChar);
				CombatCharacterStateBase.TimeUpdateMainPoison(combatChar);
				CombatCharacterStateBase.TimeUpdateMainTeammateCommand(context, combatChar);
			}
		}

		// Token: 0x060061E5 RID: 25061 RVA: 0x0037A250 File Offset: 0x00378450
		private static void TimeUpdateMainAgile(DataContext context, CombatCharacter combatChar)
		{
			short moveSkillId = combatChar.GetAffectingMoveSkillId();
			bool flag = moveSkillId < 0;
			if (!flag)
			{
				int costMobility = DomainManager.Combat.GetSkillCostMobilityPerFrame(combatChar, moveSkillId);
				DomainManager.Combat.ChangeMobilityValue(context, combatChar, -costMobility, false, null, false);
				bool flag2 = combatChar.GetMobilityValue() <= 0;
				if (flag2)
				{
					combatChar.SetAffectingMoveSkillId(-1, context);
				}
				else
				{
					int addMobility = CombatSkill.Instance[moveSkillId].MobilityAddSpeed;
					DomainManager.Combat.ChangeMobilityValue(context, combatChar, addMobility, false, null, false);
				}
			}
		}

		// Token: 0x060061E6 RID: 25062 RVA: 0x0037A2D0 File Offset: 0x003784D0
		private static void TimeUpdateNormalAttackRecovery(DataContext context, CombatCharacter combatChar)
		{
			SilenceFrameData recoveryData = combatChar.GetNormalAttackRecovery();
			bool flag = recoveryData.Tick(1);
			if (flag)
			{
				combatChar.SetNormalAttackRecovery(recoveryData, context);
			}
		}

		// Token: 0x060061E7 RID: 25063 RVA: 0x0037A2FC File Offset: 0x003784FC
		private static void TimeUpdateMainDefend(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.GetAffectingDefendSkillId() < 0;
			if (!flag)
			{
				combatChar.DefendSkillLeftFrame -= 1;
				bool flag2 = combatChar.DefendSkillLeftFrame > 0;
				if (flag2)
				{
					byte percent = (byte)(combatChar.DefendSkillLeftFrame * 100 / combatChar.DefendSkillTotalFrame);
					bool flag3 = percent != combatChar.GetDefendSkillTimePercent();
					if (flag3)
					{
						combatChar.SetDefendSkillTimePercent(percent, context);
					}
				}
				else
				{
					combatChar.SetAffectingDefendSkillId(-1, context);
					DomainManager.Combat.SetProperLoopAniAndParticle(context, combatChar, false);
					DomainManager.Combat.UpdateSkillCanUse(context, combatChar);
				}
			}
		}

		// Token: 0x060061E8 RID: 25064 RVA: 0x0037A38C File Offset: 0x0037858C
		private static void TimeUpdateMainPoison(CombatCharacter combatChar)
		{
			bool flag = combatChar.PoisonOverflow(4);
			if (flag)
			{
				combatChar.AddPoisonAffectValue(4, 1, false);
			}
			bool flag2 = combatChar.PoisonOverflow(5);
			if (flag2)
			{
				combatChar.AddPoisonAffectValue(5, 1, false);
			}
		}

		// Token: 0x060061E9 RID: 25065 RVA: 0x0037A3C4 File Offset: 0x003785C4
		private static void TimeUpdateMainTeammateCommand(DataContext context, CombatCharacter combatChar)
		{
			bool flag = !DomainManager.Combat.IsMainCharacter(combatChar);
			if (!flag)
			{
				int[] charList = DomainManager.Combat.GetCharacterList(combatChar.IsAlly);
				for (int i = 1; i < charList.Length; i++)
				{
					int teammateId = charList[i];
					bool flag2 = teammateId < 0;
					if (!flag2)
					{
						CombatCharacter teammate = DomainManager.Combat.GetElement_CombatCharacterDict(teammateId);
						bool flag3 = DomainManager.Combat.IsCharacterFallen(teammate);
						if (!flag3)
						{
							List<sbyte> cmdList = teammate.GetCurrTeammateCommands();
							List<byte> cdPercentList = teammate.GetTeammateCommandCdPercent();
							for (int j = 0; j < cmdList.Count; j++)
							{
								bool flag4 = cmdList[j] < 0 || teammate.TeammateCommandCdCurrentCount[j] >= teammate.TeammateCommandCdTotalCount[j] || combatChar.TeammateBeforeMainChar == teammateId || combatChar.TeammateAfterMainChar == teammateId;
								if (!flag4)
								{
									teammate.TeammateCommandCdCurrentCount[j] += teammate.GetTeammateCommandCdSpeed(cmdList[j]);
									byte cdPercent = (byte)(100 - Math.Min(teammate.TeammateCommandCdCurrentCount[j] * 100 / teammate.TeammateCommandCdTotalCount[j], 100));
									bool flag5 = cdPercent != cdPercentList[j];
									if (flag5)
									{
										cdPercentList[j] = cdPercent;
										teammate.SetTeammateCommandCdPercent(cdPercentList, context);
										bool flag6 = cdPercent == 0;
										if (flag6)
										{
											DomainManager.Combat.UpdateTeammateCommandUsable(context, teammate, cmdList[j]);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060061EA RID: 25066 RVA: 0x0037A560 File Offset: 0x00378760
		[CompilerGenerated]
		internal static void <TimeUpdateAutoHeal>g__ApplyDeltas|23_0(bool changeOld, ref CombatCharacterStateBase.<>c__DisplayClass23_0 A_1)
		{
			foreach (KeyValuePair<sbyte, OuterAndInnerInts> keyValuePair in A_1.bodyPart2Delta)
			{
				sbyte b;
				OuterAndInnerInts outerAndInnerInts;
				keyValuePair.Deconstruct(out b, out outerAndInnerInts);
				sbyte bodyPart = b;
				OuterAndInnerInts delta = outerAndInnerInts;
				bool flag = delta.Outer <= 0 && delta.Inner <= 0;
				if (!flag)
				{
					A_1.injuriesChanged = true;
					A_1.injuries.Change(bodyPart, false, (int)((sbyte)(-(sbyte)delta.Outer)));
					A_1.injuries.Change(bodyPart, true, (int)((sbyte)(-(sbyte)delta.Inner)));
					bool flag2 = !changeOld;
					if (!flag2)
					{
						A_1.oldInjuriesChanged = true;
						A_1.oldInjuries.Change(bodyPart, false, (int)((sbyte)(-(sbyte)delta.Outer)));
						A_1.oldInjuries.Change(bodyPart, true, (int)((sbyte)(-(sbyte)delta.Inner)));
					}
				}
			}
		}

		// Token: 0x04001A91 RID: 6801
		protected CombatDomain CurrentCombatDomain;

		// Token: 0x04001A92 RID: 6802
		protected CombatCharacter CombatChar;

		// Token: 0x04001A93 RID: 6803
		public CombatCharacterStateType StateType;

		// Token: 0x04001A94 RID: 6804
		public bool RequireDelayFallen;

		// Token: 0x04001A95 RID: 6805
		public bool IsUpdateOnPause;

		// Token: 0x04001A96 RID: 6806
		protected bool AutoUpdateDelayCall;

		// Token: 0x04001A97 RID: 6807
		private readonly Queue<CombatCharacterStateBase.DelayCallData> _delayCallList = new Queue<CombatCharacterStateBase.DelayCallData>();

		// Token: 0x02000B48 RID: 2888
		// (Invoke) Token: 0x06008AAC RID: 35500
		public delegate void CombatCharacterStateDelayCallRequest();

		// Token: 0x02000B49 RID: 2889
		// (Invoke) Token: 0x06008AB0 RID: 35504
		public delegate void CombatCharacterStateDelayCallTickPercent(int percent);

		// Token: 0x02000B4A RID: 2890
		public struct DelayCallData
		{
			// Token: 0x170005D6 RID: 1494
			// (get) Token: 0x06008AB3 RID: 35507 RVA: 0x004F25CB File Offset: 0x004F07CB
			public int Percent
			{
				get
				{
					return CValuePercent.ParseIntClamp01(this.DelayedFrames, this.TotalDelayFrames);
				}
			}

			// Token: 0x170005D7 RID: 1495
			// (get) Token: 0x06008AB4 RID: 35508 RVA: 0x004F25DE File Offset: 0x004F07DE
			public bool Ticked
			{
				get
				{
					return this.DelayedFrames >= this.TotalDelayFrames;
				}
			}

			// Token: 0x06008AB5 RID: 35509 RVA: 0x004F25F1 File Offset: 0x004F07F1
			public DelayCallData(CombatCharacterStateBase.CombatCharacterStateDelayCallRequest action, CombatCharacterStateBase.CombatCharacterStateDelayCallTickPercent tickPercent, int frames)
			{
				this.DelayedFrames = 0;
				this.TotalDelayFrames = frames;
				this._action = action;
				this._tickPercent = tickPercent;
			}

			// Token: 0x06008AB6 RID: 35510 RVA: 0x004F2610 File Offset: 0x004F0810
			public bool Tick()
			{
				this.DelayedFrames++;
				CombatCharacterStateBase.CombatCharacterStateDelayCallTickPercent tickPercent = this._tickPercent;
				if (tickPercent != null)
				{
					tickPercent(this.Percent);
				}
				bool ticked = this.Ticked;
				if (ticked)
				{
					CombatCharacterStateBase.CombatCharacterStateDelayCallRequest action = this._action;
					if (action != null)
					{
						action();
					}
				}
				return this.Ticked;
			}

			// Token: 0x04002FBD RID: 12221
			public int DelayedFrames;

			// Token: 0x04002FBE RID: 12222
			public int TotalDelayFrames;

			// Token: 0x04002FBF RID: 12223
			private readonly CombatCharacterStateBase.CombatCharacterStateDelayCallRequest _action;

			// Token: 0x04002FC0 RID: 12224
			private readonly CombatCharacterStateBase.CombatCharacterStateDelayCallTickPercent _tickPercent;
		}
	}
}
