using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Adventure;

[AutoGenerateSerializableGameData(IsExtensible = true)]
public class AdventureRemake : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort Elements = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "Elements" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	private List<AdventureRemakeElement> _elements;

	public AdventureRemake()
	{
	}

	public AdventureRemake(AdventureRemake other)
	{
		if (other._elements != null)
		{
			List<AdventureRemakeElement> elements = other._elements;
			int count = elements.Count;
			_elements = new List<AdventureRemakeElement>(count);
			{
				foreach (AdventureRemakeElement item in elements)
				{
					_elements.Add(new AdventureRemakeElement(item));
				}
				return;
			}
		}
		_elements = null;
	}

	public void Assign(AdventureRemake other)
	{
		if (other._elements != null)
		{
			List<AdventureRemakeElement> elements = other._elements;
			int count = elements.Count;
			_elements = new List<AdventureRemakeElement>(count);
			{
				foreach (AdventureRemakeElement item in elements)
				{
					_elements.Add(new AdventureRemakeElement(item));
				}
				return;
			}
		}
		_elements = null;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		if (_elements != null)
		{
			num += 2;
			for (int i = 0; i < _elements.Count; i++)
			{
				num = ((_elements[i] == null) ? (num + 2) : (num + (2 + _elements[i].GetSerializedSize())));
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
		*(short*)ptr = 1;
		ptr += 2;
		if (_elements != null)
		{
			int count = _elements.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				if (_elements[i] != null)
				{
					byte* ptr2 = ptr;
					ptr += 2;
					int num = _elements[i].Serialize(ptr);
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
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (_elements == null)
				{
					_elements = new List<AdventureRemakeElement>();
				}
				else
				{
					_elements.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)ptr;
					ptr += 2;
					AdventureRemakeElement adventureRemakeElement;
					if (num3 > 0)
					{
						adventureRemakeElement = new AdventureRemakeElement();
						ptr += adventureRemakeElement.Deserialize(ptr);
					}
					else
					{
						adventureRemakeElement = null;
					}
					_elements.Add(adventureRemakeElement);
				}
			}
			else
			{
				_elements?.Clear();
			}
		}
		int num4 = (int)(ptr - pData);
		return (num4 <= 4) ? num4 : ((num4 + 3) / 4 * 4);
	}
}
