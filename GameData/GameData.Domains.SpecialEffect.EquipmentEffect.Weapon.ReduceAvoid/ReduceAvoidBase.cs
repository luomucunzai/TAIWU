using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid;

public abstract class ReduceAvoidBase : EquipmentEffectBase
{
	private const sbyte ReducePercent = -20;

	protected abstract bool IsRequireWeaponSubType(short weaponSubType);

	protected ReduceAvoidBase()
	{
	}

	protected ReduceAvoidBase(int charId, ItemKey itemKey, int type)
		: base(charId, itemKey, type)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		for (sbyte b = 0; b < 4; b++)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(60 + b), -1), (EDataModifyType)1);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !IsCurrWeapon())
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
