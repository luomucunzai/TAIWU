using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.DefenseAndAssist;

public class JinWuGu : DefenseSkillBase
{
	private const sbyte TrickCount = 2;

	public JinWuGu()
	{
	}

	public JinWuGu(CombatSkillKey skillKey)
		: base(skillKey, 2603)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		Events.RegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		Events.UnRegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		if (!(defender != base.CombatChar || hit) && base.CanAffect && attacker.NormalAttackHitType == 2)
		{
			DoEffect(context);
		}
	}

	private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
	{
		if (!(context.Defender != base.CombatChar || hit) && base.CanAffect && index <= 2 && DomainManager.Combat.GetDamageCompareData().HitType[index] == 2)
		{
			DoEffect(context);
		}
	}

	private void DoEffect(DataContext context)
	{
		if (base.IsDirect)
		{
			DomainManager.Combat.AddRandomTrick(context, base.CombatChar, 2);
			ShowSpecialEffectTips(0);
			return;
		}
		CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
		IReadOnlyDictionary<int, sbyte> trickDict = enemyChar.GetTricks().Tricks;
		if (trickDict.Count == 0)
		{
			return;
		}
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		list2.Clear();
		list.Clear();
		list2.AddRange(trickDict.Keys);
		list2.RemoveAll((int key) => enemyChar.IsTrickUseless(trickDict[key]));
		if (list2.Count > 0)
		{
			int num = Math.Min(2, list2.Count);
			for (int num2 = 0; num2 < num; num2++)
			{
				int index = context.Random.Next(0, list2.Count);
				list.Add(new NeedTrick(trickDict[list2[index]], 1));
				list2.RemoveAt(index);
			}
			DomainManager.Combat.RemoveTrick(context, enemyChar, list, removedByAlly: false);
			ShowSpecialEffectTips(0);
		}
		ObjectPool<List<int>>.Instance.Return(list2);
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
	}
}
