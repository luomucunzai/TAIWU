using System.Collections;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Taiwu;

[AutoGenerateSerializableGameData(NotForDisplayModule = true)]
public class SkillBreakPlateList : IList<SkillBreakPlate>, ICollection<SkillBreakPlate>, IEnumerable<SkillBreakPlate>, IEnumerable, ISerializableGameData
{
	[SerializableGameDataField]
	private List<SkillBreakPlate> _list = new List<SkillBreakPlate>();

	private IList<SkillBreakPlate> ListImplementation => _list ?? (_list = new List<SkillBreakPlate>());

	public int Count => ListImplementation.Count;

	public bool IsReadOnly => ListImplementation.IsReadOnly;

	public SkillBreakPlate this[int index]
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

	public IEnumerator<SkillBreakPlate> GetEnumerator()
	{
		return ListImplementation.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)ListImplementation).GetEnumerator();
	}

	public void Add(SkillBreakPlate item)
	{
		ListImplementation.Add(item);
	}

	public void Clear()
	{
		ListImplementation.Clear();
	}

	public bool Contains(SkillBreakPlate item)
	{
		return ListImplementation.Contains(item);
	}

	public void CopyTo(SkillBreakPlate[] array, int arrayIndex)
	{
		ListImplementation.CopyTo(array, arrayIndex);
	}

	public bool Remove(SkillBreakPlate item)
	{
		return ListImplementation.Remove(item);
	}

	public int IndexOf(SkillBreakPlate item)
	{
		return ListImplementation.IndexOf(item);
	}

	public void Insert(int index, SkillBreakPlate item)
	{
		ListImplementation.Insert(index, item);
	}

	public void RemoveAt(int index)
	{
		ListImplementation.RemoveAt(index);
	}

	public SkillBreakPlateList()
	{
	}

	public SkillBreakPlateList(SkillBreakPlateList other)
	{
		if (other._list != null)
		{
			List<SkillBreakPlate> list = other._list;
			int count = list.Count;
			_list = new List<SkillBreakPlate>(count);
			{
				foreach (SkillBreakPlate item in list)
				{
					_list.Add(new SkillBreakPlate(item));
				}
				return;
			}
		}
		_list = null;
	}

	public void Assign(SkillBreakPlateList other)
	{
		if (other._list != null)
		{
			List<SkillBreakPlate> list = other._list;
			int count = list.Count;
			_list = new List<SkillBreakPlate>(count);
			{
				foreach (SkillBreakPlate item in list)
				{
					_list.Add(new SkillBreakPlate(item));
				}
				return;
			}
		}
		_list = null;
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
				num = ((_list[i] == null) ? (num + 2) : (num + (2 + _list[i].GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
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
				if (_list[i] != null)
				{
					byte* ptr2 = ptr;
					ptr += 2;
					int num = _list[i].Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)ptr2 = (ushort)num;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
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
				_list = new List<SkillBreakPlate>();
			}
			else
			{
				_list.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				SkillBreakPlate skillBreakPlate;
				if (num2 > 0)
				{
					skillBreakPlate = new SkillBreakPlate();
					ptr += skillBreakPlate.Deserialize(ptr);
				}
				else
				{
					skillBreakPlate = null;
				}
				_list.Add(skillBreakPlate);
			}
		}
		else
		{
			_list?.Clear();
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
