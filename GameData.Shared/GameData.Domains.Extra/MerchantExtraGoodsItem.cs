using GameData.Serializer;

namespace GameData.Domains.Extra;

[SerializableGameData]
public struct MerchantExtraGoodsItem : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte Index;

	[SerializableGameDataField]
	public int Id;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 5;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)Index;
		byte* num = pData + 1;
		*(int*)num = Id;
		int num2 = (int)(num + 4 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Index = (sbyte)(*ptr);
		ptr++;
		Id = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
