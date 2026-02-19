using GameData.Combat.Math;

namespace GameData.Domains.Item;

public static class ItemFormula
{
	public static CValuePercent FormulaCalcDurabilityEffect(int currDurability, int maxDurability)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		if (maxDurability <= 0)
		{
			return CValuePercent.op_Implicit(100);
		}
		return CValuePercent.op_Implicit(25 + currDurability * 75 / maxDurability);
	}
}
