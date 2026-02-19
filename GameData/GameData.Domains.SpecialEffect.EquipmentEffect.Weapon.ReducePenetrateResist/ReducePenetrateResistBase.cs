using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReducePenetrateResist;

public class ReducePenetrateResistBase : EquipmentEffectBase
{
	private const sbyte ReducePercent = -20;

	protected sbyte RequireArmorResourceType;

	protected ReducePenetrateResistBase()
	{
	}

	protected ReducePenetrateResistBase(int charId, ItemKey itemKey, int type)
		: base(charId, itemKey, type)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 66, -1), (EDataModifyType)1);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 67, -1), (EDataModifyType)1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !IsCurrWeapon() || dataKey.CustomParam1 < 0)
		{
			return 0;
		}
		ItemKey itemKey = base.CurrEnemyChar.Armors[dataKey.CustomParam1];
		if (!itemKey.IsValid())
		{
			return 0;
		}
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		if (baseItem.GetCurrDurability() <= 0 || baseItem.GetResourceType() != RequireArmorResourceType)
		{
			return 0;
		}
		return -20;
	}
}
