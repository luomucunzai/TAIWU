using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Agile;

public class YuNvNuoLian : AgileSkillBase
{
	private const sbyte NeedMoveDistance = 10;

	private const sbyte ExchangeTrickCount = 3;

	private int _distanceAccumulator;

	public YuNvNuoLian()
	{
	}

	public YuNvNuoLian(CombatSkillKey skillKey)
		: base(skillKey, 2503)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_DistanceChanged(OnDistanceChanged);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_DistanceChanged(OnDistanceChanged);
	}

	private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
	{
		if (mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)))
		{
			return;
		}
		_distanceAccumulator += Math.Abs(distance);
		while (_distanceAccumulator >= 10)
		{
			_distanceAccumulator -= 10;
			if (!base.CanAffect)
			{
				continue;
			}
			CombatCharacter combatChar = base.CombatChar;
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
			TrickCollection tricks = base.CombatChar.GetTricks();
			TrickCollection tricks2 = combatCharacter.GetTricks();
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
			list.Clear();
			list2.Clear();
			list.AddRange(tricks.Tricks.Where(delegate(KeyValuePair<int, sbyte> trick)
			{
				CombatCharacter combatChar2 = base.CombatChar;
				KeyValuePair<int, sbyte> keyValuePair = trick;
				return combatChar2.IsTrickUseless(keyValuePair.Value);
			}).Select(delegate(KeyValuePair<int, sbyte> trick)
			{
				KeyValuePair<int, sbyte> keyValuePair = trick;
				return keyValuePair.Value;
			}));
			list2.AddRange(tricks2.Tricks.Where(delegate(KeyValuePair<int, sbyte> trick)
			{
				CombatCharacter combatChar2 = base.CombatChar;
				KeyValuePair<int, sbyte> keyValuePair = trick;
				return combatChar2.IsTrickUsable(keyValuePair.Value);
			}).Select(delegate(KeyValuePair<int, sbyte> trick)
			{
				KeyValuePair<int, sbyte> keyValuePair = trick;
				return keyValuePair.Value;
			}));
			int num = Math.Min(Math.Min(list.Count, list2.Count), 3);
			if (num > 0)
			{
				CollectionUtils.Shuffle(context.Random, list);
				CollectionUtils.Shuffle(context.Random, list2);
				List<NeedTrick> list3 = ObjectPool<List<NeedTrick>>.Instance.Get();
				List<NeedTrick> list4 = ObjectPool<List<NeedTrick>>.Instance.Get();
				List<NeedTrick> list5 = ObjectPool<List<NeedTrick>>.Instance.Get();
				List<NeedTrick> list6 = ObjectPool<List<NeedTrick>>.Instance.Get();
				for (int num2 = 0; num2 < num; num2++)
				{
					sbyte type = list[num2];
					sbyte type2 = list2[num2];
					list3.Add(new NeedTrick(type, 1));
					list4.Add(new NeedTrick(type2, 1));
					list5.Add(new NeedTrick(type2, 1));
					list6.Add(new NeedTrick(type, 1));
				}
				DomainManager.Combat.RemoveTrick(context, combatChar, list3);
				DomainManager.Combat.RemoveTrick(context, combatCharacter, list5, removedByAlly: false);
				DomainManager.Combat.AddTrick(context, combatChar, list4);
				DomainManager.Combat.AddTrick(context, combatCharacter, list6, addedByAlly: false);
				ObjectPool<List<NeedTrick>>.Instance.Return(list3);
				ObjectPool<List<NeedTrick>>.Instance.Return(list4);
				ObjectPool<List<NeedTrick>>.Instance.Return(list5);
				ObjectPool<List<NeedTrick>>.Instance.Return(list6);
				ShowSpecialEffectTips(0);
			}
			ObjectPool<List<sbyte>>.Instance.Return(list);
			ObjectPool<List<sbyte>>.Instance.Return(list2);
		}
	}
}
