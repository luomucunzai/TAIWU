using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.LegendaryBook.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.FistAndPalm;

public class JinSha : AddDamage
{
	private const short MaxDistance = 50;

	private const short BaseAddDamage = 60;

	private const short AddDamageUnit = 40;

	public JinSha()
	{
	}

	public JinSha(CombatSkillKey skillKey)
		: base(skillKey, 40302)
	{
	}

	protected override int GetAddDamagePercent()
	{
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		return (currentDistance <= 50) ? (60 + (50 - currentDistance) / 10 * 40) : 0;
	}
}
