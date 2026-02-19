using GameData.Serializer;

namespace GameData.Domains.Item.Display;

[SerializableGameData(NotForArchive = true)]
public struct ItemPowerInfo : ISerializableGameData
{
	[SerializableGameDataField]
	public short Power;

	[SerializableGameDataField]
	public short MaxPower;

	[SerializableGameDataField]
	public short RequirementsPower;

	public static ItemPowerInfo Default => new ItemPowerInfo
	{
		Power = 100,
		MaxPower = 100,
		RequirementsPower = 100
	};

	public bool AnyValue
	{
		get
		{
			if (Power <= 0 && MaxPower <= 0)
			{
				return RequirementsPower > 0;
			}
			return true;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = Power;
		byte* num = pData + 2;
		*(short*)num = MaxPower;
		byte* num2 = num + 2;
		*(short*)num2 = RequirementsPower;
		int num3 = (int)(num2 + 2 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Power = *(short*)ptr;
		ptr += 2;
		MaxPower = *(short*)ptr;
		ptr += 2;
		RequirementsPower = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
