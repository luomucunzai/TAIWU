using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.LongYuFu;

public class HuoLongFenXin : CombatSkillEffectBase
{
	private const sbyte InjuryThreshold = 4;

	private const sbyte AffectSkillCount = 3;

	public HuoLongFenXin()
	{
	}

	public HuoLongFenXin(CombatSkillKey skillKey)
		: base(skillKey, 17122, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		if (base.CombatChar.GetInjuries().Get(2, isInnerInjury: false) < 4)
		{
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
			Dictionary<SkillEffectKey, short> effectDict = combatCharacter.GetSkillEffectCollection().EffectDict;
			if (effectDict != null && effectDict.Count > 0)
			{
				List<SkillEffectKey> list = ObjectPool<List<SkillEffectKey>>.Instance.Get();
				int num = Math.Min(3, effectDict.Count);
				list.Clear();
				list.AddRange(effectDict.Keys);
				for (int i = 0; i < num; i++)
				{
					int index = context.Random.Next(list.Count);
					SkillEffectKey key = list[index];
					list.RemoveAt(index);
					DomainManager.Combat.ChangeSkillEffectToMinCount(context, combatCharacter, key);
					DomainManager.Combat.AddGoneMadInjury(context, combatCharacter, key.SkillId);
				}
				ObjectPool<List<SkillEffectKey>>.Instance.Return(list);
				DomainManager.Combat.AddToCheckFallenSet(combatCharacter.GetId());
				ShowSpecialEffectTips(0);
			}
			DomainManager.Combat.AddInjury(context, base.CombatChar, 2, isInner: false, 1, updateDefeatMark: true);
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
