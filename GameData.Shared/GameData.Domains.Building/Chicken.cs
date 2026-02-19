using GameData.Serializer;

namespace GameData.Domains.Building;

public struct Chicken : ISerializableGameData
{
	public const sbyte HappinessMin = 0;

	public const sbyte HappinessMax = 100;

	public const sbyte AutoFeedHappiness = 50;

	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public int CurrentSettlementId;

	[SerializableGameDataField]
	public sbyte Happiness;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 11;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = Id;
		byte* num = pData + 4;
		*(short*)num = TemplateId;
		byte* num2 = num + 2;
		*(int*)num2 = CurrentSettlementId;
		byte* num3 = num2 + 4;
		*num3 = (byte)Happiness;
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
		Id = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		CurrentSettlementId = *(int*)ptr;
		ptr += 4;
		Happiness = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
