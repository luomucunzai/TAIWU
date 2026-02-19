using System;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character;

[SerializableGameData(NotForDisplayModule = true)]
public class Grave : BaseGameDataObject, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 4;

		public const uint Location_Offset = 4u;

		public const int Location_Size = 4;

		public const uint Level_Offset = 8u;

		public const int Level_Size = 1;

		public const uint Durability_Offset = 9u;

		public const int Durability_Size = 2;

		public const uint Resources_Offset = 11u;

		public const int Resources_Size = 32;

		public const uint SkeletonCharId_Offset = 43u;

		public const int SkeletonCharId_Size = 4;
	}

	[CollectionObjectField(false, true, false, true, false)]
	private int _id;

	[CollectionObjectField(false, true, false, false, false)]
	private Location _location;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _level;

	[CollectionObjectField(false, true, false, false, false)]
	private short _durability;

	[CollectionObjectField(false, true, false, false, false)]
	private Inventory _inventory;

	[CollectionObjectField(false, true, false, false, false)]
	private ResourceInts _resources;

	[CollectionObjectField(false, true, false, false, false)]
	private int _skeletonCharId;

	public const int FixedSize = 47;

	public const int DynamicCount = 1;

	public Grave(DataContext context, Character character, Location location, sbyte level)
	{
		_id = character.GetId();
		_location = location;
		_level = level;
		_durability = GlobalConfig.Instance.GraveDurabilities[_level];
		if (character.GetCreatingType() == 1)
		{
			_inventory = character.GetInventory();
			foreach (ItemKey key in _inventory.Items.Keys)
			{
				ItemBase baseItem = DomainManager.Item.GetBaseItem(key);
				baseItem.RemoveOwner(ItemOwnerType.CharacterInventory, _id);
				baseItem.SetOwner(ItemOwnerType.Grave, _id);
			}
			PutEquipmentIntoInventory(character.GetEquipment(), _inventory);
		}
		else
		{
			_inventory = character.GetInventory();
			foreach (ItemKey key2 in _inventory.Items.Keys)
			{
				ItemBase baseItem2 = DomainManager.Item.GetBaseItem(key2);
				baseItem2.RemoveOwner(ItemOwnerType.CharacterInventory, _id);
				baseItem2.SetOwner(ItemOwnerType.Grave, _id);
			}
			character.SetInventory(new Inventory(), context);
		}
		RemoveEatingItems(context, ref character.GetEatingItems());
		_resources = character.GetResources();
		_skeletonCharId = -1;
	}

	public void RemoveInventoryItem(DataContext context, ItemKey itemKey, int amount, bool deleteItem = false)
	{
		if (amount > 0)
		{
			DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Grave, _id);
			_inventory.OfflineRemove(itemKey, amount);
			SetInventory(_inventory, context);
			if (deleteItem)
			{
				DomainManager.Item.RemoveItem(context, itemKey);
			}
		}
	}

	public static sbyte CalcGraveLevel(int moneyAvailable)
	{
		if (moneyAvailable < GlobalConfig.Instance.GraveLevelMoneyCosts[1])
		{
			return 0;
		}
		if (moneyAvailable < GlobalConfig.Instance.GraveLevelMoneyCosts[2])
		{
			return 1;
		}
		if (moneyAvailable < GlobalConfig.Instance.GraveLevelMoneyCosts[3])
		{
			return 2;
		}
		return 3;
	}

	private void PutEquipmentIntoInventory(ItemKey[] equipment, Inventory inventory)
	{
		for (int i = 0; i < 12; i++)
		{
			ItemKey itemKey = equipment[i];
			if (itemKey.IsValid())
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
				baseEquipment.RemoveOwner(ItemOwnerType.CharacterEquipment, _id);
				baseEquipment.SetOwner(ItemOwnerType.Grave, _id);
				inventory.Items.Add(itemKey, 1);
			}
		}
	}

	private unsafe static void RemoveEatingItems(DataContext context, ref EatingItems eatingItems)
	{
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[i];
			if (itemKey.IsValid())
			{
				DomainManager.Item.RemoveItem(context, itemKey);
			}
		}
	}

	public int GetId()
	{
		return _id;
	}

	public Location GetLocation()
	{
		return _location;
	}

	public unsafe void SetLocation(Location location, DataContext context)
	{
		_location = location;
		SetModifiedAndInvalidateInfluencedCache(1, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 4u, 4);
			ptr += _location.Serialize(ptr);
		}
	}

	public sbyte GetLevel()
	{
		return _level;
	}

	public unsafe void SetLevel(sbyte level, DataContext context)
	{
		_level = level;
		SetModifiedAndInvalidateInfluencedCache(2, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 8u, 1);
			*ptr = (byte)_level;
			ptr++;
		}
	}

	public short GetDurability()
	{
		return _durability;
	}

	public unsafe void SetDurability(short durability, DataContext context)
	{
		_durability = durability;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 9u, 2);
			*(short*)ptr = _durability;
			ptr += 2;
		}
	}

	public Inventory GetInventory()
	{
		return _inventory;
	}

	public unsafe void SetInventory(Inventory inventory, DataContext context)
	{
		_inventory = inventory;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _inventory.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 0, serializedSize);
			ptr += _inventory.Serialize(ptr);
		}
	}

	public ref ResourceInts GetResources()
	{
		return ref _resources;
	}

	public unsafe void SetResources(ref ResourceInts resources, DataContext context)
	{
		_resources = resources;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 11u, 32);
			ptr += _resources.Serialize(ptr);
		}
	}

	public int GetSkeletonCharId()
	{
		return _skeletonCharId;
	}

	public unsafe void SetSkeletonCharId(int skeletonCharId, DataContext context)
	{
		_skeletonCharId = skeletonCharId;
		SetModifiedAndInvalidateInfluencedCache(6, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 43u, 4);
			*(int*)ptr = _skeletonCharId;
			ptr += 4;
		}
	}

	public Grave()
	{
		_inventory = new Inventory();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 51;
		int serializedSize = _inventory.GetSerializedSize();
		return num + serializedSize;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _id;
		ptr += 4;
		ptr += _location.Serialize(ptr);
		*ptr = (byte)_level;
		ptr++;
		*(short*)ptr = _durability;
		ptr += 2;
		ptr += _resources.Serialize(ptr);
		*(int*)ptr = _skeletonCharId;
		ptr += 4;
		byte* ptr2 = ptr;
		ptr += 4;
		ptr += _inventory.Serialize(ptr);
		int num = (int)(ptr - ptr2 - 4);
		if (num > 4194304)
		{
			throw new Exception($"Size of field {"_inventory"} must be less than {4096}KB");
		}
		*(int*)ptr2 = num;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_id = *(int*)ptr;
		ptr += 4;
		ptr += _location.Deserialize(ptr);
		_level = (sbyte)(*ptr);
		ptr++;
		_durability = *(short*)ptr;
		ptr += 2;
		ptr += _resources.Deserialize(ptr);
		_skeletonCharId = *(int*)ptr;
		ptr += 4;
		ptr += 4;
		ptr += _inventory.Deserialize(ptr);
		return (int)(ptr - pData);
	}
}
