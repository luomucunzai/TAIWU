using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.DLC.FiveLoong;

public class JiaoPoolRecordList : ISerializableGameData
{
	[SerializableGameDataField]
	public List<JiaoPoolRecord> Collection = new List<JiaoPoolRecord>();

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		if (Collection != null)
		{
			num += 2;
			int count = Collection.Count;
			for (int i = 0; i < count; i++)
			{
				JiaoPoolRecord jiaoPoolRecord = Collection[i];
				num = ((jiaoPoolRecord == null) ? (num + 2) : (num + (2 + jiaoPoolRecord.GetSerializedSize())));
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
		if (Collection != null)
		{
			int count = Collection.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				JiaoPoolRecord jiaoPoolRecord = Collection[i];
				if (jiaoPoolRecord != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = jiaoPoolRecord.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)intPtr = (ushort)num;
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
			if (Collection == null)
			{
				Collection = new List<JiaoPoolRecord>(num);
			}
			else
			{
				Collection.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					JiaoPoolRecord jiaoPoolRecord = new JiaoPoolRecord();
					ptr += jiaoPoolRecord.Deserialize(ptr);
					Collection.Add(jiaoPoolRecord);
				}
				else
				{
					Collection.Add(null);
				}
			}
		}
		else
		{
			Collection?.Clear();
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
