using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Item;

namespace GameData.Domains.Combat;

public class CombatCharacterStateBreakAttack : CombatCharacterStateBase
{
	private const string CommonParticle = "Particle_A_B0";

	private const string CommonAudio = "se_a_b0";

	private int _leftPrepareFrame;

	private int _leftParticleFrame;

	private int _leftAudioFrame;

	public CombatCharacterStateBreakAttack(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.BreakAttack)
	{
		IsUpdateOnPause = true;
		RequireDelayFallen = true;
	}

	public override void OnEnter()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CombatChar.NeedBreakAttack = false;
		CombatChar.IsBreakAttacking = true;
		CombatChar.IsAutoNormalAttacking = true;
		Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(CurrentCombatDomain.GetUsingWeaponKey(CombatChar).Id);
		sbyte weaponAction = element_Weapons.GetWeaponAction();
		sbyte dataValue = CombatChar.GetWeaponTricks()[CombatChar.GetWeaponTrickIndex()];
		dataValue = (sbyte)DomainManager.SpecialEffect.ModifyData(CombatChar.GetId(), (short)(-1), (ushort)83, (int)dataValue, -1, -1, -1);
		CombatChar.SetAttackingTrickType(dataValue, dataContext);
		var (text, fullAniName) = CurrentCombatDomain.GetPrepareAttackAni(CombatChar, dataValue, weaponAction);
		if (string.IsNullOrEmpty(text))
		{
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Attack);
			return;
		}
		_leftPrepareFrame = AnimDataCollection.GetDurationFrame(fullAniName);
		_leftParticleFrame = AnimDataCollection.GetEventFrame(fullAniName, "break_p0");
		_leftAudioFrame = AnimDataCollection.GetEventFrame(fullAniName, "break_a0");
		CombatChar.SetAnimationToPlayOnce(text, dataContext);
		CombatChar.SetAnimationToLoop(null, dataContext);
		DomainManager.Combat.ShowSpecialEffectTips(CombatChar.GetId(), 1662, 0);
	}

	public override bool OnUpdate()
	{
		if (!base.OnUpdate())
		{
			return false;
		}
		DataContext dataContext = CombatChar.GetDataContext();
		if (_leftParticleFrame == 0)
		{
			CombatChar.SetParticleToPlay("Particle_A_B0", dataContext);
		}
		_leftParticleFrame--;
		if (_leftAudioFrame == 0)
		{
			CombatChar.SetAttackSoundToPlay("se_a_b0", dataContext);
		}
		_leftAudioFrame--;
		_leftPrepareFrame--;
		if (_leftPrepareFrame > 0)
		{
			return false;
		}
		Events.RaiseNormalAttackPrepareEnd(dataContext, CombatChar.GetId(), CombatChar.IsAlly);
		CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Attack);
		return false;
	}
}
