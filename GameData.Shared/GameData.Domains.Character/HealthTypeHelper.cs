using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Combat.Math;

namespace GameData.Domains.Character;

public static class HealthTypeHelper
{
	private static readonly CValuePercent DyingPercent = CValuePercent.op_Implicit(0);

	private static readonly CValuePercent CriticallyIllPercent = CValuePercent.op_Implicit(25);

	private static readonly CValuePercent WeakPercent = CValuePercent.op_Implicit(50);

	private static readonly CValuePercent SickPercent = CValuePercent.op_Implicit(75);

	public static EHealthType CalcType(IEnumerable<short> featureIds, short health, short maxHealth)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (featureIds.Any(IgnoreMark))
		{
			return EHealthType.Unknown;
		}
		if (health < (int)maxHealth * DyingPercent || maxHealth < 0)
		{
			return EHealthType.Dying;
		}
		if (health <= (int)maxHealth * CriticallyIllPercent || maxHealth <= 6)
		{
			return EHealthType.CriticallyIll;
		}
		if (health <= (int)maxHealth * WeakPercent || maxHealth <= 12)
		{
			return EHealthType.Weak;
		}
		if (health <= (int)maxHealth * SickPercent || maxHealth <= 24)
		{
			return EHealthType.Sick;
		}
		return EHealthType.Healthy;
	}

	public static EHealthType CalcType(short health, short maxHealth)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (health < (int)maxHealth * DyingPercent || maxHealth < 0)
		{
			return EHealthType.Dying;
		}
		if (health <= (int)maxHealth * CriticallyIllPercent || maxHealth <= 6)
		{
			return EHealthType.CriticallyIll;
		}
		if (health <= (int)maxHealth * WeakPercent || maxHealth <= 12)
		{
			return EHealthType.Weak;
		}
		if (health <= (int)maxHealth * SickPercent || maxHealth <= 24)
		{
			return EHealthType.Sick;
		}
		return EHealthType.Healthy;
	}

	private static bool IgnoreMark(short featureId)
	{
		return CharacterFeature.Instance[featureId].IgnoreHealthMark;
	}

	public static int ToCommonIndex(this EHealthType healthType)
	{
		return healthType switch
		{
			EHealthType.Healthy => 0, 
			EHealthType.Sick => 1, 
			EHealthType.Weak => 2, 
			EHealthType.CriticallyIll => 3, 
			EHealthType.Dying => 4, 
			_ => -1, 
		};
	}
}
