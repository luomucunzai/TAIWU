using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Polearm;

public class JueDou : AddDamage
{
	public JueDou()
	{
	}

	public JueDou(CombatSkillKey skillKey)
		: base(skillKey, 40902)
	{
	}

	protected override int GetAddDamagePercent()
	{
		int totalCount = base.CombatChar.GetDefeatMarkCollection().GetTotalCount();
		int num = GlobalConfig.NeedDefeatMarkCount[DomainManager.Combat.GetCombatType()] / 2;
		return 180 * totalCount / num;
	}
}
