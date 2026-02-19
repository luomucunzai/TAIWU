using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense;

public class SanYuanJiuDunTianDiBian : DefenseSkillBase
{
	private const sbyte AffectFrame = 120;

	private static readonly CValuePercent AddStanceAndBreathPercent = CValuePercent.op_Implicit(10);

	private int _frameCounter;

	private readonly List<(sbyte type, sbyte bodyPart)> _markRandomPool = new List<(sbyte, sbyte)>();

	public SanYuanJiuDunTianDiBian()
	{
	}

	public SanYuanJiuDunTianDiBian(CombatSkillKey skillKey)
		: base(skillKey, 16308)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		_frameCounter = 0;
		Events.RegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
	}

	private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		if (base.CombatChar != combatChar || DomainManager.Combat.Pause)
		{
			return;
		}
		_frameCounter++;
		if (_frameCounter < 120 || !base.CanAffect)
		{
			return;
		}
		_frameCounter = 0;
		_markRandomPool.Clear();
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		DefeatMarkCollection defeatMarkCollection = base.CombatChar.GetDefeatMarkCollection();
		DefeatMarkCollection defeatMarkCollection2 = combatCharacter.GetDefeatMarkCollection();
		for (sbyte b = 0; b < 7; b++)
		{
			for (int i = 0; i < Math.Min(defeatMarkCollection.OuterInjuryMarkList[b], 6 - defeatMarkCollection2.OuterInjuryMarkList[b]); i++)
			{
				_markRandomPool.Add((0, b));
			}
			for (int j = 0; j < Math.Min(defeatMarkCollection.InnerInjuryMarkList[b], 6 - defeatMarkCollection2.InnerInjuryMarkList[b]); j++)
			{
				_markRandomPool.Add((1, b));
			}
			for (int k = 0; k < Math.Min(defeatMarkCollection.FlawMarkList[b].Count, combatCharacter.GetMaxFlawCount() - defeatMarkCollection2.FlawMarkList[b].Count); k++)
			{
				_markRandomPool.Add((2, b));
			}
			for (int l = 0; l < Math.Min(defeatMarkCollection.AcupointMarkList[b].Count, combatCharacter.GetMaxAcupointCount() - defeatMarkCollection2.AcupointMarkList[b].Count); l++)
			{
				_markRandomPool.Add((3, b));
			}
		}
		for (int m = 0; m < defeatMarkCollection.MindMarkList.Count; m++)
		{
			_markRandomPool.Add((4, -1));
		}
		for (int n = 0; n < defeatMarkCollection.DieMarkList.Count; n++)
		{
			_markRandomPool.Add((5, -1));
		}
		if (_markRandomPool.Count <= 0)
		{
			return;
		}
		(sbyte, sbyte) tuple = _markRandomPool[context.Random.Next(0, _markRandomPool.Count)];
		if (tuple.Item1 == 0 || tuple.Item1 == 1)
		{
			Injuries injuries = base.CombatChar.GetInjuries();
			Injuries oldInjuries = base.CombatChar.GetOldInjuries();
			bool isInnerInjury = tuple.Item1 == 1;
			int num = oldInjuries.Get(tuple.Item2, isInnerInjury);
			if (num > 0 && context.Random.CheckProb(num, injuries.Get(tuple.Item2, isInnerInjury)))
			{
				oldInjuries.Change(tuple.Item2, isInnerInjury, -1);
				base.CombatChar.SetOldInjuries(oldInjuries, context);
			}
			injuries.Change(tuple.Item2, isInnerInjury, -1);
			DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries);
		}
		else if (tuple.Item1 == 2)
		{
			DomainManager.Combat.RemoveFlaw(context, base.CombatChar, tuple.Item2, context.Random.Next(0, defeatMarkCollection.FlawMarkList[tuple.Item2].Count));
		}
		else if (tuple.Item1 == 3)
		{
			DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, tuple.Item2, context.Random.Next(0, defeatMarkCollection.AcupointMarkList[tuple.Item2].Count));
		}
		else if (tuple.Item1 == 4)
		{
			DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, random: true);
		}
		else if (tuple.Item1 == 5)
		{
			int index = context.Random.Next(0, defeatMarkCollection.DieMarkList.Count);
			defeatMarkCollection.DieMarkList.RemoveAt(index);
			base.CombatChar.SetDefeatMarkCollection(defeatMarkCollection, context);
			DomainManager.Combat.AddToCheckFallenSet(combatCharacter.GetId());
		}
		DomainManager.Combat.ChangeBreathValue(context, base.CombatChar, 30000 * AddStanceAndBreathPercent);
		DomainManager.Combat.ChangeStanceValue(context, base.CombatChar, 4000 * AddStanceAndBreathPercent);
		ShowSpecialEffectTips(0);
	}
}
