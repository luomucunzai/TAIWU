using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist;

public class YiHunDaFa : DefenseSkillBase
{
	private const sbyte TransferInjury = 2;

	private const sbyte TransferQiDisorder = 15;

	public YiHunDaFa()
	{
	}

	public YiHunDaFa(CombatSkillKey skillKey)
		: base(skillKey, 15704)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
	}

	private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		if (!isFightBack || !hit || attacker != base.CombatChar || !base.CanAffect)
		{
			return;
		}
		Injuries injuries = base.CombatChar.GetInjuries().Subtract(base.CombatChar.GetOldInjuries());
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			sbyte b2 = injuries.Get(b, !base.IsDirect);
			for (int i = 0; i < b2; i++)
			{
				list.Add(b);
			}
		}
		int num = Math.Min(2, list.Count);
		for (int j = 0; j < num; j++)
		{
			int index = context.Random.Next(0, list.Count);
			sbyte bodyPart = list[index];
			DomainManager.Combat.RemoveInjury(context, base.CombatChar, bodyPart, !base.IsDirect, 1, updateDefeatMark: true);
			DomainManager.Combat.AddInjury(context, base.CurrEnemyChar, bodyPart, !base.IsDirect, 1, updateDefeatMark: true);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		if (num > 0)
		{
			DomainManager.Combat.AddToCheckFallenSet(base.CurrEnemyChar.GetId());
			ShowSpecialEffectTips(0);
		}
		int num2 = CharObj.GetDisorderOfQi() * 15 / 100;
		if (num2 > 0)
		{
			DomainManager.Combat.TransferDisorderOfQi(context, base.CombatChar, base.CurrEnemyChar, num2);
			ShowSpecialEffectTips(1);
		}
	}
}
