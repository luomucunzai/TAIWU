using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg;

public class AoWangShenTui : CombatSkillEffectBase
{
	private const string ParticleName = "Particle_Effect_BringCloserCenter";

	private const int ChangeDistanceProgressPercent = 2;

	private const int CenterDamageProgressPercent = 10;

	private const int CenterDamageRange = 1;

	private const int CenterDamageBase = 100;

	private const int CenterDamageDivisor = 2;

	private const int FlawOrAcupointOdds = 50;

	private const sbyte FlawOrAcupointLevel = 1;

	private int _lastUpdateProgressPercent;

	public AoWangShenTui()
	{
	}

	public AoWangShenTui(CombatSkillKey skillKey)
		: base(skillKey, 5107, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_PrepareSkillProgressChange(OnPrepareSkillProgressChange);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_PrepareSkillProgressChange(OnPrepareSkillProgressChange);
		base.OnDisable(context);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (SkillKey.IsMatch(charId, skillId))
		{
			_lastUpdateProgressPercent = 0;
			base.CombatChar.SetParticleToLoopByCombatSkill("Particle_Effect_BringCloserCenter", context);
		}
	}

	private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (SkillKey.IsMatch(charId, skillId))
		{
			base.CombatChar.SetParticleToLoopByCombatSkill(null, context);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (SkillKey.IsMatch(charId, skillId))
		{
			_lastUpdateProgressPercent = 0;
			base.CombatChar.SetParticleToLoopByCombatSkill(null, context);
		}
	}

	private void OnPrepareSkillProgressChange(DataContext context, int charId, bool isAlly, short skillId, sbyte preparePercent)
	{
		if (SkillKey.IsMatch(charId, skillId))
		{
			int num = preparePercent - _lastUpdateProgressPercent;
			while (num >= 2 && !IsInAttackRangeCenter())
			{
				num -= 2;
				DoChangeDistance(context);
			}
			while (num >= 10 && IsInAttackRangeCenter())
			{
				num -= 10;
				DoCenterDamage(context);
			}
			_lastUpdateProgressPercent = preparePercent - num;
		}
	}

	private int CalcRangeCenter()
	{
		OuterAndInnerInts skillAttackRange = DomainManager.Combat.GetSkillAttackRange(base.CombatChar, base.SkillTemplateId);
		return (skillAttackRange.Outer + skillAttackRange.Inner) / 2;
	}

	private bool IsInAttackRangeCenter()
	{
		int num = CalcRangeCenter();
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		return Math.Abs(currentDistance - num) <= 1;
	}

	private void DoChangeDistance(DataContext context)
	{
		int num = CalcRangeCenter();
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		if (num != currentDistance)
		{
			int addDistance = ((num >= currentDistance) ? 1 : (-1));
			DomainManager.Combat.ChangeDistance(context, base.EnemyChar, addDistance, isForced: true);
		}
	}

	private void DoCenterDamage(DataContext context)
	{
		short num = (base.IsDirect ? CharObj.GetRecoveryOfFlaw() : CharObj.GetRecoveryOfBlockedAcupoint());
		int num2 = 100 + num / 2;
		sbyte bodyPart = base.EnemyChar.RandomInjuryBodyPartMustValid(context.Random, !base.IsDirect);
		int num3 = DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, base.EnemyChar, bodyPart, base.IsDirect ? num2 : 0, (!base.IsDirect) ? num2 : 0, base.SkillTemplateId);
		for (int i = 0; i < num3; i++)
		{
			if (context.Random.CheckPercentProb(50))
			{
				if (base.IsDirect)
				{
					DomainManager.Combat.AddFlaw(context, base.EnemyChar, 1, SkillKey, -1);
				}
				else
				{
					DomainManager.Combat.AddAcupoint(context, base.EnemyChar, 1, SkillKey, -1);
				}
			}
		}
		ShowSpecialEffectTips(0);
	}
}
