using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong;

public class WuShangYuJiaFa : KeepSkillCanCast
{
	public WuShangYuJiaFa()
	{
	}

	public WuShangYuJiaFa(CombatSkillKey skillKey)
		: base(skillKey, 11008)
	{
		RequireFiveElementsType = 0;
	}
}
