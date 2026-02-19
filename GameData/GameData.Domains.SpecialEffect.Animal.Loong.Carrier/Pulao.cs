using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier;

public class Pulao : CarrierEffectBase
{
	protected override short CombatStateId => 202;

	public Pulao(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		CreateAffectedData(102, (EDataModifyType)2, -1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 102)
		{
			return 0;
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		sbyte b = (dataKey.IsNormalAttack ? DomainManager.Combat.GetUsingWeaponData(combatCharacter).GetInnerRatio() : DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(combatCharacter.GetId(), dataKey.CombatSkillId)).GetCurrInnerRatio());
		int val = 100 - b;
		return -Math.Min(val, b) / 2;
	}
}
