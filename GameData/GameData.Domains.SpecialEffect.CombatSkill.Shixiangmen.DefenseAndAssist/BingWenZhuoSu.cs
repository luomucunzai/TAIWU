using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.DefenseAndAssist;

public class BingWenZhuoSu : AssistSkillBase
{
	private static readonly CValuePercent ChangeMobilityPercent = CValuePercent.op_Implicit(12);

	private bool _affected;

	public BingWenZhuoSu()
	{
	}

	public BingWenZhuoSu(CombatSkillKey skillKey)
		: base(skillKey, 6601)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		_affected = false;
		Events.RegisterHandler_AddDirectInjury(OnAddDirectInjury);
		Events.RegisterHandler_AddDirectFatalDamageMark(OnAddDirectFatalDamageMark);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_AddDirectInjury(OnAddDirectInjury);
		Events.UnRegisterHandler_AddDirectFatalDamageMark(OnAddDirectFatalDamageMark);
	}

	private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
	{
		if (base.CanAffect && (outerMarkCount > 0 || innerMarkCount > 0))
		{
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
			if (defenderId == base.CharacterId && attackerId == combatCharacter.GetId())
			{
				ChangeMobility(context, base.IsDirect ? base.CombatChar : combatCharacter);
			}
		}
	}

	private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
	{
		if (base.CanAffect && (outerMarkCount > 0 || innerMarkCount > 0))
		{
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
			if (defenderId == base.CharacterId && attackerId == combatCharacter.GetId())
			{
				ChangeMobility(context, base.IsDirect ? base.CombatChar : combatCharacter);
			}
		}
	}

	private void ChangeMobility(DataContext context, CombatCharacter affectChar)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (!_affected)
		{
			int num = MoveSpecialConstants.MaxMobility * ChangeMobilityPercent;
			ChangeMobilityValue(context, affectChar, base.IsDirect ? num : (-num));
			ShowSpecialEffectTips(0);
			ShowEffectTips(context);
			_affected = true;
			Events.RegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
		}
	}

	private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		_affected = false;
		Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
	}
}
