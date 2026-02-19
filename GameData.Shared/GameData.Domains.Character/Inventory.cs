using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

public class Inventory : ISerializableGameData
{
	public readonly Dictionary<ItemKey, int> Items;

	public static readonly IReadOnlyDictionary<ItemKey, int> Empty = new Dictionary<ItemKey, int>();

	public int InventoryItemTotalCount => Items.Sum((KeyValuePair<ItemKey, int> i) => i.Value);

	public Inventory()
	{
		Items = new Dictionary<ItemKey, int>();
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
		Items.Clear();
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
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

	public void OfflineAdd(ItemKey itemKey, int amount)
	{
		Tester.Assert(itemKey.IsValid());
		Tester.Assert(amount > 0);
		if (Items.TryGetValue(itemKey, out var value))
		{
			Items[itemKey] = value + amount;
		}
		else
		{
			Items.Add(itemKey, amount);
		}
	}

	public void OfflineAdd(List<ItemKey> keyList)
	{
		Tester.Assert(keyList != null);
		Tester.Assert(keyList.Count > 0);
		foreach (ItemKey key in keyList)
		{
			OfflineAdd(key, 1);
		}
	}

	public void OfflineRemove(ItemKey itemKey, int amount)
	{
		Tester.Assert(itemKey.IsValid());
		Tester.Assert(amount > 0);
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

	public void OfflineRemove(ItemKey itemKey)
	{
		Tester.Assert(itemKey.IsValid());
		Items.Remove(itemKey);
	}

	public void OfflineRemove(List<ItemKey> keyList)
	{
		Tester.Assert(keyList != null);
		Tester.Assert(keyList.Count > 0);
		foreach (ItemKey key in keyList)
		{
			OfflineRemove(key, 1);
		}
	}

	public int GetInventoryItemCount(ItemKey itemKey)
	{
		Tester.Assert(itemKey.IsValid());
		Items.TryGetValue(itemKey, out var value);
		return value;
	}

	public int GetInventoryItemCount(sbyte itemType, short itemTemplateId)
	{
		Tester.Assert(itemTemplateId >= 0);
		int num = 0;
		foreach (KeyValuePair<ItemKey, int> item in Items)
		{
			if (item.Key.ItemType == itemType && item.Key.TemplateId == itemTemplateId)
			{
				num += Items[item.Key];
			}
		}
		return num;
	}

	public int GetInventoryItemTypeCount(sbyte itemType)
	{
		int num = 0;
		foreach (KeyValuePair<ItemKey, int> item in Items)
		{
			if (item.Key.ItemType == itemType)
			{
				num += Items[item.Key];
			}
		}
		return num;
	}

	public ItemKey GetInventoryItemKey(sbyte itemType, short itemTemplateId = -1)
	{
		Tester.Assert(itemTemplateId >= 0);
		foreach (ItemKey key in Items.Keys)
		{
			if (key.ItemType == itemType && (itemTemplateId == -1 || key.TemplateId == itemTemplateId))
			{
				return key;
			}
		}
		return ItemKey.Invalid;
	}

	public ItemKey GetInventoryItemKeyByItemType(short itemType)
	{
		Tester.Assert(itemType >= 0);
		foreach (ItemKey key in Items.Keys)
		{
			if (key.ItemType == itemType)
			{
				return key;
			}
		}
		return ItemKey.Invalid;
	}

	public ItemKey GetInventoryItemKeyByItemSubType(short itemSubType)
	{
		Tester.Assert(itemSubType >= 0);
		foreach (ItemKey key in Items.Keys)
		{
			if (key.HasTemplate && ItemTemplateHelper.GetItemSubType(key.ItemType, key.TemplateId) == itemSubType)
			{
				return key;
			}
		}
		return ItemKey.Invalid;
	}

	public void GetInventoryItemKeyList(sbyte itemType, short itemTemplateId, List<ItemKey> resultList)
	{
		Tester.Assert(itemTemplateId >= 0);
		Tester.Assert(resultList != null);
		foreach (ItemKey key in Items.Keys)
		{
			if (key.ItemType == itemType && key.TemplateId == itemTemplateId)
			{
				resultList.Add(key);
			}
		}
	}

	public bool HasItemInGroup(sbyte itemType, short groupId, sbyte minGrade, sbyte maxGrade, List<ItemKey> resultList = null)
	{
		bool result = false;
		bool flag = resultList != null;
		foreach (ItemKey key in Items.Keys)
		{
			if (key.ItemType != itemType || ItemTemplateHelper.GetGroupId(key.ItemType, key.TemplateId) != groupId)
			{
				continue;
			}
			sbyte grade = ItemTemplateHelper.GetGrade(key.ItemType, key.TemplateId);
			if (grade >= minGrade && grade <= maxGrade)
			{
				result = true;
				if (!flag)
				{
					return true;
				}
				resultList.Add(key);
			}
		}
		return result;
	}

	public bool HasItemInSameGroup(sbyte itemType, short templateId, int minGradeOffset = -9, List<ItemKey> resultList = null)
	{
		Tester.Assert(minGradeOffset <= 0);
		short groupId = ItemTemplateHelper.GetGroupId(itemType, templateId);
		if (groupId < 0)
		{
			return GetInventoryItemKey(itemType, templateId).IsValid();
		}
		sbyte minGrade = (sbyte)(ItemTemplateHelper.GetGrade(itemType, templateId) + minGradeOffset);
		return HasItemInGroup(itemType, groupId, minGrade, 8, resultList);
	}

	public ItemKey GetItemInGroup(sbyte itemType, short groupId, sbyte minGrade, sbyte maxGrade)
	{
		ItemKey result = ItemKey.Invalid;
		int num = -1;
		foreach (ItemKey key in Items.Keys)
		{
			if (key.ItemType == itemType && ItemTemplateHelper.GetGroupId(key.ItemType, key.TemplateId) == groupId)
			{
				sbyte grade = ItemTemplateHelper.GetGrade(key.ItemType, key.TemplateId);
				if (grade == maxGrade)
				{
					return key;
				}
				if (grade >= minGrade && grade <= maxGrade && grade >= num)
				{
					num = grade;
					result = key;
				}
			}
		}
		return result;
	}

	public ItemKey GetItemInSameGroup(sbyte itemType, short templateId, int minGradeOffset = -9)
	{
		Tester.Assert(minGradeOffset <= 0);
		short groupId = ItemTemplateHelper.GetGroupId(itemType, templateId);
		if (groupId < 0)
		{
			return GetInventoryItemKey(itemType, templateId);
		}
		sbyte grade = ItemTemplateHelper.GetGrade(itemType, templateId);
		sbyte minGrade = (sbyte)(grade + minGradeOffset);
		return GetItemInGroup(itemType, groupId, minGrade, grade);
	}

	[Obsolete]
	public void Add(ItemKey itemKey, int amount)
	{
		OfflineAdd(itemKey, amount);
	}

	[Obsolete]
	public void Remove(ItemKey itemKey, int amount)
	{
		OfflineRemove(itemKey, amount);
	}
}
