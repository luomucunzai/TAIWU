using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReducePenetrate;

public class ReducePenetrateBase : EquipmentEffectBase
{
	private const sbyte ReducePercent = -20;

	protected sbyte RequireWeaponResourceType;

	protected ReducePenetrateBase()
	{
	}

	protected ReducePenetrateBase(int charId, ItemKey itemKey, int type)
		: base(charId, itemKey, type)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 98, -1), (EDataModifyType)1);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 99, -1), (EDataModifyType)1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !IsCurrArmor((sbyte)dataKey.CustomParam1))
		{
			return 0;
		}
		ItemKey usingWeaponKey = DomainManager.Combat.GetUsingWeaponKey(base.CurrEnemyChar);
		if (DomainManager.Item.GetBaseItem(usingWeaponKey).GetResourceType() != RequireWeaponResourceType)
		{
			return 0;
		}
		return -20;
	}
}
