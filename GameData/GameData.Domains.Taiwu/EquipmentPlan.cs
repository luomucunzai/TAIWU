using System;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu;

[SerializableGameData(NotForDisplayModule = true)]
public class EquipmentPlan : ISerializableGameData
{
	[SerializableGameDataField]
	public readonly ItemKey[] Slots;

	[SerializableGameDataField]
	public readonly sbyte[] WeaponInnerRatios;

	public EquipmentPlan()
	{
		Slots = new ItemKey[12];
		WeaponInnerRatios = new sbyte[3];
		for (int i = 0; i < 12; i++)
		{
			Slots[i] = ItemKey.Invalid;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public unsafe int GetSerializedSize()
	{
		return sizeof(ItemKey) * 12 + 3;
	}

	public unsafe int Serialize(byte* pData)
	{
		for (int i = 0; i < 12; i++)
		{
			((ItemKey*)pData)[i] = Slots[i];
		}
		pData += sizeof(ItemKey) * 12;
		for (int j = 0; j < 3; j++)
		{
			pData[j] = (byte)WeaponInnerRatios[j];
		}
		return GetSerializedSize();
	}

	public unsafe int Deserialize(byte* pData)
	{
		for (int i = 0; i < 12; i++)
		{
			Slots[i] = ((ItemKey*)pData)[i];
		}
		pData += sizeof(ItemKey) * 12;
		for (int j = 0; j < 3; j++)
		{
			WeaponInnerRatios[j] = (sbyte)pData[j];
		}
		return GetSerializedSize();
	}

	public void Record(GameData.Domains.Character.Character character)
	{
		ItemKey[] equipment = character.GetEquipment();
		sbyte[] weaponInnerRatios = DomainManager.Taiwu.GetWeaponInnerRatios();
		for (int i = 0; i < 12; i++)
		{
			Slots[i] = equipment[i];
		}
		for (int j = 0; j < WeaponInnerRatios.Length; j++)
		{
			WeaponInnerRatios[j] = weaponInnerRatios[j];
		}
	}

	public bool Apply(DataContext context, GameData.Domains.Character.Character character, bool skipInvalid)
	{
		Inventory inventory = character.GetInventory();
		ItemKey[] equipment = character.GetEquipment();
		ItemKey[] array = new ItemKey[12];
		Array.Fill(array, ItemKey.Invalid);
		for (int i = 0; i < 12; i++)
		{
			ItemKey itemKey = GetEquipmentAtSlot(i);
			ItemKey itemKey2 = equipment[i];
			if (itemKey2.ItemType == 3 && Config.Clothing.Instance[itemKey2.TemplateId].AgeGroup != 2)
			{
				array[i] = itemKey2;
				continue;
			}
			if (!itemKey.IsValid() && skipInvalid)
			{
				itemKey = itemKey2;
			}
			if (itemKey.IsValid() && array.Exist(itemKey))
			{
				itemKey = ItemKey.Invalid;
			}
			array[i] = itemKey;
		}
		bool result = !CollectionUtils.Equals(equipment, array, 12);
		character.ChangeEquipment(context, array);
		if (character.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
		{
			sbyte[] weaponInnerRatios = DomainManager.Taiwu.GetWeaponInnerRatios();
			for (int j = 0; j < WeaponInnerRatios.Length; j++)
			{
				weaponInnerRatios[j] = WeaponInnerRatios[j];
			}
			DomainManager.Taiwu.SetWeaponInnerRatios(weaponInnerRatios, context);
		}
		return result;
		ItemKey GetEquipmentAtSlot(int index)
		{
			ItemKey itemKey3 = Slots[index];
			if (!itemKey3.IsValid())
			{
				return ItemKey.Invalid;
			}
			if (!DomainManager.Item.ItemExists(itemKey3))
			{
				return ItemKey.Invalid;
			}
			EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey3);
			ItemKey itemKey4 = baseEquipment.GetItemKey();
			if (!inventory.Items.ContainsKey(itemKey4) && !equipment.Exist(itemKey4))
			{
				return ItemKey.Invalid;
			}
			return itemKey4;
		}
	}
}
