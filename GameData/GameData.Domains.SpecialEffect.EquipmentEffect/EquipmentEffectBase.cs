using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect;

public class EquipmentEffectBase : SpecialEffectBase
{
	public ItemKey EquipItemKey;

	public readonly bool AutoRemoveAfterCombat;

	protected short Durability => DomainManager.Item.GetBaseItem(EquipItemKey).GetCurrDurability();

	protected EquipmentEffectBase()
	{
	}

	protected EquipmentEffectBase(int charId, ItemKey itemKey, int type)
		: base(charId, type)
	{
		EquipItemKey = itemKey;
		AutoRemoveAfterCombat = true;
	}

	protected EquipmentEffectBase(int charId, ItemKey itemKey, int type, bool autoRemoveAfterCombat)
		: base(charId, type)
	{
		EquipItemKey = itemKey;
		AutoRemoveAfterCombat = autoRemoveAfterCombat;
	}

	protected bool IsCurrWeapon()
	{
		return DomainManager.Combat.GetUsingWeaponKey(base.CombatChar).Equals(EquipItemKey);
	}

	protected bool IsCurrArmor(sbyte bodyPart)
	{
		if (bodyPart < 0)
		{
			return false;
		}
		return base.CombatChar.Armors[bodyPart].Equals(EquipItemKey) && Durability > 0;
	}

	protected override int GetSubClassSerializedSize()
	{
		return EquipItemKey.GetSerializedSize();
	}

	protected unsafe override int SerializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		ptr += EquipItemKey.Serialize(ptr);
		return (int)(ptr - pData);
	}

	protected unsafe override int DeserializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		ptr += EquipItemKey.Deserialize(ptr);
		return (int)(ptr - pData);
	}
}
