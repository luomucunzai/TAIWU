using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ShuFang;

public class MoTongShu : CombatSkillEffectBase
{
	private const sbyte AffectSkillCount = 4;

	private const sbyte ReducePower = -30;

	public MoTongShu()
	{
	}

	public MoTongShu(CombatSkillKey skillKey)
		: base(skillKey, 17084, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		for (sbyte b = 1; b <= 4; b++)
		{
			list.AddRange(combatCharacter.GetCombatSkillList(b));
		}
		list.RemoveAll((short id) => id < 0);
		int num = Math.Min(4, list.Count);
		for (int num2 = 0; num2 < num; num2++)
		{
			int index = context.Random.Next(list.Count);
			DomainManager.Combat.ReduceSkillPowerInCombat(context, new CombatSkillKey(combatCharacter.GetId(), list[index]), effectKey, -30);
			list.RemoveAt(index);
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ShowSpecialEffectTips(0);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			RemoveSelf(context);
		}
	}
}
