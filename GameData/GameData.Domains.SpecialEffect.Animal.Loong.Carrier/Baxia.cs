using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier;

public class Baxia : CarrierEffectBase
{
	private const int WeightUnit = 500;

	private const int MaxPowerUnit = 1;

	protected override short CombatStateId => 204;

	public Baxia(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		CreateAffectedData(27, (EDataModifyType)0, -1);
		CreateAffectedData(30, (EDataModifyType)0, -1);
		CreateAffectedData(279, (EDataModifyType)3, -1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		bool flag = dataKey.CharId != base.CharacterId;
		bool flag2 = flag;
		if (!flag2)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag3 = ((fieldId == 27 || fieldId == 30) ? true : false);
			flag2 = !flag3;
		}
		if (flag2)
		{
			return 0;
		}
		int currEquipmentLoad = CharObj.GetCurrEquipmentLoad();
		return currEquipmentLoad / 500;
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 279)
		{
			return dataValue;
		}
		return true;
	}
}
