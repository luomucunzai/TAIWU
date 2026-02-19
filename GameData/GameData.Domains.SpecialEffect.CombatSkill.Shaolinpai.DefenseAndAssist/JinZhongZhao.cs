using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.DefenseAndAssist;

public class JinZhongZhao : DefenseSkillBase
{
	private const sbyte ReduceDamagePercent = -30;

	private const sbyte ChangeFlaw = 2;

	private bool _affected;

	public JinZhongZhao()
	{
	}

	public JinZhongZhao(CombatSkillKey skillKey)
		: base(skillKey, 1504)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1), (EDataModifyType)1);
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		Events.RegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
		Events.RegisterHandler_BounceInjury(OnBounceInjury);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		Events.UnRegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
		Events.UnRegisterHandler_BounceInjury(OnBounceInjury);
	}

	private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		if (_affected && defender == base.CombatChar)
		{
			_affected = false;
			ShowSpecialEffectTips(0);
		}
	}

	private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
	{
		if (_affected && context.Defender == base.CombatChar)
		{
			_affected = false;
			ShowSpecialEffectTips(0);
		}
	}

	private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
	{
		if (attackerId != base.CharacterId || !base.CanAffect || (base.IsDirect && base.CombatChar.GetFlawCount().Sum() == 0))
		{
			return;
		}
		if (base.IsDirect)
		{
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			byte[] flawCount = base.CombatChar.GetFlawCount();
			list.Clear();
			for (sbyte b = 0; b < 7; b++)
			{
				for (int i = 0; i < flawCount[b]; i++)
				{
					list.Add(b);
				}
			}
			int num = Math.Min(2, list.Count);
			for (int j = 0; j < num; j++)
			{
				int index = context.Random.Next(0, list.Count);
				DomainManager.Combat.RemoveFlaw(context, base.CombatChar, list[index], 0);
				list.RemoveAt(index);
			}
			if (num > 0)
			{
				ShowSpecialEffectTips(1);
			}
			ObjectPool<List<sbyte>>.Instance.Return(list);
		}
		else
		{
			for (int k = 0; k < 2; k++)
			{
				DomainManager.Combat.AddFlaw(context, base.CurrEnemyChar, 0, SkillKey, -1);
			}
			ShowSpecialEffectTips(1);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !base.CanAffect)
		{
			return 0;
		}
		if (dataKey.FieldId == 102)
		{
			_affected = true;
			return -30;
		}
		return 0;
	}
}
