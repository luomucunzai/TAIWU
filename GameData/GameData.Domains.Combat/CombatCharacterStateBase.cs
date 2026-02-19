using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class CombatCharacterStateBase
{
	public delegate void CombatCharacterStateDelayCallRequest();

	public delegate void CombatCharacterStateDelayCallTickPercent(int percent);

	public struct DelayCallData
	{
		public int DelayedFrames;

		public int TotalDelayFrames;

		private readonly CombatCharacterStateDelayCallRequest _action;

		private readonly CombatCharacterStateDelayCallTickPercent _tickPercent;

		public int Percent => CValuePercent.ParseIntClamp01(DelayedFrames, TotalDelayFrames);

		public bool Ticked => DelayedFrames >= TotalDelayFrames;

		public DelayCallData(CombatCharacterStateDelayCallRequest action, CombatCharacterStateDelayCallTickPercent tickPercent, int frames)
		{
			DelayedFrames = 0;
			TotalDelayFrames = frames;
			_action = action;
			_tickPercent = tickPercent;
		}

		public bool Tick()
		{
			DelayedFrames++;
			_tickPercent?.Invoke(Percent);
			if (Ticked)
			{
				_action?.Invoke();
			}
			return Ticked;
		}
	}

	protected CombatDomain CurrentCombatDomain;

	protected CombatCharacter CombatChar;

	public CombatCharacterStateType StateType;

	public bool RequireDelayFallen;

	public bool IsUpdateOnPause;

	protected bool AutoUpdateDelayCall;

	private readonly Queue<DelayCallData> _delayCallList = new Queue<DelayCallData>();

	protected void DelayCall(CombatCharacterStateDelayCallRequest request, int frame)
	{
		DelayCall(request, null, frame);
	}

	protected void DelayCall(CombatCharacterStateDelayCallRequest request, CombatCharacterStateDelayCallTickPercent tickPercent, int frame)
	{
		if (frame <= 0)
		{
			request?.Invoke();
			tickPercent?.Invoke(100);
		}
		else
		{
			_delayCallList.Enqueue(new DelayCallData(request, tickPercent, frame));
		}
	}

	public CombatCharacterStateBase(CombatDomain combatDomain, CombatCharacter combatChar, CombatCharacterStateType type)
	{
		CurrentCombatDomain = combatDomain;
		CombatChar = combatChar;
		StateType = type;
		IsUpdateOnPause = false;
	}

	public virtual void OnEnter()
	{
		AutoUpdateDelayCall = true;
		_delayCallList.Clear();
	}

	public virtual void OnExit()
	{
	}

	public virtual bool OnUpdate()
	{
		if (CombatChar.ChangeCharId >= 0 && !CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly).NeedSelectMercyOption)
		{
			ChangeCurrChar();
			return false;
		}
		if (CheckCommonTranslateState())
		{
			CombatChar.StateMachine.TranslateState();
			return false;
		}
		if (!DomainManager.Combat.Pause)
		{
			DataContext dataContext = CombatChar.GetDataContext();
			int[] characterList = CurrentCombatDomain.GetCharacterList(CombatChar.IsAlly);
			int num = (CombatChar.IsAlly ? CurrentCombatDomain.GetSelfTeam() : CurrentCombatDomain.GetEnemyTeam())[0];
			TimeUpdate(dataContext, CombatChar);
			if (CurrentCombatDomain.IsMainCharacter(CombatChar))
			{
				if (CombatChar.ExecuteReserveTeammateCommand(dataContext) || CombatChar.UpdateTeammateCharStatus(dataContext))
				{
					return false;
				}
			}
			else
			{
				CombatCharacter element_CombatCharacterDict = CurrentCombatDomain.GetElement_CombatCharacterDict(num);
				TimeUpdate(dataContext, element_CombatCharacterDict);
				if (CombatChar.TeammateCommandLeftFrame > 0)
				{
					CombatChar.ReduceTeammateCommandLeftTime(dataContext);
				}
				if ((CombatChar.TeammateCommandLeftFrame == 0 && CombatChar.GetPreparingSkillId() < 0) || CurrentCombatDomain.IsCharacterFallen(element_CombatCharacterDict))
				{
					CombatChar.ResetTeammateCommandCd(dataContext, CombatChar.ExecutingTeammateCommandIndex, -1, checkEvent: true, displayEvent: true);
					CombatChar.TeammateCommandLeftFrame = -1;
					CombatChar.ExecutingTeammateCommandIndex = -1;
					CombatChar.ChangeCharId = num;
				}
			}
			for (int i = 1; i < characterList.Length; i++)
			{
				int num2 = characterList[i];
				if (num2 >= 0 && num2 != CombatChar.GetId() && num2 != num)
				{
					CombatCharacter element_CombatCharacterDict2 = CurrentCombatDomain.GetElement_CombatCharacterDict(num2);
					if (!CurrentCombatDomain.IsCharacterFallen(element_CombatCharacterDict2))
					{
						TimeUpdate(dataContext, element_CombatCharacterDict2, isMainOrCurrChar: false);
					}
				}
			}
			if (CombatChar.IsMoving)
			{
				CombatChar.MoveData.UpdateMove(dataContext, CurrentCombatDomain);
			}
			else if (CombatChar.KeepMoving && CurrentCombatDomain.CanMove(CombatChar, CombatChar.MoveForward))
			{
				if (CombatChar.MoveData.IsJumpMove(CombatChar.MoveForward))
				{
					CombatChar.MoveData.UpdateJumpPrepare(dataContext);
				}
				else
				{
					CombatChar.MoveData.StartMove(dataContext);
				}
			}
			else if (!CombatChar.KeepMoving && (CombatChar.MoveData.JumpPreparedProgress > 0 || CombatChar.GetJumpPreparedDistance() > 0))
			{
				CombatChar.MoveData.ReduceJumpPrepare(dataContext);
			}
		}
		if (AutoUpdateDelayCall)
		{
			UpdateDelayCall();
		}
		return true;
	}

	protected void UpdateDelayCall()
	{
		int num = _delayCallList.Count;
		while (num > 0)
		{
			num--;
			DelayCallData item = _delayCallList.Dequeue();
			if (!item.Tick())
			{
				_delayCallList.Enqueue(item);
			}
		}
	}

	private void ChangeCurrChar()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CombatCharacter element_CombatCharacterDict = CurrentCombatDomain.GetElement_CombatCharacterDict(CombatChar.ChangeCharId);
		CombatChar.ClearAllDoingOrReserveCommand(dataContext);
		CombatChar.SetAffectingDefendSkillId(-1, dataContext);
		CombatChar.SetAffectingMoveSkillId(-1, dataContext);
		CombatChar.SetAnimationToLoop(null, dataContext);
		CombatChar.SetParticleToLoop(null, dataContext);
		if (CombatChar.ChangeCharFailAni != null)
		{
			CombatChar.SetAnimationToPlayOnce(CombatChar.ChangeCharFailAni, dataContext);
			if (CombatChar.ChangeCharFailParticle != "")
			{
				CombatChar.SetParticleToPlay(CombatChar.ChangeCharFailParticle, dataContext);
			}
			if (CombatChar.ChangeCharFailSound != "")
			{
				CombatChar.SetDieSoundToPlay(CombatChar.ChangeCharFailSound, dataContext);
			}
		}
		else
		{
			CombatChar.SetAnimationToPlayOnce("M_004", dataContext);
			CombatChar.SetParticleToPlay(null, dataContext);
		}
		CurrentCombatDomain.SetCombatCharacter(dataContext, CombatChar.IsAlly, CombatChar.ChangeCharId);
		if (!CurrentCombatDomain.IsMainCharacter(CombatChar))
		{
			element_CombatCharacterDict.TeammateHasCommand[CurrentCombatDomain.GetCharacterList(CombatChar.IsAlly).IndexOf(CombatChar.GetId()) - 1] = false;
		}
		if (CombatChar.ExecutingTeammateCommandChangeDistance != 0)
		{
			CurrentCombatDomain.ChangeDistance(dataContext, CombatChar, CombatChar.ExecutingTeammateCommandChangeDistance);
			CombatChar.ExecutingTeammateCommandChangeDistance = 0;
		}
		CombatChar.SetExecutingTeammateCommand(-1, dataContext);
		CombatChar.SetTeammateCommandTimePercent(0, dataContext);
		CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Idle);
		CombatChar.ChangeCharId = -1;
		CombatChar.ClearAllSound(dataContext);
		element_CombatCharacterDict.SetCurrentPosition(CombatChar.GetCurrentPosition(), dataContext);
		CurrentCombatDomain.SetDisplayPosition(dataContext, CombatChar.IsAlly, int.MinValue);
		element_CombatCharacterDict.StateMachine.TranslateState(CombatCharacterStateType.ChangeCharacter);
	}

	private bool CheckCommonTranslateState()
	{
		CombatCharacterStateType properState = CombatChar.StateMachine.GetProperState();
		if (StateType == CombatCharacterStateType.Idle)
		{
			if (!CombatChar.IsMoving && properState != StateType)
			{
				return true;
			}
			if (CombatChar.IsMoving && properState == CombatCharacterStateType.PrepareAttack)
			{
				return true;
			}
		}
		if (StateType == CombatCharacterStateType.PrepareAttack)
		{
			return properState == CombatCharacterStateType.ChangeBossPhase;
		}
		return false;
	}

	private void TimeUpdate(DataContext context, CombatCharacter combatChar, bool isMainOrCurrChar = true)
	{
		TimeUpdateRecoverStandard(context, combatChar, isMainOrCurrChar);
		TimeUpdateFlawAcupoint(context, combatChar);
		TimeUpdateMindMark(context, combatChar);
		TimeUpdateAutoHeal(context, combatChar);
		TimeUpdateNeiliAllocation(context, combatChar);
		TimeUpdateMain(context, combatChar, isMainOrCurrChar);
	}

	private static void TimeUpdateRecoverStandard(DataContext context, CombatCharacter combatChar, bool isMainOrCurrChar)
	{
		bool flag = combatChar.GetPreparingSkillId() >= 0;
		if (!isMainOrCurrChar || combatChar.GetPreparingOtherAction() != -1)
		{
			return;
		}
		CombatDomain combat = DomainManager.Combat;
		if (combatChar.GetBreathValue() < combatChar.GetMaxBreathValue() && !flag)
		{
			combat.RecoverBreathValue(context, combatChar);
			if (combat.IsCurrentCombatCharacter(combatChar))
			{
				combat.UpdateSkillCostBreathStanceCanUse(context, combatChar);
			}
		}
		if (combatChar.GetAffectingMoveSkillId() < 0 && (!flag || combatChar.MoveData.CanMoveForwardInSkillPrepareDist > 0 || combatChar.MoveData.CanMoveBackwardInSkillPrepareDist > 0))
		{
			combat.RecoverMobilityValue(context, combatChar);
		}
		ItemKey[] weapons = combatChar.GetWeapons();
		for (int i = 0; i < 3; i++)
		{
			if (weapons[i].IsValid())
			{
				int recoverUnlockAttackValue = combatChar.GetRecoverUnlockAttackValue(weapons[i]);
				if (recoverUnlockAttackValue > 0)
				{
					combatChar.ChangeUnlockAttackValue(context, i, recoverUnlockAttackValue);
				}
			}
		}
	}

	private static void TimeUpdateFlawAcupoint(DataContext context, CombatCharacter combatChar)
	{
		CombatDomain combat = DomainManager.Combat;
		FlawOrAcupointCollection flawCollection = combatChar.GetFlawCollection();
		FlawOrAcupointCollection acupointCollection = combatChar.GetAcupointCollection();
		byte[] flawCount = combatChar.GetFlawCount();
		byte[] acupointCount = combatChar.GetAcupointCount();
		short recoveryOfFlaw = combatChar.GetRecoveryOfFlaw();
		short recoveryOfAcupoint = combatChar.GetRecoveryOfAcupoint();
		FlawOrAcupointCollection.ReduceKeepTimeResult reduceKeepTimeResult = flawCollection.ReduceKeepTime(combatChar, recoveryOfFlaw, flawCount, isFlaw: true);
		FlawOrAcupointCollection.ReduceKeepTimeResult reduceKeepTimeResult2 = acupointCollection.ReduceKeepTime(combatChar, recoveryOfAcupoint, acupointCount, isFlaw: false);
		if (reduceKeepTimeResult.DataChanged)
		{
			combatChar.SetFlawCollection(flawCollection, context);
		}
		if (reduceKeepTimeResult2.DataChanged)
		{
			combatChar.SetAcupointCollection(acupointCollection, context);
		}
		if (reduceKeepTimeResult.CountChanged || reduceKeepTimeResult2.CountChanged)
		{
			if (reduceKeepTimeResult.CountChanged)
			{
				combatChar.SetFlawCount(combatChar.GetFlawCount(), context);
				if (combat.IsMainCharacter(combatChar))
				{
					combat.UpdateAllTeammateCommandUsable(context, combatChar.IsAlly, ETeammateCommandImplement.HealFlaw);
				}
			}
			if (reduceKeepTimeResult2.CountChanged)
			{
				combatChar.SetAcupointCount(combatChar.GetAcupointCount(), context);
				if (combat.IsMainCharacter(combatChar))
				{
					combat.UpdateAllTeammateCommandUsable(context, combatChar.IsAlly, ETeammateCommandImplement.HealFlaw);
				}
			}
			combat.UpdateBodyDefeatMark(context, combatChar);
		}
		for (int i = 0; i < reduceKeepTimeResult.RemovedList.Count; i++)
		{
			(sbyte, sbyte) tuple = reduceKeepTimeResult.RemovedList[i];
			Events.RaiseFlawRemoved(context, combatChar, tuple.Item1, tuple.Item2);
		}
		for (int j = 0; j < reduceKeepTimeResult2.RemovedList.Count; j++)
		{
			(sbyte, sbyte) tuple2 = reduceKeepTimeResult2.RemovedList[j];
			Events.RaiseAcuPointRemoved(context, combatChar, tuple2.Item1, tuple2.Item2);
		}
	}

	private static void TimeUpdateMindMark(DataContext context, CombatCharacter combatChar)
	{
		MindMarkList mindMarkTime = combatChar.GetMindMarkTime();
		List<SilenceFrameData> markList = mindMarkTime.MarkList;
		if (markList == null || markList.Count <= 0)
		{
			return;
		}
		int deltaFrame = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 187, 1);
		int count = mindMarkTime.MarkList.Count;
		for (int num = mindMarkTime.MarkList.Count - 1; num >= 0; num--)
		{
			SilenceFrameData value = mindMarkTime.MarkList[num];
			value.Tick(deltaFrame);
			if (value.Silencing)
			{
				mindMarkTime.MarkList[num] = value;
			}
			else
			{
				mindMarkTime.MarkList.RemoveAt(num);
			}
		}
		int count2 = mindMarkTime.MarkList.Count;
		combatChar.AddInfinityMindMarkProgress(context, count - count2);
		combatChar.SetMindMarkTime(mindMarkTime, context);
		combatChar.GetDefeatMarkCollection().SyncMindMark(context, combatChar);
	}

	private static void TimeUpdateAutoHeal(DataContext context, CombatCharacter combatChar)
	{
		Dictionary<sbyte, OuterAndInnerInts> bodyPart2Delta = ObjectPool<Dictionary<sbyte, OuterAndInnerInts>>.Instance.Get();
		Injuries injuries = combatChar.GetInjuries();
		Injuries oldInjuries = combatChar.GetOldInjuries();
		bool injuriesChanged = false;
		bool oldInjuriesChanged = false;
		int value = combatChar.OuterInjuryAutoHealSpeeds.Max();
		int value2 = combatChar.InnerInjuryAutoHealSpeeds.Max();
		value = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 188, value);
		value2 = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 188, value2);
		InjuryAutoHealCollection injuryAutoHealCollection = combatChar.GetInjuryAutoHealCollection();
		if (injuryAutoHealCollection.UpdateProgress(bodyPart2Delta, value, value2))
		{
			combatChar.SetInjuryAutoHealCollection(injuryAutoHealCollection, context);
			ApplyDeltas(changeOld: false);
		}
		short outerSpeed = combatChar.OuterOldInjuryAutoHealSpeeds.Max();
		short innerSpeed = combatChar.InnerOldInjuryAutoHealSpeeds.Max();
		InjuryAutoHealCollection oldInjuryAutoHealCollection = combatChar.GetOldInjuryAutoHealCollection();
		if (oldInjuryAutoHealCollection.UpdateProgress(bodyPart2Delta, outerSpeed, innerSpeed))
		{
			combatChar.SetOldInjuryAutoHealCollection(oldInjuryAutoHealCollection, context);
			ApplyDeltas(changeOld: true);
		}
		ObjectPool<Dictionary<sbyte, OuterAndInnerInts>>.Instance.Return(bodyPart2Delta);
		if (oldInjuriesChanged)
		{
			combatChar.SetOldInjuries(oldInjuries, context);
		}
		if (injuriesChanged)
		{
			DomainManager.Combat.SetInjuries(context, combatChar, injuries, updateDefeatMark: true, syncAutoHealProgress: false);
		}
		void ApplyDeltas(bool changeOld)
		{
			foreach (var (bodyPartType, outerAndInnerInts2) in bodyPart2Delta)
			{
				if (outerAndInnerInts2.Outer > 0 || outerAndInnerInts2.Inner > 0)
				{
					injuriesChanged = true;
					injuries.Change(bodyPartType, isInnerInjury: false, (sbyte)(-outerAndInnerInts2.Outer));
					injuries.Change(bodyPartType, isInnerInjury: true, (sbyte)(-outerAndInnerInts2.Inner));
					if (changeOld)
					{
						oldInjuriesChanged = true;
						oldInjuries.Change(bodyPartType, isInnerInjury: false, (sbyte)(-outerAndInnerInts2.Outer));
						oldInjuries.Change(bodyPartType, isInnerInjury: true, (sbyte)(-outerAndInnerInts2.Inner));
					}
				}
			}
		}
	}

	private static void TimeUpdateNeiliAllocation(DataContext context, CombatCharacter combatChar)
	{
		if (combatChar.TickNeiliAllocationCd(context))
		{
			return;
		}
		NeiliAllocation neiliAllocation = combatChar.GetNeiliAllocation();
		NeiliAllocation originNeiliAllocation = combatChar.GetOriginNeiliAllocation();
		int recoveryOfQiDisorder = combatChar.GetCharacter().GetRecoveryOfQiDisorder();
		for (byte b = 0; b < 4; b++)
		{
			int num = neiliAllocation[b];
			int num2 = originNeiliAllocation[b];
			if (num == num2)
			{
				continue;
			}
			GameData.Domains.Character.Character character = combatChar.GetCharacter();
			int num3 = 0;
			if (num < num2)
			{
				sbyte disorderLevelOfQi = DisorderLevelOfQi.GetDisorderLevelOfQi(character.GetDisorderOfQi());
				num3 = CombatHelper.CalcNeiliCostInCombat((short)num, disorderLevelOfQi);
				if (character.GetCurrNeili() < num3)
				{
					combatChar.SetNeiliAllocationRecoverProgress(context, b, 0);
					continue;
				}
			}
			int num4 = ((num > num2) ? GlobalConfig.Instance.CombatNeiliAllocationAutoReduceTotalProgress : GlobalConfig.Instance.CombatNeiliAllocationAutoAddTotalProgress);
			combatChar.NeiliAllocationAutoRecoverProgress[b] += CFormula.CalcNeiliAllocationAutoRecoverProgress(recoveryOfQiDisorder, num, num2);
			if (combatChar.NeiliAllocationAutoRecoverProgress[b] >= num4)
			{
				combatChar.NeiliAllocationAutoRecoverProgress[b] = 0;
				combatChar.SetNeiliAllocationRecoverProgress(context, b, 0);
				if (num > num2)
				{
					combatChar.ChangeNeiliAllocation(context, b, -1, applySpecialEffect: false);
					continue;
				}
				combatChar.ChangeNeiliAllocation(context, b, 1, applySpecialEffect: false);
				character.ChangeCurrNeili(context, -num3);
			}
			else
			{
				short percent = (short)((num > num2) ? ((num4 - combatChar.NeiliAllocationAutoRecoverProgress[b]) * 100 / num4) : (combatChar.NeiliAllocationAutoRecoverProgress[b] * 100 / num4));
				combatChar.SetNeiliAllocationRecoverProgress(context, b, percent);
			}
		}
	}

	private static void TimeUpdateMain(DataContext context, CombatCharacter combatChar, bool isMainOrCurrChar)
	{
		if (isMainOrCurrChar)
		{
			TimeUpdateMainAgile(context, combatChar);
			TimeUpdateNormalAttackRecovery(context, combatChar);
			DomainManager.Combat.UpdateWeaponCd(context, combatChar);
			DomainManager.Combat.UpdateSkillCd(context, combatChar);
			TimeUpdateMainDefend(context, combatChar);
			TimeUpdateMainPoison(combatChar);
			TimeUpdateMainTeammateCommand(context, combatChar);
		}
	}

	private static void TimeUpdateMainAgile(DataContext context, CombatCharacter combatChar)
	{
		short affectingMoveSkillId = combatChar.GetAffectingMoveSkillId();
		if (affectingMoveSkillId >= 0)
		{
			int skillCostMobilityPerFrame = DomainManager.Combat.GetSkillCostMobilityPerFrame(combatChar, affectingMoveSkillId);
			DomainManager.Combat.ChangeMobilityValue(context, combatChar, -skillCostMobilityPerFrame);
			if (combatChar.GetMobilityValue() <= 0)
			{
				combatChar.SetAffectingMoveSkillId(-1, context);
				return;
			}
			int mobilityAddSpeed = Config.CombatSkill.Instance[affectingMoveSkillId].MobilityAddSpeed;
			DomainManager.Combat.ChangeMobilityValue(context, combatChar, mobilityAddSpeed);
		}
	}

	private static void TimeUpdateNormalAttackRecovery(DataContext context, CombatCharacter combatChar)
	{
		SilenceFrameData normalAttackRecovery = combatChar.GetNormalAttackRecovery();
		if (normalAttackRecovery.Tick())
		{
			combatChar.SetNormalAttackRecovery(normalAttackRecovery, context);
		}
	}

	private static void TimeUpdateMainDefend(DataContext context, CombatCharacter combatChar)
	{
		if (combatChar.GetAffectingDefendSkillId() < 0)
		{
			return;
		}
		combatChar.DefendSkillLeftFrame--;
		if (combatChar.DefendSkillLeftFrame > 0)
		{
			byte b = (byte)(combatChar.DefendSkillLeftFrame * 100 / combatChar.DefendSkillTotalFrame);
			if (b != combatChar.GetDefendSkillTimePercent())
			{
				combatChar.SetDefendSkillTimePercent(b, context);
			}
		}
		else
		{
			combatChar.SetAffectingDefendSkillId(-1, context);
			DomainManager.Combat.SetProperLoopAniAndParticle(context, combatChar);
			DomainManager.Combat.UpdateSkillCanUse(context, combatChar);
		}
	}

	private static void TimeUpdateMainPoison(CombatCharacter combatChar)
	{
		if (combatChar.PoisonOverflow(4))
		{
			combatChar.AddPoisonAffectValue(4, 1);
		}
		if (combatChar.PoisonOverflow(5))
		{
			combatChar.AddPoisonAffectValue(5, 1);
		}
	}

	private static void TimeUpdateMainTeammateCommand(DataContext context, CombatCharacter combatChar)
	{
		if (!DomainManager.Combat.IsMainCharacter(combatChar))
		{
			return;
		}
		int[] characterList = DomainManager.Combat.GetCharacterList(combatChar.IsAlly);
		for (int i = 1; i < characterList.Length; i++)
		{
			int num = characterList[i];
			if (num < 0)
			{
				continue;
			}
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(num);
			if (DomainManager.Combat.IsCharacterFallen(element_CombatCharacterDict))
			{
				continue;
			}
			List<sbyte> currTeammateCommands = element_CombatCharacterDict.GetCurrTeammateCommands();
			List<byte> teammateCommandCdPercent = element_CombatCharacterDict.GetTeammateCommandCdPercent();
			for (int j = 0; j < currTeammateCommands.Count; j++)
			{
				if (currTeammateCommands[j] < 0 || element_CombatCharacterDict.TeammateCommandCdCurrentCount[j] >= element_CombatCharacterDict.TeammateCommandCdTotalCount[j] || combatChar.TeammateBeforeMainChar == num || combatChar.TeammateAfterMainChar == num)
				{
					continue;
				}
				element_CombatCharacterDict.TeammateCommandCdCurrentCount[j] += element_CombatCharacterDict.GetTeammateCommandCdSpeed(currTeammateCommands[j]);
				byte b = (byte)(100 - Math.Min(element_CombatCharacterDict.TeammateCommandCdCurrentCount[j] * 100 / element_CombatCharacterDict.TeammateCommandCdTotalCount[j], 100));
				if (b != teammateCommandCdPercent[j])
				{
					teammateCommandCdPercent[j] = b;
					element_CombatCharacterDict.SetTeammateCommandCdPercent(teammateCommandCdPercent, context);
					if (b == 0)
					{
						DomainManager.Combat.UpdateTeammateCommandUsable(context, element_CombatCharacterDict, currTeammateCommands[j]);
					}
				}
			}
		}
	}
}
