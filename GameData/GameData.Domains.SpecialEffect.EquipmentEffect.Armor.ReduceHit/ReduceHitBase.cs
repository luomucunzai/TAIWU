using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit;

public abstract class ReduceHitBase : EquipmentEffectBase
{
	private const sbyte ReducePercent = -20;

	protected abstract bool IsRequireWeaponSubType(short weaponSubType);

	protected ReduceHitBase()
	{
	}

	protected ReduceHitBase(int charId, ItemKey itemKey, int type)
		: base(charId, itemKey, type)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		for (sbyte b = 0; b < 4; b++)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(90 + b), -1), (EDataModifyType)1);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !IsCurrArmor((sbyte)dataKey.CustomParam2))
		{
			return 0;
		}
		ItemKey usingWeaponKey = DomainManager.Combat.GetUsingWeaponKey(base.CurrEnemyChar);
		short itemSubType = DomainManager.Item.GetElement_Weapons(usingWeaponKey.Id).GetItemSubType();
		if (!IsRequireWeaponSubType(itemSubType))
		{
			return 0;
		}
		return -20;
	}
}
