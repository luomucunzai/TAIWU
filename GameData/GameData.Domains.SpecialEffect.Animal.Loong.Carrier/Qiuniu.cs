using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier;

public class Qiuniu : CarrierEffectBase
{
	private static readonly CValuePercent AddOtherPercent = CValuePercent.op_Implicit(33);

	protected override short CombatStateId => 199;

	public Qiuniu(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		base.OnEnableSubClass(context);
		CreateAffectedData(277, (EDataModifyType)0, -1);
		CreateAffectedData(278, (EDataModifyType)0, -1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		bool flag = dataKey.CharId != base.CharacterId;
		bool flag2 = flag;
		if (!flag2)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag3 = (uint)(fieldId - 277) <= 1u;
			flag2 = !flag3;
		}
		if (flag2)
		{
			return 0;
		}
		sbyte b = (sbyte)dataKey.CustomParam0;
		if (b != 3)
		{
			return 0;
		}
		int customParam = dataKey.CustomParam1;
		return customParam * AddOtherPercent;
	}
}
