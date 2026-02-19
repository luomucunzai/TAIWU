using System.Collections.Generic;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.LegendaryBook;

public class LegendaryBookIncrementData : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<sbyte, MapBlockData> BlockDataMap;

	[SerializableGameDataField]
	public Dictionary<sbyte, FullBlockName> BlockNameDataMap;

	[SerializableGameDataField]
	public Dictionary<sbyte, Location> BookLocationMap;

	[SerializableGameDataField]
	public Dictionary<sbyte, int> BookDurationMap;

	[SerializableGameDataField]
	public Dictionary<int, LegendaryBookCharacterRelatedData> CharacterMap;

	[SerializableGameDataField]
	public Dictionary<sbyte, int> OwnerMap;

	[SerializableGameDataField]
	public List<int> ContestList;

	[SerializableGameDataField]
	public List<int> ShockedList;

	[SerializableGameDataField]
	public List<int> InsaneList;

	[SerializableGameDataField]
	public List<int> ConsumedList;

	public LegendaryBookIncrementData()
	{
		BlockDataMap = new Dictionary<sbyte, MapBlockData>();
		BlockNameDataMap = new Dictionary<sbyte, FullBlockName>();
		BookLocationMap = new Dictionary<sbyte, Location>();
		BookDurationMap = new Dictionary<sbyte, int>();
		CharacterMap = new Dictionary<int, LegendaryBookCharacterRelatedData>();
		OwnerMap = new Dictionary<sbyte, int>();
		ContestList = new List<int>();
		ShockedList = new List<int>();
		InsaneList = new List<int>();
		ConsumedList = new List<int>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<sbyte, MapBlockData>(BlockDataMap);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<sbyte, FullBlockName>(BlockNameDataMap);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<sbyte, Location>(BookLocationMap);
		num += DictionaryOfBasicTypePair.GetSerializedSize<sbyte, int>((IReadOnlyDictionary<sbyte, int>)BookDurationMap);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, LegendaryBookCharacterRelatedData>(CharacterMap);
		num += DictionaryOfBasicTypePair.GetSerializedSize<sbyte, int>((IReadOnlyDictionary<sbyte, int>)OwnerMap);
		num = ((ContestList == null) ? (num + 2) : (num + (2 + 4 * ContestList.Count)));
		num = ((ShockedList == null) ? (num + 2) : (num + (2 + 4 * ShockedList.Count)));
		num = ((InsaneList == null) ? (num + 2) : (num + (2 + 4 * InsaneList.Count)));
		num = ((ConsumedList == null) ? (num + 2) : (num + (2 + 4 * ConsumedList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<sbyte, MapBlockData>(ptr, ref BlockDataMap);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<sbyte, FullBlockName>(ptr, ref BlockNameDataMap);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<sbyte, Location>(ptr, ref BookLocationMap);
		ptr += DictionaryOfBasicTypePair.Serialize<sbyte, int>(ptr, ref BookDurationMap);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, LegendaryBookCharacterRelatedData>(ptr, ref CharacterMap);
		ptr += DictionaryOfBasicTypePair.Serialize<sbyte, int>(ptr, ref OwnerMap);
		if (ContestList != null)
		{
			int count = ContestList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = ContestList[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ShockedList != null)
		{
			int count2 = ShockedList.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((int*)ptr)[j] = ShockedList[j];
			}
			ptr += 4 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (InsaneList != null)
		{
			int count3 = InsaneList.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				((int*)ptr)[k] = InsaneList[k];
			}
			ptr += 4 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ConsumedList != null)
		{
			int count4 = ConsumedList.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int l = 0; l < count4; l++)
			{
				((int*)ptr)[l] = ConsumedList[l];
			}
			ptr += 4 * count4;
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
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<sbyte, MapBlockData>(ptr, ref BlockDataMap);
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<sbyte, FullBlockName>(ptr, ref BlockNameDataMap);
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<sbyte, Location>(ptr, ref BookLocationMap);
		ptr += DictionaryOfBasicTypePair.Deserialize<sbyte, int>(ptr, ref BookDurationMap);
		ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, LegendaryBookCharacterRelatedData>(ptr, ref CharacterMap);
		ptr += DictionaryOfBasicTypePair.Deserialize<sbyte, int>(ptr, ref OwnerMap);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ContestList == null)
			{
				ContestList = new List<int>(num);
			}
			else
			{
				ContestList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ContestList.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			ContestList?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (ShockedList == null)
			{
				ShockedList = new List<int>(num2);
			}
			else
			{
				ShockedList.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				ShockedList.Add(((int*)ptr)[j]);
			}
			ptr += 4 * num2;
		}
		else
		{
			ShockedList?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (InsaneList == null)
			{
				InsaneList = new List<int>(num3);
			}
			else
			{
				InsaneList.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				InsaneList.Add(((int*)ptr)[k]);
			}
			ptr += 4 * num3;
		}
		else
		{
			InsaneList?.Clear();
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (ConsumedList == null)
			{
				ConsumedList = new List<int>(num4);
			}
			else
			{
				ConsumedList.Clear();
			}
			for (int l = 0; l < num4; l++)
			{
				ConsumedList.Add(((int*)ptr)[l]);
			}
			ptr += 4 * num4;
		}
		else
		{
			ConsumedList?.Clear();
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
