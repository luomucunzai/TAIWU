using GameData.Serializer;

namespace GameData.Domains.Extra;

public struct WeaveClothingDisplaySetting : ISerializableGameData
{
	[SerializableGameDataField]
	public byte ClothingDisplayOriginSettingGender;

	[SerializableGameDataField]
	public byte ClothingDisplayOriginSettingBodyType;

	[SerializableGameDataField]
	public byte ClothingDisplayPreviewSettingGender;

	[SerializableGameDataField]
	public byte ClothingDisplayPreviewSettingBodyType;

	public void Init()
	{
		ClothingDisplayOriginSettingGender = 0;
		ClothingDisplayOriginSettingBodyType = 2;
		ClothingDisplayPreviewSettingGender = 0;
		ClothingDisplayPreviewSettingBodyType = 2;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = ClothingDisplayOriginSettingGender;
		byte* num = pData + 1;
		*num = ClothingDisplayOriginSettingBodyType;
		byte* num2 = num + 1;
		*num2 = ClothingDisplayPreviewSettingGender;
		byte* num3 = num2 + 1;
		*num3 = ClothingDisplayPreviewSettingBodyType;
		int num4 = (int)(num3 + 1 - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ClothingDisplayOriginSettingGender = *ptr;
		ptr++;
		ClothingDisplayOriginSettingBodyType = *ptr;
		ptr++;
		ClothingDisplayPreviewSettingGender = *ptr;
		ptr++;
		ClothingDisplayPreviewSettingBodyType = *ptr;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
