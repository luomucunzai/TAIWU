using System;
using Config;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.Taiwu.Profession;

namespace GameData.Domains.Combat;

public class CombatCharacterStateAnimalAttack : CombatCharacterStateBase
{
	private CombatCharacter _animalChar;

	private sbyte _trickType;

	private string _attackAni;

	private string _attackParticle;

	private string _attackSound;

	private short _animalEnterFrame;

	private short _attackDamageFrame;

	private short _animalLeaveFrame;

	private short _stateTotalFrame;

	public CombatCharacterStateAnimalAttack(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.AnimalAttack)
	{
		IsUpdateOnPause = true;
	}

	public override void OnEnter()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		_animalChar = CurrentCombatDomain.GetElement_CombatCharacterDict(CurrentCombatDomain.GetCarrierAnimalCombatCharId());
		ItemKey[] weapons = _animalChar.GetWeapons();
		int num = 0;
		for (int i = 1; i < 3 && weapons[i].IsValid(); i++)
		{
			num = i;
		}
		CurrentCombatDomain.ChangeWeapon(dataContext, _animalChar, dataContext.Random.Next(num));
		sbyte[] weaponTricks = _animalChar.GetWeaponTricks();
		_trickType = weaponTricks[dataContext.Random.Next(weaponTricks.Length)];
		GameData.Domains.Item.Weapon item = _animalChar.GetWeaponData().Item;
		sbyte distance = _animalChar.AnimalConfig.AttackDistances[_animalChar.GetUsingWeaponIndex()];
		int displayPosition = CurrentCombatDomain.GetDisplayPosition(CombatChar.IsAlly, distance);
		(string, string, string, string) attackEffect = CurrentCombatDomain.GetAttackEffect(_animalChar, item, _trickType);
		_attackAni = attackEffect.Item1;
		_attackParticle = attackEffect.Item3;
		_attackSound = attackEffect.Item4;
		_animalEnterFrame = 34;
		_attackDamageFrame = (short)((double)_animalEnterFrame + Math.Round(AnimDataCollection.Data[attackEffect.Item2].Events["act0"][0] * 60f));
		_animalLeaveFrame = (short)((double)_animalEnterFrame + Math.Round(AnimDataCollection.Data[attackEffect.Item2].Duration * 60f));
		_stateTotalFrame = (short)(_animalLeaveFrame + 24);
		CombatChar.NeedAnimalAttack = false;
		_animalChar.SetVisible(visible: true, dataContext);
		_animalChar.SetDisplayPosition(displayPosition, dataContext);
		_animalChar.SetAnimationToLoop(CurrentCombatDomain.GetIdleAni(_animalChar), dataContext);
		short templateId = CombatChar.GetCharacter().GetEquipment()[11].TemplateId;
		sbyte grade = Config.Carrier.Instance[templateId].Grade;
		ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[13];
		int baseDelta = formulaCfg.Calculate(grade);
		DomainManager.Extra.ChangeProfessionSeniority(dataContext, 1, baseDelta);
	}

	public override bool OnUpdate()
	{
		if (!base.OnUpdate())
		{
			return false;
		}
		if (_animalEnterFrame > 0)
		{
			_animalEnterFrame--;
			if (_animalEnterFrame == 0)
			{
				DataContext dataContext = CombatChar.GetDataContext();
				_animalChar.SetAnimationToPlayOnce(_attackAni, dataContext);
				_animalChar.SetParticleToPlay(_attackParticle, dataContext);
				_animalChar.SetAttackSoundToPlay(_attackSound, dataContext);
			}
		}
		if (_attackDamageFrame > 0)
		{
			_attackDamageFrame--;
			if (_attackDamageFrame == 0)
			{
				CombatContext context = CombatContext.Create(_animalChar, null, -1, -1);
				CombatCharacter combatCharacter = CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly, tryGetCoverCharacter: true);
				_animalChar.NormalAttackHitType = CurrentCombatDomain.GetAttackHitType(_animalChar, _trickType);
				_animalChar.NormalAttackBodyPart = CurrentCombatDomain.GetAttackBodyPart(_animalChar, combatCharacter, context.Random, -1, _trickType, -1);
				CurrentCombatDomain.UpdateDamageCompareData(context);
				CurrentCombatDomain.CalcNormalAttack(context, _trickType);
			}
		}
		if (_animalLeaveFrame > 0)
		{
			_animalLeaveFrame--;
			if (_animalLeaveFrame == 0)
			{
				DataContext dataContext2 = CombatChar.GetDataContext();
				_animalChar.SetDisplayPosition(int.MinValue, dataContext2);
			}
		}
		if (_stateTotalFrame > 0)
		{
			_stateTotalFrame--;
			if (_stateTotalFrame == 0)
			{
				_animalChar.SetVisible(visible: false, CombatChar.GetDataContext());
				CombatChar.StateMachine.TranslateState();
			}
		}
		return false;
	}
}
