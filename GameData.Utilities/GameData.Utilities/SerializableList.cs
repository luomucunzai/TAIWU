using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Utilities;

public struct SerializableList<T> : ISerializableGameData where T : ISerializableGameData, new()
{
	public List<T> Items;

	public static SerializableList<T> Create()
	{
		SerializableList<T> result = default(SerializableList<T>);
		result.Items = new List<T>();
		return result;
	}

	public SerializableList(SerializableList<T> other)
	{
		Items = ((other.Items == null) ? null : new List<T>(other.Items));
	}

	public SerializableList(IEnumerable<T> items)
	{
		Items = new List<T>();
		if (items != null)
		{
			Items.AddRange(items);
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		if (Items != null)
		{
			for (int i = 0; i < Items.Count; i++)
			{
				num += ((ISerializableGameData)Items[i]/*cast due to .constrained prefix*/).GetSerializedSize();
			}
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Items != null)
		{
			int count = Items.Count;
			Tester.Assert(count <= 65535);
			*(int*)ptr = count;
			ptr += 4;
			for (int i = 0; i < count; i++)
			{
				ptr += ((ISerializableGameData)Items[i]/*cast due to .constrained prefix*/).Serialize(ptr);
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
		if (num > 0)
		{
			if (Items == null)
			{
				Items = new List<T>(num);
			}
			else
			{
				Items.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				T item = new T();
				ptr += ((ISerializableGameData)item/*cast due to .constrained prefix*/).Deserialize(ptr);
				Items.Add(item);
			}
		}
		else
		{
			Items?.Clear();
		}
		return (int)(ptr - pData);
	}
}
