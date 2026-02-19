using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiYiHou;

public class AttackExtraPart : CombatSkillEffectBase
{
	protected sbyte AttackExtraPartCount;

	protected AttackExtraPart()
	{
	}

	protected AttackExtraPart(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
	{
		if (context.SkillKey != SkillKey || index != 3 || !CombatCharPowerMatchAffectRequire())
		{
			return;
		}
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			if (b != base.CombatChar.SkillAttackBodyPart)
			{
				list.Add(b);
			}
		}
		for (int i = 0; i < AttackExtraPartCount; i++)
		{
			int index2 = context.Random.Next(0, list.Count);
			DomainManager.Combat.DoSkillHit(context.Attacker, context.Defender, base.SkillTemplateId, list[index2], hitType);
			list.RemoveAt(index2);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		ShowSpecialEffectTips(0);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			RemoveSelf(context);
		}
	}
}
