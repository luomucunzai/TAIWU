using GameData.Domains.Character.Display;
using GameData.Serializer;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

public struct TaiwuEventCharacterNameDecodeData : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharacterId;

	[SerializableGameDataField]
	public NameRelatedData NameRelatedData;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 36;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharacterId;
		ptr += 4;
		ptr += NameRelatedData.Serialize(ptr);
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
		CharacterId = *(int*)ptr;
		ptr += 4;
		ptr += NameRelatedData.Deserialize(ptr);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
