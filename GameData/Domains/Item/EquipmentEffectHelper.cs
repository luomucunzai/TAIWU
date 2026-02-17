using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;

namespace GameData.Domains.Item
{
	// Token: 0x0200065C RID: 1628
	public static class EquipmentEffectHelper
	{
		// Token: 0x06004E47 RID: 20039 RVA: 0x002B0460 File Offset: 0x002AE660
		public static int ModifyValue(this IEnumerable<EquipmentEffectItem> effects, int value, EquipmentEffectHelper.ValueSelector selector)
		{
			CValueModify modify = CValueModify.Zero;
			foreach (EquipmentEffectItem effect in effects)
			{
				int factor = selector(effect);
				modify = (effect.IsTotalPercent ? modify.ChangeC(factor) : modify.ChangeB(factor));
			}
			return value * modify;
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x002B04DC File Offset: 0x002AE6DC
		public static EquipmentEffectHelper.ValueSelector GetAvoidFactorSelector(int type)
		{
			if (!true)
			{
			}
			EquipmentEffectHelper.ValueSelector result;
			switch (type)
			{
			case 0:
				result = new EquipmentEffectHelper.ValueSelector(EquipmentEffectHelper.AvoidFactorSelectorStrength);
				break;
			case 1:
				result = new EquipmentEffectHelper.ValueSelector(EquipmentEffectHelper.AvoidFactorSelectorTechnique);
				break;
			case 2:
				result = new EquipmentEffectHelper.ValueSelector(EquipmentEffectHelper.AvoidFactorSelectorSpeed);
				break;
			case 3:
				result = new EquipmentEffectHelper.ValueSelector(EquipmentEffectHelper.AvoidFactorSelectorMind);
				break;
			default:
				result = null;
				break;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004E49 RID: 20041 RVA: 0x002B054C File Offset: 0x002AE74C
		private static int AvoidFactorSelectorStrength(EquipmentEffectItem effect)
		{
			return (int)effect.AvoidFactors[0];
		}

		// Token: 0x06004E4A RID: 20042 RVA: 0x002B0568 File Offset: 0x002AE768
		private static int AvoidFactorSelectorTechnique(EquipmentEffectItem effect)
		{
			return (int)effect.AvoidFactors[1];
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x002B0584 File Offset: 0x002AE784
		private static int AvoidFactorSelectorSpeed(EquipmentEffectItem effect)
		{
			return (int)effect.AvoidFactors[2];
		}

		// Token: 0x06004E4C RID: 20044 RVA: 0x002B05A0 File Offset: 0x002AE7A0
		private static int AvoidFactorSelectorMind(EquipmentEffectItem effect)
		{
			return (int)effect.AvoidFactors[3];
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x002B05BC File Offset: 0x002AE7BC
		public static EquipmentEffectHelper.ValueSelector GetPenetrationResistFactorSelector(bool inner)
		{
			return inner ? new EquipmentEffectHelper.ValueSelector(EquipmentEffectHelper.PenetrationResistFactorSelectorInner) : new EquipmentEffectHelper.ValueSelector(EquipmentEffectHelper.PenetrationResistFactorSelectorOuter);
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x002B05DB File Offset: 0x002AE7DB
		private static int PenetrationResistFactorSelectorInner(EquipmentEffectItem effect)
		{
			return (int)effect.PenetrationResistFactors.Inner;
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x002B05E8 File Offset: 0x002AE7E8
		private static int PenetrationResistFactorSelectorOuter(EquipmentEffectItem effect)
		{
			return (int)effect.PenetrationResistFactors.Outer;
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x002B05F5 File Offset: 0x002AE7F5
		public static EquipmentEffectHelper.ValueSelector GetInjuryFactorSelector(bool inner)
		{
			return inner ? new EquipmentEffectHelper.ValueSelector(EquipmentEffectHelper.InjuryFactorSelectorInner) : new EquipmentEffectHelper.ValueSelector(EquipmentEffectHelper.InjuryFactorSelectorOuter);
		}

		// Token: 0x06004E51 RID: 20049 RVA: 0x002B0614 File Offset: 0x002AE814
		private static int InjuryFactorSelectorInner(EquipmentEffectItem effect)
		{
			return (int)effect.InjuryFactors.Inner;
		}

		// Token: 0x06004E52 RID: 20050 RVA: 0x002B0621 File Offset: 0x002AE821
		private static int InjuryFactorSelectorOuter(EquipmentEffectItem effect)
		{
			return (int)effect.InjuryFactors.Outer;
		}

		// Token: 0x02000AA3 RID: 2723
		// (Invoke) Token: 0x060088A3 RID: 34979
		public delegate int ValueSelector(EquipmentEffectItem effect);
	}
}
