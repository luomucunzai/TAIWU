using System.Collections.Generic;
using Config;
using GameData.Combat.Math;

namespace GameData.Domains.Item;

public static class EquipmentEffectHelper
{
	public delegate int ValueSelector(EquipmentEffectItem effect);

	public static int ModifyValue(this IEnumerable<EquipmentEffectItem> effects, int value, ValueSelector selector)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		CValueModify val = CValueModify.Zero;
		foreach (EquipmentEffectItem effect in effects)
		{
			int num = selector(effect);
			val = (effect.IsTotalPercent ? ((CValueModify)(ref val)).ChangeC(num) : ((CValueModify)(ref val)).ChangeB(num));
		}
		return value * val;
	}

	public static ValueSelector GetAvoidFactorSelector(int type)
	{
		if (1 == 0)
		{
		}
		ValueSelector result = type switch
		{
			0 => AvoidFactorSelectorStrength, 
			1 => AvoidFactorSelectorTechnique, 
			2 => AvoidFactorSelectorSpeed, 
			3 => AvoidFactorSelectorMind, 
			_ => null, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static int AvoidFactorSelectorStrength(EquipmentEffectItem effect)
	{
		return effect.AvoidFactors[0];
	}

	private static int AvoidFactorSelectorTechnique(EquipmentEffectItem effect)
	{
		return effect.AvoidFactors[1];
	}

	private static int AvoidFactorSelectorSpeed(EquipmentEffectItem effect)
	{
		return effect.AvoidFactors[2];
	}

	private static int AvoidFactorSelectorMind(EquipmentEffectItem effect)
	{
		return effect.AvoidFactors[3];
	}

	public static ValueSelector GetPenetrationResistFactorSelector(bool inner)
	{
		return inner ? new ValueSelector(PenetrationResistFactorSelectorInner) : new ValueSelector(PenetrationResistFactorSelectorOuter);
	}

	private static int PenetrationResistFactorSelectorInner(EquipmentEffectItem effect)
	{
		return effect.PenetrationResistFactors.Inner;
	}

	private static int PenetrationResistFactorSelectorOuter(EquipmentEffectItem effect)
	{
		return effect.PenetrationResistFactors.Outer;
	}

	public static ValueSelector GetInjuryFactorSelector(bool inner)
	{
		return inner ? new ValueSelector(InjuryFactorSelectorInner) : new ValueSelector(InjuryFactorSelectorOuter);
	}

	private static int InjuryFactorSelectorInner(EquipmentEffectItem effect)
	{
		return effect.InjuryFactors.Inner;
	}

	private static int InjuryFactorSelectorOuter(EquipmentEffectItem effect)
	{
		return effect.InjuryFactors.Outer;
	}
}
