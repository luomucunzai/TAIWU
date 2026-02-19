using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Agile;

public class ZuiBaXianBu : AgileSkillBase
{
	private enum EType
	{
		Mobility,
		Breath,
		Stance
	}

	private delegate void TypeDoAffectHandler(DataContext context, CombatCharacter affectChar, int param);

	private static readonly Dictionary<EType, TypeDoAffectHandler> Type2Affects = new Dictionary<EType, TypeDoAffectHandler>
	{
		{
			EType.Mobility,
			ChangeMobility
		},
		{
			EType.Breath,
			ChangeBreath
		},
		{
			EType.Stance,
			ChangeStance
		}
	};

	private const int AffectRequireDistance = 5;

	private const int JudgeOdds = 20;

	private const int WineOdds = 100;

	private static readonly int[] DirectParam = new int[4] { 20, 20, 20, 1 };

	private static readonly int[] ReverseParam = new int[4] { -20, -20, -20, -1 };

	private int _prevDistance;

	private int _movedDistance;

	private short CurrDistance => DomainManager.Combat.GetCurrentDistance();

	public ZuiBaXianBu()
	{
	}

	public ZuiBaXianBu(CombatSkillKey skillKey)
		: base(skillKey, 14401)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_MoveBegin(OnMoveBegin);
		Events.RegisterHandler_MoveEnd(OnMoveEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_MoveBegin(OnMoveBegin);
		Events.UnRegisterHandler_MoveEnd(OnMoveEnd);
		base.OnDisable(context);
	}

	private void OnMoveBegin(DataContext context, CombatCharacter mover, int distance, bool isJump)
	{
		if (IsMatchCharacter(mover))
		{
			_prevDistance = ((base.CanAffect && IsMatchAttackRange()) ? CurrDistance : (-1));
		}
	}

	private void OnMoveEnd(DataContext context, CombatCharacter mover, int distance, bool isJump)
	{
		if (!IsMatch(mover) || _prevDistance < 0)
		{
			return;
		}
		_movedDistance += Math.Abs(CurrDistance - _prevDistance);
		if (_movedDistance >= 5)
		{
			int num = _movedDistance / 5;
			_movedDistance %= 5;
			for (int i = 0; i < num; i++)
			{
				DoAffect(context);
			}
		}
	}

	private bool IsMatch(CombatCharacter mover)
	{
		return IsMatchCharacter(mover) && IsMatchAttackRange();
	}

	private bool IsMatchCharacter(CombatCharacter mover)
	{
		return base.IsDirect ? (mover.GetId() == base.CharacterId) : (mover.IsAlly != base.CombatChar.IsAlly);
	}

	private bool IsMatchAttackRange()
	{
		CombatCharacter character = (base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly) : base.CombatChar);
		return DomainManager.Combat.InAttackRange(character);
	}

	private void DoAffect(DataContext context)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < Type2Affects.Count; i++)
		{
			list.Add(20);
		}
		if (CharObj.GetEatingItems().ContainsWine())
		{
			list[RandomUtils.GetRandomIndex(list, context.Random)] = 100;
		}
		CombatCharacter affectChar = (base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly));
		int[] array = (base.IsDirect ? DirectParam : ReverseParam);
		foreach (var (eType2, typeDoAffectHandler2) in Type2Affects)
		{
			if (context.Random.CheckPercentProb(list[(int)eType2]))
			{
				typeDoAffectHandler2(context, affectChar, array[(int)eType2]);
				ShowSpecialEffectTips((byte)eType2);
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	private static void ChangeMobility(DataContext context, CombatCharacter affectChar, int param)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		CValuePercent val = CValuePercent.op_Implicit(param);
		DomainManager.Combat.ChangeMobilityValue(context, affectChar, MoveSpecialConstants.MaxMobility * val);
		DomainManager.Combat.UpdateSkillCanUse(context, affectChar);
	}

	private static void ChangeBreath(DataContext context, CombatCharacter affectChar, int param)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		CValuePercent val = CValuePercent.op_Implicit(param);
		DomainManager.Combat.ChangeBreathValue(context, affectChar, 30000 * val);
		DomainManager.Combat.UpdateSkillCanUse(context, affectChar);
	}

	private static void ChangeStance(DataContext context, CombatCharacter affectChar, int param)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		CValuePercent val = CValuePercent.op_Implicit(param);
		DomainManager.Combat.ChangeStanceValue(context, affectChar, 4000 * val);
		DomainManager.Combat.UpdateSkillCanUse(context, affectChar);
	}
}
