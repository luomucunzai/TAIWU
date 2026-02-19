using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.FistAndPalm;

public class XueZhiZhang : AttackBodyPart
{
	private const int AbsorbStancePercent = 20;

	public XueZhiZhang()
	{
	}

	public XueZhiZhang(CombatSkillKey skillKey)
		: base(skillKey, 12102)
	{
		BodyParts = new sbyte[1] { 1 };
		ReverseAddDamagePercent = 30;
	}

	protected override void OnCastAffectPower(DataContext context)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		AbsorbStanceValue(context, base.CurrEnemyChar, CValuePercent.op_Implicit(20));
		ShowSpecialEffectTips(1);
	}
}
