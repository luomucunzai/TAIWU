using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.LifeRecord;

public class ReadonlyLifeRecordsWithTotalCount : ISerializableGameData
{
	[SerializableGameDataField]
	public int TotalCount;

	[SerializableGameDataField]
	public ReadonlyLifeRecords Records;

	public ReadonlyLifeRecordsWithTotalCount()
	{
		Records = new ReadonlyLifeRecords();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((Records == null) ? (num + 2) : (num + (2 + Records.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = TotalCount;
		ptr += 4;
		if (Records != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = Records.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
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
		TotalCount = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Records == null)
			{
				Records = new ReadonlyLifeRecords();
			}
			ptr += Records.Deserialize(ptr);
		}
		else
		{
			Records = null;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
