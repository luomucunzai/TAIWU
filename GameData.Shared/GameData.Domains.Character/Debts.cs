using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

public class Debts : ISerializableGameData
{
	public long Equivalent;

	public readonly SortedList<long, int> Nonequivalents;

	private static readonly Queue<(long, int)> OnExchange = new Queue<(long, int)>();

	public Debts()
	{
		Nonequivalents = new SortedList<long, int>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		return 12 + 12 * Nonequivalents.Count;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(long*)ptr = Equivalent;
		ptr += 8;
		*(int*)ptr = Nonequivalents.Count;
		ptr += 4;
		foreach (KeyValuePair<long, int> nonequivalent in Nonequivalents)
		{
			long key = nonequivalent.Key;
			int value = nonequivalent.Value;
			*(long*)ptr = key;
			ptr += 8;
			*(int*)ptr = value;
			ptr += 4;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		Nonequivalents.Clear();
		byte* ptr = pData;
		Equivalent = *(long*)ptr;
		ptr += 8;
		int num = *(int*)ptr;
		ptr += 4;
		for (int i = 0; i < num; i++)
		{
			long key = *(long*)ptr;
			ptr += 8;
			int value = *(int*)ptr;
			ptr += 4;
			Nonequivalents.Add(key, value);
		}
		return (int)(ptr - pData);
	}

	public static long ItemToWorth(sbyte itemType, short itemTemplateId)
	{
		return ItemTemplateHelper.GetBaseFavorabilityChange(itemType, itemTemplateId) * 10;
	}

	public static long ResourceAmountToWorth(short resourceType, int amount)
	{
		return GlobalConfig.ResourcesWorth[resourceType] * amount;
	}

	public static long ResourceAmountToWorth(ref ResourceInts resources)
	{
		long num = 0L;
		for (int i = 0; i < 7; i++)
		{
			num += GlobalConfig.ResourcesWorth[i] * resources.Get(i);
		}
		return num;
	}

	public static int WorthToResourceAmount(short resourceType, long worth, bool keepRemainder = false)
	{
		worth = Math.Abs(worth);
		sbyte b = GlobalConfig.ResourcesWorth[resourceType];
		long num = worth / b;
		long num2 = worth % b;
		return (int)Math.Min((keepRemainder && num2 > 0) ? (num + 1) : num, 2147483647L);
	}

	public short GetMaxFavorabilityWithDebt()
	{
		return (short)MathUtils.Clamp(30000 - GetTotalWorthLentToTaiwu() / 10, -30000L, 30000L);
	}

	public void LendEquivalentToTaiwu(long worth)
	{
		if (worth <= 0)
		{
			throw new Exception($"Worth must be greater than zero: {worth}");
		}
		Equivalent += worth;
	}

	public void BorrowEquivalentFromTaiwu(long worth)
	{
		if (worth <= 0)
		{
			throw new Exception($"Worth must be greater than zero: {worth}");
		}
		Equivalent -= worth;
	}

	public void LendNonequivalentToTaiwu(long worth, int count = 1)
	{
		if (worth <= 0)
		{
			throw new Exception($"Worth must be greater than zero: {worth}");
		}
		IList<long> keys = Nonequivalents.Keys;
		IList<int> values = Nonequivalents.Values;
		if (Nonequivalents.Count == 0 || Nonequivalents.Keys[0] > 0)
		{
			int num = CollectionUtils.BinarySearch(keys, 0, keys.Count, worth);
			if (num >= 0)
			{
				long key = keys[num];
				Nonequivalents[key] += count;
			}
			else
			{
				Nonequivalents.Add(worth, count);
			}
			return;
		}
		OnExchange.Clear();
		OnExchange.Enqueue((worth, count));
		while (OnExchange.Count > 0)
		{
			(long, int) tuple = OnExchange.Dequeue();
			long item = tuple.Item1;
			int item2 = tuple.Item2;
			long num2 = -item;
			int num3 = CollectionUtils.BinarySearch(keys, 0, keys.Count, num2);
			if (num3 < 0)
			{
				num3 = ~num3 - 1;
			}
			if (num3 >= 0 && num3 < Nonequivalents.Count && keys[num3] < 0)
			{
				long num4 = keys[num3];
				int num5 = values[num3];
				if (num5 <= item2)
				{
					Nonequivalents.RemoveAt(num3);
					int num6 = item2 - num5;
					if (num6 > 0)
					{
						OnExchange.Enqueue((item, num6));
					}
				}
				else
				{
					Nonequivalents[num4] = num5 - item2;
				}
				if (num4 < num2)
				{
					BorrowEquivalentFromTaiwu((num2 - num4) * Math.Min(num5, item2));
				}
			}
			else if (Nonequivalents.ContainsKey(item))
			{
				Nonequivalents[item] += item2;
			}
			else
			{
				Nonequivalents.Add(item, item2);
			}
		}
	}

	public bool BorrowNonequivalentFromTaiwu(long worth, int count = 1)
	{
		if (worth <= 0)
		{
			throw new Exception($"Worth must be greater than zero: {worth}");
		}
		IList<long> keys = Nonequivalents.Keys;
		IList<int> values = Nonequivalents.Values;
		long num = -worth;
		if (Nonequivalents.Count == 0 || Nonequivalents.Keys[Nonequivalents.Count - 1] < 0)
		{
			int num2 = CollectionUtils.BinarySearch(keys, 0, keys.Count, num);
			if (num2 >= 0)
			{
				long key = keys[num2];
				Nonequivalents[key] += count;
			}
			else
			{
				Nonequivalents.Add(num, count);
			}
			return false;
		}
		bool result = false;
		OnExchange.Clear();
		OnExchange.Enqueue((worth, count));
		while (OnExchange.Count > 0)
		{
			(long, int) tuple = OnExchange.Dequeue();
			long item = tuple.Item1;
			int item2 = tuple.Item2;
			int count2 = keys.Count;
			int num3 = CollectionUtils.BinarySearch(keys, 0, count2, item);
			if (num3 < 0)
			{
				num3 = ~num3 - 1;
			}
			if (num3 < count2 && num3 >= 0 && keys[num3] > 0)
			{
				result = true;
				long num4 = keys[num3];
				int num5 = values[num3];
				if (num5 > item2)
				{
					Nonequivalents[num4] = num5 - item2;
					if (item > num4)
					{
						OnExchange.Enqueue((item - num4, item2));
					}
					continue;
				}
				if (item > num4)
				{
					OnExchange.Enqueue((item - num4, num5));
				}
				Nonequivalents.RemoveAt(num3);
				if (item2 != num5)
				{
					OnExchange.Enqueue((item, item2 - num5));
				}
			}
			else
			{
				BorrowEquivalentFromTaiwu(item * item2);
			}
		}
		return result;
	}

	public void Clear()
	{
		Equivalent = 0L;
		Nonequivalents.Clear();
	}

	public long GetTotalWorthLentToTaiwu()
	{
		return GetEquivalentWorth() + GetTotalNonEquivalentWorth();
	}

	public long GetEquivalentWorth()
	{
		if (Equivalent < 0)
		{
			return 0L;
		}
		return Equivalent;
	}

	public long GetTotalNonEquivalentWorth()
	{
		long num = 0L;
		foreach (KeyValuePair<long, int> nonequivalent in Nonequivalents)
		{
			long key = nonequivalent.Key;
			int value = nonequivalent.Value;
			if (key >= 0)
			{
				num += key * value;
			}
		}
		return num;
	}

	public long GetFinalNonEquivalentWorth()
	{
		long num = 0L;
		foreach (KeyValuePair<long, int> nonequivalent in Nonequivalents)
		{
			long key = nonequivalent.Key;
			int value = nonequivalent.Value;
			num += key * value;
		}
		return num;
	}

	public long GetFinalWorth()
	{
		return Equivalent + GetFinalNonEquivalentWorth();
	}

	public override string ToString()
	{
		string text = Equivalent.ToString();
		if (Nonequivalents.Count <= 0)
		{
			return text;
		}
		foreach (KeyValuePair<long, int> nonequivalent in Nonequivalents)
		{
			long key = nonequivalent.Key;
			int value = nonequivalent.Value;
			text += $", ({key}, {value})";
		}
		return text;
	}

	public long GetTotalPositiveNonEquivalentWorth()
	{
		return Nonequivalents.Sum((KeyValuePair<long, int> pair) => (pair.Key > 0) ? (pair.Key * pair.Value) : 0);
	}

	public long GetTotalNegativeNonEquivalentWorth()
	{
		return Nonequivalents.Sum((KeyValuePair<long, int> pair) => (pair.Key < 0) ? (pair.Key * pair.Value) : 0);
	}

	public void CopyDataFrom(Debts otherDebts)
	{
		Equivalent = otherDebts.Equivalent;
		Nonequivalents.Clear();
		foreach (KeyValuePair<long, int> nonequivalent in otherDebts.Nonequivalents)
		{
			Nonequivalents.Add(nonequivalent.Key, nonequivalent.Value);
		}
	}
}
