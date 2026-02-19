using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Item;

namespace GameData.Domains.Combat;

public class CombatCharacterStateAttack : CombatCharacterStateBase
{
	private bool _inAttackRange;

	private CombatCharacter _enemyChar;

	private sbyte _trickType;

	private bool _isFightBack;

	private bool _isCritical;

	private short _moveAniFrame;

	private short _attackAniFrame;

	private short _damageFrame;

	private short _pursueFrame;

	private short _attackEndWaitFrame;

	private int _weaponId;

	private int _aniIndex;

	private string _attackAniName;

	private string _attackParticleName;

	private string _attackSound;

	public CombatCharacterStateAttack(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.Attack)
	{
		IsUpdateOnPause = true;
		RequireDelayFallen = true;
	}

	public override void OnEnter()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		_inAttackRange = CurrentCombatDomain.InAttackRange(CombatChar);
		_enemyChar = CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly, tryGetCoverCharacter: true);
		_trickType = (CombatChar.GetChangeTrickAttack() ? CombatChar.ChangeTrickType : CombatChar.GetAttackingTrickType());
		_isFightBack = CombatChar.GetIsFightBack();
		if (CombatChar.PursueAttackCount == 0)
		{
			CombatChar.NormalAttackHitType = CurrentCombatDomain.GetAttackHitType(CombatChar, _trickType);
		}
		CombatChar.NormalAttackHitType = (sbyte)DomainManager.SpecialEffect.ModifyData(CombatChar.GetId(), (short)(-1), (ushort)68, (int)CombatChar.NormalAttackHitType, -1, -1, -1);
		CombatChar.NormalAttackBodyPart = (sbyte)((!CombatChar.GetChangeTrickAttack()) ? CurrentCombatDomain.GetAttackBodyPart(CombatChar, _enemyChar, dataContext.Random, -1, _trickType, CombatChar.NormalAttackHitType) : ((CombatChar.NormalAttackHitType != 3) ? CombatChar.ChangeTrickBodyPart : (-1)));
		if (CurrentCombatDomain.IsMainCharacter(CombatChar))
		{
			CurrentCombatDomain.ForceAllTeammateLeaveCombatField(dataContext, CombatChar.IsAlly);
		}
		CurrentCombatDomain.UpdateDamageCompareData(CombatContext.Create(CombatChar, null, -1, -1));
		if (CurrentCombatDomain.IsPlayingMoveAni(_enemyChar))
		{
			_enemyChar.SetAnimationToLoop(CurrentCombatDomain.GetIdleAni(_enemyChar), dataContext);
		}
		InitAttack(dataContext);
	}

	public override void OnExit()
	{
		CombatChar.IsBreakAttacking = false;
		CombatChar.PursueAttackCount = 0;
		CombatChar.SetAnimationToLoop(CurrentCombatDomain.GetIdleAni(CombatChar), CombatChar.GetDataContext());
	}

	public override bool OnUpdate()
	{
		if (!base.OnUpdate())
		{
			return false;
		}
		if (CurrentCombatDomain.IsCharacterFallen(CombatChar))
		{
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Idle);
			return false;
		}
		if (_moveAniFrame > 0)
		{
			_moveAniFrame--;
			if (_moveAniFrame == 0)
			{
				PlayAttackAnimation();
			}
			return false;
		}
		if (_enemyChar.GetIsFightBack())
		{
			DataContext dataContext = CombatChar.GetDataContext();
			sbyte attackingTrickType = _enemyChar.GetWeaponTricks()[_enemyChar.GetWeaponTrickIndex()];
			if (_enemyChar.StateMachine.GetCurrentStateType() == CombatCharacterStateType.PrepareAttack)
			{
				_enemyChar.NormalAttackLeftRepeatTimes++;
			}
			else
			{
				_enemyChar.IsAutoNormalAttacking = true;
			}
			_enemyChar.SetAttackingTrickType(attackingTrickType, dataContext);
			_enemyChar.StateMachine.TranslateState(CombatCharacterStateType.Attack);
			if (!CombatChar.IsAutoNormalAttacking)
			{
				CombatChar.SetWeaponTrickIndex((byte)((CombatChar.GetWeaponTrickIndex() + 1) % 6), dataContext);
			}
			CombatChar.NormalAttackLeftRepeatTimes = 0;
			CombatChar.NeedNormalAttackSkipPrepare = 0;
			CombatChar.NeedFreeAttack = false;
			Events.RaiseNormalAttackAllEnd(dataContext, CombatChar, _enemyChar);
			CombatChar.NormalAttackRecovery(dataContext);
			CombatChar.FinishFreeAttack();
			CombatChar.SetAttackingTrickType(-1, dataContext);
			CombatChar.StateMachine.TranslateState();
			return false;
		}
		if (_damageFrame > 0)
		{
			_damageFrame--;
			if (_damageFrame == 0)
			{
				DataContext dataContext2 = CombatChar.GetDataContext();
				if (_inAttackRange)
				{
					CombatContext combatContext = CombatContext.Create(CombatChar, null, -1, -1);
					_isCritical = combatContext.CheckCritical(CombatChar.NormalAttackHitType);
					CurrentCombatDomain.CalcNormalAttack(combatContext.Critical(_isCritical), _trickType);
				}
				else
				{
					CombatChar.SetAttackOutOfRange(attackOutOfRange: true, dataContext2);
					Events.RaiseNormalAttackOutOfRange(dataContext2, CombatChar.GetId(), CombatChar.IsAlly);
				}
			}
		}
		if (_pursueFrame > 0)
		{
			_pursueFrame--;
			if (_pursueFrame == 0 && !_isFightBack && CurrentCombatDomain.CanPursue(CombatChar, _isCritical) && _weaponId == CurrentCombatDomain.GetUsingWeaponKey(CombatChar).Id)
			{
				CombatChar.PursueAttackCount++;
				OnEnter();
			}
		}
		if (_attackAniFrame > 0)
		{
			_attackAniFrame--;
			if (_attackAniFrame == _attackEndWaitFrame)
			{
				DataContext dataContext3 = CombatChar.GetDataContext();
				bool flag = CombatChar.NormalAttackLeftRepeatTimes > 0 && !CurrentCombatDomain.IsCharacterFallen(_enemyChar) && !CurrentCombatDomain.IsCharacterFallen(CombatChar);
				CombatChar.SetAttackingTrickType(-1, dataContext3);
				if (CombatChar.GetIsFightBack())
				{
					CurrentCombatDomain.SetDisplayPosition(dataContext3, !CombatChar.IsAlly, int.MinValue);
				}
				else if (!flag)
				{
					CurrentCombatDomain.SetDisplayPosition(dataContext3, CombatChar.IsAlly, int.MinValue);
				}
			}
			if (_attackAniFrame == 0)
			{
				DataContext dataContext4 = CombatChar.GetDataContext();
				CombatChar.SetAnimationTimeScale(1f, dataContext4);
				if (!CombatChar.IsAutoNormalAttacking)
				{
					CombatChar.SetWeaponTrickIndex((byte)((CombatChar.GetWeaponTrickIndex() + 1) % 6), dataContext4);
				}
				CurrentCombatDomain.ClearDamageCompareData(dataContext4);
				Events.RaiseNormalAttackAllEnd(dataContext4, CombatChar, _enemyChar);
				CombatChar.NormalAttackRecovery(dataContext4);
				CombatChar.FinishFreeAttack();
				if (CombatChar.GetIsFightBack())
				{
					CombatChar.SetIsFightBack(isFightBack: false, dataContext4);
					CombatChar.FightBackWithHit = false;
					CombatChar.FightBackHitType = -1;
				}
				if (CombatChar.GetChangeTrickAttack())
				{
					CombatChar.SetChangeTrickAttack(changeTrickAttack: false, dataContext4);
				}
				if (CombatChar.AttackForceHitCount > 0)
				{
					CombatChar.AttackForceHitCount--;
				}
				if (CombatChar.AttackForceMissCount > 0)
				{
					CombatChar.AttackForceMissCount--;
				}
				if (CombatChar.NormalAttackLeftRepeatTimes > 0 && !CurrentCombatDomain.IsCharacterFallen(_enemyChar) && !CurrentCombatDomain.IsCharacterFallen(CombatChar))
				{
					sbyte dataValue = CombatChar.GetWeaponTricks()[CombatChar.GetWeaponTrickIndex()];
					dataValue = (sbyte)DomainManager.SpecialEffect.ModifyData(CombatChar.GetId(), (short)(-1), (ushort)83, (int)dataValue, -1, -1, -1);
					CombatChar.SetAttackingTrickType(dataValue, dataContext4);
					if (CombatChar.NormalAttackRepeatIsFightBack)
					{
						CombatChar.SetIsFightBack(isFightBack: true, dataContext4);
					}
					CombatChar.NormalAttackLeftRepeatTimes--;
					CombatChar.IsAutoNormalAttacking = true;
					CombatChar.PursueAttackCount = 0;
					OnEnter();
				}
				else
				{
					CombatChar.NormalAttackRepeatIsFightBack = false;
					CombatChar.StateMachine.TranslateState();
					if (_enemyChar.GetAnimationToLoop() == CurrentCombatDomain.GetIdleAni(CombatChar))
					{
						CurrentCombatDomain.SetProperLoopAniAndParticle(dataContext4, _enemyChar);
					}
				}
			}
		}
		return false;
	}

	private void InitAttack(DataContext context)
	{
		_weaponId = CurrentCombatDomain.GetUsingWeaponKey(CombatChar).Id;
		GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(_weaponId);
		(string aniName, string fullAniName, string particle, string sound) attackEffect = CurrentCombatDomain.GetAttackEffect(CombatChar, element_Weapons, _trickType);
		string item = attackEffect.aniName;
		string item2 = attackEffect.fullAniName;
		string item3 = attackEffect.particle;
		string item4 = attackEffect.sound;
		_aniIndex = element_Weapons.GetWeaponAction();
		_attackAniName = item;
		_attackParticleName = item3;
		_attackSound = item4;
		float duration = AnimDataCollection.Data[item2].Duration;
		_attackAniFrame = CombatChar.CalcNormalAttackAnimationFrames(duration);
		float num = (float)_attackAniFrame / 60f;
		CombatChar.SetAnimationTimeScale(duration / num, context);
		_damageFrame = (short)Math.Max(Math.Round(AnimDataCollection.Data[item2].Events["act0"][0] * 60f), 1.0);
		_pursueFrame = (short)((_inAttackRange && !CombatChar.GetIsFightBack() && CombatChar.PursueAttackCount < 5 && CombatChar.AnimalConfig == null) ? ((short)Math.Round(AnimDataCollection.Data[item2].Events["hit"][0] * 60f)) : (-1));
		StartAttack();
	}

	private void StartAttack()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		short currentDistance = CurrentCombatDomain.GetCurrentDistance();
		TrickTypeItem trickTypeItem = Config.TrickType.Instance[_trickType];
		int usingWeaponIndex = CombatChar.GetUsingWeaponIndex();
		BossItem bossConfig = CombatChar.BossConfig;
		sbyte b = ((bossConfig != null) ? bossConfig.AttackDistances[CombatChar.GetBossPhase()][usingWeaponIndex] : (CombatChar.AnimalConfig?.AttackDistances[usingWeaponIndex] ?? trickTypeItem.AttackDistance[_aniIndex]));
		if (_inAttackRange && CombatChar.PursueAttackCount == 0 && !CombatChar.GetIsFightBack() && b > 0 && b != currentDistance)
		{
			_moveAniFrame = 9;
			CurrentCombatDomain.SetDisplayPosition(dataContext, CombatChar.IsAlly, CurrentCombatDomain.GetDisplayPosition(CombatChar.IsAlly, b));
		}
		else
		{
			_moveAniFrame = 0;
			PlayAttackAnimation();
		}
		_attackEndWaitFrame = 6;
		_attackAniFrame += _attackEndWaitFrame;
	}

	private void PlayAttackAnimation()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CombatChar.SetAnimationToPlayOnce(_attackAniName, dataContext);
		CombatChar.SetParticleToPlay(_attackParticleName, dataContext);
		CombatChar.SetAttackSoundToPlay(_attackSound, dataContext);
	}
}
