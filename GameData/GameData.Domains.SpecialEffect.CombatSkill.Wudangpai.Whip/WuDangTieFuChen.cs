using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip;

public class WuDangTieFuChen : CombatSkillEffectBase
{
	private sbyte _affectBodyPart;

	public WuDangTieFuChen()
	{
	}

	public WuDangTieFuChen(CombatSkillKey skillKey)
		: base(skillKey, 4301, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		_affectBodyPart = -1;
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker != base.CombatChar || skillId != base.SkillTemplateId)
		{
			return;
		}
		FlawOrAcupointCollection acupointCollection = defender.GetAcupointCollection();
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		int num = 0;
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			List<(sbyte, int, int)> list2 = acupointCollection.BodyPartDict[b];
			int num2 = 0;
			for (int i = 0; i < list2.Count; i++)
			{
				num2 += list2[i].Item1 + 1;
			}
			if (num2 != 0 && num2 >= num)
			{
				if (num2 > num)
				{
					list.Clear();
					num = num2;
				}
				list.Add(b);
			}
		}
		if (list.Count > 0)
		{
			_affectBodyPart = list[context.Random.Next(0, list.Count)];
			if (base.IsDirect)
			{
				attacker.SkillAttackBodyPart = _affectBodyPart;
			}
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
	{
		if (context.SkillKey != SkillKey || index != 3 || _affectBodyPart < 0 || context.Attacker.GetAttackSkillPower() == 0)
		{
			return;
		}
		if (base.IsDirect)
		{
			FlawOrAcupointCollection acupointCollection = context.Defender.GetAcupointCollection();
			List<(sbyte, int, int)> list = acupointCollection.BodyPartDict[_affectBodyPart];
			for (int i = 0; i < list.Count; i++)
			{
				(sbyte, int, int) value = list[i];
				value.Item3 = value.Item2;
				list[i] = value;
			}
			context.Defender.SetAcupointCollection(acupointCollection, context);
		}
		else
		{
			DomainManager.Combat.DoSkillHit(context.Attacker, context.Defender, base.SkillTemplateId, _affectBodyPart, hitType);
		}
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
