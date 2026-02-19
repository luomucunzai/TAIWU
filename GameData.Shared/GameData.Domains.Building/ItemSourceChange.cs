using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Domains.Taiwu;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

public class ItemSourceChange : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte ItemSourceType;

	[SerializableGameDataField]
	public List<ItemKeyAndCount> Items = new List<ItemKeyAndCount>();

	[SerializableGameDataField]
	public Dictionary<ItemKey, int> PriceChanges = new Dictionary<ItemKey, int>();

	public ItemSourceType ItemSourceTypeEnum => (ItemSourceType)ItemSourceType;

	public ItemSourceChange(ItemSourceType type)
	{
		ItemSourceType = (sbyte)type;
	}

	public void AddItem(ItemKey key, int count = 1, int priceChange = 0)
	{
		ChangeItem(key, isAdd: true, count, priceChange);
	}

	public void AddItemList(List<ItemKey> itemList, int priceChange = 0)
	{
		foreach (ItemKey item in itemList)
		{
			AddItem(item, 1, priceChange);
		}
	}

	public void RemoveItem(ItemKey key, int count = 1, int priceChange = 0)
	{
		ChangeItem(key, isAdd: false, count, priceChange);
	}

	public void RemoveItemList(List<ItemKey> itemList, int priceChange = 0)
	{
		foreach (ItemKey item in itemList)
		{
			RemoveItem(item, 1, priceChange);
		}
	}

	private void ChangeItem(ItemKey key, bool isAdd, int count = 1, int priceChange = 0)
	{
		int num = (ItemTemplateHelper.IsMiscResource(key.ItemType, key.TemplateId) ? (-1) : Items.FindIndex((ItemKeyAndCount i) => i.ItemKey.Equals(key)));
		int num2 = ((num >= 0) ? Items[num].Count : 0);
		int num3 = (isAdd ? (num2 + count) : (num2 - count));
		if (num3 == 0)
		{
			if (num >= 0)
			{
				Items.RemoveAt(num);
			}
			PriceChanges.Remove(key);
		}
		else
		{
			if (num < 0)
			{
				Items.Add(new ItemKeyAndCount(key, num3));
			}
			else
			{
				Items[num] = new ItemKeyAndCount(key, num3);
			}
			PriceChanges[key] = priceChange;
		}
	}

	public ItemSourceChange()
	{
	}

	public ItemSourceChange(ItemSourceChange other)
	{
		ItemSourceType = other.ItemSourceType;
		Items = ((other.Items == null) ? null : new List<ItemKeyAndCount>(other.Items));
		PriceChanges = ((other.PriceChanges == null) ? null : new Dictionary<ItemKey, int>(other.PriceChanges));
	}

	public void Assign(ItemSourceChange other)
	{
		ItemSourceType = other.ItemSourceType;
		Items = ((other.Items == null) ? null : new List<ItemKeyAndCount>(other.Items));
		PriceChanges = ((other.PriceChanges == null) ? null : new Dictionary<ItemKey, int>(other.PriceChanges));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 1;
		num = ((Items == null) ? (num + 2) : (num + (2 + 12 * Items.Count)));
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(PriceChanges);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)ItemSourceType;
		ptr++;
		if (Items != null)
		{
			int count = Items.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += Items[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(ptr, ref PriceChanges);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ItemSourceType = (sbyte)(*ptr);
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Items == null)
			{
				Items = new List<ItemKeyAndCount>(num);
			}
			else
			{
				Items.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ItemKeyAndCount item = default(ItemKeyAndCount);
				ptr += item.Deserialize(ptr);
				Items.Add(item);
			}
		}
		else
		{
			Items?.Clear();
		}
		ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(ptr, ref PriceChanges);
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
