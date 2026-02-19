using System.Collections;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Taiwu;

[AutoGenerateSerializableGameData]
public class SkillBreakPlateBonusList : IList<SkillBreakPlateBonus>, ICollection<SkillBreakPlateBonus>, IEnumerable<SkillBreakPlateBonus>, IEnumerable, ISerializableGameData
{
	[SerializableGameDataField]
	private List<SkillBreakPlateBonus> _list = new List<SkillBreakPlateBonus>();

	private IList<SkillBreakPlateBonus> ListImplementation => _list ?? (_list = new List<SkillBreakPlateBonus>());

	public int Count => ListImplementation.Count;

	public bool IsReadOnly => ListImplementation.IsReadOnly;

	public SkillBreakPlateBonus this[int index]
	{
		get
		{
			return ListImplementation[index];
		}
		set
		{
			ListImplementation[index] = value;
		}
	}

	public SkillBreakPlateBonusList(IEnumerable<SkillBreakPlateBonus> collection)
	{
		if (_list == null)
		{
			_list = new List<SkillBreakPlateBonus>();
		}
		_list.Clear();
		_list.AddRange(collection);
	}

	public IEnumerator<SkillBreakPlateBonus> GetEnumerator()
	{
		return ListImplementation.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)ListImplementation).GetEnumerator();
	}

	public void Add(SkillBreakPlateBonus item)
	{
		ListImplementation.Add(item);
	}

	public void Clear()
	{
		ListImplementation.Clear();
	}

	public bool Contains(SkillBreakPlateBonus item)
	{
		return ListImplementation.Contains(item);
	}

	public void CopyTo(SkillBreakPlateBonus[] array, int arrayIndex)
	{
		ListImplementation.CopyTo(array, arrayIndex);
	}

	public bool Remove(SkillBreakPlateBonus item)
	{
		return ListImplementation.Remove(item);
	}

	public int IndexOf(SkillBreakPlateBonus item)
	{
		return ListImplementation.IndexOf(item);
	}

	public void Insert(int index, SkillBreakPlateBonus item)
	{
		ListImplementation.Insert(index, item);
	}

	public void RemoveAt(int index)
	{
		ListImplementation.RemoveAt(index);
	}

	public SkillBreakPlateBonusList()
	{
	}

	public SkillBreakPlateBonusList(SkillBreakPlateBonusList other)
	{
		_list = ((other._list == null) ? null : new List<SkillBreakPlateBonus>(other._list));
	}

	public void Assign(SkillBreakPlateBonusList other)
	{
		_list = ((other._list == null) ? null : new List<SkillBreakPlateBonus>(other._list));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		if (_list != null)
		{
			num += 2;
			for (int i = 0; i < _list.Count; i++)
			{
				num += _list[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
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
		if (_list != null)
		{
			int count = _list.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				int num = _list[i].Serialize(ptr);
				ptr += num;
				Tester.Assert(num <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (_list == null)
			{
				_list = new List<SkillBreakPlateBonus>();
			}
			else
			{
				_list.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				SkillBreakPlateBonus item = default(SkillBreakPlateBonus);
				ptr += item.Deserialize(ptr);
				_list.Add(item);
			}
		}
		else
		{
			_list?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
