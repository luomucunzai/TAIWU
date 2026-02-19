using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class EventSelectFuyuFaithCountData : ISerializableGameData
{
	[SerializableGameDataField]
	public DarkAshCounter Counter;

	[SerializableGameDataField]
	public int Curr;

	[SerializableGameDataField]
	public int Max;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 8;
		num += Counter.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		int num = Counter.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		*(int*)ptr = Curr;
		ptr += 4;
		*(int*)ptr = Max;
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
		ptr += Counter.Deserialize(ptr);
		Curr = *(int*)ptr;
		ptr += 4;
		Max = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
