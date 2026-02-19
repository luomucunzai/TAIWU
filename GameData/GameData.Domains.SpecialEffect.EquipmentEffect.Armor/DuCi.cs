using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor;

public class DuCi : EquipmentEffectBase
{
	private const sbyte AddPercent = 20;

	public DuCi()
	{
	}

	public DuCi(int charId, ItemKey itemKey)
		: base(charId, itemKey, 30117)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 73, -1), (EDataModifyType)1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CustomParam1 != EquipItemKey.ItemType || dataKey.CustomParam2 != EquipItemKey.Id)
		{
			return 0;
		}
		if (dataKey.FieldId == 73)
		{
			return 20;
		}
		return 0;
	}
}
