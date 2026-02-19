using System;
using Config;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.Combat;

public class CombatCharacterStateSelectMercy : CombatCharacterStateBase
{
	private EShowMercyOption _optionType;

	private EShowMercySelect _selected;

	public CombatCharacterStateSelectMercy(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.SelectMercy)
	{
		IsUpdateOnPause = true;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		DataContext dataContext = CombatChar.GetDataContext();
		CombatChar.NeedSelectMercyOption = false;
		_optionType = ((!CombatChar.IsAlly) ? EShowMercyOption.EnemyShowMercy : (CurrentCombatDomain.IsInfectedCombat() ? EShowMercyOption.FuyuSword : EShowMercyOption.PlayerShowMercy));
		CurrentCombatDomain.SetShowMercyOption(dataContext, _optionType);
		_selected = EShowMercySelect.Unselected;
		CurrentCombatDomain.SetSelectedMercyOption(dataContext, _selected);
	}

	public override void OnExit()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CurrentCombatDomain.SetShowMercyOption(dataContext, EShowMercyOption.Invalid);
		if (CurrentCombatDomain.IsInCombat())
		{
			CurrentCombatDomain.SetDisplayPosition(dataContext, CombatChar.IsAlly, int.MinValue);
		}
		base.OnExit();
	}

	public override bool OnUpdate()
	{
		if (!base.OnUpdate())
		{
			return false;
		}
		DataContext dataContext = CombatChar.GetDataContext();
		EShowMercySelect selectedMercyOption = (EShowMercySelect)CurrentCombatDomain.GetSelectedMercyOption();
		if (selectedMercyOption <= EShowMercySelect.Unselected || _selected > EShowMercySelect.Unselected)
		{
			return false;
		}
		_selected = selectedMercyOption;
		if (selectedMercyOption == EShowMercySelect.Cancel)
		{
			ApplyFailEffect();
		}
		else if (_optionType == EShowMercyOption.FuyuSword)
		{
			CombatItemUseItem prepareFuyuSword = CombatItemUse.DefValue.PrepareFuyuSword;
			CombatChar.SetAnimationToPlayOnce(prepareFuyuSword.Animation, dataContext);
			CombatChar.SetParticleToPlay(prepareFuyuSword.Particle, dataContext);
			CombatChar.SetSkillSoundToPlay(prepareFuyuSword.Sound, dataContext);
			CombatChar.SetAnimationToLoop(null, dataContext);
			DelayCall(OnPreparedFuyu, Config.Misc.DefValue.FuyuSwordFragment.UseFrame);
		}
		else
		{
			string text = "C_007_1";
			DelayCall(OnFlash, AnimDataCollection.GetDurationFrame(text));
			CombatChar.SetAnimationToPlayOnce(text, dataContext);
			CombatChar.SetAnimationToLoop(null, dataContext);
			CombatChar.SetSkillSoundToPlay("se_combat_preskill", dataContext);
		}
		return false;
	}

	private void OnFlash()
	{
		DataContext context = CurrentCombatDomain.Context;
		sbyte trickType = CombatChar.GetWeaponTricks()[CombatChar.GetWeaponTrickIndex()];
		GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(CurrentCombatDomain.GetUsingWeaponKey(CombatChar).Id);
		(string, string) prepareAttackAni = CurrentCombatDomain.GetPrepareAttackAni(CombatChar, trickType, element_Weapons.GetWeaponAction());
		DelayCall(OnPrepared, AnimDataCollection.GetDurationFrame(prepareAttackAni.Item2));
		CombatChar.SetAnimationToPlayOnce(prepareAttackAni.Item1, context);
	}

	private void OnPrepared()
	{
		DataContext context = CurrentCombatDomain.Context;
		sbyte index = CombatChar.GetWeaponTricks()[CombatChar.GetWeaponTrickIndex()];
		int usingWeaponIndex = CombatChar.GetUsingWeaponIndex();
		BossItem bossConfig = CombatChar.BossConfig;
		sbyte b = ((bossConfig != null) ? bossConfig.AttackDistances[CombatChar.GetBossPhase()][usingWeaponIndex] : (CombatChar.AnimalConfig?.AttackDistances[usingWeaponIndex] ?? Config.TrickType.Instance[index].AttackDistance[0]));
		int frame = 0;
		if (b > 0 && b != CurrentCombatDomain.GetCurrentDistance())
		{
			frame = 9;
			CurrentCombatDomain.SetDisplayPosition(context, CombatChar.IsAlly, CurrentCombatDomain.GetDisplayPosition(CombatChar.IsAlly, b));
		}
		DelayCall(PlayAttackAnimation, frame);
	}

	private void PlayAttackAnimation()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		int id = CurrentCombatDomain.GetUsingWeaponKey(CombatChar).Id;
		GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(id);
		sbyte trickType = CombatChar.GetWeaponTricks()[CombatChar.GetWeaponTrickIndex()];
		(string, string, string, string) attackEffect = CurrentCombatDomain.GetAttackEffect(CombatChar, element_Weapons, trickType);
		DelayCall(ApplyFailEffect, AnimDataCollection.GetEventFrame(attackEffect.Item2, "act0"));
		DelayCall(OnAttacked, AnimDataCollection.GetDurationFrame(attackEffect.Item2));
		CombatChar.SetAnimationToPlayOnce(attackEffect.Item1, dataContext);
		CombatChar.SetParticleToPlay(attackEffect.Item3, dataContext);
		CombatChar.SetAttackSoundToPlay(attackEffect.Item4, dataContext);
	}

	private void OnAttacked()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CombatChar.SetAnimationToLoop(CurrentCombatDomain.GetIdleAni(CombatChar), dataContext);
	}

	private void ApplyFailEffect()
	{
		if (_optionType != EShowMercyOption.FuyuSword || _selected == EShowMercySelect.Cancel)
		{
			SetFailAnimation();
		}
		DelayCall(OnSettlement, (short)(Math.Ceiling(DomainManager.Combat.GetTimeScale()) + 1.0));
	}

	private void SetFailAnimation()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CombatCharacter combatCharacter = CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly);
		bool flag = _selected != EShowMercySelect.Cancel;
		var (text, text2, text3) = CurrentCombatDomain.GetFailAnimationAndSound(dataContext, CombatChar, flee: false, flag);
		if (flag)
		{
			int id = CurrentCombatDomain.GetUsingWeaponKey(CombatChar).Id;
			GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(id);
			WeaponItem weaponData = Config.Weapon.Instance[element_Weapons.GetTemplateId()];
			CurrentCombatDomain.PlayHitSound(dataContext, combatCharacter, weaponData);
			CurrentCombatDomain.ClearBurstBodyPartFlawAndAcupoint(dataContext, combatCharacter, text);
		}
		else
		{
			CombatChar.PlayWinAnimation(dataContext);
		}
		combatCharacter.SetAnimationToPlayOnce(text, dataContext);
		if (!string.IsNullOrEmpty(text2))
		{
			combatCharacter.SetParticleToPlay(text2, dataContext);
		}
		if (!string.IsNullOrEmpty(text3))
		{
			combatCharacter.SetDieSoundToPlay(text3, dataContext);
		}
	}

	private void OnSettlement()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CurrentCombatDomain.CombatSettlement(dataContext, CombatChar.IsAlly ? CombatStatusType.EnemyFail : CombatStatusType.SelfFail);
		CombatChar.StateMachine.TranslateState();
	}

	private void OnPreparedFuyu()
	{
		DataContext context = CurrentCombatDomain.Context;
		CombatItemUseItem useFuyuSword = CombatItemUse.DefValue.UseFuyuSword;
		int frame = 0;
		short distance = useFuyuSword.Distance;
		if (distance > 0 && distance != CurrentCombatDomain.GetCurrentDistance())
		{
			frame = 9;
			CurrentCombatDomain.SetDisplayPosition(context, CombatChar.IsAlly, CurrentCombatDomain.GetDisplayPosition(CombatChar.IsAlly, distance));
		}
		DelayCall(PlayUseFuyuAnim, frame);
	}

	private void PlayUseFuyuAnim()
	{
		DataContext context = CurrentCombatDomain.Context;
		CombatItemUseItem useFuyuSword = CombatItemUse.DefValue.UseFuyuSword;
		CombatChar.SetParticleToPlay(useFuyuSword.Particle, context);
		CombatChar.SetSkillSoundToPlay(useFuyuSword.Sound, context);
		CombatChar.SetAnimationToPlayOnce(useFuyuSword.Animation, context);
		DelayCall(PlayFuyuHitAnim, AnimDataCollection.GetEventFrame(useFuyuSword.Animation, "act0"));
		DelayCall(PlayFuyuCastAnim, AnimDataCollection.GetDurationFrame(useFuyuSword.Animation));
	}

	private void PlayFuyuHitAnim()
	{
		DataContext context = CurrentCombatDomain.Context;
		CombatItemUseItem useFuyuSword = CombatItemUse.DefValue.UseFuyuSword;
		CombatCharacter combatCharacter = CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly);
		if (!string.IsNullOrEmpty(useFuyuSword.BeHitAnimation))
		{
			combatCharacter.SetAnimationToPlayOnce(useFuyuSword.BeHitAnimation, context);
		}
	}

	private void PlayFuyuCastAnim()
	{
		DataContext context = CurrentCombatDomain.Context;
		CombatCharacter combatCharacter = CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly);
		DomainManager.Combat.AppendGetChar(combatCharacter.GetId());
		DomainManager.Combat.AppendEvaluation((sbyte)((CombatChar.GetCharacter().GetConsummateLevel() > combatCharacter.GetCharacter().GetConsummateLevel()) ? 23 : 24));
		DomainManager.TaiwuEvent.SetListenerEventActionBoolArg("CombatOver", "UsedFuyuSwordInCombat", value: true);
		CurrentCombatDomain.EndCombat(context, combatCharacter, flee: false, playAni: false);
	}
}
