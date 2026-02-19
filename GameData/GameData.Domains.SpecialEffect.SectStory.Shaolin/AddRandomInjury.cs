using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin;

public class AddRandomInjury : DemonSlayerTrialEffectBase
{
	private readonly int _bodyPartCount;

	private readonly int _outerInjuryLevel;

	private readonly int _innerInjuryLevel;

	public AddRandomInjury(int charId, IReadOnlyList<int> parameters)
		: base(charId)
	{
		_bodyPartCount = parameters[0];
		_outerInjuryLevel = parameters[1];
		_innerInjuryLevel = parameters[2];
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		base.OnDisable(context);
	}

	private void OnCombatBegin(DataContext context)
	{
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			list.Add(b);
		}
		Injuries injuries = base.CombatChar.GetInjuries();
		foreach (sbyte item3 in RandomUtils.GetRandomUnrepeated(context.Random, _bodyPartCount, list))
		{
			(sbyte outer, sbyte inner) tuple = injuries.Get(item3);
			sbyte item = tuple.outer;
			sbyte item2 = tuple.inner;
			sbyte b2 = (sbyte)Math.Min(6 - item, _outerInjuryLevel);
			sbyte b3 = (sbyte)Math.Min(6 - item2, _innerInjuryLevel);
			int num = _outerInjuryLevel + _innerInjuryLevel - b2 - b3;
			if (b2 > 0)
			{
				DomainManager.Combat.AddInjury(context, base.CombatChar, item3, isInner: false, b2);
			}
			if (b3 > 0)
			{
				DomainManager.Combat.AddInjury(context, base.CombatChar, item3, isInner: true, b3);
			}
			if (num > 0)
			{
				DomainManager.Combat.AppendFatalDamageMark(context, base.CombatChar, num, -1, -1);
			}
		}
		DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}
}
