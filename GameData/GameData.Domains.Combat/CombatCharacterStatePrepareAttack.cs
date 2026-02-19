using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Item;

namespace GameData.Domains.Combat;

public class CombatCharacterStatePrepareAttack : CombatCharacterStateBase
{
	public enum EType
	{
		Normal,
		NoPrepare,
		Prefer
	}

	private int _leftPrepareFrame;

	public CombatCharacterStatePrepareAttack(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.PrepareAttack)
	{
	}

	public override void OnEnter()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		switch (OnEnterCheckAttackType(dataContext))
		{
		case EType.Prefer:
			CombatChar.StateMachine.TranslateState();
			return;
		case EType.NoPrepare:
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Attack);
			return;
		}
		_leftPrepareFrame = CombatChar.CalcNormalAttackStartupFrames();
		Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(CurrentCombatDomain.GetUsingWeaponKey(CombatChar).Id);
		sbyte weaponAction = element_Weapons.GetWeaponAction();
		sbyte attackingTrickType = CombatChar.GetAttackingTrickType();
		(string, string) prepareAttackAni = CurrentCombatDomain.GetPrepareAttackAni(CombatChar, attackingTrickType, weaponAction);
		float duration = AnimDataCollection.Data[prepareAttackAni.Item2].Duration;
		float num = (float)_leftPrepareFrame / 60f;
		CombatChar.SetAnimationTimeScale(duration / num, dataContext);
		CombatChar.SetAnimationToPlayOnce(prepareAttackAni.Item1, dataContext);
		CombatChar.SetAnimationToLoop(null, dataContext);
	}

	public override bool OnUpdate()
	{
		if (!base.OnUpdate())
		{
			return false;
		}
		_leftPrepareFrame--;
		if (_leftPrepareFrame <= 0)
		{
			DataContext dataContext = CombatChar.GetDataContext();
			Events.RaiseNormalAttackPrepareEnd(dataContext, CombatChar.GetId(), CombatChar.IsAlly);
			CombatChar.SetAnimationTimeScale(1f, dataContext);
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Attack);
		}
		return false;
	}

	private EType OnEnterCheckAttackType(DataContext context)
	{
		bool flag = false;
		if (CombatChar.NeedNormalAttackSkipPrepare > 0)
		{
			CombatChar.NeedNormalAttackSkipPrepare--;
			CombatChar.IsAutoNormalAttacking = true;
			flag = true;
		}
		else if (CombatChar.NeedChangeTrickAttack)
		{
			CombatChar.NeedChangeTrickAttack = false;
			CombatChar.SetChangeTrickAttack(changeTrickAttack: true, context);
		}
		else if (CombatChar.NeedFreeAttack)
		{
			CombatChar.NeedFreeAttack = false;
			CombatChar.IsAutoNormalAttacking = true;
		}
		else
		{
			if (TryChangeToUnlockAttack(context))
			{
				return EType.Prefer;
			}
			if (CombatChar.NeedNormalAttackImmediate)
			{
				CombatChar.NeedNormalAttackImmediate = false;
			}
			else
			{
				CombatChar.SetReserveNormalAttack(reserveNormalAttack: false, context);
			}
		}
		if (!flag && CombatChar.NextAttackNoPrepare)
		{
			CombatChar.NextAttackNoPrepare = false;
			flag = true;
		}
		sbyte dataValue = (CombatChar.GetChangeTrickAttack() ? CombatChar.ChangeTrickType : CombatChar.GetWeaponTricks()[CombatChar.GetWeaponTrickIndex()]);
		dataValue = (sbyte)DomainManager.SpecialEffect.ModifyData(CombatChar.GetId(), (short)(-1), (ushort)83, (int)dataValue, -1, -1, -1);
		CombatChar.SetAttackingTrickType(dataValue, context);
		return flag ? EType.NoPrepare : EType.Normal;
	}

	private bool TryChangeToUnlockAttack(DataContext context)
	{
		int usingWeaponIndex = CombatChar.GetUsingWeaponIndex();
		if (!CombatChar.CanUnlockAttackByConfig(usingWeaponIndex))
		{
			return false;
		}
		if (!DomainManager.SpecialEffect.ModifyData(CombatChar.GetId(), -1, 307, dataValue: false))
		{
			return false;
		}
		CombatChar.NeedNormalAttackImmediate = false;
		CombatChar.SetReserveNormalAttack(reserveNormalAttack: false, context);
		CombatChar.NeedUnlockAttack = true;
		CombatChar.UnlockWeaponIndex = usingWeaponIndex;
		return true;
	}
}
