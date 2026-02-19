using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist;

public class ShengSiBaMen : AssistSkillBase
{
	public ShengSiBaMen()
	{
	}

	public ShengSiBaMen(CombatSkillKey skillKey)
		: base(skillKey, 3606)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_AddDirectInjury(OnAddDirectInjury);
		Events.RegisterHandler_AddDirectFatalDamageMark(OnAddDirectFatalDamageMark);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_AddDirectInjury(OnAddDirectInjury);
		Events.UnRegisterHandler_AddDirectFatalDamageMark(OnAddDirectFatalDamageMark);
		base.OnDisable(context);
	}

	private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
	{
		TryAffect(context, attackerId, defenderId, bodyPart, outerMarkCount, innerMarkCount);
	}

	private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
	{
		TryAffect(context, attackerId, defenderId, bodyPart, outerMarkCount, innerMarkCount);
	}

	private void TryAffect(DataContext context, int attackerId, int defenderId, sbyte part, int outer, int inner)
	{
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(attackerId);
		CombatCharacter element_CombatCharacterDict2 = DomainManager.Combat.GetElement_CombatCharacterDict(defenderId);
		if (IsAffectMaker(element_CombatCharacterDict) && IsAffectChar(element_CombatCharacterDict2))
		{
			if (inner > 0)
			{
				DoAffect(context, element_CombatCharacterDict2, part, inner: true);
			}
			if (outer > 0)
			{
				DoAffect(context, element_CombatCharacterDict2, part, inner: false);
			}
		}
	}

	private bool IsAffectMaker(CombatCharacter maker)
	{
		if (base.IsDirect)
		{
			return maker.IsAlly != base.CombatChar.IsAlly;
		}
		return maker.GetId() == base.CharacterId;
	}

	private bool IsAffectChar(CombatCharacter character)
	{
		if (base.IsDirect)
		{
			return character.GetId() == base.CharacterId;
		}
		return character.IsAlly != base.CombatChar.IsAlly;
	}

	private void DoAffect(DataContext context, CombatCharacter affectChar, sbyte bodyPart, bool inner)
	{
		if (TryGetTarget(affectChar, context.Random, ref bodyPart, ref inner))
		{
			if (base.IsDirect)
			{
				affectChar.RemoveInjury(context, bodyPart, inner, 1);
			}
			else
			{
				affectChar.WorsenInjury(context, bodyPart, inner);
			}
			ShowSpecialEffectTips(0);
		}
	}

	private bool TryGetTarget(CombatCharacter affectChar, IRandomSource random, ref sbyte bodyPart, ref bool inner)
	{
		int num = 0;
		Injuries injuries = affectChar.GetInjuries();
		Injuries injuries2 = (base.IsDirect ? injuries.Subtract(affectChar.GetOldInjuries()) : injuries);
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
		for (sbyte b = 0; b < 7; b++)
		{
			if (b != bodyPart)
			{
				sbyte b2 = injuries2.Get(b, isInnerInjury: true);
				sbyte b3 = injuries2.Get(b, isInnerInjury: false);
				if (b2 > 0 || b3 > 0)
				{
					if (b2 > num || b3 > num)
					{
						num = Math.Max(Math.Max(num, b2), b3);
						list.Clear();
						list2.Clear();
					}
					if (b2 == num)
					{
						list.Add(b);
					}
					if (b3 == num)
					{
						list2.Add(b);
					}
				}
			}
		}
		bool flag = list.Count > 0;
		bool flag2 = list2.Count > 0;
		if (flag || flag2)
		{
			inner = random.RandomIsInner(flag, flag2);
			bodyPart = (inner ? list : list2).GetRandom(random);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		ObjectPool<List<sbyte>>.Instance.Return(list2);
		return flag || flag2;
	}
}
