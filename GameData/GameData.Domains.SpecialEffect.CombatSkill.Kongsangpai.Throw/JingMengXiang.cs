using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw;

public class JingMengXiang : CombatSkillEffectBase
{
	public JingMengXiang()
	{
	}

	public JingMengXiang(CombatSkillKey skillKey)
		: base(skillKey, 10402, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		OuterAndInnerShorts attackRange = base.CombatChar.GetAttackRange();
		short num = Math.Max(attackRange.Outer, (short)20);
		short num2 = Math.Min(attackRange.Inner, (short)120);
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		int num3 = base.CombatChar.SkillPrepareTotalProgress * (base.IsDirect ? (num2 - currentDistance) : (currentDistance - num)) / (num2 - num);
		if (num3 > 0)
		{
			DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, num3);
			ShowSpecialEffectTips(0);
		}
		Events.RegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	private unsafe void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
	{
		if (context.SkillKey != SkillKey || index != 3)
		{
			return;
		}
		if (CombatCharPowerMatchAffectRequire())
		{
			HitOrAvoidInts hitValues = CharObj.GetHitValues();
			HitOrAvoidInts avoidValues = base.CurrEnemyChar.GetCharacter().GetAvoidValues();
			if (hitValues.Items[3] > avoidValues.Items[3])
			{
				if (base.CurrEnemyChar.GetPreparingSkillId() >= 0)
				{
					DomainManager.Combat.InterruptSkill(context, base.CurrEnemyChar);
				}
				sbyte preparingOtherAction = base.CurrEnemyChar.GetPreparingOtherAction();
				if (preparingOtherAction >= 0 && preparingOtherAction != 3)
				{
					DomainManager.Combat.InterruptOtherAction(context, base.CurrEnemyChar);
				}
				ShowSpecialEffectTips(1);
			}
		}
		RemoveSelf(context);
	}
}
