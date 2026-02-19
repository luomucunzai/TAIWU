using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Map;

[SerializableGameData(IsExtensible = true)]
public class CricketPlaceExtraData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort ExtraMapUnits = 0;

		public const ushort RegularCrickets = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "ExtraMapUnits", "RegularCrickets" };
	}

	[SerializableGameDataField]
	public Dictionary<short, short> ExtraMapUnits;

	[SerializableGameDataField]
	public List<short> RegularCrickets;

	public bool IsRegularCricket(short blockId)
	{
		return RegularCrickets?.Contains(blockId) ?? false;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, short>((IReadOnlyDictionary<short, short>)ExtraMapUnits);
		num = ((RegularCrickets == null) ? (num + 2) : (num + (2 + 2 * RegularCrickets.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 2;
		ptr += 2;
		ptr += DictionaryOfBasicTypePair.Serialize<short, short>(ptr, ref ExtraMapUnits);
		if (RegularCrickets != null)
		{
			int count = RegularCrickets.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = RegularCrickets[i];
			}
			ptr += 2 * count;
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
			ptr += DictionaryOfBasicTypePair.Deserialize<short, short>(ptr, ref ExtraMapUnits);
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (RegularCrickets == null)
				{
					RegularCrickets = new List<short>(num2);
				}
				else
				{
					RegularCrickets.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					RegularCrickets.Add(((short*)ptr)[i]);
				}
				ptr += 2 * num2;
			}
			else
			{
				RegularCrickets?.Clear();
			}
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
