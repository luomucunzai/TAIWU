using System;
using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Extra;

[Obsolete]
public class MerchantBuyBackDebt : ISerializableGameData
{
	[SerializableGameDataField]
	public readonly Dictionary<ItemKey, int> Items = new Dictionary<ItemKey, int>();

	public bool Contains(ItemKey key)
	{
		return Items.ContainsKey(key);
	}

	public void AddItem(ItemKey itemKey, int amount)
	{
		if (Items.TryGetValue(itemKey, out var value))
		{
			Items[itemKey] = value + amount;
		}
		else
		{
			Items.Add(itemKey, amount);
		}
	}

	public void RemoveItem(ItemKey itemKey, int amount)
	{
		int num = Items[itemKey] - amount;
		if (num > 0)
		{
			Items[itemKey] = num;
			return;
		}
		if (num == 0)
		{
			Items.Remove(itemKey);
			return;
		}
		throw new Exception($"Item amount cannot be negative after removing: {itemKey}, {amount}");
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		return 4 + 12 * Items.Count;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Items.Count;
		ptr += 4;
		foreach (KeyValuePair<ItemKey, int> item in Items)
		{
			ptr += item.Key.Serialize(ptr);
			*(int*)ptr = item.Value;
			ptr += 4;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
		Items.Clear();
		for (int i = 0; i < num; i++)
		{
			ItemKey key = default(ItemKey);
			ptr += key.Deserialize(ptr);
			int value = *(int*)ptr;
			ptr += 4;
			Items.Add(key, value);
		}
		return (int)(ptr - pData);
	}
}
