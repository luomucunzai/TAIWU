using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ShuFang;

public class TianYanShu : CombatSkillEffectBase
{
	private const sbyte AffectSkillCount = 2;

	private const sbyte AddPower = 30;

	public TianYanShu()
	{
	}

	public TianYanShu(CombatSkillKey skillKey)
		: base(skillKey, 17081, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		for (sbyte b = 1; b <= 4; b++)
		{
			list.AddRange(base.CombatChar.GetCombatSkillList(b));
		}
		list.RemoveAll((short id) => id < 0);
		int num = Math.Min(2, list.Count);
		for (int num2 = 0; num2 < num; num2++)
		{
			int index = context.Random.Next(list.Count);
			DomainManager.Combat.AddSkillPowerInCombat(context, new CombatSkillKey(base.CombatChar.GetId(), list[index]), effectKey, 30);
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
