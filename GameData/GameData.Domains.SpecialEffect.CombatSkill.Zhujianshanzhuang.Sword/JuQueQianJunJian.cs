using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Sword;

public class JuQueQianJunJian : SwordAddFatalEffectBase
{
	protected override IEnumerable<sbyte> RequirePersonalityTypes
	{
		get
		{
			yield return 2;
			yield return 3;
		}
	}

	protected override int RequirePersonalityValue => 40;

	protected override CValueMultiplier FlawOrAcupointCount => CValueMultiplier.op_Implicit(base.EnemyChar.GetDefeatMarkCollection().GetTotalFlawCount());

	public JuQueQianJunJian()
	{
	}

	public JuQueQianJunJian(CombatSkillKey skillKey)
		: base(skillKey, 9103)
	{
	}
}
