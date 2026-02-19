using System.Runtime.CompilerServices;
using Config.Common;

namespace Config;

public static class VillagerRoleFormulaImpl
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Calculate(int templateId)
	{
		return VillagerRoleFormula.Instance[templateId].Calculate();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Calculate(int templateId, int arg0)
	{
		return VillagerRoleFormula.Instance[templateId].Calculate(arg0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Calculate(int templateId, int arg0, int arg1)
	{
		return VillagerRoleFormula.Instance[templateId].Calculate(arg0, arg1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Calculate(int templateId, int arg0, int arg1, int arg2)
	{
		return VillagerRoleFormula.Instance[templateId].Calculate(arg0, arg1, arg2);
	}
}
