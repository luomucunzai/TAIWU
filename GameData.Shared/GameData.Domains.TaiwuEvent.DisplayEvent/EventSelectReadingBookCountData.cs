using GameData.Domains.Item.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NoCopyConstructors = true)]
public class EventSelectReadingBookCountData : ISerializableGameData
{
	[SerializableGameDataField]
	public ItemDisplayData SelectedBookData;

	[SerializableGameDataField]
	public int MaxReadingBookCount;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((SelectedBookData == null) ? (num + 2) : (num + (2 + SelectedBookData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (SelectedBookData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = SelectedBookData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = MaxReadingBookCount;
		ptr += 4;
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
			if (SelectedBookData == null)
			{
				SelectedBookData = new ItemDisplayData();
			}
			ptr += SelectedBookData.Deserialize(ptr);
		}
		else
		{
			SelectedBookData = null;
		}
		MaxReadingBookCount = *(int*)ptr;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
