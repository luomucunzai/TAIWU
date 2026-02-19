using GameData.Serializer;

namespace GameData.Domains.Character.Display;

public class CharacterInfoCountData : ISerializableGameData
{
	[SerializableGameDataField]
	public int HoldInfoCount;

	[SerializableGameDataField]
	public int HoldInfoTaiwuDontHoldCount;

	[SerializableGameDataField]
	public int HoldInfoTaiwuRelatedCount;

	public CharacterInfoCountData()
	{
	}

	public CharacterInfoCountData(CharacterInfoCountData other)
	{
		HoldInfoCount = other.HoldInfoCount;
		HoldInfoTaiwuDontHoldCount = other.HoldInfoTaiwuDontHoldCount;
		HoldInfoTaiwuRelatedCount = other.HoldInfoTaiwuRelatedCount;
	}

	public void Assign(CharacterInfoCountData other)
	{
		HoldInfoCount = other.HoldInfoCount;
		HoldInfoTaiwuDontHoldCount = other.HoldInfoTaiwuDontHoldCount;
		HoldInfoTaiwuRelatedCount = other.HoldInfoTaiwuRelatedCount;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = HoldInfoCount;
		byte* num = pData + 4;
		*(int*)num = HoldInfoTaiwuDontHoldCount;
		byte* num2 = num + 4;
		*(int*)num2 = HoldInfoTaiwuRelatedCount;
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
		HoldInfoCount = *(int*)ptr;
		ptr += 4;
		HoldInfoTaiwuDontHoldCount = *(int*)ptr;
		ptr += 4;
		HoldInfoTaiwuRelatedCount = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
