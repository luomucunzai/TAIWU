using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.LifeRecord;

public class ReadonlyLifeRecordsWithDate : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharId;

	[SerializableGameDataField]
	public int StartDate;

	[SerializableGameDataField]
	public int MonthCount;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	public ReadonlyLifeRecords Records;

	public ReadonlyLifeRecordsWithDate()
	{
		Records = new ReadonlyLifeRecords();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		num = ((Records == null) ? (num + 4) : (num + (4 + Records.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharId;
		ptr += 4;
		*(int*)ptr = StartDate;
		ptr += 4;
		*(int*)ptr = MonthCount;
		ptr += 4;
		if (Records != null)
		{
			byte* intPtr = ptr;
			ptr += 4;
			int num = Records.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= int.MaxValue);
			*(int*)intPtr = num;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
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
		CharId = *(int*)ptr;
		ptr += 4;
		StartDate = *(int*)ptr;
		ptr += 4;
		MonthCount = *(int*)ptr;
		ptr += 4;
		int num = *(int*)ptr;
		ptr += 4;
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
