using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw;

public class JieQingShiJue : TrickBuffFlaw
{
	public JieQingShiJue()
	{
	}

	public JieQingShiJue(CombatSkillKey skillKey)
		: base(skillKey, 13300)
	{
		RequireTrickType = 1;
	}

	protected override bool OnReverseAffect(DataContext context, int trickCount)
	{
		int num = trickCount / 2;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			num2 += base.CurrEnemyChar.UpgradeRandomFlawOrAcupoint(context, isFlaw: true, 1, -1);
		}
		return num2 > 0;
	}
}
