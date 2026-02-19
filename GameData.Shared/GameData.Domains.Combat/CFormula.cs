using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Domains.Character;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat;

public static class CFormula
{
	public enum EAttackType
	{
		Normal,
		Unlock,
		Spirit,
		Skill,
		MindSkill
	}

	private static int BaseCriticalOdds => GlobalConfig.Instance.BaseCriticalOdds;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FormulaCalcHitOdds(int hitValue, int avoidValue)
	{
		return (int)Math.Clamp((long)hitValue * 100L / Math.Max(avoidValue, 1) / ((hitValue >= avoidValue) ? 1 : 2), 0L, 2147483647L);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FormulaCalcPursueOdds(int weaponFactor, CValuePercent weaponPower, int pursueCount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return weaponFactor * weaponPower * (200 - pursueCount * 10 - pursueCount * pursueCount * 5) / 100;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FormulaCalcCriticalOdds(int hitOdds)
	{
		return 10 + hitOdds / 100;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FormulaCalcCriticalPercent(int hitOdds)
	{
		return 125 + hitOdds / (50 + BaseCriticalOdds * hitOdds / 10000);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FormulaCalcStrike(int penetrate, int penetrateResist, int penetrateRatio)
	{
		long num = (long)penetrate * 100L;
		penetrateResist = penetrateResist * penetrateRatio / 100;
		return (int)Math.Clamp(num / Math.Max(penetrateResist, 1) / ((penetrate >= penetrateResist) ? 1 : 2), 0L, 2147483647L);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FormulaCalcDamageValue(long damageValue, long strike, long penetrateRatio, long attackOdds)
	{
		return (int)Math.Clamp(damageValue * penetrateRatio / 100 * (100 + strike * 100 / (100 + attackOdds * strike / 10000)) / 100, 0L, 2147483647L);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FormulaCalcOneDamageValue(int damageValue, int penetrate, int penetrateResist, int penetrateRatio, int attackOdds)
	{
		int num = FormulaCalcStrike(penetrate, penetrateResist, penetrateRatio);
		return FormulaCalcDamageValue(damageValue, num, penetrateRatio, attackOdds);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FormulaCalcWeaponArmorFactor(int factor, int attack, int defense)
	{
		if (attack <= defense || factor <= 0)
		{
			return factor;
		}
		return factor - factor * Math.Min(20 + 10 * attack / Math.Max(defense, 1), 100) / 100;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int FormulaCalcRopeHitOdds(sbyte baseHitOdds, sbyte requireMarkCount, int totalHit, int totalAvoid, int bonus, int markCount)
	{
		return totalHit * (baseHitOdds + (markCount - requireMarkCount) * baseHitOdds / 2) / totalAvoid * (100 + bonus) / 100;
	}

	public static int CalcBreathRecoverValue(CValuePercent breathRecoverSpeed)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		sbyte recoverBreathBaseValue = GlobalConfig.Instance.RecoverBreathBaseValue;
		return recoverBreathBaseValue + (int)recoverBreathBaseValue * breathRecoverSpeed / 4;
	}

	public static int CalcStanceRecoverValue(CValuePercent stanceRecoverSpeed, int weaponAddValue, sbyte attackPreparePointCost, bool isPursue)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		sbyte recoverStanceBaseValue = GlobalConfig.Instance.RecoverStanceBaseValue;
		int num = (isPursue ? GlobalConfig.Instance.RecoverStanceDivisorByWeapon[attackPreparePointCost] : 4);
		return recoverStanceBaseValue + weaponAddValue + weaponAddValue * stanceRecoverSpeed / num;
	}

	public static int CalcSkillPrepareSpeed(int skillPrepareSpeed)
	{
		return 60 + MathUtils.Max(skillPrepareSpeed, 0) * 30 / 100;
	}

	public static int CalcAttackStartupOrRecoveryWeaponFrame(int weight, int baseWeight, int baseFrame)
	{
		return baseFrame * (50 + MathUtils.Max(weight, 1) * 50 / MathUtils.Max(baseWeight, 1)) / 100;
	}

	public static int CalcAttackStartupOrRecoveryFrame(int attackSpeed, int weaponFrame)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		CValuePercent val = CValuePercent.op_Implicit((int)GlobalConfig.Instance.AttackSpeedFactor);
		return MathUtils.Max((attackSpeed >= 100) ? (weaponFrame - weaponFrame * attackSpeed * val / 1000) : (weaponFrame + weaponFrame * (100 - attackSpeed) * val / 100), 1);
	}

	public static int CalcWeaponCdFrameSpeed(int switchSpeed, int weight)
	{
		int num = GlobalConfig.Instance.WeaponCdExtraWeight + weight;
		return 10 + 500 * switchSpeed / 100 / MathUtils.Max(50 + num / 10, 1);
	}

	public static int CalcItemMaxUsePower(int switchSpeed)
	{
		return 80 + 10 * switchSpeed / 100;
	}

	public static int CalcNeiliAllocationAutoRecoverProgress(int recoveryOfQiDisorder, int currValue, int originValue)
	{
		if (currValue <= originValue)
		{
			return MathUtils.Max((120 + 12 * recoveryOfQiDisorder / 100) * (100 + (200 - currValue * 200 / MathUtils.Max(originValue, 1))) / 100, 1);
		}
		return MathUtils.Max((60 - 6 * recoveryOfQiDisorder / 100) * currValue / MathUtils.Max(originValue, 1), 1);
	}

	public static int CalcMoveCd(int moveSpeed)
	{
		int moveCdBase = MoveSpecialConstants.MoveCdBase;
		if (moveSpeed < 100)
		{
			return moveCdBase + MoveSpecialConstants.MoveCdFactor * (33 - moveSpeed / 3) / 100;
		}
		return moveCdBase - MoveSpecialConstants.MoveCdFactor * moveSpeed / (MoveSpecialConstants.MoveCdDivisorBase + moveSpeed * MoveSpecialConstants.MoveCdDivisorFactor / 100);
	}

	public static int CalcJumpSpeed(int moveSpeed)
	{
		return 100 + 10 * moveSpeed / 100;
	}

	public static int CalcFlawOrAcupointRecoverSpeed(int recoverSpeed)
	{
		return GlobalConfig.Instance.FlawOrAcupointReduceBaseTime + recoverSpeed / 2;
	}

	public static OuterAndInnerInts FormulaCalcMixedDamageValue(int damageValue, int attackOdds, sbyte innerRatio, int outerPenetrate, int outerPenetrateResist, int innerPenetrate, int innerPenetrateResist)
	{
		return FormulaCalcMixedDamageValue(damageValue, attackOdds, innerRatio, new OuterAndInnerInts(outerPenetrate, innerPenetrate), new OuterAndInnerInts(outerPenetrateResist, innerPenetrateResist));
	}

	public static OuterAndInnerInts FormulaCalcMixedDamageValue(int damageValue, int attackOdds, sbyte innerRatio, OuterAndInnerInts penetrate, OuterAndInnerInts penetrateResist)
	{
		int num = 100 - innerRatio;
		int outer = 0;
		if (num > 0)
		{
			outer = FormulaCalcOneDamageValue(damageValue, penetrate.Outer, penetrateResist.Outer, num, attackOdds);
		}
		int inner = 0;
		if (innerRatio > 0)
		{
			inner = FormulaCalcOneDamageValue(damageValue, penetrate.Inner, penetrateResist.Inner, innerRatio, attackOdds);
		}
		return new OuterAndInnerInts(outer, inner);
	}

	public static CValuePercentBonus CalcConsummateChangeDamagePercent(sbyte attackerConsummate, sbyte defenderConsummate)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		ConsummateLevelItem consummateLevelItem = ConsummateLevel.Instance[attackerConsummate];
		ConsummateLevelItem consummateLevelItem2 = ConsummateLevel.Instance[defenderConsummate];
		if (attackerConsummate > defenderConsummate)
		{
			return CValuePercentBonus.op_Implicit(consummateLevelItem.DamageAddPercent - consummateLevelItem2.DamageAddPercent);
		}
		if (defenderConsummate > attackerConsummate)
		{
			return CValuePercentBonus.op_Implicit(consummateLevelItem2.DamageDecPercent - consummateLevelItem.DamageDecPercent);
		}
		return CValuePercentBonus.op_Implicit(0);
	}

	public static sbyte CalcFlawOrAcupointLevel(int hitOdds, bool isFlaw)
	{
		short[] array = (isFlaw ? GlobalConfig.Instance.FlawLevelRequireHitOdds : GlobalConfig.Instance.AcupointLevelRequireHitOdds);
		for (int num = array.Length - 1; num >= 0; num--)
		{
			if (hitOdds >= array[num])
			{
				return (sbyte)num;
			}
		}
		return -1;
	}

	public static CValuePercentBonus CalcFlawDamageBonus(int flawCount, int extraFlawCount)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		return CValuePercentBonus.op_Implicit((flawCount > 0) ? GlobalConfig.Instance.FlawAddDamagePercent : 0) + CValuePercentBonus.op_Implicit(extraFlawCount * GlobalConfig.Instance.ExtraFlawAddDamagePercent);
	}

	public static int CalcBaseDamageValue(EAttackType type, sbyte attackPointCost)
	{
		switch (type)
		{
		case EAttackType.Normal:
			return GlobalConfig.Instance.BaseAttackDamageValue + GlobalConfig.Instance.AddBaseAttackDamageValue * attackPointCost;
		case EAttackType.Unlock:
			return GlobalConfig.Instance.BaseUnlockDamageValue;
		case EAttackType.Spirit:
			return GlobalConfig.Instance.BaseSpiritDamageValue;
		case EAttackType.Skill:
		case EAttackType.MindSkill:
			return GlobalConfig.Instance.BaseSkillDamageValue;
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
	}

	public static int CalcBaseAttackOdds(EAttackType type)
	{
		return type switch
		{
			EAttackType.Normal => GlobalConfig.Instance.BaseAttackOdds, 
			EAttackType.Unlock => GlobalConfig.Instance.BaseUnlockAttackOdds, 
			EAttackType.Spirit => GlobalConfig.Instance.BaseSpiritAttackOdds, 
			EAttackType.Skill => GlobalConfig.Instance.BaseSkillAttackOdds, 
			EAttackType.MindSkill => GlobalConfig.Instance.BaseMindAttackOdds, 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
	}

	public static int CalcInfinityMindMarkProgress(int existCount)
	{
		return MathUtils.Min(GlobalConfig.Instance.InfinityMindMarkProgressBase + existCount * GlobalConfig.Instance.InfinityMindMarkProgressStep, GlobalConfig.Instance.InfinityMindMarkProgressMax);
	}

	public static sbyte CalcRopeRequireMinMarkCount(CombatType combatType)
	{
		return combatType switch
		{
			CombatType.Beat => GlobalConfig.Instance.RopeRequireMinMarkCountInBeat, 
			CombatType.Die => GlobalConfig.Instance.RopeRequireMinMarkCountInDie, 
			_ => sbyte.MaxValue, 
		};
	}

	public static sbyte CalcRopeBaseHitOdds(CombatType combatType)
	{
		return combatType switch
		{
			CombatType.Beat => GlobalConfig.Instance.RopeBaseHitOddsInBeat, 
			CombatType.Die => GlobalConfig.Instance.RopeBaseHitOddsInDie, 
			_ => sbyte.MinValue, 
		};
	}

	public static byte CalcMobilityLevel(int mobilityValue)
	{
		if (mobilityValue <= MoveSpecialConstants.MaxMobility * 25 / 100)
		{
			return 0;
		}
		if (mobilityValue < MoveSpecialConstants.MaxMobility * 75 / 100)
		{
			return 1;
		}
		return 2;
	}

	public static int CalcHealInjuryValue(int doctorAttainment, int injuryStep, int injuryCount, out int requireAttainment)
	{
		requireAttainment = ((injuryCount > 0) ? GlobalConfig.Instance.HealInjuryAttainment[injuryCount - 1] : 0);
		if (doctorAttainment < requireAttainment)
		{
			return 0;
		}
		return injuryStep + injuryStep * (doctorAttainment - requireAttainment) / 100;
	}

	public static int CalcHealPoisonValue(int doctorAttainment, int poisonLevel, out int requireAttainment)
	{
		requireAttainment = ((poisonLevel > 0) ? GlobalConfig.Instance.HealPoisonAttainment[poisonLevel - 1] : 0);
		if (doctorAttainment < requireAttainment)
		{
			return 0;
		}
		return doctorAttainment * GlobalConfig.Instance.HealPoisonAttainmentPercent / 100;
	}

	public static int CalcHealQiDisorderValue(int doctorAttainment, sbyte qiDisorderLevel)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		int num = ((qiDisorderLevel > 0) ? GlobalConfig.Instance.HealQiDisorderAttainment[qiDisorderLevel - 1] : 0);
		if (doctorAttainment < num)
		{
			return 0;
		}
		CValuePercent val = CValuePercent.op_Implicit(GlobalConfig.Instance.HealQiDisorderAttainmentPercent);
		return doctorAttainment * val;
	}

	public static int CalcHealHealthValue(int doctorAttainment, EHealthType healthType)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		int num;
		switch (healthType)
		{
		case EHealthType.Unknown:
			return 0;
		case EHealthType.Sick:
			num = GlobalConfig.Instance.HealHealthAttainment[0];
			break;
		case EHealthType.Weak:
			num = GlobalConfig.Instance.HealHealthAttainment[1];
			break;
		case EHealthType.CriticallyIll:
			num = GlobalConfig.Instance.HealHealthAttainment[2];
			break;
		case EHealthType.Dying:
			num = GlobalConfig.Instance.HealHealthAttainment[3];
			break;
		default:
			num = 0;
			break;
		}
		int num2 = num;
		if (doctorAttainment < num2)
		{
			return 0;
		}
		CValuePercent val = CValuePercent.op_Implicit(GlobalConfig.Instance.HealHealthAttainmentPercent);
		return doctorAttainment * val;
	}

	public static int CalcPartRepairDurabilityValue(sbyte grade, int attainment, int current, int costed)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		short num = GlobalConfig.Instance.RepairAttainments[grade];
		if (current > 0)
		{
			num /= 2;
		}
		CValuePercent val = CValuePercent.op_Implicit(attainment * 50 / MathUtils.Max(num, 1));
		return MathUtils.Max(costed * val, 1);
	}

	public static int CalcMixPoisonAffectCount(int poisonMarkCount)
	{
		return MathUtils.Clamp((poisonMarkCount - 1) / 2, 1, 4);
	}

	public static int CalcMainCharacterWisdomMultiplier(int teammateCharCount)
	{
		return 4 - teammateCharCount;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static short RandomCalcDisorderOfQiDelta(IRandomSource random, int delta)
	{
		return (short)(delta * random.Next(GlobalConfig.Instance.TeaWineEffectDisorderOfQiDelta[0], GlobalConfig.Instance.TeaWineEffectDisorderOfQiDelta[1] + 1) / 100);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static (int max, int min) RandomCalcDisorderOfQiRange(int delta)
	{
		int val = delta * GlobalConfig.Instance.TeaWineEffectDisorderOfQiDelta[0] / 100;
		int val2 = delta * GlobalConfig.Instance.TeaWineEffectDisorderOfQiDelta[1] / 100;
		return (max: Math.Max(val2, val), min: Math.Min(val2, val));
	}
}
