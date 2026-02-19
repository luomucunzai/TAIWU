using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

public struct EatingItems : ISerializableGameData
{
	public const int MinCount = 3;

	public const int MaxCount = 9;

	public unsafe fixed ulong ItemKeys[9];

	public unsafe fixed short Durations[9];

	public unsafe void Initialize()
	{
		for (int i = 0; i < 9; i++)
		{
			ItemKeys[i] = (ulong)ItemKey.Invalid;
			Durations[i] = 0;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 90;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		for (int i = 0; i < 9; i++)
		{
			((long*)ptr)[i] = (long)ItemKeys[i];
		}
		ptr += 72;
		for (int j = 0; j < 9; j++)
		{
			((short*)ptr)[j] = Durations[j];
		}
		return 90;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		for (int i = 0; i < 9; i++)
		{
			ItemKeys[i] = ((ulong*)ptr)[i];
		}
		ptr += 72;
		for (int j = 0; j < 9; j++)
		{
			Durations[j] = ((short*)ptr)[j];
		}
		return 90;
	}

	public static bool IsValid(ItemKey itemKey)
	{
		if (!itemKey.IsValid())
		{
			return itemKey.TemplateId >= 0;
		}
		return true;
	}

	public static bool IsWug(ItemKey itemKey)
	{
		if (itemKey.ItemType == 8)
		{
			return Medicine.Instance[itemKey.TemplateId].ItemSubType == 802;
		}
		return false;
	}

	public static bool IsWugKing(ItemKey itemKey)
	{
		if (IsWug(itemKey))
		{
			return Medicine.Instance[itemKey.TemplateId].WugGrowthType == 5;
		}
		return false;
	}

	public static sbyte CalcMaxEatingSlotsCount(int maxVitality)
	{
		return (sbyte)MathUtils.Clamp(maxVitality / 10, 3, 9);
	}

	public unsafe int GetAvailableEatingSlotsCount(sbyte currMaxEatingSlotsCount)
	{
		Tester.Assert(currMaxEatingSlotsCount <= 9);
		int num = 0;
		for (int i = 0; i < currMaxEatingSlotsCount; i++)
		{
			if (!IsValid((ItemKey)ItemKeys[i]))
			{
				num++;
			}
		}
		return num;
	}

	public unsafe sbyte GetAvailableEatingSlot(sbyte currMaxEatingSlotsCount)
	{
		Tester.Assert(currMaxEatingSlotsCount <= 9);
		for (sbyte b = 0; b < currMaxEatingSlotsCount; b++)
		{
			if (!IsValid((ItemKey)ItemKeys[b]))
			{
				return b;
			}
		}
		return -1;
	}

	public unsafe bool Equals(EatingItems other)
	{
		bool result = true;
		for (int i = 0; i < 9; i++)
		{
			if (!ItemKeys[i].Equals(other.ItemKeys[i]) || Durations[i] != other.Durations[i])
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public unsafe int GetValidCount()
	{
		int num = 0;
		for (int i = 0; i < 9; i++)
		{
			if (IsValid((ItemKey)ItemKeys[i]))
			{
				num++;
			}
		}
		return num;
	}

	public unsafe ItemKey Get(int index)
	{
		if ((index < 0 || index >= 9) ? true : false)
		{
			throw new IndexOutOfRangeException();
		}
		return (ItemKey)ItemKeys[index];
	}

	public unsafe void Set(int index, ItemKey itemKey, short duration)
	{
		Tester.Assert(IsValid(itemKey));
		ItemKey itemKey2 = (ItemKey)ItemKeys[index];
		if (itemKey2.IsValid() && !IsWug(itemKey2))
		{
			throw new Exception("Overwrite item instance");
		}
		ItemKeys[index] = (ulong)itemKey;
		Durations[index] = duration;
	}

	public int GetTotalMedicineEffectValue(EMedicineEffectSubType subType)
	{
		int num = 0;
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = Get(i);
			if (itemKey.ItemType == 8)
			{
				MedicineItem medicineItem = Medicine.Instance[itemKey.TemplateId];
				if (medicineItem.EffectSubType == subType)
				{
					num += medicineItem.EffectValue;
				}
			}
		}
		return num;
	}

	public unsafe bool UpdateDurations(List<ItemKey> itemsToBeRemoved, ref List<short> removedWugs, ref List<ItemKey> removedWugKings)
	{
		bool flag = false;
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)ItemKeys[i];
			if (!IsValid(itemKey))
			{
				continue;
			}
			short num = Durations[i];
			num = (Durations[i] = (short)(num - 1));
			flag = true;
			if (num > 0)
			{
				continue;
			}
			ItemKeys[i] = (ulong)ItemKey.Invalid;
			if (itemKey.IsValid())
			{
				itemsToBeRemoved.Add(itemKey);
			}
			if (IsWug(itemKey))
			{
				if (removedWugs == null)
				{
					removedWugs = new List<short>();
				}
				removedWugs.Add(itemKey.TemplateId);
			}
			if (IsWugKing(itemKey))
			{
				if (removedWugKings == null)
				{
					removedWugKings = new List<ItemKey>();
				}
				removedWugKings.Add(itemKey);
			}
		}
		return SortWugs() || flag;
	}

	public unsafe bool ContainsAny()
	{
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)ItemKeys[i];
			if (itemKey.IsValid() && !IsWug(itemKey))
			{
				return true;
			}
		}
		return false;
	}

	public unsafe bool ContainsWugKing(sbyte wugType)
	{
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)ItemKeys[i];
			if (IsWugKing(itemKey) && Medicine.Instance[itemKey.TemplateId].WugType == wugType)
			{
				return true;
			}
		}
		return false;
	}

	public unsafe int IndexOfWugKing()
	{
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)ItemKeys[i];
			if (itemKey.ItemType == 8 && Medicine.Instance[itemKey.TemplateId].WugGrowthType == 5)
			{
				return i;
			}
		}
		return -1;
	}

	public unsafe sbyte CountOfWugMark()
	{
		int num = 0;
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)ItemKeys[i];
			if (IsWug(itemKey) && !WugGrowthType.IsGood(Medicine.Instance[itemKey.TemplateId].WugGrowthType))
			{
				num++;
			}
		}
		return (sbyte)num;
	}

	public unsafe sbyte GetSlotForNewWug()
	{
		for (sbyte b = 8; b >= 0; b--)
		{
			if (!IsWug((ItemKey)ItemKeys[b]))
			{
				return b;
			}
		}
		return -1;
	}

	public unsafe bool SortWugs()
	{
		bool result = false;
		SpanList<int> spanList = stackalloc int[9];
		spanList.Clear();
		for (int i = 0; i < 9; i++)
		{
			if (IsWug(Get(i)))
			{
				spanList.Push(i);
			}
		}
		int num = 8;
		while (num >= 0 && spanList.Count != 0)
		{
			if (!Get(num).IsValid() || IsWug(Get(num)))
			{
				int num2 = spanList.Pop();
				if (num2 != num)
				{
					Set(num, Get(num2), Durations[num2]);
					Clear(num2);
					result = true;
				}
			}
			num--;
		}
		return result;
	}

	public unsafe void Clear(int index)
	{
		ItemKeys[index] = (ulong)ItemKey.Invalid;
		Durations[index] = 0;
	}
}
