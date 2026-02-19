using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Common;

public class InterruptEnemyCast : CombatSkillEffectBase
{
	private const sbyte AddCastTimePercent = 50;

	protected InterruptEnemyCast()
	{
		IsLegendaryBookEffect = true;
	}

	protected InterruptEnemyCast(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
		IsLegendaryBookEffect = true;
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 212, base.SkillTemplateId), (EDataModifyType)1);
		Events.RegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
	{
		if (!(context.SkillKey != SkillKey) && index == 3 && context.Attacker.GetAttackSkillPower() >= 100 && !base.CombatChar.GetAutoCastingSkill())
		{
			short preparingSkillId = base.CurrEnemyChar.GetPreparingSkillId();
			if (preparingSkillId >= 0 && Config.CombatSkill.Instance[preparingSkillId].Type == Config.CombatSkill.Instance[base.SkillTemplateId].Type)
			{
				DomainManager.Combat.InterruptSkill(context, base.CurrEnemyChar);
			}
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return 0;
		}
		if (dataKey.FieldId == 212)
		{
			return 50;
		}
		return 0;
	}
}
