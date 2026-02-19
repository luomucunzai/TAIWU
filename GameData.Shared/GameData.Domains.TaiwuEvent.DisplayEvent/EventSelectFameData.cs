using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class EventSelectFameData : ISerializableGameData
{
	[SerializableGameDataField]
	public List<FameActionRecord> fameActionRecords;

	public EventSelectFameData()
	{
	}

	public EventSelectFameData(EventSelectFameData other)
	{
		fameActionRecords = ((other.fameActionRecords == null) ? null : new List<FameActionRecord>(other.fameActionRecords));
	}

	public void Assign(EventSelectFameData other)
	{
		fameActionRecords = ((other.fameActionRecords == null) ? null : new List<FameActionRecord>(other.fameActionRecords));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((fameActionRecords == null) ? (num + 2) : (num + (2 + 8 * fameActionRecords.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (fameActionRecords != null)
		{
			int count = fameActionRecords.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += fameActionRecords[i].Serialize(ptr);
			}
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
			if (fameActionRecords == null)
			{
				fameActionRecords = new List<FameActionRecord>(num);
			}
			else
			{
				fameActionRecords.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				FameActionRecord item = default(FameActionRecord);
				ptr += item.Deserialize(ptr);
				fameActionRecords.Add(item);
			}
		}
		else
		{
			fameActionRecords?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
