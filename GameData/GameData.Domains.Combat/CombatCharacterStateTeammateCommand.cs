using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class CombatCharacterStateTeammateCommand : CombatCharacterStateBase
{
	private static readonly List<string> NeedPostfixAnis = new List<string> { "M_018", "M_019", "M_023" };

	private CombatCharacter _teammateChar;

	private sbyte _commandType;

	private TeammateCommandItem _commandConfig;

	private ETeammateCommandImplement _commandImplement;

	private string _backCharAni;

	private string _foreCharAni;

	private string _foreCharParticle;

	private string _foreCharSound;

	private int _foreCharAniStartFrame;

	private int _applyLogicEffectFrame;

	private int _teammateFallBackFrame;

	private int _clearCmdFrame;

	private int _stateLeftFrame;

	private CValuePercentBonus CmdEffectPercent => CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(CombatChar.GetId(), 184, (EDataModifyType)0, (int)_commandImplement, -1, -1, (EDataSumType)0));

	private bool Preparing => _teammateChar.TeammateCommandLeftPrepareFrame > 0;

	private bool NeedPrepare => _commandConfig.PrepareFrame > 0;

	private bool TeammateBeforeMainChar => _commandConfig.PosOffset > 0;

	public CombatCharacterStateTeammateCommand(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.TeammateCommand)
	{
		IsUpdateOnPause = true;
	}

	public override void OnEnter()
	{
		OnEnterInitFields();
		DataContext dataContext = _teammateChar.GetDataContext();
		if (Preparing)
		{
			OnEnterPreparing(dataContext);
		}
		else if (NeedPrepare)
		{
			OnEnterPrepare(dataContext);
		}
		else
		{
			OnEnterNoPrepare(dataContext);
		}
	}

	public override bool OnUpdate()
	{
		if (!base.OnUpdate())
		{
			return false;
		}
		if (_foreCharAniStartFrame > 0)
		{
			_foreCharAniStartFrame--;
			if (_foreCharAniStartFrame == 0)
			{
				OnForeCharAniStart();
			}
		}
		if (_applyLogicEffectFrame > 0)
		{
			_applyLogicEffectFrame--;
			if (_applyLogicEffectFrame == 0)
			{
				ApplyLogicEffect();
			}
		}
		if (_teammateFallBackFrame > 0)
		{
			_teammateFallBackFrame--;
			if (_teammateFallBackFrame == 0)
			{
				OnTeammateFallBack();
			}
		}
		if (_clearCmdFrame > 0)
		{
			_clearCmdFrame--;
			if (_clearCmdFrame == 0)
			{
				OnClearCmd();
			}
		}
		if (_stateLeftFrame > 0)
		{
			_stateLeftFrame--;
			if (_stateLeftFrame == 0)
			{
				OnStateLeft();
			}
		}
		return false;
	}

	private void OnEnterInitFields()
	{
		_teammateChar = CombatChar.ActingTeammateCommandChar;
		_commandType = _teammateChar.GetExecutingTeammateCommand();
		_commandConfig = TeammateCommand.Instance[_commandType];
		_commandImplement = _commandConfig.Implement;
		_backCharAni = (Preparing ? _commandConfig.BackCharEnterAni : null);
		_foreCharAni = _commandConfig.ForeCharAni1;
		_foreCharParticle = null;
		_foreCharSound = null;
		_foreCharAniStartFrame = 0;
		_applyLogicEffectFrame = 0;
		_teammateFallBackFrame = 0;
		_clearCmdFrame = 0;
		_stateLeftFrame = (Preparing ? AnimDataCollection.GetDurationFrame(_commandConfig.BackCharEnterAni) : 48);
		if (NeedPostfixAnis.Contains(_foreCharAni))
		{
			CombatWeaponData weaponData = (TeammateBeforeMainChar ? _teammateChar : CombatChar).GetWeaponData();
			_foreCharAni += weaponData.Template.TeammateCmdAniPostfix;
		}
		if ((!NeedPrepare || Preparing) && !TeammateBeforeMainChar)
		{
			if (Preparing || _commandConfig.ForeCharAniUseHit)
			{
				_foreCharAniStartFrame = AnimDataCollection.GetEventFrame(_commandConfig.BackCharEnterAni, "hit");
			}
			if (!string.IsNullOrEmpty(_foreCharAni) && !NeedPrepare)
			{
				_teammateFallBackFrame = AnimDataCollection.GetEventFrame(_commandConfig.BackCharEnterAni, "move", 1);
			}
		}
	}

	private void OnEnterPreparing(DataContext context)
	{
		_teammateChar.SetAnimationToPlayOnce(TeammateBeforeMainChar ? _foreCharAni : _backCharAni, context);
		_teammateChar.SetAnimationToLoop(TeammateBeforeMainChar ? _commandConfig.ForeCharAni2 : _commandConfig.BackCharPrepareAni, context);
		if (TeammateBeforeMainChar)
		{
			_teammateChar.SpecialAnimationLoop = _foreCharAni;
			CombatChar.SetAnimationToPlayOnce(_backCharAni, context);
			CombatChar.SetAnimationToLoop(_commandConfig.BackCharPrepareAni, context);
			CombatChar.SpecialAnimationLoop = _commandConfig.BackCharPrepareAni;
		}
	}

	private void OnEnterPrepare(DataContext context)
	{
		if (TeammateBeforeMainChar)
		{
			CombatChar.SpecialAnimationLoop = null;
			CombatChar.SetAnimationToPlayOnce(_commandConfig.BackCharExitAni, context);
			CombatChar.SetAnimationToLoop(CurrentCombatDomain.GetIdleAni(CombatChar), context);
			_teammateChar.SpecialAnimationLoop = null;
			_teammateChar.SetAnimationToPlayOnce(_commandConfig.ForeCharAni3, context);
			_teammateFallBackFrame = 1;
			_stateLeftFrame = (_clearCmdFrame = AnimDataCollection.GetDurationFrame(_commandConfig.ForeCharAni3));
		}
		else
		{
			_teammateChar.ClearTeammateCommand(context);
			_stateLeftFrame = 48;
		}
		ApplyLogicEffect();
	}

	private void OnEnterNoPrepare(DataContext context)
	{
		if (_commandImplement.IsPushOrPull())
		{
			OnEnterPushOrPull(context);
		}
		else if (_commandConfig.Type == ETeammateCommandType.Negative)
		{
			OnEnterNegative(context);
		}
		else if (_commandImplement.IsAttack())
		{
			OnEnterAttack(context);
		}
		else if (_commandImplement.IsDefend())
		{
			OnEnterDefend(context);
		}
	}

	private void OnEnterPushOrPull(DataContext context)
	{
		_teammateChar.SetAnimationToPlayOnce(_commandConfig.BackCharEnterAni, context);
		_teammateChar.SetParticleToPlay(_commandConfig.BackCharParticle, context);
		_applyLogicEffectFrame = _foreCharAniStartFrame + AnimDataCollection.GetEventFrame(_foreCharAni, "act0");
		_stateLeftFrame = AnimDataCollection.GetDurationFrame(_commandConfig.BackCharEnterAni);
		string skillSoundToPlay = _commandConfig.BackCharEnterSound;
		if (_teammateChar.AnimalConfig?.TeammateCommandBackCharEnterSound != null)
		{
			sbyte executingTeammateCommand = _teammateChar.GetExecutingTeammateCommand();
			int num = _teammateChar.GetCurrTeammateCommands().IndexOf(executingTeammateCommand);
			if (num >= 0 && num < _teammateChar.AnimalConfig.TeammateCommandBackCharEnterSound.Count)
			{
				skillSoundToPlay = _teammateChar.AnimalConfig.TeammateCommandBackCharEnterSound[num];
			}
		}
		_teammateChar.SetSkillSoundToPlay(skillSoundToPlay, context);
	}

	private void OnEnterNegative(DataContext context)
	{
		_teammateChar.SetAnimationToPlayOnce(_commandConfig.BackCharEnterAni, context);
		_teammateChar.SetParticleToPlay(_commandConfig.BackCharParticle, context);
		_applyLogicEffectFrame = AnimDataCollection.GetEventFrame(_commandConfig.BackCharEnterAni, "act0");
		_stateLeftFrame = AnimDataCollection.GetDurationFrame(_commandConfig.BackCharEnterAni);
		_teammateChar.SetSkillSoundToPlay(_commandConfig.BackCharEnterSound, context);
	}

	private void OnEnterAttack(DataContext context)
	{
		sbyte attackCommandTrickType = _teammateChar.GetAttackCommandTrickType();
		int displayPosition = CurrentCombatDomain.GetDisplayPosition(CombatChar.IsAlly, _teammateChar.GetNormalAttackPosition(attackCommandTrickType));
		_foreCharAni = _teammateChar.GetNormalAttackAnimation(attackCommandTrickType);
		_foreCharParticle = _teammateChar.GetNormalAttackParticle(attackCommandTrickType);
		_foreCharSound = _teammateChar.GetNormalAttackSound(attackCommandTrickType);
		_foreCharAniStartFrame = 34;
		string normalAttackAnimationFull = _teammateChar.GetNormalAttackAnimationFull(_foreCharAni);
		_applyLogicEffectFrame = _foreCharAniStartFrame + AnimDataCollection.GetEventFrame(normalAttackAnimationFull, "act0");
		_teammateFallBackFrame = 34 + AnimDataCollection.GetDurationFrame(normalAttackAnimationFull);
		_stateLeftFrame = (_clearCmdFrame = _teammateFallBackFrame + 48);
		_teammateChar.SetDisplayPosition(displayPosition, context);
		_teammateChar.SetAnimationToPlayOnce("M_003", context);
	}

	private void OnEnterDefend(DataContext context)
	{
		_applyLogicEffectFrame = (_stateLeftFrame = 34);
		_teammateChar.SetAnimationToPlayOnce("M_003", context);
	}

	private void ApplyLogicEffect()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		ETeammateCommandImplement implement = TeammateCommand.Instance[_commandType].Implement;
		if (_commandConfig.PosOffset < 0)
		{
			_teammateChar.SetParticleToLoop(null, dataContext);
		}
		else
		{
			CombatChar.SetParticleToLoop(null, dataContext);
		}
		_teammateChar.SetSoundToLoop(null, dataContext);
		CombatChar.SetSoundToLoop(null, dataContext);
		bool flag;
		switch (implement)
		{
		case ETeammateCommandImplement.AccelerateCast:
			ApplyAccelerateCast(dataContext);
			return;
		case ETeammateCommandImplement.Push:
		case ETeammateCommandImplement.Pull:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			ApplyPushOrPull(dataContext);
			return;
		}
		if (implement == ETeammateCommandImplement.PushOrPullIntoDanger)
		{
			ApplyPushOrPullIntoDanger(dataContext);
			return;
		}
		if (implement.IsAttack())
		{
			ApplyAttack();
			return;
		}
		if (implement.IsDefend())
		{
			ApplyDefend(dataContext);
			return;
		}
		switch (implement)
		{
		case ETeammateCommandImplement.HealInjury:
			ApplyHealInjury(dataContext);
			break;
		case ETeammateCommandImplement.HealPoison:
			ApplyHealPoison(dataContext);
			break;
		case ETeammateCommandImplement.HealFlaw:
			ApplyHealFlaw(dataContext);
			break;
		case ETeammateCommandImplement.HealAcupoint:
			ApplyHealAcupoint(dataContext);
			break;
		case ETeammateCommandImplement.TransferNeiliAllocation:
			ApplyTransferNeiliAllocation(dataContext);
			break;
		case ETeammateCommandImplement.TransferInjury:
			ApplyTransferInjury(dataContext);
			break;
		case ETeammateCommandImplement.InterruptSkill:
			ApplyInterruptSkill(dataContext);
			break;
		case ETeammateCommandImplement.AttackFlawAndAcupoint:
			ApplyAttackFlawAndAcupoint(dataContext);
			break;
		case ETeammateCommandImplement.ClearAgileAndDefense:
			ApplyClearAgileAndDefense(dataContext);
			break;
		case ETeammateCommandImplement.AddInjuryAndPoison:
			ApplyAddInjuryAndPoison(dataContext);
			break;
		case ETeammateCommandImplement.InterruptOtherAction:
			ApplyInterruptOtherAction(dataContext);
			break;
		case ETeammateCommandImplement.ReduceNeiliAllocation:
			ApplyReduceNeiliAllocation(dataContext);
			break;
		case ETeammateCommandImplement.AddUnlockAttackValue:
			ApplyAddUnlockAttackValue(dataContext);
			break;
		case ETeammateCommandImplement.TransferManyMark:
			ApplyTransferManyMark(dataContext);
			break;
		case ETeammateCommandImplement.RepairItem:
			ApplyRepairItem(dataContext);
			break;
		}
	}

	private void ApplyAccelerateCast(DataContext context)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (CombatChar.GetPreparingSkillId() >= 0)
		{
			int num = CombatChar.SkillPrepareTotalProgress * _commandConfig.IntArg / 100;
			num *= CmdEffectPercent;
			CombatChar.SkillPrepareCurrProgress = Math.Min(CombatChar.SkillPrepareCurrProgress + num, CombatChar.SkillPrepareTotalProgress);
			CombatChar.SetSkillPreparePercent((byte)(CombatChar.SkillPrepareCurrProgress * 100 / CombatChar.SkillPrepareTotalProgress), context);
		}
	}

	private void ApplyPushOrPull(DataContext context)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		CurrentCombatDomain.ChangeDistance(context, CombatChar, _commandConfig.IntArg * CmdEffectPercent);
	}

	private void ApplyPushOrPullIntoDanger(DataContext context)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!CombatChar.IsAlly);
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		int addDistance = ((currentDistance > combatCharacter.GetAttackRange().Inner) ? (-10) : 10);
		CurrentCombatDomain.ChangeDistance(context, CombatChar, addDistance);
	}

	private void ApplyAttack()
	{
		CombatCharacter combatCharacter = CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly, tryGetCoverCharacter: true);
		ApplyAttack(combatCharacter);
	}

	private void ApplyAttack(CombatCharacter enemyChar)
	{
		CombatContext combatContext = CombatContext.Create(_teammateChar, enemyChar, -1, -1);
		sbyte attackCommandTrickType = _teammateChar.GetAttackCommandTrickType();
		_teammateChar.NormalAttackHitType = CurrentCombatDomain.GetAttackHitType(_teammateChar, attackCommandTrickType);
		_teammateChar.NormalAttackBodyPart = CurrentCombatDomain.GetAttackBodyPart(_teammateChar, enemyChar, combatContext.Random, -1, attackCommandTrickType, -1);
		CurrentCombatDomain.UpdateDamageCompareData(combatContext);
		CurrentCombatDomain.CalcNormalAttack(combatContext, attackCommandTrickType);
		Events.RaiseNormalAttackAllEnd(combatContext, _teammateChar, enemyChar);
		_teammateChar.FinishFreeAttack();
	}

	private void ApplyDefend(DataContext context)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		short defendCommandSkillId = _teammateChar.GetDefendCommandSkillId();
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills((charId: _teammateChar.GetId(), skillId: defendCommandSkillId));
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[defendCommandSkillId];
		_teammateChar.SetAffectingDefendSkillId(defendCommandSkillId, context);
		_teammateChar.SetAnimationToLoop(combatSkillItem.DefendAnimation, context);
		short num = CombatSkillDomain.CalcContinuousFrames(element_CombatSkills);
		int value = (int)num * CValuePercent.op_Implicit(_commandConfig.DefendSkillDurationPercent);
		num = (short)Math.Clamp(value, 1, 32767);
		_teammateChar.TeammateCommandLeftFrame = (_teammateChar.TeammateCommandTotalFrame = num);
		CurrentCombatDomain.UpdateMaxSkillGrade(_teammateChar.IsAlly, defendCommandSkillId);
		DomainManager.SpecialEffect.Add(context, _teammateChar.GetId(), defendCommandSkillId, 0, -1);
	}

	private void ApplyHealInjury(DataContext context)
	{
		_teammateChar.SetHealInjuryCount((byte)Math.Max(_teammateChar.GetHealInjuryCount() - 1, 0), context);
		DomainManager.Character.UseCombatResources(context, _teammateChar.GetId(), EHealActionType.Healing, 1);
		CurrentCombatDomain.HealInjuryInCombat(context, CombatChar, _teammateChar);
	}

	private void ApplyHealPoison(DataContext context)
	{
		_teammateChar.SetHealPoisonCount((byte)Math.Max(_teammateChar.GetHealPoisonCount() - 1, 0), context);
		DomainManager.Character.UseCombatResources(context, _teammateChar.GetId(), EHealActionType.Detox, 1);
		CurrentCombatDomain.HealPoisonInCombat(context, CombatChar, _teammateChar);
	}

	private void ApplyHealFlaw(DataContext context)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		CombatChar.RemoveRandomFlawOrAcupoint(context, isFlaw: true, _commandConfig.IntArg * CmdEffectPercent);
	}

	private void ApplyHealAcupoint(DataContext context)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		CombatChar.RemoveRandomFlawOrAcupoint(context, isFlaw: false, _commandConfig.IntArg * CmdEffectPercent);
	}

	private unsafe void ApplyTransferNeiliAllocation(DataContext context)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		NeiliAllocation neiliAllocation = _teammateChar.GetNeiliAllocation();
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(CombatChar.GetId(), 184, (EDataModifyType)0, (int)_commandImplement, -1, -1, (EDataSumType)0));
		for (byte b = 0; b < 4; b++)
		{
			if (neiliAllocation.Items[(int)b] > 0)
			{
				int num = Math.Min(neiliAllocation.Items[(int)b], _commandConfig.IntArg * val);
				_teammateChar.ChangeNeiliAllocation(context, b, -num, applySpecialEffect: false);
				CombatChar.ChangeNeiliAllocation(context, b, num, applySpecialEffect: false);
			}
		}
	}

	private void ApplyTransferInjury(DataContext context)
	{
		Injuries injuries = CombatChar.GetInjuries();
		Injuries injuries2 = injuries.Subtract(CombatChar.GetOldInjuries());
		Injuries injuries3 = _teammateChar.GetInjuries();
		bool transferInjuryCommandIsInner = CombatChar.TransferInjuryCommandIsInner;
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			int num = Math.Min(injuries2.Get(b, transferInjuryCommandIsInner), 6 - injuries3.Get(b, transferInjuryCommandIsInner));
			for (int i = 0; i < num; i++)
			{
				list.Add(b);
			}
		}
		int num2 = Math.Min(_commandConfig.IntArg, list.Count);
		for (int j = 0; j < num2; j++)
		{
			sbyte b2 = list[context.Random.Next(0, list.Count)];
			list.Remove(b2);
			injuries.Change(b2, transferInjuryCommandIsInner, -1);
			CurrentCombatDomain.AddInjury(context, _teammateChar, b2, transferInjuryCommandIsInner, 1);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		CurrentCombatDomain.SetInjuries(context, CombatChar, injuries);
		CurrentCombatDomain.UpdateBodyDefeatMark(context, _teammateChar);
		CurrentCombatDomain.TransferFatalDamageMark(context, CombatChar, _teammateChar, _commandConfig.IntArg);
	}

	private void ApplyInterruptSkill(DataContext context)
	{
		DomainManager.Combat.InterruptSkill(context, CombatChar);
		DomainManager.Combat.SetProperLoopAniAndParticle(context, CombatChar);
	}

	private void ApplyAttackFlawAndAcupoint(DataContext context)
	{
		ApplyAttack(CombatChar);
		bool flag = context.Random.CheckPercentProb(50);
		for (sbyte b = 0; b <= 2; b++)
		{
			if (flag)
			{
				DomainManager.Combat.AddFlaw(context, CombatChar, b, CombatSkillKey.Invalid, -1);
			}
			else
			{
				DomainManager.Combat.AddAcupoint(context, CombatChar, b, CombatSkillKey.Invalid, -1);
			}
		}
	}

	private void ApplyClearAgileAndDefense(DataContext context)
	{
		DomainManager.Combat.ClearAffectingAgileSkill(context, CombatChar);
		DomainManager.Combat.ClearAffectingDefenseSkill(context, CombatChar);
	}

	private void ApplyAddInjuryAndPoison(DataContext context)
	{
		GameData.Domains.Character.Character character = _teammateChar.GetCharacter();
		GameData.Domains.Character.Character character2 = CombatChar.GetCharacter();
		sbyte poisonActionPhase = character.GetPoisonActionPhase(context.Random, character2);
		if (poisonActionPhase > 3)
		{
			DomainManager.Character.ApplyPoisonActionEffect(context, character, character2, ItemKey.Invalid);
		}
		sbyte plotHarmActionPhase = character.GetPlotHarmActionPhase(context.Random, character2);
		if (plotHarmActionPhase > 3)
		{
			DomainManager.Character.ApplyPlotHarmActionEffect(context, character, character2, ItemKey.Invalid);
		}
	}

	private void ApplyInterruptOtherAction(DataContext context)
	{
		DomainManager.Combat.InterruptOtherAction(context, CombatChar);
		CombatChar.SetPreparingItem(ItemKey.Invalid, context);
		DomainManager.Combat.SetProperLoopAniAndParticle(context, CombatChar);
	}

	private void ApplyReduceNeiliAllocation(DataContext context)
	{
		int intArg = _teammateChar.ExecutingTeammateCommandConfig.IntArg;
		for (byte b = 0; b < 4; b++)
		{
			CombatChar.ChangeNeiliAllocation(context, b, -intArg);
		}
	}

	private void ApplyAddUnlockAttackValue(DataContext context)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		int delta = GlobalConfig.Instance.UnlockAttackUnit * CValuePercent.op_Implicit(_commandConfig.IntArg);
		CombatChar.ChangeUnlockAttackValue(context, CombatChar.GetUsingWeaponIndex(), delta);
	}

	private void ApplyTransferManyMark(DataContext context)
	{
		ApplyTransferManyMarkInjury(context);
		ApplyTransferManyMarkPoison(context);
		ApplyTransferManyMarkQiDisorder(context);
	}

	private int ApplyTransferManyMarkDiv(int value, int canTransfer)
	{
		int num = Math.Max(_commandConfig.IntArg, 1);
		int val = value / num + ((value % num > 0) ? 1 : 0);
		return Math.Min(val, canTransfer);
	}

	private void ApplyTransferManyMarkInjury(DataContext context)
	{
		Injuries mainCharInjuries = CombatChar.GetInjuries();
		Injuries mainCharNewInjuries = mainCharInjuries.Subtract(CombatChar.GetOldInjuries());
		Injuries teammateInjuries = _teammateChar.GetInjuries();
		bool flag = false;
		sbyte i;
		for (i = 0; i < 7; i++)
		{
			flag = TryChangeInjuries(inner: true) || flag;
			flag = TryChangeInjuries(inner: false) || flag;
		}
		if (flag)
		{
			CurrentCombatDomain.SetInjuries(context, CombatChar, mainCharInjuries);
			CurrentCombatDomain.UpdateBodyDefeatMark(context, _teammateChar);
		}
		bool TryChangeInjuries(bool inner)
		{
			sbyte value = mainCharNewInjuries.Get(i, inner);
			int canTransfer = 6 - teammateInjuries.Get(i, inner);
			sbyte b = (sbyte)ApplyTransferManyMarkDiv(value, canTransfer);
			if (b <= 0)
			{
				return false;
			}
			mainCharInjuries.Change(i, inner, (sbyte)(-b));
			mainCharNewInjuries.Change(i, inner, (sbyte)(-b));
			CurrentCombatDomain.AddInjury(context, _teammateChar, i, inner, b);
			return true;
		}
	}

	private void ApplyTransferManyMarkPoison(DataContext context)
	{
		PoisonInts other = CombatChar.GetOldPoison();
		PoisonInts poison = CombatChar.GetPoison();
		PoisonInts poisonInts = poison.Subtract(ref other);
		PoisonInts poison2 = _teammateChar.GetPoison();
		bool flag = false;
		for (int i = 0; i < 6; i++)
		{
			int value = poison[i];
			int canTransfer = 25000 - poison2[i];
			int num = ApplyTransferManyMarkDiv(value, canTransfer);
			if (num > 0)
			{
				poison[i] -= num;
				poisonInts[i] -= num;
				poison2[i] += num;
				flag = true;
			}
		}
		if (flag)
		{
			CurrentCombatDomain.SetPoisons(context, CombatChar, poison);
			CurrentCombatDomain.SetPoisons(context, _teammateChar, poison2);
		}
	}

	private void ApplyTransferManyMarkQiDisorder(DataContext context)
	{
		short disorderOfQi = CombatChar.GetCharacter().GetDisorderOfQi();
		int value = disorderOfQi - CombatChar.GetOldDisorderOfQi();
		int canTransfer = DisorderLevelOfQi.MaxValue - _teammateChar.GetCharacter().GetDisorderOfQi();
		int num = ApplyTransferManyMarkDiv(value, canTransfer);
		if (num > 0)
		{
			CurrentCombatDomain.TransferDisorderOfQi(context, CombatChar, _teammateChar, num);
		}
	}

	private void ApplyRepairItem(DataContext context)
	{
		ItemKey[] equipment = CombatChar.GetCharacter().GetEquipment();
		foreach (ItemKey itemKey in equipment)
		{
			int num = _teammateChar.CalcTeammateCommandRepairDurabilityValue(itemKey);
			if (num > 0)
			{
				CurrentCombatDomain.ChangeDurability(context, CombatChar, itemKey, num, EChangeDurabilitySourceType.Teammate);
			}
		}
	}

	private void OnForeCharAniStart()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		if (!string.IsNullOrEmpty(_foreCharAni))
		{
			CombatCharacter combatCharacter = ((_commandConfig.PosOffset > 0) ? _teammateChar : CombatChar);
			combatCharacter.SetAnimationToPlayOnce(_foreCharAni, dataContext);
			if (_foreCharParticle != null)
			{
				combatCharacter.SetParticleToPlay(_foreCharParticle, dataContext);
			}
			if (_foreCharSound != null)
			{
				combatCharacter.SetAttackSoundToPlay(_foreCharSound, dataContext);
			}
		}
		if (_commandConfig.PosOffset < 0)
		{
			_teammateChar.SetParticleToLoop(_commandConfig.BackCharParticle, dataContext);
			if (!string.IsNullOrEmpty(_commandConfig.BackCharPrepareSound))
			{
				_teammateChar.SetSoundToLoop(_commandConfig.BackCharPrepareSound, dataContext);
			}
		}
	}

	private void OnTeammateFallBack()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		bool flag = CombatChar.TeammateBeforeMainChar == _teammateChar.GetId();
		_teammateChar.SetDisplayPosition(int.MinValue, dataContext);
		if (flag)
		{
			CombatChar.TeammateBeforeMainChar = -1;
		}
		else
		{
			CombatChar.TeammateAfterMainChar = -1;
		}
		CurrentCombatDomain.SetDisplayPosition(dataContext, CombatChar.IsAlly, int.MinValue);
		if (flag)
		{
			CombatChar.TeammateBeforeMainChar = _teammateChar.GetId();
		}
		else
		{
			CombatChar.TeammateAfterMainChar = _teammateChar.GetId();
		}
		if (_commandImplement.IsAttack())
		{
			_teammateChar.SetAnimationToPlayOnce("M_004", dataContext);
		}
	}

	private void OnClearCmd()
	{
		if (_commandImplement == ETeammateCommandImplement.GearMateA)
		{
			_teammateChar.PartlyClearTeammateCommand(_teammateChar.GetDataContext());
		}
		else
		{
			_teammateChar.ClearTeammateCommand(_teammateChar.GetDataContext());
		}
	}

	private void OnStateLeft()
	{
		ETeammateCommandImplement commandImplement = _commandImplement;
		if ((uint)(commandImplement - 2) <= 1u)
		{
			_teammateChar.ClearTeammateCommand(_teammateChar.GetDataContext());
		}
		else if (_commandImplement == ETeammateCommandImplement.TransferInjury && _teammateChar.TeammateCommandLeftPrepareFrame > 0)
		{
			DataContext dataContext = CombatChar.GetDataContext();
			CombatChar.SetParticleToLoop(_commandConfig.BackCharParticle, CombatChar.GetDataContext());
			CombatChar.SetSoundToLoop(_commandConfig.BackCharPrepareSound, dataContext);
		}
		CombatChar.StateMachine.TranslateState();
	}
}
