using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin;

public class AddRandomFlawAndAcupoint : DemonSlayerTrialEffectBase
{
	private readonly int _bodyPartCount;

	private readonly int _flawCount;

	private readonly int _flawLevel;

	private readonly int _acupointCount;

	private readonly int _acupointLevel;

	public AddRandomFlawAndAcupoint(int charId, IReadOnlyList<int> parameters)
		: base(charId)
	{
		_bodyPartCount = parameters[0];
		_flawCount = parameters[1];
		_flawLevel = parameters[2];
		_acupointCount = parameters[3];
		_acupointLevel = parameters[4];
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
		foreach (sbyte item in RandomUtils.GetRandomUnrepeated(context.Random, _bodyPartCount, list))
		{
			CombatSkillKey skillKey = new CombatSkillKey(-1, -1);
			DomainManager.Combat.AddFlaw(context, base.CombatChar, (sbyte)_flawLevel, skillKey, item, _flawCount);
			DomainManager.Combat.AddAcupoint(context, base.CombatChar, (sbyte)_acupointLevel, skillKey, item, _acupointCount);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}
}
