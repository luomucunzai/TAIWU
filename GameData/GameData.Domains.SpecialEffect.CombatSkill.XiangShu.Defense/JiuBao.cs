using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense;

public class JiuBao : DefenseSkillBase
{
	private const int ReduceInjuryCount = 3;

	public JiuBao()
	{
	}

	public JiuBao(CombatSkillKey skillKey)
		: base(skillKey, 16303)
	{
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		if (base.CombatChar.GetInjuries().GetSum() <= 0 || !base.SkillData.GetCanAffect())
		{
			return;
		}
		Injuries injuries = base.CombatChar.GetInjuries();
		Injuries oldInjuries = base.CombatChar.GetOldInjuries();
		for (sbyte b = 0; b < 7; b++)
		{
			if (DomainManager.Combat.CheckBodyPartInjury(base.CombatChar, b))
			{
				var (b2, b3) = injuries.Get(b);
				var (b4, b5) = oldInjuries.Get(b);
				injuries.Change(b, isInnerInjury: false, (sbyte)(-Math.Min(b2 - b4, 3)));
				injuries.Change(b, isInnerInjury: true, (sbyte)(-Math.Min(b3 - b5, 3)));
			}
		}
		Injuries injuries2 = injuries.Subtract(oldInjuries);
		List<(bool, bool)> list = new List<(bool, bool)>();
		list.Clear();
		for (sbyte b6 = 0; b6 < 7; b6++)
		{
			(sbyte, sbyte) tuple3 = oldInjuries.Get(b6);
			(sbyte, sbyte) tuple4 = injuries2.Get(b6);
			for (int i = 0; i < tuple3.Item1; i++)
			{
				list.Add((false, true));
			}
			for (int j = 0; j < tuple3.Item2; j++)
			{
				list.Add((true, true));
			}
			for (int k = 0; k < tuple4.Item1; k++)
			{
				list.Add((false, false));
			}
			for (int l = 0; l < tuple4.Item2; l++)
			{
				list.Add((true, false));
			}
		}
		int num = list.Count / 7;
		int num2 = list.Count % 7;
		injuries.Initialize();
		oldInjuries.Initialize();
		for (sbyte b7 = 0; b7 < 7; b7++)
		{
			int injuryCount = 0;
			for (int m = 0; m < num; m++)
			{
				AllocationInjury(context, list, b7, ref injuries, ref oldInjuries, ref injuryCount);
			}
			if (num2 > 0 && context.Random.CheckProb(num2, 7 - b7))
			{
				num2--;
				AllocationInjury(context, list, b7, ref injuries, ref oldInjuries, ref injuryCount);
			}
		}
		base.CombatChar.SetOldInjuries(oldInjuries, context);
		DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries);
		DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
		ShowSpecialEffectTips(0);
	}

	private static void AllocationInjury(DataContext context, List<(bool outer, bool old)> injuryRandomPool, sbyte bodyPart, ref Injuries injuries, ref Injuries oldInjuries, ref int injuryCount)
	{
		int index = context.Random.Next(0, injuryRandomPool.Count);
		(bool, bool) tuple = injuryRandomPool[index];
		CollectionUtils.SwapAndRemove(injuryRandomPool, index);
		injuries.Change(bodyPart, tuple.Item1, 1);
		if (tuple.Item2)
		{
			oldInjuries.Change(bodyPart, tuple.Item1, 1);
		}
		else
		{
			injuryCount++;
		}
	}
}
