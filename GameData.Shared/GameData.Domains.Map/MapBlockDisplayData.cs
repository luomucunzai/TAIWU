using GameData.Domains.Extra;
using GameData.Serializer;

namespace GameData.Domains.Map;

[SerializableGameData(NotForArchive = true)]
public struct MapBlockDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public TreasureExpectResult TreasureExpect;

	[SerializableGameDataField]
	public int ProfessionId;

	[SerializableGameDataField]
	public int Count0;

	[SerializableGameDataField]
	public int Count1;

	[SerializableGameDataField]
	public int Count2;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 28;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += TreasureExpect.Serialize(ptr);
		*(int*)ptr = ProfessionId;
		ptr += 4;
		*(int*)ptr = Count0;
		ptr += 4;
		*(int*)ptr = Count1;
		ptr += 4;
		*(int*)ptr = Count2;
		ptr += 4;
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
		ptr += TreasureExpect.Deserialize(ptr);
		ProfessionId = *(int*)ptr;
		ptr += 4;
		Count0 = *(int*)ptr;
		ptr += 4;
		Count1 = *(int*)ptr;
		ptr += 4;
		Count2 = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
