using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip;

public class BaLongShenBian : PoisonAddInjury
{
	private const int AddPowerUnit = 10;

	public BaLongShenBian()
	{
	}

	public BaLongShenBian(CombatSkillKey skillKey)
		: base(skillKey, 12407)
	{
		RequirePoisonType = 0;
	}

	protected override void OnCastMaxPower(DataContext context)
	{
		CombatCharacter combatCharacter = (base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly) : base.CombatChar);
		int num = combatCharacter.GetDefeatMarkCollection().PoisonMarkList[RequirePoisonType] * 10;
		if (num != 0)
		{
			DomainManager.Combat.AddSkillPowerInCombat(context, SkillKey, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), num);
			ShowSpecialEffectTips(1);
		}
	}
}
