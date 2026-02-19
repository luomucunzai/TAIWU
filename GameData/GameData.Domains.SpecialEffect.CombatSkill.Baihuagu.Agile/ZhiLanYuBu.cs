using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Agile;

public class ZhiLanYuBu : AgileSkillBase
{
	private const sbyte HealInjuryCount = 2;

	private bool _affecting;

	private List<(sbyte, bool, sbyte)> _injuryRandomPool;

	public ZhiLanYuBu()
	{
	}

	public ZhiLanYuBu(CombatSkillKey skillKey)
		: base(skillKey, 3406)
	{
		ListenCanAffectChange = true;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_DistanceChanged(OnDistanceChanged);
		_affecting = false;
		OnMoveSkillCanAffectChanged(context, default(DataUid));
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_DistanceChanged(OnDistanceChanged);
		DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
	}

	private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
	{
		if (mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !_affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar))
		{
			return;
		}
		Injuries injuries = base.CombatChar.GetInjuries().Subtract(base.CombatChar.GetOldInjuries());
		if (_injuryRandomPool == null)
		{
			_injuryRandomPool = new List<(sbyte, bool, sbyte)>();
		}
		_injuryRandomPool.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b);
			if (tuple.Item1 > 0)
			{
				_injuryRandomPool.Add((b, false, tuple.Item1));
			}
			if (tuple.Item2 > 0)
			{
				_injuryRandomPool.Add((b, true, tuple.Item2));
			}
		}
		if (_injuryRandomPool.Count > 0)
		{
			(sbyte, bool, sbyte) tuple2 = _injuryRandomPool[context.Random.Next(0, _injuryRandomPool.Count)];
			DomainManager.Combat.RemoveInjury(context, base.CombatChar, tuple2.Item1, tuple2.Item2, Math.Min(2, tuple2.Item3), updateDefeatMark: true);
			ShowSpecialEffectTips(0);
		}
	}

	protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
	{
		bool canAffect = base.CanAffect;
		if (_affecting != canAffect)
		{
			_affecting = canAffect;
			if (canAffect)
			{
				DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId, base.IsDirect);
			}
			else
			{
				DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
			}
		}
	}
}
