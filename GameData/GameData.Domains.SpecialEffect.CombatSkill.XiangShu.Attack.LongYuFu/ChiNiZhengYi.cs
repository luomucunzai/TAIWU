using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.LongYuFu;

public class ChiNiZhengYi : CombatSkillEffectBase
{
	private const sbyte InjuryThreshold = 4;

	private const sbyte PowerCountPerInjury = 2;

	public ChiNiZhengYi()
	{
	}

	public ChiNiZhengYi(CombatSkillKey skillKey)
		: base(skillKey, 17121, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Injuries injuries = base.CombatChar.GetInjuries();
		int num = 0;
		if (injuries.Get(5, isInnerInjury: false) < 4)
		{
			DomainManager.Combat.AddInjury(context, base.CombatChar, 5, isInner: false, 1, updateDefeatMark: true);
			num++;
		}
		if (injuries.Get(6, isInnerInjury: false) < 4)
		{
			DomainManager.Combat.AddInjury(context, base.CombatChar, 6, isInner: false, 1, updateDefeatMark: true);
			num++;
		}
		if (num > 0)
		{
			int id = base.CurrEnemyChar.GetId();
			Dictionary<CombatSkillKey, SkillPowerChangeCollection> allSkillPowerAddInCombat = DomainManager.Combat.GetAllSkillPowerAddInCombat();
			List<CombatSkillKey> list = ObjectPool<List<CombatSkillKey>>.Instance.Get();
			list.Clear();
			foreach (CombatSkillKey key in allSkillPowerAddInCombat.Keys)
			{
				if (key.CharId == id)
				{
					list.Add(key);
				}
			}
			if (list.Count > 0)
			{
				int num2 = Math.Min(2 * num, list.Count);
				for (int i = 0; i < num2; i++)
				{
					int index = context.Random.Next(0, list.Count);
					CombatSkillKey skillKey = list[index];
					SkillPowerChangeCollection skillPowerChangeCollection = DomainManager.Combat.RemoveSkillPowerAddInCombat(context, skillKey);
					list.RemoveAt(index);
					if (skillPowerChangeCollection != null)
					{
						DomainManager.Combat.AddSkillPowerInCombat(context, SkillKey, new SkillEffectKey(base.SkillTemplateId, isDirect: true), skillPowerChangeCollection.GetTotalChangeValue());
					}
				}
				ShowSpecialEffectTips(0);
			}
			ObjectPool<List<CombatSkillKey>>.Instance.Return(list);
			DomainManager.Combat.AddToCheckFallenSet(base.CombatChar.GetId());
		}
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
