using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier;

public class Chaofeng : CarrierEffectBase
{
	private const int MobilityRecoverSpeedAddPercent = 50;

	protected override short CombatStateId => 201;

	public Chaofeng(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		CreateAffectedData(197, (EDataModifyType)2, -1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 197)
		{
			return 0;
		}
		return 50;
	}
}
