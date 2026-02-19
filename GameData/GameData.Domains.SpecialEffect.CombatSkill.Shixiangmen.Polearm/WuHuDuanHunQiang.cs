using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm;

public class WuHuDuanHunQiang : PowerUpOnCast
{
	private const sbyte AddPowerUnit = 20;

	protected override EDataModifyType ModifyType => (EDataModifyType)1;

	public WuHuDuanHunQiang()
	{
	}

	public WuHuDuanHunQiang(CombatSkillKey skillKey)
		: base(skillKey, 6301)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		OuterAndInnerShorts attackRange = base.CombatChar.GetAttackRange();
		OuterAndInnerShorts attackRange2 = combatCharacter.GetAttackRange();
		PowerUpValue = Math.Max((base.IsDirect ? (attackRange.Inner - attackRange2.Inner) : (attackRange2.Outer - attackRange.Outer)) / 10 * 20, 0);
		base.OnEnable(context);
	}
}
