using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

public class TrickBuffFlaw : CombatSkillEffectBase
{
	private const sbyte DirectTrickUnit = 3;

	protected sbyte RequireTrickType;

	public TrickBuffFlaw()
	{
	}

	public TrickBuffFlaw(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		Events.RegisterHandler_AttackSkillAttackBegin(OnAttackSkillAttackBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_AttackSkillAttackBegin(OnAttackSkillAttackBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnAttackSkillAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool hit)
	{
		if (attacker.GetId() == base.CharacterId && skillId == base.SkillTemplateId && hit && index >= 3 && base.IsDirect && CombatCharPowerMatchAffectRequire())
		{
			int num = CalcTrickCount(attacker.GetTricks());
			int num2 = num / 3;
			if (num2 > 0)
			{
				DamageCompareData damageCompareData = DomainManager.Combat.GetDamageCompareData();
				int hitValue = (int)Math.Clamp((long)damageCompareData.HitValue[attacker.SkillFinalAttackHitIndex] * (long)attacker.GetAttackSkillPower() / 100, 0L, 2147483647L);
				int avoidValue = damageCompareData.AvoidValue[attacker.SkillFinalAttackHitIndex];
				int hitOdds = CFormula.FormulaCalcHitOdds(hitValue, avoidValue);
				int num3 = CFormula.CalcFlawOrAcupointLevel(hitOdds, isFlaw: true) + 1;
				DomainManager.Combat.AddFlaw(context, defender, (sbyte)num3, SkillKey, attacker.SkillAttackBodyPart, num2);
				ShowSpecialEffectTips(0);
			}
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power) && !base.IsDirect)
		{
			sbyte[] weaponTricks = base.CombatChar.GetWeaponTricks();
			int num = 0;
			sbyte[] array = weaponTricks;
			foreach (sbyte b in array)
			{
				if (b == RequireTrickType)
				{
					num++;
				}
			}
			if (OnReverseAffect(context, num))
			{
				ShowSpecialEffectTips(0);
			}
		}
		RemoveSelf(context);
	}

	protected int CalcTrickCount(TrickCollection trickCollection)
	{
		IReadOnlyDictionary<int, sbyte> tricks = trickCollection.Tricks;
		int num = 0;
		foreach (sbyte value in tricks.Values)
		{
			if (value == RequireTrickType)
			{
				num++;
			}
		}
		return num;
	}

	protected virtual bool OnReverseAffect(DataContext context, int trickCount)
	{
		return false;
	}
}
