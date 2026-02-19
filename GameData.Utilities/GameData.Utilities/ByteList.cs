using System.Collections;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Utilities;

[SerializableGameData]
public class ByteList : IList<byte>, ICollection<byte>, IEnumerable<byte>, IEnumerable, IReadOnlyList<byte>, IReadOnlyCollection<byte>, ISerializableGameData
{
	[SerializableGameDataField]
	private List<byte> _implementList = new List<byte>();

	private IList<byte> Implement => _implementList;

	public int Count => Implement.Count;

	public bool IsReadOnly => Implement.IsReadOnly;

	public byte this[int index]
	{
		get
		{
			return Implement[index];
		}
		set
		{
			Implement[index] = value;
		}
	}

	public IEnumerator<byte> GetEnumerator()
	{
		return Implement.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)Implement).GetEnumerator();
	}

	public void Add(byte item)
	{
		Implement.Add(item);
	}

	public void Clear()
	{
		Implement.Clear();
	}

	public bool Contains(byte item)
	{
		return Implement.Contains(item);
	}

	public void CopyTo(byte[] array, int arrayIndex)
	{
		Implement.CopyTo(array, arrayIndex);
	}

	public bool Remove(byte item)
	{
		return Implement.Remove(item);
	}

	public int IndexOf(byte item)
	{
		return Implement.IndexOf(item);
	}

	public void Insert(int index, byte item)
	{
		Implement.Insert(index, item);
	}

	public void RemoveAt(int index)
	{
		Implement.RemoveAt(index);
	}

	public void AddRange(IEnumerable<byte> items)
	{
		_implementList.AddRange(items);
	}

	public ByteList()
	{
	}

	public ByteList(ByteList other)
	{
		_implementList = ((other._implementList == null) ? null : new List<byte>(other._implementList));
	}

	public void Assign(ByteList other)
	{
		_implementList = ((other._implementList == null) ? null : new List<byte>(other._implementList));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((_implementList == null) ? (num + 2) : (num + (2 + _implementList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (_implementList != null)
		{
			int count = _implementList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr[i] = _implementList[i];
			}
			ptr += count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (_implementList == null)
			{
				_implementList = new List<byte>(num);
			}
			else
			{
				_implementList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				_implementList.Add(ptr[i]);
			}
			ptr += (int)num;
		}
		else
		{
			_implementList?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
