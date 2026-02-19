using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm;

public class ShiXiangBaMuQiang : CombatSkillEffectBase
{
	private const sbyte StatePowerChangePercent = 10;

	public ShiXiangBaMuQiang()
	{
	}

	public ShiXiangBaMuQiang(CombatSkillKey skillKey)
		: base(skillKey, 6302, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (power > 0)
		{
			sbyte b = (sbyte)(base.IsDirect ? 1 : 2);
			CombatStateCollection combatStateCollection = base.CombatChar.GetCombatStateCollection(b);
			if (combatStateCollection.StateDict.Count > 0)
			{
				List<short> list = ObjectPool<List<short>>.Instance.Get();
				list.Clear();
				list.AddRange(combatStateCollection.StateDict.Keys);
				short num = list[context.Random.Next(0, list.Count)];
				int num2 = (int)Math.Ceiling((float)(combatStateCollection.StateDict[num].power * 10 * power) / 1000f);
				ObjectPool<List<short>>.Instance.Return(list);
				if (num2 > 0)
				{
					DomainManager.Combat.AddCombatState(context, base.CombatChar, b, num, base.IsDirect ? num2 : (-num2), reverse: false, applyEffect: false);
					ShowSpecialEffectTips(0);
				}
			}
		}
		RemoveSelf(context);
	}
}
