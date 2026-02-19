using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Finger;

public class TieHuaFuGuZhua : AttackBodyPart
{
	private const int AbsorbBreathPercent = 20;

	public TieHuaFuGuZhua()
	{
	}

	public TieHuaFuGuZhua(CombatSkillKey skillKey)
		: base(skillKey, 10202)
	{
		BodyParts = new sbyte[1];
		ReverseAddDamagePercent = 30;
	}

	protected override void OnCastAffectPower(DataContext context)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		AbsorbBreathValue(context, base.CurrEnemyChar, CValuePercent.op_Implicit(20));
		ShowSpecialEffectTips(1);
	}
}
