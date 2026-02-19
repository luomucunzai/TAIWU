using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword;

public class DaCiBeiJian : AttackBodyPart
{
	public DaCiBeiJian()
	{
	}

	public DaCiBeiJian(CombatSkillKey skillKey)
		: base(skillKey, 5203)
	{
		BodyParts = new sbyte[2] { 3, 4 };
		ReverseAddDamagePercent = 45;
	}

	protected override void OnCastAffectPower(DataContext context)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		ChangeMobilityValue(context, combatCharacter, -MoveSpecialConstants.MaxMobility);
		ClearAffectingAgileSkill(context, combatCharacter);
		ShowSpecialEffectTips(1);
	}
}
