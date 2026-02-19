using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class SelectApprovedTaiwu : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<int, short> CharacterApprovingRate;

	[SerializableGameDataField]
	public List<int> DukeTitleCharIdList;

	[SerializableGameDataField]
	public short TargetApprovingRate;

	public SelectApprovedTaiwu()
	{
		CharacterApprovingRate = new Dictionary<int, short>();
		DukeTitleCharIdList = new List<int>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, short>((IReadOnlyDictionary<int, short>)CharacterApprovingRate);
		num = ((DukeTitleCharIdList == null) ? (num + 2) : (num + (2 + 4 * DukeTitleCharIdList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfBasicTypePair.Serialize<int, short>(ptr, ref CharacterApprovingRate);
		if (DukeTitleCharIdList != null)
		{
			int count = DukeTitleCharIdList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = DukeTitleCharIdList[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = TargetApprovingRate;
		ptr += 2;
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
		ptr += DictionaryOfBasicTypePair.Deserialize<int, short>(ptr, ref CharacterApprovingRate);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (DukeTitleCharIdList == null)
			{
				DukeTitleCharIdList = new List<int>(num);
			}
			else
			{
				DukeTitleCharIdList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				DukeTitleCharIdList.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			DukeTitleCharIdList?.Clear();
		}
		TargetApprovingRate = *(short*)ptr;
		ptr += 2;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
