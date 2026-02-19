using GameData.Serializer;

namespace GameData.Domains.Merchant;

[SerializableGameData(NoCopyConstructors = true)]
public class MerchantInfoAreaData : ISerializableGameData
{
	[SerializableGameDataField]
	public short AreaTemplateId;

	[SerializableGameDataField]
	public int CaravanCount;

	[SerializableGameDataField]
	public int MerchantCount;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = AreaTemplateId;
		byte* num = pData + 2;
		*(int*)num = CaravanCount;
		byte* num2 = num + 4;
		*(int*)num2 = MerchantCount;
		int num3 = (int)(num2 + 4 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		AreaTemplateId = *(short*)ptr;
		ptr += 2;
		CaravanCount = *(int*)ptr;
		ptr += 4;
		MerchantCount = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
