using GameData.Combat.Math;

namespace GameData.Domains.Combat;

public static class WorsenConstants
{
	public static readonly CValuePercent[] WorsenFatalPercent = (CValuePercent[])(object)new CValuePercent[6]
	{
		CValuePercent.op_Implicit(10),
		CValuePercent.op_Implicit(20),
		CValuePercent.op_Implicit(40),
		CValuePercent.op_Implicit(70),
		CValuePercent.op_Implicit(110),
		CValuePercent.op_Implicit(160)
	};

	public static readonly CValuePercent DefaultPercent = CValuePercent.op_Implicit(80);

	public static readonly CValuePercent LowPercent = CValuePercent.op_Implicit(40);

	public static readonly CValuePercent HighPercent = CValuePercent.op_Implicit(120);

	public static readonly CValuePercent SpecialPercentBaiXie = CValuePercent.op_Implicit(160);

	public static readonly CValuePercent SpecialPercentMingYunWuJianYu = CValuePercent.op_Implicit(240);

	public static readonly CValuePercent SpecialPercentLoongFire = CValuePercent.op_Implicit(160);

	public static CValuePercent CalcPoisonPercent(CValueMultiplier poisonLevel)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return HighPercent * poisonLevel;
	}
}
