using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense;

public class FengShenZhaoMing : DefenseSkillBase
{
	private readonly List<(sbyte, bool)> _injuryRandomPool = new List<(sbyte, bool)>();

	public FengShenZhaoMing()
	{
	}

	public FengShenZhaoMing(CombatSkillKey skillKey)
		: base(skillKey, 16307)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 116, -1), (EDataModifyType)3);
	}

	public override OuterAndInnerInts GetModifiedValue(AffectedDataKey dataKey, OuterAndInnerInts dataValue)
	{
		if (dataKey.CharId != base.CharacterId || !base.CanAffect)
		{
			return dataValue;
		}
		DataContext context = DomainManager.Combat.Context;
		Injuries injuries = base.CombatChar.GetInjuries().Subtract(base.CombatChar.GetOldInjuries());
		if (dataValue.Outer + dataValue.Inner <= 0 || injuries.GetSum() <= 0)
		{
			return dataValue;
		}
		_injuryRandomPool.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b);
			for (int i = 0; i < tuple.Item1; i++)
			{
				_injuryRandomPool.Add((b, false));
			}
			for (int j = 0; j < tuple.Item2; j++)
			{
				_injuryRandomPool.Add((b, true));
			}
		}
		while (dataValue.Outer + dataValue.Inner > 0 && _injuryRandomPool.Count > 0)
		{
			int index = context.Random.Next(0, _injuryRandomPool.Count);
			(sbyte, bool) tuple2 = _injuryRandomPool[index];
			_injuryRandomPool.RemoveAt(index);
			DomainManager.Combat.ChangeToOldInjury(context, base.CombatChar, tuple2.Item1, tuple2.Item2, 1);
			if (dataValue.Inner <= 0 || (dataValue.Outer > 0 && context.Random.CheckPercentProb(50)))
			{
				dataValue.Outer--;
			}
			else
			{
				dataValue.Inner--;
			}
		}
		ShowSpecialEffectTips(0);
		return dataValue;
	}
}
